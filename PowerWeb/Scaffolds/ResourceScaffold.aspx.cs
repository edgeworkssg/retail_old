using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using SubSonic.Utilities;
using SubSonic;
using System.Data;

namespace PowerWeb.Scaffolds
{
    public partial class ResourceScaffold : PageBase
    {
        private bool isAdd = false;
        private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        private const int colItemNo = 2;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region *) Display: Show Error Message (If Any)
            if (Request.QueryString["msg"] != null)
            {
                string msg = Utility.GetParameter("msg"); ;
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + msg + "</span>";
            }
            #endregion

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
                    if (!Page.IsPostBack)
                    {
                        //it's an add, show the editor
                        isAdd = true;
                        ToggleEditor(true);
                        LoadDrops();
                        txtResourceID.Text = ResourceController.getNewResourceID();
                    }

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
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                txtResourceID.Text = id.ToString();
                txtResourceID.ReadOnly = true;

                //pull the record
                Resource item = new Resource(id);
                //bind the page 

                txtResourceName.Text = item.ResourceName;
                txtRoomCharge.Text = item.RoomCharge.HasValue ? item.RoomCharge.Value.ToString("N2") : "0";
                txtMinSpending.Text = item.MinSpending.HasValue ? item.MinSpending.Value.ToString("N2") : "0";
                txtMinSpendingCharge.Text = item.MinSpendingCharge.HasValue ? item.MinSpendingCharge.Value.ToString("N2") : "0" ;
                txtCapacity.Text = item.Capacity.HasValue ? item.Capacity.Value.ToString("N0") : "0"; 

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
            Query qryusermst = ResourceGroup.CreateQuery();
            qryusermst.AddWhere(ResourceGroup.Columns.Deleted, false);
            qryusermst.OrderBy = OrderBy.Asc("GroupName");
            Utility.LoadDropDown(ddlResourceGroup, qryusermst.ExecuteReader(), true);
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
            Resource.Delete(Utility.GetParameter("id"));
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                BindAndSave(id, false);
            }
            catch (Exception x)
            {
                if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Resource ID:") + " " + txtResourceID.Text + " " + LanguageManager.GetTranslation("has already been used. Please use another name.") + "</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Resource not saved:") + "</span> " + x.Message;
                }
            }
        }

        protected void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            try
            {
                BindAndSave(id, true);
            }
            catch (Exception x)
            {
                if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                {
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Resource ID:") + " " + txtResourceID.Text + " " + LanguageManager.GetTranslation("has already been used. Please use another name.") + "</span> ";
                }
                else
                {
                    //haveError = true;
                    lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Resource not saved:") + "</span> " + x.Message;
                }
            }
        }

        void BindAndSave(string id, bool IsSaveNew)
        {

            Resource item;
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                //it's an edit
                item = new Resource(id);
                item.IsNew = false;
            }
            else
            {
                //add
                item = new Resource();
                item.IsNew = true;
                item.ResourceID = txtResourceID.Text.Trim();
            }

            item.ResourceName = txtResourceName.Text;
            item.ResourceGroupID = Int32.Parse(ddlResourceGroup.SelectedValue);
            item.Capacity = Int32.Parse(txtCapacity.Text);
            item.MinSpending = Decimal.Parse(txtMinSpending.Text.Replace(",", ""));
            item.MinSpendingCharge = Decimal.Parse(txtMinSpendingCharge.Text.Replace(",", ""));
            item.RoomCharge = Decimal.Parse(txtRoomCharge.Text.Replace(",", ""));
            item.Deleted = false;
            
            //bind it
            item.Save(Session["username"].ToString());

            if (IsSaveNew)
            {
                Response.Redirect("ResourceScaffold.aspx?id=0&msg=" + LanguageManager.GetTranslation("Resource with Item No") + " " + item.ResourceID + " " + LanguageManager.GetTranslation("saved"));
            }
            else
            {
                Response.Redirect("ResourceScaffold.aspx?id=" + item.ResourceID + "&msg=" + LanguageManager.GetTranslation("Resource saved"));
            }
        }
        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            string strSearch = string.IsNullOrEmpty(txtSearch.Text) ? "%" : txtSearch.Text ;
           
            GridView1.DataSource = ResourceController.GetResourceData(strSearch);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(string.Empty);
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

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
                    Resource myItem = new Resource(GridView1.Rows[row.RowIndex].Cells[colItemNo].Text);
                    if (myItem.ResourceID != "")
                    {
                        myItem.Deleted = true;
                        commands.Add(myItem.GetUpdateCommand("SYSTEM"));
                        count++;
                    }

                }

            }
            SubSonic.DataService.ExecuteTransaction(commands);
            Session["DeleteMessage"] = String.Format("<span style='color:red; font-weight:bold;'>Deleted {0} Record(s)..</span>", count);
            Response.Redirect("ResourceScaffold.aspx?");
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid(string.Empty);
            DataTable dt = (DataTable)GridView1.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
        }

    }
}
