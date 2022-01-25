IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GuestBook]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GuestBook](
		[GuestBookID] [int] IDENTITY(1,1) NOT NULL,
		[MembershipNo] [varchar](50) NULL,
		[InTime] [datetime] NULL,
		[OutTime] [datetime] NULL,
		[CreatedBy] [varchar](50) NULL,
		[CreatedOn] [datetime] NULL,
		[ModifiedBy] [varchar](50) NULL,
		[ModifiedOn] [datetime] NULL,
		[Deleted] [bit] NULL,
	 CONSTRAINT [PK_GuestBook] PRIMARY KEY CLUSTERED 
	(
		[GuestBookID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END


