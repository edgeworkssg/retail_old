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
    public partial class frmConfirmAmount : Form
    {
        public bool ConfirmStatus;
        
        public frmConfirmAmount()
        {
            InitializeComponent();

            ConfirmStatus = false;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            ConfirmStatus = true;

            Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            ConfirmStatus = false;

            Close();
        }

        private void frmConfirmAmount_Load(object sender, EventArgs e)
        {
            
        }
    }
}
