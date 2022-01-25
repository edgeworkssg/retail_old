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
    public partial class frmOverallDiscount : Form
    {
        public POSController pos;
        public frmOverallDiscount()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDiscountTier1_Click(object sender, EventArgs e)
        {
            frmKeypad f = new frmKeypad();
            f.IsInteger = true;                        
            f.initialValue = txtDiscountTier1.Text;
            f.ShowDialog();
            //validate value
            decimal val = decimal.Parse(f.value);
            if (val < 0 || val > 100)
            {
                MessageBox.Show("The entered amount is invalid!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                txtDiscountTier1.Text = val.ToString("N2");
            
        }

        private void txtDiscountTier2_Click(object sender, EventArgs e)
        {
            frmKeypad f = new frmKeypad();
            f.IsInteger = true;
            f.initialValue = txtDiscountTier2.Text;
            f.ShowDialog();
            //validate value
            //validate value
            decimal val = decimal.Parse(f.value);
            if (val < 0 || val > 100)
            {
                MessageBox.Show("The entered amount is invalid!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtDiscountTier2.Text = val.ToString("N2");
        }
    }
}
