using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;

namespace PowerPOS
{
    [Serializable]
    public partial class SaveItemController
    {
        //Constructor
        public SaveItemController()
        {

        }

        //Save
        public static bool SaveTransfers(DataTable itemTable, string SaveName,
            int FromLocation, int ToLocation, string UserName, string Remark, out string status)
        {
            using (TransactionScope ts = new TransactionScope())
            {

                //Delete all that belong to the old save name...
                SaveItem.Destroy(SaveItem.Columns.SaveName, SaveName);

                //insert new into the new save name...
                for (int i = 0; i < itemTable.Rows.Count; i++)
                {
                    //perform insert for everyline....


                    SaveItem.Insert(SaveName, itemTable.Rows[i]["ItemNo"].ToString(),
                        int.Parse(itemTable.Rows[i]["Quantity"].ToString()), 0.0M, null, null,
                        FromLocation, ToLocation, Remark, itemTable.Rows[i]["Remark"].ToString(), DateTime.Now,
                        "Transfer Out", "", "", "", "", "", "", "", "", "", "",
                        null, null, null, null, null,
                        null, null, null, null, null,
                        null, null, null, null, null,
                        DateTime.Now,
                        UserName, DateTime.Now, UserName, false);


                    /*
                    //bulk update of the reason and destinations - not a normalized DB - Bad Design
                    Query qr = SaveItem.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddWhere(SaveItem.Columns.SaveName, SaveName);
                    qr.AddUpdateSetting(SaveItem.Columns.StockOutReasonID, StockOutReasonId);
                    qr.AddUpdateSetting(SaveItem.Columns.InventoryLocationID, LocationID);
                    qr.Execute();
                     * 
                     */
                }

                //Save Transaction
                ts.Complete();
            }
            status = "";
            return true;
        }

        public static bool SaveStockOut(DataTable itemTable, string SaveName,
            int LocationID, int StockOutReasonId, string UserName, string Remark, out string status)
        {
            using (TransactionScope ts = new TransactionScope())
            {

                //Delete all that belong to the old save name...
                SaveItem.Destroy(SaveItem.Columns.SaveName, SaveName);

                //insert new into the new save name...
                for (int i = 0; i < itemTable.Rows.Count; i++)
                {
                    //perform insert for everyline....


                    SaveItem.Insert(SaveName, itemTable.Rows[i]["ItemNo"].ToString(),
                        int.Parse(itemTable.Rows[i]["Quantity"].ToString()), 0.0M, StockOutReasonId, LocationID,
                        null, null, Remark, itemTable.Rows[i]["Remark"].ToString(), DateTime.Now,
                        "Stock Out", "", "", "", "", "", "", "", "", "", "",
                        null, null, null, null, null,
                        null, null, null, null, null,
                        null, null, null, null, null,
                        DateTime.Now,
                        UserName, DateTime.Now, UserName, false);


                    /*
                    //bulk update of the reason and destinations - not a normalized DB - Bad Design
                    Query qr = SaveItem.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddWhere(SaveItem.Columns.SaveName, SaveName);
                    qr.AddUpdateSetting(SaveItem.Columns.StockOutReasonID, StockOutReasonId);
                    qr.AddUpdateSetting(SaveItem.Columns.InventoryLocationID, LocationID);
                    qr.Execute();
                    */
                }

                //Save Transaction
                ts.Complete();
            }
            status = "";
            return true;
        }

        public static string GetMovementType(string savename)
        {
            Query qr = SaveItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = "MovementType";
            qr.AddWhere(SaveItem.Columns.SaveName, savename);
            DataTable dt = qr.DISTINCT().ORDER_BY("MovementType").ExecuteDataSet().Tables[0];
            if (dt.Rows.Count > 0)
            {

                return dt.Rows[0]["MovementType"].ToString();
            }
            else
            {
                return "";
            }

        }

        public static void GetTransferHeaderInfo(string SaveName, out int FromLocation, out int ToLocation, out string remark, out string status)
        {
            Query qr = SaveItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = "TransferFrom,TransferTo,Remark";
            qr.AddWhere(SaveItem.Columns.SaveName, SaveName);
            DataTable dt = qr.DISTINCT().ORDER_BY("TransferFrom,TransferTo,Remark").ExecuteDataSet().Tables[0];
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["TransferFrom"].ToString(), out FromLocation);
                int.TryParse(dt.Rows[0]["TransferTo"].ToString(), out ToLocation);
                remark = dt.Rows[0]["Remark"].ToString();
            }
            else
            {
                FromLocation = 0;
                ToLocation = 0;
                remark = "";
            }

            status = "";

