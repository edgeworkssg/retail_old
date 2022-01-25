using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Configuration;
using PowerPOS.Container;
using System.Linq;

namespace WinPowerPOS.MembershipForm
{
    public partial class frmMembershipQuickAddQuotation : Form
    {
        public string firstname;
        public string lastname;
        public string nric;
        public string membershipNo;
        public bool IsSuccessful;
        public bool applyPromo;
        public string mobileno;
        public PowerPOS.QuoteController pos;

        public frmMembershipQuickAddQuotation()
        {
            InitializeComponent();

            // Checking On Hold New Membership
            HoldController holdCtrl = new HoldController();
            DataTable dtHolded = holdCtrl.ToDataTable();

            if (dtHolded.Rows.Count > 0)
            {
                // Get Max Holded Membership No
                List<int> holdedIDs = new List<int>();
                foreach (DataRow drHolded in dtHolded.Rows)
                {
                    if (drHolded["MembershipNo"].ToString() != "<<NEW>>")
                    {
                        //holdedIDs.Add(int.Parse(drHolded["MembershipNo"].ToString().Substring(PointOfSaleInfo.MembershipPrefixCode.Length)));
                        string holdedMembershipNo = drHolded["MembershipNo"].ToString();
                        if (holdedMembershipNo.Length > PointOfSaleInfo.MembershipPrefixCode.Length)
                            holdedMembershipNo = holdedMembershipNo.Substring(PointOfSaleInfo.MembershipPrefixCode.Length);
                        int val = 0;
                        if (int.TryParse(holdedMembershipNo, out val))
                            holdedIDs.Add(val);
                    }
                }
                int maxHoldedMembershipNo = 0;
                if (holdedIDs.Count > 0)
                {
                    maxHoldedMembershipNo = holdedIDs.Max();
                }
                
                // Compare To GetNewMembershipNo Method
                string newMembershipNoStr = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                Logger.writeLog("Str " + newMembershipNoStr.Substring(PointOfSaleInfo.MembershipPrefixCode.Length));
                int newMembershipNo = int.Parse(newMembershipNoStr.Substring(PointOfSaleInfo.MembershipPrefixCode.Length));

                if (newMembershipNo > maxHoldedMembershipNo)
                {
                    txtMemberNo.Text = newMembershipNoStr;
                }
                else
                {
                    txtMemberNo.Text = PointOfSaleInfo.MembershipPrefixCode + (maxHoldedMembershipNo + 1).ToString().PadLeft(5, '0');
                }
                
                String tempMembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                if (String.Compare(tempMembershipNo,txtMemberNo.Text) > 0)
                {
                    txtMemberNo.Text = tempMembershipNo;
                }
            }
            else
            {
                //autogenerate membership number
                txtMemberNo.Text = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //membership number must be filled in
            if (string.IsNullOrEmpty(txtMemberNo.Text))
            {
                MessageBox.Show("Please fill the membership number", "Compulsary informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtNRIC.Text) && string.IsNullOrEmpty(txtMobile.Text))
            {
                MessageBox.Show("You have to fill either NRIC or Mobile No.", "Compulsary informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("NRIC is compulsary. Please fill up.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string duplicate;
            #region *) Validation: Check if member is already registered
            if (MembershipController.IsParticularAlreadyExist(txtMemberNo.Text, txtNRIC.Text, txtMobile.Text, out duplicate))
            {
                Membership m = new Membership(duplicate);
                string status;
                DialogResult dr = MessageBox.Show
                    ("Member's particular is exist in database as Member " + duplicate + ",\nName: " + m.NameToAppear + ".\nDo you want to use this card number?", "", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    if (pos.AssignMembership(duplicate, out status))
                    {
                        this.IsSuccessful = true;
                        this.Close();
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Error assigning membership " + duplicate + " ." + status);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            #endregion

             
            //Assign new member with detail info --- John harries
            if (pos.AssignNewMember(txtMemberNo.Text, txtFirstName.Text,
                    txtNRIC.Text, txtMobile.Text, txtHomeNo.Text,
                    ((MembershipGroup)cmbGroup.SelectedValue).MembershipGroupId,
                    dtpExpiryDate.Value, txtAddress_1.Text, txtAddress_2.Text, txtZipCode.Text,
                    txtCityName.Text, txtCountryName.Text, applyPromo, txtEmail.Text, txtFirst_Name.Text, txtLast_Name.Text, txtChineseName.Text,
                    txtChristianName.Text, dtpDOB.Value, dtpSubscriptionDate.Value, cbIsVitaMix.Checked, cbIsYoung.Checked, cbIsWaterFilter.Checked,
                    cbIsJuicePlus.Checked, txtRemark.Text, txtOccupation.Text, txtFax.Text, txtOffice.Text, txtCustomField1.Tag.ToString(), txtCustomField1.Text, txtCustomField2.Tag.ToString(), txtCustomField2.Text))
            {
                this.IsSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("An Error has been encountered. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //renew membership....

            //take payment...
        }

        private void frmMembershipQuickAdd_Load(object sender, EventArgs e)
        {
            int SubscriptionLengthInMonths = 12; //default
            if (AppSetting.GetSettingFromDBAndConfigFile("SubscriptionLengthInMonths") != null)
            {
                int.TryParse(AppSetting.GetSettingFromDBAndConfigFile("SubscriptionLengthInMonths").ToString(), out SubscriptionLengthInMonths);
            }
            Logger.writeLog(SubscriptionLengthInMonths.ToString());
            dtpExpiryDate.Value = DateTime.Now.AddMonths(SubscriptionLengthInMonths);
            PowerPOS.MembershipGroupCollection grp = new PowerPOS.MembershipGroupCollection();
            grp.Load();
            cmbGroup.DataSource = grp;
            cmbGroup.Refresh();

            MembershipCustomFieldCollection cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.OrderByAsc(MembershipCustomField.Columns.FieldName);
            cr.Load();

            if (cr != null && cr.Count > 0)
            {
                for (int i = 0; i < cr.Count; i++)
                {
                    //column name in membership table is stored after comma
                    //string field = cr[i].FieldName.Substring(cr[i].FieldName.LastIndexOf(',') + 1);
                    string[] field = cr[i].FieldName.Split(',');
                    //field name column name consist of 4 ((0)index,(1)fieldname,
                    //(2)column name in membership table, and (3)order it is saved in that field of membership table)

                    //get the value from field in membership table
                    string data = (string)(field[2]);

                    //if the data is empty (has never been processed before)
                    if (data != "" || data != null)
                    {

                        foreach (Control ctr in this.GBAdditionalInformation.Controls)
                        {
                            if (ctr is Label)
                            {
                                if (ctr.Name == "lblCustomField" + (i + 1).ToString())
                                {
                                    ctr.Text = cr[i].Label;
                                }
                            }
                            if (ctr is TextBox)
                            {
                                if (ctr.Name == "txtCustomField" + (i + 1).ToString())
                                {
                                    ctr.Tag = data;
                                }
                            }
                        }
                        
                    }

                    
                }

                for (int i = cr.Count; i < 5; i++)
                {
                    foreach (Control ctr in this.GBAdditionalInformation.Controls)
                    {
                        if (ctr is Label)
                        {
                            if (ctr.Name == "lblCustomField" + (i + 1).ToString())
                            {
                                ctr.Visible = false;
                            }
                        }
                        if (ctr is TextBox)
                        {
                            if (ctr.Name == "txtCustomField" + (i + 1).ToString())
                            {
                                ctr.Visible = false;
                                ctr.Tag = "";
                            }
                        }
                    }
                }
            }
            else
            {
                txtCustomField1.Tag = "";
                txtCustomField2.Tag = "";
                GBAdditionalInformation.Visible = false;
            }
        }

        private void txtNRIC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("\t");
                //Control currCtl = (Control)sender; //current control
                //e.Handled = true;
                //Control c = GetNextControl(currCtl, true);
                //c.Focus();
            }
        }

        private void txtRemark_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
                    }

        private void GBAdditionalInformation_Enter(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

    }
}