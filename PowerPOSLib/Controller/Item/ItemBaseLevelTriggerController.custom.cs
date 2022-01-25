using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;

namespace PowerPOS
{
    public partial class ItemBaseLevelController
    {
        /// <summary>
        /// Get a table for item quantity trigger
        /// </summary>
        public static DataTable FetchData(string _itemId)
        {
            string SQLString;
            #region -= SQL STRING =-
            SQLString =
                @"SELECT q.BaseLevelID, q.ItemNo, i.ItemName, q.BaseLevelQuantity, q.InventoryLocationId, il.InventoryLocationName,
                    c.CategoryName as Category, i.userfld1 as UOM   
                FROM ItemBaseLevel q, Item i, Category c, InventoryLocation il 
                WHERE q.ItemNo = i.ItemNo and q.InventoryLocationId = il.InventoryLocationId and i.categoryname = c.categoryname and 
                c.CategoryName = i.categoryName and  i.ItemName like '%' + @Search +'%'  And ISNULL(q.Deleted,0) <> 1 ";
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

        /// <summary>
        /// Get a table for item quantity trigger
        /// </summary>
        public static DataTable FetchDataByInventoryLocation(int inventorylocationid, int supplierID)
        {
            string SQLString;
            #region -= SQL STRING =-
            SQLString =
                @"SELECT q.BaseLevelID, q.ItemNo, i.ItemName, q.BaseLevelQuantity, q.InventoryLocationId, il.InventoryLocationName,
                     i.userfld1 as UOM   
                FROM ItemBaseLevel q, Item i, InventoryLocation il 
                WHERE q.ItemNo = i.ItemNo and q.InventoryLocationId = il.InventoryLocationId and 
                q.InventoryLocationid = " + inventorylocationid + " And ISNULL(q.Deleted,0) <> 1 ";

            if (supplierID != 0)
            {
                SQLString += " and i.ItemNo in (select ItemNo from ItemSupplierMap where SupplierID = " + supplierID.ToString() + ")";
            }
            #endregion

            //if ()

            DataTable DT = new DataTable();
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);

            //Cmd.AddParameter("@Search", _itemId);

            DT.Load(DataService.GetReader(Cmd));
            #endregion

            return DT;
        }

        public static DataTable FetchDataByInventoryLocationSupplierPortal(int inventorylocationid, int supplierID, string username)
        {
            string SQLString;

            string queryW = "Select count(*) from Supplier where ISNULL(Userflag1,0) = 1 and ISNULL(Deleted,0) = 0";

            int countWarehouse = (int)DataService.ExecuteScalar(new QueryCommand(queryW));

            #region -= SQL STRING =-
            SQLString =
                @"SELECT q.BaseLevelID, q.ItemNo, i.ItemName, q.BaseLevelQuantity, q.InventoryLocationId, il.InventoryLocationName,
                     i.userfld1 as UOM   
                FROM ItemBaseLevel q, Item i, InventoryLocation il 
                WHERE q.ItemNo = i.ItemNo and q.InventoryLocationId = il.InventoryLocationId and 
                q.InventoryLocationid = " + inventorylocationid + " And ISNULL(q.Deleted,0) <> 1 ";

            bool isUseSupplierPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);

            if (supplierID != 0 && countWarehouse > 1)
            {
                SQLString += " and i.ItemNo in (select ItemNo from ItemSupplierMap where SupplierID = " + supplierID.ToString() + ")";
            }
            else
            {
                List<string> supID = new List<string>();
                string supp = "";
                UserMst us = new UserMst(UserMst.Columns.UserName, username);
                if (isUseSupplierPortal && us != null && us.IsSupplier && us.IsRestrictedSupplierList && supplierID == 0)
                {
                    if (us != null)
                    {

                        string query = string.Format("SELECT * FROM Supplier WHERE ISNULL(deleted,0) = 0 and userfld4 = '{0}'", us.UserName);
                        QueryCommand qc = new QueryCommand(query);
                        SupplierCollection colSup = new SupplierCollection();
                        colSup.LoadAndCloseReader(DataService.GetReader(qc));

                        if (colSup.Count > 0)
                        {

                            for (int i = 0; i < colSup.Count; i++)
                            {
                                if (supplierID == 0 || (supplierID != 0 && supplierID == colSup[i].SupplierID))
                                {
                                    supID.Add(colSup[i].SupplierID.ToString());
                                }
                            }

                            supp = String.Join(",", supID.ToArray());
                        }
                    }

                    if (string.IsNullOrEmpty(supp))
                        supp = "0";

                    SQLString += " and i.ItemNo in (select ItemNo from ItemSupplierMap where SupplierID IN (" + supp + ") )";
                }
            }
            #endregion

            //if ()

            DataTable DT = new DataTable();
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);

            //Cmd.AddParameter("@Search", _itemId);

            DT.Load(DataService.GetReader(Cmd));
            #endregion

            return DT;
        }

        public static DataTable FetchDataByInventoryLocation(int inventorylocationid)
        {
            return FetchDataByInventoryLocation(inventorylocationid, 0);
        }

        public static int getOptimalStockLevel(string _itemId, int InventoryLocationid)
        {
            int res = 0;
            ItemBaseLevelCollection col = new ItemBaseLevelCollection();


            string SQLString;
            #region -= SQL STRING =-
            SQLString =
                @"SELECT BaseLevelQuantity
                FROM ItemBaseLevel q
                WHERE itemno like '" + _itemId + "' And InventoryLocationID = " + InventoryLocationid + " And ISNULL(q.Deleted,0) <> 1 ";
            #endregion

            //if ()

            object temp;
            #region *) Fetch: Load the Data
            QueryCommand Cmd = new QueryCommand(SQLString);

            temp = DataService.ExecuteScalar(Cmd);
            if (temp != null)
            {
                int.TryParse(temp.ToString(), out res);
            }
            #endregion

            return res;
        }
    }
}
