IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailySales]'))
DROP VIEW [dbo].[viewDW_DailySales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_DailySales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_DailySales] as 
select cast(s.orderdate as date) orderdate, s.outletname, 
	   sum(s.pax) pax, sum(s.bill) bill, sum(s.grossamount) grossamount, sum(s.disc) disc, sum(s.afterdisc) afterdisc, 
	   sum(s.svccharge) svccharge, sum(s.befgst) befgst, sum(s.gst) gst, sum(s.rounding) rounding, 
	   sum(s.nettamount) nettamount, sum(s.pointSale) pointSale, sum(s.installmentPaymentSale) installmentPaymentSale, 
	   sum(p.payByCash) payByCash,
	   sum(p.pay01) pay01, sum(p.pay02) pay02, sum(p.pay03) pay03, sum(p.pay04) pay04, sum(p.pay05) pay05, 
	   sum(p.pay06) pay06, sum(p.pay07) pay07, sum(p.pay08) pay08, sum(p.pay09) pay09, sum(p.pay10) pay10,
	   sum(p.pay11) pay11, sum(p.pay12) pay12, sum(p.pay13) pay13, sum(p.pay14) pay14, sum(p.pay15) pay15, 
	   sum(p.pay16) pay16, sum(p.pay17) pay17, sum(p.pay18) pay18, sum(p.pay19) pay19, sum(p.pay20) pay20,
	   sum(p.pay21) pay21, sum(p.pay22) pay22, sum(p.pay23) pay23, sum(p.pay24) pay24, sum(p.pay25) pay25, 
	   sum(p.pay26) pay26, sum(p.pay27) pay27, sum(p.pay28) pay28, sum(p.pay29) pay29, sum(p.pay30) pay30,
	   sum(p.pay31) pay31, sum(p.pay32) pay32, sum(p.pay33) pay33, sum(p.pay34) pay34, sum(p.pay35) pay35, 
	   sum(p.pay36) pay36, sum(p.pay37) pay37, sum(p.pay38) pay38, sum(p.pay39) pay39, sum(p.pay40) pay40,
	   sum(p.payOthers) payOthers, sum(p.totalpayment) totalpayment,
	   sum(p.pointAllocated) pointAllocated, sum(p.payByInstallment) payByInstallment, 
	   sum(p.payByPoint) payByPoint
from dw_hourlysales s WITH(NOLOCK)
left join dw_hourlypayment p WITH(NOLOCK) on s.outletname=p.outletname and s.orderdate=p.orderdate
group by cast(s.orderdate as date), s.outletname
'