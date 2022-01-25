using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPOS;

namespace PowerWeb.Product.Action
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class List : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ArrayList data = new ArrayList();

            string filterProductId = context.Request.Form["filterProductId"];
            string filterProductName = context.Request.Form["filterProductName"];

            ItemController it = new ItemController();
            //DataTable dt = it.SearchItem_PlusPointInfo(filterProductId, false);
            DataTable dt = it.SearchItem_ProductSetup(filterProductId, false);
            dt = CommonUILib.DataTableConvertBoolColumnToYesOrNo(dt);

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
