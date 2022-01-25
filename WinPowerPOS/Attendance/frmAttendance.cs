using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinPowerPOS.Attendance
{
    public partial class frmAttendance : Form
    {
        public BackgroundWorker SyncAttendanceThread;
        public BackgroundWorker SyncSalesThread;

        public frmAttendance()
        {
            InitializeComponent();
        }

        public frmAttendance(BackgroundWorker syncAttendance, BackgroundWorker syncSales)
        {
            SyncAttendanceThread = syncAttendance;
            SyncSalesThread = syncSales;
            InitializeComponent();
        }

        private void frmAttendance_Load(object sender, EventArgs e)
        {
            dgActive.AutoGenerateColumns = false;
            dgCheckOut.AutoGenerateColumns = false;

            tabControl1_SelectedIndexChanged(null, null);
            txtScan.Focus();

            BindGrid();
       }

        private void txtScan_Leave(object sender, EventArgs e)
        {
            txtScan.Focus();
        }

        private void txtScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnGo.PerformClick();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startTime, endTime;
                MembershipAttendanceController.ActivityType activityType;

                Membership Member = PowerPOS.Factory.Membership.GetMember(txtScan.Text, true, true, true);
                MembershipAttendanceController.ScanMembership(Member, out activityType, out startTime, out endTime);
                BindGrid();
                
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncAttendance), false))
                {
                    if (!SyncAttendanceThread.IsBusy)
                        SyncAttendanceThread.RunWorkerAsync();
                }

                if (activityType == MembershipAttendanceController.ActivityType.Logout && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.GenerateOrderUponCheckOut), false))
                {
                    DialogResult answer = MessageBox.Show("Do you want to create order?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (answer == DialogResult.Yes)
                    {
                        string itemNo = AppSetting.GetSetting(AppSetting.SettingsName.Attendance.ItemNo);
                        FormController.ShowInvoiceFromAttendance(Member.MembershipNo, itemNo, startTime, endTime, SyncSalesThread);
                        this.Close();
                        this.Dispose();
                    }
                }
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
            finally
            {
                txtScan.Text = "";
            }
        }

        private void BindGrid()
        {
            dgActive.DataSource = MembershipAttendanceController.FetchActiveMember();
            dgCheckOut.DataSource = MembershipAttendanceController.FetchCheckOutMember
                (DateTime.Today, DateTime.Today.AddDays(1), PowerPOS.Container.PointOfSaleInfo.PointOfSaleID);

            #region *) Setting: Update Refresh Interval. More data, longer interval
            if (dgActive.Rows.Count < 50)
                timerData.Interval = 10000;
            else if (dgActive.Rows.Count < 300)
                timerData.Interval = 60000;
            else
                timerData.Interval = 300000;
            #endregion

            if (dgActive.SortedColumn == null)
                dgActive.Sort(dgvcStartTime, ListSortDirection.Ascending);

            if (dgCheckOut.SortedColumn == null)
                dgCheckOut.Sort(dgvcOEndTime, ListSortDirection.Descending);

            #region *) Fetch: Get last checked out member
            DataRow Dr = MembershipAttendanceController.FetchLastCheckOutMemberSince(DateTime.Today);

            lblCardNo.Text = "<NONE>";
            lblStart.Text = "00 : 00 : 00";
            lblEnd.Text = "00 : 00 : 00";
            lblDuration.Text = "00d 00h 00m 00s";

            if (Dr != null)
            {
                lblCardNo.Text = Dr["MembershipNo"].ToString();
                lblStart.Text = ((DateTime) Dr["LoginTime"]).ToString("HH : mm : ss");
                lblEnd.Text = ((DateTime)Dr["LogoutTime"]).ToString("HH : mm : ss");
                lblDuration.Text = Dr["Duration"].ToString();
            }
            #endregion

            lblTotalActive.Text = dgActive.Rows.Count.ToString("N0");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void timerData_Tick(object sender, EventArgs e)
        {
            DataGridViewColumn SortedColumn = dgActive.SortedColumn;
            
            ListSortDirection ListSortedOrder;
            SortOrder SortedOrder = dgActive.SortOrder;
            if (SortedOrder == SortOrder.Descending)
                ListSortedOrder = ListSortDirection.Descending;
            else
                ListSortedOrder = ListSortDirection.Ascending;

            dgActive.DataSource = MembershipAttendanceController.FetchActiveMember();

            if (SortedColumn != null)
            {
                dgActive.Sort(SortedColumn, ListSortedOrder);
            }
        }

        private void dgCheckOut_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnShowSearch_Click(object sender, EventArgs e)
        {
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            FormController.ShowForm(FormController.FormNames.AttendanceReport, new frmAttendanceReport());
        }

        private void txtScan_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == ';')
            //{
            //    txtScan.Text = "";
            //}
            //else if (e.KeyChar == '?')
            //{
            //    txtScan.Text = txtScan.Text.Replace(";", "").Replace("?", "");
            //    btnGo.PerformClick();
            //}
            //else
            //{
            //    txtScan.Text += e.KeyChar.ToString();
            //}

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.GenerateOrderUponCheckOut), false))
                btnCreateInvoice.Visible = true;
            else
                btnCreateInvoice.Visible = false;
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string itemNo = AppSetting.GetSetting(AppSetting.SettingsName.Attendance.ItemNo);
                string membershipNo = dgCheckOut.Rows[dgCheckOut.SelectedCells[0].RowIndex].Cells[dgvcOCardNo.Index].Value.ToString();
                DateTime startTime = (DateTime)dgCheckOut.Rows[dgCheckOut.SelectedCells[0].RowIndex].Cells[dgvcOStartTime.Index].Value;
                DateTime endTime = (DateTime)dgCheckOut.Rows[dgCheckOut.SelectedCells[0].RowIndex].Cells[dgvcOEndTime.Index].Value;

                FormController.ShowInvoiceFromAttendance(membershipNo, itemNo, startTime, endTime, SyncSalesThread);
                this.Close();
                this.Dispose();
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }
    }
}
