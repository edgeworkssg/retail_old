using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using PowerPOS;


namespace CitiLib
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
                if (responseInfo.ContainsKey(CitiConstants.ResponseCode.NAME))
                {
                    responseInfo[CitiConstants.ResponseCode.NAME] = responseCode;
                }
                else
                {
                    addKeyValueToResponseInfo(CitiConstants.ResponseCode.NAME, responseCode);
                }

                int idx = 3 + CitiConstants.MessageHeader.RESPONSE_LENGTH; //1 byte STX, 2 bytes message length

                string fieldCode = string.Empty;
                int len = 0;

                while ((idx < response.Length) && (response[idx] != CitiConstants.ETX))
                {
                    //Extract field to determine the process to retrieve response
                    fieldCode = Encoding.ASCII.GetString(new byte[] { response[idx++], response[idx++] });
                    //Extract len to get the correct bytes array from message data in response
                    len = Utilities.bytesBCDToInt(new byte[] { response[idx++], response[idx++] });
                    byte[] temp = new byte[len];
                    Array.Copy(response, idx, temp, 0, len); ;

                    //compare field code and format response
                    if (fieldCode.Equals(CitiConstants.MessageData.RESPONSE_TEXT.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        addKeyValueToResponseInfo(CitiConstants.MessageData.RESPONSE_TEXT.Name, str);
                    }
                    else if (fieldCode.Equals(CitiConstants.MessageData.MERCHANT_NAME_ADDRESS.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(CitiConstants.MessageData.MERCHANT_NAME_ADDRESS.Name, str);
                    }
                    else if (fieldCode.Equals(CitiConstants.MessageData.TERMINAL_ID.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(CitiConstants.MessageData.TERMINAL_ID.Name, str);
                    }
                    else if (fieldCode.Equals(CitiConstants.MessageData.MERCHANT_ID.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(CitiConstants.MessageData.MERCHANT_ID.Name, str);
                    }
                    
                    else if (fieldCode.Equals(CitiConstants.MessageData.APPROVAL_CODE.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(CitiConstants.MessageData.APPROVAL_CODE.Name, str);
                    }
                    else if (fieldCode.Equals(CitiConstants.MessageData.CARD_ISSUER_NAME.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(CitiConstants.MessageData.CARD_ISSUER_NAME.Name, str);
                    }
                    else if (fieldCode.Equals(CitiConstants.MessageData.CARD_ISSUER_ID.FieldCode))
                    {
                        string str = Encoding.ASCII.GetString(temp).Trim();

                        str = Utilities.removeExtraSpaceBtwnWords(str);

                        addKeyValueToResponseInfo(CitiConstants.MessageData.CARD_ISSUER_ID.Name, str);
                    }
                    
                    idx += len + 1; //1 byte for separator
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Processing" + ex.ToString());
                //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            addKeyValueToResponseInfo(CitiConstants.ResponseCode.NAME, "");
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
            byte[] contentLength = Utilities.intToBytesBCD(content.Length, CitiConstants.MSG_LENGTH_BYTES_COUNT);

            //Inclue STX, ETX, LRC and length of content in full packet
            byte[] fullPacket = new byte[3 + contentLength.Length + content.Length];

            int idx = 0;

            fullPacket[idx++] = CitiConstants.STX;

            Array.Copy(contentLength, 0, fullPacket, idx, contentLength.Length);
            idx += contentLength.Length;

            Array.Copy(content, 0, fullPacket, idx, content.Length);
            idx += content.Length;

            fullPacket[idx++] = CitiConstants.ETX;
            fullPacket[idx++] = ComputeLRC(fullPacket);
            
            return fullPacket;
        }

        /// <summary>
        /// Generate full packet based on the content, add in STX, LRC and such
        /// </summary>
        /// <param name="content">byte array</param>
        /// <returns>Full packet byte array</returns>
        protected byte[] generateFullPacketPrepaid(byte[] content)
        {
            byte[] contentLength = Utilities.intToBytesBCD(content.Length, CitiConstants.MSG_LENGTH_BYTES_COUNT);

            //Inclue STX, ETX, LRC and length of content in full packet
            byte[] fullPacket = new byte[3 + contentLength.Length + content.Length];

            int idx = 0;

            fullPacket[idx++] = CitiConstants.STX;

            Array.Copy(contentLength, 0, fullPacket, idx, contentLength.Length);
            idx += contentLength.Length;

            Array.Copy(content, 0, fullPacket, idx, content.Length);
            idx += content.Length;

            fullPacket[idx++] = CitiConstants.ETX;

            

            string hex = BitConverter.ToString(fullPacket).Replace("-", string.Empty);
          //  hex = hex.Substring(

            fullPacket[2] = 53;
            fullPacket[idx++] = ComputeLRC(fullPacket);

            
            return fullPacket;
        }

        /// <summary>
        /// Generate message header byte array, not in full packet yet, i.e. missing STX, LRC etc
        /// <para>All message headers are in same length, same format, just different function code, thus use this function to construct message header</para>
        /// </summary>
        /// <param name="functionCode">Function code to be set into the message header, two characters string</param>
        /// <returns>Message header byte array</returns>
        protected byte[] generateMessageHeader(string functionCode, string transactionType)
        {
            byte[] content = new byte[CitiConstants.MessageHeader.REQUEST_LENGTH];

            int idx = 0;

            byte[] temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageHeader.HEADER_FILLER);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageHeader.FORMAT_VERSION);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(transactionType);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(functionCode);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageHeader.RFU);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageHeader.END_OF_MSG);
            Array.Copy(temp, 0, content, idx, temp.Length);
            idx += temp.Length;

            content[idx++] = CitiConstants.MessageHeader.SEPARATOR;

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
            byte[] retval = new byte[CitiConstants.MessageHeader.RESP_CODE_LENGTH];
            int startIdx = 3 + //3 for (STX + 2 bytes LEN)
                CitiConstants.MessageHeader.HEADER_FILLER_LENGTH +
                CitiConstants.MessageHeader.FunctionCode.LENGTH+CitiConstants.MessageHeader.FORMAT_VERSION_LENGTH + CitiConstants.MessageHeader.RR_INDICATOR_LENGTH;

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

        public string getValue(String KeyValue)
        {
            //string retval = Environment.NewLine + "==============================" + Environment.NewLine;

            foreach (KeyValuePair<string, string> kvp in responseInfo)
            {
                if (kvp.Key == KeyValue)
                    return kvp.Value;
                
            }
            return "";
           
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
