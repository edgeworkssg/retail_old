using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PaymentTypes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = new ArrayList();
            data.Add(new
            {
                Text = "CASH",
                Value = "CASH"
            });
            data.Add(new
            {
                Text = "CHEQUE",
                Value = "CHEQUE"
            });
            data.Add(new
            {
                Text = "MASTERCARD",
                Value = "MASTERX"
            });
            data.Add(new
            {
                Text = "MISC",
                Value = "MISC"
            });
            data.Add(new
            {
                Text = "NETS",
                Value = "NETSX"
            });
            data.Add(new
            {
                Text = "VISA",
                Value = "VISAX"
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
