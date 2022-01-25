using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
namespace WinPowerPOS
{
    public partial class frmVoucher : Form
    {
        public bool IsSuccessful;
        public POSController pos;

        public frmVoucher()
        {
            IsSuccessful = false;
            InitializeComponent();
            txtVoucherNo.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            decimal result;
            if (CommonUILib.ValidateTextBoxAsUnsignedDecimal(txtAmount, out result))
            {
                string status;
                string OrderLineID = pos.IsItemIsInOrderLine(POSController.VOUCHER_BARCODE);
                if (OrderLineID == "")
                {
                    //change price
                    //add new item to order
                    if (pos.AddItemToOrder(new Item(POSController.VOUCHER_BARCODE), 1, 0, false, out status))
                    {
                        OrderLineID = pos.IsItemIsInOrderLine(POSController.VOUCHER_BARCODE);
                        if (!pos.ChangeOrderLineUnitPrice(OrderLineID, -result, out status))
                        {
                            MessageBox.Show("Error encountered: " + status);
                            return;
                        }
                        else
                        {
                            pos.SetLineRemark(OrderLineID, txtVoucherNo.Text, out status);
                            pos.ApplyMembershipDiscount();
                            IsSuccessful = true;
                            this.Close();
                        }
                    }
                }
                else
                {
                    if (!pos.ChangeOrderLineUnitPrice(OrderLineID, -result, out status))
                    {
                        MessageBox.Show("Error encountered: " + status);
                        return;
                    }
                    else
                    {
                        pos.SetLineRemark(OrderLineID, txtVoucherNo.Text, out status);
                        pos.ApplyMembershipDiscount();
                        IsSuccessful = true;
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter valid voucher amount.");
            }           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }
    }
}