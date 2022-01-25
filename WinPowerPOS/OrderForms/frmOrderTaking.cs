using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using POSDevices;
using WinPowerPOS.MembershipForm;
using WinPowerPOS.LoginForms;
using SubSonic;
//using WinPowerPOS.InstallmentForms;
//using WinPowerPOS.ClassAttendance;
using WinPowerPOS.MembershipForms;
using System.Configuration;
using Features = PowerPOS.Feature;
using System.Threading;
using System.Globalization;
using LanguageManager = WinPowerPOS.Properties.Language;
using PowerPOSLib.PowerPOSSync;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.IO;
using WinPowerPOS.Delivery;
using System.Drawing.Imaging;
using WinPowerPOS.EditBillForms;
using PowerPOS.Controller;
using PowerPOSLib.Container;

namespace WinPowerPOS.OrderForms
{
    public partial class frmOrderTaking : FormBase
    {

        bool GiveDiscountAllowed;
        bool UseCustomInvoiceNo;
        //bool ChangeUnitPriceAllowed;
        decimal PreferedDiscount;
        POSController pos;
        POSController posRefund;
        //Hold Transaction
        POSController[] posHold;
        
        PriceDisplay myDisplay;
        ItemController itemLogic;
        //Controllers


        DataTable dtHotKeys;
        string status;              //To record statuses
        string poleDisplayMessage;   //Message shown in pole display
        //ProgrammableKeyboardController hotKeyCtrl;

        public frmOrderSecondScreen fOrderSecondScreen;
        public frmWaitConfirmSecondary fWaitConfirmSecondary;
        public frmPrintReceipt frmPrintReceipt;
        public int bufPrintReceipt;

        bool ApplyPromo;
        public const int PICT_COL = 2;
        public const int QTY_COL = 5;
        public const int RPRICE_COL = 6;
        public const int DISC_COL = 8;
        public const int DISCPRICE_COL = 7;
        public const int DISCOUNTDETAIL_COL = 9;
        public const int SPCLDISCPRICE_COL = 10;
        public const int TAX_COL = 11;
        public const int ISSPECIAL_COL = 13;
        public const int PREORDER_COL = 14;
        public const int FOC_COL = 15;
        public const int VOID_COL = 19;
        public const int LINECOMMISSION_COL = 16;
        public const int LINECOMMISSION_COL2 = 17;

        private bool IsItemPictureShown = false;
        private bool UseProjectModule = false;
        private bool IsMembership_Compulsory = false;
        private int poleDisplayWidth;
        private bool EnableSecondDiscount = false;
        private bool EnableMultiTierPrice = false;
        private bool EnableMultiTierPriceInGlobalDiscount = false;
        private string lineInfoCaption;
        private bool EnableSecondSalesPerson = false;
        private bool ShowSalesPersonColumn = true;
        public bool isSuccessful = false;
        public string OrderHdrID = "";

        public BackgroundWorker SyncSalesThread;
        public BackgroundWorker SyncMemberThread;
        public BackgroundWorker SyncSendDeliveryOrderThread;
        public BackgroundWorker SyncRatingThread;
        public BackgroundWorker SyncAttendanceThread;
        public BackgroundWorker SyncPointsThread;
        public BackgroundWorker SyncCashRecordingThread;
        public string mode = "";

        private bool IsLoadedFromAppointment = false;
        private bool isRefund = false;
        private Appointment appointment;
        private UserMst salesPersonFromAppt;

        public bool PrintReceipt = false;
        public bool EmailReceipt = false;

