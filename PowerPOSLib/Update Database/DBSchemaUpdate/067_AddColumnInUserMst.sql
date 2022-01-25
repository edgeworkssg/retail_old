IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Gender' AND [object_id] = OBJECT_ID(N'UserMst'))
BEGIN
    ALTER TABLE UserMst ADD Gender bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ImageData' AND [object_id] = OBJECT_ID(N'UserMst'))
BEGIN
    ALTER TABLE UserMst ADD ImageData varbinary(MAX) NULL
END