using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using SubSonic;
using PowerPOS.SyncLogsController;
using PowerPOS.SalesController;


namespace WinPowerPOS
{
    public partial class frmSync : Form
    {
        CultureInfo currentCulture;

        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncInventoryThread;
        public BackgroundWorker SyncLogsThread;
        public BackgroundWorker SyncMasterDataThread;
        public BackgroundWorker SyncUserThread;
        public BackgroundWorker SyncProductThread;
        public frmSync()
        {
            InitializeComponent();
            //SyncClientController.Load_WS_URL();
            lblSendTo.Text = SyncClientController.WS_URL;
            CommonUILib.FormatDateFilter(ref dtpStartDateSales, ref dtpEndDateSales);
            CommonUILib.FormatDateFilter(ref dtpStartDateLogs, ref dtpEndDateLogs);
            CommonUILib.FormatDateFilter(ref dtpStartDateSyncAll, ref dtpEndDateSyncAll);

            currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            CultureInfo ct = new CultureInfo("");
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy-MM-dd";
            ct.DateTimeFormat = dtFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture = ct;
            System.Threading.Thread.CurrentThread.CurrentUICulture = ct;

            string tmp = AppSetting.GetSetting("ProjectModule");
            if (tmp == null)
            {
                AppSetting.SetSetting("ProjectModule", "no");
            }
            if (AppSetting.GetSetting(AppSetting.SettingsName.Project.ProjectModule).ToString().ToLower() == "false")
            {
                cbProject.Visible = false;
                cbProject.Checked = false;
            }
            else
            {
                cbProject.Visible = true;
                cbProject.Checked = true;
            }
            
        }

        private void btnSyncSales_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Sync - Sales", "");
            ShowPanel();

            
            // Delete AppSetting
/*            if (AppSetting.GetSetting("IsRealTimeSales") == "1")
            {
                txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Client synchronization started" + "\r\n" + txtMsg.Text;
                txtMsg.Refresh();
                PointOfSaleController.GetPointOfSaleInfo();
                string serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_OrderLastSyncVersion";
                //SyncClientController.DeleteAppSetting(serverAppSettingKey);
                if (!ClientSyncExecution())
                {
                    MessageBox.Show("Client Synchronization Failed.");
                }
                else
                {
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Client synchronization success" + "\r\n" + txtMsg.Text;
                    txtMsg.Refresh();
                }
            }
            else
            {*/
            //    SendData(dtpStartDateSales.Value, dtpEndDateSales.Value);
           // }
            SyncSalesRealTimeController sync = new SyncSalesRealTimeController();
            bool result = sync.SendRealTimeSales();

            // Send Delivery Order data if using "Cash & Carry / Pre-Order" feature
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
            {
                DateTime StartDate = dtpStartDateSales.Value.Date;
                DateTime EndDate = dtpEndDateSales.Value.Date.AddDays(1).AddSeconds(-1);
                result &= SyncClientController.SendDeliveryOrderToServer(StartDate, EndDate, true);
            }

            if (result)
            {
                MessageBox.Show("Sync Successful");
            }
            else
            {
                MessageBox.Show("Sync Failed. Please check sync logs");
            }

            HidePanel();
        }

        private void SendData(DateTime StartDate, DateTime EndDate)
        {
            bool result = false;
            bool TotalResult = true;

            //Send 
            TotalResult &= SyncClientController.SendLogsToServer
                (StartDate, EndDate.AddSeconds(5));

            while (DateTime.Compare(StartDate, EndDate) <= 0)
            {
                int count = 0;
                txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Sending from " +
                            StartDate.ToString("dd/MM/yy HH:mm") +
                            " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm")
                             + "\r\n" + txtMsg.Text;
                txtMsg.Refresh();
                result = SyncClientController.
                     SendOrderCCMW(StartDate, StartDate.AddHours(8));
                TotalResult &= result;

                // Send Delivery Order data if using "Cash & Carry / Pre-Order" feature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
                {
                    result = SyncClientController.SendDeliveryOrderToServer(StartDate, StartDate.AddHours(8), false);
                    TotalResult &= result;
                }

                if (AppSetting.GetSetting("SyncLocalInventoryToServer") == "yes")
                {
                    result = SyncClientController.SendInventory(StartDate, StartDate.AddHours(8));
                    TotalResult &= result;
                }            
                Logger.writeLog("Sending from " +
                StartDate.ToString("dd/MM/yy HH:mm:ss") +
                " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm:ss")
                + " POS ID " + PointOfSaleInfo.PointOfSaleID + " completed");

