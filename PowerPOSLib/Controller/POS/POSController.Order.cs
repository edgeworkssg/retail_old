using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SubSonic;
using PowerPOS.Container;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Configuration;

namespace PowerPOS
{

    public delegate void OpenPriceItemHandler(object sender, string orderDetID);

    public delegate void OpenPriceItemEditFieldHandler(object sender, string orderDetID, bool IsUsingQuantityField);

    public delegate void AutoCaptureWeightItemHandler(object sender, string orderDetID);

    public partial class POSController
    {
        [field: NonSerializedAttribute()]
        public event OpenPriceItemHandler OpenPriceItemAdded;
        [field: NonSerializedAttribute()]
        public event OpenPriceItemHandler OpenPriceItemAddedHotKey;
        [field: NonSerializedAttribute()]
        public event OpenPriceItemEditFieldHandler OpenPriceItemAddedEditField;
        [field: NonSerializedAttribute()]
        public event AutoCaptureWeightItemHandler AutoCaptureWeightItemAdded;

        public OrderHdr myOrderHdr;             //Order Header        
        public OrderDetCollection myOrderDet;   //Order Detail
        public PreOrderRecord preOrderInfo;
        public const string DEPOSIT_ITEM = "DEPOSIT";
        private Dictionary<string, decimal> _listOpenItemFromHotKeys = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> ListOpenItemFromHotKeys
        {
            get { return _listOpenItemFromHotKeys; }
            private set { _listOpenItemFromHotKeys = value; }
        }

        public DataTable PointPackageBreakdown;

        #region *) WaitingConfirmation: Ready to delete
        public bool getAvailablePointOnServer(out decimal Points, out string Status)
        {
            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                Points = ws.GetCurrentPoint(CurrentMember.MembershipNo, myOrderHdr.OrderDate, out Status);

                return true;
            }
            catch (Exception X)
            {
                Points = 0;
                Status = X.Message;
                return false;
            }
        }
        #endregion

        #region "Confirm Order"

        /// <summary>
        /// New Confirm Order with RealTime Point Allocation feature
        /// </summary>
        /// <param name="doRounding">Should this function has Rounding feature?</param>
        /// <param name="isOnlinePointAllocation">Should this function do RealTime point update to Server?</param>
        /// <param name="isPointAllocated">Is point allocation process successful?</param>
        /// <param name="status">Error Message (If any)</param>
        /// <returns>True if transaction is commited - for [RealTime point allocation] status, look at isPointAllocated </returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Need to review the Update Package Process
        /// </remarks>
        /// 
        public bool ConfirmOrderFromRealTimeSync(bool doRounding, out bool isPointAllocated, string userName, out string status)
        {
            isPointAllocated = false;

            try
            {
                //round the amount of orderdet first so the amount will be tally
                //RoundDownAmountOrderDet();

                #region *) Validation: Check if OrderDet has PreOrder but PreOderInfo has not been set [Exit if False]
                if (hasPreOrder())
                {
                    if (preOrderInfo == null)
                    {
                        status = "Error:Pre-Order information has not been set.";
                        return false;
                    }
                }
                #endregion

                decimal diffPoint = 0;  /// Jumlah point yang akan ditambah / di-deduct ke server
                decimal availablePoint = 0;

                DataTable PackageList = new DataTable();
                #region -= Points & Packages - Validation =-
                string PercentagePointsName = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);
                decimal pointPercentage = 0.0M;
                if (GetMemberInfo() != null && GetMemberInfo().MembershipGroup != null)
                {
                    pointPercentage = GetMemberInfo().MembershipGroup.PointsPercentage;
                }
                if (!Feature.Package.BreakDownPackageChangesList
                    (myOrderDet, recDet, pointPercentage, out PackageList, out diffPoint, out status))
                    throw new Exception(status);

                if (PackageList != null && PackageList.Rows.Count > 0 && !MembershipApplied())
                    throw new Exception("Cannot find member to allocate point/package");
                #endregion

                status = "";
                #region *) Do Rounding if necessary
                if (doRounding)
                {
                    decimal roundAmt = RoundTotalReceiptAmount();
                    decimal actualAmt = CalculateTotalAmount(out status);

                    //insert rounding inventory item....
                    if (roundAmt != actualAmt)
                    {
                        AddItemToOrder
                            (new Item(ROUNDING_ITEM), 1, 0, false, out status);

                        ChangeOrderLineUnitPrice
                            (IsItemIsInOrderLine(ROUNDING_ITEM),
                             roundAmt - actualAmt, out status);
                    }
                }

                #endregion

                //Create the command collection to contain the transactions SQL
                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd;

                #region *) Core: Save Membership for new member sign up
                /*if (MembershipApplied() && isNewMember
                    && CurrentMember.NameToAppear != "Other")
                {
                    //generate new membership number
                    //for new member only
                    //CurrentMember.MembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                    myOrderHdr.MembershipNo = CurrentMember.MembershipNo;
                    if (SalesPersonInfo.SalesPersonID != null && SalesPersonInfo.SalesPersonID != "")
                    {
                        CurrentMember.SalesPersonID = SalesPersonInfo.SalesPersonID;
                    }
                    mycmd = CurrentMember.GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    //cmd.Add(SyncCommandController.AddCommand("Add Member", mycmd, UserInfo.username));
                }*/
                #endregion

                //CurrentMember = new Membership(myOrderHdr.MembershipNo);
                bool newPOSID = false;
                PointOfSale posSelected = new PointOfSale();
                PointOfSaleCollection posColl = new PointOfSaleCollection();
                //posColl.Where(PointOfSale.Columns.Deleted, false);
                posColl.Load();
                if (MembershipApplied())
                {
                    foreach (PointOfSale p in posColl)
                    {

                        if (p.LinkedMembershipNo == CurrentMember.MembershipNo)
                        {
                            newPOSID = true;
                            posSelected = p;
                        }
                    }
                }
                #region *) Core: Save OrderHdr Info
                //create a new order refno

                string newOrderHdrID = CreateNewOrderNo(newPOSID ? posSelected.PointOfSaleID : myOrderHdr.PointOfSaleID);
                myOrderHdr.PointOfSaleID = newPOSID ? posSelected.PointOfSaleID : myOrderHdr.PointOfSaleID;
                myOrderHdr.OrderHdrID = newOrderHdrID;
                myOrderHdr.CashierID = userName;
                myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;

                bool isOutletSales = false;
                if (GetMemberInfo() != null && GetMemberInfo().MembershipNo != "" && GetMemberInfo().MembershipNo != "WALK-IN")
                {
                    //validation if outlet order
                    PointOfSaleCollection posColl1 = new PointOfSaleCollection();
                    posColl1.Where(PointOfSale.UserColumns.LinkedMembershipNo, GetMemberInfo().MembershipNo);
                    posColl1.Load();
                    if (posColl1.Count > 0)
                        isOutletSales = true;

                }
                if (!(isOutletSales) && myOrderHdr.OrderDate == null)
                {
                    myOrderHdr.OrderDate = DateTime.Now;
                }
                if (isOutletSales)
                {
                    newOrderHdrID = CreateNewOrderNoForOutletSales(newPOSID ? posSelected.PointOfSaleID : myOrderHdr.PointOfSaleID, myOrderHdr.OrderDate);
                    myOrderHdr.PointOfSaleID = newPOSID ? posSelected.PointOfSaleID : myOrderHdr.PointOfSaleID;
                    myOrderHdr.OrderHdrID = newOrderHdrID;
                    //myOrderHdr.CashierID = userName;
                    myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;
                }
                myOrderHdr.UniqueID = Guid.NewGuid();
                myOrderHdr.NettAmount = CalculateTotalPaid(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
                myOrderHdr.GrossAmount = CalculateGrossAmount();
                if (MembershipApplied())
                { myOrderHdr.MembershipNo = CurrentMember.MembershipNo; }
                string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
                if (useCustomInvoiceNo != null && useCustomInvoiceNo.ToLower() == "true")
                {
                    myOrderHdr.Userfld5 = CreateNewCustomReceiptNo();  //PointOfSaleInfo.PointOfSaleID
                }
                else
                {
                    myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                }
                mycmd = myOrderHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                //decimal GrossAmount = 0;

                #region *) Core: Save OrderDetCollections
                //Get the query for every order line
                int indexODUOM = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                    {
                        myOrderDet[i].OrderHdrID = newOrderHdrID;
                        myOrderDet[i].UniqueID = Guid.NewGuid();
                        myOrderDet[i].OrderDetID = newOrderHdrID + "." + i.ToString();
                        //myOrderDet[i].Userint4 = i;
                        myOrderDet[i].OrderDetDate = myOrderHdr.OrderDate;
                        myOrderDet[i].CostOfGoodSold = 0; //initialize to allow sync
                        //myOrderDet[i].InventoryHdrRefNo = "";

                        myOrderDet[i].OriginalRetailPrice = myOrderDet[i].Item.RetailPrice;
                        //Calculate Gross Amount and Discount
                        //TODO: Use retail price for open item?                        
                        myOrderDet[i].GrossSales =
                            CalculateLineGrossAmount(myOrderDet[i]);
                        //myOrderDet[i].OriginalRetailPrice * myOrderDet[i].Quantity;
                        //GrossAmount += myOrderDet[i].GrossSales.Value;

                        if (myOrderDet[i].IsPromo)
                        {
                            myOrderDet[i].Amount = myOrderDet[i].PromoAmount;
                            myOrderDet[i].Discount = (decimal)myOrderDet[i].PromoDiscount;
                        }
                        mycmd = myOrderDet[i].GetInsertCommand(UserInfo.username);
                        cmd.Add(mycmd);

                        if (myOrderDet[i].ItemNo == VOUCHER_BARCODE)
                        {
                            //upgrade voucher id....
                            Query qr = Voucher.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
                            if (myOrderDet[i].Quantity > 0)
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                qr.AddUpdateSetting(Voucher.Columns.DateSold, myOrderHdr.OrderDate);
                            }
                            else
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, myOrderHdr.OrderDate);
                            }
                            cmd.Add(qr.BuildUpdateCommand());
                        }

