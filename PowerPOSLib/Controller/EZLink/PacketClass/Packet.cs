using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace PowerPOS.EZLink
{
    /// <summary>
    /// Abstract class for general packet, to be inherited for other packet to use
    /// </summary>
    abstract class Packet
    {
        /// <summary>
        /// Contains all available message data returned from NETS machine
        /// </summary>
        protected Dictionary<string, string> responseInfo = new Dictionary<string,string>();
        /// <summary>
        /// Contains the raw response data in bytes array
        /// </summary>
        protected byte[] rawResponseData;
        /// <summary>
        /// Store transaction data if the packet is deduction related packet
        /// </summary>
        protected TransDataFormat transData;

        /// <summary>
        /// Retrieve ResponseInfo Dictionary, for further process
        /// </summary>
        /// <returns>responseInfo</returns>
        public Dictionary<string, string> GetResponseInfo()
        {
            return responseInfo;
        }

        /// <summary>
        /// Return transaction data, <seealso cref="TransDataFormat"/>
        /// </summary>
        /// <returns>TransDataFormat object</returns>
        public TransDataFormat GetTransData()
        {
            return transData;
        }

        /// <summary>
        /// Reset responseInfo, rawResponseData, transData
        /// </summary>
        public virtual void reset()
        {
            if (responseInfo != null)
            {
                responseInfo.Clear();
            }

            if (rawResponseData != null)
            {
                rawResponseData = new byte[2] { 0x00, 0x00 }; //just for reset, can set to any amount
            }

            if (transData != null)
            {
                transData.reset();
            }
        }

        /// <summary>
        /// Generate bytes array based on the type of packet, must be implemented by inheriting classes
        /// </summary>
        /// <returns>Bytes array representing the packet</returns>
        public abstract byte[] toBytes();

        /// <summary>
        /// Generic function to process all available message data in response from NETS machine
        /// <para>NOTE : This is not fully implemented, further development for additional message data is required</para>
        /// </summary>
        /// <param name="response">Raw response byte array from NETS Machine</param>
        public virtual void processResponse(byte[] response)
        {
            try
            {
                rawResponseData = new byte[response.Length];
                Array.Copy(response, rawResponseData, rawResponseData.Length);

                string responseCode = getResponseCodeStrFromMessageHeader(response); //retrieve response code from message header

                //Store response code into responseInfo
                if (responseInfo.ContainsKey(EZLinkConstants.ResponseCode.NAME))
                {
                    responseInfo[EZLinkConstants.ResponseCode.NAME] = responseCode;
                }
                else
                {
                    addKeyValueToResponseInfo(EZLinkConstants.ResponseCode.NAME, responseCode);
                }


                int idx = 3 + EZLinkConstants.MessageHeader.RESPONSE_LENGTH; //1 byte STX, 2 bytes message length

                string fieldCode = string.Empty;
                int len = 0;

                while ((idx < response.Length) && (response[idx] != EZLinkConstants.ETX))
                {
                    //Extract field to determine the process to retrieve response
                    fieldCode = Encoding.ASCII.GetString(new byte[] { response[idx++], response[idx++] });
                    //Extract len to get the correct bytes array from message data in response
                    len = Utilities.bytesBCDToInt(new byte[] { response[idx++], response[idx++] });
                    byte[] temp = new byte[len];
                    Array.Copy(response, idx, temp, 0, len); ;

                    //compare field code and format response
                    if (fieldCode.Equals(EZLinkConstants.MessageData.APPROVAL_CODE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();
                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.APPROVAL_CODE.Name, str);
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.TRACE_NUMBER.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.TRACE_NUMBER.Name, str);
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.CARD_NUMBER.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.CARD_NUMBER.Name, str);
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.ISSUER_NAME.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.ISSUER_NAME.Name, str);
                    }
                    /*else if (fieldCode.Equals(EZLinkConstants.MessageData.CEPAS_VERSION.FieldCode))
                    {
                        string ver = Encoding.ASCII.GetString(temp).Trim();

                        ver = Utilities.removeExtraSpaceBtwnWords(ver);

                        string str = string.Empty;

                        if (ver.Equals(EZLinkConstants.MessageData.CEPASVersion.CEPAS_1.Key))
                        {
                            str = EZLinkConstants.MessageData.CEPASVersion.CEPAS_1.Value;
                        }
                        else if (ver.Equals(EZLinkConstants.MessageData.CEPASVersion.CEPAS_2.Key))
                        {
                            str = EZLinkConstants.MessageData.CEPASVersion.CEPAS_2.Value;
                        }

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.CEPAS_VERSION.Name, str);
                    }*/
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.DEDUCTED_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.DEDUCTED_AMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.EXPIRY_DATE.FieldCode))
                    {
                        //Convert expiry date value to "dd/MM/yyyy" format
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        DateTime dt = DateTime.Now;
                        DateTime.TryParseExact(str, EZLinkConstants.RESPONSE_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.EXPIRY_DATE.Name, dt.ToString(EZLinkConstants.DISPLAY_DATE_FORMAT));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.CARD_BALANCEAMOUNT.FieldCode))
                    {
                        //Convert string to $x.xx format
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.CARD_BALANCEAMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.DISCOUNT_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.DISCOUNT_AMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.FAIL_REASON.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.FAIL_REASON.Name, str);
                    }
                    /*else if (fieldCode.Equals(EZLinkConstants.MessageData.ATU_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.ATU_AMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.BATCH_NUMBER.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.BATCH_NUMBER.Name, str);
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.STAN.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.STAN.Name, str);
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.TRANSACTION_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.TRANSACTION_AMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.TOTAL_FEE_TOP_UP.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.TOTAL_FEE_TOP_UP.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(EZLinkConstants.MessageData.FEE_DUE_TO_MERCHANT_TOP_UP.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(EZLinkConstants.MessageData.FEE_DUE_TO_MERCHANT_TOP_UP.Name, Utilities.stringToCurrencyFormat(str));
                    }*/
                    

                    idx += len + 1; //1 byte for separator
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Generic function to add key value pair into responseInfo, check if key exist, if exist, replace the value, otherwise, add new key
        /// </summary>
        /// <param name="key">String</param>
        /// <param name="value">String</param>
        protected void addKeyValueToResponseInfo(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "-NA-";
            }

            if (responseInfo.ContainsKey(key))
            {
                responseInfo[key] = value;
            }
            else
            {
                responseInfo.Add(key, value);
            }
        }

        /// <summary>
        /// Initialize function, since every functions with response contain response code in message header, store the value first
        /// </summary>
        protected void init()
        {
            addKeyValueToResponseInfo(EZLinkConstants.ResponseCode.NAME, "");
        }

        /// <summary>
        /// Function to generate LRC for each packet
        /// </summary>
        /// <param name="data">Bytes array</param>
        /// <returns>1 byte of LRC</returns>
        public static byte ComputeLRC(byte[] data)
        {
            byte lrc = 0x00;

            /*
             * XOR every bytes, start from 2nd byte, STX not included
             * end at ETX, second last byte
             */
            for (int i = 1; i < (data.Length - 1); i++)
            {
                lrc ^= data[i];
            }

            return lrc;
        }

        /// <summary>
        /// Generate full packet based on the content, add in STX, LRC and such
        /// </summary>
        /// <param name="content">byte array</param>
        /// <returns>Full packet byte array</returns>
        protected byte[] generateFullPacket(byte[] content)
        {
            byte[] contentLength = Utilities.intToBytesBCD(content.Length, EZLinkConstants.MSG_LENGTH_BYTES_COUNT);

            //Inclue STX, ETX, LRC and length of content in full packet
            byte[] fullPacket = new byte[3 + contentLength.Length + content.Length];

            int idx = 0;

            fullPacket[idx++] = EZLinkConstants.STX;

            Array.Copy(contentLength, 0, fullPacket, idx, contentLength.Length);
            idx += contentLength.Length;

            Array.Copy(content, 0, fullPacket, idx, content.Length);
            idx += content.Length;

            fullPacket[idx++] = EZLinkConstants.ETX;
            fullPacket[idx++] = ComputeLRC(fullPacket);

            return fullPacket;
        }

        /// <summary>
        /// Generate message header byte array, not in full packet yet, i.e. missing STX, LRC etc
        /// <para>All message headers are in same length, same format, just different function code, thus use this function to construct message header</para>
        /// </summary>
        /// <param name="functionCode">Function code to be set into the message header, two characters string</param>
        /// <returns>Message header byte array</returns>
        protected byte[] generateMessageHeader(string functionCode)
        {
            byte[] content = new byte[EZLinkConstants.MessageHeader.REQUEST_LENGTH];

            int idx = 0;

            byte[] temp = Utilities.asciiStrToBytesArray(EZLinkConstants.MessageHeader.HEADER_FILLER);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(functionCode);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            /*temp = Utilities.asciiStrToBytesArray(EZLinkConstants.MessageHeader.RFU);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;*/

            temp = Utilities.asciiStrToBytesArray(EZLinkConstants.MessageHeader.END_OF_MSG);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            content[idx++] = EZLinkConstants.MessageHeader.SEPARATOR;

            return content;
        }

        /// <summary>
        /// Extract response code returned by NETS Machine in the message header (part of full response byte array)
        /// <para>Overload function (byte[] getResponseCodeFromMessageHeader(byte[] response))</para>
        /// </summary>
        /// <param name="response">Full response byte array</param>
        /// <returns>Two characters byte array</returns>
        public static string getResponseCodeStrFromMessageHeader(byte[] response)
        {
            return (Utilities.bytesArrayToASCIIString(getResponseCodeFromMessageHeader(response)));
        }

        /// <summary>
        /// Extract response code returned by NETS Machine in the message header (part of full response byte array)
        /// <para>Overload function (string getResponseCodeFromMessageHeader(byte[] response))</para>
        /// </summary>
        /// <param name="response">Full response byte array</param>
        /// <returns>Two characters byte array</returns>
        public static byte[] getResponseCodeFromMessageHeader(byte[] response)
        {
            byte[] retval = new byte[EZLinkConstants.MessageHeader.RESP_CODE_LENGTH];
            int startIdx = 3 + //3 for (STX + 2 bytes LEN)
                EZLinkConstants.MessageHeader.HEADER_FILLER_LENGTH; 
            //+   EZLinkConstants.MessageHeader.FunctionCode.LENGTH;

            Array.Copy(response, startIdx, retval, 0, retval.Length);

            return retval;
        }

        /// <summary>
        /// Format all key value stored in responseInfo to readable format, for viewing purpose
        /// </summary>
        /// <returns>Formatted string</returns>
        public string getResponseString()
        {
            string retval = Environment.NewLine + "==============================" + Environment.NewLine;

            foreach (KeyValuePair<string, string> kvp in responseInfo)
            {
                retval += kvp.Key + " : " + kvp.Value + Environment.NewLine;
            }

            if ((transData != null) && (!transData.isEmpty()))
            {
                retval += transData.ToString();
            }

            retval += "==============================";

            return retval;
        }

        /// <summary>
        /// Gets rawResponseData in Hexadecimal string
        /// </summary>
        /// <returns>rawResponseData</returns>
        public string getRawResponseDataInHex()
        {
            return Utilities.bytesArrayToHex(rawResponseData);
        }
    }
}
