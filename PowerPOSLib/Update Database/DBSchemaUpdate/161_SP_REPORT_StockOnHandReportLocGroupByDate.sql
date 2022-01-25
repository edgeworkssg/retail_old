IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportLocGroupByDate]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportLocGroupByDate]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportLocGroupByDate]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportLocGroupByDate]
 @ItemDepartmentID NVARCHAR(500),
 @Category NVARCHAR(500),
 @LocationID int,
 @Supplier NVARCHAR(500),
 @StartDate DATETIME,
 @Search NVARCHAR(MAX),
 @IsUseInvLocGroup bit
AS
BEGIN
	SET NOCOUNT ON;
	SET @StartDate =DATEADD(dd,1,@StartDate)

SELECT  I.ItemNo
		, I.CategoryName
		, ID.DepartmentName
		, I.Barcode 
		, I.ItemName
		, I.RetailPrice
		, ISNULL(RemainingQty,0) Quantity 
		, ISNULL(RemainingQty,0) * I.RetailPrice as TotalRetail
		, ISNULL(CGI.CostPriceByItemInvLoc,0) as COG  
		, ISNULL(RemainingQty,0) * ISNULL(CGI.CostPriceByItemInvLoc,0) as TotalCost
		, ISNULL(CGI.CostPriceByItemInvGroup,I.RetailPrice) as COGGroup   
		, ISNULL(CGI.CostPriceByItemInvGroup,I.RetailPrice) * ISNULL(RemainingQty,0) as TotalCostGroup
		, ISNULL(I.AvgCostPrice, I.FactoryPrice) as GlobalCostGroup
		, ISNULL(RemainingQty,0) * ISNULL(I.AvgCostPrice, I.RetailPrice) as TotalGlobalCostGroup
		, I.Userfld1 AS UOM    
		, I.Attributes1, I.Attributes2, I.Attributes3, I.Attributes4, I.Attributes5, I.Attributes6, I.Attributes7, I.Attributes8 
		, ISNULL(SUPP.SupplierName,'''') as SupplierName 		
FROM	Item I  WITH(NOLOCK) 
		INNER JOIN Category C WITH(NOLOCK) ON C.CategoryName = I.CategoryName
		INNER JOIN ItemDepartment ID WITH(NOLOCK) ON ID.ItemDepartmentID = C.ItemDepartmentID
		LEFT JOIN (
    		SELECT ItemNo 
				, SUM(CASE WHEN MovementType LIKE ''% In'' then Quantity ELSE -1 * Quantity END) as RemainingQTY 
    		FROM InventoryHdr IH  WITH(NOLOCK)
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
    			INNER JOIN InventoryLocation LI  WITH(NOLOCK) ON IH.InventoryLocationID = LI.InventoryLocationID 
			WHERE Ih.InventoryDate < @StartDate
				AND  (@LocationID = 0 OR LI.InventoryLocationID = @LocationID)
			GROUP BY ItemNo 
		) INV ON INV.ItemNo = I.ItemNo
		LEFT JOIN  
		( 
			SELECT ItemNo, CostPriceByItem, CostPriceByItemInvGroup, CostPriceByItemInvLoc FROM 
			(
				SELECT  ID.ItemNo, ID.CostPriceByItem, ISNULL(ID.CostPriceByItemInvGroup ,IT.FactoryPrice) as CostPriceByItemInvGroup,
				ISNULL(ID.CostPriceByItemInvLoc,IT.FactoryPrice) CostPriceByItemInvLoc,
				row_number() over(partition by ID.ItemNo order by IH.InventoryDate desc) rn
				FROM InventoryHdr IH 
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
				INNER JOIN Item IT  WITH(NOLOCK) ON IT.ItemNo = ID.ItemNo
				WHERE IH.InventoryLocationID = @LocationID 
			) A where rn = 1
		) CGI on I.Itemno = CGI.ItemNo
		LEFT JOIN (
			SELECT   ISM.ItemNo
					,ISM.SupplierID
					,SUPP.SupplierName
					,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
			FROM	ItemSupplierMap ISM
					INNER JOIN Supplier SUPP WITH(NOLOCK) ON SUPP.SupplierID = ISM.SupplierID
			WHERE	ISNULL(ISM.Deleted,0) = 0
		) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
WHERE	ISNULL(I.Deleted,0) = 0
		AND ISNULL(I.IsInInventory,0) = 1
		AND ISNULL(RemainingQty,0) <> 0 
		AND (@Category = ''ALL'' OR I.CategoryName = @Category)
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
		OR I.CategoryName LIKE ''%''+@Search+''%'')	
Order By CategoryName, ItemNo	 
END
' 
END

