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
    class UOBAliPayPacket : Packet
    {
        private string debitAmount = "0";
        private string refno = "";
        private string uniqueNo = "";
        private string qrtype = NETSConstants.MessageHeader.QRMPMType.ALIPAY;

        public UOBAliPayPacket()
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

        public void setRefNo(string refnos)
        {
            if (!refnos.Substring(2,1).Equals(" "))
                this.refno = (refnos + "                  ").Trim().Left(19);
            else
                this.refno = "0000000000000000000";
        }

        public void setUniqueNo(string _uniqueNo)
        {
            if (!string.IsNullOrEmpty(uniqueNo))
                this.uniqueNo = ("000000" + _uniqueNo).Trim().Right(6);
            else
                this.uniqueNo = "000000";
        }

        public void setQRType(string qrType)
        {
            qrtype = qrType;
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
            string FunctionCode = NETSConstants.MessageHeader.FunctionCode.QRCODE_PAYMENT_UOB;

            byte[] content = generateMessageHeaderUOB(FunctionCode);

            byte[] msgData = new byte[NETSConstants.MessageData.TRANSACTION_AMOUNT.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            byte[] msgData2 = new byte[NETSConstants.MessageData.UOB_ECR_REFERENCE_NUMBER.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator
            byte[] msgData4 = new byte[NETSConstants.MessageData.QR_TYPE6.Len + 5]; //2 bytes field code, 2 bytes len, 1 byte separator

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

            #region Message Data 2 (ECR Ref No)
            idx = 0; //reset idx

            //Set cashback amount field code in message data
            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.UOB_ECR_REFERENCE_NUMBER.FieldCode);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.UOB_ECR_REFERENCE_NUMBER.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData2, idx, temp.Length);
            idx += temp.Length;

            string myString = "00000000000000000000";
            if (!String.IsNullOrEmpty(this.refno))
                myString = ("00000000000000000000" + refno).Trim().Right(19);

            string[] myArray = new string[myString.Length];
            byte[] toBytes = new byte[1];

            for (int i = 0; i < myString.Length; i++)
            {
                myArray[i] = myString[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < NETSConstants.MessageData.UOB_ECR_REFERENCE_NUMBER.Len; i++)
            {
                toBytes = Encoding.ASCII.GetBytes(myArray[i]);
                msgData2[idx++] = toBytes[0];
            }

            //add separator to message data
            msgData2[msgData2.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion


            #region Message Data 4 (QRType)
            idx = 0; //reset idx

            //Set cashback amount field code in message data
            temp = Utilities.asciiStrToBytesArray(NETSConstants.MessageData.QR_TYPE6.FieldCode);
            Array.Copy(temp, 0, msgData4, idx, temp.Length);
            idx += temp.Length;

            //set message data length
            temp = Utilities.intToBytesBCD(NETSConstants.MessageData.QR_TYPE6.Len, NETSConstants.MSG_LENGTH_BYTES_COUNT);
            Array.Copy(temp, 0, msgData4, idx, temp.Length);
            idx += temp.Length;

            
            myArray = new string[qrtype.Length];
            toBytes = new byte[1];

            for (int i = 0; i < qrtype.Length; i++)
            {
                myArray[i] = qrtype[i].ToString();
            }

            //set cashback amount in message data, set all to "0"
            for (int i = 0; i < NETSConstants.MessageData.QR_TYPE6.Len; i++)
            {
                toBytes = Encoding.ASCII.GetBytes(myArray[i]);
                msgData4[idx++] = toBytes[0];
            }

            //add separator to message data
            msgData4[msgData4.Length - 1] = NETSConstants.MessageHeader.SEPARATOR;
            #endregion

            //combine message header, message data 1, message data 2
            idx = 0;
            temp = new byte[content.Length + msgData.Length + msgData2.Length + msgData4.Length];
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;

            Array.Copy(msgData, 0, temp, idx, msgData.Length);
            idx += msgData.Length;

            Array.Copy(msgData2, 0, temp, idx, msgData2.Length);
            idx += msgData2.Length;

            //Array.Copy(msgData3, 0, temp, idx, msgData3.Length);
            //idx += msgData3.Length;

            Array.Copy(msgData4, 0, temp, idx, msgData4.Length);
            idx += msgData4.Length;

            //temp = Utilities.hexStrToBytesArray("36 30 30 30 30 30 30 30 30 30 31 30 55 45 30 30 30 1C 34 30 00 12 30 30 30 30 30 30 30 30 31 32 33 34 1C 44 38 00 09 39 30 30 30 30 33 34 33 33 1C 33 35 00 06 41 4C 49 50 41 59 1C");

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
