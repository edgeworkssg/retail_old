IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'BarcodeCheckValue')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'BarcodeCheckValue','',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'IsReplaceMembershipText')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'IsReplaceMembershipText','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'MembershipTextReplacement')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'MembershipTextReplacement','',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Order_AutoAssignMode')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Order_AutoAssignMode','No Auto Assign',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'WeighingMachine_UseWeighingMachine')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'WeighingMachine_UseWeighingMachine','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'WeighingMachine_COMPort')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'WeighingMachine_COMPort','5',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'WeighingMachine_CommandToExecute')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'WeighingMachine_CommandToExecute','W',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'GuestBookName')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'GuestBookName','',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'IsAskingPassCode')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Points_IsAskingPassCode','',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'BarcodePrinter_Template')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'BarcodePrinter_Template','\PrintTemplate\pnt-Default.bin',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Invoice_UseContainerWeight')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Invoice_UseContainerWeight','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Payment_ShowPaymentTypeWhenZero')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Payment_ShowPaymentTypeWhenZero','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Inventory_CheckQuantityonServer')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Inventory_CheckQuantityonServer','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Discount_AllowZeroMultiTierPrice')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'AllowZeroMultiTierPrice','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Inventory_IncludeGSTExclusive')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Inventory_IncludeGSTExclusive','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Order_ChangeMemberWithoutSupervisorLogin')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Order_ChangeMemberWithoutSupervisorLogin','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Sales_HoldOrderFromServer')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Sales_HoldOrderFromServer','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END


IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Integration_SyncCreateDeliveryOrder')<=0)
BEGIN INSERT AppSetting  VALUES (NEWID(),'Integration_SyncCreateDeliveryOrder','False',NULL,GETDATE(),'UPGRADE',GETDATE(),'UPGRADE') END
