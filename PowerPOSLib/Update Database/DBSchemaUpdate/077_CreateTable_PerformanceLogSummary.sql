SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PerformanceLogSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PerformanceLogSummary](
	[PerformanceLogSummaryID] [uniqueidentifier] NOT NULL,
	[ModuleName] [varchar](max) NOT NULL,
	[FunctionName] [varchar](max) NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[AvgElapsedTime] [numeric](18, 2) NOT NULL,
	[MinElapsedTime] [numeric](18, 2) NOT NULL,
	[MaxElapsedTime] [numeric](18, 2) NOT NULL,
	[TransCount] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_PerformanceSummary] PRIMARY KEY CLUSTERED 
(
	[PerformanceLogSummaryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

SET ANSI_PADDING OFF
