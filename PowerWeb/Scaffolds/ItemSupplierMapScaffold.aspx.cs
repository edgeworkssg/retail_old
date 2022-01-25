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

public partial class ItemSupplierMapScaffold : PageBase
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
            listCtrl[i].Visible = i < MaxPackingSizeOnItemSupplierMap;

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
            lblID.Text = id.ToString();

            //pull the record
            ItemSupplierMap itemSupplierMap = new ItemSupplierMap(id);

            ctrlItem.Text = itemSupplierMap.Item.ItemName;
            ctrlCostPrice.Text = itemSupplierMap.CostPrice.ToString(NumDigit);
            ctrlSupplier.Text = itemSupplierMap.Supplier.SupplierName;
            chkIsPreferredSupplier.Checked = itemSupplierMap.IsPreferredSupplier.GetValueOrDefault(false);

            try
            {
                ctrlSupplier.SelectedValue = itemSupplierMap.Supplier.SupplierName;
                lblCurrency.Text = itemSupplierMap.Supplier.Currency;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            if (itemSupplierMap.CreatedOn.HasValue)
                ctrlCreatedOn.Text = itemSupplierMap.CreatedOn.Value.ToString();
            if (itemSupplierMap.ModifiedOn.HasValue)
                ctrlModifiedOn.Text = itemSupplierMap.ModifiedOn.Value.ToString();
            /*if (itemQuantityTrigger.Deleted.HasValue)
                ctrlDeleted.Checked = itemQuantityTrigger.Deleted.Value;*/

            ctrlCreatedBy.Text = itemSupplierMap.CreatedBy;
            ctrlModifiedBy.Text = itemSupplierMap.ModifiedBy;

            if(!string.IsNullOrEmpty(itemSupplierMap.Currency)) ddlCurrency.SelectedValue = itemSupplierMap.Currency;
            ddlGST.SelectedIndex = itemSupplierMap.GSTRule.GetValueOrDefault(0);

            if (itemSupplierMap.GSTRule.HasValue && itemSupplierMap.GSTRule.Value == 0)
                ddlGST.SelectedIndex = 3;

            txtPackingSize1.Text = itemSupplierMap.PackingSize1;
            txtPackingSize2.Text = itemSupplierMap.PackingSize2;
            txtPackingSize3.Text = itemSupplierMap.PackingSize3;
            txtPackingSize4.Text = itemSupplierMap.PackingSize4;
            txtPackingSize5.Text = itemSupplierMap.PackingSize5;
            txtPackingSize6.Text = itemSupplierMap.PackingSize6;
            txtPackingSize7.Text = itemSupplierMap.PackingSize7;
            txtPackingSize8.Text = itemSupplierMap.PackingSize8;
            txtPackingSize9.Text = itemSupplierMap.PackingSize9;
            txtPackingSize10.Text = itemSupplierMap.PackingSize10;

            if(itemSupplierMap.PackingSizeUOM1.HasValue) 
                txtPackingSizeUOM1.Text = itemSupplierMap.PackingSizeUOM1.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM2.HasValue)
                txtPackingSizeUOM2.Text = itemSupplierMap.PackingSizeUOM2.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM3.HasValue)
                txtPackingSizeUOM3.Text = itemSupplierMap.PackingSizeUOM3.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM4.HasValue)
                txtPackingSizeUOM4.Text = itemSupplierMap.PackingSizeUOM4.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM5.HasValue)
                txtPackingSizeUOM5.Text = itemSupplierMap.PackingSizeUOM5.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM6.HasValue)
                txtPackingSizeUOM6.Text = itemSupplierMap.PackingSizeUOM6.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM7.HasValue)
                txtPackingSizeUOM7.Text = itemSupplierMap.PackingSizeUOM7.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM8.HasValue)
                txtPackingSizeUOM8.Text = itemSupplierMap.PackingSizeUOM8.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM9.HasValue)
                txtPackingSizeUOM9.Text = itemSupplierMap.PackingSizeUOM9.GetValueOrDefault(0).ToString("N2");
            if (itemSupplierMap.PackingSizeUOM10.HasValue)
                txtPackingSizeUOM10.Text = itemSupplierMap.PackingSizeUOM10.GetValueOrDefault(0).ToString("N2");

            if (itemSupplierMap.CostPrice1.HasValue)
                txtCostPrice1.Text = itemSupplierMap.CostPrice1.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice2.HasValue)
                txtCostPrice2.Text = itemSupplierMap.CostPrice2.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice3.HasValue)
                txtCostPrice3.Text = itemSupplierMap.CostPrice3.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice4.HasValue)
                txtCostPrice4.Text = itemSupplierMap.CostPrice4.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice5.HasValue)
                txtCostPrice5.Text = itemSupplierMap.CostPrice5.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice6.HasValue)
                txtCostPrice6.Text = itemSupplierMap.CostPrice6.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice7.HasValue)
                txtCostPrice7.Text = itemSupplierMap.CostPrice7.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice8.HasValue)
                txtCostPrice8.Text = itemSupplierMap.CostPrice8.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice9.HasValue)
                txtCostPrice9.Text = itemSupplierMap.CostPrice9.GetValueOrDefault(0).ToString(NumDigit);
            if (itemSupplierMap.CostPrice10.HasValue)
                txtCostPrice10.Text = itemSupplierMap.CostPrice10.GetValueOrDefault(0).ToString(NumDigit);
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
        if (ctrlTriggerLevel.Text == "") ctrlTriggerLevel.Text = "0";
        /*if (ctrlPointsPercentage.Text == "") ctrlPointsPercentage.Text = "0";
        if (ctrlSpendingLimit.Text == "") ctrlSpendingLimit.Text = "0";*/

        string itemId = ""; int supplierId = 0;
        IDataReader reader = null;
        reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
        if (reader.Read())
        {
            itemId = reader["ItemNo"].ToString();
        }

        reader = Supplier.FetchByParameter("SupplierName", ctrlSupplier.Text);
        if (reader.Read())
        {
            supplierId = Convert.ToInt16(reader["SupplierId"].ToString());
        }
        int gst = ddlGST.SelectedIndex;
        if (ddlGST.SelectedIndex == 3) 
            gst = 0;

        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            Query qr = ItemSupplierMap.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(ItemSupplierMap.Columns.ItemSupplierMapID, id);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.ItemNo, itemId);
            //qr.AddUpdateSetting(ItemSupplierMap.Columns.TriggerQuantity, ctrlQuantity.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice, ctrlCostPrice.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.SupplierID, supplierId);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.ModifiedBy, Session["username"].ToString());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.ModifiedOn, DateTime.Now);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.Deleted, ctrlDeleted.Checked);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.Currency, ddlCurrency.SelectedValue);

            qr.AddUpdateSetting(ItemSupplierMap.Columns.GSTRule, gst);

            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize1, txtPackingSize1.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize2, txtPackingSize2.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize3, txtPackingSize3.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize4, txtPackingSize4.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize5, txtPackingSize5.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize6, txtPackingSize6.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize7, txtPackingSize7.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize8, txtPackingSize8.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize9, txtPackingSize9.Text);
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSize10, txtPackingSize10.Text);

            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM1, txtPackingSizeUOM1.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM2, txtPackingSizeUOM2.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM3, txtPackingSizeUOM3.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM4, txtPackingSizeUOM4.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM5, txtPackingSizeUOM5.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM6, txtPackingSizeUOM6.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM7, txtPackingSizeUOM7.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM8, txtPackingSizeUOM8.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM9, txtPackingSizeUOM9.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.PackingSizeUOM10, txtPackingSizeUOM10.Text.GetDecimalValue());

            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice1, txtCostPrice1.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice2, txtCostPrice2.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice3, txtCostPrice3.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice4, txtCostPrice4.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice5, txtCostPrice5.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice6, txtCostPrice6.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice7, txtCostPrice7.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice8, txtCostPrice8.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice9, txtCostPrice9.Text.GetDecimalValue());
            qr.AddUpdateSetting(ItemSupplierMap.Columns.CostPrice10, txtCostPrice10.Text.GetDecimalValue());
            qr.Execute();
        }
        else
        {

            ItemSupplierMap mg = new ItemSupplierMap();

            mg.IsNew = true;
            //mg.TriggerID = mg.get
            mg.ItemNo = itemId;
            mg.CostPrice = Convert.ToDecimal(ctrlCostPrice.Text);
            //mg.TriggerLevel = ctrlTriggerLevel.Text;
            mg.SupplierID = supplierId;
            mg.GSTRule = gst;
            mg.Currency = ddlCurrency.SelectedValue;
            mg.PackingSize1 = txtPackingSize1.Text;
            mg.PackingSize2 = txtPackingSize2.Text;
            mg.PackingSize3 = txtPackingSize3.Text;
            mg.PackingSize4 = txtPackingSize4.Text;
            mg.PackingSize5 = txtPackingSize5.Text;
            mg.PackingSize6 = txtPackingSize6.Text;
            mg.PackingSize7 = txtPackingSize7.Text;
            mg.PackingSize8 = txtPackingSize8.Text;
            mg.PackingSize9 = txtPackingSize9.Text;
            mg.PackingSize10 = txtPackingSize10.Text;

            mg.PackingSizeUOM1 = txtPackingSizeUOM1.Text.GetDecimalValue();
            mg.PackingSizeUOM2 = txtPackingSizeUOM2.Text.GetDecimalValue();
            mg.PackingSizeUOM3 = txtPackingSizeUOM3.Text.GetDecimalValue();
            mg.PackingSizeUOM4 = txtPackingSizeUOM4.Text.GetDecimalValue();
            mg.PackingSizeUOM5 = txtPackingSizeUOM5.Text.GetDecimalValue();
            mg.PackingSizeUOM6 = txtPackingSizeUOM6.Text.GetDecimalValue();
            mg.PackingSizeUOM7 = txtPackingSizeUOM7.Text.GetDecimalValue();
            mg.PackingSizeUOM8 = txtPackingSizeUOM8.Text.GetDecimalValue();
            mg.PackingSizeUOM9 = txtPackingSizeUOM9.Text.GetDecimalValue();
            mg.PackingSizeUOM10 = txtPackingSizeUOM10.Text.GetDecimalValue();

            mg.CostPrice1 = txtCostPrice1.Text.GetDecimalValue();
            mg.CostPrice2 = txtCostPrice2.Text.GetDecimalValue();
            mg.CostPrice3 = txtCostPrice3.Text.GetDecimalValue();
            mg.CostPrice4 = txtCostPrice4.Text.GetDecimalValue();
            mg.CostPrice5 = txtCostPrice5.Text.GetDecimalValue();
            mg.CostPrice6 = txtCostPrice6.Text.GetDecimalValue();
            mg.CostPrice7 = txtCostPrice7.Text.GetDecimalValue();
            mg.CostPrice8 = txtCostPrice8.Text.GetDecimalValue();
            mg.CostPrice9 = txtCostPrice9.Text.GetDecimalValue();
            mg.CostPrice10 = txtCostPrice10.Text.GetDecimalValue();

            mg.CreatedBy = Session["username"].ToString();
            mg.CreatedOn = DateTime.Now;
            mg.ModifiedBy = Session["username"].ToString();
            mg.ModifiedOn = DateTime.Now;
            mg.Deleted = false;
            mg.Save(Session["username"].ToString());
        }

        var qry = new Query("ItemSupplierMap");
        qry.AddWhere(ItemSupplierMap.Columns.ItemNo, itemId);
        qry.AddWhere(ItemSupplierMap.Columns.SupplierID, Comparison.NotEquals, supplierId);
        var existingData = new ItemSupplierMapController().FetchByQuery(qry).Where(o => o.Deleted == false).FirstOrDefault();
        if (existingData == null)
        {
            chkIsPreferredSupplier.Checked = true;
        }

        if (chkIsPreferredSupplier.Checked)
            ItemSupplierMapController.SetPreferredSupplier(supplierId, itemId, Session["username"] + "");

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
        CheckExisting();
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

    private bool CheckExisting()
    {
        bool isSuccess = true;

        //if ((Request.QueryString["id"] + "").GetIntValue() == 0)
        //{
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

            var qr = new Query("ItemSupplierMap");
            qr.AddWhere(ItemSupplierMap.Columns.ItemNo, itemNo);
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
}
