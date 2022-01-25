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
    class TestPacket : Packet
    {
        private string command = "";

        public TestPacket()
        {
            init();
        }

        public void setCommand(string commandString)
        {
            command = commandString;
        }

        public override byte[] toBytes()
        {
            byte[] temp = Utilities.hexStrToBytesArray(command);

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
        }
    }
}
