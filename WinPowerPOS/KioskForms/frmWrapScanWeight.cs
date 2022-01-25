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
    public partial class frmWrapScanWeight : Form
    {
        private IScanWeight main;

        public frmWrapScanWeight(IScanWeight main)
        {
            InitializeComponent();

            Opacity = 0.9f;

            this.main = main;
        }

        private void frmWrapScanWeight_Shown(object sender, EventArgs e)
        {
            frmScanWeight frm = new frmScanWeight();
            DialogResult dlg = frm.ShowDialog();
            if (dlg == DialogResult.OK)
            {
                main.SetWeightResult(frm.IsSuccess, frm.Weight);

                this.Close();
            }
            else if (dlg == DialogResult.Cancel)
            {
                Opacity = 0;

                main.CancelScanWeight();

                this.Close();
            }
        }
    }
}
