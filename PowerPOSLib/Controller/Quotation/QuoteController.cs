using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using SubSonic;
using PowerPOS.Container;
using System.Transactions;
using System.Windows.Forms;
using System.Configuration;
using PowerPOS.Controller;

namespace PowerPOS
{
    [Serializable]
    public partial class QuoteController
    {
        #region "Constants"

        //Constant for payment types        
        public const string PAY_CASH = "CASH";

        public const string PAY_FOREIGN_CURRENCY = "FOREIGN CURRENCY";

        public const string PAY_NETS = "NETS";

        public const string PAY_CHINA_NETS = "CHINA NETS";

        public const string PAY_AMEX = "AMEX";

        public const string PAY_VISA = "VISA";

        public const string PAY_MASTER = "MASTER";

        public const string PAY_VOUCHER = "VOUCHER";

        public const string PAY_CHEQUE = "CHEQUE";

        public const string PAY_INSTALLMENT = "INSTALLMENT";

        public const string PAY_POINTS = "POINTS";

        public const string PAY_PACKAGE = "PACKAGE";

        //public const bool ENABLE_PROGRAMMABLE_KEYBOARD = false;

        public const bool DISCOUNT_BY_PERCENTAGE = true;


        public const string VOUCHER_BARCODE = "VOUCHER";

        public static string RoundingPreference;

        public const string ROUNDING_ITEM = "R0001";

        #endregion

        private double GST;
        private decimal preferredDiscount;

        [field: NonSerializedAttribute()]
        public event OpenPriceItemHandler OpenPriceItemAdded;
        [field: NonSerializedAttribute()]
        public event OpenPriceItemHandler OpenPriceItemAddedHotKey;

        public QuotationHdr myOrderHdr;             //Order Header        
        public QuotationDetCollection myOrderDet;   //Order Detail
        //public PreOrderRecord preOrderInfo;
        public const string DEPOSIT_ITEM = "DEPOSIT";
        private Dictionary<string, decimal> _listOpenItemFromHotKeys = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> ListOpenItemFromHotKeys
        {
            get { return _listOpenItemFromHotKeys; }
            private set { _listOpenItemFromHotKeys = value; }
        }

        bool isNewMember;
        private decimal membershipDiscount;
        public Membership CurrentMember;
        public string redemptionMembershipNo;

        private DateTime newExpiryDate; //For renewals...
        private int newMembershipGroupID = -1;

        public QuoteController()
        {
            ResetObject();
        }
        //Creaet new order hdr object
        public void ResetObject()
        {
            try
            {
                //Initialize order hdr
                myOrderHdr = new QuotationHdr();
                myOrderDet = new QuotationDetCollection();
                isNewMember = false;
                LoadGST();
                membershipDiscount = 0;
                myOrderHdr.OrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
                bool useCustomQuotationNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_UseCustomNo), false);
                if (useCustomQuotationNo)
                {
                    myOrderHdr.OrderRefNo = CreateNewCustomReceiptNo();
                }
                else
                {
                    myOrderHdr.OrderRefNo = "OR" + myOrderHdr.OrderHdrID;
                }
                myOrderHdr.CashierID = UserInfo.username;
                myOrderHdr.Discount = 0; //get default values from database
                myOrderHdr.Gst = GST;

                myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;  //PointOfSaleInfo.PointOfSaleID


                myOrderHdr.OrderDate = DateTime.Now;
                myOrderHdr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID; //get default values from config file

                //Initialize order detail
                myOrderDet = new QuotationDetCollection();

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public static string GetOrderHdrIDByCustomReceiptNo(string receiptNo)
        {
            string orderhdrid = "";

            string sql = "select orderhdrid from orderhdr where userfld5 = '" + receiptNo + "'";

            QueryCommand qr = new QueryCommand(sql);

            object found = DataService.ExecuteScalar(qr);
            if (found != null)
                orderhdrid = found.ToString();

            return orderhdrid;

        }

        public static string GetCustomReceiptNoByOrderHdrID(string OrderHdrID)
        {
            string customNo = "";

            string sql = "select userfld5 from orderhdr where orderhdrid = '" + OrderHdrID + "'";

            QueryCommand qr = new QueryCommand(sql);

            object found = DataService.ExecuteScalar(qr);
            if (found != null)
                customNo = found.ToString();

            return customNo;

        }

