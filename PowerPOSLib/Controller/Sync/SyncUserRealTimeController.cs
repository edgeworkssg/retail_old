using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS;
using PowerPOSLib.PowerPOSSync;
using SubSonic;
using System.Linq;
using System.Globalization;
using PowerPOS.Container;
using System.Threading;
using PowerPOS.Controller;

namespace PowerPOS.SyncUserRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncUserRealTimeController
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

        public bool SyncUser()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync User Start");
                QueryCommandCollection cmd;
                QueryCommand mycmd;
                Query qry;
                Where whr;

                //Basic Info from server is Item and User
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

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
                    cmd = new QueryCommandCollection();

                    #region *) Download UserGroup

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserGroup", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the UserGroup on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("UserGroup", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync UserGroup");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserGroup", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the UserGroup on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("UserGroup", "GroupID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "UserGroup", "GroupID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download UserGroup data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download User Group"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All UserGroup have been synchronized successfully.");
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

                    #region *) Download SalesGroup

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("SalesGroup", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the SalesGroup on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("SalesGroup", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync SalesGroup");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("SalesGroup", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the SalesGroup on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("SalesGroup", "SalesGroupId", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "SalesGroup", "SalesGroupId", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download SalesGroup data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download SalesGroup"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All SalesGroup have been synchronized successfully.");
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

                    #region *) Download UserMst

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserMst", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the UserMst on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("UserMst", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync UserMst");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserMst", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the UserMst on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("UserMst", "UserName", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "UserMst", "UserName", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download UserMst data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download UserMst"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All UserMst have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading User. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download UserPrivilege

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserPrivilege", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the UserPrivilege on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("UserPrivilege", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync UserPrivilege");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("UserPrivilege", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the UserPrivilege on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var dataID = ws.FetchDataSetByTimestamp("UserPrivilege", "UserPrivilegeID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "UserPrivilege", "UserPrivilegeID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download UserPrivilege data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download UserPrivilege"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All UserPrivilege have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading user privileges. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download GroupUserPrivilege

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("GroupUserPrivilege", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the GroupUserPrivilege on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("GroupUserPrivilege", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync GroupUserPrivilege");

                            var updatedGroupID = ws.FetchUpdatedGroupUserPrivileges(lastModifiedOnID);

                            for (int i = 0; i < updatedGroupID.Length; i++)
                            {
                                #region *) Delete Existing Data

                                Query qrDelete = new Query("GroupUserPrivilege");
                                qrDelete.AddWhere(GroupUserPrivilege.Columns.GroupID, updatedGroupID[i]);
                                qrDelete.QueryType = QueryType.Delete;
                                qrDelete.Execute();

                                #endregion

                                #region *) Download Updated Data

                                var dataID = ws.FetchGroupPrivilegesRealTime(updatedGroupID[i]);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "GroupUserPrivilege", "GroupUserPrivilegeID", true))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Downloaded GroupUserPrivilege data. {0} of {1} ", (i + 1), updatedGroupID.Length));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download GroupUserPrivilege")); 
                                }
                                //GroupUserPrivilegeCollection idColl = new GroupUserPrivilegeCollection();
                                //idColl.Load(dsID.Tables[0]);
                                //if (idColl.Count > 0)
                                //{
                                //    SubSonic.TableSchema.TableColumnCollection ColumnList = new SubSonic.TableSchema.TableColumnCollection();
                                //    ColumnList.AddRange(DataService.GetSchema("GroupUserPrivilege", "PowerPOS").Columns);
                                //    cmd = new QueryCommandCollection();

                                //    for (int j = 0; j < idColl.Count; j++)
                                //    {
                                //        //Get count....
                                //        idColl[j].DirtyColumns.AddRange(ColumnList);
                                //        mycmd = idColl[j].GetInsertCommand("SYNC");
                                //        SyncRealTimeController.SetModifiedOn(mycmd, idColl[j].ModifiedOn.GetValueOrDefault(DateTime.Now));
                                //        cmd.Add(mycmd);
                                //    }
                                //    if (cmd.Count > 0)
                                //        DataService.ExecuteTransaction(cmd);
                                //    cmd = new QueryCommandCollection();
                                //    if (OnProgressUpdates != null)
                                //        OnProgressUpdates(this, string.Format("Downloaded GroupUserPrivilege data. {0} of {1} ", (i + 1), updatedGroupID.Length));
                                //}
                                #endregion
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All GroupUserPrivilege have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Group User Privilege. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    Thread.Sleep(retrySecConnected * 1000);

                    if (OnDataDownloaded != null)
                        OnDataDownloaded(this, "Data Downloaded");
                }
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync User stopped");
                return true;
            }
            catch (Exception ex)
            {
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Error : " + ex.Message);
                Logger.writeLog(ex);
                return false;
            }
        }
    }
}
