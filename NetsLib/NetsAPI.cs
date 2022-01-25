﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Contains :
    /// <para>Default settings for serial port</para>
    /// <para>Packets which needed for sending appropriate bytes array to nets machine</para>
    /// </summary>
    class NetsAPI
    {
        public const int DEFAULT_BAUDRATE = 9600;
        public const Parity DEFAULT_PARITY = Parity.None;
        public const int DEFAULT_DATABITS = 8;
        public const StopBits DEFAULT_STOPBITS = StopBits.One;
        public const Handshake DEFAULT_HANDSHAKE = Handshake.None;
        public const int DEFAULT_TIMEOUT = 2000;
        public const int DEFAULT_RETRIES = 3;

        public static ACKPacket ACK = new ACKPacket();
        public static NACKPacket NACK = new NACKPacket();

        public static RequestTerminalStatusPacket REQUEST_TERMINAL_STATUS = new RequestTerminalStatusPacket();
        public static ContactlessOfflineCardEnquiryPacket CONTACTLESS_OFFLINE_CARD_ENQUIRY = new ContactlessOfflineCardEnquiryPacket();
        public static ContactlessDebitPacket CONTACTLESS_DEBIT = new ContactlessDebitPacket();
        public static NETSPurchasePacket NETS_PURCHASE = new NETSPurchasePacket();
        public static CashcardPurchasePacket CASHCARD_PURCHASE = new CashcardPurchasePacket();
        public static CreditCardPurchasePacket CREDITCARD_PURCHASE = new CreditCardPurchasePacket();
        public static UOBCreditCardPurchasePacket UOBCREDITCARD_PURCHASE = new UOBCreditCardPurchasePacket();
        public static NETSPurchaseCashbackPacket NETS_PURCHASECASHBACK = new NETSPurchaseCashbackPacket();
        public static UOBCreditCardPassThroughPacket UOBCREDITCARD_PASSTHROUGH = new UOBCreditCardPassThroughPacket();

        public static CashcardPurchaseServiceFeePacket CASHCARD_PURCHASE_SERVICE_FEE = new CashcardPurchaseServiceFeePacket();
        public static BCAPurchasePacket BCA_PURCHASE = new BCAPurchasePacket();
        public static UnionPayPacket UNION_PAY = new UnionPayPacket();
        public static CUPPurchasePacket CUP_PURCHASE = new CUPPurchasePacket();
        public static PrepaidPurchasePacket PREPAID_PURCHASE = new PrepaidPurchasePacket();
        public static NETSSwitchPrepaidPacket NETSSwitchPrepaidPacket = new NETSSwitchPrepaidPacket();
        public static NETSSwitchPrepaidBackPacket NETSSwitchPrepaidBackPacket = new NETSSwitchPrepaidBackPacket();  

        //ECR 3
        public static NETS3CheckStatusPacket NETS3_CHECK_STATUS = new NETS3CheckStatusPacket();
        public static NETSPurchasePacketECR3 NETS3_PURCHASE = new NETSPurchasePacketECR3();

        public static UOBAliPayPacket UOB_ALIPAY = new UOBAliPayPacket();
        public static UOBWechatPayPacket UOB_WECHATPAY = new UOBWechatPayPacket();
        public static UOBUPIPacket UOB_UPI = new UOBUPIPacket();
        public static UOBQRInquiryPacket UOB_QRINQUIRY = new UOBQRInquiryPacket();

        public static TestPacket NETS_TEST = new TestPacket();

    }
}
