declare @statement Nvarchar(max)
set @statement = ''
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewStockTake]'))
BEGIN
 set @statement = N'
ALTER VIEW [dbo].[ViewStockTake]
AS
SELECT     dbo.StockTake.StockTakeDate, dbo.StockTake.ItemNo, dbo.StockTake.StockTakeID, dbo.StockTake.InventoryLocationID, dbo.StockTake.StockTakeQty, 
                      dbo.StockTake.TakenBy, dbo.StockTake.VerifiedBy, dbo.StockTake.AuthorizedBy, dbo.StockTake.IsAdjusted, dbo.StockTake.Remark, dbo.StockTake.CostOfGoods, 
                      dbo.Item.ItemName, dbo.Item.CategoryName, dbo.Item.IsInInventory, dbo.InventoryLocation.InventoryLocationName, dbo.StockTake.AdjustmentHdrRefNo, 
                      dbo.StockTake.BalQtyAtEntry, dbo.StockTake.AdjustmentQty, dbo.StockTake.Marked, dbo.StockTake.userfld1 as[BatchNo], dbo.StockTake.userflag1 as Deleted 
FROM         dbo.Item INNER JOIN
                      dbo.StockTake ON dbo.Item.ItemNo = dbo.StockTake.ItemNo INNER JOIN
                      dbo.InventoryLocation ON dbo.StockTake.InventoryLocationID = dbo.InventoryLocation.InventoryLocationID'
   
EXEC dbo.sp_executesql @statement                      
END