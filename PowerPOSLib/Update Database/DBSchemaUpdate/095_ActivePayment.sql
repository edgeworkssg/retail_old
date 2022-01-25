IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[ActivePayment]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[ActivePayment](
	[PaymentID] [int] NOT NULL,
	[PaymentName] [nvarchar](250) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Remarks] [varchar](50) NULL,
	[AllowChange] [bit] NULL,
	[AllowExtra] [bit] NULL,
	[ShowRemark1] [bit] NULL,
	[LabelRemark1] [nvarchar](150) NULL,
	[ShowRemark2] [bit] NULL,
	[LabelRemark2] [nvarchar](150) NULL,
	[ShowRemark3] [bit] NULL,
	[LabelRemark3] [nvarchar](150) NULL,
	[ShowRemark4] [bit] NULL,
	[LabelRemark4] [nvarchar](150) NULL,
	[ShowRemark5] [bit] NULL,
	[LabelRemark5] [nvarchar](150) NULL,
	[userfld1] [varchar](50) NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_ActivePayment] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END