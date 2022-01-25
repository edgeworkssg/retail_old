using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using PowerPOS.Container;
using SubSonic;

namespace PowerPOS.SyncAttendanceRealTimeController
{

    public delegate void UpdateProgress(object sender, string message);
   
    public class SyncAttendanceRealTimeController
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

        public bool GetLocalDataCount(DateTime lastModifiedOn, out int dataCount, out string statusMsg)
        {
            bool isSuccess = false;
            statusMsg = "";
            dataCount = 0;
            try
            {
                string sql = @"SELECT	COUNT(*) DataCount
                                FROM	MembershipAttendance 
                                WHERE	PointOfSaleID = {0}
		                                AND ModifiedOn > '{1}'";
                sql = string.Format(sql, PointOfSaleInfo.PointOfSaleID, lastModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Rows.Count > 0)
                    dataCount = (dt.Rows[0]["DataCount"] + "").GetIntValue();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                dataCount = 0;
                statusMsg = "Error : " + ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        public bool GetLocalData(DateTime StartDate, int numofRecords, out string[][] objAccessLog,
            out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {

                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";

                MembershipAttendanceCollection myHdr = new MembershipAttendanceCollection();

                DataTable dt = SyncClientController.FetchAttendanceNotInServerWithPOSID(StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);
                    LastModifiedOn = StartDate;
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        myHdr[i].UniqueID = new Guid(dt.Rows[i]["UniqueID"].ToString());
                        if (myHdr[i].ModifiedOn.GetValueOrDefault(DateTime.Now) > LastModifiedOn)
                            LastModifiedOn = myHdr[i].ModifiedOn.GetValueOrDefault(DateTime.Now);
                    }

                    //-----------------------------------------------------------------------------------------------                

                    objAccessLog = new string[myHdr.Count][];
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
                                    objArr[oT] = myHdr[op].ModifiedOn.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                }

                                if (param[oT].ParameterName.ToLower() == "@createdon")
                                {
                                    objArr[oT] = myHdr[op].CreatedOn.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff");
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
                        objAccessLog[count] = objArr;
                        count++;
                    }
                    return true;
                }
                else
                {
                    objAccessLog = null;
                    statusMsg = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                objAccessLog = null;
                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Send Attendance failed." + ex.Message, true);
                return false;
            }
        }

        public bool SendRealTimeAttendance()
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
                int retrySecConnected = 600;
                int recordsPerTime = 100;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.AttendanceRetrySecWhenDisconnected), out retrySecConnected))
                    retrySecConnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAttendanceRecordsPerTime), out recordsPerTime))
                    recordsPerTime = 100;

                #endregion
                while (true)
                {

                    //Logger.writeLog(tryTimes.ToString());
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Sync Attendance Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;
                    #region *) Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            lastSalesModifiedOn = ws.GetAttendanceLastTimeStamp(PointOfSaleInfo.PointOfSaleID);
                            isSuccessConnecting = true;
                        }
                        catch (Exception ex)
                        {
                            if (tryTimes == 1)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Failed To Connect To Server. Retrying in 5 sec.");
                                Logger.writeLog("Error Connecting To Server. " + ex.Message);
                            }
                            Thread.Sleep(retrySecConnected * 1000);
                        }
                    }
                    #endregion

                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Server is Connected");

                    if (isSuccessConnecting)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //Logger.writeLog("Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        int dataCount = 0;
                        string statusMsg;
                        if (!GetLocalDataCount(lastSalesModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            //Thread.Sleep(retrySecConnected * 1000);
                            //continue;
                            return false;
                        }

                        if (dataCount == 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync Attendance is completed.");
                            //Thread.Sleep(retrySecConnected * 1000);
                            //continue;
                            return true;
                        }

                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

                        int dataSent = 0;
                        while (dataSent < dataCount)
                        {

                            string[][] objAttendance;

                            //send sales
                            DateTime lastModifiedOnPerTimes;
                            if (!GetLocalData(lastSalesModifiedOn, recordsPerTime > dataCount - dataSent ? dataCount - dataSent : recordsPerTime,
                                out objAttendance, out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch Attendance Data Failed.");
                                //Thread.Sleep(retrySecConnected * 1000);
                                //continue;
                                return false;
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objAttendance.Length) + " of " + dataCount + " to Server...");


                            try
                            {
                                result = ws.FetchAAttendanceRealTime(objAttendance);
                                if (result)
                                {
                                    lastSalesModifiedOn = lastModifiedOnPerTimes;

                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objAttendance.Length) + " of " + dataCount + " to Server is completed successfully.");
                                    dataSent += recordsPerTime;
                                }
                            }
                            catch (Exception ex)
                            {
                                //server connection problem
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Server is disconnected, will keep trying every 5 seconds until server is connected");
                                Logger.writeLog("Error Connecting To Server. " + ex.Message);

                                //break out from sending data loop, so that the thread will try to reconnect to server again
                                //Thread.Sleep(retrySecConnected * 1000);
                                //continue;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex1)
            {
                Logger.writeLog(ex1.Message);
                return false;
            }
        }

    }
}
