IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffAssistLog]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[StaffAssistLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime] NULL,
	[Type] [varchar](50) NULL,
	[Msg] [varchar](max) NULL,
	[PointOfSaleID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END