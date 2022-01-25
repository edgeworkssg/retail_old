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
    public partial class frmCustomMessageBox : Form
    {

        public string choice = "cancel"; //default is cancel

        public frmCustomMessageBox(string formTitle, string label)
        {
            InitializeComponent();

            this.Text = formTitle;
            label1.Text = label;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            choice = "yes";
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            choice = "no";
            this.Close();
        }

    }
}
