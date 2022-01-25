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
using PowerPOS;
using PowerPOS.Container;
namespace PowerWeb.API.Membership
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetMembership : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string count = context.Request.Params["Count"] ?? "0";
            string paramMembershipNo = context.Request.Params["ParamMembershipNo"] ?? "";
            string paramMembershipPrefixCode = context.Request.Params["ParamMembershipPrefixCode"] ?? "";
            string paramCreatedOn = context.Request.Params["ParamCreatedOn"] ?? "";
            string strSql = "select top " + count + " *, convert(varchar,ExpiryDate,120) as MemberExpiryDate, convert(varchar, a.CreatedOn, 120) as MemberCreatedOn, convert(varchar, a.ModifiedOn, 120) as MemberModifiedOn, convert(varchar, a.DateOfBirth, 120) as MemberDateOfBirth, convert(varchar, a.SubscriptionDate, 120) as MemberSubscriptionDate from Membership a where CreatedOn > '" + paramCreatedOn + "' and MembershipNo != '" + paramMembershipNo + "' and MembershipNo not like '" + paramMembershipPrefixCode + "%' order by CreatedOn asc, ModifiedOn asc";

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
