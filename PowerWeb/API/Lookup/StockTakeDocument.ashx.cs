using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using SubSonic;
using Newtonsoft.Json;

namespace PowerWeb.API
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class StockTakeDocument : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string strSql = @"select Text = ISNULL(Userfld3,''), Value = ISNULL(Userfld3,'') , MAX(modifiedon) as modifiedon
                                from StockTake where ISNULL(userfld3,'') <> ''
                                group BY ISNULL(Userfld3,'')
                                order by MAX(modifiedon) desc ";

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
