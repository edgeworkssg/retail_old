-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[FetchCustomerPurchaseBehaviorReport] 
	-- Add the parameters for the stored procedure here
	@startdate datetime,
	@enddate datetime,
	@CategoryName varchar(50),
	@ItemName varchar(50),
	@PointOfSaleName varchar(50),
	@OutletName varchar(50),
	@membershipno varchar(50),
	@nametoappear varchar(50),
	@firstname varchar(50),
	@lastname varchar(50),
	@MembershipGroupID int,
	@sortby varchar(50),
	@sortdir varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare	
	@Realstartdate datetime,
	@Realenddate datetime,
	@RealCategoryName varchar(50),
	@RealItemName varchar(50),
	@RealPointOfSaleName varchar(50),
	@RealOutletName varchar(50),
	@Realmembershipno varchar(50),
	@Realnametoappear varchar(50),
	@Realfirstname varchar(50),
	@Reallastname varchar(50),
	@RealMembershipGroupID int,
	@Realsortby varchar(50),
	@Realsortdir varchar(5)

	SET @Realstartdate = @startdate
	SET @Realenddate = @enddate
	SET @RealCategoryName = @CategoryName
	SET @RealItemName = @ItemName
	SET @RealPointOfSaleName = @PointOfSaleName
	SET @RealOutletName = @OutletName
	SET @Realmembershipno = @membershipno
	SET @Realnametoappear = @nametoappear
	SET @Realfirstname = @firstname
	SET @Reallastname = @lastname
	SET @RealMembershipGroupID = @MembershipGroupID
	SET @Realsortby = @sortby
	SET @Realsortdir = @sortdir



    -- Insert statements for procedure here
	select 
		sum(lineamount) as TotalPurchased,
		sum(Quantity) as TotalItemBought,
		Count(distinct OrderHdrID) as NumberOfTransaction,
		sum(lineamount)/(Count(distinct OrderHdrID)+0.0001) as AvgAmountPerTransaction,
		(sum(lineamount)/(sum(Quantity)+0.0001)) as AvgAmountPerItem,
		nametoappear, membershipno, Email,mobile, FirstName,LastName,StreetName,City,
		Country,dateofbirth, expirydate, groupname, SubscriptionDate
	from viewtransactionwithmembership
	where orderdate > @Realstartdate and 
	orderdate < @Realenddate
	and pointofsalename like '%' + @RealPointOfSaleName
	and OutletName  like '%' + @RealOutletName
	and CategoryName like '%' + @RealCategoryName + '%'
	and ItemName like '%' + @RealItemName + '%'
	and membershipno like '%' + @Realmembershipno + '%'
	and nametoappear like N'%' + @Realnametoappear + '%'
	and firstname like N'%' + @Realfirstname + '%'
	and lastname like N'%' + @Reallastname + '%'
	and (@RealMembershipGroupID = 0 or MembershipGroupID = @RealMembershipGroupID)
	group by nametoappear, membershipno, Email,mobile, FirstName,LastName,StreetName,City,Country,dateofbirth, expirydate, groupname,SubscriptionDate
	ORDER BY
		CASE    WHEN @Realsortby = 'TotalPurchased' and @Realsortdir = 'DESC' 
				THEN rank() over (order by sum(lineamount) desc)
				WHEN @Realsortby = 'TotalPurchased' and @Realsortdir = 'ASC' 
				THEN rank() over (order by sum(lineamount) asc)
				WHEN @Realsortby = 'TotalItemBought' and @Realsortdir = 'DESC' 
				THEN rank() over (order by sum(Quantity) desc)
				WHEN @Realsortby = 'TotalItemBought' and @Realsortdir = 'ASC' 
				THEN rank() over (order by sum(Quantity) asc)				
				WHEN @Realsortby = 'NumberOfTransaction' and @Realsortdir = 'DESC' 
				THEN rank() over (order by Count(distinct OrderHdrID) desc)
				WHEN @Realsortby = 'NumberOfTransaction' and @Realsortdir = 'ASC' 
				THEN rank() over (order by Count(distinct OrderHdrID) asc)
				WHEN @Realsortby = 'AvgAmountPerTransaction' and @Realsortdir = 'DESC' 
				THEN rank() over (order by (sum(lineamount)/(Count(distinct OrderHdrID)+0.0001)) desc)
				WHEN @Realsortby = 'AvgAmountPerTransaction' and @Realsortdir = 'ASC' 
				THEN rank() over (order by (sum(lineamount)/(Count(distinct OrderHdrID)+0.0001)) asc)
				WHEN @Realsortby = 'AvgAmountPerItem' and @Realsortdir = 'DESC' 
				THEN rank() over (order by (sum(lineamount)/(sum(Quantity)+0.0001)) desc)
				WHEN @Realsortby = 'AvgAmountPerItem' and @Realsortdir = 'ASC' 
				THEN rank() over (order by (sum(lineamount)/(sum(Quantity)+0.0001)) asc)
				WHEN @Realsortby = 'nametoappear' and @Realsortdir = 'DESC' 
				THEN rank() over (order by nametoappear desc)
				WHEN @Realsortby = 'nametoappear' and @Realsortdir = 'ASC' 
				THEN rank() over (order by nametoappear asc)
			    WHEN @Realsortby = 'membershipno' and @Realsortdir = 'DESC' 
				THEN rank() over (order by membershipno desc)				
				WHEN @Realsortby = 'membershipno' and @Realsortdir = 'ASC' 
				THEN rank() over (order by membershipno ASC)
				WHEN @Realsortby = 'groupname' and @Realsortdir = 'ASC' 
				THEN rank() over (order by groupname ASC)	
				ELSE rank() over (order by sum(lineamount) DESC)			
		END 
END



