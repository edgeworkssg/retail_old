IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportItemSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_StockOnHandReportItemSummary]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_StockOnHandReportItemSummary]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_StockOnHandReportItemSummary]
	@ItemDepartmentID NVARCHAR(500),
	@Category NVARCHAR(500),
	@LocationID INT,
	@Supplier NVARCHAR(500),
	@Search NVARCHAR(500)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT 
			I.FactoryPrice
			,I.RetailPrice
			,ID.DepartmentName
			,I.ItemNo
			,I.Barcode 
			,C.CategoryName
			,REPLACE(I.ItemName, ''\"'', '''') AS ItemName
			,ISNULL(ISM.BalanceQty,0) AS Quantity 
			,ISNULL(ISM.BalanceQty,0) * I.RetailPrice  AS TotalRetail  
			,ISNULL(ISM.CostPrice,I.RetailPrice)  AS COG 
			,ISNULL(ISM.BalanceQty,0) * ISNULL(ISM.CostPrice,I.RetailPrice)  AS TotalCost 
			,I.Userfld1 AS UOM    
			,I.Attributes1, I.Attributes2, I.Attributes3, I.Attributes4, I.Attributes5, I.Attributes6, I.Attributes7, I.Attributes8 
			, ISNULL(SUPP.SupplierName,'''') as SupplierName 
	FROM	Item I
			INNER JOIN Category C WITH(NOLOCK) ON C.CategoryName = I.CategoryName
			INNER JOIN ItemDepartment ID WITH(NOLOCK) ON ID.ItemDepartmentID = C.ItemDepartmentID
			LEFT OUTER JOIN ItemSummary ISM WITH(NOLOCK) ON ISM.ItemNo = I.ItemNo and ISM.InventoryLocationID = @LocationID
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
	ORDER BY ID.DepartmentName, C.CategoryName, I.ItemNo
END
' 
END