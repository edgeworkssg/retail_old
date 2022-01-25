IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxItemNoMatrix]') AND type in (N'P', N'PC'))	
	Drop Procedure [dbo].[GetMaxItemNoMatrix]

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetMaxItemNoMatrix]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[GetMaxItemNoMatrix] 
	@attributes1 varchar(50)
AS
BEGIN
	select ISNULL(MAX(CAST(SUBSTRING(ItemNo,  len(@attributes1) + 1, len(itemno)-len(@attributes1)) AS int)),0)   
	from Item where Attributes1 = @attributes1  
END'

END




