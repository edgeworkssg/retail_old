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
    class NETSPurchasePacketECR3 : Packet
    {
        private string debitAmount = "0";
        private string cashBackAmoutt = "0"; //since this is created for purchase, not cashback, to be implemented in the future
        private string transactionType = "01";
        private string refno = "";
        private string sequenceNo = "";

        public NETSPurchasePacketECR3()
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
            this.refno = refnos;
        }

        public void setSequenceNo(string seqNo)
        {
            this.sequenceNo = seqNo;
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
            this.debitAmount = Utilities.IntToHexString(Convert.ToInt32(temp * 100),8);
            /*if (debitAmount.Length % 2 == 1)
                debitAmount = "0" + debitAmount;*/
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
            string result = "";
            byte[] content = generateMessageHeaderECR3(NETSConstants.MessageHeader.ECR3MessageType.MSG_TYPE_PAYMENT,  NETSConstants.MessageHeader.ECR3MessageCode.MSG_CODE_PAYMENT_AUTO, sequenceNo, DateTime.Now, out result);

            string payload = generateMessageData(NETSConstants.MessageData.ID_TXN_TYPE, "0000");
            payload += generateMessageData(NETSConstants.MessageData.ID_TXN_AMOUNT, debitAmount);
            payload += generateMessageData(NETSConstants.MessageData.ID_TXN_MER_REF_NUM, refno);
            
            //-8 for padding length, -8 for field type,RFU and Field Encoding
            int paddingLength = 128 - payload.Length - 8 - 8;
            payload += generateMessageData(NETSConstants.MessageData.ID_PADDING, generatePadding(paddingLength));

            //calculate crc for header + payload
            result += payload;
            string crc = Utilities.CalculateCRC(Utilities.StringToByteArray(result));
            result = "80000000" + Utilities.reverseString(crc) + result;

            result = Utilities.AddTransparency(result);
            byte[] temp = Utilities.StringToByteArray(result);
            byte[] packet = generateFullPacketECR3(temp);

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
