IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockReportBreakdownByLocation]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[FetchStockReportBreakdownByLocation]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchStockReportBreakdownByLocation]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchStockReportBreakdownByLocation] 
	-- Add the parameters for the stored procedure here
	 @Search nvarchar(50),
	 @CostingMethod nvarchar(50)
AS
BEGIN
	
	DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)
	DECLARE @DynamicPivotQuery2 AS NVARCHAR(MAX)
	DECLARE @ColumnName AS NVARCHAR(MAX)
	DECLARE @ColumnNameSelect AS NVARCHAR(MAX)

	SELECT @ColumnName= ISNULL(@ColumnName + '','','''') + QUOTENAME(InventoryLocationName)
	FROM InventoryLocation where (deleted=0 or deleted is null)
	ORDER BY InventoryLocationID
	
	SELECT @ColumnNameSelect= ISNULL(@ColumnNameSelect + '','','''')  + ''ISNULL('' + QUOTENAME(InventoryLocationName) +'',0) as '' + QUOTENAME(InventoryLocationName) 
	FROM InventoryLocation where (deleted=0 or deleted is null)
	ORDER BY InventoryLocationID

	IF @CostingMethod = ''fixed avg''
	BEGIN
		SET @DynamicPivotQuery = N''
		SELECT DepartmentName, CategoryName, ItemNo, ItemName, '' + @ColumnNameSelect + ''
		FROM
		(
			SELECT st.DepartmentName, st.CategoryName, St.ItemNo, st.ItemName, st.InventoryLocationName,
					ISNULL(st.[Stock In] - st.[Stock Out] + st.[Adjustment In] - st.[Adjustment Out] + st.[Transfer In] - st.[Transfer Out],0) - ISNULL(ns.quantity,0) as StockOnHand
			FROM
			(
				SELECT IP.DepartmentName, IT.ItemNo , IC.CategoryName , IT.ItemName , IT.RetailPrice, JQ.InventoryLocationName      
						, Quantity = ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) - ISNULL(OD.Qty,0)    
						, [Stock In] = ISNULL(StockIn,0), [Stock Out] = ISNULL(StockOut,0) , [Transfer In] = ISNULL(TransferIn,0), [Transfer Out] = ISNULL(TransferOut,0)
						, [Adjustment In] = ISNULL(AdjustmentIn,0) , [Adjustment Out] = ISNULL(AdjustmentOut,0) 
						, Attributes1, Attributes2 , Attributes3, Attributes4, Attributes5, Attributes6, Attributes7 , Attributes8    
						, UndeductedSales = ISNULL(OD.Qty,0), COG = CASE WHEN ISNULL(JQ.ItemNo,'''''''') = '''''''' THEN IT.FactoryPrice WHEN ISNULL(CG.COG,0) = 0 THEN IT.FactoryPrice ELSE ISNULL(CG.COG,0) END                
						, TotalCostPrice =	CASE WHEN (ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) - ISNULL(OD.Qty,0)) < 0 THEN 0 
										ELSE (ISNULL((ISNULL(StockIn,0)- ISNULL(StockOut,0) + ISNULL(TransferIn,0) -ISNULL(TransferOut,0)+ ISNULL(AdjustmentIn,0)-AdjustmentOut),0) - ISNULL(OD.Qty,0)) * ISNULL(CG.COG,0) END  
				FROM (
						SELECT ItemNo, LI.InventoryLocationName
						, SUM (CASE WHEN movementtype LIKE ''''Stock In'''' THEN quantity  ELSE 0  END) as StockIn             
						, SUM (CASE WHEN movementtype LIKE ''''Stock Out'''' THEN quantity ELSE 0  END) as StockOut     
						, SUM (CASE WHEN movementtype LIKE ''''Transfer In'''' THEN quantity ELSE 0 END) as TransferIn     
						, SUM (CASE WHEN movementtype LIKE ''''Transfer Out'''' THEN quantity ELSE 0 END) as TransferOut     
						, SUM (CASE WHEN movementtype LIKE ''''Adjustment In'''' THEN quantity ELSE 0 END) as AdjustmentIn     
						, SUM (CASE WHEN movementtype LIKE ''''Adjustment Out'''' THEN quantity ELSE 0 END) as AdjustmentOut            
						, SUM (CASE WHEN MovementType LIKE ''''% In'''' then RemainingQty ELSE 0 END) as RemainingQTY    
						FROM InventoryHdr IH             
							INNER JOIN InventoryDet ID  ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo         
							INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID         
							GROUP BY ItemNo, LI.InventoryLocationName
						) JQ 
						RIGHT OUTER JOIN Item IT ON JQ.ItemNo = IT.ItemNo    
						INNER JOIN Category IC ON IC.CategoryName = IT.CategoryName     
						INNER JOIN itemdepartment IP on IP.itemdepartmentid = IC.ItemDepartmentId     
						LEFT JOIN (select itemno, sum(quantity) as qty from orderdet where ISNULL(inventoryhdrrefno,'''''''') = ''''''''     
								and orderhdrid in (select orderhdrid from orderhdr where isvoided = 0) group by itemno
						) OD on JQ.ItemNo = OD.ItemNo      
						LEFT JOIN (select id.itemno, avg(costofgoods) as cog from inventoryhdr ih, inventorydet id, item i 
									where ih.inventoryhdrrefno = id.inventoryhdrrefno and ih.movementtype like ''''% In'''' and id.itemno = i.itemno 
									group by id.itemno, i.factoryprice
							)  CG on JQ.Itemno = CG.ItemNo 
					WHERE (IT.Deleted=0 or IT.Deleted is null) 
					AND IP.ItemDepartmentID <> ''''SYSTEM'''' 
					AND IsInInventory = 1 
					AND IT.ItemNo + ItemName + IC.CategoryName+ ISNULL(IP.DepartmentName,'''''''') + ISNULL(ItemDesc,'''''''') + ISNULL(attributes1,'''''''') + ISNULL(attributes2,'''''''') + ISNULL(attributes3,'''''''') + ISNULL(attributes4,'''''''') + ISNULL(attributes5,'''''''') 
					+ ISNULL(attributes6,'''''''') + ISNULL(attributes7,'''''''') + ISNULL(attributes8,'''''''') LIKE N''''%'' + @search + ''%'''' 
			)as st
			LEFT OUTER JOIN 
			(
				select c.categoryname,c.ItemNo,c.ItemName, 
				f.InventoryLocationName, SUM(quantity) as quantity  
				from  
				OrderHdr a inner join 
				OrderDet b on a.OrderHdrID = b.OrderHdrID  
				inner join Item c 
				on b.ItemNo = c.ItemNo  
				inner join PointOfSale d 
				on d.PointOfSaleID = a.PointOfSaleID  
				inner join Outlet e on d.outletname = e.OutletName  
				inner join InventoryLocation f  
				on e.InventoryLocationID = f.InventoryLocationID 
				where 
				(b.InventoryHdrRefNo = '''''''' or b.InventoryHdrRefNo is null) 
				and a.IsVoided =0 and b.IsVoided = 0 
				and IsInInventory = 1 
				group by  c.categoryname,c.ItemNo,c.ItemName,f.InventoryLocationName 
			) ns on st.ItemNo = ns.ItemNo and st.InventoryLocationName = ns.InventoryLocationName
		)
		as C PIVOT
		(
			SUM(StockOnHand) 
			FOR InventoryLocationName IN ('' + @ColumnName + '')) AS PVTTable''	
	END
	ELSE
	BEGIN

		SET @DynamicPivotQuery = N''
		SELECT DepartmentName, CategoryName, ItemNo, ItemName, '' + @ColumnNameSelect + ''
		FROM
		(
			SELECT st.DepartmentName, st.CategoryName, st.ItemNo,st.ItemName, st.InventoryLocationName,
				ISNULL(ns.quantity,0) as negatifsales, ISNULL(st.[Stock In] - st.[Stock Out] + st.[Adjustment In] - st.[Adjustment Out] + 
				st.[Transfer In] - st.[Transfer Out],0) - ISNULL(ns.quantity,0) as StockOnHand
			FROM
			(
			SELECT IT.RetailPrice, IP.DepartmentName,IT.ItemNo,IC.CategoryName, IT.ItemName , JQ.InventoryLocationName
			, [Stock In], [Stock Out], [Transfer In], [Transfer Out], [Adjustment In], [Adjustment Out] 
			, Attributes1, Attributes2, Attributes3, Attributes4, Attributes5, Attributes6, Attributes7, Attributes8 
			FROM 
			( 
				SELECT ItemNo , LI.InventoryLocationName
					, SUM(CASE WHEN movementtype LIKE ''''Stock In'''' THEN quantity ELSE 0 END) as [Stock In] 
					, SUM(CASE WHEN movementtype LIKE ''''Stock Out'''' THEN quantity ELSE 0 END) as [Stock Out] 
					, SUM(CASE WHEN movementtype LIKE ''''Transfer In'''' THEN quantity ELSE 0 END) as [Transfer In] 
					, SUM(CASE WHEN movementtype LIKE ''''Transfer Out'''' THEN quantity ELSE 0 END) as [Transfer Out] 
					, SUM(CASE WHEN movementtype LIKE ''''Adjustment In'''' THEN quantity ELSE 0 END) as [Adjustment In] 
					, SUM(CASE WHEN movementtype LIKE ''''Adjustment Out'''' THEN quantity ELSE 0 END) as [Adjustment Out] 
				FROM InventoryHdr IH 
					INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
					INNER JOIN InventoryLocation LI ON IH.InventoryLocationID = LI.InventoryLocationID 
					--INNER JOIN Item I ON Item
				GROUP BY ItemNo, LI.InventoryLocationName 
			) JQ 
			RIGHT OUTER JOIN Item IT ON JQ.ItemNo = IT.ItemNo 
			INNER JOIN Category IC ON IC.CategoryName = IT.CategoryName 
			INNER JOIN itemdepartment IP on IP.itemdepartmentid = IC.ItemDepartmentId 
			WHERE (IT.Deleted=0 or IT.Deleted is null) 
			AND IP.ItemDepartmentID <> ''''SYSTEM'''' AND IsInInventory = 1 
			AND (ISNULL(IP.DepartmentName,'''''''') + IT.ItemNo+IT.Barcode+ItemName+ISNULL(attributes1,'''''''')+ISNULL(attributes2,'''''''')+ISNULL(attributes3,'''''''') 
				+ ISNULL(attributes4,'''''''') + ISNULL(attributes5,'''''''') + ISNULL(attributes6,'''''''')+ISNULL(attributes7,'''''''')+ISNULL(attributes8,'''''''') 
				+ ISNULL(ItemDesc,'''''''') + IC.CategoryName LIKE ''''%'' + @search +  ''%'''') 
			)st
			LEFT OUTER JOIN 
			(
				select c.categoryname,c.ItemNo,c.ItemName, 
				f.InventoryLocationName, SUM(quantity) as quantity  
				from  
				OrderHdr a inner join 
				OrderDet b on a.OrderHdrID = b.OrderHdrID  
				inner join Item c 
				on b.ItemNo = c.ItemNo  
				inner join PointOfSale d 
				on d.PointOfSaleID = a.PointOfSaleID  
				inner join Outlet e on d.outletname = e.OutletName  
				inner join InventoryLocation f  
				on e.InventoryLocationID = f.InventoryLocationID 
				where 
				(b.InventoryHdrRefNo = '''''''' or b.InventoryHdrRefNo is null) 
				and a.IsVoided =0 and b.IsVoided = 0 
				and IsInInventory = 1 
				group by  c.categoryname,c.ItemNo,c.ItemName,f.InventoryLocationName 
			) ns on st.ItemNo = ns.ItemNo and st.InventoryLocationName = ns.InventoryLocationName
		)
		as C PIVOT
		(
			SUM(StockOnHand) 
			FOR InventoryLocationName IN ('' + @ColumnName + '')) AS PVTTable''
	END

	print @DynamicPivotQuery 
	EXEC sp_executesql @DynamicPivotQuery
END'

END


