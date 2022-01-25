IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetSalesCommissionDetail]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fnGetSalesCommissionDetail]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnGetSalesCommissionDetail]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[fnGetSalesCommissionDetail]
(
	@StartDate DATETIME,
	@EndDate DATETIME,
	@OutletName NVARCHAR(500),
	@PointOfSaleID INT,
	@SalesPersonID NVARCHAR(500),
	@CategoryName NVARCHAR(500),
	@ItemDepartmentID NVARCHAR(500),
	@Search NVARCHAR(500)

)
RETURNS @Result TABLE
(
	 OrderHdrID NVARCHAR(500)
	,PointOfSaleID INT
	,PointOfSaleName NVARCHAR(500)
	,OutletName NVARCHAR(500)
	,OrderRefNo NVARCHAR(500)
	,OrderDate DATETIME
	,SalesPersonID NVARCHAR(500)
	,SalesPersonName NVARCHAR(500)
	,OrderDetID NVARCHAR(500)
	,ItemDepartmentID NVARCHAR(500)
	,ItemDepartmentName NVARCHAR(500)
	,CategoryID NVARCHAR(500)
	,CategoryName NVARCHAR(500)
	,ItemNo NVARCHAR(500)
	,Barcode NVARCHAR(500)
	,ItemName NVARCHAR(500)
	,ItemType NVARCHAR(500)
	,PointRedeemable NVARCHAR(500)
	,Quantity DECIMAL(18,8)
	,Amount MONEY
	,GSTAmount MONEY
	,POINTReceipt MONEY
	,SALESReceipt MONEY
	,POINTAmount MONEY
	,SALESAmount MONEY
	,RowType NVARCHAR(500)
	,PointPayment MONEY
	,SalesPayment MONEY
	,PointPaymentBefGST MONEY
	,SalesPaymentBefGST MONEY
	,Attributes1 NVARCHAR(500)
)
AS
BEGIN

DECLARE @Temp AS TABLE
(
	 OrderHdrID NVARCHAR(500)
	,OrderRefNo NVARCHAR(500)
	,OrderDate DATETIME
	,SalesPersonID NVARCHAR(500)
	,SalesPersonName NVARCHAR(500)
	,OrderDetID NVARCHAR(500)
	,ItemDepartmentID NVARCHAR(500)
	,ItemDepartmentName NVARCHAR(500)
	,CategoryID NVARCHAR(500)
	,CategoryName NVARCHAR(500)
	,ItemNo NVARCHAR(500)
	,Barcode NVARCHAR(500)
	,ItemName NVARCHAR(500)
	,ItemType NVARCHAR(500)
	,PointRedeemable NVARCHAR(500)
	,Quantity DECIMAL(18,8)
	,UnitPrice MONEY	
	,Amount MONEY
	,GSTAmount MONEY
	,POINTReceipt MONEY
	,SALESReceipt MONEY
	,PointOfSaleID INT
	,PointOfSaleName NVARCHAR(500)
	,OutletName NVARCHAR(500)
	,Attributes1 NVARCHAR(500)
	,POINTAmount MONEY
	,SALESAmount MONEY	
	,RowType NVARCHAR(500)
)

INSERT INTO @Temp
SELECT   TAB.*
		,CASE WHEN TAB.PointRedeemable IN (''D'',''T'') THEN TAB.Amount ELSE 0 END POINTAmount
		,CASE WHEN TAB.PointRedeemable = ''N'' THEN TAB.Amount ELSE 0 END SALESAmount
		,CASE WHEN TAB.PointRedeemable = ''N'' THEN 1
			  WHEN TAB.PointRedeemable IN (''D'',''T'') THEN 2
			  ELSE 0 END RowType
