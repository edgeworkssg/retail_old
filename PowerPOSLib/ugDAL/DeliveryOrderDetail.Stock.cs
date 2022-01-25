using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SubSonic;

namespace PowerPOS
{
    public partial class DeliveryOrderDetail
    {
        /// <summary>
        /// Generate Query Command to do Stock Out. Do it with caution and only after the DeliveryOrderDetail has been saved to DB
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
                OrderDet orderDet = new OrderDet(this.OrderDetID);

                #region *) Validation: If OrderDet not found then exit
                if (orderDet == null || orderDet.OrderDetID != this.OrderDetID)
                    return;
                #endregion

                int InventoryLocationID;
                #region *) Validation: Make sure POS is attached to InventoryLocation

                InventoryLocationID = orderDet.OrderHdr.PointOfSale.Outlet.InventoryLocationID.GetValueOrDefault(-1);
                if (orderDet.IsPreOrder.GetValueOrDefault(false) && orderDet.PreOrderPointOfSalesID != 0)
                {
                    PointOfSale p = new PointOfSale(orderDet.PreOrderPointOfSalesID);
                    InventoryLocationID = p.Outlet.InventoryLocationID.GetValueOrDefault(-1);
                }
               
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


                if (!orderDet.IsPreOrder.GetValueOrDefault(false))
                {
                    #region *) Script: Update the DeliveryOrderDetail.InventoryHdrRefNo
                    Query qr = new Query("DeliveryOrderDetails");
                    qr.QueryType = QueryType.Update;
                    qr.AddWhere(DeliveryOrderDetail.Columns.DetailsID, this.DetailsID);
                    qr.AddUpdateSetting(DeliveryOrderDetail.Columns.InventoryHdrRefNo, "NONPREORDER");
                    qr.AddUpdateSetting(DeliveryOrderDetail.Columns.ModifiedOn, DateTime.Now);
                    Result.Add(qr.BuildUpdateCommand());
                    #endregion
                }
                else if (orderDet.OrderHdr.IsVoided || !orderDet.Item.IsInInventory)
                {
                    #region *) Script: Mark OrderDet as NONINVENTORY
                    //if non inventory
                    Query qr = OrderDet.CreateQuery();
                    qr.AddWhere(OrderDet.Columns.OrderDetID, this.OrderDetID);
                    qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "NONINVENTORY");
                    qr.AddUpdateSetting(OrderDet.Columns.Userflag1, true);
                    Result.Add(qr.BuildCommand());
                    #endregion

                    #region *) Script: Mark DeliveryOrderDetail as NONINVENTORY
                    //if non inventory
                    qr = DeliveryOrderDetail.CreateQuery();
                    qr.AddWhere(DeliveryOrderDetail.Columns.DetailsID, this.DetailsID);
                    qr.AddUpdateSetting(DeliveryOrderDetail.Columns.InventoryHdrRefNo, "NONINVENTORY");
                    qr.AddUpdateSetting(DeliveryOrderDetail.Columns.ModifiedOn, DateTime.Now);
                    Result.Add(qr.BuildCommand());
                    #endregion
                }
                else
                {
                    ////Find the last stock take date for the location
                    //DateTime lastStockTakeDate;
                    //#region *) Fetch: Get Last Stock Take Date
                    //lastStockTakeDate = StockTakeController.FetchLastStockTakeDateByInventoryLocationByItemNo(InventoryLocationID, ItemNo);
                    //#endregion

                    //decimal balQty;
                    //balQty = InventoryController.GetStockBalanceQtyByItemByDate(ItemNo, InventoryLocationID, DateTime.Now, out status);

                    //if (orderDet.OrderDetDate < lastStockTakeDate)
                    //{
                    //    #region *) Script: Update the OrderDet.InventoryHdrRefNo
                    //    Query qr = new Query("OrderDet");
                    //    qr.QueryType = QueryType.Update;
                    //    qr.AddWhere(OrderDet.Columns.OrderDetID, this.OrderDetID);
                    //    qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, "ADJUSTED");
                    //    Result.Add(qr.BuildUpdateCommand());
                    //    #endregion

                    //    #region *) Script: Update the DeliveryOrderDetail.InventoryHdrRefNo
                    //    qr = new Query("DeliveryOrderDetails");
                    //    qr.QueryType = QueryType.Update;
                    //    qr.AddWhere(DeliveryOrderDetail.Columns.DetailsID, this.DetailsID);
                    //    qr.AddUpdateSetting(DeliveryOrderDetail.Columns.InventoryHdrRefNo, "ADJUSTED");
                    //    qr.AddUpdateSetting(DeliveryOrderDetail.Columns.ModifiedOn, DateTime.Now);
                    //    Result.Add(qr.BuildUpdateCommand());
                    //    #endregion
                    //}
                    //else
                    {
                        QueryCommandCollection cmd = new QueryCommandCollection();
                        bool UseOrderDate = AppSetting.CastBool(AppSetting.GetSetting("UseOrderDateForStockOut"), false);


                        #region *) Script: Insert Inventory Movement based on DeliveryOrderDetail
                        InventoryController invCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                        if (this.Quantity.GetValueOrDefault(0) >= 0)
                        {
                            invCtr.SetInventoryHdrRefNo("DO" + this.DetailsID.Replace(".", ""));
                        }
                        else {
                            invCtr.SetInventoryHdrRefNo("INDO" + this.DetailsID.Replace(".", ""));
                        }
                        invCtr.setInvoiceNo(this.OrderDetID);
                        if (UseOrderDate)
                            invCtr.SetInventoryDate(orderDet.OrderDetDate);
                        else
                            invCtr.SetInventoryDate(DateTime.Now);
                        invCtr.SetInventoryLocation(InventoryLocationID);
                        if (this.Quantity.GetValueOrDefault(0) >= 0)
                        {
                            invCtr.AddItemIntoInventoryForSales(this.ItemNo, (int)this.Quantity.GetValueOrDefault(0), InventoryController.FetchCostOfGoodsByItemNoForSales(this.ItemNo, InventoryLocationID), out status);

                            /// TODO: Handle the negative qty sales
                            if (!invCtr.CreateStockOutQueryCommand("SYSTEM", 0, InventoryLocationID, false, true, out status, out cmd))
                                throw new Exception(status);
                        }
                        
                        Result.AddRange(cmd);
                        #endregion

                        invHdrRef = invCtr.GetInvHdrRefNo();

                        #region *) Script: Update the OrderDet.InventoryHdrRefNo
                        Query qr = new Query("OrderDet");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(OrderDet.Columns.OrderDetID, this.OrderDetID);
                        qr.AddUpdateSetting(OrderDet.Columns.CostOfGoodSold, InventoryController.FetchCostOfGoodsByItemNoForSales(this.ItemNo, InventoryLocationID) * this.Quantity);
                        qr.AddUpdateSetting(OrderDet.Columns.InventoryHdrRefNo, invHdrRef);
                        Result.Add(qr.BuildUpdateCommand());
                        #endregion

                        #region *) Script: Update the DeliveryOrderDetail.InventoryHdrRefNo
                        qr = new Query("DeliveryOrderDetails");
                        qr.QueryType = QueryType.Update;
                        qr.AddWhere(DeliveryOrderDetail.Columns.DetailsID, this.DetailsID);
                        qr.AddUpdateSetting(DeliveryOrderDetail.Columns.InventoryHdrRefNo, invHdrRef);
                        qr.AddUpdateSetting(DeliveryOrderDetail.Columns.ModifiedOn, DateTime.Now);
                        Result.Add(qr.BuildUpdateCommand());
                        #endregion
                    }
                }

