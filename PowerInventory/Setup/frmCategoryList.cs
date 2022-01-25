using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;
using System.Collections;
using PowerInventory.ItemForms;
namespace PowerInventory
{
    public partial class frmCategoryList : Form
    {        
        #region "Form initialization and load"
        public frmCategoryList()
        {
            InitializeComponent();            
            
        }
        private void frmUserMst_Load(object sender, EventArgs e)
        {            
            dgvItems.AutoGenerateColumns = false;

            LoadDeptCombo();

            displayData();

            txtAccountCategory.Select();
        }

        private void LoadDeptCombo()
        {
            ItemDepartmentCollection itemDept = new ItemDepartmentCollection();
            itemDept.Where(ItemDepartment.Columns.Deleted, false);
            itemDept.Load();
            itemDept.Insert(0, new ItemDepartment());

            cmbDepartment.DataSource = itemDept;
            cmbDepartment.Refresh();
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
            string categoryName, remarks, departmentID, accountCategory;
            departmentID = "";
            accountCategory = txtAccountCategory.Text;
            categoryName = txtCategoryName.Text;
            remarks = txtRemark.Text;
            if (cmbDepartment.SelectedIndex >  0)
            {
                departmentID = ((ItemDepartment)cmbDepartment.SelectedValue).ItemDepartmentID;
            }
            
            ViewCategoryCollection col = new ViewCategoryCollection();
            col.Where(ViewCategory.Columns.Deleted, false);

            //search filter
            if (categoryName!= "")
            {
                col.Where(ViewCategory.Columns.CategoryName, Comparison.Like, "%" + categoryName + "%");                
            }

            if (accountCategory != "")
            {
                col.Where(ViewCategory.Columns.AccountCategory, Comparison.Like, "%" + accountCategory + "%");
            }

            if (remarks != "")
            {
                col.Where(ViewCategory.Columns.CategoryName, Comparison.Like, "%" + remarks + "%");
            }

            if (departmentID != "")
            {
                col.Where(ViewCategory.Columns.ItemDepartmentID, Comparison.Like, "%" + departmentID + "%");
            }

            col.Load();
            DataTable dt = col.ToDataTable();
            dgvItems.DataSource = dt;
            dgvItems.Refresh();
        }
        #endregion


        #region "Edit and delete items"
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                try
                {

                    frmAddItemCategory myFrm = new frmAddItemCategory();
                    myFrm.CategoryName = dgvItems.Rows[dgvItems.CurrentCell.RowIndex].Cells["categoryname"].Value.ToString();
                    CommonUILib.displayTransparent();myFrm.ShowDialog();CommonUILib.hideTransparent();
                    myFrm.Dispose();
                    displayData();
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        #endregion                

        private void btnDeleteChecked_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete these categories?");
            if (dr == DialogResult.No)
            {
                return;
            }
            try
            {                
                //Add item from the selected checkboxes into the gridview
                for (int i = 0; i < dgvItems.Rows.Count; i++)
                {
                    //if (dgvItemList.Rows[i].Cells[0]
                    if (dgvItems.Rows[i].Cells[0].Value is bool &&
                        (bool)dgvItems.Rows[i].Cells[0].Value == true)
                    {                        
                        Category.Delete(dgvItems.Rows[i].Cells[1].Value);
                    }
                }
                MessageBox.Show("Categories has been deleted");
                displayData();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            frmAddItemCategory f = new frmAddItemCategory();            
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();
            displayData();
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

        private void btnNewDepartment_Click(object sender, EventArgs e)
        {
            frmAddItemDepartment f = new frmAddItemDepartment();
            CommonUILib.displayTransparent();f.ShowDialog();CommonUILib.hideTransparent();
            f.Dispose();
            LoadDeptCombo();
        }
    }
}