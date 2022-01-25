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
    public class GetQuotationDet : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string OrderHdrID = context.Request.Params["OrderHdrId"] ?? "";

            DataTable header = new DataTable();
            DataTable detail = new DataTable();
            string query = "";
            string query2 = "";
            string msg = "";

            
            query2 = @"Select * from QuotationDet where OrderHdrId = '{0}'";
            query2 = string.Format(query2, OrderHdrID);

            DataSet ds = DataService.GetDataSet(new QueryCommand(query2));
            detail = ds.Tables[0];

           

            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(detail));

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
