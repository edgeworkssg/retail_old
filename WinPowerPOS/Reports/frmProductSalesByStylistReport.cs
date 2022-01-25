using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;

namespace WinPowerPOS.Reports
{
    public partial class frmProductSalesByStylistReport : Form
    {
        public frmProductSalesByStylistReport()
        {
            InitializeComponent();
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
            dgvReport.AutoGenerateColumns = true;
        }

        private void frmStylistReport_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {
            string search = "%" + txtSearch.Text + "%";
            
            DataTable dt = 
                ReportController.FetchTransactionDetailWithSalesPersonReport
                (dtpStartDate.Value, dtpEndDate.Value, search, PointOfSaleInfo.PointOfSaleName, PointOfSaleInfo.OutletName, "%","%", "","");

            dgvReport.DataSource = dt;
            dgvReport.Refresh();
        }

        private void dgvReport_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvReport.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                fsdExportToExcel.ShowDialog();
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }   
        }
        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            DataTable dt = (DataTable)dgvReport.DataSource;
            ExportController.ExportToCSV(dt, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
