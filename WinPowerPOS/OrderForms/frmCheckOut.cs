using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using SubSonic;
using PowerPOS.Container;
using POSDevices;
using WinPowerPOS.LoginForms;
using System.Collections;
using System.IO;
using System.Configuration;


namespace WinPowerPOS.OrderForms
{
    public partial class frmCheckOut : Form
    {
        private decimal variance;
        private decimal systemCollected;
        public bool IsSuccessful;
        private decimal cashIn, cashOut, openingBal;
        private decimal TotalCashColleted;
        private decimal TotalPay1Colleted;
        private decimal TotalPay2Colleted;
        private decimal TotalVoucherColleted;
        private decimal TotalPay4Colleted;
        private decimal TotalPay3Colleted;
        private decimal TotalPay5Colleted;
        private decimal TotalPay6Colleted;
        private decimal TotalPay7Colleted;
        private decimal TotalPay8Colleted;
        private decimal TotalPay9Colleted;
        private decimal TotalPay10Colleted;
        private decimal TotalChequeCollected;
        private decimal TotalPointCollected;
        private decimal TotalPackageCollected;
        private decimal TotalSMFCollected;
        private decimal TotalPAMedCollected;
        private decimal TotalPWFCollected;
        private decimal TotalNETSCashCardCollected;
        private decimal TotalNETSFlashPayCollected;
        private decimal TotalNETSATMCardCollected;
        private decimal TotalForeignCurrency1;
        private decimal TotalForeignCurrency2;
        private decimal TotalForeignCurrency3;
        private decimal TotalForeignCurrency4;
        private decimal TotalForeignCurrency5;
        private decimal TotalForeignCurrency;
        private bool isLoaded;
        private decimal installmentAmount;
        private decimal totalPaid;
        private bool enableNETSIntegration = false;
        public BackgroundWorker syncLogsThread;
        Dictionary<string, decimal> ForeignCurrency = new Dictionary<string, decimal>();
        bool hideRecordedSystemOnCheckout = false;
        public bool isClosed = false;


