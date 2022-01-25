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
using SubSonic;

namespace PowerInventory.InventoryForms
{
    public partial class frmCheckStockOnHandCorrectness: Form
    {                                
        ItemController itemLogic;
        public string searchQueryString;
        #region "Form event handler"
        public frmCheckStockOnHandCorrectness()
        {            
            InitializeComponent();            
            itemLogic = new ItemController();                        
            AssignPrivileges();            
        }        
        private void OrderTaking_Activated(object sender, EventArgs e)
        {                        

        }
        private void OrderTaking_Load(object sender, EventArgs e)
        {
            //txtBarcode.Focus();                        
            dgvPurchase.AutoGenerateColumns = false;
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
                                    CommonUILib.displayTransparent();frm.ShowDialog();CommonUILib.hideTransparent();
                                    frm.Dispose();
                                    btnStockIn_Click(this, new EventArgs());
                                    break;
                                case ProgrammableKeyboardController.STOCK_OUT:
                                    /* frmInventoryReceive frm = new frmInventoryReceive();
                                     CommonUILib.displayTransparent();frm.ShowDialog();CommonUILib.hideTransparent();
                                     frm.Dispose();
                                    btnStockOut_Click(this, new EventArgs());                                     
                                    break;
                                case ProgrammableKeyboardController.OPEN_INVENTORY:
                                    /* frmInventoryReceive frm = new frmInventoryReceive();
                                     CommonUILib.displayTransparent();frm.ShowDialog();CommonUILib.hideTransparent();
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
                inventoryLocationID = ((InventoryLocation)(cmbLocation.SelectedValue)).InventoryLocationID;
                searchQuery = txtItemName.Text;
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
            qty = -1;
            
            if (!int.TryParse(dgvPurchase.Rows[e.RowIndex].Cells["Quantity"].Value.ToString(), out qty) || qty < 0)
            {
                MessageBox.Show("You need to enter a non negative number for quantity");                
                dgvPurchase.CancelEdit();
                return false;
            }                        
            return true;
        }
       
        #endregion               
                    
        /*
        private void dgvPurchase_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 2)
            {

                int qty;
                decimal unitPrice;

                string ID = dgvPurchase.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                
                if (!GetValueFromRow(out qty, e))
                {                    
                    return;
                }
                frmKeypad t = new PowerInventory.frmKeypad();
                t.IsInteger = false;
                t.initialValue = qty.ToString();
                t.textMessage = "Set Qty:" + dgvPurchase.Rows[e.RowIndex].Cells["ItemName"].Value.ToString(); ;
                CommonUILib.displayTransparent();t.ShowDialog();CommonUILib.hideTransparent();
                if (t.value == "")
                {                    
                    return;
                }
                else
                {
                    qty = int.Parse(t.value);
                }                
                invCtrl.UpdateStockTake(int.Parse(ID), qty, UserInfo.username, "SYSTEM");
                populateOrderItemGridView();
                //updateTotalAmount();

                
                return;
            }
        }        
        private void btnKeypad_Click(object sender, EventArgs e)
        {
            frmKeypad t = new frmKeypad();
            t.IsInteger = true;
            t.textMessage = "Enter Item Barcode";
            CommonUILib.displayTransparent();t.ShowDialog();CommonUILib.hideTransparent();
            if (t.value == "")
            {                
                return;
            }
            else
            {
                SendKeys.Send("{ENTER}");
            }
        }
        */

        private void btnExit_Click(object sender, EventArgs e)
        {         
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
             
            DataTable dt = (DataTable)dgvPurchase.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonUILib.displayTransparent();fsdExportToExcel.ShowDialog();CommonUILib.hideTransparent();                
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }            
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvPurchase, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
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
            DataTable dt = ReportController.FetchStockReport(searchQuery,
                    inventoryLocationID,false, "", "ItemNo", "ASC");
            dt.Columns.Add("RemainingQty", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Diff", System.Type.GetType("System.Int32"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int balQty = -1;
                int remainingQty = 0;
                
                balQty = (int)dt.Rows[i]["OnHand"];
                
                Query qr = ViewInventoryActivity.CreateQuery();
                qr.AddWhere(ViewInventoryActivity.Columns.ItemNo, dt.Rows[i]["ItemNo"].ToString());                
                if (inventoryLocationID != 0) qr.AddWhere(ViewInventoryActivity.Columns.InventoryLocationID, inventoryLocationID);
                qr.AddWhere(ViewInventoryActivity.Columns.RemainingQty, Comparison.GreaterThan, 0);
                object obj = qr.GetSum(ViewInventoryActivity.Columns.RemainingQty);
                if (obj != null && obj is int)
                {
                    remainingQty = (int)obj;
                }
                else
                {
                    remainingQty = 0;
                }

                dt.Rows[i]["RemainingQty"] = remainingQty;

                dt.Rows[i]["Diff"] = balQty - remainingQty;
            }
            e.Result = dt;
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
        }

        private void dgvPurchase_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {            
        }        
    }
}