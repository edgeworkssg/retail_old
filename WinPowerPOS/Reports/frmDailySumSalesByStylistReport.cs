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
    public partial class frmDailySumSalesByStylistReport : Form
    {
        public frmDailySumSalesByStylistReport()
        {
            InitializeComponent();
            dtpStartDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpEndDate.Value = DateTime.Now;
            dgvReport.AutoGenerateColumns = true;
            cmbType.SelectedIndex = 0;
        }

        private void frmSalesByStylistReport_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {            
            DataTable dt = 
                ReportController.FetchTransactionWithSalesPersonByDaily
                (dtpStartDate.Value, dtpEndDate.Value, cmbType.SelectedIndex, PointOfSaleInfo.PointOfSaleID);

            dgvReport.DataSource = dt;
            for (int i = 1; i < dgvReport.Columns.Count; i++)
            {
                dgvReport.Columns[i].DefaultCellStyle.Format = "N2";
            }
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
