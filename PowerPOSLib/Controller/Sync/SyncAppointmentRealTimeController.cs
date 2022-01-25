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

namespace PowerPOS.AppointmentRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncAppointmentRealTimeController
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

        public bool SyncAppointment()
        {
            string status = "";
            try
            {
                if (!UploadAppointment())
                    OnProgressUpdates(this, "Unable to send appointment to server" );
                else
                {
                    bool getApp = SyncClientController.GetAppointments(false);
                    if (getApp)
                    {
                        OnProgressUpdates(this, "Synced Successfully");
                    }
                    else
                    {
                        OnProgressUpdates(this, "Unable to get appointment from server");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                OnProgressUpdates(this, "Error sync appointment " + ex.Message);

                return false;
            }
        }

        public bool UploadAppointment()
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
                    OnProgressUpdates(this, "Sync Appointment Started. Connecting To Server");
                tryTimes = 0;
                isSuccessConnecting = false;
                #region Connecting & Get Last TimeStamp
                while (!isSuccessConnecting)
                {
                    tryTimes++;
                    try
                    {
                        isSuccessConnecting = ws.GetAppointmentLastTimeStamp(PointOfSaleInfo.PointOfSaleID, false, out lastSalesModifiedOn);
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
                    if (!getLocalOrderDataCount(lastSalesModifiedOn, PointOfSaleInfo.PointOfSaleID, out dataCount, out statusMsg))
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                        return false;
                    }

                    if (dataCount == 0)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "All data has been sent successfully to Server. Sync Appointment is completed.");
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
                    string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncRecordsPerTime);
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

                        //send sales
                        DateTime lastModifiedOnPerTimes;
                        if (!getAppointmentData(lastSalesModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes, out objHdr,
                            out objDet, out objMember, out statusMsg, out lastModifiedOnPerTimes))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Fetch Order Data Failed.");
                            return false;
                        }
                        if (OnProgressUpdates != null)
                        {
                            OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server...");
                            Logger.writeLog("Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server...");
                        }


                        try
                        {
                            result = ws.FetchAppointmentsCCMWRealTime(objHdr, objDet, objMember);
                            if (result)
                            {
                                for (int i = 0; i < objHdr.GetLength(0); i++)
                                {
                                    string id = objHdr[i][0];
                                    Appointment apt = new Appointment(id);
                                    if (apt != null)
                                    {
                                        apt.IsServerUpdate = false;
                                        apt.Save(UserInfo.username);
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

        public bool getLocalOrderDataCount(DateTime lastModifiedOn, int PointOfSaleID, out int dataCount, out string statusMsg)
        {
            statusMsg = "";
            try
            {
                string qry = "Select count(*) from appointment where isServerUpdate =1 and PointOfSaleID = " + PointOfSaleID.ToString();

                //AppointmentCollection ohCol = new AppointmentCollection();
                //ohCol.Where(Appointment.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                //ohCol.Where(Appointment.Columns.IsServerUpdate, Comparison.Equals, true);
                //ohCol.OrderByAsc(Appointment.Columns.ModifiedOn);
                //ohCol.Load();
                object tmp = DataService.ExecuteScalar(new QueryCommand(qry));
                int ct = 0;
                dataCount = 0;
                if (int.TryParse(tmp.ToString(),out ct))
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

        public bool getAppointmentData(DateTime StartDate, int numofRecords, out string[][] objHdr,
            out string[][] objDet, out string[][] objMember, out string statusMsg, out DateTime LastModifiedOn)
        {
            string parametername = "";
            try
            {

                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";

                AppointmentCollection myHdr = new AppointmentCollection();

                DataTable dt = AppointmentSync.FetchAppointmentNotInServerWithPOSID
                    (StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);

                    AppointmentItemCollection myDet = new AppointmentItemCollection();
                    MembershipCollection myMember = new MembershipCollection();

                    LastModifiedOn = StartDate;
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        myHdr[i].Id = new Guid(dt.Rows[i]["Id"].ToString());

                        Logger.writeLog("Appointment id: " + dt.Rows[i]["Id"].ToString());
                        if (myHdr[i].ModifiedOn > LastModifiedOn)
                            LastModifiedOn = myHdr[i].ModifiedOn.Value;
                    }

                    for (int i = 0; i < myHdr.Count; i++)
                        myDet.AddRange(myHdr[i].AppointmentItemRecords());

                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        Membership member = new Membership(myHdr[i].MembershipNo);
                        if(member!= null && member.MembershipNo!="")
                            myMember.Add(member);
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
                    return true;


                }
                else
                {
                    objHdr = null;
                    objDet = null;
                    objMember = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objHdr = null;

                objDet = null;
                objMember = null;
                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Send Appointment failed." + ex.Message, true);
                Logger.writeLog(parametername);
                return false;
            }
        }

        public bool getAppointmentDataWithListID(List<Guid> objHdrID, out string[][] objHdr,
            out string[][] objDet, out string statusMsg)
        {
            string parametername = "";
            try
            {

                statusMsg = "";
                AppointmentCollection myHdr = new AppointmentCollection();

                if (objHdrID.Count() > 0)
                {
                    for (int i = 0; i < objHdrID.Count(); i++)
                    {
                        myHdr.Add(new Appointment(objHdrID[i]));
                    }

                    AppointmentItemCollection myDet = new AppointmentItemCollection();

                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        //myDet.AddRange(myHdr[i].AppointmentItemRecords());
                        string sql = "SELECT * FROM AppointmentItem WHERE AppointmentId = @AppointmentId";
                        QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                        cmd.AddParameter("@AppointmentId", myHdr[i].Id, DbType.Guid);
                        DataSet ds = DataService.GetDataSet(cmd);

                        if (ds != null && ds.Tables.Count > 0)
                        {
                            AppointmentItemCollection tmpDet = new AppointmentItemCollection();
                            tmpDet.Load(ds.Tables[0]);
                            myDet.AddRange(tmpDet);
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
                    return true;


                }
                else
                {
                    objHdr = null;
                    objDet = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objHdr = null;

                objDet = null;
                statusMsg = ex.Message;
                Logger.writeLog("Get Appointment Data." + ex.Message, true);
                Logger.writeLog(parametername);
                return false;
            }
        }


        public bool DownloadAppointment()
        {
            //if (!e.Cancel)
            //{
                try
                {
                    #region *) Load Config
                    Load_WS_URL();
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Sync Download Appointment Start");

                    //Basic Info from server is Item and User
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Url = WS_URL;

                    DataSet ds;

                    int retrySecConnected = 600;
                    int retrySecDisconnected = 5;
                    int recordsPerTime = 100;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UserRetrySecWhenConnected), out retrySecConnected))
                        retrySecConnected = 600;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UserRetrySecWhenDisconnected), out retrySecDisconnected))
                        retrySecDisconnected = 10;
                    if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncUserRecordsPerTime), out recordsPerTime))
                        recordsPerTime = 100;
                    #endregion

                    while (true)
                    {
                        if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMasterData), false))
                        {
                            #region Download PointOfSale
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("PointOfSale", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Point Of Sale on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("PointOfSale", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync Point Of Sale");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("PointOfSale", "PointOfSaleID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "PointOfSale", "PointOfSaleID", true))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Point Of Sale data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Point Of Sale"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Point Of Sale have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading User group. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                            #region Download Outlet
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Outlet", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Outlet on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var totalRecordID = ws.FetchRecordNoByTimestamp("Outlet", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync Outlet");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {

                                        var dataID = ws.FetchDataSetByTimestamp("Outlet", "OutletName", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "Outlet", "OutletName", false))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Outlet data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Outlet"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Outlet have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Outlet. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                            #region Download InventoryLocation
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("InventoryLocation", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Inventory Location on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("InventoryLocation", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync Inventory Location");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("InventoryLocation", "InventoryLocationID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "InventoryLocation", "InventoryLocationID", true))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download inventory Location data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Inventory Location"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Inventory Location have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Inventory Location. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                            #region Download Department
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Department", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Department on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("Department", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync Department");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("Department", "DepartmentID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "Department", "DepartmentID", true))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Department data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Department"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Department have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Department. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                        }

                        if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncUser), false))
                        {
                            #region Download UserMst
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserMst", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the User on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var totalRecordID = ws.FetchRecordNoByTimestamp("UserMst", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync user Master");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("UserMst", "UserName", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "UserMst", "UserName", false))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download User Master data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download User Master"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All User Master have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading User Master. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion
                        }

                        if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncProducts), false))
                        {
                            #region Download ItemDepartment
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemDepartment", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Item Department on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("ItemDepartment", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync item Department");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("ItemDepartment", "ItemDepartmentID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "ItemDepartment", "ItemDepartmentID", false))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Item Department data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Item Department"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Item Department have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Item Department. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                            #region Download Category
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Category", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Category on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("Category", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync Category");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("Category", "CategoryName", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "Category", "CategoryName", false))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Category data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Category"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Category have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Category. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                            #region Download Alternate Barcode
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("AlternateBarcode", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Alternate Barcode on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("AlternateBarcode", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Alternate Barcode");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("AlternateBarcode", "BarcodeID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "AlternateBarcode", "BarcodeID", true))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Alternate Barcode data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Alternate Barcode"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Alternate barcode have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Alternate Barcode. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion

                            #region Download Item
                            try
                            {
                                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Item", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Item on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var totalRecordID = ws.FetchRecordNoByTimestamp("Item", lastModifiedOnID);
                                if (totalRecordID > 0)
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Start Sync Item");

                                    int dataGetID = 0;
                                    int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                    while (dataGetID < totalRecordID)
                                    {
                                        var dataID = ws.FetchDataSetByTimestamp("Item", "ItemNo", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                        if (SyncRealTimeController.DownloadData(dsID, "Item", "ItemNo", false))
                                        {
                                            dataGetID += dsID.Tables[0].Rows.Count;
                                            if (dataGetID > totalRecordID)
                                                dataGetID = totalRecordID;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download Item data. {0} of {1} ", dataGetID, totalRecordID));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download Item"));
                                        }
                                    }

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "All Item have been synchronized successfully.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.writeLog(ex);
                                string msg = "Error downloading Item. Server is disconnected, will keep trying every {0} seconds until server is connected";
                                msg = string.Format(msg, retrySecDisconnected);
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, msg);
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                            #endregion
                        }

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

                        #region Download ResourceGroup
                        try
                        {
                            DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("ResourceGroup", out lastModifiedOnID))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the Resource Group on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordID = ws.FetchRecordNoByTimestamp("ResourceGroup", lastModifiedOnID);
                            if (totalRecordID > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync Resource Group");

                                int dataGetID = 0;
                                int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                                while (dataGetID < totalRecordID)
                                {
                                    var dataID = ws.FetchDataSetByTimestamp("ResourceGroup", "ResourceGroupID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                    var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                    if (!SyncRealTimeController.DownloadData(dsID, "ResourceGroup", "ResourceGroupID", true))
                                    {
                                        dataGetID += dsID.Tables[0].Rows.Count;
                                        if (dataGetID > totalRecordID)
                                            dataGetID = totalRecordID;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download Resource Group data. {0} of {1} ", dataGetID, totalRecordID));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download Resource Group"));
                                    }
                                }

                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All Resource Grouop have been synchronized successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading Rsource Group. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion

                        #region Download Resource
                        try
                        {
                            DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("Resource", out lastModifiedOnID))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the Resource on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordID = ws.FetchRecordNoByTimestamp("Resource", lastModifiedOnID);
                            if (totalRecordID > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync Resource");

                                int dataGetID = 0;
                                int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                while (dataGetID < totalRecordID)
                                {
                                var dataID = ws.FetchDataSetByTimestamp("Resource", "ResourceID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "Resource", "ResourceID", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Resource data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Resource"));
                                }
                                }

                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All Resource have been synchronized successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading Resource. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion

                        #region Download Appointment
                        try
                        {
                            DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("Appointment", out lastModifiedOnID))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the Appointment on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordID = ws.FetchRecordNoByTimestampAppointment(lastModifiedOnID, PointOfSaleInfo.PointOfSaleID);
                            if (totalRecordID > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync Appointment");

                                int dataGetID = 0;
                                int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                                while (dataGetID < totalRecordID)
                                {
                                   
                                    //var dataID = ws.FetchDataAppointmentByTimeStamp(lastModifiedOnID, totalRecordID, false, 0, true);
                                    var dataID = ws.FetchDataSetByTimestampAppointment(lastModifiedOnID, totalRecordID, true, PointOfSaleInfo.PointOfSaleID);
                                    var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                    bool isSuccess = true;
                                    string tempAppointmentID = "";
                                    
                                    for (int i = 0; i < dsID.Tables.Count; i++)
                                    {
                                        DataTable dtID = dsID.Tables[i];
                                        if (dtID.TableName.ToLower() == "appointment")
                                        {
                                            if (SyncRealTimeController.DownloadDataWithDataTable(dtID, "Appointment", "Id", false))
                                            {
                                                for (int j = 0; j< dtID.Rows.Count; j++)
                                                {
                                                    if (tempAppointmentID =="")
                                                    {
                                                        tempAppointmentID += dtID.Rows[j]["Id"].ToString();
                                                    }
                                                    else
                                                    {
                                                        tempAppointmentID += ("," + dtID.Rows[j]["Id"].ToString());
                                                    }
                                                }
                                                dataGetID += dtID.Rows.Count;
                                                if (dataGetID > totalRecordID)
                                                    dataGetID = totalRecordID;
                                                if (OnProgressUpdates != null)
                                                    OnProgressUpdates(this, string.Format("Download Appointment data. {0} of {1} ", dataGetID, totalRecordID));
                                                try
                                                {
                                                    AppointmentController.UpdateAppointmentIsUpdated(tempAppointmentID);
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
                                                    OnProgressUpdates(this, string.Format(">> Failed download Appointment"));
                                                isSuccess = false;
                                            }
                                        }
                                        else if (dtID.TableName.ToLower() == "appointmentitem")
                                        {
                                            if (SyncRealTimeController.DownloadDataWithDataTable(dtID, "AppointmentItem", "Id", false))
                                            {
                                                dataGetID += dtID.Rows.Count;
                                                if (dataGetID > totalRecordID)
                                                    dataGetID = totalRecordID;
                                                if (OnProgressUpdates != null)
                                                    OnProgressUpdates(this, string.Format("Download Appointment Item data. {0} of {1} ", dataGetID, totalRecordID));
                                                try
                                                {
                                                    if (OnDataDownloaded != null)
                                                        OnDataDownloaded(this, "Appointment Updated");
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
                                                    OnProgressUpdates(this, string.Format(">> Failed download Appointment Item"));
                                                isSuccess = false;
                                            }
                                        }
                                        else
                                            continue;
                                    }

                                    //update server
                                    if (isSuccess)
                                    {
                                        ws.UpdateAppointmentsIsUpdated(tempAppointmentID);
                                    }
                                    //foreach(DataRow dr in dataID)
                                }

                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All Appointment have been synchronized successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading Appointment. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        #endregion

                        #region Download AppointmentItem
                        //try
                        //{
                        //    DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        //    if (!SyncRealTimeController.GetLatestModifiedOnLocal("AppointmentItem", out lastModifiedOnID))
                        //    {
                        //        if (OnProgressUpdates != null)
                        //            OnProgressUpdates(this, "Error Getting Last Modified Date of the Appointment Item on Local");
                        //        Thread.Sleep(retrySecDisconnected * 1000);
                        //        continue;
                        //    }

                        //    var totalRecordID = ws.FetchRecordNoByTimestamp("AppointmentItem", lastModifiedOnID);
                        //    if (totalRecordID > 0)
                        //    {
                        //        if (OnProgressUpdates != null)
                        //            OnProgressUpdates(this, "Start Sync Appointment Item");

                        //        int dataGetID = 0;
                        //        int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;

                        //        while (dataGetID < totalRecordID)
                        //        {
                                   
                        //            var dataID = ws.FetchDataSetByTimestamp("AppointmentItem", "Id", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                        //            var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                        //            if (SyncRealTimeController.DownloadData(dsID, "AppointmentItem", "Id", false))
                        //            {
                        //                dataGetID += dsID.Tables[0].Rows.Count;
                        //                if (dataGetID > totalRecordID)
                        //                    dataGetID = totalRecordID;
                        //                if (OnProgressUpdates != null)
                        //                    OnProgressUpdates(this, string.Format("Download Appointment Item data. {0} of {1} ", dataGetID, totalRecordID));

                        //                try
                        //                {
                        //                    if (OnDataDownloaded != null)
                        //                        OnDataDownloaded(this, "Appointment Item Updated");
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    if (OnProgressUpdates != null)
                        //                        OnProgressUpdates(this, "Error occured : " + ex.Message);
                        //                    Thread.Sleep(retrySecDisconnected * 1000);
                        //                    Logger.writeLog(ex);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (OnProgressUpdates != null)
                        //                    OnProgressUpdates(this, string.Format(">> Failed download Appointment Item"));
                        //            }
                        //        }

                        //        if (OnProgressUpdates != null)
                        //            OnProgressUpdates(this, "All Appointment Item have been synchronized successfully.");
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //    Logger.writeLog(ex);
                        //    string msg = "Error downloading Appointment Item. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        //    msg = string.Format(msg, retrySecDisconnected);
                        //    if (OnProgressUpdates != null)
                        //        OnProgressUpdates(this, msg);
                        //    Thread.Sleep(retrySecDisconnected * 1000);
                        //    continue;
                        //}

                        #endregion

                        Thread.Sleep(retrySecConnected * 1000);
                    }
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Download appointment stopped");
                    return true;
                }
                catch (Exception ex)
                {
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Download Appointment with Error: " + ex.Message);

                    Logger.writeLog(ex);
                    return false;
                }
            //}
            //return false;
        }

        public bool SendAppointmentToServer(List<Guid> objHdrID, out string status)
        {
            status = "";
            try
            {
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;

                string[][] objHdr;
                string[][] objDet;
                string[][] objMember;


                if (!getAppointmentDataWithListID(objHdrID, out objHdr, out objDet, out status))
                {
                    throw new Exception("Error getting data");
                }

                bool result = ws.FetchAppointmentsCCMWRealTime(objHdr, objDet, null);
                if (!result)
                {
                    throw new Exception("Error send appointment in server. Please log to see the error.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Appontment failed : " + ex.Message);
                status = ex.Message;
                return false;
            }
        }

        public bool SendAppointmentToServer(AppointmentCollection appointment, AppointmentItemCollection appointmentitems, out string status)
        {
            bool Result = false;
            status = "";
            try
            {
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;

                DataSet SavedToServer = new DataSet();

                SavedToServer.Tables.Add(appointment.ToDataTable());
                SavedToServer.Tables.Add(appointmentitems.ToDataTable());

                if (!ws.SaveAppointment(SavedToServer))
                {
                    throw new Exception("Error created appointment in server. Please Contact administrator");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Appontment failed : " + ex.Message);
                status = ex.Message;
                Result = false;
            }

            return Result;
        }

        public bool SendAppointmentToServer(AppointmentCollection appointment, AppointmentItemCollection appointmentitems, MembershipCollection members, out string status)
        {
            bool Result = false;
            status = "";
            try
            {
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;

                DataSet SavedToServer = new DataSet();

                SavedToServer.Tables.Add(members.ToDataTable());
                SavedToServer.Tables.Add(appointment.ToDataTable());
                SavedToServer.Tables.Add(appointmentitems.ToDataTable());
                

                if (!ws.SaveAppointment(SavedToServer))
                {
                    throw new Exception("Error created appointment in server. Please Contact administrator");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Send Appontment failed : " + ex.Message);
                status = ex.Message;
                Result = false;
            }

            return Result;
        }

    }
}
