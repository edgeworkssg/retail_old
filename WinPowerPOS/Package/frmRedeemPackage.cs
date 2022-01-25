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
using System.Collections;
using SubSonic;

namespace WinPowerPOS.Package
{
    public partial class frmRedeemPackage : Form
    {
        public frmRedeemPackage()
        {
            InitializeComponent();
            dgvRedeem.AutoGenerateColumns = false;
            UserMstCollection st = new UserMstCollection();
            //st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.Deleted, false);
            st.Where(UserMst.Columns.IsASalesPerson, true);
            st.Load();
            st.Sort(UserMst.Columns.UserName, true);

            ArrayList ar = new ArrayList();
            for (int i = 0; i < st.Count; i++)
            {
                ar.Add(st[i].UserName);
            }
            cmbStylist.DataSource = ar;
            cmbStylist.Refresh();
        }
        private void BindGrid()
        {
            
            ViewPackageRedemptionCollection pr = new ViewPackageRedemptionCollection();
            pr.Where(PackageRedemptionLog.Columns.Deleted, false);
            pr.Where(ViewPackageRedemption.Columns.PackageRedeemDate, Comparison.GreaterOrEquals, DateTime.Today);
            pr.OrderByDesc(PackageRedemptionLog.Columns.PackageRedeemDate);
            pr.Load();
            dgvRedeem.DataSource = pr;
            dgvRedeem.Refresh();
        }
        private void btnRedeem_Click(object sender, EventArgs e)
        {
            try
            {
                decimal amount;
                if (!decimal.TryParse(txtAmount.Text, out amount))
                {
                    MessageBox.Show("Please specify a valid amount");
                    txtAmount.Focus();
                    return;
                }

                DialogResult dr = MessageBox.Show("Are you sure you want to proceed?", "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                {
                    return;
                }
                PackageRedemptionLog newLog = new PackageRedemptionLog();
                newLog.StylistId = cmbStylist.SelectedItem.ToString();
                newLog.PackageRedeemDate = DateTime.Now;
                newLog.Amount = amount;
                newLog.Deleted = false;
                newLog.UniqueID = Guid.NewGuid();
                newLog.MembershipNo = txtMembershipNo.Text;
                newLog.Name = txtNameOf.Text;
                newLog.Save(UserInfo.username);
                MessageBox.Show("Redemption captured successfully");
                BindGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error, please contact your administrator.");
                Logger.writeLog(ex);
            }
        }

        private void frmRedeemPackage_Load(object sender, EventArgs e)
        {
            BindGrid();
        }        
    }
}
