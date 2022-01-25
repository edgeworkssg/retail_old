IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_CategorySalesReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_CategorySalesReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_CategorySalesReport]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_CategorySalesReport]
 @CategoryName NVARCHAR(MAX),
 @StartDate DATETIME,
 @EndDate DATETIME,
 @OutletName VARCHAR(50),
 @DeptID VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

SET @OutletName = LTRIM(RTRIM(@OutletName)); 

	declare @NearStartDate datetime 
	declare @NearEndDate datetime 
	declare @diff int 
	set @diff = datediff(second, @StartDate, @EndDate)

	if @diff <= 86400	
	BEGIN
		SELECT   ID.DepartmentName
				,C.CategoryName
				,CAST(SUM(OD.Quantity) AS MONEY) TotalQuantity
				,CAST(SUM(OD.Amount) AS MONEY) TotalAmount
				,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END OutletName     
        FROM	OrderHdr OH
                LEFT JOIN PointOfSale POS WITH(NOLOCK) ON POS.PointOfSaleID = OH.PointOfSaleID                
                LEFT JOIN OrderDet OD WITH(NOLOCK) ON OD.OrderHdrID = OH.OrderHdrID
                LEFT JOIN Item I WITH(NOLOCK) ON I.ItemNo = OD.ItemNo
                LEFT JOIN Category C WITH(NOLOCK) ON C.CategoryName = I.CategoryName
                LEFT JOIN ItemDepartment ID WITH(NOLOCK) ON ID.ItemDepartmentID = C.ItemDepartmentId				
        WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0 AND OD.ItemNo NOT IN (''R0001'')
                AND OH.OrderDate BETWEEN @StartDate AND @EndDate
                AND (ID.ItemDepartmentID = @DeptID OR @DeptID = ''ALL'' OR @DeptID = '''')
				AND C.CategoryName IN ( SELECT DATA FROM dbo.fnSplitRow(@CategoryName,'':'') ) 
				AND (@OutletName = ''ALL'' OR @OutletName = ''ALL - BreakDown'' OR  POS.OutletName = @OutletName)				
        GROUP BY  ID.DepartmentName
                ,C.CategoryName		        
                ,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END 
	END
	ELSE
	BEGIN
		select @NearStartDate = dateadd(day,1, dateadd(day, datediff(day, 0, @StartDate), 0))
		select @NearEndDate = dateadd(second, -1, dateadd(day, datediff(day, 0, @EndDate), 0))

		SELECT ItemNo
	    ,SUM(Quantity) as Quantity
		,SUM(Amount) as Amount
		,OutletName
		INTO	#TempDW_HourlyProductSales
		FROM	dbo.fnGetDWHourlyProductSales(@NearStartDate,@NearEndDate,@OutletName,'''',0)
		Group By ItemNo, OutletName
	
	
		SELECT 
				DepartmentName
				,CategoryName
				,SUM(TotalQuantity) TotalQuantity
				,SUM(TotalAmount) TotalAmount	
				,OutletName			
		FROM
		(
		
			SELECT   ID.DepartmentName
					,C.CategoryName
					,CAST(SUM(OD.Quantity) AS MONEY) TotalQuantity
					,CAST(SUM(OD.Amount) AS MONEY) TotalAmount
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END OutletName   
			FROM	OrderHdr OH
					LEFT JOIN PointOfSale POS WITH(NOLOCK) ON POS.PointOfSaleID = OH.PointOfSaleID					
					LEFT JOIN OrderDet OD WITH(NOLOCK) ON OD.OrderHdrID = OH.OrderHdrID
					LEFT JOIN Item I WITH(NOLOCK) ON I.ItemNo = OD.ItemNo
					LEFT JOIN Category C WITH(NOLOCK) ON C.CategoryName = I.CategoryName
					LEFT JOIN ItemDepartment ID WITH(NOLOCK) ON ID.ItemDepartmentID = C.ItemDepartmentId					
			WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0  AND OD.ItemNo NOT IN (''R0001'')
					AND ((OH.OrderDate BETWEEN @StartDate AND @NearStartDate) OR (OH.OrderDate BETWEEN @NearEndDate AND @EndDate))
					AND (ID.ItemDepartmentID = @DeptID OR @DeptID = ''ALL'' OR @DeptID = '''')
					AND C.CategoryName IN ( SELECT DATA FROM dbo.fnSplitRow(@CategoryName,'':'') ) 					
					AND (@OutletName = ''ALL'' OR @OutletName = ''ALL - BreakDown'' OR  POS.OutletName = @OutletName)					
			GROUP BY  ID.DepartmentName
					,C.CategoryName
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE POS.OutletName END   
			UNION ALL
			SELECT   ID.DepartmentName
					,C.CategoryName
					,CAST(SUM(OH.Quantity) AS MONEY) TotalQuantity
					,CAST(SUM(OH.Amount) AS MONEY) TotalAmount
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE OH.OutletName END OutletName 
			FROM	#TempDW_HourlyProductSales OH
					LEFT JOIN Item I WITH(NOLOCK) ON I.ItemNo = OH.ItemNo
					LEFT JOIN Category C WITH(NOLOCK) ON C.CategoryName = I.CategoryName
					LEFT JOIN ItemDepartment ID WITH(NOLOCK) ON ID.ItemDepartmentID = C.ItemDepartmentId
			WHERE	(ID.ItemDepartmentID = @DeptID OR @DeptID = ''ALL'' OR @DeptID = '''')
					AND C.CategoryName IN ( SELECT DATA FROM dbo.fnSplitRow(@CategoryName,'':'') )
			GROUP BY ID.DepartmentName
					,C.CategoryName					
					,CASE WHEN @OutletName = ''ALL'' THEN ''ALL'' ELSE OH.OutletName END 					
		)X 
		Group By DepartmentName
                ,CategoryName	
				,OutletName	        
	
	END

END
' 
END