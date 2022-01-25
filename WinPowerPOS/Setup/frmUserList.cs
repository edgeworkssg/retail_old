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
namespace WinPowerPOS
{
    public partial class frmUserMst : Form
    {        
        #region "Form initialization and load"
        public frmUserMst()
        {
            InitializeComponent();
        }
        private void frmUserMst_Load(object sender, EventArgs e)
        {
            
            dgvItems.AutoGenerateColumns = false;

            //Load User Group            
            UserGroupCollection userGroup = new UserGroupCollection();
            userGroup.Where(UserGroup.Columns.Deleted, false);
            userGroup.Load();
            UserGroup tmp = new UserGroup();
            tmp.GroupID = 0;
            tmp.GroupName = "--Please Select--";
            userGroup.Insert(0, tmp);

            cmbGroupName.DataSource = userGroup;
            cmbGroupName.Refresh();
            cmbGroupName.SelectedIndex = 0;
            
            displayData();
            txtDisplayName.Select();
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
                UserController.FetchUsers
                (txtUserName.Text,txtDisplayName.Text,txtRemark.Text,
                ((UserGroup)cmbGroupName.SelectedValue).GroupID);            
            dgvItems.DataSource = dt;
            dgvItems.Refresh();
        }
        #endregion


        #region "Edit and delete items"
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                frmUserSetup f = new frmUserSetup();
                f.username = dgvItems.Rows[e.RowIndex].Cells["UserName"].Value.ToString();
                f.ShowDialog();
            }
        }
        
        #endregion                

        private void btnDeleteChecked_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete these users?");
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
                        UserMst.Delete(dgvItems.Rows[i].Cells[1].Value);
                    }
                }
                MessageBox.Show("Users has been deleted");
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
            frmUserSetup f = new frmUserSetup();
            f.username = "";
            f.ShowDialog();
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