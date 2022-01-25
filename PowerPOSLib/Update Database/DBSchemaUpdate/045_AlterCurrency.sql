IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CurrencySymbol' AND [object_id] = OBJECT_ID(N'Currencies'))
BEGIN
    ALTER TABLE Currencies ADD CurrencySymbol NVARCHAR(50)
END
