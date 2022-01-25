using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS.Nets
{
    /// <summary>
    /// Generic class for storing information of message data
    /// <para>Name</para>
    /// <para>Field Code</para>
    /// <para>Length of message data</para>
    /// </summary>
    class ResponseMessageData
    {
        public ResponseMessageData(string name, string fieldCode, int len)
        {
            Name = name;
            FieldCode = fieldCode;
            Len = len;
        }

        public string Name { get; private set; }
        public string FieldCode { get; private set; }
        public int Len { get; private set; }
    }
}
