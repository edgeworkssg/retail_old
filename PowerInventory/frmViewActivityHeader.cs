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
using System.Configuration;

namespace PowerInventory.InventoryForms
{
    public partial class frmViewActivityHeader : Form
    {
        ItemController itemLogic;
        public string searchQueryString;

        #region "Form event handler"
        string existingConnectionString;
        public frmViewActivityHeader()
        {
            existingConnectionString = DataService.GetInstance("POWERPOS").DefaultConnectionString;
            InitializeComponent();
            itemLogic = new ItemController();
            AssignPrivileges();
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
                cmbTransferTo.DataSource = inv;
                cmbTransferTo.Refresh();
                cmbTransferTo.SelectedIndex = 0;
                this.WindowState = FormWindowState.Maximized;
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
                UseStartDate = dtpStartDate.Checked;
                UseEndDate = dtpEndDate.Checked;
                StartDate = dtpStartDate.Value;
                EndDate = dtpEndDate.Value;
                remark = txtRemark.Text;
                refNo = txtRefno.Text;
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
        private string userName;
        private string movementType;
        private string searchQuery;
        private string remark;
        private string refNo;
        private string inventoryLocationID;
        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                DataTable dt = new DataTable();
                if (PointOfSaleInfo.IntegrateWithInventory)
                {
                    dt =
                    ReportController.FetchInventoryActivityHeaderWithRefNo
                    (UseStartDate, UseEndDate,
                     StartDate, EndDate, "%" + userName + "%",
                     inventoryLocationID, movementType, remark, "", "", "",
                     PrivilegesController.HasPrivilege("Goods Receive", UserInfo.privileges));
                }
                else
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.FetchInventoryActivityHeaderWithRefNo(UseStartDate, UseEndDate,
                     StartDate, EndDate, "%" + userName + "%",
                     inventoryLocationID, movementType, remark, refNo, "", "",
                     PrivilegesController.HasPrivilege("Goods Receive", UserInfo.privileges));
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
            try
            {
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;


                    //Show Inventory Activity Sheet?
                    if (dgvPurchase.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString().ToLower() == "stock in")
                    {

                        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        if (PointOfSaleInfo.IntegrateWithInventory)
                            inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                        else
                        {
                            byte[] data = ws.FetchInventoryControllerByRefNo(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                            if (myDataSet == null || myDataSet.Tables.Count < 2)
                            {
                                MessageBox.Show("Error downloading inventory. Please check your connection");
                                return;
                            }
                            if (!inv.LoadFromDataTableWithoutSetIsNew(myDataSet.Tables[0], myDataSet.Tables[1]))
                            {
                                MessageBox.Show("Failed loading data.");
                                return;
                            }
                        }

                        // Try to get PO Custom Ref No
                        string PurchaseOrderNo = "";
                        if (!string.IsNullOrEmpty(inv.getPurchaseOrderNo()))
                        {
                            PurchaseOdrController poc = new PurchaseOdrController(inv.getPurchaseOrderNo());
                            PurchaseOrderNo = string.IsNullOrEmpty(poc.GetCustomRefNo()) ? inv.getPurchaseOrderNo() : poc.GetCustomRefNo();
                        }

                        #region *) Get additional fields label
                        string customField1Label = "", customField2Label = "", customField3Label = "", customField4Label = "", customField5Label = "";
                        customField1Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField1Label);
                        customField2Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField2Label);
                        customField3Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField3Label);
                        customField4Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField4Label);
                        customField5Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField5Label);