        #region "Form init and load"
        public frmCheckOut()
        {
            isLoaded = false;
            InitializeComponent();
            enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            hideRecordedSystemOnCheckout = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.HideSystemRecordedOnCheckOut), false);
        }
        private const int TOTAL_BUTTONS = 10;
        private void LoadPaymentTypeLabels()
        {
            for (int Counter = 1; Counter <= TOTAL_BUTTONS; Counter++)
            {
                Control[] tmp = this.Controls.Find("gbPay" + Counter.ToString(), true);
                if (tmp.Length > 0)
                {

                    GroupBox gbPay = (GroupBox)tmp[0];                    
                    gbPay.Text = PaymentTypesController.FetchPaymentByID(gbPay.Tag.ToString());
                    if (gbPay.Text == "" || (enableNETSIntegration && gbPay.Text.ToUpper().Contains("NETS")))
                    {
                        gbPay.Visible = false;
                    }
                    else
                    {
                        if (gbPay.Text.ToUpper() == "INSTALLMENT" ||
                            gbPay.Text.ToUpper() ==  "" ||
                            gbPay.Text.ToUpper() ==  "-" ||
                            gbPay.Text.ToUpper() ==  "POINTS")
                        {
                            gbPay.Visible = false;
                            Control[] tmp2 = this.Controls.Find("txtPay" + Counter.ToString(), true);
                            if (tmp2.Length > 0)
                            {
                                ((TextBox)tmp2[0]).Text = "0.00";
                            }
                            /*
                            Control[] tmp2 = this.Controls.Find("txtPay" + Counter.ToString(), true);
                            //Control[] tmp3 = this.Controls.Find("lblPay" + Counter.ToString(), true);

                            if (tmp2.Length > 0 )
                            {
                                ((TextBox)tmp2[0]).Text = installmentAmount.ToString("N2"); // ((Label)tmp3[0]).Text.Replace("$", "");
                                ((TextBox)tmp2[0]).Enabled = false;
                            }
                            */ 
                        }
                    }
                }
            }

            #region *) Show/hide Funding Method
            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enableSMF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableSMF), false);
            bool enablePAMed = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePAMed), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            gbSMF.Visible = (enableFunding && enableSMF);
            gbPAMed.Visible = (enableFunding && enablePAMed);
            gbPWF.Visible = (enableFunding && enablePWF);
            #endregion

            gbNETSATMCard.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            gbNETSCashCard.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            gbNETSFlashPay.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
        }

        private void frmCheckOut_Load(object sender, EventArgs e)
        {
            try
            {

                #region check Privileges For Close Counter
                DataRow[] dr = UserInfo.privileges.Select("privilegeName = '" + PrivilegesController.ACCEPT_CLOSE_COUNTER + "'");
                bool IsAuthorized = false;
                if (!(UserInfo.username.ToLower() == "edgeworks") && dr.Length <= 0)
                {
                    string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                    if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                    {

                        frmReadMSR f = new frmReadMSR();
                        f.privilegeName = PrivilegesController.ACCEPT_CLOSE_COUNTER;
                        f.loginType = LoginType.Authorizing;
                        f.ShowDialog();
                        IsAuthorized = f.IsAuthorized;
                        f.Dispose();
                    }
                    else
                    {
                        //Ask for verification....
                        //Prompt Supervisor Password
                        LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                        sl.privilegeName = PrivilegesController.ACCEPT_CLOSE_COUNTER;
                        sl.ShowDialog();
                        //SupID = sl.mySupervisor;
                        IsAuthorized = sl.IsAuthorized;
                    }
                }
                else
                {
                    IsAuthorized = true;
                }

                if (!IsAuthorized)
                {
                    this.Close();
                    return;
                }
                #endregion

                #region Check Hold Transaction if Hold Transaction Setting is enabled
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.DontAllowIfGotHold), false))
                {
                    //Check if got hold transaction
                    HoldController instance = new HoldController();
                    int holdCount = instance.ToDataTable().Rows.Count;
                    if (holdCount > 0)
                    {
                        MessageBox.Show("Please clear your pending transaction before proceed");
                        this.Close();
                        return;
                    }
                }
                #endregion

                bool enableChangeShift = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.EnableChangeShift),false);

                if (enableChangeShift != null && enableChangeShift)
                {
                    btnCloseShift.Visible = true;
                    btnResetShift.Visible = true;
                }
                //ShowPanel();
                dtpStartDate.Value = ClosingController.FetchOpeningShift(PointOfSaleInfo.PointOfSaleID);
                //txtNetsTerminalID.Text = PointOfSaleInfo.NETsTerminalID;
                txtFloatBalance.Text = openingBal.ToString("N");
                DateTime saved = ClosingController.ReadLastSaved(PointOfSaleInfo.PointOfSaleID);
                
                if (saved > dtpStartDate.Value)
                {
                    dtpEndDate.Value = saved;

                }

                //Show standard cash form or total cash only
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.UseTotalCashForCheckOut), false))
                {
                    gbTotalCash.Visible = true;
                    gbTotalCash.BringToFront();
                    gbCashBreakdown.Visible = false;
                }
                else
                {
                    gbTotalCash.Visible = false;

                    gbCashBreakdown.Visible = true;
                }

                #region Hide System Recorded
                if (hideRecordedSystemOnCheckout)
                {
                    hideSystemRecorded();
                }
                #endregion

                string FloatBalance, NetsCollected, NetsTerminalID, VisaCollected,
                VisaBatchNo, AmexCollected, AmexBatchNo, MasterCollected,
                ChinaNetsTerminalID, VoucherCollected, ChequeCollected, DepositBagNo,
                PointOfSaleID, C100, C50, C10, C5, C2, C1, C050, C020, C010, C005;

                //Load total Installment for the day
                installmentAmount =
                    ReceiptController.FetchTotalInstallment(dtpStartDate.Value,
                    dtpEndDate.Value, PointOfSaleInfo.PointOfSaleID, out totalPaid);
                
                LoadPaymentTypeLabels();

                //Load...
                if (ClosingController.FetchSavedClosing(out FloatBalance, out  NetsCollected, out  
                    NetsTerminalID, out  VisaCollected, out  VisaBatchNo, out  
                    AmexCollected, out  AmexBatchNo, out  MasterCollected, out  
                    ChinaNetsTerminalID, out  VoucherCollected, out  ChequeCollected, out 
                    DepositBagNo, out  PointOfSaleID, out  C100, out  C50, out  C10, out  
                    C5, out  C2, out  C1, out  C050, out  C020, out  C010, out  C005))
                {
                    //if there is record.. load...
                    txtFloatBalance.Text = FloatBalance;
                    txtPay1.Text = NetsCollected;
                    //txtNetsTerminalID.Text = NetsTerminalID;
                    txtPay2.Text = VisaCollected;
                    //txtVisaBatchNo.Text = VisaBatchNo;
                    txtPay4.Text = AmexCollected;
                    //txtAmexBatchNo.Text = AmexBatchNo;
                    txtPay3.Text = MasterCollected;
                    //txtChinaNetsTerminalID.Text = ChinaNetsTerminalID;
                    txtPay5.Text = NetsTerminalID;
                    txtPay6.Text = VisaBatchNo;

                    txtVoucher.Text = VoucherCollected;
                    txtChequeAmt.Text = ChequeCollected;
                    txtDepositBagNo.Text = DepositBagNo;
                    txt100.Text = C100;
                    txt50.Text = C50;
                    txt10.Text = C10;
                    txt5.Text = C5;
                    txt2.Text = C2;
                    txt1.Text = C1;
                    txt050.Text = C050;
                    txt020.Text = C020;
                    txt010.Text = C010;
                    txt005.Text = C005;
                }

                //txtInstallment.Text = installmentAmount.ToString("N2");

                //cashier ID
                lblCashierID.Text = PowerPOS.Container.UserInfo.username;

                //display breakdown of collected
                displayCollectedOfIndividualPaymentMode();

                //display data                
                txt100_Validating(this, new CancelEventArgs());

                //kick  
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseCustomKickDrawer), false))
                {
                    #region *) Core: Run External Script (for Landlord Integration)
                    if (AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != null
                        && AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath) != "")
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString());
                        }
                        catch (Exception ex)
                        {
                            Logger.writeLog("Unable start remote process: " + AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.CustomKickDrawerAppPath).ToString() + " " + ex.Message);
                        }
                    }
                    #endregion
                }
                else
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.UseFlyTechCashDrawer), false))
                    {
                        FlyTechCashDrawer cdrw = new FlyTechCashDrawer();
                        cdrw.OpenDrawer(AppSetting.GetSetting(AppSetting.SettingsName.CustomKickDrawer.KickDrawerPort));
                    }
                    else
                    {
                        CashDrawer drw = new CashDrawer();
                        drw.OpenDrawer();
                    }

                isLoaded = true;
                isClosed = false;

            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

        private void hideSystemRecorded()
        {
            label44.Visible = false;
            label99.Visible = false;
            
            lblPay4.Visible = false;
            lblDefi4.Visible = false;
            
            label46.Visible = false;
            label93.Visible = false;
            
            lblPay3.Visible = false;
            lblDefi3.Visible = false;


            label45.Visible = false;
            label96.Visible = false;
           
            lblPay1.Visible = false;
            lblDefi1.Visible = false;


            label49.Visible = false;
            label102.Visible = false;
            
            lblPay2.Visible = false;
            lblDefi2.Visible = false;


            label119.Visible = false;
            label116.Visible = false;
            
            lblPay5.Visible = false;
            lblDefi5.Visible = false;

            label111.Visible = false;
            
            label104.Visible = false;
            
            lblPay6.Visible = false;
            lblDefi6.Visible = false;

            lblPay7.Visible = false;
            lblDefi7.Visible = false;

            lblPay8.Visible = false;
            lblDefi8.Visible = false;

            lblPay9.Visible = false;
            lblDefi9.Visible = false;

            lblPay10.Visible = false;
            lblDefi10.Visible = false;

            label76.Visible = false;
            label73.Visible = false;
            lblChequeAmt.Visible = false;
            lblChequeDefi.Visible = false;

            label23.Visible = false;
            label105.Visible = false;
            lblVoucherCollected.Visible = false;
            lblVoucherDefi.Visible = false;

            label122.Visible = false;            
            label110.Visible = false;            
            lblSMFCollected.Visible = false;
            lblSMFDefi.Visible = false;

            label130.Visible = false;           
            label127.Visible = false;            
            lblPAMedCollected.Visible = false;
            lblPAMedDefi.Visible = false;

            label138.Visible = false;            
            label135.Visible = false;            
            lblPWFCollected.Visible = false;
            lblPWFDefi.Visible = false;

            label47.Visible = false;
            
            lblCashCollected.Visible = false;

            label63.Visible = false;
            
            lblTotalCashExpected.Visible = false;

            label3.Visible = false;
            
            lblCollected.Visible = false;

            label22.Visible = false;
            
            lblCashActivity.Visible = false;

            label11.Visible = false;
           
            lblVariance.Visible = false;

            label108.Visible = false;
            
            lblCashDefi.Visible = false;

            gbCash.Visible = false;
            groupBox3.Visible = false;
            gbTotal.Visible = false;

        }

        private void displayCollectedOfIndividualPaymentMode()
        {
            try
            {
                int PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                //Get cash in, cash out and etc
                CashRecordingController crCtrl = new CashRecordingController();

                crCtrl.fetchCashRecordingSummary(dtpStartDate.Value, dtpEndDate.Value, PointOfSaleID, out cashIn, out cashOut, out openingBal);

                lblNoOfBill.Text = ClosingController.GetTotalNumberOfOrder(dtpStartDate.Value, dtpEndDate.Value,
                       PointOfSaleID.ToString());

                //Fetch Opening Balance Setting if Applicable then override the opening bal
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.UseOpeningBalance), false))
                {
                    decimal.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.DefaultOpeningBalance), out openingBal);
                }

                lblCashIn.Text = cashIn.ToString("N");
                lblCashOut.Text = cashOut.ToString("N");
                lblOpeningAmt.Text = openingBal.ToString("N");
                ReceiptController.GetSystemCollectedBreakdownByPaymentType(
                    dtpStartDate.Value, dtpEndDate.Value, PointOfSaleID,
                     out TotalCashColleted,
                     out TotalPay1Colleted,
                     out TotalPay2Colleted,
                     out TotalVoucherColleted,                
                     out TotalPay3Colleted,
                     out TotalPay4Colleted,
                     out TotalPay5Colleted,
                     out TotalPay6Colleted,
                     out TotalPay7Colleted,
                     out TotalPay8Colleted,
                     out TotalPay9Colleted,
                     out TotalPay10Colleted,
                     out TotalChequeCollected,
                     out TotalPointCollected,
                     out TotalPackageCollected,
                     out TotalSMFCollected,
                     out TotalPAMedCollected,
                     out TotalPWFCollected,
                     out TotalNETSCashCardCollected,
                     out TotalNETSFlashPayCollected,
                     out TotalNETSATMCardCollected,
                     out TotalForeignCurrency,
                     out ForeignCurrency);

                //Fetch Voucher Redeemed amount
                TotalVoucherColleted =
                    ClosingController.FetchVoucherCollected
                    (PointOfSaleID, dtpStartDate.Value, dtpEndDate.Value);

                lblPay4.Text = TotalPay4Colleted.ToString("N");
                lblCashCollected.Text = TotalCashColleted.ToString("N");
                lblPay3.Text = TotalPay3Colleted.ToString("N");
                lblPay1.Text = TotalPay1Colleted.ToString("N");
                lblPay2.Text = TotalPay2Colleted.ToString("N");
                lblPay5.Text = TotalPay5Colleted.ToString("N");
                lblPay6.Text = TotalPay6Colleted.ToString("N");
                lblPay7.Text = TotalPay7Colleted.ToString("N");
                lblPay8.Text = TotalPay8Colleted.ToString("N");
                lblPay9.Text = TotalPay9Colleted.ToString("N");
                lblPay10.Text = TotalPay10Colleted.ToString("N");
                lblVoucherCollected.Text = TotalVoucherColleted.ToString("N");
                lblTotalCashExpected.Text = (TotalCashColleted + openingBal + cashIn - cashOut).ToString("N");
                lblChequeAmt.Text = TotalChequeCollected.ToString("N");
                lblPointRedemption.Text = TotalPointCollected.ToString("N");
                lblPackageRedemption.Text = TotalPackageCollected.ToString("N");
                lblRedemption.Text = (TotalPointCollected + TotalPackageCollected).ToString("N");
                lblSMFCollected.Text = TotalSMFCollected.ToString("N");
                lblPAMedCollected.Text = TotalPAMedCollected.ToString("N");
                lblPWFCollected.Text = TotalPWFCollected.ToString("N");
                lblNETSFlashPayAmt.Text = TotalNETSFlashPayCollected.ToString("N");
                lblNETSCashCardAmt.Text = TotalNETSCashCardCollected.ToString("N");
                lblNETSATMCardAmt.Text = TotalNETSATMCardCollected.ToString("N");
                
                lblTotalForeignCurrency.Text = TotalForeignCurrency.ToString("N");
                lblTotalForeignCurrency.Visible = TotalForeignCurrency != 0;
                lblDefaultCurrencySymbol.Visible = TotalForeignCurrency != 0;
                lblTotalForeignCurrencyText.Visible = TotalForeignCurrency != 0;
                lblDefaultCurrencyCode.Visible = TotalForeignCurrency != 0;

                string defaultCurrency = AppSetting.GetSetting(AppSetting.SettingsName.Currency.DefaultCurrency);
                Currency defCurr = new Currency(Currency.Columns.CurrencyCode, defaultCurrency);
                if (!defCurr.IsNew)
                {
                    if (!string.IsNullOrEmpty(defCurr.CurrencySymbol))
                        lblDefaultCurrencySymbol.Text = defCurr.CurrencySymbol;
                    else
                        lblDefaultCurrencySymbol.Text = "";
                    lblDefaultCurrencyCode.Text = string.Format("({0})", defCurr.CurrencyCode);
                }

                systemCollected = ReceiptController.GetTotalSystemCollected(dtpStartDate.Value, dtpEndDate.Value, PointOfSaleID);
                systemCollected += TotalVoucherColleted;
                systemCollected += (cashIn - cashOut);
                systemCollected -= installmentAmount ;
                systemCollected -= totalPaid;
                systemCollected -= (TotalPointCollected + TotalPackageCollected);

                gbForeignCurrency1.Visible = false;
                gbForeignCurrency2.Visible = false;
                gbForeignCurrency3.Visible = false;
                gbForeignCurrency4.Visible = false;
                gbForeignCurrency5.Visible = false;

                int count = 0;
                foreach (var item in ForeignCurrency)
                {
                    count ++;
                    if (count == 1)
                    {
                        gbForeignCurrency1.Visible = true;
                        gbForeignCurrency1.Tag = item.Key;
                        gbForeignCurrency1.Text = item.Key;
                        lblForeignCurrency1PayAmt.Text = item.Value.ToString("N");
                        TotalForeignCurrency1 = item.Value;
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        {
                            lblCurrSign1_1.Text = curr.CurrencySymbol;
                            lblCurrSign1_2.Text = curr.CurrencySymbol;
                            lblCurrSign1_3.Text = curr.CurrencySymbol;
                        }
                    }
                    else if (count == 2)
                    {
                        gbForeignCurrency2.Visible = true;
                        gbForeignCurrency2.Tag = item.Key;
                        gbForeignCurrency2.Text = item.Key;
                        lblForeignCurrency2PayAmt.Text = item.Value.ToString("N");
                        TotalForeignCurrency2 = item.Value;
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        {
                            lblCurrSign2_1.Text = curr.CurrencySymbol;
                            lblCurrSign2_2.Text = curr.CurrencySymbol;
                            lblCurrSign2_3.Text = curr.CurrencySymbol;
                        }
                    }
                    else if (count == 3)
                    {
                        gbForeignCurrency3.Visible = true;
                        gbForeignCurrency3.Tag = item.Key;
                        gbForeignCurrency3.Text = item.Key;
                        lblForeignCurrency3PayAmt.Text = item.Value.ToString("N");
                        TotalForeignCurrency3 = item.Value;
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        {
                            lblCurrSign3_1.Text = curr.CurrencySymbol;
                            lblCurrSign3_2.Text = curr.CurrencySymbol;
                            lblCurrSign3_3.Text = curr.CurrencySymbol;
                        }
                    }
                    else if (count == 4)
                    {
                        gbForeignCurrency4.Visible = true;
                        gbForeignCurrency4.Tag = item.Key;
                        gbForeignCurrency4.Text = item.Key;
                        lblForeignCurrency4PayAmt.Text = item.Value.ToString("N");
                        TotalForeignCurrency4 = item.Value;
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        {
                            lblCurrSign4_1.Text = curr.CurrencySymbol;
                            lblCurrSign4_2.Text = curr.CurrencySymbol;
                            lblCurrSign4_3.Text = curr.CurrencySymbol;
                        }
                    }
                    else if (count == 5)
                    {
                        gbForeignCurrency5.Visible = true;
                        gbForeignCurrency5.Tag = item.Key;
                        gbForeignCurrency5.Text = item.Key;
                        lblForeignCurrency5PayAmt.Text = item.Value.ToString("N");
                        TotalForeignCurrency5 = item.Value;
                        Currency curr = new Currency(Currency.Columns.CurrencyCode, item.Key.Replace("CASH-", ""));
                        if (!curr.IsNew && !string.IsNullOrEmpty(curr.CurrencySymbol))
                        {
                            lblCurrSign5_1.Text = curr.CurrencySymbol;
                            lblCurrSign5_2.Text = curr.CurrencySymbol;
                            lblCurrSign5_3.Text = curr.CurrencySymbol;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region "Display Cash Recording info at screen upon events"
        private void displayCashRecordingData()
        {
            try
            {
                int PointOfSaleID = PointOfSaleInfo.PointOfSaleID;

                decimal floatAmt = decimal.Parse(txtFloatBalance.Text);
                //CALCULATE ALL THE CLOSING AMOUNTS SPECIFIED BY USERS!
                decimal actualCollected;
                if (!decimal.TryParse(txtTotalCollected.Text, out actualCollected))
                {
                    MessageBox.Show("Please enter valid amount for Total Collected.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalCollected.Focus();
                    return;
                }

                if (enableNETSIntegration)
                {
                    actualCollected = actualCollected
                                    + txtPay1.Text.GetDecimalValue()
                                    + txtPay2.Text.GetDecimalValue()
                                    + txtPay3.Text.GetDecimalValue()
                                    + txtPay4.Text.GetDecimalValue()
                                    + txtPay5.Text.GetDecimalValue()
                                    + txtPay6.Text.GetDecimalValue()
                                    + txtPay7.Text.GetDecimalValue()
                                    + txtPay8.Text.GetDecimalValue()
                                    + txtPay9.Text.GetDecimalValue()
                                    + txtPay10.Text.GetDecimalValue()
                                    + txtVoucher.Text.GetDecimalValue()
                                    + txtSMF.Text.GetDecimalValue()
                                    + txtPAMed.Text.GetDecimalValue()
                                    + txtPWF.Text.GetDecimalValue()
                                    + txtChequeAmt.Text.GetDecimalValue()
                                    + txtNETSFlashPay.Text.GetDecimalValue()
                                    + txtNETSCashCard.Text.GetDecimalValue()
                                    + txtNETSATMCard.Text.GetDecimalValue()
                                    - openingBal;
                }
                else
                {
                    actualCollected = actualCollected
                                    + txtPay1.Text.GetDecimalValue()
                                    + txtPay2.Text.GetDecimalValue()
                                    + txtPay3.Text.GetDecimalValue()
                                    + txtPay4.Text.GetDecimalValue()
                                    + txtPay5.Text.GetDecimalValue()
                                    + txtPay6.Text.GetDecimalValue()
                                    + txtPay7.Text.GetDecimalValue()
                                    + txtPay8.Text.GetDecimalValue()
                                    + txtPay9.Text.GetDecimalValue()
                                    + txtPay10.Text.GetDecimalValue()
                                    + txtVoucher.Text.GetDecimalValue()
                                    + txtSMF.Text.GetDecimalValue()
                                    + txtPAMed.Text.GetDecimalValue()
                                    + txtPWF.Text.GetDecimalValue()
                                    + txtChequeAmt.Text.GetDecimalValue()
                                    - openingBal;
                }

                //if (gbForeignCurrency1.Visible)
                //    actualCollected += txtForeignCurrency1.Text.GetDecimalValue();
                //if (gbForeignCurrency2.Visible)
                //    actualCollected += txtForeignCurrency2.Text.GetDecimalValue();
                //if (gbForeignCurrency3.Visible)
                //    actualCollected += txtForeignCurrency3.Text.GetDecimalValue();
                //if (gbForeignCurrency4.Visible)
                //    actualCollected += txtForeignCurrency4.Text.GetDecimalValue();
                //if (gbForeignCurrency5.Visible)
                //    actualCollected += txtForeignCurrency5.Text.GetDecimalValue();
                //actualCollected = actualCollected - openingBal + floatAmt;
                //closeBal = closeBal - decimal.Parse(txtCashOut.Text) - decimal.Parse(txtFloatBalance.Text);
                //lblCashActivity.Text = (openingBal + cashIn - cashOut - floatAmt).ToString("N2");
                lblCashActivity.Text = (cashIn - cashOut).ToString("N");
                lblActual.Text = actualCollected.ToString("N");

                //lblTotalCashExpected.Text = "$" + (TotalCashColleted + openingBal + cashOut - cashOut - decimal.Parse(txtCashOut.Text) - decimal.Parse(txtFloatBalance.Text)).ToString("N2");


                //CALCULATE VARIANCES
                decimal PointsAndPackage = 0;
                decimal.TryParse(lblRedemption.Text, out PointsAndPackage);
                decimal systemCollectedMinPoints = decimal.Parse(systemCollected.ToString("N"));// -PointsAndPackage;
                //total = openingBal + systemCollected - floatAmt;
                lblCollected.Text = systemCollectedMinPoints.ToString("N"); //total.ToString("N2");
                //lblCollected.Text = total.ToString("N2") + decimal.Parse(txtFloatBalance.Text);
                variance = actualCollected - systemCollectedMinPoints;

                if (!hideRecordedSystemOnCheckout)
                {
                    if (variance > 0)
                    {
                        lblVariance.ForeColor = System.Drawing.Color.DarkGreen;
                        lblVariance.Text = variance.ToString("N");
                        gbTotal.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else if (variance < 0)
                    {
                        lblVariance.ForeColor = System.Drawing.Color.DarkRed;
                        lblVariance.Text = variance.ToString("N");
                        gbTotal.BackColor = System.Drawing.Color.Salmon;
                    }
                    else
                    {
                        lblVariance.ForeColor = System.Drawing.Color.Black;
                        lblVariance.Text = variance.ToString("N");
                        gbTotal.BackColor = System.Drawing.Color.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            displayCashRecordingData();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            displayCashRecordingData();
        }

        private void txt100_KeyDown(object sender, KeyEventArgs e)
        {
            txt100_Validating(sender, new CancelEventArgs());
        }
        #endregion

        #region "Save Cash Recording to DB"
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //-----------------VALIDATIONS-----------------------------
            #region *) Validation: DepositBagNo cannot be empty [Exit if False]
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.DepositBagMandatory), false))
            {
                if (txtDepositBagNo.Text == "")
                {
                    MessageBox.Show("Please specify deposit bag no", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDepositBagNo.Focus();
                    return;
                }
            }
            #endregion

            #region *) Confirmation: 
            DialogResult dr = MessageBox.Show("Are you sure you want to close counter?", "Close counter", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No) return;
            #endregion

            #region *) Warning: If closing amount is not tally
            if (variance != 0)
            {
                dr = MessageBox.Show
                    ("WARNING!!! THE CLOSING AMOUNT DOES NOT TALLY!! ARE YOU SURE YOU WANT TO CLOSE?",
                    "Close counter", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                    return;
            }
            #endregion

            try
            {
                decimal mFloatBalance;
                decimal mTotalCollected;
                decimal mNETSAmt;
                decimal mVisaAmt;
                decimal mAmexAmt;
                decimal mPay5;
                decimal mPay6;
                decimal mPay7;
                decimal mPay8;
                decimal mPay9;
                decimal mPay10;
                decimal mMasterCollected;
                decimal mVoucher;
                decimal mChequeAmt;
                decimal mPointAmt;
                decimal mPackageAmt;
                decimal mXTotalCollected;
                decimal mSMF;
                decimal mPAMed;
                decimal mPWF;
                decimal mNETSCashCardCollected;
                decimal mNETSFlashCardCollected;
                decimal mNETSATMCardCollected;
                decimal mForeignCurrency1 = 0;
                decimal mForeignCurrency2 = 0;
                decimal mForeignCurrency3 = 0;
                decimal mForeignCurrency4 = 0;
                decimal mForeignCurrency5 = 0;


                #region *) Quarry: Fetch all the figures
                if (!decimal.TryParse(txtFloatBalance.Text, out mFloatBalance) |
                        !decimal.TryParse(txtTotalCollected.Text, out mTotalCollected) |
                        !decimal.TryParse(txtPay1.Text, out mNETSAmt) |
                        !decimal.TryParse(txtPay2.Text, out mVisaAmt) |
                        !decimal.TryParse(txtPay3.Text, out mMasterCollected) |
                        !decimal.TryParse(txtPay4.Text, out mAmexAmt) |
                        !decimal.TryParse(txtPay5.Text, out mPay5) |
                        !decimal.TryParse(txtPay6.Text, out mPay6) |
                        !decimal.TryParse(txtPay7.Text, out mPay7) |
                        !decimal.TryParse(txtPay8.Text, out mPay8) |
                        !decimal.TryParse(txtPay9.Text, out mPay9) |
                        !decimal.TryParse(txtPay10.Text, out mPay10) |
                        !decimal.TryParse(txtVoucher.Text, out mVoucher) |
                        !decimal.TryParse(txtChequeAmt.Text, out mChequeAmt) |
                        !decimal.TryParse(txtSMF.Text, out mSMF) |
                        !decimal.TryParse(txtPAMed.Text, out mPAMed) |
                        !decimal.TryParse(txtPWF.Text, out mPWF) |
                        !decimal.TryParse(txtTotalCollected.Text, out mXTotalCollected) |
                        !decimal.TryParse(lblPointRedemption.Text, out mPointAmt) |
                        !decimal.TryParse(lblPackageRedemption.Text, out mPackageAmt) |
                        !decimal.TryParse(txtNETSCashCard.Text, out mNETSCashCardCollected) |
                        !decimal.TryParse(txtNETSFlashPay.Text, out mNETSFlashCardCollected) |
                        !decimal.TryParse(txtNETSATMCard.Text, out mNETSATMCardCollected) |
                        (gbForeignCurrency1.Visible && !decimal.TryParse(txtForeignCurrency1.Text, out mForeignCurrency1)) |
                        (gbForeignCurrency2.Visible && !decimal.TryParse(txtForeignCurrency2.Text, out mForeignCurrency2)) |
                        (gbForeignCurrency3.Visible && !decimal.TryParse(txtForeignCurrency3.Text, out mForeignCurrency3)) |
                        (gbForeignCurrency4.Visible && !decimal.TryParse(txtForeignCurrency4.Text, out mForeignCurrency4)) |
                        (gbForeignCurrency5.Visible && !decimal.TryParse(txtForeignCurrency5.Text, out mForeignCurrency5)))
                {
                    MessageBox.Show("Please enter valid numeric value for all the textboxes.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion

                string CashRecRefNo = "";
                string CounterClosedLogId = "";
                int PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                int runningNo = 0;
                IDataReader ds;
                bool IsAuthorized;
                string supName;

                #region *) Authorize: Get supervisor password
                bool Prompt = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnCheckOut), true);
                if (Prompt)
                {
                    //frmSupervisorLogin sl = new frmSupervisorLogin();
                    //sl.privilegeName = PrivilegesController.CLOSE_COUNTER;
                    //sl.ShowDialog();
                    //IsAuthorized = sl.IsAuthorized;

                    //supName = sl.mySupervisor;
                    //sl.Dispose();
                    string SupID = "-";
                    /*Prompt =  AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.PromptPassword.OnVoid), true);
                    if (Prompt)
                    {*/
                        string useMagenticStripReader = AppSetting.GetSetting(AppSetting.SettingsName.UseMagneticStripReader.ForAuthorizing);
                        if (useMagenticStripReader != null && useMagenticStripReader.ToLower() == "yes")
                        {

                            frmReadMSR f = new frmReadMSR();
                            f.privilegeName = PrivilegesController.CLOSE_COUNTER;
                            f.loginType = LoginType.Authorizing;
                            f.ShowDialog();
                            supName = f.buffer;
                            IsAuthorized = f.IsAuthorized;
                            f.Dispose();
                        }
                        else
                        {
                            //Ask for verification....
                            //Prompt Supervisor Password
                            LoginForms.frmSupervisorLogin sl = new LoginForms.frmSupervisorLogin();
                            sl.privilegeName = PrivilegesController.CLOSE_COUNTER;
                            sl.ShowDialog();
                            supName = sl.mySupervisor;
                            IsAuthorized = sl.IsAuthorized;
                        }
                    /*}
                    else
                    { IsAuthorized = true; supName = "-"; }*/
                }
                else
                { IsAuthorized = true; supName = UserInfo.username; }

                if (!IsAuthorized) return;
                #endregion

                QueryCommandCollection cmd = new QueryCommandCollection();
                QueryCommand mycmd;

                #region *) Core: Create Cash Recording into DB
                CashRecording cr = new CashRecording();
                // change from captured total sales, now save total cash only instead
                //cr.Amount = decimal.Parse(lblCollected.Text.ToString().Replace("$", ""));
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecording.SaveTotalCashOnlyWhenCheckOut), false))
                {
                    cr.Amount = decimal.Parse(lblTotalCashExpected.Text.ToString().Replace("$", ""));
                }
                else
                {
                    cr.Amount = decimal.Parse(lblCollected.Text.ToString().Replace("$", ""));
                }
                cr.CashierName = lblCashierID.Text;
                cr.CashRecordingTime = DateTime.Now;
                cr.CashRecordingTypeId = 4;
                ds = PowerPOS.SPs.GetNewCashRecRefNoByPointOfSaleID(PointOfSaleID).GetReader();
                while (ds.Read())
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                ds.Close();
                runningNo += 1;
                //CRYYMMDDSSSSNNNN                
                //YY - year
                //MM - month
                //DD - day
                //SSSS - PointOfSale ID
                //NNNN - Running No
                CashRecRefNo = "CR" + DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
                cr.CashRecRefNo = CashRecRefNo;
                cr.SupervisorName = supName;
                cr.PointOfSaleID = PointOfSaleID;
                cr.UniqueID = Guid.NewGuid();
                
                mycmd = cr.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                #region *) Core: Save to collection report
                CounterCloseLog myCounter = new CounterCloseLog();
                runningNo = 0;
                ds = PowerPOS.SPs.GetNewCounterCloseIDByPointOfSaleID(PointOfSaleID).GetReader();
                while (ds.Read())
                {
                    if (!int.TryParse(ds.GetValue(0).ToString(), out runningNo))
                    {
                        runningNo = 0;
                    }
                }
                ds.Close();
                runningNo += 1;
                CounterClosedLogId = "CL" + DateTime.Now.ToString("yyMMdd") + PointOfSaleID.ToString().PadLeft(4, '0') + runningNo.ToString().PadLeft(4, '0');
                myCounter.CounterCloseID = CounterClosedLogId;
                myCounter.Cashier = lblCashierID.Text;
                myCounter.FloatBalance = mFloatBalance;
                myCounter.CashRecorded = TotalCashColleted;                                
                myCounter.NetsRecorded = TotalPay1Colleted;
                myCounter.VisaRecorded = TotalPay2Colleted;
                myCounter.ChinaNetsRecorded = TotalPay3Colleted;
                myCounter.AmexRecorded = TotalPay4Colleted;
                myCounter.Pay5Recorded = TotalPay5Colleted;
                myCounter.Pay6Recorded = TotalPay6Colleted;
                myCounter.Pay7Recorded = TotalPay7Colleted;
                myCounter.Pay8Recorded = TotalPay8Colleted;
                myCounter.Pay9Recorded = TotalPay9Colleted;
                myCounter.Pay10Recorded = TotalPay10Colleted;
                myCounter.ChequeRecorded = 0;
                myCounter.CashCollected = mXTotalCollected - openingBal;
                myCounter.NetsCollected = mNETSAmt;
                myCounter.VisaCollected = mVisaAmt;
                myCounter.AmexCollected = mAmexAmt;
                myCounter.ChinaNetsCollected = mMasterCollected;
                myCounter.Pay5Collected = mPay5;
                myCounter.Pay6Collected = mPay6;
                myCounter.Pay7Collected = mPay7;
                myCounter.Pay8Collected = mPay8;
                myCounter.Pay9Collected = mPay9;
                myCounter.Pay10Collected = mPay10;
                myCounter.VoucherCollected = mVoucher;
                myCounter.VoucherRecorded = TotalVoucherColleted;
                myCounter.ChequeRecorded = TotalChequeCollected;
                myCounter.ChequeCollected = mChequeAmt;
                myCounter.PointRecorded = mPointAmt;
                myCounter.PackageRecorded = mPackageAmt;
                myCounter.SMFRecorded = TotalSMFCollected;
                myCounter.SMFCollected = mSMF;
                myCounter.PAMedRecorded = TotalPAMedCollected;
                myCounter.PAMedCollected = mPAMed;
                myCounter.PWFRecorded = TotalPWFCollected;
                myCounter.PWFCollected = mPWF;
                myCounter.NetsATMCardCollected = mNETSATMCardCollected;
                myCounter.NetsATMCardRecorded = TotalNETSATMCardCollected;
                myCounter.NetsCashCardCollected = mNETSCashCardCollected;
                myCounter.NetsCashCardRecorded = TotalNETSCashCardCollected;
                myCounter.NetsFlashPayCollected = mNETSFlashCardCollected;
                myCounter.NetsFlashPayRecorded = TotalNETSFlashPayCollected;

                if (enableNETSIntegration)
                {
                    myCounter.NetsRecorded = myCounter.NetsATMCardRecorded.GetValueOrDefault(0)
                                            + myCounter.NetsCashCardRecorded.GetValueOrDefault(0)
                                            + myCounter.NetsFlashPayRecorded.GetValueOrDefault(0);
                    myCounter.NetsCollected = myCounter.NetsATMCardCollected.GetValueOrDefault(0)
                                            + myCounter.NetsCashCardCollected.GetValueOrDefault(0)
                                            + myCounter.NetsFlashPayCollected.GetValueOrDefault(0);
                }
                if (gbForeignCurrency1.Visible)
                {
                    myCounter.ForeignCurrency1 = gbForeignCurrency1.Tag + "";
                    myCounter.ForeignCurrency1Recorded = TotalForeignCurrency1;
                    myCounter.ForeignCurrency1Collected = mForeignCurrency1;
                }
                if (gbForeignCurrency2.Visible)
                {
                    myCounter.ForeignCurrency2 = gbForeignCurrency2.Tag + "";
                    myCounter.ForeignCurrency2Recorded = TotalForeignCurrency2;
                    myCounter.ForeignCurrency2Collected = mForeignCurrency2;
                }
                if (gbForeignCurrency3.Visible)
                {
                    myCounter.ForeignCurrency3 = gbForeignCurrency3.Tag + "";
                    myCounter.ForeignCurrency3Recorded = TotalForeignCurrency3;
                    myCounter.ForeignCurrency3Collected = mForeignCurrency3;
                }
                if (gbForeignCurrency4.Visible)
                {
                    myCounter.ForeignCurrency4 = gbForeignCurrency4.Tag + "";
                    myCounter.ForeignCurrency4Recorded = TotalForeignCurrency4;
                    myCounter.ForeignCurrency4Collected = mForeignCurrency4;
                }
                if (gbForeignCurrency5.Visible)
                {
                    myCounter.ForeignCurrency5 = gbForeignCurrency5.Tag + "";
                    myCounter.ForeignCurrency5Recorded = TotalForeignCurrency5;
                    myCounter.ForeignCurrency5Collected = mForeignCurrency5;
                }
                decimal tmpDec;
                if (!decimal.TryParse(lblActual.Text, out tmpDec))
                {
                    MessageBox.Show("Error Converting Actual Amount to decimal upon closing. Actual > " + lblActual.Text);
                    Logger.writeLog("Error Converting Actual Amount to decimal upon closing. Actual > " + lblActual.Text);
                    return;
                }
                myCounter.TotalActualCollected = tmpDec;                                        
                /*
                if (myCounter.ChequeCollected.HasValue)
                {
                    myCounter.TotalActualCollected += myCounter.ChequeCollected.Value;
                }*/
                myCounter.TotalForeignCurrency = lblTotalForeignCurrency.Text.GetDecimalValue();
                myCounter.NetsTerminalID = "";// txtNetsTerminalID.Text;
                myCounter.VisaBatchNo = ""; // txtVisaBatchNo.Text;
                myCounter.AmexBatchNo = ""; // txtAmexBatchNo.Text;
                myCounter.ChinaNetsTerminalID = ""; //txtChinaNetsTerminalID.Text;
                myCounter.DepositBagNo = txtDepositBagNo.Text;
                myCounter.CashIn = cashIn;
                myCounter.CashOut = cashOut;
                myCounter.ClosingCashOut = mXTotalCollected;
                myCounter.StartTime = dtpStartDate.Value;
                myCounter.EndTime = dtpEndDate.Value;
                myCounter.OpeningBalance = openingBal;
                myCounter.Supervisor = supName;
                myCounter.TotalSystemRecorded = systemCollected;// -mPointAmt - mPackageAmt;
                myCounter.Variance = variance;
                myCounter.PointOfSaleID = PointOfSaleInfo.PointOfSaleID;
                myCounter.UniqueID = Guid.NewGuid();

                mycmd = myCounter.GetInsertCommand(UserInfo.username);
                cmd.Add(mycmd);
                #endregion

                #region Core save CashDenomination
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.UseTotalCashForCheckOut), false))
                {
                    if (lbl005.Visible)
                    {
                        CounterCloseDet ccd005 = new CounterCloseDet();
                        ccd005.CounterCloseID = CounterClosedLogId;
                        ccd005.CounterCloseDetID = CounterClosedLogId + "-0.05";
                        ccd005.PaymentType = "CASH";
                        ccd005.UnitValue = (decimal)0.05;
                        ccd005.UnitDisplayedName = "$0.05";
                        ccd005.TotalCount = txt005.Text.GetIntValue();
                        ccd005.TotalAmount = lbl005.Text.GetDecimalValue();
                        ccd005.UniqueID = Guid.NewGuid();
                        ccd005.Deleted = false;
                        cmd.Add(ccd005.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl010.Visible)
                    {
                        CounterCloseDet ccd010 = new CounterCloseDet();
                        ccd010.CounterCloseID = CounterClosedLogId;
                        ccd010.CounterCloseDetID = CounterClosedLogId + "-0.10";
                        ccd010.PaymentType = "CASH";
                        ccd010.UnitValue = (decimal)0.10;
                        ccd010.UnitDisplayedName = "$0.10";
                        ccd010.TotalCount = txt010.Text.GetIntValue();
                        ccd010.TotalAmount = lbl010.Text.GetDecimalValue();
                        ccd010.UniqueID = Guid.NewGuid();
                        ccd010.Deleted = false;
                        cmd.Add(ccd010.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl020.Visible)
                    {
                        CounterCloseDet ccd020 = new CounterCloseDet();
                        ccd020.CounterCloseID = CounterClosedLogId;
                        ccd020.CounterCloseDetID = CounterClosedLogId + "-0.20";
                        ccd020.PaymentType = "CASH";
                        ccd020.UnitValue = (decimal)0.20;
                        ccd020.UnitDisplayedName = "$0.20";
                        ccd020.TotalCount = txt020.Text.GetIntValue();
                        ccd020.TotalAmount = lbl020.Text.GetDecimalValue();
                        ccd020.UniqueID = Guid.NewGuid();
                        ccd020.Deleted = false;
                        cmd.Add(ccd020.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl050.Visible)
                    {
                        CounterCloseDet ccd050 = new CounterCloseDet();
                        ccd050.CounterCloseID = CounterClosedLogId;
                        ccd050.CounterCloseDetID = CounterClosedLogId + "-0.50";
                        ccd050.PaymentType = "CASH";
                        ccd050.UnitValue = (decimal)0.50;
                        ccd050.UnitDisplayedName = "$0.50";
                        ccd050.TotalCount = txt050.Text.GetIntValue();
                        ccd050.TotalAmount = lbl050.Text.GetDecimalValue();
                        ccd050.UniqueID = Guid.NewGuid();
                        ccd050.Deleted = false;
                        cmd.Add(ccd050.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl1.Visible)
                    {
                        CounterCloseDet ccd1 = new CounterCloseDet();
                        ccd1.CounterCloseID = CounterClosedLogId;
                        ccd1.CounterCloseDetID = CounterClosedLogId + "-1.00";
                        ccd1.PaymentType = "CASH";
                        ccd1.UnitValue = (decimal)1.00;
                        ccd1.UnitDisplayedName = "$1.00";
                        ccd1.TotalCount = txt1.Text.GetIntValue();
                        ccd1.TotalAmount = lbl1.Text.GetDecimalValue();
                        ccd1.UniqueID = Guid.NewGuid();
                        ccd1.Deleted = false;
                        cmd.Add(ccd1.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl2.Visible)
                    {
                        CounterCloseDet ccd2 = new CounterCloseDet();
                        ccd2.CounterCloseID = CounterClosedLogId;
                        ccd2.CounterCloseDetID = CounterClosedLogId + "-2.00";
                        ccd2.PaymentType = "CASH";
                        ccd2.UnitValue = (decimal)1.00;
                        ccd2.UnitDisplayedName = "$2.00";
                        ccd2.TotalCount = txt2.Text.GetIntValue();
                        ccd2.TotalAmount = lbl2.Text.GetDecimalValue();
                        ccd2.UniqueID = Guid.NewGuid();
                        ccd2.Deleted = false;
                        cmd.Add(ccd2.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl5.Visible)
                    {
                        CounterCloseDet ccd5 = new CounterCloseDet();
                        ccd5.CounterCloseID = CounterClosedLogId;
                        ccd5.CounterCloseDetID = CounterClosedLogId + "-5.00";
                        ccd5.PaymentType = "CASH";
                        ccd5.UnitValue = (decimal)5.00;
                        ccd5.UnitDisplayedName = "$5.00";
                        ccd5.TotalCount = txt5.Text.GetIntValue();
                        ccd5.TotalAmount = lbl5.Text.GetDecimalValue();
                        ccd5.UniqueID = Guid.NewGuid();
                        ccd5.Deleted = false;
                        cmd.Add(ccd5.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl10.Visible)
                    {
                        CounterCloseDet ccd10 = new CounterCloseDet();
                        ccd10.CounterCloseID = CounterClosedLogId;
                        ccd10.CounterCloseDetID = CounterClosedLogId + "-10.00";
                        ccd10.PaymentType = "CASH";
                        ccd10.UnitValue = (decimal)10.00;
                        ccd10.UnitDisplayedName = "$10.00";
                        ccd10.TotalCount = txt10.Text.GetIntValue();
                        ccd10.TotalAmount = lbl10.Text.GetDecimalValue();
                        ccd10.UniqueID = Guid.NewGuid();
                        ccd10.Deleted = false;
                        cmd.Add(ccd10.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl50.Visible)
                    {
                        CounterCloseDet ccd50 = new CounterCloseDet();
                        ccd50.CounterCloseID = CounterClosedLogId;
                        ccd50.CounterCloseDetID = CounterClosedLogId + "-50.00";
                        ccd50.PaymentType = "CASH";
                        ccd50.UnitValue = (decimal)50.00;
                        ccd50.UnitDisplayedName = "$50.00";
                        ccd50.TotalCount = txt50.Text.GetIntValue();
                        ccd50.TotalAmount = lbl50.Text.GetDecimalValue();
                        ccd50.UniqueID = Guid.NewGuid();
                        ccd50.Deleted = false;
                        cmd.Add(ccd50.GetInsertCommand(UserInfo.username));
                    }

                    if (lbl100.Visible)
                    {
                        CounterCloseDet ccd100 = new CounterCloseDet();
                        ccd100.CounterCloseID = CounterClosedLogId;
                        ccd100.CounterCloseDetID = CounterClosedLogId + "-100.00";
                        ccd100.PaymentType = "CASH";
                        ccd100.UnitValue = (decimal)100.00;
                        ccd100.UnitDisplayedName = "$100.00";
                        ccd100.TotalCount = txt100.Text.GetIntValue();
                        ccd100.TotalAmount = lbl100.Text.GetDecimalValue();
                        ccd100.UniqueID = Guid.NewGuid();
                        ccd100.Deleted = false;
                        cmd.Add(ccd100.GetInsertCommand(UserInfo.username));
                    }
                }
                #endregion

                #region *) Backup DB
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.EnableAutoBackup), true))
                {
                    try
                    {
                        string path = string.Empty;
                        try
                        {
                            path = ConfigurationManager.AppSettings["BackUpDirectory"];
                        }
                        catch
                        {
                            path = Environment.CurrentDirectory + "\\Backup\\";
                        }

                        DbUtilityController.DoDBBackUp(path, "Check Out");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to BackUp Database, Please Contact EdgeWorks Support Team. \n" + ex.ToString());
                    }
                }
            
                #endregion

                #region *) Core: Commit Transaction
                SubSonic.DataService.ExecuteTransaction(cmd);
                #endregion

                #region *) Notification: Success
                MessageBox.Show("Counter Closed Successfully.");
                IsSuccessful = true;
                #endregion

                PointOfSaleInfo.NETsTerminalID = ""; // txtNetsTerminalID.Text;
                //PointOfSaleController.SaveNETSTerminalID(txtNetsTerminalID.Text);
                ClosingController.DeleteSavedClosing();
                ClosingController.DeleteSaved(PointOfSaleInfo.PointOfSaleID);
            
                #region *) Core: Print Counter Close Report
                bool PrintProductSalesReport = false;
                PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintCounterCloseReport), true))
                {
                    try
                    {
                        bool printDiscount = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintDiscountOnCounterCloseReport), false));
                        PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                        POSDeviceController.PrintCloseCounterReport(new CounterCloseLog(CounterCloseLog.Columns.CounterCloseID, CounterClosedLogId), PrintProductSalesReport, printDiscount);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to print settlement. Please try again at Closing Report section");
                        Logger.writeLog(ex);
                    }
                }
                #endregion

                #region *) Core: Run External Script (for Landlord Integration)
                if (AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterCounterClose") != null
                    && AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterCounterClose") != "")
                {
                    try
                    {
                        System.Diagnostics.Process.Start(AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterCounterClose").ToString());
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Unable start remote process: " + AppSetting.GetSettingFromDBAndConfigFile("ExternalScriptAfterCounterClose").ToString() + " " + ex.Message);
                    }
                }
                #endregion

                #region *) Core: Send Emails (Closing Information)
                try
                {
                    ArrayList emails = new ArrayList();
                    EmailNotificationCollection em = new EmailNotificationCollection();
                    em.Where(EmailNotification.Columns.Deleted, false);
                    em.Load();
                    if (em.Count > 0)
                    {
                        for (int i = 0; i < em.Count; i++)
                            emails.Add(em[i].EmailAddress);

                        PowerPOS.MassEmail s = new PowerPOS.MassEmail();
                        string status = "", htmlBody = "";
                        ClosingController cl = new ClosingController();
                        bool isInclusiveGST = false;
                        if (AppSetting.GetSettingFromDBAndConfigFile("PriceInclusiveGST") != null &&
                            AppSetting.GetSettingFromDBAndConfigFile("PriceInclusiveGST").ToString() == "yes")
                        {
                            isInclusiveGST = true;
                        }

                        htmlBody = cl.FormatEmailOutput(myCounter, isInclusiveGST, PrintProductSalesReport);
                        s.SendEmails(emails, "", AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                            "Settlement Summary " + myCounter.EndTime.ToString("dd MMM yyyy HH:mm") +
                            " " + myCounter.PointOfSale.PointOfSaleName,
                            htmlBody, htmlBody,
                             AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SMTP),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_SenderEmail),
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Password), false, "", 
                            AppSetting.GetSetting(AppSetting.SettingsName.EmailSender.EmailSender_Port), out status);
                    }
                }
                catch (Exception X)
                {
                    CommonUILib.HandleException(X);
                }
                #endregion

                #region *) Core: Send Emails (Membership)
                if (AppSetting.GetSetting("EmailMembershipNotification") != null
                    && AppSetting.GetSetting("EmailMembershipNotification") != "yes")
                {
                    try
                    {
                        ArrayList emails = new ArrayList();
                        EmailNotificationCollection em = new EmailNotificationCollection();
                        em.Where(EmailNotification.Columns.Deleted, false);
                        em.Load();
                        if (em.Count > 0)
                        {
                            for (int i = 0; i < em.Count; i++)
                            {
                                emails.Add(em[i].EmailAddress);
                            }
                            DateTime startDate, endDate;
                            string rootDir = Application.StartupPath;
                            startDate = dtpStartDate.Value;
                            endDate = DateTime.Now;
                            MembershipController.SendMembershipNotification(emails, startDate, endDate, rootDir);
                        }
                    }
                    catch (Exception X)
                    {
                        CommonUILib.HandleException(X);
                    }
                }
                #endregion

                #region *) Core: Synchronization...

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.SyncAfterCheckOut), true))
                {
                    ShowPanel();
                    bool result = false; int count = 3;

                    if (AppSetting.GetSetting("IsRealTimeSales") == "1")
                    {
                        result = true;

                        bool transactionresult = true;
                        if (!SyncClientController.SendNewSignUpWithoutOrder())
                        {
                            transactionresult = false;
                        }

                        if (!OrderSync.StartOrderSync())
                        {
                            transactionresult = false;
                        }
                        if (!LogsSync.StartLogsSync())
                        {
                            transactionresult = false;
                        }
                        SyncClientController.deductInventory();
                        if (!InventorySync.GetCurrentInventory())
                        {
                            transactionresult = false;
                        }
                        if (!SyncClientController.UpdateOrderDetFromDownloadedInventoryHdr())
                        {
                            transactionresult = false;
                        }
                        if (!InventorySync.DeleteInventoryDetFromVoidedOrder())
                        {
                            transactionresult = false;
                        }
                        if (!OrderSync.UpdateOrderHdrUserFld1236ByPosIDViewBillLimitAndIsVoided())
                        {
                            transactionresult = false;
                        }

                        // Send & get Delivery Order data if using "Cash & Carry / Pre-Order" feature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
                        {
                            if (!SyncClientController.SendDeliveryOrderToServer(dtpStartDate.Value.AddDays(-3), DateTime.Now, true))
                            {
                                transactionresult = false;
                            }

                            if (!SyncClientController.GetDeliveryOrderFromServer(dtpStartDate.Value.AddDays(-3), DateTime.Now))
                            {
                                transactionresult = false;
                            }
                        }

                        if (!AppointmentSync.SyncAppointment(myCounter.StartTime, myCounter.EndTime))
                        {
                            //transactionresult = false;
                        }

                        result &= transactionresult;
                    }
                    else
                    {
                        while (count > 0 && !result)
                        {
                            //Change date time
                            result = SendData(dtpStartDate.Value.AddDays(-3), DateTime.Now);
                            --count;
                        }
                        AppointmentSync.SyncAppointment(dtpStartDate.Value.AddDays(-3), DateTime.Now);
                    }
                    HidePanel();

                    if (!result)
                    {
                        MessageBox.Show("WARNING: Sending Data FAILED! Please try to resend or contact your system administrator.");
                    }
                }
                else
                {
                    if (!syncLogsThread.IsBusy)
                        syncLogsThread.RunWorkerAsync();
                }
                #endregion

                #region *) Core: Update Membership Upgrade if it is not Client-Server - update 2013/02/06 : just update, despite localhost / client-server
                //SettingCollection Stg = new SettingCollection();
                //Stg.Load();
                //if (Stg[0].IntegrateWithInventory.GetValueOrDefault(false))
                    MembershipController.updateExpiryDateFromRenewalRequest();
                #endregion
                    
                isClosed = true;
                this.Close();
                
                AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", "Counter Closing", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //log error
                Logger.writeLog(ex);
            }
        }
        #endregion

        #region "Close the form"
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClosingController.SavePartiallyEnteredClosing
                (txtFloatBalance.Text, txtPay1.Text, txtPay5.Text, txtPay2.Text, txtPay6.Text, txtPay4.Text,
                 "", txtPay3.Text, "", txtVoucher.Text, txtChequeAmt.Text,
                 txtDepositBagNo.Text, PointOfSaleInfo.PointOfSaleID.ToString(), txt100.Text, txt50.Text, txt10.Text, txt5.Text,
                 txt2.Text, txt1.Text, txt050.Text, txt020.Text, txt010.Text, txt005.Text);
            this.Close();
        }
        #endregion

        private void txt100_Validating(object sender, CancelEventArgs e)
        {
            int v100, v50, v10, v5, v2, v1, v050, v020, v010, v005 ;
            decimal totalCash = 0;
            ep1.Clear();
            #region *) Calculate: 100 dolars
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt100, out v100))
            {
                ep1.SetError(txt100, "Please specify valid number");
                txt100.Select();
                return;
            }
            lbl100.Text = (v100 * 100).ToString("N");
            #endregion
            #region *) Calculate:  50 dolars
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt50, out v50))
            {
                ep1.SetError(txt50, "Please specify valid number");                                
                txt50.Focus();
                return;
            }
            lbl50.Text = (v50 * 50).ToString("N");
            #endregion
            #region *) Calculate:  10 dolars
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt10, out v10))
            {
                ep1.SetError(txt10, "Please specify valid number");                
                txt10.Select();
                return;
            }
            lbl10.Text = (v10 * 10).ToString("N");
            #endregion
            #region *) Calculate:   5 dolars
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt5, out v5))
            {
                ep1.SetError(txt5, "Please specify valid number");
                txt5.Select();                
                return;
            }
            lbl5.Text = (v5 * 5).ToString("N");
            #endregion
            #region *) Calculate:   2 dolars
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt2, out v2))
            {
                ep1.SetError(txt2, "Please specify valid number");             
                txt2.Select();
                return;
            }
            lbl2.Text = (v2 * 2).ToString("N");
            #endregion
            #region *) Calculate:   1 dolars
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt1, out v1))
            {
                ep1.SetError(txt1, "Please specify valid number");                
                txt1.Select();
                return;
            }
            lbl1.Text = (v1 * 1).ToString("N");
            #endregion
            #region *) Calculate: 50 cents
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt050, out v050))
            {
                ep1.SetError(txt050, "Please specify valid number");                
                txt050.Select();
                return;
            }
            lbl050.Text = (v050 * 0.5).ToString("N");
            #endregion
            #region *) Calculate: 20 cents
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt020, out v020))
            {
                ep1.SetError(txt020, "Please specify valid number");                
                txt020.Select();
                return;
            }
            lbl020.Text = (v020 * 0.20).ToString("N");
            #endregion
            #region *) Calculate: 10 cents
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt010, out v010))
            {
                ep1.SetError(txt010, "Please specify valid number");                
                txt010.Select();
                return;
            }
            lbl010.Text = (v010 * 0.10).ToString("N");
            #endregion
            #region *) Calculate:  5 cents
            if (!CommonUILib.ValidateTextBoxAsUnsignedNumericAllowNegative(txt005, out v005))
            {
                ep1.SetError(txt005, "Please specify valid number");                
                txt005.Select();
                return;
            }
            lbl005.Text = (v005 * 0.05).ToString("N");
            #endregion
            #region *) Calculate: Total cash
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(TxtTotalCash, out totalCash))
            {
                ep1.SetError(TxtTotalCash, "Please specify valid number");
                TxtTotalCash.Select();
                return;
            }
            #endregion

            decimal result; // To hold temporary data
            #region *) With NETS Integration

            if (enableNETSIntegration)
            {
                #region *) Calculate: Nets CashCard difference
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtNETSCashCard, out result))
                {
                    ep1.SetError(txtNETSCashCard, "Please specify valid amount");
                    txtNETSCashCard.Select();
                    return;
                }
                decimal NetsCashCardDefi = (result - decimal.Parse(lblNETSCashCardAmt.Text));
                lblNETSCashCardDefi.Text = NetsCashCardDefi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (NetsCashCardDefi < 0)
                    {
                        gbNETSCashCard.BackColor = System.Drawing.Color.Salmon;
                    }
                    else if (NetsCashCardDefi > 0)
                    {
                        gbNETSCashCard.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        gbNETSCashCard.BackColor = System.Drawing.Color.Transparent;
                    }
                }
                #endregion
                #region *) Calculate: Nets FlashPay difference
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtNETSFlashPay, out result))
                {
                    ep1.SetError(txtNETSFlashPay, "Please specify valid amount");
                    txtNETSFlashPay.Select();
                    return;
                }
                decimal NetsFlashPayDefi = (result - decimal.Parse(lblNETSFlashPayAmt.Text));
                lblNETSFlashPayDefi.Text = NetsFlashPayDefi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (NetsFlashPayDefi < 0)
                    {
                        gbNETSFlashPay.BackColor = System.Drawing.Color.Salmon;
                    }
                    else if (NetsFlashPayDefi > 0)
                    {
                        gbNETSFlashPay.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        gbNETSFlashPay.BackColor = System.Drawing.Color.Transparent;
                    }
                }
                #endregion
                #region *) Calculate: Nets ATMCard difference
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtNETSATMCard, out result))
                {
                    ep1.SetError(txtNETSATMCard, "Please specify valid amount");
                    txtNETSATMCard.Select();
                    return;
                }
                decimal NetsATMCardDefi = (result - decimal.Parse(lblNETSATMCardAmt.Text));
                lblNETSATMCardDefi.Text = NetsATMCardDefi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (NetsATMCardDefi < 0)
                    {
                        gbNETSATMCard.BackColor = System.Drawing.Color.Salmon;
                    }
                    else if (NetsATMCardDefi > 0)
                    {
                        gbNETSATMCard.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        gbNETSATMCard.BackColor = System.Drawing.Color.Transparent;
                    }
                }
                #endregion
            }

            #endregion
            #region *) Without NETS Integration

            else
            {
                #region *) Calculate: Nets difference
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay1, out result))
                {
                    ep1.SetError(txtPay1, "Please specify valid amount");
                    txtPay1.Select();
                    return;
                }
                decimal NetsDefi = (result - decimal.Parse(lblPay1.Text));
                lblDefi1.Text = NetsDefi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (NetsDefi < 0)
                    {
                        gbPay1.BackColor = System.Drawing.Color.Salmon;
                    }
                    else if (NetsDefi > 0)
                    {
                        gbPay1.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        gbPay1.BackColor = System.Drawing.Color.Transparent;
                    }
                }
                #endregion
            }

            #endregion

            #region *) Calculate: Visa difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay2, out result))
            {
                ep1.SetError(txtPay2, "Please specify valid amount");
                txtPay2.Select();                
                return;
            }
            decimal VISADefi = (result - decimal.Parse(lblPay2.Text));
            lblDefi2.Text = VISADefi.ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (VISADefi < 0)
                {
                    gbPay2.BackColor = System.Drawing.Color.Salmon;
                }
                else if (VISADefi > 0)
                {
                    gbPay2.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay2.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion
            
            #region *) Calculate: Voucher difference

            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtVoucher, out result))
            {
                ep1.SetError(txtVoucher, "Please specify valid amount");
                txtVoucher.Select();                
                return;
            }
            decimal VoucherDefi = (result - decimal.Parse(lblVoucherCollected.Text));
            lblVoucherDefi.Text = VoucherDefi.ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (VoucherDefi < 0)
                {
                    gbVoucher.BackColor = System.Drawing.Color.Salmon;
                }
                else if (VoucherDefi > 0)
                {
                    gbVoucher.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbVoucher.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion
            
            #region *) Calculate: China Nets difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay3, out result))
            {
                ep1.SetError(txtPay3, "Please specify valid amount");
                txtPay3.Select();                
                return;
            }
            decimal ChinaDefi = (result - decimal.Parse(lblPay3.Text));
            lblDefi3.Text = ChinaDefi.ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (ChinaDefi < 0)
                {
                    gbPay3.BackColor = System.Drawing.Color.Salmon;
                }
                else if (ChinaDefi > 0)
                {
                    gbPay3.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay3.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion
            
            #region *) Calculate: Amex difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay4, out result))
            {
                ep1.SetError(txtPay4, "Please specify valid amount");
                txtPay4.Select();                
                return;
            }
            decimal AMEXDefi = (result - decimal.Parse(lblPay4.Text));
            lblDefi4.Text = (AMEXDefi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (AMEXDefi < 0)
                {
                    gbPay4.BackColor = System.Drawing.Color.Salmon;
                }
                else if (AMEXDefi > 0)
                {
                    gbPay4.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay4.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Pay5 difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay5, out result))
            {
                ep1.SetError(txtPay5, "Please specify valid amount");                
                txtPay5.Select();
                return;
            }
            decimal Pay5Defi = (result - decimal.Parse(lblPay5.Text));
            lblDefi5.Text = (Pay5Defi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (Pay5Defi < 0)
                {
                    gbPay5.BackColor = System.Drawing.Color.Salmon;
                }
                else if (Pay5Defi > 0)
                {
                    gbPay5.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay5.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Pay6 difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay6, out result))
            {
                ep1.SetError(txtPay6, "Please specify valid amount");                
                txtPay6.Select();
                return;
            }
            decimal Pay6Defi = (result - decimal.Parse(lblPay6.Text));
            lblDefi6.Text = (Pay6Defi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (Pay6Defi < 0)
                {
                    gbPay6.BackColor = System.Drawing.Color.Salmon;
                }
                else if (Pay6Defi > 0)
                {
                    gbPay6.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay6.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Pay7 difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay7, out result))
            {
                ep1.SetError(txtPay7, "Please specify valid amount");
                txtPay7.Select();
                return;
            }
            decimal Pay7Defi = (result - decimal.Parse(lblPay7.Text));
            lblDefi7.Text = (Pay7Defi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (Pay7Defi < 0)
                {
                    gbPay7.BackColor = System.Drawing.Color.Salmon;
                }
                else if (Pay7Defi > 0)
                {
                    gbPay7.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay7.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Pay8 difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay8, out result))
            {
                ep1.SetError(txtPay8, "Please specify valid amount");
                txtPay8.Select();
                return;
            }
            decimal Pay8Defi = (result - decimal.Parse(lblPay8.Text));
            lblDefi8.Text = (Pay8Defi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (Pay8Defi < 0)
                {
                    gbPay8.BackColor = System.Drawing.Color.Salmon;
                }
                else if (Pay8Defi > 0)
                {
                    gbPay8.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay8.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Pay9 difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay9, out result))
            {
                ep1.SetError(txtPay9, "Please specify valid amount");
                txtPay9.Select();
                return;
            }
            decimal Pay9Defi = (result - decimal.Parse(lblPay9.Text));
            lblDefi9.Text = (Pay9Defi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (Pay9Defi < 0)
                {
                    gbPay9.BackColor = System.Drawing.Color.Salmon;
                }
                else if (Pay9Defi > 0)
                {
                    gbPay9.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay9.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Pay10 difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPay10, out result))
            {
                ep1.SetError(txtPay10, "Please specify valid amount");
                txtPay10.Select();
                return;
            }
            decimal Pay10Defi = (result - decimal.Parse(lblPay10.Text));
            lblDefi10.Text = (Pay10Defi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (Pay10Defi < 0)
                {
                    gbPay10.BackColor = System.Drawing.Color.Salmon;
                }
                else if (Pay10Defi > 0)
                {
                    gbPay10.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPay10.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: Cheque difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtChequeAmt, out result))
            {
                ep1.SetError(txtChequeAmt, "Please specify valid amount");
                txtChequeAmt.Select();                
                return;
            }
            decimal ChequeDefi = (result - decimal.Parse(lblChequeAmt.Text));
            lblChequeDefi.Text = (ChequeDefi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (ChequeDefi < 0)
                {
                    gbCheque.BackColor = System.Drawing.Color.Salmon;
                }
                else if (ChequeDefi > 0)
                {
                    gbCheque.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbCheque.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: SMF difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtSMF, out result))
            {
                ep1.SetError(txtSMF, "Please specify valid amount");
                txtSMF.Select();
                return;
            }
            decimal SMFDefi = (result - decimal.Parse(lblSMFCollected.Text));
            lblSMFDefi.Text = SMFDefi.ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (SMFDefi < 0)
                {
                    gbSMF.BackColor = System.Drawing.Color.Salmon;
                }
                else if (SMFDefi > 0)
                {
                    gbSMF.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbSMF.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: PAMedifund difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPAMed, out result))
            {
                ep1.SetError(txtPAMed, "Please specify valid amount");
                txtPAMed.Select();
                return;
            }
            decimal PAMedDefi = (result - decimal.Parse(lblPAMedCollected.Text));
            lblPAMedDefi.Text = PAMedDefi.ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (PAMedDefi < 0)
                {
                    gbPAMed.BackColor = System.Drawing.Color.Salmon;
                }
                else if (PAMedDefi > 0)
                {
                    gbPAMed.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPAMed.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Calculate: PWF difference
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtPWF, out result))
            {
                ep1.SetError(txtPWF, "Please specify valid amount");
                txtPWF.Select();
                return;
            }
            decimal PWFDefi = (result - decimal.Parse(lblPWFCollected.Text));
            lblPWFDefi.Text = PWFDefi.ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (PWFDefi < 0)
                {
                    gbPWF.BackColor = System.Drawing.Color.Salmon;
                }
                else if (PWFDefi > 0)
                {
                    gbPWF.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbPWF.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            #region *) Foreign Currency 1

            if (gbForeignCurrency1.Visible)
            {
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtForeignCurrency1, out result))
                {
                    ep1.SetError(txtForeignCurrency1, "Please specify valid amount");
                    txtForeignCurrency1.Select();
                    return;
                }
                decimal foreignCurrency1Defi = (result - lblForeignCurrency1PayAmt.Text.GetDecimalValue());
                lblForeignCurrency1PayDefi.Text = foreignCurrency1Defi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (foreignCurrency1Defi < 0)
                        gbForeignCurrency1.BackColor = System.Drawing.Color.Salmon;
                    else if (foreignCurrency1Defi > 0)
                        gbForeignCurrency1.BackColor = System.Drawing.Color.LightGreen;
                    else
                        gbForeignCurrency1.BackColor = System.Drawing.Color.Transparent;
                }
            }

            #endregion

            #region *) Foreign Currency 2

            if (gbForeignCurrency2.Visible)
            {
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtForeignCurrency2, out result))
                {
                    ep1.SetError(txtForeignCurrency2, "Please specify valid amount");
                    txtForeignCurrency2.Select();
                    return;
                }
                decimal foreignCurrency2Defi = (result - lblForeignCurrency2PayAmt.Text.GetDecimalValue());
                lblForeignCurrency2PayDefi.Text = foreignCurrency2Defi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (foreignCurrency2Defi < 0)
                        gbForeignCurrency2.BackColor = System.Drawing.Color.Salmon;
                    else if (foreignCurrency2Defi > 0)
                        gbForeignCurrency2.BackColor = System.Drawing.Color.LightGreen;
                    else
                        gbForeignCurrency2.BackColor = System.Drawing.Color.Transparent;
                }
            }

            #endregion

            #region *) Foreign Currency 3

            if (gbForeignCurrency3.Visible)
            {
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtForeignCurrency3, out result))
                {
                    ep1.SetError(txtForeignCurrency3, "Please specify valid amount");
                    txtForeignCurrency3.Select();
                    return;
                }
                decimal foreignCurrency3Defi = (result - lblForeignCurrency3PayAmt.Text.GetDecimalValue());
                lblForeignCurrency3PayDefi.Text = foreignCurrency3Defi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (foreignCurrency3Defi < 0)
                        gbForeignCurrency3.BackColor = System.Drawing.Color.Salmon;
                    else if (foreignCurrency3Defi > 0)
                        gbForeignCurrency3.BackColor = System.Drawing.Color.LightGreen;
                    else
                        gbForeignCurrency3.BackColor = System.Drawing.Color.Transparent;
                }
            }

            #endregion

            #region *) Foreign Currency 4

            if (gbForeignCurrency4.Visible)
            {
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtForeignCurrency4, out result))
                {
                    ep1.SetError(txtForeignCurrency4, "Please specify valid amount");
                    txtForeignCurrency4.Select();
                    return;
                }
                decimal foreignCurrency4Defi = (result - lblForeignCurrency4PayAmt.Text.GetDecimalValue());
                lblForeignCurrency4PayDefi.Text = foreignCurrency4Defi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (foreignCurrency4Defi < 0)
                        gbForeignCurrency4.BackColor = System.Drawing.Color.Salmon;
                    else if (foreignCurrency4Defi > 0)
                        gbForeignCurrency4.BackColor = System.Drawing.Color.LightGreen;
                    else
                        gbForeignCurrency4.BackColor = System.Drawing.Color.Transparent;
                }
            }

            #endregion

            #region *) Foreign Currency 5

            if (gbForeignCurrency5.Visible)
            {
                if (!CommonUILib.ValidateTextBoxAsUnsignedDecimalAllowNegative(txtForeignCurrency4, out result))
                {
                    ep1.SetError(txtForeignCurrency5, "Please specify valid amount");
                    txtForeignCurrency5.Select();
                    return;
                }
                decimal foreignCurrency5Defi = (result - lblForeignCurrency5PayAmt.Text.GetDecimalValue());
                lblForeignCurrency5PayDefi.Text = foreignCurrency5Defi.ToString("N");
                if (!hideRecordedSystemOnCheckout)
                {
                    if (foreignCurrency5Defi < 0)
                        gbForeignCurrency5.BackColor = System.Drawing.Color.Salmon;
                    else if (foreignCurrency5Defi > 0)
                        gbForeignCurrency5.BackColor = System.Drawing.Color.LightGreen;
                    else
                        gbForeignCurrency5.BackColor = System.Drawing.Color.Transparent;
                }
            }

            #endregion

            #region *) Validate: Float Balance is valid
            if (!CommonUILib.ValidateTextBoxAsUnsignedDecimal(txtFloatBalance, out result))
            {
                ep1.SetError(txtFloatBalance, "Please specify valid amount");
                e.Cancel = true;
                return;
            }
            #endregion

            /// Clear all previous error
            /// I think, it should be put on top
            ep1.Clear();

            #region *) Calculate: Total Cash in form of Notes
            decimal totalNotes =
                v100 * 100 +
                v50 * 50 +
                v10 * 10 +
                v5 * 5 +
                v2 * 2;

            lblTotalNotes.Text = totalNotes.ToString("N");
            #endregion

            #region *) Calculate: Total Cash in form of Coins
            decimal totalCoins = v1 * 1 +
                            v050 * 0.50M +
                            v020 * 0.20M +
                            v010 * 0.10M +
                            v005 * 0.05M;
            lblTotalCoins.Text = totalCoins.ToString("N");
            #endregion

            #region *) Calculate: Total Cash
            decimal total = 0;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Closing.UseTotalCashForCheckOut), false))
            {
                total = totalCash;
            }
            else
            {
                total = totalNotes + totalCoins;
            }
            txtTotalCollected.Text = total.ToString("N");
            #endregion

            #region *) Calculate: Cash difference
            decimal CashDefi =
                ((decimal)total - decimal.Parse(lblTotalCashExpected.Text));
            lblCashDefi.Text = (CashDefi).ToString("N");
            if (!hideRecordedSystemOnCheckout)
            {
                if (CashDefi < 0)
                {
                    gbCashBreakdown.BackColor = System.Drawing.Color.Salmon;
                    gbCash.BackColor = System.Drawing.Color.Salmon;
                    lblCashDefi.Text = lblCashDefi.Text;
                }
                else if (CashDefi > 0)
                {
                    gbCashBreakdown.BackColor = System.Drawing.Color.LightGreen;
                    gbCash.BackColor = System.Drawing.Color.LightGreen;
                }
                else
                {
                    gbCashBreakdown.BackColor = System.Drawing.Color.Transparent;
                    gbCash.BackColor = System.Drawing.Color.Transparent;
                }
            }
            #endregion

            displayCashRecordingData();

        }

        private void txtNETSAmt_Click(object sender, EventArgs e)
        {
            //prompt keypad
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;
            f.initialValue = ((TextBox)sender).Text;
            f.ShowDialog();

            ((TextBox)sender).Text = f.value.ToString();
            txt100_Validating(this, new CancelEventArgs());
        }

        private void txtVoucher_TextChanged(object sender, EventArgs e)
        {
            if (isLoaded)
                txt100_Validating(this, new CancelEventArgs());
        }

        private void ShowPanel()
        {
            pnlWait.Visible = true;
            pnlWait.BringToFront();
            pnlWait.Refresh();
        }

        private void HidePanel()
        {
            pnlWait.Visible = false;
            pnlWait.SendToBack();
        }

        private bool SendData(DateTime StartDate, DateTime EndDate)
        {
            //SyncClientController.Load_WS_URL();
            bool isLocalhost = SyncClientController.WS_URL.StartsWith("http://localhost") || SyncClientController.WS_URL.StartsWith("http://127.0.0.1");
  
            if (SyncClientController.WS_URL == "" || isLocalhost)
            {
                return true;
            }
            bool TotalResult = SyncClientController.SendLogsToServer(StartDate, EndDate.AddSeconds(5)); ;
            if (!SyncClientController.SendNewSignUpWithoutOrder())
            {
                TotalResult = false;
            }
            bool result = false;
            while (DateTime.Compare(StartDate, EndDate) <= 0)
            {
                result = SyncClientController.
                    SendOrderCCMW(StartDate, StartDate.AddHours(8));
                TotalResult &= result;

                // Send & get Delivery Order data if using "Cash & Carry / Pre-Order" feature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Order.PromptSelectOrderType), false))
                {
                    result = SyncClientController.SendDeliveryOrderToServer(StartDate, StartDate.AddHours(8), false);
                    TotalResult &= result;

                    result = SyncClientController.GetDeliveryOrderFromServer(StartDate, StartDate.AddHours(8));
                    TotalResult &= result;
                }

                if (AppSetting.GetSetting("SyncLocalInventoryToServer") == "yes")
                {
                    result = SyncClientController.SendInventory(StartDate, StartDate.AddHours(8));
                    TotalResult &= result;
                }
                Logger.writeLog("Sending Sales from " +
                StartDate.ToString("dd/MM/yy HH:mm:ss") +
                " to " + StartDate.AddHours(8).ToString("dd/MM/yy HH:mm:ss")
                + " POS ID " + PointOfSaleInfo.PointOfSaleID + " completed");
                StartDate = StartDate.AddHours(8);
            }
            SyncClientController.deductInventory();
      
            Logger.writeLog("Sending result:" + TotalResult);

            return TotalResult;
        }

        private void btnForeignCurrency_Click(object sender, EventArgs e)
        {
            //open foreign currency declaration form
            frmDeclareForeignCurrency f = new frmDeclareForeignCurrency();
            f.startDate = dtpStartDate.Value;
            f.endDate = dtpEndDate.Value;
            f.pointOfSaleID = PointOfSaleInfo.PointOfSaleID;
            f.ShowDialog();
            f.Dispose();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want to clear all the values? All your changes will be lost.", "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                txtPay1.Text = "0.0";
                txtPay2.Text = "0.0";
                txtPay3.Text = "0.0";
                txtPay4.Text = "0.0";
                txtPay5.Text = "0.0";
                txtPay6.Text = "0.0";
                txtChequeAmt.Text = "0.0";
                txtVoucher.Text = "0.0";
                txt100.Text = "0";
                txt50.Text = "0";
                txt10.Text = "0";
                txt5.Text = "0";
                txt2.Text = "0";
                txt1.Text = "0";
                txt005.Text = "0";
                txt010.Text = "0";
                txt020.Text = "0";
                txt050.Text = "0";
                txtSMF.Text = "0";
                txtPAMed.Text = "0";
                txtPWF.Text = "0";
                txtNETSFlashPay.Text = "0";
                txtNETSCashCard.Text = "0";
                txtNETSATMCard.Text = "0";
                txtForeignCurrency1.Text = "0";
                txtForeignCurrency2.Text = "0";
                txtForeignCurrency3.Text = "0";
                txtForeignCurrency4.Text = "0";
                txtForeignCurrency5.Text = "0";
            }
        }

        private void btnCloseShift_Click(object sender, EventArgs e)
        {
            if (ClosingController.ReadLastSaved(PointOfSaleInfo.PointOfSaleID) != new DateTime(2007, 1, 1))
            {
                MessageBox.Show("Please Reset Shift before you can set another change shift time");
                return;
            }
            DialogResult dr =
                MessageBox.Show("Are you sure you want to change shift?",
                "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                bool printProductSales = false;
                printProductSales = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");
                
                //POSDeviceController.PrintXReport(dtpStartDate.Value, dtpEndDate.Value, printProductSales);
                ClosingController.WriteLastSaved(PointOfSaleInfo.PointOfSaleID, DateTime.Now);
                IsSuccessful = true;
                this.Close();
            }
        }

        private void btnResetShift_Click(object sender, EventArgs e)
        {
            DialogResult dr =
                MessageBox.Show("Are you sure you want to reset the Change shift Info ?",
                "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                ClosingController.DeleteSaved(PointOfSaleInfo.PointOfSaleID);
                dtpEndDate.Value = DateTime.Now;
                this.Close();
            }
        }

        private void btnXReport_Click(object sender, EventArgs e)
        {
            bool printDiscount = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.PrintDiscountOnCounterCloseReport), false));
            bool PrintProductSalesReport = (AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport") != null && AppSetting.GetSettingFromDBAndConfigFile("PrintProductSalesReport").ToString().ToUpper() == "YES");

            POSDeviceController.PrintXReport(dtpStartDate.Value, dtpEndDate.Value, PrintProductSalesReport, printDiscount);
        }
    }
}