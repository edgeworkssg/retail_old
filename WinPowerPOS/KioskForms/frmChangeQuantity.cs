using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.KioskForms
{
    public partial class frmChangeQuantity : Form
    {
        private POSController pos;

        private DataRowView drv;

        private decimal qty, default_qty;

        private string status;

        public bool isDecreaseQty = false; 
        public bool isIncreaseQty = false; 


        public frmChangeQuantity(POSController pos, DataRowView drv)
        {
            InitializeComponent();

            this.pos = pos;

            this.drv = drv;

            label1.Text = drv["ItemName"].ToString();
            label2.Text = drv["Quantity"].ToString();

            default_qty = qty = decimal.Parse(drv["Quantity"].ToString());

            btnPlus.Visible = false;
            btnMin.Visible = false;

            isDecreaseQty = false; 
            isIncreaseQty = false; 
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            qty += 1;

            label2.Text = qty.ToString("N0");

            //btnPlus.Enabled = qty < default_qty;
            btnMin.Enabled = qty > 0;
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            qty -= 1;

            label2.Text = qty.ToString("N0");

            //btnPlus.Enabled = qty < default_qty;
            btnMin.Enabled = qty > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            qty -= 1;
            
            decimal change_qty = 1;

            if (change_qty > 0)
            {
                isDecreaseQty = true;
                Logger.writeStaffAssistLog("INFO", "DECREASE QTY : " + Math.Abs(change_qty).ToString("N2"));
            }
            else if (change_qty < 0)
            {
                isIncreaseQty = true;
                Logger.writeStaffAssistLog("INFO", "INCREASE QTY : " + Math.Abs(change_qty).ToString("N2"));
            }
            else
                Logger.writeStaffAssistLog("INFO", "NO CHANGING");

            if (qty > 0)
                pos.ChangeOrderLineQuantity(drv["ID"].ToString(), qty, false, out status);
            else
            {
                OrderDet det = pos.GetLine(drv["ID"].ToString(), out status);
                if (det != null)
                    pos.RemoveLine(det);
            }

            Close();
        }

        private void frmChangeQuantity_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.writeStaffAssistLog("INFO", "FORM CHANGE QUANTITY IS CLOSED");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
