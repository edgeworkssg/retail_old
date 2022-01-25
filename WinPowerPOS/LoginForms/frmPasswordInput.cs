using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using PowerPOS;

namespace WinPowerPOS.LoginForms
{
    public partial class frmPasswordInput : Form
    {
        public bool IsSuccess = false;
        public frmPasswordInput(string userName)
        {
            InitializeComponent();
            txtUserName.Text = userName;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseOnscreenKeyboard();
            IsSuccess = false;
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string role, status, DeptID;
            UserMst user;
            if (!UserController.login(txtUserName.Text, txtPassword.Text, LoginType.Authorizing, out user, out role, out DeptID, out status))
            {
                MessageBox.Show("Login failed : " + status);
                IsSuccess = false;
            }
            else
            {
                CloseOnscreenKeyboard();
                IsSuccess = true;
                this.Close();
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

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnterAsOK), false))
                {
                    btnLogin_Click(this, new EventArgs());
                }
            }
        }
    }
}
