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
using System.Collections.Generic;
using System.Linq;

using PowerPOS;
using SubSonic;
using SubSonic.Utilities;


public partial class StockOnHandReport : PageBase
{    
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    private int QuantityColumn = 5;
    private int RetailPriceColumn = 6;
    private int TotalRetailColumn = 7;
    private int COGColumn = 8;
    private int TotalCostColumn = 9;
    private int Attributes1Column = 10;
    private int Attributes2Column = 11;
    private int Attributes3Column = 12;
    private int Attributes4Column = 13;
    private int Attributes5Column = 14;
    private int Attributes6Column = 15;
    private int Attributes7Column = 16;
    private int Attributes8Column = 17;
    private string InventoryLocationCost = "Inventory Location Cost";
    private string InventoryLocationGroupCost = "Inventory Location Group Cost";
    private string GlobalCost = "Global Cost";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["sortBy"] = "";
            ViewState["IsUseLocationGroup"] = false;
            ViewState[SORT_DIRECTION] = "";
            string status;
            useGroupPrice.Checked = false;

            cbUseInventoryLocationGroup.Visible = false;
            useGlobalPrice.Visible = false;
            useGroupPrice.Visible = false;

            if (InventoryLocationController.IsHaveLocationGroup(out status))
            {
                cbUseInventoryLocationGroup.Visible = true;
            }

            ddlInventoryLocationCost.Items.Clear();
            ddlInventoryLocationCost.Items.Add(new ListItem(InventoryLocationCost));
            ddlInventoryLocationCost.Items.Add(new ListItem(InventoryLocationGroupCost));
            ddlInventoryLocationCost.Items.Add(new ListItem(GlobalCost));
            ddlInventoryLocationCost.DataBind();
            ddlInventoryLocationCost.SelectedIndex = 0;

            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

           
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

        gvReport.Columns[Attributes1Column].Visible = false;
        gvReport.Columns[Attributes2Column].Visible = false;
        gvReport.Columns[Attributes3Column].Visible = false;
        gvReport.Columns[Attributes4Column].Visible = false;
        gvReport.Columns[Attributes5Column].Visible = false;
        gvReport.Columns[Attributes6Column].Visible = false;
        gvReport.Columns[Attributes7Column].Visible = false;
        gvReport.Columns[Attributes8Column].Visible = false;


