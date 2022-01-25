using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using System.IO;
using System.Linq;

public partial class ItemSupplierMapNewScaffold : PageBase
{

    private bool isAdd = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    string id = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!this.IsPostBack)
            SetDisplay();
        if (Request.QueryString["id"] != null)
        {
            id = Utility.GetParameter("id");
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                if (!Page.IsPostBack)
                {
                    LoadEditor(id);
                }

            }

            else
            {
                //it's an add, show the editor
                isAdd = true;
                ToggleEditor(true);
                if (!Page.IsPostBack)
                    LoadDrops();
                //btnDelete.Visible = false;
            }

        }

        else
        {
            ToggleEditor(false);
            if (!Page.IsPostBack)
            {
                BindGrid(String.Empty);
            }

        }
    }

    private void SetDisplay()
    {
        bool DisplayCurrencyOnItemSupplierMap = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DisplayCurrencyOnItemSupplierMap), false);
        //bool DisplayGSTOnItemSupplierMap = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DisplayGSTOnItemSupplierMap), false);
        int MaxPackingSizeOnItemSupplierMap = (AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.MaxPackingSizeOnItemSupplierMap) + "").GetIntValue();
        rowCurrency.Visible = DisplayCurrencyOnItemSupplierMap;
        //rowGST.Visible = DisplayGSTOnItemSupplierMap;
        var listCtrl = new List<Control>();
        listCtrl.Add(rowPS1);
        listCtrl.Add(rowPS2);
        listCtrl.Add(rowPS3);
        listCtrl.Add(rowPS4);
        listCtrl.Add(rowPS5);
        listCtrl.Add(rowPS6);
        listCtrl.Add(rowPS7);
        listCtrl.Add(rowPS8);
        listCtrl.Add(rowPS9);
        listCtrl.Add(rowPS10);
        for (int i = 0; i < listCtrl.Count; i++)
            listCtrl[i].Visible = false;

        for (int i = 0; i < GridView1.Columns.Count; i++)
        {
            string header = GridView1.Columns[i].HeaderText;
            if (header == "Currency")
                GridView1.Columns[i].Visible = DisplayCurrencyOnItemSupplierMap;
            else if (header == "GST Rule")
                GridView1.Columns[i].Visible = false;// DisplayGSTOnItemSupplierMap;
        }

    }

    /// <summary>
    /// Loads the editor with data
    /// </summary>
    /// <param name="id"></param>
    void LoadEditor(string id)
    {
        //Load the setting
        string NumDigit = "N" + (String.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit)) ? "2" : AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit));

        ToggleEditor(true);
        LoadDrops();
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            //pull the record
            ItemSupplierMap itm;
            ItemSupplierMap itemSupplierMap = new ItemSupplierMap(id);

            //load the main Item
            int SupplierID = itemSupplierMap.SupplierID;
            string status = "";
            string MainItemNo = ItemSupplierMapController.FetchMainItemNonInvProduct(itemSupplierMap.ItemNo, out status);

            if (status != "")
            {
                lblResult.Text = status;
                return;
            }            

            if (itemSupplierMap.ItemNo == MainItemNo)
                itm = itemSupplierMap;
            else {
                ItemSupplierMapCollection col = new ItemSupplierMapCollection();
                col.Where(ItemSupplierMap.Columns.ItemNo, MainItemNo);
                col.Where(ItemSupplierMap.Columns.SupplierID, SupplierID);
                col.Load();

                if (col.Count() > 0)
                    itm = col[0];
                else
                    itm = itemSupplierMap;
            }

            lblID.Text = itm.SupplierID.ToString();
            ctrlItem.Text = itm.Item.ItemName;
            //ctrlCostPrice.Text = itm.CostPrice.ToString(NumDigit);
            ctrlSupplier.Text = itm.Supplier.SupplierName;
            try
            {
                ctrlSupplier.SelectedValue = itm.Supplier.SupplierName;
                lblCurrency.Text = itm.Supplier.Currency;
                ctrlSupplier.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            rowSearchItem.Visible = false;
            ctrlItem.Enabled = false;

            if (itm.CreatedOn.HasValue)
                ctrlCreatedOn.Text = itm.CreatedOn.Value.ToString();
            if (itm.ModifiedOn.HasValue)
                ctrlModifiedOn.Text = itm.ModifiedOn.Value.ToString();
            /*if (itemQuantityTrigger.Deleted.HasValue)
                ctrlDeleted.Checked = itemQuantityTrigger.Deleted.Value;*/

            txtMOQ.Text = itm.PackingSizeUOM10.GetValueOrDefault(0).ToString("N2");

            ctrlCreatedBy.Text = itm.CreatedBy;
            ctrlModifiedBy.Text = itm.ModifiedBy;

            if (!string.IsNullOrEmpty(itm.Currency)) ddlCurrency.SelectedValue = itm.Currency;
            ddlGST.SelectedIndex = itm.GSTRule.GetValueOrDefault(0);

            if (itm.GSTRule.HasValue && itm.GSTRule.Value == 0)
                ddlGST.SelectedIndex = 3;

            LoadPackageSizeFromTable(MainItemNo, SupplierID);

            ctrlItem_SelectedIndexChanged(ctrlItem, new EventArgs());
            //set the delete confirmation
            btnDelete.Attributes.Add("onclick", "return CheckDelete();");
        }

    }

    /// <summary>
    /// Loads the DropDownLists
    /// </summary>
    void LoadDrops()
    {
        //load the listboxes
        ctrlSupplier.Items.Clear();
        DataTable dt;
        SupplierCollection sup = new SupplierCollection();
        sup.OrderByAsc(Supplier.Columns.SupplierName);
        dt = sup.Load().ToDataTable();
        ctrlSupplier.Items.Add(LanguageManager.GetTranslation("-- Please select supplier --"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ctrlSupplier.Items.Add(dt.Rows[i]["SupplierName"].ToString());
        }
        //load the inventory
        LoadDropsForItem("");
        //ctrlItem_SelectedIndexChanged(ctrlItem, new EventArgs());
    }

    /// <summary>
    /// Loads the DropDownLists for inventory with parameter 
    /// </summary>
    void LoadDropsForItem(string param)
    {
        ctrlItem.Items.Clear();
        ItemController it = new ItemController();
        DataTable dt = it.SearchItem_PlusPointInfo(param, false);
        DataView dv = dt.DefaultView;
        dv.Sort = "ItemName ASC";
        DataTable sorteddt = dv.ToTable();
        ctrlItem.Items.Add(LanguageManager.GetTranslation("-- Please select item --"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ctrlItem.Items.Add(sorteddt.Rows[i]["ItemName"].ToString());
        }

        string currencies = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.AvailableCurrency)) ? "SGD,USD,EUR,IDR,JPY,AUD" : AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.AvailableCurrency);
        string defaultCurrency = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency)) ? "SGD" : AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency);
        var currenciesData = currencies.Split(',');
        ddlCurrency.DataSource = currenciesData;
        ddlCurrency.DataBind();
        ddlCurrency.SelectedValue = defaultCurrency;
        
    }

    /// <summary>
    /// Shows/Hides the Grid and Editor panels
    /// </summary>
    /// <param name="showIt"></param>
    void ToggleEditor(bool showIt)
    {
        pnlEdit.Visible = showIt;
        pnlGrid.Visible = !showIt;
    }

    /// <summary>
    /// Binds the GridView
    /// </summary>
    private void BindGrid(string orderBy)
    {
        DataTable dt;
        if (orderBy == string.Empty)
            orderBy = "ItemSupplierMapID";

        dt = ItemSupplierMapController.
             FetchData(
             txtItemName.Text,
             ddlSupplier.SelectedValue.GetIntValue().ToString() 
             );
        
        dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void GridView1_DataBound(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = GridView1.BottomPagerRow;
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
            for (int i = 0; i < GridView1.PageCount; i++)
            {
                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());
                if (i == GridView1.PageIndex)
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
            DataSet ds = GridView1.DataSource as DataSet;
            if (ds != null)
            {
                itemCount = ds.Tables[0].Rows.Count;
            }

            string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b>";
            lblPageCount.Text = pageCount;
        }
        
        Button btnPrev = (Button)gvrPager.Cells[0].FindControl("btnPrev");
        Button btnNext = (Button)gvrPager.Cells[0].FindControl("btnNext");
        Button btnFirst = (Button)gvrPager.Cells[0].FindControl("btnFirst");
        Button btnLast = (Button)gvrPager.Cells[0].FindControl("btnLast");
        //now figure out what page we're on
        if (GridView1.PageIndex == 0)
        {
            btnPrev.Enabled = false;
            btnFirst.Enabled = false;
        }

        else if (GridView1.PageIndex + 1 == GridView1.PageCount)
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

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        string columnName = e.SortExpression;
        //rebind the grid
        if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
        {
            ViewState[SORT_DIRECTION] = SqlFragment.DESC;
        }

        else
        {
            ViewState[SORT_DIRECTION] = SqlFragment.ASC;
        }

        BindGrid(columnName);
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

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid(String.Empty);
    }

    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = GridView1.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        GridView1.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
        BindGrid(String.Empty);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region *) Validation

            if (ctrlItem.SelectedIndex == 0)
            {
                Show("Please select item");
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Supplier Map not saved: Please select item") + "</span> ";
                return;                
            }

            if (ctrlSupplier.SelectedIndex == 0)
            {
                Show("Please select supplier");
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Supplier Map not saved: Please select supplier") + "</span> ";
                return;
            }

            #endregion

            if (!CheckExisting())
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Supplier Map not saved: Item Supplier Map Already Exist") + "</span> ";
                return;
            }
            else
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Item Supplier Map saved.") + "</span>";
            }
        }
        catch (Exception x)
        {
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Supplier Map not saved:") + "</span> " + x.Message;
        }
    }

    /// <summary>
    /// Binds and saves the data
    /// </summary>
    /// <param name="id"></param>
    void BindAndSave(string id)
    {

        var listCtrl = new List<Control>();
        listCtrl.Add(rowPS1);
        listCtrl.Add(rowPS2);
        listCtrl.Add(rowPS3);
        listCtrl.Add(rowPS4);
        listCtrl.Add(rowPS5);
        listCtrl.Add(rowPS6);
        listCtrl.Add(rowPS7);
        listCtrl.Add(rowPS8);
        listCtrl.Add(rowPS9);
        listCtrl.Add(rowPS10);

        if (ctrlTriggerLevel.Text == "") ctrlTriggerLevel.Text = "0";
        /*if (ctrlPointsPercentage.Text == "") ctrlPointsPercentage.Text = "0";
        if (ctrlSpendingLimit.Text == "") ctrlSpendingLimit.Text = "0";*/

        string itemId = ""; int inventoryLocationId = 0;
        IDataReader reader = null;
        reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
        if (reader.Read())
        {
            itemId = reader["ItemNo"].ToString();
        }

        reader = Supplier.FetchByParameter("SupplierName", ctrlSupplier.Text);
        if (reader.Read())
        {
            inventoryLocationId = Convert.ToInt16(reader["SupplierId"].ToString());
        }
        int gst = ddlGST.SelectedIndex;
        if (ddlGST.SelectedIndex == 3)
            gst = 0;

        QueryCommandCollection col = new QueryCommandCollection();

        for (int i = 0; i < listCtrl.Count; i++)
        {
            if (listCtrl[i].Visible)
            {
                ItemSupplierMapCollection itmcol = new ItemSupplierMapCollection();
                ItemSupplierMap itm;

                switch (i)
                {
                    case 0:
                        
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo1.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        
                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo1.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.CostPrice = txtCostPrice1.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if(itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));
                        
                        break;
                    case 1:
                        
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo2.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo2.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice2.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 2:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo3.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo3.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.GSTRule = gst;
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice3.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 3:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo4.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo4.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice4.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 4:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo5.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo5.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice5.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 5:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo6.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo6.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice6.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 6:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo7.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo7.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice7.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 7:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo8.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo8.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice8.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                    case 8:
                        itmcol.Where(ItemSupplierMap.Columns.ItemNo, ItemNo9.Value);
                        itmcol.Where(ItemSupplierMap.Columns.SupplierID, inventoryLocationId);
                        itmcol.Load();

                        if (itmcol.Count > 0)
                            itm = itmcol[0];
                        else
                        {
                            itm = new ItemSupplierMap();
                            itm.ItemNo = ItemNo9.Value;
                            itm.SupplierID = inventoryLocationId;
                        }

                        itm.GSTRule = gst;
                        itm.PackingSizeUOM10 = txtMOQ.Text.GetDecimalValue();
                        itm.Currency = ddlCurrency.SelectedValue;
                        itm.CostPrice = txtCostPrice9.Text.GetDecimalValue();
                        itm.Deleted = false;

                        if (itm.IsNew)
                            col.Add(itm.GetInsertCommand(Session["username"].ToString()));
                        else
                            col.Add(itm.GetUpdateCommand(Session["username"].ToString()));

                        break;
                }
            }
        }

        if (col.Count > 0)
            DataService.ExecuteTransaction(col);

        //bind it

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ItemSupplierMap.Delete(Utility.GetParameter("id"));
        //redirect
        Response.Redirect(Request.CurrentExecutionFilePath);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadDropsForItem(txtItemNo.Text);
        ctrlItem_SelectedIndexChanged(ctrlItem, e);
    }

    protected void ctrlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        IDataReader reader = null;
        reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
        string UOM = "";
        if (reader.Read())
        {
            UOM = reader["userfld1"].ToString();
            MainItemNo.Value = reader["ItemNo"].ToString();
            MainItemName.Value = reader["ItemName"].ToString();
            MainItemBarcode.Value = reader["Barcode"].ToString();
        }
        ltUOM.Text = UOM;
        lblUOM1.Text = UOM;
        lblUOM2.Text = UOM;
        lblUOM3.Text = UOM;
        lblUOM4.Text = UOM;
        lblUOM5.Text = UOM;
        lblUOM6.Text = UOM;
        lblUOM7.Text = UOM;
        lblUOM8.Text = UOM;
        lblUOM9.Text = UOM;
        lblUOM10.Text = UOM;
        
        if (CheckExisting())
            LoadItemMapping();
        
    }

    protected void ctrlSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckExisting();
        IDataReader reader = Supplier.FetchByParameter("SupplierName", ctrlSupplier.Text);
        if (reader.Read())
        {
            ddlGST.SelectedIndex = (reader["userint1"]+"").ToString().GetIntValue();
            lblCurrency.Text = (reader["userfld2"] + "").ToString();
        }
    }

    private void LoadItemMapping()
    {
        IDataReader reader = null;
        reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
        string ItemNo = "";
        if (reader.Read())
        {
            ItemNo = reader["ItemNo"].ToString();
        }

        reader = Supplier.FetchByParameter("SupplierName", ctrlSupplier.Text);
        int SupplierID = 0;
        if (reader.Read())
        {
            SupplierID = reader["SupplierID"].ToString().GetIntValue();
        }

        //if new get the item mapping and load
        if ((Request.QueryString["id"] + "").GetIntValue() == 0 && !string.IsNullOrEmpty(ItemNo) )
        {
            string status = "";
            string MainItemNo = ItemSupplierMapController.FetchMainItemNonInvProduct(ItemNo, out status);

            if (status != "")
            {
                lblResult.Text = status;
                return;
            }

            LoadPackageSizeFromTable(MainItemNo, SupplierID);
        }
    }

    private void LoadPackageSizeFromTable(string ItemNo, int SupplierID)
    {
        string status = "";
        DataTable dt = ItemSupplierMapController.FetchItemSupplierMapNew(ItemNo, SupplierID, out status);
        if (status != "")
        {
            lblResult.Text = status;
            return;
        }

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        txtPackingSize1.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM1.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice1.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize1.Enabled = false;
                        txtPackingSizeUOM1.Enabled = false;
                        rowPS1.Visible = true;
                        ItemNo1.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 1:
                        txtPackingSize2.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM2.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice2.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize2.Enabled = false;
                        txtPackingSizeUOM2.Enabled = false;
                        rowPS2.Visible = true;
                        ItemNo2.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 2:
                        txtPackingSize3.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM3.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice3.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize3.Enabled = false;
                        txtPackingSizeUOM3.Enabled = false;
                        rowPS3.Visible = true;
                        ItemNo3.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 3:
                        txtPackingSize4.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM4.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice4.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize4.Enabled = false;
                        txtPackingSizeUOM4.Enabled = false;
                        rowPS4.Visible = true;
                        ItemNo4.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 4:
                        txtPackingSize5.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM5.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice5.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize5.Enabled = false;
                        txtPackingSizeUOM5.Enabled = false;
                        rowPS5.Visible = true;
                        ItemNo5.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 5:
                        txtPackingSize6.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM6.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice6.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize6.Enabled = false;
                        txtPackingSizeUOM6.Enabled = false;
                        rowPS6.Visible = true;
                        ItemNo6.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 6:
                        txtPackingSize7.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM7.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice7.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize7.Enabled = false;
                        txtPackingSizeUOM7.Enabled = false;
                        rowPS7.Visible = true;
                        ItemNo7.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 7:
                        txtPackingSize8.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM8.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice8.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize8.Enabled = false;
                        txtPackingSizeUOM8.Enabled = false;
                        ItemNo8.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                    case 8:
                        txtPackingSize9.Text = dt.Rows[i]["UOM"].ToString();
                        txtPackingSizeUOM9.Text = dt.Rows[i]["DeductConvType"].ToString() == "1" ? (1 / (dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue() == 0 ? 1 : dt.Rows[i]["DeductConvRate"].ToString().GetDecimalValue())).ToString() : dt.Rows[i]["DeductConvRate"].ToString();
                        txtCostPrice9.Text = dt.Rows[i]["CostPrice"].ToString();
                        txtPackingSize9.Enabled = false;
                        txtPackingSizeUOM9.Enabled = false;
                        rowPS9.Visible = true;
                        ItemNo9.Value = dt.Rows[i]["ItemNo"].ToString();
                        break;
                }
            }
        }
    }

    private bool CheckExisting()
    {
        bool isSuccess = true;

        if ((Request.QueryString["id"] + "").GetIntValue() == 0 && ctrlItem.SelectedIndex != 0)
        {
            try
            {
                IDataReader reader = null;
                reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
                string itemNo = "";
                if (reader.Read())
                    itemNo = reader["ItemNo"].ToString();
                reader = Supplier.FetchByParameter("SupplierName", ctrlSupplier.Text);
                int supplierId = 0;
                if (reader.Read())
                {
                    supplierId = reader["SupplierId"].ToString().GetIntValue();
                    //ddlGST.SelectedIndex = reader["userint1"].ToString().GetIntValue();
                }

                string status = "";
                string MainItemNo = ItemSupplierMapController.FetchMainItemNonInvProduct(itemNo, out status);

                if (status != "")
                {
                    lblResult.Text = status;
                    return false;
                }

                var qr = new Query("ItemSupplierMap");
                qr.AddWhere(ItemSupplierMap.Columns.ItemNo, MainItemNo);
                qr.AddWhere(ItemSupplierMap.Columns.SupplierID, supplierId);
                var existingData = new ItemSupplierMapController().FetchByQuery(qr).Where(o => o.Deleted == false).FirstOrDefault();
                if (existingData != null)
                {
                    if (existingData.ItemSupplierMapID == (Request.QueryString["id"] + "").GetIntValue()
                        && existingData.ItemNo == itemNo
                        && existingData.SupplierID == supplierId)
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        isSuccess = false;
                        Show("Item Supplier Map Already exist!");
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }
        }
        return isSuccess;
    }

    public void Show(string message)
    {
        string cleanMessage = message.Replace("'", "\'");
        Page page = HttpContext.Current.CurrentHandler as Page;
        string script = string.Format("alert('{0}');", cleanMessage);
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
        {
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "alert", script, true /* addScriptTags */);
        }
    }

    protected void btnSearchData_Click(object sender, EventArgs e)
    {
        BindGrid(String.Empty);
    }

    protected void ddlSupplier_Init(object sender, EventArgs e)
    {
        var ddl = (DropDownList)sender;
        ddl.Items.Clear();
        DataTable dt;
        SupplierCollection sup = new SupplierCollection();
        dt = sup.Load().ToDataTable();
        DataView dv = dt.DefaultView;
        dv.Sort = "SupplierName ASC";
        DataTable sortedDt = dv.ToTable();
        ddl.Items.Add(new ListItem { Text = "ALL", Value = "0" });
        for (int i = 0; i < sortedDt.Rows.Count; i++)
        {
            ddl.Items.Add(new ListItem 
            {
                Text = sortedDt.Rows[i]["SupplierName"].ToString(),
                Value = sortedDt.Rows[i]["SupplierID"].ToString() 
            });
        }
    }

    protected void ddlItemName_Init(object sender, EventArgs e)
    {
        var ddl = (DropDownList)sender;
        ddl.Items.Clear();
        ItemController it = new ItemController();
        DataTable dt = it.SearchItem_PlusPointInfo("", false);
        DataView dv = dt.DefaultView;
        DataTable sorteddt = dv.ToTable();
        ctrlItem.Items.Add("-- Please select item --");
        
        ddl.Items.Add(new ListItem { Text = "ALL", Value = "0" });

        for (int i = 0; i < sorteddt.Rows.Count; i++)
        {
            ddl.Items.Add(new ListItem
            {
                Text = dt.Rows[i]["ItemName"].ToString(),
                Value = dt.Rows[i]["ItemNo"].ToString()
            });
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlSupplier.SelectedIndex = 0;
        txtItemName.Text = "";
        BindGrid("");
    }

    protected void btnAddNIP_Click(object sender, EventArgs e)
    {
        IDataReader reader = null;
        reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
        string ItemNo = "";
        if (reader.Read())
        {
            ItemNo = reader["ItemNo"].ToString();
        }

        reader = Supplier.FetchByParameter("SupplierName", ctrlSupplier.Text);
        int SupplierID = 0;
        if (reader.Read())
        {
            SupplierID = reader["SupplierID"].ToString().GetIntValue();
        }

        
        string status = "";
        string MainItemNo = ItemSupplierMapController.FetchMainItemNonInvProduct(ItemNo, out status);

        if (status != "")
        {
            lblResult.Text = status;
            return;
        }

        LoadPackageSizeFromTable(MainItemNo, SupplierID);
        
    }
}
