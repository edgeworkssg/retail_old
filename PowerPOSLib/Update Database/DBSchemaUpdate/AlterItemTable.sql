IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ItemImage' AND [object_id] = OBJECT_ID(N'Item'))
BEGIN
    ALTER TABLE Item ADD ItemImage varbinary(MAX)
END