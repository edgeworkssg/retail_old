using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using PowerPOS.SalesController;
using PowerPOSLib.Controller;
using System.Threading;
using WinPowerPOS.LoginForms;

namespace WinPowerPOS.KioskForms
{
    public partial class frmStartKiosk : Form, ISync, IWeighingMachine, ISubject, IObserver
    {
        #region DECLARATION 
        public event StateChangeHandler StateChanged;
        public event StateChangeHandler SubStateChanged;

        public BackgroundWorker SyncProductThread;
        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncMasterDataThread;
        public BackgroundWorker SyncUserThread;
        public BackgroundWorker SyncLogThread;
        public BackgroundWorker BWNets;

        private WeighingMachineController wmController;
        private NetsController netsController;

        public Queue<string> BufferLog = new Queue<string>();
        private List<IObserver> bufferLogObserver = new List<IObserver>();

        private string _state = "";
        public string State
        {
            get { return _state; }
            set 
            {
                string oldVal = _state;
                _state = value;
                if (StateChanged != null && !oldVal.ToLower().Equals(_state.ToLower()))
                    StateChanged(this, new StateChangeAgrs { PreviousState = oldVal, CurrentState = value });
            }
        }

        private string _subState = "";
        public string SubState
        {
            get { return _subState; }
            set 
            { 
                string oldVal = _subState;
                _subState = value;
                if (SubStateChanged != null && !oldVal.ToLower().Equals(_subState.ToLower()))
                    SubStateChanged(this, new StateChangeAgrs { PreviousState = oldVal, CurrentState = value });
            }
        }

        public decimal LastWeight = 0;
        public decimal newWeight = 0;
        private bool IsZeroWeight = true;

        public bool IsWMConnected = false;
        public bool IsNETSConnected = false;

        public int weightTolerance = 0;

        public bool SimulatorWeighingScale = false;
        public bool SimulatorNETS = false;

        public frmSimulatorWeightScale frmSimulatorWeightScale;

        public BarcodeScannerController barcodeScanner;

        private frmKiosk fKiosk;
        private frmSettingKiosk fSetting;
        public bool kioskEnabled = true;
        private bool isReadmode = false;
        public bool isCashMachineAvailable = false;
        public bool cashMachineEnabled = false;

        public bool useWeighingScale = false;
        public bool useChsLang = false;
        //public CashManager cashMgr;

        private string kioskMode;

        public class KioskState
        {
            public const string PROCESSING = "PROCESSING";
            public const string STANDBY = "STANDBY";
            public const string CHECKOUT = "CHECKOUT";
            public const string DISABLED = "DISABLED";

            public const string PROCESSING_WAITFORITEMSCAN = "Wait For Item Scan";
            public const string PROCESSING_WAITFORBAGGING = "Wait For Bagging";
            public const string PROCESSING_WAITFORREMOVAL = "Wait For Removal";

            public const string STANDBY_WAITFORREADY = "Wait For Ready";
            public const string STANDBY_READY = "Ready";

            //public const string CHECKOUT = "CHECKOUT";
            //public const string DISABLED = "DISABLED";
        }

        private List<string> indicatorList = new List<string>();

        List<string> hardwareList = new List<string>();
            


        #endregion

        public frmStartKiosk()
        {
            InitializeComponent();

            SyncSalesThread = new BackgroundWorker();
            SyncSalesThread.DoWork += new DoWorkEventHandler(SyncThread_DoWork);

            SyncProductThread = new BackgroundWorker();
            SyncProductThread.DoWork += new DoWorkEventHandler(SyncProductThread_DoWork);

            SyncMasterDataThread = new BackgroundWorker();
            SyncMasterDataThread.DoWork += new DoWorkEventHandler(SyncMasterDataThread_DoWork);

            SyncUserThread = new BackgroundWorker();
            SyncUserThread.DoWork += new DoWorkEventHandler(SyncUserThread_DoWork);

            SyncLogThread = new BackgroundWorker();
            SyncLogThread.DoWork += new DoWorkEventHandler(SyncLogThread_DoWork);

            BWNets = new BackgroundWorker();
            //BWNets.DoWork += new DoWorkEventHandler(BWNets_DoWork);


            UserMst user = null;
            string role = null;
            string DeptID = null;
            string status = "";

            if (UserController.login("edgeworks", "pressingon", LoginType.Login, out user, out role, out DeptID, out status))
            {
                UserInfo.username = user.UserName;
                UserInfo.role = role;
                UserInfo.displayName = user.DisplayName;
                UserInfo.SalesPersonGroupID = user.SalesPersonGroupID;

                SalesPersonInfo.SalesPersonID = UserInfo.username;
                SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
            }

            SyncProduct();
            SyncMasterData();
            SyncUserData();
            SyncLogData();

            weightTolerance = 0;
            Int32.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance), out weightTolerance);

