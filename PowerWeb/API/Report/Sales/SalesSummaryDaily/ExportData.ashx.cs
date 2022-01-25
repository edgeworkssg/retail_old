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

namespace PowerWeb.API.Report.Sales.SalesSummaryDaily
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

            string strSql = "declare @Date date;  ";
            strSql += "declare @OutletName varchar(100); ";
            strSql += "set @OutletName = '" + outlet + "'; ";
            strSql += "set @Date = '" + salesDate + "';   "; //2013-06-15
            strSql += "begin try   ";
            strSql += "drop table #raw_order; ";
            strSql += "end try  ";
            strSql += "begin catch   ";
            strSql += "end catch  ";
            strSql += "begin try  ";
            strSql += "drop table #raw_receipt;   ";
            strSql += "end try   ";
            strSql += "begin catch  ";
            strSql += "end catch  ";
            strSql += "select * into #raw_order from (   ";
            strSql += "select a.OrderHdrID   ";
            strSql += " , b.OrderDetID  ";
            strSql += " , a.OrderDate  ";
            strSql += ", e.DepartmentName  ";
            strSql += ", d.CategoryName  ";
            strSql += ", c.ItemNo  ";
            strSql += ", c.ItemName  ";
            strSql += ", c.ItemDesc  ";
            strSql += ", b.Quantity  ";
            strSql += ", b.UnitPrice   ";
            strSql += ", b.Amount   ";
            strSql += "from OrderHdr a   ";
            strSql += "inner join OrderDet b   ";
            strSql += "on b.OrderHdrID = a.OrderHdrID   ";
            strSql += "inner join Item c  ";
            strSql += "on c.ItemNo = b.ItemNo   ";
            strSql += "and c.ItemNo != 'INST_PAYMENT'   ";
            strSql += "inner join Category d   ";
            strSql += "on d.CategoryName = c.CategoryName   ";
            strSql += "inner join ItemDepartment e  ";
            strSql += "on e.ItemDepartmentID = d.ItemDepartmentId   ";
            strSql += "inner join PointOfSale f  ";
            strSql += "on f.PointOfSaleID = a.PointOfSaleID  ";
            strSql += " inner join Outlet g  ";
            strSql += "on g.OutletName = f.OutletName  ";
            strSql += "where convert(date, a.OrderDate) = @Date   ";
            strSql += " and g.OutletName like ('%' + @OutletName + '%')  ";
            strSql += ") as raw_data;   ";
            strSql += "select Category = (a.DepartmentName + ' ' + a.CategoryName)  ";
            strSql += ", a.ItemNo  ";
            strSql += ", a.ItemName  ";
            strSql += ", isnull(a.ItemDesc, a.ItemName) as ItemDesc  ";
            strSql += " , Qty = sum(a.Quantity)  ";
            strSql += ", a.UnitPrice ";
            strSql += ", SalesAmount = sum(a.Amount)  ";
            strSql += "from #raw_order a   ";
            strSql += "group by (a.DepartmentName + ' ' + a.CategoryName)  ";
            strSql += ", a.ItemNo  ";
            strSql += ", a.UnitPrice ";
            strSql += ", a.ItemName  ";
            strSql += "  , a.ItemDesc;  ";
            strSql += "select * into #raw_receipt  ";
            strSql += "from (  ";
            strSql += "select a.ReceiptHdrID  ";
            strSql += " , b.ReceiptDetID  ";
            strSql += " , b.PaymentType  ";
            strSql += ", b.Amount  ";
            strSql += " , a.OrderHdrID ";
            strSql += "from ReceiptHdr a  ";
            strSql += "inner join ReceiptDet b  ";
            strSql += "on b.ReceiptHdrID = a.ReceiptHdrID  ";
            strSql += "inner join OrderHdr c  ";
            strSql += "on c.OrderHdrID = a.OrderHdrID  ";
            strSql += "inner join PointOfSale d  ";
            strSql += "on d.PointOfSaleID = c.PointOfSaleID  ";
            strSql += "inner join Outlet e  ";
            strSql += "on e.OutletName = d.OutletName  ";
            strSql += " where convert(date, a.ReceiptDate) = @Date  ";
            strSql += "and d.OutletName like ('%' + @OutletName + '%')  ";
            strSql += ") as data;  ";
            strSql += "select *  ";
            strSql += "from ( ";
            strSql += "select a.PaymentType as PaymentType  ";
            strSql += ", sum(a.Amount) as SalesAmount  ";
            strSql += "from #raw_receipt a  ";
            strSql += "group by a.PaymentType ";
            strSql += ") as data  ";
            strSql += "pivot ( ";
            strSql += "sum(SalesAmount) ";
            strSql += "for [PaymentType] in (CASH, MASTER, NETS, VISA)	 ";
            strSql += ") as clean_data;  ";
            strSql += "select @Date as SalesDate;  ";
            strSql += "if @OutletName is null or @OutletName = ''   ";
            strSql += "select 'ALL' as OutletName; ";
            strSql += "else ";
            strSql += "select @OutletName as OutletName; ";
            strSql += " ";

            ReportDocument crDoc = new ReportDocument();
            string reportPath = context.Request.PhysicalApplicationPath + "Reports\\ReportFiles\\Sales\\SalesSummaryDailyReport.rpt";
            crDoc.Load(reportPath);
            DataSet ds = DataService.GetDataSet(new QueryCommand(strSql, "PowerPOS"));
            ds.Tables[0].TableName = "SalesSummaryDaily_List";
            ds.Tables[1].TableName = "SalesSummaryDaily_PaymentType";
            ds.Tables[2].TableName = "SalesSummaryDaily_SalesDate";
            ds.Tables[3].TableName = "SalesSummaryDaily_Outlet";

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
