IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailySales_src]'))
DROP VIEW [dbo].[viewDW_DailySales_src]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailySales_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_DailySales_src] as
select x.orderdate, x.outletname, x.bill pax, x.bill, x.grossamount, x.disc, x.afterdisc, 
	   x.svccharge, x.befgst, x.gst, y.rounding, x.nettamount, z.payByCash,
	   z.pay01, z.pay02, z.pay03, z.pay04, z.pay05, z.pay06, z.pay07, z.pay08, z.pay09, z.pay10,
	   z.pay11, z.pay12, z.pay13, z.pay14, z.pay15, z.pay16, z.pay17, z.pay18, z.pay19, z.pay20,
	   z.pay21, z.pay22, z.pay23, z.pay24, z.pay25, z.pay26, z.pay27, z.pay28, z.pay29, z.pay30,
	   z.pay31, z.pay32, z.pay33, z.pay34, z.pay35, z.pay36, z.pay37, z.pay38, z.pay39, z.pay40,
	   z.payOthers, z.totalpayment,
	   isnull(y.pointSale,0) pointSale, isnull(y.installmentPaymentSale,0) installmentPaymentSale, 0 pointAllocated, 
	   isnull(z.payByInstallment,0) payByInstallment, isnull(z.payByPoint,0) payByPoint,
	   0 regenerate
