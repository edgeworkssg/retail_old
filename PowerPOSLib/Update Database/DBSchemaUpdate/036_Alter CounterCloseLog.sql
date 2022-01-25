IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat6' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat6 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat7' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat7 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat8' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat8 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat9' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat9 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat10' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat10 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat11' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat11 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat12' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat12 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat13' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat13 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat14' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat14 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat15' AND [object_id] = OBJECT_ID(N'CounterCloseLog'))
BEGIN
    ALTER TABLE CounterCloseLog ADD userfloat15 money NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'NetsCashCardCollected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD NetsCashCardCollected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'NetsCashCardRecorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD NetsCashCardRecorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'NetsFlashPayCollected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD NetsFlashPayCollected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'NetsFlashPayRecorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD NetsFlashPayRecorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'NetsATMCardCollected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD NetsATMCardCollected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'NetsATMCardRecorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD NetsATMCardRecorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency1'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency1 NVARCHAR(200) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency1Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency1Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency1Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency1Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency2'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency2 NVARCHAR(200) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency2Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency2Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency2Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency2Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency3'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency3 NVARCHAR(200) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency3Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency3Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency3Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency3Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency4'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency4 NVARCHAR(200) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency4Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency4Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency4Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency4Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency5'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency5 NVARCHAR(200) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency5Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency5Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'ForeignCurrency5Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD ForeignCurrency5Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'TotalForeignCurrency'
) BEGIN
    ALTER TABLE CounterCloseLog ADD TotalForeignCurrency MONEY NULL
END


IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay7Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay7Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay7Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay7Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay8Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay8Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay8Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay8Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay9Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay9Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay9Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay9Recorded MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay10Collected'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay10Collected MONEY NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'CounterCloseLog'
    AND Column_Name = 'Pay10Recorded'
) BEGIN
    ALTER TABLE CounterCloseLog ADD Pay10Recorded MONEY NULL
END