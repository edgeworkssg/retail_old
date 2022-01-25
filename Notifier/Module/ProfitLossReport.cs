using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;
using System.Data;
using System.Net.Mail;
using Notifier.Properties;
using System.Net;
using PowerReport;
using CrystalDecisions.CrystalReports.Engine;
using System.Reflection;
using System.IO;
using Notifier.Helper;

namespace Notifier.Module
{
    public class ProfitLossReport: MailBase
    {
        public override string ModuleReport
        {
            get
            {
                return "ProfitLossReport";
            }
        }
        public override string ReportFilename
        {
            get { return "ProfitLossReport.rpt"; }
        }

        public bool IsReportLoaded;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProfitLossReport(): base()
        {
            this.StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
            this.EndDate = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1, 23, 59, 59).AddDays(-1);
        }

        private static ProfitLossReport instance;
        public static ProfitLossReport Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ProfitLossReport();
                }

                return instance;
            }
        }

        private bool IsNoData(out DataTable dt, out string strMailBody)
        {
            try
            {
                bool isSaleTally = true;
                dt = new DataTable();

                string SQL =                 
                "SELECT ItemDepartment.DepartmentName, " +
                "Item.ItemNo, " + 
                "Item.ItemName, " + 
                "SUM(OrderDet.Quantity) AS TotalQuantity,  " +
                "Sum(OrderDet.Amount) AS TotalAmount, " +
                "Sum(OrderDet.GSTAmount) AS GSTAmount,  " +
                "(Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount)) AS TotalAmountWithoutGST,  " +
                "CASE WHEN OrderDet.IsVoided = 1 THEN 'VOID' ELSE 'NOT VOID' END AS TransactionStatus, " + 
                "'ALL' AS PointOfSaleName, " + 
                "Outlet.OutletName, " +
                "Item.CategoryName, " + 
                "Item.ProductLine" +
                    ", ISNULL(Attributes1,'') Attributes1, ISNULL(Attributes2,'') Attributes2, ISNULL(Attributes3,'') Attributes3 " +
                    ", ISNULL(Attributes4,'') Attributes4, ISNULL(Attributes5,'') Attributes5, ISNULL(Attributes6,'') Attributes6 " +
                    ", ISNULL(Attributes7,'') Attributes7 " +
                ", isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold, " + 
                "Sum(OrderDet.Discount) AS Discount, " +
                "isnull((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss, " +
                "isnull((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/((Sum(OrderDet.Amount)- Sum(OrderDet.GSTAmount))+0.001)*100,0) as ProfitLossPercentage " +

                "FROM ItemDepartment Inner Join Category on ItemDepartment.ItemDepartmentID = Category.ItemDepartmentID inner join Item on Category.CategoryName = Item.CategoryName INNER JOIN " +
                "  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN " +
                "  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN " +
                "  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN " +
                "  Outlet ON PointOfSale.OutletName = Outlet.OutletName  " +
                "LEFT outer jOIN " +
                "ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO " +
                "AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO) " +
                "WHERE  (OrderHdr.OrderDate BETWEEN CONVERT(datetime, '" + StartDate.ToString("MM/dd/yyyy HH:mm:ss") + "', 101) AND CONVERT(datetime, '" + EndDate.ToString("MM/dd/yyyy HH:mm:ss") + "', 101)) " +
                "   AND Outlet.OutletName Like '%' " +
                "   AND Item.CategoryName Like '%' " +
                "   AND OrderHdr.IsVoided = 0 " +
                    "AND orderdet.itemno <> 'inst_payment' "+
                "GROUP BY ItemDepartment.DepartmentName,Item.ItemNo, Item.ItemName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName, Item.ProductLine " +
                    ", ISNULL(Attributes1,''), ISNULL(Attributes2,''), ISNULL(Attributes3,'') " +
                    ", ISNULL(Attributes4,''), ISNULL(Attributes5,''), ISNULL(Attributes6,'') " +
                    ", ISNULL(Attributes7,'') ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                dt = DataService.GetDataSet(cmd).Tables[0];

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<h3>PROFIT/LOSS REPORT</h3>");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("<table style=\"border: 1px solid black; font-size: 12px;\">");
                sb.AppendLine("<tr>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Department</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Item No</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Item Name</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Quantity</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Total</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">GST</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Total With No GST</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Transaction Status</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Point of Sale</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Outlet</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Category</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Product Line</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr1</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr2</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr3</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr4</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr5</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr6</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Attr7</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Total COG Sold</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Discount</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Profit/Loss</td>");
                sb.AppendLine("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">Profit/Loss(%)</td>");
                sb.AppendLine("<tr>");

                foreach (DataRow item in dt.Rows)
                {
                    sb.AppendLine("<tr>");
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["DepartmentName"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["ItemNo"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["ItemName"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["TotalQuantity"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["TotalAmount"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["GSTAmount"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["TotalAmountWithoutGST"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["TransactionStatus"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["PointOfSaleName"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["OutletName"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["CategoryName"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["ProductLine"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes1"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes2"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes3"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes4"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes5"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes6"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Attributes7"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["TotalCostOfGoodsSold"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["Discount"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["ProfitLoss"]).AppendLine();
                    sb.AppendFormat("<td style=\"border: 1px solid black; margin: 0px 5px 0px 5px;\">{0}</td>", item["ProfitLossPercentage"]).AppendLine();
                    sb.AppendLine("<tr>");
                }

                sb.AppendLine("</table>");
                strMailBody = sb.ToString();

                return dt.Rows.Count <= 0;
            }
            catch (Exception ex)
            {
                throw new ProfitLossReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public bool VerificationReportData()
        {
            try
            {
                if (Settings.Default.UseCrystalReport)
                {
                    string exportedPath = this.GenerateReport(string.Format(@"{0}\Reports\{1}", Directory.GetParent(Assembly.GetAssembly(this.GetType()).Location), this.ReportFilename), CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<h3>PROFIT/LOSS REPORT</h3>");
                    sb.AppendLine();
                    sb.AppendLine();
                    if (Settings.Default.UseLocalMailSetting)
                    {
                        this.SentLocalNotificationMail(Settings.Default.ProfitLossReport_MailSubject, true, sb.ToString(), exportedPath);
                    }
                    else
                    {
                        this.Message.Body = sb.ToString();
                        this.Message.Subject = Settings.Default.ProfitLossReport_MailSubject;

                        foreach (string item in this.GetMailReceivers())
                        {
                            this.Message.To.Add(item);
                        }

                        this.SentNotificationMail(true, exportedPath);
                    }
                    return true;
                }
                else
                {
                    DataTable dt = new DataTable();
                    string strMailBody = string.Empty;

                    //if report is not tally then send mail, if not then otherwise
                    if (!IsNoData(out dt, out strMailBody))
                    {
                        if (Settings.Default.UseLocalMailSetting)
                        {
                            this.SentLocalNotificationMail(Settings.Default.ProfitLossReport_MailSubject, true, strMailBody);
                        }
                        else
                        {
                            this.Message.Body = strMailBody;
                            this.Message.Subject = Settings.Default.ProfitLossReport_MailSubject;

                            foreach (string item in this.GetMailReceivers())
                            {
                                this.Message.To.Add(item);
                            }

                            this.SentNotificationMail(true);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (MailBaseException mbex)
            {
                throw mbex;
            }
            catch (Exception ex)
            {
                throw new ProfitLossReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public bool VerificationReportData(DateTime startDate, DateTime endDate)
        {
            try
            {
                this.StartDate = startDate;
                this.EndDate = endDate;

                DataTable dt = new DataTable();
                string strMailBody = string.Empty;

                //if report is not tally then send mail, if not then otherwise
                if (!IsNoData(out dt, out strMailBody))
                {
                    this.Message.Body = strMailBody;

                    foreach (string item in this.GetMailReceivers())
                    {
                        this.Message.To.Add(item);
                    }

                    this.SentNotificationMail(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (MailBaseException mbex)
            {
                throw mbex;
            }
            catch (Exception ex)
            {
                throw new ProfitLossReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }

        public override SortedList<string, string> GetCRParam()
        {
            try
            {
                SortedList<string, string> parameters = new SortedList<string, string>();
                #region *) Get the Filter Value that User Keyed in
                parameters.Add("StartDate", this.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
                parameters.Add("EndDate", this.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                #endregion
                return parameters;
            }
            catch (Exception ex)
            {
                throw new ProfitLossReportException(
                    string.Format("An error occured when executing [{0}] method. Please see inner exception.",
                    System.Reflection.MethodBase.GetCurrentMethod().Name),
                    ex);
            }
        }
    }

    [global::System.Serializable]
    public class ProfitLossReportException : Exception
    {
        public ProfitLossReportException() { }
        public ProfitLossReportException(string message) : base(message) { }
        public ProfitLossReportException(string message, Exception inner) : base(message, inner) 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("============================================================");
            sb.AppendFormat("Logger [{0}]", DateTime.Now.ToString("dd MMM yyyy HH:mm:ss")).AppendLine();
            sb.AppendLine("============================================================");
            sb.AppendFormat("Caption: {0}", message).AppendLine();
            sb.AppendFormat("Message: {0}", inner.Message).AppendLine();
            sb.AppendFormat("Inner Message: {0}", inner.InnerException.Message).AppendLine();
            sb.AppendFormat("Stack Trace: [{0}]", inner.StackTrace).AppendLine().AppendLine();
            Logger.WriteLogToFile(sb.ToString());
        }
        protected ProfitLossReportException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
