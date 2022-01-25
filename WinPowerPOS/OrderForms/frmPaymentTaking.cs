using System.Globalization;
using System;
using PowerPOS;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POSDevices;
using PowerPOS.Container;
using System.Collections;
using PowerPOSLib.Controller;
using SubSonic;

namespace WinPowerPOS.OrderForms
{
    public partial class frmPaymentTaking : Form
    {
        public bool isSuccessful;
        public bool printReceipt;
        public decimal change;
        public decimal tenderAmt;
        private decimal amount;
        private bool IgnoreValidation;
        private decimal rate;
        private Hashtable exchangeRate;
        private TextBox focusedTextbox;
        public POSController pos;
        POSDevices.PriceDisplay myDsp;

        #region "Form init and load"
        private void PaymentTaking_Load(object sender, EventArgs e)
        {
            focusedTextbox = txtCashReceived;
            myDsp = new POSDevices.PriceDisplay();

            bool useWindcor = false;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.UseWindcor), false))
            {
                useWindcor = true;
                myDsp.useWindcor = true;
            }
            else
            {
                myDsp.useWindcor = false;
            }

            if (useWindcor)
            {
                myDsp.FirstLineCommand = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.FirstLineCommand);
                myDsp.SecondLineCommand = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.SecondLineCommand);
            }

            myDsp.ShowTotal(double.Parse(lblAmount.Text.ToString()));
            amount = decimal.Parse(lblAmount.Text.ToString());
            isSuccessful = false;
            IgnoreValidation = false;
            this.lblRefNo.Text = pos.GetUnsavedRefNo().Substring(13);
            lblNumOfItems.Text = pos.GetSumOfItemQuantity().ToString();

            //Load Currency
            CurrencyCollection c = new CurrencyCollection();
            c.Where(Currency.Columns.Deleted, false);
            c.Load();

            //Load Rates
            ExchangeRateController ex = new ExchangeRateController();
            ex.Load("ExchangeRate");
            exchangeRate = ex.GetHashTable();

            string DefaultCurrencyCode = AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency);
            if (DefaultCurrencyCode == null || DefaultCurrencyCode == "") DefaultCurrencyCode = "SGD";
            lblDefaultCurrency.Text = string.Format("(Default currency is {0})", DefaultCurrencyCode);
            List<string> ar = new List<string>();
            ar.Add(DefaultCurrencyCode);
            for (int i = 0; i < c.Count; i++)
            {
                if(!ar.Contains(c[i].CurrencyCode))
                    ar.Add(c[i].CurrencyCode);
            }
            cmbExchange.DataSource = ar;
            cmbExchange.Refresh();
            cmbExchange.SelectedIndex = 0;
            cmbExchange_SelectedIndexChanged(cmbExchange, new EventArgs());
            LoadQuickCash();

            try
            {
                string deffCurrSymbol = "$";
                Currency deffCurrData = new Currency(Currency.Columns.CurrencyCode, DefaultCurrencyCode);
                if (!deffCurrData.IsNew && !string.IsNullOrEmpty(deffCurrData.CurrencySymbol))
                    deffCurrSymbol = deffCurrData.CurrencySymbol;
                lblCurrSignTotal.Text = deffCurrSymbol;
                lblCurrSignReturn.Text = deffCurrSymbol;
            }
            catch (Exception ex1)
            {
                Logger.writeLog(ex1);
            }
        }
        public frmPaymentTaking()
        {
            InitializeComponent();

        }
        #endregion

        #region "Update and calculate change logic"
        private void captureSales()
        {
            try
            {
                //DISPLAY CHANGE
                decimal cashRcv = decimal.Parse(txtCashReceived.Text);
                decimal amt = amount;
                change = 0;

                string status;
                pos.DeleteAllReceiptLinePayment();
                var tempPos = pos.myOrderDet;
                bool addPaymentSuccessful;
                if (cmbExchange.SelectedIndex == 0)
                {
                    addPaymentSuccessful = pos.AddReceiptLinePayment(cashRcv, POSController.PAY_CASH, "",0,"",0, out change, out status);
                }
                else
                {
                    addPaymentSuccessful = pos.AddReceiptLinePayment(cashRcv, POSController.PAY_CASH + "-" + cmbExchange.SelectedValue, 
                        "", lblExchange.Text.GetDecimalValue(), 
                        cmbExchange.SelectedValue + "",
                        txtForeignAmount.Text.GetDecimalValue() - lblForeignCurrReturned.Text.GetDecimalValue(), 
                        out change, out status);
                }
                if (!addPaymentSuccessful)
                {
                    MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCashReceived.Text = "";
                    CalculateChange();
                    return;
                }
                else
                {
                    bool IsQtyInsufficient = false;
                    if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.RemoveZeroInventoryValidation), false))
                    {
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
                    }

                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    bool IsPointAllocationSuccess;
                    if (!pos.ConfirmOrder(true, out IsPointAllocationSuccess, out status))
                    {
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        MessageBox.Show( status, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    #region *) Magento Integration
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.UseMagentoFeatures), false))
                    {
                        if (!pos.SyncToMagento(out status))
                        {
                            if (status != "")
                                MessageBox.Show(status);
                        }
                    }
                    #endregion
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    //if (!IsPointAllocationSuccess)
                    //{ MessageBox.Show("Point is not updated!" + Environment.NewLine + status, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                    if (!IsQtyInsufficient)
                        if (!pos.ExecuteStockOut(out status))
                        {
                            MessageBox.Show
                                ("Error while performing Stock Out: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    myDsp.ShowChange((double)change);
                    lblChange.Text = change.ToString("N");

                }
                isSuccessful = true;

                //disable all buttons
                txtCashReceived.Enabled = false;
                cmbExchange.Enabled = false;
                btnOK.Enabled = false;
                //btn10.Enabled = false;
                //btn2.Enabled = false;
                //btn5.Enabled = false;
                //btn50.Enabled = false;
                flpQuickCash.Enabled = false;



                if (printReceipt)
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, decimal.Parse(lblChange.Text.Replace('$', '0')), false,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);


                //kick  
                if (pos.GetTotalSales() != 0)
                {
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

                this.Close();

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "Buttons handler"

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtCashReceived.Text == "")
            {
                txtCashReceived.Text = lblAmount.Text;
            }
            else if (decimal.Parse(txtCashReceived.Text) < amount)
            {
                MessageBox.Show("Amount is insufficient", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCashReceived.Focus();
                return;
            }
            else if (decimal.Parse(txtCashReceived.Text) > 0 && amount < 0)
            {

                MessageBox.Show("Add Payment Type Failed. For refund transaction, total paid should be less than 0", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCashReceived.Focus();
                return;
            }
            else if (amount < 0 && (decimal.Parse(txtCashReceived.Text) != amount))
            {

                MessageBox.Show("Add Payment Type Failed. For refund transaction, total paid must tally with refund amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCashReceived.Focus();
                return;
            }
            captureSales();
        }
        private void btnQuickCash_Click(object sender, EventArgs e)
        {
            string pressedAmount = ((Button)sender).Text.Replace('$', '0');
            if (focusedTextbox.Text == "")
            {
                focusedTextbox.Text = pressedAmount.GetDecimalValue().ToString("N");
            }
            else
            {
                decimal amount, current;
                if (decimal.TryParse(pressedAmount, out amount) &&
                    decimal.TryParse((focusedTextbox.Text), out current))
                {
                    focusedTextbox.Text = (current + amount).ToString("N");
                }
            }
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            focusedTextbox.Focus();
            SendKeys.Send(((Button)sender).Text);
        }


        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!focusedTextbox.Text.Contains("."))
            {
                focusedTextbox.Focus();
                SendKeys.Send(((Button)sender).Text);
            }
        }

        private void btnCLEAR_Click(object sender, EventArgs e)
        {
            focusedTextbox.Text = "0.0";
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if (focusedTextbox.Text.Length < 1) return;
            focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.Text.Length - 1);
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            decimal val;
            if (focusedTextbox.Text == "")
                return;
            val = decimal.Parse(focusedTextbox.Text);
            val = val + 1;
            focusedTextbox.Text = val.ToString();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (focusedTextbox.Text == "")
                return;
            decimal val;
            val = decimal.Parse(focusedTextbox.Text);
            if (val >= 1)
            {
                val = val - 1;
            }
            focusedTextbox.Text = val.ToString();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //IgnoreValidation = true;

            //DialogResult dr = MessageBox.Show("Are you sure you want to close?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            //{
                this.Close();
            //}
            //else
            //{
            //    return;
            //}

        }

        private void txtCashReceived_KeyDown(object sender, KeyEventArgs e)
        {

        }
        #endregion

        #region "Close Form"
        private void btnOKClose_Click(object sender, EventArgs e)
        {
            IgnoreValidation = true;
            this.Close();
        }
        #endregion

        private void frmPaymentTaking_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtCashReceived_Validating(object sender, CancelEventArgs e)
        {
            ep1.Clear();
            return;
        }

        private void txtCashReceived_TextChanged(object sender, EventArgs e)
        {
            CalculateChange();
        }

        private void CalculateChange()
        {
            decimal collected;
            if (decimal.TryParse(txtCashReceived.Text, out collected))
            {
                change = collected - amount;
                tenderAmt = collected;
                if (change > 0)
                {
                    lblChange.Text = change.ToString("N");
                }
                else
                {
                    lblChange.Text = "";
                }
            }
            else
            {
                lblChange.Text = "";
            }
        }

        private void txtCashReceived_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOk_Click(this, new EventArgs());
            }
            else if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(this, new EventArgs());
            }
        }

        private void txtForeignAmount_TextChanged(object sender, EventArgs e)
        {
            decimal tmp = 1.0M;
            if (decimal.TryParse(txtForeignAmount.Text, out tmp))
            {
                txtCashReceived.Text = (tmp * rate).ToString("N");
            }
            else
            {
                txtCashReceived.Text = "";
            }

            if (tmp > lblForeignCurrAmount.Text.GetDecimalValue())
            {
                lblForeignCurrReturned.Text = (tmp - lblForeignCurrAmount.Text.GetDecimalValue()).ToString("N");
            }
            else
            {
                lblForeignCurrReturned.Text = "-";
            }
        }

        private void cmbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            rate = 1.0M;
            if (cmbExchange.SelectedIndex == 0)
            {
                txtForeignAmount.Text = "";
                focusedTextbox = txtCashReceived;
                txtForeignAmount.Enabled = false;
                txtCashReceived.Enabled = true;
                lblDefaultCurrency.Visible = false;
                txtCashReceived.Select();
                lblExchange.Text = "-";
                lblForeignCurrAmount.Text = "-";
                lblForeignCurrReturned.Text = "-";
            }
            else
            {
                focusedTextbox = txtForeignAmount;
                txtCashReceived.Enabled = false;
                txtForeignAmount.Enabled = true;
                lblDefaultCurrency.Visible = true;
                txtForeignAmount.Select();
                if (exchangeRate != null)
                {
                    object obj = exchangeRate[cmbExchange.SelectedValue];
                    if (obj != null)
                    {
                        decimal.TryParse(obj.ToString(), out rate);
                    }
                }
                decimal tmp = 0.0M;
                decimal.TryParse(txtForeignAmount.Text, out tmp);
                txtCashReceived.Text = (rate * tmp).ToString("N");

                lblExchange.Text = rate.ToString("N6");
                if (lblExchange.Text.GetDecimalValue() != 0)
                    lblForeignCurrAmount.Text = (lblAmount.Text.GetDecimalValue() / lblExchange.Text.GetDecimalValue()).ToString("N");
                else
                    lblForeignCurrAmount.Text = decimal.Parse("0.00").ToString("N");
            }
            Currency curr = new Currency(Currency.Columns.CurrencyCode, cmbExchange.SelectedValue);
            if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
            {
                lblCurrSignRate.Text = curr.CurrencySymbol;
                lblCurrSignAmount.Text = curr.CurrencySymbol;
                lblCurrSignForeignReturn.Text = curr.CurrencySymbol;
            }
        }

        private void LoadQuickCash()
        {
            try
            {
                string quickCashStr = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.QuickCashPayment);
                List<int> quickCashList = new List<int>();
                if (string.IsNullOrEmpty(quickCashStr))
                {
                    quickCashList.Add(2);
                    quickCashList.Add(5);
                    quickCashList.Add(10);
                    quickCashList.Add(50);
                }
                else
                {
                    var data = quickCashStr.Split(',');
                    foreach (var item in data)
                    {
                        if (item.GetIntValue() != 0 && !quickCashList.Contains(item.GetIntValue()))
                            quickCashList.Add(item.GetIntValue());
                    }
                    quickCashList.Sort();
                }

                flpQuickCash.Controls.Clear();
                for (int i = 0; i < quickCashList.Count; i++)
                {
                    Button btn = new Button();
                    btn.Margin = new Padding(0, 0, 0, 0);
                    btn.Size = new Size(130, 50);
                    btn.UseVisualStyleBackColor = true;
                    btn.ForeColor = Color.White;
                    btn.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    btn.CausesValidation = false;
                    btn.BackgroundImage = WinPowerPOS.Properties.Resources.greenbutton;
                    btn.Name = "btnQuickCash_" + i;
                    btn.Text = quickCashList[i].ToString("N");
                    btn.Click += new EventHandler(btnQuickCash_Click);
                    flpQuickCash.Controls.Add(btn);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
