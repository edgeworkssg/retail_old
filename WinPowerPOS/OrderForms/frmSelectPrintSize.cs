using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POSDevices;

namespace WinPowerPOS.OrderForms
{
    public partial class frmSelectPrintSize : Form
    {
        public string PrintSize = "";

        public frmSelectPrintSize()
        {
            InitializeComponent();
        }

        private void btnA4_Click(object sender, EventArgs e)
        {
            PrintSize = "A4";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnReceipt_Click(object sender, EventArgs e)
        {
            PrintSize = "RECEIPT";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
