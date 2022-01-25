using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;


namespace AttendanceTracker
{
    public partial class frmMembershipAttendance : Form
    {
        #region "Form event handler"
        public frmMembershipAttendance()
        {
            InitializeComponent();
            dgvAttendance.AutoGenerateColumns = false;
        }


        #endregion
        private void BindGrid()
        {
            dgvAttendance.DataSource = MembershipAttendanceController.FetchAttendanceList
                (DateTime.Today, DateTime.Now, PointOfSaleInfo.PointOfSaleID, "", "");
            dgvAttendance.Refresh();
            lblWarning.Text = "";
            lblMsg.Text = "";
        }

        private void frmAttendance_Load(object sender, EventArgs e)
        {
            /*
            frmPOSLogin f = new frmPOSLogin();
            f.allowClose = false;
            f.ShowDialog();
            if (f.IsSuccessful)
            {
                PointOfSaleController.GetPointOfSaleInfo();
                llPOSName.Text = PointOfSaleInfo.PointOfSaleName;
            }
            else
            {
                Application.Exit();
                return;
            }
            f.Dispose();
            */
            llPOSName.Text = PointOfSaleInfo.PointOfSaleName;
            BindGrid();
        }

        private void txtScan_KeyDown(object sender, KeyEventArgs e)
        {
            string msg;
            Membership m;
            if (e.KeyCode == Keys.Enter)
            {
                if (!MembershipController.IsExistingMember(txtScan.Text, out m))
                {
                    if (m != null && m.IsLoaded && m.ExpiryDate.HasValue && m.ExpiryDate.Value < DateTime.Now)
                    {
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = "Membership " + txtScan.Text + " has expired on " + m.ExpiryDate.Value.ToString("dd MMM yyyy");
                        return;
                    }
                    if (!MembershipController.IsMembersNRIC(txtScan.Text, out m))
                    {
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = txtScan.Text + " is not valid barcode";
                        return;
                    }
                }
                //by now m is something alredy
                /*frmScanLocker frm = new frmScanLocker();
                frm.membershipNo = m.MembershipNo;
                frm.ShowDialog();
                if (frm.IsSuccessful)*/
                {

                    //Check membership 
                    if (MembershipAttendanceController.ScanMembership(m, out msg))
                    {
                        //Fetch membership information.....
                        //Flash messages.... expiry and etc

                        BindGrid();
                        /*
                        string filename = Application.StartupPath + "\\Photos\\" + m.MembershipNo + ".jpg";
                        if (System.IO.File.Exists(filename))
                        {
                            pbImage.Image = Image.FromFile(filename);
                        }
                        else
                        {
                            pbImage.Image = null;
                        }*/
                        lblMsg.ForeColor = Color.Green;
                        lblMsg.Text = "Scanning successful for member " + txtScan.Text;

                        if (msg != String.Empty)
                        {
                            lblWarning.Text = msg;
                            lblWarning.BackColor = Color.Black;
                            lblWarning.ForeColor = Color.Yellow;
                        }
                        else
                        {
                            lblWarning.Text = "";
                        }
                    }
                    else
                    {
                        //MessageBox.Show("The scanned barcode [" + txtScan.Text + "] is not a valid membership number.");
                        lblMsg.ForeColor = Color.Red;
                        lblMsg.Text = msg;
                    }
                    txtScan.Text = "";
                }
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            frmSyncData f = new frmSyncData();
            f.ShowDialog();
            f.Dispose();            
        }

        private void llPOSName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Pop a window to allow you change POS Location
            frmPOSLocation f = new frmPOSLocation();
            f.ShowDialog();
            f.Dispose();
            llPOSName.Text = PointOfSaleInfo.PointOfSaleName;            
        }
    }
}