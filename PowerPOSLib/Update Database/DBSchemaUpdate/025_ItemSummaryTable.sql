IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemSummary](
	[ItemSummaryID] [varchar](18) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[InventoryLocationID] [int] NULL,
	[BalanceQty] [int] NULL,
	[CostPrice] [money] NULL,		
	[Remark] [nvarchar](max) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[UniqueID] [uniqueidentifier] NOT NULL,
	[Deleted] [bit] NULL,
	[userfld1] [varchar](50) NULL,	
	[userfld2] [varchar](50) NULL,
	[userfld3] [varchar](50) NULL,
	[userfld4] [varchar](50) NULL,
	[userfld5] [varchar](50) NULL,
	[userfld6] [varchar](50) NULL,
	[userfld7] [varchar](50) NULL,
	[userfld8] [varchar](50) NULL,
	[userfld9] [varchar](50) NULL,
	[userfld10] [varchar](50) NULL,
	[userflag1] [bit] NULL,
	[userflag2] [bit] NULL,
	[userflag3] [bit] NULL,
	[userflag4] [bit] NULL,
	[userflag5] [bit] NULL,
	[userfloat1] [money] NULL,
	[userfloat2] [money] NULL,
	[userfloat3] [money] NULL,
	[userfloat4] [money] NULL,
	[userfloat5] [money] NULL,
	[userint1] [int] NULL,
	[userint2] [int] NULL,
	[userint3] [int] NULL,
	[userint4] [int] NULL,
	[userint5] [int] NULL
 CONSTRAINT [PK_ItemSummary] PRIMARY KEY CLUSTERED 
(
	[ItemSummaryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[ItemSummary]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemSummary_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])
ON UPDATE CASCADE
END

ALTER TABLE ItemSummary ALTER COLUMN BalanceQty FLOAT NULL

IF EXISTS
    (SELECT *
    FROM    sys.columns c
    WHERE   c.object_id = OBJECT_ID('dbo.ItemSummary')
    AND c.name = 'ItemSummaryID'
    AND c.system_type_id = 167 
    AND c.max_length = 18)
BEGIN
	ALTER TABLE ItemSummary Drop Constraint PK_ItemSummary
    ALTER TABLE ItemSummary ALTER COLUMN ItemSummaryID varchar(50) NOT NULL
    ALTER TABLE ItemSummary 
	ADD CONSTRAINT [PK_ItemSummary] PRIMARY KEY CLUSTERED (ItemSummaryID);
END