IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[DW_HourlySales]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[DW_HourlySales](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[orderdate] [datetime] NOT NULL,
	[outletname] [varchar](50) NOT NULL,
	[pax] [int] NULL,
	[bill] [int] NULL,
	[grossamount] [money] NULL,
	[disc] [money] NULL,
	[afterdisc] [money] NULL,
	[svccharge] [money] NULL,
	[befgst] [money] NULL,
	[gst] [money] NULL,
	[rounding] [money] NULL,
	[nettamount] [money] NULL,
	[pointSale] [money] NULL,
	[installmentPaymentSale] [money] NULL,
	[regenerate] [int] NULL,
	[LastUpdateOn] [datetime] NULL,
 CONSTRAINT [PK_DW_HourlySales] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [uq_dw_hourlysales] UNIQUE NONCLUSTERED 
(
	[orderdate] ASC,
	[outletname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LastUpdateOn' AND OBJECT_ID = OBJECT_ID(N'[DW_HourlySales]')) BEGIN 
	ALTER TABLE DW_HourlySales ADD LastUpdateOn [datetime] NULL;
END 
  