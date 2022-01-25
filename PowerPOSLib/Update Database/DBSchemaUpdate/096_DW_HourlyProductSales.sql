IF  NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[DW_HourlyProductSales]') AND type in (N'U')) BEGIN

CREATE TABLE [dbo].[DW_HourlyProductSales](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[orderdate] [datetime] NOT NULL,
	[outletname] [varchar](50) NOT NULL,
	[itemno] [varchar](50) NOT NULL,
	[quantity] [decimal](18, 2) NULL,
	[amount] [money] NULL,
	[regenerate] [int] NULL,
	[LastUpdateOn] [datetime] NULL,
 CONSTRAINT [PK_DW_HourlyProductSales] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'LastUpdateOn' AND OBJECT_ID = OBJECT_ID(N'[DW_HourlyProductSales]')) BEGIN 
	ALTER TABLE DW_HourlyProductSales ADD LastUpdateOn [datetime] NULL;
END 
  