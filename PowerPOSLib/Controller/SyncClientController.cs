using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;
using System.Collections;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Transactions;
using PowerPOSLib.Magento;
using System.Linq;
using PowerPOS.InventoryRealTimeController;
using System.Web.Script.Serialization;

namespace PowerPOS
{
    public class SyncClientController
    {
        public const string XMLFILENAME = "\\WS.XML";
        public static string WS_URL;
        private const string ChangeTrackingLastVersionName = "ChangeTrackingLastVersion";
        static string providerName = "PowerPOS";

        public static bool Load_WS_URL()
        {
            try
            {
                //if it does not exist in database, load from text file
                //this is for backward compatibility 
                if (AppSetting.GetSetting("WS_URL") != null)
                {
                    WS_URL = AppSetting.GetSetting("WS_URL").ToString();
                    return true;
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                    WS_URL = ds.Tables[0].Rows[0]["URL"].ToString();
                    return true;
                }
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("Load_WS_URL");
                Logger.writeLog(ex);
                return false;
            }
        }

        #region "Sending - From Client to Server"

        private static DataTable FetchOrderHdrNotInServer
            (DateTime StartDate, DateTime EndDate,
            int pointOfSaleID, string[][] excludedOrderHdrIDList)
        {
            try
            {
                string excludeList = "";

                for (int i = 0; i < excludedOrderHdrIDList.Length; i++)
                {
                    excludeList += "'" + excludedOrderHdrIDList[i][0] + "',";
                }
                if (excludeList.Length > 1)
                {
                    excludeList = excludeList.Remove(excludeList.Length - 1); //delete last character
                    DataSet ds = SPs.GetOrderHdrNotInOrderList(StartDate, EndDate, pointOfSaleID, excludeList).GetDataSet();

                    return ds.Tables[0];
                }
                else
                {
                    //No excluded List, just load all from table
                    OrderHdrCollection myHdr = new OrderHdrCollection();
                    myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, StartDate);
                    myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, EndDate);
                    myHdr.Where(OrderHdr.Columns.PointOfSaleID, pointOfSaleID);
                    myHdr.Load();

                    return myHdr.ToDataTable();

                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Order List from server:" + ex.Message, true);
                return null;
            }
        }

