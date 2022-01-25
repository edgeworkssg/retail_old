IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[DW_HourlyPayment]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[DW_HourlyPayment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[OutletName] [varchar](50) NOT NULL,
	[PayByCash] [money] NULL,
	[Pay01] [money] NULL,
	[Pay02] [money] NULL,
	[Pay03] [money] NULL,
	[Pay04] [money] NULL,
	[Pay05] [money] NULL,
	[Pay06] [money] NULL,
	[Pay07] [money] NULL,
	[Pay08] [money] NULL,
	[Pay09] [money] NULL,
	[Pay10] [money] NULL,
	[Pay11] [money] NULL,
	[Pay12] [money] NULL,
	[Pay13] [money] NULL,
	[Pay14] [money] NULL,
	[Pay15] [money] NULL,
	[Pay16] [money] NULL,
	[Pay17] [money] NULL,
	[Pay18] [money] NULL,
	[Pay19] [money] NULL,
	[Pay20] [money] NULL,
	[Pay21] [money] NULL,
	[Pay22] [money] NULL,
	[Pay23] [money] NULL,
	[Pay24] [money] NULL,
	[Pay25] [money] NULL,
	[Pay26] [money] NULL,
	[Pay27] [money] NULL,
	[Pay28] [money] NULL,
	[Pay29] [money] NULL,
	[Pay30] [money] NULL,
	[Pay31] [money] NULL,
	[Pay32] [money] NULL,
	[Pay33] [money] NULL,
	[Pay34] [money] NULL,
	[Pay35] [money] NULL,
	[Pay36] [money] NULL,
	[Pay37] [money] NULL,
	[Pay38] [money] NULL,
	[Pay39] [money] NULL,
	[Pay40] [money] NULL,
	[PayOthers] [money] NULL,
	[Totalpayment] [money] NULL,
	[PointAllocated] [money] NULL,
	[PayByInstallment] [money] NULL,
	[PayByPoint] [money] NULL,
	[Regenerate] [int] NULL,
	[LastUpdateOn] [datetime] NULL,
 CONSTRAINT [PK_DW_HourlyPayment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LastUpdateOn' AND OBJECT_ID = OBJECT_ID(N'[DW_HourlyPayment]')) BEGIN 
	ALTER TABLE DW_HourlyPayment ADD LastUpdateOn [datetime] NULL;
END 
  