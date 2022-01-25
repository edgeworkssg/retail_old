IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'RecipientName' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    RecipientName nvarchar(200) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MobileNo' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    MobileNo varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'HomeNo' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    HomeNo varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PostalCode' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    PostalCode varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UnitNo' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    UnitNo varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Remark' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    Remark nvarchar(MAX) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UniqueID' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    UniqueID uniqueidentifier NOT NULL CONSTRAINT DF_DeliveryOrder_UniqueID DEFAULT (newid())
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsVendorDelivery' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    IsVendorDelivery bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DeliveryOutlet' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    DeliveryOutlet varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsDelivered' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    IsDelivered bit NULL
END

--comment it for a while, we need it later
--IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Signature' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
--BEGIN
--ALTER TABLE dbo.DeliveryOrder ADD
--    Signature varbinary(MAX)  NULL
--END

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'Signature' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder DROP COLUMN Signature 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsServerUpdate' AND OBJECT_ID = OBJECT_ID(N'[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder ADD
    IsServerUpdate bit NULL
END