from 
(
select cast(h.orderdate as date) orderdate,
	   p.outletname,
	   count(distinct h.orderhdrid) bill,
	   sum(d.grosssales-d.gstamount) grossamount,
	   sum(d.grosssales-d.amount) disc,
	   sum(d.amount-d.gstamount) afterdisc,
	   0 svccharge,
	   sum(d.amount-d.gstamount) befgst,
	   sum(d.gstamount) gst,
	   sum(d.amount) nettamount
from orderhdr h WITH(NOLOCK)
left join orderdet d WITH(NOLOCK) on h.orderhdrid=d.orderhdrid
left join pointofsale p WITH(NOLOCK) on p.pointofsaleid=h.pointofsaleid
where h.isvoided=0
and d.itemno not in (''R0001'',''INST_PAYMENT'')
and d.itemno not in (select itemno from item where userfld10=''D'')
group by cast(h.orderdate as date), p.outletname
) x 
left join 
(
select cast(h.orderdate as date) orderdate,
	   p.outletname,
	   sum(case when d.itemno=''R0001'' then d.amount else 0 end) rounding,
	   sum(case when d.itemno=''INST_PAYMENT'' then d.amount else 0 end) installmentPaymentSale,
	   sum(case when i.userfld10=''D'' then d.amount else 0 end) pointSale
from orderhdr h WITH(NOLOCK)
left join orderdet d WITH(NOLOCK) on h.orderhdrid=d.orderhdrid
left join item i WITH(NOLOCK) on i.itemno=d.itemno
left join pointofsale p WITH(NOLOCK) on p.pointofsaleid=h.pointofsaleid
where h.isvoided=0
and (d.itemno in (''R0001'',''INST_PAYMENT'') or i.userfld10=''D'')
group by cast(h.orderdate as date), p.outletname
) y on x.orderdate=y.orderdate and x.outletname=y.outletname
left join 
(
select cast(h.orderdate as date) orderdate, s.outletname, 
	   sum(case when rd.paymenttype=''CASH'' then rd.amount else 0 end) payByCash,
	   sum(case when p.paymentid=1 then rd.amount else 0 end) pay01,
	   sum(case when p.paymentid=2 then rd.amount else 0 end) pay02,
	   sum(case when p.paymentid=3 then rd.amount else 0 end) pay03,
	   sum(case when p.paymentid=4 then rd.amount else 0 end) pay04,
	   sum(case when p.paymentid=5 then rd.amount else 0 end) pay05,
	   sum(case when p.paymentid=6 then rd.amount else 0 end) pay06,
	   sum(case when p.paymentid=7 then rd.amount else 0 end) pay07,
	   sum(case when p.paymentid=8 then rd.amount else 0 end) pay08,
	   sum(case when p.paymentid=9 then rd.amount else 0 end) pay09,
	   sum(case when p.paymentid=10 then rd.amount else 0 end) pay10,
	   sum(case when p.paymentid=11 then rd.amount else 0 end) pay11,
	   sum(case when p.paymentid=12 then rd.amount else 0 end) pay12,
	   sum(case when p.paymentid=13 then rd.amount else 0 end) pay13,
	   sum(case when p.paymentid=14 then rd.amount else 0 end) pay14,
	   sum(case when p.paymentid=15 then rd.amount else 0 end) pay15,
	   sum(case when p.paymentid=16 then rd.amount else 0 end) pay16,
	   sum(case when p.paymentid=17 then rd.amount else 0 end) pay17,
	   sum(case when p.paymentid=18 then rd.amount else 0 end) pay18,
	   sum(case when p.paymentid=19 then rd.amount else 0 end) pay19,
	   sum(case when p.paymentid=20 then rd.amount else 0 end) pay20,
	   sum(case when p.paymentid=21 then rd.amount else 0 end) pay21,
	   sum(case when p.paymentid=22 then rd.amount else 0 end) pay22,
	   sum(case when p.paymentid=23 then rd.amount else 0 end) pay23,
	   sum(case when p.paymentid=24 then rd.amount else 0 end) pay24,
	   sum(case when p.paymentid=25 then rd.amount else 0 end) pay25,
	   sum(case when p.paymentid=26 then rd.amount else 0 end) pay26,
	   sum(case when p.paymentid=27 then rd.amount else 0 end) pay27,
	   sum(case when p.paymentid=28 then rd.amount else 0 end) pay28,
	   sum(case when p.paymentid=29 then rd.amount else 0 end) pay29,
	   sum(case when p.paymentid=30 then rd.amount else 0 end) pay30,
	   sum(case when p.paymentid=31 then rd.amount else 0 end) pay31,
	   sum(case when p.paymentid=32 then rd.amount else 0 end) pay32,
	   sum(case when p.paymentid=33 then rd.amount else 0 end) pay33,
	   sum(case when p.paymentid=34 then rd.amount else 0 end) pay34,
	   sum(case when p.paymentid=35 then rd.amount else 0 end) pay35,
	   sum(case when p.paymentid=36 then rd.amount else 0 end) pay36,
	   sum(case when p.paymentid=37 then rd.amount else 0 end) pay37,
	   sum(case when p.paymentid=38 then rd.amount else 0 end) pay38,
	   sum(case when p.paymentid=39 then rd.amount else 0 end) pay39,
	   sum(case when p.paymentid=40 then rd.amount else 0 end) pay40,
	   sum(case when isnull(p.paymentid,41)>40 and rd.paymenttype not in (''CASH'',''INSTALLMENT'',''POINTS'',''PACKAGE'') then rd.amount else 0 end) payOthers,
	   sum(case when rd.paymenttype=''INSTALLMENT'' then rd.amount else 0 end) payByInstallment,
	   sum(case when rd.paymenttype=''POINTS'' then rd.amount else 0 end) payByPoint,
	   sum(rd.amount) totalpayment
from orderhdr h WITH(NOLOCK)
left join receipthdr rh WITH(NOLOCK) on h.orderhdrid=rh.orderhdrid
left join receiptdet rd WITH(NOLOCK) on rh.receipthdrid=rd.receipthdrid
left join activepayment p WITH(NOLOCK) on p.paymentname=rd.paymenttype
left join pointofsale s WITH(NOLOCK) on s.pointofsaleid=h.pointofsaleid
where h.isvoided=0
group by cast(h.orderdate as date), s.outletname
) z on x.orderdate=z.orderdate and x.outletname=z.outletname
'
