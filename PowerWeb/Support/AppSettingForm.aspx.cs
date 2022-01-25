using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PowerPOS;
using SubSonic;
using System.Linq;
using PowerPOS.Container;
using System.Web.UI.WebControls;

namespace PowerWeb.Support
{
    public partial class AppSettingForm : PageBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LoadSetting();
            }
        }

        protected void ddlLangSetting_Init(object sender, EventArgs e)
        {
            string sql = @"SELECT   ID
		                            ,Name 
                            FROM	LANGUAGE_SETTINGS
                            WHERE	ISNULL(Name,'') <> ''";
            DataTable dt = new DataTable();
            dt.Load(DataService.GetReader(new QueryCommand(sql)));
            var engRow = dt.NewRow();
            engRow["ID"] = "ENG";
            engRow["Name"] = "English (United States)";
            var chsRow = dt.NewRow();
            chsRow["ID"] = "CHS";
            chsRow["Name"] = "中文(新加坡)";
            dt.Rows.InsertAt(chsRow, 0);
            dt.Rows.InsertAt(engRow, 0);

            var ddl = (DropDownList)sender;
            ddl.DataSource = dt;
            ddl.DataBind();
        }

        private void LoadPointPercentageSetting()
        {
            chkUsingPointsPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.IsUsingPercentage), false);
            txtPointsItemNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName);
            chkUsingExpiryPeriodForPoints.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.HaveExpiryDate), false);
            txtExpiryPeriod.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipPoint.ValidityPeriodInMonth);
            //AppSetting.SetSetting(AppSetting.SettingsName.Points.HaveExpiryDate, chkUsingExpiryPeriodForPoints.Checked.ToString());
            //AppSetting.SetSetting(AppSetting.SettingsName.Points.ExpiredAfter, txtExpiryPeriod.Text);
            chkWontGetRewardPointsIfBuyPackageItem.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WontGetRewardPointsIfBuyPackageItem), true);
            chkExcludePaymentTypeForPointsCalculation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.ExcludePaymentTypeForPointsCalculation), false);
            txtExcludedPaymentType.Text = AppSetting.GetSetting(AppSetting.SettingsName.Points.ExcludedPaymentTypes);
        }

        private void SavePointPercentageSetting()
        {
            
            AppSetting.SetSetting(AppSetting.SettingsName.Points.IsUsingPercentage, chkUsingPointsPercentage.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Points.PercentagePointName, txtPointsItemNo.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Points.HaveExpiryDate, chkUsingExpiryPeriodForPoints.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.MembershipPoint.ValidityPeriodInMonth, txtExpiryPeriod.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Points.WontGetRewardPointsIfBuyPackageItem, chkWontGetRewardPointsIfBuyPackageItem.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Points.ExcludePaymentTypeForPointsCalculation, chkExcludePaymentTypeForPointsCalculation.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Points.ExcludedPaymentTypes, txtExcludedPaymentType.Text);
        }

        private void LoadItemSetting()
        {
            DisplayItemOpenPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemOpenPrice), true);
            DisplayItemService.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemService), true);
            DisplayItemPointPackage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemPointPackage), true);
            DisplayItemCourse.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayItemCourse), true);
            DisplayGiveCommission.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayGiveCommission), true);
            DisplayIsNonDiscountable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayIsNonDiscountable), true);
            DisplayPointRedeemable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPointRedeemable), true);
            DisplaySupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplaySupplier), true);
            DisplayUOM.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayUOM), false);
            txtProductSetupPageSize.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.ProductSetupPageSize);
            CostPriceText.Text = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.CostPriceText)) ? "Cost Price" : AppSetting.GetSetting(AppSetting.SettingsName.Item.CostPriceText);
            RetailPriceText.Text = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Item.RetailPriceText)) ? "Retail Price" : AppSetting.GetSetting(AppSetting.SettingsName.Item.RetailPriceText);
            UseSelectableItemNameFilter.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseSelectableItemNameFilter), true);
            UseSelectableAttributesFilter.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseSelectableAttributesFilter), true);
            txtNumDigit.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.NumDigit);
            HideDeletedItem.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.HideDeletedItem), false);
            LastBarcodeGenerated.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated);
            Item_DisplayAutoCaptureWeight.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayAutoCaptureWeight), false);
            Item_DisplayNonInventoryProduct.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct), false);
            AllowOverrideItemNameOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet), false);
            UseCustomerPricing.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseCustomerPricing), false);

            Item_DisplayApplyFuturePrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayApplyFuturePrice), false);
            Item_DisplayMinimumSellingPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice), false);
        }

        private void LoadSetting()
        {
            #region *) Load Setting
            
			string costingMode = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostCalculationMode) + "";
            if (string.IsNullOrEmpty(costingMode))
                costingMode = "AVERAGE";
            CostCalculationMode.SelectedValue = costingMode;
            EnableProductSerialNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false);
            #region GoodsOrdering
            /*AllowCreateOrderForOtherOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowCreateOrderForOtherOutlet), false);*/
            AutoApproveOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveOrder), false);
            AllowOutletToOrderFromSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowOutletToOrderFromSupplier), false);
            /*DefaultSalesDateRange.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.DefaultSalesDateRange);*/
            AutoCreateSupplierPOUponOutletOrderApproval.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoCreateSupplierPOUponOutletOrderApproval), false);
            AutoApproveSupplierPO.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveSupplierPO), false);
            AutoUpdateCostPriceOnSupplierPOApproval.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval), false);

            AllowDeductInvQtyNotSufficient.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowDeductInvQtyNotSufficient), false);
            ShowPriceLevelForWebOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPriceLevelForWebOrder), false);
            ShowFactoryPriceInGoodsOrdering.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInGoodsOrdering), false);
            ShowFactoryPriceInOrderApproval.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInOrderApproval), false);
            ShowFactoryPriceInReturnApproval.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInReturnApproval), false);
            AllowCreateInvoiceForStockTransferAndGoodsOrdering.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowCreateInvoiceForStockTransferAndGoodsOrdering), false);
            StockReturnWillReturnStockToWarehouse.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockReturnWillReturnStockToWarehouse), false);
            StockTransferWillGoThroughWarehouse.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse), false);
            ShowPrintDOButtonInGoodsOrdering.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPrintDOButtonInGoodsOrdering), false);

            ShowFactoryPriceInTransferApproval.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInTransferApproval), false);
            UseTransferApproval.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.UseTransferApproval), false);
            HideQtyInOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.HideQtyInOutlet), false);
            GoodsOrdering_InvoiceGSTRule.SelectedValue = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.InvoiceGSTRule);
            #endregion

            LoadItemSetting();


            Report_UseDataWarehouse.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Report.UseDataWarehouse), false);

            LinkPOSToMember.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.LinkPOSToMember), false);
            

            #region Auto Generate Barcode

            chAutoGenerateBarcode.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AutoGenerateBarcode), false);

            string optionAGB = AppSetting.GetSetting(AppSetting.SettingsName.Item.OptionAutoGenerateBarcode);
            if (optionAGB == "B")
                rbAutoGenerateBarcodeB.Checked = true;
            else
                rbAutoGenerateBarcodeA.Checked = true;

            BarcodePrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.BarcodePrefix);
            txtCategoryRunningNumberNoofDigit.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.CategoryRunningNumberNoofDigit);

            #endregion

            //AddProductOneOutletOnly.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddProductOneOutletOnly), false);

            DisplayPrice1.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice1), true);
            DisplayPrice2.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice2), true);
            DisplayPrice3.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice3), true);
            DisplayPrice4.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice4), true);
            DisplayPrice5.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.DisplayPrice5), true);
            txtDiscountP1Name.Text = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P1DiscountName);
            txtDiscountP2Name.Text = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P2DiscountName);
            txtDiscountP3Name.Text = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P3DiscountName);
            txtDiscountP4Name.Text = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P4DiscountName);
            txtDiscountP5Name.Text = AppSetting.GetSetting(AppSetting.SettingsName.Discount.P5DiscountName);


            Invoice_SelectableDiscountReason.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableDiscountReason);
            DiscountReportShowSearchDiscountReason.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DiscountReportShowSearchDiscountReason), false);

            DisplayCurrencyOnItemSupplierMap.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DisplayCurrencyOnItemSupplierMap), false);
            DisplayGSTOnItemSupplierMap.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DisplayGSTOnItemSupplierMap), false);
            MaxPackingSizeOnItemSupplierMap.Text = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.MaxPackingSizeOnItemSupplierMap)) ? "0" : AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.MaxPackingSizeOnItemSupplierMap);
            AvailableCurrency.Text = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.AvailableCurrency)) ? "SGD,USD,EUR,IDR,JPY,AUD" : AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.AvailableCurrency);
            DefaultCurrency.Text = string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency)) ? "SGD" : AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency);

            PORole.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderRole);
            POCompany.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderCompany);
            POMailCC.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.POMailCC);
            POMailContent.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.POMailContent);
            POMailSubject.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.POMailSubject);
            IsSellingPriceEditable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsSellingPriceEditable), false);

            DisplayCurrencyOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayCurrencyOnSupplier), false);
            DisplayGSTOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayGSTOnSupplier), false);
            DisplayMinimumOrderOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayMinimumOrderOnSupplier), false);
            DisplayDeliveryChargeOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayDeliveryChargeOnSupplier), false);
            DisplayPaymentTermOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayPaymentTermOnSupplier), false);

            LineInfo_ReplaceTextWith.Text = AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith);

            Membership_ShowRemarkInTransactionReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowRemarkInTransactionReport), false);
            Membership_ShowLineInfoInTransactionReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowLineInfoInTransactionReport), false);
            Membership_ShowBalancePaymentInTransactionReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowBalancePaymentInTransactionReport), false);
            Membership_ShowQtyInTransactionReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowQtyInTransactionReport), false);
            Membership_ShowQtyOnHandInTransactionReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowQtyOnHandInTransactionReport), false);
            Membership_AllowShowSalesOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AllowShowSalesOutlet), false);

            Item_AllowPreOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AllowPreOrder), false);
            Item_ShowVendorDeliveryOption.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.ShowVendorDeliveryOption), false);

            //Funding
            Funding_EnableFunding.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            Funding_EnablePAMed.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            Funding_EnableSMF.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            Funding_EnablePWF.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            Funding_PAMedPercentage.Text = AppSetting.GetSetting(AppSetting.SettingsName.Funding.PAMedPercentage);
            Funding_SMFPercentage.Text = AppSetting.GetSetting(AppSetting.SettingsName.Funding.SMFPercentage);

            //Email Sender
            EmailSender_SMTP.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP);
            EmailSender_Port.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port);
            EmailSender_SenderEmail.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail);
            EmailSender_DefaultMailTo.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo);
            EmailSender_Username.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username);
            EmailSender_Password.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password);
            EmailSender_BccToOwner.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
            EmailSender_OwnerEmailAddress.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
            EmailSender_ReceiptNoInEmailReceipt.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);
            EmailSender_Cc.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Cc);

            //PO Numbering
            Fill_PO_Reset_Combobox();
            PurchaseOrder_UseCustomNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false);
            PurchaseOrder_CustomPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix);
            PurchaseOrder_CustomSuffix.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix);
            PurchaseOrder_NumberLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength);
            PurchaseOrder_CurrentNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo);
            PurchaseOrder_ResetNumberEvery.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery);
            PurchaseOrder_CustomNoDateFormat.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat);

            //GR Numbering
            Fill_GR_Reset_Combobox();
            GoodsReceive_UseCustomNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo), false);
            GoodsReceive_CustomPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix);
            GoodsReceive_CustomSuffix.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix);
            GoodsReceive_NumberLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength);
            GoodsReceive_CurrentNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo);
            GoodsReceive_ResetNumberEvery.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery);
            GoodsReceive_CustomNoDateFormat.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat);

            AutoGenerateInvoiceNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo), false);
            AutoGenerateInvoiceNoPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNoPrefix);
            AutoGenerateInvoiceLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceLength);
            GoodsOrdering_ShowSalesGR.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.ShowSalesGR), false);
            RangeSalesShownGR.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.RangeSalesShownGR);
            ddlStatusAllReceived.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.StatusAllTallyReceived);

            ddSalesCostPrice.Text = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SalesCostOfGoods);

            #region Inventory
            AllowStockTransferGoThroughEvenStockIsZero.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowStockTransferEvenStockIsZero), false);
            CalculateAvgCostatInventoryLocationLevel.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationLevel), false);
            CalculateAvgCostatInventoryLocationGroupLevel.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationGroupLevel), false);
            //GetCostForAdjusmentInfromItemSetupIfZero.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.GetCostForAdjusmentInfromItemSetupIfZero), true);
            //GetCostForStockOutfromItemSetupIfZero.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.GetCostForStockOutfromItemSetupIfZero), true);
            ShowBatchNoStockTake.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBatchNoStockTake), false);
            ShowParValueStockTake.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowParValueStockTake), false);
            StockReturnNoAffectCOGS.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.StockReturnNoAffectCOGS), false);

            EditableAutoStockIn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EditableAutoStockIn), false);
            IsAutoStockIn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IsAutoStockIn), false);
            TextBeautyAdvisors.Text = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.TextBeautyAdvisors);
            IsLockSalesPersonGR.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IsLockSalesPersonGR), false);
            Inventory_AllowToUpdateRetailPriceInGoodsReceive.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowToUpdateRetailPriceInGoodsReceive), false);
            #endregion
            txtItemExpiryPeriodColumn.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.ExpiryPeriodColumn);
            ShowWarningWhenSellingPriceLessThanCostPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowWarningWhenSellingPriceLessThanCostPrice), false);

            chkEnableNETSIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            chkNETSATMCard.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            chkNETSCashCard.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            chkNETSFlashPay.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);

            SeparateUserPerOutletPrivileges.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.SeparateUserPerOutletPrivileges), false);
            //UseUserGroupOutletAssignment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseUserGroupOutletAssignment), false);
            EnableStockTransferAtInvoice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableStockTransferAtInvoice), false);
            EnableCreditInvoice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableCreditInvoice), false);

            #region *) MemberGroup Separated RefNo

            UseSeparatedRefNoPerMembershipGroup.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseSeparatedRefNoPerMembershipGroup), false);
            txtFirstSeparator.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.FirstRefNoSeparator);
            txtSecondSeparator.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SecondRefNoSeparator);
            SeparatedRefNoLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SeparatedRefNoLength);
            SeparatedRefNoStartNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SeparatedRefNoStartNo);
            SeparatedRefNoStartIncrement.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SeparatedRefNoStartIncrement);

            string posCodePos = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.POSCodeRefNoPosition);
            string memberGroupCodePos = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition);
            string runningNoPos = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.RunningNoRefNoPosition);

            if (posCodePos == "1")
                ddlFirstPos.SelectedValue = AppSetting.SettingsName.Invoice.POSCodeRefNoPosition;
            else if (posCodePos == "2")
                ddlSecondPos.SelectedValue = AppSetting.SettingsName.Invoice.POSCodeRefNoPosition;
            else if (posCodePos == "3")
                ddlThirdPos.SelectedValue = AppSetting.SettingsName.Invoice.POSCodeRefNoPosition;

            if (memberGroupCodePos == "1")
                ddlFirstPos.SelectedValue = AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition;
            else if (memberGroupCodePos == "2")
                ddlSecondPos.SelectedValue = AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition;
            else if (memberGroupCodePos == "3")
                ddlThirdPos.SelectedValue = AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition;

            if (runningNoPos == "1")
                ddlFirstPos.SelectedValue = AppSetting.SettingsName.Invoice.RunningNoRefNoPosition;
            else if (runningNoPos == "2")
                ddlSecondPos.SelectedValue = AppSetting.SettingsName.Invoice.RunningNoRefNoPosition;
            else if (runningNoPos == "3")
                ddlThirdPos.SelectedValue = AppSetting.SettingsName.Invoice.RunningNoRefNoPosition;

            #endregion

            LoadPointPercentageSetting();

            POSAppSettingSyncList.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.POSAppSettingSyncList);
            ShowPointOfSale.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowPointOfSale), false);
            ShowOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.ShowOutlet), false);
            User_UseSupplierPortal.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);

            UseMallManagement.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement), false);
            OutletText.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.OutletText);
            PointOfSaleText.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.PointOfSaleText);
            POSIDPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.POSIDPrefix);            
            InterfaceDevTeam.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.InterfaceDevTeam);
            //DefaultShopLevel.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.DefaultShopLevel);
            //DefaultShopNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.DefaultShopNo);
            TenantIDIncrement.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.TenantIDIncrement);
            TenantIDStartFrom.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.TenantIDStartFrom);
            DiscrepancyPercentage.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.DiscrepancyPercentage);
            CutOffDate.Text = (AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.CutOffDate)+"").GetIntValue().ToString();
            MallManagementTeamEmail.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallManagementTeamEmail);
            EdgeworksTeamEmail.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.EdgeworksTeamEmail);
            RetailerLevelAttribute1.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute1);
            RetailerLevelAttribute2.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute2);
            InterfaceFileName.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.InterfaceFileName);
            string langSetting = AppSetting.GetSetting(AppSetting.SettingsName.LanguageSetting);
            if (!string.IsNullOrEmpty(langSetting))
                ddlLangSetting.SelectedValue = langSetting;

            Refund_RefundReceiptFromOtherOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet), false);


            //Appointment
            UseResourceOnAppointment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);
            MinimumIntervalWeb.Text = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.MinimumIntervalWeb);
            AutoCheckOutIntervalGuestBook.Text = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.AutoCheckOutIntervalGuestBook);
            ShowPrefixMembershipOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GuestBook.ShowPrefixMembershipOutlet), false);

            ddDefaultGSTSetting.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.DefaultGSTSetting);

            ddlPOSType.Text = AppSetting.GetSetting(AppSetting.SettingsName.POSType);
            CompanyID.Text = AppSetting.GetSetting(AppSetting.SettingsName.CompanyID);
            CustomerMasterURL.Text = AppSetting.GetSetting(AppSetting.SettingsName.CustomerMasterURL);
            APICallerID.Text = AppSetting.GetSetting(AppSetting.SettingsName.APICallerID);
            APIPrivateKey.Text = AppSetting.GetSetting(AppSetting.SettingsName.APIPrivateKey);

            // Mobile
            #region Mobile Stock Take
            Mobile_DisplayCost.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.DisplayCost), false);
            Mobile_DisplayBatchNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.DisplayBatchNo), false);
            Mobile_DisplayShelf.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.DisplayShelf), false);
            Mobile_EnableStockIn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnableStockIn), false);
            Mobile_EnableStockOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnableStockOut), false);
            Mobile_EnableStockTake.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnableStockTake), false);
            Mobile_EnableStockTransfer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnableStockTransfer), false);
            Mobile_EnablePO.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnablePO), false);
            Mobile_EnableStockInFromPO.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnableStockInFromPO), false);

            Mobile_EnableRecordData.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.EnableRecordData), false);
            Mobile_RecordData1.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData1);
            Mobile_RecordData2.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData2);
            Mobile_RecordData3.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData3);
            Mobile_RecordData4.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData4);
            Mobile_RecordData5.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData5);
            Mobile_RecordData6.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData6);
            Mobile_RecordData7.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData7);
            Mobile_RecordData8.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData8);
            Mobile_RecordData9.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData9);
            Mobile_RecordData10.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.RecordData10);

            Mobile_SaveStockInInTemporaryFiles.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Mobile.StockInSaveToFile), false);
            #endregion

            //Reports
            Reports_AggregatedSalesReportMaxHistory.Text = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.Reports.AggregatedSalesReportMaxHistory), 100).ToString();
            Reports_ZXV3UploadDirectory.Text = AppSetting.GetSetting(AppSetting.SettingsName.Reports.ZXV3UploadDirectory);
            Reports_ShowPointInstallmentBreakdownInDailySales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Reports.ShowPointInstallmentBreakdownInDailySales), false);

            //FTP Settings
            FTP_Protocol.SelectedValue = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Protocol);
            FTP_Host.Text = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Host);
            FTP_Port.Text = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Port);
            FTP_Username.Text = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Username);
            FTP_Password.Text = AppSetting.GetSetting(AppSetting.SettingsName.FTP.Password);
            FTP_PassiveMode.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FTP.PassiveMode), false);


            //Audit Log
            AuditLog_ProductSetup.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.ProductSetup), false);
            AuditLog_SetupProductPromotion.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion), false);

            
            // Other Item No
            OtherItemNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.OtherItemNo);

            IsEditCategory_ProductOutletSetup.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.IsEditCategory_ProductOutletSetup), false);
            IsEditCostPrice_ProductOutletSetup.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.IsEditCostPrice_ProductOutletSetup), false);

            // Recipe
            Recipe_EnableRecipeManagement.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Recipe.EnableRecipeManagement), false);

            #endregion

            #region Low Quantity Userfld
            LowQtyUserfld1.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1);
            LowQtyUserfld2.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2);
            LowQtyUserfld3.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3);
            LowQtyUserfld4.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4);
            LowQtyUserfld5.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5);
            LowQtyUserfld6.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6);
            LowQtyUserfld7.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7);
            LowQtyUserfld8.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8);
            LowQtyUserfld9.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9);
            LowQtyUserfld10.Text = AppSetting.GetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10);
            #endregion

            #region attributes
            txtAttributes1.Text = ProductAttributeInfo.Attributes1;
            txtAttributes2.Text = ProductAttributeInfo.Attributes2;
            txtAttributes3.Text = ProductAttributeInfo.Attributes3;
            txtAttributes4.Text = ProductAttributeInfo.Attributes4;
            txtAttributes5.Text = ProductAttributeInfo.Attributes5;
            txtAttributes6.Text = ProductAttributeInfo.Attributes6;
            txtAttributes7.Text = ProductAttributeInfo.Attributes7;
            txtAttributes8.Text = ProductAttributeInfo.Attributes8;
            #endregion

            #region Commisssion

            Commission_GiveCommissionUponPayment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Commission.GiveCommissionUponPayment), false);
            Commission_BasedOn.SelectedValue = AppSetting.GetSetting(AppSetting.SettingsName.Commission.CommissionBasedOn);

            #endregion

            #region Mobile Stock
            ddlProductApplicableTo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Mobile.UpdateProductApplicableTo);
            #endregion

            #region Membership
            Membership_DefaultExpiryDate.Text = AppSetting.GetSetting(AppSetting.SettingsName.Membership.DefaultExpiryDate);
            #endregion
        }

        private void SaveItemSetting()
        {
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayMinimumSellingPrice, Item_DisplayMinimumSellingPrice.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayApplyFuturePrice, Item_DisplayApplyFuturePrice.Checked.ToString());
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            AccessLogController.AddLog(AccessSource.WEB, Session["UserName"] + "", "-", string.Format("Save Setting"), "");

            AppSetting.SetSetting(AppSetting.SettingsName.Report.UseDataWarehouse, Report_UseDataWarehouse.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.LinkPOSToMember, LinkPOSToMember.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.Supplier.DisplayCurrencyOnSupplier, DisplayCurrencyOnSupplier.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Supplier.DisplayGSTOnSupplier, DisplayGSTOnSupplier.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Supplier.DisplayMinimumOrderOnSupplier, DisplayMinimumOrderOnSupplier.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Supplier.DisplayDeliveryChargeOnSupplier, DisplayDeliveryChargeOnSupplier.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Supplier.DisplayPaymentTermOnSupplier, DisplayPaymentTermOnSupplier.Checked.ToString());

            #region GoodsOrdering
            /*AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AllowCreateOrderForOtherOutlet, AllowCreateOrderForOtherOutlet.Checked.ToString());*/
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveOrder, AutoApproveOrder.Checked.ToString());
            /*AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.DefaultSalesDateRange, DefaultSalesDateRange.Text);*/
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AutoCreateSupplierPOUponOutletOrderApproval, AutoCreateSupplierPOUponOutletOrderApproval.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AutoApproveSupplierPO, AutoApproveSupplierPO.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.AutoUpdateCostPriceOnSupplierPOApproval, AutoUpdateCostPriceOnSupplierPOApproval.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPriceLevelForWebOrder, ShowPriceLevelForWebOrder.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInGoodsOrdering, ShowFactoryPriceInGoodsOrdering.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInOrderApproval, ShowFactoryPriceInOrderApproval.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInReturnApproval, ShowFactoryPriceInReturnApproval.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AllowCreateInvoiceForStockTransferAndGoodsOrdering, AllowCreateInvoiceForStockTransferAndGoodsOrdering.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.StockReturnWillReturnStockToWarehouse, StockReturnWillReturnStockToWarehouse.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.StockTransferWillGoThroughWarehouse, StockTransferWillGoThroughWarehouse.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowPrintDOButtonInGoodsOrdering, ShowPrintDOButtonInGoodsOrdering.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowFactoryPriceInTransferApproval, ShowFactoryPriceInTransferApproval.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.UseTransferApproval, UseTransferApproval.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AllowDeductInvQtyNotSufficient, AllowDeductInvQtyNotSufficient.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.HideQtyInOutlet, HideQtyInOutlet.Checked.ToString());
            #endregion

            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayItemOpenPrice, DisplayItemOpenPrice.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayItemService, DisplayItemService.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayItemPointPackage, DisplayItemPointPackage.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayItemCourse, DisplayItemCourse.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayGiveCommission, DisplayGiveCommission.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayIsNonDiscountable, DisplayIsNonDiscountable.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayPointRedeemable, DisplayPointRedeemable.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplaySupplier, DisplaySupplier.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayPrice1, DisplayPrice1.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayPrice2, DisplayPrice2.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayPrice3, DisplayPrice3.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayPrice4, DisplayPrice4.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayPrice5, DisplayPrice5.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayUOM, DisplayUOM.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.CostPriceText, CostPriceText.Text.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.RetailPriceText, RetailPriceText.Text.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.ProductSetupPageSize, txtProductSetupPageSize.Text.GetIntValue().ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.NumDigit, txtNumDigit.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Item.UseSelectableItemNameFilter, UseSelectableItemNameFilter.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.UseSelectableAttributesFilter, UseSelectableAttributesFilter.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.HideDeletedItem, HideDeletedItem.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.LastBarcodeGenerated, LastBarcodeGenerated.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Item.BarcodePrefix, BarcodePrefix.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Item.OptionAutoGenerateBarcode, rbAutoGenerateBarcodeA.Checked ? "A" : "B");
            AppSetting.SetSetting(AppSetting.SettingsName.Item.CategoryRunningNumberNoofDigit, txtCategoryRunningNumberNoofDigit.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Item.AutoGenerateBarcode, chAutoGenerateBarcode.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayAutoCaptureWeight, Item_DisplayAutoCaptureWeight.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.DisplayNonInventoryProduct, Item_DisplayNonInventoryProduct.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.IsEditCategory_ProductOutletSetup, IsEditCategory_ProductOutletSetup.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.IsEditCostPrice_ProductOutletSetup, IsEditCostPrice_ProductOutletSetup.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.AllowOverrideItemNameOutlet, AllowOverrideItemNameOutlet.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.UseCustomerPricing, UseCustomerPricing.Checked.ToString());
            //AppSetting.SetSetting(AppSetting.SettingsName.Item.AddProductOneOutletOnly, AddProductOneOutletOnly.Checked.ToString());
            SaveItemSetting();
                
            //Discount
            AppSetting.SetSetting(AppSetting.SettingsName.Discount.P1DiscountName, txtDiscountP1Name.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Discount.P2DiscountName, txtDiscountP2Name.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Discount.P3DiscountName, txtDiscountP3Name.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Discount.P4DiscountName, txtDiscountP4Name.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Discount.P5DiscountName, txtDiscountP5Name.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Discount.DiscountReportShowSearchDiscountReason, DiscountReportShowSearchDiscountReason.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.SelectableDiscountReason, Invoice_SelectableDiscountReason.Text);




            #region Update Special Discount

            UpdatePriceLevel("P1", txtDiscountP1Name.Text, DisplayPrice1.Checked, 1);
            UpdatePriceLevel("P2", txtDiscountP2Name.Text, DisplayPrice2.Checked, 2);
            UpdatePriceLevel("P3", txtDiscountP3Name.Text, DisplayPrice3.Checked, 3);
            UpdatePriceLevel("P4", txtDiscountP4Name.Text, DisplayPrice4.Checked, 4);
            UpdatePriceLevel("P5", txtDiscountP5Name.Text, DisplayPrice5.Checked, 5);

            #endregion

            AppSetting.SetSetting(AppSetting.SettingsName.ItemSupplierMap.DisplayCurrencyOnItemSupplierMap, DisplayCurrencyOnItemSupplierMap.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.ItemSupplierMap.DisplayGSTOnItemSupplierMap, DisplayGSTOnItemSupplierMap.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.ItemSupplierMap.MaxPackingSizeOnItemSupplierMap, MaxPackingSizeOnItemSupplierMap.Text.GetIntValue().ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.ItemSupplierMap.AvailableCurrency, AvailableCurrency.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency, DefaultCurrency.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderCompany, POCompany.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderRole, PORole.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.POMailCC, POMailCC.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.POMailContent, POMailContent.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.POMailSubject, POMailSubject.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith, LineInfo_ReplaceTextWith.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.Membership.ShowRemarkInTransactionReport, Membership_ShowRemarkInTransactionReport.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Membership.ShowLineInfoInTransactionReport, Membership_ShowLineInfoInTransactionReport.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Membership.ShowBalancePaymentInTransactionReport, Membership_ShowBalancePaymentInTransactionReport.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Membership.ShowQtyInTransactionReport, Membership_ShowQtyInTransactionReport.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Membership.ShowQtyOnHandInTransactionReport, Membership_ShowQtyOnHandInTransactionReport.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Membership.AllowShowSalesOutlet, Membership_AllowShowSalesOutlet.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.Item.AllowPreOrder, Item_AllowPreOrder.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Item.ShowVendorDeliveryOption, Item_ShowVendorDeliveryOption.Checked.ToString());

            //Funding
            AppSetting.SetSetting(AppSetting.SettingsName.Funding.EnableFunding, Funding_EnableFunding.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Funding.EnablePAMed, Funding_EnablePAMed.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Funding.EnableSMF, Funding_EnableSMF.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Funding.EnablePWF, Funding_EnablePWF.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Funding.PAMedPercentage, Funding_PAMedPercentage.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Funding.SMFPercentage, Funding_SMFPercentage.Text);

            //Email Sender
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP, EmailSender_SMTP.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port, EmailSender_Port.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail, EmailSender_SenderEmail.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo, EmailSender_DefaultMailTo.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username, EmailSender_Username.Text);
            if (!string.IsNullOrEmpty(EmailSender_Password.Text)) 
                AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password, EmailSender_Password.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner, EmailSender_BccToOwner.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress, EmailSender_OwnerEmailAddress.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt, EmailSender_ReceiptNoInEmailReceipt.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Cc, EmailSender_Cc.Text);

            //PO Numbering
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo, PurchaseOrder_UseCustomNo.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix, PurchaseOrder_CustomPrefix.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix, PurchaseOrder_CustomSuffix.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength, PurchaseOrder_NumberLength.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo, PurchaseOrder_CurrentNo.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery, PurchaseOrder_ResetNumberEvery.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat, PurchaseOrder_CustomNoDateFormat.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.IsSellingPriceEditable, IsSellingPriceEditable.Checked.ToString());

            //GR Numbering
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo, GoodsReceive_UseCustomNo.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix, GoodsReceive_CustomPrefix.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix, GoodsReceive_CustomSuffix.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength, GoodsReceive_NumberLength.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo, GoodsReceive_CurrentNo.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery, GoodsReceive_ResetNumberEvery.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat, GoodsReceive_CustomNoDateFormat.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.SalesCostOfGoods, ddSalesCostPrice.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.ShowSalesGR, GoodsOrdering_ShowSalesGR.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.RangeSalesShownGR, RangeSalesShownGR.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNo, AutoGenerateInvoiceNo.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceNoPrefix, AutoGenerateInvoiceNoPrefix.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AutoGenerateInvoiceLength, AutoGenerateInvoiceLength.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.AllowOutletToOrderFromSupplier, AllowOutletToOrderFromSupplier.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.StatusAllTallyReceived, ddlStatusAllReceived.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsOrdering.InvoiceGSTRule, GoodsOrdering_InvoiceGSTRule.SelectedValue);

            #region Inventory 
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.AllowStockTransferEvenStockIsZero, AllowStockTransferGoThroughEvenStockIsZero.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationLevel, CalculateAvgCostatInventoryLocationLevel.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationGroupLevel, CalculateAvgCostatInventoryLocationGroupLevel.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.ShowBatchNoStockTake, ShowBatchNoStockTake.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.ShowParValueStockTake, ShowParValueStockTake.Checked.ToString());
            //AppSetting.SetSetting(AppSetting.SettingsName.Inventory.GetCostForAdjusmentInfromItemSetupIfZero, GetCostForAdjusmentInfromItemSetupIfZero.Checked.ToString());
            //AppSetting.SetSetting(AppSetting.SettingsName.Inventory.GetCostForStockOutfromItemSetupIfZero, GetCostForStockOutfromItemSetupIfZero.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.ShowWarningWhenSellingPriceLessThanCostPrice, ShowWarningWhenSellingPriceLessThanCostPrice.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.StockReturnNoAffectCOGS, StockReturnNoAffectCOGS.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo, EnableProductSerialNo.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.EditableAutoStockIn, EditableAutoStockIn.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.IsAutoStockIn, IsAutoStockIn.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.TextBeautyAdvisors, TextBeautyAdvisors.Text.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.IsLockSalesPersonGR, IsLockSalesPersonGR.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Inventory.AllowToUpdateRetailPriceInGoodsReceive, Inventory_AllowToUpdateRetailPriceInGoodsReceive.Checked.ToString());
            #endregion

            AppSetting.SetSetting(AppSetting.SettingsName.Item.ExpiryPeriodColumn, txtItemExpiryPeriodColumn.Text);

            //NETS Integration
            AppSetting.SetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration, chkEnableNETSIntegration.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard, chkNETSATMCard.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard, chkNETSCashCard.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay, chkNETSFlashPay.Checked.ToString());

            //User
            AppSetting.SetSetting(AppSetting.SettingsName.User.SeparateUserPerOutletPrivileges, SeparateUserPerOutletPrivileges.Checked.ToString());
            //AppSetting.SetSetting(AppSetting.SettingsName.User.UseUserGroupOutletAssignment, UseUserGroupOutletAssignment.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.User.ShowPointOfSale, ShowPointOfSale.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.User.ShowOutlet, ShowOutlet.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.User.UseSupplierPortal, User_UseSupplierPortal.Checked.ToString());

            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.EnableCreditInvoice, EnableCreditInvoice.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.EnableStockTransferAtInvoice, EnableStockTransferAtInvoice.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Sync.POSAppSettingSyncList, POSAppSettingSyncList.Text);
            
            #region Mall Integration
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.UseMallManagement, UseMallManagement.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.PointOfSaleText, PointOfSaleText.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.OutletText, OutletText.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.POSIDPrefix, POSIDPrefix.Text);            
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.InterfaceDevTeam, InterfaceDevTeam.Text);
            //AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.DefaultShopLevel, DefaultShopLevel.Text);
            //AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.DefaultShopNo, DefaultShopNo.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.TenantIDIncrement, TenantIDIncrement.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.TenantIDStartFrom, TenantIDStartFrom.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.DiscrepancyPercentage, DiscrepancyPercentage.Text.GetDecimalValue().ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.CutOffDate, CutOffDate.Text.GetIntValue().ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.MallManagementTeamEmail, MallManagementTeamEmail.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.EdgeworksTeamEmail, EdgeworksTeamEmail.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LanguageSetting, ddlLangSetting.SelectedValue);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute1, RetailerLevelAttribute1.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.RetailerLevelAttribute2, RetailerLevelAttribute2.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.InterfaceFileName, InterfaceFileName.Text);
            #endregion

            SavePointPercentageSetting();

            #region *) MemberGroup Separated RefNo

            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.UseSeparatedRefNoPerMembershipGroup, UseSeparatedRefNoPerMembershipGroup.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.FirstRefNoSeparator, txtFirstSeparator.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.SecondRefNoSeparator, txtSecondSeparator.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.SeparatedRefNoLength, SeparatedRefNoLength.Text.GetIntValue() == 0 ? "5" : SeparatedRefNoLength.Text.GetIntValue().ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.SeparatedRefNoStartNo, SeparatedRefNoStartNo.Text.GetIntValue().ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Invoice.SeparatedRefNoStartIncrement, SeparatedRefNoStartIncrement.Text.GetIntValue() == 0 ? "1" : SeparatedRefNoStartIncrement.Text.GetIntValue().ToString());

            if (ddlFirstPos.SelectedValue == AppSetting.SettingsName.Invoice.POSCodeRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.POSCodeRefNoPosition, "1");
            else if (ddlFirstPos.SelectedValue == AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition, "1");
            else if (ddlFirstPos.SelectedValue == AppSetting.SettingsName.Invoice.RunningNoRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.RunningNoRefNoPosition, "1");

            if (ddlSecondPos.SelectedValue == AppSetting.SettingsName.Invoice.POSCodeRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.POSCodeRefNoPosition, "2");
            else if (ddlSecondPos.SelectedValue == AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition, "2");
            else if (ddlSecondPos.SelectedValue == AppSetting.SettingsName.Invoice.RunningNoRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.RunningNoRefNoPosition, "2");

            if (ddlThirdPos.SelectedValue == AppSetting.SettingsName.Invoice.POSCodeRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.POSCodeRefNoPosition, "3");
            else if (ddlThirdPos.SelectedValue == AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.MemberGroupCodeRefNoPosition, "3");
            else if (ddlThirdPos.SelectedValue == AppSetting.SettingsName.Invoice.RunningNoRefNoPosition)
                AppSetting.SetSetting(AppSetting.SettingsName.Invoice.RunningNoRefNoPosition, "3");

            #endregion

            #region refund
            AppSetting.SetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromOtherOutlet, Refund_RefundReceiptFromOtherOutlet.Checked.ToString());
            #endregion

            #region *) Appointment
            //Email Sender
            AppSetting.SetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment, UseResourceOnAppointment.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Appointment.MinimumIntervalWeb, MinimumIntervalWeb.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Appointment.AutoCheckOutIntervalGuestBook, AutoCheckOutIntervalGuestBook.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.GuestBook.ShowPrefixMembershipOutlet, ShowPrefixMembershipOutlet.Checked.ToString());

            #endregion


            #region Mobile Stock

            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.DisplayCost, Mobile_DisplayCost.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.DisplayBatchNo, Mobile_DisplayBatchNo.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.DisplayShelf, Mobile_DisplayShelf.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnableStockIn, Mobile_EnableStockIn.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnableStockOut, Mobile_EnableStockOut.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnableStockTake, Mobile_EnableStockTake.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnableStockTransfer, Mobile_EnableStockTransfer.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnablePO, Mobile_EnablePO.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnableStockInFromPO, Mobile_EnableStockInFromPO.Checked.ToString());
            
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.EnableRecordData, Mobile_EnableRecordData.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData1, Mobile_RecordData1.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData2, Mobile_RecordData2.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData3, Mobile_RecordData3.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData4, Mobile_RecordData4.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData5, Mobile_RecordData5.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData6, Mobile_RecordData6.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData7, Mobile_RecordData7.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData8, Mobile_RecordData8.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData9, Mobile_RecordData9.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.RecordData10, Mobile_RecordData10.Text);

            //Adi 20170302
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.StockInSaveToFile, Mobile_SaveStockInInTemporaryFiles.Checked.ToString());

            //Adi 20180815
            AppSetting.SetSetting(AppSetting.SettingsName.Mobile.UpdateProductApplicableTo, ddlProductApplicableTo.Text.ToString());

            #endregion


            AppSetting.SetSetting(AppSetting.SettingsName.Item.DefaultGSTSetting, ddDefaultGSTSetting.Text);

            AppSetting.SetSetting(AppSetting.SettingsName.POSType, ddlPOSType.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.CompanyID, CompanyID.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.CustomerMasterURL, CustomerMasterURL.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.APICallerID, APICallerID.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.APIPrivateKey, APIPrivateKey.Text);

            #region Attributes Label
            AttributesLabelController.SaveAttributesLabel(1, txtAttributes1.Text);
            AttributesLabelController.SaveAttributesLabel(2, txtAttributes2.Text);
            AttributesLabelController.SaveAttributesLabel(3, txtAttributes3.Text);
            AttributesLabelController.SaveAttributesLabel(4, txtAttributes4.Text);
            AttributesLabelController.SaveAttributesLabel(5, txtAttributes5.Text);
            AttributesLabelController.SaveAttributesLabel(6, txtAttributes6.Text);
            AttributesLabelController.SaveAttributesLabel(7, txtAttributes7.Text);
            AttributesLabelController.SaveAttributesLabel(8, txtAttributes8.Text);
            AttributesLabelController.FetchProductAttributeLabel();
            #endregion

            #region Low Quantity
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld1, LowQtyUserfld1.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld2, LowQtyUserfld2.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld3, LowQtyUserfld3.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld4, LowQtyUserfld4.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld5, LowQtyUserfld5.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld6, LowQtyUserfld6.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld7, LowQtyUserfld7.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld8, LowQtyUserfld8.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld9, LowQtyUserfld9.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.LowQtyWarning.LowQtyUserfld10, LowQtyUserfld10.Text);
            #endregion

            //Reports
            AppSetting.SetSetting(AppSetting.SettingsName.Reports.AggregatedSalesReportMaxHistory, Reports_AggregatedSalesReportMaxHistory.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Reports.ZXV3UploadDirectory, Reports_ZXV3UploadDirectory.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.Reports.ShowPointInstallmentBreakdownInDailySales, Reports_ShowPointInstallmentBreakdownInDailySales.Checked.ToString());

            //FTP Settings
            AppSetting.SetSetting(AppSetting.SettingsName.FTP.Protocol, FTP_Protocol.SelectedValue);
            AppSetting.SetSetting(AppSetting.SettingsName.FTP.Host, FTP_Host.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.FTP.Port, FTP_Port.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.FTP.Username, FTP_Username.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.FTP.Password, FTP_Password.Text);
            AppSetting.SetSetting(AppSetting.SettingsName.FTP.PassiveMode, FTP_PassiveMode.Checked.ToString());

            //Audit Log
            AppSetting.SetSetting(AppSetting.SettingsName.AuditLog.ProductSetup, AuditLog_ProductSetup.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.AuditLog.SetupProductPromotion, AuditLog_SetupProductPromotion.Checked.ToString());

            // OtherItemNo
            AppSetting.SetSetting(AppSetting.SettingsName.Sync.OtherItemNo, OtherItemNo.Text);

            // Recipe
            AppSetting.SetSetting(AppSetting.SettingsName.Recipe.EnableRecipeManagement, Recipe_EnableRecipeManagement.Checked.ToString());

            #region Commission

            AppSetting.SetSetting(AppSetting.SettingsName.Commission.GiveCommissionUponPayment, Commission_GiveCommissionUponPayment.Checked.ToString());
            AppSetting.SetSetting(AppSetting.SettingsName.Commission.CommissionBasedOn, Commission_BasedOn.SelectedValue);

            #endregion

            #region Memebership

            AppSetting.SetSetting(AppSetting.SettingsName.Membership.DefaultExpiryDate, Membership_DefaultExpiryDate.Text);

            #endregion

            LoadSetting();
        }



        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTableName.Text) && !string.IsNullOrEmpty(txtPKColumn.Text))
            {
                SyncRealTimeController.UpdateModifiedOn(txtTableName.Text, txtPKColumn.Text);
            }
        }

        protected void btnGenerateItemSummary_Click(object sender, EventArgs e)
        {
            bool isSuccess = ItemSummaryController.GenerateItemSummary();
        }

        private void Fill_PO_Reset_Combobox()
        {
            PurchaseOrder_ResetNumberEvery.Items.Clear();
            PurchaseOrder_ResetNumberEvery.Items.Add(Constants.ResetEvery.Never);
            PurchaseOrder_ResetNumberEvery.Items.Add(Constants.ResetEvery.Day);
            PurchaseOrder_ResetNumberEvery.Items.Add(Constants.ResetEvery.Month);
            PurchaseOrder_ResetNumberEvery.Items.Add(Constants.ResetEvery.Year);
            PurchaseOrder_ResetNumberEvery.Items.Add(Constants.ResetEvery.MaxReached);
        }

        private void Fill_GR_Reset_Combobox()
        {
            GoodsReceive_ResetNumberEvery.Items.Clear();
            GoodsReceive_ResetNumberEvery.Items.Add(Constants.ResetEvery.Never);
            GoodsReceive_ResetNumberEvery.Items.Add(Constants.ResetEvery.Day);
            GoodsReceive_ResetNumberEvery.Items.Add(Constants.ResetEvery.Month);
            GoodsReceive_ResetNumberEvery.Items.Add(Constants.ResetEvery.Year);
            GoodsReceive_ResetNumberEvery.Items.Add(Constants.ResetEvery.MaxReached);
        }

        private void UpdatePriceLevel(string DiscountName, string Label, bool _enabled, int _priorityLevel)
        {
            SpecialDiscount sd = new SpecialDiscount(DiscountName);
            if (sd.IsLoaded && sd.DiscountName != "")
            {
                sd.DiscountName = DiscountName;
                sd.DiscountLabel = Label;
                sd.DiscountPercentage = 0;
                sd.ApplicableToAllItem = false;
                sd.Enabled = _enabled;

                sd.PriorityLevel = _priorityLevel;
                sd.ShowLabel = true;
                sd.StartDate = new DateTime(2015, 1, 1);
                sd.EndDate = new DateTime(2115, 1, 1);
                sd.IsBankPromo = false;
                sd.MinimumSpending = 0;
                sd.Deleted = false;
                //sd.SpecialDiscountID = new Guid();
                sd.Save(UserInfo.username);
            }
            else
            {
                //Add New
                SpecialDiscount sdNew = new SpecialDiscount();
                sdNew.DiscountName = DiscountName;
                sdNew.DiscountLabel = Label;
                sdNew.DiscountPercentage = 0;
                sdNew.ApplicableToAllItem = false;
                sdNew.Enabled = _enabled;
                
                sdNew.PriorityLevel = _priorityLevel;
                sdNew.ShowLabel = true;
                sdNew.StartDate = new DateTime(2015, 1, 1);
                sdNew.EndDate = new DateTime(2115, 1, 1);
                sdNew.IsBankPromo = false;
                sdNew.MinimumSpending = 0;
                sdNew.Deleted = false;
                sdNew.SpecialDiscountID = new Guid();
                sdNew.Save(UserInfo.username);
            }
        }

        protected void btnUploadFileSpec_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuFileSpec.HasFile)
                {
                    byte[] theFile = fuFileSpec.FileBytes;
                    string fileName = fuFileSpec.FileName;
                    string fileContent = Convert.ToBase64String(theFile);
                    InterfaceFileName.Text = fileName;
                    string path = Server.MapPath(@"~/Uploads/"+fileName);
                    if (File.Exists(path))
                        File.Delete(path);
                    File.WriteAllBytes(path, theFile);
                    AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.InterfaceFileName, fileName);
                    AppSetting.SetSetting(AppSetting.SettingsName.MallIntegration.InterfaceFileSource, path);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }
    }
}
