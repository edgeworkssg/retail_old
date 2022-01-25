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
    public partial class frmWaitConfirmSecondary : Form
    {
        public frmOrderSecondScreen fOrderSecondScreen;
        public frmWaitConfirmSecondary()
        {
            InitializeComponent();
        }

        private void frmWaitConfirmSecondary_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
            
        }

        public bool WaitConfirmSecondaryDialog()
        {
            frmConfirmAmount frm = new frmConfirmAmount();
            try
            {
                frm.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            catch
            {
                frm.Location = Screen.AllScreens[0].WorkingArea.Location;
            }
            if (fOrderSecondScreen != null)
            frm.ShowDialog(fOrderSecondScreen);

            return frm.ConfirmStatus;
        }

        private void frmWaitConfirmSecondary_Activated(object sender, EventArgs e)
        {
            
        }

        private void frmWaitConfirmSecondary_Shown(object sender, EventArgs e)
        {
            bool res = WaitConfirmSecondaryDialog();
            if (res)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
