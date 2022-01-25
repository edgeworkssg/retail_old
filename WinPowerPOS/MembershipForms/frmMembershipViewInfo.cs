using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using SubSonic;

namespace WinPowerPOS.MembershipForms
{
    /// <summary>
    /// UI to view membership detail and update remark. (created by John Harries)
    /// </summary>
    public partial class frmMembershipViewInfo : Form
    {
        
        public frmMembershipViewInfo(string membershipNo)
        {
            InitializeComponent();
            loadMembershipDetail(membershipNo);
        }

        private void loadMembershipDetail(string membershipNo)
        {
            Membership membership = new Membership(membershipNo);

            this.txtAddress.Text = !string.IsNullOrEmpty(membership.StreetName) ? membership.StreetName : string.Empty;
            this.txtAddress1.Text = !string.IsNullOrEmpty(membership.StreetName2) ? membership.StreetName2 : string.Empty;
            this.txtChineseName.Text = !string.IsNullOrEmpty(membership.ChineseName) ? membership.ChineseName : string.Empty;
            this.txtChristianName.Text = !string.IsNullOrEmpty(membership.ChristianName) ? membership.ChristianName : string.Empty;
            this.txtCity.Text = !string.IsNullOrEmpty(membership.City) ? membership.City : string.Empty;
            this.txtCountry.Text = !string.IsNullOrEmpty(membership.Country) ? membership.Country : string.Empty;
            this.txtEmail.Text = !string.IsNullOrEmpty(membership.Email) ? membership.Email : string.Empty;
            this.txtFax.Text = !string.IsNullOrEmpty(membership.Fax) ? membership.Fax : string.Empty;
            this.txtFirstName.Text = !string.IsNullOrEmpty(membership.FirstName) ? membership.FirstName : string.Empty;
            this.txtHome.Text = !string.IsNullOrEmpty(membership.Home) ? membership.Home : string.Empty;
            this.txtLastName.Text = !string.IsNullOrEmpty(membership.LastName) ? membership.LastName : string.Empty;
            this.txtMembershipNo.Text = !string.IsNullOrEmpty(membership.MembershipNo) ? membership.MembershipNo : string.Empty;
            this.txtMobile.Text = !string.IsNullOrEmpty(membership.Mobile) ? membership.Mobile : string.Empty;
            this.txtNameToAppear.Text = !string.IsNullOrEmpty(membership.NameToAppear) ? membership.NameToAppear : string.Empty;
            this.txtNRIC.Text = !string.IsNullOrEmpty(membership.Nric) ? membership.Nric : string.Empty;
            this.txtOccupation.Text = !string.IsNullOrEmpty(membership.Occupation) ? membership.Occupation : string.Empty;
            this.txtOffice.Text = !string.IsNullOrEmpty(membership.Office) ? membership.Office : string.Empty;
            this.txtRemark.Text = !string.IsNullOrEmpty(membership.Remarks) ? membership.Remarks : string.Empty;
            this.txtZipCode.Text = !string.IsNullOrEmpty(membership.ZipCode) ? membership.ZipCode : string.Empty;
            this.dtpDOB.Value = membership.DateOfBirth.HasValue ? (membership.DateOfBirth.Value) : DateTime.Today.Date;
            this.dtpExpiryDate.Value = membership.ExpiryDate.HasValue ? (membership.ExpiryDate.Value) : DateTime.Today.Date;
            this.dtpSubscriptionDate.Value = membership.SubscriptionDate.HasValue ? (membership.SubscriptionDate.Value) : DateTime.Today.Date;

            this.cmbNationality.SelectedIndex = string.IsNullOrEmpty(membership.Nationality) ? cmbNationality.Items.IndexOf("Singaporean") : cmbNationality.Items.IndexOf(membership.Nationality);
            this.cmbGroupName.DataSource = MembershipController.FetchMembershipGroupList();
            this.cmbGroupName.DisplayMember = "GroupName";
            this.cmbGroupName.ValueMember = "MembershipGroupID";
            this.cmbGroupName.SelectedValue = membership.MembershipGroupId;

            this.cmbGender.SelectedValue = membership.Gender;

            LoadAdditionalInformation(membershipNo);
        }

        private void LoadAdditionalInformation(string membershipno)
        {
            Membership member = new Membership(membershipno);

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
                                    ctr.Text = member.GetColumnValue(data) == null ? "" : member.GetColumnValue(data).ToString();
                                    
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            var membership = new Membership(txtMembershipNo.Text);
            membership.Remarks = this.txtRemark.Text;
            membership.Save(UserInfo.username);

            var cclController = new CounterCloseLogController();
            var cclColl = (from ccl in cclController.FetchAll().ToList()
                           where ccl.PointOfSaleID == PointOfSaleInfo.PointOfSaleID
                           orderby ccl.EndTime descending
                           select ccl).ToList().DefaultIfEmpty(new CounterCloseLog()).FirstOrDefault();

            //if member's created on is null, use subscription date
            //only connect to server when member has already existed before createdon < last counterclose
            if (DateTime.Compare(cclColl.EndTime, membership.CreatedOn.HasValue ? (membership.CreatedOn.Value) : membership.SubscriptionDate.Value ) > 0)
            {
                SyncClientController.SendModifiedMembershipRemarks(membership);
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMembershipViewInfo_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} View Info", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Membership");
            lblMembershipNo.Text = string.Format("{0} No", AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)) ? AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() : "Membership");
        }
    }
}
