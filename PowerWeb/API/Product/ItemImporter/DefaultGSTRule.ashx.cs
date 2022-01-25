using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Configuration;
using Newtonsoft.Json;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DefaultGSTRule : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string gstRuleRaw = "Non GST";
            int gstRule = 0;

            try
            {
                gstRuleRaw = ConfigurationManager.AppSettings["DefaultGSTRule"].ToString();
            }
            catch (Exception) { 
                gstRuleRaw = "Non GST";
            }

            switch (gstRuleRaw)
            {
                case "Non GST":
                    gstRule = 3;
                    break;
                case "Inclusive GST":
                    gstRule = 2;
                    break;
                case "Exclusive GST":
                    gstRule = 1;
                    break;
            };

            context.Response.ContentType = "text/plain";
            context.Response.Write(gstRule);
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
