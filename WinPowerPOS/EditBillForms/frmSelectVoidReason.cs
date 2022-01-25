using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.EditBillForms
{
    public partial class frmSelectVoidReason : Form
    {
        public bool IsSuccess = false;
        public string Reason = "";
        public bool isRefund = false;
        public bool returnInventory = true;
        public POSController pos;

        public frmSelectVoidReason()
        {
            InitializeComponent();
            string selectableRemark = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableVoidReason);
            cmbVoidReason.DataSource = selectableRemark.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
            cmbVoidReason.Refresh();
            cmbVoidReason.SelectedIndex = 0;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (cmbVoidReason.SelectedItem != null)
            {
                IsSuccess = true;
                if(!string.IsNullOrEmpty(pos.myOrderHdr.Remark))
                    pos.myOrderHdr.Remark = pos.myOrderHdr.Remark + "\n" + label1.Text +":" + cmbVoidReason.SelectedItem;
                else
                    pos.myOrderHdr.Remark = label1.Text +":" + cmbVoidReason.SelectedItem;
                Reason = pos.myOrderHdr.Remark;
                returnInventory = chkReturnInventory.Checked;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccess = false;
            this.Close();
        }

        private void frmSelectVoidReason_Load(object sender, EventArgs e)
        {
            if (isRefund)
                label1.Text = "REFUND REASON";
        }
    }
}
