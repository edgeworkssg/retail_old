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


public partial class LineSheet : PageBase
{
    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private const int Col_ItemNo = 6;
    private const int Col_Qty = 8;
    private const int Col_OrderDetID = 1;
    private const int Col_Status = 12;
    private const int Col_Outstanding = 9;

    private DataTable dtLineSheet;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            BindGrid();
        }
        
            if (Session["LineData"] != null && Session["LineData"] is DataTable)
                dtLineSheet = (DataTable)Session["LineData"];
        
    }
    private void BindGrid()
    {
        DataTable dt = null;

        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }

        try
        {
            
            //dt = DeliveryController.FetchDeliveryListNew(txtRefNo.Text, pos.PointOfSaleID, startDate, endDate, txtSearch.Text, ProjectName, CorporateName, Status);
            ItemController it = new ItemController();
            dt = it.SearchItem_PlusPointInfo(txtSearch.Text, false);
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                gvReport.DataSource = dt;
            }
            else
            {
                gvReport.DataSource = null;
            }
            gvReport.DataBind();
        }
        catch (Exception X)
        {
            Logger.writeLog(X);
            //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer saved.</span>";
        }

        btnSend.Enabled = (dt!=null && dt.Rows.Count > 0);
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
            decimal amount;
            if (decimal.TryParse(e.Row.Cells[4].Text, out amount))
            {
                e.Row.Cells[4].Text = String.Format("{0:N2}", amount);
            }
            if (decimal.TryParse(e.Row.Cells[5].Text, out amount))
            {
                e.Row.Cells[5].Text = String.Format("{0:N2}", amount);
            }
        }
    }
    protected void ddPOS_Init(object sender, EventArgs e)
    {
        /*
            cmbPOS.DataSource = (new PointOfSaleCollection()).Load();
            cmbPOS.ValueMember = PointOfSale.Columns.PointOfSaleID;
            cmbPOS.DisplayMember = PointOfSale.Columns.PointOfSaleName;
        */

        
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["sortBy"] = null;
        
        BindGrid();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {

        if (dtLineSheet != null)
        {
            dtLineSheet.Rows.Clear();
        }
            BindGrid();
    }



    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
    }
    protected void lnkSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvReport.Rows)
        {
            decimal outstanding = 0;
            Decimal.TryParse(row.Cells[Col_Outstanding].Text, out outstanding);
            if (outstanding > 0)
            {
                ((CheckBox)row.FindControl("CheckBox1")).Checked = true;
            }
        }
    }
    protected void lnkSelectNone_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvReport.Rows)
            ((CheckBox)row.FindControl("CheckBox1")).Checked = false;
    }

    protected void btnCloseHelp_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVAllColumns(dt, this.Page.Title.Trim(' '), this.Page.Title);
    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (dtLineSheet != null && dtLineSheet.Rows.Count > 0)
        {
            Session["LineData"] = dtLineSheet;
            Response.Redirect("LineSheetResult.aspx");
        }
        

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (dtLineSheet == null)
            {
                dtLineSheet = new DataTable();
                dtLineSheet.Columns.Add("ItemNo");
                dtLineSheet.Columns.Add("ItemName");
                dtLineSheet.Columns.Add("CategoryName");
                dtLineSheet.Columns.Add("RetailPrice", typeof(Decimal));
                dtLineSheet.Columns.Add("CostPrice", typeof(Decimal));
                dtLineSheet.Columns.Add("Attributes1");
                dtLineSheet.Columns.Add("Attributes2");
                dtLineSheet.Columns.Add("Attributes3");
                dtLineSheet.Columns.Add("Attributes4");
            }
            foreach (GridViewRow row in gvReport.Rows)
            {
                if (((CheckBox)row.FindControl("CheckBox1")).Checked == true)
                {
                    DataRow dr = dtLineSheet.NewRow();
                    dr["ItemNo"] = row.Cells[1].Text;
                    dr["ItemName"] = row.Cells[2].Text;
                    dr["CategoryName"] = row.Cells[3].Text;
                    dr["RetailPrice"] = decimal.Parse(row.Cells[4].Text);
                    dr["CostPrice"] = decimal.Parse(row.Cells[5].Text);
                    dr["Attributes1"] = row.Cells[6].Text;
                    dr["Attributes2"] = row.Cells[7].Text;
                    dr["Attributes3"] = row.Cells[8].Text;
                    dr["Attributes4"] = row.Cells[9].Text;
                    dtLineSheet.Rows.Add(dr);
                }
            }
            Session["LineData"] = dtLineSheet;
            Response.Redirect("LineSheet.aspx");

        }
        catch (Exception ex) { Logger.writeLog(ex.Message); }
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        //Response.Redirect("SalesImporter.aspx");
    }   
}
