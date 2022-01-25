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

namespace PowerPOS.SyncProductController
{
    public delegate void UpdateProgress(object sender, string message);
    public delegate void DataDownloadedHandler(object sender, string message);

    public class SyncProductRealTimeController
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

        public bool SyncProducts()
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
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.ProductRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.ProductRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncProductRecordsPerTime), out recordsperTime))
                    recordsperTime = 100;

                int retryCount = 0;  
                int retryMax = (AppSetting.GetSetting(AppSetting.SettingsName.Sync.MaxRetryWhenError) + "").GetIntValue();
                if (retryMax == 0)
                    retryMax = 3;

                #endregion

                while (true)
                {
                    bool isItemUpdated = false, isPromoUpdated = false, isItemGroupMapUpdated = false;

                    #region *) Download ItemDepartment

                    try
                    {
                        DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemDepartment", out lastModifiedOnID))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Item Department on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordID = ws.FetchRecordNoByTimestamp("ItemDepartment", lastModifiedOnID);
                        if (totalRecordID > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Item Department");

                            int dataGetID = 0;
                            int recordPerTimeID = totalRecordID < recordsperTime ? totalRecordID : recordsperTime;

                            retryCount = 0;
                            while (dataGetID < totalRecordID)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemDepartment", out lastModifiedOnID))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Item Department Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataID = ws.FetchDataSetByTimestamp("ItemDepartment", "ItemDepartmentID", lastModifiedOnID, recordsperTime, false, 0, false, 0);
                                var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                                if (SyncRealTimeController.DownloadData(dsID, "ItemDepartment", "ItemDepartmentID", false))
                                {
                                    dataGetID += dsID.Tables[0].Rows.Count;
                                    if (dataGetID > totalRecordID)
                                        dataGetID = totalRecordID;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Item Department data. {0} of {1} ", dataGetID, totalRecordID));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Item Department"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Item Department have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading item department. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Category

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Category", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Category on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("Category", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Category");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Category", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Category Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("Category", "CategoryName", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "Category", "CategoryName", false))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download category data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Category"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Category have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading category. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Item

                    try
                    {
                        DateTime lastModifiedOnItem = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Item", out lastModifiedOnItem))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Item Last Modified Date on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        int totalRecords = ws.GetItemRecordCountAfterTimestamp(lastModifiedOnItem, PointOfSaleInfo.OutletName);

                        if (totalRecords > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Products");

                            int dataGet = 0;
                            retryCount = 0;
                            while (dataGet < totalRecords)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Item", out lastModifiedOnItem))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                byte[] data = ws.GetItemTableCompressedRealTime(lastModifiedOnItem, PointOfSaleInfo.OutletName, recordsperTime);
                                ds = SyncClientController.DecompressDataSetFromByteArray(data);

                                if (SyncRealTimeController.DownloadData(ds, "Item", "ItemNo", false))
                                {
                                    dataGet += ds.Tables[0].Rows.Count;
                                    if (dataGet > totalRecords)
                                        dataGet = totalRecords;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Product data. {0} of {1} ", dataGet, totalRecords));
                                    try
                                    {
                                        if (OnDataDownloaded != null)
                                            OnDataDownloaded(this, "Item Updated");
                                    }
                                    catch (Exception ex)
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error occured : " + ex.Message);
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        Logger.writeLog(ex);
                                    }
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Product"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All Product have been synchronized successfully.");

                            isItemUpdated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Product. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }

                    #endregion

                    #region *) Download AlternateBarcode

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("AlternateBarcode", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the AlternateBarcode on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("AlternateBarcode", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync AlternateBarcode");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("AlternateBarcode", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting AlternateBarcode Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("AlternateBarcode", "BarcodeID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "AlternateBarcode", "BarcodeID", true))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download AlternateBarcode data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download AlternateBarcode"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All AlternateBarcode have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading AlternateBarcode. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Supplier 
                    
                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("Supplier", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the Supplier on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("Supplier", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync Supplier");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("Supplier", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting Supplier Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("Supplier", "SupplierID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "Supplier", "SupplierID", true))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download Supplier data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download Supplier"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
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
                    #endregion

                    #region *) Download Item Supplier Map
                    
                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemSupplierMap", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemSupplierMap on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("ItemSupplierMap", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync ItemSupplierMap");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemSupplierMap", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting ItemSupplierMap Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("ItemSupplierMap", "ItemSupplierMapID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "ItemSupplierMap", "ItemSupplierMapID", true))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download ItemSupplierMap data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download ItemSupplierMap"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All ItemSupplierMap have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading ItemSupplierMap. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download ItemSummary

                    try
                    {
                        bool downloadAllLoc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DownloadItemSummaryAllLocation), false);
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);

                        if (downloadAllLoc)
                        {
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemSummary", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemSummary on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                        }
                        else
                        {
                            if (!SyncRealTimeController.GetLatestModifiedOnLocalByInvLoc("ItemSummary", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemSummary on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }
                        }

                        int totalRecordCtg = 0;
                        if (downloadAllLoc)
                            totalRecordCtg = ws.FetchRecordNoByTimestamp("ItemSummary", lastModifiedOnCategory);
                        else
                            totalRecordCtg = ws.FetchRecordNoByTimestampByInvLocID("ItemSummary", lastModifiedOnCategory, PointOfSaleInfo.InventoryLocationID);

                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync ItemSummary");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (downloadAllLoc)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemSummary", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting ItemSummary Last Modified Date on Local");
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }
                                }
                                else
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocalByInvLoc("ItemSummary", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting ItemSummary Last Modified Date on Local");
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }
                                }

                                byte[] dataCtg;
                                if (downloadAllLoc)
                                    dataCtg = ws.FetchDataSetByTimestamp("ItemSummary", "ItemSummaryID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                else
                                    dataCtg = ws.FetchDataSetByTimestamp("ItemSummary", "ItemSummaryID", lastModifiedOnCategory, recordsperTime, true, PointOfSaleInfo.InventoryLocationID, false, 0);

                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "ItemSummary", "ItemSummaryID", false))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download ItemSummary data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download ItemSummary"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All ItemSummary have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading Item Summary. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download ItemGroup

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemGroup", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemGroup on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("ItemGroup", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync ItemGroup");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemGroup", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting ItemGroup Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                                using (System.Transactions.TransactionScope transScope =
                                new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                                {

                                    var dataCtg = ws.GetItemGroupRealTimeData(lastModifiedOnCategory, recordsperTime);
                                    var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                    if (SyncRealTimeController.DownloadData(dsCtg, "ItemGroup", "ItemGroupID", true))
                                    {
                                        DataTable dt = dsCtg.Tables[1].Copy();
                                        DataSet dsItemGroupMap = new DataSet();
                                        dsItemGroupMap.Tables.Add(dt);
                                        SyncRealTimeController.DownloadData(dsItemGroupMap, "ItemGroupMap", "ItemGroupMapID", true);
                                        dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                        if (dataGetCtg > totalRecordCtg)
                                            dataGetCtg = totalRecordCtg;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download ItemGroup data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download ItemGroup"));
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }

                                    transScope.Complete();
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All ItemGroup have been synchronized successfully.");

                            isItemGroupMapUpdated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading ItemGroup. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download ItemGroupMap -- Removed
                    /*
                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemGroupMap", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemGroupMap on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("ItemGroupMap", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync ItemGroupMap");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemGroupMap", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting ItemGroupMap Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    continue;
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("ItemGroupMap", "ItemGroupMapID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "ItemGroupMap", "ItemGroupMapID", true))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download ItemGroupMap data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download ItemGroupMap"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All ItemGroupMap have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading ItemGroupMap. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    */
                    #endregion

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
                    {
                        #region *) Download PromoCampaignHdr
                        try
                        {
                            DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                            //DateTime lastModifiedOnPromoOutlet = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("PromoCampaignHdr", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the PromoCampaignHdr on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordCtg = ws.GetPromoRecordCountAfterTimestamp(lastModifiedOnCategory);
                            //var totalRecordPromoOutlet = ws.FetchPromoRecordNoByTimestampByOutletName(lastModifiedOnPromoOutlet,PointOfSaleInfo.OutletName);
                            if (totalRecordCtg > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync PromoCampaignHdr");
                                int dataGetCtg = 0;

                                int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                                retryCount = 0;
                                while (dataGetCtg < totalRecordCtg)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("PromoCampaignHdr", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting PromoCampaignHdr Last Modified Date on Local");
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }

                                    System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                                    to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                                    using (System.Transactions.TransactionScope transScope =
                                    new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                                    {
                                        var dataCtg = ws.GetPromoRealTimeDataAll(lastModifiedOnCategory, recordsperTime);
                                        var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                        if (SyncRealTimeController.DownloadData(dsCtg, "PromoCampaignHdr", "PromoCampaignHdrID", true))
                                        {
                                            DataTable dt = dsCtg.Tables[1].Copy();
                                            DataSet dsPromoCampaignDet = new DataSet();
                                            dsPromoCampaignDet.Tables.Add(dt);
                                            SyncRealTimeController.DownloadData(dsPromoCampaignDet, "PromoCampaignDet", "PromoCampaignDetID", true);

                                            DataTable dtPromoOutlet = dsCtg.Tables[2].Copy();
                                            DataSet dsPromoOutlet = new DataSet();
                                            dsPromoOutlet.Tables.Add(dtPromoOutlet);
                                            SyncRealTimeController.DownloadData(dsPromoOutlet, "PromoOutlet", "PromoOutletID", true);

                                            /*DataTable dtPromoLocationMap = dsCtg.Tables[4].Copy();
                                            DataSet dsPromoLocationMap = new DataSet();
                                            dsPromoLocationMap.Tables.Add(dtPromoLocationMap);
                                            SyncRealTimeController.DownloadData(dsPromoLocationMap, "PromoLocationMap", "PromoLocationMapID", true);*/

                                            DataTable dtPromoDaysMap = dsCtg.Tables[3].Copy();
                                            DataSet dsPromoDaysMap = new DataSet();
                                            dsPromoDaysMap.Tables.Add(dtPromoDaysMap);
                                            SyncRealTimeController.DownloadData(dsPromoDaysMap, "PromoDaysMap", "PromoDaysID", true);

                                            dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                            if (dataGetCtg > totalRecordCtg)
                                                dataGetCtg = totalRecordCtg;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download PromoCampaignHdr data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download PromoCampaignHdr"));
                                            Thread.Sleep(retrySecDisconnected * 1000);
                                            if (++retryCount < retryMax)
                                                continue;
                                            else
                                                throw new Exception("Failed download");
                                        }
                                        transScope.Complete();
                                    }
                                }
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All PromoCampaignHdr have been synchronized successfully.");

                                isPromoUpdated = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading PromoCampaignHdr. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion
                    }
                    else
                    {
                        #region *) Download PromoCampaignHdr
                        try
                        {
                            DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                            //DateTime lastModifiedOnPromoOutlet = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("PromoCampaignHdr", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the PromoCampaignHdr on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordCtg = ws.FetchPromoRecordNoByTimestampByOutletName(lastModifiedOnCategory, PointOfSaleInfo.OutletName);
                            //var totalRecordPromoOutlet = ws.FetchPromoRecordNoByTimestampByOutletName(lastModifiedOnPromoOutlet,PointOfSaleInfo.OutletName);
                            if (totalRecordCtg > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync PromoCampaignHdr");
                                int dataGetCtg = 0;

                                int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                                retryCount = 0;
                                while (dataGetCtg < totalRecordCtg)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("PromoCampaignHdr", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting PromoCampaignHdr Last Modified Date on Local");
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }

                                    System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                                    to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                                    using (System.Transactions.TransactionScope transScope =
                                    new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                                    {
                                        var dataCtg = ws.GetPromoRealTimeData(lastModifiedOnCategory, recordsperTime, PointOfSaleInfo.PointOfSaleID);
                                        var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                        if (SyncRealTimeController.DownloadData(dsCtg, "PromoCampaignHdr", "PromoCampaignHdrID", true))
                                        {
                                            DataTable dt = dsCtg.Tables[1].Copy();
                                            DataSet dsPromoCampaignDet = new DataSet();
                                            dsPromoCampaignDet.Tables.Add(dt);
                                            SyncRealTimeController.DownloadData(dsPromoCampaignDet, "PromoCampaignDet", "PromoCampaignDetID", true);

                                            DataTable dtPromoOutlet = dsCtg.Tables[2].Copy();
                                            DataSet dsPromoOutlet = new DataSet();
                                            dsPromoOutlet.Tables.Add(dtPromoOutlet);
                                            SyncRealTimeController.DownloadData(dsPromoOutlet, "PromoOutlet", "PromoOutletID", true);

                                            /*DataTable dtPromoLocationMap = dsCtg.Tables[4].Copy();
                                            DataSet dsPromoLocationMap = new DataSet();
                                            dsPromoLocationMap.Tables.Add(dtPromoLocationMap);
                                            SyncRealTimeController.DownloadData(dsPromoLocationMap, "PromoLocationMap", "PromoLocationMapID", true);*/

                                            DataTable dtPromoDaysMap = dsCtg.Tables[3].Copy();
                                            DataSet dsPromoDaysMap = new DataSet();
                                            dsPromoDaysMap.Tables.Add(dtPromoDaysMap);
                                            SyncRealTimeController.DownloadData(dsPromoDaysMap, "PromoDaysMap", "PromoDaysID", true);

                                            dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                            if (dataGetCtg > totalRecordCtg)
                                                dataGetCtg = totalRecordCtg;
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format("Download PromoCampaignHdr data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                        }
                                        else
                                        {
                                            if (OnProgressUpdates != null)
                                                OnProgressUpdates(this, string.Format(">> Failed download PromoCampaignHdr"));
                                            Thread.Sleep(retrySecDisconnected * 1000);
                                            if (++retryCount < retryMax)
                                                continue;
                                            else
                                                throw new Exception("Failed download");
                                        }
                                        transScope.Complete();
                                    }
                                }
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All PromoCampaignHdr have been synchronized successfully.");

                                isPromoUpdated = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading PromoCampaignHdr. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion
                    }
                    
                    #region *) Download Special Discount

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("SpecialDiscounts", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the SpecialDiscounts on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("SpecialDiscounts", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync SpecialDiscounts");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("SpecialDiscounts", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting SpecialDiscounts Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestampSpecialDiscount(lastModifiedOnCategory, recordsperTime, PointOfSaleInfo.OutletName);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "SpecialDiscounts", "DiscountName", false))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download SpecialDiscounts data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download SpecialDiscounts"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All SpecialDiscounts have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading SpecialDiscounts. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    #region *) Download Special Discount Detail

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("SpecialDiscountDetail", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the SpecialDiscountDetail on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("SpecialDiscountDetail", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync SpecialDiscountDetail");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("SpecialDiscountDetail", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting SpecialDiscountDetail Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("SpecialDiscountDetail", "SpecialDiscountDetailID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "SpecialDiscountDetail", "SpecialDiscountDetailID", true))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download SpecialDiscountDetail data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download SpecialDiscountDetail"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All SpecialDiscountDetail have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading SpecialDiscountDetail. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion 

                    #region *) Download Item Quantity Trigger

                    try
                    {
                        DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                        if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemQuantityTrigger", out lastModifiedOnCategory))
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Error Getting Last Modified Date of the ItemQuantityTrigger on Local");
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }

                        var totalRecordCtg = ws.FetchRecordNoByTimestamp("ItemQuantityTrigger", lastModifiedOnCategory);
                        if (totalRecordCtg > 0)
                        {
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "Start Sync ItemQuantityTrigger");
                            int dataGetCtg = 0;

                            int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                            retryCount = 0;
                            while (dataGetCtg < totalRecordCtg)
                            {
                                if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemQuantityTrigger", out lastModifiedOnCategory))
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, "Error Getting ItemQuantityTrigger Last Modified Date on Local");
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }

                                var dataCtg = ws.FetchDataSetByTimestamp("ItemQuantityTrigger", "TriggerID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                if (SyncRealTimeController.DownloadData(dsCtg, "ItemQuantityTrigger", "TriggerID", true))
                                {
                                    dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                    if (dataGetCtg > totalRecordCtg)
                                        dataGetCtg = totalRecordCtg;
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format("Download ItemQuantityTrigger data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                }
                                else
                                {
                                    if (OnProgressUpdates != null)
                                        OnProgressUpdates(this, string.Format(">> Failed download ItemQuantityTrigger"));
                                    Thread.Sleep(retrySecDisconnected * 1000);
                                    if (++retryCount < retryMax)
                                        continue;
                                    else
                                        throw new Exception("Failed download");
                                }
                            }
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, "All ItemQuantityTrigger have been synchronized successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                        string msg = "Error downloading ItemQuantityTrigger. Server is disconnected, will keep trying every {0} seconds until server is connected";
                        msg = string.Format(msg, retrySecDisconnected);
                        if (OnProgressUpdates != null)
                            OnProgressUpdates(this, msg);
                        Thread.Sleep(retrySecDisconnected * 1000);
                        continue;
                    }
                    #endregion

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseCustomerPricing), false))
                    {
                        #region *) Download Customer Pricing

                        try
                        {
                            DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("CustomerPricing", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the CustomerPricing on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordCtg = ws.FetchRecordNoByTimestamp("CustomerPricing", lastModifiedOnCategory);
                            if (totalRecordCtg > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync CustomerPricing");
                                int dataGetCtg = 0;

                                int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                                retryCount = 0;
                                while (dataGetCtg < totalRecordCtg)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("CustomerPricing", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting CustomerPricing Last Modified Date on Local");
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }

                                    var dataCtg = ws.FetchDataSetByTimestamp("CustomerPricing", "CustomerPricingID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                    var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                    if (SyncRealTimeController.DownloadData(dsCtg, "CustomerPricing", "CustomerPricingID", true))
                                    {
                                        dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                        if (dataGetCtg > totalRecordCtg)
                                            dataGetCtg = totalRecordCtg;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download CustomerPricing data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download CustomerPricing"));
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }
                                }
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All CustomerPricing have been synchronized successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading CustomerPricing. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion

                        #region *) Download Outlet Item Customer Pricing

                        try
                        {
                            DateTime lastModifiedOnCategory = DateTime.Now.AddDays(-7);
                            if (!SyncRealTimeController.GetLatestModifiedOnLocal("OutletCustomerPricing", out lastModifiedOnCategory))
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Error Getting Last Modified Date of the OutletCustomerPricing on Local");
                                Thread.Sleep(retrySecDisconnected * 1000);
                                continue;
                            }

                            var totalRecordCtg = ws.FetchRecordNoByTimestamp("OutletCustomerPricing", lastModifiedOnCategory);
                            if (totalRecordCtg > 0)
                            {
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "Start Sync OutletCustomerPricing");
                                int dataGetCtg = 0;

                                int recordPerTimeCtg = totalRecordCtg < recordsperTime ? totalRecordCtg : recordsperTime;

                                retryCount = 0;
                                while (dataGetCtg < totalRecordCtg)
                                {
                                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("OutletCustomerPricing", out lastModifiedOnCategory))
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, "Error Getting OutletCustomerPricing Last Modified Date on Local");
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }

                                    var dataCtg = ws.FetchDataSetByTimestamp("OutletCustomerPricing", "OutletCustomerPricingID", lastModifiedOnCategory, recordsperTime, false, 0, false, 0);
                                    var dsCtg = SyncClientController.DecompressDataSetFromByteArray(dataCtg);
                                    if (SyncRealTimeController.DownloadData(dsCtg, "OutletCustomerPricing", "OutletCustomerPricingID", true))
                                    {
                                        dataGetCtg += dsCtg.Tables[0].Rows.Count;
                                        if (dataGetCtg > totalRecordCtg)
                                            dataGetCtg = totalRecordCtg;
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format("Download OutletCustomerPricing data. {0} of {1} ", dataGetCtg, totalRecordCtg));
                                    }
                                    else
                                    {
                                        if (OnProgressUpdates != null)
                                            OnProgressUpdates(this, string.Format(">> Failed download OutletCustomerPricing"));
                                        Thread.Sleep(retrySecDisconnected * 1000);
                                        if (++retryCount < retryMax)
                                            continue;
                                        else
                                            throw new Exception("Failed download");
                                    }
                                }
                                if (OnProgressUpdates != null)
                                    OnProgressUpdates(this, "All OutletCustomerPricing have been synchronized successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                            string msg = "Error downloading OutletCustomerPricing. Server is disconnected, will keep trying every {0} seconds until server is connected";
                            msg = string.Format(msg, retrySecDisconnected);
                            if (OnProgressUpdates != null)
                                OnProgressUpdates(this, msg);
                            Thread.Sleep(retrySecDisconnected * 1000);
                            continue;
                        }
                        #endregion
                    }

                    if (isItemUpdated || isItemGroupMapUpdated || isPromoUpdated) 
                        ApplyPromotionController.isPromoUpdated = true;

                    Thread.Sleep(retrySecConnected * 1000);
                }
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync products stopped");
                return true;
            }
            catch (Exception ex)
            {
                if (OnProgressUpdates != null)
                    OnProgressUpdates(this, "Sync products stopped : " + ex.Message);
                //Logger.writeLog("Synchronization failed.");
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool DownloadProducts()
        {
            bool isSuccess = false;

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
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.ProductRetrySecWhenConnected), out retrySecConnected))
                    retrySecConnected = 600;
                if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.ProductRetrySecWhenDisconnected), out retrySecDisconnected))
                    retrySecDisconnected = 10;

                #endregion

                #region *) Download ItemDepartment

                try
                {
                    DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemDepartment", out lastModifiedOnID))
                        return false;

                    var totalRecordID = ws.FetchRecordNoByTimestamp("ItemDepartment", lastModifiedOnID);
                    if (totalRecordID > 0)
                    {
                        var dataID = ws.FetchDataSetByTimestamp("ItemDepartment", "ItemDepartmentID", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                        if (!SyncRealTimeController.DownloadData(dsID, "ItemDepartment", "ItemDepartmentID", false))
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    return false;
                }
                #endregion

                #region *) Download Category

                try
                {
                    DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("Category", out lastModifiedOnID))
                        return false;

                    var totalRecordID = ws.FetchRecordNoByTimestamp("Category", lastModifiedOnID);
                    if (totalRecordID > 0)
                    {
                        var dataID = ws.FetchDataSetByTimestamp("Category", "CategoryName", lastModifiedOnID, totalRecordID, false, 0, false, 0);
                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                        if (!SyncRealTimeController.DownloadData(dsID, "Category", "CategoryName", false))
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    return false;
                }
                #endregion

                #region *) Download Item

                try
                {
                    DateTime lastModifiedOnItem = DateTime.Now.AddDays(-7);
                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("Item", out lastModifiedOnItem))
                        return false;
                    int totalRecords = ws.GetItemRecordCountAfterTimestamp(lastModifiedOnItem, PointOfSaleInfo.OutletName);
                    if (totalRecords > 0)
                    {

                        byte[] data = ws.GetItemTableCompressedRealTime(lastModifiedOnItem, PointOfSaleInfo.OutletName, totalRecords);
                        ds = SyncClientController.DecompressDataSetFromByteArray(data);
                        if (!SyncRealTimeController.DownloadData(ds, "Item", "ItemNo", false))
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    return false;
                }

                #endregion

                #region *) Download ItemSummary

                try
                {
                    DateTime lastModifiedOnID = DateTime.Now.AddDays(-7);
                    if (!SyncRealTimeController.GetLatestModifiedOnLocal("ItemSummary", out lastModifiedOnID))
                        return false;

                    var totalRecordID = ws.FetchRecordNoByTimestamp("ItemSummary", lastModifiedOnID);
                    if (totalRecordID > 0)
                    {
                        var dataID = ws.FetchDataSetByTimestamp("ItemSummary", "ItemSummaryID", lastModifiedOnID, totalRecordID, true, PointOfSaleInfo.InventoryLocationID, false, 0);
                        var dsID = SyncClientController.DecompressDataSetFromByteArray(dataID);
                        if (!SyncRealTimeController.DownloadData(dsID, "ItemSummary", "ItemSummaryID", false))
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }
    }
}
