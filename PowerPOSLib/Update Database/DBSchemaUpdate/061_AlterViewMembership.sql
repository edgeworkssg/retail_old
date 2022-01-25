declare @statement Nvarchar(max)
set @statement = ''
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewMembership]'))
BEGIN
 set @statement = N'
ALTER VIEW [dbo].[ViewMembership]
AS
SELECT        dbo.Membership.MembershipNo, dbo.Membership.MembershipGroupId, dbo.Membership.Title, dbo.Membership.LastName, dbo.Membership.FirstName, 
                         dbo.Membership.ChristianName, dbo.Membership.NameToAppear, dbo.Membership.Gender, dbo.Membership.DateOfBirth, dbo.Membership.Nationality, 
                         dbo.Membership.NRIC, dbo.Membership.Occupation, dbo.Membership.MaritalStatus, dbo.Membership.Email, dbo.Membership.Block, 
                         dbo.Membership.BuildingName, dbo.Membership.StreetName, dbo.Membership.UnitNo, dbo.Membership.City, dbo.Membership.Country, dbo.Membership.ZipCode, 
                         dbo.Membership.Mobile, dbo.Membership.Office, dbo.Membership.Fax, dbo.Membership.Home, dbo.Membership.ExpiryDate, dbo.Membership.Remarks, 
                         dbo.Membership.SubscriptionDate, dbo.Membership.IsChc, dbo.Membership.Ministry, dbo.Membership.IsStudentCard, dbo.Membership.CreatedOn, 
                         dbo.Membership.CreatedBy, dbo.Membership.ModifiedOn, dbo.Membership.ModifiedBy, dbo.Membership.Deleted, dbo.Membership.UniqueID, 
                         dbo.MembershipGroup.GroupName, dbo.MembershipGroup.Discount, dbo.Membership.ChineseName, dbo.Membership.StreetName2, 
                         ISNULL(dbo.Membership.StreetName, '''') + '' '' + ISNULL(dbo.Membership.StreetName2, '''') + '' '' + ISNULL(dbo.Membership.City, '''') 
                         + '' '' + ISNULL(dbo.Membership.Country, '''') + '' '' + ISNULL(dbo.Membership.ZipCode, '''') AS Address, DATEPART(m, dbo.Membership.DateOfBirth) AS BirthDayMonth, 
                         ISNULL(dbo.Membership.FirstName, '''') + '' '' + ISNULL(dbo.Membership.LastName, '''') + '' '' + ISNULL(dbo.Membership.NameToAppear, '''') 
                         + '' '' + ISNULL(dbo.Membership.ChristianName, '''') + '' '' + ISNULL(dbo.Membership.ChineseName, '''') AS Name, dbo.Membership.IsVitaMix, 
                         dbo.Membership.IsWaterFilter, dbo.Membership.IsJuicePlus, dbo.Membership.IsYoung, dbo.Membership.SalesPersonID, 
						 dbo.Membership.userfld1, dbo.Membership.userfld2, dbo.Membership.userfld3, dbo.Membership.userfld4, dbo.Membership.userfld5,
                         dbo.Membership.userfld6, dbo.Membership.userfld7, dbo.Membership.userfld8, dbo.Membership.userfld9, dbo.Membership.userfld10 
FROM            dbo.Membership INNER JOIN
                         dbo.MembershipGroup ON dbo.Membership.MembershipGroupId = dbo.MembershipGroup.MembershipGroupId'
   
EXEC dbo.sp_executesql @statement                      
END
                         
