using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using SubSonic;
using PowerPOS;
using Newtonsoft.Json;

namespace PowerWeb.Product.Action
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadItem : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
           
            string itemNo = context.Request.Params["ItemNo"] ?? "";
            string userFlag1 = context.Request.Params["Userflag1"] == "Yes" ? "True" : "False";
            context.Response.ContentType = "application/json";
            
            Boolean matrixmode = Convert.ToBoolean(userFlag1);
            
            //different treatment for item matrix
            if (matrixmode) {
                Item itemmatrix = ItemController.GetDataItemMatrix(itemNo);
                context.Response.Write(JsonConvert.SerializeObject(itemmatrix));
            }else{
                Item item = new Item(itemNo);
                context.Response.Write(JsonConvert.SerializeObject(item));
            }
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
