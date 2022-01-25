using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETSApplication
{
    /// <summary>
    /// Packet to request terminal status (i.e. NETS Machine's status)
    /// <para>Field Code : 55</para>
    /// </summary>
    class RequestTerminalStatusPacket : Packet
    {
        public RequestTerminalStatusPacket()
        {
            init();
        }

        public override byte[] toBytes()
        {
            reset();

            byte[] content = generateMessageHeader(NETSConstants.MessageHeader.FunctionCode.REQUEST_TERMINAL_STATUS);

            return (generateFullPacket(content));
        }

        public override void processResponse(byte[] response)
        {
            base.processResponse(response);
        }
    }
}
