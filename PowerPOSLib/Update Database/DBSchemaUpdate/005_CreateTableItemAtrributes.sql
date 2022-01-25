IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemAttributes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemAttributes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](20) NULL,
	[Value] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_ItemAttributes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


