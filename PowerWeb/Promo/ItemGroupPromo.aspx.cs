using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PowerPOS;
using SubSonic;
using SubSonic.Utilities;
using System.Drawing;
using PowerPOS.Container;
using System.Text.RegularExpressions;
using SpreadsheetLight;

namespace PowerWeb.Promo
{
    public partial class ItemGroupPromo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");
                #region *) Display: Show Error Message (If Any)
                if (Request.QueryString["msg"] != null)
                {
                    string msg = Utility.GetParameter("msg"); ;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
                }
                #endregion

                if (!String.IsNullOrEmpty(id) && id != "0")
                {
                    if (!IsPostBack)
                    {
                        ToggleEditor(true);
                        //LoadDropdown("");
                        LoadEditor(id);
                    }
                }
                else
                {
                    if (!IsPostBack)
                    {
                        ToggleEditor(true);
                        //LoadDropdown("");
                        BindDetails();

                        btnDelete.Visible = false;
                    }
                }

            }
            else
            {
                if (!IsPostBack)
                {
                    ToggleEditor(false);
                    BindGrid();

                    if (Session["DeleteMessage"] != null && Session["DeleteMessage"].ToString() != "")
                    {
                        lblMsg.Text = Session["DeleteMessage"].ToString();
                        Session["DeleteMessage"] = null;
                    }
                    else
                    {
                        lblMsg.Text = "";
                    }
                }
            }
        }

        #region "Helper"
        private void BindGrid()
        {
            string search = txtSearch.Text ?? "%";
            OutletCollection outletlist = OutletController.FetchByUserNameForReport(false, true, Session["UserName"] + "");
            bool isEligibleForAllOutlet = false;
            foreach (Outlet s in outletlist)
            {
                if (s.OutletName.Contains("ALL"))
                {
                    isEligibleForAllOutlet = true;
                }
            }

            Logger.writeLog("Search Item Group" + search);

            DataTable dt = PromotionAdminController.SearchItemGroup(search, isEligibleForAllOutlet, outletlist);
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private void BindDetails()
        {
            
            
            
            DataTable dt = (DataTable)ViewState["ItemList"];
            if (dt == null)
            {
                dt = new DataTable("Item");
                dt.Columns.Add("ItemNo");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("UnitQty");
                dt.Columns.Add("Price");
                dt.Columns.Add("Deleted");
            }

            gvItem.DataSource = dt;
            gvItem.DataBind();
        }

        private void LoadEditor(string id)
        {
            ItemGroup gr = new ItemGroup(id);
            if (gr != null)
            {
                ItemGroupId.Value = gr.ItemGroupId.ToString();
                txtGroupName.Text = gr.ItemGroupName;
                txtBarcode.Text = gr.Barcode;
            }

            ItemGroupMapCollection col = new ItemGroupMapCollection();
            col.Where(ItemGroupMap.Columns.ItemGroupID, Comparison.Equals, id);
            col.Where(ItemGroupMap.Columns.Deleted, Comparison.Equals, false);
            col.Load();

            if (col.Count > 0)
            {
                DataTable dt = (DataTable)ViewState["ItemList"];
                if (dt == null)
                {
                    dt = new DataTable("Item");
                    dt.Columns.Add("ItemNo");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Deleted");
                }

                foreach (ItemGroupMap i in col)
                {
                    Item it = new Item(i.ItemNo);
                    dt.Rows.Add(i.ItemNo, it.ItemName, it.RetailPrice, false);
                }
                ViewState["ItemList"] = dt;
                BindDetails();
            }
        }

        private void LoadDropdown(string search)
        {
            if (search == string.Empty)
                search = "%";

            //string sql = "select * from Item where (ISNULL(deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap WHERE ItemNo = Item.ItemNo AND ISNULL(Deleted, 0) = 0) ) and ISNULL(ItemNo,'') + ' ' + ISNULL(ItemName,'') + ' ' + ISNULL(Barcode,'') LIKE '%" + search.Replace("'", "''") + "%' order by ItemName";
            string sql = @"select i.* 
                            from Item i
                            left join AlternateBarcode ab on ab.itemNo = i.ItemNo and ISNULL(ab.Deleted,0) = 0
                            where (ISNULL(i.deleted,0) = 0 OR EXISTS(SELECT * FROM OutletGroupItemMap 
                            WHERE ItemNo = i.ItemNo AND ISNULL(Deleted, 0) = 0) ) 
                            and ISNULL(i.ItemNo,'') + ' ' + ISNULL(i.ItemName,'') + ' ' + ISNULL(i.Barcode,'') + ' ' + ISNULL(ab.Barcode,'') LIKE '%{0}%' order by ItemName";
            sql = string.Format(sql, search.Replace("'", "''"));
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql));
            ItemCollection col = new ItemCollection();
            col.Load(ds.Tables[0]);

            ddlItem.DataSource = col;
            ddlItem.DataBind();
        }

        private void ToggleEditor(bool showIt)
        {
            pnlEdit.Visible = showIt;
            pnlIndex.Visible = !showIt;
        }

        private void ExportData(string fileName, DataTable dt)
        {
            fileName = fileName.Replace(" ", "_");
            SLDocument sl = new SLDocument();
            var style = sl.CreateStyle();
            style.FormatCode = "###";
            int iStartRowIndex = 1;
            int iStartColumnIndex = 1;
            sl.SetColumnStyle(5, style);
            sl.ImportDataTable(iStartRowIndex, iStartColumnIndex, dt, true);
            int iEndRowIndex = iStartRowIndex + dt.Rows.Count + 1 - 1;
            int iEndColumnIndex = iStartColumnIndex + dt.Columns.Count - 1;
            SLTable table = sl.CreateTable(iStartRowIndex, iStartColumnIndex, iEndRowIndex, iEndColumnIndex);
            table.SetTableStyle(SLTableStyleTypeValues.Medium2);
            table.HasTotalRow = false;
            sl.InsertTable(table);

            sl.AutoFitColumn(2, iEndColumnIndex);

            sl.FreezePanes(1, 0);

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            //Response.BufferOutput = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "_Result.xlsx");
            sl.SaveAs(Response.OutputStream);
            Response.End();
        }

        protected DataTable GetInitialDataTable()
        {
            DataTable dtInput = new DataTable();
            dtInput.Columns.Add("ITEMNO", typeof(string)); // 0
            dtInput.Columns.Add("ITEMNAME", typeof(string));//1
    
            return dtInput;
        }

        #endregion

        #region "Event Handler"
        protected void BtnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = true;
            }
        }

        protected void BtnClearSelection_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                field.Checked = false;
            }
        }

        protected void BtnDeleteSelection_Click(object sender, EventArgs e)
        {
            int count = 0;
            QueryCommandCollection commands = new QueryCommandCollection();
            foreach (GridViewRow row in GridView1.Rows)
            {
                CheckBox field = (CheckBox)row.FindControl("CheckBox1");
                if (field.Checked)
                {
                    Label groupid = (Label)row.FindControl("lblItemGroupID");
                    ItemGroup myItem = new ItemGroup(Int32.Parse(groupid.Text));
                    if (myItem.ItemGroupId.ToString() != "")
                    {
                        myItem.Deleted = true;
                        commands.Add(myItem.GetUpdateCommand("SYSTEM"));
                        count++;
                    }
                }
            }
            SubSonic.DataService.ExecuteTransaction(commands);
            Session["DeleteMessage"] = String.Format("<span style='color:red; font-weight:bold;'>Deleted {0} Record(s)..</span>", count);
            Response.Redirect("ItemGroupPromo.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
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

        protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
        {
            GridViewRow gvrPager = GridView1.BottomPagerRow;
            DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
            GridView1.PageIndex = ddlPages.SelectedIndex;
            // a method to populate your grid
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSearchItem_Click(object sender, EventArgs e)
        {
            LoadDropdown(txtSearchItem.Text);
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
                using (System.Transactions.TransactionScope transScope =
                new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    ItemGroup at = null;
                    int ItemGroupID = 0;
                    int newItemGroupId = 0;
                    if (!String.IsNullOrEmpty(ItemGroupId.Value))
                    {
                        ItemGroupID = Int32.Parse(ItemGroupId.Value);
                    }

                    if (!string.IsNullOrEmpty(txtBarcode.Text))
                    {
                        if (PromotionAdminController.CheckDuplicateBarcodeItemGroup(txtBarcode.Text.Trim(), ItemGroupID))
                        {
                            throw new Exception(LanguageManager.GetTranslation("Barcode has already been taken up. Please specify a new barcode."));
                        }
                    }


                    /*duplicate promo code*/
                    if (!string.IsNullOrEmpty(txtGroupName.Text))
                    {
                        if (PromotionAdminController.CheckDuplicateItemGroupName(txtGroupName.Text.Trim(), ItemGroupID))
                        {
                            throw new Exception(LanguageManager.GetTranslation("Item Group Name has already been taken up. Please specify a new Item Group Name."));
                        }
                    }
                    /*validate details*/
                    DataTable Items = (DataTable)ViewState["ItemList"];
                    if (Items.Rows.Count == 0)
                    {
                        throw new Exception(LanguageManager.GetTranslation("At least 1 item must be added"));
                    }
                    else
                    {
                        int rowdeleted = 0;
                        foreach (DataRow dr in Items.Rows)
                        {
                            if (dr["Deleted"].ToString().ToLower() == "true")
                                rowdeleted++;
                        }

                        if (rowdeleted == Items.Rows.Count)
                        {
                            throw new Exception(LanguageManager.GetTranslation("At least 1 item must be added"));
                        }
                    }

                    if (ItemGroupId.Value.ToString() != "")
                    {
                        at = new ItemGroup(Int32.Parse(ItemGroupId.Value));
                    }
                    else
                    {
                       at = new ItemGroup();
                    }

                    at.ItemGroupName = txtGroupName.Text;
                    at.Barcode = txtBarcode.Text;
                    at.Deleted = false;
                    if (at.Userint1.HasValue)
                    {
                        at.Userint1 = 1;
                    }
                    else
                        at.Userint1++;
                    at.Save(UserInfo.username);
                    newItemGroupId = at.ItemGroupId;

                    /*save details*/
                    
                    string sql2 = "Update ItemGroupMap set Deleted = 'true' where ItemGroupID = " + newItemGroupId;
                    DataService.ExecuteQuery(new QueryCommand(sql2));

                    ItemGroupMap myMap;
                    for (int i = 0; i < Items.Rows.Count; i++)
                    {
                        if (Items.Rows[i]["Deleted"].ToString().ToLower() != "true")
                        {
                            ItemGroupMapCollection col = new ItemGroupMapCollection();
                            col.Where(ItemGroupMap.Columns.ItemNo, Comparison.Equals, Items.Rows[i]["ItemNo"].ToString());
                            col.Where(ItemGroupMap.Columns.ItemGroupID, Comparison.Equals, newItemGroupId);
                            col.Where(ItemGroupMap.Columns.Deleted, Comparison.Equals, false);
                            col.Load();

                            if (col.Count() == 0)
                            {
                                myMap = new ItemGroupMap();
                            }
                            else 
                            {
                                myMap = col[0];
                            }

                            myMap.ItemNo = Items.Rows[i]["ItemNo"].ToString();
                            myMap.ItemGroupID = at.ItemGroupId;
                            myMap.UnitQty = 1;
                            myMap.Deleted = false;
                            myMap.Save();

                        }
                    }
                    transScope.Complete();
                    Response.Redirect("ItemGroupPromo.aspx?id=" + newItemGroupId.ToString() + "&msg="+LanguageManager.GetTranslation("Product saved"));
                }
            }
            catch (Exception x)
            {
                Logger.writeLog(x);
                if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">"+LanguageManager.GetTranslation("Promo not saved: Group Code:") + txtGroupName.Text + " "+LanguageManager.GetTranslation("has already been used. Choose another name")+"</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">"+LanguageManager.GetTranslation("Product not saved:")+"</span> " + x.Message;
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ItemGroupController ctr = new ItemGroupController();
            ctr.Delete(Utility.GetParameter("id"));
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            DataTable dt;
            if (ViewState["ItemList"] == null)
            {
                dt = new DataTable("Item");
                dt.Columns.Add("ItemNo");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("UnitQty");
                dt.Columns.Add("Price");
                dt.Columns.Add("Deleted");
            }
            else
            {
                dt = (DataTable)ViewState["ItemList"];
                if (dt == null)
                {
                    dt = new DataTable("Item");
                    dt.Columns.Add("ItemNo");
                    dt.Columns.Add("ItemName");
                    dt.Columns.Add("UnitQty");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Deleted");
                }
            }
            if (dt.Select("ItemNo = '" + Regex.Replace(ddlItem.SelectedItem.Value.ToString(), @"\'\b", "''")  + "'").Length == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ItemNo"] = ddlItem.SelectedItem.Value;
                dr["ItemName"] = ddlItem.SelectedItem.Text;
                dr["Price"] = new Item(ddlItem.SelectedItem.Value.ToString()).RetailPrice.ToString("N2");
                dr["Deleted"] = false;
                dt.Rows.Add(dr);
                ViewState["ItemList"] = dt;
                gvItem.DataSource = dt;
                gvItem.DataBind();
            }
        }

        protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ItemList"];

            dt.Rows[e.RowIndex]["Deleted"] = "true";

            ViewState["ItemList"] = dt;
            BindDetails();
        }

        protected void gvItem_RowDataBound(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvItem.Rows)
            {
                Label field = (Label)row.FindControl("lblDeletedGVDetails");

                if (field.Text.ToString().ToLower() == "true")
                {
                    row.Cells[0].Controls[0].Visible = false;
                    row.BackColor = Color.LightPink;
                }
            }
        }

        protected void gvItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItem.EditIndex = -1;
            BindDetails();
        }

        protected void gvItem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvItem.EditIndex = e.NewEditIndex;
            BindDetails();
        }

        protected void gvItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            gvItem.EditIndex = -1;
            GridViewRow row = (GridViewRow)gvItem.Rows[e.RowIndex];
            DataTable dt = (DataTable)ViewState["ItemList"];

            TextBox qty = (TextBox)row.Cells[1].FindControl("txtQtyGV");
            dt.Rows[e.RowIndex]["UnitQty"] = Int32.Parse(qty.Text ?? "0");

            ViewState["ItemList"] = dt;
            BindDetails();
        }

        protected void btnExportBlank_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            DataTable dt = GetInitialDataTable();

            DataTable dt2 = (DataTable)ViewState["ItemList"];
            if (dt2 != null)
            {
                foreach (DataRow i in dt2.Rows)
                {
                    if (i[3].ToString().ToLower() == "false")
                    {
                        dt.Rows.Add(i[0], i[1]);
                    }
                }
            }
            ExportData("ItemGroupPromo", dt);
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            if (fuItemImporter.HasFile)
            {
                DataTable dtInput = GetInitialDataTable();

                #region *) Read Excel

                try
                {
                    using (SLDocument sl = new SLDocument(fuItemImporter.PostedFile.InputStream, "Sheet1"))
                    {
                        SLWorksheetStatistics stats = sl.GetWorksheetStatistics();
                        int iStartColumnIndex = 1;
                        int iStartRowIndex = stats.StartRowIndex + 1;
                        int iEndRowIndex = stats.EndRowIndex;
                        int iEndRowDataIndex = iEndRowIndex;
                        

                        for (int row = iStartRowIndex; row <= iEndRowDataIndex; ++row)
                        {
                            dtInput.Rows.Add(
                                sl.GetCellValueAsString(row, iStartColumnIndex),
                                sl.GetCellValueAsString(row, iStartColumnIndex + 1));
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error " + ex.Message;
                    Logger.writeLog(ex);
                    return;
                }
                #endregion

                #region *) Import Data

                if (dtInput.Rows.Count > 0)
                {
                    DataTable resultDt = new DataTable();
                    

                    try
                    {
                        DataTable dt = (DataTable)ViewState["ItemList"];
                        if (dt == null)
                        {
                            dt = new DataTable("Item");
                            dt.Columns.Add("ItemNo");
                            dt.Columns.Add("ItemName");
                            dt.Columns.Add("Price");
                            dt.Columns.Add("Deleted");
                        }

                        foreach (DataRow i in dtInput.Rows)
                        {
                            string itemno = i[0].ToString();

                            DataRow[] result = dt.Select("ItemNo = '" + itemno + "'and Deleted='false'");

                            if (result.Count() == 0)
                            {

                                Item it = new Item(itemno);
                                if (it != null)
                                    dt.Rows.Add(it.ItemNo, it.ItemName, it.RetailPrice, false);
                            }
                        }

                        ViewState["ItemList"] = dt;
                        BindDetails();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "ERROR : " + ex.Message;
                    }
                }
                else
                {
                    lblStatus.Text = "No Data Imported";
                }

                #endregion
            }
        }

        #endregion

    }
}
