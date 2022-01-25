IF EXISTS (SELECT * FROM sysobjects WHERE xtype = 'D' AND name = 'DF_AppSetting_CreationDate') 
BEGIN
	ALTER TABLE [dbo].[AppSetting] DROP CONSTRAINT [DF_AppSetting_CreationDate]
END

ALTER TABLE AppSetting
ALTER COLUMN AppSettingValue NVARCHAR(MAX)

IF NOT EXISTS (SELECT * FROM sysobjects WHERE xtype = 'D' AND name = 'DF_AppSetting_CreationDate')
BEGIN
	ALTER TABLE [dbo].[AppSetting] 
	ADD  CONSTRAINT [DF_AppSetting_CreationDate]  
	DEFAULT (getutcdate()) FOR [AppSettingValue]
END