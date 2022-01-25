using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using System.Collections;
using PowerPOS;
using SubSonic;
using LanguageManager = PowerInventory.Properties.InventoryLanguage;



namespace PowerInventory
{
    public partial class frmAddItemWithFilter : Form
    {
        public string supplierName;
        public string searchReq;
        public string criteria;
        public int invLocation;
        public int supplierID;
        ItemController itemLogic;

        public ArrayList itemNumbers;
        public ArrayList quantities;
        public ArrayList uoms;
        public ArrayList costPrices;
        public ArrayList convRate;

        private DataRow[] keptRows;

        #region "Form Initialization and loading"
        public frmAddItemWithFilter()
        {
            Program.LoadCultureCode();
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            if (string.IsNullOrEmpty(criteria)) criteria = "contains"; // contains | starts with | ends with | exact match
        }

        private void frmAddItemWithFilter_Load(object sender, EventArgs e)
        {
            lblSupplierName.Text = supplierName;
            txtFilter.Text = searchReq;
            GetShowAttrSetting();
            ShowHideAttrColumns();
            GetShowPrevSalesSetting();
            BindGrid();
        }
        #endregion

        private void GetShowAttrSetting()
        {
            try
            {
                string setting = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_clbAttributes);
                if (!string.IsNullOrEmpty(setting))
                {
                    string[] checkedIndex = setting.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in checkedIndex)
                    {
                        if (index.GetIntValue() < clbAttributes.Items.Count)
                            clbAttributes.SetItemChecked(index.GetIntValue(), true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveShowAttrSetting()
        {
            try
            {
                List<string> checkedIndex = new List<string>();
                foreach (int index in clbAttributes.CheckedIndices)
                {
                    checkedIndex.Add(index.ToString());
                }
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_clbAttributes, string.Join(",", checkedIndex.ToArray()));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowHideAttrColumns()
        {
            try
            {
                for (int i = 0; i < clbAttributes.Items.Count; i++)
                {
                    dgvItemList.Columns["colAttributes" + (i + 1).ToString()].Visible = clbAttributes.GetItemChecked(i);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetShowPrevSalesSetting()
        {
            try
            {
                chkShowSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_chkShowSales), false);
                txtNumOfDays.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_txtNumOfDays);
                if (txtNumOfDays.Text == "") txtNumOfDays.Text = "10";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveShowPrevSalesSetting()
        {
            try
            {
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_chkShowSales, chkShowSales.Checked.ToString());
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_txtNumOfDays, txtNumOfDays.Text);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowHidePrevSalesColumns()
        {
            try
            {
                if (chkShowSales.Checked)
                {
                    string numOfDays = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_txtNumOfDays);
                    if (string.IsNullOrEmpty(numOfDays)) numOfDays = "10";

                    DateTime startDate1, startDate2, startDate3, endDate1, endDate2, endDate3;
                    startDate1 = DateTime.Today.AddDays(numOfDays.GetIntValue() * -1);
                    startDate2 = DateTime.Today.AddDays(numOfDays.GetIntValue() * -2);
                    startDate3 = DateTime.Today.AddDays(numOfDays.GetIntValue() * -3);
                    endDate1 = startDate1.AddDays(numOfDays.GetIntValue()).AddSeconds(-1);
                    endDate2 = startDate2.AddDays(numOfDays.GetIntValue()).AddSeconds(-1);
                    endDate3 = startDate3.AddDays(numOfDays.GetIntValue()).AddSeconds(-1);

                    colSales1.HeaderText = startDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate1.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
                    colSales2.HeaderText = startDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate2.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();
                    colSales3.HeaderText = startDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim() + " - " + endDate3.ToString("d MMM yyyy").Replace(DateTime.Today.Year.ToString(), "").Trim();

                    //colSales1.HeaderText = numOfDays + " days";
                    //colSales2.HeaderText = numOfDays.GetIntValue() * 2 + " days";
                    //colSales3.HeaderText = numOfDays.GetIntValue() * 3 + " days";
                }
                colSales1.Visible = chkShowSales.Checked;
                colSales2.Visible = chkShowSales.Checked;
                colSales3.Visible = chkShowSales.Checked;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowAttr_Click(object sender, EventArgs e)
        {
            pnlAttributes.Visible = true;
        }

        private void btnAttrOK_Click(object sender, EventArgs e)
        {
            SaveShowAttrSetting();
            ShowHideAttrColumns();
            pnlAttributes.Visible = false;
        }

        private void btnAttrCancel_Click(object sender, EventArgs e)
        {
            pnlAttributes.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SaveShowPrevSalesSetting();

            // Keep items with order qty > 0
            if (dgvItemList.DataSource != null)
            {
                DataTable dt = (DataTable)dgvItemList.DataSource;
                keptRows = dt.Select("OrderQty > 0");
            }

            searchReq = txtFilter.Text;
            BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                itemLogic = new ItemController();

                string searchText;
                if (criteria.ToLower() == "contains")
                    searchText = "%" + searchReq.Replace("'", "''") + "%";
                else if (criteria.ToLower() == "starts with")
                    searchText = searchReq.Replace("'", "''") + "%";
                else if (criteria.ToLower() == "ends with")
                    searchText = "%" + searchReq.Replace("'", "''");
                else if (criteria.ToLower() == "exact match")
                    searchText = searchReq.Replace("'", "''");
                else
                    searchText = "%" + searchReq.Replace("'", "''") + "%";

                string numOfDays = txtNumOfDays.Text;
                if (string.IsNullOrEmpty(numOfDays)) numOfDays = "10";
                ShowHidePrevSalesColumns();

                string message = "";
                DataTable dt = new DataTable();
                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    SyncClientController.Load_WS_URL();
                    PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                    ws.Timeout = 100000;
                    ws.Url = SyncClientController.WS_URL;
                    byte[] data = ws.InventoryFetchItemWithFilterWithSupplierID(searchText, supplierID, numOfDays, PointOfSaleInfo.OutletName, invLocation, out message);
                    DataSet myDataSet = SyncClientController.DecompressDataSetFromByteArray(data);
                    dt = myDataSet.Tables[0];                
                }
                else { 
                    dt = InventoryController.InventoryFetchItemWithFilterWithSupplierID(searchText, supplierID, numOfDays, PointOfSaleInfo.OutletName, invLocation, out message);
                }                

                if (keptRows != null)
                {
                    foreach (DataRow keptRow in keptRows)
                    {
                        DataRow delrow = dt.Rows.Find(keptRow["ItemNo"]);
                        if (delrow != null) dt.Rows.Remove(delrow);

                        DataRow newrow = dt.NewRow();
                        newrow.ItemArray = keptRow.ItemArray;
                        dt.Rows.InsertAt(newrow, 0);
                    }
                }

                //item exist?
                dgvItemList.AutoGenerateColumns = false;
                this.dgvItemList.DataSource = dt;
                this.dgvItemList.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                itemNumbers = null;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pnlLoading.Visible = true;
            pnlLoading.BringToFront();

            try
            {
                itemNumbers = new ArrayList();
                quantities = new ArrayList();
                uoms = new ArrayList();
                costPrices = new ArrayList();
                convRate = new ArrayList();

                //Add item from the selected checkboxes into the gridview
                for (int i = 0; i < dgvItemList.Rows.Count; i++)
                {
                    if ((dgvItemList.Rows[i].Cells[colOrderQty.Name].Value + "").GetDecimalValue() > 0)
                    {
                        itemNumbers.Add(dgvItemList.Rows[i].Cells[colItemNo.Name].Value);
                        quantities.Add(dgvItemList.Rows[i].Cells[colOrderQty.Name].Value);
                        uoms.Add(dgvItemList.Rows[i].Cells[OrderUOM.Name].Value);
                        costPrices.Add(dgvItemList.Rows[i].Cells[colCost.Name].Value);
                        convRate.Add(dgvItemList.Rows[i].Cells[colConvRate.Name].Value);
                    }
                }

                pnlLoading.Visible = false;
                pnlLoading.SendToBack();

                this.Close();
            }
            catch (Exception ex)
            {
                pnlLoading.Visible = false;
                pnlLoading.SendToBack();
                Logger.writeLog(ex);
                MessageBox.Show(LanguageManager.An_unknown_error_has_been_encountered__Please_contact_your_system_administrator_, LanguageManager.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnSearch_Click(sender, e);
        }

        private void dgvItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            try
            {
                if (dgvItemList.Columns[e.ColumnIndex] == colOrderQty)
                {
                    decimal oldVal = (dgvItemList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value + "").GetDecimalValue();
                    frmKeypad frm = new frmKeypad();
                    frm.IsInteger = false;
                    frm.initialValue = oldVal.ToString();
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (string.IsNullOrEmpty(frm.value))
                        return;
                    decimal newVal = frm.value.GetDecimalValue();
                    dgvItemList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = newVal;
                }
                else if (dgvItemList.Columns[e.ColumnIndex].Name == OrderUOM.Name)
                {
                    string itemNo = dgvItemList.Rows[e.RowIndex].Cells[colItemNo.Name].Value + "";
                    
                    Supplier sup = new Supplier(Supplier.Columns.SupplierName, lblSupplierName.Text);
                    int supplierID = sup.IsNew ? 0 : sup.SupplierID;
                    string packingSize = dgvItemList.Rows[e.RowIndex].Cells[OrderUOM.Name].Value + "";
                    var data = PurchaseOrderController.FetchPackingSizeByItemNoAndSupplierNew(itemNo, supplierID);
                    if (data.Count == 0)
                        return;
                    frmSelectPackingSize frm = new frmSelectPackingSize(itemNo, supplierID, packingSize, data);
                    CommonUILib.displayTransparent();
                    frm.ShowDialog();
                    CommonUILib.hideTransparent();
                    if (!frm.IsSuccess)
                        return;
                    else
                    {
                        dgvItemList.Rows[e.RowIndex].Cells[OrderUOM.Name].Value = frm.SelectedPackingSizeName;
                        dgvItemList.Rows[e.RowIndex].Cells[colCost.Name].Value = frm.SelectedPackingSizeCost;
                        dgvItemList.Rows[e.RowIndex].Cells[colConvRate.Name].Value = frm.SelectedPackingSizeUOM;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void chkShowSales_CheckedChanged(object sender, EventArgs e)
        {
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.frmAddItemWithFilter_chkShowSales, chkShowSales.Checked.ToString());
            ShowHidePrevSalesColumns();
        }

        private void saveFileDialogExport_FileOk(object sender, CancelEventArgs e)
        {
            DataTable stock = (DataTable)dgvItemList.DataSource;
            
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgvItemList.Columns)
            {      
                if(col.Visible)
                    dt.Columns.Add(col.HeaderText);
            }

            for (int i = 0; i < stock.Rows.Count; i++)
            {
                DataRow dRow = dt.NewRow();
                DataRow r = stock.Rows[i];
                foreach (DataGridViewColumn col in dgvItemList.Columns)
                {
                    if (dgvItemList.Columns[col.Index].Visible)
                    {
                        //if(r[col.DataPropertyName].GetType() == typeof(string))
                        //    dRow[col.Index] = string.Format("{0}", stock.Rows[i][col.DataPropertyName].ToString()) ;
                        //else
                            dRow[dgvItemList.Columns[col.Index].HeaderText] = stock.Rows[i][col.DataPropertyName];
                    }
                }
                dt.Rows.Add(dRow);
            }

            string status = "";
            if(!Helper.ExportExcel(dt, saveFileDialogExport.FileName, out status))
            {
                MessageBox.Show(status, "Error");
            }
            else
                MessageBox.Show("Export successful.");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CommonUILib.displayTransparent(); saveFileDialogExport.ShowDialog(); CommonUILib.hideTransparent();
        }


    }
}