        //Load existing OrderHdrs from the receipt number
        public QuoteController(string OrderHdrID)
        {
            try
            {
                myOrderHdr = new QuotationHdr(OrderHdrID);

                myOrderDet = new QuotationDetCollection().Where("OrderHdrID", OrderHdrID).Load();

                if (myOrderHdr.MembershipNo != null && myOrderHdr.MembershipNo != "")
                {
                    CurrentMember = new Membership(Membership.Columns.MembershipNo, myOrderHdr.MembershipNo);
                }
                else
                {
                    CurrentMember = new Membership();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        //Get the maximum OrderDet Line ID
        //use this to add new OrderDet
        private string GetDetMaxID(out string status)
        {
            status = "";
            try
            {
                if (myOrderDet != null)
                {
                    //if the list is empty, start with zero
                    if (myOrderDet.Count == 0) return "1";

                    //otherwise loop through the existing orderdet and find the max value
                    int max = 0;
                    int lineVal;
                    for (int i = 0; i < myOrderDet.Count; i++)
                    {
                        //get the number behind the dot
                        lineVal = int.Parse(myOrderDet[i].OrderDetID.Split('.')[1]);
                        //if it is more than the max, assign it
                        if (max < lineVal)
                        {
                            max = lineVal;
                        }
                    }
                    //return the max plus 1
                    return (max + 1).ToString();
                }
                //if somehow failed, return this
                status = "Order Line Detail has not been created.";
                return "1";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return "";
            }
        }

        //Load GST from database
        private bool LoadGST()
        {
            //Load GST from GST Table
            Query qry = new Query("GST");
            Where whr = new Where();
            whr.ColumnName = PowerPOS.Gst.Columns.CommenceDate;
            whr.Comparison = Comparison.LessOrEquals;
            whr.ParameterName = "@CommenceDate";
            whr.ParameterValue = DateTime.Now.ToString("yyyy-MM-dd");
            whr.TableName = "GST";
            //pull out from GST table
            object obj = qry.GetMax(PowerPOS.Gst.Columns.GSTRate, whr);
            GST = (double.Parse(obj.ToString()));
            return true;
        }
        //set the prefered line discount
        public void SetPreferredDiscount(decimal disc)
        {
            preferredDiscount = disc;
        }

        //is the receipt voided?
        public bool IsVoided()
        {
            //get whether or not receipt is voided
            return myOrderHdr.IsVoided;
        }
        //get the gst set
        public double getGST(out string status)
        {

            status = "";
            return myOrderHdr.Gst;
        }

        #region "Static Method"
        //Load the GST
        /*
        public static double LoadGST(out string status)
        {
            try
            {
                //Load GST from GST Table
                Query qry = new Query("GST");
                Where whr = new Where();
                whr.ColumnName = PowerPOS.Gst.Columns.CommenceDate;
                whr.Comparison = Comparison.LessOrEquals;
                whr.ParameterName = "@CommenceDate";
                whr.ParameterValue = DateTime.Now.ToString("dd MMM yyyy");
                whr.TableName = "GST";
                object obj = qry.GetMax(PowerPOS.Gst.Columns.GSTRate, whr);
                status = "";
                return (double.Parse(obj.ToString()));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error:" + ex.Message;
                return -1;
            }
        }
        */
        /*
        public static ArrayList FetchPaymentTypes()
        {
            ArrayList ar = new ArrayList();
            ar.Add("");
            ar.Add(POSController.PAY_AMEX);
            ar.Add(POSController.PAY_CASH);
            ar.Add(POSController.PAY_CHINA_NETS);
            ar.Add(POSController.PAY_NETS);
            ar.Add(POSController.PAY_VISA);
            ar.Add(POSController.PAY_VOUCHER);
            ar.Add(POSController.PAY_CHEQUE);
            return ar;
        }
        */

        //Create new order ref no for POS
        private string CreateNewOrderNo(int PointOfSaleID)
        {
            int runningNo = 0;

            //use stored procedure to pull out the biggest number for today's order
            //format of order: YYMMDDSSSSNNNN
            //This stored procedure pull out the last order number
            string cmd = @"SELECT isnull(max(right(orderhdrid,4)),'0') from quotationhdr
	where left(orderhdrid,6) = substring(convert(varchar,getdate(),112),3,6)
	AND convert(int,substring(orderhdrid,7,4)) = " + PointOfSaleInfo.PointOfSaleID;
            DataTable dt = DataService.GetDataSet(new QueryCommand(cmd)).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!int.TryParse(dt.Rows[i][0].ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            //ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
        }

        public string CreateNewCustomReceiptNo()
        {
            //string prefixselect = ;
            string prefix = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_CustomPrefix);

            //string suffixselect = "select AppSettingValue from AppSetting where AppSettingKey='ReceiptSuffix'";
            //string suffix = DataService.ExecuteScalar(new QueryCommand(suffixselect)).ToString();

            //get current receiptno
            string sql = "select case AppSettingValue when '' then '0' else AppSettingValue end from AppSetting where AppSettingKey='Quotation_CurrentReceiptNo'";
            QueryCommand Qcmd = new QueryCommand(sql);
            string currentReceiptNo = DataService.ExecuteScalar(Qcmd).ToString();

            //default max receiptno is 4
            string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='Quotation_ReceiptLength'";
            QueryCommand Qcmd2 = new QueryCommand(sql2);
            int maxReceiptNo = 4;
            int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

            int runningNo = 0;
            if (currentReceiptNo != null)
            {
                #region *) obsoleted doesn't make sense code to add running number
                //string str2 = str, zerostr = "";
                //while (str2.Substring(0, 1) == "0")
                //{
                //    zerostr += "0";
                //    str2 = str2.Substring(1);
                //    if (str2 == "")
                //    {
                //        break;
                //    }
                //}

                //if (Convert.ToInt32(str) == 0)
                //    str = zerostr.Substring(1) + "1";
                //else
                //    str = zerostr + Convert.ToString(Convert.ToInt32(str) + 1);


                //if (str == "9999")
                //{
                //    string updatemaxnum = "update appsetting set AppSettingValue='00000' where AppSettingKey='MaxReceiptNumber'";
                //    DataService.ExecuteQuery(new QueryCommand(updatemaxnum));

                //    string sql2 = "select AppSettingValue from AppSetting where AppSettingKey='MaxReceiptNumber'";
                //    string newmax = DataService.ExecuteQuery(new QueryCommand(sql2)).ToString();
                //    //LastDigit = FristDig + "0001";

                //    str2 = newmax;
                //    zerostr = "";
                //    while (str2.Substring(0, 1) == "0")
                //    {
                //        zerostr += "0";
                //        str2 = str2.Substring(1);
                //        if (str2 == "")
                //        {
                //            break;
                //        }
                //    }

                //    if (Convert.ToInt32(str) == 0)
                //        str = zerostr.Substring(1) + "1";
                //    else
                //        str = zerostr + Convert.ToString(Convert.ToInt32(newmax) + 1);

                //}

                //if (str == "99999")
                //{
                //    string updatemaxnum = "update appsetting set AppSettingValue='000000' where AppSettingKey='MaxReceiptNumber'";
                //    DataService.ExecuteQuery(new QueryCommand(updatemaxnum));

                //    string sql2 = "select AppSettingValue from AppSetting where AppSettingKey='MaxReceiptNumber'";
                //    string newmax = DataService.ExecuteQuery(new QueryCommand(sql2)).ToString();
                //    //LastDigit = FristDig + "0001";

                //    str2 = newmax;
                //    zerostr = "";
                //    while (str2.Substring(0, 1) == "0")
                //    {
                //        zerostr += "0";
                //        str2 = str2.Substring(1);

                //        if (str2 == "")
                //        {
                //            break;
                //        }
                //    }
                //    if (Convert.ToInt32(str) == 0)
                //        str = zerostr.Substring(1) + "1";
                //    else
                //        str = zerostr + Convert.ToString(Convert.ToInt32(newmax) + 1);
                //}
                #endregion
                if (!int.TryParse(currentReceiptNo, out runningNo))
                {
                    runningNo = 0;
                }
                runningNo += 1;
            }

            return prefix + runningNo.ToString().PadLeft(maxReceiptNo, '0');
        }




        public static DateTime FetchLatestCloseCounterTime(int PointOfSaleID)
        {
            Query qr = new Query("CounterCloseLog");
            qr.Top = "1";

            DataSet ds = qr.WHERE(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleID)
                .AND(CounterCloseLog.Columns.EndTime, Comparison.LessOrEquals, DateTime.Now).ORDER_BY("EndTime", "DESC").ExecuteDataSet();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["EndTime"] is DateTime)
            {
                return (DateTime)ds.Tables[0].Rows[0]["EndTime"];
            }
            else
            {
                //Get the login date time....
                Query qr1 = new Query("LoginActivity");
                Where myWhere1 = new Where();
                myWhere1.ColumnName = LoginActivity.Columns.PointOfSaleID;
                myWhere1.ParameterName = "@PointOfSaleID";
                myWhere1.Comparison = Comparison.Equals;
                myWhere1.ParameterValue = PointOfSaleInfo.PointOfSaleID;
                myWhere1.TableName = "LoginActivity";

                Object maxLoginDate = qr1.GetMin(LoginActivity.Columns.LoginDateTime, myWhere1);
                if (maxLoginDate is DateTime)
                    return (DateTime)maxLoginDate;
                return new DateTime(1979, 11, 3);
            }
        }

        public static DateTime FetchFirstLoginAfterCloseCounterTime()
        {
            Query qr = new Query("CounterCloseLog");
            Where myWhere = new Where();
            myWhere.ColumnName = CounterCloseLog.Columns.PointOfSaleID;
            myWhere.ParameterName = "@PointOfSaleID";
            myWhere.Comparison = Comparison.Equals;
            myWhere.ParameterValue = PointOfSaleInfo.PointOfSaleID;
            myWhere.TableName = "CounterCloseLog";

            Object maxDate = qr.GetMax(CounterCloseLog.Columns.EndTime, myWhere);
            if (maxDate is DateTime)
            {
                //Fetch cash recording with opening balance
                //(new Query("CashRecording"))
                return (DateTime)maxDate;
            }
            else
            {
                //Get the login date time....
                Query qr1 = new Query("LoginActivity");
                Where myWhere1 = new Where();
                myWhere1.ColumnName = LoginActivity.Columns.PointOfSaleID;
                myWhere1.ParameterName = "@PointOfSaleID";
                myWhere1.Comparison = Comparison.Equals;
                myWhere1.ParameterValue = PointOfSaleInfo.PointOfSaleID;
                myWhere1.TableName = "LoginActivity";

                Object maxLoginDate = qr1.GetMin(LoginActivity.Columns.LoginDateTime, myWhere);
                return (DateTime)maxLoginDate;
            }
        }


        #endregion
        public void AppendOrderDet(QuotationDetCollection orderDetCollection)
        {
            #region *) Validation: Ignore if don't have anything
            if (orderDetCollection.Count == 0) return;
            #endregion

            int max;
            //find the max
            if (myOrderDet.Count > 0)
            {
                max = 0; //start from zero
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    int lastIndex = myOrderDet[i].OrderDetID.LastIndexOf('.');
                    if (myOrderDet[i].OrderDetID.Length > lastIndex + 1)
                    {
                        //get the number behind the dot
                        int tmp = int.Parse(
                            myOrderDet[i].OrderDetID.Substring(lastIndex + 1));
                        if (tmp > max) max = tmp;  //assign if current number is bigger
                    }
                }
                max++; //add 1 from the max
            }
            else
            {
                max = 0; //start from zero
            }
            //get the orderdet from the temporary collection
            int n = 0;

            while (true)
            {
                //create a new det id
                string newDetID = myOrderHdr.OrderHdrID + "." + max.ToString();
                //assign the new orderdet
                orderDetCollection[n].OrderDetID = newDetID;
                myOrderDet.Add(orderDetCollection[n]);
                orderDetCollection.RemoveAt(n);
                max++; //increase the counter
                if (orderDetCollection.Count == 0) break;
            }
            TryMergeALLOrderLine();
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
        public bool ConfirmOrder(bool doRounding, out bool isPointAllocated, out string status)
        {
            isPointAllocated = false;

            try
            {


                decimal diffPoint = 0;  /// Jumlah point yang akan ditambah / di-deduct ke server
                decimal availablePoint = 0;

                DataTable PackageList = new DataTable();
                status = "";
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

                        ChangeOrderLineUnitPriceQuotation
                            (IsItemIsInOrder(ROUNDING_ITEM),
                             roundAmt - actualAmt, out status);
                    }
                }
                #endregion


                #region *) Core: Save OrderHdr Info
                //create a new order refno
                string newOrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
                myOrderHdr.OrderHdrID = newOrderHdrID;
                myOrderHdr.CashierID = UserInfo.username;
                string prefix = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_CustomPrefix);
                if (prefix != null && prefix != "")
                {
                    myOrderHdr.OrderRefNo = prefix + newOrderHdrID;
                }
                else
                {
                    myOrderHdr.OrderRefNo = "OR" + newOrderHdrID;
                }

                myOrderHdr.OrderDate = DateTime.Now;
                myOrderHdr.UniqueID = Guid.NewGuid();
                myOrderHdr.NettAmount = CalculateTotalAmount(out status); //use total paid > How much customer paid. This amount can be more because of vouchers
                myOrderHdr.GrossAmount = CalculateGrossAmount();
                if (MembershipApplied())
                { myOrderHdr.MembershipNo = CurrentMember.MembershipNo; }
                string useCustomInvoiceNo = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_UseCustomNo);
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
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (!myOrderDet[i].IsVoided) //only save non voided transactions
                    {
                        myOrderDet[i].OrderHdrID = newOrderHdrID;
                        myOrderDet[i].UniqueID = Guid.NewGuid();
                        myOrderDet[i].OrderDetID = newOrderHdrID + "." + i.ToString();
                        myOrderDet[i].Userint4 = i;
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
                        /*
                        else if (myOrderDet[i].ItemNo == DEPOSIT_ITEM)
                        {
                            cmd.AddRange(MembershipTapController.adjustTap
                                (CurrentMember.MembershipNo, myOrderDet[i].Amount, UserInfo.username, out status));
                        }*/
                    }
                }
                #endregion

