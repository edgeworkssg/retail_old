
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


namespace PowerWeb.Scaffolds
{
    public partial class OutletGroup : PageBase
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
                    //it's an add, show the editor
                    isAdd = true;
                    ToggleEditor(true);
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

        private void SetFormSetting()
        {
            try
            {
                //string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                //this.Page.Title = outletText + " Group Setup";
                //GridView1.Columns[1].HeaderText = outletText + " Group ID";
                //GridView1.Columns[2].HeaderText = outletText + " Group Name";
                //lblOutletGroupID.Text = outletText + " Group ID";
                //lblOutletGroupName.Text = outletText + " Group Name";
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
                hdfID.Value = id.ToString();
                //pull the record
                PowerPOS.OutletGroup item = new PowerPOS.OutletGroup(id);
                //bind the page 

                //ctrlOutletID.Text = item.outoutlet.ToString();
                txtOutletGroupID.Text = id.ToString();

                ctrlOutletGroupName.Text = item.OutletGroupName;

                if (item.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();

                ctrlCreatedBy.Text = item.CreatedBy;
                if (item.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();

                ctrlModifiedBy.Text = item.ModifiedBy;
                if (item.Deleted.HasValue)
                    ctrlDeleted.Checked = item.Deleted.Value;

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
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
            PowerPOS.OutletGroup.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE OutletGroup : {0}", Utility.GetParameter("id")), "");
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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Outlet Group saved.") + "</span>";
            }

            catch (Exception x)
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Outlet Group not saved:") + "</span> " + x.Message;
                Logger.writeLog(x);
            }
        }

        /// <summary>
        /// Binds and saves the data
        /// </summary>
        /// <param name="id"></param>
        void BindAndSave(string id)
        {

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE OutletGroup : {0}", Utility.GetParameter("id")), "");
                Query qr = PowerPOS.OutletGroup.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(PowerPOS.OutletGroup.Columns.OutletGroupID, txtOutletGroupID.Text);
                qr.AddUpdateSetting(PowerPOS.OutletGroup.Columns.OutletGroupName, ctrlOutletGroupName.Text);
                qr.AddUpdateSetting(PowerPOS.OutletGroup.Columns.ModifiedBy, Session["UserName"].ToString());
                qr.AddUpdateSetting(PowerPOS.OutletGroup.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(PowerPOS.OutletGroup.Columns.Deleted, ctrlDeleted.Checked);
                qr.Execute();
            }
            else
            {
                PowerPOS.OutletGroup ou = new PowerPOS.OutletGroup();
                ou.OutletGroupName = ctrlOutletGroupName.Text;
                ou.Deleted = false;
                ou.Save(Session["UserName"] + "");
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD OutletGroup : {0}", ou.OutletGroupID), "");
            }
        }

        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string orderBy)
        {
            DataTable dt;
            if (orderBy == string.Empty)
                orderBy = "OutletGroupName";
            if (ViewState[SORT_DIRECTION] == null)
                ViewState[SORT_DIRECTION] = "ASC";

            OutletGroupCollection ot = new OutletGroupCollection();
            if (ViewState[SORT_DIRECTION].ToString() == "ASC")
                ot.OrderByAsc(orderBy);
            else
                ot.OrderByDesc(orderBy);
            ot.Load();
            dt = ot.ToDataTable();

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
    }
}