        IDataReader Rdr = DataService.GetReader(new QueryCommand(sqlAttLabel));
        while (Rdr.Read())
        {
            switch (Rdr.GetInt32(0))
            {
                case 1: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes1Column].Visible = true; gvReport.Columns[Attributes1Column].HeaderText = Rdr.GetString(1); } break;
                case 2: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes2Column].Visible = true; gvReport.Columns[Attributes2Column].HeaderText = Rdr.GetString(1); } break;
                case 3: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes3Column].Visible = true; gvReport.Columns[Attributes3Column].HeaderText = Rdr.GetString(1); } break;
                case 4: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes4Column].Visible = true; gvReport.Columns[Attributes4Column].HeaderText = Rdr.GetString(1); } break;
                case 5: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes5Column].Visible = true; gvReport.Columns[Attributes5Column].HeaderText = Rdr.GetString(1); } break;
                case 6: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes6Column].Visible = true; gvReport.Columns[Attributes6Column].HeaderText = Rdr.GetString(1); } break;
                case 7: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes7Column].Visible = true; gvReport.Columns[Attributes7Column].HeaderText = Rdr.GetString(1); } break;
                case 8: if (!string.IsNullOrEmpty(Rdr.GetString(1))) { gvReport.Columns[Attributes8Column].Visible = true; gvReport.Columns[Attributes8Column].HeaderText = Rdr.GetString(1); } break;
            }
            
        }

        DataSet ds = new DataSet();
        if (ddlInventoryLocation.SelectedValue.GetIntValue() == 0)
        {
            ds = SPs.ReportStockOnHandReportItemGlobal(ddlItemDepartment.SelectedValue, ddlCategory.SelectedValue, ddlInventoryLocation.SelectedValue.GetIntValue(),
                ddlSupplier.SelectedItem.Text, txtSearch.Text).GetDataSet();
        }
        else {
            if (ddlInventoryLocationCost.SelectedValue == InventoryLocationGroupCost)
                ds = SPs.ReportStockOnHandReportItemSummaryGroup(ddlItemDepartment.SelectedValue, ddlCategory.SelectedValue, ddlInventoryLocation.SelectedValue.GetIntValue(),
                ddlSupplier.SelectedItem.Text, txtSearch.Text).GetDataSet();
            else
                ds = SPs.ReportStockOnHandReportItemSummary(ddlItemDepartment.SelectedValue, ddlCategory.SelectedValue, ddlInventoryLocation.SelectedValue.GetIntValue(),
                ddlSupplier.SelectedItem.Text, txtSearch.Text).GetDataSet();
        }

        

        DataTable dt = ds.Tables[0];                          
        gvReport.DataSource = dt;
        gvReport.DataBind(); 
        
    }

    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReport.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void cbUseInventoryLocationGroup_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbUseInventoryLocationGroup.Checked)
        {
            ViewState["IsUseLocationGroup"] = true;
            lblInventoryLocation.Text = "Inventory Location Group";
            LoadInventoryLocation();

            ddlInventoryLocationCost.Items.Clear();
            ddlInventoryLocationCost.Items.Add(new ListItem(InventoryLocationGroupCost));
            ddlInventoryLocationCost.Items.Add(new ListItem(GlobalCost));
            ddlInventoryLocationCost.DataBind();
            ddlInventoryLocationCost.SelectedIndex = 0;
        }
        else
        {
            ViewState["IsUseLocationGroup"] = false;
            lblInventoryLocation.Text = "Inventory Location";
            LoadInventoryLocation();

            ddlInventoryLocationCost.Items.Clear();
            ddlInventoryLocationCost.Items.Add(new ListItem(InventoryLocationCost));
            ddlInventoryLocationCost.Items.Add(new ListItem(InventoryLocationGroupCost));
            ddlInventoryLocationCost.Items.Add(new ListItem(GlobalCost));
            ddlInventoryLocationCost.DataBind();
            ddlInventoryLocationCost.SelectedIndex = 0;
        }
    }

    private void calculateSum(out decimal TotalQty, out decimal TotalCost)
    {
        try
        {
            TotalQty = 0; TotalCost = 0; 
            DataTable dt = (DataTable)gvReport.DataSource;
            if (dt.Rows.Count > 0)
            {
                TotalQty = decimal.Parse(dt.Compute("SUM(Quantity)", "").ToString());
                TotalCost = decimal.Parse(dt.Compute("SUM(TotalCost)", "").ToString());
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
         
            decimal tmp = 0;
            if (Decimal.TryParse(e.Row.Cells[QuantityColumn].Text,out tmp))
                e.Row.Cells[QuantityColumn].Text = String.Format("{0:N4}", tmp);

            if (Decimal.TryParse(e.Row.Cells[RetailPriceColumn].Text, out tmp))
                e.Row.Cells[RetailPriceColumn].Text = String.Format("{0:N2}", tmp);

            if (Decimal.TryParse(e.Row.Cells[TotalRetailColumn].Text, out tmp))
                e.Row.Cells[TotalRetailColumn].Text = String.Format("{0:N2}", tmp);

            if (Decimal.TryParse(e.Row.Cells[COGColumn].Text, out tmp))
                e.Row.Cells[COGColumn].Text = String.Format("{0:N4}", tmp);

            if (Decimal.TryParse(e.Row.Cells[TotalCostColumn].Text, out tmp))
                e.Row.Cells[TotalCostColumn].Text = String.Format("{0:N4}", tmp);

            
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            decimal TotalQty;
            decimal TotalCost;
            calculateSum(out TotalQty, out TotalCost);
            e.Row.Cells[QuantityColumn].Text = TotalQty.ToString("N4");
            e.Row.Cells[TotalCostColumn].Text =  TotalCost.ToString("N4");                                      
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
        DataTable dt = new DataTable();
            dt = (DataTable)gvReport.DataSource;
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

        DataSet ds = SPs.ReportStockOnHandReportInvLocGroup(ddlItemDepartment.SelectedValue, ddlCategory.SelectedValue, ddlInventoryLocation.SelectedValue.GetIntValue(),
                ddlSupplier.SelectedItem.Text, txtSearch.Text, cbUseInventoryLocationGroup.Checked).GetDataSet();

        DataTable dt = ds.Tables[0];  

        //delete unnecessary columns
        dt.Columns.Remove("RetailPrice");
        dt.Columns.Remove("TotalRetail");
        dt.Columns.Remove("Attributes1");
        dt.Columns.Remove("Attributes2");
        dt.Columns.Remove("Attributes3");
        dt.Columns.Remove("Attributes4");
        dt.Columns.Remove("Attributes5");
        dt.Columns.Remove("Attributes6");
        dt.Columns.Remove("Attributes7");
        dt.Columns.Remove("Attributes8");
        dt.Columns.Remove("COG");
        dt.Columns.Remove("TotalCost");
        dt.Columns.Remove("COGGroup");
        dt.Columns.Remove("TotalCostGroup");
        dt.Columns.Remove("GlobalCostGroup");
        dt.Columns.Remove("TotalGlobalCostGroup");


        DataTable dt1 = new DataTable();

        dt1.Columns.Add("DepartmentName", Type.GetType("System.String"));
        dt1.Columns.Add("ItemNo", Type.GetType("System.String"));
        dt1.Columns.Add("CategoryName", Type.GetType("System.String"));
        dt1.Columns.Add("ItemName", Type.GetType("System.String"));
        dt1.Columns.Add("Quantity", Type.GetType("System.Int32"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            decimal val;
            if (decimal.TryParse(dt.Rows[i]["Quantity"].ToString(), out val) && val > 0)
            {
                dt1.ImportRow(dt.Rows[i]);
            }

        }
        //DataRow[] dr = dt.Select("OnHand > 0 AND OnHand <> 0");

        //dt = dr.CopyToDataTable();

        //clearing value on stock on hand column
        dt1.Columns.Remove("Quantity");

        //re-add the column
        dt1.Columns.Add("Quantity");

        CommonWebUILib.ExportCSVAllColumns(dt1, "StockTakeForm_", "Stock Take Form");
    }

    protected void ddlInventoryLocation_Init(object sender, EventArgs e)
    {
        LoadInventoryLocation();
    }

    protected void ddlItemDepartment_Init(object sender, EventArgs e)
    {
        Query qr = ItemDepartment.CreateQuery();
        qr.AddWhere(ItemDepartment.Columns.Deleted, false);

        ItemDepartmentCollection Dt = new ItemDepartmentCollection();
            Dt.LoadAndCloseReader(DataService.GetReader( qr.BuildCommand()));
        ItemDepartment Obj = new ItemDepartment();
        Obj.ItemDepartmentID = "ALL";
        Obj.DepartmentName = "ALL";
        Dt.Insert(0, Obj);
        ddlItemDepartment.DataSource = Dt;
        ddlItemDepartment.DataValueField = ItemDepartment.Columns.ItemDepartmentID;
        ddlItemDepartment.DataTextField = ItemDepartment.Columns.DepartmentName;
        ddlItemDepartment.DataBind();
        ddlItemDepartment.DataBind();
    }

    protected void ddlCategory_Init(object sender, EventArgs e)
    {
        Query qr = Category.CreateQuery();
        qr.AddWhere(Category.Columns.Deleted, false);

        CategoryCollection Dt = new CategoryCollection();
        Dt.LoadAndCloseReader(DataService.GetReader(qr.BuildCommand()));
        Category Obj = new Category();
        Obj.CategoryName = "ALL";
        Obj.CategoryName = "ALL";
        Dt.Insert(0, Obj);
        ddlCategory.DataSource = Dt;
        ddlCategory.DataValueField = Category.Columns.CategoryName;
        ddlCategory.DataTextField = Category.Columns.CategoryName;
        ddlCategory.DataBind();
    }

    protected void ddlSupplier_Init(object sender, EventArgs e)
    {
        SupplierCollection Dt = (new SupplierCollection()).Load();
        Supplier Obj = new Supplier();
        Obj.SupplierID = 0;
        Obj.SupplierName = "ALL";
        Dt.Insert(0, Obj);
        ddlSupplier.DataSource = Dt;
        ddlSupplier.DataValueField = Supplier.Columns.SupplierID;
        ddlSupplier.DataTextField = Supplier.Columns.SupplierName;
        ddlSupplier.DataBind();
    }

    protected void LoadInventoryLocation()
    {
        if (ViewState["IsUseLocationGroup"] == null || (bool) ViewState["IsUseLocationGroup"] == false)
        {
            var data = new InventoryLocationController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.InventoryLocationName).ToList();
            data.Insert(0, new InventoryLocation { InventoryLocationID = 0, InventoryLocationName = "ALL" });
            
            ddlInventoryLocation.DataValueField = InventoryLocation.Columns.InventoryLocationID;
            ddlInventoryLocation.DataTextField = InventoryLocation.Columns.InventoryLocationName;
            ddlInventoryLocation.DataSource = data;
            ddlInventoryLocation.DataBind();
        }
        else {
            var data = new InventoryLocationGroupController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.InventoryLocationGroupName).ToList();
            data.Insert(0, new InventoryLocationGroup { InventoryLocationGroupID = 0, InventoryLocationGroupName = "ALL" });

            ddlInventoryLocation.DataValueField = InventoryLocationGroup.Columns.InventoryLocationGroupID;
            ddlInventoryLocation.DataTextField = InventoryLocationGroup.Columns.InventoryLocationGroupName;
            ddlInventoryLocation.DataSource = data;
            ddlInventoryLocation.DataBind();
        }
    
    }

}
