
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

	public partial class Supplier_scaffold : PageBase
	{
		private bool isAdd = false;
		private const string SORT_DIRECTION = "SORT_DIRECTION";
        private const string ORDER_BY = "ORDER_BY";

        private void SetDisplay()
        {
            bool isDisplayCurrencyOnSupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayCurrencyOnSupplier), false);
            bool isDisplayGSTOnSupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayGSTOnSupplier), false);
            bool isDisplayMinimumOrderOnSupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayMinimumOrderOnSupplier), false);
            bool isDisplayDeliveryChargeOnSupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayDeliveryChargeOnSupplier), false);
            bool isDisplayPaymentTermOnSupplier = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayPaymentTermOnSupplier), false);

            DisplayCurrencyOnSupplier.Visible = isDisplayCurrencyOnSupplier;
            DisplayGSTOnSupplier.Visible = isDisplayGSTOnSupplier;
            DisplayMinimumOrderOnSupplier.Visible = isDisplayMinimumOrderOnSupplier;
            DisplayDeliveryChargeOnSupplier.Visible = isDisplayDeliveryChargeOnSupplier;
            DisplayPaymentTermOnSupplier.Visible = isDisplayPaymentTermOnSupplier;

            for (int i = 0; i < GridView1.Columns.Count; i++)
            {
                string header = GridView1.Columns[i].HeaderText;
                if (header == "Currency")
                    GridView1.Columns[i].Visible = isDisplayCurrencyOnSupplier;
                else if (header == "GST")
                    GridView1.Columns[i].Visible = isDisplayGSTOnSupplier;
                else if (header == "Minimum Order")
                    GridView1.Columns[i].Visible = isDisplayMinimumOrderOnSupplier;
                else if (header == "Delivery Charge")
                    GridView1.Columns[i].Visible = isDisplayDeliveryChargeOnSupplier;
                else if (header == "Payment Term")
                    GridView1.Columns[i].Visible = isDisplayPaymentTermOnSupplier;
            }
        }

		protected void Page_Load(object sender, EventArgs e) 
		{
            SetDisplay();
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
                    BindGrid("SupplierName");
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
                Supplier item = new Supplier(id);
                //bind the page 

                ctrlSupplierName.Text = item.SupplierName;

                ctrlSupplierCode.Text = item.SupplierCode;

                ctrlCustomerAddress.Text = item.CustomerAddress;

                ctrlShipToAddress.Text = item.ShipToAddress;

                ctrlBillToAddress.Text = item.BillToAddress;

                ctrlContactNo1.Text = item.ContactNo1;

                ctrlContactNo2.Text = item.ContactNo2;

                ctrlContactNo3.Text = item.ContactNo3;

                ctrlContactPerson1.Text = item.ContactPerson1;

                ctrlContactPerson2.Text = item.ContactPerson2;

                ctrlContactPerson3.Text = item.ContactPerson3;

                ctrlAccountNo.Text = item.AccountNo;

                try
                {
                    ddlCurrency.SelectedValue = item.Userfld2;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                ddlGST.SelectedIndex = item.Userint1.GetValueOrDefault(0);
                if (item.Userint1.HasValue && item.Userint1.Value == 0)
                    ddlGST.SelectedIndex = 3;
                ctrlPaymentTerm.Text = item.Userfld1;
                txtMinOrder.Text = item.Userfloat1.GetValueOrDefault(0).ToString("N2");
                txtDeliveryCharge.Text = item.Userfloat2.GetValueOrDefault(0).ToString("N2");

                chkIsWarehouse.Checked = item.IsWarehouse.GetValueOrDefault(false);
                ddlWarehouseID.SelectedValue = item.WarehouseID.GetValueOrDefault(0).ToString();

                chkIsWarehouse_CheckedChanged(null, null);

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
            var curr = new CurrencyController().FetchAll().Where(o => o.Deleted == false).ToList();
            var ddl = (DropDownList)ddlCurrency;
            ddl.DataTextField = Currency.Columns.CurrencyCode;
            ddl.DataValueField = Currency.Columns.CurrencyCode;
            ddl.DataSource = curr;
            ddl.DataBind();

            string defaultCurr = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency)) ? "SGD" : AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency);
            try
            {
                ddl.SelectedValue = defaultCurr;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
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
			Supplier.Delete(Utility.GetParameter("id"));
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
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Supplier saved.") + "</span>";
			}

			 catch (Exception x) 
			 {
				//haveError = true;
                 lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Supplier not saved:") + "</span> " + x.Message;
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

            Supplier item;
            if (!String.IsNullOrEmpty(id) && id != "0")
            {
                //it's an edit
                item = new Supplier(id);
            }

            else
            {
                //add
                item = new Supplier();
            }

            object valctrlSupplierName = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("SupplierName"), ctrlSupplierName, isAdd, false);

            //item.SupplierName = Convert.ToString(valctrlSupplierName);
            item.SupplierName = Convert.ToString(valctrlSupplierName);

            object valctrlSupplierCode = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("userfld3"), ctrlSupplierCode, isAdd, false);
            item.SupplierCode = Convert.ToString(valctrlSupplierCode);

            object valctrlCustomerAddress = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("CustomerAddress"), ctrlCustomerAddress, isAdd, false);

            if (valctrlCustomerAddress == null)
            {
                item.CustomerAddress = null;
            }

            else
            {

                item.CustomerAddress = Convert.ToString(valctrlCustomerAddress);

            }


            object valctrlShipToAddress = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ShipToAddress"), ctrlShipToAddress, isAdd, false);

            if (valctrlShipToAddress == null)
            {
                item.ShipToAddress = null;
            }

            else
            {

                item.ShipToAddress = Convert.ToString(valctrlShipToAddress);

            }


            object valctrlBillToAddress = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("BillToAddress"), ctrlBillToAddress, isAdd, false);

            if (valctrlBillToAddress == null)
            {
                item.BillToAddress = null;
            }

            else
            {

                item.BillToAddress = Convert.ToString(valctrlBillToAddress);

            }


            object valctrlContactNo1 = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ContactNo1"), ctrlContactNo1, isAdd, false);

            if (valctrlContactNo1 == null)
            {
                item.ContactNo1 = null;
            }

            else
            {

                item.ContactNo1 = Convert.ToString(valctrlContactNo1);

            }


            object valctrlContactNo2 = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ContactNo2"), ctrlContactNo2, isAdd, false);

            if (valctrlContactNo2 == null)
            {
                item.ContactNo2 = null;
            }

            else
            {

                item.ContactNo2 = Convert.ToString(valctrlContactNo2);

            }


            object valctrlContactNo3 = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ContactNo3"), ctrlContactNo3, isAdd, false);

            if (valctrlContactNo3 == null)
            {
                item.ContactNo3 = null;
            }

            else
            {

                item.ContactNo3 = Convert.ToString(valctrlContactNo3);

            }


            object valctrlContactPerson1 = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ContactPerson1"), ctrlContactPerson1, isAdd, false);

            if (valctrlContactPerson1 == null)
            {
                item.ContactPerson1 = null;
            }

            else
            {

                item.ContactPerson1 = Convert.ToString(valctrlContactPerson1);

            }


            object valctrlContactPerson2 = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ContactPerson2"), ctrlContactPerson2, isAdd, false);

            if (valctrlContactPerson2 == null)
            {
                item.ContactPerson2 = null;
            }

            else
            {

                item.ContactPerson2 = Convert.ToString(valctrlContactPerson2);

            }


            object valctrlContactPerson3 = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("ContactPerson3"), ctrlContactPerson3, isAdd, false);

            if (valctrlContactPerson3 == null)
            {
                item.ContactPerson3 = null;
            }

            else
            {

                item.ContactPerson3 = Convert.ToString(valctrlContactPerson3);

            }


            object valctrlAccountNo = Utility.GetDefaultControlValue(Supplier.Schema.GetColumn("AccountNo"), ctrlAccountNo, isAdd, false);

            if (valctrlAccountNo == null)
            {
                item.AccountNo = null;
            }

            else
            {

                item.AccountNo = Convert.ToString(valctrlAccountNo);

            }

            item.Userfld2 = ddlCurrency.SelectedValue.ToString();
            item.Userint1 = ddlGST.SelectedIndex;
            if (item.Userint1.GetValueOrDefault(0) == 3)
                item.Userint1 = 0;
            item.Userfld1 = ctrlPaymentTerm.Text;
            item.Userfloat1 = txtMinOrder.Text.GetDecimalValue();
            item.Userfloat2 = txtDeliveryCharge.Text.GetDecimalValue();

            item.IsWarehouse = chkIsWarehouse.Checked;
            if (ddlWarehouseID.SelectedValue == "0")
                item.WarehouseID = null;
            else
                item.WarehouseID = ddlWarehouseID.SelectedValue.GetIntValue();

            item.Deleted = false;
            //bind it
            item.Save(User.Identity.Name);

            var supplierId = item.SupplierID;

            string strSQL = "update Supplier set SupplierName=N'" + item.SupplierName + "' where SupplierID=" + item.SupplierID;
            QueryCommand cmd = new QueryCommand(strSQL);
            DataService.ExecuteQuery(cmd);
        }

		/// <summary>
		/// Binds the GridView
		/// </summary>
        private void BindGrid(string orderBy)
        {
            string sql = @"SELECT  SupplierID
		                ,SupplierName
		                ,userfld2 Currency
		                ,CASE WHEN ISNULL(userint1,0) = 1 THEN 'Exclusive GST'
			                  WHEN ISNULL(userint1,0) = 2 THEN 'Inclusive GST'
			                  ELSE 'Non GST' END GST
                        ,ISNULL(userfld1,'') PaymentTerm
		                ,ISNULL(userfloat1,0) MinimumOrder
		                ,ISNULL(userfloat2,0) DeliveryCharge
		                ,CustomerAddress
		                ,ShipToAddress
		                ,BillToAddress
		                ,ContactNo1
		                ,ContactNo2
		                ,ContactNo3
		                ,ContactPerson1
		                ,ContactPerson2
		                ,ContactPerson3
		                ,AccountNo
		                ,userfld3
                FROM	Supplier 
                WHERE   1=1
                        AND (ISNULL(SupplierName,'') LIKE N'%{0}%'
                             OR ISNULL(userfld1,'')  LIKE N'%{0}%'
                             OR ISNULL(userfld2,'') LIKE N'%{0}%'
                             OR ISNULL(userfld3,'') LIKE N'%{0}%'
                             OR ISNULL(CustomerAddress,'') LIKE N'%{0}%'
                             OR ISNULL(ShipToAddress,'') LIKE N'%{0}%'
                             OR ISNULL(BillToAddress,'') LIKE N'%{0}%'
                             OR ISNULL(ContactNo1,'') LIKE N'%{0}%'
                             OR ISNULL(ContactNo2,'') LIKE N'%{0}%'
                             OR ISNULL(ContactNo3,'') LIKE N'%{0}%'
                             OR ISNULL(ContactPerson1,'') LIKE N'%{0}%'
                             OR ISNULL(ContactPerson2,'') LIKE N'%{0}%'
                             OR ISNULL(ContactPerson3,'') LIKE N'%{0}%'
                             OR ISNULL(AccountNo,'') LIKE N'%{0}%')
                ORDER BY {1} {2}";
            string sortColumn = null;
            string sortDir = (ViewState[SORT_DIRECTION] + "");
            if (!String.IsNullOrEmpty(orderBy))
                sortColumn = orderBy;
            else if (ViewState[ORDER_BY] != null)
                sortColumn = (string)ViewState[ORDER_BY];
            if (string.IsNullOrEmpty(sortDir))
                sortDir = "ASC";
            ViewState[SORT_DIRECTION] = sortDir;
            ViewState[ORDER_BY] = sortColumn;

            sql = string.Format(sql, txtSearch.Text, sortColumn, sortDir);
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
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

				string pageCount = "<b>" + GridView1.PageCount.ToString() + "</b>";
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

        protected void btnSearchData_Click(object sender, EventArgs e)
        {
            BindGrid(String.Empty);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            BindGrid(String.Empty);
        }

        protected void ddlCurrency_Init(object sender, EventArgs e)
        {

        }

        protected void ddlWarehouseID_Init(object sender, EventArgs e)
        {
            try
            {
                var ddl = (DropDownList)sender;
                var invLocs = new InventoryLocationController().FetchAll().Where(o => o.Deleted.GetValueOrDefault(false) == false).OrderBy(o => o.InventoryLocationName).ToList();
                invLocs.Insert(0, new InventoryLocation
                {
                    InventoryLocationID = 0,
                    InventoryLocationName = "- Please select -"
                });
                ddl.DataSource = invLocs;
                ddl.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void chkIsWarehouse_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsWarehouse.Checked)
                trWarehouseID.Visible = true;
            else
                trWarehouseID.Visible = false;
        }
	}



