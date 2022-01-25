using System;
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
        public static CityBankCreditCardPurchasePacket CITYBANK_CREDITCARD_PURCHASE = new CityBankCreditCardPurchasePacket();
    }
}
