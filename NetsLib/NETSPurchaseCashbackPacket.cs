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
    class NETSPurchaseCashbackPacket : Packet
    {
        private string debitAmount = "0";
        private string cashBackAmoutt = "0"; //since this is created for purchase, not cashback, to be implemented in the future
        private string transactionType = "01";
        private string refno = "";

        public NETSPurchaseCashbackPacket()
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

        public void setCashbackAmount(string cashbackAmount)
        {
            double temp = 0;

            if (double.TryParse(cashbackAmount, out temp))
            {
                setCashBackAmoutt(temp);
            }
        }

        public void setRefNo(string refnos)
        {
            if (!refnos.Substring(2,1).Equals(" "))
                this.refno = refnos;
            else
                this.refno = "            ";
        }

        public void setTransactionType(string tType)
        {
            if (String.IsNullOrEmpty(tType))
            {
                this.transactionType = "01";
                return;
            }
            this.transactionType = tType;
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

        /// <summary>
        /// Set cashback amount so it will be set in the actual packet byte array
        /// <para>Overload : void setCashBackAmount(string temp)</para>
        /// </summary>
        /// <param name="debitAmount">Debit amount in double</param>
        private void setCashBackAmoutt(double temp)
        {
            this.cashBackAmoutt = Convert.ToInt32(temp * 100).ToString().PadLeft(NETSConstants.MessageData.CASH_BACK_AMOUNT.Len, '0');
        }



        public override byte[] toBytes()
        {
            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.NETS_PURCHASEOLD);

            byte[] msgData = new byte[NETSConstants.MessageData.TRANSACTION_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            byte[] msgData2 = new byte[NETSConstants.MessageData.CASH_BACK_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            byte[] msgData3 = new byte[NETSConstants.MessageData.ECR_REFERENCE_NUMBER10.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            //byte[] msgDataTransactionType = new byte[NETSConstants.MessageData.TRANSACTION_TYPE_INDICATOR.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            //byte[] msgDataLoyaltyRedemption = new byte[NETSConstants.MessageData.LOYALTY_REDEMPTION.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

            int idx = 0;

            #region Message Data -2 (Transaction Type)
            //Set transaction amount field code in message data
            /*byte[] temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.TRANSACTION_TYPE_INDICATOR.FieldCode);
            Array.Copy(temp, 0, msgDataTransactionType, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.TRANSACTION_TYPE_INDICATOR.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgDataTransactionType, idx, temp.Length);
            idx += temp.Length;

            //set transaction amount in message data
            string TransactionType = transactionType;
            string[] TransactionTypeArray = new string[TransactionType.Length];
            byte[] toBytesTemp = new byte[1];
            for (int i = 0; i < TransactionType.Length; i++)
            {
                TransactionTypeArray[i] = TransactionType[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < NETSConstants.MessageData.TRANSACTION_TYPE_INDICATOR.Len; i++)
            {
                toBytesTemp = Encoding.ASCII.GetBytes(TransactionTypeArray[i]);
                msgDataTransactionType[idx++] = toBytesTemp[0];
            }

            //Array.Copy(temp, 0, msgDataTransactionType, idx, temp.Length);
            //idx += temp.Length;

            //add separator to message data
            msgDataTransactionType[msgDataTransactionType.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;*/
            #endregion

            #region Message Data -1 (Loyalty Redemption)
            /*idx = 0;
            //Set transaction amount field code in message data
            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.LOYALTY_REDEMPTION.FieldCode);
            Array.Copy(temp, 0, msgDataLoyaltyRedemption, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.LOYALTY_REDEMPTION.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgDataLoyaltyRedemption, idx, temp.Length);
            idx += temp.Length;

            //set transaction amount in message data
            string LoyaltyRedemptionString = "0";
            string[] LoyaltyArray = new string[LoyaltyRedemptionString.Length];
            //byte[] toBytesTemp = new byte[1];
            for (int i = 0; i < LoyaltyRedemptionString.Length; i++)
            {
                LoyaltyArray[i] = LoyaltyRedemptionString[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < NETSConstants.MessageData.LOYALTY_REDEMPTION.Len; i++)
            {
                toBytesTemp = Encoding.ASCII.GetBytes(LoyaltyArray[i]);
                msgDataLoyaltyRedemption[idx++] = toBytesTemp[0];
            }

            //Array.Copy(temp, 0, msgDataLoyaltyRedemption, idx, temp.Length);
           // idx += temp.Length;

            //add separator to message data
            msgDataLoyaltyRedemption[msgDataLoyaltyRedemption.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;*/
            #endregion

            #region Message Data 1 (Transaction amount)
            idx = 0;
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
            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.CASH_BACK_AMOUNT.FieldCode);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.CASH_BACK_AMOUNT.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            //set transaction amount in message data
            temp = Encoding.ASCII.GetBytes(this.cashBackAmoutt);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            //add separator to message data
            msgData2[msgData2.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion

            #region Message Data 3 (ECR Ref No)
            idx = 0; //reset idx

            //Set cashback amount field code in message data
            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.ECR_REFERENCE_NUMBER10.FieldCode);
            Array.Copy(temp, 0, msgData3, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.ECR_REFERENCE_NUMBER10.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData3, idx, temp.Length);
            idx += temp.Length;

            string myString1 = this.refno.PadLeft(10,'0');
            string[] myArray1 = new string[myString1.Length];
            byte[] toBytes1 = new byte[1];

            for (int i = 0; i < myString1.Length; i++)
            {
                myArray1[i] = myString1[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < NETSConstants.MessageData.ECR_REFERENCE_NUMBER10.Len; i++)
            {
                toBytes1 = Encoding.ASCII.GetBytes(myArray1[i]);
                msgData3[idx++] = toBytes1[0];
            }

            //add separator to message data
            msgData3[msgData3.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion

            //combine message header, message data 1, message data 2
            idx = 0;
            //temp = new byte[content.Length + msgDataTransactionType.Length + msgData.Length + msgData2.Length + msgData3.Length];
            temp = new byte[content.Length + msgData.Length + msgData2.Length + msgData3.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            /*Array.Copy(msgDataTransactionType, 0, temp, idx, msgDataTransactionType.Length);
            idx += msgDataTransactionType.Length;
            */
            //Array.Copy(msgDataLoyaltyRedemption, 0, temp, idx, msgDataLoyaltyRedemption.Length);
            //idx += msgDataLoyaltyRedemption.Length;

            Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

            Array.Copy(msgData2, 0, temp, idx, msgData2.Length);
            idx += msgData2.Length;

            Array.Copy(msgData3, 0, temp, idx, msgData3.Length);
            idx += msgData3.Length;

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
