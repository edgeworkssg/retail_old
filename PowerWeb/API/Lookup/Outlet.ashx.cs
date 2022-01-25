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
    public class POS : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string strSql = "Select Text = a.OutletName  ";
            strSql += ", Value = a.OutletName  ";
            strSql += "from Outlet a  ";
            strSql += "where isnull(Deleted, 0) = 0  ";
            strSql += "order by a.OutletName asc  ";

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
