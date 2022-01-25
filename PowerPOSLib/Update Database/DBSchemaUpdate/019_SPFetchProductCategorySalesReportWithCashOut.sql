IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchProductCategorySalesReportWithCashOut]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[FetchProductCategorySalesReportWithCashOut]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchProductCategorySalesReportWithCashOut]') AND type in (N'P', N'PC'))
BEGIN

EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchProductCategorySalesReportWithCashOut]
	-- Add the parameters for the stored procedure here
		@categoryname varchar(50),
		@startdate datetime,
		@enddate datetime,
		@PointOfSaleName varchar(50),
		@OutletName varchar(50),
		@DeptID varchar(5),
		@IsVoided bit,
		@sortby varchar(50),
		@sortdir varchar(5) 
AS
BEGIN

	SET NOCOUNT ON;
	set @sortby = LTRIM(RTRIM(@sortby));
	set @sortdir = LTRIM(RTRIM(@sortdir));
	set @OutletName = LTRIM(RTRIM(@OutletName));
	
	if (@OutletName = ''ALL'' AND @PointOfSaleName = ''ALL'') --See total amt for all outlets 
	Begin	
		Select *
		From
		(
			SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
			   SUM(OrderDet.Amount) AS TotalAmount, 
				isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage,
				isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,
			   OrderDet.IsVoided, @OutletName as OutletName, @PointOfSaleName as PointOfSaleName		   
			FROM  Item INNER JOIN
				  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
				  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
				  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN
 				  Outlet ON PointOfSale.OutletName = Outlet.OutletName	
				  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID	
				  LEFT outer jOIN
					ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO
					AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO)					
			WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)			   
				   AND Item.CategoryName Like @CategoryName
				   AND OrderDet.IsVoided = @IsVoided
				   AND OrderHdr.IsVoided = 0
				   --AND OrderDet.Amount >= 0
				   AND Department.DepartmentID like ''%'' + @DeptID
			GROUP BY Item.CategoryName,
					 OrderDet.IsVoided
		
		UNION 
		
			SELECT CashRecordingTypeName as CategoryName, 0 as TotalQuantity, 0 - SUM(ISNULL(amount,0)) as TotalAmount, 0 as ProfitLoss,
				0 as ProfitLossPercentage, 0 as TotalCostOfGoodsSold, 0 as GSTAmount, 0 as IsVoided,@OutletName as OutletName, @PointOfSaleName as PointOfSaleName	
			FROM ViewCashRecording
			WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
				   AND CashRecordingTypeName = ''CASH OUT''
				   AND DepartmentID like ''%'' + @DeptID
			GROUP BY CashRecordingTypeName, OutletName, PointOfSaleName
		)e
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by CategoryName asc)				
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalAmount desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalAmount asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalQuantity desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalQuantity asc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalCostOfGoodsSold desc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalCostOfGoodsSold asc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLoss desc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLoss asc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLossPercentage desc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLossPercentage asc)
				ELSE rank() over (order by CategoryName asc)
			END			
	End
	Else if (@PointOfSaleName = ''ALL'' and @OutletName != ''ALL'')  --See total amt for PointOfSales at one outlet
	Begin
		SELECT *
		FROM
		(
			SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
			   SUM(OrderDet.Amount) AS TotalAmount, 
				isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage,
				isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,
			   OrderDet.IsVoided, ''ALL'' as PointOfSaleName, Outlet.OutletName		   
			FROM  Item INNER JOIN
				  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
				  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
				  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
 				  Outlet ON PointOfSale.OutletName = Outlet.OutletName
				  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID
				  LEFT outer jOIN
					ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO
					AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO)					
			WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)			   
				   AND Item.CategoryName Like @CategoryName
				   AND OrderDet.IsVoided = @IsVoided
				   AND Outlet.OutletName Like @OutletName
				   --AND OrderDet.Amount >= 0
				   AND OrderHdr.IsVoided = 0
				   AND Department.DepartmentID like ''%'' + @DeptID
			GROUP BY Item.CategoryName,
					 Outlet.OutletName, OrderDet.IsVoided
			UNION 
		
			SELECT CashRecordingTypeName as CategoryName, 0 as TotalQuantity, 0 - SUM(ISNULL(amount,0)) as TotalAmount, 0 as ProfitLoss,
				0 as ProfitLossPercentage, 0 as TotalCostOfGoodsSold, 0 as GSTAmount, 0 as IsVoided,OutletName, @PointOfSaleName as PointOfSaleName	
			FROM ViewCashRecording
			WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
				   AND CashRecordingTypeName = ''CASH OUT''
				   AND DepartmentID like ''%'' + @DeptID
				   AND OutletName Like @OutletName
			GROUP BY CashRecordingTypeName, OutletName, PointOfSaleName
		)e
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by CategoryName asc)				
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalAmount desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalAmount asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalQuantity desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalQuantity asc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalCostOfGoodsSold desc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalCostOfGoodsSold asc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLoss desc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLoss asc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLossPercentage desc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLossPercentage asc)
				ELSE rank() over (order by CategoryName asc)
			END			
	End
	Else if (@PointOfSaleName != ''ALL'' and @OutletName = ''ALL'')  	
	begin
		SELECT *
		FROM
		(
			SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
			   SUM(OrderDet.Amount) AS TotalAmount, 
				 isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage,
				isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,
			   OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName		   
			FROM  Item INNER JOIN
				  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
				  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
				  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID INNER JOIN
 				  Outlet ON PointOfSale.OutletName = Outlet.OutletName 		
				  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID	
				  LEFT outer jOIN
					ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO
					AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO)					
			WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
				   AND PointOfSale.PointOfSaleName Like @PointOfSaleName
				   AND Item.CategoryName Like @CategoryName
				   AND OrderDet.IsVoided = @IsVoided
				   --AND OrderDet.Amount >= 0
				   AND OrderHdr.IsVoided = 0
				   AND Department.DepartmentID like ''%'' + @DeptID
			GROUP BY Item.CategoryName,
					 PointOfSale.PointOfSaleName, Outlet.OutletName, OrderDet.IsVoided
			UNION 
		
			SELECT CashRecordingTypeName as CategoryName, 0 as TotalQuantity, 0 - SUM(ISNULL(amount,0)) as TotalAmount, 0 as ProfitLoss,
				0 as ProfitLossPercentage, 0 as TotalCostOfGoodsSold, 0 as GSTAmount, 0 as IsVoided,OutletName, @PointOfSaleName as PointOfSaleName	
			FROM ViewCashRecording
			WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
				   AND CashRecordingTypeName = ''CASH OUT''
				   AND DepartmentID like ''%'' + @DeptID
				   AND PointOfSaleName Like @PointOfSaleName
			GROUP BY CashRecordingTypeName, OutletName, PointOfSaleName
		)e
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by CategoryName asc)				
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalAmount desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalAmount asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalQuantity desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalQuantity asc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalCostOfGoodsSold desc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalCostOfGoodsSold asc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLoss desc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLoss asc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLossPercentage desc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLossPercentage asc)
				ELSE rank() over (order by CategoryName asc)
			END			
	end
	Else
	Begin   --See total amt for one PointOfSale --OutletName and PointOfSale name is specified
    -- Insert statements for procedure here
		SELECT *
		FROM
		(
			SELECT Item.CategoryName, SUM(OrderDet.Quantity) AS TotalQuantity, 
			   SUM(OrderDet.Amount) AS TotalAmount, 
				isnull(sum(CostofGoods * ViewInventorySalesStockOut.Quantity),0) as TotalCostOfGoodsSold,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity)),0) as ProfitLoss,
				isnull((Sum(OrderDet.Amount) - sum(CostofGoods * ViewInventorySalesStockOut.Quantity))/(Sum(OrderDet.Amount)+0.001)*100,0) as ProfitLossPercentage,
				isnull(SUM(OrderDet.GSTAmount),0) as GSTAmount,
			   OrderDet.IsVoided, PointOfSale.PointOfSaleName, Outlet.OutletName		   
			FROM  Item INNER JOIN
				  OrderDet ON Item.ItemNo = OrderDet.ItemNo INNER JOIN
				  OrderHdr ON OrderDet.OrderHdrID = OrderHdr.OrderHdrID INNER JOIN
				  PointOfSale ON OrderHdr.PointOfSaleID = PointOfSale.PointOfSaleID  INNER JOIN
 				  Outlet ON PointOfSale.OutletName = Outlet.OutletName	
				  INNER JOIN Department on PointOfSale.DepartmentID = Department.DepartmentID	
				  LEFT outer jOIN
					ViewInventorySalesStockOut ON (ORDERDET.INVENTORYHDRREFNO = ViewInventorySalesStockOut.INVENTORYHDRREFNO
					AND ViewInventorySalesStockOut.ITEMNO=ORDERDET.ITEMNO)					
			WHERE  (OrderHdr.OrderDate BETWEEN @startdate AND @enddate)
				   AND PointOfSale.PointOfSaleName Like @PointOfSaleName
				   AND Outlet.OutletName Like @OutletName
				   AND Item.CategoryName Like @CategoryName
				   AND OrderDet.IsVoided = @IsVoided
				   AND OrderHdr.IsVoided = 0
				   --AND OrderDet.Amount >= 0
				   AND Department.DepartmentID like ''%'' + @DeptID
			GROUP BY Item.CategoryName, Outlet.OutletName,
					 PointOfSale.PointOfSaleName, OrderDet.IsVoided
			UNION 
		
			SELECT CashRecordingTypeName as CategoryName, 0 as TotalQuantity, 0 - SUM(ISNULL(amount,0)) as TotalAmount, 0 as ProfitLoss,
				0 as ProfitLossPercentage, 0 as TotalCostOfGoodsSold, 0 as GSTAmount, 0 as IsVoided,OutletName, @PointOfSaleName as PointOfSaleName	
			FROM ViewCashRecording
			WHERE  (CashRecordingTime BETWEEN @startdate AND @enddate)			   
				   AND CashRecordingTypeName = ''CASH OUT''
				   AND DepartmentID like ''%'' + @DeptID
				   AND PointOfSaleName Like @PointOfSaleName
				   AND OutletName Like @OutletName
			GROUP BY CashRecordingTypeName, OutletName, PointOfSaleName
		)e
		ORDER BY
		CASE    
				WHEN @sortby = ''CategoryName'' and @sortdir = ''DESC'' 
				THEN rank() over (order by CategoryName desc)
				WHEN @sortby = ''CategoryName'' and @sortdir = ''ASC'' 
				THEN rank() over (order by CategoryName asc)				
				WHEN @sortby = ''IsVoided'' and @sortdir = ''DESC'' 
				THEN rank() over (order by IsVoided desc)
				WHEN @sortby = ''IsVoided'' and @sortdir = ''ASC'' 
				THEN rank() over (order by IsVoided asc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalAmount desc)
				WHEN @sortby = ''TotalAmount'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalAmount asc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalQuantity desc)
				WHEN @sortby = ''TotalQuantity'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalQuantity asc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''DESC'' 
				THEN rank() over (order by TotalCostOfGoodsSold desc)
				WHEN @sortby = ''TotalCostOfGoodsSold'' and @sortdir = ''ASC'' 
				THEN rank() over (order by TotalCostOfGoodsSold asc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLoss desc)
				WHEN @sortby = ''ProfitLoss'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLoss asc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''DESC'' 
				THEN rank() over (order by ProfitLossPercentage desc)
				WHEN @sortby = ''ProfitLossPercentage'' and @sortdir = ''ASC'' 
				THEN rank() over (order by ProfitLossPercentage asc)
				ELSE rank() over (order by CategoryName asc)
			END	
	End
END
'
END


