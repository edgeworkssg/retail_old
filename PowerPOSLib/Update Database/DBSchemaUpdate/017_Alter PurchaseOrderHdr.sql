﻿ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld1 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld2 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld3 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld4 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld5 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld6 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld7 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld8 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld9 NVARCHAR(MAX)
ALTER TABLE PurchaseOrderHdr ALTER COLUMN Userfld10 NVARCHAR(MAX)

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CustomRefNo' AND OBJECT_ID = OBJECT_ID(N'[PurchaseOrderHdr]'))
BEGIN
ALTER TABLE dbo.PurchaseOrderHdr ADD
    CustomRefNo varchar(50) NULL
END
