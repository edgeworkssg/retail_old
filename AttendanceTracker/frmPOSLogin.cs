using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Threading;
using System.Globalization;
using System.IO;

namespace AttendanceTracker
{
    public partial class frmPOSLogin : Form
    {
        public bool IsSuccessful;
        public bool isClosing;
        public bool allowClose;
        public frmPOSLogin()
        {
            InitializeComponent();
            IsSuccessful = false;
            isClosing = false;
            allowClose = false;
            if (File.Exists(Application.StartupPath  + "\\bannerLogin.jpg"))
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\bannerLogin.jpg");
                pictureBox1.Refresh();
            }            
        }

        private void txtUserID_Click(object sender, EventArgs e)
        {

        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            isClosing = false;
            this.Close();
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string status, role, DeptID;
            UserMst user;
            
            PointOfSaleController.GetPointOfSaleInfo();
            //AttributesLabelController.FetchProductAttributeLabel();
            if (UserController.login(txtUserID.Text, txtPassword.Text,
                LoginType.Login, out user,
                out role, out DeptID, out status))
            {

                //assign the user name and password to a common container
                UserInfo.username = txtUserID.Text;
                UserInfo.role = role;
                UserInfo.displayName = user.DisplayName;
                UserInfo.SalesPersonGroupID = user.SalesPersonGroupID;
                LanguageInfo.LangCode = "en-US";
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageInfo.LangCode);

                //pull privilege list
                UserInfo.privileges = UserController.FetchGroupPrivileges(UserInfo.role);
                IsSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(this, new EventArgs());
            }
        }

        private void frmPOSLogin_Load(object sender, EventArgs e)
        {
            if (allowClose)
            {
                btnCancel.Enabled = false;
            }
        }


    }
}
