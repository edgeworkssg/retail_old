using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Web;
using PowerPOS;
using System.Configuration;

public partial class AccountStatementDownload : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Query String
        string MembershipNo = Request.QueryString["id"];
        string StartDate = Request.QueryString["startdate"];
        string EndDate = Convert.ToDateTime(Request.QueryString["enddate"]).AddSeconds(86399).ToString("dd-MMM-yyyy HH:mm:ss");
        string download = Request.QueryString["download"];

        if (MembershipNo == null)
        {
            return;
        }
        
        
        DataTable dt = ReportController.FetchAccountStatementDetail(StartDate, EndDate, MembershipNo);

        DataTable dtprev = ReportController.FetchPreviousOutstandingBalanceDetail(StartDate, EndDate, MembershipNo);

        decimal[] Prevbalance = new Decimal[13];

        for (int j = 0; j < dtprev.Rows.Count; j++ )
        {
            Prevbalance[Convert.ToInt16(dtprev.Rows[j]["receiptmonth"].ToString())] = Convert.ToDecimal(dtprev.Rows[j]["balance"].ToString());  
        }

        // Prepare Report
        string ReportName = Server.MapPath("~\\bin\\Reports\\YearlyAccountStatementReport.rpt");
        ReportDocument rpt = new ReportDocument();
        rpt.Load(ReportName);
        rpt.SetDataSource(dt);

        DateTime EndDateConv = Convert.ToDateTime(EndDate);
        

        for (int i = 0; i < rpt.DataDefinition.FormulaFields.Count; i++)
        {
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@EndDate}")
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + EndDate + "\"";
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@MonthEndDate}")
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:MMMM}", EndDateConv).ToString() +"\"";

            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev1}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[1]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev2}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[2]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev3}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[3]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev4}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[4]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev5}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[5]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev6}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[6]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev7}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[7]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev8}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[8]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev9}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[9]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev10}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[10]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev11}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[11]) + "\"";
            }
            if (rpt.DataDefinition.FormulaFields[i].FormulaName == "{@Prev12}")
            {
                rpt.DataDefinition.FormulaFields[i].Text = "\"" + String.Format("{0:N2}", Prevbalance[12]) + "\"";
            }

        }
        
        try
        {
            if (download == null)
            {
                CrystalReportViewer CRViewer = new CrystalReportViewer();
                form2.Controls.Add(CRViewer);
                CRViewer.ReportSource = rpt;
                CRViewer.RefreshReport();
            }
            else
            {
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, MembershipNo);
            }

        }

        catch (Exception X)
        {
            Logger.writeLog(X);
        }
    }
}
