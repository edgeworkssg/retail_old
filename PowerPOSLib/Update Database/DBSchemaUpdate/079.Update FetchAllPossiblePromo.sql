declare @statement Nvarchar(max)
set @statement = ''

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAllPossibleItemGroupPromo]') AND type in (N'P', N'PC'))
BEGIN
	set @statement = @statement + N'CREATE PROCEDURE [dbo].[FetchAllPossibleItemGroupPromo] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER PROCEDURE [dbo].[FetchAllPossibleItemGroupPromo] '	
END
set @statement = @statement + N' 
	@ItemNoList Varchar(MAX),
	@HasMember bit,
	@PointOfSaleID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
declare @SQL varchar(MAX);
    -- Insert statements for procedure here
Set @SQL = ''
select d.itemgroupid,c.itemno,c.unitqty,d.promocampaignhdrid 
from viewpromomasterdetail d
Inner join itemgroupmap  as c on c.itemgroupid=d.itemgroupid
where 
d.itemgroupid in (
Select A.itemgroupid from 
(select itemgroupid,count(distinct(itemno)) as ItemCountInOrderList
from ItemGroupMap where 
ItemNo in ('' + @ItemNoList + '')
group by itemgroupid) as A
Inner join ( 
select itemgroupid,count(distinct(itemno)) as TotalItemCount
from ItemGroupMap 
group by itemgroupid) as B
on A.itemgroupid=B.itemgroupid
where itemcountinOrderList = TotalItemCount) 
AND 
	DateFrom <= getdate() AND dateTo >=getdate()
AND 
	Enabled = 1
AND
		('' + cast(@HasMember as varchar(1)) + '' | ForNonMembersAlso) = 1
AND 
		((PointOfSaleID  = '' + cast(@PointOfSaleID as varchar(4)) + '') OR (PointOfSaleID = -1))
AND CampaignType <> ''''AnyXOffAllItems''''
ORDER BY PROMOPRICE DESC
'';

EXEC (@SQL);

END
'

EXEC dbo.sp_executesql @statement