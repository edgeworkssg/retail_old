using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Runtime.InteropServices;
using System.Windows.Input;
using PowerPOS.Container;
using WinPowerPOS.OrderForms;
using System.IO;
using System.Reflection;
using PowerPOS.SalesController;
using System.Configuration;
using Features = PowerPOS.Feature;
using POSDevices;
using PowerPOSLib.Controller;
using System.Threading;
using SubSonic;
using WeighingMachine;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WinPowerPOS.KioskForms
{
    public partial class frmKiosk : Form, IKiosk, IScanWeight, IObserver
    {
        #region *) VARIABLE DECLARATION

        private POSController pos;
        private ItemController itemLogic;

        private String status;
        private decimal PreferedDiscount = new decimal(0.0d);
        private bool ApplyPromo = true;
        private bool PrintReceipt = true;

        private decimal paidAmount = new decimal(0.0d);
        private decimal change = new decimal(0.0d);

        private bool isSuccessful = false;
        private decimal amount = new decimal(0.0d);

        private bool isCheckout = false;
        private bool isRetry = true;
        private bool canScan = true;
        private bool showKeyCode = false;

        private bool isPaymentSuccessful = false;
        private decimal paymentAmount = 0;
        private string paymentType = "";
        private string newPaymentType = "";

        private Dictionary<string, int> mapItemWeight;
        private int sumOfDiffMinus = 0;
        private bool isWaitingForBagging = false;
        private bool isValidWeight = true;
        private int weightTolerance;
        private int lastWeight = 0;
        private int newWeight = 0;

        private string lastScannedBarcode = "";
        private string lastError = "";
        private int tmpWeight = 0;
        private int idleCounter = 0;
        private int idleInterval = 0;
        private bool isExit = false;

        private bool isCheckoutKiosk = false;
        private bool isUserEligibleForCheckOut = false;

        public bool isAgeVerified = false;
        private int verifiedAge = 0;

        public bool isStart = false;
        private bool useWeighingScale =false;


        public const string ROUNDING_ITEM = "R0001";

        //for change timer :
        private int changeInterval;

        public BarcodeScannerController barcodeScanner = null;
        private bool isBarcodeScannerEnabled = false;
        private BarcodeScanState _barcodeState = BarcodeScanState.ScanItem;
        public BarcodeScanState BarcodeState
        {
            get { return _barcodeState; }
            set
            {
                _barcodeState = value;
                AppendLog(string.Format("Barcode scanner state changed to : " + _barcodeState));
            }
        }
        private string lastItemNo = "";

        private bool isUseBarcodeScanner = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.Enabled), false);

        private frmSimulateCashPayment frmSimulateCashPayment;
        private frmSimulateNets frmSimulateNets;
        private Dictionary<string, string> dictLanguages = new Dictionary<string, string>();
        private WeighingMachineController wmController;
        private frmStartKiosk frmStartKiosk;

        private UserMst _currentUser = new UserMst();
        public UserMst CurrentUser
        {
            get { return _currentUser; }
            set 
            {
                _currentUser = value;
                if (_currentUser == null)
                    _currentUser = new UserMst();

                //hostHelp.Visible = !_currentUser.IsNew;
                hostLogOut.Visible = !_currentUser.IsNew;
                lblUserName.Visible = !_currentUser.IsNew;
                if (!_currentUser.IsNew)
                    lblUserName.Text = string.Format("Welcome {0}!", _currentUser.DisplayName);
                else
                    lblUserName.Text = "";
                lvOrders.ShowQtyColumn(!_currentUser.IsNew);
            }
        }

        public bool IsStaffLoggedIn
        {
            get
            {
                return CurrentUser != null && !CurrentUser.IsNew;
            }
        }

        //VIDEO STATE CONSTANT
        public class VideoState
        {
            public const string VideoScanFirst = "frmKiosk.VideoScanFirst";
            public const string VideoCash = "frmKiosk.VideoCash";
            public const string VideoPutBack = "frmKiosk.VideoPutBack";
            public const string VideoBagging = "frmKiosk.VideoBagging";
            public const string VideoChange = "frmKiosk.VideoChange";
            public const string VideoNETS = "frmKiosk.VideoNETS";
            public const string VideoProcessing = "frmKiosk.VideoProcessing";
            public const string VideoReceipt = "frmKiosk.VideoReceipt";
            public const string VideoScan = "frmKiosk.VideoScan";
            public const string VideoScanNRIC = "frmKiosk.VideoScanNRIC";
            public const string VideoScanStaff = "frmKiosk.VideoScanStaff";
            public const string VideoWaitForRemoval = "frmKiosk.VideoWaitForRemoval";
        }

        public enum BarcodeScanState
        {
            ScanItem,
            ScanVerifyNRIC,
            ScanVerifyStaffCard,
            ScanStaffCard,
            ScanReceipt
        }

        private AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        private bool isScanningCompleted = false;
        private bool isPromptShowing = false;


        #endregion

        public frmKiosk(frmStartKiosk frmStartKiosk)
        {
            InitializeComponent();

            #region *) Load Language

            string langID = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
            langID = String.IsNullOrEmpty(langID) ? "ENG" : langID;

            QueryCommand qc = new QueryCommand("SELECT ID, " + langID + " FROM TEXT_LANGUAGE WHERE ID LIKE @ID", "PowerPOS");
            qc.AddParameter("ID", "frmKiosk%");

            IDataReader rdr = DataService.GetReader(qc);
            while (rdr.Read())
                dictLanguages.Add(rdr["ID"].ToString(), rdr[langID].ToString());

            #endregion

            #region *) Init Controller

            pos = new POSController();
            itemLogic = new ItemController();
            //barcodeScanner = new BarcodeScannerController();
            
            
            ctrlScanner.CancelClick += new EventHandler(ctrlScanner_CancelClick);
            #endregion

            #region *) Load Rounding Preference that the customer like

            if (AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference") != null)
                POSController.RoundingPreference = AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference").ToString();
            else
                POSController.RoundingPreference = "";

            #endregion

            #region *) Init Button

            ctrlKeyCode.ButtonText = "Key Code";
            ctrlKeyCode.ButtonImage = "KioskForms/keyCodeIcon.png";
            ctrlKeyCode.ControlClick += new EventHandler(ctrlKeyCode_ControlClick);

            ctrlStaff.ButtonText = "Staff\nFunction";
            ctrlStaff.ControlClick += new EventHandler(ctrlStaff_ControlClick);

            ctrlLogOut.ButtonText = "Log Out";
            ctrlLogOut.ControlClick += new EventHandler(ctrlLogOut_ControlClick);
            ctrlLogOut.ButtonColor = "#CC5151";
            ctrlLogOut.ButtonHoverColor = "#CC7070";
            ctrlLogOut.ButtonPressColor = "#A54242";


            ctrlReprint.ButtonText = "Reprint Last Receipt";
            ctrlReprint.ControlClick += new EventHandler(ctrlReprint_ControlClick);

            ctrlResetBagging.ButtonText = "Reset\nBagging Area";
            ctrlResetBagging.ButtonColor = "#CCA351";
            ctrlResetBagging.ButtonHoverColor = "#F2C160";
            ctrlResetBagging.ButtonPressColor = "#CB9933";
            ctrlResetBagging.ControlClick += new EventHandler(ctrlResetBagging_ControlClick);

            ctrlResetTransaction.ButtonText = "Reset Transaction";
            ctrlResetTransaction.ButtonColor = "#FF6633";
            ctrlResetTransaction.ButtonHoverColor = "#FE8259";
            ctrlResetTransaction.ButtonPressColor = "#D83E0A";
            ctrlResetTransaction.ControlClick += new EventHandler(ctrlResetTransaction_ControlClick);

            ctrlCloseStaffFunction.ButtonText = "X";
            ctrlCloseStaffFunction.ButtonColor = "#CC5151";
            ctrlCloseStaffFunction.ButtonHoverColor = "#CC7070";
            ctrlCloseStaffFunction.ButtonPressColor = "#A54242";
            ctrlCloseStaffFunction.ControlClick += new EventHandler(ctrlCloseStaffFunction_ControlClick);

            ctrlCheckOutKiosk.ButtonText = "Check Out";
            ctrlCheckOutKiosk.ButtonColor = "#CC5151";
            ctrlCheckOutKiosk.ButtonHoverColor = "#CC7070";
            ctrlCheckOutKiosk.ButtonPressColor = "#A54242";
            ctrlCheckOutKiosk.ControlClick += new EventHandler(ctrlCheckOut_ControlClick);

            #endregion

            #region *) Attach frmStartKiosk

            this.frmStartKiosk = frmStartKiosk;
            this.frmStartKiosk.Attach(this);
            //this.frmStartKiosk.StateChanged += new StateChangeHandler(frmStartKiosk_StateChanged);

            //Avoid double assignment for the handler
            //this.frmStartKiosk.SubStateChanged -= new StateChangeHandler(frmStartKiosk_SubStateChanged);
            //this.frmStartKiosk.SubStateChanged += new StateChangeHandler(frmStartKiosk_SubStateChanged);
            
            this.frmStartKiosk.SubStateChanged += frmStartKiosk_SubStateChanged;

            #endregion

            #region *) Register Events

            lvOrders.OnKeyDown += new OrderList.OnKeyDownEventHandler(lvOrders_OnKeyDown);
            lvOrders.OnSelectedItem += new OrderList.OnSelectedItemEventHandler(lvOrders_OnSelectedItem);
            lvOrders.OnQtyChanged += new OrderList.OnQtyChangedHandler(lvOrders_OnQtyDecreased);
            /*lvOrders.OnLayoutUpdated += new OrderList.OnLayoutUpdatedHandler(lvOrders_OnLayoutUpdated);
            lvOrders.OnTargetUpdated += new OrderList.OnTargetUpdatedHandler(lvOrders_OnTargetUpdated);
            lvOrders.OnLoaded += new OrderList.OnLoadedHandler(lvOrders_OnLoaded);*/

            btnCheckOut.OnClicked += new CheckOutButton.OnClickEventHandler(btnCheckOut_OnClicked);
            btnCancel.OnClicked += new CancelButton.OnClickEventHandler(btnCancel_OnClicked);

            paymentPanel.OnCashPaymentClicked += new PaymentPanel.OnClickEventHandler(paymentPanel_OnCashPaymentClicked);
            paymentPanel.OnNetsPaymentClicked += new PaymentPanel.OnClickEventHandler(paymentPanel_OnNetsPaymentClicked);
            paymentPanel.OnNetsFlashPaymentClicked += new PaymentPanel.OnClickEventHandler(paymentPanel_OnNetsFlashPaymentClicked);
            paymentPanel.OnNetsCashCardPaymentClicked += new PaymentPanel.OnClickEventHandler(paymentPanel_OnNetsCashCardPaymentClicked);
            paymentPanel.OnCreditCardPaymentClicked += new PaymentPanel.OnClickEventHandler(paymentPanel_OnCreditCardPaymentClicked);

            btnSelectOtherPayment.OnClicked += new SelectOtherPaymentButton.OnClickEventHandler(btnSelectOtherPayment_OnClicked);

            keyCode.OnClicked += new KeyCode.OnClickEventHandler(keyCode_OnClicked);

            txtBarcode.LostFocus += new EventHandler(txtBarcode_LostFocus);

            #endregion

            #region *) Payment Simulator Mode

            frmSimulateCashPayment = new frmSimulateCashPayment(this);
            frmSimulateCashPayment.Visible = false;

            frmSimulateNets = new frmSimulateNets(this);
            frmSimulateNets.Visible = false;

            panel22.Visible = false;

            #endregion

            #region *) Init Weighing Machine

            wmController = ((IWeighingMachine)frmStartKiosk).GetWeightingMachineController();
            
            wmController.WeightChanged += new WeightChangedHandler(wmController_WeightChanged);
            wmController.StatusChanged += new StatusChangedHandler(wmController_StatusChanged);

            #endregion

            #region *) Simulator Mode

            frmStartKiosk.frmSimulatorWeightScale.WeightChanged += new WeightChangedHandler(wmController_WeightChanged);

            #endregion

            #region *) Init Variable

            mapItemWeight = new Dictionary<string, int>();

            isWaitingForBagging = false;
            sumOfDiffMinus = 0;
            isValidWeight = true;
            canScan = true;

            weightTolerance = 0;
            Int32.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance), out weightTolerance);
            idleInterval = (AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.IdleTimes) + "").GetIntValue();
            idleCounter = 0;

            CurrentUser = new UserMst();

            btnLog.Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.HideLogButton), false);
            #endregion
        }

        

        #region *) METHOD

        //private void InitBarcodeScanner()
        //{
        //    if (isUseBarcodeScanner)
        //    {
        //        AppendLog(string.Format("Init Barcode scanner on port : " + barcodeScanner.COMPort));
        //        string barcodeStatus = "";
        //        if (!barcodeScanner.init(out barcodeStatus))
        //            AppendLog("Init barcode failed. " + barcodeStatus);
        //    }
        //}

        

        private void DoRounding()
        {
            #region *) Do Rounding
            {
                decimal roundAmt = pos.RoundTotalReceiptAmount();
                decimal actualAmt = pos.CalculateTotalAmount(out status);

                //insert rounding inventory item....
                if (roundAmt != actualAmt)
                {
                    pos.AddItemToOrder
                        (new Item(ROUNDING_ITEM), 1, 0, false, out status);

                    pos.ChangeOrderLineUnitPrice(
                        pos.IsItemIsInOrderLine(ROUNDING_ITEM),
                        roundAmt - actualAmt,
                        out status);
                }
            }
            #endregion
        }

        private bool IsValidStaffToken(string barcode, out UserMst um)
        {
            bool isSuccess = true;

            um = new UserMst(UserMst.UserColumns.BarcodeToken, barcode);
            if (um.IsNew)
            {
                AppendLog(string.Format("Invalid staff token {0}", barcode));

                frmWrapMessage frmWrapMessage = new frmWrapMessage("Ask for assistance...", "Invalid Token");
                frmWrapMessage.ShowDialog(this);
                isSuccess = false;
            }

            return isSuccess;
        }

        private bool DoStaffLogin(string barcode)
        {
            bool isSuccess = true;

            try
            {
                UserMst um = new UserMst();

                if (IsValidStaffToken(barcode, out um))
                {
                    if (!um.IsHavePrivilegesFor("Kiosk Staff Function"))
                    {
                        AppendLog(string.Format("Insuficient Privilege for staff {0} to open staff function", um.UserName));

                        frmWrapMessage frmWrapMessage = new frmWrapMessage("Ask for assistance...", "Insuficient Privilege");
                        frmWrapMessage.ShowDialog(this);
                        isSuccess = false;
                    }
                    if (!um.IsHavePrivilegesFor(PrivilegesController.CLOSE_COUNTER))
                    {
                        hostKioskStaff.Visible = false;
                        ctrlCheckOutKiosk.Visibility = System.Windows.Visibility.Hidden;
                    }


                    AppendLog(string.Format("Staff loggin in : {0}", um.UserName));
                    CurrentUser = um;
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                    frmStartKiosk_SubStateChanged(frmStartKiosk, new StateChangeAgrs { CurrentState = frmStartKiosk.SubState, PreviousState = frmStartKiosk.SubState });
                }

            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        public void WaitBaggingArea()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false))
            {
                //AppendLog("WEIGHING MACHINE STATUS = " + wmStatus + "\n");
                string wmStatus = wmController.getStatus();

                if (frmStartKiosk.SimulatorWeighingScale)
                    wmStatus = frmStartKiosk.frmSimulatorWeightScale.getStatus();

                if (wmStatus.ToUpper() == "CONNECTED")
                {
                    canScan = false;

                    isWaitingForBagging = true;

                    frmStartKiosk.SubState = "Wait For Bagging";

                    AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);
                    AppendLog("Barcode Scanning & Keycode & CHECKOUT are disabled");

                    showVideo(VideoState.VideoBagging);
                    //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\bagging.wmv"), true, GetText("frmKiosk.VideoBagging"));
                    //AppendLog("Play Video bagging.wmv");

                    return;
                }
                else
                {
                    frmStartKiosk.SubState = "Wait For Item Scan";

                    if (frmStartKiosk.SimulatorWeighingScale)
                        AppendLog("Weighing Scale Simulator is disconnected, Skip Weight Check");
                    else
                        AppendLog("Weighing Scale is disconnected, Skip Weight Check");

                    AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);
                    frmStartKiosk_SubStateChanged(frmStartKiosk, new StateChangeAgrs { CurrentState = frmStartKiosk.SubState, PreviousState = frmStartKiosk.SubState });
                    
                }
            }
            else
            {
                frmStartKiosk.SubState = "Wait For Item Scan";
               
                 AppendLog("Weighing Scale Checking is disabled, Skip Weight Check");
                

                AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);
                frmStartKiosk_SubStateChanged(frmStartKiosk, new StateChangeAgrs { CurrentState = frmStartKiosk.SubState, PreviousState = frmStartKiosk.SubState });
            }
        }

        public int SaveItemWeight(string barcode, int weight)
        {
            int sample = 10;
            Int32.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.SampleWeighingScale), out sample);

            QueryCommand cmd = new QueryCommand("SELECT Barcode, ActualWeight, LastWeight, Min, Max, Avg, Count, PointOfSaleID FROM ItemWeight WHERE Barcode = @Barcode", "PowerPOS");
            cmd.AddParameter("Barcode", barcode);

            IDataReader rdr = DataService.GetReader(cmd);

            if (!rdr.Read())
            {
                /*//AppendLog("INSERT ITEMWEIGHT " + barcode + "," + 0 + "," + weight + "," + weight + "," + weight + "," + weight + "," + 1 + "," + PointOfSaleInfo.PointOfSaleID);

                QueryCommand cmd2 = new QueryCommand("INSERT INTO ItemWeight (Barcode, ActualWeight, LastWeight, Min, Max, Avg, Count, PointOfSaleID) VALUES (@Barcode, @ActualWeight, @LastWeight, @Min, @Max, @Avg, @Count, @PointOfSaleID)", "PowerPOS");
                cmd2.AddParameter("Barcode", barcode);
                cmd2.AddParameter("ActualWeight", 0);
                cmd2.AddParameter("LastWeight", weight);
                cmd2.AddParameter("Min", weight);
                cmd2.AddParameter("Max", weight);
                cmd2.AddParameter("Avg", weight);
                cmd2.AddParameter("Count", 1);
                cmd2.AddParameter("PointOfSaleID", PointOfSaleInfo.PointOfSaleID);

                DataService.ExecuteQuery(cmd2);*/
            }
            else
            {
                int min = int.Parse(rdr["Min"].ToString());
                int max = int.Parse(rdr["Max"].ToString());
                int avg = int.Parse(rdr["Avg"].ToString());
                int count = int.Parse(rdr["Count"].ToString());
                int actualWeight = int.Parse(rdr["ActualWeight"].ToString());

                {
                    min = Math.Min(min, weight);
                    max = Math.Max(max, weight);
                    avg = (int)Math.Round(((count * avg) + weight) / (decimal)(count + 1));
                    count = Math.Min(1000, (count + 1));

                    //AppendLog("UPDATE ITEMWEIGHT " + barcode + "," + weight + "," + actualWeight + "," + min + "," + max + "," + avg + "," + count + "," + PointOfSaleInfo.PointOfSaleID);

                    /*QueryCommand cmd2 = new QueryCommand("UPDATE ItemWeight SET LastWeight = @LastWeight, ActualWeight = @ActualWeight, Min = @Min, Max = @Max, Avg = @Avg, Count = @Count WHERE Barcode = @Barcode AND PointOfSaleID = @PointOfSaleID", "PowerPOS");
                    cmd2.AddParameter("Barcode", barcode);
                    cmd2.AddParameter("LastWeight", weight);
                    cmd2.AddParameter("ActualWeight", actualWeight);
                    cmd2.AddParameter("Min", min);
                    cmd2.AddParameter("Max", max);
                    cmd2.AddParameter("Avg", avg);
                    cmd2.AddParameter("Count", count);
                    cmd2.AddParameter("PointOfSaleID", PointOfSaleInfo.PointOfSaleID);

                    DataService.ExecuteQuery(cmd2);*/
                }

                //{
                //    if (actualWeight == 0 && count > sample)
                //        actualWeight = avg;

                //    //AppendLog("UPDATE ITEMWEIGHT " + barcode + " SET ActualWeight " + actualWeight);

                //    QueryCommand cmd2 = new QueryCommand("UPDATE ItemWeight SET ActualWeight = @ActualWeight WHERE Barcode = @Barcode AND PointOfSaleID = @PointOfSaleID", "PowerPOS");
                //    cmd2.AddParameter("Barcode", barcode);
                //    cmd2.AddParameter("ActualWeight", actualWeight);
                //    cmd2.AddParameter("PointOfSaleID", PointOfSaleInfo.PointOfSaleID);

                //    DataService.ExecuteQuery(cmd2);
                //}

                if (count <= sample)
                {
                    //AppendLog("SaveItemWeight RESULT = " + 0);
                    return 0;
                }

                int tmp = actualWeight > 0 && Math.Abs(weight - actualWeight) / actualWeight * 100 <= 4 ? 1 : -1;
                //AppendLog("SaveItemWeight RESULT = " + tmp);

                return tmp;
            }

            return 0;
        }

        public void BindGrid()
        {
            //AppendLog("BindGrid is called");
            
            lvOrders.SetDataSource(pos.FetchUnSavedOrderItemsForGrid(out status));
            //lvOrders.Focus();
            label3.Text = "TAX: $" + string.Format("{0:N2}", pos.calculateTotalGST());
            label6.Text = "TOTAL: $" + string.Format("{0:N2}", pos.CalculateTotalAmount(out status));

            this.Invalidate();
        }

        public void OpenStaffFunction()
        {

            //Check Current State and show the panel button accordingly
            if (frmStartKiosk.State == "CHECKOUT")
            {
                hostReprint.Visible = false;
                hostHostBagging.Visible = false;
                hostCheckOut.Visible = false;
            }
            else
            {
                hostReprint.Visible = true;
                hostHostBagging.Visible = true;
                hostCheckOut.Visible = true;
            }

            int x = (this.Width / 2) - (pnlStaffFunction.Width / 2);
            int y = (this.Height / 2) - (pnlStaffFunction.Height / 2);
            pnlStaffFunction.Location = new Point(x, y);
            pnlStaffFunction.Visible = true;
            pnlStaffFunction.BringToFront();
            hostKeyCode.Visible = true;

            
        }

        public void CloseStaffFunction()
        {
            BarcodeState = BarcodeScanState.ScanItem;
            pnlStaffFunction.Visible = false;
            pnlStaffFunction.SendToBack();
            StateInit();
        }

        public void CloseStaffFunctionExit()
        {
            //BarcodeState = BarcodeScanState.ScanItem;
            pnlStaffFunction.Visible = false;
            pnlStaffFunction.SendToBack();
            hostKeyCode.Visible = false;
            //if ((frmStartKiosk.SubState + "").ToLower().Equals("wait for item scan"))
            //    RunBarcodeWorker();
        }

        /// <summary>
        /// Show panel to ask to scan staff card / IC, Turn on Barcode Scanner 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void OpenBarcodeScanner(string title, string message)
        {
            
            isPromptShowing = true;
            AppendLog("Disable Timer Idle");
            enabledTimerIdle(false);
            EnableBarcodeScanner(true);

            int x = (this.Width / 2) - (hostScanner.Width / 2);
            int y = (this.Height / 2) - (hostScanner.Height / 2);
            ctrlScanner.Title = title;
            ctrlScanner.Message = message;

            hostScanner.Location = new Point(x, y);
            hostScanner.Visible = true;
            hostScanner.BringToFront();

            txtBarcode.Text = "";
            txtBarcode.Focus();
        }

        /// <summary>
        /// hide panel to ask to scan staff card / IC, Turn off Barcode Scanner  
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void CloseBarcodeScanner()
        {
            barcodeScanner.disableDevice();
            
            BarcodeState = BarcodeScanState.ScanItem;
            hostScanner.Visible = false;
            hostScanner.SendToBack();

            //handle when scanner form is closed. check to enable barcode scanner.
            scanPromptClosed();
        }

        #endregion

        #region *) FORM EVENTS

        private void frmStartKiosk_SubStateChanged(object sender, StateChangeAgrs e)
        {
            /*try
            {
                if ((e.CurrentState + "").ToLower().Equals("wait for item scan") && lastError == "")
                {
                    EnableBarcodeScanner(true);
                }
            }
            catch (Exception ex)
            {
                AppendLog(string.Format("Error after substate changed to {0} : {1}", e.CurrentState, ex.Message));
                Logger.writeLog(ex);
            }*/
        }

        private bool isPromptShown()
        {
            // waiting for staff login / prompt
            /*if ( pnlStaffFunction.Visible)
                return true;
            //waiting ic verification
            //if (frmStartKiosk.SubState == "Wait authorization")
            //    return true;

            //Ask to scan NRIC or Staff Card
            if (hostScanner.Visible == true)
                return true;

            //waiting key code
            if (showKeyCode)
                return true;
            
            // no prompt
                return isProm;*/
            return isPromptShowing;
        }


        private void frmKiosk_Shown(object sender, EventArgs e)
        {
            //AppendLog("Form Activated. isStart = " + isStart.ToString());
            #region init variable


            lastWeight = 0;
            newWeight = 0;

            lastScannedBarcode = "";
            lastError = "";
            tmpWeight = 0;
            isExit = false;

            isWaitingForBagging = false;
            sumOfDiffMinus = 0;
            isValidWeight = true;
            canScan = true;

            lastWeight = 0;

            weightTolerance = 0;
            Int32.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance), out weightTolerance);
            idleInterval = (AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.IdleTimes) + "").GetIntValue();
            idleCounter = 0;

            CurrentUser = new UserMst();

            btnLog.Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.HideLogButton), false);
            #endregion 

            txtBarcode.Focus();

            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = false;

            panelKeyCode.Visible = true;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = false;

            panel9.Visible = true;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = false;
            panel22.Visible = false;

            hostKeyCode.Visible = false;

            showVideo(VideoState.VideoScan);
            //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\scan.wmv"), true, GetText("frmKiosk.VideoScan"), 36);
            //AppendLog("Play VIdeo scan.wmv");
            timerIdle.Start();
            idleCounter = 0;
            BarcodeState = BarcodeScanState.ScanItem;
            EnableBarcodeScanner(true);
            //barcodeScanner.disableDevice();
            //bwBarcode.RunWorkerAsync();
            //InitBarcodeScanner();
            //this.BringToFront();
            txtBarcode.Focus();
        }

        private void frmKiosk_FormClosed(object sender, FormClosedEventArgs e)
        {
            //barcodeScanner.disableDevice();
            //barcodeScanner.Dispose();

            //this.frmStartKiosk.StartVideo();

            //wmController.WeightChanged -= wmController_WeightChanged;

            #region Simulator Mode

            frmStartKiosk.frmSimulatorWeightScale.WeightChanged -= wmController_WeightChanged;


            #endregion
            //remove the 
            //this.frmStartKiosk.SubStateChanged -= frmStartKiosk_SubStateChanged;

            //this.frmStartKiosk.Detach(this);
            //if(wmController!=null)
            //    wmController.Dispose();

            //AppendLog("=== END KIOSK Service with pending items cart (force close or error is occured) ===");
        }

        private void frmKiosk_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();
                barcodeScanner.disableDevice();
                //this.frmStartKiosk.SubStateChanged -= frmStartKiosk_SubStateChanged;
                this.frmStartKiosk.StartVideo();
                e.Cancel = true;
                /*if (isBarcodeScannerEnabled)
                {
                    barcodeScanner.isActive = false;
                    isExit = true;
                    e.Cancel = true;
                }*/
                /*if (isUseBarcodeScanner)
                {
                    AppendLog("Disabling barcode scanner");
                    if (barcodeScanner != null)
                        barcodeScanner.disableDevice();
                }*/
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void lvOrders_OnQtyDecreased(object sender, QtyChangeArgs args)
        {
            if (frmStartKiosk.SubState != frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN)
                return;

            AppendLog(string.Format("Change qty of item {0} to {1}", args.ItemName, args.Quantity));

            string status = "";
            bool result = true;
            var oDet = pos.myOrderDet.Where(o => o.OrderDetID == args.ID).FirstOrDefault();
            if (args.Quantity == 0)
                pos.RemoveLine(oDet);
            else
                result = pos.ChangeOrderLineQuantity(args.ID, args.Quantity, false, out status);

            if (!result)
            {
                AppendLog(string.Format("Change qty item {0} failed : {1}", args.ItemName, status));
                return;
            }
            else
                BindGrid();

            frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORREMOVAL;
            StateInit();

            
        }

        private void lvOrders_OnSelectedItem(object sender, MouseButtonEventArgs args)
        {
            if (!canScan)
                return;

            if (isCheckout)
                return;

            if (!IsStaffLoggedIn)
                return;

            if (frmStartKiosk.State != frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState != frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN)
                return;

            var item = sender as System.Windows.Controls.ListViewItem;
            if (item != null && item.IsSelected)
            {
                var r = item.Content as DataRowView;
                frmChangeQuantity frmDlg = new frmChangeQuantity(pos, r);
                frmDlg.ShowDialog(this);
                BindGrid();


                if (frmDlg.isDecreaseQty)
                {
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORREMOVAL;
                    StateInit();
                    return;
                }

                /*if (frmDlg.isIncreaseQty)
                {
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORBAGGING;
                    StateInit();
                    return;
                }*/
                
            }
        }

        private void lvOrders_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs args)
        {
            txtBarcode.Focus();
            txtBarcode.Text = String.Format("{0}", GetCharFromKey(args.Key));
            txtBarcode.Text = txtBarcode.Text.Trim();

            // little bit trick
            txtBarcode.SelectionStart = txtBarcode.Text.Length + 1; // add some logic if length is 0
            txtBarcode.SelectionLength = 0;
        }

        

        /*private void lvOrders_OnLayoutUpdated(object sender)
        {
            AppendLog("Grid Layout Updated");
        }

        private void lvOrders_OnTargetUpdated(object sender)
        {
            AppendLog("Grid TArget Updated");
        }

        private void lvOrders_OnLoaded(object sender)
        {
            AppendLog("Grid Loaded");
        }*/

        private void ctrlLogOut_ControlClick(object sender, EventArgs e)
        {
            hostKeyCode.Visible = false;
            AppendLog(string.Format("Staff {0} logout", CurrentUser.UserName));
            CurrentUser = new UserMst();
        }

        private void ctrlHelp_ControlClick(object sender, EventArgs e)
        {

        }

        private void enableKeyCodeInput(bool enabled)
        {
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = false;
            panelKeyCode.Visible = !enabled;
            panel23.Visible = enabled;

            if (enabled)
                AppendLog("Keycode is shown");
            else
                AppendLog("Keycode is hidden");
        }

        private void ctrlKeyCode_ControlClick(object sender, EventArgs e)
        {
            //if (!canScan)
            //    return;

            //disable Barcode Scanner
            AppendLog("User Clicked Keycode button in order page");

            if (!showKeyCode)
            {  //Keycode is shown
                AppendLog("Barcode Scanner Disabled");
                EnableBarcodeKeycodeCheckOut(false);
                
                enableKeyCodeInput(true);
                isPromptShowing = true;
                AppendLog("Disable Timer Idle");
                enabledTimerIdle(false);
                //stopVideo();
                showKeyCode = !showKeyCode;

            }
            else
            {// Keycode is closed 


                enableKeyCodeInput(false);
                //isPromptShowing = false;
                showVideo(VideoState.VideoScan);
                showKeyCode = !showKeyCode;

                //Check enabled barcode scanner when prompt is closed and last error = "" 
                scanPromptClosed();  
                
            }
        }

        private void ctrlStaff_ControlClick(object sender, EventArgs e)
        {
            if (frmStartKiosk.State == "CHECKOUT" && frmStartKiosk.SubState == "Waiting For Change")
                return;

            if (IsStaffLoggedIn)
                OpenStaffFunction();
            else
            {
                if (BarcodeState != BarcodeScanState.ScanStaffCard)
                {
                    BarcodeState = BarcodeScanState.ScanStaffCard;
                    OpenBarcodeScanner("Staff Access", "Please scan your staff card");
                }
            }
        }

        private void ctrlScanner_CancelClick(object sender, EventArgs e)
        {
            CloseBarcodeScanner();
            if (BarcodeState == BarcodeScanState.ScanVerifyNRIC || BarcodeState == BarcodeScanState.ScanVerifyStaffCard)
            {
                AppendLog("User press cancel");
                AppendLog("Item is removed");
            }
            scanPromptClosed();
            
            //frmStartKiosk_SubStateChanged(frmStartKiosk, new StateChangeAgrs { CurrentState = frmStartKiosk.SubState, PreviousState = frmStartKiosk.SubState });
            //autoResetEvent.Set();
        }
        #region Staff Function

        private void ctrlCloseStaffFunction_ControlClick(object sender, EventArgs e)
        {
            CloseStaffFunction();
        }

        private void ctrlResetTransaction_ControlClick(object sender, EventArgs e)
        {
            
            
            AppendLog(string.Format("Staff {0} reset transaction", CurrentUser.UserName));
            
            //EnableBarcodeScanner(false);
            
            if (frmStartKiosk.State == "CHECKOUT" && isCashPayment)
            {
                //if the state is checkout and have cash payment then need to reset the cash machine state  
                isCashPayment = false;
                CloseStaffFunctionExit();
                isExit = true;

                return;
            }

            ResetTransactionAndBackToStartScreen();
            
        }

        private void ResetTransactionAndBackToStartScreen()
        {
            CloseStaffFunctionExit();
            //CloseBarcodeScanner();
            pos = new POSController();
            itemLogic = new ItemController();

            AppendLog("Disable Timer Idle");
            enabledTimerIdle(false);
            CurrentUser = new UserMst();
            AppendLog("Reset Last Error to Empty");
            lastError = "";

            isExit = true;
            BindGrid();
            frmStartKiosk.State = frmStartKiosk.KioskState.STANDBY;
            frmStartKiosk.SubState = frmStartKiosk.KioskState.STANDBY_WAITFORREADY;
            frmStartKiosk.StateInit();
            this.Close();
        }

        private void ctrlResetBagging_ControlClick(object sender, EventArgs e)
        {
            CloseStaffFunction();

            AppendLog("Reset bagging area (Last Weight = " + newWeight.ToString() + ")");
            lastWeight = newWeight;

            AppendLog("Reset Last Error = ''");
            lastError = "";
            
            frmStartKiosk.State = frmStartKiosk.KioskState.PROCESSING;
            frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
            StateInit();
            /*if (frmStartKiosk.State == "PROCESSING" && frmStartKiosk.SubState == "Wait For Bagging")
                SetWeightResult(true, this.newWeight);
            else
                wmController_WeightChanged(wmController, new WeightChangedArgs(lastWeight, lastWeight));*/

        }

        private void ctrlReprint_ControlClick(object sender, EventArgs e)
        {
            CloseStaffFunction();

            AppendLog(string.Format("Staff {0} reprint last transaction", CurrentUser.UserName));
            string orderHdrID = ReceiptController.LoadLastTransaction(PointOfSaleInfo.PointOfSaleID, UserInfo.username);
            AppendLog("Start Reprint: " + orderHdrID);

            POSController posReprint = new POSController(orderHdrID);
            //tryDownloadPoints(posReprint.GetMemberInfo().MembershipNo, "");
            if (posReprint != null && posReprint.GetSavedRefNo() != "")
            {
                //Logger.writeLog("After Download Points: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                POSDeviceController.PrintAHAVATransactionReceipt(posReprint, 0, true,
                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                PrintSettingInfo.receiptSetting.PaperSize.Value),
                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                AppendLog("Finish Print last order");
            }
            else
            {
                AppendLog("Last order not found");
            }
        }

        private void ctrlCheckOut_ControlClick(object sender, EventArgs e)
        {
            CloseStaffFunction();
            isCheckoutKiosk = true;
            //Open frmKiosk
            AppendLog("Check Out Page Opened");
            frmCheckOutKiosk fCheckOutKiosk = new frmCheckOutKiosk();
            fCheckOutKiosk.currentUser = CurrentUser;
            fCheckOutKiosk.fStartKiosk = frmStartKiosk;
            //fCheckOutKiosk.TopMost = true;
            DialogResult dr = fCheckOutKiosk.ShowDialog();
            isCheckoutKiosk = false;
            if (dr != DialogResult.OK)
            {
                AppendLog("Check Out Failed." +fCheckOutKiosk.status);
                return;
            }
            AppendLog("Checked Out Successfuly.");

            ResetPage();
            frmStartKiosk.State = frmStartKiosk.KioskState.STANDBY;
            frmStartKiosk.SubState = frmStartKiosk.KioskState.STANDBY_WAITFORREADY;
            frmStartKiosk.StateInit();
            this.Close();
            
            
        }
        #endregion

        public void barcodeScanner_DeviceStateChanged(BarcodeScannerController m, DeviceStateChangedArgs e)
        {
            isBarcodeScannerEnabled = e.IsEnabled;
            if (e.IsEnabled)
                AppendLog("Barcode scanner enabled");
            else
            {
                AppendLog("Barcode scanner disabled");
                /*if (isExit)
                    this.Close();*/
            }
        }
        

        
        private void wmController_StatusChanged(WeighingMachineController m, StatusChangedArgs e)
        {
            frmStartKiosk.IsWMConnected = e.currentStatus.ToUpper() == "CONNECTED";
            if (frmStartKiosk.State != frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.State != frmStartKiosk.KioskState.CHECKOUT)
                return;

            if (frmStartKiosk.IsWMConnected) // 2.B.1
            {
                if (frmStartKiosk.SimulatorWeighingScale)
                    AppendLog("Weighing Scale Simulator is connected");
                else
                    AppendLog("Hardware: Weighing Scale is connected");
                return;
            }

            if (!frmStartKiosk.IsWMConnected) // 2.B.1
            {
                if (frmStartKiosk.SimulatorWeighingScale)
                    AppendLog("Weighing Scale Simulator is disconnected");
                else
                    AppendLog("Hardware: Weighing Scale is disconnected");

                if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORBAGGING)
                {
                    AppendLog("Weighing Scale Disconnected. Skip Weight Check");
                    frmStartKiosk.SendCommand("PROCESSING|Wait For Bagging|Weighing Scale is Disconnected");
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    
                }

                if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORREMOVAL)
                {
                    AppendLog("Weighing Scale Disconnected. Skip Weight Check");
                    frmStartKiosk.SendCommand("PROCESSING|Wait For Removal|Weighing Scale is Disconnected");
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();

                }
                return;
            }


            

            
        }

        private void wmController_WeightChanged(WeighingMachineController m, WeightChangedArgs e)
        {
            if (e.diff == 0)
            {
                return;
            }

            if (pos == null)
                return;

            //Skip Checking Weight if weight checking is disabled
            if (!useWeighingScale)
            {
                return;
            }

            //Skip handling weight change because state is handled by start Page.
            if (frmStartKiosk.State == "DISABLED" || frmStartKiosk.State == "STANDBY")
            {
                return;
            }
                

            //After I scan the first barcode, the system will read current weight
            newWeight = e.currentWeight;

            // Ignore Insignificant Weight Changed
            if (KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance))
            {
                if (lastError == "")
                {
                    return;
                }
            }

            // Display Log for Weight Increase
            if (frmStartKiosk.SimulatorWeighingScale)
                AppendLog("Weight simulator changed to " + newWeight + "g (Last Weight " + lastWeight + "g)");
            else
                AppendLog("Weight changed to " + newWeight + "g (Last Weight " + lastWeight + "g)");

            

            if( frmStartKiosk.State == "CHECKOUT")
            {
                AppendLog("Ignore Weight Change during checkout");
            }

            if (frmStartKiosk.State == "PROCESSING" && frmStartKiosk.SubState == "Wait For Item Scan")
            {
                
                //if (Math.Abs(newWeight - lastWeight) > weightTolerance)
                //{

                

                //If new weight > Last Weight & no prompt
                if (KioskHelper.isNewWeightMoreThan(newWeight, lastWeight, weightTolerance))
                {
                    AppendLog("new weight > Last Weight, Prompt Please scan first");
                    lastError = VideoState.VideoScanFirst;
                    showVideo(lastError);
                    EnableKeyCode(false);
                    EnableCheckOut(false);
                    enabledTimerIdle(false);

                    if (!isPromptShown())
                        EnableBarcodeScanner(false);
                    else
                        AppendLog("Barcode scanner is not disabled because prompt is showing");

                    return;
                }

                //If new weight < Last Weight, Item Count > 0 & no prompt,
                if (KioskHelper.isNewWeightLessThan(newWeight, lastWeight, weightTolerance) && pos.GetNoOfItem() > 0 )
                {
                    AppendLog("new weight < Last Weight, Prompt Please return to bagging area");
                    lastError = VideoState.VideoPutBack;
                    showVideo(lastError);
                    EnableKeyCode(false);
                    EnableCheckOut(false);
                    enabledTimerIdle(false);

                    if (!isPromptShown())
                        EnableBarcodeScanner(false);
                    else
                        AppendLog("Barcode scanner is not disabled because prompt is showing");

                    return;
                }

                //If new weight <> Last Weight, Item Conut =0, Ignore
                /*if (!KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance) && pos.GetNoOfItem() == 0)
                {
                    AppendLog("No Item yet, ignore weight increase/decrease");
                }*/

                //If weight change during prompt for IC verification, prompt for cashier scan, prompt for KEYCODE SEARCH page, do Nothing
                /*if (!KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance) )
                {
                    lastError = "";
                    AppendLog("Reset Last Error To ''");
                    //showVideo(VideoState.VideoScan);

                    if (!isPromptShown())
                        StateInit();
                    else
                        AppendLog("Don't go to State Init PROCESSING (Wait For Item Scan) because prompt is showing");
                    
                }*/

                //If new weight = Last Weight, Reset Last Error = ""
                if (KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance))
                {
                    //to avoid looping..
                    if (lastError != "")
                    {
                        AppendLog("Weight returned to Last Weight");
                        lastError = "";
                        AppendLog("Reset Last Error To ''");
                        StateInit();
                    }
                    

                }
                return;
            }//End Of if (frmStartKiosk.State == "PROCESSING" && frmStartKiosk.SubState == "Wait For Item Scan")


            if (frmStartKiosk.State == "PROCESSING" && frmStartKiosk.SubState == "Wait For Bagging")
            {
                

                //if new weight < Last weight, Display Error = ""Return Item to Bagging Area"" , Weight Reduced
                if (KioskHelper.isNewWeightLessThan(newWeight, lastWeight, weightTolerance))
                {
                    AppendLog("Item was removed from Bagging Area");
                    lastError = VideoState.VideoPutBack;
                    showVideo(lastError);
                    EnableBarcodeKeycodeCheckOut(false);
                    return;
                }

                if (KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance))
                {
                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {
                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    showVideo(VideoState.VideoBagging);
                    AppendLog("Waiting for customer to put item to Bagging Area");

                }

                //Weight Already Increase Previously.. 
                if (KioskHelper.isNewWeightMoreThan(newWeight, lastWeight, weightTolerance))
                {


                    //Calculate item weight, record this weight for new item
                    SetWeightResult(true, newWeight - lastWeight);
                    AppendLog("Item is put on bagging area (item weight = " + (newWeight - lastWeight).ToString() + "g");

                    //Record this weight as Last Weight
                    lastWeight = newWeight;
                    AppendLog("Set Last Weight = " + lastWeight.ToString());

                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {

                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }


                
            } //End Of if (frmStartKiosk.State == "PROCESSING" && frmStartKiosk.SubState == "Wait For Bagging")

            if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORREMOVAL )
            {
                //if new weight < Last weight, Display Error = ""Return Item to Bagging Area"" , Weight Reduced
                if (KioskHelper.isNewWeightMoreThan(newWeight, lastWeight, weightTolerance))
                {
                    AppendLog("Pls Scan your item before putting in bagging area");
                    lastError = VideoState.VideoScanFirst;
                    showVideo(lastError);
                    //StateInit();
                    return;
                }

                if (KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance))
                {


                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {
                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    showVideo(VideoState.VideoWaitForRemoval);
                    AppendLog("Remove Item from Bagging Area");

                }

                //Weight Already Increase Previously.. 
                if (KioskHelper.isNewWeightLessThan(newWeight, lastWeight, weightTolerance))
                {


                    //Calculate item weight, record this weight for new item
                    SetWeightResult(true, newWeight - lastWeight);
                    AppendLog("Item is removed from bagging area (item weight = " + (newWeight - lastWeight).ToString() + "g");

                    //Record this weight as Last Weight
                    lastWeight = newWeight;
                    AppendLog("Set Last Weight = " + lastWeight.ToString());

                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {

                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }


                


            } // End Of if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORREMOVAL )
        }

        private void btnCancel_OnClicked(object sender, EventArgs args)
        {
            //AppendLog("CANCEL CHECKOUT IS CLICKED");

            if (bwNets.IsBusy)
            {
                frmWrapMessage frmWrapMessage = new frmWrapMessage("Please Wait", "Your transaction is still being procesed.");
                frmWrapMessage.ShowDialog(this);

                return;
            }

            //Check if have paid amount for cash
            /*if (pos.recDet.Count > 0)
            {
                if (pos.recDet[0].PaymentType == "CASH")
                {
                    string status = "";
                    if (!frmStartKiosk.cashMgr.CancelAndReturnCash(out status))
                    {
                        AppendLog(status);
                    }
                }
            }*/


            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = false;

            panelKeyCode.Visible = true;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = false;

            panel9.Visible = false;
            panel13.Visible = true;
            panel15.Visible = false;
            panel21.Visible = false;

            showVideo(VideoState.VideoScan);
            //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\scan.wmv"), true, GetText("frmKiosk.VideoScan"));
            //AppendLog("Play Video scan.wmv");

            isCheckout = false;

            frmStartKiosk.State = frmStartKiosk.KioskState.PROCESSING;
            frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
            StateInit();

            //AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);

            
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            frmStartKiosk.Notify();
            txtLog.Visible = !txtLog.Visible;

            txtLog.Text = "";

            txtLog.AppendText(String.Join(Environment.NewLine, frmStartKiosk.BufferLog.ToArray()));

            txtBarcode.Text = "";
            txtBarcode.Focus();
        }

        private void btnCheckOut_OnClicked(object sender, EventArgs args)
        {
            if (!canScan)
                return;

            //AppendLog("CHECKOUT IS CLICKED");

            //disable Barcode
            barcodeScanner.disableDevice();

            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = true;
            panelNetsPayment.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = true;
            panel21.Visible = false;

            ctrlVideo.Stop();

            RefreshPayment();

            isCheckout = true;
            paidAmount = 0;
            isCashPayment = false;


            
            #region *) Initialize: Update OrderTime to NOW
            pos.myOrderHdr.OrderDate = DateTime.Now;
            #endregion

            //if (Features.Package.isAvailable && pos.containPackages && !pos.MembershipApplied())
            //    throw new Exception("(warning)Please assign membership to continue");
            //if (pos.IsItemIsInOrderLine("INS_PAYMENT") != "" && !pos.MembershipApplied())
            //    throw new Exception("(warning)Please assign membership to continue");

            if (!pos.AssignMembership("WALK-IN", out status))
            {
                Logger.writeLog("Unable to assign walk in member: " + status);
                //AppendLog("Unable to assign walk in member: " + status);
            }

            //Hide or show btn cash for the setting
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableCoins), false) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.EnableNotes), false))
                paymentPanel.btnCash.Visibility = System.Windows.Visibility.Visible;
            else
                paymentPanel.btnCash.Visibility = System.Windows.Visibility.Hidden;

            //Hide btnCash if the machine is not available
            /*if (frmStartKiosk.cashMgr == null || !frmStartKiosk.cashMgr.isConnected)
            {
                paymentPanel.btnCash.Visibility = System.Windows.Visibility.Hidden;
            }*/

            //Clear Payment Type
            if (pos.recDet.Count > 0)
            {
                pos.recDet.Clear();
            }

            
            frmStartKiosk.State = "CHECKOUT";
            frmStartKiosk.SubState = "Wait For Payment Mode";

            AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);
        }

        private void btnSelectOtherPayment_OnClicked(object sender, EventArgs args)
        {
            //AppendLog("OTHER PAYMENT IS CLICKED");
            
            

            barcodeScanner.disableDevice();
            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = true;
            panelNetsPayment.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = paidAmount <= 0;
            panel21.Visible = false;

            videoControl2.Stop();

            frmSimulateCashPayment.Visible = false;
            frmSimulateNets.Visible = false;

            isRetry = false;

            //if paid amount > 0 then disable cash
            if (paidAmount > 0)
                paymentPanel.btnCash.Visibility = System.Windows.Visibility.Hidden;
            frmStartKiosk.SubState = "Select Other Payment";
            CancelAndKeepCash();
            
        }

        private bool isInsufficientCharge = false;

        private bool ReturnPaidAmount()
        {

            AppendLog("Returning Cash : " + paidAmount.ToString("N2"));
            /*if (!frmStartKiosk.cashMgr.Dispense(paidAmount, out status))
                return false;*/
            //summaryPanel.UpdateReturnedCash(paidAmount);
            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\change.wmv"), true, GetText("frmKiosk.VideoChange"));
            //AppendLog("Play Video change.wmv");
            AppendLog("Wait For Collecting Money");
            isInsufficientCharge = true;
            initTimerChange();
            return true;
        }

        private bool CancelAndKeepCash()
        {
            //If No payment yet and is using cash mgr
            //if (paidAmount <= 0)
            //    return true;

            /*if (frmStartKiosk.cashMgr != null)
            {
                if (frmStartKiosk.cashMgr.isAccepting)
                {
                    //cancel and return cash
                    if (!frmStartKiosk.cashMgr.CancelAndKeepCash(out status))
                    {
                        AppendLog("Cancel And Keep Cash Failed. " + status);
                        return false;
                    }
                }
            }*/
            return true;
        }

        private void txtBarcode_LostFocus(object sender, EventArgs e)
        {
            //if ((frmStartKiosk.SubState + "").ToLower().Equals("wait for item scan") ||
            //    (frmStartKiosk.SubState + "").ToLower().Equals("wait for staff card scan") ||
            //    (frmStartKiosk.SubState + "").ToLower().Equals("wait for nric scan"))
            //{
            //    txtBarcode.Focus();
            //}
        }

        private void txtBarcode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            #region *) Validation



            if (frmStartKiosk.State != "PROCESSING")
            {
                if (frmStartKiosk.State != "CHECKOUT" && BarcodeState != BarcodeScanState.ScanStaffCard)
                return;
            }

            if (BarcodeState == BarcodeScanState.ScanItem)
            {
                if (isCheckout)
                {
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    return;
                }

                if (!canScan)
                {
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    return;
                }
            }

            #endregion

            if (e.KeyCode == Keys.Enter)
            {
                txtBarcode.Text = txtBarcode.Text.Trim();
                ProcessScanBarcode(txtBarcode.Text,false);
            }
        }

        
        //After Button OK is Clicked when finished
        private void keyCode_OnClicked(object sender, EventArgs args)
        {
            //Process Barcode
            ProcessScanBarcode(keyCode.Barcode, true);

            //Close Key Code Panel && Clear KeyCode    
            keyCode.Clear();
            enableKeyCodeInput(false);
            showKeyCode = false;
        }

        #endregion

        #region *) CASH MACHINE PAYMENT METHOD
        public bool AcceptCash(decimal amount, out string status)
        {
            status = "";
            /*if (amount <= 0)
            {
                status = "Amount cannot be less than zero";
                return false;
            }
            if (!frmStartKiosk.cashMgr.Deposit(amount, out status))
            {
                AppendLog("deposit Failed. " + status);
                frmMessage fMessage = new frmMessage("Ask For Assistance", status);
                fMessage.ShowDialog();
                return false;
            }*/

            return true;
        }


        /*public void onCashReceived(CashManager m, CashReceivedArgs e)
        {
            //add the total paid
            try
            {
                if (!PayCash(e.Amount))
                {
                    AppendLog("Add Payment Cash Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Payment By Cash Machine Failed." + ex.Message);
                AppendLog("Payment By Cash Machine Failed." + ex.Message);
            }
        }

        public void onCashCollected(CashManager m)
        {
            //add the total paid
            try
            {
                // check the mode first if it was returning a change or not
                if (frmStartKiosk.SubState != "Waiting For Change")
                    return;

                
                // confirm the sales and back to start screen
                this.Invoke((MethodInvoker)delegate
                {
                    AppendLog("Note Returned. Change Collected");
                    //return to payment page if failed 
                    if (isInsufficientCharge)
                    {
                        isInsufficientCharge = false;
                        paidAmount = 0;
                        change = 0;
                        btnCheckOut_OnClicked(new Object(), new EventArgs());
                        return;
                    }

                    if (isExit)
                    {
                        //AppendLog("");
                        ResetPage();
                        this.Close();
                    }
                    else
                    {
                        ChangeInCash();
                    }
                });

            }
            catch (Exception ex)
            {
                //Logger.writeLog("Cash Collected Event Error." + ex.Message);
                AppendLog("Cash Collected Event Error." + ex.Message);
            }
        }

        public void onCashReturned(CashManager m, CashReturnedArgs e)
        {
            //add the total paid
            try
            {
                // check the mode first if it was returning a change or not
                if (frmStartKiosk.SubState != "Waiting For Change")
                    return;
                
                //show collect change video
                this.Invoke((MethodInvoker)delegate
                {
                    AppendLog("Note ready to collect");
                });

            }
            catch (Exception ex)
            {
                //Logger.writeLog("Cash Returned ." + ex.Message);
                AppendLog("Cash Returned Event Error." + ex.Message);
            }
        }

        public void onCashLogged(CashManager m, CashLogArgs e)
        {
            //add the total paid
            try
            {
                AppendLog(e.Log);
            }
            catch (Exception ex)
            {
                //Logger.writeLog("Payment By Cash Machine Failed." + ex.Message);
                //AppendLog("Payment By Cash Machine Failed." + ex.Message);
            }
        }

        public void onCashOutNote(CashManager m)
        {
            //add the total paid
            try
            {
                // check the mode first if it was returning a change or not
                if (frmStartKiosk.SubState != "Waiting For Change")
                    return;


                // confirm the sales and back to start screen
                this.Invoke((MethodInvoker)delegate
                {
                    timerChange.Stop();    
                });

            }
            catch (Exception ex)
            {
                //Logger.writeLog("Cash Collected Event Error." + ex.Message);
                AppendLog("Cash Collected Event Error." + ex.Message);
            }
        }

        public void onCashError(CashManager m, CashErrorArgs e)
        {
            //add the total paid
            try
            {
                if (e.ErrorLog != "Insufficient change to return to customer")
                    return;

                // check the mode first if it was returning a change or not
                //if (frmStartKiosk.cashMgr.isAccepting)
                //{
                    string status = "";
                    frmStartKiosk.cashMgr.isAccepting = false;
                    
                    this.Invoke((MethodInvoker)delegate
                    {
                        timerChange.Stop();
                        frmMessage fMessage = new frmMessage("Ask For Assistance", "Insufficient change to return to customer");
                        fMessage.ShowDialog();
                        ReturnPaidAmount();
                    });
                //}

            }
            catch (Exception ex)
            {
                //Logger.writeLog("Cash Collected Event Error." + ex.Message);
                AppendLog("Cash Error Event Error." + ex.Message);
            }
        }

        public void onCashMachineConnected(CashManager m)
        {
            AppendLog("Cash Machine Connected");
        }

        public void onCashMachineDisconnected(CashManager m)
        {
            AppendLog("Cash Machine Disconnected");
        }*/
        #endregion

        #region *) PAYMENT EVENTS
        string responseInfo = "";
        void paymentPanel_OnCreditCardPaymentClicked(object sender, EventArgs args)
        {
            //AppendLog("CREDITCARD PAYMENT IS CLICKED");
            AppendLog("User clicks Payment Mode: NETS CreditCard");

            pnlFunctionMenu.Visible = false;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = true;
            panel23.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = false;

            frmStartKiosk.State = "CHECKOUT";

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\nets.wmv"), true, GetText("frmKiosk.VideoNETS"), 26);
            //AppendLog("Play Video nets.wmv");

            frmSimulateCashPayment.Visible = false;

            DoRounding();

            isRetry = true;

            if (frmStartKiosk.SimulatorNETS)
            {
                paymentAmount = pos.CalculateTotalAmount(out status) - paidAmount;
                paymentType = "CREDITCARD".ToUpper().Trim();

                AppendLog("Simulator Sending command to " + paymentType);
                AppendLog("Simulator Amount paid by customer: $" + paymentAmount + " (Ref No: " + pos.myOrderHdr.OrderRefNo + ")");

                frmSimulateNets.Visible = true;
                frmSimulateNets.SetType(paymentType);
                frmSimulateNets.SetAmount(paymentAmount);
            }
            else
            {
                isPaymentSuccessful = false;
                paymentType = "CREDITCARD".ToUpper().Trim();

                if (!bwNets.IsBusy)
                {
                    responseInfo = "";
                    bwNets.RunWorkerAsync();
                }
                else
                {
                    //AppendLog("Your transaction is still being processed.");

                    frmWrapMessage frmWrapMessage = new frmWrapMessage("Please Wait", "Your transaction is still being processed.");
                    frmWrapMessage.ShowDialog(this);
                }
            }
        }

        private void ResetPage()
        {
            //AppendLog(string.Format("Staff {0} reset transaction", CurrentUser.UserName));
            pos = new POSController();
            itemLogic = new ItemController();
            idleCounter = 0;
            CurrentUser = new UserMst();
            isExit = true;
            change = 0;
            BindGrid();

            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = false;

            panelKeyCode.Visible = true;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = false;

            panel9.Visible = false;
            panel13.Visible = true;
            panel15.Visible = false;
            panel21.Visible = false;

            //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\scan.wmv"), true, GetText("frmKiosk.VideoScan"));
            //AppendLog("Play Video scan.wmv");

            isCheckout = false;
        }

        private void paymentPanel_OnNetsCashCardPaymentClicked(object sender, EventArgs args)
        {
            //AppendLog("NETS CASHCARD PAYMENT IS CLICKED");
            AppendLog("User clicks Payment Mode: NETS CashCard");

            pnlFunctionMenu.Visible = false;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = true;
            panel23.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = false;

            frmStartKiosk.State = "CHECKOUT";

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\nets.wmv"), true, GetText("frmKiosk.VideoNETS"), 26);
            //AppendLog("Play Video nets.wmv");

            frmSimulateCashPayment.Visible = false;

            
            DoRounding();
            

            isRetry = true;

            if (frmStartKiosk.SimulatorNETS)
            {
                paymentAmount = pos.CalculateTotalAmount(out status) - paidAmount;
                paymentType = POSController.PAY_NETSCashCard;

                AppendLog("Simulator Sending command to " + paymentType);
                AppendLog("Simulator Amount paid by customer: $" + paymentAmount + " (Ref No: " + pos.myOrderHdr.OrderRefNo + ")");
               

                frmSimulateNets.Visible = true;
                frmSimulateNets.SetType(paymentType);
                frmSimulateNets.SetAmount(paymentAmount);
            }
            else
            {
                isPaymentSuccessful = false;
                paymentType = POSController.PAY_NETSCashCard.Trim();

                if (!bwNets.IsBusy)
                {
                    responseInfo = "";
                    bwNets.RunWorkerAsync();
                }
                else
                {
                    frmWrapMessage frmWrapMessage = new frmWrapMessage("Please Wait", "Your transaction is still being processed.");
                    frmWrapMessage.ShowDialog(this);
                }
            }
        }

        private void paymentPanel_OnNetsFlashPaymentClicked(object sender, EventArgs args)
        {
            //AppendLog("NETS FLASH PAYMENT IS CLICKED");
            AppendLog("User clicks Payment Mode: NETS Flash");

            pnlFunctionMenu.Visible = false;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = true;
            panel23.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = false;

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\nets.wmv"), true, GetText("frmKiosk.VideoNETS"), 26);
            //AppendLog("Play Video nets.wmv");

            frmSimulateCashPayment.Visible = false;

            
                DoRounding();
            

            isRetry = true;

            if (frmStartKiosk.SimulatorNETS)
            {
                paymentAmount = pos.CalculateTotalAmount(out status) - paidAmount;
                paymentType = POSController.PAY_NETSFlashPay.Trim();

                AppendLog("Simulator Sending command to " + paymentType);
                AppendLog("Simulator Amount paid by customer: $" + paymentAmount + " (Ref No: " + pos.myOrderHdr.OrderRefNo + ")");

                frmSimulateNets.Visible = true;
                frmSimulateNets.SetType(paymentType);
                frmSimulateNets.SetAmount(paymentAmount);
            }
            else
            {
                isPaymentSuccessful = false;
                paymentType = POSController.PAY_NETSFlashPay.Trim();

                if (!bwNets.IsBusy)
                {
                    responseInfo = "";
                    bwNets.RunWorkerAsync();
                }
                else
                {
                    frmWrapMessage frmWrapMessage = new frmWrapMessage("Please Wait", "Your transaction is still being processed.");
                    frmWrapMessage.ShowDialog(this);
                }
            }
        }

        private void paymentPanel_OnNetsPaymentClicked(object sender, EventArgs args)
        {
            //AppendLog("NETS ATMCARD PAYMENT IS CLICKED");
            AppendLog("User clicks Payment Mode: NETS");

            pnlFunctionMenu.Visible = false;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = true;
            panel23.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = false;

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\nets.wmv"), true, GetText("frmKiosk.VideoNETS"), 26);
            //AppendLog("Play Video nets.wmv");

            frmSimulateCashPayment.Visible = false;

            
                DoRounding();
            

            isRetry = true;

            if (frmStartKiosk.SimulatorNETS)
            {
                paymentAmount = pos.CalculateTotalAmount(out status) - paidAmount;
                paymentType = POSController.PAY_NETSATMCard.Trim();

                AppendLog("Simulator Sending command to " + paymentType);
                AppendLog("Simulator Amount paid by customer: $" + paymentAmount + " (Ref No: " + pos.myOrderHdr.OrderRefNo + ")");

                frmStartKiosk.SubState = "Wait For Print Receipt";
                AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);

                frmSimulateNets.Visible = true;
                frmSimulateNets.SetType(paymentType);
                frmSimulateNets.SetAmount(paymentAmount);
            }
            else
            {
                isPaymentSuccessful = false;
                paymentType = POSController.PAY_NETSATMCard.Trim();

                if (!bwNets.IsBusy)
                {
                    responseInfo = "";
                    bwNets.RunWorkerAsync();
                }
                else
                {
                    frmWrapMessage frmWrapMessage = new frmWrapMessage("Please Wait", "Your transaction is still being processed.");
                    frmWrapMessage.ShowDialog(this);
                }
            }
        }

        private bool isCashPayment = false;

        private void paymentPanel_OnCashPaymentClicked(object sender, EventArgs args)
        {
            //AppendLog("CASH PAYMENT IS CLICKED");
            AppendLog("Cash Payment is selected");
            isCashPayment = true;
            if (AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.MachineModelNotes) != "" && !frmStartKiosk.isCashMachineAvailable)
            {
                AppendLog("Cash Machine is not active. Please check the connection in setting page");
                return;
            }

            DoRounding();


            if (AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.MachineModelNotes) == "")
                //Simulator mode
                frmSimulateCashPayment.Visible = true;
            else
            {
                string status = "";
                decimal amt = pos.CalculateTotalAmount(out status);
                
                if (!AcceptCash(amt, out status))
                {
                    AppendLog("Error Accept Cash." + status);
                }
            }

            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = true;

            panelKeyCode.Visible = false;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = true;
            panel23.Visible = false;

            panel9.Visible = false;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = true;

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\cash.wmv"), true, GetText("frmKiosk.VideoCash"));
            //AppendLog("Play Video cash.wmv");
    
            RefreshPayment();
            
        }

        

        #endregion

        #region *) BACKGROUND WORKER NETS

        private void bwNets_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                NetsController nets = new NetsController();
                if (nets.setupSerialPort(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NetsCOMPort)))
                {
                    paymentAmount = pos.CalculateTotalAmount(out status) - paidAmount;

                    //AppendLog(string.Format(">> NETS Integration {0} : ${1}", POSController.PAY_NETSATMCard.ToUpper().Trim(), paymentAmount.ToString("N2")));
                    AppendLog("Sending command to " + paymentType);
                    AppendLog("Amount paid by customer: $" + paymentAmount + " (Ref No: " + pos.myOrderHdr.OrderRefNo + ")");
                    newPaymentType = paymentType;
                    switch (paymentType.ToUpper())
                    {
                        case "NETS ATM CARD":
                            nets.addNetsCardPayment(paymentAmount.ToString("N2"), pos.myOrderHdr.OrderHdrID.Substring(6, 8), out status);
                            break;
                        case "NETS FLASHPAY":
                            nets.addFlashPayPayment(paymentAmount.ToString("N2"), pos.myOrderHdr.OrderHdrID.Substring(6, 8), out status);
                            break;
                        case "NETS CASHCARD":
                            nets.addCashCardDebitPayment(paymentAmount.ToString("N2"), pos.myOrderHdr.OrderHdrID.Substring(6, 8), out status);
                            break;
                        case "CREDITCARD":
                            nets.addCreditCardCardPayment(paymentAmount.ToString("N2"), pos.myOrderHdr.OrderHdrID.Substring(6, 8), out status, out newPaymentType);
                            break;
                        default:
                            status = "ERROR: Payment is not supported";
                            break;
                    }


                    if (status.StartsWith("ERROR"))
                    {
                        throw new Exception(status);
                        isPaymentSuccessful = false;
                    }
                    else
                    {
                        responseInfo = nets.responseInfo;
                        isPaymentSuccessful = true;
                    }
                    //paymentType = "";
                }
                else
                {
                    throw new Exception("Connect to Nets Port Failed, Please check the port name");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                Logger.writeStaffAssistLog("ERROR", ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                //AppendLog(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        private void bwNets_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isPaymentSuccessful)
            {
                frmStartKiosk.SubState = "Wait For Print Receipt";

                AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);

                PayNonCash(paymentAmount, newPaymentType);
            }
            else
            {
                frmWrapMessage frmWrapMessage = new frmWrapMessage("Ask for assistance...", "Your transaction has been declined. Please ask our staff for assistance.");
                frmWrapMessage.ShowDialog(this);

                btnSelectOtherPayment_OnClicked(null, new EventArgs());
            }
        }

        #endregion

        #region *) BACKGROUND WORKER BARCODE SCANNER

        private void bwBarcode_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!isExit)
            {
                string barcodeStatus = "";
                bool isInitBarcodeSuccess = barcodeScanner.init(out barcodeStatus);
                barcodeStatus = "enable barcode failed. " + barcodeStatus;
                if (isInitBarcodeSuccess)
                    e.Result = "";
                else
                    e.Result = barcodeStatus;
            }
        }

        private void bwBarcode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string barcodeStatus = e.Result + "";
            if (!string.IsNullOrEmpty(barcodeStatus))
                AppendLog(barcodeStatus);

            if ((frmStartKiosk.SubState + "").ToLower().Equals("wait for item scan") && (lastError+"").Trim()=="")
                if (!isExit)
                EnableBarcodeScanner(true);
        }

        #endregion

        #region *) FORM TIMER

        private void timerThankYou_Tick(object sender, EventArgs e)
        {
            AppendLog("Receipt is printed");
            AppendLog("All required hardware are connected");

               
            
                
                //AppendLog("State:" + frmStartKiosk.State + ", substate:" + frmStartKiosk.SubState);
            

            videoControl2.Stop();

            timerThankYou.Enabled = false;

            panel22.BringToFront();
            panel22.Visible = true;

            timerClose.Enabled = true;
            timerClose.Start();

            frmStartKiosk.LastWeight = lastWeight;

            frmStartKiosk.State = frmStartKiosk.KioskState.STANDBY;
            frmStartKiosk.SubState = frmStartKiosk.KioskState.STANDBY_WAITFORREADY;

            frmStartKiosk.StateInit();
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            //AppendLog("=== END KIOSK Service ===");

            timerClose.Enabled = false;
            ResetPage();
            Close();
            
        }

        private bool isTimerEnabled = false;

        public void enabledTimerIdle(bool enabled)
        {
            isTimerEnabled = enabled;
            idleCounter = 0;
        }

        private void timerIdle_Tick(object sender, EventArgs e)
        {
            if (!isTimerEnabled)
                return;

            //Check the count 
            
            if (idleCounter >= (idleInterval * 60))
            {
                AppendLog("Timer Idle disabled");
                enabledTimerIdle(false);
                AppendLog(string.Format("Transaction reset due to idle time {0} min(s)", idleInterval));
                ResetTransactionAndBackToStartScreen();
                return;
            }
            idleCounter++;

            //Debug.WriteLine("frmStartKiosk.State: " + frmStartKiosk.State);
            //Debug.WriteLine("frmStartKiosk.SubState: " + frmStartKiosk.SubState);
            /*if (idleInterval == 0 || pos.myOrderDet.Count != 0)
            {
                idleCounter = 0;
                return;
            }

            string subState = frmStartKiosk.SubState;
            if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && subState == frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN && !isCheckoutKiosk)
            {
                idleCounter++;
                if (idleCounter >= (idleInterval * 60))
                {

                    AppendLog(string.Format("Transaction reset due to idle time {0} min(s)", idleInterval));
                    ResetTransactionAndBackToStartScreen();
                }
            }
            else
            {
                idleCounter = 0;
            }*/
        }

        #endregion

        #region *) LANGUAGE TEXT

        private string GetText(string key)
        {
            string result = "";
            try
            {
                result = dictLanguages[key];
            }
            catch
            {
                result = "";
            }

            return result == null ? "" : result;
        }

        #endregion

        #region *) KEYBOARD SCANNING

        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(Key key)
        {
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
                default:
                    {
                        ch = stringBuilder[0];
                        break;
                    }
            }
            return ch;
        }

        #endregion

        #region Timer change
        private void initTimerChange()
        {
            changeTimerCounter = 0;
            timerChange = new System.Windows.Forms.Timer();
            timerChange.Interval = 1000;
            timerChange.Tick += new EventHandler(timerChange_Tick);
            timerChange.Start();
        }

        #endregion

        #region IKiosk Members

        public void RefreshPayment()
        {
            BindGrid();

            
            summaryPanel.UpdateSummary(pos.CalculateTotalAmount(out status), paidAmount);
            

            if (change > 0)
            {
                frmStartKiosk.SubState = "Waiting For Change";
                //Play Video Processing
                AppendLog("Show Change : " + change.ToString());
                panel21.Visible = false;
                videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\change.wmv"), true, GetText("frmKiosk.VideoChange"));
                //AppendLog("Play Video change.wmv");
                initTimerChange();
                return;
                //
            }

            
            var rcpts = pos.FetchReceiptDet().GetList();
            if (rcpts.Count == 1 && rcpts[0].PaymentType == "CASH" && paidAmount == pos.CalculateTotalAmount(out status) && change == new decimal(0.0d))
            {
                ChangeInCash();
            }
            
        }

        public bool PayCash(decimal amt)
        {
            //AppendLog("PAY CASH : " + amt);

            paidAmount = amt;

            pos.RemoveReceiptByPaymentType(POSController.PAY_CASH);
            pos.AddReceiptLinePayment(paidAmount, POSController.PAY_CASH, "", 0, "", 0, out change, out status);
            
            this.Invoke((MethodInvoker)delegate
                {
                    RefreshPayment();
                });

            return paidAmount < pos.CalculateTotalAmount(out status);
        }

        public void ChangeInCash()
        {
            bool IsPointAllocationSuccess;
            bool Success;
            
            panel21.Visible = false;

            summaryPanel.Visibility = System.Windows.Visibility.Hidden;

            frmSimulateCashPayment.Visible = false;

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\receipt.wmv"), true, GetText("frmKiosk.VideoReceipt"));
            //AppendLog("Play Video receipt.wmv");
            AppendLog("Transaction Confirmed");

            try
            {
                
                    #region *) Core: Confirm Order
                    Success = pos.ConfirmOrder(false, out IsPointAllocationSuccess, out status);
                    #endregion

                    if (Success)
                    {
                        if (!pos.ExecuteStockOut(out status))
                        {
                            //MessageBox.Show
                            //    ("Error while performing Stock Out: " + status,
                            //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Logger.writeStaffAssistLog("Error", "Error while performing Stock Out: " + status);

                            return;
                        }


                        frmStartKiosk.SyncSales();

                        //tryDownloadPoints();

                        //print receipt
                        if (PrintReceipt)
                        {
                            AppendLog("Receipt is printing...");

                            POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        }
                    }
                
                
                //Logger.writeStaffAssistLog("INFO", "CHANGE IN CASH");
                //AppendLog("CHANGE IN CASH");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);

                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //AppendLog("An unknown error has been encountered. Please contact your system administrator.");

                return;
            }

            AppendLog("Receipt is printed");

            timerThankYou.Enabled = true;
            timerThankYou.Start();
        }

        public bool PayNonCash(decimal amt, string paymentType)
        {
            //AppendLog("PAY NON CASH : " + amt);

            paidAmount += amt;

            
            pos.AddReceiptLinePayment(amt, paymentType, "", 0, "", 0, out change, out status, false, responseInfo);
            

            RefreshPayment();

            panel21.Visible = false; // button others payment

            summaryPanel.Visibility = System.Windows.Visibility.Hidden;

            frmSimulateNets.Visible = false;

            videoControl2.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\receipt.wmv"), true, GetText("frmKiosk.VideoReceipt"));
            //AppendLog("Play Video receipt.wmv");

            bool IsPointAllocationSuccess;
            bool Success;

            try
            {
                
                    #region *) Core: Confirm Order
                    Success = pos.ConfirmOrder(false, out IsPointAllocationSuccess, out status);
                    #endregion

                    if (Success)
                    {
                        if (!pos.ExecuteStockOut(out status))
                        {
                            //AppendLog("Error while performing Stock Out: " + status);

                            //MessageBox.Show
                            //    ("Error while performing Stock Out: " + status,
                            //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            Logger.writeStaffAssistLog("Error", "Error while performing Stock Out: " + status);

                            return false;
                        }


                        frmStartKiosk.SyncSales();

                        //tryDownloadPoints();

                        //print receipt
                        if (PrintReceipt)
                        {
                            //AppendLog("PRINT RECEIPT");
                            AppendLog("Receipt is printing...");

                            POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        }
                    }
                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);

                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            timerThankYou.Enabled = true;
            timerThankYou.Start();

            return true;
        }

        public void CancelNonCashSimulate()
        {
            frmWrapMessage frmWrapMessage = new frmWrapMessage("Ask for assistance...", "Your transaction has been declined. Please ask our staff for assistance.");
            frmWrapMessage.ShowDialog(this);

            btnSelectOtherPayment_OnClicked(null, new EventArgs());
        }

        public void EndSimulate()
        {
        }

        #endregion

        #region IScanWeight

        private bool IsSuccess = false;
        private decimal Weight = new decimal(0.0f);

        public void SetWeightResult(bool IsSuccess, decimal Weight)
        {
            this.IsSuccess = IsSuccess;
            this.Weight = Weight;

            if (IsSuccess)
            {
                ViewItem theItem = itemLogic.FetchByBarcode(txtBarcode.Text);
                if (SaveItemWeight(theItem.Barcode, (int)Weight) < 0)
                {
                    DataTable dt = pos.FetchUnSavedOrderItems(out status);
                    int rowCount = dt.Rows.Count;
                    if (rowCount > 0)
                    {
                        decimal qty = 0;
                        decimal.TryParse(dt.Rows[0]["Quantity"].ToString(), out qty);

                        qty -= 1;

                        if (qty > 0)
                            pos.ChangeOrderLineQuantity(dt.Rows[0]["ID"].ToString(), qty, false, out status);
                        else
                        {
                            OrderDet det = pos.GetLine(dt.Rows[0]["ID"].ToString(), out status);
                            if (det != null)
                                pos.RemoveLine(det);
                        }
                    }

                    //AppendLog("Product cannot be added. The actual weight is incorrect");
                    lastError = "Please Remove Item From Bagging Area";
                    frmWrapMessage frmWrapMessage = new frmWrapMessage("Ask for assistance...", "Please Remove Item From Bagging Area");
                    frmWrapMessage.ShowDialog(this);
                }
            }
            else
            {
                //AppendLog("SET RESULT SCAN WEIGHT FAILED");
            }

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    showVideo(VideoState.VideoScan);
                    //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\scan.wmv"), true, GetText("frmKiosk.VideoScan"));
                });
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            isWaitingForBagging = false;

            canScan = true;

            frmStartKiosk.SubState = "Wait For Item Scan";

            AppendLog("Barcode Scanning & Keycode & CHECKOUT are enabled");
            lastScannedBarcode = "";
            AppendLog("Last scanned barcode = \"\"");

            this.Invalidate();

            //AppendLog("SET RESULT FINISHED");
        }

        public void CancelScanWeight()
        {
            //AppendLog("CANCEL SCAN WEIGHT, SCANNING BARCODE : " + txtBarcode.Text);

            frmWrapMessage frm = new frmWrapMessage("Ask for assistance...", "Product cannot be added to your shopping cart. Checking processs is failed");
            frm.ShowDialog();

            showVideo(VideoState.VideoScan);
            //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\scan.wmv"), true, GetText("frmKiosk.VideoScan"));

            canScan = true;
        }

        #endregion

        #region IObserver Members

        void IObserver.UpdateLog()
        {
            try {

                if (!this.IsDisposed)
                {
                    txtLog.Text = "";

                    txtLog.AppendText(String.Join(Environment.NewLine, frmStartKiosk.BufferLog.ToArray()));
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Update Log " + ex.Message);
            }
        }

        void IObserver.ExecuteCommand(string command)
        {
            switch (command)
            {
                case "PROCESSING|Wait For Bagging|Weighing Scale is Disconnected":
                    canScan = true;

                    try
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            showVideo(VideoState.VideoScan);
                            //ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + "videos\\scan.wmv"), true, GetText("frmKiosk.VideoScan"));
                        });
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Form Controls Event Handler
        private void frmKiosk_Load(object sender, EventArgs e)
        {
            //InitKioskForm();
        }

        private void frmKiosk_Activated(object sender, EventArgs e)
        {
            AppendLog("Form Activated. isStart = " + isStart.ToString());
            if (isStart)
            {
                InitKioskForm();
                StateInit();                
                
            }
        }

        private void timerChange_Tick(object sender, EventArgs e)
        {
            if (frmStartKiosk.SubState != "Waiting For Change")
                return;

            changeTimerCounter++;
            if (changeTimerCounter >= changeInterval)
            {
                AppendLog("No Note Returned. Change has been collected.");
                //reset state
                //frmStartKiosk.cashMgr.isAccepting = false;
                //frmStartKiosk.cashMgr.isDispensing = false;


                timerChange.Stop();

                //return to payment page if failed 
                if (isInsufficientCharge)
                {
                    isInsufficientCharge = false;
                    paidAmount = 0;
                    change = 0;
                    btnCheckOut_OnClicked(sender, e);
                    return;
                }

                if (isExit)
                {
                    ResetPage();
                    this.Close();
                }
                else
                {
                    ChangeInCash();
                }
            }
        }
        #endregion

        private int changeTimerCounter;

        

        #region Order Page
        public void StateInit()
        {
            AppendLog("State : " + frmStartKiosk.State + ", SubState : " + frmStartKiosk.SubState);

            if (frmStartKiosk.State != frmStartKiosk.KioskState.PROCESSING)
                return;

            if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN)
            {

                if (lastError != "")
                {
                    AppendLog("Last Error is not empty, so do nothing");
                    return;
                }

                //Show "Scan Item Barcode"
                
                AppendLog("Waiting for customer to scan item barcode");
                BarcodeState = BarcodeScanState.ScanItem;
                showVideo(VideoState.VideoScan);
                

                //set last scanned barcode= "";
                if (!isPromptShown())
                {
                    AppendLog("Reset Last Scanned Barcode = ''");
                    lastScannedBarcode = "";
                    AddBarcodeScanDelay();
                    EnableBarcodeScanner(true);

                    //Enable Keycode Button
                    EnableKeyCode(true);

                    //Enable CHECKOUT button if item count > 0
                    if (pos.GetNoOfItem() > 0)
                    {
                        AppendLog("CHECKOUT button is enabled (item count = 1)");
                        EnableCheckOut(true);
                        enabledTimerIdle(false);
                    }

                    //Disable CHECKOUT button if item count <= 0 
                    if (pos.GetNoOfItem() <= 0)
                    {
                        AppendLog("CHECKOUT button is disabled (item count = 0)");
                        EnableCheckOut(false);
                        enabledTimerIdle(true);
                    }
                }
                else
                    AppendLog("Dont enable barcode scanner, Keycode, Checkout because prompt is showing");
                
            }

            if (frmStartKiosk.State == "PROCESSING" && frmStartKiosk.SubState == "Wait For Bagging")
            {
                // Disable Barcode Scanner, KEYCODE, & CHECKOUT button
                EnableBarcodeScanner(false);
                EnableKeyCode(false);
                EnableCheckOut(false);
                enabledTimerIdle(false);

                //Skip Weight Check if not use weighing scale
                if (!useWeighingScale)
                {
                    AppendLog("Weighing Machine is disabled, skip weight check");
                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }

                //Skip Weight Check if Weighing Scale is not Connected
                if (!frmStartKiosk.IsWMConnected)
                {
                    AppendLog("Weighing Machine is disconnected, skip weight check");
                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }

                //if new weight < Last weight, Display Error = ""Return Item to Bagging Area"" , Weight Reduced
                if (KioskHelper.isNewWeightLessThan(newWeight, lastWeight, weightTolerance))
                {
                    AppendLog("Item was removed from Bagging Area");
                    lastError = VideoState.VideoPutBack;
                    showVideo(lastError);
                    //StateInit();
                    return;
                }

                //Weight Already Increase Previously.. 
                if (KioskHelper.isNewWeightMoreThan(newWeight, lastWeight, weightTolerance))
                {


                    //Calculate item weight, record this weight for new item
                    SetWeightResult(true, newWeight - lastWeight);
                    AppendLog("Item is put on bagging area (item weight = " + (newWeight - lastWeight).ToString() + "g");

                    //Record this weight as Last Weight
                    lastWeight = newWeight;
                    AppendLog("Set Last Weight = " + lastWeight.ToString());

                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {

                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }


                if (KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance))
                {


                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {
                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    showVideo(VideoState.VideoBagging);
                    AppendLog("Waiting for customer to put item to Bagging Area");

                }
            }

            if (frmStartKiosk.State == frmStartKiosk.KioskState.PROCESSING && frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORREMOVAL )
            {
                // Disable Barcode Scanner, KEYCODE, & CHECKOUT button
                EnableBarcodeScanner(false);
                EnableKeyCode(false);
                EnableCheckOut(false);
                enabledTimerIdle(false);

                //Skip Weight Check if not use weighing scale
                if (!useWeighingScale)
                {
                    AppendLog("Weighing Machine is disabled, skip weight check");
                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }

                //Skip Weight Check if Weighing Scale is not Connected
                if (!frmStartKiosk.IsWMConnected)
                {
                    AppendLog("Weighing Machine is disconnected, skip weight check");
                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }

                //if new weight < Last weight, Display Error = ""Return Item to Bagging Area"" , Weight Reduced
                if (KioskHelper.isNewWeightMoreThan(newWeight, lastWeight, weightTolerance))
                {
                    AppendLog("Pls Scan your item before putting in bagging area");
                    lastError = VideoState.VideoScanFirst;
                    showVideo(lastError);
                    //StateInit();
                    return;
                }

                //Weight Already Increase Previously.. 
                if (KioskHelper.isNewWeightLessThan(newWeight, lastWeight, weightTolerance))
                {


                    //Calculate item weight, record this weight for new item
                    SetWeightResult(true, newWeight - lastWeight);
                    AppendLog("Item is removed from bagging area (item weight = " + (newWeight - lastWeight).ToString() + "g");

                    //Record this weight as Last Weight
                    lastWeight = newWeight;
                    AppendLog("Set Last Weight = " + lastWeight.ToString());

                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {

                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    //go to State Init PROCESSING - Wait For Item Scan
                    frmStartKiosk.State = "PROCESSING";
                    frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                    StateInit();
                    return;
                }


                if (KioskHelper.isNewWeightEquals(newWeight, lastWeight, weightTolerance))
                {


                    //If Last Error <> """", Reset Last Error = """"
                    if (lastError != "")
                    {
                        lastError = "";
                        AppendLog("Reset Last Error");
                    }

                    showVideo(VideoState.VideoWaitForRemoval);
                    AppendLog("Remove Item from Bagging Area");

                }


            }
            //if (lastError)
            //AppendLog("Error: \"Pls scan your items before putting in Bagging Area\"");
            //
        }

        private bool isVideoErrorState(string videoState)
        {
            if (videoState == VideoState.VideoScanFirst)
                return true;
            if (videoState == VideoState.VideoPutBack)
                return true;

            return false;

        }

        private void stopVideo()
        {
            ctrlVideo.Stop();
            AppendLog("Video is stopped");

        }

        private void showVideo(string videoText)
        {
            string videoURL = "";
            
            switch (videoText)
            {
                case VideoState.VideoScanFirst :
                    videoURL = "videos\\scanfirst.wmv";
                    break;
                case VideoState.VideoPutBack :
                    videoURL = "videos\\putback.wmv";
                    break;
                case VideoState.VideoScan:
                    videoURL = "videos\\scan.wmv";
                    break;
                case VideoState.VideoBagging:
                    videoURL = "videos\\bagging.wmv";
                    break;
                case VideoState.VideoWaitForRemoval:
                    videoURL = "videos\\scanfirst.wmv";
                    break;

            };
            try
            {

                if (videoURL != "")
                    this.Invoke((System.Windows.Forms.MethodInvoker)delegate
                    {
                        System.Windows.Media.SolidColorBrush clr = System.Windows.Media.Brushes.White;
                        if (isVideoErrorState(videoText))
                            clr = System.Windows.Media.Brushes.Red;
                        ctrlVideo.Play(new Uri(AppDomain.CurrentDomain.BaseDirectory + videoURL), true, GetText(videoText), 36, clr, videoText);
                    });
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error on Loading Video." + ex.Message);
            }
        }

        delegate void EnableBarcodeScannerDelegate(bool enabled);    

        private void EnableBarcodeScanner(bool enabled)
        {
            
            if (this.InvokeRequired)
            {
                EnableBarcodeScannerDelegate d = new EnableBarcodeScannerDelegate(EnableBarcodeScanner);
                this.Invoke(d, new object[] { enabled });
                return;
            }

            if (isUseBarcodeScanner)
            {
                if (enabled)
                {
                    AppendLog(string.Format("Enable Barcode scanner on port : " + barcodeScanner.COMPort));
                    string status = "";
                    barcodeScanner.enableDevice();
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    return;
                    //TODO : Wait until barcode is on
                }

                if (!enabled)
                {
                    AppendLog(string.Format("Disable Barcode scanner on port : " + barcodeScanner.COMPort));
                    //string status = "";
                    barcodeScanner.disableDevice();
                    return;
                }
            }
            
        }

        delegate void EnableKeyCodeDelegate(bool enabled);

        private void EnableKeyCode(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnableKeyCodeDelegate(EnableKeyCode), new object[] { enabled });
                return;
            }

            AppendLog(string.Format("Enable KeyCode : " + enabled.ToString()));
            if (!enabled)
            {
                ctrlKeyCode.Visibility = System.Windows.Visibility.Hidden;
                
                return;
            }

            ctrlKeyCode.Visibility = System.Windows.Visibility.Visible;
            
            return;
        }

        delegate void EnableCheckOutDelegate(bool enabled);
        private void EnableCheckOut(bool enabled)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnableCheckOutDelegate(EnableCheckOut), new object[] { enabled });
                return;
            }

            AppendLog(string.Format("Enable Check Out Button : " + enabled.ToString()));
            if (!enabled)
            {
                btnCheckOut.Visibility = System.Windows.Visibility.Hidden;
                panel13.Visible = false;
                //panel9.Visible = true;
                return;
            }
            panel13.Visible = true;
            panel9.Visible = false;
            btnCheckOut.Visibility = System.Windows.Visibility.Visible;
            
            return;
        }

        private void EnableBarcodeKeycodeCheckOut(bool enabled)
        {
            EnableBarcodeScanner(enabled);
            EnableKeyCode(enabled);
            EnableCheckOut(enabled);
        }

        private void InitKioskForm()
        {
            AppendLog("Init Order Page");
            lastWeight = 0;
            newWeight = 0;

            AppendLog("Reset Last Scanned Barcode =''");
            lastScannedBarcode = "";
            AppendLog("Reset Last Error =''");
            lastError = "";
            tmpWeight = 0;
            isExit = false;

            isWaitingForBagging = false;
            sumOfDiffMinus = 0;
            isValidWeight = true;
            canScan = true;

            lastWeight = 0;

            weightTolerance = 0;
            Int32.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.Tolerance), out weightTolerance);
            idleInterval = (AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.IdleTimes) + "").GetIntValue();
            idleCounter = 0;

            CurrentUser = new UserMst();

            btnLog.Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.HideLogButton), false);


            paidAmount = 0;


            pnlFunctionMenu.Visible = true;
            pnlPaidAmount.Visible = false;

            panelKeyCode.Visible = true;
            panelPaymentList.Visible = false;
            panelNetsPayment.Visible = false;

            panel9.Visible = true;
            panel13.Visible = false;
            panel15.Visible = false;
            panel21.Visible = false;
            panel22.Visible = false;

            hostKeyCode.Visible = false;
            isCheckout = false;
            isStart = false;

            //Timer Idle
            enabledTimerIdle(true);
            timerIdle.Start();
            
            //Change
            changeInterval = 8;
            changeTimerCounter = 0;
            responseInfo = "";

            //Age Checking
            isAgeVerified = false;
            verifiedAge = 0;
            tmpAgeToVerify = 0;
            useWeighingScale = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);

            //Timer Thank you
            string strThankYouPanelInterval = AppSetting.GetSetting(AppSetting.SettingsName.Kiosk.ThankYouInterval);
            int ThankyouInterval = 0;
            if (!int.TryParse(strThankYouPanelInterval, out ThankyouInterval))
                ThankyouInterval = 3000;
            timerThankYou.Interval = ThankyouInterval;
            
        }

        /*private int verifyAge(int ageToVerify, bool showVerifyIC)
        {
            frmLoginKiosk frmLogin = new frmLoginKiosk();
            AppendLog("Open authorization confirmation");
            var result = frmLogin.ShowDialog();
            if (result == DialogResult.Cancel)
                return 0;

            isAgeVerified = false;
            if (frmLogin.IsStaffVerify)
            {
                BarcodeState = BarcodeScanState.ScanVerifyStaffCard;
                OpenBarcodeScanner("Age Above " + ageToVerify.ToString() + "?", "Before scanning your staff card, Pls verify customer IC otherwise press cancel");
                //wait until barcode is scanned 
                //Put the current thread into waiting state until it receives the signal
                //autoResetEvent.WaitOne();
                isScanningCompleted = false;
                while (!isScanningCompleted)
                {
                    Application.DoEvents();
                    Thread.Sleep(10);

                }
                if (!isAgeVerified)
                {
                    return 0;
                }

                verifiedAge = ageToVerify;
                return verifiedAge;
            }
            else
            {   
                //Scan IC  
                BarcodeState = BarcodeScanState.ScanVerifyNRIC;
                OpenBarcodeScanner("Please scan your IC", "Only For Singaporean / PR, otherwise press cancel");
                //OpenBarcodeScanner("nric");
                
                //Waiting for scan IC to be completed
                //isScanningCompleted = false;
                

               
            }

            

            

            //
        }*/
        #endregion

        #region COMMON
        

        delegate void AppendLogDelegate(string item);

        public void AppendLog(string item)
        {
            //Logger.writeLog(item);
            //return;

            if (frmStartKiosk.InvokeRequired)
            {
                AppendLogDelegate d = new AppendLogDelegate(AppendLog);
                frmStartKiosk.Invoke(d, new object[] { item });
            }
            else
            {
                try
                {
                    //this.Invoke((MethodInvoker)delegate
                    //{
                    DateTime dt = DateTime.Now;

                    Logger.writeStaffAssistLog("INFO", item);

                    if (frmStartKiosk.BufferLog.Count > 1000)
                        frmStartKiosk.BufferLog.Dequeue();

                    frmStartKiosk.BufferLog.Enqueue(dt.ToString("dd-MM-yyyy hh:mm:ss") + " - " + item);

                    //frmStartKiosk.Notify();
                    //});
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }
        }
        #endregion 

        #region Scan Barcode Logic

        delegate void BarcodeScannedDelegate(BarcodeScannerController m, BarcodeScannedArgs e);

        public void barcodeScanner_BarcodeScanned(BarcodeScannerController m, BarcodeScannedArgs e)
        {
            if (frmStartKiosk.State != "PROCESSING" && frmStartKiosk.State != "CHECKOUT")
                return;

            AppendLog("Barcode scanned : " + e.lastBarcode);
            //Thread.Sleep(100);
            if (txtBarcode.InvokeRequired)
            {
                BarcodeScannedDelegate d = new BarcodeScannedDelegate(barcodeScanner_BarcodeScanned);
                this.Invoke(d, new object[] { m, e });
                return;
            }

            if (string.IsNullOrEmpty(e.lastBarcode))
                return;
            //txtBarcode.Text = (e.lastBarcode + "").Trim();
            ProcessScanBarcode(e.lastBarcode,false);

        }

        delegate void showErrorMessageDelegate(string errorText, string errorTitle);

        private void showErrorMessage(string errorText, string errorTitle)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new showErrorMessageDelegate(showErrorMessage), new object[] { errorText, errorTitle });
                return;
            }

            frmWrapMessage frmWrapMessage = new frmWrapMessage(errorTitle, errorText);
            frmWrapMessage.ShowDialog(this);
        }

        private List<ItemStruct> tempItemForAgeVerification = null;
        private int tmpAgeToVerify = 0;

        /*private void scanBarcodeItem(string barcodeText, out bool isItemAdded)
        {
            tempItemForAgeVerification = null;
            tmpAgeToVerify = 0;
            isItemAdded = false;
            barcodeScanner.disableDevice();
            List<ItemStruct> tmpItemStruct = new List<ItemStruct>();
            bool res = pos.FindItemByBarcode(barcodeText, out tmpItemStruct);
            if (!res)
            {
                //Error "Barcode '123' is not registered yet"
                
                return;
            }
            if (res)
            {
                //Validate for Selling Restriction
                if (!pos.validateSellingRestriction(tmpItemStruct, out status))
                {
                    showErrorMessage(status, "Ask for Assistance");
                    AppendLog(status);
                    AppendLog("Item is removed");
                    return;
                }

                int tmpMaxAgeVerifications = pos.getMaxAgeRestrictions(tmpItemStruct);
                if (verifiedAge < tmpMaxAgeVerifications)
                {
                    //Start Age Verification
                    tmpAgeToVerify = tmpMaxAgeVerifications;
                    tempItemForAgeVerification = tmpItemStruct;
                    startAgeVerification();
                    return;
                }

                //Add Item To List if no error
                if (AddItemToOrder(tmpItemStruct,))
                {
                    isItemAdded = true;
                }
                
                return;
            }
        }*/

        private bool showSearchItem(string barcodeText, out string selectedItemBarcode)
        {
            bool res = false;
            #region *) Partial Search
            
            string sql = @"SELECT  I.ItemNo
		                            ,I.ItemName
		                            ,I.Barcode
		                            ,I.RetailPrice UnitPrice
                            FROM	Item I
                            WHERE	ISNULL(I.Deleted,0) = 0
		                            AND I.Barcode LIKE '%{0}%'
                            ORDER BY I.ItemName";
            sql = string.Format(sql, barcodeText);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            
            #endregion

            //if no item found
            if (dt.Rows.Count == 0)
            {
                selectedItemBarcode = "";
                return false;
            }
            
            //show item selection form
            frmItemList frmItem = new frmItemList(dt);
            frmItem.TopMost = true;

            if (frmItem.ShowDialog() != DialogResult.OK)
            {
                selectedItemBarcode = "";
                return false;
            }

            //Return the barcode of selected ITem
            selectedItemBarcode = frmItem.Barcode;
            
            return true;

        }

        private void ProcessScanBarcode(string barcodeText, bool isFromKeyCode)
        {
            //Set Last Scanned Barcode
            lastScannedBarcode = barcodeText;
            AppendLog("Last scanned barcode = " + lastScannedBarcode);

            if (BarcodeState == BarcodeScanState.ScanItem && ((frmStartKiosk.SubState + "").ToLower().Equals("wait for item scan")))
            {
                bool isItemAdded = false;
                //isItemAdded = false;
                barcodeScanner.disableDevice();
                
                List<ItemStruct> tmpItemStruct = new List<ItemStruct>();
                
                bool res = pos.FindItemByBarcode(barcodeText, out tmpItemStruct);

                //Validate Barcode not registered
                if (!res || tmpItemStruct == null || tmpItemStruct.Count ==0)
                {
                    if (!isFromKeyCode)
                    {
                        showErrorMessage("Barcode " + barcodeText + " is not registered yet", "Ask for Assistance");
                        AppendLog("Barcode " + barcodeText + " is not registered yet");
                        AppendLog("Item is removed");
                        StateInit();
                        return;
                    }

                    //Partial Search if is from Key Code
                    string selectedItemBarcode = "";
                    if (!showSearchItem(barcodeText, out selectedItemBarcode))
                    {
                        showErrorMessage("Barcode " + barcodeText + " is not registered yet", "Ask for Assistance");
                        AppendLog("Barcode " + barcodeText + " is not registered yet");
                        AppendLog("Item is removed");
                        scanPromptClosed();
                        StateInit();
                        return;
                    }
                    //2nd time of check 
                    res = pos.FindItemByBarcode(selectedItemBarcode, out tmpItemStruct);
                    if (!res || tmpItemStruct == null || tmpItemStruct.Count ==0)
                    {
                        showErrorMessage("Barcode " + barcodeText + " is not registered yet", "Ask for Assistance");
                        AppendLog("Barcode " + barcodeText + " is not registered yet");
                        AppendLog("Item is removed");
                        StateInit();
                        return;
                    }
                    
                }
                
                //Validate for Selling Restriction
                if (!pos.validateSellingRestriction(tmpItemStruct, out status))
                {
                    showErrorMessage(status, "Ask for Assistance");
                    AppendLog(status);
                    AppendLog("Item is removed");
                    StateInit();
                    return;
                }

                //Validate Age Restrictions
                int tmpMaxAgeVerifications = pos.getMaxAgeRestrictions(tmpItemStruct);
                if (verifiedAge < tmpMaxAgeVerifications)
                {
                    //Start Age Verification
                    tmpAgeToVerify = tmpMaxAgeVerifications;
                    tempItemForAgeVerification = tmpItemStruct;
                    startAgeVerification();
                    return;
                }

                //Validate Open Price 
                foreach (ItemStruct itemStruct in tmpItemStruct)
                {
                    if (!itemStruct.isOpenPrice)
                        continue;

                    if (!IsStaffLoggedIn)
                    {
                        showErrorMessage("Please ask for staff assistance. ", "Please Ask Assistance");
                        frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                        StateInit();
                        return;
                    }

                    //show keypad form
                    frmKeypad fKeypad = new frmKeypad();
                    fKeypad.textMessage = "Please Enter Price";
                    //fKeypad.value = itemStruct.unitPrice.ToString("N2");
                    DialogResult isKeypadSuccess = fKeypad.ShowDialog();
                    if (isKeypadSuccess != DialogResult.OK)
                    {
                        frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                        StateInit();
                        return;
                    }

                    decimal tmpValue = 0;
                    if (decimal.TryParse(fKeypad.value, out tmpValue))
                    {
                        if (tmpValue <= 0)
                        {
                            showErrorMessage("Item price cannot be 0. Please re key in. ", "Please Ask Assistance");
                            frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN;
                            StateInit();
                            return;
                        }

                        itemStruct.unitPrice = tmpValue;
                    }

                    
                }

                //Add Item To List if no error
                AddItemToOrder(tmpItemStruct, isFromKeyCode);
                
            }
            else if (BarcodeState == BarcodeScanState.ScanStaffCard)
            {
                //CloseBarcodeScanner();
                barcodeScanner.disableDevice();
                BarcodeState = BarcodeScanState.ScanItem;
                hostScanner.Visible = false;
                hostScanner.SendToBack();

                if (DoStaffLogin(barcodeText))
                {
                    OpenStaffFunction();
                }
                AppendLog("Add 2 Sec Delay purposely to avoid double scan");
                AddDelay(2);
                scanPromptClosed();
                
            }
            else if (BarcodeState == BarcodeScanState.ScanVerifyStaffCard)
            {
                barcodeScanner.disableDevice();
                BarcodeState = BarcodeScanState.ScanItem;
                hostScanner.Visible = false;
                hostScanner.SendToBack();
                UserMst um = new UserMst();
                if (!IsValidStaffToken(barcodeText, out um))
                {
                    //showErrorMessage("Barcode " + barcodeText + " is not registered yet", "Ask for Assistance");
                    //AppendLog("Barcode " + barcodeText + " is not registered yet");
                    scanPromptClosed();
                    AppendLog("Item is removed");
                    StateInit();
                    return;
                    
                }
                //isPromptShowing = false;
                verifiedAge = tmpAgeToVerify;
                AddItemToOrder(tempItemForAgeVerification, false);
                
            }
            else if (BarcodeState == BarcodeScanState.ScanVerifyNRIC)
            {
                barcodeScanner.disableDevice();
                BarcodeState = BarcodeScanState.ScanItem;
                hostScanner.Visible = false;
                hostScanner.SendToBack();
                //init 
                isAgeVerified = false;
                verifiedAge = 0;

                //Verify NRIC
                int birthYear = 0;
                bool isForeigner = false;
                string statusVerification = "";
                bool isNRICValid = PowerPOS.NRICController.ICVerification(barcodeText,
                    out birthYear,
                    out isForeigner,
                    out status);
                
                if (!isNRICValid )
                {
                    AppendLog("Invalid IC, Please Try Again");
                    showErrorMessage("Invalid IC, Please Try Again", "Please Ask Assistance");
                    scanPromptClosed();
                    startAgeVerification();
                    
                    return;
                }

                if (isForeigner)
                {
                    AppendLog("Customer IC is not Singaporean or PR, pls ask for Staff Assistance");
                    showErrorMessage("Customer IC is not Singaporean or PR, pls ask for Staff Assistance", "Please Ask Assistance");
                    scanPromptClosed();
                    startAgeVerification();
                    return;
                }

                if ((DateTime.Now.Year - birthYear) < tmpAgeToVerify)
                {
                    AppendLog("Customer IC age is below Age Restriction (" + tmpAgeToVerify.ToString() + ")");
                    showErrorMessage("Customer IC age is below Age Restriction (" + tmpAgeToVerify.ToString() + ")", "Please Ask Assistance");
                    scanPromptClosed();
                    startAgeVerification();
                    return;
                }
                
                verifiedAge = DateTime.Now.Year - birthYear;
                AddItemToOrder(tempItemForAgeVerification,false);
                return;
            }
        }

        private void AddDelay(int sec)
        {
            DateTime endTime = DateTime.Now.AddSeconds(sec);
            while (DateTime.Now < endTime)
            {
                Thread.Sleep(10);
                Application.DoEvents();
            }
        }

        private void scanPromptClosed()
        {
            isPromptShowing = false;
            if (frmStartKiosk.SubState == frmStartKiosk.KioskState.PROCESSING_WAITFORITEMSCAN && lastError == "")
            {
                EnableBarcodeScanner(true);
                lastScannedBarcode = "";
                EnableKeyCode(true);
                if (pos.GetNoOfItem() > 0)
                {
                    EnableCheckOut(true);
                    enabledTimerIdle(false);
                }
                else
                {
                    AppendLog("Enable Timer Idle");
                    enabledTimerIdle(true);
                    AppendLog("Check Out is not Shown because no item yet");
                }
                
                //EnableBarcodeKeycodeCheckOut(true);
            }
        }

        #endregion 

        #region ItemLogic



        private void startAgeVerification()
        {
            
            //Check the age restrictions compare to verified age

            AppendLog("Prompt Age Verification & disable Timer Idle");
            isPromptShowing = true;
            enabledTimerIdle(false);
            
            frmLoginKiosk fLoginKiosk = new frmLoginKiosk();
            DialogResult dr = fLoginKiosk.ShowDialog();
            
            isPromptShowing = false;


            if (dr == DialogResult.Cancel)
            {
                AppendLog("User press cancel");
                AppendLog("Item is removed");
                StateInit();
                return;
            }

            if (fLoginKiosk.IsStaffVerify)
            {
                AppendLog("Ask To Scan Staff Card for Age Verifications");
                BarcodeState = BarcodeScanState.ScanVerifyStaffCard;
                OpenBarcodeScanner("Age Above " + tmpAgeToVerify.ToString() + "?", "Before scanning your staff card, Pls verify customer IC otherwise press cancel");
            }
            else
            {
                AppendLog("Ask To Scan NRIC for Age Verifications");
                BarcodeState = BarcodeScanState.ScanVerifyNRIC;
                OpenBarcodeScanner("Please scan your IC", "Only For Singaporean / PR, otherwise press cancel");
            }

        }


        private bool AddItemToOrder(List<ItemStruct> res, bool isFromKeyCode)
        {
            
            if (res.Count == 0)
                return true;
            
            //Add Item
            
            if (!pos.AddItemListToOrder(res, out status))
            {
                showErrorMessage( "Error." + status, "Please ask assistance");
                return false;
            }
            
            BindGrid();
            
            isPromptShowing = false;
            //enabledTimerIdle(false);
            //if (isFromKeyCode)
            //   scanPromptClosed();

            frmStartKiosk.SubState = frmStartKiosk.KioskState.PROCESSING_WAITFORBAGGING;
            StateInit();
            return true;
           
        }

        #endregion

        private void AddBarcodeScanDelay()
        {
            if (useWeighingScale)
                return;

            int barcodeScannerDelay = getBarcodeScannerDelayFromSettings();
            if (barcodeScannerDelay <= 0)
                return;

            AddDelay(barcodeScannerDelay);
        }

        private int getBarcodeScannerDelayFromSettings()
        {
            String strDelayInSeconds = AppSetting.GetSetting(AppSetting.SettingsName.BarcodeScanner.DelayInSeconds);
            int barcodeScannerDelay = 0;
            if (!int.TryParse(strDelayInSeconds, out barcodeScannerDelay))
                barcodeScannerDelay = 0;
            if (barcodeScannerDelay > 10)
                barcodeScannerDelay = 10;
            
            return barcodeScannerDelay;
        }

    }

    

    public interface IKiosk
    {
        bool PayCash(decimal amt);

        bool PayNonCash(decimal amt, string paymentType);

        void RefreshPayment();

        void ChangeInCash();

        void EndSimulate();

        void CancelNonCashSimulate();
    }

    public interface IScanWeight
    {
        void SetWeightResult(bool IsSuccess, decimal Weight);

        void CancelScanWeight();
    }

    public class ItemReceipt
    {
        public int Quantity { get; set; }
        public String ItemName { get; set; }
        public double Amount { get; set; }
    }
}
