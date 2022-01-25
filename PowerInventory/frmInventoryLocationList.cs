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

namespace PowerInventory
{
    public partial class frmInventoryLocationList : Form
    {        
        #region "Form initialization and load"
        public frmInventoryLocationList()
        {
            InitializeComponent();            
            
        }
        private void frmUserMst_Load(object sender, EventArgs e)
        {            
            dgvItems.AutoGenerateColumns = false;            

            displayData();
            
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
            InventoryLocationCollection col = new InventoryLocationCollection();
            col.Where(InventoryLocation.Columns.Deleted, false);
            col.Load();
            DataTable dt = col.ToDataTable();
            dgvItems.DataSource = dt;
            dgvItems.Refresh();
        }
        #endregion


        

        private void btnDeleteChecked_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete these Locations?");
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
                        InventoryLocation.Delete(dgvItems.Rows[i].Cells[2].Value);
                    }
                }
                MessageBox.Show("Locations has been deleted");
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
            frmAddInventoryLocation f = new frmAddInventoryLocation();            
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
            
        }
    }
}