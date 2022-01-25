IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewOrderHdrInstOutstandingBal]'))
DROP VIEW [dbo].[ViewOrderHdrInstOutstandingBal]

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[ViewOrderHdrInstOutstandingBal]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[ViewOrderHdrInstOutstandingBal]
AS

-- OrderHdr with 1 additional column: Installment Outstanding Balance (InstOutstandingBal)

SELECT oh.OrderHdrID, oh.OrderRefNo, oh.Discount, oh.InventoryHdrRefNo, oh.CashierID, oh.PointOfSaleID, oh.OrderDate, oh.GrossAmount, oh.NettAmount, oh.DiscountAmount, oh.GST, oh.IsVoided, oh.MembershipNo, oh.Remark, 
                         oh.CreatedOn, oh.CreatedBy, oh.ModifiedOn, oh.ModifiedBy, oh.UniqueID, oh.userfld1, oh.userfld2, oh.userfld3, oh.userfld4, oh.userfld5, oh.userfld6, oh.userfld7, oh.userfld8, oh.userfld9, oh.userfld10, oh.userflag1, 
                         oh.userflag2, oh.userflag3, oh.userflag4, oh.userflag5, oh.userfloat1, oh.userfloat2, oh.userfloat3, oh.userfloat4, oh.userfloat5, oh.userint1, oh.userint2, oh.userint3, oh.userint4, oh.userint5, oh.PromoCodeID, oh.GSTAmount, 
                         oh.IsPointAllocated, ISNULL(bal.OutstandingBalance, 0) AS InstOutstandingBal
FROM OrderHdr oh
    LEFT JOIN (
        SELECT OrderHdrID, OutstandingBalance = ISNULL(SUM(Credit),0.00) - ISNULL(SUM(Debit),0.00)
        FROM ( 
                SELECT OrderHdrID , ISNULL(SUM(Amount),0.00) AS Credit, 0.00 AS Debit 
                FROM OrderHdr OH 
                    INNER JOIN ReceiptDet RD ON OH.OrderHdrID = RD.ReceiptHdrID 
                WHERE OH.IsVoided = 0 
                    AND PaymentType = ''INSTALLMENT'' 
                    AND OrderDate <= GETDATE() 
                GROUP BY OrderHdrID  
                UNION ALL  
                SELECT OD.Userfld3 AS OrderHdrID ,0.00 as Credit, ISNULL(SUM(AMOUNT),0.00) as Debit  
                FROM OrderHdr OH 
                    INNER JOIN OrderDet OD ON OH.OrderHdrID = OD.OrderHdrID 
                WHERE OH.IsVoided = 0 
                    AND OD.IsVoided = 0 
                    AND ItemNo = ''INST_PAYMENT'' 
                    AND OrderDate <= GETDATE()
                GROUP BY OD.Userfld3 
            ) DT 
        GROUP BY OrderHdrID  
    ) bal ON oh.OrderHdrID = bal.OrderHdrID


' 
