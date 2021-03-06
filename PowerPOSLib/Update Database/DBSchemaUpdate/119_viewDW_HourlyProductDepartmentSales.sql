IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductDepartmentSales]'))
DROP VIEW [dbo].[viewDW_HourlyProductDepartmentSales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductDepartmentSales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlyProductDepartmentSales] as 
select s.orderdate, s.outletname, t.departmentname,
	   sum(s.quantity) quantity, sum(s.amount) amount
from dw_hourlyproductsales s WITH(NOLOCK)
left join item i WITH(NOLOCK) on i.itemno=s.itemno
left join category c WITH(NOLOCK) on c.categoryname=i.categoryname
left join itemdepartment t WITH(NOLOCK) on t.itemdepartmentid=c.itemdepartmentid
group by s.orderdate, s.outletname, t.departmentname
'