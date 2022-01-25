declare @statement Nvarchar(max)
set @statement = ''

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAllPossiblePromoAnyXofAllItemsMemberGroup]') AND type in (N'P', N'PC'))
BEGIN
	set @statement = @statement + N'CREATE PROCEDURE [dbo].[FetchAllPossiblePromoAnyXofAllItemsMemberGroup] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER PROCEDURE [dbo].[FetchAllPossiblePromoAnyXofAllItemsMemberGroup] '	
END
set @statement = @statement + N' 
	@ItemNoList Varchar(MAX),
	@HasMember bit,
	@PointOfSaleID int,
	@MemberGroupID int,
	@CategoryName NVarchar(MAX),
	@ItemGroupIDList VARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	declare @SQL Nvarchar(MAX);

	Set @SQL = 
	N''SELECT distinct PromoCampaignHdrID, Priority
	FROM
	(
	SELECT     h.PromoCampaignHdrID, h.ForNonMembersAlso, h.DateFrom, h.DateTo, ISNULL(h.Priority,99) AS Priority,p.PointOfSaleID
	FROM        dbo.PromoCampaignHdr h  INNER JOIN
				dbo.PromoOutlet o ON o.PromoCampaignHdrID =  h.PromoCampaignHdrID INNER JOIN
				dbo.PointOfSale p ON RTRIM(LTRIM(p.OutletName)) = RTRIM(LTRIM(o.OutletName)) INNER JOIN
				dbo.PromoDaysMap m on m.PromoCampaignHdrID = h.PromoCampaignHdrID 
				LEFT JOIN promocampaigndet DetAny on h.PromoCampaignHdrID = DetAny.PromoCampaignHdrID and ISNULL(DetAny.AnyQty,0) > 0 and detAny.Deleted =0 
				LEFT JOIN promocampaigndet DetUnit on h.PromoCampaignHdrID = DetUnit.PromoCampaignHdrID and ISNULL(DetUnit.UnitQty,0) > 0 and detUnit.Deleted = 0
	WHERE     ISNULL(h.Deleted,0) = 0 AND h.CampaignType = ''''AnyXOffAllItems''''
			  AND ISNULL(p.Deleted,0) = 0 AND ISNULL(o.Deleted,0) = 0 and m.DaysPromo = DATENAME(dw, GETDATE()) and ISNULL(m.Deleted,0) = 0
			  AND ((ISNULL(h.IsRestricHour,0) = 1 AND (CONVERT(char(12), GETDATE(), 114) >= CONVERT(char(12), h.RestrictHourStart, 114)) AND (CONVERT(char(12), GETDATE(), 114) <= CONVERT(char(12), h.RestrictHourEnd, 114))) OR (ISNULL(h.IsRestricHour,0) = 0))
			  and (ISNULL(detUnit.ItemNo,'''''''') in ('' + @ItemNoList + '') or ISNULL(detAny.ItemGroupID,0) in ('' + @ItemGroupIDList + '') or ISNULL(detAny.CategoryName,'''''''') = N'''''' + @CategoryName +'''''')
			  AND ( CASE WHEN ISNULL(h.ForNonMembersAlso,1) = 1 THEN 1 ELSE 0 END = 1 OR ( ISNULL(h.ForNonMembersAlso,1) = 0 AND '' + cast(@HasMember as varchar(1)) + '' = 1 AND (( ISNULL(h.userfld1,''''0'''') = ''''0'''' OR ('''','''' + RTRIM(ISNULL(h.userfld1,''''0'''')) + '''','''') LIKE ''''%,'''' + ''''''+ CAST(@MemberGroupID AS VARCHAR(MAX)) + ''''''+ '''',%''''))))
	) t
	WHERE	('' + cast(@HasMember as varchar(1)) + '' | t.ForNonMembersAlso) = 1 AND (t.PointOfSaleID  = '' + cast(@PointOfSaleID as varchar(4)) + '') AND t.DateFrom <= GETDATE() and t.DateTo >= GETDATE()
	ORDER BY Priority''

	EXEC (@SQL);

END
'

EXEC dbo.sp_executesql @statement