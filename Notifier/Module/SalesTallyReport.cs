using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.Net.Mail;
using Notifier.Properties;
using System.Net;
using PowerPOS;
using System.IO;
using System.Reflection;

namespace Notifier.Module
{
    public class SalesTallyReport: MailBase
    {
        public override string ModuleReport
        {
            get
            {
                return "SalesTallyReport";
            }
        }
        public override string ReportFilename
        {
            get { return "SalesTally.rpt"; }
        }

        public bool IsReportLoaded;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public SalesTallyReport(): base()
        {
            this.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
            this.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1, 23, 59, 59).AddDays(-1);
        }

        private static SalesTallyReport instance;
        public static SalesTallyReport Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new SalesTallyReport();
                }

                return instance;
            }
        }

        private bool IsSaleTally(out DataTable dt, out string strMailBody)
        {
            try
            {
                bool isSaleTally = true;
                dt = new DataTable();

                string SQL = "select ps.pointofsaleid as [POS ID],pointofsalename AS [POS NAME], ps.userfld3 as dbname, sum(nettamount * oh.userfloat3) as [HQ Amount In Foreign Currency], sum(nettamount) as [HQ Amount In Local Currency] " +
                                "from orderhdr oh inner join pointofsale ps " +
                                "on ps.pointofsaleid = oh.pointofsaleid " +
                                "where isvoided=0 and not ps.userfld3 is null " +
                                "and oh.CreatedOn <= (CONVERT(DATE, GETDATE()) + CAST(CONVERT(TIME, '01-01-2000 07:00:00') as DATETIME)) " +
                                "group by ps.pointofsaleid,pointofsalename,ps.userfld3;";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                dt = DataService.GetDataSet(cmd).Tables[0];

                dt.Columns.Add("RetailerLinkAmount", System.Type.GetType("System.Decimal"));

                //loop through and add the columns
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sql1 = "DECLARE @dbname nvarchar(128)" +
                                    "SET @dbname = N'" + dt.Rows[i]["dbname"].ToString() + "'" +
                                    "IF (EXISTS (SELECT name " +
                                    "FROM master.dbo.sysdatabases " +
                                    "WHERE ('[' + name + ']' = @dbname " +
                                    "OR name = @dbname))) " +
                                    "SELECT CAST(1 as bit) result " +
                                    "ELSE " +
                                    "SELECT CAST(0 as bit) result ";
                    cmd = new QueryCommand(sql1, "PowerPOS");
                    bool isDbExist = bool.Parse(DataService.ExecuteScalar(cmd).ToString());

                    if (isDbExist)
                    {
                        string sql2 = "use " + dt.Rows[i]["dbname"].ToString() + ";";
                        sql2 += "select sum(nettamount) as retailerLinkAmount " +
                                    "from orderhdr oh inner join pointofsale ps " +
                                    "on ps.pointofsaleid = oh.pointofsaleid  " +
                                    "and oh.CreatedOn <= (CONVERT(DATE, GETDATE()) + CAST(CONVERT(TIME, '01-01-2000 07:00:00') as DATETIME)) " +
                                    "where isvoided=0 and ps.userint1 = '" + dt.Rows[i]["POS ID"].ToString() + "'";

                        cmd = new QueryCommand(sql2, "PowerPOS");
                        decimal result = string.IsNullOrEmpty(DataService.ExecuteScalar(cmd).ToString()) ?
                            0M : decimal.Parse(DataService.ExecuteScalar(cmd).ToString());
                        dt.Rows[i]["RetailerLinkAmount"] = result;

                        decimal localAmount = string.IsNullOrEmpty(dt.Rows[i]["HQ Amount In Local Currency"].ToString()) ?
                            0M : decimal.Parse(dt.Rows[i]["HQ Amount In Local Currency"].ToString());
                        isSaleTally &= (localAmount) == result;
                    }
                }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<p>SALES TALLY REPORT</p>");
                sb.AppendLine();

                foreach (DataRow item in dt.Rows)
                {
                    sb.AppendLine(string.Format("<p><b>POS ID:</b> {0}</p>", item["POS ID"]));
                    sb.AppendLine(string.Format("<p><b>POS Name:</b> {0}</p>", item["POS NAME"]));
                    sb.AppendLine(string.Format("<p><b>Database Name:</b> {0}</p>", item["dbname"]));
                    sb.AppendLine(string.Format("<p><b>HQ Amount In Foreign Currency:</b> {0}</p>", item["HQ Amount In Foreign Currency"]));
                    sb.AppendLine(string.Format("<p><b>HQ Amount In Local Currency:</b> {0}</p>", item["HQ Amount In Local Currency"]));
                    sb.AppendLine(string.Format("<p><b>Retailer Link Amount:</b> {0}</p>", item["RetailerLinkAmount"]));
                    sb.AppendLine();
                }

                strMailBody = sb.ToString();

                return isSaleTally;
            }
            catch (Exception ex)
            {
                throw new SalesTallyReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public bool VerificationReportData()
        {
            try
            {
                DataTable dt = new DataTable();
                string strMailBody = string.Empty;

                if (Settings.Default.UseCrystalReport && !string.IsNullOrEmpty(ReportFilename))
                {
                    string exportedPath = this.GenerateReport(string.Format(@"{0}\Reports\{1}", Directory.GetParent(Assembly.GetAssembly(this.GetType()).Location), this.ReportFilename), CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<h3>SALES TALLY REPORT</h3>");
                    sb.AppendLine();
                    sb.AppendLine();
                    if (Settings.Default.UseLocalMailSetting)
                    {
                        this.SentLocalNotificationMail(Settings.Default.ProfitLossReport_MailSubject, true, sb.ToString(), exportedPath);
                    }
                    else
                    {
                        this.Message.Body = sb.ToString();
                        this.Message.Subject = Settings.Default.ProfitLossReport_MailSubject;

                        foreach (string item in this.GetMailReceivers())
                        {
                            this.Message.To.Add(item);
                        }

                        this.SentNotificationMail(true, exportedPath);
                    }

                    return true;
                }
                else
                {
                    //if report is not tally then send mail, if not then otherwise
                    if (!IsSaleTally(out dt, out strMailBody))
                    {
                        if (Settings.Default.UseLocalMailSetting)
                        {
                            this.SentLocalNotificationMail(Settings.Default.SaleTallyReport_MailSubject, true, strMailBody);
                        }
                        else
                        {
                            this.Message.Body = strMailBody;
                            this.Message.Subject = Settings.Default.SaleTallyReport_MailSubject;

                            foreach (string item in this.GetMailReceivers())
                            {
                                this.Message.To.Add(item);
                            }

                            this.SentNotificationMail(true);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (MailBaseException mbex)
            {
                throw mbex;
            }
            catch (Exception ex)
            {
                throw new SalesTallyReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public override SortedList<string, string> GetCRParam()
        {
            try
            {
                SortedList<string, string> parameters = new SortedList<string, string>();
                #region *) Get the Filter Value that User Keyed in
                parameters.Add("StartDate", this.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("EndDate", this.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                #endregion
                return parameters;
            }
            catch (Exception ex)
            {
                throw new SalesTallyReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }
    }

    [global::System.Serializable]
    public class SalesTallyReportException : Exception
    {
        public SalesTallyReportException() { }
        public SalesTallyReportException(string message) : base(message) { }
        public SalesTallyReportException(string message, Exception inner) : base(message, inner) { }
        protected SalesTallyReportException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
