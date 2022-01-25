using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Contains most of the constant used for <see cref="NetsAPI"/>
    /// </summary>
    class NETSConstants
    {
        public const byte STX = 0x02;
        public const byte ETX = 0x03;
        public const byte ETXECR3 = 0x04;
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
        public const int ECR3_MSG_LENGTH_BYTES_COUNT = 4;

        /// <summary>
        /// Message Header Type
        /// </summary>
        public class MessageHeader
        {
            public const int HEADER_FILLER_LENGTH = 12;
            public const int RESP_CODE_LENGTH = 2;
            public const int RFU_LENGTH = 2;
            public const int END_OF_MSG_LENGTH = 1;
            public const int SEPARATOR_LENGTH = 1;
            public const int RFUNEW_LENGTH = 1;

            public const int REQUEST_LENGTH = HEADER_FILLER_LENGTH + FunctionCode.LENGTH + RFU_LENGTH + END_OF_MSG_LENGTH + SEPARATOR_LENGTH;
            public const int RESPONSE_LENGTH = HEADER_FILLER_LENGTH + FunctionCode.LENGTH + RESP_CODE_LENGTH + END_OF_MSG_LENGTH + SEPARATOR_LENGTH;
            public const int UOB_PASSTHROUGH_RESPONSE_LENGTH = HEADER_FILLER_LENGTH + FunctionCode.LENGTH + RESP_CODE_LENGTH + END_OF_MSG_LENGTH + SEPARATOR_LENGTH;

            public const string HEADER_FILLER = "000000000000";
            public const string HEADER_FILLER_UOB = "600000000010";
            public const string RFU = "00";
            public const string RFUNEW = "0";
            public const string VERSION_TYPE = "01";
            public const string END_OF_MSG = "0";
            public const byte SEPARATOR = (byte)0x1C;

            //ECR3
            public const int ECR3_MSGVERSION_LENGTH = 1;
            public const int ECR3_MSGDIRECTION_LENGTH = 1;
            public const int ECR3_CRC32_LENGTH = 4;
            public const int ECR3_MSGTIME_LENGTH = 8;
            public const int ECR3_MSGSEQUENCE_LENGTH = 4;
            public const int ECR3_MSGCLASS_LENGTH = 2;
            public const int ECR3_MSGTYPE_LENGTH = 4;
            public const int ECR3_MSGCODE_LENGTH = 4;
            public const int ECR3_MSGCOMPLETION_LENGTH = 1;
            public const int ECR3_MSGNOTIFICATION_LENGTH = 1;
            public const int ECR3_MSGSTATUS_LENGTH = 4;
            public const int ECR3_DEVICEPROVIDER_LENGTH = 2;
            public const int ECR3_DEVICETYPE_LENGTH = 2;
            public const int ECR3_DEVICELOCATION_LENGTH = 1;
            public const int ECR3_DEVICENUMBER_LENGTH = 3;
            public const int ECR3_ENCALGORITHM_LENGTH = 1;
            public const int ECR3_ENCKEYINDEX_LENGTH = 1;
            public const int ECR3_ENCMAC_LENGTH = 8;
            public const int ECR3_RESERVED_LENGTH = 8;
            //public const int ECR3_RESERVED_LENGTH = 8;

            public const int ECR3_REQUEST_LENGTH = 64;
            public const int ECR3_RESPONSE_LENGTH = 64;

            public const string ECR3_MSGVERSION = "00";
            public const string ECR3_MSGDIRECTION = "00";
            public const string ECR3_MSGCLASS = "0001";
            public const string ECR3_MSGTYPE = "10000000";
            public const string ECR3_MSGCODE = "10000000";
            public const string ECR3_MSGCOMPLETION = "01";
            public const string ECR3_MSGNOTIFICATION = "00";
            public const string ECR3_MSGSTATUS = "00000000";
            public const string ECR3_DEVICEPROVIDER = "5702";
            public const string ECR3_DEVICETYPE = "0001";
            public const string ECR3_DEVICENUMBER = "00BC614E";
            public const string ECR3_DEVICELOCATION = "0";
            public const string ECR3_ENCALGORITHM  = "0000";
            public const string ECR3_ENCKEYINDEX = "0";
            public const string ECR3_ENCMAC = "00000000000000000000000000000000";
            public const string ECR3_RESERVED = "00000000";
            public const string ECR3_ENDOFMSG = "04";


            public class NETSPurchaseTransactionType
            {
                public const string NetsPurchase = "01";
                public const string NetsPurchaseWithCashback = "00";
                public const string QRPurchase = "04";
            }

            public class QRMPMType
            {
                public const string ALIPAY = "ALIPAY";
                public const string WECHAT = "WECHAT";
                public const string UPI = "UPI";
            }

            public class FunctionCode
            {
                public const int LENGTH = 2;

                public const string REQUEST_TERMINAL_STATUS = "55";
                public const string REQUEST_TERMINAL_FINANCIAL_TRANSACTION_STATUS = "90";
                public const string LOGON = "80";

                public const string CONTACTLESS_EFTPOS_PURCHASE = "37";
                public const string CONTACTLESS_DEBIT = "24";
                public const string CONTACTLESS_OFFLINE_CARD_ENQUIRY = "71";
                public const string LAST_SUCCESSFUL_CEPAS2_TRANSACTION = "87";

                /* Old function for NETS Bank Card, will be suspended soon.
                 * In future, NETS Card will no longer available, 
                 * and will be integrated into the small chip in every bank card 
                 * (Insert, no swap)
                */
                public const string NETS_PURCHASE = "30"; //New NETS Bank Card Function

                public const string NETS_PURCHASEOLD = "28"; 
                public const string CASHCARD_PURCHASE = "25";
                public const string CASHCARD_PURCHASE_WITH_SERVICE_FEE = "51";
                public const string UNION_PAY = "31";
                public const string BCA_PURCHASE = "65";
                public const string CUP = "29";
                public const string PREPAID_PURCHASE = "70";
                public const string SWITCH_TO_LOYALTY = "59";
                public const string SWITCH_TO_NETS = "58";

                //Credit Card
                public const string CREDITCARD_PURCHASE = "I6";
                public const string CREDITCARD_PURCHASEECR2 = "I0";
                public const string CREDITCARD_PURCHASE_UOB = "20";
                public const string CREDITCARD_PURCHASE_PASSTHROUGH = "90"; // only available in ECR2 

                public const string QRCODE_PAYMENT_UOB = "UE";
                public const string QRCODE_INQUIRY = "49";
            }

            public class ECR3MessageType
            {
                public const string MSG_TYPE_DEVICE = "1000000";
                public const string MSG_TYPE_AUTHENTICATION = "20000000";
                public const string MSG_TYPE_CARD = "30000000";
                public const string MSG_TYPE_PAYMENT = "40000000";
                public const string MSG_TYPE_CANCELLATION = "50000000";
                public const string MSG_TYPE_TOPUP = "60000000";
                public const string MSG_TYPE_RECORD = "70000000";
                public const string MSG_TYPE_OTHER = "F0000000";

            }

            public class ECR3MessageCode
            {
                public const string MSG_CODE_DEVICE_STATUS = "10000000";
                public const string MSG_CODE_PAYMENT_AUTO = "10000000";
            }

            public class ECR3MessageClass
            {
                public const string MSG_CLASS_ACK = "0002";
                public const string MSG_CLASS_RESPONSE = "0003";
            }

            public class ECR3MEssageStatus
            {
                public const string SUCCESS = "00000000";
                public const string PENDING = "00000001";
                public const string TIMEOUT = "00000002";

            }
        }

        /// <summary>
        /// For all constants used in Message Data
        /// <para>Format : Name, Field Code, Len</para>
        /// </summary>
        public class MessageData
        {
            public const char SINGLE_FILLER_CHAR = '0';

            public static ResponseMessageData ECR_REFERENCE_NUMBER = new ResponseMessageData("ECR Reference Number", "HD", 13);
            public static ResponseMessageData TRANSACTION_AMOUNT = new ResponseMessageData("Transaction Amount", "40", 12);
            public static ResponseMessageData TRANSACTION_CASHBACKAMOUNT = new ResponseMessageData("Transaction Amount", "42", 12);
            public static ResponseMessageData TRANSACTION_AMOUNT_PREPAID = new ResponseMessageData("Transaction Amount", "02", 4);
            public static ResponseMessageData RESPONSE_TEXT = new ResponseMessageData("Response Text", "02", 40);
            public static ResponseMessageData MERCHANT_NAME_ADDRESS = new ResponseMessageData("Merchant Name and Address", "D0", 69);
            public static ResponseMessageData TERMINAL_ID = new ResponseMessageData("Terminal ID", "16", 8);
            public static ResponseMessageData MERCHANT_ID = new ResponseMessageData("Merchant ID", "D1", 15);
            public static ResponseMessageData CEPAS_VERSION = new ResponseMessageData("CEPAS Version", "C1", 2);
            public static ResponseMessageData CAN = new ResponseMessageData("CAN", "30", 16); //len is 19 if card is NETS ETFPPOS card
            public static ResponseMessageData EXPIRY_DATE = new ResponseMessageData("Expiry Date", "31", 8);
            public static ResponseMessageData CARD_BALANCE = new ResponseMessageData("Card Balance", "HC", 12);
            public static ResponseMessageData PURSE_STATUS = new ResponseMessageData("Purse Status", "S3", 2);
            public static ResponseMessageData ATU_STATUS = new ResponseMessageData("ATU Status", "S2", 2);
            public static ResponseMessageData ATU_AMOUNT = new ResponseMessageData("ATU Amount", "C8", 12);
            public static ResponseMessageData BATCH_NUMBER = new ResponseMessageData("Batch Number", "50", 12); //Field code 50 is RFU in NETS_PURCHASE, len is 4
            public static ResponseMessageData STAN = new ResponseMessageData("STAN", "65", 6);
            public static ResponseMessageData TOTAL_FEE_TOP_UP = new ResponseMessageData("Total Fee (Top Up)", "ZA", 6);
            public static ResponseMessageData FEE_DUE_TO_MERCHANT_TOP_UP = new ResponseMessageData("Fee Due To Merchant (Top Up)", "ZB", 6);
            public static ResponseMessageData FEE_DUE_FROM_MERCHANT_TOP_UP = new ResponseMessageData("Fee Due From Merchant (Top Up)", "ZC", 6);
            public static ResponseMessageData RESPONSE_MESSAGE_1 = new ResponseMessageData("Response Message I", "R0", 20);
            public static ResponseMessageData RESPONSE_MESSAGE_2 = new ResponseMessageData("Response Message II", "R1", 20);
            public static ResponseMessageData PURCHASE_FEE = new ResponseMessageData("Purchase Fee", "ZP", 6);
            public static ResponseMessageData APPROVAL_CODE = new ResponseMessageData("Approval Code", "01", 6);
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
            public static ResponseMessageData TRANSACTION_TYPE_INDICATOR = new ResponseMessageData("Transaction Type Indicator", "T2", 2);
            public static ResponseMessageData LOYALTY_REDEMPTION = new ResponseMessageData("Loyalty Redemption", "43", 1);
            public static ResponseMessageData ECR_REFERENCE_NUMBER10 = new ResponseMessageData("ECR Reference Number", "H4", 10);
            public static ResponseMessageData ACQUIRER_NAME = new ResponseMessageData("Acquirer Name", "9G", 10);
            public static ResponseMessageData RFU_59 = new ResponseMessageData("RFU Message", "59", 2);
            public static ResponseMessageData AID_EMV = new ResponseMessageData("AID (EMV)", "9A", 16);
            public static ResponseMessageData TVR_EMV = new ResponseMessageData("TVR (EMV)", "9E", 10);
            //public static ResponseMessageData ENTRY_TYPE = new ResponseMessageData("Entry Type", "BC", );
            public static ResponseMessageData INVOICE_NUMBER = new ResponseMessageData("Invoice Number", "9H", 6);
            public static ResponseMessageData NEW_BALANCE_AMOUNT = new ResponseMessageData("New Balance Amount", "C0", 12);
            public static ResponseMessageData PREVIOUS_BALANCE_AMOUNT = new ResponseMessageData("Previous Balance Amount", "C0", 12);
            
            public static ResponseMessageData INVOICE_NO = new ResponseMessageData("INVOICE_NO", "65", 6);
            public static ResponseMessageData UOB_ECR_REFERENCE_NUMBER = new ResponseMessageData("ECR Reference Number", "D8", 19);
            public static ResponseMessageData ENTRY_TYPE = new ResponseMessageData("Entry Type", "98", 10);
            public static ResponseMessageData TRACE_NO = new ResponseMessageData("Trace No", "97", 10);
            public static ResponseMessageData QR_TYPE6 = new ResponseMessageData("QR Type", "35", 6);
            public static ResponseMessageData QR_TYPE3 = new ResponseMessageData("QR Type", "35", 3);

            public class CEPASVersion
            {
                public static KeyValuePair<string, string> CEPAS_1 = new KeyValuePair<string, string>("10", "CEPAS 1.0");
                public static KeyValuePair<string, string> CEPAS_2 = new KeyValuePair<string, string>("20", "CEPAS 2.0");
            }

            public class FieldEncoding
            {
                public const string FIELD_ENCODING_NONE = "30";
                public const string FIELD_ENCODING_ARRAY_ASCII = "31";
                public const string FIELD_ENCODING_ARRAY_ASCII_HEX = "32";
                public const string FIELD_ENCODING_ARRAY_HEX = "33";
                public const string FIELD_ENCODING_VALUE_ASCII = "34";
                public const string FIELD_ENCODING_VALUE_ASCII_HEX = "35";
                public const string FIELD_ENCODING_VALUE_BCD = "36";
                public const string FIELD_ENCODING_VALUE_HEX_BIG = "37";
                public const string FIELD_ENCODING_VALUE_HEX_LITTLE = "38";
            }

            public class PaymentType
            {
                public const string PAYMENT_AUTO = "0000";
                public const string PAYMENT_NETS_EFT= "1000";
                public const string PAYMENT_NETS_NCC = "1100";
                public const string PAYMENT_NETS_NFP = "1200";
                public const string PAYMENT_NETS_RSVP = "1300";
                public const string PAYMENT_NETS_QR = "1400";
                public const string PAYMENT_NETS_BCA = "1600";
                public const string PAYMENT_NETS_EZL = "1700";
                public const string PAYMENT_CREDIT = "2000";
                public const string PAYMENT_DEBIT = "3000";
            }

            public static ECR3ResponseMessageData ID_TXN_TYPE = new ECR3ResponseMessageData("ID TXN TYPE", "5000", NETSConstants.MessageData.FieldEncoding.FIELD_ENCODING_VALUE_HEX_LITTLE);
            public static ECR3ResponseMessageData ID_TXN_AMOUNT = new ECR3ResponseMessageData("ID_TXN_AMOUNT", "5001", NETSConstants.MessageData.FieldEncoding.FIELD_ENCODING_VALUE_HEX_LITTLE);
            public static ECR3ResponseMessageData ID_TXN_MER_REF_NUM = new ECR3ResponseMessageData("ID_TXN_MER_REF_NUM", "500B", NETSConstants.MessageData.FieldEncoding.FIELD_ENCODING_ARRAY_ASCII);
            public static ECR3ResponseMessageData ID_PADDING = new ECR3ResponseMessageData("ID_PADDING", "0000", NETSConstants.MessageData.FieldEncoding.FIELD_ENCODING_ARRAY_HEX);

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
