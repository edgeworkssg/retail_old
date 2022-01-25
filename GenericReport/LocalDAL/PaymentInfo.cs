using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LocalDAL
{
    public class PaymentInfo
    {
        private string _PaymentType;

        public string PaymentType
        {
            get { return _PaymentType; }
            set { _PaymentType = value; }
        }

        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public PaymentInfo(string PaymentType, decimal Amount)
        {
            this.PaymentType = PaymentType;
            this.Amount = Amount;
        }
    }
}
