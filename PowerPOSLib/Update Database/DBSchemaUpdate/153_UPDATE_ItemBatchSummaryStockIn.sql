IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_ItemBatchSummaryStockIn]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_ItemBatchSummaryStockIn]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_ItemBatchSummaryStockIn]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_ItemBatchSummaryStockIn]
	 @InventoryDetID AS VARCHAR(50), 
	 @ItemNo AS VARCHAR(50),
	 @InventoryLocationID INT,
	 @InventoryDate DATETIME,
	 @Qty AS FLOAT,
	 @CostPrice AS FLOAT,
	 @BatchNo VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @InitialQty FLOAT;
	DECLARE @RemainingQty FLOAT;
	DECLARE @ItemBatchSummaryID VARCHAR(50);
	
	SET @ItemBatchSummaryID = @ItemNo + ''-'' + CAST(@InventoryLocationID AS VARCHAR(50)) + ''-''+
							+ CONVERT(VARCHAR(8), GETDATE(), 12)
							+ RIGHT(''00''+CAST(DATEPART(HOUR, GETDATE()) AS VARCHAR(2)), 2)
							+ RIGHT(''00''+CAST(DATEPART(MINUTE, GETDATE()) AS VARCHAR(2)), 2)
							+ RIGHT(''00''+CAST(DATEPART(SECOND, GETDATE()) AS VARCHAR(2)), 2)
							+ RIGHT(''00''+CAST(DATEPART(MILLISECOND, GETDATE()) AS VARCHAR(3)),2);

	SELECT  @InitialQty = (BalanceQty - @Qty)
	FROM    ItemSummary
	WHERE	ISNULL(Deleted,0) = 0
			AND ItemNo = @ItemNo
			AND InventoryLocationID = @InventoryLocationID

	SET @RemainingQty = @Qty;
	IF ISNULL(@InitialQty,0) < 0 BEGIN
		SET @RemainingQty = @Qty + ISNULL(@InitialQty,0)
	END 

	IF ISNULL(@RemainingQty,0) > 0 BEGIN
		INSERT INTO ItemBatchSummary
		(
			 ItemBatchSummaryID
			,ItemNo
			,InventoryLocationID
			,ReceivingDate
			,Qty
			,RemainingQty
			,CostPrice
			,BatchNo
			,InventoryDetRefNo

			,UniqueID
			,Deleted
			,CreatedBy
			,CreatedOn
			,ModifiedBy
			,ModifiedOn
		)
		VALUES
		(
			 @ItemBatchSummaryID
			,@ItemNo
			,@InventoryLocationID
			,@InventoryDate
			,@Qty
			,@RemainingQty
			,@CostPrice
			,@BatchNo
			,@InventoryDetID

			,NEWID()
			,0
			,''SYNC''
			,GETDATE()
			,''SYNC''
			,GETDATE()
		)
	END
	ELSE BEGIN
		INSERT INTO ItemBatchSummaryArchive
		(
			 ItemBatchSummaryID
			,ItemNo
			,InventoryLocationID
			,ReceivingDate
			,Qty
			,RemainingQty
			,CostPrice
			,BatchNo
			,InventoryDetRefNo
			,UniqueID
			,Deleted
			,CreatedBy
			,CreatedOn
			,ModifiedBy
			,ModifiedOn
		)
		VALUES
		(
			 @ItemBatchSummaryID
			,@ItemNo
			,@InventoryLocationID
			,@InventoryDate
			,@Qty
			,CASE WHEN @RemainingQty < 0 THEN 0 ELSE @RemainingQty END
			,@CostPrice
			,@BatchNo
			,@InventoryDetID
			,NEWID()
			,0
			,''SYNC''
			,GETDATE()
			,''SYNC''
			,GETDATE()
		)
	END		
END
' 
 
END