namespace PowerPOS.SQLScript
{
    public class SQLProfitAndLoss
    {
        /// <summary>
        /// Need: 
        ///     @StartDate, @EndDate, @OutletName.
        /// Return: 
        ///     OrderDetID, OrderHdrID, OrderRefNo, OrderDate, GrossSales, DiscountSales
        ///     , NettSalesBeforeGST, GSTAmount, NettSalesAfterGST, GST, CostOfGoodsSold
        ///     , ProfitLoss, PointOfSaleName, OutletName
        /// </summary>
        public static string RawProfitAndLossScript
        {
            get
            {
                return
                    "SELECT OD.OrderDetID, OH.OrderHdrID, OrderRefNo, OrderDate " +
                        ", GrossAmount AS GrossSales, DiscountAmount AS DiscountSales, NettAmount AS NettSalesBeforeGST " +
                        ", OH.GSTAmount AS GSTAmount, NettAmount - OH.GSTAmount AS NettSalesAfterGST, OH.GST " +
                        ", ISNULL(IC.COG,0) * OD.Quantity AS CostOfGoodsSold " +
                        ", (NettAmount - OH.GSTAmount - ISNULL(IC.COG,0) * OD.Quantity) AS ProfitLoss " +
                        ", LP.PointOfSaleName, LO.OutletName " +
                    "FROM OrderHdr OH " +
                        "INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID " +
                        "INNER JOIN PointOfSale LP ON LP.PointOfSaleID = OH.PointOfSaleID " +
                        "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                        "LEFT JOIN " +
                        "( " +
                            "SELECT InventoryLocationID, ItemNo, SUM(Quantity) AS Quantity" +
                                ", CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END AS COG " +
                            "FROM InventoryHdr IH " +
                                "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                            "WHERE MovementType LIKE '% In' " +
                            "GROUP BY InventoryLocationID, ItemNo " +
                        ") IC ON LO.InventoryLocationID = IC.InventoryLocationID AND OD.ItemNo = IC.ItemNo " +
                    "WHERE OrderDate BETWEEN @startdate AND @enddate " +
                        "AND OH.IsVoided = 0 AND OD.IsVoided = 0 " +
                        "AND LO.OutletName LIKE CASE WHEN LOWER(@OutletName) = 'all' THEN '%' ELSE @OutletName END ";
            }
        }
    }
}
