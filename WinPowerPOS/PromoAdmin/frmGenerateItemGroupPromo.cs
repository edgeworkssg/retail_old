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
    public partial class frmGenerateItemGroupPromo : Form
    {
        ItemCollection itCol;
        public frmGenerateItemGroupPromo()
        {
            itCol = new ItemCollection();

            InitializeComponent();
            txtBarcode.Select();
            dgvItems.AutoGenerateColumns = false;            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            double discount;

            if (dgvItems.Rows.Count == 0)
            {
                MessageBox.Show("No item was specified to be discounted. Please scan/search items.");
                txtBarcode.Focus();
                return;
            }
            if (!double.TryParse(txtPrice.Text, out discount))
            {
                MessageBox.Show("Please specify price.");
                txtPrice.Focus();
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

            //Create Item Group

            decimal price=0;
            discount = 0;
            if (rbPrice.Checked)
            {
                if (decimal.TryParse(txtPrice.Text, out price))
                {
                    discount = 0;
                }
                else 
                {
                    MessageBox.Show("Please specify correct price.");
                    txtPrice.Select();
                }
            }
            else if (rbDiscount.Checked)
            {
                if (double.TryParse(txtDiscount.Text, out discount))
                {
                    price = 0;
                }
                else 
                {
                    MessageBox.Show("Please specify correct discount.");
                    txtDiscount.Select();
                }
            }
            if (PromotionAdminController.InsertItemGroupPriceDiscount
                (txtCampaignName.Text, dtpStartDate.Value, dtpEndDate.Value, 
                txtCampaignName.Text,txtBarcodeItemGroup.Text,itCol, price, discount, ForNonMembersAlso))    
            {
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Generate Item Group Price Discount", "");
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
                    myItem.Userfloat1 = 1;
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
            else if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                //Pop up quantity key pad
                frmKeypad f = new frmKeypad();
                f.IsInteger = true;
                try
                {
                    f.initialValue = dgvItems.Rows[e.RowIndex].Cells[2].Value.ToString();
                }
                catch { }
                f.ShowDialog();
                if (f.value != "")
                {
                    //update user float1 on the item....
                    int tmp= itCol.Find(Item.Columns.ItemNo, dgvItems.Rows[e.RowIndex].Cells[1].Value.ToString());
                    decimal tmpDec;
                    decimal.TryParse(f.value, out tmpDec);
                    itCol[tmp].Userfloat1 = tmpDec;
                    dgvItems.Refresh();
                }
            }
        }
    }
}
