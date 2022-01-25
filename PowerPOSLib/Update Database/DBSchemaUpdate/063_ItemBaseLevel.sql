IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[ItemBaseLevel]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[ItemBaseLevel](
	[BaseLevelID] [int] IDENTITY(1,1) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[BaseLevelQuantity] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NOT NULL,
	[InventoryLocationID] [int] NULL,
 CONSTRAINT [PK_ItemBaseLevel] PRIMARY KEY CLUSTERED 
(
	[BaseLevelID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemBaseLevel_InventoryLocation]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[ItemBaseLevel]  WITH CHECK ADD  CONSTRAINT [FK_ItemBaseLevel_InventoryLocation] FOREIGN KEY([InventoryLocationID])
REFERENCES [dbo].[InventoryLocation] ([InventoryLocationID])
END

ALTER TABLE [dbo].[ItemBaseLevel] CHECK CONSTRAINT [FK_ItemBaseLevel_InventoryLocation];


IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemBaseLevel_Item]') AND type in (N'F'))
BEGIN
ALTER TABLE [dbo].[ItemBaseLevel]  WITH CHECK ADD  CONSTRAINT [FK_ItemBaseLevel_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])
END

ALTER TABLE [dbo].[ItemBaseLevel] CHECK CONSTRAINT [FK_ItemBaseLevel_Item];

IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[DF_ItemBaseLevel_Deleted]') )
BEGIN
ALTER TABLE [dbo].[ItemBaseLevel] ADD  CONSTRAINT [DF_ItemBaseLevel_Deleted]  DEFAULT ((0)) FOR [Deleted]
END

IF NOT EXISTS (
    SELECT * FROM sys.views v
    INNER JOIN sys.schemas s on v.schema_id = s.schema_id
    WHERE s.name = 'dbo' and v.name = 'viewitembaselevel'
)
BEGIN
    EXEC sp_executesql @statement = N'create View viewitembaselevel as 
			SELECT q.BaseLevelID, q.ItemNo, i.ItemName, q.BaseLevelQuantity, q.InventoryLocationId, il.InventoryLocationName,
                    c.CategoryName as Category, i.userfld1 as UOM   
                FROM ItemBaseLevel q, Item i, Category c, InventoryLocation il 
                WHERE q.ItemNo = i.ItemNo and q.InventoryLocationId = il.InventoryLocationId and i.categoryname = c.categoryname and 
                ISNULL(q.Deleted,0) <> 1' ;

END
