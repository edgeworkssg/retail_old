using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using PowerInventory.InventoryForms;
using PowerInventory.ItemForms;
using System.Configuration;
using SubSonic;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;

namespace PowerInventory.InventoryForms
{
    public partial class frmStockOnHand: Form
    {                                
        ItemController itemLogic;
        public string searchQueryString;
        string existingConnectionString;
        #region "Form event handler"
        public frmStockOnHand()
        {
            Program.LoadCultureCode();
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
            
            InitializeComponent();            
            itemLogic = new ItemController();                        
            AssignPrivileges();
            if (PointOfSaleInfo.IntegrateWithInventory)
            {
                InventoryController.AssignStockOutToConfirmedOrderUsingTransaction(true);
            }
            
        }        
        
        private void OrderTaking_Activated(object sender, EventArgs e)
        {                        
  
        }
        
        private void OrderTaking_Load(object sender, EventArgs e)
        {
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
            if (!PointOfSaleInfo.IntegrateWithInventory)
            {
                pnlProgress.Visible = true;
                this.Enabled = false;
                bgDownload.RunWorkerAsync();
            }
            else
            {
                if (StockTakeController.IsThereUnAdjustedStockTake())
                {
                   
                    MessageBox.Show("WARNING! There is an unadjusted stock take!");
                   
                }   
            }
            //txtBarcode.Focus();                        
            dgvPurchase.AutoGenerateColumns = false;
            dgvPurchase.Columns[3].Visible = false;

            AttributesLabelCollection labels = new AttributesLabelCollection();
            DataTable dt = labels.Load().ToDataTable();
            foreach (DataRow row in dt.Rows)
            {
                string ColumnName = row["Label"].ToString();
                DataGridViewColumn dColumn = new DataGridViewColumn();
                dColumn.HeaderText = ColumnName;
                dColumn.DataPropertyName = "Attributes" + row["AttributesNo"].ToString();
                dColumn.CellTemplate = new DataGridViewTextBoxCell();
                dColumn.Width = 70;
                dColumn.Frozen = false;
                dgvPurchase.Columns.Insert(dgvPurchase.Columns.Count - 2, dColumn);
            }

            //bool showCostPrice = false;
            //bool.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCostOnStockOnHand), out showCostPrice);
            //dgvPurchase.Columns[3].Visible = showCostPrice;

            InventoryLocationCollection inv = new InventoryLocationCollection();
            inv.Where(PowerPOS.InventoryLocation.Columns.Deleted, false);
            inv.Load();
            InventoryLocation tmpInv = new InventoryLocation();
            tmpInv.InventoryLocationName = "ALL";
            inv.Insert(0, tmpInv);
            cmbLocation.DataSource = inv;
            cmbLocation.Refresh();
            cmbLocation.SelectedIndex = 0;            
            this.WindowState = FormWindowState.Maximized;
            txtItemName.Select();

            if (searchQueryString != null && searchQueryString != "")
            {
                txtItemName.Text = searchQueryString;
                populateOrderItemGridView();
            }
        }
        #endregion

        #region "Privileges Related"
        private void AssignPrivileges()
        {
         
        }
        #endregion

