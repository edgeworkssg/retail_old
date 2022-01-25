﻿SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerQuickRef]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerQuickRef](
	[RefID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [varchar](50) NOT NULL,
	[LastModifiedon] [datetime] NOT NULL,
	[PointOfSaleID] [int] NULL,
	[Outlet] [varchar](50) NULL,
	[InventoryLocationID] [int] NULL,
 CONSTRAINT [PK_ServerQuickRef] PRIMARY KEY CLUSTERED 
(
	[RefID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

