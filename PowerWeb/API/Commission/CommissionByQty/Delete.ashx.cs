using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using PowerPOSLib.Controller.Commission;
using Newtonsoft.Json;
using PowerPOSLib.Container;

namespace PowerWeb.API.Commission.CommissionByQty
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Delete : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            string uniqueIDRaw = context.Request.Params["UniqueID"];
            int uniqueID = 0;

            try
            {
                uniqueID = Int32.Parse(uniqueIDRaw);
            }
            catch (Exception) { }

            result = new CommissionBasedOnQtyController().Delete(uniqueID);

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
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
