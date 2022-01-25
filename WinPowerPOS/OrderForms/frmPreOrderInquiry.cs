using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;
using System.Collections;
namespace WinPowerPOS.OrderForms
{
    public partial class frmPreOrderInquiry : Form
    {
        public POSController pos;
        public bool isSuccessful;
        public frmPreOrderInquiry()
        {
            isSuccessful = false;
            InitializeComponent();
            ArrayList a = PointOfSaleController.FetchOutletNames();
            a.RemoveAt(0);
            cmbOutlet.DataSource = a;
            cmbOutlet.Refresh();            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pos.setPreOrderInfo(txtFirstName.Text, txtMobile.Text, cmbOutlet.SelectedItem.ToString());
            isSuccessful = true;
            this.Close();
        }

        private void frmPreOrderInquiry_Load(object sender, EventArgs e)
        {
            if (pos.MembershipApplied())
            {
                txtFirstName.Text = pos.CurrentMember.NameToAppear;
                txtMobile.Text = pos.CurrentMember.Mobile;
            }
        }
    }
}
