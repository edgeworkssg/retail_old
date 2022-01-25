using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;

namespace PowerWeb.Scaffolds
{
    public partial class MallLayoutSetup : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            SetFormSetting();
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
                    #region *) Privileges
                    try
                    {
                        UserMst um = new UserMst((Session["UserName"] + ""));
                        if (!um.IsHavePrivilegesFor("Add Mall Layout"))
                            Response.Redirect(Request.CurrentExecutionFilePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                    #endregion

                    //it's an add, show the editor
                    isAdd = true;
                    ToggleEditor(true);
                    btnDelete.Visible = false;
                    ddlOutletName.Enabled = true;
                    ddlShopLevel.Enabled = true;
                    txtShopLevel.Enabled = true;
                    txtShopNo.Enabled = true;
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

        private void SetFormSetting()
        {
            try
            {
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                GridView1.Columns[1].HeaderText = outletText;
                lblOutletName.Text = outletText;
                lblOutletFilter.Text = outletText;
                string attribute1 = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute1) + "";
                string attribute2 = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute2) + "";
                trAttribute1.Visible = !string.IsNullOrEmpty(attribute1);
                trAttribute2.Visible = !string.IsNullOrEmpty(attribute2);
                lblAttribute1.Text = attribute1;
                lblAttribute2.Text = attribute2;
                GridView1.Columns[5].Visible = !string.IsNullOrEmpty(attribute1);
                GridView1.Columns[6].Visible = !string.IsNullOrEmpty(attribute2);
                GridView1.Columns[5].HeaderText = attribute1;
                GridView1.Columns[6].HeaderText = attribute2;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        /// <summary>
        /// Loads the editor with data
        /// </summary>
        /// <param name="id"></param>
        void LoadEditor(string id)
        {
            ToggleEditor(true);
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                //pull the record
                PowerPOS.RetailerLevel item = new PowerPOS.RetailerLevel(id);
                hdfID.Value = id.ToString();
                txtRetailerLevelID.Text = id;
                ddlOutletName_Init(ddlOutletName, new EventArgs());
                ddlOutletName.SelectedValue = item.MallName;
                ddlOutletName_SelectedIndexChanged(ddlOutletName, new EventArgs());
                ddlShopLevel.SelectedValue = item.ShopLevel;
                txtShopLevel.Text = item.ShopLevel;
                txtShopNo.Text = item.ShopNo;
                txtShopArea.Text = item.ShopArea;
                txtAttribute1.Text = item.Attribute1;
                txtAttribute2.Text = item.Attribute2;

                if (item.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                ctrlCreatedBy.Text = item.CreatedBy;
                if (item.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                ctrlModifiedBy.Text = item.ModifiedBy;
                ctrlDeleted.Checked = item.Deleted.GetValueOrDefault(false);
                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
                ddlOutletName.Enabled = false;
                ddlShopLevel.Enabled = false;
                txtShopLevel.Enabled = false;
                txtShopNo.Enabled = false;
            }
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LoadEditor("0");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            PowerPOS.RetailerLevel.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE RetailerLevel : {0}", Utility.GetParameter("id")), "");
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Retailer Level saved.") + "</span>";
            }

            catch (Exception x)
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Retailer Level not saved:") + "</span> " + x.Message;
                Logger.writeLog(x);
            }
        }

        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id)
        {
            RetailerLevel item = new RetailerLevel(id);

            #region *) Validation
            var query = new Query("RetailerLevel");
            query.AddWhere(RetailerLevel.Columns.Userfld1, Comparison.Equals, ddlOutletName.SelectedValue);
            query.AddWhere(RetailerLevel.Columns.ShopLevel, Comparison.Equals, txtShopLevel.Text);
            query.AddWhere(RetailerLevel.Columns.ShopNo, Comparison.Equals, txtShopNo.Text);

            var existingData = new RetailerLevelController().FetchByQuery(query).FirstOrDefault();
            if (existingData != null && item.RetailerLevelID != existingData.RetailerLevelID)
                throw new Exception(string.Format("{0} {1} {2} {3} {4} "
                    , LanguageManager.GetTranslation("Retailer level at")
                    , txtShopNo.Text
                    , LanguageManager.GetTranslation("in")
                    , txtShopLevel.Text
                    , ddlOutletName.SelectedValue
                    , LanguageManager.GetTranslation("already exist")));

            #endregion

            item.MallName = ddlOutletName.SelectedValue + "";
            item.ShopLevel = txtShopLevel.Text;
            item.ShopNo = txtShopNo.Text;
            item.ShopArea = txtShopArea.Text;
            item.Attribute1 = txtAttribute1.Text;
            item.Attribute2 = txtAttribute2.Text;
            item.Deleted = ctrlDeleted.Checked;
            item.Remark = "";
            item.Save(Session["UserName"] + "");
            try
            {
                ddlOutletName_SelectedIndexChanged(ddlOutletName, new EventArgs());
                ddlShopLevel.SelectedValue = txtShopLevel.Text;
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("SAVE RetailerLevel : {0}", item.RetailerLevelID), "");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string sortBy)
        {
            string sortCriteria = "";
            string sortDir = (ViewState[SORT_DIRECTION] + "");
            if (string.IsNullOrEmpty(sortDir))
                sortDir = "ASC";
            if (!string.IsNullOrEmpty(sortBy))
                sortCriteria = string.Format("{0} {1}", sortBy, sortDir);
            else
                sortCriteria = "OutletName, ShopLevel, ShopNo";

            string sql = @"SELECT   RL.RetailerLevelID
		                            ,ISNULL(RL.userfld1,'') OutletName
		                            ,RL.ShopLevel
		                            ,RL.ShopNo
		                            ,ISNULL(RL.userfld2,'') ShopArea
		                            ,ISNULL(RL.userfld3,'') Attribute1
		                            ,ISNULL(RL.userfld4,'') Attribute2 
		                            ,ISNULL(RL.Deleted,0) Deleted
                            FROM	RetailerLevel RL
                            WHERE	(ISNULL(RL.Deleted,0) = 0 OR {0} = 1)
		                            AND (ISNULL(RL.userfld1,'') = N'{1}' OR N'{1}' = N'ALL' OR N'{1}' = N'')
		                            AND (ISNULL(RL.ShopLevel,'') = N'{2}' OR N'{2}' = N'ALL' OR N'{2}' = N'')
		                            AND (ISNULL(RL.ShopNo,'') LIKE N'%{3}%')
                            ORDER BY {4}";
            DataTable dt = new DataTable();
            sql = string.Format(sql, chkShowDeletedItems.Checked ? 1 : 0
                                   , ddlFilterOutlet.SelectedValue
                                   , ddlFilterRetailerLevel.SelectedValue
                                   , txtFilterShopNo.Text
                                   , sortCriteria);
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
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
                DataTable ds = GridView1.DataSource as DataTable;
                if (ds != null)
                    itemCount = ds.Rows.Count;

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

        protected void ddlOutletName_Init(object sender, EventArgs e)
        {
            try
            {
                var ddl = (DropDownList)sender;
                var data = OutletController.FetchAll(false, false);
                ddl.DataSource = data;
                ddl.DataBind();
                ddl.SelectedIndex = 0;
                ddlOutletName_SelectedIndexChanged(ddlOutletName, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlOutletName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var data = RetailerLevelController.FetchRetailerLevel(ddlOutletName.SelectedValue + "");
                ddlShopLevel.DataSource = data;
                ddlShopLevel.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnAddNew_Init(object sender, EventArgs e)
        {
            try
            {
                var ctrl = (HtmlInputButton)sender;
                UserMst um = new UserMst((Session["UserName"] + ""));
                ctrl.Visible = !um.IsNew && um.IsHavePrivilegesFor("Add Mall Layout");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid("");
            DataTable dt = ((DataTable)GridView1.DataSource);
            CommonWebUILib.ExportCSV(dt, this.Page.ToString().Trim(' '), "Mall Layout Report", GridView1);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlFilterOutlet.SelectedIndex = 0;
            ddlFilterOutlet_SelectedIndexChanged(ddlFilterOutlet, new EventArgs());
            txtFilterShopNo.Text = "";
            BindGrid("");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid("");
        }

        protected void ddlFilterOutlet_Init(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            var data =
            ddl.DataSource = OutletController.FetchAll(false, true);
            ddl.DataBind();

            ddlFilterOutlet_SelectedIndexChanged(sender, new EventArgs());
        }

        protected void ddlFilterOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = RetailerLevelController.FetchRetailerLevel(ddlFilterOutlet.SelectedValue + "");
            data.Insert(0, "ALL");
            ddlFilterRetailerLevel.DataSource = data;
            ddlFilterRetailerLevel.DataBind();
            BindGrid("");
        }
    }
}
