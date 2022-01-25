using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.EditBillForms
{
    public partial class frmMailInput : Form
    {
        public delegate void ButtonSendClickHandler(object sender, SendMailArgs e);
        public event ButtonSendClickHandler ButtonSendClicked;

        public string EmailTo
        {
            set
            {
                txtBoxEmail.Text = value;
            }
            get
            {
                return txtBoxEmail.Text;
            }
        }

        public string EmailSubject
        {
            set
            {
                txtBoxSubject.Text = value;
            }
            get
            {
                return txtBoxSubject.Text;
            }
        }

        public string EmailBody
        {
            set
            {
                txtBoxMailContent.Text = value;
            }
            get
            {
                return txtBoxMailContent.Text;
            }
        }

        public string EmailBcc { get; set; }

        public frmMailInput()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!SubSonic.Sugar.Validation.IsEmail(txtBoxEmail.Text))
            {
                MessageBox.Show("Please enter a valid mail address", "Warning", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(txtBoxSubject.Text))
            {
                MessageBox.Show("Please fill mail subject");
                return;
            }
            if (string.IsNullOrEmpty(txtBoxMailContent.Text))
            {
                MessageBox.Show("Please fill mail content");
                return;
            }

            string mailTo = txtBoxEmail.Text;
            string mailSubject = txtBoxSubject.Text;
            string mailContent = txtBoxMailContent.Text;
            string mailBcc = EmailBcc;
            this.Close();
            if (ButtonSendClicked != null)
                ButtonSendClicked(this, new SendMailArgs { MailTo = mailTo, MailSubject = mailSubject, MailContent = mailContent, MailBcc = mailBcc });
        }
    }

    public class SendMailArgs : EventArgs
    {
        public string MailTo { set; get; }
        public string MailSubject { set; get; }
        public string MailContent { set; get; }
        public string MailBcc { set; get; }
    }
}
