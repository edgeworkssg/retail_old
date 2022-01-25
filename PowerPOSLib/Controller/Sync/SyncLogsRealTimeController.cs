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
using PowerPOS;

namespace PowerPOS.SyncLogsController
{
    public delegate void UpdateProgress(object sender, string message);
    public class SyncLogsRealTimeController
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
        
        
        public bool getLocalOrderDataCount(DateTime lastModifiedOn,  out int dataCount, out string statusMsg)
        {
            statusMsg= "";
            try{

                CounterCloseLogCollection ohCol = new CounterCloseLogCollection();
                ohCol.Where(CounterCloseLog.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                ohCol.Where(CounterCloseLog.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                ohCol.OrderByAsc(CounterCloseLog.Columns.ModifiedOn);
                ohCol.Load();
                dataCount = ohCol.Count;
                return true;
            }
            catch (Exception ex)
            {
                dataCount =0;
                statusMsg = "";
                return false;
            }
        }

        public bool getLocalLoginActivityDataCount(DateTime lastModifiedOn, out int dataCount, out string statusMsg)
        {
            statusMsg = "";
            try
            {
                LoginActivityCollection ohCol = new LoginActivityCollection();
                ohCol.Where(LoginActivity.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                ohCol.Where(LoginActivity.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                ohCol.OrderByAsc(LoginActivity.Columns.ModifiedOn);
                ohCol.Load();
                dataCount = ohCol.Count;
                return true;
            }
            catch (Exception ex)
            {
                dataCount = 0;
                statusMsg = "";
                return false;
            }
        }

        

        public bool SendLogs()
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
                DateTime lastClosingModifiedOn = DateTime.Today;
                int tryTimes = 0;
                bool isSuccessConnecting = false;
                bool result = false;
                #endregion
                while (true)
                {

                    //Logger.writeLog(tryTimes.ToString());
                    if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Logs Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;
                    #region Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getCounterCloselogLastTimeStamp(PointOfSaleInfo.PointOfSaleID, out lastClosingModifiedOn);
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
                        if (!getLocalOrderDataCount(lastClosingModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (dataCount==0) 
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync Logs is completed.");
                              //everything completed successfully, return true
                              return true;
                        }
                        
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

                        #region init variables
                        int dataSent = 0;
                        int recordPerTimes = 0;
                        string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncLogsPerTime);
                        if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                        {
                            recordPerTimes = 100;
                        }
                        #endregion
                        while (dataSent < dataCount)
                        {
                            
                            string[][] objCounterCloseLog;
                            string[][] objCounterCloseDet;
                            
                                //send sales
                            DateTime lastModifiedOnPerTimes;
                            if (!getOrderData(lastClosingModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes, 
                                out objCounterCloseLog, out objCounterCloseDet,
                                out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch Order Data Failed.");
                                return false;
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objCounterCloseLog.Length) + " of " + dataCount + " to Server...");
                            

                            try
                            {
                                result = ws.FetchCounterCloseLogWithDetailRealTime(objCounterCloseLog, objCounterCloseDet);
                                if (result)
                                {
                                    lastClosingModifiedOn = lastModifiedOnPerTimes;
                                    SyncClientController.deductInventory();
                                    // send data to server is successful, update the count
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objCounterCloseLog.Length) + " of " + dataCount + " to Server is completed successfully.");
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

        public bool SendLoginActivity()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Login Activity Start");
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
                        OnProgressUpdates(this, "Sync Login Activity Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;
                    #region Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getLastTimeStampByPOSID("LoginActivity",PointOfSaleInfo.PointOfSaleID, out lastClosingModifiedOn);
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
                        if (!getLocalLoginActivityDataCount(lastClosingModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (dataCount == 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync Login Activity is completed.");
                            //everything completed successfully, return true
                            return true;
                        }

                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

                        #region init variables
                        int dataSent = 0;
                        int recordPerTimes = 0;
                        string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncLogsPerTime);
                        if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                        {
                            recordPerTimes = 100;
                        }
                        #endregion
                        while (dataSent < dataCount)
                        {

                            string[][] objCounterCloseLog;


                            //send sales
                            DateTime lastModifiedOnPerTimes;
                            if (!getLoginActivityData(lastClosingModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes,
                                out objCounterCloseLog,
                                out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch Login ACtivity Data Failed.");
                                return false;
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objCounterCloseLog.Length) + " of " + dataCount + " to Server...");


                            try
                            {
                                result = ws.FetchLoginActivityRealTime(objCounterCloseLog);
                                if (result)
                                {
                                    lastClosingModifiedOn = lastModifiedOnPerTimes;
                                    // send data to server is successful, update the count
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objCounterCloseLog.Length) + " of " + dataCount + " to Server is completed successfully.");
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




        public bool getOrderData(DateTime StartDate, int numofRecords, out string[][] objCounterCloseLog, out string[][] objCounterCloseDet,
            out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {
                
                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";


                CounterCloseLogCollection myHdr = new CounterCloseLogCollection();

                DataTable dt = SyncClientController.FetchCounterCloseLogWithPOSID
                    (StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);

                    CounterCloseDetCollection myDet = new CounterCloseDetCollection();

                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        Query qDet = CounterCloseDet.CreateQuery();
                        qDet.AddWhere(CounterCloseDet.Columns.CounterCloseID, myHdr[i].CounterCloseID);

                        myDet.LoadAndCloseReader(qDet.ExecuteReader());
                    }

                    //-----------------------------------------------------------------------------------------------                

                    objCounterCloseLog = new string[myHdr.Count][];
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
                        objCounterCloseLog[count] = objArr;
                        count++;
                    }

                    objCounterCloseDet = new string[myDet.Count][];

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
                            else
                            {
                                if (param[oT].ParameterName.ToLower() == "@modifiedon")
                                {
                                    objArr[oT] = myDet[op].ModifiedOn.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                }

                                if (param[oT].ParameterName.ToLower() == "@createdon")
                                {
                                    objArr[oT] = myDet[op].CreatedOn.GetValueOrDefault(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                }

                                if (param[oT].ParameterName.ToLower() == "@createdby")
                                {
                                    objArr[oT] = myDet[op].CreatedBy;
                                }

                                if (param[oT].ParameterName.ToLower() == "@modifiedby")
                                {
                                    objArr[oT] = myDet[op].ModifiedBy;
                                }
                            }

                            //if (objArr[oT] != null && objArr[oT].IndexOf(char.ConvertFromUtf32(30)) > -1)
                            //    objArr[oT] = objArr[oT].Replace(char.ConvertFromUtf32(30).ToString(), string.Empty);
                        }
                        objCounterCloseDet[op] = objArr;
                    }                   

                    return true;                    
                    
                }
                else
                {
                    objCounterCloseLog = null;
                    objCounterCloseDet  = null;
                    statusMsg = "";
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                objCounterCloseLog = null;
                objCounterCloseDet = null;

                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Get Closing log failed." + ex.Message, true);
                return false;
            }
        }

        public bool getLoginActivityData(DateTime StartDate, int numofRecords, out string[][] objCounterCloseLog,
            out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {

                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";


                LoginActivityCollection myHdr = new LoginActivityCollection();

                DataTable dt = SyncClientController.FetchLoginActivityWithPOSID
                    (StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);

                    LastModifiedOn = StartDate;
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        if (myHdr[i].ModifiedOn > LastModifiedOn)
                            LastModifiedOn = myHdr[i].ModifiedOn;
                    }

                    //-----------------------------------------------------------------------------------------------                

                    objCounterCloseLog = new string[myHdr.Count][];
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
                        objCounterCloseLog[count] = objArr;
                        count++;
                    }

                    return true;


                }
                else
                {
                    objCounterCloseLog = null;
                    statusMsg = "";

                    return true;
                }
            }
            catch (Exception ex)
            {
                objCounterCloseLog = null;
                statusMsg = ex.Message;
                LastModifiedOn = StartDate;
                Logger.writeLog("Get Closing log failed." + ex.Message, true);
                return false;
            }
        }

    }
}

;