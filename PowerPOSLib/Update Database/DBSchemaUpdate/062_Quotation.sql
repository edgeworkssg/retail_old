IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[QuotationHdr]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[QuotationHdr](
	[OrderHdrID] [varchar](14) NOT NULL,
	[OrderRefNo] [varchar](50) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[InventoryHdrRefNo] [varchar](50) NULL,
	[CashierID] [varchar](50) NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[GrossAmount] [money] NOT NULL,
	[NettAmount] [money] NOT NULL,
	[DiscountAmount] [money] NOT NULL,
	[GST] [float] NOT NULL,
	[IsVoided] [bit] NOT NULL,
	[MembershipNo] [varchar](50) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) NULL,
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
	[PromoCodeID] [int] NULL,
	[GSTAmount] [money] NULL,
	[IsPointAllocated] [bit] NOT NULL,
 CONSTRAINT [PK_QuotationHdr] PRIMARY KEY CLUSTERED 
(
	[OrderHdrID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[QuotationHdr]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationHdr_Stall] FOREIGN KEY([PointOfSaleID])
REFERENCES [dbo].[PointOfSale] ([PointOfSaleID])

ALTER TABLE [dbo].[QuotationHdr] CHECK CONSTRAINT [FK_QuotationHdr_Stall]

ALTER TABLE [dbo].[QuotationHdr] ADD  CONSTRAINT [DF_QuotationHdr_Discount]  DEFAULT ((0)) FOR [Discount]

ALTER TABLE [dbo].[QuotationHdr] ADD  CONSTRAINT [DF_QuotationHdr_IsVoided]  DEFAULT ((0)) FOR [IsVoided]

ALTER TABLE [dbo].[QuotationHdr] ADD  CONSTRAINT [DF_QuotationHdr_UniqueID]  DEFAULT (newid()) FOR [UniqueID]

ALTER TABLE [dbo].[QuotationHdr] ADD  CONSTRAINT [DF_QuotationHdr_IsPointAllocated]  DEFAULT ((0)) FOR [IsPointAllocated]

END

IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[QuotationDet]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[QuotationDet](
	[OrderDetID] [varchar](18) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[OrderDetDate] [datetime] NOT NULL,
	[Quantity] [decimal](20, 5) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Discount] [money] NOT NULL,
	[Amount] [money] NOT NULL,
	[GrossSales] [money] NULL,
	[IsFreeOfCharge] [bit] NOT NULL,
	[CostOfGoodSold] [money] NULL,
	[IsPromo] [bit] NOT NULL,
	[PromoDiscount] [float] NOT NULL,
	[PromoUnitPrice] [money] NOT NULL,
	[PromoAmount] [money] NOT NULL,
	[IsPromoFreeOfCharge] [bit] NOT NULL,
	[UsePromoPrice] [bit] NULL,
	[PromoHdrID] [int] NULL,
	[PromoDetID] [int] NULL,
	[VoucherNo] [varchar](50) NULL,
	[IsVoided] [bit] NOT NULL,
	[IsSpecial] [bit] NOT NULL,
	[Remark] [nvarchar](max) NULL,
	[IsEventPrice] [bit] NULL,
	[SpecialEventID] [int] NULL,
	[OrderHdrID] [varchar](14) NOT NULL,
	[IsPreOrder] [bit] NULL,
	[IsExchange] [bit] NOT NULL,
	[IsPendingCollection] [bit] NULL,
	[giveCommission] [bit] NULL,
	[InventoryHdrRefNo] [varchar](50) NULL,
	[ExchangeDetRefNo] [varchar](50) NULL,
	[OriginalRetailPrice] [money] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
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
	[GSTAmount] [money] NULL,
 CONSTRAINT [PK_Quotationdet] PRIMARY KEY CLUSTERED 
(
	[OrderDetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[QuotationDet]  WITH NOCHECK ADD  CONSTRAINT [FK_QuotationDet_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])
ON UPDATE CASCADE

ALTER TABLE [dbo].[QuotationDet] CHECK CONSTRAINT [FK_QuotationDet_Item]

ALTER TABLE [dbo].[QuotationDet] ADD  CONSTRAINT [DF_QuotationDet_Discount]  DEFAULT ((0)) FOR [Discount]

ALTER TABLE [dbo].[QuotationDet] ADD  CONSTRAINT [DF_QuotationDet_Amount]  DEFAULT ((0)) FOR [Amount]

ALTER TABLE [dbo].[QuotationDet] ADD  CONSTRAINT [DF_QuotationDet_PromoUnitPrice]  DEFAULT ((0)) FOR [PromoUnitPrice]

ALTER TABLE [dbo].[QuotationDet] ADD  CONSTRAINT [DF_QuotationDet_OriginalRetailPrice]  DEFAULT ((0)) FOR [OriginalRetailPrice]

ALTER TABLE [dbo].[QuotationDet] ADD  CONSTRAINT [DF_QuotationDet_UniqueID]  DEFAULT (newid()) FOR [UniqueID]

END