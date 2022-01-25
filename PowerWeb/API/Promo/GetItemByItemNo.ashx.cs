using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PowerPOS;
using Newtonsoft.Json;

namespace PowerWeb.API.Promo
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetItemByItemNo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string ItemNo = context.Request.Params["ItemNo"];
            Item it = new Item(ItemNo);

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(it));
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
