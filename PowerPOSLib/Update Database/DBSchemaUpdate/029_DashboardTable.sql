IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dashboard]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Dashboard](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NULL,
	[SubTitle] [nvarchar](500) NULL,
	[Description] [nvarchar](max) NULL,
	[PlotType] [varchar](200) NULL,
	[PlotOption] [nvarchar](max) NULL,
	[Width] [varchar](500) NULL,
	[Height] [varchar](500) NULL,
	[SQLString] [nvarchar](max) NULL,
	[IsInline] [bit] NULL,
	[BreakAfter] [bit] NULL,
	[BreakBefore] [bit] NULL,
	[ColumnStyle] [nvarchar](max) NULL,
	[IsEnable] [bit] NULL,
	[DisplayOrder] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[UniqueID] [uniqueidentifier] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_Dashboard] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
