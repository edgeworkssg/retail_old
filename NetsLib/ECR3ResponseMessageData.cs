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
    class ECR3ResponseMessageData
    {
        public ECR3ResponseMessageData(string name, string fieldCode, string fieldEncoding    )
        {
            Name = name;
            FieldCode = fieldCode;
            FieldEncoding = fieldEncoding;

        }

        public string Name { get; private set; }
        public string FieldCode { get; private set; }
        public int Len { get; private set; }
        public string FieldEncoding {get; private set;}
    }
}
