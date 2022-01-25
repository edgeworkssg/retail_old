using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinPowerPOS.OrderForms;
using WinPowerPOS.EditBillForms;
using WinPowerPOS.MembershipForm;
using PowerPOS;
using PowerPOS.Container;
using System.Threading;
using System.Globalization;
using WinPowerPOS.LoginForms;
using WinPowerPOS.MembershipForms;
using WinPowerPOS.ItemForms;
using WinPowerPOS.Setup;
using POSDevices;
using PowerInventory;
using System.IO;
using WinVoucherBatchGenerator;
using WinPowerPOS.PromoAdmin;
//using WinPowerPOS.InstallmentForms;
using WinPowerPOS.DepositForms;
using WinPowerPOS.Reports;
using WinPowerPOS.Package;
using PowerInventory.InventoryForms;
using WinPowerPOS.AppointmentForms;
using System.Configuration;
using System.Resources;
using LanguageManager = WinPowerPOS.Properties.Language;
using WinPowerPOS.MaintenanceForms;
using System.Reflection;
using PowerPOS.SalesController;
using PowerPOS.InventoryRealTimeController;
using PowerPOS.SyncLogsController;
using WinPowerPOS.Delivery;
using PowerPOS.SyncPerformanceLogController;
using PowerPOSLib.RatingController;
using WinPowerPOS.BarcodePrinter;

namespace WinPowerPOS
{
    public partial class frmMain : Form
    {
        public static BackgroundWorker SyncSalesThread;
        public static BackgroundWorker SyncQuotationThread;
        public static BackgroundWorker SyncCashRecordingThread;
        public static BackgroundWorker SyncInventoryThread;
        public static BackgroundWorker SyncLogsThread;
        public static BackgroundWorker SyncAccessLogsThread;
        public static BackgroundWorker SyncMasterDataThread;
        public static BackgroundWorker SyncUserThread;
        public static BackgroundWorker SyncProductThread;
        public static BackgroundWorker SyncMemberThread;
        public static BackgroundWorker SyncAppointmentThread;
        public static BackgroundWorker SyncPerformanceLogThread;
        public static BackgroundWorker SyncSendAppointmentThread;
        public static BackgroundWorker SyncGetDeliveryOrderThread;
        public static BackgroundWorker SyncSendDeliveryOrderThread;
        public static BackgroundWorker SyncRatingThread;
        public static BackgroundWorker SyncAttendanceThread;
        public static BackgroundWorker SyncVoucherThread;

        public frmViewSyncLog fSyncLog;

        private List<string> syncInventoryHistory = new List<string>();
        private List<string> getInventoryHistory = new List<string>();
        private List<string> syncSalesHistory = new List<string>();
        private List<string> syncQuotationHistory = new List<string>();
        private List<string> syncCashRecordingHistory = new List<string>();
        private List<string> syncLogsHistory = new List<string>();
        private List<string> syncAccessLogsHistory = new List<string>();
        private List<string> syncMasterDataHistory = new List<string>();
        private List<string> syncUserHistory = new List<string>();
        private List<string> syncProductHistory = new List<string>();
        private List<string> syncMemberHistory = new List<string>();
        private List<string> syncAppointmentHistory = new List<string>();
        private List<string> syncSendAppointmentHistory = new List<string>();
        private List<string> syncPerformanceLogHistory = new List<string>();
        private List<string> syncDeliveryOrderHistory = new List<string>();
        private List<string> syncRatingHistory = new List<string>();
        private List<string> syncAttendanceHistory = new List<string>();
        private List<string> syncVoucherHistory = new List<string>();


        private int maxHistory = 1000;

        #region cashRecycler
        //CashRecycler cashRecycler = new CashRecycler();
        #endregion


        public frmMain()
        {
            #region *) Load Culture Code
            string CultureCode =
                  AppSetting.GetSetting(AppSetting.SettingsName.CultureCode);

            if (CultureCode != null)
            {
                try
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureCode);
                }
                catch (Exception ex)
                {
                    Logger.writeLog("Invalid Culture Code Specified in Setting!" + ex.Message);
                    MessageBox.Show(LanguageManager.Invalid_Culture_Code_Specified_in_Setting_ + ex.Message);

                }
            }
            #endregion
            //ResourceManager rm = new ResourceManager("Language", typeof(Language).Assembly);

            InitializeComponent();
            PointOfSaleController.GetPointOfSaleInfo();

