using System.Globalization;
using System.Collections;
using PowerPOSLib.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmKeypadFC : Form
    {
        private Hashtable exchangeRate;
        private decimal rate;
        private decimal outstanding;

        public string value;
        public string initialValue;
        public string textMessage;
        public bool IsInteger;
        public decimal foreignCurrencyRate = 0;
        public string foreignCurrencyCode = "";
        public decimal foreignCurrencyAmount = 0;

        public frmKeypadFC()
        {
            InitializeComponent();
            IsInteger = false;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtEntry.Select();
            SendKeys.Send(((Button)sender).Text);
        }


        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!txtEntry.Text.Contains(".") & !IsInteger)
            {
                txtEntry.Focus();
                SendKeys.Send(((Button)sender).Text);
            }
        }

        private void btnCLEAR_Click(object sender, EventArgs e)
        {
            txtEntry.Text = "";
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            //txtEntry.Text = txtEntry.Text.Remove(txtEntry.Text.Length - 1);
            txtEntry.Select();
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            value = initialValue;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            decimal val;
            if (txtEntry.Text == "")
                return;
            val = decimal.Parse(txtEntry.Text);
            val = val + 1;
            txtEntry.Text = val.ToString();
            txtEntry.Select();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (txtEntry.Text == "")
                return;
            decimal val;
            val = decimal.Parse(txtEntry.Text);
            if (val >= 1)
            {
                val = val - 1;
            }
            txtEntry.Text = val.ToString();
            txtEntry.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            value = txtLocalAmount.Text;
            if (cmbExchange.SelectedIndex > 0)
            {
                foreignCurrencyCode = cmbExchange.SelectedValue.ToString();
                foreignCurrencyRate = rate;

                decimal localAmount;
                decimal.TryParse(txtLocalAmount.Text, out localAmount);
                if (localAmount > outstanding)
                    foreignCurrencyAmount = outstanding / rate;
                else
                    foreignCurrencyAmount = localAmount / rate;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmKeypad_Load(object sender, EventArgs e)
        {
            txtEntry.Text = initialValue;
            lblMessage.Text = textMessage;
            txtEntry.Focus();
            txtEntry.Select();

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
            
            ArrayList ar = new ArrayList();
            ar.Add(DefaultCurrencyCode);
            for (int i = 0; i < c.Count; i++)
            {
                ar.Add(c[i].CurrencyCode);
            }
            cmbExchange.DataSource = ar;
            cmbExchange.Refresh();

            decimal.TryParse(textMessage.Replace("O/S:", ""), out outstanding);
        }

        private void txtEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(this, new EventArgs());
            }
        }

        private void frmKeypad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtEntry_TextChanged(object sender, EventArgs e)
        {
            CalculateValue();
        }

        private void cmbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateValue();
        }
        private void CalculateValue()
        {
            txtForeignAmount.Visible = false;
            lblForeignAmount.Visible = false;

            rate = 1.0M;
            if (cmbExchange.SelectedIndex > 0)
            {
                if (exchangeRate != null)
                {
                    object obj = exchangeRate[cmbExchange.SelectedValue];
                    if (obj != null)
                        decimal.TryParse(obj.ToString(), out rate);
                }

                txtForeignAmount.Visible = true;
                lblForeignAmount.Visible = true;
            }

            decimal tmp = 0.0M;
            decimal.TryParse(txtEntry.Text, out tmp);

            txtLocalAmount.Text = (rate * tmp).ToString("N2");
            txtForeignAmount.Text = (outstanding / rate).ToString("N2");
            lblExchange.Text = rate.ToString("N6");
            txtEntry.Enabled = true;
            txtEntry.Select();
        }

    }
}