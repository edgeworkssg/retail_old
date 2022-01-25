IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxItemRefNo]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetMaxItemRefNo]
BEGIN

EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetMaxItemRefNo]
	-- Add the parameters for the stored procedure here
	@PointOfSaleID int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT isnull(max(right(itemno,4)),''0'') from Item
	--where itemno like ''I0'' + cast(@PointOfSaleID as varchar) + ''%''
	
	--check with item matrix too
	select max(right(itemno,4))
	from
	(
		select isnull(itemno,''0'') as itemno from item where itemno like ''I0'' + cast(@PointOfSaleID as varchar) + ''%'' and ISNULL(userflag1,''false'') = ''false''
		union
		select isnull(Attributes1,''0'') as itemno from item where itemno like ''I0'' + cast(@PointOfSaleID as varchar) + ''%'' and ISNULL(userflag1,''false'') = ''true''
	) as t
END
'
END


