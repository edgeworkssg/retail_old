using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using System.Data;

namespace PowerPOS
{
    public partial class InventorySync
    {
        public const string XMLFILENAME = "\\WS.XML";
        public static string WS_URL;
        private const int RETRY_LIMIT = 10;

        public static bool Load_WS_URL()
        {
            try
            {
                //if it does not exist in database, load from text file
                //this is for backward compatibility 
                if (AppSetting.GetSetting("WS_URL") != null)
                {
                    WS_URL = AppSetting.GetSetting("WS_URL").ToString();
                    return true;
                }
                else
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + XMLFILENAME);
                    WS_URL = ds.Tables[0].Rows[0]["URL"].ToString();
                    return true;
                }
            }
            catch (Exception ex) //system exception
            {
                Logger.writeLog("Load_WS_URL");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool GetCurrentInventory()
        {
            try
            {
                return SyncClientController.GetCurrentInventory();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool DeleteInventoryDetFromVoidedOrder()
        {
            try
            {
                // Init Web Service
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                return ws.DeleteInventoryDetFromVoidedOrder();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool StartInventorySync()
        {
            try
            {
                // Init POS Controller (to get POSId)
                PointOfSaleController.GetPointOfSaleInfo();

                // Init WS
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                // Check Change Tracking on Client DB
                long? clientCTVersion = SQLChangeTracking.GetChangeTrackingVersion();
                if (clientCTVersion == null)
                {
                    // Auto Enable Change Tracking on Client DB
                    SQLChangeTracking.EnableChangeTracking();
                    clientCTVersion = SQLChangeTracking.GetChangeTrackingVersion();
                }
                if (SQLChangeTracking.GetMinValidVersion("InventoryHdr") == null)
                {
                    // Auto Enable Change Tracking on Client DB Table
                    SQLChangeTracking.EnableChangeTrackingTable("InventoryHdr");
                }
                if (SQLChangeTracking.GetMinValidVersion("InventoryDet") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("InventoryDet");
                }
                if (SQLChangeTracking.GetMinValidVersion("StockTake") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("StockTake");
                }
                if (SQLChangeTracking.GetMinValidVersion("LocationTransfer") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("LocationTransfer");
                }

                // Check Last Sync on Server DB (NULL if not exist)
                string serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_InventoryLastSyncVersion";
                string lastSyncVersion = ws.GetAppSettingValue(serverAppSettingKey);
                long longLastSyncVersion = -1;
                long.TryParse(lastSyncVersion, out longLastSyncVersion);

                // Check Min Valid Version (OrderHdr)
                long? minValidOrderHdr = SQLChangeTracking.GetMinValidVersion("InventoryHdr");

                // Upload Data to Server
                if (lastSyncVersion == null)
                {
                    // Upload Everything

                    // Retrieve Client Order Tables (OrderHdr, OrderDet, ReceiptHdr, ReceiptDet, SalesCommissionRecord)
                    // Into Dataset and then Compress

                    DataSet dsInventoryHdr = SQLChangeTracking.GetFullTable("InventoryHdr", "InventoryHdrRefNo");
                    DataSet dsInventoryDet = SQLChangeTracking.GetFullTable("InventoryDet", "InventoryDetRefNo");
                    DataSet dsStockTake = SQLChangeTracking.GetFullTable("StockTake", "StockTakeID");
                    DataSet dsLocationTransfer = SQLChangeTracking.GetFullTable("LocationTransfer", "LocationTransferID");

                    DataTable dtInventoryHdr = dsInventoryHdr.Tables[0].Copy();
                    DataTable dtInventoryDet = dsInventoryDet.Tables[0].Copy();
                    DataTable dtStockTake = dsStockTake.Tables[0].Copy();
                    DataTable dtLocationTransfer = dsLocationTransfer.Tables[0].Copy();

                    DataSet inventoryDS = new DataSet();
                    inventoryDS.Tables.Add(dtInventoryHdr);
                    inventoryDS.Tables.Add(dtInventoryDet);
                    inventoryDS.Tables.Add(dtStockTake);
                    inventoryDS.Tables.Add(dtLocationTransfer);

                    byte[] inventoryDSData = SyncClientController.CompressDataSetToByteArray(inventoryDS);

                    string status = string.Empty;
                    if (ws.UploadInventoryTables(inventoryDSData, UserInfo.username, out status))
                    {
                        // Update App Setting on Server
                        ws.SetAppSettingValue(serverAppSettingKey, clientCTVersion.ToString());
                    }
                }
                else
                {
                    // Compare Server Last Sync Version with Client CT Version
                    if (clientCTVersion.Value == longLastSyncVersion)
                    {
                        // No Sync
                    }
                    else
                    {
                        if (minValidOrderHdr <= longLastSyncVersion)
                        {
                            // Upload Partial

                            // Retrieve Client Order Tables (OrderHdr, OrderDet, ReceiptHdr, ReceiptDet, SalesCommissionRecord)
                            // Into Dataset and then Compress

                            DataSet dsInventoryHdr = SQLChangeTracking.GetChanges("InventoryHdr", longLastSyncVersion, "InventoryHdrRefNo");
                            DataSet dsInventoryDet = SQLChangeTracking.GetChanges("InventoryDet", longLastSyncVersion, "InventoryDetRefNo");
                            DataSet dsStockTake = SQLChangeTracking.GetChanges("StockTake", longLastSyncVersion, "StockTakeID");
                            DataSet dsLocationTransfer = SQLChangeTracking.GetChanges("LocationTransfer", longLastSyncVersion, "LocationTransferID");

                            DataTable dtInventoryHdr = dsInventoryHdr.Tables[0].Copy();
                            DataTable dtInventoryDet = dsInventoryDet.Tables[0].Copy();
                            DataTable dtStockTake = dsStockTake.Tables[0].Copy();
                            DataTable dtLocationTransfer = dsLocationTransfer.Tables[0].Copy();

                            DataSet inventoryDs = new DataSet();
                            inventoryDs.Tables.Add(dtInventoryHdr);
                            inventoryDs.Tables.Add(dtInventoryDet);
                            inventoryDs.Tables.Add(dtStockTake);
                            inventoryDs.Tables.Add(dtLocationTransfer);

                            byte[] inventoryDSData = SyncClientController.CompressDataSetToByteArray(inventoryDs);

                            string status = string.Empty;
                            if (ws.UploadInventoryTables(inventoryDSData, UserInfo.username, out status))
                            {
                                // Update App Setting on Server
                                ws.SetAppSettingValue(serverAppSettingKey, clientCTVersion.ToString());
                            }
                        }
                        else
                        {
                            // Upload Everything

                            // Retrieve Client Order Tables (OrderHdr, OrderDet, ReceiptHdr, ReceiptDet, SalesCommissionRecord)
                            // Into Dataset and then Compress

                            DataSet dsInventoryHdr = SQLChangeTracking.GetFullTable("InventoryHdr", "InventoryHdrRefNo");
                            DataSet dsInventoryDet = SQLChangeTracking.GetFullTable("InventoryDet", "InventoryDetRefNo");
                            DataSet dsStockTake = SQLChangeTracking.GetFullTable("StockTake", "StockTakeID");
                            DataSet dsLocationTransfer = SQLChangeTracking.GetFullTable("LocationTransfer", "LocationTransferID");

                            DataTable dtInventoryHdr = dsInventoryHdr.Tables[0].Copy();
                            DataTable dtInventoryDet = dsInventoryDet.Tables[0].Copy();
                            DataTable dtStockTake = dsStockTake.Tables[0].Copy();
                            DataTable dtLocationTransfer = dsLocationTransfer.Tables[0].Copy();

                            DataSet inventoryDS = new DataSet();
                            inventoryDS.Tables.Add(dtInventoryHdr);
                            inventoryDS.Tables.Add(dtInventoryDet);
                            inventoryDS.Tables.Add(dtStockTake);
                            inventoryDS.Tables.Add(dtLocationTransfer);

                            byte[] inventoryDSData = SyncClientController.CompressDataSetToByteArray(inventoryDS);

                            string status = string.Empty;
                            if (ws.UploadInventoryTables(inventoryDSData, UserInfo.username, out status))
                            {
                                // Update App Setting on Server
                                ws.SetAppSettingValue(serverAppSettingKey, clientCTVersion.ToString());
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }
    }
}
