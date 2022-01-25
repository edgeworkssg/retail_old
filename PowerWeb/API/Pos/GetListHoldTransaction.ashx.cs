using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using SubSonic;
using Newtonsoft.Json;

namespace PowerWeb.API.Pos
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetListHoldTransaction : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var data = new HoldTransactionServerModel();
            string strSql = @"
                select * 
                  from HoldTransaction 
                 where deleted=0;
            ";

            QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
            DataTableCollection rawData = DataService.GetDataSet(cmd).Tables;
            data.HoldList = rawData[0];

            string sqlUpdate = @"Update HoldTransaction 
                                set deleted=1
                                where deleted=0";

            QueryCommand qcUpdate = new QueryCommand(sqlUpdate);
            DataService.ExecuteQuery(qcUpdate);


            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(data));
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
