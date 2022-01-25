declare @statement Nvarchar(max)
set @statement = ''

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetApplicableDaysPromo]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
	set @statement = @statement + N'CREATE FUNCTION [dbo].[GetApplicableDaysPromo] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER FUNCTION [dbo].[GetApplicableDaysPromo] '	
END
set @statement = @statement + N'
  (
	-- Add the parameters for the function here
	@PromoCampaignHdrId int,
	@Complete bit
)
RETURNS varchar(200)
AS
BEGIN
	-- Declare the return variable here
	Declare @applicabledays varchar(255)
	
	Declare @days varchar(10)

	-- Add the T-SQL statements to compute the return value here
	set @applicabledays = ''''
	
	DECLARE days_cursor CURSOR FOR 
	SELECT DaysPromo
	FROM PromoDaysMap
	WHERE PromoCampaignHdrID = @PromoCampaignHdrId and ISNULL(Deleted,0) = 0
	order by DaysNumber asc
	
	OPEN days_cursor

	FETCH NEXT FROM days_cursor 
	INTO @days
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @days = ''Monday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Monday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''M'' + '',''
			END
		END
		
		IF @days = ''Tuesday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Tuesday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''Tu'' + '',''
			END
		END
		
		IF @days = ''Wednesday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Wednesday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''W'' + '',''
			END
		END
		
		IF @days = ''Thursday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Thursday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''Th'' + '',''
			END
		END
		
		IF @days = ''Friday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Friday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''F'' + '',''
			END
		END
		
		IF @days = ''Saturday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Saturday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''Sa'' + '',''
			END
		END
		
		IF @days = ''Sunday''
		BEGIN
			IF @Complete = 1
			BEGIN
				SET @applicabledays = @applicabledays + ''Sunday'' + '',''
			END
			ELSE
			BEGIN
				SET @applicabledays = @applicabledays + ''Su'' + '',''
			END
		END
		
		FETCH NEXT FROM days_cursor 
		INTO @days
	END
	
	CLOSE days_cursor;
	DEALLOCATE days_cursor;

	if LEN(@applicabledays) > 0
	BEGIN
		SET @applicabledays = SUBSTRING(@applicabledays,0, LEN(@applicabledays))
	END

	-- Return the result of the function
	RETURN @applicabledays

END
'

EXEC dbo.sp_executesql @statement