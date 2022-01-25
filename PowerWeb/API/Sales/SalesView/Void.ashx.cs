using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPOS;
using System.Web.SessionState;
using PowerPOSLib.Container;

namespace PowerWeb.API.Sales.SalesView
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Void : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string orderHdrRefNo = context.Request.Params["OrderHdrRefNo"] ?? "";
            string remark = context.Request.Params["Remark"] ?? "";
            OrderHdr orderHdr = new OrderHdr("OrderRefNo", orderHdrRefNo);
            JsonResult result = new JsonResult();

            if (orderHdr != null)
            {
                orderHdr.IsVoided = true;
                orderHdr.Remark = remark;

                orderHdr.Save((string)context.Session["username"] ?? "");
                result.Status = true;
                result.Message = "Order has been voided.";
                result.Details = "";
                result.Data = null;
            }
            else
            {
                result.Status = false;
                result.Message = "Cannot void order.";
                result.Details = "";
                result.Data = null;
            }

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
