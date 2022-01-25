using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using PowerPOS;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace PowerWeb.API.Product.ItemAttributes
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Add : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var type = context.Request.Params["Type"];
            var value = context.Request.Params["Value"];

            JsonResult result = new JsonResult()
            {
                Status = false,
                Message = "",
                Details = "",
                Data = null
            };

            try
            {
                System.Transactions.TransactionOptions to = new System.Transactions.TransactionOptions();
                to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

                using (System.Transactions.TransactionScope transScope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required, to))
                {
                    ItemAttribute item = new ItemAttribute();
                    
                    item.Type = type;
                    item.ValueX = value;

                    item.Save(context.Session["username"].ToString());
                    transScope.Complete();

                    result.Status = true;
                    result.Message = "Product has been saved.";
                }

            }
            catch (Exception)
            {
                result.Status = false;
                result.Message = "Cannot save item attributes.";
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
