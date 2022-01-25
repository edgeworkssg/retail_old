using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerInventory.Support
{
    public partial class StockInElement
    {
        public string MovementType;
        public string ReferenceNo;
        public DateTime InventoryDate;
        public int Quantity;
        public List<StockOutElement> StockOutList;

        public int QuantityAssigned
        {
            get
            {
                int Total = 0;
                for (int Counter = 0; Counter < StockOutList.Count; Counter++)
                    Total += StockOutList[Counter].Quantity;
                return Total;
            }
        }

        public int QuantityLeft
        {
            get { return Quantity - QuantityAssigned; }
        }

        public StockInElement(string MovementType, string ReferenceNo, DateTime InventoryDate, int Quantity)
        {
            this.MovementType = MovementType;
            this.ReferenceNo = ReferenceNo;
            this.InventoryDate = InventoryDate;
            this.Quantity = Quantity;
            StockOutList = new List<StockOutElement>();
        }
    }
}
