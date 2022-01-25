using PowerPOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SubSonic;
using System.Diagnostics;
using PowerPOS.Container;

namespace WinPowerPOS.Edgeworks
{
    public partial class frmFeatureSetup : Form
    {
        DataTable PaymentTypes;
        public frmFeatureSetup()
        {
            InitializeComponent();
            try
            {
                //Load Currency
                CurrencyCollection currencies = new CurrencyCollection();
                currencies.Where(Currency.Columns.Deleted, false);
                currencies.Load();
                SecondScreen_ForeignCurrency.Items.Clear();
                Currency_DefaultCurrency.Items.Clear();

                for (int i = 0; i < currencies.Count; i++)
                {
                    SecondScreen_ForeignCurrency.Items.Add(currencies[i].CurrencyCode);
                    Currency_DefaultCurrency.Items.Add(currencies[i].CurrencyCode);
                }

                string _status;
                PaymentTypesController.LoadPaymentTypes(out _status);

                PaymentTypes = new DataTable();
                PaymentTypes.Columns.Add("Name");

                DataRow emptyDr = PaymentTypes.NewRow();
                emptyDr["Name"] = "";
                PaymentTypes.Rows.Add(emptyDr);
                for (int i = 0; i < PointOfSaleInfo.PaymentTypes.Rows.Count; i++)
                {
                    DataRow dr = PaymentTypes.NewRow();
                    dr["Name"] = PointOfSaleInfo.PaymentTypes.Rows[i]["Name"];
                    PaymentTypes.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void LoadPaymentTypeComboBox()
        {
            //PointOfSaleInfo.PaymentTypes;
            Payment_NETSPayment.DataSource = PaymentTypes.Copy();
            Payment_NETSPayment.ValueMember = "Name";
            Payment_NETSPayment.DisplayMember = "Name";

            Payment_PaymentNETSCC_VISA.DataSource = PaymentTypes.Copy();
            Payment_PaymentNETSCC_VISA.ValueMember = "Name";
            Payment_PaymentNETSCC_VISA.DisplayMember = "Name";

            Payment_PaymentNETSCC_MASTER.DataSource = PaymentTypes.Copy();
            Payment_PaymentNETSCC_MASTER.ValueMember = "Name";
            Payment_PaymentNETSCC_MASTER.DisplayMember = "Name";

            Payment_PaymentNETSCC_AMEX.DataSource = PaymentTypes.Copy();
            Payment_PaymentNETSCC_AMEX.ValueMember = "Name";
            Payment_PaymentNETSCC_AMEX.DisplayMember = "Name";

            /*Payment_PaymentNETSCC_DINERS.DataSource = PaymentTypes.Copy();
            Payment_PaymentNETSCC_DINERS.ValueMember = "Name";
            Payment_PaymentNETSCC_DINERS.DisplayMember = "Name";

            Payment_PaymentNETSCC_JCB.DataSource = PaymentTypes.Copy();
            Payment_PaymentNETSCC_JCB.ValueMember = "Name";
            Payment_PaymentNETSCC_JCB.DisplayMember = "Name";*/

            Payment_UNIONPayPayment.DataSource = PaymentTypes.Copy();
            Payment_UNIONPayPayment.ValueMember = "Name";
            Payment_UNIONPayPayment.DisplayMember = "Name";

            Payment_PrepaidPurchasePayment.DataSource = PaymentTypes.Copy();
            Payment_PrepaidPurchasePayment.ValueMember = "Name";
            Payment_PrepaidPurchasePayment.DisplayMember = "Name";

            Payment_BCAPayment.DataSource = PaymentTypes.Copy();
            Payment_BCAPayment.ValueMember = "Name";
            Payment_BCAPayment.DisplayMember = "Name";

            Payment_PaymentCitiBank_VISA.DataSource = PaymentTypes.Copy();
            Payment_PaymentCitiBank_VISA.ValueMember = "Name";
            Payment_PaymentCitiBank_VISA.DisplayMember = "Name";

            Payment_PaymentCitiBank_MASTER.DataSource = PaymentTypes.Copy();
            Payment_PaymentCitiBank_MASTER.ValueMember = "Name";
            Payment_PaymentCitiBank_MASTER.DisplayMember = "Name";

            Payment_PaymentCitiBank_AMEX.DataSource = PaymentTypes.Copy();
            Payment_PaymentCitiBank_AMEX.ValueMember = "Name";
            Payment_PaymentCitiBank_AMEX.DisplayMember = "Name";

            Payment_PaymentCitiBank_JCB.DataSource = PaymentTypes.Copy();
            Payment_PaymentCitiBank_JCB.ValueMember = "Name";
            Payment_PaymentCitiBank_JCB.DisplayMember = "Name";

            Payment_PaymentCitiBank_DINERS.DataSource = PaymentTypes.Copy();
            Payment_PaymentCitiBank_DINERS.ValueMember = "Name";
            Payment_PaymentCitiBank_DINERS.DisplayMember = "Name";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <remarks>
        /// Do not change this function if you want to save new settings.
        /// Just set the name of the control that you want to save as the name 
        /// that will appear on AppSetting table.
        /// </remarks>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Feature Setup", "");
                for (int Counter = 0; Counter < tabControl1.TabCount; Counter++)
                    Save(tabControl1.TabPages[Counter]);

                SaveAppointmentSettings();

                if (!PointOfSaleInfo.IntegrateWithInventory)
                {
                    // Save some settings to server
                    SaveToServer();
                }

                MessageBox.Show("Changes Saved Successfully");

                this.Close();
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
            }
        }

        private void Save(Control Inst)
        {
            bool isRecursive = false;
            isRecursive = isRecursive || (Inst is TabPage);
            isRecursive = isRecursive || (Inst is Panel);
            isRecursive = isRecursive || (Inst is FlowLayoutPanel);
            isRecursive = isRecursive || (Inst is GroupBox);

            if (isRecursive)
            {
                for (int Counter = 0; Counter < Inst.Controls.Count; Counter++)
                {
                    Save(Inst.Controls[Counter]);
                }
            }
            else
            {
                if (Inst is CheckBox)
                    AppSetting.SetSetting(Inst.Name, ((CheckBox)Inst).Checked.ToString());
                else if (Inst is TextBox)
                    AppSetting.SetSetting(Inst.Name, Inst.Text);
                else if (Inst is RichTextBox)
                    AppSetting.SetSetting(Inst.Name, Inst.Text);
                else if (Inst is ComboBox)
                    AppSetting.SetSetting(Inst.Name, Inst.Text);
                else if (Inst is RadioButton)
                    AppSetting.SetSetting(Inst.Name, ((RadioButton)Inst).Checked.ToString());
            }
        }

        private void SaveToServer()
        {
            try
            {
                SyncClientController.Load_WS_URL();
                PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                ws.Timeout = 100000;
                ws.Url = SyncClientController.WS_URL;
                //ws.SetAppSettingValue(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero, AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero));

                DataTable dt = new DataTable("AppSetting");
                dt.Columns.Add("AppSettingKey", Type.GetType("System.String"));
                dt.Columns.Add("AppSettingValue", Type.GetType("System.String"));

                dt.Rows.Add(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero, AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero));

                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowCustomField1, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField1));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowCustomField2, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField2));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowCustomField3, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField3));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowCustomField4, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField4));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowCustomField5, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField5));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomField1Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField1Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomField2Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField2Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomField3Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField3Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomField4Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField4Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.CustomField5Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField5Label));

                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost1, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost1));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost2, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost2));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost3, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost3));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost4, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost4));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost5, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost5));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost1Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost2Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost3Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost4Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost5Label, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5Label));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost1_IsPercentage, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1_IsPercentage));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost2_IsPercentage, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2_IsPercentage));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost3_IsPercentage, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3_IsPercentage));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost4_IsPercentage, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4_IsPercentage));
                dt.Rows.Add(AppSetting.SettingsName.GoodsReceive.AdditionalCost5_IsPercentage, AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5_IsPercentage));

                ws.SetAppSettingValueBatch(dt);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("Error occured when saving to server. Please check the log.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAppointmentSettings()
        {
            string startTime = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.PointOfSale_WorkingHoursStart);
            string endtime = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.PointOfSale_WorkingHoursEnd);
            string interval = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.PointOfSale_MinimumTimeInterval);

            string[] parts;
            if (startTime != null)
            {
                parts = startTime.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    cmbStartHour.Text = parts[0];
                    cmbStartMinute.Text = parts[1];
                }
                else
                {
                    cmbStartHour.SelectedIndex = 0;
                    cmbStartMinute.SelectedIndex = 0;
                }
            }
            else
            {
                cmbStartHour.SelectedIndex = 0;
                cmbStartMinute.SelectedIndex = 0;
            }

            if (endtime != null)
            {
                parts = endtime.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    cmbEndHour.Text = parts[0];
                    cmbEndMinute.Text = parts[1];
                }
                else
                {
                    cmbEndHour.SelectedIndex = 0;
                    cmbEndMinute.SelectedIndex = 0;
                }
            }
            else
            {
                cmbEndHour.SelectedIndex = 0;
                cmbEndMinute.SelectedIndex = 0;
            }

            if (interval != null)
            {
                parts = interval.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3)
                {
                    cmbIntervalHour.Text = parts[0];
                    cmbIntervalMinute.Text = parts[1];
                    cmbIntervalSecond.Text = parts[2];
                }
                else
                {
                    cmbIntervalHour.SelectedIndex = 0;
                    cmbIntervalMinute.SelectedIndex = 0;
                    cmbIntervalSecond.SelectedIndex = 0;
                }
            }
            else
            {
                cmbIntervalHour.SelectedIndex = 0;
                cmbIntervalMinute.SelectedIndex = 0;
                cmbIntervalSecond.SelectedIndex = 0;
            }
        }

        private void SaveAppointmentSettings()
        {

            string value = cmbStartHour.Text + ":" + cmbStartMinute.Text; //START
            AppSetting.SetSetting(AppSetting.SettingsName.Appointment.PointOfSale_WorkingHoursStart, value);

            value = cmbEndHour.Text + ":" + cmbEndMinute.Text; //END
            AppSetting.SetSetting(AppSetting.SettingsName.Appointment.PointOfSale_WorkingHoursEnd, value);

            value = cmbIntervalHour.Text + ":" + cmbIntervalMinute.Text + ":" + cmbIntervalSecond.Text; //INTERVAL
            AppSetting.SetSetting(AppSetting.SettingsName.Appointment.PointOfSale_MinimumTimeInterval, value);
        }

        private void Check(Control Inst)
        {
            bool isRecursive = false;
            isRecursive = isRecursive || (Inst is TabPage);
            isRecursive = isRecursive || (Inst is Panel);
            isRecursive = isRecursive || (Inst is FlowLayoutPanel);
            isRecursive = isRecursive || (Inst is GroupBox);

            if (isRecursive)
            {
                for (int Counter = 0; Counter < Inst.Controls.Count; Counter++)
                {
                    Check(Inst.Controls[Counter]);
                }
            }
            else
            {
                if (Inst is TextBox)
                {
                    if (Inst.Tag == null) return;
                    if (Inst.Tag.ToString() == "") return;
                    if (Inst.Tag.ToString().ToLower() == "system.int32")
                        //if()
                        AppSetting.SetSetting(Inst.Name, Inst.Text);
                }
                else if (Inst is ComboBox)
                    AppSetting.SetSetting(Inst.Name, Inst.Text);
            }
        }

        private void frmFeatureSetup_Load(object sender, EventArgs e)
        {
            LoadPaymentTypeComboBox();
            Sales_HoldOrderFromServer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sales.EnableHoldOrderFromServer), false);

            Invoice_QuickCashPayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.QuickCashPayment);

            Invoice_EnableOutletSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableOutletSales), false);
            Invoice_EnableQuotation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnableQuotation), false);
            Invoice_PromtPasswordOnDiscLimit.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PromtPasswordOnDiscLimit), true);
            Invoice_UseDiscountReason.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseDiscountReason), false);
            Invoice_SelectableDiscountReason.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableDiscountReason);
            Invoice_UseLastTransactionPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseLastTransactionPrice), false);

            SecondScreen_ForeignCurrency.SelectedItem = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.ForeignCurrency);
            SecondScreen_ShowForeignCurrency.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.ShowForeignCurrency), false);

            /*Payment_EnableNETSIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            Payment_NETSCashCard.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSCashCard), false);
            Payment_NETSFlashPay.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSFlashPay), false);
            Payment_NETSATMCard.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSATMCard), false);*/

            SyncAfterCheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.SyncAfterCheckOut), true);
            SelectableVoidReason.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SelectableVoidReason);
            UseQuickPayment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseQuickPayment), false);
            User_LinkTheUserWithOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.LinkTheUserWithOutlet), false);
            PrintCashierOnTheReceipt.Checked = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintCashierOnTheReceipt), true));
            PrintDiscountOnCounterCloseReport.Checked = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintDiscountOnCounterCloseReport), false));
            Closing_IsShowCashDenominationOnReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.IsShowCashDenominationOnReceipt), false);

            Attendance_IsAvailable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.IsAvailable), false);
            Attendance_AutoAttendAfterSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.AutoAttendAfterSales), false);
            Attendance_GenerateOrderUponCheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.GenerateOrderUponCheckOut), false);
            Attendance_MinutesRoundingUp.Text = AppSetting.CastInt(AppSetting.GetSetting(AppSetting.SettingsName.Attendance.MinutesRoundingUp), 1).ToString();
            Attendance_ItemNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Attendance.ItemNo);

            Points_IsUsingPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.IsUsingPercentage), false);
            Points_PercentagePointName.Text = AppSetting.CastString(AppSetting.GetSetting(AppSetting.SettingsName.Points.PercentagePointName));
            Points_AskRemarksWhenUsePoint.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.AskRemarksWhenUsePoint), false);
            Points_NotAlwaysUse1Point.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.NotAlwaysUse1Point), false);
            Points_HaveExpiryDate.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.HaveExpiryDate), false);
            Points_ExpiredAfter.Text = AppSetting.GetSetting(AppSetting.SettingsName.Points.ExpiredAfter);
            Points_Rounding.Text = AppSetting.GetSetting(AppSetting.SettingsName.Points.Rounding);
            Points_IsAskingPassCode.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.IsAskingPassCode), false);
            Points_ChoosePointPackageForPayment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.ChoosePointPackageForPayment), false);

            Print_IsUsingDeliveryOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.IsUsingDeliveryOrder), false);
            Print_DeliveryOrderFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.DeliveryOrderFileLocation);
            Print_ReceiptFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.ReceiptFileLocation);
            Print_PromptSelectPrintSize.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PromptSelectPrintSize), false);
            Currency_DefaultCurrency.Text = AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency);
            Signature_IsAvailable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailable), false);
            Signature_IsAvailableForAllPayment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false);
            Signature_IsDeliveryPreOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsDeliveryPreOrder), false);
            Auto_Membership_Upgrade.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AutoMembershipUpgrade), false);
            Inventory_HideCost.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideInventoryCost), false);
            Inventory_IncludeGSTExclusive.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IncludeGSTExclusive), false);
            Inventory_HideCostInStockTransfer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideCostInStockTransfer), false);
            Inventory_HideRetailPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.HideRetailPrice), false);
            Inventory_IncludeGSTExclusive.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IncludeGSTExclusive), false);
            AddItemPicture.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.AddItemPicture), false);
            ProjectModule.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Project.ProjectModule), false);
            Membership_Compulsory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.Membership_Compulsory), false);
            Membership_ShowPreOrderSummaryButton.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowPreOrderSummaryButton), false);
            AllowDiscount.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.AllowDiscount), false);
            DiscountPercentageColumn.Text = AppSetting.GetSetting(AppSetting.SettingsName.Promo.DiscountPercentageColumn);
            UseCustomInvoiceNo.Checked = AppSetting.CastBool(AppSetting.GetSetting("UseCustomInvoiceNo"), false);
            LowQuantityPromptInSalesScreen.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false);
            //PromptPassword_CheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.LowQuantityPromptInSalesScreen), false);
            PromptPassword_CheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnCheckOut), true);
            PromptPassword_Void.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnVoid), true);
            PromptPassword_OpeningBalance.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnOpeningBalance), true);
            PromptPassword_CashIn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnCashIn), true);
            PromptPassword_OnCashOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnCashOut), true);
            PromptPassword_OnRefund.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnRefund), false);
            Signature_IsStaffReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsStaffReceipt), false);
            SwitchColumnRowsMatrixForm.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SwitchColumnRowsMatrixForm), false);
           
            Use_Default_Payment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.UseDefaultPayment), false);
            Default_Payment_Type.Text = AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.DefaultPaymentType);
            DefaultPayment_ShowCashForm.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.DefaultPayment.DefaultPayment_ShowCashForm), false);

            UseImageItemFromLocalFolder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseImageItemFromLocalFolder), false);
            ItemImageFolderLocal.Text = AppSetting.GetSetting(AppSetting.SettingsName.Item.ItemImageFolderLocal);

            UseOpeningBalance.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.UseOpeningBalance), false);
            DefaultOpeningBalance.Text = AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.DefaultOpeningBalance);

            UseSecondScreen.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.UseSecondScreen), false);
            MarqueeText.Text = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.MarqueeText);
            SlideShowFolder.Text = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.SlideShowFolder);
            SecondScreen_HideLogo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.HideLogo), false);
            SecondScreen_HideItemNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.HideItemNo), false);
            SecondScreen_RefreshInterval.Text = AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.RefreshInterval);

            Print_StockTakeReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockTakeReportFileLocation);
            Print_GoodsReceiveReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.GoodsReceiveReportFileLocation);
            Print_StockIssueReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockIssueReportFileLocation);
            Print_StockAdjustmentReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockAdjustmentReportFileLocation);
            Print_StockTransferReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockTransferReportFileLocation);
            Print_StockReturnReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.StockReturnReportFileLocation);
            Print_OtherStockActivityReportFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.OtherStockActivityReportFileLocation);
            MembershipSummaryRowNumber.Text = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSummaryRowNumber);

            PoleDisplayText.Text = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayText);

            //PrintPointOnTheReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintPointOnTheReceipt), false);
            PrintBalancePaymentOnTheReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintBalancePaymentOnTheReceipt), false);
            PrintNETSResponseInfoOnTheReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintNETSResponseInfoOnTheReceipt), false);

            PrintInvoiceOnDelivery.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintInvoiceOnDelivery), false);
            AddDeliveryRemarks.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.AddDeliveryRemarks), false);
            AddPurchaseOrderInfo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.AddPurchaseOrderInfo), false);

            UseCategoryFilterOnClosingReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.UseCategoryFilterOnClosingReport), false);
            CategoryFilterName.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName);

            UseCategoryFilterOnClosingReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.UseCategoryFilterOnClosingReport), false);
            CategoryFilterName.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.CategoryFilterName);

            IsReplaceMembershipText.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.IsReplaceMembershipText), false);
            MembershipTextReplacement.Text = AppSetting.GetSetting(AppSetting.SettingsName.MembershipTextReplacement);
            AskPassCodeWhenCreateNewMember.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AskPassCodeWhenCreateNewMember), true);

            IsReplaceConfirmTextButton.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.IsReplaceConfirmTextButton), false);
            ReplaceConfirmTextButtonWith.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.ReplaceConfirmTextButtonWith);

            SaveTotalCashOnlyWhenCheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.SaveTotalCashOnlyWhenCheckOut), false);

            AllowChangeGSTonSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.AllowChangeGSTonSales), false);
            AllowEditPriceTwoDecimal.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.AllowEditPriceTwoDecimal), false);
            //AllowChangeGSTonSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.AllowChangeGSTonSales), false);

            string tmp = AppSetting.GetSetting("ReceiptPrefix");
            if (tmp == null)
            {
                AppSetting.SetSetting("ReceiptPrefix", "");
            }
            ReceiptPrefix.Text = tmp;

            tmp = AppSetting.GetSetting("ReceiptSuffix");
            if (tmp == null)
            {
                AppSetting.SetSetting("ReceiptSuffix", "");
            }
            ReceiptSuffix.Text = tmp;

            tmp = AppSetting.GetSetting("MaxReceiptNo");
            if (tmp == null)
            {
                AppSetting.SetSetting("MaxReceiptNo", "");
            }
            MaxReceiptNo.Text = tmp;
            tmp = AppSetting.GetSetting("CurrentReceiptNo");
            if (tmp == null)
            {
                AppSetting.SetSetting("CurrentReceiptNo", "");
            }
            CurrentReceiptNo.Text = tmp;

            UseWindcor.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.UseWindcor), false);
            FirstLineCommand.Text = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.FirstLineCommand);
            SecondLineCommand.Text = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.SecondLineCommand);
            PoleDisplayLinesLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayLinesLength);

            PromptPassword_DeductPoints.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnDeductPoints), true);
            PrintCounterCloseReport.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintCounterCloseReport), false);
            PrintItemDepartmentOnCheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintItemDepartmentOnCheckOut), false);

            PromptPassword_DeductPoints.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnDeductPoints), true);
            PromptPassword_OnVoidItem.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnVoidItem), false);

            ShowMembershipWarning.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowMembershipWarning), true);
            MembershipWarningFields.Text = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipWarningFields);
            Membership_ShowAttachedParticular.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.Membership_ShowAttachedParticular), false);
            Membership_ShowPackageEvenIfRemainingIsZero.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ShowPackageEvenIfRemainingIsZero), true);
            Membership_ExpiryDateBasedOnRenewalDate.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ExpiryDateBasedOnRenewalDate), false);
            Membership_EmailCompulsoryIsTicked.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.EmailCompulsoryIsTicked), false);

            useNUHMallIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.useNUHMallIntegration), false);
            MallIntegration_TenantID.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallIntegration_TenantID);
            MallIntegration_OutputDirectory.Text = AppSetting.GetSetting(AppSetting.SettingsName.MallIntegration.MallIntegration_OutputDirectory);

            //EMAIL SENDER
            EmailSender_SMTP.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP);
            EmailSender_Port.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port);
            EmailSender_SenderEmail.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail);
            EmailSender_DefaultMailTo.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_DefaultMailTo);
            EmailSender_Username.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Username);
            EmailSender_Password.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password);
            EmailSender_BccToOwner.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_BccToOwner), false);
            EmailSender_OwnerEmailAddress.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_OwnerEmailAddress);
            EmailSender_ReceiptNoInEmailReceipt.Text = AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_ReceiptNoInEmailReceipt);

            //Custom Kick Drawer
            UseCustomKickDrawer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseCustomKickDrawer), false);
            CustomKickDrawerAppPath.Text = AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath);

            //Export products to file
            Export_ExportProductsToFileEnabled.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsToFileEnabled), false);
            Export_ExportProductsTemplateFile.Text = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsTemplateFile);
            Export_ExportProductsDirectory.Text = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsDirectory);
            Export_ExportProductsFileName.Text = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsFileName);
            Export_ExportProductsFilter.Text = AppSetting.GetSetting(AppSetting.SettingsName.Export.ExportProductsFilter);

            //Magento
            UseMagentoFeatures.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.UseMagentoFeatures), false);
            MagentoURL.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoURL);
            MagentoUser.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoUser);
            MagentoPassword.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.MagentoPassword);
            CategoryRootID.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.CategoryRootID);
            Magento_DefaultCustEmail.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustEmail);
            Magento_DefaultShippingMethod.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultShippingMethod);

            Magento_ViewAppointmentEnable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentEnable), false);
            Magento_ViewAppointmentURL.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentURL);
            Magento_ViewAppointmentUser.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentUser);
            Magento_ViewAppointmentPassword.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentPassword);
            Magento_ViewAppointmentText.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.ViewAppointmentText);

            Magento_DefaultCustStreet.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustStreet);
            Magento_DefaultCustTelephone.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustTelephone);
            Magento_DefaultCustPostCode.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustPostCode);
            Magento_DefaultCustCountryID.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustCountryID);
            Magento_DefaultCustRegion.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustRegion);
            Magento_DefaultCustCity.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultCustCity);
            Magento_DefaultPaymentType.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultPaymentType);
            Magento_DefaultStore.Text = AppSetting.GetSetting(AppSetting.SettingsName.Magento.DefaultStore);

            Inventory_RemoveZeroInventoryValidation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.RemoveZeroInventoryValidation), false);
            EnableChangeShift.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.EnableChangeShift), false);
            ShowTotalQuantityInSalesScreen.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowTotalQuantityInSalesScreen), false);
            ShowCostOnStockOnHand.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCostOnStockOnHand), false);
            MembershipSyncSegmentSize.Text = AppSetting.GetSetting(AppSetting.SettingsName.Membership.MembershipSyncSegmentSize);

            InstallmentText.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText);
            UseNegativeSalesAmount.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseNegativeSalesAmount), false);
            ShowTotalItemInSalesScreen.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowTotalItemInSalesScreen), false);

            UseTotalCashForCheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.UseTotalCashForCheckOut), false);
            HideSystemRecordedOnCheckOut.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.HideSystemRecordedOnCheckOut), false);


            ChangeMemberWithoutSupervisorLogin.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ChangeMemberWithoutSupervisorLogin),false);
            Order_AllowBackdatedSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AllowBackdatedSales), false);
            Order_BackdateMaxDays.Text = AppSetting.GetSetting(AppSetting.SettingsName.Order.BackdateMaxDays);

            EnableSecondDiscount.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableSecondDiscount), false);
            Discount_DisableGiveLineDiscountSalesInv.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.DisableGiveLineDiscountSalesInv), false);

            //Membership_ViewRealTimeSalesHistory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.ViewRealTimeSalesHistory), false);

            UseRealTimeSales.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false);
            SyncRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncRecordsPerTime);
            SalesRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SalesRetrySecWhenDisconnected);

            UseRealTimeInventory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeInventory), false);
            InventoryRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenConnected);
            InventoryRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventoryRetrySecWhenDisconnected);
            InventorySyncRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.InventorySyncRecordsPerTime);

            Inventory_AddItemPicture.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddItemPicture), false);
            Inventory_UseImageLocal.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UseImageLocal), false);
            Inventory_ImageLocalPath.Text = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ImageLocalPath);

            UseSpecialBarcode.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcode), false);
            BarcodeCheckDigit.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.BarcodeCheckDigit);
            BarcodeCheckValue.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.BarcodeCheckValue);
            ItemDigitStart.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitStart);
            ItemDigitLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.ItemDigitLength);
            PriceDigitStart.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitStart);
            PriceDigitLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.PriceDigitLength);
            RecordedDigitStart.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.RecordedDigitStart);
            RecordedDigitLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.RecordedDigitLength);

            QuantityDigitStart.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.QuantityDigitStart);
            QuantityDigitLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.QuantityDigitLength);
            IntegerDigitLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.IntegerDigitLength);

            UseSpecialBarcodeForPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcodeForPrice), false);
            UseSpecialBarcodeForQuantity.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SpecialBarcode.UseSpecialBarcodeForQuantity), false);

            Closing_EnableAutoBackup.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.EnableAutoBackup), true);

            PurchaseOrder_ShowDeliveryAddress.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryAddress), false);
            PurchaseOrder_ShowDeliveryDateTime.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryDateTime), false);
            PurchaseOrder_ShowGST.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowGST), false);
            PurchaseOrder_ShowPaymentType.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPaymentType), false);
            PurchaseOrder_ShowPackingSize.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowPackingSize), false);
            PurchaseOrder_ShowOrderQty.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowOrderQty), false);
            PurchaseOrder_ShowUOM.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowUOM), false);
            PurchaseOrder_ShowCurrency.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowCurrency), false);
            PurchaseOrder_ShowRetailPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowRetailPrice), false);
            PurchaseOrder_ShowFactoryPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowFactoryPrice), false);
            PurchaseOrder_ShowSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowSupplier), false);
            PurchaseOrder_ShowStatus.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowStatus), false);
            PurchaseOrder_ShowLoadAddressBtn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowLoadAddressBtn), false);
            PurchaseOrder_AllowSameItemMultipleTimes.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowSameItemMultipleTimes), false);

            EnableKeyInOpenPriceItemName.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.EnableKeyInOpenPriceItemName), false);
            UseCustomerPricing.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Item.UseCustomerPricing), false);

            PurchaseOrder_PurchaseOrderRole.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderRole);
            PurchaseOrder_PurchaseOrderCompany.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.PurchaseOrderCompany);
            PurchaseOrder_IsSellingPriceEditable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsSellingPriceEditable), false);
            PurchaseOrder_IsCostPriceEditable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsCostPriceEditable), false);
            PurchaseOrder_IsCostPerPackingSizeEditable.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.IsCostPerPackingSizeEditable), false);
            PurchaseOrder_SearchItemBySupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.SearchItemBySupplier), false);
            PurchaseOrder_AllowLoadUnapprovedPOInGR.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AllowLoadUnapprovedPOInGR), false);

            PurchaseOrder_ShowMinPurchase.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowMinPurchase), false);
            PurchaseOrder_ShowDeliveryCharge.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowDeliveryCharge), false);
            PurchaseOrder_ShowOrderInfo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ShowOrderInfo), false);

            PurchaseOrder_EnableAttachment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.EnableAttachment), false);
            PurchaseOrder_AttachmentMaxFileSize.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.AttachmentMaxFileSize);

            //Line Info
            LineInfo_ShowInInvoice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ShowInInvoice), false);
            LineInfo_DropdownCanAddNew.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.DropdownCanAddNew), false);
            LineInfo_ReplaceTextWith.Text = AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ReplaceTextWith);
            LineInfo_ShowInViewReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.ShowInViewReceipt), false);
            LineInfo_IsMandatory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.IsMandatory), false);
            LineInfo_MandatoryType.Text = AppSetting.GetSetting(AppSetting.SettingsName.LineInfo.MandatoryType);

            Print_PurchaseOrderFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.PurchaseOrderFileLocation);

            //Invoice
            Invoice_AllowAddRemarkAfterClosing.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.AllowAddRemarkAfterClosing), false);
            Invoice_SendPreOrderReceiptToEmail.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.SendPreOrderReceiptToEmail), false);
            Invoice_PreOrderReceiptTemplate.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PreOrderReceiptTemplate);
            Invoice_PreOrderNotifyTemplate.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PreOrderNotifyTemplate);

            Sync_UseRealTimeSyncLogs.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncLogs), false);
            Sync_SyncLogsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncLogsPerTime);
            Sync_SyncLogsRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncLogsRetrySecWhenDisconnected);

            Sync_UseRealTimeSyncQuotation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncQuotation), false);
            Sync_SyncQuotationPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncQuotationPerTime);
            Sync_SyncQuotationRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncQuotationRetrySecWhenDisconnected);

            Interface_HideSalesTab.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Interface.HideSalesTab), false);
            Interface_HideListingTab.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Interface.HideListingTab), false);
            Interface_HideInventoryTab.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Interface.HideInventoryTab), false);
            Interface_ReplaceGSTTextWith.Text = AppSetting.GetSetting(AppSetting.SettingsName.Interface.ReplaceGSTTextWith);

            Order_PromptSelectOrderType.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false);
            Order_ReplaceCashCarryTextWith.Text = AppSetting.GetSetting(AppSetting.SettingsName.Order.ReplaceCashCarryTextWith);
            Order_ReplacePreOrderTextWith.Text = AppSetting.GetSetting(AppSetting.SettingsName.Order.ReplacePreOrderTextWith);
            Order_AllowSplitDelivery.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AllowSplitDelivery), false);
            Order_DepositAssignmentValidation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.DepositAssignmentValidation), false);
            Order_AutoAssignDepositUponInstPayment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignDepositUponInstPayment), false);
            Order_AutoCancelDeliveryItemIfReturned.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoCancelDeliveryItemIfReturned), false);
            Order_ShowVendorDelivery.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowVendorDelivery), false);
            Order_ShowDeliveryOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowDeliveryOutlet), false);
            Order_DoNotRoundDiscountedPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.DoNotRoundDiscountedPrice), false);
            Order_ShowReminderWhenConfirm.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.ShowReminderWhenConfirm), false);
            Order_ReminderMessage.Text = AppSetting.GetSetting(AppSetting.SettingsName.Order.ReminderMessage);
            Order_OnlySearchItemNoItemName.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.OnlySearchItemNoItemName), false);
            Order_SkipDeliverySetupScreen.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.SkipDeliverySetupScreen), false);
            Order_AutoCreateWhenSkipDelivery.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoCreateWhenSkipDelivery), false);
            Order_EnableSecondSalesPerson.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnableSecondSalesPerson), false);
            Order_EnablePerformanceLog.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.EnablePerformanceLog), false);

            Order_AutoAssignMode.Items.Clear();
            Order_AutoAssignMode.Items.Add(AppSetting.SettingsName.Order.AutoAssignWeightAge);
            Order_AutoAssignMode.Items.Add(AppSetting.SettingsName.Order.AutoAssignFirstItem);
            Order_AutoAssignMode.Items.Add(AppSetting.SettingsName.Order.AutoAssignNo);
            Order_AutoAssignMode.SelectedIndex = Order_AutoAssignMode.Items.IndexOf(AppSetting.GetSetting(AppSetting.SettingsName.Order.AutoAssignMode));


            DO_UseCustomNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_UseCustomNo), false);
            DO_CustomPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_CustomPrefix);
            DO_ReceiptLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_ReceiptLength);
            DO_CurrentReceiptNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.DO_CurrentReceiptNo);

            UseRealTimeSyncMasterData.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMasterData), false);
            SyncMasterDataRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncMasterDataRecordsPerTime);
            MasterDataRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenConnected);
            MasterDataRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.MasterDataRetrySecWhenDisconnected);

            UseRealTimeSyncUser.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncUser), false);
            SyncUserRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncUserRecordsPerTime);
            UserRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.UserRetrySecWhenConnected);
            UserRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.UserRetrySecWhenDisconnected);

            UseRealTimeSyncMember.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncMember), false);
            SyncMemberRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncMemberRecordsPerTime);
            MemberRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.MemberRetrySecWhenConnected);
            MemberRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.MemberRetrySecWhenDisconnected);

            UseRealTimeSyncProducts.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncProducts), false);
            SyncProductRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncProductRecordsPerTime);
            ProductRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.ProductRetrySecWhenConnected);
            ProductRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.ProductRetrySecWhenDisconnected);

            UseRealTimeSyncAppointment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncAppointment), false);
            SyncAppointmentRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAppointmentRecordsPerTime);
            AppointmentRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.AppointmentRetrySecWhenConnected);
            AppointmentRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.AppointmentRetrySecWhenDisconnected);

            // Sync Delivery Order
            UseRealTimeSyncDeliveryOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncDeliveryOrder), false);
            SyncDeliveryOrderRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncDeliveryOrderRecordsPerTime);
            DeliveryOrderRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.DeliveryOrderRetrySecWhenConnected);
            DeliveryOrderRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.DeliveryOrderRetrySecWhenDisconnected);

            //Sync Attendance
            UseRealTimeSyncAttendance.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncAttendance), false);
            SyncAttendanceRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAttendanceRecordsPerTime);
            AttendanceRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.AttendanceRetrySecWhenDisconnected);

            //Sync Voucher
            UseRealTimeSyncVoucher.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncVoucher), false);
            SyncVoucherRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncVoucherRecordsPerTime);
            VoucherRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.VoucherRetrySecWhenConnected);
            VoucherRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.VoucherRetrySecWhenDisconnected);

            Inventory_ShowUOM.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowUOM), false);
            Inventory_ShowCurrency.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowCurrency), false);
            SupplierIsCompulsory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsory), false);
            SupplierIsCompulsoryPO.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SupplierIsCompulsoryPO), false);
            Inventory_DontUpdateFactoryPriceIfZero.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DontUpdateFactoryPriceIfZero), false);
            Inventory_ShowPreOrderQtyInStockOnHand.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowPreOrderQtyInStockOnHand), false);

            //Funding
            Funding_EnableFunding.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            Funding_EnablePAMed.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            Funding_EnableSMF.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            Funding_EnablePWF.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            Funding_PAMedPercentage.Text = AppSetting.GetSetting(AppSetting.SettingsName.Funding.PAMedPercentage);
            Funding_SMFPercentage.Text = AppSetting.GetSetting(AppSetting.SettingsName.Funding.SMFPercentage);

            Points_CanUseMultiplePackageInOneReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.CanUseMultiplePackageInOneReceipt), false);

            //Numbering
            tabControl1.TabPages.RemoveByKey("tpNumbering"); // Hide the settings for now, will use the setting from server instead
            Fill_PO_Reset_Combobox();
            PurchaseOrder_UseCustomNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.UseCustomNo), false);
            PurchaseOrder_CustomPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix);
            PurchaseOrder_CustomSuffix.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix);
            PurchaseOrder_NumberLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength);
            PurchaseOrder_CurrentNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo);
            PurchaseOrder_ResetNumberEvery.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery);
            PurchaseOrder_CustomNoDateFormat.Text = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat);
            PurchaseOrder_GenerateCustomNoInServer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.GenerateCustomNoInServer), false);
            Fill_GR_Reset_Combobox();
            GoodsReceive_UseCustomNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo), false);
            GoodsReceive_CustomPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix);
            GoodsReceive_CustomSuffix.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix);
            GoodsReceive_NumberLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength);
            GoodsReceive_CurrentNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo);
            GoodsReceive_ResetNumberEvery.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery);
            GoodsReceive_CustomNoDateFormat.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat);
            GoodsReceive_GenerateCustomNoInServer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.GenerateCustomNoInServer), false);

            //z2closing
            ShowClosingBreakdownOnZ2Printout.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Z2Report.ShowClosingBreakdownOnZ2Printout), false);
            PrintCategorySalesReportOnZ2Printout.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Z2Report.PrintCategorySalesReportOnZ2Printout), false);
            PrintProductSalesReportOnZ2Printout.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Z2Report.PrintProductSalesReportOnZ2Printout), false);

            //Inventory

            Inventory_OnlyAllowInCurrentInventoryLocation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false);
            Inventory_UpdateFactoryPriceOnStockIn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UpdateFactoryPriceOnStockIn), false);
            Inventory_BlockTransactionIfBalQtyNotSuf.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.BlockTransactionIfBalQtyNotSuf), false);
            Inventory_VendorInvoiceNoCompulsory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.VendorInvoiceNoCompulsory), false);
            Inventory_CheckQuantityonServer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CheckQuantityonServer), false);
            Inventory_AllowToUpdateRetailPriceInGoodsReceive.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowToUpdateRetailPriceInGoodsReceive), false);

            //Goods Receive
            GoodsReceive_ShowCustomField1.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField1), false);
            GoodsReceive_ShowCustomField2.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField2), false);
            GoodsReceive_ShowCustomField3.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField3), false);
            GoodsReceive_ShowCustomField4.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField4), false);
            GoodsReceive_ShowCustomField5.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowCustomField5), false);
            GoodsReceive_CustomField1Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField1Label);
            GoodsReceive_CustomField2Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField2Label);
            GoodsReceive_CustomField3Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField3Label);
            GoodsReceive_CustomField4Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField4Label);
            GoodsReceive_CustomField5Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomField5Label);
            GoodsReceive_ShowAdditionalCost1.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost1), false);
            GoodsReceive_ShowAdditionalCost2.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost2), false);
            GoodsReceive_ShowAdditionalCost3.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost3), false);
            GoodsReceive_ShowAdditionalCost4.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost4), false);
            GoodsReceive_ShowAdditionalCost5.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ShowAdditionalCost5), false);
            GoodsReceive_AdditionalCost1Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1Label);
            GoodsReceive_AdditionalCost2Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2Label);
            GoodsReceive_AdditionalCost3Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3Label);
            GoodsReceive_AdditionalCost4Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4Label);
            GoodsReceive_AdditionalCost5Label.Text = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5Label);
            GoodsReceive_AdditionalCost1_IsPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1_IsPercentage), false);
            GoodsReceive_AdditionalCost2_IsPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2_IsPercentage), false);
            GoodsReceive_AdditionalCost3_IsPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3_IsPercentage), false);
            GoodsReceive_AdditionalCost4_IsPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4_IsPercentage), false);
            GoodsReceive_AdditionalCost5_IsPercentage.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5_IsPercentage), false);

            //Discount
            ShowDiscountDescription.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.ShowDiscountDescription), false);
            UseRoundingForFinalPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.UseRoundingForFinalPrice), true);
            IsOverwriteExistingPromoOnButtonDiscountAll.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.IsOverwriteExistingPromoOnButtonDiscountAll), false);
            Discount_EnableMultiLevelPricing.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricing), false);
            Discount_EnableMultiLevelPricingInGlobalDiscount.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.EnableMultiLevelPricingInGlobalDiscount), false);
            Discount_AllowZeroMultiTierPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Discount.AllowZeroMultiTierPrice), false);

            //Formal Invoice
            IsRoundingPreferenceAccordingToConfig.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.IsRoundingPreferenceAccordingToConfig), false);
            UseSpecialGSTRuleForFormal.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.UseSpecialGSTRuleForFormal), false);
            SpecialGSTRule.Text = AppSetting.GetSetting(AppSetting.SettingsName.FormalInvoice.SpecialGSTRule);

            //Save PDF
            Print_SaveReceiptAsDocument.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.Print_SaveReceiptAsDocument), false);
            PDFPath.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.PDFPath);

            Sync_RealTimeAccessLog.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAccessLog), false);
            Sync_SyncAccessLogPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAccessLogPerTime);
            Sync_SyncAccessLogRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncAccessLogRetrySecWhenDisconnected);

            Sync_UseRealTimeSyncCashRecording.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncCashRecording), false);
            Sync_SyncCashRecordingPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncCashRecordingPerTime);
            Sync_SyncCashRecordingRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncCashRecordingRetrySecWhenDisconnected);

            Sync_UseRealTimeSyncPerformanceLog.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncPerformanceLog), false);
            Sync_SyncPerformanceLogPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncPerformanceLogPerTime);
            Sync_SyncPerformanceLogRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncPerformanceLogRetrySecWhenDisconnected);

            Inventory_ShowBalanceQuantityOnTransaction.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityOnTransaction), false);
            Inventory_AllowChangeInventoryDate.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowChangeInventoryDate), false);
            Inventory_ShowBalanceQuantityForStockTransfer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ShowBalanceQuantityForStockTransfer), false);
            Points_WaitToDownloadPointsBeforePrintReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WaitToDownloadPointsBeforePrintReceipt), false);

            Membership_DownloadAllRecentPurchase.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.DownloadAllRecentPurchase), false);
            Invoice_KickCashDrawerAfterAmountEntered.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.KickCashDrawerAfterAmountEntered), false);
            Invoice_MaxChangeAllowed.Text = AppSetting.GetSetting(AppSetting.SettingsName.Invoice.MaxChangeAllowed);

            UseFlyTechCashDrawer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseFlyTechCashDrawer), false);
            KickDrawerPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.KickDrawerPort);
            Inventory_SaveFileInTheServer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SaveFileInTheServer), false);

            Invoice_UseWeight.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseWeight), false);
            WeighingMachine_UseWeighingMachine.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.UseWeighingMachine), false);
            WeighingMachine_COMPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.COMPort);
            WeighingMachine_CommandToExecute.Text = AppSetting.GetSetting(AppSetting.SettingsName.WeighingMachine.CommandToExecute);

            //add UseContainerWeight
            Invoice_UseContainerWeight.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.UseContainerWeight), false);

            //add ShowPaymentTypeWhenZero
            Payment_ShowPaymentTypeWhenZero.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowPaymentTypeWhenZeo), false);

            ChangePriceStockAdjIssue.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.ChangePriceStockAdjIssue), false);

            //Appointment
            //SendAppointmentDirectlyToServer.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.SendAppointmentDirectlyToServer), false);
            LoadAppointmentSettings();
            Appointment_AppointmentSearchList.Text = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.AppointmentSearchList);
            Appointment_MembershipMandatory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.MembershipMandatory), false);
            Appointment_ServicesMandatory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.ServicesMandatory), false);
            Appointment_DisableTimeCollisionCheck.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.DisableTimeCollisionCheck), false);
            Appointment_UseWeeklyView.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseWeeklyView), false);
            Appointment_UseMonthlyView.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseMonthlyView), false);
            Appointment_UseResourceOnAppointment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Appointment.UseResourceOnAppointment), false);
            SalesPerson_OrderBy.Text = AppSetting.GetSetting(AppSetting.SettingsName.Appointment.SalesPerson_OrderBy);

            Invoice_EnterAsOK.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.EnterAsOK), false);
            Invoice_PreviousReceiptNoNotCompulsory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PreviousReceiptNoNotCompulsory), false);
            Invoice_PromptPasswordOnSelectSalesPerson.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Invoice.PromptPasswordOnSelectSalesPerson), false);

            QuotationReceiptFileLocation.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.QuotationReceiptFileLocation);
            Quotation_UseCustomNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_UseCustomNo), false);
            Quotation_CustomPrefix.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_CustomPrefix);
            Quotation_ReceiptLength.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_ReceiptLength);
            Quotation_CurrentReceiptNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Receipt.Quotation_CurrentReceiptNo);

            Refund_RefundReceiptFromSameOutlet.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Refund.RefundReceiptFromSameOutlet), false);
            HidePrintPackageBalanceOnReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), true);

            // SUPPLIER
            DisplayCurrencyOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayCurrencyOnSupplier), false);
            DisplayGSTOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayGSTOnSupplier), false);
            DisplayDeliveryChargeOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayDeliveryChargeOnSupplier), false);
            DisplayMinimumOrderOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayMinimumOrderOnSupplier), false);
            DisplayPaymentTermOnSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Supplier.DisplayPaymentTermOnSupplier), false);
            DefaultCurrency.Text = AppSetting.GetSetting(AppSetting.SettingsName.ItemSupplierMap.DefaultCurrency);

            try
            {
                SIGNATURE_DEVICE.SelectedItem = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
            }
            catch (Exception ex)
            {
                SIGNATURE_DEVICE.SelectedIndex = 0;
            }

            SecondScreen_RequireCustomerConfirm.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.RequireCustomerConfirm), false);
            SecondScreen_AskPrintEmailReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SecondScreen.AskPrintEmailReceipt), false);

            Promo_PromptNonPromoItemWhenConfirm.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.PromptNonPromoItemWhenConfirm), false);
            Promo_ShowPromoNameNoDetails.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.PromptNonPromoItemWhenConfirm), false);

            SalesInvoice_HideDiscountColumn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideDiscountColumn), false);
            SalesInvoice_HideSalesPersonColumn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideSalespersonColumn), false);
            SalesInvoice_HideSpecialColumn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideSpecialColumn), false);
            SalesInvoice_HideTaxColumn.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.HideTaxColumn), false);
            SalesInvoice_SortHotKeyByName.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.SortHotKeyByName), false);
            SalesInvoice_ShowUomInEditQtyForm.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowUomInEditQtyForm), false);
            SalesInvoice_ShowPossiblePromo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowPossiblePromo), true);

            ShowSalesWithoutCategoryFilter.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutCategoryFilter), false);
            ShowSalesWithoutItemDeptFilter.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.ShowSalesWithoutItemDeptFilter), false);
            ItemDeptFilterName.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.ItemDeptFilterName);

            Promo_EnableGrouping.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Promo.EnableGrouping), false);

            //SICC
            SICC_CheckValidationPrinting.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SICC.CheckValidationPrinting), false);
            PoleDisplay_DisablePoleDisplay.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.DisablePoleDisplay), false);


            #region *) Rating

            Rating_UseRatingSystem.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Rating.UseRatingSystem), false);

            UseRealTimeSyncRating.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSyncRating), false);
            SyncRatingRecordsPerTime.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.SyncRatingRecordsPerTime);
            RatingRetrySecWhenConnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.RatingRetrySecWhenConnected);
            RatingRetrySecWhenDisconnected.Text = AppSetting.GetSetting(AppSetting.SettingsName.Sync.RatingRetrySecWhenDisconnected);

            #endregion

            HideActualCollection.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Receipt.HideActualCollection), false);

            #region *) EZLink
            Payment_EnableEZLinkIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableEZLinkIntegration), false);
            Payment_EZLinkPaymentType.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType);
            #endregion

            #region Cash Recycler
            CashRecycler_EnableCashRecycler.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.EnableCashRecycler), false);
            CashRecycler_MachineType.Text = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.MachineType);
            CashRecycler_APIUrl.Text = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.APIUrl);
            CashRecycler_APIPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.APIPort);
            CashRecycler_Username.Text = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.Username);
            CashRecycler_Password.Text = AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.Password);
            #endregion

            #region NETS Integration
            Payment_EnableNETSIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            Payment_EnableNETSATMCard.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            Payment_EnableNETSFlashPay.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            Payment_EnableNETSCashCard.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            Payment_NETSPayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSPayment);
            rbNETSWithService.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSCashCardWithService), false);
            rbNETSWithoutService.Checked = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSCashCardWithService), false);

            Payment_EnableUNIONPayIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableUNIONPayIntegration), false);
            Payment_UNIONPayPayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.UNIONPayPayment);

            Payment_EnableBCAIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableBCAIntegration), false);
            Payment_BCAPayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.BCAPayment);

            //Payment_EnableCUPIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCUPIntegration), false);
            //Payment_CUPPayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.CUPPayment);

            Payment_EnablePrepaidPurchaseIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnablePrepaidPurchaseIntegration), false);
            Payment_PrepaidPurchasePayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PrepaidPurchasePayment);

            Payment_PaymentNETSCC_Grouped.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped);
            Payment_EnableCreditCardIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCreditCardIntegration), false);

            Payment_NetsThreadSleepWait.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NetsThreadSleepWait);
            //Payment_ShowOrderRefNoOnECRReceipt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowOrderRefNoOnECRReceipt), false);
            Payment_NetsCOMPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NetsCOMPort);

            Payment_PaymentNETSCC_VISA.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
            Payment_PaymentNETSCC_MASTER.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER);
            Payment_PaymentNETSCC_AMEX.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX);
            //Payment_PaymentNETSCC_DINERS.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_DINERS);
            //Payment_PaymentNETSCC_JCB.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_JCB);
            Payment_NETSVersion.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSVersion);
            Payment_UseUOBCreditCardPassThrough.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseUOBCreditCardPassThrough), false);

            #endregion

            #region Citibank
            Payment_EnableCitiBankIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCitiBankIntegration), false);
            Payment_CitiBankPayment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.CitiBankPayment);
            Payment_CitiBankCOMPort.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.CitiBankCOMPort);

            Payment_PaymentCitiBank_VISA.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_VISA);
            Payment_PaymentCitiBank_MASTER.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_MASTER);
            Payment_PaymentCitiBank_AMEX.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_AMEX);
            Payment_PaymentCitiBank_DINERS.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_DINERS);
            Payment_PaymentCitiBank_JCB.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_JCB);
            #endregion

            SalesInvoice_WaitUntilDrawerClosed.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.WaitUntilCashDrawerClosed), false);
            Closing_DepositBagMandatory.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.DepositBagMandatory), false);
            SalesInvoice_MaxHoldTransaction.Text = AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.MaxHoldTransaction);


            BarcodePrinter_CustomQtyActive.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.CustomQtyActive), false);
            BarcodePrinter_CustomQtyOrigin.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.CustomQtyOrigin);
            BarcodePrinter_Template.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.Template);
            BarcodePrinter_PrinterName.Items.Clear();

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                BarcodePrinter_PrinterName.Items.Add(printer);
            }

            BarcodePrinter_PrinterName.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.PrinterName);

            MembershipGroupCollection grp = new MembershipGroupCollection();
            grp.Load();

            grp.Add(new MembershipGroup() { GroupName = "" });
            Membership_DefaultMemberGroup.DataSource = grp.OrderBy(o => o.GroupName).ToList();
            Membership_DefaultMemberGroup.DisplayMember = "GroupName";
            Membership_DefaultMemberGroup.ValueMember = "GroupName";
            Membership_DefaultMemberGroup.Refresh();

            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.Membership.DefaultMembershipGroup)))
                Membership_DefaultMemberGroup.SelectedValue = AppSetting.GetSetting(AppSetting.SettingsName.Membership.DefaultMembershipGroup);


            Membership_AllowEditMemberGroup.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Membership.AllowEditMemberGroup), false);

            #region *) CRMIntegration
            Fill_CRMIntegration_Type_Combobox();
            CRMIntegration_IsEnabled.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.IsEnabled), false);
            if (!string.IsNullOrEmpty(AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.Type)))
                CRMIntegration_Type.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.Type);
            CRMIntegration_APIServer.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.APIServer);
            CRMIntegration_TerminalID.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.TerminalID);
            CRMIntegration_TerminalPwd.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.TerminalPwd);
            CRMIntegration_APIKey.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.APIKey);
            CRMIntegration_MerchantName.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.MerchantName);
            CRMIntegration_StaffName.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.StaffName);
            CRMIntegration_StaffCode.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.StaffCode);
            CRMIntegration_ItemNoForVoucher.Text = AppSetting.GetSetting(AppSetting.SettingsName.CRMIntegration.ItemNoForVoucher);
            #endregion

            Inventory_GoodsReceiveGSTRuleFromSupplier.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.GoodsReceiveGSTRuleFromSupplier), false);
            Inventory_UseBasicCostPrice.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.UseBasicCostPrice), false);
            Inventory_DownloadItemSummaryAllLocation.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.DownloadItemSummaryAllLocation), false);

            //Adi 2017.07.10 
            Order_UseUserTokenToGiveDiscount.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.UseUserTokenToGiveDiscount), false);

            //Adi 2017.10.15
            SalesInvoice_RoundingForAllPayment.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.RoundingForAllPayment), false);

            //Adi 2017.10.27
            Closing_DontAllowIfGotHold.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.DontAllowIfGotHold), false);

            //Adi 2017.11.29
            Payment_EnableNETSQR.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSQR), false);

            //Adi 2017.12.11
            BarcodePrinter_LabelWidth.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.LabelWidth);
            BarcodePrinter_NumberOfColumns.Text = AppSetting.GetSetting(AppSetting.SettingsName.BarcodePrinter.NumberOfColumns);

            //Adi 2017.01.08
            SalesInvoice_ShowPreviewButton.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ShowPreviewButton), false);
            SalesInvoice_DisableOpenPricePrompt.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.DisableOpenPricePrompt), false);

            //Adi 2018.02.20 -- Nets Card Cashback Option use different transaction type
            Payment_WithCashbackOption.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.WithCashbackOption), false);
            Payment_CashbackItemNo.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.CashbackItemNo);

            //Adi 2018.05.05 Setting for the grid font
            SalesInvoice_FontSize.Text = AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.FontSize);
            SalesInvoice_ResetPriceModeAfterScanItem.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.ResetPriceModeAfterScanItem), false);

            //Adi 2018.06.07
            SalesInvoice_PromptPasswordClearOrder.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.SalesInvoice.PromptPasswordClearOrder), false);

            Inventory_EnableProductSerialNo.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.EnableProductSerialNo), false);
            PrintPreOrderTemplateReport.Text = AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintPreOrderTemplateReport);
            Payment_UseUOBCreditCardIntegration.Checked = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseUOBCreditCardIntegration), false);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            Control Ctl = (Control)openFileDialog1.Tag;
            Ctl.Text = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
        }

        private void btn_Print_ReceiptFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_ReceiptFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnPrint_DeliveryOrderFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_DeliveryOrderFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void ProjectModule_CheckedChanged(object sender, EventArgs e)
        {
            if (ProjectModule.Checked)
            {
                CreateProjectTable();
            }
        }
        private void CreateProjectTable()
        {
            try
            {
                string SQL = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Project]') AND type in (N'U')) " +
                "BEGIN " +
                "CREATE TABLE [dbo].[Project]( " +
                "[ID] [int] IDENTITY(1,1) NOT NULL, " +
                "[ProjectName] [nvarchar](100) NULL, " +
                "[MembershipNumber] [nvarchar](100) NULL, " +
                "[CreatedOn] [datetime] NULL, " +
                "[CreatedBy] [varchar](50) NULL, " +
                "[ModifiedOn] [datetime] NULL, " +
                "[ModifiedBy] [varchar](50) NULL, " +
                "[UniqueID] [uniqueidentifier] NULL, " +
                "[userfld1] [varchar](50) NULL, " +
                "[userfld2] [varchar](50) NULL, " +
                "[userfld3] [varchar](50) NULL, " +
                "[userfld4] [varchar](50) NULL, " +
                "[userfld5] [varchar](50) NULL, " +
                "[userfld6] [varchar](50) NULL, " +
                "[userfld7] [varchar](50) NULL, " +
                "[userfld8] [varchar](50) NULL, " +
                "[userfld9] [varchar](50) NULL, " +
                "[userflag1] [bit] NULL, " +
                "[userflag2] [bit] NULL, " +
                "[userflag3] [bit] NULL, " +
                "[userflag4] [bit] NULL, " +
                "[userflag5] [bit] NULL, " +
                "[userfloat1] [money] NULL, " +
                "[userfloat2] [money] NULL, " +
                "[userfloat3] [money] NULL, " +
                "[userfloat4] [money] NULL, " +
                "[userfloat5] [money] NULL, " +
                "[userint1] [int] NULL, " +
                "[userint2] [int] NULL, " +
                "[userint3] [int] NULL, " +
                "[userint4] [int] NULL, " +
                "[userint5] [int] NULL, " +
                "CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED  " +
                "( " +
                "[ID] ASC " +
                ")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] " +
                ") ON [PRIMARY] " +
                "END;";
                SubSonic.DataService.ExecuteQuery(new QueryCommand(SQL));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable To Create Project Table");
            }

        }

        private void UseCustomInvoiceNo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tpPOSSetup_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_StockTakeReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_StockTakeReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void tpSecondScreen_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tpMallIntegration_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = CustomKickDrawerAppPath;
            openFileDialog1.ShowDialog();
        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void AllowDiscount_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MembershipWarningFields_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabMembership_Click(object sender, EventArgs e)
        {

        }

        private void tpRealTimeSales_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_PurchaseOrderTemplateLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_PurchaseOrderFileLocation;
            openFileDialog1.ShowDialog();
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

        private void Fill_CRMIntegration_Type_Combobox()
        {
            CRMIntegration_Type.Items.Clear();
            CRMIntegration_Type.Items.Add("");
            CRMIntegration_Type.Items.Add("POKET");
        }

        private void btnTestCustomPONo_Click(object sender, EventArgs e)
        {
            // Backup the current setting first
            string CustomPrefix = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix);
            string CustomSuffix = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix);
            string NumberLength = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength);
            string CurrentNo = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo);
            string ResetNumberEvery = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery);
            string CustomNoDateFormat = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat);
            string LastReset = AppSetting.GetSetting(AppSetting.SettingsName.PurchaseOrder.LastReset);

            try
            {
                // Save the new setting
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix, PurchaseOrder_CustomPrefix.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix, PurchaseOrder_CustomSuffix.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength, PurchaseOrder_NumberLength.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo, PurchaseOrder_CurrentNo.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery, PurchaseOrder_ResetNumberEvery.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat, PurchaseOrder_CustomNoDateFormat.Text);

                MessageBox.Show(PurchaseOdrController.CreateNewCustomRefNo(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Revert to old setting
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomPrefix, CustomPrefix);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomSuffix, CustomSuffix);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.NumberLength, NumberLength);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CurrentNo, CurrentNo);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.ResetNumberEvery, ResetNumberEvery);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.CustomNoDateFormat, CustomNoDateFormat);
                AppSetting.SetSetting(AppSetting.SettingsName.PurchaseOrder.LastReset, LastReset);
            }
        }

        private void btnTestCustomGRNo_Click(object sender, EventArgs e)
        {
            // Backup the current setting first
            string CustomPrefix = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix);
            string CustomSuffix = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix);
            string NumberLength = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength);
            string CurrentNo = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo);
            string ResetNumberEvery = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery);
            string CustomNoDateFormat = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat);
            string LastReset = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.LastReset);

            try
            {
                // Save the new setting
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix, GoodsReceive_CustomPrefix.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix, GoodsReceive_CustomSuffix.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength, GoodsReceive_NumberLength.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo, GoodsReceive_CurrentNo.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery, GoodsReceive_ResetNumberEvery.Text);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat, GoodsReceive_CustomNoDateFormat.Text);

                MessageBox.Show(InventoryController.CreateNewCustomRefNo(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Revert to old setting
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix, CustomPrefix);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix, CustomSuffix);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength, NumberLength);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo, CurrentNo);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery, ResetNumberEvery);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat, CustomNoDateFormat);
                AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.LastReset, LastReset);
            }
        }

        private void PurchaseOrder_GenerateCustomNoInServer_CheckedChanged(object sender, EventArgs e)
        {
            PurchaseOrder_CustomPrefix.ReadOnly = PurchaseOrder_GenerateCustomNoInServer.Checked;
            PurchaseOrder_CustomSuffix.ReadOnly = PurchaseOrder_GenerateCustomNoInServer.Checked;
            PurchaseOrder_NumberLength.ReadOnly = PurchaseOrder_GenerateCustomNoInServer.Checked;
            PurchaseOrder_CurrentNo.ReadOnly = PurchaseOrder_GenerateCustomNoInServer.Checked;
            PurchaseOrder_ResetNumberEvery.Enabled = !PurchaseOrder_GenerateCustomNoInServer.Checked;
            PurchaseOrder_CustomNoDateFormat.ReadOnly = PurchaseOrder_GenerateCustomNoInServer.Checked;
        }

        private void GoodsReceive_GenerateCustomNoInServer_CheckedChanged(object sender, EventArgs e)
        {
            GoodsReceive_CustomPrefix.ReadOnly = GoodsReceive_GenerateCustomNoInServer.Checked;
            GoodsReceive_CustomSuffix.ReadOnly = GoodsReceive_GenerateCustomNoInServer.Checked;
            GoodsReceive_NumberLength.ReadOnly = GoodsReceive_GenerateCustomNoInServer.Checked;
            GoodsReceive_CurrentNo.ReadOnly = GoodsReceive_GenerateCustomNoInServer.Checked;
            GoodsReceive_ResetNumberEvery.Enabled = !GoodsReceive_GenerateCustomNoInServer.Checked;
            GoodsReceive_CustomNoDateFormat.ReadOnly = GoodsReceive_GenerateCustomNoInServer.Checked;
        }

        private void btnPrint_GoodsReceiveReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_GoodsReceiveReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnPrint_OtherStockActivityReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_OtherStockActivityReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnExportProductsTemplateFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Template File|*.rpt";
            openFileDialog1.Tag = Export_ExportProductsTemplateFile;
            openFileDialog1.ShowDialog();
        }

        private void btnExportProductsToLocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Tag = Export_ExportProductsDirectory;
            folderBrowserDialog1.ShowDialog();
        }

        private void btnInvoice_PreOrderReceiptTemplate_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Invoice_PreOrderReceiptTemplate;
            openFileDialog1.ShowDialog();
        }

        private void btnInvoice_PreOrderNotifyTemplate_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Invoice_PreOrderNotifyTemplate;
            openFileDialog1.ShowDialog();
        }

        private void QuotationReceiptFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = QuotationReceiptFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void ShowSalesWithoutCategoryFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowSalesWithoutCategoryFilter.Checked)
            {
                if (ShowSalesWithoutItemDeptFilter.Checked)
                    ShowSalesWithoutItemDeptFilter.Checked = false;
            }
        }

        private void ShowSalesWithoutItemDeptFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowSalesWithoutItemDeptFilter.Checked)
            {
                if (ShowSalesWithoutCategoryFilter.Checked)
                    ShowSalesWithoutCategoryFilter.Checked = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Rating_UseRatingSystem_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void PoleDisplay_DisablePoleDisplay_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Inventory_RemoveZeroInventoryValidation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void IsReplaceConfirmTextButton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ReplaceConfirmTextButtonWith_TextChanged(object sender, EventArgs e)
        {

        }

        private void tpSupplier_Click(object sender, EventArgs e)
        {

        }

        private void tpBarcodePrinter_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_StockIssueReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_StockIssueReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnPrint_StockAdjustmentReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_StockAdjustmentReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnPrint_StockTransferReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_StockTransferReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnPrint_StockReturnReportFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = Print_StockReturnReportFileLocation;
            openFileDialog1.ShowDialog();
        }

        private void btnPreOrderDeliveryLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Print Template|*.rpt";
            openFileDialog1.Tag = PrintPreOrderTemplateReport;
            openFileDialog1.ShowDialog();
        }
    }
}
