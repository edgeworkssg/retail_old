IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailyProductCategorySales]'))
DROP VIEW [dbo].[viewDW_DailyProductCategorySales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailyProductCategorySales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_DailyProductCategorySales] as 
select cast(s.orderdate as date) orderdate, s.outletname, i.categoryname,
	   sum(s.quantity) quantity, sum(s.amount) amount
from dw_hourlyproductsales s WITH(NOLOCK)
left join item i WITH(NOLOCK) on i.itemno=s.itemno
group by cast(s.orderdate as date), s.outletname, i.categoryname
'