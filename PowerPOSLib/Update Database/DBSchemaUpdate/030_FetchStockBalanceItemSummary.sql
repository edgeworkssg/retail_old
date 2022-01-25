IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockBalanceItemSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchStockBalanceItemSummary]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockBalanceItemSummary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE FetchStockBalanceItemSummary
	@Search NVARCHAR(MAX),
	@InventoryLocationID INT
AS
BEGIN
	SET NOCOUNT ON;

	declare @a table ( 
		ItemNo varchar(50) 
		,ItemName nvarchar(300)
		,Attributes1 nvarchar(max)
		,Attributes2 nvarchar(max)
		,Attributes3 nvarchar(max)
		,Attributes4 nvarchar(max)
		,Attributes5 nvarchar(max)
		,Attributes6 nvarchar(max)
		,Attributes7 nvarchar(max)
		,Attributes8 nvarchar(max)
		,AvgCostPrice money
		,BalanceQuantity float
		,PreOrderQty int)

	INSERT INTO @a (ItemNo,ItemName ,Attributes1,Attributes2,Attributes3,Attributes4,Attributes5
		,Attributes6, Attributes7,Attributes8,AvgCostPrice,BalanceQuantity, PreOrderQty)
	SELECT i.ItemNo,i.ItemName ,i.Attributes1,i.Attributes2,i.Attributes3,i.Attributes4,i.Attributes5
		,i.Attributes6, i.Attributes7,i.Attributes8,i.AvgCostPrice,i.BalanceQuantity,  dbo.GetItemTotalPreOrderQty(i.ItemNo, @InventoryLocationID)
	FROM Item i LEFT OUTER JOIN AlternateBarcode AB ON I.ItemNo = AB.ItemNo
	WHERE	I.IsInInventory = 1  AND ISNULL(I.Deleted,0) = 0
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


	SELECT ItemNo, ItemName, CAST(MAX(COG) as Numeric(18,2)) COG, CAST(MAX(Quantity) AS Numeric(18,2)) Quantity
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
		select it.ItemNo
				,it.ItemName
				,ISNULL(it.AvgCostPrice,0) COG
				,CASE WHEN @InventoryLocationID = 0 THEN ISNULL(it.BalanceQuantity,0) ELSE ISNULL(ISM.BalanceQty,0) END Quantity
				,it.Attributes1
				,it.Attributes2
				,it.Attributes3
				,it.Attributes4
				,it.Attributes5
				,it.Attributes6
				,it.Attributes7
				,it.Attributes8
				,0 StockTakeID
				,PreOrderQty 
				,CASE WHEN @InventoryLocationID = 0 
					THEN it.BalanceQuantity - PreOrderQty 
					ELSE ISNULL(ISM.BalanceQty,0) - PreOrderQty 
					END PreOrderBalance
		From @a it
		LEFT OUTER JOIN ItemSummary ISM ON ISM.ItemNo = it.ItemNo AND (ISM.InventoryLocationID = @InventoryLocationID OR @InventoryLocationID = 0)
	)a
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
