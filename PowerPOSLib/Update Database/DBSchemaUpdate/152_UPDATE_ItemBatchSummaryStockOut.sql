IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_ItemBatchSummaryStockOut]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_ItemBatchSummaryStockOut]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_ItemBatchSummaryStockOut]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_ItemBatchSummaryStockOut]
	 @InventoryDetID AS VARCHAR(50),
	 @CostingMode AS VARCHAR(50),
	 @MovementType AS VARCHAR(50),	 
	 @ItemNo AS VARCHAR(50),
	 @InventoryLocationID INT,
	 @InventoryDate DATETIME,
	 @Qty AS FLOAT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ItemBatchSummaryID VARCHAR(100);
	DECLARE @DeductionQty AS FLOAT;
	DECLARE @DeductedQty FLOAT = 0;
	DECLARE @TotalCostPrice MONEY = 0;
	DECLARE @ItemBatch AS TABLE
	(
		ItemBatchSummaryID VARCHAR(100),
		ReceivingDate DATETIME,
		RemainingQty FLOAT,
		CostPrice MONEY
	);

	SET @DeductionQty = @Qty;

	INSERT INTO @ItemBatch
	SELECT   IBM.ItemBatchSummaryID
			,IBM.ReceivingDate
			,IBM.RemainingQty
			,IBM.CostPrice
	FROM	ItemBatchSummary IBM
	WHERE	ISNULL(Deleted,0) = 0
			AND ISNULL(IBM.RemainingQty,0) > 0
			AND IBM.ItemNo = @ItemNo
			AND IBM.InventoryLocationID = @InventoryLocationID
	ORDER BY IBM.ReceivingDate

	SET @ItemBatchSummaryID = (SELECT TOP 1 ItemBatchSummaryID FROM @ItemBatch ORDER BY ReceivingDate);

	IF @ItemBatchSummaryID IS NULL BEGIN
		SELECT  @TotalCostPrice = (@Qty * ISNULL(Userfloat1,0))
		FROM	ItemSummary
		WHERE	ISNULL(Deleted,0) = 0
				AND ItemNo = @ItemNo
				AND InventoryLocationID = @InventoryLocationID
	END

	WHILE @ItemBatchSummaryID IS NOT NULL BEGIN
		DECLARE @ItemBatchQty FLOAT;
		DECLARE @CostPrice MONEY;

		SELECT  TOP 1 
			    @ItemBatchQty = RemainingQty
			   ,@CostPrice = CostPrice 
		FROM   @ItemBatch 
		WHERE  ItemBatchSummaryID = @ItemBatchSummaryID

		IF ISNULL(@ItemBatchQty,0) > 0 BEGIN
		
			DECLARE @RemainingQty FLOAT;
			DECLARE @QtyTaken FLOAT;
			SET @DeductedQty = @Qty - @DeductionQty;

			IF ISNULL(@ItemBatchQty,0) >= ISNULL(@DeductionQty,0) BEGIN
				SET @RemainingQty = ISNULL(@ItemBatchQty,0) - ISNULL(@DeductionQty,0);
				SET @QtyTaken = @DeductionQty;
				SET @DeductionQty = 0;
			END
			ELSE BEGIN
				SET @DeductionQty = ISNULL(@DeductionQty,0) - ISNULL(@ItemBatchQty,0);
				SET @QtyTaken = @ItemBatchQty;
				SET @RemainingQty = 0;
			END

			SET @TotalCostPrice = ISNULL(@TotalCostPrice,0) + (@QtyTaken * @CostPrice);

			IF @RemainingQty > 0 BEGIN
				UPDATE  ItemBatchSummary
				SET		RemainingQty = @RemainingQty
						,ModifiedOn = GETDATE()
						,ModifiedBy = ''SYNC''
				WHERE   ItemBatchSummaryID = @ItemBatchSummaryID
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
				SELECT 	 ItemBatchSummaryID
						,ItemNo
						,InventoryLocationID
						,ReceivingDate
						,Qty
						,0 RemainingQty
						,CostPrice
						,BatchNo
						,InventoryDetRefNo
						,UniqueID
						,Deleted
						,CreatedBy
						,CreatedOn
						,ModifiedBy
						,ModifiedOn
				FROM   ItemBatchSummary
				WHERE  ItemBatchSummaryID = @ItemBatchSummaryID

				DELETE  ItemBatchSummary
				WHERE   ItemBatchSummaryID = @ItemBatchSummaryID
			END 
		END

		IF ISNULL(@DeductionQty,0) <= 0
			BREAK;

		DELETE @ItemBatch  WHERE ItemBatchSummaryID = @ItemBatchSummaryID
		SET @ItemBatchSummaryID = (SELECT TOP 1 ItemBatchSummaryID FROM @ItemBatch ORDER BY ReceivingDate);


		IF @ItemBatchSummaryID IS NULL AND ISNULL(@DeductionQty,0) > 0 BEGIN
			SET @TotalCostPrice = ISNULL(@TotalCostPrice,0) + (ISNULL(@DeductionQty,0) * @CostPrice);
		END

	END

	IF @CostingMode = ''FIFO'' AND @MovementType NOT LIKE ''%ADJUSTMENT%'' BEGIN
		UPDATE	ID
		SET		 ID.FactoryPrice = @TotalCostPrice / ID.Quantity
				,ID.CostOfGoods = @TotalCostPrice
		FROM	InventoryDet ID 
		WHERE	ID.InventoryDetRefNo = @InventoryDetID
				AND ISNULL(ID.Userflag1,0) = 0
	END
END
' 
 
END