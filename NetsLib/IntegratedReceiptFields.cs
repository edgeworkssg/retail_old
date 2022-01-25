using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    public class IntegratedReceiptFields
    {
        public class ReceiptField
        {
            public ReceiptField(string name, string description)
            {
                Name = name;
                Description = description;
            }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public static List<ReceiptField> NETSATMCard()
        {
            List<ReceiptField> lst = new List<ReceiptField>();
            lst.Add(new ReceiptField(NETSConstants.MessageData.MERCHANT_ID.Name, "Merchant ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TERMINAL_ID.Name, "Terminal ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.RETRIEVAL_REF_NUM.Name, "Stan No"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.APPROVAL_CODE.Name, "Approval Code"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_AMOUNT.Name, "Purchase"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_DATE.Name, "Transaction Date"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_TIME.Name, "Transaction Time"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.RESPONSE_TEXT.Name, "Response Text"));
            return lst;
        }

        public static List<ReceiptField> NETSCashCard()
        {
            List<ReceiptField> lst = new List<ReceiptField>();
            lst.Add(new ReceiptField(NETSConstants.MessageData.MERCHANT_ID.Name, "Merchant ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TERMINAL_ID.Name, "Terminal ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.CARD_BALANCE.Name, "New Balance"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.CAN.Name, "CAN Number"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.CASHCARD_TRANSACTION_COUNTER.Name, "CTC"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.SIGN_CERTIFICATE.Name, "Authorisation Code"));
            
            lst.Add(new ReceiptField(NETSConstants.MessageData.TERMINATE_CERTIFICATE.Name, "BCERT"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_DATE.Name, "Transaction Date"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_TIME.Name, "Transaction Time"));
            return lst;
        }

        public static List<ReceiptField> NETSFlashPay()
        {
            List<ReceiptField> lst = new List<ReceiptField>();
            lst.Add(new ReceiptField(NETSConstants.MessageData.CAN.Name, "CAN"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.MERCHANT_ID.Name, "Merchant ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TERMINAL_ID.Name, "Terminal ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.PREVIOUS_BALANCE_AMOUNT.Name, "Current Balance"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_AMOUNT.Name, "Purchase"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.NEW_BALANCE_AMOUNT.Name, "New Balance"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_DATE.Name, "Transaction Date"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_TIME.Name, "Transaction Time"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.RESPONSE_TEXT.Name, "Response Text"));
            return lst;
        }

        public static List<ReceiptField> NETSCreditCard()
        {
            List<ReceiptField> lst = new List<ReceiptField>();
            lst.Add(new ReceiptField(NETSConstants.MessageData.MERCHANT_ID.Name, "Merchant ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TERMINAL_ID.Name, "Terminal ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.AID_EMV.Name, "App ID"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.STAN.Name, "Invoice No"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRACE_NO.Name, "Trace No"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.APPROVAL_CODE.Name, "Approval Code"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.BATCH_NUMBER.Name, "Batch Number"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.CAN.Name, "CAN Number"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.EXPIRY_DATE.Name, "Expiry Date"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.INVOICE_NUMBER.Name, "Invoice Number"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.RETRIEVAL_REF_NUM.Name, "RRN"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_DATE.Name, "Transaction Date"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TRANSACTION_TIME.Name, "Transaction Time"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.TVR_EMV.Name, "TVR (EMV)"));
            lst.Add(new ReceiptField(NETSConstants.MessageData.CARD_ISSUER_NAME.Name, "Card Issuer Name"));

            lst.Add(new ReceiptField(NETSConstants.MessageData.ENTRY_TYPE.Name, "Entry Type"));  // TEMPORARY
            return lst;
        }
    }
}
