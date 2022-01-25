IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemMatrix]') AND type in (N'P', N'PC'))
	Drop Procedure [dbo].[GetItemMatrix]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetItemMatrix]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetItemMatrix]
	@itemno varchar(50)
AS
BEGIN
	declare @attributes1 varchar(50)
	declare @attributes3 varchar(50)
	declare @listattributes3 varchar(255)
	declare @attributes4 varchar(50)
	declare @listattributes4 varchar(255)

	select top 1 @attributes1 = Attributes1 
	from Item
	where ItemNo = @itemno and Deleted = 0 

	SET @listattributes3 = ''''
	SET @listattributes4 = ''''

	DECLARE db_cursor CURSOR FOR  
	SELECT distinct Attributes3 
	FROM item
	WHERE Attributes1 = @attributes1 and Deleted = 0
	OPEN db_cursor   
	FETCH NEXT FROM db_cursor INTO @attributes3   

	WHILE @@FETCH_STATUS = 0   
	BEGIN   
		   SET @listattributes3 = @listattributes3 + @attributes3 + '',''

		   FETCH NEXT FROM db_cursor INTO @attributes3   
	END   

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	DECLARE db_cursor2 CURSOR FOR  
	SELECT distinct Attributes4 
	FROM item
	WHERE Attributes1 = @attributes1 and Deleted = 0
	OPEN db_cursor2   
	FETCH NEXT FROM db_cursor2 INTO @attributes4   

	WHILE @@FETCH_STATUS = 0   
	BEGIN   
		   SET @listattributes4 = @listattributes4 + @attributes4 + '',''

		   FETCH NEXT FROM db_cursor2 INTO @attributes4   
	END   

	CLOSE db_cursor2   
	DEALLOCATE db_cursor2

	SELECT Attributes1 as ItemNo, Attributes2 as ItemName, Barcode, CategoryName, RetailPrice, FactoryPrice, MinimumPrice, 
			ItemDesc, IsServiceItem, IsInInventory, IsNonDiscountable, 
           IsCourse, CourseTypeID, Brand, ProductLine, Attributes1, Attributes2, 
		   SUBSTRING(@listattributes3,0,LEN(@listattributes3)) as Attributes3, SUBSTRING(@listattributes4,0,LEN(@listattributes4)) as Attributes4, 
		   Attributes5, Attributes6, Attributes7, Attributes8, Remark, 
           ProductionDate, IsGST, hasWarranty, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy, UniqueID, Deleted, 
		   userfld1 as Userfld1, userfld2 as Userfld2, userfld3 as Userfld3, userfld4 as Userfld4, userfld5 as Userfld5, 
		   userfld6 as Userfld6, userfld7 as Userfld7, userfld8 as Userfld8, userfld9 as Userfld9, userfld10 as Userfld10, 
		   userflag1 as Userflag1, userflag2 as Userflag2, userflag3 as Userflag3, userflag4 as Userflag4, userflag5 as Userflag5, 
		   userfloat1 as Userfloat1, userfloat2 as Userfloat2, userfloat3 as Userfloat3, userfloat4 as Userfloat4, userfloat5 as Userfloat5, 
		   userint1 as Userint1, userint2 as Userint2, userint3 as Userint3, userint4 as Userint4, userint5 as Userint5, 
		   IsDelivery, GSTRule, IsVitaMix, IsWaterFilter, IsYoung, IsJuicePlus, IsCommission
	FROM  Item
	WHERE (ItemNo = @itemno) AND Deleted = 0

END'
END


