using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Packet for Cashcard purchase, i.e. Normal Cashcard debit
    /// <para>Field Code : 25</para>
    /// </summary>
    class UnionPayPacket : Packet
    {
        private string debitAmount = "0";
        private string refno = "";

        public UnionPayPacket()
        {
            init();
        }

        public void setDebitAmount(string debitAmount)
        {
            double temp = 0;

            if (double.TryParse(debitAmount, out temp))
            {
                setDebitAmount(temp);
            }
        }

        private void setDebitAmount(double temp)
        {
            this.debitAmount = Convert.ToInt32(temp * 100).ToString().PadLeft(NETSConstants.MessageData.TRANSACTION_AMOUNT.Len, '0');
        }

        public void setRefNo(string refnos)
        {
            if (!refnos.Substring(2, 1).Equals(" "))
                this.refno = refnos;
            else
                this.refno = "             ";
        }

        public override byte[] toBytes()
        {
            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.UNION_PAY);

            byte[] msgData = new byte[NETSConstants.MessageData.TRANSACTION_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            byte[] msgData2 = new byte[NETSConstants.MessageData.ECR_REFERENCE_NUMBER.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

            int idx = 0;

            #region Message Data 1 (Transaction amount)
            //Set transaction amount field code in message data
            byte[] temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.TRANSACTION_AMOUNT.FieldCode);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.TRANSACTION_AMOUNT.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //set transaction amount in message data
            temp = Encoding.ASCII.GetBytes(this.debitAmount);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //add separator to message data
            msgData[msgData.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion

            #region Message Data 2 (Cashback Amount)
            idx = 0; //reset idx

            //Set cashback amount field code in message data
            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.ECR_REFERENCE_NUMBER.FieldCode);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.ECR_REFERENCE_NUMBER.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            string myString = this.refno;
            string[] myArray = new string[myString.Length];
            byte[] toBytes = new byte[1];

            for (int i = 0; i < myString.Length; i++)
            {
                myArray[i] = myString[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < NETSConstants.MessageData.ECR_REFERENCE_NUMBER.Len; i++)
            {
                toBytes = Encoding.ASCII.GetBytes(myArray[i]);
                msgData2[idx++] = toBytes[0];
            }

            //add separator to message data
            msgData2[msgData2.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion

            //combine message header, message data 1, message data 2
            idx = 0;
            temp = new byte[content.Length + msgData.Length + msgData2.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

            Array.Copy(msgData2, 0, temp, idx, msgData2.Length);
            idx += msgData2.Length;

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

            debitAmount = "0".PadLeft(NETSConstants.MessageData.TRANSACTION_AMOUNT.Len, '0');
        }
    }
}
