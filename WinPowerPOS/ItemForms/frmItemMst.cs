using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using PowerPOS.Setup;

namespace WinPowerPOS.ItemForms
{
    public partial class frmItemMst : Form
    {
        SetupController MasterController = null;

        #region "Form initialization and load"
        public frmItemMst()
        {
            InitializeComponent();

            MasterController = new ItemSetupController();
        }
        private void frmItemMst_Load(object sender, EventArgs e)
        {
            if (PrivilegesController.HasPrivilege("Product Additional", UserInfo.privileges))
            {
                dgvItems.Columns[Attributes1.Name].HeaderText = ProductAttributeInfo.Attributes1;
                dgvItems.Columns[Attributes2.Name].HeaderText = ProductAttributeInfo.Attributes2;
                dgvItems.Columns[Attributes3.Name].HeaderText = ProductAttributeInfo.Attributes3;
                dgvItems.Columns[Attributes4.Name].HeaderText = ProductAttributeInfo.Attributes4;
                dgvItems.Columns[Attributes5.Name].HeaderText = ProductAttributeInfo.Attributes5;
            }
            else
            {
                dgvItems.Columns[Attributes1.Name].Visible = false;
                dgvItems.Columns[Attributes2.Name].Visible = false;
                dgvItems.Columns[Attributes3.Name].Visible = false;
                dgvItems.Columns[Attributes4.Name].Visible = false;
                dgvItems.Columns[Attributes5.Name].Visible = false;
            }

            dgvItems.AutoGenerateColumns = false;

            ItemSetupController dt = new ItemSetupController();

            dgvItems.DataSource = dt.FetchAll();
            dgvItems.Refresh();

            displayData();
            tSearch.Select();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region "Search Data"
        private void btnSearch_Click(object sender, EventArgs e)
        {
            displayData();
        }

        private void displayData()
        {
            if (optBarcodeError.Checked)
            {
                dgvItems.DataSource = MasterController.FetchSpecial(ItemSetupController.SpecialCodes.Show_Error_Barcode, tSearch.Text);
                dgvItems.Refresh();
            }
            else
            {
                dgvItems.DataSource = MasterController.FetchAll(tSearch.Text);
                dgvItems.Refresh();
            }

            dgvItems.Refresh();
        }
        #endregion
        

        #region "Edit and delete items"
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            /*
            try
            {                
                if (e.RowIndex >= 0 && e.ColumnIndex > 0)
                {
                    frmEditItem myFrm = new frmEditItem();
                    myFrm.ItemRefNo = dgvItems.Rows[dgvItems.CurrentCell.RowIndex].Cells["ItemNo"].Value.ToString();
                    myFrm.ShowDialog();
                    myFrm.Dispose();                                                    
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */ 
        }

        private void dgvItems_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            try
            {                
                if (dgvItems.CurrentCell.ColumnIndex == 1 || dgvItems.CurrentCell.OwningRow.Selected)
                {
                    if (e.KeyCode == Keys.Back)
                    {
                        DialogResult d = MessageBox.Show("Are you sure you want to delete this item?","Warning", MessageBoxButtons.YesNo);
                        if (d == DialogResult.Yes)
                        {
                            string delItemNo = dgvItems.Rows[dgvItems.CurrentCell.RowIndex].Cells["ItemNo"].Value.ToString();
                            Item.Delete(delItemNo);
                            dgvItems.Rows.RemoveAt(dgvItems.CurrentCell.RowIndex);
                            System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\images\\" + delItemNo + ".jpg");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_OrderDet_Item"))
                {
                    MessageBox.Show("There are Cash Receipts that contain this item. Delete them first before you can delete this Item", "", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            */ 
        }
        #endregion

        
        private void tSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            frmEditItem f = new frmEditItem();
            f.ShowDialog();
            f.Dispose();
            displayData();
        }

        private void btnNewCategory_Click(object sender, EventArgs e)
        {
            frmAddItemCategory f = new frmAddItemCategory();
            f.ShowDialog();
            f.Dispose();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {


            if (dgvItems != null && dgvItems.Rows.Count > 0)
            {
                fsdExportToExcel.ShowDialog();
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }            
        }

        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvItems, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }

        private void dgvItems_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                Dictionary<string, object> UpdateList = new Dictionary<string, object>();
                for (int Counter = 0; Counter < dgvItems.Columns.Count; Counter++)
                {
                    if (string.IsNullOrEmpty(dgvItems.Columns[Counter].DataPropertyName)) continue;
                    UpdateList.Add(dgvItems.Columns[Counter].DataPropertyName, dgvItems[Counter, e.RowIndex].EditedFormattedValue);
                }

                MasterController.Update(UpdateList);
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Some error occured. Please contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(X);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                MasterController.SaveChanges();
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Some error occured. Please contact your administrator", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(X);
                }
            }
        }

        private void optBarcodeError_CheckedChanged(object sender, EventArgs e)
        {
            displayData();
        }
    }
}