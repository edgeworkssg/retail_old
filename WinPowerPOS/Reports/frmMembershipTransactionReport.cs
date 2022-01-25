using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Collections;
using PowerPOSLib;
using WinPowerPOS.OrderForms;
using POSDevices;
using WinPowerPOS.MembershipForms;

namespace WinPowerPOS.Reports
{
    /// <remarks>
    /// Update Track:
    /// *) [205] Voided data filtered by SQL StoredProcedure [24 September 2010]
    /// </remarks>
    public partial class frmMembershipTransactionReport : Form
    {
        public frmMembershipTransactionReport()
        {
            InitializeComponent();

            SubSonic.QueryCommand cmd;
            IDataReader Rdr;
            DataTable dt;
            DataRow Rw;

            cmd = new SubSonic.QueryCommand(
                "SELECT MembershipGroupId, GroupName " +
                "FROM MembershipGroup " +
                "WHERE Deleted IS NOT NULL AND Deleted = 'false'");
            dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(cmd));
            Rw = dt.NewRow(); Rw[0] = 0; Rw[1] = "--Please Select--";
            dt.Rows.InsertAt(Rw, 0);
            cmbGroupName.DataSource = dt;
            cmbGroupName.ValueMember = "MembershipGroupId";
            cmbGroupName.DisplayMember = "GroupName";

            cmd = new SubSonic.QueryCommand(
               "SELECT CategoryName " +
               "FROM Category " +
               "WHERE Deleted IS NOT NULL AND Deleted = 'false'");
            dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(cmd));
            Rw = dt.NewRow(); Rw[0] = "ALL";
            dt.Rows.InsertAt(Rw, 0);
            cmbCategory.DataSource = dt;
            cmbCategory.ValueMember = "CategoryName";
            cmbCategory.DisplayMember = "CategoryName";

