IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewDeliveryNoByPointOfSaleID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNewDeliveryNoByPointOfSaleID]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewDeliveryNoByPointOfSaleID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetNewDeliveryNoByPointOfSaleID]
	@PointOfSaleId int	
AS
BEGIN
	SET NOCOUNT ON;		
	SELECT isnull(max(right(ordernumber, CASE WHEN OrderNumber LIKE ''%W'' THEN 5 ELSE 4 END)),''0'') from deliveryorder
	where left(ordernumber,6) = substring(convert(varchar,getdate(),112),3,6)
	AND convert(int,substring(ordernumber,7,4)) = @PointOfSaleId;
END
' 
END
