using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
namespace PowerPOS
{
	#region Tables Struct
	public partial struct Tables
	{
		
		public static string AccessLog = @"AccessLog";
        
		public static string ActivePayment = @"ActivePayment";
        
		public static string AlternateBarcode = @"AlternateBarcode";
        
		public static string AlternateBarcodeTombstone = @"AlternateBarcode_Tombstone";
        
		public static string AppliedPromo = @"AppliedPromo";
        
		public static string AppliedPromoTombstone = @"AppliedPromo_Tombstone";
        
		public static string Appointment = @"Appointment";
        
		public static string AppointmentItem = @"AppointmentItem";
        
		public static string AppPendingProcess = @"AppPendingProcess";
        
		public static string AppSetting = @"AppSetting";
        
		public static string AppSettingTombstone = @"AppSetting_Tombstone";
        
		public static string AttachedParticular = @"AttachedParticular";
        
		public static string AttendanceSheet = @"AttendanceSheet";
        
		public static string AttendanceSheetTombstone = @"AttendanceSheet_Tombstone";
        
		public static string AttributesLabel = @"AttributesLabel";
        
		public static string AuditLog = @"AuditLog";
        
		public static string AutoCompleteName = @"AutoCompleteNames";
        
		public static string BillInfo = @"BillInfo";
        
		public static string Building = @"Building";
        
		public static string Camera = @"Camera";
        
		public static string CashRecording = @"CashRecording";
        
		public static string CashRecordingType = @"CashRecordingType";
        
		public static string Category = @"Category";
        
		public static string Commission = @"Commission";
        
		public static string CommissionBasedOnPercentage = @"CommissionBasedOnPercentage";
        
		public static string CommissionBasedOnQty = @"CommissionBasedOnQty";
        
		public static string CommissionDetBy = @"CommissionDetBy";
        
		public static string CommissionDetFor = @"CommissionDetFor";
        
		public static string CommissionHdr = @"CommissionHdr";
        
		public static string CommissionStructure = @"CommissionStructure";
        
		public static string Company = @"Company";
        
		public static string CounterCloseDet = @"CounterCloseDet";
        
		public static string CounterCloseLog = @"CounterCloseLog";
        
		public static string Course = @"Course";
        
		public static string CourseType = @"CourseType";
        
		public static string Currency = @"Currencies";
        
		public static string CustomerPricing = @"CustomerPricing";
        
		public static string Dashboard = @"Dashboard";
        
		public static string DeliveryPersonnel = @"Delivery_Personnel";
        
		public static string DeliveryOrder = @"DeliveryOrder";
        
		public static string DeliveryOrderDetail = @"DeliveryOrderDetails";
        
		public static string Department = @"Department";
        
		public static string DwHourlyPayment = @"DW_HourlyPayment";
        
		public static string DwHourlyProductSale = @"DW_HourlyProductSales";
        
		public static string DwHourlySale = @"DW_HourlySales";
        
		public static string DwRegenerateDate = @"DW_RegenerateDate";
        
		public static string EmailNotification = @"EmailNotification";
        
		public static string EventAttendance = @"EventAttendance";
        
		public static string EventItemMap = @"EventItemMap";
        
		public static string EventLocationMap = @"EventLocationMap";
        
		public static string ExchangeLog = @"ExchangeLog";
        
		public static string EZlinkMessage = @"EZlinkMessage";
        
		public static string EZLinkMessageParameter = @"EZLinkMessageParameter";
        
		public static string EZLinkMessageParameterMap = @"EZLinkMessageParameterMap";
        
		public static string EZLinkMsgLog = @"EZLinkMsgLog";
        
		public static string EZLinkResponseCode = @"EZLinkResponseCode";
        
		public static string EZLinkUnCfmLog = @"EZLinkUnCfmLog";
        
		public static string Family = @"Family";
        
		public static string FamilyGroupMap = @"FamilyGroupMap";
        
		public static string FeedBackLog = @"FeedBackLog";
        
		public static string FeedBackMsg = @"FeedBackMsg";
        
		public static string FileAttachment = @"FileAttachment";
        
		public static string GroupUserPrivilege = @"GroupUserPrivilege";
        
		public static string Gst = @"GST";
        
		public static string GuestBook = @"GuestBook";
        
		public static string GuestBookCompulsory = @"GuestBookCompulsory";
        
		public static string HoldTransaction = @"HoldTransaction";
        
		public static string Installment = @"Installment";
        
		public static string InstallmentDetail = @"InstallmentDetail";
        
		public static string InventoryDet = @"InventoryDet";
        
		public static string InventoryHdr = @"InventoryHdr";
        
		public static string InventoryLocation = @"InventoryLocation";
        
		public static string InventoryLocationGroup = @"InventoryLocationGroup";
        
		public static string InventoryStockOutReason = @"InventoryStockOutReason";
        
		public static string InventoryTransferDiscrepancy = @"InventoryTransferDiscrepancy";
        
		public static string InventoryUpdateLog = @"InventoryUpdateLog";
        
		public static string InvoiceSequenceNo = @"InvoiceSequenceNo";
        
		public static string Item = @"Item";
        
		public static string ItemAttribute = @"ItemAttributes";
        
		public static string ItemBaseLevel = @"ItemBaseLevel";
        
		public static string ItemBatchSummary = @"ItemBatchSummary";
        
		public static string ItemBatchSummaryArchive = @"ItemBatchSummaryArchive";
        
		public static string ItemCookDetail = @"ItemCookDetail";
        
		public static string ItemCookHistory = @"ItemCookHistory";
        
		public static string ItemCostPrice = @"ItemCostPrice";
        
		public static string ItemCostSummary = @"ItemCostSummary";
        
		public static string ItemDepartment = @"ItemDepartment";
        
		public static string ItemFuturePrice = @"ItemFuturePrice";
        
		public static string ItemGroup = @"ItemGroup";
        
		public static string ItemGroupMap = @"ItemGroupMap";
        
		public static string ItemMemberPrice = @"ItemMemberPrice";
        
		public static string ItemQuantityTrigger = @"ItemQuantityTrigger";
        
		public static string ItemsNew = @"ItemsNew";
        
		public static string ItemSummary = @"ItemSummary";
        
		public static string ItemSummaryGroup = @"ItemSummaryGroup";
        
		public static string ItemSupplierMap = @"ItemSupplierMap";
        
		public static string ItemTagStatusDetail = @"ItemTagStatusDetail";
        
		public static string ItemTagSummary = @"ItemTagSummary";
        
		public static string ItemWeight = @"ItemWeight";
        
		public static string LanguageSetting = @"LANGUAGE_SETTINGS";
        
