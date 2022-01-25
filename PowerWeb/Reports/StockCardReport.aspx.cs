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


public partial class StockCardReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Server.ScriptTimeout = 3600;
            ddCategory.Items.AddRange(ItemController.FetchCategoryNamesInListItem());
            ddCategory.DataBind();
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";            
        }
    }

    private void BindGrid()
    {
        try
        {
            if (ViewState["sortBy"] == null)
            {
                ViewState["sortBy"] = "ItemName";
                ViewState[SORT_DIRECTION] = "ASC";
            }
            string locationName = "";
            if (ddlInventoryLocation.SelectedItem != null)
                locationName = ddlInventoryLocation.SelectedItem.Text;

            DataTable dt =
                ReportController.FetchStockCardReport
                (txtStartDate.Text.GetDateTimeValue("dd MMM yyyy"), txtEndDate.Text.GetDateTimeValue("dd MMM yyyy").AddSeconds(86399),
                    (ddlInventoryLocation.SelectedValue+"").GetIntValue(), txtItemName.Text, ddCategory.SelectedValue);
            DataView dv = new DataView();
            dt.TableName = "Stock Card";
            dv.Table = dt;
            dv.Sort = ViewState["sortBy"] + " " + ViewState[SORT_DIRECTION];
            gvReport.DataSource = dv;
            gvReport.DataBind();

            AttributesLabel al = new AttributesLabel(1);
            if (al != null && al.AttributesNo != null && !String.IsNullOrEmpty(al.Label))
            {
                gvReport.Columns[4].Visible = true;
                gvReport.Columns[4].HeaderText = al.Label;
            }
            else
            {
                gvReport.Columns[4].Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
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
        if (e.Row.RowType == DataControlRowType.Header)
        {
            AttributesLabel al = new AttributesLabel(1);
            if (al != null && al.AttributesNo != null && !String.IsNullOrEmpty(al.Label))
            {
                e.Row.Cells[4].Text = al.Label;
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal tmp = 0;
            if (Decimal.TryParse(e.Row.Cells[5].Text, out tmp))
                e.Row.Cells[5].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[6].Text, out tmp))
                e.Row.Cells[6].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[7].Text, out tmp))
                e.Row.Cells[7].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[8].Text, out tmp))
                e.Row.Cells[8].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[9].Text, out tmp))
                e.Row.Cells[9].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[10].Text, out tmp))
                e.Row.Cells[10].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[11].Text, out tmp))
                e.Row.Cells[11].Text = String.Format("{0:0.####}", tmp);

            if (Decimal.TryParse(e.Row.Cells[12].Text, out tmp))
                e.Row.Cells[12].Text = String.Format("{0:0.####}", tmp);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            DataTable dt = ((DataView)gvReport.DataSource).Table;

            e.Row.Cells[5].Text = String.Format("{0:0.####}",dt.Compute("SUM(BalanceBefore)",""));
            e.Row.Cells[6].Text = String.Format("{0:0.####}",dt.Compute("SUM(StockIn)", ""));
            e.Row.Cells[7].Text = String.Format("{0:0.####}",dt.Compute("SUM(StockOut)", ""));
            e.Row.Cells[8].Text = String.Format("{0:0.####}",dt.Compute("SUM(TransferIn)", ""));
            e.Row.Cells[9].Text = String.Format("{0:0.####}",dt.Compute("SUM(TransferOut)", ""));
            e.Row.Cells[10].Text = String.Format("{0:0.####}",dt.Compute("SUM(AdjustmentIn)", ""));
            e.Row.Cells[11].Text = String.Format("{0:0.####}",dt.Compute("SUM(AdjustmentOut)", ""));
            e.Row.Cells[12].Text = String.Format("{0:0.####}", dt.Compute("SUM(BalanceAfter)", ""));
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
        try
        {
            BindGrid();
            DataTable dt = ((DataView)gvReport.DataSource).Table;
            CommonWebUILib.ExportCSVWithInvinsibleFields(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }
        catch (Exception ex)
        {
            if (gvReport.DataSource == null)
            {
                lblMsg.Text = "No result to be exported.";
            }
            else
            {
                lblMsg.Text = ex.Message;
            }
        }
    }   
}
