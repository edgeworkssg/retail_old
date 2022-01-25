using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notifier.Properties;
using System.IO;
using System.Reflection;
using System.Data;

namespace Notifier.Module
{
    public class MonthlySalesReport: MailBase
    {
        public override string ModuleReport
        {
            get
            {
                return "MonthlySaleReport";
            }
        }

        public override string ReportFilename
        {
            get
            {
                return "MonthlySaleReport.rpt";
            }
        }

        public string Outlet { get; set; }
        private string month;
        public string Month
        {
            get { return this.month; }
            set
            {
                int currentValue = 0;
                if (int.TryParse(value, out currentValue))
                {
                    if (currentValue == 0)
                    {
                        this.month = DateTime.Today.Month.ToString();
                    }
                    else if (int.Parse(value) < 0)
                    {
                        this.month = DateTime.Today.AddMonths(currentValue).Month.ToString();
                        this.year = DateTime.Today.AddMonths(currentValue).Year.ToString();
                    }
                    else
                    {
                        this.month = value;
                    }
                }
                else
                {
                    this.month = value;
                }
            }
        }
        private string year;
        public string Year 
        {
            get { return this.year; }
            set
            {
                int currentValue = 0;
                if (int.TryParse(value, out currentValue))
                {
                    if (currentValue == 0)
                    {
                        this.year = DateTime.Today.Year.ToString();
                    }
                    else if (int.Parse(value) < 0)
                    {
                        this.year = DateTime.Today.AddYears(currentValue).Year.ToString();
                    }
                    else
                    {
                        this.year = value;
                    }
                }
                else
                {
                    this.year = value;
                }
            }
        }

        public MonthlySalesReport()
            : base()
        {
            this.Outlet = "0";
        }

        private static MonthlySalesReport instance;
        public static MonthlySalesReport Instance 
        {
            get 
            {
                if (instance == null)
                {
                    instance = new MonthlySalesReport();
                }
                return instance;
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
                    sb.AppendLine("<h3>MONTHLY SALES REPORT</h3>");
                    sb.AppendLine();
                    sb.AppendLine();
                    if (Settings.Default.UseLocalMailSetting)
                    {
                        this.SentLocalNotificationMail(Settings.Default.MonthlySaleReport_MailSubject, true, sb.ToString(), exportedPath);
                    }
                    else
                    {
                        this.Message.Body = sb.ToString();
                        this.Message.Subject = Settings.Default.MonthlySaleReport_MailSubject;

                        foreach (string item in this.GetMailReceivers())
                        {
                            this.Message.To.Add(item);
                        }

                        this.SentNotificationMail(true, exportedPath);
                    }
                    return true;
                }
                else return false;
            }
            catch (MailBaseException mbex)
            {
                throw mbex;
            }
            catch (Exception ex)
            {
                throw new MonthlySalesReportException(
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
                parameters.Add("Outlet", this.Outlet);
                parameters.Add("Month", this.Month);
                parameters.Add("Year", this.Year);
                #endregion
                return parameters;
            }
            catch (Exception ex)
            {
                throw new MonthlySalesReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }
    }

    [global::System.Serializable]
    public class MonthlySalesReportException : Exception
    {
        public MonthlySalesReportException() { }
        public MonthlySalesReportException(string message) : base(message) { }
        public MonthlySalesReportException(string message, Exception inner) : base(message, inner) { }
        protected MonthlySalesReportException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
