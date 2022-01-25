IF NOT EXISTS (SELECT * FROM Item WHERE ItemNo = 'LINE_DISCOUNT' )
BEGIN
	insert into item (ItemNo, ItemName, Barcode, CategoryName, RetailPrice, FactoryPrice, MinimumPrice,
	ItemDesc, IsServiceItem, IsInInventory, IsNonDiscountable, Attributes1, Attributes2, Attributes3,
	Attributes4, Attributes5, Attributes6, Attributes7, Attributes8, CreatedOn, CreatedBy, 
	Modifiedon, ModifiedBy, UniqueID, Deleted, Userfld9, Userfld10, GSTRule,
	IsCommission)
	values 
	('LINE_DISCOUNT', 'LINE_DISCOUNT','','SYSTEM',0,0,0,'',1,0,0, '','','','','','','','',
	GETDATE(), 'SCRIPT', GETDATE(), 'SCRIPT', NEWID(), 0, 'N','N', 3, 1)
END

