using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinPowerPOS.MaintenanceForms
{
    public partial class frmSupport : Form
    {
        public frmSupport()
        {
            InitializeComponent();
        }

        private void btnBackupDB_Click(object sender, EventArgs e)
        {
            DbUtilityController.DoDBBackUp(Environment.CurrentDirectory + "\\Backup\\", "Regular Backup");
            MessageBox.Show("Completed!");

        }

        private void btnClearTrainingData_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ARE YOU SURE YOU WANT TO DELETE ALL ITEMS AND MEMBERSHIP DATA?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Logger.writeLog("Clear All Item and Membersihp Data");
                DbUtilityController.DoDBBackUp(Environment.CurrentDirectory + "\\Backup\\", "Before Clear Training Data");
                DbUtilityController.ClearAllItemAndMembership();
                MessageBox.Show("Completed");
            }
        }

        private void btnClearSalesAndInventory_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("ARE YOU SURE YOU WANT TO DELETE ALL SALES & INVENTORY DATA?", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                Logger.writeLog("Clear All Sales and Inventory");
                DbUtilityController.DoDBBackUp(Environment.CurrentDirectory + "\\Backup\\", "Before Clear Sales & Inventory");
                DbUtilityController.ClearSalesAndInventory();
                MessageBox.Show("Completed");
            }
        }

        private void btnViewDBLog_Click(object sender, EventArgs e)
        {
            frmViewDBLog f = new frmViewDBLog();
            f.ShowDialog();
            f.Dispose();
            
        }
    }
}
