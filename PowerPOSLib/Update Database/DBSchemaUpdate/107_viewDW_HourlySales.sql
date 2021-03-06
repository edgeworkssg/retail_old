IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlySales]'))
DROP VIEW [dbo].[viewDW_HourlySales]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlySales]'))
EXEC dbo.sp_executesql @statement = N'
CREATE view [dbo].[viewDW_HourlySales] as
	   select s.orderdate, s.outletname, 
	   s.pax, s.bill, s.grossamount, s.disc, s.afterdisc, 
	   s.svccharge, s.befgst, s.gst, s.rounding, 
	   s.nettamount, s.pointSale, s.installmentPaymentSale, 
	   p.payByCash,
	   p.pay01, p.pay02, p.pay03, p.pay04, p.pay05, 
	   p.pay06, p.pay07, p.pay08, p.pay09, p.pay10,
	   p.pay11, p.pay12, p.pay13, p.pay14, p.pay15, 
	   p.pay16, p.pay17, p.pay18, p.pay19, p.pay20,
	   p.pay21, p.pay22, p.pay23, p.pay24, p.pay25, 
	   p.pay26, p.pay27, p.pay28, p.pay29, p.pay30,
	   p.pay31, p.pay32, p.pay33, p.pay34, p.pay35, 
	   p.pay36, p.pay37, p.pay38, p.pay39, p.pay40,
	   p.payOthers, p.totalpayment,
	   p.pointAllocated, p.payByInstallment, 
	   p.payByPoint
from dw_hourlysales s WITH(NOLOCK)
left join dw_hourlypayment p WITH(NOLOCK) on s.outletname=p.outletname and s.orderdate=p.orderdate
'
