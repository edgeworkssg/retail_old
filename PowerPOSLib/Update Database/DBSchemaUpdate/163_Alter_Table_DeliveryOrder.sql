IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DeliveredDate' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add DeliveredDate DateTime NULL 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld1' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld1 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld2' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld2 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld3' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld3 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld4' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld4 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld5' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld5 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld6' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld6 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld7' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld7 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld8' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld8 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld9' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld9 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld10' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder Add Userfld10 NVARCHAR(500)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat1' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userfloat1 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat2' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userfloat2 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat3' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userfloat3 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat4' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userfloat4 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat5' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userfloat5 MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag1' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userflag1 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag2' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userflag2 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag3' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userflag3 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag4' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userflag4 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag5' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userflag5 BIT NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint1' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userint1 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint2' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userint2 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint3' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userint3 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint4' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userint4 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint5' AND [object_id] = OBJECT_ID(N'DeliveryOrder'))
BEGIN
    ALTER TABLE DeliveryOrder ADD Userint5 INT NULL
END