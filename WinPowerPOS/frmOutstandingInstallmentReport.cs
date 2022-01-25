using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS
{
    public partial class frmOutstandingInstallmentReport : Form
    {
        public frmOutstandingInstallmentReport()
        {
            InitializeComponent();
        }

        private void frmOutstandingInstallmentReport_Load(object sender, EventArgs e)
        {
            dgvInstallment.AutoGenerateColumns = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            dgvInstallment.DataSource = ReportController.FetchOutstandingInstallmentReport(txtSearch.Text);
            dgvInstallment.Refresh();

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvInstallment.DataSource;
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
            DataTable dt = (DataTable)dgvInstallment.DataSource;
            if (dt.Columns.Contains("LockerID")) dt.Columns.Remove("LockerID");
            ExportController.ExportToCSV(dt, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }

    }
}
