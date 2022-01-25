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
    public partial class frmSimulateCashPayment : Form
    {
        private IKiosk kiosk;

        private string status;

        private decimal change;

        private decimal cash = new decimal(0);

        public frmSimulateCashPayment(IKiosk kiosk)
        {
            InitializeComponent();

            this.kiosk = kiosk;

            panel1.BringToFront();
        }

        private void btn1d_Click(object sender, EventArgs e)
        {
            cash = 1;

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
            
        }

        private void btn5d_Click(object sender, EventArgs e)
        {
            cash = 5;

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn10d_Click(object sender, EventArgs e)
        {
            cash = 10;

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn25d_Click(object sender, EventArgs e)
        {
            cash = 25;

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn50d_Click(object sender, EventArgs e)
        {
            cash = 50;

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn100d_Click(object sender, EventArgs e)
        {
            cash = 100;

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn1c_Click(object sender, EventArgs e)
        {
            cash = new decimal(0.01);

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn5c_Click(object sender, EventArgs e)
        {
            cash = new decimal(0.05);

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn10c_Click(object sender, EventArgs e)
        {
            cash = new decimal(0.1);

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn20c_Click(object sender, EventArgs e)
        {
            cash = new decimal(0.2);

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btn50c_Click(object sender, EventArgs e)
        {
            cash = new decimal(0.5);

            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!kiosk.PayCash(cash))
                panel2.BringToFront();
        }

        private void btnCollectChanged_Click(object sender, EventArgs e)
        {
            kiosk.ChangeInCash();
        }
    }
}
