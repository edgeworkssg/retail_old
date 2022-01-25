using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Diagnostics;

namespace WinPowerPOS.LoginForms
{
    public partial class frmSupervisorLogin : Form
    {
        public bool IsAuthorized;
        public bool NeedSomeoneElseToVerify = true;
        public bool OnlyCheckLogin = false;
        public string mySupervisor;
        public string privilegeName;
        public frmSupervisorLogin()
        {
            InitializeComponent();
        }

        private void SupervisorLogin_Load(object sender, EventArgs e)
        {
            IsAuthorized = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            CloseOnscreenKeyboard();
            if (NeedSomeoneElseToVerify)
            {
                if (txtSupervisorId.Text == PowerPOS.Container.UserInfo.username)
                {
                    MessageBox.Show("You need to ask someone else to login for you.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            string role, status, DeptID;
            UserMst user;
            if (UserController.login(txtSupervisorId.Text, txtPassword.Text, LoginType.Authorizing,out user, out role, out DeptID, out status))
            {
                if (!OnlyCheckLogin)
                {
                    if (PrivilegesController.HasPrivilege(privilegeName, UserController.FetchGroupPrivilegesWithUsername(role, user.UserName)))
                    {
                        mySupervisor = txtSupervisorId.Text;
                        IsAuthorized = true;
                        this.Close();
                        AccessLogController.AddLog(AccessSource.POS, UserInfo.username, txtSupervisorId.Text, "Supervisor login : " + role, "");
                    }
                    else
                    {
                        MessageBox.Show(txtSupervisorId.Text + " has insufficient privileges.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else {
                    mySupervisor = txtSupervisorId.Text;
                    IsAuthorized = true;
                    this.Close();
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, txtSupervisorId.Text, "Supervisor login : " + role, "");
                }
                
                /* --MAKE SURE IT IS SUPERVISOR--
                if (role == "Supervisor")
                {
                    mySupervisor = txtSupervisorId.Text;
                    IsAuthorized = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(txtSupervisorId.Text + " is not a supervisor. Please ask a supervisor to authorize.");
                }*/
            }
            else
            {
                MessageBox.Show("Invalid supervisor id/password. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseOnscreenKeyboard();
            this.Close();
        }

        private int clickNo = 0;
        private static void OpenWindows8TouchKeyboard()
        {
            var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
            Process.Start(path);
        }
        private void CloseOnscreenKeyboard()
        {
            //Kill all on screen keyboards
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            if (oskProcessArray.Length > 0)
            {
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            clickNo++;
            switch (clickNo)
            {
                case 1: OpenWindows8TouchKeyboard(); break;
                case 2: CloseOnscreenKeyboard(); clickNo = 0; break;
            }      
        }    
    }
}