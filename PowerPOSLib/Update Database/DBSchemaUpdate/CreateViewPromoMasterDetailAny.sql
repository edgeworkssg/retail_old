IF NOT EXISTS(SELECT * FROM sys.columns WHERE [name] = N'promoprice' AND [object_id] = OBJECT_ID(N'promocampaigndet'))
BEGIN
    alter table promocampaigndet add promoprice money
END

declare @statement Nvarchar(max)
set @statement = ''
IF  NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewPromoMasterDetailAny]'))
BEGIN
	set @statement = @statement + N'CREATE VIEW [dbo].[ViewPromoMasterDetailAny] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER VIEW [dbo].[ViewPromoMasterDetailAny] '	
END
 set @statement = @statement + N'
AS
SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,d.ItemNo, i.ItemName ,i.CategoryName ,ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
, ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
FROM PromoCampaignDet d 
INNER JOIN Item i on d.ItemNo = i.ItemNo
INNER JOIN PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
WHERE ISNULL(d.ItemGroupID,'''') = '''' AND ISNULL(d.ItemNo,'''') <> '''' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = ''AnyXOffAllItems''
UNION
SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID ,i.ItemNo, i.ItemName,i.CategoryName, ISNULL(d.UnitQty,0) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
, ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
FROM PromoCampaignDet d 
inner join Item i on d.CategoryName = i.CategoryName
inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
WHERE ISNULL(d.ItemNo,'''') = '''' and ISNULL(d.ItemGroupID,'''') = '''' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = ''AnyXOffAllItems''
UNION
SELECT h.PromoCampaignHdrID, d.PromoCampaignDetID,i.ItemNo, i.ItemName,i.CategoryName, (ISNULL(m.UnitQty,0)*ISNULL(d.UnitQty,0)) as UnitQty, ISNULL(d.AnyQty,0) as AnyQty, d.MinQuantity
, ISNULL(d.PromoPrice,0) as PromoPrice, ISNULL(d.DiscAmount,0) as DiscAmount, ISNULL(d.DiscPercent,DiscPercent) as DiscPercent
FROM PromoCampaignDet d 
inner join ItemGroupMap m on d.ItemGroupId = m.ItemGroupID 
inner join Item i on m.ItemNo = i.ItemNo
inner join PromoCampaignHdr h on d.PromoCampaignHdrID = h.PromoCampaignHdrID
WHERE ISNULL(d.ItemNo,'''') = '''' and ISNULL(d.CategoryName,'''') = '''' AND ISNULL(d.Deleted,0) = 0 AND ISNULL(i.Deleted, 0) = 0
	AND ISNULL(h.Deleted,0) = 0 and h.CampaignType = ''AnyXOffAllItems''
'

EXEC dbo.sp_executesql @statement