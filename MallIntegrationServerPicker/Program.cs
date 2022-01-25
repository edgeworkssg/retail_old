using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using MallIntegrationServerPicker.Properties;
using System.IO;
using System.Data;
using System.Collections;
using PowerPOS;
using System.Web.Script.Serialization;

namespace MallIntegrationServerPicker
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

        public class SendDataResult
        {
            public bool result;
            public string status;

        }

        public class AppContext : ApplicationContext
        {
            private NotifyIcon trayIcon;
            private frmSetting fSetting;
            private frmStatus fStatus;
            private frmHistory fHistory;
            private Timer timerConn = new Timer();
            private Timer timerCheck = new Timer();
            private bool isConnected = false;
            private bool firstConnect = true;
            private string serverURL;
            private int maxOrder;
            private string currentStatus;
            private string currentAction;
            private static DateTime timeStamp;
            private BackgroundWorker bgWorkerConn = new BackgroundWorker();
            private BackgroundWorker bgWorkerCheck = new BackgroundWorker();
            private int totalNewOrders = 0;
            private bool isCheckError = false;
            private List<string> history = new List<string>();
            private int maxHistory = 1000;
            private string fileDirectory;

            private const string STATUS_CONNECTED = "Connected to server";
            private const string STATUS_DISCONNECTED = "Not connected to server";
            private const string ACTION_CONNECTING = "Connecting to server...";
            private const string ACTION_CHECKING = "Checking for new file since {0}";
            private const string ACTION_FOUNDORDER = "{0} new file after {1}. Downloading {2} of {0} new file";
            private const string ACTION_NOORDER = "No new file since {0}";
            private const string ACTION_PRINTING = "{0} new orders after {1}. Printing {2} of {3} new orders";
            private const string ACTION_ERROR = "Error occured: {0}";
            private const string ACTION_SUCCESS = "Sending File Success";

            public AppContext()
            {
                timeStamp = DateTime.Now;

                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                    Icon = Resources.start_icon,
                    Text = "Edgeworks Mall Integration Picker",
                    ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Status", ShowStatus),
                    new MenuItem("Setting", ShowSetting),
                    new MenuItem("History Log", ShowHistory),
                    new MenuItem("Exit", Exit)
                }),
                    Visible = true
                };

                trayIcon.BalloonTipText = "Starting Edgeworks Mall Integration Server Picker application";
                trayIcon.ShowBalloonTip(3000);

                int checkInterval = Properties.Settings.Default.CheckFileInterval;
                timerCheck.Tick += new EventHandler(timerCheck_Tick);
                timerCheck.Interval = checkInterval * 1000;

                bgWorkerCheck.DoWork += new DoWorkEventHandler(bgWorkerCheck_DoWork);
                bgWorkerCheck.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerCheck_RunWorkerCompleted);

                //SetStatus(STATUS_DISCONNECTED);
                timerCheck.Start();
            }

            private void GetSettings()
            {
                serverURL = Settings.Default.WS_URL.TrimEnd('/') + "/Synchronization/MobileWS.asmx";
                //timerConn.Interval = Settings.Default.CONNECT_INTERVAL_SEC * 1000;
                timerCheck.Interval = Settings.Default.CheckFileInterval * 1000;
                //maxOrder = Settings.Default.MAX_ORDER_FOR_EACH_DOWNLOAD;
            }

            private void timerCheck_Tick(object sender, EventArgs e)
            {
                timerCheck.Enabled = false;
                SetAction(string.Format(ACTION_CHECKING, timeStamp.ToString("dd-MMM-yyyy HH:mm:ss")));
                // CHECK FOR NEW ORDERS
                if (!bgWorkerCheck.IsBusy)
                    bgWorkerCheck.RunWorkerAsync();
            }

            private void ShowSetting(object sender, EventArgs e)
            {
                if (fSetting != null && !fSetting.IsDisposed)
                {
                    fSetting.Activate();
                }
                else
                {
                    fSetting = new frmSetting();
                    fSetting.Show();
                }
            }

            private void SetAction(string msg)
            {
                currentAction = msg;
                if (fStatus != null && !fStatus.IsDisposed)
                {
                    fStatus.setAction(currentAction);
                }
                WriteHistory(currentAction);
                //System.Diagnostics.Debug.WriteLine(currentAction);
            }

            public static bool ChangeTimeStamp(DateTime newTimeStamp)
            {
                timeStamp = newTimeStamp;
                return true;
            }

            private void ShowStatus(object sender, EventArgs e)
            {
                if (fStatus != null && !fStatus.IsDisposed)
                {
                    fStatus.Activate();
                }
                else
                {
                    fStatus = new frmStatus(currentStatus, currentAction);
                    fStatus.Show();
                }
            }

            private void ShowHistory(object sender, EventArgs e)
            {
                if (fHistory != null && !fHistory.IsDisposed)
                {
                    fHistory.Activate();
                }
                else
                {
                    fHistory = new frmHistory(string.Join(System.Environment.NewLine, history.ToArray()));
                    fHistory.Show();
                }
            }

            private void WriteHistory(string msg)
            {
                msg = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss - ") + msg;
                history.Insert(0, msg);
                if (history.Count > maxHistory)
                {
                    history.RemoveRange(history.Count - 1, history.Count - maxHistory);
                }

                if (fHistory != null && !fHistory.IsDisposed)
                {
                    fHistory.writeHistory(string.Join(System.Environment.NewLine, history.ToArray()));
                }
            }

            private bool BackupFile(string path, out string status)
            {
                status = "";
                SetAction("Backup File From : " + path);
                if (File.Exists(path))
                {
                    string targetPath = Properties.Settings.Default.BackupFolder + "\\Backup";
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }
                    
                    string sourcePath = Properties.Settings.Default.Folder;
                    string destinationFile = path.Replace(sourcePath, targetPath);
                    SetAction("Backup File To : " + destinationFile);
                    if (File.Exists(destinationFile))
                        File.Delete(destinationFile);

                    File.Move(path, destinationFile);
                    return true;
                }
                else
                {
                    status = "File not Exist";
                    return false;
                }
            }

            private bool BackupErrorFile(string path, out string status)
            {
                status = "";
                SetAction("Backup File From : " + path);
                if (File.Exists(path))
                {
                    string targetPath = Properties.Settings.Default.BackupFolder + "\\Error";
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.IO.Directory.CreateDirectory(targetPath);
                    }
                    
                    string sourcePath = Properties.Settings.Default.Folder;
                    string destinationFile = path.Replace(sourcePath, targetPath);
                    SetAction("Backup File To : " + destinationFile);
                    if (File.Exists(destinationFile))
                        File.Delete(destinationFile);

                    File.Move(path, destinationFile);
                    return true;
                }
                else
                {
                    status = "File not Exist";
                    return false;
                }
            }

            private void bgWorkerCheck_DoWork(object sender, DoWorkEventArgs e)
            {
                try
                {
                    DateTime tmpTimeStamp = timeStamp;
                    isCheckError = false;
                    string status = "";
                    string directory = Properties.Settings.Default.Folder;
                    if (directory == "")
                    {
                        SetAction("No Directory Folder Defined. Please set the directory folder in the setting");
                    }
                    else
                    {
                        //scan for directory 
                        string[] directories = Directory.GetDirectories(directory);
                        if (directories.GetLength(0) > 0 )
                        {
                            foreach (string dir in directories)
                            {
                                //scan for files
                                string[] files = Directory.GetFiles(dir);

                                if (files.GetLength(0) > 0)
                                {
                                    int i = 1;
                                    foreach (string path in files)
                                    {
                                        SetAction(string.Format("Trying to send file {0} of {1}", i.ToString(), files.GetLength(0)));
                                        if (!path.EndsWith("csv"))
                                        {
                                            //invalid file 
                                            SetAction("Wrong file format for : " + path + ". File will be removed.");
                                            //remove the file into backup folder
                                            BackupFile(path, out status);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                string tmpTenantCode = "";
                                                DataTable message;
                                                var data = new ArrayList();
                                                if (!ExcelController.ImportExcelCSVWithDelimiter(';', path, out message, false))
                                                    throw new Exception("Failed Import the Excel. Please check your format.");

                                                if (message.Columns.Count != 8)
                                                    throw new Exception("The format is wrong. number of columns is not match with the format specification.");

                                                foreach (DataRow dr in message.Rows)
                                                {
                                                    tmpTenantCode = dr[1].ToString();
                                                    var tmpRow = new
                                                    {

                                                        MallCode = dr[0].ToString(),
                                                        TenantCode = dr[1].ToString(),
                                                        Date = dr[2].ToString(),
                                                        Hour = dr[3].ToString(),
                                                        TransactionCount = dr[4].ToString(),
                                                        TotalSalesAfterTax = dr[5].ToString(),
                                                        TotalSalesBeforeTax = dr[6].ToString(),
                                                        TotalTax = dr[7].ToString()

                                                    };
                                                    data.Add(tmpRow);
                                                }
                                                // Get API Key 
                                                MallIntegrationService.MallIntegrationServer msService = new MallIntegrationServerPicker.MallIntegrationService.MallIntegrationServer();
                                                msService.Url = Properties.Settings.Default.WS_URL + "Synchronization/MallIntegrationServer.asmx";
                                                MallIntegrationService.UserCredentials user = new MallIntegrationService.UserCredentials();
                                                user.mallCode = Properties.Settings.Default.MallCode;
                                                user.userName = Properties.Settings.Default.UserName;
                                                user.password = Properties.Settings.Default.Password;
                                                msService.UserCredentialsValue = user;
                                                string ApiKeyResult = msService.getAPIKey(tmpTenantCode);
                                                string temp = "";
                                                if (ApiKeyResult != "")
                                                {
                                                    msService = new MallIntegrationServerPicker.MallIntegrationService.MallIntegrationServer();
                                                    msService.Url = Properties.Settings.Default.WS_URL + "Synchronization/MallIntegrationServer.asmx";
                                                    user = new MallIntegrationService.UserCredentials();
                                                    user.mallCode = Properties.Settings.Default.MallCode;
                                                    user.userName = tmpTenantCode;
                                                    user.password = ApiKeyResult;
                                                    msService.UserCredentialsValue = user;
                                                    temp = msService.SendDataFromServer(new JavaScriptSerializer().Serialize(data));
                                                    SendDataResult res = new JavaScriptSerializer().Deserialize<SendDataResult>(temp);
                                                    if (res.result)
                                                    {
                                                        BackupFile(path, out status);
                                                        SetAction(ACTION_SUCCESS);
                                                    }
                                                    else
                                                    {
                                                        BackupErrorFile(path, out status);
                                                        SetAction(string.Format(ACTION_ERROR, res.status));

                                                    }
                                                }
                                                else
                                                {
                                                    BackupErrorFile(path, out status);
                                                    SetAction(string.Format(ACTION_ERROR, "API Key for not found for tenant : " + tmpTenantCode));
                                                }

                                                
                                            }
                                            catch (Exception ex)
                                            {
                                                SetAction(ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    // Failed
                    isCheckError = true;
                    SetAction(string.Format(ACTION_ERROR, ex.Message));
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            private void bgWorkerCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {
                firstConnect = false;

                if (totalNewOrders > 0 && !isCheckError)
                {
                    timerCheck_Tick(null, null);
                }
                else
                {
                    if (!isCheckError)
                    {
                        SetAction(string.Format(ACTION_NOORDER, timeStamp.ToString("dd-MMM-yyyy HH:mm:ss")));
                    }
                    timerCheck.Enabled = true;
                }
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
