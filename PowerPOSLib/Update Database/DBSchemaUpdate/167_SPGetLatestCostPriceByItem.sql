declare @statement Nvarchar(max)
set @statement = ''

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLatestCostPriceByItemNo]') AND type in (N'P', N'PC'))
BEGIN
	set @statement = @statement + N'CREATE PROCEDURE [dbo].[GetLatestCostPriceByItemNo] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER PROCEDURE [dbo].[GetLatestCostPriceByItemNo] '	
END
set @statement = @statement + N' 
	@StartDate datetime,
	@EndDate datetime,
	@ItemNo varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	if object_id(''tempdb..#ResultsCostPrice'') is not null drop table #ResultsCostPrice;
	
	create table #ResultsCostPrice
	(
		CostPriceByItem decimal(18,2)
	);

	Insert 
	into #ResultsCostPrice
	select  isnull(A.CostPriceByItem,0) 
	FROM 
			(
				SELECT  ID.ItemNo, ID.CostPriceByItem,
				CASE WHEN ISNULL(ID.CostPriceByItemInvLoc,IT.FactoryPrice) =0 THEN IT.FactoryPrice ELSE ISNULL(ID.CostPriceByItemInvLoc,IT.FactoryPrice) END CostPriceByItemInvLoc,
				row_number() over(partition by ID.ItemNo order by IH.InventoryDate desc) rn
				FROM InventoryHdr IH 
				INNER JOIN InventoryDet ID  WITH(NOLOCK) ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
				INNER JOIN Item IT  WITH(NOLOCK) ON IT.ItemNo = ID.ItemNo
				WHERE ID.ItemNo = @ItemNo  and  inventorydate >= @StartDate and inventorydate <= @EndDate
			) A where rn = 1

			IF @@ROWCOUNT= 0 
			select 0.00;
			ELSE 
			select CostPriceByItem from #ResultsCostPrice;

END
'

EXEC dbo.sp_executesql @statement