IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemQtyOnHand]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetItemQtyOnHand]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemQtyOnHand]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetItemQtyOnHand] 
(
	@ItemNo varchar(50),
    @InventoryLocationID int,
    @Date datetime
)
RETURNS int
AS
BEGIN

DECLARE @qty int

SELECT @qty = SUM(CASE WHEN IH.MovementType LIKE ''% In'' THEN ID.Quantity ELSE -ID.Quantity END)
FROM InventoryHdr IH 
    INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
WHERE IH.InventoryDate <= @Date
    AND ID.ItemNo = @ItemNo
    AND IH.InventoryLocationID = (CASE WHEN @InventoryLocationID = 0 THEN IH.InventoryLocationID ELSE @InventoryLocationID END) 

-- Return the result of the function
RETURN ISNULL(@qty, 0)

END
' 
END


