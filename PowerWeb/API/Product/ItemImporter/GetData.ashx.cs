using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetData : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            DataTable dt = (DataTable) context.Session[context.Request.Params["DataID"]];
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
