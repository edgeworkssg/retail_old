using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinPowerPOS.PromoAdmin;
using WinVoucherBatchGenerator;
using WinPowerPOS.MembershipForms;
using PowerPOS;

namespace WinPowerPOS
{
    public partial class frmMainAdmin : Form
    {
        public frmMainAdmin()
        {
            InitializeComponent();
        }
        frmVoucherList FfrmVoucherList;
        private void btnVoucherList_Click(object sender, EventArgs e)
        {

            if (FfrmVoucherList == null || FfrmVoucherList.IsDisposed)
            {
                FfrmVoucherList = new frmVoucherList();
                FfrmVoucherList.Show();
            }
            else
            {
                FfrmVoucherList.WindowState = FormWindowState.Normal;
                FfrmVoucherList.Activate();
            }
        }
        frmListOfDiscountByItemGroup FfrmListOfDiscountByItemGroup;
        private void btnPromoByGroupList_Click(object sender, EventArgs e)
        {
            if (FfrmListOfDiscountByItemGroup == null || FfrmListOfDiscountByItemGroup.IsDisposed)
            {
                FfrmListOfDiscountByItemGroup = new frmListOfDiscountByItemGroup();
                FfrmListOfDiscountByItemGroup.Show();
            }
            else
            {
                FfrmListOfDiscountByItemGroup.WindowState = FormWindowState.Normal;
                FfrmListOfDiscountByItemGroup.Activate();
            }
        }
        frmListOfDiscountByItem FfrmListOfDiscountByItem;
        private void btnPromoByItemList_Click(object sender, EventArgs e)
        {
            if (FfrmListOfDiscountByItem == null || FfrmListOfDiscountByItem.IsDisposed)
            {
                FfrmListOfDiscountByItem = new frmListOfDiscountByItem();
                FfrmListOfDiscountByItem.Show();
            }
            else
            {
                FfrmListOfDiscountByItem.WindowState = FormWindowState.Normal;
                FfrmListOfDiscountByItem.Activate();
            }
        }
        frmGenerateVoucher FfrmGenerateVoucher;
        private void btnCreateVoucher_Click(object sender, EventArgs e)
        {
            if (FfrmGenerateVoucher == null || FfrmGenerateVoucher.IsDisposed)
            {
                FfrmGenerateVoucher = new frmGenerateVoucher();
                FfrmGenerateVoucher.Show();
            }
            else
            {
                FfrmGenerateVoucher.WindowState = FormWindowState.Normal;
                FfrmGenerateVoucher.Activate();
            }

        }
        frmMembershipPointImporter FfrmMembershipPointImporter;
        private void btnImportPoints_Click(object sender, EventArgs e)
        {
            if (FfrmMembershipPointImporter == null || FfrmMembershipPointImporter.IsDisposed)
            {
                FfrmMembershipPointImporter = new frmMembershipPointImporter();
                FfrmMembershipPointImporter.Show();
            }
            else
            {
                FfrmMembershipPointImporter.WindowState = FormWindowState.Maximized;
                FfrmMembershipPointImporter.Activate();
            }
        }

        private void frmMainAdmin_Load(object sender, EventArgs e)
        {
            string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForLogin);
            bool IsAuthorized = false;
            if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
            {
                groupBox1.Select();
                groupBox1.Focus();
                frmReadMSR f = new frmReadMSR();
                //f.privilegeName = PrivilegesController.;
                f.loginType = LoginType.Login;
                f.ShowDialog();
                IsAuthorized = f.IsAuthorized;
                if (IsAuthorized)
                {
                }
                else
                {
                    Application.Exit();
                    return;
                }
                f.Dispose();
            }
            else
            {
                LoginForms.frmPOSLogin f = new WinPowerPOS.LoginForms.frmPOSLogin();
                f.allowClose = false;
                f.ShowDialog();
                if (f.IsSuccessful)
                {

                }
                else
                {
                    Application.Exit();
                    return;
                }
                f.Dispose();
            }
        }

        private void btnPromoByLocation_Click(object sender, EventArgs e)
        {

        }
    }
}
