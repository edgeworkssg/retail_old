using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Forms;

namespace NETSApplication
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
                if (responseInfo.ContainsKey(NETSConstants.ResponseCode.NAME))
                {
                    responseInfo[NETSConstants.ResponseCode.NAME] = responseCode;
                }

                int idx = 3 + NETSConstants.MessageHeader.RESPONSE_LENGTH; //1 byte STX, 2 bytes message length

                string fieldCode = string.Empty;
                int len = 0;

                while ((idx < response.Length) && (response[idx] != NETSConstants.ETX))
                {
                    //Extract field to determine the process to retrieve response
                    fieldCode = Encoding.ASCII.GetString(new byte[] { response[idx++], response[idx++] });
                    //Extract len to get the correct bytes array from message data in response
                    len = Utilities.bytesBCDToInt(new byte[] { response[idx++], response[idx++] });
                    byte[] temp = new byte[len];
                    Array.Copy(response, idx, temp, 0, len); ;

                    //compare field code and format response
                    if (fieldCode.Equals(NETSConstants.MessageData.RESPONSE_TEXT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();
                        addKeyValueToResponseInfo(NETSConstants.MessageData.RESPONSE_TEXT.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.MERCHANT_NAME_ADDRESS.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.MERCHANT_NAME_ADDRESS.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TERMINAL_ID.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TERMINAL_ID.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.MERCHANT_ID.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.MERCHANT_ID.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CEPAS_VERSION.FieldCode))
                    {
                        string ver = Encoding.ASCII.GetString(temp).Trim();

                        ver = Utilities.removeExtraSpaceBtwnWords(ver);

                        string str = string.Empty;

                        if (ver.Equals(NETSConstants.MessageData.CEPASVersion.CEPAS_1.Key))
                        {
                            str = NETSConstants.MessageData.CEPASVersion.CEPAS_1.Value;
                        }
                        else if (ver.Equals(NETSConstants.MessageData.CEPASVersion.CEPAS_2.Key))
                        {
                            str = NETSConstants.MessageData.CEPASVersion.CEPAS_2.Value;
                        }

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CEPAS_VERSION.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CAN.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CAN.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.EXPIRY_DATE.FieldCode))
                    {
                        //Convert expiry date value to "dd/MM/yyyy" format
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        DateTime dt = DateTime.Now;
                        DateTime.TryParseExact(str, NETSConstants.RESPONSE_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.EXPIRY_DATE.Name, dt.ToString(NETSConstants.DISPLAY_DATE_FORMAT));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CARD_BALANCE.FieldCode))
                    {
                        //Convert string to $x.xx format
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CARD_BALANCE.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.PURSE_STATUS.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.PURSE_STATUS.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.ATU_STATUS.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.ATU_STATUS.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.ATU_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.ATU_AMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.BATCH_NUMBER.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        addKeyValueToResponseInfo(NETSConstants.MessageData.BATCH_NUMBER.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.STAN.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        addKeyValueToResponseInfo(NETSConstants.MessageData.STAN.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TRANSACTION_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TRANSACTION_AMOUNT.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TOTAL_FEE_TOP_UP.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TOTAL_FEE_TOP_UP.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.FEE_DUE_TO_MERCHANT_TOP_UP.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.FEE_DUE_TO_MERCHANT_TOP_UP.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.FEE_DUE_FROM_MERCHANT_TOP_UP.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.FEE_DUE_FROM_MERCHANT_TOP_UP.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RESPONSE_MESSAGE_1.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RESPONSE_MESSAGE_1.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RESPONSE_MESSAGE_2.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RESPONSE_MESSAGE_2.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.PURCHASE_FEE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.PURCHASE_FEE.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.APPROVAL_CODE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.APPROVAL_CODE.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CARD_ISSUER_NAME.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CARD_ISSUER_NAME.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.ADDITIONAL_TRANS_DATA.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.ADDITIONAL_TRANS_DATA.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RETRIEVAL_REF_NUM.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RETRIEVAL_REF_NUM.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TRANS_DATA.FieldCode))
                    {
                        //Trans Data has more information, therefore, need additional class to store the information
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TRANS_DATA.Name, str);

                        string cepasVersion = NETSConstants.MessageData.CEPASVersion.CEPAS_2.Value;

                        if (responseInfo.ContainsKey(NETSConstants.MessageData.CEPAS_VERSION.Name))
                        {
                            cepasVersion = responseInfo[NETSConstants.MessageData.CEPAS_VERSION.Name];
                        }

                        transData = new TransDataFormat(str, cepasVersion);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TRANSACTION_DATE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        DateTime dt = DateTime.Now;
                        DateTime.TryParseExact(str, NETSConstants.TRANSACTION_DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TRANSACTION_DATE.Name, dt.ToString(NETSConstants.DISPLAY_DATE_FORMAT));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TRANSACTION_TIME.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        DateTime dt = DateTime.Now;
                        DateTime.TryParseExact(str, NETSConstants.TRANSACTION_TIME_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TRANSACTION_TIME.Name, dt.ToString(NETSConstants.DISPLAY_TIME_FORMAT));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CASHBACK_SERVICE_FEE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CASHBACK_SERVICE_FEE.Name, Utilities.stringToCurrencyFormat(str));
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RFU_31.FieldCode))
                    {
                        //Most RFU fields are not in used, i.e. with zero in it, in future might be in use
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RFU_31.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RFU_D4.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RFU_D4.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.FOREIGN_AMOUNT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.FOREIGN_AMOUNT.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.FOREIGN_MID.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.FOREIGN_MID.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.TERMINATE_CERTIFICATE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.TERMINATE_CERTIFICATE.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RFU_H6.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RFU_H6.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CASHCARD_TRANSACTION_COUNTER.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CASHCARD_TRANSACTION_COUNTER.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.BLACKLIST_VERSION.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.BLACKLIST_VERSION.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.SIGN_CERTIFICATE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.SIGN_CERTIFICATE.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CHECKSUM.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CHECKSUM.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.RFU_HB.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.RFU_HB.Name, str);
                    }
                    else if (fieldCode.Equals(NETSConstants.MessageData.CASHCARD_BALANCE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(NETSConstants.MessageData.CASHCARD_BALANCE.Name, Utilities.stringToCurrencyFormat(str));
                    }

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
            addKeyValueToResponseInfo(NETSConstants.ResponseCode.NAME, "");
        }

        /// <summary>
        /// Function to generate LRC for each packet
        /// </summary>
        /// <param name="data">Bytes array</param>
        /// <returns>1 byte of LRC</returns>
        protected byte ComputeLRC(byte[] data)
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
            byte[] contentLength = Utilities.intToBytesBCD(content.Length, NETSConstants.MSG_LENGTH_BYTES_COUNT);

            //Inclue STX, ETX, LRC and length of content in full packet
            byte[] fullPacket = new byte[3 + contentLength.Length + content.Length];

            int idx = 0;

            fullPacket[idx++] = NETSConstants.STX;

            Array.Copy(contentLength, 0, fullPacket, idx, contentLength.Length);
            idx += contentLength.Length;

            Array.Copy(content, 0, fullPacket, idx, content.Length);
            idx += content.Length;

            fullPacket[idx++] = NETSConstants.ETX;
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
            byte[] content = new byte[NETSConstants.MessageHeader.REQUEST_LENGTH];

            int idx = 0;

            byte[] temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageHeader.HEADER_FILLER);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(functionCode);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageHeader.RFU);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageHeader.END_OF_MSG);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            content[idx++] = NETSConstants.MessageHeader.SEPARATOR;

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
            byte[] retval = new byte[NETSConstants.MessageHeader.RESP_CODE_LENGTH];
            int startIdx = 3 + //3 for (STX + 2 bytes LEN)
                NETSConstants.MessageHeader.HEADER_FILLER_LENGTH +
                NETSConstants.MessageHeader.FunctionCode.LENGTH;

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
