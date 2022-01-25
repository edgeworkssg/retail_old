using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace WinPowerPOS
{
    public partial class frmMenuPanel : Form
    {
        public string CategoryName;
        public POSController pos;

        public frmMenuPanel()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMenuPanel_Load(object sender, EventArgs e)
        {
            //Load Touch menu panel based on category name
            lblCategoryName.Text = CategoryName;
            TouchScreenHotKeysController.populateItemDisplayPanel(flowLayoutPanel1, CategoryName);           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string status = "";
            if (((Button)sender).Tag == null)
            {
                return;
            }

            string itemno = ((Button)sender).Tag.ToString();
            if (itemno != "")
            {
                Item myItem = new Item(itemno);
                if (myItem != null && myItem.IsLoaded)
                {
                    if (pos.AddItemToOrder(myItem, 1, 0, true, out status))
                    {
                        this.Close();                        
                    }
                    else
                    {
                        MessageBox.Show("Error:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                       
                        return;
                    }
                }
            }
        }

    }
}