            return;

        }
        public static void GetStockOutHeaderInfo
            (string SaveName, out int LocationID, out int StockOutReasonId, out string remark, out string status)
        {
            Query qr = SaveItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = "StockOutReasonID,InventoryLocationID,Remark";
            qr.AddWhere(SaveItem.Columns.SaveName, SaveName);
            DataTable dt = qr.DISTINCT().ORDER_BY("StockOutReasonID,InventoryLocationID,Remark").ExecuteDataSet().Tables[0];
            if (dt.Rows.Count > 0)
            {
                int.TryParse(dt.Rows[0]["StockOutReasonID"].ToString(), out StockOutReasonId);
                int.TryParse(dt.Rows[0]["InventoryLocationID"].ToString(), out LocationID);
                remark = dt.Rows[0]["Remark"].ToString();
            }
            else
            {
                LocationID = 0;
                StockOutReasonId = 0;
                remark = "";
            }

            status = "";

            return;

        }

        public static DataTable GetStockOutHeaderList()
        {
            /*
            Query qr = SaveItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = "MAX(InventoryDate),SaveName,StockOutReasonID,InventoryLocationID,Remark";
            
            qr.AddWhere(SaveItem.Columns.MovementType, "Stock Out");
            DataTable dt = qr.DISTINCT().ORDER_BY("MAX(InventoryDate),SaveName,StockOutReasonID,InventoryLocationID,Remark").ExecuteDataSet().Tables[0];
             */

            DataTable dt = (new ViewSaveItemCollection()).Where(ViewSaveItem.Columns.MovementType, "Stock Out").Load().ToDataTable();
            dt.Columns.Add("StockOutReasonName");
            dt.Columns.Add("InventoryLocationName");

            InventoryLocationCollection it = new InventoryLocationCollection();
            it.Load();

            InventoryStockOutReasonCollection rsn = new InventoryStockOutReasonCollection();
            rsn.Load();
            InventoryLocation myLoc;
            InventoryStockOutReason myReason;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                myLoc = ((InventoryLocation)it.Find(int.Parse(dt.Rows[i]["InventoryLocationID"].ToString())));
                if (myLoc != null && myLoc.IsLoaded)
                    dt.Rows[i]["InventoryLocationName"] = myLoc.InventoryLocationName;

                myReason = ((InventoryStockOutReason)rsn.Find(int.Parse(dt.Rows[i]["StockOutReasonID"].ToString())));
                if (myReason != null && myReason.IsLoaded)
                    dt.Rows[i]["StockOutReasonName"] = myReason.ReasonName;
            }

            dt.Columns.Remove("InventoryLocationID");
            dt.Columns.Remove("StockOutReasonID");
            dt.Columns.Remove("TransferFrom");
            dt.Columns.Remove("TransferTo");

            return dt;
        }
        public static DataTable GetTransferOutHeaderList()
        {
            /*
            Query qr = SaveItem.CreateQuery();
            qr.QueryType = QueryType.Select;
            qr.SelectList = "InventoryDate,SaveName,TransferFrom,TransferTo,Remark";
            qr.AddWhere(SaveItem.Columns.MovementType, "Transfer Out");
            DataTable dt = qr.DISTINCT().ORDER_BY("InventoryDate,SaveName,TransferFrom,TransferTo,Remark").ExecuteDataSet().Tables[0];
            */

            DataTable dt = (new ViewSaveItemCollection()).
                Where(ViewSaveItem.Columns.MovementType, "Transfer Out").Load().ToDataTable();


            InventoryLocationCollection it = new InventoryLocationCollection();
            it.Load();

            InventoryStockOutReasonCollection rsn = new InventoryStockOutReasonCollection();
            rsn.Load();


            dt.Columns.Add("TransferFromName");
            dt.Columns.Add("TransferToName");

            InventoryLocation TransferFrom, TransferTo;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                TransferFrom = ((InventoryLocation)it.Find(int.Parse(dt.Rows[i]["TransferFrom"].ToString())));

                if (TransferFrom != null && TransferFrom.IsLoaded)
                    dt.Rows[i]["TransferFromName"] = TransferFrom.InventoryLocationName;

                TransferTo = ((InventoryLocation)it.Find(int.Parse(dt.Rows[i]["TransferTo"].ToString())));
                if (TransferTo != null && TransferTo.IsLoaded)
                    dt.Rows[i]["TransferToName"] = TransferTo.InventoryLocationName;
            }

            dt.Columns.Remove("InventoryLocationID");
            dt.Columns.Remove("StockOutReasonID");
            dt.Columns.Remove("TransferFrom");
            dt.Columns.Remove("TransferTo");
            return dt;

        }
    }
}
