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

public partial class MultitierDiscount: PageBase
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
        if ( PromotionAdminController.InsertMultiTierDiscount(
            txtCampaignName.Text, startDate, endDate,
            ddsItemNo.SelectedValue, ((DataTable)ViewState["DiscountTierList"]), ForNonMembersAlso))
        {
            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Campaign " + txtCampaignName.Text + " has been created successfully";
            gvReport.DataBind();
            ClearControls();
        }
        else
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Campaign " + txtCampaignName.Text + " cant be created. Please try again. If the problem persist, contact your administrator.";
        }
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {        
        DataTable dt;
        if (ViewState["DiscountTierList"] == null)
        {
            dt = new DataTable("Discount Tier");
            dt.Columns.Add("TresholdQty");
            dt.Columns.Add("Discount");            
        }
        else
        {
            dt = (DataTable)ViewState["DiscountTierList"];
            if (dt == null)
            {
                dt = new DataTable("Discount Tier");
                dt.Columns.Add("TresholdQty");
                dt.Columns.Add("Discount");
            }
        }

        if (dt.Select("TresholdQty = '" + int.Parse(txtThreshold.Text) + "'").Length == 0)
        {
            DataRow dr = dt.NewRow();
            dr["TresholdQty"] = int.Parse(txtThreshold.Text);
            dr["Discount"] = decimal.Parse(txtDiscount.Text);            
            dt.Rows.Add(dr);
            ViewState["DiscountTierList"] = dt;
            gvItem.DataSource = dt;
            gvItem.DataBind();
        }                        
    }

    protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
    }
    protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["DiscountTierList"];
        if (dt == null)
        {
            dt = new DataTable("Discount Tier");
            dt.Columns.Add("TresholdQty");
            dt.Columns.Add("Discount");
        }
        DataRow[] dr = dt.Select("TresholdQty = " + gvItem.Rows[e.RowIndex].Cells[1].Text);
        for (int i=0;i <dr.Length;i++) 
            dt.Rows.Remove(dr[i]);
        gvItem.DataSource = dt;
        gvItem.DataBind();
    }
    protected void ddItemGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        //
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearControls();
    }

    private void ClearControls()
    {
        txtThreshold.Text = "";
        txtCampaignName.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtDiscount.Text = "0";
        txtMinQty.Text = "0";       
        ViewState["DiscountTierList"] = null;
        gvItem.DataSource = null;
        gvItem.DataBind();
    }
}
