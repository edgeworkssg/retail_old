using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using PowerPOSLib.Container;
using Newtonsoft.Json;

namespace PowerWeb.API.Product.ItemImporter
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SaveStatus : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = null;

            if (context.Session["SavingItemState"] != null)
            {
                if ((bool)context.Session["SavingItemState"] == true || context.Session["SavingItemState"].ToString() == "true")
                {
                    result.Status = true;
                    result.Message = "Product saved successfully.";
                }
                else if ((bool)context.Session["SavingItemState"] == false || context.Session["SavingItemState"].ToString() == "false")
                {
                    result.Status = false;
                    result.Message = "Cannot saving product from excel. Please contact administrator or check the error within the grid.";
                }
            }
            else
            {
                result.Status = false;
                result.Message = "Cannot saving product from excel. Please contact administrator or check the error within the grid.";
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
