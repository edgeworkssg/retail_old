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
    public partial class LogsSync
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

        public static bool StartLogsSync()
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
                if (SQLChangeTracking.GetMinValidVersion("WarningMsg") == null)
                {
                    // Auto Enable Change Tracking on Client DB Table
                    SQLChangeTracking.EnableChangeTrackingTable("WarningMsg");
                }
                if (SQLChangeTracking.GetMinValidVersion("MembershipUpgradeLog") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("MembershipUpgradeLog");
                }
                if (SQLChangeTracking.GetMinValidVersion("CashRecording") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("CashRecording");
                }
                if (SQLChangeTracking.GetMinValidVersion("CounterCloseLog") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("CounterCloseLog");
                }
                if (SQLChangeTracking.GetMinValidVersion("CounterCloseDet") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("CounterCloseDet");
                }
                if (SQLChangeTracking.GetMinValidVersion("SpecialActivityLog") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("SpecialActivityLog");
                }
                if (SQLChangeTracking.GetMinValidVersion("PreOrderRecord") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("PreOrderRecord");
                }
                if (SQLChangeTracking.GetMinValidVersion("Membership") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("Membership");
                }
                if (SQLChangeTracking.GetMinValidVersion("MembershipRenewal") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("MembershipRenewal");
                }
                if (SQLChangeTracking.GetMinValidVersion("PackageRedemptionLog") == null)
                {
                    SQLChangeTracking.EnableChangeTrackingTable("PackageRedemptionLog");
                }

                // Check Last Sync on Server DB (NULL if not exist)
                string serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_LogsLastSyncVersion";
                string lastSyncVersion = ws.GetAppSettingValue(serverAppSettingKey);
                long longLastSyncVersion = -1;
                long.TryParse(lastSyncVersion, out longLastSyncVersion);

                // Check Min Valid Version (OrderHdr)
                long? minValidWarningMsg = SQLChangeTracking.GetMinValidVersion("WarningMsg");

                // Upload Data to Server
                if (lastSyncVersion == null)
                {
                    // Upload Everything

                    // Retrieve Client Logs Tables
                    // Into Dataset and then Compress

                    DataSet dsWarningMsg = SQLChangeTracking.GetWarningMsgFullTable(dtLastClosing);
                    DataSet dsMembershipUpgradeLog = SQLChangeTracking.GetMembershipUpgradeLogFullTable(dtLastClosing);
                    DataSet dsCashRecording = SQLChangeTracking.GetCashRecordingFullTable(dtLastClosing);
                    DataSet dsCounterCloseLog = SQLChangeTracking.GetCounterCloseLogFullTable(dtLastClosing);
                    DataSet dsCounterCloseDet = SQLChangeTracking.GetCounterCloseDetFullTable(dtLastClosing);
                    DataSet dsSpecialActivityLog = SQLChangeTracking.GetSpecialActivityLogFullTable(dtLastClosing);
                    DataSet dsPreOrderRecord = SQLChangeTracking.GetPreOrderRecordFullTable(dtLastClosing);
                    DataSet dsMembership = SQLChangeTracking.GetMembershipFullTable(dtLastClosing);
                    DataSet dsMembershipRenewal = SQLChangeTracking.GetMembershipRenewalFullTable(dtLastClosing);
                    DataSet dsPackageRedemptionLog = SQLChangeTracking.GetPackageRedemptionLogFullTable(dtLastClosing);

                    DataTable dtWarningMsg = dsWarningMsg.Tables[0].Copy();
                    DataTable dtMembershipUpgradeLog = dsMembershipUpgradeLog.Tables[0].Copy();
                    DataTable dtCashRecording = dsCashRecording.Tables[0].Copy();
                    DataTable dtCounterCloseLog = dsCounterCloseLog.Tables[0].Copy();
                    DataTable dtCounterCloseDet = dsCounterCloseDet.Tables[0].Copy();
                    DataTable dtSpecialActivityLog = dsSpecialActivityLog.Tables[0].Copy();
                    DataTable dtPreOrderRecord = dsPreOrderRecord.Tables[0].Copy();
                    DataTable dtMembership = dsMembership.Tables[0].Copy();
                    DataTable dtMembershipRenewal = dsMembershipRenewal.Tables[0].Copy();
                    DataTable dtPackageRedemptionLog = dsPackageRedemptionLog.Tables[0].Copy();

                    DataSet logsDS = new DataSet();
                    logsDS.Tables.Add(dtWarningMsg);
                    logsDS.Tables.Add(dtMembershipUpgradeLog);
                    logsDS.Tables.Add(dtCashRecording);
                    logsDS.Tables.Add(dtCounterCloseLog);
                    logsDS.Tables.Add(dtCounterCloseDet);
                    logsDS.Tables.Add(dtSpecialActivityLog);
                    logsDS.Tables.Add(dtPreOrderRecord);
                    logsDS.Tables.Add(dtMembership);
                    logsDS.Tables.Add(dtMembershipRenewal);
                    logsDS.Tables.Add(dtPackageRedemptionLog);

                    byte[] logsDSData = SyncClientController.CompressDataSetToByteArray(logsDS);

                    string status = string.Empty;
                    if (ws.UploadLogsTables(logsDSData, UserInfo.username, out status))
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
                        if (minValidWarningMsg <= longLastSyncVersion)
                        {
                            // Upload Partial

                            // Retrieve Client Order Tables (OrderHdr, OrderDet, ReceiptHdr, ReceiptDet, SalesCommissionRecord)
                            // Into Dataset and then Compress

                            DataSet dsWarningMsg = SQLChangeTracking.GetChanges("WarningMsg", longLastSyncVersion, "UniqueID");
                            DataSet dsMembershipUpgradeLog = SQLChangeTracking.GetChanges("MembershipUpgradeLog", longLastSyncVersion, "OrderHdrID");
                            DataSet dsCashRecording = SQLChangeTracking.GetChanges("CashRecording", longLastSyncVersion, "CashRecID");
                            DataSet dsCounterCloseLog = SQLChangeTracking.GetChanges("CounterCloseLog", longLastSyncVersion, "CounterCloseLogID");
                            DataSet dsCounterCloseDet = SQLChangeTracking.GetChanges("CounterCloseDet", longLastSyncVersion, "CounterCloseDetID");
                            DataSet dsSpecialActivityLog = SQLChangeTracking.GetChanges("SpecialActivityLog", longLastSyncVersion, "SpecialActivityLogID");
                            DataSet dsPreOrderRecord = SQLChangeTracking.GetChanges("PreOrderRecord", longLastSyncVersion, "PreOrderLogID");
                            DataSet dsMembership = SQLChangeTracking.GetChanges("Membership", longLastSyncVersion, "MembershipNo");
                            DataSet dsMembershipRenewal = SQLChangeTracking.GetChanges("MembershipRenewal", longLastSyncVersion, "renewalID");
                            DataSet dsPackageRedemptionLog = SQLChangeTracking.GetChanges("PackageRedemptionLog", longLastSyncVersion, "PackageRedeemID");

                            DataTable dtWarningMsg = dsWarningMsg.Tables[0].Copy();
                            DataTable dtMembershipUpgradeLog = dsMembershipUpgradeLog.Tables[0].Copy();
                            DataTable dtCashRecording = dsCashRecording.Tables[0].Copy();
                            DataTable dtCounterCloseLog = dsCounterCloseLog.Tables[0].Copy();
                            DataTable dtCounterCloseDet = dsCounterCloseDet.Tables[0].Copy();
                            DataTable dtSpecialActivityLog = dsSpecialActivityLog.Tables[0].Copy();
                            DataTable dtPreOrderRecord = dsPreOrderRecord.Tables[0].Copy();
                            DataTable dtMembership = dsMembership.Tables[0].Copy();
                            DataTable dtMembershipRenewal = dsMembershipRenewal.Tables[0].Copy();
                            DataTable dtPackageRedemptionLog = dsPackageRedemptionLog.Tables[0].Copy();

                            DataSet logsDS = new DataSet();
                            logsDS.Tables.Add(dtWarningMsg);
                            logsDS.Tables.Add(dtMembershipUpgradeLog);
                            logsDS.Tables.Add(dtCashRecording);
                            logsDS.Tables.Add(dtCounterCloseLog);
                            logsDS.Tables.Add(dtCounterCloseDet);
                            logsDS.Tables.Add(dtSpecialActivityLog);
                            logsDS.Tables.Add(dtPreOrderRecord);
                            logsDS.Tables.Add(dtMembership);
                            logsDS.Tables.Add(dtMembershipRenewal);
                            logsDS.Tables.Add(dtPackageRedemptionLog);

                            byte[] logsDSData = SyncClientController.CompressDataSetToByteArray(logsDS);

                            string status = string.Empty;
                            if (ws.UploadLogsTables(logsDSData, UserInfo.username, out status))
                            {
                                // Update App Setting on Server
                                ws.SetAppSettingValue(serverAppSettingKey, clientCTVersion.ToString());
                            }
                        }
                        else
                        {
                            // Upload Everything

                            // Retrieve Client Logs Tables
                            // Into Dataset and then Compress

                            DataSet dsWarningMsg = SQLChangeTracking.GetWarningMsgFullTable(dtLastClosing);
                            DataSet dsMembershipUpgradeLog = SQLChangeTracking.GetMembershipUpgradeLogFullTable(dtLastClosing);
                            DataSet dsCashRecording = SQLChangeTracking.GetCashRecordingFullTable(dtLastClosing);
                            DataSet dsCounterCloseLog = SQLChangeTracking.GetCounterCloseLogFullTable(dtLastClosing);
                            DataSet dsCounterCloseDet = SQLChangeTracking.GetCounterCloseDetFullTable(dtLastClosing);
                            DataSet dsSpecialActivityLog = SQLChangeTracking.GetSpecialActivityLogFullTable(dtLastClosing);
                            DataSet dsPreOrderRecord = SQLChangeTracking.GetPreOrderRecordFullTable(dtLastClosing);
                            DataSet dsMembership = SQLChangeTracking.GetMembershipFullTable(dtLastClosing);
                            DataSet dsMembershipRenewal = SQLChangeTracking.GetMembershipRenewalFullTable(dtLastClosing);
                            DataSet dsPackageRedemptionLog = SQLChangeTracking.GetPackageRedemptionLogFullTable(dtLastClosing);

                            DataTable dtWarningMsg = dsWarningMsg.Tables[0].Copy();
                            DataTable dtMembershipUpgradeLog = dsMembershipUpgradeLog.Tables[0].Copy();
                            DataTable dtCashRecording = dsCashRecording.Tables[0].Copy();
                            DataTable dtCounterCloseLog = dsCounterCloseLog.Tables[0].Copy();
                            DataTable dtCounterCloseDet = dsCounterCloseDet.Tables[0].Copy();
                            DataTable dtSpecialActivityLog = dsSpecialActivityLog.Tables[0].Copy();
                            DataTable dtPreOrderRecord = dsPreOrderRecord.Tables[0].Copy();
                            DataTable dtMembership = dsMembership.Tables[0].Copy();
                            DataTable dtMembershipRenewal = dsMembershipRenewal.Tables[0].Copy();
                            DataTable dtPackageRedemptionLog = dsPackageRedemptionLog.Tables[0].Copy();

                            DataSet logsDS = new DataSet();
                            logsDS.Tables.Add(dtWarningMsg);
                            logsDS.Tables.Add(dtMembershipUpgradeLog);
                            logsDS.Tables.Add(dtCashRecording);
                            logsDS.Tables.Add(dtCounterCloseLog);
                            logsDS.Tables.Add(dtCounterCloseDet);
                            logsDS.Tables.Add(dtSpecialActivityLog);
                            logsDS.Tables.Add(dtPreOrderRecord);
                            logsDS.Tables.Add(dtMembership);
                            logsDS.Tables.Add(dtMembershipRenewal);
                            logsDS.Tables.Add(dtPackageRedemptionLog);

                            byte[] logsDSData = SyncClientController.CompressDataSetToByteArray(logsDS);

                            string status = string.Empty;
                            if (ws.UploadLogsTables(logsDSData, UserInfo.username, out status))
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
