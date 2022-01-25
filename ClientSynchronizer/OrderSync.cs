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
    public partial class OrderSync
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

        public static bool StartOrderSync()
        {
            try
            {
                // Init POS Controller (to get POSId)
                PointOfSaleController.GetPointOfSaleInfo();

                // Get Last Closing Date
                DateTime dtLastClosing = ClosingController.FetchLastClosingTime(PointOfSaleInfo.PointOfSaleID);

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
                if (SQLChangeTracking.GetMinValidVersion("OrderHdr") == null)
                {
                    // Auto Enable Change Tracking on Client DB Table
                    SQLChangeTracking.EnableChangeTrackingTable("OrderHdr");
                }
                if (SQLChangeTracking.GetMinValidVersion("OrderDet") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("OrderDet");
                }
                if (SQLChangeTracking.GetMinValidVersion("ReceiptHdr") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("ReceiptHdr");
                }
                if (SQLChangeTracking.GetMinValidVersion("ReceiptDet") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("ReceiptDet");
                }
                if (SQLChangeTracking.GetMinValidVersion("SalesCommissionRecord") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("SalesCommissionRecord");
                }

                // Check Last Sync on Server DB (NULL if not exist)
                string serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_OrderLastSyncVersion";
                string lastSyncVersion = ws.GetAppSettingValue(serverAppSettingKey);
                long longLastSyncVersion = -1;
                long.TryParse(lastSyncVersion, out longLastSyncVersion);

                // Check Min Valid Version (OrderHdr)
                long? minValidOrderHdr = SQLChangeTracking.GetMinValidVersion("OrderHdr");

                // Upload Data to Server
                if (lastSyncVersion == null)
                {
                    // Upload Everything

                    // Retrieve Client Order Tables (OrderHdr, OrderDet, ReceiptHdr, ReceiptDet, SalesCommissionRecord)
                    // Into Dataset and then Compress

                    DataSet dsOrderHdr = SQLChangeTracking.GetOrderHdrFullTable(dtLastClosing);
                    DataSet dsOrderDet = SQLChangeTracking.GetOrderDetFullTable(dtLastClosing);
                    DataSet dsReceiptHdr = SQLChangeTracking.GetReceiptHdrFullTable(dtLastClosing);
                    DataSet dsReceiptDet = SQLChangeTracking.GetReceiptDetFullTable(dtLastClosing);
                    DataSet dsSalesCommissionRecord = SQLChangeTracking.GetSalesCommissionRecordFullTable(dtLastClosing);

                    DataTable dtOrderHdr = dsOrderHdr.Tables[0].Copy();
                    DataTable dtOrderDet = dsOrderDet.Tables[0].Copy();
                    DataTable dtReceiptHdr = dsReceiptHdr.Tables[0].Copy();
                    DataTable dtReceiptDet = dsReceiptDet.Tables[0].Copy();
                    DataTable dtSalesCommissionRecord = dsSalesCommissionRecord.Tables[0].Copy();

                    DataSet orderDS = new DataSet();
                    orderDS.Tables.Add(dtOrderHdr);
                    orderDS.Tables.Add(dtOrderDet);
                    orderDS.Tables.Add(dtReceiptHdr);
                    orderDS.Tables.Add(dtReceiptDet);
                    orderDS.Tables.Add(dtSalesCommissionRecord);

                    byte[] orderDSData = SyncClientController.CompressDataSetToByteArray(orderDS);

                    string status = string.Empty;
                    if (ws.UploadOrderTables(orderDSData, UserInfo.username, out status))
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

                            DataSet dsOrderHdr = SQLChangeTracking.GetChanges("OrderHdr", longLastSyncVersion, "OrderHdrID");
                            DataSet dsOrderDet = SQLChangeTracking.GetChanges("OrderDet", longLastSyncVersion, "OrderDetID");
                            DataSet dsReceiptHdr = SQLChangeTracking.GetChanges("ReceiptHdr", longLastSyncVersion, "ReceiptHdrID");
                            DataSet dsReceiptDet = SQLChangeTracking.GetChanges("ReceiptDet", longLastSyncVersion, "ReceiptDetID");
                            DataSet dsSalesCommissionRecord = SQLChangeTracking.GetChanges("SalesCommissionRecord", longLastSyncVersion,
                                "CommissionRecordID");

                            DataTable dtOrderHdr = dsOrderHdr.Tables[0].Copy();
                            DataTable dtOrderDet = dsOrderDet.Tables[0].Copy();
                            DataTable dtReceiptHdr = dsReceiptHdr.Tables[0].Copy();
                            DataTable dtReceiptDet = dsReceiptDet.Tables[0].Copy();
                            DataTable dtSalesCommissionRecord = dsSalesCommissionRecord.Tables[0].Copy();

                            DataSet orderDS = new DataSet();
                            orderDS.Tables.Add(dtOrderHdr);
                            orderDS.Tables.Add(dtOrderDet);
                            orderDS.Tables.Add(dtReceiptHdr);
                            orderDS.Tables.Add(dtReceiptDet);
                            orderDS.Tables.Add(dtSalesCommissionRecord);

                            byte[] orderDSData = SyncClientController.CompressDataSetToByteArray(orderDS);

                            string status = string.Empty;
                            if (ws.UploadOrderTables(orderDSData, UserInfo.username, out status))
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

                            DataSet dsOrderHdr = SQLChangeTracking.GetOrderHdrFullTable(dtLastClosing);
                            DataSet dsOrderDet = SQLChangeTracking.GetOrderDetFullTable(dtLastClosing);
                            DataSet dsReceiptHdr = SQLChangeTracking.GetReceiptHdrFullTable(dtLastClosing);
                            DataSet dsReceiptDet = SQLChangeTracking.GetReceiptDetFullTable(dtLastClosing);
                            DataSet dsSalesCommissionRecord = SQLChangeTracking.GetSalesCommissionRecordFullTable(dtLastClosing);

                            DataTable dtOrderHdr = dsOrderHdr.Tables[0].Copy();
                            DataTable dtOrderDet = dsOrderDet.Tables[0].Copy();
                            DataTable dtReceiptHdr = dsReceiptHdr.Tables[0].Copy();
                            DataTable dtReceiptDet = dsReceiptDet.Tables[0].Copy();
                            DataTable dtSalesCommissionRecord = dsSalesCommissionRecord.Tables[0].Copy();

                            DataSet orderDS = new DataSet();
                            orderDS.Tables.Add(dtOrderHdr);
                            orderDS.Tables.Add(dtOrderDet);
                            orderDS.Tables.Add(dtReceiptHdr);
                            orderDS.Tables.Add(dtReceiptDet);
                            orderDS.Tables.Add(dtSalesCommissionRecord);

                            byte[] orderDSData = SyncClientController.CompressDataSetToByteArray(orderDS);

                            string status = string.Empty;
                            if (ws.UploadOrderTables(orderDSData, UserInfo.username, out status))
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

        /// <summary>
        /// This method will update changes from client to server, table OrderHdr, 
        /// will update these five columns: UserFld1, UserFld2, UserFld3, UserFld6, Remark
        /// filtered by current POSId, and ViewBillLimit, and IsVoided = false
        /// for Lighting UserFld1, UserFld2, UserFld3, UserFld6 = deliveryType, DeliveryDate, DeliveryAddress, ShiftType
        /// </summary>
        /// <returns></returns>
        public static bool UpdateOrderHdrUserFld1236ByPosIDViewBillLimitAndIsVoided()
        {
            try
            {
                // Init POS Controller (to get POSId)
                PointOfSaleController.GetPointOfSaleInfo();

                // Get ViewBillLimit
                int viewBillLimit = int.Parse(AppSetting.GetSettingFromDBAndConfigFile("ViewBillLimit"));

                // Init WS
                Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Url = WS_URL;

                // Building Data to be uploaded
                DataSet dsOrderHdr = SQLChangeTracking.GetOrderHdrByPosIDViewBillLimitAndIsVoided(PointOfSaleInfo.PointOfSaleID,
                    viewBillLimit);

                byte[] orderHdrDSData = SyncClientController.CompressDataSetToByteArray(dsOrderHdr);

                string status = string.Empty;

                if (!ws.UploadTable(orderHdrDSData, false, UserInfo.username, out status))
                    return false;

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
