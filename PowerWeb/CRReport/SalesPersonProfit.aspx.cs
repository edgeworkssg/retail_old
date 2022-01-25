using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS.Container;

using PowerPOS;
using SubSonic;
using SubSonic.Utilities;


namespace PowerWeb.CRReport
{
    public partial class SalesPersonProfit : System.Web.UI.Page
    {

        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblYear.Text = DateTime.Today.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                txtStartDate.Text = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).ToString("dd MMM yyyy");
            }

            BindGrid();
        }
        private void BindGrid()
        {
            string ReportPath =
                Server.MapPath("~\\bin\\Reports\\SalesPerson\\SalesPersonProfit.rpt");

            string SearchQuery = "";
            if (SearchQuery == "") SearchQuery = "%";

            DateTime StartDate = DateTime.Parse(txtStartDate.Text);
            DateTime EndDate = DateTime.Parse(txtEndDate.Text);

            CrystalReportViewer1.ReportSource = PowerReport.SalesPersonProfit.GetReport(ReportPath, StartDate, EndDate, false, SearchQuery);
            CrystalReportViewer1.RefreshReport();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ViewState["sortBy"] = null;

            if (rdbMonth.Checked)
            {
                txtStartDate.Text = (new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
                txtEndDate.Text = new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
            }

            BindGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {


            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            //gvReport.PageIndex = 0;
        }
    }
}
