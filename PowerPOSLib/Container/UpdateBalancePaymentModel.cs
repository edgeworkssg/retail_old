using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;

namespace PowerPOSLib.Container
{
    public class UpdateBalancePaymentModel
    {
        public Object OrderHdrList { get; set; }
        public Object OrderDetList { get; set; }
        public Object ReceiptHdrList { get; set; }
        public Object ReceiptDetList { get; set; }
    }
}

