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
    public partial class frmWrapMessage : Form
    {
        private string title;

        private string message;

        public frmWrapMessage(string title, string message)
        {
            InitializeComponent();

            Opacity = 0.9f;

            this.title = title;
            this.message = message;
        }

        private void frmWrapMessage_Shown(object sender, EventArgs e)
        {
            frmMessage frm = new frmMessage(title, message);
            DialogResult dlg = frm.ShowDialog();
            if (dlg == DialogResult.Cancel)
                this.Close();
        }
    }
}
