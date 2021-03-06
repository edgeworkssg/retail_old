IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailyProductSales]'))
DROP VIEW [dbo].[viewDW_DailyProductSales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailyProductSales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_DailyProductSales] as 
select cast(s.orderdate as date) orderdate, s.outletname, s.itemno,
	   sum(s.quantity) quantity, sum(s.amount) amount
from dw_hourlyproductsales s WITH(NOLOCK)
group by cast(s.orderdate as date), s.outletname, s.itemno
'