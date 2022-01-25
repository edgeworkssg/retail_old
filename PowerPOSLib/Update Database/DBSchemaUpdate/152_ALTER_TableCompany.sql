IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'Address1'
) BEGIN
    ALTER TABLE Company ADD Address1 VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'Address2'
) BEGIN
    ALTER TABLE Company ADD Address2 VARCHAR(50) NULL
END


IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'City'
) BEGIN
    ALTER TABLE Company ADD City VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'Country'
) BEGIN
    ALTER TABLE Company ADD Country VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'ZipCode'
) BEGIN
    ALTER TABLE Company ADD ZipCode VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'Mobile'
) BEGIN
    ALTER TABLE Company ADD Mobile VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'Fax'
) BEGIN
    ALTER TABLE Company ADD Fax VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'CompanyImage'
) BEGIN
    ALTER TABLE Company ADD CompanyImage varbinary(max) NULL
END


IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfld1'
) BEGIN
    ALTER TABLE Company ADD userfld1 VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfld2'
) BEGIN
    ALTER TABLE Company ADD userfld2 VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfld3'
) BEGIN
    ALTER TABLE Company ADD userfld3 VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfld4'
) BEGIN
    ALTER TABLE Company ADD userfld4 VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfld5'
) BEGIN
    ALTER TABLE Company ADD userfld5 VARCHAR(50) NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userflag1'
) BEGIN
    ALTER TABLE Company ADD userflag1 bit NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userflag2'
) BEGIN
    ALTER TABLE Company ADD userflag2 bit NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userflag3'
) BEGIN
    ALTER TABLE Company ADD userflag3 bit NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userflag4'
) BEGIN
    ALTER TABLE Company ADD userflag4 bit NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userflag5'
) BEGIN
    ALTER TABLE Company ADD userflag5 bit NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfloat1'
) BEGIN
    ALTER TABLE Company ADD userfloat1 money NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfloat2'
) BEGIN
    ALTER TABLE Company ADD userfloat2 money NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfloat3'
) BEGIN
    ALTER TABLE Company ADD userfloat3 money NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfloat4'
) BEGIN
    ALTER TABLE Company ADD userfloat4 money NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userfloat5'
) BEGIN
    ALTER TABLE Company ADD userfloat5 money NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userint1'
) BEGIN
    ALTER TABLE Company ADD userint1 int NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userint2'
) BEGIN
    ALTER TABLE Company ADD userint2 int NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userint3'
) BEGIN
    ALTER TABLE Company ADD userint3 int NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userint4'
) BEGIN
    ALTER TABLE Company ADD userint4 int NULL
END

IF NOT EXISTS(
    SELECT * FROM information_schema.columns 
    WHERE Table_Schema = 'dbo'
    AND Table_Name = 'Company'
    AND Column_Name = 'userint5'
) BEGIN
    ALTER TABLE Company ADD userint5 int NULL
END