SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecipeDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RecipeDetail](
	[RecipeDetailID] [varchar](64) NOT NULL,
	[RecipeHeaderID] [varchar](64) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[Qty] [decimal](20, 5) NOT NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[ModifiedOn] [datetime] NULL,
	[userfld1] [varchar](50) NULL,
	[userfld2] [varchar](50) NULL,
	[userfld3] [varchar](50) NULL,
	[userfld4] [varchar](50) NULL,
	[userfld5] [varchar](50) NULL,
	[Deleted] [bit] NULL,
	[IsPacking] [bit] NULL,
	[UOM] [nvarchar](50) NULL,
 CONSTRAINT [PK_RecipeDetail] PRIMARY KEY CLUSTERED 
(
	[RecipeDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RecipeHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RecipeHeader](
	[RecipeHeaderID] [varchar](64) NOT NULL,
	[RecipeName] [varchar](150) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[CreatedBy] [nvarchar](200) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](200) NULL,
	[ModifiedOn] [datetime] NULL,
	[userfld1] [varchar](50) NULL,
	[userfld2] [varchar](50) NULL,
	[userfld3] [varchar](50) NULL,
	[userfld4] [varchar](50) NULL,
	[userfld5] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_RecipeHeader] PRIMARY KEY CLUSTERED 
(
	[RecipeHeaderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RecipeDetail_RecipeHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[RecipeDetail]'))
ALTER TABLE [dbo].[RecipeDetail]  WITH CHECK ADD  CONSTRAINT [FK_RecipeDetail_RecipeHeader] FOREIGN KEY([RecipeHeaderID])
REFERENCES [dbo].[RecipeHeader] ([RecipeHeaderID])

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RecipeDetail_RecipeHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[RecipeDetail]'))
ALTER TABLE [dbo].[RecipeDetail] CHECK CONSTRAINT [FK_RecipeDetail_RecipeHeader]

