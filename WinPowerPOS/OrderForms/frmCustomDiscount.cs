using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmCustomDiscount : Form
    {
        public bool IsSuccessful;
        public frmCustomDiscount()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            decimal test;
            if (decimal.TryParse(txtDisc.Text, out test) && test >= 0 && test <= 100)
            {
                IsSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid number between 0-100.", "", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}