FROM (
	SELECT   OH.OrderHdrID
			,ISNULL(OH.Userfld5,OH.OrderRefNo) OrderRefNo
			,OH.OrderDate
			,UM.UserName SalesPersonID
			,UM.DisplayName SalesPersonName
			,OD.OrderDetID
			,ID.ItemDepartmentID
			,ID.DepartmentName ItemDepartmentName
			,ISNULL(CTG.Category_ID,'''') CategoryID
			,CTG.CategoryName
			,I.ItemNo
			,I.Barcode
			,I.ItemName
			, CASE  WHEN (I.userfld10 = ''D'' or I.userfld10 =''T'') THEN ''SERVICE''     
					WHEN (I.IsInInventory = 1) THEN ''PRODUCT''     
					WHEN (I.IsServiceItem = 1) THEN ''SERVICE'' ELSE ''OTHERS'' END AS ItemType 
			,CASE WHEN ISNULL(OD.Userflag5,0) = 1 THEN ''T''
				  WHEN ISNULL(I.userfld9,'''') = ''D'' THEN ''D''
				  ELSE ''N'' END PointRedeemable
			,OD.Quantity
			,OD.UnitPrice
			,CASE WHEN ISNULL(OD.Userflag5,0) = 1 THEN ISNULL(OD.Userfloat8,OD.Amount) ELSE OD.Amount END Amount
            ,CASE WHEN ISNULL(OD.Userflag5,0) = 1 THEN ISNULL(OD.Userfloat8,OD.Amount) / (1 + (oh.GST / 100)) * (oh.GST / 100) ELSE OD.GSTAmount END GSTAmount
			--,OD.GSTAmount
			,ISNULL(RD.POINTReceipt,0) POINTReceipt
			,ISNULL(RD.SALESReceipt,0) SALESReceipt
			,POS.PointOfSaleID
			,POS.PointOfSaleName
			,POS.OutletName
			,I.Attributes1
	FROM	OrderHdr OH WITH(NOLOCK)
			INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
			INNER JOIN OrderDet OD WITH(NOLOCK) ON OD.OrderHdrID = OH.OrderHdrID
			INNER JOIN Item I ON I.ItemNo = OD.ItemNo
			INNER JOIN Category CTG ON CTG.CategoryName = I.CategoryName
			INNER JOIN ItemDepartment ID ON ID.ItemDepartmentID = CTG.ItemDepartmentID
			LEFT JOIN SalesCommissionRecord SCR WITH(NOLOCK) ON SCR.OrderHdrID = OH.OrderHdrID
			INNER JOIN UserMst UM ON UM.UserName = ISNULL(NULLIF(OD.Userfld1,''''), SCR.SalesPersonID) 
			LEFT JOIN (
				SELECT   RH.OrderHdrID
						,SUM(CASE WHEN RD.PaymentType IN (''POINTS'') THEN RD.Amount ELSE 0 END) POINTReceipt
						,SUM(CASE WHEN RD.PaymentType NOT IN (''POINTS'',''PACKAGE'') THEN RD.Amount ELSE 0 END) SALESReceipt
				FROM	OrderHdr OH
						INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
						LEFT JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
						LEFT JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
				WHERE	OH.IsVoided = 0
						AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE) AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
						AND (@OutletName = ''ALL'' 
							OR @OutletName = ''ALL-BreakDown''
							OR @OutletName = ''ALL - BreakDown''
							OR @OutletName = '''' 
							OR POS.OutletName = @OutletName)
						AND (@PointOfSaleID = 0 OR POS.PointOfSaleID = @PointOfSaleID)
						OR ISNULL(OH.Userfld5,OH.OrderRefNo) LIKE ''%''+@Search+''%''
				GROUP BY RH.OrderHdrID
			) RD ON RD.OrderHdrID = OH.OrderHdrID
	WHERE	ISNULL(OH.IsVoided,0) = 0 AND ISNULL(OD.IsVoided,0) = 0
			AND CTG.CategoryName <> ''SYSTEM''  
			AND CAST(OH.OrderDate AS DATE) >= CAST(@StartDate AS DATE) AND CAST(OH.OrderDate AS DATE) <= CAST(@EndDate AS DATE)
			AND (@OutletName = ''ALL'' 
				OR @OutletName = ''ALL-BreakDown''
				OR @OutletName = ''ALL - BreakDown''
				OR @OutletName = '''' 
				OR POS.OutletName = @OutletName)
			AND (@PointOfSaleID = 0 OR POS.PointOfSaleID = @PointOfSaleID)
			OR ISNULL(OH.Userfld5,OH.OrderRefNo) LIKE ''%''+@Search+''%''
) TAB

INSERT INTO @Result
SELECT   FINAL.OrderHdrID
		,FINAL.PointOfSaleID 
		,FINAL.PointOfSaleName
		,FINAL.OutletName
		,FINAL.OrderRefNo
		,FINAL.OrderDate 
		,FINAL.SalesPersonID
		,FINAL.SalesPersonName
		,FINAL.OrderDetID
		,FINAL.ItemDepartmentID
		,FINAL.ItemDepartmentName
		,FINAL.CategoryID
		,FINAL.CategoryName
		,FINAL.ItemNo
		,FINAL.Barcode
		,FINAL.ItemName
		,FINAL.ItemType
		,FINAL.PointRedeemable
		,FINAL.Quantity 
		,FINAL.Amount 
		,FINAL.GSTAmount
		,FINAL.POINTReceipt 
		,FINAL.SALESReceipt 
		,FINAL.POINTAmount 
		,FINAL.SALESAmount 
		,FINAL.RowType
		,FINAL.PointPayment
		,FINAL.SalesPayment
		,FINAL.PointPayment - (FINAL.GSTAmount * (CASE WHEN FINAL.Amount = 0 THEN 0 ELSE FINAL.PointPayment/FINAL.Amount END)) PointPaymentBefGST
		,FINAL.SalesPayment - (FINAL.GSTAmount * (CASE WHEN FINAL.Amount = 0 THEN 0 ELSE FINAL.SalesPayment/FINAL.Amount END)) SalesPaymentbefGST
		,FINAL.Attributes1
FROM (
	SELECT   TAB.*
			,CASE WHEN TAB.PointRedeemable = ''T'' THEN TAB.POINTAmount
				  ELSE ROUND((TAB.PointAmount * TAB.POINTReceipt )/ CASE WHEN ISNULL(TAB.TotalPOINTAmount,0) = 0 THEN 1 ELSE ISNULL(TAB.TotalPOINTAmount,0) END ,2) END PointPayment
			, CASE WHEN TAB.PointRedeemable = ''T'' THEN 0 ELSE 
			TAB.SALESAmount + (TotalSALESAmountLeft * TAB.POINTAmount / CASE WHEN TAB.TotalPointAmount = 0 THEN 1 ELSE TAB.TotalPointAmount END ) END SalesPayment
	FROM	(
		SELECT   TD.*
				,ISNULL(TOTAL.TotalPOINTAmount,0) TotalPOINTAmount
				,ISNULL(TOTAL.TotalSALESAmount,0) TotalSALESAmount
				,(TD.SALESReceipt - ISNULL(TOTAL.TotalSALESAmount,0)) TotalSALESAmountLeft
				,CASE WHEN TD.PointRedeemable = ''T'' THEN 0
					  WHEN ISNULL(TOTAL.TotalPOINTAmount,0) = 0 THEN 0
					  ELSE TD.POINTAmount/ISNULL(TOTAL.TotalPOINTAmount,0) END PointRate
		FROM	@Temp TD
				LEFT JOIN (
					SELECT   TD.OrderHdrID
							,ISNULL(SUM(TD.POINTAmount),0) TotalPOINTAmount
							,ISNULL(SUM(TD.SALESAmount),0) TotalSALESAmount
					FROM	@Temp TD
					WHERE	TD.PointRedeemable <> ''T''
					GROUP BY TD.OrderHdrID
				) TOTAL ON TOTAL.OrderHdrID = TD.OrderHdrID
	) TAB
) FINAL
WHERE	1 = 1
		AND (@SalesPersonID = '''' OR @SalesPersonID = ''ALL'' OR FINAL.SalesPersonID = @SalesPersonID)
		AND (@CategoryName = '''' OR @CategoryName = ''ALL'' OR FINAL.CategoryName = @CategoryName)
		AND (@ItemDepartmentID = '''' OR @ItemDepartmentID = ''ALL'' OR FINAL.ItemDepartmentID = @ItemDepartmentID)
		AND (FINAL.ItemNo LIKE ''%''+@Search+''%''
				OR FINAL.Barcode LIKE ''%''+@Search+''%''
				OR FINAL.ItemName LIKE ''%''+@Search+''%'')

    RETURN

END
' 
END
