SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemCookHistory](
	[ItemCookHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[CookDate] [datetime] NULL,
	[ItemNo] [varchar](50) NULL,
	[Quantity] [decimal](38, 8) NULL,
	[PointOfSaleID] [int] NULL,
	[InventoryLocationID] [int] NULL,
	[Description] [varchar](250) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NOT NULL,
	[UniqueID] [uniqueidentifier] NOT NULL,
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
	[userint5] [int] NULL,
 CONSTRAINT [PK_ItemCookHistory] PRIMARY KEY CLUSTERED 
(
	[ItemCookHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCookHistory_InventoryLocation]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]'))
ALTER TABLE [dbo].[ItemCookHistory]  WITH CHECK ADD  CONSTRAINT [FK_ItemCookHistory_InventoryLocation] FOREIGN KEY([InventoryLocationID])
REFERENCES [dbo].[InventoryLocation] ([InventoryLocationID])

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCookHistory_InventoryLocation]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]'))
ALTER TABLE [dbo].[ItemCookHistory] CHECK CONSTRAINT [FK_ItemCookHistory_InventoryLocation]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCookHistory_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]'))
ALTER TABLE [dbo].[ItemCookHistory]  WITH CHECK ADD  CONSTRAINT [FK_ItemCookHistory_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCookHistory_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]'))
ALTER TABLE [dbo].[ItemCookHistory] CHECK CONSTRAINT [FK_ItemCookHistory_Item]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCookHistory_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]'))
ALTER TABLE [dbo].[ItemCookHistory]  WITH CHECK ADD  CONSTRAINT [FK_ItemCookHistory_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [dbo].[PointOfSale] ([PointOfSaleID])

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCookHistory_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCookHistory]'))
ALTER TABLE [dbo].[ItemCookHistory] CHECK CONSTRAINT [FK_ItemCookHistory_PointOfSale]

