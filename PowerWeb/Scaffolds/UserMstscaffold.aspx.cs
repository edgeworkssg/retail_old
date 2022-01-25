using Google.GData.Calendar;
using System.Collections.Generic;

//Generated on 23/4/2007 6:05:07 PM by Albert
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
using System.Threading;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Net;
using Edgeworks.Model.CustomerMaster;
using Edgeworks.Model;

public partial class UserMstscaffold : PageBase
{
    public bool isUseSupplierPortal = false;

    private bool isAdd = false;
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string ORDER_BY = "ORDER_BY";
    protected void Page_Load(object sender, EventArgs e)
    {
        SetFormSetting();
        if (!Page.IsPostBack)
        {
            ViewState["changePwd"] = false;
        }
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
                btnAdd2.Visible = false;
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
            if (!Page.IsPostBack)
            {
                BindGrid(String.Empty);
            }
        }
    }


    private void SetFormSetting()
    {
        try
        {
            string posText = LanguageManager.GetTranslation(LabelController.PointOfSaleText);
            string outletText = LanguageManager.GetTranslation(LabelController.OutletText);
            bool showPOS = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowPointOfSale), false);
            bool showOutlet = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowOutlet), false);
            isUseSupplierPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);

            trPOS.Visible = showPOS;
            trOutlet.Visible = showOutlet;

            GridView1.Columns[8].Visible = showOutlet;
            GridView1.Columns[8].HeaderText = outletText;
            GridView1.Columns[9].Visible = showPOS;
            GridView1.Columns[9].HeaderText = posText;

            lblPOS.Text = posText;
            lblOutlet.Text = outletText;
            ddlMultiOutlet.Texts.SelectBoxCaption = "Select " + outletText;
            ddlMultiPOS.Texts.SelectBoxCaption = "Select " + posText;
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
            txtID.Text = id.ToString();
            txtID.ReadOnly = true;
            txtCfmPwd.Visible = false;
            ctrlPassword.Visible = false;
            btnChangePwd.Visible = true;

            //pull the record
            UserMst item = new UserMst(id);
            //bind the page
            txtEmail.Text = item.Email;
            txtDisplayName.Text = item.DisplayName;
            ctrlPassword.Text = item.Password;
            ctrlRemark.Text = item.Remark;
            ctrlGroupName.SelectedValue = item.GroupName.ToString();
            ctrlDeptName.SelectedValue = item.DepartmentID.Value.ToString();
            ctrlCreatedBy.Text = item.CreatedBy;
            lblBarcodeToken.Text = item.BarcodeToken;
            btnGenerateToken.Visible = string.IsNullOrEmpty(item.BarcodeToken);
            btnRemoveToken.Visible = !btnGenerateToken.Visible;
            if (item.CreatedOn.HasValue)
            {
                ctrlCreatedOn.Text = item.CreatedOn.Value.ToString();
            }

            try
            {
                string[] selOutlet = item.AssignedOutletList;
                for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
                    ddlMultiOutlet.Items[i].Selected = selOutlet.Contains(ddlMultiOutlet.Items[i].Value);

                string[] selPOS = item.AssignedPOSList;
                for (int i = 0; i < ddlMultiPOS.Items.Count; i++)
                    ddlMultiPOS.Items[i].Selected = selPOS.Contains(ddlMultiPOS.Items[i].Value);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            ctrlModifiedBy.Text = item.ModifiedBy;
            if (item.ModifiedOn.HasValue)
            {
                ctrlModifiedOn.Text = item.ModifiedOn.Value.ToString();
            }
            cbIsSalesPerson.Checked = item.IsASalesPerson;
            ddGroupName.SelectedValue = item.SalesPersonGroupID.ToString();
            //set the delete confirmation
            btnDelete.Attributes.Add("onclick", "return CheckDelete();");

            ctrlBasicSalary.Text = item.BasicSalary.ToString("N2");
            ctrlOtherAllowance.Text = item.OtherAllowance.ToString("N2");

            chkIsSupplier.Checked = item.IsSupplier;
            chkIsRestrictedSupplierList.Checked = item.IsRestrictedSupplierList;

            if (isUseSupplierPortal)
            {
                ViewState["SupplierList"] = item.GetSupplierList();
                BindDetails();
            }
        }
    }

    /// <summary>
    /// Loads the DropDownLists
    /// </summary>
    void LoadDrops()
    {
        //load the listboxes
        Query qryctrlGroupName = UserGroup.CreateQuery();
        qryctrlGroupName.OrderBy = OrderBy.Asc("GroupName");
        Utility.LoadDropDown(ctrlGroupName, qryctrlGroupName.ExecuteReader(), true);

        Query qryctrlDeptName = Department.CreateQuery();
        qryctrlDeptName.OrderBy = OrderBy.Asc("DepartmentName");
        Utility.LoadDropDown(ctrlDeptName, qryctrlDeptName.ExecuteReader(), true);

        var qr = new Query("Outlet");
        qr.AddWhere(Outlet.Columns.Deleted, Comparison.Equals, 0);
        var ouList = new OutletController()
                    .FetchByQuery(qr)
                    .OrderByDescending(o => o.OutletName)
                    .ToList();

        ddlMultiOutlet.Items.Clear();
        foreach (var ou in ouList)
            ddlMultiOutlet.Items.Add(new ListItem { Value = ou.OutletName, Text = ou.OutletName });

        qr = new Query("PointOfSale");
        qr.AddWhere(PointOfSale.Columns.Deleted, Comparison.Equals, 0);
        var posList = new PointOfSaleController()
                    .FetchByQuery(qr)
                    .OrderByDescending(o => o.PointOfSaleName)
                    .ToList();

        ddlMultiPOS.Items.Clear();
        foreach (var pos in posList)
            ddlMultiPOS.Items.Add(new ListItem { Value = pos.PointOfSaleID.ToString(), Text = pos.PointOfSaleName });
    }

    private void LoadDropdown(string search)
    {
        if (search == string.Empty)
            search = "%";

        string sql = "select * from Supplier where ISNULL(Deleted, 0) = 0 and ISNULL(SupplierName,'') LIKE '%" + search.Replace("'", "''") + "%' order by SupplierName";
        DataSet ds = DataService.GetDataSet(new QueryCommand(sql));
        SupplierCollection col = new SupplierCollection();
        col.Load(ds.Tables[0]);

        ddlSupplier.DataSource = col;
        ddlSupplier.DataBind();
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
        Random r = new Random();
        byte[] b = new byte[100];
        //UserMst.Delete(Utility.GetParameter("id"));
        UserMst u = new UserMst(Utility.GetParameter("id"));
        r.NextBytes(b);
        u.Password = b.ToString();
        u.Deleted = true;
        u.Save();

        //redirect
        AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("DELETE UserMst : {0}", Utility.GetParameter("id")), "");

        #region *) Audit Log
        AuditLogController.AddLog("Deletion", "UserMst", "UserName", u.UserName, "", u.UserGroup.GroupName, Session["username"].ToString());
        #endregion

        UpdateToCustomerMaster(u);

        Response.Redirect(Request.CurrentExecutionFilePath);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string id = Utility.GetParameter("id");
        //bool haveError = false;
        try
        {
            BindAndSave(id);
            lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("User saved.") + "</span>";
        }

        catch (Exception x)
        {

            if (x.Message.Contains("Violation of PRIMARY KEY constraint"))
            {
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("User name:") + " " + txtID.Text + " " + LanguageManager.GetTranslation("already exist. Please use another name.") + " </span> ";
            }
            else
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("User not saved:") + "</span> " + x.Message;
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
        QueryCommandCollection cmdCol = new QueryCommandCollection();

        if (cbIsSalesPerson.Checked && ddGroupName.SelectedIndex == 0)
        {
            lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Please select Sales Person Group") + " </span> ";
            return;
        }
        UserMst item;
        UserMst originalItem = new UserMst();

        if (!String.IsNullOrEmpty(id) && id != "0")
        {
            //it's an edit
            item = new UserMst(id);
            item.CopyTo(originalItem);
            originalItem.IsNew = false;
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("UPDATE UserMst : {0}", id), "");
        }

        else
        {
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("ADD UserMst : {0}", txtID.Text), "");
            //add
            item = new UserMst();
            item.Deleted = false;
            item.UserName = txtID.Text;
            item.DisplayName = txtDisplayName.Text;
            ViewState["changePwd"] = true;
        }

        //dont touch the password if it is an edit....
        if ((bool)ViewState["changePwd"] == true)
        {
            object valctrlPassword = Utility.GetDefaultControlValue(UserMst.Schema.GetColumn("Password"), ctrlPassword, isAdd, false);
            item.Password = PowerPOS.UserController.EncryptData(Convert.ToString(valctrlPassword));
        }
        object valctrlRemark = Utility.GetDefaultControlValue(UserMst.Schema.GetColumn("Remark"), ctrlRemark, isAdd, false);
        if (valctrlRemark == null)
        {
            item.Remark = null;
        }

        else
        {
            item.Remark = Convert.ToString(valctrlRemark);
        }

        if (!string.IsNullOrEmpty(txtEmail.Text))
        {
            if (!txtEmail.Text.IsValidEmail())
                throw new Exception("Invalid email " + txtEmail.Text);

            var existingEmail = new UserMst(UserMst.UserColumns.Email, txtEmail.Text);
            if (!existingEmail.IsNew &&
                !item.UserName.ToLower().Equals(existingEmail.UserName.ToLower()))
                throw new Exception(string.Format("Email {0} already used for other user ({1})", txtEmail.Text, existingEmail.UserName));
        }

        item.Email = txtEmail.Text;
        item.DisplayName = txtDisplayName.Text;
        item.IsASalesPerson = cbIsSalesPerson.Checked;
        item.BarcodeToken = lblBarcodeToken.Text;
        if (cbIsSalesPerson.Checked)
        {
            item.SalesPersonGroupID = int.Parse(ddGroupName.SelectedValue.ToString());
        }
        else
        {
            item.SalesPersonGroupID = 0;
        }
        object valctrlGroupName = Utility.GetDefaultControlValue(UserMst.Schema.GetColumn("GroupName"), ctrlGroupName, isAdd, false);
        item.GroupName = Convert.ToInt32(valctrlGroupName);
        object valctrlDeptID = Utility.GetDefaultControlValue(UserMst.Schema.GetColumn("DepartmentID"), ctrlDeptName, isAdd, false);
        item.DepartmentID = Convert.ToInt32(valctrlDeptID);

        var selOutlet = new List<string>();
        for (int i = 0; i < ddlMultiOutlet.Items.Count; i++)
        {
            if (ddlMultiOutlet.Items[i].Selected)
                selOutlet.Add(ddlMultiOutlet.Items[i].Value);
        }
        if (selOutlet.Count == ddlMultiOutlet.Items.Count)
        {
            selOutlet.Clear();
            selOutlet.Add("ALL");
        }
        item.AssignedOutletList = selOutlet.ToArray();

        var selPOS = new List<string>();
        for (int i = 0; i < ddlMultiPOS.Items.Count; i++)
        {
            if (ddlMultiPOS.Items[i].Selected)
                selPOS.Add(ddlMultiPOS.Items[i].Value);
        }
        if (selPOS.Count == ddlMultiPOS.Items.Count)
        {
            selPOS.Clear();
            selPOS.Add("ALL");
        }
        item.AssignedPOSList = selPOS.ToArray();


        decimal _basicSalary = 0m;
        decimal.TryParse(ctrlBasicSalary.Text, out _basicSalary);
        item.BasicSalary = _basicSalary;

        decimal _otherAllowance = 0m;
        decimal.TryParse(ctrlOtherAllowance.Text, out _otherAllowance);
        item.OtherAllowance = _otherAllowance;

        item.IsSupplier = chkIsSupplier.Checked;
        item.IsRestrictedSupplierList = chkIsRestrictedSupplierList.Checked;

        //bind it
        //item.Save(Session["username"].ToString());
        cmdCol.Add(item.GetSaveCommand(Session["username"].ToString()));

        if (isUseSupplierPortal && ViewState["SupplierList"] != null)
        {
            DataTable suppliers = (DataTable)ViewState["SupplierList"];
            for (int i = 0; i < suppliers.Rows.Count; i++)
            {
                var dr = suppliers.Rows[i];

                if (dr["SupplierID"] + "" == "") continue;

                Supplier s = new Supplier(dr["SupplierID"].ToString().GetIntValue());
                if (s.SupplierID == dr["SupplierID"].ToString().GetIntValue())
                {
                    if ((dr["Deleted"].ToString().ToLower() != "true"))
                    {
                        s.LinkedUser = item.UserName;
                        cmdCol.Add(s.GetUpdateCommand(Session["username"].ToString()));
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(s.LinkedUser) && !string.IsNullOrEmpty(item.UserName) && s.LinkedUser.ToLower() == item.UserName.ToLower())
                        {
                            s.LinkedUser = "";
                            cmdCol.Add(s.GetUpdateCommand(Session["username"].ToString()));
                        }
                    }
                }
            }
        }

        if (!UpdateToCustomerMaster(item))
            throw new Exception("Failed update user to customer master");

        cmdCol.RemoveAll(c => c == null);
        if (cmdCol.Count > 0)
            DataService.ExecuteTransaction(cmdCol);

        LoadEditor(item.UserName);

        #region *) Audit Log
        string operation;
        if (originalItem.IsNew)
        {
            operation = "User Creation";
            AuditLogController.AddLog(operation, "UserMst", "UserName", item.UserName, "", item.UserGroup.GroupName, Session["username"].ToString());
        }
        else if (item.GroupName != originalItem.GroupName)
        {
            operation = "Role Changed";
            AuditLogController.AddLog(operation, "UserMst", "UserName", item.UserName, originalItem.UserGroup.GroupName, item.UserGroup.GroupName, Session["username"].ToString());
        }
        #endregion

        #region *) Create Google Calendars
        string useGoogleAppointment = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.IsAvailable);
        if (item.IsASalesPerson && useGoogleAppointment != null && bool.Parse(useGoogleAppointment))
        {
            try
            {
                bool isCalendarExist = false;
                List<CalendarEntry> allCalendars = GoogleController.GetOwnCalendar();
                foreach (CalendarEntry oneCalendar in allCalendars)
                {
                    if (oneCalendar.Title.Text.ToLower() == item.UserName.ToLower())
                    { isCalendarExist = true; break; }
                }

                if (!isCalendarExist)
                {
                    /// Create new google calendar
                    GoogleController.CreateNewCalendar(item.UserName);
                }
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                throw new Exception("Failed to integrate with Google Calendar (User saved successfully)");
            }
        }
        #endregion
    }

    private bool UpdateToCustomerMaster(UserMst data)
    {
        bool isSuccess = false;

        try
        {
            string customerMasterURL = AppSetting.GetSetting(AppSetting.SettingsName.CustomerMasterURL);
            string callerID = AppSetting.GetSetting(AppSetting.SettingsName.APICallerID);
            string privateKey = AppSetting.GetSetting(AppSetting.SettingsName.APIPrivateKey);

            if (string.IsNullOrEmpty(customerMasterURL))
                return true;
            if (string.IsNullOrEmpty(callerID))
                return true;
            if (string.IsNullOrEmpty(privateKey))
                return true;

            if (string.IsNullOrEmpty(data.Email))
                return true;

            if (!data.Email.IsValidEmail())
                return true;

            privateKey = UserController.DecryptData(privateKey);

            using (WebClient client = new WebClient())
            {
                client.Encoding = System.Text.UTF8Encoding.UTF8;

                string baseURL = customerMasterURL + "/API/Merchant/SaveAccount";

                client.Headers.Add("CallerID", callerID);
                client.Headers.Add("Authorization", "Bearer " + privateKey);
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");

                var input = new CompanyAccountModel();
                input.Email = data.Email;
                input.CashierID = data.UserName;
                input.Password = data.Password;
                input.DisplayName = data.DisplayName;
                input.Remark = data.Remark;
                input.Outlet = data.AssignedOutlet;
                input.Deleted = data.Deleted.GetValueOrDefault(false);

                string inputStr = input.ConvertToJSON();
                string resultStr = client.UploadString(baseURL, inputStr);
                var result = resultStr.ConvertFromJSON<ResultModel>();
                if (result.Status != Edgeworks.Model.StatusCode.SUCCESS.ToString())
                {
                    Logger.writeLog(resultStr);
                    throw new Exception(result.Message);
                }
            }

            isSuccess = true;
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }

        return isSuccess;
    }

    /// <summary>
    /// Binds the GridView
    /// </summary>
    private void BindGrid(string orderBy)
    {
        try
        {
            DataTable dt = new DataTable();
            string sql = @"
            SELECT	 dbo.UserMst.UserName
		            ,dbo.UserMst.DisplayName
		            ,dbo.UserMst.Password
		            ,dbo.UserMst.Remark
		            ,dbo.UserMst.SalesPersonGroupID
		            ,dbo.UserMst.DepartmentID
		            ,dbo.UserGroup.GroupDescription
		            ,dbo.UserGroup.Deleted AS GroupDeleted
		            ,dbo.UserMst.Deleted
		            ,dbo.UserGroup.GroupID
		            ,dbo.UserGroup.GroupName
		            ,ISNULL(dbo.SalesGroup.GroupName, '') AS SalesPersonGroupName
		            ,dbo.UserMst.IsASalesPerson
		            ,dbo.UserMst.userfld1 Outlet
                    ,dbo.UserMst.userfld5 PointOfSale
                    ,dbo.UserMst.userfld7 Barcode
            FROM	dbo.UserGroup 
		            INNER JOIN dbo.UserMst ON dbo.UserGroup.GroupID = dbo.UserMst.GroupName 
		            LEFT OUTER JOIN dbo.SalesGroup ON dbo.UserMst.SalesPersonGroupID = dbo.SalesGroup.SalesGroupId
            WHERE	dbo.UserMst.Deleted = 0";
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            dt.Columns["PointOfSale"].ReadOnly = false;
            dt.Columns["PointOfSale"].MaxLength = int.MaxValue;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string posIDs = (dt.Rows[i]["PointOfSale"]) + "";
                if (!string.IsNullOrEmpty(posIDs) && !posIDs.ToUpper().Equals("ALL"))
                {
                    try
                    {
                        sql = @"SELECT  TOP 1 STUFF((SELECT DISTINCT ',' + PointOfSaleName
			                                      FROM	PointOfSale
			                                      WHERE	ISNULL(Deleted,0) = 0
					                                    AND PointOfSaleID IN ({0})
			                                      FOR XML PATH (''))
			                                      , 1, 1, '') AS PointOfSaleName
                                    FROM	PointOfSale POS 
                                    WHERE	ISNULL(POS.Deleted,0) = 0
		                                    AND POS.PointOfSaleID IN ({0})";
                        sql = string.Format(sql, posIDs);
                        var dtPOS = new DataTable();
                        dtPOS.Load(DataService.GetReader(new QueryCommand(sql)));
                        if (dtPOS.Rows.Count > 0)
                            dt.Rows[i]["PointOfSale"] = dtPOS.Rows[0]["PointOfSaleName"];
                    }
                    catch (Exception exx)
                    {
                        Logger.writeLog(exx);
                    }
                }
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    private void BindDetails()
    {
        DataTable dt = (DataTable)ViewState["SupplierList"];
        if (dt == null)
        {
            dt = new DataTable("Supplier");
            dt.Columns.Add("SupplierID");
            dt.Columns.Add("SupplierName");
            dt.Columns.Add("Deleted");
        }

        gvSupplier.DataSource = dt;
        gvSupplier.DataBind();
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

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserMstscaffold.aspx?id=0");
    }
    protected void btnChangePwd_Click(object sender, EventArgs e)
    {
        ViewState["changePwd"] = true;
        txtCfmPwd.Visible = true;
        ctrlPassword.Visible = true;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            bool tmpBool;
            if (bool.TryParse(e.Row.Cells[7].Text, out tmpBool))
            {
                e.Row.Cells[7].Text = CommonUILib.GetYesOrNoFromBool(tmpBool);
            }

            try
            {
                HyperLink linkPrint = ((HyperLink)e.Row.Cells[0].Controls[3]);
                if (linkPrint != null)
                {
                    string userName = e.Row.Cells[1].Text;
                    UserMst um = new UserMst(userName);
                    string url = string.Format("javascript:poptastic('PrintUserBarcode.aspx?Param={0}');", userName.StringToBinary());
                    linkPrint.Visible = !string.IsNullOrEmpty(um.BarcodeToken);
                    linkPrint.NavigateUrl = url;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
    protected void lnkExport_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid(String.Empty);
            DataTable dt = (DataTable)GridView1.DataSource;
            CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, GridView1);
        }
        catch (Exception ex)
        {
            lblResult.Text = ex.Message;
        }
    }

    protected void btnGenerateToken_Click(object sender, EventArgs e)
    {
        //lblBarcodeToken.Text = Guid.NewGuid().ToString("N");
        lblBarcodeToken.Text = string.Format("{0:X}", DateTime.Now.ToString("yyMMMddHHmmssfff").GetHashCode()).ToUpper();
        btnGenerateToken.Visible = false;
        btnRemoveToken.Visible = true;
    }

    protected void btnRemoveToken_Click(object sender, EventArgs e)
    {
        lblBarcodeToken.Text = "";
        btnRemoveToken.Visible = false;
        btnGenerateToken.Visible = true;
    }

    protected void btnGenerateAllBarcode_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = @"SELECT  UserName
                        FROM	UserMst
                        WHERE	ISNULL(Deleted,0) = 0
		                        AND ISNULL(Userfld7,'') = ''";
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            QueryCommandCollection qmc = new QueryCommandCollection();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string userName = dt.Rows[i]["UserName"] + "";
                UserMst um = new UserMst(userName);
                um.BarcodeToken = string.Format("{0:X}", DateTime.Now.ToString("yyMMMddHHmmssfff").GetHashCode()).ToUpper();
                qmc.Add(um.GetUpdateCommand(Session["UserName"] + ""));
                Thread.Sleep(100);
            }
            DataService.ExecuteTransaction(qmc);
            BindGrid("UserName");
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    protected void btnSearchSupplier_Click(object sender, EventArgs e)
    {
        LoadDropdown(txtSearchSupplier.Text);
    }

    protected void btnAddSupplier_Click(object sender, EventArgs e)
    {
        DataTable dt;
        if (ViewState["SupplierList"] == null)
        {
            dt = new DataTable("Supplier");
            dt.Columns.Add("SupplierID");
            dt.Columns.Add("SupplierName");
            dt.Columns.Add("Deleted");
        }
        else
        {
            dt = (DataTable)ViewState["SupplierList"];
            if (dt == null)
            {
                dt = new DataTable("Supplier");
                dt.Columns.Add("SupplierID");
                dt.Columns.Add("SupplierName");
                dt.Columns.Add("Deleted");
            }
        }

        var suppliers = dt.Select("SupplierID = '" + Regex.Replace(ddlSupplier.SelectedItem.Value.ToString(), @"\'\b", "''") + "'");
        if (suppliers.Length == 0)
        {
            DataRow dr = dt.NewRow();
            dr["SupplierID"] = ddlSupplier.SelectedItem.Value;
            dr["SupplierName"] = ddlSupplier.SelectedItem.Text;
            dr["Deleted"] = false;
            dt.Rows.Add(dr);
            ViewState["SupplierList"] = dt;
            BindDetails();
        }
        else
        {
            suppliers[0]["Deleted"] = false;
            ViewState["SupplierList"] = dt;
            BindDetails();
        }
    }

    protected void gvSupplier_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (ViewState["SupplierList"] == null) return;
        DataTable dt = (DataTable)ViewState["SupplierList"];

        dt.Rows[e.RowIndex]["Deleted"] = true;

        ViewState["SupplierList"] = dt;
        BindDetails();
    }

    protected void gvSupplier_RowDataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvSupplier.Rows)
        {
            Label field = (Label)row.FindControl("lblDeletedGVDetails");

            if (field.Text.ToString().ToLower() == "true")
            {
                row.Cells[0].Controls[0].Visible = false;
                row.BackColor = Color.LightPink;
            }
        }
    }
}


