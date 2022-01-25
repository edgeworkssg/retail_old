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
    public partial class frmBalancePayment : Form
    {
        private POSController pos;
        public decimal FinalAmount
        {
            get
            {
                decimal val = 0;
                if (!decimal.TryParse(txtFinalAmount.Text, out val))
                    decimal.TryParse(lblOriginalAmount.Text, out val);
                return val;
            }
        }

        public frmBalancePayment(POSController pos)
        {
            InitializeComponent();
            this.pos = pos;
            string status = "";
            decimal roundAmt = this.pos.RoundTotalReceiptAmount();
            decimal actualAmt = this.pos.CalculateTotalAmount(out status);
            lblRoundingAmount.Text = roundAmt.ToString("N2");
            lblOriginalAmount.Text = actualAmt.ToString("N2");
        }

        private void txtFinalAmount_TextChanged(object sender, EventArgs e)
        {
            decimal val = 0;
            TextBox txt = (TextBox)sender;
            if (!decimal.TryParse(txt.Text, out val))
            {
                txt.Text = lblOriginalAmount.Text;
                decimal.TryParse(lblOriginalAmount.Text, out val);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string status = "";
            decimal roundAmt = this.pos.RoundTotalReceiptAmount();
            decimal actualAmt = this.pos.CalculateTotalAmount(out status);
            roundAmt = actualAmt - roundAmt;
            decimal maxVal = 0;
            decimal.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Payment.MaxBalancePayment) + "", out maxVal);

            if (roundAmt <= maxVal)
            {
                lblRoundingAmount.Text = roundAmt.ToString("N2");
                this.Close();
            }
            else
            {
                roundAmt = this.pos.RoundTotalReceiptAmount();
                actualAmt = this.pos.CalculateTotalAmount(out status);

                MessageBox.Show("Final amount not allowed");
                return;
            }
        }
    }
}
