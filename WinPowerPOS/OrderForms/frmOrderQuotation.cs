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

namespace WinPowerPOS.OrderForms
{
    public partial class frmOrderQuotation : FormBase
    {

        bool GiveDiscountAllowed;
        bool UseCustomInvoiceNo;
        //bool ChangeUnitPriceAllowed;
        decimal PreferedDiscount;
        QuoteController pos;
        //Hold Transaction
        POSController[] posHold;

        PriceDisplay myDisplay;
        ItemController itemLogic;
        //Controllers


        DataTable dtHotKeys;
        string status;              //To record statuses
        string poleDisplayMessage;   //Message shown in pole display
        //ProgrammableKeyboardController hotKeyCtrl;

        bool ApplyPromo;
        public const int QTY_COL = 4;
        public const int RPRICE_COL = 5;
        public const int DISC_COL = 7;
        public const int DISCPRICE_COL = 7;
        public const int DISCOUNTDETAIL_COL = 8;
        public const int SPCLDISCPRICE_COL = 9;
        public const int ISSPECIAL_COL = 12;
        public const int PREORDER_COL = 13;
        public const int FOC_COL = 14;
        public const int VOID_COL = 15;
        public const int UP_COL = 16;
        public const int DOWN_COL = 17;
        public const int LINECOMMISSION_COL = 20;

        private bool IsItemPictureShown = false;
        private bool UseProjectModule = false;
        private bool IsMembership_Compulsory = false;
        private bool EnableSecondDiscount = false;
        private int poleDisplayWidth;

        public string OrderHdrID;
        public bool isLoadedFromViewQuotation = false;

        public string mode = "";
        public BackgroundWorker SyncQuotationThread;

