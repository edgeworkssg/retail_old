using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PowerPOS;
using System.Data;
using SubSonic;

namespace ERPIntegration.API
{
    class Update_StockOnHand
    {
//        struct MessageType
//        {
//            public static string INFO = "UPDATE_STOCKONHAND_INFO";
//            public static string ERROR = "UPDATE_STOCKONHAND_ERROR";
//        }

//        public static bool DoUpdate()
//        {
//            bool proceed = true;

//            try
//            {
//                Helper.WriteLog("Starting Update Stock On Hand module", MessageType.INFO);

//                DataSet ds = GetLatestExtStockOnHand();
//                if (ds != null && ds.Tables.Count > 0)
//                {
//                    Helper.WriteLog(string.Format("Processing {0} record(s)...", ds.Tables[0].Rows.Count), MessageType.INFO);

//                    DateTime importDate = DateTime.Now;
//                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
//                    {
//                        proceed = true;
//                        DataRow dr = ds.Tables[0].Rows[i];
//                        string locationName = dr["LOCATION"].ToString().Trim();
//                        string itemNo = dr["ITEMNO"].ToString().Trim();
//                        int newQty = 0;
//                        int locationID = 0;

//                        #region *) VALIDATION
//                        if (string.IsNullOrEmpty(locationName))
//                        {
//                            Helper.WriteLog(string.Format("LOCATION is empty for record #{0}", i + 1), MessageType.ERROR);
//                            proceed = false;
//                        }
//                        else
//                        {
//                            InventoryLocation loc = new InventoryLocation("InventoryLocationName", locationName);
//                            if (loc.InventoryLocationID == 0)
//                            {
//                                Helper.WriteLog(string.Format("LOCATION \"{0}\" does not exist in InventoryLocation table.", locationName), MessageType.ERROR);
//                                proceed = false;
//                            }
//                            else
//                            {
//                                locationID = loc.InventoryLocationID;
//                            }
//                        }
                        
//                        if (string.IsNullOrEmpty(itemNo))
//                        {
//                            Helper.WriteLog(string.Format("ITEMNO is empty for record #{0}", i + 1), MessageType.ERROR);
//                            proceed = false;
//                        }
//                        else
//                        {
//                            Item item = new Item(itemNo);
//                            if (string.IsNullOrEmpty(item.ItemNo))
//                            {
//                                Helper.WriteLog(string.Format("ITEMNO \"{0}\" does not exist in Item table.", itemNo), MessageType.ERROR);
//                                proceed = false;
//                            }
//                        }

//                        decimal tmpQty;
//                        if (!decimal.TryParse(dr["QTYONHAND"].ToString(), out tmpQty))
//                        {
//                            Helper.WriteLog(string.Format("Invalid QTYONHAND value for record #{0} ({1} - {2})", i + 1, locationName, itemNo), MessageType.ERROR);
//                            proceed = false;
//                        }
//                        else
//                        {
//                            newQty = Convert.ToInt32(tmpQty);
//                        }
//                        #endregion

//                        if (proceed)
//                        {
//                            // Log the new quantity if necessary
//                            WriteExtStockQtyLog(locationName, itemNo, newQty);

//                            int qtyInv = GetStockOnHandExclSalesMovement(locationID, itemNo);
//                            int diffQty = Math.Abs(qtyInv - newQty);

//                            if (diffQty != 0)
//                            {
//                                string status = "";
//                                string username = "ERPIntegration";

//                                PowerPOS.Container.CostingMethods CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
//                                string strCostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
//                                if (strCostingMethod.ToLower() == "fifo")
//                                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;
//                                else if (strCostingMethod.ToLower() == "fixed avg")
//                                    CostingMethod = PowerPOS.Container.CostingMethods.FixedAvg;
//                                else
//                                    CostingMethod = PowerPOS.Container.CostingMethods.FIFO;

//                                InventoryController ctrl = new InventoryController(CostingMethod);
//                                ctrl.SetInventoryHeaderInfo("", "", "ERPIntegration", 0, 0, 0);
//                                ctrl.SetInventoryDate(importDate);
//                                ctrl.AddItemIntoInventory(itemNo, diffQty, out status);
                            
//                                if (newQty > qtyInv)
//                                {
//                                    // Do Stock In to increase qty in inventory
//                                    ctrl.StockIn(username, locationID, false, true, out status);
//                                }
//                                else if (newQty < qtyInv)
//                                {
//                                    // Do Stock Out to decrease qty in inventory
//                                    ctrl.StockOut(username, 0, locationID, false, true, out status);
//                                }
//                            }
//                        }
//                    }
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
//                return false;
//            }
//        }

//        private static DataSet GetLatestExtStockOnHand()
//        {
//            string sql = "SELECT * FROM ViewExtStockOnHand";
//            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
//            DataSet ds = DataService.GetDataSet(cmd);
//            return ds;
//        }

//        /// <summary>
//        /// Insert new record only if not exist yet or quantity has changed.
//        /// </summary>
//        /// <returns></returns>
//        private static bool WriteExtStockQtyLog(string location, string itemNo, int qty)
//        {
//            try
//            {
//                string sql = @"
//                                SELECT Quantity FROM ExtStockQtyLog 
//                                WHERE Location = @Location AND ItemNo = @ItemNo 
//                                    AND UpdatedOn = (SELECT MAX(UpdatedOn) FROM ExtStockQtyLog WHERE Location = @Location AND ItemNo = @ItemNo) 
//                             ";
//                QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
//                cmd.AddParameter("@Location", location, DbType.String);
//                cmd.AddParameter("@ItemNo", itemNo, DbType.String);

//                object tmpQty = DataService.ExecuteScalar(cmd);
//                if (tmpQty == null || (int)tmpQty != qty)
//                {
//                    // Do Insert
//                    sql = @"
//                            INSERT INTO ExtStockQtyLog (Location, ItemNo, Quantity, UpdatedOn)
//                            VALUES (@Location, @ItemNo, @Quantity, GETDATE())
//                          ";
//                    cmd = new QueryCommand(sql, "PowerPOS");
//                    cmd.AddParameter("@Location", location, DbType.String);
//                    cmd.AddParameter("@ItemNo", itemNo, DbType.String);
//                    cmd.AddParameter("@Quantity", qty, DbType.Int32);
//                    DataService.ExecuteQuery(cmd);
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                Helper.WriteLog(ex.ToString(), MessageType.ERROR);
//                return false;
//            }
//        }

//        //private static int GetExternalStockQty(string location, string itemNo)
//        //{
//        //    int qty;
//        //    string sql = "SELECT Quantity FROM ExtStockQty WHERE Location = @Location AND ItemNo = @ItemNo";
//        //    QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
//        //    int.TryParse(DataService.ExecuteScalar(cmd).ToString(), out qty);
//        //    return qty;
//        //}

//        /// <summary>
//        /// Get Stock On Hand from Inventory, excluding inventory movement caused by sales since last cut-off date/time
//        /// </summary>
//        /// <param name="location"></param>
//        /// <param name="itemNo"></param>
//        /// <returns></returns>
//        private static int GetStockOnHandExclSalesMovement(int inventoryLocationID, string itemNo)
//        {
//            string cutoffDate = AppSetting.GetSetting("LastSalesCutOffDate");
//            if (string.IsNullOrEmpty(cutoffDate))
//            {
//                cutoffDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
//                AppSetting.SetSetting("LastSalesCutOffDate", cutoffDate);
//            }

//            string sql = @"
//                            SELECT SUM(CASE WHEN IH.MovementType LIKE '% In' THEN ID.Quantity ELSE -ID.Quantity END) Quantity
//                            FROM InventoryHdr IH 
//                                INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo 
//                                LEFT JOIN OrderDet OD ON OD.InventoryHdrRefNo = IH.InventoryHdrRefNo AND OrderDetDate > @CutOffDate
//                            WHERE IH.InventoryLocationID = @InventoryLocationID AND ID.ItemNo = @ItemNo AND OD.OrderDetID IS NULL
//                            GROUP BY IH.InventoryLocationID, ID.ItemNo
//                          ";
//            QueryCommand cmd = new QueryCommand(sql, "PowerPOS");
//            cmd.Parameters.Add("@InventoryLocationID", inventoryLocationID, DbType.Int32);
//            cmd.Parameters.Add("@ItemNo", itemNo, DbType.String);
//            cmd.Parameters.Add("@CutOffDate", cutoffDate, DbType.DateTime);

//            int qty = 0;
//            object tmpQty = DataService.ExecuteScalar(cmd);
//            if (tmpQty != null)
//            {
//                int.TryParse(tmpQty.ToString(), out qty);
//            }
//            return qty;
//        }
    }
}
