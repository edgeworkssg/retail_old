using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.TestingReport
{
    public partial class frmClosingReport : Form
    {
        public frmClosingReport()
        {
            InitializeComponent();
        }

        private void frmClosingReport_Load(object sender, EventArgs e)
        {
            datClosingReportTableAdapters.CounterCloseLogTableAdapter aa = new WinPowerPOS.TestingReport.datClosingReportTableAdapters.CounterCloseLogTableAdapter();
            DataTable dt = aa.GetData();
            rptClosingReport1.SetDataSource(dt);
        }
    }
}
