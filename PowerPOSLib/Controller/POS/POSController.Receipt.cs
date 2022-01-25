using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using SubSonic;
using PowerPOS.Container;
using System.Linq;

namespace PowerPOS
{
    public partial class POSController
    {
        public const string ROUNDING_ITEM = "R0001";
        public const string EXTRACHARGE_ITEM = "EXTRA_CHARGE";
        public static string RoundingPreference;
        public ReceiptHdr recHdr;
        public ReceiptDetCollection recDet;

        public const string PAY_NETSATMCard = "NETS ATM Card";
        public const string PAY_NETSFlashPay = "NETS FlashPay";
        public const string PAY_NETSCashCard = "NETS CashCard";
        public const string PAY_NETSQR = "NETS QR";

        public decimal RoundUpNearestTenCent()
        {
            string status;

            decimal TotalAmount = CalculateTotalAmount(out status);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;

            if (temp > 0)
            {
                TotalAmount = TotalAmount - (temp * d) +0.1M;
            }

            return TotalAmount;
        }        
        public decimal RoundNearestTenCent()
        {
            string status;

            decimal TotalAmount = CalculateTotalAmount(out status);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;
            
            if (temp > 5)
            {
                TotalAmount = TotalAmount - temp * d + 0.10M; //Direct round down                                
            }
            else if (temp > 0 && temp <= 5)
            {
                TotalAmount = TotalAmount - temp * d; //Direct round down                                
            }
            
            return TotalAmount;
        }

        public decimal RoundNearestFiveCent()
        {
            string status;

            decimal TotalAmount = CalculateTotalAmount(out status);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;
            temp = temp % 5;
            if (temp > 2)
            {
                TotalAmount = TotalAmount - temp * d + 0.05M; //Direct round down                                
            }
            else if (temp > 0 && temp <= 5)
            {
                TotalAmount = TotalAmount - temp * d; //Direct round down                                
            }

            return TotalAmount;
        }

