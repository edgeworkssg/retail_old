
//Generated on 5/6/2007 4:12:16 PM by Albert
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
using PowerPOS.Container;

namespace PowerPOS
{
	public partial class MembershipQuickAdd : System.Web.UI.Page 
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
                    ctrlMembershipNo.Text = MembershipController.getNewMembershipNo("E");
                    if (!Page.IsPostBack)
                    {
                        txtSubscriptionDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                        ctrlExpiryDate.Text =
                            (new DateTime
                            (DateTime.Now.Year + 1, DateTime.Now.Month,
                            DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))).
                            ToString("dd MMM yyyy");
                        ctrlDateOfBirth.Text = DateTime.Today.AddYears(-25).ToString("dd MMM yyyy");
                        if (!Page.IsPostBack)
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
				Membership item = new Membership(id);
				//bind the page 
					
				ctrlMembershipNo.Text = item.MembershipNo;						
				ctrlMembershipGroupId.SelectedValue = item.MembershipGroupId.ToString();
                ctrlStylist.SelectedValue = item.SalesPersonID;   
                if (item.ExpiryDate.HasValue)
                    ctrlExpiryDate.Text = item.ExpiryDate.Value.ToString("dd MMM yyyy");
                if (item.SubscriptionDate.HasValue)
                {
                    txtSubscriptionDate.Text = item.SubscriptionDate.Value.ToString("dd MMM yyyy");
                }
                else
                {
                    txtSubscriptionDate.Text = DateTime.Today.ToString("dd MMM yyyy");
                }

                ctrlNameToAppear.Text = item.NameToAppear;
                ctrlNRIC.Text = item.Nric;
                ctrlLastName.Text = item.LastName;						
				ctrlFirstName.Text = item.FirstName;						
                txtChristianName.Text = item.ChristianName;
                txtChineseName.Text = item.ChineseName;
				ctrlGender.Text = item.Gender;						
				if(item.DateOfBirth.HasValue)
				{						
					ctrlDateOfBirth.Text = item.DateOfBirth.Value.ToString("dd MMM yyyy");						
				}
                ctrlOccupation.Text = item.Occupation;

				
                ctrlStreetName.Text = item.StreetName;                
                ctrlStreetName2.Text = item.StreetName2;
                ctrlZipCode.Text = item.ZipCode;						
				ctrlCity.Text = item.City;
                ctrlCountry.Text = item.Country;               									
                
				ctrlMobile.Text = item.Mobile;						
				ctrlOffice.Text = item.Office;						
				ctrlHome.Text = item.Home;
                ctrlEmail.Text = item.Email;	
				
				ctrlRemarks.Text = item.Remarks;															
    
                //CUSTOMER RELATED INFORMATION
                if (item.IsVitaMix.HasValue)
                {
                    cbIsVitaMix.Checked = item.IsVitaMix.Value;
                }
                if (item.IsJuicePlus.HasValue)
                {
                    cbIsJuicePlus.Checked = item.IsJuicePlus.Value;
                }
                if (item.IsWaterFilter.HasValue)
                {
                    cbIsWaterFilter.Checked = item.IsWaterFilter.Value;
                }
                if (item.IsYoung.HasValue)
                {
                    cbIsYoung.Checked = item.IsYoung.Value;
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
			Query qryctrlMembershipGroupId = MembershipGroup.CreateQuery(); 
			qryctrlMembershipGroupId.OrderBy = OrderBy.Asc("GroupName");
			Utility.LoadDropDown(ctrlMembershipGroupId, qryctrlMembershipGroupId.ExecuteReader(), true);

            Query qryusermst = UserMst.CreateQuery();
            qryusermst.AddWhere(UserMst.Columns.IsASalesPerson, true);
            qryusermst.AddWhere(UserMst.Columns.Deleted, false);
            qryusermst.OrderBy = OrderBy.Asc("GroupName");
            Utility.LoadDropDown(ctrlStylist, qryusermst.ExecuteReader(), true);			

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
			Membership.Delete(Utility.GetParameter("id"));
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
	        
			Membership member;
			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
				//it's an edit
				member = new Membership(id);
			}
			else 
			{
                string duplicate;
                if (MembershipController.IsNRICAlreadyExist(ctrlNRIC.Text, out duplicate))
                {
                    CommonWebUILib.ShowMessage(lblResult, "<b>Error. Duplicate NRIC number with member <a target=_blank href=../Membership/membershipdetail.aspx?id=" + duplicate + ">"  + duplicate + "</a></b>", CommonWebUILib.MessageType.BadNews);
                    return;
                }
				//add
				member = new Membership();
			}
            member.MembershipNo = ctrlMembershipNo.Text;
            object valctrlMembershipGroupId = Utility.GetDefaultControlValue(Membership.Schema.GetColumn("MembershipGroupId"), ctrlMembershipGroupId, isAdd, false);
            member.MembershipGroupId = Convert.ToInt32(valctrlMembershipGroupId);
            member.SalesPersonID = ctrlStylist.SelectedValue;
            DateTime expiryDate;
            if (DateTime.TryParse(ctrlExpiryDate.Text, out expiryDate)) member.ExpiryDate = expiryDate;
            
