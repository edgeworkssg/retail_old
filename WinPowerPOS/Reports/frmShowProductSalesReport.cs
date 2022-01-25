using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PowerPOS.Container;
using SubSonic;
using PowerPOS;
using PowerPOSReports;

namespace WinPowerPOS.Reports
{    
    public partial class frmShowProductSalesReport : Form
    {
        public DataTable salesReport;
        public DateTime startDate, endDate;
        
        public frmShowProductSalesReport()
        {
            InitializeComponent();                      
        }
        //private ConnectionInfo crConnectionInfo;

        private void frmShowProductSalesReport_Load(object sender, EventArgs e)
        {
            ProductSalesReport1.SetDataSource(salesReport);  
            ProductSalesReport1.DataDefinition.FormulaFields["StartDate"].Text = "\"" + startDate.ToString("dd MMM yyyy") + "\"";
            ProductSalesReport1.DataDefinition.FormulaFields["EndDate"].Text = "\"" + endDate.ToString("dd MMM yyyy") + "\"";
            ProductSalesReport1.DataDefinition.FormulaFields["Outlet"].Text = "\"" + PointOfSaleInfo.OutletName + "\"";
            ProductSalesReport1.DataDefinition.FormulaFields["PointOfSale"].Text = "\"" + PointOfSaleInfo.PointOfSaleName + "\"";

            crystalReportViewer1.ReportSource = ProductSalesReport1;
            crystalReportViewer1.RefreshReport();            
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
