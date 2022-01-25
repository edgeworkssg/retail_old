
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


	public partial class OutletScaffold : PageBase
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
                    if (!Page.IsPostBack)
                    {
                        LoadDrops();
                        if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GuestBook.ShowPrefixMembershipOutlet), false))
                        {
                            trPrefixMembership.Visible = false;
                        }
                    }
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

        private void SetFormSetting()
        {
            try
            {
                bool enableMallIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);

                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
                //this.Page.Title = outletText + " Setup";
                //GridView1.Columns[1].HeaderText = outletText + " Name";
                //GridView1.Columns[2].HeaderText = outletText + " Code";
                //GridView1.Columns[3].HeaderText = outletText + " Group";
                //GridView1.Columns[4].HeaderText = outletText + " Address";
                //lblOutletName.Text = outletText + " Name";
                //lblOutletGroup.Text = outletText + " Group";
                //lblOutletAddress.Text = outletText + " Address";
                //lblMallCode.Text = outletText + " Code";
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
			LoadDrops();
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                hdfID.Value = id.ToString();
                txtOutletName.Text = id.ToString();

                Outlet item = new Outlet(id);

                ctrlOutletAddress.Text = item.OutletAddress;
                if (item.InventoryLocationID.HasValue)
                {
                    ctrlInventoryLocationID.SelectedValue = item.InventoryLocationID.Value.ToString();
                }

                ctrlPhoneNo.Text = item.PhoneNo;
                txtMallCode.Text = item.MallCode;
                txtPrefixMembership.Text = item.PrefixMembership;
                ctrlRemark.Text = item.Remark;

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GuestBook.ShowPrefixMembershipOutlet), false))
                {
                    trPrefixMembership.Visible = false;
                }

                if (item.CreatedOn.HasValue)
                {
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                }

                ctrlCreatedBy.Text = item.CreatedBy;
                if (item.ModifiedOn.HasValue)
                {
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                }
                ctrlModifiedBy.Text = item.ModifiedBy;

                if (item.Deleted.HasValue)
                {
                    ctrlDeleted.Checked = item.Deleted.Value;
                }
                ddlOutletGroup.SelectedValue = item.OutletGroupID.GetValueOrDefault(0).ToString();
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

            Query qryctrlInventoryLocationID = InventoryLocation.CreateQuery();
            qryctrlInventoryLocationID.OrderBy = OrderBy.Asc("InventoryLocationName");
            Utility.LoadDropDown(ctrlInventoryLocationID, qryctrlInventoryLocationID.ExecuteReader(), true);
            ctrlInventoryLocationID.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));

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
			Outlet.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE Outlet : {0}", Utility.GetParameter("id")), "");
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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Outlet saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
                 if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                 {
                     lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Outlet name:") + " " + txtOutletName.Text + " " + LanguageManager.GetTranslation("already exist. Please use another name") + "</span> ";
                 }
                 else
                 {                  
                     lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Outlet not saved:</span> " + x.Message;
                 }
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

            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE Outlet : {0}", Utility.GetParameter("id")), "");
                Query qr = Outlet.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(Outlet.Columns.OutletName, hdfID.Value);
                qr.AddUpdateSetting(Outlet.Columns.OutletName, txtOutletName.Text);
                qr.AddUpdateSetting(Outlet.Columns.InventoryLocationID, ctrlInventoryLocationID.SelectedValue);
                qr.AddUpdateSetting(Outlet.Columns.PhoneNo, ctrlPhoneNo.Text);
                qr.AddUpdateSetting(Outlet.Columns.Userfld1, txtMallCode.Text);
                qr.AddUpdateSetting(Outlet.Columns.Userfld2, txtPrefixMembership.Text);
                qr.AddUpdateSetting(Outlet.Columns.OutletAddress, ctrlOutletAddress.Text);
                qr.AddUpdateSetting(Outlet.Columns.ModifiedBy, Session["UserName"].ToString());
                qr.AddUpdateSetting(Outlet.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(Outlet.Columns.Deleted, ctrlDeleted.Checked);
                qr.AddUpdateSetting(Outlet.Columns.OutletGroupID, (ddlOutletGroup.SelectedValue+"").GetIntValue());
                qr.AddUpdateSetting(Outlet.Columns.Remark, ctrlRemark.Text);
                qr.Execute();
            }
            else
            {
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD Outlet : {0}", txtOutletName.Text), "");
                Outlet.Insert(txtOutletName.Text, ctrlOutletAddress.Text, int.Parse(ctrlInventoryLocationID.SelectedValue.ToString()),
                    ctrlPhoneNo.Text, ctrlRemark.Text, DateTime.Now,
                    Session["UserName"].ToString(), DateTime.Now, Session["UserName"].ToString(),
                    false, txtMallCode.Text, txtPrefixMembership.Text, "",
                    "", "", "", "", "", "", "", null, null, null, null,
                    null, null, null, null, null, null, null, null, null, null, null, (ddlOutletGroup.SelectedValue + "").GetIntValue());

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
                orderBy = "OutletName";
            if (ViewState[SORT_DIRECTION] == null)
                ViewState[SORT_DIRECTION] = "ASC";

            OutletCollection ot = new OutletCollection();
            if (ViewState[SORT_DIRECTION].ToString() == "ASC")
            {
                ot.OrderByAsc(orderBy);
            }
            else
            {
                ot.OrderByDesc(orderBy);
            }
            ot.Load();
            /*
            Query qr = new Query("Outlet");
            
                    DataSet ds = qr.ORDER_BY(orderBy,ViewState[SORT_DIRECTION].ToString()).ExecuteJoinedDataSet();
             */
            dt = ot.ToDataTable();
            int locID;
            dt.Columns.Add("InventoryLocationName");
            dt.Columns.Add("OutletGroupName");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                locID = -1;
                int.TryParse(dt.Rows[i]["InventoryLocationID"].ToString(), out locID);
                if (locID != -1)
                    dt.Rows[i]["InventoryLocationName"] = (new InventoryLocation(locID)).InventoryLocationName;

                locID = -1;
                int.TryParse(dt.Rows[i]["OutletGroupID"].ToString(), out locID);
                if (locID > 0)
                    dt.Rows[i]["OutletGroupName"] = (new OutletGroup(locID)).OutletGroupName;
                else
                    dt.Rows[i]["OutletGroupName"] = "-";
            }

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

        protected void ddlOutletGroup_Init(object sender, EventArgs e)
        {
            OutletGroupCollection ogc = new OutletGroupCollection();
            ogc.Load();
            ogc.Insert(0, new OutletGroup { OutletGroupName = "--Not Specified--", OutletGroupID = 0 });
            DropDownList theDll = (DropDownList)sender;
            theDll.DataSource = ogc;
            theDll.DataBind();
        }
	}



