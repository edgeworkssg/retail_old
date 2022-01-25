using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.LoginForms
{
    public partial class frmSupportLogin : Form
    {

        public Boolean IsAuthorized { get; private set; }

        public frmSupportLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.Equals("pressingon", this.txtPassword.Text))
            {
                this.IsAuthorized = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Password!");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.IsAuthorized = false;
            this.Close();
        }
    }
}
