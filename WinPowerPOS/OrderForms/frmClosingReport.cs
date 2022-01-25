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

namespace WinPowerPOS.OrderForms
{    
    public partial class frmClosingReport : Form
    {
        public string CounterCloseLogID;
        public frmClosingReport()
        {
            InitializeComponent();
        }
        //private ConnectionInfo crConnectionInfo;
        
        private void frmClosingReport_Load(object sender, EventArgs e)
        {
            
            //Load Report...                        
            ViewCloseCounterReportCollection settlement;
            DataTable ProductSalesReport, RefundSalesReport;
            ViewVouchersSoldCollection VoucherSoldReport;
            ViewVoucherRedeemedCollection VoucherRedeemedReport;

            POSReports.FetchSettlementSalesReport
                (CounterCloseLogID, false, out settlement, out ProductSalesReport, out RefundSalesReport,
                 out VoucherSoldReport, out VoucherRedeemedReport);


            CounterCloseReport1.SetDataSource(settlement.ToDataTable());
            CounterCloseReport1.Subreports["ProductSales"].SetDataSource(ProductSalesReport);
            CounterCloseReport1.Subreports["ExchangeReport"].SetDataSource(RefundSalesReport);
            CounterCloseReport1.Subreports["VoucherSold"].SetDataSource(VoucherSoldReport.ToDataTable());
            CounterCloseReport1.Subreports["ViewVoucherRedeemed"].SetDataSource(VoucherRedeemedReport.ToDataTable());
            CounterCloseReport1.DataDefinition.FormulaFields["CounterCloseLogID"].Text = "\"" + CounterCloseLogID + "\"";
            CounterCloseReport1.DataDefinition.
                FormulaFields["RoundingAmount"].Text = "\"$" + POSReports.FetchTotalRoundingAmount(settlement[0].StartTime, settlement[0].EndTime, settlement[0].PointOfSaleID).ToString("N2") + "\"";
            
            crystalReportViewer1.ReportSource = CounterCloseReport1;
            crystalReportViewer1.RefreshReport();            
        }
    }
}
