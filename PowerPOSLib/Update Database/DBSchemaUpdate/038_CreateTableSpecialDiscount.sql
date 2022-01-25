DECLARE @RES varchar(100);
SELECT @Res= COLUMN_NAME
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
AND TABLE_NAME = 'SpecialDiscounts'

IF Upper(@Res) <> 'DISCOUNTNAME'
BEGIN
	ALTER TABLE dbo.SpecialDiscounts
	DROP CONSTRAINT PK_SpecialDiscounts

ALTER TABLE dbo.SpecialDiscounts ADD CONSTRAINT
	PK_SpecialDiscounts PRIMARY KEY CLUSTERED 
	(
	DiscountName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	ALTER TABLE dbo.SpecialDiscounts SET (LOCK_ESCALATION = TABLE)
	
	

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'UseSPP' AND [object_id] = OBJECT_ID(N'SpecialDiscounts'))
BEGIN
	Alter Table SpecialDiscounts Add [UseSPP] [bit] NULL;
	Alter Table SpecialDiscounts Add [Enabled] [bit] NULL
	Alter Table SpecialDiscounts Add [ApplicableToAllItem] [bit] NULL
	Alter Table SpecialDiscounts Add [StartDate] [datetime] NULL
	Alter Table SpecialDiscounts Add [EndDate] [datetime] NULL
	Alter Table SpecialDiscounts Add [MinimumSpending] [decimal](18, 2) NULL
	Alter Table SpecialDiscounts Add [isBankPromo] [bit] NULL
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DiscountLabel' AND [object_id] = OBJECT_ID(N'SpecialDiscounts'))
BEGIN
	Alter Table SpecialDiscounts Add [DiscountLabel] varchar(50) NULL;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'AssignedOutlet' AND [object_id] = OBJECT_ID(N'SpecialDiscounts'))
BEGIN
	Alter Table SpecialDiscounts Add [AssignedOutlet] varchar(max) NULL;
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SpecialDiscountDetail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SpecialDiscountDetail](
	[DiscountName] [nvarchar](50) NOT NULL,
	[ItemNo] [varchar](50) NOT NULL,
	[DiscountPercentage] [decimal](18, 4) NULL,
	[DiscountAmount] [decimal](18, 4) NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
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
	[SpecialDiscountDetailID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_SpecialDiscountDetail] PRIMARY KEY CLUSTERED 
(
	[SpecialDiscountDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [dbo].[SpecialDiscountDetail]  WITH CHECK ADD  CONSTRAINT [FK_SpecialDiscountDetail_SpecialDiscounts] FOREIGN KEY([DiscountName])
REFERENCES [dbo].[SpecialDiscounts] ([DiscountName])
ON UPDATE CASCADE;

ALTER TABLE [dbo].[SpecialDiscountDetail] CHECK CONSTRAINT [FK_SpecialDiscountDetail_SpecialDiscounts];

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.FK_SpecialDiscountDetail_Item')
   AND parent_object_id = OBJECT_ID(N'dbo.SpecialDiscountDetail')
)
  ALTER TABLE SpecialDiscountDetail DROP CONSTRAINT FK_SpecialDiscountDetail_Item


END