DECLARE @StartDateExec DATETIME;
DECLARE @EndDateExec DATETIME;
DECLARE @OrderYear_ INT;
DECLARE @OrderMonth_ INT;
DECLARE @OrderDay_ INT;

SET @StartDateExec = (SELECT CAST(MIN(OrderDate) AS DATE) FROM OrderHdr WHERE DataIndex IS NULL);
SET @EndDateExec = CAST(GETDATE() AS DATE);

--SELECT @StartDateExec, @EndDateExec

WHILE @StartDateExec <= @EndDateExec BEGIN
	SET @OrderYear_ = YEAR(@StartDateExec);
	SET @OrderMonth_ = MONTH(@StartDateExec);
	SET @OrderDay_ = DAY(@StartDateExec);

	--EXEC [dbo].[UPDATE_OrderHdr_DataIndex]
	--		@OrderYear = @OrderYear_,
	--		@OrderMonth = @OrderMonth_,
	--		@OrderDay = @OrderDay_

	--EXEC [dbo].[UPDATE_OrderDet_DataIndex]
	--		@OrderYear = @OrderYear_,
	--		@OrderMonth = @OrderMonth_,
	--		@OrderDay = @OrderDay_

	--EXEC [dbo].[UPDATE_ReceiptDet_DataIndex]
	--		@OrderYear = @OrderYear_,
	--		@OrderMonth = @OrderMonth_,
	--		@OrderDay = @OrderDay_

	PRINT CAST(@OrderYear_ AS VARCHAR(10))+'-'+
		  CAST(@OrderMonth_ AS VARCHAR(10))+'-'+
		  CAST(@OrderDay_ AS VARCHAR(10));

	SET @StartDateExec = DATEADD(DAY,1,@StartDateExec);
END