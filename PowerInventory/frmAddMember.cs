using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using PowerPOS.Container;
using System.Collections;
using PowerPOS;
using PowerPOSLib.PowerPOSSync;
using SubSonic;
namespace PowerInventory
{
    public partial class frmAddMember : Form
    {
        public string searchReq;

        public decimal PreferedDiscount;
        public string membershipNo;

        private string membersText = "Members";

        #region "Form Initialization and loading"
        public frmAddMember()
        {
            InitializeComponent();

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false) && !string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement)))
            {
                membersText = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement);
            }
            this.Text = string.Format("Search {0}", membersText);
            labelSearchNotification.Text = "";
        }
        #endregion

        #region "Close Form"
        private void btnCancelAdd_Click(object sender, EventArgs e)
        {
            try
            {
                membershipNo = "";
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void dgvMembersList_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    //Get the membership no            
                    membershipNo = dgvMembersList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    this.Close();
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                return;
            }

            string searchText = txtSearch.Text;
            //find item using the given text
            MembershipController mbr = new MembershipController();
            ViewMembershipCollection coll = mbr.SearchMembership(searchText);

            if (coll.Count == 0)
            {
                labelSearchNotification.ForeColor = Color.Red;
                labelSearchNotification.Text = string.Format("{0} cannot be found!", membersText);
            }
            else
            {
                labelSearchNotification.ForeColor = Color.Blue;
                labelSearchNotification.Text = coll.Count + string.Format(" {0} found.", membersText);
            }

            dgvMembersList.AutoGenerateColumns = false;
            this.dgvMembersList.DataSource = coll;
            this.dgvMembersList.Refresh();
        }
    }
}
