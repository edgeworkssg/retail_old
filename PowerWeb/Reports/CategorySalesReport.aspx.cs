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
using System.Text.RegularExpressions;
using System.Data.SqlClient;

public partial class CategorySalesReport : PageBase
{
    private const int QTY= 2;
    private const int AMOUNT = 3;
    private const int COGS = 4;
    private const int PL = 5;
    private const int PLPercent = 6;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            List<ListItem> listYear = new List<ListItem>();

            for (int i = 0; i < 60; i++)
            {
                ddStartMinute.Items.Add(new ListItem(i.ToString("00")));
                ddStartSecond.Items.Add(new ListItem(i.ToString("00")));
                ddEndMinute.Items.Add(new ListItem(i.ToString("00")));
                ddEndSecond.Items.Add(new ListItem(i.ToString("00")));
            }

            int minyear = UtilityController.GetMinOrderDateYear();
            for (int i = DateTime.Today.Year; i >= minyear; i--)
            {
                listYear.Add(new ListItem(i.ToString(), i.ToString()));
            }
            ddlYear.Items.AddRange(listYear.ToArray());
            ddlYear.SelectedIndex = 0;
            //lblYear.Text = DateTime.Today.Year.ToString();
            ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
            //ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            //ddCategory.DataBind();

            CategoryCollection categories = new CategoryCollection();
            ArrayList list = new ArrayList();
            categories.Load();
            for (int i = 0; i < categories.Count; i++)
            {
                list.Add(categories[i].CategoryName);
            }
            mccCategory.ClearAll();
            mccCategory.AddItems(list);
            list.Clear();

            //ddCategory.SelectedIndex = 0;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            ddEndHour.SelectedValue = "23";
            ddEndMinute.SelectedValue = "59";
            ddEndSecond.SelectedValue = "59";

            ViewState["sortBy"] = "CategoryName";
            ViewState[SORT_DIRECTION] = "ASC";
            BindGrid();
        }
    }

    private void BindGrid()
    {
        try
        {
            if (ViewState["sortBy"] == null)
                ViewState["sortBy"] = "CategoryName";
            if (ViewState[SORT_DIRECTION] == null)
                ViewState[SORT_DIRECTION] = "ASC";

            String tmpStartDate, tmpEndDate;
            tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem + ":" + ddStartSecond.SelectedItem;
            tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem + ":" + ddEndSecond.SelectedItem;

            DateTime startDate = DateTime.Parse(tmpStartDate);
            DateTime endDate = DateTime.Parse(tmpEndDate);
            if (!cbUseEndDate.Checked)
                endDate = DateTime.Now.AddYears(2);
            if (!cbUseStartDate.Checked)
                startDate = new DateTime(2007, 02, 1);

            string SelectedCategory = "";
            string[] category = mccCategory.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in category)
            {
                //SelectedCategory += "'" + item.Replace("'", "''").Trim() + "',";
                SelectedCategory += item.Replace("'", "''").Trim() + ":";
            }

            if (SelectedCategory == string.Empty)
            {
                CategoryCollection categories = new CategoryController().FetchAll();
                foreach (var item in categories)
                {
                    //SelectedCategory += "'" + item.CategoryName.Replace("'", "''").Trim() + "',";
                    SelectedCategory += item.CategoryName.Replace("'", "''").Trim() + ":";
                }
            }

            SelectedCategory = SelectedCategory.Substring(0, SelectedCategory.Length - 1);

            //DataTable dt =
            //    ReportController.FetchProductCategorySalesReportMultipleCategory(
            //    startDate, endDate.AddSeconds(86399),
            //    OutletDropdownList.GetDdlPOSSelectedItemText, OutletDropdownList.GetDdlOutletSelectedItemText,
            //    SelectedCategory, ddDept.SelectedValue.ToString(), false,
            //    ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());        


            ReportController.RegenerateDWData(new DWType[] { DWType.ALL }, startDate, endDate, ddlOutlet.SelectedValue);

            string sql = @"
            EXEC [dbo].[REPORT_CategorySalesReport]
             @CategoryName = @CategoryName_,
             @StartDate = @StartDate_,
             @EndDate = @EndDate_,
             @OutletName = @OutletName_,
             @DeptID = @DeptID_";
            QueryCommand cmd = new QueryCommand(sql);
            cmd.AddParameter("@CategoryName_", SelectedCategory, DbType.String);
            cmd.AddParameter("@StartDate_", startDate);
            cmd.AddParameter("@EndDate_", endDate);
            cmd.AddParameter("@OutletName_", ddlOutlet.SelectedValue + "");
            cmd.AddParameter("@Search_", txtSearch.Text);
            cmd.AddParameter("@DeptID_", ddDept.SelectedValue + "");

            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(cmd));

            DataView dv = dt.DefaultView;
            dv.Sort = ViewState["sortBy"] + " " + ViewState[SORT_DIRECTION];
            DataTable sortedDT = dv.ToTable();

            gvReport.DataSource = sortedDT;
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
        mccCategory.unselectAllItems();
        mccCategory.Text = "";
        //OutletDropdownList.ResetDdlOutlet();
        //OutletDropdownList.ResetDdlPOS();
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
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddDept.WhereField = "DepartmentID";
            ddDept.WhereValue = Session["DeptID"].ToString();
        }
    }

    protected void ddlOutlet_Init(object sender, EventArgs e)
    {
        ddlOutlet.Items.Clear();
        ddlOutlet.Items.AddRange(PointOfSaleController.FetchOutletNames(true, Session["UserName"] + ""));
    }
}
