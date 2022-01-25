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
using PowerPOS.Container;

namespace WinPowerPOS
{
    public partial class frmAddNewItemWeb : Form
    {
        public frmAddNewItemWeb()
        {
            InitializeComponent();
        }

        private void frmAddNewItemWeb_Load(object sender, EventArgs e)
        {
            var text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentText);
            this.Text =  text != null ? text : "";
            //SyncClientController.Load_WS_URL();
            //string address = SyncClientController.WS_URL.ToLower().Replace("synchronization/synchronization.asmx","") + "product/productmaster.aspx?passcode=31179" + PointOfSaleInfo.PointOfSaleID.ToString();
            // string address = @"http://www.2ezcommerces.com/client/pd1923/pdadmin";
            string address = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentURL);
            webBrowser1.Url = new Uri(address);
            webBrowser1.Refresh();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (e.Url.AbsoluteUri == @"http://www.2ezcommerces.com/client/pd1923/pdadmin")
            if (e.Url.AbsoluteUri == AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentURL))
            {
                var doc = webBrowser1.Document;
                var msgs = doc.GetElementById("messages");
                if (msgs.InnerHtml == null)
                {
                    var user = doc.GetElementById("username");
                    var pass = doc.GetElementById("login");
                    var frm = doc.GetElementById("loginForm");

                    user.SetAttribute("value", AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentUser));
                    pass.SetAttribute("value", AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentPassword));
                    frm.InvokeMember("submit");
                }
            }
        }
    }
}
