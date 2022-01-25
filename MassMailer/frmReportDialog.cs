using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MassMailer
{
    public partial class frmReportDialog : Form
    {
        public string status;
        public frmReportDialog()
        {
            InitializeComponent();
        }

        private void frmReportDialog_Load(object sender, EventArgs e)
        {
            txtStatus.Text = status;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}