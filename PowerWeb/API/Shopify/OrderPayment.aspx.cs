using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PowerPOS;

public partial class API_Shopify_OrderPayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int statusCode = 500;
        try
        {
            if (!IsPostBack)
            {
                var header = HttpContext.Current.Request.Headers["x-shopify-hmac-sha256"];
                string data = new StreamReader(Request.InputStream).ReadToEnd().ToString().Trim();
                Logger.writeLog(string.Format("Shopify>>OrderPayment:{0}", data));

                string status = "";
                if (ShopifyIntegrationController.AddShopifyIntegration(ShopifyEvent.OrderPayment, header, data, out status))
                    statusCode = 200;

            }
        }
        catch (Exception ex)
        {
            Logger.writeLog(ex);
        }
        Response.StatusCode = statusCode;
    }
}
