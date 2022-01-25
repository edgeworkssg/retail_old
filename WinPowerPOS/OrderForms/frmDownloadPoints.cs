using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOS.Container;
using System.Collections;
using SubSonic;

namespace WinPowerPOS.OrderForms
{
    public partial class frmDownloadPoints : Form
    {
        public bool IsSuccessful;
        public string membershipNo;
        public string orderHdrID;
        public bool isVoid;
        public POSController pos;

        public BackgroundWorker ParentSyncPointsThread;

        public frmDownloadPoints()
        {
            InitializeComponent();
            isVoid = false;
        }

        bool loop = true; 

        private void frmSalesPerson_Load(object sender, EventArgs e)
        {
            IsSuccessful = false;

            pos = new POSController(orderHdrID);
            
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            IsSuccessful = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bgDownloadPoints.CancelAsync();
            //IsSuccessful = false;
            //this.Close();
        }

        private void bgDownloadPoints_DoWork(object sender, DoWorkEventArgs e)
        {
            while (loop)
            {
                if (bgDownloadPoints.CancellationPending)
                {
                    // Indicate that the task was canceled.
                    e.Cancel = true;
                    loop = false;
                    break;
                }
                else
                {
                    // ParentSyncPointsThread is a backgroundworker running in frmOrderTaking to sync points when member is scanned.
                    if (ParentSyncPointsThread != null)
                    {
                        bool waiting = ParentSyncPointsThread.IsBusy;
                        while (waiting)
                        {
                            System.Threading.Thread.Sleep(500);
                            waiting = ParentSyncPointsThread.IsBusy;
                        }
                        loop = false;
                        IsSuccessful = true;
                        e.Result = true;
                    }
                    else
                    {
                        if (orderHdrID == "")
                        {
                            PowerPOS.SyncPointsController.SyncPointsRealTimeController snc = new PowerPOS.SyncPointsController.SyncPointsRealTimeController();
                            if (!snc.SyncPoints(membershipNo))
                            {
                                e.Result = false;
                                IsSuccessful = false;
                                loop = false;
                            }
                            else
                            {
                                loop = false;
                                IsSuccessful = true;
                                e.Result = true;
                            }
                        }
                        else
                        {
                            PointAllocationLogCollection pCol = new PointAllocationLogCollection();
                            pCol.Where(PointAllocationLog.Columns.OrderHdrID, orderHdrID);
                            if (isVoid)
                                pCol.Where(PointAllocationLog.Columns.Userfld2, Comparison.Like, "%VOID%");
                            pCol.Load();
                            if (pCol.Count > 0)
                            {
                                loop = false;
                                IsSuccessful = true;
                                e.Result = true;
                                //this.Close();
                            }
                            else
                            {
                                PowerPOS.SyncPointsController.SyncPointsRealTimeController snc = new PowerPOS.SyncPointsController.SyncPointsRealTimeController();
                                if (!snc.SyncPoints(membershipNo))
                                {
                                    e.Result = false;
                                    IsSuccessful = false;
                                    loop = false;
                                }
                                else
                                {
                                    loop = false;
                                    IsSuccessful = true;
                                    e.Result = true;
                                }
                            }
                        }
                    }


                }
            }
        }

        private void bgDownloadPoints_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            IsSuccessful = true;
            if (e.Cancelled)
            {
                loop = false;
                IsSuccessful = false;
                
            }
            this.Close();
        }

        private void frmDownloadPoints_Shown(object sender, EventArgs e)
        {
            if (orderHdrID == "")
            {
                if (!bgDownloadPoints.IsBusy)
                    bgDownloadPoints.RunWorkerAsync();
            }
            else
            {
                if (pos.MembershipApplied() && pos.GetMemberInfo().MembershipNo != "" && pos.GetMemberInfo().MembershipNo != "WALK-IN")
                {
                    if (!bgDownloadPoints.IsBusy)
                        bgDownloadPoints.RunWorkerAsync();
                }
                else
                {
                    IsSuccessful = true;
                    this.Close();
                }
            }
                
        }

    }
}