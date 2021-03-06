IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_MonthlySales]'))
DROP VIEW [dbo].[viewDW_MonthlySales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_MonthlySales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_MonthlySales] as 
select cast(convert(varchar,datepart(year,x.orderdate))+''-''+convert(varchar,datepart(month,x.orderdate))+''-01'' as date) orderdate, 
	   x.outletname, 
	   sum(x.pax) pax, sum(x.bill) bill, sum(x.grossamount) grossamount, sum(x.disc) disc, sum(x.afterdisc) afterdisc, 
	   sum(x.svccharge) svccharge, sum(x.befgst) befgst, sum(x.gst) gst, sum(x.rounding) rounding, 
	   sum(x.nettamount) nettamount, sum(x.pointSale) pointSale, sum(x.installmentPaymentSale) installmentPaymentSale, 
	   sum(x.payByCash) payByCash,
	   sum(x.pay01) pay01, sum(x.pay02) pay02, sum(x.pay03) pay03, sum(x.pay04) pay04, sum(x.pay05) pay05, 
	   sum(x.pay06) pay06, sum(x.pay07) pay07, sum(x.pay08) pay08, sum(x.pay09) pay09, sum(x.pay10) pay10,
	   sum(x.pay11) pay11, sum(x.pay12) pay12, sum(x.pay13) pay13, sum(x.pay14) pay14, sum(x.pay15) pay15, 
	   sum(x.pay16) pay16, sum(x.pay17) pay17, sum(x.pay18) pay18, sum(x.pay19) pay19, sum(x.pay20) pay20,
	   sum(x.pay21) pay21, sum(x.pay22) pay22, sum(x.pay23) pay23, sum(x.pay24) pay24, sum(x.pay25) pay25, 
	   sum(x.pay26) pay26, sum(x.pay27) pay27, sum(x.pay28) pay28, sum(x.pay29) pay29, sum(x.pay30) pay30,
	   sum(x.pay31) pay31, sum(x.pay32) pay32, sum(x.pay33) pay33, sum(x.pay34) pay34, sum(x.pay35) pay35, 
	   sum(x.pay36) pay36, sum(x.pay37) pay37, sum(x.pay38) pay38, sum(x.pay39) pay39, sum(x.pay40) pay40,
	   sum(x.payOthers) payOthers, sum(x.totalpayment) totalpayment,
	   sum(x.pointAllocated) pointAllocated, sum(x.payByInstallment) payByInstallment, 
	   sum(x.payByPoint) payByPoint
from viewDW_HourlySales x WITH(NOLOCK)
group by datepart(year,x.orderdate),datepart(month,x.orderdate),x.outletname
'