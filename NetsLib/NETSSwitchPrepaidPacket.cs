using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Packet for NETS purchase, i.e. NETS Swipe debit
    /// <para>Field Code : 28</para>
    /// </summary>
    class NETSSwitchPrepaidPacket : Packet
    {
        private string debitAmount = "0";
        private string cashBackAmoutt = "0"; //since this is created for purchase, not cashback, to be implemented in the future

        public NETSSwitchPrepaidPacket()
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
            reset();

            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.SWITCH_TO_LOYALTY);

            //byte[] msgData = new byte[NETSConstants.MessageData.RFU_00.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

            int idx = 0;
            
            ////Set transaction amount field code in message data
            //byte[] temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.RFU_59.FieldCode);
            //Array.Copy(temp, 0, msgData, idx, temp.Length);
            //idx += temp.Length;

            ////set message data length
            //temp = Utilities.intToBytesBCD(NETSConstants.MessageData.TRANSACTION_AMOUNT.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            //Array.Copy(temp, 0, msgData, idx, temp.Length);
            //idx += temp.Length;

            ////set transaction amount in message data
            //temp = Encoding.ASCII.GetBytes(this.debitAmount);
            //Array.Copy(temp, 0, msgData, idx, temp.Length);
            //idx += temp.Length;

            ////add separator to message data
            //msgData[msgData.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;

            //combine message header and message data
            idx = 0;
            byte[] temp = new byte[content.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            /*Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;*/

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
