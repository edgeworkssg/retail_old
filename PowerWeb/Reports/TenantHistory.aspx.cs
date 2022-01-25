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
    public partial class TenantHistory : PageBase
    {
        private const int TOTAL_SALES = 4;
        private const int TOTAL_TRANS = 5;

        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
            if (!Page.IsPostBack)
            {
                txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
                txtEndDate.Text = DateTime.Today.AddDays(1).ToString("dd MMM yyyy");
                ViewState["sortBy"] = "";
                ViewState[SORT_DIRECTION] = "";
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
            Session["UserName"] = userName;
            DateTime startDate = (Request.QueryString["StartDate"] + "").GetDateTimeValue("dd MMM yyyy");
            DateTime endDate = (Request.QueryString["EndDate"] + "").GetDateTimeValue("dd MMM yyyy");
            string pointOfSale = Request.QueryString["PointOfSale"] + "";
            string outlet = Request.QueryString["Outlet"] + "";
            bool showBeforGST = (Request.QueryString["ShowAmountBeforeGST"] + "") == "1";
            txtStartDate.Text = startDate.ToString("dd MMM yyyy");
            txtEndDate.Text = endDate.ToString("dd MMM yyyy");
            ddlOutlet_Init(ddlOutlet, new EventArgs());
            ddlOutlet.SelectedValue = outlet;
            ddPOS.SelectedValue = pointOfSale;
            lnkExport_Click(lnkExport, new EventArgs());
        }

        private void SetFormSetting()
        {
            try
            {
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                lblPOS.Text = posText;
                lblOutlet.Text = outletText;

                gvReport.Columns[0].HeaderText = outletText;
                gvReport.Columns[1].HeaderText = posText;
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

                DateTime startDate = DateTime.Parse(txtStartDate.Text).Date;
                DateTime endDate = DateTime.Parse(txtEndDate.Text).Date;

                DataTable dt =
                    ReportController.FetchTenantHistory(startDate, endDate,
                    ddlOutlet.SelectedValue, (ddPOS.SelectedValue + "").GetIntValue(),
                    ddlAccessSource.SelectedItem.Text,txtSearch.Text, 
                    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
                gvReport.DataSource = dt;
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
                DataTable ds = gvReport.DataSource as DataTable;
                if (ds != null)
                {
                    itemCount = ds.Rows.Count;
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

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

            ddlOutlet.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            gvReport.PageIndex = 0;
        }
        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid();
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            CommonWebUILib.ExportCSV(dt, this.Page.ToString().Trim(' '), "Daily Sales Report", gvReport);
        }

        protected void ddlOutlet_Init(object sender, EventArgs e)
        {
            try
            {
                ddlOutlet.DataSource = OutletController.FetchByUserNameForReport(false, true, (Session["UserName"] + ""));
                ddlOutlet.DataBind();
                ddlOutlet.SelectedIndex = 0;
                ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddPOS.DataSource = PointOfSaleController.FetchByUserNameForReport(false, true, (Session["UserName"] + ""), ddlOutlet.SelectedValue);
                ddPOS.DataBind();
                BindGrid();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
