using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using System.Data;
using SubSonic;

namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ItemGroup : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string strSql = "select Text = a.ItemGroupName ";
            strSql += ", Value = a.ItemGroupId ";
            strSql += "from ItemGroup a ";
            strSql += "Where ISNULL(a.Deleted,0) = 0 ";
            strSql += " order by a.ItemGroupName asc ";

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
