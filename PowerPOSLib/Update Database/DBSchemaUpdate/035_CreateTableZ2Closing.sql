IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Z2ClosingLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Z2ClosingLog](
	[Z2ClosingLogID] [varchar](14) NOT NULL,
	[FloatBalance] [money] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[Cashier] [varchar](50) NOT NULL,
	[Supervisor] [varchar](50) NOT NULL,
	[CashIn] [money] NOT NULL,
	[CashOut] [money] NOT NULL,
	[OpeningBalance] [money] NOT NULL,
	[TotalSystemRecorded] [money] NOT NULL,
	[CashCollected] [money] NOT NULL,
	[CashRecorded] [money] NULL,
	[NetsCollected] [money] NOT NULL,
	[NetsRecorded] [money] NULL,
	[NetsTerminalID] [varchar](50) NULL,
	[VisaCollected] [money] NOT NULL,
	[VisaRecorded] [money] NULL,
	[VisaBatchNo] [varchar](50) NULL,
	[AmexCollected] [money] NOT NULL,
	[AmexRecorded] [money] NULL,
	[AmexBatchNo] [varchar](50) NULL,
	[ChinaNetsCollected] [money] NOT NULL,
	[ChinaNetsRecorded] [money] NULL,
	[ChinaNetsTerminalID] [varchar](50) NULL,
	[VoucherCollected] [money] NOT NULL,
	[VoucherRecorded] [money] NULL,
	[DepositBagNo] [varchar](50) NULL,
	[TotalActualCollected] [money] NOT NULL,
	[ClosingCashOut] [money] NOT NULL,
	[Variance] [money] NOT NULL,
	[PointOfSaleID] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [varchar](50) NOT NULL,
	[UniqueID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Z2ClosingLog_UniqueID]  DEFAULT (newid()),
 CONSTRAINT [PK_Z2ClosingLog] PRIMARY KEY CLUSTERED 
(
	[Z2ClosingLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

