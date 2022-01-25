IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat6' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item Add Userfloat6 money
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat7' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item Add Userfloat7 money
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat8' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item Add Userfloat8 money
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat9' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item Add Userfloat9 money
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Userfloat10' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item Add Userfloat10 money
END