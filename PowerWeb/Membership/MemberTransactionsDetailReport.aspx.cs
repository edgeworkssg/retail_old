using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;

public partial class MemberTransactionsDetailReport : System.Web.UI.Page
{
    protected string ReceiptNo = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string ID = Request.QueryString["id"];
            OrderHdr oh = new OrderHdr(ID);
            Membership member = new Membership(oh.MembershipNo);
            ReceiptNo = oh.Userfld5;
            lblMembership.Text = string.Format("{0} ({1}) Invoice No {2}", member.NameToAppear, member.MembershipNo, oh.Userfld5);
            BindGrid();
        }
    }
    protected void BindGrid()
    { 
        string ID =  Request.QueryString["id"];
        //DataTable dt = ReportController.FetchMemberTransactions(ID);
        DataTable dt = ReportController.FetchAccountStatementHistory(ID);

        gvTransactions.DataSource = dt;
        gvTransactions.DataBind();
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid();
        DataTable dt = (DataTable)gvTransactions.DataSource;
        CommonWebUILib.ExportCSV(dt, this.Page.Title.Trim(' '), this.Page.Title, gvTransactions);
    }
}

