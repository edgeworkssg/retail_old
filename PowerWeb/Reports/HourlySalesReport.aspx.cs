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

public partial class HourlySalesReport : PageBase
{
    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            List<ListItem> listYear = new List<ListItem>();
            for (int i = 0; i < 2; i++)
            {
                listYear.Add(new ListItem(DateTime.Today.AddYears(-i).Year.ToString(), DateTime.Today.AddYears(-i).Year.ToString()));
            }
            ddlYear.Items.AddRange(listYear.ToArray());
            ddlYear.SelectedIndex = 0;
            ddPOS.Items.AddRange(PointOfSaleController.FetchPointOfSaleNames(false, Session["UserName"] + ""));
            ddPOS.DataBind();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();            
            txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");

            for (int i = 0; i < 60; i++)
            {
                ddStartMinute.Items.Add(new ListItem(i.ToString("00")));
                ddStartSecond.Items.Add(new ListItem(i.ToString("00")));
                ddEndMinute.Items.Add(new ListItem(i.ToString("00")));
                ddEndSecond.Items.Add(new ListItem(i.ToString("00")));
            }

            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            BindGrid();
        }
    }
    private void BindGrid()
    {
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }
        String tmpStartDate, tmpEndDate;
        tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem + ":" + ddStartSecond.SelectedItem;
        tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem + ":" + ddEndSecond.SelectedItem;

        DateTime startDate = DateTime.Parse(tmpStartDate);
        DateTime endDate = DateTime.Parse(tmpEndDate);
        /*
        DataTable dt = 
            ReportController.FetchProductSalesReport(
            startDate, endDate.AddSeconds(86399),
            txtItemName.Text, ddPOS.SelectedItem.Text, ddlOutlet.SelectedValue.ToString(), 
             ddCategory.SelectedValue.ToString(), ddDept.SelectedValue.ToString(), false, 
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        */
        /*DataTable dt =
             ReportController.FetchHourlyTransactionReport
             (startDate, endDate.AddSeconds(86399), ddPOS.SelectedItem.Text);*/
        DataTable dt =
             ReportController.FetchHourlyTransactionReport
             (startDate, endDate, ddPOS.SelectedItem.Text);
        if (dt !=null && dt.Rows.Count > 0)
        {
            gvReport.DataSource = dt;            
        }
        else
        {
            gvReport.DataSource = null;
        }
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
        ViewState["sortBy"]  = e.SortExpression;
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
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                decimal tmp = 0;
                Decimal.TryParse(e.Row.Cells[i].Text, out tmp);
                e.Row.Cells[i].Text = String.Format("{0:N2}", tmp);
            }
        }
    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        //if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        //{
        //    ddPOS.WhereField = "DepartmentID";
        //    ddPOS.WhereValue = Session["DeptID"].ToString();
        //}
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        
        if (rdbMonth.Checked)
        {            
            txtStartDate.Text = (new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), 1)).ToString("dd MMM yyyy");
            txtEndDate.Text = new DateTime(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue), DateTime.DaysInMonth(int.Parse(ddlYear.Text), int.Parse(ddlMonth.SelectedValue))).ToString("dd MMM yyyy");                
        }

        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {  
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");        
            ddPOS.SelectedIndex = 0;
            gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVAllColumns(dt, this.Page.Title.Trim(' '),this.Page.Title);
    }   
}
