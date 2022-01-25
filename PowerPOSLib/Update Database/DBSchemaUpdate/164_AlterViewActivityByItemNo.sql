ALTER VIEW [dbo].[ViewInventoryActivityByItemNo]
AS
SELECT     InventoryHdrRefNo, InventoryDate, MovementType, InventoryLocationID, ItemNo, SUM(Quantity) AS Quantity, AVG(CostOfGoods) AS CostOfGoods, AVG(FactoryPrice) as FactoryPrice,
                      StockOutReasonID
FROM         dbo.ViewInventoryActivity
GROUP BY InventoryHdrRefNo, ItemNo, InventoryDate, MovementType, InventoryLocationID, StockOutReasonID



