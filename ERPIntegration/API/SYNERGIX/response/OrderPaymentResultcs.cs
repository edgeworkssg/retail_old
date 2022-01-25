using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.SYNERGIX.response
{
    public class OrderPaymentSucces
    {
        public string invoice_no;
        public string receipt_no;
    }

    public class OrderPaymentError
    {
        public IList<string> error_message;
    }
}
