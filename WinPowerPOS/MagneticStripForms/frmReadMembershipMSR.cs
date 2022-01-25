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
using System.Diagnostics;

namespace WinPowerPOS
{
    public partial class frmReadMembershipMSR : Form
    {
        public string buffer;
        
        public frmReadMembershipMSR()
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
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");

            foreach (Process onscreenProcess in oskProcessArray)
            {
                onscreenProcess.Kill();
            }

            if (e.KeyChar == ';')
            {
                buffer = "";
            }
            else if (e.KeyChar == '?')
            {
                buffer = buffer.Replace(";","").Replace("?","");   
                this.Close();
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
    }
}
