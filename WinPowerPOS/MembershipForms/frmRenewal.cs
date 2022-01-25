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
    public partial class frmRenewal : Form
    {
        public string membershipno;
        public POSController pos;
        public bool IsSuccessful;        
        private DateTime? nowExpiryDate;
        public int selectedMembershipGroupID;

        public frmRenewal()
        {
            InitializeComponent();

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                this.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString() + " Renewal";
                label5.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper() + " NO";
                label6.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString().ToUpper() + " GROUP";
            }
            else
            {
                this.Text = "Membership Renewal";
                label5.Text = "MEMBERSHIP NO";
                label6.Text = "MEMBERSHIP GROUP";
            }    
        }

        //public int MembershipGroupID
        //{
        //    get
        //    {
        //        if (cmbMembershipGroup.Items.Count > 0)
        //            return (int)cmbMembershipGroup.SelectedValue;
        //        else
        //            return 0;
        //    }
        //}

        private void btnOK_Click(object sender, EventArgs e)
        {
            string status;
            DateTime tmpDate = dtpNewExpiryDate.Value;
            pos.SetNewExpiryDate(tmpDate);

            pos.SetNewMembershipGroupID((int)cmbMembershipGroup.SelectedValue);
            selectedMembershipGroupID = ((int)cmbMembershipGroup.SelectedValue);

            if (pos.AssignExpiredMember(membershipno, out status)) //ignore expiry date
            {
                pos.ApplyMembershipDiscount();
                IsSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Error: " + status, "", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }

        private void frmRenewal_Load(object sender, EventArgs e)
        {
            /* change membership to customer*/
            string scanmembership = "Please scan membership no";
            string membershipnorequired = "Membership No Required";
            string scanmemberwarning = "Invalid membership number scanned.";
            string membershipgrouperror = "(error)Cannot find membership group.\nPlease contact your administrator to enable this feature";

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                scanmembership = string.Format("Please scan {0} no", AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString());
                membershipnorequired = string.Format("{0} No Required", AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString());
                scanmemberwarning = string.Format("Invalid {0} number scanned", AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString());
                membershipgrouperror = string.Format("(error)Cannot find {0} group.\nPlease contact your administrator to enable this feature", AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement).ToString());
            }

            int SelectedGroupID = 0;

            try
            {
                MembershipGroupCollection AllGroups = new MembershipGroupCollection();
                AllGroups.Load();
                cmbMembershipGroup.DataSource = AllGroups;
                cmbMembershipGroup.DisplayMember = MembershipGroup.Columns.GroupName;
                cmbMembershipGroup.ValueMember = MembershipGroup.Columns.MembershipGroupId;
                if (cmbMembershipGroup.Items.Count <= 0)
                    throw new Exception(membershipgrouperror);
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
                this.Close();
            }

            if (!pos.MembershipApplied())
            {
                bool acceptable = false;
                while (!acceptable)
                {
                    string tmp = Microsoft.VisualBasic.Interaction.InputBox
                        (scanmembership, membershipnorequired, "", 0, 0);

                    if (tmp == null || tmp == "")
                    {
                        IsSuccessful = false;
                        acceptable = true;
                        this.Close();
                    }
                    else
                    {
                        Membership tmpMbr = new Membership(Membership.Columns.MembershipNo, tmp);
                        if (tmpMbr.IsLoaded && !tmpMbr.IsNew)
                        {
                            membershipno = tmpMbr.MembershipNo;
                            SelectedGroupID = tmpMbr.MembershipGroupId;
                            if (tmpMbr.ExpiryDate.HasValue)
                            {
                                lblEpxDateNow.Text = tmpMbr.ExpiryDate.Value.ToString("dd MMM yyyy");
                                nowExpiryDate = tmpMbr.ExpiryDate.Value;
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ExpiryDateBasedOnRenewalDate), false))
                                {
                                    dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                                }
                                else
                                {
                                    if (nowExpiryDate >= DateTime.Now)
                                    {
                                        //lblExpDateNew.Text = tmpMbr.ExpiryDate.Value.AddMonths(int.Parse(txtMonths.Text)).ToString("dd MMM yyyy");
                                        dtpNewExpiryDate.Value = tmpMbr.ExpiryDate.Value.AddMonths(int.Parse(txtMonths.Text));
                                    }
                                    else
                                    {
                                        //lblExpDateNew.Text = DateTime.Today.AddMonths(int.Parse(txtMonths.Text)).ToString("dd MMM yyyy");
                                        dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                                    }
                                }
                            }                            
                            acceptable = true;
                        }
                        else
                        {
                            MessageBox.Show(scanmemberwarning, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                Membership tmpMbr = pos.GetMemberInfo();
                membershipno = tmpMbr.MembershipNo;
                SelectedGroupID = tmpMbr.MembershipGroupId;
                if (tmpMbr.ExpiryDate.HasValue)
                {
                    lblEpxDateNow.Text = tmpMbr.ExpiryDate.Value.ToString("dd MMM yyyy");
                    nowExpiryDate = tmpMbr.ExpiryDate.Value;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ExpiryDateBasedOnRenewalDate), false))
                    {
                        dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                    }
                    else
                    {
                        if (nowExpiryDate >= DateTime.Now)
                        {
                            //lblExpDateNew.Text = tmpMbr.ExpiryDate.Value.AddMonths(int.Parse(txtMonths.Text)).ToString("dd MMM yyyy");
                            dtpNewExpiryDate.Value = tmpMbr.ExpiryDate.Value.AddMonths(int.Parse(txtMonths.Text));
                        }
                        else
                        {
                            dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                        }
                    }
                }

            }
            cmbMembershipGroup.SelectedValue = SelectedGroupID;
            txtMembershipNo.Text = membershipno;
            txtMonths.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void txtMonths_TextChanged(object sender, EventArgs e)
        {
            if (txtMonths.Text != "")
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ExpiryDateBasedOnRenewalDate), false))
                {
                    dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                }
                else
                {
                    if (nowExpiryDate.HasValue)
                    {
                        if (nowExpiryDate >= DateTime.Now)
                        {
                            //lblExpDateNew.Text = nowExpiryDate.Value.AddMonths(int.Parse(txtMonths.Text)).ToString("dd MMM yyyy");
                            dtpNewExpiryDate.Value = nowExpiryDate.Value.AddMonths(int.Parse(txtMonths.Text));
                        }
                        else
                        {
                            dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                            //lblExpDateNew.Text = DateTime.Today.AddMonths(int.Parse(txtMonths.Text)).ToString("dd MMM yyyy");
                        }
                    }
                    else
                    {
                        dtpNewExpiryDate.Value = DateTime.Today.AddMonths(int.Parse(txtMonths.Text));
                        //lblExpDateNew.Text = DateTime.Today.AddMonths(int.Parse(txtMonths.Text)).ToString("dd MMM yyyy");                    
                    }
                }
            }
        }

        private void txtMonths_MouseClick(object sender, MouseEventArgs e)
        {
            frmKeypad f = new frmKeypad();
            f.IsInteger = true;
            f.ShowDialog();
            txtMonths.Text = f.value;
        }
    }
}