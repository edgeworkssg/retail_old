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

public partial class Buy1Get1Free : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
            ViewStartDate.Value = "01 Jan 1986";
            ViewEndDate.Value = DateTime.Now.ToString("dd MMM yyyy");
        }
        gvReport.DataBind();        
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (!PromotionAdminController.IsCampaignNameNotUsed(txtCampaignName.Text))
        {
            CommonWebUILib.ShowMessage(lblMsg, "Campaign Name has been used before. Please choose another name", CommonWebUILib.MessageType.BadNews);
            return;
        }
        bool ForNonMembersAlso;
        if (rbBoth.Checked)
            ForNonMembersAlso = true;
        else
            ForNonMembersAlso = false;
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text).AddSeconds(86399);
        if (PromotionAdminController.InsertBuyXGetYFree(
            txtCampaignName.Text,
            startDate, endDate, 
            ddsItem.SelectedValue,
            int.Parse(txtUnitQty.Text), ddsFreeItem.SelectedValue, 
            int.Parse(txtFreeQty.Text),ForNonMembersAlso))
        {
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Campaign " + txtCampaignName.Text + " has been created successfully";
            gvReport.DataBind();
        }
        else
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Campaign " + txtCampaignName.Text + " cant be created. Please try again. If the problem persist, contact your administrator.";
        }
    }
}
