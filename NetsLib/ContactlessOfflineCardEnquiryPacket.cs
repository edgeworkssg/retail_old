using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Packet for contactless offline card enquiry packet, to read card balance offline
    /// </summary>
    class ContactlessOfflineCardEnquiryPacket : Packet
    {
        public ContactlessOfflineCardEnquiryPacket()
        {
            init();
        }

        public override byte[] toBytes()
        {
            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.CONTACTLESS_OFFLINE_CARD_ENQUIRY);

            return (generateFullPacket(content));
        }

        public override void processResponse(byte[] response)
        {
            base.processResponse(response);
        }
    }
}
