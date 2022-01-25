using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmRemark : Form
    {
        public bool IsSuccessful;
        public frmRemark()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            IsSuccessful = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

    }
}