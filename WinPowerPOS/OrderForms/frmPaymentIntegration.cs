using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerPOS;
using PowerPOSLib.Controller;
using System.Threading;
using WinPowerPOS.OrderForms;
using CitiLib;

namespace WinPowerPOS.OrderForms
{
    public partial class frmPaymentIntegration : Form
    {
        private string _paymentType = "";
        private decimal _amount = 0;
        private string _orderRefNo = "";  // 8 feb 2018 : changed to pass the full OrderHdrID instead of only last 8 digits

        public string NewPaymentType = "";
        public string InvNo = "";

        public POSController pos;
        public string responseInfo;

        bool enableCitiBankTerminal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCitiBankIntegration), false);
        string citiBankPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.CitiBankPayment) + "").Trim().ToUpper();

        //string netsPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSPayment) + "").Trim().ToUpper();
        string unionPayPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.UNIONPayPayment) + "").Trim().ToUpper();
        string bcaPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.BCAPayment) + "").Trim().ToUpper();
        string cupPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.CUPPayment) + "").Trim().ToUpper();
        string prepaidPurchasePayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PrepaidPurchasePayment) + "").Trim().ToUpper();

        string creditCardPayment = (AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_Grouped) + "").Trim().ToUpper();
        string netsATMCardPayment = POSController.PAY_NETSATMCard.ToUpper();
        string netsFlashPayPayment = POSController.PAY_NETSFlashPay.ToUpper();
        string netsCashCardPayment = POSController.PAY_NETSCashCard.ToUpper();
        bool netsCashCardWithService = false;

        bool _showRefNo = false;
        int _netsThreadSleep = 0;
        string _portName = "";

        public decimal cashbackAmount = 0;

        public frmPaymentIntegration(string paymentType, decimal amount, string orderRefNo, bool isShownChangePayment)
        {
            InitializeComponent();

            NewPaymentType = "";
            InvNo = "";
            
            _paymentType = paymentType;
            _amount = amount;
            _orderRefNo = orderRefNo;

            //bool showRefNo = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.ShowOrderRefNoOnECRReceipt), false);
            //if (showRefNo)
                _orderRefNo = "  " + orderRefNo + "  ";
            //else
            //    _orderRefNo = "        ";
            _netsThreadSleep = 1;
            _netsThreadSleep = _netsThreadSleep * 1000;

            lblPaymentType.Text = paymentType;
            lblPaymentAmount.Text = amount.ToString("N");

            if (_paymentType.ToLower().Equals(citiBankPayment.ToLower()))
                _portName = AppSetting.GetSetting(AppSetting.SettingsName.Payment.CitiBankCOMPort);
            else
                _portName = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NetsCOMPort);
            _portName = (_portName + "").Trim();
            if (string.IsNullOrEmpty(_portName))
            {
                DataSet ds3 = new DataSet();
                ds3.ReadXml(AppDomain.CurrentDomain.BaseDirectory + "\\NetsComPort.xml");
                if (ds3.Tables[0].Rows.Count > 0)
                    _portName = ds3.Tables[0].Rows[0].ItemArray[0].ToString().Trim();
            }
            btnCancel.Visible = !isShownChangePayment;
            btnChangePaymentType.Visible = isShownChangePayment;
        }

        private static bool _isPaymentSuccess = false;
        private static bool _isManualAllowed = false;
        private static string _status = "";
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _isManualAllowed = false;

                if (_paymentType.ToUpper().Trim().Equals(citiBankPayment.Trim()) && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.EnableCitiBankIntegration), false))
                {
                    string invNo = "";
                    string newPaymentType = "";
                    CitiLib.CitiController citi = new CitiLib.CitiController();
                    if (citi.setupSerialPort(_portName))
                    {
                        citi.addCitibankPayment(_amount.ToString("N2"), pos.myOrderHdr.OrderHdrID, out _status, out newPaymentType, out invNo);
                        PaymentTypeResult = newPaymentType;
                    }
                    else
                    {
                        throw new Exception("ERROR CONNECTION. Connect to Citi Port Failed, Please check the port name");
                    }
                    if (_netsThreadSleep > 0)
                        Thread.Sleep(_netsThreadSleep);

                    if (_status.StartsWith("ERROR"))
                    {
                        throw new Exception(_status);
                        return;
                    }

                    NewPaymentType = newPaymentType;
                    InvNo = invNo;
                    _isPaymentSuccess = true;
                }
                else
                {
                    NetsController nets = new NetsController();
                    //CitiController citi = new CitiController();
                    if (nets.setupSerialPort(_portName))
                    {
                        string cashIssuerID = "";
                        string invNo = "";

                        Logger.writeLog(string.Format(">> NETS Integration {0} : ${1}", _paymentType, _amount.ToString("N2")));
                        if (_paymentType.ToUpper().Trim().Equals(POSController.PAY_NETSATMCard.ToUpper().Trim()) && cashbackAmount <= 0)
                            nets.addNetsCardPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(POSController.PAY_NETSATMCard.ToUpper().Trim()) && cashbackAmount > 0)
                            nets.addNetsCashBackPayment((_amount - cashbackAmount).ToString("N2"), cashbackAmount.ToString("N2"), _orderRefNo, out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(POSController.PAY_NETSFlashPay.ToUpper().Trim()))
                            nets.addFlashPayPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(POSController.PAY_NETSQR.ToUpper().Trim()))
                        {
                            nets.addNetsQRPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                            PaymentTypeResult = POSController.PAY_NETSATMCard.Trim();
                        }
                        else if (_paymentType.ToUpper().Trim().Equals(POSController.PAY_NETSCashCard.ToUpper().Trim()))
                        {
                            if (netsCashCardWithService)
                                nets.addCashCardWithServiceFeeDebitPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                            else
                                nets.addCashCardDebitPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                        }
                        else if (_paymentType.ToUpper().Trim().Equals(unionPayPayment.Trim()))
                            nets.addUnionPayDebitPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(bcaPayment.Trim()))
                            nets.addBCAPurchaseDebitPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(cupPayment.Trim()))
                            nets.addCUPDebitPayment(_amount.ToString("N2"), out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(prepaidPurchasePayment.Trim()))
                            nets.addPrepaidPayment(_amount.ToString("N2"), _orderRefNo, out _status);
                        else if (_paymentType.ToUpper().Trim().Equals(creditCardPayment.Trim()))
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseUOBCreditCardIntegration), false))
                            {
                                nets.addCreditCardCardPayment_UOB(_amount.ToString("N2"), _orderRefNo, out _status, out cashIssuerID);
                                PaymentTypeResult = cashIssuerID;
                            }
                            else
                                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Payment.UseUOBCreditCardPassThrough), false))
                                {
                                    nets.addCreditCardCardPayment_UOBPassThrough(_amount.ToString("N2"), _orderRefNo, out _status, out cashIssuerID);
                                    PaymentTypeResult = cashIssuerID;
                                }
                                else
                            {
                                nets.addCreditCardCardPayment(_amount.ToString("N2"), _orderRefNo, out _status, out cashIssuerID);
                                PaymentTypeResult = cashIssuerID;
                            }
                        }
                        /*else if (_paymentType.ToUpper().Trim().Equals(citiBankPayment.Trim()))
                        {
                            citi.addCitibankPayment(_amount.ToString("N2"), _orderRefNoLast8Digits, out _status, out cashIssuerID, out invNo);
                            PaymentTypeResult = cashIssuerID;
                        }*/

                        //Get Response Info from NETS Controller
                        responseInfo = nets.responseInfo;

                        if (_netsThreadSleep > 0)
                            Thread.Sleep(_netsThreadSleep);

                        if (_status.StartsWith("ERROR"))
                            throw new Exception(_status);

                    }
                    else
                    {
                        throw new Exception("ERROR CONNECTION. Connect to Nets Port Failed, Please check the port name");
                    }
                    _isPaymentSuccess = true;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                _isPaymentSuccess = false;
                if (_status.StartsWith("ERROR CONNECTION"))
                    _isManualAllowed = true;

                _status = ex.Message;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbLoading.Visible = false;
            if (_isPaymentSuccess)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblStatus.Text = _status;
                btnManual.Enabled = _isManualAllowed;
                lblError.Text = _status;
                ShowErrorPanel();
                btnChangePaymentType.Enabled = true;
                btnRetry.Enabled = true;
                btnCancel.Enabled = true;
            }
        }

        private void ShowErrorPanel()
        {
            lblError.Visible = true;
            lblError.BringToFront();
        }

        private void HideErrorPanel()
        {
            lblError.SendToBack();
            lblError.Visible = false;
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            HideErrorPanel();
            pbLoading.Visible = true;
            lblStatus.Text = string.Format("Please insert {0} card", _paymentType);
            btnManual.Enabled = false;
            btnRetry.Enabled = false;
            btnCancel.Enabled = false;
            btnChangePaymentType.Enabled = false;
            worker.RunWorkerAsync();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmPaymentIntegration_Shown(object sender, EventArgs e)
        {
            HideErrorPanel();
            pbLoading.Visible = true;
            lblStatus.Text = string.Format("Please insert {0} card", _paymentType);
            btnManual.Enabled = false;
            btnRetry.Enabled = false;
            btnCancel.Enabled = false;
            btnChangePaymentType.Enabled = false;
            worker.RunWorkerAsync();
        }
        public decimal Amount = 0;
        public string PaymentTypeResult = "";
        private void btnManual_Click(object sender, EventArgs e)
        {
            frmNETSManualInput frm = new frmNETSManualInput(pos, _amount, _paymentType);
            frm.ShowDialog();
            if (frm.IsSuccess)
            {
                PaymentTypeResult = frm.PaymentTypeResult;
                _amount = frm.Amount;
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void btnChangePaymentType_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
