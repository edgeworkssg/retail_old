using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.SYNERGIX.response
{
    public class Product_Prices
    {
        public IList<Product_Price> product_pricings;
    }

    public class Product_Price
    {
        public string customer_group_code;
        public string start_date;
        public string end_date;
        public string create_date;
        public string item_type;
        public string item_code;
        public string pack_size_code;
        public decimal unit_price;
        public string uom_type;
    }

}