            cmd = new SubSonic.QueryCommand(
                "SELECT PointOfSaleID, PointOfSaleName " +
                "FROM PointOfSale " +
                "WHERE Deleted IS NOT NULL AND Deleted = 'false'");
            dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(cmd));
            Rw = dt.NewRow(); Rw[0] = 0; Rw[1] = "ALL";
            dt.Rows.InsertAt(Rw, 0);
            cmbPosID.DataSource = dt;
            cmbPosID.ValueMember = "PointOfSaleID";
            cmbPosID.DisplayMember = "PointOfSaleName";

            cmd = new SubSonic.QueryCommand(
                "SELECT OutletName " +
                "FROM PointOfSale " +
                "WHERE Deleted IS NOT NULL AND Deleted = 'false'");
            dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(cmd));
            Rw = dt.NewRow(); Rw[0] = "ALL";
            dt.Rows.InsertAt(Rw, 0);
            cmbOutlet.DataSource = dt;
            cmbOutlet.ValueMember = "OutletName";
            cmbOutlet.DisplayMember = "OutletName";

            cmd = new SubSonic.QueryCommand(
                "SELECT DepartmentID,DepartmentName " +
                "FROM Department " +
                "WHERE Deleted IS NOT NULL AND Deleted = 'false'");
            dt = new DataTable();
            dt.Load(SubSonic.DataService.GetReader(cmd));
            //Rw = dt.NewRow(); Rw[0] = 0; Rw[1] = "ALL";
            //dt.Rows.InsertAt(Rw, 0);
            cmbDepartment.DataSource = dt;
            cmbDepartment.ValueMember = "DepartmentID";
            cmbDepartment.DisplayMember = "DepartmentName";
            dgvRcpt.AutoGenerateColumns = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DisplayData();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayData()
        {
            try
            {
                int GroupNo;
                int.TryParse(cmbGroupName.SelectedValue.ToString(), out GroupNo);

                string selPOS = cmbPosID.Text == "ALL" ? "%" : cmbPosID.Text + "%";
                string selOutlet = cmbOutlet.SelectedValue.ToString() == "ALL" ? "%" : cmbOutlet.SelectedValue.ToString() + "%";
                string selCat = cmbCategory.Text == "ALL" ? "%" : cmbCategory.Text + "%";

                DataTable dt = ReportController.FetchMembershipTransactionReport
                    (dtpStartDate.Checked, dtpEndDate.Checked, dtpStartDate.Value, dtpEndDate.Value,
                    txtMembershipNo.Text.Trim() + "%", GroupNo, txtNRIC.Text + "%", txtFirstName.Text + "%", txtLastName.Text + "%",
                    txtAppearName.Text + "%", txtRefNo.Text + "%", txtItemName.Text + "%", selCat, 0, selPOS, selOutlet, cmbDepartment.SelectedValue.ToString(),
                    "", "",
                    "", "");
                /*
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                DataTable dt = ws.FetchMembershipTransactionReport(dtpStartDate.Checked, dtpEndDate.Checked, dtpStartDate.Value, dtpEndDate.Value,
                    txtMembershipNo.Text.Trim() + "%", GroupNo, txtNRIC.Text + "%", txtFirstName.Text + "%", txtLastName.Text + "%",
                    txtAppearName.Text + "%", txtRefNo.Text + "%", txtItemName.Text + "%", selCat, 0, selPOS, selOutlet, cmbDepartment.SelectedValue.ToString(),
                    "", "");
                */
                /*
                dt.Columns["OrderDate"].SetOrdinal(0);
                dt.Columns["MembershipNo"].SetOrdinal(1);
                dt.Columns["NameToAppear"].SetOrdinal(2);
                dt.Columns["CategoryName"].SetOrdinal(3);
                dt.Columns["ItemName"].SetOrdinal(4);
                dt.Columns["LineAmount"].SetOrdinal(5);
                dt.Columns["PointOfSaleName"].SetOrdinal(6);
                dt.Columns["OutletName"].SetOrdinal(7);
                dt.Columns["OrderHdrID"].SetOrdinal(8);
                while (dt.Columns.Count > 9)
                {
                    dt.Columns.RemoveAt(9);
                }
                */
                dgvRcpt.DataSource = dt;
                dgvRcpt.Refresh();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRcpt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                if (e.ColumnIndex == 0)
                {
                    if (dgvRcpt.Rows[e.RowIndex].Cells[0].Value is bool)
                    {
                        dgvRcpt.Rows[e.RowIndex].Cells[0].Value = !(bool)dgvRcpt.Rows[e.RowIndex].Cells[0].Value;
                    }
                    else
                    {
                        dgvRcpt.Rows[e.RowIndex].Cells[0].Value = true;
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                    EditBillForms.frmViewBillDetail myfrm = new EditBillForms.frmViewBillDetail();
                    myfrm.OrderHdrID = dgvRcpt.Rows[e.RowIndex].Cells["OrderHdrID"].Value.ToString();
                    myfrm.ShowDialog();
                    myfrm.Dispose();
                    DisplayData();
                }
                else if (e.ColumnIndex == dgvRcpt.Columns["MembershipNo"].Index)
                {
                    string membershipNo = dgvRcpt.Rows[e.RowIndex].Cells["MembershipNo"].Value.ToString();
                    Membership member = new Membership(Membership.Columns.MembershipNo, membershipNo);
                    if (!member.IsNew)
                    {
                        frmNewMembershipEdit f = new frmNewMembershipEdit(member);
                        f.IsReadOnly = false;
                        f.ShowDialog();
                        f.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRcpt_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //try
            //{
            //    for (int j = 0; j < dgvRcpt.ColumnCount; ++j)
            //    {
            //        if (dgvRcpt.Columns[j].Visible == true && bool.Parse(dgvRcpt["IsVoided", e.RowIndex].Value.ToString()) == true)
            //        {
            //            dgvRcpt.Rows[e.RowIndex].Cells[j].Style.ForeColor = System.Drawing.Color.White;
            //            dgvRcpt.Rows[e.RowIndex].Cells[j].Style.BackColor = System.Drawing.Color.DarkRed;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.writeLog(ex);
            //    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvRcpt != null && dgvRcpt.Rows.Count > 0)
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
            ExportController.ExportToExcel(dgvRcpt, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }
    }
}
