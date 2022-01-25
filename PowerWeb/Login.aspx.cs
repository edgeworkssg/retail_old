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
using PowerPOS.Container;

using PowerPOS;
using System.Globalization;

public partial class Login : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //
        Session["Company"] = null;
        Session["UserName"] = null;
        Session["Role"] = null;
        Session["privileges"] = null;
        Session["DeptID"] = null;
        if (!Page.IsPostBack)
        {
            foreach (CultureInfo culture in LanguageManager.AvailableCultures)
            {
                ddlLanguages.Items.Add(new System.Web.UI.WebControls.ListItem(culture.NativeName, culture.Name));
            }
            ddlLanguages.SelectedValue = LanguageManager.CurrentCulture.Name;
        }
    }
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string myRole, status, myDept;
        UserMst user;
        PointOfSaleInfo.PointOfSaleID = (int)Application["PointOfSaleID"]; //Fixed for Web Module
        if (UserController.login(UserName.Text, Password.Text, LoginType.Login, out user,
            out myRole, out myDept,out status))
        {
            Session["UserName"] = UserName.Text;
            Session["Role"] = myRole;
            Session["DeptID"] = myDept;
            DataTable dt = UserController.FetchGroupPrivilegesWithUsername(myRole, UserName.Text);
            //DataTable dt = UserController.FetchGroupPrivileges(myRole);
            dt.CaseSensitive = false;
            Session["privileges"] = dt;
            CompanyCollection theCompany = new CompanyCollection();
            theCompany.Load();
            Session["Company"] = theCompany[0].CompanyName;

            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"]+"", "-", "Login", "");
            #region Create Table if not exist
            LineInfoController.CreateLineInfoTable();
            PriceSchemeController.CreatePriceSchemeTable();
            MembershipController.CheckAttachedParticularTable();
            #endregion

            //Response.Redirect("Dashboard/Dashboard.aspx");
            Response.Redirect("home/home.aspx");

          
            
        }
        else
        {
            FailureText.Text = status;
        }                
    }
    protected void ddlLanguages_SelectedIndexChanged(object sender, EventArgs e)
    {
        ApplyNewLanguageAndRefreshPage(new CultureInfo(ddlLanguages.SelectedValue));
    }
}
