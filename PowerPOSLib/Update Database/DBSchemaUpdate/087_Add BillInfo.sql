SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BillInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[BillInfo](
	[BillInfoID] [int] IDENTITY(1,1) NOT NULL,
	[BillInfoName] [nvarchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[UniqueID] [uniqueidentifier] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [BillInfoID] PRIMARY KEY CLUSTERED 
(
	[BillInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

