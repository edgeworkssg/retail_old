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
    public partial class frmPrintReceipt : Form
    {
        public frmOrderTaking fOrderTaking;
        public bool isHaveEmail;

        public frmPrintReceipt()
        {
            InitializeComponent();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (fOrderTaking != null)
            {
                fOrderTaking.bufPrintReceipt = 1;
                fOrderTaking.EmailReceipt = false;
            }

            Hide();
        }

        private void btnNoPrint_Click(object sender, EventArgs e)
        {
            if (fOrderTaking != null)
            {
                fOrderTaking.bufPrintReceipt = 0;
                fOrderTaking.EmailReceipt = false;
            }

            Hide();
        }

        private void btnEmailReceipt_Click(object sender, EventArgs e)
        {
            if (fOrderTaking != null)
            {
                fOrderTaking.bufPrintReceipt = 0;
                fOrderTaking.EmailReceipt = true;
            }


            Hide();
        }

        private void frmPrintReceipt_Load(object sender, EventArgs e)
        {
            btnEmailReceipt.Visible = false;
            if (isHaveEmail)
                btnEmailReceipt.Visible = true;

        }
    }
}
