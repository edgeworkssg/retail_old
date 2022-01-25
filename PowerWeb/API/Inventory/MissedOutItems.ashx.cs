using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using PowerPOS;
using System.Data;
using Newtonsoft.Json;

namespace PowerWeb.API.Inventory
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MissedOutItems : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ArrayList data = new ArrayList();

            //string filter = context.Request.Form["filter"];
            string filter = "";
            string InventoryLocationID = context.Request.Form["InventoryLocationID"];

            StockTakeController it = new StockTakeController();
            DataTable dt = StockTakeController.FetchMissedOutItemWithFilter(InventoryLocationID.GetIntValue(), filter);
            
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(dt));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
