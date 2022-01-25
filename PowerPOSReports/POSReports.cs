using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PowerPOS;
using SubSonic;
using System.Collections;
namespace PowerPOSReports
{
    public class POSReports
    {
        public static void FetchSettlementSalesReport
            (string CounterCloseID, bool IsWithCashOut, out ViewCloseCounterReportCollection settlement,
             out DataTable ProductSalesReport, out DataTable RefundSalesReport,
             out ViewVouchersSoldCollection VoucherSoldReport, out ViewVoucherRedeemedCollection VoucherRedeemedReport)
        {
            DataTable CashRecordingReport;
            CounterCloseDetCollection CounterCloseDetReport;
            FetchSettlementSalesReport(CounterCloseID, IsWithCashOut, out settlement, out ProductSalesReport, out RefundSalesReport, out VoucherSoldReport, out VoucherRedeemedReport, out CashRecordingReport, out CounterCloseDetReport);
        }
        
        public static void FetchSettlementSalesReport
            (string CounterCloseID, bool IsWithCashOut, out ViewCloseCounterReportCollection settlement,
             out DataTable ProductSalesReport, out DataTable RefundSalesReport,
             out ViewVouchersSoldCollection VoucherSoldReport, out ViewVoucherRedeemedCollection VoucherRedeemedReport, 
             out DataTable CashRecordingReport, out CounterCloseDetCollection CounterCloseDetReport)
        {
            //Settlement
            settlement = new ViewCloseCounterReportCollection();
            settlement.Where(ViewCloseCounterReport.Columns.CounterCloseID, CounterCloseID);
            settlement.Load();

            if (settlement.Count > 0)
            {
                //Product Sales Report
                //if (IsWithCashOut)
                //{
                //    ProductSalesReport =
                //        ReportController.FetchProductCategorySalesReportWithCashOut
                //        (settlement[0].StartTime, settlement[0].EndTime,
                //           settlement[0].PointOfSaleName, settlement[0].OutletName, "",
                //           settlement[0].DepartmentID.ToString(), false, Category.Columns.CategoryName, "asc");
                //}
                //else {
                    ProductSalesReport =
                        ReportController.FetchProductCategorySalesReport
                        (settlement[0].StartTime, settlement[0].EndTime,
                           settlement[0].PointOfSaleName, settlement[0].OutletName, "",
                           settlement[0].DepartmentID.ToString(), false, Category.Columns.CategoryName, "asc");
                //}

                //Refund Report
                RefundSalesReport =
                    ReportController.FetchRefundProductCategorySalesReport
                    (settlement[0].StartTime, settlement[0].EndTime,
                       settlement[0].PointOfSaleName, settlement[0].OutletName, "",
                       settlement[0].DepartmentID.ToString(), false, Category.Columns.CategoryName, "asc");

                //Cash Recording Report
                CashRecordingReport = ReportController.FetchCashOutReport(settlement[0].StartTime, settlement[0].EndTime,
                       settlement[0].PointOfSaleName, settlement[0].OutletName, settlement[0].DepartmentID.ToString());

                //Voucher Sold Report
                VoucherSoldReport = new ViewVouchersSoldCollection();
                VoucherSoldReport.Where(
                    ViewVouchersSold.Columns.OrderDate, Comparison.GreaterOrEquals, settlement[0].StartTime);
                VoucherSoldReport.Where(
                    ViewVouchersSold.Columns.OrderDate, Comparison.LessOrEquals, settlement[0].EndTime);
                VoucherSoldReport.Where(ViewVouchersSold.Columns.PointOfSaleID, settlement[0].PointOfSaleID);
                VoucherSoldReport.Load();

                //Voucher Redeemed Report
                VoucherRedeemedReport = new ViewVoucherRedeemedCollection();
                VoucherRedeemedReport.Where(ViewVoucherRedeemed.Columns.OrderDate, Comparison.GreaterOrEquals, settlement[0].StartTime);
                VoucherRedeemedReport.Where(ViewVoucherRedeemed.Columns.OrderDate, Comparison.LessOrEquals, settlement[0].EndTime);
                VoucherRedeemedReport.Where(ViewVoucherRedeemed.Columns.PointOfSaleID, settlement[0].PointOfSaleID);
                VoucherRedeemedReport.Load();

                Query qr = CounterCloseDet.CreateQuery();
                qr.AddWhere(CounterCloseDet.Columns.CounterCloseID, CounterCloseID);
                qr.AddWhere(CounterCloseDet.Columns.Deleted, false);
                CounterCloseDetReport = new CounterCloseDetCollection();
                CounterCloseDetReport.LoadAndCloseReader(qr.ExecuteReader());
 
            }
            else
            {
                ProductSalesReport = null;
                RefundSalesReport = null;
                VoucherSoldReport = null;
                VoucherRedeemedReport = null;
                CashRecordingReport = null;
                CounterCloseDetReport = null;
            }
        }

        public static decimal 
            FetchTotalRoundingAmount
            (DateTime startDate, DateTime endDate, int pointOfSaleID)
        {            
            Query qr = ViewTransactionDetail.CreateQuery();
            object o = qr.WHERE(ViewTransactionDetail.Columns.OrderDetDate, Comparison.GreaterOrEquals, startDate).
                AND(ViewTransactionDetail.Columns.OrderDetDate, Comparison.LessOrEquals, endDate).
                AND(ViewTransactionDetail.Columns.PointOfSaleID, pointOfSaleID).
                AND(ViewTransactionDetail.Columns.ItemNo, POSController.ROUNDING_ITEM).AND(ViewTransactionDetail.Columns.IsVoided, false).
                AND(ViewTransactionDetail.Columns.IsLineVoided, false).GetSum(ViewTransactionDetail.Columns.Amount);

            if (o is decimal)
            {
                return (decimal)o;
            }
            return 0.0M;


        }

        public static DataTable FetchProductSalesReportByPointOfSale
            (DateTime startDate, DateTime endDate, int pointOfSaleID, 
             ArrayList itemNoList, string sortBy, string sortDir)
        {
            DataTable result;

            if (itemNoList == null || itemNoList.Count == 0)
            {
                result = SPs.FetchProductSalesReportByPointOfSale(startDate, endDate, pointOfSaleID.ToString(), sortBy, sortDir).GetDataSet().Tables[0];          
            }
            else
            {
                string ItemNoListStr;
                
                //convert array list
                ItemNoListStr = "'" + itemNoList[0].ToString() + "'";
                for (int i = 1; i < itemNoList.Count; i++)
                {
                    ItemNoListStr += ",'" + itemNoList[i].ToString() + "'";
                }

                result = SPs.FetchProductSalesReportByPointOfSaleByItems 
                    (startDate, endDate, pointOfSaleID.ToString(), ItemNoListStr, sortBy, sortDir).GetDataSet().Tables[0];          
            }
            return result;
        }
    }
}
