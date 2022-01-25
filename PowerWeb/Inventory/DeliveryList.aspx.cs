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


public partial class DeliveryList : PageBase
{
    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private const int Col_ItemNo = 8;
    private const int Col_Qty = 10;
    private const int Col_OrderDetID = 1;
    private const int Col_Status = 11;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {         
            txtStartDate.Text = DateTime.Today.AddDays(0).ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.AddDays(0).ToString("dd MMM yyyy");
            ViewState["sortBy"] = "";
            ViewState[SORT_DIRECTION] = "";
            BindGrid();
        }
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
            DateTime startDate = DateTime.Parse(txtStartDate.Text);
            DateTime endDate = DateTime.Parse(txtEndDate.Text);

            PointOfSale pos = new PointOfSale("PointOfSaleName", ddPOS.SelectedItem.Text);

            dt = DeliveryController.FetchDeliveryList(txtRefNo.Text, pos.PointOfSaleID, startDate, endDate, txtSearch.Text);


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
            if (e.Row.Cells[Col_Status].Text == "Delivered")
            {
                e.Row.BackColor = System.Drawing.Color.Lime;
                ((CheckBox)e.Row.FindControl("CheckBox1")).Enabled = false;
            }
            else
            {
                ((CheckBox)e.Row.FindControl("CheckBox1")).Enabled = true;
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

        if (Session["DeptID"] != null && Session["DeptID"].ToString() != "0")
        {
            ddPOS.WhereField = "DepartmentID";
            ddPOS.WhereValue = Session["DeptID"].ToString();
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
            ddPOS.SelectedIndex = 0;
            gvReport.PageIndex = 0;

            BindGrid();
    }



    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvReport.DataSource;
        if (dt != null)
        {
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvReport);
        }
        else
        {
            CommonWebUILib.ShowMessage(lblResult, "<b>"+LanguageManager.GetTranslation("Error: Result is empty, cannot be exported.")+"</b>", CommonWebUILib.MessageType.BadNews);
        }
    }
    protected void lnkSelectAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvReport.Rows)
            ((CheckBox)row.FindControl("CheckBox1")).Checked = (row.Cells[Col_Status].Text == "Pending");
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
        string SQLString;

        try
        {
            InventoryController TmpCtrl = null;

            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
                if (CostingMethod == null) CostingMethod = "";
                CostingMethod = CostingMethod.ToLower();

                if (CostingMethod == "fifo")
                    TmpCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);
                else if (CostingMethod == "fixed avg")
                    TmpCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FixedAvg);
                else
                    TmpCtrl = new InventoryController(PowerPOS.Container.CostingMethods.FIFO);

            TmpCtrl.SetInventoryLocation((new PointOfSale(int.Parse(ddPOS.SelectedValue))).Outlet.InventoryLocationID.GetValueOrDefault(0));

            SQLString =
                "IF NOT EXISTS (SELECT * FROM InventoryStockOutReason WHERE ReasonName = 'Delivery') " +
                "INSERT INTO InventoryStockOutReason (ReasonName,CreatedBy,CreatedOn,ModifiedBy,ModifiedOn,Deleted) " +
                "VALUES('Delivery','SYSTEM',GETDATE(),'SYSTEM',GETDATE(),0); " +
                "SELECT MIN(ReasonID) FROM InventoryStockOutReason WHERE ReasonName = 'Delivery' ";

            TmpCtrl.setInventoryStockOutReasonID((int)DataService.ExecuteScalar(new QueryCommand(SQLString)));

            foreach (GridViewRow row in gvReport.Rows)
            {
                if (((CheckBox)row.FindControl("CheckBox1")).Checked == true)
                {
                    if (row.Cells[Col_Status].Text == "Delivered")
                        throw new Exception("(warning)Ref no " + row.Cells[Col_OrderDetID].Text + " has been delivered");

                    TmpCtrl.AddItemIntoInventoryForDelivery
                        (row.Cells[Col_ItemNo].Text
                            , decimal.Parse(row.Cells[Col_Qty].Text)
                            , row.Cells[Col_OrderDetID].Text);
                }
            }

            Session["ToBeDeliveredList"] = TmpCtrl;
            Response.Redirect("StockOut.aspx");
        }
        catch (Exception X)
        {
            if (X.Message.StartsWith("(error)"))
                CommonWebUILib.ShowMessage(lblResult, "<b>Error: " + X.Message + "</b>", CommonWebUILib.MessageType.BadNews);
            if (X.Message.StartsWith("(warning)"))
                CommonWebUILib.ShowMessage(lblResult, "<b>Warning: " + X.Message + "</b>", CommonWebUILib.MessageType.BadNews);
            else
                CommonWebUILib.ShowMessage(lblResult, "<b>Unhandled Error: " + X.Message + "</b>", CommonWebUILib.MessageType.BadNews);
            Logger.writeLog(X);
        }
    }   
}
