using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using SubSonic;
using PowerPOS.Container;
using System.Transactions;
using PowerPOS.Controller;
using System.Linq;

namespace PowerPOS
{
    public partial class POSController
    {
        /*
        public static void GetLastOpeningInfo(out string cashierName, out decimal amount, out DateTime closingtime, string PointOfSaleName)
        {

            cashierName = "-";
            amount = 0;
            closingtime = DateTime.MinValue;
            try
            {
                int PointOfSaleID = (new PointOfSale("PointOfSaleName", PointOfSaleName)).PointOfSaleID;
                //PowerPOS.PointOfSaleController.SavePointOfSaleInfo(PointOfSaleName);
                PowerPOS.PointOfSaleController.GetPointOfSaleInfo();
                Query qr = new Query("CashRecording");

                qr.Top = "1";
                qr.OrderBy = OrderBy.Desc("CashRecordingTime");
                qr.QueryType = QueryType.Select;
                qr.AddWhere("CashRecordingTime", Comparison.LessOrEquals, DateTime.Now);
                qr.AddWhere("PointOfSaleID", PointOfSaleID);
                qr.AddWhere("CashRecordingTypeID", 3);
                DataSet ds = qr.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    amount = decimal.Parse(ds.Tables[0].Rows[0]["amount"].ToString());
                    closingtime = DateTime.Parse(ds.Tables[0].Rows[0]["CashRecordingTime"].ToString());
                    cashierName = ds.Tables[0].Rows[0]["CashierName"].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
        */
        public void AssignDefaultSalesPerson(string SalesPersonID)
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].Item.IsCommission.HasValue &&
                    myOrderDet[i].Item.IsCommission.Value &&
                    (myOrderDet[i].Userfld1 == "" || myOrderDet[i].Userfld1 == null))
                {
                    myOrderDet[i].Userfld1 = SalesPersonID;
                }
            }
        }

        //Apply discount to all item in the receipt
        public bool applyDiscount(decimal disc)
        {
            string status;
            //apply discounts to Non-Promo items
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!ChangeOrderLineDiscount
                    (myOrderDet[i].OrderDetID, disc, false, false, out status))
                {
                    throw new Exception("status");
                }
            }
            SetPreferredDiscount(disc);
            return true;
        }

        public bool clearDiscount(decimal disc)
        {
            string status;
            //apply discounts to Non-Promo items
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!myOrderDet[i].IsPromo)
                {
                    myOrderDet[i].Discount2Percent = 0;
                    myOrderDet[i].Discount2Dollar = 0;
                    if (!ChangeOrderLineDiscount
                        (myOrderDet[i].OrderDetID, disc, true, true, out status))
                    {
                        throw new Exception("status");
                    }
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), false))
                        myOrderDet[i].Remark = "";
                    myOrderDet[i].SpecialDiscount = null;
                    myOrderDet[i].DiscountDetail = null;
                }
            }
            myOrderHdr.Userfld7 = "";
            //SetPreferredDiscount(disc);
            return true;
        }

        public bool applyBankDiscount(decimal BankDiscAmount, string discName)
        {
            /*string status;
            //apply discounts to Non-Promo items

            decimal TotalAmount1 = this.CalculateTotalAmount(out status);
            if (BankDiscAmount > TotalAmount1) { BankDiscAmount = TotalAmount1; }
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!ChangeOrderLineDiscount
                    (myOrderDet[i].OrderDetID, BankDiscAmount, TotalAmount1, out status))
                {
                    throw new Exception("status");
                }
                if (myOrderDet[i].Userfld7 != null && myOrderDet[i].Userfld7 != "")
                {
                    myOrderDet[i].Userfld7 = myOrderDet[i].Userfld7 + "," + discName;
                }
                else
                {
                    myOrderDet[i].Userfld7 = discName;
                }
                if (myOrderHdr.Userfld7 != null && myOrderHdr.Userfld7 != "")
                {
                    //if (!myOrderHdr.Userfld7.Contains(discName))
                    //{
                    myOrderHdr.Userfld7 = myOrderDet[i].Userfld7;
                    //}
                }
                else
                {
                    myOrderHdr.Userfld7 = discName;
                }
            }
            //SetPreferredDiscount(disc);*/
            return true;
        }

        //Apply discount to all item in the receipt
        public bool applyDiscount(string discName)
        {
            return applyDiscount(discName, true);
        }

        //Apply discount to all item in the receipt with overwrite existing or not.
        public bool applyDiscount(string discName, bool overwriteExisting)
        {
            //string status;


            //apply discounts to Non-Promo items
            SpecialDiscount sd = new SpecialDiscount("DiscountName", discName);
            decimal DiscountPercent = sd.DiscountPercentage;
            bool useSPP = sd.UseSPP.GetValueOrDefault(false);
            //Decimal DiscountPrice;
            //Decimal RetailPrice;
            if ((bool)sd.Enabled)
            {
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    OrderDet od = myOrderDet[i];
                    if (!ApplyDiscountOrderDet(sd.DiscountName, ref od, overwriteExisting))
                    {
                        return false;
                    }
                }
                promoCtrl.UndoPromoToOrder();
                promoCtrl.ApplyPromoToOrder();

                return true;
            }
            else
            {
                return true;
            }
        }

        public bool ApplyDiscountOrderDet(string discName, ref OrderDet od)
        {
            if (od.UnitPrice == 0)
            {
                return false;
            }
            else
            {
                bool result = ApplyDiscountOrderDet(discName, ref od, true);
                promoCtrl.UndoPromoToOrder();
                promoCtrl.ApplyPromoToOrder();
                return result;
            }
        }

        public bool ApplyDiscountOrderDet(bool applyPromo, string discName, ref OrderDet od)
        {
            if (od.UnitPrice == 0)
            {
                return false;
            }
            else
            {
                bool result = ApplyDiscountOrderDet(discName, ref od, true);
                return result;
            }
        }

        public bool ApplyDiscountOrderDet(string discName, ref OrderDet od, bool overwriteExisting)
        {
            string status;
            //apply discounts to Non-Promo items
            SpecialDiscount sd = new SpecialDiscount("DiscountName", discName);

            if (sd != null && sd.DiscountName != "")
            {
                decimal DiscountPercent = sd.DiscountPercentage;
                bool useSPP = sd.UseSPP.GetValueOrDefault(false);
                Decimal DiscountPrice;
                Decimal RetailPrice;
                if (sd.Enabled.GetValueOrDefault(false))
                {

                    //if (od.Item.CategoryName.ToUpper() != "SYSTEM" && !(od.IsPromo))
                    if (od.Item.CategoryName.ToUpper() != "SYSTEM")
                    {
                        #region *) Get current Discount1 and Discount2
                        string Discount1 = "";
                        string Discount2 = "";

                        if (!string.IsNullOrEmpty(od.DiscountDetail))
                        {
                            string[] discounts = od.DiscountDetail.Split('+');
                            if (discounts.Length > 1)
                            {
                                Discount1 = discounts[0];
                                Discount2 = discounts[1];
                            }
                            else
                            {
                                Discount1 = discounts[0];
                            }
                        }
                        #endregion

                        if (!string.IsNullOrEmpty(Discount1) && Discount1 != "0%" && !overwriteExisting)
                        {
                            // Discount level 1 already exists, and not allowed to overwrite existing discount,
                            // then skip and return
                            return true;
                        }

                        if ((bool)sd.ApplicableToAllItem)
                        {
                            OrderDet det = od;
                            if (det.IsPromo)
                                return true;

                            if (det.IsPromo && (discName != "P1"
                                && discName != "P2"
                                && discName != "P3"
                                && discName != "P4"
                                && discName != "P5"
                                && discName != "0"))
                            {
                                RetailPrice = od.UnitPrice;
                                DiscountPercent = 0;
                                DiscountPrice = 0;
                            }
                            else
                            {
                                det.SpecialDiscount = sd.DiscountName;
                                det.IsPromo = false;
                                det.IsSpecial = true;
                                if (useSPP == true && det.Item.Userfloat4.HasValue)
                                {
                                    Decimal.TryParse(det.Item.Userfloat4.ToString(), out RetailPrice);
                                    //det.UnitPrice = RetailPrice;
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true))
                                        DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity.GetValueOrDefault(0);
                                    else
                                        DiscountPrice = RetailPrice * ((decimal)DiscountPercent / 100) * det.Quantity.GetValueOrDefault(0);
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                                    {
                                        det.Remark = "SPP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%";
                                    }
                                    det.OriginalPriceOfSPP = RetailPrice;
                                }
                                else
                                {
                                    RetailPrice = det.UnitPrice;
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true))
                                        DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity.GetValueOrDefault(0);
                                    else
                                        DiscountPrice = RetailPrice * ((decimal)DiscountPercent / 100) * det.Quantity.GetValueOrDefault(0);
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                                    {
                                        det.Remark = "RP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%";
                                    }
                                    det.OriginalPriceOfSPP = 0;
                                }

                            }
                            //decimal CurrentAmount = RetailPrice - PromoSpecialDiscountController.RoundUp((DiscountPercent * RetailPrice / 100));
                            decimal CurrentAmount = 0;
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true))
                                CurrentAmount = PromoSpecialDiscountController.RoundUp(RetailPrice - (DiscountPercent * RetailPrice / 100));
                            else
                                CurrentAmount = RetailPrice - DiscountPercent * RetailPrice / 100;


                            #region *) Calculate Discount1 + Discount2
                            decimal TotalDiscountedPrice = CurrentAmount;
                            decimal discPercent1 = ((RetailPrice - CurrentAmount) / (RetailPrice + 0.00001M)) * 100;

                            Discount1 = discPercent1.ToString("N0") + "%";
                            det.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
                            if (!string.IsNullOrEmpty(Discount2))
                            {
                                if (Discount2.StartsWith("$"))
                                {
                                    decimal dollar;
                                    decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                                    TotalDiscountedPrice -= dollar;
                                }
                                else if (Discount2.EndsWith("%"))
                                {
                                    decimal percent;
                                    decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                                    TotalDiscountedPrice *= (1 - percent / 100);
                                }
                            }
                            #endregion

                            det.Discount = (det.UnitPrice - TotalDiscountedPrice) * 100 / det.UnitPrice;
                            det.Amount = (RetailPrice * det.Quantity.GetValueOrDefault(0)) - DiscountPrice;
                            det.SpecialDiscount = discName;
                            CalculateLineAmount(ref det);
                            /*Logger.writeLog(det.ItemNo + ", " + Convert.ToDecimal(det.GrossSales).ToString() + ", "
                                + Convert.ToDecimal(det.Discount).ToString("N0") + ", " + Convert.ToDecimal(det.Amount).ToString("N0") + ", " + det.SpecialDiscount);*/

                        }
                        else
                        {
                            OrderDet det = od;

                            if (det.IsPromo && sd.DiscountName != "P1" && sd.DiscountName != "P2"
                                && sd.DiscountName != "P3"
                                && sd.DiscountName != "P4"
                                && sd.DiscountName != "P5")
                                return true;

                            //SpecialDiscountDetailCollection sdd = new SpecialDiscountDetailCollection();
                            //sdd.Where(SpecialDiscountDetail.Columns.DiscountName, sd.DiscountName);
                            //sdd.Where(SpecialDiscountDetail.Columns.ItemNo, det.ItemNo);
                            //sdd.Load();

                             Decimal discount = 0;
                            if (sd.DiscountName == "P1" && det.Item.Userfloat6 != null)
                                discount = det.Item.Userfloat6.GetValueOrDefault(0);

                            if (sd.DiscountName == "P2" && det.Item.Userfloat7 != null)
                                discount = det.Item.Userfloat7.GetValueOrDefault(0);

                            if (sd.DiscountName == "P3" && det.Item.Userfloat8 != null)
                                discount = det.Item.Userfloat8.GetValueOrDefault(0);

                            if (sd.DiscountName == "P4" && det.Item.Userfloat9 != null)
                                discount = det.Item.Userfloat9.GetValueOrDefault(0);

                            if (sd.DiscountName == "P5" && det.Item.Userfloat10 != null)
                                discount = det.Item.Userfloat10.GetValueOrDefault(0);


                            if (discount > 0 || AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.AllowZeroMultiTierPrice), false) )
                            {
                                det.IsPromo = false;
                                det.IsSpecial = true;
                                det.SpecialDiscount = sd.DiscountName;
                                decimal discAmt = 0;
                                DiscountPercent = 0;
                                discAmt = det.Item.RetailPrice - discount;

                                if (useSPP == true && det.Item.Userfloat4.HasValue)
                                {
                                    Decimal.TryParse(det.Item.Userfloat4.ToString(), out RetailPrice);
                                    //det.UnitPrice = RetailPrice;
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true))
                                        DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity.GetValueOrDefault(0) + (discAmt * det.Quantity.GetValueOrDefault(0));
                                    else
                                        DiscountPrice = RetailPrice * ((decimal)DiscountPercent / 100) * det.Quantity.GetValueOrDefault(0) + (discAmt * det.Quantity.GetValueOrDefault(0));
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                                    {
                                        det.Remark = "SPP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%, Amount = " + discAmt.ToString("N2");
                                    }
                                    det.OriginalPriceOfSPP = RetailPrice;
                                }
                                else
                                {
                                    RetailPrice = det.UnitPrice;
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true))
                                        DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity.GetValueOrDefault(0) + (discAmt * det.Quantity.GetValueOrDefault(0));
                                    else
                                        DiscountPrice = RetailPrice * ((decimal)DiscountPercent / 100) * det.Quantity.GetValueOrDefault(0) + (discAmt * det.Quantity.GetValueOrDefault(0));
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                                    {
                                        det.Remark = "RCP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%, Amount = " + discAmt.ToString("N2");
                                    }
                                    det.OriginalPriceOfSPP = 0;
                                }

                                decimal CurrentAmount = 0;
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true))
                                    CurrentAmount = RetailPrice - PromoSpecialDiscountController.RoundUp(DiscountPercent * RetailPrice / 100) - discAmt;
                                else
                                    CurrentAmount = RetailPrice - DiscountPercent * RetailPrice / 100 - discAmt;

                                #region *) Calculate Discount1 + Discount2
                                decimal TotalDiscountedPrice = CurrentAmount;
                                decimal discPercent1 = ((RetailPrice - CurrentAmount) / (RetailPrice + 0.00001M)) * 100;
                                Discount1 = discPercent1.ToString("N0") + "%";
                                det.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
                                if (!string.IsNullOrEmpty(Discount2))
                                {
                                    if (Discount2.StartsWith("$"))
                                    {
                                        decimal dollar;
                                        decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                                        TotalDiscountedPrice -= dollar;
                                    }
                                    else if (Discount2.EndsWith("%"))
                                    {
                                        decimal percent;
                                        decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                                        TotalDiscountedPrice *= (1 - percent / 100);
                                    }
                                }
                                #endregion

                                det.Discount = (det.UnitPrice - TotalDiscountedPrice) * 100 / det.UnitPrice;
                                det.Amount = (RetailPrice * det.Quantity.GetValueOrDefault(0)) - DiscountPrice;
                                det.SpecialDiscount = discName;

                                CalculateLineAmount(ref det);
                                //Logger.writeLog(det.ItemNo + ", " + Convert.ToDecimal(det.GrossSales).ToString() + ", "
                                //+ Convert.ToDecimal(det.Discount).ToString("N0") + ", " + Convert.ToDecimal(det.Amount).ToString("N0") + ", " + det.Userfld5);

                            }
                        }

                    }

                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (discName == "0")
                {
                    //if (!od.IsPromo)
                    //{
                    od.Discount2Percent = 0;
                    od.Discount2Dollar = 0;
                    od.FinalPrice = 0;
                    if (!ChangeOrderLineDiscount
                        (od.OrderDetID, 0, true, true, out status))
                    {
                        throw new Exception("status");
                    }
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                    {
                        od.Remark = "";
                    }
                    od.SpecialDiscount = null;
                    od.DiscountDetail = null;
                    od.DiscountReason = "";
                    od.DiscountAuthorizedBy = "";
                    od.IsSpecial = false;
                    //}
                    myOrderHdr.Userfld7 = "";

                }
                return true;
            }
        }

        public bool ApplyCustomerPrice(string MembershipNo, ref OrderDet od)
        {
            string status = "";
            try
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseCustomerPricing), false) && MembershipNo != "WALK-IN"
                    && od.Item.CategoryName.ToUpper() != "SYSTEM")
                {
                    od.UnitPrice = ItemController.GetCustomPricing(od.ItemNo, MembershipNo, PointOfSaleInfo.PointOfSaleID);
                    od.Amount = (od.Quantity ?? 0) * od.UnitPrice;
                    od.Discount2Percent = 0;
                    od.Discount2Dollar = 0;
                    od.FinalPrice = 0;
                    od.Discount = 0;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                    {
                        od.Remark = "";
                    }
                    od.SpecialDiscount = null;
                    od.DiscountDetail = null;
                    od.DiscountReason = "";
                    od.DiscountAuthorizedBy = "";
                    od.IsSpecial = false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                Logger.writeLog("Error Apply Customer Price :" + Ex.Message);
                status = "Error Apply Customer Price :" + Ex.Message;
                return false;
            }

        }

        public bool ApplyCustomerPriceAll(string MembershipNo)
        {
            string status = "";
            try
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseCustomerPricing), false) && MembershipNo != "WALK-IN")
                {
                    OrderDet od;

                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        od = myOrderDet[i];
                        if (od.Item.CategoryName.ToUpper() != "SYSTEM" && !od.IsVoided)
                        {

                            od.UnitPrice = ItemController.GetCustomPricing(od.ItemNo, MembershipNo, PointOfSaleInfo.PointOfSaleID);
                            od.Amount = (od.Quantity ?? 0) * od.UnitPrice;
                            od.Discount2Percent = 0;
                            od.Discount2Dollar = 0;
                            od.FinalPrice = 0;
                            od.Discount = 0;
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), true))
                            {
                                od.Remark = "";
                            }
                            od.SpecialDiscount = null;
                            od.DiscountDetail = null;
                            od.DiscountReason = "";
                            od.DiscountAuthorizedBy = "";
                            od.IsSpecial = false;

                            CalculateLineAmount(ref od);
                        }
                    }
                    UndoPromo();
                    ApplyPromo();
                }

                return true;
            }
            catch (Exception Ex)
            {
                Logger.writeLog("Error Apply Customer Price :" + Ex.Message);
                status = "Error Apply Customer Price :" + Ex.Message;
                return false;
            }

        }

        //get line amount using the ID
        public OrderDet GetLine(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return null;
            }

        }
        //calculate line amount
        public bool CalculateLineAmount(ref OrderDet myOrderDetItem)
        {
            int GSTRule = 0;
            bool isUsingSpecialGSTRule = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.UseSpecialGSTRuleForFormal), false);
            //assign GST rules
            if (myOrderDetItem.Item.GSTRule.HasValue)
                GSTRule = myOrderDetItem.Item.GSTRule.Value;

            //If GST has been set on XML file, use the outlet GST rule instead
            if (GSTOverride.GSTRule != 0)
                GSTRule = GSTOverride.GSTRule;

            if (mode == "formal")
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.UseSpecialGSTRuleForFormal), false))
                    GSTRule = int.Parse(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.SpecialGSTRule));
            }

            if (mode == "formal" && myOrderDetItem.Item.GSTRule == 3)
            {
                GSTRule = 3;
            }

            if (myOrderHdr.GSTRule != 0)
            {
                GSTRule = myOrderHdr.GSTRule;
            }

            if (myOrderDetItem.IsChangetoNonGST)
            {
                myOrderDetItem.UnitPrice = myOrderDetItem.OriginalRetailPrice;
                myOrderDetItem.IsChangetoNonGST = false;
            }

            double _GST = GST;
            if (myOrderDetItem.Item != null && myOrderDetItem.Item.Category != null)
            {
                Category cat = myOrderDetItem.Item.Category;
                if (cat.IsOverrideGST)
                    _GST = Convert.ToDouble(cat.GSTPercentage);
            }

            if (myOrderDetItem.IsPromo) //calculate value for promo item
            {
                if (myOrderDetItem.UsePromoPrice.HasValue &&
                    myOrderDetItem.UsePromoPrice.Value) //if it is using promo price
                {
                    //Variety GST rules
                    if (GSTRule == 1)
                    {
                        //Exclusive GST
                        myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                         (decimal)myOrderDetItem.PromoUnitPrice, 2);
                        myOrderDetItem.DiscountDetail = myOrderDetItem.PromoDiscount.ToString("N2") + "%";
                        if (myOrderDetItem.Discount2Percent > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - (myOrderDetItem.Discount2Percent / 100 * myOrderDetItem.PromoAmount), 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ " + myOrderDetItem.Discount2Percent.ToString("N2") + "%";
                        }
                        if (myOrderDetItem.Discount2Dollar > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - (myOrderDetItem.Discount2Dollar * myOrderDetItem.Quantity.GetValueOrDefault(0)), 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + myOrderDetItem.Discount2Dollar.ToString("N2");
                        }
                        if (myOrderDetItem.FinalPrice > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.FinalPrice * myOrderDetItem.Quantity.GetValueOrDefault(0), 2);
                            decimal diff = Math.Round((myOrderDetItem.UnitPrice - ((myOrderDetItem.UnitPrice * (decimal)myOrderDetItem.PromoDiscount) / 100)) - myOrderDetItem.FinalPrice, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + diff.ToString("N2");
                        }
                        //myOrderDetItem.PromoAmount

                        myOrderDetItem.PromoAmount = myOrderDetItem.PromoAmount
                         * (1 + ((decimal)_GST) / 100); //The GST part   

                        //assign GST amount
                        myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100);

                    }
                    else
                    {
                        //Inclusive GST
                        myOrderDetItem.PromoAmount =
                            Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                         (decimal)myOrderDetItem.PromoUnitPrice, 2);
                        myOrderDetItem.DiscountDetail = myOrderDetItem.PromoDiscount.ToString("N2") + "%";
                        if (myOrderDetItem.Discount2Percent > 0)
                        {
                            decimal DiscountedPromoUnitPrice = Math.Round((decimal)myOrderDetItem.PromoUnitPrice - (myOrderDetItem.Discount2Percent / 100 * myOrderDetItem.PromoUnitPrice), 2);
                            //myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - (myOrderDetItem.Discount2Percent / 100 * myOrderDetItem.PromoAmount), 2);
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                         DiscountedPromoUnitPrice, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ " + myOrderDetItem.Discount2Percent.ToString("N2") + "%";
                        }
                        if (myOrderDetItem.Discount2Dollar > 0)
                        {
                            decimal DiscountedPromoUnitPrice = Math.Round((decimal)myOrderDetItem.PromoUnitPrice - myOrderDetItem.Discount2Dollar, 2);
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                         DiscountedPromoUnitPrice, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + myOrderDetItem.Discount2Dollar.ToString("N2");
                        }
                        if (myOrderDetItem.FinalPrice > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.FinalPrice * myOrderDetItem.Quantity.GetValueOrDefault(0), 2);
                            decimal diff = Math.Round((myOrderDetItem.UnitPrice - ((myOrderDetItem.UnitPrice * (decimal)myOrderDetItem.PromoDiscount) / 100)) - myOrderDetItem.FinalPrice, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + diff.ToString("N2");
                        }

                        if (GSTRule == 2) //Inclusive GST
                        {
                            //calculate the GST formula
                            myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100); ;
                        }
                        else if (GSTRule == 3) //Absorb GST
                        {
                            myOrderDetItem.GSTAmount = 0;
                        }
                        else
                        {
                            //non GST, there is no value
                            myOrderDetItem.GSTAmount = 0;

                            if (myOrderDetItem.Item.GSTRule == 2)
                            {
                                myOrderDetItem.UnitPrice = RoundDownNearestFiveCent(myOrderDetItem.UnitPrice - (myOrderDetItem.UnitPrice / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100));
                                myOrderDetItem.PromoUnitPrice = RoundDownNearestFiveCent(myOrderDetItem.PromoUnitPrice - (myOrderDetItem.PromoUnitPrice / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100));
                                myOrderDetItem.PromoAmount = RoundDownNearestFiveCent((decimal)myOrderDetItem.Quantity * (decimal)myOrderDetItem.PromoUnitPrice);
                                myOrderDetItem.IsChangetoNonGST = true;
                            }
                        }
                    }
                }
                else //for promo by percentage - using percentage discount
                {
                    if (GSTRule == 1)
                    {
                        //Exclusive GST
                        myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            (decimal)myOrderDetItem.UnitPrice *
                            (decimal)(1 - (myOrderDetItem.PromoDiscount / 100)), 2);
                        myOrderDetItem.DiscountDetail = myOrderDetItem.PromoDiscount.ToString("N2") + "%";

                        if (myOrderDetItem.Discount2Percent > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - (myOrderDetItem.Discount2Percent / 100 * myOrderDetItem.PromoAmount), 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ " + myOrderDetItem.Discount2Percent.ToString("N2") + "%";
                        }
                        if (myOrderDetItem.Discount2Dollar > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - myOrderDetItem.Discount2Dollar, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + myOrderDetItem.Discount2Dollar.ToString("N2");
                        }
                        if (myOrderDetItem.FinalPrice > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.FinalPrice * myOrderDetItem.Quantity.GetValueOrDefault(0), 2);
                            decimal diff = Math.Round((myOrderDetItem.UnitPrice - ((myOrderDetItem.UnitPrice * (decimal)myOrderDetItem.PromoDiscount) / 100)) - myOrderDetItem.FinalPrice, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + diff.ToString("N2");
                        }

                        myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount * (1 + ((decimal)_GST) / 100), 2); //The GST part        

                        //GST amount is additional
                        myOrderDetItem.GSTAmount =
                            (myOrderDetItem.PromoAmount / (1 + ((decimal)_GST) / 100)) *
                            ((decimal)_GST / 100);
                    }
                    else //Inclusive GST & NO GST
                    {

                        myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            (decimal)myOrderDetItem.UnitPrice *
                            (decimal)(1 - (myOrderDetItem.PromoDiscount / 100)), 2);
                        myOrderDetItem.DiscountDetail = myOrderDetItem.PromoDiscount.ToString("N2") + "%";

                        if (myOrderDetItem.Discount2Percent > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - (myOrderDetItem.Discount2Percent / 100 * myOrderDetItem.PromoAmount), 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ " + myOrderDetItem.Discount2Percent.ToString("N2") + "%";
                        }
                        if (myOrderDetItem.Discount2Dollar > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.PromoAmount - myOrderDetItem.Discount2Dollar, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + myOrderDetItem.Discount2Dollar.ToString("N2");
                        }
                        if (myOrderDetItem.FinalPrice > 0)
                        {
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.FinalPrice * myOrderDetItem.Quantity.GetValueOrDefault(0), 2);
                            decimal diff = Math.Round((myOrderDetItem.UnitPrice - ((myOrderDetItem.UnitPrice * (decimal)myOrderDetItem.PromoDiscount) / 100)) - myOrderDetItem.FinalPrice, 2);
                            myOrderDetItem.DiscountDetail = myOrderDetItem.DiscountDetail + "+ $" + diff.ToString("N2");
                        }

                        //inclusive gst and non gst has the same values.
                        if (GSTRule == 2) //Inclusive GST
                        {
                            myOrderDetItem.GSTAmount =
                                (myOrderDetItem.PromoAmount / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100); ;
                        }
                        else if (GSTRule == 3) //Absorb GST
                        {
                            myOrderDetItem.GSTAmount = 0;
                        }
                        else
                        {
                            //no GST
                            myOrderDetItem.GSTAmount = 0;

                            if (myOrderDetItem.Item.GSTRule == 2)
                            {
                                myOrderDetItem.UnitPrice = RoundDownNearestFiveCent(myOrderDetItem.UnitPrice - (myOrderDetItem.UnitPrice / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100));
                                myOrderDetItem.PromoAmount = RoundDownNearestFiveCent((decimal)myOrderDetItem.Quantity * (decimal)myOrderDetItem.UnitPrice * (decimal)(1 - (myOrderDetItem.PromoDiscount / 100)));
                                myOrderDetItem.IsChangetoNonGST = true;
                            }
                        }
                    }
                }
            }
            else //calculation for non promo item
            {
                if (GSTRule == 1)
                {
                    //Exclusive GST
                    myOrderDetItem.UnitPrice = Math.Round(myOrderDetItem.UnitPrice, 2, MidpointRounding.AwayFromZero);
                    myOrderDetItem.Amount = Math.Round(
                            myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            myOrderDetItem.UnitPrice *
                            (1 - (myOrderDetItem.Discount / 100))
                             * (1 + ((decimal)_GST) / 100), 2); //The GST part            
                    
                    myOrderDetItem.GSTAmount = (myOrderDetItem.Amount / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100);
                }
                else
                {
                    //Inclusive GST
                    myOrderDetItem.UnitPrice = Math.Round(myOrderDetItem.UnitPrice, 2, MidpointRounding.AwayFromZero);
                    

                    myOrderDetItem.Amount = Math.Round(
                            myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            myOrderDetItem.UnitPrice *
                            (1 - (myOrderDetItem.Discount / 100)), 2);
                    

                    if (GSTRule == 2) //Inclusive GST
                    {
                        myOrderDetItem.GSTAmount = myOrderDetItem.Amount / (1 + ((decimal)_GST) / 100) * ((decimal)_GST / 100); ;
                    }
                    else if (GSTRule == 3) //Absorb GST
                    {   
                        myOrderDetItem.GSTAmount = 0;
                    }
                    else
                    {
                        myOrderDetItem.GSTAmount = 0;

                        if (myOrderDetItem.Item.GSTRule == 2)
                        {
                            myOrderDetItem.UnitPrice = RoundDownNearestFiveCent(myOrderDetItem.UnitPrice - (myOrderDetItem.UnitPrice / (1 + ((decimal)_GST) / 100)) * ((decimal)_GST / 100));
                            myOrderDetItem.Amount = RoundDownNearestFiveCent((decimal)myOrderDetItem.Quantity * myOrderDetItem.UnitPrice * (1 - (myOrderDetItem.Discount / 100)));
                            myOrderDetItem.IsChangetoNonGST = true;
                        }
                    }
                }
            }

            return true;
        }

        public decimal GetGrossAmount()
        {
            return myOrderHdr.GrossAmount;
        }

        public decimal GetGrossAmountByUnitPrice()
        {
            decimal tmp = 0;
            foreach (OrderDet od in myOrderDet)
            {
                tmp = tmp + ((decimal)od.UnitPrice * (decimal)od.Quantity.GetValueOrDefault(0));
            }
            return tmp;
        }

        public decimal GetCashBackAmount()
        {
            decimal tmp = 0;
            string cashBackItemNo = AppSetting.GetSetting(AppSetting.SettingsName.Payment.CashbackItemNo);
            foreach (OrderDet od in myOrderDet)
            {
                if (od.ItemNo == cashBackItemNo)
                {
                    tmp += od.Amount;
                }
            }
            return tmp;
        }

        //check if inside the receipt has any pre orders items
        public bool hasPreOrder()
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].IsPreOrder.HasValue &&
                    myOrderDet[i].IsPreOrder.Value && !myOrderDet[i].IsVoided)
                {
                    return true;
                }
            }
            return false;
        }

        //allow change cashier if necessary
        public void SetNewCashier(string newCashierName)
        {
            myOrderHdr.CashierID = newCashierName;
        }

        //Get a point of sale ID of a particular receipt
        public int GetPointOfSaleId()
        {
            return myOrderHdr.PointOfSaleID;
        }
        /*
        public Membership GetOtherMemberInfo()
        {
            if (myOrderHdr.MembershipNo != "")
            {
                Membership member = new Membership();
                member.MembershipNo = myOrderHdr.MembershipNo;
                member.NameToAppear = "Other";
                member.MembershipGroupId = MembershipController.DEFAULT_GROUPID;
                return member;
            }
            return null;
        }*/
        /*
        public bool hasCashPayment()
        {
            //
            for (int i = 0; i < recDet.Count; i++)
            {
                if (recDet[i].PaymentType == PAY_CASH)
                {
                    return true;
                }
            }
            return false;
        }*/
        //check if a particular receipt has a certain payment type
        public bool hasPaymentType(string paymentType)
        {
            //
            for (int i = 0; i < recDet.Count; i++)
            {
                if (recDet[i].PaymentType == paymentType)
                {
                    return true;
                }
            }
            return false;
        }
        /*
        public decimal GetInstallmentAmount()
        {
            //
            for (int i = 0; i < recDet.Count; i++)
            {
                if (recDet[i].PaymentType == PAY_INSTALLMENT)
                {
                    return recDet[i].Amount;
                }
            }
            return 0.0M;
        }*/
        /*
        public void SetOverallDiscount(decimal disc)
        {
            myOrderHdr.Discount = disc;
        }
        */
        /*
        public decimal getOverallDisc()
        {
            return myOrderHdr.Discount;
        }

        public decimal getOverallDiscAmount()
        {
            return ((myOrderHdr.NettAmount / (100 - myOrderHdr.Discount)) * 100) - myOrderHdr.NettAmount;
        }
        */

        //Fetch Items that are inside orderdet
        //return as datatable - 
        //this is needed to print report 
        //without needing to pass the entire table to crystal report
        public DataTable fetchItemDt()
        {
            Query qr = Item.CreateQuery();
            qr.QueryType = QueryType.Select;
            string[] itemnos = new string[myOrderDet.Count];
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                itemnos[i] = myOrderDet[i].ItemNo;
            }
            DataSet ds = qr.IN(Item.Columns.ItemNo, itemnos).ExecuteDataSet();
            DataTable dt = ds.Tables[0];
            ds.Tables.RemoveAt(0);
            dt.TableName = "Item";

            return dt;
        }
        /*
        public DataSet fetchDataSet()
        {
            DataSet ds = new DataSet();
            Logger.writeLog("");
            OrderHdrCollection a = new OrderHdrCollection();
            a.Add(myOrderHdr);
        
            ds.Tables.Add(a.ToDataTable());
            OrderDetCollection tmpdet = new OrderDetCollection();        
            myOrderDet.CopyTo(tmpdet);                                    
            int i = tmpdet.Count -1;
            while (i >= 0)
            {
                if (tmpdet[i].IsVoided)
                    tmpdet.RemoveAt(i);            
                i--;
            }        
            DataTable dtTest = tmpdet.ToDataTable();
            //DataTable dtTest = convertOrderDetToDataTable(tmpdet); //for boosting performance. untested
            ds.Tables.Add(dtTest);
            
            MembershipCollection b = new MembershipCollection();
            if (this.CurrentMember == null)
            {
                LoadMembership();
            }
            b.Add(this.CurrentMember);
            ds.Tables.Add(b.ToDataTable());
           
            //
            ds.Tables.Add(fetchItemDt());

            
            PointOfSaleCollection l = new PointOfSaleCollection();
            l.Where(PointOfSale.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID).Load();            
            ds.Tables.Add(l.ToDataTable());            

            return ds;
        }
        */
        /* -For boosting performance- untested
        private DataTable convertOrderDetToDataTable(OrderDetCollection myOrderDet)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.TableName = "OrderDet";
                TableSchema.TableColumnCollection t = OrderDet.Schema.Columns;

                for (int i = 0; i < t.Count; i++)
                {
                    dt.Columns.Add(t[i].ColumnName, t[i].GetPropertyType());
                }
                DataRow dr;
                foreach (OrderDet orderDet in myOrderDet)
                {
                    for (int i = 0; i < t.Count; i++)
                    {
                        dr = dt.NewRow();
                        object obj  = orderDet.GetColumnValue(t[i].ColumnName);
                        if (obj != null)
                            dr[t[i].ColumnName] = obj;
                        else
                            dr[t[i].ColumnName] = DBNull.Value;
                        dt.Rows.Add(dr);
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        */
        /*
        //Take all data set from payment
        public DataSet fetchDataSetPaymentsDataSet()
        {
            ReceiptHdrCollection r = new ReceiptHdrCollection();
            r.Add(recHdr);
            DataSet ds = new DataSet();
            ds.Tables.Add(r.ToDataTable());
            ds.Tables.Add(recDet.ToDataTable());
            return ds;
        }
        */
        /*
        public bool hasWarrantyItems()
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].Item.HasWarranty.HasValue &&
                    myOrderDet[i].Item.HasWarranty.Value)
                    return true;
            }
            return false;
        }
        */
        /*
        public bool hasDeliveryItem()
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].IsPreOrder.HasValue&&
                    myOrderDet[i].IsPreOrder.Value)
                    return true;
            }
            return false;
        }
        */

        //calculate total GST
        public decimal calculateTotalGST()
        {
            decimal GST = 0;
            //loop through every line item and add the GST amount
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!myOrderDet[i].IsVoided &&
                    myOrderDet[i].GSTAmount.HasValue)
                    GST += Math.Round(myOrderDet[i].GSTAmount.Value, 2);
            }
            //calculate total GST, take discount at orderhdr into consideration
            //TODO: remove overall discount?
            myOrderHdr.GSTAmount = Math.Round(GST *
                (decimal)(1 - myOrderHdr.Discount / 100), 2);

            return myOrderHdr.GSTAmount.Value;
        }

        public decimal calculateTotalQuantity()
        {
            decimal Qty = 0;
            //loop through every line item and add the GST amount
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!myOrderDet[i].IsVoided)
                    Qty += myOrderDet[i].Quantity.GetValueOrDefault(0);
            }
            //calculate total GST, take discount at orderhdr into consideration
            //TODO: remove overall discount?


            return Qty;
        }

        public decimal calculateTotalItems()
        {
            decimal Qty = 0;
            //loop through every line item and add the GST amount
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!myOrderDet[i].IsVoided)
                    Qty += 1;
            }
            //calculate total GST, take discount at orderhdr into consideration
            //TODO: remove overall discount?


            return Qty;
        }

        //TODO: to be reviewed if it is necessary
        public void SetTotalDiscount(decimal totalDiscountAmount)
        {
            myOrderHdr.DiscountAmount = totalDiscountAmount;
        }
        /*
        public bool InstallmentPaymentHasAlreadyBeenSpecified(string refno)
        {

            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].ItemNo == InstallmentController.INSTALLMENT_ITEM 
                    && myOrderDet[i].VoucherNo == refno)
                {
                    return true;
                }
            }
            return false;
        }
        */


        /// <summary>
        /// Loop through the order det and check if we have enough balance to deduct sales
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool IsQtySufficientToDoStockOut(out string status)
        {
            status = "";

            //commented because of real time sales being sent
            #region *) Validation: Always return true if not integrated with Inventory
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                status = "";
                return true;
            }
            #endregion

            //create a hashtable
            Hashtable items = new Hashtable();
            #region *) Fetch: Total all the quantities needed for current transaction
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                bool ShouldIProcess = true;
                ShouldIProcess = ShouldIProcess && !myOrderDet[i].IsVoided;                 /* OrderDet is NOT voided */
                ShouldIProcess = ShouldIProcess && myOrderDet[i].Item.IsInInventory;        /* Item is Inventory Item */
                ShouldIProcess = ShouldIProcess && myOrderDet[i].IsPreOrder.HasValue;
                ShouldIProcess = ShouldIProcess && !myOrderDet[i].IsPreOrder.Value;
                ShouldIProcess = ShouldIProcess && myOrderDet[i].InventoryHdrRefNo == "";   /* Only deduct the undeducted */

                if (ShouldIProcess)
                {
                    if (items.ContainsKey(myOrderDet[i].ItemNo))
                    {
                        //add the quantity to the hash table
                        //this is to tackle the scenario where two line item has same item no
                        items[myOrderDet[i].ItemNo] = (decimal)items[myOrderDet[i].ItemNo] + myOrderDet[i].Quantity;
                    }
                    else
                    {
                        //add the item to the hash table
                        items.Add(myOrderDet[i].ItemNo, myOrderDet[i].Quantity);
                    }
                }
            }
            #endregion

            foreach (string k in items.Keys)
            {
                #region *) Validation: Negative sales will only add qty, no need to check
                if ((decimal)items[k] <= 0) continue;
                #endregion

                #region *) Validation: Check if quantity sufficient
                string innerStatus = "";
                decimal BalanceQty = ItemSummaryController.FetchStockBalanceByItemNo(k, PointOfSaleInfo.InventoryLocationID);
                decimal SalesQty = (decimal)items[k];

                if (BalanceQty < SalesQty)
                {
                    if (status == "") status = "Insufficient quantity to perform stock out";
                    status += "\nItem " + k + ". Sales quantity is " + items[k].ToString() + " and balance quantity is " + BalanceQty;
                }
                #endregion
            }

            return (status == ""); /* If got error, return false */
        }

        public bool IsQtySufficientToDoStockOutLocal(out string status)
        {
            status = "";

            //create a hashtable
            Hashtable items = new Hashtable();
            #region *) Fetch: Total all the quantities needed for current transaction
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                bool ShouldIProcess = true;
                ShouldIProcess = ShouldIProcess && !myOrderDet[i].IsVoided;                 /* OrderDet is NOT voided */
                ShouldIProcess = ShouldIProcess && myOrderDet[i].Item.IsInInventory;        /* Item is Inventory Item */
                ShouldIProcess = ShouldIProcess && myOrderDet[i].IsPreOrder.HasValue;
                ShouldIProcess = ShouldIProcess && !myOrderDet[i].IsPreOrder.Value;
                ShouldIProcess = ShouldIProcess && myOrderDet[i].InventoryHdrRefNo == "";   /* Only deduct the undeducted */

                if (ShouldIProcess)
                {
                    if (items.ContainsKey(myOrderDet[i].ItemNo))
                    {
                        //add the quantity to the hash table
                        //this is to tackle the scenario where two line item has same item no
                        items[myOrderDet[i].ItemNo] = (decimal)items[myOrderDet[i].ItemNo] + myOrderDet[i].Quantity;
                    }
                    else
                    {
                        //add the item to the hash table
                        items.Add(myOrderDet[i].ItemNo, myOrderDet[i].Quantity);
                    }
                }
            }
            #endregion

            foreach (string k in items.Keys)
            {
                #region *) Validation: Negative sales will only add qty, no need to check
                if ((decimal)items[k] <= 0) continue;
                #endregion

                #region *) Validation: Check if quantity sufficient
                string innerStatus = "";
                decimal BalanceQty = ItemSummaryController.FetchStockBalanceByItemNo(k, PointOfSaleInfo.InventoryLocationID);
                decimal SalesQty = (decimal)items[k];

                if (BalanceQty < SalesQty)
                {
                    if (status == "") status = "Insufficient quantity to perform stock out";
                    status += "\nItem " + k + ". Sales quantity is " + items[k].ToString() + " and balance quantity is " + BalanceQty;
                }
                #endregion
            }

            return (status == ""); /* If got error, return false */
        }

        //Get GST amount calculated 
        public decimal GetGSTAmount()
        {
            if (myOrderHdr.GSTAmount.HasValue)
                return myOrderHdr.GSTAmount.Value;
            return 0;
        }

        //Assign Line sales person to userfld1
        public void AssignItemSalesPerson(string LineID, string username, string username2)
        {
            string status;
            OrderDet myDet = GetLine(LineID, out status);
            myDet.SalesPerson = username;
            myDet.SalesPerson2 = username2;
        }
        /*
        //Assign default sales person if the line sales person is empty
        //TODO: consider to remove this function, since the reports are able to handle
        public void AssignDefaultSalesPerson(string SalesPersonID)
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].Item.IsCommission.HasValue &&
                    myOrderDet[i].Item.IsCommission.Value &&
                    (myOrderDet[i].Userfld1 == "" || myOrderDet[i].Userfld1 == null))
                {
                    myOrderDet[i].Userfld1 = SalesPersonID;
                }
            }
        }
        */
        //Calculate the total sum of all line discount
        public decimal CalculateTotalDiscount()
        {
            decimal Amount;
            decimal totalDiscount = 0.0M;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!myOrderDet[i].IsVoided)
                {
                    if (myOrderDet[i].IsPromo)
                    {
                        //using promo price                             
                        if (myOrderDet[i].UsePromoPrice.HasValue &&
                            myOrderDet[i].UsePromoPrice.Value)
                        {
                            Amount = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].Amount));
                            totalDiscount += myOrderDet[i].GrossSales.GetValueOrDefault(0) - CommonUILib.RemoveRoundUp(myOrderDet[i].PromoAmount);
                            //Note: removed because of wrong calculation
                            //if (myOrderDet[i].Userfloat3.HasValue && myOrderDet[i].Userfloat3 != 0)
                            //{
                            //    totalDiscount += CommonUILib.RemoveRoundUp((myOrderDet[i].Quantity.GetValueOrDefault(0) * myOrderDet[i].PromoUnitPrice * (decimal)myOrderDet[i].Userfloat3 / 100));
                            //}
                            //if (myOrderDet[i].Userfloat4.HasValue && myOrderDet[i].Userfloat4 != 0)
                            //{
                            //    totalDiscount += myOrderDet[i].Userfloat4.GetValueOrDefault(0);
                            //}

                        }
                        else
                        {
                            //using promo discount
                            totalDiscount += CommonUILib.RemoveRoundUp((myOrderDet[i].Quantity.GetValueOrDefault(0) * myOrderDet[i].UnitPrice * (decimal)myOrderDet[i].PromoDiscount / 100));
                            if (myOrderDet[i].Userfloat3.HasValue && myOrderDet[i].Userfloat3 != 0)
                            {
                                totalDiscount += CommonUILib.RemoveRoundUp((myOrderDet[i].Quantity.GetValueOrDefault(0) * myOrderDet[i].PromoUnitPrice * (decimal)myOrderDet[i].Userfloat3 / 100));
                            }
                            if (myOrderDet[i].Userfloat4.HasValue && myOrderDet[i].Userfloat4 != 0)
                            {
                                totalDiscount += myOrderDet[i].Userfloat4.GetValueOrDefault(0);
                            }
                        }
                    }
                    else
                    {
                        //using normal non promo (use percentage)
                        totalDiscount += Math.Round((myOrderDet[i].Quantity.GetValueOrDefault(0) * myOrderDet[i].UnitPrice * myOrderDet[i].Discount / 100), 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            return totalDiscount;
        }
        //Calculate the percentage of the line discount (shown on line receipt)
        public decimal GetLineDiscountPercentage(int i)
        {
            if (myOrderDet[i].IsPromo)
            {
                //how about promo by price?
                return CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount));

            }
            else
            {
                return myOrderDet[i].Discount;
            }
        }

        //Calculate the percentage of the line discount (shown on line receipt)
        public decimal GetLineDiscountPercentageByOrderDetID(string OrderDetID)
        {
            OrderDet od = new OrderDet(OrderDetID);
            if (od.IsPromo)
            {
                //how about promo by price?
                return CommonUILib.RemoveRoundUp(((decimal)od.PromoDiscount));

            }
            else
            {
                return od.Discount;
            }
        }
        //get the header remark
        public string GetRemarks()
        {
            if (myOrderHdr.Remark != null)
                return myOrderHdr.Remark;

            return "";
        }

        public string GetReturnedReceiptNo()
        {
            string result = "";
            foreach (OrderDet od in myOrderDet)
            {
                if (od.ReturnedReceiptNo != null && od.ReturnedReceiptNo != "")
                    result = od.ReturnedReceiptNo;
            }

            return result;
        }

        public bool ApplyAuthorizedBy(string LineID, string AuthorizedBy)
        {
            bool isSuccess = false;
            string status = "";
            try
            {
                OrderDet myDet = GetLine(LineID, out status);
                if (myDet != null)
                    myDet.DiscountAuthorizedBy = AuthorizedBy;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool ApplyDiscountReason(string LineID, string DiscountReason)
        {
            bool isSuccess = false;
            string status = "";
            try
            {
                OrderDet myDet = GetLine(LineID, out status);
                if (myDet != null)
                    myDet.DiscountReason = DiscountReason;
                isSuccess = true;
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool ApplyDiscountLevel1(string Discount1, string LineID, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                OrderDet myDet = GetLine(LineID, out status);

                #region *) Validation
                if (string.IsNullOrEmpty(Discount1))
                    throw new Exception("Discount Invalid");
                decimal TotalDiscountedPrice = 0;
                string Discount1Text = Discount1;

                if (myDet.UnitPrice == 0)
                {
                    throw new Exception("Cannot apply discount to the $0 item");
                }

                if (myDet.Item.IsNonDiscountable)
                {
                    return true;
                }



                if (Discount1.StartsWith("$"))
                {
                    if (myDet.Quantity == 0)
                        return false;
                    TotalDiscountedPrice = myDet.UnitPrice
                                            - (Discount1.Replace("$", "").GetDecimalValue() / myDet.Quantity.GetValueOrDefault(0))
                                            - (myDet.Discount2Dollar / myDet.Quantity.GetValueOrDefault(0));
                    if (myDet.UnitPrice != 0)
                        Discount1Text = (((Discount1.Replace("$", "").GetDecimalValue() / myDet.Quantity.GetValueOrDefault(0)) / myDet.UnitPrice) * 100).ToString("N2") + "%";
                    else
                        Discount1Text = "0%";
                }
                else if (Discount1.EndsWith("%"))
                {
                    if (Discount1.Replace("%", "").GetDecimalValue() < 0 ||
                        Discount1.Replace("%", "").GetDecimalValue() > 100)
                    {
                        throw new Exception("Discount percentage invalid");
                    }

                    TotalDiscountedPrice = myDet.UnitPrice
                        - (myDet.UnitPrice * (Discount1.Replace("%", "").GetDecimalValue() / 100))
                        - (myDet.Discount2Dollar / myDet.Quantity.GetValueOrDefault(0));
                }
                if (Math.Round(TotalDiscountedPrice, 2) < 0)
                {
                    throw new Exception(string.Format("Item {0}: Discount amount cannot exceed {1}.",
                        myDet.ItemNo,
                        "$" + (Discount1.Replace("$", "").GetDecimalValue()
                           + myDet.Discount2Dollar).ToString("N2")));
                }

                if (myDet.Discount2Percent > 0)
                {
                    TotalDiscountedPrice = TotalDiscountedPrice
                                           - (TotalDiscountedPrice * (myDet.Discount2Percent / 100));
                }

                #endregion

                decimal DiscPercent = ((myDet.UnitPrice - TotalDiscountedPrice) / myDet.UnitPrice) * 100;
                //update discount - IF NON PROMO
                if (!myDet.IsPromo)
                {
                    myDet.Discount = DiscPercent;
                    if (myDet.Discount != GetPreferredDiscount())
                    {
                        myDet.IsSpecial = true;
                    }
                    string Discount2 = "";
                    if (myDet.Discount2Dollar > 0)
                        Discount2 = "+$" + myDet.Discount2Dollar.ToString("N2");
                    else if (myDet.Discount2Percent > 0)
                        Discount2 = "+" + myDet.Discount2Percent.ToString("N2") + "%";
                    myDet.DiscountDetail = Discount1Text + Discount2;
                }
                else
                {
                    myDet.Discount = 0;
                    myDet.Discount2Dollar = 0;
                    myDet.Discount2Percent = 0;
                    myDet.FinalPrice = 0;
                    myDet.DiscountDetail = "";
                    myDet.DiscountReason = "";
                    myDet.DiscountAuthorizedBy = "";
                }
                CalculateLineAmount(ref myDet);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                status = "Discount Fail : " + ex.Message;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool ApplyDiscountLevel2(string Discount2, string LineID, out string status)
        {
            status = "";
            OrderDet myDet = GetLine(LineID, out status);
            if (myDet.UnitPrice == 0)
            {
                status = "Cannot apply discount to the $0 item";
                return false;
            }

            if (myDet.Item.IsNonDiscountable)
            {
                status = "";
                return true;
            }


            #region *) Get discounted price before Discount level 2
            decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
            if (myDet.IsPromo)
                discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * (decimal)myDet.PromoDiscount) / 100);
            string oldDiscount2 = "";
            if (!myDet.IsPromo)
            {
                if (!string.IsNullOrEmpty(myDet.DiscountDetail))
                {
                    string[] discounts = myDet.DiscountDetail.Split('+');
                    if (discounts.Length > 1)
                    {
                        oldDiscount2 = discounts[1];
                    }
                }
                if (!string.IsNullOrEmpty(oldDiscount2))
                {
                    if (oldDiscount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(oldDiscount2.TrimStart('$'), out dollar);
                        discountedPrice += (dollar / myDet.Quantity.GetValueOrDefault(0));
                    }
                    else if (oldDiscount2.EndsWith("%"))
                    {
                        decimal percent;
                        decimal.TryParse(oldDiscount2.TrimEnd('%'), out percent);
                        if (percent == 100)
                            discountedPrice = myDet.UnitPrice;
                        else
                            discountedPrice /= (1 - percent / 100);
                    }
                }
            }
            #endregion

            #region *) Calculate Discount1 + Discount2
            decimal RetailPrice = myDet.UnitPrice;
            decimal TotalDiscountedPrice = discountedPrice;
            //decimal discPercent1 = ((RetailPrice - discountedPrice) / (RetailPrice + 0.00001M)) * 100;
            decimal discPercent1 = ((RetailPrice - discountedPrice) / RetailPrice) * 100;
            string Discount1 = discPercent1.ToString("N0") + "%";
            if (!string.IsNullOrEmpty(Discount2))
            {
                if (Discount2.StartsWith("$"))
                {
                    decimal dollar;
                    decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                    TotalDiscountedPrice -= (dollar / myDet.Quantity.GetValueOrDefault(0));
                    myDet.Discount2Percent = 0;
                    myDet.Discount2Dollar = dollar;
                    myDet.FinalPrice = 0;
                    if (Math.Round(TotalDiscountedPrice, 2) < 0)
                        status = string.Format("Item {0}: Discount amount cannot exceed {1}.", myDet.ItemNo, discountedPrice.ToString("N2"));
                }
                else if (Discount2.EndsWith("%"))
                {
                    decimal percent;
                    decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                    TotalDiscountedPrice *= (1 - percent / 100);
                    myDet.Discount2Percent = percent;
                    myDet.Discount2Dollar = 0;
                    myDet.FinalPrice = 0;
                }
            }
            #endregion

            myDet.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
            //decimal DiscPercent = ((RetailPrice - TotalDiscountedPrice) / (RetailPrice + 0.00001M)) * 100;
            decimal DiscPercent = ((RetailPrice - TotalDiscountedPrice) / RetailPrice) * 100;

            //update discount - IF NON PROMO
            //if (!myDet.IsPromo)
            //{
            myDet.Discount = DiscPercent;
            if (myDet.Discount != GetPreferredDiscount())
            {
                myDet.IsSpecial = true;
            }
            /*}
            else
            {
                myDet.Discount = 0;
                myDet.Discount2Dollar = 0;
                myDet.Discount2Percent = 0;
                myDet.DiscountDetail = "";
            }*/
            CalculateLineAmount(ref myDet);

            return true;
        }

        public bool ApplyDiscountLevel2FinalPrice(decimal FinalPrice, string LineID, out string status)
        {
            status = "";
            OrderDet myDet = GetLine(LineID, out status);
            if (myDet.UnitPrice == 0)
            {
                status = "Cannot apply discount to the $0 item";
                return false;
            }

            if (myDet.Item.IsNonDiscountable)
            {
                status = "";
                return true;
            }


            #region *) Get discounted price before Discount level 2
            decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
            if (myDet.IsPromo)
                discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * (decimal)myDet.PromoDiscount) / 100);
            string oldDiscount2 = "";
            if (!myDet.IsPromo)
            {
                if (!string.IsNullOrEmpty(myDet.DiscountDetail))
                {
                    string[] discounts = myDet.DiscountDetail.Split('+');
                    if (discounts.Length > 1)
                    {
                        oldDiscount2 = discounts[1];
                    }
                }
                if (!string.IsNullOrEmpty(oldDiscount2))
                {
                    if (oldDiscount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(oldDiscount2.TrimStart('$'), out dollar);
                        discountedPrice += (dollar / myDet.Quantity.GetValueOrDefault(0));
                    }
                    else if (oldDiscount2.EndsWith("%"))
                    {
                        decimal percent;
                        decimal.TryParse(oldDiscount2.TrimEnd('%'), out percent);
                        if (percent == 100)
                            discountedPrice = myDet.UnitPrice;
                        else
                            discountedPrice /= (1 - percent / 100);
                    }
                }
            }
            #endregion

            #region *) Calculate Discount1 + Discount2
            decimal RetailPrice = myDet.UnitPrice;
            decimal TotalDiscountedPrice = discountedPrice;
            //decimal discPercent1 = ((RetailPrice - discountedPrice) / (RetailPrice + 0.00001M)) * 100;
            decimal discPercent1 = ((RetailPrice - discountedPrice) / RetailPrice) * 100;
            string Discount1 = discPercent1.ToString("N0") + "%";
            if (FinalPrice > 0)
            {

                myDet.Discount2Percent = 0;
                myDet.Discount2Dollar = 0;
                myDet.FinalPrice = FinalPrice;
            }
            #endregion

            decimal DiscPercent = ((RetailPrice - FinalPrice) / RetailPrice) * 100;

            myDet.DiscountDetail = string.Format("{0}+${1}",Discount1, (discountedPrice - FinalPrice).ToString("N2")).TrimEnd('+');
            myDet.Discount = DiscPercent;
            if (myDet.Discount != GetPreferredDiscount())
            {
                myDet.IsSpecial = true;
            }
            
            CalculateLineAmount(ref myDet);

            return true;
        }

        public bool ApplyDiscountLevel2ToAll(string Discount2, out List<string> status)
        {
            status = new List<string>();
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                OrderDet myDet = myOrderDet[i];

                #region *) Get discounted price before Discount level 2
                decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
                if (myDet.IsPromo)
                    discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * (decimal)myDet.PromoDiscount) / 100);
                string oldDiscount2 = "";
                if (!string.IsNullOrEmpty(myDet.DiscountDetail))
                {
                    string[] discounts = myDet.DiscountDetail.Split('+');
                    if (discounts.Length > 1)
                    {
                        oldDiscount2 = discounts[1];
                    }
                }
                if (!string.IsNullOrEmpty(oldDiscount2))
                {
                    if (oldDiscount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(oldDiscount2.TrimStart('$'), out dollar);
                        discountedPrice += dollar;
                    }
                    else if (oldDiscount2.EndsWith("%"))
                    {
                        decimal percent;
                        decimal.TryParse(oldDiscount2.TrimEnd('%'), out percent);
                        if (percent == 100)
                            discountedPrice = myDet.UnitPrice;
                        else
                            discountedPrice /= (1 - percent / 100);
                    }
                }
                #endregion

                #region *) Calculate Discount1 + Discount2
                decimal RetailPrice = myDet.UnitPrice;
                decimal TotalDiscountedPrice = discountedPrice;
                //decimal discPercent1 = ((RetailPrice - discountedPrice) / (RetailPrice + 0.00001M)) * 100;
                decimal discPercent1 = ((RetailPrice - discountedPrice) / RetailPrice) * 100;
                string Discount1 = discPercent1.ToString("N0") + "%";
                if (!string.IsNullOrEmpty(Discount2))
                {
                    if (Discount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                        TotalDiscountedPrice -= dollar;
                        myDet.Discount2Percent = 0;
                        myDet.Discount2Dollar = dollar;
                        myDet.FinalPrice = 0;
                        if (Math.Round(TotalDiscountedPrice, 2) < 0)
                        {
                            status.Add(string.Format("Item {0}: Discount amount cannot exceed {1}.", myDet.ItemNo, discountedPrice.ToString("N2")));
                            continue;
                        }
                    }
                    else if (Discount2.EndsWith("%"))
                    {
                        decimal percent;
                        decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                        TotalDiscountedPrice *= (1 - percent / 100);
                        myDet.Discount2Percent = percent;
                        myDet.Discount2Dollar = 0;
                        myDet.FinalPrice = 0;
                    }
                }
                #endregion

                myDet.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
                //decimal DiscPercent = ((RetailPrice - TotalDiscountedPrice) / (RetailPrice + 0.00001M)) * 100;
                decimal DiscPercent = ((RetailPrice - TotalDiscountedPrice) / RetailPrice) * 100;

                //update discount - IF NON PROMO
                if (!myDet.IsPromo)
                {
                    myDet.Discount = DiscPercent;
                    if (myDet.Discount != GetPreferredDiscount())
                    {
                        myDet.IsSpecial = true;
                    }
                }
                else
                {
                    myDet.Discount = 0;
                }
                CalculateLineAmount(ref myDet);
            }

            return true;
        }

        public bool ApplyDiscountLevel2FinalPriceToAll(decimal FinalPrice, out List<string> status)
        {
            status = new List<string>();
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                OrderDet myDet = myOrderDet[i];

                #region *) Get discounted price before Discount level 2
                decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
                if (myDet.IsPromo)
                    discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * (decimal)myDet.PromoDiscount) / 100);
                string oldDiscount2 = "";
                if (!myDet.IsPromo)
                {
                    if (!string.IsNullOrEmpty(myDet.DiscountDetail))
                    {
                        string[] discounts = myDet.DiscountDetail.Split('+');
                        if (discounts.Length > 1)
                        {
                            oldDiscount2 = discounts[1];
                        }
                    }
                    if (!string.IsNullOrEmpty(oldDiscount2))
                    {
                        if (oldDiscount2.StartsWith("$"))
                        {
                            decimal dollar;
                            decimal.TryParse(oldDiscount2.TrimStart('$'), out dollar);
                            discountedPrice += (dollar / myDet.Quantity.GetValueOrDefault(0));
                        }
                        else if (oldDiscount2.EndsWith("%"))
                        {
                            decimal percent;
                            decimal.TryParse(oldDiscount2.TrimEnd('%'), out percent);
                            if (percent == 100)
                                discountedPrice = myDet.UnitPrice;
                            else
                                discountedPrice /= (1 - percent / 100);
                        }
                    }
                }
                #endregion

                #region *) Calculate Discount1 + Discount2
                decimal RetailPrice = myDet.UnitPrice;
                decimal TotalDiscountedPrice = discountedPrice;
                //decimal discPercent1 = ((RetailPrice - discountedPrice) / (RetailPrice + 0.00001M)) * 100;
                decimal discPercent1 = ((RetailPrice - discountedPrice) / RetailPrice) * 100;
                string Discount1 = discPercent1.ToString("N0") + "%";
                if (FinalPrice > 0)
                {

                    myDet.Discount2Percent = 0;
                    myDet.Discount2Dollar = 0;
                    myDet.FinalPrice = FinalPrice;
                }
                #endregion

                decimal DiscPercent = ((RetailPrice - FinalPrice) / RetailPrice) * 100;


                myDet.Discount = DiscPercent;
                if (myDet.Discount != GetPreferredDiscount())
                {
                    myDet.IsSpecial = true;
                }

                CalculateLineAmount(ref myDet);
            }

            return true;
        }

        public bool CheckDiscount1Privileges(string UserName, string LineID, string Discount1, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                OrderDet myDet = GetLine(LineID, out status);

                #region *) Validation
                if (string.IsNullOrEmpty(Discount1))
                    throw new Exception("Discount Invalid");
                decimal TotalDiscountedPrice = 0;
                if (Discount1.StartsWith("$"))
                {
                    TotalDiscountedPrice = myDet.UnitPrice
                                            - (Discount1.Replace("$", "").GetDecimalValue() / myDet.Quantity.GetValueOrDefault(0))
                                            - (myDet.Discount2Dollar / myDet.Quantity.GetValueOrDefault(0));
                }
                else if (Discount1.EndsWith("%"))
                {
                    if (Discount1.Replace("%", "").GetDecimalValue() < 0 ||
                        Discount1.Replace("%", "").GetDecimalValue() > 100)
                    {
                        throw new Exception("Discount percentage invalid");
                    }

                    TotalDiscountedPrice = myDet.UnitPrice
                        - (myDet.UnitPrice * (Discount1.Replace("%", "").GetDecimalValue() / 100))
                        - (myDet.Discount2Dollar / myDet.Quantity.GetValueOrDefault(0));
                }
                if (Math.Round(TotalDiscountedPrice, 2) < 0)
                {
                    throw new Exception(string.Format("Item {0}: Discount amount cannot exceed {1}.",
                        myDet.ItemNo,
                        "$" + (Discount1.Replace("$", "").GetDecimalValue()
                           + myDet.Discount2Dollar).ToString("N2")));
                }

                if (myDet.Discount2Percent > 0)
                {
                    TotalDiscountedPrice = TotalDiscountedPrice
                                           - (TotalDiscountedPrice * (myDet.Discount2Percent / 100));
                }

                #endregion

                decimal DiscPercent = ((myDet.UnitPrice - TotalDiscountedPrice) / myDet.UnitPrice) * 100;
                if (myDet.IsPromo)
                    DiscPercent = 0;
                UserMst um = new UserMst(UserMst.Columns.UserName, UserName);
                isSuccess = um.IsAbleToGiveDiscount(DiscPercent);
                status = "Discount exceeded discount limit in user privileges";
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckDiscount1PrivilegesToAll(string UserName, string Discount1, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                List<string> listStatus = new List<string>();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].UnitPrice != 0)
                    {
                        isSuccess = CheckDiscount1Privileges(UserName, myOrderDet[i].OrderDetID, Discount1, out status);
                        if (!isSuccess)
                            listStatus.Add(string.Format("Discount failed on item {0} : {1}", myOrderDet[i].ItemNo, status));
                    }
                }
                isSuccess = listStatus.Count == 0;
                status = string.Join("\n", listStatus.ToArray());
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckDiscount1PrivilegesToGlobal(string UserName, string type, decimal Discount1, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                List<string> listStatus = new List<string>();
                decimal total = CalculateGrossAmountExcludeLineDiscount();
                decimal DiscPercent = (Discount1 / total) * 100;
                UserMst um = new UserMst(UserMst.Columns.UserName, UserName);
                isSuccess = um.IsAbleToGiveDiscount(DiscPercent);
                status = "Discount exceeded discount limit in user privileges";
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckDiscount2Privileges(string UserName, string LineID, string Discount2, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                OrderDet myDet = GetLine(LineID, out status);

                #region *) Get discounted price before Discount level 2
                decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
                if (myDet.IsPromo)
                    discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * (decimal)myDet.PromoDiscount) / 100);
                string oldDiscount2 = "";
                if (!string.IsNullOrEmpty(myDet.DiscountDetail))
                {
                    string[] discounts = myDet.DiscountDetail.Split('+');
                    if (discounts.Length > 1)
                    {
                        oldDiscount2 = discounts[1];
                    }
                }
                if (!string.IsNullOrEmpty(oldDiscount2))
                {
                    if (oldDiscount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(oldDiscount2.TrimStart('$'), out dollar);
                        discountedPrice += (dollar / myDet.Quantity.GetValueOrDefault(0));
                    }
                    else if (oldDiscount2.EndsWith("%"))
                    {
                        decimal percent;
                        decimal.TryParse(oldDiscount2.TrimEnd('%'), out percent);
                        if (percent == 100)
                            discountedPrice = myDet.UnitPrice;
                        else
                            discountedPrice /= (1 - percent / 100);
                    }
                }
                #endregion

                #region *) Calculate Discount1 + Discount2
                decimal RetailPrice = myDet.UnitPrice;
                decimal TotalDiscountedPrice = discountedPrice;
                decimal discPercent1 = ((RetailPrice - discountedPrice) / RetailPrice) * 100;
                string Discount1 = discPercent1.ToString("N0") + "%";
                if (!string.IsNullOrEmpty(Discount2))
                {
                    if (Discount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                        TotalDiscountedPrice -= (dollar / myDet.Quantity.GetValueOrDefault(0));
                        if (Math.Round(TotalDiscountedPrice, 2) < 0)
                            throw new Exception(string.Format("Item {0}: Discount amount cannot exceed {1}.", myDet.ItemNo, discountedPrice.ToString("N2")));
                    }
                    else if (Discount2.EndsWith("%"))
                    {
                        decimal percent;
                        decimal.TryParse(Discount2.TrimEnd('%'), out percent);
                        TotalDiscountedPrice *= (1 - percent / 100);
                    }
                }
                #endregion

                decimal DiscPercent = ((RetailPrice - TotalDiscountedPrice) / RetailPrice) * 100;
                if (myDet.IsPromo)
                    DiscPercent = 0;
                UserMst um = new UserMst(UserMst.Columns.UserName, UserName);
                isSuccess = um.IsAbleToGiveDiscount(DiscPercent);
                status = "Discount exceeded discount limit in user privileges";
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckFinalPrivilegesToAll(string UserName, decimal FinalPrice, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                List<string> listStatus = new List<string>();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].UnitPrice != 0)
                    {
                        string Discount2 = "";
                        if (FinalPrice < 0)
                            Discount2 = "";
                        else
                        {
                            decimal discountedPrice = myOrderDet[i].UnitPrice - ((myOrderDet[i].UnitPrice * myOrderDet[i].Discount) / 100);
                            if (myOrderDet[i].IsPromo)
                                discountedPrice = myOrderDet[i].UnitPrice - ((myOrderDet[i].UnitPrice * (decimal)myOrderDet[i].PromoDiscount) / 100);

                            decimal discount = (discountedPrice * (myOrderDet[i].Quantity ?? 0)) - (FinalPrice * (myOrderDet[i].Quantity ?? 0));

                            if (discount > 0)
                                Discount2 = "$" + discount.ToString("N2");
                            else
                                Discount2 = "";

                        }
                        isSuccess = CheckDiscount2Privileges(UserName, myOrderDet[i].OrderDetID, Discount2, out status);
                        if (!isSuccess)
                            listStatus.Add(string.Format("Second Discount failed on item {0} : {1}", myOrderDet[i].ItemNo, status));

                    }
                }
                isSuccess = listStatus.Count == 0;
                status = string.Join("\n", listStatus.ToArray());
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckDiscount2PrivilegesToAll(string UserName, string Discount2, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                List<string> listStatus = new List<string>();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].UnitPrice != 0)
                    {
                        isSuccess = CheckDiscount2Privileges(UserName, myOrderDet[i].OrderDetID, Discount2, out status);
                        if (!isSuccess)
                            listStatus.Add(string.Format("Second Discount failed on item {0} : {1}", myOrderDet[i].ItemNo, status));
                    }
                }
                isSuccess = listStatus.Count == 0;
                status = string.Join("\n", listStatus.ToArray());
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckPriceLevelPrivileges(string UserName, string LineID, string DiscountName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                OrderDet myDet = GetLine(LineID, out status);

                Query qr = new Query("SpecialDiscountDetail");
                qr.AddWhere(SpecialDiscountDetail.Columns.ItemNo, Comparison.Equals, myDet.ItemNo);
                qr.AddWhere(SpecialDiscountDetail.Columns.DiscountName, Comparison.Equals, DiscountName);
                var spDiscDet = new SpecialDiscountDetailController()
                                    .FetchByQuery(qr)
                                    .Where(o => o.Deleted.GetValueOrDefault(false) == false)
                                    .FirstOrDefault();
                if (spDiscDet != null)
                {
                    string Discount1 = string.Format("${0}", spDiscDet.DiscountAmount.GetValueOrDefault(0).ToString("N2"));
                    isSuccess = CheckDiscount1Privileges(UserName, LineID, Discount1, out status);
                }
                else
                {
                    isSuccess = true;
                    status = "";
                }
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public bool CheckPriceLevelPrivilegesToAll(string UserName, string DiscountName, out string status)
        {
            bool isSuccess = false;
            status = "";

            try
            {
                List<string> listStatus = new List<string>();
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    isSuccess = CheckPriceLevelPrivileges(UserName, myOrderDet[i].OrderDetID, DiscountName, out status);
                    if (!isSuccess)
                        listStatus.Add(string.Format("Apply Price Level failed on item {0} : {1}", myOrderDet[i].ItemNo, status));
                }
                isSuccess = listStatus.Count == 0;
                status = string.Join("\n", listStatus.ToArray());
            }
            catch (Exception ex)
            {
                status = "Error : " + ex.Message;
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public decimal RoundDownNearestFiveCent(decimal input)
        {
            decimal TotalAmount = input;

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;
            if (temp > 5)
            {

                TotalAmount = TotalAmount - temp * d + 0.05M; //Direct round down                                
            }
            else if (temp > 0 && temp < 5)
            {
                TotalAmount = TotalAmount - temp * d; //Direct round down                                
            }

            return TotalAmount;
        }
    }
}