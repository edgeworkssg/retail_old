IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OutletName' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD OutletName VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld1' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld1 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld2' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld2 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld3' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld3 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld4' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld4 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld5' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld5 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld6' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld6 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld7' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld7 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld8' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld8 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld9' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld9 VARCHAR(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld10' AND [object_id] = OBJECT_ID(N'GuestBook'))
BEGIN
    ALTER TABLE GuestBook ADD userfld10 VARCHAR(50) NULL
END

