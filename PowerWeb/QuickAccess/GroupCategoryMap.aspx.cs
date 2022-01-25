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
using SubSonic;
using SubSonic.Utilities;

public partial class GroupCategoryMap : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ddlName0_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        QuickAccessController.AddCategoryGroupMap(new Guid(ddlQuickAccessGroup.SelectedValue.ToString()),
            new Guid(ddlCategory.SelectedValue.ToString()));
        BindGrid();
    }
    private void BindGrid()
    {
        if (ddlQuickAccessGroup.SelectedValue != "")
        {
            gvDetails.DataSource =
                QuickAccessController.FetchCategories
                (null, new Guid(ddlQuickAccessGroup.SelectedValue.ToString()));
            gvDetails.DataBind();
        }
    }
    protected void gvDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (gvDetails.Rows[e.RowIndex].RowType == DataControlRowType.DataRow)
        {
            QuickAccessGroupMapController q = new QuickAccessGroupMapController();
            q.Delete(new Guid(gvDetails.DataKeys[e.RowIndex].Value.ToString()));
            BindGrid();
            e.Cancel = true;
            
        }
    }
    protected void gvDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
