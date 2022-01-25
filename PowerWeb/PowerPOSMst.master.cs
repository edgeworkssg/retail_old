using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS;
using PowerPOS.Container;
using System.Globalization;
using SubSonic;
using System.Diagnostics;

public partial class PowerPOSMst : System.Web.UI.MasterPage
{
    private const bool ENABLE_LOGIN = true;
    private const string SESSION_KEY_LANGUAGE = "CURRENT_LANGUAGE";

    protected void Page_Init(object sender, EventArgs e)
    {        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblVersion.Text = UtilityController.GetCurrentVersionMajorMinor();
            lblRevision.Text = UtilityController.GetCurrentVersion();
        }
        catch (Exception ex)
        {

        }
        try
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();
            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                mnUndeductedSalesReport.Visible = true;
            else
                mnUndeductedSalesReport.Visible = false;
        }
        catch (Exception ex)
        {
 
        }
        if (!Page.IsPostBack)
        {

            foreach (CultureInfo culture in LanguageManager.AvailableCultures)
            {
                ddlLanguages.Items.Add(new System.Web.UI.WebControls.ListItem(culture.NativeName, culture.Name));
            }
            ddlLanguages.SelectedValue = LanguageManager.CurrentCulture.Name;
        }
        /*Session["UserName"] = "admin";
        UserInfo.username = "admin";*/
        if (ENABLE_LOGIN)
        {
            //check login token
            if (Page.Request.QueryString["ut"] != null)
            {
                string myRole, status, myDept;
                UserMst user;

                if (UserController.loginWithToken(Page.Request.QueryString["ut"], LoginType.Login, out user,
                out myRole, out myDept, out status))
                {
                    Session["UserName"] = user.UserName;
                    Session["Role"] = myRole;
                    Session["DeptID"] = myDept;
                    DataTable dt = UserController.FetchGroupPrivilegesWithUsername(myRole, Session["UserName"].ToString());
                    dt.CaseSensitive = false;
                    Session["privileges"] = dt;
                    CompanyCollection theCompany = new CompanyCollection();
                    theCompany.Load();
                    Session["Company"] = theCompany[0].CompanyName;

                    AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "Login", "");
                    #region Create Table if not exist
                    LineInfoController.CreateLineInfoTable();
                    PriceSchemeController.CreatePriceSchemeTable();
                    MembershipController.CheckAttachedParticularTable();
                    #endregion

                }
            }

            if (Session["UserName"] == null || Session["Role"] == null || (string)Session["UserName"] == "" || (string)Session["Role"] == "")
            {
                Response.Redirect("../login.aspx");
            }
            lblCompany.Text =  (string)Session["Company"];
            lblUserName.Text = (string)Session["UserName"];
            if (((string)Session["UserName"]).ToLower() != "edgeworks" && ((string)Session["Role"]).ToLower() != "admin")
            {
                string tmp = Page.Request.FilePath;
                tmp = tmp.Substring(tmp.LastIndexOf('/') + 1);

                if (tmp == "CRReport.aspx")
                {
                    tmp = Page.Request.RawUrl;
                    tmp = tmp.Substring(tmp.LastIndexOf('/') + 1);
                }
                
                if (tmp != "NotAuthorized.aspx" && tmp != "home.aspx" && tmp != "ChangePassword.aspx")
                {
                    
                    if (!PrivilegesController.IsPageInPrivilege(tmp, (DataTable)Session["privileges"]))
                    {
                        if (tmp != "Dashboard.aspx")
                        {
                            Response.Redirect("../Misc/NotAuthorized.aspx");
                        }
                        else
                        {
                            Response.Redirect("../home/home.aspx");
                        }
                    }
                }
            }
        }
        RegisterJQueryScript();
        //RegisterPrivilegeScript(GetPrivilegeList());
        SetMenuPrivileges(GetPrivilegeData());
        //SetFormSetting();
    }

    private void SetMenuPrivileges(List<string> privileges)
    {
        try
        {
            var ctrl = tdMainMenu.Controls;
            for (int i = 0; i < ctrl.Count; i++)
            {
                if (ctrl[i] is HtmlAnchor)
                {
                    var lnk = (HtmlAnchor)ctrl[i];
                    string text = lnk.InnerText;
                    if (!string.IsNullOrEmpty(lnk.HRef) && !(lnk.HRef + "").Trim().ToLower().StartsWith("javascript:void(0)"))
                    {

                        bool isEng = true;

                        try
                        {
                            string name = CultureInfo.CurrentUICulture.Name;
                            isEng = name.Equals("en-US");
                        }
                        catch (Exception exxx)
                        {
                            Logger.writeLog(exxx);
                        }

                        //Adi Remove the validation for is Eng
                        //if (isEng)
                        //{
                            lnk.Visible = privileges.Contains(text.Trim().ToUpper());
                        //}
                        //InsertPrivileges(lnk.HRef, text);
                    }
                    if ((lnk.HRef + "").Trim().ToLower().StartsWith("javascript:void(0)"))
                        lnk.InnerText = LanguageManager.GetTranslation(lnk.InnerText).ToUpper();
                    else
                        lnk.InnerText = LanguageManager.GetTranslation(lnk.InnerText);
                    if (!lnk.Visible)
                        Debug.WriteLine(">>> " + text);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    private void InsertPrivileges(string url, string name)
    {
        try
        {
            string fileName = "";
            if (url.ToLower().EndsWith(".aspx"))
            {
                Uri uri = new Uri(Server.MapPath(url));
                fileName = System.IO.Path.GetFileName(uri.LocalPath);
                Debug.WriteLine(string.Format("{0} [{1}]", url, name));
            }
            else if (url.ToLower().EndsWith(".rpt"))
            {
                int startIndex = url.LastIndexOf("=");
                if (startIndex >= 0)
                {
                    int lenght = url.Length - startIndex;
                    fileName = url.Substring(startIndex, lenght).Replace("=", "");
                }
            }
            if (!string.IsNullOrEmpty(fileName))
            {
                string sql = @"IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = '{0}')<=0)
                        BEGIN 
                        INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
                        ModifiedBy, ModifiedOn, Deleted) 
                        VALUES ('{0}','{1}','AUTO-UPGRADE',GETDATE(),'AUTO-UPGRADE',GETDATE(),0) 
                        END";
                sql = string.Format(sql, name, fileName);
                DataService.ExecuteQuery(new QueryCommand(sql));
            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
    }

    private void RegisterJQueryScript()
    {
        ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "jquery1", ResolveClientUrl("~/Scripts/jquery-1.8.3.min.js"));
    }

    private void RegisterPrivilegeScript(List<string> privileges)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("$(document).ready(function () {");
        sb.Append(string.Format("var privileges = [{0}];", string.Join(",", privileges.ToArray())));
        sb.Append("    $('#qm0').each(function (i, list) {");
        sb.Append("        $(list).find('div a').each(function () {");
        sb.Append("            var current = $(this);");
        sb.Append("            if ($.inArray(current.text(), privileges) < 0) { current.hide(); }");
        sb.Append("        });");
        sb.Append("    });");
        sb.Append("    $('#qm0').each(function (i, list) {");
        sb.Append("        $(list).find('.qmparent').each(function (i2, list2) {");
        sb.Append("            var parent = $(list2).parent();");
        sb.Append("            $(parent).find('div').each(function (i3, list3) {");
        sb.Append("                var count = 0;");
        sb.Append("                var visibleLinkCount = 0;");
        sb.Append("                var lastElement = '';");
        sb.Append("                $(list3).children().each(function (i4, list4) {");
        sb.Append("                    var current = $(list4);");
        sb.Append("                    if (current.is(':visible')) {");
        sb.Append("                        if (current.prop('tagName') == 'SPAN') {");
        sb.Append("                            if (lastElement == 'SPAN' && count == 0) {");
        sb.Append("                                current.hide();");
        sb.Append("                            }");
        sb.Append("                            count = 0;");
        sb.Append("                        }");
        sb.Append("                        else if (current.prop('tagName') == 'A') {");
        sb.Append("                            count++;");
        sb.Append("                            visibleLinkCount++;");
        sb.Append("                        }");
        sb.Append("                        lastElement = current.prop('tagName');");
        sb.Append("                    }");
        sb.Append("                });");
        sb.Append("                if (visibleLinkCount == 0) {");
        sb.Append("                    $(list3).hide();");
        sb.Append("                }");
        sb.Append("            });");
        sb.Append("        });");
        sb.Append("    });");
        sb.Append("});");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "script1", sb.ToString(), true);
    }

    private List<string> GetPrivilegeData()
    {
        var privileges = new List<string>();
        var table = (DataTable)Session["privileges"];
        if (table != null)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row["privilegename"] != DBNull.Value)
                {
                    privileges.Add(row["privilegename"].ToString().Trim().ToUpper());
                }
            }
        }

        return privileges;
    }

    private List<string> GetPrivilegeList()
    {
        var privileges = new List<string>();
        var table = (DataTable)Session["privileges"];
        if (table != null)
        {
            foreach (DataRow row in table.Rows)
            {
                if (row["privilegename"] != DBNull.Value)
                {
                    privileges.Add(string.Format("\"{0}\"", row["privilegename"].ToString().Trim()));
                }
            }
        }

        try
        {
            string posText = LabelController.PointOfSaleText;
            string outletText = LabelController.OutletText;

            if (privileges.Contains(string.Format("\"{0}\"","Outlet Setup")))
                privileges.Add(string.Format("\"{0}\"",(outletText + " Setup")));
            
            if (privileges.Contains(string.Format("\"{0}\"","Outlet Group")))
                privileges.Add(string.Format("\"{0}\"",(outletText + " Group")));
            
            if (privileges.Contains(string.Format("\"{0}\"","Point Of Sale Setup")))
                privileges.Add(string.Format("\"{0}\"",(posText + " Setup")));

            if (privileges.Contains(string.Format("\"{0}\"", "Outlet Transaction Hourly Report")))
                privileges.Add(string.Format("\"{0}\"", (outletText + " Transaction Hourly Report")));

            if (privileges.Contains(string.Format("\"{0}\"", "Outlet Daily Sales")))
                privileges.Add(string.Format("\"{0}\"", (outletText + " Daily Sales")));
        }
        catch (Exception ex)
        {
            //Logger.writeLog(ex);
        }


        return privileges;
    }

    protected void LogoutButton_Click(object sender, EventArgs e)
    {
        AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "Logout", "");

        Session.Abandon();

        Session.Contents.RemoveAll();

        System.Web.Security.FormsAuthentication.SignOut();

        Response.Redirect("../Login.aspx");
    }

    protected void ddlLanguages_SelectedIndexChanged(object sender, EventArgs e)
    {
        //store requested language as new culture in the session
        Session[SESSION_KEY_LANGUAGE] = new CultureInfo(ddlLanguages.SelectedValue);

        //reload last requested page with new culture
        Server.Transfer(Request.Path);
    }

    private void SetFormSetting()
    {
        try
        {
            //string posText = LabelController.PointOfSaleText;
            //string outletText = LabelController.OutletText;
            //lnkOutlet.InnerText = outletText + " Setup";
            //lnkOutletgroup.InnerText = outletText + " Group";
            //lnkPointofSale.InnerText = posText + " Setup";
            //lnkOutletTransactionHourlyReport.InnerText = outletText + " Transaction Hourly Report";
            //lnkOutletDailySales.InnerText = outletText + " Daily Sales";
        }
        catch (Exception ex)
        {
            //Logger.writeLog(ex);
        }
    }
}
