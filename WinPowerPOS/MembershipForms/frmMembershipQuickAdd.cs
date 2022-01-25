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
    public partial class frmMembershipQuickAdd : Form
    {
        public string firstname;
        public string lastname;
        public string nric;
        public string membershipNo;
        public bool IsSuccessful;
        public bool applyPromo;
        public string mobileno;
        public PowerPOS.POSController pos;

        private void Fill_ComboBoxGender()
        {
            cmbGender_AP.Items.AddRange(new object[] { "", "M", "F" });
        }

        private void Deselect_DataGridView(DataGridView dgv)
        {
            dgv.ClearSelection();
            dgv.CurrentCell = null;
            ClearObjects_AP();
        }

        private void ClearObjects_AP()
        {
            txtFirstName_AP.Text = "";
            txtLastName_AP.Text = "";
            txtChristianName_AP.Text = "";
            txtChineseName_AP.Text = "";
            cmbGender_AP.ResetText();
            txtOccupation_AP.Text = "";
            dtpDOB_AP.Value = DateTime.Today;
        }

        public frmMembershipQuickAdd()
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
                        int tmpNo;
                        int.TryParse(drHolded["MembershipNo"].ToString().Replace(PointOfSaleInfo.MembershipPrefixCode, ""), out tmpNo);
                        holdedIDs.Add(tmpNo);
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
                if (String.Compare(tempMembershipNo, txtMemberNo.Text) > 0)
                {
                    txtMemberNo.Text = tempMembershipNo;
                }
            }
            else
            {
                //autogenerate membership number
                txtMemberNo.Text = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
            }

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                this.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Add";
                label14.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper() + " NO";
                groupBox1.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Information";
                tabControl1.TabPages[0].Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Quick Add";
                tabControl1.TabPages[1].Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Additional Info";
                tabControl1.TabPages[3].Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Pass Code";
            }
            else
            {
                this.Text = "Membership Add";
                label14.Text = "MEMBERSHIP NO";
                groupBox1.Text = "Membership Information";
                tabControl1.TabPages[0].Text = "Membership Quick Add";
                tabControl1.TabPages[1].Text = "Membership Additional Info";
                tabControl1.TabPages[3].Text = "Membership Pass Code";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pos.CancelNewMember();
            IsSuccessful = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (chkIsCompulsory.Checked)
            {
                if (string.IsNullOrEmpty(txtEmail.Text) || !txtEmail.Text.IsValidEmail())
                {
                    MessageBox.Show("Please fill valid email", "Compulsary informations are missing", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
            }

            //membership number must be filled in
            string warningfillmembershipnumber = "Please fill the membership number";
            string memberString = "Member";
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                warningfillmembershipnumber = string.Format("Please fill the {0} number", AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString());
                memberString = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString();
            }

            if (string.IsNullOrEmpty(txtMemberNo.Text))
            {
                MessageBox.Show(warningfillmembershipnumber, "Compulsory informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtNRIC.Text) && string.IsNullOrEmpty(txtMobile.Text))
            {
                MessageBox.Show("You have to fill either NRIC or Mobile No.", "Compulsory informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //MessageBox.Show("NRIC is compulsory. Please fill up.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AskPassCodeWhenCreateNewMember), true))
            {
                if (string.IsNullOrEmpty(txtPassCode.Text))
                {
                    if (MessageBox.Show("Are you sure to leave the Pass Code empty ?", "Compulsary informations are missing",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    if (txtPassCode.Text != txtConfirmPassCode.Text)
                    {
                        MessageBox.Show("Pass Code not same. Please check your Pass Code", "Confirm Pass Code",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            string duplicate;
            #region *) Validation: Check if member is already registered
            if (MembershipController.IsParticularAlreadyExist(txtMemberNo.Text, txtNRIC.Text, txtMobile.Text, out duplicate))
            {
                Membership m = new Membership(duplicate);
                string status;
                DialogResult dr = MessageBox.Show
                    (memberString + "'s particular is exist in database as " + memberString + " " + duplicate + ",\nName: " + m.NameToAppear + ".\nDo you want to use this card number?", "", MessageBoxButtons.YesNo);

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
                        MessageBox.Show("Error assigning " + memberString + " " + duplicate + " ." + status);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            #endregion

            if (MembershipController.IsEmailExist(txtEmail.Text))
            {
                MessageBox.Show("Email has been used. Please use another email");
                return;
            }

            //Assign new member with detail info --- John harries
            if (pos.AssignNewMember(txtMemberNo.Text, txtFirstName.Text,
                    txtNRIC.Text, txtMobile.Text, txtHomeNo.Text,
                    (int)cmbGroup.SelectedValue,
                    dtpExpiryDate.Value, txtAddress_1.Text, txtAddress_2.Text, txtZipCode.Text,
                    txtCityName.Text, txtCountryName.Text, applyPromo, txtEmail.Text, txtFirst_Name.Text, txtLast_Name.Text, txtChineseName.Text,
                    txtChristianName.Text, dtpDOB.Value, dtpSubscriptionDate.Value, cbIsVitaMix.Checked, cbIsYoung.Checked, cbIsWaterFilter.Checked,
                    cbIsJuicePlus.Checked, txtRemark.Text, txtOccupation.Text, txtFax.Text, txtOffice.Text, txtCustomField1.Tag.ToString(),
                    txtCustomField1.Text, txtCustomField2.Tag.ToString(), txtCustomField2.Text, cmbGender.Text, txtPassCode.Text))
            {
                //validation warning
                List<string> EmptyMandatoryFields;
                if (!pos.ValidateMembership(pos.GetMemberInfo(), out EmptyMandatoryFields))
                {
                    string separator = System.Environment.NewLine + "- ";
                    DialogResult dialogResult = MessageBox.Show("These fields are important: " + separator + string.Join(separator, EmptyMandatoryFields.ToArray()) + ". Are you sure you want to register without filling these details?", "Compulsory Fields", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }

                pos.CurrentAttachedParticular = (AttachedParticularCollection)dgvAttachedParticular.DataSource;

                //if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowMembershipWarning), false))
                //{
                //    string EmptyMandatoryFields = "";
                //    string fields = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipWarningFields);
                //    if (fields != null && fields != "")
                //    {
                //        string[] result = fields.Split(';');
                //        Membership member = pos.GetMemberInfo();
                //        foreach (string s in result)
                //        {
                //            if (member.GetColumnValue(s) == null || member.GetColumnValue(s).ToString() == "")
                //            {
                //                EmptyMandatoryFields += System.Environment.NewLine + "- " + s;
                //            }
                //        }

                //        if (EmptyMandatoryFields.Length > 0)
                //        {
                //            MessageBox.Show("You have to fill these fields : " + EmptyMandatoryFields, "Compulsory informations are missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //            return;
                //        }
                //    }
                //}

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
            cmbGroup.DataSource = grp.OrderBy(o => o.GroupName).ToList();
            cmbGroup.DisplayMember = "GroupName";
            cmbGroup.ValueMember = "MembershipGroupId";
            

            string defaultmembergroup = AppSetting.GetSetting(AppSetting.SettingsName.Membership.DefaultMembershipGroup);
            if (!string.IsNullOrEmpty(defaultmembergroup))
            {
                cmbGroup.SelectedIndex = cmbGroup.FindString(defaultmembergroup);

                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AllowEditMemberGroup), false))
                    cmbGroup.Enabled = false;
            }

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

            Fill_ComboBoxGender();
            dtpDOB_AP.Value = DateTime.Today;
            dgvAttachedParticular.Columns["colDateOfBirth"].DefaultCellStyle.Format = "dd MMM yyyy";

            // Bind Attached Particular
            AttachedParticularCollection collAP = new AttachedParticularCollection();
            dgvAttachedParticular.AutoGenerateColumns = false;
            dgvAttachedParticular.DataSource = collAP;
            dgvAttachedParticular.Refresh();

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.Membership_ShowAttachedParticular), false) == false)
            {
                tabControl1.TabPages.Remove(tpAttachedParticular);
                //tpAttachedParticular.Hide();
            }

            chkIsCompulsory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.EmailCompulsoryIsTicked), false);
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

        private void dgvAttachedParticular_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAttachedParticular.CurrentRow != null)
            {
                if (dgvAttachedParticular.SelectedRows.Count == 0 || dgvAttachedParticular.SelectedRows[0].DataBoundItem == null)
                {
                    ClearObjects_AP();
                }
                else
                {
                    AttachedParticular ap = (AttachedParticular)dgvAttachedParticular.CurrentRow.DataBoundItem;
                    txtFirstName_AP.Text = ap.FirstName;
                    txtLastName_AP.Text = ap.LastName;
                    txtChristianName_AP.Text = ap.ChristianName;
                    txtChineseName_AP.Text = ap.ChineseName;
                    cmbGender_AP.SelectedItem = ap.Gender;
                    txtOccupation_AP.Text = ap.Occupation;
                    dtpDOB_AP.Value = ap.DateOfBirth.GetValueOrDefault();
                }
            }
        }

        private void btnOK_AP_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirstName_AP.Text))
            {
                MessageBox.Show("Please enter First Name.");
                return;
            }

            AttachedParticular ap;
            if (dgvAttachedParticular.SelectedRows.Count == 0 || dgvAttachedParticular.SelectedRows[0].DataBoundItem == null)
            {
                ap = new AttachedParticular();
                ap.FirstName = txtFirstName_AP.Text;
                ap.LastName = txtLastName_AP.Text;
                ap.ChristianName = txtChristianName_AP.Text;
                ap.ChineseName = txtChineseName_AP.Text;
                ap.Gender = cmbGender_AP.GetItemText(cmbGender_AP.SelectedItem);
                ap.Occupation = txtOccupation_AP.Text;
                ap.DateOfBirth = dtpDOB_AP.Value;
                ap.Deleted = false;
                ap.UniqueID = Guid.NewGuid();
                ((AttachedParticularCollection)dgvAttachedParticular.DataSource).Add(ap);
                dgvAttachedParticular.Refresh();
            }
            else
            {
                ap = (AttachedParticular)dgvAttachedParticular.SelectedRows[0].DataBoundItem;
                ap.FirstName = txtFirstName_AP.Text;
                ap.LastName = txtLastName_AP.Text;
                ap.ChristianName = txtChristianName_AP.Text;
                ap.ChineseName = txtChineseName_AP.Text;
                ap.Gender = cmbGender_AP.GetItemText(cmbGender_AP.SelectedItem);
                ap.Occupation = txtOccupation_AP.Text;
                ap.DateOfBirth = dtpDOB_AP.Value;
                dgvAttachedParticular.Refresh();
            }

            Deselect_DataGridView(dgvAttachedParticular);
        }

        private void btnCancel_AP_Click(object sender, EventArgs e)
        {
            Deselect_DataGridView(dgvAttachedParticular);
        }

        private void dgvAttachedParticular_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAttachedParticular.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (dgvAttachedParticular.Columns[e.ColumnIndex].Name == "colDelete" && dgvAttachedParticular.Rows[e.RowIndex].DataBoundItem != null)
                {
                    dgvAttachedParticular.Rows.RemoveAt(e.RowIndex);
                    Deselect_DataGridView(dgvAttachedParticular);
                    dgvAttachedParticular.Refresh();
                }
            }
        }

        private void txtZipCode_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtZipCode.Modified)
                {
                    if (txtAddress_1.Text != "" || txtAddress_2.Text != "")
                    {
                        if (MessageBox.Show("Do you want to replace the address?", "Replace Address", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            txtZipCode.Modified = false;
                            return;
                        }
                    }

                    if (string.IsNullOrEmpty(txtZipCode.Text))
                    {
                        txtAddress_1.Text = "";
                        txtAddress_2.Text = "";
                        txtCityName.Text = "";
                        txtCountryName.Text = "";
                        txtZipCode.Tag = null;
                        txtZipCode.Modified = false;
                    }
                    else
                    {
                        PostalCodeDB postalCode = new PostalCodeDB(txtZipCode.Text);
                        if (string.IsNullOrEmpty(postalCode.Zip))
                        {
                            MessageBox.Show("Postal Code not found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtZipCode.Tag = null;
                            txtAddress_1.Text = "";
                            txtAddress_2.Text = "";
                            txtCityName.Text = "";
                            txtCountryName.Text = "";
                        }
                        else
                        {
                            txtAddress_1.Text = postalCode.Area1;
                            txtAddress_2.Text = postalCode.Area2;
                            txtCityName.Text = postalCode.City;
                            txtCountryName.Text = postalCode.Country;
                            txtZipCode.Tag = postalCode.City;
                            txtZipCode.Modified = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show(
                    "Some error occurred. Please contact your administrator.", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}