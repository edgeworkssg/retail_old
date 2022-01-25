using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PowerPOS
{
    public partial class OrderDet
    {
        /// <summary>
        /// Generate Query Command to do Stock Out. Do it with caution and only after the OrderDet has been saved to DB
        /// </summary>
        /// <returns></returns>
        public void DoStockOut()
        {
            try
            {
                if (InventoryHdrRefNo != null && InventoryHdrRefNo != "")
                    return;

                string status = "";
                QueryCommandCollection Result = new QueryCommandCollection();
                string invHdrRef = "";
                #region *) Validation: No need to do anything if it is pre order
                if (IsPreOrder.Value) return;
                #endregion

                int InventoryLocationID;
                #region *) Validation: Make sure POS is attached to InventoryLocation
                InventoryLocationID = this.OrderHdr.PointOfSale.Outlet.InventoryLocationID.GetValueOrDefault(-1);
                if (InventoryLocationID == -1)
                {
                    Logger.writeLog("WARNING: POS is not attached to any InventoryLocation.");
                    throw new Exception("POS is not attached to any Inventory Location.\nPlease contact your administrator");
                }
                #endregion

                //#region *) Validation: No movement is allowed when there are un-adjusted stock take in place
                ///* Return true because we don't want to disturb the cashier with warning */
                //if (StockTakeController.IsThereUnAdjustedStockTake(InventoryLocationID))
                //{
                //    Logger.writeLog("There are Stock Take in progress, no stock movement is allowed.");
                //    return;
                //}
                //#endregion


                if (this.OrderHdr.IsVoided || (!this.Item.IsInInventory && !this.Item.NonInventoryProduct))
                {
                    #region *) Script: Mark OrderDet as NONINVENTORY
                    //if non inventory
                    Query qr = OrderDet.CreateQuery();
                    qr.AddWhere(OrderDet.Columns.OrderDetID, OrderDetID);
                    qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "NONINVENTORY");
                    qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                    Result.Add(qr.BuildCommand());
                    #endregion

                }
                else
                {
                    string ItemNoToDeduct = this.Item.NonInventoryProduct ? this.Item.DeductedItem : ItemNo;

                    //Find the last stock take date for the location
                    DateTime lastStockTakeDate;
                    #region *) Fetch: Get Last Stock Take Date
                    lastStockTakeDate = StockTakeController.FetchLastStockTakeDateByInventoryLocationByItemNo(InventoryLocationID, ItemNoToDeduct);
                    #endregion

                    //int balQty;
                    //balQty = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, InventoryLocationID, DateTime.Now, out status);



                    if (OrderDetDate < lastStockTakeDate)
                    {
                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID, OrderDetID);
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                        Result.Add(qr.BuildUpdateCommand());
                        #endregion
                    }
                    else
                    {
                        QueryCommandCollection cmd = new QueryCommandCollection();
                        bool UseOrderDate = AppSetting.CastBool(AppSetting.GetSetting("UseOrderDateForStockOut"), true);

                        #region *) Script: Insert Inventory Movement based on OrderDet
                        InventoryController invCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        invCtr.SetInventoryHdrRefNo(OrderDetID.Replace(".", ""));
                        invCtr.setInvoiceNo(OrderDetID);
                        if (UseOrderDate)
                            invCtr.SetInventoryDate(OrderDetDate);
                        else
                            invCtr.SetInventoryDate(DateTime.Now);
                        invCtr.SetInventoryLocation(InventoryLocationID);
                        decimal Cog = 0;
                        Item theItem = new Item(ItemNo);
                        if (Quantity >= 0)
                        {
                            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Recipe.EnableRecipeManagement), false))
                            {
                                #region *) Fetch Deduction List
                                Logger.writeLog("Deduct Recipe for " + ItemNo);

                                bool isRecipeDeductionSuccess = false;
                                string recipeDeductionStatus = "";

                                var recipeDetList = RecipeController.GetRecipeDeductionList(ItemNo, Quantity.GetValueOrDefault(0),
                                    InventoryLocationID, true, false,
                                    out isRecipeDeductionSuccess, out recipeDeductionStatus);

                                if (!isRecipeDeductionSuccess)
                                    throw new Exception(recipeDeductionStatus);

                                #endregion

                                foreach (var rd in recipeDetList)
                                {
                                    //Cog = ItemSummaryController.GetAvgCostPrice(rd.ItemNo, InventoryLocationID);

                                    #region get Cog
                                    decimal theAmount = Amount;
                                    theItem = new Item(rd.ItemNo);

                                    Logger.writeLog("Deduct Recipe Inggedient for " + rd.ItemNo + " and Qty" + Quantity.GetValueOrDefault(0).ToString("N2"));

                                    if (theItem.GSTRule == 1)
                                        theAmount = theAmount - GSTAmount.GetValueOrDefault(0);

                                    if (theItem.IsUsingFixedCOG)
                                    {
                                        if (theItem.FixedCOGType == ItemController.FIXEDCOG_PERCENTAGE)
                                        {
                                            Cog = (theAmount / Quantity.GetValueOrDefault(1)) * ((100 - theItem.FixedCOGValue) / 100);
                                        }
                                        else
                                        {
                                            Cog = theItem.FixedCOGValue;
                                        }
                                    }
                                    else
                                    {
                                        Cog = ItemSummaryController.GetAvgCostPrice(ItemNoToDeduct, InventoryLocationID);
                                    }
                                    #endregion

                                    invCtr.AddItemIntoInventoryForSales(rd.ItemNo, rd.Quantity, Cog, out status);
                                }
                            }
                            else
                            {
                                #region get Qty
                                decimal RealQty = Quantity.GetValueOrDefault(0);
                                if (this.Item.NonInventoryProduct)
                                {
                                    IDataReader reader = null;
                                    reader = OrderDetUOMConversion.FetchByParameter("OrderDetID", this.OrderDetID);

                                    if (reader.Read())
                                        RealQty = reader["Qty"].ToString().GetDecimalValue();
                                }
                                #endregion

                                #region get Cog
                                decimal theAmount = Amount;

                                if (theItem.GSTRule == 1)
                                    theAmount = theAmount - GSTAmount.GetValueOrDefault(0);

                                if (theItem.IsUsingFixedCOG)
                                {
                                    if (theItem.FixedCOGType == ItemController.FIXEDCOG_PERCENTAGE)
                                    {
                                        Cog = (theAmount / Quantity.GetValueOrDefault(1)) * ((100 - theItem.FixedCOGValue) / 100);
                                    }
                                    else
                                    {
                                        Cog = theItem.FixedCOGValue;
                                    }
                                }
                                else
                                {
                                    Cog = ItemSummaryController.GetAvgCostPrice(ItemNoToDeduct, InventoryLocationID);
                                }
                                #endregion

                                //old code
                                //invCtr.AddItemIntoInventoryForSales(ItemNo, Quantity.GetValueOrDefault(0), Cog, out status);

                                invCtr.AddItemIntoInventoryForSales(this.Item.NonInventoryProduct ? this.Item.DeductedItem : ItemNo, RealQty, ItemSummaryController.GetAvgCostPrice(ItemNoToDeduct, InventoryLocationID), out status);
                            }

                            /// TODO: Handle the negative qty sales
                            if (!invCtr.CreateStockOutQueryCommand("SYSTEM", 0, InventoryLocationID, false, true, out status, out cmd))
                                throw new Exception(status);
                        }
                        else
                        {
                            invCtr.SetInventoryHeaderInfo("", "", "Stock in for order " + OrderDetID, 0.0M, 1, 0.0M);
                            //Check Refund Ref No
                            invCtr.SetRemark("Refund");
                            Cog = ItemSummaryController.GetAvgCostPrice(ItemNoToDeduct, InventoryLocationID);
                            decimal RealQty = Quantity.GetValueOrDefault(0);

                            if (this.Item.NonInventoryProduct)
                            {
                                IDataReader reader = null;
                                reader = OrderDetUOMConversion.FetchByParameter("OrderDetID", this.OrderDetID);

                                if (reader.Read())
                                    RealQty = reader["Qty"].ToString().GetDecimalValue();
                            }


                            bool IsRefundExist = false;

                            if (RefundOrderDetID != null && RefundOrderDetID != "")
                            {
                                OrderDet od = new OrderDet(RefundOrderDetID);
                                if (od != null && od.OrderDetID == RefundOrderDetID)
                                {
                                    Cog = od.CostOfGoodSold.GetValueOrDefault(0) / od.Quantity.GetValueOrDefault(1);
                                    invCtr.SetRemark("Refund For Order " + RefundOrderDetID);
                                    IsRefundExist = true;
                                }
                                //else
                                    //Cog = ItemSummaryController.GetAvgCostPrice(ItemNoToDeduct, InventoryLocationID);
                                
                            }
                            
                            if(!IsRefundExist)
                            {

                                /*if (Cog <= 0 && AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.GetCostForStockOutfromItemSetupIfZero), true))
                                {
                                    Item theItem = new Item(ItemNo);
                                    Cog = theItem.FactoryPrice;
                                }*/
                                
                                if (theItem.IsUsingFixedCOG)
                                {
                                    decimal theAmount = Amount;
                                    if (theItem.GSTRule == 1)
                                        theAmount = theAmount - GSTAmount.GetValueOrDefault(0);

                                    if (theItem.FixedCOGType == ItemController.FIXEDCOG_PERCENTAGE)
                                    {
                                        Cog = (theAmount / Quantity.GetValueOrDefault(1)) * ((100 - theItem.FixedCOGValue) / 100);
                                    }
                                    else
                                    {
                                        Cog = theItem.FixedCOGValue;
                                    }
                                }
                                else
                                {
                                    Cog = ItemSummaryController.GetAvgCostPrice(ItemNoToDeduct, InventoryLocationID);                                    
                                }
                            }

                            invCtr.AddItemIntoInventoryForSales(this.Item.NonInventoryProduct ? this.Item.DeductedItem : ItemNo, -RealQty
                                , Cog
                                , out status);
                            cmd = invCtr.CreateStockInQueryCommand("SYSTEM", InventoryLocationID, true, true);
                        }


                        Result.AddRange(cmd);
                        #endregion

                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        decimal CostOfGoodSold = this.Item.NonInventoryProduct ? (Cog * (this.Item.DeductConvType ? (Quantity.GetValueOrDefault(0) / (this.Item.DeductConvRate == 0 ? 1 : this.Item.DeductConvRate)) : (this.Item.DeductConvRate * Quantity.GetValueOrDefault(0)))) : Cog * Quantity.GetValueOrDefault(0);
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID, OrderDetID);
                        qr.AddUpdateSetting(OrderDet.Columns.CostOfGoodSold, CostOfGoodSold);
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, invCtr.GetInvHdrRefNo());
                        Result.Add(qr.BuildUpdateCommand());
                        invHdrRef = invCtr.GetInvHdrRefNo();
                        #endregion
                    }
                }

                for (int Counter = Result.Count - 1; Counter >= 0; Counter--)
                    if (Result[Counter] == null) Result.RemoveAt(Counter);

                DataService.ExecuteTransaction(Result);
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex.Message);
            }
        }

        /// <summary>
        /// Generate Query Command to do Stock Out. Do it with caution and only after the OrderDet has been saved to DB
        /// </summary>
        /// <returns></returns>
        public void DoStockOutForVoided()
        {
            try
            {
                if (InventoryHdrRefNo == "NONINVENTORY" || InventoryHdrRefNo == "ADJUSTED")
                    return;

                string status = "";
                QueryCommandCollection Result = new QueryCommandCollection();
                string invHdrRef = "";
                #region *) Validation: No need to do anything if it is pre order
                if (IsPreOrder.Value) return;
                #endregion

                int InventoryLocationID;
                #region *) Validation: Make sure POS is attached to InventoryLocation
                InventoryLocationID = this.OrderHdr.PointOfSale.Outlet.InventoryLocationID.GetValueOrDefault(-1);
                if (InventoryLocationID == -1)
                {
                    Logger.writeLog("WARNING: POS is not attached to any InventoryLocation.");
                    throw new Exception("POS is not attached to any Inventory Location.\nPlease contact your administrator");
                }
                #endregion

                #region *) Validation: No movement is allowed when there are un-adjusted stock take in place
                /* Return true because we don't want to disturb the cashier with warning */
                if (StockTakeController.IsThereUnAdjustedStockTake(InventoryLocationID))
                {
                    Logger.writeLog("There are Stock Take in progress, no stock movement is allowed.");
                    return;
                }
                #endregion


                if (this.OrderHdr.IsVoided || (!this.Item.IsInInventory && !this.Item.NonInventoryProduct))
                {
                    #region *) Script: Mark OrderDet as NONINVENTORY
                    //if non inventory
                    Query qr = OrderDet.CreateQuery();
                    qr.AddWhere(OrderDet.Columns.OrderDetID, OrderDetID);
                    qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "NONINVENTORY");
                    qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                    Result.Add(qr.BuildCommand());
                    #endregion

                    string ItemNoToDeduct = this.Item.NonInventoryProduct ? this.Item.DeductedItem : ItemNo;

                    if (!String.IsNullOrEmpty(InventoryHdrRefNo))
                    {
                        DateTime lastStockTakeDate;
                        #region *) Fetch: Get Last Stock Take Date
                        lastStockTakeDate = StockTakeController.FetchLastStockTakeDateByInventoryLocationByItemNo(InventoryLocationID, ItemNo);
                        #endregion

                        decimal balQty;
                        //balQty = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, InventoryLocationID, DateTime.Now, out status);

                        if (OrderDetDate > lastStockTakeDate)
                        {
                            QueryCommandCollection cmd = new QueryCommandCollection();
                            bool UseOrderDate = AppSetting.CastBool(AppSetting.GetSetting("UseOrderDateForStockOut"), false);


                            #region *) Script: Insert Inventory Movement based on OrderDet
                            InventoryController invCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                            invCtr.SetInventoryHdrRefNo("VO" + OrderDetID.Replace(".", ""));
                            invCtr.setInvoiceNo(OrderDetID);
                            invCtr.SetRemark("Void");
                            if (UseOrderDate)
                                invCtr.SetInventoryDate(OrderDetDate);
                            else
                                invCtr.SetInventoryDate(DateTime.Now);
                            invCtr.SetInventoryLocation(InventoryLocationID);
                            if (Quantity < 0)
                            {
                                decimal RealQty = Quantity.GetValueOrDefault(0);
                                if (this.Item.NonInventoryProduct)
                                {
                                    IDataReader reader = null;
                                    reader = OrderDetUOMConversion.FetchByParameter("OrderDetID", this.OrderDetID);

                                    if (reader.Read())
                                        RealQty = reader["Qty"].ToString().GetDecimalValue();
                                }
                                decimal cog = (decimal)(CostOfGoodSold / RealQty);
                                invCtr.AddItemIntoInventoryForSales(ItemNoToDeduct, -RealQty, Math.Abs(cog), out status);

                                /// TODO: Handle the negative qty sales
                                if (!invCtr.CreateStockOutQueryCommand("SYSTEM", 0, InventoryLocationID, false, true, out status, out cmd))
                                    throw new Exception(status);
                            }
                            else if (Quantity > 0)
                            {
                                invCtr.SetInventoryHeaderInfo("", "", "Stock in for order " + OrderDetID, 0.0M, 1, 0.0M);
                                decimal RealQty = Quantity.GetValueOrDefault(0);
                                if (this.Item.NonInventoryProduct)
                                {
                                    IDataReader reader = null;
                                    reader = OrderDetUOMConversion.FetchByParameter("OrderDetID", this.OrderDetID);

                                    if (reader.Read())
                                        RealQty = reader["Qty"].ToString().GetDecimalValue();
                                }
                                decimal cog = (decimal)(CostOfGoodSold / RealQty);

                                /*invCtr.AddItemIntoInventory(ItemNo, -Quantity
                                    , InventoryController.FetchAverageCostOfGoodsLeftByItemNo(balQty, ItemNo, InventoryLocationID)
                                    , out status);*/
                                invCtr.AddItemIntoInventoryForSales(ItemNoToDeduct, RealQty
                                    , Math.Abs(cog)
                                    , out status);
                                invCtr.SetInventoryHdrUserName("SYSTEM");
                                cmd = invCtr.CreateStockInQueryCommand("SYSTEM", InventoryLocationID, true, false);
                            }

                            Result.AddRange(cmd);
                            #endregion

                            invHdrRef = invCtr.GetInvHdrRefNo();

                        }
                    }

                }
                else
                {
                    //Find the last stock take date for the location
                    DateTime lastStockTakeDate;
                    #region *) Fetch: Get Last Stock Take Date
                    lastStockTakeDate = StockTakeController.FetchLastStockTakeDateByInventoryLocationByItemNo(InventoryLocationID, ItemNo);
                    #endregion

                    decimal balQty;
                    //balQty = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, InventoryLocationID, DateTime.Now, out status);

                    if (OrderDetDate < lastStockTakeDate)
                    {
                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID, OrderDetID);
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                        Result.Add(qr.BuildUpdateCommand());
                        #endregion
                    }
                    else
                    {
                        QueryCommandCollection cmd = new QueryCommandCollection();
                        bool UseOrderDate = AppSetting.CastBool(AppSetting.GetSetting("UseOrderDateForStockOut"), false);


                        #region *) Script: Insert Inventory Movement based on OrderDet
                        InventoryController invCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        invCtr.SetInventoryHdrRefNo(OrderDetID.Replace(".", ""));
                        invCtr.setInvoiceNo(OrderDetID);
                        if (UseOrderDate)
                            invCtr.SetInventoryDate(OrderDetDate);
                        else
                            invCtr.SetInventoryDate(DateTime.Now);
                        invCtr.SetInventoryLocation(InventoryLocationID);
                        if (Quantity >= 0)
                        {
                            invCtr.AddItemIntoInventoryForSales(ItemNo, Quantity.GetValueOrDefault(0), InventoryController.FetchCostOfGoodsByItemNoForSales(ItemNo, InventoryLocationID), out status);

                            /// TODO: Handle the negative qty sales
                            if (!invCtr.CreateStockOutQueryCommand("SYSTEM", 0, InventoryLocationID, false, true, out status, out cmd))
                                throw new Exception(status);
                        }
                        else
                        {
                            invCtr.SetInventoryHeaderInfo("", "", "Stock in for order " + OrderDetID, 0.0M, 1, 0.0M);

                            /*invCtr.AddItemIntoInventory(ItemNo, -Quantity
                                , InventoryController.FetchAverageCostOfGoodsLeftByItemNo(balQty, ItemNo, InventoryLocationID)
                                , out status);*/
                            invCtr.AddItemIntoInventoryForSales(ItemNo, -Quantity.GetValueOrDefault(0)
                                , InventoryController.FetchCostOfGoodsByItemNoForSales(ItemNo, InventoryLocationID)
                                , out status);
                            cmd = invCtr.CreateStockInQueryCommand("SYSTEM", InventoryLocationID, true, true);
                        }

                        Result.AddRange(cmd);
                        #endregion

                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID, OrderDetID);
                        qr.AddUpdateSetting(OrderDet.Columns.CostOfGoodSold, InventoryController.FetchCostOfGoodsByItemNoForSales(ItemNo, InventoryLocationID) * Quantity);
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, invCtr.GetInvHdrRefNo());
                        Result.Add(qr.BuildUpdateCommand());
                        invHdrRef = invCtr.GetInvHdrRefNo();
                        #endregion
                    }
                }

                for (int Counter = Result.Count - 1; Counter >= 0; Counter--)
                    if (Result[Counter] == null) Result.RemoveAt(Counter);

                DataService.ExecuteTransaction(Result);
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
        }

    }
}
