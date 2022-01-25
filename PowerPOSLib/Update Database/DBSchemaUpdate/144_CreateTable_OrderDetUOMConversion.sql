IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[OrderDetUOMConversion]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[OrderDetUOMConversion](
	[OrderDetUOMConvID] [varchar](50) NOT NULL,
	[OrderDetID] [varchar](18) NULL,
	[OrderHdrID] [varchar](18) NULL,
	[OrderDetItemNo] [varchar](50) NULL,
	[DeductedItemNo] [varchar](50) NULL,
	[Qty] [decimal](20, 5) NULL,
	[UnitPrice] [money] NULL,
	[Amount] [money] NULL,
	[ConversionRate] [decimal](20, 5) NULL,
	[IsVoided] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[UniqueID] [uniqueidentifier] NULL,
	[userfld1] [varchar](50) NULL,
	[userfld2] [varchar](50) NULL,
	[userfld3] [varchar](50) NULL,
	[userfld4] [varchar](50) NULL,
	[userfld5] [varchar](50) NULL,
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
 CONSTRAINT [PK_OrderDetUOMConversion] PRIMARY KEY CLUSTERED 
(
	[OrderDetUOMConvID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


ALTER TABLE [dbo].[OrderDetUOMConversion]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetUOMConversion_OrderDet] FOREIGN KEY([OrderDetID])
REFERENCES [dbo].[OrderDet] ([OrderDetID])

ALTER TABLE [dbo].[OrderDetUOMConversion] CHECK CONSTRAINT [FK_OrderDetUOMConversion_OrderDet]

ALTER TABLE [dbo].[OrderDetUOMConversion]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetUOMConversion_OrderDetUOMConversion] FOREIGN KEY([OrderDetUOMConvID])
REFERENCES [dbo].[OrderDetUOMConversion] ([OrderDetUOMConvID])

ALTER TABLE [dbo].[OrderDetUOMConversion] CHECK CONSTRAINT [FK_OrderDetUOMConversion_OrderDetUOMConversion]

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OrderDate' AND [object_id] = OBJECT_ID(N'OrderDetUOMConversion'))
BEGIN
    ALTER TABLE OrderDetUOMConversion Add OrderDate datetime null 
END


