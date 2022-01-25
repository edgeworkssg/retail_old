using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PowerPOS;
using PowerPOS.Container;

namespace MassMailer
{
    public partial class frmMassMailer : Form
    {
        public frmMassMailer()
        {
            InitializeComponent();
        }
        
        

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            txtFileName.Text = openFileDialog1.FileName;

            //upload preview box
            pbPreview.Image = new Bitmap(txtFileName.Text);
            pbPreview.Refresh();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvMember.Rows.Count == 0)
                {
                    MessageBox.Show("There is no names in your recepient list.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtFileName.Text == "")
                {
                    MessageBox.Show("You have not specified a picture file for the eblast.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtSubject.Text == "")
                {
                    MessageBox.Show("Please specify the email subject", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSubject.Focus();
                    return;
                }

                ViewMembershipCollection member = new ViewMembershipCollection();
                member.Load((DataTable)dgvMember.DataSource);                

                    pnlSendOrder.Visible = true;
                    pnlSendOrder.BringToFront();
                    pnlSendOrder.Refresh();

                    SyncThread pThread = new SyncThread();

                    pThread.members = member;
                    pThread.FileName = txtFileName.Text;
                    pThread.MailSubject = txtSubject.Text;
                
                    Thread oThread = new Thread(new ThreadStart(pThread.runSyncUpdates));
                    oThread.Start();

                    while (!oThread.Join(30))
                    {                        
                        pnlSendOrder.Refresh();                        
                    }

                    frmReportDialog frm = new frmReportDialog();
                    frm.status = pThread.status;
                    frm.ShowDialog();

                    pnlSendOrder.Visible = false;
                    pnlSendOrder.SendToBack();
                    pnlSendOrder.Refresh();

                    if (pThread.failedMembers.Count > 0)
                    {
                        MessageBox.Show("There are failed sending.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        frmFailSend fr = new frmFailSend();
                        fr.member = pThread.failedMembers;
                        fr.filename = txtFileName.Text;
                        fr.subject = txtSubject.Text;
                        fr.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Sending Successful.");
                        this.Close();
                    }                
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //
            MembershipWS.FetchMembersPastTransaction ws = new MembershipWS.FetchMembersPastTransaction();            
            ws.Url = PowerPOS.SyncClientController.WS_URL;

            DataSet ds = 
                ws.FetchMembershipReport
                (dtpStartExpiryDate.Checked, dtpEndExpiryDate.Checked, dtpStartExpiryDate.Value, dtpEndExpiryDate.Value, 
                    dtpStartBirthDay.Checked, dtpEndBirthDay.Checked, dtpStartBirthDay.Value, dtpEndBirthDay.Value,
                    txtMembershipNoFrom.Text, txtMembershipNoTo.Text, int.Parse(cmbGroupName.SelectedValue.ToString()), 
                    "membershipno", "ASC",UserController.EncryptData(PowerPOS.Container.UserInfo.username));
                                
            if (ds != null)
            {
                dgvMember.DataSource = ds.Tables[0];
                dgvMember.Refresh();
            }
            else
            {
                dgvMember.DataSource = null;
                dgvMember.Refresh();
            }
            

        }

        private void frmMassMailer_Load(object sender, EventArgs e)
        {
            PowerPOS.SyncClientController.Load_WS_URL();
            CashierLogin cr = new CashierLogin();
            cr.ShowDialog();

            if (UserInfo.username != null)
            {
                MembershipWS.FetchMembersPastTransaction ws = new MembershipWS.FetchMembersPastTransaction();
                ws.Url = PowerPOS.SyncClientController.WS_URL;
                DataSet ds = ws.FetchMembershipGroupName();
                cmbGroupName.DataSource = ds.Tables[0];
                cmbGroupName.DisplayMember = "GroupName";
                cmbGroupName.ValueMember = "MembershipGroupID";
                dgvMember.AutoGenerateColumns = false;
            }

        }

        private void dgvMember_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvMember_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //if (!SubSonic.Sugar.Validation.IsEmail(dgvMember.Rows[e.RowIndex].Cells["email"].Value.ToString()))
            if (dgvMember.Rows[e.RowIndex].Cells["email"].Value.ToString() == "")            
            {
                for (int i = 0; i < dgvMember.ColumnCount; i++)
                {
                    if ((dgvMember.Columns[i].Visible))
                    {
                        dgvMember.Rows[e.RowIndex].Cells[i].Style.ForeColor = System.Drawing.Color.White;
                        dgvMember.Rows[e.RowIndex].Cells[i].Style.BackColor = System.Drawing.Color.DarkRed;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the application?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }

    public class SyncThread
    {
        public ViewMembershipCollection members;
        public ViewMembershipCollection failedMembers;
        public string FileName;
        public string MailSubject;
        public int sentCount;
        public string status;

        public void runSyncUpdates()
        {
            MassEmail ms = new MassEmail();
            
            failedMembers = ms.SendEmails
                (members, FileName, "promo@ahava.com.sg", MailSubject,
                    "", "", "mail.ahava.com.sg",
                    "Promo@ahava.com.sg", "12345", false, out status);
            
        }
    }
}