        private static DataTable FetchOrderHdrNotInServerWithoutPOSID
            (DateTime StartDate, DateTime EndDate,
            string[][] excludedOrderHdrIDList)
        {
            try
            {
                string excludeList = "";

                for (int i = 0; i < excludedOrderHdrIDList.Length; i++)
                {
                    excludeList += "'" + excludedOrderHdrIDList[i][0] + "',";
                }
                if (excludeList.Length > 1)
                {
                    excludeList = excludeList.Remove(excludeList.Length - 1); //delete last character
                    DataSet ds = SPs.GetOrderHdrNotInOrderListWithoutPOSID(StartDate, EndDate, excludeList).GetDataSet();

                    return ds.Tables[0];

                }
                else
                {
                    //No excluded List, just load all from table
                    OrderHdrCollection myHdr = new OrderHdrCollection();
                    myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, StartDate);
                    myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, EndDate);
                    myHdr.Load();

                    return myHdr.ToDataTable();

                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Order List from server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchOrderHdrNotInServerWithPOSID
            (DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "SELECT TOP " + numofRecords + " * FROM OrderHdr WHERE ModifiedOn > '" +
                                    StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND PointOfSaleID = " + PointOfSaleID.ToString() +
                                    "ORDER BY ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Order List from server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchQuotationHdrNotInServerWithPOSID
            (DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "SELECT TOP " + numofRecords + " * FROM QuotationHdr WHERE ModifiedOn > '" +
                                    StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND PointOfSaleID = " + PointOfSaleID.ToString() +
                                    "ORDER BY ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Order List from server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchCashRecordingNotInServerWithPOSID(DateTime StartDate, int numofRecords, int PointOfSaleID)
        {
            try
            {
                string sqlString = "SELECT TOP " + numofRecords + " * FROM CashRecording WHERE ModifiedOn > '" +
                                    StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' AND PointOfSaleID = " + PointOfSaleID.ToString() +
                                    "ORDER BY ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get CashRecordingt from server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchAccessLogNotInServerWithPOSID(DateTime startDate, int numOfRecord, int pointOfSaleID)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = @"SELECT	*
                                FROM	AccessLog 
                                WHERE	PointOfSaleID = {0}
		                                AND AccessSource = 'POS'
                                        AND ModifiedOn > '{1}'
                                ORDER BY ModifiedOn ASC";
                sql = string.Format(sql, pointOfSaleID, startDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchAttendanceNotInServerWithPOSID(DateTime startDate, int numOfRecord, int pointOfSaleID)
        {
            DataTable dt = new DataTable();

            try
            {
                string sql = @"SELECT	*
                                FROM	MembershipAttendance 
                                WHERE	PointOfSaleID = {0}
		                                AND ModifiedOn > '{1}'
                                ORDER BY ModifiedOn ASC";
                sql = string.Format(sql, pointOfSaleID, startDate.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchCounterCloseLogWithPOSID
            (DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "Select Top " + numofRecords + " * from CounterCloseLog where ModifiedOn > '" +
                                    StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and pointofsaleid = " + PointOfSaleID.ToString() +
                                    "Order By ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

                return dt;


            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Order List from server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchLoginActivityWithPOSID
            (DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "Select Top " + numofRecords + " * from LoginActivity where ModifiedOn > '" +
                                    StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and pointofsaleid = " + PointOfSaleID.ToString() +
                                    "Order By ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];

                return dt;


            }
            catch (Exception ex)
            {
                Logger.writeLog("Get LoginActivity from server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchDeliveryOrderNotInServerWithPOSID(DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "SELECT TOP " + numofRecords + " do.* FROM DeliveryOrder do INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo where ISNULL(do.IsServerUpdate, 0) = 1 AND oh.PointOfSaleID = " + PointOfSaleID.ToString() +
                                   " ORDER BY do.ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString, "PowerPOS")).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get Delivery Order List Not In Server:" + ex.Message, true);
                return null;
            }
        }

        public static DataTable FetchTableDataNotInServerWithPOSID
            (string TableName, DateTime StartDate, int numofRecords,
            int PointOfSaleID)
        {
            try
            {
                string sqlString = "Select Top " + numofRecords + " * from " + TableName + " where ModifiedOn > '" +
                                    StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "' and pointofsaleid = " + PointOfSaleID.ToString() +
                                    "Order By ModifiedOn ASC ";
                DataTable dt = DataService.GetDataSet(new QueryCommand(sqlString)).Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Get " + TableName + " from server:" + ex.Message, true);
                return null;
            }
        }

        public static void deductInventory()
        {
            try
            {
                //Call web Service....
                PowerPOSLib.PowerPOSSync.Synchronization ws =
                    new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 1000000;
                ws.Url = WS_URL;

                //deduct inventory
                //ws.Timeout = 500000;
                ws.AssignStockOutToConfirmedOrderUsingTransactionScope();
                //ws.AssignStockOutToConfirmedOrderUsingTransactionScope();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public static bool SendOrderCCMW
            (DateTime StartDate, DateTime EndDate)
        {
            try
            {
                //make sure end time is less than the latest closing time
                //Assume POS ID is populated.... For integrated sync with POS, it will be so
                //For separate sync, they need to call GetPointOfSaleInfo();
                DateTime dtLastClosing = ClosingController.FetchLastClosingTime(PointOfSaleInfo.PointOfSaleID);
                if (EndDate > dtLastClosing)
                    EndDate = dtLastClosing;

                if (!SendNewSignUp(StartDate, EndDate))
                {
                    return false;
                }

                //Fetch list of order number and GUID of the specific period of time from SERVER
                //Call web Service....
                PowerPOSLib.PowerPOSSync.Synchronization ws =
                    new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 1000000;
                ws.Url = WS_URL;

                string[][] orderHdrIDList = ws.GetOrderHdrListWithoutPOSID(StartDate, EndDate);

                //compare the list and exclude the order number that is in the server already

                OrderHdrCollection myHdr = new OrderHdrCollection();
                OrderDetCollection myDet = new OrderDetCollection();

                ReceiptHdrCollection myRcptHdr = new ReceiptHdrCollection();
                ReceiptDetCollection myRcptDet = new ReceiptDetCollection();

                /*------------------------------Apply Date Filtering----------------------------------
                myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, StartDate);
                myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, EndDate);
                myHdr.Where(OrderHdr.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                myHdr.Load();*/
                DataTable dt = SyncClientController.FetchOrderHdrNotInServerWithoutPOSID
                    (StartDate, EndDate, orderHdrIDList);
                if (dt != null)
                {
                    myHdr.Load(dt);

                    //take care of GUID load conversion
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        myHdr[i].UniqueID = new Guid(dt.Rows[i]["UniqueID"].ToString());
                    }

                    for (int i = 0; i < myHdr.Count; i++)
                        myDet.AddRange(myHdr[i].OrderDetRecords());

                    for (int i = 0; i < myHdr.Count; i++)
                        myRcptHdr.AddRange(myHdr[i].ReceiptHdrRecords());

                    /*
                    myRcptHdr.Where(ReceiptHdr.Columns.ReceiptDate, Comparison.GreaterOrEquals, StartDate);
                    myRcptHdr.Where(ReceiptHdr.Columns.ReceiptDate, Comparison.LessOrEquals, EndDate);
                    myRcptHdr.Load();
                    */

                    for (int i = 0; i < myRcptHdr.Count; i++)
                        myRcptDet.AddRange(myRcptHdr[i].ReceiptDetRecords());
                    //-----------------------------------------------------------------------------------------------                
                    QueryParameterCollection param;

                    string[][] objHdr = new string[myHdr.Count][];
                    if (myHdr.Count > 0)
                    {
                        for (int op = 0; op < myHdr.Count; op++)
                        {
                            param = myHdr[op].GetInsertCommand("").Parameters;

                            string[] objArr = new string[param.Count];
                            for (int oT = 0; oT < param.Count; oT++)
                            {
                                if (param[oT].ParameterName.ToLower() != "@createdon" &
                                       param[oT].ParameterName.ToLower() != "@createdby" &
                                        param[oT].ParameterName.ToLower() != "@modifiedon" &
                                        param[oT].ParameterName.ToLower() != "@modifiedby")
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                            }
                            objHdr[op] = objArr;
                        }
                    }


                    string[][] objDet = new string[myDet.Count][];
                    if (myDet.Count > 0)
                    {

                        for (int op = 0; op < myDet.Count; op++)
                        {
                            param = myDet[op].GetInsertCommand("").Parameters;

                            string[] objArr = new string[param.Count];
                            for (int oT = 0; oT < param.Count; oT++)
                            {
                                if (param[oT].ParameterName.ToLower() != "@createdon" &
                                       param[oT].ParameterName.ToLower() != "@createdby" &
                                        param[oT].ParameterName.ToLower() != "@modifiedon" &
                                        param[oT].ParameterName.ToLower() != "@modifiedby")
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                            objDet[op] = objArr;
                        }
                    }

                    string[][] objReceiptHdr = new string[myRcptHdr.Count][];
                    if (myRcptHdr.Count > 0)
                    {

                        for (int op = 0; op < myRcptHdr.Count; op++)
                        {
                            param = myRcptHdr[op].GetInsertCommand("").Parameters;
                            string[] objArr = new string[param.Count];
                            for (int oT = 0; oT < param.Count; oT++)
                            {
                                if (param[oT].ParameterName.ToLower() != "@createdon" &
                                       param[oT].ParameterName.ToLower() != "@createdby" &
                                        param[oT].ParameterName.ToLower() != "@modifiedon" &
                                        param[oT].ParameterName.ToLower() != "@modifiedby")
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                            }
                            objReceiptHdr[op] = objArr;
                        }
                    }

                    string[][] objReceiptDet = new string[myRcptDet.Count][];
                    if (myRcptDet.Count > 0)
                    {

                        for (int op = 0; op < myRcptDet.Count; op++)
                        {
                            param = myRcptDet[op].GetInsertCommand("").Parameters;
                            string[] objArr = new string[param.Count];
                            for (int oT = 0; oT < param.Count; oT++)
                            {
                                if (param[oT].ParameterName.ToLower() != "@createdon" &
                                       param[oT].ParameterName.ToLower() != "@createdby" &
                                        param[oT].ParameterName.ToLower() != "@modifiedon" &
                                        param[oT].ParameterName.ToLower() != "@modifiedby")
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                            }
                            objReceiptDet[op] = objArr;
                        }
                    }
                    Logger.writeLog("Start Sending Orders");
                    bool result = ws.FetchOrdersCCMW(objHdr, objDet, objReceiptHdr, objReceiptDet);
                    Logger.writeLog("Sending Orders completed. Result = " + result);


                    if (result)
                    {
                        DataSet ds = new DataSet();
                        ds.Tables.Add(new SalesCommissionRecordCollection().
                            Where("ModifiedOn", Comparison.GreaterThan, StartDate).
                            Where("ModifiedOn", Comparison.LessOrEquals, EndDate).
                            Load().ToDataTable());
                        setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                        result = ws.FetchLogTable(ds, "SalesCommissionRecord");
                        Logger.writeLog("Sending SalesCommissionRecord result = " + result);


                    }
                    return result;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Order failed." + ex.Message, true);
                return false;
            }
        }

        public static bool SendOrder()
        {
            try
            {
                OrderHdrCollection myHdr = new OrderHdrCollection();
                OrderDetCollection myDet = new OrderDetCollection();
                SalesCommissionRecordCollection myComm = new SalesCommissionRecordCollection();
                ReceiptHdrCollection myRcptHdr = new ReceiptHdrCollection();
                ReceiptDetCollection myRcptDet = new ReceiptDetCollection();

                //------------------------------Apply Date Filtering----------------------------------
                myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.GreaterOrEquals, DateTime.Today.AddHours(-48));
                myHdr.Where(OrderHdr.Columns.OrderDate, Comparison.LessOrEquals, DateTime.Today);
                myHdr.Load();
                for (int i = 0; i < myHdr.Count; i++)
                    myDet.AddRange(myHdr[i].OrderDetRecords());

                for (int i = 0; i < myHdr.Count; i++)
                    myComm.AddRange(myHdr[i].SalesCommissionRecordRecords());

                myRcptHdr.Where(ReceiptHdr.Columns.ReceiptDate, Comparison.GreaterOrEquals, DateTime.Today.AddHours(-48));
                myRcptHdr.Where(ReceiptHdr.Columns.ReceiptDate, Comparison.LessOrEquals, DateTime.Today);
                myRcptHdr.Load();

                for (int i = 0; i < myRcptHdr.Count; i++)
                    myRcptDet.AddRange(myRcptHdr[i].ReceiptDetRecords());

                //-----------------------------------------------------------------------------------------------                
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DataSet dsHdr = new DataSet();
                DataTable dtHdr = myHdr.ToDataTable();
                dtHdr.TableName = "OrderHeader";
                dsHdr.Tables.Add(dtHdr);

                DataSet dsDet = new DataSet();
                DataTable dtDet = myDet.ToDataTable();
                dtDet.TableName = "OrderDetail";
                dsDet.Tables.Add(dtDet);

                DataSet dsRcptHdr = new DataSet();
                DataTable dtRcptHdr = myRcptHdr.ToDataTable();
                dtRcptHdr.TableName = "ReceiptHdr";
                dsRcptHdr.Tables.Add(dtRcptHdr);


                DataSet dsRcptDet = new DataSet();
                DataTable dtRcptDet = myRcptDet.ToDataTable();
                dtRcptDet.TableName = "ReceiptDet";
                dsRcptDet.Tables.Add(dtRcptDet);

                DataSet dsComm = new DataSet();
                DataTable dtComm = myComm.ToDataTable();
                dtComm.TableName = "SalesCommissionStructure";
                dsComm.Tables.Add(dtComm);
                Logger.writeLog("Start Sending Orders");
                bool result = ws.FetchOrders(dsHdr, dsDet, dsRcptHdr, dsRcptDet, dsComm);
                Logger.writeLog("Sending Orders completed. Result = " + result);

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Order failed.");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool SendNewSignUpWithoutOrder(DateTime startDate, DateTime endDate)
        {
            return true;
            /*
            try
            {
                MembershipCollection myMembers = new MembershipCollection();
                myMembers.Where(Membership.Columns.CreatedOn, Comparison.GreaterOrEquals, startDate);
                myMembers.Where(Membership.Columns.CreatedOn, Comparison.LessOrEquals, endDate);
                myMembers.Load();

                ArrayList particularList = new ArrayList();
                foreach (Membership m in myMembers.Where(o => o.Userflag5.GetValueOrDefault(false) == true))
                {
                    m.Userflag5 = false;
                    particularList.Add(m.MembershipNo);
                }

                AttachedParticularCollection myAPs = new AttachedParticularCollection();
                myAPs.Where(AttachedParticular.Columns.MembershipNo, Comparison.In, particularList);
                myAPs.Where(AttachedParticular.Columns.CreatedOn, Comparison.GreaterOrEquals, startDate);
                myAPs.Where(AttachedParticular.Columns.CreatedOn, Comparison.LessOrEquals, endDate);
                myAPs.Load();

                PowerPOSLib.PowerPOSSync.Synchronization ws
                    = new PowerPOSLib.PowerPOSSync.Synchronization();

                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DataSet dsMember = new DataSet();
                DataTable dtMember = myMembers.ToDataTable();
                DataTable dtAP = myAPs.ToDataTable();
                setAllDateTimeColumnToBeLocal(dtMember);
                setAllDateTimeColumnToBeLocal(dtAP);
                dtMember.TableName = "Membership";
                dtAP.TableName = "AttachedParticular";
                dsMember.Tables.Add(dtMember);
                dsMember.Tables.Add(dtAP);

                Logger.writeLog("Start Sending New Membership Signups");
                bool result =
                    ws.FetchNewMembershipSignUps(dsMember);

                myMembers = new MembershipCollection();
                myMembers.Where(Membership.Columns.CreatedOn, Comparison.GreaterOrEquals, startDate);
                myMembers.Where(Membership.Columns.CreatedOn, Comparison.LessOrEquals, endDate);
                myMembers.Load();
                foreach (Membership mp in myMembers.Where(o => o.Userflag5.GetValueOrDefault(false) == true))
                {
                    Membership m = new Membership(Membership.Columns.MembershipNo, mp.MembershipNo);
                    if (m.Userflag5.GetValueOrDefault(false) == true && result)
                        m.Userflag5 = false;
                    m.Save(UserInfo.username);
                }

                Logger.writeLog
                    ("Sending New Membership signup completed. Result = " + result);

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send New Membership signup failed.");
                Logger.writeLog(ex);
                return false;
            }
             * */
        }

        public static bool SendNewSignUpWithoutOrder()
        {
            return true;
            //return SendNewSignUpWithoutOrder(DateTime.Now.AddYears(-100), DateTime.Now.AddYears(100));
        }

        public static bool SendNewSignUp(DateTime startDate, DateTime endDate)
        {
            try
            {
                bool result = SendNewSignUpWithoutOrder(startDate, endDate);

                DateTime lastClosingTime = ClosingController.FetchLastClosingTime(PointOfSaleInfo.PointOfSaleID);

                if (endDate > lastClosingTime)
                    endDate = lastClosingTime;

                Query qr = ViewTransactionWithMembership.CreateQuery();
                qr.QueryType = QueryType.Select;
                qr.SelectList = "MembershipNo";
                qr.AddWhere(ViewTransactionWithMembership.Columns.OrderDate, Comparison.GreaterOrEquals, startDate);
                qr.AddWhere(ViewTransactionWithMembership.Columns.OrderDate, Comparison.LessOrEquals, endDate);
                qr.AddWhere(ViewTransactionWithMembership.Columns.ItemNo, "MEMBER");
                DataSet dsList = qr.ExecuteDataSet();
                ArrayList membershipList = new ArrayList();
                for (int k = 0; k < dsList.Tables[0].Rows.Count; k++)
                {
                    membershipList.Add(dsList.Tables[0].Rows[k][0].ToString());
                }

                MembershipCollection myMembers = new MembershipCollection();
                myMembers.Where(Membership.Columns.MembershipNo, Comparison.In, membershipList);
                myMembers.Where(Membership.Columns.CreatedOn, Comparison.GreaterOrEquals, startDate);
                myMembers.Where(Membership.Columns.CreatedOn, Comparison.LessOrEquals, endDate);
                myMembers.Load();

                ArrayList particularList = new ArrayList();
                foreach (Membership m in myMembers)
                {
                    particularList.Add(m.MembershipNo);
                }

                AttachedParticularCollection myAPs = new AttachedParticularCollection();
                myAPs.Where(AttachedParticular.Columns.MembershipNo, Comparison.In, particularList);
                myAPs.Where(AttachedParticular.Columns.CreatedOn, Comparison.GreaterOrEquals, startDate);
                myAPs.Where(AttachedParticular.Columns.CreatedOn, Comparison.LessOrEquals, endDate);
                myAPs.Load();

                PowerPOSLib.PowerPOSSync.Synchronization ws
                    = new PowerPOSLib.PowerPOSSync.Synchronization();

                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DataSet dsMember = new DataSet();
                DataTable dtMember = myMembers.ToDataTable();
                DataTable dtAP = myAPs.ToDataTable();
                setAllDateTimeColumnToBeLocal(dtMember);
                setAllDateTimeColumnToBeLocal(dtAP);
                dtMember.TableName = "Membership";
                dtAP.TableName = "AttachedParticular";
                dsMember.Tables.Add(dtMember);
                dsMember.Tables.Add(dtAP);

                Logger.writeLog("Start Sending New Membership Signups");
                result =
                    ws.FetchNewMembershipSignUps(dsMember);
                Logger.writeLog
                    ("Sending New Membership signup completed. Result = " + result);

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send New Membership signup failed.");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool SendInventory(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                InventoryHdrCollection myHdr = new InventoryHdrCollection();
                InventoryDetCollection myDet = new InventoryDetCollection();

                //------------------------------Apply Date Filtering----------------------------------
                myHdr.Where(InventoryHdr.Columns.InventoryDate, Comparison.GreaterOrEquals, StartDate);
                myHdr.Where(InventoryHdr.Columns.InventoryDate, Comparison.LessOrEquals, EndDate);
                myHdr.Load();

                string detSQL = "select * from InventoryDet where InventoryHdrRefNo in (select InventoryHdrRefNo from inventoryhdr where inventorydate >= '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "' and inventorydate <= '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "');";
                DataTable dtDet = DataService.GetDataSet(new QueryCommand(detSQL, "PowerPOS")).Tables[0];
                myDet.Load(dtDet);

                //Pull out Stock In Ref No for updates
                string remainingQtySQL = "declare @startDate datetime; " +
                                            "declare @endDate datetime; " +
                                            "set @startDate = '" + StartDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                                            "set @endDate = '" + EndDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                                            "select InventoryDetRefNo, RemainingQty  " +
                                            "from InventoryDet where InventoryDetRefNo in " +
                                            "(select StockInRefNo from InventoryHdr a inner join InventoryDET b " +
                                            "on a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
                                            "where not stockinrefno is null and a.InventoryDate > @startDate and a.InventoryDate <= @endDate) ";

                DataTable dt = DataService.GetDataSet(new QueryCommand(remainingQtySQL, "PowerPOS")).Tables[0];

                //-----------------------------------------------------------------------------------------------
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;

                string[][] objHdr = new string[myHdr.Count][];
                QueryParameterCollection param;
                if (myHdr.Count > 0)
                {

                    for (int op = 0; op < myHdr.Count; op++)
                    {
                        param = myHdr[op].GetInsertCommand("SYNC").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                                objArr[oT] = param[oT].ParameterValue.ToString();
                        }
                        objHdr[op] = objArr;
                    }
                }

                string[][] objDet = new string[myDet.Count][];
                if (myDet.Count > 0)
                {

                    for (int op = 0; op < myDet.Count; op++)
                    {
                        param = myDet[op].GetInsertCommand("SYNC").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                                objArr[oT] = param[oT].ParameterValue.ToString();
                        }
                        objDet[op] = objArr;
                    }
                }

                string[][] objRemQty = new string[dt.Rows.Count][];
                for (int op = 0; op < dt.Rows.Count; op++)
                {
                    string[] objArr = new string[2];

                    objArr[0] = dt.Rows[op][0].ToString();
                    objArr[1] = dt.Rows[op][1].ToString();

                    objRemQty[op] = objArr;
                }
                Logger.writeLog("Start Sending Inventorys");
                bool result = ws.FetchInventorys(objHdr, objDet, objRemQty);
                Logger.writeLog("Sending Inventorys completed. Result = " + result);

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Inventory failed:" + ex.Message, true);
                //Logger.writeLog(ex);
                return false;
            }
        }

        /*
        public static bool SendNewSignUp(DateTime startDate, DateTime endDate)
        {
            try
            {

                MembershipCollection myMembers = new MembershipCollection();

                DateTime lastClosingTime = ClosingController.FetchLastClosingTime(PointOfSaleInfo.PointOfSaleID);

                if (endDate > lastClosingTime)
                        endDate = lastClosingTime;
                //------------------------------Apply Date Filtering----------------------------------
                myMembers.Where(Membership.Columns.CreatedOn, 
                    Comparison.GreaterOrEquals, startDate);
                myMembers.Where(Membership.Columns.CreatedOn, 
                    Comparison.LessOrEquals, endDate);
                myMembers.Load();
                
                
                PowerPOSLib.PowerPOSSync.Synchronization ws 
                    = new PowerPOSLib.PowerPOSSync.Synchronization();

                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DataSet dsMember = new DataSet();
                DataTable dtMember = myMembers.ToDataTable();
                dtMember.TableName = "Membership";
                dsMember.Tables.Add(dtMember);

                Logger.writeLog("Start Sending New Membership Signups");
                bool result = 
                    ws.FetchNewMembershipSignUps(dsMember);
                Logger.writeLog
                    ("Sending New Membership signup completed. Result = " + result);

                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Order failed.");
                Logger.writeLog(ex);
                return false;
            }
        }
        */

        private static void setAllDateTimeColumnToBeLocal(DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].DataType == Type.GetType("System.DateTime"))
                {
                    dt.Columns[i].DateTimeMode = DataSetDateTime.Unspecified;
                }
            }
        }

        public static bool SendMembershipAttendanceToServer(DateTime StartDate, DateTime EndDate)
        {
            CultureInfo ct = new CultureInfo("");
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";

            ct.DateTimeFormat = dtFormat;

            Logger.writeLog("Start sending Membership Attendance to server");
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 100000;
            ws.Url = WS_URL;
            bool result;
            DataSet ds;

            //Membership Attendance
            Logger.writeLog("Sending MembershipAttendance table...");
            ds = new DataSet();
            ds.Tables.Add(new MembershipAttendanceCollection().
                Where("ModifiedOn", Comparison.GreaterOrEquals, StartDate).
                Where("ModifiedOn", Comparison.LessOrEquals, EndDate).Load().ToDataTable());
            ds.Locale = ct;
            setAllDateTimeColumnToBeLocal(ds.Tables[0]);
            result = ws.FetchLogTable(ds, "MembershipAttendance");

            Logger.writeLog("Sending result = " + result);

            return result;
        }

        public static bool SendLogsToServer(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                CultureInfo ct = new CultureInfo("");
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd";

                ct.DateTimeFormat = dtFormat;

                Logger.writeLog("Start sending Log to server");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                bool result;
                bool TotalResult = true;
                DataSet ds;


                Logger.writeLog("Sending Warning table...");
                ds = new DataSet();

                WarningMsgCollection w = new WarningMsgCollection();
                w.Where("CreatedOn", Comparison.GreaterThan, StartDate);
                w.Where("CreatedOn", Comparison.LessOrEquals, EndDate);
                w.Load();

                ds.Tables.Add(
                    w.ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "WarningMsg");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                Logger.writeLog("Sending MembershipUpgrades table...");
                ds = new DataSet();
                ds.Tables.Add(new MembershipUpgradeLogCollection().BetweenAnd("ModifiedOn", StartDate, EndDate).Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "MembershipUpgradeLog");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                Logger.writeLog("Sending CashRecording table...");
                ds = new DataSet();
                ds.Tables.Add(new CashRecordingCollection().BetweenAnd("ModifiedOn", StartDate, EndDate).Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "CashRecording");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //CounterCloseLog
                Logger.writeLog("Sending CounterCloseLog table...");
                ds = new DataSet();
                ds.Tables.Add(new CounterCloseLogCollection().
                    BetweenAnd("ModifiedOn", StartDate, EndDate).Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "CounterCloseLog");

                Logger.writeLog("Sending CounterCloseDet table...");
                ds = new DataSet();
                ds.Tables.Add(new CounterCloseDetCollection().
                    BetweenAnd("ModifiedOn", StartDate, EndDate).Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "CounterCloseDet");

                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                /*LoginActivity                
                Logger.writeLog("Sending LoginActivity table...");
                ds = new DataSet();
                ds.Tables.Add(new LoginActivityCollection().BetweenAnd("ModifiedOn", StartDate, EndDate).Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "LoginActivity");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);
                */
                //SpecialActivityLog                
                Logger.writeLog("Sending SpecialActivityLog table...");
                ds = new DataSet();
                ds.Tables.Add(new SpecialActivityLogCollection().Load().BetweenAnd("ModifiedOn", StartDate, EndDate).ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "SpecialActivityLog");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //Send PreOrder Record
                Logger.writeLog("Sending PreOrder Record table...");
                ds = new DataSet();
                ds.Tables.Add(new PreOrderRecordCollection().Load().BetweenAnd("ModifiedOn", StartDate, EndDate).ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "PreOrderRecord");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //Membership Signup
                Logger.writeLog("Sending New Signup...");
                result = SendNewSignUp(StartDate, EndDate);
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //Membership Renewals
                Logger.writeLog("Sending MembershipRenewal table...");
                ds = new DataSet();
                ds.Tables.Add(new MembershipRenewalCollection().
                    Where("ModifiedOn", Comparison.GreaterThan, StartDate).
                    Where("ModifiedOn", Comparison.LessOrEquals, EndDate).
                    Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "MembershipRenewal");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //Send sales commission record....                

                //Send Package Redemption....
                Logger.writeLog("Sending PackageRedemptionLog table...");
                ds = new DataSet();
                ds.Tables.Add(new PackageRedemptionLogCollection().
                    Where("ModifiedOn", Comparison.GreaterThan, StartDate).
                    Where("ModifiedOn", Comparison.LessOrEquals, EndDate).
                    Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "PackageRedemptionLog");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);


                //Send Attendance data
                Logger.writeLog("Sending Membership Attendance table...");
                ds = new DataSet();
                ds.Tables.Add(new MembershipAttendanceCollection().
                    Where("ModifiedOn", Comparison.GreaterThan, StartDate).
                    Where("ModifiedOn", Comparison.LessOrEquals, EndDate).
                    Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "MembershipAttendance");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //Send LineInfo data
                Logger.writeLog("Sending LineInfo table...");
                ds = new DataSet();
                ds.Tables.Add(new LineInfoCollection().
                    Where("ModifiedOn", Comparison.GreaterThan, StartDate).
                    Where("ModifiedOn", Comparison.LessOrEquals, EndDate).
                    Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "LineInfo");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                //Send Project Table....
                /*Logger.writeLog("Sending Project table...");
                ds = new DataSet();
                ds.Tables.Add(new ProjectCollection().
                    Where("ModifiedOn", Comparison.GreaterThan, StartDate).
                    Where("ModifiedOn", Comparison.LessOrEquals, EndDate).
                    Load().ToDataTable());
                ds.Locale = ct;
                setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                result = ws.FetchLogTable(ds, "Project");
                TotalResult &= result;
                Logger.writeLog("Sending result = " + result);

                */
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        /// <summary>
        /// Send modified membership remarks to web service. (created by John Harries)
        /// </summary>
        /// <param name="membership">Modified membership remarks.</param>
        /// <returns>True for success process and false for unsuccess process.</returns>
        public static bool SendModifiedMembershipRemarks(Membership membership)
        {
            try
            {
                MembershipCollection memberColl = new MembershipCollection();
                memberColl.Add(membership);

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DataSet ds = new DataSet();
                ds.Tables.Add(memberColl.ToDataTable());
                ws.FetchMemberRemarks(ds);

                Logger.writeLog(string.Format("Sending result = {0}", true.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool UpdateOrderHdrRemarkToServer(string orderHdrID, string remark)
        {
            try
            {
                Logger.writeLog("Sending OrderHdr Remark To Server");

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                bool success = ws.UpdateOrderHdrRemark(orderHdrID, remark);

                Logger.writeLog(string.Format("Sending result = {0}", success.ToString()));
                return success;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool UpdateDepositAmountToServer(DataSet dsData)
        {
            try
            {
                Logger.writeLog("Sending Deposit Amount To Server");

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                bool success = ws.FetchDeliveryOrder(dsData);

                Logger.writeLog(string.Format("Sending result = {0}", success.ToString()));
                return success;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool SendRealTimeSales(POSController posc, out string status, out string orderHdrID)
        {
            status = "";
            orderHdrID = "";
            try
            {
                PowerPOSLib.PowerPOSSync.Synchronization ws
                    = new PowerPOSLib.PowerPOSSync.Synchronization();

                ws.Timeout = 100000;
                ws.Url = WS_URL;

                string memberRowID = posc.IsItemIsInOrderLine(MembershipController.MEMBERSHIP_SIGNUP_BARCODE);

                DataSet dsMember = new DataSet();
                MembershipCollection mcol = new MembershipCollection();
                mcol.Add(posc.CurrentMember);
                DataTable dtMember = mcol.ToDataTable();
                dtMember.TableName = "Member";

                if (memberRowID != "")
                {
                    DataTable dtMember1 = dtMember.Copy();
                    dsMember.Tables.Add(dtMember1);
                    ws.FetchNewMembershipSignUps(dsMember);
                }
                DataSet dsOrder = new DataSet();
                DataTable dtOrderHdr = new DataTable("OrderHdr");
                OrderHdrCollection oHdrCol = new OrderHdrCollection();
                posc.myOrderHdr.CashierID = UserInfo.username;
                oHdrCol.Add(posc.myOrderHdr); //.ToDataTable();
                dtOrderHdr = oHdrCol.ToDataTable();
                dtOrderHdr.TableName = "OrderHdr";

                DataTable dtOrderDet = posc.myOrderDet.ToDataTable();
                dtOrderDet.TableName = "OrderDet";

                DataTable dtReceiptHdr = new DataTable();
                ReceiptHdrCollection rHdrCol = new ReceiptHdrCollection();
                oHdrCol.Add(posc.myOrderHdr); //.ToDataTable();
                dtReceiptHdr = rHdrCol.ToDataTable();
                dtReceiptHdr.TableName = "ReceiptHdr";

                DataTable dtReceiptDet = posc.recDet.ToDataTable();
                dtReceiptDet.TableName = "ReceiptDet";

                dsOrder.Tables.Add(dtOrderHdr);
                dsOrder.Tables.Add(dtOrderDet);
                dsOrder.Tables.Add(dtReceiptHdr);
                dsOrder.Tables.Add(dtReceiptDet);
                dsOrder.Tables.Add(dtMember);


                Logger.writeLog("Start Sending Real Time Sales");
                byte[] data = CompressDataSetToByteArray(dsOrder);

                #region Check HQ
                //bool createPurchaseOrder = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoReplenishOrderUponSales), false);
                //PointOfSale poSale = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
                //if (poSale.Userflag1.HasValue)
                //    if ((bool)poSale.Userflag1)
                //        createPurchaseOrder = false;

                //Logger.writeLog(createPurchaseOrder.ToString());
                PointOfSaleCollection pColl = new PointOfSaleCollection();
                //pColl.Where(PointOfSale.Columns.Deleted,false);
                pColl.Load();

                foreach (PointOfSale p in pColl)
                {


                    if (p.LinkedMembershipNo == posc.CurrentMember.MembershipNo)
                    {
                        //Logger.writeLog(p.Userfld1 + "-" + posc.CurrentMember.MembershipNo);
                        //createPurchaseOrder = true && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoReplenishOrderUponSales), false);
                    }
                }
                #endregion

                bool result = ws.SubmitSales(data, SalesPersonInfo.SalesPersonID == "0" ? UserInfo.username : SalesPersonInfo.SalesPersonID, false, out status, out orderHdrID);
                if (result)
                    deductInventory();
                Logger.writeLog
                    ("Sending Real Time Sales completed. Result = " + result);

                status = "";
                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send New Membership signup failed.");
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public static bool SendRealTimeSalesByOrderHdrID(POSController posc, string OrderHdrID, out string status)
        {
            status = "";
            try
            {

                bool result = false;
                PowerPOSLib.PowerPOSSync.Synchronization ws
                    = new PowerPOSLib.PowerPOSSync.Synchronization();

                ws.Timeout = 100000;
                ws.Url = WS_URL;

                string[][] objHdr;
                string[][] objDet;
                string[][] objSalesRec;
                string[][] objReceiptHdr;
                string[][] objReceiptDet;
                string[][] objMembership;
                string[][] objVoidLog;
                string[][] objDetUOMConv;

                string statusMsg = "";

                if (!getOrderDataByOrderHdrID(OrderHdrID, out objHdr, out objDet, out objReceiptHdr, out objReceiptDet, out objSalesRec,
                    out objMembership, out objVoidLog, out objDetUOMConv, out statusMsg))
                {
                    throw new Exception(statusMsg);
                }

                result = ws.FetchOrdersCCMWRealTime(objHdr, objDet, objReceiptHdr, objReceiptDet, objSalesRec, objMembership, objVoidLog, objDetUOMConv);
                if (!result)
                {
                    statusMsg = "Update data failed. Please contact Administrator.";
                }

                return result;

            }
            catch (Exception ex)
            {
                Logger.writeLog("Sending Real Time Sales signup failed.");
                Logger.writeLog(ex);
                status = ex.Message;
                return false;
            }
        }

        public static bool getOrderDataByOrderHdrID(string OrderHdrID, out string[][] objHdr,
          out string[][] objDet, out string[][] objReceiptHdr, out string[][] objReceiptDet,
          out string[][] objSalesRec, out string[][] objMembership, out string[][] objVoidLog,
          out string[][] objDetUOMConv, out string statusMsg)
        {
            try
            {
                statusMsg = "";

                OrderHdrCollection myHdr = new OrderHdrCollection();
                myHdr.Where(OrderHdr.Columns.OrderHdrID, OrderHdrID);
                myHdr.Load();

                if (myHdr.Count() > 0)
                {
                    OrderDetCollection myDet = new OrderDetCollection();
                    ReceiptHdrCollection myRcptHdr = new ReceiptHdrCollection();
                    ReceiptDetCollection myRcptDet = new ReceiptDetCollection();
                    SalesCommissionRecordCollection mySalesRecord = new SalesCommissionRecordCollection();
                    MembershipCollection myMembers = new MembershipCollection();
                    VoidLogCollection myVoidLog = new VoidLogCollection();
                    OrderDetUOMConversionCollection myDetUOMConv = new OrderDetUOMConversionCollection();

                    for (int i = 0; i < myHdr.Count; i++)
                        myDet.AddRange(myHdr[i].OrderDetRecords());

                    for (int i = 0; i < myHdr.Count; i++)
                        myRcptHdr.AddRange(myHdr[i].ReceiptHdrRecords());

                    for (int i = 0; i < myRcptHdr.Count; i++)
                        myRcptDet.AddRange(myRcptHdr[i].ReceiptDetRecords());

                    for (int i = 0; i < myHdr.Count; i++)
                        mySalesRecord.AddRange(myHdr[i].SalesCommissionRecordRecords());
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        if (myHdr[i].IsVoided == false && myHdr[i].MembershipNo != "WALK-IN")
                        {
                            Membership m = new Membership(myHdr[i].MembershipNo);
                            bool isFound = false;
                            foreach (Membership ms in myMembers)
                            {
                                if (m.MembershipNo == ms.MembershipNo)
                                    isFound = true;
                            }
                            if (!isFound)
                                myMembers.Add(m);
                        }
                    }

                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        if (myHdr[i].IsVoided == true)
                        {
                            VoidLog v = new VoidLog(VoidLog.Columns.OrderHdrID, myHdr[i].OrderHdrID);
                            bool isFound = false;
                            foreach (VoidLog vtemp in myVoidLog)
                            {
                                if (v.OrderHdrID == vtemp.OrderHdrID)
                                    isFound = true;
                            }
                            if (!isFound)
                                myVoidLog.Add(v);
                        }

                        OrderDetUOMConversionCollection uc = new OrderDetUOMConversionCollection();
                        uc.Where(OrderDetUOMConversion.Columns.OrderHdrID, myHdr[i].OrderHdrID);
                        uc.Load();
                        foreach (OrderDetUOMConversion uca in uc)
                        {
                            bool isFoundUC = false;
                            foreach (OrderDetUOMConversion uctemp in myDetUOMConv)
                            {
                                if (uctemp.OrderDetUOMConvID == uca.OrderDetUOMConvID)
                                    isFoundUC = true;
                            }
                            if (!isFoundUC)
                                myDetUOMConv.Add(uca);
                        }
                    }

                    //-----------------------------------------------------------------------------------------------                

                    objHdr = new string[myHdr.Count][];
                    int count = 0;
                    for (int op = 0; op < myHdr.Count; op++)
                    {

                        QueryParameterCollection param = myHdr[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime)
                                {
                                    objArr[oT] = param[oT].ParameterValue != null ? ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss.fff") : null;
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                            else
                            {
                                if (param[oT].ParameterName.ToLower() == "@modifiedon")
                                {
                                    objArr[oT] = myHdr[op].ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                }

                                if (param[oT].ParameterName.ToLower() == "@createdon")
                                {
                                    objArr[oT] = myHdr[op].CreatedOn.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                }

                                if (param[oT].ParameterName.ToLower() == "@createdby")
                                {
                                    objArr[oT] = myHdr[op].CreatedBy;
                                }

                                if (param[oT].ParameterName.ToLower() == "@modifiedby")
                                {
                                    objArr[oT] = myHdr[op].ModifiedBy;
                                }
                            }
                        }
                        objHdr[count] = objArr;
                        count++;
                    }




                    objDet = new string[myDet.Count][];

                    for (int op = 0; op < myDet.Count; op++)
                    {
                        QueryParameterCollection param = myDet[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime)
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objDet[op] = objArr;
                    }

                    objReceiptHdr = new string[myRcptHdr.Count][];

                    for (int op = 0; op < myRcptHdr.Count; op++)
                    {
                        QueryParameterCollection param = myRcptHdr[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime)
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objReceiptHdr[op] = objArr;
                    }


                    objReceiptDet = new string[myRcptDet.Count][];

                    for (int op = 0; op < myRcptDet.Count; op++)
                    {
                        QueryParameterCollection param = myRcptDet[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime)
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objReceiptDet[op] = objArr;
                    }

                    objSalesRec = new string[mySalesRecord.Count][];

                    for (int op = 0; op < mySalesRecord.Count; op++)
                    {
                        QueryParameterCollection param = mySalesRecord[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime)
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objSalesRec[op] = objArr;
                    }

                    objMembership = new string[myMembers.Count][];

                    for (int op = 0; op < myMembers.Count; op++)
                    {
                        QueryParameterCollection param = myMembers[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime && param[oT].ParameterValue != null && param[oT].ParameterValue.ToString() != "")
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objMembership[op] = objArr;
                    }

                    objVoidLog = new string[myVoidLog.Count][];

                    for (int op = 0; op < myVoidLog.Count; op++)
                    {
                        QueryParameterCollection param = myVoidLog[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime && param[oT].ParameterValue != null && param[oT].ParameterValue.ToString() != "")
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objVoidLog[op] = objArr;
                    }

                    objDetUOMConv = new string[myDetUOMConv.Count][];

                    for (int op = 0; op < myDetUOMConv.Count; op++)
                    {
                        QueryParameterCollection param = myDetUOMConv[op].GetInsertCommand("").Parameters;
                        string[] objArr = new string[param.Count];
                        for (int oT = 0; oT < param.Count; oT++)
                        {
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime && param[oT].ParameterValue != null && param[oT].ParameterValue.ToString() != "")
                                {
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objDetUOMConv[op] = objArr;
                    }

                    return true;


                }
                else
                {
                    objHdr = null;
                    objDet = null;
                    objReceiptHdr = null;
                    objReceiptDet = null;
                    objSalesRec = null;
                    objMembership = null;
                    objVoidLog = null;
                    objDetUOMConv = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objHdr = null;

                objDet = null;
                objSalesRec = null;
                objReceiptHdr = null;
                objReceiptDet = null;
                objMembership = null;
                objVoidLog = null;
                objDetUOMConv = null;
                statusMsg = ex.Message;
                Logger.writeLog("Get Order failed." + ex.Message, true);
                return false;
            }
        }

        #region "old code"
        /*public static bool SendInventory()
        {
            try
            {
                InventoryHdrCollection myHdr = new InventoryHdrCollection();
                InventoryDetCollection myDet = new InventoryDetCollection();
                
                //Apply Date Filtering

                myHdr.Load();
                myDet.Load();
                
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                
                DataSet dsHdr = new DataSet();
                DataTable dtHdr = myHdr.ToDataTable();
                dtHdr.TableName = "InventoryHeader";
                dsHdr.Tables.Add(dtHdr);
                
                DataSet dsDet = new DataSet();
                DataTable dtDet = myDet.ToDataTable();
                dtDet.TableName = "InventoryDetail";
                dsDet.Tables.Add(dtDet);
                
                return ws.FetchInventories(dsDet,dsHdr);                
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Inventory failed.");
                Logger.writeLog(ex);
                return false;
            }
            
        }*/
        /*public static bool SendItems()
        {
            try
            {
                ItemCollection myHdr = new ItemCollection();                

                //Apply Date Filtering?

                myHdr.Load();
                
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                DataSet dsHdr = new DataSet();
                DataTable dtHdr = myHdr.ToDataTable();
                dtHdr.TableName = "Item";
                dsHdr.Tables.Add(dtHdr);

                return ws.FetchItems(dsHdr);                
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Items failed.");
                Logger.writeLog(ex);
                return false;
            }

        }*/
        /*public static bool SendCategories()
        {
            try
            {
                CategoryCollection myHdr = new CategoryCollection();

                //Apply Date Filtering

                myHdr.Load();

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                DataSet dsHdr = new DataSet();
                DataTable dtHdr = myHdr.ToDataTable();
                dtHdr.TableName = "Category";
                dsHdr.Tables.Add(dtHdr);

                return ws.FetchCategories(dsHdr);
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Categories failed.");
                Logger.writeLog(ex);
                return false;
            }
        }*/
        #endregion
        #endregion

        #region "From Server to Client"

        private const int RETRY_LIMIT = 10;

        public static bool GetBasicInfoFromServer
            (bool fetchPOS, bool fetchUsers,
             bool fetchMemberships, bool fetchPromotions, bool fetchItems, bool fetchHotKeys,
             bool fetchLineInfo, bool fetchProjects,
             bool syncAll)
        {
            bool TotalResult = true;
            bool result;
            int i;

            Logger.writeLog("Fetch basic info from server. Synchronization started..");



            if (fetchPOS)
            {
                //Get outlet and POS related location
                TotalResult = GetPOSLocations(syncAll);
            }
            if (fetchUsers)
            {
                //Get user accounts
                TotalResult &= GetUsers(syncAll);
            }

            if (fetchMemberships)
            {
                //Get memberships
                TotalResult &= GetMemberships(syncAll);
            }

            if (fetchItems)
            {
                TotalResult &= GetItems(syncAll);
                TotalResult &= GetPriceScheme(syncAll);
            }

            if (fetchHotKeys)
            {
                TotalResult &= GetHotKeys(syncAll);
            }

            if (fetchPromotions)
            {
                //Promotions
                TotalResult &= GetPromotions(syncAll);
            }
            if (fetchLineInfo)
            {
                //LineInfo
                TotalResult &= GetLineInfo(syncAll);
            }
            if (fetchProjects)
            {
                //Projects
                TotalResult &= GetProjects(syncAll);
            }
            TotalResult &= GetVouchers(syncAll);

            Logger.writeLog("Synchronization completed with success=" + TotalResult);

            return TotalResult;
        }
        public static bool GetLineInfo(bool syncAll)
        {
            bool TotalResult = true, result; int i;
            string[] tableName = { "LineInfo" };
            string[] primaryKeyName = { "LineInfoID" };
            bool[] IsPKAutoGenerated = { true };
            bool[] AlwaysSyncAll = { true };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetProjects(bool syncAll)
        {
            bool TotalResult = true, result; int i;
            string[] tableName = { "Project" };
            string[] primaryKeyName = { "ID" };
            bool[] IsPKAutoGenerated = { true };
            bool[] AlwaysSyncAll = { true };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }
        public static bool GetUsers(bool syncAll)
        {
            bool TotalResult, result;
            TotalResult = true;
            string[] tableName = { "UserPrivilege", "UserGroup", "GroupUserPrivilege", "UserMst" };
            string[] primaryKeyName = { "UserPrivilegeID", "GroupID", "GroupUserPrivilegeID", "UserName" };
            bool[] IsPKAutoGenerated = { true, true, true, false };
            bool[] AlwaysSyncAll = { true, true, true, false };
            bool IsSyncAll = false;
            int i;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k] == true)
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    QueryCommandCollection cDel = new QueryCommandCollection();
                    if (tableName[k] == "GroupUserPrivilege")
                    {

                        cDel = new QueryCommandCollection();
                        GroupUserPrivilegeCollection gCol = new GroupUserPrivilegeCollection();
                        gCol.Load();
                        for (int e = 0; e < gCol.Count; e++)
                        {
                            gCol[e].IsNew = true;
                            cDel.Add(gCol[e].GetInsertCommand("SYSTEM"));
                        }
                        using (TransactionScope ts = new TransactionScope())
                        {
                            DataService.ExecuteQuery(new QueryCommand("Delete from GroupUserPrivilege", "PowerPOS"));
                            result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                            if (!result && tableName[k] == "GroupUserPrivilege" && cDel.Count > 0)
                            {
                                for (int e = 0; e < cDel.Count; e++)
                                {
                                    DataService.ExecuteQuery(cDel[e]);
                                }
                            }
                            ts.Complete();
                        }
                    }
                    else
                    {
                        result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    }
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetHotKeys(bool syncAll)
        {
            bool TotalResult = true, result;
            string[] tableName = { "QuickAccessCategory", "QuickAccessGroup", "QuickAccessButton", "QuickAccessGroupMap" };
            string[] primaryKeyName = { "QuickAccessCategoryId", "QuickAccessGroupID", "QuickAccessButtonID", "QuickAccessGroupMapID" };
            bool[] IsPKAutoGenerated = { false, false, false, false, false, false };
            bool[] AlwaysSyncAll = { true, true, true, true, true, true };
            bool IsSyncAll;
            int i;
            for (int k = 0; k < tableName.Length; k++)
            {
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                i = 0; result = false;
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;

        }

        public static bool GetPOSLocations(bool syncAll)
        {
            bool TotalResult = true, result;
            string[] tableName = { "cashrecordingtype", "emailnotification", "InventoryStockOutReason", "InventoryLocation", "Department", "Outlet", "QuickAccessGroup", "PointOfSale" };
            string[] primaryKeyName = { "cashrecordingtypeid", "EmailAddress", "ReasonID", "InventoryLocationID", "DepartmentID", "OutletName", "QuickAccessGroupID", "PointOfSaleID" };
            bool[] IsPKAutoGenerated = { true, false, true, true, true, false, false, true };
            bool[] AlwaysSyncAll = { true, true, true, true, true, true, true, true };
            bool IsSyncAll;
            int i;
            for (int k = 0; k < tableName.Length; k++)
            {
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                i = 0; result = false;
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetItems(bool syncAll)
        {
            bool TotalResult = true, result; int i;
            string[] tableName = { "ItemDepartment", "Category", "Item", "AlternateBarcode", "Supplier", "ItemSupplierMap" };
            string[] primaryKeyName = { "ItemDepartmentID", "CategoryName", "ItemNo", "BarcodeID", "SupplierID", "ItemSupplierMapID" };
            bool[] IsPKAutoGenerated = { false, false, false, true, true, true };
            bool[] AlwaysSyncAll = { true, true, false, false, true, true };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetMemberships(bool syncAll)
        {
            bool TotalResult = true, result;

            string[] tableName = { "MembershipGroup", "Membership", "AttachedParticular" };
            string[] primaryKeyName = { "MembershipGroupId", "MembershipNo", "AttachedParticularID" };
            bool[] IsPKAutoGenerated = { true, false, true };
            bool[] AlwaysSyncAll = { true, false, false };
            if (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem") == "yes")
            {
                tableName = new string[] { "MembershipGroup", "Membership", "MembershipPoints", "AttachedParticular" };
                primaryKeyName = new string[] { "MembershipGroupId", "MembershipNo", "PointID", "AttachedParticularID" };
                IsPKAutoGenerated = new bool[] { true, false, true, true };
                AlwaysSyncAll = new bool[] { true, false, false, false };
            }

            bool IsSyncAll;
            int i;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetVouchers(bool syncAll)
        {
            bool TotalResult = true, result; int i;
            string[] tableName = { "Vouchers" };
            string[] primaryKeyName = { "VoucherID" };
            bool[] IsPKAutoGenerated = { false };
            bool[] AlwaysSyncAll = { false };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTableNoUpdate(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }
            return TotalResult;
        }

        public static bool GetPromotions(bool syncAll)
        {
            bool TotalResult = true, result; int i;
            string[] tableName = { "ItemGroup", "ItemGroupMap", "PromoCampaignHdr", "PromoCampaignDet", "PromoOutlet", "PromoLocationMap", "PromoDaysMap" };
            //string[] tableName = { "ItemGroup", "ItemGroupMap", "PromoCampaignHdr", "PromoLocationMap" };
            string[] primaryKeyName = { "ItemGroupID", "ItemGroupMapID", "PromoCampaignHdrID", "PromoCampaignDetID", "PromoOutletID", "PromoLocationMapID", "PromoDaysID" };
            //string[] primaryKeyName = { "ItemGroupID", "ItemGroupMapID", "PromoCampaignHdrID", "PromoLocationMapID" };
            bool[] IsPKAutoGenerated = { true, true, true, true, true, false, true };
            bool[] AlwaysSyncAll = { true, true, true, true, true, false, true };
            //bool[] IsPKAutoGenerated = { true, true, true, true, false, false };
            //bool[] AlwaysSyncAll = { true, true, false, false, false, false };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetPriceScheme(bool syncAll)
        {
            Query qr = new Query("PriceScheme");
            qr.QueryType = QueryType.Delete;
            qr.Execute();

            bool TotalResult = true, result; int i;
            string[] tableName = { "PriceScheme" };
            string[] primaryKeyName = { "SchemeID" };
            bool[] IsPKAutoGenerated = { false };
            bool[] AlwaysSyncAll = { true };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    PointOfSale posInfo = new PointOfSale(PointOfSaleInfo.PointOfSaleID);
                    string sqlQuery = "SELECT * FROM PriceScheme WHERE SchemeID = '" +
                        posInfo.PriceSchemeID + "'";
                    result = GetTableCustom(sqlQuery, IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static bool GetDeliveryOrderFromServer(DateTime startDate, DateTime endDate)
        {
            try
            {
                Logger.writeLog("Synchronization started for Delivery Order");

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;
                DataSet ds;

                ds = ws.GetDeliveryOrder(startDate, endDate, PointOfSaleInfo.PointOfSaleID);

                if (ds != null && ds.Tables.Contains("DeliveryOrder"))
                {
                    DataSet newDS = new DataSet();
                    newDS.Tables.Add(ds.Tables["DeliveryOrder"].Copy());
                    FetchTableWithUpdateOption(newDS, "DeliveryOrder", true);
                }

                if (ds != null && ds.Tables.Contains("DeliveryOrderDetails"))
                {
                    DataSet newDS = new DataSet();
                    newDS.Tables.Add(ds.Tables["DeliveryOrderDetails"].Copy());
                    FetchTableWithUpdateOption(newDS, "DeliveryOrderDetails", true);
                }

                if (ds != null && ds.Tables.Contains("DepositAmount"))
                {
                    foreach (DataRow dr in ds.Tables["DepositAmount"].Rows)
                    {
                        OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
                        if (!string.IsNullOrEmpty(od.OrderDetID))
                        {
                            od.DepositAmount = Convert.ToDecimal(dr["DepositAmount"]);
                            od.Save("SYNC");
                        }
                    }
                }

                Logger.writeLog("Synchronization completed for Delivery Order");

                return true;

            }
            catch (Exception ex)
            {
                Logger.writeLog("Synchronization failed.");
                Logger.writeLog(ex);
                return false;
            }

        }

        public static bool GetGuestbook(bool syncAll)
        {

            bool TotalResult = true, result; int i;
            string[] tableName = { "Membership", "GuestBook" };
            string[] primaryKeyName = { "MembershipNo", "GuestBookID" };
            bool[] IsPKAutoGenerated = { false, true };
            bool[] AlwaysSyncAll = { true, true };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        #endregion

        public static bool ContainsUnicodeCharacter(string input)
        {
            bool isNonUnicode = false;

            const int MaxAnsiCode = 255;

            char[] inputChar = input.ToCharArray();

            for (int i = 0; i < inputChar.Length; i++)
            {
                //if ((int)inputChar[i] > MaxAnsiCode)
                if (char.GetUnicodeCategory(inputChar[i]) == UnicodeCategory.OtherLetter)
                {
                    isNonUnicode = true;
                    break;
                }
            }

            return isNonUnicode;
        }

        /*
        public static bool isChinese(string text)
        {
            return text.Any(c => c >= 0x20000 && c <= 0xFA2D);
        }
*/

        #region "Individual Table - From Server to Client"

        private static bool GetTable(bool syncAll, string tableName, string primaryKeyName, bool IsPKAutoGenerated)
        {
            string insertSql = "";
            string ColumnList = "";
            string ParameterList = "";

            /*
            int i = 0;
            if (tableName.ToLower() == "category")
            {
                i++;
            }
            i++;
            */

            try
            {
                Logger.writeLog("Synchronization started for " + tableName);

                QueryCommandCollection cmd; QueryCommand mycmd;
                Query qry; Where whr;
                ArrayList Columns, Parameters;

                //Basic Info from server is Category and User
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;
                //Get the Category
                DataSet ds;

                //Get the Compressed DataTable
                byte[] data = ws.GetDataTableCompressed(tableName, syncAll);

                ds = DecompressDataSetFromByteArray(data);

                cmd = new QueryCommandCollection();
                if (IsPKAutoGenerated)
                {
                    mycmd = new QueryCommand("SET IDENTITY_INSERT " + tableName + " ON;"); //Ignore column 0 if it is identity column
                    cmd.Add(mycmd);
                }

                Columns = new ArrayList(); Parameters = new ArrayList();

                for (int i = 0; i < ds.Tables[0].Columns.Count - 1; i++)
                {
                    ColumnList += ds.Tables[0].Columns[i].ColumnName + ",";
                    ParameterList += "@" + ds.Tables[0].Columns[i].ColumnName + ",";
                    Columns.Add(ds.Tables[0].Columns[i].ColumnName);
                    Parameters.Add("@" + ds.Tables[0].Columns[i].ColumnName);
                }

                ColumnList += ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName;
                ParameterList += "@" + ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName;
                Columns.Add(ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName);
                Parameters.Add("@" + ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName);

                //Create Active Record from table                                                
                insertSql = "insert into " + tableName + " (" + ColumnList + ")";
                insertSql += " values (" + ParameterList + ") ";
                Logger.writeLog(insertSql);
                //Set where condition to check the count....

                qry = new Query(tableName);
                whr = new Where();
                whr.ColumnName = primaryKeyName;
                whr.Comparison = Comparison.Equals;
                whr.ParameterName = "@" + primaryKeyName;

                bool hasDeleteColumn = false;
                if (ds.Tables[0].Columns.Contains("Deleted"))
                {
                    hasDeleteColumn = true;
                }


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //where clause to check the type
                    //if guid, need to convert to string
                    if (ds.Tables[0].Rows[i][primaryKeyName] is Guid)
                    {
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName].ToString();
                    }
                    //if nvarchar
                    else if (ds.Tables[0].Rows[i][primaryKeyName] is String)
                    {
                        whr.DbType = DbType.String; //returning sql datatype nvarchar
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName].ToString();
                    }
                    else
                    {
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName];
                    }

                    int NumberOfRowWithSamePrimaryKey = qry.GetCount(primaryKeyName, whr);

                    if (NumberOfRowWithSamePrimaryKey > 0)
                    {
                        if (hasDeleteColumn && ds.Tables[0].Rows[i]["Deleted"] is bool && (bool)ds.Tables[0].Rows[i]["Deleted"] == true)
                        {
                            //it's delete                            
                            Query qrytmp = new Query(tableName);
                            qrytmp.AddWhere(primaryKeyName, ds.Tables[0].Rows[i][primaryKeyName]);
                            qrytmp.AddUpdateSetting("Deleted", true);
                            qrytmp.QueryType = QueryType.Update;
                            mycmd = qrytmp.BuildUpdateCommand();
                        }
                        else
                        {
                            //if exist, do update
                            Query qrytmp = new Query(tableName);
                            qrytmp.AddWhere(primaryKeyName, ds.Tables[0].Rows[i][primaryKeyName]);
                            for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (ds.Tables[0].Columns[k].ToString().ToLower() != primaryKeyName.ToLower())
                                    qrytmp.AddUpdateSetting(ds.Tables[0].Columns[k].ToString(), ds.Tables[0].Rows[i][k]);
                            }
                            qrytmp.QueryType = QueryType.Update;
                            mycmd = qrytmp.BuildUpdateCommand();
                        }
                    }
                    else
                    {
                        mycmd = new QueryCommand(insertSql, "PowerPOS");

                        for (int k = 0; k < Columns.Count; k++)
                        {

                            if (ds.Tables[0].Columns[Columns[k].ToString()].DataType == System.Type.GetType("System.Guid"))
                            {
                                mycmd.AddParameter(Parameters[k].ToString(),
                                    (new Guid(ds.Tables[0].Rows[i][Columns[k].
                                        ToString()].ToString())).ToString(),
                                        DataService.GetSchema(tableName, "PowerPOS").
                                        GetColumn(Columns[k].ToString()).DataType);
                            }
                            else
                            {
                                mycmd.AddParameter
                                    (Parameters[k].ToString(),
                                    ds.Tables[0].Rows[i][Columns[k].ToString()],
                                    DataService.GetSchema(tableName, "PowerPOS").
                                    GetColumn(Columns[k].ToString()).DataType);
                            }
                        }
                    }
                    cmd.Add(mycmd);
                }
                if (IsPKAutoGenerated)
                {
                    mycmd = new QueryCommand("SET IDENTITY_INSERT " + tableName + " OFF;"); //Ignore column 0 if it is identity column
                    cmd.Add(mycmd);
                }
                if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                Logger.writeLog("Synchronization completed.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Synchronization failed.");
                Logger.writeLog(ex);

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        throw ex;
                    }
                }

                return false;
            }
        }

        private static bool GetTableNoUpdate
            (bool syncAll, string tableName, string primaryKeyName, bool IsPKAutoGenerated)
        {
            try
            {
                Logger.writeLog("Synchronization started for " + tableName);

                QueryCommandCollection cmd; QueryCommand mycmd;
                Query qry; Where whr;
                ArrayList Columns, Parameters;

                //Basic Info from server is Category and User
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;
                //Get the Category
                DataSet ds;

                //Get the Compressed DataTable
                byte[] data = ws.GetDataTableCompressed(tableName, syncAll);

                ds = DecompressDataSetFromByteArray(data);

                cmd = new QueryCommandCollection();
                if (IsPKAutoGenerated)
                {
                    mycmd = new QueryCommand("SET IDENTITY_INSERT " + tableName + " ON;"); //Ignore column 0 if it is identity column
                    cmd.Add(mycmd);
                }
                Columns = new ArrayList(); Parameters = new ArrayList();
                string ColumnList = "", ParameterList = "";
                for (int i = 0; i < ds.Tables[0].Columns.Count - 1; i++)
                {
                    ColumnList += ds.Tables[0].Columns[i].ColumnName + ",";
                    ParameterList += "@" + ds.Tables[0].Columns[i].ColumnName + ",";
                    Columns.Add(ds.Tables[0].Columns[i].ColumnName);
                    Parameters.Add("@" + ds.Tables[0].Columns[i].ColumnName);
                }
                ColumnList += ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName;
                ParameterList += "@" + ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName;
                Columns.Add(ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName);
                Parameters.Add("@" + ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName);
                //Create Active Record from table                                                
                string insertSql = "insert into " + tableName + " (" + ColumnList + ")";
                insertSql += " values (" + ParameterList + ") ";

                //Set where condition to check the count....

                qry = new Query(tableName);
                whr = new Where();
                whr.ColumnName = primaryKeyName;
                whr.Comparison = Comparison.Equals;
                whr.ParameterName = "@" + primaryKeyName;

                bool hasDeleteColumn = false;
                if (ds.Tables[0].Columns.Contains("Deleted"))
                {
                    hasDeleteColumn = true;
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (ds.Tables[0].Rows[i][primaryKeyName] is Guid)
                    {
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName].ToString();
                    }
                    else
                    {
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName];
                    }
                    if (qry.GetCount(primaryKeyName, whr) > 0)
                    {
                        if (hasDeleteColumn && ds.Tables[0].Rows[i]["Deleted"] is bool && (bool)ds.Tables[0].Rows[i]["Deleted"] == true)
                        {
                            //it's delete                            
                            Query qrytmp = new Query(tableName);
                            qrytmp.AddWhere(primaryKeyName, ds.Tables[0].Rows[i][primaryKeyName]);
                            qrytmp.AddUpdateSetting("Deleted", true);
                            qrytmp.QueryType = QueryType.Update;
                            mycmd = qrytmp.BuildUpdateCommand();
                        }
                        else
                        {
                            //No Update
                            /*
                            Query qrytmp = new Query(tableName);
                            qrytmp.AddWhere(primaryKeyName, ds.Tables[0].Rows[i][primaryKeyName]);
                            for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (ds.Tables[0].Columns[k].ToString().ToLower() != primaryKeyName.ToLower())
                                    qrytmp.AddUpdateSetting(ds.Tables[0].Columns[k].ToString(), ds.Tables[0].Rows[i][k]);
                            }
                            qrytmp.QueryType = QueryType.Update;
                            mycmd = qrytmp.BuildUpdateCommand();
                             */
                            mycmd = null;
                        }
                    }
                    else
                    {
                        mycmd = new QueryCommand(insertSql, "PowerPOS");

                        for (int k = 0; k < Columns.Count; k++)
                        {
                            if (ds.Tables[0].Columns[Columns[k].ToString()].DataType == System.Type.GetType("System.Guid"))
                            {
                                mycmd.AddParameter(Parameters[k].ToString(),
                                    (new Guid(ds.Tables[0].Rows[i][Columns[k].ToString()].ToString())).ToString());
                            }
                            else
                            {
                                mycmd.AddParameter(Parameters[k].ToString(), ds.Tables[0].Rows[i][Columns[k].ToString()]);
                            }
                        }
                    }
                    if (mycmd != null) cmd.Add(mycmd);
                }
                if (IsPKAutoGenerated)
                {
                    mycmd = new QueryCommand("SET IDENTITY_INSERT " + tableName + " OFF;"); //Ignore column 0 if it is identity column
                    cmd.Add(mycmd);
                }
                if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                Logger.writeLog("Synchronization completed.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Synchronization failed.");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static DataSet DecompressDataSetFromByteArray(byte[] data)
        {
            //Decompressed it
            MemoryStream memStream = new MemoryStream(data);

            GZipStream unzipStream = new GZipStream(memStream, CompressionMode.Decompress);

            DataSet ds = new DataSet();

            ds.ReadXml(unzipStream, XmlReadMode.ReadSchema);

            memStream.Close();

            unzipStream.Close();
            return ds;
        }

        private static bool GetTableCustom(string sqlQuery, bool syncAll, string tableName, string primaryKeyName, bool IsPKAutoGenerated)
        {
            string insertSql = "";
            string ColumnList = "";
            string ParameterList = "";

            /*
            int i = 0;
            if (tableName.ToLower() == "category")
            {
                i++;
            }
            i++;
            */

            try
            {
                Logger.writeLog("Synchronization started for " + tableName);

                QueryCommandCollection cmd; QueryCommand mycmd;
                Query qry; Where whr;
                ArrayList Columns, Parameters;

                //Basic Info from server is Category and User
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;
                //Get the Category
                DataSet ds;

                //Get the Compressed DataTable
                byte[] data = ws.GetDataTableCustomCompressed(sqlQuery);

                ds = DecompressDataSetFromByteArray(data);

                cmd = new QueryCommandCollection();

                if (IsPKAutoGenerated)
                {
                    mycmd = new QueryCommand("SET IDENTITY_INSERT " + tableName + " ON;"); //Ignore column 0 if it is identity column
                    cmd.Add(mycmd);
                }

                Columns = new ArrayList(); Parameters = new ArrayList();

                for (int i = 0; i < ds.Tables[0].Columns.Count - 1; i++)
                {
                    ColumnList += ds.Tables[0].Columns[i].ColumnName + ",";
                    ParameterList += "@" + ds.Tables[0].Columns[i].ColumnName + ",";
                    Columns.Add(ds.Tables[0].Columns[i].ColumnName);
                    Parameters.Add("@" + ds.Tables[0].Columns[i].ColumnName);
                }

                ColumnList += ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName;
                ParameterList += "@" + ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName;
                Columns.Add(ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName);
                Parameters.Add("@" + ds.Tables[0].Columns[ds.Tables[0].Columns.Count - 1].ColumnName);

                //Create Active Record from table                                                
                insertSql = "insert into " + tableName + " (" + ColumnList + ")";
                insertSql += " values (" + ParameterList + ") ";
                Logger.writeLog(insertSql);
                //Set where condition to check the count....

                qry = new Query(tableName);
                whr = new Where();
                whr.ColumnName = primaryKeyName;
                whr.Comparison = Comparison.Equals;
                whr.ParameterName = "@" + primaryKeyName;

                bool hasDeleteColumn = false;
                if (ds.Tables[0].Columns.Contains("Deleted"))
                {
                    hasDeleteColumn = true;
                }


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    //where clause to check the type
                    //if guid, need to convert to string
                    if (ds.Tables[0].Rows[i][primaryKeyName] is Guid)
                    {
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName].ToString();
                    }
                    //if nvarchar
                    else if (ds.Tables[0].Rows[i][primaryKeyName] is String)
                    {
                        whr.DbType = DbType.String; //returning sql datatype nvarchar
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName].ToString();
                    }
                    else
                    {
                        whr.ParameterValue = ds.Tables[0].Rows[i][primaryKeyName];
                    }

                    int NumberOfRowWithSamePrimaryKey = qry.GetCount(primaryKeyName, whr);

                    if (NumberOfRowWithSamePrimaryKey > 0)
                    {
                        if (hasDeleteColumn && ds.Tables[0].Rows[i]["Deleted"] is bool && (bool)ds.Tables[0].Rows[i]["Deleted"] == true)
                        {
                            //it's delete                            
                            Query qrytmp = new Query(tableName);
                            qrytmp.AddWhere(primaryKeyName, ds.Tables[0].Rows[i][primaryKeyName]);
                            qrytmp.AddUpdateSetting("Deleted", true);
                            qrytmp.QueryType = QueryType.Update;
                            mycmd = qrytmp.BuildUpdateCommand();
                        }
                        else
                        {
                            //if exist, do update
                            Query qrytmp = new Query(tableName);
                            qrytmp.AddWhere(primaryKeyName, ds.Tables[0].Rows[i][primaryKeyName]);
                            for (int k = 0; k < ds.Tables[0].Columns.Count; k++)
                            {
                                if (ds.Tables[0].Columns[k].ToString().ToLower() != primaryKeyName.ToLower())
                                    qrytmp.AddUpdateSetting(ds.Tables[0].Columns[k].ToString(), ds.Tables[0].Rows[i][k]);
                            }
                            qrytmp.QueryType = QueryType.Update;
                            mycmd = qrytmp.BuildUpdateCommand();
                        }
                    }
                    else
                    {
                        mycmd = new QueryCommand(insertSql, "PowerPOS");

                        for (int k = 0; k < Columns.Count; k++)
                        {

                            if (ds.Tables[0].Columns[Columns[k].ToString()].DataType == System.Type.GetType("System.Guid"))
                            {
                                mycmd.AddParameter(Parameters[k].ToString(),
                                    (new Guid(ds.Tables[0].Rows[i][Columns[k].
                                        ToString()].ToString())).ToString(),
                                        DataService.GetSchema(tableName, "PowerPOS").
                                        GetColumn(Columns[k].ToString()).DataType);
                            }
                            else
                            {
                                mycmd.AddParameter
                                    (Parameters[k].ToString(),
                                    ds.Tables[0].Rows[i][Columns[k].ToString()],
                                    DataService.GetSchema(tableName, "PowerPOS").
                                    GetColumn(Columns[k].ToString()).DataType);
                            }
                        }
                    }
                    cmd.Add(mycmd);
                }
                if (IsPKAutoGenerated)
                {
                    mycmd = new QueryCommand("SET IDENTITY_INSERT " + tableName + " OFF;"); //Ignore column 0 if it is identity column
                    cmd.Add(mycmd);
                }
                if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                Logger.writeLog("Synchronization completed.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Synchronization failed.");
                Logger.writeLog(ex);

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        throw ex;
                    }
                }

                return false;
            }
        }

        private static bool UpdateAdjustedOrder()
        {
            string sqlQuery = "Select OrderDetID from orderdet where inventoryhdrrefno = 'ADJUSTED' ";

            try
            {
                //Logger.writeLog("Synchronization started Adjusted OrderDet ");

                QueryCommandCollection cmd; QueryCommand mycmd;
                Query qry; Where whr;


                //Basic Info from server is Category and User
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;
                //Get the Category
                DataSet ds;

                //Get the Compressed DataTable
                byte[] data = ws.GetDataTableCustomCompressed(sqlQuery);

                ds = DecompressDataSetFromByteArray(data);

                cmd = new QueryCommandCollection();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        mycmd = new QueryCommand("Update OrderDet set InventoryHdrRefNo = 'ADJUSTED' where orderdetid = '" + dr["OrderDetID"].ToString() + "'");
                        cmd.Add(mycmd);
                    }
                }
                if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                //Logger.writeLog("Adjust Adjusted Order Completed.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Adjust Adjusted Order failed.");
                Logger.writeLog(ex);

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        throw ex;
                    }
                }

                return false;
            }
        }

        #endregion

        public static bool SyncInventory(DateTime startDate, DateTime endDate)
        {
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                // Get Server CT Version (on Server)
                long? currentCTVersion = ws.GetChangeTrackingVersion();

                DataSet dsInventoryHdr = new DataSet();
                DataSet dsInventoryDet = new DataSet();
                DataSet dsStockTake = new DataSet();
                DataSet dsLocationTransfer = new DataSet();
                DataSet dsSupplierItemMap = new DataSet();
                for (int i = 0; i < RETRY_LIMIT; i++)
                {
                    try
                    {
                        dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                            ws.GetInventoryHdrFullTable(startDate));
                        break;
                    }
                    catch (Exception) { }
                }
                for (int i = 0; i < RETRY_LIMIT; i++)
                {
                    try
                    {
                        dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                            ws.GetInventoryDetFullTable(startDate));
                        break;
                    }
                    catch (Exception) { }
                }
                for (int i = 0; i < RETRY_LIMIT; i++)
                {
                    try
                    {
                        dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                            ws.GetChangeTrackingFullTable("StockTake", "StockTakeID"));
                        break;
                    }
                    catch (Exception) { }
                }
                for (int i = 0; i < RETRY_LIMIT; i++)
                {
                    try
                    {
                        dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                            ws.GetChangeTrackingFullTable("LocationTransfer", "LocationTransferID"));
                        break;
                    }
                    catch (Exception) { }
                }
                DataTable dtInventoryHdr = dsInventoryHdr.Tables[0].Copy().RemoveExisting("InventoryHdr", "InventoryHdrRefNo");
                DataTable dtInventoryDet = dsInventoryDet.Tables[0].Copy().RemoveExisting("InventoryDet", "InventoryDetRefNo");
                DataTable dtStockTake = dsStockTake.Tables[0].Copy().RemoveExisting("StockTake", "StockTakeID");
                DataTable dtLocationTransfer = dsLocationTransfer.Tables[0].Copy().RemoveExisting("LocationTransfer", "LocationTransferID");

                DataSet dsInventoryHdr1 = new DataSet();
                dsInventoryHdr1.Tables.Add(dtInventoryHdr);
                DataSet dsInventoryDet1 = new DataSet();
                dsInventoryDet1.Tables.Add(dtInventoryDet);
                DataSet dsStockTake1 = new DataSet();
                dsStockTake1.Tables.Add(dtStockTake);
                DataSet dsLocationTransfer1 = new DataSet();
                dsLocationTransfer1.Tables.Add(dtLocationTransfer);

                QueryCommandCollection cmdCol = new QueryCommandCollection();
                cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsInventoryHdr1, false));
                cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsInventoryDet1, false));
                cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsStockTake1, true));
                cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsLocationTransfer1, true));

                DataService.ExecuteTransaction(cmdCol);

                AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());

