using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace PowerWeb.Product.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GST : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ArrayList data = new ArrayList();
            data.Add(new
            {
                Text = "Exclusive GST",
                Value = "1"
            });
            data.Add(new
            {
                Text = "Inclusive GST",
                Value = "2"
            });
            data.Add(new
            {
                Text = "Non GST",
                Value = "3"
            });

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(data));
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
