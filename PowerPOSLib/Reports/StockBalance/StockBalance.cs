using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using PowerPOS;

namespace PowerReport
{
    public partial class StockBalance
    {
        #region #) Skeleton for the Report
        public int ItemDepartmentID;
        public string DepartmentName;
        public string CategoryName;
        public string ItemNo;
        public string ItemName;
        public string Barcode;
        public decimal RetailPrice;
        public decimal FactoryPrice;
        public string ItemDesc;
        public bool IsServiceItem;
        public bool IsInInventory;
        public bool IsNonDiscountable;
        public string ProductLine;
        public string Attributes1;
        public string Attributes2;
        public string Attributes3;
        public string Attributes4;
        public string Attributes5;
        public string Attributes6;
        public string Attributes7;
        public string Attributes8;
        public string Remark;
        public bool Deleted;
        public string userfld1;
        public string userfld2;
        public string userfld3;
        public string userfld4;
        public string userfld5;
        public string userfld6;
        public string userfld7;
        public string userfld8;
        public string userfld9;
        public string userfld10;
        public bool userflag1;
        public bool userflag2;
        public bool userflag3;
        public bool userflag4;
        public bool userflag5;
        public decimal userfloat1;
        public decimal userfloat2;
        public decimal userfloat3;
        public decimal userfloat4;
        public decimal userfloat5;
        public int userint1;
        public int userint2;
        public int userint3;
        public int userint4;
        public int userint5;
        public bool IsDelivery;
        public int GSTRule;
        public bool IsCommission;

        public string Att1Name;
        public string Att2Name;
        public string Att3Name;
        public string Att4Name;
        public string Att5Name;
        public string Att6Name;
        public string Att7Name;
        public string Att8Name;

        public int InventoryLocationID;
        public string InventoryLocationName;

        public int StockOnHand;
        public decimal AvgUnitCost;
        public decimal TotalCost;
        #endregion

