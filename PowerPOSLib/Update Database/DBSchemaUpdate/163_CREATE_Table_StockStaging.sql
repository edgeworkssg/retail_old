IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[StockStaging]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[StockStaging](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[InventoryDate] [datetime] NULL,
	[ItemNo] [varchar](50) NULL,
	[InventoryLocationID] [int] NULL,
	[BalanceQty] [float] NULL,
	[CostPriceByItem] [money] NULL,
	[CostPriceByItemInvLoc] [money] NULL,
	[CostPriceByItemInvGroup] [money] NULL,
 CONSTRAINT [PK_StockStaging] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END



