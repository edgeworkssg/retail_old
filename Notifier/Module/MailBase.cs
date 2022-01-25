using System;
using System.Collections.Generic;
using System.Linq;
using SubSonic;
using System.Data;
using System.Text;
using System.Net.Mail;
using Notifier.Properties;
using System.Net;
using CrystalDecisions.CrystalReports.Engine;
using PowerReport;
using CrystalDecisions.Shared;
using System.Reflection;
using System.IO;
using Notifier.Helper;

namespace Notifier.Module
{
    public class MailBase: IMailBase
    {
        public virtual string ModuleReport { get { return "%"; } }
        public virtual string ReportFilename
        {
            get { return string.Empty; }
        }
        protected CRReport Report { get; set; }

        public MailBase()
        {
            if (this.Message == null)
            {
                this.Message = new MailMessage();
            }
        }

        protected MailMessage Message { get; set; }

        public void SentNotificationMail(bool isBodyHTML)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Settings.Default.SMTP_Server,
                    Port = Settings.Default.SMTP_Port,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(Settings.Default.SMTP_Username, Settings.Default.SMTP_Password),
                    EnableSsl = true,
                    Timeout = 40000
                };

                this.Message.IsBodyHtml = isBodyHTML;
                this.Message.From = new MailAddress(Settings.Default.Mail_From_Address);

                smtp.Send(this.Message);

                System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                throw new MailBaseException(string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public void SentNotificationMail(bool isBodyHTML, string attachmentFilePath)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Settings.Default.SMTP_Server,
                    Port = Settings.Default.SMTP_Port,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(Settings.Default.SMTP_Username, Settings.Default.SMTP_Password),
                    EnableSsl = true,
                    Timeout = 40000
                };

                this.Message.IsBodyHtml = isBodyHTML;
                this.Message.From = new MailAddress(Settings.Default.Mail_From_Address);

                if (attachmentFilePath != "")
                {
                    using (Attachment data = new Attachment(attachmentFilePath))
                    {
                        this.Message.Attachments.Add(data);
                        smtp.Send(this.Message);
                    }
                }
                else
                {
                    smtp.Send(this.Message);
                }

