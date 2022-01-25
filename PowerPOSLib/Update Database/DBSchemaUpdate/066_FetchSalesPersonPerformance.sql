IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchSalesPersonPerformance]') AND type in (N'P', N'PC'))
	Drop Procedure [dbo].[FetchSalesPersonPerformance]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchSalesPersonPerformance]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchSalesPersonPerformance]
	@startDate AS DATETIME, 
	@endDate AS DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
--declare @startDate AS datetime;  
--declare @endDate AS datetime;  
 
--SET @startDate = ''2015-05-22 00:00:00''; 
--SET @endDate = ''2015-05-23 23:59:00''; 
 
SELECT UserName  
    , ISNULL(ServiceSalesAmount,0) AS [Service]  
    , ISNULL(ProductSalesAmount,0) AS [Product]  
    , ISNULL(OpenProductSalesAmount,0) AS [OpenPriceProduct]  
    , ISNULL(SystemItemAmount,0) AS [SystemItem]  
    , ISNULL(NonCommissionableAmount,0) AS [NonCommission]  
    , ISNULL(LineAmount, 0) AS [TotalProductAndService]  
    , ISNULL(PointSoldAmount,0) AS [PointSold]  
    , ISNULL(PointSoldValue,0) AS [PointSoldValue]  
    , ISNULL(TotalPointRedeem,0) AS [PointRedeem]
    , ISNULL(PackageSoldAmount,0) AS [PackageSold]  
    , ISNULL(PackageRedeemQty,0) AS [PackageRedeemed]  
    , ISNULL(PackageRedeemAmount,0) AS [PackageRedeemedValue]  
    , ISNULL(TotalPointAndPackage,0) AS [TotalPointAndPackage]  
    , ISNULL(LineAmount,0) + ISNULL(TotalPointAndPackage,0) AS [TotalAmount]  
    , ISNULL(TotalOrderAmount,0) AS TotalOrderAmount  
    , ISNULL(TotalPointAndPackage_INSTALLMENT,0) AS [TotalPointAndPackage_INST]
    , ISNULL(LineAmount_INSTALLMENT,0) AS [TotalProductAndService_INST]    
