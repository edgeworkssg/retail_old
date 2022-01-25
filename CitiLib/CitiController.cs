using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using PowerPOS;
using System.Text.RegularExpressions;

namespace CitiLib
{
    public class CitiController
    {
        static int DEFAULT_RESPONSE_LENGTH = 1024;
        delegate void SetTextCallback(string text);

        SerialPort _serialPort;
        string[] ports;

        byte[] response;
        byte[] readBuffer = new byte[DEFAULT_RESPONSE_LENGTH];
        int respIdx = 0;
        bool finishedReading = true;
        DateTime lastSendTime;

        string PAYMENT_VISA;
        string PAYMENT_MASTER;
        string PAYMENT_AMEX;
        string PAYMENT_DINERS;
        string PAYMENT_JCB;

        public CitiController()
        {
            _serialPort = new SerialPort();
            PAYMENT_VISA = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_VISA);
            PAYMENT_MASTER = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_MASTER);
            PAYMENT_AMEX = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_AMEX);
            PAYMENT_DINERS = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_DINERS);
            PAYMENT_JCB = AppSetting.GetSetting(AppSetting.SettingsName.Payment.PaymentCitiBank_JCB);
        }
        //change
        #region payment controller
        /*public void addCUPDebitPayment(string amount, out string status)
        {
            status = "";
            string status1;
            try
            {
                CitiAPI.CUP_PURCHASE.setDebitAmount(amount);
                int res = sendDataToSerialPort(CitiAPI.CUP_PURCHASE, true);
                if (res == CitiConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = CitiAPI.CUP_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = CitiAPI.CUP_PURCHASE.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[CitiConstants.ResponseCode.NAME];
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

                CitiAPI.CUP_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                CitiAPI.CUP_PURCHASE.reset();
            }

        }*/
    
        public static string serialResponse;

        public void addCitibankPayment(string amount, string refno, out string status, out string cashIssuerID, out string InvNo)
        {
            cashIssuerID = "";
            InvNo = "";
            try
            {
                CitiAPI.CITI_PURCHASE.setDebitAmount(amount);
                CitiAPI.CITI_PURCHASE.setRefNo(refno);
                //CitiAPI.CITI_PURCHASE.setInvNo(refno);
                int res = sendDataToSerialPort(CitiAPI.CITI_PURCHASE, true);
                if (res == CitiConstants.SerialPortResult.SUCCESS)
                {
                    string responseText = CitiAPI.CITI_PURCHASE.GetResponseInfo()["Response Text"];
                    string responseCode = CitiAPI.CITI_PURCHASE.GetResponseInfo()["Response Code"];
                    if (CitiAPI.CITI_PURCHASE.GetResponseInfo().ContainsKey("Card Issuer ID"))
                    {
                        string temp = CitiAPI.CITI_PURCHASE.GetResponseInfo()["Card Issuer ID"];
                        if (temp.Length > 1)
                            temp = temp[1].ToString();
                        switch (temp)
                        {
                            case "4": cashIssuerID = PAYMENT_VISA;
                                break;
                            case "5": cashIssuerID = PAYMENT_MASTER;
                                break;
                            case "6": cashIssuerID = PAYMENT_DINERS;
                                break;
                            case "7": cashIssuerID = PAYMENT_AMEX;
                                break;
                            case "8": cashIssuerID = PAYMENT_JCB;
                                break;
                            default: cashIssuerID = PAYMENT_VISA;
                                break;
                        }       


                    }
                    else
                        cashIssuerID = "";

                    if (CitiAPI.CITI_PURCHASE.GetResponseInfo().ContainsKey("Invoice Number"))
                        InvNo = CitiAPI.CITI_PURCHASE.GetResponseInfo()["Invoice Number"];
                    else
                        InvNo = "";
                    Logger.writeLog("Response Text : " + responseText);
                    if (responseText != "TIMEOUT")
                    {
                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[CitiConstants.ResponseCode.NAME];
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
                    Thread.Sleep(300);//wait for 3 second before error message prompt
                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                    return;
                }
                string closePortStatus = "";
                bool res1 = this.closePort(out closePortStatus);

                CitiAPI.CITI_PURCHASE.reset();
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                CitiAPI.CITI_PURCHASE.reset();
                return;
            }
        }

        /*public void addCitiBankPayment(string amount, string refno, out string mainStatus)
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
        }*/

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

        #endregion

        #region Functions to Setup Serial Port
        /// <summary>
        /// Setup serial port then open it
        /// </summary>
        /// <param name="portName">Port name to be opened</param>
        /// <returns>True if successful, otherwise false</returns>
        public bool setupSerialPort(string portName)
        {
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
                    _serialPort.BaudRate = CitiAPI.DEFAULT_BAUDRATE;
                    _serialPort.Parity = CitiAPI.DEFAULT_PARITY;
                    _serialPort.DataBits = CitiAPI.DEFAULT_DATABITS;
                    _serialPort.StopBits = CitiAPI.DEFAULT_STOPBITS;
                    //_serialPort.Handshake = NetsAPI.DEFAULT_HANDSHAKE;

                    _serialPort.ReadTimeout = CitiAPI.DEFAULT_TIMEOUT;
                    _serialPort.WriteTimeout = CitiAPI.DEFAULT_TIMEOUT;

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
            while (readBuffer[startIdx] != CitiConstants.STX)
            {
                ++startIdx;
            }
            while (readBuffer[etx] != CitiConstants.ETX)
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
                while (readBuffer[etx] != CitiConstants.ETX && etx < respIdx)
                {
                    ++etx;
                    if (readBuffer[etx] == CitiConstants.ETX)
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
            sendDataToSerialPort(CitiAPI.ACK, false);
        }

        /// <summary>
        /// Send NACK packet to NETS machine upon wrong or no response
        /// </summary>
        private void sendNACK()
        {
            sendDataToSerialPort(CitiAPI.NACK, false);
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
            int result = CitiConstants.SerialPortResult.FAILED_HW;

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
                            if (sendCount <= CitiConstants.MAX_TRIALS_NUM - 1)//test  && sendCount == 2
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
                            if ((readBuffer[0] == CitiConstants.NACK) //receive NACK
                                || ((readBuffer[0] != CitiConstants.ACK) && (DateTime.Now - waitStartTime).TotalSeconds > 2)) //resend after 2 seconds, but if ACK is received, cancel resend
                            {
                                resend = true;
                                continueWaiting = true;
                                resetReadBuffer();
                                if (readBuffer[0] == CitiConstants.NACK)
                                {
                                    ++nackCount;
                                }
                            }
                            else if ((respIdx > 2) && (readBuffer[respIdx - 2] == CitiConstants.ETX))//test && sendCount > 2
                            {
                                //Thread.Sleep(100);
                                getResponseFromReadBuffer();

                                //Process response regardless of checksum incorrect
                                Logger.writeLog("Full Packet Read : " + Utilities.bytesArrayToHex(response));

                                pkt.processResponse(response);

                                Logger.writeLog(pkt.getResponseString());  

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
                                    //result = CitiConstants.SerialPortResult.WRONG_CHECKSUM;
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
                                result = CitiConstants.SerialPortResult.SUCCESS;                               
                            }
                            else if (((DateTime.Now - startTime).TotalSeconds > 120)
                                || (sendCount > CitiConstants.MAX_TRIALS_NUM)
                                || (nackCount > CitiConstants.MAX_TRIALS_NUM))
                            {
                                //if timeout, or send more than sending limit, return error
                                if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK" )
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = CitiConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = CitiConstants.SerialPortResult.FAILED_HW; //timeout
                                }
                            }
                            else if (wrongChecksumCount >= CitiConstants.MAX_TRIALS_NUM)
                            {
                                if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                {
                                    continueWaiting = false;
                                    resend = false;
                                    result = CitiConstants.SerialPortResult.SUCCESS;
                                }
                                else
                                {
                                    if (pkt.getValue("Response Code") == "00" || pkt.getValue("Response Code") == "OK")
                                    {
                                        continueWaiting = false;
                                        resend = false;
                                        result = CitiConstants.SerialPortResult.SUCCESS;
                                    }
                                    else
                                    {
                                        //if received wrong checksum for more that limit, return checksum error
                                        continueWaiting = false;
                                        resend = false;
                                        result = CitiConstants.SerialPortResult.WRONG_CHECKSUM;//wrong checksum
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
                result = CitiConstants.SerialPortResult.FAILED_EXCEPTION;
            }
            finally
            {
                resetReadBuffer();
            }

            return result;
        }
        #endregion

    }
}
