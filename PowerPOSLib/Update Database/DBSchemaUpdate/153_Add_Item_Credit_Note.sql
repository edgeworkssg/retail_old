
if not exists (select * from item where itemno = 'CREDIT_NOTE')
begin
	INSERT INTO Item(ItemNo, ItemName, Barcode, CategoryName, RetailPrice, FactoryPrice, MinimumPrice, IsServiceItem, IsInInventory, IsNonDiscountable,
		CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, uniqueID, Deleted, GSTRule, IsCommission)
	values ('CREDIT_NOTE', 'CREDIT_NOTE', 'CREDIT_NOTE', 'SYSTEM', 0,0,0,0,0,1,
		getdate(), 'SCRIPT', getdate(), 'SCRIPT', newid(), 0, 3, 0) 
end