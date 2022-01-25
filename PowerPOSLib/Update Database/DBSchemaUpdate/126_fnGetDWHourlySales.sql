IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetDWHourlySales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnGetDWHourlySales]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetDWHourlySales]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fnGetDWHourlySales]
(
	@StartDate DATETIME,
	@EndDate DATETIME,
	@OutletName NVARCHAR(MAX),
	@CutOffHour INT
)
RETURNS @Result TABLE
(
	 OrderDate DATETIME
	,OutletName NVARCHAR(MAX)
    ,bill INT
    ,pax INT
    ,grossamount MONEY
    ,disc MONEY
    ,afterdisc MONEY
    ,svccharge MONEY
    ,befgst MONEY
    ,gst MONEY
    ,rounding MONEY
    ,nettamount MONEY
    ,pointSale MONEY
    ,installmentPaymentSale MONEY
    ,payByCash MONEY
    ,pay01 MONEY
    ,pay02 MONEY
    ,pay03 MONEY
    ,pay04 MONEY
    ,pay05 MONEY
    ,pay06 MONEY
    ,pay07 MONEY
    ,pay08 MONEY
    ,pay09 MONEY
    ,pay10 MONEY
    ,pay11 MONEY
    ,pay12 MONEY
    ,pay13 MONEY
    ,pay14 MONEY
    ,pay15 MONEY
    ,pay16 MONEY
    ,pay17 MONEY
    ,pay18 MONEY
    ,pay19 MONEY
    ,pay20 MONEY
    ,pay21 MONEY
    ,pay22 MONEY
    ,pay23 MONEY
    ,pay24 MONEY
    ,pay25 MONEY
    ,pay26 MONEY
    ,pay27 MONEY
    ,pay28 MONEY
    ,pay29 MONEY
    ,pay30 MONEY
    ,pay31 MONEY
    ,pay32 MONEY
    ,pay33 MONEY
    ,pay34 MONEY
    ,pay35 MONEY
    ,pay36 MONEY
    ,pay37 MONEY
    ,pay38 MONEY
    ,pay39 MONEY
    ,pay40 MONEY
    ,payOthers MONEY
    ,totalpayment MONEY
    ,pointAllocated MONEY
    ,payByInstallment MONEY
    ,payByPoint MONEY
)
AS
BEGIN

	DECLARE @Outlet AS TABLE (OutletName NVARCHAR(500));

	INSERT INTO @Outlet
	SELECT DISTINCT DATA
	FROM	dbo.fnSplitRow(@OutletName,'','');

	INSERT INTO @Result
	SELECT * 
	FROM	viewDW_HourlySales VW
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

	IF CAST(@EndDate AS DATE) >= CAST(GETDATE() AS DATE) BEGIN
		INSERT INTO @Result
		SELECT * 
		FROM	viewDW_HourlySales_today_src VW
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
	END

    RETURN

END' 
END
