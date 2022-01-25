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
    public class ItemAttributes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string strSql = "select a.Type, a.Value ";
            strSql += "from ItemAttributes a ";
            strSql += "where ISNULL(a.Deleted, 'false') = 'false' ";
            strSql += "order by a.Value asc";

            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            //ds.Tables[0].Merge(ds.Tables[1]);
            //return ds.Tables[0];

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
