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

namespace PowerWeb.API.Sales.SalesView
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class List : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            const int VIEW_BILL_LIMIT = 365; //Number of days behind when we can see the bill....
            string startDate = context.Request.Params["StartDate"] ?? "";
            string endDate = context.Request.Params["EndDate"] ?? "";
            string refNo = context.Request.Params["RefNo"] ?? "";
            string cashier = context.Request.Params["Cashier"] ?? "";
            string status = context.Request.Params["Status"] ?? "";
            string remark = context.Request.Params["Remark"] ?? "";
            string name = context.Request.Params["Name"] ?? "";

            string isVoidedValue = "";

            if (status == "Voided")
            {
                isVoidedValue = "1";
            }
            else if (status == "Not Voided")
            {
                isVoidedValue = "0";
            }

            refNo = refNo.Replace("RCP", "").Replace("OR", "");

            DataTable data = ReportController.FetchTransactionReportForViewSalesWeb(startDate, endDate, refNo, cashier, "", PointOfSaleInfo.OutletName, remark, isVoidedValue, name);

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
