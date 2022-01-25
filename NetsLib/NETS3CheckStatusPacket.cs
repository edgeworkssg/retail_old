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
    class NETS3CheckStatusPacket : Packet
    {
        private DateTime transactionDate;
        public NETS3CheckStatusPacket()
        {
            init();
            transactionDate = new DateTime(2019, 11, 25, 10, 38, 44);
        }

        public NETS3CheckStatusPacket(DateTime transDate)
        {
            init();
            transactionDate = transDate;
        }


        public override byte[] toBytes()
        {
            string tmp = "";
            byte[] content = generateMessageHeaderECR3(NETSConstants.MessageHeader.ECR3MessageType.MSG_TYPE_DEVICE, 
                NETSConstants.MessageHeader.ECR3MessageCode.MSG_CODE_DEVICE_STATUS, "1", DateTime.Now, out tmp);
            
            string crc = Utilities.CalculateCRC(Utilities.StringToByteArray(tmp));
            string fullPacketString = "40000000" + Utilities.reverseString(crc) + Utilities.AddTransparency(tmp);

            NetsLogger.writeLog("Full Packet : " + fullPacketString);
            //byte[] crcObj = Utilities.asciiStrToBytesArray(crc);
            int idx = 0;

            content = Utilities.asciiStrToBytesArray(fullPacketString);
            //combine message header, message data 1, message data 2
            /*idx = 0;
            byte[] temp = new byte[crcObj.Length + content.Length];
            Array.Copy(crcObj, 0, temp, idx, crcObj.Length);
            idx += crc.Length;

            
            Array.Copy(content, 0, temp, idx, content.Length);
            idx += content.Length;
            */
            //byte[] packet = generateFullPacketECR3(temp);

            byte[] packet = generateFullPacketECR3(content);
            
            //byte[] packet = ;

            return packet;
        }


        public override void processResponse(byte[] response)
        {
            base.processResponse(response);
        }

        public override void reset()
        {
            base.reset();
        }
    }
}
