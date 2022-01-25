/* 
==============================
INSTALLMENT TABLE
==============================
*/

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CeatedOn' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment DROP COLUMN CeatedOn
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CreatedOn' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD CreatedOn datetime NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CurrentBalance' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD CurrentBalance money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld1' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld1 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld2' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld2 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld3' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld3 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld4' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld4 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld5' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld5 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld6' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld6 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld7' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld7 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld8' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld8 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld9' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld9 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld10' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfld10 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag1' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userflag1 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag2' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userflag2 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag3' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userflag3 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag4' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userflag4 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag5' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userflag5 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat1' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfloat1 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat2' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfloat2 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat3' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfloat3 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat4' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfloat4 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat5' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userfloat5 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint1' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userint1 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint2' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userint2 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint3' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userint3 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint4' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userint4 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint5' AND [object_id] = OBJECT_ID(N'Installment'))
BEGIN
    ALTER TABLE Installment ADD userint5 int NULL
END

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME='FK_Installment_OrderHdr')
BEGIN
	ALTER TABLE Installment DROP CONSTRAINT FK_Installment_OrderHdr
END

/* 
==============================
INSTALLMENTDETAIL TABLE
==============================
*/

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OrderHdrID' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD OrderHdrID varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld1' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld1 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld2' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld2 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld3' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld3 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld4' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld4 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld5' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld5 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld6' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld6 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld7' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld7 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld8' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld8 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld9' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld9 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfld10' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfld10 varchar(50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag1' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userflag1 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag2' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userflag2 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag3' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userflag3 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag4' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userflag4 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userflag5' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userflag5 bit NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat1' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfloat1 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat2' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfloat2 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat3' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfloat3 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat4' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfloat4 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userfloat5' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userfloat5 money NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint1' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userint1 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint2' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userint2 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint3' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userint3 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint4' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userint4 int NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'userint5' AND [object_id] = OBJECT_ID(N'InstallmentDetail'))
BEGIN
    ALTER TABLE InstallmentDetail ADD userint5 int NULL
END
