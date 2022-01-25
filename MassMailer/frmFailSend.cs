using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using PowerPOS;

namespace MassMailer
{
    public partial class frmFailSend : Form
    {
        public ViewMembershipCollection member;
        public string filename;
        public string subject;

        public frmFailSend()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
                    

                    pnlSendOrder.Visible = true;
                    pnlSendOrder.BringToFront();
                    pnlSendOrder.Refresh();

                    SyncThread pThread = new SyncThread();

                    pThread.members = member;
                    pThread.FileName = filename;
                    pThread.MailSubject = subject;
                
                    Thread oThread = new Thread(new ThreadStart(pThread.runSyncUpdates));
                    oThread.Start();
                    while (!oThread.Join(30))
                    {
                        
                        pnlSendOrder.Refresh();
                    }

                    pnlSendOrder.Visible = false;
                    pnlSendOrder.SendToBack();
                    pnlSendOrder.Refresh();

                    if (pThread.failedMembers.Count > 0)
                    {
                        MessageBox.Show("There are failed sending.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        member = pThread.failedMembers;
                        dgvMember.DataSource = null;
                        dgvMember.Refresh();
                        dgvMember.DataSource = member;
                        dgvMember.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Sending Successful.");
                        this.Close();
                    }
        }

        private void frmFailSend_Load(object sender, EventArgs e)
        {
            this.dgvMember.ClipboardCopyMode =
                DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            if (member.Count == 0) this.Close();
            dgvMember.AutoGenerateColumns = false;
            dgvMember.DataSource = member;
            dgvMember.Refresh();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            dgvMember.SelectAll();
            Clipboard.SetDataObject(
               this.dgvMember.GetClipboardContent());

        }
    }
}