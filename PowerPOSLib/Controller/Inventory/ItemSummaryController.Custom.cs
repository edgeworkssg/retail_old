using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SubSonic;
using System.Configuration;

namespace PowerPOS
{
    public partial class ItemSummaryController
    {
        public static DataTable FetchStockBalance(string search, int inventoryLocID)
        {
            DataTable dt = new DataTable();

            try
            {
                var ds = SPs.FetchStockBalanceItemSummary(search, inventoryLocID).GetDataSet();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static DataTable FetchStockBalanceWithZeroQty(string search, int inventoryLocID, bool removeZeroQty)
        {
            return FetchStockBalanceWithZeroQty(search, inventoryLocID, removeZeroQty, "ALL");
        }

        public static DataTable FetchStockBalanceWithZeroQty(string search, int inventoryLocID, bool removeZeroQty, string categoryName)
        {
            DataTable dt = new DataTable();

            try
            {
                var ds = SPs.FetchStockBalanceItemSummaryByCategory(search, inventoryLocID, categoryName).GetDataSet();
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];

                if (removeZeroQty)
                {
                    DataSet dsn = new DataSet();
                    DataTable table = dt.Clone();
                    var dr = dt.Select("Quantity > 0");
                    DataTable finalResultwithoutzero = dr.Count() > 0 ? dr.CopyToDataTable() : table;
                    dsn.Tables.Add(finalResultwithoutzero);
                    return dsn.Tables[0];
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return dt;
        }

        public static decimal FetchCostPrice(string itemNo, int inventoryLocID)
        {
            decimal costPrice = 0;

            try
            {
                Query qr = new Query("ItemSummary");
                qr.AddWhere(ItemSummary.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSummary.Columns.InventoryLocationID, inventoryLocID);
                var itemSummary = new ItemSummaryController().FetchByQuery(qr).FirstOrDefault();
                if (itemSummary != null
                    && itemSummary.BalanceQty.GetValueOrDefault(0) > 0
                    && itemSummary.CostPrice.GetValueOrDefault(0) > 0)
                    costPrice = itemSummary.CostPrice.GetValueOrDefault(0);
                else
                {
                    Item theItem = new Item(Item.Columns.ItemNo, itemNo);
                    costPrice = theItem.FactoryPrice;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return costPrice;
        }

        public static decimal GetAvgCostPrice(string itemNo, int inventoryLocID)
        {
            bool useInvLoc = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationLevel), false));
            bool useInvLocGroup = (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationGroupLevel), false));

            decimal costPrice = 0;
            if (useInvLoc)
            {
                try
                {
                    Query qr = new Query("ItemSummary");
                    qr.AddWhere(ItemSummary.Columns.ItemNo, itemNo);
                    qr.AddWhere(ItemSummary.Columns.InventoryLocationID, inventoryLocID);
                    var itemSummary = new ItemSummaryController().FetchByQuery(qr).FirstOrDefault();
                    if (itemSummary != null
                        && itemSummary.BalanceQty.GetValueOrDefault(0) > 0
                        && itemSummary.CostPrice.GetValueOrDefault(0) > 0)
                        costPrice = itemSummary.CostPrice.GetValueOrDefault(0);
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                }
            }
            else if (useInvLocGroup)
            {
                try
                {
                    string sql = @"
                    SELECT * 
                    FROM	ViewItemSummary VIS
                    WHERE	VIS.ItemNo = @ItemNo
		                    AND VIS.InventoryLocationID = @InventoryLocationID";
                    QueryCommand cmd = new QueryCommand(sql);
                    cmd.AddParameter("@ItemNo", itemNo);
                    cmd.AddParameter("@InventoryLocationID", inventoryLocID, DbType.Int32);

                    DataTable dt = new DataTable();
                    dt.Load(DataService.GetReader(cmd));

                    if (dt.Rows.Count > 0)
                        costPrice = ((decimal?)dt.Rows[0]["LocGroupCostPrice"]).GetValueOrDefault(0);

                    //Logger.writeLog(string.Format("GetAvgCostPrice Group {0} - {1} : {2}", itemNo, inventoryLocID, costPrice));
                }
                catch (Exception ex)
                {
                    Logger.writeLog(ex);
                } 
            }
            else
            {
                Item theItem = new Item(Item.Columns.ItemNo, itemNo);
                costPrice = theItem.AvgCostPrice.GetValueOrDefault(0);
            }

            //Logger.writeLog(string.Format("GetAvgCostPrice {0} - {1} : {2}", itemNo, inventoryLocID, costPrice));

            return costPrice;
        }

