using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Transactions;
using System.Collections;
using PowerPOS.Container;
using SubSonic;
using System.ComponentModel;
using System.Linq;
namespace PowerPOS
{

    public partial class InventoryController
    {
        public static decimal GetReceivedQtyForPurchaseOrderByItemNo(string PurchaseOrderHeaderRefNo, string ItemNo)
        {
            string SQL = "SELECT SUM(invdet.Quantity) " +
                         "FROM InventoryHdr invhdr " +
                         "     INNER JOIN InventoryDet invdet ON invhdr.InventoryHdrRefNo = invdet.InventoryHdrRefNo " +
                         "WHERE invhdr.PurchaseOrderNo = @PurchaseOrderHeaderRefNo AND " +
                         "      invhdr.MovementType IN ('Stock In', 'Transfer In') AND " +
                         "      invdet.ItemNo = @ItemNo " +
                         "GROUP BY invdet.ItemNo ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            cmd.AddParameter("@PurchaseOrderHeaderRefNo", PurchaseOrderHeaderRefNo, DbType.String);
            cmd.AddParameter("@ItemNo", ItemNo, DbType.String);
            object obj = DataService.ExecuteScalar(cmd);
            if (obj is decimal) return (decimal)obj;
            return 0;
        }

        public static string getNewInventoryRefNo(int InventoryLocationID)
        {
            return getNewInventoryRefNo(InventoryLocationID, 0);
        }

        public static string getNewInventoryRefNo(int InventoryLocationID, int addition)
        {
            int runningNo = 0;
            string InventoryRefNo;
            /*
            object ob = SPs.GetNewInventoryRefNoByInventoryLocationID(InventoryLocationID).ExecuteScalar();
            if (ob != null)
            {
                int.TryParse(ob.ToString(), out runningNo);
            }
            */
            string header = "IN" + DateTime.Now.ToString("yyMMdd") + InventoryLocationID.ToString().PadLeft(4, '0');
            Query qr = InventoryHdr.CreateQuery();
            qr.AddWhere(InventoryHdr.Columns.InventoryHdrRefNo, Comparison.Like, header + "%");
            qr.QueryType = QueryType.Select;
            qr.Top = "1";
            qr.SetSelectList("InventoryHdrRefNo");
            qr.OrderBy = OrderBy.Desc("InventoryHdrRefNo");

            DataSet ds = qr.ExecuteDataSet();
            if (ds.Tables[0].Rows.Count > 0)
            {
                runningNo = int.Parse(ds.Tables[0].Rows[0][0].ToString().Substring(header.Length));
            }
            else
            {
                runningNo = 0;
            }
            runningNo += (1 + addition);

            //INYYMMDDSSSSNNNN                
            //YY - year
            //MM - month
            //DD - day
            //SSSS - PointOfSale ID
            //NNNN - Running No
            InventoryRefNo = header + runningNo.ToString().PadLeft(4, '0');

            return InventoryRefNo;
        }

        public static string CreateNewCustomRefNo()
        {
            string prefix = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomPrefix);
            string suffix = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomSuffix);

