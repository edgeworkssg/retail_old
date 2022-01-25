using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImportStatus : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            if (context.Session["ImportingItemState"] != null)
            {
                if ((bool)context.Session["ImportingItemState"] == true || context.Session["ImportingItemState"].ToString() == "true")
                {
                    result.Status = true;
                    result.Message = "Item imported successfully.";
                }
                else if ((bool)context.Session["ImportingItemState"] == false || context.Session["ImportingItemState"].ToString() == "false")
                {
                    result.Status = false;
                    result.Message = "Cannot import item from template. Please contact administrator or check the error within the grid.";
                }
            }
            else
            {
                result.Status = false;
                result.Message = "Cannot import item from template. Please contact administrator or check the error within the grid.";   
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
