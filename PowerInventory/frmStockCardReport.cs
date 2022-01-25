using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using PowerPOS.Container;
using System.Configuration;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;


namespace PowerInventory
{
    public partial class frmStockCardReport : Form
    {
        string existingConnectionString;
        public frmStockCardReport()
        {
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
             
            InitializeComponent();
        }

        private void frmSearchInvoiceDet_Load(object sender, EventArgs e)
        {
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                this.Enabled = false;
                pnlProgress.Visible = true;
                bgDownload.RunWorkerAsync();
            }
            else
            {
                if (StockTakeController.IsThereUnAdjustedStockTake())
                {

                    MessageBox.Show("WARNING! There is an unadjusted stock take!");

                }   
            }
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
            dgvRcpt.AutoGenerateColumns = false;

            InventoryLocationCollection inv = new InventoryLocationCollection();
            inv.Where(PowerPOS.InventoryLocation.Columns.Deleted, false);
            inv.Load();
            InventoryLocation tmpInv = new InventoryLocation();
            tmpInv.InventoryLocationName = "ALL";
            inv.Insert(0, tmpInv);
            cmbLocation.DataSource = inv;
            cmbLocation.Refresh();
            
            //Populate combo boxes
            cmbCategory.DataSource = ItemController.FetchCategoryNames();
            cmbCategory.Refresh();            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private DateTime startDate;
        private DateTime endDate;
        private string searchQuery;
        private int locationID;
        private void BindGrid()
        {
            //show panel please wait...
            
            pnlProgress.Visible = true;
            dgvRcpt.DataSource = null;
            dgvRcpt.Refresh();
            locationID = ((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID;
            searchQuery = txtItemName.Text;
            
            bgSearch.RunWorkerAsync();
            this.Enabled = false;
            
            //Do search binding here....       
                        
        }
        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = new DataTable();
            if (PointOfSaleInfo.IntegrateWithInventory)
            {
                dt =
                ReportController.FetchStockCardReport(
                dtpStartDate.Value,
                dtpEndDate.Value,
                locationID,
                txtItemName.Text,"");
            }
            else
            {
                
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                byte[] data = ws.FetchStockCardReport(dtpStartDate.Value,
                dtpEndDate.Value, locationID, txtItemName.Text, "");
                DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                if (myDataSet.Tables.Count > 0)
                {
                    dt = myDataSet.Tables[0];
                    //dgvPurchase.Refresh();
                }
            }
            /*DataTable dt =
                ReportController.FetchStockCardReport(
                dtpStartDate.Value,
                dtpEndDate.Value,
                locationID,
                txtItemName.Text);*/
            e.Result = dt;
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = (DataTable)e.Result;
            dgvRcpt.DataSource = dt;
            dgvRcpt.Refresh();
            pnlProgress.Visible = false;
            this.Enabled = true;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show(LanguageManager.There_is_no_data_for_the_given_search_criteria_);
            }
        }

        private void bgSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb1.PerformStep();
        }

        

        private void dgvRcpt_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvRcpt.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonUILib.displayTransparent();fsdExportToExcel.ShowDialog();CommonUILib.hideTransparent();                
            }
            else
            {
                MessageBox.Show(LanguageManager.There_is_no_data_to_export);
            }            
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvRcpt, fsdExportToExcel.FileName);
            MessageBox.Show(LanguageManager.File_saved);
        }

        private void frmStockCardReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DataService.GetInstance("PowerPOS").DefaultConnectionString = existingConnectionString;
        }
        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download inventoryhdr and inventorydet
                SyncClientController.Load_WS_URL();
                //bool result = SyncClientController.GetCurrentInventoryRealTime();
                //result = result && SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                //result = result && SyncClientController.GenerateInventoryHdrForAdjustedSales();
                e.Result = true;
            }
            catch (Exception ex)
            {
                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        MessageBox.Show(LanguageManager.Error_loading_inventory_from_the_web__Please_download_all_data_from_server_first_);
                    }
                }
            }
        }
        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pnlProgress.Visible = false;
            if (!(bool)e.Result)
            {
                MessageBox.Show(LanguageManager.Error_loading_inventory_from_the_web__Please_check_your_internet_connection_);
                this.Close();
            }
            else
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                if (ws.IsThereUnAdjustedStockTake(PointOfSaleInfo.InventoryLocationID))
                {
                    MessageBox.Show("WARNING! There is an unadjusted stock take!");

                }

                this.Enabled = true;
            }
        }

        private void dgvRcpt_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}