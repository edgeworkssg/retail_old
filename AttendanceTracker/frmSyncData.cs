using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace AttendanceTracker
{
    public partial class frmSyncData : Form
    {
        public frmSyncData()
        {
            InitializeComponent();
        }

        private void frmSyncData_Load(object sender, EventArgs e)
        {
            dtpStartDate.Value = DateTime.Today;
            dtpStartDate.Value = DateTime.Now;
            SyncClientController.Load_WS_URL();
            lblServer.Text = "Server: " + SyncClientController.WS_URL;
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            //Send Data Up to the Server and Also Download....
            SyncClientController.GetBasicInfoFromServer(true, true,true, true, false, false,false, false,false);

            //Send Attendance up....
            if (SyncClientController.SendMembershipAttendanceToServer(dtpStartDate.Value, dtpEndDate.Value))
            {
                MessageBox.Show("Sending Successful.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Sending failed. Please check your internet connection. Contact your system administrator if your problem persist.");
            }
        }
    }
}
