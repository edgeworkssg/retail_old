
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommissionHdr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommissionHdr](
	[CommissionHdrID] [int] IDENTITY(1,1) NOT NULL,
	[SchemeName] [nvarchar](50) NULL,
	[IsProduct] [int] NULL,
	[ProductWeight] [decimal](5,2) NULL,
	[IsService] [int] NULL,
	[ServiceWeight] [decimal](5,2) NULL,
	[IsPointSold] [int] NULL,
	[PointSoldWeight] [decimal](5,2) NULL,
	[IsPackageSold] [int] NULL,
	[PackageSoldWeight] [decimal](5,2) NULL,
	[IsPointRedeem] [int] NULL,
	[PointRedeemWeight] [decimal](5,2) NULL,
	[IsPackageRedeem] [int] NULL,
	[PackageRedeemWeight] [decimal](5,2) NULL,
	[IsDeductionFor2ndSalesPerson] [int] NULL,
	[DeductionBy] [varchar](20) NULL,
	[DeductionValue] [decimal](18,2) NULL,
	[SalesGroupID] [int] NULL,
	[CommissionBy] [varchar](20) NULL,
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
 CONSTRAINT [PK_CommissionHdr] PRIMARY KEY CLUSTERED 
(
	[CommissionHdrID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommissionDetFor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommissionDetFor](
	[CommissionDetForID] [int] IDENTITY(1,1) NOT NULL,
	[CommissionHdrID] [int] NULL,
	[CategoryName] [nvarchar](250) NULL,
	[ItemNo] [nvarchar](50) NULL,
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
 CONSTRAINT [PK_CommissionDetFor] PRIMARY KEY CLUSTERED 
(
	[CommissionDetForID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CommissionDetBy]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CommissionDetBy](
	[CommissionDetByID] [int] IDENTITY(1,1) NOT NULL,
	[CommissionHdrID] [int] NULL,
	[From] [decimal](18,2) NULL,
	[To] [decimal](18,2) NULL,
	[Value] [decimal](18,2) NULL,
 CONSTRAINT [PK_CommissionDetBy] PRIMARY KEY CLUSTERED 
(
	[CommissionDetByID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesCommissionSummary]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesCommissionSummary](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MonthDate] [date] NULL,
	[Month] [nvarchar](20) NULL,
	[Staff] [nvarchar](50) NULL,
	[Salary] [money] NULL,
	[OtherAllowance] [money] NULL,
	[Commission] [money] NULL,
	[Total] [money] NULL,
	[Status] [nvarchar](20) NULL,
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
 CONSTRAINT [PK_SalesCommissionSummary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesCommissionDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesCommissionDetails](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MonthDate] [date] NULL,
	[Month] [nvarchar](20) NULL,
	[Staff] [nvarchar](50) NULL,
	[CommissionType] [nvarchar](200) NULL,
	[TotalQty] [float] NULL,
	[TotalSales] [money] NULL,
	[TotalCommission] [money] NULL,
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
 CONSTRAINT [PK_SalesCommissionDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesCommissionDetails_Commission]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesCommissionDetails_Commission](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Month] [nvarchar](50) NOT NULL,
	[Staff] [nvarchar](50) NOT NULL,
	[Scheme] [nvarchar](50) NOT NULL,
	[CommissionText] [nvarchar](200) NULL,
	[CommissionValue] [money] NULL,
 CONSTRAINT [PK_SalesCommissionDetails_Commission] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SalesCommissionDetails_Deduction]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SalesCommissionDetails_Deduction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Month] [nvarchar](50) NULL,
	[Staff] [nvarchar](50) NULL,
	[Deduction] [money] NULL,
 CONSTRAINT [PK_SalesCommissionDetails_Deduction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END