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

public partial class PromotionLocationMap : PageBase
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
                CommonWebUILib.ShowMessage(lblErrorMsg, "Please create event first. To create event, click <a href='EventScaffold.aspx'>here</a>", CommonWebUILib.MessageType.BadNews);
            }
            
        }
    }
    
    #region "Detail GridView Events"
    private void BindGrid()
    {

        if (ddPromo.SelectedValue != "")
        {
            DataTable dt = PromotionAdminController.
                FetchPromoLocationMap(DateTime.Today, 
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

    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PromoLocationMap.Destroy(PromoLocationMap.Columns.PromoLocationMapID,
                                new Guid(gvDetails.DataKeys[e.RowIndex].Value.ToString()));
        
        BindGrid();
        e.Cancel = true;
    }
    protected void gvDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {             
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

    }
    #endregion


    protected void btnOk_Click(object sender, EventArgs e)
    {
        PromotionAdminController.
            AddPromoLocationMap(int.Parse(ddPromo.SelectedValue.ToString()), 
            int.Parse(ddlName.SelectedValue.ToString()));
        BindGrid();
    }
    
    protected void ddlPromo_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvDetails.EditIndex = -1;
        BindGrid();
    }
}
