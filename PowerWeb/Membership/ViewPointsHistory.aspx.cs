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

public partial class ViewPointsHistory : PageBase
{
    private const string SORT_DIRECTION = "SORT_DIRECTION";
    private const string SORT_BY = "SORT_BY";
    string id;
    string pointID;
    bool showAll = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        string status;
        if (Session["UserName"] == null || Session["Role"] == null || 
            (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }
        if (Request.QueryString["id"] != null)
        {
            id = Utility.GetParameter("id");
            if (Request.QueryString["pointid"] != null &&
                (pointID = Utility.GetParameter("pointid")) != "")
            {
                try
                {
                    ViewState[SORT_BY] = "";
                    ViewState[SORT_DIRECTION] = "";
                    BindGrid(showAll);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }
        }
    }

    private void BindGrid(bool showAll)
    {
        DateTime startValidPeriod, endValidPeriod;
        decimal remainingPoint;
        
        decimal balance;
        DataTable dt = MembershipController.GetHistory_Point_WebSiteWithOption(showAll, id, pointID, out startValidPeriod, out endValidPeriod, out remainingPoint, out balance);
        dt.Columns.Add("Balance", Type.GetType("System.Decimal"));
    
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Balance"] = Decimal.Round(balance,2);
            if (dt.Rows[i]["Amount"] is decimal)
                balance -= (decimal)dt.Rows[i]["Amount"];

        }

        gvReport.DataSource = dt;
        gvReport.DataBind();
    }

    protected void lnkExport_Click(object sender, EventArgs e)
    {
        BindGrid(showAll);
        DataTable dt = (DataTable)gvReport.DataSource;
        CommonWebUILib.ExportCSVWithInvinsibleFields(dt, OrderDetailTitle.Text.Trim(' '), OrderDetailTitle.Text, gvReport);
    }

    protected void showAll_Click(object sender, EventArgs e)
    {
        showAll = true;
        BindGrid(showAll);
    }
}
