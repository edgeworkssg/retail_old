using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PowerPOS;
using PowerEdge.MembershipForms;

namespace PowerEdge.MembershipForm
{
    public partial class frmMembershipSearch : Form
    {
        public frmMembershipSearch()
        {
            InitializeComponent();
            cmbBirthdayMonth.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            bool useBdayMonth = false;
            if (cmbBirthdayMonth.SelectedIndex > 0)
            {
                useBdayMonth = true;
            }

            DataTable dt =
                ReportController.FetchMembershipReport
                (dtpStartExpiryDate.Checked, dtpEndExpiryDate.Checked, 
                    dtpStartExpiryDate.Value, dtpEndExpiryDate.Value, 
                    dtpStartBirthDay.Checked, dtpEndBirthDay.Checked, dtpStartBirthDay.Value, 
                    dtpEndBirthDay.Value,useBdayMonth,
                    cmbBirthdayMonth.SelectedIndex, txtMembershipNo.Text, txtMembershipNoTo.Text,
                    int.Parse(cmbGroupName.SelectedValue.ToString()), txtNRIC.Text, 
                    cmbGender.SelectedText, 
                    txtNameToAppear.Text, txtAddress.Text, txtMobile.Text,txtHome.Text,"","",
                    "membershipno", "ASC");
            dgvMember.DataSource = dt;
            dgvMember.Refresh();            

        }

        private void frmMassMailer_Load(object sender, EventArgs e)
        {
            DataTable dt = MembershipController.FetchMembershipGroupList();      
            cmbGroupName.DataSource = dt;
            cmbGroupName.DisplayMember = "GroupName";
            cmbGroupName.ValueMember = "MembershipGroupID";
            dgvMember.AutoGenerateColumns = false;
            cmbBirthdayMonth.SelectedIndex = 0;
        }

        private void dgvMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void dgvMember_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DateTime bdae;
            
            if (DateTime.TryParse(dgvMember.Rows[e.RowIndex].Cells["birthdae"].Value.ToString(), out bdae))
            {
                if (bdae.Month  == DateTime.Today.Month)            
                {
                    for (int i = 0; i < dgvMember.ColumnCount; i++)
                    {
                        if ((dgvMember.Columns[i].Visible))
                        {
                            dgvMember.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.Black;
                            dgvMember.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.Yellow;
                        }
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void dgvMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                frmNewMembershipEdit f = new frmNewMembershipEdit();
                f.membershipNo = dgvMember.Rows[e.RowIndex].Cells[1].Value.ToString();
                f.IsReadOnly = false;
                f.ShowDialog();
                f.Dispose();
            }
        }

        private void btnNewMember_Click(object sender, EventArgs e)
        {
            //
            frmNewMembershipEdit f = new frmNewMembershipEdit();
            f.membershipNo = "";
            f.IsReadOnly = false;
            f.ShowDialog();
            f.Dispose();

        }
    }
    
}