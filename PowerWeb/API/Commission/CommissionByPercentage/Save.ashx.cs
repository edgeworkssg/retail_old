using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using PowerPOSLib.Controller.Commission;
using PowerPOSLib.Container;
using Newtonsoft.Json;

namespace PowerWeb.API.Commission.CommissionByPercentage
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
            var result = new JsonResult();
            result.Status = false;
            result.Message = "";
            result.Details = "";
            result.Data = "";

            string activeUser = context.Session["username"].ToString();
            string salesGroupIDRaw = context.Request.Params["SalesGroupID"];
            string commissionType = context.Request.Params["CommissionType"];
            string lowerLimitRaw = context.Request.Params["LowerLimit"];
            string upperLimitRaw = context.Request.Params["UpperLimit"];
            string percentCommissionRaw = context.Request.Params["PercentCommission"];
            decimal lowerLimit = 0;
            decimal upperLimit = 0;
            decimal percentCommission = 0;
            int salesGroupID = 0;

            try
            {
                salesGroupID = Int32.Parse(salesGroupIDRaw);
            }
            catch (Exception) { }

            try
            {
                lowerLimit = Decimal.Parse(lowerLimitRaw);
            }
            catch (Exception) { }

            try
            {
                upperLimit = Decimal.Parse(upperLimitRaw);
            }
            catch (Exception) { }

            try
            {
                percentCommission = Decimal.Parse(percentCommissionRaw);
            }
            catch (Exception) { }

            var commissionController = new CommissionBasedOnPercentageController();

            if (lowerLimit > upperLimit)
            {
                result.Status = false;
                result.Message = "Lower limit must be less or equal than upper limit.";
            }
            else if (percentCommission > 100)
            {
                result.Status = false;
                result.Message = "Percent commission must be less or equal 100.";
            }
            else if (commissionController.CheckIfDataExists(salesGroupID, commissionType, lowerLimit, upperLimit) > 0)
            {
                result.Status = false;
                result.Message = "Limit value conflict with another.";
            }
            else
            {
                result = commissionController.Save(salesGroupID, commissionType, lowerLimit, upperLimit, percentCommission, activeUser);
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
