IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'version')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'version','2.0',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

