
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

	public partial class CategoryScaffold : PageBase
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
					    LoadDrops();
					btnDelete.Visible = false;

                    cbIsSaleRestricted.Checked = false;
                    DateTime dt = DateTime.Parse("00:00 AM");
                    MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                    if (dt.ToString("tt") == "AM")
                    {
                        am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    }
                    else
                    {
                        am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    }
                    tsRestrictedStart.SetTime(dt.Hour, dt.Minute, am_pm);
                    tsRestrictedEnd.SetTime(dt.Hour, dt.Minute, am_pm);
				}

                if (!Page.IsPostBack)
                {
                    trFunding.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
                    divPAMed.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
                    divSMF.Visible = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
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
                hdfID.Value = id.ToString();
                txtID.Text = id.ToString();

                //pull the record
                Category item = new Category(id);
                //bind the page 
                //txtCategoryID.Text = item.CategoryId;
                //txtAccountCat.Text = item.AccountCategory;
                //cbIsGST.Checked = item.IsGST;
                //cbIsForSale.Checked = item.IsForSale;
                //cbIsDiscountable.Checked = item.IsDiscountable;

                ddlItemDept.SelectedValue = item.ItemDepartmentId;
                cbIsForSale.Checked = item.IsForSale;
                ctrlRemarks.Text = item.Remarks;
                txtAgeRestrictedItems.Text = item.AgeRestrictedItems == null ? "" : item.AgeRestrictedItems.ToString();
                ctrlCreatedBy.Text = item.CreatedBy;

                if (item.CreatedOn.HasValue)
                {
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                }

                ctrlModifiedBy.Text = item.ModifiedBy;

                if (item.ModifiedOn.HasValue)
                {
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                }

                if (item.Deleted.HasValue)
                {
                    ctrlDeleted.Checked = item.Deleted.Value;
                }

               
                cbIsSaleRestricted.Checked = item.IsSellingRestriction;


                if (item.RestrictedTimeStart != null && item.RestrictedTimeStart != "")
                {
                    DateTime dt = DateTime.Parse(item.RestrictedTimeStart);
                    MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                    if (dt.ToString("tt") == "AM")
                    {
                        am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    }
                    else
                    {
                        am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    }
                    tsRestrictedStart.SetTime(dt.Hour, dt.Minute, am_pm);
                }
                else
                {
                    tsRestrictedStart.SetTime(0, 0, MKB.TimePicker.TimeSelector.AmPmSpec.AM);
                }

                if (item.RestrictedTimeEnd != null && item.RestrictedTimeEnd != "")
                {
                    DateTime dt = DateTime.Parse(item.RestrictedTimeEnd);
                    MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                    if (dt.ToString("tt") == "AM")
                    {
                        am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                    }
                    else
                    {
                        am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                    }
                    tsRestrictedEnd.SetTime(dt.Hour, dt.Minute, am_pm);
                }
                else
                {
                    tsRestrictedEnd.SetTime(0, 0, MKB.TimePicker.TimeSelector.AmPmSpec.AM);
                }


                txtPAMedCap.Text = item.PAMedifundCap.ToString("N2");
                txtSMFCap.Text = item.SMFCap.ToString("N2");

                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");


                // Override GST From Category
                chbOverrideGST.Checked = item.IsOverrideGST;
                txtGSTPercentage.Text = item.GSTPercentage.ToString("N2");

                // Barcode by Prefix
                txtLastBarcodeGenerated.Text = item.LastBarcodeGenerated;
                txtBarcodePrefix.Text = item.BarcodePrefix;
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
			Category.Delete(Utility.GetParameter("id"));
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE Category : {0}", Utility.GetParameter("id")), "");
			//redirect
			Response.Redirect(Request.CurrentExecutionFilePath);
		}

		protected void btnSave_Click(object sender, EventArgs e) 
		{
            string id = hdfID.Value;
			//bool haveError = false;
			try 
			{
				BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Category saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
                 lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Category not saved:") + "</span> " + x.Message;
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
            if (txtID.Text == "")
            {
                throw new Exception(LanguageManager.GetTranslation("Category Name cannot be empty"));
            }

            DateTime restrictstart = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", tsRestrictedStart.Hour, tsRestrictedStart.Minute, tsRestrictedStart.Second, tsRestrictedStart.AmPm));
            DateTime restrictend = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", tsRestrictedEnd.Hour, tsRestrictedEnd.Minute, tsRestrictedEnd.Second, tsRestrictedEnd.AmPm));

            int AgeRestrictedItem = 0;
            if (txtAgeRestrictedItems.Text != "")
            {
                if (!int.TryParse(txtAgeRestrictedItems.Text, out AgeRestrictedItem))
                    AgeRestrictedItem = 0;
            }


			if (!String.IsNullOrEmpty(id) && id != "0") 
			{
                Query qr = Category.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(Category.Columns.CategoryName, hdfID.Value);
                qr.AddUpdateSetting(Category.Columns.CategoryName, txtID.Text);
                qr.AddUpdateSetting(Category.Columns.Remarks, ctrlRemarks.Text);
                qr.AddUpdateSetting(Category.Columns.ItemDepartmentId, ddlItemDept.SelectedValue);
                qr.AddUpdateSetting(Category.Columns.IsForSale, cbIsForSale.Checked);
                qr.AddUpdateSetting(Category.UserColumns.PAMedifundCap, decimal.Parse(txtPAMedCap.Text));
                qr.AddUpdateSetting(Category.UserColumns.SMFCap, decimal.Parse(txtSMFCap.Text));
                qr.AddUpdateSetting(Category.UserColumns.IsSellingRestriction, cbIsSaleRestricted.Checked);
                qr.AddUpdateSetting(Category.UserColumns.RestrictedTimeStart, restrictstart.ToString("hh:mm tt"));
                qr.AddUpdateSetting(Category.UserColumns.RestrictedTimeEnd, restrictend.ToString("hh:mm tt"));
                qr.AddUpdateSetting(Category.UserColumns.AgeRestrictedItems, txtAgeRestrictedItems.Text);
                // Override GST From Category
                qr.AddUpdateSetting(Category.UserColumns.IsOverrideGST, chbOverrideGST.Checked);
                qr.AddUpdateSetting(Category.UserColumns.GSTPercentage, decimal.Parse(txtGSTPercentage.Text));
                // Barcode Prefix
                qr.AddUpdateSetting(Category.UserColumns.BarcodePrefix, txtBarcodePrefix.Text);
                qr.AddUpdateSetting(Category.UserColumns.LastBarcodeGenerated, txtLastBarcodeGenerated.Text);
                //qr.AddUpdateSetting(Category.Columns.AccountCategory, txtAccountCat.Text);
                //qr.AddUpdateSetting(Category.Columns.CategoryId, txtCategoryID.Text);
                //qr.AddUpdateSetting(Category.Columns.IsDiscountable, cbIsDiscountable.Checked);
                //qr.AddUpdateSetting(Category.Columns.IsForSale, cbIsForSale.Checked);
                //qr.AddUpdateSetting(Category.Columns.IsGST, cbIsGST.Checked);
                qr.AddUpdateSetting(Category.Columns.ModifiedBy, Session["Username"].ToString());
                qr.AddUpdateSetting(Category.Columns.ModifiedOn, DateTime.Now);
                qr.AddUpdateSetting(Category.Columns.Deleted, ctrlDeleted.Checked);

                qr.Execute();
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE Category : {0}", Utility.GetParameter("id")), "");
			}
			else 
			{

                Category ct = new Category();
                ct.IsNew = true;
                ct.CategoryName = txtID.Text;
                ct.ItemDepartmentId = ddlItemDept.SelectedValue;
                ct.Remarks = ctrlRemarks.Text;
                ct.IsForSale = cbIsForSale.Checked;
                ct.IsSellingRestriction = cbIsSaleRestricted.Checked;
                ct.RestrictedTimeStart = restrictstart.ToString("hh:mm tt");
                ct.RestrictedTimeEnd = restrictend.ToString("hh:mm tt");
                ct.PAMedifundCap = decimal.Parse(string.IsNullOrEmpty(txtPAMedCap.Text) ? "0" : txtPAMedCap.Text);
                ct.SMFCap = decimal.Parse(string.IsNullOrEmpty(txtSMFCap.Text) ? "0" : txtSMFCap.Text);
                
                ct.AgeRestrictedItems = AgeRestrictedItem;
                // Override GST From Category
                ct.IsOverrideGST = chbOverrideGST.Checked;
                ct.GSTPercentage = decimal.Parse(string.IsNullOrEmpty(txtGSTPercentage.Text) ? "0" : txtGSTPercentage.Text);
                // Barcode Prefix
                ct.BarcodePrefix = txtBarcodePrefix.Text;
                ct.LastBarcodeGenerated = string.IsNullOrEmpty(txtLastBarcodeGenerated.Text) ? "0" : txtLastBarcodeGenerated.Text;
                ct.ModifiedBy = Session["Username"].ToString();
                ct.ModifiedOn = DateTime.Now;
                ct.CreatedBy = Session["Username"].ToString();
                ct.CreatedOn = DateTime.Now;
                ct.Deleted = false;
                ct.IsNew = true;
                ct.Save(Session["username"].ToString());
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD Category : {0}", txtID.Text), "");
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
                orderBy = "CategoryName";

            if (ViewState[SORT_DIRECTION] == SqlFragment.ASC)
            {
                dt = new ViewCategoryCollection().OrderByAsc(orderBy).Load().ToDataTable();
            }
            else
            {
                dt = new ViewCategoryCollection().OrderByDesc(orderBy).Load().ToDataTable();
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

	}



