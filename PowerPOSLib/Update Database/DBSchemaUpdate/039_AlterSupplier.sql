alter table supplier alter column contactno1 varchar(50)
alter table supplier alter column contactno2 varchar(50)
alter table supplier alter column contactno3 varchar(50)

Alter Table Supplier Alter Column CustomerAddress nvarchar(250) 
Alter Table Supplier Alter Column ShipToaddress nvarchar(250) 
Alter Table Supplier Alter Column BillToAddress nvarchar(250) 

Alter Table Supplier Alter Column ContactPerson1 nvarchar(50) 
Alter Table Supplier Alter Column ContactPerson2 nvarchar(50) 
Alter Table Supplier Alter Column ContactPerson3 nvarchar(50) 

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CreatedOn' AND [object_id] = OBJECT_ID(N'Supplier'))
BEGIN
	alter table supplier add CreatedOn DateTime
	alter table supplier add CreatedBy varchar(50)
	alter table supplier add ModifiedOn DateTime 
	alter table supplier add ModifiedBy varchar(50)
	alter table supplier add Deleted bit default 0
END