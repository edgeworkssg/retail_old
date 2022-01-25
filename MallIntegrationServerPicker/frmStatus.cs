using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MallIntegrationServerPicker.Properties;

namespace MallIntegrationServerPicker
{
    public partial class frmStatus : Form
    {
        delegate bool setStatusCallback(string msg);
        delegate bool setActionCallback(string msg);

        private DateTime timeStamp;
        private string serverURL;
        private int totalNewOrders = 0;
        private string errorMessage;
        private bool isCheckError = false;
        private bool isChangeTimeStamp = false;
        private BackgroundWorker bgWorkerCheck = new BackgroundWorker();

        private const string ACTION_PROMPT = "There are {0} orders since the timestamp specified. Are you sure to proceed?";

        public frmStatus()
        {
            Init("", "");
        }

        public frmStatus(string status, string action)
        {
            Init(status, action);
        }

        public void Init(string status, string action)
        {
            InitializeComponent();
            lblStatus.Text = status;
            lblAction.Text = action;
            mtbTime.ValidatingType = Type.GetType("System.DateTime");
            serverURL = Settings.Default.WS_URL.TrimEnd('/') + "/Synchronization/MallIntegration.asmx";

            bgWorkerCheck.DoWork += new DoWorkEventHandler(bgWorkerCheck_DoWork);
            bgWorkerCheck.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerCheck_RunWorkerCompleted);
        }

        private void bgWorkerCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                isCheckError = false;
                isChangeTimeStamp = false;
                /*WinAutoPrint.MobileWS.MobileWS ws = new WinAutoPrint.MobileWS.MobileWS();
                ws.Url = serverURL;
                ws.Timeout = 60 * 1000;

                totalNewOrders = ws.GetNewOrderCountAfter(timeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                if (totalNewOrders > 0)
                {
                    DialogResult dialogResult = MessageBox.Show(string.Format(ACTION_PROMPT, totalNewOrders), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        isChangeTimeStamp = true;
                    }
                }
                else
                {
                    isChangeTimeStamp = true;
                }*/
            }
            catch (Exception ex)
            {
                // Failed
                isCheckError = true;
                errorMessage = "Error occured: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void bgWorkerCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isCheckError)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (isChangeTimeStamp)
                {
                    MallIntegrationServerPicker.Program.AppContext.ChangeTimeStamp(timeStamp);
                    groupBox1.Visible = false;
                }
            }
        }

        public bool setStatus(string msg)
        {
            if (lblStatus.InvokeRequired)
            {
                setStatusCallback d = new setStatusCallback(setStatus);
                Invoke(d, msg);
            }
            else
            {
                lblStatus.Text = msg;
            }
            return true;
        }

        public bool setAction(string msg)
        {
            if (lblAction.InvokeRequired)
            {
                setActionCallback d = new setActionCallback(setAction);
                Invoke(d, msg);
            }
            else
            {
                lblAction.Text = msg;
            }
            return true;
        }

        private void btnOverwrite_Click(object sender, EventArgs e)
        {
            dtpDate.Value = DateTime.Today;
            mtbTime.Text = DateTime.Now.ToString("HH:mm:ss");
            groupBox1.Visible = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (mtbTime.ValidateText() == null)
            {
                MessageBox.Show("Invalid time: " + mtbTime.Text);
                return;
            }
            DateTime.TryParse(dtpDate.Value.ToString("yyyy-MM-dd") + " " + mtbTime.Text, out timeStamp);
            bgWorkerCheck.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
    }
}
