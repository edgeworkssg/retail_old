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
using SubSonic.Utilities;
using PowerPOS;
using SubSonic;

namespace PowerWeb.Scaffolds
{
    public partial class ItemAttributesScaffold : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["msg"] != null)
                {
                    string msg = Utility.GetParameter("msg"); ;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
                }

                if (!Page.IsPostBack)
                {
                    LoadAttributes();
                }
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
                    LoadDrops();
                    btnDelete.Visible = false;
                }
            }
            else
            {
                ToggleEditor(false);
                if (!Page.IsPostBack)
                {
                    BindGrid();
                }
            }
        }

        private void LoadAttributes()
        {
            AttributesLabelCollection ac = new AttributesLabelCollection();
            ac.Load();

            ddlType.Items.Clear();

            for (int i = 0; i < ac.Count(); i++)
            {
                ListItem it = new ListItem() { Text = ac[i].Label, Value = "Attributes" + ac[i].AttributesNo.ToString() };
                ddlType.Items.Add(it);
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
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                lblID.Text = id.ToString();
                //pull the record
                ItemAttribute item = new ItemAttribute(id);
                //bind the page 

                ddlType.SelectedValue = item.Type;

                ctrlValue.Text = item.ValueX;

                if (item.CreatedOn.HasValue)
                {
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                }

                if (item.ModifiedOn.HasValue)
                {
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                }

                ctrlCreatedBy.Text = item.CreatedBy;

                ctrlModifiedBy.Text = item.ModifiedBy;

                if (item.Deleted.HasValue)
                {
                     ctrlDeleted.Checked = item.Deleted.Value;
                }

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
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            LoadEditor("0");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ItemAttribute item = new ItemAttribute(Utility.GetParameter("id"));

            Query qry = new Query("Item");
            qry.AddWhere(Item.Columns.Userflag1, true);
            if (item.Type.ToLower() == "attributes3")
            {
                qry.AddWhere(Item.Columns.Attributes3, item.ValueX);
            }
            else if (item.Type.ToLower() == "attributes4")
            {
                qry.AddWhere(Item.Columns.Attributes4, item.ValueX);
            }
            qry.AddUpdateSetting(Item.Columns.Deleted, true);
            qry.Execute();

            ItemAttribute.Delete(Utility.GetParameter("id"));

            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                string idreturn = "";
                BindAndSave(id, out idreturn);
                //lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Item Attributes saved.") + "</span>";
                Response.Redirect("ItemAttributesScaffold.aspx?id=" + idreturn + "&msg=Item Attributes saved.");
            }
            catch (Exception x)
            {
                if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" +  LanguageManager.GetTranslation("Item Attributes ID:") + " " + lblID.Text + " "+ LanguageManager.GetTranslation("has already been used. Please use another name.") + "</span> ";
                }
                else if (x.Message.Contains("Item Attributes already exist"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Attributes already exist. Please use another value.") + "</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Item Attributes not saved:") + "</span> " + x.Message;
                }
            }
            //if(!haveError)
            //  Response.Redirect(Request.CurrentExecutionFilePath);
        }

        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id, out string idreturn)
        {
            idreturn = "0";
            ItemAttributeController itemLogic = new ItemAttributeController();
            ItemAttribute item;

            Query q = new Query("ItemAttributes");
            q.AddWhere(ItemAttribute.Columns.Type, ddlType.SelectedValue);
            q.AddWhere(ItemAttribute.Columns.ValueX, ctrlValue.Text);
            ItemAttributeCollection it = itemLogic.FetchByQuery(q);

            if (it.Count() > 0)
            {
                throw (new Exception("Item Attributes already exist"));
            }

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                //it's an edit
                item = new ItemAttribute(id);
                item.IsNew = false;

                var oldValue = item.ValueX;
                var newValue = ctrlValue.Text;

                //update item
                if (oldValue.Trim() != newValue.Trim()) 
                {
                    Query qry = new Query("Item");
                    qry.AddWhere(Item.Columns.Userflag1, true);
                    if (item.Type.ToLower() == "attributes3")
                    {
                        qry.AddUpdateSetting(Item.Columns.Attributes3, newValue);
                        qry.AddWhere(Item.Columns.Attributes3, oldValue);
                    }
                    else if (item.Type.ToLower() == "attributes4")
                    {
                        qry.AddUpdateSetting(Item.Columns.Attributes4, newValue);
                        qry.AddWhere(Item.Columns.Attributes4, oldValue);
                    }
                    qry.Execute();
                }
            }
            else
            {
                //add
                item = new ItemAttribute();
                item.IsNew = true;
            }

            item.Type = ddlType.SelectedValue;

            item.ValueX = ctrlValue.Text;

           
            object valctrlDeleted = Utility.GetDefaultControlValue(ItemAttribute.Schema.GetColumn("Deleted"), ctrlDeleted, isAdd, false);

            if (valctrlDeleted == null)
            {
                item.Deleted = null;
            }
            else
            {
                item.Deleted = Convert.ToBoolean(valctrlDeleted);
            }

            //bind it
            item.Save(User.Identity.Name);
            idreturn = item.Id.ToString();
        }
        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid()
        {   
            string SQL = "Select * from ItemAttributes where ISNULL(Deleted,0) = 0 {0} order by type ";
            string search = "";
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                search = "and ISNULL(Type,'') + ' ' + ISNULL(Value,'') like '%" + txtSearch.Text + "%'";
            }
            QueryCommand cmd = new QueryCommand(string.Format(SQL,search), "PowerPOS");
            DataTable dt = new DataTable();

            dt.Load(DataService.GetReader(cmd));
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnSearch_Click(Object sender, EventArgs e) 
        {
            BindGrid();
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
            BindGrid();
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

    }
}
