ALTER TABLE UserMst
ALTER COLUMN Userfld1 VARCHAR(MAX)

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SalesCommission' AND [object_id] = OBJECT_ID(N'UserMst'))
BEGIN
    ALTER TABLE UserMst Add SalesCommission uniqueidentifier null
END

-- remove foreign key in beauty db
IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_SalesCommissionRecord_UserMst')
   AND parent_object_id = OBJECT_ID(N'dbo.UserMst')
)
  ALTER TABLE [dbo.UserMst] DROP CONSTRAINT [FK_SalesCommissionRecord_UserMst]
  

IF (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'UserMst' AND COLUMN_NAME = 'DisplayName') = 'varchar'
	ALTER TABLE UserMst ALTER COLUMN DisplayName NVARCHAR(50)
