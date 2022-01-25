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

namespace PowerPOS.SyncMasterDataRealTimeController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncMasterDataRealTimeController
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

        public bool SyncMasterData()
        {
            try
            {
                #region Load Config
                Load_WS_URL();
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Master Data Start");

                //Basic Info from server is Item and User
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                DataSet ds;

                int retrySecConnected = 600;
                int retrySecDisconnected = 5;
                int recordsPerTime = 100;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncMasterDataRecordsPerTime), out recordsPerTime))
                    recordsPerTime = 100;

                #endregion

                while (true)
                {
                    #region *) Download Department

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Department", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Department on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("Department", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Department");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Department", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Department on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("Department", "DepartmentID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "Department", "DepartmentID", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Department data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Department"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Department have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Department. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Inventory Location

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("InventoryLocation", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Inventory Location on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("InventoryLocation", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Inventory Location");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("InventoryLocation", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Inventory Location on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("InventoryLocation", "InventoryLocationID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "InventoryLocation", "InventoryLocationID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Inventory Location data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Inventory Location"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Inventory Location have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Inventory Location. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Outlet Group

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("OutletGroup", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Outlet Group on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("OutletGroup", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Outlet Group");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("OutletGroup", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Outlet Group on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("OutletGroup", "OutletGroupID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "OutletGroup", "OutletGroupID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Outlet Group data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Outlet Group"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Outlet Group have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Outlet Group. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Outlet

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Outlet", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Outlet on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("Outlet", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Outlet");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Outlet", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Outlet on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("Outlet", "OutletName", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "Outlet", "OutletName", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Outlet data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Outlet"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Outlet have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Outlet. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download PointOfSale

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("PointOfSale", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Point Of Sale on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("PointOfSale", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync PointOfSale");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("PointOfSale", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Point Of Sale on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("PointOfSale", "PointOfSaleID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "PointOfSale", "PointOfSaleID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Point Of Sale data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Point of sale"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Point Of Sale have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading Point of Sale. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Supplier
                    /*
                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Supplier", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Supplier on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("Supplier", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Supplier");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Supplier", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Supplier on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("Supplier", "SupplierID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "Supplier", "SupplierID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Supplier data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Supplier"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Supplier have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Supplier. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                     **/
                    #endregion

                    #region *) Download GST

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("GST", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the GST on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("GST", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync GST");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("GST", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the GST on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("GST", "GSTID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "GST", "GSTID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download GST data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download GST"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All GST have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading GST. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download InventoryStockOutReason

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("InventoryStockOutReason", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the InventoryStockOutReason on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("InventoryStockOutReason", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync InventoryStockOutReason");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("InventoryStockOutReason", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the GST on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("InventoryStockOutReason", "ReasonID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "InventoryStockOutReason", "ReasonID", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download InventoryStockOutReason data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download InventoryStockOutReason"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All InventoryStockOutReason have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading InventoryStockOutReason. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download CashRecordingType

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("CashRecordingType", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the CashRecordingType on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("CashRecordingType", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync CashRecordingType");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("CashRecordingType", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the CashRecordingType on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("CashRecordingType", "CashRecordingTypeId", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "CashRecordingType", "CashRecordingTypeId", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download CashRecordingType data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download CashRecordingType"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All CashRecordingType have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.writeLog(ex);
                        string msg = "Error downloading CashRecordingType. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Currencies

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Currencies", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the GST on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("Currencies", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Currencies");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Currencies", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the Currencies on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("Currencies", "CurrencyId", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "Currencies", "CurrencyId", true))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Currencies data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Currencies"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Currencies have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Currencies. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download AccessLog
                    /*
                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("AccessLog", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the AccessLog on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("AccessLog", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync AccessLog");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsPerTime ? totalRecordID : recordsPerTime;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("AccessLog", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date of the AccessLog on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }
                                var dataID = ws.FetchDataSetByTimestamp("AccessLog", "AccessLogID", lastModifiedOnID, recordsPerTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);

                                if (SyncRealTimeController.DownloadData(dsID, "AccessLog", "AccessLogID", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download AccessLog data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download AccessLog"));
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All AccessLog have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading AccessLog. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    */
                    #endregion

                    #region *) Download AppSetting
                    //obsolete code, this setting is not used anymore
                    //string serverVal = ws.GetAppSettingValue(AppSetting.SettingsName.Sync.POSAppSettingSyncList);
                    //if (!string.IsNullOrEmpty(serverVal) && AppSetting.GetSetting(AppSetting.SettingsName.Sync.POSAppSettingSyncList) != serverVal)
                    //{
                    //    AppSetting.SetSetting(AppSetting.SettingsName.Sync.POSAppSettingSyncList, serverVal);
                    //    if (OnProgressUpdates != null)
                    //        OnProgressUpdates(this, string.Format("Download AppSetting {0} : {1} "
                    //            , AppSetting.SettingsName.Sync.POSAppSettingSyncList
                    //            , serverVal));
                    //}
                    //string settingList = AppSetting.GetSetting(AppSetting.SettingsName.Sync.POSAppSettingSyncList);
                    //if (!string.IsNullOrEmpty(settingList))
                    //{
                    //    string[] appSettingList = (AppSetting.GetSetting(AppSetting.SettingsName.Sync.POSAppSettingSyncList) + "").Split(',');
                    //    foreach (var appSettingKey in appSettingList)
                    //    {
                    //        try
                    //        {
                    //            string theKey = appSettingKey.Trim(Environment.NewLine.ToCharArray());
                    //            if (!string.IsNullOrEmpty(theKey))
                    //            {
                    //                serverVal = ws.GetAppSettingValue(theKey);
                    //                if (!string.IsNullOrEmpty(serverVal) && AppSetting.GetSetting(theKey) != serverVal)
                    //                {
                    //                    AppSetting.SetSetting(theKey, serverVal);
                    //                    if (OnProgressUpdates != null)
                    //                        OnProgressUpdates(this, string.Format("Download AppSetting {0} : {1} "
                    //                            , theKey
                    //                            , serverVal));
                    //                }
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Logger.writeLog(ex);
                    //            string msg = "Error downloading AppSetting. Server is disconnected, will keep trying every {0} seconds until server is connected";
                    //            msg = string.Format(msg, retrySecDisconnected);
                    //            if (OnProgressUpdates != null)
                    //                OnProgressUpdates(this, msg);
                    //            Thread.Sleep(retrySecDisconnected * 1000);
                    //            continue;
                    //        }
                    //    }
                    //}

                    #endregion

                    Thread.Sleep(retrySecConnected * 1000);

                    if (OnDataDownloaded != null)
                        OnDataDownloaded(this, "Data Downloaded");
                }
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync Master Data stopped");
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
