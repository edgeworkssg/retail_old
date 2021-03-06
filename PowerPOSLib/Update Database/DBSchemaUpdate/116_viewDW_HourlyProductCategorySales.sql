IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductCategorySales]'))
DROP VIEW [dbo].[viewDW_HourlyProductCategorySales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductCategorySales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlyProductCategorySales] as 
select s.orderdate, s.outletname, i.categoryname,
	   sum(s.quantity) quantity, sum(s.amount) amount
from dw_hourlyproductsales s WITH(NOLOCK)
left join item i WITH(NOLOCK) on i.itemno=s.itemno
group by s.orderdate, s.outletname, i.categoryname 
'