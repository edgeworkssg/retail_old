IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ManualSalesUpdate]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ManualSalesUpdate](
	[ManualSalesUpdateID] [int] IDENTITY(1,1) NOT NULL,
	[PointOfSaleID] [int] NULL,
	[MallCode] [nvarchar](200) NULL,
	[TenantCode] [nvarchar](200) NULL,
	[Date] [nvarchar](200) NULL,
	[Hour] [nvarchar](200) NULL,
	[TransactionCount] [int] NULL,
	[TotalSalesAfterTax] [money] NULL,
	[TotalSalesBeforeTax] [money] NULL,
	[TotalTax] [money] NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
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
	[userint5] [int] NULL,
 CONSTRAINT [PK_ManualSalesUpdate] PRIMARY KEY CLUSTERED 
(
	[ManualSalesUpdateID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END