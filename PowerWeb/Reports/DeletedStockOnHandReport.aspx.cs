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


public partial class DeletedStockOnHandReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {                        
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            BindGrid();
            string status;
            
            DataTable dt = InventoryController.FetchUndeductedStockSummary(out status);
            if (status != "")
            {
                CommonWebUILib.ShowMessage(lblMsg, status, CommonWebUILib.MessageType.BadNews);
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    string message = "WARNING: There are undeducted sales on the following locations:<BR/> ";
                    for (int p = 0; p < dt.Rows.Count; p++)
                    {
                        message += dt.Rows[p]["InventoryLocationName"] + ": " + dt.Rows[p]["Quantity"].ToString() + " items.<BR/>";
                    }
                    message += "Click <a href=\"./undeductedsalesreport.aspx\"> here </a> to view Undeducted Sales Report";
                    CommonWebUILib.ShowMessage(lblMsg, message, CommonWebUILib.MessageType.BadNews);
                }
            }
        }
    }

    private void BindGrid()
    {
        
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }
        
        int inventoryLocationID = 0;
        int.TryParse(ddlInventoryLocation.SelectedItem.Value.ToString(), out inventoryLocationID);
        string searchQuery = txtSearch.Text;


        DataTable dt = ReportController.FetchDeletedItemStockReport(searchQuery,
                    inventoryLocationID, cbShowCostPrice.Checked,"", ViewState["sortBy"].ToString(), 
                    ViewState[SORT_DIRECTION].ToString());        
        gvReport.DataSource = dt;
        gvReport.DataBind(); 
        
    }

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid();
    }
    private void calculateSum(out int TotalQty, out decimal TotalCost)
    {
        try
        {
            TotalQty = 0; TotalCost = 0;
            DataTable dt = (DataTable)gvReport.DataSource;
            if (dt.Rows.Count > 0)
            {
                TotalQty = int.Parse(dt.Compute("SUM(OnHand)", "").ToString());
                if (cbShowCostPrice.Checked)
                {
                    TotalCost = decimal.Parse(dt.Compute("SUM(TotalCost)", "").ToString());
                }
            }
        }
        catch (Exception ex)
        {
            TotalQty = 0; TotalCost = 0;
            Logger.writeLog(ex);
        }
    }

    protected void gvReport_DataBound(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvReport.BottomPagerRow;
        if (gvrPager == null)
        {
            return;
        }
        
        Label lblTotalQty = (Label)gvrPager.Cells[0].FindControl("lblTotalQty");        
        Label lblTotalCost = (Label)gvrPager.Cells[0].FindControl("lblTotalCost");

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
            if (cbShowCostPrice.Checked)
            {
                e.Row.Cells[5].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[5].Text));
                e.Row.Cells[6].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[6].Text));
            }
            else
            {
                e.Row.Cells[5].Text = "--";
                e.Row.Cells[6].Text = "--";
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            int TotalQty;
            decimal TotalCost;
            calculateSum(out TotalQty, out TotalCost);            
            e.Row.Cells[4].Text = TotalQty.ToString();
            if (cbShowCostPrice.Checked)
            {
                e.Row.Cells[6].Text = TotalCost.ToString("N2");
            }                           
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlInventoryLocation.SelectedIndex = 0;
        
        txtSearch.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVWithInvinsibleFields(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }

}
