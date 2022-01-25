IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewOrderHdrNoByPointOfSaleID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNewOrderHdrNoByPointOfSaleID]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewOrderHdrNoByPointOfSaleID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetNewOrderHdrNoByPointOfSaleID]
	-- Add the parameters for the stored procedure here	
	@PointOfSaleId int	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;		
	
    SELECT isnull(max(right(orderhdrid,4)),''0'') from orderhdr
    where left(orderhdrid,6) = substring(convert(varchar,getdate(),112),3,6)
    AND convert(int,substring(orderhdrid,7,4)) = @PointOfSaleId
    AND left(right(orderhdrid,4), 1) <> ''W''  -- Exclude OrderHdrID created by Web	
	
END
' 
END
