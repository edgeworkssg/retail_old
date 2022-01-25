/****** Object:  Table [dbo].[Appointment]    Script Date: 2/5/2016 5:30:46 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Appointment]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Appointment](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[Duration] [int] NOT NULL,
	[Description] [varchar](500) NULL,
	[BackColor] [int] NOT NULL,
	[FontColor] [int] NOT NULL,
	[SalesPersonID] [varchar](50) NOT NULL,
	[MembershipNo] [varchar](50) NULL,
	[OrderHdrID] [varchar](14) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NULL,
	[Organization] [nvarchar](200) NULL,
	[PickUpLocation] [nvarchar](200) NULL,
	[NoOfChildren] [int] NULL,
	[PointOfSaleID] [int] NULL,
	[ResourceID] [varchar](50) NULL,
	[CheckInByWho] [nvarchar](50) NULL,
	[CheckOutByWho] [nvarchar](50) NULL,
	[CheckInTime] [datetime] NULL,
	[CheckOutTime] [datetime] NULL,
	[Remark] [nvarchar](max) NULL,
	[IsServerUpdate] [bit] NULL,
	[TimeExtension] [int] NULL,
 CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

SET ANSI_PADDING OFF

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Appointment_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Appointment] ADD  CONSTRAINT [DF_Appointment_Id]  DEFAULT (newid()) FOR [Id]
END

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE name LIKE 'DF__Appointme__BackC__%' AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Appointment] ADD  DEFAULT ((-2127584)) FOR [BackColor]
END

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE name LIKE 'DF__Appointme__FontC__%' AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Appointment] ADD  DEFAULT ((-1)) FOR [FontColor]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointment_Membership]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointment]'))
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_Membership] FOREIGN KEY([MembershipNo])
REFERENCES [dbo].[Membership] ([MembershipNo])
ON UPDATE CASCADE
ON DELETE CASCADE

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointment_Membership]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointment]'))
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_Membership]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointment_UserMst]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointment]'))
ALTER TABLE [dbo].[Appointment]  WITH CHECK ADD  CONSTRAINT [FK_Appointment_UserMst] FOREIGN KEY([SalesPersonID])
REFERENCES [dbo].[UserMst] ([UserName])
ON UPDATE CASCADE
ON DELETE CASCADE

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Appointment_UserMst]') AND parent_object_id = OBJECT_ID(N'[dbo].[Appointment]'))
ALTER TABLE [dbo].[Appointment] CHECK CONSTRAINT [FK_Appointment_UserMst]
