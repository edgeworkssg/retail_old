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
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace WinPowerPOS.OrderForms
{    
    public partial class frmViewInvoiceReceipt : Form
    {
        public POSController posCtrl;
        //public PrintOutParameters printOutParameters;
        public frmViewInvoiceReceipt()
        {
            InitializeComponent();
        }

        public frmViewInvoiceReceipt(POSController _pos)
        {
            InitializeComponent();
            posCtrl = _pos;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            string status;
            ReportDocument rpt = new ReportDocument();
            rpt = GenericReport.NewPrint.PrintController.GetReportDocForPreview(posCtrl);
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.RefreshReport();
        }
    }
}
