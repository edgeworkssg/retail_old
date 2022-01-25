using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using PowerPOS;

namespace WinPowerPOS.MaintenanceForms
{
    public partial class frmMagentoSync : Form
    {

        bool isLocalhost;
        public frmMagentoSync()
        {
            InitializeComponent();
        }

        private void frmMagentoSync_Load(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void LoadCategory()
        {
            CategoryCollection CategoryColl = new CategoryCollection();
            //CategoryColl.Where(Category.Columns.Deleted, false);
            CategoryColl.Load();

            lbCategory.Items.Clear();
            foreach (Category cat in CategoryColl)
            {
                if (cat.Userint1 != null && cat.Userint1 > 0)
                {
                    lbCategory.Items.Add(cat.CategoryName);
                }
            }

        }

        private void btnSyncCategory_Click(object sender, EventArgs e)
        {
            SyncClientController.Load_WS_URL();
            isLocalhost = SyncClientController.WS_URL.StartsWith("http://localhost"); //|| SyncClientController.WS_URL.StartsWith("http://127.0.0.1");
            if (isLocalhost)
            {
                if (SyncClientController.SyncCategory())
                {
                    MessageBox.Show("Sync Category Success");
                    LoadCategory();
                }
                else
                {
                    MessageBox.Show("Sync Category Failed. Please check the log");
                }
            }
            else
            {
                
            }

        }

        private void btnSyncItem_Click(object sender, EventArgs e)
        {
            ShowPanel();
            foreach (object itemChecked in lbCategory.CheckedItems)
            {
                string categoryName = itemChecked.ToString();
                if (SyncClientController.SyncItem(categoryName))
                {
                    txtMsg.Text = "Sync Category " + categoryName + " Success \r\n" + txtMsg.Text;
                    
                }
                else
                {
                    txtMsg.Text = "Sync Category " + categoryName + " Failed \r\n" + txtMsg.Text;
                }
            }
            LoadCategory();
            HidePanel();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string user = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
            string password = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
            PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();
            ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
            string SessionId = ws.login(user, password);

            try
            {
                PowerPOSLib.Magento.storeEntity[] dump = ws.storeList(SessionId);
                foreach (PowerPOSLib.Magento.storeEntity ct in dump)
                {
                    txtMsg.Text = ct.code + "," + ct.name + "," + ct.store_id +" \r\n" + txtMsg.Text;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            //var dump1 = ws.directoryRegionList(SessionId,)
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string user = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
            string password = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
            PowerPOSLib.Magento.MagentoService ws = new PowerPOSLib.Magento.MagentoService();
            ws.Url = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
            string SessionId = ws.login(user, password);

            try
            {
                //PowerPOSLib.Magento.directoryRegionEntity[] dump = ws.shoppingCartPaymentList(SessionId, "SG");
                
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ShowPanel()
        {
            pnlWait.Visible = true;
            pnlWait.BringToFront();
            pnlWait.Refresh();
        }
        private void HidePanel()
        {
            pnlWait.Visible = false;
            pnlWait.SendToBack();
        }
    }
}
