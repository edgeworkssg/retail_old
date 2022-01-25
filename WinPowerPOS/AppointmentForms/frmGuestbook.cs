using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmGuestbook : Form
    {
        public frmGuestbook()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGuestbook_Load(object sender, EventArgs e)
        {
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                pnlProgress.Visible = true;
                this.Enabled = false;
                bgDownload.RunWorkerAsync();
            }

            BindGrid();
        }

        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download inventoryhdr and inventorydet
                SyncClientController.Load_WS_URL();
                bool result = SyncClientController.GetGuestbook(true);
                
                e.Result = result;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Error loading inventory from the web. Please check your internet connection.");
                Logger.writeLog(ex.Message);
                   
            }
        }

        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            pnlProgress.Visible = false;
            if (!(bool)e.Result)
            {
                MessageBox.Show("Error loading inventory from the web. Please check your internet connection.");
                this.Close();
            }
            else
            {
                this.Enabled = true;
            }
        }

        private void BindGrid()
        {
            DataTable dt = GuestBookController.GetGuestBookData(dtStart.Value, dtEnd.Value, txtSearch.Text);

            dgvPreview.DataSource = dt;
            dgvPreview.Refresh();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
