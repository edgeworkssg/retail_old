using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using POSDevices;
//using WinPowerPOS.InstallmentForms;
using System.Configuration;
using PowerPOSLib.Controller;
using System.Runtime.InteropServices;
using PowerPOS.Container;
namespace WinPowerPOS.OrderForms
{

    public partial class frmPartialPayment : Form
    {

        public POSController pos;
        PriceDisplay myDisplay;
        public bool IsEdit;
        public bool hasInstallment;
        public bool IsSuccessful;
        public decimal change;
        private decimal OrderAmount;
        private string ChequeNo, BankName;
        int poleDisplayWidth = 0;
        //private bool enableNETSIntegration = false;

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
        //bool enableCreditCardIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCreditCardIntegration), false);
        //string CreditCardPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped) + "").Trim().ToUpper();

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

        public BackgroundWorker ParentSyncPointsThread;

        public bool CustomerIsAMember
        {
            get
            { return btnPoint.Enabled; }
            set
            {
                btnPoint.Enabled = value;
            }

        }

        public frmPartialPayment()
        {
            InitializeComponent();
            //enableNETSIntegration = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSIntegration), false);
            LoadPaymentTypeLabels();
            
            myDisplay = new PriceDisplay();
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
                    string label =PaymentTypesController.FetchPaymentByID(btnPay.Tag.ToString());
                    if (label != "" && label != "-" &&
                        label.ToLower() != "points" &&
                        label.ToLower() != "point" 
                        && label.ToLower() != "installment")
                    {
                        btnPay.Text = label;
                    }
                    else
                    {
                        btnPay.Visible = false;
                    }
                    if (enableNetsCreditCardTerminal &&
                            (label == netsCC_VISA || label == netsCC_MASTER || label == netsCC_AMEX
                            || label == netsCC_DINERS || label == netsCC_JCB))
                    {
                        btnPay.Visible = false;
                        btnPay.Enabled = false;
                    }

                    if (enableCitiBankTerminal &&
                            (label == citiBank_VISA || label == citiBank_MASTER || label == citiBank_AMEX
                            || label == citiBank_DINERS || label == citiBank_JCB))
                    {
                        btnPay.Visible = false;
                        btnPay.Enabled = false;
                    }
                }
            }

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

