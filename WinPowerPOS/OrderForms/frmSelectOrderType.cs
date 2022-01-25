using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    /// <summary>
    /// To allow for selection of different Order type (Cash and Carry or Preorder) in the Sales Invoicing page. 
    /// If Preorder is selected, an additional screen will be initiated for input of delivery time and address.
    /// Cancel / Closing the form will return Dialog Result as DialogResult.Cancel
    /// </summary>
    public partial class frmSelectOrderType : Form
    {
        public string OrderType = "";

        public frmSelectOrderType()
        {
            InitializeComponent();

            string cashCarryText = AppSetting.GetSetting(AppSetting.SettingsName.Order.ReplaceCashCarryTextWith);
            if (!string.IsNullOrEmpty(cashCarryText))
                btnCashCarry.Text = cashCarryText;

            string preOrderText = AppSetting.GetSetting(AppSetting.SettingsName.Order.ReplacePreOrderTextWith);
            if (!string.IsNullOrEmpty(preOrderText))
                btnPreorder.Text = preOrderText;
        }

        private void btnCashCarry_Click(object sender, EventArgs e)
        {
            OrderType = "CASH_CARRY";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnPreorder_Click(object sender, EventArgs e)
        {
            OrderType = "PREORDER";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
