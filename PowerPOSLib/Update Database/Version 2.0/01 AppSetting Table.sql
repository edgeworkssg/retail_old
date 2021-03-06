/*
   Thursday, 10 February, 20112:47:26 PM
   User: 
   Server: localhost\SQLR2
   Database: devRetail20
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppSetting]') AND type in (N'U'))
BEGIN
	BEGIN TRANSACTION
	SET QUOTED_IDENTIFIER ON
	SET ARITHABORT ON
	SET NUMERIC_ROUNDABORT OFF
	SET CONCAT_NULL_YIELDS_NULL ON
	SET ANSI_NULLS ON
	SET ANSI_PADDING ON
	SET ANSI_WARNINGS ON
	COMMIT
	BEGIN TRANSACTION
	GO
	ALTER TABLE dbo.Outlet SET (LOCK_ESCALATION = TABLE)
	GO
	COMMIT
	BEGIN TRANSACTION
	GO
	ALTER TABLE dbo.AppSetting
		DROP CONSTRAINT DF_AppSetting_AppSettingId
	GO
	ALTER TABLE dbo.AppSetting
		DROP CONSTRAINT DF_AppSetting_LastEditDate
	GO
	ALTER TABLE dbo.AppSetting
		DROP CONSTRAINT DF_AppSetting_CreationDate
	GO
	CREATE TABLE dbo.Tmp_AppSetting
		(
		AppSettingId uniqueidentifier NOT NULL,
		AppSettingKey varchar(50) NOT NULL,
		AppSettingValue varchar(50) NOT NULL,
		OutletName varchar(50) NULL,
		CreatedOn datetime NULL,
		CreatedBy varchar(50) NULL,
		ModifiedOn datetime NULL,
		ModifiedBy varchar(50) NULL
		)  ON [PRIMARY]
	GO
	ALTER TABLE dbo.Tmp_AppSetting SET (LOCK_ESCALATION = TABLE)
	GO
	ALTER TABLE dbo.Tmp_AppSetting ADD CONSTRAINT
		DF_AppSetting_AppSettingId DEFAULT (newid()) FOR AppSettingId
	GO
	ALTER TABLE dbo.Tmp_AppSetting ADD CONSTRAINT
		DF_AppSetting_LastEditDate DEFAULT (getutcdate()) FOR AppSettingKey
	GO
	ALTER TABLE dbo.Tmp_AppSetting ADD CONSTRAINT
		DF_AppSetting_CreationDate DEFAULT (getutcdate()) FOR AppSettingValue
	GO
	IF EXISTS(SELECT * FROM dbo.AppSetting)
		 EXEC('INSERT INTO dbo.Tmp_AppSetting (AppSettingId, AppSettingKey, AppSettingValue, OutletName, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
			SELECT AppSettingId,''UNIDENTIFIED'',''UNIDENTIFIED'',''UNIDENTIFIED'', CONVERT(varchar(50), CreationDate), NULL, CONVERT(varchar(50), LastEditDate), NULL FROM dbo.AppSetting WITH (HOLDLOCK TABLOCKX)')
	GO
	DROP TABLE dbo.AppSetting
	GO
	EXECUTE sp_rename N'dbo.Tmp_AppSetting', N'AppSetting', 'OBJECT' 
	GO
	ALTER TABLE dbo.AppSetting ADD CONSTRAINT
		PK_AppSetting PRIMARY KEY CLUSTERED 
		(
		AppSettingId
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	GO
	ALTER TABLE dbo.AppSetting WITH NOCHECK ADD CONSTRAINT
		FK_AppSetting_Outlet FOREIGN KEY
		(
		OutletName
		) REFERENCES dbo.Outlet
		(
		OutletName
		) ON UPDATE  CASCADE 
		 ON DELETE  CASCADE 
		
	GO
	CREATE TRIGGER [AppSetting_UpdateTrigger] 
		ON dbo.AppSetting 
		AFTER UPDATE 
	AS 
	BEGIN 
		SET NOCOUNT ON 
		UPDATE [AppSetting] 
		SET [ModifiedOn] = GETUTCDATE() 
		FROM inserted 
		WHERE inserted.[AppSettingId] = [AppSetting].[AppSettingId] 
	END;
	GO
	CREATE TRIGGER [AppSetting_InsertTrigger] 
		ON dbo.AppSetting 
		AFTER INSERT 
	AS 
	BEGIN 
		SET NOCOUNT ON 
		UPDATE [AppSetting] 
		SET [CreatedOn] = GETUTCDATE(), [ModifiedOn] = GETUTCDATE() 
		FROM inserted 
		WHERE inserted.[AppSettingId] = [AppSetting].[AppSettingId] 
	END;
	GO
	COMMIT
END