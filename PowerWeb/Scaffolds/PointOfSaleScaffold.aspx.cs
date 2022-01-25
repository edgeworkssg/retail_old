
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

	public partial class PointOfSaleScaffold : PageBase
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
                    {
                        LoadDrops();
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
                PointOfSale item = new PointOfSale(id);
                //bind the page 

                ctrlPointOfSaleName.Text = item.PointOfSaleName;

                ctrlPointOfSaleDescription.Text = item.PointOfSaleDescription;

                ctrlOutletName.SelectedValue = item.OutletName.ToString();

                ctrlPhoneNo.Text = item.PhoneNo;

                if (item.DepartmentID.HasValue)
                {

                    ctrlDepartmentID.SelectedValue = item.DepartmentID.Value.ToString();

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
                txtMembershipCode.Text = item.MembershipCode;

                txtPriceSchemeID.Text = item.PriceSchemeID;
                if(!string.IsNullOrEmpty(item.LinkedMembershipNo))
                    ctrlMember.SelectedValue = item.LinkedMembershipNo;
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

            ListItem[] li = PointOfSaleController.FetchOutletNamesForDropDown();
            Utility.LoadDropDown(ctrlOutletName, li, "Text", "Value", "Web");

            /*					Query qryctrlOutletName = Outlet.CreateQuery(); 
                                qryctrlOutletName.OrderBy = OrderBy.Asc("OutletID");
                                Utility.LoadDropDown(ctrlOutletName, qryctrlOutletName.ExecuteReader(), true);
            */
            Query qryctrlDepartmentID = Department.CreateQuery();
            qryctrlDepartmentID.OrderBy = OrderBy.Asc("DepartmentName");
            Utility.LoadDropDown(ctrlDepartmentID, qryctrlDepartmentID.ExecuteReader(), true);
            ctrlDepartmentID.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));
            ctrlDepartmentID.SelectedValue = "10001";

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.LinkPOSToMember), false))
            {
                MembershipCollection mcol = new MembershipCollection();
                mcol.Where(PowerPOS.Membership.Columns.Deleted, false);
                mcol.Load();
                DataTable dt = mcol.ToDataTable();
                DataRow dr = dt.NewRow();
                dr["membershipno"] = "0";
                dr["NameToAppear"] = "(Not Specified)";
                dr["membershipgroupid"] = 9;
                dr["uniqueid"] = new Guid();
                dt.Rows.InsertAt(dr, 0);
                ctrlMember.DataSource = dt;

                ctrlMember.DataValueField = PowerPOS.Membership.Columns.MembershipNo;
                ctrlMember.DataTextField = PowerPOS.Membership.Columns.NameToAppear;
                ctrlMember.DataBind();
                rowLinkToMember.Visible = true;
            }
            else
            {
                rowLinkToMember.Visible = false;
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
			PointOfSale.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE PointOfSale : {0}", Utility.GetParameter("id")), "");
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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" +LanguageManager.GetTranslation("Point of Sale saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
                 lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Point of Sale not saved:") + "</span> " + x.Message;
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

            PointOfSale item;
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                //it's an edit
                item = new PointOfSale(id);
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE PointOfSale : {0}", Utility.GetParameter("id")), "");
            }

            else
            {
                //add
                item = new PointOfSale();
            }



            object valctrlPointOfSaleName = Utility.GetDefaultControlValue(PointOfSale.Schema.GetColumn("PointOfSaleName"), ctrlPointOfSaleName, isAdd, false);

            item.PointOfSaleName = Convert.ToString(valctrlPointOfSaleName);

            object valctrlPointOfSaleDescription = Utility.GetDefaultControlValue(PointOfSale.Schema.GetColumn("PointOfSaleDescription"), ctrlPointOfSaleDescription, isAdd, false);

            if (valctrlPointOfSaleDescription == null)
            {
                item.PointOfSaleDescription = null;
            }

            else
            {

                item.PointOfSaleDescription = Convert.ToString(valctrlPointOfSaleDescription);

            }


            object valctrlOutletName = Utility.GetDefaultControlValue(PointOfSale.Schema.GetColumn("OutletName"), ctrlOutletName, isAdd, false);

            item.OutletName = Convert.ToString(valctrlOutletName);

            object valctrlPhoneNo = Utility.GetDefaultControlValue(PointOfSale.Schema.GetColumn("PhoneNo"), ctrlPhoneNo, isAdd, false);

            if (valctrlPhoneNo == null)
            {
                item.PhoneNo = null;
            }

            else
            {

                item.PhoneNo = Convert.ToString(valctrlPhoneNo);

            }


            object valctrlDepartmentID = Utility.GetDefaultControlValue(PointOfSale.Schema.GetColumn("DepartmentId"), ctrlDepartmentID, isAdd, false);

            if (valctrlDepartmentID == null)
            {
                item.DepartmentID = null;
            }

            else
            {

                item.DepartmentID = Convert.ToInt32(valctrlDepartmentID);

            }
            item.MembershipCode = txtMembershipCode.Text;
            item.QuickAccessGroupID = new Guid(ctrlQuickAccessGroup.SelectedValue.ToString());

            item.DepartmentID = 0;

            item.PriceSchemeID = txtPriceSchemeID.Text;
            item.LinkedMembershipNo = ctrlMember.SelectedValue;
            //bind it
            item.Save((string)Session["UserName"]);
            if (String.IsNullOrEmpty(id) || id == "0")
            {
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD PointOfSale : {0}", Utility.GetParameter("id")), "");
            }

        }

		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            PointOfSaleCollection ps = new PointOfSaleCollection();
            GridView1.DataSource = ps.Load();
            GridView1.DataBind();
            /*
		    TableSchema.Table tblSchema = DataService.GetTableSchema("PointOfSale", "PowerPOS");
            if (tblSchema.PrimaryKey != null)
            {
                Query query = new Query(tblSchema);
                string sortColumn = null;
                if (!String.IsNullOrEmpty(orderBy))
                {
                    sortColumn = orderBy;
                }

                else if (ViewState[ORDER_BY] != null)
                {
                    sortColumn = (string)ViewState[ORDER_BY];
                }

                int colIndex = -1;
                if (!String.IsNullOrEmpty(sortColumn))
                {
                    ViewState.Add(ORDER_BY, sortColumn);
                    TableSchema.TableColumn col = tblSchema.GetColumn(sortColumn);
                    if (col == null)
                    {
                        for (int i = 0; i < tblSchema.Columns.Count; i++)
                        {
                            TableSchema.TableColumn fkCol = tblSchema.Columns[i];
                            if (fkCol.IsForeignKey && !String.IsNullOrEmpty(fkCol.ForeignKeyTableName))
                            {
                                TableSchema.Table fkTbl = DataService.GetSchema(fkCol.ForeignKeyTableName, tblSchema.Provider.Name, TableType.Table);
                                if (fkTbl != null)
                                {
                                    col = fkTbl.Columns[1];
                                    colIndex = i;
                                    break;
                                }

                            }

                        }

                    }

                    if (col != null && col.MaxLength < 2048)
                    {
                        if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
                        {
                            if (colIndex > -1)
                            {
                                query.OrderBy = OrderBy.Asc(col, SqlFragment.JOIN_PREFIX + colIndex);
                            }

                            else
                            {
                                query.OrderBy = OrderBy.Asc(col);
                            }

                            ViewState[SORT_DIRECTION] = SqlFragment.ASC;
                        }

                        else
                        {
                            if (colIndex > -1)
                            {
                                query.OrderBy = OrderBy.Desc(col, SqlFragment.JOIN_PREFIX + colIndex);
                            }

                            else
                            {
                                query.OrderBy = OrderBy.Desc(col);
                            }

                            ViewState[SORT_DIRECTION] = SqlFragment.DESC;
                        }

                    }

                }

                DataTable dt = query.ExecuteJoinedDataSet().Tables[0];
                GridView1.DataSource = dt;
                for (int i = 1; i < tblSchema.Columns.Count; i++)
                {
                    BoundField field = (BoundField)GridView1.Columns[i];
                    field.DataField = dt.Columns[i].ColumnName;
                    field.SortExpression = dt.Columns[i].ColumnName;
                    field.HtmlEncode = false;
                    if (tblSchema.Columns[i].IsForeignKey)
                    {
                        TableSchema.Table schema;
                        if (tblSchema.Columns[i].ForeignKeyTableName == null)
                        {
                            schema = DataService.GetForeignKeyTable(tblSchema.Columns[i], tblSchema);
                        }

                        else
                        {
                            schema = DataService.GetSchema(tblSchema.Columns[i].ForeignKeyTableName, tblSchema.Provider.Name, TableType.Table);
                        }

                        if (schema != null)
                        {
                            field.HeaderText = schema.DisplayName;
                        }

                    }

                    else
                    {
                        field.HeaderText = tblSchema.Columns[i].DisplayName;
                    }

                }

                GridView1.DataBind();
            }
            */
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



