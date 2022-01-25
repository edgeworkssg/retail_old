using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SubSonic;
using Newtonsoft.Json;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ItemAll : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string strSql = "select Text = a.ItemNo + ' - ' + a.ItemName ";
            strSql += ", Value = a.ItemNo ";
            strSql += "from Item a ";
            strSql += "where ISNULL(a.Deleted,0) = 0 ";
            strSql += "order by a.ItemName asc";

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
