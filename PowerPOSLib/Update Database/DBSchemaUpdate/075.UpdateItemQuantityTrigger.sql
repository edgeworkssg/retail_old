IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld1' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld1 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld2' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld2 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld3' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld3 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld4' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld4 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld5' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld5 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld6' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld6 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld7' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld7 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld8' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld8 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld9' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld9 nvarchar(50) null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld10' AND [object_id] = OBJECT_ID(N'ItemQuantityTrigger'))
BEGIN
    ALTER TABLE ItemQuantityTrigger Add userfld10 nvarchar(50) null
END