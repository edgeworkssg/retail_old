using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace PowerPOS
{
    /// <summary>
    /// Custom Mail Controller currently use for feedback form and feedback page. By Karl M.
    /// Modified by Yohanda M. Add Attachment property.
    /// </summary>
    public class MailController
    {
        public String MessageBody { get; set; }
        public String Subject { get; set; }
        public String DisplayName { get; set; }
        public String ToCommaSeparated { get; set; }
        public String From { get; set; }
        public String CCCommaSeparated { get; set; }
        public String BccCommaSeparated { get; set; }

        public Int32 SMTPPort { get; set; }
        public String SMTPHOST { get; set; }
        public NetworkCredential SMTPCredential { get; set; }
        private Dictionary<string, Stream> _attachments = new Dictionary<string, Stream>();
        public Dictionary<string, Stream> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        public Boolean Send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                mail.Body = this.MessageBody;
                mail.Subject = this.Subject;

                if (!String.IsNullOrEmpty(this.ToCommaSeparated))
                {
                    foreach (String to in this.ToCommaSeparated.Split(','))
                    {
                        mail.To.Add(to);
                    }
                }
                else
                {
                    throw new Exception("Please provide a recipient email address.");
                }

                if (!String.IsNullOrEmpty(this.CCCommaSeparated))
                {
                    foreach (String cc in this.CCCommaSeparated.Split(','))
                    {
                        mail.CC.Add(cc);
                    }
                }

                if (!String.IsNullOrEmpty(this.BccCommaSeparated))
                {
                    foreach (String bcc in this.BccCommaSeparated.Split(','))
                    {
                        mail.Bcc.Add(bcc);
                    }
                }

                if (!String.IsNullOrEmpty(this.From))
                {
                    mail.From = new MailAddress(this.From, this.DisplayName);
                }
                else
                {
                    throw new Exception("Please provide a sender email address.");
                }

                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                foreach (var item in _attachments)
                    mail.Attachments.Add(new Attachment(item.Value, item.Key));

                smtp.Port = this.SMTPPort;
                smtp.Host = this.SMTPHOST;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = this.SMTPCredential;
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public static String FormatFeedbackMessage(String CompanyName, String SenderName, String Email, String Subject, String Severity, String Description, String Rate)
        {
            StringBuilder formattedMsg = new StringBuilder();
            formattedMsg.Append("<h4>" + Subject + "</h2><br/></br>");
            //formattedMsg.Append("<table>");
            //formattedMsg.Append("<tr>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>Company :</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>" + CompanyName + "</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("</tr>");
            //formattedMsg.Append("<tr>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>Name :</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>" + SenderName + "</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("</tr>");
            //formattedMsg.Append("<tr>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>Email :</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>" + Email + "</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("</tr>");
            //formattedMsg.Append("<tr>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>Severity :</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>" + Severity + "</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("</tr>");
            //formattedMsg.Append("<tr>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>Description :</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<p>" + Description + "</p>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("</tr>");
            //formattedMsg.Append("<tr>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<span>Rate :</span>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("<td>");
            //formattedMsg.Append("<p>" + Rate + "</p>");
            //formattedMsg.Append("</td>");
            //formattedMsg.Append("</tr>");
            //formattedMsg.Append("</table>");



            formattedMsg.Append("<span>FROM:</span><br/>");
            formattedMsg.Append("<span>" + SenderName + " (" + Email + ")</span></br>");
            formattedMsg.Append("<span>" + CompanyName + "</span></br></br></br></br>");
            formattedMsg.Append("<span>Severity: " + Severity + "</span></br></br>");
            formattedMsg.Append("<span>Rate: " + Rate + "</span></br></br></br></br>");
            formattedMsg.Append("<span>Description: </span></br></br>");
            formattedMsg.Append("<p>" + Description + "</p><br/><br/>");
            return formattedMsg.ToString();
        }
    }
}
