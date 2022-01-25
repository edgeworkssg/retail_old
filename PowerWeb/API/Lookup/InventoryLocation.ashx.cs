using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using SubSonic;
using Newtonsoft.Json;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class InventoryLocation : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string strSql = "select Text = a.InventoryLocationName ";
            strSql += ", Value = a.InventoryLocationID ";
            strSql += "from InventoryLocation a ";
            strSql += "order by a.InventoryLocationName ";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(ds.Tables[0]));
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
