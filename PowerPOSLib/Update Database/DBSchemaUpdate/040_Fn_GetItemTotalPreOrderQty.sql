IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemTotalPreOrderQty]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetItemTotalPreOrderQty]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemTotalPreOrderQty]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetItemTotalPreOrderQty] 
(
	@ItemNo varchar(50),
    @InventoryLocationID int
)
RETURNS int
AS
BEGIN

DECLARE @qty int

--SELECT @qty = SUM(ISNULL(od.Quantity, 0))
--FROM OrderDet od
--    INNER JOIN OrderHdr oh ON oh.OrderHdrID = od.OrderHdrID
--WHERE od.IsVoided = 0 AND oh.IsVoided = 0 
--    AND od.IsPreOrder = 1
--    AND ISNULL(od.InventoryHdrRefNo, '''') = ''''
--    AND od.ItemNo = @ItemNo

SELECT @qty = SUM(OdQty) - SUM(DoQty)
FROM (
    SELECT od.OrderDetID, ISNULL(od.Quantity, 0) OdQty, ISNULL(dod.Quantity, 0) DoQty
    FROM OrderDet od
        INNER JOIN OrderHdr oh ON oh.OrderHdrID = od.OrderHdrID
        LEFT JOIN (
                SELECT OrderDetID, SUM(Quantity) Quantity
                FROM DeliveryOrderDetails
                WHERE ISNULL(Deleted, 0) = 0
                GROUP BY OrderDetID
            ) dod ON dod.OrderDetID = od.OrderDetID
        INNER JOIN PointOfSale pos ON pos.PointOfSaleID = oh.PointOfSaleID
        INNER JOIN Outlet ol ON ol.OutletName = pos.OutletName
    WHERE od.IsVoided = 0 AND oh.IsVoided = 0 
        AND od.IsPreOrder = 1
        AND od.ItemNo = @ItemNo
        AND ol.InventoryLocationID = (CASE WHEN @InventoryLocationID = 0 THEN ol.InventoryLocationID ELSE @InventoryLocationID END)
) a

-- Return the result of the function
RETURN ISNULL(@qty, 0)

END' 
END

