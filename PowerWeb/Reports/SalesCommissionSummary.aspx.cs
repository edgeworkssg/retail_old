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
using System.Collections.Generic;
using System.Linq;

public partial class SalesCommissionSummary : PageBase
{
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");

            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";

            ViewState["ShowUnpaidOnly"] = 0;
        }
    }

    private void BindGrid(bool generate)
    {
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }

        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        if (!cbUseStartDate.Checked)
            startDate = startDate.AddYears(-100);

        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        if (!cbUseEndDate.Checked)
            endDate = endDate.AddYears(100);

        string sql = "";

        sql = @"EXEC [dbo].[REPORT_SalesCommissionSummary]
                    @FilterStartDate = @StartDate,
                    @FilterEndDate = @EndDate,
                    @FilterUserName = @UserName,
                    @Generate = @a,
                    @ShowUnpaidOnly = @b";

        QueryCommand cmd = new QueryCommand(sql);
        cmd.AddParameter("@StartDate", startDate);
        cmd.AddParameter("@EndDate", endDate);
        cmd.AddParameter("@UserName", ddlStaff.SelectedValue);
        cmd.AddParameter("@a", generate ? 1 : 0);
        cmd.AddParameter("@b", ViewState["ShowUnpaidOnly"]);

        DataTable dt = new DataTable();
        dt.Load(DataService.GetReader(cmd));

        gvReport.DataSource = dt;
        gvReport.DataBind();
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        ViewState["ShowUnpaidOnly"] = 0;
        BindGrid(true);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        ViewState["ShowUnpaidOnly"] = 0;
        BindGrid(false);
    }

    protected void btnShowUnpaid_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        ViewState["ShowUnpaidOnly"] = 1;
        BindGrid(false);
    }

    protected void cldStartDate_SelectionChanged(object sender, EventArgs e)
    {
        cbUseStartDate.Checked = true;
    }
    protected void cldEndDate_SelectionChanged(object sender, EventArgs e)
    {
        cbUseEndDate.Checked = true;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        cbUseStartDate.Checked = false;
        cbUseEndDate.Checked = false;

        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");

        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid(false);
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid(false);
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

        BindGrid(false);
    }

    protected void gvReport_DataBound(object sender, EventArgs e)
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

    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            bool isUnpaid = e.Row.Cells[6].Text == "Unpaid";
            Button btnPay = ((Button)(e.Row.FindControl("btnPay")));
            btnPay.Visible = isUnpaid;
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable)gvReport.DataSource;

            e.Row.Cells[0].Text = "Grand Total";
            
            decimal salary = (from o in dt.AsEnumerable()
                                       select o.Field<decimal>("Salary")).Sum<decimal>(o => o);
            e.Row.Cells[2].Text = salary.ToString("N2");

            decimal otherAllowance = (from o in dt.AsEnumerable()
                                   select o.Field<decimal>("OtherAllowance")).Sum<decimal>(o => o);
            e.Row.Cells[3].Text = otherAllowance.ToString("N2");

            decimal commission = (from o in dt.AsEnumerable()
                                      select o.Field<decimal>("Commission")).Sum<decimal>(o => o);
            e.Row.Cells[4].Text = commission.ToString("N2");

            decimal total = (from o in dt.AsEnumerable()
                                  select o.Field<decimal>("Total")).Sum<decimal>(o => o);
            e.Row.Cells[5].Text = total.ToString("N2");
        }
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvReport.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvReport.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
        BindGrid(false);
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

    protected void ddlStaff_Init(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        var data = new UserMstController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false && o.IsASalesPerson).OrderBy(o => o.DisplayName).ToList();
        data.Insert(0, new UserMst { UserName = "ALL", DisplayName = "ALL" });
        ddl.DataSource = data;
        ddl.DataBind();
    }

    protected void gvReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((Button)e.CommandSource).NamingContainer);

        string arg = e.CommandArgument.ToString();
        if (e.CommandName == "Pay")
            DoPay(arg);
        else if (e.CommandName == "Details")
            ShowDetails(arg);
    }

    private void DoPay(string id)
    {
        QueryCommand cmd = new QueryCommand("UPDATE SalesCommissionSummary SET Status = 'Paid' WHERE ID = @ID");
        cmd.AddParameter("@ID", id);

        DataService.ExecuteQuery(cmd);

        BindGrid(false);
    }

    private void ShowDetails(string arg)
    {

    }
}
