using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using WinAutoSync.Properties;
using SubSonic;

namespace WinAutoSync
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppContext());
        }

        public class AppContext : ApplicationContext
        {
            private NotifyIcon trayIcon;
            private Timer timerCheck = new Timer();
            private BackgroundWorker bgWorkerCheck = new BackgroundWorker();
            private string SyncSaleFilePath = "";
            private string SyncProductFilePath = "";

            public AppContext()
            {
                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = Resources.start_icon,
                    Text = "Edgeworks Auto Sync",
                    ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                }),
                    Visible = true
                };



                trayIcon.BalloonTipText = "Starting Edgeworks Auto Print application";
                trayIcon.ShowBalloonTip(3000);

                SyncSaleFilePath = Settings.Default.SyncSaleFilePath;
                SyncProductFilePath = Settings.Default.SyncProductFilePath;
                timerCheck.Interval = Settings.Default.CHECKORDER_INTERVAL_SEC * 1000;
                timerCheck.Tick += new EventHandler(timerCheck_Tick);
                

                bgWorkerCheck.DoWork += new DoWorkEventHandler(bgWorkerCheck_DoWork);
                //bgWorkerCheck.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerCheck_RunWorkerCompleted);

                timerCheck.Start();

            }

            private void bgWorkerCheck_DoWork(object sender, DoWorkEventArgs e)
            {
                try
                {
                    //check if got closing
                    TrySyncSales();

                    TrySyncProduct();

                    
                }
                catch (Exception ex)
                {
                    // Failed
                    System.Diagnostics.Debug.WriteLine("Unable start remote process: " + SyncSaleFilePath + " " + ex.Message);
                }
            }

            private void TrySyncSales()
            {
                
                string sqlString = "Select AppSettingValue from AppSetting where AppSettingKey = 'Integration_NeedToSyncSale'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                bool isNeedToSync = false;
                if (tmp == null || !bool.TryParse(tmp.ToString(), out isNeedToSync))
                    isNeedToSync = false;

                if (!isNeedToSync)
                    return;

                string updateQuery = "Update Appsetting set appsettingvalue = 'False' where appsettingkey = 'Integration_NeedToSyncSale'";
                DataService.ExecuteQuery(new QueryCommand(updateQuery));
                System.Diagnostics.Process.Start(SyncSaleFilePath);
            }

            private void TrySyncProduct()
            {
                string sqlString = "Select AppSettingValue from AppSetting where AppSettingKey = 'Integration_NeedToSyncProduct'";
                object tmp = DataService.ExecuteScalar(new QueryCommand(sqlString));
                bool isNeedToSync = false;
                if (tmp == null || !bool.TryParse(tmp.ToString(), out isNeedToSync))
                    isNeedToSync = false;

                if (!isNeedToSync)
                    return;

                string updateQuery = "Update Appsetting set appsettingvalue = 'False' where appsettingkey = 'Integration_NeedToSyncProduct'";
                DataService.ExecuteQuery(new QueryCommand(updateQuery));
                System.Diagnostics.Process.Start(SyncProductFilePath);
            }



            

            private void timerCheck_Tick(object sender, EventArgs e)
            {
                
                if (!bgWorkerCheck.IsBusy)
                    bgWorkerCheck.RunWorkerAsync();
            }

            private void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Visible = false;
                Application.Exit();
            }
        }
    }
}
