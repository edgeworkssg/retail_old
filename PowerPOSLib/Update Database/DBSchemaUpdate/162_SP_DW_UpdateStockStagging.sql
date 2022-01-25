IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DW_UpdateStockStaging]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DW_UpdateStockStaging]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DW_UpdateStockStaging]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE DW_UpdateStockStaging
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@InventoryLocationID int
AS
BEGIN
	Delete from StockStaging where InventoryDate = @EndDate and InventoryLocationID = @InventoryLocationID   

	INSERT INTO StockStaging (InventoryDate, ItemNo, InventoryLocationID, BalanceQty, CostPriceByItem, CostPriceByItemInvLoc, CostPriceByItemInvGroup)
	SELECT  @EndDate InventoryDate
			,ID.ItemNo
			,IH.InventoryLocationID
			,SUM(CASE WHEN IH.MovementType like ''% In'' Then ID.Quantity ELSE -1 * ID.Quantity END )  Quantity
			,MAX(TAB.CostPriceByItem) as CostPriceByItem
			,MAX(TAB.CostPriceByItemInvLoc) as CostPriceByItemInvLoc
			,MAX(TAB.CostPriceByItemInvGroup) as CostPriceByItemInvGroup
	FROM	InventoryHdr IH
			LEFT JOIN InventoryDet ID ON ID.InventoryHdrRefNo = IH.InventoryHdrRefNo
			LEFT JOIN (
					SELECT   ROW_NUMBER() OVER(PARTITION BY ID.ItemNo, IH.InventoryLocationID ORDER BY IH.InventoryDate DESC) NoRow
							,IH.InventoryHdrRefNo
							,IH.InventoryDate
							,IH.InventoryLocationID
							,ID.ItemNo
							,ID.CostPriceByItem
							,ID.CostPriceByItemInvLoc
							,ID.CostPriceByItemInvGroup
					FROM	InventoryHdr IH
							LEFT JOIN InventoryDet ID ON ID.InventoryHdrRefNo = IH.InventoryHdrRefNo
					WHERE	IH.InventoryDate between @StartDate and @EndDate
			) TAB ON TAB.NoRow = 1
					 AND TAB.ItemNo = ID.ItemNo
					 AND TAB.InventoryLocationID = IH.InventoryLocationID
	WHERE	IH.InventoryDate between @StartDate and @EndDate 
			AND (@InventoryLocationID = 0 OR IH.InventoryLocationID = @InventoryLocationID)
	GROUP BY IH.InventoryLocationID
			,ID.ItemNo
END
'
END
