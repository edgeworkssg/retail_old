SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemFuturePrice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemFuturePrice](
	[ItemFuturePriceID] [uniqueidentifier] NOT NULL,
	[ApplicableDate] [datetime] NULL,
	[Status] [varchar](50) NULL,
	[ApplicableTo] [varchar](50) NULL,
	[OutletID] [varchar](50) NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[RetailPrice] [money] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[P1] [money] NULL,
	[P2] [money] NULL,
	[P3] [money] NULL,
	[P4] [money] NULL,
	[P5] [money] NULL,
	[Remarks] [nvarchar](max) NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Userfld1] [nvarchar](500) NULL,
	[Userfld2] [nvarchar](500) NULL,
	[Userfld3] [nvarchar](500) NULL,
	[Userfld4] [nvarchar](500) NULL,
	[Userfld5] [nvarchar](500) NULL,
	[Userfld6] [nvarchar](500) NULL,
	[Userfld7] [nvarchar](500) NULL,
	[Userfld8] [nvarchar](500) NULL,
	[Userfld9] [nvarchar](500) NULL,
	[Userfld10] [nvarchar](500) NULL,
	[Userfloat1] [money] NULL,
	[Userfloat2] [money] NULL,
	[Userfloat3] [money] NULL,
	[Userfloat4] [money] NULL,
	[Userfloat5] [money] NULL,
	[Userflag1] [bit] NULL,
	[Userflag2] [bit] NULL,
	[Userflag3] [bit] NULL,
	[Userflag4] [bit] NULL,
	[Userflag5] [bit] NULL,
	[Userint1] [int] NULL,
	[Userint2] [int] NULL,
	[Userint3] [int] NULL,
	[Userint4] [int] NULL,
	[Userint5] [int] NULL,
 CONSTRAINT [PK_ItemFuturePrice] PRIMARY KEY CLUSTERED 
(
	[ItemFuturePriceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

SET ANSI_PADDING OFF