        public decimal RoundDownNearestTenCent()
        {
            string status;

            decimal TotalAmount = Math.Round(CalculateTotalAmount(out status), 2, MidpointRounding.AwayFromZero);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;
            TotalAmount = TotalAmount - temp * d;
            /*
            if (temp > 5)
            {
                TotalAmount = TotalAmount - temp * d + 0.05M; //Direct round down                                
            }
            else if (temp > 0 && temp < 5)
            {
                TotalAmount = TotalAmount - temp * d; //Direct round down                                
            }
            */
            return TotalAmount;
        }        
        public decimal RoundUpNearestFiveCent()
        {
            string status;

            decimal TotalAmount = Math.Round(CalculateTotalAmount(out status),2,MidpointRounding.AwayFromZero);
            
            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;
            if (temp > 5)
            {

                TotalAmount = TotalAmount - temp * d + 0.10M; //Direct round down                                
            }
            else if (temp > 0 && temp < 5)
            {

                TotalAmount = TotalAmount - temp * d + 0.05M; //Direct round down                                
            
                //TotalAmount = TotalAmount - temp * d; //Direct round down                                
            }

            return TotalAmount;
        }
        public decimal RoundDownNearestFiveCent()
        {
            //
            string status;

            decimal TotalAmount = Math.Round(CalculateTotalAmount(out status), 2, MidpointRounding.AwayFromZero);

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

        public decimal RoundNearestDollar()
        {
            string status;

            decimal TotalAmount = Math.Round(CalculateTotalAmount(out status), 2, MidpointRounding.AwayFromZero);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            if (temp < 0.5M)
                TotalAmount = TotalAmount - temp;
            else
                TotalAmount = (TotalAmount - temp) + 1;

            return TotalAmount;
        }

        public decimal RoundDownNearestDollar()
        {
            string status;

            decimal TotalAmount = Math.Round(CalculateTotalAmount(out status), 2, MidpointRounding.AwayFromZero);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            TotalAmount = TotalAmount - temp;

            return TotalAmount;
        }

        public decimal RoundUpNearestDollar()
        {
            string status;

            decimal TotalAmount = Math.Round(CalculateTotalAmount(out status), 2, MidpointRounding.AwayFromZero);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            TotalAmount = (TotalAmount - temp) + (temp > 0 ? 1 : 0);
            return TotalAmount;
        }

        public decimal RoundTotalReceiptAmount()
        {

            //Choose rounding mode
            if (RoundingPreference == "RoundDownNearestFiveCent")
            {
                return RoundDownNearestFiveCent();
            }
            if (RoundingPreference == "RoundUpNearestFiveCent")
            {
                return RoundUpNearestFiveCent();
            }
            if (RoundingPreference == "RoundDownNearestTenCent")
            {
                return RoundDownNearestTenCent();
            }
            if (RoundingPreference == "RoundUpNearestTenCent")
            {
                return RoundUpNearestTenCent();
            }
            if (RoundingPreference == "RoundNearestTenCent")
            {
                return RoundNearestTenCent();
            }
            if (RoundingPreference == "RoundNearestFiveCent")
            {
                return RoundNearestFiveCent();
            }
            if (RoundingPreference == "RoundDownNearestDollar")
            {
                return RoundDownNearestDollar();
            }
            if (RoundingPreference == "RoundUpNearestDollar")
            {
                return RoundUpNearestDollar();
            }
            if (RoundingPreference == "RoundNearestDollar")
            {
                return RoundNearestDollar();
            }


            return RoundDownNearestFiveCent(); //default rounding
        }

        public void SetTotalReceiptAmount(decimal p)
        {
            string status;
            decimal total = CalculateTotalAmount(out status);
            if (total > 0)
                myOrderHdr.Discount = ((total - p) / total) * 100;
            //throw new Exception("The method or operation is not implemented.");
        }


        public decimal CalculateTotalAmount(out string status)
        {
            try
            {
                status = "";
                decimal TotalAmount = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided
                        && !myOrderDet[i].IsPromoFreeOfCharge)
                    {
                        if (myOrderDet[i].IsPromo)
                            TotalAmount += myOrderDet[i].PromoAmount;
                        else
                            TotalAmount += myOrderDet[i].Amount;
                    }
                }
                //TotalAmount = TotalAmount * (decimal)(1 + GST / 100);

                //Calculate overall discount
                TotalAmount = TotalAmount * (decimal)(1 - myOrderHdr.Discount / 100);

                //return CommonUILib.RemoveRoundUp(TotalAmount);
                return TotalAmount;

            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        public decimal CalculateAmountForPayment(string paymentType)
        {
            string stat = "";
            decimal amt = CalculateTotalAmount(out stat);

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.RoundingForAllPayment), false) || paymentType.ToUpper() == "CASH")
            {
                amt = RoundTotalReceiptAmount();
            }
            return amt;
            

        }

        /*
        public decimal CalculateTotalDiscount(out string status)
        {
            //return CalculateTotalPaid  CalculateTotalAmount; 
            
            status = "";
            try
            {
                decimal TotalDiscount = 0;

                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!(bool)myOrderDet[i].IsVoided)
                        TotalDiscount += CalculateDiscountInDollar(i);
                }

                //TotalDiscount = TotalDiscount * (decimal)(1 + GST / 100);

                //Calculate overall discount
                TotalDiscount = TotalDiscount * (decimal)(1 - myOrderHdr.Discount / 100);

                //round down to the closest 5 cent
                decimal temp = TotalDiscount % 1;
                temp = temp * 10;
                temp = temp % 1;
                temp = temp * 10;

                decimal d = 0.01M;

                if (temp > 5)
                {
                    TotalDiscount = TotalDiscount + ((10 - temp) * d); //round up
                    //total = total - ((temp - 5) * d);    //round down
                }
                else
                {
                    if (temp > 0)
                    {
                        TotalDiscount = TotalDiscount + ((5 - temp) * d); //round up
                        //total = total - (temp * d);    //round down                
                    }
                }
                return TotalDiscount;

            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
              
        }
        */
        public decimal CalculateTotalPaid(out string status)
        {
            status = "";
            try
            {
                decimal TotalPaid = 0;

                for (int i = 0; i < recDet.Count; i++)
                {
                    if (recDet[i].PaymentType != POSController.PAY_PACKAGE)
                        TotalPaid += recDet[i].Amount;
                }
                recHdr.Amount = TotalPaid;
                return TotalPaid;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// Total amount of receipt(s) that is paid by points
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        /// </remarks>
        public decimal CalculateTotalPaid_ByPoints(out string status)
        {
            status = "";
            try
            {
                decimal TotalPaid = 0;

                for (int i = 0; i < recDet.Count; i++)
                {
                    if (recDet[i].PaymentType == POSController.PAY_POINTS)
                        TotalPaid += recDet[i].Amount;
                }
                //recHdr.Amount = TotalPaid;
                return TotalPaid;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        /// <summary>
        /// Total amount of receipt(s) that is paid by points
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        /// </remarks>
        public decimal CalculateTotalPaid_ByPointsByName(string itemno, out string status)
        {
            status = "";
            try
            {
                decimal TotalPaid = 0;

                for (int i = 0; i < recDet.Count; i++)
                {
                    if (recDet[i].PaymentType == POSController.PAY_POINTS && recDet[i].PointItemNo == itemno)
                        TotalPaid += recDet[i].Amount;
                }
                //recHdr.Amount = TotalPaid;
                return TotalPaid;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        public DataTable FetchUnsavedReceipt()
        {
            return FetchUnsavedReceipt(false);
        }

        public DataTable FetchUnsavedReceipt(bool withPointName)
        {
            DataTable dt = recDet.ToDataTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Amount"] = decimal.Parse(dt.Rows[i]["Amount"].ToString()).ToString("N2");
            }

            if (withPointName)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["PaymentType"].ToString() == POSController.PAY_POINTS && dr["userfld1"].ToString() != "")
                    {
                        Item itm = new Item(dr["userfld1"].ToString());
                        if (itm != null && itm.ItemNo == dr["userfld1"].ToString())
                        {
                            dr["PaymentType"] = dr["PaymentType"].ToString() + " (" + itm.ItemName + ")";
                        }
                    }
                }
            }

            return dt;
        }

        public ReceiptDetCollection FetchUnsavedReceipt(out string status)
        {
            status = "";
            return recDet;
        }

        public bool AddChequeReceiptLinePayment(decimal paymentAmt,
            string ChequeNo, string BankName,
            out string status)
        {
            try
            {
                status = "";

                decimal OrderAmount = CalculateTotalAmount(out status);
                decimal TotalPaid = CalculateTotalPaid(out status);

                if (status != "")
                {
                    status = "Error while calculating total amount: " + status;
                    return false;
                }

                //Validate txtAmount
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false))
                {
                    if (paymentAmt <= 0)
                    {
                        status = "Payment must be bigger than 0";
                        return false;
                    }
                }

                //check if total amount exceed the balance                        
                if (Math.Abs(Math.Round(TotalPaid + paymentAmt, 2)) > Math.Abs(OrderAmount))
                {
                    //just reject
                    status = "The amount you entered exceeds the amount needed.";
                    return false;
                }

                //Add the amount into receipt detail collection
                ReceiptDet recDetTmp = new ReceiptDet();

                recDetTmp.Amount = paymentAmt;
                recDetTmp.Userfld1 = ChequeNo;
                recDetTmp.Userfld2 = BankName;

                recDetTmp.ReceiptDetID = recHdr.ReceiptHdrID + "." + recDet.Count;
                recDetTmp.PaymentType = POSController.PAY_CHEQUE;

                recDet.Add(recDetTmp);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }

        }

        public decimal GetAmountReceiptLinePaymentType(string paymentType)
        {
            decimal paymentAmt = new decimal(0);
            for (int i = 0; i < recDet.Count; i++)
            {
                if (recDet[i].PaymentType == paymentType)
                    paymentAmt += recDet[i].Amount;
            }

            return paymentAmt;
        }
        

        public bool AddReceiptLinePayment
            (decimal paymentAmt, string paymentType, string paymentrefno,
            decimal foreignCurrencyRate, string foreignCurrencyCode, decimal foreignCurrencyAmount,
             out decimal change, out string status)
        {
            return AddReceiptLinePayment
                (paymentAmt, paymentType, paymentrefno,
                foreignCurrencyRate, foreignCurrencyCode, foreignCurrencyAmount,
                out change, out status, false);
        }

        public bool AddReceiptLinePayment
            (decimal paymentAmt, string paymentType, string paymentrefno,
            decimal foreignCurrencyRate, string foreignCurrencyCode, decimal foreignCurrencyAmount,
             out decimal change, out string status, bool doNotRoundCash)
        {
            return AddReceiptLinePayment
                (paymentAmt, paymentType, paymentrefno,
                foreignCurrencyRate, foreignCurrencyCode, foreignCurrencyAmount,
                out change, out status, doNotRoundCash, "");
        }

        public bool AddReceiptLinePayment
            (decimal paymentAmt, string paymentType, string paymentrefno,
            decimal foreignCurrencyRate, string foreignCurrencyCode, decimal foreignCurrencyAmount,
             out decimal change, out string status, bool doNotRoundCash, string netsResponseInfo)
        {
            ////////////////////////////////////////////////////////
            /// DEFINITION:                                      ///
            /// ------------------------------------------------ ///
            /// OrderAmount -> Total amount that need to be paid ///
            /// TotalPaid   -> Total amount that has been paid   ///
            /// paymentAmt  -> amount of current payment         ///
            ////////////////////////////////////////////////////////

            try
            {
                #region *) Initialize: Fill default output parameters
                change = 0;
                status = "";
                #endregion

                status = "";
                if (paymentType != PAY_VOUCHER && paymentType != PAY_PAMEDIFUND && paymentType != PAY_SMF)
                {
                    #region *) Validation: Payment Type has not been on the list [Return if False]
                    for (int i = 0; i < recDet.Count; i++)
                    {
                        if (recDet[i].PaymentType == paymentType)
                            throw new Exception("(error)The payment type: " + paymentType + ", has already been specified.");
                    }
                    #endregion
                }

                decimal OrderAmount;
                #region *) Initialize: Get total amount that SHOULD be paid
                if (paymentType != PAY_CASH || doNotRoundCash)
                {
                    /// Calculate Total Amount
                    OrderAmount = CalculateTotalAmount(out status);
                }
                else
                {
                    /// Calculate Total Amount and Round Down by 5 cents
                    OrderAmount = RoundTotalReceiptAmount();
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.RoundingForAllPayment),false))
                {
                    OrderAmount = RoundTotalReceiptAmount();
                }
                #endregion

                decimal TotalPaid;
                #region *) Initialize: Get total amount that HAS BEEN paid AndAlso Update to ReceiptHdr.Amount
                TotalPaid = CalculateTotalPaid(out status);
                if (status != "")
                    throw new Exception("(error)Error while calculating total amount: " + status);
                #endregion

                #region *) Validation: Payment must be bigger than 0 [Exit if False]
                //Validate txtAmount
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false))
                {
                    if (paymentAmt <= 0 && !this.HasCreditNote())
                        throw new Exception("(warning)Payment must be bigger than 0");
                }
                #endregion

                OrderAmount = Decimal.Parse(OrderAmount.ToString("N2"));
                #region *) Validation: Total paid cannot be more than remaining balance (Except Cash will add Change)
                if (Math.Abs(Math.Round(TotalPaid + paymentAmt, 2)) > Math.Abs(OrderAmount))
                {
                    //Accept if voucher payment is more than what is wanted.
                    if (paymentType != POSController.PAY_VOUCHER &&
                        paymentType != POSController.PAY_CASH
                        && paymentType != POSController.PAY_FOREIGN_CURRENCY
                        && !paymentType.StartsWith(POSController.PAY_CASH+"-"))
                    {
                        throw new Exception("(warning)The amount you entered exceeds the amount needed.");
                    }
                    else if (paymentType == POSController.PAY_CASH
                        || paymentType.StartsWith(POSController.PAY_CASH + "-"))
                    {
                        change = TotalPaid + paymentAmt - OrderAmount;

                        paymentAmt = OrderAmount - TotalPaid;
                        //Validate txtAmount
                        if (paymentAmt <= 0)
                        {
                            throw new Exception("(warning)You are not allowed to make anymore payment.");
                        }
                        decimal maxChangeAllowed = 0;
                        if (!decimal.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.MaxChangeAllowed),out maxChangeAllowed ))
                        {
                            maxChangeAllowed = 1000;
                        }
                        if (change > maxChangeAllowed)
                        {
                            throw new Exception("Change can't more than " + maxChangeAllowed.ToString());
                            //return false;
                        }
                    }
                }
                #endregion

                decimal ExtraCharge;
                #region *) Initialize: Get Extra Charge amount
                ExtraCharge = CheckExtraChargeAmount(paymentType, paymentAmt);
                #endregion

                #region *) Core: Create / Update Extra Charge information in OrderDetCollection
                AddExtraCharge(ExtraCharge);
                #endregion

                #region *) Core: Create new ReceiptDet record
                ReceiptDet recDetTmp = new ReceiptDet();
                recDetTmp.Amount = paymentAmt + ExtraCharge;
                recDetTmp.PaymentRefNo = paymentrefno;
                recDetTmp.ReceiptDetID = recHdr.ReceiptHdrID + "." + recDet.Count;
                recDetTmp.PaymentType = paymentType;
                recDetTmp.ExtraChargeAmount = ExtraCharge;
                recDetTmp.ForeignCurrencyCode = foreignCurrencyCode;
                recDetTmp.ForeignCurrencyRate = foreignCurrencyRate;
                recDetTmp.ForeignAmountReceived = foreignCurrencyAmount;
                if (paymentType == POSController.PAY_CASH
                        || paymentType.StartsWith(POSController.PAY_CASH + "-"))
                {
                    recDetTmp.Change = change;
                    recDetTmp.ForeignAmountReturned = foreignCurrencyRate == 0 ? 0 : (change / foreignCurrencyRate);
                }
                recDetTmp.NETSResponseInfo = netsResponseInfo;
                recDet.Add(recDetTmp);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                change = 0;
                status = ex.Message;
                return false;
            }
        }

        public bool AddReceiptLinePaymentWithRemark
            (decimal paymentAmt, string paymentType, string Remark,
             out decimal change, out string status)
        {
            ////////////////////////////////////////////////////////
            /// DEFINITION:                                      ///
            /// ------------------------------------------------ ///
            /// OrderAmount -> Total amount that need to be paid ///
            /// TotalPaid   -> Total amount that has been paid   ///
            /// paymentAmt  -> amount of current payment         ///
            ////////////////////////////////////////////////////////

            try
            {
                #region *) Initialize: Fill default output parameters
                change = 0;
                status = "";
                #endregion

                status = "";
                if (paymentType != PAY_VOUCHER)
                {
                    #region *) Validation: Payment Type has not been on the list [Return if False]
                    for (int i = 0; i < recDet.Count; i++)
                    {
                        if (recDet[i].PaymentType == paymentType)
                            throw new Exception("(error)The payment type: " + paymentType + ", has already been specified.");
                    }
                    #endregion
                }

                decimal OrderAmount;
                #region *) Initialize: Get total amount that SHOULD be paid
                if (paymentType != PAY_CASH)
                {
                    /// Calculate Total Amount
                    OrderAmount = CalculateTotalAmount(out status);
                }
                else
                {
                    /// Calculate Total Amount and Round Down by 5 cents
                    OrderAmount = RoundTotalReceiptAmount();
                }
                #endregion

                decimal TotalPaid;
                #region *) Initialize: Get total amount that HAS BEEN paid AndAlso Update to ReceiptHdr.Amount
                TotalPaid = CalculateTotalPaid(out status);
                if (status != "")
                    throw new Exception("(error)Error while calculating total amount: " + status);
                #endregion

                #region *) Validation: Payment must be bigger than 0 [Exit if False]
                //Validate txtAmount
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false))
                {
                    if (paymentAmt <= 0)
                        throw new Exception("(warning)Payment must be bigger than 0");
                }
                #endregion

                OrderAmount = Decimal.Parse(OrderAmount.ToString("N2"));

                #region *) Validation: Total paid cannot be more than remaining balance (Except Cash will add Change)
                if (Math.Round(TotalPaid + paymentAmt, 2) > OrderAmount)
                {
                    //Accept if voucher payment is more than what is wanted.
                    if (paymentType != POSController.PAY_VOUCHER &&
                        paymentType != POSController.PAY_CASH
                        && paymentType != POSController.PAY_FOREIGN_CURRENCY)
                    {
                        throw new Exception("(warning)The amount you entered exceeds the amount needed.");
                    }
                    else if (paymentType == POSController.PAY_CASH)
                    {
                        change = TotalPaid + paymentAmt - OrderAmount;
                        paymentAmt = OrderAmount - TotalPaid;
                        //Validate txtAmount
                        if (paymentAmt <= 0)
                        {
                            throw new Exception("(warning)You are not allowed to make anymore payment.");
                        }
                    }
                }
                #endregion

                decimal ExtraCharge;
                #region *) Initialize: Get Extra Charge amount
                ExtraCharge = CheckExtraChargeAmount(paymentType, paymentAmt);
                #endregion

                #region *) Core: Create / Update Extra Charge information in OrderDetCollection
                AddExtraCharge(ExtraCharge);
                #endregion

                #region *) Core: Create new ReceiptDet record
                ReceiptDet recDetTmp = new ReceiptDet();
                recDetTmp.Amount = paymentAmt + ExtraCharge;
                recDetTmp.Userfld1 = Remark;
                recDetTmp.ReceiptDetID = recHdr.ReceiptHdrID + "." + recDet.Count;
                recDetTmp.PaymentType = paymentType;
                recDetTmp.ExtraChargeAmount = ExtraCharge;
                if (paymentType == POSController.PAY_CASH)
                {
                    recDetTmp.Change = change;
                }
                recDet.Add(recDetTmp);
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                change = 0;
                status = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentAmt"></param>
        /// <param name="PaymentRefNo"></param>
        /// <param name="PointType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        /// </remarks>
        public bool AddReceiptLinePayment_Points
            (decimal paymentAmt, string PaymentRefNo, string PointItemNo, out string status)
        {
            ////////////////////////////////////////////////////////
            /// DEFINITION:                                      ///
            /// ------------------------------------------------ ///
            /// OrderAmount -> Total amount that need to be paid ///
            /// TotalPaid   -> Total amount that has been paid   ///
            /// paymentAmt  -> amount of current payment         ///
            ////////////////////////////////////////////////////////

            decimal OrderAmount;
            decimal TotalPaid;

            try
            {
                #region *) Initialize: Fill default output parameters
                status = "";
                #endregion

                #region *) Initialize: Get OrderAmount
                OrderAmount = CalculateTotalAmount(out status);
                OrderAmount = Decimal.Parse(OrderAmount.ToString("N2"));
                #endregion

                #region *) Initialize: Get TotalPaid AndAlso Update to ReceiptHdr.Amount 
                TotalPaid = CalculateTotalPaid(out status);
                if (status != "")
                    throw new Exception("(error)Error while calculating total amount: " + status);
                #endregion

                #region *) Validation: Current Payment must be bigger than 0 
                if (paymentAmt == 0)
                    throw new Exception("(warning)Payment must be bigger than 0");
                #endregion

                #region *) Validation: Total Payment cannot exceed OrderAmount
                if (Math.Abs(TotalPaid + paymentAmt) > Math.Abs(OrderAmount))
                    throw new Exception("(warning)The amount you entered exceeds the amount needed.");
                #endregion

                decimal ExtraCharge;
                #region *) Initialize: Get Extra Charge amount
                ExtraCharge = CheckExtraChargeAmount(POSController.PAY_POINTS, paymentAmt);
                #endregion

                #region *) Core: Create / Update Extra Charge information in OrderDetCollection
                AddExtraCharge(ExtraCharge);
                #endregion

                #region *) Core: Add new ReceiptDet
                ReceiptDet recDetTmp = new ReceiptDet();
                recDetTmp.Amount = paymentAmt + ExtraCharge;
                recDetTmp.PaymentRefNo = PaymentRefNo;
                recDetTmp.ReceiptDetID = recHdr.ReceiptHdrID + "." + recDet.Count;
                recDetTmp.PaymentType = POSController.PAY_POINTS;
                recDetTmp.PointItemNo = PointItemNo;
                recDetTmp.ExtraChargeAmount = ExtraCharge;
                recDet.Add(recDetTmp);
                #endregion

                return true;
            }
            catch (Exception X)
            {
                status = X.Message;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paymentAmt"></param>
        /// <param name="paymentrefno"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Need to change the recDetTmp.PointItemNo to record ItemNo
        /// </remarks>
        public bool AddReceiptLinePayment_Package
           (decimal paymentAmt, string paymentrefno, out string status)
        {
            ////////////////////////////////////////////////////////
            /// DEFINITION:                                      ///
            /// ------------------------------------------------ ///
            /// OrderAmount -> Total amount that need to be paid ///
            /// TotalPaid   -> Total amount that has been paid   ///
            /// paymentAmt  -> amount of current payment         ///
            ////////////////////////////////////////////////////////

            try
            {
                #region *) Initialize: Fill default output parameters
                status = "";
                #endregion

                #region *) Validation: Cannot have more than 1 PAY_PACKAGE receipt 
                for (int i = 0; i < recDet.Count; i++)
                {
                    if (recDet[i].PaymentType == PAY_PACKAGE)
                        throw new Exception("(warning)The payment type: " + PAY_PACKAGE + ", has already been specified.");
                }
                #endregion

                decimal OrderAmount;
                #region *) Initialize: Get OrderAmount
                OrderAmount = paymentAmt;
                OrderAmount = Decimal.Parse(OrderAmount.ToString("N2"));
                #endregion

                #region *) Validation: Current Payment must be bigger than 0
                /*if (paymentAmt <= 0)
                    throw new Exception("(warning)Payment must be bigger than 0");*/
                #endregion

                decimal ExtraCharge;
                #region *) Initialize: Get Extra Charge amount
                ExtraCharge = CheckExtraChargeAmount(PAY_PACKAGE, paymentAmt);
                #endregion

                #region *) Core: Create / Update Extra Charge information in OrderDetCollection
                AddExtraCharge(ExtraCharge);
                #endregion

                #region *) Core: Add new ReceiptDet
                ReceiptDet recDetTmp = new ReceiptDet();
                recDetTmp.Amount = paymentAmt + ExtraCharge;
                recDetTmp.PaymentRefNo = paymentrefno;
                recDetTmp.ReceiptDetID = recHdr.ReceiptHdrID + "." + recDet.Count;
                recDetTmp.PaymentType = PAY_PACKAGE;
                recDetTmp.PointItemNo = "COURSE";   // TODO: Please change this to ItemNo
                recDetTmp.ExtraChargeAmount = ExtraCharge;
                recDet.Add(recDetTmp);
                #endregion

                return true;
            }
            catch (Exception X)
            {
                status = X.Message;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReceiptDetID"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        /// </remarks>
        public string getReceiptPaymentType(string ReceiptDetID)
        {
            try
            {
                ReceiptDet tmpDet = (ReceiptDet)recDet.Find(ReceiptDetID);
                return tmpDet.PaymentType;
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                return null;
            }
        }

        public bool RemoveReceiptByPaymentType(string paymentType)
        {
            var listRecDet = (from o in recDet
                              where o.PaymentType == paymentType
                              select o.ReceiptDetID).ToList();

            bool isSuccess = false;

            foreach (string recDetID in listRecDet)
            {
                DeleteReceiptLinePayment(recDetID);
            }
            isSuccess = true;
            return isSuccess;

        }

        public bool DeleteReceiptLinePayment(string ReceiptDetID)
        {
            ReceiptDet tmpDet = (ReceiptDet)recDet.Find(ReceiptDetID);
            recDet.Remove(tmpDet);

            string OrderDetID = IsItemIsInOrderLine(EXTRACHARGE_ITEM);

            decimal InitialValue = GetTotalExtraCharge();

            string status = "";
            if (OrderDetID != "")
                ChangeOrderLineUnitPrice(OrderDetID, InitialValue, out status);

            return true;
        }
        public bool DeleteAllReceiptLinePayment()
        {
            recDet = new ReceiptDetCollection();

            for (int Counter = myOrderDet.Count - 1; Counter >= 0; Counter--)
                if (myOrderDet[Counter].ItemNo == EXTRACHARGE_ITEM)
                    myOrderDet.RemoveAt(Counter);
            
            return true;
        }
        public decimal CheckExtraChargeAmount(string paymentType, decimal paymentAmount)
        {
            decimal ExtraChargeTotalAmount = 
                AppSetting.CastDecimal(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ExtraChargeAmount + paymentType), 0);

            string ExtraChargeType = 
                AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ExtraChargeType + paymentType));

            if (ExtraChargeType.ToLower() != "amount")
                /* Meaning: It is a percentage extra charge by default */
                ExtraChargeTotalAmount = ExtraChargeTotalAmount * paymentAmount / 100;

            return ExtraChargeTotalAmount;
        }

        /// <summary>
        /// Recalculate the extra charge from recorded RecDet and add the Amount
        /// </summary>
        /// <param name="Amount">The amount to be added</param>
        public void AddExtraCharge(decimal Amount)
        {
            string status = "";

            string OrderDetID = IsItemIsInOrderLine(EXTRACHARGE_ITEM);

            if (Amount != 0 && OrderDetID == "")
            {
                if (!AddItemToOrder(new Item(EXTRACHARGE_ITEM), 1, 0, false, out status))
                    throw new Exception("(error)" + status);

                OrderDetID = IsItemIsInOrderLine(EXTRACHARGE_ITEM);
            }

            decimal InitialValue = GetTotalExtraCharge();

            if (OrderDetID != "")
                ChangeOrderLineUnitPrice(OrderDetID, InitialValue + Amount, out status);
        }

        public decimal GetTotalExtraCharge()
        {
            decimal TotalExtraCharge = 0;
            for (int Counter = 0; Counter < recDet.Count; Counter++)
                TotalExtraCharge += recDet[Counter].ExtraChargeAmount.GetValueOrDefault(0);

            return TotalExtraCharge;
        }

        public static decimal GetDepositBalance(string OrderHdrID, out string status)
        {
            status = "";
            try
            {
                ViewOrderHdrInstOutstandingBal oh = new ViewOrderHdrInstOutstandingBal("OrderHdrID", OrderHdrID);
                if (string.IsNullOrEmpty(oh.OrderHdrID))
                {
                    status = "Record not found";
                    return -1;
                }
                else
                {
                    return oh.NettAmount - oh.InstOutstandingBal;
                }
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        public static decimal GetAssignedDeposit(string OrderHdrID, out string status)
        {
            status = "";
            try
            {
                decimal totalAssignedDeposit = 0;
                OrderDetCollection od = new OrderDetCollection();
                od.Where(OrderDet.Columns.OrderHdrID, OrderHdrID);
                od.Where(OrderDet.Columns.IsVoided, false);
                od.Load();

                if (od.Count > 0)
                {
                    for (int i = 0; i < od.Count; i++)
                    {
                        totalAssignedDeposit += od[i].DepositAmount;
                    }
                }

                return totalAssignedDeposit;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        public decimal GetCashReceived() {
            decimal cashReceived = 0;

            for (int i = 0; i < recDet.Count; i++)
            {
                if (recDet[i].PaymentType == PAY_CASH)
                {
                    cashReceived = recDet[i].Amount + recDet[i].Change.GetValueOrDefault(0);
                }
            }

            return cashReceived;
        }

        public decimal GetChange()
        {
            decimal cashReceived = 0;

            for (int i = 0; i < recDet.Count; i++)
            {
                if (recDet[i].PaymentType == PAY_CASH)
                {
                    cashReceived = recDet[i].Change.GetValueOrDefault(0);
                }
            }

            return cashReceived;
        } 

        #region z2closing

        public decimal CalculateSubTotalAmount(out string status)
        {
            try
            {
                status = "";
                decimal TotalAmount = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided
                        && !myOrderDet[i].IsPromoFreeOfCharge)
                    {
                        if (myOrderDet[i].IsPromo)
                            TotalAmount += myOrderDet[i].PromoAmount;
                        else
                            TotalAmount += myOrderDet[i].Amount;
                    }
                }

                return TotalAmount;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return -1;
            }
        }

        #endregion
    }
}
