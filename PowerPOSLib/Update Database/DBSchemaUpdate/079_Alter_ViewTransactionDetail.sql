ALTER VIEW [dbo].[ViewTransactionDetail]
AS
SELECT     dbo.OrderDet.ItemNo, dbo.OrderDet.OrderDetDate, dbo.OrderDet.Quantity, dbo.OrderDet.UnitPrice, dbo.OrderDet.Discount, dbo.OrderDet.Amount, 
                      dbo.OrderDet.IsPromo, dbo.OrderDet.PromoDiscount, dbo.OrderDet.PromoAmount, dbo.OrderDet.IsPromoFreeOfCharge, dbo.OrderDet.IsFreeOfCharge, 
                      dbo.PointOfSale.PointOfSaleName, dbo.OrderHdr.OrderRefNo, dbo.Item.ItemName, dbo.OrderHdr.NettAmount, dbo.OrderHdr.IsVoided, 
                      dbo.OrderDet.IsVoided AS IsLineVoided, dbo.Item.CategoryName, dbo.PointOfSale.OutletName, dbo.PointOfSale.DepartmentID, dbo.PointOfSale.PointOfSaleID, 
                      dbo.OrderDet.InventoryHdrRefNo, dbo.OrderDet.IsPreOrder
FROM         dbo.OrderDet INNER JOIN
                      dbo.OrderHdr ON dbo.OrderDet.OrderHdrID = dbo.OrderHdr.OrderHdrID INNER JOIN
                      dbo.PointOfSale ON dbo.OrderHdr.PointOfSaleID = dbo.PointOfSale.PointOfSaleID INNER JOIN
                      dbo.Item ON dbo.OrderDet.ItemNo = dbo.Item.ItemNo

