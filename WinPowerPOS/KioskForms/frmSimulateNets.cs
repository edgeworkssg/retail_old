using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.KioskForms
{
    public partial class frmSimulateNets : Form
    {
        private IKiosk kiosk;

        private decimal amt = new decimal(0d);
        private string type = "";

        public frmSimulateNets(IKiosk kiosk)
        {
            InitializeComponent();

            this.kiosk = kiosk;
        }

        public void SetAmount(decimal amt)
        {
            this.amt = amt;
        }

        public void SetType(string type)
        {
            this.type = type;

            btnOK.Text = "Pay with " + type;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            kiosk.PayNonCash(amt, type);
            kiosk.EndSimulate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            kiosk.CancelNonCashSimulate();
        }
    }
}
