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

public partial class CustomerBehaviorReport : PageBase
{
    private const int TotalPurchased = 7;
    private const int TotalItemBought = 8;
    private const int NumberOfTransaction = 9;
    private const int AvgAmountPerTransaction = 10;
    private const int AvgAmountPerItem = 11;

    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Init(object sender, EventArgs e)
    {
        OutletDropdownList.setIsUseAllBreakdownOutlet(false);
        OutletDropdownList.setIsUseAllBreakdownPOS(false);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            lblYear.Text = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();            
            ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            ddCategory.DataBind();
            txtStartDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(-1).ToString("dd MMM yyyy");
            ddlMembershipGroup.DataSource = MembershipController.FetchMembershipGroupList();
            ddlMembershipGroup.DataTextField = "GroupName";
            ddlMembershipGroup.DataValueField = "MembershipGroupId";
            ddlMembershipGroup.DataBind();
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
        if (!cbFilterByDate.Checked)
        {
            startDate = new DateTime(1979, 11, 3);
            endDate = DateTime.Now;
        }
        DataTable dt = 
            ReportController.FetchCustomerPurchaseBehaviorReport(
            startDate, endDate.AddSeconds(86399),
            ddCategory.SelectedValue.ToString(), txtItemName.Text ,
            OutletDropdownList.GetDdlPOSSelectedItemText, OutletDropdownList.GetDdlOutletSelectedItemText,
            txtMembershipNo.Text, txtNameToAppear.Text,txtFirstName.Text,txtLastName.Text, ddlMembershipGroup.SelectedValue.GetIntValue(),
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString().Trim());        
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
            e.Row.Cells[TotalPurchased].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[TotalPurchased].Text));
            e.Row.Cells[AvgAmountPerTransaction].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AvgAmountPerTransaction].Text));
            e.Row.Cells[AvgAmountPerItem].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[AvgAmountPerItem].Text));                       
        }
            /*
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = (DataTable) gvReport.DataSource;
            if (dt != null)
            {
                decimal Amount, PLAmt;
                e.Row.Cells[5].Text = dt.Compute("SUM(TOTALQuantity)", "").ToString();
                Amount = decimal.Parse(dt.Compute("SUM(TOTALAMOUNT)", "").ToString());
                e.Row.Cells[AMOUNT].Text =  Amount.ToString("N2");
                e.Row.Cells[COGS].Text =  decimal.Parse(dt.Compute("SUM(TotalCOSTOFGOODSSOLD)", "").ToString()).ToString("N2");
                PLAmt = decimal.Parse(dt.Compute("SUM(PROFITLOSS)", "").ToString());
                e.Row.Cells[PL].Text =  PLAmt.ToString("N2") + "%";
                e.Row.Cells[PLPercent].Text = (PLAmt / Amount * 100).ToString("N2") + "%";
                    /*
                decimal PLPercentageTMP;
                if (decimal.TryParse(dt.Compute("SUM(PROFITLOSSpercentage)",
                    "ProfitLossPercentage <=100 AND ProfitLossPercentage >= -100").ToString(), out PLPercentageTMP)) 
                {
                    e.Row.Cells[PLPercent].Text = 
                        (PLPercentageTMP / int.Parse(dt.Compute("COUNT(ItemNO)", 
                        "ProfitLossPercentage <=100 AND ProfitLossPercentage > -100").ToString())).ToString("N2") + "%";

                }*/
          //  }
        //}
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

        OutletDropdownList.ResetDdlOutlet();
        OutletDropdownList.ResetDdlPOS();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        //ddCategory.SelectedItem.Text 
          // = "";
        txtItemName.Text = "";
        //txtPointOfSale.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVWithInvinsibleFields(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }

    //protected void ddPOS_Init(object sender, EventArgs e)
    //{
    //    if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
    //    {
    //        ddPOS.WhereField = "DepartmentID";
    //        ddPOS.WhereValue = Session["DeptID"].ToString();
    //    }
    //}
}