                        if (myOrderDet[i].Item.NonInventoryProduct)
                        {
                            if (!myOrderDet[i].IsVoided)
                            {
                                OrderDetUOMConversion oduom = new OrderDetUOMConversion();

                                Item oit = myOrderDet[i].Item;

                                oduom.OrderDetUOMConvID = newOrderHdrID + "." + indexODUOM.ToString();
                                oduom.OrderDetID = myOrderDet[i].OrderDetID;
                                oduom.OrderHdrID = newOrderHdrID;
                                oduom.UniqueID = Guid.NewGuid();
                                oduom.OrderDetItemNo = myOrderDet[i].ItemNo;
                                oduom.DeductedItemNo = myOrderDet[i].Item.DeductedItem;
                                oduom.ConversionRate = oit.DeductConvType ? 1 / (oit.DeductConvRate == 0 ? 1 : oit.DeductConvRate) : oit.DeductConvRate;
                                oduom.Qty = myOrderDet[i].Quantity * oduom.ConversionRate;
                                oduom.Amount = myOrderDet[i].Amount;
                                oduom.UnitPrice = oduom.Amount / (oduom.Qty == 0 ? 1 : oduom.Qty);
                                oduom.IsVoided = false;
                                oduom.OrderDate = myOrderHdr.OrderDate;

                                cmd.Add(oduom.GetInsertCommand(UserInfo.username));

                                indexODUOM++;
                            }
                        }
                        /*
                        else if (myOrderDet[i].ItemNo == DEPOSIT_ITEM)
                        {
                            cmd.AddRange(MembershipTapController.adjustTap
                                (CurrentMember.MembershipNo, myOrderDet[i].Amount, UserInfo.username, out status));
                        }*/
                    }
                }
                #endregion

                #region *) Core: Save PreOrderInfo
                if (preOrderInfo != null)
                {
                    preOrderInfo.OrderHdrID = myOrderHdr.OrderHdrID;
                    preOrderInfo.UniqueID = Guid.NewGuid();
                    cmd.Add(preOrderInfo.GetInsertCommand(UserInfo.username));
                }
                #endregion

                #region *) Core: Save ReceiptHdr [contains the payment modes and amounts]
                recHdr.CashierID = userName;
                recHdr.ReceiptHdrID = newOrderHdrID;
                recHdr.OrderHdrID = newOrderHdrID;
                recHdr.ReceiptRefNo = "RCP" + newOrderHdrID;
                recHdr.ReceiptDate = myOrderHdr.OrderDate;

                recHdr.UniqueID = Guid.NewGuid();
                mycmd = recHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                #region add payment package if redeem package
                if (IsPackageRedeemTransaction())
                {
                    decimal amountPackage = 0;

                    if (recDet.Count == 1 && recDet[0].PaymentType == POSController.PAY_CASH && recDet[0].Amount == 0)
                        recDet.RemoveAt(0);

                    foreach (OrderDet od in myOrderDet)
                    {
                        if (!od.IsVoided && od.Quantity > 0 && od.Item.Category.CategoryName != "SYSTEM"
                            && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                        {
                            decimal CurrBalance = 0;
                            decimal CurrBreakdownPrice = 0;
                            decimal quantity = od.Quantity.GetValueOrDefault(0);
                            string PackageRefNo = od.PointItemNo;
                            var actionResult = Feature.Package.GetCurrentAmount_andBreakdownPrice(CurrentMember.MembershipNo, myOrderHdr.OrderDate,
                                PackageRefNo, out CurrBalance, out CurrBreakdownPrice, out status);

                            amountPackage += quantity * CurrBreakdownPrice;
                        }
                    }

                    if (amountPackage > 0)
                    {
                        var chk = recDet.Where(o => o.PaymentType == POSController.PAY_PACKAGE).ToList<ReceiptDet>();

                        if (chk.Count == 0)
                        {
                            AddReceiptLinePayment_Package(amountPackage, "", out status);
                        }
                        else
                        {
                            chk[0].Amount = amountPackage;
                        }
                    }
                }


                #endregion

                #region *) Core: Save ReceiptDet
                for (int i = 0; i < recDet.Count; i++)
                {
                    recDet[i].ReceiptHdrID = newOrderHdrID;
                    recDet[i].ReceiptDetID = newOrderHdrID + "." + i;
                    recDet[i].UniqueID = Guid.NewGuid();
                    mycmd = recDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    //if voucher - change voucher status to be redeemed

                    if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
                    {
                        Query qr = Voucher.CreateQuery();
                        qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                }
                #endregion

                #region *) Core: Save SalesCommissionRecord
                //if (PointOfSaleInfo.promptSalesPerson)
                {
                    UserMst user = new UserMst(userName);
                    if (SalesPersonInfo.SalesPersonID != "0")
                    {
                        //assign sales person to the sales person
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = user.SalesPersonGroupID;
                        sr.SalesPersonID = user.UserName;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = SalesPersonInfo.SalesPersonName;
                    }
                    else
                    {
                        //assign commission to th cashier if no sales person selected
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = user.SalesPersonGroupID;
                        sr.SalesPersonID = user.UserName;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = UserInfo.displayName;
                    }
                }
                #endregion

                #region *) Core: Save MembershipRenewal
                if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && !IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewal r = new MembershipRenewal();
                    r.MembershipNo = GetMemberInfo().MembershipNo;

                    r.OldExpiryDate = CurrentMember.ExpiryDate ?? newExpiryDate;
                    r.NewExpiryDate = newExpiryDate;
                    r.NewMembershipGroupID = newMembershipGroupID;
                    r.Deleted = false;
                    r.UniqueId = Guid.NewGuid();
                    r.OrderHdrId = newOrderHdrID;
                    cmd.Add(r.GetInsertCommand(UserInfo.username));

                    //cmd.Add(SyncCommandController.AddCommand("Renew Membership", r.GetInsertCommand(UserInfo.username), UserInfo.username));
                }
                else if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewalCollection mbr = new MembershipRenewalCollection();
                    mbr.Where(MembershipRenewal.Columns.UserFld1, Comparison.IsNot, null);
                    mbr.Where(MembershipRenewal.Columns.MembershipNo, CurrentMember.MembershipNo);
                    mbr.OrderByDesc(MembershipRenewal.Columns.UserFld1);
                    mbr.Load();
                    if (mbr.Count > 0)
                    {
                        MembershipRenewal r = new MembershipRenewal();
                        r.MembershipNo = GetMemberInfo().MembershipNo;

                        r.OldExpiryDate = Convert.ToDateTime(CurrentMember.ExpiryDate);
                        r.NewExpiryDate = Convert.ToDateTime(mbr[0].UserFld1);
                        r.NewMembershipGroupID = newMembershipGroupID;
                        r.Deleted = false;
                        r.UniqueId = Guid.NewGuid();
                        r.OrderHdrId = newOrderHdrID;
                        cmd.Add(r.GetInsertCommand(UserInfo.username));
                    }

                }
                #endregion

                #region *) Core: Save MembershipUpgrade - Not Active
                //if (EligibleForUpgrade())
                //{
                //Query qr = Membership.CreateQuery();
                //qr.QueryType = QueryType.Update;
                //qr.AddUpdateSetting("IsVitaMix", true);
                //qr.AddUpdateSetting("IsJuicePlus", true);
                //qr.AddUpdateSetting("IsWaterFilter", true);
                //qr.AddUpdateSetting("IsYoung", true);
                //qr.AddUpdateSetting("MembershipGroupID", MembershipController.GOLD_GROUPID);
                //cmd.Add(qr.BuildUpdateCommand());

                //MembershipUpgradeLog mbr = new MembershipUpgradeLog();
                //mbr.OrderHdrID = myOrderHdr.OrderHdrID;
                //if (CurrentMember.IsVitaMix.HasValue)
                //    mbr.IsVitaMixPrevValue = CurrentMember.IsVitaMix.Value;
                //if (CurrentMember.IsWaterFilter.HasValue)
                //    mbr.IsWaterFilterPrevValue = CurrentMember.IsWaterFilter.Value;
                //if (CurrentMember.IsYoung.HasValue)
                //    mbr.IsYoungPrevValue = CurrentMember.IsYoung.Value;
                //if (CurrentMember.IsJuicePlus.HasValue)
                //    mbr.IsJuicePlusPrevValue = CurrentMember.IsJuicePlus.Value;
                //mbr.Deleted = false;
                //mbr.IsNew = true;

                //cmd.Add(mbr.GetInsertCommand(UserInfo.username));
                //}

                #endregion

                status = "";

                #region *) Core: Commit all local transaction
                SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                #region *) PostTransaction: Apply Points from Sales
                if (PackageList != null && PackageList.Rows.Count > 0)
                {
                    isPointAllocated = true;

                    /// If PackageList is greater 0, assumed that membership 
                    /// is applied (Look at code above)
                    try
                    {
                        DataTable dt = new DataTable("PointPackage");
                        dt.Columns.Add("RefNo", Type.GetType("System.String"));
                        dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                        dt.Columns.Add("PointType", Type.GetType("System.String"));

                        foreach (DataRow Rw in PackageList.Rows)
                        {
                            string oneKey = Rw[0].ToString();
                            Item myItem = new Item(oneKey);
                            string Mode = Item.PointMode.Times;
                            if (!myItem.IsNew && myItem.IsLoaded) Mode =
                                myItem.PointGetMode;
                            if (oneKey == PercentagePointsName) Mode = Item.PointMode.Dollar;
                            dt.Rows.Add(new object[] { oneKey, decimal.Parse(Rw[1].ToString()), Mode });
                        }
                        string validPeriodInMonth = AppSetting.GetSetting(AppSetting.SettingsName.MembershipPoint.ValidityPeriodInMonth);
                        int validPeriod = 0;
                        if (validPeriodInMonth != null)
                        {
                            int.TryParse(validPeriodInMonth, out validPeriod);
                        }
                        if (!Feature.Package.UpdatePackage
                            (dt, myOrderHdr.OrderHdrID,
                            myOrderHdr.OrderDate, validPeriod, CurrentMember.MembershipNo, GetSalesPerson(), UserInfo.username, out availablePoint, out diffPoint, out status))
                        { Logger.writeLog(status); isPointAllocated = false; }
                    }
                    catch (System.Net.WebException X)
                    {
                        Logger.writeLog(X);
                        status = "Point allocation failed.";
                        isPointAllocated = false;
                    }
                }
                else
                {
                    isPointAllocated = true;
                }
                #endregion

                #region *) PostTransaction: Set IsPointAllocated as TRUE on OrderHdr
                if (isPointAllocated)
                {
                    myOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Re-Create instead of Update.
                    myOrderHdr.InitialPoint = availablePoint;
                    myOrderHdr.PointAmount = diffPoint;
                    myOrderHdr.IsPointAllocated = true;
                    myOrderHdr.Save();
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                #region *) Revert: Delete Rounding Item
                string LineID = IsItemIsInOrderLine(ROUNDING_ITEM);
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    //myOrderDet .... remove.....
                    if (myOrderDet[i].OrderDetID == LineID)
                    {
                        myOrderDet.RemoveAt(i);
                        break;
                    }
                }
                #endregion

                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");

                //log into logger
                Logger.writeLog(ex);

                return false;
            }
        }

        private string SalesPersonName;

        public void SetAsDelivery(bool IsDelivery)
        {
            if (IsDelivery)
            {
                for (int Counter = 0; Counter < myOrderDet.Count; Counter++)
                {
                    if (myOrderDet[Counter].InventoryHdrRefNo == "")
                        myOrderDet[Counter].InventoryHdrRefNo = "DELIVERY";
                }
            }
            else
            {
                for (int Counter = 0; Counter < myOrderDet.Count; Counter++)
                {
                    if (myOrderDet[Counter].InventoryHdrRefNo == "DELIVERY")
                        myOrderDet[Counter].InventoryHdrRefNo = "";
                }
            }
        }

        bool deliveryStatus = false;
        public bool IsADelivery
        {
            get
            {
                deliveryStatus = false;
                if (myOrderDet.Count > 0)
                {
                    for (int Counter = 0; Counter < myOrderDet.Count; Counter++)
                    {
                        if (myOrderDet[Counter].InventoryHdrRefNo == "DELIVERY")
                        {
                            deliveryStatus = true;
                            break;
                        }
                    }
                }

                return deliveryStatus;
                //return myOrderDet.Count > 0 && myOrderDet[0].InventoryHdrRefNo == "DELIVERY"; 
            }
        }

        #region Obsoleted ConfirmOrder
        //public bool ConfirmOrder(bool doRounding, out string status)
        //{
        //    try
        //    {
        //        if (hasPreOrder())
        //        {
        //            if (preOrderInfo == null)
        //            {
        //                status = "Error:Pre-Order information has not been set.";
        //                return false;
        //            }
        //        }
        //        if (doRounding)
        //        {
        //            decimal roundAmt = RoundTotalReceiptAmount();
        //            decimal actualAmt = CalculateTotalAmount(out status);

        //            //insert rounding inventory item....
        //            AddItemToOrder
        //                (new Item(ROUNDING_ITEM), 1, 0, false, out status);

        //            ChangeOrderLineUnitPrice
        //                (IsItemIsInOrderLine(ROUNDING_ITEM), 
        //                 roundAmt - actualAmt, out status);                    
        //        }

        //        //create a new order refno
        //        myOrderHdr.OrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
        //        myOrderHdr.OrderRefNo = "OR" + myOrderHdr.OrderHdrID;
        //        myOrderHdr.UniqueID = Guid.NewGuid();
        //        myOrderHdr.NettAmount = CalculateTotalPaid(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
        //        //myOrderHdr.DiscountAmount = CalculateTotalDiscount(out status);
        //        if (preOrderInfo != null)
        //        {
        //            preOrderInfo.OrderHdrID = myOrderHdr.OrderHdrID;
        //            preOrderInfo.UniqueID = Guid.NewGuid();
        //        }

        //        //Create the command collection to contain the transactions SQL
        //        QueryCommandCollection cmd = new QueryCommandCollection();
        //        QueryCommand mycmd;

        //        if (MembershipApplied())
        //        {
        //            if (isNewMember && CurrentMember.NameToAppear != "Other")
        //            {
        //                mycmd = CurrentMember.GetInsertCommand(UserInfo.username);
        //                cmd.Add(mycmd);
        //            }
        //            myOrderHdr.MembershipNo = CurrentMember.MembershipNo;
        //        }

        //        //myOrderHdr.GrossAmount = myOrderHdr.NettAmount + myOrderHdr.DiscountAmount;
        //        recHdr.ReceiptHdrID = myOrderHdr.OrderHdrID;                
        //        recHdr.OrderHdrID = myOrderHdr.OrderHdrID;
        //        recHdr.ReceiptRefNo = "RCP" + recHdr.ReceiptHdrID;

        //        mycmd = myOrderHdr.GetInsertCommand(UserInfo.username);
        //        cmd.Add(mycmd);             

        //        decimal GrossAmount=0;
        //        //Get the query for every order line
        //        for (int i = 0; i < myOrderDet.Count; i++)
        //        {
        //            if (!myOrderDet[i].IsVoided) //only save non voided transactions
        //            {
        //                myOrderDet[i].OrderHdrID = myOrderHdr.OrderHdrID;
        //                myOrderDet[i].UniqueID = Guid.NewGuid();
        //                myOrderDet[i].OrderDetID = myOrderHdr.OrderHdrID + "." + i.ToString();
        //                myOrderDet[i].CostOfGoodSold = 0; //initialize to allow sync
        //                myOrderDet[i].InventoryHdrRefNo = "";

        //                //Calculate Gross Amount and Discount
        //                myOrderDet[i].OriginalRetailPrice = myOrderDet[i].Item.RetailPrice;
        //                myOrderDet[i].GrossSales = myOrderDet[i].OriginalRetailPrice * myOrderDet[i].Quantity;
        //                GrossAmount += myOrderDet[i].GrossSales.Value;

        //                if (myOrderDet[i].IsPromo)
        //                {
        //                    myOrderDet[i].Amount = myOrderDet[i].PromoAmount;
        //                    myOrderDet[i].Discount = (decimal)myOrderDet[i].PromoDiscount;
        //                }
        //                mycmd = myOrderDet[i].GetInsertCommand(UserInfo.username);
        //                cmd.Add(mycmd);                        

        //                if (myOrderDet[i].ItemNo == InstallmentController.INSTALLMENT_ITEM)
        //                {
        //                    cmd.Add(InstallmentController.Update(myOrderDet[i].VoucherNo,
        //                        myOrderDet[i].Amount, UserInfo.username, out status));
        //                }
        //                else if (myOrderDet[i].ItemNo == VOUCHER_BARCODE)
        //                {
        //                    //upgrade voucher id....
        //                    Query qr = Voucher.CreateQuery();
        //                    qr.QueryType = QueryType.Update;
        //                    qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
        //                    if (myOrderDet[i].Quantity > 0)
        //                    {
        //                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
        //                        qr.AddUpdateSetting(Voucher.Columns.DateSold, myOrderHdr.OrderDate);
        //                    }
        //                    else
        //                    {
        //                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
        //                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, myOrderHdr.OrderDate);
        //                    }
        //                    cmd.Add(qr.BuildUpdateCommand());
        //                }
        //                else if (myOrderDet[i].ItemNo == DEPOSIT_ITEM)
        //                {
        //                    cmd.AddRange(MembershipTapController.adjustTap
        //                        (CurrentMember.MembershipNo, myOrderDet[i].Amount, UserInfo.username, out status));
        //                }
        //            }                    
        //        }

        //        //Calculate Total Discount                                
        //        if (preOrderInfo != null)
        //            cmd.Add(preOrderInfo.GetInsertCommand(UserInfo.username));
        //        //Get the query for every order line

        //        //Creation of Receipt - contains the payment modes and amounts
        //        recHdr.UniqueID = Guid.NewGuid();                
        //        mycmd = recHdr.GetInsertCommand(UserInfo.username);                
        //        cmd.Add(mycmd);

        //        //Receipt details
        //        for (int i = 0; i < recDet.Count; i++)
        //        {
        //            recDet[i].ReceiptHdrID = recHdr.ReceiptHdrID;
        //            recDet[i].ReceiptDetID = recHdr.ReceiptHdrID + "." + i;
        //            recDet[i].UniqueID = Guid.NewGuid();
        //            mycmd = recDet[i].GetInsertCommand(UserInfo.username);
        //            cmd.Add(mycmd);

        //            //if voucher - change voucher status to be redeemed

        //            if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
        //            {
        //                Query qr = Voucher.CreateQuery();
        //                qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
        //                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
        //                qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
        //                cmd.Add(qr.BuildUpdateCommand());

        //            }
        //        }

        //        //Create Commission Information
        //        if (PointOfSaleInfo.promptSalesPerson)
        //        {
        //            if (SalesPersonInfo.SalesPersonID != "0")
        //            {
        //                SalesCommissionRecord sr = new SalesCommissionRecord();
        //                sr.OrderHdrID = myOrderHdr.OrderHdrID;
        //                sr.SalesGroupID = SalesPersonInfo.SalesGroupID;
        //                sr.SalesPersonID = SalesPersonInfo.SalesPersonID;
        //                sr.UniqueID = Guid.NewGuid();
        //                cmd.Add(sr.GetInsertCommand(UserInfo.username));
        //                SalesPersonName = SalesPersonInfo.SalesPersonName;
        //            }
        //            else
        //            {
        //                SalesCommissionRecord sr = new SalesCommissionRecord();
        //                sr.OrderHdrID = myOrderHdr.OrderHdrID;
        //                sr.SalesGroupID = UserInfo.SalesPersonGroupID;
        //                sr.SalesPersonID = UserInfo.username;
        //                sr.UniqueID = Guid.NewGuid();
        //                cmd.Add(sr.GetInsertCommand(UserInfo.username));
        //                SalesPersonName = UserInfo.displayName;
        //            }
        //        }

        //        if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied())
        //        {
        //            MembershipRenewal r = new MembershipRenewal();
        //            r.MembershipNo = GetMemberInfo().MembershipNo;
        //            r.NewExpiryDate = newExpiryDate;
        //            r.Deleted = false;
        //            r.UniqueId = Guid.NewGuid();
        //            r.OrderHdrId = myOrderHdr.OrderHdrID;
        //            cmd.Add(r.GetInsertCommand(UserInfo.username));
        //        }

        //        if (EligibleForUpgrade())
        //        {
        //            Query qr = Membership.CreateQuery();
        //            qr.QueryType = QueryType.Update;
        //            qr.AddUpdateSetting("IsVitaMix", true);
        //            qr.AddUpdateSetting("IsJuicePlus", true);
        //            qr.AddUpdateSetting("IsWaterFilter", true);
        //            qr.AddUpdateSetting("IsYoung", true);
        //            qr.AddUpdateSetting("MembershipGroupID", MembershipController.GOLD_GROUPID);
        //            qr.AddWhere(Membership.Columns.MembershipNo, CurrentMember.MembershipNo);
        //            cmd.Add(qr.BuildUpdateCommand());

        //            MembershipUpgradeLog mbr = new MembershipUpgradeLog();
        //            mbr.OrderHdrID = myOrderHdr.OrderHdrID;
        //            if (CurrentMember.IsVitaMix.HasValue)
        //                 mbr.IsVitaMixPrevValue = CurrentMember.IsVitaMix.Value;
        //            if (CurrentMember.IsWaterFilter.HasValue)
        //                mbr.IsWaterFilterPrevValue = CurrentMember.IsWaterFilter.Value;
        //            if (CurrentMember.IsYoung.HasValue)
        //                mbr.IsYoungPrevValue = CurrentMember.IsYoung.Value;
        //            if (CurrentMember.IsJuicePlus.HasValue)
        //                mbr.IsJuicePlusPrevValue = CurrentMember.IsJuicePlus.Value;
        //            mbr.Deleted = false;
        //            mbr.IsNew = true;

        //            cmd.Add(mbr.GetInsertCommand(UserInfo.username));

        //        }
        //        //Execute transactions                                
        //        SubSonic.DataService.ExecuteTransaction(cmd);
        //        status = "";
        //        return true;                               
        //    }
        //    catch (Exception ex)
        //    {
        //        //if do rounding, delete the rounding inventory item....
        //        string LineID = IsItemIsInOrderLine(ROUNDING_ITEM);
        //        for (int i = 0; i < myOrderDet.Count; i++)
        //        {
        //            //myOrderDet .... remove.....
        //            if (myOrderDet[i].OrderDetID == LineID)
        //            {
        //                myOrderDet.RemoveAt(i);
        //                break;
        //            }
        //        }

        //        status = ex.Message;

        //        //log into logger
        //        Logger.writeLog(ex);

        //        return false;
        //    }
        //}
        #endregion

        /// <summary>
        /// New Confirm Order with RealTime Point Allocation feature
        /// </summary>
        /// <param name="doRounding">Should this function has Rounding feature?</param>
        /// <param name="isOnlinePointAllocation">Should this function do RealTime point update to Server?</param>
        /// <param name="isPointAllocated">Is point allocation process successful?</param>
        /// <param name="status">Error Message (If any)</param>
        /// <returns>True if transaction is commited - for [RealTime point allocation] status, look at isPointAllocated </returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Need to review the Update Package Process
        /// </remarks>
        public bool ConfirmOrder(bool doRounding, out bool isPointAllocated, out string status)
        {
            isPointAllocated = false;

            try
            {
                DateTime processStartTime = DateTime.Now;

                #region *) Validation: Check if OrderDet has PreOrder but PreOderInfo has not been set [Exit if False]
                if (hasPreOrder())
                {
                    if (preOrderInfo == null)
                    {
                        status = "Error:Pre-Order information has not been set.";
                        return false;
                    }
                }
                #endregion

                decimal diffPoint = 0;  /// Jumlah point yang akan ditambah / di-deduct ke server
                decimal availablePoint = 0;
                string realTimePointSystem = ConfigurationManager.AppSettings["RealTimePointSystem"];
                bool isRealTimePointSystem = realTimePointSystem != null && realTimePointSystem.ToLower() == "yes";
                DataTable PackageList = new DataTable();
                PointPackageBreakdown = new DataTable();


                //#region -= Points & Packages - Validation =-

                //string PercentagePointsName = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);
                //decimal pointPercentage = 0.0M;
                //if (GetMemberInfo() != null && GetMemberInfo().MembershipGroup != null)
                //{
                //    pointPercentage = GetMemberInfo().MembershipGroup.PointsPercentage;
                //}
                //if (!Feature.Package.BreakDownPackageChangesList
                //    (myOrderDet, recDet, pointPercentage, out PackageList, out diffPoint, out status))
                //    throw new Exception(status);

                //if (PackageList != null && PackageList.Rows.Count > 0 && !MembershipApplied())
                //    throw new Exception("Cannot find member to allocate point/package");

                //#endregion

                //#region Check Package Usage
                //bool isHavePackageUsage = false;
                //for (int i = 0; i < myOrderDet.Count; i++)
                //{
                //    if (myOrderDet[i].Userflag5.GetValueOrDefault(false) && myOrderDet[i].IsVoided == false)
                //    {
                //        string refNo = string.IsNullOrEmpty(myOrderDet[i].PointItemNo) ? myOrderDet[i].ItemNo : myOrderDet[i].PointItemNo;
                //        PackageList = PackageList.DeleteRow(PackageList.Columns[0].ColumnName, refNo);
                //        isHavePackageUsage = true;
                //    }
                //}
                //#endregion
                status = "";

                //
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.RoundingForAllPayment), false))
                    doRounding = true;

                #region *) Do Rounding if necessary
                if (doRounding)
                {
                    decimal roundAmt = RoundTotalReceiptAmount();
                    decimal actualAmt = CalculateTotalAmount(out status);

                    //insert rounding inventory item....
                    if (roundAmt != actualAmt)
                    {
                        AddItemToOrder
                            (new Item(ROUNDING_ITEM), 1, 0, false, out status);

                        ChangeOrderLineUnitPrice
                            (IsItemIsInOrderLine(ROUNDING_ITEM),
                             roundAmt - actualAmt, out status);
                    }
                }
                #endregion

                #region *) Outlet Sales
                bool isOutletSales = false;
                int newPOSID = PointOfSaleInfo.PointOfSaleID;
                string newOrderHdrID = "";
                bool enableOutletSales = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false);
                if (enableOutletSales)
                {
                    if (MembershipApplied())
                    {
                        string sql = @"SELECT  TOP 1 POS.PointOfSaleID
                                       FROM	   PointOfSale POS
                                       WHERE   POS.userfld10 = '{0}'";
                        sql = string.Format(sql, CurrentMember.MembershipNo);
                        DataTable dtNewPOS = new DataTable();
                        dtNewPOS.Load(DataService.GetReader(new QueryCommand(sql)));
                        if (dtNewPOS.Rows.Count > 0)
                        {
                            newPOSID = (dtNewPOS.Rows[0][0] + "").GetIntValue();
                            if (newPOSID == 0)
                                newPOSID = PointOfSaleInfo.PointOfSaleID;
                            if (newPOSID != PointOfSaleInfo.PointOfSaleID)
                            {
                                isOutletSales = true;
                                if (!SyncClientController.SendRealTimeSales(this, out status, out newOrderHdrID))
                                    throw new Exception("Failed send outlet sales to the server!");
                            }
                        }
                    }
                }
                #endregion

                #region *) Get Outstanding Balance from Member
                if (MembershipApplied() && !isNewMember)
                {
                    this.OutstandingBalanceOverall = Installment.GetOutstandingBalancePerMember(CurrentMember.MembershipNo);
                }
                #endregion

                //Create the command collection to contain the transactions SQL
                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd;

                #region *) Core: Save Membership for new member sign up
                if (MembershipApplied() && isNewMember
                    && CurrentMember.NameToAppear != "Other")
                {
                    //generate new membership number
                    //for new member only
                    //CurrentMember.MembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                    myOrderHdr.MembershipNo = CurrentMember.MembershipNo;
                    if (SalesPersonInfo.SalesPersonID != null && SalesPersonInfo.SalesPersonID != "")
                    {
                        CurrentMember.SalesPersonID = SalesPersonInfo.SalesPersonID;
                    }
                    mycmd = CurrentMember.GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    if (CurrentAttachedParticular != null && CurrentAttachedParticular.Count > 0)
                    {
                        foreach (AttachedParticular ap in CurrentAttachedParticular)
                        {
                            ap.MembershipNo = CurrentMember.MembershipNo;
                            mycmd = ap.GetInsertCommand(UserInfo.username);
                            cmd.Add(mycmd);
                        }
                    }

                    string sqlUpdateModifiedOn = "Update Membership set ModifiedOn = '2010-01-01' where membershipno = '" + CurrentMember.MembershipNo + "'";
                    cmd.Add(new QueryCommand(sqlUpdateModifiedOn));
                    //cmd.Add(SyncCommandController.AddCommand("Add Member", mycmd, UserInfo.username));
                }
                #endregion

                #region *) Core: Save OrderHdr Info

                //create a new order refno
                if (string.IsNullOrEmpty(newOrderHdrID))
                    newOrderHdrID = CreateNewOrderNo(newPOSID);
                myOrderHdr.PointOfSaleID = newPOSID;
                myOrderHdr.OrderHdrID = newOrderHdrID;
                myOrderHdr.CashierID = UserInfo.username;
                myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;

                if (!isOutletSales)
                {
                    myOrderHdr.OrderDate = DateTime.Now;


                }
                /*else
                {
                    newOrderHdrID = CreateNewOrderNoForOutletSales(newPOSID, myOrderHdr.OrderDate);
                    myOrderHdr.OrderHdrID = newOrderHdrID;
                    myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;
                }*/

                myOrderHdr.UniqueID = Guid.NewGuid();
                myOrderHdr.NettAmount = CalculateTotalPaid(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
                myOrderHdr.GrossAmount = CalculateGrossAmount();
                if (MembershipApplied())
                { myOrderHdr.MembershipNo = CurrentMember.MembershipNo; }
                string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
                if (useCustomInvoiceNo != null && useCustomInvoiceNo.ToLower() == "true")
                {
                    myOrderHdr.Userfld5 = CreateNewCustomReceiptNo();  //PointOfSaleInfo.PointOfSaleID
                }
                else
                {
                    myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                }
                mycmd = myOrderHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                //decimal GrossAmount = 0;

                #region *) Core: Save OrderDetCollections
                //Get the query for every order line
                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, false);
                int indexODUOM = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                    {
                        myOrderDet[i].OrderHdrID = newOrderHdrID;
                        myOrderDet[i].UniqueID = Guid.NewGuid();
                        myOrderDet[i].OrderDetID = newOrderHdrID + "." + i.ToString();
                        myOrderDet[i].OrderDetDate = myOrderHdr.OrderDate;
                        myOrderDet[i].CostOfGoodSold = 0; //initialize to allow sync
                        //myOrderDet[i].InventoryHdrRefNo = "";
                        myOrderDet[i].UOM = myOrderDet[i].Item.UOM;

                        myOrderDet[i].OriginalRetailPrice = myOrderDet[i].Item.RetailPrice;
                        //Calculate Gross Amount and Discount
                        //TODO: Use retail price for open item?                        
                        myOrderDet[i].GrossSales =
                            CalculateLineGrossAmount(myOrderDet[i]);
                        //myOrderDet[i].OriginalRetailPrice * myOrderDet[i].Quantity;
                        //GrossAmount += myOrderDet[i].GrossSales.Value;

                        if (myOrderDet[i].IsPromo)
                        {
                            myOrderDet[i].Amount = myOrderDet[i].PromoAmount;
                            myOrderDet[i].Discount = (decimal)myOrderDet[i].PromoDiscount;
                        }
                        mycmd = myOrderDet[i].GetInsertCommand(UserInfo.username);
                        cmd.Add(mycmd);

                        if (myOrderDet[i].ItemNo == VOUCHER_BARCODE)
                        {
                            //upgrade voucher id....
                            Query qr = Voucher.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
                            if (myOrderDet[i].Quantity > 0)
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                qr.AddUpdateSetting(Voucher.Columns.DateSold, myOrderHdr.OrderDate);
                            }
                            else
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, myOrderHdr.OrderDate);
                            }
                            cmd.Add(qr.BuildUpdateCommand());
                        }

                        if (myOrderDet[i].Item.NonInventoryProduct)
                        {
                            if (!myOrderDet[i].IsVoided)
                            {
                                OrderDetUOMConversion oduom = new OrderDetUOMConversion();

                                Item oit = myOrderDet[i].Item;

                                oduom.OrderDetUOMConvID = newOrderHdrID + "." + indexODUOM.ToString();
                                oduom.OrderDetID = myOrderDet[i].OrderDetID;
                                oduom.OrderHdrID = newOrderHdrID;
                                oduom.UniqueID = Guid.NewGuid();
                                oduom.OrderDetItemNo = myOrderDet[i].ItemNo;
                                oduom.DeductedItemNo = myOrderDet[i].Item.DeductedItem;
                                oduom.ConversionRate = oit.DeductConvType ? 1 / (oit.DeductConvRate == 0 ? 1 : oit.DeductConvRate) : oit.DeductConvRate;
                                oduom.Qty = myOrderDet[i].Quantity * oduom.ConversionRate;
                                oduom.Amount = myOrderDet[i].Amount;
                                oduom.UnitPrice = oduom.Amount / (oduom.Qty == 0 ? 1 : oduom.Qty);
                                oduom.IsVoided = false;
                                oduom.OrderDate = myOrderHdr.OrderDate;
                                cmd.Add(oduom.GetInsertCommand(UserInfo.username));

                                indexODUOM++;
                            }
                        }
                        /*
                        else if (myOrderDet[i].ItemNo == DEPOSIT_ITEM)
                        {
                            cmd.AddRange(MembershipTapController.adjustTap
                                (CurrentMember.MembershipNo, myOrderDet[i].Amount, UserInfo.username, out status));
                        }*/
                    }
                }
                #endregion



                #region *) Core: Save PreOrderInfo
                if (preOrderInfo != null)
                {
                    preOrderInfo.OrderHdrID = myOrderHdr.OrderHdrID;
                    preOrderInfo.UniqueID = Guid.NewGuid();
                    cmd.Add(preOrderInfo.GetInsertCommand(UserInfo.username));
                }
                #endregion

                #region *) Core: Save ReceiptHdr [contains the payment modes and amounts]
                recHdr.CashierID = UserInfo.username;
                recHdr.ReceiptHdrID = newOrderHdrID;
                recHdr.OrderHdrID = newOrderHdrID;
                recHdr.ReceiptRefNo = "RCP" + newOrderHdrID;
                recHdr.ReceiptDate = myOrderHdr.OrderDate;

                recHdr.UniqueID = Guid.NewGuid();
                mycmd = recHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                #region add payment package if redeem package
                if (IsPackageRedeemTransaction())
                {
                    decimal amountPackage = 0;

                    if (recDet.Count == 1 && recDet[0].PaymentType == POSController.PAY_CASH && recDet[0].Amount == 0)
                        recDet.RemoveAt(0);

                    foreach (OrderDet od in myOrderDet)
                    {
                        if (!od.IsVoided && od.Quantity > 0 && od.Item.Category.CategoryName != "SYSTEM"
                            && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                        {
                            decimal CurrBalance = 0;
                            decimal CurrBreakdownPrice = 0;
                            decimal quantity = od.Quantity.GetValueOrDefault(0);
                            string PackageRefNo = od.PointItemNo;
                            var actionResult = Feature.Package.GetCurrentAmount_andBreakdownPrice(CurrentMember.MembershipNo, myOrderHdr.OrderDate,
                                PackageRefNo, out CurrBalance, out CurrBreakdownPrice, out status);

                            amountPackage += quantity * CurrBreakdownPrice;
                        }
                    }

                    if (amountPackage > 0)
                    {
                        var chk = recDet.Where(o => o.PaymentType == POSController.PAY_PACKAGE).ToList<ReceiptDet>();

                        if (chk.Count == 0)
                        {
                            AddReceiptLinePayment_Package(amountPackage, "", out status);
                        }
                        else
                        {
                            chk[0].Amount = amountPackage;
                        }
                    }
                }


                #endregion

                #region *) Core: Save ReceiptDet
                for (int i = 0; i < recDet.Count; i++)
                {
                    recDet[i].ReceiptHdrID = newOrderHdrID;
                    recDet[i].ReceiptDetID = newOrderHdrID + "." + i;
                    recDet[i].UniqueID = Guid.NewGuid();
                    mycmd = recDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    //if voucher - change voucher status to be redeemed

                    if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
                    {
                        Query qr = Voucher.CreateQuery();
                        qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
                        cmd.Add(qr.BuildUpdateCommand());

                    }

                    if (recDet[i].PaymentType == POSController.PAY_INSTALLMENT)
                    {
                        this.OutstandingBalanceOverall += recDet[i].Amount;
                        this.OutstandingBalanceOrder += recDet[i].Amount;
                    }
                }
                #endregion

                #region -= Points & Packages - Validation =-

                string PercentagePointsName = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);
                decimal pointPercentage = 0.0M;
                if (GetMemberInfo() != null && GetMemberInfo().MembershipGroup != null)
                {
                    pointPercentage = GetMemberInfo().MembershipGroup.PointsPercentage;
                }
                if (!Feature.Package.BreakDownPackageChangesList
                    (myOrderDet, recDet, pointPercentage, out PackageList, out diffPoint, out status))
                    throw new Exception(status);

                if (PackageList != null && PackageList.Rows.Count > 0 && !MembershipApplied())
                    throw new Exception("Cannot find member to allocate point/package");

                #endregion

                #region Check Package Usage
                bool isHavePackageUsage = false;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].Userflag5.GetValueOrDefault(false) && myOrderDet[i].IsVoided == false)
                    {
                        string refNo = string.IsNullOrEmpty(myOrderDet[i].PointItemNo) ? myOrderDet[i].ItemNo : myOrderDet[i].PointItemNo;
                        PackageList = PackageList.DeleteRow(PackageList.Columns[0].ColumnName, refNo);
                        isHavePackageUsage = true;
                    }
                }
                #endregion




                #region *) Core: Save SalesCommissionRecord
                //if (PointOfSaleInfo.promptSalesPerson)
                {
                    if (SalesPersonInfo.SalesPersonID != "0")
                    {
                        //assign sales person to the sales person
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = SalesPersonInfo.SalesGroupID;
                        sr.SalesPersonID = SalesPersonInfo.SalesPersonID;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = SalesPersonInfo.SalesPersonName;
                    }
                    else
                    {
                        //assign commission to th cashier if no sales person selected
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = UserInfo.SalesPersonGroupID;
                        sr.SalesPersonID = UserInfo.username;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = UserInfo.displayName;
                    }
                }
                #endregion

                #region *) Core: Save MembershipRenewal
                if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && !IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewal r = new MembershipRenewal();
                    r.MembershipNo = GetMemberInfo().MembershipNo;
                    r.OldExpiryDate = CurrentMember.ExpiryDate ?? newExpiryDate;
                    r.NewExpiryDate = newExpiryDate;

                    r.NewMembershipGroupID = newMembershipGroupID;
                    r.Deleted = false;
                    r.UniqueId = Guid.NewGuid();
                    r.OrderHdrId = newOrderHdrID;
                    cmd.Add(r.GetInsertCommand(UserInfo.username));

                    CurrentMember.MembershipGroupId = newMembershipGroupID;
                    CurrentMember.ExpiryDate = newExpiryDate;
                    cmd.Add(CurrentMember.GetUpdateCommand(UserInfo.username));


                    //cmd.Add(SyncCommandController.AddCommand("Renew Membership", r.GetInsertCommand(UserInfo.username), UserInfo.username));
                }
                else if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewalCollection mbr = new MembershipRenewalCollection();
                    mbr.Where(MembershipRenewal.Columns.UserFld1, Comparison.IsNot, null);
                    mbr.Where(MembershipRenewal.Columns.MembershipNo, CurrentMember.MembershipNo);
                    mbr.OrderByDesc(MembershipRenewal.Columns.UserFld1);
                    mbr.Load();
                    if (mbr.Count > 0)
                    {
                        MembershipRenewal r = new MembershipRenewal();
                        r.MembershipNo = GetMemberInfo().MembershipNo;

                        r.OldExpiryDate = Convert.ToDateTime(CurrentMember.ExpiryDate);
                        r.NewExpiryDate = Convert.ToDateTime(mbr[0].UserFld1);
                        r.NewMembershipGroupID = newMembershipGroupID;
                        r.Deleted = false;
                        r.UniqueId = Guid.NewGuid();
                        r.OrderHdrId = newOrderHdrID;
                        cmd.Add(r.GetInsertCommand(UserInfo.username));

                        //CurrentMember.MembershipGroupId = newMembershipGroupID;
                        CurrentMember.ExpiryDate = Convert.ToDateTime(mbr[0].UserFld1);
                        cmd.Add(CurrentMember.GetUpdateCommand(UserInfo.username));
                    }

                }
                #endregion

                #region *) Core: Save MembershipUpgrade - Not Active
                //if (EligibleForUpgrade())
                //{
                //Query qr = Membership.CreateQuery();
                //qr.QueryType = QueryType.Update;
                //qr.AddUpdateSetting("IsVitaMix", true);
                //qr.AddUpdateSetting("IsJuicePlus", true);
                //qr.AddUpdateSetting("IsWaterFilter", true);
                //qr.AddUpdateSetting("IsYoung", true);
                //qr.AddUpdateSetting("MembershipGroupID", MembershipController.GOLD_GROUPID);
                //cmd.Add(qr.BuildUpdateCommand());

                //MembershipUpgradeLog mbr = new MembershipUpgradeLog();
                //mbr.OrderHdrID = myOrderHdr.OrderHdrID;
                //if (CurrentMember.IsVitaMix.HasValue)
                //    mbr.IsVitaMixPrevValue = CurrentMember.IsVitaMix.Value;
                //if (CurrentMember.IsWaterFilter.HasValue)
                //    mbr.IsWaterFilterPrevValue = CurrentMember.IsWaterFilter.Value;
                //if (CurrentMember.IsYoung.HasValue)
                //    mbr.IsYoungPrevValue = CurrentMember.IsYoung.Value;
                //if (CurrentMember.IsJuicePlus.HasValue)
                //    mbr.IsJuicePlusPrevValue = CurrentMember.IsJuicePlus.Value;
                //mbr.Deleted = false;
                //mbr.IsNew = true;

                //cmd.Add(mbr.GetInsertCommand(UserInfo.username));
                //}

                #endregion

                #region *) Core: Create Cash Out For Cashback
                if (GetCashBackAmount() > 0)
                {
                    QueryCommand qryCashback;
                    if (CashRecordingController.createCashOutFromOrder(myOrderHdr.Userfld5, GetCashBackAmount(), myOrderHdr.OrderDate, UserInfo.username, UserInfo.username, out qryCashback))
                    {
                        cmd.Add(qryCashback);
                    }
                }

                #endregion

                status = "";

                #region *) Core: Commit all local transaction
                SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                #region *) PostTransaction: Apply Points from Sales
                if (isHavePackageUsage)
                {
                    decimal InitialPoint = 0;
                    decimal DiffPoint = 0;

                    try
                    {
                        #region #) Core: Send data to server

                        if (!isRealTimePointSystem && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                            isPointAllocated = true;

                        DataTable dt = new DataTable("PointPackage");
                        dt.Columns.Add("RefNo", Type.GetType("System.String"));
                        dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                        dt.Columns.Add("PointType", Type.GetType("System.String"));

                        foreach (OrderDet oneKey in myOrderDet)
                        {
                            if (oneKey.Userflag5.GetValueOrDefault(false) && oneKey.IsVoided == false)
                            {
                                dt.Rows.Add(new object[] { string.IsNullOrEmpty(oneKey.PointItemNo) ? oneKey.ItemNo : oneKey.PointItemNo, 0 - oneKey.Quantity, Item.PointMode.Times });
                            }
                        }

                        // Also add the same row to PointPackageBreakdown datatable
                        PointPackageBreakdown.Merge(dt, true, MissingSchemaAction.Add);

                        if (!isRealTimePointSystem && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        {
                            if (!Feature.Package.UpdatePackage(dt, myOrderHdr.OrderHdrID, myOrderHdr.OrderDate, 0, CurrentMember.MembershipNo, GetSalesPerson(), UserInfo.username, out InitialPoint, out DiffPoint, out status))
                            { Logger.writeLog(status); isPointAllocated = false; }
                        }
                        #endregion
                    }
                    catch (System.Net.WebException X)
                    {
                        Logger.writeLog(X);
                        status = "Point allocation failed.";
                        isPointAllocated = false;
                    }
                }

                if (PackageList != null && PackageList.Rows.Count > 0)
                {
                    if (!isRealTimePointSystem && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        isPointAllocated = true;

                    /// If PackageList is greater 0, assumed that membership 
                    /// is applied (Look at code above)
                    try
                    {
                        DataTable dt = new DataTable("PointPackage");
                        dt.Columns.Add("RefNo", Type.GetType("System.String"));
                        dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                        dt.Columns.Add("PointType", Type.GetType("System.String"));

                        foreach (DataRow Rw in PackageList.Rows)
                        {
                            string oneKey = Rw[0].ToString();
                            string myItemNo = oneKey;
                            //break open price package...
                            if (oneKey.Contains("|OPP|")) //if it is open price package
                            {
                                myItemNo = oneKey.Substring(0, oneKey.IndexOf("|OPP|"));
                            }
                            Item myItem = new Item(myItemNo);
                            string Mode = Item.PointMode.Times;
                            if (!myItem.IsNew && myItem.IsLoaded) Mode =
                                myItem.PointGetMode;
                            if (oneKey == PercentagePointsName) Mode = Item.PointMode.Dollar;
                            dt.Rows.Add(new object[] { oneKey, decimal.Parse(Rw[1].ToString()), Mode });
                        }
                        string validPeriodInMonth = AppSetting.GetSetting(AppSetting.SettingsName.MembershipPoint.ValidityPeriodInMonth);
                        int validPeriod = 0;
                        if (validPeriodInMonth != null)
                        {
                            int.TryParse(validPeriodInMonth, out validPeriod);
                        }

                        // Also add the same row to PointPackageBreakdown datatable
                        PointPackageBreakdown.Merge(dt, true, MissingSchemaAction.Add);

                        if (!isRealTimePointSystem && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        {
                            if (!Feature.Package.UpdatePackage(dt, myOrderHdr.OrderHdrID,
                                myOrderHdr.OrderDate, validPeriod, CurrentMember.MembershipNo, GetSalesPerson(), UserInfo.username, out availablePoint, out diffPoint, out status))
                            { Logger.writeLog(status); isPointAllocated = false; }
                        }
                    }
                    catch (System.Net.WebException X)
                    {
                        Logger.writeLog(X);
                        status = "Point allocation failed.";
                        isPointAllocated = false;
                    }
                }
                else
                {
                    if (!isRealTimePointSystem && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        isPointAllocated = true;
                }
                #endregion

                #region *) PostTransaction: Save Points In PointTempLog Table That Will Be Deleted When Point Is Synced
                if (isRealTimePointSystem)
                {
                    QueryCommandCollection cmdPTL = new QueryCommandCollection();
                    foreach (DataRow dr in PointPackageBreakdown.Rows)
                    {
                        PointTempLog ptl = new PointTempLog();
                        ptl.OrderHdrID = myOrderHdr.OrderHdrID;
                        ptl.MembershipNo = myOrderHdr.MembershipNo;
                        ptl.PointAllocated = dr["Amount"].ToString().GetDecimalValue();
                        ptl.RefNo = dr["RefNo"].ToString();
                        ptl.PointType = dr["PointType"].ToString();
                        cmdPTL.Add(ptl.GetInsertCommand(UserInfo.username));
                    }
                    if (cmdPTL.Count > 0) DataService.ExecuteTransaction(cmdPTL);
                }
                #endregion

                #region *) PostTransaction: Set IsPointAllocated as TRUE on OrderHdr
                if (isPointAllocated)
                {
                    myOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Re-Create instead of Update.
                    myOrderHdr.InitialPoint = availablePoint;
                    myOrderHdr.PointAmount = diffPoint;
                    myOrderHdr.IsPointAllocated = true;
                    myOrderHdr.Save();
                }
                #endregion

                #region *) PostTransaction: Update Installment table
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    if (!InstallmentController.UpdateInstallmentByOrderHdr(myOrderHdr.OrderHdrID, out status))
                        MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region *) PostTransaction: Update Pre Order
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    if (!PreOrderController.AssignAutoDeposit(myOrderHdr.OrderHdrID, out status))
                        MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (!PreOrderController.RefundPreOrder(myOrderHdr.OrderHdrID, out status))
                        MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion


                #region *) AccessLog
                /*
                try
                {
                    string logText = "";
                    for (int i = 0; i < recDet.Count; i++)
                    {
                        logText += string.Format("{0} ${1}, ", recDet[i].PaymentType, recDet[i].Amount.ToString("N2"));
                    }
                    logText += "for ";
                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        Item theItem = myOrderDet[i].Item;
                        if (theItem.CategoryName != "SYSTEM")
                            logText += string.Format("{0} ({1}), ", theItem.ItemName, myOrderDet[i].Quantity);
                        if (myOrderDet[i].Discount > 0)
                            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Give Discount {0}% ({1})", myOrderDet[i].Discount.ToString("N2"), myOrderDet[i].OrderHdrID), "");
                        if (theItem.ItemNo == "MEMBER")
                            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Membership sign up {0}", myOrderHdr.MembershipNo), "");
                        if (myOrderDet[i].Quantity < 0)
                            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Refund for Item : {0} ({1})", myOrderDet[i].ItemNo, myOrderDet[i].OrderHdrID), "");
                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", logText, "");
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                 * */
                #endregion


                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnablePerformanceLog), false))
                {
                    PerformanceLogController.AddLog(processStartTime, DateTime.Now, "POS", "CreateOrder",
                        myOrderHdr.PointOfSaleID, myOrderHdr.OrderHdrID, UserInfo.username);
                }

                return true;
            }
            catch (Exception ex)
            {
                #region *) Revert: Delete Rounding Item
                string LineID = IsItemIsInOrderLine(ROUNDING_ITEM);
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    //myOrderDet .... remove.....
                    if (myOrderDet[i].OrderDetID == LineID)
                    {
                        myOrderDet.RemoveAt(i);
                        break;
                    }
                }
                #endregion

                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");

                //log into logger
                Logger.writeLog(ex);

                return false;
            }
        }

        /// <summary>
        /// New Confirm Order with RealTime Point Allocation feature
        /// </summary>
        /// <param name="doRounding">Should this function has Rounding feature?</param>
        /// <param name="isOnlinePointAllocation">Should this function do RealTime point update to Server?</param>
        /// <param name="isPointAllocated">Is point allocation process successful?</param>
        /// <param name="status">Error Message (If any)</param>
        /// <returns>True if transaction is commited - for [RealTime point allocation] status, look at isPointAllocated </returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Need to review the Update Package Process
        /// </remarks>
        public bool ConfirmOrderFromSync(bool doRounding, string cashier, out bool isPointAllocated, out string status)
        {
            isPointAllocated = false;

            try
            {
                #region *) Validation: Check if OrderDet has PreOrder but PreOderInfo has not been set [Exit if False]
                if (hasPreOrder())
                {
                    if (preOrderInfo == null)
                    {
                        status = "Error:Pre-Order information has not been set.";
                        return false;
                    }
                }
                #endregion

                decimal diffPoint = 0;  /// Jumlah point yang akan ditambah / di-deduct ke server
                decimal availablePoint = 0;

                DataTable PackageList = new DataTable();
                #region -= Points & Packages - Validation =-
                string PercentagePointsName = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);
                decimal pointPercentage = 0.0M;
                if (GetMemberInfo() != null && GetMemberInfo().MembershipGroup != null)
                {
                    pointPercentage = GetMemberInfo().MembershipGroup.PointsPercentage;
                }
                if (!Feature.Package.BreakDownPackageChangesList
                    (myOrderDet, recDet, pointPercentage, out PackageList, out diffPoint, out status))
                    throw new Exception(status);

                if (PackageList != null && PackageList.Rows.Count > 0 && !MembershipApplied())
                    throw new Exception("Cannot find member to allocate point/package");
                #endregion

                status = "";
                #region *) Do Rounding if necessary
                if (doRounding)
                {
                    decimal roundAmt = RoundTotalReceiptAmount();
                    decimal actualAmt = CalculateTotalAmount(out status);

                    //insert rounding inventory item....
                    if (roundAmt != actualAmt)
                    {
                        AddItemToOrder
                            (new Item(ROUNDING_ITEM), 1, 0, false, out status);

                        ChangeOrderLineUnitPrice
                            (IsItemIsInOrderLine(ROUNDING_ITEM),
                             roundAmt - actualAmt, out status);
                    }
                }
                #endregion

                //Create the command collection to contain the transactions SQL
                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd;

                #region *) Core: Save Membership for new member sign up
                if (MembershipApplied() && isNewMember
                    && CurrentMember.NameToAppear != "Other")
                {
                    //generate new membership number
                    //for new member only
                    //CurrentMember.MembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                    myOrderHdr.MembershipNo = CurrentMember.MembershipNo;
                    if (SalesPersonInfo.SalesPersonID != null && SalesPersonInfo.SalesPersonID != "")
                    {
                        CurrentMember.SalesPersonID = SalesPersonInfo.SalesPersonID;
                    }
                    mycmd = CurrentMember.GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    //cmd.Add(SyncCommandController.AddCommand("Add Member", mycmd, UserInfo.username));
                }
                #endregion

                #region *) Core: Save OrderHdr Info
                //create a new order refno
                string newOrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
                myOrderHdr.OrderHdrID = newOrderHdrID;
                myOrderHdr.CashierID = cashier;
                myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;

                myOrderHdr.OrderDate = DateTime.Now;
                myOrderHdr.UniqueID = Guid.NewGuid();
                myOrderHdr.NettAmount = CalculateTotalPaid(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
                myOrderHdr.GrossAmount = CalculateGrossAmount();
                if (MembershipApplied())
                { myOrderHdr.MembershipNo = CurrentMember.MembershipNo; }
                /*string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
                if (useCustomInvoiceNo != null && useCustomInvoiceNo.ToLower() == "true")
                {
                    myOrderHdr.Userfld5 = CreateNewCustomReceiptNo();  //PointOfSaleInfo.PointOfSaleID
                }
                else
                {
                    myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                }*/
                mycmd = myOrderHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                //decimal GrossAmount = 0;

                #region *) Core: Save OrderDetCollections
                //Get the query for every order line
                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, false);
                int indexODUOM = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                    {
                        myOrderDet[i].OrderHdrID = newOrderHdrID;
                        myOrderDet[i].UniqueID = Guid.NewGuid();
                        myOrderDet[i].OrderDetID = newOrderHdrID + "." + i.ToString();
                        myOrderDet[i].OrderDetDate = myOrderHdr.OrderDate;
                        myOrderDet[i].CostOfGoodSold = 0; //initialize to allow sync
                        //myOrderDet[i].InventoryHdrRefNo = "";

                        myOrderDet[i].OriginalRetailPrice = myOrderDet[i].Item.RetailPrice;
                        //Calculate Gross Amount and Discount
                        //TODO: Use retail price for open item?                        
                        myOrderDet[i].GrossSales =
                            CalculateLineGrossAmount(myOrderDet[i]);
                        //myOrderDet[i].OriginalRetailPrice * myOrderDet[i].Quantity;
                        //GrossAmount += myOrderDet[i].GrossSales.Value;

                        if (myOrderDet[i].IsPromo)
                        {
                            myOrderDet[i].Amount = myOrderDet[i].PromoAmount;
                            myOrderDet[i].Discount = (decimal)myOrderDet[i].PromoDiscount;
                        }
                        mycmd = myOrderDet[i].GetInsertCommand(UserInfo.username);
                        cmd.Add(mycmd);

                        if (myOrderDet[i].ItemNo == VOUCHER_BARCODE)
                        {
                            //upgrade voucher id....
                            Query qr = Voucher.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
                            if (myOrderDet[i].Quantity > 0)
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                qr.AddUpdateSetting(Voucher.Columns.DateSold, myOrderHdr.OrderDate);
                            }
                            else
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, myOrderHdr.OrderDate);
                            }
                            cmd.Add(qr.BuildUpdateCommand());
                        }

                        if (myOrderDet[i].Item.NonInventoryProduct)
                        {
                            if (!myOrderDet[i].IsVoided)
                            {
                                OrderDetUOMConversion oduom = new OrderDetUOMConversion();

                                Item oit = myOrderDet[i].Item;

                                oduom.OrderDetUOMConvID = newOrderHdrID + "." + indexODUOM.ToString();
                                oduom.OrderDetID = myOrderDet[i].OrderDetID;
                                oduom.OrderHdrID = newOrderHdrID;
                                oduom.UniqueID = Guid.NewGuid();
                                oduom.OrderDetItemNo = myOrderDet[i].ItemNo;
                                oduom.DeductedItemNo = myOrderDet[i].Item.DeductedItem;
                                oduom.ConversionRate = oit.DeductConvType ? 1 / (oit.DeductConvRate == 0 ? 1 : oit.DeductConvRate) : oit.DeductConvRate;
                                oduom.Qty = myOrderDet[i].Quantity * oduom.ConversionRate;
                                oduom.Amount = myOrderDet[i].Amount;
                                oduom.UnitPrice = oduom.Amount / (oduom.Qty == 0 ? 1 : oduom.Qty);
                                oduom.IsVoided = false;
                                oduom.OrderDate = myOrderHdr.OrderDate;
                                cmd.Add(oduom.GetInsertCommand(UserInfo.username));

                                indexODUOM++;
                            }
                        }
                        /*
                        else if (myOrderDet[i].ItemNo == DEPOSIT_ITEM)
                        {
                            cmd.AddRange(MembershipTapController.adjustTap
                                (CurrentMember.MembershipNo, myOrderDet[i].Amount, UserInfo.username, out status));
                        }*/
                    }
                }
                #endregion

                #region *) Core: Save PreOrderInfo
                if (preOrderInfo != null)
                {
                    preOrderInfo.OrderHdrID = myOrderHdr.OrderHdrID;
                    preOrderInfo.UniqueID = Guid.NewGuid();
                    cmd.Add(preOrderInfo.GetInsertCommand(UserInfo.username));
                }
                #endregion

                #region *) Core: Save ReceiptHdr [contains the payment modes and amounts]
                recHdr.CashierID = cashier;
                recHdr.ReceiptHdrID = newOrderHdrID;
                recHdr.OrderHdrID = newOrderHdrID;
                recHdr.ReceiptRefNo = "RCP" + newOrderHdrID;
                recHdr.ReceiptDate = myOrderHdr.OrderDate;

                recHdr.UniqueID = Guid.NewGuid();
                mycmd = recHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                #region add payment package if redeem package
                if (IsPackageRedeemTransaction())
                {
                    decimal amountPackage = 0;

                    if (recDet.Count == 1 && recDet[0].PaymentType == POSController.PAY_CASH && recDet[0].Amount == 0)
                        recDet.RemoveAt(0);

                    foreach (OrderDet od in myOrderDet)
                    {
                        if (!od.IsVoided && od.Quantity > 0 && od.Item.Category.CategoryName != "SYSTEM"
                            && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                        {
                            decimal CurrBalance = 0;
                            decimal CurrBreakdownPrice = 0;
                            decimal quantity = od.Quantity.GetValueOrDefault(0);
                            string PackageRefNo = od.PointItemNo;
                            var actionResult = Feature.Package.GetCurrentAmount_andBreakdownPrice(CurrentMember.MembershipNo, myOrderHdr.OrderDate,
                                PackageRefNo, out CurrBalance, out CurrBreakdownPrice, out status);

                            amountPackage += quantity * CurrBreakdownPrice;
                        }
                    }

                    if (amountPackage > 0)
                    {
                        var chk = recDet.Where(o => o.PaymentType == POSController.PAY_PACKAGE).ToList<ReceiptDet>();

                        if (chk.Count == 0)
                        {
                            AddReceiptLinePayment_Package(amountPackage, "", out status);
                        }
                        else
                        {
                            chk[0].Amount = amountPackage;
                        }
                    }
                }


                #endregion

                #region *) Core: Save ReceiptDet
                for (int i = 0; i < recDet.Count; i++)
                {
                    recDet[i].ReceiptHdrID = newOrderHdrID;
                    recDet[i].ReceiptDetID = newOrderHdrID + "." + i;
                    recDet[i].UniqueID = Guid.NewGuid();
                    mycmd = recDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    //if voucher - change voucher status to be redeemed

                    if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
                    {
                        Query qr = Voucher.CreateQuery();
                        qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                }
                #endregion

                #region *) Core: Save SalesCommissionRecord
                //if (PointOfSaleInfo.promptSalesPerson)
                {
                    if (SalesPersonInfo.SalesPersonID != "0")
                    {
                        UserMst us = new UserMst("Admin");
                        //assign sales person to the sales person
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = us.SalesPersonGroupID;
                        sr.SalesPersonID = us.UserName;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = SalesPersonInfo.SalesPersonName;
                    }
                    else
                    {
                        UserMst us = new UserMst("Admin");
                        //assign commission to th cashier if no sales person selected
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = newOrderHdrID;
                        sr.SalesGroupID = us.SalesPersonGroupID;
                        sr.SalesPersonID = us.UserName;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = UserInfo.displayName;
                    }
                }
                #endregion

                #region *) Core: Save MembershipRenewal
                if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && !IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewal r = new MembershipRenewal();
                    r.MembershipNo = GetMemberInfo().MembershipNo;
                    r.OldExpiryDate = CurrentMember.ExpiryDate ?? newExpiryDate;
                    r.NewExpiryDate = newExpiryDate;
                    r.NewMembershipGroupID = newMembershipGroupID;
                    r.Deleted = false;
                    r.UniqueId = Guid.NewGuid();
                    r.OrderHdrId = newOrderHdrID;
                    cmd.Add(r.GetInsertCommand(UserInfo.username));

                    //cmd.Add(SyncCommandController.AddCommand("Renew Membership", r.GetInsertCommand(UserInfo.username), UserInfo.username));
                }
                else if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewalCollection mbr = new MembershipRenewalCollection();
                    mbr.Where(MembershipRenewal.Columns.UserFld1, Comparison.IsNot, null);
                    mbr.Where(MembershipRenewal.Columns.MembershipNo, CurrentMember.MembershipNo);
                    mbr.OrderByDesc(MembershipRenewal.Columns.UserFld1);
                    mbr.Load();
                    if (mbr.Count > 0)
                    {
                        MembershipRenewal r = new MembershipRenewal();
                        r.MembershipNo = GetMemberInfo().MembershipNo;

                        r.OldExpiryDate = Convert.ToDateTime(CurrentMember.ExpiryDate);
                        r.NewExpiryDate = Convert.ToDateTime(mbr[0].UserFld1);
                        r.NewMembershipGroupID = newMembershipGroupID;
                        r.Deleted = false;
                        r.UniqueId = Guid.NewGuid();
                        r.OrderHdrId = newOrderHdrID;
                        cmd.Add(r.GetInsertCommand(UserInfo.username));
                    }

                }
                #endregion

                #region *) Core: Save MembershipUpgrade - Not Active
                //if (EligibleForUpgrade())
                //{
                //Query qr = Membership.CreateQuery();
                //qr.QueryType = QueryType.Update;
                //qr.AddUpdateSetting("IsVitaMix", true);
                //qr.AddUpdateSetting("IsJuicePlus", true);
                //qr.AddUpdateSetting("IsWaterFilter", true);
                //qr.AddUpdateSetting("IsYoung", true);
                //qr.AddUpdateSetting("MembershipGroupID", MembershipController.GOLD_GROUPID);
                //cmd.Add(qr.BuildUpdateCommand());

                //MembershipUpgradeLog mbr = new MembershipUpgradeLog();
                //mbr.OrderHdrID = myOrderHdr.OrderHdrID;
                //if (CurrentMember.IsVitaMix.HasValue)
                //    mbr.IsVitaMixPrevValue = CurrentMember.IsVitaMix.Value;
                //if (CurrentMember.IsWaterFilter.HasValue)
                //    mbr.IsWaterFilterPrevValue = CurrentMember.IsWaterFilter.Value;
                //if (CurrentMember.IsYoung.HasValue)
                //    mbr.IsYoungPrevValue = CurrentMember.IsYoung.Value;
                //if (CurrentMember.IsJuicePlus.HasValue)
                //    mbr.IsJuicePlusPrevValue = CurrentMember.IsJuicePlus.Value;
                //mbr.Deleted = false;
                //mbr.IsNew = true;

                //cmd.Add(mbr.GetInsertCommand(UserInfo.username));
                //}

                #endregion

                status = "";

                #region *) Core: Commit all local transaction
                SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                #region *) PostTransaction: Apply Points from Sales
                if (PackageList != null && PackageList.Rows.Count > 0)
                {
                    isPointAllocated = true;

                    /// If PackageList is greater 0, assumed that membership 
                    /// is applied (Look at code above)
                    try
                    {
                        DataTable dt = new DataTable("PointPackage");
                        dt.Columns.Add("RefNo", Type.GetType("System.String"));
                        dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                        dt.Columns.Add("PointType", Type.GetType("System.String"));

                        foreach (OrderDet oneKey in myOrderDet)
                        {
                            if (oneKey.Userflag5.GetValueOrDefault(false) && oneKey.IsVoided == false)
                            {
                                dt.Rows.Add(new object[] { oneKey.Userfld2 == null ? oneKey.ItemNo : oneKey.Userfld2, 0 - oneKey.Quantity, Item.PointMode.Times });
                            }
                        }


                        foreach (DataRow Rw in PackageList.Rows)
                        {
                            string oneKey = Rw[0].ToString();
                            Item myItem = new Item(oneKey);
                            string Mode = Item.PointMode.Times;
                            if (!myItem.IsNew && myItem.IsLoaded) Mode =
                                myItem.PointGetMode;
                            if (oneKey == PercentagePointsName) Mode = Item.PointMode.Dollar;
                            dt.Rows.Add(new object[] { oneKey, decimal.Parse(Rw[1].ToString()), Mode });
                        }
                        string validPeriodInMonth = AppSetting.GetSetting(AppSetting.SettingsName.MembershipPoint.ValidityPeriodInMonth);
                        int validPeriod = 0;
                        if (validPeriodInMonth != null)
                        {
                            int.TryParse(validPeriodInMonth, out validPeriod);
                        }
                        if (!Feature.Package.UpdatePackage(dt, myOrderHdr.OrderHdrID,
                            myOrderHdr.OrderDate, validPeriod, CurrentMember.MembershipNo, GetSalesPerson(), UserInfo.username, out availablePoint, out diffPoint, out status))
                        { Logger.writeLog(status); isPointAllocated = false; }
                    }
                    catch (System.Net.WebException X)
                    {
                        Logger.writeLog(X);
                        status = "Point allocation failed.";
                        isPointAllocated = false;
                    }
                }
                else
                {
                    isPointAllocated = true;
                }
                #endregion

                #region *) PostTransaction: Set IsPointAllocated as TRUE on OrderHdr
                if (isPointAllocated)
                {
                    myOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Re-Create instead of Update.
                    myOrderHdr.InitialPoint = availablePoint;
                    myOrderHdr.PointAmount = diffPoint;
                    myOrderHdr.IsPointAllocated = true;
                    myOrderHdr.Save();
                }
                #endregion

                #region *) AccessLog
                try
                {
                    string logText = "";
                    for (int i = 0; i < recDet.Count; i++)
                    {
                        logText += string.Format("{0} ${1}, " + recDet[i].PaymentType, recDet[i].Amount.ToString("N2"));
                    }
                    logText += "for ";
                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        Item theItem = myOrderDet[i].Item;
                        if (theItem.CategoryName != "SYSTEM")
                            logText += string.Format("{0} ({1}), ", theItem.ItemName, myOrderDet[i].Quantity);
                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", logText, "");
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                #region *) Revert: Delete Rounding Item
                string LineID = IsItemIsInOrderLine(ROUNDING_ITEM);
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    //myOrderDet .... remove.....
                    if (myOrderDet[i].OrderDetID == LineID)
                    {
                        myOrderDet.RemoveAt(i);
                        break;
                    }
                }
                #endregion

                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");

                //log into logger
                Logger.writeLog(ex);

                return false;
            }
        }

        public static bool IsOrderExist(string OrderRefNo)
        {
            OrderHdrCollection col = new OrderHdrCollection();
            col.Where(OrderHdr.Columns.Userfld5, OrderRefNo);
            col.Load();
            if (col.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doRounding"></param>
        /// <param name="isOnlinePackageAllocation"></param>
        /// <param name="isPointAllocated"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <remarks>
        ///     *) Fully Migrated
        ///     *) Need to review the Update Package Process
        /// </remarks>
        public bool ConfirmPackageOrder(bool doRounding, out bool isPointAllocated, out string status)
        {
            bool ActionResult = false;
            isPointAllocated = false;

            try
            {
                #region *) Validation: Check if OrderDet has PreOrder but PreOderInfo has not been set [Exit if False]
                if (hasPreOrder())
                {
                    if (preOrderInfo == null)
                    {
                        status = "Error:Pre-Order information has not been set.";
                        return false;
                    }
                }
                #endregion

                if (recDet.Count != 1 || recDet[0].PaymentType != PAY_PACKAGE)
                    throw new Exception("You cannot do partial payment for package.");

                if (myOrderDet.Count != 1)
                    throw new Exception("You cannot do partial payment for package.");

                if (myOrderDet[0].Item.PointGetMode != Item.PointMode.Times)
                    throw new Exception("Item [" + myOrderDet[0].Item.ItemName + "] is not a package");

                decimal CurrBalance = 0;
                decimal CurrBreakdownPrice = 0;
                string Package_MainItemNo = myOrderDet[0].PointItemNo == null ? myOrderDet[0].ItemNo : myOrderDet[0].Userfld2;
                ActionResult = Feature.Package.GetCurrentAmount_andBreakdownPrice(CurrentMember.MembershipNo, myOrderHdr.OrderDate, Package_MainItemNo, out CurrBalance, out CurrBreakdownPrice, out status);
                //ActionResult = Feature.Package.GetCurrentAmount(CurrentMember.MembershipNo, myOrderHdr.OrderDate, Package_MainItemNo, out CurrBalance,out status);

                if (!ActionResult) return false;

                if (myOrderDet[0].Quantity > CurrBalance)
                    throw new Exception("Remaining balance is not sufficient.\nYou only have " + CurrBalance + " Credits");

                status = "";
                #region *) Repair: Do Rounding [If Options = True]
                if (doRounding)
                {
                    decimal roundAmt = RoundTotalReceiptAmount();
                    decimal actualAmt = CalculateTotalAmount(out status);

                    //insert rounding inventory item....
                    AddItemToOrder
                        (new Item(ROUNDING_ITEM), 1, 0, false, out status);

                    ChangeOrderLineUnitPrice
                        (IsItemIsInOrderLine(ROUNDING_ITEM),
                         roundAmt - actualAmt, out status);
                }
                #endregion

                //Create the command collection to contain the transactions SQL
                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd;

                //for (int i = 0; i < myOrderDet.Count; i++)
                //{
                //    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                //    {
                //        Item ItemChecked = myOrderDet[i].Item;
                //        if (ItemChecked.PointGetMode == Item.PointMode.Times && ItemChecked.Userfloat3.HasValue)
                //            myOrderDet[i].UnitPrice = ItemChecked.Userfloat3.GetValueOrDefault(0);
                //    }
                //}

                #region *) Core: Save Membership
                if (MembershipApplied() && isNewMember && CurrentMember.NameToAppear != "Other")
                {
                    mycmd = CurrentMember.GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                #endregion

                #region *) Core: Save OrderHdr Info
                //create a new order refno
                myOrderHdr.OrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
                myOrderHdr.OrderRefNo = "OR" + myOrderHdr.OrderHdrID;
                myOrderHdr.UniqueID = Guid.NewGuid();
                myOrderHdr.NettAmount = CalculateTotalPaid(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
                //myOrderHdr.DiscountAmount = CalculateTotalDiscount(out status);

                if (MembershipApplied())
                { myOrderHdr.MembershipNo = CurrentMember.MembershipNo; }

                mycmd = myOrderHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                #region *) Core: Set PreOrderInfo [Not Finished]
                if (preOrderInfo != null)
                {
                    preOrderInfo.OrderHdrID = myOrderHdr.OrderHdrID;
                    preOrderInfo.UniqueID = Guid.NewGuid();
                }
                #endregion

                #region *) Core: Set ReceiptHdr [Not Finished]
                //myOrderHdr.GrossAmount = myOrderHdr.NettAmount + myOrderHdr.DiscountAmount;
                recHdr.ReceiptHdrID = myOrderHdr.OrderHdrID;
                recHdr.OrderHdrID = myOrderHdr.OrderHdrID;
                recHdr.ReceiptRefNo = "RCP" + recHdr.ReceiptHdrID;
                #endregion

                decimal GrossAmount = 0;

                //Get the query for every order line
                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, false);
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                    {
                        //Item ItemChecked = myOrderDet[i].Item;
                        //if (ItemChecked.Userfloat3.HasValue)
                        //myOrderDet[i].UnitPrice = CurrBreakdownPrice;
                        myOrderDet[i].OrderHdrID = myOrderHdr.OrderHdrID;
                        myOrderDet[i].UniqueID = Guid.NewGuid();
                        myOrderDet[i].OrderDetID = myOrderHdr.OrderHdrID + "." + i.ToString();
                        myOrderDet[i].CostOfGoodSold = 0; //initialize to allow sync
                        myOrderDet[i].InventoryHdrRefNo = "";

                        //Calculate Gross Amount and Discount
                        //myOrderDet[i].OriginalRetailPrice = myOrderDet[i].Item.RetailPrice;
                        //myOrderDet[i].GrossSales = myOrderDet[i].OriginalRetailPrice * myOrderDet[i].Quantity;
                        //GrossAmount += myOrderDet[i].GrossSales.Value;

                        if (myOrderDet[i].IsPromo)
                        {
                            myOrderDet[i].Amount = myOrderDet[i].PromoAmount;
                            myOrderDet[i].Discount = (decimal)myOrderDet[i].PromoDiscount;
                        }
                        mycmd = myOrderDet[i].GetInsertCommand(UserInfo.username);
                        cmd.Add(mycmd);

                        if (myOrderDet[i].ItemNo == InstallmentController.INSTALLMENT_ITEM)
                        {
                            cmd.Add(InstallmentController.Update(myOrderDet[i].VoucherNo,
                                myOrderDet[i].Amount, UserInfo.username, out status));
                        }
                        else if (myOrderDet[i].ItemNo == VOUCHER_BARCODE)
                        {
                            //upgrade voucher id....
                            Query qr = Voucher.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddWhere(Voucher.Columns.VoucherNo, myOrderDet[i].VoucherNo);
                            if (myOrderDet[i].Quantity > 0)
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                qr.AddUpdateSetting(Voucher.Columns.DateSold, myOrderHdr.OrderDate);
                            }
                            else
                            {
                                qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, myOrderHdr.OrderDate);
                            }
                            cmd.Add(qr.BuildUpdateCommand());
                        }
                        else if (myOrderDet[i].ItemNo == DEPOSIT_ITEM)
                        {
                            cmd.AddRange(MembershipTapController.adjustTap
                                (CurrentMember.MembershipNo, myOrderDet[i].Amount, UserInfo.username, out status));
                        }
                    }
                }

                //Calculate Total Discount                                
                if (preOrderInfo != null)
                    cmd.Add(preOrderInfo.GetInsertCommand(UserInfo.username));
                //Get the query for every order line

                //Creation of Receipt - contains the payment modes and amounts
                recHdr.UniqueID = Guid.NewGuid();
                mycmd = recHdr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);

                //Receipt details
                for (int i = 0; i < recDet.Count; i++)
                {
                    recDet[i].ReceiptHdrID = recHdr.ReceiptHdrID;
                    recDet[i].ReceiptDetID = recHdr.ReceiptHdrID + "." + i;
                    recDet[i].UniqueID = Guid.NewGuid();
                    mycmd = recDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);

                    //if voucher - change voucher status to be redeemed

                    if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
                    {
                        Query qr = Voucher.CreateQuery();
                        qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                }

                #region *) Core: Save Commission Information
                /* Commented by Evan
                 * Reason: If never save, system will display wrongly */

                //if (PointOfSaleInfo.promptSalesPerson)
                {
                    if (SalesPersonInfo.SalesPersonID != "0")
                    {
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = myOrderHdr.OrderHdrID;
                        sr.SalesGroupID = SalesPersonInfo.SalesGroupID;
                        sr.SalesPersonID = SalesPersonInfo.SalesPersonID;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = SalesPersonInfo.SalesPersonName;
                    }
                    else
                    {
                        SalesCommissionRecord sr = new SalesCommissionRecord();
                        sr.OrderHdrID = myOrderHdr.OrderHdrID;
                        sr.SalesGroupID = UserInfo.SalesPersonGroupID;
                        sr.SalesPersonID = UserInfo.username;
                        sr.UniqueID = Guid.NewGuid();
                        cmd.Add(sr.GetInsertCommand(UserInfo.username));
                        SalesPersonName = UserInfo.displayName;
                    }
                }
                #endregion

                if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && !IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewal r = new MembershipRenewal();
                    r.MembershipNo = GetMemberInfo().MembershipNo;
                    r.OldExpiryDate = CurrentMember.ExpiryDate ?? newExpiryDate;
                    r.NewExpiryDate = newExpiryDate;
                    r.Deleted = false;
                    r.UniqueId = Guid.NewGuid();
                    r.OrderHdrId = myOrderHdr.OrderHdrID;
                    cmd.Add(r.GetInsertCommand(UserInfo.username));
                }
                else if (IsItemIsInOrderLine(MembershipController.RENEWAL_BARCODE) != "" && MembershipApplied() && IsItemIsRefundInOrderLine(MembershipController.RENEWAL_BARCODE))
                {
                    MembershipRenewalCollection mbr = new MembershipRenewalCollection();
                    mbr.Where(MembershipRenewal.Columns.UserFld1, Comparison.IsNot, null);
                    mbr.Where(MembershipRenewal.Columns.MembershipNo, CurrentMember.MembershipNo);
                    mbr.OrderByDesc(MembershipRenewal.Columns.UserFld1);
                    mbr.Load();
                    if (mbr.Count > 0)
                    {
                        MembershipRenewal r = new MembershipRenewal();
                        r.MembershipNo = GetMemberInfo().MembershipNo;

                        r.OldExpiryDate = Convert.ToDateTime(CurrentMember.ExpiryDate);
                        r.NewExpiryDate = Convert.ToDateTime(mbr[0].UserFld1);
                        r.NewMembershipGroupID = newMembershipGroupID;
                        r.Deleted = false;
                        r.UniqueId = Guid.NewGuid();
                        r.OrderHdrId = myOrderHdr.OrderHdrID;
                        cmd.Add(r.GetInsertCommand(UserInfo.username));
                    }

                }
                /*
                if (EligibleForUpgrade())
                {
                    Query qr = Membership.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddUpdateSetting("IsVitaMix", true);
                    qr.AddUpdateSetting("IsJuicePlus", true);
                    qr.AddUpdateSetting("IsWaterFilter", true);
                    qr.AddUpdateSetting("IsYoung", true);
                    qr.AddUpdateSetting("MembershipGroupID", MembershipController.GOLD_GROUPID);
                    qr.AddWhere(Membership.Columns.MembershipNo, CurrentMember.MembershipNo);
                    cmd.Add(qr.BuildUpdateCommand());

                    MembershipUpgradeLog mbr = new MembershipUpgradeLog();
                    mbr.OrderHdrID = myOrderHdr.OrderHdrID;
                    if (CurrentMember.IsVitaMix.HasValue)
                        mbr.IsVitaMixPrevValue = CurrentMember.IsVitaMix.Value;
                    if (CurrentMember.IsWaterFilter.HasValue)
                        mbr.IsWaterFilterPrevValue = CurrentMember.IsWaterFilter.Value;
                    if (CurrentMember.IsYoung.HasValue)
                        mbr.IsYoungPrevValue = CurrentMember.IsYoung.Value;
                    if (CurrentMember.IsJuicePlus.HasValue)
                        mbr.IsJuicePlusPrevValue = CurrentMember.IsJuicePlus.Value;
                    mbr.Deleted = false;
                    mbr.IsNew = true;

                    cmd.Add(mbr.GetInsertCommand(UserInfo.username));

                }*/

                status = "";

                #region *) Core: Commit all local transaction
                SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                isPointAllocated = true;

                #region *) PostTransaction: Apply Points from Sales
                decimal InitialPoint = 0;
                decimal DiffPoint = 0;

                try
                {
                    #region #) Core: Send data to server
                    isPointAllocated = true;

                    DataTable dt = new DataTable("PointPackage");
                    dt.Columns.Add("RefNo", Type.GetType("System.String"));
                    dt.Columns.Add("Amount", Type.GetType("System.Decimal"));
                    dt.Columns.Add("PointType", Type.GetType("System.String"));

                    foreach (OrderDet oneKey in myOrderDet)
                    {
                        dt.Rows.Add(new object[] { oneKey.Userfld2 == null ? oneKey.ItemNo : oneKey.Userfld2, 0 - oneKey.Quantity, Item.PointMode.Times });
                    }

                    if (!Feature.Package.UpdatePackage(dt, myOrderHdr.OrderHdrID, myOrderHdr.OrderDate, 0, CurrentMember.MembershipNo, GetSalesPerson(), UserInfo.username, out InitialPoint, out DiffPoint, out status))
                    { Logger.writeLog(status); isPointAllocated = false; }

                    #endregion
                }
                catch (System.Net.WebException X)
                {
                    Logger.writeLog(X);
                    status = "Point allocation failed.";
                    isPointAllocated = false;
                }

                #region #) Core: Set IsPointAllocated as TRUE on OrderHdr
                if (isPointAllocated)
                {
                    myOrderHdr.MarkOld(); /// Without this line, MyOrderHdr will be considered as NEW, and will do Create instead of Update.
                    myOrderHdr.InitialPoint = InitialPoint;
                    myOrderHdr.PointAmount = DiffPoint;
                    myOrderHdr.IsPointAllocated = true;
                    myOrderHdr.Save();
                }
                #endregion
                #endregion

                #region *) AccessLog
                try
                {
                    string logText = "";
                    for (int i = 0; i < recDet.Count; i++)
                    {
                        logText += string.Format("{0} ${1}, " + recDet[i].PaymentType, recDet[i].Amount.ToString("N2"));
                    }
                    logText += "for ";
                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        Item theItem = myOrderDet[i].Item;
                        if (theItem.CategoryName != "SYSTEM")
                            logText += string.Format("{0} ({1}), ", theItem.ItemName, myOrderDet[i].Quantity);
                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", logText, "");
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                //if do rounding, delete the rounding inventory item....
                string LineID = IsItemIsInOrderLine(ROUNDING_ITEM);
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    //myOrderDet .... remove.....
                    if (myOrderDet[i].OrderDetID == LineID)
                    {
                        myOrderDet.RemoveAt(i);
                        break;
                    }
                }

                status = ex.Message;

                //log into logger
                Logger.writeLog(ex);

                return false;
            }
        }

        public decimal getAllRedeemableTotalAmount
        {
            get
            {
                decimal PointNeeded = 0;
                foreach (OrderDet oneDet in myOrderDet)
                {
                    if (oneDet.Item.PointRedeemMode == Item.PointMode.Dollar)
                    {
                        if (!oneDet.IsVoided && !oneDet.IsPromoFreeOfCharge)
                        {
                            if (oneDet.IsPromo)
                                PointNeeded += oneDet.PromoAmount;
                            else
                                PointNeeded += oneDet.Amount;
                        }
                    }
                }

                //Calculate overall discount
                PointNeeded = PointNeeded * (decimal)(1 - myOrderHdr.Discount / 100);

                return CommonUILib.RemoveRoundUp(PointNeeded);
            }
        }

        public bool hasItemThatCannotBeRedeemedByPoints
        {
            get
            {
                if (myOrderDet.Count < 1) return true;

                foreach (OrderDet oneDet in myOrderDet)
                {
                    if (!oneDet.IsVoided && !oneDet.IsPromoFreeOfCharge)
                    {
                        if (oneDet.Item.PointRedeemMode != Item.PointMode.Dollar)
                            return true;
                    }
                }

                return false;
            }
        }

        public bool containPackages
        {
            get
            {
                if (myOrderDet.Count < 1) return false;

                foreach (OrderDet oneDet in myOrderDet)
                {
                    if (!oneDet.IsVoided)
                    {
                        if (oneDet.Item.PointGetMode == Item.PointMode.Dollar || oneDet.Item.PointGetMode == Item.PointMode.Times)
                            return true;
                    }
                }

                return false;
            }
        }
        [Obsolete]
        public bool isPointInServerSufficient
        {
            get
            {
                string Status;
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;

                decimal PointNeeded = getAllRedeemableTotalAmount;
                decimal PointAvailable = ws.GetCurrentPoint(CurrentMember.MembershipNo, myOrderHdr.OrderDate, out Status);

                if (PointNeeded > PointAvailable) return false;

                return true;
            }
        }

        public bool ExecuteStockOut(out string status)
        {
            /* --- PARAMETER DECLARATION --- */
            QueryCommandCollection cmd = new QueryCommandCollection();

            status = "";

            #region *) Validation: Don't do anything if IntegrateWithInventory is false
            /* If not IntegrateWithInventory, Stock Out will be done at server */
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                return true;
            }
            #endregion

            //#region *) Validation: No movement is allowed when there are un-adjusted stock take in place
            ///* Return true because we don't want to disturb the cashier with warning */
            //if (StockTakeController.IsThereUnAdjustedStockTake())
            //{
            //    Logger.writeLog("There are Stock Take in progress, no stock movement is allowed.");
            //    return true;
            //}
            //#endregion

            #region *) Validation: Make sure POS is attached to InventoryLocation
            if (PointOfSaleInfo.InventoryLocationID == -1)
            {
                status = "POS is not attached to any Inventory Location.\nPlease contact your administrator";
                Logger.writeLog("WARNING: POS is not attached to any InventoryLocation.");
                return false;
            }
            #endregion

            status = "";
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                try
                {
                    if (!myOrderDet[i].IsVoided)
                    {
                        myOrderDet[i].DoStockOut();
                    }
                }
                catch (Exception X)
                {
                    status += "\n" + X.Message;
                }
            }

            if (status == "")
                return true;
            else
                return false;
        }

        public bool SyncToMagento(out string status)
        {
            /* --- PARAMETER DECLARATION --- */
            QueryCommandCollection cmd = new QueryCommandCollection();

            try
            {
                status = "";
                string user = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
                string password = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
                PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();

                ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
                string SessionId = ws.login(user, password);
                string store = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultStore);
                int cartNo = ws.shoppingCartCreate(SessionId, store);
                int recordCount = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    Item it = new Item(myOrderDet[i].ItemNo);
                    if (it.CategoryName != "SYSTEM")
                        recordCount = recordCount + 1;
                }

                #region Add Product from OrderDet
                PowerPOSLib.Magento.shoppingCartProductEntity[] sp = new PowerPOSLib.Magento.shoppingCartProductEntity[recordCount];
                int j = 0;
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    Item it = new Item(myOrderDet[i].ItemNo);
                    if (it.CategoryName != "SYSTEM")
                    {
                        sp[j] = new PowerPOSLib.Magento.shoppingCartProductEntity();
                        sp[j].product_id = myOrderDet[i].ItemNo;
                        sp[j].qty = Convert.ToDouble(myOrderDet[i].Quantity.GetValueOrDefault(0));
                        j++;
                    }
                }

                if (!ws.shoppingCartProductAdd(SessionId, cartNo, sp, store))
                {
                    status = "Cannot Add Product";
                    return false;
                }
                #endregion

                #region Add Customer
                PowerPOSLib.Magento.shoppingCartCustomerEntity cust = new PowerPOSLib.Magento.shoppingCartCustomerEntity();
                cust.mode = "guest";
                if (myOrderHdr.MembershipNo == "" || myOrderHdr.MembershipNo == "WALK-IN")
                {
                    cust.firstname = "WALK-IN";
                    cust.lastname = "WALK-IN";
                }
                else
                {
                    cust.firstname = CurrentMember.FirstName == null ? CurrentMember.NameToAppear : CurrentMember.FirstName;
                    cust.lastname = CurrentMember.LastName;
                }

                cust.email = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustEmail);

                if (!ws.shoppingCartCustomerSet(SessionId, cartNo, cust, store))
                {
                    status = "Cannot Add Customer";
                    return false;
                }
                #endregion

                #region Add Billing And Shipping Address
                PowerPOSLib.Magento.shoppingCartCustomerAddressEntity[] addresses = new PowerPOSLib.Magento.shoppingCartCustomerAddressEntity[2];
                addresses[0] = new PowerPOSLib.Magento.shoppingCartCustomerAddressEntity();
                addresses[0].mode = "billing";
                if (myOrderHdr.MembershipNo == "" || myOrderHdr.MembershipNo == "WALK-IN")
                {
                    addresses[0].firstname = "WALK-IN";
                    addresses[0].lastname = "WALK-IN";
                }
                else
                {
                    addresses[0].firstname = CurrentMember.FirstName == null ? CurrentMember.NameToAppear : CurrentMember.FirstName;
                    addresses[0].lastname = CurrentMember.LastName;
                }
                addresses[0].street = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustStreet);
                addresses[0].telephone = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustTelephone);
                addresses[0].postcode = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustPostCode);
                addresses[0].country_id = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustCountryID);
                addresses[0].region = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustRegion);
                addresses[0].city = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustCity);

                addresses[1] = new PowerPOSLib.Magento.shoppingCartCustomerAddressEntity();
                addresses[1].mode = "shipping";
                if (myOrderHdr.MembershipNo == "" || myOrderHdr.MembershipNo == "WALK-IN")
                {
                    addresses[1].firstname = "WALK-IN";
                    addresses[1].lastname = "WALK-IN";
                }
                else
                {
                    addresses[1].firstname = CurrentMember.FirstName == null ? CurrentMember.NameToAppear : CurrentMember.FirstName;
                    addresses[1].lastname = CurrentMember.LastName;
                }
                addresses[1].street = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustStreet);
                addresses[1].telephone = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustTelephone);
                addresses[1].postcode = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustPostCode);
                addresses[1].country_id = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustCountryID);
                addresses[1].region = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustRegion);
                addresses[1].city = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustCity);

                if (!ws.shoppingCartCustomerAddresses(SessionId, cartNo, addresses, store))
                {
                    status = "Cannot Add Addresses";
                    return false;
                }
                #endregion

                #region Set Shipping Method
                string delMethod = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultShippingMethod);
                PowerPOSLib.Magento.shoppingCartShippingMethodEntity[] shiptemp = ws.shoppingCartShippingList(SessionId, cartNo, store);
                if (delMethod == "" && shiptemp.GetLength(0) > 0)
                {
                    delMethod = shiptemp[0].code;
                }
                if (!ws.shoppingCartShippingMethod(SessionId, cartNo, delMethod, "pos"))
                {
                    status = "Cannot Add Shipping Method";
                    return false;
                }
                #endregion

                #region Set Payment Mode
                PowerPOSLib.Magento.shoppingCartPaymentMethodResponseEntity[] dump = ws.shoppingCartPaymentList(SessionId, cartNo, store);
                PowerPOSLib.Magento.shoppingCartPaymentMethodEntity payment = new PowerPOSLib.Magento.shoppingCartPaymentMethodEntity();
                payment.method = "checkmo";
                if (dump.GetLength(0) > 0)
                {
                    //Logger.writeLog(dump[0].code);
                    string payMode = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultPaymentType);
                    payment.method = dump[0].code;
                    if (payMode != "")
                    {
                        payment.method = payMode;
                    }

                    /*payment.cc_number = "4556781829595441";
                    payment.cc_exp_month = "02";
                    payment.cc_exp_year = "2017";
                    payment.cc_type = "VI";
                    payment.cc_owner = "Edgeworks";*/

                }


                if (!ws.shoppingCartPaymentMethod(SessionId, cartNo, payment, store))
                {
                    status = "Cannot Add PaymentMethod";
                    return false;
                }
                #endregion

                #region add Cart
                String[] agreement = new string[1];
                agreement[0] = "";
                string result = ws.shoppingCartOrder(SessionId, cartNo, store, agreement);

                PowerPOSLib.Magento.salesOrderEntity so = ws.salesOrderInfo(SessionId, result);
                PowerPOSLib.Magento.orderItemIdQty[] orderItem = new PowerPOSLib.Magento.orderItemIdQty[recordCount];
                if (so != null)
                {
                    //so.items
                    int i = 0;
                    orderItem = new PowerPOSLib.Magento.orderItemIdQty[so.items.GetLength(0)];
                    foreach (PowerPOSLib.Magento.salesOrderItemEntity soItem in so.items)
                    {
                        orderItem[i] = new PowerPOSLib.Magento.orderItemIdQty();
                        orderItem[i].order_item_id = int.Parse(soItem.item_id);
                        orderItem[i].qty = double.Parse(soItem.qty_ordered);
                        i++;
                    }
                }
                string resultInvoice = ws.salesOrderInvoiceCreate(SessionId, result, orderItem, "", AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustEmail), "");
                myOrderHdr.Userfld6 = resultInvoice;
                myOrderHdr.Userfld7 = result;
                myOrderHdr.Save();

                orderItem = null;
                string resultShipment = ws.salesOrderShipmentCreate(SessionId, result, orderItem, null, 0, 0);
                #endregion
            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            if (status == "")
                return true;
            else
                return false;
        }

        public bool Magento_CancelOrder(out string status)
        {
            /* --- PARAMETER DECLARATION --- */
            QueryCommandCollection cmd = new QueryCommandCollection();

            try
            {
                status = "";

                if (myOrderHdr.Userfld6 == null || myOrderHdr.Userfld6 == "")
                {
                    status = "Order not yet synced";
                    return false;
                }

                string user = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
                string password = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
                PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();

                ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
                string SessionId = ws.login(user, password);
                string store = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultStore);

                string res = ws.salesOrderInvoiceCancel(SessionId, myOrderHdr.Userfld6);

            }
            catch (Exception ex)
            {
                status = ex.Message;
            }
            if (status == "")
                return true;
            else
                return false;
        }

        public bool ExecuteStockOutFromSync(out string status)
        {
            /* --- PARAMETER DECLARATION --- */
            QueryCommandCollection cmd = new QueryCommandCollection();

            status = "";

            #region *) Validation: Don't do anything if IntegrateWithInventory is false
            /* If not IntegrateWithInventory, Stock Out will be done at server */
            /*if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                return true;
            }*/
            #endregion

            #region *) Validation: No movement is allowed when there are un-adjusted stock take in place
            /* Return true because we don't want to disturb the cashier with warning */
            /*if (StockTakeController.IsThereUnAdjustedStockTake())
            {
                Logger.writeLog("There are Stock Take in progress, no stock movement is allowed.");
                return true;
            }*/
            #endregion

            #region *) Validation: Make sure POS is attached to InventoryLocation
            if (PointOfSaleInfo.InventoryLocationID == -1)
            {
                status = "POS is not attached to any Inventory Location.\nPlease contact your administrator";
                Logger.writeLog("WARNING: POS is not attached to any InventoryLocation.");
                return false;
            }
            #endregion

            status = "";
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                try
                {
                    if (!myOrderDet[i].IsVoided)
                    {
                        myOrderDet[i].DoStockOut();
                    }
                }
                catch (Exception X)
                {
                    status += "\n" + X.Message;
                }
            }

            if (status == "")
                return true;
            else
                return false;
        }

        public bool SavePaymentTypes()
        {
            try
            {
                QueryCommandCollection cmd = new QueryCommandCollection();

                Query qr = ReceiptDet.CreateQuery();
                qr.QueryType = QueryType.Delete;
                cmd.Add(qr.WHERE(ReceiptDet.Columns.ReceiptHdrID, recHdr.ReceiptHdrID).BuildDeleteCommand());

                ReceiptDetCollection RDetColl = new ReceiptDetCollection();
                RDetColl.Where(ReceiptDet.ReceiptHdrIDColumn.ColumnName, recHdr.ReceiptHdrID);
                RDetColl.Load();
                if (RDetColl.Count > 0)
                {
                    #region
                    if (GetOrderDate() < POSController.FetchLatestCloseCounterTime(GetPointOfSaleId()))
                    {
                        //AccessLogController.AddAccessLog(AccessSource.POS, UserInfo.username, "-", "Override Transaction", GetSavedRefNo() + " - " + myOrderHdr.NettAmount);
                        AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, AccessSource.POS, UserInfo.username, "-", "Override Transaction", GetSavedRefNo() + " - " + myOrderHdr.NettAmount);
                        // change COunterCloseAount
                        CounterCloseLogCollection CCLColl = new CounterCloseLogCollection();
                        CCLColl.Where(CounterCloseLog.StartTimeColumn.ColumnName, SubSonic.Comparison.LessOrEquals, myOrderHdr.OrderDate);
                        CCLColl.Where(CounterCloseLog.EndTimeColumn.ColumnName, SubSonic.Comparison.GreaterOrEquals, myOrderHdr.OrderDate);
                        CCLColl.Load();
                        if (CCLColl.Count > 0)
                        {
                            Query qrCCLUpdate = CounterCloseLog.CreateQuery();
                            qrCCLUpdate.QueryType = QueryType.Update;
                            qrCCLUpdate.AddWhere(CounterCloseLog.Columns.CounterCloseID, CCLColl[0].CounterCloseID);


                            var PaymentTypeUsed = "CASH";
                            var newAmount = 0;
                            // change payment type amount
                            foreach (var recDet in RDetColl)
                            {


                                //CashRecorded,NetsRecorded,VisaRecorded, AmexRecorded, ChinaNets, VoucherRecorded, TotalRecorded, NetsCashCard, NetsFlashPay, NetsATMCard, 
                                if (recDet.PaymentType.ToUpper() == "CASH")
                                {
                                    PaymentTypeUsed = "CASH";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.CashRecorded, CCLColl[0].CashRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "NETS")
                                {
                                    PaymentTypeUsed = "NETS";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsRecorded, CCLColl[0].NetsRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "VISA")
                                {
                                    PaymentTypeUsed = "VISA";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.VisaRecorded, CCLColl[0].NetsRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "AMEX")
                                {
                                    PaymentTypeUsed = "AMEX";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.AmexRecorded, CCLColl[0].AmexRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "CHINANETS")
                                {
                                    PaymentTypeUsed = "CHINANETS";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.ChinaNetsRecorded, CCLColl[0].ChinaNetsRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "VOUCHER")
                                {
                                    PaymentTypeUsed = "VOUCHER";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.VoucherRecorded, CCLColl[0].VoucherRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "NETSCASHCARD")
                                {
                                    PaymentTypeUsed = "NETSCASHCARD";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsCashCardRecorded, CCLColl[0].NetsCashCardRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "NETSFLASHPAY")
                                {
                                    PaymentTypeUsed = "NETSFLASHPAY";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsFlashPayRecorded, CCLColl[0].NetsFlashPayRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType.ToUpper() == "NETSATMCARD")
                                {
                                    PaymentTypeUsed = "NETSATMCARD";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.NetsATMCardRecorded, CCLColl[0].NetsATMCardRecorded - recDet.Amount);
                                }
                                if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("5"))
                                {
                                    PaymentTypeUsed = "Pay5";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.UserColumns.Pay5Recorded, CCLColl[0].Pay5Recorded - recDet.Amount);
                                }
                                if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("6"))
                                {
                                    PaymentTypeUsed = "Pay6";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.UserColumns.Pay6Recorded, CCLColl[0].Pay6Recorded - recDet.Amount);
                                }
                                if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("7"))
                                {
                                    PaymentTypeUsed = "Pay7";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay7Recorded, CCLColl[0].Pay7Recorded - recDet.Amount);
                                }
                                if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("8"))
                                {
                                    PaymentTypeUsed = "Pay8";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay8Recorded, CCLColl[0].Pay8Recorded - recDet.Amount);
                                }
                                if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("9"))
                                {
                                    PaymentTypeUsed = "Pay9";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay9Recorded, CCLColl[0].Pay9Recorded - recDet.Amount);
                                }
                                if (recDet.PaymentType == PaymentTypesController.FetchPaymentByID("10"))
                                {
                                    PaymentTypeUsed = "Pay10";
                                    qrCCLUpdate.AddUpdateSetting(CounterCloseLog.Columns.Pay10Recorded, CCLColl[0].Pay10Recorded - recDet.Amount);
                                }
                            }
                            qrCCLUpdate.Execute();
                        }
                    }
                    #endregion
                }
                Query qrCCLUpdate1 = CounterCloseLog.CreateQuery();
                CounterCloseLogCollection CCLColl1 = new CounterCloseLogCollection();
                if (GetOrderDate() < POSController.FetchLatestCloseCounterTime(GetPointOfSaleId()))
                {
                    //AccessLogController.AddAccessLog(AccessSource.POS, UserInfo.username, "-", "Override Transaction", GetSavedRefNo() + " - " + myOrderHdr.NettAmount);
                    AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, AccessSource.POS, UserInfo.username, "-", "Override Transaction", GetSavedRefNo() + " - " + myOrderHdr.NettAmount);
                    // change COunterCloseAount
                    //CCLColl = new CounterCloseLogCollection();
                    CCLColl1.Where(CounterCloseLog.StartTimeColumn.ColumnName, SubSonic.Comparison.LessOrEquals, myOrderHdr.OrderDate);
                    CCLColl1.Where(CounterCloseLog.EndTimeColumn.ColumnName, SubSonic.Comparison.GreaterOrEquals, myOrderHdr.OrderDate);
                    CCLColl1.Load();
                    if (CCLColl1.Count > 0)
                    {
                        //qrCCLUpdate = CounterCloseLog.CreateQuery();
                        qrCCLUpdate1.QueryType = QueryType.Update;
                        qrCCLUpdate1.AddWhere(CounterCloseLog.Columns.CounterCloseID, CCLColl1[0].CounterCloseID);
                    }
                }

                //var PaymentTypeUsed = "CASH";
                //var newAmount = 0;

                //Receipt details
                for (int i = 0; i < recDet.Count; i++)
                {
                    recDet[i].ReceiptHdrID = recHdr.ReceiptHdrID;
                    recDet[i].ReceiptDetID = recHdr.ReceiptHdrID + "." + i;
                    recDet[i].UniqueID = Guid.NewGuid();
                    cmd.Add(recDet[i].GetInsertCommand(UserInfo.username));

                    //if voucher - change voucher status to be redeemed
                    if (recDet[i].PaymentType == POSController.PAY_VOUCHER)
                    {
                        qr = Voucher.CreateQuery();
                        qr.AddWhere(Voucher.Columns.VoucherNo, recDet[i].PaymentRefNo);
                        qr.AddUpdateSetting(Voucher.Columns.VoucherStatusId, 2);
                        qr.AddUpdateSetting(Voucher.Columns.DateRedeemed, DateTime.Now);
                        cmd.Add(qr.BuildUpdateCommand());

                    }

                    #region

                    // change payment type amount

                    if (CCLColl1 != null)
                    {
                        if (CCLColl1.Count > 0)
                        {
                            var PaymentTypeUsed = "CASH";
                            //CashRecorded,NetsRecorded,VisaRecorded, AmexRecorded, ChinaNets, VoucherRecorded, TotalRecorded, NetsCashCard, NetsFlashPay, NetsATMCard, 
                            if (recDet[i].PaymentType.ToUpper() == "CASH")
                            {
                                PaymentTypeUsed = "CASH";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.CashRecorded, CCLColl1[0].CashRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "NETS")
                            {
                                PaymentTypeUsed = "NETS";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.NetsRecorded, CCLColl1[0].NetsRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "VISA")
                            {
                                PaymentTypeUsed = "VISA";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.VisaRecorded, CCLColl1[0].NetsRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "AMEX")
                            {
                                PaymentTypeUsed = "AMEX";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.AmexRecorded, CCLColl1[0].AmexRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "CHINANETS")
                            {
                                PaymentTypeUsed = "CHINANETS";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.ChinaNetsRecorded, CCLColl1[0].ChinaNetsRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "VOUCHER")
                            {
                                PaymentTypeUsed = "VOUCHER";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.VoucherRecorded, CCLColl1[0].VoucherRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "NETSCASHCARD")
                            {
                                PaymentTypeUsed = "NETSCASHCARD";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.NetsCashCardRecorded, CCLColl1[0].NetsCashCardRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "NETSFLASHPAY")
                            {
                                PaymentTypeUsed = "NETSFLASHPAY";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.NetsFlashPayRecorded, CCLColl1[0].NetsFlashPayRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType.ToUpper() == "NETSATMCARD")
                            {
                                PaymentTypeUsed = "NETSATMCARD";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.NetsATMCardRecorded, CCLColl1[0].NetsATMCardRecorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType == PaymentTypesController.FetchPaymentByID("5"))
                            {
                                PaymentTypeUsed = "Pay5";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.UserColumns.Pay5Recorded, CCLColl1[0].Pay5Recorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType == PaymentTypesController.FetchPaymentByID("6"))
                            {
                                PaymentTypeUsed = "Pay6";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.UserColumns.Pay6Recorded, CCLColl1[0].Pay6Recorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType == PaymentTypesController.FetchPaymentByID("7"))
                            {
                                PaymentTypeUsed = "Pay7";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.Pay7Recorded, CCLColl1[0].Pay7Recorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType == PaymentTypesController.FetchPaymentByID("8"))
                            {
                                PaymentTypeUsed = "Pay8";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.Pay8Recorded, CCLColl1[0].Pay8Recorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType == PaymentTypesController.FetchPaymentByID("9"))
                            {
                                PaymentTypeUsed = "Pay9";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.Pay9Recorded, CCLColl1[0].Pay9Recorded + recDet[i].Amount);
                            }
                            if (recDet[i].PaymentType == PaymentTypesController.FetchPaymentByID("10"))
                            {
                                PaymentTypeUsed = "Pay10";
                                qrCCLUpdate1.AddUpdateSetting(CounterCloseLog.Columns.Pay10Recorded, CCLColl1[0].Pay10Recorded + recDet[i].Amount);
                            }
                        }
                    }

                }

                if (CCLColl1 != null)
                {
                    if (CCLColl1.Count > 0)
                    {
                        qrCCLUpdate1.Execute();
                    }
                }


                    #endregion
                QueryCommand comm = new QueryCommand("Update OrderHdr set Modifiedon= GETDATE() where OrderHdrID = '" + myOrderHdr.OrderHdrID + "'");
                cmd.Add(comm);
                SubSonic.DataService.ExecuteTransaction(cmd);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }
        }
        #endregion

        #region "Pre-Orders"
        public bool setPreOrderInfo(string name, string contactnumber, string locationName)
        {
            if (preOrderInfo == null)
                preOrderInfo = new PreOrderRecord();
            preOrderInfo.Name = name ?? "";
            preOrderInfo.ContactNo = contactnumber ?? "";
            preOrderInfo.CollectionLocation = locationName;

            return true;
        }
        public PreOrderRecord getPreOrderInfo()
        {
            return preOrderInfo;
        }
        #endregion

        #region "Calculators"

        public decimal CalculateGrossAmount()
        {
            decimal total = 0;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                myOrderDet[i].GrossSales = CalculateLineGrossAmount(myOrderDet[i]);
                total += myOrderDet[i].GrossSales.Value;
            }
            myOrderHdr.GrossAmount = CommonUILib.RemoveRoundUp(total);

            return myOrderHdr.GrossAmount;
        }

        public decimal CalculateGrossAmountForDisplay()
        {
            decimal total = 0;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                //myOrderDet[i].GrossSales = CalculateLineGrossAmount(myOrderDet[i]);
                total += CalculateLineGrossAmountForDisplay(myOrderDet[i]);
            }
            myOrderHdr.GrossAmount = CommonUILib.RemoveRoundUp(total);

            return myOrderHdr.GrossAmount;
        }

        public decimal CalculateGrossAmountExcludeLineDiscount()
        {
            decimal total = 0;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                decimal temp = CalculateLineGrossAmountExcludeLineDiscount(myOrderDet[i]);
                total += temp;
            }
            total = CommonUILib.RemoveRoundUp(total);

            return total;
        }

        public decimal CalculateLineGrossAmountForReceipt()
        {
            decimal total = 0;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                decimal temp = CalculateLineGrossAmountForReceipt(myOrderDet[i]);
                total += temp;
            }
            total = CommonUILib.RemoveRoundUp(total);

            return total;
        }

        public decimal CalculateLineGrossAmount(OrderDet oneOrderDet)
        {
            decimal result = 0;
            if (!((bool)oneOrderDet.IsVoided))
            {
                if (oneOrderDet.Item.IsServiceItem.GetValueOrDefault(false)
                    || oneOrderDet.Item.IsOpenPricePackage
                    || oneOrderDet.Item.CategoryName.ToLower() == "system")
                {
                    result += oneOrderDet.Quantity.GetValueOrDefault(0) * oneOrderDet.UnitPrice;
                }
                else
                {
                    result += oneOrderDet.Quantity.GetValueOrDefault(0) * oneOrderDet.OriginalRetailPrice;
                }
                if (oneOrderDet.GSTAmount.HasValue && oneOrderDet.Item.GSTRule.HasValue && oneOrderDet.Item.GSTRule.Value == 1)
                    result += oneOrderDet.GSTAmount.Value;
            }

            return result;
        }

        public decimal CalculateLineGrossAmountExcludeLineDiscount(OrderDet oneOrderDet)
        {
            decimal result = 0;
            if (!((bool)oneOrderDet.IsVoided) && oneOrderDet.ItemNo != "LINE_DISCOUNT")
            {

                result += oneOrderDet.Amount;

            }

            return result;
        }

        public decimal CalculateLineGrossAmountForDisplay(OrderDet oneOrderDet)
        {
            decimal result = 0;
            if (!((bool)oneOrderDet.IsVoided))
            {
                if (oneOrderDet.Item.IsServiceItem.GetValueOrDefault(false)
                    || oneOrderDet.Item.IsOpenPricePackage
                    || oneOrderDet.Item.CategoryName.ToLower() == "system")
                {
                    result += oneOrderDet.Quantity.GetValueOrDefault(0) * oneOrderDet.UnitPrice;
                }
                else
                {
                    result += oneOrderDet.Quantity.GetValueOrDefault(0) * oneOrderDet.OriginalRetailPrice;
                }
                //if (oneOrderDet.GSTAmount.HasValue && oneOrderDet.Item.GSTRule.HasValue && oneOrderDet.Item.GSTRule.Value == 1)
                //    result += oneOrderDet.GSTAmount.Value;
            }

            return result;
        }

        public decimal CalculateLineGrossAmountForReceipt(OrderDet oneOrderDet)
        {
            decimal result = 0;
            if (!((bool)oneOrderDet.IsVoided))
            {
                if (oneOrderDet.Item.IsServiceItem.GetValueOrDefault(false)
                    || oneOrderDet.Item.IsOpenPricePackage
                    || oneOrderDet.Item.CategoryName.ToLower() == "system"
                    || oneOrderDet.IsPriceManuallyChange)
                {
                    result += oneOrderDet.Quantity.GetValueOrDefault(0) * oneOrderDet.UnitPrice;
                }
                else
                {
                    result += oneOrderDet.Quantity.GetValueOrDefault(0) * oneOrderDet.OriginalRetailPrice;
                }
                if (oneOrderDet.GSTAmount.HasValue && oneOrderDet.Item.GSTRule.HasValue && oneOrderDet.Item.GSTRule.Value == 1)
                    result += oneOrderDet.GSTAmount.Value;
            }

            return result;
        }


        //To calculate line discount in dollar term instead of percentage...
        private decimal CalculateDiscountInDollar(int index)
        {
            try
            {
                decimal discount;
                if (myOrderDet[index].IsPromo)
                {
                    discount =
                        myOrderDet[index].OriginalRetailPrice *
                        (decimal)(myOrderDet[index].PromoDiscount / 100);
                }
                else
                {
                    discount =
                        myOrderDet[index].OriginalRetailPrice *
                        (myOrderDet[index].Discount / 100);
                }

                /*
                if (myOrderHdr.Discount > 0)
                {
                    discount = discount * (myOrderHdr.Discount / 100);
                }*/
                int dotPos = discount.ToString("N4").LastIndexOf('.');
                if (dotPos >= 0)
                    discount = decimal.Parse(discount.ToString("N4").Substring(0, dotPos + 3));
                return discount;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region "Add Item To Order"

        public void RemoveLine(OrderDet oDet)
        {
            try
            {
                myOrderDet.Remove(oDet);
                promoCtrl.UndoPromoToOrder();
                promoCtrl.ApplyPromoToOrder();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public void CallOpenPriceItemAdded()
        {
            foreach (var item in _listOpenItemFromHotKeys)
            {
                CallOpenPriceItemAdded(item.Key);
            }
            _listOpenItemFromHotKeys = new Dictionary<string, decimal>();
        }

        public void CallOpenPriceItemAdded(string orderDetID)
        {
            if (OpenPriceItemAdded != null)
                OpenPriceItemAdded(this, orderDetID);
        }

        public void CallOpenPriceItemAddedEditField(string orderDetID, bool IsUsingQuantityField)
        {
            if (OpenPriceItemAddedEditField != null)
                OpenPriceItemAddedEditField(this, orderDetID, IsUsingQuantityField);
        }

        public void CallAutoCaptureWeightItemAdded(string orderDetID)
        {
            if (AutoCaptureWeightItemAdded != null)
                AutoCaptureWeightItemAdded(this, orderDetID);
        }

        public bool CheckRestricted(ArrayList ItemNoList, out ArrayList ItemNoFree, out string statusCollection, out string errorStatus)
        {
            ItemNoFree = new ArrayList();
            statusCollection = "";
            errorStatus = "";
            string status = "";

            try
            {

                for (int i = 0; i < ItemNoList.Count; i++)
                {
                    Item item = new Item(ItemNoList[i]);

                    if (!IsRestricted(item, out status))
                    {
                        statusCollection += status + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        ItemNoFree.Add(ItemNoList[i]);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errorStatus = "Error Check Restriction :" + ex.Message;
                return false;
            }
        }

        public bool IsRestricted(ViewItem inp, out string status)
        {
            Item item = new Item(inp.ItemNo);

            return IsRestricted(item, out status);
        }

        public bool IsRestricted(Item myItem, out string status)
        {
            status = "";
            try
            {
                Category cat = new Category(myItem.CategoryName);
                if (!cat.IsSellingRestriction)
                    return true;

                DateTime start;
                DateTime end;
                string dateStr = "";

                dateStr = string.Format("{0} {1}", DateTime.Now.ToString("dd MMM yyyy"), cat.RestrictedTimeStart);
                DateTime.TryParseExact(dateStr, "dd MMM yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out start);

                dateStr = string.Format("{0} {1}", DateTime.Now.ToString("dd MMM yyyy"), cat.RestrictedTimeEnd);
                DateTime.TryParseExact(dateStr, "dd MMM yyyy hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out end);

                // checking if start is 22:00 and end is 07;00 
                if (end < start)
                {
                    //check if date time is AM and less than < end
                    DateTime startOne = start.AddDays(-1);
                    DateTime endOne = end;


                    DateTime startTwo = start;
                    DateTime endTwo = end.AddDays(1);

                    if (DateTime.Now >= startOne && DateTime.Now <= endOne || DateTime.Now >= startTwo && DateTime.Now <= endTwo)
                    {
                        status = string.Format("Category {0}, Item No {1}, Item Name {2} can not be sale during {3} and {4}", cat.CategoryName, myItem.ItemNo, myItem.ItemName, start.ToString("hh:mm tt"), end.ToString("hh:mm tt"));
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    if (DateTime.Now >= start && DateTime.Now <= end)
                    {
                        status = string.Format("Category {0}, Item No {1}, Item Name, {2} can not be sale during {3} and {4}", cat.CategoryName, myItem.ItemNo, myItem.ItemName, start.ToString("hh:mm tt"), end.ToString("hh:mm tt"));
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                status = "Error check restricted : " + ex.Message;
                Logger.writeLog("Error check restricted : " + ex.Message);
                return false;
            }
        }

        public bool IsPackageRedeemTransaction()
        {
            bool result = false;
            foreach (OrderDet od in myOrderDet)
            {
                if (od.Userflag5.GetValueOrDefault(false))
                    result = true;
            }

            return result;
        }

        public bool AddPackageToOrder
            (Item myItem, int qty, decimal PreferedDiscount,
            bool ApplyPromo, out string status)
        {

            try
            {
                if (!myItem.IsLoaded)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                /*
                if (!(new Category("CategoryName", myItem.CategoryName)).IsForSale)                              
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }
                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                #region *) If Item already exist, Change the Qty and Calculate the Total Amount, dll
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !(myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!myOrderDet[i].IsPreOrder.HasValue ||
                            myOrderDet[i].IsPreOrder == defaultPreOrder)
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        return true;
                    }
                }
                #endregion

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                /*
                if (myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                {
                    tmpdet.IsSpecial = true;
                }*/
                if (myItem.PointGetMode == Item.PointMode.Times)
                {
                    tmpdet.UnitPrice = 0;
                    tmpdet.GrossSales = 0;
                    tmpdet.GiveCommission = true;
                }
                tmpdet.OriginalRetailPrice = 0;



                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                /// Commented @ 09 Nov 2010 By Evan
                /// Reason: Possible Bug
                ///     *) Unit Price & Gross Sales has been set before
                ///     *) Why Gross Sales is not x Qty?
                //else
                //{
                //    tmpdet.UnitPrice = myItem.RetailPrice;
                //    tmpdet.GrossSales = myItem.RetailPrice;
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                tmpdet.OrderDetDate = DateTime.Now;

                /// Save Course (MainItem)
                //tmpdet.Userfld2 = theItem.ItemNo;
                tmpdet.Userflag5 = true;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddPackageToOrder
            (Item myItem, int qty, decimal PreferedDiscount,
            bool ApplyPromo, bool isCheckPackage, string packageRefNo, out string status)
        {

            try
            {
                if (!myItem.IsLoaded)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                /*
                if (!(new Category("CategoryName", myItem.CategoryName)).IsForSale)                              
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }
                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                #region *) If Item already exist, Change the Qty and Calculate the Total Amount, dll
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !(myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!myOrderDet[i].IsPreOrder.HasValue ||
                            myOrderDet[i].IsPreOrder == defaultPreOrder)
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        return true;
                    }
                }
                #endregion

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                /*
                if (myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                {
                    tmpdet.IsSpecial = true;
                }*/
                if (myItem.PointGetMode == Item.PointMode.Times)
                {
                    tmpdet.UnitPrice = 0;
                    tmpdet.GrossSales = 0;
                    tmpdet.GiveCommission = true;
                }
                tmpdet.OriginalRetailPrice = 0;



                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                /// Commented @ 09 Nov 2010 By Evan
                /// Reason: Possible Bug
                ///     *) Unit Price & Gross Sales has been set before
                ///     *) Why Gross Sales is not x Qty?
                //else
                //{
                //    tmpdet.UnitPrice = myItem.RetailPrice;
                //    tmpdet.GrossSales = myItem.RetailPrice;
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                tmpdet.OrderDetDate = DateTime.Now;

                /// Save Course (MainItem)
                //tmpdet.Userfld2 = theItem.ItemNo;
                tmpdet.Userflag5 = isCheckPackage;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }
                tmpdet.PointItemNo = packageRefNo;
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddPackageToOrder
            (Item myItem, int qty, decimal PreferedDiscount,
            bool ApplyPromo, bool isCheckPackage, string packageRefNo, decimal BreakdownPrice, out string status)
        {

            try
            {
                if (!myItem.IsLoaded)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                /*
                if (!(new Category("CategoryName", myItem.CategoryName)).IsForSale)                              
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }
                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                #region *) If Item already exist, Change the Qty and Calculate the Total Amount, dll
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !(myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!myOrderDet[i].IsPreOrder.HasValue ||
                            myOrderDet[i].IsPreOrder == defaultPreOrder)
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        return true;
                    }
                }
                #endregion

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                //tmpdet.UnitPrice = Price;
                //tmpdet.Amount = qty * Price;
                tmpdet.PackageBreakdownAmount = qty * BreakdownPrice;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                /*
                if (myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                {
                    tmpdet.IsSpecial = true;
                }*/
                if (myItem.PointGetMode == Item.PointMode.Times)
                {
                    tmpdet.UnitPrice = 0;
                    tmpdet.GrossSales = 0;
                    tmpdet.GiveCommission = true;
                }
                tmpdet.OriginalRetailPrice = 0;



                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                /// Commented @ 09 Nov 2010 By Evan
                /// Reason: Possible Bug
                ///     *) Unit Price & Gross Sales has been set before
                ///     *) Why Gross Sales is not x Qty?
                //else
                //{
                //    tmpdet.UnitPrice = myItem.RetailPrice;
                //    tmpdet.GrossSales = myItem.RetailPrice;
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                tmpdet.OrderDetDate = DateTime.Now;

                /// Save Course (MainItem)
                //tmpdet.Userfld2 = theItem.ItemNo;
                tmpdet.Userflag5 = isCheckPackage;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }
                tmpdet.PointItemNo = packageRefNo;
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddSpecialItemToOrder
            (ViewItem myItem, decimal quantity, decimal amount,
             decimal PreferedDiscount, bool ApplyPromo, string remark, string specialBarcode, out string status)
        {
            try
            {
                /*if (!myItem.IsServiceItem.HasValue || !myItem.IsServiceItem.Value)
                {
                    status = "This item is not a service item";
                    return false;
                }*/

                if (ApplyPromo)
                {
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.Remark = remark;
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                tmpdet.UnitPrice = amount;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = amount;
                tmpdet.Quantity = quantity;
                tmpdet.GrossSales = quantity * amount;

                if (myItem.IsCommission.HasValue)
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                else
                    tmpdet.GiveCommission = false;

                if (tmpdet.Item.IsNonDiscountable)
                    tmpdet.Discount = 0;
                else
                    tmpdet.Discount = PreferedDiscount;

                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.SpecialBarcode = specialBarcode;
                CalculateLineAmount(ref tmpdet);
                //tmpdet.CreatedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    promoCtrl.ApplyPromoToOrder();
                }
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddSpecialItemToOrderWithPriceMode
            (ViewItem myItem, decimal quantity, decimal amount,
             decimal PreferedDiscount, bool ApplyPromo, string remark, string specialBarcode, string PriceMode, out string status)
        {
            try
            {
                Item item = new Item(myItem.ItemNo);

                /*if (!myItem.IsServiceItem.HasValue || !myItem.IsServiceItem.Value)
                {
                    status = "This item is not a service item";
                    return false;
                }*/

                if (ApplyPromo)
                {
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.Remark = remark;
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.PriceMode = PriceMode;

                bool isWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcodeForQuantity), false);

                if (isWeight)
                {
                    switch (PriceMode)
                    {
                        case "P1":
                            tmpdet.UnitPrice = item.P1Price ?? myItem.RetailPrice;
                            tmpdet.OriginalRetailPrice = item.P1Price ?? myItem.RetailPrice;
                            break;
                        case "P2":
                            tmpdet.UnitPrice = item.P2Price ?? myItem.RetailPrice;
                            tmpdet.OriginalRetailPrice = item.P2Price ?? myItem.RetailPrice;
                            break;
                        case "P3":
                            tmpdet.UnitPrice = item.P3Price ?? myItem.RetailPrice;
                            tmpdet.OriginalRetailPrice = item.P3Price ?? myItem.RetailPrice;
                            break;
                        case "P4":
                            tmpdet.UnitPrice = item.P4Price ?? myItem.RetailPrice;
                            tmpdet.OriginalRetailPrice = item.P4Price ?? myItem.RetailPrice;
                            break;
                        case "P5":
                            tmpdet.UnitPrice = item.P5Price ?? myItem.RetailPrice;
                            tmpdet.OriginalRetailPrice = item.P5Price ?? myItem.RetailPrice;
                            break;
                        default:
                            tmpdet.UnitPrice = amount;
                            tmpdet.OriginalRetailPrice = amount;
                            break;
                    }
                }
                else
                {
                    tmpdet.UnitPrice = amount;
                    tmpdet.OriginalRetailPrice = amount;
                }

                tmpdet.Quantity = quantity;
                tmpdet.GrossSales = tmpdet.UnitPrice * tmpdet.Quantity;

                if (myItem.IsCommission.HasValue)
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                else
                    tmpdet.GiveCommission = false;

                if (tmpdet.Item.IsNonDiscountable)
                    tmpdet.Discount = 0;
                else
                    tmpdet.Discount = PreferedDiscount;

                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.SpecialBarcode = specialBarcode;
                CalculateLineAmount(ref tmpdet);
                //tmpdet.CreatedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    promoCtrl.ApplyPromoToOrder();
                }
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrder
            (Item myItem, int qty, decimal PreferedDiscount,
            bool ApplyPromo, out string status)
        {

            try
            {
                if (myItem.IsNew)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }
                /*
                if (!(new Category("CategoryName", myItem.CategoryName)).IsForSale)                              
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo != myItem.ItemNo) continue;
                    if (myItem.IsServiceItem.GetValueOrDefault(false)) continue;
                    if (myOrderDet[i].IsSpecial) continue;
                    if (myOrderDet[i].IsExchange) continue;
                    if ((bool)myOrderDet[i].IsVoided) continue;
                    if ((bool)myOrderDet[i].IsFreeOfCharge) continue;
                    if ((bool)myOrderDet[i].IsPromo) continue;
                    if (myOrderDet[i].IsPreOrder.HasValue && myOrderDet[i].IsPreOrder != defaultPreOrder) continue;
                    if ((bool)myOrderDet[i].IsPromoFreeOfCharge) continue;
                    if (myOrderDet[i].Quantity < 0) continue;

                    //if (myOrderDet[i].ItemNo == myItem.ItemNo
                    //        && !(myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                    //        && !myOrderDet[i].IsExchange
                    //        && !((bool)myOrderDet[i].IsVoided)
                    //        && (!((bool)myOrderDet[i].IsFreeOfCharge))
                    //        && (!((bool)myOrderDet[i].IsPromo))
                    //        && (!myOrderDet[i].IsPreOrder.HasValue ||
                    //        myOrderDet[i].IsPreOrder == defaultPreOrder)
                    //        && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                    //    )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                            //amount?
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        //myOrderDet[i].ModifiedOn = DateTime.Now;
                        OrderDet tmpOD = myOrderDet[i];
                        CalculateLineAmount(ref tmpOD);

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.GrossSales = myItem.RetailPrice * qty;
                /*
                if (myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                {
                    tmpdet.IsSpecial = true;
                }*/

                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                //bool addRemarks = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.EnableKeyInOpenPriceItemName), false);
                //if (addRemarks)
                //{
                //WinPowerPOS.OrderForms.frmKeyboard keyOpenPriceRemarks = new inPowerPOS.OrderForms.frmKeyboard();
                //keyOpenPriceRemarks.IsInteger = false;
                //keyOpenPriceRemarks.textMessage = "Enter Remarks";
                //keyOpenPriceRemarks.ShowDialog();
                //tmpdet.Remark = keyOpenPriceRemarks.value;
                //}


                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = myItem.RetailPrice;
                    tmpdet.GrossSales = myItem.RetailPrice;
                }

                //if (_listOpenItemFromHotKeys.ContainsKey(myItem.ItemNo))
                //{
                //    tmpdet.UnitPrice = _listOpenItemFromHotKeys[myItem.ItemNo];
                //    tmpdet.GrossSales = _listOpenItemFromHotKeys[myItem.ItemNo];
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    if (discountname != "" && discountname != null)
                    {
                        try
                        {
                            if (Convert.ToDouble(myItem.GetColumnValue(discountname)) >= 0)
                            {
                                tmpdet.Discount = Convert.ToDecimal(myItem.GetColumnValue(discountname).ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }

                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null)
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion
                CalculateLineAmount(ref tmpdet);

                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                //tmpdet.ModifiedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                }
                //Open price from a Hot Keys
                //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsServiceItem.GetValueOrDefault(false))
                //{
                //    if (OpenPriceItemAddedHotKey != null)
                //        OpenPriceItemAddedHotKey(this, tmpdet.OrderDetID);
                //}
                //Open price from order taking form
                if ((myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory) || myItem.IsOpenPricePackage)
                    CallOpenPriceItemAdded(tmpdet.OrderDetID);

                //Auto Capture Weight from order taking form
                if ((myItem.AutoCaptureWeight && myItem.IsInInventory))
                    CallAutoCaptureWeightItemAdded(tmpdet.OrderDetID);
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrderWithPriceMode
         (Item myItem, int qty, decimal PreferedDiscount,
         bool ApplyPromo, string PriceMode, out string status)
        {

            try
            {
                if (myItem.IsNew)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo != myItem.ItemNo) continue;
                    if (myItem.IsServiceItem.GetValueOrDefault(false)) continue;
                    if (myOrderDet[i].IsSpecial) continue;
                    if (myOrderDet[i].IsExchange) continue;
                    if ((bool)myOrderDet[i].IsVoided) continue;
                    if ((bool)myOrderDet[i].IsFreeOfCharge) continue;
                    if ((bool)myOrderDet[i].IsPromo) continue;
                    if (myOrderDet[i].IsPreOrder.HasValue && myOrderDet[i].IsPreOrder != defaultPreOrder) continue;
                    if ((bool)myOrderDet[i].IsPromoFreeOfCharge) continue;
                    if (myOrderDet[i].Quantity < 0) continue;
                    if (myOrderDet[i].ItemNo == myItem.ItemNo && myOrderDet[i].PriceMode != PriceMode) continue;
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                            //amount?
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        //myOrderDet[i].ModifiedOn = DateTime.Now;
                        OrderDet tmpOD = myOrderDet[i];
                        CalculateLineAmount(ref tmpOD);

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.PriceMode = PriceMode;


                switch (PriceMode)
                {
                    case "P1":
                        tmpdet.UnitPrice = myItem.P1Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P1Price ?? myItem.RetailPrice;
                        break;
                    case "P2":
                        tmpdet.UnitPrice = myItem.P2Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P2Price ?? myItem.RetailPrice;
                        break;
                    case "P3":
                        tmpdet.UnitPrice = myItem.P3Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P3Price ?? myItem.RetailPrice;
                        break;
                    case "P4":
                        tmpdet.UnitPrice = myItem.P4Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P4Price ?? myItem.RetailPrice;
                        break;
                    case "P5":
                        tmpdet.UnitPrice = myItem.P5Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P5Price ?? myItem.RetailPrice;
                        break;
                    default:
                        tmpdet.UnitPrice = myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                        break;
                }
                tmpdet.GrossSales = tmpdet.UnitPrice * tmpdet.Quantity;

                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = myItem.RetailPrice;
                    tmpdet.GrossSales = myItem.RetailPrice;
                }

                //if (_listOpenItemFromHotKeys.ContainsKey(myItem.ItemNo))
                //{
                //    tmpdet.UnitPrice = _listOpenItemFromHotKeys[myItem.ItemNo];
                //    tmpdet.GrossSales = _listOpenItemFromHotKeys[myItem.ItemNo];
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    if (discountname != "" && discountname != null)
                    {
                        try
                        {
                            if (Convert.ToDouble(myItem.GetColumnValue(discountname)) >= 0)
                            {
                                tmpdet.Discount = Convert.ToDecimal(myItem.GetColumnValue(discountname).ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                CalculateLineAmount(ref tmpdet);

                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                //tmpdet.ModifiedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                }
                //Open price from a Hot Keys
                //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsServiceItem.GetValueOrDefault(false))
                //{
                //    if (OpenPriceItemAddedHotKey != null)
                //        OpenPriceItemAddedHotKey(this, tmpdet.OrderDetID);
                //}
                //Open price from order taking form
                if ((myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory) || myItem.IsOpenPricePackage)
                    CallOpenPriceItemAdded(tmpdet.OrderDetID);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrderWithPriceMode
         (Item myItem, decimal qty, decimal PreferedDiscount,
         bool ApplyPromo, string PriceMode, bool isAlreadyWeighted, out string status)
        {

            try
            {
                if (myItem.IsNew)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo != myItem.ItemNo) continue;
                    if (myItem.IsServiceItem.GetValueOrDefault(false)) continue;
                    if (myOrderDet[i].IsSpecial) continue;
                    if (myOrderDet[i].IsExchange) continue;
                    if ((bool)myOrderDet[i].IsVoided) continue;
                    if ((bool)myOrderDet[i].IsFreeOfCharge) continue;
                    if ((bool)myOrderDet[i].IsPromo) continue;
                    if (myOrderDet[i].IsPreOrder.HasValue && myOrderDet[i].IsPreOrder != defaultPreOrder) continue;
                    if ((bool)myOrderDet[i].IsPromoFreeOfCharge) continue;
                    if (myOrderDet[i].Quantity < 0) continue;
                    if (myOrderDet[i].ItemNo == myItem.ItemNo && myOrderDet[i].PriceMode != PriceMode) continue;
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                            //amount?
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        //myOrderDet[i].ModifiedOn = DateTime.Now;
                        OrderDet tmpOD = myOrderDet[i];
                        CalculateLineAmount(ref tmpOD);

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.PriceMode = PriceMode;


                switch (PriceMode)
                {
                    case "P1":
                        tmpdet.UnitPrice = myItem.P1Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P1Price ?? myItem.RetailPrice;
                        break;
                    case "P2":
                        tmpdet.UnitPrice = myItem.P2Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P2Price ?? myItem.RetailPrice;
                        break;
                    case "P3":
                        tmpdet.UnitPrice = myItem.P3Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P3Price ?? myItem.RetailPrice;
                        break;
                    case "P4":
                        tmpdet.UnitPrice = myItem.P4Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P4Price ?? myItem.RetailPrice;
                        break;
                    case "P5":
                        tmpdet.UnitPrice = myItem.P5Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.P5Price ?? myItem.RetailPrice;
                        break;
                    default:
                        tmpdet.UnitPrice = myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                        break;
                }
                tmpdet.GrossSales = tmpdet.UnitPrice * tmpdet.Quantity;

                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = myItem.RetailPrice;
                    tmpdet.GrossSales = myItem.RetailPrice;
                }

                //if (_listOpenItemFromHotKeys.ContainsKey(myItem.ItemNo))
                //{
                //    tmpdet.UnitPrice = _listOpenItemFromHotKeys[myItem.ItemNo];
                //    tmpdet.GrossSales = _listOpenItemFromHotKeys[myItem.ItemNo];
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    if (discountname != "" && discountname != null)
                    {
                        try
                        {
                            if (Convert.ToDouble(myItem.GetColumnValue(discountname)) >= 0)
                            {
                                tmpdet.Discount = Convert.ToDecimal(myItem.GetColumnValue(discountname).ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                CalculateLineAmount(ref tmpdet);

                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                //tmpdet.ModifiedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                }
                //Open price from a Hot Keys
                //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsServiceItem.GetValueOrDefault(false))
                //{
                //    if (OpenPriceItemAddedHotKey != null)
                //        OpenPriceItemAddedHotKey(this, tmpdet.OrderDetID);
                //}
                //Open price from order taking form
                if ((myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory) || myItem.IsOpenPricePackage)
                    CallOpenPriceItemAdded(tmpdet.OrderDetID);

                //Auto Capture Weight from order taking form
                if ((myItem.AutoCaptureWeight && myItem.IsInInventory && !isAlreadyWeighted))
                    CallAutoCaptureWeightItemAdded(tmpdet.OrderDetID);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }
        public bool AddItemToOrderWithPriceModeShopify
        (Item myItem, decimal qty, decimal PreferedDiscount, bool ApplyPromo, string PriceMode, bool isAlreadyWeighted, decimal price, out string status)
        {

            try
            {
                if (myItem.IsNew)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo != myItem.ItemNo) continue;
                    if (myItem.IsServiceItem.GetValueOrDefault(false)) continue;
                    if (myOrderDet[i].IsSpecial) continue;
                    if (myOrderDet[i].IsExchange) continue;
                    if ((bool)myOrderDet[i].IsVoided) continue;
                    if ((bool)myOrderDet[i].IsFreeOfCharge) continue;
                    if ((bool)myOrderDet[i].IsPromo) continue;
                    if (myOrderDet[i].IsPreOrder.HasValue && myOrderDet[i].IsPreOrder != defaultPreOrder) continue;
                    if ((bool)myOrderDet[i].IsPromoFreeOfCharge) continue;
                    if (myOrderDet[i].Quantity < 0) continue;
                    if (myOrderDet[i].ItemNo == myItem.ItemNo && myOrderDet[i].PriceMode != PriceMode) continue;
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * price;
                            //amount?
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * price;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        //myOrderDet[i].ModifiedOn = DateTime.Now;
                        OrderDet tmpOD = myOrderDet[i];
                        CalculateLineAmount(ref tmpOD);

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.PriceMode = PriceMode;



                tmpdet.UnitPrice = price;
                tmpdet.OriginalRetailPrice = price;

                tmpdet.GrossSales = price * tmpdet.Quantity;

                //Check Item New Price
                int EventID;

                tmpdet.UnitPrice = price;

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    if (discountname != "" && discountname != null)
                    {
                        try
                        {
                            if (Convert.ToDouble(myItem.GetColumnValue(discountname)) >= 0)
                            {
                                tmpdet.Discount = Convert.ToDecimal(myItem.GetColumnValue(discountname).ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                CalculateLineAmount(ref tmpdet);

                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                //tmpdet.ModifiedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                }

                if ((myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory) || myItem.IsOpenPricePackage)
                    CallOpenPriceItemAdded(tmpdet.OrderDetID);

                //Auto Capture Weight from order taking form
                if ((myItem.AutoCaptureWeight && myItem.IsInInventory && !isAlreadyWeighted))
                    CallAutoCaptureWeightItemAdded(tmpdet.OrderDetID);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrder
            (Item myItem, int qty, decimal PreferedDiscount,
            bool ApplyPromo, out string status, bool needToEnterOpenPrice)
        {

            try
            {
                if (myItem.IsNew)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }
                /*
                if (!(new Category("CategoryName", myItem.CategoryName)).IsForSale)                              
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo != myItem.ItemNo) continue;
                    if (myItem.IsServiceItem.GetValueOrDefault(false)) continue;
                    if (myOrderDet[i].IsSpecial) continue;
                    if (myOrderDet[i].IsExchange) continue;
                    if ((bool)myOrderDet[i].IsVoided) continue;
                    if ((bool)myOrderDet[i].IsFreeOfCharge) continue;
                    if ((bool)myOrderDet[i].IsPromo) continue;
                    if (myOrderDet[i].IsPreOrder.HasValue && myOrderDet[i].IsPreOrder != defaultPreOrder) continue;
                    if ((bool)myOrderDet[i].IsPromoFreeOfCharge) continue;
                    if (myOrderDet[i].Quantity < 0) continue;

                    //if (myOrderDet[i].ItemNo == myItem.ItemNo
                    //        && !(myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                    //        && !myOrderDet[i].IsExchange
                    //        && !((bool)myOrderDet[i].IsVoided)
                    //        && (!((bool)myOrderDet[i].IsFreeOfCharge))
                    //        && (!((bool)myOrderDet[i].IsPromo))
                    //        && (!myOrderDet[i].IsPreOrder.HasValue ||
                    //        myOrderDet[i].IsPreOrder == defaultPreOrder)
                    //        && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                    //    )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                            //amount?
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        OrderDet tmpOD = myOrderDet[i];
                        //myOrderDet[i].ModifiedOn = DateTime.Now;
                        CalculateLineAmount(ref tmpOD);

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.GrossSales = myItem.RetailPrice * qty;
                /*
                if (myItem.IsServiceItem.HasValue && myItem.IsServiceItem.Value)
                {
                    tmpdet.IsSpecial = true;
                }*/

                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;



                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = myItem.RetailPrice;
                    tmpdet.GrossSales = myItem.RetailPrice;
                }

                //if (_listOpenItemFromHotKeys.ContainsKey(myItem.ItemNo))
                //{
                //    tmpdet.UnitPrice = _listOpenItemFromHotKeys[myItem.ItemNo];
                //    tmpdet.GrossSales = _listOpenItemFromHotKeys[myItem.ItemNo];
                //}

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    if (discountname != "" && discountname != null)
                    {
                        if (Convert.ToDouble(myItem.GetColumnValue(discountname)) >= 0)
                        {
                            tmpdet.Discount = Convert.ToDecimal(myItem.GetColumnValue(discountname).ToString());
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                CalculateLineAmount(ref tmpdet);

                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                //tmpdet.ModifiedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                }
                //Open price from a Hot Keys
                //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsServiceItem.GetValueOrDefault(false))
                //{
                //    if (OpenPriceItemAddedHotKey != null)
                //        OpenPriceItemAddedHotKey(this, tmpdet.OrderDetID);
                //}
                //Open price from order taking form
                if ((myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory) || myItem.IsOpenPricePackage)
                {
                    if (needToEnterOpenPrice)
                        CallOpenPriceItemAdded(tmpdet.OrderDetID);
                    else
                        _listOpenItemFromHotKeys.Add(tmpdet.OrderDetID, 0);
                }

                //Auto Capture Weight from order taking form
                if ((myItem.AutoCaptureWeight && myItem.IsInInventory))
                    CallAutoCaptureWeightItemAdded(tmpdet.OrderDetID);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrder
            (ViewItem myItem, int qty,
            decimal PreferedDiscount,
            bool ApplyPromo, out string status)
        {

            try
            {
                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                /*
                if (!myItem.IsForSale)
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                /*#region check item promo possibility 
                string sqlCheckItemGroup = "Select Distinct ItemGroupID from ItemGroupMap where deleted = 0 and itemno = '" + myItem.ItemNo + "'";
                DataTable dtItemGroup = DataService.GetDataSet(new QueryCommand(sqlCheckItemGroup)).Tables[0];
                string possibleItemGroupID = "";
                for (int j = 0; j < dtItemGroup.Rows.Count; j++)
                {
                    if (dtItemGroup.Rows[j][0].ToString() != "0")
                    {
                        possibleItemGroupID += dtItemGroup.Rows[j][0].ToString() + ",";
                    }
                }
                possibleItemGroupID = possibleItemGroupID.TrimEnd(',');
                
                string ItemGroupList = "-1";
                if (possibleItemGroupID != "")
                    ItemGroupList = possibleItemGroupID;

                int posid = PointOfSaleInfo.PointOfSaleID;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
                {
                    if (MembershipApplied())
                    {
                        if (GetMemberInfo() != null && GetMemberInfo().MembershipNo != "" && GetMemberInfo().MembershipNo != "WALK-IN")
                        {
                            //validation if outlet order
                            PointOfSaleCollection posColl = new PointOfSaleCollection();
                            posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, GetMemberInfo().MembershipNo);
                            posColl.Load();
                            if (posColl.Count > 0)
                                posid = posColl[0].PointOfSaleID;

                        }
                    }
                }

                int memberGroupID = 0;
                if (MembershipApplied())
                {
                    memberGroupID = GetMemberInfo().MembershipGroupId;
                }

                string ItemNoList = "'" + myItem.ItemNo + "',";

                DataSet ds = SPs.FetchAllPossiblePromoAnyXofAllItemsMemberGroup(ItemNoList, MembershipApplied() && GetMemberInfo().MembershipNo != "WALK-IN", posid, memberGroupID, myItem.CategoryName, ItemGroupList).GetDataSet();
                DataTable dt = ds.Tables[0];
                bool isHavePromo = false;

                if (dt.Rows.Count > 0)
                    isHavePromo = true;
                
                #endregion*/


                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    //if (isHavePromo)
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //check restricted]
                Item item = new Item(myItem.ItemNo);
                if (!IsRestricted(item, out status))
                {
                    return false;
                }

                //Load default Pre Order value                

                string itemno = myItem.ItemNo;


                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {

                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !myItem.IsServiceItem.GetValueOrDefault(false)
                            && !myOrderDet[i].IsSpecial
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge)
                            && (myOrderDet[i].Quantity >= 0))
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;
                        OrderDet tmpOD = myOrderDet[i];
                        CalculateLineAmount(ref tmpOD);
                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.GrossSales = myItem.RetailPrice;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = myItem.RetailPrice;
                    tmpdet.GrossSales = myItem.RetailPrice;
                }

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    decimal tempDisc = 0;
                    if (discountname != "" && discountname != null)
                    {
                        if (decimal.TryParse(myItem.GetColumnValue(discountname).ToString(), out tempDisc))
                        {
                            tmpdet.Discount = tempDisc;
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                CalculateLineAmount(ref tmpdet);
                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.CreatedOn = DateTime.Now;



                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    //if (isHavePromo)
                    //{
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                    //}
                }
                Item theItem = new Item(myItem.ItemNo);

                //Open price from order taking form
                if (myItem.IsServiceItem.GetValueOrDefault(false) || theItem.IsOpenPricePackage)
                    //&& myItem.IsInInventory)
                    CallOpenPriceItemAdded(tmpdet.OrderDetID);

                //Auto Capture Weight from order taking form
                if ((myItem.AutoCaptureWeight.GetValueOrDefault(false) && myItem.IsInInventory))
                    CallAutoCaptureWeightItemAdded(tmpdet.OrderDetID);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrderWithPriceMode
            (ViewItem myItem, int qty,
            decimal PreferedDiscount,
            bool ApplyPromo, string PriceMode, out string status)
        {

            try
            {
                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    //if (isHavePromo)
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //check restricted]
                Item item = new Item(myItem.ItemNo);
                if (!IsRestricted(item, out status))
                {
                    return false;
                }

                //Load default Pre Order value                

                string itemno = myItem.ItemNo;

                Logger.writeLog("Promo Log - 10. Check the item already exist");
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {

                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !myItem.IsServiceItem.GetValueOrDefault(false)
                            && !myOrderDet[i].IsSpecial
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge)
                            && (myOrderDet[i].Quantity >= 0)
                            && myOrderDet[i].PriceMode == PriceMode)
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;
                        OrderDet tmpOD = myOrderDet[i];
                        CalculateLineAmount(ref tmpOD);
                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }

                        return true;
                    }
                }
                Logger.writeLog("Promo Log - 10. Finish Check the item already exist");

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.PriceMode = PriceMode;

                switch (PriceMode)
                {
                    case "P1":
                        tmpdet.UnitPrice = item.P1Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = item.P1Price ?? myItem.RetailPrice;
                        break;
                    case "P2":
                        tmpdet.UnitPrice = item.P2Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = item.P2Price ?? myItem.RetailPrice;
                        break;
                    case "P3":
                        tmpdet.UnitPrice = item.P3Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = item.P3Price ?? myItem.RetailPrice;
                        break;
                    case "P4":
                        tmpdet.UnitPrice = item.P4Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = item.P4Price ?? myItem.RetailPrice;
                        break;
                    case "P5":
                        tmpdet.UnitPrice = item.P5Price ?? myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = item.P5Price ?? myItem.RetailPrice;
                        break;
                    default:
                        tmpdet.UnitPrice = myItem.RetailPrice;
                        tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                        break;
                }
                tmpdet.GrossSales = tmpdet.UnitPrice * tmpdet.Quantity;

                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }


                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    decimal tempDisc = 0;
                    if (discountname != "" && discountname != null)
                    {
                        if (decimal.TryParse(myItem.GetColumnValue(discountname).ToString(), out tempDisc))
                        {
                            tmpdet.Discount = tempDisc;
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;

                #region apply auto membership discount
                string pricelevel = "";
                if (MembershipApplied() && GetMemberInfo().MembershipGroup.Userfld1 != null && GetMemberInfo().MembershipGroup.Userfld1 != "")
                    pricelevel = GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    try
                    {
                        //clearDiscount(0);
                        ApplyDiscountOrderDet(pricelevel, ref tmpdet, true);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }
                #endregion

                Logger.writeLog("Promo Log - 10. Apply Customer PRice");

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);
                Logger.writeLog("Promo Log - 10. Finish Apply Customer PRice");
                
                Logger.writeLog("Promo Log - 10. Calculate Line Amount");
                CalculateLineAmount(ref tmpdet);
                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.CreatedOn = DateTime.Now;
                Logger.writeLog("Promo Log - 10. Finish Calculate Line Amount");


                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    //if (isHavePromo)
                    //{
                    promoCtrl.ApplyPromoToOrder();
                    TryMergeOrderLine(tmpdet.OrderDetID);
                    //}
                }
                Item theItem = new Item(myItem.ItemNo);

                Logger.writeLog("Promo Log - 10. Open Price");
                //Open price from order taking form
                if (myItem.IsServiceItem.GetValueOrDefault(false) || theItem.IsOpenPricePackage)
                    //&& myItem.IsInInventory)
                    CallOpenPriceItemAdded(tmpdet.OrderDetID);
                Logger.writeLog("Promo Log - 10. Finish OPen Price");

                Logger.writeLog("Promo Log - 10. AutoCaptureWeight");
                //Auto Capture Weight from order taking form
                if ((myItem.AutoCaptureWeight.GetValueOrDefault(false) && myItem.IsInInventory))
                    CallAutoCaptureWeightItemAdded(tmpdet.OrderDetID);
                Logger.writeLog("Promo Log - 10. Finish AutoCaptureWeight");
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddItemToOrder
            (ViewItem myItem, int qty,
            decimal PreferedDiscount,
            bool ApplyPromo, out string status, bool IsItemNo)
        {

            try
            {
                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                /*
                if (!myItem.IsForSale)
                {
                    status = "Item is not for sale";
                    return false;
                }*/
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //check restricted]
                Item item = new Item(myItem.ItemNo);
                if (!IsRestricted(item, out status))
                {
                    return false;
                }

                //Load default Pre Order value                

                string itemno = myItem.ItemNo;


                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {

                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !myItem.IsServiceItem.GetValueOrDefault(false)
                            && !myOrderDet[i].IsSpecial
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge)
                            && (myOrderDet[i].Quantity >= 0))
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                        
                        myOrderDet[i].Quantity += qty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                            && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }

                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }

                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.GrossSales = myItem.RetailPrice;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = myItem.RetailPrice;
                    tmpdet.GrossSales = myItem.RetailPrice;
                }

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    decimal tempDisc = 0;
                    if (discountname != "" && discountname != null)
                    {
                        if (decimal.TryParse(myItem.GetColumnValue(discountname).ToString(), out tempDisc))
                        {
                            tmpdet.Discount = tempDisc;
                        }
                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;

                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);

                CalculateLineAmount(ref tmpdet);
                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }
                tmpdet.CreatedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }
                Item theItem = new Item(myItem.ItemNo);

                if ((myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory) || theItem.IsOpenPricePackage)
                {
                    CallOpenPriceItemAddedEditField(tmpdet.OrderDetID, IsItemNo);
                }
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }



        /*
        public bool AddInstallmentItemToOrder
            (decimal amount, decimal PreferedDiscount, string InstallmentDetRefNo,
            bool ApplyPromo, out string status)
        {
            try
            {

                Item myItem = new Item(Item.Columns.ItemNo, InstallmentController.INSTALLMENT_ITEM);

                if (myItem.IsNew)
                {
                    status = "Installment Item is not properly setup. Contact your system Administrator";
                    return false;
                }

                if (ApplyPromo)
                {
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == InstallmentController.INSTALLMENT_ITEM &&
                        myOrderDet[i].VoucherNo == InstallmentDetRefNo
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        //item already existed                        
                        status = "You can only use one pay the same installment once.";


                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }

                        return false;
                    }
                }
                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                tmpdet.Quantity = 1;
                tmpdet.UnitPrice = amount;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = amount;
                tmpdet.Amount = amount;
                tmpdet.GrossSales = amount;
                tmpdet.VoucherNo = InstallmentDetRefNo;
                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                tmpdet.OrderDetDate = DateTime.Now;
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }
        */

        public bool AddSynchronizedItemToOrder
            (Item myItem, int quantity, decimal unitPrice, decimal amount, decimal GSTAmount,
             decimal PreferedDiscount, bool ApplyPromo, string remark, out string status)
        {
            try
            {

                if (ApplyPromo)
                {
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.Remark = remark;
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                tmpdet.UnitPrice = unitPrice;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.GrossSales = amount;
                tmpdet.Amount = amount;
                tmpdet.Quantity = quantity;
                tmpdet.GSTAmount = GSTAmount;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                //CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                /*if (ApplyPromo)
                {
                    promoCtrl.ApplyPromoToOrder();
                }*/
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddServiceItemToOrder
            (ViewItem myItem, int quantity, decimal amount,
             decimal PreferedDiscount, bool ApplyPromo, string remark, out string status)
        {
            try
            {
                if (!myItem.IsServiceItem.HasValue || !myItem.IsServiceItem.Value)
                {
                    status = "This item is not a service item";
                    return false;
                }

                if (ApplyPromo)
                {
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.Remark = remark;
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                tmpdet.UnitPrice = amount;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = amount;
                tmpdet.GrossSales = amount;
                tmpdet.Quantity = quantity;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                tmpdet.OrderDetDate = DateTime.Now;
                if (MembershipApplied() && GetMemberInfo().MembershipNo != "")
                    ApplyCustomerPrice(GetMemberInfo().MembershipNo, ref tmpdet);
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    promoCtrl.ApplyPromoToOrder();
                }
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddVoucherItemToOrder
            (decimal amount, decimal PreferedDiscount, Voucher myVoucher,
            bool ApplyPromo, bool isSellVoucher, out string status)
        {
            try
            {
                if (myVoucher.ExpiryDate.HasValue && myVoucher.ExpiryDate.Value < DateTime.Now)
                {
                    status = "Voucher expired on " + myVoucher.ExpiryDate.Value.ToString("dd MMM yyyy") + " !";
                    return false;
                }
                Item myItem = new Item(Item.Columns.ItemNo, VOUCHER_BARCODE);

                if (myItem.IsNew)
                {
                    status = "Voucher Item is not properly setup. Contact your system Administrator";
                    return false;
                }

                if (ApplyPromo)
                {
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == VOUCHER_BARCODE &&
                        myOrderDet[i].VoucherNo == myVoucher.VoucherNo
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
                            && (!((bool)myOrderDet[i].IsFreeOfCharge))
                            && (!((bool)myOrderDet[i].IsPromo))
                            && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        //item already existed                        
                        status = "You can only use one voucher once.";


                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }

                        return false;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.IsPreOrder = false;
                if (isSellVoucher)
                {
                    tmpdet.Quantity = 1;
                }
                else
                {
                    tmpdet.Quantity = -1;
                }
                tmpdet.UnitPrice = amount;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = amount;
                tmpdet.GrossSales = amount;
                tmpdet.VoucherNo = myVoucher.VoucherNo;
                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.OrderDetDate = DateTime.Now;
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddExchangeItemToOrder
            (Item myItem, OrderDet ExchangedDet, out string status)
        {

            try
            {
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                        && myOrderDet[i].IsExchange
                        && myOrderDet[i].ExchangeDetRefNo == ExchangedDet.OrderDetID
                        && (!((bool)myOrderDet[i].IsFreeOfCharge))
                        && (!((bool)myOrderDet[i].IsPromo))
                        && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        if ((bool)myOrderDet[i].IsVoided)
                        {
                            status = "Item is voided. Unvoid the item first before adding a new quantity.";
                            return false;
                        }
                        //item already existed                                                
                        if (Math.Abs(myOrderDet[i].Quantity.GetValueOrDefault(0)) + 1 > ExchangedDet.Quantity.GetValueOrDefault(0))
                        {
                            status = "Quantity exceeded the receipt quantity of the exchange item";
                            return false;
                        }
                        myOrderDet[i].Quantity -= 1;
                        myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        myOrderDet[i].OrderDetDate = DateTime.Now;
                        OrderDet det = myOrderDet[i];
                        CalculateLineAmount(ref det);

                        //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);
                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsExchange = true;
                tmpdet.ExchangeDetRefNo = ExchangedDet.OrderDetID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = -1 * ExchangedDet.Quantity;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.OriginalRetailPrice = ExchangedDet.OriginalRetailPrice;
                tmpdet.UnitPrice = ExchangedDet.Amount / ExchangedDet.Quantity.GetValueOrDefault(0);
                tmpdet.GrossSales = ExchangedDet.Amount;
                tmpdet.Discount = 0;
                tmpdet.OrderDetDate = DateTime.Now;

                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);                
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddLeftOverItemToOrder
            (Item myItem, decimal leftOverQty, decimal PreferedDiscount,
            bool ApplyPromo, DateTime OrderDetDate, out string status)
        {

            try
            {
                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                /*Check the stock balance whether it is ok
                int balance = InventoryController.GetStockBalanceQtyByItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, out status);
                if (status != "")
                {
                    Logger.writeLog(status);
                    return false;
                }
                //Tell Cashier it is no more stock, as warning
                if (balance <= 0)
                {
                    status = "Stock quantity in system for item:" + myItem.ItemName + " shown as zero. Please inform your supervisor.";
                    Logger.writeLog(status);
                }*/
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                        && !myOrderDet[i].IsExchange
                        && !((bool)myOrderDet[i].IsVoided)
                        && (!((bool)myOrderDet[i].IsFreeOfCharge))
                        && (!((bool)myOrderDet[i].IsPromo))
                        && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        )
                    {
                        //item already existed
                        myOrderDet[i].Quantity += leftOverQty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                                           && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        myOrderDet[i].OrderDetDate = DateTime.Now;
                        OrderDet det = myOrderDet[i];
                        CalculateLineAmount(ref det);


                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);
                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = leftOverQty;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.GrossSales = myItem.RetailPrice * leftOverQty;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                    //apply membership discount....
                    /*if (MembershipApplied())
                    {
                        tmpdet.Discount += membershipDiscount; //(decimal)CurrentMember.MembershipGroup.Discount;
                    }*/
                }
                tmpdet.OrderDetDate = OrderDetDate;
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }
                ApplyMembershipDiscount();
                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddLeftOverItemToOrderForPromoLogic
            (Item myItem, decimal leftOverQty, decimal PreferedDiscount,
            bool ApplyPromo, DateTime OrderDetDate, string possibleItemGroupID, out string status)
        {

            try
            {
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                /*Check the stock balance whether it is ok
                int balance = InventoryController.GetStockBalanceQtyByItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, out status);
                if (status != "")
                {
                    Logger.writeLog(status);
                    return false;
                }
                //Tell Cashier it is no more stock, as warning
                if (balance <= 0)
                {
                    status = "Stock quantity in system for item:" + myItem.ItemName + " shown as zero. Please inform your supervisor.";
                    Logger.writeLog(status);
                }*/
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                        && !myOrderDet[i].IsExchange
                        && !((bool)myOrderDet[i].IsVoided)
                        && (!((bool)myOrderDet[i].IsFreeOfCharge))
                        && (!((bool)myOrderDet[i].IsPromo))
                        && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                        && (myOrderDet[i].PriceMode == "NORMAL" || myOrderDet[i].PriceMode == null || myOrderDet[i].PriceMode == "")
                        )
                    {
                        //item already existed
                        myOrderDet[i].Quantity += leftOverQty;
                        if (myOrderDet[i].Item.IsServiceItem.HasValue
                                           && myOrderDet[i].Item.IsServiceItem.HasValue)
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        }
                        else
                        {
                            myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].OriginalRetailPrice;
                        }
                        if (myItem.IsOpenPricePackage)
                        {
                            myOrderDet[i].PointGetAmount = myItem.PointGetAmount;
                        }
                        myOrderDet[i].OrderDetDate = DateTime.Now;

                        OrderDet det = myOrderDet[i];
                        CalculateLineAmount(ref det);


                        if (ApplyPromo)
                        {
                            //promoCtrl.ApplyPromo();
                            promoCtrl.ApplyPromoToOrder();
                        }
                        //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);
                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = leftOverQty;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.GrossSales = myItem.RetailPrice * leftOverQty;
                tmpdet.PriceMode = "NORMAL";
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                    //apply membership discount....
                    /*if (MembershipApplied())
                    {
                        tmpdet.Discount += membershipDiscount; //(decimal)CurrentMember.MembershipGroup.Discount;
                    }*/
                }
                tmpdet.OrderDetDate = OrderDetDate;
                tmpdet.PossibleItemGroupID = possibleItemGroupID;
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool AddFOCItemToOrder(Item myItem, decimal qty, out string status)
        {
            try
            {
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }


                //Check the stock balance whether it is ok
                decimal balance = InventoryController.GetStockBalanceQtyByItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, out status);
                if (status != "")
                {
                    Logger.writeLog(status);
                    return false;
                }
                //Tell Cashier it is no more stock, as warning
                if (balance <= 0)
                {
                    status = "Stock quantity in system for item:" + myItem.ItemName + " shown as zero. Please inform your supervisor.";
                    Logger.writeLog(status);
                }
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                        && !myOrderDet[i].IsExchange
                        && !((bool)myOrderDet[i].IsVoided)
                        && (!((bool)myOrderDet[i].IsFreeOfCharge))
                        && (!((bool)myOrderDet[i].IsPromo))
                        && (!((bool)myOrderDet[i].IsPromoFreeOfCharge)))
                    {
                        //item already existed
                        myOrderDet[i].Quantity += qty;
                        myOrderDet[i].GrossSales = myOrderDet[i].Quantity * myOrderDet[i].UnitPrice;
                        OrderDet det = myOrderDet[i];
                        CalculateLineAmount(ref det);


                        //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);
                        return true;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;

                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = 1;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.IsFreeOfCharge = true;
                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.GrossSales = myItem.RetailPrice;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                CalculateLineAmount(ref tmpdet);
                myOrderDet.Add(tmpdet);

                //PromotionClientController.CheckPROMO(this, out MaxValue, out PromoID, out status);

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        //Add Promo FOC
        public string AddFOCPromoItem(Item myItem, int qty, DateTime OrderDetDate, ViewPromoMasterDetail myPromo, out string status)
        {
            try
            {
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }


                /*Check the stock balance whether it is ok
                int balance = InventoryController.GetStockBalanceQtyByItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, out status);
                if (status != "")
                {
                    Logger.writeLog(status);
                    return "";
                }
                //Tell Cashier it is no more stock, as warning
                if (balance <= 0)
                {
                    status = "Stock quantity in system for item:" + myItem.ItemName + " shown as zero. Please inform your supervisor.";
                    Logger.writeLog(status);
                }*/
                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                      && !myOrderDet[i].IsExchange
                      && !((bool)myOrderDet[i].IsVoided)
                      && (!((bool)myOrderDet[i].IsFreeOfCharge))
                      && (((bool)myOrderDet[i].IsPromo))
                      && (((bool)myOrderDet[i].IsPromoFreeOfCharge))
                      && myOrderDet[i].PromoHdrID == myPromo.PromoCampaignHdrID
                      && myOrderDet[i].PromoDetID == myPromo.PromoCampaignDetID)
                    {
                        //item already existed
                        myOrderDet[i].Quantity += qty;
                        OrderDet det = myOrderDet[i];
                        return det.OrderDetID;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.OrderDetDate = OrderDetDate;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = true;
                tmpdet.IsPromoFreeOfCharge = true;
                tmpdet.PromoHdrID = myPromo.PromoCampaignHdrID;
                tmpdet.PromoDetID = myPromo.PromoCampaignDetID;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.UnitPrice = 0;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo); tmpdet.GrossSales = 0;

                myOrderDet.Add(tmpdet);

                return tmpdet.OrderDetID;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return "";
            }
        }

        //Add Promo FOC
        public string AddNewPromoLineItem
            (Item myItem, decimal qty, double promoDiscount, DateTime OrderDetDate, ViewPromoMasterDetail myPromo, out string status)
        {
            try
            {
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                /*Check the stock balance whether it is ok
                int balance = InventoryController.GetStockBalanceQtyByItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, out status);
                if (status != "")
                {
                    Logger.writeLog(status);
                    return "";
                }
                //Tell Cashier it is no more stock, as warning
                if (balance <= 0)
                {
                    status = "Stock quantity in system for item:" + myItem.ItemName + " shown as zero. Please inform your supervisor.";
                    Logger.writeLog(status);
                }*/
                /*Item already exist?*/
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                      && !myOrderDet[i].IsExchange
                      && !((bool)myOrderDet[i].IsVoided)
                      && (!((bool)myOrderDet[i].IsFreeOfCharge))
                      && (((bool)myOrderDet[i].IsPromo))
                      && (!((bool)myOrderDet[i].IsPromoFreeOfCharge))
                      && myOrderDet[i].PromoDiscount == promoDiscount
                        && myOrderDet[i].PromoHdrID == myPromo.PromoCampaignHdrID
                        && myOrderDet[i].PromoDetID == myPromo.PromoCampaignDetID)
                    {
                        //item already existed
                        myOrderDet[i].Quantity += qty;
                        OrderDet det = myOrderDet[i];
                        return det.OrderDetID;
                    }
                }

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.OrderDetDate = OrderDetDate;
                tmpdet.IsVoided = false;
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.UnitPrice = myItem.RetailPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = true;
                tmpdet.PromoHdrID = myPromo.PromoCampaignHdrID;
                tmpdet.PromoDetID = myPromo.PromoCampaignDetID;
                tmpdet.GrossSales = tmpdet.OriginalRetailPrice * tmpdet.Quantity;
                tmpdet.IsPreOrder = PreOrderController.IsItemDefaultPreOrder(myItem.ItemNo);
                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                myOrderDet.Add(tmpdet);

                return tmpdet.OrderDetID;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return "";
            }
        }

        public bool AddItemToOrderWithPrice(Item myItem, int qty, decimal UnitPrice, decimal PreferedDiscount, bool ApplyPromo, out string status)
        {
            try
            {
                if (myItem.IsNew)
                {
                    status = "Item does not exist";
                    return false;
                }

                if (myItem.Deleted.HasValue && myItem.Deleted.Value)
                {
                    status = "Item has been deleted";
                    return false;
                }

                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                }
                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }
                //Load default Pre Order value                
                bool defaultPreOrder = false;
                Query qr = PreOrderSchedule.CreateQuery();
                Object count = qr.WHERE(PreOrderSchedule.Columns.ItemNo, myItem.ItemNo).AND(PreOrderSchedule.Columns.ValidFrom, Comparison.LessOrEquals, DateTime.Now).AND(PreOrderSchedule.Columns.ValidTo, Comparison.GreaterOrEquals, DateTime.Now).GetCount(PreOrderSchedule.Columns.ItemNo);
                if (count is Int32 && ((Int32)count) > 0) defaultPreOrder = true;

                //default values
                OrderDet tmpdet = new OrderDet();
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + GetDetMaxID(out status);
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                if (myItem.IsCommission.HasValue)
                {
                    tmpdet.GiveCommission = myItem.IsCommission.Value;
                }
                else
                {
                    tmpdet.GiveCommission = false;
                }
                tmpdet.ItemNo = myItem.ItemNo;
                tmpdet.Quantity = qty;
                tmpdet.IsSpecial = false;
                tmpdet.IsPromo = false;
                tmpdet.GrossSales = UnitPrice * qty;
                tmpdet.UnitPrice = UnitPrice;
                tmpdet.OriginalRetailPrice = myItem.RetailPrice;



                //Check Item New Price
                int EventID;
                decimal SpecialPrice = SpecialEventController.
                    FetchSpecialEventPrice(DateTime.Now, myItem.ItemNo, PointOfSaleInfo.PointOfSaleID, out EventID);
                if (SpecialPrice != -1)
                {
                    tmpdet.UnitPrice = SpecialPrice;
                    tmpdet.GrossSales = SpecialPrice;
                    tmpdet.IsEventPrice = true;
                    tmpdet.SpecialEventID = EventID;
                }
                else
                {
                    tmpdet.UnitPrice = UnitPrice;
                    tmpdet.GrossSales = UnitPrice * qty;
                }

                if (tmpdet.Item.IsNonDiscountable)
                {
                    tmpdet.Discount = 0;
                }
                else
                {
                    tmpdet.Discount = PreferedDiscount;
                }
                bool getDisc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
                if (getDisc)
                {
                    string discountname = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
                    if (discountname != "" && discountname != null)
                    {
                        try
                        {
                            if (Convert.ToDouble(myItem.GetColumnValue(discountname)) >= 0)
                            {
                                tmpdet.Discount = Convert.ToDecimal(myItem.GetColumnValue(discountname).ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }

                    }
                }
                tmpdet.DiscountDetail = tmpdet.Discount.ToString("N0") + "%";
                tmpdet.OrderDetDate = DateTime.Now;

                if (myItem.IsOpenPricePackage)
                {
                    tmpdet.PointGetAmount = myItem.PointGetAmount;
                }
                CalculateLineAmount(ref tmpdet);

                //Inventory
                if (!tmpdet.Item.IsInInventory ||
                    tmpdet.ItemNo == POSController.ROUNDING_ITEM)
                {
                    if (tmpdet.Item.NonInventoryProduct)
                        tmpdet.InventoryHdrRefNo = "";
                    else
                        tmpdet.InventoryHdrRefNo = "NONINVENTORY";
                }
                else
                {
                    tmpdet.InventoryHdrRefNo = "";
                }

                tmpdet.OrderDetDate = DateTime.Now;
                tmpdet.CreatedOn = DateTime.Now;
                myOrderDet.Add(tmpdet);

                if (ApplyPromo)
                {
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        //Get Item List From Barcode (Item, Alternate Barcode, Promo)
        public bool FindItemByBarcode(string barcodeText, out List<ItemStruct> res)
        {
            //define default result;
            res = new List<ItemStruct>();
            try
            {


                //Ignore blanks....
                if (barcodeText == "")
                {
                    return false;
                }

                ViewItem tItem = null;
                //Find By Barcode
                string itemName = "";

                //1. Find Item By Barcode
                ItemController itemLogic = new ItemController();
                bool IsItemNo;
                tItem = itemLogic.FetchByBarcode(barcodeText, out IsItemNo);
                itemName = "";
                if (tItem != null && tItem.IsLoaded && !tItem.IsNew)
                {
                    ItemStruct iStruct = new ItemStruct();
                    iStruct.itemNo = tItem.ItemNo;
                    iStruct.isSpecialBarcode = false;
                    iStruct.qty = 1;
                    iStruct.unitPrice = tItem.RetailPrice;
                    iStruct.isOpenPrice = tItem.IsServiceItem.GetValueOrDefault(false) && tItem.RetailPrice == 0;
                    res.Add(iStruct);
                    return true;
                }

                //2. Find Item By AlternateBarcode
                AlternateBarcodeCollection altbar = new AlternateBarcodeCollection();
                altbar.Where(AlternateBarcode.Columns.Barcode, barcodeText);
                altbar.OrderByDesc(AlternateBarcode.Columns.CreatedOn);
                altbar.Load();

                if (altbar.Count > 0 && altbar[0].IsLoaded)
                {
                    tItem = new ViewItem(ViewItem.Columns.ItemNo, altbar[0].ItemNo);
                    if (tItem.Deleted.GetValueOrDefault(true))
                        return false;

                    ItemStruct iStruct = new ItemStruct();
                    iStruct.itemNo = tItem.ItemNo;
                    iStruct.isSpecialBarcode = false;
                    iStruct.qty = 1;
                    iStruct.unitPrice = tItem.RetailPrice;
                    iStruct.isOpenPrice = tItem.IsServiceItem.GetValueOrDefault(false) && tItem.RetailPrice == 0;
                    res.Add(iStruct);
                    return true;

                }

                //3. Find Item By Special Barcode 
                bool useSpecialBarcode = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcode), false);
                if (useSpecialBarcode)
                {
                    int result = -1;
                    decimal price = 0;
                    decimal quantity = 0;
                    string status = "";
                    string recordedDigit = "";
                    result = SpecialBarcodeController.ScanBarcode(barcodeText, out tItem, out price, out quantity, out recordedDigit, out status);

                    if (result == -1)
                    {
                        //iStruct.isSpecialBarcode = false;
                    }
                    else if (result == 0)
                    {
                        ItemStruct iStruct = new ItemStruct();
                        iStruct.itemNo = tItem.ItemNo;
                        iStruct.isSpecialBarcode = true;
                        iStruct.qty = quantity;
                        iStruct.unitPrice = price;
                        iStruct.isOpenPrice = false;
                        res.Add(iStruct);

                        return true;
                        //Special Barcode item Found. return.

                    }
                }

                //4. Find Item By Promo
                PromoCampaignHdrCollection promoCol = new PromoCampaignHdrCollection();
                promoCol.Where(PromoCampaignHdr.Columns.Barcode, barcodeText.Trim());
                promoCol.Where(PromoCampaignHdr.Columns.Enabled, true);
                promoCol.Where(PromoCampaignHdr.Columns.Deleted, false);
                promoCol.Load();

                for (int z = 0; z < promoCol.Count; z++)
                {
                    PromoCampaignHdr it = promoCol[z];

                    if (it.IsLoaded && !it.IsNew && it.Deleted.HasValue && !it.Deleted.Value)
                    {
                        if (DateTime.Now <= it.DateFrom || DateTime.Now >= it.DateTo)
                        {
                            continue;
                        }

                        //Load Check Location
                        PromoOutletCollection po = new PromoOutletCollection();
                        po.Where(PromoOutlet.Columns.PromoCampaignHdrID, it.PromoCampaignHdrID);
                        po.Where(PromoOutlet.Columns.OutletName.Trim(), PointOfSaleInfo.OutletName.Trim());
                        po.Where(PromoOutlet.Columns.Deleted, false);
                        po.Load();
                        if (po.Count <= 0)
                        {
                            continue;
                        }

                        //Load the item group map....

                        PromoCampaignDetCollection itG = new PromoCampaignDetCollection();
                        itG.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, it.PromoCampaignHdrID);
                        itG.Where(PromoCampaignDet.Columns.Deleted, false);
                        itG.Load();

                        ArrayList ItemList = new ArrayList();
                        ArrayList ItemFree = new ArrayList();
                        //loop through and add the item
                        bool haveItemGroupRow = false;
                        for (int i = 0; i < itG.Count; i++)
                        {
                            if (itG[i].ItemNo != null && itG[i].ItemNo != "")
                            {
                                //Found itemno 
                                ItemStruct iStruct = new ItemStruct();
                                iStruct.itemNo = itG[i].ItemNo;
                                iStruct.isSpecialBarcode = false;
                                if (itG[i].UnitQty != null)
                                    iStruct.qty = (decimal)itG[i].UnitQty;
                                iStruct.unitPrice = 0;
                                res.Add(iStruct);
                            }
                            else if (itG[i].ItemGroupID != null && itG[i].ItemGroupID != 0)
                            {
                                haveItemGroupRow = true;
                            }
                        }
                        if (haveItemGroupRow)
                            continue;
                        if (!haveItemGroupRow && res.Count > 0)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //AppendLog(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                return false;
            }
        }

        public bool AddItemListToOrder(List<ItemStruct> res, out string status)
        {
            //Add Item To List
            status = "";
            foreach (ItemStruct iStruct in res)
            {
                ViewItem theItem = new ViewItem(ViewItem.Columns.ItemNo, iStruct.itemNo);
                if (!iStruct.isSpecialBarcode && !iStruct.isOpenPrice)
                {

                    if (!AddItemToOrder(theItem, iStruct.qty.GetIntValue(), 0, true, out status))
                    {
                        return false;
                    }
                    continue;
                }

                if (!AddSpecialItemToOrder(theItem, iStruct.qty, iStruct.unitPrice, 0, true, "", "", out status))
                {
                    return false;
                }
            }
            return true;
        }

        // Check Selling Restrictions
        public bool validateSellingRestriction(List<ItemStruct> res, out string status)
        {
            status = "";
            foreach (ItemStruct iStruct in res)
            {
                Item tItem = new Item(iStruct.itemNo);
                if (!IsRestricted(tItem, out status))
                {
                    return false;
                }
            }
            return true;
        }

        // Check Selling Restrictions
        public int getMaxAgeRestrictions(List<ItemStruct> res)
        {
            //Check Age Restrictions from Category
            int ageRestrictionsMax = 0;
            foreach (ItemStruct iStruct in res)
            {
                Item tItem = new Item(iStruct.itemNo);
                Category cat = new Category(tItem.CategoryName);
                if (cat.AgeRestrictedItems != 0)
                {
                    if (ageRestrictionsMax < cat.AgeRestrictedItems)
                        ageRestrictionsMax = cat.AgeRestrictedItems;
                }
            }

            return ageRestrictionsMax;
        }


        #endregion

        #region "Setters - Change Attributes"

        public bool ChangeOrderLineQuantity(string OrderDetID, decimal newQty, bool ApplyPromo, out string status)
        {
            status = "";
            if (newQty < 1)
            {
                status = "Quantity must be more than zero. To delete, void the line item. Press Backspace if you want to void the line item.";
                return false;
            }


            try
            {
                OrderDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                if (myOrderDetItem.IsExchange)
                {
                    //compare with max qty
                    if ((new OrderDet(myOrderDetItem.ExchangeDetRefNo)).Quantity < Math.Abs(newQty))
                    {
                        status = "Quantity exceeded the original quantity of the exchanged item.";
                        return false;
                    }

                    if (newQty > 0)
                        newQty = -newQty;
                }
                myOrderDetItem.Quantity = newQty;

                CalculateLineAmount(ref myOrderDetItem);

                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeOrderLineQuantityForNegativePromo(string OrderDetID, decimal newQty, bool ApplyPromo, out string status)
        {
            status = "";
            //if (newQty < 1)
            //{
            //    status = "Quantity must be more than zero. To delete, void the line item. Press Backspace if you want to void the line item.";
            //    return false;
            //}


            try
            {
                OrderDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                if (myOrderDetItem.IsExchange)
                {
                    //compare with max qty
                    if ((new OrderDet(myOrderDetItem.ExchangeDetRefNo)).Quantity < Math.Abs(newQty))
                    {
                        status = "Quantity exceeded the original quantity of the exchanged item.";
                        return false;
                    }

                    if (newQty > 0)
                        newQty = -newQty;
                }
                myOrderDetItem.Quantity = newQty;

                CalculateLineAmount(ref myOrderDetItem);

                if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeOrderLineQuantityForPromoLogic(string OrderDetID, decimal newQty, bool ApplyPromo, out string status)
        {
            status = "";
            if (newQty < 1)
            {
                status = "Quantity must be more than zero. To delete, void the line item. Press Backspace if you want to void the line item.";
                return false;
            }


            try
            {
                OrderDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                if (myOrderDetItem.IsExchange)
                {
                    //compare with max qty
                    if ((new OrderDet(myOrderDetItem.ExchangeDetRefNo)).Quantity < Math.Abs(newQty))
                    {
                        status = "Quantity exceeded the original quantity of the exchanged item.";
                        return false;
                    }

                    if (newQty > 0)
                        newQty = -newQty;
                }
                myOrderDetItem.Quantity = newQty;

                CalculateLineAmount(ref myOrderDetItem);

                /*if (ApplyPromo)
                {
                    //promoCtrl.UndoCurrentPromo();
                    promoCtrl.UndoPromoToOrder();
                    //promoCtrl.ApplyPromo();
                    promoCtrl.ApplyPromoToOrder();
                }*/
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeOrderLineDiscount(string OrderDetID, decimal newDiscount, bool AllowChangeSpecial, bool overwriteExisting, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                if (myOrderDet == null) return false;
                if (AllowChangeSpecial
                    || (!AllowChangeSpecial & myOrderDetItem.IsSpecial == false)
                    )
                {
                    if (newDiscount == 0)
                    {
                        myOrderDetItem.Discount = 0;
                        myOrderDetItem.DiscountDetail = "0%";
                    }
                    else
                    {
                        #region *) Get current Discount1 and Discount2
                        string Discount1 = "";
                        string Discount2 = "";

                        if (!string.IsNullOrEmpty(myOrderDetItem.DiscountDetail))
                        {
                            string[] discounts = myOrderDetItem.DiscountDetail.Split('+');
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

                        if (!myOrderDetItem.Item.IsNonDiscountable && !myOrderDetItem.IsExchange)
                        {
                            if (!myOrderDetItem.IsPromo)
                            {
                                /*if (MembershipApplied())
                                {
                                    newDiscount += membershipDiscount;//(decimal)CurrentMember.MembershipGroup.Discount;
                                }*/

                                #region *) Calculate Discount1 + Discount2
                                decimal TotalDiscountedPrice = myOrderDetItem.UnitPrice;
                                decimal discPercent1 = newDiscount;
                                TotalDiscountedPrice = TotalDiscountedPrice * (1 - (discPercent1 / 100));
                                Discount1 = discPercent1.ToString("N0") + "%";
                                myOrderDetItem.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
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

                                myOrderDetItem.Discount = (myOrderDetItem.UnitPrice - TotalDiscountedPrice) * 100 / (myOrderDetItem.UnitPrice == 0 ? 1 : myOrderDetItem.UnitPrice);
                                //myOrderDetItem.Discount = newDiscount;
                                myOrderDetItem.IsSpecial = true;
                            }
                        }
                        else
                        {
                            myOrderDetItem.Discount = 0;
                        }
                    }
                }
                CalculateLineAmount(ref myOrderDetItem);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool ChangeOrderLineUnitPrice(string OrderDetID, decimal newUnitPrice, out string status)
        {
            status = "";
            try
            {
                OrderDet myOrderDetItem;
                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);
                if (myOrderDetItem == null) return true;
                if (myOrderDetItem.IsExchange)
                {
                    status = "You are not allowed to change the price of exchanged item";
                    return false;
                }
                myOrderDetItem.UnitPrice = newUnitPrice;

                CalculateLineAmount(ref myOrderDetItem);

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public bool SetVoidPromoOrderLine(int PromoHdrID, bool value, out string status)
        {
            status = "";
            try
            {
                foreach (OrderDet myOrderDetItem in myOrderDet)
                {
                    if (!myOrderDetItem.IsPromoFreeOfCharge && myOrderDetItem.PromoHdrID.GetValueOrDefault(0) == PromoHdrID)
                    {

                        myOrderDetItem.IsVoided = value;
                        if (!value) //false
                        {
                            //If it is no longer free with 0, try to merge
                            TryMergeOrderLine(myOrderDetItem.OrderDetID);
                            //CalculateLineAmount(ref myOrderDetItem);
                        }
                        else
                        {
                            //check if it is membership
                            if (myOrderDetItem.Item.Barcode
                                == MembershipController.MEMBERSHIP_SIGNUP_BARCODE)
                            {
                                RemoveMemberFromReceipt();
                            }
                        }

                    }
                }

                promoCtrl.UndoPromoToOrder();
                promoCtrl.ApplyPromoToOrder();

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return false;
            }
        }

        public bool SetVoidOrderLine(string OrderDetID, bool value, out string status)
        {
            status = "";
            try
            {

                OrderDet myOrderDetItem;
                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderDetID);

                //add exception report log
                string tmp = myOrderDetItem.Item.ItemName + " x" + myOrderDetItem.Quantity.ToString() + " =" + myOrderDetItem.Amount.ToString();
                AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, PowerPOS.AccessSource.POS, UserInfo.username, "", "Remove Item", tmp);

                if (!myOrderDetItem.IsPromoFreeOfCharge)
                {

                    myOrderDetItem.IsVoided = value;
                    if (!value) //false
                    {
                        //If it is no longer free with 0, try to merge
                        TryMergeOrderLine(OrderDetID);
                        //CalculateLineAmount(ref myOrderDetItem);
                    }
                    else
                    {
                        //check if it is membership
                        if (myOrderDetItem.Item.Barcode
                            == MembershipController.MEMBERSHIP_SIGNUP_BARCODE)
                        {
                            RemoveMemberFromReceipt();
                        }
                    }
                    promoCtrl.UndoPromoToOrder();
                    promoCtrl.ApplyPromoToOrder();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return false;
            }
        }

        public void SetHeaderRemark(string remark)
        {
            myOrderHdr.Remark = remark;
        }

        public void SetQuotationHdrID(string quotationHdrID)
        {
            myOrderHdr.QuotationHdrID = quotationHdrID;
        }

        public bool SetLineRemark(string LineID, string remark, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                {
                    tmp.Remark = remark;
                    return true;
                }
                else
                {
                    status = "Unable to set Order line Item with OrderDetID = " + LineID;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return false;
            }
        }

        public bool SetIsFreeOfCharge(string LineID, bool value, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                {
                    tmp.IsFreeOfCharge = value;
                    if (value)
                    {
                        //tmp.UnitPrice = 0;
                        tmp.IsSpecial = true;
                        tmp.Discount = 100; //hundred percent discount approach
                        CalculateLineAmount(ref tmp);
                    }
                    else
                    {
                        //If it is no longer free with 0, try to merge
                        if (!TryMergeOrderLine(LineID))
                        {
                            tmp.IsSpecial = false;
                            //if cant merge, then set price back again to factory price
                            tmp.UnitPrice = tmp.Item.RetailPrice;
                            tmp.Discount = GetPreferredDiscount();
                            CalculateLineAmount(ref tmp);
                        }
                        promoCtrl.UndoPromoToOrder();
                        promoCtrl.ApplyPromoToOrder();

                    }
                    return true;
                }
                else
                {
                    status = "Unable to set IsSpecial for line Item with OrderDetID = " + LineID;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set IsSpecial for line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return false;
            }
        }

        public bool SetIsSpecial(string LineID, bool value, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                {
                    tmp.IsSpecial = value;
                    if (value == false)
                    {
                        //reset the value back to original            
                        tmp.UnitPrice = tmp.OriginalRetailPrice;
                        tmp.Discount = GetPreferredDiscount();
                        CalculateLineAmount(ref tmp);
                        TryMergeALLOrderLine();
                    }
                    return true;
                }
                else
                {
                    status = "Unable to set IsSpecial for line Item with OrderDetID = " + LineID;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set IsSpecial for line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return false;
            }
        }

        public bool SetPreOrder(string LineID, bool value, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                {
                    tmp.IsPreOrder = value;
                    return true;
                }
                else
                {
                    status = "Unable to set IsPreOrder for line Item with OrderDetID = " + LineID;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to to set IsSpecial for line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return false;
            }
        }

        internal bool TryMergeOrderLine(string OrderLineID)
        {
            bool mergeFound = false;
            OrderDet tmp = (OrderDet)myOrderDet.Find(OrderLineID);
            if (tmp != null)
            {
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == tmp.ItemNo
                        && tmp.OrderDetID != myOrderDet[i].OrderDetID
                        && !tmp.IsExchange
                        && !tmp.IsSpecial
                        && !tmp.IsPromo
                        && !tmp.IsVoided
                        && !tmp.IsFreeOfCharge
                        && !myOrderDet[i].IsPromo
                        && !myOrderDet[i].IsVoided
                        && !myOrderDet[i].IsFreeOfCharge
                        && !myOrderDet[i].IsExchange
                        && !myOrderDet[i].IsSpecial
                        && (!myOrderDet[i].Item.IsServiceItem.HasValue ||
                            !myOrderDet[i].Item.IsServiceItem.Value)
                        && tmp.PriceMode == myOrderDet[i].PriceMode
                        )
                    {

                        if ((myOrderDet[i].IsPreOrder.HasValue == tmp.IsPreOrder.HasValue &&
                            myOrderDet[i].IsPreOrder.Value == tmp.IsPreOrder.Value))
                        {

                            myOrderDet[i].Quantity += tmp.Quantity;
                            OrderDet tst = myOrderDet[i];
                            CalculateLineAmount(ref tst);
                            mergeFound = true;
                            break;
                        }


                    }

                    else
                    {

                        if (myOrderDet[i].ItemNo == tmp.ItemNo
                            && tmp.OrderDetID != myOrderDet[i].OrderDetID
                            && myOrderDet[i].IsSpecial && tmp.IsSpecial
                            && !myOrderDet[i].IsVoided
                            && !myOrderDet[i].IsPromo
                            && !myOrderDet[i].IsFreeOfCharge
                            && !myOrderDet[i].IsExchange
                            && !tmp.IsVoided
                            && !tmp.IsExchange
                            && !tmp.IsPromo
                            && !tmp.IsFreeOfCharge
                            && !String.IsNullOrEmpty(myOrderDet[i].Userfld6) && myOrderDet[i].Userfld6 == tmp.Userfld6
                            && (!myOrderDet[i].Item.IsServiceItem.HasValue ||
                                !myOrderDet[i].Item.IsServiceItem.Value)
                            && tmp.PriceMode == myOrderDet[i].PriceMode)
                        {
                            myOrderDet[i].Quantity += tmp.Quantity;
                            OrderDet tst = myOrderDet[i];
                            CalculateLineAmount(ref tst);
                            mergeFound = true;
                            break;
                        }
                    }
                }
                if (mergeFound)
                    myOrderDet.Remove(tmp);
                return mergeFound;
            }
            return false;

        }

        internal bool TryMergeALLOrderLine()
        {
            int i = myOrderDet.Count - 1;
            while (i >= 0)
            {
                if (myOrderDet[i].ItemNo != POSController.VOUCHER_BARCODE
                    && myOrderDet[i].ItemNo != InstallmentController.INSTALLMENT_ITEM)
                    TryMergeOrderLine(myOrderDet[i].OrderDetID);
                i--;
            }
            return true;
        }

        public void SetHeaderGSTRule(int gstRule)
        {
            myOrderHdr.GSTRule = gstRule;
        }

        #endregion

        #region "Fetch Details"
        public OrderDetCollection FetchUnsavedOrderDet()
        {
            return myOrderDet;
        }

        public string FetchUnsavedOrderDetText()
        {
            string data = "";
            foreach (OrderDet od in myOrderDet)
            {

                data += od.Item.ItemName + " " + od.Quantity.ToString() + " = " + od.Amount.ToString("N2") + "\n";

            }
            string stat = "";
            data += "Total : " + CalculateTotalAmount(out stat).ToString("N2");
            return data;
        }

        public OrderDetCollection FetchOrderDetForReceipt(string OrderHDRID)
        {
            OrderDetCollection det = new OrderDetCollection();
            string sql = "select * from OrderDet where OrderHDRID = '" + OrderHDRID + "' order by Cast(substring(OrderDetID,CHARINDEX('.',OrderDetID) + 1,5) as int) desc";
            det.Load(DataService.GetDataSet(new QueryCommand(sql)).Tables[0]);

            if (det.Count == 0 && myOrderDet.Count > 0)
                return myOrderDet;

            return det;
        }

        public OrderDetCollection FetchOrderDetForReceiptASC(string OrderHDRID)
        {
            OrderDetCollection det = new OrderDetCollection();
            string sql = "select * from OrderDet where OrderHDRID = '" + OrderHDRID + "' order by Cast(substring(OrderDetID,CHARINDEX('.',OrderDetID) + 1,5) as int) ASC";
            det.Load(DataService.GetDataSet(new QueryCommand(sql)).Tables[0]);

            return det;
        }
        public DataTable FetchUnSavedOrderItems(out string status)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Disc($$)");
                dTable.Columns.Add("Price", Type.GetType("System.Decimal"));
                dTable.Columns.Add("DPrice", Type.GetType("System.Decimal"));
                dTable.Columns.Add("GSTAmount", Type.GetType("System.Decimal"));
                dTable.Columns.Add("Amount", Type.GetType("System.Decimal"));
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPreOrder");
                dTable.Columns.Add("IsPromo");
                dTable.Columns.Add("IsNonDiscountable");
                dTable.Columns.Add("IsExchange");
                dTable.Columns.Add("SalesPerson", Type.GetType("System.String"));
                dTable.Columns.Add("SalesPerson2", Type.GetType("System.String"));
                dTable.Columns.Add("DiscountDetail", Type.GetType("System.String"));
                dTable.Columns.Add("OrderDetSequence", typeof(Int32));
                dTable.Columns.Add("PriceLevelName", typeof(string));

                Item myItem;
                decimal qty, unitprice;
                myOrderDet.Sort(OrderDet.Columns.OrderDetID, true);
                bool enableMultiTier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricing), false);
                bool enableWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
                for (int i = myOrderDet.Count - 1; i >= 0; i--)
                {
                    dr = dTable.NewRow();
                    int orderdetsequence = 0;
                    if (!string.IsNullOrEmpty(myOrderDet[i].OrderDetID))
                    {
                        var splitid = myOrderDet[i].OrderDetID.Split('.');
                        orderdetsequence = Convert.ToInt32(splitid[1]);
                    }
                    dr["OrderDetSequence"] = orderdetsequence;

                    myItem = new Item(myOrderDet[i].ItemNo);

                    dr["ItemNo"] = myItem.ItemNo;
                    if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                    {
                        dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                    }
                    else
                    {
                        dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                    }
                    dr["CategoryName"] = myItem.CategoryName;
                    dr["ItemDesc"] = myItem.ItemDesc;
                    qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                    unitprice = myOrderDet[i].UnitPrice;
                    if (enableWeight)
                        dr["Quantity"] = qty.ToString("N2");
                    else
                        dr["Quantity"] = qty.ToString("N0");

                    //dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N2");
                    dr["Price"] = unitprice;

                    dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                    dr["ID"] = myOrderDet[i].OrderDetID;
                    dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                    dr["IsPromo"] = myOrderDet[i].IsPromo;
                    dr["IsExchange"] = myOrderDet[i].IsExchange;
                    dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;

                    if (string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) || myOrderDet[i].DiscountDetail.Equals("0%"))
                    {
                        if (myOrderDet[i].IsPromo)
                        {
                            //if (!myOrderDet[i].ItemIsNonDiscountable)
                            //{
                            //dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N2");
                            //dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                            dr["Amount"] = ((decimal)myOrderDet[i].PromoAmount);
                            dr["Disc(%)"] = ((decimal)myOrderDet[i].PromoDiscount).ToString("N0") + "%";
                            //}
                            if (myOrderDet[i].UsePromoPrice.HasValue &&
                                myOrderDet[i].UsePromoPrice.Value)
                            {
                                //dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N2");
                                //dr["DPrice"] = PromoAmount / myOrderDet[i].PromoUnitPrice.ToString("N2");
                                dr["DPrice"] = ((decimal)myOrderDet[i].PromoAmount / myOrderDet[i].Quantity.GetValueOrDefault(0));
                            }
                            else
                            {
                                //dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N2");
                                dr["DPrice"] = (unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100);
                            }
                            dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                            dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N2");

                            dr["DiscountDetail"] = dr["Disc(%)"];
                        }
                        else
                        {
                            decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                            //dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);
                            dr["DPrice"] = DPrice;

                            //dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N2");
                            dr["Amount"] = myOrderDet[i].Amount;
                            dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                            dr["Disc(%)"] = myOrderDet[i].Discount.ToString("N0") + "%";
                            //dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N2");
                            dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N2");
                            dr["DiscountDetail"] = dr["Disc(%)"];
                        }
                    }
                    else
                    {
                        decimal tmp = 0;
                        decimal.TryParse(myOrderDet[i].DiscountDetail.Replace("%", ""), out tmp);

                        //decimal DPrice = (unitprice - unitprice * tmp / 100);
                        decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);

                        dr["DPrice"] = DPrice;
                        dr["Amount"] = myOrderDet[i].Amount;
                        dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                        dr["Disc(%)"] = myOrderDet[i].DiscountDetail;
                        dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N2");
                        dr["DiscountDetail"] = myOrderDet[i].DiscountDetail;
                    }

                    if (myOrderDet[i].IsPreOrder.HasValue)
                    {
                        dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                    }
                    else
                    {
                        dr["IsPreOrder"] = false.ToString();
                    }
                    if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value; else dr["GSTAmount"] = 0;

                    dr["SalesPerson"] = myOrderDet[i].SalesPerson;
                    dr["SalesPerson2"] = myOrderDet[i].SalesPerson2 ?? "";

                    if (enableMultiTier)
                    {
                        dr["PriceLevelName"] = "Tier";
                        if (!string.IsNullOrEmpty(myOrderDet[i].SpecialDiscount))
                        {
                            SpecialDiscount sp = new SpecialDiscount(myOrderDet[i].SpecialDiscount);
                            if (!sp.IsNew &&
                                (sp.DiscountName == "P1" ||
                                sp.DiscountName == "P2" ||
                                sp.DiscountName == "P3" ||
                                sp.DiscountName == "P4" ||
                                sp.DiscountName == "P5"))
                            {
                                dr["PriceLevelName"] = sp.ShowLabel ? sp.DiscountLabel : sp.DiscountName;
                            }
                        }
                    }
                    else
                    {
                        dr["PriceLevelName"] = "Disc";
                    }

                    dTable.Rows.Add(dr);

                }
                DataView dv = dTable.DefaultView;
                dv.Sort = "OrderDetSequence desc";
                return dv.ToTable();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return null;
            }
        }


        public string getDisplayItemName(OrderDetCollection odCol, int promoHdrID)
        {
            string res = "";
            PromoCampaignHdr pHdr = new PromoCampaignHdr("PromoCampaignHdrID", promoHdrID);
            res = pHdr.PromoCampaignName;
            foreach (OrderDet od in odCol)
            {
                if (od.PromoHdrID.GetValueOrDefault(0) == promoHdrID)
                {
                    res = res + "\r    " + od.Item.ItemName + "x" + od.Quantity.GetValueOrDefault(0).ToString("N0");

                }
            }
            return res;
        }

        public DataTable FetchUnSavedOrderItemsForGrid(out string status)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Disc($$)");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("DPrice");
                dTable.Columns.Add("GSTAmount");
                dTable.Columns.Add("Amount");
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPreOrder");
                dTable.Columns.Add("IsPromo");
                dTable.Columns.Add("IsNonDiscountable");
                dTable.Columns.Add("IsExchange");
                dTable.Columns.Add("SalesPerson", Type.GetType("System.String"));
                dTable.Columns.Add("SalesPerson2", Type.GetType("System.String"));
                dTable.Columns.Add("DiscountDetail", Type.GetType("System.String"));
                dTable.Columns.Add("OrderDetSequence", typeof(Int32));
                dTable.Columns.Add("PriceLevelName", typeof(string));
                dTable.Columns.Add("OrderDetDate", typeof(DateTime));

                Item myItem;
                decimal qty, unitprice;
                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, false);
                bool enableMultiTier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricing), false);
                bool enableWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
                bool enableGrouping = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.EnableGrouping), false);


                for (int i = myOrderDet.Count - 1; i >= 0; i--)
                {
                    if (enableGrouping)
                    {
                        if (myOrderDet[i].IsPromo)
                        {
                            //validation if promo already exist
                            bool isFound = false;
                            foreach (DataRow drTable in dTable.Rows)
                            {
                                if (myOrderDet[i].PromoHdrID.ToString() == drTable["ItemNo"].ToString())
                                    isFound = true;
                            }
                            if (!isFound)
                            {
                                dr = dTable.NewRow();
                                int orderdetsequence = 0;
                                if (!string.IsNullOrEmpty(myOrderDet[i].OrderDetID))
                                {
                                    var splitid = myOrderDet[i].OrderDetID.Split('.');
                                    orderdetsequence = Convert.ToInt32(splitid[1]);
                                }
                                dr["OrderDetSequence"] = orderdetsequence;
                                dr["ItemNo"] = myOrderDet[i].PromoHdrID.ToString();
                                dr["ItemName"] = getDisplayPromoName(myOrderDet, myOrderDet[i].PromoHdrID.GetValueOrDefault(0), true);
                                dr["CategoryName"] = "Promo";
                                dr["ItemDesc"] = "";
                                decimal totalAmount = 0;
                                decimal tmpQuantity = getPromoQuantity(myOrderDet, myOrderDet[i].PromoHdrID.GetValueOrDefault(0), out totalAmount);
                                dr["Quantity"] = tmpQuantity.ToString("N0");
                                dr["Amount"] = totalAmount.ToString("N2");
                                dr["Price"] = (totalAmount / tmpQuantity).ToString("N2");

                                dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                                dr["ID"] = myOrderDet[i].OrderDetID;
                                dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                                dr["IsPromo"] = myOrderDet[i].IsPromo;
                                dr["IsExchange"] = myOrderDet[i].IsExchange;
                                dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;

                                dr["Disc(%)"] = "0" + "%";
                                dr["DPrice"] = (totalAmount / tmpQuantity).ToString("N2");

                                dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                                dr["Disc($$)"] = "0";

                                dr["DiscountDetail"] = "0%";
                                if (myOrderDet[i].IsPreOrder.HasValue)
                                {
                                    dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                                }
                                else
                                {
                                    dr["IsPreOrder"] = false.ToString();
                                }
                                if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N2"); else dr["GSTAmount"] = "0.00";

                                dr["SalesPerson"] = myOrderDet[i].SalesPerson;
                                dr["SalesPerson2"] = myOrderDet[i].SalesPerson2 ?? "";
                                if (enableMultiTier)
                                {
                                    dr["PriceLevelName"] = "Tier";
                                    if (!string.IsNullOrEmpty(myOrderDet[i].SpecialDiscount))
                                    {
                                        SpecialDiscount sp = new SpecialDiscount(myOrderDet[i].SpecialDiscount);
                                        if (!sp.IsNew &&
                                            (sp.DiscountName == "P1" ||
                                            sp.DiscountName == "P2" ||
                                            sp.DiscountName == "P3" ||
                                            sp.DiscountName == "P4" ||
                                            sp.DiscountName == "P5"))
                                        {
                                            dr["PriceLevelName"] = sp.ShowLabel ? sp.DiscountLabel : sp.DiscountName;
                                        }
                                    }
                                }
                                else
                                {
                                    dr["PriceLevelName"] = "Disc";
                                }
                                dr["OrderDetDate"] = myOrderDet[i].OrderDetDate;
                                dTable.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dr = dTable.NewRow();
                            int orderdetsequence = 0;
                            if (!string.IsNullOrEmpty(myOrderDet[i].OrderDetID))
                            {
                                var splitid = myOrderDet[i].OrderDetID.Split('.');
                                orderdetsequence = Convert.ToInt32(splitid[1]);
                            }
                            dr["OrderDetSequence"] = orderdetsequence;

                            myItem = new Item(myOrderDet[i].ItemNo);

                            dr["ItemNo"] = myItem.ItemNo;
                            if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                            {
                                dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                            }
                            else
                            {
                                dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowPossiblePromo), true) && !myOrderDet[i].IsPromo)
                                {
                                    string promo = getDisplayPossiblePromo(myOrderDet[i]);
                                    if (!string.IsNullOrEmpty(promo))
                                    {
                                        dr["ItemName"] += promo;
                                    }
                                }
                            }
                            dr["CategoryName"] = myItem.CategoryName;
                            dr["ItemDesc"] = myItem.ItemDesc;
                            qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                            unitprice = myOrderDet[i].UnitPrice;
                            if (enableWeight)
                                dr["Quantity"] = qty.ToString("N3");
                            else
                                dr["Quantity"] = qty.ToString("N0");

                            //dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N2");
                            dr["Price"] = unitprice.ToString("N2");

                            dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                            dr["ID"] = myOrderDet[i].OrderDetID;
                            dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                            dr["IsPromo"] = myOrderDet[i].IsPromo;
                            dr["IsExchange"] = myOrderDet[i].IsExchange;
                            dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;
                            if (myOrderDet[i].IsPromo)
                            {

                                //if (!myOrderDet[i].ItemIsNonDiscountable)
                                //{
                                //dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N2");
                                //dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                                dr["Amount"] = ((decimal)myOrderDet[i].PromoAmount).ToString("N");
                                dr["Disc(%)"] = ((decimal)myOrderDet[i].PromoDiscount).ToString("N0") + "%";
                                //}
                                if (myOrderDet[i].UsePromoPrice.HasValue &&
                                    myOrderDet[i].UsePromoPrice.Value)
                                {
                                    //dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N2");
                                    //dr["DPrice"] = PromoAmount / myOrderDet[i].PromoUnitPrice.ToString("N2");
                                    dr["DPrice"] = ((decimal)myOrderDet[i].PromoAmount / myOrderDet[i].Quantity.GetValueOrDefault(0)).ToString("N");
                                }
                                else
                                {
                                    //dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N2");
                                    //dr["DPrice"] = (unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100).ToString("N");
                                    if (myOrderDet[i].Discount2Percent > 0 || myOrderDet[i].Discount2Dollar > 0 || myOrderDet[i].FinalPrice > 0)
                                        dr["DPrice"] = ((decimal)myOrderDet[i].PromoAmount / myOrderDet[i].Quantity.GetValueOrDefault(0)).ToString("N");
                                    else
                                        dr["DPrice"] = (unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100).ToString("N");
                                }
                                dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                                dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N");

                                dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? dr["Disc(%)"] : myOrderDet[i].DiscountDetail;
                            }
                            else
                            {
                                decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                                //dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);
                                dr["DPrice"] = DPrice.ToString("N");

                                //dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N2");
                                dr["Amount"] = myOrderDet[i].Amount.ToString("N");
                                dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                                dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                                //dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N2");
                                dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N");
                                dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? dr["Disc(%)"] : myOrderDet[i].DiscountDetail;
                            }
                            if (myOrderDet[i].IsPreOrder.HasValue)
                            {
                                dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                            }
                            else
                            {
                                dr["IsPreOrder"] = false.ToString();
                            }
                            if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N"); else dr["GSTAmount"] = "0";

                            dr["SalesPerson"] = myOrderDet[i].SalesPerson;
                            dr["SalesPerson2"] = myOrderDet[i].SalesPerson2 ?? "";

                            if (enableMultiTier)
                            {
                                dr["PriceLevelName"] = "Tier";
                                if (!string.IsNullOrEmpty(myOrderDet[i].SpecialDiscount))
                                {
                                    SpecialDiscount sp = new SpecialDiscount(myOrderDet[i].SpecialDiscount);
                                    if (!sp.IsNew &&
                                        (sp.DiscountName == "P1" ||
                                        sp.DiscountName == "P2" ||
                                        sp.DiscountName == "P3" ||
                                        sp.DiscountName == "P4" ||
                                        sp.DiscountName == "P5"))
                                    {
                                        dr["PriceLevelName"] = sp.ShowLabel ? sp.DiscountLabel : sp.DiscountName;
                                    }
                                }
                            }
                            else
                            {
                                dr["PriceLevelName"] = "Disc";
                            }
                            dr["OrderDetDate"] = myOrderDet[i].OrderDetDate;

                            dTable.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        dr = dTable.NewRow();
                        int orderdetsequence = 0;
                        if (!string.IsNullOrEmpty(myOrderDet[i].OrderDetID))
                        {
                            var splitid = myOrderDet[i].OrderDetID.Split('.');
                            orderdetsequence = Convert.ToInt32(splitid[1]);
                        }
                        dr["OrderDetSequence"] = orderdetsequence;

                        myItem = new Item(myOrderDet[i].ItemNo);

                        dr["ItemNo"] = myItem.ItemNo;
                        if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                        {
                            dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                        }
                        else
                        {
                            dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowPossiblePromo), true) && !myOrderDet[i].IsPromo)
                            {
                                string promo = getDisplayPossiblePromo(myOrderDet[i]);
                                if (!string.IsNullOrEmpty(promo))
                                {
                                    dr["ItemName"] += promo;
                                }
                            }
                        }
                        dr["CategoryName"] = myItem.CategoryName;
                        dr["ItemDesc"] = myItem.ItemDesc;
                        qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                        unitprice = myOrderDet[i].UnitPrice;
                        if (enableWeight)
                            dr["Quantity"] = qty.ToString("N3");
                        else
                            dr["Quantity"] = qty.ToString("N0");

                        //dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N2");
                        dr["Price"] = unitprice.ToString("N");

                        dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                        dr["ID"] = myOrderDet[i].OrderDetID;
                        dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                        dr["IsPromo"] = myOrderDet[i].IsPromo;
                        dr["IsExchange"] = myOrderDet[i].IsExchange;
                        dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;
                        if (myOrderDet[i].IsPromo)
                        {

                            //if (!myOrderDet[i].ItemIsNonDiscountable)
                            //{
                            //dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N2");
                            //dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                            dr["Amount"] = ((decimal)myOrderDet[i].PromoAmount).ToString("N");
                            dr["Disc(%)"] = ((decimal)myOrderDet[i].PromoDiscount).ToString("N0") + "%";
                            //}
                            if (myOrderDet[i].UsePromoPrice.HasValue &&
                                myOrderDet[i].UsePromoPrice.Value)
                            {
                                //dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N2");
                                //dr["DPrice"] = PromoAmount / myOrderDet[i].PromoUnitPrice.ToString("N2");
                                dr["DPrice"] = ((decimal)myOrderDet[i].PromoAmount / myOrderDet[i].Quantity.GetValueOrDefault(0)).ToString("N");
                            }
                            else
                            {
                                //dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N2");
                                //dr["DPrice"] = (unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100).ToString("N");
                                if (myOrderDet[i].Discount2Percent > 0 || myOrderDet[i].Discount2Dollar > 0 || myOrderDet[i].FinalPrice > 0)
                                    dr["DPrice"] = ((decimal)myOrderDet[i].PromoAmount / myOrderDet[i].Quantity.GetValueOrDefault(0)).ToString("N");
                                else
                                    dr["DPrice"] = (unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100).ToString("N");
                            }
                            dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                            dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N");

                            dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? dr["Disc(%)"] : myOrderDet[i].DiscountDetail;
                        }
                        else
                        {
                            decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                            //dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);
                            dr["DPrice"] = DPrice.ToString("N");

                            //dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N2");
                            dr["Amount"] = myOrderDet[i].Amount.ToString("N");
                            dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                            dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                            //dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N2");
                            dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N");
                            dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? dr["Disc(%)"] : myOrderDet[i].DiscountDetail;
                        }
                        if (myOrderDet[i].IsPreOrder.HasValue)
                        {
                            dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                        }
                        else
                        {
                            dr["IsPreOrder"] = false.ToString();
                        }
                        if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N"); else dr["GSTAmount"] = "0";

                        dr["SalesPerson"] = myOrderDet[i].SalesPerson;
                        dr["SalesPerson2"] = myOrderDet[i].SalesPerson2 ?? "";

                        if (enableMultiTier)
                        {
                            dr["PriceLevelName"] = "Tier";
                            if (!string.IsNullOrEmpty(myOrderDet[i].SpecialDiscount))
                            {
                                SpecialDiscount sp = new SpecialDiscount(myOrderDet[i].SpecialDiscount);
                                if (!sp.IsNew &&
                                    (sp.DiscountName == "P1" ||
                                    sp.DiscountName == "P2" ||
                                    sp.DiscountName == "P3" ||
                                    sp.DiscountName == "P4" ||
                                    sp.DiscountName == "P5"))
                                {
                                    dr["PriceLevelName"] = sp.ShowLabel ? sp.DiscountLabel : sp.DiscountName;
                                }
                            }
                        }
                        else
                        {
                            dr["PriceLevelName"] = "Disc";
                        }
                        dr["OrderDetDate"] = myOrderDet[i].OrderDetDate;

                        dTable.Rows.Add(dr);
                    }



                }
                DataView dv = dTable.DefaultView;
                dv.Sort = "OrderDetDate desc";
                return dv.ToTable();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return null;
            }
        }

        public DataTable FetchUnSavedOrderItemsforSecondScreen(out string status)
        {
            status = "";
            try
            {
                bool isUseWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
                bool enableGrouping = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.EnableGrouping), false);
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Disc($$)");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("DPrice");
                dTable.Columns.Add("GSTAmount");
                dTable.Columns.Add("Amount");
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPreOrder");
                dTable.Columns.Add("IsPromo");
                dTable.Columns.Add("IsNonDiscountable");
                dTable.Columns.Add("IsExchange");
                dTable.Columns.Add("SalesPerson", Type.GetType("System.String"));

                Item myItem;
                decimal qty, unitprice;

                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, true);

                for (int i = myOrderDet.Count - 1; i >= 0; i--)
                {
                    if (enableGrouping)
                    {
                        if (!myOrderDet[i].IsVoided)
                        {
                            if (myOrderDet[i].IsPromo)
                            {
                                //validation if promo already exist
                                bool isFound = false;
                                foreach (DataRow drTable in dTable.Rows)
                                {
                                    if (myOrderDet[i].PromoHdrID.ToString() == drTable["ItemNo"].ToString())
                                        isFound = true;
                                }
                                if (!isFound)
                                {
                                    dr = dTable.NewRow();
                                    int orderdetsequence = 0;
                                    if (!string.IsNullOrEmpty(myOrderDet[i].OrderDetID))
                                    {
                                        var splitid = myOrderDet[i].OrderDetID.Split('.');
                                        orderdetsequence = Convert.ToInt32(splitid[1]);
                                    }
                                    dr["ItemNo"] = myOrderDet[i].PromoHdrID.ToString();
                                    dr["ItemName"] = getDisplayPromoName(myOrderDet, myOrderDet[i].PromoHdrID.GetValueOrDefault(0), AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.ShowPromoNameNoDetails), false));
                                    dr["CategoryName"] = "Promo";
                                    dr["ItemDesc"] = "";
                                    decimal totalAmount = 0;
                                    decimal tmpQuantity = getPromoQuantity(myOrderDet, myOrderDet[i].PromoHdrID.GetValueOrDefault(0), out totalAmount);
                                    dr["Quantity"] = tmpQuantity.ToString("N0");
                                    dr["Amount"] = totalAmount.ToString("N");
                                    dr["Price"] = (totalAmount / tmpQuantity).ToString("N");

                                    dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                                    dr["ID"] = myOrderDet[i].OrderDetID;
                                    dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                                    dr["IsPromo"] = myOrderDet[i].IsPromo;
                                    dr["IsExchange"] = myOrderDet[i].IsExchange;
                                    dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;

                                    dr["Disc(%)"] = "0" + "%";
                                    dr["DPrice"] = (totalAmount / tmpQuantity).ToString("N");

                                    dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                                    dr["Disc($$)"] = "0";

                                    if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N"); else dr["GSTAmount"] = "0";

                                    dr["SalesPerson"] = myOrderDet[i].SalesPerson;
                                    //dr["SalesPerson2"] = myOrderDet[i].SalesPerson2 ?? "";
                                    //dr["PriceLevelName"] = "Disc";
                                    //dr["OrderDetDate"] = myOrderDet[i].OrderDetDate;
                                    dTable.Rows.Add(dr);
                                }


                            }
                            else
                            {
                                dr = dTable.NewRow();
                                myItem = new Item(myOrderDet[i].ItemNo);
                                dr["ItemNo"] = myItem.ItemNo;
                                if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                                {
                                    dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                                }
                                else
                                {
                                    dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                                }
                                dr["CategoryName"] = myItem.CategoryName;
                                dr["ItemDesc"] = myItem.ItemDesc;
                                qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                                unitprice = myOrderDet[i].UnitPrice;
                                if (isUseWeight)
                                    dr["Quantity"] = qty.ToString("N2");
                                else
                                    dr["Quantity"] = qty.ToString("N0");

                                dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N");

                                dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                                dr["ID"] = myOrderDet[i].OrderDetID;
                                dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                                dr["IsPromo"] = myOrderDet[i].IsPromo;
                                dr["IsExchange"] = myOrderDet[i].IsExchange;
                                dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;
                                if (myOrderDet[i].IsPromo)
                                {

                                    //if (!myOrderDet[i].ItemIsNonDiscountable)
                                    //{
                                    dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N");
                                    dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                                    //}
                                    if (myOrderDet[i].UsePromoPrice.HasValue &&
                                        myOrderDet[i].UsePromoPrice.Value)
                                    {
                                        dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N");
                                    }
                                    else
                                    {
                                        dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N2");
                                    }
                                    dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                                    dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N");
                                }
                                else
                                {
                                    decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                                    dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);

                                    dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N");
                                    dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                                    dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                                    dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N");
                                }
                                if (myOrderDet[i].IsPreOrder.HasValue)
                                {
                                    dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                                }
                                else
                                {
                                    dr["IsPreOrder"] = false.ToString();
                                }
                                if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N"); else dr["GSTAmount"] = "0";

                                dr["SalesPerson"] = myOrderDet[i].Userfld1;

                                dTable.Rows.Add(dr);
                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (!myOrderDet[i].IsVoided)
                        {
                            dr = dTable.NewRow();
                            myItem = new Item(myOrderDet[i].ItemNo);
                            dr["ItemNo"] = myItem.ItemNo;
                            if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                            {
                                dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                            }
                            else
                            {
                                dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                            }
                            dr["CategoryName"] = myItem.CategoryName;
                            dr["ItemDesc"] = myItem.ItemDesc;
                            qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                            unitprice = myOrderDet[i].UnitPrice;
                            if (isUseWeight)
                                dr["Quantity"] = qty.ToString("N2");
                            else
                                dr["Quantity"] = qty.ToString("N0");

                            dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N");

                            dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                            dr["ID"] = myOrderDet[i].OrderDetID;
                            dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                            dr["IsPromo"] = myOrderDet[i].IsPromo;
                            dr["IsExchange"] = myOrderDet[i].IsExchange;
                            dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;
                            if (myOrderDet[i].IsPromo)
                            {

                                //if (!myOrderDet[i].ItemIsNonDiscountable)
                                //{
                                dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N");
                                dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                                //}
                                if (myOrderDet[i].UsePromoPrice.HasValue &&
                                    myOrderDet[i].UsePromoPrice.Value)
                                {
                                    dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N");
                                }
                                else
                                {
                                    dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N");
                                }
                                dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                                dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N");
                            }
                            else
                            {
                                decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                                dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);

                                dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N");
                                dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                                dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                                dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N");
                            }
                            if (myOrderDet[i].IsPreOrder.HasValue)
                            {
                                dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                            }
                            else
                            {
                                dr["IsPreOrder"] = false.ToString();
                            }
                            if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N"); else dr["GSTAmount"] = "0";

                            dr["SalesPerson"] = myOrderDet[i].Userfld1;

                            dTable.Rows.Add(dr);
                        }
                    }
                }
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return null;
            }
        }

        public DataTable FetchUnSavedOrderItems(DateTime minimumTime, out string status)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Disc($$)");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("DPrice");
                dTable.Columns.Add("GSTAmount");
                dTable.Columns.Add("Amount");
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPreOrder");
                dTable.Columns.Add("IsPromo");
                dTable.Columns.Add("IsNonDiscountable");
                dTable.Columns.Add("IsExchange");
                dTable.Columns.Add("SalesPerson", Type.GetType("System.String"));

                Item myItem;
                decimal qty, unitprice;


                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, false);
                myOrderDet.Sort(OrderDet.Columns.CreatedOn, false);

                for (int i = myOrderDet.Count - 1; i >= 0; i--)
                {
                    if ((myOrderDet[i].OrderDetDate).Subtract(minimumTime).Ticks > 0)
                    {
                        dr = dTable.NewRow();
                        myItem = new Item(myOrderDet[i].ItemNo);

                        dr["ItemNo"] = myItem.ItemNo;
                        if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                        {
                            dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                        }
                        else
                        {
                            dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                        }
                        dr["CategoryName"] = myItem.CategoryName;
                        dr["ItemDesc"] = myItem.ItemDesc;
                        qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                        unitprice = myOrderDet[i].UnitPrice;
                        dr["Quantity"] = qty.ToString("N0");

                        dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N2");

                        dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                        dr["ID"] = myOrderDet[i].OrderDetID;
                        dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                        dr["IsPromo"] = myOrderDet[i].IsPromo;
                        dr["IsExchange"] = myOrderDet[i].IsExchange;
                        dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;
                        if (myOrderDet[i].IsPromo)
                        {
                            //if (!myOrderDet[i].ItemIsNonDiscountable)
                            //{
                            dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N2");
                            dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                            //}
                            if (myOrderDet[i].UsePromoPrice.HasValue &&
                                myOrderDet[i].UsePromoPrice.Value)
                            {
                                dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N2");
                            }
                            else
                            {
                                dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N2");
                            }
                            dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                            dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N2");
                        }
                        else
                        {
                            decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                            dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);

                            dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N2");
                            dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                            dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                            dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N2");
                        }
                        if (myOrderDet[i].IsPreOrder.HasValue)
                        {
                            dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                        }
                        else
                        {
                            dr["IsPreOrder"] = false.ToString();
                        }
                        if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N2"); else dr["GSTAmount"] = "0.00";

                        dr["SalesPerson"] = myOrderDet[i].Userfld1;

                        dTable.Rows.Add(dr);
                    }
                }
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return null;
            }
        }

        public DataTable FetchUnSavedOrderItemsForPromo(int PromoHdrID, out string status)
        {
            status = "";
            try
            {
                //create and return a datatable.....
                DataTable dTable = new DataTable();
                DataRow dr;

                dTable.Columns.Add("ItemNo");
                dTable.Columns.Add("ItemName");
                dTable.Columns.Add("itemdesc");
                dTable.Columns.Add("CategoryName");
                dTable.Columns.Add("Quantity");
                dTable.Columns.Add("Disc(%)");
                dTable.Columns.Add("Disc($$)");
                dTable.Columns.Add("Price");
                dTable.Columns.Add("DPrice");
                dTable.Columns.Add("GSTAmount");
                dTable.Columns.Add("Amount");
                dTable.Columns.Add("IsVoided");
                dTable.Columns.Add("IsSpecial");
                dTable.Columns.Add("ID");
                dTable.Columns.Add("IsFreeOfCharge");
                dTable.Columns.Add("IsPreOrder");
                dTable.Columns.Add("IsPromo");
                dTable.Columns.Add("IsNonDiscountable");
                dTable.Columns.Add("IsExchange");
                dTable.Columns.Add("SalesPerson", Type.GetType("System.String"));
                dTable.Columns.Add("SalesPerson2", Type.GetType("System.String"));
                dTable.Columns.Add("DiscountDetail", Type.GetType("System.String"));
                dTable.Columns.Add("OrderDetSequence", typeof(Int32));
                dTable.Columns.Add("PriceLevelName", typeof(string));
                dTable.Columns.Add("OrderDetDate", typeof(DateTime));

                Item myItem;
                decimal qty, unitprice;
                myOrderDet.Sort(OrderDet.Columns.OrderDetID, true);
                bool enableMultiTier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricing), false);
                bool enableWeight = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
                for (int i = myOrderDet.Count - 1; i >= 0; i--)
                {
                    if (myOrderDet[i].PromoHdrID.GetValueOrDefault(0) != PromoHdrID)
                        continue;

                    dr = dTable.NewRow();
                    int orderdetsequence = 0;
                    if (!string.IsNullOrEmpty(myOrderDet[i].OrderDetID))
                    {
                        var splitid = myOrderDet[i].OrderDetID.Split('.');
                        orderdetsequence = Convert.ToInt32(splitid[1]);
                    }
                    dr["OrderDetSequence"] = orderdetsequence;

                    myItem = new Item(myOrderDet[i].ItemNo);

                    dr["ItemNo"] = myItem.ItemNo;
                    if (myItem.ItemNo == POSController.VOUCHER_BARCODE || myItem.ItemNo == InstallmentController.INSTALLMENT_ITEM)
                    {
                        dr["ItemName"] = "Ref No:" + myOrderDet[i].VoucherNo;
                    }
                    else
                    {
                        dr["ItemName"] = myItem.ItemName + " " + myOrderDet[i].Remark;
                    }
                    dr["CategoryName"] = myItem.CategoryName;
                    dr["ItemDesc"] = myItem.ItemDesc;
                    qty = myOrderDet[i].Quantity.GetValueOrDefault(0);
                    unitprice = myOrderDet[i].UnitPrice;
                    if (enableWeight)
                        dr["Quantity"] = qty.ToString("N2");
                    else
                        dr["Quantity"] = qty.ToString("N0");

                    //dr["Price"] = CommonUILib.RemoveRoundUp(unitprice).ToString("N2");
                    dr["Price"] = unitprice.ToString("N2");

                    dr["IsVoided"] = myOrderDet[i].IsVoided.ToString();
                    dr["ID"] = myOrderDet[i].OrderDetID;
                    dr["IsSpecial"] = myOrderDet[i].IsSpecial;

                    dr["IsPromo"] = myOrderDet[i].IsPromo;
                    dr["IsExchange"] = myOrderDet[i].IsExchange;
                    dr["IsNonDiscountable"] = myOrderDet[i].Item.IsNonDiscountable;
                    if (myOrderDet[i].IsPromo)
                    {

                        //if (!myOrderDet[i].ItemIsNonDiscountable)
                        //{
                        //dr["Amount"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount)).ToString("N2");
                        //dr["Disc(%)"] = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoDiscount)).ToString("N0") + "%";
                        dr["Amount"] = ((decimal)myOrderDet[i].PromoAmount).ToString("N2");
                        dr["Disc(%)"] = ((decimal)myOrderDet[i].PromoDiscount).ToString("N0") + "%";
                        //}
                        if (myOrderDet[i].UsePromoPrice.HasValue &&
                            myOrderDet[i].UsePromoPrice.Value)
                        {
                            //dr["DPrice"] = CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice).ToString("N2");
                            //dr["DPrice"] = PromoAmount / myOrderDet[i].PromoUnitPrice.ToString("N2");
                            dr["DPrice"] = ((decimal)myOrderDet[i].PromoAmount / myOrderDet[i].Quantity.GetValueOrDefault(0)).ToString("N2");
                        }
                        else
                        {
                            //dr["DPrice"] = CommonUILib.RemoveRoundUp((unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100)).ToString("N2");
                            dr["DPrice"] = (unitprice - unitprice * (decimal)myOrderDet[i].PromoDiscount / 100).ToString("N2");
                        }
                        dr["IsFreeOfCharge"] = myOrderDet[i].IsPromoFreeOfCharge;
                        dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N2");

                        dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? dr["Disc(%)"] : myOrderDet[i].DiscountDetail;
                    }
                    else
                    {
                        decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                        //dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);
                        dr["DPrice"] = DPrice.ToString("N2");

                        //dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N2");
                        dr["Amount"] = myOrderDet[i].Amount.ToString("N2");
                        dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                        dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                        //dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N2");
                        dr["Disc($$)"] = CalculateDiscountInDollar(i).ToString("N2");
                        dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? dr["Disc(%)"] : myOrderDet[i].DiscountDetail;
                    }
                    if (myOrderDet[i].IsPreOrder.HasValue)
                    {
                        dr["IsPreOrder"] = myOrderDet[i].IsPreOrder.Value;
                    }
                    else
                    {
                        dr["IsPreOrder"] = false.ToString();
                    }
                    if (myOrderDet[i].GSTAmount.HasValue) dr["GSTAmount"] = myOrderDet[i].GSTAmount.Value.ToString("N2"); else dr["GSTAmount"] = "0.00";

                    dr["SalesPerson"] = myOrderDet[i].SalesPerson;
                    dr["SalesPerson2"] = myOrderDet[i].SalesPerson2 ?? "";

                    if (enableMultiTier)
                    {
                        dr["PriceLevelName"] = "Tier";
                        if (!string.IsNullOrEmpty(myOrderDet[i].SpecialDiscount))
                        {
                            SpecialDiscount sp = new SpecialDiscount(myOrderDet[i].SpecialDiscount);
                            if (!sp.IsNew &&
                                (sp.DiscountName == "P1" ||
                                sp.DiscountName == "P2" ||
                                sp.DiscountName == "P3" ||
                                sp.DiscountName == "P4" ||
                                sp.DiscountName == "P5"))
                            {
                                dr["PriceLevelName"] = sp.ShowLabel ? sp.DiscountLabel : sp.DiscountName;
                            }
                        }
                    }
                    else
                    {
                        dr["PriceLevelName"] = "Disc";
                    }
                    dr["OrderDetDate"] = myOrderDet[i].OrderDetDate;

                    dTable.Rows.Add(dr);
                }
                DataView dv = dTable.DefaultView;
                dv.Sort = "OrderDetDate desc";
                return dv.ToTable();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return null;
            }
        }

        public DataTable FetchCostOfGoods()
        {
            if (myOrderDet == null || myOrderDet.Count == 0)
                return null;

            ArrayList ar = new ArrayList();
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].InventoryHdrRefNo != null && myOrderDet[i].InventoryHdrRefNo != "NON INVENTORY")
                {
                    ar.Add(myOrderDet[i].InventoryHdrRefNo);
                }
            }
            if (ar.Count == 0) return null;
            Query qr = ViewInventoryActivity.CreateQuery();
            qr.QueryType = QueryType.Select;
            DataSet ds = qr.IN(ViewInventoryActivity.Columns.InventoryHdrRefNo, ar).ExecuteDataSet();
            return ds.Tables[0];
            /*
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].InventoryHdrRefNo != null)
                {
                    v.Where(ViewInventoryActivity.Columns.InventoryHdrRefNo, myOrderDet[i].InventoryHdrRefNo);
                    v.Load();
                }
            }
            return v.ToDataTable();*/
        }

        #endregion

        #region Promo Display

        public string getDisplayPromoName(OrderDetCollection tmpMyOrderDet, int PromoHdrID, bool showDetails)
        {
            string result = "";
            PromoCampaignHdr hdr = new PromoCampaignHdr(PromoCampaignHdr.Columns.PromoCampaignHdrID, PromoHdrID);
            if (hdr != null && hdr.PromoCampaignName != "")
            {
                result = hdr.PromoCampaignName;
                if (showDetails)
                {
                    foreach (OrderDet tmpOd in tmpMyOrderDet)
                    {
                        if (tmpOd.IsPromo && tmpOd.PromoHdrID.GetValueOrDefault(0) == PromoHdrID)
                        {
                            result = result + "\r    " + tmpOd.Quantity.GetValueOrDefault(0).ToString("N0") + "x " + tmpOd.Item.ItemName;
                        }
                    }
                }
            }
            return result;
        }

        public decimal getPromoQuantity(OrderDetCollection tmpMyOrderDet, int PromoHdrID, out decimal totalPromoAmount)
        {
            PromoCampaignDetCollection hdr = new PromoCampaignDetCollection();
            hdr.Where(PromoCampaignHdr.Columns.PromoCampaignHdrID, PromoHdrID);
            hdr.Where(PromoCampaignDet.Columns.Deleted, false);
            hdr.Load();
            decimal maxQty = 10000;
            totalPromoAmount = 0;
            foreach (PromoCampaignDet pd in hdr)
            {
                decimal totalPerPromoDetail = 0;
                foreach (OrderDet tmpOd in tmpMyOrderDet)
                {
                    if (tmpOd.IsPromo && tmpOd.PromoDetID.GetValueOrDefault(0) == pd.PromoCampaignDetID)
                    {
                        totalPromoAmount += tmpOd.PromoAmount;
                        if (pd.AnyQty > 0)
                        {
                            totalPerPromoDetail += tmpOd.Quantity.GetValueOrDefault(0) / pd.AnyQty.GetValueOrDefault(1);
                        }
                        else
                        {
                            totalPerPromoDetail += tmpOd.Quantity.GetValueOrDefault(0) / pd.UnitQty.GetValueOrDefault(1);
                        }

                    }
                }
                if (maxQty > totalPerPromoDetail)
                    maxQty = totalPerPromoDetail;

            }
            return maxQty;
        }

        public string getDisplayPossiblePromo(OrderDet tmpMyOrderDet)
        {
            string PossiblePromo = tmpMyOrderDet.PossiblePromoID;
            StringBuilder sb = new StringBuilder();
            if (tmpMyOrderDet.PossiblePromoID != null && tmpMyOrderDet.PossiblePromoID != "")
            {
                sb.AppendLine();
                string[] listPromo = tmpMyOrderDet.PossiblePromoID.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in listPromo)
                {
                    PromoCampaignHdr p = new PromoCampaignHdr(s);
                    if (p != null)
                    {
                        sb.AppendFormat("Promo: {0}", p.PromoCampaignName);
                        sb.AppendLine();
                    }
                }
            }

            return sb.ToString();
        }
        #endregion

        #region "Getters "



        public string GetCashierID()
        {
            return myOrderHdr.CashierID;
        }
        public string GetCashierName()
        {
            UserMst m = new UserMst(myOrderHdr.CashierID);
            if (m.IsLoaded && !m.IsNew)
            {
                return m.DisplayName;
            }
            return "-";
        }

        public string GetSalesPerson()
        {
            SalesCommissionRecordCollection str = myOrderHdr.SalesCommissionRecordRecords();
            if (str != null && str.Count > 0)
                return str[0].UserMst.UserName;
            else
                return "";
        }

        public string GetCustomizedRefNo()
        {
            if (myOrderHdr != null && myOrderHdr.Userfld5 != null && myOrderHdr.Userfld5 != "")
                return myOrderHdr.Userfld5;

            return "";
        }

        public string GetSavedRefNo()
        {
            if (myOrderHdr != null && myOrderHdr.IsLoaded)
                return myOrderHdr.OrderRefNo;

            return "";
        }
        public string GetUnsavedRefNo()
        {
            if (myOrderHdr != null)
                return myOrderHdr.OrderRefNo;

            return "";
        }

        public string GetUnsavedCustomRefNo()
        {
            if (myOrderHdr != null)
                return myOrderHdr.Userfld5;

            return "";
        }

        public DateTime GetOrderDate()
        {
            return myOrderHdr.OrderDate;
        }
        public int GetLineRowCount()
        {
            return myOrderDet.Count;
        }

        public decimal GetSumOfItemQuantity()
        {
            decimal total = 0;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!(bool)myOrderDet[i].IsVoided)
                    total += myOrderDet[i].Quantity.GetValueOrDefault(0);
            }
            return total;
        }

        public int GetNoOfItem()
        {
            if (myOrderDet == null)
                return 0;
            int total = (from o in myOrderDet
                         where o.IsVoided == false
                         select o.Item).Distinct().ToList().Count;
            return total;
        }

        public string GetHeaderRemark()
        {
            return myOrderHdr.Remark;
        }
        public string GetLineRemark(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.Remark;

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
        public decimal GetLinePromoUnitPrice(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.PromoUnitPrice;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return 0;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return -1;
            }

        }

        public decimal GetLineUnitPrice(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.UnitPrice;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return 0;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return -1;
            }

        }
        public DateTime GetLineOrderDetDate(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.OrderDetDate;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return DateTime.Now;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return DateTime.Now;
            }

        }

        public string GetLineInfoOfFirstLine()
        {
            if (myOrderDet != null && myOrderDet.Count > 0)
                return myOrderDet[0].LineInfo;
            else
                return "";
        }

        public bool IsVoided(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.IsVoided;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return false;
            }

        }
        public decimal GetLineQuantity(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.Quantity.GetValueOrDefault(0);

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return -1;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return -1;
            }
        }
        public decimal GetLineAmount(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.Amount;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return -1;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return -1;
            }

        }

        public bool IsPromoVoided(int PromoHdrID, out string status)
        {
            try
            {
                status = "";
                bool result = true;
                foreach (OrderDet od in myOrderDet)
                {
                    if (od.PromoHdrID == PromoHdrID && od.IsVoided == false)
                        result = false;

                }
                return result;
                //status = "Unable to find Order line Item with OrderDetID = " + LineID;
                //return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = " Detail error: " + ex.Message;
                return false;
            }

        }

        public bool IsItemNonDiscountable(string LineID, out string status)
        {
            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.Item.IsNonDiscountable;

                status = "Unable to find Order line Item with OrderDetID = " + LineID;
                return true;//if not sure discountable or not, assume non discountable
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Order line Item with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return true; //if not sure discountable or not, assume non discountable
            }
        }
        public string IsItemIsInOrderLine(string ItemNo)
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].ItemNo == ItemNo & !myOrderDet[i].IsVoided)
                {
                    return myOrderDet[i].OrderDetID;
                }
            }
            return "";
        }

        public bool IsItemIsRefundInOrderLine(string ItemNo)
        {
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].ItemNo == ItemNo && !myOrderDet[i].IsVoided && myOrderDet[i].Quantity == -1)
                    return true;

            }
            return false;
        }

        public decimal GetPreferredDiscount()
        {
            if (MembershipApplied() && preferredDiscount < membershipDiscount)
            {
                return membershipDiscount;
            }
            return preferredDiscount;
        }
        public decimal GetPreferredDiscountedPrice(decimal unitPrice, decimal discount)
        {
            //decimal discount = GetPreferredDiscount();
            decimal discountedPrice = Math.Round(unitPrice * (1 - (discount / 100)), 2);
            return discountedPrice;
        }
        internal string GetLineItemNo(string OrderLineID)
        {
            try
            {
                OrderDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.OrderDet)myOrderDet.Find(OrderLineID);
                return myOrderDetItem.ItemNo;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return "";
            }
        }

        public string GetLineIDOfItemNo(string itemNo)
        {
            string lineID = "";

            foreach (var od in myOrderDet)
            {
                if (od.ItemNo == itemNo)
                {
                    lineID = od.OrderDetID;
                    break;
                }
            }

            return lineID;
        }

        public decimal GetBalancePayment()
        {
            decimal BalancePayment = 0;
            string status = "";


            if (myOrderDet != null && myOrderDet.Count > 0)
            {
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided && myOrderDet[i].ItemNo == "INST_PAYMENT")
                    {
                        BalancePayment += myOrderDet[i].Amount;
                    }
                }
            }

            ReceiptDetCollection RcptDet = FetchUnsavedReceipt(out status);

            if (RcptDet != null && RcptDet.Count > 0)
            {
                for (int i = 0; i < RcptDet.Count; i++)
                {
                    if (RcptDet[i].PaymentType == POSController.PAY_INSTALLMENT)
                        BalancePayment += RcptDet[i].Amount;

                }
            }

            return BalancePayment;
        }

        public decimal GetTotalSales()
        {
            decimal BalancePayment = 0;
            string status = "";


            if (myOrderDet != null && myOrderDet.Count > 0)
            {
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided && myOrderDet[i].ItemNo != "INST_PAYMENT")
                    {
                        BalancePayment += myOrderDet[i].Amount;
                    }
                }
            }

            return BalancePayment;
        }

        public OrderDetCollection GetOrderItemPointPackage()
        {
            OrderDetCollection coll = new OrderDetCollection();
            foreach (OrderDet od in myOrderDet)
            {
                if (!od.IsVoided && od.Item.Category.CategoryName != "SYSTEM"
                    && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                {
                    coll.Add(od);
                }
            }
            return coll;
        }
        #endregion

        public bool CheckPackageUsageQuantity(out string packageStatus)
        {
            bool result = true;
            packageStatus = "";

            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (myOrderDet[i].Userflag5.GetValueOrDefault(false) == true)
                {
                    decimal CurrBalance = 0;
                    decimal CurrBreakdownPrice = 0;
                    decimal quantity = myOrderDet[i].Quantity.GetValueOrDefault(0);
                    string status = "";
                    string PackageRefNo = myOrderDet[i].PointItemNo;
                    var actionResult = Feature.Package.GetCurrentAmount_andBreakdownPrice(CurrentMember.MembershipNo, myOrderHdr.OrderDate,
                        PackageRefNo, out CurrBalance, out CurrBreakdownPrice, out status);
                    result = quantity <= CurrBalance;
                    if (!result)
                    {
                        packageStatus = string.Format("Remaining balance of {0} is not sufficient.\nYou only have {1} Credits", myOrderDet[i].Item.ItemName, CurrBalance.ToString("N0"));
                        break;
                    }
                }
            }

            return result;
        }

        public ReceiptDetCollection FetchReceiptDet()
        {
            return recDet;
        }

        #region "Validators"
        public bool HasOrderLine()
        {
            if (myOrderDet.Count <= 0) return false;

            foreach (OrderDet oneOrderDet in myOrderDet)
            {
                if (!oneOrderDet.IsVoided) return true;
            }

            return false;
        }

        public bool IsReturnedItemsOnly()
        {
            int totalItem = myOrderDet.Count;
            int returnedCount = 0;

            foreach (OrderDet od in myOrderDet)
            {
                if (!od.IsVoided && od.Quantity < 0 && od.Item.Category.CategoryName != "SYSTEM")
                    returnedCount++;
                else if (!od.IsVoided && od.Quantity > 0 && od.Item.ItemNo == "LINE_DISCOUNT")
                    returnedCount++;
            }

            return (totalItem == returnedCount);
        }

        public bool HasReturnedItems()
        {
            foreach (OrderDet od in myOrderDet)
            {
                if (!od.IsVoided && od.Quantity < 0 && od.Item.Category.CategoryName != "SYSTEM")
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasReturnedPointPackageItems()
        {
            foreach (OrderDet od in myOrderDet)
            {
                if (!od.IsVoided && od.Quantity < 0 && od.Item.Category.CategoryName != "SYSTEM"
                    && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasOrderedPointPackageItems()
        {
            foreach (OrderDet od in myOrderDet)
            {
                if (!od.IsVoided && od.Quantity > 0 && od.Item.Category.CategoryName != "SYSTEM"
                    && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasOrderedPointPackageItems(bool includingRefund)
        {
            foreach (OrderDet od in myOrderDet)
            {
                if (!od.IsVoided && (od.Quantity > 0 || includingRefund) && od.Item.Category.CategoryName != "SYSTEM"
                    && (od.Item.PointGetMode == "D" || od.Item.PointGetMode == "T"))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Assign Return Receipt
        public bool AssignSalesReturnReceiptNo(string LineID, string receiptNo, out string status)
        {
            //string status;
            //OrderDet myDet = GetLine(LineID, out status);
            //myDet.SalesReturnReceiptNo = receiptNo;
            //OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
            //((OrderDet)myOrderDet.Find(tmp.OrderDetID)).SalesReturnReceiptNo = receiptNo;

            try
            {
                status = "";
                OrderDet tmp = (OrderDet)myOrderDet.Find(LineID);
                if (tmp != null)
                {
                    tmp.ReturnedReceiptNo = receiptNo;
                    return true;
                }
                else
                {
                    status = "Unable to set Sales Return Receipt No with OrderDetID = " + LineID;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Unable to find Sales Return Receipt No with OrderDetID = " + LineID + ". Detail error: " + ex.Message;
                return false;
            }

        }
        #endregion

        #region "Add Delivery/Installation/SelfCollection"
        public bool AddDeliveryToOrder(string deliveryRemarks, string deliveryAddress, string StoreReference, string deliveryMode)
        {
            return AddDeliveryToOrder(deliveryRemarks, deliveryAddress, StoreReference, deliveryMode, null, null);
        }

        public bool AddDeliveryToOrder(string deliveryRemarks, string deliveryAddress, string StoreReference, string deliveryMode, object deliveryDate, string ShiftType)
        {
            try
            {
                myOrderHdr.Userfld6 = StoreReference;
                recHdr.Remark = String.IsNullOrEmpty(deliveryRemarks) ? " " : deliveryRemarks;
                recHdr.Remark = recHdr.Remark + "~";
                string delAdr = String.IsNullOrEmpty(deliveryAddress) ? " " : deliveryAddress;
                recHdr.Remark = recHdr.Remark + delAdr;
                myOrderHdr.Userfld7 = deliveryMode;
                //DeliveryAddress = deliveryAddress;

                if (deliveryDate != null)
                {
                    //if it is advise/taken, shift shouldn't be put & don't convert date
                    if (deliveryDate.ToString().ToLower() == "advised" || deliveryDate.ToString().ToLower() == "taken")
                    {
                        recHdr.DeliveryTime = "";
                        recHdr.DeliveryDate = deliveryDate.ToString();
                    }
                    else
                    {
                        recHdr.DeliveryTime = ShiftType;
                        recHdr.DeliveryDate = ((DateTime)deliveryDate).ToString("yyyy-MM-dd");
                    }
                }

                return true;
                //if not sure discountable or not, assume non discountable
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //status = "Unable to find Order line Item with OrderDetID = Detail error: " + ex.Message;
                return true;
                //if not sure discountable or not, assume non discountable
            }
        }

        public bool AddPurchaseOrderNo(string PurchaseOrderNo)
        {
            try
            {
                myOrderHdr.Userfld8 = PurchaseOrderNo;
                return true;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                //status = "Unable to find Order line Item with OrderDetID = Detail error: " + ex.Message;
                return true;
                //if not sure discountable or not, assume non discountable
            }
        }
        #endregion

        public bool IsUserHaveAuthorizationChangePrice(string username)
        {
            UserMst us = new UserMst(username);
            return true;
            //if (us == null)
            //    return false;
            //else
            //    return !string.IsNullOrEmpty(us.UserGroup.PriceRestrictedTo);

        }

        public bool IsAuthorizedChangePriceManually(Item it, string username, decimal newPrice, out string status)
        {
            status = "";
            try
            {
                decimal restrictedPrice = 0;
                if (newPrice >= it.RetailPrice)
                    return true;

                UserMst us = new UserMst(username);
                string restricted = string.IsNullOrEmpty(us.UserGroup.PriceRestrictedTo) ? "" : us.UserGroup.PriceRestrictedTo;

                if (string.IsNullOrEmpty(restricted))
                    throw new Exception("You are not authorized to change the price");

                if ((it.IsServiceItem.HasValue && it.IsServiceItem.Value) || it.IsInInventory || it.NonInventoryProduct)
                {
                    if (it.IsServiceItem.HasValue && it.IsServiceItem.Value)
                    {
                        if (restricted == UserGroupController.MinimumPrice)
                        {
                            restrictedPrice = 0;
                        }
                        else if (restricted == UserGroupController.CostPrice)
                        {
                            restrictedPrice = it.FactoryPrice;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (restricted == UserGroupController.MinimumPrice)
                        {
                            restrictedPrice = it.MinimumPrice == 0 ? it.RetailPrice : it.MinimumPrice;
                        }
                        else if (restricted == UserGroupController.CostPrice)
                        {
                            restrictedPrice = it.FactoryPrice;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }

                if (newPrice >= restrictedPrice)
                    return true;
                else
                    throw new Exception("You are not authorized to change the price");
            }
            catch (Exception ex)
            {
                status = ex.Message;
                return false;
            }
        }
    }

    //For the result of checking the barcode text
    public class ItemStruct
    {
        public string itemNo { get; set; }
        public bool isSpecialBarcode { get; set; }
        public decimal unitPrice { get; set; }
        public decimal qty { get; set; }
        public bool isOpenPrice { get; set; }

        public ItemStruct()
        {
            itemNo = "";
            isSpecialBarcode = false;
            unitPrice = 0;
            qty = 0;
        }
    }
}
