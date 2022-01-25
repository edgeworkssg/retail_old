using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS.EZLink;
using System.IO.Ports;
using PowerPOS;
using System.Threading;
using System.Windows.Forms;

namespace PowerPOSLib.Controller
{
    public class EZLinkController
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

        public EZLinkController()
        {
            _serialPort = new SerialPort();
        }

        #region payment controller
        public void addEZLinkPayment(string amount, out string status)
        {

            status = "";
            string status1 = "";

            try
            {
                EZLinkAPI.EZLINK_PURCHASE_PACKET.setDebitAmount(amount);
                int res = sendDataToSerialPort(EZLinkAPI.EZLINK_PURCHASE_PACKET, true);

                //Check if data is sent successfully, if not, perform error handling
                if (res == EZLinkConstants.SerialPortResult.SUCCESS)
                {
                    //Data is sent successfully, therefore response code is received, check response code
                    string responseText = EZLinkAPI.EZLINK_PURCHASE_PACKET.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Code : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseText == "00"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. CARD ERROR, PLEASE TRY AGAIN";
                            //if (EZLinkAPI.EZLINK_PURCHASE_PACKET.GetResponseInfo().Contains()
                            try
                            {
                                string failReason = EZLinkAPI.EZLINK_PURCHASE_PACKET.GetResponseInfo()["Fail Reason"];
                                status = "ERROR. " + getFailReasonStr(failReason);
                            }
                            catch (Exception ex)
                            {
                                status = "ERROR. CARD ERROR, PLEASE TRY AGAIN";
                            }
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
                    Thread.Sleep(3000); // Wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                EZLinkAPI.EZLINK_PURCHASE_PACKET.reset(); //after processed, reset the packet for next use
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                EZLinkAPI.EZLINK_PURCHASE_PACKET.reset();
            }
        }

        public void addEZLinkContactlessPayment(string amount, out string status)
        {

            status = "";
            string status1 = "";

            try
            {
                EZLinkAPI.EZLINK_CONTACTLESSPURCHASE_PACKET.setDebitAmount(amount);
                int res = sendDataToSerialPort(EZLinkAPI.EZLINK_CONTACTLESSPURCHASE_PACKET, true);

                //Check if data is sent successfully, if not, perform error handling
                if (res == EZLinkConstants.SerialPortResult.SUCCESS)
                {
                    //Data is sent successfully, therefore response code is received, check response code
                    string responseText = EZLinkAPI.EZLINK_CONTACTLESSPURCHASE_PACKET.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Code : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseText == "ND"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. CARD ERROR, PLEASE TRY AGAIN";
                            //if (EZLinkAPI.EZLINK_PURCHASE_PACKET.GetResponseInfo().Contains()
                            try
                            {
                                string failReason = EZLinkAPI.EZLINK_CONTACTLESSPURCHASE_PACKET.GetResponseInfo()["Fail Reason"];
                                status = "ERROR. " + getFailReasonStr(failReason);
                            }
                            catch (Exception ex)
                            {
                                status = "ERROR. CARD ERROR, PLEASE TRY AGAIN";
                            }
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
                    Thread.Sleep(3000); // Wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                EZLinkAPI.EZLINK_CONTACTLESSPURCHASE_PACKET.reset(); //after processed, reset the packet for next use
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                EZLinkAPI.EZLINK_CONTACTLESSPURCHASE_PACKET.reset();
            }
        }

        public void addEZLinkCreditCardPayment(string amount, out string status)
        {

            status = "";
            string status1 = "";

            try
            {
                EZLinkAPI.EZLINK_CREDITCARDPURCHASE_PACKET.setDebitAmount(amount);
                int res = sendDataToSerialPort(EZLinkAPI.EZLINK_CREDITCARDPURCHASE_PACKET, true);

                //Check if data is sent successfully, if not, perform error handling
                if (res == EZLinkConstants.SerialPortResult.SUCCESS)
                {
                    //Data is sent successfully, therefore response code is received, check response code
                    string responseText = EZLinkAPI.EZLINK_CREDITCARDPURCHASE_PACKET.GetResponseInfo()["Response Code"];
                    Logger.writeLog("Response Code : " + responseText);
                    if (responseText != "TIMEOUT")
                    {

                        //string responseCode = NetsAPI.NETS_PURCHASE.GetResponseInfo()[NETSConstants.ResponseCode.NAME];
                        //MessageBoxIcon mbi = MessageBoxIcon.Information;

                        if (!(responseText == "ND"))
                        {
                            //response code is not "00" therefore, perform error handling.
                            status = "ERROR. CARD ERROR, PLEASE TRY AGAIN";
                            //if (EZLinkAPI.EZLINK_PURCHASE_PACKET.GetResponseInfo().Contains()
                            try
                            {
                                string failReason = EZLinkAPI.EZLINK_CREDITCARDPURCHASE_PACKET.GetResponseInfo()["Fail Reason"];
                                status = "ERROR. " + getFailReasonStr(failReason);
                            }
                            catch (Exception ex)
                            {
                                status = "ERROR. CARD ERROR, PLEASE TRY AGAIN";
                            }
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
                    Thread.Sleep(3000); // Wait for 3 second before error message prompt

                    //send failed, error handling
                    status = "ERROR. Payment Unsuccessful, Please Check The Machine and Try Again. ";
                }

                bool res1 = this.closePort(out status1);

                EZLinkAPI.EZLINK_CREDITCARDPURCHASE_PACKET.reset(); //after processed, reset the packet for next use
            }
            catch (Exception ex)
            {
                status = "ERROR. " + ex.Message.ToString();
                Logger.writeLog(ex.Message.ToString());
                EZLinkAPI.EZLINK_CREDITCARDPURCHASE_PACKET.reset();
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
                    _serialPort.BaudRate = EZLinkAPI.DEFAULT_BAUDRATE;
                    _serialPort.Parity = EZLinkAPI.DEFAULT_PARITY;
                    _serialPort.DataBits = EZLinkAPI.DEFAULT_DATABITS;
                    _serialPort.StopBits = EZLinkAPI.DEFAULT_STOPBITS;
                    //_serialPort.Handshake = NetsAPI.DEFAULT_HANDSHAKE;

                    _serialPort.ReadTimeout = EZLinkAPI.DEFAULT_TIMEOUT;
                    _serialPort.WriteTimeout = EZLinkAPI.DEFAULT_TIMEOUT;

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

            while (readBuffer[startIdx] != EZLinkConstants.STX)
            {
                ++startIdx;
            }

            response = new byte[respIdx - startIdx];
            Array.Copy(readBuffer, startIdx, response, 0, response.Length);


        }

        /// <summary>
        /// Send ACK packet to NETS machine upon successfully received response
        /// </summary>
        private void sendACK()
        {
            sendDataToSerialPort(EZLinkAPI.ACK, false);
        }

        /// <summary>
        /// Send NACK packet to NETS machine upon wrong or no response
        /// </summary>
        private void sendNACK()
        {
            sendDataToSerialPort(EZLinkAPI.NACK, false);
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
            int result = EZLinkConstants.SerialPortResult.FAILED_HW;

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
                            if (sendCount <= EZLinkConstants.MAX_TRIALS_NUM - 1)//test  && sendCount == 2
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
                            Thread.Sleep(5000);
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
                            string transactionResult = "";
                            int dataLength = _serialPort.BytesToRead;
                            byte[] data = new byte[dataLength];
                            if (_serialPort.Read(data, 0, dataLength) != 0)
                            {
                                if ((respIdx + dataLength) > DEFAULT_RESPONSE_LENGTH)
                                {
                                    resetReadBuffer();
                                }

                                Array.Copy(data, 0, readBuffer, respIdx, dataLength);
                                respIdx += dataLength;

                                string str = Utilities.bytesArrayToHex(data);

                                Logger.writeLog("Read : " + str);

                                waitStartTime = DateTime.Now;
                            }
                            // }//test

                            if ((readBuffer[0] == EZLinkConstants.NACK) //receive NACK
                                || ((readBuffer[0] != EZLinkConstants.ACK) && (DateTime.Now - waitStartTime).TotalSeconds > 2)) //resend after 2 seconds, but if ACK is received, cancel resend
                            {
                                resend = true;
                                continueWaiting = true;
                                resetReadBuffer();
                                if (readBuffer[0] == EZLinkConstants.NACK)
                                {
                                    ++nackCount;
                                }
                            }
                            else if ((respIdx > 2) && (readBuffer[respIdx - 2] == EZLinkConstants.ETX))//test && sendCount > 2
                            {                               

                                getResponseFromReadBuffer();

                                //Process response regardless of checksum incorrect
                                Logger.writeLog("Full Packet : " + Utilities.bytesArrayToHex(response));
                                //response = Utilities.hexStrToBytesArray("02 01 19 36 30 30 30 30 30 30 30 30 30 31 31 32 30 30 30 30 1C 30 31 00 06 00 00 00 00 00 00 1C 36 35 00 06 30 30 30 30 30 32 1C 33 30 00 16 31 30 30 30 31 31 30 30 30 31 30 39 32 34 35 35 1C 33 31 00 04 30 30 30 30 1C 33 32 00 10 45 5A 4C 49 4E 4B 00 00 00 00 1C 34 31 00 12 30 30 30 30 30 30 30 30 30 30 30 31 1C 34 32 00 12 30 30 30 30 30 30 30 30 30 30 30 30 1C 03 3F");
                                pkt.processResponse(response);

                                Logger.writeLog(pkt.getResponseString());  

                                byte checksum = readBuffer[respIdx - 1];

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
                                    sendNACK();

                                    //don't resend packet, but wait for response again
                                    resend = false;
                                    continueWaiting = true;
                                    resetReadBuffer();
                                    ++wrongChecksumCount;
                                }
                            }

                            if (readFinish)
                            {
                                continueWaiting = false;
                                resend = false;
                                result = EZLinkConstants.SerialPortResult.SUCCESS;                               
                            }
                            else if (((DateTime.Now - startTime).TotalSeconds > 120)
                                || (sendCount > EZLinkConstants.MAX_TRIALS_NUM)
                                || (nackCount > EZLinkConstants.MAX_TRIALS_NUM))
                            {
                                //if timeout, or send more than sending limit, return error
                                continueWaiting = false;
                                resend = false;
                                result = EZLinkConstants.SerialPortResult.FAILED_HW; //timeout
                            }
                            else if (wrongChecksumCount >= EZLinkConstants.MAX_TRIALS_NUM)
                            {
                                //if received wrong checksum for more that limit, return checksum error
                                continueWaiting = false;
                                resend = false;
                                result = EZLinkConstants.SerialPortResult.WRONG_CHECKSUM;//wrong checksum
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
                MessageBox.Show(ex.ToString(), "    ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                result = EZLinkConstants.SerialPortResult.FAILED_EXCEPTION;
            }
            finally
            {
                resetReadBuffer();
            }

            return result;
        }
        #endregion

        #region Tester
        public string processResponseCode(string temp)
        {
            byte[] resp = Utilities.hexStrToBytesArray(temp);
            EZLinkPurchasePacket pkt = new EZLinkPurchasePacket();
            pkt.processResponse(resp);
            string responseText = pkt.GetResponseInfo()["Response Code"];
            return responseText;
            //pkt.GetResponseInfo();
        }

        public string getFailReasonStr(string code)
        {
            if (code == "51")
            {
                return "READ CARD ERROR";
            }
            else if (code == "52")
            {
                return "Card Balance has no value(Balance = 0.00)";
            }
            else if (code == "53")
            {
                return "Invalid card(Including blacklist card)";
            }
            else if (code == "54")
            {
                return "Card has insufficient balance";
            }
            else if (code == "55")
            {
                return "User cancel transaction(Card been removed from reader)";
            }
            else if (code == "56")
            {
                return "Card reader time out when reading card";
            }
            else if (code == "57")
            {
                return "Card Reader no ready";
            }
            else if (code == "58")
            {
                return "Terminal no ready";
            }
            return "";
        }
        #endregion
    }
}
