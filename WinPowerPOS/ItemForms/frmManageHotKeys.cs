using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;

namespace WinPowerPOS.ItemForms
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

            cmbItemNames.DataSource = ItemController.FetchItems("", "", "", "", "", "", "", "", "", "", "");
            cmbItemNames.ValueMember = Item.Columns.ItemNo;
            cmbItemNames.DisplayMember = Item.Columns.ItemName;
            //cmbItemNames.DataSource = ItemController.FetchItemNames();
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
            hkCtrl.AssignHotkey(cmbHotkeys.SelectedItem.ToString(), txtItemNo.Text, txtItemName.Text);
            displayGrid();
        }
        private void btnScan_Click(object sender, EventArgs e)
        {
            ItemController itemCtrl = new ItemController();
            //MapItemToScreen(itemCtrl.FetchByBarcode(txtBarcode.Text));
            MapItemBarcodeItemNo();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ItemController itemCtrl = new ItemController();
            //ViewItemCollection coll = itemCtrl.FetchByName(cmbItemNames.SelectedItem);
            ViewItem coll = new ViewItem(ViewItem.Columns.ItemNo, cmbItemNames.SelectedValue.ToString());
            //if (coll != null && coll.Count > 0)
            MapItemToScreen(coll);
        }

        private void MapItemToScreen(ViewItem myItem)
        {
            ItemController itemCtrl = new ItemController();

            if (myItem == null)
            {
                MessageBox.Show("The Item you specified is invalid.");
                this.Close();
            }

            //barcode doesn't find
            if (!string.IsNullOrEmpty(myItem.ItemNo))
            {
                cmbItemNames.SelectedValue = myItem.ItemNo;
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

        private void MapItemBarcodeItemNo()
        {
            string search = txtBarcode.Text;

            ItemController itemCtrl = new ItemController();
            ViewItem myItem = new ViewItem();

            if (!string.IsNullOrEmpty(txtBarcode.Text))
            {
                myItem = itemCtrl.FetchByBarcode(txtBarcode.Text);
            }

            if (myItem == null)
            {
                MessageBox.Show("The Item you specified is invalid.");
                this.Close();
            }

            //barcode found
            if (!string.IsNullOrEmpty(myItem.ItemNo))
            {
                DataTable dt = ItemController.FetchItemsContainsItemNo("");

                cmbItemNames.DataSource = dt;
                cmbItemNames.ValueMember = Item.Columns.ItemNo;
                cmbItemNames.DisplayMember = Item.Columns.ItemName;
                cmbItemNames.Refresh();

                cmbItemNames.SelectedValue = myItem.ItemNo;
            }
            else
            {
                //find by itemno 
                DataTable dt = ItemController.FetchItemsContainsItemNo(txtBarcode.Text);

                cmbItemNames.DataSource = dt;
                cmbItemNames.ValueMember = Item.Columns.ItemNo;
                cmbItemNames.DisplayMember = Item.Columns.ItemName;
                cmbItemNames.Refresh();
                if (dt.Rows.Count > 0)
                {
                    myItem = new ViewItem(ViewItem.Columns.ItemNo, dt.Rows[0]["ItemNo"]);
                }

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

        private void button1_Click(object sender, EventArgs e)
        {
            using (frmManageXMLButtons inst = new frmManageXMLButtons())
            {
                inst.ShowDialog();
            }
            //using (frmTouchScreenXML inst = new frmTouchScreenXML())
            //{
            //    inst.IsEditMode = true;
            //    inst.ShowDialog();
            //}
        }

    }
}