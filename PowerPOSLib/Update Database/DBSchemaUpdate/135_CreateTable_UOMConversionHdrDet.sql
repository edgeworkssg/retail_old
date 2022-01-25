SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UOMConversionDet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UOMConversionDet](
	[ConversionDetID] [int] IDENTITY(1,1) NOT NULL,
	[ConversionHdrID] [int] NOT NULL,
	[FromUOM] [nvarchar](50) NOT NULL,
	[ToUOM] [nvarchar](50) NOT NULL,
	[ConversionRate] [decimal](20, 5) NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
	[Remark] [nvarchar](200) NULL,
 CONSTRAINT [PK_UOMConversionDet] PRIMARY KEY CLUSTERED 
(
	[ConversionDetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UOMConversionHdr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UOMConversionHdr](
	[ConversionHdrID] [int] IDENTITY(1,1) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_UOMConversionHdr] PRIMARY KEY CLUSTERED 
(
	[ConversionHdrID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UOMConversionDet_UOMConversionHdr]') AND parent_object_id = OBJECT_ID(N'[dbo].[UOMConversionDet]'))
ALTER TABLE [dbo].[UOMConversionDet]  WITH CHECK ADD  CONSTRAINT [FK_UOMConversionDet_UOMConversionHdr] FOREIGN KEY([ConversionHdrID])
REFERENCES [dbo].[UOMConversionHdr] ([ConversionHdrID])

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UOMConversionDet_UOMConversionHdr]') AND parent_object_id = OBJECT_ID(N'[dbo].[UOMConversionDet]'))
ALTER TABLE [dbo].[UOMConversionDet] CHECK CONSTRAINT [FK_UOMConversionDet_UOMConversionHdr]

