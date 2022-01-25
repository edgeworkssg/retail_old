IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewMembershipNoByPointOfSalePrefix]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNewMembershipNoByPointOfSalePrefix]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNewMembershipNoByPointOfSalePrefix]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[GetNewMembershipNoByPointOfSalePrefix]
	@PrefixCode varchar(4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    BEGIN TRY
	    SELECT isnull(max(CAST(substring(MembershipNo,LEN(@PrefixCode)+1,LEN(MEMBERSHIPNO)) AS int)),''0'')	 
	    from Membership
	    where substring(MembershipNo,0,LEN(@PrefixCode)+1) = @PrefixCode 			
    END TRY
    BEGIN CATCH
    END CATCH

END


' 
END

