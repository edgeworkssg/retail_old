using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using WinPowerPOS.OrderForms;
using PowerPOS.Container;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace SelfServiceKiosk
{
    public partial class frmKioskLandscape : Form
    {
        POSController pos;
        ItemController itemLogic;

        String status;
        decimal PreferedDiscount;
        bool ApplyPromo;

        public frmKioskLandscape()
        {
            InitializeComponent();

            pos = new POSController();
            itemLogic = new ItemController();

            ApplyPromo = true;

            btnWeight.OnClicked += new WinPowerPOS.KioskForms.WeightButton.OnClickEventHandler(btnWeight_OnClicked);
            btnKeyCode.OnClicked += new WinPowerPOS.KioskForms.KeyCodeButton.OnClickEventHandler(btnKeyCode_OnClicked);
            btnCancelAll.OnClicked += new WinPowerPOS.KioskForms.CancelAllButtonLandscape.OnClickEventHandler(btnCancelAll_OnClicked);
            btnFinishAndPay.OnClicked += new WinPowerPOS.KioskForms.FinishAndPayButtonLandscape.OnClickEventHandler(btnFinishAndPay_OnClicked);
            lvOrders.OnKeyDown += new WinPowerPOS.KioskForms.OrderListLandscape.OnKeyDownEventHandler(lvOrders_OnKeyDown);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Top = 0;
            pnlBanner.Top = 0;
            pnlBanner.Left = 0;

            pnlBanner.BackColor = Color.FromArgb(255, 0, 0);
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            pictureBox1.Height = 80;            
            pnlBanner.Height = pictureBox1.Height;

            Form1_Resize(null, null);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            pnlLeft.Width = this.Width / 2;

            panel4.Width = Width / 2;
            label2.Width = (int)(panel4.Width * 0.72);

            panel5.Width = Width / 2;
            label4.Width = (int)(panel5.Width * 0.72);

            pnlGroupLeft.Width = Width / 2;
            pnlCancelAll.Width = pnlGroupLeft.Width / 2;
            pnlFinishAll.Width = pnlGroupLeft.Width / 2;


            label1.Left = (pnlRight.Width - label1.Width) / 2;
            pictureBox2.Left = (pnlRight.Width - pictureBox2.Width) / 2;

            pnlRight.Width = Width / 2;
            pnlGroup2.Width = pnlRight.Width;
            pnlWeight.Width = pnlGroup2.Width / 2;

            pnlBanner.Width = pnlRight.Width;
            if (pictureBox1.Image != null)
                pictureBox1.Left = (pnlRight.Width - pictureBox1.Image.Width) / 2;
        }

      

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtBarcode.Focus();
        }

        void btnWeight_OnClicked(object sender, EventArgs args)
        {
            txtBarcode.Focus();
        }

        void btnKeyCode_OnClicked(object sender, EventArgs args)
        {
            txtBarcode.Focus();
        }

        void btnCancelAll_OnClicked(object sender, EventArgs args)
        {
            txtBarcode.Focus();
        }

        void btnFinishAndPay_OnClicked(object sender, EventArgs args)
        {
            txtBarcode.Focus();
        }

        private void txtBarcode_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtBarcode.Text = txtBarcode.Text.Trim();

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
                    //Logger.writeLog("Checking Barcode");
                    //if (CheckMembershipFromScannedBarcode())
                    //{
                    //    pos.ApplyMembershipDiscount();
                    //    txtBarcode.Focus();
                    //    return;
                    //}

                    //MEMBERSHIP SIGNUP
                    //if (txtBarcode.Text == MembershipController.MEMBERSHIP_SIGNUP_BARCODE)
                    //{

                    //    if (MembershipSignup(true))
                    //    {
                    //        pos.ApplyMembershipDiscount();
                    //        txtBarcode.Focus();
                    //        return;
                    //    }
                    //}


                    //if (txtBarcode.Text == MembershipController.RENEWAL_BARCODE)
                    //{

                    //    if (MembershipRenewal(true))
                    //    {
                    //        txtBarcode.Focus();
                    //        return;
                    //    }
                    //}

                    //Check Item Group barcode
                    //if (CheckBarcodeFromItemGroup()) return;

                    //if (CheckBarcodeFromPromoBarcode()) return;

                    //if (CheckBarcodeForRefund()) return;
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

        private bool AddItemToOrder()
        {
            //Adi 20131122 Load Settings
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
            if (useSpecialBarcode)
            {
                if (txtBarcode.Text.Substring(0, checkstring.Length) == checkstring)
                {
                    int itemstart = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitStart));
                    int itemlength = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitLength));
                    int pricestart = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitStart));
                    int pricelength = Convert.ToInt16(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitLength));
                    int recordedstart = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.RecordedDigitStart), 0);
                    int recordedlength = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.RecordedDigitLength), 0);

                    int minLength = 0;
                    if (itemstart > pricestart)
                    {
                        minLength = itemstart - 1 + itemlength;
                    }
                    else
                    {
                        minLength = pricestart - 1 + pricelength;
                    }

                    if (txtBarcode.Text.Length < minLength)
                    {
                        status = "Error. Special Barcode Length is Wrong. Please check the settings.";
                        //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                        return false;
                    }

                    string itemNo = txtBarcode.Text.Substring(itemstart - 1, itemlength);
                    string price = txtBarcode.Text.Substring(pricestart - 1, pricelength);

                    string recordedDigits = "";
                    if (recordedstart > 0 && recordedlength > 0)
                    {
                        try
                        {
                            recordedDigits = txtBarcode.Text.Substring(recordedstart - 1, recordedlength);
                        }
                        catch (Exception ex)
                        {
                            status = "Unable to parse recorded digits of Special Barcode. Please check the settings.";
                            //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            Logger.writeLog(ex);
                            return false;
                        }
                    }

                    theItem = itemLogic.FetchByBarcode(itemNo);
                    itemName = "";

                    if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
                    {
                        decimal priceDec = Convert.ToDecimal(price);
                        priceDec = priceDec / 100;

                        if (!pos.IsRestricted(theItem, out status))
                        {
                            //frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                            //frmError.lblMessage = status;
                            //frmError.ShowDialog();
                            //txtBarcode.Focus();
                            //return false;
                        }
                        else
                        {
                            if (!pos.AddSpecialItemToOrder(theItem, 1, priceDec, PreferedDiscount, ApplyPromo, "", recordedDigits, out status))
                            {
                                //alert error message.                    
                                //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.Focus();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        status = "Barcode does not exist in the system.";
                        //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtBarcode.Focus();
                    }
                    //MessageBox.Show(itemNo+ "," + price);
                }
                else
                {
                    //find item using the barcode
                    theItem = itemLogic.FetchByBarcode(txtBarcode.Text);
                    itemName = "";

                    if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
                    {
                        if (!pos.IsRestricted(theItem, out status))
                        {
                            //frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                            //frmError.lblMessage = status;
                            //frmError.ShowDialog();
                            //txtBarcode.Focus();
                            //return false;
                        }
                        else
                        {
                            if (!pos.AddItemToOrder(theItem, 1, PreferedDiscount, ApplyPromo, out status))
                            {
                                //alert error message.                    
                                //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.Focus();
                                return false;
                            }

                            else
                            {
                                if (status != "")
                                {
                                    //MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    txtBarcode.Focus();
                                    return true;
                                }
                                itemName = theItem.ItemName;
                                pos.ApplyMembershipDiscount();

                                // LowQuantity Feature
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                                {
                                    if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                                    {
                                        //MessageBox.Show(theItem.ItemName + " item is only left with " + ItemController.GetStockOnHand(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                    }
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
                            if (!pos.IsRestricted(new Item(altbar[0].ItemNo), out status))
                            {
                                //frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                                //frmError.lblMessage = status;
                                //frmError.ShowDialog();
                                //txtBarcode.Focus();
                                //return false;
                            }
                            else
                            {

                                if (!pos.AddItemToOrder(new Item(altbar[0].ItemNo),
                                    1, PreferedDiscount,
                                    true, out status))
                                {
                                    //alert error message.                    
                                    //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                        //MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }

                                    // LowQuantity Feature
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                                    {
                                        if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                                        {
                                            //MessageBox.Show(item.ItemName + " is only left with " + ItemController.GetStockOnHand(item.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //if (!CheckVoucherFromScannedBarcode())
                            //{
                            //    frmErrorMessage frm = new frmErrorMessage();
                            //    frm.lblMessage = "Error: Barcode " + tmpBarcode + " does not exist";
                            //    frm.ShowDialog();
                            //    //MessageBox.Show(, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                        }
                    }
                }
            }
            else
            {
                //find item using the barcode
                //Logger.writeLog("Check Item Barcode");
                theItem = itemLogic.FetchByBarcode(txtBarcode.Text);
                itemName = "";
                if (theItem != null && theItem.IsLoaded && !theItem.IsNew)
                {

                    if (!pos.IsRestricted(theItem, out status))
                    {
                        //frmErrorMessageWithTextArea frmError = new frmErrorMessageWithTextArea();
                        //frmError.lblMessage = status;
                        //frmError.ShowDialog();
                        //txtBarcode.Focus();
                        //return false;
                    }
                    else
                    {
                        if (!pos.AddItemToOrder(theItem, 1, PreferedDiscount, ApplyPromo, out status))
                        {
                            //alert error message.                    
                            //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtBarcode.Focus();
                            return false;
                        }
                        else
                        {
                            if (status != "")
                            {
                                //MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                txtBarcode.Focus();
                                return true;
                            }
                            itemName = theItem.ItemName;
                            pos.ApplyMembershipDiscount();

                            // LowQuantity Feature
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false))
                            {
                                if (ItemController.IsLowQuantityItem(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID, 1))
                                {
                                    //MessageBox.Show(theItem.ItemName + " is only left with " + ItemController.GetStockOnHand(theItem.ItemNo, PointOfSaleInfo.InventoryLocationID).ToString() + " qty.");
                                }
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
                            if (!pos.AddItemToOrder(new Item(altbar[0].ItemNo),
                                1, PreferedDiscount,
                                true, out status))
                            {
                                //alert error message.                    
                                //MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtBarcode.Text = "";
                                txtBarcode.Focus();
                                return false;
                            }
                            else
                            {
                                itemName = new Item(altbar[0].ItemNo).ItemName;
                                if (status != "")
                                {
                                    //MessageBox.Show("Warning: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            pos.ApplyMembershipDiscount();
                        }

                    }
                    else
                    {

                        if (!CheckVoucherFromScannedBarcode())
                        {
                            //frmErrorMessage frm = new frmErrorMessage();
                            //frm.lblMessage = "Error: Barcode " + tmpBarcode + " does not exist";
                            //frm.ShowDialog();
                            //MessageBox.Show("Error: Barcode " + txtBarcode.Text + " does not exist in the system.", "", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);
                        }
                    }
                }
            }
            //refresh the display
            BindGrid();
            //PriceDisplay                            
            //myDisplay.ClearScreen();
            //if (theItem.IsServiceItem.GetValueOrDefault(false) && theItem.IsInInventory)
            //{
            //    OrderDetCollection tmpCol = pos.FetchUnsavedOrderDet();
            //    for (int i = tmpCol.Count - 1; i >= 0; i--)
            //    {
            //        OrderDet od = tmpCol[i];
            //        if (od.ItemNo == theItem.ItemNo)
            //        {
            //            myDisplay.ShowItemPrice(
            //                itemName,
            //                (double)od.Amount,
            //                (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
            //        }
            //    }
            //}
            //else
            //{
            //    myDisplay.ShowItemPrice(
            //        itemName,
            //        (double)theItem.RetailPrice,
            //        (double)pos.CalculateTotalAmount(out status), poleDisplayWidth);
            //}
            txtBarcode.Text = "";

            label5.Text = string.Format("{0:N2}", pos.CalculateTotalAmount(out status));

            return true;
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
                    if (pos.AddVoucherItemToOrder(v.Amount, PreferedDiscount, v, false, true, out status))
                    {
                        txtBarcode.Text = "";
                        //BindGrid();
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
                        //BindGrid();
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

        public void BindGrid()
        {
            lvOrders.SetDataSource(pos.FetchUnSavedOrderItemsForGrid(out status));
        }

        void lvOrders_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs args)
        {
            txtBarcode.Focus();
            txtBarcode.Text = String.Format("{0}", GetCharFromKey(args.Key));
            txtBarcode.Text = txtBarcode.Text.Trim();
            
            // little bit trick
            txtBarcode.SelectionStart = txtBarcode.Text.Length  + 1; // add some logic if length is 0
            txtBarcode.SelectionLength = 0;
        }

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
    }
}
