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

namespace PowerPOS.SalesController
{
    public delegate void UpdateProgress(object sender, string message);
    public class SyncSalesRealTimeController
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
        
        public bool getLocalOrderDataCount(DateTime lastModifiedOn, out int dataCount, out string statusMsg)
        {
            statusMsg= "";
            try{
                
                OrderHdrCollection ohCol = new OrderHdrCollection();
                ohCol.Where(OrderHdr.Columns.ModifiedOn, Comparison.GreaterThan, lastModifiedOn);
                ohCol.Where(OrderHdr.Columns.PointOfSaleID, PointOfSaleInfo.PointOfSaleID);
                ohCol.OrderByAsc(OrderHdr.Columns.ModifiedOn);
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

        public bool SendRealTimeSales()
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
                while (true)
                {

                    //Logger.writeLog(tryTimes.ToString());
                    if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Sales Started. Connecting To Server");
                    tryTimes = 0;
                    isSuccessConnecting = false;
                    #region Connecting & Get Last TimeStamp
                    while (!isSuccessConnecting)
                    {
                        tryTimes++;
                        try
                        {
                            isSuccessConnecting = ws.getSalesLastTimeStamp(PointOfSaleInfo.PointOfSaleID, out lastSalesModifiedOn);
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
                            OnProgressUpdates(this, "Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        //Logger.writeLog("Last Modified On from Server : " + lastSalesModifiedOn.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                        
                        int dataCount = 0;
                        string statusMsg;
                        if (!getLocalOrderDataCount(lastSalesModifiedOn, out dataCount, out statusMsg))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Get Local Data Count. " + statusMsg);
                            return false;
                        }

                        if (dataCount==0) 
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All data has been sent successfully to Server. Sync Sales is completed.");
                              //everything completed successfully, return true
                              return true;
                        }
                        
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Total Records to be Synced : " + dataCount.ToString());

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
                            string[][] objSalesRec;
                            string[][] objReceiptHdr;
                            string[][] objReceiptDet;
                            string[][] objMembership;
                            string[][] objVoidLog;
                            string[][] objDetUOMConv;
                            
                                //send sales
                            DateTime lastModifiedOnPerTimes;
                            if (!getOrderData(lastSalesModifiedOn, recordPerTimes > dataCount - dataSent ? dataCount-dataSent : recordPerTimes, out objHdr,
                                out objDet, out objReceiptHdr, out objReceiptDet, out objSalesRec, out objMembership, out objVoidLog, out objDetUOMConv, out statusMsg, out lastModifiedOnPerTimes))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Fetch Order Data Failed.");
                                return false;
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Sending records #" + (dataSent + 1) + "-" + (dataSent + objHdr.Length) + " of " + dataCount + " to Server...");
                            

                            try
                            {
                                result = ws.FetchOrdersCCMWRealTime(objHdr, objDet, objReceiptHdr, objReceiptDet, objSalesRec, objMembership, objVoidLog, objDetUOMConv);
                                if (result)
                                {
                                    lastSalesModifiedOn = lastModifiedOnPerTimes;
                                    SyncClientController.deductInventory();
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


        public bool getOrderData(DateTime StartDate, int numofRecords, out string[][] objHdr, 
            out string[][] objDet, out string[][] objReceiptHdr, out string[][] objReceiptDet,
            out string[][] objSalesRec, out string[][] objMembership, out string[][] objVoidLog,
            out string[][] objDetUOMConv, out string statusMsg, out DateTime LastModifiedOn)
        {
            try
            {
                
                string[][] orderHdrIDList = new String[0][];
                statusMsg = "";

                OrderHdrCollection myHdr = new OrderHdrCollection();

                DataTable dt = SyncClientController.FetchOrderHdrNotInServerWithPOSID
                    (StartDate, numofRecords, PointOfSaleInfo.PointOfSaleID);
                LastModifiedOn = StartDate;
                if (dt != null)
                {
                    myHdr.Load(dt);
                    
                    OrderDetCollection myDet = new OrderDetCollection();
                    ReceiptHdrCollection myRcptHdr = new ReceiptHdrCollection();
                    ReceiptDetCollection myRcptDet = new ReceiptDetCollection();
                    SalesCommissionRecordCollection mySalesRecord = new SalesCommissionRecordCollection();
                    MembershipCollection myMembers = new MembershipCollection();
                    VoidLogCollection myVoidLog = new VoidLogCollection();
                    OrderDetUOMConversionCollection myDetUOMConv = new OrderDetUOMConversionCollection();

                    LastModifiedOn = StartDate;
                    for (int i = 0; i < myHdr.Count; i++)
                    {
                        myHdr[i].UniqueID = new Guid(dt.Rows[i]["UniqueID"].ToString());
                        if (myHdr[i].ModifiedOn > LastModifiedOn)
                            LastModifiedOn = myHdr[i].ModifiedOn;
                    }
                    
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

                            //if (objArr[oT] != null && objArr[oT].IndexOf(char.ConvertFromUtf32(30)) > -1)
                            //    objArr[oT] = objArr[oT].Replace(char.ConvertFromUtf32(30).ToString(), string.Empty);
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
                LastModifiedOn = StartDate;
                Logger.writeLog("Send Order failed." + ex.Message, true);
                return false;
            }
        }

    }
}

;