IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_FuturePrice]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_FuturePrice]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_FuturePrice]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_FuturePrice]
	@ExecDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	IF OBJECT_ID(''tempdb..#TempFuturePrice'') IS NOT NULL
	  DROP TABLE #TempFuturePrice

	DECLARE @RowCount INT;

	SET @RowCount = 0;

	SELECT  ItemFuturePriceID
		   ,ApplicableTo
		   ,OutletID
		   ,ItemNo
		   ,RetailPrice
		   ,CostPrice
		   ,P1
		   ,P2
		   ,P3
		   ,P4
		   ,P5
	INTO   #TempFuturePrice
	FROM   ItemFuturePrice
	WHERE  ISNULL(Deleted,0) = 0
		   AND Status = ''PENDING''
		   AND CAST(ApplicableDate AS DATE) = CAST(@ExecDate AS DATE)
	ORDER BY ItemNo, ApplicableTo, OutletID

	DECLARE @ItemFuturePriceID AS UNIQUEIDENTIFIER,
			@ApplicableTo AS VARCHAR(100),
			@OutletID AS VARCHAR(100),
			@ItemNo AS VARCHAR(50),
			@RetailPrice AS MONEY,
			@CostPrice AS MONEY,
			@P1 AS MONEY,
			@P2 AS MONEY,
			@P3 AS MONEY,
			@P4 AS MONEY,
			@P5 AS MONEY

	SET @ItemFuturePriceID = (SELECT TOP 1 ItemFuturePriceID FROM #TempFuturePrice ORDER BY ItemNo, ApplicableTo, OutletID);
	WHILE @ItemFuturePriceID IS NOT NULL BEGIN

		SELECT   @ApplicableTo = ApplicableTo
				,@OutletID = OutletID
				,@ItemNo = ItemNo
				,@RetailPrice = RetailPrice
				,@CostPrice = CostPrice
				,@P1 = P1
				,@P2 = P2
				,@P3 = P3
				,@P4 = P4
				,@P5 = P5
		FROM	#TempFuturePrice
		WHERE	ItemFuturePriceID = @ItemFuturePriceID

		IF @ApplicableTo = ''Product Master'' BEGIN
		
			UPDATE   Item
			SET		 RetailPrice = @RetailPrice
					,Userfloat6 = @P1
					,Userfloat7 = @P2
					,Userfloat8 = @P3
					,Userfloat9 = @P4
					,Userfloat10 = @P5
					,ModifiedOn = GETDATE()
					,ModifiedBy = ''JOBS - FUTURE PRICE''
			WHERE	ItemNo = @ItemNo

		END
		ELSE IF @ApplicableTo = ''Outlet'' BEGIN
			SELECT  @RowCount = COUNT(*)
			FROM    OutletGroupItemMap
			WHERE	ISNULL(Deleted,0) = 0
					AND ItemNo = @ItemNo
					AND OutletName = @OutletID

			IF @RowCount = 0 BEGIN
				INSERT INTO OutletGroupItemMap
				(
					 OutletName
					,ItemNo
					,RetailPrice
					,CostPrice
					,P1
					,P2
					,P3
					,P4
					,P5
					,IsItemDeleted
					,Deleted
					,CreatedOn
					,CreatedBy
					,ModifiedOn
					,ModifiedBy
				)
				VALUES
				(@OutletID, @ItemNo, @RetailPrice, @CostPrice, @P1, @P2, @P3, @P4, @P5, 0, 0, GETDATE(), ''SCRIPT'', GETDATE(), ''SCRIPT'')
			END
			ELSE BEGIN
				UPDATE   OutletGroupItemMap
				SET		 RetailPrice = @RetailPrice
						,CostPrice = @CostPrice
						,P1 = @P1
						,P2 = @P2
						,P3 = @P3
						,P4 = @P4
						,P5 = @P5
				WHERE	ISNULL(Deleted,0) = 0
						AND ItemNo = @ItemNo
						AND OutletName = @OutletID
			END

			UPDATE Item SET ModifiedOn = GETDATE(), ModifiedBy = ''JOBS - FUTURE PRICE'' WHERE ItemNo = @ItemNo
		END
		ELSE IF @ApplicableTo = ''Outlet Group'' BEGIN
			SELECT  @RowCount = COUNT(*)
			FROM    OutletGroupItemMap
			WHERE	ISNULL(Deleted,0) = 0
					AND ItemNo = @ItemNo
					AND OutletGroupID = @OutletID

			IF @RowCount = 0 BEGIN
				INSERT INTO OutletGroupItemMap
				(
					 OutletGroupID
					,ItemNo
					,RetailPrice
					,CostPrice
					,P1
					,P2
					,P3
					,P4
					,P5
					,IsItemDeleted
					,Deleted
					,CreatedOn
					,CreatedBy
					,ModifiedOn
					,ModifiedBy
				)
				VALUES
				(@OutletID, @ItemNo, @RetailPrice, @CostPrice, @P1, @P2, @P3, @P4, @P5, 0, 0, GETDATE(), ''SCRIPT'', GETDATE(), ''SCRIPT'')
			END 
			ELSE BEGIN
				UPDATE   OutletGroupItemMap
				SET		 RetailPrice = @RetailPrice
						,CostPrice = @CostPrice
						,P1 = @P1
						,P2 = @P2
						,P3 = @P3
						,P4 = @P4
						,P5 = @P5
				WHERE	ISNULL(Deleted,0) = 0
						AND ItemNo = @ItemNo
						AND OutletGroupID = @OutletID
			END

			UPDATE Item SET ModifiedOn = GETDATE(), ModifiedBy = ''JOBS - FUTURE PRICE'' WHERE ItemNo = @ItemNo
		END

		UPDATE	ItemFuturePrice
		SET		Status = ''COMPLETED''
		WHERE	ItemFuturePriceID = @ItemFuturePriceID

		DELETE #TempFuturePrice WHERE ItemFuturePriceID = @ItemFuturePriceID
		SET @ItemFuturePriceID = (SELECT TOP 1 ItemFuturePriceID FROM #TempFuturePrice ORDER BY ItemNo, ApplicableTo, OutletID);
	END

END' 
 
END