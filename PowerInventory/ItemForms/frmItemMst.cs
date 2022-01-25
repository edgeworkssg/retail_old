using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Collections;
using System.Threading;
namespace PowerInventory.ItemForms
{
    public partial class frmItemMst : Form
    {        

        #region "Form initialization and load"
        public string searchQuery;
        public frmItemMst()
        {
            InitializeComponent();
        }
        private void frmItemMst_Load(object sender, EventArgs e)
        {

            lblAttributes1.Text = ProductAttributeInfo.Attributes1;
            lblAttributes2.Text = ProductAttributeInfo.Attributes2;
            lblAttributes3.Text = ProductAttributeInfo.Attributes3;
            lblAttributes4.Text = ProductAttributeInfo.Attributes4;
            lblAttributes5.Text = ProductAttributeInfo.Attributes5;

            if (PrivilegesController.HasPrivilege("Product Additional", UserInfo.privileges))
            {
                dgvItems.Columns["Attributes1"].HeaderText = ProductAttributeInfo.Attributes1;
                dgvItems.Columns["Attributes2"].HeaderText = ProductAttributeInfo.Attributes2;
                dgvItems.Columns["Attributes3"].HeaderText = ProductAttributeInfo.Attributes3;
                dgvItems.Columns["Attributes4"].HeaderText = ProductAttributeInfo.Attributes4;
                dgvItems.Columns["Attributes5"].HeaderText = ProductAttributeInfo.Attributes5;
            }
            else
            {
                dgvItems.Columns["Attributes1"].Visible = false;
                dgvItems.Columns["Attributes2"].Visible = false;
                dgvItems.Columns["Attributes3"].Visible = false;
                dgvItems.Columns["Attributes4"].Visible = false;
                dgvItems.Columns["Attributes5"].Visible = false;

                txtAttributes1.Enabled = false;
                txtAttributes2.Enabled = false;
                txtAttributes3.Enabled = false;
                txtAttributes4.Enabled = false;
                txtAttributes5.Enabled = false;
            }

            dgvItems.AutoGenerateColumns = false;
            cmbCategoryName.DataSource = ItemController.FetchCategoryNames();
            cmbCategoryName.Refresh();
            //cmbItemName.DataSource = ItemController.FetchItemNames();
            //cmbItemName.Refresh();
            //displayData();
            if (searchQuery != null && searchQuery != "")
            {
                txtSearch.Text = searchQuery;
                btnSearch_Click(this, new EventArgs());
            }
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
            //show panel please wait...
            pnlProgress.Visible = true;
            dgvItems.DataSource = null;
            dgvItems.Refresh();
            this.Enabled = false;
            bgSearch.RunWorkerAsync();
        }
        private DataTable getFromDB()
        {
            ItemController itemLogic = new ItemController();

            //find item using the given text            
            ViewItemCollection coll = itemLogic.SearchItem(txtSearch.Text,false,false);

            return coll.ToDataTable();            
        }
        #endregion
        

        #region "Edit and delete items"
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {                
                if (e.RowIndex >= 0 && e.ColumnIndex == 1)
                {
                    frmEditItem myFrm = new frmEditItem();
                    myFrm.ItemRefNo = dgvItems.Rows[dgvItems.CurrentCell.RowIndex].Cells["ItemNo"].Value.ToString();
                    CommonUILib.displayTransparent();myFrm.ShowDialog();CommonUILib.hideTransparent();
                    myFrm.Dispose();                                                    
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

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            frmEditItem f = new frmEditItem();
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();
            displayData();
        }

        private void btnNewCategory_Click(object sender, EventArgs e)
        {
            frmAddItemCategory f = new frmAddItemCategory();
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();

            //reload combo filter...
            cmbCategoryName.DataSource = ItemController.FetchCategoryNames();
            cmbCategoryName.Refresh();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {


            if (dgvItems != null && dgvItems.Rows.Count > 0)
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
            ExportController.ExportToExcel(dgvItems, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }

        private void btnDeleteChecked_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
            {
                return;
            }
            

            ArrayList ar = new ArrayList();

            //loop through and delete....
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                if (dgvItems.Rows[i].Cells[0].Value != null &&
                    dgvItems.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                {
                    Item.Delete(dgvItems.Rows[i].Cells["ItemNo"].Value);
                }
            }
            displayData();            
        }

        private void bgSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = getFromDB();
            e.Result = dt;
        }

        private void bgSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataTable dt = (DataTable)e.Result;
            dgvItems.DataSource = dt;
            dgvItems.Refresh();            
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


        private void llSelectAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                dgvItems.Rows[i].Cells[0].Value = true;
            }
        }

        private void llSelectNone_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                dgvItems.Rows[i].Cells[0].Value = false;
            }
        }

        private void llInvert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                if (dgvItems.Rows[i].Cells[0].Value != null
                    && dgvItems.Rows[i].Cells[0].Value.ToString().ToLower() == "true")
                {
                    dgvItems.Rows[i].Cells[0].Value = false;
                }
                else
                {
                    dgvItems.Rows[i].Cells[0].Value = true;
                }
            }
        }
    }
}