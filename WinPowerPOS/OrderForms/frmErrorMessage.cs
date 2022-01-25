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
    public partial class frmErrorMessage : Form
    {
        public string lblMessage;
        public frmErrorMessage()
        {
            InitializeComponent();
        }

        private void btnSellVouchers_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRedeemVoucher_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnlVoucherPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmErrorMessage_Load(object sender, EventArgs e)
        {
            lblMsg.Text = lblMessage;
            btnTemp.Focus();
            
        }

        private void btnOK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                return; 
            }
        }
    }
}
