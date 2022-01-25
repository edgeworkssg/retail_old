using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POSDevices;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmWaitForDrawer : Form
    {
        public string lblMessage;
        public string change;
        public frmWaitForDrawer()
        {
            InitializeComponent();
        }

        private void frmErrorMessage_Load(object sender, EventArgs e)
        {
            lblChange1.Text = change;
            bwCashDrawer.RunWorkerAsync();
        }

        private void btnOK_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.Enter)
            {
                return; 
            }*/
        }

        private void bwCashDrawer_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isDrawerOpened = true;
            Char ch;
            //Logger.writeLog("Drawer State :" + isDrawerOpened.ToString());
            while (isDrawerOpened)
            {
                FlyTechCashDrawer c = new FlyTechCashDrawer();
                isDrawerOpened = c.isCashDrawerOpen(out ch);
                //Logger.writeLog("state :" + ch.ToString());
                //Logger.writeLog("Drawer State :" + isDrawerOpened.ToString());
            }
        }

        private void bwCashDrawer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Logger.writeLog("Worker Finished");
            this.Close();
            this.DialogResult = DialogResult.OK;
            
        }


    }
}
