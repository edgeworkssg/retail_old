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

namespace WinPowerPOS.PromoAdmin
{
    public partial class frmGenerateDiscount : Form
    {
        ItemCollection itCol;
        public frmGenerateDiscount()
        {
            itCol = new ItemCollection();

            InitializeComponent();
            txtBarcode.Select();
            dgvItems.AutoGenerateColumns = false;            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            if (dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("No item was specified to be discounted. Please scan/search items.");
                txtBarcode.Focus();
                return;
            }
            double discount;
            if (!double.TryParse(txtDiscount.Text,out discount))
            {
                MessageBox.Show("Please specify discount.");
                txtDiscount.Focus();
                return;
            }
            ArrayList ItemNo = new ArrayList();

            for (int j = 0; j < dgvItems.Rows.Count; j++)
            {
                ItemNo.Add(dgvItems.Rows[j].Cells["ItemNo"].Value.ToString());
            }

            
            if (!PromotionAdminController.IsCampaignNameNotUsed(txtCampaignName.Text))
            {
                MessageBox.Show("Campaign Name has been used before. Please choose another name");
                return;
            }
            bool ForNonMembersAlso = true;

            if (PromotionAdminController.InsertDiscountByItemByBatch
                (txtCampaignName.Text, dtpStartDate.Value, dtpEndDate.Value, ItemNo, discount, ForNonMembersAlso))
            {
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Generate Discount By Item", "");
                MessageBox.Show("Campaign has been created");
                this.Close();
            }
            else
            {
                MessageBox.Show("Campaign cannot be created. Please try again.");
            }
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
                        itCol.Add(myItem);
                    }
                }
                txtSearch.Text = "";
                txtBarcode.Select();
                dgvItems.DataSource = itCol;
                dgvItems.Refresh();                

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
                    itCol.Add(tmpItem);
                    dgvItems.DataSource = itCol;
                    dgvItems.Refresh();
                    txtBarcode.Text = "";                    
                }
                else
                {
                    MessageBox.Show("Barcode does not exist in the system.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Text = "";

                }
            }
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }

        private void frmProductSalesReport_Load(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today;
            dtpEndDate.Value = DateTime.Today.AddDays(1);
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                //remove product from the list....
                itCol.RemoveAt(e.RowIndex);                
                dgvItems.DataSource = itCol;
                dgvItems.Refresh();
            }
        }
    }
}
