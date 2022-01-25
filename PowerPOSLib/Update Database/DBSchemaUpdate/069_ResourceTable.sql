IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ResourceGroup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[ResourceGroup](
		[ResourceGroupID] [int] IDENTITY(1,1) NOT NULL,
		[GroupName] [nvarchar](50) NULL,
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
	 CONSTRAINT [PK_ResourceGroup] PRIMARY KEY CLUSTERED 
	(
		[ResourceGroupID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Resource]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Resource](
		[ResourceID] [varchar](50) NOT NULL,
		[ResourceName] [nvarchar](255) NULL,
		[ResourceGroupID] [int] NULL,
		[Status] [bit] NULL,
		[Capacity] [int] NULL,
		[RoomCharge] [money] NULL,
		[MinSpending] [money] NULL,
		[MinSpendingCharge] [money] NULL,
		[ShowHide] [bit] NULL,
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
	 CONSTRAINT [PK_Resource] PRIMARY KEY CLUSTERED 
	(
		[ResourceID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]


	ALTER TABLE [dbo].[Resource]  WITH CHECK ADD  CONSTRAINT [FK_Resource_ResourceGroup] FOREIGN KEY([ResourceGroupID])
	REFERENCES [dbo].[ResourceGroup] ([ResourceGroupID])


	ALTER TABLE [dbo].[Resource] CHECK CONSTRAINT [FK_Resource_ResourceGroup]

END


IF((SELECT COUNT(*) FROM Item WHERE ItemNo = 'ROOM_CHARGE')<=0)
BEGIN 
INSERT into Item (ItemNo, ItemName, Barcode, CategoryName,UniqueID, IsServiceItem, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted) 
VALUES ('ROOM_CHARGE', 'ROOM_CHARGE','ROOM_CHARGE', 'SYSTEM', NEWID(),0,'edgeworks',GETDATE(),'edgeworks',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM ResourceGroup WHERE GroupName = 'SYSTEM')<=0)
BEGIN 
SET IDENTITY_iNSERT ResourceGroup ON

INSERT into ResourceGroup (ResourceGroupID,GroupName, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted) 
VALUES (0, 'SYSTEM', 'SYSTEM',GETDATE(),'SYSTEM',GETDATE(),0) 

SET IDENTITY_iNSERT ResourceGroup OFF

END

IF((SELECT COUNT(*) FROM [Resource] WHERE ResourceID = 'ROOM_DEFAULT')<=0)
BEGIN 
INSERT into [Resource] (ResourceID, ResourceName, ResourceGroupID, Capacity, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted) 
VALUES ('ROOM_DEFAULT', 'ROOM_DEFAULT',0, 999, 'SYSTEM',GETDATE(),'SYSTEM',GETDATE(),0) 
END


