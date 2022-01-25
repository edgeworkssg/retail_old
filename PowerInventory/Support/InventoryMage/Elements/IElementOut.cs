using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerInventory.Support.InventoryMages.Elements
{
    public interface IElementOut
    {
        string MovementType { get; set; }
        string ReferenceNo { get; set; }
        DateTime InventoryDate { get; set; }
        int Quantity { get; set; }


    }
}
