using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.OrderForms
{
    public partial class frmKeypad : Form
    {
        public string value;
        public string initialValue;
        public string textMessage;
        public bool IsInteger;
        public frmKeypad()
        {
            InitializeComponent();
            IsInteger = false;
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtEntry.Select();
            SendKeys.Send(((Button)sender).Text);
            //decimal val=0;
            //decimal.TryParse(txtEntry.Text, out val);
            //txtEntry.Text = val.ToString("N2");
        }

        
        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!txtEntry.Text.Contains(".") & !IsInteger)
            {
                txtEntry.Focus();
                SendKeys.Send(((Button)sender).Text);
            }
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

        private void btnPlus_Click(object sender, EventArgs e)
        {
            decimal val;
            if (txtEntry.Text == "")
                return;
            val = decimal.Parse(txtEntry.Text);
            val = val + 1;            
            txtEntry.Text = val.ToString();
            txtEntry.Select();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            if (txtEntry.Text == "")
                return;
            decimal val;            
                val = decimal.Parse(txtEntry.Text);
                if (val >= 1)
                {
                    val = val - 1;
                }
                txtEntry.Text = val.ToString();
                txtEntry.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            value = txtEntry.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmKeypad_Load(object sender, EventArgs e)
        {
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

        private void frmKeypad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
                this.DialogResult = DialogResult.Cancel;
        }

    }
}