FROM  
(  
    SELECT	UserName  
    FROM	UserMst  
    WHERE	IsASalesPerson = 1  
			AND Deleted = 0  
) UM  
LEFT OUTER JOIN  
(  
    SELECT	 UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) LineAmount
    FROM	OrderDet OD
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			--AND I.IsCommission = 1 
			AND OH.OrderDate >= @startDate  
			AND OH.OrderDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') NOT IN (''D'',''T'')
    GROUP BY UM1.UserName
) X ON X.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
	SELECT SalesPersonID, SUM(CurrentBalance) AS LineAmount_INSTALLMENT
	FROM (
		SELECT	  DISTINCT SCR.SalesPersonID AS SalesPersonID  
				,INS.CurrentBalance, OH.OrderHdrID   
		FROM	OrderDet OD
				INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
				LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
				INNER JOIN Item I ON I.ItemNo = OD.ItemNo  
				INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
				INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
				LEFT OUTER JOIN Installment INS ON INS.OrderHdrID = OH.OrderHdrID
		WHERE	OD.IsVoided = 0  
				AND OH.IsVoided = 0  
				AND OH.OrderDate >= @startDate  
				AND OH.OrderDate <= @endDate  
				AND ISNULL(I.Userfld10,'''') NOT IN (''D'',''T'')
				AND ISNULL(RD.PaymentType,'''') = ''INSTALLMENT''
	 ) C 
	 GROUP BY SalesPersonID
) X_INSTALLMENT ON X_INSTALLMENT.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) ServiceSalesAmount			
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND I.IsCommission = 1  
			AND I.CategoryName <> ''SYSTEM''  
			AND I.IsServiceItem = 1  
			AND I.IsInInventory = 0  
			AND ISNULL(I.Userfld10,'''') NOT IN (''D'',''T'')
    GROUP BY UM1.UserName
) P ON P.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  		
    SELECT	UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) ProductSalesAmount			
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND I.IsCommission = 1  
			AND I.CategoryName <> ''SYSTEM''  
			AND I.IsInInventory = 1  
			AND I.IsServiceItem = 0  
			AND ISNULL(I.Userfld10,'''') NOT IN (''D'',''T'')
    GROUP BY UM1.UserName			
			
) Q ON Q.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) OpenProductSalesAmount			
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND I.IsCommission = 1  
			AND I.CategoryName <> ''SYSTEM''  
			AND I.IsInInventory = 1  
			AND I.IsServiceItem = 1   
			AND ISNULL(I.Userfld10,'''') NOT IN (''D'',''T'')
    GROUP BY UM1.UserName						
) OP ON OP.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  			
    SELECT	UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) SystemItemAmount			
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND I.IsCommission = 1  
			AND I.CategoryName = ''SYSTEM''  
    GROUP BY UM1.UserName				
) SYS ON SYS.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) NonCommissionableAmount			
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND I.IsCommission = 0  
			AND I.CategoryName <> ''SYSTEM''  
			AND ISNULL(I.Userfld10,'''') NOT IN (''D'',''T'')
    GROUP BY UM1.UserName							
			
) NC ON NC.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	 UM1.UserName AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) AS PointSoldAmount  
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided=0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') = ''D''  
			AND I.CategoryName <> ''SYSTEM''  
    GROUP BY UM1.UserName
) R ON R.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	 UM1.UserName AS SalesPersonID 
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN PAL.PointAllocated/2
				  ELSE PAL.PointAllocated END) AS PointSoldValue  
    FROM	PointAllocationLog PAL 
			INNER JOIN OrderHdr OH ON PAL.OrderHdrID = OH.OrderHdrID  
			INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID AND PAL.userfld1 LIKE OD.ItemNo+''%''
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OH.IsVoided = 0  
			AND OD.IsVoided = 0
			AND OrderDate >= @startDate  
			AND OrderDate <= @endDate  
			AND SCR.userfld10 = ''D''  
			AND PAL.PointAllocated > 0  
    GROUP BY UM1.UserName
) S ON S.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	 SCR.SalesPersonID  AS SalesPersonID  
			,SUM(RD.Amount) AS TotalPointRedeem  
    FROM	OrderHdr OH 
			INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID  
			INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
    WHERE	OH.IsVoided = 0  
			AND OH.OrderDate >= @startDate  
			AND OH.OrderDate <= @endDate  
			AND ISNULL(RD.PaymentType,'''') = ''POINTS''
    GROUP BY SCR.SalesPersonID
) PR  ON PR.SalesPersonID = UM.UserName
LEFT OUTER JOIN  
(  
    SELECT	 UM1.UserName AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) AS PackageSoldAmount  
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided=0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') = ''T''  
			AND I.CategoryName <> ''SYSTEM''  
    GROUP BY UM1.UserName
) T  
ON T.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	 ISNULL(NULLIF(OD.userfld1,''''), SCR.SalesPersonID)  AS SalesPersonID  
			,SUM(OD.Quantity) AS PackageRedeemQty  
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OD.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
    WHERE	OD.IsVoided=0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') = ''T''  
			--AND OD.giveCommission = 1  
			AND OD.UnitPrice = 0  
    GROUP BY ISNULL(NULLIF(OD.userfld1,'''')
			,SCR.SalesPersonID)  
) U  
ON U.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	ISNULL(NULLIF(OD.userfld1,''''), SCR.SalesPersonID)  AS SalesPersonID  
			,SUM(OD.Userfloat8) AS PackageRedeemAmount  
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OD.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
    WHERE	OD.IsVoided=0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') = ''T''  
			--AND OD.giveCommission = 1  
			AND OD.UnitPrice = 0  
    GROUP BY ISNULL(NULLIF(OD.userfld1,'''')
			,SCR.SalesPersonID)  
) V  
ON V.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	 UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) AS TotalPointAndPackage  
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided=0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') IN (''D'',''T'')  
    GROUP BY UM1.UserName
) W ON W.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	 ISNULL(NULLIF(OD.userfld1,''''), SCR.SalesPersonID)  AS SalesPersonID  
			,SUM(RD.Amount) AS TotalPointAndPackage_INSTALLMENT
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OD.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN ReceiptHdr RH ON RH.OrderHdrID = OH.OrderHdrID
			INNER JOIN ReceiptDet RD ON RD.ReceiptHdrID = RH.ReceiptHdrID			
    WHERE	OD.IsVoided=0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
			AND ISNULL(I.Userfld10,'''') IN (''D'',''T'')  
			AND ISNULL(RD.PaymentType,'''') = ''INSTALLMENT''
    GROUP BY ISNULL(NULLIF(OD.userfld1,'''')
			,SCR.SalesPersonID)  
) W_INSTALLMENT ON W_INSTALLMENT.SalesPersonID = UM.UserName  
LEFT OUTER JOIN  
(  
    SELECT	UM1.UserName  AS SalesPersonID  
			,SUM(CASE WHEN ISNULL(OD.userfld1,'''')<>'''' AND ISNULL(OD.userfld20,'''')<>'''' AND ISNULL(OD.userfld1,'''')<>ISNULL(OD.userfld20,'''')  
				  THEN OD.Amount/2
				  ELSE OD.Amount END) AS TotalOrderAmount  
    FROM	OrderDet OD 
			INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID  
			LEFT OUTER JOIN SalesCommissionRecord SCR ON OH.OrderHdrID = SCR.OrderHdrID  
			INNER JOIN Item I ON OD.ItemNo = I.ItemNo  
			INNER JOIN UserMst UM1 ON UM1.UserName = ISNULL(NULLIF(OD.userfld1,''''),SCR.SalesPersonID)
								      OR (UM1.UserName = OD.userfld20 AND ISNULL(OD.userfld20,'''') <> '''')
    WHERE	OD.IsVoided = 0  
			AND OH.IsVoided = 0  
			AND OrderDate >= @startDate  
			AND OrderDetDate <= @endDate  
    GROUP BY UM1.UserName
) Y  ON Y.SalesPersonID = UM.UserName	
END'
END


