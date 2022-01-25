using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.TestingReport
{
    public partial class frmHourlySalesReport : Form
    {
        public frmHourlySalesReport()
        {
            InitializeComponent();
        }

        private void frmHourlySalesReport_Load(object sender, EventArgs e)
        {
            datHourlySalesReportTableAdapters.HourlySalesDataTableAdapter aa = new WinPowerPOS.TestingReport.datHourlySalesReportTableAdapters.HourlySalesDataTableAdapter();
            datHourlySalesReport.HourlySalesDataDataTable dt = aa.GetData(new DateTime(2010, 11, 25), new DateTime(2010, 11, 27));

            for (int Counter = 0; Counter < dt.Rows.Count; Counter++)
            {

            }
                rptHourlySalesReport1.SetDataSource((DataTable)dt);
        }
    }
}
