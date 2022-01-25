using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Threading;
using System.Globalization;
using SubSonic;

namespace MassMailer
{
    public partial class CashierLogin : Form
    {        
        public bool IsAuthorized;

        public CashierLogin()
        {
            InitializeComponent();
            LanguageInfo.LangCode = "en-US";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string status;
            try 
            {
                MembershipWS.FetchMembersPastTransaction ws = new MassMailer.MembershipWS.FetchMembersPastTransaction();
                ws.Url = PowerPOS.SyncClientController.WS_URL;
                if (ws.Login(UserController.EncryptData(txtUserID.Text), UserController.EncryptData(txtPassword.Text), out status))
                {
                    UserInfo.username = txtUserID.Text;
                    
                    this.Close();
                }
                else
                {
                    MessageBox.Show(status);
                }
                /*                           
                if (UserController.login(txtUserID.Text, txtPassword.Text, LoginType.Login, out role, out DeptID, out status))
                {                    

                }
                else
                {
                    MessageBox.Show(status);                    
                }*/
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CashierLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}