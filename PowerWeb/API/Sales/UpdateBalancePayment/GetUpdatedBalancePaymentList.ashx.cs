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
using PowerPOSLib.Container;

namespace PowerWeb.API.Sales.UpdateBalancePayment
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetUpdatedBalancePaymentList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string orderDate = context.Request.Params["OrderDate"] ?? "";

            var data = new UpdateBalancePaymentModel();
            string strSql = @"
                declare @OrderDate  datetime = '" + orderDate + @"';
                
                begin try 
                    if object_id('#temp_01') is not null drop table #temp_01;
                end try 
                begin catch end catch;

                select distinct OrderHdrID into #temp_01
                  from OrderDet a
                 where a.OrderDetDate >= @OrderDate
				   and a.ItemNo = 'INST_PAYMENT';
                
                select *
                  from OrderHdr a 
                 where a.OrderHdrID in ( select x.OrderHdrID from #temp_01 x );

                select *
                  from OrderDet a 
                 where a.OrderHdrID in ( select x.OrderHdrID from #temp_01 x );

                select *
                  from ReceiptHdr a 
                 where a.OrderHdrID in ( select x.OrderHdrID from #temp_01 x );

                select * 
                  from ReceiptDet a
                 where a.ReceiptHdrID in ( select x.OrderHdrID from #temp_01 x );

            ";
            QueryCommand cmd = new QueryCommand(strSql, "PowerPOS");
            DataTableCollection rawData = DataService.GetDataSet(cmd).Tables;
            data.OrderHdrList = rawData[0];
            data.OrderDetList = rawData[1];
            data.ReceiptHdrList = rawData[2];
            data.ReceiptDetList = rawData[3];

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
