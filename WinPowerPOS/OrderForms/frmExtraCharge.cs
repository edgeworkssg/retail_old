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
    public partial class frmExtraCharge : Form
    {
        public decimal totalAmount = 0;
        public decimal extraCharge = 0;
        public decimal totalAmountAfterCharge = 0;
        public bool isConfirmed = false;
        public frmExtraCharge()
        {
            InitializeComponent();
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            isConfirmed = true;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmExtraCharge_Load(object sender, EventArgs e)
        {

            //Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));
            //e.Graphics.DrawLine(pen, 20, 10, 300, 100);

            lblTotalAmount.Text = totalAmount.ToString("N2");
            lblExtraCharge.Text = extraCharge.ToString("N2"); ;
            lblTotalAfterCharge.Text = totalAmountAfterCharge.ToString("N2");
        }
    }
}
