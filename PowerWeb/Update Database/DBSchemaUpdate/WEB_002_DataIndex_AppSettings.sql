IF (SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'DataIndex_OrderHdr') = 0 BEGIN
	INSERT INTO AppSetting ( AppSettingID, AppSettingKey, AppSettingValue, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy )
	VALUES ( NEWID(), 'DataIndex_OrderHdr','0' ,GETDATE(),'SCRIPT',GETDATE(),'SCRIPT' )
END
IF (SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'DataIndex_OrderDet') = 0 BEGIN
	INSERT INTO AppSetting ( AppSettingID, AppSettingKey, AppSettingValue, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy )
	VALUES ( NEWID(), 'DataIndex_OrderDet','0' ,GETDATE(),'SCRIPT',GETDATE(),'SCRIPT' )
END
IF (SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'DataIndex_ReceiptDet') = 0 BEGIN
	INSERT INTO AppSetting ( AppSettingID, AppSettingKey, AppSettingValue, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy )
	VALUES ( NEWID(), 'DataIndex_ReceiptDet','0' ,GETDATE(),'SCRIPT',GETDATE(),'SCRIPT' )
END