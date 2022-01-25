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

public partial class StylistProductSalesReport : PageBase
{
    private const int QTY = 4;
    private const int AMOUNT_WITH_GST = 5;
    private const int GSTAMOUNT = 6;
    private const int AMOUNT = 7;
    private const int COGS = 8;
    private const int PL = 9;
    private const int PLPercent = 10;
    int totalDefaultCol = 4;

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
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            BindGrid();
        }
        AttributesLabelCollection atCol = new AttributesLabelCollection();
        atCol.Load();
        totalDefaultCol = 4;
        foreach (PowerPOS.AttributesLabel a in atCol)
        {
            if (!String.IsNullOrEmpty(a.Label))
            {
                totalDefaultCol++;
            }
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
        /*
        DataTable dt = 
            ReportController.FetchProductSalesReport(
            startDate, endDate.AddSeconds(86399),
            txtItemName.Text, ddPOS.SelectedItem.Text, ddlOutlet.SelectedValue.ToString(), 
             ddCategory.SelectedValue.ToString(), ddDept.SelectedValue.ToString(), false, 
            ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        */
        DataTable dt =
         ReportController.FetchTransactionDetailWithSalesPersonReport
             (startDate, endDate.AddSeconds(86399), "%" + txtItemName.Text + "%",
             OutletDropdownList.GetDdlPOSSelectedItemText, OutletDropdownList.GetDdlOutletSelectedItemText, ddDept.SelectedValue.ToString(), ddCategory.SelectedValue.ToString(),
             ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        if (dt.Rows.Count > 0)
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

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Text = LanguageManager.GetTranslation("Department");
            e.Row.Cells[1].Text = LanguageManager.GetTranslation("Category");
            e.Row.Cells[2].Text = LanguageManager.GetTranslation("Item No");
            e.Row.Cells[3].Text = LanguageManager.GetTranslation("Item");
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = totalDefaultCol; i < e.Row.Cells.Count; i++)
            {
                decimal money;
                if (decimal.TryParse(e.Row.Cells[i].Text, out money))
                {
                    e.Row.Cells[i].Text = money.ToString("N2");
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            AttributesLabelCollection atCol = new AttributesLabelCollection();
            atCol.Load();
            var atcCount = 0;
            //int attLabelCount = atCol.Where(PowerPOS.AttributesLabel.Columns.Label, Comparison.IsNot, string.Empty).Count;
            foreach (PowerPOS.AttributesLabel a in atCol)
            {
                if (!String.IsNullOrEmpty(a.Label))
                {
                    atcCount++;
                }
            }

            DataTable dt = (DataTable)gvReport.DataSource;
            if (dt != null)
            {
                decimal Amount, PLAmt;
                for (var i = 4 + atcCount; i < dt.Columns.Count; i++)
                {
                    decimal TotalAmountFooter;

                    TotalAmountFooter = decimal.Parse(dt.Compute("SUM([" + dt.Columns[i].ToString() + "])", "").ToString());
                    e.Row.Cells[i].Text = TotalAmountFooter.ToString("N2");
                }


            }
        }
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
        CommonWebUILib.ExportCSVAllColumns(dt, this.Page.Title.Trim(' '), this.Page.Title);
    }
    protected void ddDept_Init(object sender, EventArgs e)
    {
        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddDept.WhereField = "DepartmentID";
            ddDept.WhereValue = Session["DeptID"].ToString();
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
}
