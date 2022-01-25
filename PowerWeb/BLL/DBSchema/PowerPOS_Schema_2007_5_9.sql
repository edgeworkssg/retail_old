/****** Object:  Table [dbo].[Category]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Category](
	[CategoryName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Remarks] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [DF_CategoryName_Unique] UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  StoredProcedure [dbo].[GetNewCashRecRefNoByPointOfSaleID]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewCashRecRefNoByPointOfSaleID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNewCashRecRefNoByPointOfSaleID] 
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int	
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;		
	
	SELECT isnull(max(right(cashrecrefno,4)),''0'') from cashrecording
	where substring(cashrecrefno,3,6) = substring(convert(varchar,getdate(),112),3,6)
	AND convert(int,substring(cashrecrefno,9,4)) = @PointOfSaleId;
END
' 
END
/****** Object:  Table [dbo].[CashRecordingType]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CashRecordingType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CashRecordingType](
	[CashRecordingTypeId] [int] IDENTITY(1,1) NOT NULL,
	[CashRecordingTypeName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_CashRecordingType_1] PRIMARY KEY CLUSTERED 
(
	[CashRecordingTypeId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[UserGroup]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGroup]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserGroup](
	[GroupID] [int] IDENTITY(1,1) NOT NULL,
	[GroupName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[GroupDescription] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_UserGroup_1] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  View [dbo].[ViewLoginActivityReport]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewLoginActivityReport]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewLoginActivityReport
AS
SELECT     dbo.LoginActivity.LoginActivityID, dbo.LoginActivity.UserName, dbo.LoginActivity.LoginType, dbo.LoginActivity.LoginDateTime, dbo.Outlet.OutletName, 
                      dbo.PointOfSale.PointOfSaleName, dbo.PointOfSale.PointOfSaleID
FROM         dbo.LoginActivity INNER JOIN
                      dbo.PointOfSale ON dbo.LoginActivity.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN
                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName
' 
/****** Object:  StoredProcedure [dbo].[FetchItemByName]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchItemByName]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchItemByName] 
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int,
	@itemName varchar(50)		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT A.ItemNo FROM ITEM A, ITEMCATPointOfSaleCAT B, PointOfSale C
	WHERE A.CATEGORYNAME = B.CATEGORYNAME AND B.PointOfSaleCATEGORYID = C.PointOfSaleCATEGORYID
	AND C.PointOfSaleID = @PointOfSaleId AND A.ITEMNAME LIKE ''%'' + @itemName + ''%'';
END
' 
END
/****** Object:  StoredProcedure [dbo].[FetchItemByBarcode]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchItemByBarcode]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchItemByBarcode]
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int,
	@barcode varchar(50)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;	

    -- Insert statements for procedure here
	SELECT A.ItemNo FROM ITEM A, ITEMCATPointOfSaleCAT B, PointOfSale C
	WHERE A.CATEGORYNAME = B.CATEGORYNAME AND B.PointOfSaleCATEGORYID = C.PointOfSaleCATEGORYID
	AND C.PointOfSaleID = @PointOfSaleId AND A.BARCODE = @barcode; 
	
END
' 
END
/****** Object:  StoredProcedure [dbo].[GetNewCounterCloseIDByPointOfSaleID]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewCounterCloseIDByPointOfSaleID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE GetNewCounterCloseIDByPointOfSaleID
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT isnull(max(right(CounterCloseID,4)),''0'') from CounterCloseLog
	where substring(CounterCloseID,3,6) = substring(convert(varchar,getdate(),112),3,6)
	AND convert(int,substring(CounterCloseID,9,4)) = @PointOfSaleId;
END
' 
END
/****** Object:  View [dbo].[ViewReceiptSummary]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewReceiptSummary]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[ViewReceiptSummary]
AS
SELECT     dbo.ReceiptHdr.ReceiptDate, dbo.ReceiptHdr.ReceiptRefNo, dbo.ReceiptHdr.IsVoided, SUM(dbo.ReceiptDet.Amount) AS TotalReceiptAmount, 
                      dbo.ReceiptHdr.CashierID, dbo.ReceiptHdr.ReceiptHdrID
FROM         dbo.ReceiptDet INNER JOIN
                      dbo.ReceiptHdr ON dbo.ReceiptDet.ReceiptHdrID = dbo.ReceiptHdr.ReceiptHdrID
GROUP BY dbo.ReceiptHdr.ReceiptDate, dbo.ReceiptHdr.ReceiptRefNo, dbo.ReceiptDet.Amount, dbo.ReceiptHdr.IsVoided, dbo.ReceiptHdr.CashierID, 
                      dbo.ReceiptHdr.ReceiptHdrID
' 
/****** Object:  Table [dbo].[InventoryHdr]    Script Date: 05/09/2007 20:35:27 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryHdr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InventoryHdr](
	[InventoryHdrRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[InventoryDate] [datetime] NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PurchaseOrderNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[InvoiceNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DeliveryOrderNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Supplier] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Tax] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PointOfSaleId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_InventoryHdr_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_InventoryHdr] PRIMARY KEY CLUSTERED 
(
	[InventoryHdrRefNo] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  StoredProcedure [dbo].[GetNewInventoryRefNoByPointOfSaleID]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewInventoryRefNoByPointOfSaleID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNewInventoryRefNoByPointOfSaleID]
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here	 
	SELECT isnull(max(right(InventoryHdrRefNo,4)),''0'') from InventoryHdr
	where substring(InventoryHdrRefNo,3,6) = substring(convert(varchar,getdate(),112),3,6)
	AND convert(int,substring(InventoryHdrRefNo,9,4)) = @PointOfSaleId;
END
' 
END
/****** Object:  Table [dbo].[UserPrivilege]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPrivilege]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserPrivilege](
	[UserPrivilegeID] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[FormName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserPrivilege] PRIMARY KEY CLUSTERED 
(
	[UserPrivilegeID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[Outlet]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Outlet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Outlet](
	[OutletName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OutletAddress] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PhoneNo] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Outlet_1] PRIMARY KEY CLUSTERED 
(
	[OutletName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  StoredProcedure [dbo].[GetMaxItemRefNo]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxItemRefNo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMaxItemRefNo]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT isnull(max(right(itemno,8)),''0'') from Item	
END
' 
END
/****** Object:  Table [dbo].[EditInvLog]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EditInvLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EditInvLog](
	[EditInvLogId] [uniqueidentifier] NOT NULL,
	[InventoryHdrRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[EditInvDate] [datetime] NOT NULL,
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WitnessName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_EditInvLog_1] PRIMARY KEY CLUSTERED 
(
	[EditInvLogId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[EditBillLog]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EditBillLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EditBillLog](
	[EditBillLogID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_EditBillLog_EditBillLogID]  DEFAULT (newid()),
	[OrderHdrRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PreviousAmount] [money] NOT NULL,
	[NewAmount] [money] NOT NULL,
	[EditDateTime] [datetime] NOT NULL,
	[userName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[witnessName] [nchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_EditBillLog] PRIMARY KEY CLUSTERED 
(
	[EditBillLogID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[PaymentType]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PaymentType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PaymentType](
	[PaymentTypeName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_PaymentType_1] PRIMARY KEY CLUSTERED 
(
	[PaymentTypeName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[Item]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Item]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Item](
	[ItemNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ItemName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Barcode] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CategoryName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Price] [money] NOT NULL,
	[MinPrice] [money] NULL CONSTRAINT [DF_Item_MinPrice]  DEFAULT ((0)),
	[ItemDesc] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsInInventory] [bit] NULL,
	[Brand] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Remark] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Item_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_Item_1] PRIMARY KEY CLUSTERED 
(
	[ItemNo] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[CashRecording]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CashRecording]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CashRecording](
	[CashRecRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CashRecordingTime] [datetime] NOT NULL,
	[amount] [money] NOT NULL,
	[CashRecordingTypeId] [int] NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[CashierName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SupervisorName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_CashRecording_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_CashRecording_1] PRIMARY KEY CLUSTERED 
(
	[CashRecRefNo] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[GroupUserPrivilege]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupUserPrivilege]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GroupUserPrivilege](
	[GroupUserPrivilegeID] [int] IDENTITY(1,1) NOT NULL,
	[GroupID] [int] NOT NULL,
	[UserPrivilegeID] [int] NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_GroupUserPrivilege] PRIMARY KEY CLUSTERED 
(
	[GroupUserPrivilegeID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[UserMst]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserMst]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserMst](
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Password] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[GroupName] [int] NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_UserMst] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[ReceiptDet]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReceiptDet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ReceiptDet](
	[ReceiptDetID] [varchar](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ReceiptHdrID] [varchar](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PaymentTypeName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Amount] [money] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ReceiptDet_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_ReceiptDet] PRIMARY KEY CLUSTERED 
(
	[ReceiptDetID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[InventoryDet]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryDet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InventoryDet](
	[InventoryDetRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ItemNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[InventoryHdrRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ExpiryDate] [datetime] NULL,
	[Quantity] [int] NOT NULL,
	[CostOfGoods] [money] NULL,
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Discount] [decimal](18, 2) NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_InventoryDet] PRIMARY KEY CLUSTERED 
(
	[InventoryDetRefNo] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[Promotion]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Promotion]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Promotion](
	[promotionID] [int] NOT NULL,
	[ItemNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[DateFr] [datetime] NOT NULL,
	[DateTo] [datetime] NOT NULL,
	[Qty] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_PROMOTION] PRIMARY KEY CLUSTERED 
(
	[promotionID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[Camera]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Camera]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Camera](
	[CameraId] [int] IDENTITY(1,1) NOT NULL,
	[CameraIp] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CameraNo] [int] NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_CameraMst] PRIMARY KEY CLUSTERED 
(
	[CameraId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[CounterCloseLog]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CounterCloseLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CounterCloseLog](
	[CounterCloseID] [varchar](16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ActualClosingBalance] [money] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[Cashier] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Supervisor] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CashIn] [money] NOT NULL,
	[CashOut] [money] NOT NULL,
	[OpeningBalance] [money] NOT NULL,
	[TotalCollected] [money] NOT NULL,
	[Variance] [money] NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_CounterCloseLog_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_CounterCloseLog] PRIMARY KEY CLUSTERED 
(
	[CounterCloseID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[LoginActivity]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoginActivity]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LoginActivity](
	[LoginActivityID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_LoginActivity_LoginActivityID]  DEFAULT (newid()),
	[UserName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoginType] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[LoginDateTime] [datetime] NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_LoginActivity] PRIMARY KEY CLUSTERED 
(
	[LoginActivityID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[OrderHdr]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderHdr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderHdr](
	[OrderHdrID] [varchar](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[OrderRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL CONSTRAINT [DF_OrderHdr_Discount]  DEFAULT ((0)),
	[ServiceCharge] [decimal](18, 2) NOT NULL CONSTRAINT [DF_OrderHdr_ServiceCharge]  DEFAULT ((0)),
	[CashierID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[IsSuspended] [bit] NOT NULL CONSTRAINT [DF_OrderHdr_IsSuspended]  DEFAULT ((0)),
	[IsVoided] [bit] NOT NULL CONSTRAINT [DF_OrderHdr_IsVoided]  DEFAULT ((0)),
	[Remark] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OrderHdr_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_OrderHdr] PRIMARY KEY CLUSTERED 
(
	[OrderHdrID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[OrderDet]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderDet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderDet](
	[OrderDetID] [varchar](18) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ItemNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Discount] [money] NOT NULL CONSTRAINT [DF_OrderDet_Discount]  DEFAULT ((0)),
	[Amount] [money] NOT NULL CONSTRAINT [DF_OrderDet_Amount]  DEFAULT ((0)),
	[IsVoided] [bit] NULL,
	[OrderHdrID] [varchar](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_OrderDet_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_orderdet] PRIMARY KEY CLUSTERED 
(
	[OrderDetID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[ReceiptHdr]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ReceiptHdr]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ReceiptHdr](
	[ReceiptHdrID] [varchar](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ReceiptRefNo] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[ReceiptDate] [datetime] NOT NULL,
	[CashierID] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[OrderHdrID] [varchar](14) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Amount] [money] NOT NULL,
	[Tax] [float] NULL,
	[IsVoided] [bit] NOT NULL CONSTRAINT [DF_ReceiptHdr_IsVoided]  DEFAULT ((0)),
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ReceiptHdr_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_ReceiptHdr] PRIMARY KEY CLUSTERED 
(
	[ReceiptHdrID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  Table [dbo].[PointOfSale]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PointOfSale]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PointOfSale](
	[PointOfSaleID] [int] IDENTITY(1,1) NOT NULL,
	[PointOfSaleName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PointOfSaleDescription] [varchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OutletName] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PhoneNo] [varchar](15) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_PointOfSale] PRIMARY KEY CLUSTERED 
(
	[PointOfSaleID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
/****** Object:  StoredProcedure [dbo].[FetchItemNameListByPointOfSaleID]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchItemNameListByPointOfSaleID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchItemNameListByPointOfSaleID]	
	@PointOfSaleId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Select distinct a.ItemName
	from Item a, ItemCatPointOfSaleCat b, category c, PointOfSale d
	where a.CategoryName = c.CategoryName 
	and a.CategoryName = b.CategoryName	
	and d.PointOfSaleCategoryId = b.PointOfSaleCategoryId
	and d.PointOfSaleid = @PointOfSaleId;	
END
' 
END
/****** Object:  StoredProcedure [dbo].[FetchCategoryNameByPointOfSaleId]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchCategoryNameByPointOfSaleId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchCategoryNameByPointOfSaleId]
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    Select distinct b.CategoryName
	from Item a, ItemCatPointOfSaleCat b, category c, PointOfSale d
	where a.CategoryName = c.CategoryName 
	and a.CategoryName = b.CategoryName	
	and d.PointOfSaleCategoryId = b.PointOfSaleCategoryId
	and d.PointOfSaleid = @PointOfSaleId;	
END
' 
END
/****** Object:  View [dbo].[ViewCashRecording]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewCashRecording]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewCashRecording
AS
SELECT     dbo.CashRecording.CashRecordingTime, dbo.CashRecording.CashRecRefNo, dbo.CashRecordingType.CashRecordingTypeName, 
                      dbo.CashRecording.amount, dbo.CashRecording.CashierName, dbo.CashRecording.SupervisorName, dbo.CashRecording.Remark, 
                      dbo.Outlet.OutletName, dbo.PointOfSale.PointOfSaleName, dbo.PointOfSale.PointOfSaleID
FROM         dbo.PointOfSale INNER JOIN
                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName INNER JOIN
                      dbo.CashRecording INNER JOIN
                      dbo.CashRecordingType ON dbo.CashRecording.CashRecordingTypeId = dbo.CashRecordingType.CashRecordingTypeId ON 
                      dbo.PointOfSale.PointOfSaleID = dbo.CashRecording.PointOfSaleID
' 
/****** Object:  View [dbo].[ViewGroupPrivileges]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewGroupPrivileges]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewGroupPrivileges
AS
SELECT     dbo.UserGroup.GroupName, dbo.UserPrivilege.PrivilegeName, dbo.UserPrivilege.FormName, dbo.UserGroup.GroupDescription
FROM         dbo.GroupUserPrivilege INNER JOIN
                      dbo.UserGroup ON dbo.GroupUserPrivilege.GroupID = dbo.UserGroup.GroupID INNER JOIN
                      dbo.UserPrivilege ON dbo.GroupUserPrivilege.UserPrivilegeID = dbo.UserPrivilege.UserPrivilegeID
' 
/****** Object:  View [dbo].[ViewSalesDetail]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewSalesDetail]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewSalesDetail
AS
SELECT     TOP (100) PERCENT dbo.OrderHdr.OrderDate, dbo.OrderHdr.OrderRefNo, dbo.Item.ItemName, dbo.Item.CategoryName, dbo.OrderDet.Quantity, 
                      dbo.OrderDet.Amount, dbo.OrderHdr.CashierID, dbo.ReceiptHdr.Amount AS TotalReceiptAmount, dbo.Outlet.OutletName, dbo.PointOfSale.PointOfSaleName
FROM         dbo.Item INNER JOIN
                      dbo.OrderDet ON dbo.Item.ItemNo = dbo.OrderDet.ItemNo INNER JOIN
                      dbo.OrderHdr ON dbo.OrderDet.OrderHdrID = dbo.OrderHdr.OrderHdrID INNER JOIN
                      dbo.PointOfSale ON dbo.OrderHdr.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN
                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName INNER JOIN
                      dbo.ReceiptHdr ON dbo.OrderHdr.OrderHdrID = dbo.ReceiptHdr.OrderHdrID
ORDER BY dbo.OrderHdr.OrderDate DESC, dbo.OrderHdr.OrderRefNo DESC, dbo.Item.ItemName, dbo.Item.CategoryName, dbo.OrderHdr.CashierID, 
                      dbo.PointOfSale.PointOfSaleName, dbo.Outlet.OutletName
' 
/****** Object:  View [dbo].[ViewTransactions]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewTransactions]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewTransactions
AS
SELECT     dbo.OrderHdr.OrderRefNo, dbo.OrderHdr.OrderDate, dbo.ReceiptHdr.Amount, dbo.OrderHdr.CashierID, dbo.PointOfSale.PointOfSaleName, dbo.Outlet.OutletName, 
                      dbo.PointOfSale.PointOfSaleID
FROM         dbo.OrderHdr INNER JOIN
                      dbo.ReceiptHdr ON dbo.OrderHdr.OrderHdrID = dbo.ReceiptHdr.OrderHdrID INNER JOIN
                      dbo.PointOfSale ON dbo.OrderHdr.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN
                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName
' 
/****** Object:  View [dbo].[ViewInventoryActivity]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewInventoryActivity]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewInventoryActivity
AS
SELECT     dbo.InventoryDet.InventoryDetRefNo, dbo.InventoryDet.ItemNo, dbo.InventoryDet.Quantity, dbo.InventoryDet.Remark AS ItemRemark, 
                      dbo.PointOfSale.PointOfSaleName, dbo.Outlet.OutletName, dbo.InventoryHdr.InvoiceNo, dbo.InventoryHdr.Supplier, dbo.InventoryHdr.Remark, 
                      dbo.InventoryHdr.InventoryDate, dbo.InventoryHdr.UserName, dbo.InventoryHdr.InventoryHdrRefNo, dbo.Item.ItemName, dbo.Item.CategoryName, 
                      dbo.InventoryHdr.PointOfSaleId
FROM         dbo.InventoryDet INNER JOIN
                      dbo.InventoryHdr ON dbo.InventoryDet.InventoryHdrRefNo = dbo.InventoryHdr.InventoryHdrRefNo INNER JOIN
                      dbo.PointOfSale ON dbo.InventoryHdr.PointOfSaleId = dbo.PointOfSale.PointOfSaleID INNER JOIN
                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName INNER JOIN
                      dbo.Item ON dbo.InventoryDet.ItemNo = dbo.Item.ItemNo
GROUP BY dbo.InventoryDet.InventoryDetRefNo, dbo.InventoryDet.ItemNo, dbo.InventoryDet.Quantity, dbo.InventoryDet.Remark, dbo.PointOfSale.PointOfSaleName, 
                      dbo.Outlet.OutletName, dbo.InventoryHdr.InvoiceNo, dbo.InventoryHdr.Supplier, dbo.InventoryHdr.Remark, dbo.InventoryHdr.InventoryDate, 
                      dbo.InventoryHdr.UserName, dbo.InventoryHdr.InventoryHdrRefNo, dbo.Item.ItemName, dbo.Item.CategoryName, dbo.InventoryHdr.PointOfSaleId
' 
/****** Object:  StoredProcedure [dbo].[FetchStockReport]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchStockReport]
	-- Add the parameters for the stored procedure here	
	@itemname varchar(50),
	@CategoryName varchar(50),
	@PointOfSaleName varchar(50),
	@OutletName varchar(50),		
	@sortby varchar(50),
	@sortdir varchar(5),
	@report_month int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	set @sortby = LTRIM(RTRIM(@sortby));
	set @sortdir = LTRIM(RTRIM(@sortdir));
	if (@OutletName = ''ALL'' AND @PointOfSaleName = ''ALL'') --See total amt for all outlets 
	Begin	
		-- Insert statements for procedure here
		SELECT dbo.Item.ItemNo, dbo.Item.ItemName, 
			   SUM(dbo.InventoryDet.Quantity) AS OnHand, 
			   dbo.Item.CategoryName,
			   ''ALL'' as OutletName,
		       ''ALL'' as PointOfSaleName
		FROM  dbo.InventoryHdr INNER JOIN
			  dbo.InventoryDet ON dbo.InventoryDet.InventoryHdrRefNo = dbo.InventoryHdr.InventoryHdrRefNo INNER JOIN
              dbo.Item ON dbo.InventoryDet.ItemNo = dbo.Item.ItemNo INNER JOIN
			  dbo.PointOfSale ON dbo.PointOfSale.PointOfSaleID = dbo.InventoryHdr.PointOfSaleID INNER JOIN
			  dbo.Outlet ON dbo.Outlet.OutletName = dbo.PointOfSale.OutletName
		WHERE  
			   Item.ItemName Like @itemname 
			   AND Item.IsInInventory = 1			   
			   AND Item.CategoryName Like @CategoryName	
			   AND datepart(mm,InventoryDate) = @report_month
			   AND datepart(yy,InventoryDate) = datepart(yy,GetDate())
		GROUP BY Item.ItemNo, Item.ItemName, Item.CategoryName
		ORDER BY
		CASE  	WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)				
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) desc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) asc)
				ELSE rank() over (order by Item.ItemName asc)
		END 
	End
	Else if (@PointOfSaleName = ''ALL'' and @OutletName != ''ALL'')  --See total amt for PointOfSales at one outlet
	Begin
		-- Insert statements for procedure here
		SELECT dbo.Item.ItemNo, dbo.Item.ItemName, 
			   SUM(dbo.InventoryDet.Quantity) AS OnHand, 
			   dbo.Item.CategoryName,
			   Outlet.OutletName,
				''ALL'' as PointOfSaleName
		FROM  dbo.InventoryHdr INNER JOIN
			  dbo.InventoryDet ON dbo.InventoryDet.InventoryHdrRefNo = dbo.InventoryHdr.InventoryHdrRefNo INNER JOIN
              dbo.Item ON dbo.InventoryDet.ItemNo = dbo.Item.ItemNo INNER JOIN
			  dbo.PointOfSale ON dbo.PointOfSale.PointOfSaleID = dbo.InventoryHdr.PointOfSaleID INNER JOIN
			  dbo.Outlet ON dbo.Outlet.OutletName = dbo.PointOfSale.OutletName
		WHERE  
			   Item.ItemName Like @itemname 
			   AND Item.IsInInventory = 1			   
			   AND Item.CategoryName Like @CategoryName
			   AND Outlet.OutletName Like @OutletName	
			   AND datepart(mm,InventoryDate) = @report_month
			   AND datepart(yy,InventoryDate) = datepart(yy,GetDate())
		GROUP BY Item.ItemNo, Item.ItemName, Item.CategoryName,Outlet.OutletName
		ORDER BY
		CASE  	WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)				
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) desc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) asc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Outlet.OutletName desc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Outlet.OutletName asc)
				ELSE rank() over (order by Item.ItemName asc)
		END 
	End
	Else if (@PointOfSaleName != ''ALL'' and @OutletName = ''ALL'')  	
	begin
		-- Insert statements for procedure here
		SELECT dbo.Item.ItemNo, dbo.Item.ItemName, 
			   SUM(dbo.InventoryDet.Quantity) AS OnHand, 
			   dbo.Item.CategoryName,
			   ''ALL'' as OutletName,
		       PointOfSale.PointOfSaleName
		FROM  dbo.InventoryHdr INNER JOIN
			  dbo.InventoryDet ON dbo.InventoryDet.InventoryHdrRefNo = dbo.InventoryHdr.InventoryHdrRefNo INNER JOIN
              dbo.Item ON dbo.InventoryDet.ItemNo = dbo.Item.ItemNo INNER JOIN
			  dbo.PointOfSale ON dbo.PointOfSale.PointOfSaleID = dbo.InventoryHdr.PointOfSaleID INNER JOIN
			  dbo.Outlet ON dbo.Outlet.OutletName = dbo.PointOfSale.OutletName
		WHERE  
			   Item.ItemName Like @itemname 
			   AND Item.IsInInventory = 1			   
			   AND Item.CategoryName Like @CategoryName			   
			   AND PointOfSale.PointOfSaleName Like @PointOfSaleName	
			   AND datepart(mm,InventoryDate) = @report_month
			   AND datepart(yy,InventoryDate) = datepart(yy,GetDate())
		GROUP BY Item.ItemNo, Item.ItemName, Item.CategoryName,PointOfSale.PointOfSaleName
		ORDER BY
		CASE  	WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)				
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) desc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) asc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)				
				ELSE rank() over (order by Item.ItemName asc)
		END 
	End
	Else
	Begin
		-- Insert statements for procedure here
		SELECT dbo.Item.ItemNo, dbo.Item.ItemName, 
			   SUM(dbo.InventoryDet.Quantity) AS OnHand, 
			   dbo.Item.CategoryName,
			   Outlet.OutletName,
		       PointOfSale.PointOfSaleName
		FROM  dbo.InventoryHdr INNER JOIN
			  dbo.InventoryDet ON dbo.InventoryDet.InventoryHdrRefNo = dbo.InventoryHdr.InventoryHdrRefNo INNER JOIN
              dbo.Item ON dbo.InventoryDet.ItemNo = dbo.Item.ItemNo INNER JOIN 
		      dbo.PointOfSale ON dbo.PointOfSale.PointOfSaleID = dbo.InventoryHdr.PointOfSaleID INNER JOIN
			  dbo.Outlet ON dbo.Outlet.OutletName = dbo.PointOfSale.OutletName
		WHERE  
			   Item.ItemName Like @itemname 
			   AND Item.IsInInventory = 1			   
			   AND Item.CategoryName Like @CategoryName
			   AND Outlet.OutletName Like @OutletName
			   AND PointOfSale.PointOfSaleName Like @PointOfSaleName	
			   AND datepart(mm,InventoryDate) = @report_month
               AND datepart(yy,InventoryDate) = datepart(yy,GetDate())
		GROUP BY Item.ItemNo, Item.ItemName, Item.CategoryName,Outlet.OutletName,PointOfSale.PointOfSaleName
		ORDER BY
		CASE  	WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)				
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) desc)
				WHEN @sortby = ''OnHand'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(InventoryDet.Quantity) asc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)				
				WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Outlet.OutletName desc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Outlet.OutletName asc)
				ELSE rank() over (order by Item.ItemName asc)
		END 
	End
END
' 
END
/****** Object:  View [dbo].[ViewInventoryItemOnHandQty]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewInventoryItemOnHandQty]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewInventoryItemOnHandQty
AS
SELECT     dbo.Item.ItemNo, dbo.Item.ItemName, SUM(dbo.InventoryDet.Quantity) AS OnHand, dbo.Item.CategoryName
FROM         dbo.InventoryDet INNER JOIN
                      dbo.Item ON dbo.InventoryDet.ItemNo = dbo.Item.ItemNo
WHERE     (dbo.Item.IsInInventory = 1)
GROUP BY dbo.Item.ItemNo, dbo.Item.ItemName, dbo.Item.CategoryName
' 
/****** Object:  StoredProcedure [dbo].[FetchCollectionReport]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchCollectionReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchCollectionReport]
	-- Add the parameters for the stored procedure here
	@startdate datetime,
	@enddate datetime,	
	@PointOfSaleName varchar(50),
	@OutletName varchar(50),		
	@sortby varchar(50),
	@sortdir varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	set @sortby = LTRIM(RTRIM(@sortby));
	set @sortdir = LTRIM(RTRIM(@sortdir));
	if (@OutletName = ''ALL'' AND @PointOfSaleName = ''ALL'') --See total amt for all outlets 
	Begin				
		SELECT isNull(SUM(CounterCloseLog.ActualClosingBalance),0) AS TotalCollected, 
			   ''ALL'' as PointOfSaleName, ''ALL'' as OutletName		   
		FROM  CounterCloseLog INNER JOIN			  
			  PointOfSale ON CounterCloseLog.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (CounterCloseLog.EndDateTime BETWEEN @startdate AND @enddate)			  		
		ORDER BY
		CASE    
				WHEN @sortby = ''TotalCollected'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) desc)
				WHEN @sortby = ''TotalCollected'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
				ELSE rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
		END 
	End
	Else if (@PointOfSaleName = ''ALL'' and @OutletName != ''ALL'')  --See total amt for PointOfSales at one outlet
	Begin
		SELECT isNull(SUM(CounterCloseLog.ActualClosingBalance),0) AS TotalCollected, 
			   ''ALL'' as PointOfSaleName, Outlet.OutletName		   
		FROM  CounterCloseLog INNER JOIN			  
			  PointOfSale ON CounterCloseLog.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (CounterCloseLog.EndDateTime BETWEEN @startdate AND @enddate)			   
			   AND Outlet.OutletName Like @OutletName			   		
		GROUP BY 
				 Outlet.OutletName			
		ORDER BY
			CASE    
					WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
					THEN rank() over (order by Outlet.OutletName desc)
					WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
					THEN rank() over (order by Outlet.OutletName asc)				
					WHEN @sortby = ''TotalCollected'' and @sortdir = ''DESC'' 
					THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) desc)
					WHEN @sortby = ''TotalCollected'' and @sortdir = ''ASC'' 
					THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
					ELSE rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
			END 
	End
	Else if (@PointOfSaleName != ''ALL'' and @OutletName = ''ALL'')  
		--See total amt for a PointOfSale without particular outlet
	begin
		set @OutletName = ''%'';
		SELECT isNull(SUM(CounterCloseLog.ActualClosingBalance),0) AS TotalCollected, 
				PointOfSale.PointOfSaleName, ''ALL'' as OutletName		   
		FROM  CounterCloseLog INNER JOIN			  
			  PointOfSale ON CounterCloseLog.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (CounterCloseLog.EndDateTime BETWEEN @startdate AND @enddate)
			   AND PointOfSale.PointOfSaleName Like @PointOfSaleName			   
		GROUP BY 
				 PointOfSale.PointOfSaleName
		ORDER BY
		CASE    
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)
				WHEN @sortby = ''TotalCollected'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) desc)
				WHEN @sortby = ''TotalCollected'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
				ELSE rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
		END 		
	end
	Else
	Begin   --See total amt for one PointOfSale --OutletName and PointOfSale name is specified
		SELECT isNull(SUM(CounterCloseLog.ActualClosingBalance),0) AS TotalCollected, 
		   PointOfSale.PointOfSaleName, Outlet.OutletName		   
		FROM  CounterCloseLog INNER JOIN			  
			  PointOfSale ON CounterCloseLog.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (CounterCloseLog.EndDateTime BETWEEN @startdate AND @enddate)
			   AND PointOfSale.PointOfSaleName Like @PointOfSaleName
			   AND Outlet.OutletName Like @OutletName			   		
		GROUP BY 
				 PointOfSale.PointOfSaleName, Outlet.OutletName		ORDER BY
		CASE    WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)
				WHEN @sortby = ''TotalCollected'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) desc)
				WHEN @sortby = ''TotalCollected'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
				ELSE rank() over (order by sum(CounterCloseLog.ActualClosingBalance) asc)
		END 		
	End		
end' 
END
/****** Object:  StoredProcedure [dbo].[FetchProductSalesReport]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchProductSalesReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchProductSalesReport]
	-- Add the parameters for the stored procedure here
	@startdate datetime,
	@enddate datetime,
	@itemname varchar(50),
	@PointOfSaleName varchar(50),
	@OutletName varchar(50),
	@CategoryName varchar(50),
	@IsVoided bit,
	@sortby varchar(50),
	@sortdir varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	set @sortby = LTRIM(RTRIM(@sortby));
	set @sortdir = LTRIM(RTRIM(@sortdir));
	if (@OutletName = ''ALL'' AND @PointOfSaleName = ''ALL'') --See total amt for all outlets 
	Begin				
		SELECT Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, ''ALL'' AS PointOfSaleName, ''ALL'' AS OutletName,
		   Item.CategoryName
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
			   AND Item.ItemName Like @itemname 			   
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
		GROUP BY Item.ItemNo, Item.ItemName, OrderDet.IsVoided, Item.CategoryName
		ORDER BY
		CASE    WHEN @sortby = ''ItemNo'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemNo desc)
				WHEN @sortby = ''ItemNo'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemNo asc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)				
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.ItemName asc)
		END 
	End
	Else if (@PointOfSaleName = ''ALL'' and @OutletName != ''ALL'')  --See total amt for PointOfSales at one outlet
	Begin
			SELECT Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity, 
			   SUM(OrderDet.Amount) AS TotalAmount, 
			   OrderDet.IsVoided, ''ALL'' AS PointOfSaleName, Outlet.OutletName,
			   Item.CategoryName
			FROM  Item INNER JOIN
				  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
				  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
				  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
				  Outlet ON PointOfSale.OutletName = Outlet.OutletName
			WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
				   AND Item.ItemName Like @itemname 
				   AND Outlet.OutletName Like @OutletName
				   AND Item.CategoryName Like @CategoryName
				   AND OrderDet.IsVoided = @IsVoided
			GROUP BY Item.ItemNo, Item.ItemName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName
			ORDER BY
			CASE    WHEN @sortby = ''ItemNo'' and @sortdir = ''DESC'' 
					THEN rank() over (order by Item.ItemNo desc)
					WHEN @sortby = ''ItemNo'' and @sortdir = ''ASC'' 
					THEN rank() over (order by Item.ItemNo asc)
					WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
					THEN rank() over (order by Item.ItemName desc)
					WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
					THEN rank() over (order by Item.ItemName asc)					
					WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
					THEN rank() over (order by OrderDet.IsVoided desc)
					WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
					THEN rank() over (order by OrderDet.IsVoided asc)
					WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
					THEN rank() over (order by Item.CategoryName desc)
					WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
					THEN rank() over (order by Item.CategoryName asc)
					WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
					THEN rank() over (order by sum(OrderDet.Amount) desc)
					WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
					THEN rank() over (order by sum(OrderDet.Amount) asc)
					WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
					THEN rank() over (order by sum(OrderDet.Quantity) desc)
					WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
					THEN rank() over (order by sum(OrderDet.Quantity) asc)
					ELSE rank() over (order by Item.ItemName asc)
			END 
	End
	Else if (@PointOfSaleName != ''ALL'' and @OutletName = ''ALL'')  
		--See total amt for a PointOfSale without particular outlet
	begin
		set @OutletName = ''%'';
		SELECT Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, PointOfSale.PointOfSaleID, PointOfSale.PointOfSaleName, Outlet.OutletName,
		   Item.CategoryName
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
			   AND Item.ItemName Like @itemname AND PointOfSale.PointOfSaleName Like @PointOfSaleName
			   AND Outlet.OutletName Like @OutletName
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
		GROUP BY Item.ItemNo, Item.ItemName, PointOfSale.PointOfSaleID, 
				 PointOfSale.PointOfSaleName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName
		ORDER BY
		CASE    WHEN @sortby = ''ItemNo'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemNo desc)
				WHEN @sortby = ''ItemNo'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemNo asc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Outlet.OutletName desc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Outlet.OutletName asc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.ItemName asc)
		END 		
	end
	Else
	Begin   --See total amt for one PointOfSale --OutletName and PointOfSale name is specified
		SELECT Item.ItemNo, Item.ItemName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, PointOfSale.PointOfSaleID, PointOfSale.PointOfSaleName, Outlet.OutletName,
		   Item.CategoryName
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
			   AND Item.ItemName Like @itemname AND PointOfSale.PointOfSaleName Like @PointOfSaleName
			   AND Outlet.OutletName Like @OutletName
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
		GROUP BY Item.ItemNo, Item.ItemName, PointOfSale.PointOfSaleID, 
				 PointOfSale.PointOfSaleName, OrderDet.IsVoided, Item.CategoryName, Outlet.OutletName
		ORDER BY
		CASE    WHEN @sortby = ''ItemNo'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemNo desc)
				WHEN @sortby = ''ItemNo'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemNo asc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.ItemName desc)
				WHEN @sortby = ''ItemName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.ItemName asc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.ItemName asc)
		END 		
	End		
end' 
END
/****** Object:  StoredProcedure [dbo].[FetchProductCategorySalesReport]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchProductCategorySalesReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[FetchProductCategorySalesReport]
	-- Add the parameters for the stored procedure here
		 @categoryname varchar(50),
		 @startdate datetime,
		 @enddate datetime,
		 @PointOfSaleName varchar(50),
		 @OutletName varchar(50),
		 @IsVoided bit,
		 @sortby varchar(50),
		 @sortdir varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	set @sortby = LTRIM(RTRIM(@sortby));
	set @sortdir = LTRIM(RTRIM(@sortdir));
	set @OutletName = LTRIM(RTRIM(@OutletName));

	if (@OutletName = ''ALL'' AND @PointOfSaleName = ''ALL'') --See total amt for all outlets 
	Begin	
		SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, @OutletName as OutletName, @PointOfSaleName as PointOfSaleName		   
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN
 			  Outlet ON PointOfSale.OutletName = Outlet.OutletName			  
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)			   
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
		GROUP BY Item.CategoryName,
				 OrderDet.IsVoided
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)				
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.CategoryName asc)
			END			
	End
	Else if (@PointOfSaleName = ''ALL'' and @OutletName != ''ALL'')  --See total amt for PointOfSales at one outlet
	Begin
	SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, ''ALL'' as PointOfSaleName, Outlet.OutletName		   
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
 			  Outlet ON PointOfSale.OutletName = Outlet.OutletName
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)			   
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
			   AND Outlet.OutletName Like @OutletName
		GROUP BY Item.CategoryName,
				 Outlet.OutletName, OrderDet.IsVoided
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Outlet.OutletName desc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Outlet.OutletName asc)								
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.CategoryName asc)
			END
	End
	Else if (@PointOfSaleName != ''ALL'' and @OutletName = ''ALL'')  	
	begin
	SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName		   
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
 			  Outlet ON PointOfSale.OutletName = Outlet.OutletName 			  
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
			   AND PointOfSale.PointOfSaleName Like @PointOfSaleName
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
		GROUP BY Item.CategoryName,
				 PointOfSale.PointOfSaleName, Outlet.OutletName, OrderDet.IsVoided
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)				
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Outlet.OutletName desc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Outlet.OutletName asc)	
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.CategoryName asc)
			END
	end
	Else
	Begin   --See total amt for one PointOfSale --OutletName and PointOfSale name is specified
    -- Insert statements for procedure here
		SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
		   SUM(OrderDet.Amount) AS TotalAmount, 
		   OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName		   
		FROM  Item INNER JOIN
			  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
			  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
			  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN
 			  Outlet ON PointOfSale.OutletName = Outlet.OutletName			  
		WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
			   AND PointOfSale.PointOfSaleName Like @PointOfSaleName
				AND Outlet.OutletName Like @OutletName
			   AND Item.CategoryName Like @CategoryName
			   AND OrderDet.IsVoided = @IsVoided
		GROUP BY Item.CategoryName, Outlet.OutletName,
				 PointOfSale.PointOfSaleName, OrderDet.IsVoided
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Item.CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Item.CategoryName asc)				
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName desc)
				WHEN @sortby = ''PointOfSaleName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by PointOfSale.PointOfSaleName asc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by Outlet.OutletName desc)
				WHEN @sortby = ''OutletName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by Outlet.OutletName asc)	
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by OrderDet.IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by OrderDet.IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Amount) desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Amount) asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by sum(OrderDet.Quantity) asc)
				ELSE rank() over (order by Item.CategoryName asc)
			END
	End
END
' 
END
/****** Object:  StoredProcedure [dbo].[GetNewOrderHdrNoByPointOfSaleId]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewOrderHdrNoByPointOfSaleId]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetNewOrderHdrNoByPointOfSaleId]
	-- Add the parameters for the stored procedure here	
	@PointOfSaleId int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;		
	
	SELECT isnull(max(right(orderhdrid,4)),''0'') from orderhdr
	where left(orderhdrid,6) = substring(convert(varchar,getdate(),112),3,6)
	AND convert(int,substring(orderhdrid,7,4)) = @PointOfSaleId;
	
	
END
' 
END
/****** Object:  View [dbo].[ViewCloseCounterReport]    Script Date: 05/09/2007 20:35:28 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewCloseCounterReport]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW dbo.ViewCloseCounterReport
AS
SELECT     dbo.CounterCloseLog.CounterCloseID, dbo.CounterCloseLog.ActualClosingBalance, dbo.CounterCloseLog.StartTime, 
                      dbo.CounterCloseLog.EndDateTime, dbo.CounterCloseLog.Cashier, dbo.CounterCloseLog.Supervisor, dbo.CounterCloseLog.CashIn, 
                      dbo.CounterCloseLog.CashOut, dbo.CounterCloseLog.OpeningBalance, dbo.CounterCloseLog.TotalCollected, dbo.CounterCloseLog.Variance, 
                      dbo.CounterCloseLog.PointOfSaleID, dbo.PointOfSale.PointOfSaleName, dbo.Outlet.OutletName
FROM         dbo.CounterCloseLog INNER JOIN
                      dbo.PointOfSale ON dbo.CounterCloseLog.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN
                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName
' 
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_Category]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Category] FOREIGN KEY([CategoryName])
REFERENCES [Category] ([CategoryName])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CashRecording_CashRecordingType1]') AND parent_object_id = OBJECT_ID(N'[dbo].[CashRecording]'))
ALTER TABLE [dbo].[CashRecording]  WITH CHECK ADD  CONSTRAINT [FK_CashRecording_CashRecordingType1] FOREIGN KEY([CashRecordingTypeId])
REFERENCES [CashRecordingType] ([CashRecordingTypeId])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CashRecording_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[CashRecording]'))
ALTER TABLE [dbo].[CashRecording]  WITH CHECK ADD  CONSTRAINT [FK_CashRecording_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [PointOfSale] ([PointOfSaleID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GroupUserPrivilege_GroupUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupUserPrivilege]'))
ALTER TABLE [dbo].[GroupUserPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_GroupUserPrivilege_GroupUser] FOREIGN KEY([GroupID])
REFERENCES [UserGroup] ([GroupID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GroupUserPrivilege_UserPrivilege]') AND parent_object_id = OBJECT_ID(N'[dbo].[GroupUserPrivilege]'))
ALTER TABLE [dbo].[GroupUserPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_GroupUserPrivilege_UserPrivilege] FOREIGN KEY([UserPrivilegeID])
REFERENCES [UserPrivilege] ([UserPrivilegeID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMst_UserGroup]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMst]'))
ALTER TABLE [dbo].[UserMst]  WITH CHECK ADD  CONSTRAINT [FK_UserMst_UserGroup] FOREIGN KEY([GroupName])
REFERENCES [UserGroup] ([GroupID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReceiptDet_PaymentType]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReceiptDet]'))
ALTER TABLE [dbo].[ReceiptDet]  WITH CHECK ADD  CONSTRAINT [FK_ReceiptDet_PaymentType] FOREIGN KEY([PaymentTypeName])
REFERENCES [PaymentType] ([PaymentTypeName])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReceiptDet_ReceiptHdr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReceiptDet]'))
ALTER TABLE [dbo].[ReceiptDet]  WITH CHECK ADD  CONSTRAINT [FK_ReceiptDet_ReceiptHdr] FOREIGN KEY([ReceiptHdrID])
REFERENCES [ReceiptHdr] ([ReceiptHdrID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Inventory_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryDet]'))
ALTER TABLE [dbo].[InventoryDet]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Item] FOREIGN KEY([ItemNo])
REFERENCES [Item] ([ItemNo])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryDet_InventoryHdr]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryDet]'))
ALTER TABLE [dbo].[InventoryDet]  WITH CHECK ADD  CONSTRAINT [FK_InventoryDet_InventoryHdr] FOREIGN KEY([InventoryHdrRefNo])
REFERENCES [InventoryHdr] ([InventoryHdrRefNo])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Promotion_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Promotion]'))
ALTER TABLE [dbo].[Promotion]  WITH CHECK ADD  CONSTRAINT [FK_Promotion_Item] FOREIGN KEY([ItemNo])
REFERENCES [Item] ([ItemNo])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Promotion_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[Promotion]'))
ALTER TABLE [dbo].[Promotion]  WITH CHECK ADD  CONSTRAINT [FK_Promotion_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [PointOfSale] ([PointOfSaleID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Camera_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[Camera]'))
ALTER TABLE [dbo].[Camera]  WITH CHECK ADD  CONSTRAINT [FK_Camera_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [PointOfSale] ([PointOfSaleID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CounterCloseLog_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[CounterCloseLog]'))
ALTER TABLE [dbo].[CounterCloseLog]  WITH CHECK ADD  CONSTRAINT [FK_CounterCloseLog_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [PointOfSale] ([PointOfSaleID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LoginActivity_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[LoginActivity]'))
ALTER TABLE [dbo].[LoginActivity]  WITH CHECK ADD  CONSTRAINT [FK_LoginActivity_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [PointOfSale] ([PointOfSaleID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHdr_PointOfSale]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHdr]'))
ALTER TABLE [dbo].[OrderHdr]  WITH CHECK ADD  CONSTRAINT [FK_OrderHdr_PointOfSale] FOREIGN KEY([PointOfSaleID])
REFERENCES [PointOfSale] ([PointOfSaleID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderDet_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderDet]'))
ALTER TABLE [dbo].[OrderDet]  WITH CHECK ADD  CONSTRAINT [FK_OrderDet_Item] FOREIGN KEY([ItemNo])
REFERENCES [Item] ([ItemNo])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderDet_OrderHdr]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderDet]'))
ALTER TABLE [dbo].[OrderDet]  WITH CHECK ADD  CONSTRAINT [FK_OrderDet_OrderHdr] FOREIGN KEY([OrderHdrID])
REFERENCES [OrderHdr] ([OrderHdrID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ReceiptHdr_OrderHdr]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReceiptHdr]'))
ALTER TABLE [dbo].[ReceiptHdr]  WITH CHECK ADD  CONSTRAINT [FK_ReceiptHdr_OrderHdr] FOREIGN KEY([OrderHdrID])
REFERENCES [OrderHdr] ([OrderHdrID])
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PointOfSale_Outlet]') AND parent_object_id = OBJECT_ID(N'[dbo].[PointOfSale]'))
ALTER TABLE [dbo].[PointOfSale]  WITH CHECK ADD  CONSTRAINT [FK_PointOfSale_Outlet] FOREIGN KEY([OutletName])
REFERENCES [Outlet] ([OutletName])


