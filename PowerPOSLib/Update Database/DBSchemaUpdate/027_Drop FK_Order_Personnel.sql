IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Order_Personnel]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeliveryOrder]'))
BEGIN
ALTER TABLE dbo.DeliveryOrder
	DROP CONSTRAINT FK_Order_Personnel
END
