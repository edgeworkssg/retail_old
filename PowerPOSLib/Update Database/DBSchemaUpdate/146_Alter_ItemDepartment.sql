IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'DepartmentOrder' AND OBJECT_ID = OBJECT_ID(N'[ItemDepartment]'))
BEGIN
	ALTER TABLE ItemDepartment Add [DepartmentOrder] [int] NULL 
END