using System;

namespace PowerInventory.Support
{
    public partial class InventoryMageStructure
    {
        public string ItemNo;
        public string ItemName;
        public int InventoryLocationID;
        public string InventoryLocationName;

        public string StockInMovementType;
        public string StockInReferenceNo;
        public DateTime StockInInventoryDate;
        public int StockInQuantity;
        public string StockOutMovementType;
        public string StockOutReferenceNo;
        public DateTime StockOutInventoryDate;
        public int StockOutQuantity;
    }
}
