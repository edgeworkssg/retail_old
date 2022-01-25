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
using PowerPOS;
using SubSonic.Utilities;
public partial class MembershipDetail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string status;
        if (Session["UserName"] == null || Session["Role"] == null || (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }
        if (Request.QueryString["id"] != null)
        {
            string id = Utility.GetParameter("id");
            try
            {

                PowerPOS.Membership member = new PowerPOS.Membership(PowerPOS.Membership.Columns.MembershipNo, id);
                ctrlMembershipNo.Text = member.MembershipNo;
                ctrlGroupName.Text = member.MembershipGroup.GroupName;
                ctrlExpiryDate.Text = member.ExpiryDate.Value.ToString("dd MMM yyyy");
                ctrlRemarks.Text = member.Remarks;                
                ctrlNRIC.Text = member.Nric;
                ctrlFirstName.Text = member.FirstName;
                ctrlLastName.Text = member.LastName;
                ctrlNameToAppear.Text = member.NameToAppear;                
                ctrlGender.Text = member.Gender;
                ctrlDateOfBirth.Text = member.DateOfBirth.Value.ToString("dd MMM yyyy");
                ctrlOccupation.Text = member.Occupation;
                ctrlStreetName.Text = member.StreetName;
                ctrlStreetName2.Text = member.StreetName2;
                ctrlCountry.Text = member.Country;
                ctrlZipCode.Text = member.ZipCode;
                ctrlCity.Text = member.City;
                ctrlMobile.Text = member.Mobile;
                ctrlOffice.Text = member.Office;
                ctrlHome.Text = member.Home;
                ctrlEmail.Text = member.Email;
                //CUSTOMER RELATED INFORMATION
                if (member.IsVitaMix.HasValue)
                {
                    cbIsVitaMix.Checked = member.IsVitaMix.Value;
                }
                if (member.IsJuicePlus.HasValue)
                {
                    cbIsJuicePlus.Checked = member.IsJuicePlus.Value;
                }
                if (member.IsWaterFilter.HasValue)
                {
                    cbIsWaterFilter.Checked = member.IsWaterFilter.Value;
                }
                if (member.IsYoung.HasValue)
                {
                    cbIsYoung.Checked = member.IsYoung.Value;
                }
                ctrlRemarks.Text = member.Remarks;   
            }
            catch (Exception ex)
            {               
                Logger.writeLog(ex);
            }
        }
    }
    protected void ctrlMembershipNo_TextChanged(object sender, EventArgs e)
    {

    }
}
