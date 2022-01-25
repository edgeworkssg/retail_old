IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Print User Barcode')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Print User Barcode','PrintUserBarcode.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Kiosk Staff Function')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Kiosk Staff Function','Kiosk Staff Function','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Kiosk Setting Page')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Kiosk Setting Page','Kiosk Setting Page','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'UpdatePOS')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('UpdatePOS','UpdatePOS','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Item Group Promo')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Item Group Promo','ItemGroupPromo.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Setup Product Promotion')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Setup Product Promotion','SetupProductPromotion.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'VIEW QUOTATIONS')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('VIEW QUOTATIONS','VIEW QUOTATIONS','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'QUOTATION')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('QUOTATION','QUOTATION','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Select Sales Person')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Select Sales Person','Select Sales Person','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Goods Receive Detail Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Goods Receive Detail Report','GRDetailReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Add Mall Layout')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Add Mall Layout','Add Mall Layout','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Mall Layout Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Mall Layout Setup','MallLayoutSetup.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Audit Log Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Audit Log Report','AuditLogReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Add New Tenant')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Add New Tenant','Add New Tenant','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Daily Sales VS Manual Submission Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Daily Sales VS Manual Submission Report','DailySalesVSManualSubmission.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Missing Daily Sales')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Missing Daily Sales','MissingDailySales.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Manual Sales Update')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Manual Sales Update','ManualSalesUpdate.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Manual Sales Submisson')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Manual Sales Submisson','ManualSalesUpdate.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Outlet Transaction Hourly Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Outlet Transaction Hourly Report','OutletTransactionHourlyReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Transaction Hourly Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Transaction Hourly Report','TransactionHourlyReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Daily Sales Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Daily Sales Report','DailySalesReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Outlet Daily Sales')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Outlet Daily Sales','OutletDailySales.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Discount Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Discount Setup','SpecialDiscountSetup.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Item Importer')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Item Importer','ItemImporter.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Purchase Order Approval')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Purchase Order Approval','PurchaseOrderApproval.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Item Supplier and Cost Price Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Item Supplier and Cost Price Setup','ItemSupplierMapScaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

