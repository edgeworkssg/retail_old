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
using System.Web.SessionState;

namespace PowerWeb.API.NewMembership
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetNewMember : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string startTime = DateTime.Now.AddMinutes(-130).ToString("yyyy-MM-dd HH:mm:ss");
            string strSql = "select *, convert(varchar,ExpiryDate,120) as MemberExpiryDate from Membership a where a.CreatedOn between '" + startTime + "' and '" + endTime + "'";
            QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
            DataTable data = DataService.GetDataSet(cmd).Tables[0];

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
