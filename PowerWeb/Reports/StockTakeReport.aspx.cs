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

public partial class StockTakeReport : PageBase
{
    private const int AMOUNT = 5;
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
            InventoryLocationController ctr = new InventoryLocationController();
            InventoryLocationCollection r = ctr.FetchAll();
            ListItem l;
            l = new ListItem();
            l.Text = "ALL";
            l.Value = "0";
            ddInventoryLocation.Items.Add(l);
            for (int i = 0; i < r.Count; i++)
            {
                l = new ListItem();
                l.Text = r[i].InventoryLocationName;
                l.Value = r[i].InventoryLocationID.ToString();
                ddInventoryLocation.Items.Add(l);
            }
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
        tmpStartDate = txtStartDate.Text + " " + ddStartHour.SelectedValue + ":" + ddStartMinute.SelectedItem;
        tmpEndDate = txtEndDate.Text + " " + ddEndHour.SelectedValue + ":" + ddEndMinute.SelectedItem;

        DateTime startDate = DateTime.Parse(tmpStartDate);
        DateTime endDate = DateTime.Parse(tmpEndDate);
        int LocationId;
        int.TryParse(ddInventoryLocation.SelectedValue, out LocationId);

        DataTable dt = null;
        dt = ReportController.FetchStockTakeReport
            (cbUseStartDate.Checked, cbUseEndDate.Checked, startDate, endDate, txtItemName.Text, LocationId, "", "", ViewState["sortBy"].ToString(), ViewState[SORT_DIRECTION].ToString());
        gvReport.DataSource = dt;
        gvReport.DataBind();

        AttributesLabel al = new AttributesLabel(1);
        if (al != null && al.AttributesNo != null && !String.IsNullOrEmpty(al.Label))
        {
            gvReport.Columns[5].Visible = true;
            gvReport.Columns[5].HeaderText = al.Label;
        }
        else
        {
            gvReport.Columns[5].Visible = false;
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
            e.Row.Cells[6].Text = String.Format("{0:0.####}", Decimal.Parse(e.Row.Cells[6].Text)); // Bal Qty
            e.Row.Cells[7].Text = String.Format("{0:0.####}", Decimal.Parse(e.Row.Cells[7].Text)); // Stock Take Qty
            e.Row.Cells[8].Text = String.Format("{0:0.####}", Decimal.Parse(e.Row.Cells[8].Text)); // Adjusted Qty
            e.Row.Cells[9].Text = String.Format("{0:0.####}", Decimal.Parse(e.Row.Cells[9].Text));
            e.Row.Cells[10].Text = String.Format("{0:0.##}", Decimal.Parse(e.Row.Cells[10].Text));
        }
        else if (e.Row.RowType == DataControlRowType.Footer) 
        {
            DataTable dt = (DataTable)gvReport.DataSource;
            decimal TotalDiscrepancyValue = 0.0M;
            decimal AdjustedQty=0, multiplier=1;

            for (int i = 0; i < dt.Rows.Count; i++)
            {                            
                AdjustedQty += (multiplier * decimal.Parse(dt.Rows[i]["AdjustmentQty"].ToString()));                
                TotalDiscrepancyValue += (multiplier * decimal.Parse(dt.Rows[i]["TotalDiscrepancyValue"].ToString()));
            }
            /*
            e.Row.Cells[7].Text = 
                String.Format("{0:N2}",
                AdjustedQty);*/

            e.Row.Cells[10].Text = 
                String.Format("{0:N2}", 
                TotalDiscrepancyValue);
        }      
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        BindGrid();
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
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }  


    protected void gvReport_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvReport.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    protected void gvReport_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = int.Parse(gvReport.DataKeys[e.RowIndex]["StockTakeID"].ToString());
        string Remark = ((TextBox)gvReport.Rows[e.RowIndex].Cells[11].Controls[0]).Text;

        StockTakeController.UpdateStockTakeRemark(id, Remark);
        gvReport.EditIndex = -1;
        BindGrid();
    }

    protected void gvReport_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvReport.EditIndex = -1;
        BindGrid();
    }
}
