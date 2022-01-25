using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmNETSManualInput : Form
    {
        public bool IsSuccess = false;
        private POSController pos;
        public decimal Amount = 0;
        public string PaymentTypeResult = "";

        string citiBankPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_Grouped) + "").Trim().ToUpper();
        string creditCardPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped) + "").Trim().ToUpper();

        public string PaymentType
        {
            set
            {
                lblPaymentType.Text = value;
            }
        }

        public frmNETSManualInput(POSController pos, string paymentType)
        {
            InitializeComponent();
            this.pos = pos;
            string status = "";
            Amount = pos.CalculateTotalAmount(out status);
            txtAmount.Text = Amount.ToString("N2");
            lblPaymentType.Text = paymentType;
            LoadPaymentType();
        }

        public frmNETSManualInput(POSController pos, decimal _amount, string paymentType)
        {
            InitializeComponent();
            this.pos = pos;
            string status = "";
            Amount = _amount;
            txtAmount.Text = Amount.ToString("N2");
            lblPaymentType.Text = paymentType;
            LoadPaymentType();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccess = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbPaymentType.Visible)
                PaymentTypeResult = cmbPaymentType.Text;
            else
                PaymentTypeResult = "";

            if (lblPaymentType.Text == POSController.PAY_NETSQR)
                PaymentTypeResult = POSController.PAY_NETSATMCard;

            Amount = txtAmount.Text.GetDecimalValue();
            IsSuccess = true;
            this.Close();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            var txt = (TextBox)sender;
            decimal val = 0;
            if (!decimal.TryParse(txt.Text, out val))
                txt.Text = "0.0";
        }

        private void LoadPaymentType()
        {
            List<string> listPaymentType = new List<string>();
            string visa = "";
            string master = "";
            string amex = "";
            string diners = "";
            string jcb = "";
            if (lblPaymentType.Text.Trim().ToLower().Equals(citiBankPayment.ToLower().Trim()))
            {
                visa = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_VISA);
                master = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_MASTER);
                amex = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_AMEX);
                diners = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_DINERS);
                jcb = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_JCB);
            }
            else if (lblPaymentType.Text.Trim().ToLower().Equals(creditCardPayment.ToLower().Trim()))
            {
                visa = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
                master = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER);
                amex = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX);
                //diners = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCC_DINERS);
                //jcb = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCC_JCB);
            }
            else
            {
                cmbPaymentType.Visible = false;
            }

            if (!string.IsNullOrEmpty(visa)) listPaymentType.Add(visa);
            if (!string.IsNullOrEmpty(master)) listPaymentType.Add(master);
            if (!string.IsNullOrEmpty(amex)) listPaymentType.Add(amex);
            if (!string.IsNullOrEmpty(diners)) listPaymentType.Add(diners);
            if (!string.IsNullOrEmpty(jcb)) listPaymentType.Add(jcb);

            cmbPaymentType.DataSource = listPaymentType;
            cmbPaymentType.Refresh();
        }

        private void frmNETSManualInput_Load(object sender, EventArgs e)
        {
            //LoadPaymentType();

        }
    }
}
