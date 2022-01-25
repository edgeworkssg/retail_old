IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[SalesOrderMapping]') AND type in (N'U'))

BEGIN

CREATE TABLE [dbo].[SalesOrderMapping](
	[SalesOrderMappingID] [int] IDENTITY(1,1) NOT NULL,
	[OrderDetID] [varchar](50) NULL,
	[PurchaseOrderDetRefNo] [varchar](50) NULL,
	[Qty] [int] NULL,
	[QtyApproved] [int] NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_SalesOrderMapping] PRIMARY KEY CLUSTERED 
(
	[SalesOrderMappingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END