        bool isPrintValidation = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SICC.CheckValidationPrinting), false);

        public static DateTime counterStartTime;

        public bool isUserTokenForDiscountScanned = false;

        //public CashRecycler cashRecycler;

        #region "Form event handler"
        public frmOrderTaking()
        {
            frmOrderTaking_Init(Guid.Empty);
        }

        public frmOrderTaking(Guid AppointmentID)
        {
            frmOrderTaking_Init(AppointmentID);
        }

        public frmOrderTaking(bool _isRefund, POSController _posRefund)
        {
            frmOrderTaking_Init(Guid.Empty);
            posRefund = _posRefund;
            isRefund = _isRefund;
            //IsLoadedFromAppointment = true;
        }

        private void frmOrderTaking_Init(Guid AppointmentID)
        {
            ApplyPromo = true;
            //LoadCultureCode();
            InitializeComponent();
            //hotKeyCtrl = new ProgrammableKeyboardController();
            itemLogic = new ItemController();

            myDisplay = new PriceDisplay();
            bool useWindcor = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.UseWindcor), false))
            {
                useWindcor = true;
                myDisplay.useWindcor = true;
            }
            else
            {
                myDisplay.useWindcor = false;
            }

            if (useWindcor)
            {
                myDisplay.FirstLineCommand = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.FirstLineCommand);
                myDisplay.SecondLineCommand = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.SecondLineCommand);
            }

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.DisablePoleDisplay),false))
            {
                myDisplay.setPrinterName("");
            }

            pos = new POSController();
            pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
            pos.AutoCaptureWeightItemAdded += new AutoCaptureWeightItemHandler(pos_AutoCaptureWeightItemAdded);
            posHold = new POSController[3];
            AssignPrivileges();
            UseCustomInvoiceNo = false;
            if (!PointOfSaleInfo.allowLineDisc)
            {
                btnDiscount.Visible = false;
            }

            if (!PointOfSaleInfo.useMembership)
            {
                gbMembership.Visible = false;
                btnSearchMember.Visible = false;
            }

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false))
            {
                btnAttendance.Visible = false;
            }
            if (AppSetting.GetSetting("UseCustomInvoiceNo") == "True")
            {
                UseCustomInvoiceNo = true;
            }

            /*
            if (!PointOfSaleInfo.allowFeedback)
            {
                btnFeedBack.Visible = false;
            }*/
            if (AppSetting.GetSettingFromDBAndConfigFile("TimerLimit") != null)
            {
                int.TryParse(AppSetting.GetSettingFromDBAndConfigFile("TimerLimit").ToString(), out TIMER_LIMIT);
            }

            if (AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayText) != null && AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayText).Length > 0)
            {
                poleDisplayMessage = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayText);
            }
            else
            {
                poleDisplayMessage = "Welcome!";
            }

            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayLinesLength), out poleDisplayWidth))
            {
                poleDisplayWidth = 20;
            }

            IsItemPictureShown = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false);
            UseProjectModule = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Project.ProjectModule), false);
            IsMembership_Compulsory = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.Membership_Compulsory), false);
            EnableSecondDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
            EnableMultiTierPrice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricing), false);
            EnableMultiTierPriceInGlobalDiscount = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricingInGlobalDiscount), false);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                btnSearchMember.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper();
                gbMembership.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString();
                btnMembershipRemark.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Info";
                btnNewMember.Text = "NEW " + AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper();
            }
            //else
            //{
            //    btnSearchMember.Text = "MEMBER";
            //    gbMembership.Text = "Membership";
            //    btnMembershipRemark.Text = "Member Info";
            //    btnNewMember.Text = "NEW MEMBER";
            //}
            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.IsReplaceConfirmTextButton), false) &&
                AppSetting.GetSetting(AppSetting.SettingsName.Invoice.ReplaceConfirmTextButtonWith) != "")
            {
                btnConfirm.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.ReplaceConfirmTextButtonWith);
            }
            btnQuotations.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableQuotation), false);

            EnableSecondSalesPerson = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnableSecondSalesPerson), false);
            ShowSalesPersonColumn = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideSalespersonColumn), false);
            #region *) For Appointment
            if (!AppointmentID.Equals(Guid.Empty))
            {
                appointment = new Appointment(AppointmentID);
                if (appointment != null)
                {
                    IsLoadedFromAppointment = true;
                    salesPersonFromAppt = new UserMst(appointment.SalesPersonID);
                    if (salesPersonFromAppt == null || !salesPersonFromAppt.IsLoaded)
                    {
                        MessageBox.Show("Error Loading sales person from Appointment table. Sales person ID not recognized.");
                        Logger.writeLog("Error Loading sales person from Appointment table. Sales person ID not recognized.");
                        isSuccessful = false;
                        this.Close();
                        return;
                    }
                    //lblSalesPersonName.Text = salesPersonFromAppt.DisplayName;
                    pos.AssignMembership(appointment.MembershipNo, out status);
                    var items = new AppointmentItemCollection();
                    items.Where(AppointmentItem.Columns.AppointmentId, appointment.Id);
                    items.Where(AppointmentItem.Columns.Deleted, false);
                    items.Load();


                    decimal totalspending = 0;
                    foreach (var item in items)
                    {
                        var vi = new ViewItem(ViewItem.Columns.ItemNo, item.ItemNo);
                        pos.AddServiceItemToOrder(vi, item.Quantity, item.UnitPrice, 0, ApplyPromo, "", out status);
                        totalspending += item.Quantity * item.UnitPrice;
                    }

                    //addroom charge
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false))
                    {
                        var resource = new Resource(appointment.ResourceID);

                        if (resource.ResourceID != ResourceController.ROOM_DEFAULT)
                        {
                            decimal resourceCharge = resource.RoomCharge ?? 0;
                            if (totalspending < resource.MinSpending)
                            {
                                resourceCharge += resource.MinSpendingCharge ?? 0;
                            }

                            Item ROOM_CHARGE = new Item("ROOM_CHARGE");
                            pos.AddItemToOrderWithPrice(ROOM_CHARGE, 1, resourceCharge, 0, ApplyPromo, out status);
                        }
                    }
                    //set sales person
                    foreach (OrderDet i in pos.myOrderDet)
                    {
                        i.SalesPerson = salesPersonFromAppt.UserName;
                    }
                }
            }
            #endregion

            btnViewAppointments.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentEnable), false);
            
            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                groupBox5.Text = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

            SyncPointsThread = new BackgroundWorker();
            SyncPointsThread.DoWork += new DoWorkEventHandler(SyncPointsThread_DoWork);
            SyncPointsThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SyncPointsThread_RunWorkerCompleted);

            DataTable ListPM = GetListPriceMode();
            if (ListPM.Rows.Count > 1)
            {
                btnPriceMode.Visible = true;
                btnPriceMode.Text = "NORMAL";
                btnPriceMode.Tag = "NORMAL";
            }
            else
            {
                btnPriceMode.Visible = false;
            }
        }

        private void OrderTaking_Activated(object sender, EventArgs e)
        {
            llCashier.Text = UserInfo.displayName;
            txtBarcode.Focus();
        }

        private void OrderTaking_Load(object sender, EventArgs e)
        {
            //Resize the screen elements accordingly depending on the screen resolution
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            this.Scale(new SizeF(screenWidth / 800.0F, screenHeight / 600.0F));

            //Load Rounding Preference that the customer like
            if (AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference") != null)
            {
                POSController.RoundingPreference = AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference").ToString();
            }
            else
            {
                POSController.RoundingPreference = "";
            }

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
            {
                fOrderSecondScreen = new frmOrderSecondScreen();
                fOrderSecondScreen.frmOrderTaking = this;
                fOrderSecondScreen.Show();

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.RequireCustomerConfirm), false))
                {
                    fWaitConfirmSecondary = new frmWaitConfirmSecondary();
                }
            }

            #region grid Setting
            DataGridViewImageColumn pictureColumn = new DataGridViewImageColumn();
            pictureColumn.Name = "Photo";
            pictureColumn.Width = 60;
            pictureColumn.Visible = true;
            dgvPurchase.Columns.Insert(2, pictureColumn);

            if (!IsItemPictureShown)
            {
                pictureColumn.Visible = false;
            }

            dgvPurchase.Columns["DiscountDetail"].Visible = EnableSecondDiscount;
            dgvPurchase.Columns["Discount"].Visible = !EnableSecondDiscount;
            dgvPurchase.Columns[SPCLDISCPRICE_COL].Visible = EnableMultiTierPrice;
            dgvPurchase.Columns[LINECOMMISSION_COL2].Visible = EnableSecondSalesPerson;

            for (int i = 0; i < dgvPurchase.Columns.Count; i++)
            {
                if (dgvPurchase.Columns[i].Visible)
                {
                    string tmp = AppSetting.GetSetting(dgvPurchase.Columns[i].Name + "_Width");
                    int wdth = 0;
                    if (tmp != null && int.TryParse(tmp.ToString(), out wdth))
                    {
                        dgvPurchase.Columns[i].Width = wdth;
                    }
                }
            }


                #region Hide Fields
                dgvPurchase.Columns[LINECOMMISSION_COL].Visible = ShowSalesPersonColumn;
            dgvPurchase.Columns[TAX_COL].Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideTaxColumn),false);
            dgvPurchase.Columns["Discount"].Visible = false;
            dgvPurchase.Columns["DiscountDetail"].Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideDiscountColumn), false);
            dgvPurchase.Columns[ISSPECIAL_COL].Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideSpecialColumn), false); 
            #endregion

            //update font
            int fontSize = 12;
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.FontSize), out fontSize))
                fontSize = 12;
            dgvPurchase.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", fontSize);
            dgvPurchase.RowsDefaultCellStyle.Font = new Font("Microsoft Sans Serif", fontSize);

            #endregion
            //load hotkeys
            populateHotKeys();
            //display info
            llCashier.Text = UserInfo.displayName;
            lblPointOfSaleName.Text = PointOfSaleInfo.PointOfSaleName;

            dgvPurchase.AutoGenerateColumns = false;

            if (!IsLoadedFromAppointment)
            {
                clearControls();
            }


            myDisplay.ClearScreen();
            myDisplay.SendCommandToDisplay(poleDisplayMessage);

            gbTotalQuantity.Visible = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowTotalQuantityInSalesScreen), false))
            {
                gbTotalQuantity.Visible = true;
            }

            gbTotalItems.Visible = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowTotalItemInSalesScreen), false))
            {
                gbTotalItems.Visible = true;
            }

            gbLineInfo.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ShowInInvoice), false);
            if (!string.IsNullOrEmpty(lineInfoCaption)) gbLineInfo.Text = lineInfoCaption;

            SyncClientController.Load_WS_URL();

            if (IsLoadedFromAppointment || isRefund)
            {
                if (isRefund)
                    pos = posRefund;
                BindGrid();
                if (pos.GetMemberInfo() != null)
                    DisplayMembershipToScreen(pos.GetMemberInfo());

            }
            /*
             * Button sync is removed, Sukanto doesn't want redundant button.
             * Sync membership moved into frmAddMember, and triggered whien the form is open.
            */
            //bool isLocalhost = SyncClientController.WS_URL.StartsWith("http://localhost"); //|| SyncClientController.WS_URL.StartsWith("http://127.0.0.1");
            //if (isLocalhost)
            //{
            //    btnSync.Visible = false;
            //}
            ////btnSync.Visible = true;
            //progressBarSyncMembership.Maximum = 100;
            //progressBarSyncMembership.Value = 0;
            
            var ViewAppointmentText = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentText);
            btnViewAppointments.Text =  ViewAppointmentText != null ? ViewAppointmentText : "View Appointments";

            #region *) For Appointment
            if (IsLoadedFromAppointment && !SyncPointsThread.IsBusy)
                SyncPointsThread.RunWorkerAsync();
            #endregion

            btnPreview.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowPreviewButton), false);

        }
        #endregion

        #region "Privileges Related"
        private void AssignPrivileges()
        {
            if (AppSetting.GetSettingFromDBAndConfigFile("ShowFOCColumn") != null
                && AppSetting.GetSettingFromDBAndConfigFile("ShowFOCColumn").ToString().ToLower() == "no")
            {
                //dgvPurchase.Columns[FOC_COL].Visible = false;
                dgvPurchase.Columns["FOC"].Visible = false;
            }
            else
            {
                //dgvPurchase.Columns[FOC_COL].Visible = true;
                dgvPurchase.Columns["FOC"].Visible = true;
            }
            if (AppSetting.GetSettingFromDBAndConfigFile("ShowPreOrderColumn") != null
                && AppSetting.GetSettingFromDBAndConfigFile("ShowPreOrderColumn").ToString().ToLower() == "no")
            {
                //dgvPurchase.Columns[PREORDER_COL].Visible = false;
                dgvPurchase.Columns["IsPreOrder"].Visible = false;
            }
            else
            {
                //dgvPurchase.Columns[PREORDER_COL].Visible = true;
                dgvPurchase.Columns["IsPreOrder"].Visible = true;
            }
            if (PrivilegesController.HasPrivilege(PrivilegesController.GIVE_DISCOUNT, UserInfo.privileges))
            {
                GiveDiscountAllowed = true;
            }
            else
            {
                GiveDiscountAllowed = false;
            }

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.UseDefaultPayment), false))
            {
                btnDefault.Visible = false;
            }
            else
            {
                btnDefault.Text = AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.DefaultPaymentType) + " Tender";
            }

            btnCashIn.Visible = false;
            btnCashOut.Visible = false;
            if (PrivilegesController.HasPrivilege(PrivilegesController.CASH_RECORDING, UserInfo.privileges))
            {
                btnCashIn.Visible = true;
                btnCashOut.Visible = true;
            }


            //dgvPurchase.Columns[VOID_COL].Visible = true;
        }
        #endregion

        #region "non-order related (Cash Recordings, check out etc) "
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to quit?", "", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                myDisplay.ClearScreen();
                this.Close();
            }
        }
        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                frmCheckOut frmCk = new frmCheckOut();
                frmCk.ShowDialog();
                bool isSuccessful = frmCk.IsSuccessful;
                frmCk.Dispose();
                if (isSuccessful)
                {
                    this.MdiParent.Close();
                    this.Close();
                }
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBarcode.Focus();
            }
            //close form and back to login page

        }
        private void btnCashRec_Click(object sender, EventArgs e)
        {
            try
            {
                frmCashRecording frm = new frmCashRecording();
                frm.ShowDialog();
                frm.Dispose();
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBarcode.Focus();
            }
        }
        #endregion

        #region "Quick Access Buttons & Programmable Keyboard"
        private void frmOrderTaking_KeyDown(object sender, KeyEventArgs e)
        {
            //mapKeyPress(e);
            if (e.KeyCode == Keys.Escape)
            {
                pnlShowChange.Visible = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                {
                    fOrderSecondScreen.UpdateChange("0");
                }
            }
        }

        private void SetImage(Button btn, string itemNo)
        {
            if (IsItemPictureShown)
            {
                var theItem = new Item(Item.Columns.ItemNo, itemNo);
                if (theItem.ItemImage != null)
                {
                    btn.Image = ResizeImage(Image.FromStream(new System.IO.MemoryStream(theItem.ItemImage)), new Size(btn.Height, btn.Height));
                    btn.ImageAlign = ContentAlignment.MiddleLeft;
                    btn.TextAlign = ContentAlignment.MiddleRight;
                }
            }
        }

        private int clickNo = 0;

        private void populateHotKeys()
        {
            HotKeysController hkCtrl = new HotKeysController();
            string imageFilePath;

            dtHotKeys = hkCtrl.GetDataTables();
            if (dtHotKeys != null)
            {
                for (int i = 0; i < dtHotKeys.Rows.Count; i++)
                {
                    switch (dtHotKeys.Rows[i]["keyname"].ToString())
                    {
                        case "Quick Items: Row 01":
                            q11.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q11, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                        case "Quick Items: Row 02":
                            q12.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q12, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;

                        case "Quick Items: Row 03":
                            q21.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q21, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                        case "Quick Items: Row 04":
                            q22.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q22, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                        case "Quick Items: Row 05":
                            q31.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q31, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;

                        case "Quick Items: Row 06":
                            q32.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q32, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                        case "Quick Items: Row 07":
                            q41.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q41, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                        case "Quick Items: Row 08":
                            q42.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q42, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;

                        case "Quick Items: Row 09":
                            q51.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q51, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                        case "Quick Items: Row 10":
                            q52.Text = dtHotKeys.Rows[i]["itemname"].ToString();
                            SetImage(q52, dtHotKeys.Rows[i]["ItemNo"].ToString());
                            break;
                    }
                }
            }
        }
        private void btnF1_Click(object sender, EventArgs e)
        {
            try
            {
                //Handle functional keys --
                string buttonName = ((Button)(sender)).Name.ToString();
                DataRow[] dr;
                dr = dtHotKeys.Select("keycode='" + buttonName + "'");
                if (dr != null && dr.Length > 0)
                {
                    ViewItem myItem = new ViewItem(ViewItem.Columns.ItemNo, dr[0]["itemno"].ToString());
                    Item item = new Item(myItem.ItemNo);
                    if (!pos.IsRestricted(item, out status))
                    {
                        frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                        frmError.lblMessage = status;
                        frmError.ShowDialog();
                        //txtBarcode.Focus();
                        //return false;
                    }
                    else
                    {
                        if (!pos.AddItemToOrderWithPriceMode(myItem, 1, PreferedDiscount, ApplyPromo, this.PriceMode, out status))
                        {
                            //alert error message.                    
                            MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            return;
                        }
                        else
                        {
                            if (status != "")
                            {
                                MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            //pos.ApplyMembershipDiscount();

                            // LowQuantity Feature
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                            {
                                if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                                {
                                    MessageBox.Show(myItem.ItemName + " is only left with " + ItemController.GetStockOnHand(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                }

                                //if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                                //{
                                //    MessageBox.Show(myItem.ItemName + " is only left with " + ItemController.GetStockOnHand(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                //}
                            }
                        }
                        BindGrid();
                        //PriceDisplay //myDisplay = new PriceDisplay();
                        myDisplay.ClearScreen();

                        if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory)
                        {

                            OrderDetCollection tmpCol = pos.FetchUnsavedOrderDet();
                            for (int j = tmpCol.Count - 1; j >= 0; j--)
                            {
                                OrderDet od = tmpCol[j];
                                if (od.ItemNo == myItem.ItemNo)
                                {
                                    Logger.writeLog("Show Open Item Price " + od.Amount.ToString("N2"));
                                    myDisplay.ShowItemPrice(
                                        myItem.ItemName,
                                        (double)od.Amount,
                                        (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                                }
                            }
                        }
                        else
                        {

                            myDisplay.ShowItemPrice(
                                myItem.ItemName,
                                (double)myItem.RetailPrice,
                                (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                        }

                        /*myDisplay.ShowItemPrice(myItem.ItemName, (double)myItem.RetailPrice,
                            (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);*/
                        updateTotalAmount();
                    }
                }
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBarcode.Focus();
            }
        }
        /*
        private void mapKeyPress(KeyEventArgs e)
        {
            if (POSController.ENABLE_PROGRAMMABLE_KEYBOARD)
            {
                try
                {
                    if (e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.ControlKey && ((int)e.KeyCode) != 18) //alt
                    {
                        int index = 0;

                        String newItemno = hotKeyCtrl.GetItemNo(e);
                        if (newItemno == "")
                        {
                            //do nothin' it is not a registered key
                        }
                        else if (newItemno == ProgrammableKeyboardController.FUNCTION_CODE)
                        {
                            string functionName = hotKeyCtrl.GetFunctionKey(e);
                            switch (functionName)
                            {
                                case ProgrammableKeyboardController.CHANGE_PRICE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;

                                        dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells["Price"];
                                        dgvPurchase.BeginEdit(true);

                                        e.SuppressKeyPress = true;
                                    }
                                    break;
                                case ProgrammableKeyboardController.CHANGE_DISCOUNT:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;

                                        dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells["Discount"];
                                        dgvPurchase.BeginEdit(true);

                                        e.SuppressKeyPress = true;
                                    }
                                    break;
                                case ProgrammableKeyboardController.CHANGE_QTY:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;

                                        dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells["Quantity"];
                                        dgvPurchase.BeginEdit(true);

                                        e.SuppressKeyPress = true;
                                    }
                                    break;
                                case ProgrammableKeyboardController.CHANGE_TOTAL_DISCOUNT:
                                    //txtDiscount.Focus();
                                    break;
                                case ProgrammableKeyboardController.CHANGE_TOTAL_TAX:
                                    //txtTax.Focus();
                                    break;
                                case ProgrammableKeyboardController.SCAN_BARCODE:
                                    txtBarcode.Focus();
                                    break;
                                case ProgrammableKeyboardController.DELETE_ORDER_LINE:
                                    //dgvPurchase_KeyPress(this, new KeyPressEventArgs((char)Keys.Back));
                                    e.SuppressKeyPress = true;
                                    break;
                                case ProgrammableKeyboardController.DOWN_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;
                                        if (index < dgvPurchase.Rows.Count - 1)
                                        {
                                            index += 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells[dgvPurchase.SelectedCells[0].ColumnIndex];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.UP_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;
                                        if (index > 0)
                                        {
                                            index -= 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells[dgvPurchase.SelectedCells[0].ColumnIndex];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.LEFT_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].ColumnIndex;
                                        if (index > 0 && dgvPurchase.Columns[index - 1].Visible == true)
                                        {
                                            index -= 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[dgvPurchase.SelectedCells[0].RowIndex].Cells[index];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.RIGHT_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].ColumnIndex;
                                        if (index < dgvPurchase.Columns.Count - 1 && dgvPurchase.Columns[index + 1].Visible == true)
                                        {
                                            index += 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[dgvPurchase.SelectedCells[0].RowIndex].Cells[index];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.CANCEL_ORDER:
                                    btnCancel_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CASH_RECORDING:
                                    btnCashRec_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CLOSE_COUNTER:
                                    btnCheckOut_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CLOSE_CASH_BILL:
                                    btnLogOut_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.STOCK_IN:
                                    // frmInventoryReceive frm = new frmInventoryReceive();
                                    // frm.ShowDialog();
                                    // frm.Dispose();
                                    //break;
                            }
                        }
                        else if (!pos.AddItemToOrder(new Item(newItemno), 1, PreferedDiscount, ApplyPromo, out status))
                        {
                            BindGrid();
                            e.SuppressKeyPress = true;
                            for (int l = 0; l < dgvPurchase.Rows.Count; l++)
                            {
                                if (dgvPurchase.Rows[l].Cells["ItemNo"].Value.ToString() == newItemno)
                                {
                                    dgvPurchase.CurrentCell = dgvPurchase.Rows[l].Cells[1];
                                }
                            }
                            txtBarcode.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                        }
                    }
                }

                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Focus();
                }
            }
        }
        */
        #endregion

        public bool ResetDiscount()
        {
            bool result = true;
            for (int i = 0; i < pos.myOrderDet.Count; i++)
            {
                OrderDet od = pos.myOrderDet[i];
                result &= pos.ApplyDiscountOrderDet("0", ref od, true);
            }
            pos.UndoPromo();
            pos.ApplyMembershipDiscount();
            pos.ApplyPromo();
            return result;
        }

        #region "Scan and Search"
        private void btnSearchMember_Click(object sender, EventArgs e)
        {
            clickNo = 0;

            string useCard = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForMembership);
            if (useCard != null &&
                useCard.ToLower() == "yes")
            {
                #region -= USE SWIPE CARD =-
                string buffer = "";

                frmReadMembershipMSR f = new frmReadMembershipMSR();
                f.ShowDialog();
                buffer = f.buffer;
                f.Dispose();
                if (buffer == "") return;
                Membership member;
                if (MembershipController.IsMembersNRIC
                    (buffer, out member))
                {


                    if (member.ExpiryDate.HasValue && member.ExpiryDate.Value.AddDays(1) >= DateTime.Now)
                    {
                        //havent expired
                        addMemberToPOS(member.MembershipNo);
                        PreferedDiscount = pos.GetPreferredDiscount();
                        return;
                    }
                    else if (member.ExpiryDate.HasValue)
                    {
                        //expired already
                        //prompt window to allow bypass?
                        DialogResult dr = MessageBox.Show("This member has already expired on " + member.ExpiryDate.Value.ToString("dd MMM yyyy") + ". Do you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            addExpiredMemberToPOS(member.MembershipNo);
                            PreferedDiscount = pos.GetPreferredDiscount();
                            return;
                        }
                    }
                }
                else
                {

                    Application.DoEvents();
                    MessageBox.Show("Members card not recognized");

                    return;
                }
                #endregion
            }
            else
            {
                if (txtBarcode.Text == "")
                //if (txtSearch.Text == "")
                {
                    return;
                }

                if (CheckMembershipFromScannedBarcode())
                {
                    ResetDiscount();
                    pos.ApplyMembershipDiscount();
                    PreferedDiscount = pos.GetPreferredDiscount();
                    string pricelevel = pos.GetMemberInfo().MembershipGroup.Userfld1;
                    if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                    {
                        SpecialDiscount sd = new SpecialDiscount(pricelevel);
                        btnDiscount.Text = sd.DiscountLabel;
                        
                    }
                    if (pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "")
                        pos.ApplyCustomerPriceAll(pos.GetMemberInfo().MembershipNo);
                    BindGrid();
                    txtBarcode.Focus();
                    return;
                }
                frmAddMember f = new frmAddMember();
                f.searchReq = txtBarcode.Text;// txtSearch.Text;
                f.ShowDialog();
                string tmpMembershipNo = f.membershipNo;
                if (tmpMembershipNo != "")
                {
                    bool hasExpired;
                    DateTime ExpiryDate;
                    if (MembershipController.IsExistingMember(tmpMembershipNo, out hasExpired, out ExpiryDate))
                    {
                        addMemberToPOS(tmpMembershipNo);
                        PreferedDiscount = pos.GetPreferredDiscount();
                    }
                    else
                    {
                        if (hasExpired)
                        {
                            //prompt window to allow bypass?
                            DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ". Do you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                addExpiredMemberToPOS(tmpMembershipNo);
                                PreferedDiscount = pos.GetPreferredDiscount();
                            }
                        }
                    }
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    //txtSearch.Text = "";                
                }
                f.Dispose();
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtBarcode.Text == "")
            //if (txtSearch.Text == "")
            {
                return;
            }

            clickNo = 0;
            frmAddItem myAddItem = new frmAddItem();
            myAddItem.searchReq = txtBarcode.Text.Replace(' ', '%');
            myAddItem.PreferedDiscount = PreferedDiscount;
            myAddItem.ShowDialog();
            ViewItem myItem;
            if (myAddItem != null && myAddItem.itemNumbers != null)
            {
                ArrayList ItemFree = new ArrayList();
                string statusCollection = "";

                #region Check Item Restriction
                if (!pos.CheckRestricted(myAddItem.itemNumbers, out ItemFree, out statusCollection, out status))
                {
                    MessageBox.Show("Error check restriction : " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Focus();
                    return;
                }

                if (statusCollection != "")
                {
                    //MessageBox.Show(statusCollection, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    frmErrorMessageWithTextArea frm = new frmErrorMessageWithTextArea();
                    frm.lblMessage = statusCollection;
                    frm.ShowDialog();
                }
                #endregion
                //for (int i = 0; i < myAddItem.itemNumbers.Count; i++)
                for (int i = 0; i < ItemFree.Count; i++)
                {
                    myItem = new ViewItem(ViewItem.Columns.ItemNo, ItemFree[i]);
                    if (!pos.AddItemToOrderWithPriceMode(myItem, 1, (decimal)PreferedDiscount, ApplyPromo,this.PriceMode, out status))
                    {
                        MessageBox.Show("Error encountered while adding items. Please try again. " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        return;
                    }
                    else
                    {
                        //pos.ApplyMembershipDiscount();
                        myDisplay.ClearScreen();

                        if (myItem.IsServiceItem.GetValueOrDefault(false) && myItem.IsInInventory)
                        {
                            OrderDetCollection tmpCol = pos.FetchUnsavedOrderDet();
                            for (int j = tmpCol.Count - 1; j >= 0; j--)
                            {
                                OrderDet od = tmpCol[j];
                                if (od.ItemNo == myItem.ItemNo)
                                {
                                    myDisplay.ShowItemPrice(
                                        myItem.ItemName,
                                        (double)od.Amount,
                                        (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                                }
                            }
                        }
                        else
                        {
                            myDisplay.ShowItemPrice(
                                myItem.ItemName,
                                (double)myItem.RetailPrice,
                                (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                        }





                        // LowQuantity Feature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                        {
                            if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                            {
                                MessageBox.Show(myItem.ItemName + " is only left with " + ItemController.GetStockOnHand(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                            }
                        }

                        txtBarcode.Focus();
                    }
                }
                txtBarcode.Text = "";
                //txtSearch.Text = "";
            }
            myAddItem.Dispose();
            BindGrid();
        }

        /*
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //return.btnSearch_Click(this, new EventArgs());
            }
        }*/

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    //Ignore blanks....
                    if (txtBarcode.Text == "")
                    {
                        txtBarcode.Focus();
                        return;
                    }
                    /*
                    if (txtBarcode.Text.Length > 2 &&
                        txtBarcode.Text.Substring(0, 2).ToUpper() == "OR")
                    {
                        LoadReceiptFromBarcode();
                        txtBarcode.Focus();
                        return;
                    }*/
                    
                    //Logger.writeLog("Promo Log - 1. Start Checking Membership Barcode");
                    if (CheckMembershipFromScannedBarcode())
                    {
                        pos.ApplyMembershipDiscount();
                        txtBarcode.Focus();
                        PreferedDiscount = pos.GetPreferredDiscount();
                        return;
                    }
                    //Logger.writeLog("Promo Log - 2. Checking Membership Signup");

                    //MEMBERSHIP SIGNUP
                    if (txtBarcode.Text == MembershipController.MEMBERSHIP_SIGNUP_BARCODE)
                    {

                        if (MembershipSignup(true))
                        {
                            pos.ApplyMembershipDiscount();
                            txtBarcode.Focus();
                            return;
                        }
                    }

                    //Logger.writeLog("Promo Log - 3. Checking Membership Renewal");
                    if (txtBarcode.Text == MembershipController.RENEWAL_BARCODE)
                    {

                        if (MembershipRenewal(true))
                        {
                            txtBarcode.Focus();
                            return;
                        }
                    }

                    //Check Item Group barcode
                    //Logger.writeLog("Promo Log - 4. Checking Barcode For Item Group");
                    if (CheckBarcodeFromItemGroup()) return;

                    //Logger.writeLog("Promo Log - 5. Checking Barcode For Promo Barcode");
                    if (CheckBarcodeFromPromoBarcode()) return;

                    //Logger.writeLog("Promo Log - 6. Checking Barcode For Refund");
                    if (CheckBarcodeForRefund()) return;
                    //Logger.writeLog("Checking Barcode Finish ");
                    AddItemToOrder();
                    //Logger.writeLog("Add Item To Order Finish ");
                    txtBarcode.Focus();
                    return;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Focus();
                }
            }
        }

        private bool CheckBarcodeFromPromoBarcode()
        {
            //
            string status;
            string barcode = "";
            barcode = txtBarcode.Text;
            int qtyBar = 1;
            if (barcode.IndexOf('*') > 0)
            {
                string[] barcodeSplit = barcode.Split('*');
                string qty = barcodeSplit[0].ToString();
                string realBarcode = barcodeSplit[1].ToString();
                if (!Int32.TryParse(qty, out qtyBar))
                {
                    MessageBox.Show("Error: Please input correct Quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Focus();
                    return false;
                }

                barcode = realBarcode;
            }
            //Fix to find the enabled and not deleted only 
            PromoCampaignHdrCollection promoCol = new PromoCampaignHdrCollection();
            promoCol.Where(PromoCampaignHdr.Columns.Barcode, barcode);
            promoCol.Where(PromoCampaignHdr.Columns.Enabled, true);
            promoCol.Where(PromoCampaignHdr.Columns.Deleted, false);
            promoCol.Load();

            //PromoCampaignHdr it = new PromoCampaignHdr(PromoCampaignHdr.Columns.Barcode, txtBarcode.Text.Trim());
            for (int z = 0; z < promoCol.Count; z++)
            {
                PromoCampaignHdr it = promoCol[z];

                if (it.IsLoaded && !it.IsNew && it.Deleted.HasValue && !it.Deleted.Value)
                {
                    if (DateTime.Now <= it.DateFrom || DateTime.Now >= it.DateTo)
                    {
                        continue;
                    }

                    //Load Check Location
                    PromoOutletCollection po = new PromoOutletCollection();
                    po.Where(PromoOutlet.Columns.PromoCampaignHdrID, it.PromoCampaignHdrID);
                    po.Where(PromoOutlet.Columns.OutletName.Trim(), PointOfSaleInfo.OutletName.Trim());
                    po.Where(PromoOutlet.Columns.Deleted, false);
                    po.Load();
                    if (po.Count <= 0)
                    {
                        continue;
                    }

                    //Load the item group map....

                    PromoCampaignDetCollection itG = new PromoCampaignDetCollection();
                    itG.Where(PromoCampaignDet.Columns.PromoCampaignHdrID, it.PromoCampaignHdrID);
                    itG.Where(PromoCampaignDet.Columns.Deleted, false);
                    itG.Load();

                    //if (itG.Count != 1)
                    //    return false;
                    ArrayList ItemList = new ArrayList();
                    ArrayList ItemFree = new ArrayList();

                    //loop through and add the item
                    for (int i = 0; i < itG.Count; i++)
                    {
                        if (itG[i].ItemNo != null && itG[i].ItemNo != "")
                        {
                            if (!pos.IsRestricted(new Item(itG[i].ItemNo), out status))
                            {
                                frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                                frmError.lblMessage = status;
                                frmError.ShowDialog();
                                //txtBarcode.Focus();
                                //return false;
                            }
                            else
                            {
                                if (!pos.AddItemToOrderWithPriceMode(
                                    new ViewItem(ViewItem.Columns.ItemNo, itG[i].ItemNo),
                                    (int)itG[i].UnitQty*qtyBar, pos.GetPreferredDiscount(),
                                    true, this.PriceMode, out status))
                                {
                                    MessageBox.Show("Error adding item to order: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtBarcode.Focus();
                                    return false;
                                }
                            }
                        }
                        else if (itG[i].ItemGroupID != null && itG[i].ItemGroupID != 0)
                        {
                            //Load the item group map....
                            ItemGroupMapCollection itGM = new ItemGroupMapCollection();
                            itG.Where(ItemGroupMap.Columns.ItemGroupID, itG[i].ItemGroupID);
                            itG.Where(ItemGroupMap.Columns.Deleted, false);
                            itG.Load();

                            //loop through and add the item
                            for (int j = 0; j < itG.Count; j++)
                            {
                                if (!pos.IsRestricted(new Item(itGM[j].ItemNo), out status))
                                {
                                    frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                                    frmError.lblMessage = status;
                                    frmError.ShowDialog();
                                    //txtBarcode.Focus();
                                    //return false;
                                }
                                else
                                {

                                    if (!pos.AddItemToOrderWithPriceMode(
                                        new ViewItem(ViewItem.Columns.ItemNo, itGM[j].ItemNo),
                                        itGM[j].UnitQty*qtyBar, pos.GetPreferredDiscount(),
                                        true, this.PriceMode, out status))
                                    {
                                        MessageBox.Show("Error adding item to order: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.Focus();
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    BindGrid();
                    txtBarcode.Focus();
                    txtBarcode.Text = "";
                    return true;
                }
            }

            return false;
        }

        private bool CheckBarcodeForRefund()
        {
            //
            string status;

            if (!txtBarcode.Text.StartsWith("OR")) return false;

            bool refundSameOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromSameOutlet), false); ;
            bool refundOtherOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet), false); ;

            if (!refundSameOutlet)
            {
                OrderHdrCollection ohCol = new OrderHdrCollection();
                ohCol.Where(OrderHdr.Columns.OrderRefNo, txtBarcode.Text);
                ohCol.Load();

                if (ohCol.Count > 0)
                {
                    if (ohCol[0].IsVoided)
                    {
                        MessageBox.Show("Order " + ohCol[0].Userfld5 + ".Already Voided.");
                        return false;
                    }
                    POSController posTemp = new POSController(ohCol[0].OrderHdrID);
                    
                    int isRefunded = 0;
                    if (posTemp != null && posTemp.myOrderHdr != null && posTemp.myOrderHdr.OrderHdrID != "")
                    {
                        ArrayList tmpRemove = new ArrayList();
                        foreach (OrderDet od in posTemp.myOrderDet)
                        {
                            decimal qtyRefunded = 0;
                            if (POSController.isRefunded(od.OrderDetID, out qtyRefunded) || od.Quantity < 0)
                            {
                                if (qtyRefunded < od.Quantity)
                                {
                                    if (od.IsPackageRedeemed)
                                    {
                                        od.PackageBreakdownAmount = od.PackageBreakdownAmount / (od.Quantity ?? 1) * (((od.Quantity ?? 0) - qtyRefunded) * -1);
                                    }

                                    od.Quantity = (od.Quantity - qtyRefunded) * -1;
                                    od.ReturnedReceiptNo = "OR" + od.OrderHdrID;
                                    od.RefundOrderDetID = od.OrderDetID;
                                    od.InventoryHdrRefNo = "";
                                    od.IsSpecial = true; // to exclude from promo calculation
                                    OrderDet tempOD = od;
                                    posTemp.CalculateLineAmount(ref tempOD);
                                    
                                }
                                else
                                {
                                    tmpRemove.Add(od.OrderDetID);
                                }
                            }
                            else
                            {
                                if (!POSController.isRefunded(od.OrderDetID, out qtyRefunded))
                                {
                                    if (od.IsPackageRedeemed)
                                    {
                                        od.PackageBreakdownAmount = od.PackageBreakdownAmount * -1;
                                    }

                                    od.Quantity = od.Quantity * -1;
                                    od.ReturnedReceiptNo = "OR" + od.OrderHdrID;
                                    od.RefundOrderDetID = od.OrderDetID;
                                    od.InventoryHdrRefNo = "";
                                    od.IsSpecial = true; // to exclude from promo calculation
                                    OrderDet tempOD = od;
                                    posTemp.CalculateLineAmount(ref tempOD);
                                }
                            }
                        }
                        if (tmpRemove.Count == posTemp.myOrderDet.Count)
                        {
                            MessageBox.Show("All Item in the order " + posTemp.myOrderHdr.Userfld5 + " already Refunded");
                            return false;
                        }
                        else
                        {
                            if (tmpRemove.Count > 0)
                            {
                                for (int i = 0; i < tmpRemove.Count; i++)
                                {
                                    OrderDet od = (OrderDet)posTemp.myOrderDet.Find(tmpRemove[i].ToString());
                                    if (od != null && od.OrderDetID != "")
                                        posTemp.myOrderDet.Remove(od);
                                }
                                status = "Warning. Some items has already refunded.";
                            }
                        }
                        /*if (isRefunded == posTemp.myOrderDet.Count)
                        {
                            MessageBox.Show("All Item in the order " + posTemp.myOrderHdr.Userfld5 + " already Refunded");
                            return false;
                        }*/
                        if (posTemp.myOrderHdr.MembershipNo != "WALK-IN")
                        {
                            posTemp.AssignMembership(posTemp.myOrderHdr.MembershipNo, out status);
                        }
                        posTemp.DeleteAllReceiptLinePayment();
                        posTemp.myOrderHdr.IsPointAllocated = false; // Make sure IsPointAllocated is false
                        pos = posTemp;

                        BindGrid();
                        if (pos.GetMemberInfo() != null)
                            DisplayMembershipToScreen(pos.GetMemberInfo());
                        txtBarcode.Focus();
                        txtBarcode.Text = "";
                        return true;
                    }

                }
                else return false;
            }
            else
            {
                DataSet ds = SyncClientController.GetOrderForRefund(txtBarcode.Text, PointOfSaleInfo.PointOfSaleID, refundSameOutlet, out status);
                if (ds != null && ds.Tables.Count > 0)
                {

                    POSController posTemp = new POSController();
                    posTemp.myOrderHdr.Load(ds.Tables[0]);
                    posTemp.myOrderDet.Load(ds.Tables[1]);
                    //obj = new JavaScriptSerializer().Deserialize<InventoryController>(result);
                    bool isRefund = false;
                    if (posTemp != null && posTemp.myOrderHdr != null && posTemp.myOrderHdr.OrderHdrID != "")
                    {
                        /*if (isRefund)
                        {
                            MessageBox.Show("Orders contain refund item.");
                            return false;
                        }*/

                        if (status != "" && status.StartsWith("Warning."))
                        {
                            MessageBox.Show(status.Replace("Warning.", ""));
                        }
                        if (posTemp.myOrderHdr.MembershipNo != "WALK-IN")
                        {
                            posTemp.AssignMembership(posTemp.myOrderHdr.MembershipNo, out status);
                        }
                        posTemp.DeleteAllReceiptLinePayment();
                        posTemp.myOrderHdr.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                        posTemp.myOrderHdr.IsPointAllocated = false; // Make sure IsPointAllocated is false
                        pos = posTemp;

                        BindGrid();
                        if (pos.GetMemberInfo() != null)
                            DisplayMembershipToScreen(pos.GetMemberInfo());
                        txtBarcode.Focus();
                        txtBarcode.Text = "";
                        return true;
                    }
                }
                else
                {
                    MessageBox.Show(status);
                }

            }


            return false;
        }

        private bool CheckBarcodeFromItemGroup()
        {
            //
            string status;
            string barcode = "";
            barcode = txtBarcode.Text;
            int qtyBar = 1;
            if (barcode.IndexOf('*') > 0)
            {
                string[] barcodeSplit = barcode.Split('*');
                string qty = barcodeSplit[0].ToString();
                string realBarcode = barcodeSplit[1].ToString();
                if (!Int32.TryParse(qty, out qtyBar))
                {
                    MessageBox.Show("Error: Please input correct Quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Focus();
                    return false;
                }

                barcode = realBarcode;
            }

            ItemGroup it = new ItemGroup(ItemGroup.Columns.Barcode, barcode);

            if (it.IsLoaded && !it.IsNew && it.Deleted.HasValue && !it.Deleted.Value)
            {
                //Load the item group map....
                ItemGroupMapCollection itG = new ItemGroupMapCollection();
                itG.Where(ItemGroupMap.Columns.ItemGroupID, it.ItemGroupId);
                itG.Where(ItemGroupMap.Columns.Deleted, false);
                itG.Load();

                //loop through and add the item
                for (int i = 0; i < itG.Count; i++)
                {
                    if (!pos.IsRestricted(new Item(itG[i].ItemNo), out status))
                    {
                        frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                        frmError.lblMessage = status;
                        frmError.ShowDialog();
                        //txtBarcode.Focus();
                        //return false;
                    }
                    else
                    {
                        if (!pos.AddItemToOrderWithPriceMode(
                            new ViewItem(ViewItem.Columns.ItemNo, itG[i].ItemNo),
                            itG[i].UnitQty * qtyBar, pos.GetPreferredDiscount(),
                            true,this.PriceMode, out status))
                        {
                            MessageBox.Show("Error adding item to order: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            return false;
                        }
                    }
                }
                BindGrid();
                txtBarcode.Focus();
                txtBarcode.Text = "";
                return true;
            }
            return false;
        }

        public void SetFocusBarcode()
        {
            txtBarcode.Focus();
        }

        private void LoadReceiptFromBarcode()
        {
            EditBillForms.frmViewBillDetail f = new WinPowerPOS.EditBillForms.frmViewBillDetail();
            string orderHdrID = txtBarcode.Text.Substring(2);
            f.OrderHdrID = orderHdrID; //fetch from the database - latest from this location
            f.ShowDialog();
            return;
        }
        /*
        private void UseVoucher()
        {
            //pop up the membership option
            frmVoucher frm = new frmVoucher();
            frm.pos = pos;
            frm.ShowDialog();
            frm.Dispose();
            if (frm.IsSuccessful)
                BindGrid();
            txtBarcode.Text = "";
        }*/
        private bool AddItemToOrder()
        {
            //Adi 20131122 Load Settings
            //Logger.writeLog("Promo Log - 7. Start Add Item To Order");
            bool useSpecialBarcode = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcode), false);
            string checkstring = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.BarcodeCheckValue).ToString();
            if (useSpecialBarcode && checkstring == "")
            {
                return false;
            }
            //find item using the barcode
            ViewItem theItem = null;
            string itemName = "";
            string tmpBarcode = txtBarcode.Text;

            #region *) OBSOLETE : special barcode checking has been moved down the order
            //if (useSpecialBarcode)
            //{
            //    int result = -1;
            //    decimal price = 0;
            //    decimal quantity = 0;
            //    string status = "";
            //    string recordedDigit = "";
            //    result = SpecialBarcodeController.ScanBarcode(tmpBarcode, out theItem, out price, out quantity, out recordedDigit, out status);

            //    if (result == -1)
            //    {
            //        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        txtBarcode.Focus();
            //        return false;
            //    }
            //    else if (result == 0)
            //    {
            //        if (!pos.IsRestricted(theItem, out status))
            //        {
            //            frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
            //            frmError.lblMessage = status;
            //            frmError.ShowDialog();
            //        }
            //        else
            //        {
            //            if (!pos.AddSpecialItemToOrder(theItem, quantity, price, PreferedDiscount, ApplyPromo, "", recordedDigit, out status))
            //            {
            //                MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                txtBarcode.Focus();
            //                return false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //find item using the barcode
            //        bool IsItemNo = false;
            //        theItem = itemLogic.FetchByBarcode(txtBarcode.Text, out IsItemNo);
            //        itemName = "";

            //        if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
            //        {
            //            if (!pos.IsRestricted(theItem, out status))
            //            {
            //                frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
            //                frmError.lblMessage = status;
            //                frmError.ShowDialog();
            //                //txtBarcode.Focus();
            //                //return false;
            //            }
            //            else
            //            {
            //                if (!pos.AddItemToOrder(theItem, 1, PreferedDiscount, ApplyPromo, out status))
            //                {
            //                    //alert error message.                    
            //                    MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                    txtBarcode.Focus();
            //                    return false;
            //                }

            //                else
            //                {
            //                    if (status != "")
            //                    {
            //                        MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                        txtBarcode.Focus();
            //                        return true;
            //                    }
            //                    itemName = theItem.ItemName;
            //                    //pos.ApplyMembershipDiscount();

            //                    // LowQuantity Feature
            //                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
            //                    {
            //                        if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
            //                        {
            //                            MessageBox.Show(theItem.ItemName + " item is only left with " + ItemController.GetStockOnHand(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            AlternateBarcodeCollection altbar = new AlternateBarcodeCollection();
            //            altbar.Where(AlternateBarcode.Columns.Barcode, txtBarcode.Text);
            //            altbar.OrderByDesc(AlternateBarcode.Columns.CreatedOn);
            //            altbar.Load();

            //            if (altbar.Count > 0 && altbar[0].IsLoaded)
            //            {
            //                if (!pos.IsRestricted(new Item(altbar[0].ItemNo), out status))
            //                {
            //                    frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
            //                    frmError.lblMessage = status;
            //                    frmError.ShowDialog();
            //                    //txtBarcode.Focus();
            //                    //return false;
            //                }
            //                else
            //                {

            //                    if (!pos.AddItemToOrder(new Item(altbar[0].ItemNo),
            //                        1, PreferedDiscount,
            //                        true, out status))
            //                    {
            //                        //alert error message.                    
            //                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                        txtBarcode.Text = "";
            //                        txtBarcode.Focus();
            //                        return false;
            //                    }
            //                    else
            //                    {
            //                        var item = new Item(altbar[0].ItemNo);
            //                        itemName = new Item(altbar[0].ItemNo).ItemName;
            //                        if (status != "")
            //                        {
            //                            MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                        }

            //                        // LowQuantity Feature
            //                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
            //                        {
            //                            if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
            //                            {
            //                                MessageBox.Show(item.ItemName + " is only left with " + ItemController.GetStockOnHand(item.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (!CheckVoucherFromScannedBarcode())
            //                {
            //                    frmErrorMessage frm = new frmErrorMessage();
            //                    frm.lblMessage = "Error: Barcode " + tmpBarcode + " does not exist";
            //                    frm.ShowDialog();
            //                    //MessageBox.Show(, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            #endregion

            //find item using the barcode
            //Logger.writeLog("Promo Log - 8. Fetch Item by Barcode");
            #region SPLIT BARCODE AND QUANTITY
            string barcode = "";
            barcode = txtBarcode.Text;
            int qtyBar = 1;
            if (barcode.IndexOf('*') > 0)
            {
                string[] barcodeSplit = barcode.Split('*');
                string qty = barcodeSplit[0].ToString();
                string realBarcode = barcodeSplit[1].ToString();
                if (!Int32.TryParse(qty, out qtyBar))
                {
                    MessageBox.Show("Error: Please input correct Quantity","", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Focus();
                    return false;
                }

                barcode = realBarcode;
            }
            #endregion

            bool IsItemNo = false;
            theItem = itemLogic.FetchByBarcode(barcode, out IsItemNo);
            itemName = "";
            if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
            {

                if (!pos.IsRestricted(theItem, out status))
                {
                    frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                    frmError.lblMessage = status;
                    frmError.ShowDialog();
                    //txtBarcode.Focus();
                    //return false;
                }
                else
                {
                    //Logger.writeLog("Promo Log - 9. Add Item To Order");
                    if (!pos.AddItemToOrderWithPriceMode(theItem, qtyBar, PreferedDiscount, ApplyPromo, this.PriceMode, out status))
                    {
                        //Logger.writeLog("Promo Log - 10. Finish Add Item To Order");
                        //alert error message.                    
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        return false;
                    }
                    else
                    {
                        if (status != "")
                        {
                            MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtBarcode.Focus();
                            return true;
                        }
                        itemName = theItem.ItemName;
                        //pos.ApplyMembershipDiscount();

                        // LowQuantity Feature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                        {
                            if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                            {
                                MessageBox.Show(theItem.ItemName + " is only left with " + ItemController.GetStockOnHand(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                            }
                        }
                    }
                }
            }
            else
            {
                AlternateBarcodeCollection altbar = new AlternateBarcodeCollection();
                altbar.Where(AlternateBarcode.Columns.Barcode, barcode);
                altbar.OrderByDesc(AlternateBarcode.Columns.CreatedOn);
                altbar.Load();

                if (altbar.Count > 0 && altbar[0].IsLoaded)
                {

                    if (!pos.IsRestricted(new Item(altbar[0].ItemNo), out status))
                    {
                        frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                        frmError.lblMessage = status;
                        frmError.ShowDialog();
                        //txtBarcode.Focus();
                        //return false;
                    }
                    else
                    {
                        theItem = new ViewItem(ViewItem.Columns.ItemNo, altbar[0].ItemNo);
                        if (!pos.AddItemToOrderWithPriceMode(theItem,
                            qtyBar, PreferedDiscount,
                            ApplyPromo, this.PriceMode, out status))
                        {
                            //alert error message.                    
                            MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Text = "";
                            txtBarcode.Focus();
                            return false;
                        }
                        else
                        {
                            var item = new Item(altbar[0].ItemNo);
                            itemName = new Item(altbar[0].ItemNo).ItemName;
                            if (status != "")
                            {
                                MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            // LowQuantity Feature
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                            {
                                if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                                {
                                    MessageBox.Show(item.ItemName + " is only left with " + ItemController.GetStockOnHand(item.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                }
                            }
                        }
                        //pos.ApplyMembershipDiscount();
                    }

                }
                else
                {
                    if (CheckVoucherFromScannedBarcode())
                    {
                        // no need to anything, already handled in CheckVoucherFromScannedBarcode()
                    }
                    else
                    {
                        if (useSpecialBarcode)
                        {
                            int result = -1;
                            decimal price = 0;
                            decimal quantity = 0;
                            string status = "";
                            string recordedDigit = "";
                            result = SpecialBarcodeController.ScanBarcode(tmpBarcode, out theItem, out price, out quantity, out recordedDigit, out status);

                            if (result == -1)
                            {
                                MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.Focus();
                                return false;
                            }
                            else if (result == 0)
                            {
                                if (!pos.IsRestricted(theItem, out status))
                                {
                                    frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                                    frmError.lblMessage = status;
                                    frmError.ShowDialog();
                                }
                                else
                                {
                                    if (!pos.AddSpecialItemToOrderWithPriceMode(theItem, quantity, price, PreferedDiscount, ApplyPromo, "", recordedDigit, this.PriceMode, out status))
                                    {
                                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.Focus();
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                ShowBarcodeNotExist(tmpBarcode);                                
                            }
                        }
                        else
                        {
                            ShowBarcodeNotExist(tmpBarcode);
                        }
                    }
                }
            }

            //refresh the display
            //Logger.writeLog("Promo Log - 11. Bind Grid");
            BindGrid();
            //Logger.writeLog("Promo Log - 12. Finish Bind Grid");

            if (theItem != null)
            {
                //PriceDisplay                            
                myDisplay.ClearScreen();
                if (theItem.IsServiceItem.GetValueOrDefault(false) && theItem.IsInInventory)
                {
                    OrderDetCollection tmpCol = pos.FetchUnsavedOrderDet();
                    for (int i = tmpCol.Count - 1; i >= 0; i--)
                    {
                        OrderDet od = tmpCol[i];
                        if (od.ItemNo == theItem.ItemNo)
                        {
                            myDisplay.ShowItemPrice(
                                itemName,
                                (double)od.Amount,
                                (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                        }
                    }
                }
                else
                {
                    myDisplay.ShowItemPrice(
                        itemName,
                        (double)theItem.RetailPrice,
                        (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                }
            }
            txtBarcode.Text = "";
            return true;
        }

        private void ShowBarcodeNotExist(string tmpBarcode)
        {
            System.Media.SystemSounds.Asterisk.Play();
            frmErrorMessage frm = new frmErrorMessage();
            frm.lblMessage = "Error: Barcode " + tmpBarcode + " does not exist";
            frm.ShowDialog();
        }

        void pos_OpenPriceItemAdded(object sender, string orderDetID)
        {

            //Open Price item
            //if (theItem.IsServiceItem.GetValueOrDefault(false))
            //{
            //    //MessageBox.Show("Service item");
            //    string lineID = pos.GetLineIDOfItemNo(theItem.ItemNo);
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.DisableOpenPricePrompt), false))
                return; 
            if (!string.IsNullOrEmpty(orderDetID))
            {
                OrderForms.frmOrderLineKeypad t = new WinPowerPOS.OrderForms.frmOrderLineKeypad();
                t.pos = pos;
                //t.isNew = true;
                t.LineID = orderDetID;
                t.editField = frmOrderLineKeypad.EditedField.RetailPrice;
                t.ApplyPromo = ApplyPromo;

                CommonUILib.displayTransparent();
                t.ShowDialog();
                CommonUILib.hideTransparent();

                if (t.isCancel)
                {
                    //remove
                    OrderDet myDet1 = pos.GetLine(orderDetID, out status);
                    pos.RemoveLine(myDet1);
                }
                BindGrid();
                updateTotalAmount();
                if (!t.isCancel)
                {
                    OrderDet myDet = pos.GetLine(orderDetID, out status);
                    myDisplay.ShowItemPrice(myDet.Item.ItemName, (double)myDet.Amount, double.Parse(lblTotalAmount.Text), poleDisplayWidth);
                }
            }
            //}
        }

        void pos_OpenPriceItemAddedEditField(object sender, string orderDetID, bool IsUsingQuantityField)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.DisableOpenPricePrompt), false))
                return; 

            if (!string.IsNullOrEmpty(orderDetID))
            {
                OrderForms.frmOrderLineKeypad t = new WinPowerPOS.OrderForms.frmOrderLineKeypad();
                t.pos = pos;
                //t.isNew = true;
                t.LineID = orderDetID;
                if (IsUsingQuantityField)
                    t.editField = frmOrderLineKeypad.EditedField.Quantity;
                else
                    t.editField = frmOrderLineKeypad.EditedField.RetailPrice;
                t.ApplyPromo = ApplyPromo;

                CommonUILib.displayTransparent();
                t.ShowDialog();
                CommonUILib.hideTransparent();

                if (t.isCancel)
                {
                    //remove
                    OrderDet myDet1 = pos.GetLine(orderDetID, out status);
                    pos.RemoveLine(myDet1);
                }
                BindGrid();
                updateTotalAmount();
                if (!t.isCancel)
                {
                    OrderDet myDet = pos.GetLine(orderDetID, out status);
                    myDisplay.ShowItemPrice(myDet.Item.ItemName, (double)myDet.Amount, double.Parse(lblTotalAmount.Text), poleDisplayWidth);
                }
            }
            //}
        }

        void pos_AutoCaptureWeightItemAdded(object sender, string orderDetID)
        {
            if (!string.IsNullOrEmpty(orderDetID))
            {
                OrderDet myDet = pos.GetLine(orderDetID, out status);

                frmLoadWeighing frm = new frmLoadWeighing(myDet.Item.UOM);
                frm.ShowDialog();
                if (frm.IsSuccess)
                {
                    myDet.Quantity = frm.Weight;
                    pos.CalculateLineAmount(ref myDet);
                }
                else
                {
                    MessageBox.Show(frm.Status);
                }

                BindGrid();
                updateTotalAmount();
                if (frm.IsSuccess)
                {
                    myDisplay.ShowItemPrice(myDet.Item.ItemName, (double)myDet.Amount, double.Parse(lblTotalAmount.Text), poleDisplayWidth);
                }
            }
        }

        private bool CheckVoucherFromScannedBarcode()
        {
            Voucher v = new Voucher(Voucher.Columns.VoucherNo, txtBarcode.Text);
            if (v.IsLoaded && !v.IsNew && !v.Deleted)
            {
                //Prompt SELL or REDEEM
                frmVoucherDialog f = new frmVoucherDialog();
                f.ShowDialog();
                bool sellVoucher = f.SellVoucher;
                f.Dispose();

                if (sellVoucher)
                {
                    if (v.ExpiryDate.HasValue && v.ExpiryDate.Value < DateTime.Now)
                    {
                        MessageBox.Show("This voucher has already expired in " + v.ExpiryDate.Value.ToString("dd MMM yyyy"), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                        return true;
                    }
                    bool wantContinue = true;
                    if (v.VoucherStatusId == VoucherController.VOUCHER_REDEEMED)
                    {
                        DialogResult dr = MessageBox.Show("Warning. This voucher has already been redeemed before. Do you like to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            wantContinue = false;
                    }
                    else if (v.VoucherStatusId == VoucherController.VOUCHER_SOLD)
                    {
                        DialogResult dr = MessageBox.Show("Warning. This voucher has already been sold before. Do you like to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            wantContinue = false;
                    }
                    if (!wantContinue)
                    {
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                        return true;
                    }
                    if (pos.AddVoucherItemToOrder(v.Amount, PreferedDiscount, v, false, true, out status))
                    {
                        txtBarcode.Text = "";
                        BindGrid();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error:" + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                        return true;
                    }
                }
                else //REDEEMING VOUCHER
                {
                    if (v.ExpiryDate.HasValue && v.ExpiryDate.Value < DateTime.Now)
                    {
                        MessageBox.Show("Voucher has already expired on " + v.ExpiryDate.Value.ToString("dd MMM yyyy"));
                        txtBarcode.Focus();
                        return true;
                    }
                    bool wantContinue = true;
                    if (v.VoucherStatusId == VoucherController.VOUCHER_REDEEMED)
                    {
                        DialogResult dr = MessageBox.Show("Warning. This voucher has already been redeemed before. Do you like to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            wantContinue = false;
                    }
                    else if (v.VoucherStatusId == VoucherController.VOUCHER_PRINTED)
                    {
                        DialogResult dr = MessageBox.Show("Warning. This voucher has never been sold before. Do you like to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            wantContinue = false;
                    }
                    if (!wantContinue)
                    {
                        txtBarcode.Text = "";
                        txtBarcode.Focus();

                        return true;
                    }
                    if (pos.AddVoucherItemToOrder(v.Amount, PreferedDiscount, v, false, false, out status))
                    {
                        txtBarcode.Text = "";
                        BindGrid();
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error:" + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                        return true;
                    }
                }
            }

            txtBarcode.Text = "";
            txtBarcode.Focus();

            return false;
        }
        private bool MembershipSignup(bool AddNewItemLine)
        {
            /// Change
            try
            {
                //Check if the same item is already inside....
                if (pos.IsNewMembersRegistered())
                    throw new Exception("(error)Please confirm before registering a new member.\n" +
                        "Only one new registration allowed per receipt.");

                //pop up the membership option
                frmMembershipQuickAdd frm = new frmMembershipQuickAdd();
                frm.pos = this.pos;
                frm.applyPromo = ApplyPromo;
                frm.ShowDialog();
                if (frm.IsSuccessful)
                {
                    Membership member = pos.GetMemberInfo();
                    DisplayMembershipToScreen(member);
                    frm.Dispose();
                    BindGrid();
                    txtBarcode.Focus();
                    txtBarcode.Text = MembershipController.MEMBERSHIP_SIGNUP_BARCODE;
                    frm.Dispose();
                    if (AddNewItemLine)
                        AddItemToOrder();
                    return true;
                }
                frm.Dispose();
                return false;
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
        }
        private bool MembershipRenewal(bool AddNewItemLine)
        {


            //pop up the membership option
            frmRenewal frm = new frmRenewal();
            frm.pos = this.pos;

            frm.ShowDialog();
            if (frm.IsSuccessful)
            {
                Membership member = frm.pos.GetMemberInfo();

                this.pos.CurrentMember.MembershipGroupId = frm.selectedMembershipGroupID;
                var discount = new Select(MembershipGroup.Columns.Discount).From("MembershipGroup").Where(MembershipGroup.Columns.MembershipGroupId).IsEqualTo(pos.CurrentMember.MembershipGroupId).ExecuteScalar();
                try
                {
                    pos.applyDiscount(Convert.ToDecimal(discount));
                }
                catch { }

                //this.pos.CurrentMember.
                //pos.SetNewMembershipGroupID(frm.selectedMembershipGroupID);

                //this.pos.AssignMembership();

                pos.ApplyMembershipDiscount();



                frm.Dispose();
                txtBarcode.Text = MembershipController.RENEWAL_BARCODE;
                txtBarcode.Focus();
                if (AddNewItemLine)
                    AddItemToOrder();
                BindGrid();
                return true;
            }
            frm.Dispose();
            return false;
        }
        private bool CheckMembershipFromScannedBarcode()
        {
            //Check membership.....
            //if (txtBarcode.Text.StartsWith(MembershipController.MEMBERSHIP_PREFIX))
            //{

            //EXISTING MEMBERSHIP IN THE SYSTEM
            bool hasExpired;
            DateTime ExpiryDate;
            if (MembershipController.IsExistingMember(txtBarcode.Text, out hasExpired, out ExpiryDate))
            {
                return addMemberToPOS(txtBarcode.Text);
            }
            else
            {
                if (hasExpired)
                {
                    //prompt window to allow bypass?
                    DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ". Do you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        return addExpiredMemberToPOS(txtBarcode.Text);
                    }
                }
                else
                {
                    //Check NRIC
                    Membership member;
                    if (MembershipController.IsMembersNRIC(txtBarcode.Text, out member))
                    {

                        if (member.ExpiryDate.HasValue && member.ExpiryDate.Value.AddDays(1) >= DateTime.Now)
                        {
                            //havent expired
                            return addMemberToPOS(member.MembershipNo);
                        }
                        else if (member.ExpiryDate.HasValue)
                        {
                            //expired already
                            //prompt window to allow bypass?
                            DialogResult dr = MessageBox.Show("This member has already expired on " + member.ExpiryDate.Value.ToString("dd MMM yyyy") + ". Do you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                return addExpiredMemberToPOS(member.MembershipNo);
                            }
                        }
                    }
                    else if (MembershipController.IsMembersTagNo(txtBarcode.Text, out member))
                    {
                        if (member.ExpiryDate.HasValue && member.ExpiryDate.Value.AddDays(1) >= DateTime.Now)
                        {
                            //havent expired
                            return addMemberToPOS(member.MembershipNo);
                        }
                        else if (member.ExpiryDate.HasValue)
                        {
                            //expired already
                            //prompt window to allow bypass?
                            DialogResult dr = MessageBox.Show("This member has already expired on " + member.ExpiryDate.Value.ToString("dd MMM yyyy") + ". Do you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                return addExpiredMemberToPOS(member.MembershipNo);
                            }
                        }
                    }
                }
                //check format.... MC followed by 14 digit
                //if correct, apply the barcode to orderhdr
                /*
                 * For attributes,allow membership that follow the format MC20xxxx to be used at other counter
                 * This is because attributes has many counter and a member can signup at one counter and
                 * buy another item from another counter on the same day.
                 * 
                if (txtBarcode.Text.Length == 14 && txtBarcode.Text.Substring(0, 4).ToUpper() == "MC20")
                {
                    pos.AssignOtherMembership(txtBarcode.Text);
                    pos.ApplyMembershipDiscount();
                    BindGrid();
                    Membership member = pos.GetMemberInfo();
                    DisplayMembershipToScreen(member);
                    
                    //dgvPastTransaction.DataSource =
                    //    MembershipController.FetchLastPurchasedTransactions(member.MembershipNo);
                    //dgvPastTransaction.Refresh();
                    
                    myDisplay.ClearScreen();
                    myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status));
                    txtBarcode.Text = "";
                    txtBarcode.Focus();
                    return true;
                    
                }
                */
                return false;
            }
        }
        private bool addMemberToPOS(string membershipNo)
        {
            bool isChangeMember = pos.MembershipApplied();
            double oldDiscount = 0;
            string oldPriceLevel = "No Link";
            if (isChangeMember)
            {
                oldDiscount = pos.GetMemberInfo().MembershipGroup.Discount;
                oldPriceLevel = pos.GetMemberInfo().MembershipGroup.Userfld1;
            }

            if (pos.AssignMembership(membershipNo, out status))
            {
                if (isChangeMember)
                {
                    if ((oldDiscount != null && oldDiscount > 0) || (!String.IsNullOrEmpty(oldPriceLevel) && oldPriceLevel != "No Link"))
                    {
                        ResetDiscount();
                        pos.ApplyMembershipDiscount();
                    }
                    else
                    {
                        if ((pos.GetMemberInfo().MembershipGroup.Discount != null && pos.GetMemberInfo().MembershipGroup.Discount > 0) ||
                            (!String.IsNullOrEmpty(pos.GetMemberInfo().MembershipGroup.Userfld1) && pos.GetMemberInfo().MembershipGroup.Userfld1 != "No Link"))
                        {
                            //ResetDiscount();
                            pos.ApplyMembershipDiscount();
                            
                        }
                    }

                    if (pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "")
                        pos.ApplyCustomerPriceAll(pos.GetMemberInfo().MembershipNo);
                }
                else
                {
                    if ((pos.GetMemberInfo().MembershipGroup.Discount != null && pos.GetMemberInfo().MembershipGroup.Discount > 0) ||
                        (!String.IsNullOrEmpty(pos.GetMemberInfo().MembershipGroup.Userfld1) && pos.GetMemberInfo().MembershipGroup.Userfld1 != "No Link"))
                    {
                        //ResetDiscount();
                        pos.ApplyMembershipDiscount();                        
                    }

                    if (pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "")
                        pos.ApplyCustomerPriceAll(pos.GetMemberInfo().MembershipNo);
                }

                string pricelevel = pos.GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    frmDiscounts frd = new frmDiscounts();
                    try
                    {
                        pos.clearDiscount(0);
                        pos.applyDiscount(pricelevel);
                        btnDiscount.Text = sd.DiscountLabel;
                        //BindGrid();
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }

                BindGrid();
                Membership member = pos.GetMemberInfo();
                DisplayMembershipToScreen(member);
                //decimal balance = MembershipController.FetchRemainingInstallmentBalance(member.MembershipNo);
                decimal balance = Installment.GetOutstandingBalancePerMember(member.MembershipNo);
                if (balance > 0)
                {
                    MessageBox.Show(LanguageManager.Outstanding_Balance + " : " + balance.ToString("N2"));
                }
                /*
                dgvPastTransaction.DataSource =
                    MembershipController.FetchLastPurchasedTransactions(member.MembershipNo);
                dgvPastTransaction.Refresh();
                 * */
                myDisplay.ClearScreen();
                myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (!SyncPointsThread.IsBusy)
                    SyncPointsThread.RunWorkerAsync();
                return true;
            }
            return false;

        }
        private bool addExpiredMemberToPOS(string membershipNo)
        {

            if (pos.AssignExpiredMember(membershipNo, out status))
            {
                pos.ApplyMembershipDiscount();
                BindGrid();
                Membership member = pos.GetMemberInfo();
                DisplayMembershipToScreen(member);
                //decimal balance = MembershipController.FetchRemainingInstallmentBalance(member.MembershipNo);
                decimal balance = Installment.GetOutstandingBalancePerMember(member.MembershipNo);
                if (balance > 0)
                {
                    MessageBox.Show(LanguageManager.Outstanding_Balance + " : " + balance.ToString("N2"));
                }
                /*
                dgvPastTransaction.DataSource =
                    MembershipController.FetchLastPurchasedTransactions(member.MembershipNo);
                dgvPastTransaction.Refresh();
                 * */
                myDisplay.ClearScreen();
                myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                txtBarcode.Text = "";
                txtBarcode.Focus();
                if (!SyncPointsThread.IsBusy)
                    SyncPointsThread.RunWorkerAsync();
                return true;
            }
            return false;

        }
        private void DisplayMembershipToScreen(PowerPOS.Membership member)
        {
            lblMembershipNo.Text = member.MembershipNo;
            lblName.Text = member.NameToAppear;
            if (member.ExpiryDate != null)
                lblExpiryDate.Text = ((DateTime)member.ExpiryDate).ToString("dd MMM yyyy");
            lblMemberGroup.Text = member.MembershipGroup.GroupName;
            lblDiscount.Text = "Disc:" + member.MembershipGroup.Discount.ToString("N2") + "%";
            PreferedDiscount = pos.GetPreferredDiscount();
            if (member.DateOfBirth.HasValue)
                lblDOB.Text = member.DateOfBirth.Value.ToString("dd MMM yyyy");

            if (member.DateOfBirth.HasValue && member.DateOfBirth.Value.Month == DateTime.Now.Month)
            {
                MessageBox.Show("MEMBER DATE OF BIRTH IS THIS MONTH (" + member.DateOfBirth.Value.ToString("dd MMM yyyy") + ")");
            }

            // Birthday prompt for attached particulars
            AttachedParticularCollection apColl = new AttachedParticularCollection();
            apColl.Where("MembershipNo", member.MembershipNo);
            apColl.Where("Deleted", false);
            apColl.Load();
            foreach (AttachedParticular ap in apColl)
            {
                if (ap.DateOfBirth.HasValue && ap.DateOfBirth.Value.Month == DateTime.Now.Month)
                {
                    MessageBox.Show(string.Format("MEMBER'S ATTACHED PARTICULAR ({0}) DATE OF BIRTH IS THIS MONTH ({1})", (ap.FirstName + " " + ap.LastName).Trim(), ap.DateOfBirth.Value.ToString("dd MMM yyyy")));
                }
            }

            //validation warning
            List<string> EmptyMandatoryFields;
            if (!pos.IsNewMembersRegistered() && !pos.ValidateMembership(member, out EmptyMandatoryFields))
            {
                string separator = System.Environment.NewLine + "- ";
                MessageBox.Show("You have to fill in these fields : " + separator + string.Join(separator, EmptyMandatoryFields.ToArray()), "Compulsory informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                frmNewMembershipEdit frm = new frmNewMembershipEdit(member);
                //frm.membershipNo = member.MembershipNo;
                frm.btnSave.Visible = true;
                frm.ShowDialog();
                pos.AssignMembership(member.MembershipNo, out status);
                //DisplayMembershipToScreen(pos.GetMemberInfo());
            }
            //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowMembershipWarning), false))
            //{
            //    string EmptyMandatoryFields = "";
            //    string fields = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipWarningFields);
            //    if (fields != null && fields != "")
            //    {
            //        string[] result = fields.Split(';');
            //        foreach (string s in result)
            //        {
            //            if (member.GetColumnValue(s) == null || member.GetColumnValue(s).ToString() == "")
            //            {
            //                EmptyMandatoryFields += System.Environment.NewLine + "- " + s;
            //            }
            //        }

            //        if (EmptyMandatoryFields.Length > 0)
            //        {
            //            MessageBox.Show("You have to fill these fields : " + EmptyMandatoryFields, "Compulsory informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            frmNewMembershipEdit frm = new frmNewMembershipEdit();
            //            frm.membershipNo = member.MembershipNo;
            //            frm.btnSave.Visible = true;
            //            frm.ShowDialog();
            //            pos.AssignMembership(member.MembershipNo, out status);
            //            DisplayMembershipToScreen(pos.GetMemberInfo());
            //        }
            //    }
            //}
        }
        #endregion

        #region "Confirm, Cancel & updating total amount"

        public bool AllowtoClearOrder()
        {
            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.PromptPasswordClearOrder), false))
            {
                return true;
            }
            if (!PrivilegesController.HasPrivilege(PrivilegesController.CLEAR_ORDER, UserInfo.privileges))
            {
                frmSupervisorLogin f = new frmSupervisorLogin();
                f.privilegeName = PrivilegesController.CLEAR_ORDER;
                f.ShowDialog();
                if (!f.IsAuthorized)
                {
                    f.Dispose();
                    return false;
                }
                else
                {
                    f.Dispose();
                }
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to cancel order?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                if (!AllowtoClearOrder())
                {
                    MessageBox.Show("You are not allowed to clear order. Please check with your supervisor.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                myDisplay.ClearScreen();
                AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, PowerPOS.AccessSource.POS, UserInfo.username, "", "Clear Order", pos.FetchUnsavedOrderDetText());
                
                clearControls();
                if (fOrderSecondScreen != null && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                {
                    fOrderSecondScreen.ClearControls();
                }

            }
            txtBarcode.Focus();
        }
        private void clearControls()
        {
            try
            {
                //dgvPurchase.Rows.Clear();
                while (dgvPurchase.Rows.Count > 0)
                {
                    dgvPurchase.Rows.RemoveAt(0);
                }
                dgvPurchase.Refresh();
                pos = new POSController();

                if (mode == "formal")
                {
                    pos.mode = "formal";
                }

                //If GST has been set on XML file, use the outlet GST rule instead
                if (GSTOverride.GSTRule != 0)
                    pos.SetHeaderGSTRule(GSTOverride.GSTRule);

                if (mode == "formal")
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.UseSpecialGSTRuleForFormal), false))
                        pos.SetHeaderGSTRule(int.Parse(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.SpecialGSTRule)));
                }

                pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
                pos.OpenPriceItemAddedEditField += new OpenPriceItemEditFieldHandler(pos_OpenPriceItemAddedEditField);
                pos.AutoCaptureWeightItemAdded += new AutoCaptureWeightItemHandler(pos_AutoCaptureWeightItemAdded);

                string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
                if (useCustomInvoiceNo != null && useCustomInvoiceNo.ToLower() == "true")
                {
                    this.lblRefNo.Text = pos.GetUnsavedCustomRefNo();
                }
                else
                {
                    string temp = pos.GetUnsavedRefNo();
                    this.lblRefNo.Text = temp.Substring(temp.Length - 3);
                }
                /*string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
                if (useCustomInvoiceNo.ToLower() == "true")
                {
                    this.lblRefNo.Text = pos.GetUnsavedCustomRefNo();
                }
                else
                {
                    string temp = pos.GetUnsavedCustomRefNo();
                    this.lblRefNo.Text = temp.Substring(temp.Length -3);
                }*/
                lblTotalAmount.Text = "0";
                lblTotalDiscount.Text = "0";
                lblGrossAmount.Text = "0";
                PreferedDiscount = 0;
                lblGST.Text = "0";
                //lblPreferredDisc.Text = "0";
                //btnDiscount.Text = "Discount 0%";

                //lblNumOfItems.Text = "0";
                rbPromoEnable.Checked = true;
                // Clear Membership info
                lblMemberGroup.Text = "-";
                lblMembershipNo.Text = "-";

                lblName.Text = "-";
                lblExpiryDate.Text = "-";
                lblDiscount.Text = "-";
                lblRemark.Text = "-";
                lblLineInfo.Text = "-";
                lblDOB.Text = "-";
                /*Clear History Grid
                for (int j = dgvPastTransaction.Rows.Count-1; j >= 0; j--)
                {
                    dgvPastTransaction.Rows.RemoveAt(j);
                }*/
                myDisplay.ClearScreen();
                myDisplay.SendCommandToDisplay(poleDisplayMessage);

                //Adi 20170710 - Set to not scanned for user token discount
                isUserTokenForDiscountScanned = false;

                 btnPriceMode.Text = "NORMAL";
                 btnPriceMode.Tag = "NORMAL";

                #region *) Clear Second Screen
                if (fOrderSecondScreen != null)
                {
                    fOrderSecondScreen.pos = pos;
                    fOrderSecondScreen.UpdateView();
                }

                if (frmPrintReceipt != null)
                {
                    frmPrintReceipt.Hide();
                }
                #endregion
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearControlsMembership()
        {
            try
            {
                rbPromoEnable.Checked = true;
                // Clear Membership info
                lblMemberGroup.Text = "-";
                lblMembershipNo.Text = "-";

                lblName.Text = "-";
                lblExpiryDate.Text = "-";
                lblDiscount.Text = "-";
                lblRemark.Text = "-";
                lblLineInfo.Text = "-";
                lblDOB.Text = "-";

                myDisplay.ClearScreen();
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void updateTotalAmount()
        {
            try
            {
                decimal disc = pos.GetPreferredDiscount();
                //if (disc == 0)
                //{
                //    btnDiscount.Text = "Discount 0%";
                //}
                //else
                //{
                //    btnDiscount.Text = "Discount " + ((disc % 1 > 0) ? disc.ToString("N2") : disc.ToString("N0")) + "%";
                //}
                //Calculate total discount
                decimal GrossAmount = pos.CalculateGrossAmount();
                GrossAmount = pos.CalculateGrossAmountForDisplay();
                decimal TotalAmount = pos.CalculateTotalAmount(out status);
                decimal GSTAmount = pos.calculateTotalGST();

                lblTotalAmount.Text = TotalAmount.ToString("N");
                lblGrossAmount.Text = GrossAmount.ToString("N");
                decimal totalDiscount = pos.CalculateTotalDiscount();//(GrossAmount - TotalAmount);
                pos.SetTotalDiscount(totalDiscount);
                lblTotalDiscount.Text = totalDiscount.ToString("N");
                lblGST.Text = GSTAmount.ToString("N");

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowTotalQuantityInSalesScreen), false))
                {
                    decimal qty = pos.calculateTotalQuantity();
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false))
                        lblTotalQuantity.Text = qty.ToString("N3");
                    else
                        lblTotalQuantity.Text = qty.ToString("N");

                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowTotalItemInSalesScreen), false))
                {
                    lblTotalItems.Text = pos.calculateTotalItems().ToString();

                }

                if (status != "")
                {
                    MessageBox.Show("Error while calculating total amount: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string campaignName, campaignType;
                pos.GetPromotionInfo(out campaignName, out campaignType);
                if (!pos.MembershipApplied())
                {
                    ClearMembership();
                }
                pnlShowChange.Visible = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                {
                    fOrderSecondScreen.UpdateChange("0");
                }
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ResetPriceModeAfterScanItem), false))
                {
                    //Reset Price Mode
                    btnPriceMode.Text = "NORMAL";
                    btnPriceMode.Tag = "NORMAL";
                }

                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tryDownloadPoints(string membershipno, string orderHdrID)
        {
            tryDownloadPoints(membershipno, orderHdrID, false);
        }

        private void tryDownloadPoints(string membershipno, string orderHdrID, bool reprint)
        {
            bool overwriteSetting = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), true);
            if ((Features.Package.isAvailable && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WaitToDownloadPointsBeforePrintReceipt), false)) || overwriteSetting)
            {
                //Download 
                //this.Enabled = false;
                string isRealTime = ConfigurationManager.AppSettings["RealTimePointSystem"];
                if (isRealTime == "yes" || isRealTime == "true")
                {
                    if (membershipno != "WALK-IN")
                    {
                        frmDownloadPoints fDownloadPoints = new frmDownloadPoints();
                        fDownloadPoints.membershipNo = membershipno;
                        fDownloadPoints.orderHdrID = orderHdrID;
                        fDownloadPoints.ParentSyncPointsThread = reprint ? null : SyncPointsThread;
                        fDownloadPoints.ShowDialog();
                        if (!fDownloadPoints.IsSuccessful)
                            MessageBox.Show("Latest Point Data is not downloaded yet. Showing the latest point data in the receipt.");

                        /*string sqlQuery = "Select max(modifiedon) from membershipPoints where membershipno = '" + membershipno + "'";
                        if (pos.MembershipApplied())
                        {
                            object maxModifiedOn = DataService.ExecuteScalar(new QueryCommand(sqlQuery));
                            DateTime testMo = DateTime.Now;
                            if (DateTime.TryParse(maxModifiedOn.ToString(), out testMo))
                            {
                                Logger.writeLog("Initial Points. " + testMo.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            }
                        }*/
                    }
                }

            }

        }

        private void HandleNegativeAmount()
        {
            if (!IsLoadedFromAppointment)
            {
                string assignedMemberStaff = pos.GetAssignedMembershipStaff();
                UserMst memberUser = new UserMst();

                if (string.IsNullOrEmpty(assignedMemberStaff))
                {
                    memberUser = new UserMst(assignedMemberStaff);
                }


                if (PointOfSaleInfo.promptSalesPerson)
                {
                    frmSalesPerson f = new frmSalesPerson();
                    if (!memberUser.IsNew)
                    {
                        f.AssignedStaff = memberUser.UserName;
                    }

                    f.ShowDialog();
                    if (!f.IsSuccessful)
                    {
                        f.Dispose();
                        return;
                    }
                    f.Dispose();
                }
                else
                {
                    SalesPersonInfo.SalesPersonID = UserInfo.username;
                    SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                    SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
                    
                }
            }
            else
            {
                SalesPersonInfo.SalesPersonID = salesPersonFromAppt.UserName;
                SalesPersonInfo.SalesPersonName = salesPersonFromAppt.DisplayName;
                SalesPersonInfo.SalesGroupID = salesPersonFromAppt.SalesPersonGroupID;
            }

            #region *) DELIVERY: Choose Print Template
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.IsUsingDeliveryOrder), false))
            {
                frmSelectPrintTemplate FfrmSelectPrintTemplate = new frmSelectPrintTemplate(pos);
                DialogResult DR = FfrmSelectPrintTemplate.ShowDialog();

                FfrmSelectPrintTemplate.Dispose();
                if (DR == DialogResult.Cancel) return;
            }
            #endregion

            bool isOutletSales = false;

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
            {

                if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                {
                    //validation if outlet order
                    PointOfSaleCollection posColl = new PointOfSaleCollection();
                    posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                    posColl.Load();
                    if (posColl.Count > 0)
                        isOutletSales = true;

                }
            }

            #region *) ORDER TYPE: Select Order Type (ex.: Cash&Carry or Preorder)
            DeliveryOrderCollection delOrderHdrColl = new DeliveryOrderCollection();
            DeliveryOrderDetailCollection delOrderDetColl = new DeliveryOrderDetailCollection();

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false) && !isOutletSales)
            {
                frmSelectOrderType f = new frmSelectOrderType();
                DialogResult result = f.ShowDialog();
                string orderType = f.OrderType;
                f.Dispose();

                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    pos.myOrderHdr.OrderType = orderType.ToUpper();
                    // NOTE: If Pre-Order is selected, the Delivery Setup form will be opened after Payment completion
                }
            }
            #endregion

            pos.SetTotalReceiptAmount(0);
            decimal change;

            pos.AddReceiptLinePayment(0, POSController.PAY_CASH, "", 0, "", 0, out change, out status);

            bool IsQtyInsufficient = false;
            if (!pos.IsQtySufficientToDoStockOut(out status))
            {

                DialogResult dr = MessageBox.Show
                    ("Error: " + status + ". Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.No)
                {
                    pos.DeleteAllReceiptLinePayment();
                    return;
                }
                IsQtyInsufficient = true;
            }

            #region *) Signature
            if (pos.IsPackageRedeemTransaction())
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailable), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                }
            }
            #endregion

            bool IsPointAllocationSuccess = false;
            string tempOrderHdrID = pos.myOrderHdr.OrderHdrID;

            if (pos.ConfirmOrder(false, out IsPointAllocationSuccess, out status))
            {
                if (UseCustomInvoiceNo)
                {
                    CustNumUpdate(); //Graham
                }

                #region *) Open Delivery Setup form if PREORDER
                //bool isReturnItemsAndLineDiscount = IsReturnItemsAndLineDiscount(pos);
                if (pos.myOrderHdr.OrderType != null && pos.myOrderHdr.OrderType.ToUpper() == "PREORDER")
                {
                    // Skip if all quantity is negative (contains return items only)
                    if (!pos.IsReturnedItemsOnly())
                    {
                        bool skipDeliverySetup = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.SkipDeliverySetupScreen), false);
                        if (!skipDeliverySetup)
                        {
                            //pos.SetAsDelivery(true);
                            frmDeliverySetup fDelivery = new frmDeliverySetup();
                            fDelivery.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                            fDelivery.IsUnsavedOrder = true;
                            fDelivery.pos = pos;
                            if (pos.MembershipApplied())
                            {
                                fDelivery.membershipNo = pos.GetMemberInfo().MembershipNo;
                            }
                            fDelivery.ShowDialog();
                            delOrderHdrColl = fDelivery.delOrderHdrColl;
                            delOrderDetColl = fDelivery.delOrderDetColl;
                            fDelivery.Dispose();
                        }
                        else
                        {
                            // Skip Delivery Setup screen and auto assign deposit amount
                            #region *) Assign Deposit Amount to OrderDet
                            decimal depositBalance = POSController.GetDepositBalance(pos.myOrderHdr.OrderHdrID, out status);

                            QueryCommandCollection cmdColl = new QueryCommandCollection();
                            OrderDetCollection refOD = pos.myOrderDet;
                            decimal remainingAmt = depositBalance;
                            foreach (OrderDet tmpOD in refOD)
                            {
                                OrderDet od = new OrderDet(tmpOD.OrderDetID);
                                if (od.IsVoided) continue;
                                if (od.Amount == null) od.Amount = 0;
                                if (od.DepositAmount == null) od.DepositAmount = 0;

                                if (od.Amount > 0 && od.Amount > od.DepositAmount)
                                {
                                    decimal discrepancy = od.Amount - od.DepositAmount;
                                    if (discrepancy <= remainingAmt)
                                    {
                                        od.DepositAmount += discrepancy;
                                        remainingAmt -= discrepancy;
                                    }
                                    else
                                    {
                                        od.DepositAmount += remainingAmt;
                                        remainingAmt = 0;
                                    }

                                    cmdColl.Add(od.GetUpdateCommand(UserInfo.username));
                                    if (remainingAmt <= 0) break;
                                }
                            }

                            DataService.ExecuteTransaction(cmdColl);
                            #endregion
                        }
                    }
                    else
                    {
                        // Assign and Update OrderDet.DepositAmount automatically
                        QueryCommandCollection cmdColl = new QueryCommandCollection();
                        foreach (OrderDet od in pos.myOrderDet)
                        {
                            OrderDet tmpOD = new OrderDet(od.OrderDetID);
                            tmpOD.DepositAmount = od.Amount;
                            QueryCommand cmd = tmpOD.GetUpdateCommand(UserInfo.username);
                            cmdColl.Add(cmd);
                        }
                        DataService.ExecuteTransaction(cmdColl);
                    }
                }
                #endregion

                #region *) Save Delivery Information (if any)
                if (delOrderHdrColl != null && delOrderDetColl != null & delOrderHdrColl.Count > 0 && delOrderDetColl.Count > 0)
                {
                    foreach (DeliveryOrder doHdr in delOrderHdrColl)
                    {
                        doHdr.SalesOrderRefNo = pos.myOrderHdr.OrderHdrID;
                    }

                    DeliveryOrderController.SaveMultipleOrder(delOrderHdrColl, delOrderDetColl);

                    // Update OrderDet.DepositAmount
                    QueryCommandCollection cmdColl = new QueryCommandCollection();
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        OrderDet tmpOD = new OrderDet(od.OrderDetID);
                        tmpOD.DepositAmount = od.DepositAmount;
                        QueryCommand cmd = tmpOD.GetUpdateCommand(UserInfo.username);
                        cmdColl.Add(cmd);
                    }
                    DataService.ExecuteTransaction(cmdColl);
                    foreach (DeliveryOrder doHdr in delOrderHdrColl)
                    {
                        POSDevices.Receipt rcpt = new POSDevices.Receipt();
                        rcpt.PrintDeliveryOrder(doHdr, delOrderDetColl, pos);
                    }
                }
                #endregion

                #region *) Cancel Delivery if the item is returned
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoCancelDeliveryItemIfReturned), false))
                {
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        if (!string.IsNullOrEmpty(od.ReturnedReceiptNo))
                        {
                            // Cancel DeliveryOrderDetails related to this returned item
                            DeliveryOrderController.CancelDeliveryOfReturnedItem(od.ReturnedReceiptNo, od.ItemNo);
                        }
                    }
                }
                #endregion

                if (!SyncSalesThread.IsBusy)
                    SyncSalesThread.RunWorkerAsync();

                if (mode.ToLower() == "formal")
                {
                    frmChoosePrint fchoosePrint = new frmChoosePrint();
                    fchoosePrint.ShowDialog();
                    if (fchoosePrint.IsSuccessful)
                    {
                        if (PrintSettingInfo.receiptSetting.PrintReceipt)
                            tryDownloadPoints(pos.GetMemberInfo().MembershipNo, pos.myOrderHdr.OrderHdrID);

                        if (pos.myOrderDet.Count == 1 && pos.myOrderDet[0].ItemNo == "MEMBER")
                        {
                        }
                        else
                        {
                            POSDeviceController.PrintAHAVATransactionReceipt(pos, change, false,
                                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        }
                    }
                }
                else
                {
                    if (PrintSettingInfo.receiptSetting.PrintReceipt)
                        tryDownloadPoints(pos.GetMemberInfo().MembershipNo, pos.myOrderHdr.OrderHdrID);

                    if (pos.myOrderDet.Count == 1 && pos.myOrderDet[0].ItemNo == "MEMBER")
                    {
                    }
                    else
                    {
                        POSDeviceController.PrintAHAVATransactionReceipt(pos, change, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                    }
                }

                //if (!IsPointAllocationSuccess)
                //{ MessageBox.Show("Point is not updated!" + Environment.NewLine + status, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false))
                {
                    MallIntegrationController mc = new MallIntegrationController(tempOrderHdrID);
                    if (!mc.GenerateFile(out status))
                    {
                        MessageBox.Show("Generate Integration File was not successful. Please recreate the file.");

                    }
                }

                if (!IsQtyInsufficient)
                    if (!pos.ExecuteStockOut(out status))
                    {
                        MessageBox.Show
                            ("Error while performing Stock Out: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                if (IsLoadedFromAppointment)
                {
                    isSuccessful = true;
                    OrderHdrID = pos.myOrderHdr.OrderHdrID;
                    this.Close();
                }

                clearControls();
            }
            else
            {
                MessageBox.Show("Error:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "DataGridView - Editing of qty in the cell"
        private void dgvPurchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string IDTemp = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                OrderDet myDet1 = pos.GetLine(IDTemp, out status);
                bool enableGrouping = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.EnableGrouping), false);
                if (enableGrouping)
                {
                    if (myDet1.IsPromo)
                    {
                        //edit promo, only allow to edit quantity
                        if (e.ColumnIndex == QTY_COL || e.ColumnIndex == RPRICE_COL || e.ColumnIndex == DISCPRICE_COL)
                        {
                            frmEditPromoItem fEditPromoItem = new frmEditPromoItem();
                            fEditPromoItem.pos = pos;
                            fEditPromoItem.PromoHdrID = myDet1.PromoHdrID.GetValueOrDefault(0);
                            fEditPromoItem.ShowDialog();
                            if (fEditPromoItem.isSuccessful)
                            {
                                pos.UndoPromo();
                                pos.ApplyPromo();
                                BindGrid();
                            }
                        }
                        else if (e.ColumnIndex == SPCLDISCPRICE_COL || e.ColumnIndex == DISCOUNTDETAIL_COL || e.ColumnIndex == DISC_COL)
                        {

                            string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            if (!GiveDiscountAllowed)
                            {
                                //Adi 
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.UseUserTokenToGiveDiscount), false))
                                {
                                    //use user token to give discount, if haven't 
                                    if (!isUserTokenForDiscountScanned)
                                    {
                                        DialogResult result = DialogResult.Cancel;
                                        //membershipNo = "";
                                        try
                                        {
                                            frmScanStaffCard fScanStaffCard = new frmScanStaffCard();
                                            fScanStaffCard.title = "Scan User Token";
                                            fScanStaffCard.txtMessage = "Please Scan Staff Token for discount";
                                            //fScanStaffCard.Mode = mode;
                                            result = fScanStaffCard.ShowDialog();
                                            if (result != DialogResult.OK)
                                            {
                                                return;
                                            }
                                            //string role, status, DeptID;
                                            UserMst user;
                                            user = new UserMst(fScanStaffCard.membershipNo);

                                            if (user != null && user.UserName == fScanStaffCard.membershipNo)
                                            {
                                                UserGroup grp = new UserGroup(user.GroupName);
                                                if (PrivilegesController.HasPrivilege(PrivilegesController.GIVE_DISCOUNT, UserController.FetchGroupPrivilegesWithUsername(grp.GroupName, user.UserName)))
                                                {
                                                    //mySupervisor = fScanStaffCard.membershipNo;
                                                    //IsAuthorized = true;
                                                    isUserTokenForDiscountScanned = true;
                                                    //this.Close();

                                                }
                                                else
                                                {
                                                    MessageBox.Show(fScanStaffCard.membershipNo + " has insufficient privileges.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.writeLog(ex);
                                        }
                                    }
                                }
                                else
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
                                    }
                                }
                            }
                            bool isMultiTier = false;
                            if (e.ColumnIndex == SPCLDISCPRICE_COL)
                                isMultiTier = EnableMultiTierPrice;

                            if (e.ColumnIndex == DISCOUNTDETAIL_COL || e.ColumnIndex == DISC_COL)
                            {
                                string ItemNo = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                                Item it = new Item(ItemNo);
                                if (it.IsNonDiscountable)
                                {
                                    //CommonUILib.hideTransparent();
                                    return;
                                }
                            }
                            CommonUILib.displayTransparent();
                            frmDiscountSpecial frm = new frmDiscountSpecial(pos, ID, false, isMultiTier);
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog();
                            CommonUILib.hideTransparent();
                            //Adi 20131121
                            //if (frm.IsSuccessful)
                            //{
                            updateTotalAmount();
                            BindGrid();
                            //}


                        }
                    }
                    else
                    {
                        if (e.ColumnIndex == QTY_COL || e.ColumnIndex == RPRICE_COL || e.ColumnIndex == DISCPRICE_COL)
                        {

                            string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            OrderDet myDet = pos.GetLine(ID, out status);


                            //if (e.ColumnIndex == RPRICE_COL)
                            //{
                            //    myDet = pos.GetLine(ID, out status);
                            //    if (myDet.Item.IsInInventory = true && !myDet.Item.IsServiceItem.GetValueOrDefault(false))
                            //        //insufficient privilege
                            //        return;
                            //}

                            OrderForms.frmOrderLineKeypad t = new WinPowerPOS.OrderForms.frmOrderLineKeypad();
                            t.pos = pos;
                            t.LineID = ID;
                            t.isUserTokenForDiscountScanned = isUserTokenForDiscountScanned;
                            if (e.ColumnIndex == QTY_COL)
                            {
                                t.editField = frmOrderLineKeypad.EditedField.Quantity;
                            }
                            else if (e.ColumnIndex == RPRICE_COL)
                            {
                                t.editField = frmOrderLineKeypad.EditedField.RetailPrice;
                            }
                            if (e.ColumnIndex == DISCPRICE_COL)
                            {
                                t.editField = frmOrderLineKeypad.EditedField.DiscountedPrice;

                            }
                            t.ApplyPromo = ApplyPromo;

                            if (e.ColumnIndex == DISCPRICE_COL || e.ColumnIndex == RPRICE_COL)
                            {
                                string ItemNo = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                                Item it = new Item(ItemNo);
                                if (it.IsNonDiscountable)
                                {
                                    CommonUILib.hideTransparent();
                                    return;
                                }
                            }

                            CommonUILib.displayTransparent();
                            t.ShowDialog();
                            CommonUILib.hideTransparent();

                            BindGrid();
                            updateTotalAmount();
                            myDisplay.ShowTotal(double.Parse(lblTotalAmount.Text));

                        }
                        else if (e.ColumnIndex == SPCLDISCPRICE_COL || e.ColumnIndex == DISCOUNTDETAIL_COL || e.ColumnIndex == DISC_COL)
                        {
                            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DisableGiveLineDiscountSalesInv), false))
                            {
                                string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                                if (!GiveDiscountAllowed)
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
                                    }
                                }
                                bool isMultiTier = false;
                                if (e.ColumnIndex == SPCLDISCPRICE_COL)
                                    isMultiTier = EnableMultiTierPrice;

                                if (e.ColumnIndex == DISCOUNTDETAIL_COL || e.ColumnIndex == DISC_COL)
                                {
                                    string ItemNo = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                                    Item it = new Item(ItemNo);
                                    if (it.IsNonDiscountable)
                                    {
                                        //CommonUILib.hideTransparent();
                                        return;
                                    }
                                }
                                CommonUILib.displayTransparent();
                                frmDiscountSpecial frm = new frmDiscountSpecial(pos, ID, false, isMultiTier);
                                frm.StartPosition = FormStartPosition.CenterParent;
                                frm.ShowDialog();
                                CommonUILib.hideTransparent();
                                //Adi 20131121
                                //if (frm.IsSuccessful)
                                //{
                                updateTotalAmount();
                                BindGrid();
                                //}
                            }

                        }
                        else if (e.ColumnIndex == LINECOMMISSION_COL || e.ColumnIndex == LINECOMMISSION_COL2)
                        {
                            string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            //pop out line remark and sales person

                            OrderDet det = pos.GetLine(LineID, out status);

                            frmLineCommission f = new frmLineCommission(det.ItemNo);
                            f.Remark = det.Remark;
                            f.UserName = det.SalesPerson;
                            f.UserName2 = det.SalesPerson2;
                            f.OldReceiptNo = det.ReturnedReceiptNo;
                            f.LineInfoRemark = det.LineInfo;

                            if (det.Quantity < 0 || (det.Quantity > 0 && det.ItemNo == "LINE_DISCOUNT"))
                            {
                                f.IsSalesReturn = true;
                            }

                            if (det.Quantity < 0 && det.ItemNo == "LINE_DISCOUNT")
                            {
                                f.IsSalesReturn = false;
                            }

                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                //assign to Line ID
                                pos.AssignItemSalesPerson(LineID, f.UserName, f.UserName2);
                                pos.SetLineRemark(LineID, f.Remark, out status);
                                det.Userfld4 = f.LineInfoRemark.Trim();
                                pos.AssignSalesReturnReceiptNo(LineID, f.OldReceiptNo, out status);
                                //ItemReturn = f.OldReceiptNo;
                            }
                            f.Dispose();

                            updateTotalAmount();
                            BindGrid();
                            myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                            txtBarcode.Focus();
                        }
                    }
                }
                else
                {
                    if (e.ColumnIndex == QTY_COL || e.ColumnIndex == RPRICE_COL || e.ColumnIndex == DISCPRICE_COL)
                    {

                        string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                        OrderDet myDet = pos.GetLine(ID, out status);


                        //if (e.ColumnIndex == RPRICE_COL)
                        //{
                        //    myDet = pos.GetLine(ID, out status);
                        //    if (myDet.Item.IsInInventory = true && !myDet.Item.IsServiceItem.GetValueOrDefault(false))
                        //        //insufficient privilege
                        //        return;
                        //}

                        OrderForms.frmOrderLineKeypad t = new WinPowerPOS.OrderForms.frmOrderLineKeypad();
                        t.pos = pos;
                        t.LineID = ID;
                        t.isUserTokenForDiscountScanned = isUserTokenForDiscountScanned;
                        if (e.ColumnIndex == QTY_COL)
                        {
                            t.editField = frmOrderLineKeypad.EditedField.Quantity;
                        }
                        else if (e.ColumnIndex == RPRICE_COL)
                        {
                            t.editField = frmOrderLineKeypad.EditedField.RetailPrice;
                        }
                        if (e.ColumnIndex == DISCPRICE_COL)
                        {
                            t.editField = frmOrderLineKeypad.EditedField.DiscountedPrice;

                        }
                        t.ApplyPromo = ApplyPromo;

                        if (e.ColumnIndex == DISCPRICE_COL || e.ColumnIndex == RPRICE_COL)
                        {
                            string ItemNo = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                            Item it = new Item(ItemNo);
                            if (it.IsNonDiscountable)
                            {
                                CommonUILib.hideTransparent();
                                return;
                            }
                        }

                        CommonUILib.displayTransparent();
                        t.ShowDialog();
                        CommonUILib.hideTransparent();

                        BindGrid();
                        updateTotalAmount();
                        myDisplay.ShowTotal(double.Parse(lblTotalAmount.Text));

                    }
                    else if (e.ColumnIndex == SPCLDISCPRICE_COL || e.ColumnIndex == DISCOUNTDETAIL_COL || e.ColumnIndex == DISC_COL)
                    {
                        if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DisableGiveLineDiscountSalesInv), false))
                        {
                            string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                            if (!GiveDiscountAllowed)
                            {
                                //Adi 
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.UseUserTokenToGiveDiscount), false))
                                {
                                    //use user token to give discount, if haven't 
                                    if (!isUserTokenForDiscountScanned)
                                    {
                                        DialogResult result = DialogResult.Cancel;
                                        //membershipNo = "";
                                        try
                                        {
                                            frmScanStaffCard fScanStaffCard = new frmScanStaffCard();
                                            fScanStaffCard.title = "Scan User Token";
                                            fScanStaffCard.txtMessage = "Please Scan Staff Token for discount";
                                            //fScanStaffCard.Mode = mode;
                                            result = fScanStaffCard.ShowDialog();
                                            if (result != DialogResult.OK)
                                            {
                                                return;
                                            }
                                            //string role, status, DeptID;
                                            UserMst user;
                                            user = new UserMst(fScanStaffCard.membershipNo);

                                            if (user != null && user.UserName == fScanStaffCard.membershipNo)
                                            {
                                                UserGroup grp = new UserGroup(user.GroupName);
                                                if (PrivilegesController.HasPrivilege(PrivilegesController.GIVE_DISCOUNT, UserController.FetchGroupPrivilegesWithUsername(grp.GroupName, user.UserName)))
                                                {
                                                    //mySupervisor = fScanStaffCard.membershipNo;
                                                    //IsAuthorized = true;
                                                    isUserTokenForDiscountScanned = true;
                                                    //this.Close();

                                                }
                                                else
                                                {
                                                    MessageBox.Show(fScanStaffCard.membershipNo + " has insufficient privileges.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Logger.writeLog(ex);
                                        }
                                    }
                                }
                                else
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
                                    }
                                }
                            }
                            bool isMultiTier = false;
                            if (e.ColumnIndex == SPCLDISCPRICE_COL)
                                isMultiTier = EnableMultiTierPrice;

                            if (e.ColumnIndex == DISCOUNTDETAIL_COL || e.ColumnIndex == DISC_COL)
                            {
                                string ItemNo = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                                Item it = new Item(ItemNo);
                                if (it.IsNonDiscountable)
                                {
                                    //CommonUILib.hideTransparent();
                                    return;
                                }
                            }
                            CommonUILib.displayTransparent();
                            frmDiscountSpecial frm = new frmDiscountSpecial(pos, ID, false, isMultiTier);
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog();
                            CommonUILib.hideTransparent();
                            //Adi 20131121
                            //if (frm.IsSuccessful)
                            //{
                            updateTotalAmount();
                            BindGrid();
                        }
                    }
                    else if (e.ColumnIndex == LINECOMMISSION_COL || e.ColumnIndex == LINECOMMISSION_COL2)
                    {
                        string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                        //pop out line remark and sales person

                        OrderDet det = pos.GetLine(LineID, out status);

                        frmLineCommission f = new frmLineCommission(det.ItemNo);
                        f.Remark = det.Remark;
                        f.UserName = det.SalesPerson;
                        f.UserName2 = det.SalesPerson2;
                        f.OldReceiptNo = det.ReturnedReceiptNo;
                        f.LineInfoRemark = det.LineInfo;

                        if (det.Quantity < 0 || (det.Quantity > 0 && det.ItemNo == "LINE_DISCOUNT"))
                        {
                            f.IsSalesReturn = true;
                        }

                        f.ShowDialog();
                        if (f.IsSuccessful)
                        {
                            //assign to Line ID
                            pos.AssignItemSalesPerson(LineID, f.UserName, f.UserName2);
                            pos.SetLineRemark(LineID, f.Remark, out status);
                            det.Userfld4 = f.LineInfoRemark.Trim();
                            pos.AssignSalesReturnReceiptNo(LineID, f.OldReceiptNo, out status);
                            //ItemReturn = f.OldReceiptNo;
                        }
                        f.Dispose();

                        updateTotalAmount();
                        BindGrid();
                        myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                        txtBarcode.Focus();
                    }
                }
                
            }
        }
        private void dgvPurchase_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {

                if (pos.IsVoided(dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString(), out status))
                {
                    Font f = dgvPurchase.DefaultCellStyle.Font;
                    for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                    {
                        if ((dgvPurchase.Columns[i].Visible))
                        {
                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.Font = new Font(f, FontStyle.Strikeout);
                        }
                    }
                }
                else if (bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells["IsPromo"].Value.ToString().ToLower()) == true)
                {
                    for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                    {
                        if ((dgvPurchase.Columns[i].Visible))
                        {
                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }
                else if (bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells["IsNonDiscountable"].Value.ToString().ToLower()) == true)
                {
                    for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                    {
                        if ((dgvPurchase.Columns[i].Visible))
                        {
                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                }
                else if (bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells["IsSpecial"].Value.ToString().ToLower()) == true)
                {
                    for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                    {
                        if ((dgvPurchase.Columns[i].Visible))
                        {
                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.Snow;
                        }
                    }
                }
                else if (bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells["IsPreOrder"].Value.ToString().ToLower()) == true)
                {
                    for (int i = 0; i < dgvPurchase.ColumnCount; i++)
                    {
                        if ((dgvPurchase.Columns[i].Visible))
                        {

                            dgvPurchase.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.LightSeaGreen;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BindGrid()
        {
            try
            {
                string status = "";

                if (pos == null) return;

                //set order header
                lblRemark.Text = pos.GetHeaderRemark();
                //lblOrderDate.Text = pos.GetOrderDate().ToString();
                /*
                decimal disc = pos.getOverallDisc();
                if (disc > 0)
                    btnOverallDiscount.Text = "Total Disc: " + disc.ToString("N0") + "%";
                else
                    btnOverallDiscount.Text = "No Overall Disc";
                */
                //populate order items 
                   
                dgvPurchase.DataSource = pos.FetchUnSavedOrderItemsForGrid(out status);
                if (status != "")
                {
                    MessageBox.Show("Error: " + status);
                    return;
                }

                //ShowItemPicture();

                dgvPurchase.Refresh();
                if (dgvPurchase.Rows.Count > 0)
                    dgvPurchase.Rows[0].Selected = true;
                updateTotalAmount();

               

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                {
                    fOrderSecondScreen.pos = pos;
                    fOrderSecondScreen.UpdateView();
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBarcode.Focus();
            }
        }
        private bool GetValueFromRow(out int qty, out decimal unitPrice, out decimal discount, DataGridViewCellEventArgs e)
        {
            qty = -1;
            unitPrice = -1;
            discount = -1;
            if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), out qty))
            {
                if (!bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells["IsExchange"].Value.ToString()) && qty < 0)
                {
                    MessageBox.Show("You need to enter a non negative number for quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dgvPurchase.CancelEdit();
                    return false;
                }
            }

            if (
                !decimal.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Price"].Value.ToString(), out unitPrice)
                || unitPrice < 0
                )
            {
                //Invalid value entered
                MessageBox.Show("You need to enter a non negative number for unit price", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPurchase.CancelEdit();
                return false;
            }

            if (!decimal.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Discount"].Value.ToString(), out discount) || discount < 0 || discount > 100)
            {
                //Invalid value entered
                MessageBox.Show("You need to enter a non negative number for discount and discount must not be more than 100%", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPurchase.CancelEdit();
                return false;
            }
            return true;
        }
        private void dgvPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string status;
            bool IsPromo, IsExchange, IsNonDiscountable;

            if (e.RowIndex >= 0)
                if (e.ColumnIndex == ISSPECIAL_COL) //Mark as IsSpecial.... 
                {
                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsPromo"].Value.ToString(), out IsPromo)
                        && IsPromo)
                    {
                        return; //promo cannot
                    }
                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsExchange"].Value.ToString(), out IsExchange)
                        && IsExchange)
                    {
                        return; //exchange cannot
                    }

                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsNonDiscountable"].Value.ToString(), out IsNonDiscountable)
                        && IsNonDiscountable)
                    {
                        return; //IsNonDiscountable cannot
                    }
                    string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    bool IsSpecialValue = bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells[ISSPECIAL_COL].Value.ToString());
                    dgvPurchase.Rows[e.RowIndex].Cells[ISSPECIAL_COL].Value =
                        !IsSpecialValue;

                    if (!pos.SetIsSpecial(LineID, !IsSpecialValue, out status))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    BindGrid();
                    updateTotalAmount();
                    txtBarcode.Focus();
                }
                else if (e.ColumnIndex == FOC_COL) //Mark as IsFreeOfCharge
                {
                    //Check privilege....
                    if (!PrivilegesController.HasPrivilege("Give FOC", UserInfo.privileges))
                    {
                        //dgvPurchase.Rows[e.RowIndex].Cells[FOC_COL].Value = false;
                        dgvPurchase.CancelEdit();
                        MessageBox.Show("Insufficient privilege to give FOC");

                        return;
                    }
                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsPromo"].Value.ToString(), out IsPromo) && IsPromo)
                    {
                        return;
                    }
                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsExchange"].Value.ToString(), out IsExchange)
                        && IsExchange)
                    {
                        return;
                    }
                    string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    dgvPurchase.Rows[e.RowIndex].Cells[FOC_COL].Value = !(bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells[FOC_COL].Value.ToString()));

                    if (!pos.SetIsFreeOfCharge(LineID,
                        bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells[FOC_COL].Value.ToString()), out status))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //dgvPurchase.Rows[e.RowIndex].Cells["Amount"].Value = pos.GetLineAmount(LineID, out status).ToString("N2");
                    BindGrid();
                    updateTotalAmount();
                    myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                    txtBarcode.Focus();
                }
                else if (e.ColumnIndex == PREORDER_COL) //Mark as Pre Order
                {

                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsExchange"].Value.ToString(), out IsExchange)
                        && IsExchange)
                    {
                        return;
                    }
                    string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    dgvPurchase.Rows[e.RowIndex].Cells[PREORDER_COL].Value = !(bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells[PREORDER_COL].Value.ToString()));

                    if (!pos.SetPreOrder(LineID,
                        bool.Parse(dgvPurchase.Rows[e.RowIndex].Cells[PREORDER_COL].Value.ToString()), out status))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //dgvPurchase.Rows[e.RowIndex].Cells["Amount"].Value = pos.GetLineAmount(LineID, out status).ToString("N2");
                    updateTotalAmount();
                    BindGrid();
                    myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                    txtBarcode.Focus();
                }
                else if (e.ColumnIndex == VOID_COL) //Mark as Voided
                {

                    bool isPromo = false;
                    #region (*obsolete code
                    //if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsPromo"].Value.ToString(),
                    //    out isPromo)
                    //    && isPromo)
                    //{
                    //    string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                    //    int tmpPromoID = 0;
                    //    if (int.TryParse(LineID, out tmpPromoID))
                    //    {
                    //        bool isvoided = pos.IsPromoVoided(tmpPromoID, out status);
                    //        if (!pos.SetVoidPromoOrderLine(tmpPromoID,
                    //        !isvoided, out status))
                    //        {
                    //            MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    #endregion

                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsExchange"].Value.ToString(),
                            out IsExchange)
                            && IsExchange)
                    {
                        return;
                    }

                    bool IsAuthorized = false;
                    string SupID = "";

                    #region *) Authorization: Check Supervisor ID
                    bool Prompt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnVoidItem), false);

                    
                    if (Prompt && !PrivilegesController.HasPrivilege(PrivilegesController.VOID_BILL, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username)))
                    {
                        string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                        if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                        {

                            frmReadMSR f = new frmReadMSR();
                            f.privilegeName = PrivilegesController.VOID_BILL;
                            f.loginType = LoginType.Authorizing;
                            f.ShowDialog();
                            IsAuthorized = f.IsAuthorized;
                            f.Dispose();
                        }
                        else
                        {
                            //Ask for verification....
                            //Prompt Supervisor Password
                            LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                            sl.privilegeName = PrivilegesController.VOID_BILL;
                            sl.ShowDialog();
                            SupID = sl.mySupervisor;
                            IsAuthorized = sl.IsAuthorized;                                                       
                        }
                    }
                    else
                    { IsAuthorized = true; }
                    #endregion

                    if (IsAuthorized)
                    {
                        string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                        AccessLogController.AddAccessLog(PowerPOS.AccessSource.POS, UserInfo.username, SupID, "VOID Item", "VOID Item " + pos.GetLine(LineID, out status).ItemNo + " Amount : " + pos.GetLine(LineID, out status).Amount.ToString("N2"));
                        bool isvoided = pos.IsVoided(LineID, out status);
                        if (status != "")
                        {
                            MessageBox.Show(status);
                            return;
                        }
                        dgvPurchase.Rows[e.RowIndex].Cells[VOID_COL].Value = !isvoided;

                        if (!pos.SetVoidOrderLine(LineID,
                            !isvoided, out status))
                        {
                            MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }


                        //}
                        //pos.UndoPromo();
                        //pos.ApplyPromo();
                        //dgvPurchase.Rows[e.RowIndex].Cells["Amount"].Value = pos.GetLineAmount(LineID, out status).ToString("N2");
                        updateTotalAmount();
                        BindGrid();
                        myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                        txtBarcode.Focus();
                    }
                }
        }


        private void dgvPurchase_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dgvPurchase.Rows[e.RowIndex].Cells[0].Value = (((DataTable)dgvPurchase.DataSource).Rows.Count - e.RowIndex).ToString();
        }
        private bool GetValueFromRow(out int qty, out decimal unitPrice, out decimal discount, out decimal discountDollar, DataGridViewCellEventArgs e)
        {
            qty = -1;
            unitPrice = -1;
            discount = -1;
            discountDollar = -1;
            if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), out qty) || qty < 0)
            {
                MessageBox.Show("You need to enter a non negative number for quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPurchase.CancelEdit();
                return false;
            }

            if (!decimal.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Price"].Value.ToString(), out unitPrice) || unitPrice < 0)
            {
                //Invalid value entered
                MessageBox.Show("You need to enter a non negative number for unit price", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPurchase.CancelEdit();
                return false;
            }

            if (!decimal.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Discount"].Value.ToString(), out discount) || discount < 0 || discount > 100)
            {
                //Invalid value entered
                MessageBox.Show("You need to enter a non negative number for discount and discount must not be more than 100%", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPurchase.CancelEdit();
                return false;
            }

            if (!decimal.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["DiscDollar"].Value.ToString(), out discountDollar) || discountDollar < 0 || discountDollar > unitPrice)
            {
                //Invalid value entered
                MessageBox.Show("You need to enter a non negative number for discount and discount must not be less than unit price", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dgvPurchase.CancelEdit();
                return false;
            }


            return true;
        }

        #endregion

        private void btnRemark_Click(object sender, EventArgs e)
        {
            /*Open remark form*/

            frmRemark frm = new frmRemark();
            frm.txtRemark.Text = pos.GetHeaderRemark();
            frm.ShowDialog();
            if (frm.IsSuccessful)
            {
                pos.SetHeaderRemark(frm.txtRemark.Text);
                lblRemark.Text = frm.txtRemark.Text;
            }
            frm.Dispose();
            txtBarcode.Focus();
        }


        #region "Hold Customer"

        private void btnUnHold_Click(object sender, EventArgs e)
        {
            try
            {

                //check if using server hold
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sales.EnableHoldOrderFromServer), false))
                {
                    string serverUrl = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
                    string url = serverUrl + "API/pos/GetListHoldTransaction.ashx";
                    using (WebClient webClient = new WebClient())
                    {
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("Hold", "0");
                        byte[] responsebytes = webClient.UploadValues(url, "POST", parameters);
                        string jsonDataRaw = Encoding.UTF8.GetString(responsebytes);
                        var jsonData = JsonConvert.DeserializeObject<HoldTransactionServerModel>(jsonDataRaw);

                        if (jsonData.HoldList != null)
                        {
                            var HoldList = JsonConvert.DeserializeObject<List<HoldTransaction>>(jsonData.HoldList.ToString());

                            foreach (HoldTransaction data in HoldList)
                            {

                                string query = "insert into HoldTransaction(HoldGuid, AppTime, Membershipno,MembershipName, NRIC, LineInfo, POSControllerObject, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy) Values( @HoldGuid, @AppTime, @Membershipno,@MembershipName, @NRIC, @LineInfo, @POSControllerObject, @CreatedOn, @CreatedBy, @ModifiedOn, @ModifiedBy)";
                                //string query = "select count(*) from OrderHdr a where a.OrderHdrID = '" + data.OrderHdrID + "'";

                                QueryCommand cmd = new QueryCommand(query);
                                cmd.Parameters.Add("@HoldGuid", data.HoldGuid, DbType.Guid);
                                cmd.Parameters.Add("@AppTime", data.AppTime, DbType.DateTime);
                                cmd.Parameters.Add("@Membershipno", data.MembershipNo);
                                cmd.Parameters.Add("@MembershipName", data.MembershipName);
                                cmd.Parameters.Add("@NRIC", data.Nric);
                                cmd.Parameters.Add("@LineInfo", data.LineInfo);
                                cmd.Parameters.Add("@POSControllerObject", data.POSControllerObject, DbType.Binary);
                                cmd.Parameters.Add("@CreatedOn", data.CreatedOn, DbType.DateTime);
                                cmd.Parameters.Add("@CreatedBy", data.CreatedBy);
                                cmd.Parameters.Add("@ModifiedOn", data.ModifiedOn, DbType.DateTime);
                                cmd.Parameters.Add("@ModifiedBy", data.ModifiedBy);


                                DataService.ExecuteQuery(cmd);
                            }
                        }
                    }
                }

                #region *) Validation: Prompt if active pos contains any transaction
                if (pos.GetLineRowCount() > 0 || pos.MembershipApplied())
                    throw new Exception("(warning)The item has not been hold.\nPlease hold before you unhold.");
                #endregion

                using (frmHold instance = new frmHold())
                {
                    if (instance.ShowDialog() != DialogResult.OK) return;

                    #region *) Get new POS Controller
                    POSController tmpPOS = instance.SelectedPOS;
                    if (tmpPOS == null) return;
                    pos = tmpPOS;
                    pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
                    pos.OpenPriceItemAddedEditField += new OpenPriceItemEditFieldHandler(pos_OpenPriceItemAddedEditField);
                    pos.AutoCaptureWeightItemAdded += new AutoCaptureWeightItemHandler(pos_AutoCaptureWeightItemAdded);
                    #endregion

                    #region *) Save Log
                    AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, PowerPOS.AccessSource.POS, UserInfo.username, "", "UnHold Order", pos.FetchUnsavedOrderDetText());

                    #endregion

                    #region *) Refresh frmOrderTaking display
                    if (pos.MembershipApplied())
                        DisplayMembershipToScreen(pos.GetMemberInfo());
                    BindGrid();

                    if (!string.IsNullOrEmpty(pos.GetLineInfoOfFirstLine()))
                    {
                        lblLineInfo.Text = pos.GetLineInfoOfFirstLine();
                    }

                    //refresh price display pole after unhold                         
                    myDisplay.ClearScreen();
                    myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                    #endregion
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Some error occured. Please contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(X);
                }
            }
        }
        private void btnHold_Click(object sender, EventArgs e)
        {
            try
            {
                HoldController instance = new HoldController();
                #region *) Validation: Reject if nothing to hold
                if (!(pos.GetLineRowCount() > 0 || pos.MembershipApplied()))
                    throw new Exception("(warning)You don't need to hold empty order");

                //Setting
                int maxHoldSettings = 0;
                string maxHoldStr = AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.MaxHoldTransaction);
                if (int.TryParse(maxHoldStr, out maxHoldSettings))
                {
                    int holdCount = instance.ToDataTable().Rows.Count;
                    if (holdCount >= maxHoldSettings)
                    {
                        MessageBox.Show("System can only hold maximum " + maxHoldStr + " transactions. Please clear the hold transaction first. ");
                        return;
                    }
                }
                #endregion

                #region *) Save Hold
                
                instance.SaveHold(pos);
                #endregion

                #region *) Save Log
                AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, PowerPOS.AccessSource.POS, UserInfo.username, "", "Hold Order", pos.FetchUnsavedOrderDetText());
                
                #endregion

                #region *) Clear frmOrderTaking
                pos = new POSController();
                pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
                pos.OpenPriceItemAddedEditField += new OpenPriceItemEditFieldHandler(pos_OpenPriceItemAddedEditField);
                pos.AutoCaptureWeightItemAdded += new AutoCaptureWeightItemHandler(pos_AutoCaptureWeightItemAdded);
                clearControls();
                #endregion
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Some error occured. Please contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(X);
                }
            }
        }
        #endregion

        private void ClearMembership()
        {
            // Clear Membership info
            lblMemberGroup.Text = "-";
            lblMembershipNo.Text = "-";

            lblName.Text = "-";
            lblExpiryDate.Text = "-";
            lblDiscount.Text = "-";
            lblDOB.Text = "-";


        }


        private void btnAddMember_Click(object sender, EventArgs e)
        {
            frmMembershipQuickAdd frm = new frmMembershipQuickAdd();
            frm.ShowDialog();
            frm.Dispose();
            txtBarcode.Focus();
        }
        private void rbPromoEnable_CheckedChanged(object sender, EventArgs e)
        {
            //
            if (rbPromoEnable.Checked)
            {
                ApplyPromo = true;
                pos.ApplyPromo();
                BindGrid();
            }
            else
            {
                ApplyPromo = false;
                pos.UndoPromo();
                pos.ApplyMembershipDiscount();
                BindGrid();
            }
        }
        /// <summary>
        /// Check Function when get Point and Break Down price value reach 1 this function check from Item table
        /// </summary>
        /// <param name="ItemNo"></param>
        /// <returns></returns>
        public DataTable GetDataBreakDownPrice(string ItemNo)
        {
            string sqlqry = "Select isNull(userfloat3,0) as userfloat3,IsNUll(userfloat1,0) as userfloat1 from Item where ItemNo=@ItemNo";
            QueryCommand qcmd = new QueryCommand(sqlqry);
            qcmd.AddParameter("@ItemNo", ItemNo);
            DataTable objcol = new DataTable();
            objcol.Load(DataService.GetReader(qcmd));
            return objcol;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Documented by 205
        /// </remarks>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.RequireCustomerConfirm), false))
            {
                if (fOrderSecondScreen != null && fWaitConfirmSecondary != null)
                {
                    /*Thread t = new Thread(new ThreadStart(LaunchDialogWaitConfirmSecondary));
                    t.Start();*/
                    //fWaitConfirmSecondary.Show
                    fWaitConfirmSecondary.fOrderSecondScreen = fOrderSecondScreen;
                    DialogResult dr = fWaitConfirmSecondary.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        fWaitConfirmSecondary.Hide();
                        doConfirm();
                    }
                    else
                    {
                        fWaitConfirmSecondary.Hide();
                        MessageBox.Show("Customer did not confirm the amount");
                        return;
                    }
                }
            }
            else
            {
                doConfirm();
            }
        }

        private void doConfirm()
        {
            try
            {
                Thread t = new Thread(new ThreadStart(LaunchDialogAskPrintReceipt));
                this.bufPrintReceipt = -1;
                this.EmailReceipt = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.AskPrintEmailReceipt), false))
                {
                    
                    frmPrintReceipt = new frmPrintReceipt();
                    frmPrintReceipt.fOrderTaking = this;
                    frmPrintReceipt.isHaveEmail = pos.MembershipApplied() && (!String.IsNullOrEmpty(pos.GetMemberInfo().Email));
                    t.Start();
                }

                #region *) Initialize: Update OrderTime to NOW
                pos.myOrderHdr.OrderDate = DateTime.Now;
                #endregion

                #region *) Validation: Order cannot be empty
                if (!pos.HasOrderLine())
                {
                    txtBarcode.Select();
                    return;
                }
                #endregion

                #region *) Show reminder message if enabled
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowReminderWhenConfirm), false))
                {
                    string message = AppSetting.GetSetting(AppSetting.SettingsName.Order.ReminderMessage);
                    if (!string.IsNullOrEmpty(message))
                        MessageBox.Show(message, "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion

                #region *) Validation: Line Info is mandatory or not
                bool isLineInfoMandatory = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.IsMandatory), false);
                string lineInfoMandatoryType = AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.MandatoryType);
                if (isLineInfoMandatory && (string.IsNullOrEmpty(lineInfoMandatoryType) || lineInfoMandatoryType.ToUpper() == "FOR ALL"))
                {
                    foreach (OrderDet od in pos.myOrderDet)
                    {
                        if (string.IsNullOrEmpty(od.LineInfo))
                        {
                            if (string.IsNullOrEmpty(lineInfoCaption)) lineInfoCaption = "Line Info";
                            MessageBox.Show(string.Format("Please assign {0} first.", lineInfoCaption), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                #endregion

                #region *) Validation Mandatory Membership for pre order transaction

                if (pos.hasPreOrder() && !pos.MembershipApplied())
                {
                    MessageBox.Show("Must assign membership for Pre Order Transaction", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #endregion

                #region *) Validation for Item.AllowPreOrder = true
                foreach (OrderDet od in pos.myOrderDet)
                {
                    if (!od.IsVoided && od.IsPreOrder.GetValueOrDefault(false) && od.Quantity > 0)
                    {
                        if (od.Item.AllowPreOrder && od.Item.CapQty > 0)
                        {
                            int soldQty = ItemController.FetchPreOrderSoldQty(od.Item.ItemNo);
                            int balQty = od.Item.CapQty - soldQty;
                            if (balQty <= 0)
                            {
                                MessageBox.Show(string.Format("Cannot order item {0} because pre-order quantity has reached cap quantity.", od.Item.ItemName), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            else
                            {
                                if (od.Quantity > balQty)
                                {
                                    MessageBox.Show(string.Format("Only {0} quantity left for pre-order for item {1}. Please reduce the ordered quantity.", balQty.ToString(), od.Item.ItemName), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                        }
                    }else if (!od.IsVoided && od.IsPreOrder.GetValueOrDefault(false) && od.Quantity < 0)
                    {
                        if (string.IsNullOrEmpty(od.ReturnedReceiptNo))
                        {
                            MessageBox.Show(string.Format("Cannot refund pre order item {0} because no Returned Receipt recorded.", od.Item.ItemName), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                #endregion

                #region *) Initialize: No Membership = [WALK-IN] membership
                if (!pos.MembershipApplied())
                {
                    if (IsMembership_Compulsory)
                    {
                        MessageBox.Show("Please register a member or Scan membership card.", "Membership is Compulsory");
                        return;
                    }
                    if (mode == "formal")
                    {
                        MessageBox.Show("Please register a member or Scan membership card.", "Membership is Compulsory");
                        return;
                    }
                    if (!pos.AssignMembership("WALK-IN", out status))
                    {
                        Logger.writeLog("Unable to assign walk in member: " + status);
                    }
                }
                #endregion

                #region *) Validation: WALK-IN cannot buy points/packages
                if (pos.HasOrderedPointPackageItems(true) && pos.CurrentMember.NameToAppear == "WALK-IN")
                {
                    MessageBox.Show("Please register a member or Scan membership card.", "Membership is Compulsory");
                    return;
                }
                #endregion

                bool isOutletSales = false;

                #region *) Request Sales Date
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false))
                {
                   
                    if (pos.GetMemberInfo() != null && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                    {
                        //validation if outlet order
                        PointOfSaleCollection posColl = new PointOfSaleCollection();
                        posColl.Where(PointOfSale.UserColumns.LinkedMembershipNo, pos.GetMemberInfo().MembershipNo);
                        posColl.Load();
                        if (posColl.Count > 0)
                            isOutletSales = true;

                    }
                    if (isOutletSales && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AllowBackdatedSales), false))
                    {
                        frmRequestSalesDate fRequestSalesDate = new frmRequestSalesDate();
                        fRequestSalesDate.ShowDialog();
                        if (fRequestSalesDate.IsSuccessful)
                        {
                            pos.myOrderHdr.OrderDate = fRequestSalesDate.dateResult;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                #endregion

                bool IsQtyInsufficient = false;

                #region *) Validation: Check if Qty is sufficient to do stockout [Prompt if Error - Can be terminated by user]
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.BlockTransactionIfBalQtyNotSuf), false))
                {
                    if (!pos.IsQtySufficientToDoStockOutLocal(out status))
                    {
                        DialogResult dr = MessageBox.Show
                                ("Error: " + status + ". You can not continue the transaction.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {

                    if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.RemoveZeroInventoryValidation), false) && !isOutletSales)
                    {
                        if (!pos.IsQtySufficientToDoStockOutLocal(out status))
                        {

                            DialogResult dr = MessageBox.Show
                                ("Error: " + status + ". Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.No)
                            {
                                pos.DeleteAllReceiptLinePayment();
                                return;
                            }
                            IsQtyInsufficient = true;
                        }
                    }
                }

                #endregion

                if (Features.Package.isAvailable && pos.containPackages && !pos.MembershipApplied())
                    throw new Exception("(warning)Please assign membership to continue");
                if (pos.IsItemIsInOrderLine("INS_PAYMENT") != "" && !pos.MembershipApplied())
                    throw new Exception("(warning)Please assign membership to continue");

                #region *) Validation: Package Usage Quantity Check

                string packageStatus = "";
                if (!pos.CheckPackageUsageQuantity(out packageStatus))
                {
                    MessageBox.Show(packageStatus);
                    return;
                }

                #endregion

                #region *) Validation: Check whether there's insufficient Points for refund
                if (pos.HasReturnedPointPackageItems())
                {   
                    string msg = "{0} cannot be refunded because remaining points is insufficient (Refund: {1}, Remaining: {2}).";
                    DataTable CurrentAmount = new DataTable();
                    if (PowerPOS.Feature.Package.GetCurrentAmount(pos.GetMemberInfo().MembershipNo, pos.myOrderHdr.OrderDate, out CurrentAmount, out status))
                    {
                        if (CurrentAmount.Rows.Count > 0)
                        {
                            List<string> insufficient = new List<string>();

                            foreach (OrderDet od in pos.GetOrderItemPointPackage())
                            {
                                if (od.Quantity > 0) continue; // Not a refund item, no need to process
                                if (od.Userflag5.GetValueOrDefault(false) == true) continue; //use package not validated

                                decimal pointToRefund = Math.Abs(od.Item.PointGetAmount == 0 ? od.Amount : od.Quantity.GetValueOrDefault(0) * od.Item.PointGetAmount);

                                DataRow[] rows = CurrentAmount.Select("RefNo='" + od.Item.ItemName + "'");
                                if (rows.Length > 0)
                                {
                                    decimal pointAvailable = 0;
                                    decimal.TryParse(rows[0]["Points"].ToString(), out pointAvailable);
                                    if (pointToRefund > pointAvailable)
                                        insufficient.Add(string.Format(msg, od.Item.ItemName, pointToRefund.ToString("N2"), pointAvailable.ToString("N2")));
                                }
                                else
                                {
                                    insufficient.Add(string.Format(msg, od.Item.ItemName, pointToRefund.ToString("N2"), "0.00"));
                                }
                            }

                            if (insufficient.Count > 0)
                            {
                                MessageBox.Show(insufficient.ToArray().AsSingleLineString(Environment.NewLine));
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cannot refund point/package item because currently there's no points/package available for this member.");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(status);
                        return;
                    }
                }
                #endregion

                #region *) Initialize:Check When Get Point Value and BreakDown Price is 1
                /*if (dgvPurchase.Rows.Count > 0)
                {
                    DataTable Dt = new DataTable();
                    Decimal GetPointValue = 0, BreakDownPrice = 0;
                    for (Int32 i = 0; i < dgvPurchase.Rows.Count; i++)
                    {
                        /////Get Item No from Grid
                        String ItemNo = dgvPurchase.Rows[i].Cells["ItemNo"].Value.ToString();
                        //// Get Item Name rom grid whose pick from the list
                        String ItemName = dgvPurchase.Rows[i].Cells["ItemName"].Value.ToString();
                        ////Check Function for get time balue and breakdown price return datatable
                        Dt = GetDataBreakDownPrice(ItemNo);
                        /////if data table return greater then 0 row 
                        if (Dt.Rows.Count > 0)
                        {
                            GetPointValue = Convert.ToDecimal(Dt.Rows[0]["userfloat1"].ToString());
                            BreakDownPrice = Convert.ToDecimal(Dt.Rows[0]["userfloat3"].ToString());
                            //// If Get Point value and Break Down Price 1 then show New window for Update Get point value and break down price
                            if (GetPointValue == 1 && BreakDownPrice == 1)
                            {
                                frmBreakPointValue f = new frmBreakPointValue();
                                f.ItemNo = ItemNo;
                                f.ItemName = ItemName;
                                f.ShowDialog();
                                f.Dispose();
                            }
                        }
                    }
                }*/
                #endregion

                #region *) Initialize: Set Pre-Order Information
                if (pos.hasPreOrder())
                {
                    pos.setPreOrderInfo(pos.CurrentMember.NameToAppear, pos.CurrentMember.Mobile, PointOfSaleInfo.PointOfSaleName);

                    //frmPreOrderInquiry f = new frmPreOrderInquiry();
                    //f.pos = pos;
                    //f.ShowDialog();
                    //if (!f.isSuccessful)
                    //{
                    //    txtBarcode.Select();
                    //    return;
                    //}
                }
                #endregion

                //MessageBox.Show(pos.CalculateTotalAmount(out status).ToString());

                #region *) Prompt for Supervisor password if there's returned item
                if (pos.HasReturnedItems())
                {
                    bool IsAuthorized = true;
                    string SupID = "-";
                    bool Prompt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnRefund), false);
                    bool giveRefund = PrivilegesController.HasPrivilege(PrivilegesController.GIVE_REFUND, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username));
                    if (!giveRefund)
                    {
                        string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                        if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                        {

                            frmReadMSR f = new frmReadMSR();
                            f.privilegeName = PrivilegesController.GIVE_REFUND;
                            f.loginType = LoginType.Authorizing;
                            f.ShowDialog();
                            IsAuthorized = f.IsAuthorized;
                            f.Dispose();
                        }
                        else
                        {
                            //Ask for verification....
                            //Prompt Supervisor Password
                            LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                            sl.privilegeName = PrivilegesController.GIVE_REFUND;
                            sl.ShowDialog();
                            SupID = sl.mySupervisor;
                            IsAuthorized = sl.IsAuthorized;
                        }
                    }
                    else
                    {
                        IsAuthorized = true;
                    }

                    if (!IsAuthorized) return;

                    #region Refund Reason
                    bool returnInv = true;
                    string Remarks = "";
                    string selectableRemark = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableVoidReason);
                    if (string.IsNullOrEmpty(selectableRemark))
                    {
                        frmVoidRemark fRemark = new frmVoidRemark();
                        fRemark.pos = pos;
                        fRemark.isRefund = true;
                        CommonUILib.displayTransparent();
                        fRemark.ShowDialog();
                        CommonUILib.hideTransparent();
                        if (fRemark.IsSuccess)
                        {
                            returnInv = fRemark.returnInventory;
                            Remarks = fRemark.pos.myOrderHdr.Remark;
                        }
                        else
                        {
                            fRemark.Dispose();
                            return;
                        }
                        fRemark.Dispose();
                    }
                    else
                    {
                        frmSelectVoidReason fSelecReason = new frmSelectVoidReason();
                        fSelecReason.pos = pos;
                        fSelecReason.isRefund = true;
                        CommonUILib.displayTransparent();
                        fSelecReason.ShowDialog();
                        CommonUILib.hideTransparent();
                        if (fSelecReason.IsSuccess)
                        {
                            Remarks = fSelecReason.Reason;
                            returnInv = fSelecReason.returnInventory;
                        }
                        else
                        {
                            fSelecReason.Dispose();
                            return;
                        }
                        fSelecReason.Dispose();
                    }

                    if (!returnInv)
                    {
                        foreach (OrderDet od in pos.myOrderDet)
                        {
                            if (od.Quantity < 0)
                            {
                                od.InventoryHdrRefNo = "NOTRETURNED";
                            }
                        }
                    }


                    #endregion
                }
                #endregion

                #region *) Core: Handle Negative Amount (TotalAmount of OrderDet <= 0)
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false))
                {
                    if (pos.CalculateTotalAmount(out status) < (decimal)0.01)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        HandleNegativeAmount();
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                }
                else
                {
                    if (pos.CalculateTotalAmount(out status) < (decimal)0.01 && pos.CalculateTotalAmount(out status) > (decimal)-0.01 && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowPaymentTypeWhenZeo), false))
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        HandleNegativeAmount();
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                }

                #endregion

                /// TODO: [BUG]
                /// Validation won't success. Covered in the Negative Amount.
                //#region *) Validation: Do not confirm is amount is 0
                //if (this.lblTotalAmount.Text == "0" || this.lblTotalAmount.Text == "0.00")
                //{
                //    txtBarcode.Focus();
                //    return;
                //}
                //#endregion

                /// TODO: [BUG]
                /// Validation duplicated
                #region -= Duplicated - IGNORE IT =-
                //check amount
                if (!pos.HasOrderLine())
                {
                    //
                    //MessageBox.Show("There is no item in order. Please add product into order.");                    
                    return;
                }

                /*if (pos.CalculateTotalAmount(out status) <= (decimal)0.01)
                {
                    HandleNegativeAmount();
                    return;
                }*/

                //check amount
                //int zeroAmount = 0;
                //bool result = int.TryParse(this.lblTotalAmount.Text.ToString(), out zeroAmount);
                //if (zeroAmount == 0)
                if (this.lblTotalAmount.Text == "0")
                {
                    //
                    //MessageBox.Show("The payment amount is $0. Please add product into order.");
                    //HandleNegativeAmount();
                    txtBarcode.Focus();
                    return;
                }
                #endregion

                /// Comment: Also handled in HandleNegativePayment
                /// 
                #region *) Check if member exceeds limit
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AutoMembershipUpgrade), false))
                {
                    bool isExist = false;
                    string exist = pos.IsItemIsInOrderLine("RENEWAL");
                    if (exist != "")
                        isExist = true;

                    if (pos.MembershipApplied() && !isExist)
                    {
                        Membership theMember = pos.GetMemberInfo();
                        if (theMember != null)
                        {
                            //MessageBox.Show("" + theMember.MembershipGroupId);
                            decimal totalSpent = theMember.GetTotalSpent();

                            MembershipGroup theMemGroup = theMember.MembershipGroup;

                            decimal total = totalSpent + Convert.ToDecimal(lblTotalAmount.Text);

                            if (theMemGroup.isExceedLimit(total))
                            {
                                if (MessageBox.Show("Previous total spending : " + totalSpent.ToString("N2")
                                    + "\nCurrent Transaction : " + lblTotalAmount.Text
                                    + "\nTotal Spending : " + total.ToString("N2")
                                    + "\n\nCustomer is eligible for membership upgrade. Please proceed to Renew the membership.", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {
                                    pnlMembership.BringToFront();
                                    return;
                                }

                            }
                        }
                    }
                }
                #endregion

                #region *) Initialize Project Module
                if (UseProjectModule)
                {
                    if (lblMembershipNo.Text.Length > 1)
                    {
                        string MembershipNo = this.lblMembershipNo.Text;
                        frmProject _frmProject = new frmProject(MembershipNo);
                        _frmProject.ShowDialog();
                        if (_frmProject.IsSuccessful)
                            pos.myOrderHdr.ProjectName = _frmProject.fsProjectName;
                    }
                }
                #endregion

                #region *) Initialize: Choose Sales Person
                if (!IsLoadedFromAppointment)
                {
                    string assignedMemberStaff = pos.GetAssignedMembershipStaff();
                    UserMst memberUser = new UserMst();

                    if (!string.IsNullOrEmpty(assignedMemberStaff))
                    {
                        memberUser = new UserMst(assignedMemberStaff);
                    }

                    if (PointOfSaleInfo.promptSalesPerson)
                    {
                        frmSalesPerson f = new frmSalesPerson();
                        if (!memberUser.IsNew)
                        {
                            f.AssignedStaff = memberUser.UserName;
                        }

                        f.ShowDialog();
                        if (!f.IsSuccessful)
                        {
                            f.Dispose();
                            return;
                        }
                        f.Dispose();
                    }
                    else
                    {
                        
                        SalesPersonInfo.SalesPersonID = UserInfo.username;
                        SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                        SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
                        
                    }
                }
                else
                {
                    SalesPersonInfo.SalesPersonID = salesPersonFromAppt.UserName;
                    SalesPersonInfo.SalesPersonName = salesPersonFromAppt.DisplayName;
                    SalesPersonInfo.SalesGroupID = salesPersonFromAppt.SalesPersonGroupID;
                }

                //pos.AssignDefaultSalesPerson(SalesPersonInfo.SalesPersonID);
                #endregion

                #region *) Purchase Order
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.AddPurchaseOrderInfo), false))
                {
                    frmInsertPurchaseOrder frmI = new frmInsertPurchaseOrder();
                    frmI.pos = this.pos;
                    frmI.ShowDialog();
                    if (!frmI.IsSuccessful)
                    {
                        frmI.Dispose();
                        return;
                    }
                    frmI.Dispose();
                }
                #endregion

                #region *) DELIVERY: Choose Print Template
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.IsUsingDeliveryOrder), false) && !isOutletSales)
                {
                    frmSelectPrintTemplate FfrmSelectPrintTemplate = new frmSelectPrintTemplate(pos);
                    DialogResult DR = FfrmSelectPrintTemplate.ShowDialog();

                    FfrmSelectPrintTemplate.Dispose();
                    if (DR == DialogResult.Cancel)
                        return;

                    if (DR == DialogResult.OK)
                    {
                        if (pos.IsADelivery)
                        {
                            #region Delivery/Installation/Collection
                            //return item doesn't need delivery
                            if (pos.CalculateTotalAmount(out status) > 0 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.AddDeliveryRemarks), false))
                            {
                                frmTrackDelivery frmT = new frmTrackDelivery();
                                frmT.pos = this.pos;
                                frmT.ShowDialog();
                                if (!frmT.IsSuccessful)
                                {
                                    frmT.Dispose();
                                    return;
                                }
                                frmT.Dispose();
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                #region *) ORDER TYPE: Select Order Type (ex.: Cash&Carry or Preorder)
                DeliveryOrderCollection delOrderHdrColl = new DeliveryOrderCollection();
                DeliveryOrderDetailCollection delOrderDetColl = new DeliveryOrderDetailCollection();

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false) && !isOutletSales)
                {
                    frmSelectOrderType f = new frmSelectOrderType();
                    DialogResult result = f.ShowDialog();
                    string orderType = f.OrderType;
                    f.Dispose();

                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        pos.myOrderHdr.OrderType = orderType.ToUpper();
                        // NOTE: If Pre-Order is selected, the Delivery Setup form will be opened after Payment completion
                    }
                }
                #endregion

                //#region *) FUNDING : Select Funding Method
                //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false))
                //{
                //    frmSelectFunding frm = new frmSelectFunding();
                //    frm.ShowPAMed = pos.HasPAMedifundItem();
                //    frm.ShowSMF = pos.HasSMFItem();
                //    DialogResult result = frm.ShowDialog();
                //    pos.FundingMethod = frm.FundingMethod;
                //    frm.Dispose();

                //    if (result == DialogResult.Cancel)
                //    {
                //        return;
                //    }
                //}
                //#endregion

                #region *) Validation: Line Info is mandatory for OrderType = "PREORDER"
                if (pos.myOrderHdr.OrderType == "PREORDER" && isLineInfoMandatory && (!string.IsNullOrEmpty(lineInfoMandatoryType)))
                {
                    if (lineInfoMandatoryType.ToUpper() == "FOR PRE-ORDER" ||
                        (lineInfoMandatoryType.ToUpper() == "FOR PRE-ORDER WITH FUNDING" && pos.IsFundingSelected()))
                    {
                        foreach (OrderDet od in pos.myOrderDet)
                        {
                            if (string.IsNullOrEmpty(od.LineInfo))
                            {
                                if (string.IsNullOrEmpty(lineInfoCaption)) lineInfoCaption = "Line Info";
                                MessageBox.Show(string.Format("Please assign {0} first.", lineInfoCaption), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                    }
                }
                #endregion

                bool isSuccess = false;
                string change = "";
                string tenderAmt = "";
                PrintReceipt = false;
                //bool isCashPayment = false;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseQuickPayment), false) &&
                    mode != "formal")
                {
                    frmQuickPayment frm = new frmQuickPayment();
                    frm.pos = pos;
                    frm.IsEdit = false;
                    frm.lblAmount.Text = pos.CalculateTotalAmount(out status).ToString();
                    frm.CustomerIsAMember = !string.IsNullOrEmpty(pos.CurrentMember.MembershipNo);
                    change = frm.lblChange.Text;
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    //ask for signature here

                    isSuccess = frm.IsSuccessful;
                    if (isSuccess)
                    {
                        change = frm.lblChange.Text;
                        if (PrintReceipt)
                        {
                            POSDeviceController.PrintAHAVATransactionReceipt(pos, frm.change, false,
                                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        }
                        frm.Dispose();
                    }
                    else
                        pos.DeleteAllReceiptLinePayment();

                    #region *) Signature
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                    {
                        //pop out whether want signature or not
                        frmCustomMessageBox myfrm = new frmCustomMessageBox
                            ("Add Signature", "Do you want to add signature to this transaction?");
                        DialogResult DR = myfrm.ShowDialog();

                        if (myfrm.choice == "yes")
                        {
                            myfrm.Dispose();

                            bool isUsingStandard = false;
                            string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                            if (string.IsNullOrEmpty(signatureDevice))
                                signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                            isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                            if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                            {
                                wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                                if (usbDevices.Count == 0)
                                {
                                    isUsingStandard = true;
                                    MessageBox.Show("There is no STU device attached, using standard signature form");
                                }
                                else
                                {
                                    try
                                    {
                                        wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                        frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                        demo.ShowDialog();
                                        List<wgssSTU.IPenData> penData = demo.getPenData();
                                        if (penData != null)
                                        {
                                            // process penData here!

                                            wgssSTU.IInformation information = demo.getInformation();
                                            wgssSTU.ICapability capability = demo.getCapability();
                                        }
                                        demo.Dispose();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("An error occurred, please contact admin");
                                        Logger.writeLog(ex);
                                    }
                                }
                            }

                            if (isUsingStandard)
                            {
                                //asking for Signature
                                frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                                f.ShowDialog();
                                if (f.IsSuccessful)
                                {
                                    f.Dispose();
                                }
                            }
                        }
                        else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                            myfrm.Dispose();
                    }
                    #endregion
                }
                else
                {
                    if (mode != "formal")
                    {
                        frmSelectPayment frm = new frmSelectPayment();

                        #region *) Core: Select Payment
                        OrderDetCollection odCopy = pos.myOrderDet.Clone(); // Backup the OrderDet
                        frm.pos = pos;
                        frm.amount = pos.CalculateTotalAmount(out status);
                        frm.PrintReceipt = PrintReceipt;
                        frm.syncSalesThread = SyncSalesThread;
                        frm.ParentSyncPointsThread = SyncPointsThread;
                        //frm.cashRecycler = cashRecycler;
                        frm.ShowDialog();
                        #endregion
                        //} 
                        isSuccess = frm.isSuccessful;
                        change = frm.change;
                        tenderAmt = frm.tenderAmt;
                        if (!isSuccess) pos.myOrderDet = odCopy.Clone(); // Revert back the OrderDet
                    }
                    else
                    {
                        frmSelectPaymentFormal frm = new frmSelectPaymentFormal();

                        #region *) Core: Select Payment
                        OrderDetCollection odCopy = pos.myOrderDet.Clone(); // Backup the OrderDet
                        frm.pos = pos;
                        frm.amount = pos.CalculateTotalAmount(out status);
                        frm.PrintReceipt = PrintReceipt;
                        frm.syncSalesThread = SyncSalesThread;
                        frm.ParentSyncPointsThread = SyncPointsThread;
                        frm.ShowDialog();
                        #endregion
                        //} 
                        isSuccess = frm.isSuccessful;
                        change = frm.change;
                        tenderAmt = "0";
                        if (!isSuccess)
                        {
                            pos.myOrderDet = odCopy.Clone();
                            PrintReceipt = true;
                        }// Revert back the OrderDet
                        else
                        {
                            if (!frm.choosePrintResult)
                                PrintReceipt = true;
                        }
                    }


                    if (pos.IsFundingSelected())
                    {
                        BindGrid();
                    }
                }

                if (isSuccess)
                {
                    #region Check if there any cash payment
                    bool NeedToWaitIfDrawerOpened = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.WaitUntilCashDrawerClosed), false);
                    bool haveCashPayment = false;
                    if (NeedToWaitIfDrawerOpened)
                    {
                        foreach (ReceiptDet rd in pos.recDet)
                        {
                            if (rd.PaymentType.ToUpper() == "CASH")
                                haveCashPayment = true;
                        }
                    }
                    #endregion

                    string tmpOrderHdrID = pos.myOrderHdr.OrderHdrID;

                    if (UseCustomInvoiceNo)
                    {
                        CustNumUpdate();//Graham
                    }

                    #region *) Select Print Size
                    string selectedPrintSize = "";
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PromptSelectPrintSize), false))
                    {
                        frmSelectPrintSize frm = new frmSelectPrintSize();
                        DialogResult result = frm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            selectedPrintSize = frm.PrintSize;
                        }
                        frm.Dispose();
                    }
                    #endregion

                    #region *) Open Delivery Setup form if PREORDER
                    //bool isReturnItemsAndLineDiscount = IsReturnItemsAndLineDiscount(pos);
                    if (pos.myOrderHdr.OrderType != null && pos.myOrderHdr.OrderType.ToUpper() == "PREORDER")
                    {
                        // Skip if all quantity is negative (contains return items only)
                        if (!pos.IsReturnedItemsOnly())
                        {
                            bool skipDeliverySetup = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.SkipDeliverySetupScreen), false);
                            if (!skipDeliverySetup)
                            {
                                //pos.SetAsDelivery(true);
                                frmDeliverySetup fDelivery = new frmDeliverySetup();
                                fDelivery.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                                fDelivery.IsUnsavedOrder = true;
                                fDelivery.pos = pos;
                                if (pos.MembershipApplied())
                                {
                                    fDelivery.membershipNo = pos.GetMemberInfo().MembershipNo;
                                }
                                fDelivery.ShowDialog();
                                delOrderHdrColl = fDelivery.delOrderHdrColl;
                                delOrderDetColl = fDelivery.delOrderDetColl;
                                fDelivery.Dispose();
                            }
                            else
                            {
                                // Skip Delivery Setup screen and auto assign deposit amount
                                #region *) Assign Deposit Amount to OrderDet
                                decimal depositBalance = POSController.GetDepositBalance(pos.myOrderHdr.OrderHdrID, out status);

                                QueryCommandCollection cmdColl = new QueryCommandCollection();
                                OrderDetCollection refOD = pos.myOrderDet;
                                decimal remainingAmt = depositBalance;
                                foreach (OrderDet tmpOD in refOD)
                                {
                                    OrderDet od = new OrderDet(tmpOD.OrderDetID);
                                    if (od.IsVoided) continue;
                                    if (od.Amount == null) od.Amount = 0;
                                    if (od.DepositAmount == null) od.DepositAmount = 0;

                                    if (od.Amount > 0 && od.Amount > od.DepositAmount)
                                    {
                                        decimal discrepancy = od.Amount - od.DepositAmount;
                                        if (discrepancy <= remainingAmt)
                                        {
                                            od.DepositAmount += discrepancy;
                                            remainingAmt -= discrepancy;
                                        }
                                        else
                                        {
                                            od.DepositAmount += remainingAmt;
                                            remainingAmt = 0;
                                        }

                                        cmdColl.Add(od.GetUpdateCommand(UserInfo.username));
                                        if (remainingAmt <= 0) break;
                                    }
                                }

                                DataService.ExecuteTransaction(cmdColl);
                                #endregion

                                #region Auto Create Delivery when skip delivery screen
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoCreateWhenSkipDelivery), false))
                                { 
                                    if(!pos.CreateAutoDelivery(out delOrderHdrColl, out delOrderDetColl, out status))
                                    {
                                        throw new Exception(status);
                                    }
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            // Assign and Update OrderDet.DepositAmount automatically
                            QueryCommandCollection cmdColl = new QueryCommandCollection();
                            foreach (OrderDet od in pos.myOrderDet)
                            {
                                OrderDet tmpOD = new OrderDet(od.OrderDetID);
                                tmpOD.DepositAmount = od.Amount;
                                QueryCommand cmd = tmpOD.GetUpdateCommand(UserInfo.username);
                                cmdColl.Add(cmd);
                            }
                            DataService.ExecuteTransaction(cmdColl);
                        }
                    }
                    #endregion

                    #region *) Save Delivery Information (if any)
                    if (delOrderHdrColl != null && delOrderDetColl != null & delOrderHdrColl.Count > 0 && delOrderDetColl.Count > 0)
                    {
                        foreach (DeliveryOrder doHdr in delOrderHdrColl)
                        {
                            doHdr.SalesOrderRefNo = pos.myOrderHdr.OrderHdrID;
                        }

                        DeliveryOrderController.SaveMultipleOrder(delOrderHdrColl, delOrderDetColl);

                         //Update OrderDet.DepositAmount
                        QueryCommandCollection cmdColl = new QueryCommandCollection();
                        foreach (OrderDet od in pos.myOrderDet)
                        {
                            OrderDet tmpOD = new OrderDet(od.OrderDetID);
                            tmpOD.DepositAmount = od.DepositAmount;
                            QueryCommand cmd = tmpOD.GetUpdateCommand(UserInfo.username);
                            cmdColl.Add(cmd);
                        }
                        DataService.ExecuteTransaction(cmdColl);

                        foreach (DeliveryOrder doHdr in delOrderHdrColl)
                        {
                            POSDevices.Receipt rcpt = new POSDevices.Receipt();
                            rcpt.PrintDeliveryOrder(doHdr, delOrderDetColl, pos);
                        }
                    }
                    #endregion

                    #region *) Cancel Delivery if the item is returned
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoCancelDeliveryItemIfReturned), false))
                    {
                        foreach (OrderDet od in pos.myOrderDet)
                        {
                            if (!string.IsNullOrEmpty(od.ReturnedReceiptNo))
                            {
                                // Cancel DeliveryOrderDetails related to this returned item
                                DeliveryOrderController.CancelDeliveryOfReturnedItem(od.ReturnedReceiptNo, od.ItemNo);
                            }
                        }
                    }
                    #endregion

                    #region *) Receipt Printing
                    if (!PrintReceipt && bufPrintReceipt != 0)
                    {
                        decimal decChange = 0;

                        if (!string.IsNullOrEmpty(change))
                            decimal.TryParse(change.Replace("$", ""), out decChange);
                        if (pos.myOrderDet.Count == 1 && pos.myOrderDet[0].ItemNo == "MEMBER" && pos.CalculateTotalAmount(out status) == 0 )
                        {
                        }
                        else
                        {
                            //SICC Validation Printing
                            if (isPrintValidation)
                                CheckValidationAndPrint();

                            if (mode == "formal")
                            {
                                POSDeviceController.PrintFormalAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value, true);
                            }
                            else
                            {
                                POSDeviceController.PrintAHAVATransactionReceipt(pos, decChange, false,
                               (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                               PrintSettingInfo.receiptSetting.PaperSize.Value),
                               PrintSettingInfo.receiptSetting.NumOfCopies.Value,
                               (string.IsNullOrEmpty(selectedPrintSize) || selectedPrintSize == "RECEIPT") ? false : true);
                            }
                        }

                    }
                    #endregion

                    #region *) Email Receipt
                    if (EmailReceipt)
                    {
                        string mailSubject = "";
                        string mailContent = "";
                        string mailBcc = "";
                        string mailTo = "";

                        #region *) Send BCC if necessary
                        bool sendBcc = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
                        string ownerEmail = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
                        if (sendBcc && !string.IsNullOrEmpty(ownerEmail))
                        {
                            mailBcc = ownerEmail;
                        }
                        #endregion
                        string useForReceiptNo = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);
                        string receiptNo = "";
                        if (string.IsNullOrEmpty(useForReceiptNo) || useForReceiptNo.ToLower() == "orderhdrid")
                            receiptNo = pos.myOrderHdr.OrderHdrID;
                        else if (useForReceiptNo.ToLower() == "custom invoice no")
                            receiptNo = pos.myOrderHdr.Userfld5;
                        else if (useForReceiptNo.ToLower() == "line info")
                            receiptNo = pos.GetLineInfoOfFirstLine();
                        else
                            receiptNo = pos.myOrderHdr.OrderHdrID;

                        mailSubject = string.Format("Receipt {0} for purchase at {1}", receiptNo, CompanyInfo.CompanyName);
                        mailContent = "Please find the receipt attachment";
                        mailTo = pos.GetMemberInfo().Email;
                        if (POSDeviceController.SendMailReceipt(pos, mailTo, mailSubject, mailContent, mailBcc))
                            MessageBox.Show("Send Email Success");
                        else
                            MessageBox.Show("Send Email Failed");

                    }
                    #endregion

                    #region *) Display: Show Change Amount (If Any)
                    if (change != null && change != "" && change != "0.00" && change != "$0.00")
                    {
                        if (!NeedToWaitIfDrawerOpened)
                        {
                            lblChange.Text = "Change: " + change;
                            lblTotalAmt.Text = "Total Amount: " + pos.CalculateTotalAmount(out status).ToString("N2");
                            decimal cashReceived = pos.GetCashReceived();
                            if (cashReceived != 0)
                            {
                                lblCashReceived.Visible = true;
                                lblCashReceived.Text = "Cash Received: " + cashReceived.ToString("N2"); 
                            }
                            else {
                                lblCashReceived.Visible = false;
                                lblCashReceived.Text = "0";
                            }
                            pnlShowChange.Visible = true;
                        }
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                        {
                            fOrderSecondScreen.UpdateTenderAmount(tenderAmt);
                            fOrderSecondScreen.UpdateChange(change);
                            fOrderSecondScreen.updateClear(true);
                        }
                    }
                    else
                    {
                        lblChange.Text = "Change: 0";
                        lblTotalAmt.Text = "Total Amount: " + pos.CalculateTotalAmount(out status).ToString("N2");
                        decimal cashReceived = pos.GetCashReceived();
                        if (cashReceived != 0)
                        {
                            lblCashReceived.Visible = true;
                            lblCashReceived.Text = "Cash Received: " + cashReceived.ToString("N2");
                        }
                        else
                        {
                            lblCashReceived.Visible = false;
                            lblCashReceived.Text = "0";
                        }
                        pnlShowChange.Visible = true;

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                        {
                            fOrderSecondScreen.UpdateTenderAmount(pos.CalculateTotalAmount(out status).ToString("N2"));
                            fOrderSecondScreen.updateClear(true);
                        }
                    }
                    #endregion

                    

                    #region *) Navigation: Open Attendance Form
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false)
                        && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.AutoAttendAfterSales), false))
                    {
                        FormController.ShowForm(FormController.FormNames.AttendanceModule, new Attendance.frmAttendance());
                        this.WindowState = FormWindowState.Minimized;
                    }
                    #endregion

                    #region *) External Integration
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false))
                    {
                        //run mall integration
                        MallIntegrationController mc = new MallIntegrationController(tmpOrderHdrID);
                        if (!mc.GenerateFile(out status))
                        {
                            MessageBox.Show("Generate Integration File was not successful. Please recreate the file.");

                        }
                    }
                    #endregion
                    //prompt.
                    /*
                    if (pos.hasPaymentType(POSController.PAY_INSTALLMENT))
                    {
                        frmCreateInstallment f = new frmCreateInstallment();
                        f.pos = pos;
                        f.TotalAmount = pos.GetInstallmentAmount();
                        f.ShowDialog();
                        f.Dispose();
                    }
                    Hashtable eligibility = pos.generateEligibleClasses();
                    if (eligibility.Count > 0)
                    {
                        frmClassAttendance f = new frmClassAttendance();
                        f.eligibility = eligibility;
                        f.checkEligibility = true;
                        f.membershipNo = pos.GetMemberInfo().MembershipNo;
                        f.ShowDialog();
                        f.Dispose();
                    }
                    */
                    /*
                  bool hasWarranty = pos.hasWarrantyItems();
                  bool hasDelivery = pos.hasDeliveryItem();
                  if (hasWarranty || hasDelivery) //change this to has warranty!
                  {
                      //prompt warranty....
                      MDIPowerPOS mdi = new MDIPowerPOS();
                      mdi.pos = pos;
                      mdi.orderHdrId = pos.GetUnsavedRefNo().Substring(2);
                      mdi.membershipNo = pos.GetMemberInfo().MembershipNo;
                      mdi.hasWarranty = hasWarranty;
                      mdi.hasDelivery = hasDelivery;
                      mdi.ShowDialog();
                      mdi.Dispose();
                  }
                  */
                    //Graham Updating the GroupID for a Member
                    //Query qry = Membership.CreateQuery();
                    //qry.QueryType = QueryType.Update;
                    //qry.AddUpdateSetting(Membership.Columns.MembershipGroupId, pos.CurrentMember.MembershipGroupId);
                    //qry.WHERE(Membership.Columns.MembershipNo, pos.CurrentMember.MembershipNo);
                    //qry.Execute();

                    #region *) Try To Sync to server if is enabled
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        if (!SyncSalesThread.IsBusy)
                            SyncSalesThread.RunWorkerAsync();
                    #endregion

                    if (IsLoadedFromAppointment)
                    {
                        isSuccessful = true;
                        OrderHdrID = pos.myOrderHdr.OrderHdrID;
                        this.Close();

                        if (!(fOrderSecondScreen == null || fOrderSecondScreen.IsDisposed))
                        {
                            fOrderSecondScreen.Close();
                            fOrderSecondScreen.Dispose();
                        }
                    }

                    OrderHdrID = pos.myOrderHdr.OrderHdrID;

                    #region Block while the cash drawer still open
                    if (NeedToWaitIfDrawerOpened && haveCashPayment)
                    {
                        //Logger.writeLog("Change : " + change.ToString());
                        frmWaitForDrawer fWaitForDrawer = new frmWaitForDrawer();
                        if (change != null && change != "" && change != "0.00" && change != "$0.00")
                        {
                            fWaitForDrawer.change = change;
                        }
                        else
                        {
                            fWaitForDrawer.change = "";
                        }
                        
                        
                        fWaitForDrawer.ShowDialog();
                        //pnlWaitForDrawer.Visible = true;
                        //Thread.Sleep(100);
                        //this.Enabled = false;
                        //bwCashDrawer.RunWorkerAsync();
                    }
                    #endregion

                    clearControls();

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnablePerformanceLog), false))
                    {
                        PerformanceLogController.AddLog(counterStartTime, DateTime.Now, "POS", "ConfirmOrder",
                            pos.myOrderHdr.PointOfSaleID, OrderHdrID, UserInfo.username);
                    }

                    #region *) Core: Run External Script (for Landlord Integration)


                    if (AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder") != null
                        && AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder") != "")
                    {

                        try
                        {
                            System.Diagnostics.Process.Start(AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder").ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog("Unable start remote process: " + AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder").ToString() + " " + ex.Message);
                        }
                    }
                    #endregion

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Rating.UseRatingSystem), false) && fOrderSecondScreen != null)
                    {
                        fOrderSecondScreen.setVisiblePanelRating(true);
                    }
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            txtBarcode.Focus();
        }

        #region *) Obsoleted CustNumUpdate()
        //private void CustNumUpdate()
        //{
        //    #region customNo Update
        //    int runningNo = 0;

        //    string selectmaxno = "select AppSettingValue from AppSetting where AppSettingKey='CurrentReceiptNo'";
        //    string currentReceiptNo = DataService.ExecuteScalar(new QueryCommand(selectmaxno)).ToString();

        //    int.TryParse(currentReceiptNo, out runningNo);

        //    string updatemaxnum1 = "update appsetting set AppSettingValue='" + ++runningNo + "' where AppSettingKey='CurrentReceiptNo'";
        //    DataService.ExecuteQuery(new QueryCommand(updatemaxnum1));

        //    //default max receiptno is 4
        //    string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='MaxReceiptNo'";
        //    QueryCommand Qcmd2 = new QueryCommand(sql2);
        //    int maxReceiptNo = 4;
        //    int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

        //    //check if it has reached the max no for that digit (99,999,9999.. etc)
        //    bool maximumReached = false;
        //    if (maxReceiptNo != 0)
        //    {
        //        maximumReached = true;
        //        for (int i = 0; i < maxReceiptNo; i++)
        //        {
        //            if (currentReceiptNo[i] != '9')
        //            {
        //                maximumReached = false;
        //                break;
        //            }
        //        }

        //    }

        //    //if it has reached, update the maxreceiptno
        //    if (maximumReached)
        //    {
        //        string sql3 = "update appsetting set AppSettingValue = " + ++maxReceiptNo + " where AppSettingKey='MaxReceiptNo'";
        //        QueryCommand Qcmd3 = new QueryCommand(sql3);
        //        DataService.ExecuteQuery(Qcmd3);

        //        //string sql4 = "update appsetting set AppSettingValue = 0 where AppSettingKey='CurrentReceiptNo'";
        //        //QueryCommand Qcmd4 = new QueryCommand(sql4);
        //        //DataService.ExecuteQuery(Qcmd4);

        //        //currentReceiptNo = "0";
        //    }
        //    #endregion
        //}
        #endregion

        private void CustNumUpdate()
        {
            int runningNo;
            if (!int.TryParse(AppSetting.GetSetting("CurrentReceiptNo"), out runningNo))
            {
                runningNo = 0;
            }
            runningNo += 1;
            AppSetting.SetSetting("CurrentReceiptNo", runningNo.ToString());

            int maxReceiptNo;
            if (!int.TryParse(AppSetting.GetSetting("MaxReceiptNo"), out maxReceiptNo))
            {
                maxReceiptNo = 4;
            }
            if (runningNo.ToString().Length > maxReceiptNo)
            {
                AppSetting.SetSetting("MaxReceiptNo", runningNo.ToString().Length.ToString());
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            if (!GiveDiscountAllowed)
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.UseUserTokenToGiveDiscount), false))
                {
                    //use user token to give discount, if haven't 
                    if (!isUserTokenForDiscountScanned)
                    {
                        DialogResult result = DialogResult.Cancel;
                        //membershipNo = "";
                        try
                        {
                            frmScanStaffCard fScanStaffCard = new frmScanStaffCard();
                            fScanStaffCard.title = "Scan User Token";
                            fScanStaffCard.txtMessage = "Please Scan Staff Token for discount";
                            //fScanStaffCard.Mode = mode;
                            result = fScanStaffCard.ShowDialog();
                            if (result != DialogResult.OK) 
                            {
                                return;
                            }
                            //string role, status, DeptID;
                            UserMst user;
                            user = new UserMst(fScanStaffCard.membershipNo);
                            
                            if (user != null && user.UserName == fScanStaffCard.membershipNo)
                            {
                                UserGroup grp = new UserGroup(user.GroupName);
                                if (PrivilegesController.HasPrivilege(PrivilegesController.GIVE_DISCOUNT, UserController.FetchGroupPrivilegesWithUsername(grp.GroupName, user.UserName)))
                                {
                                    //mySupervisor = fScanStaffCard.membershipNo;
                                    //IsAuthorized = true;
                                    isUserTokenForDiscountScanned = true;
                                    //this.Close();

                                }
                                else
                                {
                                    MessageBox.Show(fScanStaffCard.membershipNo + " has insufficient privileges.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog(ex);
                        }
                    }
                }
                else
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
                    }
                }
            }

            frmDiscountSpecial frm = new frmDiscountSpecial(pos, "", true, EnableMultiTierPriceInGlobalDiscount);
            frm.ShowDialog();
            if (frm.IsSuccessful)
            {
                //PreferedDiscount = frm.DiscApplied; //record prefered discounts
                myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                BindGrid();
            }
        }
        private int TIMER_LIMIT = 4;
        private int timercount, refocuscount;
        private void timer1_Tick(object sender, EventArgs e)
        {

            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            if (refocuscount > 180)
            {
                txtBarcode.Focus();
                refocuscount = 0;
            }
            else
            {
                refocuscount += 1;
            }
            if (timercount > TIMER_LIMIT)
            {
                if (lblTotalAmount.Text != "0" && !pos.HasOrderLine()) //no order line but amount is not zero
                {
                    lblTotalAmount.Text = "0";
                    myDisplay.ClearScreen();
                    myDisplay.SendCommandToDisplay(poleDisplayMessage);
                }
                timercount = 0;
            }
            else
            {
                ++timercount;
            }
        }

        private void btnLogOut_Click_1(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Are you sure you want to log out?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                myDisplay.ClearScreen();
                myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                this.Close();

                if (!(fOrderSecondScreen == null || fOrderSecondScreen.IsDisposed))
                {
                    fOrderSecondScreen.Close();
                    fOrderSecondScreen.Dispose();
                }
            }

        }

        private void btnCatAccess_Click(object sender, EventArgs e)
        {
            string useXMLForHotKey = AppSetting.GetSetting("UseXMLForHotKey");

            if (useXMLForHotKey != null && useXMLForHotKey.ToLower() == "yes")
            {
                frmTouchScreenXML test = new frmTouchScreenXML(pos, PriceMode);
                //test.pos = pos;
                test.ShowDialog();

                //DataTable dt = test.GetResult();
                test.Dispose();
                //pos.AppendOrderDet(test.pos.FetchUnsavedOrderDet());

                pos.FetchUnsavedOrderDet();
                //pos.ApplyMembershipDiscount();

                BindGrid();
                test.Dispose();
            }
            else
            {
                if (AppSetting.GetSettingFromDBAndConfigFile("TwoLevelHotKeys").ToLower() == "yes")
                {
                    frmCatAccess f = new frmCatAccess();
                    f.pos = pos;
                    if (AppSetting.GetSettingFromDBAndConfigFile("UseItemForHotKeys") != null
                        && AppSetting.GetSettingFromDBAndConfigFile("UseItemForHotKeys").ToString().ToLower() == "yes")
                    {
                        f.useItemForHotKeys = true;
                        f.ItemDepartmentID = "";
                    }
                    else
                    {
                        f.useItemForHotKeys = false;
                    }
                    f.PriceMode = PriceMode;
                    f.ShowDialog();
                    BindGrid();
                    myDisplay.ClearScreen();
                    myDisplay.ShowSubTotal("Total", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);

                    f.Dispose();
                }
                else
                {
                    frmGroupAccess f = new frmGroupAccess();
                    f.pos = pos;
                    f.PriceMode = PriceMode;
                    f.ShowDialog();

                    BindGrid();
                    myDisplay.ClearScreen();
                    myDisplay.ShowSubTotal("Total", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);

                    f.Dispose();
                }
            }
            pos.CallOpenPriceItemAdded();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            EditBillForms.frmViewBillDetail f = new WinPowerPOS.EditBillForms.frmViewBillDetail();

            string orderHdrID = ReceiptController.LoadLastTransaction(PointOfSaleInfo.PointOfSaleID, UserInfo.username);

            if (orderHdrID != null)
            {
                f.SyncSalesThread = SyncSalesThread;
                f.OrderHdrID = orderHdrID; //fetch from the database - latest from this location
                f.ShowDialog();
            }
        }

        private void llCashier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (PointOfSaleInfo.allowChangeCashier)
            {
                //prompt login....
                frmPOSLogin f = new frmPOSLogin();
                f.ShowDialog();
                if (f.isClosing == false)
                {
                    llCashier.Text = UserInfo.displayName;
                    pos.SetNewCashier(UserInfo.username);
                }
                f.Dispose();
            }
        }

        private void btnNewMember_Click(object sender, EventArgs e)
        {
            //pnlMembership.BringToFront();
            if (MembershipSignup(true))
            {
                pnlMembership.SendToBack();
            }
        }

        private void btnMembership_Click(object sender, EventArgs e)
        {
            pnlMembership.BringToFront();
            pnlMembership.Refresh();
        }

        private void btnRenewal_Click(object sender, EventArgs e)
        {
            if (MembershipRenewal(true))
            {
                pnlMembership.SendToBack();
            }
        }

        private void btnCancelMembership_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to remove membership?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                PreferedDiscount = 0;
                pos.RemoveMemberFromReceipt(); 
                ResetDiscount();                             
                BindGrid();
                pnlMembership.SendToBack();
            }
        }
        /*
        private void btnUseVoucher_Click(object sender, EventArgs e)
        {
            UseVoucher();
        }
        */


        private void btnCloseMembershipPanel_Click(object sender, EventArgs e)
        {
            pnlMembership.SendToBack();
        }

        private void rbPromoDisable_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
        /*
        private void btnOverallDiscount_Click(object sender, EventArgs e)
        {
            decimal disc;
            frmKeypad f = new frmKeypad();
            f.IsInteger = true;
            f.ShowDialog();
            if (f.value != null && decimal.TryParse(f.value.ToString(), out disc))
            {
                //btnOverallDiscount.Text = "Total Disc: " + disc.ToString("N0") + "%";

                pos.SetOverallDiscount(disc);
                BindGrid();
            }
            else
            {
                //pos.SetOverallDiscount(0.0M);
                //btnOverallDiscount.Text = "No Total Disc";
                BindGrid();
            }
            f.Dispose();
        }*/

        private void btnViewPurchase_Click(object sender, EventArgs e)
        {

            if (lblMembershipNo.Text != "" && lblMembershipNo.Text != "-")
            {
                frmPurchaseSummary f = new frmPurchaseSummary(lblMembershipNo.Text, lblName.Text);
                f.syncSalesThread = SyncSalesThread;
                f.SyncSendDeliveryOrderThread = SyncSendDeliveryOrderThread;
                f.ParentSyncPointsThread = SyncPointsThread;
                f.MainPOS = pos;
                f.ShowDialog();

                if (f.IsUsePackage)
                {
                    clearControlsMembership();
                    pos.RemoveMemberFromReceipt();
                    //BindGrid();
                    pnlMembership.SendToBack();
                }

                f.Dispose();
                BindGrid();
            }
        }

        private void pnlShowChange_Click(object sender, EventArgs e)
        {
            pnlShowChange.Visible = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
            {
                fOrderSecondScreen.UpdateChange("0");
                fOrderSecondScreen.UpdateTenderAmount("0");
                fOrderSecondScreen.ClearControls();
                fOrderSecondScreen.updateClear(false);

            }
        }

        private void lblChange_Click(object sender, EventArgs e)
        {
            pnlShowChange.Visible = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
            {
                fOrderSecondScreen.UpdateChange("0");
                fOrderSecondScreen.UpdateTenderAmount("0");
                fOrderSecondScreen.ClearControls();
                fOrderSecondScreen.updateClear(false);
            }
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {

        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false))
                FormController.ShowForm(FormController.FormNames.AttendanceModule, new Attendance.frmAttendance(SyncAttendanceThread, SyncSalesThread));
        }

        private void btnMembershipRemark_Click(object sender, EventArgs e)
        {
            if (lblMembershipNo.Text != "" && lblMembershipNo.Text != "-")
            {
                frmMembershipEditRemark f = new frmMembershipEditRemark(lblMembershipNo.Text);
                f.ShowDialog();
                addMemberToPOS(lblMembershipNo.Text);


                f.Dispose();
            }
        }

        private void btnLineInfo_Click(object sender, EventArgs e)
        {
            if (this.pos.myOrderDet.Count != 0)
            {
                frmLineInfo frm = new frmLineInfo(this.pos);
                frm.ShowDialog();
                lblLineInfo.Text = pos.GetLineInfoOfFirstLine();
                frm.Dispose();
            }
        }
        private void ShowItemPicture()
        {
            try
            {
                if (IsItemPictureShown)
                {
                    //string ImagePhotosFolder = ConfigurationManager.AppSettings["ItemPhotosFolder"];
                    //if (String.IsNullOrEmpty(ImagePhotosFolder))
                    //{
                    //    return;
                    //}
                    //if (!ImagePhotosFolder[ImagePhotosFolder.Length - 1].ToString().Equals(@"\"))
                    //    ImagePhotosFolder += @"\";

                    //string[] extensions = { "jpg", "png", "bmp", "jpeg" };
                    foreach (DataGridViewRow row in dgvPurchase.Rows)
                    {
                        string itemNo = row.Cells["ItemNo"].Value.ToString();
                        var myItem = new Item(Item.Columns.ItemNo, itemNo);
                        string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                        {
                            string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                            if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                            {
                               
                                foreach (string ext in extensions)
                                {
                                    string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                    if (System.IO.File.Exists(ImagePath))
                                    {
                                        Image img = ResizeImage(Image.FromFile(ImagePath), new Size(40, 40));
                                        row.Cells["Photo"].Value = (Image)img;
                                        row.Cells["Photo"].Tag = ImagePath;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (myItem.ItemImage != null)
                            {
                                row.Cells["Photo"].Value = ResizeImage(Image.FromStream(new MemoryStream(myItem.ItemImage)), new Size(40, 40));
                            }
                        }
                        
                        //string ImagePath = ImagePhotosFolder + row.Cells["ItemNo"].Value.ToString();
                        //bool found = false;
                        //foreach (string ext in extensions)
                        //{
                        //    if (System.IO.File.Exists(ImagePath + "." + ext))
                        //    {
                        //        ImagePath = ImagePath + "." + ext;
                        //        found = true;
                        //        break;
                        //    }

                        //}
                        //if (found)
                        //{
                        //    Image img = ResizeImage(Image.FromFile(ImagePath), new Size(40, 40));
                        //    row.Cells["Photo"].Value = img;
                        //    row.Cells["Photo"].Tag = ImagePath;
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private bool shown = false;
        private void dgvPurchase_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {

            try
            {
                if (IsItemPictureShown)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == dgvPurchase.Columns["Photo"].Index)
                        {
                            //if (dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null) return;

                            if (!shown)
                            {
                                string itemNo = dgvPurchase.Rows[e.RowIndex].Cells["ItemNo"].Value.ToString();
                                var myItem = new Item(Item.Columns.ItemNo, itemNo);
                                Image img = null;

                                string[] extensions = new string[] { "jpg", "gif", "png", "bmp", "jpeg" };

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false))
                                {
                                    string ImagePhotosFolder = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);
                                    if (!string.IsNullOrEmpty(ImagePhotosFolder) && Directory.Exists(ImagePhotosFolder))
                                    {

                                        foreach (string ext in extensions)
                                        {
                                            string ImagePath = ImagePhotosFolder + itemNo.Trim() + "." + ext;
                                            if (System.IO.File.Exists(ImagePath))
                                            {
                                                img = Image.FromFile(ImagePath);
                                            }

                                        }
                                    }
                                }
                                else
                                {
                                    if (myItem.ItemImage != null)
                                    {
                                        img = Image.FromStream(new MemoryStream(myItem.ItemImage));
                                    }
                                }
                                
                                if (img != null)
                                {
                                    PictureBox pictureBox = new PictureBox()
                                    {
                                        Image = img,
                                        Dock = DockStyle.Fill,
                                        SizeMode = PictureBoxSizeMode.Zoom,
                                        BorderStyle = BorderStyle.None
                                    };

                                    Form newForm = new Form()
                                    {
                                        FormBorderStyle = FormBorderStyle.None,
                                        TopMost = true,
                                        ControlBox = false,
                                        //Text=" ",
                                        MaximizeBox = false,
                                        MinimizeBox = false,
                                        ShowInTaskbar = false,
                                        StartPosition = FormStartPosition.CenterParent,
                                        Location = new Point(dgvPurchase.Location.X, dgvPurchase.Location.Y),
                                        //WindowState = FormWindowState.Maximized
                                    };

                                    newForm.Controls.Add(pictureBox);
                                    //newForm.BackColor = Color.White;
                                    newForm.TransparencyKey = SystemColors.Control;
                                    newForm.Left = 100;
                                    newForm.Top = 100;
                                    newForm.Width = this.Width - 200;
                                    newForm.Height = this.Height - 200;
                                    newForm.KeyDown += new KeyEventHandler(newForm_KeyDown);
                                    newForm.Deactivate += new EventHandler(newForm_Deactivate);
                                    newForm.Show();
                                    shown = true;
                                }
                                //newForm.Dispose();
                            }
                        }
                    }
                }
            }
            catch
            { }

        }
        private void newForm_Deactivate(object sender, EventArgs e)
        {
            ((Form)sender).Close();
            shown = false;
        }
        private void newForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                ((Form)sender).Close();
        }
        private void dgvPurchase_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ShowItemPicture();
        }
        private Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }


        public string change;
        public string cashReceived;
        private void btnDefault_Click(object sender, EventArgs e)
        {
            try
            {
                #region *) Initialize: Update OrderTime to NOW
                pos.myOrderHdr.OrderDate = DateTime.Now;
                #endregion

                #region *) Validation: Order cannot be empty
                if (!pos.HasOrderLine())
                {
                    txtBarcode.Select();
                    return;
                }
                #endregion

                #region *) Validation: Check if Qty is sufficient to do stockout [Prompt if Error - Can be terminated by user]
                bool IsQtyInsufficient = false;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.BlockTransactionIfBalQtyNotSuf), false))
                {
                    if (!pos.IsQtySufficientToDoStockOutLocal(out status))
                    {
                        DialogResult dr = MessageBox.Show
                                ("Error: " + status + ". You can not continue the transaction.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {

                    if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.RemoveZeroInventoryValidation), false))
                    {
                        if (!pos.IsQtySufficientToDoStockOutLocal(out status))
                        {

                            DialogResult dr = MessageBox.Show
                                ("Error: " + status + ". Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dr == DialogResult.No)
                            {
                                pos.DeleteAllReceiptLinePayment();
                                return;
                            }
                            IsQtyInsufficient = true;
                        }
                    }
                }

                #endregion

                if (Features.Package.isAvailable && pos.containPackages && !pos.MembershipApplied())
                    throw new Exception("(warning)Please assign membership to continue");
                if (pos.IsItemIsInOrderLine("INS_PAYMENT") != "" && !pos.MembershipApplied())
                    throw new Exception("(warning)Please assign membership to continue");

                #region *) Initialize:Check When Get Point Value and BreakDown Price is 1
                if (dgvPurchase.Rows.Count > 0)
                {
                    DataTable Dt = new DataTable();
                    Decimal GetPointValue = 0, BreakDownPrice = 0;
                    for (Int32 i = 0; i < dgvPurchase.Rows.Count; i++)
                    {
                        /////Get Item No from Grid
                        String ItemNo = dgvPurchase.Rows[i].Cells["ItemNo"].Value.ToString();
                        //// Get Item Name rom grid whose pick from the list
                        String ItemName = dgvPurchase.Rows[i].Cells["ItemName"].Value.ToString();
                        ////Check Function for get time balue and breakdown price return datatable
                        Dt = GetDataBreakDownPrice(ItemNo);
                        /////if data table return greater then 0 row 
                        if (Dt.Rows.Count > 0)
                        {
                            GetPointValue = Convert.ToDecimal(Dt.Rows[0]["userfloat1"].ToString());
                            BreakDownPrice = Convert.ToDecimal(Dt.Rows[0]["userfloat3"].ToString());
                            //// If Get Point value and Break Down Price 1 then show New window for Update Get point value and break down price
                            if (GetPointValue == 1 && BreakDownPrice == 1)
                            {
                                frmBreakPointValue f = new frmBreakPointValue();
                                f.ItemNo = ItemNo;
                                f.ItemName = ItemName;
                                f.ShowDialog();
                                f.Dispose();
                            }
                        }
                    }
                }
                #endregion

                #region *) Initialize: No Membership = [WALK-IN] membership
                if (!pos.MembershipApplied())
                {
                    if (IsMembership_Compulsory)
                    {
                        MessageBox.Show("Please register a member or Scan membership card.", "Membership is Compulsory");
                        return;
                    }
                    if (!pos.AssignMembership("WALK-IN", out status))
                    {
                        Logger.writeLog("Unable to assign walk in member: " + status);
                    }
                }
                #endregion

                #region *) Initialize: Set Pre-Order Information
                if (pos.hasPreOrder())
                {
                    pos.setPreOrderInfo(pos.CurrentMember.NameToAppear, pos.CurrentMember.Mobile, PointOfSaleInfo.PointOfSaleName);

                    //frmPreOrderInquiry f = new frmPreOrderInquiry();
                    //f.pos = pos;
                    //f.ShowDialog();
                    //if (!f.isSuccessful)
                    //{
                    //    txtBarcode.Select();
                    //    return;
                    //}
                }
                #endregion

                #region *) Prompt for Supervisor password if there's returned item
                if (pos.HasReturnedItems())
                {
                    bool IsAuthorized = true;
                    string SupID = "-";
                    bool Prompt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnRefund), false);
                    bool giveRefund = PrivilegesController.HasPrivilege(PrivilegesController.GIVE_REFUND, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username));
                    if (!giveRefund)
                    {
                        string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                        if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                        {

                            frmReadMSR f = new frmReadMSR();
                            f.privilegeName = PrivilegesController.GIVE_REFUND;
                            f.loginType = LoginType.Authorizing;
                            f.ShowDialog();
                            IsAuthorized = f.IsAuthorized;
                            f.Dispose();
                        }
                        else
                        {
                            //Ask for verification....
                            //Prompt Supervisor Password
                            LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                            sl.privilegeName = PrivilegesController.GIVE_REFUND;
                            sl.ShowDialog();
                            SupID = sl.mySupervisor;
                            IsAuthorized = sl.IsAuthorized;
                        }
                    }
                    else
                    {
                        IsAuthorized = true;
                    }

                    if (!IsAuthorized) return;

                    #region Refund Reason
                    bool returnInv = true;
                    string Remarks = "";
                    string selectableRemark = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableVoidReason);
                    if (string.IsNullOrEmpty(selectableRemark))
                    {
                        frmVoidRemark fRemark = new frmVoidRemark();
                        fRemark.pos = pos;
                        fRemark.isRefund = true;
                        CommonUILib.displayTransparent();
                        fRemark.ShowDialog();
                        CommonUILib.hideTransparent();
                        if (fRemark.IsSuccess)
                        {
                            returnInv = fRemark.returnInventory;
                            Remarks = fRemark.pos.myOrderHdr.Remark;
                        }
                        else
                        {
                            fRemark.Dispose();
                            return;
                        }
                        fRemark.Dispose();
                    }
                    else
                    {
                        frmSelectVoidReason fSelecReason = new frmSelectVoidReason();
                        fSelecReason.pos = pos;
                        fSelecReason.isRefund = true;
                        CommonUILib.displayTransparent();
                        fSelecReason.ShowDialog();
                        CommonUILib.hideTransparent();
                        if (fSelecReason.IsSuccess)
                        {
                            Remarks = fSelecReason.Reason;
                            returnInv = fSelecReason.returnInventory;
                        }
                        else
                        {
                            fSelecReason.Dispose();
                            return;
                        }
                        fSelecReason.Dispose();
                    }

                    if (!returnInv)
                    {
                        foreach (OrderDet od in pos.myOrderDet)
                        {
                            if (od.Quantity < 0)
                            {
                                od.InventoryHdrRefNo = "NOTRETURNED";
                            }
                        }
                    }


                    #endregion
                }
                #endregion

                #region *) Core: Handle Negative Amount (TotalAmount of OrderDet <= 0)
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false))
                {
                    if (pos.CalculateTotalAmount(out status) < (decimal)0.01)
                    {
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        HandleNegativeAmount();
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        return;
                    }
                }
                #endregion

                /// TODO: [BUG]
                /// Validation duplicated
                #region -= Duplicated - IGNORE IT =-
                //check amount
                if (!pos.HasOrderLine())
                {
                    //
                    //MessageBox.Show("There is no item in order. Please add product into order.");                    
                    return;
                }

                /*if (pos.CalculateTotalAmount(out status) <= (decimal)0.01)
                {
                    HandleNegativeAmount();
                    return;
                }*/

                //check amount
                //int zeroAmount = 0;
                //bool result = int.TryParse(this.lblTotalAmount.Text.ToString(), out zeroAmount);
                //if (zeroAmount == 0)
                if (this.lblTotalAmount.Text == "0")
                {
                    //
                    //MessageBox.Show("The payment amount is $0. Please add product into order.");
                    //HandleNegativeAmount();
                    txtBarcode.Focus();
                    return;
                }
                #endregion

                /// Comment: Also handled in HandleNegativePayment
                /// 
                #region check if member exceeds limit
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AutoMembershipUpgrade), false))
                {
                    bool isExist = false;
                    string exist = pos.IsItemIsInOrderLine("RENEWAL");
                    if (exist != "")
                        isExist = true;

                    if (pos.MembershipApplied() && !isExist)
                    {
                        Membership theMember = pos.GetMemberInfo();
                        if (theMember != null)
                        {
                            //MessageBox.Show("" + theMember.MembershipGroupId);
                            decimal totalSpent = theMember.GetTotalSpent();

                            MembershipGroup theMemGroup = theMember.MembershipGroup;

                            decimal total = totalSpent + Convert.ToDecimal(lblTotalAmount.Text);

                            if (theMemGroup.isExceedLimit(total))
                            {
                                if (MessageBox.Show("Previous total spending : " + totalSpent.ToString("N2")
                                    + "\nCurrent Transaction : " + lblTotalAmount.Text
                                    + "\nTotal Spending : " + total.ToString("N2")
                                    + "\n\nCustomer is eligible for membership upgrade. Please proceed to Renew the membership.", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {
                                    pnlMembership.BringToFront();
                                    return;
                                }

                            }
                        }
                    }
                }
                #endregion

                #region Initialize Project Module
                if (UseProjectModule)
                {
                    if (lblMembershipNo.Text.Length > 1)
                    {
                        string MembershipNo = this.lblMembershipNo.Text;
                        frmProject _frmProject = new frmProject(MembershipNo);
                        _frmProject.ShowDialog();
                        if (_frmProject.IsSuccessful)
                            pos.myOrderHdr.ProjectName = _frmProject.fsProjectName;
                    }
                }
                #endregion

                #region *) Initialize: Choose Sales Person
                string assignedMemberStaff = pos.GetAssignedMembershipStaff();
                UserMst memberUser = new UserMst();

                if (!string.IsNullOrEmpty(assignedMemberStaff))
                {
                    memberUser = new UserMst(assignedMemberStaff);
                }

                if (PointOfSaleInfo.promptSalesPerson)
                {
                    frmSalesPerson f = new frmSalesPerson();
                    if (!memberUser.IsNew)
                    {
                        f.AssignedStaff = memberUser.UserName;
                    }

                    f.ShowDialog();
                    if (!f.IsSuccessful)
                    {
                        f.Dispose();
                        return;
                    }
                    f.Dispose();
                }
                else
                {
                    
                    SalesPersonInfo.SalesPersonID = UserInfo.username;
                    SalesPersonInfo.SalesPersonName = UserInfo.displayName;
                    SalesPersonInfo.SalesGroupID = UserInfo.SalesPersonGroupID;
                    
                }
                //pos.AssignDefaultSalesPerson(SalesPersonInfo.SalesPersonID);
                #endregion

                #region *) Add Purchase Order Info
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.AddPurchaseOrderInfo), false))
                {
                    frmInsertPurchaseOrder frmI = new frmInsertPurchaseOrder();
                    frmI.pos = this.pos;
                    frmI.ShowDialog();
                    if (!frmI.IsSuccessful)
                    {
                        frmI.Dispose();
                        return;
                    }
                    frmI.Dispose();
                }
                #endregion

                #region *) DELIVERY: Choose Print Template
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.IsUsingDeliveryOrder), false))
                {
                    frmSelectPrintTemplate FfrmSelectPrintTemplate = new frmSelectPrintTemplate(pos);
                    DialogResult DR = FfrmSelectPrintTemplate.ShowDialog();

                    FfrmSelectPrintTemplate.Dispose();
                    if (DR == DialogResult.Cancel)
                        return;

                    if (DR == DialogResult.OK)
                    {
                        if (pos.IsADelivery)
                        {
                            #region Delivery/Installation/Collection
                            //return item doesn't need delivery
                            if (pos.CalculateTotalAmount(out status) > 0 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.AddDeliveryRemarks), false))
                            {
                                frmTrackDelivery frmT = new frmTrackDelivery();
                                frmT.pos = this.pos;
                                frmT.ShowDialog();
                                if (!frmT.IsSuccessful)
                                {
                                    frmT.Dispose();
                                    return;
                                }
                                frmT.Dispose();
                            }
                            #endregion
                        }
                    }
                }
                #endregion

                frmSelectPayment frm = new frmSelectPayment();
                frm.ParentSyncPointsThread = SyncPointsThread;
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.UseDefaultPayment), false))
                {
                    frm.pos = pos;
                    frm.amount = pos.CalculateTotalAmount(out status);
                    string PaymentType = AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.DefaultPaymentType);

                    if (PaymentType.Length > 0)
                    {
                        if (PaymentType == "CASH" && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.DefaultPayment_ShowCashForm), false))
                        {
                            frmPaymentTaking pyment = new frmPaymentTaking();
                            pyment.pos = pos;

                            pyment.lblAmount.Text = pos.RoundTotalReceiptAmount().ToString("N");

                            pyment.ShowDialog();

                            frm.isSuccessful = pyment.isSuccessful;
                            if (frm.isSuccessful)
                            {
                                string sqlQuery1 = "Select max(modifiedon) from membershipPoints where membershipno = '" + pos.GetMemberInfo().MembershipNo + "'";
                                if (pos.MembershipApplied())
                                {
                                    object maxModifiedOn = DataService.ExecuteScalar(new QueryCommand(sqlQuery1));
                                    DateTime testMo = DateTime.Now;
                                    if (DateTime.TryParse(maxModifiedOn.ToString(), out testMo))
                                    {
                                        Logger.writeLog("After Success Confirm Order. " + testMo.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                                    }
                                }
                                //ask for signature here
                                #region Signature
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                                {
                                    //pop out whether want signature or not
                                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                                        ("Add Signature", "Do you want to add signature to this transaction?");
                                    DialogResult DR = myfrm.ShowDialog();

                                    if (myfrm.choice == "yes")
                                    {
                                        myfrm.Dispose();

                                        bool isUsingStandard = false;
                                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                                        if (string.IsNullOrEmpty(signatureDevice))
                                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                                        {
                                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                                            if (usbDevices.Count == 0)
                                            {
                                                isUsingStandard = true;
                                                MessageBox.Show("There is no STU device attached, using standard signature form");
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                                    demo.ShowDialog();
                                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                                    if (penData != null)
                                                    {
                                                        // process penData here!

                                                        wgssSTU.IInformation information = demo.getInformation();
                                                        wgssSTU.ICapability capability = demo.getCapability();
                                                    }
                                                    demo.Dispose();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show("An error occurred, please contact admin");
                                                    Logger.writeLog(ex);
                                                }
                                            }
                                        }

                                        if (isUsingStandard)
                                        {
                                            //asking for Signature
                                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                                            f.ShowDialog();
                                            if (f.IsSuccessful)
                                            {
                                                f.Dispose();
                                            }
                                        }
                                    }
                                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                                        myfrm.Dispose();
                                    /*
                                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                                {
                                    myfrm.Dispose();
                                    return;
                                }
                                     */
                                }
                                #endregion

                                #region *) Try To Sync to server if is enabled
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!SyncSalesThread.IsBusy)
                                        SyncSalesThread.RunWorkerAsync();
                                //Logger.writeLog("run sync finished");
                                #endregion

                                if (PrintSettingInfo.receiptSetting.PrintReceipt)
                                    tryDownloadPoints(pos.GetMemberInfo().MembershipNo, pos.myOrderHdr.OrderHdrID);

                                //SICC Validation Printing
                                //CheckValidationAndPrint();

                                POSDeviceController.PrintAHAVATransactionReceipt(pos, pyment.change, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                change = pyment.lblChange.Text;
                                frm.change = pyment.lblChange.Text;
                                frm.tenderAmt = pyment.tenderAmt.ToString("N2");
                                pyment.Dispose();
                                //this.Close();

                                OrderHdrID = pos.myOrderHdr.OrderHdrID;

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Rating.UseRatingSystem), false) && fOrderSecondScreen != null)
                                {
                                    fOrderSecondScreen.setVisiblePanelRating(true);
                                }
                            }
                        }
                        else
                            if (frm.handlePayment(PaymentType))
                            {

                                if (PaymentType.ToUpper() == "CASH" && pos.GetTotalSales() != 0)
                                {
                                    CashDrawer cd = new CashDrawer();
                                    cd.OpenDrawer();
                                }
                                //ask for signature here
                                #region Signature
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                                {
                                    //pop out whether want signature or not
                                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                                        ("Add Signature", "Do you want to add signature to this transaction?");
                                    DialogResult DR = myfrm.ShowDialog();

                                    if (myfrm.choice == "yes")
                                    {
                                        myfrm.Dispose();

                                        //asking for Signature
                                        frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                                        f.ShowDialog();
                                        if (f.IsSuccessful)
                                        {
                                            f.Dispose();
                                        }
                                    }
                                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                                        myfrm.Dispose();
                                    /*
                                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                                {
                                    myfrm.Dispose();
                                    return;
                                }
                                     */
                                }
                                #endregion

                                #region *) Try To Sync to server if is enabled
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!SyncSalesThread.IsBusy)
                                        SyncSalesThread.RunWorkerAsync();
                                //Logger.writeLog("run sync finished");
                                #endregion

                                if (PrintSettingInfo.receiptSetting.PrintReceipt)
                                    tryDownloadPoints(pos.GetMemberInfo().MembershipNo, pos.myOrderHdr.OrderHdrID);

                                //print receipt
                                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                PrintSettingInfo.receiptSetting.PaperSize.Value),
                                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                frm.isSuccessful = true;
                                //this.Close();
                            }
                    }
                }

                if (frm.isSuccessful)
                {
                    string tmpOrderHdrID = pos.myOrderHdr.OrderHdrID;
                    bool NeedToWaitIfDrawerOpened = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.WaitUntilCashDrawerClosed), false);
                    if (UseCustomInvoiceNo)
                    {
                        CustNumUpdate();//Graham
                    }

                    #region *) Display: Show Change Amount (If Any)
                    //Logger.writeLog("show Change");
                    change = frm.change;
                    if (frm.change != null && frm.change != "" && frm.change != "0.00" && frm.change != "$0.00")
                    {
                        if (!NeedToWaitIfDrawerOpened)
                        {
                            lblChange.Text = "Change: " + frm.change;
                            lblTotalAmt.Text = "Total Amount: " + pos.CalculateTotalAmount(out status).ToString("N2");
                            decimal cashReceived = pos.GetCashReceived();
                            if (cashReceived != 0)
                            {
                                lblCashReceived.Visible = true;
                                lblCashReceived.Text = "Cash Received: " + cashReceived.ToString("N2");
                            }
                            else
                            {
                                lblCashReceived.Visible = false;
                                lblCashReceived.Text = "0";
                            }
                            pnlShowChange.Visible = true;
                        }
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                        {
                            fOrderSecondScreen.UpdateTenderAmount(frm.tenderAmt);
                            fOrderSecondScreen.UpdateChange(frm.change);
                            fOrderSecondScreen.updateClear(true);
                        }
                    }
                    else
                    {
                        lblChange.Text = "Change: " + change;
                        lblTotalAmt.Text = "Total Amount: " + pos.CalculateTotalAmount(out status).ToString("N2");
                        decimal cashReceived = pos.GetCashReceived();
                        if (cashReceived != 0)
                        {
                            lblCashReceived.Visible = true;
                            lblCashReceived.Text = "Cash Received: " + cashReceived.ToString("N2");
                        }
                        else
                        {
                            lblCashReceived.Visible = false;
                            lblCashReceived.Text = "0";
                        }
                        pnlShowChange.Visible = true;

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false))
                        {
                            fOrderSecondScreen.UpdateTenderAmount(frm.tenderAmt);
                            fOrderSecondScreen.updateClear(true);
                        }

                    }
                    //Logger.writeLog("show Change End");
                    #endregion

                    #region *) Navigation: Open Attendance Form
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false)
                        && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.AutoAttendAfterSales), false))
                    {
                        FormController.ShowForm(FormController.FormNames.AttendanceModule, new Attendance.frmAttendance());
                        this.WindowState = FormWindowState.Minimized;
                    }
                    #endregion

                    #region *) External Integration
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false))
                    {
                        //run mall integration
                        MallIntegrationController mc = new MallIntegrationController(tmpOrderHdrID);
                        if (!mc.GenerateFile(out status))
                        {
                            MessageBox.Show("Generate Integration File was not successful. Please recreate the file.");

                        }
                    }
                    #endregion

                    if (IsLoadedFromAppointment)
                    {
                        isSuccessful = true;
                        OrderHdrID = pos.myOrderHdr.OrderHdrID;
                        this.Close();
                    }

                    #region Check if there any cash payment
                    
                    bool haveCashPayment = false;
                    if (NeedToWaitIfDrawerOpened)
                    {
                        foreach (ReceiptDet rd in pos.recDet)
                        {
                            if (rd.PaymentType.ToUpper() == "CASH")
                                haveCashPayment = true;
                        }
                    }
                    #endregion

                    
                    #region Block while the cash drawer still open
                    if (NeedToWaitIfDrawerOpened && haveCashPayment)
                    {
                        //Logger.writeLog("Change : " + change.ToString());
                        frmWaitForDrawer fWaitForDrawer = new frmWaitForDrawer();
                        if (change != null && change != "" && change != "0.00" && change != "$0.00")
                        {
                            fWaitForDrawer.change = change;
                        }
                        else
                        {
                            fWaitForDrawer.change = "";
                        }


                        fWaitForDrawer.ShowDialog();
                        //pnlWaitForDrawer.Visible = true;
                        //Thread.Sleep(100);
                        //this.Enabled = false;
                        //bwCashDrawer.RunWorkerAsync();
                    }
                    #endregion
                    

                    clearControls();
                    //Logger.writeLog("clear control finished");
                    #region *) Core: Run External Script (for Landlord Integration)


                    if (AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder") != null
                        && AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder") != "")
                    {

                        try
                        {
                            System.Diagnostics.Process.Start(AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder").ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog("Unable start remote process: " + AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterConfirmOrder").ToString() + " " + ex.Message);
                        }
                    }
                    #endregion


                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Some error occurred. Please contact your administrator.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            txtBarcode.Focus();
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTotalAmountDollarSign_Click(object sender, EventArgs e)
        {

        }

        private void gbTotalItems_Enter(object sender, EventArgs e)
        {

        }

        private void btnReprintLast_Click(object sender, EventArgs e)
        {
            //Logger.writeLog("Start Reprint: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            string orderHdrID = ReceiptController.LoadLastTransaction(PointOfSaleInfo.PointOfSaleID, UserInfo.username);

            POSController posReprint = new POSController(orderHdrID);
            //if (posReprint.HasOrderedPointPackageItems() || posReprint.hasPaymentType("POINTS"))
            //{
            //    tryDownloadPoints(posReprint.GetMemberInfo().MembershipNo, orderHdrID, true);
            //}
            //else
            //{
            //    tryDownloadPoints(posReprint.GetMemberInfo().MembershipNo, "", true);
            //}
            
            if (posReprint != null && posReprint.GetSavedRefNo() != "")
            {
                //Logger.writeLog("After Download Points: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                POSDeviceController.PrintAHAVATransactionReceipt(posReprint, 0, true,
                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                PrintSettingInfo.receiptSetting.PaperSize.Value),
                PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                //Logger.writeLog("Finish Printing: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
        }

        private bool IsReturnItemsAndLineDiscount(POSController paramPos)
        {
            bool result = false;
            bool isLineDiscount = false;
            bool isAllReturnItems = true;


            OrderDetCollection orderDetailCollection = pos.myOrderDet;
            foreach (OrderDet orderDetail in orderDetailCollection)
            {
                if (orderDetail.ItemNo == "LINE_DISCOUNT")
                {
                    isLineDiscount = true;
                }
                else
                {
                    if (orderDetail.Quantity >= 0)
                    {
                        isAllReturnItems = false;
                    }
                }

            }

            result = isLineDiscount && isAllReturnItems;

            return result;
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            //frmKeyboard keyboard = new frmKeyboard();
            //keyboard.IsInteger = false;
            //keyboard.textMessage = "Search Item / Member";
            //keyboard.ShowDialog();
            //txtBarcode.Text = keyboard.value;

            clickNo++;
            switch (clickNo)
            {
                case 1: OpenWindows8TouchKeyboard(); break;
                case 2: CloseOnscreenKeyboard(); clickNo = 0; break;
            }


        }

        private static void OpenWindows8TouchKeyboard()
        {
            var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
            Process.Start(path);

        }

        private void CloseOnscreenKeyboard()
        {
            //Kill all on screen keyboards
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            if (oskProcessArray.Length > 0)
            {
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (pos.CurrentMember != null && (pos.CurrentMember.MembershipNo + "").ToUpper() != "WALK-IN")
            {
                frmNewMembershipEdit frm = new frmNewMembershipEdit(pos.CurrentMember);
                frm.ShowDialog();
                if (frm.IsUpdated)
                {
                    DisplayMembershipToScreen(frm.EditedMember);
                }
            }
        }

        private void btnQuotations_Click(object sender, EventArgs e)
        {
            try
            {
                string quotationID = txtBarcode.Text;

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncQuotation), false))
                {
                    string serverUrl = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
                    string url = serverUrl + "API/Lookup/GetQuotation.ashx";
                    using (WebClient webClient = new WebClient())
                    {
                        NameValueCollection parameters = new NameValueCollection();
                        parameters.Add("OrderHdrId", quotationID);
                        byte[] responsebytes = webClient.UploadValues(url, "POST", parameters);
                        string jsonDataRaw = Encoding.UTF8.GetString(responsebytes);
                        var jsonData = (JArray)JsonConvert.DeserializeObject(jsonDataRaw);

                        if (jsonData != null)
                        {
                            clearControls();
                            addMemberToPOS(jsonData[0]["MembershipNo"].ToString());
                            pos.SetHeaderRemark(jsonData[0]["Remark"].ToString());
                            pos.SetQuotationHdrID(jsonData[0]["OrderHdrID"].ToString());

                            string OrderHdrQuotationId = jsonData[0]["OrderHdrID"].ToString();
                            string serverUrl1 = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx", "");
                            string url1 = serverUrl1 + "API/Lookup/GetQuotationDet.ashx";
                            using (WebClient webClient1= new WebClient())
                            {
                                NameValueCollection parameters1 = new NameValueCollection();
                                parameters1.Add("OrderHdrId", OrderHdrQuotationId);
                                byte[] responsebytes1 = webClient1.UploadValues(url1, "POST", parameters1);
                                string jsonDataRaw1 = Encoding.UTF8.GetString(responsebytes1);
                                var jsonDataDet = (JArray)JsonConvert.DeserializeObject(jsonDataRaw1);
                                if (jsonDataDet != null)
                                {
                                    foreach (var item in jsonDataDet)
                                    {
                                        Item myItem = new Item(item["ItemNo"].ToString());
                                        if (myItem.ItemNo != POSController.ROUNDING_ITEM)
                                        {
                                            if (!pos.AddItemToOrderWithPriceMode(myItem, item["Quantity"].ToString().GetIntValue(), (decimal)item["Discount"], ApplyPromo, this.PriceMode, out status))
                                            {
                                                MessageBox.Show("Error encountered while adding items. Please try again. " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                txtBarcode.Focus();
                                                return;
                                            }
                                            else
                                            {
                                                myDisplay.ClearScreen();
                                                myDisplay.ShowItemPrice(
                                                    myItem.ItemName,
                                                    (double)myItem.RetailPrice,
                                                    (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);


                                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                                                {
                                                    if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, item["Quantity"].ToString().GetIntValue()))
                                                    {
                                                        MessageBox.Show("This item is only left with " + ItemController.GetStockOnHand(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                                    }
                                                }

                                                txtBarcode.Focus();
                                            }
                                        }

                                        BindGrid();
                                        updateTotalAmount();
                                    }
                                }

                            }

                           
                            
                        }
                        else
                        {
                            MessageBox.Show("Quotation Not Found. ");
                            return;
                        }


                        //foreach (var item in jsonData)
                        //{
                        //    totalMembership = (int)(item["TotalMembership"] ?? "0");
                        //}
                    }


                }
                else
                {
                    QuotationHdrCollection qh = new QuotationHdrCollection();
                    qh.Where(QuotationHdr.Columns.OrderHdrID, txtBarcode.Text);
                    qh.Load();
                    if (qh.Count < 1)
                    {
                        QuotationHdrCollection qhc = new QuotationHdrCollection();
                        qhc.Where(QuotationHdr.Columns.Userfld5, txtBarcode.Text);
                        qhc.Load();
                        if (qhc.Count > 0)
                        {
                            quotationID = qhc[0].OrderHdrID;
                        }
                    }
                    QuoteController qos = new QuoteController(quotationID);



                    if (qos != null && qos.myOrderHdr.OrderHdrID != null && qos.myOrderHdr.OrderHdrID != "")
                    {
                        clearControls();
                        addMemberToPOS(qos.GetMemberInfo().MembershipNo);
                        pos.SetHeaderRemark(qos.GetHeaderRemark());
                        pos.SetQuotationHdrID(qos.myOrderHdr.OrderHdrID);
                        foreach (QuotationDet q in qos.myOrderDet)
                        {
                            Item myItem = new Item(q.ItemNo);
                            if (myItem.ItemNo != POSController.ROUNDING_ITEM)
                            {
                                if (!pos.AddItemToOrderWithPriceMode(myItem, q.Quantity.GetIntValue(), (decimal)q.Discount, ApplyPromo, this.PriceMode, out status))
                                {
                                    MessageBox.Show("Error encountered while adding items. Please try again. " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    txtBarcode.Focus();
                                    return;
                                }
                                else
                                {
                                    myDisplay.ClearScreen();
                                    myDisplay.ShowItemPrice(
                                        myItem.ItemName,
                                        (double)myItem.RetailPrice,
                                        (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);


                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                                    {
                                        if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, q.Quantity))
                                        {
                                            MessageBox.Show("This item is only left with " + ItemController.GetStockOnHand(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                        }
                                    }

                                    txtBarcode.Focus();
                                }
                            }

                            BindGrid();
                            updateTotalAmount();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Quotation Not Found. ");
                        return;
                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error. " + ex.Message);
                Logger.writeLog(ex.Message);
            }
        }
        /*
        private void btnPayInstallment_Click(object sender, EventArgs e)
        {
            if (!pos.MembershipApplied())
            {
                MessageBox.Show("Please scan membership first");
                txtBarcode.Focus();
                return;
            }
            
            frmPayInstallment f = new frmPayInstallment();
            f.membershipNo = pos.GetMemberInfo().MembershipNo;
            
            f.ShowDialog();

            decimal amount = f.amount;
            string refno = f.InstallmentDetID;
            f.Dispose();
            if (refno != null && refno != "")
            {
                //check if ref no is already been specified....
                if (pos.InstallmentPaymentHasAlreadyBeenSpecified(refno))
                {
                    MessageBox.Show("Installment payment for this  has already been specified.");
                    return;
                }
                else
                {
                    //Add Installment amount which capture the amount and installmentdetrefno........
                    if (!pos.AddInstallmentItemToOrder(amount, 0, refno, false, out status))
                    {
                        MessageBox.Show("Error taking installment payment: " + status);
                        Logger.writeLog(status);
                        return;
                    }
                }
            }
            BindGrid();
        }
        */
        /*
        private void btnDeposit_Click(object sender, EventArgs e)
        {
            if (pos.CurrentMember == null)
            {
                MessageBox.Show("Please scan membership first");
                txtBarcode.Focus();
                return;
            }
            frmMembershipTap f = new frmMembershipTap();
            f.pos = pos;
            f.ShowDialog();
            if (f.isSuccessful)
            {
                //Add item to order
                if (!pos.AddServiceItemToOrder(new ViewItem(ViewItem.Columns.ItemNo, 
                    POSController.DEPOSIT_ITEM), 1, f.value, 0, true, f.remark, out status))
                {
                    MessageBox.Show("Error in processing deposit: " + status);
                    Logger.writeLog(status);
                    f.Dispose();
                    txtBarcode.Focus();
                    return;                 
                }
                
                BindGrid();
            }
            else
            {
                //Prompt error message
                MessageBox.Show("Error in processing deposit.");
                f.Dispose();
                txtBarcode.Focus();
                return;
            }
        }       
        */

        delegate void LaunchDialogWaitConfirmSecondaryDelegate();

        public void LaunchDialogWaitConfirmSecondary()
        {
            if (fOrderSecondScreen != null)
            {
                if (fOrderSecondScreen.InvokeRequired)
                {
                    LaunchDialogWaitConfirmSecondaryDelegate d = new LaunchDialogWaitConfirmSecondaryDelegate(LaunchDialogWaitConfirmSecondary);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    frmWaitConfirmSecondary frmWaitConfirmSecondary = new frmWaitConfirmSecondary();
                    //frmWaitConfirmSecondary.
                    if (fOrderSecondScreen.WaitConfirmSecondaryDialog())
                    {
                        //frmWaitConfirmSecondary.Hide();

                        //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.AskPrintReceipt), false))
                        //{
                        //    Thread t = new Thread(new ThreadStart(LaunchDialogAskPrintReceipt));
                        //    t.Start();
                        //}
                        frmWaitConfirmSecondary.Hide();
                        doConfirm();
                    }
                    else
                    {
                        frmWaitConfirmSecondary.Hide();
                        MessageBox.Show(this, "Customer did not confirm the amount.", "", MessageBoxButtons.OK);
                        //frmWaitConfirmSecondary.Hide();
                        return;
                    }
                }
            }
        }

        delegate void LaunchDialogAskPrintReceiptDelegate();

        public void LaunchDialogAskPrintReceipt()
        {
            
            if (frmPrintReceipt != null)
            {
                if (fOrderSecondScreen.InvokeRequired)
                {
                    LaunchDialogAskPrintReceiptDelegate d = new LaunchDialogAskPrintReceiptDelegate(LaunchDialogAskPrintReceipt);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    try
                    {
                        frmPrintReceipt.Location = Screen.AllScreens[1].WorkingArea.Location;
                    }
                    catch
                    {
                        frmPrintReceipt.Location = Screen.AllScreens[0].WorkingArea.Location;
                    }
                    frmPrintReceipt.StartPosition = FormStartPosition.CenterScreen;
                    frmPrintReceipt.Show(fOrderSecondScreen);
                }
            }
        }

        private void dgvPurchase_Resize(object sender, EventArgs e)
        {
            
        }

        private void dgvPurchase_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            AppSetting.SetSetting(e.Column.Name + "_Width", e.Column.Width.ToString());
        }

        private void btnViewAppointments_Click(object sender, EventArgs e)
        {
            frmAddNewItemWeb frm = new frmAddNewItemWeb();
            frm.Show();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (pos.myOrderDet.Count > 0)
            {
                frmRemoveItem fRemoveItem = new frmRemoveItem();
                fRemoveItem.pos = pos;
                fRemoveItem.ShowDialog();
                if (fRemoveItem.IsSuccessful)
                {
                    BindGrid();
                }
            }

        }

        #region For Printing Validation
        public void CheckValidationAndPrint()
        {
            //only if item with isCommission true then we will print validation
            bool isComm = false;
            OrderDetCollection det = pos.FetchUnsavedOrderDet();
            for (int i = 0; i < det.Count; i++)
            {
                isComm = Convert.ToBoolean(det[i].Item.IsCommission);

                //found
                if (isComm == true)
                    break;
            }

            //keep asking to print validation to continue, else cannot continue
            bool valid = false;
            if (isComm == true)
            {
                //while (!valid)
                //{
                //ask if user wants to print validation
                //DialogResult dr = MessageBox.Show("Do you want to print validation?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                //if (dr == DialogResult.Yes) //print validation
                //{
                //print validation
                POSDeviceController.PrintAHAVAValidationReceipt(pos, 0, false,
                    ReceiptSizes.A4, //A4 Size validation
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                valid = true;
                //}
                //else
                //{
                //    valid = false;
                //MessageBox.Show("Please print validation to proceed!");
                //}
                //}
            }
        }
        #endregion 

        #region Change GST on Sales

        private void ChangeGSTTransaction()
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.AllowChangeGSTonSales), false))
            {
                frmChangeGST f = new frmChangeGST();

                f.ShowDialog();
                int selectedgst = f.selectedGST;
                f.Dispose();

                if (selectedgst != 0)
                {
                    pos.SetHeaderGSTRule(selectedgst);

                    foreach (var det in pos.myOrderDet)
                    {
                        OrderDet myDet = det;
                        pos.CalculateLineAmount(ref myDet);
                    }

                    BindGrid();
                    updateTotalAmount();
                }

            }
        }

        #endregion

        private void lblGST_Click(object sender, EventArgs e)
        {
            ChangeGSTTransaction();
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            ChangeGSTTransaction();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            ChangeGSTTransaction();
        }

        private void bwCashDrawer_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isDrawerOpened = true;
            while (isDrawerOpened)
            {
                FlyTechCashDrawer c = new FlyTechCashDrawer();
                isDrawerOpened = c.isCashDrawerOpen();
            }
        }

        private void bwCashDrawer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.Enabled = true;
            //pnlWaitForDrawer.Visible = false;
            //clearControls();
        }

        private void pnlWaitForDrawer_Paint(object sender, PaintEventArgs e)
        {
            if (Enabled)
            {
                //use normal realization
                base.OnPaint(e);
                return;
            }
            //custom drawing

        }

        private void SyncPointsThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Features.Package.isAvailable)
                {
                    //Download 

                    string isRealTime = ConfigurationManager.AppSettings["RealTimePointSystem"];
                    if (isRealTime == "yes" || isRealTime == "true")
                    {
                        PowerPOS.SyncPointsController.SyncPointsRealTimeController snc = new PowerPOS.SyncPointsController.SyncPointsRealTimeController();
                        if (!snc.SyncPoints(lblMembershipNo.Text))
                        {
                            e.Result = false;
                        }
                        else
                        {
                            e.Result = true;
                        }
                    }
                    else
                    {
                        e.Result = true;
                    }
                }
                else
                {
                    e.Result = true;
                }
            }
            catch (Exception ex)
            {
                e.Result = false;
                //throw new InvalidOperationException("Error Downloading Points." + ex.Message);
            }
        }

        private void SyncPointsThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.IsDisposed) return;

                bool res = false;
                if (!bool.TryParse(e.Result.ToString(), out res))
                    res = false;
                if (!res)
                    MessageBox.Show("Download Points Data Error. Please check your connection");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public string PriceMode
        {
            get
            {
                return btnPriceMode.Tag == null ? "NORMAL" : btnPriceMode.Tag.ToString();                
            }
        }

        private void btnPriceMode_Click(object sender, EventArgs e)
        {
            DataTable ListPM = GetListPriceMode();
            int index = 0;

            for (int i = 0; i < ListPM.Rows.Count; i++)
            {
                if (ListPM.Rows[i]["DiscountName"].ToString() == PriceMode)
                {
                    index = i;
                    break;
                }
            }

            if (index == ListPM.Rows.Count - 1)
                index = 0;
            else
                index += 1;

            btnPriceMode.Text = ListPM.Rows[index]["DiscountLabel"].ToString();
            btnPriceMode.Tag = ListPM.Rows[index]["DiscountName"].ToString();
        }

        private DataTable GetListPriceMode()
        {
            string query = @"select *
                            from(
                            select 'NORMAL' as DiscountName, 'NORMAL' as DiscountLabel, 0 as PriorityLevel
                            UNION
                            SELECT  DiscountName, 
		                            case when showLabel = 1 and ISNULL(DiscountLabel,'') != '' then DiscountLabel else DiscountName END as DiscountLabel,
		                            PriorityLevel
                            FROM	SpecialDiscounts SD
                            WHERE	SD.Deleted = 0
		                            AND SD.Enabled = 1
		                            AND SD.StartDate <= GETDATE()
		                            AND SD.EndDate >= DATEADD(DAY,-1,GETDATE())
		                            AND SD.isBankPromo = 0
		                            AND SD.DiscountName IN ('P1','P2','P3','P4','P5')			
                            )X
                            ORDER BY X.PriorityLevel";
            query = string.Format(query, 1);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(query)));

            return dt;
        }

        private void dgvPurchase_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && dgvPurchase.Columns[e.ColumnIndex].Name == "ItemName")
            {
                if (!e.Handled)
                {
                    e.Handled = true;
                    e.PaintBackground(e.CellBounds, dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected);
                }
                if ((e.PaintParts & DataGridViewPaintParts.ContentForeground) != DataGridViewPaintParts.None)
                {

                    string text = e.Value.ToString();
                    int index = text.IndexOf(Environment.NewLine);

                    if (index > 0)
                    {
                        string itemname = text.Substring(0, index);
                        string promo = text.Substring(index, text.Length - index - 1);

                        Size fullsize = TextRenderer.MeasureText(text, e.CellStyle.Font);
                        Size size1 = TextRenderer.MeasureText(itemname, e.CellStyle.Font);
                        Size size2 = TextRenderer.MeasureText(promo, e.CellStyle.Font);

                        Bitmap b = new Bitmap(e.CellBounds.Width, e.CellBounds.Height);
                        Graphics g = Graphics.FromImage(b);

                        Font fregular = new Font(e.CellStyle.Font, FontStyle.Regular);

                        SizeF sizeOfString = new SizeF();
                        sizeOfString = g.MeasureString(itemname, fregular, e.CellBounds.Width);

                        SizeF sizeOfPromo = new SizeF();
                        sizeOfPromo = g.MeasureString(promo, fregular, e.CellBounds.Width);

                        Size s = e.CellBounds.Size;
                        s.Height = (int)sizeOfString.Height;
                        Rectangle rect1 = new Rectangle(e.CellBounds.Location, s);

                        if (dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)
                            e.Graphics.DrawString(itemname, e.CellStyle.Font, Brushes.White, rect1);
                        else
                            e.Graphics.DrawString(itemname, e.CellStyle.Font, Brushes.Black, rect1);

                        Size s2 = e.CellBounds.Size;
                        s2.Height = (int)sizeOfPromo.Height;
                        Rectangle rect2 = new Rectangle(e.CellBounds.Location, s2);
                        int height = (int)Math.Round(sizeOfString.Height);
                        rect2.Y += (int)sizeOfString.Height;

                        Font f = new Font(e.CellStyle.Font, FontStyle.Bold);

                        e.Graphics.DrawString(promo, f, Brushes.Red, rect2);
                    }
                    else
                    {
                        Size fullsize = TextRenderer.MeasureText(text, e.CellStyle.Font);
                        Rectangle rect1 = new Rectangle(e.CellBounds.Location, e.CellBounds.Size);
                        rect1.Y += 5;

                        if (dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected)
                            e.Graphics.DrawString(text, e.CellStyle.Font, Brushes.White, rect1);
                        else
                            e.Graphics.DrawString(text, e.CellStyle.Font, Brushes.Black, rect1);

                        //dgvPurchase.Rows[e.RowIndex].Height = rect1.Height + 7;
                    }
                }
            }
        }

        private void btnNoSale_Click(object sender, EventArgs e)
        {
            bool IsAuthorized = false;
            string SupID = "";
            bool giveRefund = PrivilegesController.HasPrivilege(PrivilegesController.NO_SALES, UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username));
            if (!giveRefund)
            {
                string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                {

                    frmReadMSR f = new frmReadMSR();
                    f.privilegeName = PrivilegesController.NO_SALES;
                    f.loginType = LoginType.Authorizing;
                    f.ShowDialog();
                    IsAuthorized = f.IsAuthorized;
                    f.Dispose();
                }
                else
                {
                    //Ask for verification....
                    //Prompt Supervisor Password
                    LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                    sl.privilegeName = PrivilegesController.NO_SALES;
                    sl.ShowDialog();
                    SupID = sl.mySupervisor;
                    IsAuthorized = sl.IsAuthorized;
                }
            }
            else
            {
                IsAuthorized = true;
            }

            if (!IsAuthorized) return;
            
            //kick  
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseCustomKickDrawer), false))
            {
                #region *) Core: Run External Script (for Landlord Integration)
                if (AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != null
                    && AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != "")
                {
                    try
                    {
                        System.Diagnostics.Process.Start(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString());
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Unable start remote process: " + AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString() + " " + ex.Message);
                    }
                }
                #endregion
            }
            else
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseFlyTechCashDrawer), false))
                {
                    FlyTechCashDrawer cdrw = new FlyTechCashDrawer();
                    cdrw.OpenDrawer(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.KickDrawerPort));
                }
                else
                {
                    CashDrawer drw = new CashDrawer();
                    drw.OpenDrawer();
                }
            }
            UserMst supervisor = new UserMst(SupID);
            AccessLogController.AddLogHelper(PointOfSaleInfo.PointOfSaleID, PowerPOS.AccessSource.POS, UserInfo.username, SupID, "No Sale", "");
            
        }

        private void btnCashOut_Click(object sender, EventArgs e)
        {
            frmCashRecording FrmCashRecording = new frmCashRecording();
            FrmCashRecording.SyncCashRecordingThread = SyncCashRecordingThread;
            FrmCashRecording.defaultType = 1;
            FrmCashRecording.ShowDialog();
            FrmCashRecording.Dispose();
        }

        private void btnCashIn_Click(object sender, EventArgs e)
        {
            frmCashRecording FrmCashRecording = new frmCashRecording();
            FrmCashRecording.SyncCashRecordingThread = SyncCashRecordingThread;
            FrmCashRecording.defaultType = 2;
            FrmCashRecording.ShowDialog();
            FrmCashRecording.Dispose();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            previewCurrentPOS();
        }

        private void previewCurrentPOS()
        {
            try
            {
                frmViewInvoiceReceipt f = new frmViewInvoiceReceipt();
                f.posCtrl = pos;
                f.ShowDialog();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public bool AddAttendanceItemToOrder(string membershipNo, string itemNo, DateTime startTime, DateTime endTime)
        {
            if (string.IsNullOrEmpty(membershipNo))
            {
                MessageBox.Show("Error Loading Membership because MembershipNo is empty.");
                Logger.writeLog("Error Loading Membership because MembershipNo is empty.");
                return false;
            }

            if (string.IsNullOrEmpty(itemNo))
            {
                MessageBox.Show("Error Loading Item because ItemNo is empty.");
                Logger.writeLog("Error Loading Item because ItemNo is empty.");
                return false;
            }

            ViewItem attItem = new ViewItem(ViewItem.Columns.ItemNo, itemNo);
            if (attItem == null || attItem.ItemNo != itemNo)
            {
                MessageBox.Show("Error Loading Item. ItemNo is not found: " + itemNo);
                Logger.writeLog("Error Loading Item. ItemNo is not found: " + itemNo);
                return false;
            }

            int duration = Convert.ToInt16(Math.Ceiling(endTime.Subtract(startTime).TotalSeconds / 60));
            // round up the duration
            int roundUp = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.MinutesRoundingUp), 1);
            duration = (Math.Ceiling((decimal)duration / roundUp) * roundUp).GetIntValue();

            string remark = string.Format("From {0} To {1}", startTime.ToString("HH:mm:ss"), endTime.ToString("HH:mm:ss"));

            bool hasExpired;
            DateTime ExpiryDate;
            if (MembershipController.IsExistingMember(membershipNo, out hasExpired, out ExpiryDate))
            {
                //// Remove existing Attendance item first
                //int attIndex = pos.myOrderDet.Find("ItemNo", itemNo);
                //if (attIndex >= 0) pos.RemoveLine(pos.myOrderDet[attIndex]);

                pos.AddServiceItemToOrder(attItem, 1, (decimal)duration, 0, ApplyPromo, remark, out status);
                addMemberToPOS(membershipNo);
                return true;
            }
            else
            {
                if (hasExpired)
                {
                    //// Remove existing Attendance item first
                    //int attIndex = pos.myOrderDet.Find("ItemNo", itemNo);
                    //if (attIndex >= 0) pos.RemoveLine(pos.myOrderDet[attIndex]);

                    pos.AddServiceItemToOrder(attItem, 1, (decimal)duration, 0, ApplyPromo, remark, out status);
                    addExpiredMemberToPOS(membershipNo);
                    return true;
                }
                else
                {
                    MessageBox.Show("Error Loading Membership. MembershipNo is not found.");
                    Logger.writeLog("Error Loading Membership. MembershipNo is not found.");
                    return false;
                }
            }
        }
    }
}