                System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {
                throw new MailBaseException(string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public void SentNotificationMail(string subject,bool isBodyHTML, string body, string attachmentFileName)
        {
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = Settings.Default.SMTP_Server,
                    Port = Settings.Default.SMTP_Port,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(Settings.Default.SMTP_Username, Settings.Default.SMTP_Password),
                    EnableSsl = true,
                    Timeout = 40000
                };
                this.Message.From = new MailAddress(Settings.Default.Mail_From_Address);
                this.Message.Subject = subject;
                this.Message.IsBodyHtml = isBodyHTML;
                this.Message.Body = body;
                if (attachmentFileName != "")
                {
                    using (Attachment data = new Attachment(attachmentFileName))
                    {
                        this.Message.Attachments.Add(data);
                        smtp.Send(this.Message);
                    }
                }
                else
                {
                    smtp.Send(this.Message);
                }
                //Remove all objects that avoid deleting the attached file after being sent
                System.Threading.Thread.Sleep(200);
                smtp = null;
                this.Message.Dispose();
            }
            catch (Exception ex)
            {
                throw new MailBaseException(string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public void SentLocalNotificationMail(string subject, bool isBodyHTML, string body)
        {
            try
            {
                System.Web.Mail.MailMessage msgMailDefaulter = new System.Web.Mail.MailMessage();
                msgMailDefaulter.From = Settings.Default.Mail_From_Address;
                msgMailDefaulter.To = this.GetMailReceivers().Aggregate((result, x) => result + ";" + x);
                msgMailDefaulter.BodyFormat = System.Web.Mail.MailFormat.Html;
                msgMailDefaulter.Subject = subject;
                msgMailDefaulter.Body = body;
                System.Web.Mail.SmtpMail.SmtpServer = Properties.Settings.Default.SMTP_Server.ToString();
                System.Web.Mail.SmtpMail.Send(msgMailDefaulter);
            }
            catch (Exception ex)
            {
                throw new MailBaseException(string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public void SentLocalNotificationMail(string subject, bool isBodyHTML, string body, string attachmentFileName)
        {
            try
            {
                System.Web.Mail.MailMessage msgMailDefaulter = new System.Web.Mail.MailMessage();
                msgMailDefaulter.From = Settings.Default.Mail_From_Address;
                msgMailDefaulter.To = this.GetMailReceivers().Aggregate((result, x) => result + ";" + x);
                msgMailDefaulter.BodyFormat = System.Web.Mail.MailFormat.Html;
                msgMailDefaulter.Subject = subject;
                msgMailDefaulter.Body = body;
                if (attachmentFileName != "")
                {
                    System.Web.Mail.MailAttachment data = new System.Web.Mail.MailAttachment(attachmentFileName);
                    msgMailDefaulter.Attachments.Add(data);
                }
                System.Web.Mail.SmtpMail.SmtpServer = Properties.Settings.Default.SMTP_Server.ToString();
                System.Web.Mail.SmtpMail.Send(msgMailDefaulter);
            }
            catch (Exception ex)
            {
                throw new MailBaseException(string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public  List<string> GetMailReceivers()
        {
            try
            {
                string sql = "SELECT EmailAddress, Name, Module, Deleted " +
                    "FROM EmailNotification WHERE LOWER(Module) LIKE '%" + this.ModuleReport.ToLower() + "%'";
                QueryCommand queryCmd = new QueryCommand(sql, "PowerPOS");

                foreach (DataRow item in ((DataTable)DataService.GetDataSet(queryCmd).Tables[0]).Rows)
                {                   
                    this.Message.To.Add(item["EmailAddress"].ToString());
                }

                return this.Message.To.Select(x => x.Address).ToList();
            }
            catch (Exception ex)
            {
                throw new MailBaseException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public virtual SortedList<string, string> GetCRParam()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw new MailBaseException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public string GenerateReport(string reportPath, ExportFormatType type)
        {
            try
            {
                string extention = string.Empty;
                switch (type)
                {
                    case ExportFormatType.CrystalReport:
                        break;
                    case ExportFormatType.Excel:
                        break;
                    case ExportFormatType.ExcelRecord:
                        break;
                    case ExportFormatType.HTML32:
                        break;
                    case ExportFormatType.HTML40:
                        break;
                    case ExportFormatType.NoFormat:
                        break;
                    case ExportFormatType.PortableDocFormat:
                        extention = ".pdf";
                        break;
                    case ExportFormatType.RichText:
                        break;
                    case ExportFormatType.WordForWindows:
                        break;
                    default:
                        break;
                }

                string filename = string.Format("{0}{1}", this.ModuleReport, extention);
                ReportDocument rptDoc = new CRReport(reportPath).GetReport(this.GetCRParam());
                string exportedPath = string.Format(@"{0}{1}\{2}", Directory.GetParent(Assembly.GetAssembly(this.GetType()).Location), Settings.Default.ExportPath, filename);
                if (!Directory.GetParent(exportedPath).Exists)
                {
                    Directory.GetParent(exportedPath).Create();
                }
                rptDoc.ExportToDisk(type, exportedPath);
                return exportedPath;
            }
            catch (Exception ex)
            {
                throw new MailBaseException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }  
        }
    }

    [global::System.Serializable]
    public class MailBaseException : Exception
    {
        public MailBaseException() { }
        public MailBaseException(string message) : base(message) { }
        public MailBaseException(string message, Exception inner) : base(message, inner) 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("============================================================");
            sb.AppendFormat("Logger [{0}]", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss")).AppendLine();
            sb.AppendLine("============================================================");
            sb.AppendFormat("Caption: {0}", message).AppendLine();
            sb.AppendFormat("Message: {0}", inner.Message).AppendLine();
            sb.AppendFormat("Inner Message: {0}", inner.InnerException.Message).AppendLine();
            sb.AppendFormat("Stack Trace: [{0}]", inner.StackTrace).AppendLine().AppendLine();
            Logger.WriteLogToFile(sb.ToString());
        }
        protected MailBaseException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
