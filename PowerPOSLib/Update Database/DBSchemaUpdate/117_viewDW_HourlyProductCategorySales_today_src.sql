IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductCategorySales_today_src]'))
DROP VIEW [dbo].[viewDW_HourlyProductCategorySales_today_src]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductCategorySales_today_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlyProductCategorySales_today_src] as 
select orderdate, outletname, i.categoryname, 
	   sum(quantity) quantity, sum(amount) amount
from viewDW_HourlyProductSales_today_src s WITH(NOLOCK)
left join item i WITH(NOLOCK) on i.itemno=s.itemno
group by orderdate, outletname, i.categoryname
'