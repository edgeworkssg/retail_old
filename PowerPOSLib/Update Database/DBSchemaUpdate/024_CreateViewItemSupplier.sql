
declare @statement Nvarchar(max)
set @statement = ''
IF  NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewItemSupplier]'))
BEGIN
	set @statement = @statement + N'CREATE VIEW [dbo].[ViewItemSupplier] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER VIEW [dbo].[ViewItemSupplier] '	
END
 set @statement = @statement + N'
AS
SELECT     dbo.Category.IsForSale, dbo.Category.IsDiscountable, dbo.Category.CategoryName, dbo.Category.Category_ID, dbo.Category.IsGST, dbo.Category.AccountCategory, 
                      dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.Barcode, dbo.Item.RetailPrice, dbo.Item.FactoryPrice, dbo.Item.MinimumPrice, dbo.Item.ItemDesc, 
                      dbo.Item.IsInInventory, dbo.Item.IsNonDiscountable, dbo.Item.Brand, dbo.Item.ProductLine, dbo.Item.Remark, dbo.Item.Deleted, dbo.Item.Attributes1, 
                      dbo.Item.Attributes2, dbo.Item.Attributes3, dbo.Item.Attributes4, dbo.Item.Attributes5, dbo.Item.Attributes6, dbo.Item.Attributes8, dbo.Item.Attributes7, 
                      dbo.Category.ItemDepartmentId, dbo.ItemDepartment.DepartmentName, 
                      dbo.Item.ItemNo + '' '' + dbo.Item.ItemName + '' '' + dbo.Item.CategoryName + '' '' + ISNULL(dbo.ItemDepartment.DepartmentName, '''') + '' '' + ISNULL(dbo.Item.ItemDesc, '''') 
                      AS search, dbo.Item.IsServiceItem, dbo.Item.IsCourse, dbo.Item.CourseTypeID, dbo.Item.ProductionDate, dbo.Item.IsGST AS Expr1, dbo.Item.hasWarranty, 
                      dbo.Item.IsDelivery, dbo.Item.GSTRule, dbo.Item.IsVitaMix, dbo.Item.IsWaterFilter, dbo.Item.IsYoung, dbo.Item.IsJuicePlus, dbo.Item.IsCommission,
                      dbo.ItemSupplierMap.CostPrice, dbo.Supplier.SupplierID, dbo.Supplier.SupplierName, ISNULL(dbo.Item.userflag1, ''false'') AS Userflag1, dbo.Item.userfld1 AS UOM 
FROM         dbo.Category INNER JOIN
                      dbo.Item ON dbo.Category.CategoryName = dbo.Item.CategoryName INNER JOIN
                      dbo.ItemDepartment ON dbo.Category.ItemDepartmentId = dbo.ItemDepartment.ItemDepartmentID
                      INNER JOIN dbo.ItemSupplierMap ON dbo.Item.ItemNo = dbo.ItemSupplierMap.ItemNo AND ISNULL(dbo.ItemSupplierMap.Deleted, 0) = 0
                      INNER JOIN dbo.Supplier ON dbo.ItemSupplierMap.SupplierID = dbo.Supplier.SupplierID
WHERE ISNULL(dbo.Item.Userflag7,0) = 0
'

EXEC dbo.sp_executesql @statement