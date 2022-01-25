IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockBalanceItemSummaryByCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchStockBalanceItemSummaryByCategory]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockBalanceItemSummaryByCategory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchStockBalanceItemSummaryByCategory]
	@Search NVARCHAR(MAX),
	@InventoryLocationID INT,
    @CategoryName NVARCHAR(250)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ItemNo, ItemName, CAST(MAX(COG) as Numeric(18,2)) COG, CAST(MAX(Quantity) AS Numeric(18,4)) Quantity
			,Attributes1
			,Attributes2
			,Attributes3
			,Attributes4
			,Attributes5
			,Attributes6
			,Attributes7
			,Attributes8
			,StockTakeID
			,MAX(PreOrderQty) AS PreOrderQty
			,MAX(PreOrderBalance) AS PreOrderBalance
			FROM 
	(
	SELECT   I.ItemNo
			,I.ItemName
			,ISNULL(I.AvgCostPrice,0) COG
			,CASE WHEN @InventoryLocationID = 0 THEN ISNULL(I.BalanceQuantity,0) ELSE ISNULL(ISM.BalanceQty,0) END Quantity
			,I.Attributes1
			,I.Attributes2
			,I.Attributes3
			,I.Attributes4
			,I.Attributes5
			,I.Attributes6
			,I.Attributes7
			,I.Attributes8
			,0 StockTakeID
			,dbo.GetItemTotalPreOrderQty(I.ItemNo, @InventoryLocationID) PreOrderQty
			,CASE WHEN @InventoryLocationID = 0 
				THEN I.BalanceQuantity - dbo.GetItemTotalPreOrderQty(I.ItemNo, @InventoryLocationID) 
				ELSE ISNULL(ISM.BalanceQty,0) - dbo.GetItemTotalPreOrderQty(I.ItemNo, @InventoryLocationID) 
			 END PreOrderBalance
	FROM	Item I
			LEFT OUTER JOIN ItemSummary ISM ON ISM.ItemNo = I.ItemNo 
											AND (ISM.InventoryLocationID = @InventoryLocationID 
												 OR @InventoryLocationID = 0)
			LEFT OUTER JOIN AlternateBarcode AB ON I.ItemNo = AB.ItemNo 
	WHERE	I.IsInInventory = 1  AND ISNULL(I.Deleted,0) = 0
            AND (@CategoryName = ''ALL'' OR I.CategoryName = @CategoryName) 
			--AND I.IsServiceItem = 0
			AND (I.ItemNo LIKE ''%''+@Search+''%''
				OR ISNULL(I.ItemName,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Barcode,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes1,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes2,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes3,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes4,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes5,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes6,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes7,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes8,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(AB.Barcode,'''') LIKE ''%''+@Search+''%'')
	) a
	GROUP BY ItemNo
			, ItemName
			,Attributes1
			,Attributes2
			,Attributes3
			,Attributes4
			,Attributes5
			,Attributes6
			,Attributes7
			,Attributes8
			,StockTakeID
	ORDER BY ItemName
END

' 
END
