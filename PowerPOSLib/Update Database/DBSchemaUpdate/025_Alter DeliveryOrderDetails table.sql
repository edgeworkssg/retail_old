IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedOn' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
    CreatedOn datetime NOT NULL CONSTRAINT DF_DeliveryOrderDetails_CreatedOn DEFAULT (getdate())
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'ModifiedOn' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
    ModifiedOn datetime NOT NULL CONSTRAINT DF_DeliveryOrderDetails_ModifiedOn DEFAULT (getdate())
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedBy' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
    CreatedBy varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'ModifiedBy' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
    ModifiedBy varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UniqueID' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
    UniqueID uniqueidentifier NOT NULL CONSTRAINT DF_DeliveryOrderDetails_UniqueID DEFAULT (newid())
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Deleted' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
	Deleted bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'InventoryHdrRefNo' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrderDetails]'))
BEGIN
ALTER TABLE dbo.DeliveryOrderDetails ADD
	InventoryHdrRefNo varchar(50) NULL
END