        #region "button handler"
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to quit?", "", MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {                
                this.Close();
            }
        }
        #endregion

        #region "Quick Access Buttons & Programmable Keyboard"
        private void frmOrderTaking_KeyDown(object sender, KeyEventArgs e)
        {
            mapKeyPress(e);
        }
        
        private void mapKeyPress(KeyEventArgs e)
        {
           
        }                        

        #endregion
                                       
        #region "DataGridView - Editing of qty in the cell"

        private void dgvPurchase_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {            
        }

        private void populateOrderItemGridView()
        {
            try
            {
                //show panel please wait...
                pnlProgress.Visible = true;
                dgvPurchase.DataSource = null;
                dgvPurchase.Refresh();
                inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                searchQuery = txtItemName.Text;
                bgSearch.RunWorkerAsync();
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_ + ex.Message);
            }
        }

        private bool GetValueFromRow(out int qty, DataGridViewCellEventArgs e)
        {
            qty = -1;
            
            if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), out qty) || qty < 0)
            {
                MessageBox.Show(LanguageManager.You_need_to_enter_a_non_negative_number_for_quantity);                
                dgvPurchase.CancelEdit();
                return false;
            }                        
            return true;
        }
       
        #endregion               
                            

        private void btnExit_Click(object sender, EventArgs e)
        {         
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
             
            DataTable dt = (DataTable)dgvPurchase.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonUILib.displayTransparent();
                fsdExportToExcel.ShowDialog();
                CommonUILib.hideTransparent();                
            }
            else
            {
                MessageBox.Show(LanguageManager.There_is_no_data_to_export);
            }            
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvPurchase, fsdExportToExcel.FileName);
            MessageBox.Show(LanguageManager.File_saved);
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            populateOrderItemGridView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string searchQuery;
        private int inventoryLocationID;

        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            //bool showCostPrice = false;
            //bool.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCostOnStockOnHand), out showCostPrice);
            //dgvPurchase.Columns[3].Visible = showCostPrice;
            
            DataTable dt = ReportController.FetchStockReport(searchQuery,
                    inventoryLocationID, false, "", "ItemNo", "ASC");            
            e.Result = dt;
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)e.Result;
                
                dgvPurchase.DataSource = dt;
                dgvPurchase.Refresh();
                object obj = dt.Compute("SUM(OnHand)", "");
                if (obj != null)
                {
                    lblTotal.Text = "Total: " + obj.ToString();
                }
                else
                {
                    lblTotal.Text = "Total: -";
                }

                obj = dt.Compute("COUNT(OnHand)", "");
                if (obj != null)
                {
                    lblItemCount.Text = "Total Item Count: " + obj.ToString();
                }
                else
                {
                    lblItemCount.Text = "Total: -";
                }

                pnlProgress.Visible = false;
                this.Enabled = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show(LanguageManager.There_is_no_data_for_the_given_search_criteria_);
                }
                if (!String.IsNullOrEmpty(txtItemName.Text))
                {
                    txtItemName.Focus();
                    txtItemName.SelectionStart = 0;
                    txtItemName.SelectionLength = txtItemName.Text.Length;
                    txtItemName.SelectAll();
                }

                bool showCostPrice = false;
                bool.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCostOnStockOnHand), out showCostPrice);
                dgvPurchase.Columns[3].Visible = showCostPrice;

                bool showUOM = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowUOM),false);
                dgvPurchase.Columns[4].Visible = showUOM;
                

                for (int i = 0; i < dgvPurchase.Columns.Count; i++)
                {
                    if (i == 2)
                    {
                        dgvPurchase.Columns[i].Width = 200;
                    }
                    else 
                    {
                        dgvPurchase.Columns[i].Width = 70;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bgSearch_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgb1.PerformStep();
        }

        private void dgvPurchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                frmEditItem f = new frmEditItem();
                f.ItemRefNo = dgvPurchase.Rows[e.RowIndex].Cells[1].Value.ToString();
                f.IsReadOnly = true;
                CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            }

            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                int inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                string itemno = dgvPurchase.Rows[e.RowIndex].Cells[1].Value.ToString();
               
                Item i = new Item(itemno);
                if (i.Userflag1 == true) 
                {
                    frmStockOnHandMatrix f = new frmStockOnHandMatrix(i.Attributes1, inventoryLocationID);
                    
                    CommonUILib.displayTransparent(); f.ShowDialog(); CommonUILib.hideTransparent();
                }   
            }
        }

        private void frmStockOnHand_FormClosed(object sender, FormClosedEventArgs e)
        {
            //DataService.GetInstance("PowerPOS").DefaultConnectionString = existingConnectionString;
        }

        private void bgDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //download inventoryhdr and inventorydet
                SyncClientController.Load_WS_URL();
                bool result = SyncClientController.GetCurrentInventoryRealTime();
                result = result && SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr();
                result = result && SyncClientController.GenerateInventoryHdrForAdjustedSales();
                e.Result = result;
            }
            catch (Exception ex)
            {
                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        MessageBox.Show("You need to download latest Items from Server, please go to Setup Menu and Sync.");
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
                if (StockTakeController.IsThereUnAdjustedStockTake())
                {

                    MessageBox.Show("WARNING! There is an unadjusted stock take!");

                }

                this.Enabled = true;
            }
        }

        private void txtItemName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void btnDeductSales_Click(object sender, EventArgs e)
        {
            
            
        }
    }
}