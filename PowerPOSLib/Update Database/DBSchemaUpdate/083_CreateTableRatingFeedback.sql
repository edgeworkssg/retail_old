IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingFeedback]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[RatingFeedback](
	[RatingFeedbackID] [int] IDENTITY(1,1) NOT NULL,
	[SelectionText] [varchar](50) NULL,
	[SelectionImage] [varbinary](max) NULL,
	[RatingType] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_RatingFeedback] PRIMARY KEY CLUSTERED 
(
	[RatingFeedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END



IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'SelectionImageUrl' AND [object_id] = OBJECT_ID(N'RatingFeedback'))
BEGIN
    ALTER TABLE RatingFeedback ADD SelectionImageUrl varchar(MAX) NULL
END


