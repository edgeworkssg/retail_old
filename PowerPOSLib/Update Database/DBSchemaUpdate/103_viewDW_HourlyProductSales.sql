IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductSales]'))
DROP VIEW [dbo].[viewDW_HourlyProductSales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductSales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlyProductSales] as 
select s.orderdate, s.outletname, s.itemno,
	   s.quantity, s.amount
from dw_hourlyproductsales s WITH(NOLOCK)
'