                        string additionalCost1Label = "", additionalCost2Label = "", additionalCost3Label = "", additionalCost4Label = "", additionalCost5Label = "";
                        additionalCost1Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1Label);
                        additionalCost2Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2Label);
                        additionalCost3Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3Label);
                        additionalCost4Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4Label);
                        additionalCost5Label = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5Label);
                        #endregion

                        //frmStockIn.PrintStockInSheet
                        //    (inv, PurchaseOrderNo, inv.getSupplierName(), inv.GetFreightCharges().ToString("N2"),
                        //    inv.getDiscount().ToString("N2"), inv.getExchangeRate().ToString("N2"), "GOODS RECEIVE",
                        //    AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges),
                        //    customField1Label, customField2Label, customField3Label, customField4Label, customField5Label,
                        //    inv.InvHdr.CustomField1, inv.InvHdr.CustomField2, inv.InvHdr.CustomField3, inv.InvHdr.CustomField4, inv.InvHdr.CustomField5,
                        //    additionalCost1Label, additionalCost2Label, additionalCost3Label, additionalCost4Label, additionalCost5Label,
                        //    inv.InvHdr.AdditionalCost1.ToString("N2"), inv.InvHdr.AdditionalCost2.ToString("N2"), inv.InvHdr.AdditionalCost3.ToString("N2"), inv.InvHdr.AdditionalCost4.ToString("N2"),
                        //    inv.InvHdr.AdditionalCost5.ToString("N2"), inv.InvHdr.VendorInvoiceNo);

                        bool hideCostFromSettingAndPrivilege = PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges) && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideInventoryCost), false);
                        frmStockIn.PrintStockInSheet
                            (inv, PurchaseOrderNo, inv.getSupplierName(), inv.GetFreightCharges().ToString("N2"),
                            inv.getDiscount().ToString("N2"), inv.getExchangeRate().ToString("N2"), "GOODS RECEIVE",
                            AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), hideCostFromSettingAndPrivilege,
                            customField1Label, customField2Label, customField3Label, customField4Label, customField5Label,
                            inv.InvHdr.CustomField1, inv.InvHdr.CustomField2, inv.InvHdr.CustomField3, inv.InvHdr.CustomField4, inv.InvHdr.CustomField5,
                            additionalCost1Label, additionalCost2Label, additionalCost3Label, additionalCost4Label, additionalCost5Label,
                            inv.InvHdr.AdditionalCost1.ToString("N2"), inv.InvHdr.AdditionalCost2.ToString("N2"), inv.InvHdr.AdditionalCost3.ToString("N2"), inv.InvHdr.AdditionalCost4.ToString("N2"),
                            inv.InvHdr.AdditionalCost5.ToString("N2"), inv.InvHdr.VendorInvoiceNo);
                    }
                    else if (dgvPurchase.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString().ToLower() == "stock out")
                    {
                        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);

                        if (PointOfSaleInfo.IntegrateWithInventory)
                        {
                            inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());

                        }
                        else
                        {
                            byte[] data = ws.FetchInventoryControllerByRefNo(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                            if (myDataSet == null || myDataSet.Tables.Count < 2)
                            {
                                MessageBox.Show("Error downloading inventory. Please check your connection");
                                return;
                            }
                            if (!inv.LoadFromDataTableWithoutSetIsNew(myDataSet.Tables[0], myDataSet.Tables[1]))
                            {
                                MessageBox.Show("Failed loading data.");
                                return;
                            }
                        }
                        //inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());

                        frmStockOut.PrintStockOutSheet
                            (inv, new InventoryStockOutReason(inv.getStockOutReasonID()).ReasonName, "STOCK ISSUE",
                            AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges),
                            AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ChangePriceStockAdjIssue), false));
                    }
                    else if (dgvPurchase.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString().ToLower() == "transfer out")
                    {
                        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        string transferDest = "";
                        if (PointOfSaleInfo.IntegrateWithInventory)
                        {
                            inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            transferDest = InventoryController.GetTransferDestination(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                        }
                        else
                        {
                            byte[] data = ws.FetchInventoryControllerByRefNo(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                            if (myDataSet == null || myDataSet.Tables.Count < 2)
                            {
                                MessageBox.Show("Error downloading inventory. Please check your connection");
                                return;
                            }
                            if (!inv.LoadFromDataTableWithoutSetIsNew(myDataSet.Tables[0], myDataSet.Tables[1]))
                            {
                                MessageBox.Show("Failed loading data.");
                                return;
                            }
                            transferDest = ws.GetTransferDestination(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                        }

                        frmStockTransfer.PrintTransferSheet
                            (inv, transferDest,
                            "TRANSFER", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges));
                    }
                    else if (dgvPurchase.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString().ToLower() == "transfer in")
                    {
                        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);

                        string src = "";
                        string transferDest = "";
                        if (PointOfSaleInfo.IntegrateWithInventory)
                        {
                            src = InventoryController.GetSourceDestination(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            inv.LoadConfirmedInventoryController(src);
                            transferDest = InventoryController.GetTransferDestination(src);
                        }
                        else
                        {
                            src = ws.GetSourceDestination(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            byte[] data = ws.FetchInventoryControllerByRefNo(src);
                            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                            if (myDataSet == null || myDataSet.Tables.Count < 2)
                            {
                                MessageBox.Show("Error downloading inventory. Please check your connection");
                                return;
                            }
                            if (!inv.LoadFromDataTableWithoutSetIsNew(myDataSet.Tables[0], myDataSet.Tables[1]))
                            {
                                MessageBox.Show("Failed loading data.");
                                return;
                            }
                            transferDest = ws.GetTransferDestination(src);
                        }

                        frmStockTransfer.PrintTransferSheet
                            (inv, transferDest,
                            "TRANSFER", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges));
                    }
                    else if (dgvPurchase.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString().ToLower().Contains("adjustment"))
                    {

                        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        if (PointOfSaleInfo.IntegrateWithInventory)
                            inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                        else
                        {
                            byte[] data = ws.FetchInventoryControllerByRefNo(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                            if (myDataSet == null || myDataSet.Tables.Count < 2)
                            {
                                MessageBox.Show("Error downloading inventory. Please check your connection");
                                return;
                            }
                            if (!inv.LoadFromDataTableWithoutSetIsNew(myDataSet.Tables[0], myDataSet.Tables[1]))
                            {
                                MessageBox.Show("Failed loading data.");
                                return;
                            }
                        }

                        frmAdjustStock.PrintAdjustmentSheet
                        (inv, inv.GetMovementType(), "STOCK ADJUSTMENT",
                        AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges),
                        AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ChangePriceStockAdjIssue), false));
                    }
                    else if (dgvPurchase.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString().ToLower() == "return out")
                    {
                        InventoryController inv = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);

                        if (PointOfSaleInfo.IntegrateWithInventory)
                        {
                            inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());

                        }
                        else
                        {
                            byte[] data = ws.FetchInventoryControllerByRefNo(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());
                            DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                            if (myDataSet == null || myDataSet.Tables.Count < 2)
                            {
                                MessageBox.Show("Error downloading inventory. Please check your connection");
                                return;
                            }
                            if (!inv.LoadFromDataTableWithoutSetIsNew(myDataSet.Tables[0], myDataSet.Tables[1]))
                            {
                                MessageBox.Show("Failed loading data.");
                                return;
                            }
                        }
                        //inv.LoadConfirmedInventoryController(dgvPurchase.Rows[e.RowIndex].Cells["RefNo"].Value.ToString());

                        frmStockOutReturn.PrintStockReturnSheet
                            (inv, inv.getSupplierName(), "STOCK RETURN",inv.getVendorInvoiceNo(),
                            AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false), 
                            PrivilegesController.HasPrivilege("GOODS RECEIVE", UserInfo.privileges));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void frmViewActivityHeader_FormClosed(object sender, FormClosedEventArgs e)
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
                    MessageBox.Show("Error loading inventory from the web. Please download all data from server first.");
                    this.Close();
                }
                this.Enabled = true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }

        }
    }
}