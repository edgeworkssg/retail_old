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

namespace WinPowerPOS.Reports
{
    public partial class frmProductSalesReport : Form
    {
        ItemCollection itCol;
        public frmProductSalesReport()
        {
            itCol = new ItemCollection();

            InitializeComponent();
            //txtBarcode.Select();
            //dgvItems.AutoGenerateColumns = false;            
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;
            ArrayList itemNoList = new ArrayList();
            /*
            if (!cbAllItem.Checked)
            {
                for (int i = 0; i < dgvItems.Rows.Count; i++)
                {                    
                    itemNoList.Add(dgvItems.Rows[i].Cells["itemNo"].Value.ToString());
                }
            }*/
            //open product sales report 
            /*
            DataTable salesReport =
                POSReports.FetchProductSalesReportByPointOfSale
                (startDate, endDate, PointOfSaleInfo.PointOfSaleID, itemNoList, "CategoryName", "ASC");
             */
            DataTable salesReport = ReportController.FetchProductSalesReport(startDate, endDate, txtSearch.Text,PointOfSaleInfo.PointOfSaleName,PointOfSaleInfo.OutletName, "", "", false, "CategoryName", "asc");
            /*
                POSReports.FetchProductSalesReportByPointOfSale
                (startDate, endDate, PointOfSaleInfo.PointOfSaleID, itemNoList, "CategoryName", "ASC");
            */
            if (salesReport.Rows.Count > 0)
            {
                frmShowProductSalesReport f = new frmShowProductSalesReport();
                f.salesReport = salesReport;
                f.startDate = startDate;
                f.endDate = endDate;
                f.ShowDialog();
            }
            else
            {
                MessageBox.Show("There is no data for the given search criteria", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        /*
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
                cbAllItem.Checked = false;

            }
            myAddItem.Dispose();            

        }
        */
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /*
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
                    cbAllItem.Checked = false;
                }
                else
                {
                    MessageBox.Show("Barcode does not exist in the system.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Text = "";

                }
            }
        }
        */
        /*
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, new EventArgs());
            }
        }
        */
        private void frmProductSalesReport_Load(object sender, EventArgs e)
        {
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
        }
    }
}
