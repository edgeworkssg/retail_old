/****** Object:  Table [dbo].[AppointmentItem]    Script Date: 2/5/2016 5:08:53 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppointmentItem]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AppointmentItem](
	[Id] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[AppointmentId] [uniqueidentifier] NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_AppointmentItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

SET ANSI_PADDING OFF

IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_AppointmentItem_Id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AppointmentItem] ADD  CONSTRAINT [DF_AppointmentItem_Id]  DEFAULT (newid()) FOR [Id]
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AppointmentItem_Appointment]') AND parent_object_id = OBJECT_ID(N'[dbo].[AppointmentItem]'))
ALTER TABLE [dbo].[AppointmentItem]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentItem_Appointment] FOREIGN KEY([AppointmentId])
REFERENCES [dbo].[Appointment] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AppointmentItem_Appointment]') AND parent_object_id = OBJECT_ID(N'[dbo].[AppointmentItem]'))
ALTER TABLE [dbo].[AppointmentItem] CHECK CONSTRAINT [FK_AppointmentItem_Appointment]

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AppointmentItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[AppointmentItem]'))
ALTER TABLE [dbo].[AppointmentItem]  WITH CHECK ADD  CONSTRAINT [FK_AppointmentItem_Item] FOREIGN KEY([ItemNo])
REFERENCES [dbo].[Item] ([ItemNo])
ON UPDATE CASCADE
ON DELETE CASCADE

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AppointmentItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[AppointmentItem]'))
ALTER TABLE [dbo].[AppointmentItem] CHECK CONSTRAINT [FK_AppointmentItem_Item]

