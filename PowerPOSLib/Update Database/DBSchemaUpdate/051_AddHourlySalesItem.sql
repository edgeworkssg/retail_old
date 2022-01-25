IF NOT EXISTS(select * from Item where ItemNo = 'HOURLYSALES')  
BEGIN 
insert into item(itemNo, itemName, Barcode, CategoryName, 
RetailPrice, FactoryPrice, MinimumPrice, ItemDesc, IsServiceItem, IsInInventory, 
IsNonDiscountable, CreatedOn, CreatedBy, ModifiedOn, Modifiedby, UniqueID, Deleted,
GSTRule, IsCommission
)
values 
(
'HOURLYSALES', 'HOURLY SALES', '', 'SYSTEM', 0,0,0,'',1,0,0,GETDATE(),'SCRIPT',GETDATE(),
'SCRIPT',NEWID(),0,3,0);
END