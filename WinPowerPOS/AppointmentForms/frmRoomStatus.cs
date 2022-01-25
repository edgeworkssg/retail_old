using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.AppointmentRealTimeController;
using SubSonic;
using WinPowerPOS.MaintenanceForms;
using PowerPOS.Container;
using System.Threading;

namespace WinPowerPOS.AppointmentForms
{
    public partial class frmRoomStatus : Form
    {
        public BackgroundWorker SyncAppointmentThread;

        public frmRoomStatus()
        {
            InitializeComponent();

            #region Real Time Download Appointment

            //Init sync
            //SyncDownloadAppointmentThread = new BackgroundWorker();
            //DoWorkEventArgs doe = new DoWorkEventArgs(null);
            //SyncDownloadAppointmentThread.WorkerSupportsCancellation = true;
            //SyncDownloadAppointmentThread.DoWork += new DoWorkEventHandler(SyncDownloadAppointmentThread_DoWork);
            //SyncDownloadAppointmentThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SyncDownloadAppointmentThread_RunWorkerCompleted);

            #endregion
        }

        private void frmRoomStatus_Load(object sender, EventArgs e)
        {
            
            try
            {
                #region realtime
                //if (!PointOfSaleInfo.IntegrateWithInventory)
                //{
                //    pnlProgress.Visible = true;
                //    this.Enabled = false;
                //    SyncDownloadAppointmentThread.RunWorkerAsync();
                //}

                #endregion

                SetButton();
            }
            catch (Exception exception)
            {
                frmError.ShowError(this, exception);
            }
        }

        private void btnResource_Click(object sender, EventArgs e)
        {
            try
            {
                frmCheckIn ci = new frmCheckIn(((Button)sender).Tag.ToString());
                ci.SyncAppointmentThread = SyncAppointmentThread;
                ci.ShowDialog();

                if (ci.IsSuccessful)
                {
                    if (ci.RoomStatus)
                    {
                        ((Button)sender).BackgroundImage = WinPowerPOS.Properties.Resources.redbutton;
                    }
                    else
                    {
                        ((Button)sender).BackgroundImage = WinPowerPOS.Properties.Resources.blueButton;
                    }
                }

                ci.Dispose();
                SetButton();
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        public void SetButton()
        {
            flowLayoutPanel1.Controls.Clear();

            DataTable dt = ResourceController.GetAvailableResource();

            
            if (dt.Rows.Count > 0)
            {
                //populate the flow lay out with programmable button
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Button b = new Button();
                    b.Width = 140;
                    b.Height = 60;
                    b.ForeColor = System.Drawing.Color.White;
                    if ((int)dt.Rows[i]["AvailableQty"] == 0)
                    {
                        b.BackgroundImage = WinPowerPOS.Properties.Resources.redbutton;
                    }
                    else
                    {
                        b.BackgroundImage = WinPowerPOS.Properties.Resources.blueButton;
                    }
                    b.Font = new Font("Verdana", 12, FontStyle.Bold);
                    b.Text = dt.Rows[i]["ResourceName"].ToString() + Environment.NewLine + "Available :" + dt.Rows[i]["AvailableQty"].ToString();
                    b.Tag = dt.Rows[i]["ResourceID"].ToString();
                    b.Click += delegate
                    {
                        btnResource_Click(b, new EventArgs());
                    };
                    flowLayoutPanel1.Controls.Add(b);
                }
            }
        }

        private void frmRoomStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SetDayFromMainForm()
        {
            pnlProgress.Visible = true;
            this.Enabled = false;
            SetButton();
            this.Enabled = true;
            pnlProgress.Visible = false;
        }

        #region realtimesync

        //private void SyncDownloadAppointmentThread_DoWork(object sender, DoWorkEventArgs e)
        //{
            
        //    try
        //    {
        //        SyncAppointmentRealTimeController ssc = new SyncAppointmentRealTimeController();
        //        ssc.OnProgressUpdates += scc_OnProgressUpdates;
        //        bool result = ssc.DownloadAppointment(e);
        //        e.Result = result;

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.writeLog(ex);
        //    }
        //}

        //private void SyncDownloadAppointmentThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    pnlProgress.Visible = false;
        //    if (!(bool)e.Result)
        //    {
        //        MessageBox.Show("Error sync appointment. Please check your internet connection.");

        //    }
        //    else
        //    {
               
        //        SetButton();
        //    }
        //    this.Enabled = true;
        //}

        //protected void scc_OnProgressUpdates(object sender, string message)
        //{
        //    if (fSyncLog == null)
        //    {
        //        fSyncLog = new frmViewSyncLog();
        //    }
        //    fSyncLog.SyncAppointmentStatus = message;
        //    addLog("appointment", message);
        //}

        //private void addLog(string type, string msg)
        //{

        //    msg = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss - ") + msg;
        //    fSyncLog.AddAppointmentLog(msg);

        //}

        #endregion
    }
}
