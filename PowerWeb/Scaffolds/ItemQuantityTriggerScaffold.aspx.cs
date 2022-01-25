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
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;

public partial class ItemQuantityTriggerScaffold : PageBase
{

    private bool isAdd = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    string id = string.Empty;

    private int colUserfld1 = 8;
    private int colUserfld2 = 9;
    private int colUserfld3 = 10;
    private int colUserfld4 = 11;
    private int colUserfld5 = 12;
    private int colUserfld6 = 13;
    private int colUserfld7 = 14;
    private int colUserfld8 = 15;
    private int colUserfld9 = 16;
    private int colUserfld10 = 17;
   
    protected void Page_Load(object sender, EventArgs e)
    {
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
                {
                    LoadEditor(id);
                }
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

    protected void ddlInventoryLocation_Init(object sender, EventArgs e)
    {
        var data = new InventoryLocationController().FetchAll();
        InventoryLocationCollection Dt = (new InventoryLocationCollection()).Load();
        InventoryLocation Obj = new InventoryLocation();
        var ddl = (DropDownList)sender;
        Obj.InventoryLocationID = 0;
        Obj.InventoryLocationName = "ALL";
        Dt.Insert(0, Obj);
        ddl.DataSource = Dt;
        ddl.DataValueField = InventoryLocation.Columns.InventoryLocationID;
        ddl.DataTextField = InventoryLocation.Columns.InventoryLocationName;
        ddl.DataBind();
    }



    /// <summary>
    /// Loads the editor with data
    /// </summary>
    /// <param name="id"></param>
    void LoadEditor(string id)
    {
        ToggleEditor(true);
        LoadDrops();

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
        {
            trUserfld1.Visible = false;
        }
        else
        {
            lblUserfld1.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1);
        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
        {
            trUserfld2.Visible = false;
        }
        else
        {
            lblUserfld2.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
        {
            trUserfld3.Visible = false;
        }
        else
        {
            lblUserfld3.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
        {
            trUserfld4.Visible = false;
        }
        else
        {
            lblUserfld4.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
        {
            trUserfld5.Visible = false;
        }
        else
        {
            lblUserfld5.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
        {
            trUserfld6.Visible = false;
        }
        else
        {
            lblUserfld6.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
        {
            trUserfld7.Visible = false;
        }
        else
        {
            lblUserfld7.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
        {
            trUserfld8.Visible = false;
        }
        else
        {
            lblUserfld8.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
        {
            trUserfld9.Visible = false;
        }
        else
        {
            lblUserfld9.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9);

        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
        {
            trUserfld10.Visible = false;
        }
        else
        {
            lblUserfld10.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10);

        }

        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            lblID.Text = id.ToString();

            //pull the record
            ItemQuantityTrigger itemQuantityTrigger = new ItemQuantityTrigger(id);

            ctrlItem.Text = itemQuantityTrigger.Item.ItemName;
            ctrlQuantity.Text = itemQuantityTrigger.TriggerQuantity.ToString();
            ctrlTriggerLevel.Text = itemQuantityTrigger.TriggerLevel;
            ctrlInventoryLocation.Text = itemQuantityTrigger.InventoryLocation.InventoryLocationName;

            if (itemQuantityTrigger.CreatedOn.HasValue)
                ctrlCreatedOn.Text = itemQuantityTrigger.CreatedOn.Value.ToString();
            if (itemQuantityTrigger.ModifiedOn.HasValue)
                ctrlModifiedOn.Text = itemQuantityTrigger.ModifiedOn.Value.ToString();
            /*if (itemQuantityTrigger.Deleted.HasValue)
                ctrlDeleted.Checked = itemQuantityTrigger.Deleted.Value;*/

            ctrlCreatedBy.Text = itemQuantityTrigger.CreatedBy;
            ctrlModifiedBy.Text = itemQuantityTrigger.ModifiedBy;

            txtUserfld1.Text = itemQuantityTrigger.Userfld1;
            txtUserfld2.Text = itemQuantityTrigger.Userfld2;
            txtUserfld3.Text = itemQuantityTrigger.Userfld3;
            txtUserfld4.Text = itemQuantityTrigger.Userfld4;
            txtUserfld5.Text = itemQuantityTrigger.Userfld5;
            txtUserfld6.Text = itemQuantityTrigger.Userfld6;
            txtUserfld7.Text = itemQuantityTrigger.Userfld7;
            txtUserfld8.Text = itemQuantityTrigger.Userfld8;
            txtUserfld9.Text = itemQuantityTrigger.Userfld9;
            txtUserfld10.Text = itemQuantityTrigger.Userfld10;

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
        DataTable dt; 
        InventoryLocationCollection inventoryLocationCol = new InventoryLocationCollection();
        dt = inventoryLocationCol.Load().ToDataTable();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ctrlInventoryLocation.Items.Add(dt.Rows[i]["InventoryLocationName"].ToString());
        }
        //load the inventory
        LoadDropsForItem("");
    }

    /// <summary>
    /// Loads the DropDownLists for inventory with parameter 
    /// </summary>
    void LoadDropsForItem(string param)
    {
        ctrlItem.Items.Clear();
        ItemController it = new ItemController();
        DataTable dt = it.SearchItem_PlusPointInfo(param, false);
        for (int i = 0; i < dt.Rows.Count; i++)
        { ctrlItem.Items.Add(dt.Rows[i]["ItemName"].ToString()); }
        
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
            orderBy = "TriggerID";

        if (ViewState[SORT_DIRECTION] == null)
        {
            ViewState[SORT_DIRECTION] = SqlFragment.ASC;
        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
        {
            GridView1.Columns[colUserfld1].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld1].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1);
        }

        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
        {
            GridView1.Columns[colUserfld2].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld2].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
        {
            GridView1.Columns[colUserfld3].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld3].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
        {
            GridView1.Columns[colUserfld4].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld4].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
        {
            GridView1.Columns[colUserfld5].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld5].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
        {
            GridView1.Columns[colUserfld6].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld6].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
        {
            GridView1.Columns[colUserfld7].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld7].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
        {
            GridView1.Columns[colUserfld8].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld8].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
        {
            GridView1.Columns[colUserfld9].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld9].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9);
        }


        if (string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
        {
            GridView1.Columns[colUserfld10].Visible = false;
        }
        else
        {
            GridView1.Columns[colUserfld10].HeaderText = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10);
        }

        dt = ItemQuantityTriggerController.FetchDataWithParameters(ddlInventoryLocation.SelectedValue.GetIntValue(), txtSearchItem.Text);
        
        dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

        DataView dv = new DataView(dt);
        dv.Sort = orderBy + ViewState[SORT_DIRECTION].ToString();
        GridView1.DataSource = dv;
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

            string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b> (" + itemCount.ToString() + " Items)";
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
        //bool haveError = false;
        try
        {
            BindAndSave(id);
            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Item Quantity Trigger saved.") + "</span>";
        }

        catch (Exception x)
        {
            //haveError = true;
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Quantity Trigger not saved:") + "</span> " + x.Message;
        }

        //if(!haveError)
        //  Response.Redirect(Request.CurrentExecutionFilePath);
    }

    /// <summary>
    /// Binds and saves the data
    /// </summary>
    /// <param name="id"></param>
    void BindAndSave(string id)
    {
        int triggerLevel;
        if (!int.TryParse(ctrlTriggerLevel.Text, out triggerLevel)) ctrlTriggerLevel.Text = "0";
        /*if (ctrlPointsPercentage.Text == "") ctrlPointsPercentage.Text = "0";
        if (ctrlSpendingLimit.Text == "") ctrlSpendingLimit.Text = "0";*/

        string itemId= ""; int inventoryLocationId = 0;
        IDataReader reader = null;
        reader = Item.FetchByParameter("ItemName", ctrlItem.Text);
        if (reader.Read())
        {
            itemId = reader["ItemNo"].ToString();
        }

        reader = InventoryLocation.FetchByParameter("InventoryLocationName", ctrlInventoryLocation.Text);
        if (reader.Read())
        {
            inventoryLocationId = Convert.ToInt16(reader["InventoryLocationId"].ToString());
        }
        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            Query qr = ItemQuantityTrigger.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(ItemQuantityTrigger.Columns.TriggerID, id);
            qr.AddUpdateSetting(ItemQuantityTrigger.Columns.ItemNo, itemId);
            qr.AddUpdateSetting(ItemQuantityTrigger.Columns.TriggerQuantity, ctrlQuantity.Text);
            qr.AddUpdateSetting(ItemQuantityTrigger.Columns.TriggerLevel, ctrlTriggerLevel.Text);
            qr.AddUpdateSetting(ItemQuantityTrigger.Columns.InventoryLocationID, inventoryLocationId);
            qr.AddUpdateSetting(MembershipGroup.Columns.ModifiedBy, Session["username"].ToString());
            qr.AddUpdateSetting(MembershipGroup.Columns.ModifiedOn, DateTime.Now);
            qr.AddUpdateSetting(MembershipGroup.Columns.Deleted, ctrlDeleted.Checked);

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld1, txtUserfld1.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld2, txtUserfld2.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld3, txtUserfld3.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld4, txtUserfld4.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld5, txtUserfld5.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld6, txtUserfld6.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld7, txtUserfld7.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld8, txtUserfld8.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld9, txtUserfld9.Text);
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
            {
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld10, txtUserfld10.Text);
            }
            qr.Execute();
        }
        else
        {

            ItemQuantityTrigger mg = new ItemQuantityTrigger();

            mg.IsNew = true;
            //mg.TriggerID = mg.get
            mg.ItemNo = itemId;
            mg.TriggerQuantity = Convert.ToInt16(ctrlQuantity.Text);
            mg.TriggerLevel = ctrlTriggerLevel.Text;
            mg.InventoryLocationID = inventoryLocationId;
            mg.CreatedBy = Session["username"].ToString();
            mg.CreatedOn = DateTime.Now;
            mg.ModifiedBy = Session["username"].ToString();
            mg.ModifiedOn = DateTime.Now;
            mg.Deleted = false;

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1)))
            {
                 mg.Userfld1 = txtUserfld1.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2)))
            {
                mg.Userfld2 = txtUserfld3.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3)))
            {
                mg.Userfld3 = txtUserfld3.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4)))
            {
                mg.Userfld4 = txtUserfld4.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5)))
            {
                mg.Userfld5 = txtUserfld5.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6)))
            {
                mg.Userfld6 = txtUserfld6.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7)))
            {
                mg.Userfld7 = txtUserfld7.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8)))
            {
                mg.Userfld8 = txtUserfld8.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9)))
            {
                mg.Userfld9 = txtUserfld9.Text;
            }

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10)))
            {
                mg.Userfld10 = txtUserfld10.Text;
            }

            mg.Save(Session["username"].ToString());
        }

        //bind it

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ItemQuantityTrigger.Delete(Utility.GetParameter("id"));
        //redirect
        Response.Redirect(Request.CurrentExecutionFilePath);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadDropsForItem(txtItemNo.Text);
    }

    protected void btnSearchItem_Click(object sender, EventArgs e)
    {
        BindGrid(string.Empty);
    }
}
