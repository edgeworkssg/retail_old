using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinTestModules
{
    public partial class frmTestStockReport : Form
    {
        public frmTestStockReport()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            /*
            dgvReport.DataSource =
                ReportController.FetchStockReportByDate
                ("", 0, false, dtpStockDate.Value, "0", "ItemName", "ASC");
            dgvReport.Refresh();
             */
            
        DataTable dt = ReportController.FetchStockReportBreakdownByLocation
            ("", false, "", "", "");

        dgvReport.DataSource = dt;
            dgvReport.Refresh();
 
        }
    }
}
