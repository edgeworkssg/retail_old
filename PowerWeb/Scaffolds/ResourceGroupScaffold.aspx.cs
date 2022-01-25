﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SubSonic.Utilities;
using PowerPOS;
using System.Data;
using SubSonic;

namespace PowerWeb.Scaffolds
{
    public partial class ResourceGroupScaffold : PageBase
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
				ResourceGroup item = new ResourceGroup(id);
				//bind the page 
                
				ctrlResourceGroupName.Text = item.GroupName;
						
				if(item.CreatedOn.HasValue)
				{
				    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
				}
			
				if(item.ModifiedOn.HasValue)
				{
					ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
				}
			
				ctrlCreatedBy.Text = item.CreatedBy;
				
				ctrlModifiedBy.Text = item.ModifiedBy;
				
				if(item.Deleted.HasValue)
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
			ResourceGroup.Delete(Utility.GetParameter("id"));
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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Resource Group saved.") + "</span>";
			}
			 catch (Exception x) 
			 {
                 if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
                 {
                     lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Resource Group ID:") + " " + lblID.Text + " " + LanguageManager.GetTranslation("has already been used. Please use another name.") + "</span> ";
                 }
                 else
                 {
                     //haveError = true;
                     lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Resource Group not saved:") + "</span> " + x.Message;
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
	        
			ResourceGroup item;
			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
				//it's an edit
				item = new ResourceGroup(id);
                item.IsNew = false;
			}
			else 
			{
				//add
				item = new ResourceGroup();
                item.IsNew = true;
			}  
	      
			
			object valctrlResourceGroupName = Utility.GetDefaultControlValue(ResourceGroup.Schema.GetColumn("GroupName"), ctrlResourceGroupName, isAdd, false);
			
			item.GroupName = Convert.ToString(valctrlResourceGroupName);
			
			object valctrlDeleted = Utility.GetDefaultControlValue(ResourceGroup.Schema.GetColumn("Deleted"), ctrlDeleted, isAdd, false);
			
			if(valctrlDeleted == null)
			{
				item.Deleted = null;
			}
			else
			{
			    item.Deleted = Convert.ToBoolean(valctrlDeleted);
			}
						    
			//bind it
            item.Save(Session["username"].ToString());
		}
		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            ResourceGroupCollection id = new ResourceGroupCollection();
            id.Where(ResourceGroup.Columns.Deleted, false);
            id.Load();
            GridView1.DataSource = id;
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
