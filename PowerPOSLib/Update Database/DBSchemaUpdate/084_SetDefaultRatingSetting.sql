IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_GreetingText')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_GreetingText','Please rate my service','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_FooterText')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_FooterText','Thank you for your feedback','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END


IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_ThankYouGoodRating')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_ThankYouGoodRating','Thank you for your feedback','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_ThankYouBadRating')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_ThankYouBadRating','Thank you for your feedback. We will contact you shortly','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_ThankYouInterval')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_ThankYouInterval','5','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_AllowGoodRatingFeedback')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_AllowGoodRatingFeedback','True','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_GoodFeedbackGreeting')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_GoodFeedbackGreeting','Please share with us what we have done well','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END


IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_AllowBadRatingFeedback')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_AllowBadRatingFeedback','True','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END

IF((SELECT COUNT(*) FROM AppSetting WHERE AppSettingKey = 'Rating_BadFeedbackGreeting')<=0)
BEGIN 
INSERT into AppSetting (AppSettingId, AppSettingKey, AppSettingValue, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn) 
VALUES (NewID(),'Rating_BadFeedbackGreeting','Please share with the reason for your selection','SCRIPT',GETDATE(),'SCRIPT',GETDATE()) 
END
