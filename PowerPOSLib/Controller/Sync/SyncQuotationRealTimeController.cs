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

namespace PowerPOS.SyncQuotation
{
    public delegate void UpdateProgress(object sender, string message);
    public class SyncQuotationRealTimeController
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
        
        public bool GetLocalQuotationDataCount(DateTime lastModifiedOn,  out int dataCount, out string statusMsg)
        {
            statusMsg= "";
            try{

                QuotationHdrCollection ohCol = new QuotationHdrCollection();
                ohCol.Where(QuotationHdr.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                ohCol.Where(QuotationHdr.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                ohCol.OrderByAsc(QuotationHdr.Columns.ModifiedOn);
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

        public bool SendQuotations()
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
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, "Sync Quotation Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;

                    #region *) Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getQuotationLastTimeStamp(PointOfSaleInfo.PointOfSaleID, out lastClosingModifiedOn);
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
                        OnProgressUpdates(this, "Server is connected");

                    if (isSuccessConnecting)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Last Modified On from Server : " + lastClosingModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //Logger.writeLog("Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        
                        int dataCount = 0;
                        string statusMsg;
                        if (!GetLocalQuotationDataCount(lastClosingModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (dataCount==0) 
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync quotation is completed.");
                              return true;
                        }
                        
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

                        #region *) Init Variables
                        
                        int dataSent = 0;
                        int recordPerTimes = 0;
                        string tempRecordPerTimes = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncQuotationPerTime);
                        if (!int.TryParse(tempRecordPerTimes, out recordPerTimes))
                            recordPerTimes = 100;

                        #endregion

                        while (dataSent < dataCount)
                        {
                            
                            string[][] objHdr;
                            string[][] objDet;                            
                                //send sales
                            DateTime lastModifiedOnPerTimes;
                            if (!GetQuotationData(lastClosingModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount - dataSent : recordPerTimes,
                                out objHdr, out objDet, out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch Quotation Data Failed.");
                                return false;
                            }

                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server...");

                            try
                            {
                                result = ws.FetchQuotationRealTime(objHdr, objDet);

                                if (result)
                                {
                                    lastClosingModifiedOn = lastModifiedOnPerTimes;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server is completed successfully.");
                                    dataSent += recordPerTimes;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Server is disconnected, will keep trying every 5 seconds until server is connected");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex1) 
            { 
                Logger.writeLog(ex1.Message); return false; 
            }
        }

        public bool GetQuotationData(DateTime StartDate, int numofRecords, 
            out string[][] objHdr,out string[][] objDet, 
            out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {
                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";
                QuotationHdrCollection myHdr = new QuotationHdrCollection();
                QuotationDetCollection myDet = new QuotationDetCollection();
                DataTable dt = SyncClientController.FetchQuotationHdrNotInServerWithPOSID(StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;

                if (dt != null)
                {
                    myHdr.Load(dt);

                    LastModifiedOn = StartDate;
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        myHdr[i].UniqueID = new Guid(dt.Rows[i]["UniqueID"].ToString());
                        if (myHdr[i].ModifiedOn > LastModifiedOn)
                            LastModifiedOn = myHdr[i].ModifiedOn;
                    }

                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        var quoteDet = new QuotationDetCollection();
                        quoteDet.Where(QuotationDet.Columns.OrderHdrID, myHdr[i].OrderHdrID);
                        quoteDet.Load();
                        myDet.AddRange(quoteDet);
                    }

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
                                    objArr[oT] = param[oT].ParameterValue != null ? ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss.fff") : null;
                                else
                                    objArr[oT] = param[oT].ParameterValue.ToString();
                            }
                            else
                            {
                                if (param[oT].ParameterName.ToLower() == "@modifiedon")
                                    objArr[oT] = myHdr[op].ModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff");

                                if (param[oT].ParameterName.ToLower() == "@createdon")
                                    objArr[oT] = myHdr[op].CreatedOn.ToString("yyyy-MM-dd HH:mm:ss.fff");

                                if (param[oT].ParameterName.ToLower() == "@createdby")
                                    objArr[oT] = myHdr[op].CreatedBy;

                                if (param[oT].ParameterName.ToLower() == "@modifiedby")
                                    objArr[oT] = myHdr[op].ModifiedBy;
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
                                    objArr[oT] = ((DateTime)param[oT].ParameterValue).ToString("yyyy-MM-dd HH:mm:ss");
                                else
                                    objArr[oT] = param[oT].ParameterValue.ToString();
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
                LastModifiedOn = StartDate;
                Logger.writeLog("Send Quotation failed." + ex.Message, true);
                return false;
            }
        }

    }
}