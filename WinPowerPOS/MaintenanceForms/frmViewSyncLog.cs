using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinPowerPOS.MaintenanceForms
{
    public partial class frmViewSyncLog : Form
    {
        public DataTable dt, dtSource;
        public string SyncSalesStatus;
        public string SyncQuotationStatus;
        public string SyncProductStatus;
        public string SyncInventoryStatus;
        public string SyncMembershipStatus;
        public string SyncMasterDataStatus;
        public string SyncUserStatus;
        public string SyncLogStatus;
        public string SyncAccessLogStatus;
        public string SyncCashRecordingStatus;
        public string SyncAppointmentStatus;
        public string SyncPerformanceLogStatus;
        public string SyncDeliveryOrderStatus;
        public string SyncRatingStatus;
        public string SyncAttendanceStatus;
        public string SyncVoucherStatus;

        delegate bool addSalesLogCallback(string msg);
        delegate bool addQuotationLogCallback(string msg);
        delegate bool addInventoryLogCallback(string msg);
        delegate bool addGetInventoryLogCallback(string msg);
        delegate bool addSyncLogsLogCallback(string msg);
        delegate bool addSyncAccessLogsLogCallback(string msg);
        delegate bool addProductLogCallback(string msg);
        delegate bool addMasterDataCallback(string msg);
        delegate bool addUserCallback(string msg);
        delegate bool addMemberCallback(string msg);
        delegate bool addCashRecordingCallback(string msg);
        delegate bool addAppointmentLogCallback(string msg);
        delegate bool addPerformanceLogCallback(string msg);
        delegate bool addDeliveryOrderLogCallback(string msg);
        delegate bool addRatingLogCallback(string msg);
        delegate bool addAttendanceLogCallback(string msg);
        delegate bool addVoucherCallback(string msg);

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        } 

        public frmViewSyncLog()
        {
            InitializeComponent();
        }

        public bool AddCashRecordingLog(string Message)
        {
            if (txtCashRecording.InvokeRequired)
            {
                addCashRecordingCallback d = new addCashRecordingCallback(AddCashRecordingLog);
                Invoke(d, Message);
            }
            else
            {
                txtCashRecording.Text = Message;
            }
            return true;
        }

        public bool AddSalesLog(string Message)
        {
            if (txtSyncSalesLog.InvokeRequired)
            {
                addSalesLogCallback d = new addSalesLogCallback(AddSalesLog);
                Invoke(d, Message);
            }
            else
            {
                txtSyncSalesLog.Text = Message;
            }
            return true;
        }

        public bool AddQuotationLog(string Message)
        {
            if (txtSynxQuotationLog.InvokeRequired)
            {
                addQuotationLogCallback d = new addQuotationLogCallback(AddQuotationLog);
                Invoke(d, Message);
            }
            else
            {
                txtSynxQuotationLog.Text = Message;
            }
            return true;
        }

        public bool AddInventoryLog(string Message)
        {
            if (txtSyncInventoryLog.InvokeRequired)
            {
                addInventoryLogCallback d = new addInventoryLogCallback(AddInventoryLog);
                Invoke(d, Message);
            }
            else
            {
                txtSyncInventoryLog.Text = Message;
            }
            return true;
        }

        public bool addGetInventoryLog(string Message)
        {
            if (txtMemberLog.InvokeRequired)
            {
                addGetInventoryLogCallback d = new addGetInventoryLogCallback(addGetInventoryLog);
                Invoke(d, Message);
            }
            else
            {
                txtMemberLog.Text = Message;
            }
            return true;
        }

        public bool addSyncLogsLog(string Message)
        {
            if (txtSyncLogs.InvokeRequired)
            {
                addSyncLogsLogCallback d = new addSyncLogsLogCallback(addSyncLogsLog);
                Invoke(d, Message);
            }
            else
            {
                txtSyncLogs.Text = Message;
            }
            return true;
        }

        public bool addSyncAccessLogsLog(string Message)
        {
            if (txtAccessLog.InvokeRequired)
            {
                addSyncAccessLogsLogCallback d = new addSyncAccessLogsLogCallback(addSyncAccessLogsLog);
                Invoke(d, Message);
            }
            else
            {
                txtAccessLog.Text = Message;
            }
            return true;
        }

        public bool AddProductLog(string Message)
        {
            if (txtProductLog.InvokeRequired)
            {
                addProductLogCallback d = new addProductLogCallback(AddProductLog);
                Invoke(d, Message);
            }
            else
            {
                txtProductLog.Text = Message;
            }
            return true;
        }

        public bool AddMasterDataLog(string Message)
        {
            if (txtMasterDataLog.InvokeRequired)
            {
                addMasterDataCallback d = new addMasterDataCallback(AddMasterDataLog);
                Invoke(d, Message);
            }
            else
            {
                txtMasterDataLog.Text = Message;
            }
            return true;
        }

        public bool AddUserLog(string Message)
        {
            if (txtUserLog.InvokeRequired)
            {
                addUserCallback d = new addUserCallback(AddUserLog);
                Invoke(d, Message);
            }
            else
            {
                txtUserLog.Text = Message;
            }
            return true;
        }

        public bool AddMemberLog(string Message)
        {
            if (txtMemberLog.InvokeRequired)
            {
                addMemberCallback d = new addMemberCallback(AddMemberLog);
                Invoke(d, Message);
            }
            else
            {
                txtMemberLog.Text = Message;
            }
            return true;
        }

        public bool AddAppointmentLog(string Message)
        {
            if (txtAppointmentLog.InvokeRequired)
            {
                addAppointmentLogCallback d = new addAppointmentLogCallback(AddAppointmentLog);
                Invoke(d, Message);
            }
            else
            {
                txtAppointmentLog.Text = Message;
            }
            return true;
        }

        public bool addSyncPerformanceLogLog(string Message)
        {
            if (txtPerformanceLog.InvokeRequired)
            {
                addPerformanceLogCallback d = new addPerformanceLogCallback(addSyncPerformanceLogLog);
                Invoke(d, Message);
            }
            else
            {
                txtPerformanceLog.Text = Message;
            }
            return true;
        }

        public bool AddDeliveryOrderLog(string Message)
        {
            if (txtDeliveryOrderLog.InvokeRequired)
            {
                addDeliveryOrderLogCallback d = new addDeliveryOrderLogCallback(AddDeliveryOrderLog);
                Invoke(d, Message);
            }
            else
            {
                txtDeliveryOrderLog.Text = Message;
            }
            return true;
        }

        public bool AddSyncRatingLog(string Message)
        {
            if (txtSyncRatingLog.InvokeRequired)
            {
                addRatingLogCallback d = new addRatingLogCallback(AddSyncRatingLog);
                Invoke(d, Message);
            }
            else
            {
                txtSyncRatingLog.Text = Message;
            }
            return true;
        }

        public bool AddSyncAttendanceLog(string Message)
        {
            if (txtAttendanceLog.InvokeRequired)
            {
                addAttendanceLogCallback d = new addAttendanceLogCallback(AddSyncAttendanceLog);
                Invoke(d, Message);
            }
            else
            {
                txtAttendanceLog.Text = Message;
            }
            return true;
        }

        public bool AddVoucherLog(string Message)
        {
            if (txtVoucherLog.InvokeRequired)
            {
                addVoucherCallback d = new addVoucherCallback(AddVoucherLog);
                Invoke(d, Message);
            }
            else
            {
                txtVoucherLog.Text = Message;
            }
            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