                if (result)
                {
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Success sending from " +
                        StartDate.ToString("dd/MM/yy HH:mm") +
                        " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm") + "\r\n" + txtMsg.Text; ;
                }
                else
                {
                    
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Failed sending from " +
                        StartDate.ToString("dd/MM/yy HH:mm") +
                        " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm") + "\r\n" + txtMsg.Text; ;

                    //log failed sending by checking result
                    Logger.writeLog("failed sending from " +
                        StartDate.ToString("dd/MM/yy HH:mm:ss") +
                        " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm:ss")
                        + " POS ID " + PointOfSaleInfo.PointOfSaleID, true);
                }
                txtMsg.Refresh();

                StartDate = StartDate.AddHours(8);
            }
            //SyncClientController.deductInventory();
      
            if (TotalResult)
            {
                MessageBox.Show("Sending Successful");
            }
            else
            {
                MessageBox.Show("Sending FAILED!!");
            }
            txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Sending Completed" + "\r\n" + txtMsg.Text;
            
            return;
        }
        private void btnGetRecentlyModified_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Sync - Get Recently Modified", "");
            #region Create Required Tables
            LineInfoController.CreateLineInfoTable();
            PriceSchemeController.CreatePriceSchemeTable();
            #endregion 

            ShowPanel();

            txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Fetch recently modified information from server" + "\r\n" + txtMsg.Text;
            txtMsg.Refresh();
           bool result = SyncClientController.
                GetBasicInfoFromServer
                (cbPointOfSales.Checked, cbUserAccounts.Checked, 
                cbMemberships.Checked, cbPromotions.Checked, cbProducts.Checked, cbHotkeys.Checked,cbLineInfo.Checked,cbProject.Checked, false);

           // Get Delivery Order data if using "Cash & Carry / Pre-Order" feature
           if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
           {
               result &= SyncClientController.GetDeliveryOrderFromServer(DateTime.Today.AddDays(-7), DateTime.Now);
           }

           if (result)
           {
               MessageBox.Show("Downloading data successful");
               txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Downloading successful" + "\r\n" + txtMsg.Text;
               txtMsg.Refresh();

               if (AppSetting.GetSetting("IsRealTimeSales") == "1")
               {
                   // Delete AppSetting
                   txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Client synchronization started" + "\r\n" + txtMsg.Text;
                   txtMsg.Refresh();
                   PointOfSaleController.GetPointOfSaleInfo();
                   string serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_LogsLastSyncVersion";
                   SyncClientController.DeleteAppSetting(serverAppSettingKey);
                   serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_OrderLastSyncVersion";
                   SyncClientController.DeleteAppSetting(serverAppSettingKey);
                   if (!ClientSyncExecution())
                   {
                       MessageBox.Show("Client Synchronization Failed.");
                   }
                   else
                   {
                       txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Client synchronization success" + "\r\n" + txtMsg.Text;
                       txtMsg.Refresh();
                   }
               }

               if (cbProducts.Checked)
               {
                   ExportProductsToFile();
               }
           }
           else
           {
               MessageBox.Show("Downloading data failed. Please check your internet connection.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
               txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Sending failed" + "\r\n" + txtMsg.Text;
               txtMsg.Refresh();
           }
           HidePanel();
        }
        private void btnGetAll_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Sync - Get All", "");
            #region Create Required Tables
            LineInfoController.CreateLineInfoTable();
            PriceSchemeController.CreatePriceSchemeTable();
            #endregion 
            ShowPanel();
            /*txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Fetch all information from server" + "\r\n" + txtMsg.Text;
            txtMsg.Refresh();
            bool result = SyncClientController.
                           GetBasicInfoFromServer
                           (cbPointOfSales.Checked, cbUserAccounts.Checked,
                            cbMemberships.Checked, cbPromotions.Checked, 
                            cbProducts.Checked, cbHotkeys.Checked,cbLineInfo.Checked, cbProject.Checked, true);

            */

            #region *) Try To Sync to server if is enabled
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMasterData), false))
                if (!SyncMasterDataThread.IsBusy)
                    SyncMasterDataThread.RunWorkerAsync();
            #endregion

            #region *) Try To Sync to server if is enabled
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMasterData), false))
                if (!SyncMasterDataThread.IsBusy)
                    SyncMasterDataThread.RunWorkerAsync();
            
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncProducts), false))
                if (!SyncProductThread.IsBusy)
                    SyncProductThread.RunWorkerAsync();

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncUser), false))
                if (!SyncUserThread.IsBusy)
                    SyncUserThread.RunWorkerAsync();
            #endregion

            txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Fetch all information from server" + "\r\n" + txtMsg.Text;
            txtMsg.Refresh();
            bool result = SyncClientController.
                           GetBasicInfoFromServer
                           (false, false,
                            true, false, 
                            false, false,false, false, true);

            // Get Delivery Order data if using "Cash & Carry / Pre-Order" feature
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
            {
                result &= SyncClientController.GetDeliveryOrderFromServer(DateTime.Today.AddDays(-90), DateTime.Now);
            }

            

            //bool result = true;
            if (result)
            {
                MessageBox.Show("Downloading successful");
                txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Downloading successful" + "\r\n" + txtMsg.Text;
                txtMsg.Refresh();

                /*if (AppSetting.GetSetting("IsRealTimeSales") == "1")
                {
                    // Delete AppSetting Order & Logs, ClientSync
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Client synchronization started" + "\r\n" + txtMsg.Text;
                    txtMsg.Refresh();
                    PointOfSaleController.GetPointOfSaleInfo();
                    string serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_LogsLastSyncVersion";
                    SyncClientController.DeleteAppSetting(serverAppSettingKey);
                    serverAppSettingKey = PointOfSaleInfo.PointOfSaleID + "_OrderLastSyncVersion";
                    SyncClientController.DeleteAppSetting(serverAppSettingKey);
                    if (!ClientSyncExecution())
                    {
                        MessageBox.Show("Client Synchronization Failed.");
                    }
                    else
                    {
                        txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Client synchronization success" + "\r\n" + txtMsg.Text;
                        txtMsg.Refresh();
                    }
                }*/

                if (cbProducts.Checked)
                {
                    ExportProductsToFile();
                }
            }
            else
            {
                MessageBox.Show("Sending failed. Please check your internet connection.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Downloading failed" + "\r\n" + txtMsg.Text;
                txtMsg.Refresh();
            }
            HidePanel();
        }

        private void btnSyncLogs_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Sync - Logs", "");
            ShowPanel();
            txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Send reports to server" + "\r\n" + txtMsg.Text;
            txtMsg.Refresh();
            
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncLogs), false))
            {
                SyncLogsRealTimeController sync = new SyncLogsRealTimeController();
                bool result = sync.SendLogs();
                if (result)
                {
                    MessageBox.Show("Sending Logs successful");
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Sending successful" + "\r\n" + txtMsg.Text;
                    txtMsg.Refresh();

                }
                else
                {
                    MessageBox.Show("Sending failed. Please check your internet connection.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Sending failed" + "\r\n" + txtMsg.Text;
                    txtMsg.Refresh();
                }
            }
            //}
            HidePanel();
        }

        private void btnClearMsg_Click(object sender, EventArgs e)
        {
            txtMsg.Text = "";
            txtMsg.Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void checkAllBasicInfo()
        {
            cbHotkeys.Checked = true;
            cbMembershipRenewal.Checked = true;
            cbMemberships.Checked = true;
            cbPointOfSales.Checked = true;
            cbProducts.Checked = true;
            cbPromotions.Checked = true;
            cbRedemptionLogs.Checked = true;
            cbUserAccounts.Checked = true;
        }

        private void btnSyncAll_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Sync - All", "");
            ShowPanel();
            /*Get basic info
            checkAllBasicInfo();
            btnGetAll_Click(sender, e);
            */
            //Send sales
            dtpStartDateSales.Value = dtpStartDateSyncAll.Value;
            dtpEndDateSales.Value = dtpEndDateSyncAll.Value;
            btnSyncSales_Click(sender, e);

            if (AppSetting.GetSetting("IsRealTimeSales") != "1")
            {
                //Send logs
                dtpStartDateLogs.Value = dtpStartDateSyncAll.Value;
                dtpEndDateLogs.Value = dtpEndDateSyncAll.Value;
                btnSyncLogs_Click(sender, e);
                HidePanel();
            }

        }
        private void ShowPanel()
        {
            pnlWait.Visible = true;
            pnlWait.BringToFront();
            pnlWait.Refresh();
        }
        private void HidePanel()
        {
            pnlWait.Visible = false;
            pnlWait.SendToBack();
        }

        private void frmSync_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = currentCulture;
        }

        private bool ClientSyncExecution()
        {
            try
            {
                bool result = false, totalResult = true;

                result = OrderSync.StartOrderSync();
                totalResult &= result;

                result = LogsSync.StartLogsSync();
                totalResult &= result;

                SyncClientController.deductInventory();

                result = InventorySync.GetCurrentInventory();
                totalResult &= result;

                result = SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                totalResult &= result;

                result = InventorySync.DeleteInventoryDetFromVoidedOrder();
                totalResult &= result;

                result = OrderSync.UpdateOrderHdrUserFld1236ByPosIDViewBillLimitAndIsVoided();
                totalResult &= result;

                // Send Delivery Order data if using "Cash & Carry / Pre-Order" feature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
                {
                    result = SyncClientController.SendDeliveryOrderToServer(DateTime.Today.AddDays(-3), DateTime.Now, true);
                    totalResult &= result;
                }

                return totalResult;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        private void frmSync_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Sync - Inventory", "");
            ShowPanel();
            if (SyncClientController.SyncInventory(dtpStartDateSyncAll.Value, dtpEndDateSyncAll.Value))
            {
                MessageBox.Show("Sync Inventory Successful");

            }
            else
            {
                MessageBox.Show("Sync Inventory Failed. Please check the logs");
            }
            HidePanel();
        }

        private bool ExportProductsToFile()
        {
            string status = "";
            try
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsToFileEnabled), false))
                {
                    txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Export Products To File is started." + "\r\n" + txtMsg.Text;
                    txtMsg.Refresh();

                    string templateFile = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsTemplateFile);
                    string exportDirectory = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsDirectory);
                    string exportFileName = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsFileName);
                    string exportFilter = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsFilter);

                    if (string.IsNullOrEmpty(templateFile) ||
                        !File.Exists(templateFile) ||
                        Path.GetExtension(templateFile) != ".rpt")
                    {
                        status = "Error: Invalid template file specified for exporting products list. Please check the setting.";
                        Logger.writeLog(status);
                        txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">" + status + "\r\n" + txtMsg.Text;
                        txtMsg.Refresh();
                        return false;
                    }

                    if (string.IsNullOrEmpty(exportDirectory) ||
                        string.IsNullOrEmpty(exportFileName))
                    {
                        status = "Error: Products file export location is not specified. Please check the setting.";
                        Logger.writeLog(status);
                        txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">" + status + "\r\n" + txtMsg.Text;
                        txtMsg.Refresh();
                        return false;
                    }

                    if (!string.IsNullOrEmpty(exportDirectory) && !Directory.Exists(exportDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(exportDirectory);
                        }
                        catch (Exception ex)
                        {
                            status = string.Format("Error: Cannot create directory '{0}'. Message: {1}", exportDirectory, ex.Message);
                            Logger.writeLog(status);
                            txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">" + status + "\r\n" + txtMsg.Text;
                            txtMsg.Refresh();
                            return false;
                        }
                    }

                    string exportFullPath = Path.Combine(exportDirectory, exportFileName);
                    if (!Path.HasExtension(exportFullPath) || Path.GetExtension(exportFullPath) != ".xls") exportFullPath += ".xls";

                    ReportDocument rpt = new ReportDocument();
                    rpt.Load(templateFile);

                    string sql = "SELECT * FROM Item WHERE Deleted = 0 ";
                    if (!string.IsNullOrEmpty(exportFilter)) sql += " AND " + exportFilter;
                    QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
                    DataSet ds = DataService.GetDataSet(cmd);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        rpt.SetDataSource(ds.Tables[0]);
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, exportFullPath);
                        txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Products list exported successfully." + "\r\n" + txtMsg.Text;
                        txtMsg.Refresh();
                    }
                    else
                    {
                        txtMsg.Text = DateTime.Now.ToString("HH:mm:ss") + ">Found no product to export." + "\r\n" + txtMsg.Text;
                        txtMsg.Refresh();
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

        private void btnSyncAppointment_Click(object sender, EventArgs e)
        {
            ShowPanel();
            try
            {
                string status = "";
                if (!AppointmentSync.UploadAppointment(out status))
                    MessageBox.Show("Unable to send appointment to server : " + status);
                else
                {
                    bool getApp = SyncClientController.GetAppointments(false);
                    if (getApp)
                    {
                        MessageBox.Show("Synced Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Unable to get appointment from server");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HidePanel();
        }
    }
    
}