                for (int Counter = Result.Count - 1; Counter >= 0; Counter--)
                    if (Result[Counter] == null) Result.RemoveAt(Counter);

                DataService.ExecuteTransaction(Result);
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
        }

        public void DoAdjustmentIn()
        {
            try
            {
                string status = "";
                QueryCommandCollection Result = new QueryCommandCollection();
                string invHdrRef = "";
                OrderDet orderDet = new OrderDet(this.OrderDetID);

                #region *) Validation: If OrderDet not found then exit
                if (orderDet == null || orderDet.OrderDetID != this.OrderDetID)
                    return;
                #endregion

                #region *) check if already refund
                InventoryHdrCollection hdr = new InventoryHdrCollection();
                Query qHdr = InventoryHdr.CreateQuery();
                qHdr.AddWhere(InventoryHdr.Columns.InvoiceNo, orderDet.OrderDetID);
                qHdr.AddWhere(InventoryHdr.Columns.MovementType, "Adjustment In");

                hdr.LoadAndCloseReader(qHdr.ExecuteReader());

                if (hdr.Count > 0)
                    return;
                #endregion

                int InventoryLocationID;
                #region *) Validation: Make sure POS is attached to InventoryLocation

                InventoryLocationID = orderDet.OrderHdr.PointOfSale.Outlet.InventoryLocationID.GetValueOrDefault(-1);
                if (orderDet.IsPreOrder.GetValueOrDefault(false) && orderDet.PreOrderPointOfSalesID != 0)
                {
                    PointOfSale p = new PointOfSale(orderDet.PreOrderPointOfSalesID);
                    InventoryLocationID = p.Outlet.InventoryLocationID.GetValueOrDefault(-1);
                }
               
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


                if (orderDet.IsPreOrder.GetValueOrDefault(false) && orderDet.Quantity < 0)
                {
                    QueryCommandCollection cmd = new QueryCommandCollection();
                    
                    #region *) Script: Insert Inventory Movement based on DeliveryOrderDetail
                    InventoryController invCtr = new InventoryController(PowerPOS.Container.InventorySettings.CostingMethod);
                    invCtr.SetInventoryHdrRefNo("IN_DO" + this.DetailsID.Replace(".", ""));
                    invCtr.setInvoiceNo(this.OrderDetID);
                    invCtr.SetInventoryDate(DateTime.Now);
                    invCtr.SetInventoryLocation(InventoryLocationID);
                    if (this.Quantity.GetValueOrDefault(0) < 0)
                    
                    invCtr.AddItemIntoInventoryForSales(this.ItemNo, Math.Abs(this.Quantity.GetValueOrDefault(0)), InventoryController.FetchCostOfGoodsByItemNoForSales(this.ItemNo, InventoryLocationID), out status);

                    /// TODO: Handle the negative qty sales
                    cmd = invCtr.CreateStockInQueryCommand("SYSTEM", InventoryLocationID, true, true);                               
                    

                    Result.AddRange(cmd);
                    #endregion

                    invHdrRef = invCtr.GetInvHdrRefNo();
                    
                }

                for (int Counter = Result.Count - 1; Counter >= 0; Counter--)
                    if (Result[Counter] == null) Result.RemoveAt(Counter);

                DataService.ExecuteTransaction(Result);
            }
            catch (Exception ex) { Logger.writeLog(ex.Message); }
        }        
    }
}
