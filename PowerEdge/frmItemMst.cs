using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
namespace PowerEdge.ItemForms
{
    public partial class frmItemMst : Form
    {        

        #region "Form initialization and load"
        public frmItemMst()
        {
            InitializeComponent();
        }
        private void frmItemMst_Load(object sender, EventArgs e)
        {

            
            
            dgvItems.AutoGenerateColumns = false;
            cmbCategoryName.DataSource = ItemController.FetchCategoryNamesNonDeleted();
            cmbCategoryName.Refresh();
            
            displayData();
            txtBarcode.Select();
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
            DataTable dt =
                ItemController.FetchItems
                (txtRefNo.Text, txtItemName.Text,
                cmbCategoryName.SelectedValue.ToString(),
                txtBarcode.Text.Trim(), "", "",
                 "","","","","");
            dgvItems.DataSource = dt;
            dgvItems.Refresh();
        }
        #endregion
        

        #region "Edit and delete items"
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {                
                if (e.RowIndex >= 0 && e.ColumnIndex > 0)
                {
                                                                     
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvItems_KeyDown(object sender, KeyEventArgs e)
        {
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
        }
        #endregion

        

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
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
    }
}