            int numberLength;
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.NumberLength), out numberLength))
            {
                numberLength = 6;
            }

            string dateFormat = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CustomNoDateFormat);
            if (string.IsNullOrEmpty(dateFormat)) dateFormat = "yyMMdd";
            prefix = prefix.Replace("[DATE]", DateTime.Now.ToString(dateFormat));
            suffix = suffix.Replace("[DATE]", DateTime.Now.ToString(dateFormat));

            #region *) Check whether need to reset the runningNo or not
            string resetEvery = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.ResetNumberEvery);
            if (!string.IsNullOrEmpty(resetEvery) && resetEvery != Constants.ResetEvery.Never)
            {
                bool needReset = false;
                string lastReset = AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.LastReset);
                if (!string.IsNullOrEmpty(lastReset))
                {
                    if (resetEvery == Constants.ResetEvery.Year && lastReset.Substring(0, 2) != DateTime.Now.ToString("yy"))
                        needReset = true;
                    else if (resetEvery == Constants.ResetEvery.Month && lastReset.Substring(0, 4) != DateTime.Now.ToString("yyMM"))
                        needReset = true;
                    else if (resetEvery == Constants.ResetEvery.Day && lastReset != DateTime.Now.ToString("yyMMdd"))
                        needReset = true;
                    else if (resetEvery == Constants.ResetEvery.MaxReached)
                    {
                        int tmpNo;
                        if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo), out tmpNo))
                        {
                            tmpNo = 0;
                        }
                        tmpNo += 1;
                        if (tmpNo.ToString().Length > numberLength)
                            needReset = true;
                    }
                }
                else
                {
                    AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.LastReset, DateTime.Now.ToString("yyMMdd"));
                }

                if (needReset)
                {
                    AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo, "0");
                    AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.LastReset, DateTime.Now.ToString("yyMMdd"));
                }
            }
            #endregion

            int runningNo;
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo), out runningNo))
            {
                runningNo = 0;
            }
            runningNo += 1;

            string customRefNo = prefix + runningNo.ToString().PadLeft(numberLength, '0') + suffix;

            return customRefNo;
        }

        public static void CustomRefNoUpdate()
        {
            int runningNo;
            if (!int.TryParse(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo), out runningNo))
            {
                runningNo = 0;
            }
            runningNo += 1;
            AppSetting.SetSetting(AppSetting.SettingsName.GoodsReceive.CurrentNo, runningNo.ToString());
        }

        public static string getNextInventoryRefNo(string currentInventoryHdrRefNo)
        {
            int runningNo = 0;
            if (currentInventoryHdrRefNo.Length == 16 &&
                int.TryParse(currentInventoryHdrRefNo.Substring(12, 4), out runningNo))
            {
                //INYYMMDDSSSSNNNN                
                //YY - year
                //MM - month
                //DD - day
                //SSSS - PointOfSale ID
                //NNNN - Running No
                string InventoryRefNo = currentInventoryHdrRefNo.Substring(0, 12) + (runningNo + 1).ToString().PadLeft(4, '0');

                return InventoryRefNo;
            }
            else
            {
                return "-";
            }
        }

        public static DataTable ViewOrdersWithUnassignedInventory()
        {
            //Fetch all that has no inventoryrefno
            OrderHdrCollection myHdr = new OrderHdrCollection();
            myHdr.Where(OrderHdr.Columns.InventoryHdrRefNo, null);
            myHdr.OrderByDesc(OrderHdr.Columns.OrderDate);
            myHdr.Load();
            return myHdr.ToDataTable();
        }

        public static void RedoSalesStockOut(int InventoryLocationID)
        {

            DataSet ds = SPs.FetchStockOutWithoutStockInByInventoryLocationID(InventoryLocationID).GetDataSet();
            OrderHdr tmphdr;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmphdr = new OrderHdr(OrderHdr.Columns.InventoryHdrRefNo, ds.Tables[0].Rows[i]["InventoryHdrRefNo"].ToString());
                if (tmphdr.IsLoaded)
                {
                    tmphdr.InventoryHdrRefNo = null;
                    tmphdr.Save("SYSTEM");
                }
                InventoryHdr.Delete((ds.Tables[0].Rows[i]["InventoryHdrRefNo"].ToString()));
            }

            //Fetch point of sales ID by Inventory ID
            InventoryLocation loc = new InventoryLocation(InventoryLocationID);
            OutletCollection outletCol = loc.OutletRecords();
            PointOfSaleCollection posCol = new PointOfSaleCollection();
            for (int i = 0; i < outletCol.Count; i++)
            {
                posCol.AddRange(outletCol[i].PointOfSaleRecords());
            }
            for (int i = 0; i < posCol.Count; i++)
            {
                AssignStockOutToConfirmedOrder(posCol[i].PointOfSaleID);
            }
        }

        public static bool AssignStockOutToConfirmedOrder(int PointOfSaleID)
        {
            string startRefNo = "-1";
            string EndRefNo = "-1";
            try
            {
                Logger.writeLog("Inventory Update Starts!");
                using (TransactionScope ts = new TransactionScope())
                {
                    //Fetch all that has no inventoryrefno
                    OrderHdrCollection myHdr = new OrderHdrCollection();
                    myHdr.Where(OrderHdr.Columns.InventoryHdrRefNo, null);
                    myHdr.Where(OrderHdr.Columns.PointOfSaleID, PointOfSaleID);
                    myHdr.OrderByAsc(OrderHdr.Columns.OrderDate);
                    myHdr.Load();
                    string status;

                    OrderDetCollection myOrderDet;
                    InventoryController itCtr;
                    int max = myHdr.Count;
                    /*
                    if (max > 200)
                        max = 200; //if too long, we got a problem....
                    */
                    for (int k = 0; k < max; k++)
                    {
                        //myOrderDet = myHdr[k].OrderDetRecords();
                        myOrderDet = new OrderDetCollection();
                        myOrderDet.Where(OrderDet.Columns.OrderHdrID, myHdr[k].OrderHdrID);
                        myOrderDet.Load();

                        //Create the inventory Controller to perform inventory transactions
                        itCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        itCtr.SetInventoryDate(myHdr[k].OrderDate);

                        for (int i = 0; i < myOrderDet.Count; i++)
                        {
                            if (!(bool)myOrderDet[i].IsVoided & myOrderDet[i].Item.IsInInventory)
                            {
                                //Check balance quantity before stock out...
                                decimal bal = InventoryController.GetStockBalanceQtyByItem(myOrderDet[i].ItemNo
                                    , myHdr[k].PointOfSale.Outlet.InventoryLocationID.Value, out status);
                                if (bal >= myOrderDet[i].Quantity.GetValueOrDefault(0))
                                    itCtr.AddItemIntoInventory(myOrderDet[i].ItemNo, myOrderDet[i].Quantity.GetValueOrDefault(0), out status);
                            }
                        }

                        //Do stock out for the corresponding 
                        if (!itCtr.StockOut("SYSTEM", 0, myHdr[k].PointOfSale.Outlet.InventoryLocationID.Value, false, true, out status))
                        {
                            status = "Error while performing inventory transaction. " + status;
                            Logger.writeLog(status);
                            throw new Exception(status);
                        }

                        //Assign the inventory reference number to track orders                                
                        myHdr[k].InventoryHdrRefNo = itCtr.GetInvHdrRefNo();
                        if (k == 0)
                            startRefNo = itCtr.GetInvHdrRefNo();
                        if (k == myHdr.Count - 1)
                            EndRefNo = itCtr.GetInvHdrRefNo();

                        myHdr[k].Save("SYSTEM");
                    }

                    InventoryUpdateLogController ctr = new InventoryUpdateLogController();
                    ctr.Insert(DateTime.Now, startRefNo, EndRefNo, DateTime.Now, "SYSTEM", DateTime.Now, "SYSTEM"
                        , "", "", "", "", "", "", "", "", "", "", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                    ts.Complete();
                    Logger.writeLog("Inventory Update success!");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Inventory Update failed!");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static bool SetBalanceQuantity(string ItemNo, decimal CurrentBalQty, int LocationID)
        {
            try
            {
                //Get all item with stock in/transfer in/adjustment in form an array
                ViewInventoryActivityCollection inv = new ViewInventoryActivityCollection();
                inv.Where(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "% In");
                inv.Where(ViewInventoryActivity.Columns.InventoryLocationID, LocationID);
                inv.Where(ViewInventoryActivity.Columns.ItemNo, ItemNo);
                inv.OrderByDesc("InventoryDate");
                inv.Load();

                /*
                 int   j = 0;
                while(j < inv.Count)
                {
                    //delete
                    if (inv[j].MovementType == "Transfer In")
                    {
                        inv.RemoveAt(j);
                    }
                    else
                    {
                        j++;
                    }
                }
                */
                using (TransactionScope ts = new TransactionScope())
                {

                    for (int i = 0; i < inv.Count; i++)
                    {
                        if (inv[i].RemainingQty > 0)
                        {
                            Query qr = InventoryDet.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting(InventoryDet.Columns.RemainingQty, 0);
                            qr.AddWhere(InventoryDet.Columns.InventoryDetRefNo, inv[i].InventoryDetRefNo);
                            qr.Execute();
                        }
                    }

                    decimal remainingqty = CurrentBalQty;
                    for (int i = 0; i < inv.Count; i++)
                    {
                        if (inv[i].Quantity >= remainingqty)
                        {
                            Query qr = InventoryDet.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting(InventoryDet.Columns.RemainingQty, remainingqty);
                            qr.AddWhere(InventoryDet.Columns.InventoryDetRefNo, inv[i].InventoryDetRefNo);
                            qr.Execute();
                            remainingqty = 0;
                            break;
                        }
                        else
                        {
                            Query qr = InventoryDet.CreateQuery();
                            qr.QueryType = QueryType.Update;
                            qr.AddUpdateSetting(InventoryDet.Columns.RemainingQty, inv[i].Quantity);
                            qr.AddWhere(InventoryDet.Columns.InventoryDetRefNo, inv[i].InventoryDetRefNo);
                            qr.Execute();
                            remainingqty -= inv[i].Quantity.GetValueOrDefault(0);

                        }
                    }
                    if (remainingqty > 0)
                        throw new Exception(ItemNo + "--Insufficient stock in performed. " + remainingqty);



                    ts.Complete();
                    //Logger.writeLog("Success : " + ItemNo);
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
            return true;
        }

        public static bool AssignStockOutToConfirmedOrder()
        {
            string startRefNo = "-1";
            string EndRefNo = "-1";
            try
            {
                Logger.writeLog("Inventory Update Starts!");
                //using (TransactionScope ts = new TransactionScope())
                {
                    //Fetch all that has no inventoryrefno
                    OrderHdrCollection myHdr = new OrderHdrCollection();
                    myHdr.Where(OrderHdr.Columns.InventoryHdrRefNo, null);
                    myHdr.OrderByAsc(OrderHdr.Columns.OrderDate);
                    myHdr.Load();
                    string status;
                    bool IsSuccess;

                    OrderDetCollection myOrderDet;
                    InventoryController itCtr;

                    //Dont process more than 200 in one go, can have timeout exception on SQL transaction
                    int max = myHdr.Count;
                    if (max > 200)
                        max = 200;

                    for (int k = 0; k < max; k++)
                    {
                        if (myHdr[k].IsLoaded)
                        {
                            Logger.writeLog("Order Processed:" + myHdr[k].OrderHdrID);

                            //Get all the inventory details for this order..
                            myOrderDet = myHdr[k].OrderDetRecords();

                            IsSuccess = true;
                            //Create the inventory Controller to perform inventory transactions
                            itCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                            itCtr.SetInventoryDate(myHdr[k].OrderDate);

                            for (int i = 0; i < myOrderDet.Count; i++)
                            {
                                if (!(bool)myOrderDet[i].IsVoided & myOrderDet[i].Item.IsInInventory)
                                {
                                    decimal bal = InventoryController.GetStockBalanceQtyByItem(myOrderDet[i].ItemNo
                                           , myHdr[k].PointOfSale.Outlet.InventoryLocationID.Value, out status);
                                    if (bal >= myOrderDet[i].Quantity.GetValueOrDefault(0))
                                    {
                                        itCtr.AddItemIntoInventory(myOrderDet[i].ItemNo, myOrderDet[i].Quantity.GetValueOrDefault(0), out status);
                                        Logger.writeLog("SUCCESS for:" + myOrderDet[i].OrderDetID + " Item No" + myOrderDet[i].ItemNo);
                                    }
                                    else
                                    {
                                        Logger.writeLog("ERROR:Insufficient balance for:" + myOrderDet[i].OrderDetID + " Item No" + myOrderDet[i].ItemNo);
                                        IsSuccess = false;
                                        break;
                                    }
                                }
                            }

                            //Do stock out for the corresponding 
                            if (IsSuccess)
                            {
                                if (!itCtr.StockOut("SYSTEM", 0, myHdr[k].PointOfSale.Outlet.InventoryLocationID.Value, false, true, out status))
                                {
                                    status = "Error while performing inventory transaction. " + status;
                                    Logger.writeLog(status);
                                    throw new Exception(status);
                                }

                                //Assign the inventory reference number to track orders                                
                                myHdr[k].InventoryHdrRefNo = itCtr.GetInvHdrRefNo();
                                while (myHdr[k].IsDirty)
                                {
                                    myHdr[k].Save();
                                }

                                if (k == 0)
                                    startRefNo = itCtr.GetInvHdrRefNo();
                                if (k == myHdr.Count - 1)
                                    EndRefNo = itCtr.GetInvHdrRefNo();

                                Logger.writeLog(myHdr[k].OrderHdrID + " assigned to " + itCtr.GetInvHdrRefNo());

                            }
                            else
                            {
                                Logger.writeLog("Roll inventory unsuccessful for order - " + myHdr[k].OrderHdrID);
                            }
                        }
                    }
                    /*
                        InventoryUpdateLogController ctr = new InventoryUpdateLogController();
                        ctr.Create(DateTime.Now, startRefNo, EndRefNo, DateTime.Now, "SYSTEM", DateTime.Now, "SYSTEM"
                            , "", "", "", "", "", "", "", "", "", "", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                     */
                    //ts.Complete();
                    Logger.writeLog("Inventory Update success!");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Inventory Update failed!");
                Logger.writeLog(ex);
                return false;
            }
        }

        public static decimal GetCostOfGoods(string InventoryHdrRefNo)
        {
            decimal cogs = 0;
            InventoryDetCollection tmpInvDet = new InventoryDetCollection();
            tmpInvDet.Where(InventoryDet.Columns.InventoryHdrRefNo, InventoryHdrRefNo).Load();
            for (int i = 0; i < tmpInvDet.Count; i++)
            {
                cogs += tmpInvDet[i].CostOfGoods;
            }
            tmpInvDet.Clear();
            return cogs;
        }

        public static decimal GetStockBalanceQtyByItemByDate(string itemno, int locationID, DateTime RequiredDate, out string status)
        {
            //TODO: Create opening balance table for each item, when data is being backup and deleted, auto create an entry in opening balance
            decimal openingBalance = 0;
            DateTime openingDate = new DateTime(1979,11,3);
            decimal currentBalance;

            try
            {
                #region *) Fetch: Get Current Balance
                string SQLString =
                    "DECLARE @StartDate DATETIME; "+
                    "DECLARE @EndDate DATETIME; "+
                    "SET @StartDate = '" + openingDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'; " +
                    "SET @EndDate = '" + RequiredDate.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'; " +
                    "SELECT ISNULL(SUM(CASE WHEN MovementType LIKE '% In' THEN Quantity ELSE 0 - Quantity END), 0) AS Balance " +
                    "FROM InventoryHdr IH " +
                        "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                    "WHERE ItemNo = @ItemNo " +
                        "AND InventoryDate BETWEEN @StartDate AND @EndDate ";

                if(locationID != 0)
                    SQLString += "AND  InventoryLocationID  = " + locationID;

                QueryCommand Cmd = new QueryCommand(SQLString);
                //Cmd.AddParameter("@InventoryLocationID", locationID);
                Cmd.AddParameter("@ItemNo", itemno);
                //Cmd.AddParameter("@StartDate", openingDate);
                //Cmd.AddParameter("@EndDate", RequiredDate);

                currentBalance = (decimal)DataService.ExecuteScalar(Cmd);
                #endregion

                status = "";
                /*
                DataTable undeductedSales = ReportController.FetchUnDeductedSales(locationID);

                int negativeSales = 0;
                DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");

                if (dr != null && dr.Length > 0)
                {
                    negativeSales = (int)dr[0]["Quantity"];
                }
                else
                {
                    negativeSales = 0;
                }
                */

                return (openingBalance + currentBalance);
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                status = X.Message;
                return 0;
            }
        }

        public static decimal GetStockBalanceQtyByItemSummaryByDate(string itemno, int locationID, DateTime RequiredDate, out string status)
        {
            //TODO: Create opening balance table for each item, when data is being backup and deleted, auto create an entry in opening balance
            decimal openingBalance = 0;
            DateTime openingDate = new DateTime(1979, 11, 3);
            decimal currentBalance;

            try
            {
                #region *) Fetch: Get Current Balance
                string SQLString =
                    "DECLARE @StartDate DATETIME; " +
                    "DECLARE @EndDate DATETIME; " +
                    "SET @StartDate = '" + openingDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                    "SET @EndDate = '" + RequiredDate.ToString("yyyy-MM-dd HH:mm:ss") + "'; " +
                    "SELECT ISNULL(SUM(BalanceQty),0) AS Balance " +
                    "FROM ItemSummary " +
                    "WHERE ItemNo = '" + itemno + "'";
                        //"AND InventoryDate BETWEEN @StartDate AND @EndDate ";

                if (locationID != 0)
                    SQLString += "AND InventoryLocationID = " + locationID;

                QueryCommand Cmd = new QueryCommand(SQLString);
                //Cmd.AddParameter("@InventoryLocationID", locationID);
                //Cmd.AddParameter("@ItemNo", itemno);
                //Cmd.AddParameter("@StartDate", openingDate);
                //Cmd.AddParameter("@EndDate", RequiredDate);

                object tmp = DataService.ExecuteScalar(Cmd);
                decimal.TryParse(tmp.ToString(), out currentBalance);
                //currentBalance = (int)DataService.ExecuteScalar(Cmd);
                #endregion

                status = "";
                /*
                DataTable undeductedSales = ReportController.FetchUnDeductedSales(locationID);

                int negativeSales = 0;
                DataRow[] dr = undeductedSales.Select("itemno = '" + stockBal.Rows[j]["ItemNo"] + "'");

                if (dr != null && dr.Length > 0)
                {
                    negativeSales = (int)dr[0]["Quantity"];
                }
                else
                {
                    negativeSales = 0;
                }
                */

                return (openingBalance + currentBalance);
            }
            catch (Exception X)
            {
                Logger.writeLog(X);
                status = X.Message;
                return 0;
            }
        }
        /*
        public static DataTable FetchAverageCost()
        {
            string SQL = "select itemno,sum(Quantity), " +
"avg(costofgoods) from inventoryhdr a " 
"inner join inventorydet b " + 
"on a.inventoryhdrrefno = b.inventoryhdrrefno " +
"where " +
"a.movementtype like '%In' " +
"group by itemno"

        }*/
        private static DataTable FetchFIFOCostOfGoods
            (int quantityAt, string ItemNo, int InventoryLocationID, out string status)
        {
            status = "";
            if (quantityAt <= 0)
                return null;

            Query qr = ViewInventoryActivity.CreateQuery();
            qr.Top = quantityAt.ToString();
            qr.QueryType = QueryType.Select;
            qr.AddWhere(ViewInventoryActivity.Columns.ItemNo, ItemNo);
            qr.AddWhere(ViewInventoryActivity.Columns.MovementType, Comparison.Like, "% In");
            if (InventoryLocationID != 0) qr.AddWhere(ViewInventoryActivity.Columns.InventoryLocationID, InventoryLocationID);
            qr.OrderBy = OrderBy.Desc(ViewInventoryActivity.Columns.InventoryDate);

            DataSet ds = qr.ExecuteDataSet();

            int tmpqty = 0;
            //filter and remove the datarows
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmpqty += (int)ds.Tables[0].Rows[i]["Quantity"];
                if (tmpqty > quantityAt)
                {
                    //remove the rest of the rows.

                    for (int k = ds.Tables[0].Rows.Count - 1; k > i; k--)
                    {
                        ds.Tables[0].Rows.RemoveAt(k);
                    }
                    //exit for loop
                    break;
                    //return dataset
                }
            }
            return ds.Tables[0];
        }

        public static decimal FetchAverageCostOfGoodsLeftByItemNo(decimal balQty, string ItemNo, int LocationID)
        {
            /*
            if (balQty <= 0)
            {
                return new Item(ItemNo).FactoryPrice;
            }
            string status = "";
            DataTable dt = InventoryController.
                FetchFIFOCostOfGoods(balQty, ItemNo, LocationID, out status);

            int SumQty = 0;
            decimal SumQtyPrice = 0;
            int tmp1; decimal tmp2;
            if (dt == null)
            {
                return new Item(ItemNo).FactoryPrice;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (int.TryParse(dt.Rows[i]["Quantity"].ToString(), out tmp1)
                    && decimal.TryParse(dt.Rows[i]["CostOfGoods"].ToString(), out tmp2))
                {
                    SumQty += tmp1;
                    SumQtyPrice += tmp1 * tmp2;
                }
            }
            if (SumQty > 0)
            {
                return (SumQtyPrice / SumQty);
            }
            else
            {
                return new Item(ItemNo).FactoryPrice;
            }
             * */
            return ItemSummaryController.FetchCostPrice(ItemNo, LocationID);
        }
        
        public static decimal GetStockBalanceQtyByItem(string itemno, int locationID, out string status)
        {
            return GetStockBalanceQtyByItemByDate(itemno, locationID, DateTime.Now, out status);
        }

        public static ArrayList FetchStockOutReason()
        {
            InventoryStockOutReasonCollection arK = new InventoryStockOutReasonCollection();
            arK.Where(InventoryStockOutReason.Columns.ReasonID, Comparison.GreaterOrEquals, 2);
            arK.Load();

            ArrayList ar = new ArrayList();
            for (int i = 0; i < arK.Count; i++)
            {
                ar.Add(arK[i].ReasonName);
            }
            return ar;
        }

        public static void UndoStockOut(string InventoryHdrRefNo,
            string itemno, decimal ReturnedToStockQty)
        {
            InventoryHdr hdr
                = new InventoryHdr(InventoryHdrRefNo);

            InventoryDetCollection det = hdr.InventoryDetRecords();
            InventoryDet StockInDet;
            //Find item -- 
            for (int i = 0; i < det.Count; i++)
            {
                //split ReturnedToStockQty
                if (det[i].ItemNo == itemno & det[i].RemainingQty == 0 & det[i].Quantity >= ReturnedToStockQty)
                {
                    //Normal situation....                      
                    //Fetch the stock in ref no....
                    StockInDet = new InventoryDet(det[i].StockInRefNo);
                    if (!det[i].IsDiscrepancy)
                    {
                        if (StockInDet.ItemNo == det[i].ItemNo
                                && StockInDet.RemainingQty < StockInDet.Quantity
                                && StockInDet.Quantity - StockInDet.RemainingQty >= ReturnedToStockQty)
                        {
                            StockInDet.RemainingQty += ReturnedToStockQty;

                            StockInDet.Save();
                            ReturnedToStockQty = 0;
                            InventoryDet.Delete("InventoryDetRefNo", det[i].InventoryDetRefNo);
                        }
                    }
                    else
                    {
                        InventoryDet.Delete("InventoryDetRefNo", det[i].InventoryDetRefNo);
                    }
                }
            }
        }

        public static void UndoStockOut(string InventoryHdrRefNo)
        {
            QueryCommandCollection cmd = new QueryCommandCollection();

            InventoryHdr hdr
                = new InventoryHdr(InventoryHdrRefNo);
            if (hdr.MovementType != InventoryMovementType_StockOut)
                return;

            InventoryDetCollection det = hdr.InventoryDetRecords();
            InventoryDet StockInDet;

            InventoryController invCtrl = new InventoryController();
            
            //Find item -- 
            
            for (int i = 0; i < det.Count; i++)
            {
                //Normal situation....                      
                //Fetch the stock in ref no....                                        
                StockInDet = new InventoryDet(det[i].StockInRefNo);
                if (StockInDet.IsLoaded && !StockInDet.IsNew)
                {
                    StockInDet.RemainingQty += det[i].Quantity;
                    cmd.Add(StockInDet.GetSaveCommand());
                    Query qr = InventoryDet.CreateQuery();
                    qr.QueryType = QueryType.Delete;
                    qr.AddWhere("InventoryDetRefNo", det[i].InventoryDetRefNo);
                    cmd.Add(qr.BuildDeleteCommand());
                    Logger.writeLog("Successful");
                }
            }

            Query qr2 = InventoryDet.CreateQuery();
            qr2.QueryType = QueryType.Delete;
            qr2.AddWhere("InventoryHdrRefNo", hdr.InventoryHdrRefNo);
            cmd.Add(qr2.BuildDeleteCommand());
            DataService.ExecuteTransaction(cmd);        
            
        }

        public static bool UndoStockOutCreateNewInv(string InventoryHdrRefNo)
        {
            QueryCommandCollection cmd = new QueryCommandCollection();

            InventoryHdr hdr
                = new InventoryHdr(InventoryHdrRefNo);
            if (hdr.MovementType != InventoryMovementType_StockOut)
                return false;

            InventoryDetCollection det = hdr.InventoryDetRecords();
            InventoryDet StockInDet;

            InventoryController invCtrl = new InventoryController();

            //Find item -- 
            string status = "";
            for (int i = 0; i < det.Count; i++)
            {
                //Normal situation....                      
                //Fetch the stock in ref no....                                        
                StockInDet = new InventoryDet(det[i].StockInRefNo);
                if (StockInDet.IsLoaded && !StockInDet.IsNew)
                {
                    StockInDet.RemainingQty += det[i].Quantity;
                    cmd.Add(StockInDet.GetSaveCommand());
                    Query qr = InventoryDet.CreateQuery();
                    qr.QueryType = QueryType.Delete;
                    qr.AddWhere("InventoryDetRefNo", det[i].InventoryDetRefNo);
                    //cmd.Add(qr.BuildDeleteCommand());
                    //Logger.writeLog("Successful");
                }

                invCtrl.AddItemIntoInventory(det[i].ItemNo, det[i].Quantity.GetValueOrDefault(0), out status);
            }

            Query qr2 = InventoryDet.CreateQuery();
            qr2.QueryType = QueryType.Delete;
            qr2.AddWhere("InventoryHdrRefNo", hdr.InventoryHdrRefNo);
            cmd.Add(qr2.BuildDeleteCommand());
            //DataService.ExecuteTransaction(cmd);        
            if (!invCtrl.StockIn("Sync", (int)hdr.InventoryLocationID, false, true, out status))
            {
                return false;
            }
            return true;
        }
        

        /*
        public static int GetTotalQtyByItemNoAndMovementTypeAndLocationID
                (DateTime StartDate, DateTime EndDate, string ItemNo, string movementType, int LocationID)
        {
            return (int)SPs.GetTotalQtyByItemNoAndMovementTypeAndLocationID(StartDate, EndDate, ItemNo, movementType, LocationID).ExecuteScalar();            
        }*/

        public static decimal GetTotalUndeductedSalesByItem(string ItemNo, int LocationID)
        {
            string SQL = "declare @itemno as varchar(50); " +
                            "declare @locationid as int; " +
                            "set @itemno = '" + ItemNo + "'; " +
                            "set @locationid = " + LocationID + "; " +
                            "select isnull(sum(quantity),0) from orderdet x inner join OrderHdr y  " +
                            "on x.OrderHdrID = y.OrderHdrID  " +
                            "where x.isvoided=0 and y.isvoided=0 and x.InventoryHdrRefNo = '' and itemno = @itemno " +
                            "and IsPreOrder = 0 " +
                            "and (PointOfSaleID in " +
                            "(select c.PointOfSaleID  " +
                            "from Outlet a inner join InventoryLocation b " +
                            "on a.InventoryLocationID = b.InventoryLocationID " +
                            "inner join PointOfSale c " +
                            "on c.OutletName = a.OutletName " +
                            "where a.InventoryLocationID = @locationid) or @locationid = 0) ";

            QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
            object obj = DataService.ExecuteScalar(cmd);
            if (obj is decimal) return (decimal)obj;
            return 0;


        }

        public static decimal GetTotalQtyByItemNoAndMovementTypeAndLocationID
            (DateTime StartDate, DateTime EndDate, int LocationID,
            string ItemNo, string movementType)
        {
            if (LocationID != 0) //Dont filter by location if it is zero
            {
                return (decimal)SPs.GetTotalQtyByItemNoAndMovementTypeAndLocationID(StartDate, EndDate, ItemNo, movementType, LocationID).ExecuteScalar();
            }
            else
            {
                return (decimal)SPs.GetTotalQtyByItemNoAndMovementType(StartDate, EndDate, ItemNo, movementType).ExecuteScalar();
            }
        }

        public static decimal GetLatestCostPriceByItemNoAndLocationID(DateTime StartDate, DateTime EndDate, int LocationID, string ItemNo)
        {
            if (LocationID != 0)
            {
                //return 12;
                return (decimal)SPs.GetLatestCostPriceByItemNoAndLocationID(StartDate, EndDate, ItemNo, LocationID).ExecuteScalar();


            }
            else
            {
                //return 13;
                return (decimal)SPs.GetLatestCostPriceByItemNo(StartDate, EndDate, ItemNo).ExecuteScalar();
            }

        }

        public static decimal FetchCostOfGoodsByItemNoForSales(string ItemNo, int LocationID)
        {
            decimal costPrice = 0;

            try
            {
                if (AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SalesCostOfGoods) != "" && AppSetting.GetSetting(AppSetting.SettingsName.Inventory.SalesCostOfGoods) == "Item Summary Avg Cost Price")
                {
                    Query qr = new Query("ItemSummary");
                    qr.AddWhere(ItemSummary.Columns.ItemNo, ItemNo);
                    qr.AddWhere(ItemSummary.Columns.InventoryLocationID, LocationID);
                    var itemSummary = new ItemSummaryController().FetchByQuery(qr).FirstOrDefault();
                    if (itemSummary != null
                        && itemSummary.BalanceQty.GetValueOrDefault(0) > 0
                        && itemSummary.CostPrice.GetValueOrDefault(0) > 0)
                        costPrice = itemSummary.CostPrice.GetValueOrDefault(0);
                    else
                    {
                        Item theItem = new Item(Item.Columns.ItemNo, ItemNo);
                        costPrice = theItem.FactoryPrice;
                    }
                }
                else
                {
                    Item theItem = new Item(Item.Columns.ItemNo, ItemNo);
                    costPrice = theItem.AvgCostPrice == null ? 0 : (decimal)theItem.AvgCostPrice ;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
            }

            return costPrice;
            

        }

        public static decimal FetchCostOfGoodsByItemNo(string ItemNo, int LocationID)
        {
            /*string CostingMethod = AppSetting.GetSetting(AppSetting.SettingsName.Inventory.CostingMethod);
            if (CostingMethod == null) CostingMethod = "";
            CostingMethod = CostingMethod.ToLower();

            if (CostingMethod == InventoryController.CostingTypes.FIFO)
                return InventoryController.FetchCostOfGoodsByItemNo_FIFO(ItemNo, LocationID);
            else if (CostingMethod == InventoryController.CostingTypes.FixedAvg)
                return InventoryController.FetchCostOfGoodsByItemNo_FixedAvg(ItemNo, LocationID);
            else
                return InventoryController.FetchCostOfGoodsByItemNo_FIFO(ItemNo, LocationID);
             */
            return ItemSummaryController.FetchCostPrice(ItemNo, LocationID);
             
        }
        public static decimal FetchCostOfGoodsByItemNo_FIFO(string ItemNo, int LocationID)
        {
            string status = "";
            DataRow dr = InventoryController.FetchFIFOCostOfGoods(ItemNo, LocationID, out status);
            decimal result;
            if (dr != null && decimal.TryParse(dr["CostOfGoods"].ToString(), out result))
            {

                return result;
            }
            else
            {
                // -- Fetch previos transaction history --
                DataSet ds = SPs.FetchCostOfGoodsToHandleNegativeQty(ItemNo, InventoryController.InventoryMovementType_StockIn).GetDataSet();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && decimal.TryParse(ds.Tables[0].Rows[0]["CostOfGoods"].ToString(), out result))
                {
                    return result;
                }
            }
            return 0.0M;

        }
        public static decimal FetchCostOfGoodsByItemNo_FixedAvg(string ItemNo, int LocationID)
        {
            string sqlString =
                "DECLARE @ItemNo NVARCHAR(MAX); " +
                "DECLARE @InventoryLocationID INT; " +
                "SET @ItemNo = N'" + ItemNo + "'; " +
                "SET @InventoryLocationID = " + LocationID.ToString() + "; " +
                "SELECT ISNULL(CASE WHEN SUM(Quantity) = 0 THEN 0 ELSE SUM(Quantity * CostOfGoods) / SUM(Quantity) END, 0) AS CostOfGoods " +
                "FROM InventoryHdr IH " +
                    "INNER JOIN InventoryDet ID ON IH.InventoryHdrRefNo = ID.InventoryHdrRefNo " +
                "WHERE MovementType LIKE '% In' " +
                    "AND ItemNo = @ItemNo AND InventoryLocationID = @InventoryLocationID ";

            object TmpRst = DataService.ExecuteScalar(new QueryCommand(sqlString));
            if (TmpRst == null) return 0.0M;

            decimal Rst = 0;
            decimal.TryParse(TmpRst.ToString(), out Rst);
            return Rst;
        }

        public static DataTable FetchUndeductedStockSummary(out string status)
        {
            try
            {
                string SQL = "select sum(quantity) as quantity,inventorylocationname " +
                             "from orderhdr a inner join orderdet b " +
                             "on a.orderhdrid = b.orderhdrid " +
                             "inner join item x on x.itemno = b.itemno " + 
                             "inner join pointofsale c on a.pointofsaleid = c.pointofsaleid " +
                             "inner join outlet d on c.outletname = d.outletname " +
                             "inner join inventorylocation e on e.inventorylocationid = d.inventorylocationid " +
                             "where x.isininventory=1 and a.isvoided=0 and b.isvoided=0 and b.inventoryhdrrefno='' " +
                             "group by inventorylocationname ";

                QueryCommand cmd = new QueryCommand(SQL, "PowerPOS");
                status = "";
                return DataService.GetDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
                return null;
            }
        }

        public static DataTable FetchItemWithMinimumQuantity()
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }

        public static void UpdateCostPrice()
        {
        }
        public static void UpdateCostPrice_FixedAVG()
        {
        }

        public static DataTable InventoryFetchItemWithFilter(string searchText, string supplierName, string numOfDays,
            string outletName, int invLocation, out string message)
        {
            Supplier s = new Supplier(Supplier.Columns.SupplierName, supplierName);
            int supplierID = 0;
            if (!s.IsNew)
                supplierID = s.SupplierID;

            return InventoryFetchItemWithFilterWithSupplierID(searchText, supplierID, numOfDays, outletName, invLocation, out message);
        }

        public static DataTable InventoryFetchItemWithFilterWithSupplierID(string searchText, int supplierID, string numOfDays, string outletName, int invLocation, out string message)
        {
            DataTable dt = new DataTable();
            message = "";
            try
            {

                string sql = @"
                                DECLARE @days int, @OutletName varchar(50), @InventoryLocationID int, 
                                        @StartDate1 datetime, @StartDate2 datetime, @StartDate3 datetime,
                                        @EndDate1 datetime, @EndDate2 datetime, @EndDate3 datetime

                                SET @days = {2} 
                                SET @OutletName = '{3}' 
                                SET @InventoryLocationID = {4} 

                                SET @StartDate1 = DATEADD(day, -@days * 1, CONVERT(date, GETDATE()))
                                SET @StartDate2 = DATEADD(day, -@days * 2, CONVERT(date, GETDATE()))
                                SET @StartDate3 = DATEADD(day, -@days * 3, CONVERT(date, GETDATE()))
                                SET @EndDate1 = DATEADD(s, -1, @StartDate1 + @days)
                                SET @EndDate2 = DATEADD(s, -1, @StartDate2 + @days)
                                SET @EndDate3 = DATEADD(s, -1, @StartDate3 + @days)

                                SELECT vis.*, 
                                    ISNULL(sales1.Quantity, 0) AS Sales1, 
                                    ISNULL(sales2.Quantity, 0) AS Sales2, 
                                    ISNULL(sales3.Quantity, 0) AS Sales3, 
                                    0.00 AS OrderQty, 
                                    ISNULL(its.BalanceQty, 0) AS OnHandQty,
                                    vis.UOM as SalesUOM, 1 as ConvRate   
                                FROM ViewItemSupplier vis
                                    LEFT JOIN (
                                            SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo, 
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN ISNULL(om.Qty,0) ELSE od.Quantity END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
												LEFT JOIN OrderDetUOMConversion om on om.OrderdetID = od.OrderDetID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                                INNER JOIN PointOfSale p on p.PointOfSaleID =  oh.PointOfSaleID
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND p.OutletName = @OutletName
                                                AND oh.OrderDate BETWEEN @StartDate1 AND @EndDate1
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales1 ON sales1.ItemNo = vis.ItemNo
                                    LEFT JOIN (
                                            SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo, 
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN ISNULL(om.Qty,0) ELSE od.Quantity END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
												LEFT JOIN OrderDetUOMConversion om on om.OrderdetID = od.OrderDetID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                                INNER JOIN PointOfSale p on p.PointOfSaleID =  oh.PointOfSaleID
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND p.OutletName = @OutletName
                                                AND oh.OrderDate BETWEEN @StartDate2 AND @EndDate2
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales2 ON sales2.ItemNo = vis.ItemNo
                                    LEFT JOIN (
                                             SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo,  
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN ISNULL(om.Qty,0) ELSE od.Quantity END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
												LEFT JOIN OrderDetUOMConversion om on om.OrderdetID = od.OrderDetID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                                INNER JOIN PointOfSale p on p.PointOfSaleID =  oh.PointOfSaleID
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND p.OutletName = @OutletName
                                                AND oh.OrderDate BETWEEN @StartDate3 AND @EndDate3
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales3 ON sales3.ItemNo = vis.ItemNo
                                    LEFT JOIN ItemSummary its ON its.ItemNo = vis.ItemNo AND its.InventoryLocationID = @InventoryLocationID 
                                WHERE (vis.ItemNo LIKE N'{0}' OR vis.ItemName LIKE N'{0}' OR vis.CategoryName LIKE N'{0}' OR vis.UOM LIKE N'{0}' 
                                        OR vis.Attributes1 LIKE N'{0}' OR vis.Attributes2 LIKE N'{0}' OR vis.Attributes3 LIKE N'{0}' OR vis.Attributes4 LIKE N'{0}' 
                                        OR vis.Attributes5 LIKE N'{0}' OR vis.Attributes6 LIKE N'{0}' OR vis.Attributes7 LIKE N'{0}' OR vis.Attributes8 LIKE N'{0}') 
                                    AND vis.SupplierID = {1}
                                ORDER BY vis.ItemNo";
                sql = string.Format(sql, searchText, supplierID, numOfDays, outletName, invLocation);
                dt = DataService.GetDataSet(new QueryCommand(sql, "PowerPOS")).Tables[0];
                dt.PrimaryKey = new DataColumn[] { dt.Columns["ItemNo"] };
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return dt;
        }

        public static Decimal InventoryFetchItemSales(string itemNo, int supplierID, string multiply, 
            string rangeOfDay, int invLocation, string username, out string message)
        {
            Decimal dt = 0;
            message = "";
            int rangeDay = 0;
            if (string.IsNullOrEmpty(rangeOfDay) || !Int32.TryParse(rangeOfDay, out rangeDay))
                rangeDay = 1;
            string supplierQuery = "";
            try
            {

                bool isUseSupplierPortal = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.User.UseSupplierPortal), false);

                List<string> supID = new List<string>();
                string supp = "";
                UserMst us = new UserMst(UserMst.Columns.UserName, username);

                if (supplierID == 0)
                {
                    if (isUseSupplierPortal && us.IsSupplier && us.IsRestrictedSupplierList && supplierID == 0)
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

                        supplierQuery = string.Format(" AND vis.SupplierID in ({0})", supp);
                    }
                    
                }
                else
                {
                    supplierQuery = string.Format(" AND vis.SupplierID = {0}", supplierID.ToString());
                }

                string sql = @"
                                DECLARE @days int, @OutletName varchar(50), @InventoryLocationID int, 
                                        @StartDate1 datetime, @StartDate2 datetime, @StartDate3 datetime,
                                        @EndDate1 datetime, @EndDate2 datetime, @EndDate3 datetime

                                SET @days = {2} 
                                SET @InventoryLocationID = {3} 

                                SET @StartDate1 = DATEADD(day, -@days * {4}, CONVERT(date, GETDATE()))
                                SET @EndDate1 = DATEADD(s, -1, @StartDate1 + @days)
                                
                                SELECT ISNULL(sales1.Quantity, 0) AS Sales1                                    
                                FROM ViewItemSupplier vis
                                    LEFT JOIN (
                                            SELECT CASE WHEN ISNULL(it.userflag7,0) = 1 THEN it.userfld8 ELSE od.ItemNo END as ItemNo, 
				                                SUM(CASE WHEN ISNULL(it.userflag7,0) = 1 THEN ISNULL(om.Qty,0) ELSE od.Quantity END) Quantity
                                            FROM OrderHdr oh
                                                INNER JOIN OrderDet od ON od.OrderHdrID = oh.OrderHdrID
												LEFT JOIN OrderDetUOMConversion om on om.OrderdetID = od.OrderDetID
				                                INNER JOIN Item it ON it.ItemNo = od.ItemNo AND ISNULL(it.Deleted,0) = 0
                                                INNER JOIN PointOfSale p on p.PointOfSaleID =  oh.PointOfSaleID
                                                INNER JOIN Outlet o on o.OutletName = p.OutletName
                                            WHERE oh.IsVoided = 0 AND od.IsVoided = 0
                                                AND o.InventoryLocationID = @InventoryLocationID
                                                AND oh.OrderDate BETWEEN @StartDate1 AND @EndDate1
                                            GROUP BY CASE WHEN ISNULL(it.userflag7,0) = 1 then it.userfld8 else od.ItemNo END
                                        ) sales1 ON sales1.ItemNo = vis.ItemNo                                    
                                WHERE vis.ItemNo = N'{0}' {1}     
                                ORDER BY vis.ItemNo";
                sql = string.Format(sql, itemNo, supplierQuery, rangeOfDay, invLocation, multiply );
                Logger.writeLog(sql);
                var dec = DataService.ExecuteScalar(new QueryCommand(sql, "PowerPOS"));

                if (dec != null)
                    dt = (decimal)dec;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                Logger.writeLog(ex.Message);
            }
            return dt;
        }
    }
}
