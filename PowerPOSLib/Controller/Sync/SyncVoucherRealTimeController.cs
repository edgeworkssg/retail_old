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

namespace PowerPOS.SyncVoucherRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncVoucherRealTimeController
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

        public bool SyncVoucher()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Voucher Start");
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
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.VoucherRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.VoucherRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncVoucherRecordsPerTime), out recordsPerTime))
                    recordsPerTime = 100;

                #endregion

                while (true)
                {
                    cmd = new QueryCommandCollection();

                    #region *) Download VoucherHeader

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("VoucherHeader", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the VoucherHeader on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("VoucherHeader", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync VoucherHeader");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("VoucherHeader", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the VoucherHeader on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestampVoucherHeader(lastModifiedOnID, recordsPerTime, PointOfSaleInfo.OutletName);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "VoucherHeader", "VoucherHeaderID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download VoucherHeader data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download VoucherHeader"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All VoucherHeader have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading VoucherHeader. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Vouchers

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Vouchers", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Vouchers on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("Vouchers", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Vouchers");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Vouchers", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Vouchers on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestampVouchers(lastModifiedOnID, recordsPerTime, PointOfSaleInfo.OutletName);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "Vouchers", "VoucherID", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Vouchers data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Vouchers"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Vouchers have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Voucher. Server is disconnected, will keep trying every {0} seconds until server is connected";
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
                    OnProgressUpdates(this, "Sync Voucher stopped");
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
