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
    public partial class CashierMessage : Form
    {
        public string MemberName;
        public string CheckInOut; //Check In = 0; check out = 1
        public string visicount;
        public string ExpiryDate;
        public string strCheckOutItem;
        public string MembershipNo;

        public CashierMessage(string MemberName, string MembershipNo, string CheckInOut, string expiryDate)
        {
            InitializeComponent();
            this.MemberName = MemberName;
            this.CheckInOut = CheckInOut;
            this.MembershipNo = MembershipNo;
            this.ExpiryDate = expiryDate;
        }

        private void showOnMonitor(int showOnMonitor)
        {
            Screen[] sc;
            sc = Screen.AllScreens;
            if (showOnMonitor >= sc.Length)
            {
                showOnMonitor = 0;
            }
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point((sc[showOnMonitor].Bounds.Left + 230), (sc[showOnMonitor].Bounds.Top + 200));
            this.WindowState = FormWindowState.Normal;

        }

        private void CashierMessage_Load(object sender, EventArgs e)
        {
            showOnMonitor(2);
            #region *) Confirmation: when Member Check in GYM
            if (CheckInOut == "0")
            {
                this.lblMessage.Text = "Welcome :" + MemberName.ToString() + "\nThe Membership is valid until :\n" + ExpiryDate;   
            }
            #endregion
            #region *) Confirmation: When Member Check Out
            else
            {
                this.lblMessage.Text = "Thank you " + MemberName.ToString() + " for using TGN. \nThe Membership is valid until :\n" + ExpiryDate;

                //check for package
                string[] packageListCount = PowerPOS.MembershipController.getRemainingPackageList(MembershipNo);
                if (packageListCount != null && packageListCount.Length != 0 )
                    this.lblMessage.Text += "\n\nDid the member complete a training session today?";
            }
            #endregion
        }

        void timer1_Tick(object sender, EventArgs e)
        {
                timer1.Stop();
                this.Close();
                this.Dispose();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

    }
}
