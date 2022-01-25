using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPOSLib.Container;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CommissionTypeByQty : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = new ArrayList();
            data.Add(new
            {
                Text = "NON_COMMISSION",
                Value = "NON_COMMISSION"
            });
            data.Add(new DropdownResult()
            {
                Text = "NOT_OUTSTANDING",
                Value = "NOT_OUTSTANDING"
            });
            data.Add(new DropdownResult()
            {
                Text = "OPEN_PRODUCT",
                Value = "OPEN_PRODUCT"
            });
            data.Add(new DropdownResult()
            {
                Text = "OUTSTANDING",
                Value = "OUTSTANDING"
            });
            data.Add(new DropdownResult()
            {
                Text = "PACKAGE_REDEEM",
                Value = "PACKAGE_REDEEM"
            });
            data.Add(new DropdownResult()
            {
                Text = "PACKAGE_SOLD",
                Value = "PACKAGE_SOLD"
            });
            data.Add(new DropdownResult()
            {
                Text = "POINT_REDEEM",
                Value = "POINT_REDEEM"
            });
            data.Add(new DropdownResult()
            {
                Text = "POINT_SOLD",
                Value = "POINT_SOLD"
            });
            data.Add(new DropdownResult()
            {
                Text = "PRODUCT",
                Value = "PRODUCT"
            });
            data.Add(new DropdownResult()
            {
                Text = "SERVICE",
                Value = "SERVICE"
            });
            data.Add(new DropdownResult()
            {
                Text = "SYSTEM",
                Value = "SYSTEM"
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
