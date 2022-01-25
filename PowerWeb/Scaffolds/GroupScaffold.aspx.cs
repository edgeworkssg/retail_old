
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

public partial class GroupScaffold : PageBase
	{
		private bool isAdd = false;
		private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
        string id = string.Empty;
    
		protected void Page_Load(object sender, EventArgs e) 
		{
			if (Request.QueryString["id"] != null)
			{
				id= Utility.GetParameter("id");
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
				if(!Page.IsPostBack)
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
            rowOutlet.Visible = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseUserGroupOutletAssignment), false));
			ToggleEditor(true);
			LoadDrops();
			if (!String.IsNullOrEmpty(id) && id != "0")
			{
				lblID.Text = id.ToString();
	            
				//pull the record
                UserGroup userGroup = new UserGroup(id);

                txtGroupName.Text = userGroup.GroupName;
                ctrlGroupDescription.Text = userGroup.GroupDescription;
                ctrlDiscountLimit.Text = userGroup.Userfloat1.ToString();

                ddlPriceResctriction.SelectedValue = userGroup.PriceRestrictedTo;

                if (userGroup.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = userGroup.CreatedOn.Value.ToString();
                if (userGroup.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = userGroup.ModifiedOn.Value.ToString();
                if (userGroup.Deleted.HasValue)
                    ctrlDeleted.Checked = userGroup.Deleted.Value;

                ctrlCreatedBy.Text = userGroup.CreatedBy;
                ctrlModifiedBy.Text = userGroup.ModifiedBy;

                try
                {
                    string[] selOutlet = userGroup.AssignedOutletList;
                    for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
                    {
                        ddlMultiOutlet.Items[i].Selected = selOutlet.Contains(ddlMultiOutlet.Items[i].Value);
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
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
            var qr = new Query("Outlet");
            qr.AddWhere(Outlet.Columns.Deleted, Comparison.Equals, 0);
            var ouList = new OutletController()
                        .FetchByQuery(qr)
                        .OrderByDescending(o => o.OutletName)
                        .ToList();

            ddlMultiOutlet.Items.Clear();
            foreach (var ou in ouList)
            {
                ddlMultiOutlet.Items.Add(new ListItem { Value = ou.OutletName, Text = ou.OutletName });
            }


            ddlPriceResctriction.Items.Clear();
            ddlPriceResctriction.Items.AddRange(UserGroupController.GetPriceOverrideRestrictionList().ToArray());
            ddlPriceResctriction.DataBind();

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
            UserGroup.Delete(Utility.GetParameter("id"));
			//redirect
			Response.Redirect(Request.CurrentExecutionFilePath);
		}

		protected void btnSave_Click(object sender, EventArgs e) 
		{
			//bool haveError = false;
			try 
			{
				BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("User Group saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
                 lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("User Group not saved:") + "</span> " + x.Message;
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

            var selOutlet = new List<string>();
            for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
            {
                if (ddlMultiOutlet.Items[i].Selected)
                    selOutlet.Add(ddlMultiOutlet.Items[i].Value);
            }
            if (selOutlet.Count == ddlMultiOutlet.Items.Count)
            {
                selOutlet.Clear();
                selOutlet.Add("ALL");
            }

            if (ctrlDiscountLimit.Text == "") ctrlDiscountLimit.Text = "0";
            
			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
                Query qr = UserGroup.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(UserGroup.Columns.GroupID, id);
                qr.AddUpdateSetting(UserGroup.Columns.GroupName, txtGroupName.Text);
                qr.AddUpdateSetting(UserGroup.Columns.GroupDescription, ctrlGroupDescription.Text);
                qr.AddUpdateSetting(UserGroup.Columns.Userfloat1, ctrlDiscountLimit.Text);
                qr.AddUpdateSetting(UserGroup.Columns.Userfld1, string.Join(",", selOutlet.ToArray()));
                qr.AddUpdateSetting(UserGroup.UserColumns.PriceRestrictedTo, ddlPriceResctriction.SelectedItem.Value);
                qr.AddUpdateSetting(UserGroup.Columns.ModifiedBy, Session["username"].ToString());
                qr.AddUpdateSetting(UserGroup.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(UserGroup.Columns.Deleted, ctrlDeleted.Checked);

                qr.Execute();                
			}
			else 
			{

                UserGroup mg = new UserGroup();

                mg.IsNew = true;
                mg.GroupName = txtGroupName.Text;
                mg.GroupDescription = ctrlGroupDescription.Text;
                mg.Userfloat1 = Decimal.Parse(ctrlDiscountLimit.Text);
                mg.PriceRestrictedTo = ddlPriceResctriction.SelectedItem.Value;
                mg.CreatedBy = Session["username"].ToString();
                mg.CreatedOn = DateTime.Now;
                mg.ModifiedBy = Session["username"].ToString();
                mg.ModifiedOn = DateTime.Now;
                mg.Deleted = false;
                mg.AssignedOutletList = selOutlet.ToArray();
                mg.Save(Session["username"].ToString());
			}
            //bind it
			
		}

		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            DataTable dt;
            if (orderBy == string.Empty)
                orderBy = "GroupName";

            UserGroupCollection group = new UserGroupCollection();

            if (ViewState[SORT_DIRECTION] == SqlFragment.ASC)
            {
                dt = group.OrderByAsc(orderBy).Load().ToDataTable();
            }
            else
            {
                dt = group.OrderByDesc(orderBy).Load().ToDataTable();
            }
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[5].Visible = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseUserGroupOutletAssignment), false));
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

	}



