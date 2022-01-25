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


public partial class StockOnHandReportWithCost : PageBase
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

            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
            {
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

        string sqlAttLabel = "SELECT * FROM AttributesLabel ORDER BY AttributesNo";
        for (int Counter = 4; Counter < 12; Counter++)
            gvReport.Columns[Counter].Visible = false;

        IDataReader Rdr = DataService.GetReader(new QueryCommand(sqlAttLabel));
        while (Rdr.Read())
        {
            switch (Rdr.GetInt32(0))
            {
                case 1: gvReport.Columns[4].Visible = true; gvReport.Columns[4].HeaderText = Rdr.GetString(1); break;
                case 2: gvReport.Columns[5].Visible = true; gvReport.Columns[5].HeaderText = Rdr.GetString(1); break;
                case 3: gvReport.Columns[6].Visible = true; gvReport.Columns[6].HeaderText = Rdr.GetString(1); break;
                case 4: gvReport.Columns[7].Visible = true; gvReport.Columns[7].HeaderText = Rdr.GetString(1); break;
                case 5: gvReport.Columns[8].Visible = true; gvReport.Columns[8].HeaderText = Rdr.GetString(1); break;
                case 6: gvReport.Columns[9].Visible = true; gvReport.Columns[9].HeaderText = Rdr.GetString(1); break;
                case 7: gvReport.Columns[10].Visible = true; gvReport.Columns[10].HeaderText = Rdr.GetString(1); break;
                case 8: gvReport.Columns[11].Visible = true; gvReport.Columns[11].HeaderText = Rdr.GetString(1); break;
            }
        }
        
        DataTable dt = ReportController.FetchStockReport(searchQuery,
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
                e.Row.Cells[13].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[13].Text));
                e.Row.Cells[14].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[14].Text));
            }
            else
            {
                e.Row.Cells[13].Text = "--";
                e.Row.Cells[14].Text = "--";
            }

            e.Row.Cells[15].Text = String.Format("{0:N2}", Decimal.Parse(e.Row.Cells[15].Text));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            int TotalQty;
            decimal TotalCost;
            calculateSum(out TotalQty, out TotalCost);            
            e.Row.Cells[12].Text = TotalQty.ToString();
            if (cbShowCostPrice.Checked)
            {
                e.Row.Cells[14].Text =  TotalCost.ToString("N2");
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (ViewState["sortBy"] == null)
        {
            ViewState["sortBy"] = "";
        }

        int inventoryLocationID = 0;
        int.TryParse(ddlInventoryLocation.SelectedItem.Value.ToString(), out inventoryLocationID);
        string searchQuery = txtSearch.Text;

        DataTable dt = ReportController.FetchStockReport(searchQuery,
                    inventoryLocationID, cbShowCostPrice.Checked, "", ViewState["sortBy"].ToString(),
                    ViewState[SORT_DIRECTION].ToString());

        //delete unnecessary columns
        dt.Columns.Remove("RetailPrice");
        dt.Columns.Remove("Stock In");
        dt.Columns.Remove("Stock Out");
        dt.Columns.Remove("Transfer In");
        dt.Columns.Remove("Transfer Out");
        dt.Columns.Remove("Adjustment In");
        dt.Columns.Remove("Adjustment Out");
        dt.Columns.Remove("Attributes1");
        dt.Columns.Remove("Attributes2");
        dt.Columns.Remove("Attributes3");
        dt.Columns.Remove("Attributes4");
        dt.Columns.Remove("Attributes5");
        dt.Columns.Remove("Attributes6");
        dt.Columns.Remove("Attributes7");
        dt.Columns.Remove("Attributes8");
        dt.Columns.Remove("CostOfGoods");
        dt.Columns.Remove("TotalCost");

        DataTable dt1 = new DataTable();

        dt1.Columns.Add("DepartmentName", Type.GetType("System.String"));
        dt1.Columns.Add("ItemNo", Type.GetType("System.String"));
        dt1.Columns.Add("CategoryName", Type.GetType("System.String"));
        dt1.Columns.Add("ItemName", Type.GetType("System.String"));
        dt1.Columns.Add("OnHand", Type.GetType("System.Int32"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            int val;
            if (Int32.TryParse(dt.Rows[i]["OnHand"].ToString(), out val) && val > 0)
            {
                dt1.ImportRow(dt.Rows[i]);
            }

        }
        //DataRow[] dr = dt.Select("OnHand > 0 AND OnHand <> 0");

        //dt = dr.CopyToDataTable();

        //clearing value on stock on hand column
        dt1.Columns.Remove("OnHand");

        //re-add the column
        dt1.Columns.Add("OnHand");

        CommonWebUILib.ExportCSVAllColumns(dt1, "StockTakeForm_", "Stock Take Form");
    }

}
