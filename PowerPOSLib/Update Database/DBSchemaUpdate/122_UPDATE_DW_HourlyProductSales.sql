IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_DW_HourlyProductSales]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_DW_HourlyProductSales]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_DW_HourlyProductSales]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_DW_HourlyProductSales]
	@StartDate DATETIME,
	@EndDate DATETIME,
	@Outlet NVARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;

IF CAST (@EndDate AS DATE) >= CAST(GETDATE() AS DATE) 
	SET @EndDate = DATEADD(dd,-1,GETDATE()) 

IF OBJECT_ID(''tempdb..#TempDWHourlyProductSales'') IS NOT NULL
  DROP TABLE #TempDWHourlyProductSales

SELECT   DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME)) OrderDate
		,OH.OutletName
		,OH.ItemNo
		,SUM(OH.Quantity) Quantity
		,SUM(OH.Amount) Amount
		,0 Regenerate
INTO	#TempDWHourlyProductSales
FROM	viewDW_HourlyProductSales_src OH
WHERE	(@Outlet = ''ALL'' OR @Outlet = '''' OR @Outlet = ''ALL - Breakdown'' OR OH.OutletName = @Outlet)
		AND ((CAST(OH.OrderDate AS DATE) >= @StartDate AND CAST(OH.OrderDate AS DATE) <= @EndDate)
			  OR CAST(OH.OrderDate AS DATE) IN (SELECT DISTINCT CAST(OrderDate AS DATE) FROM DW_RegenerateDate WHERE (@Outlet = ''ALL'' OR @Outlet = ''''  OR @Outlet = ''ALL - Breakdown'' OR OutletName = @Outlet)))
GROUP BY DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME))
		,OH.OutletName
		,OH.ItemNo;

MERGE DW_HourlyProductSales AS TARGET
USING #TempDWHourlyProductSales AS SOURCE ON TARGET.OrderDate = SOURCE.OrderDate
					 AND TARGET.OutletName = SOURCE.OutletName
					 AND TARGET.ItemNo = SOURCE.ItemNo
WHEN MATCHED THEN 
    UPDATE SET  TARGET.Quantity = SOURCE.Quantity
			   ,TARGET.Amount = SOURCE.Amount
			   ,TARGET.Regenerate = SOURCE.Regenerate
			   ,TARGET.LastUpdateOn = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT 
	(
		 OrderDate
		,OutletName
		,ItemNo 
		,Quantity
		,Amount
		,Regenerate
		,LastUpdateOn
	)
    VALUES 
	(
		 SOURCE.OrderDate
		,SOURCE.OutletName
		,SOURCE.ItemNo 
		,SOURCE.Quantity
		,SOURCE.Amount
		,SOURCE.Regenerate
		,GETDATE()
	);
END
' 
END

