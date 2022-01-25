IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[ItemTagSummary]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[ItemTagSummary](
	[ItemTagSummaryID] [varchar](500) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[SerialNo] [varchar](200) NULL,
	[InventoryLocationID] [int] NULL,
	[IsAvailable] [bit] NULL,
	[Remark] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](500) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](500) NULL,
	[ModifiedOn] [datetime] NULL,
	[UniqueID] [uniqueidentifier] NOT NULL,
	[Deleted] [bit] NULL,
	[Userfld1] [nvarchar](500) NULL,
	[Userfld2] [nvarchar](500) NULL,
	[Userfld3] [nvarchar](500) NULL,
	[Userfld4] [nvarchar](500) NULL,
	[Userfld5] [nvarchar](500) NULL,
	[Userfld6] [nvarchar](500) NULL,
	[Userfld7] [nvarchar](500) NULL,
	[Userfld8] [nvarchar](500) NULL,
	[Userfld9] [nvarchar](500) NULL,
	[Userfld10] [nvarchar](500) NULL,
	[Userflag1] [bit] NULL,
	[Userflag2] [bit] NULL,
	[Userflag3] [bit] NULL,
	[Userflag4] [bit] NULL,
	[Userflag5] [bit] NULL,
	[Userfloat1] [money] NULL,
	[Userfloat2] [money] NULL,
	[Userfloat3] [money] NULL,
	[Userfloat4] [money] NULL,
	[Userfloat5] [money] NULL,
	[Userint1] [int] NULL,
	[Userint2] [int] NULL,
	[Userint3] [int] NULL,
	[Userint4] [int] NULL,
	[Userint5] [int] NULL,
 CONSTRAINT [PK_ItemTagSummary] PRIMARY KEY CLUSTERED 
(
	[ItemTagSummaryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[ItemTagSummary]  WITH CHECK ADD  CONSTRAINT [FK_ItemTagSummary_InventoryLocation] FOREIGN KEY([InventoryLocationID])
REFERENCES [dbo].[InventoryLocation] ([InventoryLocationID])
ON UPDATE CASCADE
ON DELETE CASCADE

ALTER TABLE [dbo].[ItemTagSummary]  WITH CHECK ADD  CONSTRAINT [FK_ItemTagSummary_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])
ON UPDATE CASCADE
ON DELETE CASCADE

END
