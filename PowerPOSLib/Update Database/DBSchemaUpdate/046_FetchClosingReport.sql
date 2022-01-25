IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchClosingReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchClosingReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchClosingReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE FetchClosingReport
	 @UseFromEndDate AS BIT,
	 @UseToEndDate AS BIT,
	 @FromEndDate AS DATETIME,
	 @ToEndDate AS DATETIME,
	 @CloseCounterReportRefNo AS NVARCHAR(200),
	 @Cashier AS NVARCHAR(200),
	 @SupervisorId AS NVARCHAR(200),
	 @PointOfSaleID AS INT,
	 @OutletName AS NVARCHAR(200),
	 @DeptID AS INT,
	 @SortBy AS NVARCHAR(200),
	 @SortDir AS NVARCHAR(200)
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #ForeignTab
	(
		 CounterCloseID NVARCHAR(200)
		,CurrencyOrder INT	 
		,CurrencyCode NVARCHAR(200)
		,CurrencySymbol NVARCHAR(200)	
		,CollectedAmount MONEY		
		,RecordedAmount MONEY
	)

	DECLARE @ForeignCurrency AS TABLE
	(
		 CurrencyOrder INT
		,CurrencyCode NVARCHAR(200)
		,CurrencySymbol NVARCHAR(200)	
	)

	CREATE TABLE #ClosingTab
	(
		 CounterCloseID NVARCHAR(200)
		,StartTime DATETIME
		,EndTime DATETIME
		,Cashier NVARCHAR(200)
		,TotalSystemRecorded MONEY
		,TotalActualCollected MONEY
		,Variance MONEY
		,CashRecorded MONEY
		,CashCollected MONEY
		,NetsRecorded MONEY
		,NetsCollected MONEY
		,NetsTerminalID NVARCHAR(200)
		,VisaRecorded MONEY
		,VisaCollected MONEY
		,VisaBatchNo NVARCHAR(200)
		,AmexRecorded MONEY
		,AmexCollected MONEY
		,AmexBatchNo NVARCHAR(200)
		,ChinaNetsRecorded MONEY
		,ChinaNetsCollected MONEY
		,NetsCashCardCollected MONEY
		,NetsCashCardRecorded MONEY
		,NetsFlashPayCollected MONEY
		,NetsFlashPayRecorded MONEY
		,NetsATMCardCollected MONEY
		,NetsATMCardRecorded MONEY
		,ChinaNetsTerminalID NVARCHAR(200)
		,Payment5Recorded MONEY
		,Payment5Collected MONEY
		,Payment6Recorded MONEY
		,Payment6Collected MONEY
		,Payment7Recorded MONEY
		,Payment7Collected MONEY
		,Payment8Recorded MONEY
		,Payment8Collected MONEY
		,Payment9Recorded MONEY
		,Payment9Collected MONEY
		,Payment10Recorded MONEY
		,Payment10Collected MONEY								
		,VoucherRecorded MONEY
		,VoucherCollected MONEY
		,ChequeRecorded MONEY
		,ChequeCollected MONEY
		,ClosingCashOut MONEY
		,SMFRecorded MONEY
		,SMFCollected MONEY
		,PAMedRecorded MONEY
		,PAMedCollected MONEY
		,PWFRecorded MONEY
		,PWFCollected MONEY
		,DepositBagNo NVARCHAR(200)
		,Supervisor NVARCHAR(200)
		,OpeningBalance MONEY
		,CashIn MONEY
		,CashOut MONEY
		,PointOfSaleID INT
		,PointOfSaleName NVARCHAR(200)
		,FloatBalance MONEY
		,DepartmentID INT
		,OutletName NVARCHAR(200)
		,TotalGST MONEY
		,TotalForeignCurrency MONEY
		,ForeignCurrency1 NVARCHAR(200)
		,ForeignCurrency1Collected MONEY
		,ForeignCurrency1Recorded MONEY
		,ForeignCurrency2 NVARCHAR(200)
		,ForeignCurrency2Collected MONEY
		,ForeignCurrency2Recorded MONEY	
		,ForeignCurrency3 NVARCHAR(200)
		,ForeignCurrency3Collected MONEY
		,ForeignCurrency3Recorded MONEY		
		,ForeignCurrency4 NVARCHAR(200)
		,ForeignCurrency4Collected MONEY
		,ForeignCurrency4Recorded MONEY	
		,ForeignCurrency5 NVARCHAR(200)
		,ForeignCurrency5Collected MONEY
		,ForeignCurrency5Recorded MONEY			
		,PointRecorded MONEY			
	)

	INSERT INTO #CLosingTab
	SELECT	 CCL.CounterCloseID
			,CCL.StartTime
			,CCL.EndTime
			,UM.DisplayName AS Cashier
			,CCL.TotalSystemRecorded
			,CCL.TotalActualCollected
			,CCL.Variance
			,CCL.CashRecorded
			,CCL.CashCollected
			,CCL.NetsRecorded
			,CCL.NetsCollected
			,CCL.NetsTerminalID
			,CCL.VisaRecorded
			,CCL.VisaCollected
			,CCL.VisaBatchNo
			,CCL.AmexRecorded
			,CCL.AmexCollected
			,CCL.AmexBatchNo
			,CCL.ChinaNetsRecorded
			,CCL.ChinaNetsCollected
			,ISNULL(CCL.NetsCashCardCollected,0) NetsCashCardCollected
			,ISNULL(CCL.NetsCashCardRecorded,0) NetsCashCardRecorded
			,ISNULL(CCL.NetsFlashPayCollected,0) NetsFlashPayCollected
			,ISNULL(CCL.NetsFlashPayRecorded,0) NetsFlashPayRecorded
			,ISNULL(CCL.NetsATMCardCollected,0) NetsATMCardCollected
			,ISNULL(CCL.NetsATMCardRecorded,0) NetsATMCardRecorded
			,CCL.ChinaNetsTerminalID
			,CCL.userfld3 AS Payment5Recorded
			,CCL.userfld4 AS Payment5Collected
			,CCL.userfld5 AS Payment6Recorded
			,CCL.userfld6 AS Payment6Collected
			,CCL.Pay7Recorded AS Payment7Recorded
			,CCL.Pay7Collected AS Payment7Collected
			,CCL.Pay8Recorded AS Payment8Recorded
			,CCL.Pay8Collected AS Payment8Collected
			,CCL.Pay9Recorded AS Payment9Recorded
			,CCL.Pay9Collected AS Payment9Collected
			,CCL.Pay10Recorded AS Payment10Recorded
			,CCL.Pay10Collected AS Payment10Collected								
			,CCL.VoucherRecorded
			,CCL.VoucherCollected
			,CCL.userfloat1 AS ChequeRecorded
			,CCL.userfloat2 AS ChequeCollected
			,CCL.ClosingCashOut
			,CCL.userfloat6 AS SMFRecorded
			,CCL.userfloat7 AS SMFCollected
			,CCL.userfloat8 AS PAMedRecorded
			,CCL.userfloat9 AS PAMedCollected
			,CCL.userfloat10 AS PWFRecorded
			,CCL.userfloat11 AS PWFCollected
			,CCL.DepositBagNo
			,UM2.DisplayName AS Supervisor
			,CCL.OpeningBalance
			,CCL.CashIn
			,CCL.CashOut
			,CCL.PointOfSaleID
			,POS.PointOfSaleName
			,CCL.FloatBalance
			,POS.DepartmentID
			,POS.OutletName
			,TotalGST = ISNULL((     
				SELECT	SUM(OH.GstAmount)       
				FROM	OrderHdr OH      
				WHERE	OH.OrderDate BETWEEN CCL.StartTime AND CCL.EndTime AND OH.PointOfSaleID = CCL.PointOfSaleID AND Isvoided = 0), 0.00) 
			,CCL.TotalForeignCurrency
			,CCL.ForeignCurrency1
			,CCL.ForeignCurrency1Collected
			,CCL.ForeignCurrency1Recorded
			,CCL.ForeignCurrency2
			,CCL.ForeignCurrency2Collected
			,CCL.ForeignCurrency2Recorded
			,CCL.ForeignCurrency3
			,CCL.ForeignCurrency3Collected
			,CCL.ForeignCurrency3Recorded
			,CCL.ForeignCurrency4
			,CCL.ForeignCurrency4Collected
			,CCL.ForeignCurrency4Recorded
			,CCL.ForeignCurrency5
			,CCL.ForeignCurrency5Collected
			,CCL.ForeignCurrency5Recorded																				
			,CCL.userfloat3 AS PointRecorded
	FROM	dbo.CounterCloseLog CCL 
			INNER JOIN dbo.PointOfSale POS ON CCL.PointOfSaleID = POS.PointOfSaleID 
			INNER JOIN dbo.UserMst UM ON CCL.Cashier = UM.UserName 
			INNER JOIN dbo.UserMst UM2 ON CCL.Supervisor = UM2.UserName 
	WHERE	1=1  
			AND (CCL.EndTime <= @ToEndDate OR @UseToEndDate = 0)
			AND (CCL.EndTime >= @FromEndDate OR @UseFromEndDate = 0)	
			AND (CCL.CounterCloseID LIKE ''%''+@CloseCounterReportRefNo+''%'' OR @CloseCounterReportRefNo = '''')
			AND (CCL.Cashier LIKE ''%''+@Cashier+''%'' OR @Cashier = '''')
			AND (CCL.Supervisor LIKE ''%''+@SupervisorId+''%'' OR @SupervisorId= '''')
			AND (CCL.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
			AND (POS.OutletName = @OutletName OR @OutletName = ''ALL'')
			AND (POS.DepartmentID = @DeptID OR @DeptID= 0)
			
	INSERT INTO @ForeignCurrency
	SELECT ROW_NUMBER() OVER(PARTITION BY TAB.CurrencyCode ORDER BY TAB.CurrencyCode) No
			,TAB.CurrencyCode 
			,ISNULL(CURR.CurrencySymbol,''$'') CurrencySymbol
	FROM
	(
		SELECT  DISTINCT CCL.ForeignCurrency1 CurrencyCode
		FROM	#ClosingTab CCL WHERE	ISNULL(CCL.ForeignCurrency1,'''') LIKE ''CASH-%''
		UNION ALL 
		SELECT  DISTINCT CCL.ForeignCurrency2 CurrencyCode
		FROM	#ClosingTab CCL WHERE	ISNULL(CCL.ForeignCurrency2,'''') LIKE ''CASH-%''		
		UNION ALL 
		SELECT  DISTINCT CCL.ForeignCurrency3 CurrencyCode
		FROM	#ClosingTab CCL WHERE	ISNULL(CCL.ForeignCurrency3,'''') LIKE ''CASH-%''	
		UNION ALL 
		SELECT  DISTINCT CCL.ForeignCurrency4 CurrencyCode
		FROM	#ClosingTab CCL WHERE	ISNULL(CCL.ForeignCurrency4,'''') LIKE ''CASH-%''	
		UNION ALL 
		SELECT  DISTINCT CCL.ForeignCurrency5 CurrencyCode
		FROM	#ClosingTab CCL WHERE	ISNULL(CCL.ForeignCurrency5,'''') LIKE ''CASH-%''	
	) TAB LEFT JOIN Currencies CURR ON CURR.CurrencyCode = REPLACE(TAB.CurrencyCode,''CASH-'','''') COLLATE Latin1_General_CI_AS	 
	ORDER BY TAB.CurrencyCode

	DELETE @ForeignCurrency WHERE	CurrencyOrder <> 1

	DECLARE @CurrencyOrder INT;
	SET @CurrencyOrder = 1;

	DECLARE @CurrencyCodePointer NVARCHAR(200);
	SET @CurrencyCodePointer = (SELECT TOP 1 CurrencyCode FROM @ForeignCurrency ORDER BY CurrencyCode)

	WHILE @CurrencyCodePointer IS NOT NULL BEGIN
		INSERT INTO #ForeignTab
		SELECT	 CCL.CounterCloseID
				,@CurrencyOrder
				,@CurrencyCodePointer
				,(SELECT TOP 1 CurrencySymbol FROM @ForeignCurrency WHERE CurrencyCode = @CurrencyCodePointer) CurrencySymbol
				,CASE WHEN CCL.ForeignCurrency1 = @CurrencyCodePointer THEN CCL.ForeignCurrency1Collected
					  WHEN CCL.ForeignCurrency2 = @CurrencyCodePointer THEN CCL.ForeignCurrency2Collected
					  WHEN CCL.ForeignCurrency3 = @CurrencyCodePointer THEN CCL.ForeignCurrency3Collected
					  WHEN CCL.ForeignCurrency4 = @CurrencyCodePointer THEN CCL.ForeignCurrency4Collected
					  WHEN CCL.ForeignCurrency5 = @CurrencyCodePointer THEN CCL.ForeignCurrency5Collected END CollectedAmount
				,CASE WHEN CCL.ForeignCurrency1 = @CurrencyCodePointer THEN CCL.ForeignCurrency1Recorded
					  WHEN CCL.ForeignCurrency2 = @CurrencyCodePointer THEN CCL.ForeignCurrency2Recorded
					  WHEN CCL.ForeignCurrency3 = @CurrencyCodePointer THEN CCL.ForeignCurrency3Recorded
					  WHEN CCL.ForeignCurrency4 = @CurrencyCodePointer THEN CCL.ForeignCurrency4Recorded
					  WHEN CCL.ForeignCurrency5 = @CurrencyCodePointer THEN CCL.ForeignCurrency5Recorded END RecordedAmount				  
		FROM	#ClosingTab CCL 
		WHERE	(ISNULL(CCL.ForeignCurrency1,'''') = @CurrencyCodePointer	
					OR ISNULL(CCL.ForeignCurrency2,'''') = @CurrencyCodePointer	
					OR ISNULL(CCL.ForeignCurrency3,'''') = @CurrencyCodePointer	
					OR ISNULL(CCL.ForeignCurrency4,'''') = @CurrencyCodePointer	
					OR ISNULL(CCL.ForeignCurrency5,'''') = @CurrencyCodePointer)	

		DELETE @ForeignCurrency WHERE CurrencyCode = @CurrencyCodePointer
		SET @CurrencyOrder = @CurrencyOrder + 1;
		SET @CurrencyCodePointer = (SELECT TOP 1 CurrencyCode FROM @ForeignCurrency ORDER BY CurrencyCode)
	END

	IF ISNULL(@SortBy,'''') = '''' BEGIN
		SET @SortBy = ''CounterCloseID'';
	END

	IF ISNULL(@SortDir,'''') = '''' BEGIN
		SET @SortDir = ''ASC'';
	END

	DECLARE @sql AS NVARCHAR(MAX);

	SET @sql = N''
	SELECT	 CCL.CounterCloseID
			,CCL.StartTime
			,CCL.EndTime
			,CCL.Cashier
			,CCL.TotalSystemRecorded
			,CCL.TotalActualCollected
			,CCL.Variance
			,CCL.CashRecorded
			,CCL.CashCollected
			,CCL.NetsRecorded
			,CCL.NetsCollected
			,CCL.NetsTerminalID
			,CCL.VisaRecorded
			,CCL.VisaCollected
			,CCL.VisaBatchNo
			,CCL.AmexRecorded
			,CCL.AmexCollected
			,CCL.AmexBatchNo
			,CCL.ChinaNetsRecorded
			,CCL.ChinaNetsCollected
			,CCL.NetsCashCardCollected
			,CCL.NetsCashCardRecorded
			,CCL.NetsFlashPayCollected
			,CCL.NetsFlashPayRecorded
			,CCL.NetsATMCardCollected
			,CCL.NetsATMCardRecorded
			,CCL.ChinaNetsTerminalID
			,CCL.Payment5Recorded
			,CCL.Payment5Collected
			,CCL.Payment6Recorded
			,CCL.Payment6Collected
			,CCL.Payment7Recorded
			,CCL.Payment7Collected
			,CCL.Payment8Recorded
			,CCL.Payment8Collected
			,CCL.Payment9Recorded
			,CCL.Payment9Collected
			,CCL.Payment10Recorded
			,CCL.Payment10Collected											
			,CCL.VoucherRecorded
			,CCL.VoucherCollected
			,CCL.ChequeRecorded
			,CCL.ChequeCollected
			,CCL.ClosingCashOut
			,CCL.SMFRecorded
			,CCL.SMFCollected
			,CCL.PAMedRecorded
			,CCL.PAMedCollected
			,CCL.PWFRecorded
			,CCL.PWFCollected
			,CCL.DepositBagNo
			,CCL.Supervisor
			,CCL.OpeningBalance
			,CCL.CashIn
			,CCL.CashOut
			,CCL.PointOfSaleID
			,CCL.PointOfSaleName
			,CCL.FloatBalance
			,CCL.DepartmentID
			,CCL.OutletName
			,CCL.TotalGST
			,ISNULL(CCL.TotalForeignCurrency,0) TotalForeignCurrency
			,ISNULL(FT1.CurrencyCode,'''''''') CurrencyCode1
			,ISNULL(FT1.CurrencySymbol,''''$'''') CurrencySymbol1
			,ISNULL(FT1.CollectedAmount,0) CollectedAmount1
			,ISNULL(FT1.RecordedAmount,0) RecordedAmount1	 	
			,ISNULL(FT2.CurrencyCode,'''''''') CurrencyCode2
			,ISNULL(FT2.CurrencySymbol,''''$'''') CurrencySymbol2
			,ISNULL(FT2.CollectedAmount,0) CollectedAmount2
			,ISNULL(FT2.RecordedAmount,0) RecordedAmount2	 	
			,ISNULL(FT3.CurrencyCode,'''''''') CurrencyCode3
			,ISNULL(FT3.CurrencySymbol,''''$'''') CurrencySymbol3
			,ISNULL(FT3.CollectedAmount,0) CollectedAmount3
			,ISNULL(FT3.RecordedAmount,0) RecordedAmount3	 	
			,ISNULL(FT4.CurrencyCode,'''''''') CurrencyCode4
			,ISNULL(FT4.CurrencySymbol,''''$'''') CurrencySymbol4
			,ISNULL(FT4.CollectedAmount,0) CollectedAmount4
			,ISNULL(FT4.RecordedAmount,0) RecordedAmount4	 	
			,ISNULL(FT5.CurrencyCode,'''''''') CurrencyCode5
			,ISNULL(FT5.CurrencySymbol,''''$'''') CurrencySymbol5
			,ISNULL(FT5.CollectedAmount,0) CollectedAmount5
			,ISNULL(FT5.RecordedAmount,0) RecordedAmount5	 			
			,CCL.PointRecorded
	FROM	#ClosingTab CCL
			LEFT JOIN #ForeignTab FT1 ON FT1.CounterCloseID = CCL.CounterCloseID COLLATE Latin1_General_CI_AS AND FT1.CurrencyOrder = 1 
			LEFT JOIN #ForeignTab FT2 ON FT2.CounterCloseID = CCL.CounterCloseID COLLATE Latin1_General_CI_AS AND FT2.CurrencyOrder = 2 	
			LEFT JOIN #ForeignTab FT3 ON FT3.CounterCloseID = CCL.CounterCloseID COLLATE Latin1_General_CI_AS AND FT3.CurrencyOrder = 3 	
			LEFT JOIN #ForeignTab FT4 ON FT4.CounterCloseID = CCL.CounterCloseID COLLATE Latin1_General_CI_AS AND FT4.CurrencyOrder = 4 	
			LEFT JOIN #ForeignTab FT5 ON FT5.CounterCloseID = CCL.CounterCloseID COLLATE Latin1_General_CI_AS AND FT5.CurrencyOrder = 5
	ORDER BY ''+@SortBy+'' ''+@SortDir;

	PRINT @sql;

	EXECUTE sp_executesql @sql;
			
	DROP TABLE #ClosingTab;
	DROP TABLE #ForeignTab;
END' 
END
