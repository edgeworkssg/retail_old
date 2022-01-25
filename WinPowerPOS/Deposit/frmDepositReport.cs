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
using WinPowerPOS.EditBillForms;
using WinPowerPOS.MembershipForms;
namespace WinPowerPOS.DepositForms
{
    public partial class frmDepositReport : Form
    {
        public frmDepositReport()
        {
            InitializeComponent();
            dgvInstallment.AutoGenerateColumns = false;
        
        }
        
        private void frmDepositReport_Load(object sender, EventArgs e)
        {
            /*Load
            dgvInstallment.DataSource =
                InstallmentController.GetOutstandingInstallmentDetails
                ("", false, ViewInstallmentMembershipItem.Columns.DueDate, "ASC");
            dgvInstallment.Refresh();
            if (dgvInstallment.Rows.Count > 0)
                dgvInstallment_CellClick(this, new DataGridViewCellEventArgs(0, 0));
            */
        }

        private void dgvInstallment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    //Pop membership form
                    string membershipNo = dgvInstallment.Rows[e.RowIndex].Cells["MembershipNo"].Value.ToString();
                    Membership member = new Membership(Membership.Columns.MembershipNo, membershipNo);
                    if (!member.IsNew)
                    {
                        frmNewMembershipEdit f = new frmNewMembershipEdit(member);
                        f.IsReadOnly = true;
                        f.ShowDialog();
                        f.Dispose();
                    }
                }
            }
        }

        private void dgvInstallment_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                /*
                if ((DateTime)dgvInstallment.Rows[e.RowIndex].Cells["DueDate"].Value < DateTime.Now)
                {
                    dgvInstallment.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkRed;
                    dgvInstallment.Rows[e.RowIndex].DefaultCellStyle.ForeColor  = Color.White;
                } */               
            }
        }
        
        private string searchText;

        private void btnName_Click(object sender, EventArgs e)
        {
            searchText = ((Button)sender).Name;
            DataTable dt
             = MembershipTapController.FetchMembershipTapReport("","",txtSearch.Text,"amount", "desc");             

            dgvInstallment.DataSource = dt;
            dgvInstallment.Refresh();
        }

        private void btnNRIC_Click(object sender, EventArgs e)
        {
            searchText = ((Button)sender).Name;
            DataTable dt
             = MembershipTapController.FetchMembershipTapReport("", txtSearch.Text, "", "amount", "desc");

            dgvInstallment.DataSource = dt;
            dgvInstallment.Refresh();
        }
        
        private void btnRefNo_Click(object sender, EventArgs e)
        {
            searchText = ((Button)sender).Name;
            DataTable dt
             = MembershipTapController.FetchMembershipTapReport(txtSearch.Text, "", "", "amount", "desc");
            dgvInstallment.DataSource = dt;
            dgvInstallment.Refresh();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvInstallment.DataSource;
            if (dt != null && dt.Rows.Count > 0)
            {
                fsdExportToExcel.ShowDialog();
            }
            else
            {
                MessageBox.Show("There is no data to export");
            }            
        }
        private void fsdExportToExcel_FileOk(object sender, CancelEventArgs e)
        {
            ExportController.ExportToExcel(dgvInstallment, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }
    }
}
