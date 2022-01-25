using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS.Nets;
using System.IO.Ports;
using PowerPOS;
using System.Threading;
//using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PowerPOSLib.Controller
{
   


    public class NetsController
    {
        static int DEFAULT_RESPONSE_LENGTH = 2048;
        delegate void SetTextCallback(string text);

        SerialPort _serialPort;
        string[] ports;

        byte[] response;
        byte[] readBuffer = new byte[DEFAULT_RESPONSE_LENGTH];
        int respIdx = 0;
        bool finishedReading = true;
        DateTime lastSendTime;
        bool machineStatus = false;

        public string responseInfo; // in JSON format

        public string ecrType;

        #region Events
        /*public class NetsLogArgs : EventArgs
        {
            private string log;

            public NetsLogArgs(string _log)
            {
                log = _log;
            }
            public string Log
            {
                get
                {
                    return log;
                }
            }
        }*/
        public delegate void NETSLogHandler(string log);

        public event NETSLogHandler NETSLog;

        protected virtual void OnNETSLog(string e)
        {
            if (NETSLog != null)
            {
                NETSLog(e);//Raise the event
            }
        }

        #endregion

        public NetsController()
        {
            _serialPort = new SerialPort();
            ecrType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.NETSVersion);
        }

        public void setECRType(string _ECRType)
        {
            ecrType = _ECRType;
        }


        //change
        #region payment controller
        public void requestTerminalStatus(out string status, out bool mStatus)
        {
            mStatus = false;
            
            status = "";
            try
            {
                int res = sendDataToSerialPortECR3(NetsAPI.NETS3_CHECK_STATUS, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseCode = "NA";
                    string responseText = "";
                    if (NetsAPI.REQUEST_TERMINAL_STATUS.GetResponseInfo().ContainsKey("Response Text"))
                        responseText = NetsAPI.REQUEST_TERMINAL_STATUS.GetResponseInfo()["Response Text"];
                    if (NetsAPI.REQUEST_TERMINAL_STATUS.GetResponseInfo().ContainsKey("Response Code"))
                        responseCode = NetsAPI.REQUEST_TERMINAL_STATUS.GetResponseInfo()["Response Code"];
                    if (responseText != "TIMEOUT")
                    {
                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                            if (machineStatus)
                            {
                                machineStatus = false;
                                //trigger event the 
                            }
                        }
                        else
                        {
                            if (!machineStatus)
                            {
                                machineStatus = true;
                                //trigger event status changed 
                            }
                        }
                    }
                    else
                    {
                        if (machineStatus)
                        {
                            machineStatus = false;
                            //trigger event the 
                        }
                    }
                    mStatus = machineStatus;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
                machineStatus = false;
                mStatus = machineStatus;
            }

        }

        public void addPayment(string type, string amount, string refno, out string status)
        {
            status = "";
            string responseCode = "";
            int res = 0;
            try
            {
                if (type == "Nets Flash Pay")
                {
                    addFlashPayPayment(amount, refno, out status);
                }
                else
                    if (type == "Nets Card")
                    {
                        addNetsCardPayment(amount, refno, out status);
                    }
                    else
                        if (type == "Cash Card")
                        {
                            addCashCardDebitPayment(amount, refno, out status);
                        }
                        else
                            if (type == "Cash Card With Service Fee")
                            {
                                addCashCardWithServiceFeeDebitPayment(amount, refno, out status);
                            }
                            else
                                if (type == "BCA Purchase")
                                {
                                    addBCAPurchaseDebitPayment(amount, refno, out status);
                                }
                                else
                                    if (type == "UNION Pay")
                                    {
                                        addUnionPayDebitPayment(amount, refno, out status);
                                    }
                                    else
                                        if (type == "CUP")
                                        {
                                            addCUPDebitPayment(amount, out status);
                                        }
                                        else
                                            if (type == "Prepaid")
                                            {
                                                addCUPDebitPayment(amount, out status);
                                            }
                                            else
                                                if (type == "QR")
                                                {
                                                    addNetsQRPayment(amount, refno, out status);
                                                }
            }
            catch (Exception ex) { status = "ERROR. " + ex.Message.ToString(); Logger.writeLog(ex.Message.ToString()); }

        }

        public void addCreditCardCardPayment(string amount, string refno, out string status, out string newPaymentType)
        {
            status = "";
            newPaymentType = "";
            string status1;
            try
            {
                NetsAPI.CREDITCARD_PURCHASE.setDebitAmount(amount);
                NetsAPI.CREDITCARD_PURCHASE.setRefNo(refno);//change
                NetsAPI.CREDITCARD_PURCHASE.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.CREDITCARD_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = "NOT AVAILABLE";
                    if (NetsAPI.CREDITCARD_PURCHASE.GetResponseInfo().ContainsKey("Response Text"))
                        responseText = NetsAPI.CREDITCARD_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = "NA";
                    if (NetsAPI.CREDITCARD_PURCHASE.GetResponseInfo().ContainsKey("Response Code"))
                        responseCode = NetsAPI.CREDITCARD_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                            //get the card issuer name from the user
                            string tmpPaymentType = "";
                            if (NetsAPI.CREDITCARD_PURCHASE.GetResponseInfo().ContainsKey("Card Issuer Name"))
                                tmpPaymentType = NetsAPI.CREDITCARD_PURCHASE.GetResponseInfo()["Card Issuer Name"];
                            if (tmpPaymentType.ToUpper().Contains("VISA"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
                            }
                            //else if (tmpPaymentType.ToUpper().Contains("MASTERCARD") || tmpPaymentType.ToUpper().Contains("DEBIT MAST") || tmpPaymentType.ToUpper().Contains("MASTER")) 
                            else if (tmpPaymentType.ToUpper().Contains("MAST")) 
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER);
                            }
                            else if (tmpPaymentType.ToUpper().Contains("AMERICAN"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX);
                            }
                            else
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
                            }

                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR CONNECTION. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.CREDITCARD_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.CREDITCARD_PURCHASE.reset();
            }

        }

        public void addCreditCardCardPayment_UOB(string amount, string refno, out string status, out string newPaymentType)
        {
            status = "";
            newPaymentType = "";
            string status1;
            try
            {
                NetsAPI.UOBCREDITCARD_PURCHASE.setDebitAmount(amount);
                NetsAPI.UOBCREDITCARD_PURCHASE.setRefNo(refno);//change
                string uniqueNo = AppSetting.GetSetting(AppSetting.SettingsName.Payment.LastUniqueNo);
                int uniqueNoInt = 0;
                if (string.IsNullOrEmpty(uniqueNo) || !int.TryParse(uniqueNo, out uniqueNoInt))
                {
                    uniqueNoInt = 0;
                }

                NetsAPI.UOBCREDITCARD_PURCHASE.setUniqueNo((uniqueNoInt).ToString());
                //AppSetting.SetSetting(AppSetting.SettingsName.Payment.LastUniqueNo, (uniqueNoInt + 1).ToString());
                NetsAPI.UOBCREDITCARD_PURCHASE.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.UOBCREDITCARD_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = "NOT AVAILABLE";
                    if (NetsAPI.UOBCREDITCARD_PURCHASE.GetResponseInfo().ContainsKey("Response Text"))
                        responseText = NetsAPI.UOBCREDITCARD_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = "NA";
                    if (NetsAPI.UOBCREDITCARD_PURCHASE.GetResponseInfo().ContainsKey("Response Code"))
                        responseCode = NetsAPI.UOBCREDITCARD_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                            //get the card issuer name from the user
                            string tmpPaymentType = "";
                            if (NetsAPI.UOBCREDITCARD_PURCHASE.GetResponseInfo().ContainsKey("Card Issuer Name"))
                                tmpPaymentType = NetsAPI.UOBCREDITCARD_PURCHASE.GetResponseInfo()["Card Issuer Name"];
                            if (tmpPaymentType.ToUpper().Contains("VISA"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
                            }
                            //else if (tmpPaymentType.ToUpper().Contains("MASTERCARD") || tmpPaymentType.ToUpper().Contains("DEBIT MAST") || tmpPaymentType.ToUpper().Contains("MASTER")) 
                            else if (tmpPaymentType.ToUpper().Contains("MAST"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER);
                            }
                            else if (tmpPaymentType.ToUpper().Contains("AMERICAN"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX);
                            }
                            else
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA);
                            }

                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR CONNECTION. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.UOBCREDITCARD_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.UOBCREDITCARD_PURCHASE.reset();
            }

        }

        public void addCreditCardCardPayment_UOBPassThrough(string amount, string refno, out string status, out string newPaymentType)
        {
            status = "";
            newPaymentType = "";
            string status1;
            try
            {
                NetsAPI.UOBCREDITCARD_PASSTHROUGH.setDebitAmount(amount);
                NetsAPI.UOBCREDITCARD_PASSTHROUGH.setRefNo(refno);//change
                int res = sendDataToSerialPortPassThrough(NetsAPI.UOBCREDITCARD_PASSTHROUGH, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = "NOT AVAILABLE";
                    if (NetsAPI.UOBCREDITCARD_PASSTHROUGH.GetResponseInfo().ContainsKey("Response Text"))
                        responseText = NetsAPI.UOBCREDITCARD_PASSTHROUGH.GetResponseInfo()["Response Text"];
                    string responseCode = "NA";
                    if (NetsAPI.UOBCREDITCARD_PASSTHROUGH.GetResponseInfo().ContainsKey("Response Code"))
                        responseCode = NetsAPI.UOBCREDITCARD_PASSTHROUGH.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                            //get the card issuer name from the user
                            string tmpPaymentType = "";
                            if (NetsAPI.UOBCREDITCARD_PASSTHROUGH.GetResponseInfo().ContainsKey("Card Issuer Name"))
                                tmpPaymentType = NetsAPI.UOBCREDITCARD_PASSTHROUGH.GetResponseInfo()["Card Issuer Name"];
                            if (tmpPaymentType.ToUpper().Contains("VISA"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA); 
                            }
                            //else if (tmpPaymentType.ToUpper().Contains("MASTERCARD") || tmpPaymentType.ToUpper().Contains("DEBIT MAST") || tmpPaymentType.ToUpper().Contains("MASTER")) 
                            else if (tmpPaymentType.ToUpper().Contains("MAST"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER); 
                            }
                            else if (tmpPaymentType.ToUpper().Contains("AME"))
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX); 
                            }
                            else
                            {
                                newPaymentType = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA); 
                            }

                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR CONNECTION. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.UOBCREDITCARD_PASSTHROUGH.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.UOBCREDITCARD_PASSTHROUGH.reset();
            }

        }

        public void addFlashPayPayment(string amount, string refno, out string status)
        {

            status = "";
            string status1 = "";

            try
            {
                NetsAPI.CONTACTLESS_DEBIT.setDebitAmount(amount);
                NetsAPI.CONTACTLESS_DEBIT.setRefNo(refno);
                NetsAPI.CONTACTLESS_DEBIT.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.CONTACTLESS_DEBIT, true);

                //Check if data is sent successfully, if not, perform error handling
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    //Data is sent successfully, therefore response code is received, check response code
                    string responseText = NetsAPI.CONTACTLESS_DEBIT.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.CONTACTLESS_DEBIT.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT REACHED, PLEASE TRY AGAIN";
                    }
                }
                else
                {
                    Thread.Sleep(1000); // Wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR CONNECTION. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.CONTACTLESS_DEBIT.reset(); //after processed, reset the packet for next use
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.CONTACTLESS_DEBIT.reset();
            }
        }

        public void addNetsCardPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.NETS_PURCHASE.setDebitAmount(amount);
                NetsAPI.NETS_PURCHASE.setRefNo(refno);//change
                NetsAPI.NETS_PURCHASE.setTransactionType(NETSConstants.MessageHeader.NETSPurchaseTransactionType.NetsPurchase);
                NetsAPI.NETS_PURCHASE.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.NETS_PURCHASE, true);
                
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.NETS_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR CONNECTION. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.NETS_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.NETS_PURCHASE.reset();
            }

        }

        public void addNetsCardECR3Payment(string amount, string refno, string sequenceNo, out string status, out string newPaymentType)
        {
            status = "";
            newPaymentType = "";
            string status1;
            try
            {
                NetsAPI.NETS3_PURCHASE.setDebitAmount(amount);
                NetsAPI.NETS3_PURCHASE.setRefNo(refno);
                NetsAPI.NETS3_PURCHASE.setSequenceNo(sequenceNo);//change
                int res = sendDataToSerialPortECR3(NetsAPI.NETS3_PURCHASE, true);

                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.NETS3_PURCHASE.GetResponseInfoValue("Response Text");
                    string responseCode = NetsAPI.NETS3_PURCHASE.GetResponseInfoValue("Response Code");
                    OnNETSLog("Response Code : " + responseCode);
                    if (responseText != "TIMEOUT")
                    {
                        if (responseCode != NETSConstants.MessageHeader.ECR3MEssageStatus.SUCCESS)
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            newPaymentType = NetsAPI.NETS3_PURCHASE.GetResponseInfoValue("Payment Type");
                            status = "SUCCESS!";
                        }
                        
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR CONNECTION. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.NETS_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.NETS3_PURCHASE.reset();
            }

        }

        public void addNetsQRPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.NETS_PURCHASE.setDebitAmount(amount);
                NetsAPI.NETS_PURCHASE.setRefNo(refno);//change
                NetsAPI.NETS_PURCHASE.setTransactionType(NETSConstants.MessageHeader.NETSPurchaseTransactionType.QRPurchase);
                NetsAPI.NETS_PURCHASE.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.NETS_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.NETS_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.NETS_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.NETS_PURCHASE.reset();
            }

        }

        public void addNetsCashBackPayment(string amount, string cashbackAmount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.NETS_PURCHASECASHBACK.setDebitAmount(amount);
                NetsAPI.NETS_PURCHASECASHBACK.setRefNo(refno);//change
                NetsAPI.NETS_PURCHASECASHBACK.setTransactionType(NETSConstants.MessageHeader.NETSPurchaseTransactionType.NetsPurchaseWithCashback);
                //NetsAPI.NETS_PURCHASECASHBACK.setECRType(ecrType);
                NetsAPI.NETS_PURCHASECASHBACK.setCashbackAmount(cashbackAmount);
                int res = sendDataToSerialPort(NetsAPI.NETS_PURCHASECASHBACK, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.NETS_PURCHASECASHBACK.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.NETS_PURCHASECASHBACK.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.NETS_PURCHASECASHBACK.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.NETS_PURCHASECASHBACK.reset();
            }

        }

        /*public void addCreditCardCardPayment(string amount, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.NETS_PURCHASE.setDebitAmount(amount);
                int res = sendDataToSerialPort(NetsAPI.CREDITCARD_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.NETS_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.NETS_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.NETS_PURCHASE.reset();
            }

        }*/

        public void addCashCardDebitPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.CASHCARD_PURCHASE.setDebitAmount(amount);
                NetsAPI.CASHCARD_PURCHASE.setRefNo(refno);
                int res = sendDataToSerialPort(NetsAPI.CASHCARD_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.CASHCARD_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.CASHCARD_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }
                bool res1 = this.closePort(out status1);

                NetsAPI.CASHCARD_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.CASHCARD_PURCHASE.reset();
            }

        }

        //New NETS Functions
        public void addCashCardWithServiceFeeDebitPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE.setDebitAmount(amount);
                NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE.setRefNo(refno);
                int res = sendDataToSerialPort(NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }
                bool res1 = this.closePort(out status1);

                NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.CASHCARD_PURCHASE_SERVICE_FEE.reset();
            }

        }

        public void addBCAPurchaseDebitPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.BCA_PURCHASE.setDebitAmount(amount);
                NetsAPI.BCA_PURCHASE.setRefNo(refno);//change
                int res = sendDataToSerialPort(NetsAPI.BCA_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.BCA_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.BCA_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }
                bool res1 = this.closePort(out status1);

                NetsAPI.BCA_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.BCA_PURCHASE.reset();
            }

        }

        public void addUnionPayDebitPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.UNION_PAY.setDebitAmount(amount);
                NetsAPI.UNION_PAY.setRefNo(refno);//change
                int res = sendDataToSerialPort(NetsAPI.UNION_PAY, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.UNION_PAY.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.UNION_PAY.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }
                bool res1 = this.closePort(out status1);

                NetsAPI.UNION_PAY.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.UNION_PAY.reset();
            }

        }

        public void addCUPDebitPayment(string amount, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.CUP_PURCHASE.setDebitAmount(amount);
                int res = sendDataToSerialPort(NetsAPI.CUP_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.CUP_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.CUP_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }
                bool res1 = this.closePort(out status1);

                NetsAPI.CUP_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.CUP_PURCHASE.reset();
            }

        }

        public void addPrepaidPayment(string amount, string refno, out string status)
        {

            status = "";
            string status1;
            try
            {
                sendDataToSerialPort(NetsAPI.NETSSwitchPrepaidPacket, true);
                NetsAPI.PREPAID_PURCHASE.setDebitAmount(amount);
                Logger.writeLog("Start Sending Prepaid");
                Thread.Sleep(4000);
                int res = sendDataToSerialPort(NetsAPI.PREPAID_PURCHASE, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = "";
                    string responseCode = NetsAPI.PREPAID_PURCHASE.GetResponseInfo()["Response Code"];
                    //Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "OK"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL, PLEASE TRY AGAIN";
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before error message prompt
                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }
                Thread.Sleep(4000);
                sendDataToSerialPort(NetsAPI.NETSSwitchPrepaidBackPacket, true);
                bool res1 = this.closePort(out status1);

                NetsAPI.PREPAID_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.PREPAID_PURCHASE.reset();
            }

            /*status = "";
            string status1;
            try
            {   
                //Triggering Nets machine to prepaid mode
                sendDataToSerialPort(NetsAPI.NETSSwitchPrepaidPacket,true);
                
                //POS wait for the Nets Machine to swithch to prepaid mode
                Thread.Sleep(15000);

                string stx = "02";
                string msg = "";
                string fullString = "";
                bool exitLoop = false;
                int countLoop = 1;
                string debitAmount = Convert.ToInt32(double.Parse(amount) * 100).ToString().PadLeft(8, '0');

                if (refno.Substring(2, 1).Equals(" "))
                    refno = "            ";

                string[] myArray = new string[refno.Length];
                byte[] toBytes = new byte[1];

                for (int i = 0; i < refno.Length; i++)
                {
                    myArray[i] = refno[i].ToString();
                }

                //set cashback amount in message data, set all to "0"
                for (int i = 0; i < refno.Length; i++)
                {
                    toBytes = Encoding.ASCII.GetBytes(myArray[i]);
                    fullString += toBytes[0].ToString();
                }

                msg = "003530303030303030303030303037303030301c30323132" + debitAmount + "52333132" + fullString + "1c03";

                string lrc = GetLRCChecksum(msg);
                string allMsg = stx + msg + lrc;
                allMsg = Regex.Replace(allMsg, ".{2}", "$0/");
                allMsg = allMsg.Remove(allMsg.Length - 1);
                byte[] packet = allMsg.Split('/').Select(s => Convert.ToByte(s, 16)).ToArray();

                Logger.writeLog("Writen : " + Utilities.bytesArrayToHex(packet));
                _serialPort.Write(packet, 0, packet.Length);

                int dataLength = _serialPort.BytesToRead;
                byte[] data = new byte[dataLength];

                // start to read response from citibank machine
                while (exitLoop == false)
                {
                    Thread.Sleep(1000);// evertime wait for 1 sec before next try

                    countLoop++;

                    if (countLoop == 30)// no of retry 
                    {
                        status = "ERROR. Payment Unsuccessful.";
                        break;
                    }

                    serialResponse = _serialPort.ReadExisting();

                    if (serialResponse.Contains("70") && (serialResponse.Contains("OK") || (serialResponse.Contains("ok"))))
                    {
                        status = "SUCCESS!";
                        sendACK();
                        break;
                    }
                    else if (serialResponse.Contains("70") && !(serialResponse.Contains("OK") || (serialResponse.Contains("ok"))))
                    {
                        status = "ERROR. Payment Unsuccessful.";
                        sendACK();
                        break;
                    }
                }

                //wait for nets machine turn back to the prepaid mode
                Thread.Sleep(10000);

                //Trigerring Nets machine to normal mode
                sendDataToSerialPort(NetsAPI.NETSSwitchPrepaidBackPacket,true);
                
                bool res1 = this.closePort(out status1);

                NetsAPI.PREPAID_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.PREPAID_PURCHASE.reset();
            }*/
        }
        
        //private SerialPort COMPORTS;
        public static string serialResponse;

        public void addCitiBankPayment(string amount, string refno, out string mainStatus)
        {
            string stx = "02";
            string msg = "";
            string status = "";
            string amountInHex = amount.Replace(".", "");
            string refnoInHex = "";
            StringBuilder sb = new StringBuilder();   
            StringBuilder sb2 = new StringBuilder(); 
            bool exitLoop = false;
            int countLoop = 1;

            for (int i = 0; i < amountInHex.Length; i++)
            {
                if (i % 1 == 0)
                    sb.Append('3');
                sb.Append(amountInHex[i]);
            }
            amountInHex = sb.ToString();

            refno = refno.Replace(" ","");
            refnoInHex = refno;

            if (refno.Substring(0, 1).Equals(" "))
                refno = "00000000";

            for (int i = 0; i < refno.Length; i++)
            {
                if (i % 1 == 0)
                    sb2.Append('3');
                sb2.Append(refnoInHex[i]);
            }
            refnoInHex = sb2.ToString();

            //1c44360010313233343536373839301c03

            if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 3)//example 1.11
                msg = "005036303030303030303030313032303030301c34300012303030303030303030" + amountInHex + "1c36350019" + "3030303030303030303030" + refnoInHex + "1c03";
            else if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 4)//example 11.11
                msg = "005036303030303030303030313032303030301c343000123030303030303030" + amountInHex + "1c36350019" + "3030303030303030303030" + refnoInHex + "1c03";
            else if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 5)//example 111.11
                msg = "005036303030303030303030313032303030301c3430001230303030303030" + amountInHex + "1c36350019" + "3030303030303030303030" + refnoInHex + "1c03";
            else if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 6)//example 1111.11
                msg = "005036303030303030303030313032303030301c34300012303030303030" + amountInHex + "1c36350019" + "3030303030303030303030" + refnoInHex + "1c03";

            string lrc = GetLRCChecksum(msg);
            string allMsg = stx + msg + lrc;
            allMsg = Regex.Replace(allMsg, ".{2}", "$0/");
            allMsg = allMsg.Remove(allMsg.Length - 1);
            byte[] bytes = allMsg.Split('/').Select(s => Convert.ToByte(s, 16)).ToArray();

            _serialPort.Write(bytes, 0, bytes.Length);

            int dataLength = _serialPort.BytesToRead;
            byte[] data = new byte[dataLength];

            // start to read response from citibank machine
            while (exitLoop == false)
            {
                Thread.Sleep(1000);// evertime wait for 1 sec before next try

                countLoop++;

                if (countLoop == 11)// no of retry 
                {
                    status = "ERROR. Payment Unsuccessful.";
                    break;
                }

                serialResponse = _serialPort.ReadExisting();
                Logger.writeLog("Citibank Response : " + serialResponse);
                if (!serialResponse.Contains("PLEASE TRY AGAIN"))
                {
                    status = "SUCCESS!";
                }
                else
                {
                    status = "ERROR. Payment Unsuccessful.";
                    break;
                }
            }

            mainStatus = status;
            _serialPort.Close();
        }

        public void addCitiBankPayment(string amount, out string mainStatus)
        {
            string stx = "02";
            string msg = "";
            string status = "";
            string amountInHex = amount.Replace(".", "");
            StringBuilder sb = new StringBuilder();
            bool exitLoop = false;
            int countLoop = 1;

            for (int i = 0; i < amountInHex.Length; i++)
            {
                if (i % 1 == 0)
                    sb.Append('3');
                sb.Append(amountInHex[i]);
            }
            amountInHex = sb.ToString();

            if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 3)//example 1.11
                msg = "005036303030303030303030313032303030301c34300012303030303030303030" + amountInHex + "1c44350010313233343536373839301c03";
            else if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 4)//example 11.11
                msg = "005036303030303030303030313032303030301c343000123030303030303030" + amountInHex   + "1c44360010313233343536373839301c03";
            else if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 5)//example 111.11
                msg = "005036303030303030303030313032303030301c3430001230303030303030" + amountInHex     + "1c44370010313233343536373839301c03";
            else if (amount.Replace(".", "").Trim('.').ToCharArray().Count() == 6)//example 1111.11
                msg = "005036303030303030303030313032303030301c34300012303030303030" + amountInHex       + "1c44380010313233343536373839301c03";

            string lrc = GetLRCChecksum(msg);
            string allMsg = stx + msg + lrc;
            allMsg = Regex.Replace(allMsg, ".{2}", "$0/");
            allMsg = allMsg.Remove(allMsg.Length - 1);
            byte[] bytes = allMsg.Split('/').Select(s => Convert.ToByte(s, 16)).ToArray();

            _serialPort.Write(bytes, 0, bytes.Length);

            int dataLength = _serialPort.BytesToRead;
            byte[] data = new byte[dataLength];

            // start to read response from citibank machine
            while (exitLoop == false)
            {
                Thread.Sleep(1000);// evertime wait for 1 sec before next try

                countLoop++;

                if (countLoop == 11)// no of retry 
                {
                    status = "ERROR. Payment Unsuccessful.";
                    break;
                }

                serialResponse = _serialPort.ReadExisting();

                if (!serialResponse.Contains("PLEASE TRY AGAIN"))
                {
                    status = "SUCCESS!";
                }
                else
                {
                    status = "ERROR. Payment Unsuccessful.";
                    break;
                }
            }

            mainStatus = status;
            _serialPort.Close();
        }

        public static string GetLRCChecksum(string s)
        {
            s = s.TrimEnd(' ').TrimStart(' ');
            int checksum = 0;
            foreach (char c in GetStringFromHex(s))
            {
                checksum ^= Convert.ToByte(c);
            }
            return checksum.ToString("X2");
        }

        public static string GetStringFromHex(string s)
        {
            //Logger.writeLogToFile("Start Get String from Hex");
            string result = "";
            string s2 = s.Replace(" ", "");
            int tmp;

            for (int i = 0; i < s2.Length; i += 2)
            {
                if (Int32.TryParse(s2.Substring(i, 2), System.Globalization.NumberStyles.HexNumber,
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat, out tmp))
                {

                }
                else
                {
                    // Logger.writeLogToFile("Unable to parse > " + s2.Substring(i, 2) + ". From " + s2);
                }
                result += Convert.ToChar(int.Parse(s2.Substring(i, 2), System.Globalization.NumberStyles.HexNumber));
            }
            //Logger.writeLogToFile("End Get String from Hex");
            return result;
        }

        #region *) UOB QR Payment
        public void addAliPayPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.UOB_ALIPAY.setDebitAmount(amount);
                NetsAPI.UOB_ALIPAY.setRefNo(refno);//change
                //NetsAPI.UOB_ALIPAY.setQRType(NETSConstants.MessageHeader.QRMPMType.ALIPAY);
                NetsAPI.UOB_ALIPAY.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.UOB_ALIPAY, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.UOB_ALIPAY.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.UOB_ALIPAY.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    
                    if (responseText != "TIMEOUT")
                    {
                        if (!(responseCode == "00"))
                        {
                            if (responseCode == "IN")
                            {
                                if (sendInquiry())
                                    status = "SUCCESS!";
                                else
                                    //response code is not "00" therefore, perform error handling.
                                    status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                            }
                            else
                                //response code is not "00" therefore, perform error handling.
                                status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.UOB_ALIPAY.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.UOB_ALIPAY.reset();
            }

        }

        public void addWechatPayPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.UOB_WECHATPAY.setDebitAmount(amount);
                NetsAPI.UOB_WECHATPAY.setRefNo(refno);//change
                //NetsAPI.UOB_ALIPAY.setQRType(NETSConstants.MessageHeader.QRMPMType.ALIPAY);
                NetsAPI.UOB_WECHATPAY.setECRType(ecrType);
                int res = sendDataToSerialPort(NetsAPI.UOB_WECHATPAY, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.UOB_WECHATPAY.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.UOB_WECHATPAY.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {
                        if (!(responseCode == "00"))
                        {
                            if (responseCode == "IN")
                            {
                                if (sendInquiry())
                                    status = "SUCCESS!";
                                else
                                    //response code is not "00" therefore, perform error handling.
                                    status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                            }
                            else
                                //response code is not "00" therefore, perform error handling.
                                status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.UOB_WECHATPAY.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.UOB_WECHATPAY.reset();
            }

        }

        public void addUOBUPIPayment(string amount, string refno, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.UOB_UPI.setDebitAmount(amount);
                NetsAPI.UOB_UPI.setRefNo(refno);//change
                //NetsAPI.UOB_ALIPAY.setQRType(NETSConstants.MessageHeader.QRMPMType.ALIPAY);
                NetsAPI.UOB_UPI.setECRType(NETSConstants.MessageHeader.QRMPMType.UPI);
                int res = sendDataToSerialPort(NetsAPI.UOB_UPI, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.UOB_UPI.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.UOB_UPI.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {
                        if (!(responseCode == "00"))
                        {
                            if (responseCode == "IN")
                            {
                                
                                if (sendInquiry())
                                    status = "SUCCESS!";
                                else
                                    //response code is not "00" therefore, perform error handling.
                                    status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                            }
                            else
                                //response code is not "00" therefore, perform error handling.
                                status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.UOB_UPI.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.UOB_UPI.reset();
            }

        }

        private bool sendInquiry()
        {
            Thread.Sleep(1000);
            int InquiryResult = sendDataToSerialPort(NetsAPI.UOB_QRINQUIRY, true);
            int res =0;
            if (res != NETSConstants.SerialPortResult.SUCCESS)
            {
                NetsAPI.UOB_QRINQUIRY.reset();
                return false;
            }
            if (NetsAPI.UOB_QRINQUIRY.GetResponseInfoValue("Response Code") != "00")
            {
                NetsAPI.UOB_QRINQUIRY.reset();
                return false;
            }
            return true;
        }
        #endregion

        public void addTestPayment(string command, out string status)
        {
            status = "";
            string status1;
            try
            {
                NetsAPI.NETS_TEST.setCommand(command);
                int res = sendDataToSerialPort(NetsAPI.NETS_TEST, true);
                if (res == NETSConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = NetsAPI.NETS_TEST.GetResponseInfo()["Response Text"];
                    string responseCode = NetsAPI.NETS_TEST.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseCode == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. PAYMENT UNSUCCESSFUL. " + responseText;
                        }
                        else
                        {
                            status = "SUCCESS!";
                        }
                    }
                    else
                    {
                        status = "ERROR. TIMEOUT";
                    }

                }
                else
                {
                    Thread.Sleep(3000);//wait for 3 second before next trial

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                NetsAPI.NETS_TEST.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                NetsAPI.NETS_TEST.reset();
            }

        }

        #endregion

        #region Functions to Setup Serial Port
        /// <summary>
        /// Setup serial port then open it
        /// </summary>
        /// <param name="portName">Port name to be opened</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool setupSerialPort(string portName)
        {
            //return true;
            if (_serialPort.IsOpen)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                return true;
            }

            string status = "";
            if (setSerialPortParams(portName, out status))
            {
                if (openPort(out status))
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Attach callback function to handle serial port response asynchronously, but since NETS machine
         * required to wait for response, thus this is not needed
         */
        //private bool attachReceiveEventHandler()
        //{
        //    try
        //    {
        //        _serialPort.DataReceived += new SerialDataReceivedEventHandler(serialDataReceived);

        //        logText("Attached Serial Data Received Event Handler!");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //    return false;
        //}

        /// <summary>
        /// To close serial port
        /// </summary>
        /// <returns>True if successful, otherwise false</returns>
        private bool closePort(out string status)
        {
            status = "OK";
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();

                    Logger.writeLog("Port (" + _serialPort.PortName + ") closed!");
                    return true;
                }
                else
                {
                    Logger.writeLog("> Port (" + _serialPort.PortName + ") closed!");
                }
            }
            catch (Exception ex)
            {
                status = "Error. Cannot Close Port";
            }

            return false;
        }

        /// <summary>
        /// To open serial port
        /// </summary>
        /// <returns>True if succesful, otherwise false</returns>
        private bool openPort(out string status)
        {
            status = "OK";
            try
            {
                //_serialPort.DiscardInBuffer();
                //_serialPort.DiscardOutBuffer();

                //if (attachReceiveEventHandler())
                //{
                if (_serialPort.IsOpen)
                { _serialPort.Close(); }

                _serialPort.Open();

                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                if (_serialPort.IsOpen)
                {
                    Logger.writeLog("Port (" + _serialPort.PortName + ") opened!");
                    status = "OK";
                    return true;
                }
                //}
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
            }

            return false;
        }

        /// <summary>
        /// Set the parameters for serial port, to be called before opening serial port
        /// </summary>
        /// <param name="portName">Portname to be opened</param>
        /// <returns>True if succesful, otherwise false</returns>
        private bool setSerialPortParams(string portName, out string status)
        {
            try
            {
                status = "OK";
                if (!string.IsNullOrEmpty(portName))
                {
                    _serialPort.PortName = portName;
                    _serialPort.BaudRate = NetsAPI.DEFAULT_BAUDRATE;
                    _serialPort.Parity = NetsAPI.DEFAULT_PARITY;
                    _serialPort.DataBits = NetsAPI.DEFAULT_DATABITS;
                    _serialPort.StopBits = NetsAPI.DEFAULT_STOPBITS;
                    //_serialPort.Handshake = NetsAPI.DEFAULT_HANDSHAKE;

                    _serialPort.ReadTimeout = NetsAPI.DEFAULT_TIMEOUT;
                    _serialPort.WriteTimeout = NetsAPI.DEFAULT_TIMEOUT;

                    return true;
                }
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
            }

            return false;
        }
        #endregion

        #region Serial Port operations

        /// <summary>
        /// Reset read buffer so that previous received response can be cleared
        /// </summary>
        private void resetReadBuffer()
        {
            readBuffer = new byte[DEFAULT_RESPONSE_LENGTH];
            respIdx = 0;
            finishedReading = false;
        }

        /// <summary>
        /// Response from NETS machine will include 0x06 (ACK) byte, therefore retrieve the needed
        /// response from read buffer
        /// </summary>
        private void getResponseFromReadBuffer()
        {
            int startIdx = 0;
            int etx = 0;
            while (readBuffer[startIdx] != NETSConstants.STX)
            {
                ++startIdx;
            }
            while (readBuffer[etx] != NETSConstants.ETX)
            {
                ++etx;
            }
            etx += 2;
            
            response = new byte[respIdx - startIdx];
            //Logger.writeLog("Response Length : " + response.Length.ToString());
            Array.Copy(readBuffer, startIdx, response, 0, response.Length);


        }

        /// <summary>
        /// Response from NETS machine will include 0x06 (ACK) byte, therefore retrieve the needed
        /// response from read buffer
        /// </summary>
        private void getResponseFromReadBufferECR3()
        {
            int startIdx = 0;
            int etx = 0;
            while (readBuffer[startIdx] != NETSConstants.STX)
            {
                ++startIdx;
            }
            while (readBuffer[etx] != NETSConstants.ETXECR3)
            {
                ++etx;
            }
            etx += 2;

            response = new byte[respIdx - startIdx];
            //Logger.writeLog("Response Length : " + response.Length.ToString());
            Array.Copy(readBuffer, startIdx, response, 0, response.Length);


        }


        public bool hasEtx()
        {
            int etx = 0;
            int resEtx = 0;
            if (readBuffer.Length > 0)
            {
                while (readBuffer[etx] != NETSConstants.ETX && etx < respIdx)
                {
                    ++etx;
                    if (readBuffer[etx] == NETSConstants.ETX)
                        resEtx = etx;
                }
                if (resEtx > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Send ACK packet to NETS machine upon successfully received response
        /// </summary>
        private void sendACK()
        {
            sendDataToSerialPort(NetsAPI.ACK, false);
        }

        /// <summary>
        /// Send NACK packet to NETS machine upon wrong or no response
        /// </summary>
        private void sendNACK()
        {
            sendDataToSerialPort(NetsAPI.NACK, false);
        }

        private void sendDataToSerialPortActLoyalty(Packet pkt)
        {
            byte[] packet = pkt.toBytes();
            Logger.writeLog("Writen : " + Utilities.bytesArrayToHex(packet));

            //send the packet to the machine 
            _serialPort.Write(packet, 0, packet.Length);
        }

        /// <summary>
        /// Send bytes array to NETS machine via serial port
        /// </summary>
        /// <param name="pkt">Bytes array</param>
        /// <param name="waitForReply">
        /// Indicate if to wait for reply from NETS machine, 
        /// if this is false, then after packet is sent, function will return
        /// </param>
        /// <returns>
        /// <para>0 ; Success</para>
        /// <para>1 : Sending Failed, hardware issue (time out, machine reply NACK)</para>
        /// <para>2 : Wrong Checksum</para>
        /// <para>3 : Sending failed, exception happened</para>
        /// </returns>
        private int sendDataToSerialPort(Packet pkt, bool waitForReply)
        {
            int result = NETSConstants.SerialPortResult.FAILED_HW;

            try
            {
                int wrongChecksumCount = 0, nackCount = 0, sendCount = 0;
                bool resend = true; //set true to send for first time
                bool continueWaiting = true;
                bool readFinish = false;
                DateTime waitStartTime = DateTime.Now;

                byte[] packet = pkt.toBytes();

                if (packet.Length > 0)
                {
                    

                    if (waitForReply)
                    {
                        resetReadBuffer();
                    }

                    if ((DateTime.Now - lastSendTime).TotalMilliseconds < 200)
                    {
                        Thread.Sleep(200 - ((int)((DateTime.Now - lastSendTime).TotalMilliseconds)));//Must wait for at least 200 ms before next sending time
                    }

                    DateTime startTime = DateTime.Now;
                    //while (waitForReply && (!finishedReading) && ((DateTime.Now - startTime).TotalSeconds < 120)) //wait for 2 minutes at least
                    while (continueWaiting)
                    {
                        if (resend)
                        {
                            //Logger.writeLog("Packet : " + Utilities.bytesArrayToHex(packet));

                            if (sendCount <= NETSConstants.MAX_TRIALS_NUM - 1)//test  && sendCount == 2
                            {
                                //send the packet to the machine 
                                _serialPort.Write(packet, 0, packet.Length);
                            }

                            lastSendTime = DateTime.Now;
                            waitStartTime = DateTime.Now;

                            Logger.writeLog("Writen : " + Utilities.bytesArrayToHex(packet));

                            ++sendCount;

                            //after first send, don't resend
                            resend = false;
                        }

                        if (sendCount == 3)
                        {
                            Thread.Sleep(200);
                        }

                        if (!waitForReply)
                        {
                            continueWaiting = false;
                            resend = false;
                        }
                        else
                        {
                            /*
                             * wait for reply
                             */
                            //if (sendCount > 2)//test
                            //{
                            //Logger.writeLog("Start Reading" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            string transactionResult = "";
                            int dataLength = _serialPort.BytesToRead;
                            byte[] data = new byte[dataLength];
                            if (_serialPort.Read(data, 0, dataLength) != 0)
                            {
                                if ((respIdx + dataLength) > DEFAULT_RESPONSE_LENGTH)
                                {
                                    resetReadBuffer();
                                    //readBuffer = new 
                                    readBuffer = new byte[respIdx + dataLength];
                                }
                                Logger.writeLog("Received : " + Utilities.bytesArrayToHex(data));
                                Logger.writeLog("Data Length : " + dataLength.ToString());
                                Array.Copy(data, 0, readBuffer, respIdx, dataLength);
                                respIdx += dataLength;

                                string str = Utilities.bytesArrayToHex(readBuffer);

                                Logger.writeLog("Read : " + str);

                                waitStartTime = DateTime.Now;
                            }
                            // }//test
                            //Logger.writeLog(readBuffer[0].ToString());
                            //Logger.writeLog("Resp Idx : " + respIdx.ToString());
                            if ((readBuffer[0] == NETSConstants.NACK) //receive NACK
                                || ((readBuffer[0] != NETSConstants.ACK) && (DateTime.Now - waitStartTime).TotalSeconds > 2)) //resend after 2 seconds, but if ACK is received, cancel resend
                            {
                                resend = true;
                                continueWaiting = true;
                                resetReadBuffer();
                                if (readBuffer[0] == NETSConstants.NACK)
                                {
                                    ++nackCount;
                                }
                            }
                            else if ((respIdx > 2) && (readBuffer[respIdx - 2] == NETSConstants.ETX))//test && sendCount > 2
                            {
                                //Thread.Sleep(100);
                                getResponseFromReadBuffer();

                                //Process response regardless of checksum incorrect
                                Logger.writeLog("Full Packet Read : " + Utilities.bytesArrayToHex(response));

                                pkt.processResponse(response);
                                responseInfo = pkt.getResponseJSONString();

                                Logger.writeLog(pkt.getResponseString());
                                Logger.writeLog(responseInfo);  

                                byte checksum = readBuffer[respIdx - 1];

                                Logger.writeLog("LRC = " + Packet.ComputeLRC(response).ToString() + "," + "Checksum = " + checksum.ToString());

                                if (Packet.ComputeLRC(response) == checksum)
                                {
                                    //Reply is done and checksum is correct, send ACK to NETS machine and exit the loop
                                    readFinish = true;
                                    sendACK();
                                    sendCount = 0;
                                }
                                else
                                {
                                    //Wrong checksum, send NACK
                                    //result = NETSConstants.SerialPortResult.WRONG_CHECKSUM;
                                    finishedReading = false;
                                    resetReadBuffer();
                                    sendNACK();
                                    //Logger.writeLog("Sending NACK" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //don't resend packet, but wait for response again
                                    resend = false;
                                    continueWaiting = true;
                                    ++wrongChecksumCount;
                                }
                            }

                            if (readFinish)
                            {
                                continueWaiting = false;
                                resend = false;                              
                                result = NETSConstants.SerialPortResult.SUCCESS;                               
                            }
                            else if (((DateTime.Now - startTime).TotalSeconds > 120)
                                || (sendCount > NETSConstants.MAX_TRIALS_NUM)
                                || (nackCount > NETSConstants.MAX_TRIALS_NUM))
                            {
                                //if timeout, or send more than sending limit, return error
                                if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK" )
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.FAILED_HW; //timeout
                                }
                            }
                            else if (wrongChecksumCount >= NETSConstants.MAX_TRIALS_NUM)
                            {
                                if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                    {
                                        continueWaiting = false;
                                        resend = false;
                                        result = NETSConstants.SerialPortResult.SUCCESS;
                                    }
                                    else
                                    {
                                        //if received wrong checksum for more that limit, return checksum error
                                        continueWaiting = false;
                                        resend = false;
                                        result = NETSConstants.SerialPortResult.WRONG_CHECKSUM;//wrong checksum
                                    }
                                }
                                
                            }
                            else
                            {
                                //if no condition is met, i.e. not readFinish, not timeout, not send count exceed limit, not received checksum exceed limit, continue to wait
                                continueWaiting = true;
                                Thread.Sleep(10);
                            }
                        }
                    }//end while loop
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "    ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.writeLog("Error on sending data to Serial Port : " + ex.ToString());
                result = NETSConstants.SerialPortResult.FAILED_EXCEPTION;
            }
            finally
            {
                resetReadBuffer();
            }

            return result;
        }

        /// <summary>
        /// Send bytes array to NETS machine via serial port
        /// </summary>
        /// <param name="pkt">Bytes array</param>
        /// <param name="waitForReply">
        /// Indicate if to wait for reply from NETS machine, 
        /// if this is false, then after packet is sent, function will return
        /// </param>
        /// <returns>
        /// <para>0 ; Success</para>
        /// <para>1 : Sending Failed, hardware issue (time out, machine reply NACK)</para>
        /// <para>2 : Wrong Checksum</para>
        /// <para>3 : Sending failed, exception happened</para>
        /// </returns>
        private int sendDataToSerialPortPassThrough(Packet pkt, bool waitForReply)
        {
            int result = NETSConstants.SerialPortResult.FAILED_HW;

            try
            {
                int wrongChecksumCount = 0, nackCount = 0, sendCount = 0;
                bool resend = true; //set true to send for first time
                bool continueWaiting = true;
                bool readFinish = false;
                DateTime waitStartTime = DateTime.Now;

                byte[] packet = pkt.toBytes();

                if (packet.Length > 0)
                {


                    if (waitForReply)
                    {
                        resetReadBuffer();
                    }

                    if ((DateTime.Now - lastSendTime).TotalMilliseconds < 200)
                    {
                        Thread.Sleep(200 - ((int)((DateTime.Now - lastSendTime).TotalMilliseconds)));//Must wait for at least 200 ms before next sending time
                    }

                    DateTime startTime = DateTime.Now;
                    //while (waitForReply && (!finishedReading) && ((DateTime.Now - startTime).TotalSeconds < 120)) //wait for 2 minutes at least
                    while (continueWaiting)
                    {
                        if (resend)
                        {
                            //Logger.writeLog("Packet : " + Utilities.bytesArrayToHex(packet));

                            if (sendCount <= NETSConstants.MAX_TRIALS_NUM - 1)//test  && sendCount == 2
                            {
                                //send the packet to the machine 
                                _serialPort.Write(packet, 0, packet.Length);
                            }

                            lastSendTime = DateTime.Now;
                            waitStartTime = DateTime.Now;

                            Logger.writeLog("Writen : " + Utilities.bytesArrayToHex(packet));

                            ++sendCount;

                            //after first send, don't resend
                            resend = false;
                        }

                        if (sendCount == 3)
                        {
                            Thread.Sleep(200);
                        }

                        if (!waitForReply)
                        {
                            continueWaiting = false;
                            resend = false;
                        }
                        else
                        {
                            /*
                             * wait for reply
                             */
                            //if (sendCount > 2)//test
                            //{
                            //Logger.writeLog("Start Reading" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            string transactionResult = "";
                            int dataLength = _serialPort.BytesToRead;
                            byte[] data = new byte[dataLength];
                            if (_serialPort.Read(data, 0, dataLength) != 0)
                            {
                                if ((respIdx + dataLength) > DEFAULT_RESPONSE_LENGTH)
                                {
                                    resetReadBuffer();
                                    //readBuffer = new 
                                    readBuffer = new byte[respIdx + dataLength];
                                }
                                //Logger.writeLog("Received : " + Utilities.bytesArrayToHex(data));
                                //Logger.writeLog("Data Length : " + dataLength.ToString());
                                Array.Copy(data, 0, readBuffer, respIdx, dataLength);
                                respIdx += dataLength;

                                string str = Utilities.bytesArrayToHex(readBuffer);

                                //Logger.writeLog("Read : " + str);

                                waitStartTime = DateTime.Now;
                            }
                            // }//test
                            //Logger.writeLog(readBuffer[0].ToString());
                            //Logger.writeLog("Resp Idx : " + respIdx.ToString());
                            if ((readBuffer[0] == NETSConstants.NACK) //receive NACK
                                || ((readBuffer[0] != NETSConstants.ACK) && (DateTime.Now - waitStartTime).TotalSeconds > 2)) //resend after 2 seconds, but if ACK is received, cancel resend
                            {
                                resend = true;
                                continueWaiting = true;
                                resetReadBuffer();
                                if (readBuffer[0] == NETSConstants.NACK)
                                {
                                    ++nackCount;
                                }
                            }
                            else if ((respIdx > 2) && (readBuffer[respIdx - 2] == NETSConstants.ETX))//test && sendCount > 2
                            {
                                //Thread.Sleep(100);

                                getResponseFromReadBuffer();
                                Logger.writeLog("Full Packet Read : " + Utilities.bytesArrayToHex(response));
                                //Remove the NETS Header

                                //Process response regardless of checksum incorrect
                                pkt.processResponseECR2(response);
                                responseInfo = pkt.getResponseJSONString();

                                Logger.writeLog(pkt.getResponseString());
                                Logger.writeLog(responseInfo);

                                byte checksum = readBuffer[respIdx - 1];

                                Logger.writeLog("LRC = " + Packet.ComputeLRC(response).ToString() + "," + "Checksum = " + checksum.ToString());

                                if (Packet.ComputeLRC(response) == checksum)
                                {
                                    //Reply is done and checksum is correct, send ACK to NETS machine and exit the loop
                                    readFinish = true;
                                    sendACK();
                                    sendCount = 0;
                                }
                                else
                                {
                                    //Wrong checksum, send NACK
                                    //result = NETSConstants.SerialPortResult.WRONG_CHECKSUM;
                                    finishedReading = false;
                                    resetReadBuffer();
                                    sendNACK();
                                    //Logger.writeLog("Sending NACK" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //don't resend packet, but wait for response again
                                    resend = false;
                                    continueWaiting = true;
                                    ++wrongChecksumCount;
                                }
                            }

                            if (readFinish)
                            {
                                continueWaiting = false;
                                resend = false;
                                result = NETSConstants.SerialPortResult.SUCCESS;
                            }
                            else if (((DateTime.Now - startTime).TotalSeconds > 120)
                                || (sendCount > NETSConstants.MAX_TRIALS_NUM)
                                || (nackCount > NETSConstants.MAX_TRIALS_NUM))
                            {
                                //if timeout, or send more than sending limit, return error
                                if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.FAILED_HW; //timeout
                                }
                            }
                            else if (wrongChecksumCount >= NETSConstants.MAX_TRIALS_NUM)
                            {
                                if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                    {
                                        continueWaiting = false;
                                        resend = false;
                                        result = NETSConstants.SerialPortResult.SUCCESS;
                                    }
                                    else
                                    {
                                        //if received wrong checksum for more that limit, return checksum error
                                        continueWaiting = false;
                                        resend = false;
                                        result = NETSConstants.SerialPortResult.WRONG_CHECKSUM;//wrong checksum
                                    }
                                }

                            }
                            else
                            {
                                //if no condition is met, i.e. not readFinish, not timeout, not send count exceed limit, not received checksum exceed limit, continue to wait
                                continueWaiting = true;
                                Thread.Sleep(10);
                            }
                        }
                    }//end while loop
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "    ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.writeLog("Error on sending data to Serial Port : " + ex.ToString());
                result = NETSConstants.SerialPortResult.FAILED_EXCEPTION;
            }
            finally
            {
                resetReadBuffer();
            }

            return result;
        }

        /// <summary>
        /// Send bytes array to NETS machine via serial port
        /// </summary>
        /// <param name="pkt">Bytes array</param>
        /// <param name="waitForReply">
        /// Indicate if to wait for reply from NETS machine, 
        /// if this is false, then after packet is sent, function will return
        /// </param>
        /// <returns>
        /// <para>0 ; Success</para>
        /// <para>1 : Sending Failed, hardware issue (time out, machine reply NACK)</para>
        /// <para>2 : Wrong Checksum</para>
        /// <para>3 : Sending failed, exception happened</para>
        /// </returns>
        private int sendDataToSerialPortECR3(Packet pkt, bool waitForReply)
        {
            int result = NETSConstants.SerialPortResult.FAILED_HW;

            try
            {
                int wrongChecksumCount = 0, nackCount = 0, sendCount = 0;
                bool resend = true; //set true to send for first time
                bool continueWaiting = true;
                bool readFinish = false;
                DateTime waitStartTime = DateTime.Now;

                byte[] packet = pkt.toBytes();

                if (packet.Length > 0)
                {


                    if (waitForReply)
                    {
                        resetReadBuffer();
                    }

                    if ((DateTime.Now - lastSendTime).TotalMilliseconds < 200)
                    {
                        Thread.Sleep(200 - ((int)((DateTime.Now - lastSendTime).TotalMilliseconds)));//Must wait for at least 200 ms before next sending time
                    }

                    DateTime startTime = DateTime.Now;
                    //while (waitForReply && (!finishedReading) && ((DateTime.Now - startTime).TotalSeconds < 120)) //wait for 2 minutes at least
                    while (continueWaiting)
                    {
                        if (resend)
                        {
                            //Logger.writeLog("Packet : " + Utilities.bytesArrayToHex(packet));

                            if (sendCount <= NETSConstants.MAX_TRIALS_NUM - 1)//test  && sendCount == 2
                            {
                                //send the packet to the machine 
                                _serialPort.Write(packet, 0, packet.Length);
                            }

                            lastSendTime = DateTime.Now;
                            waitStartTime = DateTime.Now;

                            OnNETSLog("Writen : " + Utilities.bytesArrayToHex(packet));

                            ++sendCount;

                            //after first send, don't resend
                            resend = false;
                        }

                        if (sendCount == 3)
                        {
                            Thread.Sleep(200);
                        }

                        if (!waitForReply)
                        {
                            continueWaiting = false;
                            resend = false;
                        }
                        else
                        {
                            /*
                             * wait for reply
                             */
                            //if (sendCount > 2)//test
                            //{
                            //Logger.writeLog("Start Reading" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            string transactionResult = "";
                            int dataLength = _serialPort.BytesToRead;
                            byte[] data = new byte[dataLength];
                            if (_serialPort.Read(data, 0, dataLength) != 0)
                            {
                                if ((respIdx + dataLength) > DEFAULT_RESPONSE_LENGTH)
                                {
                                    resetReadBuffer();
                                    //readBuffer = new 
                                    readBuffer = new byte[respIdx + dataLength];
                                }
                                Logger.writeLog("Received : " + Utilities.bytesArrayToHex(data));
                                Logger.writeLog("Data Length : " + dataLength.ToString());
                                Array.Copy(data, 0, readBuffer, respIdx, dataLength);
                                respIdx += dataLength;

                                string str = Utilities.bytesArrayToHex(readBuffer);

                                Logger.writeLog("Read : " + str);

                                waitStartTime = DateTime.Now;
                            }
                            // }//test
                            //Logger.writeLog(readBuffer[0].ToString());
                            //Logger.writeLog("Resp Idx : " + respIdx.ToString());
                            if ((respIdx > 2) && ( haveETX(readBuffer)))//test && sendCount > 2
                            {
                                //Thread.Sleep(100);
                                getResponseFromReadBufferECR3();

                                //Process response regardless of checksum incorrect
                                OnNETSLog("Full Packet Read : " + Utilities.bytesArrayToHex(response));

                                bool isACK = false;
                                pkt.processResponseECR3(response, out isACK);

                                
                                //responseInfo = pkt.getResponseJSONString();

                                //Logger.writeLog(pkt.getResponseString());
                                //Logger.writeLog(responseInfo);

                                //byte checksum = readBuffer[respIdx - 1];

                                //Logger.writeLog("LRC = " + Packet.ComputeLRC(response).ToString() + "," + "Checksum = " + checksum.ToString());

                                if (!isACK)
                                {
                                    //Reply is done and checksum is correct, send ACK to NETS machine and exit the loop
                                    readFinish = true;
                                    //sendACK();
                                    sendCount = 0;
                                }
                                else
                                {
                                    resend = false;
                                    continueWaiting = true;
                                    resetReadBuffer();
                                    
                                    //Wrong checksum, send NACK
                                    //result = NETSConstants.SerialPortResult.WRONG_CHECKSUM;
                                    //finishedReading = false;
                                    //resetReadBuffer();
                                    //sendNACK();
                                    //Logger.writeLog("Sending NACK" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                    //don't resend packet, but wait for response again
                                    //resend = false;
                                    //continueWaiting = true;
                                    //++wrongChecksumCount;
                                }
                            }

                            if (readFinish)
                            {
                                continueWaiting = false;
                                resend = false;
                                result = NETSConstants.SerialPortResult.SUCCESS;
                            }
                            else if (((DateTime.Now - startTime).TotalSeconds > 120)
                                || (sendCount > NETSConstants.MAX_TRIALS_NUM)
                                || (nackCount > NETSConstants.MAX_TRIALS_NUM))
                            {
                                //if timeout, or send more than sending limit, return error
                                if (pkt.getValue("Response Code") == NETSConstants.MessageHeader.ECR3MEssageStatus.SUCCESS)
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = NETSConstants.SerialPortResult.FAILED_HW; //timeout
                                }
                            }
                            else
                            {
                                //if no condition is met, i.e. not readFinish, not timeout, not send count exceed limit, not received checksum exceed limit, continue to wait
                                continueWaiting = true;
                                Thread.Sleep(10);
                            }
                        }
                    }//end while loop
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "    ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.writeLog("Error on sending data to Serial Port : " + ex.ToString());
                result = NETSConstants.SerialPortResult.FAILED_EXCEPTION;
            }
            finally
            {
                resetReadBuffer();
            }

            return result;
        }

        private bool haveETX(byte[] response)
        {
            for (int i = 0; i < response.Length; i++)
                if (readBuffer[i] == NETSConstants.ETXECR3)
                    return true;

            return false;


        }

        #endregion

        public static List<IntegratedReceiptFields.ReceiptField> GetReceiptFields(string paymentType)
        {
            try
            {
                string netsCC_VISA = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_VISA) ?? "";
                string netsCC_MASTER = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_MASTER) ?? "";
                string netsCC_AMEX = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentNETSCC_AMEX) ?? "";
                
                if (!string.IsNullOrEmpty(paymentType)) paymentType = paymentType.ToUpper().Trim();
                if (paymentType == POSController.PAY_NETSATMCard.ToUpper().Trim())
                    return IntegratedReceiptFields.NETSATMCard();
                else if (paymentType == POSController.PAY_NETSCashCard.ToUpper().Trim())
                    return IntegratedReceiptFields.NETSCashCard();
                else if (paymentType == POSController.PAY_NETSFlashPay.ToUpper().Trim())
                    return IntegratedReceiptFields.NETSFlashPay();
                else if (paymentType == netsCC_VISA.ToUpper().Trim() || paymentType == netsCC_MASTER.ToUpper().Trim() || paymentType == netsCC_AMEX.ToUpper().Trim())
                    return IntegratedReceiptFields.NETSCreditCard();
                else
                    return new List<IntegratedReceiptFields.ReceiptField>();
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return new List<IntegratedReceiptFields.ReceiptField>();
            }
        }
    }
}