        #region "Form event handler"
        public frmOrderQuotation()
        {
            ApplyPromo = true;
            //LoadCultureCode();
            InitializeComponent();
            //hotKeyCtrl = new ProgrammableKeyboardController();
            itemLogic = new ItemController();
            myDisplay = new PriceDisplay();
            pos = new QuoteController();
            pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
            posHold = new POSController[3];
            AssignPrivileges();
            UseCustomInvoiceNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_UseCustomNo), false);
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
            /*if (AppSetting.GetSetting("UseCustomInvoiceNo") == "True")
            {
                UseCustomInvoiceNo = true;
            }*/

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

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith)))
                groupBox5.Text = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);
        }

        private void OrderTaking_Activated(object sender, EventArgs e)
        {
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
                QuoteController.RoundingPreference = AppSetting.GetSettingFromDBAndConfigFile("RoundingPreference").ToString();
            }
            else
            {
                QuoteController.RoundingPreference = "";
            }

            /*DataGridViewImageColumn pictureColumn = new DataGridViewImageColumn();
            pictureColumn.Name = "Photo";
            pictureColumn.Width = 60;
            pictureColumn.Visible = true;
            dgvPurchase.Columns.Insert(2, pictureColumn);

            if (!IsItemPictureShown)
            {
                pictureColumn.Visible = false;
            }*/

            dgvPurchase.Columns["DiscountDetail"].Visible = EnableSecondDiscount;
            dgvPurchase.Columns["Discount"].Visible = !EnableSecondDiscount;

            //load hotkeys
            populateHotKeys();
            //display info
            llCashier.Text = UserInfo.displayName;
            lblPointOfSaleName.Text = PointOfSaleInfo.PointOfSaleName;
            clearControls();
            myDisplay.ClearScreen();
            myDisplay.SendCommandToDisplay(poleDisplayMessage);
            dgvPurchase.AutoGenerateColumns = false;

            if (!string.IsNullOrEmpty(OrderHdrID))
            {
                isLoadedFromViewQuotation = true;
                QuotationHdr header = new QuotationHdr(OrderHdrID);

                UserMst cashier = new UserMst(header.CashierID);

                llCashier.Text = cashier.DisplayName;
                pos.SetHeaderRemark(header.Remark);
                lblRemark.Text = header.Remark;


                if (!string.IsNullOrEmpty(header.MembershipNo))
                {
                    bool hasExpired = false;
                    DateTime ExpiryDate;
                    if (MembershipController.IsExistingMember(header.MembershipNo, out hasExpired, out ExpiryDate))
                    {
                        addMemberToPOS(header.MembershipNo);
                    }
                    else
                    {
                        if (hasExpired)
                        {
                            //prompt window to allow bypass?
                            DialogResult dr = MessageBox.Show("This member has already expired on " + ExpiryDate + ". Do you want to allow it to continue using the card", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                addExpiredMemberToPOS(header.MembershipNo);
                            }
                        }
                    }
                }

                QuotationDetCollection detcol = new QuotationDetCollection();
                detcol.Where("OrderHdrID", header.OrderHdrID);
                detcol.Load();

                foreach (var item in detcol)
                {

                    if (!pos.AddItemToOrder(item, out status))
                    {
                        MessageBox.Show("Error encountered while adding items. Please try again. " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        return;
                    }
                }
                BindGrid();

            }


        }
        #endregion

        #region "Privileges Related"
        private void AssignPrivileges()
        {
            if (AppSetting.GetSettingFromDBAndConfigFile("ShowFOCColumn") != null
                && AppSetting.GetSettingFromDBAndConfigFile("ShowFOCColumn").ToString().ToLower() == "no")
            {
                dgvPurchase.Columns[FOC_COL].Visible = false;
            }
            else
            {
                dgvPurchase.Columns[FOC_COL].Visible = true;
            }
            if (AppSetting.GetSettingFromDBAndConfigFile("ShowPreOrderColumn") != null
                && AppSetting.GetSettingFromDBAndConfigFile("ShowPreOrderColumn").ToString().ToLower() == "no")
            {
                dgvPurchase.Columns[PREORDER_COL].Visible = false;
            }
            else
            {
                dgvPurchase.Columns[PREORDER_COL].Visible = true;
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
                string buttonName = ((Button)(sender)).Name.ToString();
                DataRow[] dr;
                dr = dtHotKeys.Select("keycode='" + buttonName + "'");
                if (dr != null && dr.Length > 0)
                {
                    Item myItem = new Item(dr[0]["itemno"].ToString());
                    if (!pos.AddItemToOrder(myItem, 1, PreferedDiscount, ApplyPromo, out status))
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
                    }
                    BindGrid();
                    //PriceDisplay //myDisplay = new PriceDisplay();
                    myDisplay.ClearScreen();
                    myDisplay.ShowItemPrice(myItem.ItemName, (double)myItem.RetailPrice,
                        (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                    updateTotalAmount();
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

        #region "Scan and Search"
        private void btnSearchMember_Click(object sender, EventArgs e)
        {

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


                    if (member.ExpiryDate.HasValue && member.ExpiryDate.Value >= DateTime.Now)
                    {
                        //havent expired
                        addMemberToPOS(member.MembershipNo);
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
                    //pos.ApplyMembershipDiscount();
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

            frmAddItem myAddItem = new frmAddItem();
            myAddItem.searchReq = txtBarcode.Text.Replace(' ', '%');
            myAddItem.PreferedDiscount = PreferedDiscount;
            myAddItem.ShowDialog();
            Item myItem;
            if (myAddItem != null && myAddItem.itemNumbers != null)
            {
                for (int i = 0; i < myAddItem.itemNumbers.Count; i++)
                {
                    myItem = new Item(myAddItem.itemNumbers[i]);
                    if (!pos.AddItemToOrder(myItem, 1, (decimal)PreferedDiscount, ApplyPromo, out status))
                    {
                        MessageBox.Show("Error encountered while adding items. Please try again. " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        return;
                    }
                    else
                    {
                        //pos.ApplyMembershipDiscount();
                        myDisplay.ClearScreen();
                        myDisplay.ShowItemPrice(
                            myItem.ItemName,
                            (double)myItem.RetailPrice,
                            (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);

                        // LowQuantity Feature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                        {
                            if (ItemController.IsLowQuantityItem(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                            {
                                MessageBox.Show("This item is only left with " + ItemController.GetStockOnHand(myItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
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

                    //if (CheckMembershipFromScannedBarcode())
                    //{
                    //Adi 20131120 Remark for the case
                    //pos.ApplyMembershipDiscount();
                    //txtBarcode.Focus();
                    //return;
                    //}

                    //MEMBERSHIP SIGNUP
                    if (txtBarcode.Text == MembershipController.MEMBERSHIP_SIGNUP_BARCODE)
                    {

                        if (MembershipSignup(true))
                        {
                            //pos.ApplyMembershipDiscount();
                            txtBarcode.Focus();
                            return;
                        }
                    }


                    if (txtBarcode.Text == MembershipController.RENEWAL_BARCODE)
                    {

                    }

                    //Check Item Group barcode
                    if (CheckBarcodeFromItemGroup()) return;

                    AddItemToOrder();

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
        private bool CheckBarcodeFromItemGroup()
        {
            //
            string status;
            ItemGroup it = new ItemGroup(ItemGroup.Columns.Barcode, txtBarcode.Text);

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
                    if (!pos.AddItemToOrder(
                        new ViewItem(ViewItem.Columns.ItemNo, itG[i].ItemNo),
                        itG[i].UnitQty, pos.GetPreferredDiscount(),
                        true, out status))
                    {
                        MessageBox.Show("Error adding item to order: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        return false;
                    }
                }
                BindGrid();
                txtBarcode.Focus();
                txtBarcode.Text = "";
                return true;
            }
            return false;
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
            //find item using the barcode
            ViewItem theItem = itemLogic.FetchByBarcode(txtBarcode.Text);
            string itemName = "";

            if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
            {
                if (!pos.AddItemToOrder(theItem, 1, PreferedDiscount, ApplyPromo, out status))
                {
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
                        if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID,1))
                        {
                            MessageBox.Show("This item is only left with " + ItemController.GetMinQty(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                        }
                    }
                }
            }
            else
            {
                AlternateBarcodeCollection altbar = new AlternateBarcodeCollection();
                altbar.Where(AlternateBarcode.Columns.Barcode, txtBarcode.Text);
                altbar.OrderByDesc(AlternateBarcode.Columns.CreatedOn);
                altbar.Load();

                if (altbar.Count > 0 && altbar[0].IsLoaded)
                {
                    if (!pos.AddItemToOrder(new Item(altbar[0].ItemNo),
                        1, PreferedDiscount,
                        true, out status))
                    {
                        //alert error message.                    
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Text = "";
                        txtBarcode.Focus();
                        return false;
                    }
                    else
                    {
                        itemName = new Item(altbar[0].ItemNo).ItemName;
                        if (status != "")
                        {
                            MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {

                    if (!CheckVoucherFromScannedBarcode())
                    {
                        MessageBox.Show("Error: Barcode does not exist in the system.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            //refresh the display
            BindGrid();

            //PriceDisplay                            
            myDisplay.ClearScreen();
            myDisplay.ShowItemPrice(
                itemName,
                (double)theItem.RetailPrice,
                (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
            txtBarcode.Text = "";
            return true;
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
                OrderForms.frmQuotationLineKeypad t = new WinPowerPOS.OrderForms.frmQuotationLineKeypad();
                t.pos = pos;
                t.LineID = orderDetID;
                t.editField = frmQuotationLineKeypad.EditedField.RetailPrice;
                t.ApplyPromo = ApplyPromo;

                CommonUILib.displayTransparent();
                t.ShowDialog();
                CommonUILib.hideTransparent();

                BindGrid();
                updateTotalAmount();
                myDisplay.ShowTotal(double.Parse(lblTotalAmount.Text));
            }
            //}
        }

        private bool CheckVoucherFromScannedBarcode()
        {
            Voucher v = new Voucher(Voucher.Columns.VoucherNo, txtBarcode.Text);
            if (v.IsLoaded && !v.IsNew)
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
                frmMembershipQuickAddQuotation frm = new frmMembershipQuickAddQuotation();
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
        /*private bool MembershipRenewal(bool AddNewItemLine)
        {


            //pop up the membership option
            /*frmRenewal frm = new frmRenewal();
            frm.pos = this.pos;

            frm.ShowDialog();
            if (frm.IsSuccessful)
            {
                Membership member = frm.pos.GetMemberInfo();

                this.pos.CurrentMember.MembershipGroupId = frm.selectedMembershipGroupID;
                var discount = new Select(MembershipGroup.Columns.Discount).From("MembershipGroup").Where(MembershipGroup.Columns.MembershipGroupId).IsEqualTo(pos.CurrentMember.MembershipGroupId).ExecuteScalar();
                try
                {
//                    pos.applyDiscount(Convert.ToDecimal(discount));
                }
                catch { }

                //this.pos.CurrentMember.
                //pos.SetNewMembershipGroupID(frm.selectedMembershipGroupID);

                //this.pos.AssignMembership();

                //pos.ApplyMembershipDiscount();



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
        }*/
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

                        if (member.ExpiryDate.HasValue && member.ExpiryDate.Value >= DateTime.Now)
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

            if (pos.AssignMembership(membershipNo, out status))
            {
                //pos.ApplyMembershipDiscount();
                BindGrid();
                Membership member = pos.GetMemberInfo();
                DisplayMembershipToScreen(member);
                /*
                dgvPastTransaction.DataSource =
                    MembershipController.FetchLastPurchasedTransactions(member.MembershipNo);
                dgvPastTransaction.Refresh();
                 * */
                myDisplay.ClearScreen();
                myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                txtBarcode.Text = "";
                txtBarcode.Focus();
                return true;
            }
            return false;

        }
        private bool addExpiredMemberToPOS(string membershipNo)
        {

            if (pos.AssignExpiredMember(membershipNo, out status))
            {
                //pos.ApplyMembershipDiscount();
                BindGrid();
                Membership member = pos.GetMemberInfo();
                DisplayMembershipToScreen(member);
                /*
                dgvPastTransaction.DataSource =
                    MembershipController.FetchLastPurchasedTransactions(member.MembershipNo);
                dgvPastTransaction.Refresh();
                 * */
                myDisplay.ClearScreen();
                myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                txtBarcode.Text = "";
                txtBarcode.Focus();
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
            if (member.DateOfBirth.HasValue)
                lblDOB.Text = member.DateOfBirth.Value.ToString("dd MMM yyyy");

            if (member.DateOfBirth.HasValue && member.DateOfBirth.Value.Month == DateTime.Now.Month)
            {
                MessageBox.Show("MEMBER DATE OF BIRTH IS THIS MONTH (" + member.DateOfBirth.Value.ToString("dd MMM yyyy") + ")");
            }
        }
        #endregion

        #region "Confirm, Cancel & updating total amount"

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to cancel order?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                myDisplay.ClearScreen();
                clearControls();
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
                pos = new QuoteController();
                pos.OpenPriceItemAdded += new OpenPriceItemHandler(pos_OpenPriceItemAdded);
                string useCustomInvoiceNo = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_UseCustomNo);
                if (useCustomInvoiceNo != null && useCustomInvoiceNo.ToLower() == "true")
                {
                    this.lblRefNo.Text = pos.GetUnsavedCustomRefNo();
                }
                else
                {
                    string temp = pos.GetUnsavedRefNo();
                    if (!string.IsNullOrEmpty(temp))
                    {
                        this.lblRefNo.Text = temp.Substring(temp.Length - 3);
                    }
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
                //lblTotalAmount.Text = "0";
                lblTotalDiscount.Text = "0";
                lblGrossAmount.Text = "0";
                PreferedDiscount = 0;
                lblGST.Text = "0";
                //lblPreferredDisc.Text = "0";
                btnDiscount.Text = "No Discount";

                //lblNumOfItems.Text = "0";
                rbPromoEnable.Checked = true;
                // Clear Membership info
                lblMemberGroup.Text = "-";
                lblMembershipNo.Text = "-";

                lblName.Text = "-";
                lblExpiryDate.Text = "-";
                lblDiscount.Text = "-";
                lblRemark.Text = "-";
                lblDOB.Text = "-";
                /*Clear History Grid
                for (int j = dgvPastTransaction.Rows.Count-1; j >= 0; j--)
                {
                    dgvPastTransaction.Rows.RemoveAt(j);
                }*/
                myDisplay.ClearScreen();
                myDisplay.SendCommandToDisplay(poleDisplayMessage);
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
                /*if (disc == 0)
                {
                    //btnDiscount.Text = "No Discount";
                }
                else
                {
                    btnDiscount.Text = ((disc % 1 > 0) ? disc.ToString("N2") : disc.ToString("N0")) + "%";
                }*/
                //Calculate total discount
                decimal GrossAmount = pos.CalculateGrossAmount();
                decimal TotalAmount = pos.CalculateTotalAmount(out status);
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.IsRoundingPreferenceAccordingToConfig), false))
                    TotalAmount = pos.RoundTotalReceiptAmount();
                decimal GSTAmount = pos.calculateTotalGST();

                lblTotalAmount.Text = TotalAmount.ToString("N");
                lblGrossAmount.Text = GrossAmount.ToString("N");
                decimal totalDiscount = pos.CalculateTotalDiscount();//(GrossAmount - TotalAmount);
                pos.SetTotalDiscount(totalDiscount);
                lblTotalDiscount.Text = totalDiscount.ToString("N");
                lblGST.Text = GSTAmount.ToString("N2");

                if (status != "")
                {
                    MessageBox.Show("Error while calculating total amount: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                string campaignName, campaignType;
                //pos.GetPromotionInfo(out campaignName, out campaignType);
                if (!pos.MembershipApplied())
                {
                    ClearMembership();
                }
                pnlShowChange.Visible = false;
                txtBarcode.Focus();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void HandleNegativeAmount()
        {
            if (PointOfSaleInfo.promptSalesPerson)
            {
                frmSalesPerson f = new frmSalesPerson();
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

            bool IsPointAllocationSuccess = false;
            bool isRoundingAllPayment = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.IsRoundingPreferenceAccordingToConfig), false);

            if (pos.ConfirmOrder(isRoundingAllPayment, out IsPointAllocationSuccess, out status))
            {
                if (UseCustomInvoiceNo)
                {
                    CustNumUpdate(); //Graham
                }

                //remark to be opened
                /*POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);*/


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
                if (e.ColumnIndex == QTY_COL || e.ColumnIndex == RPRICE_COL || e.ColumnIndex == DISCPRICE_COL || e.ColumnIndex == DISCOUNTDETAIL_COL)
                {

                    string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    /*
                    if (e.ColumnIndex == RPRICE_COL && 
                        !PrivilegesController.HasPrivilege(PrivilegesController.CHANGE_UNIT_PRICE, 
                        UserInfo.privileges))
                    {
                        //insufficient privilege
                        return;
                    }
                    */
                    OrderForms.frmQuotationLineKeypad t = new WinPowerPOS.OrderForms.frmQuotationLineKeypad();
                    t.pos = pos;
                    t.LineID = ID;
                    if (e.ColumnIndex == QTY_COL)
                    {
                        t.editField = frmQuotationLineKeypad.EditedField.Quantity;
                    }
                    else if (e.ColumnIndex == RPRICE_COL)
                    {
                        t.editField = frmQuotationLineKeypad.EditedField.RetailPrice;
                    }
                    if (e.ColumnIndex == DISCPRICE_COL)
                    {
                        t.editField = frmQuotationLineKeypad.EditedField.DiscountedPrice;

                    }
                    t.ApplyPromo = ApplyPromo;

                    CommonUILib.displayTransparent();
                    t.ShowDialog();
                    CommonUILib.hideTransparent();

                    BindGrid();
                    updateTotalAmount();
                    myDisplay.ShowTotal(double.Parse(lblTotalAmount.Text));
                }
                else if (e.ColumnIndex == SPCLDISCPRICE_COL)
                {
                    CommonUILib.displayTransparent();
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
                    frmDiscountSpecialQuotation frm = new frmDiscountSpecialQuotation();
                    frm.pos = this.pos;
                    frm.LineID = ID;
                    frm.StartPosition = FormStartPosition.CenterParent;

                    /*frmSpecialDiscount frm = new frmSpecialDiscount();
                    frm.pos = this.pos;
                    frm.LineID = ID;
                    frm.StartPosition = FormStartPosition.CenterParent;*/

                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    //Adi 20131121
                    //if (frm.IsSuccessful)
                    //{
                    updateTotalAmount();
                    BindGrid();
                    //}
                }
                else if (e.ColumnIndex == LINECOMMISSION_COL)
                {
                    string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    //pop out line remark and sales person

                    QuotationDet det = pos.GetLine(LineID, out status);

                    frmLineCommission f = new frmLineCommission(det.ItemNo);
                    f.Remark = det.Remark;
                    f.UserName = det.Userfld1;
                    f.OldReceiptNo = "";

                    if (det.Quantity < 0)
                    {
                        f.IsSalesReturn = true;
                    }

                    f.ShowDialog();
                    if (f.IsSuccessful)
                    {
                        //assign to Line ID
                        pos.AssignItemSalesPerson(LineID, f.UserName);
                        pos.SetLineRemark(LineID, f.Remark, out status);
                        det.Userfld4 = f.LineInfoRemark.Trim();
                        //pos.AssignSalesReturnReceiptNo(LineID, f.OldReceiptNo, out status);
                        //ItemReturn = f.OldReceiptNo;
                    }
                    f.Dispose();

                    updateTotalAmount();
                    BindGrid();
                    myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                    txtBarcode.Focus();
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
                dgvPurchase.DataSource = pos.FetchUnSavedOrderItems(out status);
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
            bool IsPromo, IsExchange;

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

                    if (bool.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["IsExchange"].Value.ToString(),
                        out IsExchange)
                        && IsExchange)
                    {
                        return;
                    }
                    string LineID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
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
                    //pos.UndoPromo();
                    //pos.ApplyPromo();
                    //dgvPurchase.Rows[e.RowIndex].Cells["Amount"].Value = pos.GetLineAmount(LineID, out status).ToString("N2");
                    updateTotalAmount();
                    BindGrid();
                    myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
                    txtBarcode.Focus();
                }
                else if (e.ColumnIndex == UP_COL) //Up
                {
                    string id = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    pos.MoveDown(id);
                    BindGrid();
                    txtBarcode.Focus();
                }
                else if (e.ColumnIndex == DOWN_COL) //Down
                {
                    string id = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    pos.MoveUp(id);
                    BindGrid();
                    txtBarcode.Focus();
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

        }
        private void btnHold_Click(object sender, EventArgs e)
        {

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

                if (pos.IsItemIsInOrderLine("INS_PAYMENT") != "")
                    throw new Exception("(warning)Please assign membership to continue");



                #region *) Initialize: No Membership = [WALK-IN] membership
                if (!pos.MembershipApplied())
                {
                    MessageBox.Show("Please register a member or Scan membership card.", "Membership is Compulsory");
                    return;
                }
                #endregion



                //MessageBox.Show(pos.CalculateTotalAmount(out status).ToString());

                /// TODO: [BUG]
                /// Validation won't success. Covered in the Negative Amount.
                //#region *) Validation: Do not confirm is amount is 0
                //if (this.lblTotalAmount.Text == "0" || this.lblTotalAmount.Text == "0.00")
                //{
                //    txtBarcode.Focus();
                //    return;
                //}
                //#endregion
                bool IsPointAllocated = true;
                bool isRoundingAllPayment = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.IsRoundingPreferenceAccordingToConfig), false);

                if (pos.ConfirmOrder(isRoundingAllPayment, out IsPointAllocated, out status))
                {
                    if (!SyncQuotationThread.IsBusy)
                        SyncQuotationThread.RunWorkerAsync();

                    QuoteController qos = new QuoteController(pos.myOrderHdr.OrderHdrID);
                    POSDeviceController.PrintQuotation(qos, 0, false,
                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                    if (UseCustomInvoiceNo)
                    {
                        CustNumUpdate();//Graham
                    }
                    clearControls();

                    if (isLoadedFromViewQuotation)
                    {
                        this.Close();
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
        private void CustNumUpdate()
        {
            #region customNo Update
            int runningNo = 0;

            string selectmaxno = "select AppSettingValue from AppSetting where AppSettingKey='Quotation_CurrentReceiptNo'";
            string currentReceiptNo = DataService.ExecuteScalar(new QueryCommand(selectmaxno)).ToString();

            int.TryParse(currentReceiptNo, out runningNo);

            string updatemaxnum1 = "update appsetting set AppSettingValue='" + ++runningNo + "' where AppSettingKey='Quotation_CurrentReceiptNo'";
            DataService.ExecuteQuery(new QueryCommand(updatemaxnum1));

            //default max receiptno is 4
            string sql2 = "select case AppSettingValue when '' then '4' else AppSettingValue end from AppSetting where AppSettingKey='Quotation_ReceiptLength'";
            QueryCommand Qcmd2 = new QueryCommand(sql2);
            int maxReceiptNo = 4;
            int.TryParse(DataService.ExecuteScalar(Qcmd2).ToString(), out maxReceiptNo);

            //check if it has reached the max no for that digit (99,999,9999.. etc)
            bool maximumReached = false;
            if (maxReceiptNo != 0 && currentReceiptNo.Length >= maxReceiptNo)
            {
                maximumReached = true;
                for (int i = 0; i < maxReceiptNo; i++)
                {
                    if (currentReceiptNo[i] != '9')
                    {
                        maximumReached = false;
                        break;
                    }
                }

            }

            //if it has reached, update the maxreceiptno
            if (maximumReached)
            {
                string sql3 = "update appsetting set AppSettingValue = " + ++maxReceiptNo + " where AppSettingKey='Quotation_CurrentReceiptNo'";
                QueryCommand Qcmd3 = new QueryCommand(sql3);
                DataService.ExecuteQuery(Qcmd3);

                //string sql4 = "update appsetting set AppSettingValue = 0 where AppSettingKey='CurrentReceiptNo'";
                //QueryCommand Qcmd4 = new QueryCommand(sql4);
                //DataService.ExecuteQuery(Qcmd4);

                //currentReceiptNo = "0";
            }
            #endregion
        }
        private void btnDiscount_Click(object sender, EventArgs e)
        {
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
            frmDiscountsQuotation frm = new frmDiscountsQuotation();
            frm.pos = pos;
            frm.discChoosed = btnDiscount.Text;
            frm.ShowDialog();
            if (frm.IsSuccessful)
            {
                PreferedDiscount = frm.discApplied; //record prefered discounts
                myDisplay.ShowSubTotal("Discount Applied", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
                btnDiscount.Text = frm.discChoosed;
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


            }

        }

        private void btnCatAccess_Click(object sender, EventArgs e)
        {
            string useXMLForHotKey = AppSetting.GetSetting("UseXMLForHotKey");

            if (useXMLForHotKey != null && useXMLForHotKey.ToLower() == "yes")
            {
                frmTouchScreenXMLQuotation test = new frmTouchScreenXMLQuotation(pos);
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
                    frmCatAccessQuotation f = new frmCatAccessQuotation();
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
                    f.ShowDialog();
                    BindGrid();
                    myDisplay.ClearScreen();
                    myDisplay.ShowSubTotal("Total", (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);

                    f.Dispose();
                }
                else
                {
                    frmGroupAccessQuotation f = new frmGroupAccessQuotation();
                    f.pos = pos;
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
            /*if (MembershipRenewal(true))
            {
                pnlMembership.SendToBack();
            }*/
        }

        private void btnCancelMembership_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to remove membership?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                pos.RemoveMemberFromReceipt();
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
                f.ShowDialog();

                f.Dispose();
            }
        }

        private void pnlShowChange_Click(object sender, EventArgs e)
        {
            pnlShowChange.Visible = false;
        }

        private void lblChange_Click(object sender, EventArgs e)
        {
            pnlShowChange.Visible = false;
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
                FormController.ShowForm(FormController.FormNames.AttendanceModule, new Attendance.frmAttendance());
        }

        private void btnMembershipRemark_Click(object sender, EventArgs e)
        {
            if (lblMembershipNo.Text != "" && lblMembershipNo.Text != "-")
            {
                frmMembershipEditRemark f = new frmMembershipEditRemark(lblMembershipNo.Text);
                f.ShowDialog();

                f.Dispose();
            }
        }

        private void btnLineInfo_Click(object sender, EventArgs e)
        {
            /*if (this.pos.myOrderDet.Count != 0)
            {
                frmLineInfo frm = new frmLineInfo(this.pos);
                frm.ShowDialog();
                frm.Dispose();
            }*/
        }
        private void ShowItemPicture()
        {
            try
            {
                if (IsItemPictureShown)
                {
                    string ImagePhotosFolder = ConfigurationManager.AppSettings["ItemPhotosFolder"];
                    if (String.IsNullOrEmpty(ImagePhotosFolder))
                    {
                        return;
                    }
                    if (!ImagePhotosFolder[ImagePhotosFolder.Length - 1].ToString().Equals(@"\"))
                        ImagePhotosFolder += @"\";

                    string[] extensions = { "jpg", "png", "bmp", "jpeg" };
                    foreach (DataGridViewRow row in dgvPurchase.Rows)
                    {
                        string ImagePath = ImagePhotosFolder + row.Cells["ItemNo"].Value.ToString();
                        bool found = false;
                        foreach (string ext in extensions)
                        {
                            if (System.IO.File.Exists(ImagePath + "." + ext))
                            {
                                ImagePath = ImagePath + "." + ext;
                                found = true;
                                break;
                            }

                        }
                        if (found)
                        {
                            Image img = ResizeImage(Image.FromFile(ImagePath), new Size(40, 40));
                            row.Cells["Photo"].Value = img;
                            row.Cells["Photo"].Tag = ImagePath;
                        }

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
                            if (dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null) return;

                            if (!shown)
                            {
                                Image img = Image.FromFile(dgvPurchase.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString());
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
        private Image ResizeImage(Image imgToResize, Size size)
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
    }
}