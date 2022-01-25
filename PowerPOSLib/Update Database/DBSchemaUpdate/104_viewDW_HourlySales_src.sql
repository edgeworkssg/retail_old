IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlySales_src]'))
DROP VIEW [dbo].[viewDW_HourlySales_src]

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlySales_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[viewDW_HourlySales_src] AS 
SELECT	 OH.OrderDate
		,OH.OutletName
		,OH.Pax
		,OH.Bill
		,OH.GrossAmount
		,OH.Disc
		,OH.AfterDisc
		,OH.SvcCharge
		,OH.BefGST
		,OH.GST
		,OD.Rounding
		,OH.NettAmount
		,OD.PointSale
		,OD.InstallmentPaymentSale
		,OH.Regenerate
FROM	( 
SELECT   DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME)) orderdate
		,POS.OutletName
		,COUNT(DISTINCT OH.OrderHdrID) Pax
	    ,COUNT(DISTINCT OH.OrderHdrID) Bill
		,SUM(OD.UnitPrice*OD.Quantity-OD.GSTAmount) GrossAmount
		,SUM(OD.UnitPrice*OD.quantity-OD.Amount) Disc
		,SUM(OD.Amount-OD.GSTAmount) AfterDisc
		,0 SvcCharge
		,SUM(OD.Amount-OD.GSTAmount) BefGST
		,SUM(OD.GSTAmount) GST
		,SUM(OD.Amount) NettAmount
		,0 Regenerate
FROM	OrderHdr OH WITH(NOLOCK)
		LEFT JOIN OrderDet OD WITH(NOLOCK) ON OD.OrderHdrID = OH.OrderHdrID
		LEFT JOIN PointOfSale POS WITH(NOLOCK) ON POS.PointOfSaleID = OH.PointOfSaleID
WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
		AND OD.ItemNo NOT IN (''R0001'')
GROUP BY DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME))
		,POS.OutletName ) OH LEFT JOIN (
SELECT   DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME)) orderdate
		,POS.OutletName
		,SUM(CASE WHEN OD.ItemNo=''R0001'' THEN OD.Amount ELSE 0 END) Rounding
	    ,SUM(CASE WHEN OD.ItemNo=''INST_PAYMENT'' THEN OD.Amount ELSE 0 END) InstallmentPaymentSale
		,SUM(CASE WHEN I.Userfld10=''D'' THEN OD.Amount ELSE 0 END) PointSale
FROM	OrderHdr OH WITH(NOLOCK)
		LEFT JOIN OrderDet OD WITH(NOLOCK) ON OD.OrderHdrID = OH.OrderHdrID
		LEFT JOIN Item I WITH(NOLOCK) ON I.ItemNo = OD.ItemNo
		LEFT JOIN PointOfSale POS WITH(NOLOCK) ON POS.PointOfSaleID = OH.PointOfSaleID
WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
GROUP BY DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME))
		,POS.OutletName
		) OD ON OD.OrderDate = OH.OrderDate AND OD.OutletName = OH.OutletName'