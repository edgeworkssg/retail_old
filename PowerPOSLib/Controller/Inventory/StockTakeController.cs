using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using PowerPOS.Container;
using PowerPOS;
using SubSonic;
using System.ComponentModel;

namespace PowerPOS
{
    public partial class StockTakeController
    {
        public const int STOCKTAKE_ADJUSTMENT_REASONID = 2;

        public static bool IsInventoryLocationFrozen(int inventoryLocationID)
        {
            InventoryLocation invLoc = new InventoryLocation(inventoryLocationID);
            if (invLoc != null && invLoc.InventoryLocationID == inventoryLocationID)
            {
                return invLoc.IsFrozen;
            }
            else
            {
                return false;
            }

        }

        public static InventoryLocationCollection GetAllLocationWithOutstandingStockTake()
        {
            InventoryLocationCollection result = new InventoryLocationCollection();
            //Query qr = StockTake.CreateQuery();
            //qr.QueryType = QueryType.Select;
            //qr.DISTINCT();
            //qr.SelectList = StockTake.Columns.InventoryLocationID;
            //qr.AddWhere(StockTake.Columns.IsAdjusted, false);
            //DataSet ds = qr.ExecuteDataSet();

            string sql = "SELECT DISTINCT InventoryLocationID FROM StockTake WHERE IsAdjusted = 0 AND ISNULL(" + StockTake.UserColumns.Deleted + ", 0) = 0";
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.OnlyAllowInCurrentInventoryLocation), false))
                    {
                        if (int.Parse(ds.Tables[0].Rows[i]["InventoryLocationID"].ToString()) == PointOfSaleInfo.InventoryLocationID)
                            result.Add(new InventoryLocation(int.Parse(ds.Tables[0].Rows[i]["InventoryLocationID"].ToString())));
                    }
                    else
                    {
                        result.Add(new InventoryLocation(int.Parse(ds.Tables[0].Rows[i]["InventoryLocationID"].ToString())));
                    }
                }
            }
            return result;
        }
        public bool updateMarked(int stockTakeId, bool bit)
        {
            try
            {
                Query qr = StockTake.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddWhere(StockTake.Columns.StockTakeID, stockTakeId);
                qr.AddUpdateSetting(StockTake.Columns.Marked, bit);
                qr.Execute();
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        //fetch stock take....
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable FetchByLocation(int InventoryLocationID,bool displayCostPrice)
        {
            ViewStockTakeCollection coll;
            if (InventoryLocationID == 0)
            {
                //Fetch all
                coll = new ViewStockTakeCollection()
                    .Where("IsAdjusted", false)
                    .OrderByDesc(StockTake.Columns.StockTakeID)
                    .Load();
            }
            else
            {
                //Fetch by category...
                coll = new ViewStockTakeCollection()
                    .Where("InventoryLocationID", InventoryLocationID)
                    .Where("IsAdjusted", false)
                    .OrderByDesc(StockTake.Columns.StockTakeID)
                    .Load();
            }

            DataTable dt = coll.ToDataTable();
            dt.Columns.Add("SystemBalQty", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Defi", System.Type.GetType("System.Int32"));
            if (displayCostPrice) dt.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            string status;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                dt.Rows[i]["SystemBalQty"] =
                    InventoryController.GetStockBalanceQtyByItemByDate
                    (dt.Rows[i]["ItemNo"].ToString(), InventoryLocationID,
                    DateTime.Parse(dt.Rows[i]["StockTakeDate"].ToString()),
                    out status);
                dt.Rows[i]["Defi"] = (int)dt.Rows[i]["StockTakeQty"] - (int)dt.Rows[i]["SystemBalQty"];
                if (displayCostPrice)  dt.Rows[i]["TotalCost"] = (int)dt.Rows[i]["Defi"] * (decimal)dt.Rows[i]["CostOfGoods"];
            }

            return dt;
        }

        //fetch stock take....
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable FetchByLocationWithFilter(int InventoryLocationID, bool displayCostPrice, string filter)
        {
            ViewStockTakeCollection coll;
            string cmd = "Select * from ViewStockTake where isAdjusted = 0 and isnull(Deleted, 0) = 0 ";
            if (InventoryLocationID == 0)
            {

            }
            else
            {
                cmd = cmd + "and InventoryLocationID = " + InventoryLocationID;//Fetch by category...

            }

            cmd = cmd + " and itemNo +ItemName + TakenBy like '%" + filter + "%' Order by StockTakeID desc";

            DataTable dt = DataService.GetDataSet(new QueryCommand(cmd)).Tables[0];
            dt.Columns.Add("SystemBalQty", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Defi", System.Type.GetType("System.Decimal"));
            if (displayCostPrice) dt.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            string status;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                dt.Rows[i]["SystemBalQty"] =
                    dt.Rows[i]["BalQtyAtEntry"];
                /*InventoryController.GetStockBalanceQtyByItemByDate
                (dt.Rows[i]["ItemNo"].ToString(), InventoryLocationID,
                DateTime.Parse(dt.Rows[i]["StockTakeDate"].ToString()),
                out status);*/
                dt.Rows[i]["Defi"] = (decimal)dt.Rows[i]["StockTakeQty"] - (decimal)dt.Rows[i]["SystemBalQty"];
                if (displayCostPrice) dt.Rows[i]["TotalCost"] = (decimal)dt.Rows[i]["Defi"] * (decimal)dt.Rows[i]["CostOfGoods"];
            }

            return dt;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DataTable FetchByLocationWithFilterBatchNoParValue(int InventoryLocationID, bool displayCostPrice, string filter)
        {
            ViewStockTakeCollection coll;
            //Get userfld for DepartmentOU 
            AppSettingCollection qr1 = new AppSettingCollection();
            qr1.Where(AppSetting.Columns.AppSettingValue, "Par Value");
            qr1.Load();

            string userfldParValue = "";
            if (qr1.Count > 0)
            {
                string userfld1 = qr1[0].AppSettingKey;
                userfldParValue = userfld1.Substring(6, userfld1.Length - 6);
            }

            string cmd = @"Select vs.*, {0} 
                            from viewstocktake vs
                            left outer join ItemQuantityTrigger iq on vs.ItemNo = iq.ItemNo and vs.InventoryLocationID = iq.InventoryLocationID
                            where isAdjusted = 0 and ISNULL(vs.itemNo,'') + ISNULL(vs.ItemName,'') + ISNULL(vs.TakenBy,'') + ISNULL(vs.BatchNo,'') like '%" + filter + "%' ";

            if (string.IsNullOrEmpty(userfldParValue))
            {
                cmd = string.Format(cmd, " '' as [ParValue] ");
            }
            else
            {
                cmd = string.Format(cmd, " ISNULL(iq." + userfldParValue + ",'') as [ParValue] ");
            }

            if (InventoryLocationID != 0)
            {
                cmd = cmd + "and vs.InventoryLocationID = " + InventoryLocationID;//Fetch by category...
            }

            cmd = cmd + " Order by vs.StockTakeID desc";

            DataTable dt = DataService.GetDataSet(new QueryCommand(cmd)).Tables[0];
            dt.Columns.Add("SystemBalQty", System.Type.GetType("System.Decimal"));
            dt.Columns.Add("Defi", System.Type.GetType("System.Decimal"));
            if (displayCostPrice) dt.Columns.Add("TotalCost", System.Type.GetType("System.Decimal"));
            string status;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                dt.Rows[i]["SystemBalQty"] =
                    dt.Rows[i]["BalQtyAtEntry"];

                dt.Rows[i]["Defi"] = (decimal)dt.Rows[i]["StockTakeQty"] - (decimal)dt.Rows[i]["SystemBalQty"];
                if (displayCostPrice) dt.Rows[i]["TotalCost"] = (decimal)dt.Rows[i]["Defi"] * (decimal)dt.Rows[i]["CostOfGoods"];
            }

            return dt;
        }

        public static DataTable FetchMissedOutItemWithFilter(int InventoryLocationID, string filter)
        {
//            string sql = @"
//            select ItemNo, ItemName
//            from item 
//            where ISNULL(IsInInventory,0) = 1 and ISNULL(Deleted,0) = 0
//            and ISNULL('ItemNo','') + ISNULL(ItemName,'') like '%{0}%' 
//            and ItemNo NOT IN (select ItemNo from ViewStockTake where ISNULL(IsAdjusted,0) = 0 and InventoryLocationID = {1})
//            ";

            string sql = @"
            DECLARE @OutletName varchar(50)
            DECLARE @OutletGroupID int

            SELECT @OutletName = OutletName , @OutletGroupID = ISNULL(OutletGroupID,0) FROM outlet WHERE InventoryLocationId = {1}

            SELECT i.ItemNo, i.ItemName, ISNULL(ISM.BalanceQty,0) AS Qty
			FROM item i 
            LEFT JOIN OutletGroupItemMap OU ON i.itemno = OU.ItemNo AND ISNULL(OU.Deleted,0) = 0 AND ISNULL(OU.OutletName,'') = @OutletName
            LEFT JOIN ItemSummary iSM ON i.itemno = ISM.ItemNo  and ISM.InventoryLocationID = {1}  
            WHERE ISNULL(i.ItemNo,'') + ISNULL(i.ItemName,'') like '%{0}%' and ISNULL(i.IsInInventory,0) = 1 
            and i.ItemNo NOT IN (select ItemNo from ViewStockTake where ISNULL(IsAdjusted,0) = 0 and InventoryLocationID = {1})
            and CASE WHEN ou.OutletGroupItemMapID IS NOT NULL THEN ISNULL(ou.IsItemDeleted,0) ELSE  i.Deleted END = 0";

            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AddNegativeMissOutItemOnly), false))
            {
                sql = sql + " AND isnull(iSM.BalanceQty,0) <> 0 ";
            }

            sql = string.Format(sql, filter, InventoryLocationID);
            DataTable dt = DataService.GetDataSet(new QueryCommand(sql)).Tables[0];

            return dt;
        }

        #region "Update methods"

        //fetch stock take....
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateStockTake(int StockTakeID, int StockTakeQty, string TakenBy, string VerifiedBy, string Remark)
        {
            if (TakenBy == null) TakenBy = "";
            if (VerifiedBy == null) VerifiedBy = "";
            if (Remark == null) Remark = "";

            Query stockTakeQuery = StockTake.CreateQuery();
            stockTakeQuery.QueryType = QueryType.Update;
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.StockTakeQty, StockTakeQty);
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.TakenBy, TakenBy);
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.VerifiedBy, VerifiedBy);
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.Remark, Remark);
            stockTakeQuery.AddWhere("StockTakeID", StockTakeID);
            stockTakeQuery.Execute();
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateStockTake(int StockTakeID, int StockTakeQty, string TakenBy, string VerifiedBy)
        {
            if (TakenBy == null) TakenBy = "";
            if (VerifiedBy == null) VerifiedBy = "";

            Query stockTakeQuery = StockTake.CreateQuery();
            stockTakeQuery.QueryType = QueryType.Update;
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.StockTakeQty, StockTakeQty);
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.TakenBy, TakenBy);
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.VerifiedBy, VerifiedBy);
            stockTakeQuery.AddWhere("StockTakeID", StockTakeID);
            stockTakeQuery.Execute();
        }

        //fetch stock take....
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateStockTake(int StockTakeID, decimal CostOfGoods, string Remark)
        {
            if (Remark == null) Remark = "";

            Query stockTakeQuery = StockTake.CreateQuery();
            stockTakeQuery.QueryType = QueryType.Update;
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.CostOfGoods, CostOfGoods);
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.Remark, Remark);
            stockTakeQuery.AddWhere("StockTakeID", StockTakeID);
            stockTakeQuery.Execute();
        }

        #endregion
        /*
        public bool AddStockTakeEntry(DateTime selectedDate, string ItemNo,int LocationID, int StockTakeQty, string TakenBy,
            string VerifiedBy, string UserName, out string status)
        {
            try
            {
                status = "";
                
                //Check date validity

                //non existent do insert
                Create(
                    selectedDate,
                    ItemNo, LocationID,
                    StockTakeQty, TakenBy,
                    VerifiedBy, UserName, false, "",
                    InventoryController.FetchCostOfGoodsByItemNo(ItemNo,
                    LocationID), null, "", null, null, ""
                    , "", "", "", "", "", "", "", "", "", "", null, null, null, null,
                    null, null, null, null, null, null, null, null, null, null, null);


                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }
        */
        public bool AddStockTakeEntry
            (DateTime selectedDate, string ItemNo, int LocationID,
            int StockTakeQty, string TakenBy,
            string VerifiedBy, string UserName, out string status)
        {
            try
            {
                status = "";

                //Check date validity

                //non existent do insert
                StockTake stNew = new StockTake();
                stNew.StockTakeDate = selectedDate;
                stNew.ItemNo = ItemNo;
                stNew.InventoryLocationID = LocationID;
                stNew.StockTakeQty = StockTakeQty;
                stNew.TakenBy = TakenBy;
                stNew.VerifiedBy = VerifiedBy;
                //Store Stock On Hand into the database at user int 1
                stNew.BalQtyAtEntry = InventoryController.GetStockBalanceQtyByItemByDate
                    (ItemNo, LocationID, selectedDate, out status);
                stNew.AdjustmentQty = StockTakeQty - stNew.BalQtyAtEntry;
                stNew.CostOfGoods =
                    InventoryController.FetchAverageCostOfGoodsLeftByItemNo(stNew.BalQtyAtEntry.GetValueOrDefault(0), ItemNo, LocationID);
                /*
                stNew.CostOfGoods = InventoryController.FetchCostOfGoodsByItemNo
                                    (ItemNo, LocationID);*/
                stNew.Marked = false;
                stNew.IsNew = true;
                stNew.Save("");

                return true;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                Logger.writeLog(ex);
                return false;
            }
        }

        public bool CreateStockTakeFromInventory(InventoryController inv)
        {
            throw new Exception("Method not implemented");
        }

        public bool CorrectStockTakeDiscrepancy(string username, int LocationID)
        {
            try
            {
                string status;
                QueryCommandCollection cmd = new QueryCommandCollection();

                StockTakeCollection discr = new StockTakeCollection();
                discr.Where(StockTake.Columns.IsAdjusted, false);
                discr.Where(StockTake.Columns.InventoryLocationID, LocationID);
                discr.Where(StockTake.Columns.Marked, true);
                discr.OrderByAsc(StockTake.Columns.ItemNo);
                discr.Load();

                decimal discrQty;

                //pull out
                //store deleted to be adjusted later
                int p = discr.Count - 1;
            while (p > 0)
                {
                    if (discr[p].ItemNo == discr[p - 1].ItemNo)
                    {
                        discr[p - 1].StockTakeQty += discr[p].StockTakeQty;
                        discr[p].IsAdjusted = true;
                        cmd.Add(discr[p].GetUpdateCommand(UserInfo.username));
                        discr.RemoveAt(p);
                    }
                    p--;
                }

                //string InventoryRefNo = InventoryController.getNewInventoryRefNo(LocationID);
                List<string> ListInventoryHdr = new List<string>();

                for (int i = 0; i < discr.Count; i++)
                {

                    //Fetch balance quantity for this item.
                    /*
                    SystemBalQty = InventoryController.GetStockBalanceQtyByItemByDate
                        (discr[i].ItemNo, LocationID, discr[i].StockTakeDate, out status);                    
                     discrQty = discr[i].StockTakeQty - SystemBalQty;
                    */
                    decimal newAdjustQty=discr[i].StockTakeQty.GetValueOrDefault(0) - discr[i].BalQtyAtEntry.GetValueOrDefault(0);;
                    
                    discrQty = newAdjustQty;

                    //for every row, perform stock in or stock out
                    if (discrQty < 0)
                    {
                        discrQty = -discrQty;

                        InventoryController StockOutCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        StockOutCtrl.SetRemark("Stock Take Adj. Id:" + discr[i].StockTakeID);
                        StockOutCtrl.SetInventoryDate(discr[i].StockTakeDate);
                        //INYYMMDDSSSSNNNN                
                        StockOutCtrl.SetInventoryHdrRefNo("ST" + discr[i].StockTakeID.ToString());
                        ListInventoryHdr.Add("ST" + discr[i].StockTakeID.ToString());
                        //Logger.writeLog(StockOutCtrl.GetInvHdrRefNo());

                        //do stock out                                               
                        //decimal cogs = ItemSummaryController.GetAvgCostPrice(discr[i].ItemNo, LocationID);
                        decimal cogs = discr[i].CostOfGoods;
                        /*if (cogs == 0)
                        {
                            Item it = new Item(discr[i].ItemNo);
                            cogs = it.FactoryPrice;
                        }*/
                        StockOutCtrl.AddItemIntoInventoryStockTake(discr[i].ItemNo, discrQty, cogs, out status);
                        StockOutCtrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Stock Take Adj. Id:" + discr[i].StockTakeID, out status);
                        QueryCommandCollection cmdTmp;
                        if (StockOutCtrl.CreateStockOutQueryCommand
                            (username, STOCKTAKE_ADJUSTMENT_REASONID, LocationID, true, true, out status, out cmdTmp))
                        {
                            cmd.AddRange(cmdTmp);

                            Query qr = OrderDet.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                            qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, discr[i].StockTakeDate);
                            qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                            qr.AddWhere(OrderDet.Columns.ItemNo, discr[i].ItemNo);
                            cmd.Add(qr.BuildUpdateCommand());

                        }
                        else
                        {
                            Logger.writeLog(status);
                            return false;
                        }

                    }
                    else if (discrQty > 0)
                    {
                        InventoryController StockInCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        StockInCtrl.SetInventoryDate(discr[i].StockTakeDate);
                        StockInCtrl.SetRemark("Stock Take Adj. Id:" + discr[i].StockTakeID);
                        StockInCtrl.SetInventoryHdrRefNo("ST" + discr[i].StockTakeID.ToString());
                        ListInventoryHdr.Add("ST" + discr[i].StockTakeID.ToString());
                        //decimal cogs = ItemSummaryController.GetAvgCostPrice(discr[i].ItemNo, LocationID);
                        decimal cogs = discr[i].CostOfGoods;
                        if (cogs == 0)
                        {
                            Item it = new Item(discr[i].ItemNo);
                            cogs = it.FactoryPrice;
                        }
                        /*ViewInventoryActivityCollection vInventory = new ViewInventoryActivityCollection();
                        vInventory.Where(ViewInventoryActivity.Columns.ItemNo, discr[i].ItemNo);
                        vInventory.Where(ViewInventoryActivity.Columns.InventoryLocationID, LocationID);
                        vInventory.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "Stock Out");
                        vInventory.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.LessThan, discr[i].StockTakeDate);
                        vInventory.OrderByDesc(ViewInventoryActivity.Columns.InventoryDate);
                        vInventory.Load();

                        if (vInventory.Count > 0)
                        {
                            cogs = vInventory[0].FactoryPrice;
                        }*/


                        StockInCtrl.AddItemIntoInventoryStockTake(discr[i].ItemNo, discrQty, cogs, out status);
                        StockInCtrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Stock Take Adj. Id:" + discr[i].StockTakeID, out status);
                        StockInCtrl.SetInventoryHdrUserName(discr[i].CreatedBy);
                        QueryCommandCollection cmdTmp;
                        try
                        {
                            cmdTmp = StockInCtrl.CreateStockInQueryCommand(username, LocationID, true, false);
                        }
                        catch (Exception X)
                        {
                            status = X.Message.Replace("(error)", "").Replace("(warning)", "");
                            Logger.writeLog(status);
                            return false;
                        }

                        cmd.AddRange(cmdTmp);
                        Query qr = OrderDet.CreateQuery();
                        qr.QueryType = QueryType.Update;
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                        qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, discr[i].StockTakeDate);
                        qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                        qr.AddWhere(OrderDet.Columns.ItemNo, discr[i].ItemNo);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Adjust stock take : {0}", discr[i].StockTakeID), "");
                    //update StockTake and set isadjusted to be true
                    Query qr1 = StockTake.CreateQuery();
                    qr1.AddUpdateSetting(StockTake.Columns.IsAdjusted, true);
                    qr1.AddUpdateSetting(StockTake.Columns.ModifiedOn, DateTime.Now);
                    qr1.AddUpdateSetting(StockTake.Columns.AdjustmentQty, newAdjustQty);
                    qr1.AddWhere(StockTake.Columns.StockTakeID, discr[i].StockTakeID);
                    qr1.QueryType = QueryType.Update;
                    cmd.Add(qr1.BuildUpdateCommand());
                    //InventoryRefNo = InventoryController.getNextInventoryRefNo(InventoryRefNo);

                    // Execute each line of stock take (in trans scope)
                    DataService.ExecuteTransaction(cmd);
                    cmd = new QueryCommandCollection();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Adjusting Stock Discrepancy : " + ex.Message);
                return false;
            }
        }

        public bool CorrectStockTakeDiscrepancyWithStatusMessage(string username, int LocationID, out string status)
        {
            status = "";
            try
            {
                QueryCommandCollection cmd = new QueryCommandCollection();

                StockTakeCollection discr = new StockTakeCollection();
                discr.Where(StockTake.Columns.IsAdjusted, false);
                discr.Where(StockTake.Columns.InventoryLocationID, LocationID);
                discr.Where(StockTake.Columns.Marked, true);
                discr.OrderByAsc(StockTake.Columns.ItemNo);
                discr.Load();

                decimal discrQty;

                //pull out
                //store deleted to be adjusted later
                int p = discr.Count - 1;
                while (p > 0)
                {
                    if (discr[p].ItemNo == discr[p - 1].ItemNo)
                    {
                        discr[p - 1].StockTakeQty += discr[p].StockTakeQty;
                        discr[p].IsAdjusted = true;
                        cmd.Add(discr[p].GetUpdateCommand(UserInfo.username));
                        discr.RemoveAt(p);
                    }
                    p--;
                }

                //string InventoryRefNo = InventoryController.getNewInventoryRefNo(LocationID);
                List<string> ListInventoryHdr = new List<string>();

                for (int i = 0; i < discr.Count; i++)
                {

                    decimal newAdjustQty = discr[i].StockTakeQty.GetValueOrDefault(0) - discr[i].BalQtyAtEntry.GetValueOrDefault(0); ;

                    discrQty = newAdjustQty;

                    //for every row, perform stock in or stock out
                    if (discrQty < 0)
                    {
                        discrQty = -discrQty;

                        InventoryController StockOutCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        StockOutCtrl.SetRemark("Stock Take Adj. Id:" + discr[i].StockTakeID);
                        StockOutCtrl.SetInventoryDate(discr[i].StockTakeDate);
                        //INYYMMDDSSSSNNNN                
                        StockOutCtrl.SetInventoryHdrRefNo("ST" + discr[i].StockTakeID.ToString());
                        ListInventoryHdr.Add("ST" + discr[i].StockTakeID.ToString());
                        //Logger.writeLog(StockOutCtrl.GetInvHdrRefNo());

                        //do stock out                                               
                        //decimal cogs = ItemSummaryController.GetAvgCostPrice(discr[i].ItemNo, LocationID);
                        decimal cogs = discr[i].CostOfGoods;
                        /*if (cogs == 0)
                        {
                            Item it = new Item(discr[i].ItemNo);
                            cogs = it.FactoryPrice;
                        }*/
                        StockOutCtrl.AddItemIntoInventoryStockTake(discr[i].ItemNo, discrQty, cogs, out status);
                        StockOutCtrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Stock Take Adj. Id:" + discr[i].StockTakeID, out status);
                        QueryCommandCollection cmdTmp;
                        if (StockOutCtrl.CreateStockOutQueryCommand
                            (username, STOCKTAKE_ADJUSTMENT_REASONID, LocationID, true, true, out status, out cmdTmp))
                        {
                            cmd.AddRange(cmdTmp);

                            Query qr = OrderDet.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                            qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, discr[i].StockTakeDate);
                            qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                            qr.AddWhere(OrderDet.Columns.ItemNo, discr[i].ItemNo);
                            cmd.Add(qr.BuildUpdateCommand());

                        }
                        else
                        {
                            Logger.writeLog(status);
                            return false;
                        }

                    }
                    else if (discrQty > 0)
                    {
                        InventoryController StockInCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        StockInCtrl.SetInventoryDate(discr[i].StockTakeDate);
                        StockInCtrl.SetRemark("Stock Take Adj. Id:" + discr[i].StockTakeID);
                        StockInCtrl.SetInventoryHdrRefNo("ST" + discr[i].StockTakeID.ToString());
                        ListInventoryHdr.Add("ST" + discr[i].StockTakeID.ToString());
                        //decimal cogs = ItemSummaryController.GetAvgCostPrice(discr[i].ItemNo, LocationID);
                        decimal cogs = discr[i].CostOfGoods;
                        if (cogs == 0)
                        {
                            Item it = new Item(discr[i].ItemNo);
                            cogs = it.FactoryPrice;
                        }

                        StockInCtrl.AddItemIntoInventoryStockTake(discr[i].ItemNo, discrQty, cogs, out status);
                        StockInCtrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Stock Take Adj. Id:" + discr[i].StockTakeID, out status);
                        StockInCtrl.SetInventoryHdrUserName(discr[i].CreatedBy);
                        QueryCommandCollection cmdTmp;
                        try
                        {
                            cmdTmp = StockInCtrl.CreateStockInQueryCommand(username, LocationID, true, false);
                        }
                        catch (Exception X)
                        {
                            status = X.Message.Replace("(error)", "").Replace("(warning)", "");
                            Logger.writeLog(status);
                            return false;
                        }

                        cmd.AddRange(cmdTmp);
                        Query qr = OrderDet.CreateQuery();
                        qr.QueryType = QueryType.Update;
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                        qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, discr[i].StockTakeDate);
                        qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                        qr.AddWhere(OrderDet.Columns.ItemNo, discr[i].ItemNo);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Adjust stock take : {0}", discr[i].StockTakeID), "");
                    //update StockTake and set isadjusted to be true
                    Query qr1 = StockTake.CreateQuery();
                    qr1.AddUpdateSetting(StockTake.Columns.IsAdjusted, true);
                    qr1.AddUpdateSetting(StockTake.Columns.ModifiedOn, DateTime.Now);
                    qr1.AddUpdateSetting(StockTake.Columns.AdjustmentQty, newAdjustQty);
                    qr1.AddWhere(StockTake.Columns.StockTakeID, discr[i].StockTakeID);
                    qr1.QueryType = QueryType.Update;
                    cmd.Add(qr1.BuildUpdateCommand());
                    //InventoryRefNo = InventoryController.getNextInventoryRefNo(InventoryRefNo);

                    // Execute each line of stock take (in trans scope)
                    DataService.ExecuteTransaction(cmd);
                    cmd = new QueryCommandCollection();
                }

                return true;
            }
            catch (Exception ex)
            {
                status = "Error Adjusting Stock Discrepancy : " + ex.Message;

                Logger.writeLog("Error Adjusting Stock Discrepancy : " + ex.Message);
                return false;
            }
        }

        public bool CorrectStockTakeDiscrepancyWithStockDocument(string username, int LocationID, string StockDocumentNo)
        {
            try
            {
                string status;
                QueryCommandCollection cmd = new QueryCommandCollection();

                StockTakeCollection discr = new StockTakeCollection();
                discr.Where(StockTake.Columns.IsAdjusted, false);
                discr.Where(StockTake.Columns.InventoryLocationID, LocationID);
                discr.Where(StockTake.Columns.Marked, true);
                discr.OrderByAsc(StockTake.Columns.ItemNo);
                discr.Load();

                decimal discrQty;

                //pull out
                //store deleted to be adjusted later
                int p = discr.Count - 1;
                while (p > 0)
                {
                    if (discr[p].ItemNo == discr[p - 1].ItemNo)
                    {
                        discr[p - 1].StockTakeQty += discr[p].StockTakeQty;
                        discr[p].IsAdjusted = true;
                        cmd.Add(discr[p].GetUpdateCommand(UserInfo.username));
                        discr.RemoveAt(p);
                    }
                    p--;
                }

                //string InventoryRefNo = InventoryController.getNewInventoryRefNo(LocationID);
                List<string> ListInventoryHdr = new List<string>();

                for (int i = 0; i < discr.Count; i++)
                {

                    //Fetch balance quantity for this item.
                    /*
                    SystemBalQty = InventoryController.GetStockBalanceQtyByItemByDate
                        (discr[i].ItemNo, LocationID, discr[i].StockTakeDate, out status);                    
                     discrQty = discr[i].StockTakeQty - SystemBalQty;
                    */
                    discrQty = discr[i].AdjustmentQty.GetValueOrDefault(0);

                    //for every row, perform stock in or stock out
                    if (discrQty < 0)
                    {
                        discrQty = -discrQty;

                        InventoryController StockOutCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        StockOutCtrl.SetRemark("Stock Take Adj. Id:" + discr[i].StockTakeID);
                        StockOutCtrl.SetInventoryDate(discr[i].StockTakeDate);
                        //INYYMMDDSSSSNNNN                
                        StockOutCtrl.SetInventoryHdrRefNo("ST" + discr[i].StockTakeID.ToString());
                        StockOutCtrl.SetInventoryStockDocumentNo(StockDocumentNo);
                        ListInventoryHdr.Add("ST" + discr[i].StockTakeID.ToString());
                        //Logger.writeLog(StockOutCtrl.GetInvHdrRefNo());

                        //do stock out                                               
                        decimal cogs = ItemSummaryController.GetAvgCostPrice(discr[i].ItemNo, LocationID);
                        /*if (cogs == 0)
                        {
                            Item it = new Item(discr[i].ItemNo);
                            cogs = it.FactoryPrice;
                        }*/
                        StockOutCtrl.AddItemIntoInventoryStockTake(discr[i].ItemNo, discrQty, cogs, out status);
                        StockOutCtrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Stock Take Adj. Id:" + discr[i].StockTakeID, out status);
                        QueryCommandCollection cmdTmp;
                        if (StockOutCtrl.CreateStockOutQueryCommand
                            (username, STOCKTAKE_ADJUSTMENT_REASONID, LocationID, true, true, out status, out cmdTmp))
                        {
                            cmd.AddRange(cmdTmp);

                            Query qr = OrderDet.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                            qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, discr[i].StockTakeDate);
                            qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                            qr.AddWhere(OrderDet.Columns.ItemNo, discr[i].ItemNo);
                            cmd.Add(qr.BuildUpdateCommand());

                        }
                        else
                        {
                            Logger.writeLog(status);
                            return false;
                        }

                    }
                    else if (discrQty > 0)
                    {
                        InventoryController StockInCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        StockInCtrl.SetInventoryDate(discr[i].StockTakeDate);
                        StockInCtrl.SetRemark("Stock Take Adj. Id:" + discr[i].StockTakeID);
                        StockInCtrl.SetInventoryHdrRefNo("ST" + discr[i].StockTakeID.ToString());
                        StockInCtrl.SetInventoryStockDocumentNo(StockDocumentNo);
                        ListInventoryHdr.Add("ST" + discr[i].StockTakeID.ToString());
                        decimal cogs = ItemSummaryController.GetAvgCostPrice(discr[i].ItemNo, LocationID);
                        if (cogs == 0)
                        {
                            Item it = new Item(discr[i].ItemNo);
                            cogs = it.FactoryPrice;
                        }
                        /*ViewInventoryActivityCollection vInventory = new ViewInventoryActivityCollection();
                        vInventory.Where(ViewInventoryActivity.Columns.ItemNo, discr[i].ItemNo);
                        vInventory.Where(ViewInventoryActivity.Columns.InventoryLocationID, LocationID);
                        vInventory.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "Stock Out");
                        vInventory.Where(ViewInventoryActivity.Columns.InventoryDate, Comparison.LessThan, discr[i].StockTakeDate);
                        vInventory.OrderByDesc(ViewInventoryActivity.Columns.InventoryDate);
                        vInventory.Load();

                        if (vInventory.Count > 0)
                        {
                            cogs = vInventory[0].FactoryPrice;
                        }*/


                        StockInCtrl.AddItemIntoInventoryStockTake(discr[i].ItemNo, discrQty, cogs, out status);
                        StockInCtrl.ChangeRemarkByItemNo(discr[i].ItemNo, "Stock Take Adj. Id:" + discr[i].StockTakeID, out status);
                        StockInCtrl.SetInventoryHdrUserName(discr[i].CreatedBy);
                        QueryCommandCollection cmdTmp;
                        try
                        {
                            cmdTmp = StockInCtrl.CreateStockInQueryCommand(username, LocationID, true, false);
                        }
                        catch (Exception X)
                        {
                            status = X.Message.Replace("(error)", "").Replace("(warning)", "");
                            Logger.writeLog(status);
                            return false;
                        }

                        cmd.AddRange(cmdTmp);
                        Query qr = OrderDet.CreateQuery();
                        qr.QueryType = QueryType.Update;
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                        qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, discr[i].StockTakeDate);
                        qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                        qr.AddWhere(OrderDet.Columns.ItemNo, discr[i].ItemNo);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                    AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Adjust stock take : {0}", discr[i].StockTakeID), "");
                    //update StockTake and set isadjusted to be true
                    Query qr1 = StockTake.CreateQuery();
                    qr1.AddUpdateSetting(StockTake.Columns.IsAdjusted, true);
                    qr1.AddUpdateSetting(StockTake.Columns.ModifiedOn, DateTime.Now);
                    qr1.AddWhere(StockTake.Columns.StockTakeID, discr[i].StockTakeID);
                    qr1.QueryType = QueryType.Update;
                    cmd.Add(qr1.BuildUpdateCommand());
                    //InventoryRefNo = InventoryController.getNextInventoryRefNo(InventoryRefNo);

                    // Execute each line of stock take (in trans scope)
                    DataService.ExecuteTransaction(cmd);
                    cmd = new QueryCommandCollection();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Adjusting Stock Discrepancy : " + ex.Message);
                return false;
            }
        }

        public bool CorrectStockTakeDiscrepancySingleItem(string username, int LocationID, string ItemNo, decimal correctQty)
        {
            try
            {
                string status;
                QueryCommandCollection cmd = new QueryCommandCollection();
               
                List<string> ListInventoryHdr = new List<string>();
                if (correctQty < 0)
                {
                    correctQty = -correctQty;

                    InventoryController StockOutCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                    StockOutCtrl.SetInventoryDate(DateTime.Now);
                    StockOutCtrl.SetInventoryHdrRefNo(InventoryController.getNewInventoryRefNo(LocationID));
                    
                    //do stock out                                               
                    decimal cogs = ItemSummaryController.GetAvgCostPrice(ItemNo, LocationID);
                    StockOutCtrl.AddItemIntoInventoryStockTake(ItemNo, correctQty, cogs, out status);
                    
                    QueryCommandCollection cmdTmp;
                    if (StockOutCtrl.CreateStockOutQueryCommand
                        (username, 0, LocationID, true, true, out status, out cmdTmp))
                    {
                        cmd.AddRange(cmdTmp);

                        Query qr = OrderDet.CreateQuery();
                        qr.QueryType = QueryType.Update;
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                        qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, DateTime.Now);
                        qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                        qr.AddWhere(OrderDet.Columns.ItemNo, ItemNo);
                        cmd.Add(qr.BuildUpdateCommand());

                    }
                    else
                    {
                        Logger.writeLog(status);
                        return false;
                    }

                }
                else if (correctQty > 0)
                {
                    InventoryController StockInCtrl = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                    StockInCtrl.SetInventoryDate(DateTime.Now);
                    StockInCtrl.SetInventoryHdrRefNo(InventoryController.getNewInventoryRefNo(LocationID));
                    decimal cogs = ItemSummaryController.GetAvgCostPrice(ItemNo, LocationID);
                    if (cogs == 0)
                    {
                        Item it = new Item(ItemNo);
                        cogs = it.FactoryPrice;
                    }


                    StockInCtrl.AddItemIntoInventoryStockTake(ItemNo, correctQty, cogs, out status);
                    StockInCtrl.SetInventoryHdrUserName(username);
                    QueryCommandCollection cmdTmp;
                    try
                    {
                        cmdTmp = StockInCtrl.CreateStockInQueryCommand(username, LocationID, true, false);
                    }
                    catch (Exception X)
                    {
                        status = X.Message.Replace("(error)", "").Replace("(warning)", "");
                        Logger.writeLog(status);
                        return false;
                    }

                    cmd.AddRange(cmdTmp);
                    Query qr = OrderDet.CreateQuery();
                    qr.QueryType = QueryType.Update;
                    qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                    qr.AddWhere(OrderDet.Columns.OrderDetDate, Comparison.LessThan, DateTime.Now);
                    qr.AddWhere(OrderDet.Columns.InventoryHdrRefNo, null);
                    qr.AddWhere(OrderDet.Columns.ItemNo, ItemNo);
                    cmd.Add(qr.BuildUpdateCommand());

                }
                //AccessLogController.AddLog(AccessSource.POS, UserInfo.username, "-", string.Format("Adjust stock take : {0}", discr[i].StockTakeID), "");
                DataService.ExecuteTransaction(cmd);
                cmd = new QueryCommandCollection();
                

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Error Adjusting Stock Discrepancy : " + ex.Message);
                return false;
            }
        }

        public static bool UpdateStockTakeEntryDate(DateTime stockTakeDate, int LocationID)
        {
            //
            DateTime stockTakeLast = FetchLastStockTakeDateByInventoryLocation(LocationID);
            if (stockTakeDate > stockTakeLast)
            {
                Query qr = StockTake.CreateQuery();
                qr.QueryType = QueryType.Update;
                qr.AddUpdateSetting(StockTake.Columns.StockTakeDate, stockTakeDate);
                qr.AddWhere(StockTake.Columns.InventoryLocationID, LocationID);
                qr.AddWhere(StockTake.Columns.IsAdjusted, false);
                qr.Execute();

                return true;
            }
            else
            {
                return false;
            }
        }
        public static DateTime FetchLastStockTakeDateByInventoryLocation(int LocationID) //, string itemno)
        {
            try
            {

                Query qr = StockTake.CreateQuery();

                Where whr = new Where();

                whr.ColumnName = StockTake.Columns.InventoryLocationID;
                whr.ParameterName = "@InventoryLocationID";
                whr.ParameterValue = LocationID;
                whr.TableName = "StockTake";

                qr.AddWhere(whr);
                qr.Top = "1";
                qr.SelectList = "StockTakeDate";
                qr.OrderBy = OrderBy.Desc(StockTake.Columns.StockTakeDate);

                DateTime dt = DateTime.MinValue;

                Object obj = qr.WHERE(StockTake.Columns.IsAdjusted, 1).ExecuteScalar();
                if (obj != null)
                    DateTime.TryParse(obj.ToString(), out dt);
                else
                    return DateTime.MinValue;

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return new DateTime(2007, 1, 1);
            }
        }
        public static DateTime FetchLastStockTakeDateByInventoryLocationByItemNo(int LocationID, string itemno)
        {
            try
            {

                Query qr = StockTake.CreateQuery();

                Where whr = new Where();

                whr.ColumnName = StockTake.Columns.InventoryLocationID;
                whr.ParameterName = "@InventoryLocationID";
                whr.ParameterValue = LocationID;
                whr.TableName = "StockTake";

                qr.AddWhere(whr);

                Where whr2 = new Where();
                whr2.ColumnName = StockTake.Columns.ItemNo;
                whr2.ParameterName = "@ItemNo";
                whr2.ParameterValue = itemno;
                whr2.TableName = "StockTake";

                qr.AddWhere(whr2);

                qr.Top = "1";
                qr.SelectList = "StockTakeDate";
                qr.OrderBy = OrderBy.Desc(StockTake.Columns.StockTakeDate);

                DateTime dt = DateTime.MinValue;

                Object obj = qr.WHERE(StockTake.Columns.IsAdjusted, 1).ExecuteScalar();
                if (obj != null)
                    DateTime.TryParse(obj.ToString(), out dt);
                else
                    return DateTime.MinValue;

                return dt;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return new DateTime(2007, 1, 1);
            }
        }

        public string GetStockTakeIDForNonAdjustedItem(string itemno)
        {
            Query qr = new Query("StockTake");
            qr.SelectList = "StockTakeID";
            qr.AddWhere("IsAdjusted", false);
            qr.AddWhere("ItemNo", itemno);

            if (qr.ExecuteScalar() != null)
            {
                return qr.ExecuteScalar().ToString();
            }
            else
            {
                return "-1";
            }
        }

        //fetch stock take....
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public static void UpdateStockTakeRemark(int StockTakeID, string Remark)
        {
            if (Remark == null) Remark = "";

            Query stockTakeQuery = StockTake.CreateQuery();
            stockTakeQuery.QueryType = QueryType.Update;
            stockTakeQuery.AddUpdateSetting(StockTake.Columns.Remark, Remark);
            stockTakeQuery.AddWhere("StockTakeID", StockTakeID);
            stockTakeQuery.Execute();
        }
        public static bool IsThereUnAdjustedStockTake()
        {
            Query stockTakeQuery = StockTake.CreateQuery();
            object o = stockTakeQuery.WHERE(StockTake.Columns.IsAdjusted, false).GetCount(StockTake.Columns.StockTakeID);
            if (o != null && o is int && (int)o > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetUnAdjustedStockTakeDate(int inventoryLocationID)
        {
            StockTakeCollection stCol = new StockTakeCollection();
            //stCol.Where(StockTake.Columns.IsAdjusted, false);
            //stCol.Where(StockTake.Columns.InventoryLocationID, inventoryLocationID);
            //stCol.Load();

            string sql = "SELECT * FROM StockTake WHERE InventoryLocationID = {0} AND IsAdjusted = 0 AND ISNULL(userflag1,0) = 0";
            sql = string.Format(sql, inventoryLocationID);
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
            if (ds != null && ds.Tables.Count > 0)
            {
                stCol.Load(ds.Tables[0]);
            }

            //Query stockTakeQuery = StockTake.CreateQuery();
            //object o = stockTakeQuery.WHERE(StockTake.Columns.IsAdjusted, false).GetCount(StockTake.Columns.StockTakeID);
            //count = stCol.Count;
            if (stCol.Count > 0)
            {
                return stCol[0].StockTakeDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return "";
            }
        }

        public static bool IsThereUnAdjustedStockTake(int inventoryLocationID)
        {
            //StockTakeCollection stCol = new StockTakeCollection();
            //stCol.Where(StockTake.Columns.IsAdjusted, false);
            //stCol.Where(StockTake.Columns.InventoryLocationID, inventoryLocationID);
            //stCol.Load();

            //Query stockTakeQuery = StockTake.CreateQuery();
            //object o = stockTakeQuery.WHERE(StockTake.Columns.IsAdjusted, false).GetCount(StockTake.Columns.StockTakeID);

            StockTakeCollection stCol = new StockTakeCollection();
            string sql = "SELECT * FROM StockTake WHERE IsAdjusted = 0 AND " + StockTake.UserColumns.Deleted + " = 0 AND InventoryLocationID = {0}";
            sql = string.Format(sql, inventoryLocationID);
            DataSet ds = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS"));
            if (ds != null && ds.Tables.Count > 0)
            {
                stCol.Load(ds.Tables[0]);
            }

            if (stCol.Count > 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Fetch stock take qty of an item
        public static int FetchStockTakeLatestQty(DateTime CurrentDate, string itemno, int inventoryLocationID, out DateTime StockTakeDate)
        {
            StockTakeDate = new DateTime(2007, 1, 1);

            DataSet ds = SPs.FetchStockTakeQtyByItem(CurrentDate, itemno, inventoryLocationID).GetDataSet();
            int Quantity = 0;
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                Quantity = int.Parse(dt.Rows[0][0].ToString());
                DateTime.TryParse(dt.Rows[0][1].ToString(), out StockTakeDate);
            }
            return Quantity;
        }
    }
}
