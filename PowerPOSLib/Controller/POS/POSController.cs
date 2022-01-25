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

namespace PowerPOS
{
    [Serializable]
    public partial class POSController
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

        public const string PAY_PAMEDIFUND = "PAMedifund";

        public const string PAY_SMF = "SMF";

        public const string PAY_PWF = "PWF";

        //public const string PAY_NETS_FLASHPAY = "NetsFlashPay";

        //public const string PAY_NETS_CASHCARD = "CashCard";

        //public const string PAY_NETS_ATMCARD = "NetsCard";

        //public const bool ENABLE_PROGRAMMABLE_KEYBOARD = false;

        public const bool DISCOUNT_BY_PERCENTAGE = true;


        public const string VOUCHER_BARCODE = "VOUCHER";

        #endregion

        private double GST;
        private decimal preferredDiscount;
        public string mode;
        public POSController()
        {
            ResetObject();
        }
        //Creaet new order hdr object
        public void ResetObject()
        {
            try
            {
                //Initialize order hdr
                myOrderHdr = new OrderHdr();
                isNewMember = false;
                LoadGST();
                membershipDiscount = 0;
                myOrderHdr.OrderHdrID = CreateNewOrderNo(PointOfSaleInfo.PointOfSaleID);
                myOrderHdr.OrderRefNo = "OR" + myOrderHdr.OrderHdrID;
                myOrderHdr.CashierID = UserInfo.username;
                myOrderHdr.Discount = 0; //get default values from database
                myOrderHdr.Gst = GST;
                string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
                if (useCustomInvoiceNo != null && useCustomInvoiceNo.ToLower() == "true")
                {
                    myOrderHdr.Userfld5 = CreateNewCustomReceiptNo();  //PointOfSaleInfo.PointOfSaleID
                }
                else
                {
                    myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                }
                /*if (AppSetting.GetSetting("UseCustomInvoiceNo").ToLower() == "true")
                {
                    myOrderHdr.Userfld5 = CreateNewCustomReceiptNo();  //PointOfSaleInfo.PointOfSaleID
                }
                else
                {
                    myOrderHdr.Userfld5 = myOrderHdr.OrderRefNo;
                }*/
                myOrderHdr.OrderDate = DateTime.Now;
                myOrderHdr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID; //get default values from config file

                //Initialize order detail
                myOrderDet = new OrderDetCollection();

                //Create Receipt Hdr
                recHdr = new ReceiptHdr();
                recHdr.ReceiptDate = DateTime.Now;
                recHdr.ReceiptHdrID = myOrderHdr.OrderHdrID;
                recHdr.OrderHdrID = myOrderHdr.OrderHdrID;
                recHdr.ReceiptDate = DateTime.Now;
                recHdr.ReceiptRefNo = "RCP" + recHdr.ReceiptHdrID;
                recHdr.CashierID = UserInfo.username;
                recHdr.PointOfSaleID = myOrderHdr.PointOfSaleID;

                //Create Receipt Det
                recDet = new ReceiptDetCollection();

                promoCtrl = new ApplyPromotionController(this);
                ApplyPromotionController.viewPromoMasterDetailAny = null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public static bool UpdatePurchaseOrderReference(string orderdetid, string purchaseorderrefno)
        {
            try
            {
                OrderDet od = new OrderDet(orderdetid);
                if (od == null || od.OrderDetID == "")
                    return false;

                od.Userfld9 = purchaseorderrefno;
                od.Save(UserInfo.username);
                return true;
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
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

        public static string GetOrderDateByCustomReceiptNo(string receiptNo)
        {
            string orderhdrid = "";

            string sql = "select OrderDate from orderhdr where userfld5 = '" + receiptNo + "'  order by OrderDate desc";

            QueryCommand qr = new QueryCommand(sql);

            object found = DataService.ExecuteScalar(qr);
            if (found != null)
                orderhdrid = Convert.ToDateTime(found).ToString("yyyy-MM-dd HH:mm:ss.fff");

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
        public POSController(string OrderHdrID)
        {
            try
            {
                myOrderHdr = new OrderHdr(OrderHdrID);
                myOrderDet = myOrderHdr.OrderDetRecords();
                recHdr = new ReceiptHdr(ReceiptHdr.Columns.OrderHdrID, myOrderHdr.OrderHdrID);
                recDet = recHdr.ReceiptDetRecords();
                promoCtrl = new ApplyPromotionController(this);

                if (myOrderHdr.MembershipNo != null && myOrderHdr.MembershipNo != "")
                {
                    CurrentMember = new Membership(Membership.Columns.MembershipNo, myOrderHdr.MembershipNo);
                }
                else
                {
                    CurrentMember = new Membership();
                }
                LoadGST();
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
                return "";
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
            IDataReader ds = PowerPOS.SPs.GetNewOrderHdrNoByPointOfSaleID(PointOfSaleID).GetReader();
            while (ds.Read())
            {
                if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
        }

        private string CreateNewOrderNoForOutletSales(int PointOfSaleID, DateTime fDate)
        {
            int runningNo = 0;

            //use stored procedure to pull out the biggest number for today's order
            //format of order: YYMMDDSSSSNNNN
            //This stored procedure pull out the last order number
            string sqlString = "SELECT isnull(max(right(orderhdrid,4)),'0') from orderhdr " +
                    "where left(orderhdrid,6) =  '" + fDate.ToString("yyMMdd") + "' " +
                    "AND convert(int,substring(orderhdrid,7,4)) = " + PointOfSaleID.ToString() + " " +
                    "AND left(right(orderhdrid,4), 1) <> 'W' ";
            IDataReader ds = DataService.GetReader(new QueryCommand(sqlString));
            while (ds.Read())
            {
                if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            return fDate.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
        }

        //Create new order ref no for Web
        public string CreateNewOrderNoForWeb(int PointOfSaleID)
        {
            int runningNo = 0;

            //use stored procedure to pull out the biggest number for today's order
            //format of order: YYMMDDSSSSWNNN
            //This stored procedure pull out the last order number
            IDataReader ds = PowerPOS.SPs.GetNewOrderHdrNoByPointOfSaleIDForWeb(PointOfSaleID).GetReader();
            while (ds.Read())
            {
                if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                {
                    runningNo = 0;
                }
            }
            ds.Close();
            runningNo += 1;

            //YYMMDDSSSSWNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //W - Hardcode for web
            //NNN - Running No
            return DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + "W" + runningNo.ToString().PadLeft(3, '0');
        }

        public string CreateNewCustomReceiptNo()
        {
            string prefixselect = "select AppSettingValue from AppSetting where AppSettingKey='ReceiptPrefix'";
            string prefix = DataService.ExecuteScalar(new QueryCommand(prefixselect)).ToString();

            string suffixselect = "select AppSettingValue from AppSetting where AppSettingKey='ReceiptSuffix'";
            string suffix = DataService.ExecuteScalar(new QueryCommand(suffixselect)).ToString();

            //get current receiptno
            string sql = "select case AppSettingValue when '' then '0' else AppSettingValue end from AppSetting where AppSettingKey='CurrentReceiptNo'";
            QueryCommand Qcmd = new QueryCommand(sql);
            string currentReceiptNo = DataService.ExecuteScalar(Qcmd).ToString();

            //default max receiptno is 4
            string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='MaxReceiptNo'";
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

            return prefix + runningNo.ToString().PadLeft(maxReceiptNo, '0') + suffix;
        }




        public static DateTime FetchLatestCloseCounterTime(int PointOfSaleID)
        {
            Query qr = new Query("CounterCloseLog");
            qr.Top = "1";

            DataSet ds = qr.WHERE(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleID)
                .AND(CounterCloseLog.Columns.EndTime, Comparison.LessOrEquals, DateTime.Now).ORDER_BY("EndTime", "DESC").ExecuteDataSet();

            /*
            Where myWhere = new Where();
            myWhere.ColumnName = CounterCloseLog.Columns.PointOfSaleID;
            myWhere.ParameterName = "@PointOfSaleID";
            myWhere.Comparison = Comparison.Equals;
            myWhere.ParameterValue = PointOfSaleInfo.PointOfSaleID;
            myWhere.TableName = "CounterCloseLog";
            
            Object maxDate = qr.GetMax(CounterCloseLog.Columns.EndTime, myWhere);
            if (maxDate is DateTime) */

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
        //function to void a confirmed receipt
        public static bool VoidReceipt(string orderRefNo, string cashierid, string witnessid, string Remarks)
        {
            try
            {
                string status = "";
                //Load the order hdr based on the reference number
                OrderHdr hdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, orderRefNo);

                //dont allow order to be voided if it is already settled
                if (hdr.OrderDate < FetchLatestCloseCounterTime(PointOfSaleInfo.PointOfSaleID))
                {
                    if (!PrivilegesController.HasPrivilege(PrivilegesController.ALLOW_DO_PAST_VOID, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username)))
                    {
                        return false;
                    }
                }


                //delete from sales commission record
                SalesCommissionRecordCollection str = hdr.SalesCommissionRecordRecords();
                string SalesPersonName = "";
                if (str != null && str.Count > 0)
                    SalesPersonName = str[0].UserMst.UserName + " - VOID";
                else
                    SalesPersonName = "VOID";

                /*
                using (TransactionScope ts = new TransactionScope())
                {*/
                string Status = "";

                #region *) Core: Revert Allocated Point

                #region *) Save Reverted Points In PointTempLog Table That Will Be Deleted When Point Is Synced
                QueryCommandCollection cmdPTL = new QueryCommandCollection();

                // Check in PointAllocationLog first
                PointAllocationLogCollection palColl = new PointAllocationLogCollection();
                palColl.Where(PointAllocationLog.Columns.OrderHdrID, hdr.OrderHdrID);
                palColl.Load();
                if (palColl.Count > 0)
                {
                    foreach (var pal in palColl)
                    {
                        PointTempLog newPTL = new PointTempLog();
                        newPTL.OrderHdrID = pal.OrderHdrID;
                        newPTL.MembershipNo = pal.MembershipNo;
                        newPTL.PointAllocated = -1 * pal.PointAllocated;
                        newPTL.RefNo = pal.Userfld1;

                        Query qry = new Query("MembershipPoints").WHERE("MembershipNo", pal.MembershipNo).AND("userfld1", pal.Userfld1).SetTop("1").SetSelectList("userfld2");
                        object obj = qry.ExecuteScalar();
                        if (obj != null && obj is string)
                            newPTL.PointType = (string)obj;
                        else
                            newPTL.PointType = "D";

                        cmdPTL.Add(newPTL.GetInsertCommand(UserInfo.username));
                    }
                }
                else  // If not exist then check in PointTempLog
                {
                    PointTempLogCollection ptlColl = new PointTempLogCollection();
                    ptlColl.Where(PointTempLog.Columns.OrderHdrID, hdr.OrderHdrID);
                    ptlColl.Load();
                    foreach (var ptl in ptlColl)
                    {
                        PointTempLog newPTL = new PointTempLog();
                        ptl.CopyTo(newPTL);
                        newPTL.PointAllocated = -1 * newPTL.PointAllocated;
                        newPTL.IsNew = true;
                        cmdPTL.Add(newPTL.GetInsertCommand(UserInfo.username));
                    }
                }
                if (cmdPTL.Count > 0) DataService.ExecuteTransaction(cmdPTL);
                #endregion

                if (ConfigurationManager.AppSettings["RealTimePointSystem"] != null && ConfigurationManager.AppSettings["RealTimePointSystem"] != "yes" && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                {
                    if (!PowerPOS.Feature.Package.RevertPackage(hdr.OrderHdrID, DateTime.Now, hdr.MembershipNo, "VOID [" + UserInfo.username + "]", UserInfo.username, out Status))
                    {
                        //if failed prompt message box???? 
                        //TODO: Void should not pop message box. Handle this more gracefully
                        if (Status.StartsWith("(warning)"))
                        {
                            MessageBox.Show(Status.Replace("(warning)", ""), "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if (Status.StartsWith("(error)"))
                        {
                            MessageBox.Show(Status.Replace("(error)", ""), "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            Logger.writeLog(Status);
                            MessageBox.Show(
                                "Some error occurred. Please contact your administrator.", "Error"
                                , MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                }
                #endregion

                //set is voided as true;
                hdr.IsVoided = true;
                hdr.Remark = Remarks;

                //mark ReceiptHDr as is voided
                ReceiptHdrCollection rHdr = hdr.ReceiptHdrRecords();
                if (rHdr != null && rHdr.Count > 0)
                {
                    rHdr[0].IsVoided = true;
                    rHdr[0].Save();
                }
                hdr.Save();

                //for new sign ups
                Query qrUOM = OrderDetUOMConversion.CreateQuery();
                qrUOM.AddWhere(OrderDetUOMConversion.Columns.IsVoided, false);
                qrUOM.AddWhere(OrderDetUOMConversion.Columns.OrderHdrID, hdr.OrderHdrID);
                qrUOM.AddUpdateSetting(OrderDetUOMConversion.Columns.IsVoided, true);
                qrUOM.Execute();

                //Post Void action.....  

                //for new sign ups
                Query qr = OrderDet.CreateQuery();
                qr.AddWhere(OrderDet.Columns.IsVoided, false);
                qr.AddWhere(OrderDet.Columns.ItemNo, MembershipController.MEMBERSHIP_SIGNUP_BARCODE);
                qr.AddWhere(OrderDet.Columns.OrderHdrID, hdr.OrderHdrID);
                qr.SelectList = OrderDet.Columns.OrderDetID;
                DataSet ds =
                    qr.ExecuteDataSet();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //if there is new sign up in the system
                    //delete the new member
                    Membership.Delete(Membership.Columns.MembershipNo, hdr.MembershipNo);

                    Membership DelMember = new Membership(Membership.Columns.MembershipNo, hdr.MembershipNo);
                    DelMember.Deleted = true;
                    DelMember.ModifiedBy = UserInfo.username;
                    DelMember.ModifiedOn = DateTime.Now;
                    //SubSonic.DataService.ExecuteQuery(SyncCommandController.AddCommand("Renew Membership", DelMember.GetSaveCommand(), UserInfo.username));                    
                }

                //Delete redemption log...
                RedemptionLog.Delete(SalesCommissionRecord.Columns.OrderHdrID, hdr.OrderHdrID);

                //Delete commission log
                SalesCommissionRecord.Delete(SalesCommissionRecord.Columns.OrderHdrID, hdr.OrderHdrID);

                //Delete Membership Renewal
                MembershipRenewal.Delete(MembershipRenewal.Columns.OrderHdrId, hdr.OrderHdrID);
                MembershipRenewal DelRenewal = new MembershipRenewal(MembershipRenewal.Columns.OrderHdrId, hdr.OrderHdrID);
                DelRenewal.Deleted = true;
                DelRenewal.ModifiedBy = UserInfo.username;
                DelRenewal.ModifiedOn = DateTime.Now;
                //SubSonic.DataService.ExecuteQuery(SyncCommandController.AddCommand("Renew Membership", DelRenewal.GetSaveCommand(), UserInfo.username));                    

                #region Membership Upgrage
                //Delete Membership Upgrade 
                MembershipUpgradeLog upgrade = new MembershipUpgradeLog(MembershipUpgradeLog.Columns.OrderHdrID, hdr.OrderHdrID);

                //TODO: Remove this as this is only for Billy House
                if (!upgrade.IsNew && upgrade.IsLoaded)
                {
                    upgrade.Deleted = true;

                    qr = Membership.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddUpdateSetting(Membership.Columns.MembershipGroupId, MembershipController.DEFAULT_GROUPID);

                    if (upgrade.IsVitaMixPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsVitaMix, upgrade.IsVitaMixPrevValue.Value);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsVitaMix, false);
                    }
                    if (upgrade.IsJuicePlusPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsJuicePlus, upgrade.IsJuicePlusPrevValue.Value);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsJuicePlus, false);
                    }
                    if (upgrade.IsWaterFilterPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsWaterFilter, upgrade.IsWaterFilterPrevValue.Value);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsWaterFilter, false);
                    }
                    if (upgrade.IsYoungPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsYoung, upgrade.IsYoungPrevValue);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsYoung, false);
                    }
                    qr.AddWhere(Membership.Columns.MembershipNo, hdr.MembershipNo);
                    qr.Execute();
                    upgrade.Save();
                }

                #endregion

                OrderDetCollection dets = hdr.OrderDetRecords();
                ReceiptDetCollection myRcptDet = new ReceiptDetCollection();
                myRcptDet.Where(ReceiptDet.Columns.ReceiptHdrID, hdr.OrderHdrID);
                myRcptDet.Load();

                #region undo installment
                #region oldcode
                //Delete installment record
                //Installment.Delete(Installment.Columns.OrderHdrId, hdr.OrderHdrID);

                ////delete any warranty from this order det

                //for (int i = 0; i < dets.Count; i++)
                //{
                //    //Delete any warranty
                //    Warranty.Delete(Warranty.Columns.OrderDetId, dets[i].OrderDetID);
                //}

                ////undo payment by installment
                //for (int i = 0; i < dets.Count; i++)
                //{
                //    //Delete installment
                //    if (dets[i].ItemNo == InstallmentController.INSTALLMENT_ITEM && !dets[i].IsVoided)
                //    {
                //        //reverse action.... how?
                //        //get the ref no, put back the outstanding amount, change status back to unpaid....
                //        InstallmentDetail inv = new InstallmentDetail(dets[i].VoucherNo);
                //        if (inv.IsLoaded && !inv.IsNew)
                //        {
                //            inv.OutstandingAmount += dets[i].Amount;
                //            if (inv.OutstandingAmount > 0)
                //                inv.Status = InstallmentController.Status.OUTSTANDING;
                //            inv.Save();
                //        }
                //    }
                //    else if (dets[i].ItemNo == DEPOSIT_ITEM && !dets[i].IsVoided)
                //    {
                //        //reverse. adjust tap with the opposite amount
                //        //this is mainly for tap
                //        string status;
                //        decimal remainingTap;
                //        if (!MembershipTapController.adjustTap(hdr.MembershipNo,
                //            -dets[i].Amount, "VOID", out remainingTap, out status))
                //        {
                //            Logger.writeLog("Unable to reverse deposit payment during void. [" + dets[i].OrderDetID + "]" + status);
                //        }
                //    }

                //}
                #endregion

                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    QueryCommandCollection cmdColl = new QueryCommandCollection();

                    for (int i = 0; i < myRcptDet.Count; i++)
                    {
                        if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;

                        // Paid by INSTALLMENT, void the Installment and InstallmentDetail (set Deleted = 1)
                        Installment ins = new Installment("OrderHdrID", hdr.OrderHdrID);
                        if (ins != null && ins.OrderHdrId == hdr.OrderHdrID)
                        {
                            ins.Deleted = true;
                            cmdColl.Add(ins.GetUpdateCommand("SYNC"));
                        }

                        InstallmentDetailCollection insDetColl = ins.InstallmentDetailRecords();
                        foreach (InstallmentDetail insDet in insDetColl)
                        {
                            insDet.Deleted = true;
                            cmdColl.Add(insDet.GetUpdateCommand("SYNC"));
                        }
                    }

                    for (int i = 0; i < dets.Count; i++)
                    {
                        if (dets[i].IsVoided) continue;
                        if (dets[i].ItemNo.ToUpper() != "INST_PAYMENT") continue;
                        if (string.IsNullOrEmpty(dets[i].InstRefNo)) continue;

                        // Installment Payment ==> Update the installment balance and void installment details
                        Installment ins = new Installment(dets[i].InstRefNo);
                        if (ins != null && ins.InstallmentRefNo == dets[i].InstRefNo)
                        {
                            ins.CurrentBalance += dets[i].Amount;
                            cmdColl.Add(ins.GetUpdateCommand("SYNC"));

                            InstallmentDetail insDet = new InstallmentDetail("OrderHdrID", hdr.OrderHdrID);
                            if (insDet != null && insDet.OrderHdrID == hdr.OrderHdrID)
                            {
                                insDet.Deleted = true;
                                cmdColl.Add(insDet.GetUpdateCommand("SYNC"));
                            }
                        }
                    }

                    if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                }
                #endregion

                #region undo voucher
                if (hdr.IsVoided)
                {
                    QueryCommandCollection cmdColl = new QueryCommandCollection();

                    for (int i = 0; i < dets.Count; i++)
                    {
                        if (dets[i].IsVoided) continue;
                        if (dets[i].ItemNo == POSController.VOUCHER_BARCODE)
                        {
                            //update voucher id....
                            Query qr2 = Voucher.CreateQuery();
                            qr2.QueryType = QueryType.Update;
                            qr2.AddWhere(Voucher.Columns.VoucherNo, dets[i].VoucherNo);
                            if (dets[i].Quantity > 0)
                            {
                                qr2.AddWhere(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                qr2.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_PRINTED);
                                qr2.AddUpdateSetting(Voucher.Columns.DateSold, DBNull.Value);
                                qr2.AddUpdateSetting(Voucher.Columns.ModifiedOn, DateTime.Now);
                                qr2.AddUpdateSetting(Voucher.Columns.ModifiedBy, "SYNC");
                                cmdColl.Add(qr2.BuildUpdateCommand());

                                // Update SoldQuantity in VoucherHeader
                                string sql = @"
                                                    UPDATE VoucherHeader 
                                                    SET SoldQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = vh.VoucherHeaderID AND DateSold IS NOT NULL), 
                                                        ModifiedOn = GETDATE()
                                                    FROM VoucherHeader vh 
                                                        INNER JOIN Vouchers vs ON vs.VoucherHeaderID = vh.VoucherHeaderID
                                                    WHERE vs.VoucherNo = '{0}'
                                                  ";
                                sql = string.Format(sql, dets[i].VoucherNo);
                                cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                            }
                            else
                            {
                                qr2.AddWhere(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_REDEEMED);
                                qr2.AddUpdateSetting(Voucher.Columns.VoucherStatusId, VoucherController.VOUCHER_SOLD);
                                qr2.AddUpdateSetting(Voucher.Columns.DateRedeemed, DBNull.Value);
                                qr2.AddUpdateSetting(Voucher.Columns.ModifiedOn, DateTime.Now);
                                qr2.AddUpdateSetting(Voucher.Columns.ModifiedBy, "SYNC");
                                cmdColl.Add(qr2.BuildUpdateCommand());

                                // Update RedeemedQuantity in VoucherHeader
                                string sql = @"
                                                    UPDATE VoucherHeader 
                                                    SET RedeemedQuantity = (SELECT COUNT(*) FROM Vouchers WHERE VoucherHeaderID = vh.VoucherHeaderID AND DateRedeemed IS NOT NULL), 
                                                        ModifiedOn = GETDATE()
                                                    FROM VoucherHeader vh 
                                                        INNER JOIN Vouchers vs ON vs.VoucherHeaderID = vh.VoucherHeaderID
                                                    WHERE vs.VoucherNo = '{0}'
                                                  ";
                                sql = string.Format(sql, dets[i].VoucherNo);
                                cmdColl.Add(new QueryCommand(sql, "PowerPOS"));
                            }
                        }
                    }

                    if (cmdColl.Count > 0) DataService.ExecuteTransaction(cmdColl);
                }

                #endregion

                //undo stock out
                //if there is any inventory ref number, undo it
                //for (int i = 0; i < dets.Count; i++)
                //{
                //    if (dets[i].IsVoided == false &&
                //        dets[i].InventoryHdrRefNo != null &&
                //        dets[i].InventoryHdrRefNo != "")
                //    {

                //        InventoryController.UndoStockOutCreateNewInv(dets[i].InventoryHdrRefNo);
                //    }
                //}

                #region Undo Inventory
                //Adi fix 20170626 - add the validation only run if it was localhost
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    InventoryController.AssignStockOutToConfirmedOrderUsingTransaction(false);
                }
                #endregion


                //record the void activity into the log
                VoidLogController vd = new VoidLogController();
                vd.Insert(hdr.OrderHdrID, hdr.PointOfSaleID, cashierid, witnessid, true, null, "", DateTime.Now, ""
                    , "", "", "", "", "", "", "", "", "", "", null, null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null);
                //ts.Complete();
                //}                
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
                //MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool VoidReceiptServer(string orderRefNo, string cashierid, string witnessid, string Remarks, out string Status)
        {
            try
            {
                //Load the order hdr based on the reference number
                OrderHdr hdr = new OrderHdr(OrderHdr.Columns.OrderRefNo, orderRefNo);

                //dont allow order to be voided if it is already settled
                if (hdr.OrderDate < FetchLatestCloseCounterTime(PointOfSaleInfo.PointOfSaleID))
                {
                    Status = "Order is already closed. cannot void";
                    return false;
                }
                if (cashierid == null)
                    cashierid = "Ecommerce";
                if (witnessid == null)
                    witnessid = "";
                //delete from sales commission record
                SalesCommissionRecordCollection str = hdr.SalesCommissionRecordRecords();
                string SalesPersonName = "";
                if (str != null && str.Count > 0)
                    SalesPersonName = str[0].UserMst.UserName + " - VOID";
                else
                    SalesPersonName = "VOID";

                /*
                using (TransactionScope ts = new TransactionScope())
                {*/
                Status = "";

                #region *) Core: Revert Allocated Point

                //undo package usage if the order is voided
                if (!PowerPOS.Feature.Package.RevertPackageServer(hdr.OrderHdrID, DateTime.Now, hdr.MembershipNo, "VOID [" + UserInfo.username + "]", UserInfo.username, out Status))
                {
                    //if failed prompt message box???? 
                    //TODO: Void should not pop message box. Handle this more gracefully
                    Logger.writeLog(Status);
                    return false;
                }
                #endregion

                //set is voided as true;
                hdr.IsVoided = true;
                hdr.Remark = Remarks;

                //mark ReceiptHDr as is voided
                ReceiptHdrCollection rHdr = hdr.ReceiptHdrRecords();
                if (rHdr != null && rHdr.Count > 0)
                {
                    rHdr[0].IsVoided = true;
                    rHdr[0].Save();
                }
                hdr.Save();

                //Post Void action.....  

                //for new sign ups
                //Delete commission log
                SalesCommissionRecord.Delete(SalesCommissionRecord.Columns.OrderHdrID, hdr.OrderHdrID);

                //Delete Membership Renewal
                /*MembershipRenewal.Delete(MembershipRenewal.Columns.OrderHdrId, hdr.OrderHdrID);
                MembershipRenewal DelRenewal = new MembershipRenewal(MembershipRenewal.Columns.OrderHdrId, hdr.OrderHdrID);
                DelRenewal.Deleted = true;
                DelRenewal.ModifiedBy = UserInfo.username;
                DelRenewal.ModifiedOn = DateTime.Now;*/
                //SubSonic.DataService.ExecuteQuery(SyncCommandController.AddCommand("Renew Membership", DelRenewal.GetSaveCommand(), UserInfo.username));                    

                //Delete Membership Upgrade 
                //MembershipUpgradeLog upgrade = new MembershipUpgradeLog(MembershipUpgradeLog.Columns.OrderHdrID, hdr.OrderHdrID);

                //TODO: Remove this as this is only for Billy House
                /*if (!upgrade.IsNew && upgrade.IsLoaded)
                {
                    upgrade.Deleted = true;

                    qr = Membership.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddUpdateSetting(Membership.Columns.MembershipGroupId, MembershipController.DEFAULT_GROUPID);

                    if (upgrade.IsVitaMixPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsVitaMix, upgrade.IsVitaMixPrevValue.Value);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsVitaMix, false);
                    }
                    if (upgrade.IsJuicePlusPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsJuicePlus, upgrade.IsJuicePlusPrevValue.Value);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsJuicePlus, false);
                    }
                    if (upgrade.IsWaterFilterPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsWaterFilter, upgrade.IsWaterFilterPrevValue.Value);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsWaterFilter, false);
                    }
                    if (upgrade.IsYoungPrevValue.HasValue)
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsYoung, upgrade.IsYoungPrevValue);
                    }
                    else
                    {
                        qr.AddUpdateSetting(Membership.Columns.IsYoung, false);
                    }
                    qr.AddWhere(Membership.Columns.MembershipNo, hdr.MembershipNo);
                    qr.Execute();
                    upgrade.Save();
                }*/

                //Delete installment record
                //Installment.Delete(Installment.Columns.OrderHdrId, hdr.OrderHdrID);

                //delete any warranty from this order det
                /*OrderDetCollection dets = hdr.OrderDetRecords();
                for (int i = 0; i < dets.Count; i++)
                {
                    //Delete any warranty
                    Warranty.Delete(Warranty.Columns.OrderDetId, dets[i].OrderDetID);
                }*/

                //undo payment by installment
                /*for (int i = 0; i < dets.Count; i++)
                {
                    //Delete installment
                    if (dets[i].ItemNo == InstallmentController.INSTALLMENT_ITEM && !dets[i].IsVoided)
                    {
                        //reverse action.... how?
                        //get the ref no, put back the outstanding amount, change status back to unpaid....
                        InstallmentDetail inv = new InstallmentDetail(dets[i].VoucherNo);
                        if (inv.IsLoaded && !inv.IsNew)
                        {
                            inv.OutstandingAmount += dets[i].Amount;
                            if (inv.OutstandingAmount > 0)
                                inv.Status = InstallmentController.Status.OUTSTANDING;
                            inv.Save();
                        }
                    }
                    else if (dets[i].ItemNo == DEPOSIT_ITEM && !dets[i].IsVoided)
                    {
                        //reverse. adjust tap with the opposite amount
                        //this is mainly for tap
                        string status;
                        decimal remainingTap;
                        if (!MembershipTapController.adjustTap(hdr.MembershipNo,
                            -dets[i].Amount, "VOID", out remainingTap, out status))
                        {
                            Logger.writeLog("Unable to reverse deposit payment during void. [" + dets[i].OrderDetID + "]" + status);
                        }
                    }

                }*/


                //if (PointOfSaleInfo.IntegrateWithInventory)


                //undo stock out
                //if there is any inventory ref number, undo it
                OrderDetCollection dets = hdr.OrderDetRecords();
                for (int i = 0; i < dets.Count; i++)
                {
                    if (dets[i].IsVoided == false &&
                        dets[i].InventoryHdrRefNo != null &&
                        dets[i].InventoryHdrRefNo != "")
                    {

                        InventoryController.UndoStockOutCreateNewInv(dets[i].InventoryHdrRefNo);
                    }
                }


                //record the void activity into the log
                VoidLogController vd = new VoidLogController();
                vd.Insert(hdr.OrderHdrID, hdr.PointOfSaleID, cashierid, witnessid, true, null, "", DateTime.Now, ""
                    , "", "", "", "", "", "", "", "", "", "", null, null, null, null, null, null, null, null, null, null,
                    null, null, null, null, null);
                //ts.Complete();
                //}                
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Status = "Error on void Receipt";
                return false;
                //MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        public void AppendOrderDet(OrderDetCollection orderDetCollection)
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

        public DataTable getInventoryItemsForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Quantity", Type.GetType("System.Int32"));
            dt.Columns.Add("CostPrice", Type.GetType("System.Decimal"));
            foreach (OrderDet od in myOrderDet)
            {
                if (od.Item.IsInInventory)
                {
                    DataRow dr = dt.NewRow();
                    dr["ItemNo"] = od.ItemNo;
                    dr["ItemName"] = od.Item.ItemName;
                    dr["Quantity"] = od.Quantity;
                    dr["CostPrice"] = Math.Round(od.Amount / od.Quantity.GetValueOrDefault(0), 2);
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static bool isRefunded(string orderdetId, out decimal qty)
        {
            bool result = false;
            qty = 0;
            try
            {
                string sqlString = "Select sum(quantity) from OrderDet where ISNULL(userfld13, '') = '" +
                    orderdetId + "' and orderhdrid in (select orderhdrid from orderhdr where isvoided = 0) ";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                if (decimal.TryParse(tmp.ToString(), out qty))
                {
                    qty = -1 * qty;
                    if (qty > 0)
                    {
                        result = true;
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.writeLog("Error during checking is refunded." + ex.Message);
            }
            return result;
        }

        public bool IsFormalInvoice()
        {
            try
            {
                foreach (var d in recDet)
                {
                    if (d.PaymentType.ToUpper() != PAY_INSTALLMENT)
                        return false;

                    if (d.Userfld1.ToUpper() == "C.O.D")
                        return true;
                    else if (d.Userfld1.ToUpper() == "7 DAYS CREDIT")
                        return true;
                    else if (d.Userfld1.ToUpper() == "15 DAYS CREDIT")
                        return true;
                    else if (d.Userfld1.ToUpper() == "30 DAYS CREDIT")
                        return true;
                    else if (d.Userfld1.ToUpper() == "45 DAYS CREDIT")
                        return true;
                    else if (d.Userfld1.ToUpper() == "60 DAYS CREDIT")
                        return true;
                    else if (d.Userfld1.ToUpper() == "15 DAYS FROM MONTH END")
                        return true;
                    else if (d.Userfld1.ToUpper() == "30 DAYS FROM MONTH END")
                        return true;
                    else if (d.Userfld1.ToUpper() == "45 DAYS FROM MONTH END")
                        return true;
                    else if (d.Userfld1.ToUpper() == "60 DAYS FROM MONTH END")
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
    }
}