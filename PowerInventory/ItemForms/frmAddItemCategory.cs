using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace PowerInventory.ItemForms
{
    public partial class frmAddItemCategory : Form
    {
        public string CategoryName;
        private Category ctr;
        public frmAddItemCategory()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCategoryName.Text == "")
                {
                    MessageBox.Show("Please fill in the Category Name.");
                    return;
                }
                                
                ctr.CategoryName = txtCategoryName.Text;
                ctr.Remarks = txtRemark.Text;
                ctr.AccountCategory = txtAccountCat.Text;
                ctr.ItemDepartmentId = ((ItemDepartment)cmbDepartment.SelectedValue).ItemDepartmentID;
                ctr.IsForSale = true;
                ctr.IsDiscountable = true;
                ctr.IsGST = true;
                ctr.Deleted = false;
                ctr.Save();
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_"))
                {
                    MessageBox.Show("The Category Name you have entered is already created. Please choose another name.");
                    txtCategoryName.Select();
                }
                else
                {
                    MessageBox.Show("Unknown error has occurred, please contact your administrator.");
                }
                Logger.writeLog(ex);
                
            }
        }

        private void frmAddItemCategory_Load(object sender, EventArgs e)
        {
            ItemDepartmentCollection itemDept = new ItemDepartmentCollection();
            itemDept.Where(ItemDepartment.Columns.Deleted, false);
            itemDept.Load();
            cmbDepartment.DataSource = itemDept;
            cmbDepartment.Refresh();
             
            if (CategoryName == "")
            {
                ctr = new Category();
            }
            else
            {
                ctr = new Category(CategoryName);
                if (ctr.IsLoaded && !ctr.IsNew)
                {
                    txtCategoryName.ReadOnly = true;
                    txtCategoryName.Text = ctr.CategoryName;
                    txtRemark.Text = ctr.Remarks;
                    txtAccountCat.Text = ctr.AccountCategory;                         
                    for (int i=0; i < cmbDepartment.Items.Count; i++)
                    {
                        if (((ItemDepartment)cmbDepartment.Items[i]).ItemDepartmentID == 
                            ctr.ItemDepartmentId)
                        {
                            cmbDepartment.SelectedIndex = i;
                        }
                    }
                }
            }
            txtCategoryName.Focus();
            txtCategoryName.Select();
        }
    }
}