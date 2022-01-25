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
using System.Security.Cryptography;
using System.Linq;

namespace PowerWeb.Scaffolds
{
    public partial class TenantSetup : PageBase
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
                        LoadEditor(id);
                }
                else
                {
                    #region *) Privileges
                    try
                    {
                        UserMst um = new UserMst((Session["UserName"] + ""));
                        if (!um.IsHavePrivilegesFor("Add New Tenant"))
                            Response.Redirect(Request.CurrentExecutionFilePath);
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog(ex);
                    }
                    #endregion

                    //it's an add, show the editor
                    if (!Page.IsPostBack)
                    {
                        LoadDrops();
                        txtBusinessStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                        txtBusinessEndDate.Text = DateTime.Now.AddYears(1).ToString("dd MMM yyyy");
                        txtTenantMachineID.Text = PointOfSaleController.FetchTenantMachineID();
                        txtAPIKey.Text = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                    }
                    isAdd = true;
                    ToggleEditor(true);
                    btnDelete.Visible = false;
                }
            }
            else
            {
                ToggleEditor(false);
                if (!Page.IsPostBack)
                    BindGrid(String.Empty);
            }
        }

        protected void btnAddNew_Init(object sender, EventArgs e)
        {
            try
            {
                var ctrl = (HtmlInputButton)sender;
                UserMst um = new UserMst((Session["UserName"] + ""));
                ctrl.Visible = !um.IsNew && um.IsHavePrivilegesFor("Add New Tenant");
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void SetFormSetting()
        {
            try
            {
                bool enableMallManagement = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);
                string posIDPrefix = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.POSIDPrefix);
                string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
                string outletText = LanguageManager.GetTranslation(LabelController.OutletText);

                GridView1.Columns[1].HeaderText = outletText + " "+LanguageManager.GetTranslation("Name");
                GridView1.Columns[4].HeaderText = posText + " " + LanguageManager.GetTranslation("Code");
                GridView1.Columns[5].HeaderText = posText + " " + LanguageManager.GetTranslation("Name");
                GridView1.Columns[6].HeaderText = posText + " " + LanguageManager.GetTranslation("Description");

                this.Page.Title = posText + " " + LanguageManager.GetTranslation("Setup");
                lblPOSID.Text = posText + " " + LanguageManager.GetTranslation("ID");
                if (enableMallManagement)
                    lblPOSPrefix.Text = posIDPrefix;
                lblPointOfSaleName.Text = posText + " " + LanguageManager.GetTranslation("Name");
                lblPOSDesc.Text = posText + " " + LanguageManager.GetTranslation("Description");
                lblOutlet.Text = outletText;
                lblTenantCode.Text = posText + " " + LanguageManager.GetTranslation("Code");
                lblOutletFilter.Text = outletText;
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
                lblID.Text = id.ToString();
                PointOfSale item = new PointOfSale(id);
                txtPointOfSale.Text = item.PointOfSaleName;
                txtPointOfSaleDesc.Text = item.PointOfSaleDescription;
                ddlOutlet.SelectedValue = item.OutletName.ToString();
                txtPhoneNo.Text = item.PhoneNo;
                if (item.DepartmentID.HasValue)
                    ddlDept.SelectedValue = item.DepartmentID.Value.ToString();
                if (item.CreatedOn.HasValue)
                    ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
                ctrlCreatedBy.Text = item.CreatedBy;
                if (item.ModifiedOn.HasValue)
                    ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
                ctrlModifiedBy.Text = item.ModifiedBy;
                if (item.Deleted.HasValue)
                    ctrlDeleted.Checked = item.Deleted.Value;
                txtMembershipCode.Text = item.MembershipCode;
                txtPriceSchemeID.Text = item.PriceSchemeID;
                if (string.IsNullOrEmpty(item.TenantMachineID))
                    txtTenantMachineID.Text = PointOfSaleController.FetchTenantMachineID();
                else
                    txtTenantMachineID.Text = item.TenantMachineID;

                try
                {
                    if (!string.IsNullOrEmpty(item.ApiKey))
                    {
                        txtAPIKey.Text = PowerPOS.UserController.DecryptData(item.ApiKey);
                    }
                }
                catch (Exception exx)
                {
                    Logger.writeLog(exx);
                }
                try
                {
                    ddlValidationStatus.SelectedValue = item.InterfaceValidationStatus;
                    ddlRetailerLevel.SelectedValue = item.RetailerLevel;
                    ddlShopNo.SelectedValue = item.ShopNo;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                txtRetailerContactPerson.Text = item.RetailerContactPerson;
                txtRetailerContactNo.Text = item.RetailerContactNo;
                txtRetailerEmail.Text = item.RetailerEmail;
                try
                {
                    ddlInterfaceDevTeam.SelectedValue = item.InterfaceDevTeam;
                    rblOption.SelectedValue = item.OptionX;
                    ddlPOSType.SelectedValue = item.POSType;
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
                txtVendorContactName.Text = item.VendorContactName;
                txtVendorContactNo.Text = item.VendorContactNo;
                txtBusinessStartDate.Text = item.BusinessStartDate.GetValueOrDefault(DateTime.Now).ToString("dd MMM yyyy");
                txtBusinessEndDate.Text = item.BusinessEndDate.GetValueOrDefault(DateTime.Now).ToString("dd MMM yyyy");
                txtTenantCompanyName.Text = item.TenantCompanyName;
                txtRetailerDesignation.Text = item.RetailerDesignation;
                txtVendorEmail.Text = item.VendorEmail;
                txtPOSBrand.Text = item.POSBrand;
                txtPOSOS.Text = item.Posos;
                txtPOSSoftware.Text = item.POSSoftware;
                txtNoOfPOS.Text = item.NoOfPOS.GetValueOrDefault(0).ToString();
                ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
                //set the delete confirmation
                btnDelete.Attributes.Add("onclick", "return CheckDelete();");
            }
            else
            {
                txtBusinessStartDate.Text = DateTime.Now.ToString("dd MMM yyyy");
                txtBusinessEndDate.Text = DateTime.Now.AddYears(1).ToString("dd MMM yyyy");
                txtTenantMachineID.Text = PointOfSaleController.FetchTenantMachineID();
            }
        }

        /// <summary>
        /// Loads the DropDownLists
        /// </summary>
        void LoadDrops()
        {
            //load the listboxes
            ListItem[] li = PointOfSaleController.FetchOutletNamesForDropDown();
            Utility.LoadDropDown(ddlOutlet, li, "Text", "Value", "Web");
            Query qryctrlDepartmentID = Department.CreateQuery();
            qryctrlDepartmentID.OrderBy = OrderBy.Asc("DepartmentName");
            Utility.LoadDropDown(ddlDept, qryctrlDepartmentID.ExecuteReader(), true);
            ddlDept.Items.Insert(0, new ListItem("(Not Specified)", String.Empty));
            ddlDept.SelectedValue = "10001";

            var interfaceDevTem = (AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.InterfaceDevTeam) + "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ddlInterfaceDevTeam.DataSource = interfaceDevTem;
            ddlInterfaceDevTeam.DataBind();

            var shopMall = RetailerLevelController.FetchMallName();
            if (shopMall.Count == 0)
                shopMall.Add("NO-DATA");
            ddlOutlet_SelectedIndexChanged(ddlOutlet, new EventArgs());
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
            PointOfSale pos = new PointOfSale(Utility.GetParameter("id"));
            AccessLogController.AddTenantHistory(pos.TenantMachineID, pos.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), string.Format("DELETE {0} : {1}",LabelController.PointOfSaleText,  pos.PointOfSaleName));
            PointOfSale.Delete(Utility.GetParameter("id"));
            //redirect
            Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string id = Utility.GetParameter("id");
            //bool haveError = false;
            string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
            try
            {
                BindAndSave(id);
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + posText + " "+LanguageManager.GetTranslation("saved.")+"</span>";
            }
            catch (Exception x)
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + posText + " " + LanguageManager.GetTranslation("not saved:")+"</span> " + x.Message;
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
            #region *) Validation

            if (string.IsNullOrEmpty(txtPointOfSale.Text))
                throw new Exception(LanguageManager.GetTranslation(LabelController.PointOfSaleText) + " "+ LanguageManager.GetTranslation("Cannot Be Empty"));

            if (txtBusinessStartDate.Text.GetDateTimeValue("dd MMM yyyy").Date >=
                txtBusinessEndDate.Text.GetDateTimeValue("dd MMM yyyy").Date)
                throw new Exception(LanguageManager.GetTranslation("Business End Date must be bigger than Business Start Date"));
            string shopMall = ddlOutlet.SelectedValue + "";
            string retailerLevel = ddlRetailerLevel.SelectedValue + "";
            string shopNo = ddlShopNo.SelectedValue + "";

            if (!retailerLevel.ToUpper().Equals("NO-DATA") && !shopNo.ToUpper().Equals("NO-DATA"))
            {
                string sql = @"DECLARE @StartDate AS DATETIME;
                                DECLARE @EndDate AS DATETIME;

                                SET @StartDate = '{0}';
                                SET @EndDate = '{1}';

                                SELECT  PointOfSaleID, PointOfSaleName, BusinessStartDate, BusinessEndDate
                                FROM	PointOfSale 
                                WHERE	ISNULL(Deleted,0) = 0
                                        AND PointOfSaleID <> '{4}'
                                        AND RetailerLevel = '{2}'
                                        AND ShopNo = '{3}'
                                        AND OutletName = '{5}'
                                        AND (CAST(@StartDate AS DATE) BETWEEN CAST(BusinessStartDate AS DATE) AND CAST(BusinessEndDate AS DATE)
			                                 OR CAST(@EndDate AS DATE) BETWEEN CAST(BusinessStartDate AS DATE) AND CAST(BusinessEndDate AS DATE)
			                                 OR CAST(BusinessStartDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
			                                 OR CAST(BusinessEndDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE))";
                sql = string.Format(sql, txtBusinessStartDate.Text.GetDateTimeValue("dd MMM yyyy").ToString("yyyy-MM-dd")
                                       , txtBusinessEndDate.Text.GetDateTimeValue("dd MMM yyyy").ToString("yyyy-MM-dd")
                                       , retailerLevel
                                       , shopNo
                                       , id
                                       , shopMall);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                if (dt.Rows.Count > 0)
                {
                    if (id != (dt.Rows[0]["PointOfSaleID"] + ""))
                    {
                        throw new Exception(string.Format("Shop No {0} at {5} level {1} is taken by {2} from {3} to {4}",
                            shopNo, 
                            retailerLevel, 
                            dt.Rows[0]["PointOfSaleName"],
                            ((DateTime)dt.Rows[0]["BusinessStartDate"]).ToString("dd-MMM-yyyy"),                            
                            ((DateTime)dt.Rows[0]["BusinessEndDate"]).ToString("dd-MMM-yyyy"),
                            shopMall));
                    }
                }
            }

            #endregion

            PointOfSale item;
            if (!String.IsNullOrEmpty(id) && id != "0")
                item = new PointOfSale(id);
            else
                item = new PointOfSale();

            item.PointOfSaleName = txtPointOfSale.Text;
            item.PointOfSaleDescription = txtPointOfSaleDesc.Text;
            item.OutletName = (ddlOutlet.SelectedValue + "");
            item.PhoneNo = txtPhoneNo.Text;
            item.DepartmentID = 0;
            item.MembershipCode = txtMembershipCode.Text;
            item.QuickAccessGroupID = new Guid(ddlQuickAccess.SelectedValue.ToString());
            item.PriceSchemeID = txtPriceSchemeID.Text;
            if(!string.IsNullOrEmpty(txtAPIKey.Text))
                item.ApiKey = PowerPOS.UserController.EncryptData(txtAPIKey.Text);
            item.InterfaceValidationStatus = ddlValidationStatus.Text;
            item.RetailerLevel = ddlRetailerLevel.SelectedValue + "";
            item.ShopNo = ddlShopNo.SelectedValue + "";
            item.RetailerContactPerson = txtRetailerContactPerson.Text;
            item.RetailerContactNo = txtRetailerContactNo.Text;
            item.RetailerEmail = txtRetailerEmail.Text;
            item.InterfaceDevTeam = ddlInterfaceDevTeam.SelectedValue + "";
            item.VendorContactName = txtVendorContactName.Text;
            item.VendorContactNo = txtVendorContactNo.Text;
            if(txtTenantMachineID.Text == "")
                txtTenantMachineID.Text = PointOfSaleController.FetchTenantMachineID();
            item.TenantMachineID = txtTenantMachineID.Text;
            item.BusinessStartDate = txtBusinessStartDate.Text.GetDateTimeValue("dd MMM yyyy");
            item.BusinessEndDate = txtBusinessEndDate.Text.GetDateTimeValue("dd MMM yyyy");
            item.Deleted = ctrlDeleted.Checked;
            item.OptionX = rblOption.SelectedValue;
            item.TenantCompanyName = txtTenantCompanyName.Text;
            item.RetailerDesignation = txtRetailerDesignation.Text;
            item.VendorEmail = txtVendorEmail.Text;
            item.POSType = ddlPOSType.SelectedValue + "";
            item.POSBrand = txtPOSBrand.Text;
            item.Posos = txtPOSOS.Text;
            item.POSSoftware = txtPOSSoftware.Text;
            item.NoOfPOS = txtNoOfPOS.Text.GetIntValue();
            item.Mall = ddlOutlet.SelectedValue + "";
            //bind it
            bool isNew = item.IsNew;
            item.Save((string)Session["UserName"]);
            string tenantText = LabelController.PointOfSaleText;
            if (isNew)
                AccessLogController.AddTenantHistory(item.TenantMachineID, item.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), string.Format("ADD {0} : {1}",tenantText, item.PointOfSaleName));
            else
                AccessLogController.AddTenantHistory(item.TenantMachineID, item.PointOfSaleID, AccessSource.WEB, (Session["UserName"] + ""), string.Format("EDIT {0} : {1}",tenantText, item.PointOfSaleName));
        }

        /// <summary>
        /// Binds the GridView
        /// </summary>
        private void BindGrid(string sortBy)
        {
            try
            {
                string sortCriteria = "";
                string sortDir = ViewState[SORT_DIRECTION] + "";
                if (string.IsNullOrEmpty(sortDir))
                    sortDir = "ASC";
                if (string.IsNullOrEmpty(sortBy))
                    sortCriteria = "OutletName, ShopLevel, ShopNo";
                else
                    sortCriteria = string.Format("{0} {1}", sortBy, sortDir);

                string outletName = ddlFilterOutlet.SelectedValue + "";
                string validationStatus = ddlFilterValidationStatus.SelectedValue + "";
                string retailerLevel = ddlFilterRetailerLevel.SelectedValue + "";
                string shopNo = ddlFilterShopNo.SelectedValue + "";
                DateTime expiryDate = DateTime.Now.AddYears(200);
                string search = txtSearch.Text;
                string dateMark = "<";

                if (ddlBusinessExpiration.SelectedIndex == 1)
                {
                    expiryDate = DateTime.Now;
                    dateMark = ">=";
                }
                else if (ddlBusinessExpiration.SelectedIndex == 2)
                    expiryDate = DateTime.Now;
                else if (ddlBusinessExpiration.SelectedIndex == 3)
                    expiryDate = DateTime.Now.AddMonths(1);
                else if (ddlBusinessExpiration.SelectedIndex == 4)
                    expiryDate = DateTime.Now.AddMonths(2);
                else if (ddlBusinessExpiration.SelectedIndex == 5)
                    expiryDate = DateTime.Now.AddMonths(3);
                else if (ddlBusinessExpiration.SelectedIndex == 6)
                    expiryDate = DateTime.Now.AddMonths(6);

                string sql = @"DECLARE @OutletName NVARCHAR(200);
                            DECLARE @ValidationStatus NVARCHAR(200);
                            DECLARE @RetailerLevel NVARCHAR(200);
                            DECLARE @ShopNo NVARCHAR(200);
                            DECLARE @ExpiryDate DATETIME;
                            DECLARE @Search NVARCHAR(MAX);
                            DECLARE @ShowDeleted BIT;

                            SET @OutletName = N'{0}';
                            SET @ValidationStatus = N'{1}';
                            SET @RetailerLevel = N'{2}';
                            SET @ShopNo = N'{3}';
                            SET @ExpiryDate = '{4}';
                            SET @Search = N'%{5}%'
                            SET @ShowDeleted = {6};                            

                            SELECT   POS.PointOfSaleID
		                            ,POS.OutletName
		                            ,POS.RetailerLevel ShopLevel
		                            ,POS.ShopNo ShopNo
		                            ,POS.TenantMachineID TenantCode
		                            ,POS.PointOfSaleName TenantName
		                            ,POS.PointOfSaleDescription TenantDescription
		                            ,POS.BusinessStartDate
		                            ,POS.BusinessEndDate
		                            ,POS.InterfaceValidationStatus
		                            ,POS.RetailerContactNo
		                            ,POS.RetailerContactPerson
		                            ,POS.RetailerEmail
                                    ,POS.Deleted
                            FROM	PointOfSale POS
                            WHERE	(ISNULL(POS.Deleted,0) = 0 OR @ShowDeleted = 1)
		                            AND (POS.OutletName = @OutletName OR @OutletName = 'ALL')
		                            AND (POS.InterfaceValidationStatus = @ValidationStatus OR @ValidationStatus = 'ALL')
		                            AND (POS.RetailerLevel = @RetailerLevel OR @RetailerLevel = 'ALL')
		                            AND (POS.ShopNo = @ShopNo OR @ShopNo = 'ALL')			
		                            AND (POS.BusinessEndDate IS NULL OR 
			                            CAST(ISNULL(POS.BusinessEndDate, GETDATE()) AS DATE) {8} @ExpiryDate)
		                            AND (POS.PointOfSaleName LIKE @Search
			                             OR ISNULL(POS.PointOfSaleDescription,'') LIKE @Search
			                             OR ISNULL(POS.RetailerContactNo,'') LIKE @Search
			                             OR ISNULL(POS.RetailerContactPerson,'') LIKE @Search
			                             OR ISNULL(POS.RetailerEmail,'') LIKE @Search
                                         OR ISNULL(POS.TenantMachineID,'') LIKE @Search)
                            ORDER BY {7}";
                sql = string.Format(sql, outletName
                                       , validationStatus
                                       , retailerLevel
                                       , shopNo
                                       , expiryDate.ToString("yyyy-MM-dd")
                                       , search
                                       , chkShowDeletedItems.Checked ? 1 : 0
                                       , sortCriteria
                                       , dateMark);
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
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
                        lstItem.Selected = true;
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
                    itemCount = ds.Tables[0].Rows.Count;
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
                ViewState[SORT_DIRECTION] = SqlFragment.DESC;
            else
                ViewState[SORT_DIRECTION] = SqlFragment.ASC;
            BindGrid(columnName);
        }


        private string GetSortDirection(string sortBy)
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
                    ViewState["sortBy"] = sortBy;
            }
            else
                ViewState["sortBy"] = sortBy;
            return sortDir;
        }

        protected void btnGenerateAPIKey_Click(object sender, EventArgs e)
        {
            txtAPIKey.Text = Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        protected void ddlRetailerLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var data = RetailerLevelController.FetchShopNo(ddlOutlet.SelectedValue+"", ddlRetailerLevel.SelectedValue + "");
                if(data.Count == 0)
                    data.Add("NO-DATA");
                ddlShopNo.DataSource = data;
                ddlShopNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void ddlFilterOutlet_Init(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            ddl.DataSource = OutletController.FetchAll(false, true);
            ddl.DataBind();
            ddlFilterOutlet_SelectedIndexChanged(ddlFilterOutlet, new EventArgs());
        }

        protected void ddlFilterRetailerLevel_Init(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            var data = RetailerLevelController.FetchRetailerLevel();
            data.Insert(0, "ALL");
            ddl.DataSource = data;
            ddl.DataBind();
            ddl.SelectedIndex = 0;
            ddlFilterRetailerLevel_SelectedIndexChanged(ddl, new EventArgs());
        }

        protected void ddlFilterShopNo_Init(object sender, EventArgs e)
        {
            var data = RetailerLevelController.FetchShopNo();
            data.Insert(0, "ALL");
            ddlFilterShopNo.DataSource = data;
            ddlFilterShopNo.DataBind();
        }

        protected void ddlFilterRetailerLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var data = RetailerLevelController.FetchShopNo(ddlFilterOutlet.SelectedValue+"", 
                    ddlFilterRetailerLevel.SelectedValue + "");
                data.Insert(0, "ALL");
                ddlFilterShopNo.DataSource = data;
                ddlFilterShopNo.DataBind();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid("");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlFilterOutlet.SelectedIndex = 0;
            ddlFilterRetailerLevel.SelectedIndex = 0;
            ddlFilterRetailerLevel_SelectedIndexChanged(ddlFilterRetailerLevel, new EventArgs());
            ddlFilterShopNo.SelectedIndex = 0;
            ddlBusinessExpiration.SelectedIndex = 0;
            ddlFilterValidationStatus.SelectedIndex = 0;
            txtSearch.Text = "";
            BindGrid("");
        }

        protected void lnkExport_Click(object sender, EventArgs e)
        {
            BindGrid("");
            string posText = LabelController.PointOfSaleText;
            DataTable dt = ((DataTable)GridView1.DataSource);
            CommonWebUILib.ExportCSV(dt, this.Page.ToString().Trim(' '), posText+" Report", GridView1);
        }

        protected void ddlOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = RetailerLevelController.FetchRetailerLevel(ddlOutlet.SelectedValue + "");
            if (data.Count == 0)
                data.Add("NO-DATA");
            ddlRetailerLevel.DataSource = data;
            ddlRetailerLevel.DataBind();
            ddlRetailerLevel_SelectedIndexChanged(ddlRetailerLevel, new EventArgs());
        }

        protected void ddlFilterOutlet_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var data = RetailerLevelController.FetchRetailerLevel(ddlFilterOutlet.SelectedValue + "");
                data.Insert(0, "ALL");
                ddlFilterRetailerLevel.DataSource = data;
                ddlFilterRetailerLevel.DataBind();
                ddlFilterRetailerLevel_SelectedIndexChanged(ddlFilterRetailerLevel, new EventArgs());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
