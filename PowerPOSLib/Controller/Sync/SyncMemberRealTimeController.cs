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

namespace PowerPOS.SyncMemberRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncMemberRealTimeController
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

        public bool SyncMembership()
        {
            try
            {
                #region *) Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Membership Start");
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

                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MemberRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MemberRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncMemberRecordsPerTime), out recordsPerTime))
                    recordsPerTime = 100;

                #endregion

                cmd = new QueryCommandCollection();

                while (true)
                {
                    #region *) Download Membership Group

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("MembershipGroup", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the MembershipGroup on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("MembershipGroup", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync MembershipGroup");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("MembershipGroup", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the MembershipGroup on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("MembershipGroup", "MembershipGroupID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "MembershipGroup", "MembershipGroupID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download MembershipGroup data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download MembershipGroup"));
                                    break;
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All MembershipGroup have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        Logger.writeLog(ex);
                        string msg = "Error downloading MembershipGroup. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Membership

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
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Membership", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Membership on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("Membership", "MembershipNo", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
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
                                    break;
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Membership have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Membership. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Installment

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Installment", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Installment on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("Installment", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Installment");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Installment", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Installment on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("Installment", "InstallmentRefNo", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "Installment", "InstallmentRefNo", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Installment data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Installment"));
                                    break;
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Installment have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Installment. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download InstallmentDetail

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("InstallmentDetail", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the InstallmentDetail on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("InstallmentDetail", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync InstallmentDetail");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("InstallmentDetail", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the InstallmentDetail on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("InstallmentDetail", "InstallmentDetRefNo", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "InstallmentDetail", "InstallmentDetRefNo", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download InstallmentDetail data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download InstallmentDetail"));
                                    break;
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All InstallmentDetail have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading InstallmentDetail. Server is disconnected, will keep trying every {0} seconds until server is connected";
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
                    OnProgressUpdates(this, "Sync member stopped");
                return true;
            }
            catch (Exception ex)
            {
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync member stopped : " + ex.Message);
                Logger.writeLog(ex);
                return false;
            }
        }
    }
}
