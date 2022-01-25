declare @statement Nvarchar(max)
set @statement = ''

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAllPossiblePromoAnyXofAllItems]') AND type in (N'P', N'PC'))
BEGIN
	set @statement = @statement + N'CREATE PROCEDURE [dbo].[FetchAllPossiblePromoAnyXofAllItems] '
END
ELSE
BEGIN
	set @statement = @statement + N'ALTER PROCEDURE [dbo].[FetchAllPossiblePromoAnyXofAllItems] '	
END
set @statement = @statement + N' 
	@ItemNoList Varchar(MAX),
	@HasMember bit,
	@PointOfSaleID int
AS
BEGIN
	SET NOCOUNT ON;

	declare @SQL varchar(MAX);

	Set @SQL = 
	N''SELECT distinct PromoCampaignHdrID, Priority
	FROM
	(
	SELECT     h.PromoCampaignHdrID, h.ForNonMembersAlso, h.DateFrom, h.DateTo, ISNULL(h.Priority,99) AS Priority,p.PointOfSaleID
	FROM        dbo.PromoCampaignHdr h  INNER JOIN
				dbo.PromoOutlet o ON o.PromoCampaignHdrID =  h.PromoCampaignHdrID INNER JOIN
				dbo.PointOfSale p ON RTRIM(LTRIM(p.OutletName)) = RTRIM(LTRIM(o.OutletName)) INNER JOIN
				dbo.PromoDaysMap m on m.PromoCampaignHdrID = h.PromoCampaignHdrID INNER JOIN
				dbo.ViewPromoMasterDetailAny detail on detail.PromoCampaignHdrID = h.PromoCampaignHdrID
	WHERE     ISNULL(h.Deleted,0) = 0 AND h.CampaignType = ''''AnyXOffAllItems''''
			  AND ISNULL(p.Deleted,0) = 0 AND ISNULL(o.Deleted,0) = 0 and m.DaysPromo = DATENAME(dw, GETDATE()) and ISNULL(m.Deleted,0) = 0
			  AND ((ISNULL(h.IsRestricHour,0) = 1 AND (CONVERT(char(12), GETDATE(), 114) >= CONVERT(char(12), h.RestrictHourStart, 114)) AND (CONVERT(char(12), GETDATE(), 114) <= CONVERT(char(12), h.RestrictHourEnd, 114))) OR (ISNULL(h.IsRestricHour,0) = 0))
			  and detail.ItemNo in ('' + @ItemNoList + '')
	) t
	WHERE	('' + cast(@HasMember as varchar(1)) + '' | t.ForNonMembersAlso) = 1 AND (t.PointOfSaleID  = '' + cast(@PointOfSaleID as varchar(4)) + '') AND t.DateFrom <= GETDATE() and t.DateTo >= GETDATE()
	ORDER BY Priority''

	EXEC (@SQL);

END
'

EXEC dbo.sp_executesql @statement