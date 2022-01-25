using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS;
using PowerPOSLib.PowerPOSSync;
using SubSonic;
using System.Globalization;
using PowerPOS.Container;
using System.Threading;
using PowerPOS.Controller;

namespace PowerPOS.SyncPerformanceLogController
{
    public delegate void UpdateProgress(object sender, string message);
    public class SyncPerformanceLogRealTimeController
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

        public bool getLocalPerfLogDataCount(DateTime lastModifiedOn, out int dataCount, out string statusMsg)
        {
            statusMsg = "";
            try
            {
                PerformanceLogCollection plCol = new PerformanceLogCollection();
                plCol.Where(PerformanceLog.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                plCol.Where(PerformanceLog.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                plCol.OrderByAsc(PerformanceLog.Columns.ModifiedOn);
                plCol.Load();
                dataCount = plCol.Count;
                return true;
            }
            catch (Exception ex)
            {
                dataCount = 0;
                statusMsg = "";
                return false;
            }
        }

        public bool getLocalPerfSummDataCount(DateTime lastModifiedOn, out int dataCount, out string statusMsg)
        {
            statusMsg = "";
            try
            {
                PerformanceLogSummaryCollection plCol = new PerformanceLogSummaryCollection();
                plCol.Where(PerformanceLogSummary.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                plCol.Where(PerformanceLogSummary.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                plCol.OrderByAsc(PerformanceLogSummary.Columns.ModifiedOn);
                plCol.Load();
                dataCount = plCol.Count;
                return true;
            }
            catch (Exception ex)
            {
                dataCount = 0;
                statusMsg = "";
                return false;
            }
        }

        public bool SendPerformanceLog()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Performance Log Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DateTime lastClosingModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                #endregion
                while (true)
                {
                    //Logger.writeLog(tryTimes.ToString());
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Sync Performance Log Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;
                    #region Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getLastTimeStampByPOSID("PerformanceLog", PointOfSaleInfo.PointOfSaleID, out lastClosingModifiedOn);
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
                            OnProgressUpdates(this, "Last Modified On from Server : " + lastClosingModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //Logger.writeLog("Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        int dataCount = 0;
                        string statusMsg;
                        if (!getLocalPerfLogDataCount(lastClosingModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (dataCount == 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync PerformanceLog is completed.");
                            //everything completed successfully, return true
                            return true;
                        }

                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

                        #region init variables
                        int dataSent = 0;
                        int recordPerTimes = 0;
                        string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncPerformanceLogPerTime);
                        if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                        {
                            recordPerTimes = 100;
                        }
                        #endregion
                        while (dataSent < dataCount)
                        {
                            string[][] objLog;

                            //send log
                            DateTime lastModifiedOnPerTimes;
                            if (!getPerfLogData(lastClosingModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes,
                                out objLog,
                                out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch PerformanceLog Data Failed.");
                                return false;
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objLog.Length) + " of " + dataCount + " to Server...");

                            try
                            {
                                result = ws.FetchPerformanceLogRealTime(objLog);
                                if (result)
                                {
                                    lastClosingModifiedOn = lastModifiedOnPerTimes;
                                    // send data to server is successful, update the count
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objLog.Length) + " of " + dataCount + " to Server is completed successfully.");
                                    dataSent += recordPerTimes;
                                }
                            }
                            catch (Exception ex)
                            {
                                //server connection problem
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Server is disconnected, will keep trying every 5 seconds until server is connected");
                                //Logger.writeLog("Error Connecting To Server. " + ex.Message);

                                //break out from sending data loop, so that the thread will try to reconnect to server again
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
        }

        public bool SendPerformanceLogSummary()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Performance Log Summary Start");
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = WS_URL;
                DateTime lastClosingModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                #endregion
                while (true)
                {
                    //Logger.writeLog(tryTimes.ToString());
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Sync Performance Log Summary Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;
                    #region Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getLastTimeStampByPOSID("PerformanceLogSummary", PointOfSaleInfo.PointOfSaleID, out lastClosingModifiedOn);
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
                            OnProgressUpdates(this, "Last Modified On from Server : " + lastClosingModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //Logger.writeLog("Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                        int dataCount = 0;
                        string statusMsg;
                        if (!getLocalPerfSummDataCount(lastClosingModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (dataCount == 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync PerformanceLogSummary is completed.");
                            //everything completed successfully, return true
                            return true;
                        }

                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

                        #region init variables
                        int dataSent = 0;
                        int recordPerTimes = 0;
                        string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncPerformanceLogPerTime);
                        if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                        {
                            recordPerTimes = 100;
                        }
                        #endregion
                        while (dataSent < dataCount)
                        {
                            string[][] objLog;

                            //send log
                            DateTime lastModifiedOnPerTimes;
                            if (!getPerfSummData(lastClosingModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes,
                                out objLog,
                                out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch PerformanceLogSummary Data Failed.");
                                return false;
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objLog.Length) + " of " + dataCount + " to Server...");

                            try
                            {
                                result = ws.FetchPerformanceLogSummaryRealTime(objLog);
                                if (result)
                                {
                                    lastClosingModifiedOn = lastModifiedOnPerTimes;
                                    // send data to server is successful, update the count
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objLog.Length) + " of " + dataCount + " to Server is completed successfully.");
                                    dataSent += recordPerTimes;
                                }
                            }
                            catch (Exception ex)
                            {
                                //server connection problem
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Server is disconnected, will keep trying every 5 seconds until server is connected");
                                //Logger.writeLog("Error Connecting To Server. " + ex.Message);

                                //break out from sending data loop, so that the thread will try to reconnect to server again
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex1) { Logger.writeLog(ex1.Message); return false; }
        }

        public bool getPerfLogData(DateTime StartDate, int numofRecords, out string[][] objLog,
            out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {
                statusMsg = "";

                PerformanceLogCollection myHdr = new PerformanceLogCollection();

                DataTable dt = SyncClientController.FetchTableDataNotInServerWithPOSID
                    ("PerformanceLog", StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);

                    //-----------------------------------------------------------------------------------------------                

                    objLog = new string[myHdr.Count][];
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
                        objLog[count] = objArr;
                        count++;
                    }

                    return true;
                }
                else
                {
                    objLog = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objLog = null;
                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Get PerformanceLog failed." + ex.Message, true);
                return false;
            }
        }

        public bool getPerfSummData(DateTime StartDate, int numofRecords, out string[][] objLog,
            out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {
                statusMsg = "";

                PerformanceLogSummaryCollection myHdr = new PerformanceLogSummaryCollection();

                DataTable dt = SyncClientController.FetchTableDataNotInServerWithPOSID
                    ("PerformanceLogSummary", StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);

                    //-----------------------------------------------------------------------------------------------                

                    objLog = new string[myHdr.Count][];
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
                        objLog[count] = objArr;
                        count++;
                    }

                    return true;
                }
                else
                {
                    objLog = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objLog = null;
                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Get PerformanceLogSummary failed." + ex.Message, true);
                return false;
            }
        }
    }
}
