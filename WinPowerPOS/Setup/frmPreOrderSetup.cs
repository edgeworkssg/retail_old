using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using System.Collections;
using PowerPOSReports;
using PowerPOS;
using WinPowerPOS.OrderForms;

namespace WinPowerPOS.Setup
{
    public partial class frmPreOrderSetup : Form
    {
        PreOrderController pCtrl;
        public frmPreOrderSetup()
        {
            pCtrl = new PreOrderController();
            InitializeComponent();
            txtBarcode.Select();
            dgvItems.AutoGenerateColumns = false;
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmAddItem myAddItem = new frmAddItem();
            myAddItem.searchReq = txtSearch.Text.Replace(' ', '%');            
            myAddItem.ShowDialog();
            Item myItem;
            if (myAddItem != null && myAddItem.itemNumbers != null)
            {
                for (int i = 0; i < myAddItem.itemNumbers.Count; i++)
                {
                    myItem = new Item(myAddItem.itemNumbers[i]);
                    if (myItem.IsLoaded && !myItem.IsNew)
                    {
                        pCtrl.insertPreOrderItem(myItem.ItemNo, dtpStartDate.Value.Date, dtpEndDate.Value.Date.AddSeconds(86399));
                    }
                }
                txtSearch.Text = "";
                txtBarcode.Select();
                BindGrid();
                
            }
            myAddItem.Dispose();            

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //add new row to dgvItem.....
                Item tmpItem = new Item(Item.Columns.Barcode, txtBarcode.Text);
                if (tmpItem.IsLoaded && !tmpItem.IsNew)
                {
                    pCtrl.insertPreOrderItem(tmpItem.ItemNo,dtpStartDate.Value.Date, dtpEndDate.Value.Date.AddSeconds(86399));
                    BindGrid();
                    txtBarcode.Text = "";                    
                }
                else
                {
                    MessageBox.Show("Barcode does not exist in the system.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Text = "";

                }
            }
        }

        private void BindGrid()
        {
            dgvItems.DataSource = pCtrl.fetchPreOrderSchedule();
            dgvItems.Refresh();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                pCtrl.deletePreOrderItem(dgvItems.Rows[e.RowIndex].Cells[1].Value.ToString());
                BindGrid();
            }
        }

        private void frmPreOrderSetup_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

   
    }
}
