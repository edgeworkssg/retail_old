using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmKeyboard : Form
    {
        public string value;
        public string initialValue;
        public string textMessage;
        public bool IsInteger;
        public frmKeyboard()
        {
            InitializeComponent();
            IsInteger = false;
            CommonUILib.displayTransparent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtEntry.Select();
            SendKeys.Send(((Button)sender).Text);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            value = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void frmKeypad_Load(object sender, EventArgs e)
        {
            txtEntry.Text = initialValue;
            lblMessage.Text = textMessage;
        }

        private void frmKeypad_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonUILib.hideTransparent();
        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gbKeyboard.Controls.Count; i++)
            {
                if (gbKeyboard.Controls[i] is Button
                    && gbKeyboard.Controls[i].Tag != null
                    && gbKeyboard.Controls[i].Tag.ToString() != "")
                {
                    string tag = ((Button)gbKeyboard.Controls[i]).Tag.ToString();
                    string text = ((Button)gbKeyboard.Controls[i]).Text.ToString();

                    //swap
                    ((Button)gbKeyboard.Controls[i]).Tag = text;
                    ((Button)gbKeyboard.Controls[i]).Text = tag;
                }
            }
        }

        private void frmKeyboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnterAsOK), false))
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnOK_Click(btnOK, new EventArgs());
                }
            }
        }
    }
}