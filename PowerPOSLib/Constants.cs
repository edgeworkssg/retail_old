using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public class Constants
    {
        public const int    PRIMARY_KEY_VOILATION = 2627;
        public const string SLOT_UNAVAILABLE = "The time slot is not available. Please select another Delivery Person/Time Slot";
        public const string ORDER_TIME_SLOT_TO = "TimeSlotTo";
        public const string ORDER_TIME_SLOT_FROM = "TimeSlotFrom";
        public const string ORDER_PERSON_ASSIGNED = "PersonAssigned";
        public const string ORDERNUMBER_ALREADY_EXISTS = "Order already exists. Please choose another order number!";
        public const string ORDERDETAILNUMBER_ALREADY_EXISTS = "Order detail already exists. Please choose another order detail ID!";
        public const string DELIVERYPERSONNEL_ALREADY_EXISTS = "Delivery Personnel already exists. Please choose another Delivery Person ID!";

        public struct ResetEvery
        {
            public const string Never = "Never";
            public const string Day = "Day";
            public const string Month = "Month";
            public const string Year = "Year";
            public const string MaxReached = "Max Reached";
        }
    }
}
