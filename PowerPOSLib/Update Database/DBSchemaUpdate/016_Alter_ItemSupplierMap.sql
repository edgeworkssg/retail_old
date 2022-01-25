IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[ItemSupplierMap]') AND type in (N'U'))

BEGIN


CREATE TABLE [dbo].[ItemSupplierMap](
	[ItemSupplierMapID] [int] IDENTITY(1,1) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[SupplierID] [int] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NOT NULL,
	[Currency] [nvarchar](50) NULL,
	[GST] [decimal](18, 3) NULL,
	[GSTRule] [int] NULL,
	[PackingSize1] [nvarchar](max) NULL,
	[PackingSize2] [nvarchar](max) NULL,
	[PackingSize3] [nvarchar](max) NULL,
	[PackingSize4] [nvarchar](max) NULL,
	[PackingSize5] [nvarchar](max) NULL,
	[PackingSize6] [nvarchar](max) NULL,
	[PackingSize7] [nvarchar](max) NULL,
	[PackingSize8] [nvarchar](max) NULL,
	[PackingSize9] [nvarchar](max) NULL,
	[PackingSize10] [nvarchar](max) NULL,
	[PackingSizeUOM1] [decimal](18, 3) NULL,
	[PackingSizeUOM2] [decimal](18, 3) NULL,
	[PackingSizeUOM3] [decimal](18, 3) NULL,
	[PackingSizeUOM4] [decimal](18, 3) NULL,
	[PackingSizeUOM5] [decimal](18, 3) NULL,
	[PackingSizeUOM6] [decimal](18, 3) NULL,
	[PackingSizeUOM7] [decimal](18, 3) NULL,
	[PackingSizeUOM8] [decimal](18, 3) NULL,
	[PackingSizeUOM9] [decimal](18, 3) NULL,
	[PackingSizeUOM10] [decimal](18, 3) NULL,
	[CostPrice1] [money] NULL,
	[CostPrice2] [money] NULL,
	[CostPrice3] [money] NULL,
	[CostPrice4] [money] NULL,
	[CostPrice5] [money] NULL,
	[CostPrice6] [money] NULL,
	[CostPrice7] [money] NULL,
	[CostPrice8] [money] NULL,
	[CostPrice9] [money] NULL,
	[CostPrice10] [money] NULL,
 CONSTRAINT [PK_ItemSupplierMap] PRIMARY KEY CLUSTERED 
(
	[ItemSupplierMapID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[ItemSupplierMap]  WITH CHECK ADD  CONSTRAINT [FK_Item_ItemSupplierMap] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])


ALTER TABLE [dbo].[ItemSupplierMap] CHECK CONSTRAINT [FK_Item_ItemSupplierMap]


ALTER TABLE [dbo].[ItemSupplierMap]  WITH CHECK ADD  CONSTRAINT [FK_ItemSupplierMap_Supplier] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Supplier] ([SupplierID])


ALTER TABLE [dbo].[ItemSupplierMap] CHECK CONSTRAINT [FK_ItemSupplierMap_Supplier]

END

-- add column if it doesn't exist, for migration wholesale to retail 
IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Currency' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [Currency] [nvarchar](50) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'GST' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [GST] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'GSTRule' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [GSTRule] [int] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize1' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize1] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize2' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize2] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize3' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize3] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize4' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize4] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize5' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize5] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize6' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize6] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize7' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize7] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize8' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize8] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize9' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize9] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSize10' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSize10] [nvarchar](max) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM1' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM1] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM2' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM2] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM3' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM3] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM4' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM4] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM5' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM5] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM6' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM6] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM7' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM7] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM8' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM8] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM9' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM9] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PackingSizeUOM10' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [PackingSizeUOM10] [decimal](18, 3) NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice1' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice1] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice2' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice2] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice3' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice3] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice4' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice4] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice5' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice5] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice6' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice6] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice7' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice7] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice8' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice8] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice9' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice9] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CostPrice10' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [CostPrice10] [money] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'IsPreferredSupplier' AND [object_id] = OBJECT_ID(N'ItemSupplierMap'))
BEGIN
	ALTER TABLE ItemSupplierMap ADD [IsPreferredSupplier] [bit] NULL
END

