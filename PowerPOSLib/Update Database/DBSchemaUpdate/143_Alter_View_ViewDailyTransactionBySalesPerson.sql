ALTER VIEW ViewDailyTransactionBySalesPerson
AS
SELECT     TOP (100) PERCENT CAST(CONVERT(varchar, OrderDate, 112) AS datetime) AS orderdate, DisplayName, PointOfSaleName, OutletName, PointOfSaleID, 
                      SUM(serviceamount) AS serviceamount, SUM(productamount) AS productamount, SUM(Amount) AS Amount
FROM         (
                SELECT     ISNULL(b.serviceamount, 0) AS serviceamount, a.Amount - ISNULL(b.serviceamount, 0) AS productamount, a.OrderDate, a.OrderHdrID, a.DisplayName, 
                                      a.PointOfSaleID, a.PointOfSaleName, a.OutletName, a.Amount
                FROM         (
                
                                    SELECT     dbo.SalesGroup.GroupName, a.Amount, a.OrderDate, a.OrderRefNo, 
                                                          a.PointOfSaleName, a.OutletName, a.CashierID, a.PointOfSaleID, 
                                                          a.OrderHdrID, a.DepartmentID, dbo.UserMst.UserName, dbo.UserMst.DisplayName, dbo.UserMst.Remark, 
                                                          dbo.UserMst.IsASalesPerson, dbo.UserMst.Deleted
                                    FROM         dbo.SalesCommissionRecord INNER JOIN
                                                          (
                                                                SELECT     dbo.OrderHdr.OrderRefNo, dbo.OrderHdr.OrderDate, OD.Amount, dbo.OrderHdr.CashierID, dbo.PointOfSale.PointOfSaleName, dbo.Outlet.OutletName, 
                                                                                      dbo.PointOfSale.PointOfSaleID, dbo.OrderHdr.InventoryHdrRefNo, dbo.OrderHdr.MembershipNo, dbo.OrderHdr.Discount, dbo.OrderHdr.OrderHdrID, 
                                                                                      dbo.PointOfSale.DepartmentID, dbo.OrderHdr.IsVoided
                                                                FROM         dbo.OrderHdr INNER JOIN
                                                                                      (SELECT OrderHdrID, SUM(Amount) Amount FROM OrderDet WHERE IsVoided = 0 AND ItemNo <> 'INST_PAYMENT' GROUP BY OrderHdrID) OD ON dbo.OrderHdr.OrderHdrID = OD.OrderHdrID INNER JOIN
                                                                                      dbo.PointOfSale ON dbo.OrderHdr.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN
                                                                                      dbo.Outlet ON dbo.PointOfSale.OutletName = dbo.Outlet.OutletName
                                                                WHERE     (dbo.OrderHdr.IsVoided = 0) 
                                                          ) a ON dbo.SalesCommissionRecord.OrderHdrID = a.OrderHdrID INNER JOIN
                                                          dbo.SalesGroup ON dbo.SalesCommissionRecord.SalesGroupID = dbo.SalesGroup.SalesGroupId INNER JOIN
                                                          dbo.UserMst ON dbo.SalesCommissionRecord.SalesPersonID = dbo.UserMst.UserName                
                             ) AS a LEFT OUTER JOIN
                                          (SELECT     x.OrderHdrID, SUM(x.Amount) AS serviceamount
                                            FROM          dbo.OrderDet AS x INNER JOIN
                                                                   dbo.Item AS y ON x.ItemNo = y.ItemNo
                                            WHERE      (y.IsServiceItem = 1) AND (x.IsVoided = 0)
                                            GROUP BY x.OrderHdrID) AS b ON a.OrderHdrID = b.OrderHdrID
             ) a
GROUP BY DisplayName, CONVERT(varchar, OrderDate, 112), PointOfSaleName, OutletName, PointOfSaleID
ORDER BY OrderDate


--SELECT     TOP (100) PERCENT CAST(CONVERT(varchar, OrderDate, 112) AS datetime) AS orderdate, DisplayName, PointOfSaleName, OutletName, PointOfSaleID, 
--                      SUM(serviceamount) AS serviceamount, SUM(productamount) AS productamount, SUM(Amount) AS Amount
--FROM         dbo.ViewProductAndServiceBySalesPerson
--GROUP BY DisplayName, CONVERT(varchar, OrderDate, 112), PointOfSaleName, OutletName, PointOfSaleID
--ORDER BY OrderDate

