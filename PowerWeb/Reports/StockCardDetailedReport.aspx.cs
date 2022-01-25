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


public partial class StockCardDetailedReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Server.ScriptTimeout = 3600;
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";            
        }
    }

    private void BindGrid()
    {
        
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "ItemName";
            ViewState[SORT_DIRECTION] = "ASC";
        }
        int inventoryLocationID;
        if (ddlInventoryLocation.SelectedIndex != 0)
        {
            inventoryLocationID = int.Parse(ddlInventoryLocation.SelectedValue);
        }
        else
        {
            inventoryLocationID = 0;
        }
        DataTable dt = ReportController.FetchStockCardReportWithStockOutDetails
            (DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text).AddSeconds(86399),
                inventoryLocationID, true, txtItemName.Text, 
                ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        var index = new List<int>();
        for (int i = 0; i < dt.Columns.Count; i++)
            index.Add(i);
        gvReport.DataSource = LanguageManager.GetTranslation(dt, index.ToArray());
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
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //e.Row.Cells[0].Text = "Department";
            //e.Row.Cells[1].Text = "Category";
            //e.Row.Cells[2].Text = "Item No";
            //e.Row.Cells[3].Text = "Item Name";
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal result = 0.0M;
            for (int i = 5; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = e.Row.Cells[i].Text.GetDecimalValue().ToString("0.####");
                
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = ((DataTable)gvReport.DataSource);
            for (int i = 5; i < dt.Columns.Count; i++)
            {
                decimal val = 0;
                if (decimal.TryParse(dt.Compute("SUM([" + dt.Columns[i].ColumnName + "])", "").ToString(), out val))
                {
                    if (i == 5)
                        e.Row.Cells[i].Text = val.ToString("0.####");
                    else
                        e.Row.Cells[i].Text = val.ToString("0.####");
                    
                }
            }
            //e.Row.Cells[4].Text = String.Format("{0:N2}", e.Row.Cells[4].Text);
            /*
            e.Row.Cells[4].Text = dt.Compute("SUM(BalanceBefore)","").ToString();
            e.Row.Cells[5].Text = dt.Compute("SUM(StockIn)", "").ToString();
            e.Row.Cells[6].Text = dt.Compute("SUM(StockOut)", "").ToString();
            e.Row.Cells[7].Text = dt.Compute("SUM(TransferIn)", "").ToString();
            e.Row.Cells[8].Text = dt.Compute("SUM(TransferOut)", "").ToString();
            e.Row.Cells[9].Text = dt.Compute("SUM(AdjustmentIn)", "").ToString();
            e.Row.Cells[10].Text = dt.Compute("SUM(AdjustmentOut)", "").ToString();
            e.Row.Cells[11].Text = dt.Compute("SUM(BalanceAfter)", "").ToString(); 
            */
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
        
        txtItemName.Text = "";
        gvReport.PageIndex = 0;
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVAllColumns(dt, this.Page.Title.Trim(' '), this.Page.Title);
    }   
}
