using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinUtility
{
    public partial class frmUpdateAdjustedInventory : Form
    {
        public frmUpdateAdjustedInventory()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            string checkingSQL = "select count(*) from inventoryhdr where invoiceno in (select orderdetid from orderdet where lower(inventoryhdrrefno) ='adjusted')";
            object temp = DataService.ExecuteScalar(new QueryCommand(checkingSQL));
            int ct = 0;
            if (temp != null)
            {
                int.TryParse(temp.ToString(), out ct);
            }
            if (ct > 0)
            {
                MessageBox.Show("There are " + ct.ToString() + " records");
            }
            else
            {
                MessageBox.Show("Sector Clear. Proceed to no 3.");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string UpdateSQL = "update orderdet set inventoryhdrrefno = ''  where lower(inventoryhdrrefno) = 'adjusted'";
                DataService.ExecuteQuery(new QueryCommand(UpdateSQL));
                MessageBox.Show("Clear InventoryHdrRefNo Successful");
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); MessageBox.Show("Operation failed. Please check log"); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                InventoryController.AssignStockOutToConfirmedOrderUsingTransaction(false);
                MessageBox.Show("Stock Out Successful");
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); MessageBox.Show("Stock Out Failed. Please check log"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string UpdateSQL = "delete inventoryhdr where invoiceno in (select orderdetid from orderdet where lower(inventoryhdrrefno) ='adjusted')"; ;
                DataService.ExecuteQuery(new QueryCommand(UpdateSQL));
                MessageBox.Show("Delete Successful");
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); MessageBox.Show("Operation Failed. Please check log"); }
        }
    }

}
