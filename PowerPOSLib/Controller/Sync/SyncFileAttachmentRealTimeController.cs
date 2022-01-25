using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;
using System.Threading;

namespace PowerPOS.SyncFileAttachmentController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncFileAttachmentRealTimeController
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

        public bool SyncFileAttachment(string moduleName)
        {
            try
            {
                #region *) Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync FileAttachment Start");

                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                DataSet ds;

                int retrySecConnected = 600;
                int retrySecDisconnected = 5;
                int recordsperTime = 100;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncMasterDataRecordsPerTime), out recordsperTime))
                    recordsperTime = 100;

                #endregion

                #region *) Download FileAttachment
                var totalRecordID = 0;
                try
                {
                    DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                    if (!GetLatestModifiedOnLocalPerModuleName("FileAttachment", moduleName, out lastModifiedOnID))
                    {
                        Thread.Sleep(retrySecDisconnected * 1000);
                        return false;
                    }
                    Logger.writeLog("Last Modified On Local FileAttachment module " + moduleName + " : " + lastModifiedOnID.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    totalRecordID = ws.FetchRecordNoByTimestampByModuleName("FileAttachment", lastModifiedOnID, moduleName);
                    Logger.writeLog("Total Record FileAttachment module " + moduleName + " : " + totalRecordID.ToString());
                    if (totalRecordID > 0)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Start Sync FileAttachment");

                        int dataGetID = 0;
                        int recordPerTimeID = totalRecordID < recordsperTime ? totalRecordID : recordsperTime;
                        while (dataGetID < totalRecordID)
                        {
                            if (!GetLatestModifiedOnLocalPerModuleName("FileAttachment", moduleName, out lastModifiedOnID))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting FileAttachment Last Modified Date on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                return false;
                            }

                            var dataID = ws.FetchDataSetByTimeStampPerModuleName("FileAttachment", "AttachmentID", lastModifiedOnID, recordsperTime, moduleName);
                            var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                            if (SyncRealTimeController.DownloadData(dsID, "FileAttachment", "AttachmentID", false))
                            {
                                dataGetID += dsID.Tables[0].Rows.Count;
                                if (dataGetID > totalRecordID)
                                    dataGetID = totalRecordID;
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, string.Format("Download FileAttachment data. {0} of {1} ", dataGetID, totalRecordID));
                            }
                            else
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, string.Format(">> Failed download FileAttachment"));
                                Thread.Sleep(retrySecDisconnected * 1000);
                                return false;
                            }
                        }
                        
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "All FileAttachment have been synchronized successfully.");

                    }
                    //return true;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    string msg = "Error downloading FileAttachment. Server is disconnected, will keep trying every {0} seconds until server is connected";
                    msg = string.Format(msg, retrySecDisconnected);
                    if (OnProgressUpdates != null)
                        OnProgressUpdates(this, msg);
                    Thread.Sleep(retrySecDisconnected * 1000);
                    return false;
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync FileAttachment stopped : " + ex.Message);
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool GetLatestModifiedOnLocalPerModuleName(string tableName, string moduleName, out DateTime result)
        {
            result = DateTime.Now;
            try
            {
                string sqlString = "SELECT ISNULL(MAX(ModifiedOn),DATEADD(YEAR,-10,GETDATE())) TheDate FROM " + tableName + " Where ModuleName = '" + moduleName + "'";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sqlString)));
                if (dt.Rows.Count > 0)
                    result = (DateTime)dt.Rows[0]["TheDate"];
                else
                    result = DateTime.Now;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
            return true;
        }

    }
}
