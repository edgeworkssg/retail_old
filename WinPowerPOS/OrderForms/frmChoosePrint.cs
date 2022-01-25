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

namespace WinPowerPOS.OrderForms
{
    public partial class frmChoosePrint : Form
    {
        public bool IsSuccessful;

        public frmChoosePrint()
        {
            InitializeComponent();
        }

        private void frmSalesPerson_Load(object sender, EventArgs e)
        {
            IsSuccessful = false;
            /*if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PrintReceiptAutomatically), false))
            {
                btnOk_Click(sender, e);
            }*/
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            IsSuccessful = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

    }
}