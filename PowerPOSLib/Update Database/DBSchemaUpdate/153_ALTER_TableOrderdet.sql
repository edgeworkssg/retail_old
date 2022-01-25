IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld3' AND [object_id] = OBJECT_ID(N'OrderDet'))
BEGIN
	Alter table OrderDet
    alter column userfld3 Varchar(MAX) NULL
END