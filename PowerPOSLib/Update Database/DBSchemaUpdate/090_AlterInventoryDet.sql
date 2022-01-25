IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'GSTAmount' AND OBJECT_ID = OBJECT_ID(N'[InventoryDet]'))
BEGIN
ALTER TABLE dbo.InventoryDet ADD
    GSTAmount float NULL
END