            if (Thread.CurrentThread.CurrentUICulture.Name != "zh-CN")
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
                {
                    btnMembership.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper() + " LIST";
                }
                else
                {
                    btnMembership.Text = "MEMBERS LIST";
                }
            }

            #region -) Real Time Sync

            #region *) Real Time Sync Sales
            fSyncLog = new frmViewSyncLog();

            //Init sync
            SyncSalesThread = new BackgroundWorker();
            DoWorkEventArgs doe = new DoWorkEventArgs(null);
            SyncSalesThread.DoWork += new DoWorkEventHandler(SyncThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
            {
                SyncSalesThread.RunWorkerAsync();
            }
            #endregion

            #region -) Real Time Sync Quotation

            SyncQuotationThread = new BackgroundWorker();
            SyncQuotationThread.DoWork += new DoWorkEventHandler(SyncQuotationThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncQuotation), false))
                SyncQuotationThread.RunWorkerAsync();

            #endregion

            #region -) Real Time Sync Cash Recording

            SyncCashRecordingThread = new BackgroundWorker();
            SyncCashRecordingThread.DoWork += new DoWorkEventHandler(SyncCashRecordingThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncCashRecording), false))
                SyncCashRecordingThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Inventory
            //Init sync
            SyncInventoryThread = new BackgroundWorker();
            DoWorkEventArgs sicDoe = new DoWorkEventArgs(null);
            SyncInventoryThread.DoWork += new DoWorkEventHandler(SyncInventoryThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeInventory), false))
            {
                SyncInventoryThread.RunWorkerAsync();
            }

            #endregion

            #region *) Real Time Sync Closing

            SyncLogsThread = new BackgroundWorker();
            DoWorkEventArgs SyncLogsEvent = new DoWorkEventArgs(null);
            SyncLogsThread.DoWork += new DoWorkEventHandler(SyncLogsThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncLogs), false))
            {
                SyncLogsThread.RunWorkerAsync();
            }

            #endregion

            #region *) Real Time Sync Access Log

            SyncAccessLogsThread = new BackgroundWorker();
            DoWorkEventArgs SyncAccessLogsEvent = new DoWorkEventArgs(null);
            SyncAccessLogsThread.DoWork += new DoWorkEventHandler(SyncAccessLogsThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAccessLog), false))
            {
                SyncAccessLogsThread.RunWorkerAsync();
            }

            #endregion

            #region *) Real Time Sync Product

            SyncProductThread = new BackgroundWorker();
            SyncProductThread.DoWork += new DoWorkEventHandler(SyncProductThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncProducts), false))
                SyncProductThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Master Data

            SyncMasterDataThread = new BackgroundWorker();
            SyncMasterDataThread.DoWork += new DoWorkEventHandler(SyncMasterDataThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMasterData), false))
                SyncMasterDataThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync User

            SyncUserThread = new BackgroundWorker();
            SyncUserThread.DoWork += new DoWorkEventHandler(SyncUserThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncUser), false))
                SyncUserThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Member

            SyncMemberThread = new BackgroundWorker();
            SyncMemberThread.DoWork += new DoWorkEventHandler(SyncMemberThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMember), false))
                SyncMemberThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Appointment
            SyncAppointmentThread = new BackgroundWorker();
            SyncAppointmentThread.DoWork += new DoWorkEventHandler(SyncAppointmentThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncAppointment), false))
                SyncAppointmentThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Performance Log

            SyncPerformanceLogThread = new BackgroundWorker();
            SyncPerformanceLogThread.DoWork += new DoWorkEventHandler(SyncPerformanceLogThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncPerformanceLog), false))
                SyncPerformanceLogThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Send Appointment

            SyncSendAppointmentThread = new BackgroundWorker();
            SyncSendAppointmentThread.DoWork += new DoWorkEventHandler(SyncSendAppointmentThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncAppointment), false))
                SyncSendAppointmentThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Get Delivery Order
            SyncGetDeliveryOrderThread = new BackgroundWorker();
            SyncGetDeliveryOrderThread.DoWork += new DoWorkEventHandler(SyncGetDeliveryOrderThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncDeliveryOrder), false))
                SyncGetDeliveryOrderThread.RunWorkerAsync();
            #endregion

            #region *) Real Time Sync Send Delivery Order
            SyncSendDeliveryOrderThread = new BackgroundWorker();
            SyncSendDeliveryOrderThread.DoWork += new DoWorkEventHandler(SyncSendDeliveryOrderThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncDeliveryOrder), false))
                SyncSendDeliveryOrderThread.RunWorkerAsync();
            #endregion

            #region *) Real Time Sync Rating

            SyncRatingThread = new BackgroundWorker();
            SyncRatingThread.DoWork += new DoWorkEventHandler(SyncRatingThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncRating), false))
                SyncRatingThread.RunWorkerAsync();

            #endregion

            #region *) Real Time Sync Attendance

            SyncAttendanceThread = new BackgroundWorker();
            DoWorkEventArgs SyncAttendanceEvent = new DoWorkEventArgs(null);
            SyncAttendanceThread.DoWork += new DoWorkEventHandler(SyncAttendanceThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncAttendance), false))
            {
                SyncAttendanceThread.RunWorkerAsync();
            }

            #endregion

            #region *) Real Time Sync Voucher

            SyncVoucherThread = new BackgroundWorker();
            SyncVoucherThread.DoWork += new DoWorkEventHandler(SyncVoucherThread_DoWork);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncVoucher), false))
                SyncVoucherThread.RunWorkerAsync();

            #endregion

            #endregion

        }

        void SyncRatingThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncRatingRealTimeController syncRatingRealTimeController = new SyncRatingRealTimeController();
            syncRatingRealTimeController.OnProgressUpdates += new PowerPOSLib.RatingController.UpdateProgress(syncRatingRealTimeController_OnProgressUpdates);
            syncRatingRealTimeController.SendRealTimeRating();
        }

        void syncRatingRealTimeController_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
                fSyncLog = new frmViewSyncLog();
            fSyncLog.SyncRatingStatus = message;
            addLog("rating", message);
        }

        void SyncCashRecordingThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PowerPOS.SyncCashRecording.SyncCashRecordingRealTimeController syncCashRecordingCtrl =
                    new PowerPOS.SyncCashRecording.SyncCashRecordingRealTimeController();
                syncCashRecordingCtrl.OnProgressUpdates += new PowerPOS.SyncCashRecording.UpdateProgress(syncCashRecordingCtrl_OnProgressUpdates);
                syncCashRecordingCtrl.SendCashRecording();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void syncCashRecordingCtrl_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
                fSyncLog = new frmViewSyncLog();
            fSyncLog.SyncCashRecordingStatus = message;
            addLog("cashrecording", message);
        }

        void SyncQuotationThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PowerPOS.SyncQuotation.SyncQuotationRealTimeController syncQuoteLogsCtrl =
                    new PowerPOS.SyncQuotation.SyncQuotationRealTimeController();
                syncQuoteLogsCtrl.OnProgressUpdates += new PowerPOS.SyncQuotation.UpdateProgress(syncQuoteLogsCtrl_OnProgressUpdates);
                syncQuoteLogsCtrl.SendQuotations();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void syncQuoteLogsCtrl_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
                fSyncLog = new frmViewSyncLog();
            fSyncLog.SyncQuotationStatus = message;
            addLog("quotation", message);
        }

        void SyncAccessLogsThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PowerPOS.AccessLogSyncController.SyncAccessLogRealTimeController syncAccessLogsCtrl = 
                    new PowerPOS.AccessLogSyncController.SyncAccessLogRealTimeController();
                syncAccessLogsCtrl.OnProgressUpdates += new PowerPOS.AccessLogSyncController.UpdateProgress(syncAccessLogsCtrl_OnProgressUpdates);
                syncAccessLogsCtrl.SendRealTimeAccessLog();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void syncAccessLogsCtrl_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncAccessLogStatus = message;
            addLog("accesslog", message);
        }

        void SyncAttendanceThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PowerPOS.SyncAttendanceRealTimeController.SyncAttendanceRealTimeController syncAttendanceCtrl =
                    new PowerPOS.SyncAttendanceRealTimeController.SyncAttendanceRealTimeController();
                syncAttendanceCtrl.OnProgressUpdates += new PowerPOS.SyncAttendanceRealTimeController.UpdateProgress(SyncAttendanceCtrl_OnProgressUpdates);
                syncAttendanceCtrl.SendRealTimeAttendance();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncAttendanceCtrl_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncAttendanceStatus = message;
            addLog("attendance", message);
        }

        void SyncProductThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncProductController.SyncProductRealTimeController spc = 
                    new PowerPOS.SyncProductController.SyncProductRealTimeController();
                spc.OnProgressUpdates += new PowerPOS.SyncProductController.UpdateProgress(spc_OnProgressUpdates);
                spc.SyncProducts();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void spc_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncProductStatus = message;
            addLog("product", message);
        }

        void SyncMasterDataThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncMasterDataRealTimeController.SyncMasterDataRealTimeController syncMasterDataRealTime = 
                    new PowerPOS.SyncMasterDataRealTimeController.SyncMasterDataRealTimeController();
                syncMasterDataRealTime.OnProgressUpdates += new PowerPOS.SyncMasterDataRealTimeController.UpdateProgress(syncMasterDataRealTime_OnProgressUpdates);
                syncMasterDataRealTime.SyncMasterData();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void syncMasterDataRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncMasterDataStatus = message;
            addLog("masterdata", message);
        }

        void SyncUserThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncUserRealTimeController.SyncUserRealTimeController syncUserRealTime = 
                    new PowerPOS.SyncUserRealTimeController.SyncUserRealTimeController();
                syncUserRealTime.OnProgressUpdates += new PowerPOS.SyncUserRealTimeController.UpdateProgress(syncUserRealTime_OnProgressUpdates);
                syncUserRealTime.SyncUser();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void syncUserRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncUserStatus = message;
            addLog("user", message);
        }

        void SyncMemberThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncMemberRealTimeController.SyncMemberRealTimeController syncMemberRealTime =
                    new PowerPOS.SyncMemberRealTimeController.SyncMemberRealTimeController();
                syncMemberRealTime.OnProgressUpdates += new PowerPOS.SyncMemberRealTimeController.UpdateProgress(SyncMemberRealTime_OnProgressUpdates);
                bool result = syncMemberRealTime.SyncMembership();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncMemberRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncMembershipStatus = message;
            addLog("membership", message);
        }

        private void SyncLogsThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SyncLogsRealTimeController syncLogsCtrl = new SyncLogsRealTimeController();
                syncLogsCtrl.OnProgressUpdates += syncLogsCtrl_OnProgressUpdates;
                syncLogsCtrl.SendLogs();
                syncLogsCtrl.SendLoginActivity();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void syncLogsCtrl_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncLogStatus = message;
            addLog("Logs", message);
        }

        private void SyncPerformanceLogThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SyncPerformanceLogRealTimeController syncPerformanceLogCtrl = new SyncPerformanceLogRealTimeController();
                syncPerformanceLogCtrl.OnProgressUpdates += syncPerformanceLogCtrl_OnProgressUpdates;
                syncPerformanceLogCtrl.SendPerformanceLog();
                syncPerformanceLogCtrl.SendPerformanceLogSummary();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void syncPerformanceLogCtrl_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncPerformanceLogStatus = message;
            addLog("performancelog", message);
        }


        void SyncSendAppointmentThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.AppointmentRealTimeController.SyncAppointmentRealTimeController syncAppointmentRealTimeSend =
                    new PowerPOS.AppointmentRealTimeController.SyncAppointmentRealTimeController();
                syncAppointmentRealTimeSend.OnProgressUpdates += new PowerPOS.AppointmentRealTimeController.UpdateProgress(SyncSendAppointmentRealTime_OnProgressUpdates);
                bool result = syncAppointmentRealTimeSend.UploadAppointment();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncSendAppointmentRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncAppointmentStatus = message;
            addLog("appointment", message);
        }

        void SyncAppointmentThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.AppointmentRealTimeController.SyncAppointmentRealTimeController syncAppointmentRealTime =
                    new PowerPOS.AppointmentRealTimeController.SyncAppointmentRealTimeController();
                syncAppointmentRealTime.OnProgressUpdates += new PowerPOS.AppointmentRealTimeController.UpdateProgress(SyncAppointmentRealTime_OnProgressUpdates);
                syncAppointmentRealTime.OnDataDownloaded += new PowerPOS.AppointmentRealTimeController.DataDownloadedHandler(SyncAppointmentRealTime_OnDataDownloaded);
                bool result = syncAppointmentRealTime.DownloadAppointment();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncAppointmentRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncAppointmentStatus = message;
            addLog("appointment", message);
        }

        void SyncAppointmentRealTime_OnDataDownloaded(object sender, string message)
        {
            if (frmAppointment != null && !frmAppointment.IsDisposed)
            {
                frmAppointment.SetDayFromMainForm();
            }

            if (frmAppointmentWeekly != null && !frmAppointmentWeekly.IsDisposed)
            {
                frmAppointmentWeekly.SetDayFromMainForm();
            }

            if (frmAppointmentMonthly != null && !frmAppointmentMonthly.IsDisposed)
            {
                frmAppointmentMonthly.SetDayFromMainForm();
            }

            if (frmRoomStatus != null && !frmRoomStatus.IsDisposed)
            {
                frmRoomStatus.SetDayFromMainForm();
            }
        }

        void SyncGetDeliveryOrderThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.DeliveryOrderRealTimeController.SyncDeliveryOrderRealTimeController syncDeliveryOrderRealTimeGet =
                    new PowerPOS.DeliveryOrderRealTimeController.SyncDeliveryOrderRealTimeController();
                syncDeliveryOrderRealTimeGet.OnProgressUpdates += new PowerPOS.DeliveryOrderRealTimeController.UpdateProgress(SyncGetDeliveryOrderRealTime_OnProgressUpdates);
                syncDeliveryOrderRealTimeGet.OnDataDownloaded += new PowerPOS.DeliveryOrderRealTimeController.DataDownloadedHandler(SyncGetDeliveryOrderRealTime_OnDataDownloaded);
                bool result = syncDeliveryOrderRealTimeGet.DownloadDeliveryOrder();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncGetDeliveryOrderRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncDeliveryOrderStatus = message;
            addLog("deliveryorder", message);
        }

        void SyncGetDeliveryOrderRealTime_OnDataDownloaded(object sender, string message)
        {
            // Do nothing for now
        }

        void SyncSendDeliveryOrderThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.DeliveryOrderRealTimeController.SyncDeliveryOrderRealTimeController syncDeliveryOrderRealTimeSend =
                    new PowerPOS.DeliveryOrderRealTimeController.SyncDeliveryOrderRealTimeController();
                syncDeliveryOrderRealTimeSend.OnProgressUpdates += new PowerPOS.DeliveryOrderRealTimeController.UpdateProgress(SyncSendDeliveryOrderRealTime_OnProgressUpdates);
                bool result = syncDeliveryOrderRealTimeSend.UploadDeliveryOrder();
                e.Result = result;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncSendDeliveryOrderRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncDeliveryOrderStatus = message;
            addLog("deliveryorder", message);
        }

        void SyncVoucherThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncVoucherRealTimeController.SyncVoucherRealTimeController syncVoucherRealTime =
                    new PowerPOS.SyncVoucherRealTimeController.SyncVoucherRealTimeController();
                syncVoucherRealTime.OnProgressUpdates += new PowerPOS.SyncVoucherRealTimeController.UpdateProgress(syncVoucherRealTime_OnProgressUpdates);
                syncVoucherRealTime.SyncVoucher();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void syncVoucherRealTime_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncVoucherStatus = message;
            addLog("voucher", message);
        }


        //Modified by Graham P. 1.21.2013,added flow layout panel as container of buttons
        private void DisableButtons()
        {
            /*Disabled Setup > btnPromo*/
            btnPromo.Enabled = false;
            btnPromo.Visible = false;

            if (PrintSettingInfo.receiptSetting != null && PrintSettingInfo.receiptSetting.ShowSalesPersonInfo)
            {
                string tmp = PrintSettingInfo.receiptSetting.SalesPersonTitle.ToUpper();
                if (tmp == "") tmp = "SALES PERSON";

                btnSalesByStylist.Text = btnSalesByStylist.Text.Replace
                    (tmp, "SALES PERSON");

                //btnDailySalesByStylist.Text = btnDailySalesByStylist.Text.Replace
                //    (tmp, "SALES PERSON");

                btnProductSalesByStylist.Text = btnProductSalesByStylist.Text.Replace
                    (tmp, "SALES PERSON");
            }
            for (int i = 0; i < tbPOS.TabPages.Count; i++)
            {
                for (int j = 0; j < tbPOS.TabPages[i].Controls.Count; j++)
                {
                    if (tbPOS.TabPages[i].Controls[j] is FlowLayoutPanel)
                    {
                        foreach(Control ctl in tbPOS.TabPages[i].Controls[j].Controls)
                        {
                            if (ctl is Button)
                            {
                                //tbPOS.TabPages[i].Controls[j].Enabled = false;
                                ctl.Enabled = false;
                                ctl.Visible = false;
                            }
                            
                        }
                    }
                }
            }
        }
        //Modified by Graham P. 1.21.2013,added flow layout panel as container of buttons
        private void EnableByPrivilegesButtons()
        {
            bool inventoryOnly = false;
            
            if (AppSetting.GetSettingFromDBAndConfigFile("InventoryMode") != null 
                && AppSetting.GetSettingFromDBAndConfigFile("InventoryMode").ToUpper() == "YES")
            {
                inventoryOnly = true;
            }

            tbPOS.TabPages.Remove(tpVoucher);
            string tabVoucherPrivilege = "Tab Voucher";
            DataRow[] drt = UserInfo.privileges.Select("privilegeName = '" + tabVoucherPrivilege + "'");
            if (UserInfo.username.ToLower() == "edgeworks" || drt.Length > 0)
            {
                tbPOS.TabPages.Add(tpVoucher);
            }

            if (UserInfo.username != "")
            {
                //DisableButtons();
                for (int i = 0; i < tbPOS.TabPages.Count; i++)
                {
                    for (int j = 0; j < tbPOS.TabPages[i].Controls.Count; j++)
                    {
                        if (tbPOS.TabPages[i].Controls[j] is FlowLayoutPanel) 
                        {
                            foreach (Control ctl in tbPOS.TabPages[i].Controls[j].Controls)
                            {
                                if (ctl is Button)
                                {
                                    string ButtonName = ((Button)ctl).Tag.ToString();
                                    DataRow[] dr = UserInfo.privileges.Select("privilegeName = '" + ButtonName + "'");
                                    if (UserInfo.username.ToLower() == "edgeworks" || dr.Length > 0)
                                    {
                                        ctl.Enabled = true;
                                        ctl.Visible = true;
                                    }
                                    if ((ButtonName == "SALES INVOICE" ||
                                        ButtonName == "CASH RECORDING" ||
                                        ButtonName == "CHECK OUT")
                                        && inventoryOnly)
                                    {
                                        ctl.Visible = false;
                                        ctl.Enabled = false;
                                        //tbPOS.TabPages[i].Controls[j].Enabled = false;
                                    }

                                    if (ButtonName == "FREEZE POS")
                                    {
                                       if (UserInfo.username == "edgeworks" || UserInfo.role.ToLower() == "admin")
                                        {
                                            ctl.Visible = true;
                                            ctl.Enabled = true;

                                        }
                                        else 
                                        {
                                            ctl.Visible = false;
                                            ctl.Enabled = false;

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                
                if (PrintSettingInfo.receiptSetting.ShowSalesPersonInfo)
                {
                    string tmp = PrintSettingInfo.receiptSetting.SalesPersonTitle.ToUpper();
                    if (tmp == "") tmp = "SALES PERSON";
                    btnSalesByStylist.Text = btnSalesByStylist.Text.Replace
                        ("SALES PERSON",
                        tmp);

                    //btnDailySalesByStylist.Text = btnDailySalesByStylist.Text.Replace
                    //    ("SALES PERSON",
                    //    tmp);

                    btnProductSalesByStylist.Text = btnProductSalesByStylist.Text.Replace
                        ("SALES PERSON",
                        tmp);
                }

                btnMembersAttendance.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false);
                //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableQuotation), false))
                //    btnQuotation.Visible = false;
                btnQuotation.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableQuotation), false);
            }
        }
        frmReceiptList FfrmReceiptList;
        private void btnDailySales_Click(object sender, EventArgs e)
        {
            if (FfrmReceiptList == null || FfrmReceiptList.IsDisposed)
            {
                FfrmReceiptList = new frmReceiptList();
                FfrmReceiptList.SyncSalesThread = SyncSalesThread;
                FfrmReceiptList.fOrderTaking = FrmOrderTaking;
                FfrmReceiptList.SyncMemberThread = SyncMemberThread;
                FfrmReceiptList.Show();
            }
            else
            {
                FfrmReceiptList.WindowState = FormWindowState.Normal;
                FfrmReceiptList.Activate();
            }
        }
        frmCheckOut FfrmCheckOut;
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            /*if (FfrmCheckOut == null || FfrmCheckOut.IsDisposed)
            {*/
                FfrmCheckOut = new frmCheckOut();
                FfrmCheckOut.syncLogsThread = SyncLogsThread;
                FfrmCheckOut.ShowDialog();
                if (FfrmCheckOut.isClosed)
                {
                    lblCashierName.Text = "-";
                    UserController.ClearUserInfo();
                    PriceDisplay myDisplay = new PriceDisplay();
                    myDisplay.CounterClose();
                    List<Form> openForms = new List<Form>();

                    foreach (Form f in Application.OpenForms)
                        openForms.Add(f);

                    foreach (Form f in openForms)
                    {
                        if (f.Name != "frmMain")
                            f.Close();
                    }
                    frmMain_Load(this, new EventArgs());
                }
            /*}
            else
            {
                FfrmCheckOut.WindowState = FormWindowState.Normal;
                FfrmCheckOut.Activate();
            }*/
            


        }
        private frmOrderTaking FrmOrderTaking;
        private void btnOrderTaking_Click(object sender, EventArgs e)
        {
            if (FrmOrderTaking == null || FrmOrderTaking.IsDisposed)
            {
                FrmOrderTaking = new frmOrderTaking();
                FrmOrderTaking.SyncSalesThread = SyncSalesThread;
                FrmOrderTaking.SyncMemberThread = SyncMemberThread;
                FrmOrderTaking.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                FrmOrderTaking.SyncRatingThread = SyncRatingThread;
                FrmOrderTaking.SyncAttendanceThread = SyncAttendanceThread;
                FrmOrderTaking.SyncCashRecordingThread = SyncCashRecordingThread;
                //FrmOrderTaking.cashRecycler = cashRecycler;
                FrmOrderTaking.Show();
                DisableButtons();
                EnableByPrivilegesButtons();
                
                PowerPOS.Feature.Package.isAvailable = (AppSetting.GetSettingFromDBAndConfigFile("PackageFeature") != null && AppSetting.GetSettingFromDBAndConfigFile("PackageFeature").ToString().ToUpper() == "YES");
                PowerPOS.Feature.Package.isRealTime = (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem") != null && AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem").ToString().ToUpper() == "YES");
                
                lblCashierName.Text = UserInfo.displayName; //just in case cashier change
                tRegistration.Text = "Registered to: " + CompanyInfo.CompanyName;
            }
            else
            {
                if (!FrmOrderTaking.Visible) FrmOrderTaking.Show();
                FrmOrderTaking.WindowState = FormWindowState.Maximized;
                FrmOrderTaking.Activate();
            }
        }

        private frmSalesReport FrmSalesReport;
        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            if (FrmSalesReport == null || FrmSalesReport.IsDisposed)
            {
                FrmSalesReport = new frmSalesReport();
                FrmSalesReport.Show();
            }
            else
            {
                FrmSalesReport.WindowState = FormWindowState.Normal;
                FrmSalesReport.Activate();
            }
        }

        private frmSync FrmSync;
        private void btnSync_Click(object sender, EventArgs e)
        {
            
            if (SyncClientController.WS_URL == "")
            {
                return;
            }
            else
            {
                if (FrmSync == null || FrmSync.IsDisposed)
                {
                    FrmSync = new frmSync();
                    FrmSync.SyncSalesThread = SyncSalesThread;
                    FrmSync.SyncMasterDataThread = SyncMasterDataThread;
                    FrmSync.SyncLogsThread = SyncLogsThread;
                    FrmSync.SyncProductThread = SyncProductThread;
                    FrmSync.SyncUserThread = SyncUserThread;
                    FrmSync.Show();

                }
                else
                {
                    FrmSync.WindowState = FormWindowState.Normal;
                    FrmSync.Activate();
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Logger.writeLog("Disable Button");
            DisableButtons();

            //Logger.writeLog("Set Controller");
            #region *) Fetch: Load Banner Image
            if (File.Exists(Application.StartupPath + "\\banner.jpg"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\banner.jpg");
                pictureBox1.Refresh();
            }
            #endregion

            pnlMainScreen.Visible = false;
           
            btnUpload.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false);

            string useMagneticStripReader = 
                AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForLogin);

            bool IsAuthorized = false;
            if (useMagneticStripReader != null && useMagneticStripReader.ToLower() == "yes")
            {
                frmReadMSR f = new frmReadMSR();
                //f.privilegeName = PrivilegesController.;
                f.loginType = LoginType.Login;
                f.ShowDialog();
                IsAuthorized = f.IsAuthorized;
                f.Dispose();
            }
            else
            {
                LoginForms.frmPOSLogin f = new WinPowerPOS.LoginForms.frmPOSLogin();
                f.allowClose = false;
                f.ShowDialog();
                IsAuthorized = f.IsSuccessful;                
                f.Dispose();
            }

            #region *) Try To Sync to server if is enabled
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncLogs), false))
                if (!SyncLogsThread.IsBusy)
                    SyncLogsThread.RunWorkerAsync();
            #endregion

            Logger.writeLog("Set Authorization");

            if (IsAuthorized)
            {
                PointOfSaleController.GetPointOfSaleInfo();
                AttributesLabelController.FetchProductAttributeLabel();
                PowerPOS.Feature.Package.isAvailable = (AppSetting.GetSettingFromDBAndConfigFile("PackageFeature") != null && AppSetting.GetSettingFromDBAndConfigFile("PackageFeature").ToString().ToUpper() == "YES");
                PowerPOS.Feature.Package.isRealTime = (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem") != null && AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem").ToString().ToUpper() == "YES");

                System.Version version = Assembly.GetExecutingAssembly().GetName().Version;
                lblVersion.Text = "V" + UtilityController.GetCurrentVersionMajorMinor();
                lblRevision.Text = "(rev " + UtilityController.GetCurrentVersion() + ")";
                //lblVersion.Text = lblVersion.Text + " - " + "Contact : +65 66522567, 84250633 ";
                SyncClientController.Load_WS_URL();
                bool isLocalhost = SyncClientController.WS_URL.StartsWith("http://localhost"); //|| SyncClientController.WS_URL.StartsWith("http://127.0.0.1");
                bool isIntegrateWithInventory = PointOfSaleInfo.IntegrateWithInventory;

                if (isLocalhost && isIntegrateWithInventory) { /* TRUE */  }
                else if (!isLocalhost && !isIntegrateWithInventory) { /* TRUE */ }
                else
                {
                    MessageBox.Show("Inventory settings error in the program. Please call our support team to check.");
                    Application.Exit();
                    return;
                }

                //Logger.writeLog("Assign Setting");
                #region *) Assign Setting: Costing Method
                string tmpCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (tmpCostingMethod == null)
                {
                    AppSetting.SetSetting(AppSetting.SettingsName.Inventory.CostingMethod, InventoryController.CostingTypes.FIFO);
                    tmpCostingMethod = InventoryController.CostingTypes.FIFO;
                }
                tmpCostingMethod = tmpCostingMethod.ToLower();
                if (tmpCostingMethod == InventoryController.CostingTypes.FIFO)
                    PowerPOS.Container.InventorySettings.CostingMethod = CostingMethods.FIFO;
                else if (tmpCostingMethod == InventoryController.CostingTypes.FixedAvg)
                    PowerPOS.Container.InventorySettings.CostingMethod = CostingMethods.FixedAvg;
                else if (tmpCostingMethod == InventoryController.CostingTypes.WeightedAvg)
                    PowerPOS.Container.InventorySettings.CostingMethod = CostingMethods.WeightedAvg;
                #endregion

                string TmpAppConfig;
                #region *) LoadSettings: Points.AskRemarksWhenUsePoint
                TmpAppConfig = AppSetting.GetSetting(AppSetting.SettingsName.Points.AskRemarksWhenUsePoint);
                if (TmpAppConfig == null) AppSetting.SetSetting(AppSetting.SettingsName.Points.AskRemarksWhenUsePoint, "no");
                #endregion

                lblCashierName.Text = UserInfo.displayName;
                tRegistration.Text = "Registered to: " + CompanyInfo.CompanyName;
                lblLocation.Text = PointOfSaleInfo.PointOfSaleName;
                
                EnableByPrivilegesButtons();

                //#region *) Check btn Sync -- Use Real Time Sales
                //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                //{
                //    btnSync.Enabled = false;
                //}
                //#endregion

                #region "Show/Hide Tab Promotion"

                #endregion

                #region Hide Tabs
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Interface.HideInventoryTab), false))
                {
                    int idx = tbPOS.TabPages.IndexOf(tabPage4);
                    if (idx != -1)
                    {
                        tbPOS.TabPages.RemoveAt(idx);
                    }
                }
                else
                {
                    int idx = tbPOS.TabPages.IndexOf(tabPage4);
                    if (idx == -1)
                    {
                        tbPOS.TabPages.Add(tabPage4);
                        //tpEdgeworks.Show();
                        foreach (Control ctl in tabPage4.Controls)
                        {
                            ctl.Visible = true;
                            ctl.Enabled = true;
                        }
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Interface.HideListingTab), false))
                {
                    int idx = tbPOS.TabPages.IndexOf(tabPage3);
                    if (idx != -1)
                    {
                        tbPOS.TabPages.RemoveAt(idx);
                    }
                }
                else
                {
                    int idx = tbPOS.TabPages.IndexOf(tabPage3);
                    if (idx == -1)
                    {
                        tbPOS.TabPages.Add(tabPage3);
                        //tpEdgeworks.Show();
                        foreach (Control ctl in tabPage3.Controls)
                        {
                            ctl.Visible = true;
                            ctl.Enabled = true;
                        }
                    }
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Interface.HideSalesTab), false))
                {
                    int idx = tbPOS.TabPages.IndexOf(tabPage1);
                    if (idx != -1)
                    {
                        tbPOS.TabPages.RemoveAt(idx);
                    }
                }
                else
                {
                    int idx = tbPOS.TabPages.IndexOf(tabPage1);
                    if (idx == -1)
                    {
                        tbPOS.TabPages.Add(tabPage1);
                        //tpEdgeworks.Show();
                        foreach (Control ctl in tabPage1.Controls)
                        {
                            ctl.Visible = true;
                            ctl.Enabled = true;
                        }
                    }
                }

                //special feature for edgeworks
                if (UserInfo.username.ToLower() != "edgeworks")
                {
                    int idx = tbPOS.TabPages.IndexOf(tpEdgeworks);
                    if (idx != -1)
                    {
                        tbPOS.TabPages.RemoveAt(idx);
                    }
                }
                else
                {
                    int idx = tbPOS.TabPages.IndexOf(tpEdgeworks);
                    if (idx == -1)
                    {
                        tbPOS.TabPages.Add(tpEdgeworks);
                        //tpEdgeworks.Show();
                        //flowpanel panel
                        foreach (Control ctl in tpEdgeworks.Controls)
                        {
                            ctl.Visible = true;
                            ctl.Enabled = true;

                            foreach (Control ctlChild in ctl.Controls)
                            {
                                ctlChild.Visible = true;
                                ctlChild.Enabled = true;
                            }
                        }
                    }
                }
                #endregion

                pnlMainScreen.Visible = true;
                //pnlContact.Visible = true;
                pictureBox1.Focus();
                pictureBox1.Select();

                #region Load Culture Code
                string CultureCode =
                      AppSetting.GetSetting(AppSetting.SettingsName.CultureCode);

                if (CultureCode != null)
                {
                    try
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(CultureCode);
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Invalid Culture Code Specified in Setting!" + ex.Message);
                        MessageBox.Show(LanguageManager.Invalid_Culture_Code_Specified_in_Setting_ + ex.Message);

                    }
                }
                #endregion
                //Logger.writeLog("Autocheckoutmemberbefore");

                MembershipAttendanceController.AutoCheckoutMemberBefore(DateTime.Today);

                


                //Logger.writeLog("GetTotalFloatAmount");
                if (CashRecordingController.GetTotalFloatAmount(PointOfSaleInfo.PointOfSaleID) == 0.0M)
                {
                    btnCashRecording_Click(this, new EventArgs());
                }

                // Make sure AttachedParticular table exists.
                MembershipController.CheckAttachedParticularTable();

                //sw.Close();

                #region Init Cash Recycler
                /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.EnableCashRecycler),false))
                {
                    string status = "";
                    cashRecycler = new CashRecycler();
                    cashRecycler.machineType = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.MachineType);
                    cashRecycler.init(out status);
                    cashRecycler.connect(out status);
                }*/
                #endregion
            }
            else
            {
                Application.Exit();
                return;
            }                        
        }

        private void SyncThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //this is the thread for syncing purpose

            //1 check last time stamp
            SyncClientController.Load_WS_URL();
            try
            {
                
                    SyncSalesRealTimeController ssc = new SyncSalesRealTimeController();
                    ssc.OnProgressUpdates += scc_OnProgressUpdates;
                    ssc.SendRealTimeSales();
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void scc_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncSalesStatus = message;
            addLog("Sales",message);
        }

        private void SyncInventoryThread_DoWork(object sender, DoWorkEventArgs e)
        {
            //this is the thread for syncing purpose

            //1 check last time stamp
            //SyncClientController.Load_WS_URL();
            try
            {
                while (true)
                {
                    SyncInventoryRealTimeController sic = new SyncInventoryRealTimeController();
                    sic.OnProgressUpdates += sic_OnProgressUpdates;
                    if (sic.GetRealTimeInventory())
                    {
                        SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                        //SyncClientController.GenerateInventoryHdrForAdjustedSales
                        int RetrySecConnected = 600;
                        if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenConnected), out RetrySecConnected))
                        {
                            RetrySecConnected = 600;
                        }
                        Thread.Sleep(RetrySecConnected * 1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void sic_OnProgressUpdates(object sender, string message)
        {
            if (fSyncLog == null)
            {
                fSyncLog = new frmViewSyncLog();
            }
            fSyncLog.SyncInventoryStatus = message;
            addLog("Inventory",message);
        }

        
        private void AddTabPromotion()
        {
            TabPage tbPromotion = new TabPage();
            tbPromotion.Name = "tpPromotion";

            //tbPromotion.Controls.Add(this.gbPoints);
            //tbPromotion.Controls.Add(this.gbVouchers);
            //tbPromotion.Controls.Add(this.gbPromotion);
            tbPromotion.Name = "tpPromotion";
            tbPromotion.Text = "Promotion";
            tbPromotion.UseVisualStyleBackColor = true;
            
            tbPOS.TabPages.Add(tbPromotion);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to logout?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Logout", "");
            if (dr == DialogResult.Yes)
            {
                lblCashierName.Text = "-";
                UserController.ClearUserInfo();
                PriceDisplay myDisplay = new PriceDisplay();
                myDisplay.CounterClose();
                List<Form> openForms = new List<Form>();

                foreach (Form f in Application.OpenForms)
                    openForms.Add(f);

                foreach (Form f in openForms)
                {
                    if (f.Name != "frmMain")
                        f.Close();
                }
                frmMain_Load(this, new EventArgs());
                
            }
        }

        private frmMembershipSearch FrmMembershipSearch;
        private void btnMembership_Click(object sender, EventArgs e)
        {
            if (FrmMembershipSearch == null || FrmMembershipSearch.IsDisposed)
            {
                FrmMembershipSearch = new frmMembershipSearch();
                FrmMembershipSearch.Show();
            }
            else
            {
                FrmMembershipSearch.WindowState = FormWindowState.Normal;
                FrmMembershipSearch.Activate();
            }
        }


        private void btnCashRecording_Click(object sender, EventArgs e)
        {
            Logger.writeLog("Show Cash Recording Finished");
            frmCashRecording FrmCashRecording = new frmCashRecording(this);
            FrmCashRecording.SyncCashRecordingThread = SyncCashRecordingThread;
            FrmCashRecording.ShowDialog();
            FrmCashRecording.Dispose();

            /*
            if (FrmCashRecording == null || FrmCashRecording.IsDisposed)
            {
                FrmCashRecording = new frmCashRecording();
                FrmCashRecording.Show();
            }
            else
            {
                FrmCashRecording.WindowState = FormWindowState.Normal;
                FrmCashRecording.Activate();                
            }*/
        }

        private frmPOSSetup FrmPOSSetup;
        private void btnChangeLocation_Click(object sender, EventArgs e)
        {
            if (FrmPOSSetup == null || FrmPOSSetup.IsDisposed)
            {
                FrmPOSSetup = new frmPOSSetup();
                FrmPOSSetup.Show();
                lblLocation.Text = PointOfSaleInfo.PointOfSaleName;
            }
            else
            {
                FrmPOSSetup.WindowState = FormWindowState.Normal;
                FrmPOSSetup.Activate();
            }
        }

        PowerInventory.InventoryForms.frmStockSummary FfrmStockSummary;        
        private void btnStockSummaryReport_Click(object sender, EventArgs e)
        {
            if (FfrmStockSummary == null || FfrmStockSummary.IsDisposed)
            {
                FfrmStockSummary = new frmStockSummary();
                FfrmStockSummary.Show();                
            }
            else
            {
                FfrmStockSummary.WindowState = FormWindowState.Normal;
                FfrmStockSummary.Activate();
            }
        }

        private void lblCashierName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            /*
            string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForLogin);
            bool IsAuthorized = false;
            if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
            {
                frmReadMSR f = new frmReadMSR();
                //f.privilegeName = PrivilegesController.;
                f.loginType = LoginType.Login;
                f.ShowDialog();
                IsAuthorized = f.IsAuthorized;
                f.Dispose();
                Application.DoEvents();
            }
            else
            {   
                //prompt login....
                frmPOSLogin f = new frmPOSLogin();
                f.ShowDialog();
                IsAuthorized = !f.isClosing;
                f.Dispose();
            }
            if (IsAuthorized)
            {
                lblCashierName.Text = UserInfo.displayName;
                tRegistration.Text = "Registered to: " + CompanyInfo.CompanyName;
                EnableByPrivilegesButtons();
                PowerPOS.Feature.Package.isAvailable = (System.Configuration.ConfigurationManager.AppSettings["PackageFeature"] != null && System.Configuration.ConfigurationManager.AppSettings["PackageFeature"].ToString().ToUpper() == "YES");
                PowerPOS.Feature.Package.isRealTime = (System.Configuration.ConfigurationManager.AppSettings["RealTimePointSystem"] != null && System.Configuration.ConfigurationManager.AppSettings["RealTimePointSystem"].ToString().ToUpper() == "YES");
                //PowerPOS.Feature.Points.isAvailable = PowerPOS.Feature.Package.isAvailable;
                //PowerPOS.Feature.Points.isRealTime = PowerPOS.Feature.Package.isRealTime;
                pictureBox1.Focus();
                pictureBox1.Select();
                
            }
            */
        }

        private void btnViewNewSignUp_Click(object sender, EventArgs e)
        {

            /*
            frmNewMembersList f = new frmNewMembersList();
            f.ShowDialog();
            f.Dispose();*/
        }

        private frmManageHotKeys FrmManageHotKeys;
        private void btnButtonSetup_Click(object sender, EventArgs e)
        {
            if (FrmManageHotKeys == null || FrmManageHotKeys.IsDisposed)
            {
                FrmManageHotKeys = new frmManageHotKeys();
                FrmManageHotKeys.Show();
            }
            else
            {
                FrmManageHotKeys.WindowState = FormWindowState.Normal;
                FrmManageHotKeys.Activate();
            }
        }

        private frmSearchInvoiceDet FrmSearchInvoiceDet;
        private void btnSearchDetails_Click(object sender, EventArgs e)
        {
            if (FrmSearchInvoiceDet == null || FrmSearchInvoiceDet.IsDisposed)
            {
                FrmSearchInvoiceDet = new frmSearchInvoiceDet();
                FrmSearchInvoiceDet.Show();
            }
            else
            {
                FrmSearchInvoiceDet.WindowState = FormWindowState.Normal;
                FrmSearchInvoiceDet.Activate();
            }
        }

        private Reports.frmProductSalesReport FrmProductSalesReport;
        private void btnSalesByProduct_Click(object sender, EventArgs e)
        {
            if (FrmProductSalesReport == null || FrmProductSalesReport.IsDisposed)
            {
                FrmProductSalesReport = new WinPowerPOS.Reports.frmProductSalesReport();
                FrmProductSalesReport.Show();
            }
            else
            {
                FrmProductSalesReport.WindowState = FormWindowState.Normal;
                FrmProductSalesReport.Activate();
            }
        }

        frmPreOrderSetup FrmPreOrderSetup;
        private void btnPreOrdered_Click(object sender, EventArgs e)
        {
            if (FrmPreOrderSetup == null || FrmPreOrderSetup.IsDisposed)
            {
                FrmPreOrderSetup = new frmPreOrderSetup();
                FrmPreOrderSetup.Show();
            }
            else
            {
                FrmPreOrderSetup.WindowState = FormWindowState.Normal;
                FrmPreOrderSetup.Activate();
            }
        }
        //frmInvReceive FrmInvReceive;
        /*
        private void btnStockReceive_Click(object sender, EventArgs e)
        {
            if (FrmInvReceive == null || FrmInvReceive.IsDisposed)
            {
                FrmInvReceive = new frmInvReceive();
                FrmInvReceive.Show();
            }
                      else
            {
                FrmInvReceive.WindowState = FormWindowState.Normal;
                FrmInvReceive.Activate();                
            }

        }
         */
        /*
        frmInvStockOut FfrmInvStockOut;
        private void btnStockIssue_Click(object sender, EventArgs e)
        {
            if (FfrmInvStockOut == null || FfrmInvStockOut.IsDisposed)
            {
                FfrmInvStockOut = new frmInvStockOut();
                FfrmInvStockOut.Show();
            }
                      else
            {
                FfrmInvStockOut.WindowState = FormWindowState.Normal;
                FfrmInvStockOut.Activate();                
            }
        }
        */
        private void btnStockTake_Click(object sender, EventArgs e)
        {
            /*
            frmStockTake f = new frmStockTake();
            f.ShowDialog();
            f.Dispose();*/
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        PowerInventory.InventoryForms.frmStockOnHandItemSummary FfrmStockOnHand;
        private void btnStockOnHand_Click(object sender, EventArgs e)
        {
            if (FfrmStockOnHand == null || FfrmStockOnHand.IsDisposed)
            {
                FfrmStockOnHand = new PowerInventory.InventoryForms.frmStockOnHandItemSummary();
                FfrmStockOnHand.Show();
            }
            else
            {
                FfrmStockOnHand.WindowState = FormWindowState.Normal;
                FfrmStockOnHand.Activate();
            }
        }

        private void btnImportInventory_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (ExportController.ImportInventoryFromExcel(openFileDialog1.FileName))
            {
                MessageBox.Show("Import successful");
            }
            else
            {
                MessageBox.Show("Import failed. Please check your excel file");
            }
        }

        frmUserMst FfrmUserMst;
        private void btnUserSetup_Click(object sender, EventArgs e)
        {
            if (FfrmUserMst == null || FfrmUserMst.IsDisposed)
            {
                FfrmUserMst = new frmUserMst();
                FfrmUserMst.Show();
            }
            else
            {
                FfrmUserMst.WindowState = FormWindowState.Normal;
                FfrmUserMst.Activate();
            }
        }

        frmStockIn FfrmStockIn;
        private void btnGoodsReceive_Click(object sender, EventArgs e)
        {
            if (FfrmStockIn == null || FfrmStockIn.IsDisposed)
            {
                FfrmStockIn = new frmStockIn();
                FfrmStockIn.Show();
            }
            else
            {
                FfrmStockIn.WindowState = FormWindowState.Normal;
                FfrmStockIn.Activate();
            }
        }
        frmStockOut FfrmStockOut;
        private void btnStockIssue_Click_1(object sender, EventArgs e)
        {
            if (FfrmStockOut == null || FfrmStockOut.IsDisposed)
            {
                FfrmStockOut = new frmStockOut();
                FfrmStockOut.Show();
            }
            else
            {
                FfrmStockOut.WindowState = FormWindowState.Normal;
                FfrmStockOut.Activate();
            }
        }

        PowerInventory.frmStockTake FfrmStockTake;
        private void btnStockTake_Click_1(object sender, EventArgs e)
        {
            if (FfrmStockTake == null || FfrmStockTake.IsDisposed)
            {
                FfrmStockTake = new PowerInventory.frmStockTake();
                FfrmStockTake.Show();
            }
            else
            {
                FfrmStockTake.WindowState = FormWindowState.Normal;
                FfrmStockTake.Activate();
            }
        }
        frmAdjustStock FfrmAdjustStock;
        private void btnStockAdjustment_Click(object sender, EventArgs e)
        {
            if (FfrmAdjustStock == null || FfrmAdjustStock.IsDisposed)
            {
                FfrmAdjustStock = new frmAdjustStock();
                FfrmAdjustStock.Show();
            }
            else
            {
                FfrmAdjustStock.WindowState = FormWindowState.Normal;
                FfrmAdjustStock.Activate();
            }
        }
        frmCategoryList FfrmCategoryList;
        private void btnCategory_Click(object sender, EventArgs e)
        {
            if (FfrmCategoryList == null || FfrmCategoryList.IsDisposed)
            {
                FfrmCategoryList = new frmCategoryList();
                FfrmCategoryList.Show();
            }
            else
            {
                FfrmCategoryList.WindowState = FormWindowState.Normal;
                FfrmCategoryList.Activate();
            }
        }

        PowerInventory.frmStockCardReport FfrmStockCardReport;
        private void btnStockCard_Click(object sender, EventArgs e)
        {
            if (FfrmStockCardReport == null || FfrmStockCardReport.IsDisposed)
            {
                FfrmStockCardReport = new PowerInventory.frmStockCardReport();
                FfrmStockCardReport.Show();
            }
            else
            {
                FfrmStockCardReport.WindowState = FormWindowState.Normal;
                FfrmStockCardReport.Activate();
            }
        }
        frmStockTransfer FfrmStockTransfer;
        private void btnStockTransfer_Click(object sender, EventArgs e)
        {
            if (FfrmStockTransfer == null || FfrmStockTransfer.IsDisposed)
            {
                FfrmStockTransfer = new frmStockTransfer();
                FfrmStockTransfer.Show();
            }
            else
            {
                FfrmStockTransfer.WindowState = FormWindowState.Normal;
                FfrmStockTransfer.Activate();
            }
        }
        frmChangePassword FfrmChangePassword;
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (FfrmChangePassword == null || FfrmChangePassword.IsDisposed)
            {
                FfrmChangePassword = new frmChangePassword();
                FfrmChangePassword.Show();
            }
            else
            {
                FfrmChangePassword.WindowState = FormWindowState.Normal;
                FfrmChangePassword.Activate();
            }
        }

        private void frmMain_Activated(object sender, EventArgs e)
        {
            PriceDisplay myDisplay = new PriceDisplay();
            myDisplay.ClearScreen();
        }
        frmPrivilegeSetup FfrmPrivilegeSetup;
        private void btnPrivilegeSetup_Click(object sender, EventArgs e)
        {
            if (FfrmPrivilegeSetup == null || FfrmPrivilegeSetup.IsDisposed)
            {
                FfrmPrivilegeSetup = new frmPrivilegeSetup();
                FfrmPrivilegeSetup.Show();
            }
            else
            {
                FfrmPrivilegeSetup.WindowState = FormWindowState.Normal;
                FfrmPrivilegeSetup.Activate();
            }
        }
        frmInventoryLocationList FfrmInventoryLocationList;
        private void btnLocationSetup_Click(object sender, EventArgs e)
        {
            if (FfrmInventoryLocationList == null || FfrmInventoryLocationList.IsDisposed)
            {
                FfrmInventoryLocationList = new frmInventoryLocationList();
                FfrmInventoryLocationList.Show();
            }
            else
            {
                FfrmInventoryLocationList.WindowState = FormWindowState.Normal;
                FfrmInventoryLocationList.Activate();
            }
        }
        frmAdjustStockTake FfrmAdjustStockTake;
        private void btnAdjustStockTake_Click(object sender, EventArgs e)
        {
            if (FfrmAdjustStockTake == null || FfrmAdjustStockTake.IsDisposed)
            {
                FfrmAdjustStockTake = new frmAdjustStockTake();
                FfrmAdjustStockTake.Show();
            }
            else
            {
                FfrmAdjustStockTake.WindowState = FormWindowState.Normal;
                FfrmAdjustStockTake.Activate();
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            DialogResult dr = MessageBox.Show("Are you sure you want to exit? All unsaved work will be lost.", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
             */
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Logout", "");
        }
        frmVoucherList FfrmVoucherList;
        private void btnVoucherList_Click(object sender, EventArgs e)
        {

            if (FfrmVoucherList == null || FfrmVoucherList.IsDisposed)
            {
                FfrmVoucherList = new frmVoucherList();
                FfrmVoucherList.Show();
            }
            else
            {
                FfrmVoucherList.WindowState = FormWindowState.Normal;
                FfrmVoucherList.Activate();
            }
        }
        frmGenerateVoucher FfrmGenerateVoucher;
        private void btnGenerateVouchers_Click(object sender, EventArgs e)
        {
            if (FfrmGenerateVoucher == null || FfrmGenerateVoucher.IsDisposed)
            {
                FfrmGenerateVoucher = new frmGenerateVoucher();
                FfrmGenerateVoucher.Show();
            }
            else
            {
                FfrmGenerateVoucher.WindowState = FormWindowState.Normal;
                FfrmGenerateVoucher.Activate();
            }
        }
        
        frmGenerateDiscount FfrmGenerateDiscount;
        private void btnCreatePromoByItem_Click(object sender, EventArgs e)
        {
            if (FfrmGenerateDiscount == null || FfrmGenerateDiscount.IsDisposed)
            {
                FfrmGenerateDiscount = new frmGenerateDiscount();
                FfrmGenerateDiscount.Show();
            }
            else
            {
                FfrmGenerateDiscount.WindowState = FormWindowState.Normal;
                FfrmGenerateDiscount.Activate();
            }
        }
        
        frmGenerateItemGroupPromo FfrmGenerateItemGroupPromo;
        private void btnCreatePromoByGroup_Click(object sender, EventArgs e)
        {
            if (FfrmGenerateItemGroupPromo == null || FfrmGenerateItemGroupPromo.IsDisposed)
            {
                FfrmGenerateItemGroupPromo = new frmGenerateItemGroupPromo();
                FfrmGenerateItemGroupPromo.Show();
            }
            else
            {
                FfrmGenerateItemGroupPromo.WindowState = FormWindowState.Normal;
                FfrmGenerateItemGroupPromo.Activate();
            }
        }
        /*
        frmSearchFamily FfrmSearchFamily;
        private void btnFamilySetup_Click(object sender, EventArgs e)
        {
            if (FfrmSearchFamily == null || FfrmSearchFamily.IsDisposed)
            {
                FfrmSearchFamily = new frmSearchFamily();
                FfrmSearchFamily.Show();
            }
            else
            {
                FfrmSearchFamily.WindowState = FormWindowState.Normal;
                FfrmSearchFamily.Activate();
            }
        }
        */ 
        /*
        frmOutstandingInstallmentReport FfrmOutstandingInstallmentReport;
        private void btnOutstandingInstallment_Click(object sender, EventArgs e)
        {
            if (FfrmOutstandingInstallmentReport == null || FfrmOutstandingInstallmentReport.IsDisposed)
            {
                FfrmOutstandingInstallmentReport = new frmOutstandingInstallmentReport();
                FfrmOutstandingInstallmentReport.Show();
            }
            else
            {
                FfrmOutstandingInstallmentReport.WindowState = FormWindowState.Normal;
                FfrmOutstandingInstallmentReport.Activate();
            }
        }*/



        frmDepositReport FfrmDepositReport;
        private void btnDepositReport_Click(object sender, EventArgs e)
        {
            if (FfrmDepositReport == null || FfrmDepositReport.IsDisposed)
            {
                FfrmDepositReport = new frmDepositReport();
                FfrmDepositReport.Show();
            }
            else
            {
                FfrmDepositReport.WindowState = FormWindowState.Normal;
                FfrmDepositReport.Activate();
            }
        }
        frmDepositActivityReport FfrmDepositActivityReport;
        private void btnDepositActivityReport_Click(object sender, EventArgs e)
        {
            if (FfrmDepositActivityReport == null || FfrmDepositActivityReport.IsDisposed)
            {
                FfrmDepositActivityReport = new frmDepositActivityReport();
                FfrmDepositActivityReport.Show();
            }
            else
            {
                FfrmDepositActivityReport.WindowState = FormWindowState.Normal;
                FfrmDepositActivityReport.Activate();
            }
        }
        frmProductSalesByStylistReport FfrmStylistReport;
        private void btnProductSalesByStylist_Click(object sender, EventArgs e)
        {
            if (FfrmStylistReport == null || FfrmStylistReport.IsDisposed)
            {
                FfrmStylistReport = new frmProductSalesByStylistReport();
                FfrmStylistReport.Show();
            }
            else
            {
                FfrmStylistReport.WindowState = FormWindowState.Normal;
                FfrmStylistReport.Activate();
            }
        }
        frmSalesByStylistReport FfrmSalesByStylistReport;
        private void btnSalesByStylist_Click(object sender, EventArgs e)
        {
            if (FfrmSalesByStylistReport == null || FfrmSalesByStylistReport.IsDisposed)
            {
                FfrmSalesByStylistReport = new frmSalesByStylistReport();
                FfrmSalesByStylistReport.Show();
            }
            else
            {
                FfrmSalesByStylistReport.WindowState = FormWindowState.Normal;
                FfrmSalesByStylistReport.Activate();
            }
        }
        frmDailySumSalesByStylistReport FfrmDailySumSalesByStylistReport;
        private void btnDailySalesByStylist_Click(object sender, EventArgs e)
        {
            if (FfrmDailySumSalesByStylistReport == null || FfrmDailySumSalesByStylistReport.IsDisposed)
            {
                FfrmDailySumSalesByStylistReport = new frmDailySumSalesByStylistReport();
                FfrmDailySumSalesByStylistReport.Show();
            }
            else
            {
                FfrmDailySumSalesByStylistReport.WindowState = FormWindowState.Normal;
                FfrmDailySumSalesByStylistReport.Activate();
            }
        }
        frmRedeemPackage FfrmRedeemPackage;
        private void btnRedeemPackage_Click(object sender, EventArgs e)
        {
            if (FfrmRedeemPackage == null || FfrmRedeemPackage.IsDisposed)
            {
                FfrmRedeemPackage = new frmRedeemPackage();
                FfrmRedeemPackage.Show();
            }
            else
            {
                FfrmRedeemPackage.WindowState = FormWindowState.Normal;
                FfrmRedeemPackage.Activate();
            }
        }

        private void btnCurrencySetup_Click(object sender, EventArgs e)
        {
            //Open Exchange Rate Setup
            frmSetExhangeRate f = new frmSetExhangeRate();
            f.ShowDialog();
            f.Dispose();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            frmSavedFiles f = new frmSavedFiles();
            f.WindowState = FormWindowState.Maximized;
            f.Show();
            /*
            CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
            f.Dispose();*/
        }

        frmViewActivityHeader FfrmViewActivityHeader;
        private void btnViewActivity_Click(object sender, EventArgs e)
        {
            if (FfrmViewActivityHeader == null || FfrmViewActivityHeader.IsDisposed)
            {
                FfrmViewActivityHeader = new frmViewActivityHeader();
                FfrmViewActivityHeader.Show();
            }
            else
            {
                FfrmViewActivityHeader.WindowState = FormWindowState.Maximized;
                FfrmViewActivityHeader.Activate();
            }
        }

        frmViewActivityDetail FfrmViewActivityLog;
        private void btnViewActivityDetail_Click(object sender, EventArgs e)
        {
            if (FfrmViewActivityLog == null || FfrmViewActivityLog.IsDisposed)
            {
                FfrmViewActivityLog = new frmViewActivityDetail();
                FfrmViewActivityLog.Show();
            }
            else
            {
                FfrmViewActivityLog.WindowState = FormWindowState.Maximized;
                FfrmViewActivityLog.Activate();
            }
        }
        frmStockTakeReport FfrmStockTakeReport;
        private void btnStockTakeReport_Click(object sender, EventArgs e)
        {
            if (FfrmStockTakeReport == null || FfrmStockTakeReport.IsDisposed)
            {
                FfrmStockTakeReport = new frmStockTakeReport();
                FfrmStockTakeReport.Show();
            }
            else
            {
                FfrmStockTakeReport.WindowState = FormWindowState.Maximized;
                FfrmStockTakeReport.Activate();
            }

        }

        frmStylistSalesReport FfrmStylistSalesReport;
        private void button1_Click(object sender, EventArgs e)
        {
            if (FfrmStylistSalesReport == null || FfrmStylistSalesReport.IsDisposed)
            {
                FfrmStylistSalesReport = new frmStylistSalesReport();
                FfrmStylistSalesReport.Show();
            }
            else
            {
                FfrmStylistSalesReport.WindowState = FormWindowState.Maximized;
                FfrmStylistSalesReport.Activate();
            }
        }

        private void btnAllocatePoint_Click(object sender, EventArgs e)
        {
            string Status;
            if (!PowerPOS.Feature.Package.AllocatePendingPackage(out Status))
            {
                if (Status.StartsWith("(warning)"))
                {
                    MessageBox.Show(Status.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (Status.StartsWith("(error)"))
                {
                    MessageBox.Show(Status.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(Status);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Done");
            }
        }
        frmMembershipTransactionReport FfrmMembershipTransactionReport;
        private void btnMemberSalesReport_Click(object sender, EventArgs e)
        {
            if (FfrmMembershipTransactionReport == null || FfrmMembershipTransactionReport.IsDisposed)
            {
                FfrmMembershipTransactionReport = new frmMembershipTransactionReport();
                FfrmMembershipTransactionReport.Show();
            }
            else
            {
                FfrmMembershipTransactionReport.WindowState = FormWindowState.Maximized;
                FfrmMembershipTransactionReport.Activate();
            }
        }
        frmAddNewItemWeb FfrmItemMst;
        private void btnProductList_Click(object sender, EventArgs e)
        {
            if (FfrmItemMst == null || FfrmItemMst.IsDisposed)
            {
                FfrmItemMst = new frmAddNewItemWeb();
                FfrmItemMst.Show();
            }
            else
            {
                FfrmItemMst.WindowState = FormWindowState.Maximized;
                FfrmItemMst.Activate();
            }
        }
        //frmCalendarEmbed FfrmCalendarEmbed; 
        //private void btnCalendar_Click(object sender, EventArgs e)
        //{
        //    if (FfrmCalendarEmbed == null || FfrmCalendarEmbed.IsDisposed)
        //    {
        //        FfrmCalendarEmbed = new frmCalendarEmbed();
        //        FfrmCalendarEmbed.Show();
        //    }
        //    else
        //    {
        //        FfrmCalendarEmbed.WindowState = FormWindowState.Maximized;
        //        FfrmCalendarEmbed.Activate();
        //    }
        //}

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.Control && e.Shift && e.KeyCode == Keys.I)
            {
                MessageBox.Show("Ref: " + Application.ProductVersion);
            }
        }
        private void btnMembersAttendance_Click(object sender, EventArgs e)
        {
            FormController.ShowForm(FormController.FormNames.AttendanceReport, new Attendance.frmAttendanceReport());
        }

        private void btnSupport_Click(object sender, EventArgs e)
        {

            frmSupportLogin frmSuppLogin = new frmSupportLogin();

            frmSuppLogin.ShowDialog(this);

            if (frmSuppLogin.IsAuthorized)
            {
                WinPowerPOS.MaintenanceForms.frmSupport frm = new WinPowerPOS.MaintenanceForms.frmSupport();
                frm.ShowDialog();
                frm.Dispose();
            }


            //string tmp = Microsoft.VisualBasic.Interaction.InputBox
            //       ("Please enter support Password!", "", "", 0, 0);
            //if (tmp == "pressingon")
            //{
            //    WinPowerPOS.MaintenanceForms.frmSupport frm = new WinPowerPOS.MaintenanceForms.frmSupport();
            //    frm.ShowDialog();
            //    frm.Dispose();
            //}
        }

        //frmLocalPromoEditor FfrmLocalPromoEditor;
        //private void btnPromo_Click(object sender, EventArgs e)
        //{
        //    if (FfrmLocalPromoEditor == null || FfrmLocalPromoEditor.IsDisposed)
        //    {
        //        FfrmLocalPromoEditor = new frmLocalPromoEditor();
        //        FfrmLocalPromoEditor.Show();
        //    }
        //    else
        //    {
        //        FfrmLocalPromoEditor.WindowState = FormWindowState.Maximized;
        //        FfrmLocalPromoEditor.Activate();
        //    }
        //}
        frmMembershipPointImporter FfrmMembershipPointImporter;
        private void btnImportPoints_Click(object sender, EventArgs e)
        {
            if (FfrmMembershipPointImporter == null || FfrmMembershipPointImporter.IsDisposed)
            {
                FfrmMembershipPointImporter = new frmMembershipPointImporter();
                FfrmMembershipPointImporter.Show();
            }
            else
            {
                FfrmMembershipPointImporter.WindowState = FormWindowState.Maximized;
                FfrmMembershipPointImporter.Activate();
            }
        }

        LineCommissionReport FfrmLineCommission;        
        private void btnSalesPersonPerformance_Click(object sender, EventArgs e)
        {
            if (FfrmLineCommission == null || FfrmLineCommission.IsDisposed)
            {
                FfrmLineCommission = new LineCommissionReport();
                FfrmLineCommission.Show();
            }
            else
            {
                FfrmLineCommission.WindowState = FormWindowState.Maximized;
                FfrmLineCommission.Activate();
            }
        }
        private Edgeworks.frmInfo FfrmInfo;
        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (FfrmInfo == null || FfrmInfo.IsDisposed)
            {
                FfrmInfo = new WinPowerPOS.Edgeworks.frmInfo();
                FfrmInfo.Show();
            }
            else
            {

                FfrmInfo.WindowState = FormWindowState.Maximized;
                FfrmInfo.Activate();
            }
        }

        private void btnFeatures_Click(object sender, EventArgs e)
        {
            FormController.ShowForm(FormController.FormNames.FeatureSetup, new Edgeworks.frmFeatureSetup());
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            FormController.ShowForm(FormController.FormNames.OutstandingInstallmentReport, new frmOutstandingInstallmentReport());
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            btnDailySalesByStylist_Click(sender,e);
        }

        private void btnExtraChargeSetup_Click(object sender, EventArgs e)
        {
            FormController.ShowForm(FormController.FormNames.ExtraChargeSetup, new frmExtraChargeSetup());
        }

        frmUpdatePaymenttype fPaymentType; 
        private void btnPaymentTypesSetup_Click(object sender, EventArgs e)
        {
            if (fPaymentType == null || fPaymentType.IsDisposed)
            {
                fPaymentType = new frmUpdatePaymenttype();
                fPaymentType.Show();
            }
            else
            {
                fPaymentType.WindowState = FormWindowState.Maximized;
                fPaymentType.Activate();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string TVPath = ConfigurationManager.AppSettings["TeamViewerPath"];
                System.Diagnostics.Process.Start(TVPath);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Cannot Launch TeamViewer. " + ex.ToString());
            }
        }

        private void tbPOS_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                btnUpload.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false);
            }
            
        }

        frmUploadItemPhotos uploadPhotos;
        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (uploadPhotos == null || uploadPhotos.IsDisposed)
            {
                uploadPhotos = new frmUploadItemPhotos();
            }
            uploadPhotos.Show();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            frmFeedback frm = new frmFeedback();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        PowerInventory.frmPurchaseOrderNew fPurchaseOrder;
        private void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            if (fPurchaseOrder == null || fPurchaseOrder.IsDisposed)
            {
                fPurchaseOrder = new PowerInventory.frmPurchaseOrderNew();
                fPurchaseOrder.Show();
            }
            else
            {
                fPurchaseOrder.WindowState = FormWindowState.Maximized;
                fPurchaseOrder.Activate();
            }
        }

        frmViewPurchaseOrderHeader fViewPurchaseOrderHeader;
        private void btnViewPurchaseOrder_Click(object sender, EventArgs e)
        {
            if (fViewPurchaseOrderHeader == null || fViewPurchaseOrderHeader.IsDisposed)
            {
                fViewPurchaseOrderHeader = new frmViewPurchaseOrderHeader();
                fViewPurchaseOrderHeader.Show();
            }
            else
            {
                fViewPurchaseOrderHeader.WindowState = FormWindowState.Maximized;
                fViewPurchaseOrderHeader.Activate();
            }
        }

        frmMagentoSync fMagentoSync;
        private void btnMagentoSync_Click(object sender, EventArgs e)
        {
            if (fMagentoSync == null || fMagentoSync.IsDisposed)
            {
                fMagentoSync = new frmMagentoSync();
                fMagentoSync.Show();
            }
            else
            {
                fMagentoSync.WindowState = FormWindowState.Maximized;
                fMagentoSync.Activate();
            }
        }

        frmPromotionList fPromotionList;
        private void btnPromo_Click(object sender, EventArgs e)
        {
            if (fPromotionList == null || fPromotionList.IsDisposed)
            {
                fPromotionList = new frmPromotionList();
                fPromotionList.WindowState = FormWindowState.Maximized;
                fPromotionList.Show();
            }
            else
            {
                fPromotionList.WindowState = FormWindowState.Maximized;
                fPromotionList.Activate();
            }
        }
        
        #region "tab promotions for admin"
        frmListOfDiscountByItem FfrmListOfDiscountByItem;
        private void btnPromoByItemList_Click(object sender, EventArgs e)
        {
            if (FfrmListOfDiscountByItem == null || FfrmListOfDiscountByItem.IsDisposed)
            {
                FfrmListOfDiscountByItem = new frmListOfDiscountByItem();
                FfrmListOfDiscountByItem.Show();
            }
            else
            {
                FfrmListOfDiscountByItem.WindowState = FormWindowState.Normal;
                FfrmListOfDiscountByItem.Activate();
            }
        }

        frmListOfDiscountByItemGroup FfrmListOfDiscountByItemGroup;
        private void btnPromoByGroupList_Click(object sender, EventArgs e)
        {
            if (FfrmListOfDiscountByItemGroup == null || FfrmListOfDiscountByItemGroup.IsDisposed)
            {
                FfrmListOfDiscountByItemGroup = new frmListOfDiscountByItemGroup();
                FfrmListOfDiscountByItemGroup.Show();
            }
            else
            {
                FfrmListOfDiscountByItemGroup.WindowState = FormWindowState.Normal;
                FfrmListOfDiscountByItemGroup.Activate();
            }
        }

        private void btnVouchers_Click(object sender, EventArgs e)
        {
            if (FfrmVoucherList == null || FfrmVoucherList.IsDisposed)
            {
                FfrmVoucherList = new frmVoucherList();
                FfrmVoucherList.Show();
            }
            else
            {
                FfrmVoucherList.WindowState = FormWindowState.Normal;
                FfrmVoucherList.Activate();
            }
        }

        private void btnCreateVoucher_Click(object sender, EventArgs e)
        {
            if (FfrmGenerateVoucher == null || FfrmGenerateVoucher.IsDisposed)
            {
                FfrmGenerateVoucher = new frmGenerateVoucher();
                FfrmGenerateVoucher.Show();
            }
            else
            {
                FfrmGenerateVoucher.WindowState = FormWindowState.Normal;
                FfrmGenerateVoucher.Activate();
            }
        }

        private void btnImportPointsAdmin_Click(object sender, EventArgs e)
        {
            if (FfrmMembershipPointImporter == null || FfrmMembershipPointImporter.IsDisposed)
            {
                FfrmMembershipPointImporter = new frmMembershipPointImporter();
                FfrmMembershipPointImporter.Show();
            }
            else
            {
                FfrmMembershipPointImporter.WindowState = FormWindowState.Maximized;
                FfrmMembershipPointImporter.Activate();
            }
        }

        #endregion

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        
        private void btnViewSyncLog_Click(object sender, EventArgs e)
        {
            if (fSyncLog == null || fSyncLog.IsDisposed)
            {
                fSyncLog = new frmViewSyncLog();
                fSyncLog.WindowState = FormWindowState.Maximized;
                fSyncLog.Show();
            }
            else
            {
                fSyncLog.WindowState = FormWindowState.Maximized;
                fSyncLog.Show();
                fSyncLog.Activate();
            }        
        }

        private void addLog(string type, string msg)
        {

            msg = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss - ") + msg;
            if (type.ToLower() == "sales")
            {
                syncSalesHistory.Insert(0, msg);
                if (syncSalesHistory.Count > maxHistory)
                {
                    syncSalesHistory.RemoveRange(syncSalesHistory.Count - 1, syncSalesHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddSalesLog(string.Join(System.Environment.NewLine, syncSalesHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "quotation")
            {
                syncQuotationHistory.Insert(0, msg);
                if (syncQuotationHistory.Count > maxHistory)
                    syncQuotationHistory.RemoveRange(syncQuotationHistory.Count - 1, syncQuotationHistory.Count - maxHistory);

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                    fSyncLog.AddQuotationLog(string.Join(System.Environment.NewLine, syncQuotationHistory.ToArray()));
            }
            else if (type.ToLower() == "cashrecording")
            {
                syncCashRecordingHistory.Insert(0, msg);
                if (syncCashRecordingHistory.Count > maxHistory)
                    syncCashRecordingHistory.RemoveRange(syncCashRecordingHistory.Count - 1, syncCashRecordingHistory.Count - maxHistory);

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                    fSyncLog.AddCashRecordingLog(string.Join(System.Environment.NewLine, syncCashRecordingHistory.ToArray()));
            }
            else if (type.ToLower() == "inventory")
            {
                syncInventoryHistory.Insert(0, msg);
                if (syncInventoryHistory.Count > maxHistory)
                {
                    syncInventoryHistory.RemoveRange(syncInventoryHistory.Count - 1, syncInventoryHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddInventoryLog(string.Join(System.Environment.NewLine, syncInventoryHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "getinventory")
            {
                getInventoryHistory.Insert(0, msg);
                if (getInventoryHistory.Count > maxHistory)
                {
                    getInventoryHistory.RemoveRange(getInventoryHistory.Count - 1, getInventoryHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.addGetInventoryLog(string.Join(System.Environment.NewLine, getInventoryHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "membership")
            {
                syncMemberHistory.Insert(0, msg);
                if (syncMemberHistory.Count > maxHistory)
                    syncMemberHistory.RemoveRange(syncMemberHistory.Count - 1, syncMemberHistory.Count - maxHistory);

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                    fSyncLog.AddMemberLog(string.Join(System.Environment.NewLine, syncMemberHistory.ToArray()));
            }
            else if (type.ToLower() == "logs")
            {
                syncLogsHistory.Insert(0, msg);
                if (syncLogsHistory.Count > maxHistory)
                {
                    syncLogsHistory.RemoveRange(syncLogsHistory.Count - 1, syncLogsHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.addSyncLogsLog(string.Join(System.Environment.NewLine, syncLogsHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "accesslog")
            {
                syncAccessLogsHistory.Insert(0, msg);
                if (syncAccessLogsHistory.Count > maxHistory)
                {
                    syncAccessLogsHistory.RemoveRange(syncAccessLogsHistory.Count - 1, syncAccessLogsHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.addSyncAccessLogsLog(string.Join(System.Environment.NewLine, syncAccessLogsHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "product")
            {
                syncProductHistory.Insert(0, msg);
                if (syncProductHistory.Count > maxHistory)
                {
                    syncProductHistory.RemoveRange(syncProductHistory.Count - 1, syncProductHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddProductLog(string.Join(System.Environment.NewLine, syncProductHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "masterdata")
            {
                syncMasterDataHistory.Insert(0, msg);
                if (syncMasterDataHistory.Count > maxHistory)
                {
                    syncMasterDataHistory.RemoveRange(syncMasterDataHistory.Count - 1, syncMasterDataHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddMasterDataLog(string.Join(System.Environment.NewLine, syncMasterDataHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "user")
            {
                syncUserHistory.Insert(0, msg);
                if (syncUserHistory.Count > maxHistory)
                {
                    syncUserHistory.RemoveRange(syncUserHistory.Count - 1, syncUserHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddUserLog(string.Join(System.Environment.NewLine, syncUserHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "appointment")
            {
                syncAppointmentHistory.Insert(0, msg);
                if (syncAppointmentHistory.Count > maxHistory)
                {
                    syncAppointmentHistory.RemoveRange(syncAppointmentHistory.Count - 1, syncAppointmentHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddAppointmentLog(string.Join(System.Environment.NewLine, syncAppointmentHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "performancelog")
            {
                syncPerformanceLogHistory.Insert(0, msg);
                if (syncPerformanceLogHistory.Count > maxHistory)
                    syncPerformanceLogHistory.RemoveRange(syncPerformanceLogHistory.Count - 1, syncPerformanceLogHistory.Count - maxHistory);

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                    fSyncLog.addSyncPerformanceLogLog(string.Join(System.Environment.NewLine, syncPerformanceLogHistory.ToArray()));
            }
            else if (type.ToLower() == "deliveryorder")
            {
                syncDeliveryOrderHistory.Insert(0, msg);
                if (syncDeliveryOrderHistory.Count > maxHistory)
                {
                    syncDeliveryOrderHistory.RemoveRange(syncDeliveryOrderHistory.Count - 1, syncDeliveryOrderHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddDeliveryOrderLog(string.Join(System.Environment.NewLine, syncDeliveryOrderHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "rating")
            {
                syncRatingHistory.Insert(0, msg);
                if (syncRatingHistory.Count > maxHistory)
                {
                    syncRatingHistory.RemoveRange(syncRatingHistory.Count - 1, syncRatingHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddSyncRatingLog(string.Join(System.Environment.NewLine, syncRatingHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "attendance")
            {
                syncAttendanceHistory.Insert(0, msg);
                if (syncAttendanceHistory.Count > maxHistory)
                {
                    syncAttendanceHistory.RemoveRange(syncAttendanceHistory.Count - 1, syncAttendanceHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddSyncAttendanceLog(string.Join(System.Environment.NewLine, syncAttendanceHistory.ToArray()));
                }
            }
            else if (type.ToLower() == "voucher")
            {
                syncVoucherHistory.Insert(0, msg);
                if (syncVoucherHistory.Count > maxHistory)
                {
                    syncVoucherHistory.RemoveRange(syncVoucherHistory.Count - 1, syncVoucherHistory.Count - maxHistory);
                }

                if (fSyncLog != null && !fSyncLog.IsDisposed)
                {
                    fSyncLog.AddVoucherLog(string.Join(System.Environment.NewLine, syncVoucherHistory.ToArray()));
                }
            }
        }

        private frmDeliverySetup FrmDeliveryOrder;
        private void button2_Click(object sender, EventArgs e)
        {
            if (FrmDeliveryOrder == null || FrmDeliveryOrder.IsDisposed)
            {
                FrmDeliveryOrder = new frmDeliverySetup();
                FrmDeliveryOrder.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                FrmDeliveryOrder.Show();
            }
            else
            {
                if (!FrmDeliveryOrder.Visible) FrmDeliveryOrder.Show();
                FrmDeliveryOrder.WindowState = FormWindowState.Maximized;
                FrmDeliveryOrder.Activate();
            }
        }

        private frmDeliveryList FrmDeliveryList;
        private void btnViewDelivery_Click(object sender, EventArgs e)
        {
            if (FrmDeliveryList == null || FrmDeliveryList.IsDisposed)
            {
                FrmDeliveryList = new frmDeliveryList();
                FrmDeliveryList.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                FrmDeliveryList.Show();
            }
            else
            {
                if (!FrmDeliveryList.Visible) FrmDeliveryList.Show();
                FrmDeliveryList.WindowState = FormWindowState.Maximized;
                FrmDeliveryList.Activate();
            }
        }

        private void btnFreeze_Click(object sender, EventArgs e)
        {
            //prompt passowrd
            frmSupervisorLogin f = new frmSupervisorLogin();
            f.privilegeName = PrivilegesController.GIVE_DISCOUNT;
            f.ShowDialog();
            if (!f.IsAuthorized)
            {
                f.Dispose();
                return;
            }
            else
            {
                f.Dispose();
                //freezepos

                if (!SyncClientController.FreezePOSByPointOfSaleID(PointOfSaleInfo.PointOfSaleID))
                {
                    MessageBox.Show("Error when freeze POS. Please contact your Administrator");
                }

                //logout
                lblCashierName.Text = "-";
                UserController.ClearUserInfo();
                PriceDisplay myDisplay = new PriceDisplay();
                myDisplay.CounterClose();
                frmMain_Load(this, new EventArgs());

            }
        }

        private frmZ2Closing FrmZ2Closing;
        private void btnZ2Closing_Click(object sender, EventArgs e)
        {
            if (FrmZ2Closing == null || FrmZ2Closing.IsDisposed)
            {
                FrmZ2Closing = new frmZ2Closing();
                FrmZ2Closing.Show();
            }
            else
            {
                if (!FrmZ2Closing.Visible) FrmZ2Closing.Show();
                FrmZ2Closing.WindowState = FormWindowState.Maximized;
                FrmZ2Closing.Activate();
            }
        }

        private frmOrderTaking FrmFormalInvoice;
        private void btnFormalInvoice_Click(object sender, EventArgs e)
        {
            if (FrmFormalInvoice == null || FrmFormalInvoice.IsDisposed)
            {
                FrmFormalInvoice = new frmOrderTaking();
                FrmFormalInvoice.SyncSalesThread = SyncSalesThread;
                FrmFormalInvoice.SyncAttendanceThread = SyncAttendanceThread;
                FrmFormalInvoice.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                FrmFormalInvoice.SyncCashRecordingThread = SyncCashRecordingThread;
                FrmFormalInvoice.mode = "formal";
                //FrmFormalInvoice.cashRecycler = cashRecycler;
                FrmFormalInvoice.Show();
                DisableButtons();
                EnableByPrivilegesButtons();

                PowerPOS.Feature.Package.isAvailable = (AppSetting.GetSettingFromDBAndConfigFile("PackageFeature") != null && AppSetting.GetSettingFromDBAndConfigFile("PackageFeature").ToString().ToUpper() == "YES");
                PowerPOS.Feature.Package.isRealTime = (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem") != null && AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem").ToString().ToUpper() == "YES");

                lblCashierName.Text = UserInfo.displayName; //just in case cashier change
                tRegistration.Text = "Registered to: " + CompanyInfo.CompanyName;
            }
            else
            {
                if (!FrmFormalInvoice.Visible) FrmFormalInvoice.Show();
                FrmFormalInvoice.WindowState = FormWindowState.Maximized;
                FrmFormalInvoice.Activate();
            }
        }

        //private frmOrderTaking FrmCreditInvoice;
        private void btnCreditInvoice_Click(object sender, EventArgs e)
        {
            //if (FrmCreditInvoice == null || FrmCreditInvoice.IsDisposed)
            //{
            //    FrmCreditInvoice = new frmOrderTaking();
            //    FrmCreditInvoice.SyncSalesThread = SyncSalesThread;
            //    FrmCreditInvoice.mode = "credit";
            //    FrmCreditInvoice.Show();
            //    DisableButtons();
            //    EnableByPrivilegesButtons();

            //    PowerPOS.Feature.Package.isAvailable = (AppSetting.GetSettingFromDBAndConfigFile("PackageFeature") != null && AppSetting.GetSettingFromDBAndConfigFile("PackageFeature").ToString().ToUpper() == "YES");
            //    PowerPOS.Feature.Package.isRealTime = (AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem") != null && AppSetting.GetSettingFromDBAndConfigFile("RealTimePointSystem").ToString().ToUpper() == "YES");

            //    lblCashierName.Text = UserInfo.displayName; //just in case cashier change
            //    tRegistration.Text = "Registered to: " + CompanyInfo.CompanyName;
            //}
            //else
            //{
            //    if (!FrmCreditInvoice.Visible) FrmCreditInvoice.Show();
            //    FrmCreditInvoice.WindowState = FormWindowState.Maximized;
            //    FrmCreditInvoice.Activate();
            //}
        }
        private frmOrderQuotation fpo;
        private void btnQuotation_Click(object sender, EventArgs e)
        {
            if (fpo == null || fpo.IsDisposed)
            {
                fpo = new frmOrderQuotation();
                fpo.SyncQuotationThread = SyncQuotationThread;
                fpo.Show();
            }
            else
            {
                if (!fpo.Visible) fpo.Show();
                fpo.WindowState = FormWindowState.Maximized;
                fpo.Activate();
            }
        }

        private frmQuotationList fqo;
        private void btnQuotationList_Click(object sender, EventArgs e)
        {
            if (fqo == null || fqo.IsDisposed)
            {
                fqo = new frmQuotationList();
                fqo.SyncQuotationThread = SyncQuotationThread;
                fqo.Show();
            }
            else
            {
                if (!fqo.Visible) fqo.Show();
                fqo.WindowState = FormWindowState.Maximized;
                fqo.Activate();
            }
        }

        private frmAppointmentManager2 frmAppointment;
        private frmAppointmentManagerWeekly frmAppointmentWeekly;
        private void btnAppointmentManager_Click(object sender, EventArgs e)
        {
            bool isUseWeeklyView = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseWeeklyView), false);
            if (isUseWeeklyView)
            {
                if (frmAppointmentWeekly == null || frmAppointmentWeekly.IsDisposed)
                    frmAppointmentWeekly = new frmAppointmentManagerWeekly();
                frmAppointmentWeekly.SyncSalesThread = SyncSalesThread;
                frmAppointmentWeekly.SyncSendAppointmentThread = SyncSendAppointmentThread;
                frmAppointmentWeekly.ShowDialog();
            }
            else
            {
                if (fSyncLog == null || fSyncLog.IsDisposed)
                    fSyncLog = new frmViewSyncLog();

                if (frmAppointment == null || frmAppointment.IsDisposed)
                    frmAppointment = new frmAppointmentManager2(fSyncLog);
                frmAppointment.SyncSalesThread = SyncSalesThread;
                frmAppointment.SyncAppointmentThread = SyncAppointmentThread;
                frmAppointment.SyncSendAppointmentThread = SyncSendAppointmentThread;
                frmAppointment.ShowDialog();
            }
        }

        private frmAppointmentManagerMonthly frmAppointmentMonthly;
        private void btnMonthlyAppointment_Click(object sender, EventArgs e)
        {
            if (frmAppointmentMonthly == null || frmAppointmentMonthly.IsDisposed)
                frmAppointmentMonthly = new frmAppointmentManagerMonthly();

            if (frmAppointmentWeekly == null || frmAppointmentWeekly.IsDisposed)
                frmAppointmentWeekly = new frmAppointmentManagerWeekly();

            frmAppointmentMonthly.frmAppointmentWeekly = frmAppointmentWeekly;
            frmAppointmentMonthly.SyncSalesThread = SyncSalesThread;
            frmAppointmentMonthly.ShowDialog();
        }

        private frmRoomStatus frmRoomStatus;
        private void btnRoomStatus_Click(object sender, EventArgs e)
        {
            if (fSyncLog == null || fSyncLog.IsDisposed)
                fSyncLog = new frmViewSyncLog();

            if (frmRoomStatus == null || frmRoomStatus.IsDisposed)
            {
                frmRoomStatus = new frmRoomStatus();
                frmRoomStatus.SyncAppointmentThread = SyncAppointmentThread;

            }

            frmRoomStatus.ShowDialog();
        }

        private frmGuestbook frmGuestbook;
        private void btnMembersGuestbook_Click(object sender, EventArgs e)
        {
            if (frmGuestbook == null || frmGuestbook.IsDisposed)
                frmGuestbook = new frmGuestbook();

            frmGuestbook.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private frmPrint frmBarcodePrinter;
        private void btnBarcodePrinter_Click(object sender, EventArgs e)
        {
            if (frmBarcodePrinter == null || frmBarcodePrinter.IsDisposed)
                frmBarcodePrinter = new frmPrint();

            frmBarcodePrinter.ShowDialog();
        }

        frmStockOutReturn FfrmStockReturn;
        private void btnStockReturn_Click(object sender, EventArgs e)
        {
            if (FfrmStockReturn == null || FfrmStockReturn.IsDisposed)
            {
                FfrmStockReturn = new frmStockOutReturn();
                FfrmStockReturn.Show();
            }
            else
            {
                FfrmStockReturn.WindowState = FormWindowState.Normal;
                FfrmStockReturn.Activate();
            }
        }
    }
}