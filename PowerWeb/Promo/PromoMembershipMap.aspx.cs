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
using PowerPOS.Container;
using SubSonic;

public partial class PromotionMembershipMap : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Fetch ListItems
            PromoCampaignHdrCollection sp = new PromoCampaignHdrCollection();
            sp.Where(PromoCampaignHdr.Columns.DateFrom, Comparison.LessOrEquals, DateTime.Now);
            sp.Where(PromoCampaignHdr.Columns.DateTo, Comparison.GreaterOrEquals, DateTime.Now);
            sp.Where(PromoCampaignHdr.Columns.Enabled, true);
            sp.Load();

            if (sp.Count > 0)
            {
                SubSonic.Utilities.Utility.LoadDropDown(ddPromo, sp, "PromoCampaignName", "PromoCampaignHdrID", "");

                BindGrid();
            }
            else
            {
                btnOk.Enabled = false;
                CommonWebUILib.ShowMessage(lblErrorMsg, "There is no valid promotion. To create event, click <a href='EventScaffold.aspx'>here</a>", CommonWebUILib.MessageType.BadNews);
            }
            
        }
    }
    
    #region "Detail GridView Events"
    private void BindGrid()
    {

        if (ddPromo.SelectedValue != "")
        {
            DataTable dt = PromotionAdminController.
                FetchPromoMembershipMap(DateTime.Today, 
                "", "", 
                int.Parse(ddPromo.SelectedValue.ToString()));
            
            gvDetails.DataSource = dt;                
            gvDetails.DataBind();
        }
        else
        {
            gvDetails.DataSource = null;
            gvDetails.DataBind();
        }
    }
    protected void gvDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvDetails.EditIndex = e.NewEditIndex;
        BindGrid();
        //btnSubmit.Enabled = false;


        gvDetails.Rows[gvDetails.EditIndex].Cells[3].FindControl("txtPrice").Focus();

    }

    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PromoMembershipMap.Destroy(PromoMembershipMap.Columns.PromoMembershipID,
                                new Guid(gvDetails.DataKeys[e.RowIndex].Value.ToString()));
        
        BindGrid();
        e.Cancel = true;
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {   
        //PromoMembershipMap.Update(new Guid(gvDetails.DataKeys[e.RowIndex].Value.ToString()),  
        /*
        PromotionAdminController.AddPromoMembershipMap(int.Parse(ddPromo.SelectedValue),
                    new MembershipGroup(MembershipGroup.Columns.GroupName,gvDetails.Rows[gvDetails.EditIndex].Cells[2].Text).MembershipGroupId,
                    decimal.Parse(((TextBox)gvDetails.Rows[gvDetails.EditIndex].Cells[3].FindControl("txtPrice")).Text)
                    , out status);
        */
            
        gvDetails.EditIndex = -1;
        BindGrid();
    }
    protected void gvDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindGrid();
        //btnSubmit.Enabled = true;
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Decimal amt, disc;
        bool UsePrice;

        if (e.Row.RowType == DataControlRowType.DataRow && gvDetails.EditIndex == -1)
        {
            amt = decimal.Parse(((Label)e.Row.Cells[3].FindControl("lblPrice")).Text);
            ((Label)e.Row.Cells[3].FindControl("lblPrice")).Text = String.Format("{0:N2}", amt);

            disc = decimal.Parse(((Label)e.Row.Cells[4].FindControl("lblDisc")).Text);
            ((Label)e.Row.Cells[4].FindControl("lblDisc")).Text = disc.ToString("N2") + "%";

            if (bool.TryParse(e.Row.Cells[2].Text, out UsePrice))
            {
                if (UsePrice)
                {
                    e.Row.Cells[2].Text = "Price";
                }
                else
                {
                    e.Row.Cells[2].Text = "Discount";
                }
            }            
        }
    }
    #endregion


    protected void btnOk_Click(object sender, EventArgs e)
    {
        bool useMembership = rbUsePrice.Checked;
        if (ddlMembershipGroup.SelectedValue.ToString() == "")
        {
            CommonWebUILib.ShowMessage
                (lblErrorMsg, 
                 "Please specify the membership group", 
                 CommonWebUILib.MessageType.BadNews);
            return;
        }
        if (txtMembershipPrice.Text == "")
        {
            if (rbUsePrice.Checked)
            {
                CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the membership price", CommonWebUILib.MessageType.BadNews);
                return;
            }
            else
            {
                txtMembershipPrice.Text = "0";
            }
        }
        
       
        if (txtPromoDiscount.Text == "")
        {
            if (rbUseDisc.Checked)
            {
                CommonWebUILib.ShowMessage(lblErrorMsg, "Please specify the membership discount", CommonWebUILib.MessageType.BadNews);
                return;
            }
            else
            {
                txtPromoDiscount.Text = "0";
            }
        }
        PromotionAdminController.AddPromoMembershipMap
            (int.Parse(ddPromo.SelectedValue.ToString()),
            int.Parse(ddlMembershipGroup.SelectedValue.ToString()),
            decimal.Parse(txtMembershipPrice.Text), 
            decimal.Parse(txtPromoDiscount.Text),
            useMembership);
        BindGrid();
         
    }
    
    protected void ddlPromo_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindGrid();
    }
}
