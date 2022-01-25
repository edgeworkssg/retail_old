using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETSApplication
{
    /// <summary>
    /// Class for generating ACK(0x06) bytes array
    /// </summary>
    class ACKPacket : Packet
    {
        public override byte[] toBytes()
        {
            return (new byte[] { NETSConstants.ACK });
        }
    }
}
