using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;

namespace PowerInventory.ItemForms
{
    public partial class frmManageHotKeys : Form
    {
        public frmManageHotKeys()
        {
            InitializeComponent();            
        }

        private void frmManageHotKeys_Load(object sender, EventArgs e)
        {
            HotKeysController hk = new HotKeysController();
            cmbHotkeys.DataSource = hk.GetHotKeyList();
            cmbHotkeys.Refresh();

            cmbItemNames.DataSource = ItemController.FetchItemNames();
            cmbItemNames.Refresh();
            displayGrid();
        }
        private void displayGrid()
        {
            HotKeysController hkCtrl = new HotKeysController();
            dataGridView1.DataSource = hkCtrl.GetDataTables();
            dataGridView1.Refresh();
        }
        private void btnAssignItem_Click(object sender, EventArgs e)
        {
            if (txtItemNo.Text == "")
            {
                MessageBox.Show("Please choose an item to assign.");
                return;
            }
            if (cmbHotkeys.SelectedItem.ToString() == "") 
            {
                MessageBox.Show("Please choose a hot key to assign.");
                return;
            }
            HotKeysController hkCtrl = new HotKeysController();
            hkCtrl.AssignHotkey(cmbHotkeys.SelectedItem.ToString(), txtItemNo.Text,txtItemName.Text);
            displayGrid();
        }        
        private void btnScan_Click(object sender, EventArgs e)
        {
            ItemController itemCtrl = new ItemController();
            MapItemToScreen(itemCtrl.FetchByBarcode(txtBarcode.Text));
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ItemController itemCtrl = new ItemController();
            ViewItemCollection coll = itemCtrl.SearchItem(cmbItemNames.SelectedItem, false, false);
            if (coll != null && coll.Count > 0)
                MapItemToScreen(coll[0]);            
        }

        private void MapItemToScreen(ViewItem myItem)
        {
            if (myItem == null)
            {
                MessageBox.Show("The Item you specified is invalid.");
                this.Close();
            }
            txtItemNo.Text = myItem.ItemNo;
            txtItemName.Text = myItem.ItemName;
            txtItemBarcode.Text = myItem.Barcode;
            txtBrand.Text = myItem.Brand;
            txtPrice.Text = myItem.RetailPrice.ToString();
            
            txtCategoryName.Text = myItem.CategoryName;
            txtItemDesc.Text = myItem.ItemDesc;
            txtRemark.Text = myItem.Remark;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbItemNames_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(this, new EventArgs());
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnScan_Click(this, new EventArgs());
            }
        }

    }
}