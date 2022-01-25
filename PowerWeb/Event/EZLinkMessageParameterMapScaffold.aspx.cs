
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

namespace PowerPOS
{
	public partial class EZLinkMessageParameterMapScaffold : System.Web.UI.Page 
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
			ToggleEditor(true);
			LoadDrops();
			if (!String.IsNullOrEmpty(id) && id != "0")
			{
				lblID.Text = id.ToString();
	            
				//pull the record
				EZLinkMessageParameterMap item = new EZLinkMessageParameterMap(id);
				//bind the page 
					
							ctrlMsgName.Text = item.MsgName;
						
							ctrlParameterName.Text = item.ParameterName;
						
							ctrlStartingIndex.Text = item.StartingIndex.ToString();
						
							ctrlDeleted.Checked = item.Deleted;
						
						if(item.CreatedOn.HasValue)
						{
						
							ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
						
						}

					
							ctrlCreatedBy.Text = item.CreatedBy;
						
						if(item.ModifiedOn.HasValue)
						{
						
							ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
						
						}

					
							ctrlModifiedBy.Text = item.ModifiedBy;
						
							ctrlUniqueID.Text = item.UniqueID.ToString();
									
  
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
			EZLinkMessageParameterMap.Delete(Utility.GetParameter("id"));
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
				lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer saved.</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
				lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">Customer not saved:</span> " + x.Message;
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
	        
			EZLinkMessageParameterMap item;
			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
				//it's an edit
				item = new EZLinkMessageParameterMap(id);
			}

			else 
			{
				//add
				item = new EZLinkMessageParameterMap();
			}
  
	      
			
						object valctrlMsgName = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("MsgName"), ctrlMsgName, isAdd, false);
						
							item.MsgName = Convert.ToString(valctrlMsgName);
						
						object valctrlParameterName = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("ParameterName"), ctrlParameterName, isAdd, false);
						
							item.ParameterName = Convert.ToString(valctrlParameterName);
						
						object valctrlStartingIndex = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("startingIndex"), ctrlStartingIndex, isAdd, false);
						
							item.StartingIndex = Convert.ToInt32(valctrlStartingIndex);
						
						object valctrlDeleted = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("Deleted"), ctrlDeleted, isAdd, false);
						
							item.Deleted = Convert.ToBoolean(valctrlDeleted);
						
						object valctrlCreatedOn = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("CreatedOn"), ctrlCreatedOn, isAdd, false);
						
						if(valctrlCreatedOn == null)
						{
							item.CreatedOn = null;
						}

						else
						{
						
							item.CreatedOn = Convert.ToDateTime(valctrlCreatedOn);
						
						}

						
						object valctrlCreatedBy = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("CreatedBy"), ctrlCreatedBy, isAdd, false);
						
						if(valctrlCreatedBy == null)
						{
							item.CreatedBy = null;
						}

						else
						{
						
							item.CreatedBy = Convert.ToString(valctrlCreatedBy);
						
						}

						
						object valctrlModifiedOn = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("ModifiedOn"), ctrlModifiedOn, isAdd, false);
						
						if(valctrlModifiedOn == null)
						{
							item.ModifiedOn = null;
						}

						else
						{
						
							item.ModifiedOn = Convert.ToDateTime(valctrlModifiedOn);
						
						}

						
						object valctrlModifiedBy = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("ModifiedBy"), ctrlModifiedBy, isAdd, false);
						
						if(valctrlModifiedBy == null)
						{
							item.ModifiedBy = null;
						}

						else
						{
						
							item.ModifiedBy = Convert.ToString(valctrlModifiedBy);
						
						}

						
						object valctrlUniqueID = Utility.GetDefaultControlValue(EZLinkMessageParameterMap.Schema.GetColumn("UniqueID"), ctrlUniqueID, isAdd, false);
						
							item.UniqueID = new Guid(valctrlUniqueID.ToString());
						    
			//bind it
			item.Save(User.Identity.Name);
		}

		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
		    TableSchema.Table tblSchema = DataService.GetTableSchema("EZLinkMessageParameterMap", "PowerPOS");
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

