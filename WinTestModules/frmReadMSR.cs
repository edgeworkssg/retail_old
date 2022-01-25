using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinTestModules
{
    public partial class frmReadMSR : Form
    {
        public frmReadMSR()
        {
            InitializeComponent();
            buffer = "";
        }
        string buffer;
        private void frmTestMSR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '?')
            {
                MessageBox.Show(buffer);
                //this.Close();
            }
            else
            {
                buffer += e.KeyChar.ToString();
            }
        }
    }
}
