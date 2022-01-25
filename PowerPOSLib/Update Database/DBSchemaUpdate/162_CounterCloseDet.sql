IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterCloseDet]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[CounterCloseDet](
	[CounterCloseDetID] [varchar](50) NOT NULL,
	[CounterCloseID] [varchar](50) NOT NULL,
	[PaymentType] [nvarchar](200) NOT NULL,
	[UnitValue] [money] NOT NULL,
	[UnitDisplayedName] [nvarchar](200) NULL,
	[TotalCount] [int] NOT NULL,
	[TotalAmount] [money] NOT NULL,
	[Remark] [nvarchar](500)NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NULL,
	[UniqueID] [uniqueidentifier] NOT NULL,
	[userfld1] [nvarchar](500) NULL,
	[userfld2] [nvarchar](500) NULL,
	[userfld3] [nvarchar](500) NULL,
	[userfld4] [nvarchar](500) NULL,
	[userfld5] [nvarchar](500) NULL,
	[userfld6] [nvarchar](500) NULL,
	[userfld7] [nvarchar](500) NULL,
	[userfld8] [nvarchar](500) NULL,
	[userfld9] [nvarchar](500) NULL,
	[userfld10] [nvarchar](500) NULL,
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
 CONSTRAINT [PK_CounterCloseDet] PRIMARY KEY CLUSTERED 
(
	[CounterCloseDetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

