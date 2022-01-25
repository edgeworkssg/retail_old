IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_DW_HourlySales]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_DW_HourlySales]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_DW_HourlySales]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_DW_HourlySales]
	@StartDate AS DATETIME,
	@EndDate AS DATETIME,
	@Outlet NVARCHAR(500)
AS
BEGIN

	SET NOCOUNT ON;

IF CAST (@EndDate AS DATE) >= CAST(GETDATE() AS DATE) 
	SET @EndDate = DATEADD(dd,-1,GETDATE()) 

IF OBJECT_ID(''tempdb..#TempDWHourlySales'') IS NOT NULL
  DROP TABLE #TempDWHourlySales

SELECT  DW.OrderDate
		,DW.OutletName
		,DW.Pax
		,DW.Bill
		,DW.GrossAmount
		,DW.Disc
		,DW.AfterDisc
		,DW.SvcCharge
		,DW.BefGST
		,DW.GST
		,ISNULL(DW.Rounding,0) Rounding
		,(ISNULL(DW.NettAmount,0) + ISNULL(DW.Rounding,0)) NettAmount
		,ISNULL(DW.PointSale,0) PointSale
		,ISNULL(DW.InstallmentPaymentSale,0) InstallmentPaymentSale
		,DW.Regenerate
INTO	#TempDWHourlySales
FROM	ViewDW_HourlySales_src DW
WHERE	(@Outlet = ''ALL'' OR @Outlet = ''''  OR @Outlet = ''ALL - Breakdown'' OR DW.OutletName = @Outlet)
		AND ((CAST(DW.OrderDate AS date) >= @StartDate AND CAST(DW.OrderDate AS date) <= @EndDate)
			 OR CAST(DW.OrderDate AS date) IN (SELECT DISTINCT CAST(OrderDate AS DATE) FROM DW_RegenerateDate WHERE (@Outlet = ''ALL'' OR @Outlet = '''' OR @Outlet = ''ALL - Breakdown'' OR OutletName = @Outlet)));

MERGE DW_HourlySales AS TARGET
USING #TempDWHourlySales AS SOURCE ON TARGET.OrderDate = SOURCE.OrderDate
					 AND TARGET.OutletName = SOURCE.OutletName
WHEN MATCHED THEN 
    UPDATE SET  TARGET.Pax = SOURCE.Pax
			   ,TARGET.Bill = SOURCE.Bill
			   ,TARGET.GrossAmount = SOURCE.GrossAmount
			   ,TARGET.Disc = SOURCE.Disc
			   ,TARGET.AfterDisc = SOURCE.AfterDisc
			   ,TARGET.SvcCharge = SOURCE.SvcCharge
			   ,TARGET.BefGST = SOURCE.BefGST
			   ,TARGET.GST = SOURCE.GST
			   ,TARGET.Rounding = SOURCE.Rounding
			   ,TARGET.NettAmount = SOURCE.NettAmount
			   ,TARGET.PointSale = SOURCE.PointSale
			   ,TARGET.InstallmentPaymentSale = SOURCE.InstallmentPaymentSale
			   ,TARGET.Regenerate = SOURCE.Regenerate
			   ,TARGET.LastUpdateOn = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT 
	(
		 OrderDate
		,OutletName
		,Pax 
		,Bill
		,GrossAmount
		,Disc
		,AfterDisc
		,SvcCharge
		,BefGST
		,GST
		,Rounding
		,NettAmount
		,PointSale
		,InstallmentPaymentSale
		,Regenerate
		,LastUpdateOn
	)
    VALUES 
	(
		 SOURCE.OrderDate 
		,SOURCE.OutletName 
		,SOURCE.Pax 
		,SOURCE.Bill
		,SOURCE.GrossAmount
		,SOURCE.Disc
		,SOURCE.AfterDisc
		,SOURCE.SvcCharge
		,SOURCE.BefGST
		,SOURCE.GST
		,SOURCE.Rounding
		,SOURCE.NettAmount
		,SOURCE.PointSale
		,SOURCE.InstallmentPaymentSale
		,SOURCE.Regenerate
		,GETDATE()
	);

END
' 
END

