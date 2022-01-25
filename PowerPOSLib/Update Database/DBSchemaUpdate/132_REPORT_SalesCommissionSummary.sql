IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_SalesCommissionSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_SalesCommissionSummary]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_SalesCommissionSummary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_SalesCommissionSummary]
	@FilterStartDate AS DATETIME,
	@FilterEndDate AS DATETIME,
	@FilterUsername AS NVARCHAR(20) = ''ALL'',
	@ShowUnpaidOnly AS INT = 0,
	@Generate AS INT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @StartDate DATETIME,
			@EndDate DATETIME,
			@UserName NVARCHAR(20),
			@Commission MONEY,
			@BasicSalary MONEY,
			@OtherAllowance MONEY,
			@IsExist INT,
			@Month NVARCHAR(20),
			@ID INT,
			@Status NVARCHAR(20),
			@CommissionBasedOn NVARCHAR(20);
			
	DECLARE @TempMonth AS TABLE 
	(
		StartDate [date] NULL,
		EndDate [date] NULL
	);

	DECLARE @TempSP AS TABLE 
	(
		Sales NVARCHAR(50),
		SchemeName NVARCHAR(50),
		SalesTotalQuantity DECIMAL(20,5),
		SalesTotalAmount MONEY,
		TotalCommission MONEY
	);

	DECLARE @TempResult AS TABLE 
	(
		[Month] [datetime] NULL,
		[Staff] [nvarchar](50) NULL,
		[Salary] [money] NULL,
		[OtherAllowance] [money] NULL,
		[Commission] [money] NULL,
		[Total] [money] NULL
	);
	
	IF @Generate = 1
	BEGIN

		INSERT INTO @TempMonth
		SELECT  LEFT(CONVERT(VARCHAR(20), DATEADD(MONTH, x.number, @FilterStartDate), 112), 6) + ''01'' as StartDate, DATEADD(DAY, -1, DATEADD(MONTH, x.number + 1, @FilterStartDate)) as EndDate
		FROM    master.dbo.spt_values x
		WHERE   x.type = ''P''        
		AND     x.number <= DATEDIFF(MONTH, @FilterStartDate, @FilterEndDate);

		DECLARE db_cursor CURSOR LOCAL FOR  
		SELECT StartDate, EndDate
		FROM @TempMonth;
		
		DECLARE db_cursor2 CURSOR LOCAL FOR  
		SELECT 
			UserName, userfloat1, userfloat2
		FROM UserMst WHERE Deleted = 0 AND IsASalesPerson = 1;

		OPEN db_cursor   
		FETCH NEXT FROM db_cursor INTO @StartDate, @EndDate;

		WHILE @@FETCH_STATUS = 0   
		BEGIN		
			OPEN db_cursor2   
			FETCH NEXT FROM db_cursor2 INTO @Username, @BasicSalary, @OtherAllowance
			
			SELECT @Month = LEFT(DATENAME(MONTH, DATEADD(MONTH, 0, @StartDate)),3) + '' '' + DATENAME(YEAR, DATEADD(MONTH, 0, @StartDate));
			
			WHILE @@FETCH_STATUS = 0   
			BEGIN
				--SELECT @Month
				
				INSERT INTO @TempSP
					Exec [dbo].[REPORT_NewCommissionReport] @FilterStartDate = @StartDate, @FilterEndDate = @EndDate, @FilterUserName = ''ALL'', @Generate = @Generate, @Month = @Month
				
				SELECT @Commission = SUM(TotalCommission) FROM @TempSP WHERE Sales = @Username;
				
				INSERT INTO @TempResult VALUES (@StartDate, @UserName, ISNULL(@BasicSalary, 0), ISNULL(@OtherAllowance, 0), ISNULL(@Commission, 0), ISNULL(@BasicSalary, 0) + ISNULL(@OtherAllowance, 0) + ISNULL(@Commission, 0));
				
				IF @Generate = 1
				BEGIN
					SELECT @IsExist = COUNT(*) FROM SalesCommissionSummary WHERE Month = @Month AND Staff = @UserName;
					
					IF @IsExist <= 0
					BEGIN
						INSERT INTO SalesCommissionSummary (Month, MonthDate, Staff, Salary, OtherAllowance, Commission, Total, Status, CreatedOn, CreatedBy) 
						VALUES 
						(@Month, @StartDate, @UserName, ISNULL(@BasicSalary, 0), ISNULL(@OtherAllowance, 0), ISNULL(@Commission, 0), ISNULL(@BasicSalary, 0) + ISNULL(@OtherAllowance, 0) + ISNULL(@Commission, 0), ''Unpaid'', GETDATE(), ''SYSTEM'');
					END
					ELSE
					BEGIN
						SELECT @ID = ID, @Status = Status FROM SalesCommissionSummary WHERE Month = @Month AND Staff = @UserName;
						IF @Status = ''Unpaid''
						BEGIN
							UPDATE SalesCommissionSummary SET Salary = ISNULL(@BasicSalary, 0), OtherAllowance = ISNULL(@OtherAllowance, 0), Commission = ISNULL(@Commission, 0), Total = ISNULL(@BasicSalary, 0) + ISNULL(@OtherAllowance, 0) + ISNULL(@Commission, 0), ModifiedOn = GETDATE(), ModifiedBy = ''SYSTEM'' WHERE ID = @ID
						END
					END
				END
				
				DELETE FROM @TempSP;
			
				FETCH NEXT FROM db_cursor2 INTO @Username, @BasicSalary, @OtherAllowance;
			END   
			
			INSERT INTO @TempSP
				Exec [dbo].[REPORT_NewCommissionReport] @FilterStartDate = @StartDate, @FilterEndDate = @EndDate, @FilterUserName = ''ALL'', @Generate = @Generate, @Month = @Month
			
			CLOSE db_cursor2;
			
			FETCH NEXT FROM db_cursor INTO @StartDate, @EndDate
		END   

		CLOSE db_cursor;
	END

	IF @ShowUnpaidOnly = 1
		SELECT @Status = ''Unpaid'';
	ELSE
		SELECT @Status = ''%'';

	SELECT
		ID,
		Month,
		MonthDate,
		Staff,
		Salary,
		OtherAllowance,
		Commission,
		Total,
		Status
	FROM SalesCommissionSummary B
	WHERE
		MonthDate >= @FilterStartDate AND MonthDate < @FilterEndDate
		AND (@FilterUserName = '''' OR @FilterUserName = ''ALL'' OR Staff = @FilterUserName)
		AND Status LIKE @Status
	ORDER BY MonthDate ASC;		
END' 
 
END