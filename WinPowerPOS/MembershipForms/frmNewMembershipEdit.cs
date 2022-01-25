using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;
using System.Globalization;

namespace WinPowerPOS.MembershipForms
{

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                //case null: throw new ArgumentNullException(nameof(input));
                //case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }

    public partial class frmNewMembershipEdit : Form
    {
        private const int CONTROL_SIZE = 150;
        private char[] chrArr = { ',' };
        private Label lbl;
        private MembershipCustomFieldCollection cr;
        public bool IsReadOnly;
        public Membership EditedMember;
        public bool IsUpdated = false;

        public frmNewMembershipEdit(Membership member)
        {
            InitializeComponent();
            IsReadOnly = false;
            DataTable dt = MembershipController.FetchMembershipGroupList();
            cmbGroupName.DataSource = dt;
            cmbGroupName.DisplayMember = "GroupName";
            cmbGroupName.ValueMember = "MembershipGroupID";
            EditedMember = member;
            LoadData();
        }

        public frmNewMembershipEdit()
        {
            InitializeComponent();
            IsReadOnly = false;
            DataTable dt = MembershipController.FetchMembershipGroupList();
            cmbGroupName.DataSource = dt;
            cmbGroupName.DisplayMember = "GroupName";
            cmbGroupName.ValueMember = "MembershipGroupID";
            EditedMember = new Membership();
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
                    EditedMember.MembershipNo = newMembershipNoStr;
                }
                else
                {
                    EditedMember.MembershipNo = PointOfSaleInfo.MembershipPrefixCode + (maxHoldedMembershipNo + 1).ToString().PadLeft(5, '0');
                }

                String tempMembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                if (String.Compare(tempMembershipNo, EditedMember.MembershipNo) > 0)
                {
                    EditedMember.MembershipNo = tempMembershipNo;
                }
            }
            else
            {
                //autogenerate membership number
                EditedMember.MembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
            }
            txtMembershipNo.Text = EditedMember.MembershipNo;
            dtpExpiryDate.Value = DateTime.Today.AddYears(100);
            dtpSubscriptionDate.Value = DateTime.Today;

            cmbNationality.SelectedIndex = cmbNationality.Items.IndexOf("Singaporean");

