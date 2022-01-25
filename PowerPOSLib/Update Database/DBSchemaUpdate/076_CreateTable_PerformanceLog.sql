SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerformanceLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PerformanceLog](
	[PerformanceLogID] [uniqueidentifier] NOT NULL,
	[ModuleName] [varchar](max) NOT NULL,
	[FunctionName] [varchar](max) NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[ElapsedTime] [numeric](18, 2) NOT NULL,
	[PrimaryKeyData] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_PerformanceLog] PRIMARY KEY CLUSTERED 
(
	[PerformanceLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

SET ANSI_PADDING OFF
