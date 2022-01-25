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

public partial class ItemGroupPriceDiscount: PageBase
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
        string status;
        string ItemGrpName;
        decimal mPrice, mDiscount;
        if (!PromotionAdminController.IsCampaignNameNotUsed(txtCampaignName.Text))
        {
            CommonWebUILib.ShowMessage(lblMsg, "Campaign Name has been used before. Please choose another name", CommonWebUILib.MessageType.BadNews);
            return;
        }

        if (!decimal.TryParse(txtPromoAmount.Text, out mPrice) | (!decimal.TryParse(txtPromoDiscount.Text,out mDiscount))) 
        {
            CommonWebUILib.ShowMessage(lblMsg, "Please specify numeric for price and discount.", CommonWebUILib.MessageType.BadNews);
            return;
        }

        if (rbNew.Checked)
        {
            DataTable dt;
            dt = (DataTable)ViewState["ItemList"];
            if (!ItemGroupController.InsertItemGroup(txtGroupName.Text, dt, txtBarcode.Text, out status))
            {
                CommonWebUILib.ShowMessage(lblMsg, status, CommonWebUILib.MessageType.BadNews);
                return;
            }
            ItemGrpName = txtGroupName.Text;
        }
        else
        {
            ItemGrpName = ddItemGroup.SelectedItem.Text;
        }

        if (rbDiscount.Checked)
            mPrice = 0;
        else
            mDiscount = 0;

        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text).AddSeconds(86399);
        bool ForNonMembersAlso;
        if (rbBoth.Checked)
            ForNonMembersAlso = true;
        else
            ForNonMembersAlso = false;           
        if ( PromotionAdminController.InsertItemGroupPriceDiscount(
            txtCampaignName.Text, 
            startDate, 
            endDate,
            ItemGrpName,
            mPrice, 
            (double)mDiscount, 
            ForNonMembersAlso))
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
        rbNew.Checked = true;
        DataTable dt;
        if (ViewState["ItemList"] == null)
        {
            dt = new DataTable("Item");
            dt.Columns.Add("ItemNo");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("UnitQty");
            dt.Columns.Add("Price");
        }
        else
        {
            dt = (DataTable)ViewState["ItemList"];
            if (dt == null)
            {
                dt = new DataTable("Item");
                dt.Columns.Add("ItemNo");
                dt.Columns.Add("ItemName");
                dt.Columns.Add("UnitQty");
                dt.Columns.Add("Price");
            }
        }
        if (dt.Select("ItemName = '" + ddsItem.SelectedItem.Text + "'").Length == 0)
        {
            DataRow dr = dt.NewRow();
            dr["ItemNo"] = ddsItem.SelectedValue;
            dr["ItemName"] = ddsItem.SelectedItem.Text;
            dr["UnitQty"] = txtUnitQty.Text;
            dr["Price"] = new Item(ddsItem.SelectedValue.ToString()).RetailPrice.ToString("N2");
            dt.Rows.Add(dr);
            ViewState["ItemList"] = dt;
            gvItem.DataSource = dt;
            gvItem.DataBind();
        }                        
    }

    protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //Do Delete....        
    }
    protected void gvItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {        
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
        txtGroupName.Text = "";
        txtCampaignName.Text = "";
        txtStartDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtEndDate.Text = DateTime.Today.ToString("dd MMM yyyy");
        txtUnitQty.Text = "0";
        txtPromoAmount.Text = "0";
        txtPromoDiscount.Text = "0";
        ViewState["ItemList"] = null;        
        gvItem.DataSource = null; gvItem.DataBind();
    }
    protected void odsView_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {

    }
}
