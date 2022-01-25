IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_NewCommissionReportBracket]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_NewCommissionReportBracket]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_NewCommissionReportBracket]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_NewCommissionReportBracket]
	@FilterStartDate AS DATETIME,
	@FilterEndDate AS DATETIME,
	@FilterUserName AS NVARCHAR(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF OBJECT_ID(''tempdb..#TempOrderHdr'') IS NOT NULL
  DROP TABLE #TempOrderHdr

IF OBJECT_ID(''tempdb..#TempCommHdr'') IS NOT NULL
  DROP TABLE #TempCommHdr

IF OBJECT_ID(''tempdb..#TempComm'') IS NOT NULL
  DROP TABLE #TempComm

SELECT   UM.UserName UserName
		,UM.GroupName SalesGroupID
		,I.CategoryName
		,I.ItemNo
		,I.ItemName
		,(CASE WHEN I.CategoryName = ''SYSTEM'' THEN ''SYSTEM''  
               WHEN I.Userfld10 = ''D'' THEN ''POINT_SOLD''  
               WHEN I.Userfld10 = ''T'' AND ISNULL(OD.Userflag5,0) = 0 THEN ''PACKAGE_SOLD''  
               WHEN I.Userfld10 = ''T'' AND ISNULL(OD.Userflag5,0) = 1 THEN ''PACKAGE_REDEEM''  
               WHEN I.IsCommission = 0 THEN ''NON_COMMISSION''  
               WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0   THEN ''SERVICE''  
               WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1   THEN ''PRODUCT''  
               WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1   THEN ''PRODUCT''  
          END) AS ItemType
		,SUM(OD.Amount) Amount
		,SUM(OD.Quantity) Quantity
INTO	#TempOrderHdr
FROM	OrderHdr OH
		INNER JOIN SalesCommissionRecord SCR ON SCR.OrderHdrID = OH.OrderHdrID
		INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
		INNER JOIN Item I ON I.ItemNo = OD.ItemNo
        INNER JOIN UserMst UM on UM.UserName = ISNULL(NULLIF(OD.UserFld1, ''''), SCR.SalesPersonID)
WHERE	OH.IsVoided = 0
		AND OD.IsVoided =0 
		AND CAST(OH.OrderDate AS DATE) >= @FilterStartDate AND CAST(OH.OrderDate AS DATE) <= @FilterEndDate
		AND (@FilterUserName = '''' OR @FilterUserName = ''ALL'' OR UM.UserName = @FilterUserName)
GROUP BY UM.UserName
		,UM.GroupName
		,I.CategoryName
		,I.ItemNo
		,I.ItemName
		,(CASE WHEN I.CategoryName = ''SYSTEM'' THEN ''SYSTEM''  
               WHEN I.Userfld10 = ''D'' THEN ''POINT_SOLD''  
               WHEN I.Userfld10 = ''T'' AND ISNULL(OD.Userflag5,0) = 0 THEN ''PACKAGE_SOLD''  
               WHEN I.Userfld10 = ''T'' AND ISNULL(OD.Userflag5,0) = 1 THEN ''PACKAGE_REDEEM''  
               WHEN I.IsCommission = 0 THEN ''NON_COMMISSION''  
               WHEN I.IsServiceItem = 1 AND I.IsInInventory = 0   THEN ''SERVICE''  
               WHEN I.IsServiceItem = 0 AND I.IsInInventory = 1   THEN ''PRODUCT''  
               WHEN I.IsServiceItem = 1 AND I.IsInInventory = 1   THEN ''PRODUCT''  
          END)
ORDER BY UM.UserName, I.CategoryName, I.ItemNo

SELECT   DENSE_RANK() OVER(ORDER BY CM.SalesGroupID, CM.CommissionType, ISNULL(CM.CategoryName,''''), ISNULL(CM.ItemNo,''''), CM.CommissionBasedOn) RowNo
		,ROW_NUMBER() OVER(PARTITION BY CM.SalesGroupID, CM.CommissionType, ISNULL(CM.CategoryName,''''), ISNULL(CM.ItemNo,''''), CM.CommissionBasedOn
						   ORDER BY CM.AmountTo, CM.QuantityTo) SubRowNo
		,CM.CommissionID
		,CM.SalesGroupID
		,CM.CommissionType
		,CM.CategoryName
		,ISNULL(CM.ItemNo,'''') ItemNo
		,ISNULL(I.ItemName,'''') ItemName
		,CM.CommissionBasedOn
		,CM.QuantityFrom
		,CM.QuantityTo
		,CM.AmountCommission
		,CM.AmountFrom
		,CM.AmountTo
		,CM.PercentageCommission
INTO	#TempComm
FROM	Commission CM
		LEFT JOIN Item I ON I.ItemNo = CM.ItemNo
ORDER BY CM.SalesGroupID
		,CM.CommissionType
		,CM.CategoryName
		,CM.ItemNo
		,CM.CommissionBasedOn
		,CM.AmountTo
		,CM.QuantityTo

SELECT   *
INTO	#TempCommHdr
FROM	#TempComm
WHERE	SubRowNo = 1
ORDER BY RowNo, SubRowNo

DECLARE @RowNo INT;
DECLARE @SubRowNo  INT;
DECLARE @CommID INT;
DECLARE @SalesGroupID INT;
DECLARE @CommType VARCHAR(50);
DECLARE @ItemNo NVARCHAR(500);
DECLARE @ItemName NVARCHAR(500);
DECLARE @CategoryName NVARCHAR(500);
DECLARE @CommBasedOn VARCHAR(50);
DECLARE @QtyFrom DECIMAL(20,5);
DECLARE @QtyTo DECIMAL(20,5);
DECLARE @AmountComm MONEY;
DECLARE @AmountFrom MONEY;
DECLARE @AmountTo MONEY;
DECLARE @PercentageComm DECIMAL(20,5);
DECLARE @TempResult AS TABLE 
(
	Sales NVARCHAR(50),
	ItemType NVARCHAR(50),
	Category NVARCHAR(50),
	ItemNo NVARCHAR(50),
	ItemName NVARCHAR(50),
	SalesTotalQuantity DECIMAL(20,5),
	SalesTotalAmount MONEY,
	CommissionRule NVARCHAR(500),
	TotalCommission MONEY
);

SET @RowNo = (SELECT TOP 1 RowNo FROM #TempCommHdr ORDER BY RowNo)
WHILE @RowNo IS NOT NULL BEGIN
	SELECT  TOP 1
			 @SalesGroupID = SalesGroupID
			,@CommType = CommissionType
			,@ItemNo = ItemNo
			,@ItemName = ItemName
			,@CategoryName = CategoryName
			,@CommBasedOn = CommissionBasedOn
	FROM	#TempCommHdr 
	WHERE	RowNo = @RowNo

	IF OBJECT_ID(''tempdb..#TempOrder'') IS NOT NULL
	  DROP TABLE #TempOrder

	SELECT   UserName
			,SUM(Amount) Amount
			,SUM(Amount) AmountCalculated
			,SUM(Quantity) Quantity
			,SUM(Quantity) QuantityCalculated
	INTO	#TempOrder
	FROM	#TempOrderHdr
	WHERE	SalesGroupID = @SalesGroupID
			AND ItemType = @CommType
			AND (ISNULL(@CategoryName,'''') = '''' OR CategoryName = ISNULL(@CategoryName,''''))
			AND (ISNULL(@ItemNo,'''') = '''' OR ItemNo = ISNULL(@ItemNo,''''))
	GROUP BY UserName

	SET @SubRowNo = (SELECT TOP 1 SubRowNo FROM #TempComm WHERE RowNo = @RowNo ORDER BY SubRowNo)
	WHILE @SubRowNo IS NOT NULL BEGIN
		SELECT  TOP 1
				 @QtyFrom = ISNULL(QuantityFrom,0)
				,@QtyTo = ISNULL(QuantityTo,0)
				,@AmountComm = ISNULL(AmountCommission,0)
				,@AmountFrom = ISNULL(AmountFrom,0)
				,@AmountTo = ISNULL(AmountTo,0)
				,@PercentageComm = ISNULL(PercentageCommission,0)
				,@CommID = CommissionID
		FROM	#TempComm 
		WHERE	RowNo = @RowNo AND SubRowNo = @SubRowNo

		INSERT INTO @TempResult
		SELECT   UserName
				,@CommType ItemType
				,@CategoryName CategoryType
				,@ItemNo ItemNo
				,@ItemName ItemName
				,Quantity SalesTotalQuantity
				,Amount SalesTotalAmount
				,CASE WHEN @CommBasedOn = ''Percentage'' THEN ''Amt:''+CAST(ROUND(@AmountFrom,2) AS VARCHAR(10))+''-''+CAST(ROUND(@AmountTo,2) AS VARCHAR(10))+''=''+CAST(FLOOR(@PercentageComm) AS VARCHAR(10))+''%''
				  WHEN @CommBasedOn = ''Quantity'' THEN ''Qty:''+CAST(FLOOR(@QtyFrom) AS VARCHAR(10))+''-''+CAST(FLOOR(@QtyTo) AS VARCHAR(10))+''=''+CAST(ROUND(@AmountComm,2) AS VARCHAR(10)) END CommissionRule
				,CASE WHEN @CommBasedOn = ''Percentage'' THEN
					 ((CASE WHEN AmountCalculated > @AmountTo THEN (@AmountTo-@AmountFrom) ELSE AmountCalculated END) * (@PercentageComm/100))
				      WHEN @CommBasedOn = ''Quantity'' THEN
					 ((CASE WHEN QuantityCalculated > @QtyTo THEN (@QtyTo-@QtyFrom) ELSE QuantityCalculated END) * @AmountComm)
			     END TotalCommission
		FROM	#TempOrder
		WHERE	(@CommBasedOn = ''Percentage'' AND Amount > @AmountFrom AND AmountCalculated > 0) OR 
				(@CommBasedOn = ''Quantity'' AND Quantity > @QtyFrom AND QuantityCalculated > 0)

		UPDATE	OH
		SET		OH.AmountCalculated = OH.AmountCalculated - (CASE WHEN AmountCalculated > @AmountTo THEN (@AmountTo-@AmountFrom) ELSE AmountCalculated END)
				,OH.QuantityCalculated = OH.QuantityCalculated - (CASE WHEN QuantityCalculated > @QtyTo THEN (@QtyTo-@QtyFrom)  ELSE QuantityCalculated END) 
		FROM	#TempOrder OH
		WHERE	(@CommBasedOn = ''Percentage'' AND OH.Amount > @AmountFrom) OR 
				(@CommBasedOn = ''Quantity'' AND OH.Quantity > @QtyFrom)
		
		DELETE #TempComm WHERE RowNo = @RowNo AND SubRowNo = @SubRowNo
		SET @SubRowNo = (SELECT TOP 1 SubRowNo FROM #TempComm WHERE RowNo = @RowNo ORDER BY SubRowNo)
	END

	DELETE #TempCommHdr WHERE RowNo = @RowNo
	SET @RowNo = (SELECT TOP 1 RowNo FROM #TempCommHdr ORDER BY RowNo)
END

SELECT   ISNULL(UM.DisplayName,TR.Sales) Sales
		,TR.ItemType
		,TR.Category
		,TR.ItemNo
		,TR.ItemName
		,TR.SalesTotalQuantity
		,TR.SalesTotalAmount
		,TR.CommissionRule
		,TR.TotalCommission
FROM	@TempResult TR
		LEFT JOIN UserMst UM ON UM.UserName = TR.Sales
ORDER BY TR.Sales
		,TR.ItemType
		,TR.Category
		,TR.ItemNo
		,TR.ItemName
		,TR.CommissionRule
END' 
 
END