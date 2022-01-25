IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAllPossibleBuyXatPriceOfYPromo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchAllPossibleBuyXatPriceOfYPromo]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchAllPossibleBuyXatPriceOfYPromo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchAllPossibleBuyXatPriceOfYPromo]
 -- Add the parameters for the stored procedure here
 @ItemNoList Varchar(MAX),
 @HasMember bit,
 @PointOfSaleID int
AS
BEGIN
SET NOCOUNT ON;
declare @SQL varchar(MAX);

Set @SQL = ''
SELECT distinct PromoCampaignHdrID FROM itemgroupmap
INNER JOIN
itemgroup on
itemgroupmap.itemgroupid = itemgroup.itemgroupid
INNER JOIN
ViewPromoMasterDetail
on ViewPromoMasterDetail.itemgroupid = itemgroup.itemgroupid
WHERE itemgroupmap.itemno in ('' + @ItemNoList + '') 
AND  DateFrom <= getdate()
AND DateTo >= getdate()
AND campaigntype = ''''BuyXatThePriceOfY''''
AND Enabled = 1 AND ('' + cast(@HasMember as varchar(1)) + '' | ForNonMembersAlso) = 1
AND ((PointOfSaleID  = '' + cast(@PointOfSaleID as varchar(4)) + '') OR (PointOfSaleID = -1))''

EXEC (@SQL);
END' 
 
END