        public static decimal GetAvgCostPrice_StockTake(string itemNo, int inventoryLocID, DateTime inventoryDate)
        {
            decimal costPricePerInvLoc = 0, costPricePerItem = 0;
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CalculateAvgCostatInventoryLocationLevel), false))
            {
                ViewInventoryActivityCollection vInventory = new ViewInventoryActivityCollection();
                vInventory.Where(ViewInventoryActivity.Columns.ItemNo, itemNo);
                vInventory.Where(ViewInventoryActivity.Columns.InventoryLocationID, inventoryLocID);
                vInventory.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.LessThan, inventoryDate);
                vInventory.OrderByDesc(ViewInventoryActivity.Columns.InventoryDate);
                vInventory.Load();

                if (vInventory.Count > 0)
                {
                    InventoryDet id = new InventoryDet(vInventory[0].InventoryDetRefNo);
                    costPricePerInvLoc = id.CostPriceByItemInvLoc.GetValueOrDefault(0);
                }

                return costPricePerInvLoc;
            }
            else
            {
                ViewInventoryActivityCollection vInventoryAll = new ViewInventoryActivityCollection();
                vInventoryAll.Where(ViewInventoryActivity.Columns.ItemNo, itemNo);
                vInventoryAll.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.LessThan, inventoryDate);
                vInventoryAll.OrderByDesc(ViewInventoryActivity.Columns.InventoryDate);
                vInventoryAll.Load();

                if (vInventoryAll.Count > 0)
                {
                    InventoryDet id = new InventoryDet(vInventoryAll[0].InventoryDetRefNo);
                    costPricePerItem = id.CostPriceByItem.GetValueOrDefault(0);
                }

                return costPricePerItem;
            }
        }

        public static decimal FetchStockBalanceByItemNo(string itemNo, int inventoryLocID)
        {
            decimal stockBalance = 0;

            try
            {
                Query qr = new Query("ItemSummary");
                qr.AddWhere(ItemSummary.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSummary.Columns.InventoryLocationID, inventoryLocID);
                var itemSummary = new ItemSummaryController().FetchByQuery(qr).FirstOrDefault();
                if (itemSummary != null)
                    stockBalance = Convert.ToDecimal(itemSummary.BalanceQty.GetValueOrDefault(0));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return stockBalance;
        }

        public static QueryCommandCollection FetchItemSummaryUpdate(string itemNo, int inventoryLocID, string movementType, decimal newCostPrice, double newQty, string invDetID, DateTime invDate)
        {
            decimal costOfGoods = 0;
            return FetchItemSummaryUpdate(itemNo, inventoryLocID, movementType, newCostPrice, newQty, invDetID, invDate, out costOfGoods);
        }

        public static QueryCommandCollection FetchItemSummaryUpdate(string itemNo, int inventoryLocID, string movementType, decimal newCostPrice, double newQty, string invDetID, DateTime invDate, out decimal costOfGoods)
        {
            QueryCommandCollection qmc = new QueryCommandCollection();
            costOfGoods = 0;
            
            try
            {
                double newOnHandQtyPerLocationGroup = 0;
                double newOnHandQtyPerLocation = 0;
                double newOnHandQtyPerItem = 0;

                decimal costPricePerLocationGroup = 0;
                decimal costPricePerLocation = 0;
                decimal costPricePerItem = 0;

                double calculatedOnHandQty = 0;
                decimal calculatedCostPrice = 0;

                #region *) Init Item Summary

                Query qr = new Query("ItemSummary");
                qr.AddWhere(ItemSummary.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSummary.Columns.InventoryLocationID, inventoryLocID);
                var itemSummary = new ItemSummaryController().FetchByQuery(qr).FirstOrDefault();
                var theItem = new Item(Item.Columns.ItemNo, itemNo);

                if (itemSummary == null)
                {
                    itemSummary = new ItemSummary();
                    itemSummary.ItemSummaryID = itemNo + "-" + inventoryLocID;
                    itemSummary.ItemNo = itemNo;
                    itemSummary.InventoryLocationID = inventoryLocID;
                    itemSummary.Deleted = false;
                    itemSummary.UniqueID = Guid.NewGuid();
                    itemSummary.BalanceQty = 0;
                    itemSummary.CostPrice = 0;
                    DataService.ExecuteQuery(itemSummary.GetInsertCommand("SYNC"));
                }

                #endregion

                #region *) Init Item Summary Group

                var invLoc = new InventoryLocation(inventoryLocID);
                var invLocGroup = new InventoryLocationGroup(invLoc.InventoryLocationGroupID);
                bool isUseLocGroup = !invLocGroup.IsNew;

                var itemSummaryGroup = new ItemSummaryGroup();
                if (isUseLocGroup)
                {
                    qr = new Query("ItemSummaryGroup");
                    qr.AddWhere(ItemSummaryGroup.Columns.ItemNo, itemNo);
                    qr.AddWhere(ItemSummaryGroup.Columns.InventoryLocationGroupID, invLocGroup.InventoryLocationGroupID);

                    itemSummaryGroup = new ItemSummaryGroupController().FetchByQuery(qr).FirstOrDefault();
                    if (itemSummaryGroup == null)
                    {
                        itemSummaryGroup = new ItemSummaryGroup();
                        itemSummaryGroup.ItemSummaryGroupID = itemNo + "-" + invLocGroup.InventoryLocationGroupID;
                        itemSummaryGroup.ItemNo = itemNo;
                        itemSummaryGroup.InventoryLocationGroupID = invLocGroup.InventoryLocationGroupID;
                        itemSummaryGroup.Deleted = false;
                        itemSummaryGroup.UniqueID = Guid.NewGuid();
                        itemSummaryGroup.BalanceQty = 0;
                        itemSummaryGroup.CostPrice = 0;
                        DataService.ExecuteQuery(itemSummaryGroup.GetInsertCommand("SYNC"));
                    } 
                }

                #endregion

                #region *) Calculate AVG Costing

                if (movementType.ToUpper().Contains("IN"))
                {
                    #region *) Item Per Inventory Location Group

                    if (isUseLocGroup)
                    {
                        newOnHandQtyPerLocationGroup = itemSummaryGroup.BalanceQty.GetValueOrDefault(0) + newQty;
                        calculatedOnHandQty = newOnHandQtyPerLocationGroup;
                        if (itemSummaryGroup.BalanceQty.GetValueOrDefault(0) <= 0)
                            calculatedOnHandQty = newQty;

                        calculatedCostPrice = itemSummaryGroup.CostPrice.GetValueOrDefault(0);
                        if (!(movementType.ToUpper() == "ADJUSTMENT IN"))
                        {
                            if (calculatedCostPrice <= 0 || itemSummaryGroup.BalanceQty.GetValueOrDefault(0) <= 0)
                                calculatedCostPrice = 0;

                            if (calculatedOnHandQty != 0)
                                costPricePerLocationGroup = (((decimal)itemSummaryGroup.BalanceQty.GetValueOrDefault(0) * calculatedCostPrice) + ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                            else
                                costPricePerLocationGroup = 0;

                            if (newOnHandQtyPerLocationGroup <= 0)
                                costPricePerLocationGroup = 0;
                        }
                        else
                        {
                            costPricePerLocationGroup = newCostPrice;
                            if (newOnHandQtyPerLocationGroup <= 0)
                                costPricePerLocationGroup = 0;
                        } 
                    }

                    #endregion

                    #region *) Item Per Inventory Location

                    newOnHandQtyPerLocation = itemSummary.BalanceQty.GetValueOrDefault(0) + newQty;
                    calculatedOnHandQty = newOnHandQtyPerLocation;
                    if (itemSummary.BalanceQty.GetValueOrDefault(0) <= 0)
                        calculatedOnHandQty = newQty;

                    calculatedCostPrice = itemSummary.CostPrice.GetValueOrDefault(0);
                    if (!(movementType.ToUpper() == "ADJUSTMENT IN"))
                    {
                        if (calculatedCostPrice <= 0 || itemSummary.BalanceQty.GetValueOrDefault(0) <= 0)
                            calculatedCostPrice = 0;

                        if (calculatedOnHandQty != 0)
                            costPricePerLocation = (((decimal)itemSummary.BalanceQty.GetValueOrDefault(0) * calculatedCostPrice) + ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                        else
                            costPricePerLocation = 0;

                        if (newOnHandQtyPerLocation <= 0)
                            costPricePerLocation = 0;
                    }
                    else
                    {
                        costPricePerLocation = newCostPrice;
                        if (newOnHandQtyPerLocation <= 0)
                        {
                            costPricePerLocation = 0;
                        }
                    }

                    #endregion

                    #region *) Item over all

                    newOnHandQtyPerItem = theItem.BalanceQuantity.GetValueOrDefault(0) + newQty;
                    calculatedOnHandQty = newOnHandQtyPerItem;
                    if (theItem.BalanceQuantity.GetValueOrDefault(0) <= 0)
                        calculatedOnHandQty = newQty;

                    calculatedCostPrice = theItem.AvgCostPrice.GetValueOrDefault(0);
                    if (!(movementType.ToUpper() == "ADJUSTMENT IN"))
                    {
                        if (calculatedCostPrice <= 0 || theItem.BalanceQuantity.GetValueOrDefault(0) <= 0)
                            calculatedCostPrice = 0;

                        if (calculatedOnHandQty != 0)
                            costPricePerItem = (((decimal)theItem.BalanceQuantity.GetValueOrDefault(0) * calculatedCostPrice) + ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                        else
                            costPricePerItem = 0;

                        if (newOnHandQtyPerItem <= 0)
                            costPricePerItem = 0;
                    }
                    else
                    {
                        if (calculatedOnHandQty < 0)
                        {
                            costPricePerItem = theItem.AvgCostPrice.GetValueOrDefault(0);
                        }
                    }
                    #endregion
                }
                else
                {
                    if (isUseLocGroup)
                    {
                        newOnHandQtyPerLocationGroup = itemSummaryGroup.BalanceQty.GetValueOrDefault(0) - newQty;
                        costPricePerLocationGroup = itemSummaryGroup.CostPrice.GetValueOrDefault(0); 
                    }

                    #region *) Item per Inventory Location
                    newOnHandQtyPerLocation = itemSummary.BalanceQty.GetValueOrDefault(0) - newQty;
                    
                    calculatedOnHandQty = newOnHandQtyPerLocation;
                    if (itemSummary.BalanceQty.GetValueOrDefault(0) <= 0)
                        calculatedOnHandQty = newQty;

                    calculatedCostPrice = itemSummary.CostPrice.GetValueOrDefault(0);
                    if (movementType.ToUpper() == "RETURN OUT" && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.StockReturnNoAffectCOGS), false))
                    {
                        if (calculatedCostPrice <= 0 || itemSummary.BalanceQty.GetValueOrDefault(0) <= 0)
                            calculatedCostPrice = 0;

                        if (calculatedOnHandQty != 0)
                            costPricePerLocation = (((decimal)itemSummary.BalanceQty.GetValueOrDefault(0) * calculatedCostPrice) - ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                        else
                            costPricePerLocation = 0;

                        if (newOnHandQtyPerLocation <= 0)
                            costPricePerLocation = 0;
                    }
                    else
                    {
                        costPricePerLocation = itemSummary.CostPrice.GetValueOrDefault(0);
                    }
                    #endregion

                    #region *) Item over all

                    newOnHandQtyPerItem = theItem.BalanceQuantity.GetValueOrDefault(0) - newQty;

                    calculatedOnHandQty = newOnHandQtyPerItem;
                    if (theItem.BalanceQuantity.GetValueOrDefault(0) <= 0)
                        calculatedOnHandQty = newQty;

                    calculatedCostPrice = theItem.AvgCostPrice.GetValueOrDefault(0);
                    if ((movementType.ToUpper() == "RETURN OUT") && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.StockReturnNoAffectCOGS), true))
                    {
                        if (calculatedCostPrice <= 0 || theItem.BalanceQuantity.GetValueOrDefault(0) <= 0)
                            calculatedCostPrice = 0;

                        if (calculatedOnHandQty != 0)
                            costPricePerItem = (((decimal)theItem.BalanceQuantity.GetValueOrDefault(0) * calculatedCostPrice) - ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                        else
                            costPricePerItem = 0;

                        if (newOnHandQtyPerItem <= 0)
                            costPricePerItem = 0;
                    }
                    else
                    {
                        costPricePerItem = theItem.AvgCostPrice.GetValueOrDefault(0);                        
                    }
                    #endregion
                }

                #endregion

                #region *) Update Item Summary Group

                if (isUseLocGroup)
                {
                    itemSummaryGroup.BalanceQty = newOnHandQtyPerLocation;
                    if (itemSummaryGroup.BalanceQty > 0)
                    {
                        if (movementType.ToUpper() != "ADJUSTMENT IN")
                            itemSummaryGroup.CostPrice = costPricePerLocation;
                    }
                    else
                        itemSummaryGroup.CostPrice = 0;

                    string sqlItemSummaryGroup = @"UPDATE ItemSummaryGroup
                                              SET  ModifiedOn = GETDATE()
                                                  ,ModifiedBy = 'SYNC'
                                                  ,BalanceQty = ISNULL(BalanceQty,0) {0} {1} ";
                    if (movementType.ToUpper().Contains("IN") && movementType.ToUpper() != "ADJUSTMENT IN")
                        sqlItemSummaryGroup += ",CostPrice = ((costPrice*case when balanceqty < 0 then 0 else balanceqty end)  {0} " + ((decimal)newQty * newCostPrice).ToString("0.0000") +
                                           ") / (case when balanceqty < 0 then 0 else balanceqty end + " + newQty.ToString() + ") ";
                    sqlItemSummaryGroup += "WHERE ItemSummaryGroupID = '{2}'";
                    sqlItemSummaryGroup = string.Format(sqlItemSummaryGroup, movementType.ToUpper().Contains("IN") ? "+" : "-"
                                                                           , newQty.ToString()
                                                                           , itemSummaryGroup.ItemSummaryGroupID);

                    qmc.Add(new QueryCommand(sqlItemSummaryGroup)); 
                }

                #endregion

                #region *) Update Item Summary

                 double calculatedOnHandQtyItemSummary = 0 ;
                 if (movementType.ToUpper().Contains("IN"))
                     calculatedOnHandQtyItemSummary = itemSummary.BalanceQty.GetValueOrDefault(0) + newQty;
                 else
                     calculatedOnHandQtyItemSummary = itemSummary.BalanceQty.GetValueOrDefault(0) - newQty;

                itemSummary.BalanceQty = newOnHandQtyPerLocation;
                if (itemSummary.BalanceQty > 0)
                {
                    if (movementType.ToUpper() != "ADJUSTMENT IN")
                        itemSummary.CostPrice = costPricePerLocation;
                }
                else
                    itemSummary.CostPrice = 0;

                
                string sqlItemSummary = @"UPDATE ItemSummary
                                              SET  ModifiedOn = GETDATE()
                                                  ,ModifiedBy = 'SYNC'
                                                  ,BalanceQty = ISNULL(BalanceQty,0) {0} {1} ";
                if ((movementType.ToUpper().Contains("IN") && movementType.ToUpper() != "ADJUSTMENT IN") || (movementType.ToUpper() == "RETURN OUT" && calculatedOnHandQtyItemSummary != 0 && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.StockReturnNoAffectCOGS), true)))
                {
                    sqlItemSummary += ",CostPrice = ((costPrice*case when balanceqty < 0 then 0 else balanceqty end)  {0} " + ((decimal)newQty * newCostPrice).ToString("0.0000") +
                                       ") / (case when balanceqty < 0 then 0 else balanceqty end {0} " + newQty.ToString() + ") ";
                }
                sqlItemSummary += "WHERE ItemSummaryID = '{2}'";
                sqlItemSummary = string.Format(sqlItemSummary, movementType.ToUpper().Contains("IN") ? "+" : "-"
                                                             , newQty.ToString()
                                                             , itemSummary.ItemSummaryID);
                
                qmc.Add(new QueryCommand(sqlItemSummary));


                #endregion

                #region *) Update Item

                theItem.BalanceQuantity = newOnHandQtyPerItem;
                if (theItem.BalanceQuantity > 0)
                {
                    if (movementType.ToUpper() != "ADJUSTMENT IN")
                        theItem.AvgCostPrice = costPricePerItem;
                }
                else
                {
                    theItem.AvgCostPrice = 0;
                    costPricePerItem = 0;
                }

                if ((movementType.ToUpper() == "STOCK IN" || movementType.ToUpper() == "TRANSFER IN") && invLoc.DontUpdateItemAvgCostPrice.GetValueOrDefault(false) == true)
                {
                    string sqlItem = @"UPDATE  Item
                                       SET	   BalanceQuantity = ISNULL(BalanceQuantity,0) {0} {1} ";
                    sqlItem += "WHERE   ItemNo = '{2}'";
                    sqlItem = string.Format(sqlItem, movementType.ToUpper().Contains("IN") ? "+" : "-"
                                                   , newQty.ToString()
                                                   , itemSummary.ItemNo);
                    qmc.Add(new QueryCommand(sqlItem));
                }
                else
                {
                    string sqlItem = @"UPDATE  Item
                                   SET	   BalanceQuantity = ISNULL(BalanceQuantity,0) {0} {1} ";
                    if ((movementType.ToUpper().Contains("IN") && movementType.ToUpper() != "ADJUSTMENT IN") || (movementType.ToUpper() == "RETURN OUT" && calculatedOnHandQty != 0 && !AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.StockReturnNoAffectCOGS), false)))
                    {
                        sqlItem += ",AvgCostPrice = ((ISNULL(AvgCostPrice,0)* Case when ISNULL(BalanceQuantity,0) < 0 then 0 else ISNULL(BalanceQuantity,0) end)  {0} "
                                                + ((decimal)newQty * newCostPrice).ToString("0.0000") +
                                                       ") / (Case when ISNULL(BalanceQuantity,0) < 0 then 0 else ISNULL(BalanceQuantity,0) end {0} " + newQty.ToString() + ") ";
                    }
                    sqlItem += "WHERE   ItemNo = '{2}'";
                    sqlItem = string.Format(sqlItem, movementType.ToUpper().Contains("IN") ? "+" : "-"
                                                   , newQty.ToString()
                                                   , itemSummary.ItemNo);
                    qmc.Add(new QueryCommand(sqlItem));
                }
                #endregion

                #region *) Update Inventory Det

                string sqlInvDet = "";
                if (movementType.ToUpper() != "ADJUSTMENT IN" && movementType.ToUpper() != "ADJUSTMENT OUT")
                {
                    sqlInvDet = @"UPDATE  InventoryDet
                                  SET     CostPriceByItem = ISNULL(Item.AvgCostPrice,0)
                                         ,CostPriceByItemInvLoc = ItemSummary.CostPrice
                                  FROM Item , ItemSummary    
                                  WHERE	  InventoryDetRefNo = '{0}' AND Item.ItemNo = InventoryDet.ItemNo 
                                  AND ItemSummary.ItemNo = InventoryDet.ItemNo AND ItemSummary.InventoryLocationID = {1}";

                    sqlInvDet = string.Format(sqlInvDet, invDetID
                                                       , inventoryLocID);
                }
                else
                {
                    /*sqlInvDet = @"UPDATE  InventoryDet
                                  SET     CostPriceByItem = ISNULL(Item.AvgCostPrice,0)
                                         ,CostPriceByItemInvLoc = ItemSummary.CostPrice
                                         ,FactoryPrice = ItemSummary.CostPrice
                                         ,CostOfGoods = ItemSummary.CostPrice * InventoryDet.Quantity 
                                  FROM Item , ItemSummary    
                                  WHERE	  InventoryDetRefNo = '{0}' AND Item.ItemNo = InventoryDet.ItemNo 
                                  AND ItemSummary.ItemNo = InventoryDet.ItemNo AND ItemSummary.InventoryLocationID = {1}";*/
                    sqlInvDet = @"UPDATE  InventoryDet
                                  SET     CostPriceByItem = ISNULL(Item.AvgCostPrice,0)
                                         ,CostPriceByItemInvLoc = ItemSummary.CostPrice
                                  FROM Item , ItemSummary    
                                  WHERE	  InventoryDetRefNo = '{0}' AND Item.ItemNo = InventoryDet.ItemNo 
                                  AND ItemSummary.ItemNo = InventoryDet.ItemNo AND ItemSummary.InventoryLocationID = {1}";

                    sqlInvDet = string.Format(sqlInvDet, invDetID
                                                       , inventoryLocID);
                }

                qmc.Add(new QueryCommand(sqlInvDet));

                if (isUseLocGroup)
                {
                    sqlInvDet = @"UPDATE  InventoryDet
                                  SET     CostPriceByItemInvGroup = ItemSummaryGroup.CostPrice
                                  FROM Item , ItemSummaryGroup   
                                  WHERE	  InventoryDetRefNo = '{0}' AND Item.ItemNo = InventoryDet.ItemNo 
                                  AND ItemSummaryGroup.ItemNo = InventoryDet.ItemNo AND ItemSummaryGroup.InventoryLocationGroupID = {1}";

                    sqlInvDet = string.Format(sqlInvDet, invDetID
                                                       , invLocGroup.InventoryLocationGroupID);
                    qmc.Add(new QueryCommand(sqlInvDet));
                }

                #endregion

                costOfGoods = Convert.ToDecimal(newQty) * costPricePerLocation;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }

            return qmc;
        }

        #region *) Obsolete

        public static bool GenerateItemSummary()
        {
            bool isSuccess = true;

            try
            {
                string updateScript = @"UPDATE Item SET BalanceQuantity = 0, AvgCostPrice = 0, ModifiedOn = GETDATE(), ModifiedBy = 'AUTO GENERATE ItemSummary'
                                        UPDATE ItemSummary SET BalanceQty = 0, CostPrice = 0, ModifiedOn = GETDATE(), ModifiedBy = 'AUTO GENERATE ItemSummary'
                                        UPDATE InventoryDet SET CostPriceByItem = 0, CostPriceByItemInvLoc = 0, ModifiedOn = GETDATE(), ModifiedBy = 'AUTO GENERATE ItemSummary'";
                DataService.ExecuteQuery(new QueryCommand(updateScript));

                string selectScript = @"SELECT InventoryHdrRefNo FROM InventoryHdr ORDER BY InventoryDate ASC";
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(selectScript)));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!UpdateItemSummary(new List<string>() { (string)dt.Rows[i]["InventoryHdrRefNo"] }))
                    {
                        Logger.writeLog(">>>>> FAILED UPDATE Item Summary : " + (string)dt.Rows[i]["InventoryHdrRefNo"]);
                        isSuccess = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        private static bool UpdateItemSummary(List<string> inventoryHdrList)
        {
            bool isSuccess = true;

            try
            {
                string sql = @"SELECT    IH.InventoryLocationID
		                                ,ID.ItemNo
                                        ,IH.MovementType
                                        ,CAST(ID.Quantity AS DECIMAL(38,8)) Quantity
                                        ,ID.RemainingQty
                                        ,ID.FactoryPrice
                                        ,ID.CostOfGoods
                                        ,ID.InventoryDetRefNo
                                FROM	InventoryHdr IH
		                                INNER JOIN InventoryDet ID ON ID.InventoryHdrRefNo = IH.InventoryHdrRefNo
                                WHERE	IH.InventoryHdrRefNo IN ( '{0}' )
                                ORDER BY IH.InventoryDate ASC";
                sql = string.Format(sql, string.Join("','", inventoryHdrList.ToArray()));
                DataTable dt = new DataTable();
                dt.Load(DataService.GetReader(new QueryCommand(sql)));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    isSuccess &= UpdateCostPriceAVG((string)dt.Rows[i]["ItemNo"]
                                            , (int)dt.Rows[i]["InventoryLocationID"]
                                            , (string)dt.Rows[i]["MovementType"]
                                            , (decimal)dt.Rows[i]["FactoryPrice"]
                                            , Convert.ToDouble((decimal)dt.Rows[i]["Quantity"])
                                            , (string)dt.Rows[i]["InventoryDetRefNo"]);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return isSuccess;
        }

        private static bool UpdateCostPriceAVG(string itemNo, int inventoryLocID, string movementType, decimal newCostPrice, double newQty, string invDetID)
        {
            bool isSuccess = true;

            try
            {
                double onHandQty = 0;
                decimal costPrice = 0;
                decimal costPriceItem = 0;
                double onHandQtyItem = 0;
                double calculatedOnHandQty = 0;
                decimal calculatedCostPrice = 0;

                Query qr = new Query("ItemSummary");
                qr.AddWhere(ItemSummary.Columns.ItemNo, itemNo);
                qr.AddWhere(ItemSummary.Columns.InventoryLocationID, inventoryLocID);
                var itemSummary = new ItemSummaryController().FetchByQuery(qr).FirstOrDefault();
                var theItem = new Item(Item.Columns.ItemNo, itemNo);

                


                QueryCommandCollection qmc = new QueryCommandCollection();

                if (itemSummary == null)
                {
                    itemSummary = new ItemSummary();
                    itemSummary.ItemSummaryID = itemNo + "-" + inventoryLocID;
                    itemSummary.ItemNo = itemNo;
                    itemSummary.InventoryLocationID = inventoryLocID;
                    itemSummary.Deleted = false;
                    itemSummary.UniqueID = Guid.NewGuid();
                    itemSummary.BalanceQty = 0;
                    itemSummary.CostPrice = 0;
                }

                decimal costPricePerInvLoc = 0;
                decimal costPricePerItem = 0;

                #region *)Get Cost Price 
                if (movementType.ToUpper().Contains("ADJUSTMENT"))
                {
                    InventoryDet invDet2 = new InventoryDet(invDetID);
                    ViewInventoryActivityCollection vInventory = new ViewInventoryActivityCollection();
                    vInventory.Where(ViewInventoryActivity.Columns.ItemNo, itemNo);
                    vInventory.Where(ViewInventoryActivity.Columns.InventoryLocationID, inventoryLocID);
                    //vInventory.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "Stock Out");
                    vInventory.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.LessThan, invDet2.InventoryHdr.InventoryDate);
                    vInventory.OrderByDesc(ViewInventoryActivity.Columns.InventoryDate);
                    vInventory.Load();


                    if (vInventory.Count > 0)
                    {
                        InventoryDet id = new InventoryDet(vInventory[0].InventoryDetRefNo);
                        costPricePerInvLoc = id.CostPriceByItemInvLoc.GetValueOrDefault(0);
                        //costPricePerItem = id.CostPriceByItem.GetValueOrDefault(0);
                    }
                    else
                    {
                        InventoryDet invDet1 = new InventoryDet(invDetID);
                        ViewInventoryActivityCollection vInventory1 = new ViewInventoryActivityCollection();
                        vInventory1.Where(ViewInventoryActivity.Columns.ItemNo, itemNo);
                        vInventory1.Where(ViewInventoryActivity.Columns.InventoryLocationID, inventoryLocID);
                        //vInventory.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "Stock Out");
                        vInventory1.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.GreaterThan, invDet1.InventoryHdr.InventoryDate);
                        vInventory1.OrderByAsc(ViewInventoryActivity.Columns.InventoryDate);
                        vInventory1.Load();

                        if (vInventory1.Count > 0)
                        {
                            InventoryDet id = new InventoryDet(vInventory1[0].InventoryDetRefNo);
                            costPricePerInvLoc = id.CostPriceByItemInvLoc.GetValueOrDefault(0);
                            //costPricePerItem = id.CostPriceByItem.GetValueOrDefault(0);
                        }
                        else
                        {
                            costPricePerInvLoc = theItem.FactoryPrice;
                            //costPricePerItem = theItem.FactoryPrice;
                        }
                    }

                    ViewInventoryActivityCollection vInventoryAll = new ViewInventoryActivityCollection();
                    vInventoryAll.Where(ViewInventoryActivity.Columns.ItemNo, itemNo);
                    //vInventoryAll.Where(ViewInventoryActivity.Columns.InventoryLocationID, inventoryLocID);
                    //vInventory.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "Stock Out");
                    vInventoryAll.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.LessThan, invDet2.InventoryHdr.InventoryDate);
                    vInventoryAll.OrderByDesc(ViewInventoryActivity.Columns.InventoryDate);
                    vInventoryAll.Load();


                    if (vInventoryAll.Count > 0)
                    {
                        InventoryDet id = new InventoryDet(vInventoryAll[0].InventoryDetRefNo);
                        //costPricePerInvLoc = id.CostPriceByItemInvLoc.GetValueOrDefault(0);
                        costPricePerItem = id.CostPriceByItem.GetValueOrDefault(0);
                    }
                    else
                    {
                        InventoryDet invDet1 = new InventoryDet(invDetID);
                        ViewInventoryActivityCollection vInventory1 = new ViewInventoryActivityCollection();
                        vInventory1.Where(ViewInventoryActivity.Columns.ItemNo, itemNo);
                        //vInventory1.Where(ViewInventoryActivity.Columns.InventoryLocationID, inventoryLocID);
                        //vInventory.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "Stock Out");
                        vInventory1.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.GreaterThan, invDet1.InventoryHdr.InventoryDate);
                        vInventory1.OrderByAsc(ViewInventoryActivity.Columns.InventoryDate);
                        vInventory1.Load();

                        if (vInventory1.Count > 0)
                        {
                            InventoryDet id = new InventoryDet(vInventory1[0].InventoryDetRefNo);
                            //costPricePerInvLoc = id.CostPriceByItemInvLoc.GetValueOrDefault(0);
                            costPricePerItem = id.CostPriceByItem.GetValueOrDefault(0);
                        }
                        else
                        {
                            //costPricePerInvLoc = theItem.FactoryPrice;
                            costPricePerItem = theItem.FactoryPrice;
                        }
                    }


                }

                #endregion

                #region *) Calculate AVG Costing

                if (movementType.ToUpper().Contains("IN"))
                {
                    #region *) Item per Inventory Location

                    onHandQty = itemSummary.BalanceQty.GetValueOrDefault(0) + newQty;
                    calculatedOnHandQty = onHandQty;
                    if (itemSummary.BalanceQty.GetValueOrDefault(0) <= 0)
                        calculatedOnHandQty = newQty;

                    calculatedCostPrice = itemSummary.CostPrice.GetValueOrDefault(0);
                    if (!(movementType.ToUpper() == "ADJUSTMENT IN"))
                    {
                        if (calculatedCostPrice <= 0 || itemSummary.BalanceQty.GetValueOrDefault(0) <= 0)
                            calculatedCostPrice = 0;

                        if (calculatedOnHandQty != 0)
                            costPrice = (((decimal)itemSummary.BalanceQty.GetValueOrDefault(0) * calculatedCostPrice) + ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                        else
                            costPrice = 0;

                        if (onHandQty <= 0)
                            costPrice = 0;
                    }
                    else
                    {
                        

                        costPrice = newCostPrice;
                        if (onHandQty <= 0)
                        {
                            costPrice = 0;
                        }
                    }
                    
                    #endregion

                    #region *) Item over all

                    onHandQtyItem = theItem.BalanceQuantity.GetValueOrDefault(0) + newQty;
                    calculatedOnHandQty = onHandQtyItem;
                    if (theItem.BalanceQuantity.GetValueOrDefault(0) <= 0)
                        calculatedOnHandQty = newQty;

                    calculatedCostPrice = theItem.AvgCostPrice.GetValueOrDefault(0);
                    if (!(movementType.ToUpper() == "ADJUSTMENT IN"))
                    {
                        if (calculatedCostPrice <= 0 || theItem.BalanceQuantity.GetValueOrDefault(0) <= 0)
                            calculatedCostPrice = 0;

                        if (calculatedOnHandQty != 0)
                            costPriceItem = (((decimal)theItem.BalanceQuantity.GetValueOrDefault(0) * calculatedCostPrice) + ((decimal)newQty * newCostPrice)) / (decimal)calculatedOnHandQty;
                        else
                            costPriceItem = 0;

                        if (onHandQtyItem <= 0)
                            costPriceItem = 0;
                    }
                    else
                    {
                        if (calculatedOnHandQty < 0)
                        {
                            costPriceItem = theItem.AvgCostPrice.GetValueOrDefault(0);
                        }
                    }
                    #endregion
                }
                else
                {
                    onHandQty = itemSummary.BalanceQty.GetValueOrDefault(0) - newQty;
                    costPrice = itemSummary.CostPrice.GetValueOrDefault(0);

                    onHandQtyItem = theItem.BalanceQuantity.GetValueOrDefault(0) - newQty;
                    costPriceItem = theItem.AvgCostPrice.GetValueOrDefault(0);
                }

                #endregion

                #region *) Update Item Summary

                itemSummary.BalanceQty = onHandQty;
                if (itemSummary.BalanceQty > 0)
                {
                    if (movementType.ToUpper() != "ADJUSTMENT IN")
                        itemSummary.CostPrice = costPrice;
                }
                else
                    itemSummary.CostPrice = 0;
                if (itemSummary.IsNew)
                    qmc.Add(itemSummary.GetInsertCommand("SYNC"));
                else
                    qmc.Add(itemSummary.GetUpdateCommand("SYNC"));

                #endregion

                #region *) Update Item

                
                theItem.BalanceQuantity = onHandQtyItem;
                if (theItem.BalanceQuantity > 0)
                {
                    if (movementType.ToUpper() != "ADJUSTMENT IN")
                        theItem.AvgCostPrice = costPriceItem;
                }
                else
                {
                    theItem.AvgCostPrice = 0;
                    costPriceItem = 0;
                }

                string sqlString = "Update Item Set AvgCostPrice = " + theItem.AvgCostPrice.GetValueOrDefault(0).ToString("F4") + ", BalanceQuantity = " + onHandQtyItem.ToString("F2") +
                    " Where ItemNo = '" + theItem.ItemNo + "'";
                //qmc.Add(theItem.GetUpdateCommand("SYNC"));
                qmc.Add(new QueryCommand(sqlString));
                #endregion

                #region *) Update Inventory Det

                InventoryDet invDet = new InventoryDet(InventoryDet.Columns.InventoryDetRefNo, invDetID);
                if (movementType.ToUpper() != "ADJUSTMENT IN" && movementType.ToUpper() != "ADJUSTMENT OUT")
                {
                    invDet.CostPriceByItem = theItem.AvgCostPrice;
                    invDet.CostPriceByItemInvLoc = itemSummary.CostPrice;
                }
                else
                {
                    invDet.CostPriceByItem = costPricePerItem;
                    invDet.CostPriceByItemInvLoc = costPricePerInvLoc;
                    invDet.FactoryPrice = costPricePerInvLoc;
                    if (movementType.ToUpper() == "ADJUSTMENT OUT")
                        invDet.CostOfGoods = invDet.FactoryPrice * invDet.Quantity.GetValueOrDefault(0);
                    else
                        invDet.CostOfGoods = invDet.FactoryPrice;
                }

                qmc.Add(invDet.GetUpdateCommand("SYNC"));

                #endregion

                DataService.ExecuteTransaction(qmc);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                isSuccess = false;
            }

            return isSuccess;
        }

        #endregion
    }
}
