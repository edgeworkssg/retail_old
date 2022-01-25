IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportByDateWithCOG]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportByDateWithCOG]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportByDateWithCOG]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportByDateWithCOG]
 @ItemDepartmentID NVARCHAR(500),
 @Category NVARCHAR(500),
 @Supplier NVARCHAR(500),
 @LocationID AS INT,
 @StartDate AS DATETIME,
 @Search NVARCHAR(MAX),
 @IsConsignment Nvarchar(20),
 @COGType NVARCHAR(100) 
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @RealItemDepartmentID NVARCHAR(500),
	 @RealCategory NVARCHAR(500),
	 @RealSupplier NVARCHAR(500),
	 @RealLocationID AS INT,
	 @RealStartDate AS DATETIME,
	 @RealSearch NVARCHAR(MAX),
	 @RealIsConsignment Nvarchar(20),
	 @RealCOGType NVARCHAR(100) 
	
	SET @RealItemDepartmentID = @ItemDepartmentID
	SET @RealCategory = @Category
	SET @RealSupplier = @Supplier
	SET @RealLocationID = @LocationID
	SET @RealStartDate =DATEADD(dd,1,@StartDate)
	SET @RealSearch = @Search
	SET @RealIsConsignment = @IsConsignment
	SET @RealCOGType = @COGType

SELECT  ID.DepartmentName
		,I.ItemNo
		,C.CategoryName
		,I.ItemName
		,I.RetailPrice
		, ISNULL(Quantity,0) as Quantity
		, Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 
		, 0 as UndeductedSales 
		, CASE WHEN @RealLocationID = 0 THEN ISNULL(I.AvgCostPrice ,0)
			WHEN ISNULL(INV.ItemNo,'''') = '''' THEN ISNULL(I.FactoryPrice,0)
			WHEN @RealCOGType = N''Inventory Location Cost'' THEN ISNULL(CGI.CostPriceByItemInvLoc,0)
			WHEN @RealCOGType = N''Inventory Location Group Cost'' THEN ISNULL(CGI.CostPriceByItemInvGroup,0)
			ELSE ISNULL(CGI.CostPriceByItem,0) 
				END as COG              
		, ISNULL(Quantity,0) *  
			CASE WHEN @RealLocationID = 0 THEN ISNULL(I.AvgCostPrice ,0)
				WHEN ISNULL(INV.ItemNo,'''') = '''' THEN ISNULL(I.FactoryPrice,0)
				WHEN @RealCOGType = N''Inventory Location Cost'' THEN ISNULL(CGI.CostPriceByItemInvLoc,0)
			    WHEN @RealCOGType = N''Inventory Location Group Cost'' THEN ISNULL(CGI.CostPriceByItemInvGroup,0)
				ELSE ISNULL(CGI.CostPriceByItem,0) 
				END as TotalCost 
FROM	Item I 
		INNER JOIN Category C ON C.CategoryName = I.CategoryName
		INNER JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentID
		LEFT JOIN (
			SELECT   ISM.ItemNo
					,ISM.SupplierID
					,SUPP.SupplierName
					,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
			FROM	ItemSupplierMap ISM
					INNER JOIN Supplier SUPP ON SUPP.SupplierID = ISM.SupplierID
			WHERE	ISNULL(ISM.Deleted,0) = 0
		) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
		LEFT JOIN (
    		SELECT ItemNo, SUM(CASE WHEN movementtype LIKE ''%In'' THEN quantity ELSE -quantity END) as Quantity 				
    		FROM InventoryHdr IH 
				INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
    			INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID 
			WHERE (@RealLocationID = 0 OR IH.InventoryLocationID = @RealLocationID)
					AND Ih.InventoryDate < @RealStartDate 
			GROUP BY ItemNo 
		) INV ON INV.ItemNo = I.ItemNo
		LEFT JOIN  
		( 
			SELECT ItemNo, CostPriceByItem, CostPriceByItemInvGroup, CostPriceByItemInvLoc FROM 
			(
				SELECT  ID.ItemNo, CASE WHEN @RealLocationID = 0 then IT.FactoryPrice else ISNULL(ID.CostPriceByItem,IT.FactoryPrice) end CostPriceByItem, 
				ISNULL(ID.CostPriceByItemInvGroup ,IT.FactoryPrice) as CostPriceByItemInvGroup,
				ISNULL(ID.CostPriceByItemInvLoc,IT.FactoryPrice) CostPriceByItemInvLoc,
				row_number() over(partition by ID.ItemNo order by IH.InventoryDate desc) rn
				FROM InventoryHdr IH	
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
				INNER JOIN Item IT  WITH(NOLOCK) ON IT.ItemNo = ID.ItemNo
				WHERE IH.InventoryLocationID = @RealLocationID 
			) A where rn = 1
		) CGI on I.Itemno = CGI.ItemNo  
WHERE	ISNULL(I.Deleted,0) = 0
		AND ISNULL(I.IsInInventory,0) = 1
		AND ID.ItemDepartmentID <> ''SYSTEM''
		AND (@RealItemDepartmentID = ''ALL'' OR ID.ItemDepartmentID = @RealItemDepartmentID)
		AND (@RealCategory = ''ALL'' OR C.CategoryName = @RealCategory) 
		AND (@RealSupplier = ''ALL'' OR ISNULL(SUPP.SupplierName,'''') = @RealSupplier) 
		AND (@IsConsignment=''ALL'' OR isnull(i.userint4,0) = CASE WHEN @IsConsignment =''Yes'' then 1	else 0 end)  
		AND (I.ItemNo LIKE ''%''+@RealSearch+''%'' 
		OR I.ItemName LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.ItemDesc,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes1,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes2,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes3,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes4,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes5,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes6,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes7,'''') LIKE ''%''+@RealSearch+''%''
		OR ISNULL(I.Attributes8,'''') LIKE ''%''+@RealSearch+''%''
		OR ID.DepartmentName LIKE ''%''+@RealSearch+''%''
		OR C.CategoryName LIKE ''%''+@RealSearch+''%'')		 
END
' 
END

