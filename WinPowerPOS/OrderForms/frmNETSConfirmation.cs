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
    public enum NETConfirmation
    {
        Retry,
        Manual,
        Cancel
    }
    public partial class frmNETSConfirmation : Form
    {
        public NETConfirmation Result = NETConfirmation.Cancel;
        private POSController pos;
        public decimal Amount;
        public string PaymentType;
        public bool isPartial;

        public string Status
        {
            set
            {
                lblStatus.Text = value;
            }
        }

        public frmNETSConfirmation(POSController pos)
        {
            InitializeComponent();
            this.pos = pos;
            Amount = 0;
            isPartial = false;
        }

        public frmNETSConfirmation(POSController pos, decimal _amount)
        {
            InitializeComponent();
            this.pos = pos;
            Amount = _amount;
            isPartial = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = NETConfirmation.Cancel;
            this.Close();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            Result = NETConfirmation.Retry;
            this.Close();
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (isPartial)
            {
                frmNETSManualInput frm = new frmNETSManualInput(pos, Amount, PaymentType);
                frm.PaymentType = PaymentType;
                frm.ShowDialog();
                if (frm.IsSuccess)
                {
                    Amount = frm.Amount;
                    Result = NETConfirmation.Manual;
                    this.Close();
                }
            }
            else
            {
                frmNETSManualInput frm = new frmNETSManualInput(pos, PaymentType);
                frm.PaymentType = PaymentType;
                frm.ShowDialog();
                if (frm.IsSuccess)
                {
                    Amount = frm.Amount;
                    Result = NETConfirmation.Manual;
                    this.Close();
                }
            }
        }

        private void frmNETSConfirmation_Load(object sender, EventArgs e)
        {
            if(PaymentType != "")
            {
                label1.Text = PaymentType + " Payment Failed";
            }
        }
    }
}