            DateTime subscriptionDate;
            if (DateTime.TryParse(txtSubscriptionDate.Text, out subscriptionDate)) member.SubscriptionDate = subscriptionDate;
            
            member.Nric = ctrlNRIC.Text;
            member.NameToAppear = ctrlNameToAppear.Text;
            member.FirstName = ctrlFirstName.Text;
            member.LastName = ctrlLastName.Text;
            member.ChineseName = txtChineseName.Text;
            member.ChristianName = txtChristianName.Text;
            member.Gender = ctrlGender.SelectedItem.Text;
            
            DateTime dateOfBirth;
            if (DateTime.TryParse(ctrlDateOfBirth.Text, out dateOfBirth)) member.DateOfBirth = dateOfBirth;
            
            member.Occupation = ctrlOccupation.Text;

            member.StreetName = ctrlStreetName.Text;
            member.StreetName2 = ctrlStreetName2.Text;
            member.ZipCode = ctrlZipCode.Text;
            member.City = ctrlCity.Text;
            member.Country = ctrlCountry.Text;

            member.Mobile = ctrlMobile.Text;
            member.Fax = ctrlOffice.Text;
            member.Home = ctrlHome.Text;
            member.Email = ctrlEmail.Text;

            member.Remarks = ctrlRemarks.Text;

            member.IsVitaMix = cbIsVitaMix.Checked;
            member.IsJuicePlus = cbIsJuicePlus.Checked;
            member.IsWaterFilter = cbIsWaterFilter.Checked;
            member.IsYoung = cbIsYoung.Checked;
            member.Deleted = false;
            //bind it
            if (member.IsNew) member.UniqueID = Guid.NewGuid();
		    member.Save(Session["username"].ToString());

            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">Customer saved.</span>";
		}

		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            ViewMembershipCollection myMember = new ViewMembershipCollection();
            string sortColumn = null;            
            if (!String.IsNullOrEmpty(orderBy))
            {
                ViewState.Add(ORDER_BY, sortColumn);                
                if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
                {
                    myMember.OrderByAsc(orderBy);
                    ViewState[SORT_DIRECTION] = SqlFragment.ASC;
                }

                else
                {
                    myMember.OrderByDesc(orderBy);
                    ViewState[SORT_DIRECTION] = SqlFragment.DESC;
                }
            }
            else if (ViewState[ORDER_BY] != null)
            {
                sortColumn = (string)ViewState[ORDER_BY];
                myMember.OrderByAsc(orderBy);
                ViewState.Add(ORDER_BY, sortColumn);
                if (ViewState[SORT_DIRECTION] == null || ((string)ViewState[SORT_DIRECTION]) == SqlFragment.ASC)
                {
                    myMember.OrderByAsc(sortColumn);
                    ViewState[SORT_DIRECTION] = SqlFragment.ASC;
                }

                else
                {
                    myMember.OrderByDesc(sortColumn);
                    ViewState[SORT_DIRECTION] = SqlFragment.DESC;
                }
            }
            
            myMember.Load();

            GridView1.DataSource = myMember.ToDataTable();
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

        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            BindGrid(String.Empty);
            DataTable dt = (DataTable)GridView1.DataSource;
            
            //Massage DataTable            
            CommonWebUILib.ExportCSVWithInvinsibleFields(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
            
        }
        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime bday, expirydate;
                
                if (DateTime.TryParse(e.Row.Cells[7].Text, out bday))
                {
                    e.Row.Cells[7].Text = bday.ToString("dd MMM yyyy");
                }                
            }           
        }
}
}

