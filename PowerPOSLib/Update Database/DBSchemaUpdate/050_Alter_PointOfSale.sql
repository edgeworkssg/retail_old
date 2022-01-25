IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'TenantMachineID' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add TenantMachineID NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ApiKey' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add ApiKey NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'InterfaceValidationStatus' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add InterfaceValidationStatus NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Mall' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add Mall NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RetailerName' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add RetailerName NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RetailerLevel' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add RetailerLevel NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ShopNo' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add ShopNo NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RetailerContactPerson' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add RetailerContactPerson NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RetailerContactNo' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add RetailerContactNo NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RetailerEmail' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add RetailerEmail NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'InterfaceDevTeam' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add InterfaceDevTeam NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'VendorContactPersonName' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add VendorContactPersonName NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'VendorContactNo' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add VendorContactNo NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'VendorContactName' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add VendorContactName NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'BusinessStartDate' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add BusinessStartDate DATETIME NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'BusinessEndDate' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add BusinessEndDate DATETIME NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Option' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [Option] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'TenantCompanyName' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [TenantCompanyName] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RetailerDesignation' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [RetailerDesignation] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'VendorEmail' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [VendorEmail] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'POSType' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [POSType] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'POSBrand' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [POSBrand] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'POSOS' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [POSOS] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'POSSoftware' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [POSSoftware] NVARCHAR(500) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'NoOfPOS' AND [object_id] = OBJECT_ID(N'PointOfSale'))
BEGIN
    ALTER TABLE PointOfSale Add [NoOfPOS] INT NULL
END