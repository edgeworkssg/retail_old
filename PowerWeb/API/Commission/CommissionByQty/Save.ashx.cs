using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOSLib.Container;
using PowerPOSLib.Controller.Commission;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace PowerWeb.API.Commission.CommissionByQty
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
            result.Data = null;

            string activeUser = context.Session["username"].ToString();
            string salesGroupIDRaw = context.Request.Params["SalesGroupID"];
            string itemNo = context.Request.Params["ItemNo"];
            string quantityRaw = context.Request.Params["Quantity"];
            string amountCommissionRaw = context.Request.Params["AmountCommission"];
            string commissionType = context.Request.Params["CommissionType"];
            int salesGroupID = 0;
            int quantity = 0;
            decimal? amountCommission = 0;

            try
            {
                salesGroupID = Int32.Parse(salesGroupIDRaw);
            }
            catch (Exception) { }

            try
            {
                quantity = Int32.Parse(quantityRaw);
            }
            catch (Exception) { }

            try
            {
                amountCommission = Decimal.Parse(amountCommissionRaw);
            }
            catch (Exception) { }


            var commissionController = new CommissionBasedOnQtyController();
            if (quantity == 0)
            {
                result.Status = false;
                result.Message = "Quantity cannot filled with 0.";
            }
            //else if (amountCommission > 100)
            //{
            //    result.Status = false;
            //    result.Message = "Amount commission must be less or equal 100.";
            //}
            else if (commissionController.CheckIfDataExists(salesGroupID, itemNo, commissionType) > 0)
            {
                result.Status = false;
                result.Message = "There is existing data with the same Group, Commission Type and Item.";
            }
            else
            {
                result = commissionController.Save(salesGroupID, itemNo, quantity, amountCommission, activeUser, commissionType);
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
