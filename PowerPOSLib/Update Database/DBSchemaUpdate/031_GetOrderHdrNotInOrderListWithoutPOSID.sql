IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrderHdrNotInOrderListWithoutPOSID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetOrderHdrNotInOrderListWithoutPOSID]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetOrderHdrNotInOrderListWithoutPOSID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetOrderHdrNotInOrderListWithoutPOSID]
	-- Add the parameters for the stored procedure here
	@StartDate datetime,
	@EndDate datetime,
	@OrderList varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		DECLARE @SQL varchar(MAX)

		SET @SQL = 
		''SELECT *
		FROM dbo.OrderHdr
		WHERE OrderHdrID NOT IN ('' + @OrderList + '') 
		and orderdate > '''''' + convert(varchar,@StartDate,20) + ''''''
		and orderdate < '''''' + convert(varchar,@EndDate,20) + ''''''''

		EXEC(@SQL)
END
' 
END
