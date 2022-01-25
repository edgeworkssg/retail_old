using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Net;
using System.Text.RegularExpressions;

namespace WinPowerPOS.MaintenanceForms
{
    public partial class frmFeedback : Form
    {

        private const String MANDRILL_SMTP_HOST = "smtp.mandrillapp.com";
        private const Int32 MANDRILL_SMTP_PORT = 587;
        private const String MANDRILL_SMTP_USERNAME = "support@edgeworks.com.sg";
        private const String MANDRILL_SMTP_PASSWORD = "4xA4n7Y-VEULPcq_rJHz3w";
        private const String SUPPORT_EMAIL = "support@edgeworks.com.sg";

        private String supportEmail = String.Empty;
        
        public frmFeedback()
        {
            InitializeComponent();
            this.supportEmail = AppSetting.GetSettingFromDBAndConfigFile("SupportTeamEmail");
            if (String.IsNullOrEmpty(this.supportEmail))
            {
                this.supportEmail = SUPPORT_EMAIL;
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateFields();

                MailController mail = new MailController() { 
                     Subject = this.txtSubject.Text.Trim(),
                     From = this.txtEmail.Text.Trim(),
                     DisplayName = "Feedback - " + this.txtCompany.Text.Trim() + " (" + this.txtName.Text.Trim() + ")",
                     MessageBody = MailController.FormatFeedbackMessage(this.txtCompany.Text.Trim(),this.txtName.Text.Trim(), this.txtEmail.Text.Trim(),this.txtSubject.Text.Trim(),this.cboSeverity.Text.Trim().Split(':')[0].Trim(),this.txtDescription.Text.Trim(),this.cboRate.Text.Trim()),
                     ToCommaSeparated = this.supportEmail,
                     SMTPHOST = MANDRILL_SMTP_HOST,
                     SMTPPort = MANDRILL_SMTP_PORT,
                     SMTPCredential = new NetworkCredential(MANDRILL_SMTP_USERNAME,MANDRILL_SMTP_PASSWORD)
                };
                try
                {
                    mail.Send();
                    MessageBox.Show("Feedback successfully sent.");
                }
                catch
                {
                    MessageBox.Show("Unable to Send Feedback. Please send your feedback through your email to " + this.supportEmail);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(ex.Message);
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

            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (!regex.IsMatch(this.txtEmail.Text.Trim()))
            {
                throw new Exception("Please enter a valid email address.");
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

        private void frmFeedback_Load(object sender, EventArgs e)
        {
            //get the company name from the app.config
            this.LoadCommonInfo();
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtEmail.Text.Trim()))
            {
                this.lblEmailValidator.Visible = false;
                return;
            }
            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (!regex.IsMatch(this.txtEmail.Text.Trim()))
            {
                this.lblEmailValidator.Visible = true;
            }
            else
            {
                this.lblEmailValidator.Visible = false;
            }
        }

        private void LoadCommonInfo()
        {
            Company company = new CompanyCollection().Load().ToList().FirstOrDefault(c => !String.IsNullOrEmpty(c.CompanyName));
            this.txtCompany.Text = company.CompanyName;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtCompany.Text = String.Empty;
            this.txtName.Text = String.Empty;
            this.txtEmail.Text = String.Empty;
            this.txtSubject.Text = String.Empty;
            this.cboSeverity.SelectedIndex = 0;
            this.txtDescription.Text = String.Empty;
            this.cboRate.SelectedIndex = 0;
            this.LoadCommonInfo();
            this.txtCompany.Focus();
        }
    }
}
