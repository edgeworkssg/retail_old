IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportByDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportByDate]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportByDate]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportByDate]
 @ItemDepartmentID NVARCHAR(500),
 @Category NVARCHAR(500),
 @Supplier NVARCHAR(500),
 @LocationID AS INT,
 @StartDate AS DATETIME,
 @Search NVARCHAR(MAX),
 @IsConsignment Nvarchar(20) 
AS
BEGIN
	SET NOCOUNT ON;
	SET @StartDate =DATEADD(dd,1,@StartDate)

SELECT  ID.DepartmentName
		,I.ItemNo
		,C.CategoryName
		,I.ItemName
		,I.RetailPrice
		, ISNULL(Quantity,0) as Quantity
		, Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 
		, 0 as UndeductedSales 
		, CASE WHEN @LocationID = 0 THEN I.AvgCostPrice WHEN ISNULL(INV.ItemNo,'''') = '''' THEN I.FactoryPrice ELSE ISNULL(CGI.CostPriceByItem,0) END as COG              
		, ISNULL(Quantity,0) *  CASE WHEN @LocationID = 0 THEN I.AvgCostPrice WHEN ISNULL(INV.ItemNo,'''') = '''' THEN I.FactoryPrice ELSE ISNULL(CGI.CostPriceByItem,0) END as TotalCost 
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
			WHERE (@LocationID = 0 OR IH.InventoryLocationID = @LocationID)
					AND Ih.InventoryDate < @StartDate 
			GROUP BY ItemNo 
		) INV ON INV.ItemNo = I.ItemNo
		LEFT JOIN  
		( 
			SELECT ItemNo, CostPriceByItem, CostPriceByItemInvGroup, CostPriceByItemInvLoc FROM 
			(
				SELECT  ID.ItemNo, CASE WHEN @LocationID = 0 then IT.FactoryPrice else ISNULL(ID.CostPriceByItem,IT.FactoryPrice) end CostPriceByItem, 
				ISNULL(ID.CostPriceByItemInvGroup ,IT.FactoryPrice) as CostPriceByItemInvGroup,
				ISNULL(ID.CostPriceByItemInvLoc,IT.FactoryPrice) CostPriceByItemInvLoc,
				row_number() over(partition by ID.ItemNo order by IH.InventoryDate desc) rn
				FROM InventoryHdr IH 
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
				INNER JOIN Item IT  WITH(NOLOCK) ON IT.ItemNo = ID.ItemNo
				WHERE IH.InventoryLocationID = @LocationID 
			) A where rn = 1
		) CGI on I.Itemno = CGI.ItemNo  
WHERE	ISNULL(I.Deleted,0) = 0
		AND ISNULL(I.IsInInventory,0) = 1
		AND ID.ItemDepartmentID <> ''SYSTEM''
		AND (@ItemDepartmentID = ''ALL'' OR ID.ItemDepartmentID = @ItemDepartmentID)
		AND (@Category = ''ALL'' OR C.CategoryName = @Category) 
		AND (@Supplier = ''ALL'' OR ISNULL(SUPP.SupplierName,'''') = @Supplier) 
		AND (@IsConsignment=''ALL'' OR isnull(i.userint4,0) = CASE WHEN @IsConsignment =''Yes'' then 1	else 0 end)  
		AND (I.ItemNo LIKE ''%''+@Search+''%'' 
		OR I.ItemName LIKE ''%''+@Search+''%''
		OR ISNULL(I.ItemDesc,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes1,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes2,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes3,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes4,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes5,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes6,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes7,'''') LIKE ''%''+@Search+''%''
		OR ISNULL(I.Attributes8,'''') LIKE ''%''+@Search+''%''
		OR ID.DepartmentName LIKE ''%''+@Search+''%''
		OR C.CategoryName LIKE ''%''+@Search+''%'')		 
END
' 
END

