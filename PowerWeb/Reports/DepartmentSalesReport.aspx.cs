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

public partial class DepartmentSalesReport : PageBase
{
    private const int QTY= 1;
    private const int AMOUNT = 2;
    private const int COGS = 3;
    private const int PL = 4;
    private const int PLPercent = 5;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            OutletDropdownList.setIsUseAllBreakdownOutlet(false);
            OutletDropdownList.setIsUseAllBreakdownPOS(false);

            List<ListItem> listYear = new List<ListItem>();
            int minyear = UtilityController.GetMinOrderDateYear();
            for (int i = DateTime.Today.Year; i >= minyear; i--)
            {
                listYear.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlYear.Items.AddRange(listYear.ToArray());
            ddlYear.SelectedIndex = 0;
            //lblYear.Text = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
           
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");            
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

        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        if (!cbUseEndDate.Checked)
            endDate = DateTime.Now.AddYears(2);
        if (!cbUseStartDate.Checked)
            startDate = new DateTime(2007,02,1);
        DataTable dt = 
            ReportController.FetchDepartmentSalesReport(
            startDate, endDate.AddSeconds(86399),
            OutletDropdownList.GetDdlPOSSelectedItemText, OutletDropdownList.GetDdlOutletSelectedItemText,
            txtDept.Text, false, 
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
            e.Row.Cells[AMOUNT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AMOUNT].Text));
            //e.Row.Cells[COGS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COGS].Text));
            /*
            decimal tmp = Decimal.Parse(e.Row.Cells[PLPercent].Text);
            if (tmp >= -100 && tmp <= 100)
            {
                e.Row.Cells[PLPercent].Text = tmp.ToString("N2") + "%";
            }
            else
            {
                e.Row.Cells[PLPercent].Text = "ERR";
            }
            e.Row.Cells[PL].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PL].Text));    
             */ 
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable)gvReport.DataSource;
            if (dt != null)
            {
                decimal Amount, PLAmt;
                e.Row.Cells[QTY].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();
                Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
                e.Row.Cells[AMOUNT].Text = Amount.ToString("N2");
                //e.Row.Cells[COGS].Text =  decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");
                //PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
                //e.Row.Cells[PL].Text =  PLAmt.ToString("N2") + "%";
                //e.Row.Cells[PLPercent].Text = (PLAmt / Amount * 100).ToString("N2") + "%";
                /*
            decimal PLPercentageTMP;
            if (decimal.TryParse(dt.Compute("SUM(PROFITLOSSpercentage)",
                "ProfitLossPercentage <=100 AND ProfitLossPercentage >= -100").ToString(), out PLPercentageTMP)) 
            {
                e.Row.Cells[PLPercent].Text = 
                    (PLPercentageTMP / int.Parse(dt.Compute("COUNT(ItemNO)", 
                    "ProfitLossPercentage <=100 AND ProfitLossPercentage > -100").ToString())).ToString("N2") + "%";

            }*/
            }
        }
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
        OutletDropdownList.ResetDdlOutlet();
        OutletDropdownList.ResetDdlPOS();
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");        
        //txtCategoryName.Text = "";        
        //txtPointOfSale.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
    protected void ddDept_Init(object sender, EventArgs e)
    {
        
    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        
    }
}
