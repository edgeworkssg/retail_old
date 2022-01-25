IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportGroup]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportGroup]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportGroup]
	@ItemDepartmentID NVARCHAR(500),
	@Category NVARCHAR(500),
	@LocationID INT,
	@LocationGroupID INT,
	@Supplier NVARCHAR(500),
	@Search NVARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;
	WITH INV AS (
	SELECT   TAB.ItemNo
			,TAB.InventoryLocationGroupID
			,TAB.InventoryLocationGroupName
			,TAB.InventoryLocationID
			,TAB.InventoryLocationName
			,SUM(ISNULL(TAB.Quantity,0)) Quantity
			,CASE WHEN @LocationGroupID = 0 AND @LocationID = 0 THEN ISNULL(TAB.ItemCostPrice,0)
				  ELSE SUM(ISNULL(TAB.COG,0)) END COG
			  
	FROM (
	SELECT   TAB.ItemNo
			,CASE WHEN @LocationGroupID = 0 THEN 0
				  ELSE TAB.InventoryLocationGroupID END InventoryLocationGroupID
			,CASE WHEN @LocationGroupID = 0 THEN ''ALL''
				  ELSE TAB.InventoryLocationGroupName END InventoryLocationGroupName
			,TAB.InventoryLocationID
			,TAB.InventoryLocationName
			,TAB.Quantity
			,TAB.COG
			,TAB.ItemCostPrice
	FROM (
		SELECT   VIS.ItemNo
				,VIS.InventoryLocationGroupID
				,VIS.InventoryLocationGroupName
				,CASE WHEN @LocationID = 0 THEN 0
					  ELSE VIS.InventoryLocationID END InventoryLocationID
				,CASE WHEN @LocationID = 0 THEN ''ALL''
					  ELSE VIS.InventoryLocationName END InventoryLocationName
				,CASE WHEN @LocationID = 0 THEN VIS.LocGroupBalanceQty
					  ELSE VIS.LocBalanceQty END Quantity
				,CASE WHEN @LocationID = 0 THEN VIS.LocGroupCostPrice
					  ELSE VIS.LocCostPrice END COG
				,VIS.ItemCostPrice
		FROM	ViewItemSummary VIS
		WHERE	(@LocationID = 0 OR @LocationID = -1 OR VIS.InventoryLocationID = @LocationID)
				AND (@LocationGroupID = 0 OR @LocationGroupID = -1 OR VIS.InventoryLocationGroupID = @LocationGroupID)
	) TAB
	GROUP BY TAB.ItemNo
			,TAB.InventoryLocationGroupID
			,TAB.InventoryLocationGroupName
			,TAB.InventoryLocationID
			,TAB.InventoryLocationName
			,TAB.Quantity
			,TAB.COG
			,TAB.ItemCostPrice
	) TAB
	GROUP BY TAB.ItemNo
			,TAB.InventoryLocationGroupID
			,TAB.InventoryLocationGroupName
			,TAB.InventoryLocationID
			,TAB.InventoryLocationName
			,TAB.ItemCostPrice
	)

		SELECT   I.FactoryPrice
				,I.RetailPrice
				,ID.DepartmentName
				,I.ItemNo
				,C.CategoryName
				,REPLACE(I.ItemName, ''\"'', '''') AS ItemName
				,ISM.Quantity 
				,ISM.COG  
				,ISM.Quantity * ISM.COG TotalCost 
				,I.Userfld1 AS UOM    
				,I.Attributes1, I.Attributes2, I.Attributes3, I.Attributes4, I.Attributes5, I.Attributes6, I.Attributes7, I.Attributes8 
				, ISNULL(SUPP.SupplierName,'''') as SupplierName 
				,ISM.InventoryLocationGroupID
				,ISM.InventoryLocationGroupName
				,ISM.InventoryLocationID
				,ISM.InventoryLocationName
		FROM	Item I 
				INNER JOIN Category C ON C.CategoryName = I.CategoryName
				INNER JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentID
				LEFT JOIN INV ISM ON ISM.ItemNo = I.ItemNo
				LEFT JOIN (
					SELECT   ISM.ItemNo
							,ISM.SupplierID
							,SUPP.SupplierName
							,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
					FROM	ItemSupplierMap ISM
							INNER JOIN Supplier SUPP ON SUPP.SupplierID = ISM.SupplierID
					WHERE	ISNULL(ISM.Deleted,0) = 0
				) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
		WHERE	ISNULL(I.Deleted,0) =0
				AND ISNULL(I.IsInInventory,0) = 1
				AND ID.ItemDepartmentID <> ''SYSTEM''			
				AND (@ItemDepartmentID = ''ALL'' OR ID.ItemDepartmentID = @ItemDepartmentID)
				AND (@Category = ''ALL'' OR C.CategoryName = @Category)
				AND (@Supplier = ''ALL'' OR ISNULL(SUPP.SupplierName,'''') = @Supplier)
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
		ORDER BY ID.DepartmentName, C.CategoryName, I.ItemNo
END
' 
 
END