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
using PowerPOS;
using PowerPOS.Container;
using System.Collections.Generic;
using SubSonic;
using SubSonic.Utilities;

public partial class LineCommissionItem : PageBase
{
    private enum SortingColumns
    {
        REPORT_DATE = 0,
        SALES_PERSON = 1,
        ASSISTANT = 2,
        AMOUNT = 3
    }
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            #region *) Load Outlet list
            ddlOutlet.DataSource = PointOfSaleController.FetchByUserNameForReport(false, true, Session["UserName"] + "", "ALL");
            ddlOutlet.DataBind();
            #endregion
            #region *) Load Sales Person list
            UserMstController conSalesPerson = new UserMstController();
            DataTable dtSalesPerson = conSalesPerson.FetchSalesPerson_forListView();
            DataRow Rw = dtSalesPerson.NewRow();
            Rw[UserMstController.ListBoxColumns.ValueColumnName] = "%";
            Rw[UserMstController.ListBoxColumns.DisplayedColumnName] = "ALL";
            dtSalesPerson.Rows.InsertAt(Rw, 0);
            ddlSalesPerson.DataSource = dtSalesPerson;
            ddlSalesPerson.DataValueField = UserMstController.ListBoxColumns.ValueColumnName;
            ddlSalesPerson.DataTextField = UserMstController.ListBoxColumns.DisplayedColumnName;
            ddlSalesPerson.DataBind();
            #endregion
            #region *) Set Date/Time list
            lblYear.Text = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            txtStartDate.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            #endregion

            #region *) Set Sort settings (ViewState)
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            #endregion

            BindGrid();
        }
    }

    private void BindGrid()
    {
        #region *) Set Sort settings
        if (ViewState["sortBy"].ToString() == null | ViewState["sortBy"].ToString() == "")
        {
            ViewState["sortBy"] = "ReportDate";
        }
        #endregion

        #region *) Get Start & End Date
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        #endregion

        DataTable dt =
            ReportController.FetchLineCommissionItemByDate(
                startDate, endDate.AddDays(1).AddSeconds(-1),
                ddlOutlet.SelectedItem.Text, ddDept.SelectedValue, ddlSalesPerson.SelectedValue,
                ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());

        gvReport.DataSource = dt;
        gvReport.DataBind();
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

    /*
    private decimal totalGrossSales, totalDiscount, totalDiscountPercentage,
        totalNettSalesB4GST, totalGSTAmt, totalNettSalesAfterGST,
        totalCostOfGoods, totalProfitAndLoss, totalProfitAndLossPercentage;
    */

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal Amount;

            e.Row.Cells[SortingColumns.REPORT_DATE.GetHashCode()].Text = DateTime.Parse(e.Row.Cells[SortingColumns.REPORT_DATE.GetHashCode()].Text).ToString("dd MMM yyyy");

            Amount = Decimal.Parse(e.Row.Cells[SortingColumns.AMOUNT.GetHashCode()].Text);
            e.Row.Cells[SortingColumns.AMOUNT.GetHashCode()].Text = String.Format("{0:N2}", Amount);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable)gvReport.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                e.Row.Cells[SortingColumns.AMOUNT.GetHashCode()].Text = decimal.Parse(dt.Compute("SUM(Amount)", "").ToString()).ToString("N2");
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (rdbMonth.Checked)
        {
            txtStartDate.Text = (new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
            txtEndDate.Text = new DateTime(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(lblYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");
        }
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
        DataTable dt = (DataTable) gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' ').Replace('/', ' '), "Profit & Loss Report", gvReport);
    }
    protected void ddDept_Init(object sender, EventArgs e)
    {
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddDept.WhereField = "DepartmentID";
            ddDept.WhereValue = Session["DeptID"].ToString();
        }
    }
}
