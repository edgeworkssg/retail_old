IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OutletGroupID' AND [object_id] = OBJECT_ID(N'Outlet'))
BEGIN
    ALTER TABLE Outlet ADD OutletGroupID INT NULL
END
