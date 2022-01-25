IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_DW_HourlyPayment]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UPDATE_DW_HourlyPayment]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UPDATE_DW_HourlyPayment]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[UPDATE_DW_HourlyPayment]
	@StartDate DATETIME,
	@EndDate DATETIME,
	@Outlet NVARCHAR(500)

AS
BEGIN
	SET NOCOUNT ON;
IF @EndDate >= CONVERT(datetime,CONVERT(date, GETDATE()))
	SET @EndDate = DATEADD(ss,-1,CONVERT(datetime,CONVERT(date, GETDATE())))	
	
IF OBJECT_ID(''tempdb..#TempDWHourlyPayment'') IS NOT NULL
  DROP TABLE #TempDWHourlyPayment

SELECT   TAB.OrderDate
		,TAB.OutletName 
		,ISNULL(TAB.payByCash,0) PayByCash
		,ISNULL(TAB.pay01,0) pay01
		,ISNULL(TAB.pay02,0) pay02
		,ISNULL(TAB.pay03,0) pay03
		,ISNULL(TAB.pay04,0) pay04
		,ISNULL(TAB.pay05,0) pay05
		,ISNULL(TAB.pay06,0) pay06
		,ISNULL(TAB.pay07,0) pay07
		,ISNULL(TAB.pay08,0) pay08
		,ISNULL(TAB.pay09,0) pay09
		,ISNULL(TAB.pay10,0) pay10
		,ISNULL(TAB.pay11,0) pay11
		,ISNULL(TAB.pay12,0) pay12
		,ISNULL(TAB.pay13,0) pay13
		,ISNULL(TAB.pay14,0) pay14
		,ISNULL(TAB.pay15,0) pay15
		,ISNULL(TAB.pay16,0) pay16
		,ISNULL(TAB.pay17,0) pay17
		,ISNULL(TAB.pay18,0) pay18
		,ISNULL(TAB.pay19,0) pay19
		,ISNULL(TAB.pay20,0) pay20
		,ISNULL(TAB.pay21,0) pay21
		,ISNULL(TAB.pay22,0) pay22
		,ISNULL(TAB.pay23,0) pay23
		,ISNULL(TAB.pay24,0) pay24
		,ISNULL(TAB.pay25,0) pay25
		,ISNULL(TAB.pay26,0) pay26
		,ISNULL(TAB.pay27,0) pay27
		,ISNULL(TAB.pay28,0) pay28
		,ISNULL(TAB.pay29,0) pay29
		,ISNULL(TAB.pay30,0) pay30
		,ISNULL(TAB.pay31,0) pay31
		,ISNULL(TAB.pay32,0) pay32
		,ISNULL(TAB.pay33,0) pay33
		,ISNULL(TAB.pay34,0) pay34
		,ISNULL(TAB.pay35,0) pay35
		,ISNULL(TAB.pay36,0) pay36
		,ISNULL(TAB.pay37,0) pay37
		,ISNULL(TAB.pay38,0) pay38
		,ISNULL(TAB.pay39,0) pay39
		,ISNULL(TAB.pay40,0) pay40
		,ISNULL(TAB.payOthers,0) payOthers
		,ISNULL(TAB.totalpayment,0) totalpayment
		,ISNULL(TAB.pointAllocated,0) pointAllocated
		,ISNULL(TAB.payByInstallment,0) payByInstallment
		,ISNULL(TAB.payByPoint,0) payByPoint
		,0 Regenerate
INTO	#TempDWHourlyPayment
FROM	viewDW_HourlyPayment_src TAB
WHERE	(@Outlet = ''ALL'' OR @Outlet = '''' OR TAB.OutletName = @Outlet)
		AND ((CAST(TAB.OrderDate AS DATE) >= @StartDate AND CAST(TAB.OrderDate AS DATE) <= @EndDate)
			  OR CAST(TAB.OrderDate AS DATE) IN (SELECT DISTINCT CAST(OrderDate AS DATE) FROM DW_RegenerateDate WHERE (@Outlet = ''ALL'' OR @Outlet = '''' OR OutletName = @Outlet)));

MERGE DW_HourlyPayment AS TARGET
USING #TempDWHourlyPayment AS SOURCE ON TARGET.OrderDate = SOURCE.OrderDate
					 AND TARGET.OutletName = SOURCE.OutletName
WHEN MATCHED THEN 
    UPDATE SET  TARGET.payByCash = SOURCE.payByCash
			   ,TARGET.pay01 = SOURCE.pay01
			   ,TARGET.pay02 = SOURCE.pay02
			   ,TARGET.pay03 = SOURCE.pay03
			   ,TARGET.pay04 = SOURCE.pay04
			   ,TARGET.pay05 = SOURCE.pay05
			   ,TARGET.pay06 = SOURCE.pay06
			   ,TARGET.pay07 = SOURCE.pay07
			   ,TARGET.pay08 = SOURCE.pay08
			   ,TARGET.pay09 = SOURCE.pay09
			   ,TARGET.pay10 = SOURCE.pay10
			   ,TARGET.pay11 = SOURCE.pay11
			   ,TARGET.pay12 = SOURCE.pay12
			   ,TARGET.pay13 = SOURCE.pay13
			   ,TARGET.pay14 = SOURCE.pay14
			   ,TARGET.pay15 = SOURCE.pay15
			   ,TARGET.pay16 = SOURCE.pay16
			   ,TARGET.pay17 = SOURCE.pay17
			   ,TARGET.pay18 = SOURCE.pay18
			   ,TARGET.pay19 = SOURCE.pay19
			   ,TARGET.pay20 = SOURCE.pay20
			   ,TARGET.pay21 = SOURCE.pay21
			   ,TARGET.pay22 = SOURCE.pay22
			   ,TARGET.pay23 = SOURCE.pay23
			   ,TARGET.pay24 = SOURCE.pay24
			   ,TARGET.pay25 = SOURCE.pay25
			   ,TARGET.pay26 = SOURCE.pay26
			   ,TARGET.pay27 = SOURCE.pay27
			   ,TARGET.pay28 = SOURCE.pay28
			   ,TARGET.pay29 = SOURCE.pay29
			   ,TARGET.pay30 = SOURCE.pay30
			   ,TARGET.pay31 = SOURCE.pay31
			   ,TARGET.pay32 = SOURCE.pay32
			   ,TARGET.pay33 = SOURCE.pay33
			   ,TARGET.pay34 = SOURCE.pay34
			   ,TARGET.pay35 = SOURCE.pay35
			   ,TARGET.pay36 = SOURCE.pay36
			   ,TARGET.pay37 = SOURCE.pay37
			   ,TARGET.pay38 = SOURCE.pay38
			   ,TARGET.pay39 = SOURCE.pay39
			   ,TARGET.pay40 = SOURCE.pay40
			   ,TARGET.payOthers = SOURCE.payOthers
			   ,TARGET.totalpayment = SOURCE.totalpayment
			   ,TARGET.pointAllocated = SOURCE.pointAllocated
			   ,TARGET.payByInstallment = SOURCE.payByInstallment
			   ,TARGET.payByPoint = SOURCE.payByPoint
			   ,TARGET.Regenerate = SOURCE.Regenerate
			   ,TARGET.LastUpdateOn = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT 
	(
		 OrderDate
		,OutletName
		,payByCash 
		,pay01
		,pay02
		,pay03
		,pay04
		,pay05
		,pay06
		,pay07
		,pay08
		,pay09
		,pay10
		,pay11
		,pay12
		,pay13
		,pay14
		,pay15
		,pay16
		,pay17
		,pay18
		,pay19
		,pay20
		,pay21
		,pay22
		,pay23
		,pay24
		,pay25
		,pay26
		,pay27
		,pay28
		,pay29
		,pay30
		,pay31
		,pay32
		,pay33
		,pay34
		,pay35
		,pay36
		,pay37
		,pay38
		,pay39
		,pay40
		,payOthers
		,totalpayment
		,pointAllocated
		,payByInstallment
		,payByPoint
		,Regenerate
		,LastUpdateOn
	)
    VALUES 
	(
		 SOURCE.OrderDate
		,SOURCE.OutletName
		,SOURCE.payByCash 
		,SOURCE.pay01
		,SOURCE.pay02
		,SOURCE.pay03
		,SOURCE.pay04
		,SOURCE.pay05
		,SOURCE.pay06
		,SOURCE.pay07
		,SOURCE.pay08
		,SOURCE.pay09
		,SOURCE.pay10
		,SOURCE.pay11
		,SOURCE.pay12
		,SOURCE.pay13
		,SOURCE.pay14
		,SOURCE.pay15
		,SOURCE.pay16
		,SOURCE.pay17
		,SOURCE.pay18
		,SOURCE.pay19
		,SOURCE.pay20
		,SOURCE.pay21
		,SOURCE.pay22
		,SOURCE.pay23
		,SOURCE.pay24
		,SOURCE.pay25
		,SOURCE.pay26
		,SOURCE.pay27
		,SOURCE.pay28
		,SOURCE.pay29
		,SOURCE.pay30
		,SOURCE.pay31
		,SOURCE.pay32
		,SOURCE.pay33
		,SOURCE.pay34
		,SOURCE.pay35
		,SOURCE.pay36
		,SOURCE.pay37
		,SOURCE.pay38
		,SOURCE.pay39
		,SOURCE.pay40
		,SOURCE.payOthers
		,SOURCE.totalpayment
		,SOURCE.pointAllocated
		,SOURCE.payByInstallment
		,SOURCE.payByPoint
		,SOURCE.Regenerate
		,GETDATE()
	);

END
' 
END

