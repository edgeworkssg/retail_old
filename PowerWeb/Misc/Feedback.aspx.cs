using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PowerPOS;
using System.Net;

namespace PowerWeb
{
    public partial class WebForm1 : PageBase
    {

        private const String MANDRILL_SMTP_HOST = "smtp.mandrillapp.com";
        private const Int32 MANDRILL_SMTP_PORT = 587;
        private const String MANDRILL_SMTP_USERNAME = "support@edgeworks.com.sg";
        private const String MANDRILL_SMTP_PASSWORD = "4xA4n7Y-VEULPcq_rJHz3w";
        private const String SUPPORT_EMAIL = "feedback@edgeworks.com.sg";

        private String supportEmail = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            //get the company name from the app.config
            this.LoadCommonInfo();
        }

        private void LoadCommonInfo()
        {
            Company company = new CompanyCollection().Load().ToList().FirstOrDefault(c => !String.IsNullOrEmpty(c.CompanyName));
            this.txtCompany.Text = company.CompanyName;
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateFields();

                this.supportEmail = AppSetting.GetSettingFromDBAndConfigFile("SupportTeamEmail");
                if (String.IsNullOrEmpty(this.supportEmail))
                {
                    this.supportEmail = SUPPORT_EMAIL;
                }


                MailController mail = new MailController()
                {
                    Subject = this.txtSubject.Text.Trim(),
                    From = this.txtEmail.Text.Trim(),
                    DisplayName = "Feedback - " + this.txtCompany.Text.Trim() + " (" + this.txtName.Text.Trim() + ")",
                    MessageBody = MailController.FormatFeedbackMessage(this.txtCompany.Text.Trim(), this.txtName.Text.Trim(), this.txtEmail.Text.Trim(), this.txtSubject.Text.Trim(), this.cboSeverity.Text.Trim().Split(':')[0].Trim(), this.txtDescription.Text.Trim(), this.cboRate.Text.Trim()),
                    ToCommaSeparated = this.supportEmail,
                    SMTPHOST = MANDRILL_SMTP_HOST,
                    SMTPPort = MANDRILL_SMTP_PORT,
                    SMTPCredential = new NetworkCredential(MANDRILL_SMTP_USERNAME, MANDRILL_SMTP_PASSWORD)
                };
                try
                {
                    mail.Send();
                    lblMsg.Text = "<span style=\"font-weight:bold; color:#22bb22\">Feedback successfully sent.</span>";
                }
                catch(Exception x)
                {
                    Logger.writeLog(x);
                    lblMsg.Text = "<span style=\"font-weight:bold; color:#990000\">Unable to Send Feedback. Please send your feedback through your email to " + this.supportEmail + "</span>";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "<span style=\"font-weight:bold; color:#990000\">" + ex.Message + "</span> ";
            }
        }

        private void ValidateFields()
        {
            if (String.IsNullOrEmpty(this.txtCompany.Text.Trim()))
            {
                throw new Exception("Please enter Company field.");
            }
            if (String.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                throw new Exception("Please enter Name field.");
            }
            if (String.IsNullOrEmpty(this.txtEmail.Text.Trim()))
            {
                throw new Exception("Please enter Email field.");
            }
            if (String.IsNullOrEmpty(this.cboSeverity.Text.Trim()))
            {
                throw new Exception("Please select Severity for this feedback.");
            }

            if (String.IsNullOrEmpty(this.cboRate.Text.Trim()))
            {
                throw new Exception("Please select Rate the feedback.");
            }
        }
    }
}