		public static string LineInfo = @"LineInfo";
        
		public static string LocationTransfer = @"LocationTransfer";
        
		public static string LoginActivity = @"LoginActivity";
        
		public static string ManualSalesUpdate = @"ManualSalesUpdate";
        
		public static string Membership = @"Membership";
        
		public static string MembershipAttendance = @"MembershipAttendance";
        
		public static string MembershipCustomField = @"MembershipCustomFields";
        
		public static string MembershipGroup = @"MembershipGroup";
        
		public static string MembershipPoint = @"MembershipPoints";
        
		public static string MembershipRemark = @"MembershipRemark";
        
		public static string MembershipRemarkCategory = @"MembershipRemarkCategory";
        
		public static string MembershipRenewal = @"MembershipRenewal";
        
		public static string MembershipTap = @"MembershipTap";
        
		public static string MembershipTapsLog = @"MembershipTapsLog";
        
		public static string MembershipUpgradeLog = @"MembershipUpgradeLog";
        
		public static string OrderDet = @"OrderDet";
        
		public static string OrderDetTransfer = @"OrderDetTransfer";
        
		public static string OrderDetUOMConversion = @"OrderDetUOMConversion";
        
		public static string OrderHdr = @"OrderHdr";
        
		public static string OrderTransfer = @"OrderTransfer";
        
		public static string Outlet = @"Outlet";
        
		public static string OutletCustomerPricing = @"OutletCustomerPricing";
        
		public static string OutletGroup = @"OutletGroup";
        
		public static string OutletGroupItemMap = @"OutletGroupItemMap";
        
		public static string PackageDet = @"PackageDet";
        
		public static string PackageHdr = @"PackageHdr";
        
		public static string PackageRedemptionLog = @"PackageRedemptionLog";
        
		public static string PaymentTerm = @"PaymentTerm";
        
		public static string PerformanceLog = @"PerformanceLog";
        
		public static string PerformanceLogSummary = @"PerformanceLogSummary";
        
		public static string PointAllocationLog = @"PointAllocationLog";
        
		public static string PointOfSale = @"PointOfSale";
        
		public static string PointTempLog = @"PointTempLog";
        
		public static string PostalCodeDB = @"PostalCodeDB";
        
		public static string PowerLog = @"PowerLog";
        
		public static string PreOrderRecord = @"PreOrderRecord";
        
		public static string PreOrderSchedule = @"PreOrderSchedule";
        
		public static string PriceScheme = @"PriceScheme";
        
		public static string Project = @"Project";
        
		public static string PromoCampaignDet = @"PromoCampaignDet";
        
		public static string PromoCampaignHdr = @"PromoCampaignHdr";
        
		public static string PromoCode = @"PromoCode";
        
		public static string PromoDaysMap = @"PromoDaysMap";
        
		public static string PromoDiscountTier = @"PromoDiscountTier";
        
		public static string PromoLocationMap = @"PromoLocationMap";
        
		public static string PromoMembershipMap = @"PromoMembershipMap";
        
		public static string PromoOutlet = @"PromoOutlet";
        
		public static string PurchaseOrderDet = @"PurchaseOrderDet";
        
		public static string PurchaseOrderDetail = @"PurchaseOrderDetail";
        
		public static string PurchaseOrderHdr = @"PurchaseOrderHdr";
        
		public static string PurchaseOrderHeader = @"PurchaseOrderHeader";
        
		public static string QuickAccessButton = @"QuickAccessButton";
        
		public static string QuickAccessCategory = @"QuickAccessCategory";
        
		public static string QuickAccessGroup = @"QuickAccessGroup";
        
		public static string QuickAccessGroupMap = @"QuickAccessGroupMap";
        
		public static string QuotationDet = @"QuotationDet";
        
		public static string QuotationHdr = @"QuotationHdr";
        
		public static string Rating = @"Rating";
        
		public static string RatingFeedback = @"RatingFeedback";
        
		public static string RatingMaster = @"RatingMaster";
        
		public static string ReceiptDet = @"ReceiptDet";
        
		public static string ReceiptHdr = @"ReceiptHdr";
        
		public static string ReceiptSetting = @"ReceiptSetting";
        
		public static string RecipeDetail = @"RecipeDetail";
        
		public static string RecipeHeader = @"RecipeHeader";
        
		public static string RecordDatum = @"RecordData";
        
		public static string Recurrence = @"Recurrence";
        
		public static string RedeemLog = @"RedeemLog";
        
		public static string RedemptionItem = @"RedemptionItem";
        
		public static string RedemptionLog = @"RedemptionLog";
        
		public static string Resource = @"Resource";
        
		public static string ResourceGroup = @"ResourceGroup";
        
		public static string RetailerLevel = @"RetailerLevel";
        
		public static string Room = @"Rooms";
        
		public static string RoomsTimeSlot = @"Rooms_Time_Slots";
        
		public static string SalesCommissionDetail = @"SalesCommissionDetails";
        
		public static string SalesCommissionDetailsCommission = @"SalesCommissionDetails_Commission";
        
		public static string SalesCommissionDetailsDeduction = @"SalesCommissionDetails_Deduction";
        
		public static string SalesCommissionHistory = @"SalesCommissionHistory";
        
		public static string SalesCommissionRecord = @"SalesCommissionRecord";
        
		public static string SalesCommissionSummary = @"SalesCommissionSummary";
        
		public static string SalesGroup = @"SalesGroup";
        
		public static string SalesOrderMapping = @"SalesOrderMapping";
        
		public static string SalesPerson = @"SalesPerson";
        
		public static string SAPCustomerCode = @"SAPCustomerCode";
        
		public static string SavedClosing = @"SavedClosing";
        
		public static string SavedFile = @"SavedFiles";
        
		public static string SaveItem = @"SaveItem";
        
		public static string ScheduledDiscount = @"ScheduledDiscount";
        
		public static string ServerQuickRef = @"ServerQuickRef";
        
		public static string ServicingAppointment = @"ServicingAppointment";
        
		public static string Setting = @"Setting";
        
		public static string SpecialActivityLog = @"SpecialActivityLog";
        
		public static string SpecialDiscountDetail = @"SpecialDiscountDetail";
        
		public static string SpecialDiscount = @"SpecialDiscounts";
        
		public static string SpecialEvent = @"SpecialEvent";
        
		public static string StaffAssistLog = @"StaffAssistLog";
        
		public static string StockStaging = @"StockStaging";
        
		public static string StockTake = @"StockTake";
        
		public static string StockTransferDet = @"StockTransferDet";
        
		public static string StockTransferHdr = @"StockTransferHdr";
        
