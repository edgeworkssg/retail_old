using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerInventory.Support.InventoryMages.Elements
{
    public class StockIn : IElementIn
    {
        private string movementType;
        private string referenceNo;
        private DateTime inventoryDate;
        private int quantity;
        private decimal unitCOG;
        private List<StockOutElement> StockOutList;

        public string MovementType
        {
            get { return movementType; }
            set { movementType = value; }
        }
        public string ReferenceNo
        {
            get { return referenceNo; }
            set { referenceNo = value; }
        }
        public DateTime InventoryDate
        {
            get { return inventoryDate; }
            set { inventoryDate = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

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

        public decimal UnitCOG
        {
            get { return unitCOG; }
            set { unitCOG = value; }
        }

        public StockIn(string MovementType, string ReferenceNo, DateTime InventoryDate, int Quantity, decimal UnitCOG)
        {
            this.MovementType = MovementType;
            this.ReferenceNo = ReferenceNo;
            this.InventoryDate = InventoryDate;
            this.Quantity = Quantity;
            this.UnitCOG = UnitCOG;
            StockOutList = new List<StockOutElement>();
        }


        //void AddElementOut(IElementOut value);

    }
}
