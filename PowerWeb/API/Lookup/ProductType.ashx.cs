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
    public class ProductType : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ArrayList data = new ArrayList();
            data.Add(new
            {
                Text = "Product",
                Value = "Product"
            });
            data.Add(new
            {
                Text = "Open Price Product",
                Value = "OpenPriceProduct"
            });
            data.Add(new
            {
                Text = "Service",
                Value = "Service"
            });
            data.Add(new
            {
                Text = "Point Package",
                Value = "PointPackage"
            });
            data.Add(new
            {
                Text = "Course Package",
                Value = "CoursePackage"
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
