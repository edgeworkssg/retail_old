using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerInventory.Support
{
    public partial class StockOutElement
    {
        public string MovementType;
        public string ReferenceNo;
        public DateTime InventoryDate;
        public int Quantity;

        public StockOutElement(string MovementType, string ReferenceNo, DateTime InventoryDate, int Quantity)
        {
            this.MovementType = MovementType;
            this.ReferenceNo = ReferenceNo;
            this.InventoryDate = InventoryDate;
            this.Quantity = Quantity;
        }

    }
}
