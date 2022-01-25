using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CitiLib
{
    /// <summary>
    /// Packet for NETS purchase, i.e. NETS Swipe debit
    /// <para>Field Code : 28</para>
    /// </summary>
    class CITIPurchasePacket : Packet
    {
        private string debitAmount = "0";
        private string refno = "";
        private string invNo = "";
        private string cashBackAmoutt = "0"; //since this is created for purchase, not cashback, to be implemented in the future

        public CITIPurchasePacket()
        {
            init();
        }

        /// <summary>
        /// Set debit amount so it will be set in the actual packet byte array
        /// <para>Overload : void setDebitAmount(double temp)</para>
        /// </summary>
        /// <param name="debitAmount">Debit amount in string</param>
        public void setDebitAmount(string debitAmount)
        {
            double temp = 0;

            if (double.TryParse(debitAmount, out temp))
            {
                setDebitAmount(temp);
            }
        }

        /// <summary>
        /// Set Ref No max 19 digit
        /// </summary>
        /// <param name="refnos">refnos in string</param>
        public void setRefNo(string refnos) 
        {
            if (refnos.Length == 0)
            {
                this.refno = "                    ";
                return;
            }

            if (refnos.Length > 19)
            {
                this.refno = "                    ";
                return;
            }
            this.refno = refnos;

            /*if (!refnos.Substring(2, 1).Equals(" "))
                this.refno = refnos;
            else
                this.refno = "                    ";*/
        }

        public void setInvNo(string invNos)
        {
            if (invNos.Length == 0)
            {
                this.invNo = "      ";
                return;
            }

            if (invNos.Length > 6)
            {
                this.invNo = invNos.Substring(invNos.Length - 6);
                return;
            }
            this.invNo = invNos;

            /*if (!refnos.Substring(2, 1).Equals(" "))
                this.refno = refnos;
            else
                this.refno = "                    ";*/
        }

        /// <summary>
        /// Set debit amount so it will be set in the actual packet byte array
        /// <para>Overload : void setDebitAmount(string temp)</para>
        /// </summary>
        /// <param name="debitAmount">Debit amount in double</param>
        private void setDebitAmount(double temp)
        {
            this.debitAmount = Convert.ToInt32(temp * 100).ToString().PadLeft(CitiConstants.MessageData.TRANSACTION_AMOUNT.Len, '0');
        }

        public override byte[] toBytes()
        {
            byte[] content = generateMessageHeader(CitiConstants.MessageHeader.FunctionCode.PURCHASE, CitiConstants.MessageHeader.RR_INDICATOR_REQRESP);

            byte[] msgData = new byte[CitiConstants.MessageData.TRANSACTION_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            byte[] msgData2 = new byte[CitiConstants.MessageData.ORDER_NUMBER.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            //byte[] msgData3 = new byte[CitiConstants.MessageData.INVOICE_NUMBER.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

            int idx = 0;
            
            //Set transaction amount field code in message data
            byte[] temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageData.TRANSACTION_AMOUNT.FieldCode);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(CitiConstants.MessageData.TRANSACTION_AMOUNT.Len, CitiConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //set transaction amount in message data
            temp = Encoding.ASCII.GetBytes(this.debitAmount);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //add separator to message data
            msgData[msgData.Length - 1] = CitiConstants.MessageHeader.SEPARATOR;

            #region Message Data 2 (ECR Transaction Code)
            idx = 0; //reset idx

            //Set field code in message data
            temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageData.ORDER_NUMBER.FieldCode);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(CitiConstants.MessageData.ORDER_NUMBER.Len, CitiConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            string myString = this.refno.PadLeft(19, ' ');
            string[] myArray = new string[myString.Length];
            byte[] toBytes = new byte[1];

            for (int i = 0; i < myString.Length; i++)
            {
                myArray[i] = myString[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < CitiConstants.MessageData.ORDER_NUMBER.Len; i++)
            {
                toBytes = Encoding.ASCII.GetBytes(myArray[i]);
                msgData2[idx++] = toBytes[0];
            }

            //add separator to message data
            msgData2[msgData2.Length - 1] = CitiConstants.MessageHeader.SEPARATOR;
            #endregion 
            
            #region Message Data 3 (Invoice No) - Obsolete
            /*idx = 0; //reset idx

            //Set field code in message data
            temp = Utilities.asciiStrToBytesArray(CitiConstants.MessageData.INVOICE_NUMBER.FieldCode);
            Array.Copy(temp, 0, msgData3, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(CitiConstants.MessageData.INVOICE_NUMBER.Len, CitiConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData3, idx, temp.Length);
            idx += temp.Length;

            invNo = invNo.PadLeft(6, ' ');
            myArray = new string[invNo.Length];
            //byte[] toBytes = new byte[1];

            for (int i = 0; i < invNo.Length; i++)
            {
                myArray[i] = invNo[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < CitiConstants.MessageData.INVOICE_NUMBER.Len; i++)
            {
                toBytes = Encoding.ASCII.GetBytes(myArray[i]);
                msgData3[idx++] = toBytes[0];
            }

            //add separator to message data
            msgData3[msgData3.Length - 1] = CitiConstants.MessageHeader.SEPARATOR;*/
            #endregion

            //combine message header and message data
            idx = 0;
            temp = new byte[content.Length + msgData.Length + msgData2.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

            Array.Copy(msgData2, 0, temp, idx, msgData2.Length);
            idx += msgData2.Length;

            //Array.Copy(msgData3, 0, temp, idx, msgData3.Length);
            //idx += msgData3.Length;

            byte[] packet = generateFullPacket(temp);

            return packet;
        }

        public override void processResponse(byte[] response)
        {
            base.processResponse(response);
        }

        public override void reset()
        {
            base.reset();

            debitAmount = "0";
        }
    }
}
