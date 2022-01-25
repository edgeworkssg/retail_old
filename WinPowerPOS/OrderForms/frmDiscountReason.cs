using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;

namespace WinPowerPOS.OrderForms
{
    public partial class frmDiscountReason : Form
    {
        public bool IsSuccessful = false;
        public string DiscountReason = "";
        public frmDiscountReason()
        {
            InitializeComponent();

            try
            {
                string[] discReason = (AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableDiscountReason) + "").Split(';').OrderBy(o=>o).ToArray();
                cmbDiscReason.Items.Clear();
                foreach (var item in discReason)
                {
                    cmbDiscReason.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IsSuccessful = false;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbDiscReason.Text))
            {
                DiscountReason = cmbDiscReason.Text;
                IsSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Discount reason cannot be empty");
            }
        }
    }
}
