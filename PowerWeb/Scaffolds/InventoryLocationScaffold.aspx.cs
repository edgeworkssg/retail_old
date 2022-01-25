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
using System.Linq;
using System.Collections.Generic;

public partial class InventoryLocationScaffold : PageBase
{
    private bool isAdd = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            string id = Utility.GetParameter("id");
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
                btnDelete.Visible = false;
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

    /// <summary>
    /// Loads the editor with data
    /// </summary>
    /// <param name="id"></param>
    void LoadEditor(string id)
    {
        ToggleEditor(true);
        LoadDrops();
        if (!string.IsNullOrEmpty(id) && id != "0")
        {
            hdfID.Value = id.ToString();
            txtID.Text = id.ToString();

            //pull the record
            InventoryLocation item = new InventoryLocation(id);
            txtLocationName.Text = item.InventoryLocationName;
            txtDisplayedName.Text = item.DisplayedName;
            txtAddress1.Text = item.Address1;
            txtAddress2.Text = item.Address2;
            txtAddress3.Text = item.Address3;
            txtCity.Text = item.City;
            txtCountry.Text = item.Country;
            txtPostalCode.Text = item.PostalCode;
            ddlLocationGroup.SelectedValue = item.InventoryLocationGroupID + "";
            ddlPriceLevel.SelectedValue = item.DefaultPriceLevel + "";

            ctrlCreatedBy.Text = item.CreatedBy;
            if (item.CreatedOn.HasValue)
                ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
            ctrlModifiedBy.Text = item.ModifiedBy;
            if (item.ModifiedOn.HasValue)
                ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
            ctrlDeleted.Checked = item.Deleted.GetValueOrDefault(false);
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
    }


    /// <summary>
    /// Shows/Hides the Grid and Editor panels
    /// </summary>
    /// <param name="showIt"></param>
    void ToggleEditor(bool showIt)
    {
        pnlEdit.Visible = showIt;
        pnlGrid.Visible = !showIt;

        if (showIt)
        {
             trPriceLevel.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPriceLevelForWebOrder), false);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        LoadEditor("0");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        InventoryLocation.Delete(Utility.GetParameter("id"));
        AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE InventoryLocation : {0}", Utility.GetParameter("id")), "");
        //redirect
        Response.Redirect(Request.CurrentExecutionFilePath);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string id = hdfID.Value;
        //bool haveError = false;
        try
        {
            BindAndSave(id);
            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Inventory Location Saved.") + "</span>";
        }
        catch (Exception x)
        {
            //haveError = true;
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Inventory Location not saved:") + "</span> " + x.Message;
        }
    }

    /// <summary>
    /// Binds and saves the data
    /// </summary>
    /// <param name="id"></param>
    void BindAndSave(string id)
    {
        var invGroupID = (ddlLocationGroup.SelectedValue + "").GetIntValue();

        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            Query qr = InventoryLocation.CreateQuery();
            qr.QueryType = QueryType.Update;
            qr.AddWhere(InventoryLocation.Columns.InventoryLocationID, hdfID.Value);
            qr.AddUpdateSetting(InventoryLocation.Columns.InventoryLocationName, txtLocationName.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.DisplayedName, txtDisplayedName.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.Address1, txtAddress1.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.Address2, txtAddress2.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.Address3, txtAddress3.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.City, txtCity.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.Country, txtCountry.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.PostalCode, txtPostalCode.Text);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.InventoryLocationGroupID, invGroupID);
            qr.AddUpdateSetting(InventoryLocation.UserColumns.DefaultPriceLevel, ddlPriceLevel.SelectedValue);
            qr.AddUpdateSetting(InventoryLocation.Columns.ModifiedBy, Session["Username"].ToString());
            qr.AddUpdateSetting(InventoryLocation.Columns.ModifiedOn, DateTime.Now);
            qr.AddUpdateSetting(InventoryLocation.Columns.Deleted, ctrlDeleted.Checked);
            qr.Execute();
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE InventoryLocation : {0}", Utility.GetParameter("id")), "");
        }
        else
        {

            InventoryLocation ct = new InventoryLocation();
            ct.IsNew = true;
            ct.InventoryLocationName = txtLocationName.Text;
            ct.DisplayedName = txtDisplayedName.Text;
            ct.Address1 = txtAddress1.Text;
            ct.Address2 = txtAddress2.Text;
            ct.Address3 = txtAddress3.Text;
            ct.City = txtCity.Text;
            ct.Country = txtCountry.Text;
            ct.PostalCode = txtPostalCode.Text;
            ct.InventoryLocationGroupID = invGroupID;
            ct.DefaultPriceLevel = ddlPriceLevel.SelectedValue;
            ct.Deleted = false;
            ct.Save(Session["username"].ToString());
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD InventoryLocation : {0}", txtID.Text), "");
        }
    }

    /// <summary>
    /// Binds the GridView
    /// </summary>
    private void BindGrid(string orderBy)
    {
        DataTable dt = new InventoryLocationController().FetchAll().ToDataTable();
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid(String.Empty);
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


    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = GridView1.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        GridView1.PageIndex = ddlPages.SelectedIndex;
        // a method to populate your grid
        BindGrid(String.Empty);
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

    protected void ddlLocationGroup_Init(object sender, EventArgs e)
    {
        try
        {
            var ddl = (DropDownList)sender;
            var invGroups = new InventoryLocationGroupController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.InventoryLocationGroupName).ToList();
            invGroups.Insert(0, new InventoryLocationGroup
            {
                InventoryLocationGroupID = 0,
                InventoryLocationGroupName = "-"
            });
            ddl.DataSource = invGroups;
            ddl.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    protected void ddlPriceLevel_Init(object sender, EventArgs e)
    {
        try
        {
            var ddl = (DropDownList)sender;
            List<object> levelList = new List<object>();

            bool DisplayPrice1 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), false);
            bool DisplayPrice2 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), false);
            bool DisplayPrice3 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), false);
            bool DisplayPrice4 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), false);
            bool DisplayPrice5 = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), false);
            string P1Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
            string P2Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
            string P3Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
            string P4Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
            string P5Name = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);

            levelList.Add(new { Key = "", Value = "" });

            if (DisplayPrice1 && !string.IsNullOrEmpty(P1Name))
                levelList.Add(new { Key = "P1", Value = P1Name });

            if (DisplayPrice2 && !string.IsNullOrEmpty(P2Name))
                levelList.Add(new { Key = "P2", Value = P2Name });

            if (DisplayPrice3 && !string.IsNullOrEmpty(P3Name))
                levelList.Add(new { Key = "P3", Value = P3Name });

            if (DisplayPrice4 && !string.IsNullOrEmpty(P4Name))
                levelList.Add(new { Key = "P4", Value = P4Name });

            if (DisplayPrice5 && !string.IsNullOrEmpty(P5Name))
                levelList.Add(new { Key = "P5", Value = P5Name });
            
            ddl.DataSource = levelList;
            ddl.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }
}

