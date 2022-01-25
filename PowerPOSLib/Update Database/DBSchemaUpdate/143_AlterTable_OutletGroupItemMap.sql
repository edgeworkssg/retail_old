IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'P1' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add P1 money 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'P2' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add P2 money 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'P3' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add P3 money 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'P4' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add P4 money 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'P5' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add P5 money 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld1' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld1 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld2' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld2 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld3' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld3 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld4' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld4 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld5' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld5 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld6' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld6 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld7' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld7 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld8' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld8 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld9' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld9 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld10' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap Add Userfld10 NVARCHAR(500)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat1' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userfloat1 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat2' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userfloat2 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat3' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userfloat3 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat4' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userfloat4 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat5' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userfloat5 MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag1' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userflag1 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag2' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userflag2 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag3' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userflag3 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag4' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userflag4 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag5' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userflag5 BIT NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint1' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userint1 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint2' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userint2 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint3' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userint3 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint4' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userint4 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint5' AND [object_id] = OBJECT_ID(N'OutletGroupItemMap'))
BEGIN
    ALTER TABLE OutletGroupItemMap ADD Userint5 INT NULL
END