                return true;
            }
            catch (Exception ex)
            { Logger.writeLog(ex.Message); return false; }
        }

        public static bool GetCurrentInventory_OLD()
        {
            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            CultureInfo ct = new CultureInfo("");
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            ct.DateTimeFormat = dtFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture = ct;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

            try
            {

                //Wipe out
                Query qr = InventoryHdr.CreateQuery();
                qr.QueryType = QueryType.Delete;
                qr.Execute();

                qr = StockTake.CreateQuery();
                qr.QueryType = QueryType.Delete;
                qr.Execute();

                bool TotalResult = true, result; int i;
                string[] tableName = { "InventoryHdr", "InventoryDet", "StockTake" };
                string[] primaryKeyName = { "InventoryHdrRefNo", "InventoryDetRefNo", "StockTakeID" };
                bool[] IsPKAutoGenerated = { false, false, true };
                bool IsSyncAll;

                for (int k = 0; k < tableName.Length; k++)
                {
                    i = 0; result = false;

                    IsSyncAll = true;

                    while (!result & i < RETRY_LIMIT)
                    {
                        result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                        //result = GetItem(syncAll);
                        i += 1;
                    }
                    TotalResult &= result;
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;


                return TotalResult;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;

                return false;
            }

        }

        /// <summary>
        /// Fix past data: 
        /// Generate InventoryHdr for all sales that is marked as "ADJUSTED", so the "Adjusted" status can be transfered to Client's POS database
        /// </summary>
        /// <remarks>
        /// Function: 
        /// - To make sure the ADJUSTED status is transfered down to Client's computer
        /// - To fix past data only. The recent/future data is handled in OD.DoStockOut()
        /// </remarks>
        /// <returns></returns>
        public static bool GenerateInventoryHdrForAdjustedSales()
        {
            try
            {
                string SQLString =
                    "INSERT INTO [InventoryHdr] " +
                        "([InventoryHdrRefNo],[InventoryDate],[UserName],[MovementType],[StockOutReasonID],[ExchangeRate],[PurchaseOrderNo],[InvoiceNo] " +
                        ",[DeliveryOrderNo],[Supplier],[FreightCharge],[DeliveryCharge],[Tax],[Discount],[Remark],[InventoryLocationID],[DepartmentID] " +
                        ",[CreatedOn],[ModifiedOn],[CreatedBy],[ModifiedBy],[UniqueID],[Deleted]) " +
                    "SELECT 'AD' + REPLACE(OrderDetID, '.', ''), OrderDetDate, 'SYSTEM', 'Stock Out', 0, 1, '', OrderDetID, NULL, '', 0, NULL, NULL, 0 " +
                        ", 'Adjusted Sales', LI.InventoryLocationID, NULL, GETDATE(), GETDATE(), 'Adjustment Script', 'Adjustment Script', NEWID(), 0 " +
                    "FROM OrderDet OD " +
                        "INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID " +
                        "INNER JOIN PointOfSale LP ON OH.PointOfSaleID = LP.PointOfSaleID " +
                        "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                        "INNER JOIN InventoryLocation LI ON LO.InventoryLocationID = LI.InventoryLocationID " +
                    "WHERE OD.InventoryHdrRefNo = 'ADJUSTED' " +
                        "AND OrderDetID NOT IN (SELECT InvoiceNo FROM InventoryHdr WHERE InventoryHdrRefNo LIKE 'AD%') ";

                DataService.ExecuteQuery(new QueryCommand(SQLString, "PowerPOS"));

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Generate InventoryHdr from Past \"Adjusted Sales\" failed!\n" + ex.Message);
                return false;
            }
        }

        public static bool UpdateOrderDetFromDownloadedInventoryHdr()
        {
            try
            {
                string SqlUpdateOrderDetFromInventory = "";
                #region *) SQL String: Update OrderDet From Data in InventoryHdr
                SqlUpdateOrderDetFromInventory =
                    "UPDATE OrderDet " +
                    "SET InventoryHdrRefNo = A.InventoryHdrRefNo " +
                    "FROM InventoryHdr A " +
                    "WHERE (OrderDet.InventoryHdrRefNo IS NULL OR OrderDet.InventoryHdrRefNo = '') AND A.InvoiceNo = OrderDetID " +
                        "AND A.InventoryHdrRefNo NOT LIKE 'AD%' " /* If it is adjustment, Mark it as ADJUSTED */
                        ;
                #endregion

                string SqlUpdateOrderDetAdjusted = "";
                #region *) SQL String: Update OrderDet if the InventoryHdr is marked as adjusted
                SqlUpdateOrderDetAdjusted =
                    "UPDATE OrderDet " +
                    "SET InventoryHdrRefNo = 'ADJUSTED' " +
                    "FROM InventoryHdr A " +
                    "WHERE (OrderDet.InventoryHdrRefNo IS NULL OR OrderDet.InventoryHdrRefNo = '') AND A.InvoiceNo = OrderDetID " +
                        "AND A.InventoryHdrRefNo LIKE 'AD%' " /* If it is adjustment, Mark it as ADJUSTED */
                        ;
                #endregion

                string SqlUpdateOrderDetNonInventory = "";
                #region *) SQL String: Update OrderDet if the Item is Not In Inventory
                SqlUpdateOrderDetNonInventory =
                    "UPDATE OrderDet " +
                    "SET InventoryHdrRefNo = 'NONINVENTORY' " +
                    "WHERE (InventoryHdrRefNo IS NULL OR InventoryHdrRefNo = '') AND ItemNo IN (SELECT ItemNo FROM Item WHERE IsInInventory = 0 and ISNULL(userflag7,0) = 0)";
                #endregion

                QueryCommandCollection Cmds = new QueryCommandCollection();
                Cmds.Add(new QueryCommand(SqlUpdateOrderDetFromInventory));
                Cmds.Add(new QueryCommand(SqlUpdateOrderDetAdjusted));
                Cmds.Add(new QueryCommand(SqlUpdateOrderDetNonInventory));

                UpdateAdjustedOrder();


                DataService.ExecuteTransaction(Cmds);


                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);

                return false;
            }

        }

        public static byte[] CompressDataSetToByteArray(DataSet myDataSet)
        {
            MemoryStream memStream = new MemoryStream();

            GZipStream zipStream = new GZipStream(memStream, CompressionMode.Compress);

            myDataSet.WriteXml(zipStream, XmlWriteMode.WriteSchema);

            zipStream.Close();

            byte[] data = memStream.ToArray();
            memStream.Close();
            myDataSet.Dispose();
            return data;

        }

        //public static bool GetCurrentInventory()
        //{
        //    try
        //    {
        //        Logger.writeLog("GetCurrentInventory_Draft1 - Start");

        //        #region Init Web Service (ws)
        //        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
        //        ws.Url = WS_URL;
        //        #endregion

        //        #region Get App Setting for Change Tracking Version (appSettingCTVersion)
        //        string appSettingCTVersionStr = AppSetting.GetSetting(ChangeTrackingLastVersionName);
        //        long appSettingCTVersion = -1;
        //        long.TryParse(appSettingCTVersionStr, out appSettingCTVersion);
        //        if (appSettingCTVersion == -1)
        //        {
        //            throw new Exception("App Setting Change Tracking is Error");
        //        }
        //        #endregion

        //        #region Get Server Change Tracking Current Version (-1 for error) (serverCTVersion)
        //        long serverCTVersion = ws.GetChangeTrackingCurrentVersion();
        //        #endregion

        //        #region Get Version Sync Status (-1 for error, 1 for sync-all, 2 for partial-sync) (syncStatus)
        //        int syncStatus = -1;
        //        if (appSettingCTVersionStr != null)
        //        {
        //            syncStatus = ws.GetChangeTrackingVersionSyncStatus(appSettingCTVersion);
        //        }
        //        #endregion

        //        #region First Time Sync for NULL AppSetting
        //        if (appSettingCTVersionStr == null)
        //        {
        //            // Sync All
        //            syncStatus = 1;
        //        }
        //        #endregion

        //        #region Throw Error for Mismatched syncStatus and serverCTVersion
        //        if (syncStatus == -1)
        //        {
        //            throw new Exception("Sync Status is -1, Error");
        //        }
        //        if (serverCTVersion == -1)
        //        {
        //            throw new Exception("Server Change Tracking Version is -1, Error");
        //        }
        //        #endregion

        //        if (syncStatus == 1)
        //        {
        //            #region Sync All, If Version is NULL or 0

        //            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
        //            CultureInfo ct = new CultureInfo("");
        //            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
        //            dtFormat.ShortDatePattern = "yyyy-MM-dd";
        //            ct.DateTimeFormat = dtFormat;
        //            System.Threading.Thread.CurrentThread.CurrentCulture = ct;
        //            System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

        //            try
        //            {
        //                //Wipe out
        //                Query qr = InventoryHdr.CreateQuery();
        //                qr.QueryType = QueryType.Delete;
        //                qr.Execute();

        //                qr = StockTake.CreateQuery();
        //                qr.QueryType = QueryType.Delete;
        //                qr.Execute();

        //                qr = LocationTransfer.CreateQuery();
        //                qr.QueryType = QueryType.Delete;
        //                qr.Execute();

        //                bool TotalResult = true, result; int i;
        //                string[] tableName = { "InventoryHdr", "InventoryDet", "StockTake", "LocationTransfer" };
        //                string[] primaryKeyName = { "InventoryHdrRefNo", "InventoryDetRefNo", "StockTakeID", "LocationTransferID" };
        //                bool[] IsPKAutoGenerated = { false, false, true, false, false, true };
        //                bool IsSyncAll;

        //                for (int k = 0; k < tableName.Length; k++)
        //                {
        //                    i = 0; result = false;

        //                    IsSyncAll = true;

        //                    while (!result & i < RETRY_LIMIT)
        //                    {
        //                        result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
        //                        //result = GetItem(syncAll);
        //                        i += 1;
        //                    }
        //                    TotalResult &= result;
        //                }

        //                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        //                System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;

        //                #region Update Current Version to Client AppSetting Table
        //                string sqlUpdateAppSetting = "IF EXISTS ( SELECT AppSettingKey FROM AppSetting " +
        //                    "WHERE AppSettingKey = @ChangeTrackingLastVersionName ) " +
        //                    "UPDATE AppSetting SET AppSettingValue = @serverCTVersion, ModifiedBy = 'SCRIPT', " +
        //                    "ModifiedOn = GETDATE() " +
        //                    "WHERE AppSettingKey = @ChangeTrackingLastVersionName " +
        //                    "ELSE " +
        //                    "INSERT INTO AppSetting (AppSettingKey, AppSettingValue, CreatedOn, CreatedBy) " +
        //                    "VALUES (@ChangeTrackingLastVersionName, @serverCTVersion, GETDATE(), 'SCRIPT')";
        //                QueryCommand updateAppSetting = new QueryCommand(sqlUpdateAppSetting);
        //                updateAppSetting.Parameters.Add(new QueryParameter()
        //                {
        //                    ParameterName = "serverCTVersion",
        //                    ParameterValue = serverCTVersion
        //                });
        //                updateAppSetting.Parameters.Add(new QueryParameter()
        //                {
        //                    ParameterName = "ChangeTrackingLastVersionName",
        //                    ParameterValue = ChangeTrackingLastVersionName
        //                });
        //                DataService.ExecuteQuery(updateAppSetting);
        //                #endregion

        //                Logger.writeLog("GetCurrentInventory_Draft1 - Finished (Out syncStatus = 1)");
        //                return TotalResult;
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.writeLog("GetCurrentInventory_Draft1 - Exception (syncStatus = 1)");
        //                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
        //                System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;
        //                throw ex;
        //            }

        //            #endregion
        //        }
        //        else if (syncStatus == 2)
        //        {
        //            #region Partial Sync

        //            #region Fetch Changes (dsChangesInventoryHdr, dsChangesInventoryDet, dsChangesStockTake)
        //            DataSet dsChangesInventoryHdr = new DataSet();
        //            DataSet dsChangesInventoryDet = new DataSet();
        //            DataSet dsChangesStockTake = new DataSet();
        //            DataSet dsLocationTransfer = new DataSet();
        //            for (int i = 0; i < RETRY_LIMIT; i++)
        //            {
        //                try
        //                {
        //                    dsChangesInventoryHdr = DecompressDataSetFromByteArray(ws.GetChangeTrackingChangesCompressed("InventoryHdr", appSettingCTVersion, "InventoryHdrRefNo"));
        //                    break;
        //                }
        //                catch (Exception) { }
        //            }
        //            for (int i = 0; i < RETRY_LIMIT; i++)
        //            {
        //                try
        //                {
        //                    dsChangesInventoryDet = DecompressDataSetFromByteArray(ws.GetChangeTrackingChangesCompressed("InventoryDet", appSettingCTVersion, "InventoryDetRefNo"));
        //                    break;
        //                }
        //                catch (Exception) { }
        //            }
        //            for (int i = 0; i < RETRY_LIMIT; i++)
        //            {
        //                try
        //                {
        //                    dsChangesStockTake = DecompressDataSetFromByteArray(ws.GetChangeTrackingChangesCompressed("StockTake", appSettingCTVersion, "StockTakeID"));
        //                    break;
        //                }
        //                catch (Exception) { }
        //            }
        //            for (int i = 0; i < RETRY_LIMIT; i++)
        //            {
        //                try
        //                {
        //                    dsLocationTransfer = DecompressDataSetFromByteArray(ws.GetChangeTrackingChangesCompressed("LocationTransfer", appSettingCTVersion, "LocationTransferID"));
        //                    break;
        //                }
        //                catch (Exception) { }
        //            }
        //            #endregion

        //            QueryCommandCollection cmdCol = new QueryCommandCollection();
        //            cmdCol.AddRange(TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsChangesInventoryHdr, false));
        //            cmdCol.AddRange(TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsChangesInventoryDet, false));
        //            cmdCol.AddRange(TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsChangesStockTake, true));
        //            cmdCol.AddRange(TransformDataSetChangeTrackingChangesToQueryCommandCollection(dsLocationTransfer, true));

        //            #region Update Current Version to Client AppSetting Table
        //            string sqlUpdateAppSetting = "IF EXISTS ( SELECT AppSettingKey FROM AppSetting " +
        //                "WHERE AppSettingKey = @ChangeTrackingLastVersionName ) " +
        //                "UPDATE AppSetting SET AppSettingValue = @serverCTVersion, ModifiedBy = 'SCRIPT', " +
        //                "ModifiedOn = GETDATE() " +
        //                "WHERE AppSettingKey = @ChangeTrackingLastVersionName " +
        //                "ELSE " +
        //                "INSERT INTO AppSetting (AppSettingKey, AppSettingValue, CreatedOn, CreatedBy) " +
        //                "VALUES (@ChangeTrackingLastVersionName, @serverCTVersion, GETDATE(), 'SCRIPT')";
        //            QueryCommand updateAppSetting = new QueryCommand(sqlUpdateAppSetting);
        //            updateAppSetting.Parameters.Add(new QueryParameter()
        //            {
        //                ParameterName = "serverCTVersion",
        //                ParameterValue = serverCTVersion
        //            });
        //            updateAppSetting.Parameters.Add(new QueryParameter()
        //            {
        //                ParameterName = "ChangeTrackingLastVersionName",
        //                ParameterValue = ChangeTrackingLastVersionName
        //            });
        //            cmdCol.Add(updateAppSetting);
        //            #endregion

        //            DataService.ExecuteTransaction(cmdCol);

        //            #endregion

        //            Logger.writeLog("GetCurrentInventory_Draft1 - Finished (syncStatus = 2)");
        //            return true;
        //        }
        //        else if (syncStatus == 3)
        //        {
        //            // no need to sync
        //            Logger.writeLog("GetCurrentInventory_Draft1 - Finished (syncStatus = 3)");
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.writeLog("GetCurrentInventory_Draft1 - Exception");
        //        Logger.writeLog(ex);

        //        // if FK conflict then throw error
        //        if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
        //        {
        //            if (((System.Data.SqlClient.SqlException)ex).Number == 547)
        //            {
        //                throw ex;
        //            }
        //        }

        //        return false;
        //    }
        //}
        private static bool getLatestInventoryModifiedOnLocal(out DateTime result)
        {
            result = DateTime.Now;
            string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') from InventoryHdr ";
            object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
            if (obj != null)
            {
                if (obj != null && obj.ToString() != "")
                    result = (DateTime)obj;
                else
                    return false;

                return true;

            }
            return true;
        }

        private static bool getLatestModifiedOnLocal(string tablename, out DateTime result)
        {
            result = DateTime.Now;
            string sqlString = "Select ISNULL(max(modifiedon),'2011-01-01') from " +tablename;
            object obj = DataService.ExecuteScalar(new QueryCommand(sqlString));
            if (obj != null)
            {
                if (obj != null && obj.ToString() != "")
                    result = (DateTime)obj;
                else
                    return false;

                return true;

            }
            return true;
        }

        public static bool GetCurrentInventoryRealTime()
        {
            try
            {
                try
                {
                    #region Load Config
                    Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = WS_URL;
                    DateTime lastSalesModifiedOn = DateTime.Today;
                    #endregion
                    DateTime LastModifiedOn = DateTime.Today.AddDays(-7);
                    
                    int recordsperTime = 100;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventorySyncRecordsPerTime), out recordsperTime))
                    {
                        recordsperTime = 100;
                    }
                        
                    if (!getLatestInventoryModifiedOnLocal(out LastModifiedOn))
                    {
                        return false;
                    }

                    int dataGet = 0;
                    int totalRecords = ws.GetInventoryCountRealTime(LastModifiedOn);
                    while (dataGet < totalRecords)
                    {
                        byte[] data = ws.GetInventoryRealTimeData(LastModifiedOn, recordsperTime);

                        DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);

                        QueryCommandCollection cmd = new QueryCommandCollection();
                        InventoryHdrCollection InventoryHdrs = new InventoryHdrCollection();
                        InventoryHdrs.Load(ds.Tables[0]);
                        
                        if (InventoryHdrs.Count > 0)
                        {
                            Query qry = new Query("InventoryHdr");
                            Where whr = new Where();
                            whr.ColumnName = "InventoryHdrRefNo";
                            whr.Comparison = Comparison.Equals;
                            whr.ParameterName = "@InventoryHdrRefNo";

                            SubSonic.TableSchema.TableColumnCollection ColumnList = new SubSonic.TableSchema.TableColumnCollection();
                            ColumnList.AddRange(DataService.GetSchema("InventoryHdr", "PowerPOS").Columns);

                            for (int i = 0; i < InventoryHdrs.Count; i++)
                            {
                                //Get count....
                                InventoryHdrs[i].DirtyColumns.AddRange(ColumnList);
                                whr.ParameterValue = InventoryHdrs[i].InventoryHdrRefNo;
                                QueryCommand mycmd;
                                if (qry.GetCount(InventoryHdr.Columns.InventoryHdrRefNo, whr) > 0)
                                {
                                    mycmd = InventoryHdrs[i].GetUpdateCommand("SYNC");
                                    mycmd.Parameters.GetParameter("@ModifiedOn").ParameterValue = InventoryHdrs[i].ModifiedOn;
                                    mycmd.CommandSql = mycmd.CommandSql.Remove(mycmd.CommandSql.IndexOf("[ModifiedOn]"), 55);
                                    cmd.Add(mycmd);
                                }
                                else
                                {
                                    mycmd = InventoryHdrs[i].GetInsertCommand("SYNC");
                                    mycmd.Parameters.GetParameter("@ModifiedOn").ParameterValue = InventoryHdrs[i].ModifiedOn;
                                    cmd.Add(mycmd);
                                }

                            }

                        }


                        #region *) Inventory Det
                        InventoryDetCollection InventoryDets = new InventoryDetCollection();
                        InventoryDets.Load(ds.Tables[1]);

                        if (InventoryDets.Count > 0)
                        {
                            Query qry = new Query("InventoryDet");
                            Where whr = new Where();
                            whr.ColumnName = "InventoryDetRefNo";
                            whr.Comparison = Comparison.Equals;
                            whr.ParameterName = "@InventoryDetRefNo";

                            SubSonic.TableSchema.TableColumnCollection ColumnList = new SubSonic.TableSchema.TableColumnCollection();
                            ColumnList.AddRange(DataService.GetSchema("InventoryDet", "PowerPOS").Columns);

                            for (int i = 0; i < InventoryDets.Count; i++)
                            {
                                //Get count....
                                InventoryDets[i].DirtyColumns.AddRange(ColumnList);
                                whr.ParameterValue = InventoryDets[i].InventoryDetRefNo;
                                QueryCommand mycmd;
                                if (qry.GetCount(InventoryDet.Columns.InventoryDetRefNo, whr) > 0)
                                {
                                    mycmd = InventoryDets[i].GetUpdateCommand("SYNC");
                                    mycmd.CommandSql = mycmd.CommandSql.Remove(mycmd.CommandSql.IndexOf("[ModifiedBy]"), 55);
                                    cmd.Add(mycmd);
                                }
                                else
                                {
                                    mycmd = InventoryDets[i].GetInsertCommand("SYNC");
                                    cmd.Add(mycmd);
                                }

                            }
                        }
                        #endregion

                        if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                        dataGet += recordsperTime;
                    }
                    return true;
                }
                catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

        public static bool GetStockTakeRealTime()
        {
            try
            {
                try
                {
                    #region Load Config
                    Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = WS_URL;
                    DateTime lastSalesModifiedOn = DateTime.Today;
                    #endregion
                    DateTime LastModifiedOn = DateTime.Today.AddDays(-7);

                    int recordsperTime = 100;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventorySyncRecordsPerTime), out recordsperTime))
                    {
                        recordsperTime = 100;
                    }

                    if (!getLatestModifiedOnLocal("StockTake", out LastModifiedOn))
                    {
                        return false;
                    }

                    byte[] data = ws.GetStockTakeRealTimeData(LastModifiedOn, recordsperTime);

                    DataSet ds = SyncClientController.DecompressDataSetFromByteArray(data);

                    QueryCommandCollection cmd = new QueryCommandCollection();
                    

                    #region *) Stock Take
                    StockTakeCollection StockTakes = new StockTakeCollection();
                    StockTakes.Load(ds.Tables[0]);

                    if (StockTakes.Count > 0)
                    {
                        Query qry = new Query("StockTake");
                        Where whr = new Where();
                        whr.ColumnName = "StockTakeID";
                        whr.Comparison = Comparison.Equals;
                        whr.ParameterName = "@StockTakeID";

                        SubSonic.TableSchema.TableColumnCollection ColumnList = new SubSonic.TableSchema.TableColumnCollection();
                        ColumnList.AddRange(DataService.GetSchema("StockTake", "PowerPOS").Columns);
                        string IdentInsertString = "SET IDENTITY_INSERT StockTake ON";
                        cmd.Add(new QueryCommand(IdentInsertString));
                        for (int i = 0; i < StockTakes.Count; i++)
                        {
                            //Get count....
                            StockTakes[i].DirtyColumns.AddRange(ColumnList);
                            whr.ParameterValue = StockTakes[i].StockTakeID;
                            QueryCommand mycmd;
                            if (qry.GetCount(StockTake.Columns.StockTakeID, whr) > 0)
                            {
                                mycmd = StockTakes[i].GetUpdateCommand("SYNC");
                                mycmd.CommandSql = mycmd.CommandSql.Remove(mycmd.CommandSql.IndexOf("[ModifiedOn]"), 55);
                                mycmd.Parameters.GetParameter("@ModifiedOn").ParameterValue = StockTakes[i].ModifiedOn;
                                cmd.Add(mycmd);
                            }
                            else
                            {
                                
                                mycmd = StockTakes[i].GetInsertCommand("SYNC");
                                mycmd.Parameters.GetParameter("@ModifiedOn").ParameterValue = StockTakes[i].ModifiedOn;
                                cmd.Add(mycmd);
                            }

                        }
                        IdentInsertString = "SET IDENTITY_INSERT StockTake OFF";
                        cmd.Add(new QueryCommand(IdentInsertString));
                    }
                    #endregion

                    if (cmd.Count > 0) DataService.ExecuteTransaction(cmd);
                    return true;

                }
                catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); return false; }
        }

       


        /// <summary>
        /// With SupplierItemMap Update
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentInventory()
        {
            try
            {
                SupplierItemMap.CreateSupplierItemMapTable();

                // Init Web Service
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                // Get App Setting (on Client)
                string lastSync = AppSetting.GetSetting("ChangeTrackingLastVersion");
                long longLastSync = -1;
                long.TryParse(lastSync, out longLastSync); // longLastSync will be 0 if lastSync is NULL

                // Get Server CT Version (on Server)
                long? currentCTVersion = ws.GetChangeTrackingVersion();
                if (currentCTVersion == null)
                {
                    ws.EnableChangeTracking();
                }
                /*if (ws.GetChangeTrackingTableMinValidVersion("InventoryHdr") == null)
                {
                    ws.EnableChangeTrackingTable("InventoryHdr");
                }
                if (ws.GetChangeTrackingTableMinValidVersion("InventoryDet") == null)
                {
                    ws.EnableChangeTrackingTable("InventoryDet");
                }*/
                if (ws.GetChangeTrackingTableMinValidVersion("StockTake") == null)
                {
                    ws.EnableChangeTrackingTable("StockTake");
                }
                if (ws.GetChangeTrackingTableMinValidVersion("LocationTransfer") == null)
                {
                    ws.EnableChangeTrackingTable("LocationTransfer");
                }
                if (ws.GetChangeTrackingTableMinValidVersion("SupplierItemMap") == null)
                {
                    ws.EnableChangeTrackingTable("SupplierItemMap");
                }


                // Get Table Min Valid
                //long? inventoryHdrMinValid = ws.GetChangeTrackingTableMinValidVersion("InventoryHdr");
                long? inventoryHdrMinValid = ws.GetChangeTrackingTableMinValidVersion("StockTake");

                if (lastSync == null)
                {
                    QueryCommandCollection cmdCol = new QueryCommandCollection();

                    // Delete Existing Table
                    /*Query deleteInventoryHdr = InventoryHdr.CreateQuery();
                    deleteInventoryHdr.QueryType = QueryType.Delete;
                    deleteInventoryHdr.Execute();
                    cmdCol.Add(deleteInventoryHdr.BuildCommand());*/

                    Query deleteStockTake = StockTake.CreateQuery();
                    deleteStockTake.QueryType = QueryType.Delete;
                    deleteStockTake.Execute();
                    cmdCol.Add(deleteStockTake.BuildCommand());

                    Query deleteLocationTransfer = LocationTransfer.CreateQuery();
                    deleteLocationTransfer.QueryType = QueryType.Delete;
                    deleteLocationTransfer.Execute();
                    cmdCol.Add(deleteLocationTransfer.BuildCommand());

                    cmdCol.Add(SupplierItemMap.WipeOutTable());

                    // Download Whole Table
                    DataSet dsInventoryHdr = new DataSet();
                    DataSet dsInventoryDet = new DataSet();
                    DataSet dsStockTake = new DataSet();
                    DataSet dsLocationTransfer = new DataSet();
                    DataSet dsSupplierItemMap = new DataSet();
                    /*for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("InventoryHdr", "InventoryHdrRefNo"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("InventoryDet", "InventoryDetRefNo"));
                            break;
                        }
                        catch (Exception) { }
                    }*/
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("StockTake", "StockTakeID"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("LocationTransfer", "LocationTransferID"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsSupplierItemMap = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("SupplierItemMap", "UniqueID"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    if (! //(dsInventoryHdr.Tables.Count > 0) && !(dsInventoryDet.Tables.Count > 0)&&
                         !(dsStockTake.Tables.Count > 0) && !(dsLocationTransfer.Tables.Count > 0)
                        && !(dsSupplierItemMap.Tables.Count > 0))
                    {
                        throw new Exception("Cannot Fetch Table.");
                    }

                    // Transform DataSet to QueryCommandCollection
                    //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                    //    dsInventoryHdr, false));
                    //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                    //    dsInventoryDet, false));
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        dsStockTake, true));
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(
                        dsLocationTransfer));
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        dsSupplierItemMap, false));

                    // Execute Transaction
                    DataService.ExecuteTransaction(cmdCol);

                    // Update App Setting
                    AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());
                }
                else
                {
                    if (longLastSync >= inventoryHdrMinValid && longLastSync != currentCTVersion)
                    {
                        // Partial Sync
                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        // Download Changes Table
                        DataSet dsInventoryHdr = new DataSet();
                        DataSet dsInventoryDet = new DataSet();
                        DataSet dsStockTake = new DataSet();
                        DataSet dsLocationTransfer = new DataSet();
                        DataSet dsSupplierItemMap = new DataSet();
                        /*for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("InventoryHdr", longLastSync, "InventoryHdrRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("InventoryDet", longLastSync, "InventoryDetRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }*/
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("StockTake", longLastSync, "StockTakeID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("LocationTransfer", longLastSync, "LocationTransferID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsSupplierItemMap = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("SupplierItemmap", longLastSync, "UniqueID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        if (! //(dsInventoryHdr.Tables.Count > 0) && !(dsInventoryDet.Tables.Count > 0) &&
                             !(dsStockTake.Tables.Count > 0) && !(dsLocationTransfer.Tables.Count > 0)
                            && !(dsSupplierItemMap.Tables.Count > 0))
                        {
                            throw new Exception("Cannot Fetch Table.");
                        }

                        // Transform DataSet to QueryCommandCollection
                        //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        //    dsInventoryHdr, false));
                        //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        //    dsInventoryDet, false));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsStockTake, true));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(
                            dsLocationTransfer));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsSupplierItemMap, false));

                        foreach (QueryCommand cmd1 in cmdCol)
                        {
                            if (cmd1.CommandSql.StartsWith("INSERT"))
                            {
                                Logger.writeLog("Insert Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                            }
                            else
                                if (cmd1.CommandSql.StartsWith("UPDATE"))
                                {
                                    Logger.writeLog("Update Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                                }
                                else
                                {
                                    Logger.writeLog(cmd1.CommandSql.Substring(0, cmd1.CommandSql.Length > 10 ? 10 : cmd1.CommandSql.Length - 1));
                                }
                        }

                        // Execute Transaction
                        DataService.ExecuteTransaction(cmdCol);

                        // Update App Setting
                        AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());
                    }
                    else if (longLastSync < inventoryHdrMinValid && longLastSync != currentCTVersion)
                    {
                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        // Delete Existing Table
                        /*Query deleteInventoryHdr = InventoryHdr.CreateQuery();
                        deleteInventoryHdr.QueryType = QueryType.Delete;
                        deleteInventoryHdr.Execute();
                        cmdCol.Add(deleteInventoryHdr.BuildCommand());*/

                        Query deleteStockTake = StockTake.CreateQuery();
                        deleteStockTake.QueryType = QueryType.Delete;
                        deleteStockTake.Execute();
                        cmdCol.Add(deleteStockTake.BuildCommand());

                        Query deleteLocationTransfer = LocationTransfer.CreateQuery();
                        deleteLocationTransfer.QueryType = QueryType.Delete;
                        deleteLocationTransfer.Execute();
                        cmdCol.Add(deleteLocationTransfer.BuildCommand());

                        cmdCol.Add(SupplierItemMap.WipeOutTable());

                        // Download Whole Table
                        DataSet dsInventoryHdr = new DataSet();
                        DataSet dsInventoryDet = new DataSet();
                        DataSet dsStockTake = new DataSet();
                        DataSet dsLocationTransfer = new DataSet();
                        DataSet dsSupplierItemMap = new DataSet();
                        /*for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("InventoryHdr", "InventoryHdrRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("InventoryDet", "InventoryDetRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }*/
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("StockTake", "StockTakeID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("LocationTransfer", "LocationTransferID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsSupplierItemMap = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("SupplierItemMap", "UniqueID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        if (!//(dsInventoryHdr.Tables.Count > 0) && !(dsInventoryDet.Tables.Count > 0) &&
                             !(dsStockTake.Tables.Count > 0) && !(dsLocationTransfer.Tables.Count > 0)
                            && !(dsSupplierItemMap.Tables.Count > 0))
                        {
                            throw new Exception("Cannot Fetch Table.");
                        }

                        // Transform DataSet to QueryCommandCollection
                        /*cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsInventoryHdr, false));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsInventoryDet, false));*/
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsStockTake, true));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(
                            dsLocationTransfer));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsSupplierItemMap, false));


                        foreach (QueryCommand cmd1 in cmdCol)
                        {
                            if (cmd1.CommandSql.StartsWith("Insert"))
                            {
                                Logger.writeLog("Insert Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                            }
                            else
                                if (cmd1.CommandSql.StartsWith("Update"))
                                {
                                    Logger.writeLog("Update Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                                }
                                else
                                {
                                    Logger.writeLog(cmd1.CommandSql.Substring(0, cmd1.CommandSql.Length > 10 ? 10 : cmd1.CommandSql.Length - 1));
                                }
                        }
                        // Execute Transaction
                        DataService.ExecuteTransaction(cmdCol);

                        // Update App Setting
                        AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());
                    }
                    else if (longLastSync == currentCTVersion)
                    {
                        // No Sync
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        /// <summary>
        /// With SupplierItemMap Update
        /// </summary>
        /// <returns></returns>
        public static bool GetCurrentInventoryNew()
        {
            try
            {
                SupplierItemMap.CreateSupplierItemMapTable();

                // Init Web Service
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                // Get App Setting (on Client)
                string lastSync = AppSetting.GetSetting("ChangeTrackingLastVersion");
                long longLastSync = -1;
                long.TryParse(lastSync, out longLastSync); // longLastSync will be 0 if lastSync is NULL

                // Get Server CT Version (on Server)
                long? currentCTVersion = ws.GetChangeTrackingVersion();
                if (currentCTVersion == null)
                {
                    ws.EnableChangeTracking();
                }
                /*if (ws.GetChangeTrackingTableMinValidVersion("InventoryHdr") == null)
                {
                    ws.EnableChangeTrackingTable("InventoryHdr");
                }
                if (ws.GetChangeTrackingTableMinValidVersion("InventoryDet") == null)
                {
                    ws.EnableChangeTrackingTable("InventoryDet");
                }*/
                if (ws.GetChangeTrackingTableMinValidVersion("StockTake") == null)
                {
                    ws.EnableChangeTrackingTable("StockTake");
                }
                if (ws.GetChangeTrackingTableMinValidVersion("LocationTransfer") == null)
                {
                    ws.EnableChangeTrackingTable("LocationTransfer");
                }
                if (ws.GetChangeTrackingTableMinValidVersion("SupplierItemMap") == null)
                {
                    ws.EnableChangeTrackingTable("SupplierItemMap");
                }


                // Get Table Min Valid
                //long? inventoryHdrMinValid = ws.GetChangeTrackingTableMinValidVersion("InventoryHdr");
                long? inventoryHdrMinValid = ws.GetChangeTrackingTableMinValidVersion("StockTake");

                if (lastSync == null)
                {
                    QueryCommandCollection cmdCol = new QueryCommandCollection();

                    // Delete Existing Table
                    /*Query deleteInventoryHdr = InventoryHdr.CreateQuery();
                    deleteInventoryHdr.QueryType = QueryType.Delete;
                    deleteInventoryHdr.Execute();
                    cmdCol.Add(deleteInventoryHdr.BuildCommand());*/

                    Query deleteStockTake = StockTake.CreateQuery();
                    deleteStockTake.QueryType = QueryType.Delete;
                    deleteStockTake.Execute();
                    cmdCol.Add(deleteStockTake.BuildCommand());

                    Query deleteLocationTransfer = LocationTransfer.CreateQuery();
                    deleteLocationTransfer.QueryType = QueryType.Delete;
                    deleteLocationTransfer.Execute();
                    cmdCol.Add(deleteLocationTransfer.BuildCommand());

                    cmdCol.Add(SupplierItemMap.WipeOutTable());

                    // Download Whole Table
                    //DataSet dsInventoryHdr = new DataSet();
                    //DataSet dsInventoryDet = new DataSet();
                    DataSet dsStockTake = new DataSet();
                    DataSet dsLocationTransfer = new DataSet();
                    DataSet dsSupplierItemMap = new DataSet();
                    /*for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("InventoryHdr", "InventoryHdrRefNo"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("InventoryDet", "InventoryDetRefNo"));
                            break;
                        }
                        catch (Exception) { }
                    }*/
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("StockTake", "StockTakeID"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("LocationTransfer", "LocationTransferID"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsSupplierItemMap = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable("SupplierItemMap", "UniqueID"));
                            break;
                        }
                        catch (Exception) { }
                    }
                    if (! //(dsInventoryHdr.Tables.Count > 0) && !(dsInventoryDet.Tables.Count > 0)&&
                         !(dsStockTake.Tables.Count > 0) && !(dsLocationTransfer.Tables.Count > 0)
                        && !(dsSupplierItemMap.Tables.Count > 0))
                    {
                        throw new Exception("Cannot Fetch Table.");
                    }

                    // Transform DataSet to QueryCommandCollection
                    //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                    //    dsInventoryHdr, false));
                    //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                    //    dsInventoryDet, false));
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        dsStockTake, true));
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(
                        dsLocationTransfer));
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        dsSupplierItemMap, false));

                    // Execute Transaction
                    DataService.ExecuteTransaction(cmdCol);

                    // Update App Setting
                    AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());
                }
                else
                {
                    if (longLastSync >= inventoryHdrMinValid && longLastSync != currentCTVersion)
                    {
                        // Partial Sync
                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        // Download Changes Table
                        //DataSet dsInventoryHdr = new DataSet();
                        //DataSet dsInventoryDet = new DataSet();
                        DataSet dsStockTake = new DataSet();
                        DataSet dsLocationTransfer = new DataSet();
                        DataSet dsSupplierItemMap = new DataSet();
                        /*for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("InventoryHdr", longLastSync, "InventoryHdrRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("InventoryDet", longLastSync, "InventoryDetRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }*/
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("StockTake", longLastSync, "StockTakeID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("LocationTransfer", longLastSync, "LocationTransferID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsSupplierItemMap = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable("SupplierItemmap", longLastSync, "UniqueID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        if (! //(dsInventoryHdr.Tables.Count > 0) && !(dsInventoryDet.Tables.Count > 0) &&
                             !(dsStockTake.Tables.Count > 0) && !(dsLocationTransfer.Tables.Count > 0)
                            && !(dsSupplierItemMap.Tables.Count > 0))
                        {
                            throw new Exception("Cannot Fetch Table.");
                        }

                        // Transform DataSet to QueryCommandCollection
                        //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        //    dsInventoryHdr, false));
                        //cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        //    dsInventoryDet, false));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsStockTake, true));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(
                            dsLocationTransfer));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsSupplierItemMap, false));

                        foreach (QueryCommand cmd1 in cmdCol)
                        {
                            if (cmd1.CommandSql.StartsWith("INSERT"))
                            {
                                Logger.writeLog("Insert Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                            }
                            else
                                if (cmd1.CommandSql.StartsWith("UPDATE"))
                                {
                                    Logger.writeLog("Update Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                                }
                                else
                                {
                                    Logger.writeLog(cmd1.CommandSql.Substring(0, cmd1.CommandSql.Length > 10 ? 10 : cmd1.CommandSql.Length - 1));
                                }
                        }

                        // Execute Transaction
                        DataService.ExecuteTransaction(cmdCol);

                        // Update App Setting
                        AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());
                    }
                    else if (longLastSync < inventoryHdrMinValid && longLastSync != currentCTVersion)
                    {
                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        // Delete Existing Table
                        /*Query deleteInventoryHdr = InventoryHdr.CreateQuery();
                        deleteInventoryHdr.QueryType = QueryType.Delete;
                        deleteInventoryHdr.Execute();
                        cmdCol.Add(deleteInventoryHdr.BuildCommand());*/

                        Query deleteStockTake = StockTake.CreateQuery();
                        deleteStockTake.QueryType = QueryType.Delete;
                        deleteStockTake.Execute();
                        cmdCol.Add(deleteStockTake.BuildCommand());

                        Query deleteLocationTransfer = LocationTransfer.CreateQuery();
                        deleteLocationTransfer.QueryType = QueryType.Delete;
                        deleteLocationTransfer.Execute();
                        cmdCol.Add(deleteLocationTransfer.BuildCommand());

                        cmdCol.Add(SupplierItemMap.WipeOutTable());

                        // Download Whole Table
                        DataSet dsInventoryHdr = new DataSet();
                        DataSet dsInventoryDet = new DataSet();
                        DataSet dsStockTake = new DataSet();
                        DataSet dsLocationTransfer = new DataSet();
                        DataSet dsSupplierItemMap = new DataSet();
                        /*for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryHdr = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("InventoryHdr", "InventoryHdrRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsInventoryDet = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("InventoryDet", "InventoryDetRefNo"));
                                break;
                            }
                            catch (Exception) { }
                        }*/
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsStockTake = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("StockTake", "StockTakeID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsLocationTransfer = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("LocationTransfer", "LocationTransferID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsSupplierItemMap = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable("SupplierItemMap", "UniqueID"));
                                break;
                            }
                            catch (Exception) { }
                        }
                        if (!//(dsInventoryHdr.Tables.Count > 0) && !(dsInventoryDet.Tables.Count > 0) &&
                             !(dsStockTake.Tables.Count > 0) && !(dsLocationTransfer.Tables.Count > 0)
                            && !(dsSupplierItemMap.Tables.Count > 0))
                        {
                            throw new Exception("Cannot Fetch Table.");
                        }

                        // Transform DataSet to QueryCommandCollection
                        /*cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsInventoryHdr, false));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsInventoryDet, false));*/
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsStockTake, true));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollectionWithUniqueIDAsPK(
                            dsLocationTransfer));
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsSupplierItemMap, false));


                        foreach (QueryCommand cmd1 in cmdCol)
                        {
                            if (cmd1.CommandSql.StartsWith("Insert"))
                            {
                                Logger.writeLog("Insert Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                            }
                            else
                                if (cmd1.CommandSql.StartsWith("Update"))
                                {
                                    Logger.writeLog("Update Inv - " + cmd1.Parameters[0].ParameterValue.ToString());
                                }
                                else
                                {
                                    Logger.writeLog(cmd1.CommandSql.Substring(0, cmd1.CommandSql.Length > 10 ? 10 : cmd1.CommandSql.Length - 1));
                                }
                        }
                        // Execute Transaction
                        DataService.ExecuteTransaction(cmdCol);

                        // Update App Setting
                        AppSetting.SetSetting("ChangeTrackingLastVersion", currentCTVersion.ToString());
                    }
                    else if (longLastSync == currentCTVersion)
                    {
                        // No Sync
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }


        private static QueryCommandCollection TransformDataSetChangeTrackingChangesToQueryCommandCollection(DataSet dataSet, bool IsPKAutoGenerated)
        {
            QueryCommandCollection cmdCol = new QueryCommandCollection();

            if (dataSet.Tables.Count > 0)
            {
                string tableName = dataSet.Tables[0].TableName;

                if (IsPKAutoGenerated)
                {
                    QueryCommand cmdIdentityOn = new QueryCommand("SET IDENTITY_INSERT " + tableName + " ON;");
                    cmdCol.Add(cmdIdentityOn);
                }

                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    // Build Column List, and Param List
                    string ColumnList = "";
                    string ParameterList = "";
                    List<string> Columns = new List<string>(), Parameters = new List<string>();
                    for (int i = 6; i < dataSet.Tables[0].Columns.Count - 1; i++)
                    {
                        ColumnList += dataSet.Tables[0].Columns[i].ColumnName + ",";
                        ParameterList += "@" + dataSet.Tables[0].Columns[i].ColumnName + ",";
                        Columns.Add(dataSet.Tables[0].Columns[i].ColumnName);
                        Parameters.Add("@" + dataSet.Tables[0].Columns[i].ColumnName);
                    }
                    ColumnList += dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName;
                    ParameterList += "@" + dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName;
                    Columns.Add(dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName);
                    Parameters.Add("@" + dataSet.Tables[0].Columns[dataSet.Tables[0].Columns.Count - 1].ColumnName);

                    // Iterate Rows and Build Command (Insert, Update, Delete)
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        string operation = string.Empty;
                        operation = dataSet.Tables[0].Rows[i]["SYS_CHANGE_OPERATION"].ToString().ToLower();

                        string sqlEachCTRow = string.Empty;
                        QueryCommand cmdEachCTRow = new QueryCommand(sqlEachCTRow);

                        if (operation == "i")
                        {
                            #region Operation Insert
                            sqlEachCTRow = "INSERT INTO " + tableName + " (" + ColumnList + ") VALUES (" + ParameterList + ")";

                            if (Columns.Count == Parameters.Count)
                            {
                                for (int j = 0; j < Columns.Count; j++)
                                {
                                    if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j],
                                            new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }
                                    else
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("TransformDataSetChangeTrackingChangesToQueryCommandCollection - Columns and Parameters Count is Different");
                            }

                            cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql text
                            #endregion
                        }
                        else if (operation == "u")
                        {
                            #region Operation Update
                            string PK = dataSet.Tables[0].Rows[i]["PK"].ToString();
                            sqlEachCTRow = "UPDATE " + tableName + " SET ";

                            if (Columns.Count == Parameters.Count)
                            {
                                for (int j = 0; j < Columns.Count - 1; j++)
                                {
                                    if (IsPKAutoGenerated)
                                    {
                                        if (Columns[j] == PK)
                                        {
                                            continue;
                                        }
                                    }

                                    sqlEachCTRow += Columns[j] + " = " + Parameters[j] + ", ";
                                    if (dataSet.Tables[0].Columns[Columns[j]].DataType == System.Type.GetType("System.Guid"))
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j],
                                            new Guid(dataSet.Tables[0].Rows[i][Columns[j]].ToString()),
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }
                                    else
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[j], dataSet.Tables[0].Rows[i][Columns[j]],
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[j].ToString()).DataType);
                                    }
                                }

                                if (IsPKAutoGenerated)
                                {
                                    if (Columns[Columns.Count - 1] == PK)
                                    {

                                    }
                                    else
                                    {
                                        sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];
                                        if (dataSet.Tables[0].Columns[Columns[Columns.Count - 1]].DataType == System.Type.GetType("System.Guid"))
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1],
                                                new Guid(dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]].ToString()),
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                        }
                                        else
                                        {
                                            cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1], dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]],
                                                DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                        }
                                    }
                                }
                                else
                                {
                                    sqlEachCTRow += Columns[Columns.Count - 1] + " = " + Parameters[Parameters.Count - 1];
                                    if (dataSet.Tables[0].Columns[Columns[Columns.Count - 1]].DataType == System.Type.GetType("System.Guid"))
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1],
                                            new Guid(dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]].ToString()),
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                    }
                                    else
                                    {
                                        cmdEachCTRow.Parameters.Add(Parameters[Columns.Count - 1], dataSet.Tables[0].Rows[i][Columns[Columns.Count - 1]],
                                            DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[Columns.Count - 1].ToString()).DataType);
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("TransformDataSetChangeTrackingChangesToQueryCommandCollection - Columns and Parameters Count is Different");
                            }

                            sqlEachCTRow += " WHERE " + PK + " = @" + PK;
                            if (dataSet.Tables[0].Columns[PK].DataType == System.Type.GetType("System.Guid"))
                            {
                                cmdEachCTRow.Parameters.Add("@" + PK,
                                    new Guid(dataSet.Tables[0].Rows[i][PK].ToString()),
                                    DataService.GetSchema(tableName, "PowerPOS").GetColumn(PK).DataType);
                            }
                            else
                            {
                                cmdEachCTRow.Parameters.Add("@" + PK, dataSet.Tables[0].Rows[i][PK],
                                    DataService.GetSchema(tableName, "PowerPOS").GetColumn(PK).DataType);
                            }

                            cmdEachCTRow.CommandSql = sqlEachCTRow; // rebind sql text
                            #endregion
                        }
                        else if (operation == "d")
                        {
                            // No Deletion Method
                        }

                        cmdCol.Add(cmdEachCTRow);
                    }
                }

                if (IsPKAutoGenerated)
                {
                    QueryCommand cmdIdentityOff = new QueryCommand("SET IDENTITY_INSERT " + tableName + " OFF;");
                    cmdCol.Add(cmdIdentityOff);
                }
            }

            return cmdCol;
        }

        public static void DeleteAppSetting(string appSettingKey)
        {
            try
            {
                // Init Web Service
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                ws.DeleteAppSettingKey(appSettingKey);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public static bool SyncTable(string tableName)
        {
            // DEBUG
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "CSDebug"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "CSDebug");
            }
            TextWriter newWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "CSDebug\\" + tableName + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".txt", true);
            newWriter.Flush();

            try
            {
                newWriter.WriteLine("Start Syncing " + tableName);

                // Init Web Service
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                // Get App Setting (on Client)
                QueryCommand getAppSetting = new QueryCommand("SELECT AppSettingValue FROM AppSetting WITH (NOLOCK) WHERE AppSettingKey = '" +
                    tableName + "_LastSyncVersion'", providerName);
                object lastSync = DataService.ExecuteScalar(getAppSetting);
                long longLastSync = -1;
                if (lastSync != null)
                {
                    long.TryParse(lastSync.ToString(), out longLastSync);
                }

                // Get Server CT Version (on Server)
                long? currentCTVersion = ws.GetChangeTrackingVersion();
                if (currentCTVersion == null)
                {
                    ws.EnableChangeTracking();
                }
                if (ws.GetChangeTrackingTableMinValidVersion(tableName) == null)
                {
                    ws.EnableChangeTrackingTable(tableName);
                }

                // Get Table Min Valid
                long? minValid = ws.GetChangeTrackingTableMinValidVersion(tableName);

                // Get Primary Key Column
                QueryCommand getPK = new QueryCommand("SELECT column_name " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                    "WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 " +
                    "AND table_name = '" + tableName + "'", providerName);
                object resultGetPK = DataService.ExecuteScalar(getPK);
                string primaryKeyName = string.Empty;

                if (resultGetPK != null)
                {
                    primaryKeyName = resultGetPK.ToString();
                }
                else
                {
                    throw new Exception(tableName + ": No Primary Key.");
                }

                // Get IsIdentity 
                QueryCommand getIsIdentity = new QueryCommand("SELECT is_identity " +
                    "FROM sys.columns " +
                    "WHERE object_id = object_id('" + tableName + "') " +
                    "AND name = '" + primaryKeyName + "'", providerName);
                object resultGetIsIdentity = DataService.ExecuteScalar(getIsIdentity);
                bool isIdentity = false;

                if (resultGetIsIdentity != null)
                {
                    isIdentity = bool.Parse(resultGetIsIdentity.ToString());
                }
                else
                {
                    throw new Exception(tableName + ": Identity Check Error.");
                }

                // DEBUG
                newWriter.WriteLine("Client Version: " + longLastSync.ToString());
                newWriter.WriteLine("Server Version : " + currentCTVersion.ToString());
                newWriter.WriteLine("Min Valid Version : " + minValid.ToString());
                newWriter.WriteLine("PK : " + primaryKeyName);
                newWriter.WriteLine("IsIdentity : " + isIdentity.ToString());
                newWriter.Flush();

                if (lastSync == null)
                {
                    QueryCommandCollection cmdCol = new QueryCommandCollection();

                    // Delete Existing Table
                    QueryCommand deleteTable = new QueryCommand("DELETE FROM " + tableName, providerName);
                    cmdCol.Add(deleteTable);

                    // Check for Cascade Delete, and delete app setting
                    QueryCommand cascadeCheckInside = new QueryCommand("EXEC sp_fkeys @pktable_name = N'" + tableName + "'", providerName);
                    DataSet dsCascadeCheckInside = DataService.GetDataSet(cascadeCheckInside);
                    if (dsCascadeCheckInside.Tables.Count > 0)
                    {
                        foreach (DataRow eachFK in dsCascadeCheckInside.Tables[0].Rows)
                        {
                            if (eachFK["DELETE_RULE"].ToString() == "0") // means cascade delete, then re-sync
                            {
                                AppSetting.DeleteSetting(eachFK["FKTABLE_NAME"].ToString() + "_LastSyncVersion");
                            }
                        }
                    }

                    // Download Whole Table
                    DataSet dsTable = new DataSet();
                    for (int i = 0; i < RETRY_LIMIT; i++)
                    {
                        try
                        {
                            dsTable = SyncClientController.DecompressDataSetFromByteArray(
                                ws.GetChangeTrackingFullTable(tableName, primaryKeyName));

                            break;
                        }
                        catch (Exception) { }
                    }

                    if (!(dsTable.Tables.Count > 0))
                    {
                        throw new Exception("Cannot Fetch Table.");
                    }

                    // DEBUG
                    foreach (DataRow eachRow in dsTable.Tables[0].Rows)
                    {
                        newWriter.WriteLine(eachRow["SYS_CHANGE_OPERATION"].ToString() + " - " + eachRow[primaryKeyName].ToString());
                        newWriter.Flush();
                    }
                    newWriter.WriteLine(dsTable.Tables[0].Rows.Count + " Rows");
                    newWriter.Flush();

                    // Transform DataSet to QueryCommandCollection
                    cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                        dsTable, isIdentity));

                    // Execute Transaction
                    DataService.ExecuteTransaction(cmdCol);

                    // Update App Setting
                    AppSetting.SetSetting(tableName + "_LastSyncVersion", currentCTVersion.ToString());
                }
                else
                {
                    if (longLastSync >= minValid && longLastSync != currentCTVersion)
                    {
                        // Partial Sync
                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        // Download Changes Table
                        DataSet dsTable = new DataSet();
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsTable = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingChangesTable(tableName, longLastSync, primaryKeyName));
                                break;
                            }
                            catch (Exception) { }
                        }

                        if (!(dsTable.Tables.Count > 0))
                        {
                            throw new Exception("Cannot Fetch Table.");
                        }

                        // DEBUG
                        foreach (DataRow eachRow in dsTable.Tables[0].Rows)
                        {
                            newWriter.WriteLine(eachRow["SYS_CHANGE_OPERATION"].ToString() + " - " + eachRow[primaryKeyName].ToString());
                            newWriter.Flush();
                        }
                        newWriter.WriteLine(dsTable.Tables[0].Rows.Count + " Rows");
                        newWriter.Flush();

                        // Transform DataSet to QueryCommandCollection
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsTable, isIdentity));

                        // Execute Transaction
                        DataService.ExecuteTransaction(cmdCol);

                        // Update App Setting
                        AppSetting.SetSetting(tableName + "_LastSyncVersion", currentCTVersion.ToString());
                    }
                    else if (longLastSync < minValid && longLastSync != currentCTVersion)
                    {
                        QueryCommandCollection cmdCol = new QueryCommandCollection();

                        // Delete Existing Table
                        QueryCommand deleteTable = new QueryCommand("DELETE FROM " + tableName, providerName);
                        cmdCol.Add(deleteTable);

                        // Check for Cascade Delete, and delete app setting
                        QueryCommand cascadeCheckInside = new QueryCommand("EXEC sp_fkeys @pktable_name = N'" + tableName + "'", providerName);
                        DataSet dsCascadeCheckInside = DataService.GetDataSet(cascadeCheckInside);
                        if (dsCascadeCheckInside.Tables.Count > 0)
                        {
                            foreach (DataRow eachFK in dsCascadeCheckInside.Tables[0].Rows)
                            {
                                if (eachFK["DELETE_RULE"].ToString() == "0") // means cascade delete, then re-sync
                                {
                                    AppSetting.Delete(eachFK["FKTABLE_NAME"].ToString() + "_LastSyncVersion");
                                }
                            }
                        }

                        // Download Whole Table
                        DataSet dsTable = new DataSet();
                        for (int i = 0; i < RETRY_LIMIT; i++)
                        {
                            try
                            {
                                dsTable = SyncClientController.DecompressDataSetFromByteArray(
                                    ws.GetChangeTrackingFullTable(tableName, primaryKeyName));
                                break;
                            }
                            catch (Exception) { }
                        }

                        if (!(dsTable.Tables.Count > 0))
                        {
                            throw new Exception("Cannot Fetch Table.");
                        }

                        // DEBUG
                        foreach (DataRow eachRow in dsTable.Tables[0].Rows)
                        {
                            newWriter.WriteLine(eachRow["SYS_CHANGE_OPERATION"].ToString() + " - " + eachRow[primaryKeyName].ToString());
                            newWriter.Flush();
                        }
                        newWriter.WriteLine(dsTable.Tables[0].Rows.Count + " Rows");
                        newWriter.Flush();

                        // Transform DataSet to QueryCommandCollection
                        cmdCol.AddRange(SQLChangeTracking.TransformDataSetChangeTrackingChangesToQueryCommandCollection(
                            dsTable, isIdentity));

                        // Execute Transaction
                        DataService.ExecuteTransaction(cmdCol);

                        // Update App Setting
                        AppSetting.SetSetting(tableName + "_LastSyncVersion", currentCTVersion.ToString());
                    }
                    else if (longLastSync == currentCTVersion)
                    {
                        // No Sync
                    }
                }

                // Recursive Cascade Sync
                QueryCommand cascadeCheck = new QueryCommand("EXEC sp_fkeys @pktable_name = N'" + tableName + "'", providerName);
                DataSet dsCascadeCheck = DataService.GetDataSet(cascadeCheck);
                if (dsCascadeCheck.Tables.Count > 0)
                {
                    foreach (DataRow eachFK in dsCascadeCheck.Tables[0].Rows)
                    {
                        if (eachFK["DELETE_RULE"].ToString() == "0") // means cascade delete, then re-sync
                        {
                            SyncTable(eachFK["FKTABLE_NAME"].ToString());
                        }
                    }
                }

                // DEBUG
                newWriter.WriteLine(tableName + " Sync Completed");

                newWriter.Flush();
                newWriter.Close();

                return true;
            }
            catch (Exception ex)
            {
                // DEBUG
                newWriter.WriteLine(tableName + " Sync Error");
                newWriter.WriteLine(ex.Message);
                newWriter.Flush();
                newWriter.Close();

                throw ex;
            }
        }

        public static bool GetCurrentPurchaseOrder()
        {
            try
            {
                SyncInventoryRealTimeController syncInventory = new SyncInventoryRealTimeController();
                syncInventory.OnProgressUpdates += new UpdateProgress(syncInventory_OnProgressUpdates);
                return syncInventory.GetPurchaseOrder();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        static void syncInventory_OnProgressUpdates(object sender, string message)
        {
            Logger.writeLog(string.Format("- SYNC PO {0} : {1}",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), message));
        }


        public static bool checkIntegrity()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
            }
            return false;
        }

        #region Magento
        public static int InsertCategory(PowerPOSLib.Magento.catalogCategoryEntity catTemp)
        {
            QueryCommand cmd = null;
            if (catTemp != null)
            {
                Category cat = new Category(catTemp.name);
                if (cat == null || !cat.IsLoaded || cat.CategoryName == "")
                {
                    //insert new category
                    cat = new Category();
                    cat.CategoryId = catTemp.name;
                    cat.CategoryName = catTemp.name;
                    ItemDepartment itemDept = new ItemDepartment(catTemp.name);
                    if (itemDept == null || !itemDept.IsLoaded || itemDept.ItemDepartmentID == "")
                    {
                        itemDept = new ItemDepartment();
                        itemDept.ItemDepartmentID = catTemp.name;
                        itemDept.DepartmentName = catTemp.name;
                        itemDept.Deleted = false;
                        itemDept.Save(UserInfo.username);
                    }
                    cat.ItemDepartmentId = catTemp.name;

                    cat.Userint1 = catTemp.category_id;
                    cat.Deleted = false;
                    return DataService.ExecuteQuery(cat.GetInsertCommand("Sync"));
                }
                else
                {
                    ItemDepartment itemDept = new ItemDepartment(catTemp.name);
                    if (itemDept == null || !itemDept.IsLoaded || itemDept.ItemDepartmentID == "")
                    {
                        itemDept = new ItemDepartment();
                        itemDept.ItemDepartmentID = catTemp.name;
                        itemDept.DepartmentName = catTemp.name;
                        itemDept.Deleted = false;
                        itemDept.Save(UserInfo.username);
                    }
                    cat.ItemDepartmentId = catTemp.name;

                    cat.Userint1 = catTemp.category_id;
                    cat.Deleted = false;
                    return DataService.ExecuteQuery(cat.GetUpdateCommand("Sync"));
                }

            }
            return 1;
        }

        public static QueryCommand InsertItem(PowerPOSLib.Magento.catalogProductReturnEntity prodTemp,
            string categoryName)
        {
            QueryCommand cmd = null;

            if (prodTemp != null)
            {
                /*if (prodTemp.product_id == "13")
                {
                    Logger.writeLog(prodTemp.product_id);
                }*/
                Item it = new Item(prodTemp.product_id);
                if (it != null && it.IsLoaded && it.ItemNo != "")
                {
                    //update item
                    it.CategoryName = categoryName;
                    it.ItemName = prodTemp.name;
                    it.IsInInventory = true;
                    it.IsServiceItem = false;
                    it.Barcode = prodTemp.sku;
                    decimal prc = 0;
                    decimal.TryParse(prodTemp.price, out prc);
                    it.RetailPrice = prc;
                    return it.GetUpdateCommand(UserInfo.username);
                }
                else
                {
                    it = new Item();
                    it.ItemNo = prodTemp.product_id;
                    it.CategoryName = categoryName;
                    it.ItemName = prodTemp.name;
                    it.IsInInventory = true;
                    it.IsServiceItem = false;
                    it.Barcode = prodTemp.sku;
                    decimal prc = 0;
                    decimal.TryParse(prodTemp.price, out prc);
                    it.RetailPrice = prc;
                    it.Deleted = false;
                    return it.GetInsertCommand(UserInfo.username);
                }
            }
            return cmd;
        }

        private static bool traverseCategory(catalogCategoryEntity cat)
        {
            try
            {
                InsertCategory(cat);
                foreach (catalogCategoryEntity cattemp in cat.children)
                {
                    traverseCategory(cattemp);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }

        }

        public static bool SyncCategory()
        {
            try
            {
                string user = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
                string password = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
                PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();
                ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
                string SessionId = ws.login(user, password);

                PowerPOSLib.Magento.catalogCategoryTree categoryList = ws.catalogCategoryTree(SessionId, "2", "1");

                ArrayList arr = new ArrayList();

                foreach (PowerPOSLib.Magento.catalogCategoryEntity cat in categoryList.children)
                {
                    traverseCategory(cat);

                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }
        }

        public static bool SyncItem(string CategoryName)
        {
            try
            {

                string user = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
                string password = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
                PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();
                ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
                string SessionId = ws.login(user, password);

                Category cat = new Category(CategoryName);

                if (cat != null && cat.CategoryName != "" && cat.Userint1 != null && cat.Userint1 != 0)
                {
                    SessionId = ws.login(user, password);
                    QueryCommandCollection col = new QueryCommandCollection();

                    Logger.writeLog(cat.CategoryName);
                    catalogAssignedProduct[] prods = ws.catalogCategoryAssignedProducts(SessionId, (int)cat.Userint1, "1");
                    int i = 1;
                    foreach (PowerPOSLib.Magento.catalogAssignedProduct prod in prods)
                    {
                        catalogProductRequestAttributes attributetemp = new catalogProductRequestAttributes();
                        Logger.writeLog(i.ToString() + "," + prod.product_id.ToString());

                        PowerPOSLib.Magento.catalogProductReturnEntity pricetemp = ws.catalogProductInfo(SessionId, prod.product_id.ToString(), "", attributetemp, "product_id");
                        QueryCommand cmdInsertItem = InsertItem(pricetemp, cat.CategoryName);
                        cmdInsertItem.CommandTimeout = 1000000;
                        if (cmdInsertItem != null) col.Add(cmdInsertItem);

                        i++;
                    }

                    DataService.ExecuteTransaction(col);


                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }
        }

        public bool SyncCategoryFromServer()
        {
            bool res = true;
            try
            {
                PowerPOSLib.PowerPOSSync.Synchronization ws =
                        new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 1000000;
                ws.Url = WS_URL;

                //deduct inventory
                //ws.Timeout = 500000;
                ws.AssignStockOutToConfirmedOrderUsingTransactionScope();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool getItemMagento()
        {
            try
            {

                //ArrayList catArr = new ArrayList();
                PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();
                ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
                string SessionId = ws.login("adiwinoto", "edgew!123");
                #region hide
                //grab category
                PowerPOSLib.Magento.catalogCategoryTree categoryList = ws.catalogCategoryTree(SessionId, "2", "1");

                ArrayList arr = new ArrayList();


                foreach (PowerPOSLib.Magento.catalogCategoryEntity cat in categoryList.children)
                {
                    traverseCategory(cat);

                }

                CategoryCollection CategoryColl = new CategoryCollection();
                CategoryColl.Load();

                foreach (Category ctg in CategoryColl)
                {
                    SessionId = ws.login("adiwinoto", "edgew!123");
                    QueryCommandCollection col = new QueryCommandCollection();

                    if (ctg.Userint1 != null && ctg.Userint1 > 0)
                    {
                        Logger.writeLog(ctg.CategoryName);
                        catalogAssignedProduct[] prods = ws.catalogCategoryAssignedProducts(SessionId, (int)ctg.Userint1, "1");
                        int i = 1;
                        foreach (PowerPOSLib.Magento.catalogAssignedProduct prod in prods)
                        {
                            catalogProductRequestAttributes attributetemp = new catalogProductRequestAttributes();
                            Logger.writeLog(i.ToString() + "," + prod.product_id.ToString());

                            PowerPOSLib.Magento.catalogProductReturnEntity pricetemp = ws.catalogProductInfo(SessionId, prod.product_id.ToString(), "", attributetemp, "product_id");
                            QueryCommand cmdInsertItem = InsertItem(pricetemp, ctg.CategoryName);
                            if (cmdInsertItem != null) col.Add(cmdInsertItem);

                            i++;
                        }
                        DataService.ExecuteTransaction(col);

                    }
                    bool test = ws.endSession(SessionId);
                }

                #endregion

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        public static bool SendDeliveryOrderToServer(DateTime StartDate, DateTime EndDate, bool isRealTimeSales)
        {
            try
            {
                if (!isRealTimeSales)
                {
                    DateTime dtLastClosing = ClosingController.FetchLastClosingTime(PointOfSaleInfo.PointOfSaleID);
                    if (EndDate > dtLastClosing)
                        EndDate = dtLastClosing;
                }

                CultureInfo ct = new CultureInfo("");
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy-MM-dd";

                ct.DateTimeFormat = dtFormat;

                Logger.writeLog("Start sending Delivery Order to server");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                bool result;
                DataSet ds;
                DataSet resultDS = new DataSet();
                Query qry;

                Logger.writeLog("Sending DeliveryOrder table...");

                // Get DeliveryOrder
                qry = new Query("DeliveryOrder");
                qry.BETWEEN_AND("CreatedOn", StartDate, EndDate);
                qry.OR_BETWEEN_AND("ModifiedOn", StartDate, EndDate);
                ds = qry.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Locale = ct;
                    setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                    DataTable dt = ds.Tables[0].Copy();
                    dt.TableName = "DeliveryOrder";
                    resultDS.Tables.Add(dt);
                }

                // Get DeliveryOrderDetails
                qry = new Query("DeliveryOrderDetails");
                qry.BETWEEN_AND("CreatedOn", StartDate, EndDate);
                qry.OR_BETWEEN_AND("ModifiedOn", StartDate, EndDate);
                ds = qry.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    ds.Locale = ct;
                    setAllDateTimeColumnToBeLocal(ds.Tables[0]);
                    DataTable dt = ds.Tables[0].Copy();
                    dt.TableName = "DeliveryOrderDetails";
                    resultDS.Tables.Add(dt);
                }

                // Get Deposit Amount from OrderDet
                qry = new Query("OrderDet");
                qry.BETWEEN_AND("CreatedOn", StartDate, EndDate);
                qry.OR_BETWEEN_AND("ModifiedOn", StartDate, EndDate);
                ds = qry.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = new DataTable("DepositAmount");
                    dt.Columns.Add("OrderDetID", Type.GetType("System.String"));
                    dt.Columns.Add("DepositAmount", Type.GetType("System.Decimal"));
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
                        if (!string.IsNullOrEmpty(od.OrderDetID))
                        {
                            dt.Rows.Add(od.OrderDetID, od.DepositAmount);
                        }
                    }
                    resultDS.Tables.Add(dt);
                }

                //// Get Deposit Amount from OrderDet if there's modified DeliveryOrderDetails
                //if (resultDS.Tables.Contains("DeliveryOrderDetails") && resultDS.Tables["DeliveryOrderDetails"].Rows.Count > 0)
                //{
                //    DataTable dt = new DataTable("DepositAmount");
                //    dt.Columns.Add("OrderDetID", Type.GetType("System.String"));
                //    dt.Columns.Add("DepositAmount", Type.GetType("System.Decimal"));
                //    foreach (DataRow dr in resultDS.Tables["DeliveryOrderDetails"].Rows)
                //    {
                //        OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
                //        if (!string.IsNullOrEmpty(od.OrderDetID))
                //        {
                //            dt.Rows.Add(od.OrderDetID, od.DepositAmount);
                //        }
                //    }
                //    resultDS.Tables.Add(dt);
                //}

                result = ws.FetchDeliveryOrder(resultDS);
                Logger.writeLog("Sending result = " + result);


                //result = ws.FetchLogTableWithUpdate(ds, "DeliveryOrder");
                //TotalResult &= result;
                //Logger.writeLog("Sending result = " + result);

                //Logger.writeLog("Sending DeliveryOrderDetails table...");
                //result = ws.FetchLogTableWithUpdate(ds, "DeliveryOrderDetails");
                //TotalResult &= result;
                //Logger.writeLog("Sending result = " + result);


                return result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool FetchTableWithUpdateOption(DataSet dsTable, string tableName, bool doUpdate)
        {
            CultureInfo ct = new CultureInfo("");
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            ct.DateTimeFormat = dtFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture = ct;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

            try
            {
                Query qry;
                Guid tmpGuid;

                qry = new Query(tableName);
                ArrayList Columns = new ArrayList();
                ArrayList Parameters = new ArrayList();
                Logger.writeLog("Fetch:" + tableName);

                int startCol = 0;
                if (DataService.GetSchema(tableName, "PowerPOS").PrimaryKey.AutoIncrement)
                {
                    startCol = 1;
                }

                if (DataService.GetSchema(tableName, "PowerPOS").PrimaryKey.DataType == DbType.Guid)
                {
                    int f = 0;
                    f = f + 1;
                }
                string ColumnList = "", ParameterList = "";
                for (int i = startCol; i < dsTable.Tables[0].Columns.Count; i++)
                {
                    if (i == dsTable.Tables[0].Columns.Count - 1)
                    {
                        ColumnList += dsTable.Tables[0].Columns[i].ColumnName;
                        ParameterList += "@" + dsTable.Tables[0].Columns[i].ColumnName;
                    }
                    else
                    {
                        ColumnList += dsTable.Tables[0].Columns[i].ColumnName + ",";
                        ParameterList += "@" + dsTable.Tables[0].Columns[i].ColumnName + ",";
                    }
                    Columns.Add(dsTable.Tables[0].Columns[i].ColumnName);
                    Parameters.Add("@" + dsTable.Tables[0].Columns[i].ColumnName);
                }
                for (int i = 0; i < dsTable.Tables[0].Rows.Count; i++)
                {
                    tmpGuid = new Guid(dsTable.Tables[0].Rows[i]["UniqueID"].ToString());

                    Where whr = new Where();
                    whr.ParameterName = "@UniqueID";
                    whr.ParameterValue = tmpGuid.ToString();
                    whr.ColumnName = "UniqueID";
                    whr.TableName = tableName;

                    if (qry.GetCount("UniqueID", whr) == 0)
                    {
                        //if non existance, do insert.....

                        string sql = "insert into " + tableName + " (" + ColumnList + ")";
                        sql += " values (" + ParameterList + ") ";
                        QueryCommand cmd = new QueryCommand(sql);
                        for (int k = 0; k < Columns.Count; k++)
                        {
                            //if (Columns[k].ToString().ToLower() == "uniqueid")
                            if (DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[k].ToString()).DataType == DbType.Guid)
                            {
                                cmd.AddParameter(Parameters[k].ToString(), (new Guid(dsTable.Tables[0].Rows[i][Columns[k].ToString()].ToString())).ToString());
                            }
                            else
                            {
                                cmd.AddParameter(Parameters[k].ToString(), dsTable.Tables[0].Rows[i][Columns[k].ToString()]);
                            }
                        }
                        //Logger.writeLog(dsLogTable.Tables[0].Rows[i][Columns[0].ToString()].ToString());
                        SubSonic.DataService.ExecuteQuery(cmd);
                    }
                    else
                    {
                        if (doUpdate)
                        {
                            // do update......

                            QueryCommand cmd = new QueryCommand("");
                            string sql = "update " + tableName + " set ";
                            for (int k = 0; k < Columns.Count; k++)
                            {
                                if (Columns[k].ToString().ToUpper() == "UNIQUEID") continue;  // Skip if column = UniqueID

                                sql += Columns[k].ToString() + "=" + Parameters[k].ToString() + ",";

                                if (DataService.GetSchema(tableName, "PowerPOS").GetColumn(Columns[k].ToString()).DataType == DbType.Guid)
                                {
                                    cmd.AddParameter(Parameters[k].ToString(), (new Guid(dsTable.Tables[0].Rows[i][Columns[k].ToString()].ToString())).ToString());
                                }
                                else
                                {
                                    cmd.AddParameter(Parameters[k].ToString(), dsTable.Tables[0].Rows[i][Columns[k].ToString()]);
                                }
                            }
                            sql = sql.Trim(',');
                            sql += " where UniqueID = @UniqueID and ModifiedOn < @ModifiedOn ";  // Only update if received data is newer
                            cmd.AddParameter("@UniqueID", tmpGuid, DbType.Guid);
                            cmd.CommandSql = sql;

                            SubSonic.DataService.ExecuteQuery(cmd);
                        }
                    }
                    //i++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
        }

        public static bool FreezePOSByPointOfSaleID(int PointOfSaleID)
        {
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                if (!ws.FreezePOSByPointOfSaleID(PointOfSaleID))
                {
                    throw new Exception("Error freeze POS.");
                }

                return true;
            }
            catch (Exception ex)
            { Logger.writeLog(ex.Message); return false; }
        }

        public static bool ExecuteUpdateInServer(DataTable dt)
        {
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                return ws.ExecuteUpdateUsingDataTable(dt);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static DataTable GetDataTableStructureForUpdate()
        {
            DataTable dt = new DataTable("DT");
            dt.Columns.Add("TableName", Type.GetType("System.String"));
            dt.Columns.Add("PKName", Type.GetType("System.String"));
            dt.Columns.Add("PKValue", Type.GetType("System.String"));
            dt.Columns.Add("PKType", Type.GetType("System.String"));
            dt.Columns.Add("UpdColName", Type.GetType("System.String"));
            dt.Columns.Add("UpdColValue", Type.GetType("System.String"));
            dt.Columns.Add("UpdColType", Type.GetType("System.String"));
            return dt;
        }

        public static bool ExecuteUpdateUsingDataTable(DataTable dt)
        {
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Query qry = new Query(dr["TableName"].ToString());
                    qry.AddUpdateSetting(dr["UpdColName"].ToString(), Convert.ChangeType(dr["UpdColValue"], Type.GetType(dr["UpdColType"].ToString())));
                    qry.AddWhere(dr["PKName"].ToString(), Convert.ChangeType(dr["PKValue"], Type.GetType(dr["PKType"].ToString())));
                    qry.Execute();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        #region Inventory - Saved Files
        public static bool SaveInventoryFile
            (InventoryController invCtrl, string saveName, string movementType, string remark, bool autoSave, out string status)
        {
            bool res = false;
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                /*invCtrl.InvHdr.ModifiedOn = DateTime.Now;
                invCtrl.InvHdr.CreatedOn = DateTime.Now;
                foreach (InventoryDet id in invCtrl.InvDet)
                {
                    id.ModifiedOn = DateTime.Now;
                    id.CreatedOn = DateTime.Now;
                }*/
                DataSet ds = new DataSet();
                ds.Tables.Add(invCtrl.InvHdrToDataTable());
                ds.Tables.Add(invCtrl.InvDetToDataTable());
                byte[] data = CompressDataSetToByteArray(ds);
                string resString = ws.SaveInventoryFile(data, movementType, remark, autoSave);
                if (!resString.Contains("Error"))
                {
                    status = "";
                    res = true;
                }
                else
                {
                    status = resString;
                    res = false;
                }
            }
            catch (Exception ex)
            {
                res = false;
                status = ex.Message;
            }
            return res;
        }

        public static DataSet GetSavedInventoryFile
            (string ar)
        {
            DataSet ds = new DataSet();
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                byte[] data = ws.GetInventorySavedFile(ar);
                ds = DecompressDataSetFromByteArray(data);
                
            }
            catch (Exception ex)
            {
                return ds;
            }
            return ds;
        }

        public static InventoryController GetLoadedInventoryFile
            (string saveName)
        {
            string result = "";
            InventoryController obj = new InventoryController();
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                byte[] data = ws.LoadInventoryFromFile(saveName) ;
                DataSet ds = DecompressDataSetFromByteArray(data);
                if (ds.Tables.Count > 0)
                {
                    obj.LoadFromDataTable(ds.Tables[0], ds.Tables[1]);
                    //obj = new JavaScriptSerializer().Deserialize<InventoryController>(result);
                }

            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Loading Inventory File. " + ex.Message);
                return obj;
            }
            return obj;
        }

        public static bool removeSavedFile(string saveName)
        {
            Load_WS_URL();
            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
            ws.Timeout = 100000;
            ws.Url = WS_URL;
            return ws.removeSavedFile(saveName);
        }

        #endregion

        public static bool GetAppointments(bool syncAll)
        {
            bool TotalResult = true, result; int i;
            string[] tableName = { "UserMst", "ItemDepartment", "Category", "Item", "AlternateBarcode", "Membership", "Appointment", "AppointmentItem", "ResourceGroup", "Resource" };
            string[] primaryKeyName = { "UserName", "ItemDepartmentID", "CategoryName", "ItemNo", "BarcodeID", "MembershipNo", "Id", "Id", "ResourceGroupID", "ResourceID" };
            bool[] IsPKAutoGenerated = { false, false, false, false, true, false, false, false, true, false };
            bool[] AlwaysSyncAll = { false, true, true, false, false, false, true, true, true, true };
            bool IsSyncAll;
            for (int k = 0; k < tableName.Length; k++)
            {
                i = 0; result = false;
                if (AlwaysSyncAll[k])
                {
                    IsSyncAll = true;
                }
                else
                {
                    IsSyncAll = syncAll;
                }
                while (!result & i < RETRY_LIMIT)
                {
                    result = GetTable(IsSyncAll, tableName[k], primaryKeyName[k], IsPKAutoGenerated[k]);
                    //result = GetItem(syncAll);
                    i += 1;
                }
                TotalResult &= result;
            }

            return TotalResult;
        }

        public static DataSet GetOrderForRefund
            (string orderhdrid, int currentPosID, bool AllowRefundForSameOutlet, out string status)
        {
            status = "";
            
            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                byte[] tmp = ws.getOrderForRefund(orderhdrid,currentPosID,AllowRefundForSameOutlet, out status);
                DataSet ds = new DataSet();
                if (tmp != null)
                {
                    ds = DecompressDataSetFromByteArray(tmp);
                    return ds;
                }

                /*DataSet ds = DecompressDataSetFromByteArray(data);
                if (ds.Tables.Count > 0)
                {
                    obj.LoadFromDataTable(ds.Tables[0], ds.Tables[1]);
                    //obj = new JavaScriptSerializer().Deserialize<InventoryController>(result);
                }*/
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Getting POS for Refund. " + ex.Message);
                return null;
            }
            
        }


        public static DataSet GetOrder
            (string orderhdrid, out string status)
        {
            status = "";

            try
            {
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                byte[] tmp = ws.getOrder(orderhdrid, out status);
                DataSet ds = new DataSet();
                if (tmp != null)
                {
                    ds = DecompressDataSetFromByteArray(tmp);
                    return ds;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Getting POS for Refund. " + ex.Message);
                return null;
            }

        }
    }
}

