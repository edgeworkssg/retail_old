using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using PowerPOS;

namespace PowerReport
{
    public partial class SalesPersonProfit
    {
        #region #) Skeleton for the Report
        public string SalesPersonID;
        public string SalesPersonName;
        public decimal SalesAmount;
        public decimal CostAmount;
        #endregion

        public static DataTable GetData(DateTime StartDate, DateTime EndDate, bool OnlyCommisionableItem, string SearchUser)
        {
            string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            StartDate = StartDate.Date;
            EndDate = EndDate.Date.AddDays(1);

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return ReportController.FetchLineCommissionByProfitReport(StartDate, EndDate);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return GetData(StartDate, EndDate, OnlyCommisionableItem, SearchUser);
            else
                return ReportController.FetchLineCommissionByProfitReport(StartDate, EndDate);
        }

        private static DataTable GetData_FixedAvg(DateTime StartDate, DateTime EndDate, bool OnlyCommisionableItem, string SearchUser)
        {
            string sqlString;
            #region #) SQL String - Get Data
            sqlString =
                "DECLARE @startDate AS DATETIME; " +
                "DECLARE @endDate AS DATETIME; " +
                "DECLARE @searchString AS NVARCHAR(MAX); " +
                "SET @startDate = '" + StartDate.ToString("dd MMM yyyy") + "'; " +
                "SET @endDate = '" + EndDate.ToString("dd MMM yyyy") + "'; " +
                "SET @searchString = '" + SearchUser + "'; " +
                "SELECT UM.UserName AS SalesPersonID " +
                    ", UM.DisplayName AS SalesPersonName " +
                    ", SUM(OD.Amount) AS SalesAmount " +
                    ", SUM(Quantity * ISNULL(XA.CostOfGoods, 0)) AS CostAmount " +
                "FROM OrderHdr OH " +
                    "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
                    (OnlyCommisionableItem ? "INNER JOIN Item IT ON OD.ItemNo = IT.ItemNo " : "") +
                    "INNER JOIN PointOfSale LP ON OH.PointOfSaleID = LP.PointOfSaleID " +
                    "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                    "LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID " +
                    "LEFT OUTER JOIN " +
                    "( " +
                        "SELECT ItemNo, InventoryLocationID, CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END AS CostOfGoods " +
                        "FROM InventoryHdr IH " +
                            "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                        "WHERE MovementType LIKE '% In' " +
                        "GROUP BY ItemNo, InventoryLocationID " +
                    ") XA ON OD.ItemNo = XA.ItemNo AND LO.InventoryLocationID = XA.InventoryLocationID " +
                    "RIGHT OUTER JOIN UserMst UM ON ISNULL(NULLIF(OD.UserFld1,''), SCR.SalesPersonID) = UM.UserName " +
                "WHERE OH.IsVoided = 0 AND OD.IsVoided = 0 " +
                    "AND OrderDate BETWEEN @startDate AND @endDate " +
                    (OnlyCommisionableItem ? "AND IT.IsCommission = 1 " : "") +
                "GROUP BY UM.UserName, UM.DisplayName ";
            #endregion

            DataTable Output = new DataTable();
            Output.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sqlString)));

            return Output;
        }

        public static ReportDocument GetWindowsReport(string ReportName, DateTime StartDate, DateTime EndDate, bool OnlyCommisionableItem, string SearchUser)
        {
            string ReceiptFileLocation = Application.StartupPath + "\\Reports\\ProductSales\\" + ReportName;

            return GetReport(ReceiptFileLocation, StartDate, EndDate, OnlyCommisionableItem, SearchUser);
        }

        public static ReportDocument GetReport(string ReportPath, DateTime StartDate, DateTime EndDate, bool OnlyCommisionableItem, string SearchUser)
        {
            ReportDocument Output = new ReportDocument();

            string ReceiptFileLocation = ReportPath;

            bool ReportLoaded = false;
            if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
            {
                try
                {
                    Output.Load(ReceiptFileLocation);
                    Output.SetDataSource(GetData(StartDate, EndDate, OnlyCommisionableItem, SearchUser));
                    ReportLoaded = true;
                }
                catch (Exception X)
                {
                    CommonUILib.HandleException(X);
                }
            }

            if (!ReportLoaded)
                return new ReportDocument();
            else
                return Output;
        }
    }
}
