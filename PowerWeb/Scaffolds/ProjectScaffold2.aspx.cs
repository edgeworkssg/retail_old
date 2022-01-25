
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

	public partial class ProjectScaffold2 : PageBase
	{
		private bool isAdd = false;
		private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";
    
		protected void Page_Load(object sender, EventArgs e)
        {
            CreateProjectTable();

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
                        //LoadDrops();
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
			//LoadDrops();
			if (!String.IsNullOrEmpty(id) && id != "0")
			{
				lblID.Text = id.ToString();
	            
				//pull the record
				Project item = new Project(id);
				//bind the page 
				
						ctrlProjectName.Text = item.ProjectName;
                        txtMembershipCode.Text = item.MembershipNumber;
						
					
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
						
                        //if(item.Deleted.HasValue)
                        //{
						
                        //    ctrlDeleted.Checked = item.Deleted.Value;
						
                        //}
                        //txtMembershipCode.Text  = item.MembershipCode;
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
			Project.Delete(Utility.GetParameter("id"));
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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Project saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
                 lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Project not saved:") + "</span> " + x.Message;
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
	        
			Project item;
            bool editMode = false;
			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
				//it's an edit
                item = new Project(id);
                editMode = true;
			}

			else 
			{
				//add
                item = new Project();
			}

            if (ctrlProjectName.Text == "")
            {
                throw new Exception(LanguageManager.GetTranslation("Project Name is required."));
            }
            if (txtMembershipCode.Text == "")
            {
                throw new Exception(LanguageManager.GetTranslation("Membership No is required."));
            }
            
                      
            MembershipCollection members = new MembershipCollection();
            DataTable dt = members.Where(PowerPOS.Membership.Columns.MembershipNo, txtMembershipCode.Text).Load().ToDataTable();
            if (dt.Rows.Count == 0)
            {
                throw new Exception(LanguageManager.GetTranslation("Membership Number does not exist."));
            }

            if (!editMode)
            {
                ProjectCollection projects = new ProjectCollection();
                dt = projects.Where(Project.Columns.ProjectName, ctrlProjectName.Text).Where(Project.Columns.MembershipNumber, txtMembershipCode.Text).Load().ToDataTable();
                if (dt.Rows.Count > 0)
                {
                    throw new Exception(LanguageManager.GetTranslation("Project already exist in this particular member."));
                }
            }
           

            item.ProjectName = ctrlProjectName.Text;
            item.MembershipNumber = txtMembershipCode.Text;
            item.UniqueID = Guid.NewGuid();
            
            //item
						
					
            //bind it
			item.Save((string)Session["UserName"]);
            
		}

		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            ProjectCollection project = new ProjectCollection();
            GridView1.DataSource = project.Load();
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
        private void CreateProjectTable()
        {
            try
            {
                string SQL = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Project]') AND type in (N'U')) " +
                "BEGIN " +
                "CREATE TABLE [dbo].[Project]( " +
                "[ID] [int] IDENTITY(1,1) NOT NULL, " +
                "[ProjectName] [nvarchar](100) NULL, " +
                "[MembershipNumber] [nvarchar](100) NULL, " +
                "[CreatedOn] [datetime] NULL, " +
                "[CreatedBy] [varchar](50) NULL, " +
                "[ModifiedOn] [datetime] NULL, " +
                "[ModifiedBy] [varchar](50) NULL, " +
                "[UniqueID] [uniqueidentifier] NULL, " +
                "[userfld1] [varchar](50) NULL, " +
                "[userfld2] [varchar](50) NULL, " +
                "[userfld3] [varchar](50) NULL, " +
                "[userfld4] [varchar](50) NULL, " +
                "[userfld5] [varchar](50) NULL, " +
                "[userfld6] [varchar](50) NULL, " +
                "[userfld7] [varchar](50) NULL, " +
                "[userfld8] [varchar](50) NULL, " +
                "[userfld9] [varchar](50) NULL, " +
                "[userflag1] [bit] NULL, " +
                "[userflag2] [bit] NULL, " +
                "[userflag3] [bit] NULL, " +
                "[userflag4] [bit] NULL, " +
                "[userflag5] [bit] NULL, " +
                "[userfloat1] [money] NULL, " +
                "[userfloat2] [money] NULL, " +
                "[userfloat3] [money] NULL, " +
                "[userfloat4] [money] NULL, " +
                "[userfloat5] [money] NULL, " +
                "[userint1] [int] NULL, " +
                "[userint2] [int] NULL, " +
                "[userint3] [int] NULL, " +
                "[userint4] [int] NULL, " +
                "[userint5] [int] NULL, " +
                "CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED  " +
                "( " +
                "[ID] ASC " +
                ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                ") ON [PRIMARY] " +
                "END;";
                SubSonic.DataService.ExecuteQuery(new QueryCommand(SQL));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.ToString());
            }

        }

	}



