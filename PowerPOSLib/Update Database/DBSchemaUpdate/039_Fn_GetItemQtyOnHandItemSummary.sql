IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemQtyOnHandItemSummary]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetItemQtyOnHandItemSummary]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemQtyOnHandItemSummary]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[GetItemQtyOnHandItemSummary] 
(
	@ItemNo varchar(50),
    @InventoryLocationID int
)
RETURNS int
AS
BEGIN

DECLARE @qty int

SELECT @qty = ISNULL(SUM(BalanceQty),0) 
FROM ItemSummary 
WHERE ItemNo = @ItemNo 
    AND InventoryLocationID = (CASE WHEN @InventoryLocationID = 0 THEN InventoryLocationID ELSE @InventoryLocationID END)

-- Return the result of the function
RETURN ISNULL(@qty, 0)

END
' 
END
