IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockReportBreakdownByLocationItemSummary]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchStockReportBreakdownByLocationItemSummary]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockReportBreakdownByLocationItemSummary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[FetchStockReportBreakdownByLocationItemSummary] 
 -- Add the parameters for the stored procedure here
  @Search nvarchar(50),
  @CostingMethod nvarchar(50)
AS
BEGIN
 
 DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)
 DECLARE @DynamicPivotQuery2 AS NVARCHAR(MAX)
 DECLARE @ColumnName AS NVARCHAR(MAX)
 DECLARE @ColumnNameSelect AS NVARCHAR(MAX)

 SELECT @ColumnName= ISNULL(@ColumnName + '','','''') + QUOTENAME(InventoryLocationName)
 FROM InventoryLocation where (deleted=0 or deleted is null)
 ORDER BY InventoryLocationID
 
 SELECT @ColumnNameSelect= ISNULL(@ColumnNameSelect + '','','''')  + ''ISNULL('' + QUOTENAME(InventoryLocationName) +'',0) as '' + QUOTENAME(InventoryLocationName) 
 FROM InventoryLocation where (deleted=0 or deleted is null)
 ORDER BY InventoryLocationID

 

  SET @DynamicPivotQuery = N''
  SELECT DepartmentName, CategoryName, ItemNo, ItemName, Barcode, Attributes1,'' + @ColumnNameSelect + ''
  FROM
  (
   SELECT st.DepartmentName, st.CategoryName, st.ItemNo,st.ItemName, st.Barcode, st.Attributes1, st.InventoryLocationName,
    0 as negatifsales,  ROUND(ISNULL(BalanceQty,0), 4) as StockOnHand
   FROM
   (
   SELECT IT.RetailPrice, IP.DepartmentName,IT.ItemNo,IC.CategoryName, IT.ItemName ,IT.Barcode, IL.InventoryLocationName
   , JQ.BalanceQty 
   , Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 
   FROM 
   ItemSummary JQ
   RIGHT OUTER JOIN InventoryLocation IL on JQ.InventoryLocationID = IL.InventoryLocationID 
   RIGHT OUTER JOIN Item IT ON JQ.ItemNo = IT.ItemNo 
   INNER JOIN Category IC ON IC.CategoryName = IT.CategoryName 
   INNER JOIN itemdepartment IP on IP.itemdepartmentid = IC.ItemDepartmentId
   LEFT JOIN (SELECT ItemNo, altBarcode = STUFF((SELECT N'''', '''' + Barcode 
		FROM dbo.AlternateBarcode AS p2
		WHERE p2.ItemNo = p.ItemNo
		ORDER BY Barcode
		FOR XML PATH(N'''''''')), 1, 2, N'''''''')  
		FROM dbo.AlternateBarcode AS p
		GROUP BY ItemNo) ALT on IT.ItemNo = ALT.ItemNo
   WHERE (IT.Deleted=0 or IT.Deleted is null) 
   AND IP.ItemDepartmentID <> ''''SYSTEM'''' AND IsInInventory = 1 
   AND (ISNULL(IP.DepartmentName,'''''''') + IT.ItemNo+IT.Barcode+ItemName+ISNULL(attributes1,'''''''')+ISNULL(attributes2,'''''''')+ISNULL(attributes3,'''''''') 
    + ISNULL(attributes4,'''''''') + ISNULL(attributes5,'''''''') + ISNULL(attributes6,'''''''')+ISNULL(attributes7,'''''''')+ISNULL(attributes8,'''''''') 
    + ISNULL(ItemDesc,'''''''') + IC.CategoryName + ISNULL(ALT.altBarcode,'''''''') LIKE ''''%'' + @search +  ''%'''') 
   )st
  )
  as C PIVOT
  (
   SUM(StockOnHand) 
   FOR InventoryLocationName IN ('' + @ColumnName + '')) AS PVTTable''
 

 print @DynamicPivotQuery 
 EXEC sp_executesql @DynamicPivotQuery
END

' 
END

