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
using PowerPOS.Container;
using System.Linq;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;

namespace PowerWeb.Reports
{
    public partial class DailySalesVSManualSubmission : PageBase
    {
        private const int TOTAL_SALES = 1;
        private const int TOTAL_SUBMIT = 2;
        private const int TOTAL_DIFF = 3;        
        private const int TOTAL_DIFFPERCENT = 4;

        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Init(object sender, EventArgs e)
        {
            OutletDropdownList.setIsUseAllBreakdownOutlet(false);
            OutletDropdownList.setIsUseAllBreakdownPOS(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            if (!Page.IsPostBack)
            {
                ddlYear.Items.Clear();
                for (int i = DateTime.Now.Year - 5; i < DateTime.Now.Year + 5; i++)
                    ddlYear.Items.Add(i.ToString());

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
                txtDiscrepancy.Text = (AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.DiscrepancyPercentage) + "").GetDecimalValue().ToString("N0");
                string queryStr = Request.QueryString["ExportData"] + "";
                if (queryStr.ToUpper().Equals("YES"))
                    ExportData();
                else
                    BindGrid();
            }
        }

        private void ExportData()
        {
            string userName = (Request.QueryString["UserName"] + "");
            string discrepancy = (Request.QueryString["Discrepancy"] + "");
            Session["UserName"] = userName;
            DateTime startDate = (Request.QueryString["StartDate"] + "").GetDateTimeValue("dd MMM yyyy");
            DateTime endDate = (Request.QueryString["EndDate"] + "").GetDateTimeValue("dd MMM yyyy");
            string pointOfSale = Request.QueryString["PointOfSale"] + "";
            string outlet = Request.QueryString["Outlet"] + "";
            txtStartDate.Text = startDate.ToString("dd MMM yyyy");
            txtEndDate.Text = endDate.ToString("dd MMM yyyy");
            txtDiscrepancy.Text = discrepancy.GetDecimalValue().ToString("N2");
            OutletDropdownList.SetDdlOutletSelectedValue(outlet);
            OutletDropdownList.SetDdlPOSSelectedValue(pointOfSale);
            lnkExport_Click(lnkExport, new EventArgs());
        }

        private void SetFormSetting()
        {
            try
            {
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                gvReport.Columns[5].HeaderText = outletText;
                gvReport.Columns[6].HeaderText = posText;
                OutletDropdownList.SetLabelPOS(posText);
                OutletDropdownList.SetLabelOutlet(outletText);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                DataService.ExecuteQuery(new QueryCommand("SET ANSI_NULLS ON"));


                if (ViewState["sortBy"].ToString() == null | ViewState["sortBy"].ToString() == "")
                {
                    ViewState["sortBy"] = "SalesDate";
                }
                DateTime startDate = DateTime.Parse(txtStartDate.Text);
                DateTime endDate = DateTime.Parse(txtEndDate.Text);
                DataTable dt =
                    ReportController.FetchDailySalesReportVSManualSubmission(
                    startDate.Date, endDate.Date, 0,
                    OutletDropdownList.GetDdlOutletSelectedItemText, (OutletDropdownList.GetDdlPOSSelectedValue + "").GetIntValue(), txtDiscrepancy.Text.GetDecimalValue(),
                    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
                DataView dv = new DataView();
                dt.TableName = "Report";
                dv.Table = dt;
                dv.Sort = ViewState["sortBy"].ToString() + " " + ViewState[SORT_DIRECTION].ToString();
                gvReport.DataSource = dv;
                gvReport.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReport.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvReport_DataBound(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            if (gvrPager == null)
            {
                return;
            }

            // get your controls from the gridview
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");
            if (ddlPages != null)
            {
                // populate pager
                for (int i = 0; i < gvReport.PageCount; i++)
                {
                    int intPageNumber = i + 1;
                    ListItem lstItem = new ListItem(intPageNumber.ToString());
                    if (i == gvReport.PageIndex)
                    {
                        lstItem.Selected = true;
                    }

                    ddlPages.Items.Add(lstItem);
                }

            }

            int itemCount = 0;
            // populate page count
            if (lblPageCount != null)
            {
                //pull the datasource
                DataSet ds = gvReport.DataSource as DataSet;
                if (ds != null)
                {
                    itemCount = ds.Tables[0].Rows.Count;
                }

                string pageCount = "<b>" + gvReport.PageCount.ToString() + "</b>";
                lblPageCount.Text = pageCount;
            }

            Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
            Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
            Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
            Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
            //now figure out what page we're on
            if (gvReport.PageIndex == 0)
            {
                btnPrev.Enabled = false;
                btnFirst.Enabled = false;
            }

            else if (gvReport.PageIndex + 1 == gvReport.PageCount)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }

            else
            {
                btnLast.Enabled = true;
                btnNext.Enabled = true;
                btnPrev.Enabled = true;
                btnFirst.Enabled = true;
            }

        }
        protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["sortBy"] = e.SortExpression;
            //rebind the grid
            if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
            {
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            }

            else
            {
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            }

            BindGrid();
        }
        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = gvReport.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            gvReport.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid();
        }
        string GetSortDirection(string sortBy)
        {
            string sortDir = " ASC";
            if (ViewState["sortBy"] != null)
            {
                string sortedBy = ViewState["sortBy"].ToString();
                if (sortedBy == sortBy)
                {
                    //the direction should be desc
                    sortDir = " DESC";
                    //reset the sorter to null
                    ViewState["sortBy"] = null;
                }
                else
                {
                    //this is the first sort for this row
                    //put it to the ViewState
                    ViewState["sortBy"] = sortBy;
                }
            }
            else
            {
                //it's null, so this is the first sort
                ViewState["sortBy"] = sortBy;
            }
            return sortDir;
        }

        protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                DataTable dt = ((DataView)gvReport.DataSource).Table;
                if (dt != null && dt.Rows.Count > 0)
                {
                    e.Row.Cells[TOTAL_SALES].Text = decimal.Parse(dt.Compute("SUM(TotalSales)", "").ToString()).ToString("N2");
                    e.Row.Cells[TOTAL_SUBMIT].Text = decimal.Parse(dt.Compute("SUM(ManualSubmission)", "").ToString()).ToString("N2");
                    e.Row.Cells[TOTAL_DIFF].Text = decimal.Parse(dt.Compute("SUM(Diff)", "").ToString()).ToString("N2");
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[TOTAL_DIFFPERCENT].Text = e.Row.Cells[TOTAL_DIFFPERCENT].Text.GetDecimalValue().ToString("N2") + "%";
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rdbMonth.Checked)
            {
                int selectedMonth = int.Parse(ddlMonth.SelectedValue);
                DateTime startDate = new DateTime((ddlYear.SelectedValue + "").GetIntValue(), selectedMonth, 1);
                txtStartDate.Text =
                    startDate.ToString("dd MMM yyyy");

                txtEndDate.Text = startDate.AddMonths(1).ToString("dd MMM yyyy");
            }
            BindGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            OutletDropdownList.ResetDdlOutlet();
            OutletDropdownList.ResetDdlPOS();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            CommonWebUILib.ExportCSV(dt, this.Page.ToString().Trim(' '), "Daily Sales VS Manual Submission Report", gvReport);
        }

    }
}
