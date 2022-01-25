using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace PowerInventory
{
    public partial class frmAddInventoryLocation : Form
    {
        public string CategoryName;
        private InventoryLocation ctr;
        public frmAddInventoryLocation()
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
                if (txtLocationName.Text == "")
                {
                    MessageBox.Show("Please fill in the Location Name.");
                    return;
                }
                int count = (InventoryLocation.CreateQuery()).WHERE(InventoryLocation.Columns.InventoryLocationName, txtLocationName.Text).AND(InventoryLocation.Columns.Deleted, false).GetCount(InventoryLocation.Columns.InventoryLocationID);
                if (count > 0)
                {
                    MessageBox.Show("The location name you entered has already been created.");
                    return;
                }
                ctr = new InventoryLocation();
                ctr.InventoryLocationName= txtLocationName.Text;
                ctr.Deleted = false;
                ctr.Save();
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_"))
                {
                    MessageBox.Show("The Category Name you have entered is already created. Please choose another name.");
                    txtLocationName.Select();
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
            txtLocationName.Focus();
            txtLocationName.Select();
        }

        private void txtLocationName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd_Click(this, new EventArgs());
            }
        }
    }
}