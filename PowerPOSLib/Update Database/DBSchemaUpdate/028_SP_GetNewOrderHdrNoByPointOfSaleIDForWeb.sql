IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewOrderHdrNoByPointOfSaleIDForWeb]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNewOrderHdrNoByPointOfSaleIDForWeb]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewOrderHdrNoByPointOfSaleIDForWeb]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetNewOrderHdrNoByPointOfSaleIDForWeb]
	-- Add the parameters for the stored procedure here
	@PointOfSaleId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
                        	
	SELECT ISNULL(MAX(RIGHT(OrderHdrID,3)),''0'') FROM OrderHdr
	WHERE LEFT(OrderHdrID,6) = SUBSTRING(CONVERT(varchar,GETDATE(),112),3,6)
	AND CONVERT(int,SUBSTRING(OrderHdrID,7,4)) = @PointOfSaleId
    AND LEFT(RIGHT(OrderHdrID,4), 1) = ''W''
END

                        
' 
END
