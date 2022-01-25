using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using System.Diagnostics;

namespace WinPowerPOS.OrderForms
{
    public partial class frmVoidRemark : Form
    {
        public POSController pos;
        public bool IsSuccess = false;
        private int clickNo = 0;
        public bool isRefund = false;
        public bool returnInventory = true;
        public frmVoidRemark()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtVoidReason.Text.Length <= 0)
            {
                MessageBox.Show(string.Format("{0} Reason is compulsory. Please enter the reason for {1}!", isRefund ? "Refund": "Void",isRefund ? "refund" : "voiding" ));
                return;
            }
            if (pos.myOrderHdr.OrderDate > POSController.FetchLatestCloseCounterTime(pos.myOrderHdr.PointOfSaleID))
            {

                pos.myOrderHdr.Remark = pos.myOrderHdr.Remark + "\n"+ string.Format("{0} REASON: ", isRefund ? "REFUND" : "VOID") + txtVoidReason.Text;
                returnInventory = chkReturnInventory.Checked;
                IsSuccess = true;
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccess = false;
            this.Close();
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            clickNo++;
            switch (clickNo)
            {
                case 1: OpenWindows8TouchKeyboard(); break;
                case 2: CloseOnscreenKeyboard(); clickNo = 0; break;
            }      
        }

        private static void OpenWindows8TouchKeyboard()
        {
            var path = @"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe";
            Process.Start(path);

        }

        private void CloseOnscreenKeyboard()
        {
            //Kill all on screen keyboards
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            if (oskProcessArray.Length > 0)
            {
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
        }

        private void frmVoidRemark_FormClosing(object sender, FormClosingEventArgs e)
        {
            Process[] oskProcessArray = Process.GetProcessesByName("TabTip");
            if (oskProcessArray.Length > 0)
            {
                foreach (Process onscreenProcess in oskProcessArray)
                {
                    onscreenProcess.Kill();
                }
            }
        }

        private void frmVoidRemark_Load(object sender, EventArgs e)
        {
            if (isRefund)
                label1.Text = "REFUND REASON";
        }
    }
}
