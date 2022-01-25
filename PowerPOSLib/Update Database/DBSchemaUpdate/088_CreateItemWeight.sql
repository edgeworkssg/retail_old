IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemWeight]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ItemWeight](
	[Barcode] [varchar](100) NULL,
	[ActualWeight] [int] NULL,
	[LastWeight] [int] NULL,
	[Min] [int] NULL,
	[Max] [int] NULL,
	[Avg] [int] NULL,
	[Count] [int] NULL,
	[PointOfSaleID] [int] NULL
) ON [PRIMARY]
END