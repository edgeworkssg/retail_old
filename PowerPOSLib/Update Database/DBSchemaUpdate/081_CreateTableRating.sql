IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rating]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Rating](
		[RatingID] [int] IDENTITY(1,1) NOT NULL,
		[POSID] [int] NULL,
		[Rating] [int] NULL,
		[Staff] [varchar](50) NULL,
		[Timestamp] [datetime] NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [varchar](50) NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [varchar](50) NULL,
		[Deleted] [bit] NULL,
		[UniqueId] [varchar](100) NULL,
	 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
	(
		[RatingID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
	 CONSTRAINT [IX_Rating] UNIQUE NONCLUSTERED 
	(
		[RatingID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OrderHdrID' AND [object_id] = OBJECT_ID(N'Rating'))
BEGIN
    ALTER TABLE Rating ADD OrderHdrID [varchar](14) NULL
END
