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
    public partial class InventoryController
    {
        private const string ISSTOCKOUTRUNNING = "IsStockOutRunning";

        /*
        public bool StockOutNonTransaction
            (string username, int StockOutReasonID, int InventoryLocationID, 
                bool IsAdjustment,
                bool deductRemainingQty, out string status)
        {
            try
            {
                //Check whether inventory detail is valid
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }
                //Set inventory header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = InventoryLocationID;
                if (IsAdjustment)
                {
                    InvHdr.MovementType = InventoryController.InventoryMovementType_AdjustmentOut;
                }
                else
                {
                    InvHdr.MovementType = InventoryController.InventoryMovementType_StockOut;
                }
                InvHdr.StockOutReasonID = StockOutReasonID;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                InvHdr.Save();

                //fetch the FIFO information from corresponding Stock In Inventory Details
                int i = 0;


                while (i < InvDet.Count) //For every inventory Details....
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    if (deductRemainingQty)
                    {
                        if (InvDet[i].Quantity > 0)
                            DistributeInventoryDetQuantityNonTransaction(ref i, out status);
                        else
                            i += 1;
                    }
                    else
                    {
                        InvDet[i].Save();
                        i += 1;
                    }
                }

                status = "";
                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;

                return false;
            }
        }
        */
        private void DistributeInventoryDetQuantityNonTransaction
            (ref int index, out string status)
        {
            status = "";
            decimal RemainingLineQty;
            DataRow dr;
            InventoryDet tmpDet;
            //calculate cost of goods out
            RemainingLineQty = InvDet[index].Quantity.GetValueOrDefault(0);

            while (RemainingLineQty > 0)
            {
                dr = FetchFIFOCostOfGoods(InvDet[index].ItemNo, (int)InvHdr.InventoryLocationID, out status);

                if (dr != null)
                {
                    //Allocate the quantity
                    RemainingLineQty = RemainingLineQty - (int)dr["RemainingQty"];

                    //Are there more to allocate?
                    if (RemainingLineQty > 0)
                    {
                        //yes
                        InvDet[index].CostOfGoods = decimal.Parse(dr["CostOfGoods"].ToString());
                        InvDet[index].Gst = double.Parse(dr["GST"].ToString());
                        InvDet[index].Quantity = int.Parse(dr["RemainingQty"].ToString());
                        InvDet[index].StockInRefNo = dr["InventoryDetRefNo"].ToString();

                        //Create a new Inventory Detail
                        //To be inserted after the current inventory detail
                        //this inventory detail have their quantity set to the remaining qty....
                        tmpDet = new InventoryDet();
                        tmpDet.Discount = InvDet[index].Discount;
                        tmpDet.ExpiryDate = InvDet[index].ExpiryDate;
                        tmpDet.FactoryPrice = decimal.Parse(dr["FactoryPrice"].ToString()); //InvDet[index].FactoryPrice;
                        tmpDet.InventoryHdrRefNo = InvDet[index].InventoryHdrRefNo;
                        tmpDet.InventoryDetRefNo = InvDet[index].InventoryHdrRefNo + "." + (index + 2).ToString();
                        tmpDet.ItemNo = InvDet[index].ItemNo;
                        tmpDet.Quantity = RemainingLineQty;
                        tmpDet.Remark = InvDet[index].Remark;
                        tmpDet.UniqueID = Guid.NewGuid();
                        InvDet.Insert(index + 1, tmpDet);

                        //Update DR - Deduct the remaining quantity so that we can procede with SPs                                
                        Query qry = new Query("InventoryDet");
                        qry.AddUpdateSetting(InventoryDet.Columns.RemainingQty, 0);
                        qry.AddWhere(InventoryDet.Columns.InventoryDetRefNo, dr["InventoryDetRefNo"].ToString());
                        qry.Execute();

                        //Create the insert command                        
                        InvDet[index].Save();

                        index += 1;
                    }
                    else
                    {
                        //No more quantity - the last one
                        //this quantity may be smaller than the current remaining qty or equal...                 
                        InvDet[index].Quantity = RemainingLineQty + (int)dr["RemainingQty"];
                        InvDet[index].CostOfGoods = decimal.Parse(dr["CostOfGoods"].ToString());
                        InvDet[index].Gst = double.Parse(dr["GST"].ToString());
                        InvDet[index].StockInRefNo = dr["InventoryDetRefNo"].ToString();

                        Query qry = new Query("InventoryDet");
                        qry.AddUpdateSetting(InventoryDet.Columns.RemainingQty, Math.Abs(RemainingLineQty));
                        qry.AddWhere(InventoryDet.Columns.InventoryDetRefNo, dr["InventoryDetRefNo"].ToString());
                        qry.Execute();

                        InvDet[index].Save();
                        index += 1;
                    }
                }
                else
                {
                    return;                    
                }
            }
        }

        public static bool CalculateLineAmountDataIntegrity(OrderDet myOrderDetItem, out decimal expectedResult)
        {
            try
            {
                decimal GST = 7.0M;
                decimal result = 0.0M;
                //if (DISCOUNT_BY_PERCENTAGE)
                //{
                //Discount by percentage
                int GSTRule = 0;
                if (myOrderDetItem.Item.GSTRule.HasValue) GSTRule = myOrderDetItem.Item.GSTRule.Value;

                if (myOrderDetItem.IsPromo)
                {
                    if (myOrderDetItem.UsePromoPrice.HasValue && myOrderDetItem.UsePromoPrice.Value)
                    {
                        if (GSTRule == 1)
                        {
                            //Exclusive GST
                            result = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                             (decimal)myOrderDetItem.PromoUnitPrice, 2)
                             * (1 + ((decimal)GST) / 100); //The GST part                       

                            myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100);
                        }
                        else
                        {
                            //Inclusive GST
                            result =
                                Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                             (decimal)myOrderDetItem.PromoUnitPrice, 2);


                            if (GSTRule == 2) //Inclusive GST
                            {
                                myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100); ;
                            }
                            else
                            {
                                myOrderDetItem.GSTAmount = 0;
                            }
                        }
                    }
                    else
                    {
                        if (GSTRule == 1)
                        {
                            //Exclusive GST
                            result = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                                (decimal)myOrderDetItem.UnitPrice *
                                (decimal)(1 - (myOrderDetItem.PromoDiscount / 100))
                                * (1 + ((decimal)GST) / 100), 2); //The GST part        

                            myOrderDetItem.GSTAmount =
                                (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) *
                                ((decimal)GST / 100);
                        }
                        else
                        {
                            //Inclusive GST & NO GST
                            result = Math.Round(myOrderDetItem.Quantity.GetValueOrDefault(0) *
                                (decimal)myOrderDetItem.UnitPrice *
                                (decimal)(1 - (myOrderDetItem.PromoDiscount / 100)), 2);

                            if (GSTRule == 2) //Inclusive GST
                            {
                                myOrderDetItem.GSTAmount = (myOrderDetItem.PromoAmount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100); ;
                            }
                            else
                            {
                                myOrderDetItem.GSTAmount = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (GSTRule == 1)
                    {
                        //Exclusive GST
                        result = Math.Round(
                            myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            myOrderDetItem.UnitPrice *
                            (1 - (myOrderDetItem.Discount / 100))
                             * (1 + ((decimal)GST) / 100), 2); //The GST part            
                        myOrderDetItem.GSTAmount = (myOrderDetItem.Amount / (1 + ((decimal)GST) / 100)) * ((decimal)GST / 100);
                    }
                    else
                    {
                        //Inclusive GST
                        result = Math.Round(
                            myOrderDetItem.Quantity.GetValueOrDefault(0) *
                            myOrderDetItem.UnitPrice *
                            (1 - (myOrderDetItem.Discount / 100)), 2);

                        if (GSTRule == 2) //Inclusive GST
                        {
                            myOrderDetItem.GSTAmount = myOrderDetItem.Amount / (1 + ((decimal)GST) / 100) * ((decimal)GST / 100); ;
                        }
                        else
                        {
                            myOrderDetItem.GSTAmount = 0;
                        }
                    }
                }

                expectedResult = result;
                if (expectedResult - myOrderDetItem.Amount <= 0.01M)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                expectedResult = -1;
                return false;
            }
        }

        //Perform deduction for sales orders
        public static bool AssignStockOutToConfirmedOrderUsingTransaction(bool AllowDeductionForSalesBeforeSettlement)
        {            
            try
            {
                #region *) Validation: No movement is allowed when there are un-adjusted stock take in place
                /*if (StockTakeController.IsThereUnAdjustedStockTake())
                {
                    Logger.writeLog("There are Stock take in progress, no stock movement is allowed");
                    return false;
                }*/
                #endregion

                #region *) If there are some transaction still running then no need to run
                if (AppSetting.CastBool(AppSetting.GetSetting("HighVolumeTransaction"), false))
                {
                    if (AppSetting.CastBool(AppSetting.GetSetting("ISSTOCKOUTRUNNING"), false))
                        return true;
                    AppSetting.SetSetting(ISSTOCKOUTRUNNING, "True");
                }
                #endregion

                string status;
                OrderDetCollection myOrderDet;
                InventoryController itCtr;
                string SQL =
                        "SELECT orderdetid FROM OrderDet where (InventoryHdrRefNo = '' OR InventoryHdrRefNo is null) ORDER BY orderdetid asc";

                DataTable dt = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS")).Tables[0];

                //Create the inventory Controller to perform inventory transactions                           
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        OrderDet od = new OrderDet(dt.Rows[i][0]);
                        if (!od.IsLoaded) continue;
                        //Logger.writeLog("Stock Out >> " + od.OrderDetID);
                        od.DoStockOut();

                        /*string sqlString = "Delete from tmpOrderDet where OrderDetID = '" + od.OrderDetID + "'";
                        DataService.ExecuteQuery(new QueryCommand(sqlString));*/

                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Inventory Update failed!" + ex.Message);
                    }
                }

                #region set Voided Order Item to nonInventory
                SQL =
                    "SELECT orderdetid FROM OrderDet " +
                    "WHERE InventoryHdrRefNo <> 'NONINVENTORY' AND InventoryHdrRefNo <> 'ADJUSTED' " +
                    "AND OrderHdrID in (Select OrderHdrID from OrderHdr where Isvoided = 1) ";
                DataTable dtVoid = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS")).Tables[0];

                for (int i = 0; i < dtVoid.Rows.Count; i++)
                {
                    try
                    {
                        OrderDet od = new OrderDet(dtVoid.Rows[i][0]);
                        if (!od.IsLoaded) continue;
                        Logger.writeLog("Stock Out For Voided >> " + od.OrderDetID);
                        od.DoStockOutForVoided();
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Inventory Update failed!" + ex.Message);
                    }
                    
                }

                #endregion

                #region *) Revert Status of the is stock out running to false
                AppSetting.SetSetting(ISSTOCKOUTRUNNING, "False");
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                //Logger.writeLogToFile("Inventory Update failed!");
                Logger.writeLog("Inventory Update failed!" + ex.Message);
                #region *) Revert Status of the is stock out running to false
                AppSetting.SetSetting(ISSTOCKOUTRUNNING, "False");
                #endregion

                return false;
            }
        }

        //Perform deduction for pre order sales.
        public static bool AssignStockOutToPreOrderSalesUsingTransaction()
        {
            try
            {
                DeliveryOrderDetailCollection doDet;
                string SQL =
                    "SELECT * FROM DeliveryOrderDetails " +
                    "WHERE (InventoryHdrRefNo = '' OR InventoryHdrRefNo is null) ";

                DataTable dt = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS")).Tables[0];

                //Get all the DeliveryOrderDetails that has not been adjusted
                doDet = new DeliveryOrderDetailCollection();
                doDet.Load(dt);

                //Create the inventory Controller to perform inventory transactions
                for (int i = 0; i < doDet.Count; i++)
                {
                    try
                    {
                        doDet[i].DoStockOut();
                    }
                    catch (Exception ex)
                    {
                        Logger.writeLog("Inventory Update failed!" + ex.Message);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Inventory Update failed!" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Fix past data: 
        /// Generate InventoryHdr for all sales that is marked as "ADJUSTED", so the "Adjusted" status can be transfered to Client's POS database
        /// </summary>
        /// <remarks>
        /// Function: 
        /// - To make sure the ADJUSTED status is transfered down to Client's computer
        /// - To fix past data only. The recent/future data is handled in OD.DoStockOut()
        /// </remarks>
        /// <returns></returns>
        public static bool GenerateInventoryHdrForAdjustedSales()
        {
            try
            {
                string SQLString =
                    "INSERT INTO [InventoryHdr] " +
                        "([InventoryHdrRefNo],[InventoryDate],[UserName],[MovementType],[StockOutReasonID],[ExchangeRate],[PurchaseOrderNo],[InvoiceNo] " +
                        ",[DeliveryOrderNo],[Supplier],[FreightCharge],[DeliveryCharge],[Tax],[Discount],[Remark],[InventoryLocationID],[DepartmentID] " +
                        ",[CreatedOn],[ModifiedOn],[CreatedBy],[ModifiedBy],[UniqueID],[Deleted]) " +
                    "SELECT 'AD' + REPLACE(OrderDetID, '.', ''), OrderDetDate, 'SYSTEM', 'Stock Out', 0, 1, '', OrderDetID, NULL, '', 0, NULL, NULL, 0 " +
                        ", 'Adjusted Sales', LI.InventoryLocationID, NULL, GETDATE(), GETDATE(), 'Adjustment Script', 'Adjustment Script', NEWID(), 0 " +
                    "FROM OrderDet OD " +
                        "INNER JOIN OrderHdr OH ON OD.OrderHdrID = OH.OrderHdrID " +
                        "INNER JOIN PointOfSale LP ON OH.PointOfSaleID = LP.PointOfSaleID " +
                        "INNER JOIN Outlet LO ON LP.OutletName = LO.OutletName " +
                        "INNER JOIN InventoryLocation LI ON LO.InventoryLocationID = LI.InventoryLocationID " +
                    "WHERE OD.InventoryHdrRefNo = 'ADJUSTED' " +
                        "AND OrderDetID NOT IN (SELECT InvoiceNo FROM InventoryHdr WHERE InventoryHdrRefNo LIKE 'AD%') ";

                DataService.ExecuteQuery(new QueryCommand(SQLString, "PowerPOS"));

                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog("Generate InventoryHdr from Past \"Adjusted Sales\" failed!\n" + ex.Message);
                return false;
            }
        }

        public void setInvoiceNo(string OrderDetID)
        {
            InvHdr.InvoiceNo = OrderDetID;
        }
        /*
        public static bool AssignStockOutToConfirmedOrderUsingTransactionScopeForBeforeAfterSettlement()
        {
            string startRefNo = "-1";
            string EndRefNo = "-1";
            try
            {
                //Logger.writeLogToFile("Inventory Update Starts!");

                //No movement is allowed when there is a stock take in place and not adjusted
                if (StockTakeController.IsThereUnAdjustedStockTake())
                {
                    Logger.writeLog("There are Stock take in progress, no stock movement is allowed");
                    return false;
                }

                string status;
                OrderDetCollection myOrderDet;
                InventoryController itCtr;

                //Get all the OrderDet that has not been adjusted
                myOrderDet = new OrderDetCollection();
                myOrderDet.Where("InventoryHdrRefNo", "");
                myOrderDet.Where("IsVoided", false);
                myOrderDet.OrderByAsc(OrderDet.Columns.OrderDetDate);
                myOrderDet.Load();

                DateTime lastStockTakeDate;

                //Create the inventory Controller to perform inventory transactions                           
                for (int i = 0; i < myOrderDet.Count; i++)
                {
                    //Sales attached to inventory location?
                    if (myOrderDet[i].OrderHdr.PointOfSale.
                                Outlet.InventoryLocationID.HasValue)
                    {
                        int locID = myOrderDet[i].OrderHdr.PointOfSale.
                                    Outlet.InventoryLocationID.Value;

                        //Sales must be inventory item, and non voided
                        if (!myOrderDet[i].OrderHdr.IsVoided
                            & !(bool)myOrderDet[i].IsVoided
                            & myOrderDet[i].Item.IsInInventory)
                        {
                            decimal expectedResult = 0.0M;

                            //Check sanity & Data Integrity
                            if (!CalculateLineAmountDataIntegrity(myOrderDet[i], out expectedResult))
                            {
                                Logger.writeLog("Sales Data Inconsistency Found > " + myOrderDet[i].OrderDetID);
                                //Mail out                                                           
                            }
                            else
                            {
                                //DateTime lastSettlementDate = ClosingController.FetchLastClosingTime(myOrderDet[i].OrderHdr.PointOfSaleID);
                                //if ((myOrderDet[i].OrderDetDate).Subtract(lastSettlementDate).Ticks < 0)
                                {
                                    //Find the last stock take date for the location
                                    lastStockTakeDate =
                                        StockTakeController.FetchLastStockTakeDateByInventoryLocationByItemNo
                                        (myOrderDet[i].OrderHdr.PointOfSale.Outlet.InventoryLocationID.Value, myOrderDet[i].ItemNo);

                                    //Get stock balance quantity
                                    int balQty = InventoryController.GetStockBalanceQtyByItemByDate
                                        (myOrderDet[i].ItemNo, locID, DateTime.Now, out status);

                                    //The order must be after the last stock take- otherwise it will be ignored
                                    if ((myOrderDet[i].OrderDetDate).Subtract(lastStockTakeDate).Ticks > 0)
                                    {

                                        //if we have enough quantity to do deduction, we will proceed.
                                        if (balQty >= myOrderDet[i].Quantity)
                                        {
                                            itCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);

                                            //InventoryDate will mark the actual date deducted.
                                            //Sales Date will mark the date the item being sold.
                                            //They may not be the same
                                            itCtr.SetInventoryDate(DateTime.Now);
                                            //itCtr.SetInventoryDate(myOrderDet[i].OrderHdr.OrderDate);

                                            //Do Stock Out if there quantity is more than zero
                                            if (myOrderDet[i].Quantity >= 0)
                                            {
                                                itCtr.AddItemIntoInventory
                                                    (myOrderDet[i].ItemNo, myOrderDet[i].Quantity, out status);

                                                if (!itCtr.StockOut("SYSTEM", 0,
                                                    locID,
                                                    false, true, out status))
                                                {
                                                    status = "Error while performing inventory transaction. " + status;
                                                    Logger.writeLog(status);
                                                    throw new Exception(status);
                                                }
                                            }
                                            else
                                            {
                                                //do stock in if quantity is negative
                                                itCtr.AddItemIntoInventory
                                                    (myOrderDet[i].ItemNo, -myOrderDet[i].Quantity,
                                                    InventoryController.FetchAverageCostOfGoodsLeftByItemNo
                                                    (balQty, myOrderDet[i].ItemNo, locID), out status);

                                                itCtr.SetInventoryHeaderInfo("", "", "Stock in for order " + myOrderDet[i].OrderDetID, 0.0M, 1, 0.0M);
                                                if (!itCtr.StockIn
                                                    ("SYSTEM", locID, true, true, out status))
                                                {
                                                    status = "Error while performing inventory transaction. " + status;
                                                    Logger.writeLog(status);
                                                    throw new Exception(status);
                                                }
                                            }

                                            if (itCtr.GetInvHdrRefNo() == "" || itCtr.GetInvHdrRefNo() == null)
                                            {
                                                throw new Exception("Error performing inventory transaction.");
                                            }

                                            Query qr = OrderDet.CreateQuery();
                                            qr.AddWhere(OrderDet.Columns.OrderDetID, myOrderDet[i].OrderDetID);
                                            qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, itCtr.GetInvHdrRefNo());
                                            qr.AddUpdateSetting(OrderDet.Columns.CostOfGoodSold, itCtr.GetAverageCostOfGoodsByItem(myOrderDet[i].ItemNo));
                                            qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                                            qr.Execute();

                                            //Logger.writeLogToFile("SUCCESS for:" + myOrderDet[i].OrderDetID + " Item No" + myOrderDet[i].ItemNo);
                                            if (i == 0)
                                                startRefNo = itCtr.GetInvHdrRefNo();
                                            if (i == myOrderDet.Count - 1)
                                                EndRefNo = itCtr.GetInvHdrRefNo();
                                            Logger.writeLog(myOrderDet[i].OrderHdrID + " assigned to " + itCtr.GetInvHdrRefNo());

                                        }
                                        else
                                        {
                                            //Logger.writeLog("ERROR:Insufficient balance for:" + myOrderDet[i].OrderDetID + " Item No" + myOrderDet[i].ItemNo);
                                        }

                                    }
                                    else
                                    {
                                        Query qr = OrderDet.CreateQuery();
                                        qr.AddWhere(OrderDet.Columns.OrderDetID, myOrderDet[i].OrderDetID);
                                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                                        qr.AddUpdateSetting(OrderDet.Columns.CostOfGoodSold,
                                            InventoryController.FetchAverageCostOfGoodsLeftByItemNo(balQty, myOrderDet[i].ItemNo, locID));
                                        qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                                        qr.Execute();
                                    }
                                }
                            }
                        }
                        else
                        {
                            //if non inventory
                            Query qr = OrderDet.CreateQuery();
                            qr.AddWhere(OrderDet.Columns.OrderDetID, myOrderDet[i].OrderDetID);
                            qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "NONINVENTORY");
                            qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                            qr.Execute();
                        }
                    }
                }
                //Logger.writeLogToFile("Inventory Update success!");

                return true;
            }
            catch (Exception ex)
            {
                //Logger.writeLogToFile("Inventory Update failed!");
                Logger.writeLog("Inventory Update failed!" + ex.Message);
                return false;
            }
        }
        */
        /*
        public decimal GetAverageCostOfGoodsByItem(string itemno)
        {
            try
            {
                decimal TotalCostTimesQty = 0;
                int TotalQty = 0;

                for (int i = 0; i < InvDet.Count; i++)
                {
                    if (InvDet[i].ItemNo == itemno)
                    {
                        TotalQty += InvDet[i].Quantity;
                        TotalCostTimesQty += InvDet[i].Quantity * InvDet[i].CostOfGoods;
                    }
                }
                if (TotalQty == 0) return 0;

                return TotalCostTimesQty / TotalQty;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return 0;
            }
        }
        */
        /*
        public bool StockInNonTransaction
            (string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, out string status)
        {
            try
            {
                //Check whether it is a valid order
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                    return false;
                }

                //Set header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = InventoryLocationID;
                if (IsAdjustment)
                {
                    InvHdr.MovementType = InventoryController.InventoryMovementType_AdjustmentIn;
                }
                else
                {
                    InvHdr.MovementType = InventoryController.InventoryMovementType_StockIn;
                }
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                InvHdr.Discount = InvHdr.Discount * (decimal)InvHdr.ExchangeRate;
                InvHdr.Save("SYSTEM");

                decimal SumOfFactoryPriceAndQty = 0;

                if (!InvHdr.FreightCharge.HasValue)
                    InvHdr.FreightCharge = 0;

                //calculate sum of factory price * quantity for distributing Freight charges
                for (int i = 0; i < InvDet.Count; i++)
                {
                    SumOfFactoryPriceAndQty += (decimal)InvDet[i].Item.FactoryPrice
                        * InvDet[i].Quantity;
                }
                if (SumOfFactoryPriceAndQty == 0)
                    SumOfFactoryPriceAndQty = 0.0001M;

                //Inventory Detail - insert into Transaction
                for (int i = 0; i < InvDet.Count; i++)
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    InvDet[i].RemainingQty = InvDet[i].Quantity;


                    //CALCULATE Cost of Goods here             
                    if (CalculateCOGS)
                    {
                        //
                        if (InvHdr.ExchangeRate > 0)
                        {
                            InvDet[i].FactoryPrice = InvDet[i].FactoryPrice * (decimal)InvHdr.ExchangeRate;
                            //InvHdr.FreightCharge = InvHdr.FreightCharge * (decimal)InvHdr.ExchangeRate;                            
                            //InvHdr.FreightCharge = InvHdr.FreightCharge - InvHdr.Discount;

                        }

                        InvDet[i].FactoryPrice =
                            InvDet[i].FactoryPrice -
                            +(decimal)
                            ((((InvDet[i].Item.FactoryPrice * InvDet[i].Quantity)
                            / SumOfFactoryPriceAndQty)
                                * InvHdr.Discount) / InvDet[i].Quantity);
                        InvDet[i].CostOfGoods = InvDet[i].FactoryPrice
                            + (decimal)((((InvDet[i].Item.FactoryPrice * InvDet[i].Quantity) / SumOfFactoryPriceAndQty)
                            * InvHdr.FreightCharge.Value) / InvDet[i].Quantity);
                    }
                    InvDet[i].Gst = GetGST();
                }
                InvDet.SaveAll("SYSTEM");
                status = "";

                return true;
            }
            catch (Exception ex)
            {

                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;

                return false;
            }
        }
*/


    }
}
