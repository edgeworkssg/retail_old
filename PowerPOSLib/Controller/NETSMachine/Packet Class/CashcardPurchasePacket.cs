using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETSApplication
{
    /// <summary>
    /// Packet for Cashcard purchase, i.e. Normal Cashcard debit
    /// <para>Field Code : 25</para>
    /// </summary>
    class CashcardPurchasePacket : Packet
    {
        private string debitAmount = "0";

        public CashcardPurchasePacket()
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

        public override byte[] toBytes()
        {
            reset();

            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.CASHCARD_PURCHASE);

            byte[] msgData = new byte[NETSConstants.MessageData.TRANSACTION_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

            int idx = 0;

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

            //combine message header and message data
            idx = 0;
            temp = new byte[content.Length + msgData.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

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
