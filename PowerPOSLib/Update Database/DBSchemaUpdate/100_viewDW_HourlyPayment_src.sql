IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyPayment_src]'))
DROP VIEW [dbo].[viewDW_HourlyPayment_src]

--------------- Recreate viewDW_HourlyPayment_src -----

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyPayment_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[viewDW_HourlyPayment_src] AS 
SELECT DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME)) OrderDate, 
	   POS.OutletName, 
	   SUM(CASE WHEN RD.PaymentType=''CASH'' THEN RD.Amount ELSE 0 END) PayByCash,
	   SUM(CASE WHEN AP.PaymentID=1 THEN RD.Amount ELSE 0 END) Pay01,
	   SUM(CASE WHEN AP.PaymentID=2 THEN RD.Amount ELSE 0 END) Pay02,
	   SUM(CASE WHEN AP.PaymentID=3 THEN RD.Amount ELSE 0 END) Pay03,
	   SUM(CASE WHEN AP.PaymentID=4 THEN RD.Amount ELSE 0 END) Pay04,
	   SUM(CASE WHEN AP.PaymentID=5 THEN RD.Amount ELSE 0 END) Pay05,
	   SUM(CASE WHEN AP.PaymentID=6 THEN RD.Amount ELSE 0 END) Pay06,
	   SUM(CASE WHEN AP.PaymentID=7 THEN RD.Amount ELSE 0 END) Pay07,
	   SUM(CASE WHEN AP.PaymentID=8 THEN RD.Amount ELSE 0 END) Pay08,
	   SUM(CASE WHEN AP.PaymentID=9 THEN RD.Amount ELSE 0 END) Pay09,
	   SUM(CASE WHEN AP.PaymentID=10 THEN RD.Amount ELSE 0 END) Pay10,
	   SUM(CASE WHEN AP.PaymentID=11 THEN RD.Amount ELSE 0 END) Pay11,
	   SUM(CASE WHEN AP.PaymentID=12 THEN RD.Amount ELSE 0 END) Pay12,
	   SUM(CASE WHEN AP.PaymentID=13 THEN RD.Amount ELSE 0 END) Pay13,
	   SUM(CASE WHEN AP.PaymentID=14 THEN RD.Amount ELSE 0 END) Pay14,
	   SUM(CASE WHEN AP.PaymentID=15 THEN RD.Amount ELSE 0 END) Pay15,
	   SUM(CASE WHEN AP.PaymentID=16 THEN RD.Amount ELSE 0 END) Pay16,
	   SUM(CASE WHEN AP.PaymentID=17 THEN RD.Amount ELSE 0 END) Pay17,
	   SUM(CASE WHEN AP.PaymentID=18 THEN RD.Amount ELSE 0 END) Pay18,
	   SUM(CASE WHEN AP.PaymentID=19 THEN RD.Amount ELSE 0 END) Pay19,
	   SUM(CASE WHEN AP.PaymentID=20 THEN RD.Amount ELSE 0 END) Pay20,
	   SUM(CASE WHEN AP.PaymentID=21 THEN RD.Amount ELSE 0 END) Pay21,
	   SUM(CASE WHEN AP.PaymentID=22 THEN RD.Amount ELSE 0 END) Pay22,
	   SUM(CASE WHEN AP.PaymentID=23 THEN RD.Amount ELSE 0 END) Pay23,
	   SUM(CASE WHEN AP.PaymentID=24 THEN RD.Amount ELSE 0 END) Pay24,
	   SUM(CASE WHEN AP.PaymentID=25 THEN RD.Amount ELSE 0 END) Pay25,
	   SUM(CASE WHEN AP.PaymentID=26 THEN RD.Amount ELSE 0 END) Pay26,
	   SUM(CASE WHEN AP.PaymentID=27 THEN RD.Amount ELSE 0 END) Pay27,
	   SUM(CASE WHEN AP.PaymentID=28 THEN RD.Amount ELSE 0 END) Pay28,
	   SUM(CASE WHEN AP.PaymentID=29 THEN RD.Amount ELSE 0 END) Pay29,
	   SUM(CASE WHEN AP.PaymentID=30 THEN RD.Amount ELSE 0 END) Pay30,
	   SUM(CASE WHEN AP.PaymentID=31 THEN RD.Amount ELSE 0 END) Pay31,
	   SUM(CASE WHEN AP.PaymentID=32 THEN RD.Amount ELSE 0 END) Pay32,
	   SUM(CASE WHEN AP.PaymentID=33 THEN RD.Amount ELSE 0 END) Pay33,
	   SUM(CASE WHEN AP.PaymentID=34 THEN RD.Amount ELSE 0 END) Pay34,
	   SUM(CASE WHEN AP.PaymentID=35 THEN RD.Amount ELSE 0 END) Pay35,
	   SUM(CASE WHEN AP.PaymentID=36 THEN RD.Amount ELSE 0 END) Pay36,
	   SUM(CASE WHEN AP.PaymentID=37 THEN RD.Amount ELSE 0 END) Pay37,
	   SUM(CASE WHEN AP.PaymentID=38 THEN RD.Amount ELSE 0 END) Pay38,
	   SUM(CASE WHEN AP.PaymentID=39 THEN RD.Amount ELSE 0 END) Pay39,
	   SUM(CASE WHEN AP.PaymentID=40 THEN RD.Amount ELSE 0 END) Pay40,
	   SUM(CASE WHEN ISNULL(AP.PaymentID,41)>40 and RD.PaymentType NOT IN (''CASH'',''INSTALLMENT'',''POINTS'',''PACKAGE'') THEN RD.Amount ELSE 0 END) PayOthers,
	   0 PointAllocated,   
	   SUM(CASE WHEN RD.PaymentType=''INSTALLMENT'' THEN RD.Amount ELSE 0 END) PayByInstallment,
	   SUM(CASE WHEN RD.PaymentType=''POINTS'' THEN RD.Amount ELSE 0 END) PayByPoint,
	   SUM(RD.Amount) TotalPayment
FROM	OrderHdr OH WITH(NOLOCK)
		LEFT JOIN ReceiptHdr RH WITH(NOLOCK) on OH.OrderHdrID = RH.OrderHdrID
		LEFT JOIN ReceiptDet RD WITH(NOLOCK) on RH.ReceiptHdrID = RD.ReceiptHdrID
		LEFT JOIN ActivePayment AP WITH(NOLOCK) on AP.PaymentName = RD.PaymentType
		LEFT JOIN PointOfSale POS WITH(NOLOCK) on POS.PointOfSaleID = OH.PointOfSaleID
WHERE	OH.IsVoided = 0
GROUP BY	DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME)), 
			POS.OutletName'