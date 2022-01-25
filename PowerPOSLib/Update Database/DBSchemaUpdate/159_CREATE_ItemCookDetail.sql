IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemCookDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemCookDetail](
	[ItemCookDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ItemCookHistoryID] [int] NULL,
	[ItemNo] [varchar](50) NULL,
	[UOM] [varchar](50) NULL,
	[Qty] [decimal](18, 5) NULL,
	[OriginalQty] [decimal](18, 5) NULL,
	[UnitPrice] [decimal](18, 5) NULL,
	[TotalCost] [decimal](18, 5) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
	[Userfld1] [varchar](50) NULL,
	[Userfld2] [varchar](50) NULL,
	[Userfld3] [varchar](50) NULL,
	[Userfld4] [varchar](50) NULL,
	[Userfld5] [varchar](50) NULL,
	[Userfloat1] [decimal](18, 2) NULL,
	[Userfloat2] [decimal](18, 2) NULL,
	[Userfloat3] [decimal](18, 2) NULL,
	[Userfloat4] [decimal](18, 2) NULL,
	[Userfloat5] [decimal](18, 2) NULL,
	[Userint1] [int] NULL,
	[Userint2] [int] NULL,
	[Userint3] [int] NULL,
	[Userint4] [int] NULL,
	[Userint5] [int] NULL,
	[Userflag1] [bit] NULL,
	[Userflag2] [bit] NULL,
	[Userflag3] [bit] NULL,
	[Userflag4] [bit] NULL,
	[Userflag5] [bit] NULL,
 CONSTRAINT [PK_ItemCookDetail] PRIMARY KEY CLUSTERED 
(
	[ItemCookDetailId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[ItemCookDetail]  WITH CHECK ADD  CONSTRAINT [FK_ItemCookDetail_ItemCookDetail] FOREIGN KEY([ItemCookDetailId])
REFERENCES [dbo].[ItemCookDetail] ([ItemCookDetailId])

ALTER TABLE [dbo].[ItemCookDetail] CHECK CONSTRAINT [FK_ItemCookDetail_ItemCookDetail]


ALTER TABLE [dbo].[ItemCookDetail]  WITH CHECK ADD  CONSTRAINT [FK_ItemCookDetail_ItemCookHistory] FOREIGN KEY([ItemCookHistoryID])
REFERENCES [dbo].[ItemCookHistory] ([ItemCookHistoryID])


ALTER TABLE [dbo].[ItemCookDetail] CHECK CONSTRAINT [FK_ItemCookDetail_ItemCookHistory]
END


