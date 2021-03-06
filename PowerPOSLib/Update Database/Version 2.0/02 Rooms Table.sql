/*
   Thursday, 10 February, 20113:04:18 PM
   User: 
   Server: localhost\SQLR2
   Database: devRetail20
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Outlet SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Building
	(
	Building_Name varchar(50) NOT NULL,
	City varchar(50) NULL,
	Country varchar(15) NULL,
	Address_Line_1 varchar(150) NULL,
	Address_Line_2 varchar(150) NULL,
	PinCode int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Building SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.Building)
	 EXEC('INSERT INTO dbo.Tmp_Building (Building_Name, City, Country, Address_Line_1, Address_Line_2, PinCode)
		SELECT CONVERT(varchar(50), Building_Name), City, Country, Address_Line_1, Address_Line_2, PinCode FROM dbo.Building WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.Rooms
	DROP CONSTRAINT FK_Rooms_Building
GO
DROP TABLE dbo.Building
GO
EXECUTE sp_rename N'dbo.Tmp_Building', N'Building', 'OBJECT' 
GO
ALTER TABLE dbo.Building ADD CONSTRAINT
	PK_Building_1 PRIMARY KEY CLUSTERED 
	(
	Building_Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Rooms
	(
	RoomID int NOT NULL IDENTITY (1, 1),
	Room_Name varchar(150) NOT NULL,
	OutletName varchar(50) NULL,
	CreatedBy varchar(50) NULL,
	CreatedOn datetime NULL,
	ModifiedBy varchar(50) NULL,
	ModifiedOn datetime NULL,
	Deleted bit NULL,
	userfld1 varchar(50) NULL,
	userfld2 varchar(50) NULL,
	userfld3 varchar(50) NULL,
	userfld4 varchar(50) NULL,
	userfld5 varchar(50) NULL,
	userfld6 varchar(50) NULL,
	userfld7 varchar(50) NULL,
	userfld8 varchar(50) NULL,
	userfld9 varchar(50) NULL,
	userfld10 varchar(50) NULL,
	userflag1 bit NULL,
	userflag2 bit NULL,
	userflag3 bit NULL,
	userflag4 bit NULL,
	userflag5 bit NULL,
	userfloat1 money NULL,
	userfloat2 money NULL,
	userfloat3 money NULL,
	userfloat4 money NULL,
	userfloat5 money NULL,
	userint1 int NULL,
	userint2 int NULL,
	userint3 int NULL,
	userint4 int NULL,
	userint5 int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Rooms SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Rooms ON
GO
IF EXISTS(SELECT * FROM dbo.Rooms)
	 EXEC('INSERT INTO dbo.Tmp_Rooms (RoomID, Room_Name, OutletName)
		SELECT ID, Room_Name, NULL FROM dbo.Rooms WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Rooms OFF
GO
ALTER TABLE dbo.Course
	DROP CONSTRAINT FK_Course_Rooms
GO
ALTER TABLE dbo.Rooms_Time_Slots
	DROP CONSTRAINT FK_Rooms_Time_Slots_Rooms
GO
DROP TABLE dbo.Rooms
GO
EXECUTE sp_rename N'dbo.Tmp_Rooms', N'Rooms', 'OBJECT' 
GO
ALTER TABLE dbo.Rooms ADD CONSTRAINT
	PK_Rooms PRIMARY KEY CLUSTERED 
	(
	RoomID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Rooms ADD CONSTRAINT
	FK_Rooms_Outlet FOREIGN KEY
	(
	OutletName
	) REFERENCES dbo.Outlet
	(
	OutletName
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Rooms_Time_Slots ADD CONSTRAINT
	FK_Rooms_Time_Slots_Rooms FOREIGN KEY
	(
	Room_ID
	) REFERENCES dbo.Rooms
	(
	RoomID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Rooms_Time_Slots SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Course ADD CONSTRAINT
	FK_Course_Rooms FOREIGN KEY
	(
	Place
	) REFERENCES dbo.Rooms
	(
	RoomID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Course SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
