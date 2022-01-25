using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;

namespace PowerWeb.CRReport
{
    public partial class StockBalance : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ReportPath = cmbReportSelector.SelectedValue;
            ReportPath = Server.MapPath("~\\bin\\Reports\\StockBalance\\StockBalanceByAttributes" + ReportPath + ".rpt");

            string SearchQuery = "";
            if (SearchQuery == "") SearchQuery = "%";

            string InventoryLocID = cmbInventoryLocation.SelectedValue;
            if (InventoryLocID == "") InventoryLocID = "0";

            CrystalReportViewer1.ReportSource = PowerReport.StockBalance.GetReport(ReportPath, int.Parse(InventoryLocID), true, SearchQuery);
            CrystalReportViewer1.RefreshReport();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
