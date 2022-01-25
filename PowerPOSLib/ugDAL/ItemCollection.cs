using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* Built manually - Not from SubSonic */

namespace PowerPOS
{
    public partial class ItemCollection
    {
        /// <summary>
        /// Fetch all undeleted Item with the specified Barcode
        /// </summary>
        /// <param name="Barcode">Barcode of the Item to be loaded</param>
        public void LoadByBarcode(string Barcode)
        {
            try
            {
                Query qr = new Query("Item");
                qr.OrderBy = OrderBy.Asc(Item.Columns.ItemNo);
                qr.QueryType = QueryType.Select;
                qr.AddWhere(Item.Columns.Barcode, Comparison.Equals, Barcode);
                qr.AddWhere(Item.Columns.Deleted, Comparison.Equals, false);

                LoadAndCloseReader(qr.ExecuteReader());
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }
        }

    }
}
