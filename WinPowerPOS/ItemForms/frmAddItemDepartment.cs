using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace WinPowerPOS.ItemForms
{
    public partial class frmAddItemDepartment : Form
    {
        public string DepartmentID;
        private ItemDepartment ctr;
        public frmAddItemDepartment()
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
                if (txtDepartmentID.Text == "")
                {
                    MessageBox.Show("Please fill in the Department ID.");
                    return;
                }
                                
                ctr.ItemDepartmentID = txtDepartmentID.Text;
                ctr.Remark = txtRemark.Text;
                ctr.DepartmentName = txtDepartmentName.Text;        
                ctr.Deleted = false;
                ctr.Save();
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_"))
                {
                    MessageBox.Show("The Department ID you have entered is already created. Please choose another ID.");
                    txtDepartmentID.Select();
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
            
            if (DepartmentID == "")
            {
                ctr = new ItemDepartment();
            }
            else
            {
                ctr = new ItemDepartment(DepartmentID);
                if (ctr.IsLoaded && !ctr.IsNew)
                {
                    txtDepartmentID.ReadOnly = true;
                    txtDepartmentID.Text = ctr.ItemDepartmentID;
                    txtRemark.Text = ctr.Remark;
                    txtDepartmentName.Text = ctr.DepartmentName;                                             
                }
            }
            txtDepartmentID.Focus();
            txtDepartmentID.Select();
        }
    }
}