IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchPreOrderReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchPreOrderReport]

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchPreOrderReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[FetchPreOrderReport]
    @StartDate datetime,
    @EndDate datetime,
    @ItemName nvarchar(300),
    @MembershipNo varchar(50),
    @CustName varchar(200),
    @IsOutstanding varchar(3),
    @Notified varchar(3),
    @OnlyReadyToDeliver bit,
    @SortBy varchar(50),
    @SortDir varchar(5),
    @Status varchar(20),
    @OutletName varchar(50)
AS
BEGIN

SELECT *
FROM (
    SELECT ISNULL(oh.userfld5, oh.OrderRefNo) AS InvoiceNo, oh.OrderDate, it.ItemNo, it.ItemName, 
        oh.MembershipNo, mb.NameToAppear AS CustomerName, ISNULL(mb.Mobile, '''') as MobileNo, od.Quantity AS OrderQty, od.UnitPrice, od.Amount,
        dbo.GetItemQtyOnHandItemSummary(it.ItemNo, ol.InventoryLocationID) AS QtyOnHand,
        dbo.GetItemTotalPreOrderQty(od.ItemNo, ol.InventoryLocationID) AS TotalPreOrderQty, 
        (od.Quantity - ISNULL(dod.Quantity, 0)) AS OutstandingQty,
        ISNULL(od.userflag2, 0) AS IsNotified, (od.Amount - ISNULL(od.userfloat1, 0)) AS OutstandingBal, ISNULL(od.userfloat1,0) as PaidAmount, 
        DeliveryStatus = CASE
                WHEN dod.OrderDetID IS NULL THEN ''Not Delivered''
                WHEN dod.IsDelivered = 1 THEN ''Delivered''
                ELSE ''Pending''
            END,
        oh.OrderHdrID, od.OrderDetID, oh.PointOfSaleID, ol.OutletName, dod.DeliveredDate
    FROM OrderHdr oh
        INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
        INNER JOIN Item it ON it.ItemNo = od.ItemNo
        LEFT JOIN (
            SELECT dod.OrderDetID, SUM(dod.Quantity) AS Quantity, 
                MIN(CONVERT(int, ISNULL(doh.IsDelivered, 0))) AS IsDelivered,
                MAX(doh.DeliveredDate) as DeliveredDate
            FROM DeliveryOrderDetails dod
                INNER JOIN DeliveryOrder doh ON doh.OrderNumber = dod.DOHDRID
            WHERE ISNULL(dod.Deleted, 0) = 0 AND ISNULL(doh.Deleted, 0) = 0
            GROUP BY dod.OrderDetID
        ) dod on dod.OrderDetID = od.OrderDetID 
        INNER JOIN Membership mb ON mb.MembershipNo = oh.MembershipNo
        INNER JOIN PointOfSale pos ON pos.PointOfSaleID = oh.PointOfSaleID
        INNER JOIN Outlet ol ON ol.OutletName = pos.OutletName
    WHERE oh.IsVoided = 0 AND od.IsVoided = 0
        AND od.IsPreOrder = 1
        AND oh.OrderDate BETWEEN @StartDate AND @EndDate
        AND (@OutletName = ''ALL'' or ol.OutletName = @OutletName)
        AND (it.ItemName LIKE CASE WHEN @ItemName = '''' THEN it.ItemName ELSE ''%'' + @ItemName + ''%'' END
			 OR it.ItemNo LIKE CASE WHEN @ItemName = '''' THEN it.ItemNo ELSE ''%'' + @ItemName + ''%'' END)
        AND oh.MembershipNo = CASE WHEN @MembershipNo = '''' THEN oh.MembershipNo ELSE @MembershipNo END
        AND (mb.NameToAppear LIKE CASE WHEN @CustName = '''' THEN mb.NameToAppear ELSE ''%'' + @CustName + ''%'' END
            OR mb.FirstName + '' '' + mb.LastName LIKE CASE WHEN @CustName = '''' THEN mb.FirstName + '' '' + mb.LastName ELSE ''%'' + @CustName + ''%'' END)
) a
WHERE 1 = CASE @IsOutstanding
            WHEN ''YES'' THEN CASE WHEN OutstandingBal > 0 THEN 1 ELSE 0 END
            WHEN ''NO'' THEN CASE WHEN OutstandingBal <= 0 THEN 1 ELSE 0 END
            ELSE 1
          END
    AND 1 = CASE @Notified
                WHEN ''YES'' THEN CASE WHEN IsNotified = 1 THEN 1 ELSE 0 END
                WHEN ''NO'' THEN CASE WHEN IsNotified = 0 THEN 1 ELSE 0 END
                ELSE 1
            END
    AND 1 = CASE @OnlyReadyToDeliver
                WHEN 0 THEN 1
                WHEN 1 THEN CASE WHEN (QtyOnHand > 0 AND OutstandingQty > 0) THEN 1 ELSE 0 END
            END    
    AND 1 = CASE @Status
				WHEN ''Delivered'' THEN CASE WHEN DeliveryStatus = ''Delivered'' THEN 1 ELSE 0 END
				WHEN ''Not Delivered'' THEN CASE WHEN DeliveryStatus = ''Not Delivered'' THEN 1 ELSE 0 END
				ELSE 1
			END
ORDER BY CASE
    WHEN @SortBy = ''InvoiceNo'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY InvoiceNo ASC)
    WHEN @SortBy = ''InvoiceNo'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY InvoiceNo DESC)
    WHEN @SortBy = ''OrderDate'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY OrderDate ASC)
    WHEN @SortBy = ''OrderDate'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY OrderDate DESC)
    WHEN @SortBy = ''ItemNo'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY ItemNo ASC)
    WHEN @SortBy = ''ItemNo'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY ItemNo DESC)
    WHEN @SortBy = ''ItemName'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY ItemName ASC)
    WHEN @SortBy = ''ItemName'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY ItemName DESC)
    WHEN @SortBy = ''CustomerName'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY CustomerName ASC)
    WHEN @SortBy = ''CustomerName'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY CustomerName DESC)
    WHEN @SortBy = ''OrderQty'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY OrderQty ASC)
    WHEN @SortBy = ''OrderQty'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY OrderQty DESC)
    WHEN @SortBy = ''UnitPrice'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY UnitPrice ASC)
    WHEN @SortBy = ''UnitPrice'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY UnitPrice DESC)
    WHEN @SortBy = ''Amount'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY Amount ASC)
    WHEN @SortBy = ''Amount'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY Amount DESC)
    WHEN @SortBy = ''QtyOnHand'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY QtyOnHand ASC)
    WHEN @SortBy = ''QtyOnHand'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY QtyOnHand DESC)
    WHEN @SortBy = ''OutstandingQty'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY OutstandingQty ASC)
    WHEN @SortBy = ''OutstandingQty'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY OutstandingQty DESC)
    WHEN @SortBy = ''IsNotified'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY IsNotified ASC)
    WHEN @SortBy = ''IsNotified'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY IsNotified DESC)
    WHEN @SortBy = ''OutstandingBal'' AND @SortDir = ''ASC''
        THEN RANK() OVER (ORDER BY OutstandingBal ASC)
    WHEN @SortBy = ''OutstandingBal'' AND @SortDir = ''DESC''
        THEN RANK() OVER (ORDER BY OutstandingBal DESC)
    ELSE
        RANK() OVER (ORDER BY OrderDate ASC)
    END

END
'

END