            //LoadData();
        }
        #region *) Method

        private void LoadData()
        {
            try
            {
                LoadAdditionalControls();

                txtChristianName.Text = EditedMember.ChristianName;
                txtChineseName.Text = EditedMember.ChineseName;
                txtAddress1.Text = EditedMember.StreetName2;
                txtAddress.Text = EditedMember.StreetName;
                txtCity.Text = EditedMember.City;
                txtCountry.Text = EditedMember.Country;
                txtEmail.Text = EditedMember.Email;
                txtFax.Text = EditedMember.Fax;
                txtFirstName.Text = EditedMember.FirstName;
                txtHome.Text = EditedMember.Home;
                txtLastName.Text = EditedMember.LastName;
                txtMembershipNo.Text = EditedMember.MembershipNo;
                txtMobile.Text = EditedMember.Mobile;
                txtNameToAppear.Text = EditedMember.NameToAppear;
                txtNRIC.Text = EditedMember.Nric;
                txtOccupation.Text = EditedMember.Occupation;
                txtOffice.Text = EditedMember.Office;
                txtZipCode.Text = EditedMember.ZipCode;

                cmbGroupName.SelectedValue = EditedMember.MembershipGroupId;
                TextInfo info = CultureInfo.CurrentCulture.TextInfo;               

                cmbNationality.SelectedIndex = string.IsNullOrEmpty(EditedMember.Nationality) ? cmbNationality.Items.IndexOf("Singaporean") : cmbNationality.Items.IndexOf(info.ToTitleCase(EditedMember.Nationality.ToLower()));

                cmbGender.SelectedItem = EditedMember.Gender;
                if (cmbGender.SelectedIndex == -1)
                    cmbGender.SelectedIndex = 0;
                if (EditedMember.IsJuicePlus.HasValue)
                    cbIsJuicePlus.Checked = EditedMember.IsJuicePlus.Value;
                if (EditedMember.IsVitaMix.HasValue)
                    cbIsVitaMix.Checked = EditedMember.IsVitaMix.Value;
                if (EditedMember.IsWaterFilter.HasValue)
                    cbIsWaterFilter.Checked = EditedMember.IsWaterFilter.Value;
                if (EditedMember.IsYoung.HasValue)
                    cbIsYoung.Checked = EditedMember.IsYoung.Value;
                txtRemark.Text = EditedMember.Remarks;
                if (EditedMember.MembershipGroup != null)
                    cmbGroupName.SelectedItem = EditedMember.MembershipGroup.GroupName;
                if (cmbGroupName.SelectedIndex == -1)
                    cmbGroupName.SelectedIndex = 0;
                if (EditedMember.DateOfBirth.HasValue)
                    dtpDOB.Value = EditedMember.DateOfBirth.Value;
                if (EditedMember.ExpiryDate.HasValue)
                    dtpExpiryDate.Value = EditedMember.ExpiryDate.Value;
                if (EditedMember.SubscriptionDate.HasValue)
                    dtpSubscriptionDate.Value = EditedMember.SubscriptionDate.Value;

                if (string.IsNullOrEmpty(EditedMember.TagNo))
                {
                    txtTagNo.Text = "";
                    txtTagNo.Enabled = true;
                    btnTagCard.Text = "TAG CARD";
                    btnTagCard.BackgroundImage = WinPowerPOS.Properties.Resources.blueButton;
                }
                else
                {
                    txtTagNo.Text = EditedMember.TagNo;
                    txtTagNo.Enabled = false;
                    btnTagCard.Text = "UNTAG CARD";
                    btnTagCard.BackgroundImage = WinPowerPOS.Properties.Resources.redbutton;
                }

                LoadAdditionalInformation();
                if (IsReadOnly)
                    SetToReadOnly();
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.Membership_ShowAttachedParticular), false) == false)
                    tabControl1.TabPages.Remove(tpAttachedParticular);
                dgvAttachedParticular.Columns["colDateOfBirth"].DefaultCellStyle.Format = "dd MMM yyyy";
                // Bind Attached Particular
                AttachedParticularCollection collAP = new AttachedParticularCollection();
                collAP.Where("MembershipNo", EditedMember.MembershipNo);
                collAP.Where("Deleted", false);
                collAP.Load();
                dgvAttachedParticular.AutoGenerateColumns = false;
                dgvAttachedParticular.DataSource = collAP;
                dgvAttachedParticular.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void SetToReadOnly()
        {
            txtChineseName.Enabled = false;
            txtChristianName.Enabled = false;
            txtAddress1.Enabled = false;
            dtpDOB.Enabled = false;
            dtpExpiryDate.Enabled = false;
            dtpSubscriptionDate.Enabled = false;
            cmbGender.Enabled = false;
            cmbGroupName.Enabled = false;
            cbIsJuicePlus.Enabled = false;
            cbIsVitaMix.Enabled = false;
            cbIsWaterFilter.Enabled = false;
            cbIsYoung.Enabled = false;
            txtRemark.Enabled = false;
            txtAddress.ReadOnly = true;
            txtCity.ReadOnly = true;
            txtCountry.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtFax.ReadOnly = true;
            txtFirstName.ReadOnly = true;
            txtHome.ReadOnly = true;
            txtLastName.ReadOnly = true;
            txtMembershipNo.ReadOnly = true;
            //txtMinistry.ReadOnly = true;
            txtMobile.ReadOnly = true;
            txtNameToAppear.ReadOnly = true;
            txtNRIC.ReadOnly = true;
            txtOccupation.ReadOnly = true;
            txtOffice.ReadOnly = true;
            //txtRemark.ReadOnly = true;
            txtZipCode.ReadOnly = true;
            //cbIsCHC.Enabled = false;
            //cbIsStudentCard.Enabled = false;
            cmbNationality.Enabled = false;
            btnSave.Enabled = false;
        }

        private void LoadAdditionalInformation()
        {
            //
            if (EditedMember != null && EditedMember.IsLoaded && cr != null && cr.Count > 0)
            {
                for (int i = 0; i < cr.Count; i++)
                {
                    Control ctrl = tbMembershipAdditional.Controls.Find(cr[i].FieldName, true)[0];
                    string[] field = cr[i].FieldName.Split(',');
                    //field name column name consist of 4 ((0)index,(1)fieldname,
                    //(2)column name in membership table, and (3)order it is saved in that field of membership table)

                    //get the value from field in membership table
                    string data = (string)EditedMember.GetColumnValue(field[2]);

                    string value = "";
                    if (data != "" && data != null)
                    {
                        string[] splitData = data.Split(',');
                        if (splitData.Length > 1)
                        {
                            value =
                                splitData[Convert.ToInt32(field[3]) - 1].Substring(
                                    splitData[Convert.ToInt32(field[3]) - 1].IndexOf(':') + 1);
                        }
                    }

                    if (value != "" && ctrl != null)
                    {
                        value = value.Substring(1, value.Length - 2); //stripping the ""

                        switch (cr[i].Type.ToLower())
                        {
                            case "string":
                                //create textbox
                                ((TextBox)ctrl).Text = value;
                                break;

                            case "boolean":
                                //create checkbox
                                if (value != "False")
                                    ((CheckBox)ctrl).Checked = true;
                                else
                                    ((CheckBox)ctrl).Checked = false;

                                break;

                            case "enum":
                                //create combobox               
                                ((ComboBox)ctrl).SelectedItem = value;
                                break;

                            case "date":
                                //create combobox              
                                DateTime tmpDate;
                                if (DateTime.TryParse(value.ToString(), out tmpDate))
                                {
                                    ((DateTimePicker)ctrl).Value = tmpDate;
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void SaveAdditionalInformation()
        {
            #region *) OBSOLETE
            //if (EditedMember != null && EditedMember.IsLoaded && cr != null && cr.Count > 0)
            //{
            //    for (int i = 0; i < cr.Count; i++)
            //    {
            //        Control ctrl = tbMembershipAdditional.Controls.Find(cr[i].FieldName, true)[0];
            //        //object value = mbr.GetColumnValue(cr[i].FieldName);
            //        if (ctrl != null)
            //        {
            //            switch (cr[i].Type.ToLower())
            //            {
            //                case "string":
            //                    //create textbox
            //                    EditedMember.SetColumnValue(cr[i].FieldName, ((TextBox)ctrl).Text);
            //                    break;

            //                case "boolean":
            //                    //create checkbox
            //                    EditedMember.SetColumnValue(cr[i].FieldName, ((CheckBox)ctrl).Checked);
            //                    break;

            //                case "enum":
            //                    //create combobox               
            //                    EditedMember.SetColumnValue(cr[i].FieldName, ((ComboBox)ctrl).SelectedValue);
            //                    break;

            //                case "date":
            //                    //create combobox               
            //                    EditedMember.SetColumnValue(cr[i].FieldName, ((DateTimePicker)ctrl).Value.ToString("yyyy-MM-dd HH:mm:ss"));
            //                    break;
            //            }
            //        }
            //    }
            //}
            #endregion

            Membership mbr = EditedMember;

            if (mbr != null && cr != null && cr.Count > 0)
            {
                for (int i = 0; i < cr.Count; i++)
                {
                    if (i >= 2) break; // for this form we only show 2 additional fields

                    //column name in membership table is stored after comma
                    //string field = cr[i].FieldName.Substring(cr[i].FieldName.LastIndexOf(',') + 1);
                    string[] field = cr[i].FieldName.Split(',');
                    //field name column name consist of 4 ((0)index,(1)fieldname,
                    //(2)column name in membership table, and (3)order it is saved in that field of membership table)

                    //get the value from field in membership table
                    string data = (string)mbr.GetColumnValue(field[2]);

                    //if the data is empty (has never been processed before)
                    if (data == "" || data == null)
                    {
                        for (int j = 0; j < cr.Count; j++)
                        {
                            string[] field2 = cr[j].FieldName.Split(',');
                            if (field2[2] == field[2]) //if it is the same, concatenate to populate the field
                            {
                                string toBeWritten = '"' + field2[1] + '"' + ":" + ",";
                                data = string.Concat(toBeWritten);
                                mbr.SetColumnValue(field2[2], data);
                            }
                        }
                    }

                    //there's value now
                    string[] splitData = data.Split(',');
                    if (splitData != null && tbMembershipAdditional.Controls.Find(cr[i].FieldName, true).Length > 0)
                    {
                        switch (cr[i].Type.ToLower())
                        {
                            //only change the word where it is stored
                            case "string":
                                //save value from textbox

                                splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + ((TextBox)tbMembershipAdditional.Controls.Find(cr[i].FieldName, true)[0]).Text + '"';
                                break;

                            case "boolean":
                                //save value from checkbox
                                splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + ((CheckBox)tbMembershipAdditional.Controls.Find(cr[i].FieldName, true)[0]).Checked + '"';
                                //mbr.SetColumnValue(field, ((CheckBox)ctrl[i]).Checked);
                                break;

                            case "enum":
                                //save value from combobox  
                                splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + ((ComboBox)tbMembershipAdditional.Controls.Find(cr[i].FieldName, true)[0]).SelectedItem + '"';
                                //mbr.SetColumnValue(field, ((DropDownList)ctrl[i]).SelectedValue);
                                break;

                            case "date":
                                //save value from date textbox    
                                DateTime dateOfBirth;
                                if (DateTime.TryParse(((TextBox)tbMembershipAdditional.Controls.Find(cr[i].FieldName, true)[0]).Text, out dateOfBirth))
                                    splitData[Convert.ToInt32(field[3]) - 1] = '"' + field[1] + '"' + ":" + '"' + dateOfBirth.ToString("dd MMM yyyy") + '"';
                                //mbr.SetColumnValue(field, dateOfBirth.ToString("dd MMM yyyy"));
                                break;

                            //case default:
                            //    break;
                        }

                        data = ""; //clear data string
                        //fill in with new value
                        data = string.Join(",", splitData);
                        //write to the column in tmembership table
                        mbr.SetColumnValue(field[2], data);
                    }
                }
            }
        }

        private void LoadAdditionalControls()
        {
            cr = new MembershipCustomFieldCollection();
            cr.Where(MembershipCustomField.Columns.Deleted, false);
            cr.OrderByAsc(MembershipCustomField.Columns.FieldName);
            cr.Load();

            int rowIndex = 0, colIndex = 0;
            for (int i = 0; i < cr.Count; i++)
            {
                switch (cr[i].Type.ToLower())
                {
                    case "string":
                        //create textbox
                        lbl = new Label();
                        lbl.Text = cr[i].Label;
                        lbl.Padding = new Padding(3);
                        lbl.Width = CONTROL_SIZE;
                        TextBox txt = new TextBox();
                        txt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        txt.Name = cr[i].FieldName;
                        txt.Tag = "";
                        txt.Width = CONTROL_SIZE;
                        tbMembershipAdditional.Controls.Add(lbl, colIndex, rowIndex);
                        tbMembershipAdditional.Controls.Add(txt, colIndex + 1, rowIndex);
                        break;

                    case "boolean":
                        //create checkbox
                        lbl = new Label();
                        lbl.Text = cr[i].Label;
                        lbl.Padding = new Padding(3);
                        lbl.Width = CONTROL_SIZE;
                        CheckBox cb = new CheckBox();
                        cb.Name = cr[i].FieldName;
                        cb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        cb.Tag = "";
                        cb.Width = CONTROL_SIZE;
                        tbMembershipAdditional.Controls.Add(lbl, colIndex, rowIndex);
                        tbMembershipAdditional.Controls.Add(cb, colIndex + 1, rowIndex);
                        break;

                    case "enum":
                        //create combobox                        
                        lbl = new Label();
                        lbl.Text = cr[i].Label;
                        lbl.Padding = new Padding(3);
                        lbl.Width = CONTROL_SIZE;
                        ComboBox cmb = new ComboBox();
                        cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        cmb.Name = cr[i].FieldName;
                        cmb.Tag = "";
                        cmb.Width = CONTROL_SIZE;
                        string[] enumItems = cr[i].EnumList.Split(chrArr);
                        for (int j = 0; j < enumItems.Length; j++)
                        {
                            //enumItems[j] = enumItems[j].Trim();
                            cmb.Items.Add(enumItems[j].Trim());
                        }
                        //cmb.DataSource = enumItems;
                        //cmb.Refresh();
                        tbMembershipAdditional.Controls.Add(lbl, colIndex, rowIndex);
                        tbMembershipAdditional.Controls.Add(cmb, colIndex + 1, rowIndex);
                        break;
                    case "date":
                        //create combobox                        
                        lbl = new Label();
                        lbl.Text = cr[i].Label;
                        lbl.Padding = new Padding(3);
                        lbl.Width = CONTROL_SIZE;
                        DateTimePicker dtp = new DateTimePicker();
                        dtp.Name = cr[i].FieldName;
                        dtp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                        dtp.Format = DateTimePickerFormat.Custom;
                        dtp.CustomFormat = "dd MMM yyyy HH:mm:ss";
                        dtp.Tag = "";
                        dtp.Width = CONTROL_SIZE;
                        tbMembershipAdditional.Controls.Add(lbl, colIndex, rowIndex);
                        tbMembershipAdditional.Controls.Add(dtp, colIndex + 1, rowIndex);
                        break;
                }
                colIndex += 3;
                if (colIndex > 3)
                {
                    colIndex = 0;
                    rowIndex += 1;
                }
            }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbGroupName.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a membership group");
                cmbGroupName.Select();
                return;
            }

            if (EditedMember.TagNo == txtNRIC.Text)
            {
                MessageBox.Show("Tag No should not be similar with NRIC");
                return;
            }
            EditedMember.StreetName = txtAddress.Text; //.ToUpper();
            EditedMember.StreetName2 = txtAddress1.Text; //.ToUpper();
            EditedMember.City = txtCity.Text; //.ToUpper();
            EditedMember.Country = txtCountry.Text; //.ToUpper();
            EditedMember.Email = txtEmail.Text;
            EditedMember.Fax = txtFax.Text; //.ToUpper();
            EditedMember.FirstName = txtFirstName.Text; //.ToUpper();
            EditedMember.Home = txtHome.Text; //.ToUpper();
            EditedMember.LastName = txtLastName.Text; //.ToUpper();
            EditedMember.ChineseName = txtChineseName.Text;
            EditedMember.ChristianName = txtChristianName.Text;
            EditedMember.Mobile = txtMobile.Text; //.ToUpper();
            EditedMember.NameToAppear = txtNameToAppear.Text; //.ToUpper();
            EditedMember.Nric = txtNRIC.Text; //.ToUpper();
            EditedMember.Occupation = txtOccupation.Text; //.ToUpper();
            EditedMember.Office = txtOffice.Text; //.ToUpper();
            EditedMember.ZipCode = txtZipCode.Text; //.ToUpper();

            EditedMember.MembershipGroupId = int.Parse(cmbGroupName.SelectedValue.ToString());
            if (cmbGender.SelectedItem != null)
                EditedMember.Gender = cmbGender.SelectedItem.ToString().ToUpper();
            else
                EditedMember.Gender = "M";

            EditedMember.IsYoung = cbIsYoung.Checked;
            EditedMember.IsWaterFilter = cbIsWaterFilter.Checked;
            EditedMember.IsVitaMix = cbIsVitaMix.Checked;
            EditedMember.IsJuicePlus = cbIsJuicePlus.Checked;
            EditedMember.Remarks = txtRemark.Text;

            if (DateTime.Compare(dtpDOB.Value, DateTime.Today.Date) < 0)
                EditedMember.DateOfBirth = dtpDOB.Value;
            EditedMember.ExpiryDate = dtpExpiryDate.Value;
            EditedMember.SubscriptionDate = dtpSubscriptionDate.Value;

            if (cmbNationality.SelectedItem != null)
                EditedMember.Nationality = cmbNationality.SelectedItem.ToString().ToUpper();
            else
                EditedMember.Nationality = "Singaporean";

            SaveAdditionalInformation();

            if (EditedMember.IsNew)
            {
                EditedMember.Deleted = false;
                EditedMember.UniqueID = Guid.NewGuid();
                IsUpdated = true;
                this.Close();
            }
            else
            {
                MembershipCollection mColl = new MembershipCollection();
                mColl.Add(EditedMember);
                string status = "";
                bool isSuccess = MembershipController.UpdateMemberData(mColl.ToDataTable(), out status);
                if (!isSuccess)
                    MessageBox.Show(status);
                else
                {
                    IsUpdated = true;
                    this.Close();
                }
            }
        }

        private void btnTagCard_Click(object sender, EventArgs e)
        {
            bool IsApproved = false;
            if (btnTagCard.Text == "UNTAG CARD")
            {
                DialogResult dr = MessageBox.Show("Are you sure want to untag card ?", "WARNING", MessageBoxButtons.YesNo);

                if (dr == DialogResult.Yes)
                {
                    IsApproved = true;
                    EditedMember.TagNo = "";
                }
            }
            else
            {
                string duplicate;
                if (String.IsNullOrEmpty(txtTagNo.Text))
                {
                    MessageBox.Show("Please fill/scan Tag No !");
                    txtTagNo.Text = "";
                    return;
                }

                MembershipController.IsTagNoSameWithNRIC(txtTagNo.Text, EditedMember.MembershipNo, out duplicate);
                if (!string.IsNullOrEmpty(duplicate))
                {
                    MessageBox.Show("Tag No similar with NRIC Member " + duplicate + ". Please choose another tag no !");
                    txtTagNo.Text = "";
                    return;
                }

                duplicate = "";
                MembershipController.IsTagNoAlreadyExist(txtTagNo.Text, EditedMember.MembershipNo, out duplicate);
                if (!string.IsNullOrEmpty(duplicate))
                {
                    MessageBox.Show("Tag No already used with Member with No :" + duplicate);
                    txtTagNo.Text = "";
                    return;

                }
                IsApproved = true;
                EditedMember.TagNo = txtTagNo.Text;

            }

            if (IsApproved)
            {

                if (EditedMember.IsNew)
                {
                    EditedMember.Deleted = false;
                    EditedMember.UniqueID = Guid.NewGuid();
                    IsUpdated = true;
                    MessageBox.Show("Tag / Untag No done succesfully");
                    this.Close();
                }
                else
                {
                    MembershipCollection mColl = new MembershipCollection();
                    mColl.Add(EditedMember);
                    string status = "";
                    bool isSuccess = MembershipController.UpdateMemberTagNo(EditedMember.MembershipNo, EditedMember.TagNo, out status);
                    if (!isSuccess)
                        MessageBox.Show(status);
                    else
                    {
                        MessageBox.Show("Tag / Untag No done succesfully");
                        IsUpdated = true;
                        this.Close();
                    }
                }
            }
        }

        private void txtTagNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnTagCard_Click(sender, e);
            }
        }

        private void frmNewMembershipEdit_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} Details", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Membership");
            lblMembershipNo.Text = string.Format("{0} No", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Membership");
            groupBox1.Text = string.Format("{0} Information", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Membership");
        }

    }
}
