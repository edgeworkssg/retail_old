using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Newtonsoft.Json;
using PowerPOS;
using PowerPOS.Container;
using PowerPOSLib.Container;
using SubSonic;
using System.Globalization;
using PowerPOSLib.Helper;
using System.Web.SessionState;

namespace PowerWeb.API.NewMembership
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Save : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            JsonResult result = new JsonResult();
            string parentSurname = StringHelper.ConvertToProperCase(context.Request.Params["ParentSurname"] ?? "");
            string parentGivenName = StringHelper.ConvertToProperCase(context.Request.Params["ParentGivenName"] ?? "");
            string parentNRIC = StringHelper.ConvertToProperCase(context.Request.Params["ParentNRIC"] ?? "");
            string parentHomeAddress = StringHelper.ConvertToProperCase(context.Request.Params["ParentHomeAddress"] ?? "");
            string parentContactNo = StringHelper.ConvertToProperCase(context.Request.Params["ParentContactNo"] ?? "");
            string parentEmail = (context.Request.Params["ParentEmail"] ?? "").ToLower();
            string child1Surname = StringHelper.ConvertToProperCase(context.Request.Params["Child1Surname"] ?? "");
            string child1GivenName = StringHelper.ConvertToProperCase(context.Request.Params["Child1GivenName"] ?? "");
            string child1DateOfBirth = StringHelper.ConvertToProperCase(context.Request.Params["Child1DateOfBirth"] ?? "");
            string child2Surname = StringHelper.ConvertToProperCase(context.Request.Params["Child2Surname"] ?? "");
            string child2GivenName = StringHelper.ConvertToProperCase(context.Request.Params["Child2GivenName"] ?? "");
            string child2DateOfBirth = StringHelper.ConvertToProperCase(context.Request.Params["Child2DateOfBirth"] ?? "");
            string expiryDate = StringHelper.ConvertToProperCase(context.Request.Params["ExpiryDate"] ?? "");
            string chkAgreement = StringHelper.ConvertToProperCase(context.Request.Params["chkAgreement"] ?? "");
            string chkMagazines = StringHelper.ConvertToProperCase(context.Request.Params["chkMagazines"] ?? "");
            string chkOnlineSearch = StringHelper.ConvertToProperCase(context.Request.Params["chkOnlineSearch"] ?? "");
            string chkOnlineMedia = StringHelper.ConvertToProperCase(context.Request.Params["chkOnlineMedia"] ?? "");
            string chkFriends = StringHelper.ConvertToProperCase(context.Request.Params["chkFriends"] ?? "");
            string chkOther = StringHelper.ConvertToProperCase(context.Request.Params["chkOther"] ?? "");
            string passCode = StringHelper.ConvertToProperCase(context.Request.Params["Passcode"] ?? "");
            string username = (context.Session["username"] ?? "").ToString();

            string[] partsOfTheName = parentGivenName.Split(' ');
            string firstName = "";
            string lastName = "";
            int i = 0;

            //if (string.IsNullOrEmpty(child1GivenName) == true)
            //{
            //    result.Status = false;
            //    result.Message = "1st child name must be filled!";

            //    context.Response.ContentType = "application/json";
            //    context.Response.Write(JsonConvert.SerializeObject(result));

            //    return;
            //}

            if (!chkAgreement.ToLower().Equals("true"))
            {
                result.Status = false;
                result.Message = "Please check the agreement!";
            }
            else
            {

                foreach (string name in partsOfTheName)
                {
                    if (i == 0)
                    {
                        firstName = name;
                    }
                    else
                    {
                        lastName += name + " ";
                    }

                    i++;
                }
                lastName = lastName.Trim();

                try
                {
                    string strSql = "select top 1 a.MembershipGroupId from MembershipGroup a where a.GroupName = 'Normal'";
                    QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
                    int defaultMembershipId = (int)DataService.ExecuteScalar(cmd);

                    string knowFrom = "";
                    if (chkFriends.ToLower().Equals("true"))
                        knowFrom += ",Friends";
                    if (chkMagazines.ToLower().Equals("true"))
                        knowFrom += ",Magazines";
                    if (chkOnlineSearch.ToLower().Equals("true"))
                        knowFrom += ",Online Search";
                    if (chkOnlineMedia.ToLower().Equals("true"))
                        knowFrom += ",Online Media";
                    if (chkOther.ToLower().Equals("true"))
                        knowFrom += ",Other";
                    if(knowFrom.Length > 0)
                        knowFrom = knowFrom.Remove(0, 1);

                    PowerPOS.Membership member = new PowerPOS.Membership();
                    member.MembershipNo = MembershipController.getNewMembershipNo(PointOfSaleInfo.MembershipPrefixCode);
                    member.NameToAppear = parentGivenName.Trim() + " " + parentSurname.Trim();
                    member.FirstName = firstName;
                    member.LastName = parentSurname;
                    member.Email = parentEmail;
                    member.Nric = parentNRIC.ToUpper();
                    member.StreetName = parentHomeAddress;
                    member.Mobile = parentContactNo;
                    member.Child1Surname = child1Surname;
                    member.Child1GivenName = child1GivenName;
                    member.Child1DateOfBirth = child1DateOfBirth;
                    member.Child2Surname = child2Surname;
                    member.Child2GivenName = child2GivenName;
                    member.Child2DateOfBirth = child2DateOfBirth;
                    member.KnowFrom = knowFrom;
                    member.PassCode = passCode;
                    //member.ExpiryDate = DateTime.ParseExact(expiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    member.ExpiryDate = DateTime.Now.AddYears(1);
                    member.MembershipGroupId = defaultMembershipId;
                    member.Deleted = false;
                    member.Userflag5 = true;
                    member.Save(username);

                    result.Status = true;
                    result.Message = "Member has been registered.";
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    result.Status = false;
                    result.Message = "Member cannot registered.<br />Please, contact your administrator!";
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(result));
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
