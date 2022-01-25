using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using SubSonic.Utilities;

namespace PowerWeb.Support
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                UserMst user = new UserMst(Session["UserName"]);
                lblDisplayName.Text = user.DisplayName;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                BindAndSave();
                lblResult.Text = "<span style=\"font-weight:bold; color:#22bb22\">" + LanguageManager.GetTranslation("Change Password succeed.") + "</span>";
                
                //logout
                AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", "Logout", "");
                Session.Abandon();
                Session.Contents.RemoveAll();
                System.Web.Security.FormsAuthentication.SignOut();
                Response.Redirect("../Login.aspx", false);
            }

            catch (Exception x)
            {
                //haveError = true;
                lblResult.Text = "<span style=\"font-weight:bold; color:#990000\">" + LanguageManager.GetTranslation("Change Password failed:") + "</span> " + x.Message;
            }

            //if(!haveError)
            //  Response.Redirect(Request.CurrentExecutionFilePath);
        }

        void BindAndSave()
        {
            UserMst user = new UserMst(Session["UserName"]);
            object valctrlPassword = Utility.GetDefaultControlValue(UserMst.Schema.GetColumn("Password"), txtOldPassword, false, false);
            string oldpassword = PowerPOS.UserController.DecryptData(Convert.ToString(user.Password)); 

            if(oldpassword != Convert.ToString(valctrlPassword))
            {
                throw new Exception("The old password doesn't match");
            }

            var valnewpassword = Utility.GetDefaultControlValue(UserMst.Schema.GetColumn("Password"), txtNewPassword, false, false);

            if ( Convert.ToString(valctrlPassword) ==  Convert.ToString(valnewpassword))
            {
                throw new Exception("The old password must not be same as the new one.");
            }
            
            user.Password = PowerPOS.UserController.EncryptData(Convert.ToString(valnewpassword));
 
            user.Save(Session["UserName"].ToString());
        }
    }
}
