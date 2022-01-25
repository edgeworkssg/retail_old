using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using WinPowerPOS.OrderForms;
using PowerPOS.Container;

namespace WinPowerPOS.MembershipForms
{
    public partial class frmMembershipTap : Form
    {
        public POSController pos;
        public decimal value;
        public bool isSuccessful;
        public string remark;
        public frmMembershipTap()
        {
            InitializeComponent();
        }

        private void frmMembershipTap_Load(object sender, EventArgs e)
        {
            if (pos == null || pos.CurrentMember == null)
            {
                isSuccessful = false;
                this.Close();
                return;
            }
            
            string status;
            
            lblMember.Text = pos.CurrentMember.MembershipNo + ": " + pos.CurrentMember.NameToAppear;

            lblCurrentTapAmount.Text =
                 MembershipTapController.
                 checkMembershipTapAmount(pos.CurrentMember.MembershipNo, out status).ToString("N2");

            txtRemark.Select();
        }
        private void TakeKeypad(bool deduction)
        {
            if (txtRemark.Text == "")
            {
                MessageBox.Show("Please specify remark");
                txtRemark.Focus();
                return;
            }
            frmKeypad f = new frmKeypad();
            f.initialValue = "0";
            f.ShowDialog();
            
            //perform the transaction.....
            decimal takeValue=0;
            if (!decimal.TryParse(f.value, out takeValue))
            {
                MessageBox.Show("Error encountered. Please contact your administrator");
                isSuccessful = false;
                return;
            }
            f.Dispose();
            //perform action
            if (deduction)
            {
                if (takeValue > 0) takeValue = -takeValue;
            }            

            //Add Tap Item to Order....
            value = takeValue;
            remark = txtRemark.Text;
            isSuccessful = true;
            this.Close();
            
            /*
            decimal remaining;
            string status;
            if (MembershipTapController.adjustTap(pos.CurrentMember.MembershipNo, takeValue, UserInfo.username, out remaining, out status))
            {

            }
            */
        }

        private void btnPutDeposit_Click(object sender, EventArgs e)
        {
            TakeKeypad(bool.Parse(((Button)sender).Tag.ToString()));
        }
    }
}
