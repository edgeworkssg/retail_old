IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SAPCustomerCode]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SAPCustomerCode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SalesType] [varchar](50) NULL,
	[PaymentType] [varchar](50) NULL,
	[PointOfSaleID] [int] NULL,
	[CustomerCode] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_SAPCustomerCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_SAPCustomerCode_Deleted]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[SAPCustomerCode] ADD  CONSTRAINT [DF_SAPCustomerCode_Deleted]  DEFAULT ((0)) FOR [Deleted]
END

