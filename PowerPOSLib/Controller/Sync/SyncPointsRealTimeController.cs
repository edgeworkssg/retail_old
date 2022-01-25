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

namespace PowerPOS.SyncPointsController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncPointsRealTimeController
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

        #region *) OBSOLETE
        //public bool SyncPoints(string membershipNo)
        //{
        //    try
        //    {
        //        Membership m = new Membership(membershipNo);

        //        //#region *) Check if sync Points/Packages is necessary or not
        //        //// If points last sync date still on the same day and no other transactions for this member, no need to try to download points
        //        //bool needSync = false;
        //        //if (m != null && m.MembershipNo == membershipNo)
        //        //{
        //        //    DateTime lastSync;
        //        //    if (string.IsNullOrEmpty(m.LastSyncPointsDate) || !DateTime.TryParse(m.LastSyncPointsDate, out lastSync))
        //        //    {
        //        //        needSync = true;
        //        //    }
        //        //    else if (lastSync.Date < DateTime.Today)
        //        //    {
        //        //        needSync = true;
        //        //    }
        //        //    else
        //        //    {
        //        //        DataTable dt = m.GetPastTransaction(lastSync.AddSeconds(1), DateTime.Now);
        //        //        if (dt != null && dt.Rows.Count > 0)
        //        //            needSync = true;
        //        //    }
        //        //}
        //        //if (!needSync) return true;
        //        //#endregion

        //        #region *) Load Config
        //        Load_WS_URL();
        //        if (OnProgressUpdates != null)
        //            OnProgressUpdates(this, "Sync Product Start");

        //        //Basic Info from server is Item and User
        //        PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
        //        ws.Url = WS_URL;

        //        DataSet ds;

        //        int retrySecConnected = 600;
        //        int retrySecDisconnected = 5;
        //        int recordsperTime = 100;
        //        if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenConnected), out retrySecConnected))
        //            retrySecConnected = 600;
        //        if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenDisconnected), out retrySecDisconnected))
        //            retrySecDisconnected = 10;
        //        if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncMasterDataRecordsPerTime), out recordsperTime))
        //            recordsperTime = 100;

        //        #endregion

        //        DateTime timeSyncPointStarted; 
        //        TimeSpan ts;

        //        #region *) Download MembershipPoints
        //        var totalRecordID = 0;
        //            try
        //            {
        //                DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
        //                if (!GetLatestModifiedOnLocalPerMember("MembershipPoints", membershipNo, out lastModifiedOnID))
        //                {
        //                    Thread.Sleep(retrySecDisconnected * 1000);
        //                    return false;
        //                }
        //                Logger.writeLog("Last Modified On Local MP " + membershipNo + " : " +lastModifiedOnID.ToString("yyyy-MM-dd HH:mm:ss.fff"));

        //                timeSyncPointStarted = DateTime.Now;
        //                totalRecordID = ws.FetchRecordNoByTimestampByMember("MembershipPoints", lastModifiedOnID, membershipNo);
        //                ts = DateTime.Now - timeSyncPointStarted;
        //                Logger.writeLog(string.Format("MembershipPoints ws.FetchRecordNoByTimestampByMember completed in {0} seconds.", ts.Seconds.ToString()));

        //                Logger.writeLog("Total Record MembershipPoints " + membershipNo + " : " + totalRecordID.ToString());
        //                if (totalRecordID > 0)
        //                {
        //                    if (OnProgressUpdates != null)
        //                        OnProgressUpdates(this, "Start Sync MembershipPoints");

        //                    int dataGetID = 0;
        //                    int recordPerTimeID = totalRecordID < recordsperTime ? totalRecordID : recordsperTime;
        //                    while (dataGetID < totalRecordID)
        //                    {
        //                        if (!GetLatestModifiedOnLocalPerMember("MembershipPoints", membershipNo, out lastModifiedOnID))
        //                        {
        //                            if (OnProgressUpdates != null)
        //                                OnProgressUpdates(this, "Error Getting MembershipPoints Last Modified Date on Local");
        //                            Thread.Sleep(retrySecDisconnected * 1000);
        //                            return false;
        //                        }

        //                        timeSyncPointStarted = DateTime.Now;
        //                        var dataID = ws.FetchDataSetByTimeStampPerMember("MembershipPoints", "PointID", lastModifiedOnID, recordsperTime, membershipNo);
        //                        ts = DateTime.Now - timeSyncPointStarted;
        //                        Logger.writeLog(string.Format("MembershipPoints ws.FetchDataSetByTimeStampPerMember completed in {0} seconds.", ts.Seconds.ToString()));

        //                        timeSyncPointStarted = DateTime.Now;
        //                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
        //                        ts = DateTime.Now - timeSyncPointStarted;
        //                        Logger.writeLog(string.Format("MembershipPoints DecompressDataSetFromByteArray completed in {0} seconds.", ts.Seconds.ToString()));

        //                        timeSyncPointStarted = DateTime.Now;
        //                        if (SyncRealTimeController.DownloadData(dsID, "MembershipPoints", "PointID", true))
        //                        {
        //                            ts = DateTime.Now - timeSyncPointStarted;
        //                            Logger.writeLog(string.Format("MembershipPoints Insert Data completed in {0} seconds.", ts.Seconds.ToString()));
        //                            dataGetID += dsID.Tables[0].Rows.Count;
        //                            if (dataGetID > totalRecordID)
        //                                dataGetID = totalRecordID;
        //                            if (OnProgressUpdates != null)
        //                                OnProgressUpdates(this, string.Format("Download MembershipPoints data. {0} of {1} ", dataGetID, totalRecordID));
        //                        }
        //                        else
        //                        {
        //                            if (OnProgressUpdates != null)
        //                                OnProgressUpdates(this, string.Format(">> Failed download MembershipPoints"));
        //                            Thread.Sleep(retrySecDisconnected * 1000);
        //                            return false;
        //                        }
        //                    }
                            
        //                    if (OnProgressUpdates != null)

        //                        OnProgressUpdates(this, "All MembershipPoints have been synchronized successfully.");
                        
        //                }
        //                //return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.writeLog(ex);
        //                string msg = "Error downloading MembershipPoints. Server is disconnected, will keep trying every {0} seconds until server is connected";
        //                msg = string.Format(msg, retrySecDisconnected);
        //                if (OnProgressUpdates != null)
        //                    OnProgressUpdates(this, msg);
        //                Thread.Sleep(retrySecDisconnected * 1000);
        //                return false;
        //            }
        //            #endregion

        //        #region *) Download Points Allocation Log

        //            if (totalRecordID > 0)
        //            {
        //                try
        //                {
        //                    DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
        //                    if (!GetLatestModifiedOnLocalPerMember("PointAllocationLog", membershipNo, out lastModifiedOnCategory))
        //                    {
        //                        if (OnProgressUpdates != null)
        //                            OnProgressUpdates(this, "Error Getting Last Modified Date of the PointAllocationLog on Local");
        //                        Thread.Sleep(retrySecDisconnected * 1000);
        //                        return false;
        //                    }
        //                    Logger.writeLog("Last Modified On Local PAL " + membershipNo + " : " + lastModifiedOnCategory.ToString("yyyy-MM-dd HH:mm:ss.fff"));

        //                    timeSyncPointStarted = DateTime.Now;
        //                    var totalRecordCtg = ws.FetchRecordNoByTimestampByMember("PointAllocationLog", lastModifiedOnCategory, membershipNo);
        //                    ts = DateTime.Now - timeSyncPointStarted;
        //                    Logger.writeLog(string.Format("PointAllocationLog ws.FetchRecordNoByTimestampByMember completed in {0} seconds.", ts.Seconds.ToString()));

        //                    Logger.writeLog("Total Record PAL " + membershipNo + " : " + totalRecordCtg.ToString());
        //                    if (totalRecordCtg > 0)
        //                    {
        //                        if (OnProgressUpdates != null)
        //                            OnProgressUpdates(this, "Start Sync PointAllocationLog");
        //                        int dataGetCtg = 0;

        //                        int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

        //                        while (dataGetCtg < totalRecordCtg)
        //                        {
        //                            if (!GetLatestModifiedOnLocalPerMember("PointAllocationLog", membershipNo, out lastModifiedOnCategory))
        //                            {
        //                                if (OnProgressUpdates != null)
        //                                    OnProgressUpdates(this, "Error Getting PointAllocationLog Last Modified Date on Local");
        //                                Thread.Sleep(retrySecDisconnected * 1000);
        //                                return false;
        //                            }

        //                            timeSyncPointStarted = DateTime.Now;
        //                            var dataCtg = ws.FetchDataSetByTimeStampPerMember("PointAllocationLog", "PointAllocID", lastModifiedOnCategory, recordsperTime, membershipNo);
        //                            ts = DateTime.Now - timeSyncPointStarted;
        //                            Logger.writeLog(string.Format("PointAllocationLog ws.FetchDataSetByTimeStampPerMember completed in {0} seconds.", ts.Seconds.ToString()));

        //                            timeSyncPointStarted = DateTime.Now;
        //                            var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
        //                            ts = DateTime.Now - timeSyncPointStarted;
        //                            Logger.writeLog(string.Format("PointAllocationLog DecompressDataSetFromByteArray completed in {0} seconds.", ts.Seconds.ToString()));

        //                            timeSyncPointStarted = DateTime.Now;
        //                            if (SyncRealTimeController.DownloadData(dsCtg, "PointAllocationLog", "PointAllocID", true))
        //                            {
        //                                ts = DateTime.Now - timeSyncPointStarted;
        //                                Logger.writeLog(string.Format("PointAllocationLog Insert Data completed in {0} seconds.", ts.Seconds.ToString()));
        //                                dataGetCtg += dsCtg.Tables[0].Rows.Count;
        //                                if (dataGetCtg > totalRecordCtg)
        //                                    dataGetCtg = totalRecordCtg;
        //                                if (OnProgressUpdates != null)
        //                                    OnProgressUpdates(this, string.Format("Download PointAllocationLog data. {0} of {1} ", dataGetCtg, totalRecordCtg));
        //                            }
        //                            else
        //                            {
        //                                if (OnProgressUpdates != null)
        //                                    OnProgressUpdates(this, string.Format(">> Failed download PointAllocationLog"));
        //                                Thread.Sleep(retrySecDisconnected * 1000);
        //                            }
        //                        }
        //                        if (OnProgressUpdates != null)
        //                            OnProgressUpdates(this, "All PointAllocationLog have been synchronized successfully.");
        //                    }
        //                    //return true;
        //                }
        //                catch (Exception ex)
        //                {
        //                    //Logger.writeLog(ex);
        //                    string msg = "Error downloading PointAllocationLog. Server is disconnected, will keep trying every {0} seconds until server is connected";
        //                    msg = string.Format(msg, retrySecDisconnected);
        //                    if (OnProgressUpdates != null)
        //                        OnProgressUpdates(this, msg);
        //                    Thread.Sleep(retrySecDisconnected * 1000);
        //                    return false;
        //                }
        //            }
        //        #endregion

        //        //AppSetting.SetSetting("Points_LastSyncTime",);
        //        #region Update Last Sync Date
        //        //Membership m = new Membership(membershipNo);
        //        if (m != null && m.IsLoaded)
        //        {
        //            m.LastSyncPointsDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        //            string sqlString = "update Membership set userfld8 = '" + m.LastSyncPointsDate + "' where membershipno = '" + membershipNo + "'";
        //            DataService.ExecuteQuery(new QueryCommand(sqlString));
        //        }
        //        #endregion
                    
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (OnProgressUpdates != null)
        //            OnProgressUpdates(this, "Sync Points stopped : " + ex.Message);
        //        //Logger.writeLog("Synchronization failed.");
        //        Logger.writeLog(ex);
        //        return false;
        //    }
        //}
        #endregion

        public bool SyncPoints(string membershipNo)
        {
            try
            {
                #region *) Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Product Start");

                //Basic Info from server is Item and User
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

                #region *) Download MembershipPoints & PointAllocationLog
                var totalRecordID = 0;
                try
                {
                    DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                    if (!GetLatestModifiedOnLocalPerMember("MembershipPoints", membershipNo, out lastModifiedOnID))
                    {
                        Thread.Sleep(retrySecDisconnected * 1000);
                        return false;
                    }
                    Logger.writeLog("Last Modified On Local MP " + membershipNo + " : " + lastModifiedOnID.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    totalRecordID = ws.FetchRecordNoByTimestampByMember("MembershipPoints", lastModifiedOnID, membershipNo);
                    Logger.writeLog("Total Record MembershipPoints " + membershipNo + " : " + totalRecordID.ToString());
                    if (totalRecordID > 0)
                    {
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "Start Sync MembershipPoints");

                        int dataGetID = 0;
                        int recordPerTimeID = totalRecordID < recordsperTime ? totalRecordID : recordsperTime;
                        while (dataGetID < totalRecordID)
                        {
                            if (!GetLatestModifiedOnLocalPerMember("MembershipPoints", membershipNo, out lastModifiedOnID))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting MembershipPoints Last Modified Date on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                return false;
                            }

                            #region *) OBSOLETE
                            //System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                            //to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                            //using (System.Transactions.TransactionScope transScope =
                            //new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                            //{
                            //    var dataID = ws.GetPointsRealTimeDataAll(lastModifiedOnID, recordPerTimeID, membershipNo);
                            //    var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                            //    if (SyncRealTimeController.DownloadData(dsID, "MembershipPoints", "PointID", true))
                            //    {
                            //        DataTable dt = dsID.Tables[1].Copy();
                            //        DataSet dsPAL = new DataSet();
                            //        dsPAL.Tables.Add(dt);
                            //        if (!SyncRealTimeController.DownloadData(dsPAL, "PointAllocationLog", "PointAllocID", true))
                            //        {
                            //            if (OnProgressUpdates != null)
                            //                OnProgressUpdates(this, string.Format(">> Failed download PointAllocationLog"));
                            //            Thread.Sleep(retrySecDisconnected * 1000);
                            //            return false;
                            //        }

                            //        #region *) Delete PointTempLog
                            //        if (dt != null)
                            //        {
                            //            foreach (DataRow dr in dt.Rows)
                            //            {
                            //                string orderHdrID = dr["OrderHdrID"].ToString();
                            //                PointTempLog.Destroy("OrderHdrID", orderHdrID);
                            //            }
                            //        }
                            //        #endregion

                            //        dataGetID += dsID.Tables[0].Rows.Count;
                            //        if (dataGetID > totalRecordID)
                            //            dataGetID = totalRecordID;
                            //        if (OnProgressUpdates != null)
                            //            OnProgressUpdates(this, string.Format("Download MembershipPoints data. {0} of {1} ", dataGetID, totalRecordID));
                            //    }
                            //    else
                            //    {
                            //        if (OnProgressUpdates != null)
                            //            OnProgressUpdates(this, string.Format(">> Failed download MembershipPoints"));
                            //        Thread.Sleep(retrySecDisconnected * 1000);
                            //        return false;
                            //    }
                            //    transScope.Complete();
                            //}
                            #endregion

                            var dataID = ws.GetPointsRealTimeDataAll(lastModifiedOnID, recordPerTimeID, membershipNo);
                            var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                            QueryCommandCollection cmdColl = new QueryCommandCollection();
                            QueryCommandCollection tmpColl = new QueryCommandCollection();
                            if (SyncRealTimeController.DownloadData(dsID, "MembershipPoints", "PointID", true, true, out tmpColl))
                            {
                                cmdColl.AddRange(tmpColl);
                                DataTable dt = dsID.Tables[1].Copy();
                                DataSet dsPAL = new DataSet();
                                dsPAL.Tables.Add(dt);
                                if (!SyncRealTimeController.DownloadData(dsPAL, "PointAllocationLog", "PointAllocID", true, true, out tmpColl))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download PointAllocationLog"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    return false;
                                }
                                cmdColl.AddRange(tmpColl);

                                #region *) Delete PointTempLog
                                if (dt != null)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        string orderHdrID = dr["OrderHdrID"].ToString();
                                        //PointTempLog.Destroy("OrderHdrID", orderHdrID);
                                        QueryCommand cmd = new QueryCommand(string.Format("DELETE FROM PointTempLog WHERE OrderHdrID = '{0}'", orderHdrID), "PowerPOS");
                                        cmdColl.Add(cmd);
                                    }
                                }
                                #endregion

                                DataService.ExecuteTransaction(cmdColl);

                                dataGetID += dsID.Tables[0].Rows.Count;
                                if (dataGetID > totalRecordID)
                                    dataGetID = totalRecordID;
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, string.Format("Download MembershipPoints data. {0} of {1} ", dataGetID, totalRecordID));
                            }
                            else
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, string.Format(">> Failed download MembershipPoints"));
                                Thread.Sleep(retrySecDisconnected * 1000);
                                return false;
                            }
                        }
                        //AppSetting.SetSetting("Points_LastSyncTime",);
                        #region Update Last Sync Date
                        Membership m = new Membership(membershipNo);
                        if (m != null && m.IsLoaded)
                        {
                            m.LastSyncPointsDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            string sqlString = "update Membership set userfld8 = '" + m.LastSyncPointsDate + "' where membershipno = '" + membershipNo + "'";
                            DataService.ExecuteQuery(new QueryCommand(sqlString));
                        }
                        #endregion
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, "All MembershipPoints have been synchronized successfully.");

                    }
                    //return true;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    string msg = "Error downloading MembershipPoints. Server is disconnected, will keep trying every {0} seconds until server is connected";
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
                    OnProgressUpdates(this, "Sync Points stopped : " + ex.Message);
                //Logger.writeLog("Synchronization failed.");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool GetLatestModifiedOnLocalPerMember(string tableName, string membershipNo, out DateTime result)
        {
            result = DateTime.Now;
            try
            {
                string sqlString = "SELECT ISNULL(MAX(ModifiedOn),DATEADD(YEAR,-10,GETDATE())) TheDate FROM " + tableName + " Where MembershipNo = '" + membershipNo + "'";
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
