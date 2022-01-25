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

public partial class GSTReport : PageBase
{
    private const int GROSS_SALES = 1;
    private const int DISCOUNT = 2;
    private const int DISCOUNT_PERCENTAGE = 3;
    private const int NETT_SALES_B4_GST = 4;
    private const int COST_OF_GOODS = 5;
    private const int GST_AMT = 6;
    private const int NETT_SALES_AFTER_GST = 7;
    private const int PROFIT_LOSS = 8;
    private const int PROFIT_LOSS_PERCENTAGE = 9;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            /*ArrayList ar = PointOfSaleController.FetchOutletNames();
            ar.Create(1, "ALL - BreakDown");                                 */
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
        DataTable dt =
            ReportController.FetchGSTReport(
            startDate, endDate.AddSeconds(86399));        
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
            
            decimal GSTPayable = Decimal.Parse(e.Row.Cells[0].Text);
            decimal GSTPaid = Decimal.Parse(e.Row.Cells[1].Text);
            e.Row.Cells[0].Text = String.Format("{0:N2}", GSTPayable);
            e.Row.Cells[1].Text = String.Format("{0:N2}", GSTPaid);
            e.Row.Cells[2].Text = String.Format("{0:N2}", GSTPayable - GSTPaid);
            
            /*
            e.Row.Cells[DISCOUNT_PERCENTAGE].Text = String.Format("{0:0%}", Decimal.Parse(e.Row.Cells[DISCOUNT_PERCENTAGE].Text));
            e.Row.Cells[NETT_SALES_B4_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[NETT_SALES_B4_GST].Text));
            e.Row.Cells[GST_AMT].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[GST_AMT].Text));
            e.Row.Cells[NETT_SALES_AFTER_GST].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[NETT_SALES_AFTER_GST].Text));
            e.Row.Cells[COST_OF_GOODS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[COST_OF_GOODS].Text));
            e.Row.Cells[PROFIT_LOSS].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[PROFIT_LOSS].Text));
            e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text = String.Format("{0:0%}", Decimal.Parse(e.Row.Cells[PROFIT_LOSS_PERCENTAGE].Text));*/
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
  
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");               
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
}
