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
    public partial class frmSelectFunding : Form
    {
        public string FundingMethod = "";
        public bool ShowPAMed = true;
        public bool ShowSMF = true;

        public frmSelectFunding()
        {
            InitializeComponent();
        }

        private void frmSelectFunding_Load(object sender, EventArgs e)
        {
            btnPAMed.Visible = ShowPAMed && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            btnSMF.Visible = ShowSMF && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
        }

        private void btnNoFunding_Click(object sender, EventArgs e)
        {
            FundingMethod = "NoFunding";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnPAMed_Click(object sender, EventArgs e)
        {
            FundingMethod = POSController.PAY_PAMEDIFUND;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSMF_Click(object sender, EventArgs e)
        {
            FundingMethod = POSController.PAY_SMF;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
