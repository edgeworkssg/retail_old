using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOS;
using Newtonsoft.Json;

namespace PowerWeb.Product.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Supplier : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var itemNo = context.Request.Form["ItemNo"];
            DataSet dsSupplier = SupplierItemMap.GetSupplierItemMapListByItemNo(itemNo);
            DataTable dtSupplier = dsSupplier.Tables[0].Copy();

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(dtSupplier));
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
