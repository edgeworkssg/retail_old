IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductSales_today_src]'))
DROP VIEW [dbo].[viewDW_HourlyProductSales_today_src]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductSales_today_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlyProductSales_today_src] as 
select dateadd(hour, datepart(hour,h.orderdate), cast(cast(h.orderdate as date) as datetime)) orderdate,
	   p.outletname,
	   d.itemno,
	   sum(d.quantity) quantity,
	   sum(d.amount) amount,
	   0 regenerate
from orderhdr h WITH(NOLOCK)
left join orderdet d WITH(NOLOCK) on h.orderhdrid=d.orderhdrid
left join pointofsale p WITH(NOLOCK) on p.pointofsaleid=h.pointofsaleid
where h.isvoided=0
and d.itemno not in (''R0001'')
and h.orderdate >= cast(getdate() as date)
group by cast(h.orderdate as date), datepart(hour,h.orderdate), p.outletname, d.itemno
'
