IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxResourceID]') AND type in (N'P', N'PC'))
	Drop Procedure [dbo].[GetMaxResourceID]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxResourceID]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE GetMaxResourceID 
	@PointOfSaleID int 
AS
BEGIN
    -- Insert statements for procedure here
	SELECT isnull(max(right(ResourceID,4)),''0'') from [Resource]
	where ResourceID like ''RS0'' + cast(@PointOfSaleID as varchar) + ''%''	
END'
END
