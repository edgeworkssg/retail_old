using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class AppSetting
    {
        public struct SettingsName
        {
            /* AppSettingKey Convention 
             * -----------------------------
             * [Object] + <Underscore> + [SettingName] */

            public const string Database_Version = "version";

            public const string CultureCode = "CultureCode";
            public const string LanguageSetting = "LanguageSetting";

            public const string HotKeysItemNamePrice = "ItemNamePrice";
            public const string HotKeysPriceItemName = "PriceItemName";

            public const string IsReplaceMembershipText = "IsReplaceMembershipText";
            public const string MembershipTextReplacement = "MembershipTextReplacement";

            public const string CompanyID = "CompanyID";
            public const string POSType = "POSType";
            public const string FileManagerURL = "FileManagerURL";
            public const string CustomerMasterURL = "CustomerMasterURL";
            public const string APICallerID = "APICallerID";
            public const string APIPrivateKey = "APIPrivateKey";

            public struct Hotkeys
            {
                public const string HotkeysDisplay = "HotkeysDisplay";
            }


            public struct Appointment
            {
                public const string IsAvailable = "Appointment_IsAvailable";
                public const string UseWeeklyView = "Appointment_UseWeeklyView";
                public const string UseMonthlyView = "Appointment_UseMonthlyView";
                public const string PointOfSale_WorkingHoursStart = "PointOfSale_WorkingHoursStart";
                public const string PointOfSale_WorkingHoursEnd = "PointOfSale_WorkingHoursEnd";
                public const string PointOfSale_MinimumTimeInterval = "PointOfSale_MinimumTimeInterval";
                public const string SalesPerson_OrderBy = "SalesPerson_OrderBy";
                public const string MembershipMandatory = "Appointment_MembershipMandatory";
                public const string ServicesMandatory = "Appointment_ServicesMandatory";
                public const string AppointmentSearchList = "Appointment_AppointmentSearchList";
                public const string DisableTimeCollisionCheck = "Appointment_DisableTimeCollisionCheck";
                public const string UseResourceOnAppointment = "Appointment_UseResourceOnAppointment";
                public const string MinimumIntervalWeb = "Appointment_MinimumIntervalWeb";
                public const string AutoCheckOutIntervalGuestBook = "AutoCheckOutIntervalGuestBook";
            }

            public struct Attendance
            {
                public const string IsAvailable = "Attendance_IsAvailable";
                public const string AutoAttendAfterSales = "Attendance_AutoAttendAfterSales";
                public const string GenerateOrderUponCheckOut = "Attendance_GenerateOrderUponCheckOut";
                public const string MinutesRoundingUp = "Attendance_MinutesRoundingUp";
                public const string ItemNo = "Attendance_ItemNo";
            }

            public struct Currency
            {
                public const string DefaultCurrency = "Currency_DefaultCurrency";
            }

            public struct Google
            {
                public const string UserID = "Google_UserID";
                public const string Password = "Google_Password";
            }

            public struct Inventory
            {
                public const string DisableInventory = "Inventory_DisableInventoryDeduction";
                public const string CostingMethod = "Inventory_CostingMethod";
                public const string HideInventoryCost = "Inventory_HideCost";
                public const string HideCostInStockTransfer = "Inventory_HideCostInStockTransfer";
                public const string HideRetailPrice = "Inventory_HideRetailPrice";
                public const string RemoveZeroInventoryValidation = "Inventory_RemoveZeroInventoryValidation";
                public const string ShowCostOnStockOnHand = "ShowCostOnStockOnHand";
                public const string SupplierIsCompulsory = "SupplierIsCompulsory";
                public const string SupplierIsCompulsoryPO = "SupplierIsCompulsoryPO";
                public const string ShowUOM = "Inventory_ShowUOM";
                public const string ShowCurrency= "Inventory_ShowCurrency";
                public const string DontUpdateFactoryPriceIfZero = "Inventory_DontUpdateFactoryPriceIfZero";
                public const string OnlyAllowInCurrentInventoryLocation = "Inventory_OnlyAllowInCurrentInventoryLocation";
                public const string UpdateFactoryPriceOnStockIn = "Inventory_UpdateFactoryPriceOnStockIn";
                public const string AllowToUpdateRetailPriceInGoodsReceive = "Inventory_AllowToUpdateRetailPriceInGoodsReceive";
                public const string SalesCostOfGoods = "Inventory_SalesCostOfGoods";
                public const string AllowStockTransferEvenStockIsZero = "AllowStockTransferEvenStockIsZero";
                public const string ShowPreOrderQtyInStockOnHand = "Inventory_ShowPreOrderQtyInStockOnHand";
                public const string ShowBalanceQuantityOnTransaction = "Inventory_ShowBalanceQuantityOnTransaction";
                public const string ShowBalanceQuantityForStockTransfer = "Inventory_ShowBalanceQuantityForStockTransfer";
                public const string AllowChangeInventoryDate = "Inventory_AllowChangeInventoryDate";
                public const string SaveFileInTheServer = "Inventory_SaveFileInTheServer";
                public const string CalculateAvgCostatInventoryLocationLevel = "Inventory_CalculateAvgCostatInventoryLocationLevel";
                public const string CalculateAvgCostatInventoryLocationGroupLevel = "Inventory_CalculateAvgCostatInvLocGroupLevel";
                public const string GetCostForAdjusmentInfromItemSetupIfZero = "Inventory_GetCostForAdjusmentInfromItemSetupIfZero";
                public const string GetCostForStockOutfromItemSetupIfZero = "Inventory_GetCostForStockOutfromItemSetupIfZero";
                public const string BlockTransactionIfBalQtyNotSuf = "Inventory_BlockTransactionIfBalQtyNotSuf";
                public const string SwitchColumnRowsMatrixForm = "SwitchColumnRowsMatrixForm";
                public const string ShowBatchNoStockTake = "ShowBatchNoStockTake";
                public const string ShowParValueStockTake = "ShowParValueStockTake";
                public const string ShowWarningWhenSellingPriceLessThanCostPrice = "ShowWarningWhenSellingPriceLessThanCostPrice";
                public const string ChangePriceStockAdjIssue = "ChangePriceStockAdjIssue";
                public const string GoodsReceiveGSTRuleFromSupplier = "Inventory_GoodsReceiveGSTRuleFromSupplier";
                public const string UseBasicCostPrice = "Inventory_UseBasicCostPrice";
                public const string AddItemPicture = "Inventory_AddItemPicture";
                public const string UseImageLocal = "Inventory_UseImageLocal";
                public const string ImageLocalPath = "Inventory_ImageLocalPath";
                public const string AddNegativeMissOutItemOnly = "Inventory_AddNegativeMissOutItemOnly";
                public const string DownloadItemSummaryAllLocation = "Inventory_DownloadItemSummaryAllLocation";
                public const string StockReturnNoAffectCOGS = "Inventory_StockReturnNoAffectCOGS";
                public const string CheckQuantityonServer = "Inventory_CheckQuantityonServer";
                public const string EditableAutoStockIn = "Inventory_EditableAutoStockIn";
                public const string IsAutoStockIn = "Inventory_IsAutoStockIn";
                public const string TextBeautyAdvisors = "Inventory_TextBeautyAdvisors";
                public const string IsLockSalesPersonGR = "Inventory_IsLockSalesPersonGR";

                public const string CostCalculationMode = "Inventory_CostCalculationMode";
                public const string EnableProductSerialNo = "Inventory_EnableProductSerialNo";
				
				public const string VendorInvoiceNoCompulsory = "Inventory_VendorInvoiceNoCompulsory";
                public const string IncludeGSTExclusive = "Inventory_IncludeGSTExclusive";
            }

            public struct GoodsReceive
            {
                public const string UseCustomNo = "GoodsReceive_UseCustomNo";
                public const string CustomPrefix = "GoodsReceive_CustomPrefix";
                public const string CustomSuffix = "GoodsReceive_CustomSuffix";
                public const string NumberLength = "GoodsReceive_NumberLength";
                public const string CurrentNo = "GoodsReceive_CurrentNo";
                public const string ResetNumberEvery = "GoodsReceive_ResetNumberEvery";
                public const string CustomNoDateFormat = "GoodsReceive_CustomNoDateFormat";
                public const string LastReset = "GoodsReceive_LastReset";
                public const string GenerateCustomNoInServer = "GoodsReceive_GenerateCustomNoInServer";

                public const string ShowCustomField1 = "GoodsReceive_ShowCustomField1";
                public const string ShowCustomField2 = "GoodsReceive_ShowCustomField2";
                public const string ShowCustomField3 = "GoodsReceive_ShowCustomField3";
                public const string ShowCustomField4 = "GoodsReceive_ShowCustomField4";
                public const string ShowCustomField5 = "GoodsReceive_ShowCustomField5";
                public const string CustomField1Label = "GoodsReceive_CustomField1Label";
                public const string CustomField2Label = "GoodsReceive_CustomField2Label";
                public const string CustomField3Label = "GoodsReceive_CustomField3Label";
                public const string CustomField4Label = "GoodsReceive_CustomField4Label";
                public const string CustomField5Label = "GoodsReceive_CustomField5Label";

                public const string ShowAdditionalCost1 = "GoodsReceive_ShowAdditionalCost1";
                public const string ShowAdditionalCost2 = "GoodsReceive_ShowAdditionalCost2";
                public const string ShowAdditionalCost3 = "GoodsReceive_ShowAdditionalCost3";
                public const string ShowAdditionalCost4 = "GoodsReceive_ShowAdditionalCost4";
                public const string ShowAdditionalCost5 = "GoodsReceive_ShowAdditionalCost5";
                public const string AdditionalCost1Label = "GoodsReceive_AdditionalCost1Label";
                public const string AdditionalCost2Label = "GoodsReceive_AdditionalCost2Label";
                public const string AdditionalCost3Label = "GoodsReceive_AdditionalCost3Label";
                public const string AdditionalCost4Label = "GoodsReceive_AdditionalCost4Label";
                public const string AdditionalCost5Label = "GoodsReceive_AdditionalCost5Label";
                public const string AdditionalCost1_IsPercentage = "GoodsReceive_AdditionalCost1_IsPercentage";
                public const string AdditionalCost2_IsPercentage = "GoodsReceive_AdditionalCost2_IsPercentage";
                public const string AdditionalCost3_IsPercentage = "GoodsReceive_AdditionalCost3_IsPercentage";
                public const string AdditionalCost4_IsPercentage = "GoodsReceive_AdditionalCost4_IsPercentage";
                public const string AdditionalCost5_IsPercentage = "GoodsReceive_AdditionalCost5_IsPercentage";
            }

            public struct MembershipPoint
            {
                public const string ValidityPeriodInMonth = "MembershipPoint_ValidityPeriodInMonth";
            }

            public struct Closing
            {
                public const string EnableChangeShift = "EnableChangeShift";
                public const string UseTotalCashForCheckOut = "UseTotalCashForCheckOut";
                public const string HideSystemRecordedOnCheckOut = "HideSystemRecordedOnCheckOut";
                public const string SyncAfterCheckOut = "SyncAfterCheckOut";
                public const string EnableAutoBackup = "Closing_EnableAutoBackup";
                public const string DepositBagMandatory = "Closing_DepositBagMandatory";
                public const string DontAllowIfGotHold = "Closing_DontAllowIfGotHold";
                public const string IsShowCashDenominationOnReceipt = "Closing_IsShowCashDenominationOnReceipt";
            }

            public struct DailyOutletCollection
            {
                public const string EnableCashBreakdown = "DailyOutletCollection_EnableCashBreakdown";
                public const string CashBreakdownUnit = "DailyOutletCollection_CashBreakdownUnit";
                public const string ShowDepositedDate = "DailyOutletCollection_ShowDepositedDate"; 
            }

            public struct Points
            {
                public const string IsUsingPercentage = "Points_IsUsingPercentage";
                public const string PercentagePointName = "Points_PercentagePointName";
                public const string AskRemarksWhenUsePoint = "Points_AskRemarksWhenUsePoint";   
                public const string NotAlwaysUse1Point = "Points_NotAlwaysUse1Point";
                public const string HaveExpiryDate = "Points_HaveExpiryDate";
                public const string ExpiredAfter = "Points_ExpiredAfter";
                public const string Rounding = "Points_Rounding";
                public const string IntegrationPointsItemNo = "Points_IntegrationPointsItemNo";
                public const string CanUseMultiplePackageInOneReceipt = "Points_CanUseMultiplePackageInOneReceipt";
                public const string WaitToDownloadPointsBeforePrintReceipt = "Points_WaitToDownloadPointsBeforePrintReceipt";
                public const string IsAskingPassCode = "Points_IsAskingPassCode";
                public const string ChoosePointPackageForPayment = "Points_ChoosePointPackageForPayment";
                public const string WontGetRewardPointsIfBuyPackageItem = "Points_WontGetRewardPointsIfBuyPackageItem";
                public const string ExcludePaymentTypeForPointsCalculation = "Points_ExcludePaymentTypeForPointsCalculation";
                public const string ExcludedPaymentTypes = "Points_ExcludedPaymentTypes";
            }

            public struct Print
            {
                public const string IsUsingDeliveryOrder = "Print_IsUsingDeliveryOrder";
                public const string ReceiptFileLocation = "Print_ReceiptFileLocation";
                public const string ClosingReceiptFileLocation = "ClosingReceiptFileLocation";
                public const string ClosingReceiptNumOfCopies = "ClosingReceiptNumOfCopies";
                public const string DeliveryOrderFileLocation = "Print_DeliveryOrderFileLocation";
                public const string PrinterName = "Print_PrinterName";
                public const string StockTakeReportFileLocation = "Print_StockTakeReportFileLocation";
                public const string GoodsReceiveReportFileLocation = "Print_GoodsReceiveReportFileLocation";
                public const string StockIssueReportFileLocation = "Print_StockIssueReportFileLocation";
                public const string StockAdjustmentReportFileLocation = "Print_StockAdjustmentReportFileLocation";
                public const string StockTransferReportFileLocation = "Print_StockTransferReportFileLocation";
                public const string StockReturnReportFileLocation = "Print_StockReturnReportFileLocation";
                public const string OtherStockActivityReportFileLocation = "Print_OtherStockActivityReportFileLocation";
                public const string UseCategoryFilterOnClosingReport = "UseCategoryFilterOnClosingReport";
                public const string CategoryFilterName = "CategoryFilterName";
                public const string PrintInvoiceOnDelivery = "PrintInvoiceOnDelivery";
                public const string AddDeliveryRemarks = "AddDeliveryRemarks";
                public const string AddPurchaseOrderInfo = "AddPurchaseOrderInfo";
                public const string PrintCounterCloseReport = "PrintCounterCloseReport";
                public const string PrintItemDepartmentOnCheckOut = "PrintItemDepartmentOnCheckOut";
                public const string HidePrintPackageBalanceOnReceipt = "HidePrintPackageBalanceOnReceipt";
                public const string PrintDiscountOnCounterCloseReport = "PrintDiscountOnCounterCloseReport";
                public const string PurchaseOrderFileLocation = "Print_PurchaseOrderFileLocation";
                public const string PrintCashierOnTheReceipt = "PrintCashierOnTheReceipt";
                //public const string PrintPointOnTheReceipt = "PrintPointOnTheReceipt";
                public const string PrintBalancePaymentOnTheReceipt = "PrintBalancePaymentOnTheReceipt";
                public const string PromptSelectPrintSize = "Print_PromptSelectPrintSize";
                public const string QuotationReceiptFileLocation = "QuotationReceiptFileLocation";
                public const string UseAdditionalPrinter = "UseAdditionalPrinter";
                public const string AdditionalPrinter = "AdditionalPrinter";
                public const string AdditionalReceipt = "AdditionalReceipt";

                public const string FormalReceiptFileLocation = "FormalReceiptFileLocation";
                public const string Print_SaveReceiptAsDocument = "Print_SaveReceiptAsDocument";
                public const string PDFPath = "PDFPath";
                public const string ShowSalesWithoutCategoryFilter = "ShowSalesWithoutCategoryFilter";
                public const string ShowSalesWithoutItemDeptFilter = "ShowSalesWithoutItemDeptFilter";
                public const string ItemDeptFilterName = "ItemDeptFilterName";
                public const string PrintNETSResponseInfoOnTheReceipt = "PrintNETSResponseInfoOnTheReceipt";
                public const string PrintPreOrderTemplateReport = "PrintPreOrderTemplateReport";
            }

            public struct PromptPassword
            {
                public const string OnCheckOut = "PromptPassword_CheckOut";
                public const string OnVoid = "PromptPassword_Void";
                public const string OnCashRecording = "PromptPassword_CashRecording";
                public const string OnDeductPoints = "PromptPassword_DeductPoints";
                public const string OnRefund = "PromptPassword_OnRefund";
                public const string OnOpeningBalance = "PromptPassword_OpeningBalance";
                public const string OnCashIn = "PromptPassword_CashIn";
                public const string OnCashOut = "PromptPassword_OnCashOut";
                public const string OnVoidItem = "PromptPassword_OnVoidItem";
            }

            public struct Sales
            {
                public const string EnableHoldOrderFromServer = "Sales_HoldOrderFromServer";
            }

            public struct Signature
            {
                public const string IsAvailable = "Signature_IsAvailable";
                public const string IsAvailableForAllPayment = "Signature_IsAvailableForAllPayment";
                public const string IsDeliveryPreOrder = "Signature_IsDeliveryPreOrder";
                public const string IsStaffReceipt = "Signature_IsStaffReceipt";
                public const string SignatureDevice = "SIGNATURE_DEVICE";
                public const string STANDARD = "STANDARD";
                public const string WACOM = "WACOM";
                public const string EPAD = "EPAD";
            }

            public struct UseMagneticStripReader
            {
                public const string ForAuthorizing = "UseMagneticStripCardForAuthorizing";
                public const string ForLogin = "UseMagneticStripCardForLogin";
                public const string ForMembership = "UseMagneticStripCardForMembership";
            }

            public struct User
            {
                public const string LinkTheUserWithOutlet = "User_LinkTheUserWithOutlet";
                public const string SeparateUserPerOutletPrivileges = "User_SeparateUserPerOutletPrivileges";
                public const string UseUserGroupOutletAssignment = "User_UseUserGroupOutletAssignment";
                public const string ShowPointOfSale = "User_ShowPointOfSale";
                public const string ShowOutlet = "User_ShowOutlet";
                public const string ChangePriceRestrictedTo = "User_ChangePriceRestrictedTo";
                public const string UseSupplierPortal = "User_UseSupplierPortal";
            }

            public struct Membership
            {
                public const string AutoMembershipUpgrade = "Auto_Membership_Upgrade";
                public const string Membership_Compulsory = "Membership_Compulsory";
                public const string MembershipSummaryRowNumber = "MembershipSummaryRowNumber";
                public const string ShowMembershipWarning = "ShowMembershipWarning";
                public const string MembershipWarningFields = "MembershipWarningFields";
                public const string Membership_ShowAttachedParticular = "Membership_ShowAttachedParticular";
                public const string MembershipSyncSegmentSize = "MembershipSyncSegmentSize";
                public const string ViewRealTimeSalesHistory = "Membership_ViewRealTimeSalesHistory";
                public const string ShowRemarkInTransactionReport = "Membership_ShowRemarkInTransactionReport";
                public const string ShowLineInfoInTransactionReport = "Membership_ShowLineInfoInTransactionReport";
                public const string ShowBalancePaymentInTransactionReport = "Membership_ShowBalancePaymentInTransactionReport";
                public const string ShowQtyInTransactionReport = "Membership_ShowQtyInTransactionReport";
                public const string ShowQtyOnHandInTransactionReport = "Membership_ShowQtyOnHandInTransactionReport";
                public const string ShowPreOrderSummaryButton = "Membership_ShowPreOrderSummaryButton";
                public const string DownloadAllRecentPurchase = "Membership_DownloadAllRecentPurchase";
                public const string AskPassCodeWhenCreateNewMember = "AskPassCodeWhenCreateNewMember";
                public const string ShowPackageEvenIfRemainingIsZero = "Membership_ShowPackageEvenIfRemainingIsZero";
                public const string ExpiryDateBasedOnRenewalDate = "Membership_ExpiryDateBasedOnRenewalDate";
                public const string AllowShowSalesOutlet = "Membership_AllowShowSalesOutlet";
                public const string EmailCompulsoryIsTicked = "Membership_EmailCompulsoryIsTicked";
                public const string AllowEditMemberGroup = "Membership_AllowEditMemberGroup";
                public const string DefaultMembershipGroup = "Membership_DefaultMemberGroup";
                public const string DefaultExpiryDate = "Membership_DefaultExpiryDate";
            }

            public struct DefaultPayment
            {
                public const string UseDefaultPayment = "Use_Default_Payment";
                public const string DefaultPaymentType = "Default_Payment_Type";
                public const string DefaultPayment_ShowCashForm = "DefaultPayment_ShowCashForm";
            }

            public struct Payment
            {
                /// <summary>
                /// Please add the payment type behind the string to check the extra charge for each payment type
                /// </summary>
                public const string ExtraChargeType = "Payment_ExtraChargeType_";
                public const string ExtraChargeAmount = "Payment_ExtraChargeAmount_";
                public const string ShowTotalQuantityInSalesScreen = "ShowTotalQuantityInSalesScreen";
                public const string ShowTotalItemInSalesScreen = "ShowTotalItemInSalesScreen";
                public const string InstallmentText = "InstallmentText";
                public const string UseNegativeSalesAmount = "UseNegativeSalesAmount";
                public const string UseQuickPayment = "UseQuickPayment";
                public const string MaxBalancePayment = "Payment_MaxBalancePayment";
                
                public const string EnableEZLinkIntegration = "Payment_EnableEZLinkIntegration";
                public const string EZLinkPaymentType = "Payment_EZLinkPaymentType";
                public const string EnableEZLinkContactlessIntegration = "Payment_EnableEZLinkContactlessIntegration";
                public const string EZLinkContactlessPaymentType = "Payment_EZLinkContactlessPaymentType";

                public const string EnableNETSIntegration = "Payment_EnableNETSIntegration";
                public const string NETSPayment = "Payment_NETSPayment";
                public const string EnableNETSATMCard = "Payment_EnableNETSATMCard";
                public const string EnableNETSFlashPay = "Payment_EnableNETSFlashPay";
                public const string EnableNETSCashCard = "Payment_EnableNETSCashCard";
                public const string NETSCashCardWithService = "Payment_NETSCashCardWithService";
                public const string EnableNETSQR = "Payment_EnableNETSQR";

                public const string NetsThreadSleepWait = "Payment_NetsThreadSleepWait";

                public const string EnableUNIONPayIntegration = "Payment_EnableUNIONPayIntegration";
                public const string UNIONPayPayment = "Payment_UNIONPayPayment";

                public const string EnableBCAIntegration = "Payment_EnableBCAIntegration";
                public const string BCAPayment = "Payment_BCAPayment";

                public const string EnableCUPIntegration = "Payment_EnableCUPIntegration";
                public const string CUPPayment = "Payment_CUPPayment";

                public const string EnablePrepaidPurchaseIntegration = "Payment_EnablePrepaidPurchaseIntegration";
                public const string PrepaidPurchasePayment = "Payment_PrepaidPurchasePayment";

                public const string EnableCitiBankIntegration = "Payment_EnableCitiBankIntegration";
                public const string CitiBankPayment = "Payment_CitiBankPayment";

                public const string EnableCreditCardIntegration = "Payment_EnableCreditCardIntegration";
                public const string CreditCardPayment = "Payment_CreditCardPayment";

                public const string NetsCOMPort = "Payment_NetsCOMPort";
                public const string CitiBankCOMPort = "Payment_CitiBankCOMPort";

                public const string EnableNETSCC_VISA = "Payment_EnableNETSCC_VISA";
                public const string EnableNETSCC_MASTER = "Payment_EnableNETSCC_MASTER";
                public const string EnableNETSCC_AMEX = "Payment_EnableNETSCC_AMEX";
                public const string EnableNETSCC_DINERS = "Payment_EnableNETSCC_DINERS";
                public const string EnableNETSCC_JCB = "Payment_EnableNETSCC_JCB";
                public const string PaymentNETSCC_VISA = "Payment_PaymentNETSCC_VISA";
                public const string PaymentNETSCC_MASTER = "Payment_PaymentNETSCC_MASTER";
                public const string PaymentNETSCC_AMEX = "Payment_PaymentNETSCC_AMEX";
                public const string PaymentNETSCC_DINERS = "Payment_PaymentNETSCC_DINERS";
                public const string PaymentNETSCC_JCB = "Payment_PaymentNETSCC_JCB";
                public const string PaymentNETSCC_GroupAsOne = "Payment_PaymentNETSCC_GroupAsOne";
                public const string PaymentNETSCC_Grouped = "Payment_PaymentNETSCC_Grouped";

                public const string EnableCitiBank_VISA = "Payment_EnableCitiBank_VISA";
                public const string EnableCitiBank_MASTER = "Payment_EnableCitiBank_MASTER";
                public const string EnableCitiBank_AMEX = "Payment_EnableCitiBank_AMEX";
                public const string EnableCitiBank_DINERS = "Payment_EnableCitiBank_DINERS";
                public const string EnableCitiBank_JCB = "Payment_EnableCitiBank_JCB";
                public const string PaymentCitiBank_VISA = "Payment_PaymentCitiBank_VISA";
                public const string PaymentCitiBank_MASTER = "Payment_PaymentCitiBank_MASTER";
                public const string PaymentCitiBank_AMEX = "Payment_PaymentCitiBank_AMEX";
                public const string PaymentCitiBank_DINERS = "Payment_PaymentCitiBank_DINERS";
                public const string PaymentCitiBank_JCB = "Payment_PaymentCitiBank_JCB";
                public const string PaymentCitiBank_GroupAsOne = "Payment_PaymentCitiBank_GroupAsOne";
                public const string PaymentCitiBank_Grouped = "Payment_PaymentCitiBank_Grouped";

                public const string ShowOrderRefNoOnECRReceipt = "Payment_ShowOrderRefNoOnECRReceipt";
                public const string WithCashbackOption = "Payment_WithCashbackOption";
                public const string CashbackItemNo = "Payment_CashbackItemNo";
                public const string CashPointMustUsePurchase = "Payment_CashPointMustUsePurchase";

                public const string NETSVersion = "Payment_NETSVersion";

                public const string ShowPaymentTypeWhenZeo = "Payment_ShowPaymentTypeWhenZero";
                public const string UseUOBCreditCardIntegration = "Payment_UseUOBCreditCardIntegration";
                public const string UseUOBCreditCardPassThrough = "Payment_UseUOBCreditCardPassThrough";

                public const string LastUniqueNo = "Payment_LastUniqueNo";
                
            }

            public struct Invoice
            {
                public const string EnableOutletSales = "Invoice_EnableOutletSales";
                public const string LinkPOSToMember = "Invoice_LinkPOSToMember";
                public const string EnableQuotation = "Invoice_EnableQuotation";
                public const string SelectableVoidReason = "SelectableVoidReason";
                public const string AllowAddRemarkAfterClosing = "Invoice_AllowAddRemarkAfterClosing";
                public const string SendPreOrderReceiptToEmail = "Invoice_SendPreOrderReceiptToEmail";
                public const string PreOrderReceiptTemplate = "Invoice_PreOrderReceiptTemplate";
                public const string PreOrderNotifyTemplate = "Invoice_PreOrderNotifyTemplate";

                public const string UseLastTransactionPrice = "Invoice_UseLastTransactionPrice";
                public const string IsReplaceConfirmTextButton = "IsReplaceConfirmTextButton";
                public const string ReplaceConfirmTextButtonWith = "ReplaceConfirmTextButtonWith";
                public const string EnableStockTransferAtInvoice = "Invoice_EnableStockTransferAtInvoice";

                public const string UseSeparatedRefNoPerMembershipGroup = "Invoice_UseSeparatedRefNoPerMembershipGroup";
                public const string POSCodeRefNoPosition = "Invoice_POSCodeRefNoPosition";
                public const string MemberGroupCodeRefNoPosition = "Invoice_MemberGroupCodeRefNoPosition";
                public const string RunningNoRefNoPosition = "Invoice_RunningNoRefNoPosition";
                public const string FirstRefNoSeparator = "Invoice_FirstRefNoSeparator";
                public const string SecondRefNoSeparator = "Invoice_SecondRefNoSeparator";
                public const string SeparatedRefNoLength = "Invoice_SeparatedRefNoLength";
                public const string SeparatedRefNoStartNo = "Invoice_SeparatedRefNoStartNo";
                public const string SeparatedRefNoStartIncrement = "Invoice_SeparatedRefNoStartIncrement";

                public const string EnableCreditInvoice = "Invoice_EnableCreditInvoice";
                public const string PromtPasswordOnDiscLimit = "Invoice_PromtPasswordOnDiscLimit";
                public const string UseDiscountReason = "Invoice_UseDiscountReason";
                public const string SelectableDiscountReason = "Invoice_SelectableDiscountReason";

                public const string KickCashDrawerAfterAmountEntered = "Invoice_KickCashDrawerAfterAmountEntered";
                public const string MaxChangeAllowed = "Invoice_MaxChangeAllowed";
                public const string UseWeight = "Invoice_UseWeight";

                public const string EnterAsOK = "Invoice_EnterAsOK";
                public const string PreviousReceiptNoNotCompulsory = "Invoice_PreviousReceiptNoNotCompulsory";
                public const string PromptPasswordOnSelectSalesPerson = "Invoice_PromptPasswordOnSelectSalesPerson";

                public const string QuickCashPayment = "Invoice_QuickCashPayment";
                public const string FormSetup = "Invoice_FormSetup";
                public const string UseContainerWeight = "Invoice_UseContainerWeight";

                public const string OverrideRestrictionOnWeekDays = "Invoice_OverrideRestrictionOnWeekDays";
                public const string OverrideRestrictionOnWeekEnd = "Invoice_OverrideRestrictionOnWeekEnd";

                public const string WeekDaysRestrictedStartOn = "Invoice_WeekDaysRestrictedStartOn";
                public const string WeekDaysRestrictedEndOn = "Invoice_WeekDaysRestrictedEndOn";

                public const string WeekEndRestrictedStartOn = "Invoice_WeekEndRestrictedStartOn";
                public const string WeekEndRestrictedEndOn = "Invoice_WeekEndRestrictedEndOn";

                public const string BlockTransactionIncompletePromo = "Invoice_BlockTransactionIncompletePromo";
                public const string ExternalLinkURL = "Invoice_ExternalLinkURL";
                public const string ExternalLinkText = "Invoice_ExternalLinkText";

                public const string CalculateGSTManually = "Invoice_CalculateGSTManually";
            }
            public struct Shopify
            {
                public const string PointOfSaleID = "Shopify_PointOfSaleID";
                public const string CashierID = "Shopify_CashierID";
                //public const string TableNo = "Shopify_TableNo";
                public const string PaymentType = "Shopify_PaymentType";
                public const string MembershipGroup = "Shopify_MembershipGroup";
                public const string ShippingCostItemNo = "Shopify_ShippingCostItemNo";
                public const string DefaultItemNo = "Shopify_DefaultItemNo";
            }
            public struct Item
            {
                public const string AddItemPicture = "AddItemPicture";
                public const string LowQuantityPromptInSalesScreen = "LowQuantityPromptInSalesScreen";
                public const string EnableKeyInOpenPriceItemName = "EnableKeyInOpenPriceItemName";
                public const string AllowPreOrder = "Item_AllowPreOrder";
                public const string ShowVendorDeliveryOption = "Item_ShowVendorDeliveryOption";
                
                public const string DisplayItemOpenPrice = "DisplayItemOpenPrice";
                public const string DisplayItemService = "DisplayItemService";
                public const string DisplayItemPointPackage = "DisplayItemPointPackage";
                public const string DisplayItemCourse = "DisplayItemCourse";
                public const string DisplayGiveCommission = "DisplayGiveCommission";
                public const string DisplayIsNonDiscountable = "DisplayIsNonDiscountable";
                public const string DisplayPointRedeemable = "DisplayPointRedeemable";
                public const string DisplaySupplier = "DisplaySupplier";
                public const string DisplayUOM = "DisplayUOM";
                public const string CostPriceText = "CostPriceText";
                public const string RetailPriceText = "RetailPriceText";
                public const string HideDeletedItem = "HideDeletedItem";

                public const string DisplayPrice1 = "DisplayPrice1";
                public const string DisplayPrice2 = "DisplayPrice2";
                public const string DisplayPrice3 = "DisplayPrice3";
                public const string DisplayPrice4 = "DisplayPrice4";
                public const string DisplayPrice5 = "DisplayPrice5";
                public const string NumDigit = "Item_NumDigit";
				public const string ExpiryPeriodColumn = "Item_ExpiryPeriodColumn";

                public const string ProductSetupPageSize = "Item_ProductSetupPageSize";
                public const string UseSelectableItemNameFilter = "Item_UseSelectableItemNameFilter";
                public const string UseSelectableAttributesFilter = "Item_UseSelectableAttributesFilter";

                public const string DefaultGSTSetting = "Item_DefaultGSTSetting";

                public const string UseImageItemFromLocalFolder = "UseImageItemFromLocalFolder";
                public const string ItemImageFolderLocal = "ItemImageFolderLocal";
                public const string LastBarcodeGenerated = "Item_LastBarcodeGenerated";
                public const string BarcodePrefix = "Item_BarcodePrefix";
                public const string OptionAutoGenerateBarcode = "Item_OptionAutoGenerateBarcode";
                public const string CategoryRunningNumberNoofDigit = "Item_CategoryRunningNumberNoofDigit";
                public const string AutoGenerateBarcode = "Item_AutoGenerateBarcode";
                public const string SizeImageReport = "SizeImageReport";
                public const string UseCustomerPricing = "UseCustomerPricing";

                public const string DisplayAutoCaptureWeight = "Item_DisplayAutoCaptureWeight";
                public const string DisplayNonInventoryProduct = "Item_DisplayNonInventoryProduct";

                public const string IsEditCategory_ProductOutletSetup = "IsEditCategory_ProductOutletSetup";
                public const string IsEditCostPrice_ProductOutletSetup = "IsEditCostPrice_ProductOutletSetup";
                public const string AllowOverrideItemNameOutlet = "AllowOverrideItemNameOutlet";
                //public const string AddProductOneOutletOnly = "Item_AddProductOneOutletOnly";

                public const string DisplayApplyFuturePrice = "Item_DisplayApplyFuturePrice";
                public const string DisplayMinimumSellingPrice = "Item_DisplayMinimumSellingPrice";

            }

            public struct Project
            {
                public const string ProjectModule = "ProjectModule";
            }
            
            public struct Promo
            {
                public const string AllowDiscount = "AllowDiscount";
                public const string DiscountPercentageColumn = "DiscountPercentageColumn";
                public const string IsOverwriteExistingPromoOnButtonDiscountAll = "IsOverwriteExistingPromoOnButtonDiscountAll";
                public const string PromptNonPromoItemWhenConfirm = "Promo_PromptNonPromoItemWhenConfirm";
                public const string ShowPromoNameNoDetails = "Promo_ShowPromoNameNoDetails";
                public const string EnableGrouping = "Promo_EnableGrouping";
            }

            public struct CashRecording
            {
                public const string UseOpeningBalance = "UseOpeningBalance";
                public const string DefaultOpeningBalance = "DefaultOpeningBalance";
                public const string SaveTotalCashOnlyWhenCheckOut = "SaveTotalCashOnlyWhenCheckOut";
                public const string UseSoCashIntegration = "CashRecording_UseSoCashIntegration";
                public const string SoCashItemNo = "CashRecording_SoCashItemNo";
                public const string SoCashPaymentType = "CashRecording_SoCashPaymentType";
                public const string SoCashMustUsePurchase = "CashRecording_SoCashMustUsePurchase";
                public const string EnableDailyAutoClosing = "CashRecording_EnableDailyAutoClosing";
                
            }

            public struct SecondScreen
            {
                public const string UseSecondScreen = "UseSecondScreen";
                public const string MarqueeText = "MarqueeText";
                public const string SlideShowFolder = "SlideShowFolder";
                public const string HideLogo = "SecondScreen_HideLogo";
                public const string HideItemNo = "SecondScreen_HideItemNo";
                public const string ShowChangeWaitSecond = "ShowChangeWaitSecond";
                public const string RefreshInterval = "SecondScreen_RefreshInterval";
                public const string ShowForeignCurrency = "SecondScreen_ShowForeignCurrency";
                public const string ForeignCurrency = "SecondScreen_ForeignCurrency";
                public const string RequireCustomerConfirm = "SecondScreen_RequireCustomerConfirm";
                public const string AskPrintEmailReceipt = "SecondScreen_AskPrintEmailReceipt";
                
            }

            public struct PoleDisplay
            {
                public const string PoleDisplayText = "PoleDisplayText";
                public const string UseWindcor = "UseWindcor";
                public const string FirstLineCommand = "FirstLineCommand";
                public const string SecondLineCommand = "SecondLineCommand";
                public const string PoleDisplayLinesLength = "PoleDisplayLinesLength";
                public const string DisablePoleDisplay = "PoleDisplay_DisablePoleDisplay";
            }

            public struct MallIntegration
            {
                public const string useNUHMallIntegration = "useNUHMallIntegration";
                public const string MallIntegration_TenantID = "MallIntegration_TenantID";
                public const string MallIntegration_OutputDirectory = "MallIntegration_OutputDirectory";
                public const string UseMallManagement = "MallIntegration_UseMallManagement";
                public const string InterfaceDevTeam = "MallIntegration_InterfaceDevTeam";
                public const string PointOfSaleText = "MallIntegration_PointOfSaleText";
                public const string OutletText = "MallIntegration_OutletText";
                public const string POSIDPrefix = "MallIntegration_POSIDPrefix";
                public const string TenantIDStartFrom = "MallIntegration_TenantIDStartFrom";
                public const string TenantIDIncrement = "MallIntegration_TenantIDIncrement";
                public const string POSType = "MallIntegration_POSType";
                public const string DiscrepancyPercentage = "MallIntegration_DiscrepancyPercentage";
                public const string CutOffDate = "MallIntegration_CutOffDate";
                public const string MallManagementTeamEmail = "MallIntegration_MallManagementTeamEmail";
                public const string EdgeworksTeamEmail = "MallIntegration_EdgeworksTeamEmail";
                public const string RetailerLevelAttribute1 = "MallIntegration_RetailerLevelAttribute1";
                public const string RetailerLevelAttribute2 = "MallIntegration_RetailerLevelAttribute2";
                public const string InterfaceFileName = "MallIntegration_InterfaceFileName";
                public const string InterfaceFileSource = "MallIntegration_InterfaceFileSource";
            }

            public struct EmailSender
            {
                public const string EmailSender_SMTP = "EmailSender_SMTP";
                public const string EmailSender_Port = "EmailSender_Port";
                public const string EmailSender_SenderEmail = "EmailSender_SenderEmail";
                public const string EmailSender_DefaultMailTo = "EmailSender_DefaultMailTo";
                public const string EmailSender_Username = "EmailSender_Username";
                public const string EmailSender_Password = "EmailSender_Password";
                public const string EmailSender_BccToOwner = "EmailSender_BccToOwner";
                public const string EmailSender_Cc = "EmailSender_Cc";
                public const string EmailSender_OwnerEmailAddress = "EmailSender_OwnerEmailAddress";
                public const string EmailSender_ReceiptNoInEmailReceipt = "EmailSender_ReceiptNoInEmailReceipt";
            }

            public struct CustomKickDrawer
            {
                public const string UseCustomKickDrawer = "UseCustomKickDrawer";
                public const string CustomKickDrawerAppPath = "CustomKickDrawerAppPath";
                public const string UseFlyTechCashDrawer = "UseFlyTechCashDrawer";
                public const string KickDrawerPort = "KickDrawerPort";
            }

            public struct Export
            {
                public const string ExportProductsToFileEnabled = "Export_ExportProductsToFileEnabled";
                public const string ExportProductsTemplateFile = "Export_ExportProductsTemplateFile";
                public const string ExportProductsDirectory = "Export_ExportProductsDirectory";
                public const string ExportProductsFileName = "Export_ExportProductsFileName";
                public const string ExportProductsFilter = "Export_ExportProductsFilter";
            }

            public struct Magento
            {
                public const string UseMagentoFeatures = "UseMagentoFeatures";
                public const string MagentoURL = "MagentoURL";
                public const string MagentoUser = "MagentoUser";
                public const string MagentoPassword = "MagentoPassword";
                public const string CategoryRootID = "CategoryRootID";
                public const string DefaultCustStreet = "Magento_DefaultCustStreet";
                public const string DefaultCustTelephone = "Magento_DefaultCustTelephone";
                public const string DefaultCustPostCode = "Magento_DefaultCustPostCode";
                public const string DefaultCustCountryID = "Magento_DefaultCustCountryID";
                public const string DefaultCustRegion = "Magento_DefaultCustRegion";
                public const string DefaultCustCity = "Magento_DefaultCustCity";
                public const string DefaultPaymentType = "Magento_DefaultPaymentType"; 

                public const string DefaultCustEmail = "Magento_DefaultCustEmail";
                public const string DefaultShippingMethod = "Magento_DefaultShippingMethod";
                public const string DefaultStore = "Magento_DefaultStore";

                public const string ViewAppointmentEnable = "Magento_ViewAppointmentEnable";
                public const string ViewAppointmentURL = "Magento_ViewAppointmentURL";
                public const string ViewAppointmentUser = "Magento_ViewAppointmentUser";
                public const string ViewAppointmentPassword = "Magento_ViewAppointmentPassword";
                public const string ViewAppointmentText = "Magento_ViewAppointmentText";
            }

            public struct Discount
            {
                public const string P1DiscountName = "P1DiscountName";
                public const string P2DiscountName = "P2DiscountName";
                public const string P3DiscountName = "P3DiscountName";
                public const string P4DiscountName = "P4DiscountName";
                public const string P5DiscountName = "P5DiscountName";
                public const string ShowDiscountDescription = "ShowDiscountDescription";
                public const string UseRoundingForFinalPrice = "UseRoundingForFinalPrice";
                public const string AllowZeroMultiTierPrice = "Discount_AllowZeroMultiTierPrice";
                public const string EnableSecondDiscount = "EnableSecondDiscount";
                public const string EnableMultiLevelPricing = "Discount_EnableMultiLevelPricing";
                public const string EnableMultiLevelPricingInGlobalDiscount = "Discount_EnableMultiLevelPricingInGlobalDiscount";
                public const string DiscountReportShowSearchDiscountReason = "DiscountReportShowSearchDiscountReason";
                public const string DisableGiveLineDiscountSalesInv = "Discount_DisableGiveLineDiscountSalesInv";
            }

            public struct Sync
            {
                public const string UseRealTimeSales = "UseRealTimeSales";
                public const string SyncRecordsPerTime = "SyncRecordsPerTime";
                public const string SalesRetrySecWhenDisconnected = "SalesRetrySecWhenDisconnected";

                public const string UseRealTimeInventory = "UseRealTimeInventory";
                public const string InventoryRetrySecWhenConnected = "InventoryRetrySecWhenConnected";
                public const string InventoryRetrySecWhenDisconnected = "InventoryRetrySecWhenDisconnected";
                public const string InventorySyncRecordsPerTime = "InventorySyncRecordsPerTime";

                public const string UseRealTimeSyncLogs = "Sync_UseRealTimeSyncLogs";
                public const string SyncLogsPerTime = "Sync_SyncLogsPerTime";
                public const string SyncLogsRetrySecWhenDisconnected = "Sync_SyncLogsRetrySecWhenDisconnected";

                public const string UseRealTimeSyncQuotation = "Sync_UseRealTimeSyncQuotation";
                public const string SyncQuotationPerTime = "Sync_SyncQuotationPerTime";
                public const string SyncQuotationRetrySecWhenDisconnected = "Sync_SyncQuotationRetrySecWhenDisconnected";

                public const string UseRealTimeSyncCashRecording = "Sync_UseRealTimeSyncCashRecording";
                public const string SyncCashRecordingPerTime = "Sync_SyncCashRecordingPerTime";
                public const string SyncCashRecordingRetrySecWhenDisconnected = "Sync_SyncCashRecordingRetrySecWhenDisconnected";

                public const string UseRealTimeSyncMasterData = "UseRealTimeSyncMasterData";
                public const string SyncMasterDataRecordsPerTime = "SyncMasterDataRecordsPerTime";
                public const string MasterDataRetrySecWhenConnected = "MasterDataRetrySecWhenConnected";
                public const string MasterDataRetrySecWhenDisconnected = "MasterDataRetrySecWhenDisconnected";

                public const string UseRealTimeSyncUser = "UseRealTimeSyncUser";
                public const string SyncUserRecordsPerTime = "SyncUserRecordsPerTime";
                public const string UserRetrySecWhenConnected = "UserRetrySecWhenConnected";
                public const string UserRetrySecWhenDisconnected = "UserRetrySecWhenDisconnected";

                public const string UseRealTimeSyncMember = "UseRealTimeSyncMember";
                public const string SyncMemberRecordsPerTime = "SyncMemberRecordsPerTime";
                public const string MemberRetrySecWhenConnected = "MemberRetrySecWhenConnected";
                public const string MemberRetrySecWhenDisconnected = "MemberRetrySecWhenDisconnected";

                public const string UseRealTimeSyncProducts = "UseRealTimeSyncProducts";
                public const string SyncProductRecordsPerTime = "SyncProductRecordsPerTime";
                public const string ProductRetrySecWhenDisconnected = "ProductRetrySecWhenDisconnected";
                public const string ProductRetrySecWhenConnected = "ProductRetrySecWhenConnected";

                public const string UseRealTimeSyncItemSummary = "UseRealTimeSyncItemSummary";
                public const string SyncItemSummaryRecordsPerTime = "SyncItemSummaryRecordsPerTime";
                public const string ItemSummaryRetrySecWhenDisconnected = "ItemSummaryRetrySecWhenDisconnected";
                public const string ItemSummaryRetrySecWhenConnected = "ItemSummaryRetrySecWhenConnected";

                public const string SyncAccessLog = "Sync_RealTimeAccessLog";
                public const string SyncAccessLogPerTime = "Sync_SyncAccessLogPerTime";
                public const string SyncAccessLogRetrySecWhenDisconnected = "Sync_SyncAccessLogRetrySecWhenDisconnected";

                public const string POSAppSettingSyncList = "Sync_POSAppSettingSyncList";
                //public const string SendAppointmentDirectlyToServer = "SendAppointmentDirectlyToServer";

                public const string UseRealTimeSyncAppointment = "UseRealTimeSyncAppointment";
                public const string SyncAppointmentRecordsPerTime = "SyncAppointmentRecordsPerTime";
                public const string AppointmentRetrySecWhenConnected = "AppointmentRetrySecWhenConnected";
                public const string AppointmentRetrySecWhenDisconnected = "AppointmentRetrySecWhenDisconnected";

                public const string UseRealTimeSyncPerformanceLog = "Sync_UseRealTimeSyncPerformanceLog";
                public const string SyncPerformanceLogPerTime = "Sync_SyncPerformanceLogPerTime";
                public const string SyncPerformanceLogRetrySecWhenDisconnected = "Sync_SyncPerformanceLogRetrySecWhenDisconnected";

                public const string UseRealTimeSyncDeliveryOrder = "UseRealTimeSyncDeliveryOrder";
                public const string SyncDeliveryOrderRecordsPerTime = "SyncDeliveryOrderRecordsPerTime";
                public const string DeliveryOrderRetrySecWhenConnected = "DeliveryOrderRetrySecWhenConnected";
                public const string DeliveryOrderRetrySecWhenDisconnected = "DeliveryOrderRetrySecWhenDisconnected";

                public const string UseRealTimeSyncAttendance = "UseRealTimeSyncAttendance";
                public const string SyncAttendanceRecordsPerTime = "SyncAttendanceRecordsPerTime";
                public const string AttendanceRetrySecWhenDisconnected = "AttendanceRetrySecWhenDisconnected";

                public const string UseRealTimeSyncRating = "UseRealTimeSyncRating";
                public const string SyncRatingRecordsPerTime = "SyncRatingRecordsPerTime";
                public const string RatingRetrySecWhenConnected = "RatingRetrySecWhenConnected";
                public const string RatingRetrySecWhenDisconnected = "RatingRetrySecWhenDisconnected";

                public const string UseRealTimeSyncVoucher = "UseRealTimeSyncVoucher";
                public const string SyncVoucherRecordsPerTime = "SyncVoucherRecordsPerTime";
                public const string VoucherRetrySecWhenConnected = "VoucherRetrySecWhenConnected";
                public const string VoucherRetrySecWhenDisconnected = "VoucherRetrySecWhenDisconnected";

                public const string MaxRetryWhenError = "MaxRetryWhenError";

                public const string OtherItemNo = "OtherItemNo";
            }

            public struct SpecialBarcode
            {
                public const string UseSpecialBarcode = "UseSpecialBarcode";
                public const string BarcodeCheckDigit = "BarcodeCheckDigit";
                public const string BarcodeCheckValue = "BarcodeCheckValue";
                public const string ItemDigitStart = "ItemDigitStart";
                public const string ItemDigitLength = "ItemDigitLength";

                public const string PriceDigitStart = "PriceDigitStart";
                public const string PriceDigitLength = "PriceDigitLength";
                public const string RecordedDigitStart = "RecordedDigitStart";
                public const string RecordedDigitLength = "RecordedDigitLength";

                public const string QuantityDigitStart = "QuantityDigitStart";
                public const string QuantityDigitLength = "QuantityDigitLength";
                public const string IntegerDigitLength = "IntegerDigitLength";

                public const string UseSpecialBarcodeForPrice = "UseSpecialBarcodeForPrice";
                public const string UseSpecialBarcodeForQuantity = "UseSpecialBarcodeForQuantity";
            }

            public struct PurchaseOrder
            {
                public const string POMailCC = "PurchaseOrder_POMailCC";
                public const string POMailSubject = "PurchaseOrder_POMailSubject";
                public const string POMailContent = "PurchaseOrder_POMailContent";
                public const string ShowDeliveryAddress = "PurchaseOrder_ShowDeliveryAddress";
                public const string ShowDeliveryDateTime = "PurchaseOrder_ShowDeliveryDateTime";
                public const string ShowGST = "PurchaseOrder_ShowGST";
                public const string ShowPaymentType = "PurchaseOrder_ShowPaymentType";
                public const string ShowPackingSize = "PurchaseOrder_ShowPackingSize";
                public const string ShowOrderQty = "PurchaseOrder_ShowOrderQty";
                public const string ShowUOM = "PurchaseOrder_ShowUOM";
                public const string ShowCurrency = "PurchaseOrder_ShowCurrency";
                public const string ShowRetailPrice = "PurchaseOrder_ShowRetailPrice";
                public const string ShowFactoryPrice = "PurchaseOrder_ShowFactoryPrice";
                public const string ShowSupplier = "PurchaseOrder_ShowSupplier";
                public const string ShowStatus = "PurchaseOrder_ShowStatus";
                public const string ShowLoadAddressBtn = "PurchaseOrder_ShowLoadAddressBtn";
                public const string AllowSameItemMultipleTimes = "PurchaseOrder_AllowSameItemMultipleTimes";

                public const string PurchaseOrderRole = "PurchaseOrder_PurchaseOrderRole";
                public const string PurchaseOrderCompany = "PurchaseOrder_PurchaseOrderCompany";
                public const string IsSellingPriceEditable = "PurchaseOrder_IsSellingPriceEditable";
                public const string IsCostPriceEditable = "PurchaseOrder_IsCostPriceEditable";
                public const string IsCostPerPackingSizeEditable = "PurchaseOrder_IsCostPerPackingSizeEditable";
                public const string AllowLoadUnapprovedPOInGR = "PurchaseOrder_AllowLoadUnapprovedPOInGR";
                public const string SearchItemBySupplier = "PurchaseOrder_SearchItemBySupplier";
                public const string ShowMinPurchase = "PurchaseOrder_ShowMinPurchase";
                public const string ShowDeliveryCharge = "PurchaseOrder_ShowDeliveryCharge";
                public const string ShowOrderInfo = "PurchaseOrder_ShowOrderInfo";
                public const string frmAddItemWithFilter_clbAttributes = "PurchaseOrder_frmAddItemWithFilter_clbAttributes";
                public const string frmAddItemWithFilter_chkShowSales = "PurchaseOrder_frmAddItemWithFilter_chkShowSales";
                public const string frmAddItemWithFilter_txtNumOfDays = "PurchaseOrder_frmAddItemWithFilter_txtNumOfDays";

                public const string UseCustomNo = "PurchaseOrder_UseCustomNo";
                public const string CustomPrefix = "PurchaseOrder_CustomPrefix";
                public const string CustomSuffix = "PurchaseOrder_CustomSuffix";
                public const string NumberLength = "PurchaseOrder_NumberLength";
                public const string CurrentNo = "PurchaseOrder_CurrentNo";
                public const string ResetNumberEvery = "PurchaseOrder_ResetNumberEvery";
                public const string CustomNoDateFormat = "PurchaseOrder_CustomNoDateFormat";
                public const string LastReset = "PurchaseOrder_LastReset";
                public const string GenerateCustomNoInServer = "PurchaseOrder_GenerateCustomNoInServer";

                public const string EnableAttachment = "PurchaseOrder_EnableAttachment";
                public const string AttachmentMaxFileSize = "PurchaseOrder_AttachmentMaxFileSize";

                public const string AutoUpdateCostPriceOnSupplierPOApproval = "AutoUpdateCostPriceOnSupplierPOApproval";
            }

            public struct ItemSupplierMap
            {
                public const string DisplayCurrencyOnItemSupplierMap = "DisplayCurrencyOnItemSupplierMap";
                public const string DisplayGSTOnItemSupplierMap = "DisplayGSTOnItemSupplierMap";
                public const string MaxPackingSizeOnItemSupplierMap = "MaxPackingSizeOnItemSupplierMap";
                public const string AvailableCurrency = "AvailableCurrency";
                public const string DefaultCurrency = "DefaultCurrency";
            }

            public struct Supplier
            {
                public const string DisplayCurrencyOnSupplier = "DisplayCurrencyOnSupplier";
                public const string DisplayGSTOnSupplier = "DisplayGSTOnSupplier";
                public const string DisplayMinimumOrderOnSupplier = "DisplayMinimumOrderOnSupplier";
                public const string DisplayDeliveryChargeOnSupplier = "DisplayDeliveryChargeOnSupplier";
                public const string DisplayPaymentTermOnSupplier = "DisplayPaymentTermOnSupplier";
            }

            public struct LineInfo
            {
                public const string ShowInInvoice = "LineInfo_ShowInInvoice";
                public const string DropdownCanAddNew = "LineInfo_DropdownCanAddNew";
                public const string ReplaceTextWith = "LineInfo_ReplaceTextWith";
                public const string ShowInViewReceipt = "LineInfo_ShowInViewReceipt";
                public const string IsMandatory = "LineInfo_IsMandatory";
                public const string MandatoryType = "LineInfo_MandatoryType";
            }

            public struct Interface
            {
                public const string HideSalesTab = "Interface_HideSalesTab";
                public const string HideListingTab = "Interface_HideListingTab";
                public const string HideInventoryTab = "Interface_HideInventoryTab";
                public const string ReplaceGSTTextWith = "Interface_ReplaceGSTTextWith";
            }

            public struct Order
            {
                public const string PromptSelectOrderType = "Order_PromptSelectOrderType";
                public const string ReplaceCashCarryTextWith = "Order_ReplaceCashCarryTextWith";
                public const string ReplacePreOrderTextWith = "Order_ReplacePreOrderTextWith";
                public const string AllowSplitDelivery = "Order_AllowSplitDelivery";
                public const string DepositAssignmentValidation = "Order_DepositAssignmentValidation";
                public const string AutoAssignDepositUponInstPayment = "Order_AutoAssignDepositUponInstPayment";
                public const string AutoCancelDeliveryItemIfReturned = "Order_AutoCancelDeliveryItemIfReturned";
                public const string ShowVendorDelivery = "Order_ShowVendorDelivery";
                public const string ShowDeliveryOutlet = "Order_ShowDeliveryOutlet";
                public const string AutoAssignMode = "Order_AutoAssignMode";
                public const string AutoAssignWeightAge = "Weightage";
                public const string AutoAssignFirstItem = "First Item";
                public const string AutoAssignNo = "No Auto Assign";
                public const string DoNotRoundDiscountedPrice = "Order_DoNotRoundDiscountedPrice";
                public const string ShowReminderWhenConfirm = "Order_ShowReminderWhenConfirm";
                public const string ReminderMessage = "Order_ReminderMessage";
                public const string OnlySearchItemNoItemName = "Order_OnlySearchItemNoItemName";
                public const string IsRoundingPreferenceAccordingToConfig = "IsRoundingPreferenceAccordingToConfig";
                public const string SkipDeliverySetupScreen = "Order_SkipDeliverySetupScreen";
                public const string AutoCreateWhenSkipDelivery = "Order_AutoCreateWhenSkipDelivery";
                public const string EnableSecondSalesPerson = "Order_EnableSecondSalesPerson";
                public const string AllowBackdatedSales = "Order_AllowBackdatedSales";
                public const string BackdateMaxDays = "Order_BackdateMaxDays";
                public const string EnablePerformanceLog = "Order_EnablePerformanceLog";
                public const string UseUserTokenToGiveDiscount ="Order_UseUserTokenToGiveDiscount";
                public const string ClearKeyboardBufferTime = "Order_ClearKeyboardBufferTime";
            public const string ChangeMemberWithoutSupervisorLogin = "Order_ChangeMemberWithoutSupervisorLogin";
            }

            public struct Receipt
            {
                public const string DO_UseCustomNo = "DO_UseCustomNo";
                public const string DO_CustomPrefix = "DO_CustomPrefix";
                public const string DO_ReceiptLength = "DO_ReceiptLength";
                public const string DO_CurrentReceiptNo = "DO_CurrentReceiptNo";
                public const string A4PrinterName = "A4PrinterName";

                public const string Quotation_UseCustomNo = "Quotation_UseCustomNo";
                public const string Quotation_CustomPrefix = "Quotation_CustomPrefix";
                public const string Quotation_ReceiptLength = "Quotation_ReceiptLength";
                public const string Quotation_CurrentReceiptNo = "Quotation_CurrentReceiptNo";

                public const string ShowReceiptNoBarcode = "Receipt_ShowReceiptNoBarcode";
                public const string BarcodeFontName = "Receipt_BarcodeFontName";
                public const string BarcodeFontSize = "Receipt_BarcodeFontSize";
                public const string UseOrderHdrIDAsBarcode = "Receipt_UseOrderHdrIDAsBarcode";

                public const string HideActualCollection = "HideActualCollection";
                public const string ReprintWord = "Receipt_ReprintWord";
                public const string DefaultReprintNumCopies = "Receipt_DefaultReprintNumCopies";

                public const string AmountToPrintSignature = "Receipt_AmountToPrintSignature";
            }

            public struct Funding
            {
                public const string EnableFunding = "Funding_EnableFunding";
                public const string EnablePAMed = "Funding_EnablePAMed";
                public const string EnableSMF = "Funding_EnableSMF";
                public const string EnablePWF = "Funding_EnablePWF";
                public const string PAMedPercentage = "Funding_PAMedPercentage";
                public const string SMFPercentage = "Funding_SMFPercentage";
            }

            public struct Z2Report
            {
                public const string ShowClosingBreakdownOnZ2Printout = "ShowClosingBreakdownOnZ2Printout";
                public const string PrintCategorySalesReportOnZ2Printout = "PrintCategorySalesReportOnZ2Printout";
                public const string PrintProductSalesReportOnZ2Printout = "PrintProductSalesReportOnZ2Printout";
            }

            public struct FormalInvoice
            {
                public const string UseSpecialGSTRuleForFormal = "UseSpecialGSTRuleForFormal";
                public const string SpecialGSTRule = "SpecialGSTRule";
            }

            public struct WeighingMachine
            {
                public const string UseWeighingMachine = "WeighingMachine_UseWeighingMachine";
                public const string COMPort = "WeighingMachine_COMPort";
                public const string CommandToExecute = "WeighingMachine_CommandToExecute";
                public const string SampleWeighingScale = "10";
                public const string Tolerance = "4";
            }

            public struct Refund
            {
                public const string RefundReceiptFromSameOutlet = "Refund_RefundReceiptFromSameOutlet";
                public const string RefundReceiptFromOtherOutlet = "Refund_RefundReceiptFromOtherOutlet";
            }

            public struct GuestBook
            {
                public const string GuestBookName = "GuestBookName";
                public const string ShowPrefixMembershipOutlet = "ShowPrefixMembershipOutlet";
            }

            public struct SalesInvoice
            {
                public const string UseCustomInvoiceNo = "UseCustomInvoiceNo";
                public const string HideDiscountColumn = "SalesInvoice_HideDiscountColumn";
                public const string HideSalespersonColumn = "SalesInvoice_HideSalespersonColumn";
                public const string HideSpecialColumn = "SalesInvoice_HideSpecialColumn";
                public const string HideTaxColumn = "SalesInvoice_HideTaxColumn";
                public const string SortHotKeyByName = "SalesInvoice_SortHotKeyByName";
                public const string ShowUomInEditQtyForm = "SalesInvoice_ShowUomInEditQtyForm";
                public const string AllowChangeGSTonSales = "AllowChangeGSTonSales";
                public const string WaitUntilCashDrawerClosed = "SalesInvoice_WaitUntilDrawerClosed";
                public const string MaxHoldTransaction = "SalesInvoice_MaxHoldTransaction";
                public const string RoundingForAllPayment = "SalesInvoice_RoundingForAllPayment";
                public const string ShowPossiblePromo = "SalesInvoice_ShowPossiblePromo";
                public const string FontSize = "SalesInvoice_FontSize";
                public const string ResetPriceModeAfterScanItem = "SalesInvoice_ResetPriceModeAfterScanItem";
                public const string ShowPreviewButton = "SalesInvoice_ShowPreviewButton";
                public const string ShowFOCColumn = "ShowFOCColumn";
                public const string ShowPreOrderColumn = "ShowPreOrderColumn";
                public const string DisableOpenPricePrompt = "SalesInvoice_DisableOpenPricePrompt";
                public const string PromptPasswordClearOrder = "SalesInvoice_PromptPasswordClearOrder";
                public const string ShowItemNoInHotKeys = "SalesInvoice_ShowItemNoInHotKeys";
                public const string AllowEditPriceTwoDecimal = "AllowEditPriceTwoDecimal";
				public const string UseSingleOrderScreen = "SalesInvoice_UseSingleOrderScreen";
                public const string UseKeypadForItemKeyboard = "SalesInvoice_UseKeypadForItemKeyboard";
                public const string DisableHotKeys = "SalesInvoice_DisableHotKeys";
                public const string DisableChangeRetailPrice = "SalesInvoice_DisableChangeRetailPrice";
                public const string DisableChangeDiscountedPrice = "SalesInvoice_DisableChangeDiscountedPrice";
                public const string RemoveItemOnLineVoid = "SalesInvoice_RemoveItemOnLineVoid";

                public const string ExcludeItemQtyForCategory = "SalesInvoice_ExcludeItemQtyForCategory";
                public const string ExcludedCategoryName = "SalesInvoice_ExcludedCategoryName";
                public const string PromptAmountForPayment = "SalesInvoice_PromptAmountForPayment";
                public const string HideCategoryForHotkeys = "SalesInvoice_HideCategoryForHotkeys";
                public const string FilteredCategoryName = "SalesInvoice_FilteredCategoryName";
            }
			
			public struct Mobile
            {
                public const string DisplayCost = "Mobile_DisplayCost";
                public const string DisplayBatchNo = "Mobile_DisplayBatchNo";
                public const string DisplayShelf = "Mobile_DisplayShelf";
                public const string EnableStockIn = "Mobile_EnableStockIn";
                public const string EnableStockOut = "Mobile_EnableStockOut";
                public const string EnableStockTake = "Mobile_EnableStockTake";
                public const string EnableStockTransfer = "Mobile_EnableStockTransfer";
                public const string EnablePO = "Mobile_EnablePO";
                public const string EnableStockInFromPO = "Mobile_EnableStockInFromPO";
                public const string EnableRecordData = "Mobile_EnableRecordData";
                public const string RecordData1 = "Mobile_RecordData1";
                public const string RecordData2 = "Mobile_RecordData2";
                public const string RecordData3 = "Mobile_RecordData3";
                public const string RecordData4 = "Mobile_RecordData4";
                public const string RecordData5 = "Mobile_RecordData5";
                public const string RecordData6 = "Mobile_RecordData6";
                public const string RecordData7 = "Mobile_RecordData7";
                public const string RecordData8 = "Mobile_RecordData8";
                public const string RecordData9 = "Mobile_RecordData9";
                public const string RecordData10 = "Mobile_RecordData10";
                public const string StockInSaveToFile = "Mobile_StockInSaveToFile";
                public const string UpdateProductApplicableTo = "Mobile_UpdateProductApplicableTo";
            }

            public struct LowQtyWarning
            {
                public const string LowQtyUserfld1 = "LowQtyUserfld1";
                public const string LowQtyUserfld2 = "LowQtyUserfld2";
                public const string LowQtyUserfld3 = "LowQtyUserfld3";
                public const string LowQtyUserfld4 = "LowQtyUserfld4";
                public const string LowQtyUserfld5 = "LowQtyUserfld5";
                public const string LowQtyUserfld6 = "LowQtyUserfld6";
                public const string LowQtyUserfld7 = "LowQtyUserfld7";
                public const string LowQtyUserfld8 = "LowQtyUserfld8";
                public const string LowQtyUserfld9 = "LowQtyUserfld9";
                public const string LowQtyUserfld10 = "LowQtyUserfld10";
            }

            public struct Rating
            {
                public const string GreetingText = "Rating_GreetingText";
                public const string FooterText = "Rating_FooterText";
                public const string ThankYouGoodRating = "Rating_ThankYouGoodRating";
                public const string ThankYouBadRating = "Rating_ThankYouBadRating";
                public const string ThankYouInterval = "Rating_ThankYouInterval";
                public const string AllowGoodRatingFeedback = "Rating_AllowGoodRatingFeedback";
                public const string GoodFeedbackGreeting = "Rating_GoodFeedbackGreeting";
                public const string AllowBadRatingFeedback = "Rating_AllowBadRatingFeedback";
                public const string BadFeedbackGreeting = "Rating_BadFeedbackGreeting";
                public const string UseRatingSystem = "Rating_UseRatingSystem";
            }

            public struct SICC  
            {
                public const string DefaultPrinterName = "SICC_DefaultPrinterName";
                public const string CheckValidationPrinting = "SICC_CheckValidationPrinting";
                public const string ValidationX = "SICC_ValidationX";
                public const string ValidationY = "SICC_ValidationY";
                public const string ValidationMode = "SICC_ValidationMode";
                public const string ValidationNumOfCopies = "SICC_ValidationNumOfCopies";
            }

            public struct Reports
            {
                public const string AggregatedSalesReportMaxHistory = "Reports_AggregatedSalesReportMaxHistory";
                public const string ZXV3UploadDirectory = "Reports_ZXV3UploadDirectory";
                public const string ShowPointInstallmentBreakdownInDailySales = "Reports_ShowPointInstallmentBreakdownInDailySales";
                public const string SeparateInstallmentPaymentInDailySales = "Reports_SeparateInstallmentPaymentInDailySales";
            }

            public struct FTP
            {
                public const string Protocol = "FTP_Protocol";
                public const string Host = "FTP_Host";
                public const string Port = "FTP_Port";
                public const string Username = "FTP_Username";
                public const string Password = "FTP_Password";
                public const string PassiveMode = "FTP_PassiveMode";
            }

            public struct AuditLog
            {
                public const string ProductSetup = "AuditLog_ProductSetup";
                public const string SetupProductPromotion = "AuditLog_SetupProductPromotion";
            }

            public struct CashRecycler
            {
                public const string EnableCashRecycler = "CashRecycler_EnableCashRecycler";
                public const string MachineType = "CashRecycler_MachineType";
                public const string APIUrl = "CashRecycler_APIUrl";
                public const string APIPort = "CashRecycler_APIPort";
                public const string Username = "CashRecycler_Username";
                public const string Password = "CashRecycler_Password";
                public const string COMPortCashMachine = "CashRecycler_COMPortCashMachine";
                public const string ChangeTimeout = "CashRecycler_ChangeTimeout";
                public const string UseCashMachineIntegration = "CashRecycler_UseCashMachineIntegration";
            }

            public struct BarcodeScanner
            {
                public const string disableCommand = "BarcodeScanner_DisableCommand";
                public const string enableCommand = "BarcodeScanner_EnableCommand";
                public const string COMPort = "BarcodeScanner_COMPort";
                public const string Enabled = "BarcodeScanner_Enabled";
                public const string Bypass = "BarcodeScanner_Bypass";
                public const string DelayInSeconds = "BarcodeScanner_DelayInSeconds";
            }

            public struct Kiosk
            {
                public const string KioskMode = "Kiosk_KioskMode";
                public const string EnableNotes = "Kiosk_EnableNotes";
                public const string MachineModelNotes = "Kiosk_MachineModelNotes";
                public const string COMPortNotes = "Kiosk_COMPortNotes";
                public const string EnableCoins = "Kiosk_EnableCoins";
                public const string MachineModelCoins = "Kiosk_MachineModelCoins";
                public const string COMPortCoins = "Kiosk_COMPortCoins";
                public const string EnableNETS = "Kiosk_EnableNETS";
                public const string COMPortNETS = "Kiosk_COMPortNETS";
                public const string SimulatorWeighingScale = "Kiosk_SimulatorWeighingScale";
                public const string SimulatorNETS = "Kiosk_SimulatorNETS";
                public const string IdleTimes = "Kiosk_IdleTimes";
                public const string UseCHSLang = "Kiosk_UseCHSLang";
                public const string ClearKeyboardBufferTime = "Kiosk_ClearKeyboardBufferTime";
                public const string HideLogButton = "Kiosk_HideLogButton";
                public const string KioskStatus = "Kiosk_Status";
                public const string ThankYouInterval = "Kiosk_ThankYouInterval";
            }

            public struct Commission
            {
                public const string GiveCommissionUponPayment = "Commission_GiveCommissionUponPayment";
                public const string CommissionBasedOn = "Commission_BasedOn";
            }

            public struct Report
            {
                public const string UseDataWarehouse = "Report_UseDataWarehouse";
                public const string DefaultCutOffHour = "Report_DefaultCutOffHour";
                public const string ShowGrossAmount = "ReportTransactionReport_ShowGrossAmount";
            }

            public struct BarcodePrinter
            {
                public const string CustomQtyActive = "BarcodePrinter_CustomQtyActive";
                public const string CustomQtyOrigin = "BarcodePrinter_CustomQtyOrigin";
                public const string Template = "BarcodePrinter_Template";
                public const string PrinterName = "BarcodePrinter_PrinterName";
                public const string LabelWidth = "BarcodePrinter_LabelWidth";
                public const string NumberOfColumns = "BarcodePrinter_NumberOfColumns";
                public const string UsePriceLevel = "BarcodePrinter_UsePriceLevel";
            }

            public struct GoodsOrdering
            {
                public const string AutoCreateSupplierPOUponOutletOrderApproval = "AutoCreateSupplierPOUponOutletOrderApproval";
                public const string AutoApproveSupplierPO = "GoodsOrdering_AutoApproveSupplierPO";
                public const string ShowPriceLevelForWebOrder = "GoodsOrdering_ShowPriceLevelForWebOrder";
                public const string AllowCreateInvoiceForStockTransferAndGoodsOrdering = "AllowCreateInvoiceForStockTransferAndGoodsOrdering";
                public const string StockReturnWillReturnStockToWarehouse = "StockReturnWillReturnStockToWarehouse";
                public const string StockTransferWillGoThroughWarehouse = "StockTransferWillGoThroughWarehouse";
                public const string ShowSalesGR = "GoodsOrdering_ShowSalesGR";
                public const string AutoApproveOrder = "AutoApproveOrder";
                public const string ShowFactoryPriceInGoodsOrdering = "GoodsOrdering_ShowFactoryPriceInGoodsOrdering";
                public const string ShowFactoryPriceInOrderApproval = "GoodsOrdering_ShowFactoryPriceInOrderApproval";
                public const string ShowFactoryPriceInReturnApproval = "GoodsOrdering_ShowFactoryPriceInReturnApproval";
                public const string ShowFactoryPriceInTransferApproval = "GoodsOrdering_ShowFactoryPriceInTransferApproval";
                public const string UseTransferApproval = "GoodsOrdering_UseTransferApproval";
                public const string ShowPrintDOButtonInGoodsOrdering = "GoodsOrdering_ShowPrintDOButtonInGoodsOrdering";
                public const string RangeSalesShownGR = "GoodsOrdering_RangeSalesShownGR";                
                public const string AllowOutletToOrderFromSupplier = "GoodsOrdering_AllowOutletToOrderFromSupplier";
                public const string AllowDeductInvQtyNotSufficient = "GoodsOrdering_AllowDeductInvQtyNotSufficient";
                public const string HideQtyInOutlet = "GoodsOrdering_HideQtyInOutlet";
                public const string StatusAllTallyReceived = "GoodsOrdering_StatusAllTallyReceived";

                public const string AutoGenerateInvoiceNo = "GoodsOrdering_AutoGenerateInvoiceNo";
                public const string AutoGenerateInvoiceNoPrefix = "GoodsOrdering_AutoGenerateInvoiceNoPrefix";
                public const string AutoGenerateInvoiceLength = "GoodsOrdering_AutoGenerateInvoiceLength";
                public const string InvoiceGSTRule = "GoodsOrdering_InvoiceGSTRule";
            }

            public struct Recipe
            {
                public const string EnableRecipeManagement = "Recipe_EnableRecipeManagement";
            }

            public struct Integration
            {
                public const string UseCustomFieldForItemNo = "Integration_UseCustomFieldForItemNo";
                public const string CustomFieldName = "Integration_CustomFieldName";
                public const string NeedToSyncSale = "Integration_NeedToSyncSale";
                public const string NeedToSyncProduct = "Integration_NeedToSyncProduct";
                public const string NeedToSyncCustomer = "Integration_NeedToSyncCustomer";
                public const string SyncCreateDeliveryOrder = "Integration_SyncCreateDeliveryOrder";
            }

            public struct CRMIntegration
            {
                public const string IsEnabled = "CRMIntegration_IsEnabled";
                public const string Type = "CRMIntegration_Type";
                public const string APIServer = "CRMIntegration_APIServer";
                public const string TerminalID = "CRMIntegration_TerminalID";
                public const string TerminalPwd = "CRMIntegration_TerminalPwd";
                public const string APIKey = "CRMIntegration_APIKey";
                public const string MerchantName = "CRMIntegration_MerchantName";
                public const string StaffName = "CRMIntegration_StaffName";
                public const string StaffCode = "CRMIntegration_StaffCode";
                public const string ItemNoForVoucher = "CRMIntegration_ItemNoForVoucher";
            }
			
            public struct ViewSales
            {
                public const string HideExportInventory = "ViewSales_HideExportInventory";
                public const string HideChangePayment = "ViewSales_HideChangePayment";
                public const string HideReprint = "ViewSales_HideReprint";
                public const string HideEmailReceipt = "ViewSales_HideEmailReceipt";
            }
			
			public struct CashMachineIntegration
            {
                public const string UseCashMachineIntegration = "UseCashMachineIntegration";
                public const string ModelCashMachineIntegration = "ModelCashMachineIntegration";
                public const string COMPortCashMachine = "COMPortCashMachine";
            }
        }

        public void LoadSetting(string SettingKey)
        {
            SetSQLProps();
            InitSetDefaults();
            try
            {
                Query qr = new Query("AppSetting");
                qr.QueryType = QueryType.Select;
                qr.Top = "1";
                qr.AddWhere(Columns.OutletName, Comparison.Is, null);
                qr.AddWhere(Columns.AppSettingKey, Comparison.Equals, SettingKey);

                LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
        public void LoadSetting(string SettingKey, string OutletName)
        {
            SetSQLProps();
            InitSetDefaults();
            try
            {
                Query qr = new Query("AppSetting");
                qr.QueryType = QueryType.Select;
                qr.Top = "1";
                qr.AddWhere(Columns.OutletName, Comparison.Equals, OutletName);
                qr.AddWhere(Columns.AppSettingKey, Comparison.Equals, SettingKey);

                LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        public void LoadVersion()
        {
            LoadSetting(SettingsName.Database_Version);
        }
        //This method is to check app setting DB table for setting 
        //if the entry is not there, will check the app config file
        //main purpose is for backward compatibility
        public static string GetSettingFromDBAndConfigFile(string SettingKey)
        {
            try
            {
                if (GetSetting(SettingKey) != null)
                {
                    return GetSetting(SettingKey).ToString();
                }
                else
                {
                    if (System.Configuration.ConfigurationManager.AppSettings[SettingKey] != null)
                    {
                        return System.Configuration.ConfigurationManager.AppSettings[SettingKey].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return "";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SettingKey"></param>
        /// <returns>If the setting is not exists, return NULL</returns>
        /// 
        public static string GetSetting(string SettingKey)
        {
            try
            {

                string sqlGetData = "SELECT TOP 1 * FROM AppSetting WHERE OutletName IS NULL AND AppSettingKey = @AppSettingKey";

                QueryCommand cmdGetData = new QueryCommand(sqlGetData);
                cmdGetData.AddParameter("@AppSettingKey", SettingKey, System.Data.DbType.String);

                AppSettingCollection selSetting = new AppSettingCollection();
                selSetting.LoadAndCloseReader(DataService.GetReader(cmdGetData));

                if (selSetting.Count < 1)
                {
                    return null;
                }
                else
                {
                    return selSetting[0].AppSettingValue;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SettingKey"></param>
        /// <param name="OutletName"></param>
        /// <returns>If the setting is not exists, return NULL</returns>
        public static string GetSetting(string SettingKey, string OutletName)
        {
            string sqlGetData = "SELECT TOP 1 * FROM AppSetting WHERE OutletName IS @OutletName AND AppSettingKey = @AppSettingKey";

            QueryCommand cmdGetData = new QueryCommand(sqlGetData);
            cmdGetData.AddParameter("@AppSettingKey", SettingKey, System.Data.DbType.String);
            cmdGetData.AddParameter("@OutletName", OutletName, System.Data.DbType.String);

            AppSettingCollection selSetting = new AppSettingCollection();
            selSetting.LoadAndCloseReader(DataService.GetReader(cmdGetData));

            if (selSetting.Count < 1)
            {
                return null;
            }
            else
            {
                return selSetting[0].AppSettingValue;
            }
        }

        /// <summary>
        /// Insert / Update AppSettings
        /// </summary>
        /// <param name="SettingKey">The Key</param>
        /// <param name="Value">The Value</param>
        public static void SetSetting(string SettingKey, string Value)
        {
            string sqlSetData =
                "DECLARE @AppSettingKey VARCHAR(MAX); " +
                "DECLARE @AppSettingVal VARCHAR(MAX); " +
                "SET @AppSettingKey = '" + SettingKey + "'; " +
                "SET @AppSettingVal = '" + Value.Replace("'", "''") + "'; " +
                "SELECT TOP 1 * FROM AppSetting WHERE OutletName IS NULL AND AppSettingKey = @AppSettingKey " +
                "IF ((SELECT COUNT(*) FROM AppSetting WHERE OutletName IS NULL AND AppSettingKey = @AppSettingKey) <= 0) " +
                    "BEGIN INSERT INTO AppSetting (AppSettingId ,AppSettingKey ,AppSettingValue ,OutletName ,CreatedOn ,CreatedBy ,ModifiedOn ,ModifiedBy) VALUES (NEWID(),@AppSettingKey,@AppSettingVal,NULL,GETDATE(),'SCRIPT',GETDATE(),'SCRIPT'); END " +
                "ELSE " +
                    "BEGIN UPDATE AppSetting SET AppSettingValue = @AppSettingVal, ModifiedOn = GETDATE(), ModifiedBy = 'SCRIPT' WHERE OutletName IS NULL AND AppSettingKey = @AppSettingKey END ";

            DataService.ExecuteQuery(new QueryCommand(sqlSetData));
        }

        /// <summary>
        /// Delete AppSettings
        /// </summary>
        /// <param name="SettingKey">The Key</param>
        public static void DeleteSetting(string SettingKey)
        {
            string sqlSetData =
               "DECLARE @AppSettingKey VARCHAR(MAX); " +
               "SET @AppSettingKey = '" + SettingKey + "'; " +
               "DELETE FROM AppSetting WHERE OutletName IS NULL AND AppSettingKey = @AppSettingKey ";

            DataService.ExecuteQuery(new QueryCommand(sqlSetData));
        }

        /// <summary>
        /// Cast the input value as Bool. Will return the default value if the input is NULL / EMPTY / Not Registered
        /// </summary>
        /// <param name="SettingsValue"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static bool CastBool(string SettingsValue, bool DefaultValue)
        {
            if (SettingsValue == null) return DefaultValue;
            if (SettingsValue == "") return DefaultValue;
            if (SettingsValue.ToLower().Trim() == "t") return true;
            if (SettingsValue.ToLower().Trim() == "f") return false;
            if (SettingsValue.ToLower().Trim() == "true") return true;
            if (SettingsValue.ToLower().Trim() == "false") return false;
            if (SettingsValue.ToLower().Trim() == "y") return true;
            if (SettingsValue.ToLower().Trim() == "n") return false;
            if (SettingsValue.ToLower().Trim() == "yes") return true;
            if (SettingsValue.ToLower().Trim() == "no") return false;
            return DefaultValue;
        }

        /// <summary>
        /// Cast the input value as Decimal. Will return the default value if the input is NULL / EMPTY / Not Registered
        /// </summary>
        /// <param name="SettingsValue"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static decimal CastDecimal(string SettingsValue, decimal DefaultValue)
        {
            if (SettingsValue == null) return DefaultValue;
            if (SettingsValue == "") return DefaultValue;

            decimal Rst = DefaultValue;
            if (decimal.TryParse(SettingsValue, out Rst))
                return Rst;

            return DefaultValue;
        }

        /// <summary>
        /// Cast the input value as Integer. Will return the default value if the input is NULL / EMPTY / Not Registered
        /// </summary>
        /// <param name="SettingsValue"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static int CastInt(string SettingsValue, int DefaultValue)
        {
            if (SettingsValue == null) return DefaultValue;
            if (SettingsValue == "") return DefaultValue;

            int Rst = DefaultValue;
            if (int.TryParse(SettingsValue, out Rst))
                return Rst;

            return DefaultValue;
        }

        public static string CastString(string SettingsValue)
        {
            if (SettingsValue == null) return "";

            return SettingsValue;
        }

    }
}
