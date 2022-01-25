SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PointTempLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PointTempLog](
	[TempID] [int] IDENTITY(1,1) NOT NULL,
	[OrderHdrID] [varchar](50) NOT NULL,
	[MembershipNo] [varchar](50) NOT NULL,
	[PointAllocated] [money] NOT NULL,
	[RefNo] [varchar](max) NULL,
	[PointType] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_PointTempLog] PRIMARY KEY CLUSTERED 
(
	[TempID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

SET ANSI_PADDING OFF

