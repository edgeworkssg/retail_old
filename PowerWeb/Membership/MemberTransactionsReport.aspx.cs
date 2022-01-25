using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;

public partial class MemberTransactionsReport : System.Web.UI.Page
{
    protected string MembershipNo = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string ID = Request.QueryString["id"];
            Membership member = new Membership(ID);
            hMembershipNo.Value = member.MembershipNo;
            MembershipNo = member.MembershipNo;
            lblMembership.Text = string.Format("{0} ({1})", member.NameToAppear, member.MembershipNo);
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            ReportController.GetRangeDateInstallmentByMember(MembershipNo, out StartDate, out EndDate);

            txtStartDate.Text = StartDate.ToString("dd MMM yyyy");
            txtEndDate.Text = EndDate.ToString("dd MMM yyyy");

            BindGrid();
        }
    }
    protected void BindGrid()
    { 
        string ID =  Request.QueryString["id"];
        //DataTable dt = ReportController.FetchMemberTransactions(ID);
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
        DataTable dt = ReportController.FetchAccountStatementByMember(ID, cbShowPaidInvoices.Checked, startDate, endDate);

        gvTransactions.DataSource = dt;
        gvTransactions.DataBind();
    }

    protected void cbShowPaidInvoices_OnCheckedChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void gvTransactions_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label field = (Label)gvTransactions.SelectedRow.FindControl("lblOrderHdrID");
        string script = "<script>window.open('MemberTransactionsDetailReport.aspx?id=" + field.Text + "',null,'left=0," + " top=0,height=600, width=900, status=no, resizable= no, scrollbars= no," + "toolbar= no,location= no, menubar= no');</script>";
        if (!ClientScript.IsStartupScriptRegistered("popUp"))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "popUp", script);
        }
        //LinkButton button = (LinkButton)gvReport.SelectedRow.FindControl("View");
        //button.Attributes.Add("OnClientClick", "window.open('WebForm2.aspx','','height=600,width=600');return false");
        // Response.Redirect("javascript:void();");
        //gvReport.DataBind();
        BindGrid();
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvTransactions.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvTransactions);
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        DateTime startDate = DateTime.Parse(txtStartDate.Text);
        DateTime endDate = DateTime.Parse(txtEndDate.Text);
        endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);
        string root = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
        string script = "<script>window.open('" + root + "CRReport/CRReport.aspx?r=MembershipAccountStatement.rpt&Membershipno=" + hMembershipNo.Value + "&StartDateWithTime=" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "&EndDateWithTime=" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "&showfilter=false&HideTopBannerMenu=true',null,'left=0," + " top=0,height=600, width=900, status=no, resizable= no, scrollbars= no," + "toolbar= no,location= no, menubar= no');</script>";
        if (!ClientScript.IsStartupScriptRegistered("popUp"))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "popUp", script);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();        
    }
}

