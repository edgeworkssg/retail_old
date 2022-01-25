IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'FactoryPrice' AND [object_id] = OBJECT_ID(N'PurchaseOrderDetail'))
BEGIN
    ALTER TABLE PurchaseOrderDetail ADD FactoryPrice MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostOfGoods' AND [object_id] = OBJECT_ID(N'PurchaseOrderDetail'))
BEGIN
    ALTER TABLE PurchaseOrderDetail ADD CostOfGoods MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Deleted' AND [object_id] = OBJECT_ID(N'PurchaseOrderDetail'))
BEGIN
    ALTER TABLE PurchaseOrderDetail ADD Deleted BIT NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'ExpiryDate' AND [object_id] = OBJECT_ID(N'PurchaseOrderDetail'))
BEGIN
    ALTER TABLE PurchaseOrderDetail ADD ExpiryDate DATETIME NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CurrencyID' AND [object_id] = OBJECT_ID(N'PurchaseOrderHeader'))
BEGIN
    ALTER TABLE PurchaseOrderHeader ADD CurrencyID INT NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DateFrom' AND [object_id] = OBJECT_ID(N'PurchaseOrderHeader'))
BEGIN
    ALTER TABLE PurchaseOrderHeader ADD DateFrom datetime NULL
END


IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DateTo' AND [object_id] = OBJECT_ID(N'PurchaseOrderHeader'))
BEGIN
    ALTER TABLE PurchaseOrderHeader ADD DateTo datetime NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Amount' AND [object_id] = OBJECT_ID(N'PurchaseOrderDetail'))
BEGIN
    ALTER TABLE PurchaseOrderDetail ADD Amount Money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'GSTAmount' AND [object_id] = OBJECT_ID(N'PurchaseOrderDetail'))
BEGIN
    ALTER TABLE PurchaseOrderDetail ADD GSTAmount Money NULL
END

-- alter quantity into decimal
ALTER TABLE PurchaseOrderDetail
ALTER COLUMN Quantity DECIMAL(20,5) NULL

ALTER TABLE PurchaseOrderDetail
ALTER COLUMN userint1 DECIMAL(20,5) NULL

ALTER TABLE PurchaseOrderDetail
ALTER COLUMN userint2 DECIMAL(20,5) NULL

ALTER TABLE PurchaseOrderDetail
ALTER COLUMN userint3 DECIMAL(20,5) NULL

ALTER TABLE PurchaseOrderDetail
ALTER COLUMN userint4 DECIMAL(20,5) NULL

ALTER TABLE SalesOrderMapping
ALTER COLUMN Qty DECIMAL(20,5) NULL

ALTER TABLE SalesOrderMapping
ALTER COLUMN QtyApproved DECIMAL(20,5) NULL
