using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace WinPowerPOS
{
    public partial class frmRedeemVoucher : Form
    {
        public bool IsSuccessful;
        public POSController pos;
        public Voucher myVoucher;
        public frmRedeemVoucher()
        {
            IsSuccessful = false;
            InitializeComponent();
            txtVoucherNo.Focus();
            myVoucher = null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void txtVoucherNo_KeyDown(object sender, KeyEventArgs e)
        {
            //handle enter
            if (e.KeyCode == Keys.Enter) 
            {
                Voucher v = new Voucher(Voucher.Columns.VoucherNo, txtVoucherNo.Text);
                if (v.IsLoaded && !v.IsNew)
                {
                    if (v.ExpiryDate.HasValue && v.ExpiryDate.Value < DateTime.Now)
                    {
                        MessageBox.Show("Voucher has already expired on " + v.ExpiryDate.Value.ToString("dd MMM yyyy"));
                        return;
                    }
                    bool wantContinue = true;
                    if (v.VoucherStatusId == 2)
                    {
                        DialogResult dr = MessageBox.Show("Warning. This voucher has already been redeemed before. Do you like to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            wantContinue = false;
                    }
                    else if (v.VoucherStatusId == 0)
                    {
                        DialogResult dr = MessageBox.Show("Warning. This voucher has never been sold before. Do you like to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                            wantContinue = false;
                    }
                    if (!wantContinue)
                    {                        
                        return;
                    }
                    txtAmount.Text = v.Amount.ToString("N2");
                    btnOK.Enabled = true;
                    myVoucher = v;
                    IsSuccessful = true;
                }
                else
                {
                    MessageBox.Show("Voucher number does not exist.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}