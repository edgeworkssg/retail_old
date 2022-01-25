using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using PowerPOS;
using PowerWeb;
using SpreadsheetLight;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace PowerWeb.Support
{
    public partial class SynchronizationMaster : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnImportProducts_Click(object sender, EventArgs e)
        {
            AppSetting.SetSetting(AppSetting.SettingsName.Integration.NeedToSyncProduct, "True");
        }

        protected void btnSendSales_Click(object sender, EventArgs e)
        {
            AppSetting.SetSetting(AppSetting.SettingsName.Integration.NeedToSyncSale, "True");
        }

        
    }
}
