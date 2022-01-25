using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmKeypadPassCode : Form
    {
        public string value;
        public string initialValue;
        public string textMessage;
        public bool IsInteger;

        public frmKeypadPassCode()
        {
            InitializeComponent();
            IsInteger = false;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtEntry.Select();
            SendKeys.Send(((Button)sender).Text);
        }

        private void btnCLEAR_Click(object sender, EventArgs e)
        {
            txtEntry.Text = "";
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            //txtEntry.Text = txtEntry.Text.Remove(txtEntry.Text.Length - 1);
            txtEntry.Select();
            SendKeys.Send("{BACKSPACE}");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            value = initialValue;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            value = txtEntry.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmKeypadPassCode_Load(object sender, EventArgs e)
        {
           this.StartPosition = FormStartPosition.Manual;
            try
            {
                //this.Location = new Point( Screen.AllScreens[1].WorkingArea.Width / 4, Screen.AllScreens[1].WorkingArea.Height / 4) ;
                //this.Location = new Point(Screen.AllScreens[1].Bounds.Width /4, Screen.AllScreens[1].Bounds.Height / 4);
                this.Left = Screen.AllScreens[1].Bounds.Left + Screen.AllScreens[1].Bounds.Width / 2 - this.Width / 2;
                this.Top = Screen.AllScreens[1].Bounds.Top + Screen.AllScreens[1].Bounds.Height / 2 - this.Height / 2;
            }
            catch
            {
                this.Location = new Point(Screen.AllScreens[0].Bounds.Width / 2 - this.Width / 2, Screen.AllScreens[0].Bounds.Height / 2 - this.Height / 2);
            }

            txtEntry.Text = initialValue;
            lblMessage.Text = textMessage;
            txtEntry.Focus();
            txtEntry.Select();
        }

        private void txtEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnOK_Click(this, new EventArgs());
            }
        }

        private void frmKeypadPassCode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                this.DialogResult = DialogResult.Cancel;
        }

        private void frmKeypadPassCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                btnOK_Click(sender, e);
            }


        }
    }
}
