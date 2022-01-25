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
    public partial class frmNewMembersList : Form
    {
        
        public frmNewMembersList()
        {
            InitializeComponent();            
        }

        private void frmNewMembersList_Load(object sender, EventArgs e)
        {
            DateTime lastLoginTime = ClosingController.FetchLastClosingTime(PointOfSaleInfo.PointOfSaleID);

            //Load membership
            ViewMembershipCollection v = new ViewMembershipCollection();
            v.Where(ViewMembership.Columns.CreatedOn, Comparison.GreaterThan, lastLoginTime);
            v.Load();

            dgvMember.DataSource = v;
            dgvMember.Refresh();            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to close?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dgvMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string membershipNo = dgvMember.Rows[e.RowIndex].Cells[1].Value.ToString();
                Membership member = new Membership(Membership.Columns.MembershipNo, membershipNo);
                if (!member.IsNew)
                {
                    frmNewMembershipEdit f = new frmNewMembershipEdit(member);
                    f.ShowDialog();
                    f.Dispose();
                }
            }
        }
    }
}
