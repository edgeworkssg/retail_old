using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.KioskForms
{
    public partial class frmMessage : Form
    {
        public frmMessage(string title, string message)
        {
            InitializeComponent();

            this.label1.Text = title;
            this.label2.Text = message;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
