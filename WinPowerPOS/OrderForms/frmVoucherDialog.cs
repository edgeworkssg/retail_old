using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmVoucherDialog : Form
    {
        public bool SellVoucher;
        public frmVoucherDialog()
        {
            InitializeComponent();
        }

        private void btnSellVouchers_Click(object sender, EventArgs e)
        {
            SellVoucher = true;
            this.Close();
        }

        private void btnRedeemVoucher_Click(object sender, EventArgs e)
        {
            SellVoucher = false;
            this.Close();
        }
    }
}
