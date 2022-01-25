using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace PowerPOS.Container
{
    public class EZLinkComPortInfo
    {
        public static string COMPort;
        public static int BaudRate; 
        public static int DataBits;
        public static System.IO.Ports.Parity Parity;
        public static System.IO.Ports.StopBits StopBits;
        public static System.IO.Ports.Handshake HandShake;        
    }
}
