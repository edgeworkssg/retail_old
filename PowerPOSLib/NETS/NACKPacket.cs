using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Class for generating NACK(0x15) bytes array
    /// </summary>
    class NACKPacket : Packet
    {
        public override byte[] toBytes()
        {
            return (new byte[] { NETSConstants.NACK });
        }
    }
}
