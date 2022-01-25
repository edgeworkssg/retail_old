declare @statement Nvarchar(max)
set @statement = ''

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetTotalFloatAmount]') AND type in (N'P', N'PC'))
BEGIN
	set @statement = @statement + N'CREATE PROCEDURE [dbo].[GetTotalFloatAmount] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER PROCEDURE [dbo].[GetTotalFloatAmount] '	
END
set @statement = @statement + N' 
	@PointOfSaleID int
AS
BEGIN
	DECLARE @closingtime datetime 
	DECLARE @closeouttime datetime 
	
	SET @closingtime = ''1979-11-3''
	
	SELECT TOP 1 @closeouttime = ISNULL(EndTime,'''')
	from CounterCloseLog 
	where EndTime <= GETDATE() and PointOfSaleID = @PointOfSaleID
	Order by EndTime desc
	
	IF ISNULL(@closeouttime,'''') <> ''''
	begin
		set @closingtime = @closeouttime
	end
	
	SELECT ISNULL(SUM(amount),0)
	FROM CashRecording
	WHERE CashRecordingTime <= GETDATE() and CashRecordingTime >= @closingtime 
		and CashRecordingTypeId = 3 and PointOfSaleID = @PointOfSaleID
	
	RETURN 
	
END
'

EXEC dbo.sp_executesql @statement