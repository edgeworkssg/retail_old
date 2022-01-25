using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PowerInventory.Setup
{
    public partial class frmChangePassword : Form
    {
        public frmChangePassword()
        {
            InitializeComponent();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {

        }

        private void txtItemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            string status;
            
            //Check if password and confirm password the same
            if (txtConfirmPassword.Text != txtNewPassword.Text)
            {
                MessageBox.Show("Passwords entered do not matched");
                txtOldPassword.Select();
                return;
            }

            if (PowerPOS.UserController.ChangePassword(txtUserName.Text, txtOldPassword.Text, txtNewPassword.Text, out status))
            {
                MessageBox.Show("Change password succesful");
                this.Close();
                return;
            }
            else
            {
                MessageBox.Show(status);                
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
