using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Configuration;
using ERPIntegration.API;
using System.Collections;
using System.IO;


namespace ERPIntegration
{
    public static class MailSender
    {
        struct MessageType
        {
            public static string INFO = "ERP_INTEGRATION_INFO";
            public static string ERROR = "ERP_INTEGRATION_ERROR";
        }

        public static bool sendEmail(string subject, StringBuilder Body, string attachmentFileName)
        {
            string SmtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            string Username = ConfigurationManager.AppSettings["Username"];
            string Password = ConfigurationManager.AppSettings["Password"];
            string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"];
            string SenderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            string DefaultMailTo = ConfigurationManager.AppSettings["DefaultMailTo"];

            ArrayList failed = new ArrayList();
            SmtpClient smtp = new SmtpClient(SmtpServer);
            //to authenticate we set the username and password properites on the SmtpClient
            smtp.Credentials = new NetworkCredential(Username, Password); 
            smtp.Port = int.Parse(SMTPPort);
            smtp.EnableSsl = true;            

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(SenderEmail);
            //string msgHTMLBodyPersonal = strHTMLBody;
            string msgTextBodyPersonal = Body.ToString();
            string msgSubjectPersonal = subject;

            AlternateView plainView = AlternateView.CreateAlternateViewFromString(msgTextBodyPersonal, null, MediaTypeNames.Text.Plain);

            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msgHTMLBodyPersonal, null, MediaTypeNames.Text.Html);
            mail.AlternateViews.Add(plainView);

            string status = "";
            int count = 0;

            mail.To.Clear();
            mail.To.Add(DefaultMailTo);
            mail.Subject = msgSubjectPersonal;

            if (attachmentFileName != "")
            {
                if (File.Exists(attachmentFileName))
                {
                    using (Attachment data = new Attachment(attachmentFileName))
                    {
                        mail.Attachments.Add(data);
                        smtp.Send(mail);
                        //System.Threading.Thread.Sleep(200);
                        Helper.WriteLog("Email sent.", MessageType.INFO);
                        return true;
                    }
                }
            }

            try
            {
                smtp.Send(mail);
                //System.Threading.Thread.Sleep(200);
                Helper.WriteLog("Email sent.",MessageType.INFO);
                return true;
                //status += member.MembershipNo + " " + member.NameToAppear + " " + member.Email + ". Email sent.\r\n";
            }
            catch (SmtpException Err)//in case of an error
            {
                if (Err.StatusCode == SmtpStatusCode.GeneralFailure)
                {
                    Helper.WriteLog("An Error Occured. Message: SMTP Host cannot be found", MessageType.ERROR);
                }
                else
                {
                    if (Err.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst)
                    {
                        Helper.WriteLog(". An Error Occured. Message: Authentication failed!", MessageType.ERROR);
                    }
                    else
                    {
                        if (Err.StatusCode == SmtpStatusCode.MailboxUnavailable)
                        {
                            Helper.WriteLog(". An Error Occured. Message: Authentication failed!", MessageType.ERROR);
                        }
                        else
                        {
                            Helper.WriteLog(". An Error Occured. Message: " + Err.Message, MessageType.ERROR);
                        }
                    }
                }

                return false;
            }
            return true;
        }
    }
}
