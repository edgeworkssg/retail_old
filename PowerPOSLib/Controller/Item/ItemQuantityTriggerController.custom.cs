using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class ItemQuantityTriggerController
    {
        /// <summary>
        /// Get a table for item quantity trigger
        /// </summary>
        public static DataTable FetchData(string _itemId)
        {
            string SQLString;
            #region -= SQL STRING =-
            SQLString =
                "SELECT q.TriggerId, q.ItemNo, i.ItemName, q.TriggerLevel, q.TriggerQuantity, q.InventoryLocationId, il.InventoryLocationName  " +
                ",q.Userfld1, q.Userfld2, q.Userfld3, q.Userfld4, q.Userfld5 " +
                ",q.Userfld6, q.Userfld7, q.Userfld8, q.Userfld9, q.Userfld10 " +
                "FROM ItemQuantityTrigger q, Item i, InventoryLocation il " +                
                "WHERE q.ItemNo = i.ItemNo and q.InventoryLocationId = il.InventoryLocationId and i.ItemName like '%' + @Search +'%' ";
            #endregion

            //if ()

            DataTable DT = new DataTable();
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);

            Cmd.AddParameter("@Search", _itemId);

            DT.Load(DataService.GetReader(Cmd));
            #endregion

            return DT;
        }

        public static DataTable FetchDataWithParameters(int inventrylocationid, string _itemId)
        {
            string SQLString;
            #region -= SQL STRING =-
            SQLString =
                "SELECT q.TriggerId, q.ItemNo, i.ItemName,i.CategoryName, q.TriggerLevel, q.TriggerQuantity, q.InventoryLocationId, il.InventoryLocationName  " +
                ",q.Userfld1, q.Userfld2, q.Userfld3, q.Userfld4, q.Userfld5 " +
                ",q.Userfld6, q.Userfld7, q.Userfld8, q.Userfld9, q.Userfld10 " +
                "FROM ItemQuantityTrigger q, Item i, InventoryLocation il " +
                "WHERE q.ItemNo = i.ItemNo and q.InventoryLocationId = il.InventoryLocationId " +
                "and ISNULL(i.Deleted,0) = 0 and ISNULL(q.Deleted,0) = 0 and ISNULL(il.Deleted,0) = 0 " +
                "and ISNULL(i.ItemNo,'') + ISNULL(i.ItemName,'') + ISNULL(i.CategoryName,'') like '%' + @Search +'%' " +
                "and (@inventorylocationid = 0 or il.InventoryLocationId = @inventorylocationid) ";
            #endregion

            //if ()

            DataTable DT = new DataTable();
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);

            Cmd.AddParameter("@Search", _itemId);
            Cmd.AddParameter("@inventorylocationid", inventrylocationid);

            DT.Load(DataService.GetReader(Cmd));
            #endregion

            return DT;
        }
    }
}
