using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.SQLScript
{
    public class SQLInventory
    {
        public static string GetCostOfGoods_FixedAvg =
            "SELECT ItemNo, InventoryLocationID, CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END AS CostOfGoods " +
            "FROM InventoryHdr IH " +
                "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
            "WHERE MovementType LIKE '% In' " +
            "GROUP BY ItemNo, InventoryLocationID ";
    }
}
