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
        #region "Stock In, Stock Out"
        private double GetGST()
        {
            try
            {
                //Load GST from GST Table
                Query qry = new Query("GST");
                Where whr = new Where();
                whr.ColumnName = PowerPOS.Gst.Columns.CommenceDate;
                whr.Comparison = Comparison.LessOrEquals;
                whr.ParameterName = "@CommenceDate";
                whr.ParameterValue = DateTime.Now.ToString("dd MMM yyyy");
                whr.TableName = "GST";
                object obj = qry.GetMax(PowerPOS.Gst.Columns.GSTRate, whr);
                return (double.Parse(obj.ToString()));
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return -1.0;
            }
        }

        #region -= Stock In =-
        public bool StockIn(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, out string status)
        {
            try
            {
                SupplierItemMap.CreateSupplierItemMapTable();
                List<string> ListInventoryHdr = new List<string>();

                QueryCommandCollection cmd = CreateStockInQueryCommand(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
                //if (!string.IsNullOrEmpty(InvHdr.PurchaseOrderNo))
                //{
                //    PurchaseOrderHdr po = new PurchaseOrderHdr(InvHdr.PurchaseOrderNo);
                //    if (!po.IsNew)
                //    {
                //        po.Userfld7 = "Posted";
                //        cmd.Add(po.GetUpdateCommand(username));
                //    }
                //}                    

                SubSonic.DataService.ExecuteTransaction(cmd);

                Logger.writeLog("Stock In completed. InventoryHdrRefNo : " + InvHdr.InventoryHdrRefNo);

                if (!string.IsNullOrEmpty(InvHdr.PurchaseOrderNo))
                {
                    Logger.writeLog("Update Purchase Order Status for : " + InvHdr.PurchaseOrderNo);
                    PurchaseOdrController.UpdatePurchaseOrderStatus(InvHdr.PurchaseOrderNo);
                }

                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo), false)
                    && InvHdr.MovementType == InventoryController.InventoryMovementType_StockIn)
                    InventoryController.CustomRefNoUpdate();

                InvHdr.IsNew = false;
                status = "";

                // check if there are any stock take after the stock in 
                string itemlist = "";
                if (gotStockTakeAfter(out itemlist))
                {
                    StockOutAdjustment(username, 1, InventoryLocationID, IsAdjustment, true, itemlist, out status);
                }

                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");
                return false;
            }
        }

        public QueryCommandCollection CreateStockInQueryCommand(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
            if (ActiveCostingType == CostingTypes.FIFO)
                return StockIn_FIFO(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
            else if (ActiveCostingType == CostingTypes.FixedAvg)
                return StockIn_FixedAvg(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
            else if (ActiveCostingType == CostingTypes.WeightedAvg)
                return StockIn_WeightedAvg(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
            else
            {
                Logger.writeLog("ActiveCostingType " + ActiveCostingType + " is not recognized. Will use FIFO");
                return StockIn_FIFO(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
            }
        }
        private QueryCommandCollection StockIn_FIFO(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null) //Valid Order?
                throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
            #endregion

            #region *) Validation: All quantity must be bigger than 0
            for (int i = 0; i < InvDet.Count; i++)
                if (InvDet[i].Quantity <= 0)
                    throw new Exception("(error)Error: Quantity must be larger than zero.");
            #endregion

            #region *) Conditioning: Set header information
            if (InvHdr.UserName == null) InvHdr.UserName = username;
            if (InvHdr.InventoryHdrRefNo == null || !InvHdr.InventoryHdrRefNo.StartsWith("ST"))
            {
                if (InvHdr.InventoryHdrRefNo == null || InvHdr.UserName.ToLower() != "system")
                    InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
            }

            InvHdr.UserName = username;
            InvHdr.InventoryLocationID = InventoryLocationID;

            if (!InvHdr.FreightCharge.HasValue) InvHdr.FreightCharge = 0;

            if (IsAdjustment)
            {
                InvHdr.MovementType = InventoryController.InventoryMovementType_AdjustmentIn;
            }
            else
            {
                InvHdr.MovementType = InventoryController.InventoryMovementType_StockIn;
            }

            if (string.IsNullOrEmpty(InvHdr.CustomRefNo))
            {
                if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.UseCustomNo), false)
                    && InvHdr.MovementType == InventoryController.InventoryMovementType_StockIn)
                {
                    InvHdr.CustomRefNo = CreateNewCustomRefNo();
                }
                else
                {
                    InvHdr.CustomRefNo = InvHdr.InventoryHdrRefNo;
                }
            }
            #endregion

            #region *) Validation: check if record already exists
            if (IsRecordExisted(InvHdr.MovementType))
                throw new Exception("(error) Record already exists.");
            #endregion

            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;
            #region *) Save: Generate Save Script for InventoryHdr
            if (InvHdr.IsNew)
            {
                InvHdr.Discount = InvHdr.Discount * (decimal)InvHdr.ExchangeRate;
                mycmd = InvHdr.GetInsertCommand(UserInfo.username);
            }
            else
            {
                mycmd = InvHdr.GetUpdateCommand(UserInfo.username);
            }
            cmd.Add(mycmd);
            #endregion

            //decimal SumOfFactoryPriceAndQty = 0;
            //#region *) Calculate: calculate sum of factory price * quantity for distributing Freight charges
            //for (int i = 0; i < InvDet.Count; i++)
            //    SumOfFactoryPriceAndQty += (decimal)InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0);

            //if (SumOfFactoryPriceAndQty == 0)
            //    SumOfFactoryPriceAndQty = 0.0001M;
            //#endregion

            //decimal totalCost = 0;
            //#region *) Calculate total additional cost
            //totalCost += InvHdr.FreightCharge.GetValueOrDefault(0);
            //if (InvHdr.AdditionalCost1 != 0)
            //{
            //    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost1_IsPercentage), false))
            //        totalCost += InvHdr.AdditionalCost1 / 100 * SumOfFactoryPriceAndQty;
            //    else
            //        totalCost += InvHdr.AdditionalCost1;
            //}
            //if (InvHdr.AdditionalCost2 != 0)
            //{
            //    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost2_IsPercentage), false))
            //        totalCost += InvHdr.AdditionalCost2 / 100 * SumOfFactoryPriceAndQty;
            //    else
            //        totalCost += InvHdr.AdditionalCost2;
            //}
            //if (InvHdr.AdditionalCost3 != 0)
            //{
            //    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost3_IsPercentage), false))
            //        totalCost += InvHdr.AdditionalCost3 / 100 * SumOfFactoryPriceAndQty;
            //    else
            //        totalCost += InvHdr.AdditionalCost3;
            //}
            //if (InvHdr.AdditionalCost4 != 0)
            //{
            //    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost4_IsPercentage), false))
            //        totalCost += InvHdr.AdditionalCost4 / 100 * SumOfFactoryPriceAndQty;
            //    else
            //        totalCost += InvHdr.AdditionalCost4;
            //}
            //if (InvHdr.AdditionalCost5 != 0)
            //{
            //    if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsReceive.AdditionalCost5_IsPercentage), false))
            //        totalCost += InvHdr.AdditionalCost5 / 100 * SumOfFactoryPriceAndQty;
            //    else
            //        totalCost += InvHdr.AdditionalCost5;
            //}
            //#endregion

            //Inventory Detail - insert into Transaction
            for (int i = 0; i < InvDet.Count; i++)
            {
                #region *) Conditioning: Set detail information
                //decimal weight = (InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0)) / SumOfFactoryPriceAndQty;

                InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                InvDet[i].RemainingQty = InvDet[i].Quantity.GetValueOrDefault(0);

                //int GetBalBeforeQty = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, DateTime.Now, out status);
                //InvDet[i].BalanceBefore = GetBalBeforeQty;
                //InvDet[i].BalanceAfter = GetBalBeforeQty + InvDet[i].Quantity;

                //if (CalculateCOGS && InvDet[i].Quantity > 0)
                //{
                //    InvDet[i].InitialFactoryPrice = InvDet[i].FactoryPrice;

                //    if (InvHdr.ExchangeRate > 0)
                //        InvDet[i].FactoryPrice = InvDet[i].FactoryPrice * (decimal)InvHdr.ExchangeRate;

                //    InvDet[i].FactoryPrice =
                //        InvDet[i].FactoryPrice - ((weight * InvHdr.Discount.Value) / InvDet[i].Quantity.GetValueOrDefault(0));

                //    // Include additional cost into the FactoryPrice so it will be included in Item Summary calculation
                //    InvDet[i].FactoryPrice = InvDet[i].FactoryPrice
                //        + (decimal)((weight * totalCost) / InvDet[i].Quantity);

                //    InvDet[i].CostOfGoods = InvDet[i].FactoryPrice;
                //}

                if (IsAdjustment)
                {
                    InvDet[i].FactoryPrice = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                    InvDet[i].CostOfGoods = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                }

                InvDet[i].Gst = GetGST();
                #endregion

                #region *) Save: Generate Save Script for InventoryDet
                if (InvDet[i].IsNew)
                {
                    mycmd = InvDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                else
                {
                    mycmd = InvDet[i].GetUpdateCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                #endregion

                #region *) Insert (If Not Exists) SupplierItemMap for this SupplierID and ItemNo

                if (IsAdjustment == false && InvHdr.Supplier != string.Empty) // only if Supplier is not string.empty (--select supplier-- means string empty)
                {
                    SupplierItemMap supItemMap = new SupplierItemMap();
                    supItemMap.SupplierID = int.Parse(InvHdr.Supplier);
                    supItemMap.ItemNo = InvDet[i].ItemNo;
                    supItemMap.UniqueID = Guid.NewGuid();
                    supItemMap.Deleted = false;

                    cmd.Add(supItemMap.GetInsertIfNotExistsCommand());
                }

                #endregion

                #region *) Update QtyReceived in PO
                if (!string.IsNullOrEmpty(InvDet[i].PurchaseOrderDetRefNo))
                {
                    PurchaseOrderDet poDet = new PurchaseOrderDet(InvDet[i].PurchaseOrderDetRefNo);
                    if (poDet != null && poDet.PurchaseOrderDetRefNo == InvDet[i].PurchaseOrderDetRefNo)
                    {
                        poDet.QtyReceived = poDet.QtyReceived.GetValueOrDefault(0) + InvDet[i].Quantity.GetValueOrDefault(0);
                        cmd.Add(poDet.GetUpdateCommand(UserInfo.username));
                    }
                }
                #endregion
            }

            #region *) Update Item Summary

            foreach (var id in InvDet)
            {
                var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                    InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                    id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                    id.InventoryDetRefNo, InvHdr.InventoryDate);
                cmd.AddRange(itemSummaryCmd);
            }

            #endregion

            #region *) Update Item Tag Summary

            var cmdItemTag = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
            if (cmdItemTag.Count > 0)
                cmd.AddRange(cmdItemTag);

            #endregion

            #region *) Update Retail Price
            if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowToUpdateRetailPriceInGoodsReceive), false))
            {
                foreach (var id in InvDet)
                {
                    if (!id.RetailPrice.HasValue) continue;

                    Item it = new Item(id.ItemNo);
                    it.RetailPrice = id.RetailPrice.Value;
                    cmd.Add(it.GetUpdateCommand(username));
                }
            }
            #endregion

            return cmd;
        }
        private QueryCommandCollection StockIn_FixedAvg(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
            /* We think that this function actually the same with the FIFO
             * Both just doing direct insert into the Database, CMIIW */
            return StockIn_FIFO(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
        }
        private QueryCommandCollection StockIn_WeightedAvg(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
            /* We think that this function actually the same with the FIFO
             * Both just doing direct insert into the Database, CMIIW */
            return StockIn_FIFO(username, InventoryLocationID, IsAdjustment, CalculateCOGS);
        }

        private double GST;

        private bool LoadGST()
        {
            //Load GST from GST Table
            Query qry = new Query("GST");
            Where whr = new Where();
            whr.ColumnName = PowerPOS.Gst.Columns.CommenceDate;
            whr.Comparison = Comparison.LessOrEquals;
            whr.ParameterName = "@CommenceDate";
            whr.ParameterValue = DateTime.Now.ToString("yyyy-MM-dd");
            whr.TableName = "GST";
            //pull out from GST table
            object obj = qry.GetMax(PowerPOS.Gst.Columns.GSTRate, whr);
            GST = (double.Parse(obj.ToString()));
            return true;
        }

        public bool SetGST(int GSTRule, bool isGoodReceive, out string status)
        {
            status = "";
            decimal TotalGSTAmount = 0;
            try
            {
                InvHdr.GSTRule = GSTRule;
                LoadGST();
                decimal gstPercentage = (decimal)GST / 100;
               
                for (int i = 0; i < InvDet.Count; i++)
                {
                    decimal gstAmount = 0;
                    decimal subtotal = 0;
                    if(isGoodReceive)
                        subtotal = InvDet[i].TotalCost.GetValueOrDefault(0);
                    else
                        subtotal = (decimal)InvDet[i].Quantity * InvDet[i].InitialFactoryPrice.GetValueOrDefault(0);

                    if (GSTRule == 1)
                    {
                        //Exclusive
                        gstAmount = subtotal * gstPercentage;

                        if (AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.IncludeGSTExclusive), false))
                        {
                            InvDet[i].TotalCost += gstAmount;
                            InvDet[i].FactoryPrice = InvDet[i].TotalCost.GetValueOrDefault(0) / InvDet[i].Quantity.GetValueOrDefault(1);
                            InvDet[i].CostOfGoods = InvDet[i].FactoryPrice;
                        }
                        TotalGSTAmount += gstAmount;
                    }
                    else if (GSTRule == 2)
                    {
                        //inclusive

                        gstAmount = subtotal * gstPercentage / (1 + gstPercentage);
                        TotalGSTAmount += gstAmount;
                    }
                    else
                    {
                        gstAmount = 0;
                    }

                    InvDet[i].GSTAmount = (double)gstAmount;
                }

                InvHdr.Tax = TotalGSTAmount;

                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");
                return false;
            }
        }
        #endregion

        #region -= Stock Out =-
        public bool StockOut(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string status)
        {
            InventoryDetCollection tmpDet = InvDet;
            try
            {
                QueryCommandCollection cmd;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                InventoryDetCollection mergedInvDet = new InventoryDetCollection();
                if (!IsQuantitySufficient(InventoryLocationID, out mergedInvDet, out status))
                {
                    return false;
                }
                else
                {
                    InvDet = mergedInvDet;

                    if (CreateStockOutQueryCommand(username, StockOutReasonID, InventoryLocationID,
                        IsAdjustment, deductRemainingQty, out status, out cmd))
                    {
                        SubSonic.DataService.ExecuteTransaction(cmd);

                        // check if there are any stock take after the stock out 
                        string itemlist = "";
                        if (gotStockTakeAfter(out itemlist))
                        {
                            StockInAdjustment(username, InventoryLocationID, IsAdjustment, true, itemlist, out status);
                        }

                        InvHdr.IsNew = false;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;
                return false;
            }
        }

        public bool CreateStockOutQueryCommand
            (string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string status, out QueryCommandCollection cmd)
        {
            try
            {
                status = "";

                if (ActiveCostingType == CostingTypes.FIFO)
                    cmd = StockOut_FixedAvg(username, StockOutReasonID, InventoryLocationID, IsAdjustment);
                else if (ActiveCostingType == CostingTypes.FixedAvg)
                    cmd = StockOut_FixedAvg(username, StockOutReasonID, InventoryLocationID, IsAdjustment);
                else if (ActiveCostingType == CostingTypes.WeightedAvg)
                    cmd = StockOut_FixedAvg(username, StockOutReasonID, InventoryLocationID, IsAdjustment);
                else
                {
                    Logger.writeLog("ActiveCostingType " + ActiveCostingType + " is not recognized. Will use Fixed Avg");
                    cmd = StockOut_FixedAvg(username, StockOutReasonID, InventoryLocationID, IsAdjustment);
                }

                return true;
            }
            catch (Exception X)
            {
                status = X.Message.Replace("(warning)", "").Replace("(error)", "");
                cmd = new QueryCommandCollection();
                return false;
            }
        }
        public bool StockOut_FIFO
            (string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, out string status, out QueryCommandCollection cmd)
        {
            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null) //Valid Order?
                throw new Exception("(error)Invalid Inventory data. Unable to stock out Inventory.");
            #endregion

            InventoryDetCollection mergedInvDet = new InventoryDetCollection();
            #region *) Conditioning: Merge all InventoryDet with same name
            MergeInventoryDet(ref mergedInvDet);
            InvDet = mergedInvDet;
            #endregion

            cmd = new QueryCommandCollection();
            QueryCommand mycmd;

            #region *) validation quantity < 0
            for (int i = 0; i < InvDet.Count; i++)
            {
                if (InvDet[i].Quantity < 0)
                {
                    throw new Exception("Error Quantity "+ InvDet[i].ItemNo +" must be larger than zero");
                }
            }
            #endregion

            #region *) Save: Generate Save Script for InventoryHdr
            if (string.IsNullOrEmpty(InvHdr.CustomRefNo))
                InvHdr.CustomRefNo = InvHdr.InventoryHdrRefNo;

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

            mycmd = InvHdr.GetInsertCommand(UserInfo.username);
            cmd.Add(mycmd);
            #endregion

            //fetch the FIFO information from corresponding Stock In Inventory Details
            //while (i < InvDet.Count) //For every inventory Details....
            int index = 0;
            for (int i = 0; i < InvDet.Count; i++)
            {
                #region *) Save: Generate Save Script for InventoryDet
                InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();

                //InvDet[i].BalanceBefore = GetStockBalanceQtyByItemByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.Value, DateTime.Now, out status);


                if (deductRemainingQty)
                {
                    if (InvDet[i].Quantity > 0)
                    {
                        DistributeInventoryDetQuantity(ref cmd, InvDet[i], ref index, out status);
                        if (status != "")
                        {
                            Logger.writeLog("Error distribute inventory quantity for Item: " + InvDet[i].ItemNo);
                            return false;
                            //i += 1;
                        }
                    }
                    else
                    {
                        i += 1;
                    }
                }
                else
                {
                    cmd.Add(InvDet[i].GetInsertCommand(username));
                    i += 1;
                }
                #endregion
            }

            #region *) Update Item Summary

            foreach (var id in InvDet)
            {
                var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                    InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                    id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                    id.InventoryDetRefNo, InvHdr.InventoryDate);
                cmd.AddRange(itemSummaryCmd);
            }

            #endregion

            #region *) Update Item Tag Summary

            var cmdItemTag = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
            if (cmdItemTag.Count > 0)
                cmd.AddRange(cmdItemTag);

            #endregion

            status = "";
            return true;
        }

        public QueryCommandCollection StockOut_FixedAvg
            (string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment)
        {
            string status = "";

            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null)
                throw new Exception("(error) Invalid Inventory data. Unable to stock out Inventory.");
            #endregion

            InventoryDetCollection mergedInvDet = new InventoryDetCollection();
            #region *) Conditioning: Merge all InventoryDet with same name
            MergeInventoryDet(ref mergedInvDet);
            InvDet = mergedInvDet;
            #endregion

            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;

            #region *) validation quantity < 0
            for (int i = 0; i < InvDet.Count; i++)
            {
                if (InvDet[i].Quantity < 0)
                {
                    throw new Exception("Error Quantity " + InvDet[i].ItemNo + " must be larger than zero");
                }
            }
            #endregion

            #region *) Save: Generate Save Script for InventoryHdr
            if (string.IsNullOrEmpty(InvHdr.CustomRefNo))
                InvHdr.CustomRefNo = InvHdr.InventoryHdrRefNo;

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

            mycmd = InvHdr.GetInsertCommand(UserInfo.username);
            cmd.Add(mycmd);
            #endregion

            #region *) Validation: check if record already exists
            if (IsRecordExisted(InvHdr.MovementType))
                throw new Exception("(error) Record already exists.");
            #endregion

            for (int i = 0; i < InvDet.Count; i++)
            {
                if (IsAdjustment)
                {
                    InvDet[i].FactoryPrice = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                    //InvDet[i].CostOfGoods = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                }

                #region *) Save: Generate Save Script for InventoryDet
                InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                InvDet[i].CostOfGoods = InvDet[i].Quantity.GetValueOrDefault(0) * InvDet[i].FactoryPrice;

                cmd.Add(InvDet[i].GetInsertCommand(username));
                #endregion
            }

            #region *) Update Item Summary

            foreach (var id in InvDet)
            {
                var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                    InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                    id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                    id.InventoryDetRefNo, InvHdr.InventoryDate);
                cmd.AddRange(itemSummaryCmd);
            }

            #endregion

            #region *) Update Item Tag Summary

            var cmdItemTag = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
            if (cmdItemTag.Count > 0)
                cmd.AddRange(cmdItemTag);

            #endregion

            return cmd;
        }

        public bool StockOutAdjustment(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, string itemList, out string status)
        {
            InventoryDetCollection tmpDet = InvDet;
            try
            {
                QueryCommandCollection cmd;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                //String[] tmpItemNoList = itemList.Split(',');

                bool isFound = false;
                ArrayList inventoryDetRefNoList = new ArrayList();

                foreach (InventoryDet id in InvDet)
                {
                    //Logger.writeLog("Check InventoryDetRefNo : " + id.InventoryDetRefNo + "," + id.ItemNo);
                    if (itemList.Contains(id.ItemNo))
                    {
                        isFound = true;
                    }
                    if (isFound)
                    {
                        inventoryDetRefNoList.Add(id.InventoryDetRefNo);
                        //Logger.writeLog("Remove InventoryDetRefNo : " + id.InventoryDetRefNo);
                    }

                }
                foreach (string tmp in inventoryDetRefNoList)
                {
                    InventoryDet id = (InventoryDet)InvDet.Find(tmp);
                    if (id != null)
                        InvDet.Remove(id);
                }

                if (InvHdr.MovementType == InventoryController.InventoryMovementType_TransferIn)
                {
                    InvHdr.Remark = "Transfer In Before Stock Take";
                }
                else
                {
                    if (IsAdjustment)
                        InvHdr.Remark = "Adjustment In Before Stock Take";
                    else
                        InvHdr.Remark = "Goods Receive Before Stock Take";
                }

                if (CreateStockOutQueryCommand(username, StockOutReasonID, InventoryLocationID,
                    true, deductRemainingQty, out status, out cmd))
                {
                    SubSonic.DataService.ExecuteTransaction(cmd);
                    InvHdr.IsNew = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;
                return false;
            }
        }

        public bool StockOutAdjustmentCommand(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool deductRemainingQty, string itemList, out QueryCommandCollection cmd, out string status)
        {
            InventoryDetCollection tmpDet = InvDet;
            cmd = new QueryCommandCollection();
            try
            {
                
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                //String[] tmpItemNoList = itemList.Split(',');

                bool isFound = false;
                ArrayList inventoryDetRefNoList = new ArrayList();

                foreach (InventoryDet id in InvDet)
                {
                    //Logger.writeLog("Check InventoryDetRefNo : " + id.InventoryDetRefNo + "," + id.ItemNo);
                    if (itemList.Contains(id.ItemNo))
                    {
                        isFound = true;
                    }
                    if (isFound)
                    {
                        inventoryDetRefNoList.Add(id.InventoryDetRefNo);
                        //Logger.writeLog("Remove InventoryDetRefNo : " + id.InventoryDetRefNo);
                    }

                }
                foreach (string tmp in inventoryDetRefNoList)
                {
                    InventoryDet id = (InventoryDet)InvDet.Find(tmp);
                    if (id != null)
                        InvDet.Remove(id);
                }

                if (InvHdr.MovementType == InventoryController.InventoryMovementType_TransferIn)
                {
                    InvHdr.Remark = "Transfer In Before Stock Take";
                }
                else
                {
                    if (IsAdjustment)
                        InvHdr.Remark = "Adjustment In Before Stock Take";
                    else
                        InvHdr.Remark = "Goods Receive Before Stock Take";
                }

                if (CreateStockOutQueryCommand(username, StockOutReasonID, InventoryLocationID,
                    true, deductRemainingQty, out status, out cmd))
                {
                    SubSonic.DataService.ExecuteTransaction(cmd);
                    InvHdr.IsNew = false;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;
                return false;
            }
        }

        public bool StockInAdjustment(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, string itemList, out string status)
        {
            status = "";
            InventoryDetCollection tmpDet = InvDet;
            try
            {
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                //String[] tmpItemNoList = itemList.Split(',');

                bool isFound = false;
                ArrayList inventoryDetRefNoList = new ArrayList();

                foreach (InventoryDet id in InvDet)
                {
                    //Logger.writeLog("Check InventoryDetRefNo : " + id.InventoryDetRefNo + "," + id.ItemNo);
                    if (itemList.Contains(id.ItemNo))
                    {
                        isFound = true;
                    }
                    if (isFound)
                    {
                        inventoryDetRefNoList.Add(id.InventoryDetRefNo);
                        //Logger.writeLog("Remove InventoryDetRefNo : " + id.InventoryDetRefNo);
                    }

                }
                foreach (string tmp in inventoryDetRefNoList)
                {
                    InventoryDet id = (InventoryDet)InvDet.Find(tmp);
                    if (id != null)
                        InvDet.Remove(id);
                }

                if (InvHdr.MovementType == InventoryController.InventoryMovementType_TransferOut)
                {
                    InvHdr.Remark = "Transfer Out Before Stock Take";
                }
                else
                {
                    if (IsAdjustment)
                        InvHdr.Remark = "Adjustment Out Before Stock Take";
                    else
                        InvHdr.Remark = "Stock Out Before Stock Take";
                }

                QueryCommandCollection cmd = CreateStockInQueryCommand(username, InventoryLocationID, true, CalculateCOGS);
                SubSonic.DataService.ExecuteTransaction(cmd);
                InvHdr.IsNew = false;
                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;
                return false;
            }
        }

        public bool StockInAdjustmentCommand(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, string itemList, out QueryCommandCollection cmd, out string status)
        {
            status = "";
            InventoryDetCollection tmpDet = InvDet;
            cmd = new QueryCommandCollection();
            try
            {
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
                //String[] tmpItemNoList = itemList.Split(',');

                bool isFound = false;
                ArrayList inventoryDetRefNoList = new ArrayList();

                foreach (InventoryDet id in InvDet)
                {
                    //Logger.writeLog("Check InventoryDetRefNo : " + id.InventoryDetRefNo + "," + id.ItemNo);
                    if (itemList.Contains(id.ItemNo))
                    {
                        isFound = true;
                    }
                    if (isFound)
                    {
                        inventoryDetRefNoList.Add(id.InventoryDetRefNo);
                        //Logger.writeLog("Remove InventoryDetRefNo : " + id.InventoryDetRefNo);
                    }

                }
                foreach (string tmp in inventoryDetRefNoList)
                {
                    InventoryDet id = (InventoryDet)InvDet.Find(tmp);
                    if (id != null)
                        InvDet.Remove(id);
                }

                if (InvHdr.MovementType == InventoryController.InventoryMovementType_TransferOut)
                {
                    InvHdr.Remark = "Transfer Out Before Stock Take";
                }
                else
                {
                    if (IsAdjustment)
                        InvHdr.Remark = "Adjustment Out Before Stock Take";
                    else
                        InvHdr.Remark = "Stock Out Before Stock Take";
                }

                cmd = CreateStockInQueryCommand(username, InventoryLocationID, true, CalculateCOGS);
                
                InvHdr.IsNew = false;
                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message;
                InvDet = tmpDet;
                return false;
            }
        }

        public QueryCommandCollection StockOutCommandServer(string username, int StockOutReasonID, int InventoryLocationID, int PointOfSaleID, int refNoAddition, out string status, out decimal totalCostOfGoods)
        {
            QueryCommandCollection qmc = new QueryCommandCollection();
            totalCostOfGoods = 0;
            try
            {
                if (InvHdr == null | InvDet == null)
                {
                    status = "Invalid Inventory data. Unable to receive Inventory.";
                }
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = InventoryLocationID;
                InvHdr.UniqueID = Guid.NewGuid();
                InvHdr.DepartmentID = PointOfSaleInfo.DepartmentID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_StockOut;
                InvHdr.StockOutReasonID = StockOutReasonID;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID, refNoAddition);

                qmc.Add(InvHdr.GetInsertCommand(username));

                //fetch the FIFO information from corresponding Stock In Inventory Details
                int i = 0;
                while (i < InvDet.Count) //For every inventory Details....
                {
                    InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    if (InvDet[i].Item.IsInInventory)
                    {
                        InvDet[i].FactoryPrice = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InventoryLocationID);
                        InvDet[i].CostOfGoods = InvDet[i].FactoryPrice * Convert.ToDecimal(InvDet[i].Quantity);
                        qmc.Add(InvDet[i].GetInsertCommand(username));
                        i += 1;
                    }
                    else
                    {
                        InvDet[i].CostOfGoods = InvDet[i].Item.FactoryPrice;
                        InvDet[i].StockInRefNo = "";
                        qmc.Add(InvDet[i].GetInsertCommand(username));
                        i += 1;
                    }
                }

                foreach (var theDet in InvDet)
                {
                    decimal cogs = 0;
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(
                        theDet.ItemNo, InventoryLocationID, InvHdr.MovementType,
                        theDet.FactoryPrice, Convert.ToDouble(theDet.Quantity), theDet.InventoryDetRefNo, InvHdr.InventoryDate, out cogs);
                    totalCostOfGoods += cogs;

                    qmc.AddRange(itemSummaryCmd);
                }
                status = "";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = ex.Message;
            }
            return qmc;
        }

        public QueryCommandCollection StockInCommandServer(string username, int InventoryLocationID, int PointOfSaleID, bool CalculateCOGS, int refNoAddition, out string status)
        {
            status = "";
            QueryCommandCollection qmc = new QueryCommandCollection();

            try
            {
                //Check whether it is a valid order
                if (InvHdr == null | InvDet == null) //Valid Order?
                {
                    throw new Exception("Invalid Inventory data. Unable to receive Inventory.");
                }

                //Set header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = InventoryLocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_StockIn;

                if (InvHdr.IsNew)
                {
                    InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID, refNoAddition);
                    InvHdr.DepartmentID = new PointOfSale(PointOfSaleID).DepartmentID;
                    InvHdr.Discount = InvHdr.Discount * (decimal)InvHdr.ExchangeRate;
                    InvHdr.UniqueID = Guid.NewGuid();
                    qmc.Add(InvHdr.GetInsertCommand(username));
                }
                else
                {
                    qmc.Add(InvHdr.GetUpdateCommand(username));
                }

                if (!InvHdr.FreightCharge.HasValue)
                    InvHdr.FreightCharge = 0;

                //Inventory Detail - insert into Transaction
                for (int i = 0; i < InvDet.Count; i++)
                {
                    if (InvDet[i].Quantity > 0)
                    {
                        InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                        InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                        InvDet[i].RemainingQty = 0;
                        InvDet[i].Gst = 0;
                        InvDet[i].CostOfGoods = InvDet[i].FactoryPrice * Convert.ToDecimal(InvDet[i].Quantity);
                        if (InvDet[i].IsNew)
                        {
                            InvDet[i].UniqueID = Guid.NewGuid();
                            qmc.Add(InvDet[i].GetInsertCommand(UserInfo.username));
                        }
                        else
                        {
                            qmc.Add(InvDet[i].GetUpdateCommand(UserInfo.username));
                        }
                    }
                }

                foreach (var theDet in InvDet)
                {
                    var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(
                        theDet.ItemNo, InventoryLocationID, InvHdr.MovementType,
                        theDet.FactoryPrice, Convert.ToDouble(theDet.Quantity), theDet.InventoryDetRefNo, InvHdr.InventoryDate);
                    qmc.AddRange(itemSummaryCmd);
                }
                //DataService.ExecuteTransaction(qmc);
                status = "";
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                status = "Error : " + ex.Message;
            }

            return qmc;
        }

        #endregion

        #region -= Stock Return =-
        public bool StockReturn(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, out string status)
        {
            return StockReturn(username, StockOutReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS, false, 0, out status);
        }

        public bool StockReturn(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS, bool isReturnToWarehouse, int WarehouseID, out string status)
        {
            try
            {
                SupplierItemMap.CreateSupplierItemMapTable();
                List<string> ListInventoryHdr = new List<string>();

                QueryCommandCollection cmd = CreateStockReturnQueryCommand(username, StockOutReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS);
                
                    
                // Do "Return In" for the warehouse
                if (WarehouseID != 0)
                {
                    InventoryController ctrl = new InventoryController();
                    ctrl.ActiveCostingType = this.ActiveCostingType;
                    ctrl.SetInventoryHeaderInfo(InvHdr.PurchaseOrderNo, "", "Return Approval: " + InvHdr.PurchaseOrderNo, 0, 0, 0);
                    ctrl.SetVendorInvoiceNo(InvHdr.InvoiceNo);

                    foreach (var poDet in InvDet)
                    {
                        if (poDet.Quantity.GetValueOrDefault(0) <= 0) continue;

                        if (!ctrl.AddItemIntoInventoryStockIn(poDet.ItemNo, poDet.Quantity.GetValueOrDefault(0), poDet.FactoryPrice, out status))
                            throw new Exception(status);
                    }
                    
                    cmd.AddRange(ctrl.CreateReturnInQueryCommand(UserInfo.username, WarehouseID, false, true));
                    
                }
                else {
                    if (isReturnToWarehouse)
                    {
                        foreach (var poDet in InvDet)
                        {
                            int? supplierID = ItemSupplierMapController.GetPreferredSupplier(poDet.ItemNo);
                            if (!supplierID.HasValue) continue;

                            Supplier s = new Supplier(supplierID.Value);
                            if (!s.IsWarehouse.GetValueOrDefault(false) || !s.WarehouseID.HasValue) continue;

                            int invLocID = s.WarehouseID.Value;

                            InventoryController ctrl = new InventoryController();
                            ctrl.ActiveCostingType = this.ActiveCostingType;
                            ctrl.SetInventoryHeaderInfo(InvHdr.PurchaseOrderNo, "", "Return Approval: " + InvHdr.PurchaseOrderNo, 0, 0, 0);
                            ctrl.SetVendorInvoiceNo(InvHdr.InvoiceNo);

                            if (poDet.Quantity.GetValueOrDefault(0) <= 0) continue;
                            ctrl.AddItemIntoInventoryStockIn(poDet.ItemNo, poDet.Quantity.GetValueOrDefault(0), poDet.FactoryPrice, out status);

                            if (status == "")
                            {
                                cmd.AddRange(ctrl.CreateReturnInQueryCommand(UserInfo.username, invLocID, false, true));
                            }
                        }
                    }
                }

                foreach (var cm in cmd)
                {
                    Logger.writeLog("exec: " + cm.CommandSql);
                    SubSonic.DataService.ExecuteQuery(cm);
                }

                //SubSonic.DataService.ExecuteTransaction(cmd);

                Logger.writeLog("Stock Return completed. InventoryHdrRefNo : " + InvHdr.InventoryHdrRefNo);

                InvHdr.IsNew = false;
                status = "";

                // check if there are any stock take after the stock in 
                string itemlist = "";
                if (gotStockTakeAfter(out itemlist))
                {
                    StockOutAdjustment(username, 1, InventoryLocationID, IsAdjustment, true, itemlist, out status);
                }

                return true;
            }
            catch (Exception ex)
            {
                //log into logger
                Logger.writeLog(ex);
                status = ex.Message.Replace("(warning)", "").Replace("(error)", "");
                return false;
            }
        }

        public QueryCommandCollection CreateStockReturnQueryCommand(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
           
            return StockReturn_FIFO(username, StockOutReasonID, InventoryLocationID, IsAdjustment, CalculateCOGS);
            
        }
        private QueryCommandCollection StockReturn_FIFO(string username, int StockOutReasonID, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null) //Valid Order?
                throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
            #endregion

            bool AllowDeductInvQtyNotSuffice = AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.GoodsOrdering.AllowDeductInvQtyNotSufficient),false);

            string status = "";

            #region *) Validation: All quantity must be bigger than 0
            for (int i = 0; i < InvDet.Count; i++)
            {
                if (InvDet[i].Quantity <= 0)
                    throw new Exception("(error)Error: Quantity must be larger than zero.");

                decimal StockOnhand = GetStockBalanceQtyByItemSummaryByDate(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0), DateTime.Now, out status);
                if (!AllowDeductInvQtyNotSuffice && InvDet[i].Quantity > StockOnhand)
                {
                    throw new Exception(string.Format("(error)Error: Balance Quantity of {0} is {1}. Quantity of Stock Return can not be more than Balance Quantity", InvDet[i].ItemNo, StockOnhand.ToString("N2")));
                }
            }
            #endregion

            #region *) Conditioning: Set header information
            if (InvHdr.UserName == null) InvHdr.UserName = username;
            InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);

            InvHdr.UserName = username;
            InvHdr.InventoryLocationID = InventoryLocationID;
            InvHdr.StockOutReasonID = StockOutReasonID;

            if (!InvHdr.FreightCharge.HasValue) InvHdr.FreightCharge = 0;
            
            InvHdr.MovementType = InventoryController.InventoryMovementType_ReturnOut;
            
            #endregion

            #region *) Validation: check if record already exists
            if (IsRecordExisted(InvHdr.MovementType))
                throw new Exception("(error) Record already exists.");
            #endregion

            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;
            #region *) Save: Generate Save Script for InventoryHdr
            if (InvHdr.IsNew)
            {
                mycmd = InvHdr.GetInsertCommand(UserInfo.username);
            }
            else
            {
                mycmd = InvHdr.GetUpdateCommand(UserInfo.username);
            }
            cmd.Add(mycmd);
            #endregion

            //Inventory Detail - insert into Transaction
            for (int i = 0; i < InvDet.Count; i++)
            {
                #region *) Conditioning: Set detail information
                //decimal weight = (InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0)) / SumOfFactoryPriceAndQty;

                InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                InvDet[i].RemainingQty = InvDet[i].Quantity.GetValueOrDefault(0);

                if (CalculateCOGS && InvDet[i].Quantity > 0)
                {
                    InvDet[i].InitialFactoryPrice = InvDet[i].FactoryPrice;
                    InvDet[i].CostOfGoods = InvDet[i].FactoryPrice;
                }

                InvDet[i].Gst = GetGST();
                #endregion

                #region *) Save: Generate Save Script for InventoryDet
                if (InvDet[i].IsNew)
                {
                    mycmd = InvDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                else
                {
                    mycmd = InvDet[i].GetUpdateCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                #endregion

                #region *) Insert (If Not Exists) SupplierItemMap for this SupplierID and ItemNo

                if (IsAdjustment == false && InvHdr.Supplier != string.Empty) // only if Supplier is not string.empty (--select supplier-- means string empty)
                {
                    SupplierItemMap supItemMap = new SupplierItemMap();
                    supItemMap.SupplierID = int.Parse(InvHdr.Supplier);
                    supItemMap.ItemNo = InvDet[i].ItemNo;
                    supItemMap.UniqueID = Guid.NewGuid();
                    supItemMap.Deleted = false;

                    cmd.Add(supItemMap.GetInsertIfNotExistsCommand());
                }

                #endregion
            }

            #region *) Update Item Summary

            foreach (var id in InvDet)
            {
                var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                    InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                    id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                    id.InventoryDetRefNo, InvHdr.InventoryDate);
                cmd.AddRange(itemSummaryCmd);
            }

            #endregion

            return cmd;
        }

        #endregion


        #region -= Return In =-
        public QueryCommandCollection CreateReturnInQueryCommand(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {

            return ReturnIn_FIFO(username, InventoryLocationID, IsAdjustment, CalculateCOGS);

        }

        private QueryCommandCollection ReturnIn_FIFO(string username, int InventoryLocationID, bool IsAdjustment, bool CalculateCOGS)
        {
            #region *) Validation: Check whether it is a valid order
            if (InvHdr == null | InvDet == null) //Valid Order?
                throw new Exception("(error)Invalid Inventory data. Unable to receive Inventory.");
            #endregion

            #region *) Validation: All quantity must be bigger than 0
            for (int i = 0; i < InvDet.Count; i++)
                if (InvDet[i].Quantity <= 0)
                    throw new Exception("(error)Error: Quantity must be larger than zero.");
            #endregion

            #region *) Conditioning: Set header information
            if (InvHdr.UserName == null) InvHdr.UserName = username;
            if (InvHdr.InventoryHdrRefNo == null || !InvHdr.InventoryHdrRefNo.StartsWith("ST"))
            {
                if (InvHdr.InventoryHdrRefNo == null || InvHdr.UserName.ToLower() != "system")
                    InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(InventoryLocationID);
            }

            InvHdr.UserName = username;
            InvHdr.InventoryLocationID = InventoryLocationID;

            if (!InvHdr.FreightCharge.HasValue) InvHdr.FreightCharge = 0;

            InvHdr.MovementType = InventoryController.InventoryMovementType_ReturnIn;
            InvHdr.CustomRefNo = InvHdr.InventoryHdrRefNo;

            #endregion

            #region *) Validation: check if record already exists
            if (IsRecordExisted(InvHdr.MovementType))
                throw new Exception("(error) Record already exists.");
            #endregion

            QueryCommandCollection cmd = new QueryCommandCollection();
            QueryCommand mycmd;
            #region *) Save: Generate Save Script for InventoryHdr
            if (InvHdr.IsNew)
            {
                mycmd = InvHdr.GetInsertCommand(UserInfo.username);
            }
            else
            {
                mycmd = InvHdr.GetUpdateCommand(UserInfo.username);
            }
            cmd.Add(mycmd);
            #endregion


            //Inventory Detail - insert into Transaction
            for (int i = 0; i < InvDet.Count; i++)
            {
                #region *) Conditioning: Set detail information
                //decimal weight = (InvDet[i].FactoryPrice * InvDet[i].Quantity.GetValueOrDefault(0)) / SumOfFactoryPriceAndQty;

                InvDet[i].InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                InvDet[i].InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                InvDet[i].RemainingQty = InvDet[i].Quantity.GetValueOrDefault(0);

                if (IsAdjustment)
                {
                    InvDet[i].FactoryPrice = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                    InvDet[i].CostOfGoods = ItemSummaryController.GetAvgCostPrice(InvDet[i].ItemNo, InvHdr.InventoryLocationID.GetValueOrDefault(0));
                }

                InvDet[i].Gst = GetGST();
                #endregion

                #region *) Save: Generate Save Script for InventoryDet
                if (InvDet[i].IsNew)
                {
                    mycmd = InvDet[i].GetInsertCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                else
                {
                    mycmd = InvDet[i].GetUpdateCommand(UserInfo.username);
                    cmd.Add(mycmd);
                }
                #endregion

                #region *) Insert (If Not Exists) SupplierItemMap for this SupplierID and ItemNo

                if (IsAdjustment == false && InvHdr.Supplier != string.Empty) // only if Supplier is not string.empty (--select supplier-- means string empty)
                {
                    SupplierItemMap supItemMap = new SupplierItemMap();
                    supItemMap.SupplierID = int.Parse(InvHdr.Supplier);
                    supItemMap.ItemNo = InvDet[i].ItemNo;
                    supItemMap.UniqueID = Guid.NewGuid();
                    supItemMap.Deleted = false;

                    cmd.Add(supItemMap.GetInsertIfNotExistsCommand());
                }

                #endregion

            }

            #region *) Update Item Summary

            foreach (var id in InvDet)
            {
                var itemSummaryCmd = ItemSummaryController.FetchItemSummaryUpdate(id.ItemNo,
                    InvHdr.InventoryLocationID.GetValueOrDefault(0), InvHdr.MovementType,
                    id.FactoryPrice, Convert.ToDouble(id.Quantity.GetValueOrDefault(0)),
                    id.InventoryDetRefNo, InvHdr.InventoryDate);
                cmd.AddRange(itemSummaryCmd);
            }
			
            var cmdItemTag = ItemTagController.FetchItemTagUpdate(InvHdr, InvDet);
            if (cmdItemTag.Count > 0)
                cmd.AddRange(cmdItemTag);

            #endregion

            return cmd;
        }
        #endregion

        #region -= Checking if InventoryDate < Stock Take Date =-
        public bool gotStockTakeAfter(out string ItemList)
        {
            bool result = false;
            ItemList = "";
            try
            {
                foreach (InventoryDet id in InvDet)
                {
                    StockTakeCollection stc = new StockTakeCollection();
                    stc.Where(StockTake.Columns.ItemNo, id.ItemNo);
                    stc.Where(StockTake.Columns.InventoryLocationID, InvHdr.InventoryLocationID);
                    stc.Where(StockTake.Columns.StockTakeDate, Comparison.GreaterThan, InvHdr.InventoryDate);
                    stc.Where(StockTake.Columns.IsAdjusted, true);
                    stc.Load();
                    if (stc.Count > 0)
                    {
                        result = true;
                    }
                    else
                    {
                        ItemList += (id.ItemNo + ",");
                    }

                    //Logger.writeLog("ItemNo that have no Stock Take : " + ItemList);
                }
                if (ItemList.Length > 0)
                {
                    ItemList = ItemList.Substring(0, ItemList.Length - 1);
                }
            }
            catch
            (Exception ex)
            {
                Logger.writeLog("Error checking Stock Take After Goods Receive : " + ex.Message);
                result = false;
            }

            return result;
        }
        #endregion

        public void SetInventoryHdrRefNo(string refNo)
        {
            InvHdr.InventoryHdrRefNo = refNo;
        }

        public void SetIsNew(bool val)
        {
            InvHdr.IsNew = val;
        }

        public void SetCustomRefNo(string customRefNo)
        {
            InvHdr.CustomRefNo = customRefNo;
        }

        public void SetGSTRule(int GSTRule)
        {
            InvHdr.GSTRule = GSTRule;
        }


        private static DataRow FetchFIFOCostOfGoods(string ItemNo, int InventoryLocationID, out string status)
        {
            DataSet ds0 = SPs.FetchCostOfGoods(ItemNo, InventoryLocationID).GetDataSet();

            status = "";
            if (ds0 != null && ds0.Tables.Count != 0 && ds0.Tables[0].Rows.Count != 0)
            {
                return ds0.Tables[0].Rows[0];
            }
            status = "ERROR";
            return null;
        }
        private decimal FetchFIFOCostOfGoodsToHandleNegativeQty(string ItemNo, int InventoryLocationID)
        {
            // -- Fetch previos transaction history --
            DataSet ds0;
            ds0 = SPs.FetchCostOfGoodsToHandleNegativeQty(ItemNo, InventoryController.InventoryMovementType_StockIn).GetDataSet();


            if (ds0 != null && ds0.Tables.Count != 0 && ds0.Tables[0].Rows.Count != 0)
            {
                return decimal.Parse(ds0.Tables[0].Rows[0]["CostOfGoods"].ToString());

            }
            else
            {
                return 0;
            }
        }

        private bool IsQuantitySufficient(int LocationID, out InventoryDetCollection mergedTmpDet, out string status)
        {
            mergedTmpDet = new InventoryDetCollection();

            MergeInventoryDet(ref mergedTmpDet);

            for (int i = 0; i < mergedTmpDet.Count; i++)
            {
                if (!AppSetting.CastBool(AppSetting.GetSetting(AppSetting.SettingsName.Inventory.AllowStockTransferEvenStockIsZero), false))
                {
                    decimal bal = InventoryController.GetStockBalanceQtyByItemSummaryByDate
                        (mergedTmpDet[i].ItemNo, LocationID, InvHdr.InventoryDate, out status);
                    if (mergedTmpDet[i].Quantity > bal)
                    {
                        status = "Insufficient quantity to perform stock out for item " +
                            mergedTmpDet[i].ItemNo + " (" + mergedTmpDet[i].Item.ItemName +
                            "). Onhand quantity is " + bal + " while stock out quantity is " +
                            mergedTmpDet[i].Quantity + ".";
                        return false;
                    }
                }
            }
            status = "";
            return true;
        }

        private void MergeInventoryDet(ref InventoryDetCollection mergedTmpDet)
        {
            InvDet.CopyTo(mergedTmpDet);
            mergedTmpDet.Sort(InventoryDet.Columns.ItemNo, true);
            int j = mergedTmpDet.Count - 1;
            while (j > 0)
            {
                if (mergedTmpDet[j].ItemNo == mergedTmpDet[j - 1].ItemNo)
                {
                    //same -- merge to the older one
                    mergedTmpDet[j - 1].Quantity += mergedTmpDet[j].Quantity;
                    mergedTmpDet.RemoveAt(j);
                }
                j--;
            }
        }

        private void DistributeInventoryDetQuantity
            (ref QueryCommandCollection cmd,
            InventoryDet myInvDet,
            ref int index, out string status)
        {
            try
            {
                status = "";
                InventoryDet tmpDet;

                //Pull out remaining quantity for an item
                /*
                DataSet ds = SPs.FetchRemainingQuantityOfAnItem
                    (myInvDet.ItemNo, (int)InvHdr.InventoryLocationID).GetDataSet();
                 */
                DataSet ds;

                ds = ExtractRemainingQuantity(myInvDet);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    status = "Attempting to pull stock balance quantity for deduction, however the quantity not found. ItemNo = " + myInvDet.ItemNo + ", LocationID: " + (int)InvHdr.InventoryLocationID;
                    Logger.writeLog(status);
                    return;
                }

                //Quantity that we want to stock out
                decimal deductionQuantity = myInvDet.Quantity.GetValueOrDefault(0);
                bool breakLoop = false;
                //loop through and create the new inventory det 
                //that will take qty from those stuff
                //using insert sql command
                //until enough quantity to be deducted
                int BalBefore = 0; // myInvDet.BalanceBefore;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    tmpDet = new InventoryDet();
                    tmpDet = myInvDet.Clone();
                    tmpDet.InventoryDetRefNo = myInvDet.InventoryHdrRefNo + "." + (index).ToString();
                    tmpDet.UniqueID = Guid.NewGuid();
                    //tmpDet.BalanceBefore = BalBefore;

                    Query qry = new Query("InventoryDet");
                    decimal remainingQty = ds.Tables[0].Rows[i]["RemainingQty"].ToString().GetDecimalValue();
                    tmpDet.CostOfGoods = (decimal)ds.Tables[0].Rows[i]["CostOfGoods"];
                    tmpDet.StockInRefNo = ds.Tables[0].Rows[i]["InventoryDetRefNo"].ToString();
                    if (deductionQuantity > remainingQty)
                    {
                        tmpDet.Quantity = remainingQty;
                        qry.AddUpdateSetting(InventoryDet.Columns.RemainingQty, 0);
                        deductionQuantity -= remainingQty;
                    }
                    else
                    {
                        tmpDet.Quantity = deductionQuantity;
                        qry.AddUpdateSetting
                            (InventoryDet.Columns.RemainingQty, remainingQty - deductionQuantity);
                        breakLoop = true;
                    }
                    //tmpDet.BalanceAfter = BalBefore - tmpDet.Quantity;
                    //BalBefore = tmpDet.BalanceAfter;

                    //Create the insert command                
                    //Logger.writeLog("Create Create for " + tmpDet.ItemNo + " Qty:" + tmpDet.Quantity.ToString() + " ID:" + tmpDet.InventoryDetRefNo);
                    cmd.Add(tmpDet.GetInsertCommand(UserInfo.username));
                    index += 1;

                    //Update remaining Quantity
                    qry.AddWhere
                        (InventoryDet.Columns.InventoryDetRefNo,
                        ds.Tables[0].Rows[i]["InventoryDetRefNo"].ToString());
                    cmd.Add(qry.BuildUpdateCommand());

                    if (breakLoop) break;
                }
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                throw ex;
            }
        }

        private DataSet ExtractRemainingQuantity(InventoryDet myInvDet)
        {
            try
            {
                object tmpObj;
                DataSet ds;
                string SQL;

                //To create how much balance left to deduct, 
                //based on the most recent used "stock in" or "adjustment in" ref no
                //Get how many quantity of this most recent used stock in that has been used
                //That quantity, add up with all the "stock in" and "inventory in" that has never been used
                //that is how much remaining quantity in the inventory that has not been deducted

                SQL = "select top 1 isnull(StockInRefNo,'') " +
                        "from inventoryhdr a inner join " +
                        "inventorydet b " +
                        "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                        "where MovementType like '% Out' and Not isnull(stockinrefno,'') = ''  " +
                        "and ItemNo = '" + myInvDet.ItemNo + "' and not MovementType = 'Transfer In' " +
                        " and inventorylocationID = " + InvHdr.InventoryLocationID.ToString() + " " +
                        "order by InventoryDate desc, b.InventoryDetRefNo desc; ";

                string YoungestStockInRefNo = "";
                tmpObj = DataService.ExecuteScalar(new QueryCommand(SQL, "PowerPOS"));
                if (tmpObj != null) YoungestStockInRefNo = tmpObj.ToString();
                int usedQty = 0;
                DateTime LatestUsedStockIn = new DateTime(1945, 8, 17);
                if (YoungestStockInRefNo != "")
                {
                    //Get the sum of quantity that already been used
                    SQL = "select isnull(SUM(quantity),0) from " +
                            "inventoryhdr a inner join " +
                            "inventorydet b  " +
                            "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                            "where StockInRefNo = '" + YoungestStockInRefNo + "'; ";
                    tmpObj = DataService.ExecuteScalar(new QueryCommand(SQL, "PowerPOS"));
                    if (tmpObj != null && tmpObj is int)
                    {
                        usedQty = (int)tmpObj;
                    }
                    SQL = "select inventorydate from " +
                            "inventoryhdr a inner join " +
                            "inventorydet b  " +
                            "on a.inventoryhdrrefno = b.inventoryhdrrefno " +
                            "where InventoryDetRefNo = '" + YoungestStockInRefNo + "'; ";
                    tmpObj = DataService.ExecuteScalar(new QueryCommand(SQL, "PowerPOS"));
                    if (tmpObj != null && tmpObj is DateTime)
                    {
                        LatestUsedStockIn = (DateTime)tmpObj;
                    }
                }
                //pull out all stock in that is equal or younger that  latest used stock in date time
                SQL = "select * from inventoryhdr a inner join inventorydet b on " +
                        "a.inventoryhdrrefno = b.inventoryhdrrefno inner join item c " +
                        "on b.itemno = c.itemno where c.itemno = '" + myInvDet.ItemNo +
                        "' and inventorydate >= '" + LatestUsedStockIn.ToString("yyyy-MM-dd HH:mm:ss") + "' and movementtype like '%In' and inventorylocationid = " +
                        InvHdr.InventoryLocationID.ToString() + " order by inventorydate asc ";
                ds = DataService.GetDataSet(new QueryCommand(SQL, "PowerPOS"));
                if (ds != null)
                {
                    //for loop and minus of the remaining quantity
                    for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        ds.Tables[0].Rows[i]["RemainingQty"] = ds.Tables[0].Rows[i]["Quantity"];
                        if (ds.Tables[0].Rows[i]["InventoryDetRefNo"].ToString() == YoungestStockInRefNo)
                        {
                            if (ds.Tables[0].Rows[i]["RemainingQty"].ToString().GetDecimalValue() >= usedQty)
                            {
                                ds.Tables[0].Rows[i]["RemainingQty"] =
                                    ds.Tables[0].Rows[i]["RemainingQty"].ToString().GetDecimalValue() - usedQty;

                                if (ds.Tables[0].Rows[i]["RemainingQty"].ToString().GetDecimalValue() == 0)
                                {
                                    ds.Tables[0].Rows.RemoveAt(i);
                                }
                            }
                            else
                            {
                                Logger.writeLog("Exception:Wierd error found, more issue than stock receive for this product!");
                                ds.Tables[0].Rows[i]["RemainingQty"] = 0;
                            }
                        }
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return null;
            }
        }
        #endregion

        #region "Stock Out Discrepancy"

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public ViewInventoryActivityCollection FetchInventoryStockOutDiscrepancy(int LocationID)
        {
            ViewInventoryActivityCollection invDet = new ViewInventoryActivityCollection();
            return invDet.Where(ViewInventoryActivity.Columns.InventoryLocationID, LocationID)
                    .Where(ViewInventoryActivity.Columns.IsDiscrepancy, true)
                    .Load();
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void UpdateInventoryDiscrepancyCostOfGoods(string InventoryDetRefNo, decimal CostOfGoods)
        {

            Query invDet = InventoryDet.CreateQuery();
            invDet.QueryType = QueryType.Update;
            invDet.AddUpdateSetting(InventoryDet.Columns.CostOfGoods, CostOfGoods);
            invDet.AddWhere("InventoryDetRefNo", InventoryDetRefNo);
            invDet.Execute();
        }

        public bool CorrectStockOutDiscrepancy(string username, int LocationID)
        {
            try
            {

                ViewInventoryActivityCollection viewInvDet = new ViewInventoryActivityCollection();
                viewInvDet = viewInvDet.Where(ViewInventoryActivity.Columns.InventoryLocationID, LocationID).Where(ViewInventoryActivity.Columns.IsDiscrepancy, true).Load();

                Query qryInvDet;
                QueryCommandCollection cmd = new QueryCommandCollection();

                //Set header information
                InvHdr.UserName = username;
                InvHdr.InventoryLocationID = LocationID;
                InvHdr.MovementType = InventoryController.InventoryMovementType_AdjustmentIn;
                InvHdr.InventoryHdrRefNo = InventoryController.getNewInventoryRefNo(LocationID);
                cmd.Add(InvHdr.GetInsertCommand(username));
                InventoryDet tmpInvDet;
                for (int i = 0; i < viewInvDet.Count; i++)
                {
                    //Create Stock In  to match the stock out....                    
                    //Create the Inventory Details....
                    tmpInvDet = new InventoryDet();

                    tmpInvDet.InventoryHdrRefNo = InvHdr.InventoryHdrRefNo;
                    tmpInvDet.InventoryDetRefNo = InvHdr.InventoryHdrRefNo + "." + (i + 1).ToString();
                    tmpInvDet.RemainingQty = 0;
                    tmpInvDet.IsDiscrepancy = false;

                    tmpInvDet.ItemNo = viewInvDet[i].ItemNo;
                    tmpInvDet.Discount = 0;
                    tmpInvDet.Remark = "Adjustment for " + viewInvDet[i].InventoryDetRefNo;
                    tmpInvDet.FactoryPrice = tmpInvDet.Item.FactoryPrice;
                    tmpInvDet.Quantity = viewInvDet[i].Quantity;

                    //CALCULATE Cost of Goods here                    
                    tmpInvDet.CostOfGoods = viewInvDet[i].CostOfGoods;
                    cmd.Add(tmpInvDet.GetInsertCommand(UserInfo.username));

                    //Set as non discrepancy
                    qryInvDet = InventoryDet.CreateQuery();
                    qryInvDet.QueryType = QueryType.Update;
                    qryInvDet.AddUpdateSetting(InventoryDet.Columns.IsDiscrepancy, false);
                    qryInvDet.AddUpdateSetting(InventoryDet.Columns.StockInRefNo, tmpInvDet.InventoryDetRefNo);
                    qryInvDet.AddWhere("IsDiscrepancy", true);
                    qryInvDet.AddWhere("InventoryDetRefNo", viewInvDet[i].InventoryDetRefNo);
                    cmd.Add(qryInvDet.BuildUpdateCommand());
                }
                SubSonic.DataService.ExecuteTransaction(cmd);
                return true;
            }
            catch (Exception ex)
            {
                Logger.writeLog(ex);
                return false;
            }
        }

        #endregion

    }
}
