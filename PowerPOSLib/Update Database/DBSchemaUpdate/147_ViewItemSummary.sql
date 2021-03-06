IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewItemSummary]'))
DROP VIEW [dbo].[ViewItemSummary]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewItemSummary]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[ViewItemSummary]
AS
SELECT	 I.ItemNo
		,I.ItemName
		,I.CategoryName
		,I.InventoryLocationID
		,I.InventoryLocationName
		,I.InventoryLocationGroupID
		,I.InventoryLocationGroupName
		,I.ItemBalanceQty
		,I.ItemCostPrice
		,ISNULL(ISM.BalanceQty,0) LocBalanceQty
		,ISNULL(ISM.CostPrice,0) LocCostPrice
		,ISNULL(INVGRP.BalanceQty,0) LocGroupBalanceQty
		,ISNULL(INVGRP.CostPrice,0) LocGroupCostPrice
FROM (
	SELECT   I.ItemNo
			,I.ItemName
			,I.CategoryName
			,ISNULL(I.AvgCostPrice,0) ItemCostPrice
			,ISNULL(I.BalanceQuantity,0) ItemBalanceQty
			,IL.InventoryLocationID
			,IL.InventoryLocationName
			,ISNULL(ILG.InventoryLocationGroupID,0) InventoryLocationGroupID
			,ISNULL(ILG.InventoryLocationGroupName,''-'') InventoryLocationGroupName
	FROM	InventoryLocation IL
			INNER JOIN InventoryLocationGroup ILG ON ILG.InventoryLocationGroupID = ISNULL(IL.Userint1,0)
			CROSS JOIN Item I 
	WHERE	ISNULL(IL.Deleted,0) = 0
) I LEFT JOIN ItemSummary ISM ON ISM.ItemNo = I.ItemNo AND ISM.InventoryLocationID = I.InventoryLocationID
    LEFT JOIN ItemSummaryGroup INVGRP ON INVGRP.InventoryLocationGroupID = I.InventoryLocationGroupID AND INVGRP.ItemNo = I.ItemNo
'