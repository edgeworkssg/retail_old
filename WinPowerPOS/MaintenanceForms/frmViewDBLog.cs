using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace WinPowerPOS.MaintenanceForms
{
    public partial class frmViewDBLog : Form
    {
        public frmViewDBLog()
        {
            InitializeComponent();
            dgvReport.AutoGenerateColumns = false;
            CommonUILib.FormatDateFilter(ref dtpStartDate, ref dtpEndDate);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            DataTable dt = UtilityController.FetchDataBaseLog
                (true, true, startDate, endDate, txtMessage.Text, "", "desc");
            dgvReport.DataSource = dt;
            dgvReport.Refresh();
        }

        private void frmViewDBLog_Load(object sender, EventArgs e)
        {
            btnSearch_Click(this, new EventArgs());
        }
    }
}
