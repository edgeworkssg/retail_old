using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerWeb.Synchronization.Classes
{
    public class InventoryItem
    {
        public String ItemNo { get; set; }
        public decimal Qty { get; set; }
    }

    public class SKUItem
    {
        public String Barcode { get; set; }
        public decimal Qty { get; set; }
    }
}
