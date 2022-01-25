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
    /// UI to update remark. (created by John Harries)
    /// </summary>
    public partial class frmMembershipEditRemark : Form
    {
        public frmMembershipEditRemark(string membershipNo)
        {
            InitializeComponent();
            loadMembershipDetail(membershipNo);

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                this.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Remark";

            }
            else {
                this.Text = "Membership Remark";
            }    
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

            this.cmbGroupName.DataSource = MembershipController.FetchMembershipGroupList();
            this.cmbGroupName.DisplayMember = "GroupName";
            this.cmbGroupName.ValueMember = "MembershipGroupID";
            this.cmbGroupName.SelectedValue = membership.MembershipGroupId;

            this.cmbGender.SelectedValue = membership.Gender;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var membership = new Membership(txtMembershipNo.Text);
            if (!membership.IsNew)
            {
                string sqlString = "Update membership set Remarks = N'" + this.txtRemark.Text + "' where membershipno ='" +
                    txtMembershipNo.Text + "'";
                try
                {
                    DataService.ExecuteQuery(new QueryCommand(sqlString));
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex.Message);
                }
                membership.Remarks = this.txtRemark.Text;


                //membership.Save(UserInfo.username);
                var cclController = new CounterCloseLogController();
                var cclColl = (from ccl in cclController.FetchAll().ToList()
                               where ccl.PointOfSaleID == PointOfSaleInfo.PointOfSaleID
                               orderby ccl.EndTime descending
                               select ccl).ToList().DefaultIfEmpty(new CounterCloseLog()).FirstOrDefault();

                //if member's created on is null, use subscription date
                //only connect to server when member has already existed before createdon < last counterclose
                if (DateTime.Compare(cclColl.EndTime, membership.CreatedOn.HasValue ? (membership.CreatedOn.Value) : membership.SubscriptionDate.Value) > 0)
                {
                    SyncClientController.SendModifiedMembershipRemarks(membership);
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