            Attach(this);

            wmController = new WeighingMachineController();

            #region Simulator Mode

            SimulatorWeighingScale = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.SimulatorWeighingScale), false);
            SimulatorNETS = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.SimulatorNETS), false);

            frmSimulatorWeightScale = new frmSimulatorWeightScale();
            if (SimulatorWeighingScale)
            {
                frmSimulatorWeightScale.init();
                frmSimulatorWeightScale.Show();
            }

            #endregion

            kioskMode = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.KioskMode);

            netsController = new NetsController();
            //netsController.setupSerialPort(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NetsCOMPort));

            useWeighingScale = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);

            wmController.StatusChanged += new StatusChangedHandler(wmController_StatusChanged);
            wmController.WeightChanged += new WeightChangedHandler(wmController_WeightChanged);

            useChsLang = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.UseCHSLang), false);

            if (SimulatorWeighingScale)
            {
                frmSimulatorWeightScale.StatusChanged += new StatusChangedHandler(wmController_StatusChanged);
                frmSimulatorWeightScale.WeightChanged += new WeightChangedHandler(wmController_WeightChanged);
            }


            #region Init Cash Machine if used 
            /*cashMgr = new CashManager();
            cashMachineEnabled = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNotes), false);
            if (cashMachineEnabled)
            {
                string cashMachineStatus = "";
                string url = Environment.CurrentDirectory + "\\" +  Properties.Settings.Default.CashMgmtDll;
                if (!cashMgr.Init(url, out cashMachineStatus))
                {
                    isCashMachineAvailable = false;
                    AppendLog(cashMachineStatus);
                }
                if (!cashMgr.Connect(out cashMachineStatus))
                {
                    isCashMachineAvailable = false;
                    AppendLog(cashMachineStatus);
                }
                
                isCashMachineAvailable = true;
            }*/
            #endregion


            // 1.A.1
           

            btnStart.OnClicked += new StartKioskButton.OnClickEventHandler(btnStart_OnClicked);
            btnLang.OnClicked += new LangButton.OnClickEventHandler(btnLang_OnClicked);
            this.btnLog.Visible = !AppSetting.CastBool(AppSetting.GetSetting("Kiosk_HideLogButton"), false);
            btnLog.Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.HideLogButton), false);
        }

        


        void SyncLogThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {

                /*SyncLogsRealTimeController syncLogsCtrl = new SyncLogsRealTimeController();
                syncLogsCtrl.OnProgressUpdates += syncLogsCtrl_OnProgressUpdates;
                syncLogsCtrl.SendLogs();*/
                PowerPOS.SyncLogsController.SyncLogsRealTimeController syncLogsCtrl =
                    new PowerPOS.SyncLogsController.SyncLogsRealTimeController();
                //spc.OnProgressUpdates += new PowerPOS.SyncProductController.UpdateProgress(spc_OnProgressUpdates);
                syncLogsCtrl.SendLogs();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncUserThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncUserRealTimeController.SyncUserRealTimeController spc =
                    new PowerPOS.SyncUserRealTimeController.SyncUserRealTimeController();
                //spc.OnProgressUpdates += new PowerPOS.SyncProductController.UpdateProgress(spc_OnProgressUpdates);
                spc.SyncUser();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void SyncMasterDataThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncMasterDataRealTimeController.SyncMasterDataRealTimeController spc =
                    new PowerPOS.SyncMasterDataRealTimeController.SyncMasterDataRealTimeController();
                //spc.OnProgressUpdates += new PowerPOS.SyncProductController.UpdateProgress(spc_OnProgressUpdates);
                spc.SyncMasterData();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void btnLang_OnClicked(object sender, EventArgs args)
        {
            string langID = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
            langID = String.IsNullOrEmpty(langID) ? "ENG" : langID;

            if (langID == "ENG")
                AppSetting.SetSetting(AppSetting.SettingsName.LanguageSetting, "CHS");
            else
                AppSetting.SetSetting(AppSetting.SettingsName.LanguageSetting, "ENG");

            btnLang.Reload();
            btnStart.Reload();
        }

        

        void wmController_WeightChanged(WeighingMachineController m, WeightChangedArgs e)
        {
            if (!useWeighingScale)
            {
                return;
            }

            if (State != KioskState.DISABLED && State != KioskState.STANDBY)
                return;

            if (e.diff == 0)
            {
                return;
            }

            //if mode = payment only no need to check weight
            if (kioskMode == KioskMode.PAYMENT_ONLY)
                return;

            //Display Weight Change
            if (SimulatorWeighingScale)
                AppendLog("Weight simulator increase " + e.diff + "g to " + e.currentWeight + "g (Last Weight " + LastWeight + "g)");
            else
                AppendLog("Weight increase " + e.diff + "g to " + e.currentWeight + "g (Last Weight " + LastWeight + "g)");

            newWeight = e.currentWeight;

            if (State == KioskState.DISABLED)
            {
                if (!KioskHelper.isNewWeightEquals(e.currentWeight, LastWeight, weightTolerance))
                {

                    AppendLog("Weight is not 0. Show Indicator W!0");
                    showIndicator("W!0");
                    return;
                }

                if (KioskHelper.isNewWeightEquals(e.currentWeight, LastWeight, weightTolerance))
                {
                    AppendLog("Weight is 0. Hide indicator W!0");
                    hideIndicator("W!0");
                    return;
                }
            }

            if (State == KioskState.STANDBY && SubState == KioskState.STANDBY_WAITFORREADY)
            {
                if (!KioskHelper.isNewWeightEquals(e.currentWeight, LastWeight, weightTolerance))
                {

                    AppendLog("Weight is not 0. Can not show start button");
                    displayStartButton(false);
                    showIndicator("W!0");
                    return;
                }

                if (KioskHelper.isNewWeightEquals(e.currentWeight, LastWeight, weightTolerance))
                {
                    AppendLog("Weight is 0. Can show start button");
                    hideIndicator("W!0");
                    SubState = KioskState.STANDBY_READY;
                    StateInit();
                    
                    return;
                }
            }

            if (State == KioskState.STANDBY && SubState == KioskState.STANDBY_READY)
            {
                if (!KioskHelper.isNewWeightEquals(e.currentWeight, LastWeight, weightTolerance))
                {
                    
                    AppendLog("Weight is not 0. Can not show start button");
                    //showIndicator("W!0");
                    SubState = KioskState.STANDBY_WAITFORREADY;
                    StateInit();
                    return;
                }

                if (KioskHelper.isNewWeightEquals(e.currentWeight, LastWeight, weightTolerance))
                {
                    return;
                }
            }

            
        }

        private void wmController_StatusChanged(WeighingMachineController m, StatusChangedArgs e)
        {
            IsWMConnected = e.currentStatus.ToUpper() == "CONNECTED";
            if (State != KioskState.DISABLED && State != KioskState.STANDBY)
                return;

            if (IsWMConnected) // 2.B.1
            {     
                if (SimulatorWeighingScale)
                    AppendLog("Weighing Scale Simulator is connected");
                else
                    AppendLog("Hardware: Weighing Scale is connected");

                if (hardwareList.Contains("Weighing Machine"))
                    hardwareList.Remove("Weighing Machine");

                State = KioskState.DISABLED;
                //SubState = KioskState.STANDBY_WAITFORREADY;
                StateInit();

                /*hideIndicator("Wx");
                

                //read new Weight from weighing machine

                if (hardwareList.Count == 0 && kioskEnabled)
                {
                    State = KioskState.STANDBY;
                    SubState = KioskState.STANDBY_WAITFORREADY;
                    StateInit();

                }*/
                return;
            }

            if (!IsWMConnected) // 2.B.1
            {     
                if (SimulatorWeighingScale)
                    AppendLog("Weighing Scale Simulator is disconnected");
                else
                    AppendLog("Hardware: Weighing Scale is disconnected");

                State = KioskState.DISABLED;
                StateInit();
                
            }


            

            
        }

        void SyncProductThread_DoWork(object sender, DoWorkEventArgs e)
        {
            SyncClientController.Load_WS_URL();
            try
            {
                PowerPOS.SyncProductController.SyncProductRealTimeController spc =
                    new PowerPOS.SyncProductController.SyncProductRealTimeController();
                //spc.OnProgressUpdates += new PowerPOS.SyncProductController.UpdateProgress(spc_OnProgressUpdates);
                spc.SyncProducts();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
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
                //ssc.OnProgressUpdates += scc_OnProgressUpdates;
                ssc.SendRealTimeSales();

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        void btnStart_OnClicked(object sender, EventArgs args)
        {
            ctrlVideo.Stop();

            try
            {
                AppendLog("START button is clicked");

                State = "PROCESSING";
                SubState = "Wait For Item Scan";

                this.Focus();

                if (fKiosk == null || fKiosk.IsDisposed)
                {
                    fKiosk = new frmKiosk(this);
                    fKiosk.barcodeScanner = this.barcodeScanner;
                }

                fKiosk.isAgeVerified = false;
                fKiosk.isStart = true;
                AppendLog("Start Form. isStart = " + fKiosk.isStart.ToString());

                if (SimulatorWeighingScale)
                    fKiosk.Show();
                else
                    fKiosk.ShowDialog(this);
            }
            catch (Exception ex)
            {
                //AppendLog("=== END KIOSK with ERROR (see PowerLog for the detail) ===");

                Logger.writeLog(ex);

                State = "DISABLED";
                SubState = "";

                MessageBox.Show(ex.Message);
            }
        }

        private void frmStartKiosk_Load(object sender, EventArgs e)
        {
            try
            {
                AppendLog("Starting up");

                #region BarcodeScanner
                if (AppSetting.CastBool(AppSetting.GetSetting("BarcodeScanner_Enabled"), false))
                {
                    this.barcodeScanner = new BarcodeScannerController();
                    this.AppendLog("Init Hardware: Barcode Scanner ");
                    string initstatus = "";
                    if (!this.barcodeScanner.initialize(out initstatus))
                    {
                        this.AppendLog("Init Hardware: Barcode Scanner Failed");
                    }
                }
                #endregion

                this.AppendLog("Init Hardware: Weighing Machine ");
                bwWM.RunWorkerAsync();
                
                
                this.fKiosk = new frmKiosk(this);
                this.barcodeScanner.BarcodeScanned += new BarcodeScannedHandler(this.barcodeScanner_BarcodeScannedStartKiosk);
                this.barcodeScanner.BarcodeScanned += new BarcodeScannedHandler(this.fKiosk.barcodeScanner_BarcodeScanned);
                this.barcodeScanner.DeviceStateChanged += new DeviceStateChangedHandler(this.fKiosk.barcodeScanner_DeviceStateChanged);
                this.fKiosk.barcodeScanner = this.barcodeScanner;
                this.barcodeScanner.disableDevice();
                

                #region CashManager
                /*if (cashMachineEnabled)
            {
                cashMgr.CashReceived += new CashReceivedHandler(fKiosk.onCashReceived);
                cashMgr.CashLogged += new CashLogHandler(fKiosk.onCashLogged);
                cashMgr.CashReturned += new CashReturnedHandler(fKiosk.onCashReturned);
                cashMgr.CashCollected += new CashCollectedHandler(fKiosk.onCashCollected);
                cashMgr.CashOutNote += new CashOutNoteHandler(fKiosk.onCashOutNote);
                cashMgr.CashError += new CashErrorHandler(fKiosk.onCashError);
                cashMgr.CashMachineConnected += new CashMachineConnectedHandler(fKiosk.onCashMachineConnected);
                cashMgr.CashMachineDisconnected += new CashMachineDisconnectedHandler(fKiosk.onCashMachineDisconnected);
            }*/
                #endregion

                /*#region setting page
            fSetting = new frmSettingKiosk();
            wmController.WeightChanged += new StatusChangedHandler(this.fSetting.wmController_StatusChanged);
            #endregion*/

                this.ctrlScanner.CancelClick += new EventHandler(this.ctrlScanner_CancelClick);
                //BWNets.RunWorkerAsync();

                this.kioskEnabled = AppSetting.CastBool(AppSetting.GetSetting("Kiosk_Status"), true);
                //AppendLog("Play Video welcome.wmv");

                ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\welcome.wmv"), true);
                btnStart.Reload();
                btnLang.Reload();
                lblStatus.Visible = false;

                State = KioskState.DISABLED;
                StateInit();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
            }
        }

        private void ctrlScanner_CancelClick(object sender, EventArgs e)
        {
            this.CloseBarcodeScanner();
        }

        private void frmStartKiosk_Activated(object sender, EventArgs e)
        {
            //AppendLog("Play Video welcome.wmv");

            if (State == "STANDBY")
            {
                ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\welcome.wmv"), true);
                btnStart.Reload();
            }
        }

        public void CloseBarcodeScanner()
        {
            this.barcodeScanner.disableDevice();
            this.hostScanner.Visible = false;
            this.hostScanner.SendToBack();
        }

        public void OpenBarcodeScanner(string title, string message)
        {
            string outstatus = "";
            this.barcodeScanner.init(out outstatus);
            int x = base.Width / 2 - this.hostScanner.Width / 2;
            int y = base.Height / 2 - this.hostScanner.Height / 2;
            this.ctrlScanner.Title = title;
            this.ctrlScanner.Message = message;
            this.hostScanner.Location = new Point(x, y);
            this.hostScanner.Visible = true;
            this.hostScanner.BringToFront();
        }


        public void barcodeScanner_BarcodeScannedStartKiosk(BarcodeScannerController m, BarcodeScannedArgs e)
        {
            if (this.State != "STANDBY" && this.State != "DISABLED")
            {
                return;
            }
            this.AppendLog("Barcode scanned : " + e.lastBarcode);
            if (string.IsNullOrEmpty(e.lastBarcode))
            {
                return;
            }
            this.Invoke(new MethodInvoker(delegate
            {
                this.OpenSetting(e.lastBarcode);
            }));
        }

        private void OpenSetting(string token)
        {
            this.CloseBarcodeScanner();
            UserMst um = new UserMst(UserMst.UserColumns.BarcodeToken, token);
            if (um.IsNew)
            {
                this.AppendLog("Invalid staff barcode : " + token);
                MessageBox.Show("Invalid token");
                return;
            }
            if (!um.IsHavePrivilegesFor("Kiosk Setting Page"))
            {
                this.AppendLog(string.Format("User {0} doesn't has privilege to open setting form", um.UserName));
                MessageBox.Show("Insuficient Privileges");
                return;
            }

            this.State = KioskState.DISABLED;
            

            AppendLog("Staff Logged in : " + um.UserName);
            SubState = "open setting page";
            frmSettingKiosk frm = new frmSettingKiosk(this, um.UserName);
            frm.ShowDialog(this);

            this.State = KioskState.DISABLED;

            SubState = "";
            this.useWeighingScale = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);
            this.kioskEnabled = AppSetting.CastBool(AppSetting.GetSetting("Kiosk_Status"), true);
            this.SimulatorWeighingScale = AppSetting.CastBool(AppSetting.GetSetting("Kiosk_SimulatorWeighingScale"), false);
            this.SimulatorNETS = AppSetting.CastBool(AppSetting.GetSetting("Kiosk_SimulatorNETS"), false);
            this.btnLog.Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.HideLogButton), false);
            if (this.SimulatorWeighingScale)
            {
                this.frmSimulatorWeightScale.init();
                this.frmSimulatorWeightScale.Show();
                this.frmSimulatorWeightScale.StatusChanged -= new StatusChangedHandler(this.wmController_StatusChanged);
                this.frmSimulatorWeightScale.StatusChanged += new StatusChangedHandler(this.wmController_StatusChanged);
                this.frmSimulatorWeightScale.WeightChanged -= new WeightChangedHandler(this.wmController_WeightChanged);
                this.frmSimulatorWeightScale.WeightChanged += new WeightChangedHandler(this.wmController_WeightChanged);
            }
            else
            {
                this.frmSimulatorWeightScale.Hide();
                this.frmSimulatorWeightScale.StatusChanged -= new StatusChangedHandler(this.wmController_StatusChanged);
                this.frmSimulatorWeightScale.WeightChanged -= new WeightChangedHandler(this.wmController_WeightChanged);
            }

            #region Reinit Cash Machine if if was disabled 
            /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNotes), false))
            {
                if (!isCashMachineAvailable)
                {
                    string status = "";
                    if (!cashMgr.Connect(out status))
                    {
                        AppendLog("Init Cash Machine Failed. " + status);
                        isCashMachineAvailable = false;
                    }
                    isCashMachineAvailable = true;
                }
            }*/
            /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableCoins), false))
            {

            }*/

            kioskMode = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.KioskMode);
            #endregion
            this.btnStart.Reload();
            this.btnLang.Reload();
            btnStart.Focus();
            StateInit();
        }

        #region ISync Members

        public void SyncProduct()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncProducts), false))
                if (!SyncProductThread.IsBusy)
                    SyncProductThread.RunWorkerAsync();
        }

        public void SyncSales()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                if (!SyncSalesThread.IsBusy)
                    SyncSalesThread.RunWorkerAsync();
        }

        public void SyncMasterData()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMasterData), false))
                if(!SyncMasterDataThread.IsBusy)
                    SyncMasterDataThread.RunWorkerAsync();
        }

        public void SyncUserData()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncUser), false))
                if (!SyncUserThread.IsBusy)
                    SyncUserThread.RunWorkerAsync();
        }

        public void SyncLogData()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncLogs), false))
                if (!SyncLogThread.IsBusy)
                    SyncLogThread.RunWorkerAsync();
        }

        #endregion

        private void btnSetting_Click(object sender, EventArgs e)
        {
            /*AppendLog("Open Setting page");

            ctrlVideo.Stop();

            frmScanBarcode frmScan = new frmScanBarcode(this,"Staff Card");
            if (frmScan.ShowDialog() != DialogResult.OK)
                return;

            UserMst um = new UserMst(UserMst.UserColumns.BarcodeToken, frmScan.BarcodeResult);
            if (um.IsNew)
            {
                AppendLog("Invalid staff barcode : "+frmScan.BarcodeResult);
                MessageBox.Show("Invalid barcode");
                return;
            }
            else 
            {
                if (!um.IsHavePrivilegesFor("Kiosk Setting Page"))
                {
                    AppendLog(string.Format("User {0} doesn't has privilege to open setting form", um.UserName));
                    MessageBox.Show("Insuficient Privileges");
                    return;
                }
            }

            frmSettingKiosk frm = new frmSettingKiosk(this, um.UserName);
            frm.ShowDialog(this);


            #region Simulator Mode

            SimulatorWeighingScale = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.SimulatorWeighingScale), false);
            SimulatorNETS = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.SimulatorNETS), false);

            if (SimulatorWeighingScale)
            {
                frmSimulatorWeightScale.init();
                frmSimulatorWeightScale.Show();

                frmSimulatorWeightScale.StatusChanged -= wmController_StatusChanged;
                frmSimulatorWeightScale.StatusChanged += new StatusChangedHandler(wmController_StatusChanged);

                frmSimulatorWeightScale.WeightChanged -= wmController_WeightChanged;
                frmSimulatorWeightScale.WeightChanged += new WeightChangedHandler(wmController_WeightChanged);
            }
            else
            {
                frmSimulatorWeightScale.Hide();

                frmSimulatorWeightScale.StatusChanged -= wmController_StatusChanged;
                frmSimulatorWeightScale.WeightChanged -= wmController_WeightChanged;
            }

            #endregion

            btnStart.Reload();
            btnLang.Reload();*/
            this.AppendLog("Open Setting page");
            //this.SubState = "scan staff for setting";
            this.isReadmode = false;
            txtBarcode.Text = "";
            txtBarcode.Focus();
            this.OpenBarcodeScanner("Staff Access", "Pls Scan your Staff Card");

            this.ctrlVideo.Stop();
        }

        private void bwWM_DoWork(object sender, DoWorkEventArgs e)
        {
            string status = "";
            bool isWeighingSuccess = wmController.init(out status);
            if (!isWeighingSuccess)
                AppendLog("Weighing Machine Init Failed : " + status);
        }

        #region IWeighingMachine Members

        public WeighingMachineController GetWeightingMachineController()
        {
            return wmController;
        }

        #endregion

        

        private void button1_Click(object sender, EventArgs e)
        {
            txtLog.Visible = !txtLog.Visible;

            txtLog.Text = "";

            txtLog.AppendText(String.Join(Environment.NewLine, BufferLog.ToArray()));

            btnStart.Focus();
        }

        void BWNets_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void timerAllAvailable_Tick(object sender, EventArgs e)
        {

            /*try
            {

                bool useWeighingMachine = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);
                IsNETSConnected = true;

                lblStatus.Visible = false;

                {
                    string tmp = "";

                    if (!IsWMConnected || !useWeighingMachine)
                        tmp += "Wx ";


                    if (!IsNETSConnected)
                        tmp += "Nx ";

                    if (Math.Abs(LastWeight) > WeightTolerance)
                        tmp += "W!0 ";

                    if (!this.kioskEnabled)
                    {
                        tmp += "D";
                    }
                    //lblStatus.Text = tmp;
                    ctrlVideo.SetStatusCode(tmp);

                    //if (lblStatus.Text != "")
                    //    lblStatus.Visible = true;
                }

                if (!this.kioskEnabled)
                {
                    if (this.State == "STANDBY")
                    {
                        this.AppendLog("All payment mode/hardware are connected");
                        this.AppendLog("State:" + this.State);
                    }
                    this.State = "DISABLED";
                    this.ButtonStartElement.Visible = false;
                    this.ctrlVideo.SetDisabledMode(true);
                    return;
                }

                if (!useWeighingMachine && IsNETSConnected && State == "DISABLED")
                {
                    State = "STANDBY";

                    AppendLog("All payment mode/hardware are connected");
                    AppendLog("State:" + State);

                    ButtonStartElement.Visible = true;
                    //lblDisabledMode.Visible = false;
                    ctrlVideo.SetDisabledMode(false);
                }
                else
                    if (IsWMConnected && IsNETSConnected && State == "DISABLED" && useWeighingMachine)
                    {
                        State = "STANDBY";

                        AppendLog("All payment mode/hardware are connected");
                        AppendLog("State:" + State);

                        ButtonStartElement.Visible = true;
                        //lblDisabledMode.Visible = false;
                        ctrlVideo.SetDisabledMode(false);
                    }
                    else if (IsWMConnected && IsNETSConnected && useWeighingMachine)
                    {
                        ButtonStartElement.Visible = IsZeroWeight;
                        //elementHost1.Visible = true;
                        //lblDisabledMode.Visible = false;
                        ctrlVideo.SetDisabledMode(false);
                    }
                    else if (!useWeighingMachine && IsNETSConnected)
                    {
                        ButtonStartElement.Visible = true;

                        ctrlVideo.SetDisabledMode(false);
                    }
                    else
                    {

                        ButtonStartElement.Visible = false;
                        //lblDisabledMode.Visible = true;
                        //948224
                        ctrlVideo.SetDisabledMode(true);
                    }

                if (ButtonStartElement.Visible)
                {
                    bool isShowCHS = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.UseCHSLang), false);
                    elementHost3.Visible = isShowCHS;
                }
                else
                {
                    elementHost3.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Timer All Available." + ex.Message);
            }*/
        }

        #region ISubject Members

        public void Attach(IObserver obj)
        {
            this.bufferLogObserver.Add(obj);
        }

        public void Detach(IObserver obj)
        {
            this.bufferLogObserver.Remove(obj);
        }

        public void Notify()
        {
            foreach (IObserver o in bufferLogObserver)
            {
                o.UpdateLog();
            }
        }

        public void SendCommand(string command)
        {
            foreach (IObserver o in bufferLogObserver)
            {
                o.ExecuteCommand(command);
            }
        }

        #endregion

        #region IObserver Members

        void IObserver.UpdateLog()
        {
            txtLog.Text = "";

            txtLog.AppendText(String.Join(Environment.NewLine, BufferLog.ToArray()));
        }

        void IObserver.ExecuteCommand(string command)
        {
            switch (command)
            {
                default:
                    break;
            }
        }

        #endregion

        private void frmStartKiosk_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wmController != null)
                wmController.Dispose();
        }

        public void StartVideo()
        {
            try
            {
                ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\welcome.wmv"), true);
                btnStart.Reload();
                btnLang.Reload();
            }
            catch (Exception ex)
            {
                AppendLog(string.Format("-Start playing video failed : " + ex.Message));
                Logger.writeLog(ex);
            }
        }

        public void StopVideo()
        {
            try
            {
                ctrlVideo.Stop();
            }
            catch (Exception ex)
            {
                AppendLog(string.Format("-Stop playing video failed : " + ex.Message));
                Logger.writeLog(ex);
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (State != "STANDBY" && State != "DISABLED" && this.State != "CHECKOUT")
                return;

            if (SubState == "")
            {
                txtBarcode.Text = "";
            }

            if (SubState == "scan staff for setting")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtBarcode.Text = txtBarcode.Text.Trim();
                    OpenSetting(txtBarcode.Text);
                }    
            }
        }

        #region state

        public void StateInit()
        {
            AppendLog("State : " + this.State + ", SubState : " + this.SubState);

            
            if (State != KioskState.STANDBY && State != KioskState.DISABLED)
                return;

            if (State == KioskState.DISABLED)
            {
                showKioskEnabled(true);
                displayStartButton(false);
                WaitAllHardwareConnected();
                if (hardwareList.Count == 0 && kioskEnabled)
                {
                    hideIndicator("Wx");
                    AppendLog("All Hardware Connected");
                    State = KioskState.STANDBY;
                    this.SubState = KioskState.STANDBY_WAITFORREADY;
                    StateInit();
                }
                //State = ""
                return;
            }

            if (State == KioskState.STANDBY && SubState == KioskState.STANDBY_WAITFORREADY)
            {
                LastWeight = 0;
                AppendLog("Reset Last Weight To 0");

                //if current weight <> 0 then display "W!0" (Weight is not zero) indicator
                newWeight = wmController.getWeight();
                AppendLog("Current Weight : " + newWeight.ToString());

                displayStartButton(false);
                AppendLog("Hide Start Button");

                showKioskEnabled(false);

                if (useWeighingScale && !IsWMConnected)
                {
                    State = KioskState.DISABLED;
                    StateInit();
                    return;
                }

                if (!useWeighingScale)
                {
                    hideIndicator("W!0");
                    hideIndicator("Wx");
                    SubState = KioskState.STANDBY_READY;
                    AppendLog("Weight is zero. Can show start button.");
                    StateInit();
                    return;
                }

                if ( !KioskHelper.isNewWeightEquals(newWeight, 0, weightTolerance))
                {
                    showIndicator("W!0");
                    AppendLog("Weight is not zero. Cannot show start button yet.");
                    return;
                }

                if (useWeighingScale && KioskHelper.isNewWeightEquals(newWeight, 0, weightTolerance))
                {
                    SubState = KioskState.STANDBY_READY;
                    AppendLog("Weight is zero. Can show start button.");
                    StateInit();
                    
                }
                return;
            }// end of if (State == KioskState.STANDBY && SubState == KioskState.STANDBY_WAITFORREADY)

            if (State == KioskState.STANDBY && SubState == KioskState.STANDBY_READY)
            {
                displayStartButton(true);
                AppendLog("Waiting for user to click START button");
                return;
            }//end of(State == KioskState.STANDBY && SubState == KioskState.STANDBY_READY)



            
            

            
        }
        #endregion

        private void hostVideo_ChildChanged(object sender, System.Windows.Forms.Integration.ChildChangedEventArgs e)
        {

        }


        #region show Control 

        void showIndicator(string indi)
        {
            if (!indicatorList.Contains(indi))
            {
                indicatorList.Add(indi);
            }

            //display the list
            displayIndicator();
        }

        void hideIndicator(string indi)
        {
            if (indicatorList.Contains(indi))
            {
                indicatorList.Remove(indi);
            }
            //display the list
            displayIndicator();
        }

        delegate void displayIndicatorDelegate();

        public void displayIndicator()
        {
            if (this.InvokeRequired)
            {
                displayIndicatorDelegate d = new displayIndicatorDelegate(displayIndicator);
                this.Invoke(d);
                return;
            }

            if (indicatorList.Count == 0)
            {
                ctrlVideo.SetStatusCode("");
                //lblStatus.Visible = false;
                //lblStatus.Text = "";
                return;
            }

            //lblStatus.Visible = true;
            string tmpIndi = "";
            foreach (string s in indicatorList)
            {
                tmpIndi += s+" ";
            }
            ctrlVideo.SetStatusCode(tmpIndi);

        }

        delegate void displayStartButtonDelegate(bool enabled);

        void displayStartButton(bool enabled)
        {
            if (this.InvokeRequired)
            {
                displayStartButtonDelegate d = new displayStartButtonDelegate(displayStartButton);
                this.Invoke(d, new object[] { enabled });
                return;
            }

            ButtonStartElement.Visible = enabled;

            if (useChsLang)
                ButtonLanguageElement.Visible = enabled;
            else
                ButtonLanguageElement.Visible = false;
            
            
        }

        delegate void showKioskEnabledDelegate(bool enabled);

        private void showKioskEnabled(bool enabled)
        {
            if (this.InvokeRequired)
            {
                showKioskEnabledDelegate d = new showKioskEnabledDelegate(showKioskEnabled);
                this.Invoke(d, new object[] { enabled });
                return;
            }
            
                if (!kioskEnabled)
                    showIndicator("D");
                else
                    hideIndicator("D");
            
            ctrlVideo.SetDisabledMode(enabled);
        }

        delegate void AppendLogDelegate(string item);

        public void AppendLog(string item)
        {
            if (this.InvokeRequired)
            {
                AppendLogDelegate d = new AppendLogDelegate(AppendLog);
                this.Invoke(d, new object[] { item });
                return;
            }
            else
            {
                try
                {

                    DateTime dt = DateTime.Now;

                    Logger.writeStaffAssistLog("INFO", item);

                    if (this.BufferLog.Count > 1000)
                        this.BufferLog.Dequeue();

                    this.BufferLog.Enqueue(dt.ToString("dd-MM-yyyy hh:mm:ss") + " - " + item);

                    this.Notify();

                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }

        }

        void WaitAllHardwareConnected()
        {
            if (IsWMConnected)
            {
                AppendLog("Weighing Machine is connected");
                
                return;
            }

            if (!useWeighingScale)
            {
                if (hardwareList.Contains("Weighing Machine"))
                    hardwareList.Remove("Weighing Machine");
            }

            if (!IsWMConnected && useWeighingScale)
            {
                AppendLog("Waiting For Weighing Machine To be connected");
                if (!hardwareList.Contains("Weighing Machine"))
                    hardwareList.Add("Weighing Machine");
                showIndicator("Wx");
            }

        }

        #endregion
    }

    public interface ISync
    {
        void SyncProduct();

        void SyncSales();

        void SyncMasterData();

        void SyncUserData();

        void SyncLogData();
    }

    public interface IWeighingMachine
    {
        WeighingMachineController GetWeightingMachineController();
    }

    public interface INetsMachine
    {
        void GetWeightingMachineController();
    }

    public interface ICashMachine
    {
        void GetWeightingMachineController();
    }

    public delegate void StateChangeHandler(object sender, StateChangeAgrs e);

    public class StateChangeAgrs : EventArgs
    {
        public string PreviousState { set; get; }
        public string CurrentState { set; get; }
    }
}
