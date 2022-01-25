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
using PowerPOS.Container;

namespace PowerWeb.API.Membership
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetTotalMembership : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string paramMembershipPrefixCode = context.Request.Params["ParamMembershipPrefixCode"] ?? "";
            string paramMembershipNo = context.Request.Params["ParamMembershipNo"] ?? "";
            string paramCreatedOn = context.Request.Params["ParamCreatedOn"] ?? "";
            string strSql = "select count(*) as TotalMembership from Membership a where a.MembershipNo != '" + paramMembershipNo + "' and CreatedOn > '" + paramCreatedOn + "' and MembershipNo not like '" + paramMembershipPrefixCode + "%' ";
            QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");

            DataTable data = DataService.GetDataSet(cmd).Tables[0];
            context.Response.ContentType = "application/json";
            string result = JsonConvert.SerializeObject(data);
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
