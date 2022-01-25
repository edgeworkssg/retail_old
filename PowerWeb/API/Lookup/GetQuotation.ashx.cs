using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using PowerPOS;
using Newtonsoft.Json;
using PowerPOSLib.Container;
using SubSonic;


namespace PowerWeb.API.Lookup
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetQuotation : IHttpHandler
    {
        public QuotationHdr myOrderHdr;             //Order Header        
        public QuotationDetCollection myOrderDet;   //Order Detail

        public void ProcessRequest(HttpContext context)
        {
            string OrderHdrID = context.Request.Params["OrderHdrId"] ?? "";           

            DataTable header = new DataTable();
            DataTable detail = new DataTable();
            string query = "";
            string query2 = "";
            string msg = "";

            query = @"Select * from QuotationHdr where OrderHdrId = '{0}' or Userfld5 = '{0}'";
            //query2 = @"Select * from QuotationDet where OrderHdrId = '{0}'";

            query = string.Format(query, OrderHdrID);
            //query2 = string.Format(query2, OrderHdrID);

            DataSet ds = DataService.GetDataSet(new QueryCommand(query));
            header = ds.Tables[0];

            //DataSet ds2 = DataService.GetDataSet(new QueryCommand(query2));
            //detail= ds2.Tables[0];
            //context.Response.ContentType = "application/json";
            //context.Response.Write(JsonConvert.SerializeObject(myOrderHdr));

            context.Response.ContentType = "application/json";
            //string result = JsonConvert.SerializeObject(header);
            context.Response.Write(JsonConvert.SerializeObject(header));

        }

        //public void ProcessRequest(HttpContext context)
        //{
        //    string OrderHdrID = context.Request.Params["OrderHdrId"] ?? "";
        //    QuotationServerModel QuotationModel = new QuotationServerModel();
        //    myOrderHdr = new QuotationHdr(OrderHdrID);
        //    myOrderDet = new QuotationDetCollection().Where("OrderHdrID", OrderHdrID).Load();
        //    QuotationModel.Quotation = myOrderHdr;
        //    QuotationModel.QuotationDet = myOrderDet;       


        //    context.Response.ContentType = "application/json";
        //    context.Response.Write(JsonConvert.SerializeObject(QuotationModel));

        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
