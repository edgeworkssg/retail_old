IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DateCanceled' AND [object_id] = OBJECT_ID(N'Vouchers'))
BEGIN
    ALTER TABLE Vouchers ADD DateCanceled datetime null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'VoucherHeaderID' AND [object_id] = OBJECT_ID(N'Vouchers'))
BEGIN
    ALTER TABLE Vouchers ADD VoucherHeaderID int null
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Outlet' AND [object_id] = OBJECT_ID(N'Vouchers'))
BEGIN
    ALTER TABLE Vouchers ADD Outlet varchar(max) null
END
