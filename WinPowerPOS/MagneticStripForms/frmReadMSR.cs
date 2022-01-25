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
using System.IO;

namespace WinPowerPOS
{
    public partial class frmReadMSR : Form
    {
        public string buffer;
        public bool IsAuthorized;
        public string privilegeName;
        public UserMst myUser;
        public LoginType loginType;

        public frmReadMSR()
        {
            InitializeComponent();
            buffer = "";
            if (File.Exists(Application.StartupPath + "\\bannerLogin.jpg"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\bannerLogin.jpg");
                pictureBox1.Refresh();
            }
        }

        private void frmReadMSR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ';')
            {
                buffer = "";
            }
            else if (e.KeyChar == '?')
            {
                UserMst myUser;
                string role, status, deptID;
                if (UserController.loginWithMagneticStripeCard
                    (buffer.Replace(";", "").Replace("?", ""), loginType, out myUser,
                    out role, out deptID, out status))
                {
                    if (loginType == LoginType.Login)
                    {
                        //assign the user name and password to a common container
                        UserInfo.username = myUser.UserName;
                        UserInfo.role = role;
                        UserInfo.displayName = myUser.DisplayName;
                        UserInfo.SalesPersonGroupID = myUser.SalesPersonGroupID;
                        LanguageInfo.LangCode = "en-US";
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageInfo.LangCode);

                        //pull privilege list
                        UserInfo.privileges = UserController.FetchGroupPrivilegesWithUsername(UserInfo.role, UserInfo.username);
                        IsAuthorized = true;
                        this.Close();
                    }
                    else
                    {
                        if (PrivilegesController.HasPrivilege(privilegeName,
                            UserController.FetchGroupPrivilegesWithUsername(role, UserInfo.username)))
                        {
                            IsAuthorized = true;

                            this.Close();
                        }
                        else
                        {
                            IsAuthorized = false;
                            this.Close();
                        }
                    }
                }
                else
                {
                    if (loginType == LoginType.Login)
                    {
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                buffer += e.KeyChar.ToString();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            buffer = "";
            this.Close();
        }

        private void frmReadMSR_Load(object sender, EventArgs e)
        {
            pictureBox1.Select();
            pictureBox1.Focus();
        }
    }
}