		public static string Supplier = @"Supplier";
        
		public static string SupplierItemMap = @"SupplierItemMap";
        
		public static string SyncLog = @"SyncLog";
        
		public static string SyncRequest = @"SyncRequest";
        
		public static string TestTable = @"TestTable";
        
		public static string TestTable2 = @"TestTable2";
        
		public static string TextLanguage = @"TEXT_LANGUAGE";
        
		public static string TimeSlot = @"Time_Slots";
        
		public static string TmpItem = @"tmpItems";
        
		public static string UOMConversionDet = @"UOMConversionDet";
        
		public static string UOMConversionHdr = @"UOMConversionHdr";
        
		public static string UserGroup = @"UserGroup";
        
		public static string UserMst = @"UserMst";
        
		public static string UserMstTombstone = @"UserMst_Tombstone";
        
		public static string UserPrivilege = @"UserPrivilege";
        
		public static string VoidLog = @"VoidLog";
        
		public static string VoucherHeader = @"VoucherHeader";
        
		public static string Voucher = @"Vouchers";
        
		public static string VoucherStatus = @"VoucherStatus";
        
		public static string WarningMsg = @"WarningMsg";
        
		public static string Warranty = @"Warranty";
        
		public static string Word = @"Word";
        
		public static string Z2ClosingLog = @"Z2ClosingLog";
        
	}
	#endregion
    #region Schemas
    public partial class Schemas {
		
		public static TableSchema.Table AccessLog{
            get { return DataService.GetSchema("AccessLog","PowerPOS"); }
		}
        
		public static TableSchema.Table ActivePayment{
            get { return DataService.GetSchema("ActivePayment","PowerPOS"); }
		}
        
		public static TableSchema.Table AlternateBarcode{
            get { return DataService.GetSchema("AlternateBarcode","PowerPOS"); }
		}
        
		public static TableSchema.Table AlternateBarcodeTombstone{
            get { return DataService.GetSchema("AlternateBarcode_Tombstone","PowerPOS"); }
		}
        
		public static TableSchema.Table AppliedPromo{
            get { return DataService.GetSchema("AppliedPromo","PowerPOS"); }
		}
        
		public static TableSchema.Table AppliedPromoTombstone{
            get { return DataService.GetSchema("AppliedPromo_Tombstone","PowerPOS"); }
		}
        
		public static TableSchema.Table Appointment{
            get { return DataService.GetSchema("Appointment","PowerPOS"); }
		}
        
		public static TableSchema.Table AppointmentItem{
            get { return DataService.GetSchema("AppointmentItem","PowerPOS"); }
		}
        
		public static TableSchema.Table AppPendingProcess{
            get { return DataService.GetSchema("AppPendingProcess","PowerPOS"); }
		}
        
		public static TableSchema.Table AppSetting{
            get { return DataService.GetSchema("AppSetting","PowerPOS"); }
		}
        
		public static TableSchema.Table AppSettingTombstone{
            get { return DataService.GetSchema("AppSetting_Tombstone","PowerPOS"); }
		}
        
		public static TableSchema.Table AttachedParticular{
            get { return DataService.GetSchema("AttachedParticular","PowerPOS"); }
		}
        
		public static TableSchema.Table AttendanceSheet{
            get { return DataService.GetSchema("AttendanceSheet","PowerPOS"); }
		}
        
		public static TableSchema.Table AttendanceSheetTombstone{
            get { return DataService.GetSchema("AttendanceSheet_Tombstone","PowerPOS"); }
		}
        
		public static TableSchema.Table AttributesLabel{
            get { return DataService.GetSchema("AttributesLabel","PowerPOS"); }
		}
        
		public static TableSchema.Table AuditLog{
            get { return DataService.GetSchema("AuditLog","PowerPOS"); }
		}
        
		public static TableSchema.Table AutoCompleteName{
            get { return DataService.GetSchema("AutoCompleteNames","PowerPOS"); }
		}
        
		public static TableSchema.Table BillInfo{
            get { return DataService.GetSchema("BillInfo","PowerPOS"); }
		}
        
		public static TableSchema.Table Building{
            get { return DataService.GetSchema("Building","PowerPOS"); }
		}
        
		public static TableSchema.Table Camera{
            get { return DataService.GetSchema("Camera","PowerPOS"); }
		}
        
		public static TableSchema.Table CashRecording{
            get { return DataService.GetSchema("CashRecording","PowerPOS"); }
		}
        
		public static TableSchema.Table CashRecordingType{
            get { return DataService.GetSchema("CashRecordingType","PowerPOS"); }
		}
        
		public static TableSchema.Table Category{
            get { return DataService.GetSchema("Category","PowerPOS"); }
		}
        
		public static TableSchema.Table Commission{
            get { return DataService.GetSchema("Commission","PowerPOS"); }
		}
        
		public static TableSchema.Table CommissionBasedOnPercentage{
            get { return DataService.GetSchema("CommissionBasedOnPercentage","PowerPOS"); }
		}
        
		public static TableSchema.Table CommissionBasedOnQty{
            get { return DataService.GetSchema("CommissionBasedOnQty","PowerPOS"); }
		}
        
		public static TableSchema.Table CommissionDetBy{
            get { return DataService.GetSchema("CommissionDetBy","PowerPOS"); }
		}
        
		public static TableSchema.Table CommissionDetFor{
            get { return DataService.GetSchema("CommissionDetFor","PowerPOS"); }
		}
        