            btnCashCard.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            btnCashCard.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSCashCard), false);
            btnFlashPay.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            btnFlashPay.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSFlashPay), false);
            btnNETSATMCard.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            btnNETSATMCard.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSATMCard), false);
            btnNETSQR.Visible = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSQR), false);
            btnNETSQR.Enabled = enableNETSIntegration && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableNETSQR), false);

        }
        private void frmPartialPayment_Load(object sender, EventArgs e)
        {
            string refno = pos.GetUnsavedRefNo();
            hasInstallment = pos.hasPaymentType(POSController.PAY_INSTALLMENT);
            lblRefNo.Text = refno.Substring(refno.Length - 3, 3);
            lblNumOfItems.Text = pos.GetSumOfItemQuantity().ToString();
            string status;
            change = 0;
            if (!decimal.TryParse(lblAmount.Text, out OrderAmount))
            {
                MessageBox.Show("Error. Invalid Order amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IsSuccessful = false;
                this.Close();
            }
            dgvPayment.AutoGenerateColumns = false;

            #region *) Funding Calculation
            Funding_Calculation();
            #endregion

            BindGrid();
            myDisplay.ShowTotal((double)pos.CalculateTotalAmount(out status));
            if (!pos.MembershipApplied() || (pos.CurrentMember != null && pos.CurrentMember.MembershipNo.ToLower() == "walk-in"))
            {
                btnInstallment.Enabled = false;
            }
            if (AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != null && AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText) != "")
            {
                btnInstallment.Text = AppSetting.GetSetting(AppSetting.SettingsName.Payment.InstallmentText).ToUpper();
            }

            bool enableFunding = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnableFunding), false);
            bool enablePWF = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Funding.EnablePWF), false);
            btnPWF.Visible = (enableFunding && enablePWF);
            btnPAMedifund.Visible = (enableFunding && pos.HasPAMedifundItem());
            btnSMF.Visible = (enableFunding && pos.HasSMFItem());
        }

        private void Funding_Calculation()
        {
            if (pos.IsFundingSelected())
            {
                // Clear the receiptdet first
                pos.DeleteAllReceiptLinePayment();
                pos.ClearFundingAmount();

                decimal fundingAmt = 0;
                string status;
                if (pos.FundingMethod == POSController.PAY_PAMEDIFUND)
                {
                    if (!pos.Funding_ApplyPAMedifund(out status, out fundingAmt))
                    {
                        MessageBox.Show("Error applying PA Medifund funding method" + Environment.NewLine + status, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IsSuccessful = false;
                        this.Close();
                    }
                }
                else if (pos.FundingMethod == POSController.PAY_SMF)
                {
                    if (!pos.Funding_ApplySMF(out status, out fundingAmt))
                    {
                        MessageBox.Show("Error applying SMF funding method" + Environment.NewLine + status, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IsSuccessful = false;
                        this.Close();
                    }
                }

                BindGrid();

                //if (fundingAmt > 0)
                //{
                //    txtAmt.Text = fundingAmt.ToString("N");
                //    AddPayment(pos.FundingMethod);
                //    txtAmt.Text = "";
                //}
            }
        }

        private void BindGrid()
        {
            string status = "";

            dgvPayment.DataSource = pos.FetchUnsavedReceipt(true);
            dgvPayment.Refresh();
            decimal totalPaid = pos.CalculateTotalPaid(out status);
            decimal totalOrderAmt;
            if (cbRounding.Checked)
            {
                totalOrderAmt = pos.RoundTotalReceiptAmount();
            }
            else
            {
                totalOrderAmt = pos.CalculateAmountForPayment("MIX");
            }
            totalOrderAmt = Math.Round(totalOrderAmt, 2, MidpointRounding.AwayFromZero);
            lblAmount.Text = totalOrderAmt.ToString("N");
            OrderAmount = totalOrderAmt;
            
            if (status != "")
            {
                MessageBox.Show("Error while calculating total amount: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTotalPaid.Text = totalPaid.ToString("N");
            lblShortFall.Text = (totalOrderAmt - totalPaid).ToString("N");
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.PoleDisplay.PoleDisplayLinesLength), out poleDisplayWidth))
            {
                poleDisplayWidth = 20;
            }
            myDisplay.ClearScreen();
            myDisplay.ShowItemPrice("Balance: ", (double)(totalOrderAmt - totalPaid), (double)totalOrderAmt, poleDisplayWidth);
        }

        private void AddPayment(string paymentType)
        {

            try
            {
                decimal paymentAmt = 0, TotalPaid = 0;
                decimal foreignCurrencyRate = 0;
                string foreignCurrencyCode = "";
                decimal foreignCurrencyAmount = 0;
                string status, refno = "";
                /*
                if (txtAmt.Text == "")
                {
                    txtAmt.Text = lblShortFall.Text;
                }*/

                #region Change Chinese Character of installment to follow english
                if (paymentType == "欠款余额")
                {
                    paymentType = "Installment";
                }
                #endregion

                

                //Validate txtAmount
                if (!decimal.TryParse(lblTotalPaid.Text, out TotalPaid) || !decimal.TryParse(txtAmt.Text, out paymentAmt) || (OrderAmount > 0 && paymentAmt <= 0))
                {
                    /*MessageBox.Show("The entered amount seems to be invalid.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;*/
                    //if (paymentType != POSController.PAY_VOUCHER)
                    {
                        frmKeypadFC k = new frmKeypadFC();
                        k.IsInteger = false;
                        k.textMessage = "O/S:" + lblShortFall.Text;
                        if (paymentType == POSController.PAY_POINTS)
                        {
                            decimal InitValue = pos.getAllRedeemableTotalAmount;
                            InitValue -= pos.CalculateTotalPaid_ByPoints(out status);
                            k.initialValue = InitValue.ToString();
                        }
                        DialogResult dlg = k.ShowDialog();
                        foreignCurrencyAmount = k.foreignCurrencyAmount;
                        foreignCurrencyCode = k.foreignCurrencyCode;
                        foreignCurrencyRate = k.foreignCurrencyRate;
                        if (dlg == DialogResult.Cancel || !decimal.TryParse(k.value, out paymentAmt))
                        {
                            //MessageBox.Show("The entered amount seems to be invalid.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            BindGrid();
                            return;
                        }
                    }
                    /*    
                    else
                    {
                        //Prompt Scanning of Voucher number....
                        frmRedeemVoucher frm = new frmRedeemVoucher();
                        frm.ShowDialog();
                        if (!frm.IsSuccessful)
                            return;

                        paymentAmt = frm.myVoucher.Amount;
                        refno = frm.myVoucher.VoucherNo;
                    }*/
                }

                #region *) Warning: Notice user if there is extra charge
                decimal ExtraChargeTotalAmount = pos.CheckExtraChargeAmount(paymentType, paymentAmt);

                if (ExtraChargeTotalAmount != 0)
                {
                    //DialogResult DR = MessageBox.Show(
                    //    "There will be extra charge applicable of " + ExtraChargeTotalAmount.ToString("N") + ". Do you still want to continue?"
                    //    , "Extra Charge Applicable", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                    //if (DR == DialogResult.Cancel) return;
                    frmExtraCharge frmExt = new frmExtraCharge();
                    frmExt.totalAmount = paymentAmt;
                    frmExt.extraCharge = ExtraChargeTotalAmount;
                    frmExt.totalAmountAfterCharge = paymentAmt + ExtraChargeTotalAmount;
                    frmExt.ShowDialog();

                    if (!frmExt.isConfirmed) return;
                }
                #endregion

                #region -= Point Validation =-
                if (paymentType == POSController.PAY_POINTS)
                {
                    decimal TotalPointableSales = pos.getAllRedeemableTotalAmount;
                    decimal PointsPaid = pos.CalculateTotalPaid_ByPoints(out status);
                    decimal PointOnServer;

                    //if (!pos.getAvailablePointOnServer(out PointOnServer, out status))
                    if (!PowerPOS.Feature.Package.getAvailablePoints(pos.CurrentMember.MembershipNo, pos.GetOrderDate(), out PointOnServer, out status))
                    {
                        Logger.writeLog(status);
                        MessageBox.Show("Some error occurred: " + status, "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //foreach (OrderDet oneOrderDet in pos.myOrderDet)
                    //{
                    //    Item myOneItem = oneOrderDet.Item;
                    //    if (myOneItem.PointGetMode == Item.PointMode.Dollar)
                    //    {
                    //        PointOnServer += oneOrderDet.Quantity * (myOneItem.PointGetAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointGetAmount);
                    //    }
                    //}

                    if (PointOnServer - (PointsPaid + paymentAmt) < 0)
                    {
                        MessageBox.Show("Point is not enough", "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (TotalPointableSales == 0)
                    {
                        MessageBox.Show("There is no item can be paid by points.\nPlease select another payment type",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (Math.Abs(PointsPaid + paymentAmt) > Math.Abs(TotalPointableSales))
                    {
                        MessageBox.Show("Customer pay too much\n" +
                            "Point-Redeemable : " + (TotalPointableSales - PointsPaid).ToString("N") + "\n" +
                            "Payment by point : " + paymentAmt, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        paymentAmt = TotalPointableSales - PointsPaid;
                    }

                    if ((pos.CalculateTotalPaid_ByPoints(out status) + paymentAmt) > PointOnServer)
                    {
                        if (MessageBox.Show("Customer only have " + (PointOnServer - PointsPaid).ToString("N") + " points\nDo you want to use it?"
                            , "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        { return; }

                        paymentAmt = (PointOnServer - PointsPaid);
                    }

                    //asking for signature after point validation
                    #region Signature
                    if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Signature.IsAvailableForAllPayment), false))
                    {
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
                    }
                    #endregion
                }
                #endregion

                if (paymentType == POSController.PAY_CHEQUE)
                {
                    //save cheque payment
                    if (!pos.AddChequeReceiptLinePayment(paymentAmt, ChequeNo, BankName, out status))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (paymentType == POSController.PAY_POINTS)
                {
                    SortedList<string, decimal> AddedPoints = new SortedList<string, decimal>();
                    foreach (OrderDet oneOrderDet in pos.myOrderDet)
                    {
                        Item myOneItem = oneOrderDet.Item;
                        if (myOneItem.PointRedeemMode == Item.PointMode.Dollar)
                        {
                            if (AddedPoints.ContainsKey(myOneItem.ItemNo))
                                AddedPoints[myOneItem.ItemNo] += oneOrderDet.Quantity.GetValueOrDefault(0) * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount);
                            else
                                AddedPoints.Add(myOneItem.ItemNo, oneOrderDet.Quantity.GetValueOrDefault(0) * (myOneItem.PointRedeemAmount == 0 ? myOneItem.RetailPrice : myOneItem.PointRedeemAmount));
                        }
                    }

                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Points.ChoosePointPackageForPayment), false))
                    {
                        DataTable dt = new DataTable();

                        if (!PowerPOS.Feature.Package.GetCurrentAmounts_Points(pos.GetMemberInfo().MembershipNo, DateTime.Now, out dt, out status))
                        {
                            MessageBox.Show("No Points Available");
                            return;
                        }
                        else
                        {
                            //show form

                            frmSelectOtherPayment fSelectOtherPayment = new frmSelectOtherPayment();
                            fSelectOtherPayment.pos = pos;
                            fSelectOtherPayment.CurrentAmounts = dt;
                            fSelectOtherPayment.isPartialPayment = true;
                            //fSelectOtherPayment.packageList = packageList;
                            fSelectOtherPayment.membershipNo = pos.GetMemberInfo().MembershipNo;
                            fSelectOtherPayment.ParentSyncPointsThread = ParentSyncPointsThread;
                            fSelectOtherPayment.ShowDialog();
                            if (fSelectOtherPayment.isSuccessful)
                            {
                                if (!PowerPOS.Feature.Package.PayReceiptsByPoints(pos.GetMemberInfo().MembershipNo, pos.GetOrderDate(), fSelectOtherPayment.PaymentTypeSelected, AddedPoints, paymentAmt, ref pos, out status))
                                    throw new Exception(status);
                            }

                        }
                    }
                    else
                    {
                        if (!PowerPOS.Feature.Package.BreakAmountIntoReceipts(pos.GetMemberInfo().MembershipNo, pos.GetOrderDate(), AddedPoints, paymentAmt, ref pos, out status))
                            throw new Exception("Error encountered - " + status);
                    }
                    //save cheque payment
                    //if (!PowerPOS.Feature.Package.BreakAmountIntoReceipts(pos.CurrentMember.MembershipNo, pos.GetOrderDate, pos.AddReceiptLinePayment_Points(paymentAmt, ChequeNo, BankName, out status))
                    //{
                    //    MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;


                }
                else
                {
                    //if payment type is cash and the amount is more than than the required amount            
                    //if (!pos.AddReceiptLinePayment(paymentAmt, paymentType, refno, 0, "", 0, out change, out status))
                    if (paymentType == POSController.PAY_CASH && !string.IsNullOrEmpty(foreignCurrencyCode))
                    {
                        paymentType += "-" + foreignCurrencyCode;
                    }
                    else
                    {
                        // Not paid using foreign cash, so just make sure these amounts are 0
                        foreignCurrencyAmount = 0;
                        foreignCurrencyRate = 0;
                    }
                    if (!pos.AddReceiptLinePayment(paymentAmt, paymentType, refno, foreignCurrencyRate, foreignCurrencyCode, foreignCurrencyAmount, out change, out status, !cbRounding.Checked))
                    {
                        MessageBox.Show("Error: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }


                lblChange.Text = change.ToString("N");
                BindGrid();
                if (change > 0)
                {
                    myDisplay.ClearScreen();
                    myDisplay.SendCommandToDisplay("Change: $" + lblChange.Text);
                }
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                MessageBox.Show(X.Message);
                BindGrid();
            }
        }

        #region -= Add Payment Selection =-
        private void btnCash_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_CASH);
            txtAmt.Text = "";
            //CashDrawer cdr = new CashDrawer();
            //cdr.OpenDrawer();
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
            if (enableNETSIntegration && ((Button)sender).Text.ToUpper().Contains("NETS"))
            {
                pnlNETSIntegration.Visible = true;
                pnlNETSIntegration.BringToFront();
                return;
            }

            AddPayment(((Button)sender).Text);
            txtAmt.Text = "";
        }

        private void btnMakePaymentNetsDetail_Click(object sender, EventArgs e)
        {
            string paymenttype = ((Button)sender).Text;
            if (paymenttype.ToUpper() == "NETS ATM CARD")
            {
                paymenttype = POSController.PAY_NETSATMCard;
            }
            else if (paymenttype.ToUpper() == "NETS FLASHPAY")
            {
                paymenttype = POSController.PAY_NETSFlashPay;
            }
            else if (paymenttype.ToUpper() == "NETS CASH CARD")
            {
                paymenttype = POSController.PAY_NETSCashCard;
            }
            else if (paymenttype.ToUpper() == "NETS QR")
            {
                paymenttype = POSController.PAY_NETSATMCard;
            }

            AddPayment(paymenttype);
            pnlNETSIntegration.SendToBack();
            pnlNETSIntegration.Visible = false;
            txtAmt.Text = "";
        }
        /*
        private void btnVoucher_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_VOUCHER);
        }

        private void btnVISA_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_VISA);
        }

        private void btnMaster_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_MASTER);
        }

        private void btnAMEX_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_AMEX);
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_INSTALLMENT);
        }
        */
        private void btnCheque_Click(object sender, EventArgs e)
        {
            frmBankChequeNo frm = new frmBankChequeNo();
            frm.ShowDialog();

            if (frm.IsSuccessful)
            {
                ChequeNo = frm.ChequeNo;
                BankName = frm.BankName;
                AddPayment(POSController.PAY_CHEQUE);
                frm.Dispose();
            }
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            if (IsEdit)
            {
                MessageBox.Show("Cannot edit payment by points", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pos.getAllRedeemableTotalAmount == 0)
            {
                MessageBox.Show("There is no item can be paid by points.\nPlease select another payment type",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            if (!isUsePoint) return;
            AddPayment(POSController.PAY_POINTS);
        }
        #endregion

        private bool IgnoreValidation = false;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //set 
            IgnoreValidation = true;
            IsSuccessful = false;
            this.Close();
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

        public bool isIntegrationPayment(string paymentType)
        {
            if (paymentType == netsATMCardPayment || paymentType == netsFlashPayPayment ||
                paymentType == netsCashCardPayment || paymentType == netsCreditCardPayment ||
                paymentType == bcaPayment || paymentType == cupPayment || paymentType == prepaidPurchasePayment
                || paymentType == netsQRPayment)
                return true;
            return false;
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

        private bool IsNeedPaymentIntegration(string paymentType)
        {
            bool needIntegrate = false;
            if ((enableNETSIntegration && (paymentType.ToUpper().Equals(netsATMCardPayment) || paymentType.ToUpper().Equals(netsFlashPayPayment) || paymentType.ToUpper().Equals(netsCashCardPayment) || paymentType.ToUpper().Equals(POSController.PAY_NETSQR)))
                || (enableUNIONPayIntegration && paymentType.ToUpper().Equals(unionPayPayment))
                || (enablePrepaidPurchaseIntegration && paymentType.ToUpper().Equals(prepaidPurchasePayment))
                || (enableCUPIntegration && paymentType.ToUpper().Equals(cupPayment))
                || (enableBCAIntegration && paymentType.ToUpper().Equals(bcaPayment))
                || (enableCitiBankTerminal && paymentType.ToUpper().Equals(citiBankPayment))
                || (enableNetsCreditCardTerminal && paymentType.ToUpper().Equals(netsCreditCardPayment))
                )
            {
                needIntegrate = true;
            }
            return needIntegrate;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Start the counter time
            frmOrderTaking.counterStartTime = DateTime.Now;
   
            //check amount completed....
            if (Decimal.Parse(lblShortFall.Text) > 0)
            {
                MessageBox.Show("There is still some amount that has not been completed. Please clear all amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool isRetry = false;
            bool isSuccessful = false;
            if (!IsEdit)
            {
                #region *) NETS Integration
                /*if (enableNETSIntegration)
                {
                    foreach (ReceiptDet rd in pos.recDet)
                    {
                        if (rd.PaymentType.ToUpper() == "NETSCARD" || rd.PaymentType.ToUpper() == "CASHCARD" || rd.PaymentType.ToUpper() == "NETSFLASHPAY")
                        {
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
                                frmLoading fl = new frmLoading("Please Insert " + rd.PaymentType);
                                fl.Show();
                                ShowAutoClosingMessageBox("Initializing ...", "Info");
                                string status1 = "";

                                if (rd.PaymentType.ToUpper() == "NETSCARD")
                                    nets.addNetsCardPayment(rd.Amount.ToString(),"", out status1);
                                else if (rd.PaymentType.ToUpper() == "CASHCARD")
                                    nets.addCashCardDebitPayment(rd.Amount.ToString(), "", out status1);
                                else if (rd.PaymentType.ToUpper() == "NETSFLASHPAY")
                                    nets.addFlashPayPayment(rd.Amount.ToString(), "", out status1);

                                fl.Close();
                                if (status1.StartsWith("ERROR"))
                                {
                                    frmNETSConfirmation frm = new frmNETSConfirmation(pos,rd.Amount);
                                    frm.PaymentType = rd.PaymentType;
                                    frm.Status = status1;
                                    frm.ShowDialog();
                                    if (frm.Result == NETConfirmation.Cancel)
                                    {
                                        isRetry = false;
                                        isSuccessful = false;
                                        return;
                                    }
                                }
                            } 

                        }
                        

                    }

                }*/

                if (IsNeedPaymentIntegration())
                {
                    bool finishedOneIntegration = false;
                    foreach (ReceiptDet rd in pos.recDet)
                    {
                        if (isIntegrationPayment(rd.PaymentType.ToUpper()))
                        {
                            bool isPaySuccess = false;
                            string payType = rd.PaymentType;
                            while (!isPaySuccess)
                            {
                                #region Call Payment Integration
                                string newPaymentName;
                                string status1 = "";
                                isPaySuccess = CallPaymentIntegration(payType, rd.Amount, finishedOneIntegration, out newPaymentName);
                                if (!isPaySuccess)  
                                {
                                    if (!finishedOneIntegration)
                                        return;
                                    else
                                    {
                                        frmSelectPaymentPopup frmSelPay = new frmSelectPaymentPopup();
                                        frmSelPay.ShowDialog();
                                        if (frmSelPay.isSuccessful)
                                        {
                                            rd.PaymentType = frmSelPay.PaymentTypeSelected;
                                            /*for (int i = 0; i < paymentTypes.Rows.Count; i++)
                                            {
                                                if ((paymentTypes.Rows[i]["PaymentType"] + "") == payType)
                                                {
                                                    paymentTypes.Rows[i]["PaymentType"] = frmSelPay.SelectedPaymentType;
                                                    if (frmSelPay.SelectedPaymentType.ToUpper().Equals("CASH"))
                                                        paymentTypes.Rows[i]["Amount"] = frmSelPay.SelectedAmount;
                                                    break;
                                                }
                                            }*/
                                            isPaySuccess = !IsNeedPaymentIntegration(frmSelPay.PaymentTypeSelected);
                                            if (isPaySuccess)
                                                break;
                                            else
                                                payType = frmSelPay.PaymentTypeSelected;
                                        }
                                    }
                                    //return;
                                }
                                else
                                {
                                    finishedOneIntegration = true;
                                    if (!String.IsNullOrEmpty(newPaymentName))
                                        rd.PaymentType = newPaymentName;
                                }
                            }

                            #endregion
                        }
                    }
                    
                }

                #endregion

                #region *) EZLInk Integration
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableEZLinkIntegration),false))
                {
                    foreach (ReceiptDet rd in pos.recDet)
                    {
                        if (rd.PaymentType.ToUpper() == AppSetting.GetSetting(AppSetting.SettingsName.Payment.EZLinkPaymentType))
                        {
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
                                frmLoading fl = new frmLoading("Please Insert " + rd.PaymentType);
                                fl.Show();
                                ShowAutoClosingMessageBox("Initializing ...", "Info");
                                string status1 = "";

                                nets.addEZLinkPayment(rd.Amount.ToString(), out status1);

                                fl.Close();
                                if (status1.StartsWith("ERROR"))
                                {
                                    frmNETSConfirmation frm = new frmNETSConfirmation(pos, rd.Amount);
                                    frm.PaymentType = rd.PaymentType;
                                    frm.Status = status1;
                                    frm.ShowDialog();
                                    if (frm.Result == NETConfirmation.Cancel)
                                    {
                                        isRetry = false;
                                        isSuccessful = false;
                                        return;
                                    }
                                }
                            }

                        }


                    }

                }
                #endregion

                #region *) Open Cash Drawer
                if (pos.hasPaymentType(POSController.PAY_CASH) && pos.GetTotalSales() != 0)
                {
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
                }
                #endregion

                string status;
                bool IsQtyInsufficient = false;
                bool IsPointAllocationSuccess = false;
                //pos.SetPaymentRemark(txtRemark.Text);
                if (!pos.ConfirmOrder(cbRounding.Checked, out IsPointAllocationSuccess, out status))
                {
                    MessageBox.Show("Error encountered when confirming order: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.writeLog(status);
                    return;
                }

                //if (!IsPointAllocationSuccess)
                //{ MessageBox.Show("Point is not updated!" + Environment.NewLine + status, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning); }

                if (!IsQtyInsufficient)
                    if (!pos.ExecuteStockOut(out status))
                    {
                        MessageBox.Show
                            ("Error while performing Stock Out: " + status, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }


                //set   
                IsSuccessful = true;


                //close dialogue
                this.Close();
            }
            else
            {
                string status = "";
                //check if it is return transaction make sure the count is right
                decimal totaltransaction = pos.CalculateTotalAmount(out status);
                decimal totalPaid = pos.CalculateTotalPaid(out status);

                if (totaltransaction < 0 && totalPaid >= 0)
                {
                    MessageBox.Show("Change Payment Type Failed. For refund transaction, total paid should be less than 0", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (totaltransaction < 0 && (totaltransaction != totalPaid))
                {
                    MessageBox.Show("Change Payment Type Failed. For refund transaction, total paid must tally with refund amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //clear all payments in existing                 
                //update to new payment types.....

                if (hasInstallment && pos.hasPaymentType(POSController.PAY_INSTALLMENT))
                {
                    //edit installment
                    Installment inst = new Installment("userfld1", pos.GetSavedRefNo());
                    InstallmentDetailCollection instColl = inst.InstallmentDetailRecords();
                    ReceiptDetCollection myRcptDet = pos.recDet;
                    decimal rcpDetInstAmount = 0;
                    for (int i = 0; i < myRcptDet.Count; i++)
                    {
                        if (myRcptDet[i].PaymentType.ToUpper() != POSController.PAY_INSTALLMENT) continue;
                        rcpDetInstAmount = myRcptDet[i].Amount;
                    }

                    decimal? totalInstallmentPaid=0;
                    foreach (var instDetail in instColl)
                    {
                        if (instDetail.InstallmentAmount < 0)
                            totalInstallmentPaid = totalInstallmentPaid + (instDetail.InstallmentAmount * -1);
                        else
                            instDetail.InstallmentAmount = rcpDetInstAmount;
                    }

                    if (rcpDetInstAmount < totalInstallmentPaid)
                    {
                        MessageBox.Show("Cannot change payment type, Installment Paid greater than new Installment Amount", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //else {
                    //    decimal lastTotalAmount = inst.TotalAmount;
                    //    inst.CurrentBalance = rcpDetInstAmount - totalInstallmentPaid;
                    //    inst.TotalAmount = rcpDetInstAmount;
                    //    foreach (var instDet in instColl)
                    //    {
                    //        instDet.OutstandingAmount = instDet.OutstandingAmount - (lastTotalAmount - rcpDetInstAmount);                           
                    //    }
                    //}                  
                    //InstallmentController.UpdateInstallment(inst, instColl);
                }
               


                pos.SavePaymentTypes();

                if (hasInstallment &&
                    !pos.hasPaymentType(POSController.PAY_INSTALLMENT))
                {
                    Installment.Delete(Installment.Columns.OrderHdrId, pos.GetSavedRefNo().Substring(2));
                }

                //If installment changed
                if (hasInstallment || (!hasInstallment && pos.hasPaymentType(POSController.PAY_INSTALLMENT)))
                {
                    //Installment inst = new Installment("userfld1", pos.GetSavedRefNo());
                    //InstallmentDetailCollection instDetail = new InstallmentDetail("InstallmentRefNo", inst.InstallmentRefNo);

                    //InstallmentController.UpdateInstallment(
                    string errMsg = "";
                    InstallmentController.UpdateInstallmentByOrderHdr(pos.GetSavedRefNo().Substring(2), out errMsg);


                    if (pos.hasPreOrder())
                    {
                        // reset deposit amount so will be assigned again
                        if (PointOfSaleInfo.IntegrateWithInventory)
                        {
                            if(!PreOrderController.ResetDepositAmount(pos.myOrderHdr.OrderHdrID, out status))
                                MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            if (!PreOrderController.AssignAutoDeposit(pos.myOrderHdr.OrderHdrID, out status))
                                MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        else {

                            SyncClientController.Load_WS_URL();
                            PowerPOSLib.PowerPOSSync.Synchronization ws = new PowerPOSLib.PowerPOSSync.Synchronization();
                            ws.Timeout = 100000;
                            ws.Url = SyncClientController.WS_URL;

                            if (!ws.ResetDepositAmount(pos.myOrderHdr.OrderHdrID, out status))
                                MessageBox.Show(status,"", MessageBoxButtons.OK,MessageBoxIcon.Error);

                            //if (!ws.AssignAutoDeposit(pos.myOrderHdr.OrderHdrID, out status))
                            //    MessageBox.Show(status, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                

                //prompt installment
                /*
                if (pos.hasPaymentType(POSController.PAY_INSTALLMENT))
                {
                    frmCreateInstallment f = new frmCreateInstallment();
                    f.pos = pos;
                    f.ShowDialog();
                }*/
                IsSuccessful = true;
                this.Close();
            }
        }

        private void dgvPayment_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                string PaymentType = pos.getReceiptPaymentType(dgvPayment[3, e.RowIndex].Value.ToString());
                if (string.IsNullOrEmpty(PaymentType))
                {
                    MessageBox.Show("Error in deleting payment. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (PaymentType == POSController.PAY_POINTS || PaymentType == POSController.PAY_PACKAGE)
                {
                    if (IsEdit)
                    {
                        MessageBox.Show("Cannot delete payment by points", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //delete from receipt line...                
                if (!pos.DeleteReceiptLinePayment(dgvPayment[3, e.RowIndex].Value.ToString()))
                {
                    MessageBox.Show("Error in deleting payment. Please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                BindGrid();
            }
        }

        private void txtAmt_Validating(object sender, CancelEventArgs e)
        {
            errorProvider1.SetError(txtAmt, "");
            decimal result;
            // if OrderAmount is negative, then txtAmt can be negative.
            if (!IgnoreValidation && !CommonUILib.ValidateTextBoxAsUnsignedDecimal(txtAmt, out result) && OrderAmount >= 0)
            {
                errorProvider1.SetError(txtAmt, "Please enter valid amount");
                e.Cancel = true;
            }

            decimal.TryParse(txtAmt.Text, out result);
            if (!IgnoreValidation && (Math.Sign(result) != Math.Sign(OrderAmount)))
            {
                errorProvider1.SetError(txtAmt, "Please enter valid amount");
                e.Cancel = true;
            }
        }

        private void txtAmt_Click(object sender, EventArgs e)
        {
            //prompt keypad
            frmKeypad f = new frmKeypad();
            f.IsInteger = false;
            f.initialValue = ((TextBox)sender).Text;
            f.textMessage = "O/S:" + lblShortFall.Text;
            f.ShowDialog();

            ((TextBox)sender).Text = f.value.ToString();

        }

        private void cbRounding_CheckedChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnInstallment_Click(object sender, EventArgs e)
        {
            AddPayment("INSTALLMENT");
            txtAmt.Text = "";
        }

        private void btnPWF_Click(object sender, EventArgs e)
        {
            AddPayment(POSController.PAY_PWF);
            txtAmt.Text = "";
        }

        private void btnPAMedifund_Click(object sender, EventArgs e)
        {
            pos.FundingMethod = POSController.PAY_PAMEDIFUND;
            Funding_Calculation();
        }

        private void btnSMF_Click(object sender, EventArgs e)
        {
            pos.FundingMethod = POSController.PAY_SMF;
            Funding_Calculation();
        }

        private void btnNETSBack_Click(object sender, EventArgs e)
        {
            pnlNETSIntegration.Visible = false;
        }

        private bool CallPaymentIntegration(string paymentName, decimal amount, bool needToChangePayment, out string newPaymentName)
        {
            bool isSuccess = false;
            newPaymentName = paymentName;

            //frmPaymentIntegration frm = new frmPaymentIntegration(paymentName, amount, pos.myOrderHdr.OrderHdrID.Substring(6, 8), needToChangePayment);

            // 8 feb 2018 : changed to pass the full OrderHdrID instead of only last 8 digits
            frmPaymentIntegration frm = new frmPaymentIntegration(paymentName, amount, pos.myOrderHdr.OrderHdrID, needToChangePayment);
            frm.pos = pos;
            frm.ShowDialog();

            isSuccess = frm.DialogResult != DialogResult.Cancel;

            if (frm.DialogResult == DialogResult.Yes || frm.DialogResult == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(frm.PaymentTypeResult))
                    newPaymentName = frm.PaymentTypeResult;
            }

            return isSuccess;

        }
    }
}