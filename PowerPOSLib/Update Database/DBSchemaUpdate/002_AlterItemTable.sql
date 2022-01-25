IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ItemImage' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD ItemImage varbinary(MAX)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'AvgCostPrice' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD AvgCostPrice MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'BalanceQuantity' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD BalanceQuantity FLOAT NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag6' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD userflag6 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag7' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD userflag7 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag8' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD userflag8 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag9' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD userflag9 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag10' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD userflag10 bit NULL
END