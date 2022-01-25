IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTransactionReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchTransactionReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTransactionReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE FetchTransactionReport
	@StartDate DATETIME,
	@EndDate DATETIME,
	@UseStartDate BIT,
	@UseEndDate BIT,
	@RefNo NVARCHAR(200),
	@CashierID NVARCHAR(200),
	@PointOfSaleID INT,
	@Outlet NVARCHAR(200),
	@PaymentType NVARCHAR(MAX),
	@Remarks NVARCHAR(MAX),
	@ShowVoidedTransaction BIT
AS
BEGIN
	 SET NOCOUNT ON; 

	;WITH CTE AS
	(
	SELECT  OH.OrderHdrID
			,OH.OrderDate
			,ISNULL(OH.userfld5,OH.OrderRefNo) OrderRefNo
			,OH.CashierID
			,OH.NettAmount
			,OH.GrossAmount
			,SUM(ROUND(ISNULL(OD.GSTAmount,0),2)) as GSTAmount
			,OH.NettAmount-SUM(ROUND(ISNULL(OD.GSTAmount,0),2)) AmountBefGST
			,OH.DiscountAmount
			,OH.MembershipNo
			,M.NameToAppear
			,OH.Remark
			,POS.PointOfSaleName
			,OU.OutletName
			,OH.IsVoided
			,RD.PaymentType
			,RD.Amount
			,RD.Change
	FROM	OrderHdr OH
			LEFT JOIN OrderDet OD on OD.OrderHdrID = OH.OrderHdrID
			LEFT JOIN Membership M ON M.MembershipNo = OH.MembershipNo
			LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
			LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
			LEFT JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
			LEFT JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
	WHERE	(ISNULL(OH.IsVoided,0) = 0 OR @ShowVoidedTransaction = 1)
			AND (OH.OrderDate >= @StartDate OR  @UseStartDate = 0)
			AND (OH.OrderDate <= @EndDate OR  @UseEndDate = 0)		
			AND ISNULL(OH.userfld5, OH.OrderRefNo) LIKE ''%''+@RefNo+''%''
			AND ISNULL(OH.CashierID,'''') LIKE ''%''+@CashierID+''%''
			AND (POS.PointOfSaleID = @PointOfSaleID OR @PointOfSaleID = 0)
			AND (OU.OutletName = @Outlet OR @Outlet = ''ALL'')
			AND (ISNULL(RD.PaymentType,'''') LIKE ''%''+@PaymentType+''%'')
			AND (ISNULL(OH.Remark,'''') LIKE ''%''+@Remarks+''%'')
	GROUP BY OH.OrderHdrID
			,OH.OrderDate
			,ISNULL(OH.userfld5,OH.OrderRefNo)
			,OH.CashierID
			,OH.NettAmount
			,OH.NettAmount
			,OH.GrossAmount
			,OH.DiscountAmount
			,OH.MembershipNo
			,M.NameToAppear
			,OH.Remark
			,POS.PointOfSaleName
			,OU.OutletName
			,OH.IsVoided
			,RD.PaymentType
			,RD.Amount
			,RD.Change
			,RD.ReceiptDetID
	)
	SELECT   OH.OrderHdrID
			,OH.OrderDate
			,OH.OrderRefNo 
			,OH.CashierID
			,1 TransactionCount
			,OH.NettAmount Amount
			,OH.GrossAmount
			,OH.GSTAmount GST
			,OH.AmountBefGST
			,OH.DiscountAmount
			,OH.MembershipNo	
			,OH.NameToAppear
			,OH.Remark
			,OH.PointOfSaleName
			,OH.OutletName
			,OH.IsVoided
			,STUFF((
				SELECT '', '' + [PaymentType] + '':'' + CAST([Amount] AS VARCHAR(MAX)) 
				FROM CTE 
				WHERE (OrderHdrID = OH.OrderHdrID) 
				FOR XML PATH(''''),TYPE).value(''(./text())[1]'',''VARCHAR(MAX)'')
			  ,1,2,'''') AS PaymentType	
			,SUM(CASE WHEN OH.PaymentType = ''CASH'' THEN OH.Amount + ISNULL(OH.Change,0) ELSE 0 END) as CashReceived
			,SUM(CASE WHEN OH.PaymentType = ''CASH'' THEN ISNULL(OH.Change,0) ELSE 0 END) as Change		
	FROM	CTE OH
	GROUP BY OH.OrderHdrID
			,OH.OrderDate
			,OH.OrderRefNo
			,OH.CashierID
			,OH.NettAmount
			,OH.GrossAmount
			,OH.GSTAmount
			,OH.AmountBefGST
			,OH.DiscountAmount
			,OH.MembershipNo
			,OH.NameToAppear
			,OH.Remark
			,OH.PointOfSaleName
			,OH.OutletName
			,OH.IsVoided
	ORDER BY OH.OrderDate DESC
END' 
END
