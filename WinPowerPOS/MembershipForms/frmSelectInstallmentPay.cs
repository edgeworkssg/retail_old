using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.MembershipForms
{
    public partial class frmSelectInstallmentPay : Form
    {
        public bool IsSuccesful = false;
        public bool IsCreditNote = false;

        public frmSelectInstallmentPay()
        {
            InitializeComponent();
        }

        private void btnCreditNote_Click(object sender, EventArgs e)
        {
            IsSuccesful = true;
            IsCreditNote = true;
            this.Close();
        }

        private void btnNormalPayment_Click(object sender, EventArgs e)
        {
            IsSuccesful = true;
            IsCreditNote = false;
            this.Close();
        }
    }
}
