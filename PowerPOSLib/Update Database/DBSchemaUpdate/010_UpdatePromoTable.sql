IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Priority' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
    ALTER TABLE PromoCampaignHdr Add Priority int
END

IF EXISTS(SELECT * FROM sys.columns WHERE [name] = N'AnyQty' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
     
    ALTER TABLE PromoCampaignHdr drop column AnyQty
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'promoprice' AND [object_id] = OBJECT_ID(N'promocampaigndet'))
BEGIN
    alter table promocampaigndet add promoprice money
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PromoCode' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
    ALTER TABLE PromoCampaignHdr Add PromoCode varchar(50)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'Barcode' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
    ALTER TABLE PromoCampaignHdr Add Barcode varchar(50)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'IsRestricHour' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
    ALTER TABLE PromoCampaignHdr Add IsRestricHour bit
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RestrictHourStart' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
    ALTER TABLE PromoCampaignHdr Add RestrictHourStart datetime
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'RestrictHourEnd' AND [object_id] = OBJECT_ID(N'PromoCampaignHdr'))
BEGIN
    ALTER TABLE PromoCampaignHdr Add RestrictHourEnd datetime
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'AnyQty' AND [object_id] = OBJECT_ID(N'PromoCampaignDet'))
BEGIN
    ALTER TABLE PromoCampaignDet Add AnyQty int
END
ELSE
BEGIN
	ALTER TABLE PromoCampaignDet Alter column AnyQty int
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DiscPercent' AND [object_id] = OBJECT_ID(N'PromoCampaignDet'))
BEGIN
    ALTER TABLE PromoCampaignDet Add DiscPercent decimal(18, 2)
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'DiscAmount' AND [object_id] = OBJECT_ID(N'PromoCampaignDet'))
BEGIN
    ALTER TABLE PromoCampaignDet Add DiscAmount money
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'PromoPrice' AND [object_id] = OBJECT_ID(N'PromoCampaignDet'))
BEGIN
    ALTER TABLE PromoCampaignDet Add PromoPrice money
END

/****** Object:  Table [dbo].[PromoOutlet]    Script Date: 02/25/2015 09:37:48 ******/
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PromoOutlet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PromoOutlet](
	[PromoOutletID] [int] IDENTITY(1,1) NOT NULL,
	[PromoCampaignHdrID] [int] NULL,
	[OutletName] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_PromoOutlet] PRIMARY KEY CLUSTERED 
(
	[PromoOutletID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

/****** Object:  Table [dbo].[PromoOutlet]    Script Date: 02/25/2015 09:37:48 ******/
IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PromoDaysMap]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PromoDaysMap](
	[PromoDaysID] [int] IDENTITY(1,1) NOT NULL,
	[PromoCampaignHdrID] [int] NULL,
	[DaysPromo] [varchar](20) NULL,
	[DaysNumber] [int] NULL,
	[CreatedBy] [varchar](50) NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) NULL,
	[ModifiedOn] [datetime] NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_PromoDaysMap] PRIMARY KEY CLUSTERED 
(
	[PromoDaysID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END