/**IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Access Log Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Access Log Report','AccessLogReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END*/

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Pre Order Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Pre Order Report','PreOrderReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Give Refund')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Give Refund','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Balance Report (With Cost)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Stock Balance Report (With Cost)','StockOnHandReportItemSummary.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Balance Report By Date')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Stock Balance Report By Date','StockOnHandReportByDate.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Product Pre Order Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Product Pre Order Report','ProductPreOrderReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Goods Receive Summary Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Goods Receive Summary Report','GRSummaryReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Currency Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Currency Setup','CurrencySetup.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Monthly Funding Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Monthly Funding Report','MonthlyFundingReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Transaction Discount Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Transaction Discount Report','TransactionDiscountReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Variance Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Stock Variance Report','StockVarianceReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Campaign Promo Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Campaign Promo Report','CampaignPromoReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Appointment Manager')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Appointment Manager','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Monthly Appointment')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Monthly Appointment','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Room Status')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Room Status','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Resource Group Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Resource Group Setup','ResourceGroupScaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Resource Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Resource Setup','ResourceScaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Daily Appointment View')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Daily Appointment View','AppointmentDaily.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Create Appointment')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Create Appointment','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'View Appointment')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('View Appointment','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Personal Sales Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Personal Sales Report','PersonalSalesReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Retail Sales Cash Analysis Summary Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Retail Sales Cash Analysis Summary Report','RSCASR.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Retail Sales Cash Analysis Detail Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Retail Sales Cash Analysis Detail Report','RSCASR2.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Retail Collection Breakdown')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Retail Collection Breakdown','RSCASR3.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Daily Product Sales Analysis Report (Summary)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Daily Product Sales Analysis Report (Summary)','DailyProductSalesAnalysisReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Daily Product Sales Analysis Detail Report(Detail)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Daily Product Sales Analysis Detail Report(Detail)','DPSAR.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Commission Report (Summary)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Commission Report (Summary)','SalesCommissionReportSummary.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Commission Report (Detail)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Commission Report (Detail)','SalesCommissionReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Commission Report (Category Detail)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Commission Report (Category Detail)','SalesCommissionReportBreakdownByCategory.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'View Membership Package')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('View Membership Package','ViewCurrentMembersPackage.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Package Utilization Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Package Utilization Report','PackageUtilizationReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Membership Unused Balance Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Membership Unused Balance Report','MembershipUnusedBalanceReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Product Sales Report Without Redeem')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Product Sales Report Without Redeem','ProductSalesReportWithoutRedeem.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Person Performance Report (NEW)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Person Performance Report (NEW)','SalesPersonPerformance.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Return(Deduction) Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Return(Deduction) Report','SalesReturnReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Assistant''s Sales Report By Day')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Assistant''s Sales Report By Day','LineCommissionItem.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Attributes Sales Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Attributes Sales Report','SumarizedProductSalesReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Guest Book Compulsory Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Guest Book Compulsory Setup','GuestbookCompulsoryScaffolds.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Customer Attendance Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Customer Attendance Report','CustomerAttendanceReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'MEMBERS GUESTBOOK')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('MEMBERS GUESTBOOK','MEMBERS GUESTBOOK','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Generate Salesman App JSON File')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Generate Salesman App JSON File','GenerateJSON.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Daily Pre-Order Sales Summary Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Daily Pre-Order Sales Summary Report','DailyPreOrderSalesSummaryReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Accept Close Counter')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Accept Close Counter','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Take Item Miss-out Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Stock Take Item Miss-out Report','StockTakeMissOutReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Transaction Detail Report (With Payment Type)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Transaction Detail Report (With Payment Type)','TransactionDetailReportWithPaymentType.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Outlet Daily Category Sales')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Outlet Daily Category Sales','OutletDailyCategorySales.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Add New Item')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Add New Item',NULL,'UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'New Product Setup')>0)
BEGIN 
UPDATE UserPrivilege SET FormName = 'NewProductMaster.aspx'  WHERE PrivilegeName = 'New Product Setup' AND (FormName = '' OR FormName IS NULL)
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Commission Report (Quantity)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Commission Report (Quantity)','CommByQty.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Commission by Percentage Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Commission by Percentage Setup','CommissionByPercentage.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Commission by Quantity Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Commission by Quantity Setup','CommissionByQty.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Item Attributes Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Item Attributes Setup','ItemAttributesScaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Average Bill Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Average Bill Report','AverageBillReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Performance Log Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Performance Log Report','PerformanceLogReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Performance Log Summary Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Performance Log Summary Report','PerformanceLogSummaryReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Item Low Qty Importer')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Item Low Qty Importer','LowQtyImporter.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Customer Rating Settings')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Customer Rating Settings','CustomerRatingConfiguration.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Customer Rating Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Customer Rating Report','CustomerRatingReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Customer Purchase Detail Summary')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Customer Purchase Detail Summary','MembershipTransactionReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Commission Report (By Percentage)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Commission Report (By Percentage)','SalesCommissionByPercentageReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Aggregated Sales Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Aggregated Sales Report','AggregatedSalesReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'SAP Customer Code Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('SAP Customer Code Setup','SAPCustomerCodeScaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Voucher Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Voucher Setup','VoucherHeaderScaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Roll Forward Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Roll Forward Report','RollForwardReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Supplier List Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Supplier List Report','SupplierListReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Item Supplier Map Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Item Supplier Map Report','ItemSupplierMapReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Outlet Category Sales')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Outlet Category Sales','OutletCategorySalesReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Pre Accrual List Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Pre Accrual List Report','PreAccrualListReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Aging Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Aging Report','AgingReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Accounting Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Accounting Report','AccountingReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'New Commission Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('New Commission Setup','NewCommission.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'New Commission Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('New Commission Report','NewCommissionReport.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Shelf Tag Printing')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Shelf Tag Printing','ShelfTagPrinting.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'BARCODE PRINTER')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('BARCODE PRINTER','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Purchase Order')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Purchase Order','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'View Purchase Order')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('View Purchase Order','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Formal Invoice')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Formal Invoice','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Detail Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1) 
VALUES ('Sales Detail Report','SalesDetailReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS') 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Consignment Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1) 
VALUES ('Consignment Report','ConsignmentReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS') 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Sales Commission Summary')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted) 
VALUES ('Sales Commission Summary','SalesCommissionSummary.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Refund Transaction Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1) 
VALUES ('Refund Transaction Report','RefundTransactionReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS') 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Membership Importer')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Membership Importer','MembershipImporter.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'POS Exception Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('POS Exception Report','AccessLogReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Setup Inventory Location Group')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag4) 
VALUES ('Setup Inventory Location Group','InventoryLocationGroupScaffolds.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0, 1, 1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Allow Change Inventory Location')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Allow Change Inventory Location','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Goods Ordering')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Goods Ordering','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Order Approval')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Order Approval','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Outlet Optimal Stock')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Outlet Optimal Stock','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Recipe Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Recipe Setup','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'UOM Conversion Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('UOM Conversion Setup','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Production')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Production','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'ADD UOM CONVERSION')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('ADD UOM CONVERSION','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'STOCK RETURN')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('STOCK RETURN','STOCK RETURN','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'13. POS-INVENTORY', 0) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Transfer Variance Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1) 
VALUES ('Stock Transfer Variance Report','StockTransferVarianceReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'04. EQUIPWEB-INVENTORY REPORT') 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Return Variance Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1) 
VALUES ('Stock Return Variance Report','StockReturnVarianceReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'04. EQUIPWEB-INVENTORY REPORT') 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Order Variance Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1) 
VALUES ('Stock Order Variance Report','StockOrderVarianceReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'04. EQUIPWEB-INVENTORY REPORT') 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Void Transaction Report Detail')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Void Transaction Report Detail','TransactionDetailVoidReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Transfer Approval')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1) 
VALUES ('Transfer Approval','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'No Sales')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('No Sales','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'11. POS-SALES', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Good Receive Print Out')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Good Receive Print Out','GRPrintout.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Order Approval Print Out')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Order Approval Print Out','OrderPrintout.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Transfer Print Out')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Stock Transfer Print Out','StockTransferPrintout.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Return Print Out')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Stock Return Print Out','ReturnPrintout.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Tax Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Tax Report','TaxReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Transfer Invoice Print Out')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Stock Transfer Invoice Print Out','TransferOrderPrintout.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Stock Transfer Return Print Out')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Stock Transfer Return Print Out','TransferReturnPrintout.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'16. WEB ORDER', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Driver Pick List Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Driver Pick List Report','DeliveryByDayReport.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Membership Account Statement Report')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Membership Account Statement Report','MembershipAccountStatement.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'08. EQUIPWEB-MEMBERSHIP', 0, 1 ,1 ,1) 
END


IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Company Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Company Setup','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Recipe Importer')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Recipe Importer','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'IMPORT RECIPES')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('IMPORT RECIPES','','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Supplier Setup')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Supplier Setup','Supplier_Scaffold.aspx','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'10. EQUIPWEB-SYSTEM SETUP', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Membership Unused Balance Report (By Date)')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1, userflag1, userflag2, userflag3, userflag4) 
VALUES ('Membership Unused Balance Report (By Date)','MembershipBalancePointsByDate.rpt','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'01. EQUIPWEB-REPORTS', 0, 1 ,1 ,1) 
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Allow Do Past Void')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1,userflag1, userflag2, userflag3, userflag4) 
VALUES ('Allow Do Past Void','VIEW SALES','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'11. POS-SALES',0, 1 ,1 ,1)
END

IF((SELECT COUNT(*) FROM UserPrivilege WHERE PrivilegeName = 'Change Past Payment Type')<=0)
BEGIN 
INSERT into UserPrivilege (PrivilegeName, FormName, CreatedBy, CreatedOn,
ModifiedBy, ModifiedOn, Deleted, userfld1,userflag1, userflag2, userflag3, userflag4) 
VALUES ('Change Past Payment Type','VIEW SALES','UPGRADE',GETDATE(),'UPGRADE',GETDATE(),0,'11. POS-SALES',0, 1 ,1 ,1)
END

