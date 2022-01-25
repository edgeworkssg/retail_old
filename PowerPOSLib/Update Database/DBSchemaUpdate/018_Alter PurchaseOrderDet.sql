ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld1 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld2 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld3 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld4 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld5 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld6 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld7 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld8 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld9 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderDet ALTER COLUMN Userfld10 NVARCHAR(MAX)

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfloat6' AND OBJECT_ID = OBJECT_ID(N'[PurchaseOrderDet]'))
BEGIN
ALTER TABLE dbo.PurchaseOrderDet ADD
    userfloat6 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfloat7' AND OBJECT_ID = OBJECT_ID(N'[PurchaseOrderDet]'))
BEGIN
ALTER TABLE dbo.PurchaseOrderDet ADD
    userfloat7 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfloat8' AND OBJECT_ID = OBJECT_ID(N'[PurchaseOrderDet]'))
BEGIN
ALTER TABLE dbo.PurchaseOrderDet ADD
    userfloat8 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfloat9' AND OBJECT_ID = OBJECT_ID(N'[PurchaseOrderDet]'))
BEGIN
ALTER TABLE dbo.PurchaseOrderDet ADD
    userfloat9 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfloat10' AND OBJECT_ID = OBJECT_ID(N'[PurchaseOrderDet]'))
BEGIN
ALTER TABLE dbo.PurchaseOrderDet ADD
    userfloat10 money NULL
END
