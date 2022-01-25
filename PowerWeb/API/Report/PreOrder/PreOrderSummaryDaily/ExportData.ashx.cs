using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using SubSonic;
using System.IO;

namespace PowerWeb.API.Report.PreOrder.PreOrderSummaryDaily
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExportData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string outlet = context.Request.Params["Outlet"];
            string salesDate = context.Request.Params["SalesDate"];

            if (string.IsNullOrEmpty(outlet))
            {
                outlet = "ALL";
            }

            string strSql = "declare @OrderDate date; declare @OutletName varchar(50); " +
                "set @OrderDate = '2014-09-04'; set @OutletName = '';  " +
                "begin try drop table #raw_installment; end try begin catch end catch " +
                "begin try drop table #raw_payment; end try begin catch end catch " +
                "begin try drop table #raw_result; end try begin catch end catch " +
                "" +
                "select * into #raw_installment " +
                "from ( " +
                "select a.OrderHdrID , b.OrderDetID , a.MembershipNo , OrderDate = convert(date, a.OrderDate), b.ItemNo , c.ItemName , ItemDesc = isnull(c.ItemDesc, c.ItemName)  " +
                ", d.CategoryName , e.DepartmentName , b.Quantity , b.UnitPrice , b.Amount , g.PaymentType, Deposit = 0, Balance = 0, EstDel = '-' " +
                "from OrderHdr a inner join OrderDet b on b.OrderHdrID = a.OrderHdrID " +
                "inner join Item c on c.ItemNo = b.ItemNo " +
                "inner join Category d on d.CategoryName = c.CategoryName  " +
                "inner join ItemDepartment e on e.ItemDepartmentID = d.ItemDepartmentId " +
                "inner join ReceiptHdr f on f.OrderHdrID = a.OrderHdrID  " +
                "inner join ReceiptDet g on g.ReceiptHdrID = f.ReceiptHdrID " +
                "inner join PointOfSale h on h.PointOfSaleID = f.PointOfSaleID " +
                "where 1=1 and convert(date, a.OrderDate) = @OrderDate and g.PaymentType = 'INSTALLMENT'  " +
                "and c.ItemNo != 'MEMBER' and c.ItemNo != 'RENEWAL'  " +
                "and h.OutletName like ('%' + @OutletName + '%') " +
                ") as data; " +
                "" +
                "select * " +
                "into #raw_payment  " +
                "from ( " +
                "select a.OrderHdrID , a.OrderDetID, a.Amount, ForPayment = a.userfld3 , ForPaymentDet = a.userfld10 , d.PaymentType " +
                "from OrderDet a inner join OrderHdr b on b.OrderHdrID = a.OrderHdrID inner join ReceiptHdr c on c.OrderHdrID = b.OrderHdrID " +
                "inner join ReceiptDet d on d.ReceiptHdrID = c.ReceiptHdrID  " +
                "where a.ItemNo = 'INST_PAYMENT' and a.userfld3 in ( select distinct x.OrderHdrID from #raw_installment x ) " +
                "and a.userfld10 is not null  " +
                ") as data; " +
                "" +
                "select * " +
                "into #raw_result " +
                "from ( " +
                "select a.OrderHdrID , a.OrderDetID , a.MembershipNo , a.OrderDate , a.ItemNo " +
                ", a.ItemName , a.ItemDesc , a.CategoryName , a.DepartmentName " +
                ", a.Quantity , a.UnitPrice , a.Amount " +
                ", Deposit = isnull(( select sum(x.Amount) from #raw_payment x where x.ForPayment = a.OrderHdrID and x.ForPaymentDet = a.OrderDetID), 0) " +
                ", a.Balance , a.EstDel " +
                "from #raw_installment a " +
                ") as data; " +
                "" +
                "select *  " +
                "from ( " +
                "select a.PaymentType, a.Amount  " +
                "from #raw_payment a " +
                ") as data " +
                "pivot (  " +
                "sum (Amount)  " +
                "for [PaymentType] in ([CASH], [NETSX], [VISAX], [MASTER])  " +
                ") as #result_payment;  " +
                "" +
                "select @OrderDate as OrderDate; " +
                "" +
                "if @OutletName is null or @OutletName = ''  " +
                "select 'ALL' as OutletName; " +
                "else " +
                "select @OutletName as OutletName; " +
                "" +
                "" +
                "" +
                "" +
                "";
            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            ds.Tables[0].TableName = "PreOrderSummaryDaily_Payment";
            ds.Tables[1].TableName = "PreOrderSummaryDaily_OrderDate";
            ds.Tables[2].TableName = "PreOrderSummaryDaily_Outlet";
            
            ReportDocument crDoc = new ReportDocument();
            string reportPath = context.Request.PhysicalApplicationPath + "Reports\\ReportFiles\\PreOrder\\PreOrderSummary.rpt";
            crDoc.Load(reportPath);
         

            crDoc.SetDataSource(ds);
            MemoryStream rawData = (MemoryStream)crDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("Content-Disposition", "inline; filename=" + "Report.pdf");
            context.Response.AddHeader("Content-Length", rawData.Length.ToString());
            context.Response.BinaryWrite(rawData.ToArray());
            context.Response.End();
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