        public static DataTable GetData(int InventoryLocation, bool IncludeDeletedItem, string SearchQuery)
        {
            string sqlString;
            #region #) SQL String - With Avg Cost [KIV]
            sqlString =
               "DECLARE @Search AS NVARCHAR(MAX); " +
               "DECLARE @InventoryLocationID AS INT; " +
               "SET @Search = '" + SearchQuery + "' " +
               "SET @InventoryLocationID = " + InventoryLocation.ToString() + " " +

               "DECLARE @CurrentBalance TABLE(InventoryLocationID INT, ItemNo VARCHAR(50), Quantity INT) " +
               "INSERT INTO @CurrentBalance " +
               "SELECT InventoryLocationID, ItemNo, SUM(CASE WHEN MovementType LIKE '% In' THEN Quantity ELSE 0 - Quantity END) AS Quantity " +
               "FROM InventoryHdr a " +
                   "INNER JOIN InventoryDet b ON a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
               "GROUP BY InventoryLocationID, ItemNo " +

               "DECLARE @UndeductedItems TABLE(InventoryLocationID INT, ItemNo VARCHAR(50), UndeductedQty INT) " +
               "INSERT INTO @UndeductedItems " +
               "SELECT InventoryLocationID, c.ItemNo, SUM(Quantity) AS UndeductedQty " +
               "FROM OrderHdr a " +
                   "INNER JOIN OrderDet b ON a.OrderHdrID = b.OrderHdrID " +
                   "INNER JOIN Item c ON b.ItemNo = c.ItemNo " +
                   "INNER JOIN PointOfSale d ON a.PointOfSaleID = d.PointOfSaleID " +
                   "INNER JOIN Outlet e ON d.OutletName = e.OutletName " +
               "WHERE (b.InventoryHdrRefNo = '' OR b.InventoryHdrRefNo IS NULL) " +
                   "AND a.IsVoided = 0 AND b.IsVoided = 0 " +
                   "AND IsInInventory = 1 " +
                   "AND CategoryName <> 'SYSTEM' " +
               "GROUP BY InventoryLocationID, c.ItemNo " +

               "DECLARE @StockInHistory TABLE(InventoryLocationID INT, InventoryDate DATETIME, ItemNo VARCHAR(50),RemainingQty INT,CostOfGoods DECIMAL(18,4),TotalRemainingQty INT) " +
               "INSERT INTO @StockInHistory " +
               "SELECT x.InventoryLocationID, x.InventoryDate,x.ItemNo,x.RemainingQty,x.CostOfGoods,SUM(y.RemainingQty) AS TotalRemainingQty FROM " +
               "( " +
                   "SELECT InventoryLocationID, InventoryDate, ItemNo, Quantity RemainingQty, CostOfGoods " +
                   "FROM InventoryDet a " +
                       "INNER JOIN InventoryHdr b ON a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
                   "WHERE MovementType LIKE '% In' " +
               ") x INNER JOIN ( " +
                   "SELECT InventoryLocationID, InventoryDate, ItemNo, Quantity RemainingQty " +
                   "FROM InventoryDet a " +
                       "INNER JOIN InventoryHdr b ON a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
                   "WHERE MovementType LIKE '% In' " +
               ") y ON x.InventoryLocationID = y.InventoryLocationID " +
                    "AND x.ItemNo = y.ItemNo AND x.InventoryDate <= y.InventoryDate " +
               "GROUP BY x.InventoryLocationID, x.InventoryDate,x.ItemNo,x.RemainingQty,x.CostOfGoods " +
               "ORDER BY x.InventoryLocationID, x.ItemNo,x.InventoryDate DESC " +

               "DECLARE @FinalProduct TABLE(InventoryLocationID INT, ItemNo VARCHAR(50), TotalQty INT, AvgCOGS DECIMAL(18,4)) " +
               "INSERT INTO @FinalProduct " +
               "SELECT X.InventoryLocationID, ItemNo,SUM(StockBalance) AS TotalQty, SUM(StockBalance * CostOfGoods) / SUM(StockBalance) AS AvgCOGS " +
               "FROM " +
               "( " +
                   "SELECT A.InventoryLocationID, InventoryDate,A.ItemNo,CostOfGoods " +
                       ", CASE " +
                           "WHEN (TotalRemainingQty - RemainingQty) > (ISNULL(Quantity,0) - ISNULL(UndeductedQty,0) ) THEN 0 " +
                           "WHEN TotalRemainingQty > (ISNULL(Quantity,0) - ISNULL(UndeductedQty,0)) THEN RemainingQty - (TotalRemainingQty - (ISNULL(Quantity,0) - ISNULL(UndeductedQty,0))) " +
                           "ELSE RemainingQty END AS StockBalance " +
                   "FROM @StockInHistory A " +
                       "LEFT OUTER JOIN @CurrentBalance B ON A.ItemNo = B.ItemNo " +
                       "LEFT OUTER JOIN @UndeductedItems C ON A.ItemNo = C.ItemNo " +
               ") X " +
               "GROUP BY X.InventoryLocationID, ItemNo " +
               "HAVING SUM(StockBalance) <> 0 " +

               "SELECT * FROM ( " +
                   "SELECT ISNULL([1],'') AS Att1Name, ISNULL([2],'') AS Att2Name, ISNULL([3],'') AS Att3Name, ISNULL([4],'') AS Att4Name " +
                       ", ISNULL([5],'') AS Att5Name, ISNULL([6],'') AS Att6Name, ISNULL([7],'') AS Att7Name, ISNULL([8],'') AS Att8Name " +
                   "FROM AttributesLabel " +
                   "PIVOT " +
                   "( " +
                       "MAX(Label) " +
                       "FOR AttributesNo IN " +
                       "([1],[2],[3],[4],[5],[6],[7],[8]) " +
                   ") AS PVT " +
               ") A, (" +
                   "SELECT D.ItemDepartmentId, DepartmentName, A.*, B.InventoryLocationID " +
                       ", B.InventoryLocationName, ISNULL(E.TotalQty,0) AS StockOnHand, ISNULL(E.AvgCOGS,0) AS AvgUnitCost, ISNULL(E.TotalQty * E.AvgCOGS,0) AS TotalCost " +
                   "FROM Item A " +
	                   "CROSS JOIN InventoryLocation B " +
	                   "INNER JOIN Category C ON A.CategoryName = C.CategoryName " +
	                   "INNER JOIN ItemDepartment D ON C.ItemDepartmentId = D.ItemDepartmentID " +
	                   "LEFT OUTER JOIN @FinalProduct E ON A.ItemNo = E.ItemNo AND B.InventoryLocationID = E.InventoryLocationID " +
                   "WHERE C.CategoryName <> 'SYSTEM'  " +
	                   "AND IsInInventory = 1  " +
                       "AND DepartmentName + '|' + C.CategoryName + '|' + A.ItemNo + '|'  " +
		                   "+ ItemName + '|' + Barcode + '|' + ISNULL(Attributes1,'') + '|'  " +
		                   "+ ISNULL(Attributes2,'') + '|' + ISNULL(Attributes3,'') + '|'  " +
		                   "+ ISNULL(Attributes4,'') + '|' + ISNULL(Attributes5,'') + '|'  " +
		                   "+ ISNULL(Attributes6,'') + '|' + ISNULL(Attributes7,'') + '|'  " +
		                   "+ ISNULL(Attributes8,'') LIKE @Search   " +
                       "#AddSearch " +
               ") B ";
            #endregion
            #region #) SQL String
            sqlString =
                "DECLARE @Search AS NVARCHAR(MAX); " +
                "DECLARE @InventoryLocationID AS VARCHAR(10); " +
                "SET @Search = '%" + SearchQuery + "%' " +
                "SET @InventoryLocationID = " + InventoryLocation.ToString() + " " +
                " " +
                "SELECT M.* " +
                    ",ISNULL(N.Quantity,0) - ISNULL(O.UndeductedQty,0) AS StockOnHand " +
                "FROM " +
                "( " +
                    "SELECT Q.*, R.*, @InventoryLocationID AS InventoryLocationID, CASE WHEN @InventoryLocationID = 0 THEN 'All Location' ELSE T.InventoryLocationName END AS InventoryLocationName " +
                    "FROM "+
                        "("+
                            "SELECT Z.DepartmentName, Z.ItemDepartmentID, X.* "+
                            "FROM Item X "+
                                "INNER JOIN Category Y ON X.CategoryName = Y.Categoryname "+
                                "INNER JOIN ItemDepartment Z ON Y.ItemDepartmentID = Z.ItemDepartmentID "+
                            "WHERE Z.ItemDepartmentID <> 'SYSTEM' AND IsInInventory = 1 " +
                        ") Q, ("+
                            "SELECT ISNULL([1],'') Att1Name, ISNULL([2],'') Att2Name, ISNULL([3],'') Att3Name, ISNULL([4],'') Att4Name "+
                                ", ISNULL([5],'') Att5Name, ISNULL([6],'') Att6Name, ISNULL([7],'') Att7Name, ISNULL([8],'') Att8Name "+
                            "FROM AttributesLabel "+
                            "PIVOT "+
                            "("+
	                            "MAX(Label) "+
	                            "FOR AttributesNo IN "+
                                "([1],[2],[3],[4],[5],[6],[7],[8]) " +
                            ") AS Pvt "+
                        ") R, ( "+
	                        "SELECT TOP 1 InventoryLocationName FROM InventoryLocation WHERE InventoryLocationID LIKE CASE WHEN @InventoryLocationID = 0 THEN '%' ELSE @InventoryLocationID END "+
                        ") T " +
                ") M " +
                "LEFT OUTER JOIN " +
                "( " +
                   "SELECT ItemNo, SUM(CASE WHEN MovementType LIKE '% In' THEN Quantity ELSE 0 - Quantity END) AS Quantity " +
                   "FROM InventoryHdr a " +
                       "INNER JOIN InventoryDet b ON a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
                   "GROUP BY ItemNo " +
                ") N ON M.ItemNo = N.ItemNo " +
                "LEFT OUTER JOIN " +
                "( " +
                    "SELECT c.ItemNo, SUM(Quantity) AS UndeductedQty " +
                    "FROM OrderHdr a " +
                       "INNER JOIN OrderDet b ON a.OrderHdrID = b.OrderHdrID " +
                       "INNER JOIN Item c ON b.ItemNo = c.ItemNo " +
                    "WHERE (b.InventoryHdrRefNo = '' OR b.InventoryHdrRefNo IS NULL) " +
                       "AND a.IsVoided = 0 AND b.IsVoided = 0 " +
                       "AND IsInInventory = 1 " +
                       "AND CategoryName <> 'SYSTEM' " +
                    "GROUP BY c.ItemNo " +
                ") O ON M.ItemNo = O.ItemNo " +
                "WHERE DepartmentName + '|' + CategoryName + '|' + M.ItemNo + '|'  " +
                        "+ ItemName + '|' + Barcode + '|' + ISNULL(Attributes1,'') + '|'  " +
                        "+ ISNULL(Attributes2,'') + '|' + ISNULL(Attributes3,'') + '|'  " +
                        "+ ISNULL(Attributes4,'') + '|' + ISNULL(Attributes5,'') + '|'  " +
                        "+ ISNULL(Attributes6,'') + '|' + ISNULL(Attributes7,'') + '|'  " +
                        "+ ISNULL(Attributes8,'') LIKE @Search " +
                    "#AddSearch ";
            #endregion
            #region #) SQL String - Testing
            sqlString =
                "DECLARE @Search AS NVARCHAR(MAX); " +
                "DECLARE @InventoryLocationID AS VARCHAR(10); " +
                "SET @Search = '%" + SearchQuery + "%' " +
                "SET @InventoryLocationID = " + InventoryLocation.ToString() + " " +
                " " +
                "SELECT M.* " +
                    ",ISNULL(N.Quantity,0) - ISNULL(O.UndeductedQty,0) AS StockOnHand " +
                "FROM " +
                "( " +
                    "SELECT Q.*, R.*, @InventoryLocationID AS InventoryLocationID, CASE WHEN @InventoryLocationID = 0 THEN 'All Location' ELSE T.InventoryLocationName END AS InventoryLocationName " +
                    "FROM " +
                        "(" +
                            "SELECT Z.DepartmentName, Z.ItemDepartmentID, X.* " +
                            "FROM Item X " +
                                "INNER JOIN Category Y ON X.CategoryName = Y.Categoryname " +
                                "INNER JOIN ItemDepartment Z ON Y.ItemDepartmentID = Z.ItemDepartmentID " +
                            "WHERE Z.ItemDepartmentID <> 'SYSTEM' AND IsInInventory = 1 " +
                        ") Q, (" +
                            "SELECT ISNULL([1],'') Att1Name, ISNULL([2],'') Att2Name, ISNULL([3],'') Att3Name, ISNULL([4],'') Att4Name " +
                                ", ISNULL([5],'') Att5Name, ISNULL([6],'') Att6Name, ISNULL([7],'') Att7Name, ISNULL([8],'') Att8Name " +
                            "FROM AttributesLabel " +
                            "PIVOT " +
                            "(" +
                                "MAX(Label) " +
                                "FOR AttributesNo IN " +
                                "([1],[2],[3],[4],[5],[6],[7],[8]) " +
                            ") AS Pvt " +
                        ") R, ( " +
                            "SELECT TOP 1 InventoryLocationName FROM InventoryLocation WHERE InventoryLocationID LIKE CASE WHEN @InventoryLocationID = 0 THEN '%' ELSE @InventoryLocationID END " +
                        ") T " +
                ") M " +
                "LEFT OUTER JOIN " +
                "( " +
                   "SELECT ItemNo, SUM(CASE WHEN MovementType LIKE '% In' THEN Quantity ELSE 0 - Quantity END) AS Quantity " +
                   "FROM InventoryHdr a " +
                       "INNER JOIN InventoryDet b ON a.InventoryHdrRefNo = b.InventoryHdrRefNo " +
                   "GROUP BY ItemNo " +
                ") N ON M.ItemNo = N.ItemNo " +
                "LEFT OUTER JOIN " +
                "( " +
                    "SELECT c.ItemNo, SUM(Quantity) AS UndeductedQty " +
                    "FROM OrderHdr a " +
                       "INNER JOIN OrderDet b ON a.OrderHdrID = b.OrderHdrID " +
                       "INNER JOIN Item c ON b.ItemNo = c.ItemNo " +
                    "WHERE (b.InventoryHdrRefNo = '' OR b.InventoryHdrRefNo IS NULL) " +
                       "AND a.IsVoided = 0 AND b.IsVoided = 0 " +
                       "AND IsInInventory = 1 " +
                       "AND CategoryName <> 'SYSTEM' " +
                    "GROUP BY c.ItemNo " +
                ") O ON M.ItemNo = O.ItemNo " +
                "WHERE DepartmentName + '|' + CategoryName + '|' + M.ItemNo + '|'  " +
                        "+ ItemName + '|' + Barcode + '|' + ISNULL(Attributes1,'') + '|'  " +
                        "+ ISNULL(Attributes2,'') + '|' + ISNULL(Attributes3,'') + '|'  " +
                        "+ ISNULL(Attributes4,'') + '|' + ISNULL(Attributes5,'') + '|'  " +
                        "+ ISNULL(Attributes6,'') + '|' + ISNULL(Attributes7,'') + '|'  " +
                        "+ ISNULL(Attributes8,'') LIKE @Search " +
                    "#AddSearch ";
            #endregion

            string AddSearch = "";
            #region *) Set search parameter
            //if (InventoryLocation > 0) AddSearch += "AND M.InventoryLocationID LIKE CASE WHEN @InventoryLocationID = 0 THEN '%' ELSE @InventoryLocationID END ";
            if (!IncludeDeletedItem) AddSearch += "AND M.Deleted = 1 ";

            sqlString = sqlString.Replace("#AddSearch", AddSearch);
            #endregion

            DataTable Output = new DataTable();
            Output.Load(SubSonic.DataService.GetReader(new SubSonic.QueryCommand(sqlString)));

            return Output;
        }

        public static ReportDocument GetWindowsReport(string ReportName, int InventoryLocation, bool IncludeDeletedItem, string SearchQuery)
        {
            string ReceiptFileLocation = Application.StartupPath + "\\Reports\\StockBalance\\" + ReportName;

            return GetReport(ReceiptFileLocation, InventoryLocation, IncludeDeletedItem, SearchQuery);
        }

        public static ReportDocument GetReport(string ReportPath, int InventoryLocation, bool IncludeDeletedItem, string SearchQuery)
        {
            ReportDocument Output = new ReportDocument();
            
            string ReceiptFileLocation = ReportPath;

            bool ReportLoaded = false;
            if (ReceiptFileLocation != null && ReceiptFileLocation.ToLower().EndsWith(".rpt") && File.Exists(ReceiptFileLocation))
            {
                try
                {
                    Output.Load(ReceiptFileLocation);
                    Output.SetDataSource(GetData(InventoryLocation, IncludeDeletedItem, SearchQuery));
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
