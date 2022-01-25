using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Controller.Commission;
using Newtonsoft.Json;

namespace PowerWeb.API.Commission.CommissionByPercentage
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
            CommissionBasedOnPercentageController controller = new CommissionBasedOnPercentageController();
            DataTable dt = controller.FetchCommissionBaseOnPercentageList();

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
