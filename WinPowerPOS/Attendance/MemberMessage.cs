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
    public partial class MemberMessage : Form
    {
        public string MemberName;
        public string CheckInOut;  //Check In = 0; check out = 1
        public string ExpiryDate;

        public MemberMessage(string MemberName, string CheckInOut, string expiryDate)
        {
            InitializeComponent();
            this.MemberName = MemberName;
            this.CheckInOut = CheckInOut;
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

            this.Size = new Size(sc[showOnMonitor].Bounds.Width, sc[showOnMonitor].Bounds.Height);
            this.Location = new Point((sc[showOnMonitor].Bounds.Left), (sc[showOnMonitor].Bounds.Top));

            this.WindowState = FormWindowState.Normal;
        }

        private void MemberMessage_Load(object sender, EventArgs e)
        {
            showOnMonitor(1);
            #region *) Confirmation: when Member Check in GYM
            if (CheckInOut == "0")
            {
                this.lblMember.Text = "Welcome :" + MemberName.ToString() + "\nThe Membership is valid until :\n" + ExpiryDate;
            }
            #endregion
            #region *) Confirmation: When Member Check Out
            else
            {
                this.lblMember.Text = "Thank you " + MemberName.ToString() + " for using TGN. See you again. \nYour Membership is valid until :\n" + ExpiryDate;
            }
            #endregion
        }

        void timer1_Tick(object sender, EventArgs e)
        {
                timer1.Stop();
                this.Close();
                this.Dispose();// here you can log off the user & call the new window
        }
    }
}
