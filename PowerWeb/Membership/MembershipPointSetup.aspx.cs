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
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using PowerPOS.Container;

public partial class MembershipPointSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        qm0.InnerHtml = "<li><a class=\"qmparent\" runat=\"server\" href=\"MembershipPointSetup.aspx\">Profit And Loss Report</a></li>";
    }
    protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
    }

    protected void gvReport_DataBound(Object sender, EventArgs e)
    {
    }
    protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
    {
    }
    protected void ddlPages_SelectedIndexChanged(Object sender, EventArgs e)
    {
    }
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void AjaxBullet_Load(object sender, EventArgs e)
    {
        //AjaxBullet.
    }
}
