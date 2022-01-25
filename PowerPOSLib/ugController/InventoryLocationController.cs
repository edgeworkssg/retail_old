using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerPOS
{
    public partial class InventoryLocationController
    {
        public static bool IsDeleted(int inventoryLocationID)
        {
            InventoryLocation invLoc = new InventoryLocation(inventoryLocationID);
            if (invLoc != null && invLoc.InventoryLocationID == inventoryLocationID)
            {
                return invLoc.Deleted.GetValueOrDefault(false);
            }
            else
            {
                return true;
            }
        }
    }
}
