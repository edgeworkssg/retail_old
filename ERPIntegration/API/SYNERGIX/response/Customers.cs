using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.SYNERGIX.response
{
    public class Customers
    {
        public IList<Customer> customers;
    }

    public class Customer
    {
        public string customer_code;
        public string customer_name;
        public IList<Address> addresses;
        public IList<Contact> contacts;
        public string customer_group_code;
        public bool is_suspend;
    }

    public class Address
    {
        public string address_code;
        public string address_desc;
        public string address;
        public string postal_code; 
    }

    public class Contact
    {
        public string contact_code;
        public string contact_name;
        public IList<string> phone_numbers;
    }
}
