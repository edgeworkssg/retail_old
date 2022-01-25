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
public partial class PromoDetail : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string status;
        /*
        if (Session["UserName"] == null || Session["Role"] == null || (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }*/
        if (!Page.IsPostBack) 
        {
            if (Request.QueryString["id"] != null)
            {
                string id = Utility.GetParameter("id");

                //Hide in hidden fields
                hdfID.Value = id;

                PromoCampaignHdr hdr = new PromoCampaignHdr(id);
                if (hdr.IsLoaded)
                {
                    lblCampaignName.Text = hdr.PromoCampaignName;
                    lblCampaignType.Text = hdr.CampaignType;
                    if (hdr.ForNonMembersAlso.Value)
                    {
                        rbNo.Checked = true;
                    }
                    else
                    {
                        rbYes.Checked = true;
                    }
                    txtStartDate.Text = hdr.DateFrom.ToString("dd MMM yyyy");
                    txtEndDate.Text = hdr.DateTo.ToString("dd MMM yyyy");
                    btnSave.Enabled = true;
                }
            }
        }
    }
    protected void gvOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Text = String.Format("{0:N2}", decimal.Parse(e.Row.Cells[3].Text));
            e.Row.Cells[6].Text = String.Format("{0:N2}", decimal.Parse(e.Row.Cells[6].Text));
            for (int i = 7; i < 11; i++)
            {
                if (bool.Parse(e.Row.Cells[i].Text))
                {
                    e.Row.Cells[i].Text = "Yes";
                }
                else
                {
                    e.Row.Cells[i].Text = "No";
                }
            }
        }
    }
    protected void gvReceipt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[1].Text = String.Format("{0:N2}", decimal.Parse(e.Row.Cells[1].Text));
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //Save the changes
        if (PromotionAdminController.PromoChangePeriod(int.Parse(hdfID.Value.ToString()), DateTime.Parse(txtStartDate.Text), DateTime.Parse(txtEndDate.Text))
         & PromotionAdminController.PromoChangeForNonMembersAlso(int.Parse(hdfID.Value.ToString()), rbNo.Checked)) {

         }
        
    }
}
