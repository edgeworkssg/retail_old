IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewPurchaseOrderHeader]'))
DROP VIEW [dbo].[ViewPurchaseOrderHeader]

--------------- Recreate ViewPurchaseOrderHeader -----

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewPurchaseOrderHeader]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[ViewPurchaseOrderHeader]
AS
SELECT poh.PurchaseOrderHeaderRefNo, poh.RequestedBy, poh.PurchaseOrderDate, poh.DepartmentID, poh.PaymentTermID, poh.ShipVia, poh.ShipTo, poh.DateNeededBy,  poh.SupplierID, 
        poh.UserName, poh.Remark, poh.InventoryLocationID, il.InventoryLocationName, poh.CreatedBy, poh.CreatedOn, poh.ModifiedBy, poh.ModifiedOn, 
        ISNULL(poh.userfld1, '''') AS Status, ISNULL(poh.userfld2, '''') AS POType, ISNULL(poh.userfld3, '''') AS ApprovalDate, ISNULL(poh.userfld4, '''') AS ApprovedBy, 
        ISNULL(poh.userfld5, '''') AS SpecialValidFrom, ISNULL(poh.userfld6, '''') AS SpecialValidTo, ISNULL(poh.userfld7, '''') AS ApprovalStatus, ISNULL(poh.userfld8, '''') 
        AS SalesPersonID, ISNULL(poh.userfld9, '''') AS PriceLevel, poh.userint1 AS ReasonID, ISNULL(poh.userint2, 0) AS DestInventoryLocationID, ISNULL(poh.userint3, 0) 
        AS WarehouseID, ISNULL(poh.userflag1, 0) AS IsAutoStockIn, ISNULL(s.SupplierName, '''') AS OrderFromName
FROM dbo.PurchaseOrderHeader AS poh LEFT OUTER JOIN
    dbo.InventoryLocation AS il ON il.InventoryLocationID = poh.InventoryLocationID LEFT OUTER JOIN
    dbo.Supplier AS s ON s.SupplierID = poh.SupplierID '