using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using POSDevices;
using WinPowerPOS.Reports;
using PowerPOS.Container;
using System.Configuration;
using Features = PowerPOS.Feature;
using PowerPOSLib.Controller;
using System.Runtime.InteropServices;
using System.Threading;
using SubSonic;

namespace WinPowerPOS.OrderForms
{
    public partial class frmSelectPayment : Form
    {
        public POSController pos;
        public bool isSuccessful;
        public decimal amount;
        string status;
        public bool PrintReceipt = true;
        //private bool enableNETSIntegration = false;
        public BackgroundWorker syncSalesThread;
        public BackgroundWorker ParentSyncPointsThread;
        //public CashRecycler cashRecycler;

        private bool UseCashRecyclerMachine;
        public bool IsCreditNote = false;

        bool enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
        //string netsPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSPayment) + "").Trim().ToUpper();
        bool enableUNIONPayIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableUNIONPayIntegration), false);
        string unionPayPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.UNIONPayPayment) + "").Trim().ToUpper();
        bool enableBCAIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableBCAIntegration), false);
        string bcaPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.BCAPayment) + "").Trim().ToUpper();
        bool enableCUPIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCUPIntegration), false);
        string cupPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.CUPPayment) + "").Trim().ToUpper();
        bool enablePrepaidPurchaseIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnablePrepaidPurchaseIntegration), false);
        string prepaidPurchasePayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PrepaidPurchasePayment) + "").Trim().ToUpper();
        bool enableCreditCardIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCreditCardIntegration), false);
        string CreditCardPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped) + "").Trim().ToUpper();

        

        string netsATMCardPayment = POSController.PAY_NETSATMCard.ToUpper();
        string netsFlashPayPayment = POSController.PAY_NETSFlashPay.ToUpper();
        string netsCashCardPayment = POSController.PAY_NETSCashCard.ToUpper();
        string netsQRPayment = POSController.PAY_NETSQR.ToUpper();

        bool enableCitiBankTerminal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCitiBankIntegration), false);
        string citiBankPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.CitiBankPayment) + "").Trim().ToUpper();
        string citiBank_VISA = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_VISA);
        string citiBank_MASTER = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_MASTER);
        string citiBank_AMEX = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_AMEX);
        string citiBank_DINERS = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_DINERS);
        string citiBank_JCB = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_JCB);

        bool enableNetsCreditCardTerminal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCreditCardIntegration), false);
        string netsCreditCardPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped) + "").Trim().ToUpper();
        string netsCC_VISA = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
        string netsCC_MASTER = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER);
        string netsCC_AMEX = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX);
        string netsCC_DINERS = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_DINERS);
        string netsCC_JCB = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_JCB);

        public frmSelectPayment()
        {
            InitializeComponent();
            //enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            LoadPaymentTypeLabels();
        }
        private const int TOTAL_BUTTONS = 10;
        private void LoadPaymentTypeLabels()
        {
            
            for (int Counter = 1; Counter <= TOTAL_BUTTONS; Counter++)
            {
                Control[] tmp = this.Controls.Find("btnPay" + Counter.ToString(), true);
                if (tmp.Length > 0)
                {
                    Button btnPay = (Button)tmp[0];
                    string paymentName = PaymentTypesController.FetchPaymentByID(btnPay.Tag.ToString());
                    if (paymentName == "" || paymentName == "-" || 
                        paymentName.ToUpper() == "INSTALLMENT" ||
                        paymentName.ToUpper() == "POINTS"
                        )
                    {
                        //Validation for NETS CREDIT CARD
                        
                        btnPay.Visible = false;
                        btnPay.Enabled = false;
                    }
                    else
                    {
                        btnPay.Text = paymentName;
                    }
                    if (enableNetsCreditCardTerminal &&
                            (paymentName == netsCC_VISA || paymentName == netsCC_MASTER || paymentName == netsCC_AMEX
                            || paymentName == netsCC_DINERS || paymentName == netsCC_JCB))
                    {
                        btnPay.Visible = false;
                        btnPay.Enabled = false;
                    }

                    if (enableCitiBankTerminal &&
                            (paymentName == citiBank_VISA || paymentName == citiBank_MASTER || paymentName == citiBank_AMEX
                            || paymentName == citiBank_DINERS || paymentName == citiBank_JCB))
                    {
                        btnPay.Visible = false;
                        btnPay.Enabled = false;
                    }
                    
                }
            }

            //create button for payment 
            btnCreditCard.Visible = false;
            if (enableNetsCreditCardTerminal)
            {
                //Button btnCreditCard = new Button();
                btnCreditCard.Visible = true;
                btnCreditCard.Text = netsCreditCardPayment;
            }

            if (enableCitiBankTerminal)
            {
                //Button btnCreditCard = new Button();
                btnCreditCard.Visible = true;
                btnCreditCard.Text = citiBankPayment;
            }

            //
            //btnPoints.Enabled = Features.Package.isAvailable && Features.Package.isRealTime;
            btnPoints.Enabled = Features.Package.isAvailable;
            btnCashCard.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            btnCashCard.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            btnFlashPay.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            btnFlashPay.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            btnNETSATMCard.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            btnNETSATMCard.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            btnNETSQR.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSQR), false);
            btnNETSQR.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSQR), false);
        }
        //PAY CASH
        public string change;
        public string tenderAmt;

        private void tryDownloadPoints()
        {
            if (PrintSettingInfo.receiptSetting.PrintReceipt)
            {
                bool overwriteSetting = !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Print.HidePrintPackageBalanceOnReceipt), true);
                if ((Features.Package.isAvailable && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.WaitToDownloadPointsBeforePrintReceipt), false)) || overwriteSetting)
                {
                    //Download 
                    //this.Enabled = false;
                    string isRealTime = ConfigurationManager.AppSettings["RealTimePointSystem"];
                    if (isRealTime == "yes" || isRealTime == "true")
                    {
                        if (pos.GetMemberInfo().MembershipNo != "WALK-IN")
                        {
                            frmDownloadPoints fDownloadPoints = new frmDownloadPoints();
                            fDownloadPoints.membershipNo = pos.GetMemberInfo().MembershipNo;
                            fDownloadPoints.orderHdrID = pos.myOrderHdr.OrderHdrID;
                            fDownloadPoints.ParentSyncPointsThread = ParentSyncPointsThread;
                            fDownloadPoints.ShowDialog();
                            if (!fDownloadPoints.IsSuccessful)
                                MessageBox.Show("Latest Point Data is not downloaded yet. Showing the latest point data in the receipt.");

                        }
                    }

                }
            }
            
        }
        private void btnCashPayment_Click(object sender, EventArgs e)
        {
            //CASH....
            if (UseCashRecyclerMachine)
            {
                try
                {
                    frmCashPaymentLoading fl = new frmCashPaymentLoading("Please Insert Cash");
                    fl.Amount = pos.RoundTotalReceiptAmount();
                    //fl.cashRecycler = cashRecycler;
                    fl.pos = pos;
                    fl.ShowDialog();
                    isSuccessful = fl.isSuccessful;
                    if (isSuccessful)
                    {
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                            if (!syncSalesThread.IsBusy)
                                syncSalesThread.RunWorkerAsync();

                        tryDownloadPoints();

                        if (PrintReceipt)
                        {
                            POSDeviceController.PrintAHAVATransactionReceipt(pos, fl.changeAmount, false,
                                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        }
                        change = fl.changeAmount.ToString("N2");
                        tenderAmt = fl.AmountReceived.ToString("N2");
                        fl.Dispose();
                        this.Close();
                    }
                    
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                try
                {
                    /*
                    CashDrawer drw = new CashDrawer();
                    drw.OpenDrawer();
                    */
                    //Get payment mode...                
                    frmPaymentTaking pyment = new frmPaymentTaking();
                    pyment.pos = pos;

                    pyment.lblAmount.Text = pos.RoundTotalReceiptAmount().ToString("N");

                    pyment.ShowDialog();

                    isSuccessful = pyment.isSuccessful;
                    if (isSuccessful)
                    {

                        //ask for signature here
                        #region Signature
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                        {
                            //pop out whether want signature or not
                            frmCustomMessageBox myfrm = new frmCustomMessageBox
                                ("Add Signature", "Do you want to add signature to this transaction?");
                            DialogResult DR = myfrm.ShowDialog();

                            if (myfrm.choice == "yes")
                            {
                                myfrm.Dispose();

                                bool isUsingStandard = false;
                                string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                                if (string.IsNullOrEmpty(signatureDevice))
                                    signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                                isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                                if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                                {
                                    wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                                    if (usbDevices.Count == 0)
                                    {
                                        isUsingStandard = true;
                                        MessageBox.Show("There is no STU device attached, using standard signature form");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                            frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                            demo.ShowDialog();
                                            List<wgssSTU.IPenData> penData = demo.getPenData();
                                            if (penData != null)
                                            {
                                                // process penData here!

                                                wgssSTU.IInformation information = demo.getInformation();
                                                wgssSTU.ICapability capability = demo.getCapability();
                                            }
                                            demo.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("An error occurred, please contact admin");
                                            Logger.writeLog(ex);
                                        }
                                    }
                                }

                                if (isUsingStandard)
                                {
                                    //asking for Signature
                                    frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                                    f.ShowDialog();
                                    if (f.IsSuccessful)
                                    {
                                        f.Dispose();
                                    }
                                }
                            }
                            else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                                myfrm.Dispose();
                            /*
                        else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                        {
                            myfrm.Dispose();
                            return;
                        }
                             */
                        }
                        #endregion

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                            if (!syncSalesThread.IsBusy)
                                syncSalesThread.RunWorkerAsync();

                        tryDownloadPoints();

                        if (PrintReceipt)
                        {
                            POSDeviceController.PrintAHAVATransactionReceipt(pos, pyment.change, false,
                                (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                        }
                        change = pyment.lblChange.Text;
                        tenderAmt = pyment.tenderAmt.ToString("N2");
                        pyment.Dispose();
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                    MessageBox.Show("An unknown error has been encountered. Please contact your system administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //Multiple Payment Mode
        private void btnOther_Click(object sender, EventArgs e)
        {
            frmPartialPayment frm = new frmPartialPayment();
            frm.pos = pos;
            frm.IsEdit = false;
            

            frm.lblAmount.Text = pos.CalculateAmountForPayment("MIX").ToString();
            //frm.CustomerIsAMember = !string.IsNullOrEmpty(pos.CurrentMember.MembershipNo);
            frm.CustomerIsAMember = (pos.MembershipApplied() && pos.CurrentMember.MembershipNo.ToLower() != "walk-in");
            frm.ParentSyncPointsThread = ParentSyncPointsThread;
            frm.ShowDialog();

            if (frm.IsSuccessful)
            {
                //ask for signature here
                #region Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                    if (!syncSalesThread.IsBusy)
                        syncSalesThread.RunWorkerAsync();

                tryDownloadPoints();

                change = frm.lblChange.Text;
                if (PrintReceipt)
                {
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, frm.change, false,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                }
                isSuccessful = true;
                frm.Dispose();
                this.Close();
            }
            else //clear payment lines if the form cancelled
            {
                pos.DeleteAllReceiptLinePayment();

                if (pos.IsFundingSelected())
                {
                    pos.FundingMethod = "";
                    pos.ClearFundingAmount();
                    //this.Close();
                }
            }
        }

        private bool CallPaymentIntegration(string paymentName, decimal amount, bool needToChangePayment, out string newPaymentName, out string responseInfo)
        {
            bool isSuccess = false;
            newPaymentName = paymentName;
            responseInfo = "";

            //frmPaymentIntegration frm = new frmPaymentIntegration(paymentName, amount, pos.myOrderHdr.OrderHdrID.Substring(6, 8), needToChangePayment);

            // 8 feb 2018 : changed to pass the full OrderHdrID instead of only last 8 digits
            frmPaymentIntegration frm = new frmPaymentIntegration(paymentName, amount, pos.myOrderHdr.OrderHdrID, needToChangePayment);
            frm.pos = pos;
            frm.cashbackAmount = pos.GetCashBackAmount();
            frm.ShowDialog();

            isSuccess = frm.DialogResult != DialogResult.Cancel;
            
            if (frm.DialogResult == DialogResult.Yes || frm.DialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(frm.PaymentTypeResult))
                    newPaymentName = frm.PaymentTypeResult;
                responseInfo = frm.responseInfo;
            }

            return isSuccess;

        }

        private bool IsNeedPaymentIntegration()
        {
            bool needIntegration = false;
            if (enableNETSIntegration
                || enableUNIONPayIntegration
                || enablePrepaidPurchaseIntegration
                || enableCUPIntegration
                || enableBCAIntegration
                || enableCitiBankTerminal
                || enableNetsCreditCardTerminal)
            {
                needIntegration = true;
            }

            return needIntegration;
        }

        public bool isIntegrationPayment(string paymentType)
        {
            if (paymentType == netsATMCardPayment || paymentType == netsFlashPayPayment ||
                paymentType == netsCashCardPayment || paymentType == CreditCardPayment ||
                paymentType == bcaPayment || paymentType == cupPayment || paymentType == prepaidPurchasePayment
                || paymentType == netsQRPayment)
                return true;
            return false;
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            // Start the counter time
            /*frmOrderTaking.counterStartTime = DateTime.Now;
            
            if (IsNeedPaymentIntegration())
            {
                if (isIntegrationPayment(paymentType.ToUpper()))
                {
                    DoNetsIntegration(paymentType);
                    return;
                }
            }

            if (enableNETSIntegration && ((Button)sender).Text.ToUpper().Equals("NETS"))
            {
                pnlNETSIntegration.Visible = true;
                return;
            }

            if (((Button)sender).Text.ToUpper() == AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType) &&
                        AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableEZLinkIntegration), false))
            {
                DoEzLinkIntegration();
                return;
            }

            if (((Button)sender).Text.ToUpper() == AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkContactlessPaymentType) &&
                        AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableEZLinkContactlessIntegration), false))
            {
                DoEzLinkIntegration();
                return;
            }

            

            if (handlePayment(((Button)sender).Text))
            {
                //GenericReport.NewPrint.A5Controller Printer = new GenericReport.NewPrint.A5Controller();
                //Printer.PrintInvoice(pos);

                //ask for signature here
                #region Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                }
                #endregion

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                    if (!syncSalesThread.IsBusy)
                        syncSalesThread.RunWorkerAsync();

                tryDownloadPoints();

                //print receipt
                if (PrintReceipt)
                {
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                }
                isSuccessful = true;
                this.Close();
            }*/

            string paymentType = ((Button)sender).Text;
            makePayment(paymentType);
        }

        public void makePayment(string PaymentType)
        {
            frmOrderTaking.counterStartTime = DateTime.Now;
            //string paymentType = ((Button)sender).Text;
            if (IsNeedPaymentIntegration())
            {
                if (isIntegrationPayment(PaymentType.ToUpper()))
                {
                    DoNetsIntegration(PaymentType);
                    return;
                }
            }

            if (enableNETSIntegration && (PaymentType.ToUpper().Equals("NETS")))
            {
                pnlNETSIntegration.Visible = true;
                return;
            }

            if (PaymentType.ToUpper() == AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType) &&
                        AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableEZLinkIntegration), false))
            {
                DoEzLinkIntegration();
                return;
            }

            if (PaymentType.ToUpper() == AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkContactlessPaymentType) &&
                        AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableEZLinkContactlessIntegration), false))
            {
                DoEzLinkIntegration();
                return;
            }



            if (handlePayment(PaymentType))
            {
                //GenericReport.NewPrint.A5Controller Printer = new GenericReport.NewPrint.A5Controller();
                //Printer.PrintInvoice(pos);

                //ask for signature here
                #region Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                }
                #endregion

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                    if (!syncSalesThread.IsBusy)
                        syncSalesThread.RunWorkerAsync();

                tryDownloadPoints();

                //print receipt
                if (PrintReceipt)
                {
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                }
                isSuccessful = true;
                this.Close();
            }
        }

        /*
        private void btnVisa_Click(object sender, EventArgs e)
        {
            if (handlePayment(POSController.PAY_VISA))
            {
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                isSuccessful = true;
                this.Close();
            }
        }
        private void btnMaster_Click(object sender, EventArgs e)
        {
            if (handlePayment(POSController.PAY_MASTER))
            {
                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);

                isSuccessful = true;
                this.Close();
            }
        }
        */
        /// <remarks>
        /// Do not handle Multiple Payment
        /// </remarks>
        public bool handlePayment(string paymentType)
        {
            try
            {
                decimal change;
                string status = "";

                #region
                if (paymentType == "欠款余额")
                {
                    paymentType = "Installment";
                }
                #endregion

                #region *) Validation: Installment payment must be attached to a member
                if (paymentType.ToLower() == "installment" 
                    && (!pos.MembershipApplied() || (pos.CurrentMember != null && pos.CurrentMember.MembershipNo.ToLower() == "walk-in")))
                    throw new Exception("(warning)Please assign a member to use installments");
                #endregion

                #region *) Initialization: Clear/Delete all PaymentList in ReceiptDetails
                pos.DeleteAllReceiptLinePayment();
                #endregion

                #region *) Warning: Notice user if there is extra charge
                decimal ExtraChargeTotalAmount = pos.CheckExtraChargeAmount(paymentType, amount);

                if (ExtraChargeTotalAmount != 0)
                {
                    //DialogResult DR = MessageBox.Show(
                    //    "There will be extra charge applicable of " + ExtraChargeTotalAmount.ToString("N2") + ". Do you want to continue?"
                    //    , "Extra Charge Applicable", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    //if (DR == DialogResult.Cancel) return false;

                    frmExtraCharge frmExt = new frmExtraCharge();
                    frmExt.totalAmount = amount;
                    frmExt.extraCharge = ExtraChargeTotalAmount;
                    frmExt.totalAmountAfterCharge = amount + ExtraChargeTotalAmount;
                    frmExt.ShowDialog();

                    if (!frmExt.isConfirmed) return false;
                }
                #endregion

                #region *) Core: Add PaymentList in ReceiptDetails
                if (paymentType == POSController.PAY_POINTS)
                {
                    SortedList<string, decimal> AddedPoints = new SortedList<string, decimal>();
                    #region *) Disabled: Enabled & review the following if you want to use the "newly bought" points
                    //foreach (OrderDet oneOrderDet in pos.myOrderDet)
                    //{
                    //    Item myOneItem = oneOrderDet.Item;
                    //    if (myOneItem.PointRedeemMode == Item.PointMode.Dollar)
                    //    {
                    //        if (AddedPoints.ContainsKey(myOneItem.ItemNo))
                    //            AddedPoints[myOneItem.ItemNo] += oneOrderDet.Quantity * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount);
                    //        else
                    //            AddedPoints.Add(myOneItem.ItemNo, oneOrderDet.Quantity * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount));
                    //    }
                    //}
                    #endregion

                    if (!Features.Package.BreakAmountIntoReceipts(pos.GetMemberInfo().MembershipNo, pos.GetOrderDate(), AddedPoints, amount, ref pos, out status))
                        throw new Exception(status);
                }
                else
                {
                    
                    if (!pos.AddReceiptLinePayment(pos.CalculateAmountForPayment(paymentType), paymentType, "",0,"",0, out change, out status))
                        throw new Exception(status);
                    
                }
                #endregion

                

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
 
                bool IsPointAllocationSuccess = true;
                bool isSuccessful;

                #region *) Core: Confirm Order
                isSuccessful = pos.ConfirmOrder
                    (false, out IsPointAllocationSuccess, out status);
                #endregion

                if (isSuccessful)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Magento.UseMagentoFeatures),false))
                    {
                        if (!pos.SyncToMagento(out status))
                        {
                            if (status != "")
                                MessageBox.Show(status);
                        }
                    }

                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    //if (!IsPointAllocationSuccess)
                    //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                    //if (!IsQtyInsufficient)
                    if (!pos.ExecuteStockOut(out status))
                    {
                        //MessageBox.Show
                        //    ("Error while performing Stock Out: " + status,
                        //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //Logger.writeLog("after stock out");
                    return true;
                }
                else
                {
                    pos.DeleteAllReceiptLinePayment();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;
            }
            catch (Exception X)
            {
                CommonUILib.HandleException(X);
                return false;
            }
        }


        private void frmSelectPayment_Load(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.lblRefNo.Text = pos.GetUnsavedRefNo().Substring(13);
            lblNumOfItems.Text = pos.GetSumOfItemQuantity().ToString();

            //btnPoints.Enabled = (pos != null) && (pos.MembershipApplied () /*string.IsNullOrEmpty(pos.CurrentMember.MembershipNo)*/) && Features.Package.isAvailable && Features.Package.isRealTime;
            //btnPoints.Enabled = (pos != null) && (pos.MembershipApplied() /*string.IsNullOrEmpty(pos.CurrentMember.MembershipNo)*/) && Features.Package.isAvailable;
            btnPoints.Enabled = (pos != null) && (pos.MembershipApplied() && pos.CurrentMember.MembershipNo.ToLower() != "walk-in") && Features.Package.isAvailable;
            if (!pos.MembershipApplied() || (pos.CurrentMember != null && pos.CurrentMember.MembershipNo.ToLower() == "walk-in"))
            {
                btnInstallment.Enabled = false;
            }

            //btnInstallment.Text = "INSTALLMENT";
            if (AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != null &&AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != "")
            {
                btnInstallment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText).ToUpper();
            }

            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            btnPWF.Visible = (enableFunding && enablePWF);
            btnPAMedifund.Visible = (enableFunding && pos.HasPAMedifundItem());
            btnSMF.Visible = (enableFunding && pos.HasSMFItem());

            UseCashRecyclerMachine = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.CashRecycler.EnableCashRecycler), false);

            //Check the 
            AutoSettleForCashback();
        }

        private void AutoSettleForCashback()
        {
            if (pos.GetCashBackAmount() <= 0)
                return;

            if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.WithCashbackOption), false))
                return;

            makePayment(POSController.PAY_NETSATMCard);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isSuccessful = false;
            this.Close();
        }

        /// <summary>
        /// Handle payment by points (Only)
        /// </summary>
        /// <remarks>
        /// *) If Not all line item can be paid by points, Exit
        /// *) If Points not sufficient, Exit
        /// </remarks>
        private void btnPoints_Click(object sender, EventArgs e)
        {
            try
            {
                if (!pos.MembershipApplied())
                    throw new Exception("(warning)Please assign a member to use point");

                if (pos.hasItemThatCannotBeRedeemedByPoints)
                    throw new Exception("(warning)Some item cannot be paid by points");

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.ChoosePointPackageForPayment), false))
                {
                    DataTable dt = new DataTable();

                    if (!Features.Package.GetCurrentAmounts_Points(pos.GetMemberInfo().MembershipNo, DateTime.Now, out dt, out status))
                    {
                        MessageBox.Show("No Points Available");
                        return;
                    }
                    else
                    {
                        #region *) asking pass code
                        bool isUsePoint = false;
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.IsAskingPassCode), false))
                        {
                            if (!pos.IsHavePasscode())
                            {
                                MessageBox.Show("This member doesn't have a pass code. Can not pay by point.");
                                return;
                            }

                            frmConfirmPassCode fc = new frmConfirmPassCode();
                            fc.pos = pos;
                            fc.ShowDialog();

                            if (fc.isConfirmed)
                            {
                                isUsePoint = true;
                            }
                        }
                        else
                        {
                            isUsePoint = true;
                        }
                        #endregion

                        if (isUsePoint)
                        {
                            decimal totalAmt = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                totalAmt = totalAmt + Convert.ToDecimal(dt.Rows[i][2].ToString());
                            }
                            if (pos.CalculateTotalAmount(out status) > totalAmt)
                            {
                                MessageBox.Show("Point is not Enough");
                                return;
                            }
                            frmSelectOtherPayment fSelectOtherPayment = new frmSelectOtherPayment();
                            fSelectOtherPayment.pos = pos;
                            fSelectOtherPayment.CurrentAmounts = dt;
                            fSelectOtherPayment.amount = amount;
                            //fSelectOtherPayment.packageList = packageList;
                            fSelectOtherPayment.membershipNo = pos.GetMemberInfo().MembershipNo;
                            fSelectOtherPayment.PrintReceipt = PrintReceipt;
                            fSelectOtherPayment.syncSalesThread = syncSalesThread;
                            fSelectOtherPayment.ParentSyncPointsThread = ParentSyncPointsThread;
                            fSelectOtherPayment.ShowDialog();
                            if (fSelectOtherPayment.isSuccessful)
                            {
                                isSuccessful = true;
                                this.Close();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    #region *) asking pass code
                    bool isUsePoint = false;
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.IsAskingPassCode), false))
                    {
                        if (!pos.IsHavePasscode())
                        {
                            MessageBox.Show("This member doesn't have a pass code. Can not pay by point.");
                            return;
                        }

                        frmConfirmPassCode fc = new frmConfirmPassCode();
                        fc.pos = pos;
                        fc.ShowDialog();

                        if (fc.isConfirmed)
                        {
                            isUsePoint = true;
                        }
                    }
                    else
                    {
                        isUsePoint = true;
                    }
                    #endregion

                    if (isUsePoint)
                    {
                        //ask for sign after breaking amount to receipt
                        #region Signature
                        //if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                        //{
                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailable), false))
                        {
                            //pop out whether want signature or not
                            frmCustomMessageBox myfrm = new frmCustomMessageBox
                                ("Add Signature", "Do you want to add signature to this transaction?");
                            DialogResult DR = myfrm.ShowDialog();

                            if (myfrm.choice == "yes")
                            {
                                myfrm.Dispose();

                                bool isUsingStandard = false;
                                string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                                if (string.IsNullOrEmpty(signatureDevice))
                                    signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                                isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                                if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                                {
                                    wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                                    if (usbDevices.Count == 0)
                                    {
                                        isUsingStandard = true;
                                        MessageBox.Show("There is no STU device attached, using standard signature form");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                            frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                            demo.ShowDialog();
                                            List<wgssSTU.IPenData> penData = demo.getPenData();
                                            if (penData != null)
                                            {
                                                // process penData here!

                                                wgssSTU.IInformation information = demo.getInformation();
                                                wgssSTU.ICapability capability = demo.getCapability();
                                            }
                                            demo.Dispose();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("An error occurred, please contact admin");
                                            Logger.writeLog(ex);
                                        }
                                    }
                                }

                                if (isUsingStandard)
                                {
                                    //asking for Signature
                                    frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                                    f.ShowDialog();
                                    if (f.IsSuccessful)
                                    {
                                        f.Dispose();
                                    }
                                }
                            }
                            else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                                myfrm.Dispose();
                            /*
                        else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                        {
                            myfrm.Dispose();
                            return;
                        }
                             */
                        }
                        //}
                        #endregion

                        if (handlePayment(POSController.PAY_POINTS))
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                if (!syncSalesThread.IsBusy)
                                    syncSalesThread.RunWorkerAsync();

                            tryDownloadPoints();

                            if (PrintReceipt)
                            {
                                Logger.writeLog("start printing");
                                //tryDownloadPoints();
                                POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                Logger.writeLog("end printing");
                            }
                            isSuccessful = true;
                            this.Close();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception X)
            {
                if (X.Message.StartsWith("(warning)"))
                {
                    MessageBox.Show(X.Message.Replace("(warning)", ""), "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (X.Message.StartsWith("(error)"))
                {
                    MessageBox.Show(X.Message.Replace("(error)", ""), "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Logger.writeLog(X);
                    MessageBox.Show(
                        "Cannot check the remaining points due to the connection problem, please check your connection and retry.", "Error"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            if (handlePayment("INSTALLMENT"))
            {
                //GenericReport.NewPrint.A5Controller Printer = new GenericReport.NewPrint.A5Controller();
                //Printer.PrintInvoice(pos);

                //ask for signature here
                #region Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                    if (!syncSalesThread.IsBusy)
                        syncSalesThread.RunWorkerAsync();

                tryDownloadPoints();
                //print receipt
                if (PrintReceipt)
                {
                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                }
                isSuccessful = true;
                this.Close();
            }
        }

        private void btnPAMedifund_Click(object sender, EventArgs e)
        {
            pos.FundingMethod = POSController.PAY_PAMEDIFUND;
            btnOther_Click(sender, e);
        }

        private void btnSMF_Click(object sender, EventArgs e)
        {
            pos.FundingMethod = POSController.PAY_SMF;
            btnOther_Click(sender, e);
        }

        private void btnPWF_Click(object sender, EventArgs e)
        {
            btnOther_Click(sender, e);
        }

        private void btnNETSATMCard_Click(object sender, EventArgs e)
        {
            //if (handlePayment(((Button)sender).Text))
            //{
            decimal change = 0;
            string status = "";
            bool IsQtyInsufficient = false;
            bool dr1 = true;
            

            if (!IsQtyInsufficient || dr1)
            {
                //ask for signature here
                #region *) Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion

                bool isRetry = true;
                while (isRetry)
                {
                    //try Connect to port.
                    string portName = "";
                    DataSet ds3 = new DataSet();
                    ds3.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\NetsComPort.xml");
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        portName = ds3.Tables[0].Rows[0].ItemArray[0].ToString();
                    }

                    NetsController nets = new NetsController();
                    if (nets.setupSerialPort(portName))
                    {
                        //show the payment panel
                        frmLoading fl = new frmLoading("Please Insert ATM Card");
                        fl.Show();

                        ShowAutoClosingMessageBox("Initializing ...", "Info");

                        //do payment
                        status = "";
                        nets.addNetsCardPayment(pos.CalculateTotalAmount(out status).ToString(), "        ", out status);
                        fl.Close();


                        if (status.StartsWith("ERROR"))
                        {
                            frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                            frm.PaymentType = "Nets Card";
                            frm.Status = status;
                            frm.ShowDialog();
                            if (frm.Result == NETConfirmation.Cancel)
                            {
                                isRetry = false;
                                isSuccessful = false;
                                return;
                            }
                            else if (frm.Result == NETConfirmation.Manual)
                            {
                                amount = frm.Amount;
                                if (handlePayment("NetsCard"))
                                {
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                        if (!syncSalesThread.IsBusy)
                                            syncSalesThread.RunWorkerAsync();

                                    tryDownloadPoints();

                                    if (PrintReceipt)
                                    {
                                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                    }
                                    isSuccessful = true;
                                    isRetry = false;
                                    this.Close();
                                }
                            }
                            else if (frm.Result == NETConfirmation.Retry)
                            {
                                isRetry = true;
                            }
                        }
                        else
                        {
                            // add into the database
                            if (!pos.AddReceiptLinePayment(amount, "NetsCard", "", 0, "", 0, out change, out status))
                                throw new Exception(status);
                            bool IsPointAllocationSuccess;
                            bool Success;

                            #region *) Core: Confirm Order
                            Success = pos.ConfirmOrder
                                (false, out IsPointAllocationSuccess, out status);
                            #endregion

                            if (Success)
                            {
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                //if (!IsPointAllocationSuccess)
                                //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                                //if (!IsQtyInsufficient)
                                if (!pos.ExecuteStockOut(out status))
                                {
                                    //MessageBox.Show
                                    //    ("Error while performing Stock Out: " + status,
                                    //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                //print receipt
                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                            else
                            {
                                pos.DeleteAllReceiptLinePayment();
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isRetry = false;
                                return;
                            }
                        }
                    }
                    else
                    {
                        frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                        frm.PaymentType = "Nets Card";
                        frm.Status = "Connect to Nets Port Failed, Please check the port name";
                        frm.ShowDialog();
                        if (frm.Result == NETConfirmation.Cancel)
                        {
                            isRetry = false;
                            isSuccessful = false;
                            break;
                        }
                        else if (frm.Result == NETConfirmation.Manual)
                        {
                            amount = frm.Amount;
                            if (handlePayment("NetsCard"))
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                        }
                        else if (frm.Result == NETConfirmation.Retry)
                        {
                            isRetry = true;
                        }
                    }
                }
            }
        }

        private void btnFlashPay_Click(object sender, EventArgs e)
        {
            decimal change = 0;
            string status = "";
            bool IsQtyInsufficient = false;
            bool dr1 = true;

            if (!IsQtyInsufficient || dr1)
            {
                //ask for signature here
                #region *) Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        //asking for Signature
                        frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                        f.ShowDialog();
                        if (f.IsSuccessful)
                        {
                            f.Dispose();
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion

                bool isRetry = true;
                while (isRetry)
                {
                    //try Connect to port.
                    string portName = "";
                    DataSet ds3 = new DataSet();
                    ds3.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\NetsComPort.xml");
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        portName = ds3.Tables[0].Rows[0].ItemArray[0].ToString();
                    }
                    NetsController nets = new NetsController();

                    if (nets.setupSerialPort(portName))
                    {
                        //show the payment panel
                        frmLoading fl = new frmLoading("Please Tap FlashPay Card");
                        fl.Show();

                        ShowAutoClosingMessageBox("Initializing ...", "Info");

                        //do payment
                        status = "";
                        decimal amount = pos.CalculateTotalAmount(out status);
                        nets.addFlashPayPayment(amount.ToString(), "        ", out status);

                        fl.Close();
                        if (status.StartsWith("ERROR"))
                        {
                            frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                            frm.PaymentType = "Nets Flash Pay";
                            frm.Status = status;
                            frm.ShowDialog();
                            if (frm.Result == NETConfirmation.Cancel)
                            {
                                isRetry = false;
                                isSuccessful = false;
                                return;
                            }
                            else if (frm.Result == NETConfirmation.Manual)
                            {
                                amount = frm.Amount;
                                if (handlePayment("NetsFlashPay"))
                                {
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                        if (!syncSalesThread.IsBusy)
                                            syncSalesThread.RunWorkerAsync();

                                    tryDownloadPoints();

                                    if (PrintReceipt)
                                    {
                                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                    }
                                    isSuccessful = true;
                                    isRetry = false;
                                    this.Close();
                                }
                            }
                            else if (frm.Result == NETConfirmation.Retry)
                            {
                                isRetry = true;
                            }
                        }
                        else
                        {
                            // add into the database
                            if (!pos.AddReceiptLinePayment(amount, "NetsFlashPay", "", 0, "", 0, out change, out status))
                                throw new Exception(status);
                            bool IsPointAllocationSuccess;
                            bool Success;

                            #region *) Core: Confirm Order
                            Success = pos.ConfirmOrder
                                (false, out IsPointAllocationSuccess, out status);
                            #endregion

                            if (Success)
                            {

                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                //if (!IsPointAllocationSuccess)
                                //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                                if (!pos.ExecuteStockOut(out status))
                                {

                                }

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                //print receipt
                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                            else
                            {
                                pos.DeleteAllReceiptLinePayment();
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isRetry = false;
                                return;
                            }
                        }
                    }
                    else
                    {
                        frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                        frm.PaymentType = "Nets Flash Pay";
                        frm.Status = "Connect to Nets Port Failed, Please check the port name";
                        frm.ShowDialog();
                        if (frm.Result == NETConfirmation.Cancel)
                        {
                            isRetry = false;
                            isSuccessful = false;
                            break;
                        }
                        else if (frm.Result == NETConfirmation.Manual)
                        {
                            amount = frm.Amount;
                            if (handlePayment("NetsFlashPay"))
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                if (PrintReceipt)
                                {
                                    

                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                        }
                        else if (frm.Result == NETConfirmation.Retry)
                        {
                            isRetry = true;
                        }
                    }
                }
            }
        }

        private void btnCashCard_Click(object sender, EventArgs e)
        {
            //if (handlePayment(((Button)sender).Text))
            //{
            decimal change = 0;
            string status = "";
            bool IsQtyInsufficient = false;
            bool dr1 = true;

            #region *) Validation: Check if Qty is sufficient to do stockout [Prompt if Error - Can be terminated by user]
            if (!pos.IsQtySufficientToDoStockOut(out status))
            {

                DialogResult dr = MessageBox.Show
                    ("Error: " + status + ". Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                //DialogResult dr = DialogResult.Yes;
                if (dr == DialogResult.No)
                {
                    pos.DeleteAllReceiptLinePayment();
                    dr1 = false;
                }
                IsQtyInsufficient = true;
            }
            #endregion

            if (!IsQtyInsufficient || dr1)
            {
                //ask for signature here
                #region *) Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion

                bool isRetry = true;
                while (isRetry)
                {
                    //try Connect to port.
                    string portName = "";
                    DataSet ds3 = new DataSet();
                    ds3.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\NetsComPort.xml");
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        portName = ds3.Tables[0].Rows[0].ItemArray[0].ToString();
                    }

                    NetsController nets = new NetsController();
                    if (nets.setupSerialPort(portName))
                    {
                        //show the payment panel
                        frmLoading fl = new frmLoading("Please Insert Cash Card");
                        fl.Show();

                        ShowAutoClosingMessageBox("Initializing ...", "Info");

                        //do payment
                        status = "";
                        nets.addCashCardDebitPayment(pos.CalculateTotalAmount(out status).ToString(), "        ", out status);
                        fl.Close();
                        if (status.StartsWith("ERROR"))
                        {
                            frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                            frm.PaymentType = "Nets Cash Card";
                            frm.Status = status;
                            frm.ShowDialog();
                            if (frm.Result == NETConfirmation.Cancel)
                            {
                                isRetry = false;
                                isSuccessful = false;
                                break;
                            }
                            else if (frm.Result == NETConfirmation.Manual)
                            {
                                amount = frm.Amount;
                                if (handlePayment("CashCard"))
                                {
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                        if (!syncSalesThread.IsBusy)
                                            syncSalesThread.RunWorkerAsync();

                                    tryDownloadPoints();

                                    if (PrintReceipt)
                                    {
                                        
                                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                    }
                                    isSuccessful = true;
                                    isRetry = false;
                                    this.Close();
                                }
                            }
                            else if (frm.Result == NETConfirmation.Retry)
                            {
                                isRetry = true;
                            }
                        }
                        else
                        {
                            // add into the database
                            if (!pos.AddReceiptLinePayment(amount, "CashCard", "", 0, "", 0, out change, out status))
                                throw new Exception(status);
                            bool IsPointAllocationSuccess;
                            bool Success;

                            #region *) Core: Confirm Order
                            Success = pos.ConfirmOrder
                                (false, out IsPointAllocationSuccess, out status);
                            #endregion

                            if (Success)
                            {

                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                //if (!IsPointAllocationSuccess)
                                //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                                //if (!IsQtyInsufficient)
                                if (!pos.ExecuteStockOut(out status))
                                {

                                }

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                //print receipt
                                if (PrintReceipt)
                                {
                                    

                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                            else
                            {
                                pos.DeleteAllReceiptLinePayment();
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                        frm.PaymentType = "Nets Cash Card";
                        frm.Status = "Connect to Nets Port Failed, Please check the port name";
                        frm.ShowDialog();
                        if (frm.Result == NETConfirmation.Cancel)
                        {
                            isRetry = false;
                            isSuccessful = false;
                            break;
                        }
                        else if (frm.Result == NETConfirmation.Manual)
                        {
                            amount = frm.Amount;
                            if (handlePayment("CashCard"))
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                if (PrintReceipt)
                                {

                                    
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                        }
                        else if (frm.Result == NETConfirmation.Retry)
                        {
                            isRetry = true;
                        }
                    }
                }
            }
        }

        private bool DoEzLinkIntegration()
        {
            //if (handlePayment(((Button)sender).Text))
            //{
            decimal change = 0;
            string status = "";
            bool IsQtyInsufficient = false;
            bool dr1 = true;

            if (!IsQtyInsufficient || dr1)
            {
                //ask for signature here
                #region *) Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion

                bool isRetry = true;
                while (isRetry)
                {
                    //try Connect to port.
                    string portName = "";
                    DataSet ds3 = new DataSet();
                    ds3.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\NetsComPort.xml");
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        portName = ds3.Tables[0].Rows[0].ItemArray[1].ToString();
                    }

                    EZLinkController nets = new EZLinkController();
                    if (nets.setupSerialPort(portName))
                    {
                        //show the payment panel
                        frmLoading fl = new frmLoading("Please Tap Ez Link Card");
                        fl.Show();

                        ShowAutoClosingMessageBox("Initializing ...", "Info");

                        //do payment
                        status = "";
                        nets.addEZLinkPayment(pos.CalculateTotalAmount(out status).ToString(), out status);
                        fl.Close();


                        if (status.StartsWith("ERROR"))
                        {
                            frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                            frm.PaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType);
                            frm.Status = status;
                            frm.ShowDialog();
                            if (frm.Result == NETConfirmation.Cancel)
                            {
                                isRetry = false;
                                isSuccessful = false;
                                return false;
                            }
                            else if (frm.Result == NETConfirmation.Manual)
                            {
                                amount = frm.Amount;
                                if (handlePayment(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType)))
                                {
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                        if (!syncSalesThread.IsBusy)
                                            syncSalesThread.RunWorkerAsync();

                                    tryDownloadPoints();

                                    if (PrintReceipt)
                                    {
                                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                    }
                                    isSuccessful = true;
                                    isRetry = false;
                                    this.Close();
                                }
                            }
                            else if (frm.Result == NETConfirmation.Retry)
                            {
                                isRetry = true;
                            }
                        }
                        else
                        {
                            // add into the database
                            if (!pos.AddReceiptLinePayment(amount, AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType), "", 0, "", 0, out change, out status))
                                throw new Exception(status);
                            bool IsPointAllocationSuccess;
                            bool Success;

                            #region *) Core: Confirm Order
                            Success = pos.ConfirmOrder
                                (false, out IsPointAllocationSuccess, out status);
                            #endregion

                            if (Success)
                            {
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                //if (!IsPointAllocationSuccess)
                                //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                                //if (!IsQtyInsufficient)
                                if (!pos.ExecuteStockOut(out status))
                                {
                                    //MessageBox.Show
                                    //    ("Error while performing Stock Out: " + status,
                                    //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                //print receipt
                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                            else
                            {
                                pos.DeleteAllReceiptLinePayment();
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isRetry = false;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                        frm.PaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType);
                        frm.Status = "Connect to EZLink Port Failed, Please check the port name";
                        frm.ShowDialog();
                        if (frm.Result == NETConfirmation.Cancel)
                        {
                            isRetry = false;
                            isSuccessful = false;
                            break;
                        }
                        else if (frm.Result == NETConfirmation.Manual)
                        {
                            amount = frm.Amount;
                            if (handlePayment(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType)))
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                        }
                        else if (frm.Result == NETConfirmation.Retry)
                        {
                            isRetry = true;
                        }
                    }
                }
            }
            return true;
        }

        private bool DoEzLinkContactlessIntegration()
        {
            //if (handlePayment(((Button)sender).Text))
            //{
            decimal change = 0;
            string status = "";
            bool IsQtyInsufficient = false;
            bool dr1 = true;

            if (!IsQtyInsufficient || dr1)
            {
                //ask for signature here
                #region *) Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                    /*
                else if (DR == DialogResult.Cancel || myfrm.choice == "cancel")
                {
                    myfrm.Dispose();
                    return;
                }
                     */
                }
                #endregion

                bool isRetry = true;
                while (isRetry)
                {
                    //try Connect to port.
                    string portName = "";
                    DataSet ds3 = new DataSet();
                    ds3.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\NetsComPort.xml");
                    if (ds3.Tables[0].Rows.Count > 0)
                    {
                        portName = ds3.Tables[0].Rows[0].ItemArray[1].ToString();
                    }

                    EZLinkController nets = new EZLinkController();
                    if (nets.setupSerialPort(portName))
                    {
                        //show the payment panel
                        frmLoading fl = new frmLoading("Please Tap Ez Link Card");
                        fl.Show();

                        ShowAutoClosingMessageBox("Initializing ...", "Info");

                        //do payment
                        status = "";
                        nets.addEZLinkContactlessPayment(pos.CalculateTotalAmount(out status).ToString(), out status);
                        fl.Close();


                        if (status.StartsWith("ERROR"))
                        {
                            frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                            frm.PaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkContactlessPaymentType);
                            frm.Status = status;
                            frm.ShowDialog();
                            if (frm.Result == NETConfirmation.Cancel)
                            {
                                isRetry = false;
                                isSuccessful = false;
                                return false;
                            }
                            else if (frm.Result == NETConfirmation.Manual)
                            {
                                amount = frm.Amount;
                                if (handlePayment(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType)))
                                {
                                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                        if (!syncSalesThread.IsBusy)
                                            syncSalesThread.RunWorkerAsync();

                                    tryDownloadPoints();

                                    if (PrintReceipt)
                                    {
                                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                            (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                            PrintSettingInfo.receiptSetting.PaperSize.Value),
                                            PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                    }
                                    isSuccessful = true;
                                    isRetry = false;
                                    this.Close();
                                }
                            }
                            else if (frm.Result == NETConfirmation.Retry)
                            {
                                isRetry = true;
                            }
                        }
                        else
                        {
                            // add into the database
                            if (!pos.AddReceiptLinePayment(amount, "NetsCard", "", 0, "", 0, out change, out status))
                                throw new Exception(status);
                            bool IsPointAllocationSuccess;
                            bool Success;

                            #region *) Core: Confirm Order
                            Success = pos.ConfirmOrder
                                (false, out IsPointAllocationSuccess, out status);
                            #endregion

                            if (Success)
                            {
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                //if (!IsPointAllocationSuccess)
                                //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                                //if (!IsQtyInsufficient)
                                if (!pos.ExecuteStockOut(out status))
                                {
                                    //MessageBox.Show
                                    //    ("Error while performing Stock Out: " + status,
                                    //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                //print receipt
                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                    (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                    PrintSettingInfo.receiptSetting.PaperSize.Value),
                                    PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                            else
                            {
                                pos.DeleteAllReceiptLinePayment();
                                this.Cursor = System.Windows.Forms.Cursors.Default;
                                MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                isRetry = false;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        frmNETSConfirmation frm = new frmNETSConfirmation(pos);
                        frm.PaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkContactlessPaymentType);
                        frm.Status = "Connect to Nets Port Failed, Please check the port name";
                        frm.ShowDialog();
                        if (frm.Result == NETConfirmation.Cancel)
                        {
                            isRetry = false;
                            isSuccessful = false;
                            break;
                        }
                        else if (frm.Result == NETConfirmation.Manual)
                        {
                            amount = frm.Amount;
                            if (handlePayment(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkContactlessPaymentType)))
                            {
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                                    if (!syncSalesThread.IsBusy)
                                        syncSalesThread.RunWorkerAsync();

                                tryDownloadPoints();

                                if (PrintReceipt)
                                {
                                    POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                                }
                                isSuccessful = true;
                                isRetry = false;
                                this.Close();
                            }
                        }
                        else if (frm.Result == NETConfirmation.Retry)
                        {
                            isRetry = true;
                        }
                    }
                }
            }
            return true;
        }

        private bool DoNetsIntegration(string _paymentType)
        {
            decimal change = 0;
            string status = "";
            bool IsQtyInsufficient = false;
            bool dr1 = true;

            if (!IsQtyInsufficient || dr1)
            {
                //ask for signature here
                #region *) Signature
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                {
                    //pop out whether want signature or not
                    frmCustomMessageBox myfrm = new frmCustomMessageBox
                        ("Add Signature", "Do you want to add signature to this transaction?");
                    DialogResult DR = myfrm.ShowDialog();

                    if (myfrm.choice == "yes")
                    {
                        myfrm.Dispose();

                        bool isUsingStandard = false;
                        string signatureDevice = AppSetting.GetSetting(AppSetting.SettingsName.Signature.SignatureDevice);
                        if (string.IsNullOrEmpty(signatureDevice))
                            signatureDevice = AppSetting.SettingsName.Signature.STANDARD;
                        isUsingStandard = signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.STANDARD.ToUpper());

                        if (signatureDevice.ToUpper().Equals(AppSetting.SettingsName.Signature.WACOM.ToUpper()))
                        {
                            wgssSTU.UsbDevices usbDevices = new wgssSTU.UsbDevices();

                            if (usbDevices.Count == 0)
                            {
                                isUsingStandard = true;
                                MessageBox.Show("There is no STU device attached, using standard signature form");
                            }
                            else
                            {
                                try
                                {
                                    wgssSTU.IUsbDevice usbDevice = usbDevices[0]; // select a device

                                    frmSignatureWacom demo = new frmSignatureWacom(usbDevice, pos.GetUnsavedRefNo());
                                    demo.ShowDialog();
                                    List<wgssSTU.IPenData> penData = demo.getPenData();
                                    if (penData != null)
                                    {
                                        // process penData here!

                                        wgssSTU.IInformation information = demo.getInformation();
                                        wgssSTU.ICapability capability = demo.getCapability();
                                    }
                                    demo.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("An error occurred, please contact admin");
                                    Logger.writeLog(ex);
                                }
                            }
                        }

                        if (isUsingStandard)
                        {
                            //asking for Signature
                            frmSignature f = new frmSignature(pos.GetUnsavedRefNo());
                            f.ShowDialog();
                            if (f.IsSuccessful)
                            {
                                f.Dispose();
                            }
                        }
                    }
                    else if (myfrm.choice == "no" || DR == DialogResult.Cancel)
                        myfrm.Dispose();
                }
                #endregion

                #region Call Payment Integration
                string newPaymentName;
                string responseInfo;
                if (!CallPaymentIntegration(_paymentType,pos.CalculateAmountForPayment(_paymentType), false, out newPaymentName, out responseInfo))
                {
                    return false;
                }
                #endregion

                if (newPaymentName == "")
                    newPaymentName = _paymentType;
                // add into the database
                if (!pos.AddReceiptLinePayment(pos.CalculateAmountForPayment(_paymentType), newPaymentName, "", 0, "", 0, out change, out status, false, responseInfo))
                    throw new Exception(status);
                bool IsPointAllocationSuccess;
                bool Success;

                #region *) Core: Confirm Order
                Success = pos.ConfirmOrder
                    (false, out IsPointAllocationSuccess, out status);
                #endregion

                if (Success)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    //if (!IsPointAllocationSuccess)
                    //{ MessageBox.Show("Point is not updated!\n" + status.Replace("(error)", "").Replace("(warning)", ""), "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                    //if (!IsQtyInsufficient)
                    if (!pos.ExecuteStockOut(out status))
                    {
                        //MessageBox.Show
                        //    ("Error while performing Stock Out: " + status,
                        //    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Sync.UseRealTimeSales), false))
                        if (!syncSalesThread.IsBusy)
                            syncSalesThread.RunWorkerAsync();

                    tryDownloadPoints();

                    //print receipt
                    if (PrintReceipt)
                    {
                        POSDeviceController.PrintAHAVATransactionReceipt(pos, 0, false,
                        (ReceiptSizes)Enum.ToObject(typeof(ReceiptSizes),
                        PrintSettingInfo.receiptSetting.PaperSize.Value),
                        PrintSettingInfo.receiptSetting.NumOfCopies.Value);
                    }
                    isSuccessful = true;
                    this.Close();
                }
                else
                {
                    pos.DeleteAllReceiptLinePayment();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    MessageBox.Show("Error encountered: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        #region For Message Box Auto Close

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.Dll")]
        static extern int PostMessage(IntPtr hWnd, UInt32 msg, int wParam, int lParam);

        private const UInt32 WM_CLOSE = 0x0010;

        public void ShowAutoClosingMessageBox(string message, string caption)
        {
            var timer = new System.Timers.Timer(50) { AutoReset = false };
            timer.Elapsed += delegate
            {
                IntPtr hWnd = FindWindowByCaption(IntPtr.Zero, caption);
                if (hWnd.ToInt32() != 0) PostMessage(hWnd, WM_CLOSE, 0, 0);
            };
            timer.Enabled = true;
            MessageBox.Show(message, caption);
        }

        #endregion

        private void btnNETSBack_Click(object sender, EventArgs e)
        {
            pnlNETSIntegration.Visible = false;
        }
    }
}
