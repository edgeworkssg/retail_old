IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPriceByItemInvLoc' AND [object_id] = OBJECT_ID(N'InventoryDet'))
BEGIN
    ALTER TABLE InventoryDet ADD CostPriceByItemInvLoc MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPriceByItem' AND [object_id] = OBJECT_ID(N'InventoryDet'))
BEGIN
    ALTER TABLE InventoryDet ADD CostPriceByItem MONEY NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPriceByItemInvGroup' AND [object_id] = OBJECT_ID(N'InventoryDet'))
BEGIN
    ALTER TABLE InventoryDet ADD CostPriceByItemInvGroup MONEY NULL
END