IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[AppPendingProcess]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[AppPendingProcess](
	[AppPendingProcessID] [uniqueidentifier] NOT NULL,
	[ProcessType] [nvarchar](100) NULL,
	[ProcessSubType] [nvarchar](100) NULL,
	[DataInput] [nvarchar](max) NULL,
	[AssemblyName] [nvarchar](500) NULL,
	[ClassName] [nvarchar](500) NULL,
	[MethodName] [nvarchar](500) NULL,
	[ProcessorID] [uniqueidentifier] NULL,
	[ProcessStatus] [nvarchar](100) NULL,
	[ProcessMessage] [nvarchar](max) NULL,
	[ProcessedCount] [int] NULL,
	[SubmittedTime] [datetime] NULL,
	[StartProcessingTime] [datetime] NULL,
	[EndProcessingTime] [datetime] NULL,
	[UserName] [nvarchar](50) NULL,
	[EnableNotification] [bit] NULL,
	[CallbackURL] [nvarchar](max) NULL,
	[Remark] [nvarchar](max) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](max) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](max) NULL,
	[Deleted] [bit] NULL,
	[Userfld1] [varchar](max) NULL,
	[Userfld2] [varchar](max) NULL,
	[Userfld3] [varchar](max) NULL,
	[Userfld4] [varchar](max) NULL,
	[Userfld5] [varchar](max) NULL,
	[Userfld6] [varchar](max) NULL,
	[Userfld7] [varchar](max) NULL,
	[Userfld8] [varchar](max) NULL,
	[Userfld9] [varchar](max) NULL,
	[Userfld10] [varchar](max) NULL,
	[Userflag1] [bit] NULL,
	[Userflag2] [bit] NULL,
	[Userflag3] [bit] NULL,
	[Userflag4] [bit] NULL,
	[Userflag5] [bit] NULL,
	[Userfloat1] [money] NULL,
	[Userfloat2] [money] NULL,
	[Userfloat3] [money] NULL,
	[Userfloat4] [money] NULL,
	[Userfloat5] [money] NULL,
	[Userint1] [int] NULL,
	[Userint2] [int] NULL,
	[Userint3] [int] NULL,
	[Userint4] [int] NULL,
	[Userint5] [int] NULL,
 CONSTRAINT [PK_AppPendingProcess] PRIMARY KEY CLUSTERED 
(
	[AppPendingProcessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'CallbackURL' AND [object_id] = OBJECT_ID(N'AppPendingProcess'))
BEGIN
    ALTER TABLE AppPendingProcess ADD CallbackURL NVARCHAR(MAX) NULL
END