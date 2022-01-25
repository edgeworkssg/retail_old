using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Data;

public partial class MemberTransactionsReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void BindGrid()
    { 
        string ID =  Request.QueryString["id"];
        //DataTable dt = ReportController.FetchMemberTransactions(ID);
        DataTable dt = Installment.GetMemberInstallmentHistory(ID);

        gvTransactions.DataSource = dt;
        gvTransactions.DataBind();
    }
}

