using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERPIntegration.API.SYNERGIX.response
{
    public class Products
    {
        public IList<Product> products;
    }

    public class Product
    {
        public string item_type;
        public string item_code;
        public string item_desc;
        public string base_uom;
        public IList<Pack_Size> pack_sizes;
        public string inventory_class;
        public string inventory_category;
        public string inventory_brand;
        public string inventory_brand_desc;
        public bool is_suspend;
        public string item_barcode;
    }
    public class Pack_Size
    {
        public string pack_size_code;
        public string pack_size_desc;
        public decimal qty_in_base_uom;        
    }

}
