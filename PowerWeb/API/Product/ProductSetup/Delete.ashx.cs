using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOS;
using SubSonic.Utilities;
using PowerPOSLib.Container;
using Newtonsoft.Json;
using SubSonic;

namespace PowerWeb.Product.Action
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Delete : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Details = "",
                Data = null
            };

            try
            {
                string itemNo = context.Request.Params["ItemNo"] ?? "";
                string userFlag1 = context.Request.Params["Userflag1"] == "Yes" ? "True" : "False";

                ItemController ctr = new ItemController();

                Boolean matrixmode = Convert.ToBoolean(userFlag1);
                if (matrixmode) 
                {
                    Query qr = new Query("Item");
                    qr.QueryType = QueryType.Update;
                    qr.AddUpdateSetting("Deleted", true);
                    qr.AddWhere("Attributes1", Comparison.Equals, itemNo);
                    qr.Execute();

                    result.Status = true;
                    result.Message = "Product has been deleted.";
                }
                else
                {
                    ctr.Delete(Utility.GetParameter("ItemNo"));
                    result.Status = true;
                    result.Message = "Product has been deleted.";
                }
            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot delete product.";
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
