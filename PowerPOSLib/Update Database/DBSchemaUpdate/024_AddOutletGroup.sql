
/****** Object:  Table [dbo].[Company]    Script Date: 3/10/2015 6:04:26 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OutletGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OutletGroup](
	[OutletGroupID] [int] IDENTITY(1,1) NOT NULL,
	[OutletGroupName] [varchar](50) NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
	[userfld1] [varchar](50) NULL,
	[userfld2] [varchar](50) NULL,
	[userfld3] [varchar](50) NULL,
	[userfld4] [varchar](50) NULL,
	[userfld5] [varchar](50) NULL,
	[userfld6] [varchar](50) NULL,
	[userfld7] [varchar](50) NULL,
	[userfld8] [varchar](50) NULL,
	[userfld9] [varchar](50) NULL,
	[userfld10] [varchar](50) NULL,
	[userflag1] [bit] NULL,
	[userflag2] [bit] NULL,
	[userflag3] [bit] NULL,
	[userflag4] [bit] NULL,
	[userflag5] [bit] NULL,
	[userfloat1] [money] NULL,
	[userfloat2] [money] NULL,
	[userfloat3] [money] NULL,
	[userfloat4] [money] NULL,
	[userfloat5] [money] NULL,
	[userint1] [int] NULL,
	[userint2] [int] NULL,
	[userint3] [int] NULL,
	[userint4] [int] NULL,
	[userint5] [int] NULL,
 CONSTRAINT [PK_OutletGroup_1] PRIMARY KEY CLUSTERED 
(
	[OutletGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

END

SET ANSI_PADDING OFF

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'OutletGroupID' AND [object_id] = OBJECT_ID(N'Outlet'))
BEGIN
    ALTER TABLE Outlet ADD OutletGroupID int
END


SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OutletGroupItemMap]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OutletGroupItemMap](
	[OutletGroupItemMapID] [int] IDENTITY(1,1) NOT NULL,
	[OutletGroupID] [int] NULL,
	[OutletName] [varchar](50) NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[RetailPrice] [money] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
 CONSTRAINT [PK_OutletGroupItemMap] PRIMARY KEY CLUSTERED 
(
	[OutletGroupItemMapID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[OutletGroupItemMap] ADD  CONSTRAINT [DF_OutletGroupItemMap_RetailPrice]  DEFAULT ((0)) FOR [RetailPrice]

ALTER TABLE [dbo].[OutletGroupItemMap] ADD  CONSTRAINT [DF_OutletGroupItemMap_CostPrice]  DEFAULT ((0)) FOR [CostPrice]

ALTER TABLE [dbo].[OutletGroupItemMap] ADD  CONSTRAINT [DF_OutletGroupItemMap_Deleted]  DEFAULT ((0)) FOR [Deleted]

END

SET ANSI_PADDING OFF

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsItemDeleted' AND OBJECT_ID = OBJECT_ID(N'[OutletGroupItemMap]'))
BEGIN
ALTER TABLE dbo.OutletGroupItemMap ADD
	IsItemDeleted bit NULL
END