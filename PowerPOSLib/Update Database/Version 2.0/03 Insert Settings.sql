IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'version')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'version','2.0',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Appointment_IsAvailable')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Appointment_IsAvailable','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Google_UserID')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Google_UserID','',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Google_Password')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Google_Password','',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'DataCollector_Column1')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'DataCollector_Column1','Barcode',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'DataCollector_Column2')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'DataCollector_Column2','Qty',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'UseXMLForHotKey')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'UseXMLForHotKey','No',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'ShowLineSalesPersonOnReceipt')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'ShowLineSalesPersonOnReceipt','No',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'PromptPassword_CheckOut')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'PromptPassword_CheckOut','Yes',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'PromptPassword_CashRecording')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'PromptPassword_CashRecording','Yes',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'PromptPassword_Void')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'PromptPassword_Void','Yes',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
