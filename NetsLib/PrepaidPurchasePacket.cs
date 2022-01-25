using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Packet for NETS purchase, i.e. NETS Swipe debit
    /// <para>Field Code : 28</para>
    /// </summary>
    class PrepaidPurchasePacket : Packet
    {
        private string debitAmount = "0";
        
        public PrepaidPurchasePacket()
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
            this.debitAmount = Convert.ToInt32(temp * 100).ToString().PadLeft(8, '0');
            Logger.writeLog(this.debitAmount.ToString());
        }

        public override byte[] toBytes()
        {
            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.PREPAID_PURCHASE);
            byte[] msgData = new byte[NETSConstants.MessageData.TRANSACTION_AMOUNT_PREPAID.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

            int idx = 0;
            #region Message Data 1 (Transaction amount)
            //Set transaction amount field code in message data
            byte[] temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.TRANSACTION_AMOUNT_PREPAID.FieldCode);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.TRANSACTION_AMOUNT_PREPAID.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //set transaction amount in message data
            //debitAmount = Regex.Replace(debitAmount, ".{2}", "$0/");
            temp = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byte test = Convert.ToByte(debitAmount.Substring(i*2,2), 16);
                temp[i] = test;
            }
                 //debitAmount.Split('/').Select(s => Convert.ToByte(s, 16)).ToArray();
            
            Array.Copy(temp, 0, msgData, idx, temp.Length);
            idx += temp.Length;

            //add separator to message data
            msgData[msgData.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion
            idx = 0;
            temp = new byte[content.Length + msgData.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

            byte[] packet = generateFullPacket(temp);

            /*string stx = "02";
            string msg = "003530303030303030303030303037303030301c30323038" + this.debitAmount + "1c03";
           
            string lrc = GetLRCChecksum(msg);
            string allMsg = stx + msg + lrc;
            allMsg = Regex.Replace(allMsg, ".{2}", "$0/");
            allMsg = allMsg.Remove(allMsg.Length - 1);
            byte[] packet = allMsg.Split('/').Select(s => Convert.ToByte(s, 16)).ToArray();
                     */
            return packet;
        }

        public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
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
