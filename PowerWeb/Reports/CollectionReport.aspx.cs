using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SubSonic;
using SubSonic.Utilities;
using PowerPOS;
using PowerPOSReports;

public partial class Reports_CollectionReport : System.Web.UI.Page
{
    private Database crDatabase;
    private CrystalDecisions.CrystalReports.Engine.Tables crTables;
    private CrystalDecisions.CrystalReports.Engine.Table crTable;
    private TableLogOnInfo crTableLogOnInfo;
    private ConnectionInfo crConnectionInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null || Session["Role"] == null ||
            (string)Session["UserName"] == "" || (string)Session["Role"] == "")
        {
            Response.Redirect("../login.aspx");
        }

        if (Request.QueryString["id"] != null)
        {
            
            string CounterCloseLogID = Utility.GetParameter("id");
            
            //Load Report...            
            ViewCloseCounterReportCollection settlement;
            DataTable ProductSalesReport, RefundSalesReport, CollectionBreakdownReport, CashRecordingReport;
            ViewVouchersSoldCollection VoucherSoldReport; 
            ViewVoucherRedeemedCollection VoucherRedeemedReport;
            CounterCloseDetCollection CounterCloseDetReport;

            bool isWithCashOut = false;
            isWithCashOut = PrivilegesController.HasPrivilege(PrivilegesController.CASH_OUT_COLLECTION_REPORT, (DataTable)Session["privileges"]);

            POSReports.FetchSettlementSalesReport
                (CounterCloseLogID, isWithCashOut,out settlement, out ProductSalesReport, out RefundSalesReport,
                 out VoucherSoldReport, out VoucherRedeemedReport, out CashRecordingReport, out CounterCloseDetReport);

            #region Prepare Collection Breakdown subreport
            CollectionBreakdownReport = new DataTable();
            CollectionBreakdownReport.Columns.Add("PaymentType", typeof(string));
            CollectionBreakdownReport.Columns.Add("Recorded", typeof(decimal));
            CollectionBreakdownReport.Columns.Add("Collected", typeof(decimal));
            CollectionBreakdownReport.Columns.Add("Diff", typeof(decimal));

            DataTable dtCCL = ReportController.FetchCounterCloseLogReport(false, false,
                DateTime.Now, DateTime.Now, CounterCloseLogID, "", "", 0, "ALL", 0, "", "");
            DataRow drCCL;
            if (dtCCL.Rows.Count > 0)
                drCCL = dtCCL.Rows[0];
            else
                drCCL = dtCCL.NewRow();

            DataSet dsPaymentTypes = new DataSet();
            dsPaymentTypes.ReadXml(Server.MapPath("~/PaymentTypes.xml"));
            DataTable dtPaymentTypes = dsPaymentTypes.Tables[0];
            decimal recorded, collected;

            recorded = Math.Round(settlement[0].CashRecorded.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
            collected = Math.Round(settlement[0].CashCollected, 2, MidpointRounding.AwayFromZero);
            CollectionBreakdownReport.Rows.Add("Cash", recorded, collected, collected - recorded);

            bool enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            bool enableNETSCashCard = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            bool enableNETSFlashPay = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            bool enableNETSATMPay = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);

            DataTable dtForeignCurrency = new DataTable();
            dtForeignCurrency.Columns.Add("CurrencyCode", typeof(string));
            dtForeignCurrency.Columns.Add("Recorded", typeof(string));
            dtForeignCurrency.Columns.Add("Collected", typeof(string));
            dtForeignCurrency.Columns.Add("Variance", typeof(string));

            if (!string.IsNullOrEmpty(drCCL["CurrencyCode1"] + ""))
            {
                decimal.TryParse(drCCL["RecordedAmount1"].ToString(), out recorded);
                decimal.TryParse(drCCL["CollectedAmount1"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                string currencySymbol = "";
                if (!string.IsNullOrEmpty(drCCL["CurrencySymbol1"] + ""))
                    currencySymbol = drCCL["CurrencySymbol1"].ToString();
                dtForeignCurrency.Rows.Add(drCCL["CurrencyCode1"].ToString(), 
                    string.Format("{0}{1}",currencySymbol,recorded.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, collected.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, (collected - recorded).ToString("N2")));
            }
            if (!string.IsNullOrEmpty(drCCL["CurrencyCode2"] + ""))
            {
                decimal.TryParse(drCCL["RecordedAmount2"].ToString(), out recorded);
                decimal.TryParse(drCCL["CollectedAmount2"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                string currencySymbol = "";
                if (!string.IsNullOrEmpty(drCCL["CurrencySymbol2"] + ""))
                    currencySymbol = drCCL["CurrencySymbol2"].ToString();
                dtForeignCurrency.Rows.Add(drCCL["CurrencyCode2"].ToString(),
                    string.Format("{0}{1}", currencySymbol, recorded.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, collected.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, (collected - recorded).ToString("N2")));
            }
            if (!string.IsNullOrEmpty(drCCL["CurrencyCode3"] + ""))
            {
                decimal.TryParse(drCCL["RecordedAmount3"].ToString(), out recorded);
                decimal.TryParse(drCCL["CollectedAmount3"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                string currencySymbol = "";
                if (!string.IsNullOrEmpty(drCCL["CurrencySymbol3"] + ""))
                    currencySymbol = drCCL["CurrencySymbol3"].ToString();
                dtForeignCurrency.Rows.Add(drCCL["CurrencyCode3"].ToString(),
                    string.Format("{0}{1}", currencySymbol, recorded.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, collected.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, (collected - recorded).ToString("N2")));
            }
            if (!string.IsNullOrEmpty(drCCL["CurrencyCode4"] + ""))
            {
                decimal.TryParse(drCCL["RecordedAmount4"].ToString(), out recorded);
                decimal.TryParse(drCCL["CollectedAmount4"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                string currencySymbol = "";
                if (!string.IsNullOrEmpty(drCCL["CurrencySymbol4"] + ""))
                    currencySymbol = drCCL["CurrencySymbol4"].ToString();
                dtForeignCurrency.Rows.Add(drCCL["CurrencyCode4"].ToString(),
                    string.Format("{0}{1}", currencySymbol, recorded.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, collected.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, (collected - recorded).ToString("N2")));
            }
            if (!string.IsNullOrEmpty(drCCL["CurrencyCode5"] + ""))
            {
                decimal.TryParse(drCCL["RecordedAmount5"].ToString(), out recorded);
                decimal.TryParse(drCCL["CollectedAmount5"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                string currencySymbol = "";
                if (!string.IsNullOrEmpty(drCCL["CurrencySymbol5"] + ""))
                    currencySymbol = drCCL["CurrencySymbol5"].ToString();
                dtForeignCurrency.Rows.Add(drCCL["CurrencyCode5"].ToString(),
                    string.Format("{0}{1}", currencySymbol, recorded.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, collected.ToString("N2")),
                    string.Format("{0}{1}", currencySymbol, (collected - recorded).ToString("N2")));
            }

            DataRow[] dr;
            dr = dtPaymentTypes.Select("ID = '1'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                if (!enableNETSIntegration)
                {
                    decimal.TryParse(drCCL["NetsRecorded"].ToString(), out recorded);
                    decimal.TryParse(drCCL["NetsCollected"].ToString(), out collected);
                    recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                    collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                    CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
                }
                else
                {
                    if (enableNETSCashCard)
                    {
                        decimal.TryParse(drCCL["NetsCashCardRecorded"].ToString(), out recorded);
                        decimal.TryParse(drCCL["NetsCashCardCollected"].ToString(), out collected);
                        recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                        collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                        CollectionBreakdownReport.Rows.Add("Nets Cash Card", recorded, collected, collected - recorded);
                    }
                    if (enableNETSFlashPay)
                    {
                        decimal.TryParse(drCCL["NetsFlashPayRecorded"].ToString(), out recorded);
                        decimal.TryParse(drCCL["NetsFlashPayCollected"].ToString(), out collected);
                        recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                        collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                        CollectionBreakdownReport.Rows.Add("Nets Flash Pay", recorded, collected, collected - recorded);
                    }
                    if (enableNETSATMPay)
                    {
                        decimal.TryParse(drCCL["NetsATMCardRecorded"].ToString(), out recorded);
                        decimal.TryParse(drCCL["NetsATMCardCollected"].ToString(), out collected);
                        recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                        collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                        CollectionBreakdownReport.Rows.Add("Nets ATM Card", recorded, collected, collected - recorded);
                    }
                }
            }

            dr = dtPaymentTypes.Select("ID = '2'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["VisaRecorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["VisaCollected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '3'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["ChinaNetsRecorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["ChinaNetsCollected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '4'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["AmexRecorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["AmexCollected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '5'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["Payment5Recorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["Payment5Collected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '6'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["Payment6Recorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["Payment6Collected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '7'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["Payment7Recorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["Payment7Collected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '8'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["Payment8Recorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["Payment8Collected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '9'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["Payment9Recorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["Payment9Collected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            dr = dtPaymentTypes.Select("ID = '10'");
            if (dr.Length > 0 && !string.IsNullOrEmpty(dr[0]["Name"].ToString()))
            {
                decimal.TryParse(drCCL["Payment10Recorded"].ToString(), out recorded);
                decimal.TryParse(drCCL["Payment10Collected"].ToString(), out collected);
                recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add(dr[0]["Name"].ToString(), recorded, collected, collected - recorded);
            }

            if (dtPaymentTypes.Select("Name = 'Voucher'").Length == 0)
            {
                // Hide Voucher if there's already a payment type named "Voucher" in PaymentTypes.xml
                recorded = Math.Round(settlement[0].VoucherRecorded.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
                collected = Math.Round(settlement[0].VoucherCollected, 2, MidpointRounding.AwayFromZero);
                CollectionBreakdownReport.Rows.Add("Voucher", recorded, collected, collected - recorded);
            }

            recorded = Math.Round(settlement[0].ChequeRecorded.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
            collected = Math.Round(settlement[0].ChequeCollected.GetValueOrDefault(0), 2, MidpointRounding.AwayFromZero);
            CollectionBreakdownReport.Rows.Add("Cheque", recorded, collected, collected - recorded);

            #region *) Show/hide Funding Method
            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enableSMF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            bool enablePAMed = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            if (enableFunding)
            {
                if (enableSMF)
                {
                    decimal.TryParse(drCCL["SMFRecorded"].ToString(), out recorded);
                    decimal.TryParse(drCCL["SMFCollected"].ToString(), out collected);
                    recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                    collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                    CollectionBreakdownReport.Rows.Add("SMF", recorded, collected, collected - recorded);
                }

                if (enablePAMed)
                {
                    decimal.TryParse(drCCL["PAMedRecorded"].ToString(), out recorded);
                    decimal.TryParse(drCCL["PAMedCollected"].ToString(), out collected);
                    recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                    collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                    CollectionBreakdownReport.Rows.Add("PAMedifund", recorded, collected, collected - recorded);
                }

                if (enablePWF)
                {
                    decimal.TryParse(drCCL["PWFRecorded"].ToString(), out recorded);
                    decimal.TryParse(drCCL["PWFCollected"].ToString(), out collected);
                    recorded = Math.Round(recorded, 2, MidpointRounding.AwayFromZero);
                    collected = Math.Round(collected, 2, MidpointRounding.AwayFromZero);
                    CollectionBreakdownReport.Rows.Add("PWF", recorded, collected, collected - recorded);
                }
            }

            if(CounterCloseDetReport.Count == 0)
                CrystalReportSource1.ReportDocument.DataDefinition.FormulaFields["IsShowCounterCloseDet"].Text = "false";
            else
                CrystalReportSource1.ReportDocument.DataDefinition.FormulaFields["IsShowCounterCloseDet"].Text = "true";
            #endregion

            #endregion

            CrystalReportSource1.ReportDocument.SetDataSource(settlement.ToDataTable());

            CrystalReportSource1.ReportDocument.Subreports["CollectionBreakdown"].SetDataSource(CollectionBreakdownReport);
            CrystalReportSource1.ReportDocument.Subreports["ProductSales"].SetDataSource(ProductSalesReport);
            CrystalReportSource1.ReportDocument.Subreports["ExchangeReport"].SetDataSource(RefundSalesReport);
            CrystalReportSource1.ReportDocument.Subreports["VoucherSold"].SetDataSource(VoucherSoldReport.ToDataTable());
            CrystalReportSource1.ReportDocument.Subreports["ViewVoucherRedeemed"].SetDataSource(VoucherRedeemedReport.ToDataTable());
            CrystalReportSource1.ReportDocument.Subreports["CashRecording"].SetDataSource(CashRecordingReport);
            CrystalReportSource1.ReportDocument.Subreports["ForeignCurrency"].SetDataSource(dtForeignCurrency);
            CrystalReportSource1.ReportDocument.Subreports["CounterCloseDet"].SetDataSource(CounterCloseDetReport.ToDataTable());
            CrystalReportViewer1.ReportSource = CrystalReportSource1.ReportDocument;
            CrystalReportSource1.ReportDocument.DataDefinition.
                FormulaFields["CounterCloseLogID"].Text = "\"" + CounterCloseLogID + "\"";

            CrystalReportSource1.ReportDocument.DataDefinition.
                FormulaFields["RoundingAmount"].Text = "\"" + POSReports.FetchTotalRoundingAmount(settlement[0].StartTime, settlement[0].EndTime, settlement[0].PointOfSaleID).ToString("N2") + "\"";

            CrystalReportSource1.ReportDocument.DataDefinition.
                FormulaFields["ShowCashOut"].Text = isWithCashOut.ToString();

            CrystalReportViewer1.RefreshReport();
        }
    }
}
