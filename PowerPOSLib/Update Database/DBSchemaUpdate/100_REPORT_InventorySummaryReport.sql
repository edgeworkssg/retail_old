IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_InventorySummaryReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[REPORT_InventorySummaryReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_InventorySummaryReport]') AND type in (N'P', N'PC')) BEGIN 
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_InventorySummaryReport]
	@ItemDepartmentID NVARCHAR(500),
	@Category NVARCHAR(500),
	@SupplierID INT,
	@Search NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	IF OBJECT_ID(''tempdb..#TempInventorySummary'') IS NOT NULL
	  DROP TABLE #TempInventorySummary

	DECLARE @query AS NVARCHAR(MAX)
	DECLARE @cols AS NVARCHAR(MAX)
	DECLARE @colToSelect AS NVARCHAR(MAX)
	DECLARE @colSum AS NVARCHAR(MAX);

	SET @cols = STUFF((SELECT DISTINCT '','' + QUOTENAME(ID.InventoryLocationName) 
	FROM InventoryLocation ID 
	WHERE ISNULL(ID.Deleted,0) = 0
	FOR XML PATH(''''), TYPE
	).value(''.'', ''NVARCHAR(MAX)'') 
	,1,1,'''') 

	SET @colToSelect = STUFF((SELECT DISTINCT '','' + ''CAST(ISNULL('' + QUOTENAME(ID.InventoryLocationName) + '',0) AS DECIMAL(18,4)) AS ''  + QUOTENAME(ID.InventoryLocationName)                                           
	FROM InventoryLocation ID                                               
	WHERE ISNULL(ID.Deleted,0) = 0                                     
	FOR XML PATH(''''), TYPE                                              
	).value(''.'', ''NVARCHAR(MAX)'')                                           
	,1,1,'''')   

	SET @colSum = STUFF((SELECT DISTINCT ''+'' + ''CAST(ISNULL('' + QUOTENAME(ID.InventoryLocationName) + '',0) AS DECIMAL(18,4))  ''                                            
	FROM InventoryLocation ID                                               
	WHERE ISNULL(ID.Deleted,0) = 0                                     
	FOR XML PATH(''''), TYPE                                              
	).value(''.'', ''NVARCHAR(MAX)'')                                           
	,1,1,'''') 

	SELECT  ID.DepartmentName [Department Name]
			,C.CategoryName [Category Name]
			,I.ItemNo [Item No]
			,I.ItemName [Item Name]
			,I.Barcode
			,I.Attributes1
			,ISNULL(ISM.BalanceQty,0) BalanceQty
			,IL.InventoryLocationName
	INTO	#TempInventorySummary
	FROM	Item I 
			INNER JOIN Category C ON C.CategoryName = I.CategoryName
			INNER JOIN ItemDepartment ID ON ID.ItemDepartmentID = C.ItemDepartmentID
			INNER JOIN ItemSummary ISM ON ISM.ItemNo = I.ItemNo
			LEFT JOIN InventoryLocation IL ON IL.InventoryLocationID = ISM.InventoryLocationID
			LEFT JOIN (
				SELECT	ItemNo, altBarcode = STUFF((SELECT N'''', '''' + Barcode 
				FROM	AlternateBarcode AS p2
				WHERE p2.ItemNo = p.ItemNo
				ORDER BY Barcode
				FOR XML PATH(N'''')), 1, 2, N'''')  
				FROM dbo.AlternateBarcode AS p
				GROUP BY ItemNo
			) ALT on I.ItemNo = ALT.ItemNo
			LEFT JOIN (
				SELECT  ItemNo
						,SupplierID
						,ROW_NUMBER() OVER(PARTITION BY ISM.ItemNo ORDER BY ISM.ItemSupplierMapID) RowNo
				FROM	ItemSupplierMap ISM
				WHERE	ISNULL(ISM.Deleted,0) = 0
			) SUPP ON SUPP.ItemNo = I.ItemNo AND ISNULL(SUPP.RowNo,0) = 1
	WHERE	ISNULL(I.Deleted,0) = 0
			AND ISNULL(I.IsInInventory,0) = 1
			AND ID.ITemDepartmentID <> ''SYSTEM''
			AND (@ItemDepartmentID = ''ALL'' OR ID.ITemDepartmentID = @ItemDepartmentID)
			AND (@Category = ''ALL'' OR C.CategoryName = @Category)
			AND (@SupplierID = 0 OR ISNULL(SUPP.SupplierID,0) = @SupplierID)
			AND (ID.ItemDepartmentID LIKE ''%''+@Search+''%''
				 OR ID.DepartmentName LIKE ''%''+@Search+''%''
				 OR C.CategoryName LIKE ''%''+@Search+''%''
				 OR I.ItemNo LIKE ''%''+@Search+''%''
				 OR I.ItemName LIKE ''%''+@Search+''%''
				 OR I.Barcode LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes1,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes2,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes3,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes4,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes5,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes6,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes7,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.Attributes8,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(I.ItemDesc,'''') LIKE ''%''+@Search+''%''
				 OR ISNULL(ALT.altBarcode,'''') LIKE ''%''+@Search+''%'')

	SET @query = N''
	SELECT	[Department Name], [Category Name], [Item No], [Item Name], [Barcode], [Attributes1], ''+@colToSelect+N'', (''+@colSum+N'') [Total]
	FROM (
		SELECT *
		FROM	#TempInventorySummary
	) X PIVOT (
		SUM(BalanceQty) FOR InventoryLocationName IN (''+@cols+'')
	) P
	'';

	PRINT CAST(@query AS NTEXT)

	EXECUTE(@query);

END
' 
END

