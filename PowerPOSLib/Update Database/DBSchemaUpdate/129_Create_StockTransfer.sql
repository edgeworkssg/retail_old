IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StockTransferHdr]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[StockTransferHdr](
	[StockTransferHdrRefNo] [varchar](50) NOT NULL,
	[TransferDate] [datetime] NOT NULL,
	[RequiredDate] [datetime] NOT NULL,
	[TransferFromLocationID] [int] NULL,
	[TransferToLocationID] [int] NULL,
	[UserName] [varchar](50) NULL,
	[Remark] [nvarchar](max) NULL,
	[Status] [nvarchar](50) NULL,
	[Deleted] [bit] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[userfld1] [nvarchar](max) NULL,
	[userfld2] [nvarchar](max) NULL,
	[userfld3] [nvarchar](max) NULL,
	[userfld4] [nvarchar](max) NULL,
	[userfld5] [nvarchar](max) NULL,
	[userfld6] [nvarchar](max) NULL,
	[userfld7] [nvarchar](max) NULL,
	[userfld8] [nvarchar](max) NULL,
	[userfld9] [nvarchar](max) NULL,
	[userfld10] [nvarchar](max) NULL,
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
 CONSTRAINT [PK_StockTransferHdr_1] PRIMARY KEY CLUSTERED 
(
	[StockTransferHdrRefNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[StockTransferHdr]  WITH CHECK ADD  CONSTRAINT [FK_StockTransferHdr_TransferFromLocationID] FOREIGN KEY([TransferFromLocationID])
REFERENCES [dbo].[InventoryLocation] ([InventoryLocationID])

ALTER TABLE [dbo].[StockTransferHdr]  WITH CHECK ADD  CONSTRAINT [FK_StockTransferHdr_TransferToLocationID] FOREIGN KEY([TransferToLocationID])
REFERENCES [dbo].[InventoryLocation] ([InventoryLocationID])

END

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StockTransferDet]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[StockTransferDet](
	[StockTransferDetRefNo] [varchar](50) NOT NULL,
	[StockTransferHdrRefNo] [varchar](50) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[Quantity] [float] NOT NULL,
	[FactoryPrice] [money] NOT NULL,
	[CostOfGoods] [money] NOT NULL,
	[FullFilledQuantity] [float] NULL,
	[Remark] [nvarchar](max) NULL,
	[Status] [nvarchar](50) NULL,
	[Deleted] [bit] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[userfld1] [nvarchar](max) NULL,
	[userfld2] [nvarchar](max) NULL,
	[userfld3] [nvarchar](max) NULL,
	[userfld4] [nvarchar](max) NULL,
	[userfld5] [nvarchar](max) NULL,
	[userfld6] [nvarchar](max) NULL,
	[userfld7] [nvarchar](max) NULL,
	[userfld8] [nvarchar](max) NULL,
	[userfld9] [nvarchar](max) NULL,
	[userfld10] [nvarchar](max) NULL,
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
 CONSTRAINT [PK_StockTransferDet_1] PRIMARY KEY CLUSTERED 
(
	[StockTransferDetRefNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

ALTER TABLE [dbo].[StockTransferDet]  WITH CHECK ADD  CONSTRAINT [FK_StockTransferDet_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])

ALTER TABLE [dbo].[StockTransferDet]  WITH CHECK ADD  CONSTRAINT [FK_StockTransferDet_StockTransferHdr] FOREIGN KEY([StockTransferHdrRefNo])
REFERENCES [dbo].[StockTransferHdr] ([StockTransferHdrRefNo])

END