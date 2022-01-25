using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Packet for Credit Card purchase, i.e. MasterCard
    /// <para>Field Code : I0</para>
    /// </summary>
    class CreditCardPurchasePacket : Packet
    {
        private string debitAmount = "0";
        private string cashBackAmoutt = "0"; //since this is created for purchase, not cashback, to be implemented in the future

        public CreditCardPurchasePacket()
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
        /// Set debit amount so it will be set in the actual packet byte array
        /// <para>Overload : void setDebitAmount(string temp)</para>
        /// </summary>
        /// <param name="debitAmount">Debit amount in double</param>
        private void setDebitAmount(double temp)
        {
            this.debitAmount = Convert.ToInt32(temp * 100).ToString().PadLeft(NETSConstants.MessageData.TRANSACTION_AMOUNT.Len, '0');
        }

        public override byte[] toBytes()
        {
            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.CREDITCARD_PURCHASE);

            byte[] msgData = new byte[NETSConstants.MessageData.TRANSACTION_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
           // byte[] msgData2 = new byte[NETSConstants.MessageData.CASH_BACK_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

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

            //#region Message Data 2 (Cashback Amount)
            //idx = 0; //reset idx

            ////Set cashback amount field code in message data
            //temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.CASH_BACK_AMOUNT.FieldCode);
            //Array.Copy(temp, 0, msgData2, idx, temp.Length);
            //idx += temp.Length;

            ////set message data length
            //temp = Utilities.intToBytesBCD(NETSConstants.MessageData.CASH_BACK_AMOUNT.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            //Array.Copy(temp, 0, msgData2, idx, temp.Length);
            //idx += temp.Length;

            ////set cashback amount in message data, set all to "0"
            //for (int i = 0; i < NETSConstants.MessageData.CASH_BACK_AMOUNT.Len; i++)
            //{
            //    msgData2[idx++] = Convert.ToByte(NETSConstants.MessageData.SINGLE_FILLER_CHAR);
            //}

            ////add separator to message data
            //msgData2[msgData2.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            //#endregion

            //combine message header, message data 1, message data 2
            //idx = 0;
            //temp = new byte[content.Length + msgData.Length + msgData2.Length];
            //Array.Copy(content, 0, temp, idx, content.Length);
            //idx += content.Length;

            //Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

            //Array.Copy(msgData2, 0, temp, idx, msgData2.Length);
            //idx += msgData2.Length;

            byte[] packet = generateFullPacket(msgData);

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
