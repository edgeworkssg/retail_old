IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductDepartmentSales_today_src]'))
DROP VIEW [dbo].[viewDW_HourlyProductDepartmentSales_today_src]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductDepartmentSales_today_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlyProductDepartmentSales_today_src] as 
select orderdate, outletname, t.departmentname, 
	   sum(quantity) quantity, sum(amount) amount
from viewDW_HourlyProductCategorySales_today_src s WITH(NOLOCK)
left join category c WITH(NOLOCK) on c.categoryname=s.categoryname
left join itemdepartment t WITH(NOLOCK) on t.itemdepartmentid=c.itemdepartmentid
group by orderdate, outletname, t.departmentname
'