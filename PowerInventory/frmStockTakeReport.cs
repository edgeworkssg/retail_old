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
using SubSonic;
using System.Configuration;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;
using PowerPOS.InventoryRealTimeController;

namespace PowerInventory
{
    public partial class frmStockTakeReport : Form
    {
        string existingConnectionString;
        public frmStockTakeReport()
        {
            Program.LoadCultureCode();
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
            
            InitializeComponent();
            dgvStock.AutoGenerateColumns = false;
        }

        private void frmStockTakeReport_Load(object sender, EventArgs e)
        {
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                this.Enabled = false;
                pnlProgress.Visible = true;
                bgDownload.RunWorkerAsync();
            } 
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);

            //
            InventoryLocationCollection inv = new InventoryLocationCollection();
            inv.Where(InventoryLocation.Columns.Deleted, false);
            inv.Load();            
            InventoryLocation tmp = new InventoryLocation();
            tmp.InventoryLocationID = 0;
            tmp.InventoryLocationName = "--SELECT--";
            inv.Insert(0, tmp);
            cmbLocation.DataSource = inv;
            cmbLocation.Refresh();            
        }
        private int LocationID;
        private bool useStartDate;
        private bool useEndDate;
        private DateTime startDate;
        private DateTime endDate;
        private string SearchText;
        private string takenBy;
        private string verifiedBy;

        private void BindGrid()
        {
            LocationID = ((InventoryLocation)cmbLocation.SelectedValue).InventoryLocationID;
            if (LocationID == 0)
            {
                MessageBox.Show(LanguageManager.Please_select_the_location_);
                cmbLocation.Focus();
                dgvStock.DataSource = null;
                dgvStock.Refresh();
                txtQtyDisc.Text = "";
                txtValueDisc.Text = "";
            }
            else 
            {
                useStartDate = dtpStartDate.Checked;
                useEndDate = dtpEndDate.Checked;
                startDate = dtpStartDate.Value;
                endDate = dtpEndDate.Value;
                SearchText = txtSearch.Text;
                takenBy = txtTakenBy.Text;
                verifiedBy = txtVerifiedBy.Text;
                
                //show panel please wait...
                pnlProgress.Visible = true;
                this.Enabled = false;
                bgSearch.RunWorkerAsync();
            }            
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //BindGrid();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        
        private void btnExport_Click(object sender, EventArgs e)
        {

            if (dgvStock != null && dgvStock.Rows.Count > 0)
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
            ExportController.ExportToExcel(dgvStock, fsdExportToExcel.FileName);
            MessageBox.Show(LanguageManager.File_saved);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void bgSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb1.PerformStep();
        }        
        
        private void dgvStock_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {



        }
        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt;
            dt = ReportController.FetchStockTakeReport
                (useStartDate, useEndDate,
                startDate, endDate,
                SearchText, LocationID,
                takenBy, verifiedBy,
                "StockTakeDate", "Desc");

            e.Result = dt;

        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = (DataTable)e.Result;

            dgvStock.DataSource = dt;
            dgvStock.Refresh();

            //Get The Sum
            if (dt.Rows.Count > 0)
            {

                txtValueDisc.Text = (decimal.Parse(dt.Compute("SUM(TotalDiscrepancyValue)", "").ToString())).ToString("N2");
                txtQtyDisc.Text = (decimal.Parse(dt.Compute("SUM(AdjustmentQty)", "").ToString())).ToString("0.####");
            }
            else
            {
                txtValueDisc.Text = "0.00";
                txtQtyDisc.Text = "0";
            }
            pnlProgress.Visible = false;
            this.Enabled = true;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show(LanguageManager.There_is_no_data_for_the_given_search_criteria_);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmStockTakeReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DataService.GetInstance("PowerPOS").DefaultConnectionString = existingConnectionString;
        }
        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download inventoryhdr and inventorydet
                SyncClientController.Load_WS_URL();
                SyncInventoryRealTimeController sync = new SyncInventoryRealTimeController();
                bool result = sync.GetRealTimeStockTake();
                //result = result & SyncClientController.GetStockTakeRealTime();
                e.Result = result;
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
            this.Enabled = true;
        }

    }
}
