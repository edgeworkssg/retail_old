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

namespace PowerInventory.InventoryForms
{
    public partial class frmViewActivityDetail: Form
    {                                
        ItemController itemLogic;
        public string searchQueryString;
        
        #region "Form event handler"
        string existingConnectionString;
        public frmViewActivityDetail()
        {
            try
            {
                existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
                InitializeComponent();
                itemLogic = new ItemController();
                AssignPrivileges();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }        
        private void OrderTaking_Activated(object sender, EventArgs e)
        {                        

        }
        private void OrderTaking_Load(object sender, EventArgs e)
        {
            try
            {
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    this.Enabled = false;
                    pnlProgress.Visible = true;
                    bgDownload.RunWorkerAsync();
                }
                CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);

                //txtBarcode.Focus();                        
                dgvPurchase.AutoGenerateColumns = false;
                InventoryLocationCollection inv = new InventoryLocationCollection();
                inv.Where(PowerPOS.InventoryLocation.Columns.Deleted, false);
                inv.Load();
                cmbMovementType.SelectedIndex = 0;
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
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }
        #endregion

        #region "Privileges Related"
        private void AssignPrivileges()
        {
            /*
            if (!PrivilegesController.HasPrivilege(PrivilegesController.INVENTORY_TRANSACTION, UserInfo.privileges))
            {                
                txtBarcode.Enabled = false;
            }*/
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
            /*
            if (POSController.ENABLE_PROGRAMMABLE_KEYBOARD)
            {
                try
                {
                    /*           
                    if (e.KeyValue > 111 & e.KeyValue < 120)
                    {
                        int tmp = e.KeyValue;
                        tmp = tmp - 111;    //112 = 1, 113 = 2 and etc
                        string buttonName = "F" + tmp.ToString();
                        DataRow[] dr;
                        dr = dtHotKeys.Select("keycode='" + buttonName + "'");
                        if (dr != null && dr.Length > 0)
                        {
                            if (!pos.AddItemToOrder(new Item(dr[0]["itemno"].ToString()), ref myHdr, ref myDet, true, out status))
                            {
                                //alert error message.                    
                                MessageBox.Show("Error: " + status);
                            }
                            populateOrderItemGridView();
                        }
                    } 
                    if (e.KeyCode != Keys.ShiftKey 
                        && e.KeyCode != Keys.ControlKey 
                        && ((int)e.KeyCode) != 18) //alt
                    {
                        int index = 0;

                        String newItemno = hotKeyCtrl.GetItemNo(e);
                        if (newItemno == "")
                        {
                            //do nothin' it is not a registered key
                        }
                        else if (newItemno == ProgrammableKeyboardController.FUNCTION_CODE)
                        {
                            string functionName = hotKeyCtrl.GetFunctionKey(e);
                            switch (functionName)
                            {
                                case ProgrammableKeyboardController.CHANGE_PRICE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;

                                        dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells["Price"];
                                        dgvPurchase.BeginEdit(true);

                                        e.SuppressKeyPress = true;
                                    }
                                    break;
                                case ProgrammableKeyboardController.CHANGE_DISCOUNT:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;

                                        dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells["Discount"];
                                        dgvPurchase.BeginEdit(true);

                                        e.SuppressKeyPress = true;
                                    }
                                    break;
                                case ProgrammableKeyboardController.CHANGE_QTY:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;

                                        dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells["Quantity"];
                                        dgvPurchase.BeginEdit(true);

                                        e.SuppressKeyPress = true;
                                    }
                                    break;
                                case ProgrammableKeyboardController.CHANGE_TOTAL_DISCOUNT:
                                    //txtDiscount.Focus();
                                    break;
                                case ProgrammableKeyboardController.CHANGE_TOTAL_TAX:
                                    //txtTax.Focus();
                                    break;
                                case ProgrammableKeyboardController.SCAN_BARCODE:
                                    txtBarcode.Focus();
                                    break;
                                case ProgrammableKeyboardController.DELETE_ORDER_LINE:
                                    dgvPurchase_KeyPress(this, new KeyPressEventArgs((char)Keys.Back));
                                    e.SuppressKeyPress = true;
                                    break;
                                case ProgrammableKeyboardController.DOWN_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;
                                        if (index < dgvPurchase.Rows.Count - 1)
                                        {
                                            index += 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells[dgvPurchase.SelectedCells[0].ColumnIndex];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.UP_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].RowIndex;
                                        if (index > 0)
                                        {
                                            index -= 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[index].Cells[dgvPurchase.SelectedCells[0].ColumnIndex];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.LEFT_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].ColumnIndex;
                                        if (index > 0 && dgvPurchase.Columns[index - 1].Visible == true)
                                        {
                                            index -= 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[dgvPurchase.SelectedCells[0].RowIndex].Cells[index];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.RIGHT_ORDER_LINE:
                                    if (dgvPurchase.SelectedCells.Count > 0)
                                    {
                                        index = dgvPurchase.SelectedCells[0].ColumnIndex;
                                        if (index < dgvPurchase.Columns.Count - 1 && dgvPurchase.Columns[index + 1].Visible == true)
                                        {
                                            index += 1;
                                            dgvPurchase.CurrentCell = dgvPurchase.Rows[dgvPurchase.SelectedCells[0].RowIndex].Cells[index];
                                            e.SuppressKeyPress = true;
                                        }
                                    }
                                    break;
                                case ProgrammableKeyboardController.CONFIRM_ORDER:
                                    btnCashPayment_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CANCEL_ORDER:
                                    btnCancel_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CASH_RECORDING:
                                    btnCashRec_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CLOSE_COUNTER:
                                    btnCheckOut_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.CLOSE_CASH_BILL:
                                    btnLogOut_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.STOCK_IN:
                                   /* frmInventoryReceive frm = new frmInventoryReceive();
                                    frm.ShowDialog();
                                    frm.Dispose();
                                    btnStockIn_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.STOCK_OUT:
                                    /* frmInventoryReceive frm = new frmInventoryReceive();
                                     frm.ShowDialog();
                                     frm.Dispose();
                                    btnStockOut_Click(this, new EventArgs());                                     
                                    break;
                                case ProgrammableKeyboardController.OPEN_INVENTORY:
                                    /* frmInventoryReceive frm = new frmInventoryReceive();
                                     frm.ShowDialog();
                                     frm.Dispose();
                                    btnBalQty_Click(this, new EventArgs());
                                    break;
                            }
                        }
                        else if (pos.AddItemToOrder(new Item(newItemno),1, PreferedDiscount,true, out status))
                        {
                            populateOrderItemGridView();
                            e.SuppressKeyPress = true;
                            for (int l = 0; l < dgvPurchase.Rows.Count; l++)
                            {
                                if (dgvPurchase.Rows[l].Cells["ItemNo"].Value.ToString() == newItemno)
                                {
                                    dgvPurchase.CurrentCell = dgvPurchase.Rows[l].Cells[1];
                                }
                            }
                            txtBarcode.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Error: " + status);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                }

            }*/
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
                inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID.ToString();
                movementType = cmbMovementType.SelectedItem.ToString();
                userName = txtUserName.Text; 
                searchQuery = txtItemName.Text;
                UseStartDate = dtpStartDate.Checked;
                UseEndDate = dtpEndDate.Checked;
                StartDate = dtpStartDate.Value;
                EndDate = dtpEndDate.Value;
                refNo = txtRefNo.Text;
                bgSearch.RunWorkerAsync();
                this.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.");
                
            }
        }
        private bool GetValueFromRow(out int qty, DataGridViewCellEventArgs e)
        {
            try
            {
                qty = -1;

                if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), out qty) || qty < 0)
                {
                    MessageBox.Show("You need to enter a non negative number for quantity");
                    dgvPurchase.CancelEdit();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
                qty = 0;
                return false;
            }
        }
       
        #endregion                                          

        private void btnExit_Click(object sender, EventArgs e)
        {         
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dgvPurchase.DataSource;
                if (dt != null && dt.Rows.Count > 0)
                {
                    CommonUILib.displayTransparent(); fsdExportToExcel.ShowDialog(); CommonUILib.hideTransparent();
                }
                else
                {
                    MessageBox.Show("There is no data to export");
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }

        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                ExportController.ExportToExcel(dgvPurchase, fsdExportToExcel.FileName);
                MessageBox.Show("File saved");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }
        }
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            populateOrderItemGridView();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool UseStartDate, UseEndDate;
        private DateTime StartDate, EndDate;
        private string userName, refNo;
        private string movementType;
        private string searchQuery;
        private string inventoryLocationID;
        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    dt =
                        ReportController.FetchInventoryActivityReportWithTransferAndRefNo
                        (UseStartDate, UseEndDate,
                         StartDate, EndDate, "%" + searchQuery + "%", "%" + userName + "%",
                         inventoryLocationID, movementType, txtRemark.Text, txtLineRemark.Text, txtRefNo.Text, "", "");
                }
                else
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.FetchInventoryActivityReportWithTransferAndRefNo(UseStartDate, UseEndDate,
                         StartDate, EndDate, "%" + searchQuery + "%", "%" + userName + "%",
                         inventoryLocationID, movementType, txtRefNo.Text, txtRemark.Text, txtLineRemark.Text);
                    DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                    if (myDataSet.Tables.Count > 0)
                    {
                        dt = myDataSet.Tables[0];
                    }
                }
                e.Result = dt;
            }
            catch (Exception ex)
            {                
                Logger.writeLog(ex);
                e.Result = null;
            }

        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)e.Result;
                dgvPurchase.DataSource = dt;
                dgvPurchase.Refresh();
                pnlProgress.Visible = false;
                this.Enabled = true;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("There is no data for the given search criteria.");
                }
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
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
                //Show Inventory Activity Sheet?

                /*
                frmEditItem f = new frmEditItem();
                f.ItemRefNo = dgvPurchase.Rows[e.RowIndex].Cells[1].Value.ToString();
                f.IsReadOnly = true;
                CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();*/
            }
        }

        private void frmViewActivityDetail_FormClosed(object sender, FormClosedEventArgs e)
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
                e.Result = true;
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                Logger.writeLog(ex);

                // if FK conflict then throw error
                if (ex.GetType().ToString() == "System.Data.SqlClient.SqlException")
                {
                    if (((System.Data.SqlClient.SqlException)ex).Number == 547)
                    {
                        MessageBox.Show("You need to download latest Items from Server, please go to Setup Menu and Sync.");
                    }
                }

                e.Result = false;
            }

        }
        private void bgDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                pnlProgress.Visible = false;
                if (!(bool)e.Result)
                {
                    MessageBox.Show("Error loading inventory from the web. Please download all data from the web.");
                    this.Close();
                }
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);                
            }

        }
             
    }
}