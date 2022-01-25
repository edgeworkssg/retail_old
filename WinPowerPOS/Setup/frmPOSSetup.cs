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
using System.IO;
using System.IO.Ports;
using System.Diagnostics;


using WinPowerPOS.LoginForms;
using System.Threading;
using System.Reflection;
using System.Drawing.Text;
namespace WinPowerPOS.OrderForms
{
    public partial class frmPOSSetup : Form
    {                

        public frmPOSSetup()
        {
            InitializeComponent();

            ReceiptSetup.LoadReceiptSetup();
            try
            {
                

                cmbOutlet.DataSource = PointOfSaleController.FetchOutletNames();
                cmbOutlet.Refresh();

                cmbStall.DataSource = PointOfSaleController.FetchPointOfSaleNames();
                cmbStall.Refresh();

                LoadPOSInfo();
                //LoadEZLinkInfo();
                LoadReceiptInfo();
                
                LoadAppSettingInfo();

                #region *) Fetch: Load all listed Printer Name

                txtPrinterName.Items.Clear();
                cbAdditionalPrinter.Items.Clear();
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    txtPrinterName.Items.Add(printer);
                    cbAdditionalPrinter.Items.Add(printer);
                    cbA4Printer.Items.Add(printer);
                    cbValidationPrinter.Items.Add(printer);
                }

                #endregion
                LoadHardware();
                cbA4Printer.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.A4PrinterName);
                string tmp = AppSetting.GetSetting(AppSetting.SettingsName.Print.FormalReceiptFileLocation);
                txtFormalReceipt.Text = (tmp == null ? "" : tmp);
                cbAdditionalPrinter.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.AdditionalPrinter);
                txtAdditionalReceipt.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.AdditionalReceipt);

                string setting = AppSetting.GetSetting(AppSetting.SettingsName.Hotkeys.HotkeysDisplay) + "";

                rbPriceItemName.Checked = string.IsNullOrEmpty(setting)
                                          || setting.ToLower() == AppSetting.SettingsName.HotKeysPriceItemName.ToLower();
                rbItemNamePrice.Checked = setting.ToLower() == AppSetting.SettingsName.HotKeysItemNamePrice.ToLower();
                MaxBalancePayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.MaxBalancePayment) + "";
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
        }
        private void LoadHardware()
        {
            DataSet ds = new DataSet();
            
            ds.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\PriceDisplay.xml");

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtPoleDisplay.Text =  ds.Tables[0].Rows[0]["PrinterName"].ToString();
                txtPoleDisplayClearScreen.Text = ds.Tables[0].Rows[0]["ClearScreen"].ToString();
            }

            txtPrinterName.Text = ReceiptSetup.PrinterName;
            DataSet ds2 = new DataSet();
            ds2.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\POSDevices\\ReceiptPrinter.xml");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                txtCashDrawerPrinterName.Text = ds2.Tables[0].Rows[0].ItemArray[1].ToString();
                txtKickDrawerCommand.Text = ds2.Tables[0].Rows[0].ItemArray[1].ToString();
            }                                    
        }

        private void LoadReceiptInfo()
        {
            cbPrintReceipt.Checked = PrintSettingInfo.receiptSetting.PrintReceipt;
            cbShowMembership.Checked = PrintSettingInfo.receiptSetting.ShowMembershipInfo;
            cbShowSalesPerson.Checked = PrintSettingInfo.receiptSetting.ShowSalesPersonInfo;
            //cbUseOutletAddress.Checked = PrintSettingInfo.receiptSetting.UseOutletAddress;

            txtSalesPersonTitle.Text = PrintSettingInfo.receiptSetting.SalesPersonTitle;

            txtAddress1.Text = PrintSettingInfo.receiptSetting.ReceiptAddress1;
            txtAddress2.Text = PrintSettingInfo.receiptSetting.ReceiptAddress2;
            txtAddress3.Text = PrintSettingInfo.receiptSetting.ReceiptAddress3;
            txtAddress4.Text = PrintSettingInfo.receiptSetting.ReceiptAddress4;

            txtTermCondition1.Text = PrintSettingInfo.receiptSetting.TermCondition1;
            txtTermCondition2.Text = PrintSettingInfo.receiptSetting.TermCondition2;
            txtTermCondition3.Text = PrintSettingInfo.receiptSetting.TermCondition3;
            txtTermCondition4.Text = PrintSettingInfo.receiptSetting.TermCondition4;
            txtTermCondition5.Text = PrintSettingInfo.receiptSetting.TermCondition5;
            txtTermCondition6.Text = PrintSettingInfo.receiptSetting.TermCondition6;

            if (PrintSettingInfo.receiptSetting.PaperSize.HasValue)
                cmbPaperSize.SelectedIndex = PrintSettingInfo.receiptSetting.PaperSize.Value;

            if (PrintSettingInfo.receiptSetting.NumOfCopies.HasValue)
                txtNumOfCopies.Text = PrintSettingInfo.receiptSetting.NumOfCopies.Value.ToString();
            else
                txtNumOfCopies.Text = "1";

            string tmp = AppSetting.GetSetting(AppSetting.SettingsName.Print.ClosingReceiptNumOfCopies);
            if (tmp != "" && tmp != null)
                txtClosingNumOfCopies.Text = tmp;
            else
                txtClosingNumOfCopies.Text = "1";

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), true))
            {
                ShowMemberPointsBalance.Checked = false;
            }
            else
            {
                ShowMemberPointsBalance.Checked = true;
            }

            
            Receipt_ShowReceiptBarcode.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.ShowReceiptNoBarcode), false);
            //cbBarcodeFont.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.BarcodeFontName);
            txtBarcodeFontSize.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.BarcodeFontSize);

        }
        private void LoadPOSInfo()
        {
            cmbOutlet.SelectedItem = PointOfSaleInfo.OutletName;
            cmbOutlet.Refresh();
            cmbStall.SelectedItem = PointOfSaleInfo.PointOfSaleName;
            cmbStall.Refresh();
            txtNETSID.Text = PointOfSaleInfo.NETsTerminalID;
            cbPromptSalesPerson.Checked = PointOfSaleInfo.promptSalesPerson;
            cbUseMembership.Checked = PointOfSaleInfo.useMembership;
            //cbAllowOverallDisc.Checked = PointOfSaleInfo.allowOverallDisc;
            cbAllowLineDiscount.Checked = PointOfSaleInfo.allowLineDisc;
            //cbAllowFeedBack.Checked = PointOfSaleInfo.allowFeedback;
            cbAllowCashierChange.Checked = PointOfSaleInfo.allowChangeCashier;
        }
        public void LoadAppSettingInfo()
        {
            string tmp = AppSetting.GetSetting(AppSetting.SettingsName.Print.ReceiptFileLocation);
            txtReceiptLocation.Text = (tmp == null ? "" : tmp);

            tmp = AppSetting.GetSetting(AppSetting.SettingsName.Print.ClosingReceiptFileLocation);
            txtClosingReceipt.Text = (tmp == null ? "" : tmp);


            tmp = AppSetting.GetSetting("IsRealTimeSales");
            if (tmp == "1")
            {
                cbRealTimeSales.Checked = true;
            }

            string value = AppSetting.GetSetting(AppSetting.SettingsName.CultureCode);
            if (value == "")
            { 
                //SET DEFAULTS
                value= "en-US";
                AppSetting.SetSetting(AppSetting.SettingsName.CultureCode, value);
            }
            switch (value)
            {
                case "en-US":
                    cboLanguage.Text = "ENGLISH";
                    break;
                case "zh-CN":
                    cboLanguage.Text = "ENGLISH";
                    break;
            }

            UseSecondScreen.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false);
            MarqueeText.Text = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.MarqueeText);
            SlideShowFolder.Text = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder);

            //Additional Printer
            UseAdditionalPrinter.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.UseAdditionalPrinter), false);
            
        }
        /*
        private void LoadEZLinkInfo()
        {
            int found = 0;
            string[] portList = PowerPOS.CommPort.Instance.GetAvailablePorts();
            for (int i = 0; i < portList.Length; ++i)
            {
                string name = portList[i];
                cmbPort.Items.Add(name);
                if (name == EZLinkComPortInfo.COMPort)
                    found = i;
            }
            if (portList.Length > 0)
                cmbPort.SelectedIndex = found;

            Int32[] baudRates = {
                100,300,600,1200,2400,4800,9600,14400,19200,
                38400,56000,57600,115200,128000,256000,0
            };
            found = 0;
            for (int i = 0; baudRates[i] != 0; ++i)
            {
                cmbBaudRate.Items.Add(baudRates[i].ToString());
                if (baudRates[i] == EZLinkComPortInfo.BaudRate)
                    found = i;
            }
                

            cmbDataBits.Items.Add("5");
            cmbDataBits.Items.Add("6");
            cmbDataBits.Items.Add("7");
            cmbDataBits.Items.Add("8");
            if (EZLinkComPortInfo.DataBits >= 5)
            {
                cmbDataBits.SelectedIndex = EZLinkComPortInfo.DataBits - 5;
            }

            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                cmbParity.Items.Add(s);
            }
            cmbParity.SelectedIndex = (int)EZLinkComPortInfo.Parity;

            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                cmbStopBits.Items.Add(s);
            }
            cmbStopBits.SelectedIndex = (int)EZLinkComPortInfo.StopBits;


            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                cmbHandShaking.Items.Add(s);
            }
            cmbHandShaking.SelectedIndex = (int)EZLinkComPortInfo.HandShake;

            cbIsEZLinkTerminal.Checked = PointOfSaleInfo.IsEZLinkTerminal;
            txtTerminalID.Text = PointOfSaleInfo.EZLinkTerminalID;
            txtPassword.Text = PointOfSaleInfo.EZLinkTerminalPwd;

        }
        */
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
            if (cmbOutlet.SelectedItem == null || cmbOutlet.SelectedItem.ToString() == "ALL")
            {
                cmbStall.DataSource = PointOfSaleController.FetchPointOfSaleNames();
                cmbStall.Refresh();
            }
            else
            {
                cmbStall.DataSource = 
                    PointOfSaleController.
                    FetchPointOfSaleNamesByOutlet(cmbOutlet.SelectedItem.ToString());
                cmbStall.Refresh();
            }

        }

        private void btnSavePOS_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "POS Setup", "");
            //int termID;
            //if (!int.TryParse(txtTerminalID.Text, out termID))
            //{
            //    MessageBox.Show("Please Enter valid Terminal ID", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            if (cmbStall.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Please Select a valid Point of Sale ID", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int PaperSize, NumOfCopies;
            PaperSize = cmbPaperSize.SelectedIndex;
            if (!int.TryParse(txtNumOfCopies.Text, out NumOfCopies))
            {
                MessageBox.Show("Please set the right number of copies", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isPOSIDChanged = false;
            PointOfSale pr = new PointOfSale(PointOfSale.Columns.PointOfSaleName, cmbStall.SelectedItem.ToString());
            if (!pr.IsNew)
            {
                if (PointOfSaleInfo.PointOfSaleID != pr.PointOfSaleID)
                    isPOSIDChanged = true;
                //Save into setting table.....
                PointOfSaleController.
                    SavePointOfSaleInfo(
                    pr.PointOfSaleID, "", 0,
                    "", "",
                    0,
                    0,
                    0, 0,
                    0, false,
                    false, false, false, cbPromptSalesPerson.Checked, cbUseMembership.Checked,
                    cbAllowLineDiscount.Checked, true,
                    cbAllowCashierChange.Checked, false);

                //Save Receipt Information
                ReceiptSettingController.
                    SaveReceiptPrinterSetting
                    (cbPrintReceipt.Checked, false,
                     txtAddress1.Text, txtAddress2.Text, txtAddress3.Text,
                     txtAddress4.Text, cbShowMembership.Checked,
                     cbShowSalesPerson.Checked,
                     txtSalesPersonTitle.Text, txtTermCondition1.Text, txtTermCondition2.Text,
                     txtTermCondition3.Text, txtTermCondition4.Text,
                     txtTermCondition5.Text, txtTermCondition6.Text,
                     PaperSize, NumOfCopies);

                //Save Hardware Info
                SaveHardware();

                #region *) Save: AppSetting values
                try
                {
                    if (txtReceiptLocation.Text != "")
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.ReceiptFileLocation, txtReceiptLocation.Text);

                    if (txtClosingReceipt.Text != "")
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.ClosingReceiptFileLocation, txtClosingReceipt.Text);

                    if (txtClosingNumOfCopies.Text != "")
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.ClosingReceiptNumOfCopies, txtClosingNumOfCopies.Text);

                    if (txtFormalReceipt.Text != "")
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.FormalReceiptFileLocation, txtFormalReceipt.Text);

                    if (txtPrinterName.Text != "")
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.PrinterName, txtPrinterName.Text);
                    if (cbRealTimeSales.Checked)
                    {
                        AppSetting.SetSetting("IsRealTimeSales", "1");
                    }
                    else
                    {
                        AppSetting.SetSetting("IsRealTimeSales", "0");
                    }
                    if (cboLanguage.SelectedIndex >= 0)
                    {
                        string value = string.Empty;
                        switch(cboLanguage.Text)
                        {
                            case "ENGLISH":
                                value = "en-US";
                                break;
                            case "CHINESE":
                                value = "zh-CN";
                                break;
                        }
                        AppSetting.SetSetting(AppSetting.SettingsName.CultureCode, value);
                    }

                    AppSetting.SetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen, UseSecondScreen.Checked.ToString());
                    AppSetting.SetSetting(AppSetting.SettingsName.SecondScreen.MarqueeText, MarqueeText.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder, SlideShowFolder.Text);

                    AppSetting.SetSetting(AppSetting.SettingsName.Print.UseAdditionalPrinter, UseAdditionalPrinter.Checked.ToString());
                    AppSetting.SetSetting(AppSetting.SettingsName.Print.AdditionalPrinter, cbAdditionalPrinter.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.Print.AdditionalReceipt, txtAdditionalReceipt.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.Payment.MaxBalancePayment, MaxBalancePayment.Text);
                    if (rbItemNamePrice.Checked)
                        AppSetting.SetSetting(AppSetting.SettingsName.Hotkeys.HotkeysDisplay, AppSetting.SettingsName.HotKeysItemNamePrice);
                    else
                        AppSetting.SetSetting(AppSetting.SettingsName.Hotkeys.HotkeysDisplay, AppSetting.SettingsName.HotKeysPriceItemName);

                    if (ShowMemberPointsBalance.Checked)
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt, false.ToString());
                    else
                        AppSetting.SetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt, true.ToString());

                    AppSetting.SetSetting(AppSetting.SettingsName.Receipt.ShowReceiptNoBarcode, Receipt_ShowReceiptBarcode.Checked.ToString());
                    //AppSetting.SetSetting(AppSetting.SettingsName.Receipt.BarcodeFontName, cbBarcodeFont.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.Receipt.BarcodeFontSize, txtBarcodeFontSize.Text);
                    
                }
                catch (Exception X)
                {
                    CommonUILib.HandleException(X);
                    return;
                }
                #endregion

                #region SICC
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SICC.CheckValidationPrinting), false))
                {
                    AppSetting.SetSetting(AppSetting.SettingsName.SICC.DefaultPrinterName, cbValidationPrinter.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.SICC.ValidationMode, txtValidationName.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.SICC.ValidationX, txtValidationX.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.SICC.ValidationY, txtValidationY.Text);
                    AppSetting.SetSetting(AppSetting.SettingsName.SICC.ValidationNumOfCopies, txtValidationNumOfCopies.Text);
                    
                }
                #endregion

                ReceiptSetup.LoadReceiptSetup();
                PointOfSaleController.GetPointOfSaleInfo();
                //MessageBox.Show("Save successful");

                if (isPOSIDChanged)
                {
                    DialogResult dialogResult = MessageBox.Show("Do you want to clear and redownload all data for this POS ?", "POS Setup Warning", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string status;
                        if (!SyncRealTimeController.UpdateModifiedOnPOSSetup(out status))
                        {
                            MessageBox.Show(status);
                        }
                        else
                        {
                            MessageBox.Show("Save successfull");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Save successfull");
                    }
                }
                else
                {
                    MessageBox.Show("Save successfull");
                }
            }
            else
            {
                MessageBox.Show("Invalid Point Of Sale ID selected.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveHardware()
        {
            //TO DO: SAVE TO XML FILES > Receipt.xml, ReceiptPrinter.xml, PoleDisplay.xml
             AppSetting.SetSetting(AppSetting.SettingsName.Receipt.A4PrinterName,cbA4Printer.Text);
        }

        private void btnNETSID_Click(object sender, EventArgs e)
        {
            frmKeypad f = new frmKeypad();
        }

        private void frmPOSSetup_Load(object sender, EventArgs e)
        {
            //#region Hide License
            //tabControl1.TabPages.Remove(tabPage3);
            //#endregion
            #region Hide / Show Validation Printing for SICC
            gbValidationPrinting.Visible = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SICC.CheckValidationPrinting), false))
            {
                gbValidationPrinting.Visible = true;
                cbValidationPrinter.Text = AppSetting.GetSetting(AppSetting.SettingsName.SICC.DefaultPrinterName);
                txtValidationName.Text = AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationMode);
                txtValidationX.Text = AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationX);
                txtValidationY.Text = AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationY);
                txtValidationNumOfCopies.Text = AppSetting.GetSetting(AppSetting.SettingsName.SICC.ValidationNumOfCopies);

            }


            #endregion
        }

        private void btnSearchRpt_Click(object sender, EventArgs e)
        {
            if (txtReceiptLocation.Text != "")
                fdSelectReceipt.InitialDirectory = txtReceiptLocation.Text;

            fdSelectReceipt.ShowDialog();
        }

        private void fdSelectReceipt_FileOk(object sender, CancelEventArgs e)
        {
            txtReceiptLocation.Text = fdSelectReceipt.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtClosingReceipt.Text != "")
                fdSelectClosingReceipt.InitialDirectory = txtClosingReceipt.Text;

            fdSelectClosingReceipt.ShowDialog();
        }

        private void txtClosingReceipt_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void fdSelectClosingReceipt_FileOk(object sender, CancelEventArgs e)
        {
            txtClosingReceipt.Text = fdSelectClosingReceipt.FileName;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnSearchAdditionalReceipt_Click(object sender, EventArgs e)
        {
            if (txtAdditionalReceipt.Text != "")
                fdSelectAdditionalReceipt.InitialDirectory = txtAdditionalReceipt.Text;

            fdSelectAdditionalReceipt.ShowDialog();
        }

        private void fdSelectAdditionalReceipt_FileOk(object sender, CancelEventArgs e)
        {
            txtAdditionalReceipt.Text = fdSelectAdditionalReceipt.FileName;
        }
        
        private void MaxBalancePayment_TextChanged(object sender, EventArgs e)
        {
            decimal val = 0;
            TextBox txt = (TextBox)sender;
            if (!decimal.TryParse(txt.Text, out val))
                txt.Text = "0.00";
        }

        private void fdSelectFormalReceipt_FileOk(object sender, CancelEventArgs e)
        {
            txtFormalReceipt.Text = fdSelectFormalReceipt.FileName;
        }

        private void btnSearchFormalReceipt_Click(object sender, EventArgs e)
        {
            if (txtFormalReceipt.Text != "")
                fdSelectFormalReceipt.InitialDirectory = txtFormalReceipt.Text;

            fdSelectFormalReceipt.ShowDialog();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}