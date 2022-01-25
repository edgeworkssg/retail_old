using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MallIntegrationServerPicker
{
    public partial class frmSetting : Form
    {
        public frmSetting()
        {
            InitializeComponent();
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            txtWebServiceURL.Text = Properties.Settings.Default.WS_URL;
            txtFolderLocation.Text = Properties.Settings.Default.Folder;
            txtUserName.Text = Properties.Settings.Default.UserName;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtBackupFolder.Text = Properties.Settings.Default.BackupFolder;
            txtCheckInterval.Text = Properties.Settings.Default.CheckFileInterval.ToString();
            txtMallCode.Text = Properties.Settings.Default.MallCode;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.WS_URL = txtWebServiceURL.Text;
            Properties.Settings.Default.Folder = txtFolderLocation.Text;
            Properties.Settings.Default.UserName = txtUserName.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.BackupFolder = txtBackupFolder.Text;
            int temp = 0;
            if (int.TryParse(txtCheckInterval.Text, out temp))
                Properties.Settings.Default.CheckFileInterval = temp;
            Properties.Settings.Default.MallCode = txtMallCode.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
