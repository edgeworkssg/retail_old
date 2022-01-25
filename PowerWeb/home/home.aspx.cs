using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PowerPOS;
using SubSonic;
using System.Threading;
using System.Globalization;

public partial class home : PageBase
{
    private const string SESSION_KEY_LANGUAGE = "CURRENT_LANGUAGE";
    protected void Page_Load(object sender, EventArgs e)
    {
        //bltErrorList.Items.Clear();

        //CheckPendingStockTake();

        //lblErrorList.Visible = (bltErrorList.Items.Count > 0);
    }

    //private void CheckPendingStockTake()
    //{
    //    string SQLString = "SELECT COUNT(*) FROM StockTake WHERE IsAdjusted = 0";
        
    //    object ObjRst = DataService.ExecuteScalar(new QueryCommand(SQLString));
    //    if (ObjRst == null) return;

    //    int Rst=0;
    //    if (int.TryParse(ObjRst.ToString(),out Rst ))
    //        if (Rst > 0)
    //        {
    //            bltErrorList.Items.Add("There are Stock Take waiting to be adjusted. Please do take note that all the Inventory Transactions and Inventory Sales Deductions are frozen at this moment and this may affect the accuracy of your stock. Please go to Adjust Stock Take on your POS program and adjust the Stock Take.");
    //        }
    //}

    [System.Web.Services.WebMethod]
    public static string GetIframeLink()
    {
        string result = string.Empty;

        result = PowerWeb.Properties.Settings.Default.HomeIframeLink;

        return result;
    }
}
