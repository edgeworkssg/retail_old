IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportWithCostGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportLocGroupByDate]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportWithCostGroup]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportWithCostGroup]
 @Category NVARCHAR(500),
 @LocationID int,
 @StartDate DATETIME,
 @Search NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

SELECT  I.ItemNo
		,I.CategoryName
		,I.Barcode 
		,I.ItemName
		,I.RetailPrice
		, ISNULL(RemainingQty,0) Quantity 
		, ISNULL(RemainingQty,0) * I.RetailPrice as TotalRetail
		, CASE WHEN @LocationID = 0 THEN I.RetailPrice ELSE ISNULL(CGI.CostPriceByItem,0) END  as COG  
		, ISNULL(RemainingQty,0) * (CASE WHEN @LocationID = 0 THEN I.FactoryPrice ELSE ISNULL(CGI.CostPriceByItem,0) END) as TotalCost
		, ISNULL(CGI.CostPriceByItemInvGroup,I.RetailPrice) as COGGroup   
		, ISNULL(CGI.CostPriceByItemInvGroup,I.RetailPrice) * ISNULL(RemainingQty,0) as TotalCostGroup
		, ISNULL(WH.CostPriceByItem,I.RetailPrice) as COGWarehouse  
		, ISNULL(RemainingQty,0) * ISNULL(WH.CostPriceByItem,I.RetailPrice) as TotalCostWarehouse
FROM	Item I  WITH(NOLOCK) 
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
			SELECT ItemNo, CostPriceByItem, CostPriceByItemInvGroup FROM 
			(
				SELECT  ID.ItemNo, ID.CostPriceByItem, ISNULL(ISG.CostPrice,IT.FactoryPrice) as CostPriceByItemInvGroup,
				row_number() over(partition by ID.ItemNo order by IH.InventoryDate desc) rn
				FROM InventoryHdr IH 
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
				INNER JOIN Item IT  WITH(NOLOCK) ON IT.ItemNo = ID.ItemNo
				LEFT JOIN InventoryLocation IL on IL.InventoryLocationID = IH.InventoryLocationID and ISNULL(IL.Deleted,0) = 0
				LEFT JOIN ItemSummaryGroup ISG on ISG.Itemno = IT.ItemNo and ISG.InventoryLocationGroupID = ISNULL(IL.Userint1,0) and ISNULL(ISG.Deleted,0) = 0
				WHERE IH.InventoryLocationID = @LocationID 
			) A where rn = 1
		) CGI on I.Itemno = CGI.ItemNo
		LEFT JOIN  
		( 
			SELECT ItemNo, CostPriceByItem, CostPriceByItemInvGroup FROM 
			(
				SELECT  ID.ItemNo, ID.CostPriceByItem, ISNULL(ID.CostPriceByItemInvLoc,IT.FactoryPrice) as CostPriceByItemInvGroup,
				row_number() over(partition by ID.ItemNo order by IH.InventoryDate desc) rn
				FROM InventoryHdr IH 
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
				INNER JOIN Item IT  WITH(NOLOCK) ON IT.ItemNo = ID.ItemNo
				INNER JOIN InventoryLocation IL ON IH.InventorylocationID = IL.InventoryLocationID and ISNULL(IL.Userint1,0) = 2
				INNER JOIN Supplier s on S.Userint2 = IL.InventoryLocationID and ISNULL(S.Userflag1,0) = 1				
			) A where rn = 1
		) WH on I.Itemno = WH.ItemNo
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

