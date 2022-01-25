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
    public partial class frmAttendanceReport : Form
    {
        public frmAttendanceReport()
        {
            InitializeComponent();
        }

        private void frmAttendanceReport_Load(object sender, EventArgs e)
        {
            PointOfSaleCollection POSes = new PointOfSaleCollection();
            POSes.Load();
            cbPOS.DataSource = POSes;
            cbPOS.ValueMember = PowerPOS.PointOfSale.Columns.PointOfSaleID;
            cbPOS.DisplayMember = PowerPOS.PointOfSale.Columns.PointOfSaleName;

            SettingCollection Stgs = new SettingCollection();
            Stgs.Load();
            cbPOS.SelectedValue = Stgs[0].PointOfSaleID;

            if (cbPOS.Items.Count > 0) cbPOS.SelectedIndex = 0;

            dgCheckOut.AutoGenerateColumns = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DateTime StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTime EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

            if (dtStart.Checked) StartTime = new DateTime(dtStart.Value.Year, dtStart.Value.Month, dtStart.Value.Day, 0, 0, 0);
            if (dtEnd.Checked) EndTime = new DateTime(dtEnd.Value.Year, dtEnd.Value.Month, dtEnd.Value.Day, 23, 59, 59); 

            dgCheckOut.DataSource = MembershipAttendanceController.FetchCheckOutMember
                (StartTime, EndTime, (int)cbPOS.SelectedValue);

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgCheckOut.DataSource;
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
            DataTable dt = (DataTable)dgCheckOut.DataSource;
            if (dt.Columns.Contains("LockerID")) dt.Columns.Remove("LockerID");
            ExportController.ExportToCSV(dt, fsdExportToExcel.FileName);
            MessageBox.Show("File saved");
        }
    }
}
