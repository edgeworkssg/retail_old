/****** Object:  Table [dbo].[Dashboard]    Script Date: 08/18/2014 10:47:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Dashboard]') AND type in (N'U'))
DROP TABLE [dbo].[Dashboard]
GO

/****** Object:  Table [dbo].[Dashboard]    Script Date: 08/18/2014 10:47:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

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

GO

SET ANSI_PADDING OFF
GO


