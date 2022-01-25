using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PowerPOS;

namespace PowerWeb
{
    public partial class customerLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string membershipNo = txtCardNo.Text;
            PowerPOS.Membership p = new PowerPOS.Membership(membershipNo);
            if (p.IsLoaded && !p.IsNew)
            {
                if (p.Nric != txtNRIC.Text)
                {
                    CommonWebUILib.ShowMessage(lblMsg, "Invalid Card No and NRIC. Please enter the correct the information.", CommonWebUILib.MessageType.BadNews);
                    return;
                }                
                Session["Member"] = p;
                Response.Redirect("redemptionPage.aspx");
            }
            else
            {
                CommonWebUILib.ShowMessage(lblMsg, "Invalid Card No and NRIC. Please enter the correct the information.", CommonWebUILib.MessageType.BadNews);
                return;
            }            
        }
    }
}
