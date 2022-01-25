using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;

namespace WinPowerPOS.OrderForms
{
    public partial class frmRequestSalesDate : Form
    {
        public bool IsSuccessful;
        public DateTime dateResult;
        public frmRequestSalesDate()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int MaxBackdate = int.Parse(AppSetting.GetSetting(AppSetting.SettingsName.Order.BackdateMaxDays));
            dateResult = dateTimePicker1.Value;
            if (dateResult > DateTime.Now)
            {
                CommonUILib.ShowErrorMessage("You cannot enter future dated sales");
                return;
            }

            if (dateResult < DateTime.Today.AddDays(-1 * MaxBackdate))
            {
                CommonUILib.ShowErrorMessage("You cannot backdate sales more than " + MaxBackdate.ToString() + " days");
                //MessageBox.Show();
                return;
            }
            /*if (dateResult < DateTime.Today)
            {
                DialogResult dr = MessageBox.Show("The sales is backdated. Are you sure you want to proceed?", "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                    return;
            }*/
            
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