		public static TableSchema.Table CommissionHdr{
            get { return DataService.GetSchema("CommissionHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table CommissionStructure{
            get { return DataService.GetSchema("CommissionStructure","PowerPOS"); }
		}
        
		public static TableSchema.Table Company{
            get { return DataService.GetSchema("Company","PowerPOS"); }
		}
        
		public static TableSchema.Table CounterCloseDet{
            get { return DataService.GetSchema("CounterCloseDet","PowerPOS"); }
		}
        
		public static TableSchema.Table CounterCloseLog{
            get { return DataService.GetSchema("CounterCloseLog","PowerPOS"); }
		}
        
		public static TableSchema.Table Course{
            get { return DataService.GetSchema("Course","PowerPOS"); }
		}
        
		public static TableSchema.Table CourseType{
            get { return DataService.GetSchema("CourseType","PowerPOS"); }
		}
        
		public static TableSchema.Table Currency{
            get { return DataService.GetSchema("Currencies","PowerPOS"); }
		}
        
		public static TableSchema.Table CustomerPricing{
            get { return DataService.GetSchema("CustomerPricing","PowerPOS"); }
		}
        
		public static TableSchema.Table Dashboard{
            get { return DataService.GetSchema("Dashboard","PowerPOS"); }
		}
        
		public static TableSchema.Table DeliveryPersonnel{
            get { return DataService.GetSchema("Delivery_Personnel","PowerPOS"); }
		}
        
		public static TableSchema.Table DeliveryOrder{
            get { return DataService.GetSchema("DeliveryOrder","PowerPOS"); }
		}
        
		public static TableSchema.Table DeliveryOrderDetail{
            get { return DataService.GetSchema("DeliveryOrderDetails","PowerPOS"); }
		}
        
		public static TableSchema.Table Department{
            get { return DataService.GetSchema("Department","PowerPOS"); }
		}
        
		public static TableSchema.Table DwHourlyPayment{
            get { return DataService.GetSchema("DW_HourlyPayment","PowerPOS"); }
		}
        
		public static TableSchema.Table DwHourlyProductSale{
            get { return DataService.GetSchema("DW_HourlyProductSales","PowerPOS"); }
		}
        
		public static TableSchema.Table DwHourlySale{
            get { return DataService.GetSchema("DW_HourlySales","PowerPOS"); }
		}
        
		public static TableSchema.Table DwRegenerateDate{
            get { return DataService.GetSchema("DW_RegenerateDate","PowerPOS"); }
		}
        
		public static TableSchema.Table EmailNotification{
            get { return DataService.GetSchema("EmailNotification","PowerPOS"); }
		}
        
		public static TableSchema.Table EventAttendance{
            get { return DataService.GetSchema("EventAttendance","PowerPOS"); }
		}
        
		public static TableSchema.Table EventItemMap{
            get { return DataService.GetSchema("EventItemMap","PowerPOS"); }
		}
        
		public static TableSchema.Table EventLocationMap{
            get { return DataService.GetSchema("EventLocationMap","PowerPOS"); }
		}
        
		public static TableSchema.Table ExchangeLog{
            get { return DataService.GetSchema("ExchangeLog","PowerPOS"); }
		}
        
		public static TableSchema.Table EZlinkMessage{
            get { return DataService.GetSchema("EZlinkMessage","PowerPOS"); }
		}
        
		public static TableSchema.Table EZLinkMessageParameter{
            get { return DataService.GetSchema("EZLinkMessageParameter","PowerPOS"); }
		}
        
		public static TableSchema.Table EZLinkMessageParameterMap{
            get { return DataService.GetSchema("EZLinkMessageParameterMap","PowerPOS"); }
		}
        
		public static TableSchema.Table EZLinkMsgLog{
            get { return DataService.GetSchema("EZLinkMsgLog","PowerPOS"); }
		}
        
		public static TableSchema.Table EZLinkResponseCode{
            get { return DataService.GetSchema("EZLinkResponseCode","PowerPOS"); }
		}
        
		public static TableSchema.Table EZLinkUnCfmLog{
            get { return DataService.GetSchema("EZLinkUnCfmLog","PowerPOS"); }
		}
        
		public static TableSchema.Table Family{
            get { return DataService.GetSchema("Family","PowerPOS"); }
		}
        
		public static TableSchema.Table FamilyGroupMap{
            get { return DataService.GetSchema("FamilyGroupMap","PowerPOS"); }
		}
        
		public static TableSchema.Table FeedBackLog{
            get { return DataService.GetSchema("FeedBackLog","PowerPOS"); }
		}
        
		public static TableSchema.Table FeedBackMsg{
            get { return DataService.GetSchema("FeedBackMsg","PowerPOS"); }
		}
        
		public static TableSchema.Table FileAttachment{
            get { return DataService.GetSchema("FileAttachment","PowerPOS"); }
		}
        
		public static TableSchema.Table GroupUserPrivilege{
            get { return DataService.GetSchema("GroupUserPrivilege","PowerPOS"); }
		}
        
		public static TableSchema.Table Gst{
            get { return DataService.GetSchema("GST","PowerPOS"); }
		}
        
		public static TableSchema.Table GuestBook{
            get { return DataService.GetSchema("GuestBook","PowerPOS"); }
		}
        
		public static TableSchema.Table GuestBookCompulsory{
            get { return DataService.GetSchema("GuestBookCompulsory","PowerPOS"); }
		}
        
		public static TableSchema.Table HoldTransaction{
            get { return DataService.GetSchema("HoldTransaction","PowerPOS"); }
		}
        
		public static TableSchema.Table Installment{
            get { return DataService.GetSchema("Installment","PowerPOS"); }
		}
        
		public static TableSchema.Table InstallmentDetail{
            get { return DataService.GetSchema("InstallmentDetail","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryDet{
            get { return DataService.GetSchema("InventoryDet","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryHdr{
            get { return DataService.GetSchema("InventoryHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryLocation{
            get { return DataService.GetSchema("InventoryLocation","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryLocationGroup{
            get { return DataService.GetSchema("InventoryLocationGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryStockOutReason{
            get { return DataService.GetSchema("InventoryStockOutReason","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryTransferDiscrepancy{
            get { return DataService.GetSchema("InventoryTransferDiscrepancy","PowerPOS"); }
		}
        
		public static TableSchema.Table InventoryUpdateLog{
            get { return DataService.GetSchema("InventoryUpdateLog","PowerPOS"); }
		}
        
		public static TableSchema.Table InvoiceSequenceNo{
            get { return DataService.GetSchema("InvoiceSequenceNo","PowerPOS"); }
		}
        
		public static TableSchema.Table Item{
            get { return DataService.GetSchema("Item","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemAttribute{
            get { return DataService.GetSchema("ItemAttributes","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemBaseLevel{
            get { return DataService.GetSchema("ItemBaseLevel","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemBatchSummary{
            get { return DataService.GetSchema("ItemBatchSummary","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemBatchSummaryArchive{
            get { return DataService.GetSchema("ItemBatchSummaryArchive","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemCookDetail{
            get { return DataService.GetSchema("ItemCookDetail","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemCookHistory{
            get { return DataService.GetSchema("ItemCookHistory","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemCostPrice{
            get { return DataService.GetSchema("ItemCostPrice","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemCostSummary{
            get { return DataService.GetSchema("ItemCostSummary","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemDepartment{
            get { return DataService.GetSchema("ItemDepartment","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemFuturePrice{
            get { return DataService.GetSchema("ItemFuturePrice","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemGroup{
            get { return DataService.GetSchema("ItemGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemGroupMap{
            get { return DataService.GetSchema("ItemGroupMap","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemMemberPrice{
            get { return DataService.GetSchema("ItemMemberPrice","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemQuantityTrigger{
            get { return DataService.GetSchema("ItemQuantityTrigger","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemsNew{
            get { return DataService.GetSchema("ItemsNew","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemSummary{
            get { return DataService.GetSchema("ItemSummary","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemSummaryGroup{
            get { return DataService.GetSchema("ItemSummaryGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemSupplierMap{
            get { return DataService.GetSchema("ItemSupplierMap","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemTagStatusDetail{
            get { return DataService.GetSchema("ItemTagStatusDetail","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemTagSummary{
            get { return DataService.GetSchema("ItemTagSummary","PowerPOS"); }
		}
        
		public static TableSchema.Table ItemWeight{
            get { return DataService.GetSchema("ItemWeight","PowerPOS"); }
		}
        
		public static TableSchema.Table LanguageSetting{
            get { return DataService.GetSchema("LANGUAGE_SETTINGS","PowerPOS"); }
		}
        
		public static TableSchema.Table LineInfo{
            get { return DataService.GetSchema("LineInfo","PowerPOS"); }
		}
        
		public static TableSchema.Table LocationTransfer{
            get { return DataService.GetSchema("LocationTransfer","PowerPOS"); }
		}
        
		public static TableSchema.Table LoginActivity{
            get { return DataService.GetSchema("LoginActivity","PowerPOS"); }
		}
        
		public static TableSchema.Table ManualSalesUpdate{
            get { return DataService.GetSchema("ManualSalesUpdate","PowerPOS"); }
		}
        
		public static TableSchema.Table Membership{
            get { return DataService.GetSchema("Membership","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipAttendance{
            get { return DataService.GetSchema("MembershipAttendance","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipCustomField{
            get { return DataService.GetSchema("MembershipCustomFields","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipGroup{
            get { return DataService.GetSchema("MembershipGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipPoint{
            get { return DataService.GetSchema("MembershipPoints","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipRemark{
            get { return DataService.GetSchema("MembershipRemark","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipRemarkCategory{
            get { return DataService.GetSchema("MembershipRemarkCategory","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipRenewal{
            get { return DataService.GetSchema("MembershipRenewal","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipTap{
            get { return DataService.GetSchema("MembershipTap","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipTapsLog{
            get { return DataService.GetSchema("MembershipTapsLog","PowerPOS"); }
		}
        
		public static TableSchema.Table MembershipUpgradeLog{
            get { return DataService.GetSchema("MembershipUpgradeLog","PowerPOS"); }
		}
        
		public static TableSchema.Table OrderDet{
            get { return DataService.GetSchema("OrderDet","PowerPOS"); }
		}
        
		public static TableSchema.Table OrderDetTransfer{
            get { return DataService.GetSchema("OrderDetTransfer","PowerPOS"); }
		}
        
		public static TableSchema.Table OrderDetUOMConversion{
            get { return DataService.GetSchema("OrderDetUOMConversion","PowerPOS"); }
		}
        
		public static TableSchema.Table OrderHdr{
            get { return DataService.GetSchema("OrderHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table OrderTransfer{
            get { return DataService.GetSchema("OrderTransfer","PowerPOS"); }
		}
        
		public static TableSchema.Table Outlet{
            get { return DataService.GetSchema("Outlet","PowerPOS"); }
		}
        
		public static TableSchema.Table OutletCustomerPricing{
            get { return DataService.GetSchema("OutletCustomerPricing","PowerPOS"); }
		}
        
		public static TableSchema.Table OutletGroup{
            get { return DataService.GetSchema("OutletGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table OutletGroupItemMap{
            get { return DataService.GetSchema("OutletGroupItemMap","PowerPOS"); }
		}
        
		public static TableSchema.Table PackageDet{
            get { return DataService.GetSchema("PackageDet","PowerPOS"); }
		}
        
		public static TableSchema.Table PackageHdr{
            get { return DataService.GetSchema("PackageHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table PackageRedemptionLog{
            get { return DataService.GetSchema("PackageRedemptionLog","PowerPOS"); }
		}
        
		public static TableSchema.Table PaymentTerm{
            get { return DataService.GetSchema("PaymentTerm","PowerPOS"); }
		}
        
		public static TableSchema.Table PerformanceLog{
            get { return DataService.GetSchema("PerformanceLog","PowerPOS"); }
		}
        
		public static TableSchema.Table PerformanceLogSummary{
            get { return DataService.GetSchema("PerformanceLogSummary","PowerPOS"); }
		}
        
		public static TableSchema.Table PointAllocationLog{
            get { return DataService.GetSchema("PointAllocationLog","PowerPOS"); }
		}
        
		public static TableSchema.Table PointOfSale{
            get { return DataService.GetSchema("PointOfSale","PowerPOS"); }
		}
        
		public static TableSchema.Table PointTempLog{
            get { return DataService.GetSchema("PointTempLog","PowerPOS"); }
		}
        
		public static TableSchema.Table PostalCodeDB{
            get { return DataService.GetSchema("PostalCodeDB","PowerPOS"); }
		}
        
		public static TableSchema.Table PowerLog{
            get { return DataService.GetSchema("PowerLog","PowerPOS"); }
		}
        
		public static TableSchema.Table PreOrderRecord{
            get { return DataService.GetSchema("PreOrderRecord","PowerPOS"); }
		}
        
		public static TableSchema.Table PreOrderSchedule{
            get { return DataService.GetSchema("PreOrderSchedule","PowerPOS"); }
		}
        
		public static TableSchema.Table PriceScheme{
            get { return DataService.GetSchema("PriceScheme","PowerPOS"); }
		}
        
		public static TableSchema.Table Project{
            get { return DataService.GetSchema("Project","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoCampaignDet{
            get { return DataService.GetSchema("PromoCampaignDet","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoCampaignHdr{
            get { return DataService.GetSchema("PromoCampaignHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoCode{
            get { return DataService.GetSchema("PromoCode","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoDaysMap{
            get { return DataService.GetSchema("PromoDaysMap","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoDiscountTier{
            get { return DataService.GetSchema("PromoDiscountTier","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoLocationMap{
            get { return DataService.GetSchema("PromoLocationMap","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoMembershipMap{
            get { return DataService.GetSchema("PromoMembershipMap","PowerPOS"); }
		}
        
		public static TableSchema.Table PromoOutlet{
            get { return DataService.GetSchema("PromoOutlet","PowerPOS"); }
		}
        
		public static TableSchema.Table PurchaseOrderDet{
            get { return DataService.GetSchema("PurchaseOrderDet","PowerPOS"); }
		}
        
		public static TableSchema.Table PurchaseOrderDetail{
            get { return DataService.GetSchema("PurchaseOrderDetail","PowerPOS"); }
		}
        
		public static TableSchema.Table PurchaseOrderHdr{
            get { return DataService.GetSchema("PurchaseOrderHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table PurchaseOrderHeader{
            get { return DataService.GetSchema("PurchaseOrderHeader","PowerPOS"); }
		}
        
		public static TableSchema.Table QuickAccessButton{
            get { return DataService.GetSchema("QuickAccessButton","PowerPOS"); }
		}
        
		public static TableSchema.Table QuickAccessCategory{
            get { return DataService.GetSchema("QuickAccessCategory","PowerPOS"); }
		}
        
		public static TableSchema.Table QuickAccessGroup{
            get { return DataService.GetSchema("QuickAccessGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table QuickAccessGroupMap{
            get { return DataService.GetSchema("QuickAccessGroupMap","PowerPOS"); }
		}
        
		public static TableSchema.Table QuotationDet{
            get { return DataService.GetSchema("QuotationDet","PowerPOS"); }
		}
        
		public static TableSchema.Table QuotationHdr{
            get { return DataService.GetSchema("QuotationHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table Rating{
            get { return DataService.GetSchema("Rating","PowerPOS"); }
		}
        
		public static TableSchema.Table RatingFeedback{
            get { return DataService.GetSchema("RatingFeedback","PowerPOS"); }
		}
        
		public static TableSchema.Table RatingMaster{
            get { return DataService.GetSchema("RatingMaster","PowerPOS"); }
		}
        
		public static TableSchema.Table ReceiptDet{
            get { return DataService.GetSchema("ReceiptDet","PowerPOS"); }
		}
        
		public static TableSchema.Table ReceiptHdr{
            get { return DataService.GetSchema("ReceiptHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table ReceiptSetting{
            get { return DataService.GetSchema("ReceiptSetting","PowerPOS"); }
		}
        
		public static TableSchema.Table RecipeDetail{
            get { return DataService.GetSchema("RecipeDetail","PowerPOS"); }
		}
        
		public static TableSchema.Table RecipeHeader{
            get { return DataService.GetSchema("RecipeHeader","PowerPOS"); }
		}
        
		public static TableSchema.Table RecordDatum{
            get { return DataService.GetSchema("RecordData","PowerPOS"); }
		}
        
		public static TableSchema.Table Recurrence{
            get { return DataService.GetSchema("Recurrence","PowerPOS"); }
		}
        
		public static TableSchema.Table RedeemLog{
            get { return DataService.GetSchema("RedeemLog","PowerPOS"); }
		}
        
		public static TableSchema.Table RedemptionItem{
            get { return DataService.GetSchema("RedemptionItem","PowerPOS"); }
		}
        
		public static TableSchema.Table RedemptionLog{
            get { return DataService.GetSchema("RedemptionLog","PowerPOS"); }
		}
        
		public static TableSchema.Table Resource{
            get { return DataService.GetSchema("Resource","PowerPOS"); }
		}
        
		public static TableSchema.Table ResourceGroup{
            get { return DataService.GetSchema("ResourceGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table RetailerLevel{
            get { return DataService.GetSchema("RetailerLevel","PowerPOS"); }
		}
        
		public static TableSchema.Table Room{
            get { return DataService.GetSchema("Rooms","PowerPOS"); }
		}
        
		public static TableSchema.Table RoomsTimeSlot{
            get { return DataService.GetSchema("Rooms_Time_Slots","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesCommissionDetail{
            get { return DataService.GetSchema("SalesCommissionDetails","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesCommissionDetailsCommission{
            get { return DataService.GetSchema("SalesCommissionDetails_Commission","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesCommissionDetailsDeduction{
            get { return DataService.GetSchema("SalesCommissionDetails_Deduction","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesCommissionHistory{
            get { return DataService.GetSchema("SalesCommissionHistory","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesCommissionRecord{
            get { return DataService.GetSchema("SalesCommissionRecord","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesCommissionSummary{
            get { return DataService.GetSchema("SalesCommissionSummary","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesGroup{
            get { return DataService.GetSchema("SalesGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesOrderMapping{
            get { return DataService.GetSchema("SalesOrderMapping","PowerPOS"); }
		}
        
		public static TableSchema.Table SalesPerson{
            get { return DataService.GetSchema("SalesPerson","PowerPOS"); }
		}
        
		public static TableSchema.Table SAPCustomerCode{
            get { return DataService.GetSchema("SAPCustomerCode","PowerPOS"); }
		}
        
		public static TableSchema.Table SavedClosing{
            get { return DataService.GetSchema("SavedClosing","PowerPOS"); }
		}
        
		public static TableSchema.Table SavedFile{
            get { return DataService.GetSchema("SavedFiles","PowerPOS"); }
		}
        
		public static TableSchema.Table SaveItem{
            get { return DataService.GetSchema("SaveItem","PowerPOS"); }
		}
        
		public static TableSchema.Table ScheduledDiscount{
            get { return DataService.GetSchema("ScheduledDiscount","PowerPOS"); }
		}
        
		public static TableSchema.Table ServerQuickRef{
            get { return DataService.GetSchema("ServerQuickRef","PowerPOS"); }
		}
        
		public static TableSchema.Table ServicingAppointment{
            get { return DataService.GetSchema("ServicingAppointment","PowerPOS"); }
		}
        
		public static TableSchema.Table Setting{
            get { return DataService.GetSchema("Setting","PowerPOS"); }
		}
        
		public static TableSchema.Table SpecialActivityLog{
            get { return DataService.GetSchema("SpecialActivityLog","PowerPOS"); }
		}
        
		public static TableSchema.Table SpecialDiscountDetail{
            get { return DataService.GetSchema("SpecialDiscountDetail","PowerPOS"); }
		}
        
		public static TableSchema.Table SpecialDiscount{
            get { return DataService.GetSchema("SpecialDiscounts","PowerPOS"); }
		}
        
		public static TableSchema.Table SpecialEvent{
            get { return DataService.GetSchema("SpecialEvent","PowerPOS"); }
		}
        
		public static TableSchema.Table StaffAssistLog{
            get { return DataService.GetSchema("StaffAssistLog","PowerPOS"); }
		}
        
		public static TableSchema.Table StockStaging{
            get { return DataService.GetSchema("StockStaging","PowerPOS"); }
		}
        
		public static TableSchema.Table StockTake{
            get { return DataService.GetSchema("StockTake","PowerPOS"); }
		}
        
		public static TableSchema.Table StockTransferDet{
            get { return DataService.GetSchema("StockTransferDet","PowerPOS"); }
		}
        
		public static TableSchema.Table StockTransferHdr{
            get { return DataService.GetSchema("StockTransferHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table Supplier{
            get { return DataService.GetSchema("Supplier","PowerPOS"); }
		}
        
		public static TableSchema.Table SupplierItemMap{
            get { return DataService.GetSchema("SupplierItemMap","PowerPOS"); }
		}
        
		public static TableSchema.Table SyncLog{
            get { return DataService.GetSchema("SyncLog","PowerPOS"); }
		}
        
		public static TableSchema.Table SyncRequest{
            get { return DataService.GetSchema("SyncRequest","PowerPOS"); }
		}
        
		public static TableSchema.Table TestTable{
            get { return DataService.GetSchema("TestTable","PowerPOS"); }
		}
        
		public static TableSchema.Table TestTable2{
            get { return DataService.GetSchema("TestTable2","PowerPOS"); }
		}
        
		public static TableSchema.Table TextLanguage{
            get { return DataService.GetSchema("TEXT_LANGUAGE","PowerPOS"); }
		}
        
		public static TableSchema.Table TimeSlot{
            get { return DataService.GetSchema("Time_Slots","PowerPOS"); }
		}
        
		public static TableSchema.Table TmpItem{
            get { return DataService.GetSchema("tmpItems","PowerPOS"); }
		}
        
		public static TableSchema.Table UOMConversionDet{
            get { return DataService.GetSchema("UOMConversionDet","PowerPOS"); }
		}
        
		public static TableSchema.Table UOMConversionHdr{
            get { return DataService.GetSchema("UOMConversionHdr","PowerPOS"); }
		}
        
		public static TableSchema.Table UserGroup{
            get { return DataService.GetSchema("UserGroup","PowerPOS"); }
		}
        
		public static TableSchema.Table UserMst{
            get { return DataService.GetSchema("UserMst","PowerPOS"); }
		}
        
		public static TableSchema.Table UserMstTombstone{
            get { return DataService.GetSchema("UserMst_Tombstone","PowerPOS"); }
		}
        
		public static TableSchema.Table UserPrivilege{
            get { return DataService.GetSchema("UserPrivilege","PowerPOS"); }
		}
        
		public static TableSchema.Table VoidLog{
            get { return DataService.GetSchema("VoidLog","PowerPOS"); }
		}
        
		public static TableSchema.Table VoucherHeader{
            get { return DataService.GetSchema("VoucherHeader","PowerPOS"); }
		}
        
		public static TableSchema.Table Voucher{
            get { return DataService.GetSchema("Vouchers","PowerPOS"); }
		}
        
		public static TableSchema.Table VoucherStatus{
            get { return DataService.GetSchema("VoucherStatus","PowerPOS"); }
		}
        
		public static TableSchema.Table WarningMsg{
            get { return DataService.GetSchema("WarningMsg","PowerPOS"); }
		}
        
		public static TableSchema.Table Warranty{
            get { return DataService.GetSchema("Warranty","PowerPOS"); }
		}
        
		public static TableSchema.Table Word{
            get { return DataService.GetSchema("Word","PowerPOS"); }
		}
        
		public static TableSchema.Table Z2ClosingLog{
            get { return DataService.GetSchema("Z2ClosingLog","PowerPOS"); }
		}
        
	
    }
    #endregion
    #region View Struct
    public partial struct Views 
    {
		
		public static string ViewAdjustedStockTake = @"ViewAdjustedStockTake";
        
		public static string ViewAlternateBarcode = @"ViewAlternateBarcode";
        
		public static string ViewAvgTransaction = @"ViewAvgTransaction";
        
		public static string ViewAvgTransactionByOutlet = @"ViewAvgTransactionByOutlet";
        
		public static string ViewCashRecording = @"ViewCashRecording";
        
		public static string ViewCategory = @"ViewCategory";
        
		public static string ViewCloseCounterReport = @"ViewCloseCounterReport";
        
		public static string ViewCostOfGood = @"ViewCostOfGoods";
        
		public static string ViewDailyTransactionBySalesPerson = @"ViewDailyTransactionBySalesPerson";
        
		public static string ViewDWDailyProductCategorySale = @"viewDW_DailyProductCategorySales";
        
		public static string ViewDWDailyProductCategorySalesTodaySrc = @"viewDW_DailyProductCategorySales_today_src";
        
		public static string ViewDWDailyProductSale = @"viewDW_DailyProductSales";
        
		public static string ViewDWDailyProductSalesTodaySrc = @"viewDW_DailyProductSales_today_src";
        
		public static string ViewDWDailySale = @"viewDW_DailySales";
        
		public static string ViewDWDailySalesSrc = @"viewDW_DailySales_src";
        
		public static string ViewDWDailySalesTodaySalesSrc = @"viewDW_DailySales_today_sales_src";
        
		public static string ViewDWDailySalesTodaySrc = @"viewDW_DailySales_today_src";
        
		public static string ViewDWHourlyPaymentSrc = @"viewDW_HourlyPayment_src";
        
		public static string ViewDWHourlyProductCategorySale = @"viewDW_HourlyProductCategorySales";
        
		public static string ViewDWHourlyProductCategorySalesTodaySrc = @"viewDW_HourlyProductCategorySales_today_src";
        
		public static string ViewDWHourlyProductDepartmentSale = @"viewDW_HourlyProductDepartmentSales";
        
		public static string ViewDWHourlyProductDepartmentSalesTodaySrc = @"viewDW_HourlyProductDepartmentSales_today_src";
        
		public static string ViewDWHourlyProductSale = @"viewDW_HourlyProductSales";
        
		public static string ViewDWHourlyProductSalesSrc = @"viewDW_HourlyProductSales_src";
        
		public static string ViewDWHourlyProductSalesTodaySrc = @"viewDW_HourlyProductSales_today_src";
        
		public static string ViewDWHourlySale = @"viewDW_HourlySales";
        
		public static string ViewDWHourlySalesSrc = @"viewDW_HourlySales_src";
        
		public static string ViewDWHourlySalesTodaySalesSrc = @"viewDW_HourlySales_today_sales_src";
        
		public static string ViewDWHourlySalesTodaySrc = @"viewDW_HourlySales_today_src";
        
		public static string ViewDWMonthlySale = @"viewDW_MonthlySales";
        
		public static string ViewErrornousSalesStockOut = @"ViewErrornousSalesStockOut";
        
		public static string ViewEventLocationMap = @"ViewEventLocationMap";
        
		public static string ViewEZLinkMessageParameter = @"ViewEZLinkMessageParameter";
        
		public static string ViewGroupPrivilege = @"ViewGroupPrivileges";
        
		public static string ViewHourlyTransaction = @"ViewHourlyTransaction";
        
		public static string ViewInstallmentMembershipItem = @"ViewInstallmentMembershipItem";
        
		public static string ViewInventoryActivity = @"ViewInventoryActivity";
        
		public static string ViewInventoryActivityByItemNo = @"ViewInventoryActivityByItemNo";
        
		public static string ViewInventoryHeader = @"ViewInventoryHeader";
        
		public static string ViewInventoryItemOnHandQty = @"ViewInventoryItemOnHandQty";
        
		public static string ViewInventorySalesStockOut = @"ViewInventorySalesStockOut";
        
		public static string ViewInventoryTransfer = @"ViewInventoryTransfer";
        
		public static string ViewInventoryTransferDetail = @"ViewInventoryTransferDetail";
        
		public static string ViewInventoryTransferDiscrepancy = @"ViewInventoryTransferDiscrepancy";
        
		public static string ViewItem = @"ViewItem";
        
		public static string Viewitembaselevel = @"viewitembaselevel";
        
		public static string ViewItemGroup = @"ViewItemGroup";
        
		public static string ViewItemGroupMap = @"ViewItemGroupMap";
        
		public static string ViewItemSummary = @"ViewItemSummary";
        
		public static string ViewItemSupplier = @"ViewItemSupplier";
        
		public static string ViewLoginActivityReport = @"ViewLoginActivityReport";
        
		public static string ViewMembership = @"ViewMembership";
        
		public static string ViewMembershipAttendance = @"ViewMembershipAttendance";
        
		public static string ViewMembershipRemark = @"ViewMembershipRemark";
        
		public static string ViewMembershipTap = @"ViewMembershipTap";
        
		public static string ViewMembershipTapsLog = @"ViewMembershipTapsLog";
        
		public static string ViewMembershipUpgrade = @"ViewMembershipUpgrades";
        
		public static string ViewMonthlyAvgTransaction = @"ViewMonthlyAvgTransaction";
        
		public static string ViewMonthlyNumTransaction = @"ViewMonthlyNumTransaction";
        
		public static string ViewNumTransaction = @"ViewNumTransaction";
        
		public static string ViewNumTransactionByOutlet = @"ViewNumTransactionByOutlet";
        
		public static string ViewOrderHdrInstOutstandingBal = @"ViewOrderHdrInstOutstandingBal";
        
		public static string ViewPackageRedemption = @"ViewPackageRedemption";
        
		public static string ViewPackage = @"ViewPackages";
        
		public static string ViewPreOrderSchedule = @"ViewPreOrderSchedule";
        
		public static string ViewProductAndServiceBySalesPerson = @"ViewProductAndServiceBySalesPerson";
        
		public static string ViewPromoLocationMap = @"ViewPromoLocationMap";
        
		public static string ViewPromoMasterDetail = @"ViewPromoMasterDetail";
        
		public static string ViewPromoMasterDetailAny = @"ViewPromoMasterDetailAny";
        
		public static string ViewPromoMembershipMap = @"ViewPromoMembershipMap";
        
		public static string ViewPromotionsByItem = @"ViewPromotionsByItem";
        
		public static string ViewPurchaseOrder = @"ViewPurchaseOrder";
        
		public static string ViewPurchaseOrderHeader = @"ViewPurchaseOrderHeader";
        
		public static string ViewQuickAccessButton = @"ViewQuickAccessButtons";
        
		public static string ViewQuickAccessGroupMap = @"ViewQuickAccessGroupMap";
        
		public static string ViewReceiptDet = @"ViewReceiptDet";
        
		public static string ViewReceiptSummary = @"ViewReceiptSummary";
        
		public static string ViewRecipe = @"ViewRecipe";
        
		public static string ViewRedemptionItem = @"ViewRedemptionItem";
        
		public static string ViewRedemptionLog = @"ViewRedemptionLog";
        
		public static string ViewSalesDetail = @"ViewSalesDetail";
        
		public static string ViewSalesPersonCurrentMonthCommission = @"ViewSalesPersonCurrentMonthCommission";
        
		public static string ViewSalesPersonLastMonthCommission = @"ViewSalesPersonLastMonthCommission";
        
		public static string ViewSaveItem = @"ViewSaveItem";
        
		public static string ViewSpecialEventItem = @"ViewSpecialEventItems";
        
		public static string ViewStockTake = @"ViewStockTake";
        
		public static string ViewTransactionDetail = @"ViewTransactionDetail";
        
		public static string ViewTransactionDetailWithRemark = @"ViewTransactionDetailWithRemark";
        
		public static string ViewTransactionDetailWithSalesPerson = @"ViewTransactionDetailWithSalesPerson";
        
		public static string ViewTransaction = @"ViewTransactions";
        
		public static string ViewTransactionWithMembership = @"ViewTransactionWithMembership";
        
		public static string ViewTransactionWithSalesPerson = @"ViewTransactionWithSalesPerson";
        
		public static string ViewUser = @"ViewUser";
        
		public static string ViewVoucherRedeemed = @"ViewVoucherRedeemed";
        
		public static string ViewVouchersSold = @"ViewVouchersSold";
        
		public static string ViewWarrantyMembershipItem = @"ViewWarrantyMembershipItem";
        
		public static string ViewWarrantyMembershipItemOrder = @"ViewWarrantyMembershipItemOrder";
        
		public static string VRandNumber = @"vRandNumber";
        
    }
    #endregion
    
    #region Query Factories
	public static partial class DB
	{
        public static DataProvider _provider = DataService.Providers["PowerPOS"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository {
            get {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository; 
            }
            set { _repository = value; }
        }
	
        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
	    {
            return Repository.SelectAllColumnsFrom<T>();
            
	    }
	    public static Select Select()
	    {
            return Repository.Select();
	    }
	    
		public static Select Select(params string[] columns)
		{
            return Repository.Select(columns);
        }
	    
		public static Select Select(params Aggregate[] aggregates)
		{
            return Repository.Select(aggregates);
        }
   
	    public static Update Update<T>() where T : RecordBase<T>, new()
	    {
            return Repository.Update<T>();
	    }
     
	    
	    public static Insert Insert()
	    {
            return Repository.Insert();
	    }
	    
	    public static Delete Delete()
	    {
            
            return Repository.Delete();
	    }
	    
	    public static InlineQuery Query()
	    {
            
            return Repository.Query();
	    }
	    	    
	    
	}
    #endregion
    
}
#region Databases
public partial struct Databases 
{
	
	public static string PowerPOS = @"PowerPOS";
    
}
#endregion