IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_DeliveryOrder]') AND type in (N'P', N'PC'))
	Drop Procedure [dbo].[REPORT_DeliveryOrder]

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REPORT_DeliveryOrder]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[REPORT_DeliveryOrder]
	@InpStartDate datetime, 
	@InpEndDate datetime
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @StartDate datetime, @EndDate datetime

	set @StartDate = @InpStartDate
	set @EndDate = @InpEndDate

    -- Insert statements for procedure here
	SELECT   DO.PurchaseOrderRefNo DONumber
			,CAST(DATEPART(DAY,DO.DeliveryDate) AS VARCHAR(4))
			 +''/''+CAST(DATEPART(MONTH,DO.DeliveryDate) AS VARCHAR(4))
			 +''/''+CAST(DATEPART(YEAR,DO.DeliveryDate) AS VARCHAR(4)) DODate     
			,'''' CustomerOrderNo
			,OH.userfld5 InvoiceNo
			,CAST(DATEPART(DAY,OH.OrderDate) AS VARCHAR(4))
			 +''/''+CAST(DATEPART(MONTH,OH.OrderDate) AS VARCHAR(4))
			 +''/''+CAST(DATEPART(YEAR,OH.OrderDate) AS VARCHAR(4)) InvoiceDate             
			,OH.CashierID SalesmanName
			,''Cash on Delivery'' PaymentTerm
			,OU.OutletName SalesOffice
			,OH.MembershipNo CustomerCode
			,'''' SalesOrder
			,ISNULL(DO.MobileNo,DO.HomeNo) CustomerTelNo
			,DO.PostalCode CustomerPostalCode
			,M.NameToAppear CustomerName1
			,ISNULL(M.StreetName,'''')
			 +'' ''+ISNULL(M.StreetName2,'''')
			 +'' ''+ISNULL(M.Country,'''')
			 +'' ''+ISNULL(M.ZipCode,'''') 
			 +'' ''+ISNULL(M.Block,'''')
			 +'' ''+ISNULL(M.UnitNo,'''') 
			 +'' ''+ISNULL(M.BuildingName,'''')INVToAddrs1
			,'''' INVToAddrs2
			,'''' INVToAddrs3
			,'''' INVToAddrs4
			,'''' INVToAddrs5
			,'''' INVToAddrs6
			,DO.RecipientName DeliveryToName1
			,DO.DeliveryAddress DELToAddrs1
			,'''' DELToAddrs2
			,'''' DELToAddrs3
			,'''' DELToAddrs4
			,'''' DELToAddrs5
			,'''' DELToAddrs6
			,ROW_NUMBER() OVER(PARTITION BY OH.OrderRefNo ORDER BY I.ItemNo) [No]
			,I.ItemNo ItemCode
			,CASE WHEN I.ItemNo LIKE ''%-DISPLAY'' THEN ''HQFG06''
				  WHEN I.ItemNo LIKE ''%-RECON'' THEN ''HQFG10''
				  WHEN I.ItemNo LIKE ''%-WHS'' THEN ''HQFG06''
				  ELSE ''POSKIV'' END WarehouseCode
			,OD.Quantity Qty
			,'''' UOM
			,'''' CURR
			,OD.UnitPrice UnitPrice
			,cast(OD.Discount as Decimal(18,0)) as DISCPercent
			,OD.Amount AmountAfterLineDisc
			,OD.Amount - ISNULL(OD.Userfloat1,0) OutstandingBalance
			,OD.Amount SubTotalAmount
			,'''' OrderDiscPercent
			,'''' OrderDiscAmount		
			,'''' AmountAfterDisc
			,OH.GSTAmount
			,OH.NettAmount TotalINVAmount
			,ISNULL(OD.Userfloat1,0) DepositPaid
			--,ROW_NUMBER() OVER(PARTITION BY CAST(DO.DeliveryDate AS DATE) ORDER BY DO.DeliveryDate) TotalShipmentNum
			,ISNULL(DOCOunt.DONo,1) TotalShipmentNum
			,I.ItemName ItemDescription1
			,'''' ItemDescription2		
			,'''' ItemDescription3				
			,OD.Remark ItemLineText1
			,'''' ItemLineText2
			,'''' ItemLineText3
			,DO.Remark FooterText1
			,OH.Remark FooterText2
			,ISNULL(REPLACE(RIGHT(DO.TimeSlotFrom, 7), '':00'', '''') + '' - '' + REPLACE(RIGHT(DO.TimeSlotTo, 7), '':00'', ''''), '''') FooterText3
			,'''' FooterText4
			,'''' FooterText5
			,'''' FooterText6
			,'''' FooterText7
			,'''' FooterText8
			,'''' FooterText9
			,'''' FooterText10
	FROM	DeliveryOrder DO
			INNER JOIN DeliveryOrderDetails DOD ON DO.OrderNumber = DOD.DOHDRID 
			INNER JOIN OrderHdr OH ON DO.SalesOrderRefNo = OH.OrderHdrID
			INNER JOIN OrderDet OD ON OD.OrderHdrID = OH.OrderHdrID AND OD.OrderDetID = DOD.OrderDetID 
			INNER JOIN Item I ON I.ItemNo = OD.ItemNo
			INNER JOIN Membership M ON M.MembershipNo = OH.MembershipNo
			INNER JOIN PointOfSale POS ON POS.PointOfSaleID = OH.PointOfSaleID
			INNER JOIN Outlet OU ON OU.OutletName = POS.OutletName
			LEFT OUTER JOIN (
				SELECT  CAST(DO.DeliveryDate AS DATE) DODate
						,COUNT(*) DONo
				FROM	DeliveryOrder DO
				WHERE	DO.Deleted = 0
						AND CAST(DO.DeliveryDate AS DATE) 
							BETWEEN CAST(@StartDate AS DATE) 
							AND CAST(@EndDate AS DATE)
				GROUP BY CAST(DO.DeliveryDate AS DATE)
			) DOCOunt ON DOCOunt.DODate = CAST(DO.DeliveryDate AS DATE)
	WHERE	OH.IsVoided = 0
			AND OD.IsVoided = 0
			AND DO.Deleted = 0
			AND ISNULL(DOD.Deleted, 0) = 0
			AND CAST(DO.DeliveryDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
	ORDER BY DO.DeliveryDate
END'
END


