IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewRecipe]'))
DROP VIEW [dbo].[ViewRecipe]

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewRecipe]'))
EXEC dbo.sp_executesql @statement = N'
CREATE View [dbo].[ViewRecipe] as
SELECT	h.RecipeHeaderID
	   ,h.ItemNo AS RecipeHeaderNo
	   ,ISNULL(a.ItemName,h.RecipeName) AS RecipeName
	   ,d.RecipeDetailID
	   ,d.ItemNo AS ItemNo
	   ,i.ItemName
	   ,d.Qty
	   ,ISNULL(d.IsPacking,0) AS IsPacking
	   ,ISNULL(d.UOM,i.Userfld1) AS UOM
	   ,ISNULL(i.userfld1,'''') AS MaterialUOM
	   ,cd.ConversionRate
	   ,cd.ConversionDetID
	   ,h.itemNo + '' '' + h.RecipeName + '' '' + d.ItemNo + '' '' + i.ItemName AS Search 
FROM	RecipeHeader h 
		LEFT JOIN Item a ON a.ItemNo = h.ItemNo  
		INNER JOIN RecipeDetail d ON h.RecipeHeaderID = d.RecipeHeaderID 
		INNER JOIN Item i on d.ItemNo = i.ItemNo 
		LEFT JOIN UOMConversionHdr ch on ch.ItemNo = i.ItemNo 
									     AND ISNULL(ch.Deleted,0) = 0 
		LEFT JOIN UOMConversionDet cd on ch.ConversionHdrID = cd.ConversionHdrID 
										 AND LOWER(cd.FromUOM) = LOWER(i.Userfld1) 
										 AND LOWER(cd.ToUOM) = LOWER(d.UOM) 
										 AND ISNULL(cd.Deleted,0) = 0
WHERE	ISNULL(h.Deleted,0) = 0 
		AND ISNULL(d.Deleted,0) = 0 
		AND ISNULL(i.Deleted,0) = 0
' 
