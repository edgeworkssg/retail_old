using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS.Container;
using PowerPOS;
using Features = PowerPOS.Feature;

namespace WinPowerPOS.OrderForms
{
    public partial class frmTrackDelivery : Form
    {

        DataTable dt = new DataTable();
        public PowerPOS.POSController pos;
        //PowerPOS.DateBlockingController DBlock;
        public bool IsSuccessful;

        public frmTrackDelivery()
        {
            InitializeComponent();
            //cmbType.Items.Add("---Please Select---");
            cmbType.Items.Add("Self Collection");
            cmbType.Items.Add("Delivery");
            cmbType.Items.Add("Installation");
            string status;
            ShiftTypesController.LoadShiftTypes(out status);            
        }

        private void frmTrackDelivery_Load(object sender, EventArgs e)
        {

            try
            {
                if (pos.MembershipApplied())
                {
                    txtAddress.Text = pos.GetMemberInfo().StreetName;
                    if (pos.GetMemberInfo().StreetName2 != null && pos.GetMemberInfo().StreetName2 != "")
                        txtAddress.Text = txtAddress.Text + Environment.NewLine + pos.GetMemberInfo().StreetName2;
                    if (pos.GetMemberInfo().City != null && pos.GetMemberInfo().City != "")
                        txtAddress.Text = txtAddress.Text + Environment.NewLine + pos.GetMemberInfo().City;
                    if (pos.GetMemberInfo().ZipCode != null && pos.GetMemberInfo().ZipCode != "")
                        txtAddress.Text = txtAddress.Text + "," + pos.GetMemberInfo().ZipCode;
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    PowerPOS.Logger.writeLog(X);
                    MessageBox.Show("Some error occurred. Please contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Type ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((cmbType.SelectedItem.ToString().ToLower() == "morning installation" || cmbType.SelectedItem.ToString().ToLower() == "afternoon installation" ||
                    cmbType.SelectedItem.ToString().ToLower() == "installation" || cmbType.SelectedItem.ToString().ToLower() == "delivery") &&
                    string.IsNullOrEmpty(txtAddress.Text))
            {
                if (!chkAdvise.Checked)
                {
                    MessageBox.Show("Please fill the Address ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string shift = "";
            if (CmbSHift.SelectedIndex != -1)
            {
                shift = CmbSHift.SelectedValue.ToString();
            }

            object dateValue = null;
            if (cmbType.SelectedItem.ToString().ToLower() == "self collection")
            {
                if (chkTaken.Checked)
                    dateValue = "TAKEN";
                else
                    dateValue = dateTimePicker1.Value;
            }
            else
            {
                if (chkAdvise.Checked)
                    dateValue = "ADVISED";
                else
                    dateValue = dateTimePicker1.Value;
            }

            //if (pos.AddDeliveryToOrder(txtRemarks.Text, txtAddress.Text, txtStoreReference.Text,txtDeliveryMode.Text))
            if (pos.AddDeliveryToOrder(txtRemarks.Text, txtAddress.Text, txtStoreReference.Text, cmbType.SelectedItem.ToString(), dateValue, shift))
            {
                IsSuccessful = true;
                this.Close();
            }

        }       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedItem.ToString() == "Self Collection")
            {
                chkAdvise.Enabled = false;
                chkTaken.Enabled = true;
                CmbSHift.SelectedIndex = -1;
                //CmbSHift.Items.Clear();
                // return;
            }
            else
            {
                chkAdvise.Enabled = true;
                chkTaken.Enabled = false;
            }

            DataTable dtSHift = new DataTable();
            dtSHift = ShiftTypesController.FetchShiftByID(cmbType.SelectedItem.ToString());
            CmbSHift.DataSource = dtSHift;
            CmbSHift.DisplayMember = "Name";
            CmbSHift.ValueMember = "Name";
            CmbSHift.Refresh();
            //string TYpe = CmbSHift.SelectedValue.ToString();


            //check date as well
            //dateTimePicker1_ValueChanged(sender, e);
            dateTimePicker1.MinDate = DateTime.Today;
        }
              

    }
}
