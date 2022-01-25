IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductSales_src]'))
DROP VIEW [dbo].[viewDW_HourlyProductSales_src]

--------------- Recreate viewDW_HourlyProductSales_src -----

IF NOT  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[viewDW_HourlyProductSales_src]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[viewDW_HourlyProductSales_src] AS 
SELECT   DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME)) orderdate
		,POS.OutletName
		,OD.ItemNo
		,SUM(OD.Quantity) Quantity
		,SUM(OD.Amount) Amount
		,0 Regenerate
FROM	OrderHdr OH WITH(NOLOCK)
		LEFT JOIN OrderDet OD WITH(NOLOCK) ON OD.OrderHdrID = OH.OrderHdrID
		LEFT JOIN Item I WITH(NOLOCK) ON I.ItemNo = OD.ItemNo
		LEFT JOIN PointOfSale POS WITH(NOLOCK) ON POS.PointOfSaleID = OH.PointOfSaleID
WHERE	OH.IsVoided = 0 AND OD.IsVoided = 0
		AND OD.ItemNo NOT IN (''R0001'')
GROUP BY DATEADD(HOUR, DATEPART(HOUR,OH.OrderDate), CAST(CAST(OH.OrderDate AS DATE) AS DATETIME))
		,POS.OutletName
		,OD.ItemNo'