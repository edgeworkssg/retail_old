using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using System.IO;
using PowerPOS;
using SubSonic;
namespace WinPowerPOS
{
    public partial class frmUserSetup : Form
    {
        public string username;
        private UserMst myUser;
        public bool isNew;
        public frmUserSetup()
        {
            username = "";
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddCat_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {   
                Logger.writeLog(ex);
            }
        }
        private void DisableControls()
        {
            txtUserName.ReadOnly = true; 
            txtDisplayName.ReadOnly = true;
            txtPassword.ReadOnly = true;
            txtConfirmPassword.ReadOnly = true;
            txtRemark.ReadOnly = true; 
            cmbUserGroup.Enabled = false;            
            txtRemark.ReadOnly = true;                                                
        }

        private void frmEditItem_Load(object sender, EventArgs e)
        {

            
            //Load User Group            
            UserGroupCollection userGroup = new UserGroupCollection();
            userGroup.Where(UserGroup.Columns.Deleted,false);
            userGroup.Load();
            cmbUserGroup.DataSource = userGroup;
            cmbUserGroup.Refresh();
            cmbUserGroup.SelectedIndex = 0;
            //Load or Delete.....
            if (username == "")
            {
                btnAddUser.Text = "Add New";
                isNew = true;

            }
            else
            {
                isNew = false;
                btnAddUser.Text = "Save";
                myUser = new UserMst(username);

                if (!myUser.IsNew && myUser.IsLoaded)
                {
                    txtUserName.Text = myUser.UserName;
                    txtDisplayName.Text = myUser.DisplayName;
                    
                    txtPassword.Text = UserController.DecryptData(myUser.Password);
                    txtConfirmPassword.Text = txtPassword.Text;
                    txtPassword.ReadOnly = true;
                    txtConfirmPassword.ReadOnly = true;
                    txtRemark.Text = myUser.Remark;
                    for (int i = 0; i < cmbUserGroup.Items.Count; i++)
                    {
                        if (((UserGroup)cmbUserGroup.Items[i]).GroupID == myUser.GroupName)
                        {
                            cmbUserGroup.SelectedIndex = i;
                        }
                    }
                }
                //disable 
                txtUserName.ReadOnly = true;

            }
            txtUserName.Focus();
            txtUserName.Select();

        }

        private void btnAddEdit_Click(object sender, EventArgs e)
        {
            //Check if password and confirm password the same
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                MessageBox.Show("Passwords entered do not matched");
                txtPassword.Select();
                return;
            }
            try
            {
                if (myUser == null)
                    myUser = new UserMst();

                myUser.UserName = txtUserName.Text;
                myUser.DisplayName = txtDisplayName.Text;
                myUser.Password = UserController.EncryptData(txtPassword.Text);
                myUser.Remark = txtRemark.Text;
                myUser.GroupName = ((UserGroup)cmbUserGroup.SelectedValue).GroupID;
                myUser.DepartmentID = PointOfSaleInfo.DepartmentID;
                myUser.Save();
                MessageBox.Show("Save Successful", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (isNew)
                {
                    txtUserName.Text = myUser.UserName;
                    txtDisplayName.Text = myUser.DisplayName;
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    txtPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    txtRemark.Text = "";
                    
                    cmbUserGroup.SelectedIndex = 0;
                    txtUserName.Focus();
                    txtUserName.Select();                    
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("PK_UserMst"))
                {
                    MessageBox.Show("User Name already exist", "", 
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUserName.Select();
                }
                else
                {
                    MessageBox.Show(
                        "Error encountered. " + ex.Message, "",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Logger.writeLog(ex);
            }
        }
    }
}