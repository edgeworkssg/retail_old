using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PowerPOS;
using System.Threading;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;

namespace PowerPOS.DeliveryOrderRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncDeliveryOrderRealTimeController
    {
        public const string XMLFILENAME = "\\WS.XML";
        public string WS_URL = "http://localhost:51831/PowerPOSWeb/Synchronization/Synchronization.asmx";

        public bool Load_WS_URL()
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                WS_URL = ds.Tables[0].Rows[0]["URL"].ToString();
                return true;
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("Load_WS_URL");
                Logger.writeLog(ex);
                return false;
            }
        }

        public event UpdateProgress OnProgressUpdates;
        public event DataDownloadedHandler OnDataDownloaded;

        public bool UploadDeliveryOrder()
        {

            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DateTime lastSalesModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                #endregion
                //while (true)
                //{

                //Logger.writeLog(tryTimes.ToString());
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Delivery Order Started. Connecting To Server");
                tryTimes = 0;
                isSuccessConnecting = false;
                #region Connecting & Get Last TimeStamp
                while (!isSuccessConnecting)
                {
                    tryTimes++;
                    try
                    {
                        isSuccessConnecting = ws.GetDeliveryOrderLastTimeStamp(PointOfSaleInfo.PointOfSaleID, false, out lastSalesModifiedOn);
                    }
                    catch (Exception ex)
                    {
                        if (tryTimes == 1)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Failed To Connect To Server. Retrying in 5 sec.");
                            Logger.writeLog("Error Connecting To Server. " + ex.Message);
                        }
                        Thread.Sleep(5000);
                    }
                }
                #endregion

                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Server is Connected");

                if (isSuccessConnecting)
                {
                    if (OnProgressUpdates != null)
                    {
                        OnProgressUpdates(this, "Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        Logger.writeLog("Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    }

                    int dataCount = 0;
                    string statusMsg;
                    if (!getLocalOrderDataCount(lastSalesModifiedOn, out dataCount, out statusMsg))
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                        return false;
                    }

                    if (dataCount == 0)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "All data has been sent successfully to Server. Sync DeliveryOrder is completed.");
                        //everything completed successfully, return true
                        return true;
                    }

                    if (OnProgressUpdates != null)
                    {
                        OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());
                        Logger.writeLog("Total Records to be Synced : " + dataCount.ToString());
                    }

                    #region init variables
                    int dataSent = 0;
                    int recordPerTimes = 0;
                    string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncDeliveryOrderRecordsPerTime);
                    if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                    {
                        recordPerTimes = 100;
                    }
                    #endregion
                    while (dataSent < dataCount)
                    {

                        string[][] objHdr;
                        string[][] objDet;
                        string[][] objMember;
                        string[][] objDeposit;

                        //send deliveryorder
                        DateTime lastModifiedOnPerTimes;
                        if (!getDeliveryOrderData(lastSalesModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes, out objHdr,
                            out objDet, out objMember, out objDeposit, out statusMsg, out lastModifiedOnPerTimes))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Fetch Delivery Order Data Failed.");
                            return false;
                        }
                        if (OnProgressUpdates != null)
                        {
                            OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server...");
                            Logger.writeLog("Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server...");
                        }


                        try
                        {
                            result = ws.FetchDeliveryOrderCCMWRealTime(objHdr, objDet, objMember, objDeposit);
                            if (result)
                            {
                                for (int i = 0; i < objHdr.GetLength(0); i++)
                                {
                                    string id = objHdr[i][0];
                                    DeliveryOrder doh = new DeliveryOrder(id);
                                    if (doh != null && doh.OrderNumber == id)
                                    {
                                        doh.IsServerUpdate = false;
                                        doh.Save(UserInfo.username);
                                    }
                                }
                                lastSalesModifiedOn = lastModifiedOnPerTimes;
                                // send data to server is successful, update the count
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server is completed successfully.");
                                dataSent += recordPerTimes;
                            }
                        }
                        catch (Exception ex)
                        {
                            //server connection problem
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Server is disconnected, will keep trying every 5 seconds until server is connected");
                            Logger.writeLog("Error Connecting To Server. " + ex.Message);

                            //break out from sending data loop, so that the thread will try to reconnect to server again
                            break;
                        }
                    }
                }
                //}
            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }

            return false;
        }

        public bool getLocalOrderDataCount(DateTime lastModifiedOn, out int dataCount, out string statusMsg)
        {
            statusMsg = "";
            try
            {
                string qry = "SELECT COUNT(*) FROM DeliveryOrder do INNER JOIN OrderHdr oh ON oh.OrderHdrID = do.SalesOrderRefNo where ISNULL(do.IsServerUpdate, 0) = 1 AND oh.PointOfSaleID = " + PointOfSaleInfo.PointOfSaleID.ToString();
                object tmp = DataService.ExecuteScalar(new QueryCommand(qry));
                int ct = 0;
                dataCount = 0;
                if (int.TryParse(tmp.ToString(), out ct))
                    dataCount = ct;
                return true;
            }
            catch (Exception ex)
            {
                dataCount = 0;
                statusMsg = "";
                return false;
            }
        }

        public bool getDeliveryOrderData(DateTime StartDate, int numofRecords, out string[][] objHdr,
            out string[][] objDet, out string[][] objMember, out string[][] objDeposit, out string statusMsg, out DateTime LastModifiedOn)
        {
            string parametername = "";
            try
            {

                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";

                DeliveryOrderCollection myHdr = new DeliveryOrderCollection();

                DataTable dt = SyncClientController.FetchDeliveryOrderNotInServerWithPOSID
                    (StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);

                    DeliveryOrderDetailCollection myDet = new DeliveryOrderDetailCollection();
                    MembershipCollection myMember = new MembershipCollection();

                    LastModifiedOn = StartDate;
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        myHdr[i].UniqueID = new Guid(dt.Rows[i]["UniqueID"].ToString());
                        if (myHdr[i].ModifiedOn > LastModifiedOn)
                            LastModifiedOn = myHdr[i].ModifiedOn.Value;
                    }

                    for (int i = 0; i < myHdr.Count; i++)
                        myDet.AddRange(myHdr[i].DeliveryOrderDetails());

                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        if (myHdr[i].MembershipNo != "WALK-IN")
                        {
                            Membership m = new Membership(myHdr[i].MembershipNo);
                            bool isFound = false;
                            foreach (Membership ms in myMember)
                            {
                                if (m.MembershipNo == ms.MembershipNo)
                                    isFound = true;
                            }
                            if (!isFound)
                                myMember.Add(m);
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
                            parametername = param[oT].ParameterName.ToLower();
                            if (param[oT].ParameterName.ToLower() != "@createdon" &
                                   param[oT].ParameterName.ToLower() != "@createdby" &
                                    param[oT].ParameterName.ToLower() != "@modifiedon" &
                                    param[oT].ParameterName.ToLower() != "@modifiedby")
                            {
                                if (param[oT].DataType == DbType.DateTime)
                                {
                                    objArr[oT] = param[oT].ParameterValue != null && param[oT].ParameterValue.ToString() != "" ? ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss.fff") : null;
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
                                    objArr[oT] = myHdr[op].ModifiedOn.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                }

                                if (param[oT].ParameterName.ToLower() == "@createdon")
                                {
                                    objArr[oT] = myHdr[op].CreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
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

                    objMember = new string[myMember.Count][];
                    for (int op = 0; op < myMember.Count; op++)
                    {
                        QueryParameterCollection param = myMember[op].GetInsertCommand("").Parameters;
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
                                    if (param[oT].ParameterValue != null && param[oT].ParameterValue.ToString() != "")
                                        objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                    else
                                        objArr[oT] = "";
                                }
                                else
                                {
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                                }
                            }
                        }
                        objMember[op] = objArr;
                    }

                    objDeposit = new string[myDet.Count][];

                    for (int op = 0; op < myDet.Count; op++)
                    {
                        string[] objArr = new string[2];
                        OrderDet od = new OrderDet(myDet[op].OrderDetID);
                        objArr[0] = od.OrderDetID;
                        objArr[1] = od.DepositAmount.ToString();
                        objDeposit[op] = objArr;
                    }

                    return true;
                }
                else
                {
                    objHdr = null;
                    objDet = null;
                    objMember = null;
                    objDeposit = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objHdr = null;
                objDet = null;
                objMember = null;
                objDeposit = null;
                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Send DeliveryOrder failed." + ex.Message, true);
                Logger.writeLog(parametername);
                return false;
            }
        }

        public bool DownloadDeliveryOrder()
        {
            //if (!e.Cancel)
            //{
            try
            {
                #region *) Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Download DeliveryOrder Start");

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                int retrySecConnected = 600;
                int retrySecDisconnected = 5;
                int recordsPerTime = 100;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.DeliveryOrderRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.DeliveryOrderRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncDeliveryOrderRecordsPerTime), out recordsPerTime))
                    recordsPerTime = 100;
                #endregion

                while (true)
                {
                    if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMember), false))
                    {
                        #region Download Membership
                        try
                        {
                            DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("Membership", out lastModifiedOnID))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the Membership on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordID = ws.FetchRecordNoByTimestamp("Membership", lastModifiedOnID);
                            if (totalRecordID > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync Membership");

                                int dataGetID = 0;
                                int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                while (dataGetID < totalRecordID)
                                {
                                    var dataID = ws.FetchDataSetByTimestamp("Membership", "MembershipNo", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                    var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                    if (SyncRealTimeController.DownloadData(dsID, "Membership", "MembershipNo", false))
                                    {
                                        dataGetID += dsID.Tables[0].Rows.Count;
                                        if (dataGetID > totalRecordID)
                                            dataGetID = totalRecordID;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download Membership data. {0} of {1} ", dataGetID, totalRecordID));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download Membership"));
                                    }
                                }

                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All Membership have been synchronized successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading Membership. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion
                    }

                    #region Download DeliveryOrder
                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("DeliveryOrder", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the DeliveryOrder on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestampDeliveryOrder(lastModifiedOnID, PointOfSaleInfo.PointOfSaleID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Delivery Order");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                            while (dataGetID < totalRecordID)
                            {
                                var dataID = ws.FetchDataSetByTimestampDeliveryOrder(lastModifiedOnID, totalRecordID, PointOfSaleInfo.PointOfSaleID);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                bool isSuccess = true;
                                string tempDOHDRID = "";

                                for (int i = 0; i < dsID.Tables.Count; i++)
                                {
                                    DataTable dtID = dsID.Tables[i];
                                    if (dtID.TableName.ToLower() == "deliveryorder")
                                    {
                                        if (SyncRealTimeController.DownloadDataWithDataTable(dtID, "DeliveryOrder", "OrderNumber", false))
                                        {
                                            for (int j = 0; j < dtID.Rows.Count; j++)
                                            {
                                                if (tempDOHDRID == "")
                                                {
                                                    tempDOHDRID += dtID.Rows[j]["OrderNumber"].ToString();
                                                }
                                                else
                                                {
                                                    tempDOHDRID += ("," + dtID.Rows[j]["OrderNumber"].ToString());
                                                }
                                            }
                                            dataGetID += dtID.Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download DeliveryOrder data. {0} of {1} ", dataGetID, totalRecordID));
                                            try
                                            {
                                                DeliveryOrderController.UpdateDeliveryOrderIsUpdated(tempDOHDRID);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (OnProgressUpdates != null)
                                                    OnProgressUpdates(this, "Error occured : " + ex.Message);
                                                Thread.Sleep(retrySecDisconnected * 1000);
                                                Logger.writeLog(ex);
                                                isSuccess = false;
                                            }

                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download DeliveryOrder"));
                                            isSuccess = false;
                                        }
                                    }
                                    else if (dtID.TableName.ToLower() == "deliveryorderdetails")
                                    {
                                        if (SyncRealTimeController.DownloadDataWithDataTable(dtID, "DeliveryOrderDetails", "DetailsID", false))
                                        {
                                            dataGetID += dtID.Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download DeliveryOrderDetails data. {0} of {1} ", dataGetID, totalRecordID));
                                            try
                                            {
                                                if (OnDataDownloaded != null)
                                                    OnDataDownloaded(this, "DeliveryOrderDetails Updated");
                                            }
                                            catch (Exception ex)
                                            {
                                                if (OnProgressUpdates != null)
                                                    OnProgressUpdates(this, "Error occured : " + ex.Message);
                                                Thread.Sleep(retrySecDisconnected * 1000);
                                                Logger.writeLog(ex);
                                                isSuccess = false;
                                            }

                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download DeliveryOrderDetails"));
                                            isSuccess = false;
                                        }
                                    }
                                    else if (dtID.TableName.ToLower() == "depositamount")
                                    {
                                        foreach (DataRow dr in dtID.Rows)
                                        {
                                            OrderDet od = new OrderDet(dr["OrderDetID"].ToString());
                                            if (!string.IsNullOrEmpty(od.OrderDetID))
                                            {
                                                if (dr["DepositAmount"] is decimal)
                                                {
                                                    od.DepositAmount = Convert.ToDecimal(dr["DepositAmount"]);
                                                    od.Save("SYNC");
                                                }
                                            }
                                        }
                                    }
                                    else
                                        continue;
                                }

                                //update server
                                if (isSuccess)
                                {
                                    ws.UpdateDeliveryOrderIsUpdated(tempDOHDRID);
                                }
                                //foreach(DataRow dr in dataID)
                            }

                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Delivery Order have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Delivery Order. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }

                    #endregion

                    Thread.Sleep(retrySecConnected * 1000);
                }
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Download DeliveryOrder stopped");
                return true;
            }
            catch (Exception ex)
            {
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Download DeliveryOrder with Error: " + ex.Message);

                Logger.writeLog(ex);
                return false;
            }
            //}
            //return false;
        }
    }
}
