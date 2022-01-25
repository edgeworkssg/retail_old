IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportInvLocGroup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportInvLocGroup]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportInvLocGroup]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportInvLocGroup]
	@ItemDepartmentID NVARCHAR(500),
	@Category NVARCHAR(500),
	@LocationID INT,
	@Supplier NVARCHAR(500),
	@Search NVARCHAR(500),
	@IsUseInvLocGroup bit
AS
BEGIN
	SET NOCOUNT ON;
	SELECT   I.FactoryPrice
			,I.RetailPrice
			,ID.DepartmentName
			,I.ItemNo
			,I.Barcode 
			,C.CategoryName
			,REPLACE(I.ItemName, ''\"'', '''') AS ItemName
			,0 [StockIn], 0 [StockOut], 0 [TransferIn], 0 [TransferOut], 0 [AdjustmentIn] 
			,0 [AdjustmentOut] 
			,SUM(ISNULL(ISM.BalanceQty,0)) AS Quantity 
			,ISNULL(CASE WHEN @LocationID = 0 THEN I.AvgCostPrice ELSE ISM.CostPrice END,0)  AS COG  
			,SUM(ISNULL(ISM.BalanceQty,0) * ISNULL(CASE WHEN @LocationID = 0 THEN I.AvgCostPrice ELSE ISM.CostPrice END,0))  AS TotalCost 
			,SUM(ISNULL(ISM.BalanceQty,0) * I.RetailPrice)  AS TotalRetail  
			,ISNULL(CASE WHEN @LocationID = 0 THEN I.AvgCostPrice ELSE ISNULL(ISMG.CostPrice,0) END,0) AS COGGroup  
			,SUM(ISNULL(ISM.BalanceQty,0) * ISNULL(CASE WHEN @LocationID = 0 THEN I.AvgCostPrice ELSE ISNULL(ISMG.CostPrice,0) END,0))  AS TotalCostGroup 
			,ISNULL(I.AvgCostPrice, I.RetailPrice) as GlobalCostGroup
			,SUM(ISNULL(ISM.BalanceQty,0) * ISNULL(I.AvgCostPrice, I.RetailPrice)) as TotalGlobalCostGroup
			,I.Userfld1 AS UOM    
			,I.Attributes1, I.Attributes2, I.Attributes3, I.Attributes4, I.Attributes5, I.Attributes6, I.Attributes7, I.Attributes8 
			, ISNULL(SUPP.SupplierName,'''') as SupplierName 
	FROM	Item I
			INNER JOIN Category C WITH(NOLOCK) ON C.CategoryName = I.CategoryName
			INNER JOIN ItemDepartment ID WITH(NOLOCK) ON ID.ItemDepartmentID = C.ItemDepartmentID
			LEFT JOIN ItemSummary ISM WITH(NOLOCK) ON ISM.ItemNo = I.ItemNo
			LEFT JOIN InventoryLocation IL WITH(NOLOCK) ON IL.InventoryLocationID = ISM.InventoryLocationID and ISNULL(IL.Deleted,0) = 0
			LEFT JOIN ItemSummaryGroup ISMG WITH(NOLOCK) ON ISMG.ItemNo = I.ItemNo and ISMG.InventoryLocationGroupID = ISNULL(IL.Userint1,0)
			LEFT JOIN (
				SELECT   ISM.ItemNo
						,ISM.SupplierID
						,SUPP.SupplierName
						,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
				FROM	ItemSupplierMap ISM
						INNER JOIN Supplier SUPP WITH(NOLOCK) ON SUPP.SupplierID = ISM.SupplierID
				WHERE	ISNULL(ISM.Deleted,0) = 0
			) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
	WHERE	ISNULL(I.Deleted,0) =0
			AND ISNULL(I.IsInInventory,0) = 1
			AND ID.ItemDepartmentID <> ''SYSTEM''			
			AND (@ItemDepartmentID = ''ALL'' OR ID.ItemDepartmentID = @ItemDepartmentID)
			AND (@Category = ''ALL'' OR C.CategoryName = @Category)
			AND ((@IsUseInvLocGroup = 0 AND (@LocationID = 0 OR ISM.InventoryLocationID = @LocationID))
				OR (@IsUseInvLocGroup = 1 AND ((@LocationID = 0 AND ISNULL(IL.UserInt1,0) <> 0) OR ISNULL(IL.Userint1,0) = @LocationID)))				
			AND (@Supplier = ''ALL'' OR ISNULL(SUPP.SupplierName,'''') = @Supplier)
			AND (I.ItemNo LIKE ''%''+@Search+''%''
			OR I.ItemNo in (select ItemNo from AlternateBarcode where barcode LIKE ''%''+@Search+''%'' and ISNULL(deleted,0) = 0)
			OR I.ItemName LIKE ''%''+@Search+''%''
			OR ISNULL(I.ItemDesc,'''') LIKE ''%''+@Search+''%''
			OR ISNULL(I.Barcode,'''') LIKE ''%''+@Search+''%''
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
	GROUP BY I.FactoryPrice
			,I.RetailPrice
			,ID.DepartmentName
			,I.ItemNo
			,I.Barcode
			,C.CategoryName
			,REPLACE(I.ItemName, ''\"'', '''')
			,ISNULL(I.AvgCostPrice,0)
			,I.Userfld1
			,I.Attributes1, I.Attributes2, I.Attributes3, I.Attributes4
			,I.Attributes5, I.Attributes6, I.Attributes7, I.Attributes8 
			,SUPP.SupplierName    
			,ISMG.CostPrice
			,I.AvgCostPrice
			,ISM.CostPrice
	ORDER BY ID.DepartmentName, C.CategoryName, I.ItemNo
END
' 
END