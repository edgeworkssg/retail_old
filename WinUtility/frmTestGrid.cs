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
using WinPowerPOS.OrderForms;
using System.Reflection;

namespace WinUtility
{
    public partial class frmTestGrid : Form
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
        public string PriceMode
        {
            get
            {
                return "NORMAL";
            }
        }

        bool isPrintValidation = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SICC.CheckValidationPrinting), false);

        public static DateTime counterStartTime;

        public bool isUserTokenForDiscountScanned = false;

        public frmTestGrid()
        {
            frmOrderTaking_Init(Guid.Empty);
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

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.DisablePoleDisplay), false))
            {
                myDisplay.setPrinterName("");
            }

            pos = new POSController();
            pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
            pos.AutoCaptureWeightItemAdded += new AutoCaptureWeightItemHandler(pos_AutoCaptureWeightItemAdded);
            posHold = new POSController[3];
            AssignPrivileges();
            UseCustomInvoiceNo = false;
            if (AppSetting.GetSetting("UseCustomInvoiceNo") == "True")
            {
                UseCustomInvoiceNo = true;
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

            lineInfoCaption = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith));

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
            dgvPurchase.Columns[TAX_COL].Visible = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideTaxColumn), false);
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

            dgvPurchase.AutoGenerateColumns = false;

            myDisplay.ClearScreen();
            myDisplay.SendCommandToDisplay(poleDisplayMessage);

            SyncClientController.Load_WS_URL();
        }

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
        }

        void pos_OpenPriceItemAdded(object sender, string orderDetID)
        {

            //Open Price item
            //if (theItem.IsServiceItem.GetValueOrDefault(false))
            //{
            //    //MessageBox.Show("Service item");
            //    string lineID = pos.GetLineIDOfItemNo(theItem.ItemNo);

            if (!string.IsNullOrEmpty(orderDetID))
            {
                WinPowerPOS.OrderForms.frmOrderLineKeypad t = new WinPowerPOS.OrderForms.frmOrderLineKeypad();
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
            }
            //}
        }

        void pos_OpenPriceItemAddedEditField(object sender, string orderDetID, bool IsUsingQuantityField)
        {

            if (!string.IsNullOrEmpty(orderDetID))
            {
                WinPowerPOS.OrderForms.frmOrderLineKeypad t = new WinPowerPOS.OrderForms.frmOrderLineKeypad();
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
            }
        }

        private void BindGrid()
        {
            try
            {
                string status = "";

                if (pos == null) return;

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
                string pricelevel = pos.GetMemberInfo().MembershipGroup.Userfld1;
                if (!String.IsNullOrEmpty(pricelevel) && pricelevel != "No Link")
                {
                    SpecialDiscount sd = new SpecialDiscount(pricelevel);
                    frmDiscounts frd = new frmDiscounts();
                    try
                    {
                        pos.clearDiscount(0);
                        pos.applyDiscount(pricelevel);
                    }
                    catch (Exception ex) { Logger.writeLog(ex.Message); }
                }

                BindGrid();
                Membership member = pos.GetMemberInfo();

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

        private void BarcodeEnter()
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
                                    (int)itG[i].UnitQty * qtyBar, pos.GetPreferredDiscount(),
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
                                        itGM[j].UnitQty * qtyBar, pos.GetPreferredDiscount(),
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
                            true, this.PriceMode, out status))
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


                pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
                pos.OpenPriceItemAddedEditField += new OpenPriceItemEditFieldHandler(pos_OpenPriceItemAddedEditField);
                pos.AutoCaptureWeightItemAdded += new AutoCaptureWeightItemHandler(pos_AutoCaptureWeightItemAdded);

                string useCustomInvoiceNo = AppSetting.GetSetting("UseCustomInvoiceNo");
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

                PreferedDiscount = 0;
                /*Clear History Grid
                for (int j = dgvPastTransaction.Rows.Count-1; j >= 0; j--)
                {
                    dgvPastTransaction.Rows.RemoveAt(j);
                }*/
                myDisplay.ClearScreen();
                myDisplay.SendCommandToDisplay(poleDisplayMessage);

                //Adi 20170710 - Set to not scanned for user token discount
                isUserTokenForDiscountScanned = false;


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
                    MessageBox.Show("Error: Please input correct Quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    if (!pos.AddSpecialItemToOrder(theItem, quantity, price, PreferedDiscount, ApplyPromo, "", recordedDigit, out status))
                                    {
                                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtBarcode.Focus();
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                frmErrorMessage frm = new frmErrorMessage();
                                frm.lblMessage = "Error: Barcode " + tmpBarcode + " does not exist";
                                frm.ShowDialog();
                                //MessageBox.Show(, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            frmErrorMessage frm = new frmErrorMessage();
                            frm.lblMessage = "Error: Barcode " + tmpBarcode + " does not exist";
                            frm.ShowDialog();
                            //MessageBox.Show("Error: Barcode " + txtBarcode.Text + " does not exist in the system.", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
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

        private void dgvPurchase_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex > -1 && (e.ColumnIndex == 2 || e.ColumnIndex == 3))
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

        private void dgvPurchase_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

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

        private void dgvPurchase_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dgvPurchase.Rows[e.RowIndex].Cells[0].Value = (((DataTable)dgvPurchase.DataSource).Rows.Count - e.RowIndex).ToString();
        }

        private void txtBarcode_Leave(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (string.IsNullOrEmpty(txtBarcode.Text)) return;

                DateTime startExec = DateTime.Now;
                string itemNo = txtBarcode.Text;

                BarcodeEnter();

                AddLog(DateTime.Now - startExec, itemNo);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBarcode.Text)) return;

            DateTime startExec = DateTime.Now;
            string itemNo = txtBarcode.Text;

            BarcodeEnter();

            AddLog(DateTime.Now - startExec, itemNo);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearControls();
        }

        private void AddLog(TimeSpan time, string itemNo)
        {
            if (string.IsNullOrEmpty(itemNo)) return;
            string txt = string.Format(">{0} ADD ITEM {1} - {2} Lines\t: {3}", DateTime.Now.ToString("HH:mm:ss.fff"), itemNo, pos.myOrderDet.Count, time);
            txtLog.Text = txt + Environment.NewLine + txtLog.Text;
        }

        private void AddLogDet(TimeSpan time, string section, string itemNo)
        {
            string txt = string.Format(">{0} {1}{2}\t: {3}", DateTime.Now.ToString("HH:mm:ss.fff"), section, string.IsNullOrEmpty(itemNo) ? "" : " - " + itemNo, time);
            txtLogDetail.Text = txt + Environment.NewLine + txtLogDetail.Text;
        }

        private void btnLoadSample_Click(object sender, EventArgs e)
        {
            string sampleDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase) + "\\ItemList.txt";
            if (sampleDir.StartsWith("file:\\"))
                sampleDir = sampleDir.Replace("file:\\", "");
            if (!File.Exists(sampleDir))
                return;
            var itemList = File.ReadAllLines(sampleDir);
            if (itemList.Length == 0) return;

            foreach (var itemBarcode in itemList)
            {
                txtBarcode.Text = itemBarcode;
                btnAdd_Click(btnAdd, new EventArgs());
            }
        }
    }
}
