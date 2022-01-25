IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetDWHourlyProductSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnGetDWHourlyProductSales]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetDWHourlyProductSales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
EXECUTE dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fnGetDWHourlyProductSales]
(
	@StartDate DATETIME,
	@EndDate DATETIME,
	@OutletName NVARCHAR(MAX),
	@ItemNo NVARCHAR(MAX),
	@CutOffHour INT
)
RETURNS @Result TABLE
(
	 OrderDate DATETIME
	,OutletName NVARCHAR(MAX)
	,ItemNo NVARCHAR(MAX)
    ,Quantity MONEY
    ,Amount MONEY
)
AS
BEGIN

	DECLARE @Outlet AS TABLE (OutletName NVARCHAR(500));

	INSERT INTO @Outlet
	SELECT DISTINCT DATA
	FROM	dbo.fnSplitRow(@OutletName,'','');

	INSERT INTO @Result
	SELECT   OrderDate
			,OutletName
			,ItemNo
			,Quantity
			,Amount
	FROM	viewDW_HourlyProductSales VW
	WHERE	(  @OutletName = ''ALL'' 
			OR @OutletName = ''ALL-BreakDown''
			OR @OutletName = ''ALL - BreakDown''
			OR @OutletName = '''' 
			OR VW.OutletName IN (SELECT OutletName FROM @Outlet))
			AND (CASE WHEN DATEPART(HH,VW.OrderDate) < @CutOffHour AND @CutoffHour > 0 
					THEN CAST(DATEADD(DD,-1,VW.OrderDate) AS DATE) ELSE CAST(VW.OrderDate AS DATE) 
					END) >= @StartDate
			AND (CASE WHEN DATEPART(HH,VW.OrderDate) < @CutOffHour AND @CutoffHour > 0 
					THEN CAST(DATEADD(DD,-1,VW.OrderDate) AS DATE) ELSE CAST(VW.OrderDate AS DATE) 
					END) <= @EndDate
			AND (@ItemNo = '''' OR @ItemNo = ''ALL'' OR VW.ItemNo = @ItemNo)

	IF CAST(@EndDate AS DATE) >= CAST(GETDATE() AS DATE) BEGIN
		INSERT INTO @Result
		SELECT   OrderDate
				,OutletName
				,ItemNo
				,Quantity
				,Amount
		FROM	viewDW_HourlyProductSales_today_src VW
		WHERE	(  @OutletName = ''ALL'' 
				OR @OutletName = ''ALL-BreakDown''
				OR @OutletName = ''ALL - BreakDown''
				OR @OutletName = '''' 
				OR VW.OutletName IN (SELECT OutletName FROM @Outlet))
				AND (CASE WHEN DATEPART(HH,VW.OrderDate) < @CutOffHour AND @CutoffHour > 0 
					  THEN CAST(DATEADD(DD,-1,VW.OrderDate) AS DATE) ELSE CAST(VW.OrderDate AS DATE) 
					  END) >= @StartDate
				AND (CASE WHEN DATEPART(HH,VW.OrderDate) < @CutOffHour AND @CutoffHour > 0 
					  THEN CAST(DATEADD(DD,-1,VW.OrderDate) AS DATE) ELSE CAST(VW.OrderDate AS DATE) 
					  END) <= @EndDate
				AND (@ItemNo = '''' OR @ItemNo = ''ALL'' OR VW.ItemNo = @ItemNo)
	END

    RETURN

END
' 
END
