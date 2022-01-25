using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinUtility
{
    public partial class WinUtilityMain : Form
    {
        public WinUtilityMain()
        {
            InitializeComponent();
        }

        private void btnAdjustCostPrice_Click(object sender, EventArgs e)
        {
            frmCostPriceUpdater frm = new frmCostPriceUpdater();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
        }

        private void btnUpdateAdjustedInventory_Click(object sender, EventArgs e)
        {
            frmUpdateAdjustedInventory frm = new frmUpdateAdjustedInventory();
            frm.ShowDialog();
        }
    }
}
