
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

public partial class MembershipGroupScaffold : PageBase
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
			ToggleEditor(true);
			LoadDrops();
			if (!String.IsNullOrEmpty(id) && id != "0")
			{
				lblID.Text = id.ToString();
	            
				//pull the record
                MembershipGroup membershipGroup = new MembershipGroup(id);

                txtGroupName.Text = membershipGroup.GroupName;
                ctrlDiscount.Text = membershipGroup.Discount.ToString();
                ctrlPointsPercentage.Text = membershipGroup.Userfloat1.ToString();
                ctrlSpendingLimit.Text = membershipGroup.Userfloat2.ToString();
                if (membershipGroup.PriceTier != null && membershipGroup.PriceTier != "")
                {
                    ddlPriceTier.SelectedValue = membershipGroup.PriceTier;
                }
                else
                {
                    ddlPriceTier.SelectedValue = "P0";
                }
                if (membershipGroup.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = membershipGroup.CreatedOn.Value.ToString();
                if (membershipGroup.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = membershipGroup.ModifiedOn.Value.ToString();
                if (membershipGroup.Deleted.HasValue)
                    ctrlDeleted.Checked = membershipGroup.Deleted.Value;

                ctrlCreatedBy.Text = membershipGroup.CreatedBy;
                ctrlModifiedBy.Text = membershipGroup.ModifiedBy;
								
  
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
            DataTable dtDiscount = new DataTable();
            dtDiscount.Columns.Add("DiscountName");
            dtDiscount.Columns.Add("Label");

            ddlPriceTier.Items.Clear();

            DataRow drDiscount1 = dtDiscount.NewRow();
            drDiscount1["DiscountName"] = "No Link";
            drDiscount1["Label"] = "No Link";
            dtDiscount.Rows.Add(drDiscount1);

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), false))
            {
                DataRow drDiscount = dtDiscount.NewRow();
                drDiscount["DiscountName"] = "P1";
                drDiscount["Label"] = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
                dtDiscount.Rows.Add(drDiscount);
            }
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), false))
            {
                DataRow drDiscount = dtDiscount.NewRow();
                drDiscount["DiscountName"] = "P2";
                drDiscount["Label"] = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
                dtDiscount.Rows.Add(drDiscount);
            }
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), false))
            {
                DataRow drDiscount = dtDiscount.NewRow();
                drDiscount["DiscountName"] = "P3";
                drDiscount["Label"] = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
                dtDiscount.Rows.Add(drDiscount);
            }
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), false))
            {
                DataRow drDiscount = dtDiscount.NewRow();
                drDiscount["DiscountName"] = "P4";
                drDiscount["Label"] = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
                dtDiscount.Rows.Add(drDiscount);
            }
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), false))
            {
                DataRow drDiscount = dtDiscount.NewRow();
                drDiscount["DiscountName"] = "P5";
                drDiscount["Label"] = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);
                dtDiscount.Rows.Add(drDiscount);
            }

            ddlPriceTier.DataSource = dtDiscount;
            ddlPriceTier.DataTextField = "Label";
            ddlPriceTier.DataValueField = "DiscountName";
            ddlPriceTier.DataBind();
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
            MembershipGroup.Delete(Utility.GetParameter("id"));
			//redirect
			Response.Redirect(Request.CurrentExecutionFilePath);
		}

		protected void btnSave_Click(object sender, EventArgs e) 
		{
			//bool haveError = false;
			try 
			{
				BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Membership Group saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
                 lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Membership not saved:") + "</span> " + x.Message;
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
			if (ctrlDiscount.Text == "") ctrlDiscount.Text = "0";
            if (ctrlPointsPercentage.Text == "") ctrlPointsPercentage.Text = "0";
            if (ctrlSpendingLimit.Text == "") ctrlSpendingLimit.Text = "0";
            //if (ddlPriceTier.Text == "No Link")  
			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
                Query qr = MembershipGroup.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(MembershipGroup.Columns.MembershipGroupId, id);
                qr.AddUpdateSetting(MembershipGroup.Columns.GroupName, txtGroupName.Text);
                qr.AddUpdateSetting(MembershipGroup.Columns.Discount, ctrlDiscount.Text);
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfloat1, ctrlPointsPercentage.Text);
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfloat2, ctrlSpendingLimit.Text);
                qr.AddUpdateSetting(MembershipGroup.Columns.Userfld1, ddlPriceTier.SelectedValue == "P0" ? "" : ddlPriceTier.SelectedValue);
                qr.AddUpdateSetting(MembershipGroup.Columns.ModifiedBy,Session["username"].ToString());
                qr.AddUpdateSetting(MembershipGroup.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(MembershipGroup.Columns.Deleted, ctrlDeleted.Checked);
                qr.Execute();                
			}
			else 
			{

                MembershipGroup mg = new MembershipGroup();

                mg.IsNew = true;
                mg.GroupName = txtGroupName.Text;
                mg.Discount = Double.Parse(ctrlDiscount.Text);
                mg.Userfloat1 = Decimal.Parse(ctrlPointsPercentage.Text);
                mg.Userfloat2 = Decimal.Parse(ctrlSpendingLimit.Text);
                mg.PriceTier = ddlPriceTier.SelectedValue == "P0" ? "" : ddlPriceTier.SelectedValue;
                mg.CreatedBy = Session["username"].ToString();
                mg.CreatedOn = DateTime.Now;
                mg.ModifiedBy = Session["username"].ToString();
                mg.ModifiedOn = DateTime.Now;
                mg.Deleted = false;
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

            MembershipGroupCollection group = new MembershipGroupCollection();

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



