IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GuestBookCompulsory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GuestBookCompulsory](
	[GuestBookCompulsoryID] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [varchar](50) NULL,
	[IsCompulsory] [bit] NULL,
	[IsVisible] [bit] NULL,
	[Remark] [varchar](max) NULL,
	[Deleted] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_GuestBookCompulsory] PRIMARY KEY CLUSTERED 
(
	[GuestBookCompulsoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Label' AND OBJECT_ID = OBJECT_ID(N'[GuestBookCompulsory]'))
BEGIN
	ALTER TABLE GuestBookCompulsory Add [Label] [nvarchar] (100) NULL 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfld1' AND OBJECT_ID = OBJECT_ID(N'[GuestBookCompulsory]'))
BEGIN
	ALTER TABLE GuestBookCompulsory Add [userfld1] [varchar] (50) NULL 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfld2' AND OBJECT_ID = OBJECT_ID(N'[GuestBookCompulsory]'))
BEGIN
	ALTER TABLE GuestBookCompulsory Add [userfld2] [varchar] (50) NULL 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfld3' AND OBJECT_ID = OBJECT_ID(N'[GuestBookCompulsory]'))
BEGIN
	ALTER TABLE GuestBookCompulsory Add [userfld3] [varchar] (50) NULL 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfld4' AND OBJECT_ID = OBJECT_ID(N'[GuestBookCompulsory]'))
BEGIN
	ALTER TABLE GuestBookCompulsory Add [userfld4] [varchar] (50) NULL 
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'userfld5' AND OBJECT_ID = OBJECT_ID(N'[GuestBookCompulsory]'))
BEGIN
	ALTER TABLE GuestBookCompulsory Add [userfld5] [varchar] (50) NULL 
END

