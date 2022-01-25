IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld11' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld11 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld12' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld12 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld13' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld13 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld14' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld14 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld15' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld15 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld16' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld16 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld17' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld17 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld18' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld18 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld19' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld19 NVARCHAR(500)
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfld20' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet Add Userfld20 NVARCHAR(500)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat6' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userfloat6 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat7' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userfloat7 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat8' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userfloat8 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat9' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userfloat9 MONEY NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat10' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userfloat10 MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag6' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userflag6 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag7' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userflag7 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag8' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userflag8 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag9' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userflag9 BIT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userflag10' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userflag10 BIT NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint6' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userint6 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint7' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userint7 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint8' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userint8 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint9' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userint9 INT NULL
END
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userint10' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
    ALTER TABLE OrderDet ADD Userint10 INT NULL
END
