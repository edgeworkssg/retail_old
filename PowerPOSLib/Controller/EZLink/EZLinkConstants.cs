using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.EZLink
{
    /// <summary>
    /// Contains most of the constant used for <see cref="NetsAPI"/>
    /// </summary>
    class EZLinkConstants
    {
        public const byte STX = 0x02;
        public const byte ETX = 0x03;
        public const byte ACK = 0x06;
        public const byte NACK = 0x15;

        public const string RESPONSE_DATE_FORMAT = "ddMMyyyy";
        public const string DISPLAY_DATE_FORMAT = "dd/MM/yyyy";
        public const string DISPLAY_TIME_FORMAT = "HH:mm:ss";
        public const string BATCH_NUMBER_DATETIME_FORMAT = TRANSACTION_DATE_FORMAT + TRANSACTION_TIME_FORMAT;
        public const string TRANSACTION_DATE_FORMAT = "yyMMdd";
        public const string TRANSACTION_TIME_FORMAT = "HHmmss";

        public const int MAX_TRIALS_NUM = 3;

        /// <summary>
        /// Message Length in the packet is 2 byte BCD
        /// </summary>
        public const int MSG_LENGTH_BYTES_COUNT = 2;

        /// <summary>
        /// Message Header Type
        /// </summary>
        public class MessageHeader
        {
            public const int HEADER_FILLER_LENGTH = 14;
            public const int RESP_CODE_LENGTH = 2;
            public const int RFU_LENGTH = 0;
            public const int END_OF_MSG_LENGTH = 1;
            public const int SEPARATOR_LENGTH = 1;

            public const int REQUEST_LENGTH = HEADER_FILLER_LENGTH + RESP_CODE_LENGTH + END_OF_MSG_LENGTH + SEPARATOR_LENGTH;
            public const int RESPONSE_LENGTH = HEADER_FILLER_LENGTH + RESP_CODE_LENGTH + END_OF_MSG_LENGTH + SEPARATOR_LENGTH;

            public const string HEADER_FILLER = "60000000001120";
            public const string RFU = "00"; 
            public const string END_OF_MSG = "0";
            public const byte SEPARATOR = (byte)0x1C;

            public class FunctionCode
            {
                public const int LENGTH = 2;

                //public const string REQUEST_TERMINAL_STATUS = "55";
                //public const string REQUEST_TERMINAL_FINANCIAL_TRANSACTION_STATUS = "90";
                //public const string LOGON = "80";

                public const string EZLINK_PURCHASE = "01";
                public const string EZLINK_CONTACTLESSPURCHASE = "02";
                public const string EZLINK_CREDITCARDPURCHASE = "03";
                //public const string CONTACTLESS_OFFLINE_CARD_ENQUIRY = "71";
                //public const string LAST_SUCCESSFUL_CEPAS2_TRANSACTION = "87";
                //public const string NETS_PURCHASE = "28";
                //public const string CASHCARD_PURCHASE = "25";
            }
        }

        /// <summary>
        /// For all constants used in Message Data
        /// <para>Format : Name, Field Code, Len</para>
        /// </summary>
        public class MessageData
        {
            public static ResponseMessageData PRODUCT_CODE = new ResponseMessageData("Product Code", "33", 20);
            public static ResponseMessageData TRANSACTION_AMOUNT = new ResponseMessageData("Transaction Amount", "40", 12);
            public static ResponseMessageData FUNCTION_CODEMSG = new ResponseMessageData("Function Code", "34", 2);
            public static ResponseMessageData RESPONSE_TEXT = new ResponseMessageData("Response Text", "02", 40);
            public static ResponseMessageData MERCHANT_NAME_ADDRESS = new ResponseMessageData("Merchant Name and Address", "D0", 69);
            public static ResponseMessageData TERMINAL_ID = new ResponseMessageData("Terminal ID", "16", 8);
            public static ResponseMessageData MERCHANT_ID = new ResponseMessageData("Merchant ID", "D1", 15);
            public static ResponseMessageData CEPAS_VERSION = new ResponseMessageData("CEPAS Version", "C1", 2);
            public static ResponseMessageData CAN = new ResponseMessageData("CAN", "30", 16); //len is 19 if card is NETS ETFPPOS card
            //public static ResponseMessageData EXPIRY_DATE = new ResponseMessageData("Expiry Date", "C2", 8);
            public static ResponseMessageData CARD_BALANCE = new ResponseMessageData("Card Balance", "HC", 12);
            public static ResponseMessageData PURSE_STATUS = new ResponseMessageData("Purse Status", "S3", 2);
            public static ResponseMessageData ATU_STATUS = new ResponseMessageData("ATU Status", "S2", 2);
            public static ResponseMessageData ATU_AMOUNT = new ResponseMessageData("ATU Amount", "C8", 12);
            public static ResponseMessageData BATCH_NUMBER = new ResponseMessageData("Batch Number", "50", 12); //Field code 50 is RFU in NETS_PURCHASE, len is 4
            public static ResponseMessageData STAN = new ResponseMessageData("STAN", "H6", 6);
            public static ResponseMessageData TOTAL_FEE_TOP_UP = new ResponseMessageData("Total Fee (Top Up)", "ZA", 6);
            public static ResponseMessageData FEE_DUE_TO_MERCHANT_TOP_UP = new ResponseMessageData("Fee Due To Merchant (Top Up)", "ZB", 6);
            public static ResponseMessageData FEE_DUE_FROM_MERCHANT_TOP_UP = new ResponseMessageData("Fee Due From Merchant (Top Up)", "ZC", 6);
            public static ResponseMessageData RESPONSE_MESSAGE_1 = new ResponseMessageData("Response Message I", "R0", 20);
            public static ResponseMessageData RESPONSE_MESSAGE_2 = new ResponseMessageData("Response Message II", "R1", 20);
            public static ResponseMessageData PURCHASE_FEE = new ResponseMessageData("Purchase Fee", "ZP", 6);
            
            public static ResponseMessageData CARD_ISSUER_NAME = new ResponseMessageData("Card Issuer Name", "D2", 10);
            public static ResponseMessageData ADDITIONAL_TRANS_DATA = new ResponseMessageData("Additional Transaction Information", "ZT", 19);
            public static ResponseMessageData RETRIEVAL_REF_NUM = new ResponseMessageData("Retrieval Reference Number", "D3", 12);
            public static ResponseMessageData TRANS_DATA = new ResponseMessageData("Trans Data", "C0", 64);
            public static ResponseMessageData CASH_BACK_AMOUNT = new ResponseMessageData("Cash Back Amount", "42", 12);
            public static ResponseMessageData TRANSACTION_DATE = new ResponseMessageData("Transaction Date", "03", 6);
            public static ResponseMessageData TRANSACTION_TIME = new ResponseMessageData("Transaction Time", "04", 6);
            public static ResponseMessageData CASHBACK_SERVICE_FEE = new ResponseMessageData("Cashback/Service Fee", "41", 12);
            public static ResponseMessageData RFU_31 = new ResponseMessageData("RFU_31", "31", 6);
            public static ResponseMessageData RFU_D4 = new ResponseMessageData("RFU_D4", "D4", 2);
            public static ResponseMessageData FOREIGN_AMOUNT = new ResponseMessageData("Foreign Amount", "FA", 12);
            public static ResponseMessageData FOREIGN_MID = new ResponseMessageData("Foreign MID", "F0", 15);
            public static ResponseMessageData TERMINATE_CERTIFICATE = new ResponseMessageData("Terminate Certificate (TTC Cert.)", "H5", 6);
            public static ResponseMessageData RFU_H6 = new ResponseMessageData("RFU_H6", "H6", 6);
            public static ResponseMessageData CASHCARD_TRANSACTION_COUNTER = new ResponseMessageData("Cashcard Transaction Counter (CTC)", "H7", 6);
            public static ResponseMessageData BLACKLIST_VERSION = new ResponseMessageData("Blacklist Version", "H8", 6);
            public static ResponseMessageData SIGN_CERTIFICATE = new ResponseMessageData("Certificate (Sign Certificate)", "H9", 4);
            public static ResponseMessageData CHECKSUM = new ResponseMessageData("Checksum", "HA", 2);
            public static ResponseMessageData RFU_HB = new ResponseMessageData("RFU_HB", "HB", 8);
            public static ResponseMessageData CASHCARD_BALANCE = new ResponseMessageData("Cashcard Balance", "HC", 12);

            //EZLink
            //public static ResponseMessageData APPROVAL_CODE = new ResponseMessageData("Approval Code", "01", 6);
            public static ResponseMessageData APPROVAL_CODE = new ResponseMessageData("Approval Code", "01", 6);
            public static ResponseMessageData TRACE_NUMBER = new ResponseMessageData("Trace Number", "65", 6);
            public static ResponseMessageData CARD_NUMBER = new ResponseMessageData("Card Number", "30", 16);
            public static ResponseMessageData EXPIRY_DATE = new ResponseMessageData("Expiry Date", "31", 4);
            public static ResponseMessageData ISSUER_NAME = new ResponseMessageData("Issuer Name", "32", 10);
            public static ResponseMessageData DEDUCTED_AMOUNT = new ResponseMessageData("Deducted Amount", "41", 12);
            public static ResponseMessageData DISCOUNT_AMOUNT = new ResponseMessageData("Discount Amount", "42", 12);
            public static ResponseMessageData CARD_BALANCEAMOUNT = new ResponseMessageData("Card Balance Amount", "43", 12);
            //public static ResponseMessageData CARD_BALANCEAMOUNT = new ResponseMessageData("Card Balance Amount", "43", 12);
            public static ResponseMessageData FAIL_REASON = new ResponseMessageData("Fail Reason", "50", 2);

            public class CEPASVersion
            {
                public static KeyValuePair<string, string> CEPAS_1 = new KeyValuePair<string, string>("10", "CEPAS 1.0");
                public static KeyValuePair<string, string> CEPAS_2 = new KeyValuePair<string, string>("20", "CEPAS 2.0");
            }
        }

        public class ResponseCode
        {
            public const string NAME = "Response Code";
            /*
             * only need APPROVED, the rest is considered as NOT APPROVED, 
             * therefore shall be displayed as "Transaction Declined"
             */
            public const string APPROVED = "00";
        }

        public class SerialPortResult
        {
            public const int SUCCESS = 0;
            public const int FAILED_HW = 1;
            public const int WRONG_CHECKSUM = 2;
            public const int FAILED_EXCEPTION = 3;
        }
    }
}