                status = "";

                #region *) Core: Commit all local transaction
                SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                return true;
            }
            catch (Exception ex)
            {


                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");

                //log into logger
                Logger.writeLog(ex);

                return false;
            }
        }





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
        public decimal CalculateLineGrossAmount(QuotationDet oneOrderDet)
        {
            decimal result = 0;
            if (!((bool)oneOrderDet.IsVoided))
            {
                if (oneOrderDet.Item.IsServiceItem.GetValueOrDefault(false)
                    || oneOrderDet.Item.CategoryName.ToLower() == "system")
                {
                    result += oneOrderDet.Quantity * oneOrderDet.UnitPrice;
                }
                else
                {
                    result += oneOrderDet.Quantity * oneOrderDet.OriginalRetailPrice;
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
                int dotPos = discount.ToString().LastIndexOf('.');
                if (dotPos >= 0)
                    discount = decimal.Parse(discount.ToString().Substring(0, dotPos + 3));
                return discount;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region "Add Item To Order"

        private string IsItemIsInOrder(string ItemNo)
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

        public bool ChangeOrderLineUnitPriceQuotation(string OrderDetID, decimal newUnitPrice, out string status)
        {
            status = "";
            try
            {
                QuotationDet myOrderDetItem;
                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
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


                        return true;
                    }
                }
                #endregion

                //default values
                QuotationDet tmpdet = new QuotationDet();
                string maxID = GetDetMaxID(out status);
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + maxID;
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;
                tmpdet.IsFreeOfCharge = false;
                tmpdet.IsVoided = false;
                tmpdet.IsPreOrder = defaultPreOrder;
                tmpdet.Userint4 = int.Parse(maxID);
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
                if (myOrderDet != null)
                {
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





                            return true;
                        }
                    }
                }

                //default values
                QuotationDet tmpdet = new QuotationDet();
                string maxID = GetDetMaxID(out status);
                tmpdet.Userint4 = int.Parse(maxID);
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + maxID;
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
                if (myOrderDet != null)
                {
                    myOrderDet.Add(tmpdet);
                }


                //Open price from a Hot Keys
                //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsServiceItem.GetValueOrDefault(false))
                //{
                //    if (OpenPriceItemAddedHotKey != null)
                //        OpenPriceItemAddedHotKey(this, tmpdet.OrderDetID);
                //}
                //Open price from order taking form
                if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory)
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





                        return true;
                    }
                }

                //default values
                QuotationDet tmpdet = new QuotationDet();
                string maxID = GetDetMaxID(out status);
                tmpdet.Userint4 = int.Parse(maxID);
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + maxID;
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

                myOrderDet.Add(tmpdet);


                //Open price from a Hot Keys
                //if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsServiceItem.GetValueOrDefault(false))
                //{
                //    if (OpenPriceItemAddedHotKey != null)
                //        OpenPriceItemAddedHotKey(this, tmpdet.OrderDetID);
                //}
                //Open price from order taking form
                if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory)
                {
                    if (needToEnterOpenPrice)
                        CallOpenPriceItemAdded(tmpdet.OrderDetID);
                    else
                        _listOpenItemFromHotKeys.Add(tmpdet.OrderDetID, 0);
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

                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }
                //Load default Pre Order value                

                string itemno = myItem.ItemNo;


                //Item already exist?
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    if (myOrderDet[i].ItemNo == myItem.ItemNo
                            && !myOrderDet[i].IsSpecial
                            && !myOrderDet[i].IsExchange
                            && !((bool)myOrderDet[i].IsVoided)
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

                        QuotationDet myDet = myOrderDet[i];
                        CalculateLineAmount(ref myDet);

                        return true;
                    }
                }

                //default values
                QuotationDet tmpdet = new QuotationDet();
                string maxID = GetDetMaxID(out status);
                tmpdet.Userint4 = int.Parse(maxID);
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + maxID;
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

                myOrderDet.Add(tmpdet);


                //Open price from order taking form
                if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory)
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


                status = "";
                //Check if OrderHeader is empty....
                if (myOrderHdr == null)
                {
                    throw new Exception("Trying to add item into a non existance order.");
                }

                //default values
                QuotationDet tmpdet = new QuotationDet();
                tmpdet.Remark = remark;
                string maxID = GetDetMaxID(out status);
                tmpdet.Userint4 = int.Parse(maxID);
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + maxID;
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

        public bool AddItemToOrder(QuotationDet detail, out string status)
        {
            status = "";
            try
            {
                QuotationDet tmpdet = new QuotationDet();
                tmpdet = detail;
                tmpdet.OrderDetID = null;
                string maxID = GetDetMaxID(out status);
                tmpdet.Userint4 = int.Parse(maxID);
                tmpdet.OrderDetID = myOrderHdr.OrderHdrID + "." + maxID;
                tmpdet.OrderHdrID = myOrderHdr.OrderHdrID;

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

        #endregion

        #region "Setters - Change Attributes"

        public bool ChangeOrderLineQuantity(string OrderDetID, int newQty, bool ApplyPromo, out string status)
        {
            status = "";
            if (newQty < 1)
            {
                status = "Quantity must be more than zero. To delete, void the line item. Press Backspace if you want to void the line item.";
                return false;
            }


            try
            {
                QuotationDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
                if (myOrderDetItem.IsExchange)
                {
                    //compare with max qty
                    if ((new QuotationDet(myOrderDetItem.ExchangeDetRefNo)).Quantity < Math.Abs(newQty))
                    {
                        status = "Quantity exceeded the original quantity of the exchanged item.";
                        return false;
                    }

                    if (newQty > 0)
                        newQty = -newQty;
                }
                myOrderDetItem.Quantity = newQty;

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

        public bool ChangeOrderLineDiscount(string OrderDetID, decimal newDiscount, bool AllowChangeSpecial, out string status)
        {
            status = "";
            try
            {
                QuotationDet myOrderDetItem;


                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
                if (myOrderDet == null) return false;
                if (AllowChangeSpecial
                    || (!AllowChangeSpecial & myOrderDetItem.IsSpecial == false)
                    )
                {
                    if (!myOrderDetItem.Item.IsNonDiscountable && !myOrderDetItem.IsExchange)
                    {
                        if (!myOrderDetItem.IsPromo)
                        {
                            /*if (MembershipApplied())
                            {
                                newDiscount += membershipDiscount;//(decimal)CurrentMember.MembershipGroup.Discount;
                            }*/
                            myOrderDetItem.Discount = newDiscount;
                        }
                    }
                    else
                    {
                        myOrderDetItem.Discount = 0;
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

        public bool ChangeOrderLineDiscount(string OrderDetID, decimal newDiscount, bool AllowChangeSpecial, bool checkSPP, out string status)
        {
            status = "";
            try
            {
                QuotationDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
                if (myOrderDet == null) return false;
                if (AllowChangeSpecial
                    || (!AllowChangeSpecial & myOrderDetItem.IsSpecial == false)
                    )
                {
                    if (!myOrderDetItem.Item.IsNonDiscountable && !myOrderDetItem.IsExchange)
                    {
                        if (!myOrderDetItem.IsPromo)
                        {
                            /*if (MembershipApplied())
                            {
                                newDiscount += membershipDiscount;//(decimal)CurrentMember.MembershipGroup.Discount;
                            }*/
                            myOrderDetItem.Discount = newDiscount;
                            if (myOrderDetItem.Item.Userfloat4.HasValue && myOrderDetItem.Item.Userfloat4 > 0)
                            {
                                //adi add for spp
                                Decimal DiscountPrice = (decimal)myOrderDetItem.Item.RetailPrice - (decimal)myOrderDetItem.Item.Userfloat4;
                                myOrderDetItem.Discount = (decimal)(DiscountPrice / myOrderDetItem.Item.RetailPrice * 100);
                            }
                        }
                    }
                    else
                    {
                        myOrderDetItem.Discount = 0;
                    }

                    myOrderDetItem.DiscountDetail = myOrderDetItem.Discount.ToString("N0") + "%";
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

        public bool ChangeOrderLineDiscount(string OrderDetID, decimal BankDiscount, decimal TotalAmount, out string status)
        {
            status = "";
            try
            {
                QuotationDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
                if (myOrderDet == null) return false;
                decimal currentDisc = myOrderDetItem.Discount;
                decimal BankDisc = 0;
                if (myOrderDetItem.IsPromo)
                {
                    BankDisc = Math.Round(myOrderDetItem.PromoAmount / TotalAmount * BankDiscount, 4);
                }
                else
                {
                    BankDisc = Math.Round(myOrderDetItem.Amount / TotalAmount * BankDiscount, 4);
                }
                decimal DiscountPrice = currentDisc * myOrderDetItem.OriginalRetailPrice / 100 + BankDisc;

                //myOrderDetItem.Discount = DiscountPrice / myOrderDetItem.OriginalRetailPrice * 100;
                if (myOrderDetItem.Userfloat1.HasValue)
                {
                    myOrderDetItem.Userfloat1 = myOrderDetItem.Userfloat1 + BankDisc;
                }
                else
                {
                    myOrderDetItem.Userfloat1 = BankDisc;
                }
                //get discount price additional 
                CalculateLineAmount(ref myOrderDetItem);
                Logger.writeLog(myOrderDetItem.ItemNo + ", " + Convert.ToDecimal(myOrderDetItem.GrossSales).ToString() + ", "
                                + Convert.ToDecimal(myOrderDetItem.Userfloat1).ToString("N0") + ", " + Convert.ToDecimal(myOrderDetItem.Amount).ToString("N0") + ", BANK DISC");
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
                QuotationDet myOrderDetItem;
                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
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

        public bool SetVoidOrderLine(string OrderDetID, bool value, out string status)
        {
            status = "";
            try
            {

                QuotationDet myOrderDetItem;
                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderDetID);
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

        public bool SetLineRemark(string LineID, string remark, out string status)
        {
            try
            {
                status = "";
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
            QuotationDet tmp = (QuotationDet)myOrderDet.Find(OrderLineID);
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
                        )
                    {
                        if ((myOrderDet[i].IsPreOrder.HasValue == tmp.IsPreOrder.HasValue &&
                            myOrderDet[i].IsPreOrder.Value == tmp.IsPreOrder.Value))
                        {

                            myOrderDet[i].Quantity += tmp.Quantity;
                            QuotationDet tst = myOrderDet[i];
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

        #endregion

        #region "Fetch Details"
        public QuotationDetCollection FetchUnsavedOrderDet()
        {
            return myOrderDet;
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
                dTable.Columns.Add("DiscountDetail", Type.GetType("System.String"));

                Item myItem;
                decimal qty, unitprice;
                myOrderDet.Sort("Userint4", true);

                for (int i = myOrderDet.Count - 1; i >= 0; i--)
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
                    qty = myOrderDet[i].Quantity;
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
                        dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? "0%" : myOrderDet[i].DiscountDetail;
                    }
                    else
                    {
                        decimal DPrice = (unitprice - unitprice * myOrderDet[i].Discount / 100);
                        dr["DPrice"] = CommonUILib.RemoveRoundUp(DPrice);

                        dr["Amount"] = CommonUILib.RemoveRoundUp(myOrderDet[i].Amount).ToString("N2");
                        dr["IsFreeOfCharge"] = myOrderDet[i].IsFreeOfCharge;
                        dr["Disc(%)"] = (myOrderDet[i].Discount).ToString("N0") + "%";
                        dr["Disc($$)"] = CommonUILib.RemoveRoundUp(CalculateDiscountInDollar(i)).ToString("N2");
                        dr["DiscountDetail"] = string.IsNullOrEmpty(myOrderDet[i].DiscountDetail) ? "0%" : myOrderDet[i].DiscountDetail;
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
                return dTable;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "";
                return null;
            }
        }

        public DataTable FetchUnSavedOrderItemsForReceipt(out string status)
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
                myOrderDet.Sort("Userint4", true);

                for (int i = myOrderDet.Count - 1; i >= 0; i--)
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
                    qty = myOrderDet[i].Quantity;
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
                myOrderDet.Sort(OrderDet.Columns.OrderDetDate, true);
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
                        qty = myOrderDet[i].Quantity;
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


        public string GetCustomizedRefNo()
        {
            if (myOrderHdr != null && myOrderHdr.IsLoaded)
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
        public int GetSumOfItemQuantity()
        {
            int total = 0;
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!(bool)myOrderDet[i].IsVoided)
                    total += myOrderDet[i].Quantity.GetIntValue();
            }
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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

        public bool IsVoided(string LineID, out string status)
        {
            try
            {
                status = "";
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
        public int GetLineQuantity(string LineID, out string status)
        {
            try
            {
                status = "";
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
                if (tmp != null)
                    return tmp.Quantity.GetIntValue();

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
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
        public bool IsItemNonDiscountable(string LineID, out string status)
        {
            try
            {
                status = "";
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
        public decimal GetPreferredDiscount()
        {
            if (MembershipApplied() && preferredDiscount < membershipDiscount)
            {
                return membershipDiscount;
            }
            return preferredDiscount;
        }
        internal string GetLineItemNo(string OrderLineID)
        {
            try
            {
                QuotationDet myOrderDetItem;

                myOrderDetItem = (PowerPOS.QuotationDet)myOrderDet.Find(OrderLineID);
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
        #endregion

        #region "Validators"
        public bool HasOrderLine()
        {
            if (myOrderDet.Count <= 0) return false;

            foreach (QuotationDet oneOrderDet in myOrderDet)
            {
                if (!oneOrderDet.IsVoided) return true;
            }

            return false;
        }
        #endregion



        #region API
        public bool applyDiscount(decimal disc)
        {
            string status;
            //apply discounts to Non-Promo items
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                if (!ChangeOrderLineDiscount
                    (myOrderDet[i].OrderDetID, disc, false, out status))
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
                    myOrderDet[i].Userfloat1 = 0;
                    if (!ChangeOrderLineDiscount
                        (myOrderDet[i].OrderDetID, disc, true, true, out status))
                    {
                        throw new Exception("status");
                    }
                    myOrderDet[i].Remark = "";
                    myOrderDet[i].Userfld6 = null;
                    myOrderDet[i].Userfld7 = null;
                }
            }
            myOrderHdr.Userfld7 = "";
            //SetPreferredDiscount(disc);
            return true;
        }

        public bool applyBankDiscount(decimal BankDiscAmount, string discName)
        {
            string status;
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
            //SetPreferredDiscount(disc);
            return true;
        }

        //Apply discount to all item in the receipt
        public bool applyDiscount(string discName)
        {
            return applyDiscount(discName, true);
        }

        //Apply discount to all item in the receipt
        public bool applyDiscount(string discName, bool overwriteExisting)
        {
            //string status;
            //apply discounts to Non-Promo items
            SpecialDiscount sd = new SpecialDiscount("DiscountName", discName);
            decimal DiscountPercent = sd.DiscountPercentage;
            bool useSPP = (bool)sd.UseSPP;
            //Decimal DiscountPrice;
            //Decimal RetailPrice;
            if ((bool)sd.Enabled)
            {
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    QuotationDet od = myOrderDet[i];
                    if (!applyDiscountOrderDet(sd.DiscountName, ref od, overwriteExisting))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        public bool applyDiscountOrderDet(string discName, ref QuotationDet od)
        {
            return applyDiscountOrderDet(discName, ref od, true);
        }

        public bool applyDiscountOrderDet(string discName, ref QuotationDet od, bool overwriteExisting)
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
                if ((bool)sd.Enabled)
                {

                    if (od.Item.CategoryName.ToUpper() != "SYSTEM" && !(od.IsPromo))
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
                            QuotationDet det = od;
                            det.Userfld5 = sd.DiscountName;

                            if (useSPP == true && det.Item.Userfloat4.HasValue)
                            {
                                Decimal.TryParse(det.Item.Userfloat4.ToString(), out RetailPrice);
                                //det.UnitPrice = RetailPrice;
                                DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity;
                                det.Remark = "SPP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%";
                                det.Userfloat5 = RetailPrice;
                            }
                            else
                            {
                                RetailPrice = det.UnitPrice;
                                DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity;
                                det.Remark = "RP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%";
                                det.Userfloat5 = 0;
                            }

                            decimal CurrentAmount = RetailPrice - PromoSpecialDiscountController.RoundUp((DiscountPercent * RetailPrice / 100));

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
                            det.Amount = (RetailPrice * det.Quantity) - DiscountPrice;
                            det.Userfld6 = discName;
                            CalculateLineAmount(ref det);
                            Logger.writeLog(det.ItemNo + ", " + Convert.ToDecimal(det.GrossSales).ToString() + ", "
                                + Convert.ToDecimal(det.Discount).ToString("N0") + ", " + Convert.ToDecimal(det.Amount).ToString("N0") + ", " + det.Userfld5);

                        }
                        else
                        {
                            QuotationDet det = od;

                            SpecialDiscountDetailCollection sdd = new SpecialDiscountDetailCollection();
                            sdd.Where(SpecialDiscountDetail.Columns.DiscountName, sd.DiscountName);
                            sdd.Where(SpecialDiscountDetail.Columns.ItemNo, det.ItemNo);
                            sdd.Load();

                            if (sdd.Count > 0)
                            {
                                det.Userfld5 = sd.DiscountName;
                                decimal discAmt = 0;
                                if (sdd[0].DiscountPercentage != null && sdd[0].DiscountPercentage != 0)
                                {
                                    DiscountPercent = (decimal)sdd[0].DiscountPercentage;
                                }
                                else
                                {
                                    DiscountPercent = 0;
                                    discAmt = (decimal)sdd[0].DiscountAmount;

                                }
                                if (useSPP == true && det.Item.Userfloat4.HasValue)
                                {
                                    Decimal.TryParse(det.Item.Userfloat4.ToString(), out RetailPrice);
                                    //det.UnitPrice = RetailPrice;
                                    DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity + (discAmt * det.Quantity);
                                    det.Remark = "SPP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%, Amount = " + discAmt.ToString("N2");
                                    det.Userfloat5 = RetailPrice;
                                }
                                else
                                {
                                    RetailPrice = det.UnitPrice;
                                    DiscountPrice = PromoSpecialDiscountController.RoundUp(RetailPrice * ((decimal)DiscountPercent / 100)) * det.Quantity + (discAmt * det.Quantity);
                                    det.Remark = "RCP = " + RetailPrice.ToString("N2") + ", Discount = " + DiscountPercent.ToString("N2") + "%, Amount = " + discAmt.ToString("N2");
                                    det.Userfloat5 = 0;
                                }

                                decimal CurrentAmount = RetailPrice - PromoSpecialDiscountController.RoundUp(DiscountPercent * RetailPrice / 100) - discAmt;

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
                                det.Amount = (RetailPrice * det.Quantity) - DiscountPrice;
                                det.Userfld6 = discName;

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

                    if (!od.IsPromo)
                    {
                        od.Userfloat1 = 0;
                        if (!ChangeOrderLineDiscount
                            (od.OrderDetID, 0, true, true, out status))
                        {
                            throw new Exception("status");
                        }
                        od.Remark = "";
                        od.Userfld6 = null;
                        od.Userfld7 = null;
                    }
                    myOrderHdr.Userfld7 = "";

                }
                return true;
            }
        }

        //get line amount using the ID
        public QuotationDet GetLine(string LineID, out string status)
        {
            try
            {
                status = "";
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(LineID);
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
        public bool CalculateLineAmount(ref QuotationDet myOrderDetItem)
        {
            int GSTRule = 0;
            //assign GST rules
            if (myOrderDetItem.Item.GSTRule.HasValue)
                GSTRule = myOrderDetItem.Item.GSTRule.Value;

            //If GST has been set on XML file, use the outlet GST rule instead
            if (GSTOverride.GSTRule != 0)
                GSTRule = GSTOverride.GSTRule;


            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.UseSpecialGSTRuleForFormal), false))
                GSTRule = int.Parse(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.SpecialGSTRule));


            if (myOrderDetItem.Item.GSTRule == 3)
            {
                GSTRule = 3;
            }

            if (myOrderDetItem.IsPromo) //calculate value for promo item
            {
                if (myOrderDetItem.UsePromoPrice.HasValue &&
                    myOrderDetItem.UsePromoPrice.Value) //if it is using promo price
                {
                    //Variety GST rules
                    if (GSTRule == 1)
                    {
                        if (myOrderDetItem.Item.GSTRule == 3)
                        {
                            myOrderDetItem.GSTAmount = 0;
                            myOrderDetItem.Amount = Math.Round(myOrderDetItem.Quantity * myOrderDetItem.PromoUnitPrice, 4);
                        }
                        else
                        {
                            //Exclusive GST
                            myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity *
                             (decimal)myOrderDetItem.PromoUnitPrice, 4)
                             * (1 + ((decimal)GST) / 100); //The GST part                       
                            //assign GST amount
                            myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100);
                        }
                    }
                    else
                    {
                        //Inclusive GST
                        myOrderDetItem.PromoAmount =
                            Math.Round(myOrderDetItem.Quantity *
                         (decimal)myOrderDetItem.PromoUnitPrice, 4);


                        if (GSTRule == 2) //Inclusive GST
                        {
                            //calculate the GST formula
                            myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100); ;
                        }
                        else
                        {
                            //non GST, there is no value
                            myOrderDetItem.GSTAmount = 0;
                        }
                    }
                }
                else //for promo by percentage - using percentage discount
                {
                    if (GSTRule == 1)
                    {

                        //Exclusive GST
                        myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity *
                            (decimal)myOrderDetItem.UnitPrice *
                            (decimal)(1 - (myOrderDetItem.PromoDiscount / 100))
                            * (1 + ((decimal)GST) / 100), 4); //The GST part        

                        //GST amount is additional
                        myOrderDetItem.GSTAmount =
                            (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) *
                            ((decimal)GST / 100);
                    }
                    else //Inclusive GST & NO GST
                    {
                        myOrderDetItem.PromoAmount = Math.Round(myOrderDetItem.Quantity *
                            (decimal)myOrderDetItem.UnitPrice *
                            (decimal)(1 - (myOrderDetItem.PromoDiscount / 100)), 4);

                        //inclusive gst and non gst has the same values.
                        if (GSTRule == 2) //Inclusive GST
                        {
                            myOrderDetItem.GSTAmount =
                                (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100); ;
                        }
                        else
                        {
                            //no GST
                            myOrderDetItem.GSTAmount = 0;
                        }
                    }
                }
            }
            else //calculation for non promo item
            {
                if (GSTRule == 1)
                {
                    if (myOrderDetItem.Item.GSTRule == 3)
                    {
                        myOrderDetItem.GSTAmount = 0;
                        myOrderDetItem.Amount = Math.Round(
                           myOrderDetItem.Quantity *
                           myOrderDetItem.UnitPrice *
                           (1 - (myOrderDetItem.Discount / 100)), 4);
                    }
                    else
                    {
                        //Exclusive GST
                        myOrderDetItem.Amount = Math.Round(
                            myOrderDetItem.Quantity *
                            myOrderDetItem.UnitPrice *
                            (1 - (myOrderDetItem.Discount / 100))
                             * (1 + ((decimal)GST) / 100), 4); //The GST part            
                        myOrderDetItem.GSTAmount = (myOrderDetItem.Amount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100);
                    }
                }
                else
                {
                    //Inclusive GST
                    myOrderDetItem.Amount = Math.Round(
                        myOrderDetItem.Quantity *
                        myOrderDetItem.UnitPrice *
                        (1 - (myOrderDetItem.Discount / 100)), 4);

                    if (GSTRule == 2) //Inclusive GST
                    {
                        myOrderDetItem.GSTAmount = myOrderDetItem.Amount / (1 + ((decimal)GST) / 100) * ((decimal)GST / 100); ;
                    }
                    else
                    {
                        myOrderDetItem.GSTAmount = 0;
                    }
                }
            }

            return true;
        }

        public decimal GetGrossAmount()
        {
            return myOrderHdr.GrossAmount;
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
                        items[myOrderDet[i].ItemNo] = (int)items[myOrderDet[i].ItemNo] + myOrderDet[i].Quantity;
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
                if ((int)items[k] <= 0) continue;
                #endregion

                #region *) Validation: Check if quantity sufficient
                string innerStatus = "";
                decimal BalanceQty = InventoryController.GetStockBalanceQtyByItem(k, PointOfSaleInfo.InventoryLocationID, out innerStatus);
                decimal SalesQty = (int)items[k];

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
        public void AssignItemSalesPerson(string LineID, string username)
        {
            string status;
            QuotationDet myDet = GetLine(LineID, out status);
            myDet.Userfld1 = username;
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
                            Amount = CommonUILib.RemoveRoundUp(((decimal)myOrderDet[i].PromoAmount));
                            totalDiscount += Amount - myOrderDet[i].Quantity * CommonUILib.RemoveRoundUp(myOrderDet[i].PromoUnitPrice);
                        }
                        else
                        {
                            //using promo discount
                            totalDiscount += CommonUILib.RemoveRoundUp((myOrderDet[i].Quantity * myOrderDet[i].UnitPrice * (decimal)myOrderDet[i].PromoDiscount / 100));
                        }
                    }
                    else
                    {
                        //using normal non promo (use percentage)
                        totalDiscount += (myOrderDet[i].Quantity * myOrderDet[i].UnitPrice * myOrderDet[i].Discount / 100);
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
        #endregion

        #region Membership
        public bool MembershipApplied()
        {
            return ((CurrentMember != null &&
                CurrentMember.IsLoaded && !CurrentMember.IsNew) | isNewMember);
        }

        //remove membership from receipt
        public bool RemoveMemberFromReceipt()
        {
            //set membership variables
            CurrentMember = null;
            isNewMember = false;
            //void lne item for new signup & renewal if any..
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                //remove new sign up item and renewal item
                if (myOrderDet[i].ItemNo ==
                    MembershipController.MEMBERSHIP_SIGNUP_BARCODE
                    || myOrderDet[i].ItemNo ==
                    MembershipController.RENEWAL_BARCODE)
                {
                    myOrderDet[i].IsVoided = true;
                }
            }
            //apply the discounts
            //promoCtrl.UndoPromoToOrder();
            //promoCtrl.ApplyPromoToOrder();
            ApplyMembershipDiscount();
            return true;
        }

        //return membership information
        public Membership GetMemberInfo()
        {
            if (MembershipApplied())
            {
                return CurrentMember;
            }
            else
            {
                return null;
            }
        }
        //apply membership discount to receipt
        //TODO: assign a different membership discount for product and service
        public void ApplyMembershipDiscount()
        {
            string status = "";
            try
            {
                QuotationDet myOrderDetItem;

                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    myOrderDetItem = myOrderDet[i];
                    //if it is not special
                    if (myOrderDetItem.IsSpecial == false)
                    {
                        //if it is not non discountable
                        if (!myOrderDetItem.Item.IsNonDiscountable)
                        {
                            //if it is not promo
                            if (!myOrderDetItem.IsPromo && !myOrderDetItem.IsSpecial && !myOrderDetItem.IsExchange)
                            {
                                //apply the discount
                                //apply product or service option here
                                // add to load discount from attributes4
                                if (myOrderDetItem.Discount < preferredDiscount)
                                {
                                    myOrderDetItem.Discount = preferredDiscount;
                                }

                                if (MembershipApplied() && myOrderDetItem.Discount < membershipDiscount)
                                {
                                    myOrderDetItem.Discount = membershipDiscount;//(decimal)CurrentMember.MembershipGroup.Discount;
                                }
                            }
                        }
                        else
                        {
                            myOrderDetItem.Discount = 0;
                        }
                    }
                    CalculateLineAmount(ref myOrderDetItem);
                }
                return;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return;
            }
        }

        public bool IsNewMembersRegistered()
        {
            return isNewMember;
        }

        //assign new member that first sign up to the pos
        public bool AssignNewMember
            (string membershipNo, string nametoappear, string nric, string mobileno, string homeno,
             int membershipgroupid, DateTime ExpiryDate,
             string address, string address2, string postalcode,
             string city, string country,
                bool applyPromo, string email)
        {

            //registers the members
            CurrentMember = new Membership();
            CurrentMember.NameToAppear = nametoappear;
            CurrentMember.FirstName = "";
            CurrentMember.LastName = "";
            CurrentMember.ChineseName = "";
            CurrentMember.ChristianName = "";
            CurrentMember.Nric = nric;
            CurrentMember.MembershipGroupId = membershipgroupid;
            CurrentMember.Mobile = mobileno;
            CurrentMember.Home = homeno;
            CurrentMember.ExpiryDate = ExpiryDate;
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.StreetName = address;
            CurrentMember.ZipCode = postalcode;
            CurrentMember.City = city;
            CurrentMember.StreetName2 = address2;
            CurrentMember.Country = country;
            CurrentMember.SubscriptionDate = DateTime.Now;
            CurrentMember.DateOfBirth = DateTime.Now.AddMonths(-2);
            CurrentMember.Email = email;
            isNewMember = true;
            CurrentMember.Deleted = false;
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;

            return true;
        }

        /// <summary>
        /// To Add membership together with detail info. (created by John Harries)
        /// </summary>
        /// <returns>True for success process and false for unsuccess process.</returns>
        public bool AssignNewMember
            (string membershipNo, string nametoappear, string nric, string mobileno, string homeno,
             int membershipgroupid, DateTime ExpiryDate,
             string address, string address2, string postalcode,
             string city, string country,
                bool applyPromo, string email, string firstName, string lastName, string chineseName,
            string christianName, DateTime dateOfBirth, DateTime subscriptionDate, bool isVitaMix, bool isYoung,
            bool isWaterFilter, bool isJuicePlus, string remarks, string occupation, string fax, string office)
        {

            //registers the members
            CurrentMember = new Membership();
            CurrentMember.NameToAppear = nametoappear;
            CurrentMember.FirstName = firstName;
            CurrentMember.LastName = lastName;
            CurrentMember.ChineseName = chineseName;
            CurrentMember.ChristianName = christianName;
            CurrentMember.Nric = nric;
            CurrentMember.MembershipGroupId = membershipgroupid;
            CurrentMember.Mobile = mobileno;
            CurrentMember.Home = homeno;
            CurrentMember.ExpiryDate = ExpiryDate;
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.StreetName = address;
            CurrentMember.ZipCode = postalcode;
            CurrentMember.City = city;
            CurrentMember.StreetName2 = address2;
            CurrentMember.Country = country;
            CurrentMember.SubscriptionDate = subscriptionDate.Date;
            if (DateTime.Compare(dateOfBirth.Date, DateTime.Today.Date) < 0) { CurrentMember.DateOfBirth = dateOfBirth.Date; }
            CurrentMember.Email = email;
            CurrentMember.IsJuicePlus = isJuicePlus;
            CurrentMember.IsVitaMix = isVitaMix;
            CurrentMember.IsWaterFilter = isWaterFilter;
            CurrentMember.IsYoung = isYoung;
            CurrentMember.Remarks = remarks;
            CurrentMember.Occupation = occupation;
            CurrentMember.Fax = fax;
            CurrentMember.Office = office;
            isNewMember = true;
            CurrentMember.Deleted = false;
            CurrentMember.UniqueID = Guid.NewGuid();
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;

            return true;
        }

        /// <summary>
        /// To Add membership together with detail info. (created by John Harries)
        /// </summary>
        /// <returns>True for success process and false for unsuccess process.</returns>
        public bool AssignNewMember
            (string membershipNo, string nametoappear, string nric, string mobileno, string homeno,
             int membershipgroupid, DateTime ExpiryDate,
             string address, string address2, string postalcode,
             string city, string country,
                bool applyPromo, string email, string firstName, string lastName, string chineseName,
            string christianName, DateTime dateOfBirth, DateTime subscriptionDate, bool isVitaMix, bool isYoung,
            bool isWaterFilter, bool isJuicePlus, string remarks, string occupation, string fax, string office, string customfield1name, string customField1value, string customField2name, string customField2value)
        {

            //registers the members
            CurrentMember = new Membership();
            CurrentMember.NameToAppear = nametoappear;
            CurrentMember.FirstName = firstName;
            CurrentMember.LastName = lastName;
            CurrentMember.ChineseName = chineseName;
            CurrentMember.ChristianName = christianName;
            CurrentMember.Nric = nric;
            CurrentMember.MembershipGroupId = membershipgroupid;
            CurrentMember.Mobile = mobileno;
            CurrentMember.Home = homeno;
            CurrentMember.ExpiryDate = ExpiryDate;
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.StreetName = address;
            CurrentMember.ZipCode = postalcode;
            CurrentMember.City = city;
            CurrentMember.StreetName2 = address2;
            CurrentMember.Country = country;
            CurrentMember.SubscriptionDate = subscriptionDate.Date;
            if (DateTime.Compare(dateOfBirth.Date, DateTime.Today.Date) < 0) { CurrentMember.DateOfBirth = dateOfBirth.Date; }
            CurrentMember.Email = email;
            CurrentMember.IsJuicePlus = isJuicePlus;
            CurrentMember.IsVitaMix = isVitaMix;
            CurrentMember.IsWaterFilter = isWaterFilter;
            CurrentMember.IsYoung = isYoung;
            CurrentMember.Remarks = remarks;
            CurrentMember.Occupation = occupation;
            CurrentMember.Fax = fax;
            CurrentMember.Office = office;
            if (!customfield1name.Equals(""))
                CurrentMember.SetColumnValue(customfield1name, customField1value);
            if (!customField2name.Equals(""))
                CurrentMember.SetColumnValue(customField2name, customField2value);

            isNewMember = true;
            CurrentMember.Deleted = false;
            CurrentMember.UniqueID = Guid.NewGuid();
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;

            return true;
        }


        //Load new member from DB
        //Allow member to be assign to CurrentMember record
        //even when members has already expired
        public bool AssignExpiredMember(string membershipno, out string status)
        {
            status = "";
            CurrentMember = new Membership(Membership.Columns.MembershipNo, membershipno);
            if (CurrentMember == null || CurrentMember.ExpiryDate == null)
            {
                status = "Membership number " + membershipno + " does not exist";
                return false;
            }

            status = "";
            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            //Apply Discount....
            //ApplyMembershipDiscount();
            isNewMember = false;
            return true;
        }
        /*
        public bool AssignOtherMembership(string membershipNo)
        {
            CurrentMember = new Membership();
            CurrentMember.MembershipNo = membershipNo;
            CurrentMember.MembershipGroupId = MembershipController.DEFAULT_GROUPID;
            CurrentMember.NameToAppear = "Other";
            CurrentMember.IsLoaded = true;
            CurrentMember.IsNew = false;
            promoCtrl.UndoPromoToOrder(); //UndoCurrentPromo();
            promoCtrl.ApplyPromoToOrder();

            membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
            //Apply Discount....
            //ApplyMembershipDiscount();
            isNewMember = false;
            return true;
        }
        */

        //Assign membership to receipt
        //Load data from DB and assign to local variable CurrentMember
        //reject if expired
        public bool AssignMembership(string membershipno, out string status)
        {
            DateTime ExpiryDate;

            if (MembershipController.CheckMembershipValid
                (membershipno, out CurrentMember, out ExpiryDate))
            {

                status = "";
                membershipDiscount = (decimal)CurrentMember.MembershipGroup.Discount;
                //Apply Discount....
                //ApplyMembershipDiscount();
                isNewMember = false;
                return true;
            }
            else
            {
                if (ExpiryDate != DateTime.MinValue)
                {
                    //Prompt that membership has expired
                    status = "Dear customer, membership has expired on " + ExpiryDate + ". Kindly renew your membership. Thank you.";
                    return false;
                }
                else
                {
                    //Prompt that this is invalid membership number 
                    status = "This is not valid membership number. No membership information is recorded in the system.";
                    return false;
                }
            }
        }
        //record new expiry date upon renewal 
        public bool SetNewExpiryDate(DateTime myExpiryDate)
        {
            newExpiryDate = myExpiryDate;
            return true;
        }
        //record new expiry date upon renewal 
        public bool SetNewMembershipGroupID(int myGroupID)
        {
            newMembershipGroupID = myGroupID;
            return true;
        }
        #endregion

        public decimal CalculateTotalAmount(out string status)
        {
            try
            {
                status = "";
                decimal TotalAmount = 0;

                if (myOrderDet != null)
                {
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

        #region Rounding

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
                return RoundDownNearestFiveCent();
            }
            if (RoundingPreference == "RoundNearestTenCent")
            {
                return RoundNearestTenCent();
            }
            return RoundDownNearestFiveCent(); //default rounding
        }

        public decimal RoundUpNearestTenCent()
        {
            string status;

            decimal TotalAmount = CalculateTotalAmount(out status);

            decimal temp = TotalAmount % 1; //return the 0.xxxx of a component

            decimal d = 0.01M;

            temp = temp * 10;
            temp = temp % 1;
            temp = temp * 10;


            TotalAmount = TotalAmount - temp * d;

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
        
        public decimal RoundDownNearestTenCent()
        {
            string status;

            decimal TotalAmount = CalculateTotalAmount(out status);

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

            decimal TotalAmount = CalculateTotalAmount(out status);

            decimal temp = Math.Round(TotalAmount % 1, 2, MidpointRounding.AwayFromZero); //return the 0.xxxx of a component

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

            decimal TotalAmount = CalculateTotalAmount(out status);

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

        #endregion


        public bool MoveUp(string ID)
        {
            try
            {
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(ID);
                if (tmp.Userint4 != null && tmp.Userint4 > 0)
                {
                    int temp = myOrderDet.Find("Userint4", tmp.Userint4 - 1);
                    if (temp >= 0)
                    {
                        QuotationDet tmp1 = myOrderDet[temp];
                        int abc = (int)tmp.Userint4;
                        tmp.Userint4 = tmp1.Userint4;
                        tmp1.Userint4 = abc;
                    }


                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message); return false;
            }

        }

        public bool MoveDown(string ID)
        {
            try
            {
                QuotationDet tmp = (QuotationDet)myOrderDet.Find(ID);
                if (tmp.Userint4 != null && tmp.Userint4 < myOrderDet.Count)
                {
                    int temp = myOrderDet.Find("Userint4", tmp.Userint4 + 1);
                    if (temp >= 0)
                    {
                        QuotationDet tmp1 = myOrderDet[temp];
                        int abc = (int)tmp.Userint4;
                        tmp.Userint4 = tmp1.Userint4;
                        tmp1.Userint4 = abc;
                    }


                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message); return false;
            }

        }

        public bool ApplyDiscountLevel2(string Discount2, out List<string> status)
        {
            status = new List<string>();
            for (int i = 0; i < myOrderDet.Count; i++)
            {
                QuotationDet myDet = myOrderDet[i];

                #region *) Get discounted price before Discount level 2
                decimal discountedPrice = myDet.UnitPrice - ((myDet.UnitPrice * myDet.Discount) / 100);
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
                decimal discPercent1 = ((RetailPrice - discountedPrice) / (RetailPrice + 0.00001M)) * 100;
                string Discount1 = discPercent1.ToString("N0") + "%";
                if (!string.IsNullOrEmpty(Discount2))
                {
                    if (Discount2.StartsWith("$"))
                    {
                        decimal dollar;
                        decimal.TryParse(Discount2.TrimStart('$'), out dollar);
                        TotalDiscountedPrice -= dollar;

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
                    }
                }
                #endregion

                myDet.DiscountDetail = (Discount1 + "+" + Discount2).TrimEnd('+');
                decimal DiscPercent = ((RetailPrice - TotalDiscountedPrice) / (RetailPrice + 0.00001M)) * 100;

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
    }
}