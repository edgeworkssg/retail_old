IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'ResourceID'
) BEGIN
    ALTER TABLE Appointment ADD ResourceID VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'CheckInByWho'
) BEGIN
    ALTER TABLE Appointment ADD CheckInByWho NVARCHAR(50) NULL
END


IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'CheckOutByWho'
) BEGIN
    ALTER TABLE Appointment ADD CheckOutByWho NVARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'CheckInTime'
) BEGIN
    ALTER TABLE Appointment ADD CheckInTime datetime NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'CheckOutTime'
) BEGIN
    ALTER TABLE Appointment ADD CheckOutTime datetime NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'Remark'
) BEGIN
    ALTER TABLE Appointment ADD Remark NVARCHAR(MAX) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'IsServerUpdate'
) BEGIN
    ALTER TABLE Appointment ADD IsServerUpdate bit NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Appointment'
    AND Column_Name = 'TimeExtension'
) BEGIN
    ALTER TABLE Appointment ADD TimeExtension int NULL
END