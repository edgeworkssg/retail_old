using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPOSLib.Controller.Commission;

namespace PowerWeb.API.Commission.CommissionByQty
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
            CommissionBasedOnQtyController controller = new CommissionBasedOnQtyController();
            DataTable dt = controller.FetchCommissionBasedOnQtyList();
            var data = new
            {
                draw = 1,
                recordsTotal = dt.Rows.Count,
                recordsFiltered = dt.Rows.Count,
                data = dt
            };

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
