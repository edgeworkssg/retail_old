using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.ComponentModel;
//using ICSharpCode.SharpZipLib.Zip;


/// <summary>
/// Summary description for MassEmail
/// </summary>
public class MassEmail
{

    public struct Membership
    {
        public string MemberId;
        public string MembershipNo;
        public string MembershipGroupId;
        public string Title;
        public string LastName;
        public string FirstName;
        public string ChristianName;
        public string NameToAppear;
        public string Email;
    }

    public MassEmail()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string RandomString(int size, bool lowerCase)
    {
        // To Create Random String
        StringBuilder builder = new StringBuilder();
        Random random = new Random();
        char ch;
        for (int i = 0; i < size; i++)
        {
            ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
            builder.Append(ch);
        }
        if (lowerCase)
            return builder.ToString().ToLower();
        return builder.ToString();
    }

    public void DeleteFile(string strFileName)
    {
        //Delete file from the server
        if (strFileName.Trim().Length > 0)
        {
            FileInfo fi = new FileInfo(strFileName);
            if (fi.Exists)//if file exists delete it
            {
                fi.Delete();
            }
        }
    }

    public void DeleteOlderDirectories(string rootDir, int MonthsBack)
    {
        // Delete folder older than MonthsBack
        int intOneMonthBefore = int.Parse(DateTime.Today.AddMonths(MonthsBack).ToString("yyyyMMdd"));
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootDir);
        foreach (System.IO.DirectoryInfo g in dir.GetDirectories())
        {
            if (intOneMonthBefore > int.Parse(g.Name))
            {
                Directory.Delete(g.FullName, true);
            }
        }
    }
    /*
    public string SendEmailMarketing(Membership[] members,string strFrom, string strSubject, 
        string strUploadedFile, string strUploadFileDir, string strDomain, Boolean boolPlainView)
    {
        string errorMessage="";
        string strHTMLMessageBody = "";
        string strTextMessageBody = "";

        string strEmailServer = "smtp.mail.yahoo.com";
        string strUsername = "sugiggstest";
        string strPassword = "test123456";

        Boolean boolTotalSuccess = true;

        // create folder  name = current date
        string strDateNow = DateTime.Now.ToString("yyyyMMdd");
        try
        {
            if (!Directory.Exists(strUploadFileDir + "\\" + strDateNow))
                Directory.CreateDirectory(strUploadFileDir + "\\" + strDateNow);
        }
        catch (Exception)//in case of an error
        {
            errorMessage = "Error in creating directory. Please contact your administrator.";
            DeleteFile(strUploadedFile);
        }

        // create unique name for each mass mail
        string strRandom = RandomString(10, true);
        while (Directory.Exists(strUploadFileDir + "\\" + strDateNow + "\\" + strRandom))
            strRandom = RandomString(10, true);
        string strDestDir = strUploadFileDir + strDateNow + "\\" + strRandom + "\\";
        string strDestDomain = strDomain + strDateNow + "/" + strRandom + "/";

        FastZip fz = new FastZip();
        Boolean zipSuccess = true;

        try
        {
            fz.ExtractZip(strUploadedFile, strDestDir, "");

        }
        catch (Exception)//in case of an error
        {
            zipSuccess = false;
            errorMessage = "Error extracting zip file. Please upload valid zip file.";
        }

        if (zipSuccess)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(strDestDir);
            FileInfo[] fileinfoHTML = dirInfo.GetFiles("*.htm*");
            FileInfo[] fileinfoText = dirInfo.GetFiles("*.txt");
            string strHTMLFile = "";
            string strTextFile = "";

            if ((fileinfoHTML.Length > 0) && (fileinfoText.Length > 0))
            {
                strHTMLFile = strDestDir + fileinfoHTML[0].Name;
                strTextFile = strDestDir + fileinfoText[0].Name;
            }
            else
            {
                if (fileinfoHTML.Length > 0)
                {
                    strHTMLFile = strDestDir + fileinfoHTML[0].Name;
                    strTextFile = strHTMLFile;
                }
                else
                    if (fileinfoText.Length > 0)
                    {
                        strTextFile = strDestDir + fileinfoText[0].Name;
                        strHTMLFile = strTextFile;
                    }

            }

            if (strHTMLFile != "")
            {
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(strHTMLFile);
                    strHTMLMessageBody = reader.ReadToEnd();
                    reader = new StreamReader(strTextFile);
                    strTextMessageBody = reader.ReadToEnd();
                }
                catch
                {
                    errorMessage = "Error. Reading HTML and Text File failed.";
                }
                finally
                {
                    reader.Close();
                }

                // Add domain to <img src=
                strHTMLMessageBody = Regex.Replace(strHTMLMessageBody, "<img(.*)src=\"([^\"]*)\"([^>]*)>", "<img$1src=\"" + strDestDomain + "$2\"$3>", RegexOptions.IgnoreCase);

                //errorMessage = SendEmails(members, strFrom, strSubject, strHTMLMessageBody, strTextMessageBody, strEmailServer, strUsername, strPassword, boolPlainView);

            }
            else
            {
                boolTotalSuccess = false;
            }
        }

        //Cleaning files
        try
        {
            DeleteFile(strUploadedFile);
            DeleteOlderDirectories(strUploadFileDir, -1);
            if (!boolTotalSuccess)
            {
                Directory.Delete(strDestDir, true);
            }
        }
        catch
        {
        }
        return errorMessage;
    }
    */
    public string SendEmails(PowerPOS.MembershipCollection members, string strFrom, string strSubject, string strHTMLBody, string strTextBody, string smtpServer, string smtpUsername, string smtpPassword, Boolean boolPlain)
    {
        
        string errorMessage = "";
        string statusSending = "";
        SmtpClient smtp = new SmtpClient(smtpServer);
        //to authenticate we set the username and password properites on the SmtpClient
        smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

        Type membershipTypes = members[0].GetType();
        System.Reflection.FieldInfo[] fields = membershipTypes.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        MailMessage mail = new MailMessage();
        
        mail.From = new MailAddress(strFrom);
        string msgHTMLBodyPersonal = @"<img src=""cid:image1""/>"; // strHTMLBody;
        string msgTextBodyPersonal = strTextBody;
        string msgSubjectPersonal = strSubject;

        AlternateView plainView = AlternateView.CreateAlternateViewFromString(msgTextBodyPersonal, null, MediaTypeNames.Text.Plain);
        AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msgHTMLBodyPersonal, null, MediaTypeNames.Text.Html);
        LinkedResource m = new LinkedResource("c:\\Outlook.jpg");
        m.ContentId = "image1";
        m.ContentType.Name = "outlook.jpg";
        htmlView.LinkedResources.Add(m);
        mail.AlternateViews.Add(htmlView);

        errorMessage = ""; // "<table border=1><th>MembershipNo<th>First Name<th>Email<th>Status";
        foreach (PowerPOS.Membership member in members)
        {
            //PowerPOS.Logger.writeLog(member.MemberId + "----" + member.MembershipNo);
            if (member.Email != null && SubSonic.Sugar.Validation.IsEmail(member.Email))
            {
                //errorMessage += "<tr>";
                mail.To.Clear();

                mail.To.Add(member.Email);
                //errorMessage += "<td>" + member.MemberId;
                errorMessage += "<td>" + member.MembershipNo;
                errorMessage += "<td>" + member.NameToAppear;
                errorMessage += "<td>" + member.Email;

                foreach (System.Reflection.FieldInfo field in fields)
                {
                    if (field.GetValue(member) != null)
                    {
                        msgHTMLBodyPersonal = msgHTMLBodyPersonal.Replace("%" + field.Name + "%", field.GetValue(member).ToString());
                        msgTextBodyPersonal = msgTextBodyPersonal.Replace("%" + field.Name + "%", field.GetValue(member).ToString());
                        msgSubjectPersonal = msgSubjectPersonal.Replace("%" + field.Name + "%", field.GetValue(member).ToString());
                    }
                }

                mail.Subject = msgSubjectPersonal;
                /*
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(msgTextBodyPersonal, null, MediaTypeNames.Text.Plain);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(msgHTMLBodyPersonal, null, MediaTypeNames.Text.Html);
                LinkedResource m = new LinkedResource("c:\\Outlook.jpg");
                m.ContentId = "image1";
                m.ContentType.Name = "outlook.jpg";                
                htmlView.LinkedResources.Add(m);
                mail.AlternateViews.Add(htmlView);
                
                 */
                if (boolPlain)
                    mail.AlternateViews.Add(plainView);

                try
                {
                    smtp.Send(mail);
                    statusSending = "<td> Email sent.";
                }
                /*catch (Exception Err)//in case of an error
                {
                    statusSending = "An Error Occured. Message: " + Err.Message;
                }*/
                catch (SmtpException Err)//in case of an error
                {

                    //errorMessage += "An Error Occured. Message: " + Err.StatusCode;
                    if (Err.StatusCode == SmtpStatusCode.GeneralFailure)
                    {
                        statusSending = "<td>An Error Occured. Message: SMTP Host cannot be found";
                    }
                    else
                        if (Err.StatusCode == SmtpStatusCode.MustIssueStartTlsFirst)
                        {
                            statusSending = "<td>An Error Occured. Message: Authentication failed!";
                        }
                        else
                            if (Err.StatusCode == SmtpStatusCode.MailboxUnavailable)
                            {
                                statusSending = "<td>An Error Occured. Message: Authentication failed!";
                            }
                            else
                            {
                                statusSending = "<td>An Error Occured. Message: " + Err.Message;
                            }
                }
                //PowerPOS.Logger.writeLog(statusSending);
                
                errorMessage += statusSending;
                
            }
            else
            {
                PowerPOS.Logger.writeLog("Invalid Email");
            }
            PowerPOS.Logger.writeLog(errorMessage);
            Console.WriteLine(errorMessage);
        }
        errorMessage += "</table>";
        return errorMessage;
    }


}
