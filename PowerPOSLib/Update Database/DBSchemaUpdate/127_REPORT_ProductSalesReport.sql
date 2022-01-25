IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_ProductSalesReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_ProductSalesReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_ProductSalesReport]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE REPORT_ProductSalesReport
	@CategoryName NVARCHAR(500),
	@StartDate DATETIME,
	@EndDate DATETIME,
	@OutletName NVARCHAR(500),
	@Search NVARCHAR(MAX),
	@SupplierID INT,
	@ItemDepartmentID NVARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;

IF OBJECT_ID(''tempdb..#TempDW_HourlyProductSales'') IS NOT NULL
	DROP TABLE #TempDW_HourlyProductSales
	
	declare @NearStartDate datetime 
	declare @NearEndDate datetime 
	declare @diff int 
	set @diff = datediff(second, @StartDate, @EndDate)

	if @diff <= 86400	
	BEGIN
		SELECT   ID.DepartmentName
                ,C.CategoryName
		        ,I.ItemNo
		        ,I.ItemName
                ,I.Barcode 
		        ,CAST(SUM(OD.Quantity) as Decimal(18,2)) TotalQuantity
		        ,SUM(OD.Amount) TotalAmount
		        ,I.Attributes1
		        ,I.Attributes2
		        ,I.Attributes3
		        ,I.Attributes4
		        ,I.Attributes5
		        ,I.Attributes6
		        ,I.Attributes7
                ,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END OutletName  
				,SUPP.SupplierName      
        FROM	OrderHdr OH
                LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
                LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
                LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
                LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
                LEFT JOIN Category C ON C.CategoryName = I.CategoryName
                LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
				LEFT JOIN (
					SELECT  ISM.ItemNo
							,ISM.SupplierID
							,SP.SupplierName
							,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
					FROM	ItemSupplierMap ISM
					LEFT JOIN Supplier SP on ISM.SupplierID = SP.SupplierID
					WHERE	ISNULL(ISM.Deleted,0) = 0
				) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
        WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0 AND OD.ItemNo NOT IN (''R0001'')
                AND OH.OrderDate BETWEEN @StartDate AND @EndDate
                AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = ''ALL'' OR @ItemDepartmentID = '''')
				AND (C.CategoryName = @CategoryName OR @CategoryName = ''ALL'' OR @CategoryName = '''') 
				AND (@SupplierID = 0 OR ISNULL(SUPP.SupplierID,0) = @SupplierID)
				AND (@OutletName = ''ALL'' OR @OutletName = ''ALL - BreakDown'' OR  OU.OutletName = @OutletName)
				AND (I.ItemNo LIKE ''%''+@Search+''%''
				OR I.ItemName LIKE ''%''+@Search+''%''		
				OR ISNULL(I.Attributes1,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes2,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes3,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes4,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes5,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes6,'''') LIKE ''%''+@Search+''%''
				OR ISNULL(I.Attributes7,'''') LIKE ''%''+@Search+''%'')
        GROUP BY  ID.DepartmentName
                ,C.CategoryName
		        ,I.ItemNo
		        ,I.ItemName	  
                ,I.Barcode  		
		        ,I.Attributes1
		        ,I.Attributes2
		        ,I.Attributes3
		        ,I.Attributes4
		        ,I.Attributes5
		        ,I.Attributes6
		        ,I.Attributes7
                ,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END   
				,SUPP.SupplierName 
	END
	ELSE
	BEGIN
		
		select @NearStartDate = dateadd(day,1, dateadd(day, datediff(day, 0, @StartDate), 0))
		select @NearEndDate = dateadd(second, -1, dateadd(day, datediff(day, 0, @EndDate), 0))

		SELECT * 
		INTO	#TempDW_HourlyProductSales
		FROM	dbo.fnGetDWHourlyProductSales(@NearStartDate,@NearEndDate,@OutletName,'''',0)
		
		SELECT 
				DepartmentName
				,CategoryName
				,ItemNo
				,ItemName
				,Barcode 
				,SUM(TotalQuantity) TotalQuantity
				,SUM(TotalAmount) TotalAmount
				,Attributes1
				,Attributes2
				,Attributes3
				,Attributes4
				,Attributes5
				,Attributes6
				,Attributes7
				,OutletName  
				,SupplierName 
		FROM
		(
		
			SELECT   ID.DepartmentName
					,C.CategoryName
					,I.ItemNo
					,I.ItemName
					,I.Barcode 
					,CAST(SUM(OD.Quantity) as Decimal(18,2)) TotalQuantity
					,SUM(OD.Amount) TotalAmount
					,I.Attributes1
					,I.Attributes2
					,I.Attributes3
					,I.Attributes4
					,I.Attributes5
					,I.Attributes6
					,I.Attributes7
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END OutletName  
					,SUPP.SupplierName      
			FROM	OrderHdr OH
					LEFT JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
					LEFT JOIN Outlet OU ON OU.OutletName = POS.OutletName
					LEFT JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID
					LEFT JOIN Item I ON I.ItemNo = OD.ItemNo
					LEFT JOIN Category C ON C.CategoryName = I.CategoryName
					LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
					LEFT JOIN (
						SELECT  ISM.ItemNo
								,ISM.SupplierID
								,SP.SupplierName
								,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
						FROM	ItemSupplierMap ISM
						LEFT JOIN Supplier SP on ISM.SupplierID = SP.SupplierID
						WHERE	ISNULL(ISM.Deleted,0) = 0
					) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
			WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0  AND OD.ItemNo NOT IN (''R0001'')
					AND ((OH.OrderDate BETWEEN @StartDate AND @NearStartDate) OR (OH.OrderDate BETWEEN @NearEndDate AND @EndDate))
					AND (ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = ''ALL'' OR @ItemDepartmentID = '''')
					AND (C.CategoryName = @CategoryName OR @CategoryName = ''ALL'' OR @CategoryName = '''') 
					AND (@SupplierID = 0 OR ISNULL(SUPP.SupplierID,0) = @SupplierID)
					AND (@OutletName = ''ALL'' OR @OutletName = ''ALL - BreakDown'' OR  OU.OutletName = @OutletName)
					AND (I.ItemNo LIKE ''%''+@Search+''%''
					OR I.ItemName LIKE ''%''+@Search+''%''		
					OR ISNULL(I.Attributes1,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes2,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes3,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes4,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes5,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes6,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes7,'''') LIKE ''%''+@Search+''%'')
			GROUP BY  ID.DepartmentName
					,C.CategoryName
					,I.ItemNo
					,I.ItemName	  
					,I.Barcode  		
					,I.Attributes1
					,I.Attributes2
					,I.Attributes3
					,I.Attributes4
					,I.Attributes5
					,I.Attributes6
					,I.Attributes7
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END   
					,SUPP.SupplierName 
			UNION ALL
			SELECT   ID.DepartmentName
					,C.CategoryName
					,I.ItemNo
					,I.ItemName
					,I.Barcode 
					,CAST(SUM(OH.Quantity) as Decimal(18,2)) TotalQuantity
					,SUM(OH.Amount) TotalAmount
					,I.Attributes1
					,I.Attributes2
					,I.Attributes3
					,I.Attributes4
					,I.Attributes5
					,I.Attributes6
					,I.Attributes7
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE OU.OutletName END OutletName  
					,SUPP.SupplierName      
			FROM	#TempDW_HourlyProductSales OH
					LEFT JOIN Outlet OU ON OU.OutletName = OH.OutletName
					LEFT JOIN Item I ON I.ItemNo = OH.ItemNo
					LEFT JOIN Category C ON C.CategoryName = I.CategoryName
					LEFT JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentId
					LEFT JOIN (
						SELECT  ISM.ItemNo
								,ISM.SupplierID
								,SP.SupplierName
								,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
						FROM	ItemSupplierMap ISM
						LEFT JOIN Supplier SP on ISM.SupplierID = SP.SupplierID
						WHERE	ISNULL(ISM.Deleted,0) = 0
					) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
			WHERE	(ID.ItemDepartmentID = @ItemDepartmentID OR @ItemDepartmentID = ''ALL'' OR @ItemDepartmentID = '''')
					AND (C.CategoryName = @CategoryName OR @CategoryName = ''ALL'' OR @CategoryName = '''') 
					AND (@SupplierID = 0 OR ISNULL(SUPP.SupplierID,0) = @SupplierID)
					AND (I.ItemNo LIKE ''%''+@Search+''%''
					OR I.ItemName LIKE ''%''+@Search+''%''		
					OR ISNULL(I.Attributes1,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes2,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes3,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes4,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes5,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes6,'''') LIKE ''%''+@Search+''%''
					OR ISNULL(I.Attributes7,'''') LIKE ''%''+@Search+''%'')
			GROUP BY  ID.DepartmentName
					,C.CategoryName
					,I.ItemNo
					,I.ItemName	  
					,I.Barcode  		
					,I.Attributes1
					,I.Attributes2
					,I.Attributes3
					,I.Attributes4
					,I.Attributes5
					,I.Attributes6
					,I.Attributes7
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE OU.OutletName END 
					,SUPP.SupplierName  
		)X 
		Group By DepartmentName
                ,CategoryName
		        ,ItemNo
		        ,ItemName
                ,Barcode 
		        ,Attributes1
		        ,Attributes2
		        ,Attributes3
		        ,Attributes4
		        ,Attributes5
		        ,Attributes6
		        ,Attributes7
                ,OutletName  
				,SupplierName 
	END  

END
' 
END

