IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecordData]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RecordData](
		[RecordDataID] [int] IDENTITY(1,1) NOT NULL,
		[InventoryLocationID] [int] NULL,
		[Val1] [varchar](50) NULL,
		[Val2] [varchar](50) NULL,
		[Val3] [varchar](50) NULL,
		[Val4] [varchar](50) NULL,
		[Val5] [varchar](50) NULL,
		[Val6] [varchar](50) NULL,
		[Val7] [varchar](50) NULL,
		[Val8] [varchar](50) NULL,
		[Val9] [varchar](50) NULL,
		[Val10] [varchar](50) NULL,
		[InventoryHdrRefNo] [varchar](50) NULL,
		[Timestamp] [datetime] NULL,
		[CreatedOn] [datetime] NULL,
		[CreatedBy] [varchar](50) NULL,
		[ModifiedOn] [datetime] NULL,
		[ModifiedBy] [varchar](50) NULL,
		[Deleted] [bit] NULL,
		[UniqueId] [varchar](100) NULL,
	 CONSTRAINT [PK_RecordData] PRIMARY KEY CLUSTERED 
	(
		[RecordDataID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
	 CONSTRAINT [IX_RecordedData] UNIQUE NONCLUSTERED 
	(
		[RecordDataID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END